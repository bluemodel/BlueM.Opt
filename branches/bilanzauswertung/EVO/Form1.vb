'*******************************************************************************
'*******************************************************************************
'**** BlueM.Opt                                                             ****
'****                                                                       ****
'**** Christoph Huebner, Felix Froehlich, Dirk Muschalla, Dominik Kerber    ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'****                                                                       ****
'**** Erstellt: Dezember 2003                                               ****
'****                                                                       ****
'**** Letzte Änderung: Dezember 2008                                        ****
'*******************************************************************************
'*******************************************************************************

Option Strict Off 'allows permissive type semantics. explicit narrowing conversions are not required 

Imports System.IO
Imports IHWB.EVO.Common.Constants

''' <summary>
''' Main Window
''' </summary>
Partial Class Form1
    Inherits System.Windows.Forms.Form

#Region "Eigenschaften"

    Private IsInitializing As Boolean

    'Anwendung
    Private Anwendung As String

    'Problem
    Private mProblem As EVO.Common.Problem

    'Progress
    Private mProgress As EVO.Common.Progress

    'Apps
    Private Testprobleme1 As EVO.Apps.Testprobleme
    Private WithEvents Sim1 As EVO.Apps.Sim
    Private TSP1 As EVO.Apps.TSP

    'Controller
    Private controller As EVO.IController

    '**** Verschiedenes ****
    Dim isrun As Boolean = False                        'Optimierung läuft
    Dim ispause As Boolean = False                      'Optimierung ist pausiert

    '**** Multithreading ****
    Private n_Threads As Integer                        'Anzahl der Threads

    'Dialoge
    Private WithEvents solutionDialog As SolutionDialog
    Private WithEvents scatterplot1, scatterplot2 As EVO.Diagramm.Scatterplot

    'Diagramme
    Private WithEvents Hauptdiagramm1 As IHWB.EVO.Diagramm.Hauptdiagramm
    Private WithEvents Monitor1 As EVO.Diagramm.Monitor

#End Region 'Eigenschaften

#Region "Methoden"

#Region "UI"

    'Form1 laden
    '***********
    Private Sub Form1_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        'XP-look
        System.Windows.Forms.Application.EnableVisualStyles()

        'Anzahl der möglichen Threads wird ermittelt
        Me.n_Threads = Me.determineNoOfThreads()

        'Monitor zuweisen
        Me.Monitor1 = EVO.Diagramm.Monitor.getInstance()

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
    Private Sub INI()

        Me.IsInitializing = True

        'Anwendungs-Groupbox aktivieren
        Me.GroupBox_Anwendung.Enabled = True

        'Anwendung
        '---------
        'Liste der Anwendungen in ComboBox schreiben und Anfangseinstellung wählen
        Me.ComboBox_Anwendung.Items.Clear()
        Me.ComboBox_Anwendung.Items.AddRange(New Object() {"", ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM, ANW_TESTPROBLEME, ANW_TSP})
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
        Me.ComboBox_Methode.Items.AddRange(New Object() {"", METH_PES, METH_CES, METH_HYBRID, METH_MetaEvo, METH_SENSIPLOT, METH_HOOKJEEVES, METH_DDS})
        Me.ComboBox_Methode.SelectedIndex = 0

        'Einstellungen
        Me.EVO_Einstellungen1.setStandard_All()
        Me.EVO_Einstellungen1.ResetUI()

        'Monitor zurücksetzen
        Me.Monitor1.Reset()

        'Progress instanzieren und an EVO_Opt_Verlauf übergeben
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
        If (Not isNothing(Me.solutionDialog)) Then
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
        If (Me.StopOptimization()) Then
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

    'About Dialog anzeigen
    '*********************
    Private Sub MenuItem_About_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_About.Click
        Dim AboutDialog As New AboutDialog()
        Call AboutDialog.ShowDialog()
    End Sub

    'Wiki aufrufen
    '*************
    Private Sub MenuItem_Wiki_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_Wiki.Click
        Call Process.Start("http://130.83.196.154/BlueM/wiki/index.php/BlueM.Opt")
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
        OpenFileDialog1.Filter = "XML-Dateien (*.xml)|*.xml"
        OpenFileDialog1.FileName = "EVO_Settings.xml"
        OpenFileDialog1.Title = "Einstellungsdatei auswählen"
        If (Not IsNothing(Sim1)) Then
            OpenFileDialog1.InitialDirectory = Sim1.WorkDir_Original
        Else
            OpenFileDialog1.InitialDirectory = CurDir()
        End If

        'Dialog anzeigen
        If (OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) Then

            'EVO_Settings aus Datei laden
            Call EVO_Einstellungen1.loadSettings(OpenFileDialog1.FileName)

        End If

    End Sub

    'EVO_Einstellungen speichern
    '***************************
    Private Sub Einstellungen_Save(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_SettingsSave.Click

        'Dialog einrichten
        SaveFileDialog1.Filter = "XML-Dateien (*.xml)|*.xml"
        SaveFileDialog1.FileName = "EVO_Settings.xml"
        SaveFileDialog1.DefaultExt = "xml"
        SaveFileDialog1.Title = "Einstellungsdatei speichern"
        If (Not IsNothing(Sim1)) Then
            SaveFileDialog1.InitialDirectory = Sim1.WorkDir_Original
        Else
            SaveFileDialog1.InitialDirectory = CurDir()
        End If

        'Dialog anzeigen
        If (SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            Call EVO_Einstellungen1.saveSettings(SaveFileDialog1.FileName)
        End If
    End Sub

#End Region 'UI

#Region "Initialisierung der Anwendungen"

    'Die Anwendung wurde ausgewählt und wird jetzt initialisiert
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    'Anwendung wurde ausgewählt
    '**************************
    Private Sub INI_App(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Anwendung.SelectedIndexChanged

        If (Me.IsInitializing = True) Then

            Exit Sub

        Else

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
            Me.ToolStripMenuItem_ErgebnisDBSave.Enabled = False
            Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = False
            Me.ToolStripButton_Scatterplot.Enabled = False
            Me.ToolStripMenuItem_ErgebnisDBCompare.Enabled = False

            'EVO_Settings zurücksetzen
            Me.EVO_Einstellungen1.isSaved = False

            'Multithreading standardmäßig verbieten
            Me.EVO_Einstellungen1.MultithreadingAllowed = False

            'Mauszeiger busy
            Cursor = Cursors.WaitCursor

            Me.Anwendung = ComboBox_Anwendung.SelectedItem

            Select Case Me.Anwendung

                Case "" 'Keine Anwendung ausgewählt
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'nix

                Case ANW_BLUEM 'Anwendung BlueM
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse BlueM initialisieren
                    Sim1 = New EVO.Apps.BlueM()


                Case ANW_SMUSI 'Anwendung Smusi
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse Smusi initialisieren
                    Sim1 = New EVO.Apps.Smusi()


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
                    Call Me.ComboBox_Methode.Items.AddRange(New String() {"", METH_PES, METH_MetaEvo, METH_HOOKJEEVES, METH_DDS})
                    Me.IsInitializing = False


                Case ANW_TSP 'Anwendung Traveling Salesman Problem (TSP)
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    TSP1 = New EVO.Apps.TSP()

                    Call TSP1.TSP_Initialize(Me.Hauptdiagramm1)

                    'Start-Button aktivieren (keine Methodenauswahl erforderlich)
                    Button_Start.Enabled = True

            End Select

            'Bei Sim-Anwendungen Multithreading vorbereiten
            If (Not IsNothing(Me.Sim1)) Then
                If (Me.Sim1.MultithreadingSupported) Then
                    Me.EVO_Einstellungen1.MultithreadingAllowed = True
                    Call Me.Sim1.prepareThreads(Me.n_Threads)
                End If
            End If

            'Datensatz UI aktivieren
            Call Me.Datensatz_initUI()

            'Progress zurücksetzen
            Call Me.mProgress.Initialize()

            'Mauszeiger wieder normal
            Cursor = Cursors.Default

        End If

    End Sub

    'Datensatz-UI anzeigen
    '*********************
    Private Sub Datensatz_initUI()

        Dim pfad As String

        'UI aktivieren
        Me.Label_Datensatz.Enabled = True
        Me.ComboBox_Datensatz.Enabled = True

        'Tooltip zurücksetzen
        Me.ToolTip1.SetToolTip(Me.ComboBox_Datensatz, "")

        'Combo_Datensatz auffüllen
        Call Me.Datensatz_populateCombo()


        If (Me.Anwendung = ANW_TESTPROBLEME) Then

            'Bei Testproblemen:
            '------------------

            'Browse-Button deaktivieren
            Me.Button_BrowseDatensatz.Enabled = False

        Else
            'Bei Simulationsanwendungen:
            '---------------------------

            'Browse-Button aktivieren
            Me.Button_BrowseDatensatz.Enabled = True

            'zuletzt benutzten Datensatz setzen?
            If (Me.ComboBox_Datensatz.Items.Count > 0) Then
                'obersten (zuletzt genutzten) Datensatz auswählen
                pfad = Me.ComboBox_Datensatz.Items(0)
                Me.ComboBox_Datensatz.SelectedItem = pfad
                'Datensatz setzen
                Cursor = Cursors.WaitCursor
                Call Sim1.setDatensatz(pfad)
                Cursor = Cursors.Default
            End If

        End If

    End Sub

    'Combo_Datensatz auffüllen
    '*************************
    Private Sub Datensatz_populateCombo()

        Dim i As Integer
        Dim pfad As String

        'vorherige Einträge löschen
        Me.ComboBox_Datensatz.Items.Clear()

        Select Case Me.Anwendung

            Case ANW_TESTPROBLEME

                'Mit Testproblemen füllen
                Me.ComboBox_Datensatz.Items.AddRange(Testprobleme1.Testprobleme)

            Case Else '(Sim-Anwendungen)

                'Mit Benutzer-MRUSimDatensätze füllen
                If (My.Settings.MRUSimDatensaetze.Count > 0) Then

                    'Combobox rückwärts füllen
                    For i = My.Settings.MRUSimDatensaetze.Count - 1 To 0 Step -1

                        'nur existierende, zur Anwendung passende Datensätze anzeigen
                        pfad = My.Settings.MRUSimDatensaetze(i)
                        If (File.Exists(pfad) _
                            And Path.GetExtension(pfad).ToUpper() = Me.Sim1.DatensatzExtension) Then
                            Me.ComboBox_Datensatz.Items.Add(My.Settings.MRUSimDatensaetze(i))
                        End If
                    Next

                End If

        End Select

    End Sub

    'Arbeitsverzeichnis/Datensatz auswählen (nur Sim-Anwendungen)
    '************************************************************
    Private Sub Datensatz_browse(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_BrowseDatensatz.Click

        Dim DiagResult As DialogResult
        Dim pfad As String

        'Dialog vorbereiten
        OpenFileDialog1.Filter = Sim1.DatensatzDateiendungen(0) & "-Dateien (*." & Sim1.DatensatzDateiendungen(0) & ")|*." & Sim1.DatensatzDateiendungen(0)
        OpenFileDialog1.Title = "Datensatz auswählen"

        'Alten Datensatz dem Dialog zuweisen
        OpenFileDialog1.InitialDirectory = Sim1.WorkDir_Original
        OpenFileDialog1.FileName = Sim1.WorkDir_Original & Sim1.Datensatz & "." & Sim1.DatensatzDateiendungen(0)

        'Dialog öffnen
        DiagResult = OpenFileDialog1.ShowDialog()

        'Neuen Datensatz speichern
        If (DiagResult = Windows.Forms.DialogResult.OK) Then

            pfad = OpenFileDialog1.FileName

            'Datensatz setzen
            Call Sim1.setDatensatz(pfad)

            'Benutzereinstellungen aktualisieren
            If (My.Settings.MRUSimDatensaetze.Contains(pfad)) Then
                My.Settings.MRUSimDatensaetze.Remove(pfad)
            End If
            My.Settings.MRUSimDatensaetze.Add(pfad)

            'Benutzereinstellungen speichern
            Call My.Settings.Save()

            'Datensatzanzeige aktualisieren
            Call Me.Datensatz_populateCombo()
            Me.ComboBox_Datensatz.SelectedItem = pfad

            'Methodenauswahl wieder zurücksetzen 
            '(Der Benutzer muss zuerst Ini neu ausführen!)
            Me.ComboBox_Methode.SelectedItem = ""

        End If

    End Sub

    'Sim-Datensatz zurücksetzen
    '**************************
    Private Sub Datensatz_reset(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_DatensatzZurücksetzen.Click

        'Original ModellParameter schreiben
        Call Sim1.Write_ModellParameter()

        MsgBox("Die Startwerte der Optimierungsparameter wurden in die Eingabedateien geschrieben.", MsgBoxStyle.Information, "Info")

    End Sub

    'Datensatz wurde ausgewählt
    '**************************
    Private Sub INI_Datensatz(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Datensatz.SelectedIndexChanged

        If (Me.IsInitializing = True) Then

            Exit Sub

        Else

            'Zurücksetzen
            '------------

            'Tooltip
            Me.ToolTip1.SetToolTip(Me.ComboBox_Datensatz, "")

            'Datensatz-Reset
            Me.MenuItem_DatensatzZurücksetzen.Enabled = False

            'gewählten Datensatz an Anwendung übergeben
            '------------------------------------------
            Select Case Me.Anwendung

                Case ANW_TESTPROBLEME

                    'Testproblem setzen
                    Testprobleme1.setTestproblem(Me.ComboBox_Datensatz.SelectedItem)

                    'Tooltip anzeigen
                    Me.ToolTip1.SetToolTip(Me.ComboBox_Datensatz, Testprobleme1.TestProblemDescription)

                Case Else '(Alle Sim-Anwendungen)

                    'Datensatz setzen
                    Call Sim1.setDatensatz(Me.ComboBox_Datensatz.SelectedItem)

                    'Tooltip anzeigen
                    Me.ToolTip1.SetToolTip(Me.ComboBox_Datensatz, Me.ComboBox_Datensatz.SelectedItem)

            End Select

            'Methodenauswahl aktivieren und zurücksetzen
            '-------------------------------------------
            Me.Label_Methode.Enabled = True
            Me.ComboBox_Methode.Enabled = True
            Me.ComboBox_Methode.SelectedItem = ""

            'Progress zurücksetzen
            Call Me.mProgress.Initialize()

        End If

    End Sub

    'Methode wurde ausgewählt
    '************************
    Private Sub INI_Method(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Methode.SelectedIndexChanged

        If (Me.IsInitializing = True Or Me.ComboBox_Methode.SelectedItem = "") Then

            Exit Sub

        Else

            'Mauszeiger busy
            Cursor = Cursors.WaitCursor

            'Problem initialisieren
            '======================
            Call Me.INI_Problem(Me.ComboBox_Methode.SelectedItem)

            'Methodenspezifische Vorbereitungen
            '(zunächst alles deaktivieren, danach je nach Methode aktivieren)
            '================================================================

            'Diagramm zurücksetzen
            Me.Hauptdiagramm1.Reset()

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Toolbar-Buttons deaktivieren
            Me.ToolStripMenuItem_ErgebnisDBSave.Enabled = False
            Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = False
            Me.ToolStripButton_Scatterplot.Enabled = False

            Select Case Me.mProblem.Method

                Case METH_SENSIPLOT 'Methode SensiPlot
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Monitor deaktivieren
                    Me.ToolStripButton_Monitor.Checked = False

                    'TODO: Progress initialisieren


                Case METH_PES 'Methode PES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Ergebnis-Buttons
                    Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = True

                    'TODO: Progress mit Standardwerten initialisieren


                Case METH_HOOKJEEVES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Kontrolle: Nur SO möglich!
                    If (Me.mProblem.Modus = EVO_MODUS.Multi_Objective) Then
                        Throw New Exception("Methode von Hook und Jeeves erlaubt nur Single-Objective Optimierung!")
                    End If

                    'Ergebnis-Buttons
                    Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = True

                    'TODO: Progress mit Standardwerten initialisieren

                Case METH_DDS
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Kontrolle: Nur SO möglich!
                    If (Me.mProblem.Modus = EVO_MODUS.Multi_Objective) Then
                        Throw New Exception("Methode DDS erlaubt nur Single-Objective Optimierung!")
                    End If

                    'Ergebnis-Buttons
                    Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = True

                    'TODO: Progress mit Standardwerten initialisieren


                Case METH_CES, METH_HYBRID 'Methode CES und HYBRID
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Funktioniert nur bei BlueM!
                    If (Not Anwendung = ANW_BLUEM) Then
                        Throw New Exception("CES/HYBRID funktioniert bisher nur mit BlueM!")
                    End If

                    'Ergebnis-Buttons
                    Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = True

                    If (Me.mProblem.Method = METH_HYBRID) Then

                        'Original ModellParameter schreiben
                        Call Sim1.Write_ModellParameter()

                        'Original Transportstrecken einlesen
                        Call CType(Me.Sim1, EVO.Apps.BlueM).SKos1.Read_TRS_Orig_Daten(Sim1.WorkDir_Original)

                    End If

                    'ggf. EVO_Einstellungen Testmodus einrichten
                    '-------------------------------------------
                    'Bei Testmodus wird die Anzahl der Kinder und Generationen überschrieben
                    If Not (Me.mProblem.CES_T_Modus = Common.Constants.CES_T_MODUS.No_Test) Then
                        Call EVO_Einstellungen1.setTestModus(Me.mProblem.CES_T_Modus, Sim1.TestPath, 1, 1, Me.mProblem.NumCombinations)
                    End If

                    'TODO: Progress mit Standardwerten initialisieren

                Case METH_MetaEvo
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Ergebnis-Buttons
                    Me.ToolStripMenuItem_ErgebnisDBLoad.Enabled = True

                    'Progress mit Standardwerten initialisieren
                    Call Me.mProgress.Initialize(1, 1, EVO_Einstellungen1.Settings.MetaEvo.NumberGenerations, EVO_Einstellungen1.Settings.MetaEvo.PopulationSize)

            End Select

            'Toolbar-Buttons aktivieren
            Me.ToolStripSplitButton_Diagramm.Enabled = True
            Me.ToolStripSplitButton_ErgebnisDB.Enabled = True
            Me.ToolStripSplitButton_Settings.Enabled = True

            'IniMethod OK -> Start Button aktivieren
            Me.Button_Start.Enabled = True

            If (Me.Anwendung <> ANW_TESTPROBLEME) Then
                'Datensatz-Reset aktivieren
                Me.MenuItem_DatensatzZurücksetzen.Enabled = True
            End If

            'Mauszeiger wieder normal
            Cursor = Cursors.Default

        End If

    End Sub

    ''' <summary>
    ''' Problem initialisieren und überall bekannt machen
    ''' </summary>
    ''' <param name="Method">gewählte Methode</param>
    Private Sub INI_Problem(ByVal Method As String)

        'Neues Problem mit ausgewählter Methode instanzieren
        Me.mProblem = New EVO.Common.Problem(Method)

        'Problemdefinition
        '=================
        If (Me.Anwendung <> ANW_TESTPROBLEME And Me.Anwendung <> ANW_TSP) Then

            'Bei allen Sim-Anwendungen
            '-------------------------

            'WorkDir und Datensatz übergeben
            Me.mProblem.WorkDir = Sim1.WorkDir_Original
            Me.mProblem.Datensatz = Sim1.Datensatz

            'EVO-Eingabedateien einlesen
            Call Me.mProblem.Read_InputFiles(Me.Sim1.SimStart, Me.Sim1.SimEnde)

            'Problem an Sim-Objekt übergeben
            Call Me.Sim1.setProblem(Me.mProblem)

        ElseIf (Me.Anwendung = ANW_TESTPROBLEME) Then

            'Bei Testproblemen definieren diese das Problem selbst
            '-----------------------------------------------------
            Call Testprobleme1.getProblem(Me.mProblem)

        End If

        'Problem an EVO_Einstellungen übergeben
        '--------------------------------------
        Call Me.EVO_Einstellungen1.Initialise(Me.mProblem)

        'Individuumsklasse mit Problem initialisieren
        '--------------------------------------------
        Call EVO.Common.Individuum.Initialise(Me.mProblem)

    End Sub

#End Region 'Initialisierung der Anwendungen

#Region "Start Button Pressed"

    'Start BUTTON wurde pressed
    'XXXXXXXXXXXXXXXXXXXXXXXXXX

    Private Sub STARTEN_Button_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Button_Start.Click

        'Stoppuhr
        Dim OptTime As New Stopwatch
        OptTime.Start()

        If (Me.isrun And Not Me.ispause) Then
            'Optimierung pausieren
            '---------------------
            Me.ispause = True
            Me.Button_Start.Text = "Continue"

            'Bei Multithreading muss Sim explizit pausiert werden
            If (Me.EVO_Einstellungen1.Settings.General.useMultithreading) Then
                Me.Sim1.isPause = True
            End If

            Do While (Me.ispause)
                System.Threading.Thread.Sleep(20)
                Application.DoEvents()
            Loop

        ElseIf (Me.isrun) Then
            'Optimierung weiterlaufen lassen
            '-------------------------------
            Me.ispause = False
            Me.Button_Start.Text = "Pause"

            'Bei Multithreading muss Sim explizit wieder gestartet werden
            If (Me.EVO_Einstellungen1.Settings.General.useMultithreading) Then
                Me.Sim1.isPause = False
            End If

        Else
            'Optimierung starten
            '-------------------
            Me.isrun = True
            Me.Button_Start.Text = "Pause"
            Me.Button_Stop.Enabled = True

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

            'EVO_Settings in temp-Verzeichnis speichern
            Dim dir As String
            dir = My.Computer.FileSystem.SpecialDirectories.Temp & "\"
            Call Me.EVO_Einstellungen1.saveSettings(dir & "EVO_Settings.xml")

            'EVO_Settings deaktivieren
            Call Me.EVO_Einstellungen1.freeze()

            'EVO_Settings an Hauptdiagramm übergeben
            Call Me.Hauptdiagramm1.setSettings(Me.EVO_Einstellungen1.Settings)

            'Diagramm vorbereiten und initialisieren
            Call Me.PrepareDiagramm()

            Select Case Anwendung

                Case ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM

                    'Settings an Sim1 übergeben
                    Call Me.Sim1.setSettings(Me.EVO_Einstellungen1.Settings)

                    'Startwert evaluieren
                    Call Me.evaluateStartwerte()

                    Select Case Me.mProblem.Method

                        Case METH_SENSIPLOT
                            'SensiPlot-Controller initialisieren und starten
                            controller = New EVO.SensiPlot.Controller()

                        Case METH_PES, METH_CES, METH_HYBRID
                            'ES-Controller initialisieren und starten
                            controller = New EVO.ES.Controller()

                        Case METH_MetaEvo
                            'MetaEVO-Controller initialisieren und starten
                            controller = New EVO.MetaEvo.Controller()

                        Case METH_HOOKJEEVES
                            'HJ-Controller initialisieren und starten
                            controller = New EVO.HookeAndJeeves.Controller()

                        Case METH_DDS
                            'DDS-Controller initialisieren und starten
                            controller = New modelEAU.DDS.Controller()

                    End Select

                    'Controller für Sim initialisieren und starten
                    Call controller.Init(Me.mProblem, Me.EVO_Einstellungen1.Settings, Me.mProgress, Me.Hauptdiagramm1)
                    Call controller.InitApp(Me.Sim1)
                    Call controller.Start()

                Case ANW_TESTPROBLEME

                    Select Case Me.mProblem.Method
                        Case METH_PES
                            'ES-Controller instanzieren
                            controller = New EVO.ES.Controller()

                        Case METH_HOOKJEEVES
                            'HJ-Controller instanzieren
                            controller = New EVO.HookeAndJeeves.Controller()

                        Case METH_DDS
                            'DDS-Controller instanzieren
                            controller = New modelEAU.DDS.Controller()

                        Case METH_MetaEvo
                            'MetaEVO-Controller instanzieren
                            controller = New EVO.MetaEvo.Controller()

                        Case Else
                            Throw New Exception("Testprobleme können mit der Methode " & Me.mProblem.Method & " nicht ausgeführt werden!")
                    End Select

                    'Controller für Testproblem initialisieren und starten
                    Call controller.Init(Me.mProblem, Me.EVO_Einstellungen1.Settings, Me.mProgress, Me.Hauptdiagramm1)
                    Call controller.InitApp(Me.Testprobleme1)
                    Call controller.Start()


                Case ANW_TSP
                    Call STARTEN_TSP()

            End Select

            ''Globale Fehlerbehandlung für Optimierungslauf:
            'Catch ex As Exception
            '    MsgBox(ex.Message, MsgBoxStyle.Critical, "Fehler")
            'End Try

            'Optimierung beendet
            '-------------------
            Me.isrun = False
            Me.Button_Start.Text = "Start"
            Me.Button_Start.Enabled = False
            Me.Button_Stop.Enabled = False

            'Ausgabe der Optimierungszeit
            OptTime.Stop()
            MsgBox("Die Optimierung dauerte:   " & OptTime.Elapsed.Hours & "h  " & OptTime.Elapsed.Minutes & "m  " & OptTime.Elapsed.Seconds & "s     " & OptTime.Elapsed.Seconds & "ms", MsgBoxStyle.Information)

        End If

    End Sub

    ''' <summary>
    ''' Die Startwerte der Optparameter evaluieren
    ''' </summary>
    ''' <remarks>nur für Sim-Anwendungen!</remarks>
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

    'Anwendung Traveling Salesman - Start                         
    '************************************
    Private Sub STARTEN_TSP()

        'Laufvariable für die Generationen
        Dim gen As Integer

        'BUG 212: Nach Klasse Diagramm auslagern!
        Call TSP1.TeeChart_Initialise_TSP(Me.Hauptdiagramm1)

        'Arrays werden Dimensioniert
        Call TSP1.Dim_Parents_TSP()
        Call TSP1.Dim_Childs()

        'Zufällige Kinderpfade werden generiert
        Call TSP1.Generate_Random_Path_TSP()

        'Generationsschleife
        For gen = 1 To TSP1.n_Gen

            'Den Kindern werden die Städte Ihres Pfades entsprechend zugewiesen
            Call TSP1.Cities_according_ChildPath()

            'Bestimmung des der Qualität der Kinder
            Call TSP1.Evaluate_child_Quality()

            'Sortieren der Kinden anhand der Qualität
            Call TSP1.Sort_Faksimile(TSP1.ChildList)

            'Selections Prozess (Übergabe der Kinder an die Eltern je nach Strategie)
            Call TSP1.Selection_Process()

            'Zeichnen des besten Elter
            'TODO: funzt nur, wenn ganz am ende gezeichnet wird
            If gen = TSP1.n_Gen Then
                Call TSP1.TeeChart_Zeichnen_TSP(Me.Hauptdiagramm1, TSP1.ParentList(0).Image)
            End If

            'Kinder werden Hier vollständig gelöscht
            Call TSP1.Reset_Childs()

            'Reproduktionsoperatoren, hier gehts dezent zur Sache
            Call TSP1.Reproduction_Control()

            'Mutationsoperatoren
            Call TSP1.Mutation_Control()

        Next gen

    End Sub

#End Region 'Start Button Pressed

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
        'Zunächst keine Achsenzuordnung (-1)
        For i = 0 To tmpZielindex.GetUpperBound(0)
            tmpZielindex(i) = -1
        Next

        'Fallunterscheidung Anwendung/Methode
        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        Select Case Anwendung

            Case ANW_TESTPROBLEME 'Testprobleme
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                Call Testprobleme1.DiagInitialise(Me.Hauptdiagramm1)

            Case ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                Select Case Me.mProblem.Method

                    Case METH_SENSIPLOT 'SensiPlot
                        'XXXXXXXXXXXXXXXXXXXXXXXXX

                        If (Me.EVO_Einstellungen1.Settings.SensiPlot.Selected_OptParameters.GetLength(0) = 1) Then

                            '1 OptParameter:
                            '---------------

                            'Achsen:
                            '-------
                            'X-Achse = QWert
                            Achse.Title = Me.mProblem.List_ObjectiveFunctions(Me.EVO_Einstellungen1.Settings.SensiPlot.Selected_Objective).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)
                            'Y-Achse = OptParameter
                            Achse.Title = Me.mProblem.List_OptParameter(Me.EVO_Einstellungen1.Settings.SensiPlot.Selected_OptParameters(0)).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)

                            'Achsenzuordnung
                            'BUG 327!
                            For i = 0 To Me.mProblem.NumObjectives - 1
                                If (Me.mProblem.List_ObjectiveFunctions(i).Bezeichnung = Me.mProblem.List_ObjectiveFunctions(Me.EVO_Einstellungen1.Settings.SensiPlot.Selected_Objective).Bezeichnung) Then
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
                            Achse.Title = Me.mProblem.List_OptParameter(Me.EVO_Einstellungen1.Settings.SensiPlot.Selected_OptParameters(0)).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)
                            'Y-Achse = Objective
                            Achse.Title = Me.mProblem.List_ObjectiveFunctions(Me.EVO_Einstellungen1.Settings.SensiPlot.Selected_Objective).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)
                            'Z-Achse = OptParameter2
                            Achse.Title = Me.mProblem.List_OptParameter(Me.EVO_Einstellungen1.Settings.SensiPlot.Selected_OptParameters(1)).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)

                            'Achsenzuordnung
                            'BUG 327!
                            Me.Hauptdiagramm1.ZielIndexX = -1
                            For i = 0 To Me.mProblem.NumObjectives - 1
                                If (Me.mProblem.List_ObjectiveFunctions(i).Bezeichnung = Me.mProblem.List_ObjectiveFunctions(Me.EVO_Einstellungen1.Settings.SensiPlot.Selected_Objective).Bezeichnung) Then
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
                                If (EVO_Einstellungen1.Settings.PES.Pop.is_POPUL) Then
                                    Achse.Maximum = EVO_Einstellungen1.Settings.PES.n_Gen * EVO_Einstellungen1.Settings.PES.n_Nachf * EVO_Einstellungen1.Settings.PES.Pop.n_Runden + 1
                                Else
                                    Achse.Maximum = EVO_Einstellungen1.Settings.PES.n_Gen * EVO_Einstellungen1.Settings.PES.n_Nachf + 1
                                End If

                            ElseIf (Me.mProblem.Method = METH_MetaEvo) Then
                                'Bei MetaEvo:
                                Achse.Maximum = EVO_Einstellungen1.Settings.MetaEvo.NumberGenerations * EVO_Einstellungen1.Settings.MetaEvo.ChildsPerParent * EVO_Einstellungen1.Settings.MetaEvo.PopulationSize

                            ElseIf (Me.mProblem.Method = METH_HOOKJEEVES) Then
                                'Bei Hooke & Jeeves:
                                Achse.Automatic = True

                            ElseIf (Me.mProblem.Method = METH_DDS) Then
                                'Bei DDS:
                                Achse.Automatic = True

                            Else
                                'Bei CES etc.:
                                Achse.Maximum = EVO_Einstellungen1.Settings.CES.n_Childs * EVO_Einstellungen1.Settings.CES.n_Generations
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
                                MsgBox("Die Anzahl der Penalty-Funktionen beträgt mehr als 3!" & eol _
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
            MsgBox("Es werden bereits 2 Scatterplot-Matrizen angezeigt" & eol & "Bitte zuerst eine schließen!", MsgBoxStyle.Information)
        End If

        Cursor = Cursors.Default

    End Sub

#Region "Lösungsauswahl"

    'Klick auf Serie in Diagramm
    '***************************
    Public Sub seriesClick(ByVal sender As Object, ByVal s As Steema.TeeChart.Styles.Series, ByVal valueIndex As Integer, ByVal e As System.Windows.Forms.MouseEventArgs)

        'Notwendige Bedingungen überprüfen
        '---------------------------------
        If (IsNothing(Sim1)) Then
            'Anwendung != Sim
            MsgBox("Lösungsauswahl funktioniert nur bei Simulationsanwendungen!", MsgBoxStyle.Information, "Info")
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
                MsgBox("Lösung nicht auswählbar!", MsgBoxStyle.Information)
            End Try

        End If

    End Sub

    'Eine Lösung auswählen
    '*********************
    Private Sub selectSolution(ByVal ind As Common.Individuum) Handles scatterplot1.pointSelected, scatterplot2.pointSelected

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
            Call Me.Hauptdiagramm1.ZeichneAusgewählteLösung(ind)

            'Lösung in den Scatterplots anzeigen
            If (Not IsNothing(Me.scatterplot1)) Then
                Call Me.scatterplot1.showSelectedSolution(ind)
            End If
            If (Not IsNothing(Me.scatterplot2)) Then
                Call Me.scatterplot2.showSelectedSolution(ind)
            End If
        End If

        'Lösungsdialog nach vorne bringen
        Call Me.solutionDialog.BringToFront()

    End Sub

    'Lösungsauswahl zurücksetzen
    '***************************
    Public Sub clearSelection()

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

        'Auswahl intern zurücksetzen
        '===========================
        Call Sim1.OptResult.clearSelectedSolutions()

    End Sub

    'ausgewählte Lösungen simulieren und in Wave anzeigen
    '****************************************************
    Public Sub showWave(ByVal checkedSolutions As Collection)

        Dim isOK As Boolean
        Dim isIHA As Boolean
        Dim isSWMM As Boolean
        Dim WorkDir_Prev As String

        Dim zre As Wave.Zeitreihe
        Dim SimSeries As New Collection                 'zu zeichnende Simulationsreihen
        Dim RefSeries As New Collection                 'zu zeichnende Referenzreihen

        'Wait cursor
        Cursor = Cursors.WaitCursor

        'Simulationen in Originalverzeichnis ausführen (ohne Threads),
        'WorDir_Current aber merken, und am Ende wieder zurücksetzen!
        WorkDir_Prev = Sim1.WorkDir_Current
        Sim1.WorkDir_Current = Sim1.WorkDir_Original

        'Wave instanzieren
        Dim Wave1 As New Wave.Wave()

        'Sonderfall BlueM mit IHA-Berechnung 
        'ein 2. Wave für RVA-Diagramme instanzieren
        Dim Wave2 As Wave.Wave = Nothing
        If (TypeOf Me.Sim1 Is EVO.Apps.BlueM) Then
            If (CType(Me.Sim1, EVO.Apps.BlueM).isIHA) Then
                isIHA = True
                Wave2 = New Wave.Wave()
                Call Wave2.PrepareChart_RVA()
                'IHA-Vergleichsmodus?
                If (CType(Me.Sim1, EVO.Apps.BlueM).IHAProc.isComparison) Then
                    'Referenz-RVAErgebnis in Wave2 laden
                    Call Wave2.Display_RVA(CType(Me.Sim1, EVO.Apps.BlueM).IHAProc.RVABase)
                End If
            End If
        End If

        'Alle ausgewählten Lösungen durchlaufen
        '======================================
        For Each ind As Common.Individuum In Sim1.OptResult.getSelectedSolutions()

            'Lösung per Checkbox ausgewählt?
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

            'Sonderfall IHA-Berechnung
            If (isIHA) Then
                'RVA-Ergebnis in Wave2 laden
                Dim RVAResult As Wave.RVA.Struct_RVAValues
                RVAResult = CType(Me.Sim1, EVO.Apps.BlueM).IHASys.RVAResult
                'Lösungsnummer an Titel anhängen
                RVAResult.Title = "Lösung " & ind.ID.ToString()
                Call Wave2.Display_RVA(RVAResult)
            End If

            'Zu zeichnenden Simulationsreihen zurücksetzen
            SimSeries.Clear()

            'zu zeichnenden Reihen aus Liste der Ziele raussuchen
            '----------------------------------------------------
            For Each objective As Common.ObjectiveFunction In Me.mProblem.List_ObjectiveFunctions
                If Not objective.isGroupLeader Then
                    With objective

                        'Referenzreihe in Wave laden
                        '---------------------------
                        'BUG 414: TODO: IHA
                        If (objective.GetObjType = Common.ObjectiveFunction.ObjectiveType.Series) Then
                            'Dim Objective_Series As Common.ObjectiveFunction_Series = CType(objective, Common.ObjectiveFunction_Series)
                            'Referenzreihen nur jeweils ein Mal zeichnen
                            With CType(objective, Common.ObjectiveFunction_Series)
                                If (Not RefSeries.Contains(.RefReiheDatei & .RefGr)) Then
                                    RefSeries.Add(.RefGr, .RefReiheDatei & .RefGr)
                                    'Referenzreihe in Wave laden
                                    Wave1.Display_Series(.RefReihe)
                                End If
                            End With
                        End If

                        'Simulationsergebnis in Wave laden
                        '---------------------------------
                        'Simulationsreihen nur jeweils ein Mal zeichnen
                        If (Not SimSeries.Contains(.SimGr)) Then
                            Call SimSeries.Add(.SimGr, .SimGr)
                            zre = Sim1.SimErgebnis(.SimGr).Clone()
                            'Lösungsnummer an Titel anhängen
                            zre.Title &= " (Lösung " & ind.ID.ToString() & ")"
                            'Simreihe in Wave laden
                            Call Wave1.Display_Series(zre)
                        End If

                    End With
                End If
            Next

        Next ind

        'Wave anzeigen
        '-------------
        Call Wave1.Show()
        If (Not IsNothing(Wave2)) Then Call Wave2.Show()

        'Cursor
        Cursor = Cursors.Default

        'Simulationsverzeichnis zurücksetzen
        Sim1.WorkDir_Current = WorkDir_Prev

    End Sub

#End Region 'Lösungsauswahl

#End Region 'Diagrammfunktionen

#Region "Ergebnisdatenbank"

    'Ergebnis-Button hat selbst keine funktionalität -> nur DropDown
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

        'Datei-öffnen Dialog anzeigen
        Me.OpenFileDialog1.Filter = "Access-Datenbanken (*.mdb)|*.mdb"
        Me.OpenFileDialog1.Title = "Ergebnisdatenbank auswählen"
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

                    'Lösungen
                    '========
                    If (importDialog.ComboBox_SekPop.SelectedItem <> "ausschließlich") Then

                        For Each ind As Common.Individuum In Sim1.OptResult.Solutions

                            If (Me.Hauptdiagramm1.ZielIndexZ = -1 And Me.Hauptdiagramm1.ZielIndexY = -1) Then
                                '1D
                                '--
                                'Constraintverletzung prüfen
                                If (ind.Is_Feasible) Then
                                    serie = Me.Hauptdiagramm1.getSeriesPoint("Population", "red")
                                Else
                                    serie = Me.Hauptdiagramm1.getSeriesPoint("Population (ungültig)", "Gray")
                                End If
                                'Zeichnen
                                serie.Add(ind.ID, ind.Objectives(Me.Hauptdiagramm1.ZielIndexX), ind.ID.ToString())
                            ElseIf (Me.Hauptdiagramm1.ZielIndexZ = -1) Then
                                '2D
                                '--
                                'Constraintverletzung prüfen
                                If (ind.Is_Feasible) Then
                                    serie = Me.Hauptdiagramm1.getSeriesPoint("Population", "Orange")
                                Else
                                    serie = Me.Hauptdiagramm1.getSeriesPoint("Population (ungültig)", "Gray")
                                End If
                                'Zeichnen
                                serie.Add(ind.Objectives(Me.Hauptdiagramm1.ZielIndexX), ind.Objectives(Me.Hauptdiagramm1.ZielIndexY), ind.ID.ToString())
                            Else
                                '3D
                                '--
                                'Constraintverletzung prüfen
                                If (ind.Is_Feasible) Then
                                    serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Population", "Orange")
                                Else
                                    serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Population (ungültig)", "Gray")
                                End If
                                'Zeichnen
                                serie3D.Add(ind.Objectives(Me.Hauptdiagramm1.ZielIndexX), ind.Objectives(Me.Hauptdiagramm1.ZielIndexY), ind.Objectives(Me.Hauptdiagramm1.ZielIndexZ), ind.ID.ToString())
                            End If

                        Next

                    End If

                    Call My.Application.DoEvents()

                    'Sekundärpopulation
                    '==================
                    If (importDialog.ComboBox_SekPop.SelectedItem <> "keine") Then

                        For Each sekpopind As Common.Individuum In Sim1.OptResult.getSekPop()
                            If (Me.Hauptdiagramm1.ZielIndexZ = -1) Then
                                '2D
                                '--
                                serie = Me.Hauptdiagramm1.getSeriesPoint("Sekundäre Population", "Green")
                                serie.Add(sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexX), sekpopind.Objectives(Me.Hauptdiagramm1.ZielIndexY), sekpopind.ID.ToString())
                            Else
                                '3D
                                '--
                                serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Sekundäre Population", "Green")
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
        Dim isOK As Boolean

        'Datei-öffnen Dialog anzeigen
        Me.OpenFileDialog1.Filter = "Access-Datenbanken (*.mdb)|*.mdb"
        Me.OpenFileDialog1.Title = "Vergleichsergebnis: Ergebnisdatenbank auswählen"
        Me.OpenFileDialog1.FileName = ""
        Me.OpenFileDialog1.InitialDirectory = Sim1.WorkDir_Original
        diagresult = Me.OpenFileDialog1.ShowDialog()

        If (diagresult = Windows.Forms.DialogResult.OK) Then

            sourceFile = Me.OpenFileDialog1.FileName

            'Cursor Wait
            Cursor = Cursors.WaitCursor

            'Daten einlesen
            '==============
            Sim1.OptResultRef = New EVO.OptResult.OptResult(Me.Sim1.Datensatz, Me.mProblem, False)
            isOK = Sim1.OptResultRef.db_load(sourceFile)

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

                    'Nadir-Punkt holen (für spätere Verwendung bei Referenz-Hypervolumen)
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
    ''' Ermittelt basierend auf der Anzahl der physikalischen Prozessoren die Anzahl zu verwendender Threads
    ''' </summary>
    ''' <remarks></remarks>
    Private Function determineNoOfThreads() As Integer

        Dim LogCPU As Integer = 0
        Dim n_Threads As Integer
        LogCPU = Environment.ProcessorCount

        If LogCPU = 1 Then
            n_Threads = 4
        ElseIf LogCPU = 2 Then
            n_Threads = 5
        ElseIf LogCPU = 4 Then
            n_Threads = 9
        End If

        Return n_Threads

    End Function

    ''' <summary>
    ''' Die Optimierung stoppen
    ''' </summary>
    ''' <returns>True wenn gestoppt</returns>
    Private Function StopOptimization() As Boolean

        Dim res As MsgBoxResult
        If (Me.isrun And Not IsNothing(Me.controller)) Then

            res = MsgBox("Optimierung wirklich abbrechen?", MsgBoxStyle.YesNo)

            If (res = MsgBoxResult.Yes) Then
                'Pause ausschalten, sonst läuft die immer weiter
                Me.ispause = False
                'Controller stoppen
                Call Me.controller.Stoppen()
                Me.controller = Nothing
                'bei Multithreading Sim explizit stoppen
                If (Me.EVO_Einstellungen1.Settings.General.useMultithreading) Then
                    Me.Sim1.isStopped = True
                End If
            Else
                'doch nicht stoppen
                Return False
            End If

        End If
        Return True

    End Function

    ''' <summary>
    ''' Stop-Button wurde geklickt
    ''' </summary>
    Private Sub Button_Stop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Stop.Click

        'Optimierung stoppen
        If (Me.StopOptimization()) Then
            Me.Button_Stop.Enabled = False
            Me.Button_Start.Text = "Start"
        End If

    End Sub

    ''' <summary>
    ''' Das Form wird geschlossen
    ''' </summary>
    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        'Optimierung stoppen
        If (Not Me.StopOptimization()) Then
            'FormClosing abbrechen
            e.Cancel = True
        End If
    End Sub

#End Region 'Methoden

End Class
