'BlueM.Opt
'Copyright (C) BlueM Dev Group
'Website: <https://www.bluemodel.org>
'
'This program is free software: you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.
'
'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
'GNU General Public License for more details.
'
'You should have received a copy of the GNU General Public License
'along with this program. If not, see <https://www.gnu.org/licenses/>.
'
Imports System.Xml
Imports System.Xml.Serialization
Imports BlueM.Opt.Common.Constants

''' <summary>
''' Main Window
''' </summary>
Partial Public Class Form1
    Inherits System.Windows.Forms.Form

#Region "Eigenschaften"

    Private IsInitializing As Boolean  'Gibt an, ob das Formular bereits fertig geladen wurde(beim Laden werden sämtliche Events ausgelöst)

    'Anwendung
    Private Anwendung As String

    'Problem
    Public mProblem As BlueM.Opt.Common.Problem

    'Settings
    Private mSettings As BlueM.Opt.Common.Settings

    'Progress
    Private mProgress As BlueM.Opt.Common.Progress

    'Apps
    Private Testprobleme1 As BlueM.Opt.Apps.Testprobleme
    Public WithEvents Sim1 As BlueM.Opt.Apps.Sim

    'Controller
    Private controller As BlueM.Opt.Algos.IController

    'Ablaufkontrolle
    '---------------
    Dim _isrun As Boolean = False
    Dim _ispause As Boolean = False

    ''' <summary>
    ''' Optimierung läuft
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
    Private WithEvents scatterplot1, scatterplot2 As BlueM.Opt.Diagramm.Scatterplot
    Private WithEvents customPlot As BlueM.Opt.Diagramm.CustomPlot

    'Diagramme
    Private WithEvents Hauptdiagramm1 As BlueM.Opt.Diagramm.Hauptdiagramm
    Private WithEvents Monitor1 As BlueM.Opt.Diagramm.Monitor

#End Region 'Eigenschaften

#Region "Methoden"

#Region "UI"

    'Form1 laden
    '***********
    Private Sub Form1_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        'XP-look
        System.Windows.Forms.Application.EnableVisualStyles()

        'Monitor zuweisen
        Me.Monitor1 = BlueM.Opt.Diagramm.Monitor.getInstance()
        'Monitor zentrieren
        Me.Monitor1.Location = New Drawing.Point(Me.Location.X + Me.Width / 2 - Me.Monitor1.Width / 2, Me.Location.Y + Me.Height / 2 - Me.Monitor1.Height / 2)
        'Add handler for log messages
        AddHandler BlueM.Opt.Common.Log.LogMessageAdded, AddressOf Monitor1.LogAppend

        'Formular initialisieren
        Call Me.INI()

        'Handler für Klick auf Serien zuweisen
        AddHandler Me.Hauptdiagramm1.ClickSeries, AddressOf seriesClick

        'Ende der Initialisierung
        Me.IsInitializing = False

    End Sub

    ''' <summary>
    ''' Formular zurücksetzen
    ''' </summary>
    Public Sub INI()

        Me.IsInitializing = True

        'clear the log
        BlueM.Opt.Common.Log.Reset()

        'Anwendungs-Groupbox aktivieren
        Me.GroupBox_Anwendung.Enabled = True

        'Anwendung
        '---------
        'Liste der Anwendungen in ComboBox schreiben und Anfangseinstellung wählen
        Me.ComboBox_Anwendung.Items.Clear()
        Me.ComboBox_Anwendung.Items.AddRange(New Object() {"", ANW_BLUEM, ANW_SWMM, ANW_TALSIM, ANW_TESTPROBLEMS, ANW_TSP}) 'ANW_SMUSI entfernt (#184)
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

        'Liste der Methoden in ComboBox schreiben und Anfangseinstellung wählen
        Me.ComboBox_Methode.Items.Clear()
        Me.ComboBox_Methode.Items.AddRange(New Object() {"", METH_PES, METH_METAEVO, METH_SENSIPLOT, METH_HOOKEJEEVES, METH_DDS})
        Me.ComboBox_Methode.SelectedIndex = 0

        'Einstellungen
        Me.mSettings = New Common.Settings()
        Me.EVO_Einstellungen1.Reset() 'für Neustart wichtig
        Me.EVO_Einstellungen1.setSettings(Me.mSettings)

        'Monitor zurücksetzen
        Me.Monitor1.Reset()

        'Progress instanzieren und an EVO_Opt_Verlauf übergeben
        Me.mProgress = New BlueM.Opt.Common.Progress()
        Me.EVO_Opt_Verlauf1.Initialisieren(Me.mProgress)

        'Toolbar-Buttons deaktivieren
        Me.ToolStripSplitButton_Diagramm.Enabled = False
        Me.ToolStripSplitButton_ErgebnisDB.Enabled = False
        Me.ToolStripButton_Scatterplot.Enabled = False
        Me.ToolStripButton_CustomPlot.Enabled = False
        Me.ToolStripButton_SelectedSolutions.Enabled = False
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

        'Scatterplot
        If Not IsNothing(Me.scatterplot1) Then
            Me.scatterplot1.Close()
            Me.scatterplot1 = Nothing
        End If
        If Not IsNothing(Me.scatterplot2) Then
            Me.scatterplot2.Close()
            Me.scatterplot2 = Nothing
        End If

        'CustomPlot
        If Not IsNothing(Me.customPlot) Then
            Me.customPlot.Close()
            Me.customPlot = Nothing
        End If

        Me.IsInitializing = False

    End Sub

    ''' <summary>
    ''' Button New geklickt
    ''' </summary>
    Private Sub Button_New_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_New.Click
        'Controller stoppen
        If (Me.STOPPEN()) Then
            'Formular zurücksetzen
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

    'Wenn Monitor geöffnet/geschlossen wird, ButtonState aktualisieren
    '*****************************************************************
    Private Sub MonitorOpenClose() Handles Monitor1.MonitorClosed, Monitor1.MonitorOpened
        Me.ToolStripButton_Monitor.Checked = Me.Monitor1.Visible
    End Sub

    ''' <summary>
    ''' Handles Help menu item clicked
    ''' Opens the help URL
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Help(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_Help.Click
        Call Process.Start(HelpURL)
    End Sub

    ''' <summary>
    ''' Handles Release notes menu item clicked
    ''' Opens the release notes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ReleaseNotesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReleaseNotesToolStripMenuItem.Click
        Dim filepath As String
        filepath = IO.Path.Combine(Application.StartupPath, "BLUEM.OPT_RELEASE-NOTES.txt")
        Try
            System.Diagnostics.Process.Start(filepath)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub

    'About Dialog anzeigen
    '*********************
    Private Sub About(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_About.Click
        Dim about As New AboutBox()
        Call about.ShowDialog(Me)
    End Sub

    'Einstellungen-Button hat selbst keine funktionalität -> nur DropDown
    '********************************************************************
    Private Sub Button_Einstellungen_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripSplitButton_Settings.ButtonClick
        Call Me.ToolStripSplitButton_Settings.ShowDropDown()
    End Sub

    'EVO_Einstellungen laden
    '***********************
    Private Sub Einstellungen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_SettingsLoad.Click

        'Dialog einrichten
        OpenFileDialog1.Filter = "XML files (*.xml)|*.xml"
        OpenFileDialog1.Title = "Select settings file"
        If (Not IsNothing(Sim1)) Then
            OpenFileDialog1.InitialDirectory = Sim1.WorkDir_Original
        Else
            OpenFileDialog1.InitialDirectory = CurDir()
        End If

        'Dialog anzeigen
        If (OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) Then

            Try
                'Settings aus Datei laden
                Call Me.loadSettings(OpenFileDialog1.FileName)

            Catch ex As Exception
                MsgBox("Error while reading settings:" & ex.Message, MsgBoxStyle.Exclamation)
            End Try

        End If

    End Sub

#End Region 'UI

#Region "Settings-IO"

    'Laden der Settings aus einer XML-Datei
    '**************************************
    Public Sub loadSettings(ByVal filename As String)

        'read settings from file
        Dim settings As Common.Settings = Common.Settings.Load(filename)

        'Checks: PES OptMode has to be identical
        If settings.PES.OptModus <> Me.mSettings.PES.OptModus Then
            Throw New Exception("The loaded settings use a different optimization mode (single-/multiobjective) and are not compatible!")
        End If

        'Geladene Settings überall neu setzen
        Me.mSettings = settings
        Me.EVO_Einstellungen1.setSettings(Me.mSettings)
        Me.Hauptdiagramm1.setSettings(Me.mSettings)
        If (Not IsNothing(Me.Sim1)) Then
            Me.Sim1.setSettings(Me.mSettings)
        End If

    End Sub

#End Region 'Settings-IO

#Region "Initialisierung der Anwendungen"

    'Die Anwendung wurde ausgewählt und wird jetzt initialisiert
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    'Anwendung wurde ausgewählt
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

            'Diagramm zurücksetzen
            Call Me.Hauptdiagramm1.Reset()

            'Alles deaktivieren, danach je nach Anwendung aktivieren
            '-------------------------------------------------------

            'Sim1 zerstören
            Me.Sim1 = Nothing

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Datensatz-Reset deaktivieren
            Me.MenuItem_DatensatzZurücksetzen.Enabled = False

            'Methodenauswahl deaktivieren
            Me.Label_Methode.Enabled = False
            Me.ComboBox_Methode.Enabled = False

            'Toolbar-Buttons
            Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = False
            Me.ToolStripButton_Scatterplot.Enabled = False
            Me.ToolStripButton_CustomPlot.Enabled = False
            Me.ToolStripButton_SelectedSolutions.Enabled = False
            Me.ToolStripMenuItem_ErgebnisDBCompare.Enabled = False

            'Multithreading standardmäßig verbieten
            Me.mSettings.General.MultithreadingAllowed = False

            'Mauszeiger busy
            Cursor = Cursors.WaitCursor

            'Ausgewählte Anwendung speichern
            Me.Anwendung = selectedAnwendung

            Select Case Me.Anwendung

                Case "" 'Keine Anwendung ausgewählt
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'nix

                Case ANW_BLUEM 'Anwendung BlueM
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse BlueM initialisieren
                    Sim1 = New BlueM.Opt.Apps.BlueMSim()


                Case ANW_SMUSI 'Anwendung Smusi
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse Smusi initialisieren
                    'Sim1 = New BlueM.Opt.Apps.Smusi()


                Case ANW_TALSIM 'Anwendung TALSIM
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse Talsim initialisieren
                    Sim1 = New BlueM.Opt.Apps.Talsim()


                Case ANW_SWMM   'Anwendung SWMM
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse SWMM initialisieren
                    Sim1 = New BlueM.Opt.Apps.SWMM()


                Case ANW_TESTPROBLEMS 'Anwendung Testprobleme
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Testprobleme instanzieren
                    Testprobleme1 = New BlueM.Opt.Apps.Testprobleme()

                    'HACK: bei Testproblemen als Methodenauswahl nur PES, H&J, MetaEVO und DDS zulassen!
                    Me.IsInitializing = True
                    Call Me.ComboBox_Methode.Items.Clear()
                    Call Me.ComboBox_Methode.Items.AddRange(New String() {"", METH_PES, METH_METAEVO, METH_HOOKEJEEVES, METH_DDS})
                    Me.IsInitializing = False


                Case ANW_TSP 'Anwendung Traveling Salesman Problem (TSP)
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'HACK: bei TSP Datensatz und Methode nicht notwendig, Abkürzung:
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
                'Settings an Sim1 übergeben
                Call Me.Sim1.setSettings(Me.mSettings)

                'ggf. Multithreading-Option aktivieren
                If (Me.Sim1.MultithreadingSupported) Then
                    Me.mSettings.General.MultithreadingAllowed = True
                End If
            End If

            'Datensatz UI aktivieren
            Call Me.Datensatz_initUI()

            'Progress zurücksetzen
            Call Me.mProgress.Initialize()

            'log
            Common.Log.AddMessage(Common.Log.levels.info, $"Set application to {Me.Anwendung}")

        Catch ex As Exception

            MsgBox("Error while initializing the application:" & eol & ex.Message, MsgBoxStyle.Critical)
            Me.IsInitializing = True
            Me.ComboBox_Anwendung.SelectedIndex = 0
            Me.IsInitializing = False

        End Try

        'wegen verändertem Setting MultithreadingAllowed
        Call Me.EVO_Einstellungen1.refreshForm()

        'Mauszeiger wieder normal
        Cursor = Cursors.Default

    End Sub

    'Datensatz-UI anzeigen
    '*********************
    Private Sub Datensatz_initUI()

        'UI aktivieren
        Me.Label_Datensatz.Enabled = True
        Me.ComboBox_Datensatz.Enabled = True

        'Tooltip zurücksetzen
        Me.ToolTip1.SetToolTip(Me.ComboBox_Datensatz, "")

        'Combo_Datensatz auffüllen
        Call Me.Datensatz_populateCombo()

        Select Case Me.Anwendung

            Case ANW_TESTPROBLEMS
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

        End Select


    End Sub

    'Combo_Datensatz auffüllen
    '*************************
    Private Sub Datensatz_populateCombo()

        Dim i As Integer
        Dim pfad As String

        'vorherige Einträge löschen
        Me.ComboBox_Datensatz.Items.Clear()

        Select Case Me.Anwendung

            Case ANW_TESTPROBLEMS

                'Mit Testproblemen füllen
                Me.ComboBox_Datensatz.Items.AddRange(Testprobleme1.Testprobleme.ToArray())

            Case ANW_TSP

                'Datensatz nicht erforderlich

            Case Else '(Sim-Anwendungen)

                'Mit Benutzer-MRUSimDatensätze füllen
                Try
                    If (My.Settings.MRUSimDatensaetze.Count > 0) Then

                        'Combobox rückwärts füllen
                        For i = My.Settings.MRUSimDatensaetze.Count - 1 To 0 Step -1

                            'nur existierende, zur Anwendung passende Datensätze anzeigen
                            pfad = My.Settings.MRUSimDatensaetze(i)
                            If (IO.File.Exists(pfad) _
                                And IO.Path.GetExtension(pfad).ToUpper() = Me.Sim1.DatensatzExtension) Then
                                Me.ComboBox_Datensatz.Items.Add(My.Settings.MRUSimDatensaetze(i))
                            End If
                        Next

                    End If
                Catch ex As Exception
                    'TODO: log My.Settings.MRUSimDatensaetze error
                End Try


        End Select

    End Sub

    'Arbeitsverzeichnis/Datensatz auswählen (nur Sim-Anwendungen)
    '************************************************************
    Private Sub Datensatz_browse(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_BrowseDatensatz.Click

        Dim DiagResult As DialogResult
        Dim pfad As String

        'Dialog vorbereiten
        OpenFileDialog1.Filter = $"{Sim1.DatensatzDateiendungen(0)} files (*.{Sim1.DatensatzDateiendungen(0)})|*.{Sim1.DatensatzDateiendungen(0)}"
        OpenFileDialog1.Title = "Select dataset"

        'Alten Datensatz dem Dialog zuweisen
        If Not IsNothing(Sim1.WorkDir_Original) Then
            OpenFileDialog1.InitialDirectory = Sim1.WorkDir_Original
            OpenFileDialog1.FileName = IO.Path.Combine(Sim1.WorkDir_Original, Sim1.Datensatz & "." & Sim1.DatensatzDateiendungen(0))
        End If

        'Dialog öffnen
        DiagResult = OpenFileDialog1.ShowDialog()

        'Neuen Datensatz speichern
        If (DiagResult = Windows.Forms.DialogResult.OK) Then

            pfad = OpenFileDialog1.FileName

            'Datensatz setzen
            Call Me.INI_Datensatz(pfad)

            'Methodenauswahl wieder zurücksetzen 
            '(Der Benutzer muss zuerst Ini neu ausführen!)
            Me.ComboBox_Methode.SelectedItem = ""

        End If

    End Sub

    'Sim-Datensatz zurücksetzen
    '**************************
    Private Sub Datensatz_reset(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_DatensatzZurücksetzen.Click

        Call Sim1.resetDatensatz()

        MsgBox("The starting values of the optimization parameters were written to the dataset.", MsgBoxStyle.Information, "Info")

    End Sub

    'Datensatz wurde ausgewählt
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

        Try

            'Zurücksetzen
            '------------

            'Tooltip
            Me.ToolTip1.SetToolTip(Me.ComboBox_Datensatz, "")

            'Datensatz-Reset
            Me.MenuItem_DatensatzZurücksetzen.Enabled = False

            'gewählten Datensatz an Anwendung übergeben
            '------------------------------------------
            Select Case Me.Anwendung

                Case ANW_TESTPROBLEMS

                    'Testproblem setzen
                    Testprobleme1.setTestproblem(selectedDatensatz)

                    'Tooltip anzeigen
                    Me.ToolTip1.SetToolTip(Me.ComboBox_Datensatz, Testprobleme1.TestProblemDescription)

                Case Else '(Alle Sim-Anwendungen)

                    'Benutzereinstellungen aktualisieren
                    Try
                        'place selected dataset at the end of the list
                        If (My.Settings.MRUSimDatensaetze.Contains(selectedDatensatz)) Then
                            My.Settings.MRUSimDatensaetze.Remove(selectedDatensatz)
                        End If
                        My.Settings.MRUSimDatensaetze.Add(selectedDatensatz)
                        'save user settings
                        Call My.Settings.Save()
                    Catch ex As Exception
                        'TODO: log My.Settings.MRUSimDatensaetze error
                    End Try

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

            'Methodenauswahl aktivieren und zurücksetzen
            '-------------------------------------------
            Me.Label_Methode.Enabled = True
            Me.ComboBox_Methode.Enabled = True
            Me.IsInitializing = True
            Me.ComboBox_Methode.SelectedItem = ""
            Me.IsInitializing = False

            'Progress zurücksetzen
            Call Me.mProgress.Initialize()

            'log
            Common.Log.AddMessage(Common.Log.levels.info, $"Set dataset to {selectedDatensatz}")

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub

    'Methode wurde ausgewählt
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
            '(zunächst alles deaktivieren, danach je nach Methode aktivieren)
            '================================================================

            'Diagramm zurücksetzen
            Me.Hauptdiagramm1.Reset()

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Toolbar-Buttons deaktivieren
            Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = False
            Me.ToolStripButton_Scatterplot.Enabled = False
            Me.ToolStripButton_CustomPlot.Enabled = False
            Me.ToolStripButton_SelectedSolutions.Enabled = False

            Select Case Me.mProblem.Method

                Case METH_SENSIPLOT
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'SensiPlot-Controller instanzieren
                    Me.controller = New BlueM.Opt.Algos.SensiPlot.SensiPlotController()

                    'Monitor deaktivieren
                    Me.ToolStripButton_Monitor.Checked = False

                    'TODO: Progress initialisieren


                Case METH_PES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'ES-Controller instanzieren
                    Me.controller = New BlueM.Opt.Algos.ES.ESController()

                    'Ergebnis-Buttons
                    Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = True

                    'TODO: Progress mit Standardwerten initialisieren


                Case METH_HOOKEJEEVES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Kontrolle: Nur SO möglich!
                    If (Me.mProblem.Modus = EVO_MODE.Multi_Objective) Then
                        Throw New Exception("The method Hooke and Jeeves is only usable for single-objective optimization problems!")
                    End If

                    'HJ-Controller instanzieren
                    Me.controller = New BlueM.Opt.Algos.HookeAndJeeves.HJController()

                    'Ergebnis-Buttons
                    Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = True

                    'TODO: Progress mit Standardwerten initialisieren


                Case METH_DDS
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Kontrolle: Nur SO möglich!
                    If (Me.mProblem.Modus = EVO_MODE.Multi_Objective) Then
                        Throw New Exception("The method DDS is only usable for single-objective optimization problems!")
                    End If

                    'DDS-Controller instanzieren
                    Me.controller = New modelEAU.DDS.DDSController()

                    'Ergebnis-Buttons
                    Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = True

                    'TODO: Progress mit Standardwerten initialisieren


                Case METH_METAEVO
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'MetaEVO-Controller instanzieren
                    Me.controller = New BlueM.Opt.Algos.MetaEvo.MetaEvoController()

                    'Ergebnis-Buttons
                    Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = True

                    'Progress mit Standardwerten initialisieren
                    Call Me.mProgress.Initialize(1, 1, mSettings.MetaEvo.NumberGenerations, mSettings.MetaEvo.PopulationSize)


                Case METH_TSP
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'TSP-Controller instanzieren
                    Me.controller = New BlueM.Opt.Algos.TSP.TSPController()

                    'TODO: Progress mit Standardwerten initialisieren

            End Select

            'Toolbar-Buttons aktivieren
            Me.ToolStripSplitButton_Diagramm.Enabled = True
            Me.ToolStripSplitButton_ErgebnisDB.Enabled = True
            Me.ToolStripSplitButton_Settings.Enabled = True

            'IniMethod OK -> Start Button aktivieren
            Me.Button_Start.Enabled = True

            If (Me.Anwendung <> ANW_TESTPROBLEMS) Then
                'Datensatz-Reset aktivieren
                Me.MenuItem_DatensatzZurücksetzen.Enabled = True
            End If

            'Multithreading-Option (de)aktivieren (Kombination ist maßgebend!)
            Me.mSettings.General.MultithreadingAllowed = (Me.mSettings.General.MultithreadingAllowed And Me.controller.MultithreadingSupported)
            Call Me.EVO_Einstellungen1.refreshForm()

        Catch ex As Exception

            MsgBox("Error while setting the method:" & eol & ex.Message, MsgBoxStyle.Critical)
            'Combobox zurücksetzen
            Me.IsInitializing = True
            Me.ComboBox_Methode.SelectedIndex = 0
            Me.IsInitializing = False

        End Try

        'Mauszeiger wieder normal
        Cursor = Cursors.Default

    End Sub

    ''' <summary>
    ''' Problem initialisieren und überall bekannt machen
    ''' </summary>
    ''' <param name="Method">gewählte Methode</param>
    Private Sub INI_Problem(ByVal Method As String)

        'Neues Problem mit ausgewählter Methode instanzieren
        Me.mProblem = New BlueM.Opt.Common.Problem(Method)

        'Problemdefinition
        '=================
        Select Case Me.Anwendung

            Case ANW_BLUEM, ANW_SMUSI, ANW_SWMM, ANW_TALSIM

                'Bei allen Sim-Anwendungen
                '-----------------------------------------------------

                'WorkDir und Datensatz übergeben
                Me.mProblem.WorkDir = Sim1.WorkDir_Original
                Me.mProblem.Datensatz = Sim1.Datensatz

                'EVO-Eingabedateien einlesen
                Call Me.mProblem.Read_InputFiles(Me.Sim1.SimStart, Me.Sim1.SimEnde)

                'Problem an Sim-Objekt übergeben
                Call Me.Sim1.setProblem(Me.mProblem)


            Case ANW_TESTPROBLEMS

                'Bei Testproblemen definieren diese das Problem selbst
                '-----------------------------------------------------
                Call Testprobleme1.getProblem(Me.mProblem)


            Case ANW_TSP

                'nix zu tun

        End Select

        'Problem an EVO_Einstellungen übergeben
        '--------------------------------------
        Call Me.EVO_Einstellungen1.setProblem(Me.mProblem)

        'Individuumsklasse mit Problem initialisieren
        '--------------------------------------------
        Call BlueM.Opt.Common.Individuum.Initialise(Me.mProblem)

        'Problembeschreibung in Log schreiben
        '------------------------------------
        Common.Log.AddMessage(Common.Log.levels.info, $"Set method to {Me.mProblem.Method}")
        Dim msg As String
        msg = "Optimization problem loaded:" & eol
        msg &= Me.mProblem.Description()
        Common.Log.AddMessage(Common.Log.levels.info, msg)

        Me.Monitor1.SelectTabLog()
        Me.Monitor1.Show()

        'dispose of any previously existing customPlot instance
        If Not IsNothing(Me.customPlot) Then
            Me.customPlot.Dispose()
            Me.customPlot = Nothing
        End If

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

        Common.Log.AddMessage(Common.Log.levels.info, "Starting optimization...")

        Call StarteDurchlauf(AllOptTime)

        MsgBox("Optimization ended!", MsgBoxStyle.Information, "BlueM.Opt")

        Common.Log.AddMessage(Common.Log.levels.info, $"The optimization took {AllOptTime.Elapsed.Hours}h {AllOptTime.Elapsed.Minutes}m {AllOptTime.Elapsed.Seconds}s {AllOptTime.Elapsed.Milliseconds}ms")

    End Sub

    Private Sub StarteDurchlauf(ByRef AllOptTime As Stopwatch)

        Try
            Dim isOK, blnSimWeiter As Boolean

            'Stoppuhr
            AllOptTime.Start()
            Dim starttime As DateTime = DateTime.Now

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
                Me.ToolStripButton_Scatterplot.Enabled = True
                Me.ToolStripButton_CustomPlot.Enabled = True
                Me.ToolStripButton_SelectedSolutions.Enabled = True
                Me.ToolStripMenuItem_ErgebnisDBCompare.Enabled = True
            End If

            'Einstellungen-Buttons
            Me.ToolStripMenuItem_SettingsLoad.Enabled = False

            'Anwendungs-Groupbox deaktivieren
            Me.GroupBox_Anwendung.Enabled = False

            'Settings deaktivieren
            Call Me.EVO_Einstellungen1.freeze()

            'Settings an Hauptdiagramm übergeben
            Call Me.Hauptdiagramm1.setSettings(Me.mSettings)

            'Diagramm vorbereiten und initialisieren
            Call Me.PrepareDiagramm()

            Select Case Anwendung

                Case ANW_BLUEM, ANW_SMUSI, ANW_SWMM, ANW_TALSIM
                    'Sim-Anwendungen

                    'Save settings to file
                    Dim settingsFile As String = IO.Path.Combine(Me.mProblem.WorkDir, $"{Me.mProblem.Datensatz}.BlueM.Opt.{starttime:yyyyMMddHHmm}.settings.xml")
                    Me.mSettings.Save(settingsFile)

                    'Set log file
                    Dim logfilename As String = IO.Path.Combine(Me.mProblem.WorkDir, $"{Me.mProblem.Datensatz}.BlueM.Opt.{starttime:yyyyMMddHHmm}.log")
                    BlueM.Opt.Common.Log.SetLogFile(logfilename)

                    'Prepare OptResult (database)
                    Call Me.Sim1.PrepareOptResult(starttime)

                    'Simulationen vorbereiten
                    Call Me.Sim1.prepareSimulation()

                    'Startwerte evaluieren
                    blnSimWeiter = True
                    If (Me.mProblem.Method <> METH_SENSIPLOT) Then
                        isOK = Me.evaluateStartwerte()
                        If Not isOK Then
                            Throw New Exception($"Simulation of start values failed! Please check the dataset in {Me.Sim1.WorkDir_Current}!")
                        End If
                    End If

                    'Controller für Sim initialisieren und starten
                    Call controller.Init(Me.mProblem, Me.mSettings, Me.mProgress, Me.Hauptdiagramm1)
                    Call controller.InitApp(Me.Sim1)
                    Call controller.Start()

                Case ANW_TESTPROBLEMS
                    'Testprobleme

                    'Controller für Testproblem initialisieren und starten
                    Call controller.Init(Me.mProblem, Me.mSettings, Me.mProgress, Me.Hauptdiagramm1)
                    Call controller.InitApp(Me.Testprobleme1)
                    Call controller.Start()

                Case ANW_TSP
                    'Traveling Salesman

                    'Controller für TSP initialisieren und starten
                    Call controller.Init(Me.mProblem, Me.mSettings, Me.mProgress, Me.Hauptdiagramm1)
                    'Call controller.InitApp() bei TSP nicht benötigt
                    Call controller.Start()

            End Select

        Catch ex As Exception

            'Globale Fehlerbehandlung für Optimierungslauf:
            Common.Log.AddMessage(Common.Log.levels.error, ex.Message)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")

        Finally

            'nochmaligen Start verhindern
            Me.Button_Start.Enabled = False

            'Optimierung beendet
            Me.isRun = False

            'Ausgabe der Optimierungszeit
            AllOptTime.Stop()

        End Try

    End Sub

    ''' <summary>
    ''' Optimierung pausieren/weiterlaufen lassen
    ''' </summary>
    Private Sub PAUSE()

        'nur wenn Optimierung läuft
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

            res = MsgBox("Are you sure you want to abort the optimization?", MsgBoxStyle.YesNo)

            If (res = MsgBoxResult.Yes) Then
                'Pause ausschalten, sonst läuft die immer weiter
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

    'Diagramm-Button hat selbst keine funktionalität -> nur DropDown
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
        SaveFileDialog1.Filter = "Excel files (*.xls)|*.xls"
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
        SaveFileDialog1.Filter = "PNG files (*.png)|*.png"
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
        SaveFileDialog1.Filter = "TeeChart files (*.ten)|*.ten"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            Me.Hauptdiagramm1.Export.Template.IncludeData = True
            Me.Hauptdiagramm1.Export.Template.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

    'Hauptdiagramm vorbereiten
    '*************************
    Private Sub PrepareDiagramm()

        Dim i, j, tmpZielindex() As Integer
        Dim Achse As BlueM.Opt.Diagramm.Diagramm.Achse
        Dim Achsen As New Collection

        ReDim tmpZielindex(2)                       'Maximal 3 Achsen
        'Zunächst keine Achsenzuordnung (-1)
        For i = 0 To tmpZielindex.GetUpperBound(0)
            tmpZielindex(i) = -1
        Next

        'Fallunterscheidung Anwendung/Methode
        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        Select Case Anwendung

            Case ANW_TESTPROBLEMS 'Testprobleme
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                Call Testprobleme1.DiagInitialise(Me.Hauptdiagramm1)

            Case ANW_BLUEM, ANW_SMUSI, ANW_SWMM, ANW_TALSIM
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                Select Case Me.mProblem.Method

                    Case METH_SENSIPLOT 'SensiPlot
                        'XXXXXXXXXXXXXXXXXXXXXXXXX

                        If (Me.mSettings.SensiPlot.Selected_OptParameters.Count = 1) Then

                            '1 OptParameter:
                            '---------------

                            'Achsen:
                            '-------
                            'X-Achse = OptParameter
                            Achse.Title = Me.mProblem.List_OptParameter(Me.mSettings.SensiPlot.Selected_OptParameters(0)).Bezeichnung
                            Achse.Automatic = False
                            Achse.Minimum = Me.mProblem.List_OptParameter(Me.mSettings.SensiPlot.Selected_OptParameters(0)).Min
                            Achse.Maximum = Me.mProblem.List_OptParameter(Me.mSettings.SensiPlot.Selected_OptParameters(0)).Max
                            Achsen.Add(Achse)
                            'Y-Achse = QWert
                            Achse.Title = Me.mProblem.List_ObjectiveFunctions(Me.mSettings.SensiPlot.Selected_Objective).Bezeichnung
                            Achse.Automatic = True
                            Achsen.Add(Achse)
                            'Achsenzuordnung
                            Me.Hauptdiagramm1.ZielIndexX = -1
                            Me.Hauptdiagramm1.ZielIndexY = Me.mSettings.SensiPlot.Selected_Objective
                            Me.Hauptdiagramm1.ZielIndexZ = -1

                        Else
                            '>= 2 OptParameter:
                            '------------------

                            'Achsen:
                            '-------
                            'X-Achse = OptParameter1
                            Achse.Title = Me.mProblem.List_OptParameter(Me.mSettings.SensiPlot.Selected_OptParameters(0)).Bezeichnung
                            Achse.Automatic = False
                            Achse.Minimum = Me.mProblem.List_OptParameter(Me.mSettings.SensiPlot.Selected_OptParameters(0)).Min
                            Achse.Maximum = Me.mProblem.List_OptParameter(Me.mSettings.SensiPlot.Selected_OptParameters(0)).Max
                            Achsen.Add(Achse)
                            'Y-Achse = Objective
                            Achse.Title = Me.mProblem.List_ObjectiveFunctions(Me.mSettings.SensiPlot.Selected_Objective).Bezeichnung
                            Achse.Automatic = True
                            Achsen.Add(Achse)
                            'Z-Achse = OptParameter2
                            Achse.Title = Me.mProblem.List_OptParameter(Me.mSettings.SensiPlot.Selected_OptParameters(1)).Bezeichnung
                            Achse.Automatic = False
                            Achse.Minimum = Me.mProblem.List_OptParameter(Me.mSettings.SensiPlot.Selected_OptParameters(1)).Min
                            Achse.Maximum = Me.mProblem.List_OptParameter(Me.mSettings.SensiPlot.Selected_OptParameters(1)).Max
                            Achsen.Add(Achse)

                            'Achsenzuordnung
                            Me.Hauptdiagramm1.ZielIndexX = -1
                            Me.Hauptdiagramm1.ZielIndexY = Me.mSettings.SensiPlot.Selected_Objective
                            Me.Hauptdiagramm1.ZielIndexZ = -1

                        End If

                        'Diagramm initialisieren
                        Call Me.Hauptdiagramm1.DiagInitialise(Anwendung, Achsen, Me.mProblem)


                    Case Else 'PES, HOOK & JEEVES, DDS
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

                            'für jedes OptZiel eine Achse hinzufügen
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
                                MsgBox("The number of primary objectives is more than 3!" & eol _
                                        & "Only the first three primary objectives will be displayed in the main chart!", MsgBoxStyle.Information)
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
            Me.scatterplot1 = New BlueM.Opt.Diagramm.Scatterplot(Me.mProblem, Sim1.OptResult, Sim1.OptResultRef)
        ElseIf (IsNothing(Me.scatterplot2) OrElse Not Me.scatterplot2.Visible) Then
            Me.scatterplot2 = New BlueM.Opt.Diagramm.Scatterplot(Me.mProblem, Sim1.OptResult, Sim1.OptResultRef)
        Else
            Cursor = Cursors.Default
            MsgBox($"There are already two scatterplot matrices open!{eol}Please close at least one of them first!", MsgBoxStyle.Information)
        End If

        Cursor = Cursors.Default

    End Sub

    ''' <summary>
    ''' Open the custom plot window
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub showCustomPlot(sender As Object, e As EventArgs) Handles ToolStripButton_CustomPlot.Click

        If IsNothing(Me.customPlot) Then
            Me.customPlot = New BlueM.Opt.Diagramm.CustomPlot(Me.mProblem, Sim1.OptResult)
        Else
            Me.customPlot.UpdateData(Sim1.OptResult)

            If Not Me.customPlot.Visible Then
                Me.customPlot.Visible = True
            End If

            Me.customPlot.BringToFront()
        End If

    End Sub

#Region "Lösungsauswahl"

    ''' <summary>
    ''' Button to show selected Solutions clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToolStripButton_SelectedSolutions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_SelectedSolutions.Click
        'Lösungsdialog initialisieren
        If (IsNothing(Me.solutionDialog)) Then
            Me.solutionDialog = New SolutionDialog(Me.mProblem)
        End If

        'Lösungsdialog anzeigen
        Call Me.solutionDialog.Show()
        Call Me.solutionDialog.BringToFront()
    End Sub


    'Klick auf Serie in Diagramm
    '***************************
    Public Sub seriesClick(ByVal sender As Object, ByVal s As Steema.TeeChart.Styles.Series, ByVal valueIndex As Integer, ByVal e As System.Windows.Forms.MouseEventArgs)

        'Notwendige Bedingungen überprüfen
        '---------------------------------
        If (IsNothing(Sim1)) Then
            'Anwendung != Sim
            MsgBox("Selecting solutions currently only works with simulation-based applications!", MsgBoxStyle.Information, "Info")
            Exit Sub
        Else

            Dim indID_clicked As Integer
            Dim ind As Common.Individuum

            Try
                'Solution-ID
                indID_clicked = s.Labels(valueIndex)

                'Lösung holen
                ind = Sim1.OptResult.getSolution(indID_clicked)

                'Lösung auswählen
                Call Me.selectSolution(ind)
            Catch
                MsgBox("Solution is not selectable!", MsgBoxStyle.Information)
            End Try

        End If

    End Sub

    'Eine Lösung auswählen
    '*********************
    Private Sub selectSolution(ByVal ind As Common.Individuum) Handles scatterplot1.pointSelected, scatterplot2.pointSelected, customPlot.pointSelected

        Dim isOK As Boolean

        'Lösung zu ausgewählten Lösungen hinzufügen
        isOK = Sim1.OptResult.selectSolution(ind.ID)

        If (isOK) Then

            'Lösungsdialog initialisieren
            If (IsNothing(Me.solutionDialog)) Then
                Me.solutionDialog = New SolutionDialog(Me.mProblem)
            End If

            'Lösungsdialog anzeigen
            Call Me.solutionDialog.Show()

            'Lösung zum Lösungsdialog hinzufügen
            Call Me.solutionDialog.addSolution(ind)

            'Lösung im Hauptdiagramm anzeigen
            Call Me.Hauptdiagramm1.DrawSelectedSolution(ind)

            'Lösung in den Scatterplots anzeigen
            If (Not IsNothing(Me.scatterplot1)) Then
                Call Me.scatterplot1.showSelectedSolution(ind)
            End If
            If (Not IsNothing(Me.scatterplot2)) Then
                Call Me.scatterplot2.showSelectedSolution(ind)
            End If

            'select solution in the custom plot
            If (Not IsNothing(Me.customPlot)) Then
                Call Me.customPlot.showSelectedSolution(ind)
            End If
        End If

        'Lösungsdialog nach vorne bringen
        Call Me.solutionDialog.BringToFront()

    End Sub

    'Lösungsauswahl zurücksetzen
    '***************************
    Public Sub clearSelectedSolutions()

        'Serie der ausgewählten Lösungen löschen
        '=======================================

        'Im Hauptdiagramm
        '----------------
        Call Me.Hauptdiagramm1.LöscheAusgewählteLösungen()

        'In den Scatterplot-Matrizen
        '---------------------------
        If (Not IsNothing(Me.scatterplot1)) Then
            Call scatterplot1.clearSelection()
        End If

        If (Not IsNothing(Me.scatterplot2)) Then
            Call scatterplot2.clearSelection()
        End If

        'In the custom plot
        If (Not IsNothing(Me.customPlot)) Then
            Call Me.customPlot.clearSelection()
        End If

        'Auswahl intern zurücksetzen
        '===========================
        Call Sim1.OptResult.clearSelectedSolutions()

    End Sub

    ''' <summary>
    ''' Lösungsauswahl aktualisieren
    ''' </summary>
    ''' <param name="selectedSolution_IDs">Array von Lösungs-IDs</param>
    ''' <remarks></remarks>
    Public Sub updateSelectedSolutions(ByVal selectedSolution_IDs() As Integer)

        'Selektierte Lösungen neu setzen
        Call Sim1.OptResult.clearSelectedSolutions()
        For Each id As Integer In selectedSolution_IDs
            Call Sim1.OptResult.selectSolution(id)
        Next

        'Im Hauptdiagramm neu zeichnen
        Call Me.Hauptdiagramm1.LöscheAusgewählteLösungen()
        For Each ind As Common.Individuum In Me.Sim1.OptResult.getSelectedSolutions
            Call Me.Hauptdiagramm1.DrawSelectedSolution(ind)
        Next

        'In den Scatterplot-Matrizen neu zeichnen
        If (Not IsNothing(Me.scatterplot1)) Then
            Call scatterplot1.clearSelection()
            For Each ind As Common.Individuum In Me.Sim1.OptResult.getSelectedSolutions
                Call Me.scatterplot1.showSelectedSolution(ind)
            Next
        End If

        If (Not IsNothing(Me.scatterplot2)) Then
            Call scatterplot2.clearSelection()
            For Each ind As Common.Individuum In Me.Sim1.OptResult.getSelectedSolutions
                Call Me.scatterplot2.showSelectedSolution(ind)
            Next
        End If

        'Im CustomPlot neu zeichnen
        If (Not IsNothing(Me.customPlot)) Then
            Call Me.customPlot.clearSelection()
            For Each ind As Common.Individuum In Me.Sim1.OptResult.getSelectedSolutions
                Call Me.customPlot.showSelectedSolution(ind)
            Next
        End If

    End Sub

    'ausgewählte Lösungen simulieren und in Wave anzeigen
    '****************************************************
    Public Sub simulateSelectedSolutions(ByVal checkedSolution_IDs() As Integer)

        Dim isOK As Boolean
        Dim isSWMM As Boolean
        Dim WorkDir_Prev, WorkDir As String

        Dim zre As Wave.TimeSeries
        Dim SimSeries As New Collection                 'zu zeichnende Simulationsreihen
        Dim RefSeries As New Collection                 'zu zeichnende Referenzreihen

        'Optimierung muss pausiert sein! (#220)
        If (Me.isRun And Not Me.isPause) Then
            MsgBox("Please pause the optimization first in order to evaluate the selected solutions!", MsgBoxStyle.Exclamation, "BlueM.Opt")
            Exit Sub
        End If

        'Wait cursor
        Cursor = Cursors.WaitCursor

        'Simulationen in eigenen Unterverzeichnissen ausführen (ohne Threads),
        'WorDir_Current aber merken, und am Ende wieder zurücksetzen!
        WorkDir_Prev = Sim1.WorkDir_Current

        'Wave instanzieren
        Dim Wave1 As New Wave.Wave()

        'Alle ausgewählten Lösungen durchlaufen
        '======================================
        For Each ind As Common.Individuum In Sim1.OptResult.getSelectedSolutions()

            'Lösung per Checkbox ausgewählt?
            '-------------------------------
            If (Not checkedSolution_IDs.Contains(ind.ID)) Then
                Continue For
            End If

            If Me.mProblem.Method = METH_SENSIPLOT AndAlso Me.mSettings.SensiPlot.Save_Results Then
                'TODO: reuse existing simulation results in folder $"sensiplot_{ind.ID:0000}"!
            End If

            'WorkDir einrichten
            WorkDir = IO.Path.Combine(Sim1.WorkDir_Original, "solution_" & ind.ID.ToString("0000"))
            If Not IO.Directory.Exists(WorkDir) Then
                IO.Directory.CreateDirectory(WorkDir)
            End If
            Sim1.copyDateset(WorkDir)
            Sim1.WorkDir_Current = WorkDir

            'Individuum in Sim evaluieren (ohne in DB zu speichern, da es ja bereits drin ist)
            isOK = Sim1.Evaluate(ind, False)

            'TODO: Simulationsfehler abfangen!

            'Sonderfall SWMM-Bechnung: keine Ganglinie anzuzeigen
            If (TypeOf Me.Sim1 Is BlueM.Opt.Apps.SWMM) Then
                isSWMM = True
                Exit Sub
            End If

            'Zu zeichnenden Simulationsreihen zurücksetzen
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
                                'TODO: Dieselbe Referenzreihe könnte aber mehrfach mit jeweils 
                                '      unterschiedlichen Evaluierungszeiträumen definiert sein.
                                '      Dann sollte sie auch mehrfach gezeichnet werden.
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
                            'Lösungsnummer an Titel anhängen
                            zre.Title &= $" (Solution {ind.ID})"
                            'Simreihe in Wave laden
                            Call Wave1.Import_Series(zre)
                        End If

                    End With
                End If
            Next

        Next ind

        'Wave anzeigen
        '-------------
        Dim app As New BlueM.Wave.App(Wave1)

        'Simulationsverzeichnis zurücksetzen
        Sim1.WorkDir_Current = WorkDir_Prev

        'Cursor
        Cursor = Cursors.Default

    End Sub

#End Region 'Lösungsauswahl

#End Region 'Diagrammfunktionen

#Region "Ergebnisdatenbank"

    'Ergebnis-Button hat selbst keine funktionalität -> nur DropDown
    '***************************************************************
    Private Sub Button_ErgebnisDB_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripSplitButton_ErgebnisDB.ButtonClick
        Call Me.ToolStripSplitButton_ErgebnisDB.ShowDropDown()
    End Sub

    'Optimierungsergebnis aus einer Datenbank einlesen
    '*************************************************
    Private Sub ErgebnisDB_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_ErgebnisDBLoad.Click

        Dim diagresult As DialogResult
        Dim sourceFile As String
        Dim isOK As Boolean

        'Datei-öffnen Dialog anzeigen
        Me.OpenFileDialog1.Filter = "Access databases (*.mdb)|*.mdb"
        Me.OpenFileDialog1.Title = "Select result DB"
        Me.OpenFileDialog1.FileName = ""
        Me.OpenFileDialog1.InitialDirectory = Sim1.WorkDir_Original
        diagresult = Me.OpenFileDialog1.ShowDialog()

        If (diagresult = Windows.Forms.DialogResult.OK) Then

            sourceFile = Me.OpenFileDialog1.FileName

            'MDBImportDialog
            '---------------
            Dim importDialog As New BlueM.Opt.OptResult.MDBImportDialog(Me.mProblem)

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
                    Dim tmpAchse As BlueM.Opt.Diagramm.Diagramm.Achse
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
                    Me.Hauptdiagramm1.DiagInitialise(IO.Path.GetFileName(sourceFile), Achsen, Me.mProblem)

                    'IstWerte in Diagramm einzeichnen
                    Call Me.Hauptdiagramm1.ZeichneIstWerte()

                    Call My.Application.DoEvents()

                    'Punkte eintragen
                    '----------------
                    Dim serie As Steema.TeeChart.Styles.Series
                    Dim serie3D As Steema.TeeChart.Styles.Points3D

                    'Lösungen
                    '========
                    If (importDialog.ComboBox_SekPop.SelectedItem <> "exclusively") Then

                        For Each ind As Common.Individuum In Sim1.OptResult.Solutions

                            If (Me.Hauptdiagramm1.ZielIndexZ = -1 And Me.Hauptdiagramm1.ZielIndexY = -1) Then
                                '1D
                                '--
                                'Constraintverletzung prüfen
                                If (ind.Is_Feasible) Then
                                    serie = Me.Hauptdiagramm1.getSeriesPoint("Population", "red")
                                Else
                                    serie = Me.Hauptdiagramm1.getSeriesPoint("Population (invalid)", "Gray")
                                End If
                                'Zeichnen
                                serie.Add(ind.ID, ind.Objectives(Me.Hauptdiagramm1.ZielIndexX) * Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexX).Richtung, ind.ID.ToString())
                            ElseIf (Me.Hauptdiagramm1.ZielIndexZ = -1) Then
                                '2D
                                '--
                                'Constraintverletzung prüfen
                                If (ind.Is_Feasible) Then
                                    serie = Me.Hauptdiagramm1.getSeriesPoint("Population", "Orange")
                                Else
                                    serie = Me.Hauptdiagramm1.getSeriesPoint("Population (invalid)", "Gray")
                                End If
                                'Zeichnen
                                serie.Add(ind.Objectives(Me.Hauptdiagramm1.ZielIndexX) * Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexX).Richtung, ind.Objectives(Me.Hauptdiagramm1.ZielIndexY) * Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexY).Richtung, ind.ID.ToString())
                            Else
                                '3D
                                '--
                                'Constraintverletzung prüfen
                                If (ind.Is_Feasible) Then
                                    serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Population", "Orange")
                                Else
                                    serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Population (invalid)", "Gray")
                                End If
                                'Zeichnen
                                serie3D.Add(ind.Objectives(Me.Hauptdiagramm1.ZielIndexX) * Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexX).Richtung, ind.Objectives(Me.Hauptdiagramm1.ZielIndexY) * Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexY).Richtung, ind.Objectives(Me.Hauptdiagramm1.ZielIndexZ) * Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexZ).Richtung, ind.ID.ToString())
                            End If

                        Next

                    End If

                    Call My.Application.DoEvents()

                    'Sekundärpopulation
                    '==================
                    If (importDialog.ComboBox_SekPop.SelectedItem <> "none") Then

                        For Each sekpopind As Common.Individuum In Sim1.OptResult.getSekPop()
                            If (Me.Hauptdiagramm1.ZielIndexZ = -1) Then
                                '2D
                                '--
                                serie = Me.Hauptdiagramm1.getSeriesPoint("Secondary population", "Green")
                                serie.Add(sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexX) * Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexX).Richtung, sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexY) * Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexY).Richtung, sekpopind.ID.ToString())
                            Else
                                '3D
                                '--
                                serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Secondary population", "Green")
                                serie3D.Add(sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexX) * Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexX).Richtung, sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexY) * Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexY).Richtung, sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexZ) * Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexZ).Richtung, sekpopind.ID.ToString())
                            End If
                        Next

                    End If

                    Call My.Application.DoEvents()

                    'Hypervolumen
                    '============
                    If (importDialog.CheckBox_Hypervol.Checked) Then

                        'Hypervolumen instanzieren
                        Dim Hypervolume As BlueM.Opt.MO_Indicators.Indicators
                        Hypervolume = BlueM.Opt.MO_Indicators.MO_IndicatorFabrik.GetInstance(BlueM.Opt.MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, Me.mProblem.NumPrimObjective)
                        Dim indicator As Double
                        Dim nadir() As Double

                        'Alle Generationen durchlaufen
                        For Each sekpop As BlueM.Opt.OptResult.OptResult.Struct_SekPop In Sim1.OptResult.SekPops

                            'Hypervolumen berechnen
                            Call Hypervolume.update_dataset(Sim1.OptResult.getSekPopValues(sekpop.iGen))
                            indicator = Math.Abs(Hypervolume.calc_indicator())
                            nadir = Hypervolume.nadir

                            'Nadirpunkt in Hauptdiagramm eintragen
                            If (Me.mProblem.NumPrimObjective = 2) Then
                                '2D
                                '--
                                Dim serie2 As Steema.TeeChart.Styles.Points
                                serie2 = Me.Hauptdiagramm1.getSeriesPoint("Nadir point", "Blue", Steema.TeeChart.Styles.PointerStyles.Diamond)
                                serie2.Clear()
                                serie2.Add(nadir(0) * Me.mProblem.List_ObjectiveFunctions(0).Richtung, nadir(1) * Me.mProblem.List_ObjectiveFunctions(1).Richtung, "Nadir point")
                            Else
                                '3D
                                '--
                                Dim serie3 As Steema.TeeChart.Styles.Points3D
                                serie3 = Me.Hauptdiagramm1.getSeries3DPoint("Nadir point", "Blue", Steema.TeeChart.Styles.PointerStyles.Diamond)
                                serie3.Clear()
                                serie3.Add(nadir(0) * Me.mProblem.List_ObjectiveFunctions(0).Richtung, nadir(1) * Me.mProblem.List_ObjectiveFunctions(1).Richtung, nadir(2) * Me.mProblem.List_ObjectiveFunctions(2).Richtung, "Nadir point")
                            End If

                            'Hypervolumen in Monitordiagramm eintragen
                            serie = Me.Monitor1.Diag.getSeriesLine("Hypervolume", "Red")
                            serie.Add(sekpop.iGen, indicator)

                            Call My.Application.DoEvents()

                        Next

                    End If

                    'Ergebnis-Buttons
                    Me.ToolStripButton_Scatterplot.Enabled = True
                    Me.ToolStripButton_CustomPlot.Enabled = True
                    Me.ToolStripButton_SelectedSolutions.Enabled = True
                    Me.ToolStripMenuItem_ErgebnisDBCompare.Enabled = True

                    'Start-Button deaktivieren
                    Me.Button_Start.Enabled = False

                End If

                'Simulationen vorbereiten (weil möglicherweise vorher noch nicht geschehen!)
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

        'Datei-öffnen Dialog anzeigen
        Me.OpenFileDialog1.Filter = "Access-Database (*.mdb)|*.mdb"
        Me.OpenFileDialog1.Title = "Result comparison: select optimization result"
        Me.OpenFileDialog1.FileName = ""
        Me.OpenFileDialog1.InitialDirectory = Sim1.WorkDir_Original
        diagresult = Me.OpenFileDialog1.ShowDialog()

        If (diagresult = Windows.Forms.DialogResult.OK) Then

            sourceFile = Me.OpenFileDialog1.FileName

            'Abfrage
            diagresult = MsgBox("Should the optimization parameters of the comparison result be loaded as well?" & eol _
                                & "(This requires that the optimization parameter definition of both results are identical!)", MsgBoxStyle.YesNo)

            If (diagresult = Windows.Forms.DialogResult.Yes) Then
                loadOptparameters = True
            Else
                loadOptparameters = False
            End If

            'Cursor Wait
            Cursor = Cursors.WaitCursor

            'Daten einlesen
            '==============
            Sim1.OptResultRef = New BlueM.Opt.OptResult.OptResult(Me.Sim1.Datensatz, Me.mProblem, False)
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
                        serie = Me.Hauptdiagramm1.getSeriesPoint("Comparison result", "Blue")
                        serie.Add(sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexX) * Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexX).Richtung, sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexY) * Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexY).Richtung, $"comparison result {sekpopind.ID}")
                    Else
                        '3D
                        '--
                        serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Comparison result", "Blue")
                        serie3D.Add(sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexX) * Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexX).Richtung, sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexY) * Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexY).Richtung, sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexZ) * Me.mProblem.List_ObjectiveFunctions(Me.Hauptdiagramm1.ZielIndexZ).Richtung, $"comparison result {sekpopind.ID}")
                    End If
                Next

                'Hypervolumen
                '============
                Dim i As Integer
                Dim sekpopvalues(,), sekpopvaluesRef(,) As Double
                Dim HypervolumeDiff, HypervolumeRef As BlueM.Opt.MO_Indicators.Hypervolume
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
                    HypervolumeDiff = BlueM.Opt.MO_Indicators.MO_IndicatorFabrik.GetInstance(MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, minmax, nadir, sekpopvalues, sekpopvaluesRef)

                    'Berechnung
                    indicatorDiff = -HypervolumeDiff.calc_indicator()

                    'Nadir-Punkt holen (für spätere Verwendung bei Referenz-Hypervolumen)
                    nadir = HypervolumeDiff.nadir

                    'In Zwischenablage kopieren
                    Call Clipboard.SetDataObject(indicatorDiff, True)

                    'Anzeige in Messagebox
                    MsgBox("Hypervolume difference to comparison result:" & eol _
                            & indicatorDiff.ToString() & eol _
                            & "(Value was copied to the clipboard)", MsgBoxStyle.Information, "Hypervolume")

                End If

                'Referenz-Hypervolumen
                '---------------------
                'Instanzierung
                HypervolumeRef = BlueM.Opt.MO_Indicators.MO_IndicatorFabrik.GetInstance(MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, minmax, nadir, sekpopvaluesRef)
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
    ''' <returns>boolean success</returns>
    ''' <remarks>nur für Sim-Anwendungen!</remarks>
    Private Function evaluateStartwerte() As Boolean

        Dim isOK As Boolean
        Dim startind As BlueM.Opt.Common.Individuum

        startind = Me.mProblem.getIndividuumStart()

        isOK = Sim1.Evaluate(startind) 'hier ohne multithreading
        If (isOK) Then
            Call Me.Hauptdiagramm1.ZeichneStartWert(startind)
            My.Application.DoEvents()
        End If

        Return isOK

    End Function

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

#End Region 'Methoden

End Class
