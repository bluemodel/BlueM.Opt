' Copyright (c) BlueM Dev Group
' Website: http://bluemodel.org
' 
' All rights reserved.
' 
' Released under the BSD-2-Clause License:
' 
' Redistribution and use in source and binary forms, with or without modification, 
' are permitted provided that the following conditions are met:
' 
' * Redistributions of source code must retain the above copyright notice, this list 
'   of conditions and the following disclaimer.
' * Redistributions in binary form must reproduce the above copyright notice, this list 
'   of conditions and the following disclaimer in the documentation and/or other materials 
'   provided with the distribution.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
' EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
' OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
' SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT 
' OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
' HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
' TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
' EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'--------------------------------------------------------------------------------------------
'
'*******************************************************************************
'*******************************************************************************
'**** BlueM.Opt                                                             ****
'****                                                                       ****
'**** Christoph Huebner, Felix Froehlich, Dirk Muschalla, Dominik Kerber    ****
'**** Frank Reu�ner                                                         ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'****                                                                       ****
'*******************************************************************************
'*******************************************************************************

Option Strict Off 'allows permissive type semantics. explicit narrowing conversions are not required 

Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports IHWB.EVO.Common.Constants
Imports BlueM



''' <summary>
''' Main Window
''' </summary>
Partial Public Class Form1
    Inherits System.Windows.Forms.Form

#Region "Eigenschaften"

    Public BatchCounter As Integer

    ''' <summary>
    ''' Wird im BatchMode ausgel�st, sobald die Settings eingelesen wurden (kurz vor Start)
    ''' </summary>
    Public Event OptimizationStarted()

    ''' <summary>
    ''' Wird im BatchMode ausgel�st, sobald die Optimierung beendet ist
    ''' </summary>
    Public Event OptimizationReady()

    Private IsInitializing As Boolean  'Gibt an, ob das Formular bereits fertig geladen wurde(beim Laden werden s�mtliche Events ausgel�st)

    'Anwendung
    Private Anwendung As String

    'Problem
    Public mProblem As EVO.Common.Problem

    'Settings
    Private mSettings As EVO.Common.Settings

    'Progress
    Private mProgress As EVO.Common.Progress

    'Apps
    Private Testprobleme1 As EVO.Apps.Testprobleme
    Public WithEvents Sim1 As EVO.Apps.Sim

    'Controller
    Private controller As EVO.IController

    'Ablaufkontrolle
    '---------------
    Dim _isrun As Boolean = False
    Dim _ispause As Boolean = False

    ''' <summary>
    ''' Optimierung l�uft
    ''' </summary>
    Private Property isRun() As Boolean
        Get
            Return _isrun
        End Get
        Set(ByVal value As Boolean)
            _isrun = value
            If (Me.isRun) Then
                Me.Button_Start.Text = "Pause"
                Me.Button_Stop.Enabled = True
            Else
                Me.Button_Stop.Enabled = False
                Me.Button_Start.Text = "Start"
            End If
        End Set
    End Property

    ''' <summary>
    ''' Optimierung ist pausiert
    ''' </summary>
    Private Property isPause() As Boolean
        Get
            Return _ispause
        End Get
        Set(ByVal value As Boolean)
            _ispause = value
            If (Me.isPause) Then
                Me.Button_Start.Text = "Continue"
            Else
                Me.Button_Start.Text = "Pause"
            End If
        End Set
    End Property

    'Dialoge
    Private WithEvents solutionDialog As SolutionDialog
    Private WithEvents scatterplot1, scatterplot2 As EVO.Diagramm.Scatterplot

    'Diagramme
    Private WithEvents Hauptdiagramm1 As IHWB.EVO.Diagramm.Hauptdiagramm
    Private WithEvents Monitor1 As EVO.Diagramm.Monitor

#End Region 'Eigenschaften


#Region "f�r MPC Schleifen"
    Public bStartenWiederholen As Boolean
#End Region


#Region "Methoden"

#Region "UI"

    'Form1 laden
    '***********
    Private Sub Form1_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        'XP-look
        System.Windows.Forms.Application.EnableVisualStyles()

        'Monitor zuweisen
        Me.Monitor1 = EVO.Diagramm.Monitor.getInstance()
        'Monitor zentrieren
        Me.Monitor1.Location = New Drawing.Point(Me.Location.X + Me.Width / 2 - Me.Monitor1.Width / 2, Me.Location.Y + Me.Height / 2 - Me.Monitor1.Height / 2)

        'BatchCounter initialisieren
        Me.BatchCounter = 0

        'Formular initialisieren
        Call Me.INI()

        'Handler f�r Klick auf Serien zuweisen
        AddHandler Me.Hauptdiagramm1.ClickSeries, AddressOf seriesClick

        'Ende der Initialisierung
        Me.IsInitializing = False

    End Sub

    ''' <summary>
    ''' Formular zur�cksetzen
    ''' </summary>
    Public Sub INI()

        Me.IsInitializing = True

        'Anwendungs-Groupbox aktivieren
        Me.GroupBox_Anwendung.Enabled = True

        'Anwendung
        '---------
        'Liste der Anwendungen in ComboBox schreiben und Anfangseinstellung w�hlen
        Me.ComboBox_Anwendung.Items.Clear()
        Me.ComboBox_Anwendung.Items.AddRange(New Object() {"", ANW_BLUEM, ANW_SCAN, ANW_SWMM, ANW_TALSIM, ANW_TESTPROBLEME, ANW_TSP}) 'ANW_SMUSI entfernt (Bug 265)
        Me.ComboBox_Anwendung.SelectedIndex = 0

        'Datensatz
        '---------
        Me.Label_Datensatz.Enabled = False
        Me.ComboBox_Datensatz.Enabled = False
        Me.Button_BrowseDatensatz.Enabled = False

        'Methode
        '-------
        Me.Label_Methode.Enabled = False
        Me.ComboBox_Methode.Enabled = False

        'Liste der Methoden in ComboBox schreiben und Anfangseinstellung w�hlen
        Me.ComboBox_Methode.Items.Clear()
        Me.ComboBox_Methode.Items.AddRange(New Object() {"", METH_PES, METH_CES, METH_HYBRID, METH_METAEVO, METH_SENSIPLOT, METH_HOOKEJEEVES, METH_DDS})
        Me.ComboBox_Methode.SelectedIndex = 0

        'Einstellungen
        Me.mSettings = New Common.Settings()
        Me.EVO_Einstellungen1.Reset() 'f�r Neustart wichtig
        Me.EVO_Einstellungen1.setSettings(Me.mSettings)

        'Monitor zur�cksetzen
        Me.Monitor1.Reset()

        'Progress instanzieren und an EVO_Opt_Verlauf �bergeben
        Me.mProgress = New EVO.Common.Progress()
        Me.EVO_Opt_Verlauf1.Initialisieren(Me.mProgress)

        'Toolbar-Buttons deaktivieren
        Me.ToolStripSplitButton_Diagramm.Enabled = False
        Me.ToolStripSplitButton_ErgebnisDB.Enabled = False
        Me.ToolStripButton_Scatterplot.Enabled = False
        Me.ToolStripSplitButton_Settings.Enabled = False
        Me.ToolStripMenuItem_SettingsLoad.Enabled = True 'weil bei vorherigem Start deaktiviert

        'Weitere Buttons
        Me.Button_Start.Enabled = False
        Me.Button_Stop.Enabled = False

        'Diagramm
        Call Me.Hauptdiagramm1.Reset()

        'SolutionDialog
        If (Not IsNothing(Me.solutionDialog)) Then
            Me.solutionDialog.Close()
            Me.solutionDialog = Nothing
        End If

        Me.IsInitializing = False

    End Sub

    ''' <summary>
    ''' Button New geklickt
    ''' </summary>
    Private Sub Button_New_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_New.Click
        'Controller stoppen
        If (Me.STOPPEN()) Then
            'Formular zur�cksetzen
            Call Me.INI()
        End If
    End Sub

    'Monitor anzeigen
    '****************
    Private Sub Button_Monitor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_Monitor.Click

        If (Me.ToolStripButton_Monitor.Checked) Then
            Me.Monitor1.Show()
        Else
            Me.Monitor1.Hide()
        End If

    End Sub

    'Wenn Monitor ge�ffnet/geschlossen wird, ButtonState aktualisieren
    '*****************************************************************
    Private Sub MonitorOpenClose() Handles Monitor1.MonitorClosed, Monitor1.MonitorOpened
        Me.ToolStripButton_Monitor.Checked = Me.Monitor1.Visible
    End Sub

    'About Dialog anzeigen
    '*********************
    Private Sub About(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_About.Click
        Dim about As New AboutBox()
        Call about.ShowDialog(Me)
    End Sub

    'Wiki aufrufen
    '*************
    Private Sub Help(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_Help.Click
        Call Process.Start(HelpURL)
    End Sub

    'Einstellungen-Button hat selbst keine funktionalit�t -> nur DropDown
    '********************************************************************
    Private Sub Button_Einstellungen_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripSplitButton_Settings.ButtonClick
        Call Me.ToolStripSplitButton_Settings.ShowDropDown()
    End Sub

    'EVO_Einstellungen laden
    '***********************
    Private Sub Einstellungen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_SettingsLoad.Click

        'Dialog einrichten
        OpenFileDialog1.Filter = "XML-Dateien (*.xml)|*.xml"
        OpenFileDialog1.FileName = "Settings.xml"
        OpenFileDialog1.Title = "Einstellungsdatei ausw�hlen"
        If (Not IsNothing(Sim1)) Then
            OpenFileDialog1.InitialDirectory = Sim1.WorkDir_Original
        Else
            OpenFileDialog1.InitialDirectory = CurDir()
        End If

        'Dialog anzeigen
        If (OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) Then

            'Settings aus Datei laden
            Call Me.loadSettings(OpenFileDialog1.FileName)

        End If

    End Sub

    'EVO_Einstellungen speichern
    '***************************
    Private Sub Einstellungen_Save(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_SettingsSave.Click

        'Dialog einrichten
        SaveFileDialog1.Filter = "XML-Dateien (*.xml)|*.xml"
        SaveFileDialog1.FileName = "Settings.xml"
        SaveFileDialog1.DefaultExt = "xml"
        SaveFileDialog1.Title = "Einstellungsdatei speichern"
        If (Not IsNothing(Sim1)) Then
            SaveFileDialog1.InitialDirectory = Sim1.WorkDir_Original
        Else
            SaveFileDialog1.InitialDirectory = CurDir()
        End If

        'Dialog anzeigen
        If (SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            Call Me.saveSettings(SaveFileDialog1.FileName)
        End If
    End Sub

#End Region 'UI

#Region "Settings-IO"

    ''' <summary>
    ''' Speichern der Settings in einer XML-Datei
    ''' </summary>
    ''' <param name="filename">Pfad zur XML-Datei</param>
    Public Sub saveSettings(ByVal filename As String)

        Dim writer As StreamWriter
        Dim serializer As XmlSerializer

        'Streamwriter �ffnen
        writer = New StreamWriter(filename)

        serializer = New XmlSerializer(GetType(Common.Settings), New XmlRootAttribute("Settings"))
        serializer.Serialize(writer, Me.mSettings)

        writer.Close()

    End Sub

    'Laden der Settings aus einer XML-Datei
    '**************************************
    Public Sub loadSettings(ByVal filename As String)

        Dim serializer As New XmlSerializer(GetType(Common.Settings))

        AddHandler serializer.UnknownElement, AddressOf serializerUnknownElement
        AddHandler serializer.UnknownAttribute, AddressOf serializerUnknownAttribute

        'Filestream �ffnen
        Dim fs As New FileStream(filename, FileMode.Open)

        Try
            'Deserialisieren
            'TODO: XmlDeserializationEvents ms-help://MS.VSCC.v90/MS.MSDNQTR.v90.en/fxref_system.xml/html/e0657840-5678-bf57-6e7a-1bd93b2b27d1.htm
            Me.mSettings = CType(serializer.Deserialize(fs), Common.Settings)

            'Geladene Settings �berall neu setzen
            Me.EVO_Einstellungen1.setSettings(Me.mSettings)
            Me.Hauptdiagramm1.setSettings(Me.mSettings)
            If (Not IsNothing(Me.Sim1)) Then
                Me.Sim1.setSettings(Me.mSettings)
            End If

        Catch e As Exception
            MsgBox("Fehler beim Einlesen der Einstellungen!" & eol & e.Message, MsgBoxStyle.Exclamation)

        Finally
            fs.Close()

        End Try

    End Sub

    'Fehlerbehandlung Serialisierung
    '*******************************
    Private Sub serializerUnknownElement(ByVal sender As Object, ByVal e As XmlElementEventArgs)
        MsgBox("Fehler beim Einlesen der Einstellungen:" & eol _
            & "Das Element '" & e.Element.Name & "' ist unbekannt!", MsgBoxStyle.Exclamation)
    End Sub

    Private Sub serializerUnknownAttribute(ByVal sender As Object, ByVal e As XmlAttributeEventArgs)
        MsgBox("Fehler beim Einlesen der Einstellungen:" & eol _
            & "Das Attribut '" & e.Attr.Name & "' ist unbekannt!", MsgBoxStyle.Exclamation)
    End Sub

#End Region 'Settings-IO

#Region "Initialisierung der Anwendungen"

    'Die Anwendung wurde ausgew�hlt und wird jetzt initialisiert
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    'Anwendung wurde ausgew�hlt
    '**************************
    Private Sub Combo_App_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Anwendung.SelectedIndexChanged
        If (Not Me.IsInitializing) Then
            Call Me.INI_App(ComboBox_Anwendung.SelectedItem)
        End If
    End Sub

    ''' <summary>
    ''' Anwendung initialisieren
    ''' </summary>
    ''' <param name="selectedAnwendung">zu setzende Anwendung</param>
    Public Sub INI_App(ByVal selectedAnwendung As String)

        Try
            'Falls Anwendung von ausserhalb gesetzt wurde
            Me.IsInitializing = True
            Me.ComboBox_Anwendung.SelectedItem = selectedAnwendung
            Me.IsInitializing = False

            'Diagramm zur�cksetzen
            Call Me.Hauptdiagramm1.Reset()

            'Alles deaktivieren, danach je nach Anwendung aktivieren
            '-------------------------------------------------------

            'Sim1 zerst�ren
            Me.Sim1 = Nothing

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Datensatz-Reset deaktivieren
            Me.MenuItem_DatensatzZur�cksetzen.Enabled = False

            'Methodenauswahl deaktivieren
            Me.Label_Methode.Enabled = False
            Me.ComboBox_Methode.Enabled = False

            'Toolbar-Buttons
            Me.ToolStripMenuItem_ErgebnisDBSave.Enabled = False
            Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = False
            Me.ToolStripButton_Scatterplot.Enabled = False
            Me.ToolStripMenuItem_ErgebnisDBCompare.Enabled = False

            'Multithreading standardm��ig verbieten
            Me.mSettings.General.MultithreadingAllowed = False

            'Mauszeiger busy
            Cursor = Cursors.WaitCursor

            'Ausgew�hlte Anwendung speichern
            Me.Anwendung = selectedAnwendung

            Select Case Me.Anwendung

                Case "" 'Keine Anwendung ausgew�hlt
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'nix

                Case ANW_BLUEM 'Anwendung BlueM
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse BlueM initialisieren
                    Sim1 = New EVO.Apps.BlueM()


                Case ANW_SMUSI 'Anwendung Smusi
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse Smusi initialisieren
                    'Sim1 = New EVO.Apps.Smusi()


                Case ANW_TALSIM 'Anwendung TALSIM
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse Talsim initialisieren
                    Sim1 = New EVO.Apps.Talsim()


                Case ANW_SCAN 'Anwendung S:CAN
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse Scan initialisieren
                    Sim1 = New EVO.Apps.Scan()


                Case ANW_SWMM   'Anwendung SWMM
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse SWMM initialisieren
                    Sim1 = New EVO.Apps.SWMM()


                Case ANW_TESTPROBLEME 'Anwendung Testprobleme
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Testprobleme instanzieren
                    Testprobleme1 = New EVO.Apps.Testprobleme()

                    'HACK: bei Testproblemen als Methodenauswahl nur PES, H&J, MetaEVO und DDS zulassen!
                    Me.IsInitializing = True
                    Call Me.ComboBox_Methode.Items.Clear()
                    Call Me.ComboBox_Methode.Items.AddRange(New String() {"", METH_PES, METH_METAEVO, METH_HOOKEJEEVES, METH_DDS})
                    Me.IsInitializing = False


                Case ANW_TSP 'Anwendung Traveling Salesman Problem (TSP)
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'HACK: bei TSP Datensatz und Methode nicht notwendig, Abk�rzung:
                    'Start-Button aktivieren (keine Methodenauswahl erforderlich)
                    'HACK: bei Testproblemen als Methodenauswahl nur PES, H&J, MetaEVO und DDS zulassen!
                    Me.IsInitializing = True
                    Call Me.ComboBox_Methode.Items.Clear()
                    Call Me.ComboBox_Methode.Items.Add(METH_TSP)
                    Me.IsInitializing = False
                    Me.ComboBox_Methode.Enabled = True
                    Me.ComboBox_Methode.SelectedIndex = 0
                    'Button_Start.Enabled = True

            End Select

            'Bei Sim-Anwendungen
            If (Not IsNothing(Me.Sim1)) Then
                'Settings an Sim1 �bergeben
                Call Me.Sim1.setSettings(Me.mSettings)

                'ggf. Multithreading-Option aktivieren
                If (Me.Sim1.MultithreadingSupported) Then
                    Me.mSettings.General.MultithreadingAllowed = True
                End If
            End If

            'Datensatz UI aktivieren
            Call Me.Datensatz_initUI()

            'Progress zur�cksetzen
            Call Me.mProgress.Initialize()

        Catch ex As Exception

            MsgBox("Fehler beim Initialisieren der Anwendung:" & eol & ex.Message, MsgBoxStyle.Critical)
            Me.IsInitializing = True
            Me.ComboBox_Anwendung.SelectedIndex = 0
            Me.IsInitializing = False

        End Try

        'wegen ver�ndertem Setting MultithreadingAllowed
        Call Me.EVO_Einstellungen1.refreshForm()

        'Mauszeiger wieder normal
        Cursor = Cursors.Default

    End Sub

    'Datensatz-UI anzeigen
    '*********************
    Private Sub Datensatz_initUI()

        Dim pfad As String

        'UI aktivieren
        Me.Label_Datensatz.Enabled = True
        Me.ComboBox_Datensatz.Enabled = True

        'Tooltip zur�cksetzen
        Me.ToolTip1.SetToolTip(Me.ComboBox_Datensatz, "")

        'Combo_Datensatz auff�llen
        Call Me.Datensatz_populateCombo()

        Select Case Me.Anwendung

            Case ANW_TESTPROBLEME
                'Testprobleme:
                '-------------

                'Browse-Button deaktivieren
                Me.Button_BrowseDatensatz.Enabled = False

            Case ANW_TSP
                'Traveling Salesman:
                '-------------------

                'Alles deaktivieren
                Me.Label_Datensatz.Enabled = False
                Me.ComboBox_Datensatz.Enabled = False
                Me.Button_BrowseDatensatz.Enabled = False

            Case Else
                'Simulationsanwendungen:
                '-----------------------

                'Browse-Button aktivieren
                Me.Button_BrowseDatensatz.Enabled = True

                'zuletzt benutzten Datensatz setzen?
                If (Me.ComboBox_Datensatz.Items.Count > 0) Then
                    'obersten (zuletzt genutzten) Datensatz ausw�hlen
                    pfad = Me.ComboBox_Datensatz.Items(0)
                    'Datensatz setzen
                    Cursor = Cursors.WaitCursor
                    Call Sim1.setDatensatz(pfad)
                    Cursor = Cursors.Default
                End If

        End Select


    End Sub

    'Combo_Datensatz auff�llen
    '*************************
    Private Sub Datensatz_populateCombo()

        Dim i As Integer
        Dim pfad As String

        'vorherige Eintr�ge l�schen
        Me.ComboBox_Datensatz.Items.Clear()

        Select Case Me.Anwendung

            Case ANW_TESTPROBLEME

                'Mit Testproblemen f�llen
                Me.ComboBox_Datensatz.Items.AddRange(Testprobleme1.Testprobleme)

            Case ANW_TSP

                'Datensatz nicht erforderlich

            Case Else '(Sim-Anwendungen)

                'Mit Benutzer-MRUSimDatens�tze f�llen
                If (My.Settings.MRUSimDatensaetze.Count > 0) Then

                    'Combobox r�ckw�rts f�llen
                    For i = My.Settings.MRUSimDatensaetze.Count - 1 To 0 Step -1

                        'nur existierende, zur Anwendung passende Datens�tze anzeigen
                        pfad = My.Settings.MRUSimDatensaetze(i)
                        If (File.Exists(pfad) _
                            And Path.GetExtension(pfad).ToUpper() = Me.Sim1.DatensatzExtension) Then
                            Me.ComboBox_Datensatz.Items.Add(My.Settings.MRUSimDatensaetze(i))
                        End If
                    Next

                End If

        End Select

    End Sub

    'Arbeitsverzeichnis/Datensatz ausw�hlen (nur Sim-Anwendungen)
    '************************************************************
    Private Sub Datensatz_browse(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_BrowseDatensatz.Click

        Dim DiagResult As DialogResult
        Dim pfad As String

        'Dialog vorbereiten
        OpenFileDialog1.Filter = Sim1.DatensatzDateiendungen(0) & "-Dateien (*." & Sim1.DatensatzDateiendungen(0) & ")|*." & Sim1.DatensatzDateiendungen(0)
        OpenFileDialog1.Title = "Datensatz ausw�hlen"

        'Alten Datensatz dem Dialog zuweisen
        OpenFileDialog1.InitialDirectory = Sim1.WorkDir_Original
        OpenFileDialog1.FileName = Sim1.WorkDir_Original & Sim1.Datensatz & "." & Sim1.DatensatzDateiendungen(0)

        'Dialog �ffnen
        DiagResult = OpenFileDialog1.ShowDialog()

        'Neuen Datensatz speichern
        If (DiagResult = Windows.Forms.DialogResult.OK) Then

            pfad = OpenFileDialog1.FileName

            'Datensatz setzen
            Call Me.INI_Datensatz(pfad)

            'Methodenauswahl wieder zur�cksetzen 
            '(Der Benutzer muss zuerst Ini neu ausf�hren!)
            Me.ComboBox_Methode.SelectedItem = ""

        End If

    End Sub

    'Sim-Datensatz zur�cksetzen
    '**************************
    Private Sub Datensatz_reset(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_DatensatzZur�cksetzen.Click

        Call Sim1.resetDatensatz()

        MsgBox("Die Startwerte der Optimierungsparameter wurden in die Eingabedateien geschrieben.", MsgBoxStyle.Information, "Info")

    End Sub

    'Datensatz wurde ausgew�hlt
    '**************************
    Private Sub Combo_Datensatz_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Datensatz.SelectedIndexChanged
        If (Not Me.IsInitializing) Then
            Call Me.INI_Datensatz(Me.ComboBox_Datensatz.SelectedItem)
        End If
    End Sub

    ''' <summary>
    ''' Datensatz setzen
    ''' </summary>
    ''' <param name="selectedDatensatz">Pfad zum Datensatz</param>
    Public Sub INI_Datensatz(ByVal selectedDatensatz As String)

        'Zur�cksetzen
        '------------

        'Tooltip
        Me.ToolTip1.SetToolTip(Me.ComboBox_Datensatz, "")

        'Datensatz-Reset
        Me.MenuItem_DatensatzZur�cksetzen.Enabled = False

        'gew�hlten Datensatz an Anwendung �bergeben
        '------------------------------------------
        Select Case Me.Anwendung

            Case ANW_TESTPROBLEME

                'Testproblem setzen
                Testprobleme1.setTestproblem(selectedDatensatz)

                'Tooltip anzeigen
                Me.ToolTip1.SetToolTip(Me.ComboBox_Datensatz, Testprobleme1.TestProblemDescription)

            Case Else '(Alle Sim-Anwendungen)

                'Benutzereinstellungen aktualisieren
                If (My.Settings.MRUSimDatensaetze.Contains(selectedDatensatz)) Then
                    My.Settings.MRUSimDatensaetze.Remove(selectedDatensatz)
                End If
                My.Settings.MRUSimDatensaetze.Add(selectedDatensatz)
                'Benutzereinstellungen speichern
                Call My.Settings.Save()

                'Datensatz Combobox aktualisieren
                Call Me.Datensatz_populateCombo()

                'Auswahl setzen (falls von ausserhalb)
                Me.IsInitializing = True
                Me.ComboBox_Datensatz.SelectedItem = selectedDatensatz
                Me.IsInitializing = False

                'Datensatz setzen
                Call Sim1.setDatensatz(selectedDatensatz)

                'Tooltip anzeigen
                Me.ToolTip1.SetToolTip(Me.ComboBox_Datensatz, selectedDatensatz)

        End Select

        'Methodenauswahl aktivieren und zur�cksetzen
        '-------------------------------------------
        Me.Label_Methode.Enabled = True
        Me.ComboBox_Methode.Enabled = True
        Me.IsInitializing = True
        Me.ComboBox_Methode.SelectedItem = ""
        Me.IsInitializing = False

        'Progress zur�cksetzen
        Call Me.mProgress.Initialize()

    End Sub

    'Methode wurde ausgew�hlt
    '************************
    Private Sub Combo_Method_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Methode.SelectedIndexChanged
        If (Not Me.IsInitializing) Then
            INI_Method(Me.ComboBox_Methode.SelectedItem)
        End If
    End Sub

    ''' <summary>
    ''' Methode setzen
    ''' </summary>
    ''' <param name="selectedMethod">zu setzende Methode (Algorithmus)</param>
    Public Sub INI_Method(ByVal selectedMethod As String)

        Try
            'Falls von ausserhalb gesetzt
            Me.IsInitializing = True
            Me.ComboBox_Methode.SelectedItem = selectedMethod
            Me.IsInitializing = False

            'Mauszeiger busy
            Cursor = Cursors.WaitCursor

            'Problem initialisieren
            '======================
            Call Me.INI_Problem(selectedMethod)

            'Methodenspezifische Vorbereitungen
            '(zun�chst alles deaktivieren, danach je nach Methode aktivieren)
            '================================================================

            'Diagramm zur�cksetzen
            Me.Hauptdiagramm1.Reset()

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Toolbar-Buttons deaktivieren
            Me.ToolStripMenuItem_ErgebnisDBSave.Enabled = False
            Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = False
            Me.ToolStripButton_Scatterplot.Enabled = False

            Select Case Me.mProblem.Method

                Case METH_SENSIPLOT
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'SensiPlot-Controller instanzieren
                    Me.controller = New EVO.SensiPlot.SensiPlotController()

                    'Monitor deaktivieren
                    Me.ToolStripButton_Monitor.Checked = False

                    'TODO: Progress initialisieren


                Case METH_PES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'ES-Controller instanzieren
                    Me.controller = New EVO.ES.ESController()

                    'Ergebnis-Buttons
                    Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = True

                    'TODO: Progress mit Standardwerten initialisieren


                Case METH_HOOKEJEEVES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Kontrolle: Nur SO m�glich!
                    If (Me.mProblem.Modus = EVO_MODUS.Multi_Objective) Then
                        Throw New Exception("Methode von Hooke und Jeeves erlaubt nur Single-Objective Optimierung!")
                    End If

                    'HJ-Controller instanzieren
                    Me.controller = New EVO.HookeAndJeeves.HJController()

                    'Ergebnis-Buttons
                    Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = True

                    'TODO: Progress mit Standardwerten initialisieren


                Case METH_DDS
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Kontrolle: Nur SO m�glich!
                    If (Me.mProblem.Modus = EVO_MODUS.Multi_Objective) Then
                        Throw New Exception("Methode DDS erlaubt nur Single-Objective Optimierung!")
                    End If

                    'DDS-Controller instanzieren
                    Me.controller = New modelEAU.DDS.DDSController()

                    'Ergebnis-Buttons
                    Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = True

                    'TODO: Progress mit Standardwerten initialisieren


                Case METH_CES, METH_HYBRID
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Funktioniert nur bei BlueM.Sim!
                    If (Not Anwendung = ANW_BLUEM) Then
                        Throw New Exception("CES/HYBRID funktioniert bisher nur mit BlueM!")
                    End If

                    'ES-Controller instanzieren
                    Me.controller = New EVO.ES.ESController()

                    'Ergebnis-Buttons
                    Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = True

                    If (Me.mProblem.Method = METH_HYBRID) Then

                        'Original ModellParameter schreiben
                        Call Sim1.Write_ModellParameter()

                    End If

                    'ggf. Testmodus einrichten
                    '-------------------------
                    'Bei Testmodus wird die Anzahl der Kinder und Generationen �berschrieben
                    If Not (Me.mProblem.CES_T_Modus = Common.Constants.CES_T_MODUS.No_Test) Then
                        Call Me.mSettings.CES.setTestModus(Me.mProblem.CES_T_Modus, Sim1.TestPath, 1, 1, Me.mProblem.NumCombinations)
                        Call Me.EVO_Einstellungen1.refreshForm()
                    End If

                    'TODO: Progress mit Standardwerten initialisieren


                Case METH_METAEVO
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'MetaEVO-Controller instanzieren
                    Me.controller = New EVO.MetaEvo.MetaEvoController()

                    'Ergebnis-Buttons
                    Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = True

                    'Progress mit Standardwerten initialisieren
                    Call Me.mProgress.Initialize(1, 1, mSettings.MetaEvo.NumberGenerations, mSettings.MetaEvo.PopulationSize)


                Case METH_TSP
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'TSP-Controller instanzieren
                    Me.controller = New EVO.TSP.TSPController()

                    'TODO: Progress mit Standardwerten initialisieren

            End Select

            'Toolbar-Buttons aktivieren
            Me.ToolStripSplitButton_Diagramm.Enabled = True
            Me.ToolStripSplitButton_ErgebnisDB.Enabled = True
            Me.ToolStripSplitButton_Settings.Enabled = True

            'IniMethod OK -> Start Button aktivieren
            Me.Button_Start.Enabled = True

            If (Me.Anwendung <> ANW_TESTPROBLEME) Then
                'Datensatz-Reset aktivieren
                Me.MenuItem_DatensatzZur�cksetzen.Enabled = True
            End If

            'Multithreading-Option (de)aktivieren (Kombination ist ma�gebend!)
            Me.mSettings.General.MultithreadingAllowed = (Me.mSettings.General.MultithreadingAllowed And Me.controller.MultithreadingSupported)
            Call Me.EVO_Einstellungen1.refreshForm()

        Catch ex As Exception

            MsgBox("Fehler beim Setzen der Methode:" & eol & ex.Message, MsgBoxStyle.Critical)
            'Combobox zur�cksetzen
            Me.IsInitializing = True
            Me.ComboBox_Methode.SelectedIndex = 0
            Me.IsInitializing = False

        End Try

        'Mauszeiger wieder normal
        Cursor = Cursors.Default

    End Sub

    ''' <summary>
    ''' Problem initialisieren und �berall bekannt machen
    ''' </summary>
    ''' <param name="Method">gew�hlte Methode</param>
    Private Sub INI_Problem(ByVal Method As String)

        'Neues Problem mit ausgew�hlter Methode instanzieren
        Me.mProblem = New EVO.Common.Problem(Method)

        'Problemdefinition
        '=================
        Select Case Me.Anwendung

            Case ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM, ANW_TALSIM

                'Bei allen Sim-Anwendungen
                '-----------------------------------------------------

                'WorkDir und Datensatz �bergeben
                Me.mProblem.WorkDir = Sim1.WorkDir_Original
                Me.mProblem.Datensatz = Sim1.Datensatz

                'EVO-Eingabedateien einlesen
                Call Me.mProblem.Read_InputFiles(Me.Sim1.SimStart, Me.Sim1.SimEnde)

                'Problem an Sim-Objekt �bergeben
                Call Me.Sim1.setProblem(Me.mProblem)


            Case ANW_TESTPROBLEME

                'Bei Testproblemen definieren diese das Problem selbst
                '-----------------------------------------------------
                Call Testprobleme1.getProblem(Me.mProblem)


            Case ANW_TSP

                'nix zu tun

        End Select

        'Problem an EVO_Einstellungen �bergeben
        '--------------------------------------
        Call Me.EVO_Einstellungen1.setProblem(Me.mProblem)

        'Individuumsklasse mit Problem initialisieren
        '--------------------------------------------
        Call EVO.Common.Individuum.Initialise(Me.mProblem)

        'Problembeschreibung in Log schreiben
        '------------------------------------
        Dim msg As String
        msg = eol & "Optimization problem loaded:" & eol
        msg &= "----------------------------" & eol
        msg &= Me.mProblem.Description()
        Me.Monitor1.LogAppend(msg)
        Me.Monitor1.SelectTabLog()
        Me.Monitor1.Show()

    End Sub

    Public Sub setBatchMode(ByVal _batchmode As Boolean)
        Me.mSettings.General.BatchMode = _batchmode
    End Sub

    Public Sub setMPCMode(ByVal _mpcmode As Boolean)
        Me.mSettings.General.MPCMode = _mpcmode
    End Sub

    Public Sub setObjBoundary(ByVal _objboundary As Double)
        Me.mSettings.General.ObjBoundary = _objboundary
    End Sub

#End Region 'Initialisierung der Anwendungen

#Region "Ablaufkontrolle"

    Private Sub Button_Start_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Button_Start.Click

        If (Me.isRun) Then
            'Pausieren/weiterlaufen lassen
            Call Me.PAUSE()
        Else
            'Optimierung starten
            Call Me.STARTEN()
        End If
    End Sub

    ''' <summary>
    ''' Startet die Optimierung
    ''' </summary>
    Public Sub STARTEN()

        'Stoppuhr
        Dim AllOptTime As New Stopwatch

        bStartenWiederholen = True
        Do While (bStartenWiederholen)
            bStartenWiederholen = False

            Call StarteDurchlauf(AllOptTime)

        Loop

        MsgBox("Optimierung beendet!", MsgBoxStyle.Information, "BlueM.Opt")
        Me.Monitor1.LogAppend("Die Optimierung dauerte:   " & AllOptTime.Elapsed.Hours & "h  " & AllOptTime.Elapsed.Minutes & "m  " & AllOptTime.Elapsed.Seconds & "s     " & AllOptTime.Elapsed.Milliseconds & "ms")

    End Sub

    Private Sub StarteDurchlauf(ByRef AllOptTime As Stopwatch)
        'Stoppuhr
        Dim blnSimWeiter As Boolean
        AllOptTime.Start()

        'Optimierung starten
        '-------------------
        Me.isRun = True

        'Monitor anzeigen
        If (Me.ToolStripButton_Monitor.Checked) Then
            Call Me.Monitor1.Show()
        End If

        'Ergebnis-Buttons
        Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = False
        If (Not IsNothing(Sim1)) Then
            Me.ToolStripMenuItem_ErgebnisDBSave.Enabled = True
            Me.ToolStripButton_Scatterplot.Enabled = True
            Me.ToolStripMenuItem_ErgebnisDBCompare.Enabled = True
        End If

        'Einstellungen-Buttons
        Me.ToolStripMenuItem_SettingsLoad.Enabled = False

        'Anwendungs-Groupbox deaktivieren
        Me.GroupBox_Anwendung.Enabled = False

        'Settings in temp-Verzeichnis speichern
        Dim dir As String
        dir = My.Computer.FileSystem.SpecialDirectories.Temp & "\"
        Me.saveSettings(dir & "Settings.xml")

        'Event ausl�sen (BatchMode)
        If (Me.mSettings.General.BatchMode) Then
            Me.BatchCounter += 1
            'Sprung in Funktion MPC.Controller.EvoController
            'dort abspeichern der Settings und dann gehts hier weiter
            RaiseEvent OptimizationStarted()
        End If

        'Settings deaktivieren
        Call Me.EVO_Einstellungen1.freeze()

        'Settings an Hauptdiagramm �bergeben
        Call Me.Hauptdiagramm1.setSettings(Me.mSettings)

        'Diagramm vorbereiten und initialisieren
        Call Me.PrepareDiagramm()

        Select Case Anwendung

            'Sim-Anwendungen
            Case ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM, ANW_TALSIM

                'Simulationen vorbereiten
                Call Me.Sim1.prepareSimulation()

                'Startwerte evaluieren
                blnSimWeiter = True
                If (Me.mProblem.Method <> METH_SENSIPLOT) Then
                    If Me.mSettings.General.MPCMode = True Then
                        'Falls die Zielfunktionsauswertung kleiner ist als der ein vorgegebener Schwellwert (MPC.Form1)
                        'dann blnSimWEiter = false, weil dann gar nicht weiter simuliert werden muss
                        Call Me.evaluateStartwerte_MPC(blnSimWeiter)
                        If blnSimWeiter = False Then
                            Exit Select
                        End If
                    Else
                        Call Me.evaluateStartwerte()
                    End If
                End If


                'Controller f�r Sim initialisieren und starten
                Call controller.Init(Me.mProblem, Me.mSettings, Me.mProgress, Me.Hauptdiagramm1)
                Call controller.InitApp(Me.Sim1)
                Call controller.Start()

                'Testprobleme
            Case ANW_TESTPROBLEME

                'Controller f�r Testproblem initialisieren und starten
                Call controller.Init(Me.mProblem, Me.mSettings, Me.mProgress, Me.Hauptdiagramm1)
                Call controller.InitApp(Me.Testprobleme1)
                Call controller.Start()

                'Traveling Salesman
            Case ANW_TSP

                'Controller f�r TSP initialisieren und starten
                Call controller.Init(Me.mProblem, Me.mSettings, Me.mProgress, Me.Hauptdiagramm1)
                'Call controller.InitApp() bei TSP nicht ben�tigt
                Call controller.Start()

        End Select

        ''Globale Fehlerbehandlung f�r Optimierungslauf:
        'Catch ex As Exception
        '    MsgBox(ex.Message, MsgBoxStyle.Critical, "Fehler")
        'End Try

        'Optimierung beendet
        '-------------------
        Me.isRun = False

        'nochmaligen Start verhindern
        Me.Button_Start.Enabled = False

        'Ausgabe der Optimierungszeit
        AllOptTime.Stop()

        If (Me.mSettings.General.BatchMode) Then
            'Event ausl�sen 
            RaiseEvent OptimizationReady()
        End If

    End Sub

    ''' <summary>
    ''' Optimierung pausieren/weiterlaufen lassen
    ''' </summary>
    Private Sub PAUSE()

        'nur wenn Optimierung l�uft
        If (Me.isRun) Then

            If (Not Me.isPause) Then

                'Optimierung pausieren
                '---------------------
                Me.isPause = True

                'Bei Multithreading muss Sim explizit pausiert werden
                If (Me.mSettings.General.UseMultithreading) Then
                    Me.Sim1.isPause = True
                End If

                'Pausen Magic :-)
                Do While (Me.isPause)
                    System.Threading.Thread.Sleep(20)
                    Application.DoEvents()
                Loop

            Else

                'Optimierung weiterlaufen lassen
                '-------------------------------
                Me.isPause = False

                'Bei Multithreading muss Sim explizit wieder gestartet werden
                If (Me.mSettings.General.UseMultithreading) Then
                    Me.Sim1.isPause = False
                End If

            End If

        End If

    End Sub

    ''' <summary>
    ''' Stop-Button wurde geklickt
    ''' </summary>
    Private Sub Button_Stop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Stop.Click
        'Optimierung stoppen
        Call Me.STOPPEN()
    End Sub

    ''' <summary>
    ''' Die Optimierung stoppen (mit Abfragedialog)
    ''' </summary>
    ''' <returns>True wenn gestoppt</returns>
    Private Function STOPPEN() As Boolean

        Dim res As MsgBoxResult
        If (Me.isRun And Not IsNothing(Me.controller)) Then

            res = MsgBox("Optimierung wirklich abbrechen?", MsgBoxStyle.YesNo)

            If (res = MsgBoxResult.Yes) Then
                'Pause ausschalten, sonst l�uft die immer weiter
                Me.isPause = False
                'Controller stoppen
                Call Me.controller.Stoppen()
                Me.controller = Nothing
                'bei Multithreading Sim explizit stoppen
                If (Me.mSettings.General.UseMultithreading) Then
                    Me.Sim1.isStopped = True
                End If

                Me.isRun = False
            Else
                'doch nicht stoppen
                Return False
            End If

        End If
        Return True

    End Function

#End Region 'Ablaufkontrolle

#Region "Diagrammfunktionen"

    'Diagrammfunktionen
    '###################

    'Diagramm-Button hat selbst keine funktionalit�t -> nur DropDown
    '***************************************************************
    Private Sub Button_Diagramm_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripSplitButton_Diagramm.ButtonClick
        Call Me.ToolStripSplitButton_Diagramm.ShowDropDown()
    End Sub

    'Hauptdiagramm bearbeiten
    '************************
    Private Sub TChartEdit(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_TChartEdit.Click
        Call Me.Hauptdiagramm1.TChartEdit(sender, e)
    End Sub

    'Chart nach Excel exportieren
    '****************************
    Private Sub TChart2Excel(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_Tchart2CSV.Click
        SaveFileDialog1.DefaultExt = Me.Hauptdiagramm1.Export.Data.Excel.FileExtension
        SaveFileDialog1.FileName = Me.Hauptdiagramm1.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "Excel-Dateien (*.xls)|*.xls"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            Me.Hauptdiagramm1.Export.Data.Excel.Series = Nothing 'export all series
            Me.Hauptdiagramm1.Export.Data.Excel.IncludeLabels = True
            Me.Hauptdiagramm1.Export.Data.Excel.IncludeIndex = True
            Me.Hauptdiagramm1.Export.Data.Excel.IncludeHeader = True
            Me.Hauptdiagramm1.Export.Data.Excel.IncludeSeriesTitle = True
            Me.Hauptdiagramm1.Export.Data.Excel.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

    'Chart als PNG exportieren
    '*************************
    Private Sub TChart2PNG(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_TChart2PNG.Click
        SaveFileDialog1.DefaultExt = Me.Hauptdiagramm1.Export.Image.PNG.FileExtension
        SaveFileDialog1.FileName = Me.Hauptdiagramm1.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "PNG-Dateien (*.png)|*.png"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            Me.Hauptdiagramm1.Export.Image.PNG.GrayScale = False
            Me.Hauptdiagramm1.Export.Image.PNG.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

    'Chart in nativem TeeChart-Format abspeichern
    '********************************************
    Private Sub TChartSave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_TChartSave.Click
        SaveFileDialog1.DefaultExt = Me.Hauptdiagramm1.Export.Template.FileExtension
        SaveFileDialog1.FileName = Me.Hauptdiagramm1.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "TeeChart-Dateien (*.ten)|*.ten"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            Me.Hauptdiagramm1.Export.Template.IncludeData = True
            Me.Hauptdiagramm1.Export.Template.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

    'Hauptdiagramm vorbereiten
    '*************************
    Private Sub PrepareDiagramm()

        Dim i, j, tmpZielindex() As Integer
        Dim Achse As EVO.Diagramm.Diagramm.Achse
        Dim Achsen As New Collection

        ReDim tmpZielindex(2)                       'Maximal 3 Achsen
        'Zun�chst keine Achsenzuordnung (-1)
        For i = 0 To tmpZielindex.GetUpperBound(0)
            tmpZielindex(i) = -1
        Next

        'Fallunterscheidung Anwendung/Methode
        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        Select Case Anwendung

            Case ANW_TESTPROBLEME 'Testprobleme
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                Call Testprobleme1.DiagInitialise(Me.Hauptdiagramm1)

            Case ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM, ANW_TALSIM
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                Select Case Me.mProblem.Method

                    Case METH_SENSIPLOT 'SensiPlot
                        'XXXXXXXXXXXXXXXXXXXXXXXXX

                        If (Me.mSettings.SensiPlot.Selected_OptParameters.Count = 1) Then

                            '1 OptParameter:
                            '---------------

                            'Achsen:
                            '-------
                            'X-Achse = QWert
                            Achse.Title = Me.mProblem.List_ObjectiveFunctions(Me.mSettings.SensiPlot.Selected_Objective).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)
                            'Y-Achse = OptParameter
                            Achse.Title = Me.mProblem.List_OptParameter(Me.mSettings.SensiPlot.Selected_OptParameters(0)).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)

                            'Achsenzuordnung
                            'BUG 327!
                            For i = 0 To Me.mProblem.NumObjectives - 1
                                If (Me.mProblem.List_ObjectiveFunctions(i).Bezeichnung = Me.mProblem.List_ObjectiveFunctions(Me.mSettings.SensiPlot.Selected_Objective).Bezeichnung) Then
                                    Me.Hauptdiagramm1.ZielIndexX = i
                                    Exit For 'Abbruch
                                End If
                            Next
                            Me.Hauptdiagramm1.ZielIndexY = -1
                            Me.Hauptdiagramm1.ZielIndexZ = -1

                        Else
                            '2 OptParameter:
                            '---------------

                            'Achsen:
                            '-------
                            'X-Achse = OptParameter1
                            Achse.Title = Me.mProblem.List_OptParameter(Me.mSettings.SensiPlot.Selected_OptParameters(0)).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)
                            'Y-Achse = Objective
                            Achse.Title = Me.mProblem.List_ObjectiveFunctions(Me.mSettings.SensiPlot.Selected_Objective).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)
                            'Z-Achse = OptParameter2
                            Achse.Title = Me.mProblem.List_OptParameter(Me.mSettings.SensiPlot.Selected_OptParameters(1)).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)

                            'Achsenzuordnung
                            'BUG 327!
                            Me.Hauptdiagramm1.ZielIndexX = -1
                            For i = 0 To Me.mProblem.NumObjectives - 1
                                If (Me.mProblem.List_ObjectiveFunctions(i).Bezeichnung = Me.mProblem.List_ObjectiveFunctions(Me.mSettings.SensiPlot.Selected_Objective).Bezeichnung) Then
                                    Me.Hauptdiagramm1.ZielIndexY = i
                                    Exit For 'Abbruch
                                End If
                            Next
                            Me.Hauptdiagramm1.ZielIndexZ = -1

                        End If

                        'Diagramm initialisieren
                        Call Me.Hauptdiagramm1.DiagInitialise(Anwendung, Achsen, Me.mProblem)


                    Case Else 'PES, CES, HYBRID, HOOK & JEEVES, DDS
                        'XXXXXXXXXXXXXXXXXXXXX

                        'Achsen:
                        '-------
                        If (Me.mProblem.NumPrimObjective = 1) Then

                            'Single-Objective
                            '================

                            'X-Achse (Nr. der Simulation (Durchlauf))
                            '----------------------------------------
                            Achse.Title = "Simulation"
                            Achse.Automatic = False
                            If (Me.mProblem.Method = METH_PES) Then
                                'Bei PES:
                                If (Me.mSettings.PES.Pop.Is_POPUL) Then
                                    Achse.Maximum = Me.mSettings.PES.N_Gen * Me.mSettings.PES.N_Nachf * Me.mSettings.PES.Pop.N_Runden + 1
                                Else
                                    Achse.Maximum = Me.mSettings.PES.N_Gen * Me.mSettings.PES.N_Nachf + 1
                                End If

                            ElseIf (Me.mProblem.Method = METH_METAEVO) Then
                                'Bei MetaEvo:
                                Achse.Maximum = Me.mSettings.MetaEvo.NumberGenerations * Me.mSettings.MetaEvo.ChildrenPerParent * Me.mSettings.MetaEvo.PopulationSize

                            ElseIf (Me.mProblem.Method = METH_HOOKEJEEVES) Then
                                'Bei Hooke & Jeeves:
                                Achse.Automatic = True

                            ElseIf (Me.mProblem.Method = METH_DDS) Then
                                'Bei DDS:
                                Achse.Automatic = True

                            Else
                                'Bei CES etc.:
                                Achse.Maximum = Me.mSettings.CES.N_Children * Me.mSettings.CES.N_Generations
                            End If

                            Achsen.Add(Achse)

                            'Y-Achse: erste (und einzige) Zielfunktion
                            '-----------------------------------------
                            For i = 0 To Me.mProblem.NumObjectives - 1
                                If (Me.mProblem.List_ObjectiveFunctions(i).isPrimObjective) Then
                                    Achse.Title = Me.mProblem.List_ObjectiveFunctions(i).Bezeichnung
                                    Achse.Automatic = True
                                    Achse.Maximum = 0
                                    Exit For 'Abbruch nach erstem OptZiel
                                End If
                            Next

                            Achsen.Add(Achse)

                            'Achsenzuordnung speichern
                            '-------------------------
                            Me.Hauptdiagramm1.ZielIndexX = -1
                            Me.Hauptdiagramm1.ZielIndexY = i
                            Me.Hauptdiagramm1.ZielIndexZ = -1

                        Else

                            'Multi-Objective
                            '===============

                            'f�r jedes OptZiel eine Achse hinzuf�gen
                            j = 0
                            For i = 0 To Me.mProblem.NumObjectives - 1
                                If (Me.mProblem.List_ObjectiveFunctions(i).isPrimObjective) Then
                                    Achse.Title = Me.mProblem.List_ObjectiveFunctions(i).Bezeichnung
                                    Achse.Automatic = True
                                    Achse.Maximum = 0
                                    Achsen.Add(Achse)
                                    tmpZielindex(j) = i
                                    j += 1
                                    If (j > 2) Then Exit For 'Maximal 3 Achsen
                                End If
                            Next

                            'Achsenzuordnung speichern:
                            '-------------------------
                            Me.Hauptdiagramm1.ZielIndexX = tmpZielindex(0)
                            Me.Hauptdiagramm1.ZielIndexY = tmpZielindex(1)
                            Me.Hauptdiagramm1.ZielIndexZ = tmpZielindex(2)

                            'Warnung bei mehr als 3 OptZielen
                            If (Me.mProblem.NumPrimObjective > 3) Then
                                MsgBox("Die Anzahl der Penalty-Funktionen betr�gt mehr als 3!" & eol _
                                        & "Es werden nur die ersten drei Penalty-Funktionen im Hauptdiagramm angezeigt!", MsgBoxStyle.Information)
                            End If

                        End If

                        'Diagramm initialisieren
                        Call Me.Hauptdiagramm1.DiagInitialise(Anwendung, Achsen, Me.mProblem)

                        'IstWerte in Diagramm einzeichnen
                        Call Me.Hauptdiagramm1.ZeichneIstWerte()

                End Select

        End Select

        Call Application.DoEvents()

    End Sub

    ''' <summary>
    ''' Scatterplot-Matrix anzeigen
    ''' </summary>
    Private Sub showScatterplot(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_Scatterplot.Click

        Cursor = Cursors.WaitCursor

        'gucken, welches Scatterplot noch frei ist
        If (IsNothing(Me.scatterplot1) OrElse Not Me.scatterplot1.Visible) Then
            Me.scatterplot1 = New EVO.Diagramm.Scatterplot(Me.mProblem, Sim1.OptResult, Sim1.OptResultRef)
        ElseIf (IsNothing(Me.scatterplot2) OrElse Not Me.scatterplot2.Visible) Then
            Me.scatterplot2 = New EVO.Diagramm.Scatterplot(Me.mProblem, Sim1.OptResult, Sim1.OptResultRef)
        Else
            Cursor = Cursors.Default
            MsgBox("Es werden bereits 2 Scatterplot-Matrizen angezeigt" & eol & "Bitte zuerst eine schlie�en!", MsgBoxStyle.Information)
        End If

        Cursor = Cursors.Default

    End Sub

#Region "L�sungsauswahl"

    'Klick auf Serie in Diagramm
    '***************************
    Public Sub seriesClick(ByVal sender As Object, ByVal s As Steema.TeeChart.Styles.Series, ByVal valueIndex As Integer, ByVal e As System.Windows.Forms.MouseEventArgs)

        'Notwendige Bedingungen �berpr�fen
        '---------------------------------
        If (IsNothing(Sim1)) Then
            'Anwendung != Sim
            MsgBox("L�sungsauswahl funktioniert nur bei Simulationsanwendungen!", MsgBoxStyle.Information, "Info")
            Exit Sub
        Else

            Dim indID_clicked As Integer
            Dim ind As Common.Individuum

            Try
                'Solution-ID
                indID_clicked = s.Labels(valueIndex)

                'L�sung holen
                ind = Sim1.OptResult.getSolution(indID_clicked)

                'L�sung ausw�hlen
                Call Me.selectSolution(ind)
            Catch
                MsgBox("L�sung nicht ausw�hlbar!", MsgBoxStyle.Information)
            End Try

        End If

    End Sub

    'Eine L�sung ausw�hlen
    '*********************
    Private Sub selectSolution(ByVal ind As Common.Individuum) Handles scatterplot1.pointSelected, scatterplot2.pointSelected

        Dim isOK As Boolean

        'L�sung zu ausgew�hlten L�sungen hinzuf�gen
        isOK = Sim1.OptResult.selectSolution(ind.ID)

        If (isOK) Then

            'L�sungsdialog initialisieren
            If (IsNothing(Me.solutionDialog)) Then
                Me.solutionDialog = New SolutionDialog(Me.mProblem)
            End If

            'L�sungsdialog anzeigen
            Call Me.solutionDialog.Show()

            'L�sung zum L�sungsdialog hinzuf�gen
            Call Me.solutionDialog.addSolution(ind)

            'L�sung im Hauptdiagramm anzeigen
            Call Me.Hauptdiagramm1.ZeichneAusgew�hlteL�sung(ind)

            'L�sung in den Scatterplots anzeigen
            If (Not IsNothing(Me.scatterplot1)) Then
                Call Me.scatterplot1.showSelectedSolution(ind)
            End If
            If (Not IsNothing(Me.scatterplot2)) Then
                Call Me.scatterplot2.showSelectedSolution(ind)
            End If
        End If

        'L�sungsdialog nach vorne bringen
        Call Me.solutionDialog.BringToFront()

    End Sub

    'L�sungsauswahl zur�cksetzen
    '***************************
    Public Sub clearSelection()

        'Serie der ausgew�hlten L�sungen l�schen
        '=======================================

        'Im Hauptdiagramm
        '----------------
        Call Me.Hauptdiagramm1.L�scheAusgew�hlteL�sungen()

        'In den Scatterplot-Matrizen
        '---------------------------
        If (Not IsNothing(Me.scatterplot1)) Then
            Call scatterplot1.clearSelection()
        End If

        If (Not IsNothing(Me.scatterplot2)) Then
            Call scatterplot2.clearSelection()
        End If

        'Auswahl intern zur�cksetzen
        '===========================
        Call Sim1.OptResult.clearSelectedSolutions()

    End Sub

    'ausgew�hlte L�sungen simulieren und in Wave anzeigen
    '****************************************************
    Public Sub showWave(ByVal checkedSolutions As Collection)

        Dim isOK As Boolean
        Dim isSWMM As Boolean
        Dim WorkDir_Prev As String

        Dim zre As Wave.TimeSeries
        Dim SimSeries As New Collection                 'zu zeichnende Simulationsreihen
        Dim RefSeries As New Collection                 'zu zeichnende Referenzreihen

        'BUG 379: Optimierung muss pausiert sein!
        If (Me.isRun And Not Me.isPause) Then
            MsgBox("Bitte die Optimierung zuerst pausieren, um ausgew�hlte Individuen auszuwerten!", MsgBoxStyle.Exclamation, "BlueM.Opt")
            Exit Sub
        End If

        'Wait cursor
        Cursor = Cursors.WaitCursor

        'Simulationen in Originalverzeichnis ausf�hren (ohne Threads),
        'WorDir_Current aber merken, und am Ende wieder zur�cksetzen!
        WorkDir_Prev = Sim1.WorkDir_Current
        Sim1.WorkDir_Current = Sim1.WorkDir_Original

        'Wave instanzieren
        Dim Wave1 As New Wave.Wave()

        'Alle ausgew�hlten L�sungen durchlaufen
        '======================================
        For Each ind As Common.Individuum In Sim1.OptResult.getSelectedSolutions()

            'L�sung per Checkbox ausgew�hlt?
            '-------------------------------
            If (Not checkedSolutions.Contains(ind.ID.ToString())) Then
                Continue For
            End If

            'Individuum in Sim evaluieren (ohne in DB zu speichern, da es ja bereits drin ist)
            isOK = Sim1.Evaluate(ind, False)

            'TODO: Simulationsfehler abfangen!

            'Sonderfall SWMM-Bechnung: keine Ganglinie anzuzeigen
            If (TypeOf Me.Sim1 Is EVO.Apps.SWMM) Then
                isSWMM = True
                Exit Sub
            End If

            'Zu zeichnenden Simulationsreihen zur�cksetzen
            SimSeries.Clear()

            'zu zeichnenden Reihen aus Liste der Ziele raussuchen
            '----------------------------------------------------
            For Each objective As Common.ObjectiveFunction In Me.mProblem.List_ObjectiveFunctions

                If (objective.GetObjType = Common.ObjectiveFunction.ObjectiveType.Series _
                    Or objective.GetObjType = Common.ObjectiveFunction.ObjectiveType.ValueFromSeries) Then

                    With objective

                        'Referenzreihe in Wave laden
                        '---------------------------
                        If (objective.GetObjType = Common.ObjectiveFunction.ObjectiveType.Series) Then
                            With CType(objective, Common.ObjectiveFunction_Series)
                                'Referenzreihen nur jeweils ein Mal zeichnen
                                If (Not RefSeries.Contains(.RefReiheDatei & .RefGr)) Then
                                    RefSeries.Add(.RefGr, .RefReiheDatei & .RefGr)
                                    'Referenzreihe in Wave laden
                                    Wave1.Import_Series(.RefReihe)
                                End If
                            End With
                        End If

                        'Simulationsergebnis in Wave laden
                        '---------------------------------
                        'Simulationsreihen nur jeweils ein Mal zeichnen
                        If (Not SimSeries.Contains(.SimGr)) Then
                            Call SimSeries.Add(.SimGr, .SimGr)
                            zre = Sim1.SimErgebnis.Reihen(.SimGr).Clone()
                            'L�sungsnummer an Titel anh�ngen
                            zre.Title &= " (L�sung " & ind.ID.ToString() & ")"
                            'Simreihe in Wave laden
                            Call Wave1.Import_Series(zre)
                        End If

                    End With
                End If
            Next

        Next ind

        'Wave anzeigen
        '-------------
        Call Wave1.Show()

        'Simulationsverzeichnis zur�cksetzen
        Sim1.WorkDir_Current = WorkDir_Prev

        'Cursor
        Cursor = Cursors.Default

    End Sub

#End Region 'L�sungsauswahl

#End Region 'Diagrammfunktionen

#Region "Ergebnisdatenbank"

    'Ergebnis-Button hat selbst keine funktionalit�t -> nur DropDown
    '***************************************************************
    Private Sub Button_ErgebnisDB_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripSplitButton_ErgebnisDB.ButtonClick
        Call Me.ToolStripSplitButton_ErgebnisDB.ShowDropDown()
    End Sub

    'Ergebnisdatenbank speichern
    '***************************
    Private Sub ErgebnisDB_Save(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_ErgebnisDBSave.Click

        Dim diagresult As DialogResult

        'Datei-speichern Dialog anzeigen
        Me.SaveFileDialog1.Filter = "Access-Datenbanken (*.mdb)|*.mdb"
        Me.SaveFileDialog1.DefaultExt = "mdb"
        Me.SaveFileDialog1.Title = "Ergebnisdatenbank speichern unter..."
        Me.SaveFileDialog1.FileName = Sim1.Datensatz & "_EVO.mdb"
        Me.SaveFileDialog1.InitialDirectory = Sim1.WorkDir_Original
        diagresult = Me.SaveFileDialog1.ShowDialog()

        If (diagresult = Windows.Forms.DialogResult.OK) Then

            'Ergebnisdatenbank speichern
            Call Sim1.OptResult.db_save(Me.SaveFileDialog1.FileName)

        End If

    End Sub

    'Optimierungsergebnis aus einer Datenbank einlesen
    '*************************************************
    Private Sub ErgebnisDB_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_ErgebnisDBLoad.Click

        Dim diagresult As DialogResult
        Dim sourceFile As String
        Dim isOK As Boolean

        'Datei-�ffnen Dialog anzeigen
        Me.OpenFileDialog1.Filter = "Access-Datenbanken (*.mdb)|*.mdb"
        Me.OpenFileDialog1.Title = "Ergebnisdatenbank ausw�hlen"
        Me.OpenFileDialog1.FileName = ""
        Me.OpenFileDialog1.InitialDirectory = Sim1.WorkDir_Original
        diagresult = Me.OpenFileDialog1.ShowDialog()

        If (diagresult = Windows.Forms.DialogResult.OK) Then

            sourceFile = Me.OpenFileDialog1.FileName

            'MDBImportDialog
            '---------------
            Dim importDialog As New EVO.OptResult.MDBImportDialog(Me.mProblem)

            diagresult = importDialog.ShowDialog()

            If (diagresult = Windows.Forms.DialogResult.OK) Then

                'Cursor Wait
                Cursor = Cursors.WaitCursor

                'Daten einlesen
                '==============
                isOK = Sim1.OptResult.db_load(sourceFile)

                If (isOK) Then

                    'Hauptdiagramm
                    '=============

                    'Achsenzuordnung
                    '---------------
                    Me.Hauptdiagramm1.ZielIndexX = importDialog.ListBox_ZieleX.SelectedIndex
                    Me.Hauptdiagramm1.ZielIndexY = importDialog.ListBox_ZieleY.SelectedIndex
                    Me.Hauptdiagramm1.ZielIndexZ = importDialog.ListBox_ZieleZ.SelectedIndex

                    'Achsen
                    '------
                    Dim Achsen As New Collection
                    Dim tmpAchse As EVO.Diagramm.Diagramm.Achse
                    tmpAchse.Automatic = True
                    If (Me.Hauptdiagramm1.ZielIndexZ = -1 And Me.Hauptdiagramm1.ZielIndexY = -1) Then
                        'Single-objective
                        '----------------
                        'X-Achse
                        tmpAchse.Title = "Simulation"
                        Achsen.Add(tmpAchse)
                        'Y-Achse
                        tmpAchse.Title = Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexX).Bezeichnung
                        Achsen.Add(tmpAchse)
                    Else
                        'Multi-objective
                        '---------------
                        'X-Achse
                        tmpAchse.Title = Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexX).Bezeichnung
                        Achsen.Add(tmpAchse)
                        'Y-Achse
                        tmpAchse.Title = Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexY).Bezeichnung
                        Achsen.Add(tmpAchse)
                        If (Not Me.Hauptdiagramm1.ZielIndexZ = -1) Then
                            'Z-Achse
                            tmpAchse.Title = Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexZ).Bezeichnung
                            Achsen.Add(tmpAchse)
                        End If
                    End If

                    'Diagramm initialisieren
                    '-----------------------
                    Me.Hauptdiagramm1.Clear()
                    Me.Hauptdiagramm1.DiagInitialise(Path.GetFileName(sourceFile), Achsen, Me.mProblem)

                    'IstWerte in Diagramm einzeichnen
                    Call Me.Hauptdiagramm1.ZeichneIstWerte()

                    Call My.Application.DoEvents()

                    'Punkte eintragen
                    '----------------
                    Dim serie As Steema.TeeChart.Styles.Series
                    Dim serie3D As Steema.TeeChart.Styles.Points3D

                    'L�sungen
                    '========
                    If (importDialog.ComboBox_SekPop.SelectedItem <> "ausschlie�lich") Then

                        For Each ind As Common.Individuum In Sim1.OptResult.Solutions

                            If (Me.Hauptdiagramm1.ZielIndexZ = -1 And Me.Hauptdiagramm1.ZielIndexY = -1) Then
                                '1D
                                '--
                                'Constraintverletzung pr�fen
                                If (ind.Is_Feasible) Then
                                    serie = Me.Hauptdiagramm1.getSeriesPoint("Population", "red")
                                Else
                                    serie = Me.Hauptdiagramm1.getSeriesPoint("Population (ung�ltig)", "Gray")
                                End If
                                'Zeichnen
                                serie.Add(ind.ID, ind.Objectives(Me.Hauptdiagramm1.ZielIndexX), ind.ID.ToString())
                            ElseIf (Me.Hauptdiagramm1.ZielIndexZ = -1) Then
                                '2D
                                '--
                                'Constraintverletzung pr�fen
                                If (ind.Is_Feasible) Then
                                    serie = Me.Hauptdiagramm1.getSeriesPoint("Population", "Orange")
                                Else
                                    serie = Me.Hauptdiagramm1.getSeriesPoint("Population (ung�ltig)", "Gray")
                                End If
                                'Zeichnen
                                serie.Add(ind.Objectives(Me.Hauptdiagramm1.ZielIndexX), ind.Objectives(Me.Hauptdiagramm1.ZielIndexY), ind.ID.ToString())
                            Else
                                '3D
                                '--
                                'Constraintverletzung pr�fen
                                If (ind.Is_Feasible) Then
                                    serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Population", "Orange")
                                Else
                                    serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Population (ung�ltig)", "Gray")
                                End If
                                'Zeichnen
                                serie3D.Add(ind.Objectives(Me.Hauptdiagramm1.ZielIndexX), ind.Objectives(Me.Hauptdiagramm1.ZielIndexY), ind.Objectives(Me.Hauptdiagramm1.ZielIndexZ), ind.ID.ToString())
                            End If

                        Next

                    End If

                    Call My.Application.DoEvents()

                    'Sekund�rpopulation
                    '==================
                    If (importDialog.ComboBox_SekPop.SelectedItem <> "keine") Then

                        For Each sekpopind As Common.Individuum In Sim1.OptResult.getSekPop()
                            If (Me.Hauptdiagramm1.ZielIndexZ = -1) Then
                                '2D
                                '--
                                serie = Me.Hauptdiagramm1.getSeriesPoint("Sekund�re Population", "Green")
                                serie.Add(sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexX), sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexY), sekpopind.ID.ToString())
                            Else
                                '3D
                                '--
                                serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Sekund�re Population", "Green")
                                serie3D.Add(sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexX), sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexY), sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexZ), sekpopind.ID.ToString())
                            End If
                        Next

                    End If

                    Call My.Application.DoEvents()

                    'Hypervolumen
                    '============
                    If (importDialog.CheckBox_Hypervol.Checked) Then

                        'Hypervolumen instanzieren
                        Dim Hypervolume As EVO.MO_Indicators.Indicators
                        Hypervolume = EVO.MO_Indicators.MO_IndicatorFabrik.GetInstance(EVO.MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, Me.mProblem.NumPrimObjective)
                        Dim indicator As Double
                        Dim nadir() As Double

                        'Alle Generationen durchlaufen
                        For Each sekpop As EVO.OptResult.OptResult.Struct_SekPop In Sim1.OptResult.SekPops

                            'Hypervolumen berechnen
                            Call Hypervolume.update_dataset(Sim1.OptResult.getSekPopValues(sekpop.iGen))
                            indicator = Math.Abs(Hypervolume.calc_indicator())
                            nadir = Hypervolume.nadir

                            'Nadirpunkt in Hauptdiagramm eintragen
                            If (Me.mProblem.NumPrimObjective = 2) Then
                                '2D
                                '--
                                Dim serie2 As Steema.TeeChart.Styles.Points
                                serie2 = Me.Hauptdiagramm1.getSeriesPoint("Nadirpunkt", "Blue", Steema.TeeChart.Styles.PointerStyles.Diamond)
                                serie2.Clear()
                                serie2.Add(nadir(0), nadir(1), "Nadirpunkt")
                            Else
                                '3D
                                '--
                                Dim serie3 As Steema.TeeChart.Styles.Points3D
                                serie3 = Me.Hauptdiagramm1.getSeries3DPoint("Nadirpunkt", "Blue", Steema.TeeChart.Styles.PointerStyles.Diamond)
                                serie3.Clear()
                                serie3.Add(nadir(0), nadir(1), nadir(2), "Nadirpunkt")
                            End If

                            'Hypervolumen in Monitordiagramm eintragen
                            serie = Me.Monitor1.Diag.getSeriesLine("Hypervolumen", "Red")
                            serie.Add(sekpop.iGen, indicator)

                            Call My.Application.DoEvents()

                        Next

                    End If

                    'Ergebnis-Buttons
                    Me.ToolStripButton_Scatterplot.Enabled = True
                    Me.ToolStripMenuItem_ErgebnisDBCompare.Enabled = True

                    'Start-Button deaktivieren
                    Me.Button_Start.Enabled = False

                End If

                'Simulationen vorbereiten (weil m�glicherweise vorher noch nicht geschehen!)
                Call Me.Sim1.prepareSimulation()

                'Cursor Default
                Cursor = Cursors.Default

            End If

        End If

    End Sub

    'Vergleichsergebnis aus Datenbank laden
    '**************************************
    Private Sub ErgebnisDB_Compare(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_ErgebnisDBCompare.Click

        Dim diagresult As DialogResult
        Dim sourceFile As String
        Dim loadOptparameters, isOK As Boolean

        'Datei-�ffnen Dialog anzeigen
        Me.OpenFileDialog1.Filter = "Access-Datenbanken (*.mdb)|*.mdb"
        Me.OpenFileDialog1.Title = "Vergleichsergebnis: Ergebnisdatenbank ausw�hlen"
        Me.OpenFileDialog1.FileName = ""
        Me.OpenFileDialog1.InitialDirectory = Sim1.WorkDir_Original
        diagresult = Me.OpenFileDialog1.ShowDialog()

        If (diagresult = Windows.Forms.DialogResult.OK) Then

            sourceFile = Me.OpenFileDialog1.FileName

            'Abfrage
            diagresult = MsgBox("Sollen auch die OptParameter-Werte des Vergleichsergebnisses geladen werden?" & eol & "(Dazu muss die OptParameter-Definition beider Ergebnisse identisch sein!)", MsgBoxStyle.YesNo, "Vergleichsergebnis laden")

            If (diagresult = Windows.Forms.DialogResult.Yes) Then
                loadOptparameters = True
            Else
                loadOptparameters = False
            End If

            'Cursor Wait
            Cursor = Cursors.WaitCursor

            'Daten einlesen
            '==============
            Sim1.OptResultRef = New EVO.OptResult.OptResult(Me.Sim1.Datensatz, Me.mProblem, False)
            isOK = Sim1.OptResultRef.db_load(sourceFile, loadOptparameters)

            If (isOK) Then

                'In Diagramm anzeigen
                '====================
                Dim serie As Steema.TeeChart.Styles.Points
                Dim serie3D As Steema.TeeChart.Styles.Points3D

                For Each sekpopind As Common.Individuum In Sim1.OptResultRef.getSekPop()
                    If (Me.Hauptdiagramm1.ZielIndexZ = -1) Then
                        '2D
                        '--
                        serie = Me.Hauptdiagramm1.getSeriesPoint("Vergleichsergebnis", "Blue")
                        serie.Add(sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexX), sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexY), "Vergleichsergebnis " & sekpopind.ID)
                    Else
                        '3D
                        '--
                        serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Vergleichsergebnis", "Blue")
                        serie3D.Add(sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexX), sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexY), sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexZ), sekpopind.ID & " (Vergleichsergebnis)")
                    End If
                Next

                'Hypervolumen
                '============
                Dim i As Integer
                Dim sekpopvalues(,), sekpopvaluesRef(,) As Double
                Dim HypervolumeDiff, HypervolumeRef As EVO.MO_Indicators.Hypervolume
                Dim nadir() As Double
                Dim minmax() As Boolean
                Dim indicatorDiff, indicatorRef As Double

                'Vorbereitungen
                ReDim nadir(Me.mProblem.NumPrimObjective - 1)
                ReDim minmax(Me.mProblem.NumPrimObjective - 1)
                For i = 0 To Me.mProblem.NumPrimObjective - 1
                    nadir(i) = 0
                    minmax(i) = False
                Next
                sekpopvalues = Sim1.OptResult.getSekPopValues()
                sekpopvaluesRef = Sim1.OptResultRef.getSekPopValues()

                'Hypervolumendifferenz
                '---------------------
                If (sekpopvalues.Length > 0) Then

                    'Instanzierung
                    HypervolumeDiff = EVO.MO_Indicators.MO_IndicatorFabrik.GetInstance(MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, minmax, nadir, sekpopvalues, sekpopvaluesRef)

                    'Berechnung
                    indicatorDiff = -HypervolumeDiff.calc_indicator()

                    'Nadir-Punkt holen (f�r sp�tere Verwendung bei Referenz-Hypervolumen)
                    nadir = HypervolumeDiff.nadir

                    'In Zwischenablage kopieren
                    Call Clipboard.SetDataObject(indicatorDiff, True)

                    'Anzeige in Messagebox
                    MsgBox("Hypervolumendifferenz zum Vergleichsergebnis:" & eol _
                            & indicatorDiff.ToString() & eol _
                            & "(Wert wurde in die Zwischenablage kopiert)", MsgBoxStyle.Information, "Hypervolumen")

                End If

                'Referenz-Hypervolumen
                '---------------------
                'Instanzierung
                HypervolumeRef = EVO.MO_Indicators.MO_IndicatorFabrik.GetInstance(MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, minmax, nadir, sekpopvaluesRef)
                indicatorRef = -HypervolumeRef.calc_indicator()

                'Im Monitor anzeigen
                Dim colorline1 As New Steema.TeeChart.Tools.ColorLine(Me.Monitor1.Diag.Chart)
                colorline1.Pen.Color = System.Drawing.Color.Red
                colorline1.Pen.Width = 2
                colorline1.AllowDrag = False
                colorline1.Axis = Me.Monitor1.Diag.Axes.Right
                colorline1.Value = indicatorRef

            End If

            'Cursor Default
            Cursor = Cursors.Default

        End If

    End Sub

#End Region 'Ergebnisdatenbank

    ''' <summary>
    ''' Die Startwerte der Optparameter evaluieren
    ''' </summary>
    ''' <remarks>nur f�r Sim-Anwendungen!</remarks>
    Private Sub evaluateStartwerte()

        Dim isOK As Boolean
        Dim startind As EVO.Common.Individuum

        startind = Me.mProblem.getIndividuumStart()

        isOK = Sim1.Evaluate(startind) 'hier ohne multithreading
        If (isOK) Then
            Call Me.Hauptdiagramm1.ZeichneStartWert(startind)
            My.Application.DoEvents()
        End If


    End Sub

    ''' <summary>
    ''' Die Startwerte der Optparameter bei MPC-Anwendungen evaluieren
    ''' </summary>
    ''' <remarks>Mit Vergleich der Zielfunktionsgrenze, damit bei MPC nicht optimiert werden muss wenn Obj = Null</remarks>
        Private Sub evaluateStartwerte_MPC(ByRef blnWeiter As Boolean)

        Dim isOK As Boolean
        Dim startind As EVO.Common.Individuum

        blnWeiter = True
        startind = Me.mProblem.getIndividuumStart()

        isOK = Sim1.Evaluate(startind) 'hier ohne multithreading
        If (isOK) Then
            Call Me.Hauptdiagramm1.ZeichneStartWert(startind)
            My.Application.DoEvents()
        End If

        If startind.PrimObjectives(0) < Me.mSettings.General.ObjBoundary Then
            blnWeiter = False
        End If

    End Sub

    ''' <summary>
    ''' Das Form wird geschlossen
    ''' </summary>
    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        'Optimierung stoppen
        If (Not Me.STOPPEN()) Then
            'FormClosing abbrechen
            e.Cancel = True
        End If
    End Sub

    ''' <summary>
    ''' Notwendig, damit das Form geschlossen wird, kA warum
    ''' </summary>
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub Start_CES_BatchMode(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BachModeToolStripMenuItem.Click

        'Fuer die fortlaufende Log Datei muss:
        'Me.Monitor1.Reset()
        'hier im Form abgeschaltet werden
        '"Die Evaluierung der Generation" abschalten
        'im OptResult die Datenbank deaktivieren:
        'Call Me.db_insert(CType(Ind, Common.Individuum_CES))
        'Grenze einsetzen im ESController: "Abbruchkriterium"

        Dim n_cycles As Integer = 15
        'Dim ReprodItem As EVO.Common.Constants.CES_REPRODOP
        'Dim MutItem As EVO.Common.Constants.CES_MUTATION

        Dim i As Integer
        'For Each ReprodItem In System.Enum.GetValues(GetType(EVO.Common.Constants.CES_REPRODOP))
        'For Each MutItem In System.Enum.GetValues(GetType(EVO.Common.Constants.CES_MUTATION))
        For i = 1 To n_cycles

            Call Me.INI_App(ANW_BLUEM)
            Call Me.INI_Datensatz("D:\xData\Erft_Final\MI_1O_1984\Erft.ALL")
            Call Me.INI_Method(METH_HYBRID)
            Call Me.loadSettings("D:\xData\Erft_Final\MI_1O_1984\Settings\Settings_batch.xml")

            'BatchMode einschalten
            Me.mSettings.General.BatchMode = True

            'Settings �ndern *************************************************
            'Me.mSettings.CES.OptReprodOp = ReprodItem
            'Me.mSettings.CES.OptMutOperator = MutItem

            '*** Hier wird immer nur einer gesetzt! *****
            'Me.mSettings.CES.OptReprodOp = CES_REPRODOP.Uniform_Crossover
            'Me.mSettings.CES.K_Value = 3

            'If Me.mSettings.CES.OptMutOperator = CES_MUTATION.RND_Switch Then
            '    Me.mSettings.CES.Pr_MutRate = 7
            'Else
            '    Me.mSettings.CES.Pr_MutRate = 20
            'End If

            Me.mSettings.CES.Mem_Strategy = MEMORY_STRATEGY.E_Two_Loc_Down

            '*****************************************************************

            'Neue Settings ins Form schreiben (optional)
            Call Me.EVO_Einstellungen1.refreshForm()

            Call Monitor1.SelectTabLog()
            Call Monitor1.Show()

            Monitor1.LogAppend(Me.mSettings.CES.Mem_Strategy.ToString)
            'Monitor1.LogAppend("MutOperator: " & Me.mSettings.CES.OptMutOperator.ToString)

            Call Me.STARTEN()

            'Qualit�t wird im Controller gepr�ft dann Stop Button
            Call Monitor1.savelog("D:\xData\Erft_Final\MI_1O_1984\Batch\Batch ")

            Call Button_New_Click(sender, e)

        Next
        'Next
        'Next

    End Sub

#End Region 'Methoden

End Class
