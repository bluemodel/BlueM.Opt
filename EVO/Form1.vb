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

    'Optionen
    Private Options As OptionsDialog

    'Anwendung
    Private Anwendung As String

    'Problem
    Private mProblem As EVO.Common.Problem

    'Progress
    Private mProgress As EVO.Common.Progress

    'Apps
    Private Testprobleme1 As EVO.Apps.Testprobleme
    Friend WithEvents Sim1 As EVO.Apps.Sim
    Private SensiPlot1 As EVO.Apps.SensiPlot
    Private TSP1 As EVO.Apps.TSP

    'Methoden
    Private CES1 As EVO.ES.CES

    '**** Globale Parameter Parameter Optimierung ****
    'TODO: diese Werte sollten eigentlich nur in CES bzw PES vorgehalten werden
    Dim array_x() As Double
    Dim array_y() As Double

    '**** Verschiedenes ****
    Dim isrun As Boolean = False                        'Optimierung läuft
    Dim ispause As Boolean = False                      'Optimierung ist pausiert

    '**** Multithreading ****
    Dim SIM_Eval_is_OK As Boolean
    Private n_Threads As Integer                        'Anzahl der Threads
    Dim MI_Thread_OK As Boolean = False

    'Dialoge
    Private WithEvents solutionDialog As SolutionDialog
    Private WithEvents scatterplot1, scatterplot2 As EVO.Diagramm.Scatterplot

    'Diagramme
    Private WithEvents Hauptdiagramm1 As IHWB.EVO.Diagramm.Hauptdiagramm
    Private WithEvents Monitor1 As EVO.Diagramm.Monitor

#End Region 'Eigenschaften

#Region "Methoden"

#Region "UI"

    'Initialisierung von Form1
    '*************************
    Private Sub Form1_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        'XP-look
        System.Windows.Forms.Application.EnableVisualStyles()

        'Anzahl der möglichen Threads wird ermittelt
        Me.n_Threads = Me.determineNoOfThreads()

        'Liste der Anwendungen in ComboBox schreiben und Anfangseinstellung wählen
        ComboBox_Anwendung.Items.AddRange(New Object() {"", ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM, ANW_TESTPROBLEME, ANW_TSP})
        ComboBox_Anwendung.SelectedIndex = 0

        'Liste der Methoden in ComboBox schreiben und Anfangseinstellung wählen
        ComboBox_Methode.Items.AddRange(New Object() {"", METH_PES, METH_CES, METH_HYBRID, METH_SENSIPLOT, METH_HOOKJEEVES, METH_DDS})
        ComboBox_Methode.SelectedIndex = 0

        'OptionsDialog instanzieren
        Me.Options = New OptionsDialog(Me.EVO_Einstellungen1.Settings)

        'Monitor instanzieren
        Me.Monitor1 = New EVO.Diagramm.Monitor()

        'Progress instanzieren und an EVO_Opt_Verlauf übergeben
        Me.mProgress = New EVO.Common.Progress()
        Me.EVO_Opt_Verlauf1.Initialisieren(Me.mProgress)

        'Handler für Klick auf Serien zuweisen
        AddHandler Me.Hauptdiagramm1.ClickSeries, AddressOf seriesClick

        'Ende der Initialisierung
        IsInitializing = False

    End Sub

    'Optionen Dialog anzeigen
    '************************
    Private Sub showOptionsDialog(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_Optionen.Click
        If (Me.isrun) Then
            Call Me.Options.DisableAll()
        Else
            Call Me.Options.EnableAll()
        End If
        Call Me.Options.ShowDialog()
    End Sub

    'Monitor anzeigen
    '****************
    Private Sub MenuItem_MonitorAnzeigen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_MonitorAnzeigen.Click

        If (Me.MenuItem_MonitorAnzeigen.Checked) Then
            Me.Monitor1.Show()
        Else
            Me.Monitor1.Hide()
        End If

    End Sub

    'Wenn Monitor geschlossen wird, Menüeintrag aktualisieren
    Private Sub MonitorClosed() Handles Monitor1.MonitorClosed
        Me.MenuItem_MonitorAnzeigen.Checked = False
    End Sub

    'About Dialog anzeigen
    '*********************
    Private Sub MenuItem_About_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_About.Click
        Dim AboutDialog As New AboutDialog()
        Call AboutDialog.ShowDialog()
    End Sub

    'EVO.NET Wiki aufrufen
    '*********************
    Private Sub MenuItem_Wiki_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_Wiki.Click
        Call Process.Start("http://130.83.196.154/BlueM/wiki/index.php/EVO.NET")
    End Sub

#End Region 'UI

#Region "Initialisierung der Anwendungen"

    'Die Anwendung wurde ausgewählt und wird jetzt initialisiert
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    'Anwendung wurde ausgewählt
    '**************************
    Friend Sub INI_App(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Anwendung.SelectedIndexChanged

        If (Me.IsInitializing = True) Then

            Exit Sub

        Else

            'Diagramm zurücksetzen
            Me.Hauptdiagramm1.Reset()

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

            'Ergebnis-Buttons
            Me.Button_saveMDB.Enabled = False
            Me.Button_openMDB.Enabled = False
            Me.Button_Scatterplot.Enabled = False
            Me.Button_loadRefResult.Enabled = False

            'EVO_Settings zurücksetzen
            Me.EVO_Einstellungen1.Enabled = False
            Me.EVO_Einstellungen1.isSaved = False

            'Multithreading verbieten
            Me.Options.MultithreadingAllowed = False

            'Mauszeiger busy
            Cursor = Cursors.WaitCursor

            Me.Anwendung = ComboBox_Anwendung.SelectedItem

            Select Case Me.Anwendung

                Case "" 'Keine Anwendung ausgewählt
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Mauszeiger wieder normal
                    Cursor = Cursors.Default
                    Exit Sub


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

                    'HACK: bei Testproblemen als Methodenauswahl nur PES zulassen!
                    Me.IsInitializing = True
                    Call Me.ComboBox_Methode.Items.Clear()
                    Call Me.ComboBox_Methode.Items.AddRange(New String() {"", METH_PES})
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
                    Me.Options.MultithreadingAllowed = True
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

        'Bei Simulationsanwendungen:
        If (Me.Anwendung <> ANW_TESTPROBLEME) Then

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

            'Browse-Button aktivieren
            Me.Button_BrowseDatensatz.Enabled = True
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

            'Ergebnis-Buttons deaktivieren
            Me.Button_saveMDB.Enabled = False
            Me.Button_openMDB.Enabled = False
            Me.Button_Scatterplot.Enabled = False

            Select Case Me.mProblem.Method

                Case METH_SENSIPLOT 'Methode SensiPlot
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'SensiPlot instanzieren
                    SensiPlot1 = New EVO.Apps.SensiPlot(Me.mProblem)

                    'Monitor deaktivieren
                    Me.MenuItem_MonitorAnzeigen.Checked = False

                    'SensiPlot Dialog anzeigen:
                    Dim SensiPlotDiagResult As Windows.Forms.DialogResult
                    SensiPlotDiagResult = SensiPlot1.ShowDialog()
                    If (Not SensiPlotDiagResult = Windows.Forms.DialogResult.OK) Then
                        'Mauszeiger wieder normal
                        Cursor = Cursors.Default
                        Exit Sub
                    End If

                    'TODO: Progress initialisieren


                Case METH_PES 'Methode PES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Ergebnis-Buttons
                    Me.Button_openMDB.Enabled = True

                    'TODO: Progress mit Standardwerten initialisieren


                Case METH_HOOKJEEVES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Kontrolle: Nur SO möglich!
                    If (Me.mProblem.Modus = EVO_MODUS.Multi_Objective) Then
                        Throw New Exception("Methode von Hook und Jeeves erlaubt nur Single-Objective Optimierung!")
                    End If

                    'Ergebnis-Buttons
                    Me.Button_openMDB.Enabled = True

                    'TODO: Progress mit Standardwerten initialisieren

                Case METH_DDS
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Kontrolle: Nur SO möglich!
                    If (Me.mProblem.Modus = EVO_MODUS.Multi_Objective) Then
                        Throw New Exception("Methode DDS erlaubt nur Single-Objective Optimierung!")
                    End If

                    'Ergebnis-Buttons
                    Me.Button_openMDB.Enabled = True

                    'TODO: Progress mit Standardwerten initialisieren


                Case METH_CES, METH_HYBRID 'Methode CES und HYBRID
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Funktioniert nur bei BlueM!
                    If (Not Anwendung = ANW_BLUEM) Then
                        Throw New Exception("CES/HYBRID funktioniert bisher nur mit BlueM!")
                    End If

                    'Ergebnis-Buttons
                    Me.Button_openMDB.Enabled = True

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


                Case METH_Hybrid2008
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    If (Me.Anwendung = ANW_TESTPROBLEME) Then
                        'Testprobleme mit Hybrid2008 Verfahren berechnen
                        MsgBox("Berechnung der Testprobleme mit Hybrid2008", MsgBoxStyle.Information, "Info")

                    Else
                        'Modelle mit Hybrid2008 berechnen
                        MsgBox("Berechnung der Modelle mit Hybrid2008", MsgBoxStyle.Information, "Info")

                    End If

                    'Ergebnis-Buttons
                    'Me.Button_openMDB.Enabled = True

                    'Progress mit Standardwerten initialisieren
                    'Call Me.mProgress.Initialize(1, 1, EVO_Einstellungen1.Settings.MetaEvo.NumberGenerations, EVO_Einstellungen1.Settings.MetaEvo.PopulationSize)

            End Select

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

    'EVO_Einstellungen laden
    '***********************
    Friend Sub Load_EVO_Settings(ByVal sender As Object, ByVal e As System.EventArgs)

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

            'EVO_Settings neu verteilen, 
            'weil durch Einlesen aus Datei alle Referenzen verloren gehen
            '------------------------------------------------------------

            'OptionsDialog
            Call Me.Options.setSettings(Me.EVO_Einstellungen1.Settings)
            'Hauptdiagramm
            Call Me.Hauptdiagramm1.setSettings(Me.EVO_Einstellungen1.Settings)

            'Anwendungen, Controller und Algos bekommen die Settings bei Start übergeben
        End If

    End Sub

    'EVO_Einstellungen speichern
    '***************************
    Friend Sub Save_EVO_Settings(ByVal sender As Object, ByVal e As System.EventArgs)

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

#End Region 'Initialisierung der Anwendungen

#Region "Start Button Pressed"

    'Start BUTTON wurde pressed
    'XXXXXXXXXXXXXXXXXXXXXXXXXX

    Private Sub STARTEN_Button_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Button_Start.Click

        If (Me.isrun And Not Me.ispause) Then
            'Optimierung pausieren
            '---------------------
            Me.ispause = True
            Me.Button_Start.Text = "Run"

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

            'Diagramm vorbereiten und initialisieren
            Call PrepareDiagramm()

            'Monitor anzeigen
            If (Me.MenuItem_MonitorAnzeigen.Checked) Then
                Call Me.Monitor1.Show()
            End If

            'Ergebnis-Buttons
            Me.Button_openMDB.Enabled = False
            If (Not IsNothing(Sim1)) Then
                Me.Button_saveMDB.Enabled = True
                Me.Button_Scatterplot.Enabled = True
                Me.Button_loadRefResult.Enabled = True
            End If

            'EVO_Settings in temp-Verzeichnis speichern
            Dim dir As String
            dir = My.Computer.FileSystem.SpecialDirectories.Temp & "\"
            Call Me.EVO_Einstellungen1.saveSettings(dir & "EVO_Settings.xml")

            'EVO_Settings an Hauptdiagramm übergeben
            Call Me.Hauptdiagramm1.setSettings(Me.EVO_Einstellungen1.Settings)

            Select Case Anwendung

                Case ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM

                    'Settings an Sim1 übergeben
                    Call Me.Sim1.setSettings(Me.EVO_Einstellungen1.Settings)

                    Select Case Me.mProblem.Method

                        Case METH_SENSIPLOT
                            Call STARTEN_SensiPlot()

                        Case METH_PES, METH_CES, METH_HYBRID
                            Dim controller As New EVO.ES.Controller(Me.mProblem, Me.EVO_Einstellungen1.Settings, Me.mProgress, Me.Monitor1, Me.Hauptdiagramm1)
                            Call controller.InitApp(Me.Sim1)
                            Call controller.Start()

                        Case METH_HOOKJEEVES
                            Call STARTEN_HookJeeves()

                        Case METH_DDS
                            Call STARTEN_DDS()

                    End Select

                Case ANW_TESTPROBLEME
                    'Testprobleme mit PES:
                    Dim controller As New EVO.ES.Controller(Me.mProblem, Me.EVO_Einstellungen1.Settings, Me.mProgress, Me.Monitor1, Me.Hauptdiagramm1)
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
            Me.Button_Start.Text = "Run"
            Me.Button_Start.Enabled = False

        End If

    End Sub

    'Anwendung SensiPlot - START; läuft ohne Evolutionsstrategie             
    '***********************************************************
    Private Sub STARTEN_SensiPlot()

        'Hinweis:
        '------------------------------------------------------------------------
        'Die Modellparameter werden auch für die nicht ausgewählten OptParameter 
        'geschrieben, und zwar mit den in der OPT-Datei angegebenen Startwerten
        '------------------------------------------------------------------------

        Dim i, j, n, Anz_SensiPara, Anz_Sim As Integer
        Dim isOK As Boolean
        Dim ind As Common.Individuum_PES
        Dim serie As Steema.TeeChart.Styles.Points
        Dim serie3D As New Steema.TeeChart.Styles.Points3D
        Dim surface As New Steema.TeeChart.Styles.Surface
        Dim SimReihe As Wave.Zeitreihe
        Dim SimReihen As Collection
        Dim Wave1 As Wave.Wave

        'Simulationen in Originalverzeichnis ausführen (keine Threads)
        Sim1.WorkDir_Current = Sim1.WorkDir_Original

        'Instanzieren
        SimReihen = New Collection()

        'Parameter
        Anz_SensiPara = SensiPlot1.Selected_OptParameter.GetLength(0)

        'Anzahl Simulationen
        If (Anz_SensiPara = 1) Then
            '1 Parameter
            Anz_Sim = SensiPlot1.Anz_Steps
        Else
            '2 Parameter
            Anz_Sim = SensiPlot1.Anz_Steps ^ 2
        End If

        'Progress initialisieren
        Call Me.mProgress.Initialize(0, 0, 0, Anz_Sim)

        'Bei 2 OptParametern 3D-Diagramm vorbereiten
        If (Anz_SensiPara > 1) Then
            'Oberfläche
            surface = New Steema.TeeChart.Styles.Surface(Me.Hauptdiagramm1.Chart)
            surface.IrregularGrid = True
            surface.NumXValues = SensiPlot1.Anz_Steps
            surface.NumZValues = SensiPlot1.Anz_Steps
            '3D-Punkte
            serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Sensiplot", "Orange")
            'Diagramm drehen (rechter Mausbutton)
            Dim rotate1 As New Steema.TeeChart.Tools.Rotate
            rotate1.Button = Windows.Forms.MouseButtons.Right
            Me.Hauptdiagramm1.Tools.Add(rotate1)
            'MarksTips
            Me.Hauptdiagramm1.add_MarksTips(serie3D, Steema.TeeChart.Styles.MarksStyles.Label)
            surface.Title = "SensiPlot"
            surface.Cursor = Cursors.Hand
        End If

        'Simulationsschleife
        '-------------------
        Randomize()

        n = 0

        'Äussere Schleife (2. OptParameter)
        '----------------------------------
        For i = 0 To ((SensiPlot1.Anz_Steps - 1) * (Anz_SensiPara - 1))

            '2. OptParameterwert variieren
            If (Anz_SensiPara > 1) Then
                Select Case SensiPlot1.Selected_SensiType
                    Case "Gleichverteilt"
                        Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Xn = Rnd()
                    Case "Diskret"
                        Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Xn = i / (SensiPlot1.Anz_Steps - 1)
                End Select
            End If

            'Innere Schleife (1. OptParameter)
            '---------------------------------
            For j = 0 To SensiPlot1.Anz_Steps - 1

                '1. OptParameterwert variieren
                Select Case SensiPlot1.Selected_SensiType
                    Case "Gleichverteilt"
                        Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Xn = Rnd()
                    Case "Diskret"
                        Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Xn = j / (SensiPlot1.Anz_Steps - 1)
                End Select

                n += 1

                'Verlaufsanzeige aktualisieren
                Me.mProgress.iNachf = n

                'Einhaltung von OptParameter-Beziehung überprüfen
                isOK = True
                If (Anz_SensiPara > 1) Then
                    'Es muss nur der zweite Parameter auf eine Beziehung geprüft werden
                    If (Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Beziehung <> Beziehung.keine) Then
                        'Beziehung bezieht sich immer auf den in der Liste vorherigen Parameter
                        If (SensiPlot1.Selected_OptParameter(0) = SensiPlot1.Selected_OptParameter(1) - 1) Then

                            isOK = False

                            Dim ref As Double = Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).RWert
                            Dim wert As Double = Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).RWert

                            Select Case Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Beziehung
                                Case Beziehung.kleiner
                                    If (wert < ref) Then isOK = True
                                Case Beziehung.kleinergleich
                                    If (wert <= ref) Then isOK = True
                                Case Beziehung.groesser
                                    If (wert > ref) Then isOK = True
                                Case Beziehung.groessergleich
                                    If (wert >= ref) Then isOK = True
                            End Select

                        End If
                    End If
                End If

                'Evaluierung nur bei isOK
                If (isOK) Then

                    'Individuum instanzieren
                    ind = New Common.Individuum_PES("SensiPlot", n)

                    'OptParameter ins Individuum kopieren
                    ind.OptParameter = Me.mProblem.List_OptParameter

                    'Individuum in Sim evaluieren
                    isOK = Sim1.Evaluate(ind)
                    'TODO: Fehlerbehandlung bei Simulationsfehler

                    'BUG 253: Verletzte Constraints bei SensiPlot kenntlich machen?

                    'Diagramm aktualisieren
                    If (Anz_SensiPara = 1) Then
                        '1 Parameter
                        serie = Me.Hauptdiagramm1.getSeriesPoint("SensiPlot", "Orange")
                        serie.Add(ind.Penalties(SensiPlot1.Selected_Penaltyfunction), ind.OptParameter_RWerte(SensiPlot1.Selected_OptParameter(0)), n.ToString())
                    Else
                        '2 Parameter
                        surface.Add(ind.OptParameter_RWerte(SensiPlot1.Selected_OptParameter(0)), ind.Penalties(SensiPlot1.Selected_Penaltyfunction), ind.OptParameter_RWerte(SensiPlot1.Selected_OptParameter(1)), n.ToString())
                        serie3D.Add(ind.OptParameter_RWerte(SensiPlot1.Selected_OptParameter(0)), ind.Penalties(SensiPlot1.Selected_Penaltyfunction), ind.OptParameter_RWerte(SensiPlot1.Selected_OptParameter(1)), n.ToString())
                    End If

                    'Simulationsergebnis in Wave laden
                    If (SensiPlot1.show_Wave) Then
                        'SimReihe auslesen
                        SimReihe = Sim1.SimErgebnis(Me.mProblem.List_Penaltyfunctions(SensiPlot1.Selected_Penaltyfunction).SimGr)
                        'Lösungs-ID an Titel anhängen
                        SimReihe.Title += " (Lösung " & n.ToString() & ")"
                        'SimReihe zu Collection hinzufügen
                        SimReihen.Add(SimReihe)
                    End If

                End If

                System.Windows.Forms.Application.DoEvents()

            Next
        Next

        'Wave Diagramm anzeigen:
        '-----------------------
        If (SensiPlot1.show_Wave) Then
            Wave1 = New Wave.Wave()
            For Each zre As Wave.Zeitreihe In SimReihen
                Wave1.Display_Series(zre)
            Next
            Call Wave1.Show()
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

    Private Sub STARTEN_DDS()

        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        'Declarations
        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        Dim i, j As Integer
        Dim run As Integer = 0
        Dim Ini_Parameter() As Double
        Dim Current_Parameter(Me.mProblem.NumParams - 1) As Double
        Dim ind As Common.Individuum_PES
        Dim DDS As New modelEAU.DDS.DDS()

        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        'Initialize
        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        If Me.mProblem.List_Featurefunctions(0).Richtung = EVO_RICHTUNG.Maximierung Then
            DDS.to_max = -1.0
        Else
            DDS.to_max = 1.0
        End If

        ReDim Ini_Parameter(Me.mProblem.NumParams - 1)
        For i = 0 To Me.mProblem.NumParams - 1
            If (Me.mProblem.List_OptParameter(i).Xn < 0 Or Me.mProblem.List_OptParameter(i).Xn > 1) Then
                Throw New Exception("Ini parameter " & i & " not between 0 and 1")
            End If
            Ini_Parameter(i) = Me.mProblem.List_OptParameter(i).Xn
        Next

        If EVO_Einstellungen1.Settings.DDS.optStartparameter Then 'Zufällige Startparameter
            DDS.initialize(EVO_Einstellungen1.Settings.DDS.r_val, EVO_Einstellungen1.Settings.DDS.maxiter, _
                       Me.mProblem.NumParams)
        Else 'Vorgegebene Startparameter
            DDS.initialize(EVO_Einstellungen1.Settings.DDS.r_val, EVO_Einstellungen1.Settings.DDS.maxiter, _
                       Me.mProblem.NumParams, Ini_Parameter)
        End If

        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        'Ini objective function evaluations
        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        For i = 0 To DDS.ini_fevals - 1
            run += 1

            Current_Parameter = DDS.ini_solution_candidate()

            ind = New Common.Individuum_PES("DDS", run)
            'OptParameter ins Individuum kopieren
            '------------------------------------
            For j = 0 To ind.OptParameter.Length - 1
                ind.OptParameter(j).Xn = Current_Parameter(j)
            Next

            'Vorbereiten des Modelldatensatzes
            '---------------------------------
            Call Sim1.PREPARE_Evaluation_PES(ind.OptParameter)

            'Evaluierung des Simulationsmodells (ToDo: Validätsprüfung fehlt)
            '----------------------------------------------------------------
            SIM_Eval_is_OK = Sim1.launchSim()

            Call My.Application.DoEvents()

            If SIM_Eval_is_OK Then Call Sim1.SIM_Ergebnis_auswerten(ind)

            Call My.Application.DoEvents()

            'Lösung im TeeChart einzeichnen
            '------------------------------
            Dim serie As Steema.TeeChart.Styles.Series
            serie = Me.Hauptdiagramm1.getSeriesPoint("DDS")
            Call serie.Add(run, ind.Penalties(0), run.ToString())

            Call My.Application.DoEvents()

            'Bestwertspeicher und Searchhistorie aktualisieren
            '-------------------------------------------------
            If (run = 1) Then
                DDS.ini_Fbest(ind.Penalties(0))
            Else
                DDS.update_Fbest(ind.Penalties(0))
            End If
            DDS.update_search_historie(ind.Penalties(0), run - 1)
        Next
        DDS.track_ini()

        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        'Ende ini objective function evaluations
        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        'Code below is now the DDS algorithm as presented in Figure 1 of 
        'Tolson and Shoemaker (2007) 
        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        'start the OUTER DDS ALGORITHM LOOP for remaining allowble function evaluations (ileft)
        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        For i = 1 To DDS.ileft
            run += 1

            Current_Parameter = DDS.determine_DV(i)

            ind = New Common.Individuum_PES("DDS", run)
            'OptParameter ins Individuum kopieren
            '------------------------------------
            For j = 0 To ind.OptParameter.Length - 1
                ind.OptParameter(j).Xn = Current_Parameter(j)
            Next

            'Vorbereiten des Modelldatensatzes
            '---------------------------------
            Call Sim1.PREPARE_Evaluation_PES(ind.OptParameter)

            'Evaluierung des Simulationsmodells (ToDo: Validätsprüfung fehlt)
            '----------------------------------------------------------------
            SIM_Eval_is_OK = Sim1.launchSim()

            Call My.Application.DoEvents()

            If SIM_Eval_is_OK Then Call Sim1.SIM_Ergebnis_auswerten(ind)

            Call My.Application.DoEvents()

            'Lösung im TeeChart einzeichnen
            '------------------------------
            Dim serie As Steema.TeeChart.Styles.Series
            serie = Me.Hauptdiagramm1.getSeriesPoint("DDS")
            Call serie.Add(run, ind.Penalties(0), run.ToString())

            Call My.Application.DoEvents()

            DDS.update_Fbest(ind.Penalties(0))

            DDS.update_search_historie(ind.Penalties(0), run - 1)
        Next

        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        'ends OUTER DDS ALGORITHM LOOP
        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    End Sub

    'Anwendung des Verfahrens von Hook und Jeeves zur Parameteroptimierung
    '*********************************************************************
    Private Sub STARTEN_HookJeeves()

        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim b As Boolean
        Dim ind As Common.Individuum_PES
        Dim QNBest() As Double = {}
        Dim QBest() As Double = {}
        Dim aktuellePara(Me.mProblem.NumParams - 1) As Double
        Dim SIM_Eval_is_OK As Boolean
        Dim durchlauf As Long
        Dim Iterationen As Long
        Dim Tastschritte_aktuell As Long
        Dim Tastschritte_gesamt As Long
        Dim Extrapolationsschritte As Long
        Dim Rueckschritte As Long

        Dim HookJeeves As New EVO.HookeAndJeeves.HookeAndJeeves(Me.mProblem.NumParams, EVO_Einstellungen1.Settings.HookJeeves.DnStart, EVO_Einstellungen1.Settings.HookJeeves.DnFinish)

        ReDim QNBest(Me.mProblem.NumPenalties - 1)
        ReDim QBest(Me.mProblem.NumPenalties - 1)

        durchlauf = 0
        Tastschritte_aktuell = 0
        Tastschritte_gesamt = 0
        Extrapolationsschritte = 0
        Rueckschritte = 0
        Iterationen = 0
        b = False

        Call HookJeeves.Initialize(Me.mProblem.List_OptParameter)

        'Initialisierungssimulation
        For i = 0 To Me.mProblem.NumParams - 1
            aktuellePara(i) = Me.mProblem.List_OptParameter(i).Xn
        Next
        QNBest(0) = 1.79E+308
        QBest(0) = 1.79E+308
        k = 0

        Do While (HookJeeves.AktuelleSchrittweite > HookJeeves.MinimaleSchrittweite)

            Iterationen += 1
            durchlauf += 1

            'Bestimmen der Ausgangsgüte
            '==========================
            'Individuum instanzieren
            ind = New Common.Individuum_PES("HJ", durchlauf)

            'HACK: OptParameter ins Individuum kopieren
            For i = 0 To ind.OptParameter.Length - 1
                ind.OptParameter(i).Xn = aktuellePara(i)
            Next

            'Vorbereiten des Modelldatensatzes
            Call Sim1.PREPARE_Evaluation_PES(ind.OptParameter)

            'Evaluierung des Simulationsmodells (ToDo: Validätsprüfung fehlt)
            SIM_Eval_is_OK = Sim1.launchSim()
            If SIM_Eval_is_OK Then Call Sim1.SIM_Ergebnis_auswerten(ind)

            'Lösung im TeeChart einzeichnen
            '------------------------------
            Dim serie As Steema.TeeChart.Styles.Series
            serie = Me.Hauptdiagramm1.getSeriesPoint("Hook and Jeeves")
            Call serie.Add(durchlauf, ind.Penalties(0), durchlauf.ToString())

            Call My.Application.DoEvents()

            'Penalties in Bestwert kopieren
            Call ind.Penalties.CopyTo(QNBest, 0)

            'Tastschritte
            '============
            For j = 0 To HookJeeves.AnzahlParameter - 1

                aktuellePara = HookJeeves.Tastschritt(j, EVO.HookeAndJeeves.HookeAndJeeves.TastschrittRichtung.Vorwärts)

                Tastschritte_aktuell += 1
                durchlauf += 1
                Me.EVO_Einstellungen1.Label_HJ_TSaktuelle.Text = Tastschritte_aktuell.ToString

                'Individuum instanzieren
                ind = New Common.Individuum_PES("HJ", durchlauf)

                'HACK: OptParameter ins Individuum kopieren
                For i = 0 To ind.OptParameter.Length - 1
                    ind.OptParameter(i).Xn = aktuellePara(i)
                Next

                'Vorbereiten des Modelldatensatzes
                Call Sim1.PREPARE_Evaluation_PES(ind.OptParameter)

                'Evaluierung des Simulationsmodells
                SIM_Eval_is_OK = Sim1.launchSim()
                If SIM_Eval_is_OK Then Call Sim1.SIM_Ergebnis_auswerten(ind)

                'Lösung im TeeChart einzeichnen
                '------------------------------
                serie = Me.Hauptdiagramm1.getSeriesPoint("Hook and Jeeves")
                Call serie.Add(durchlauf, ind.Penalties(0), durchlauf.ToString())

                Call My.Application.DoEvents()

                If (ind.Penalties(0) >= QNBest(0)) Then

                    aktuellePara = HookJeeves.Tastschritt(j, EVO.HookeAndJeeves.HookeAndJeeves.TastschrittRichtung.Rückwärts)

                    Tastschritte_aktuell += 1
                    durchlauf += 1
                    Me.EVO_Einstellungen1.Label_HJ_TSaktuelle.Text = Tastschritte_aktuell.ToString

                    'Individuum instanzieren
                    ind = New Common.Individuum_PES("HJ", durchlauf)

                    'HACK: OptParameter ins Individuum kopieren
                    For i = 0 To ind.OptParameter.Length - 1
                        ind.OptParameter(i).Xn = aktuellePara(i)
                    Next

                    'Vorbereiten des Modelldatensatzes
                    Call Sim1.PREPARE_Evaluation_PES(ind.OptParameter)

                    'Evaluierung des Simulationsmodells
                    SIM_Eval_is_OK = Sim1.launchSim()
                    If SIM_Eval_is_OK Then Call Sim1.SIM_Ergebnis_auswerten(ind)

                    'Lösung im TeeChart einzeichnen
                    '------------------------------
                    serie = Me.Hauptdiagramm1.getSeriesPoint("Hook and Jeeves")
                    Call serie.Add(durchlauf, ind.Penalties(0), durchlauf.ToString())

                    Call My.Application.DoEvents()

                    If (ind.Penalties(0) >= QNBest(0)) Then
                        aktuellePara = HookJeeves.TastschrittResetParameter(j)
                    Else
                        Call ind.Penalties.CopyTo(QNBest, 0)
                    End If
                Else
                    Call ind.Penalties.CopyTo(QNBest, 0)
                End If
            Next

            Tastschritte_gesamt += Tastschritte_aktuell
            Me.EVO_Einstellungen1.Label_HJ_TSgesamt.Text = Tastschritte_gesamt.ToString
            Tastschritte_aktuell = 0
            Me.EVO_Einstellungen1.Label_HJ_TSaktuelle.Text = Tastschritte_aktuell.ToString
            Me.EVO_Einstellungen1.Label_HJ_TSmittel.Text = Math.Round((Tastschritte_gesamt / Iterationen), 2).ToString

            Call My.Application.DoEvents()

            'Extrapolationsschritt
            If (QNBest(0) < QBest(0)) Then

                'Lösung im TeeChart einzeichnen
                '------------------------------
                serie = Me.Hauptdiagramm1.getSeriesPoint("Hook and Jeeves Best", "Green")
                Call serie.Add(durchlauf, ind.Penalties(0), durchlauf.ToString())

                Call My.Application.DoEvents()

                Call QNBest.CopyTo(QBest, 0)
                Call HookJeeves.Extrapolationsschritt()
                Extrapolationsschritte += 1
                Me.EVO_Einstellungen1.Label_HJ_ES.Text = Extrapolationsschritte.ToString
                Call My.Application.DoEvents()
                k += 1
                aktuellePara = HookJeeves.getLetzteParameter
                For i = 0 To HookJeeves.AnzahlParameter - 1
                    If aktuellePara(i) < 0 Or aktuellePara(i) > 1 Then
                        HookJeeves.Rueckschritt()
                        Rueckschritte += 1
                        Me.EVO_Einstellungen1.Label_HJ_RS.Text = Rueckschritte.ToString()
                        Call My.Application.DoEvents()
                        k += -1
                        HookJeeves.Schrittweitenhalbierung()
                        aktuellePara = HookJeeves.getLetzteParameter()
                        Exit For
                    End If
                Next
                b = True
            Else
                'If b Then
                '    HookJeeves.Rueckschritt()
                '    b = False
                'Else
                '    HookJeeves.Schrittweitenhalbierung()
                'End If
                If k > 0 Then
                    HookJeeves.Rueckschritt()
                    Me.EVO_Einstellungen1.Label_HJ_RS.Text = Rueckschritte.ToString()
                    Call My.Application.DoEvents()
                    HookJeeves.Schrittweitenhalbierung()
                    aktuellePara = HookJeeves.getLetzteParameter()
                Else
                    HookJeeves.Schrittweitenhalbierung()
                End If
            End If
        Loop
    End Sub

#End Region 'Start Button Pressed

#Region "Diagrammfunktionen"

    'Diagrammfunktionen
    '###################

    'Hauptdiagramm bearbeiten
    '************************
    Private Sub Button_TChartEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChartEdit.Click
        Call Me.Hauptdiagramm1.TChartEdit(sender, e)
    End Sub

    'Chart nach Excel exportieren
    '****************************
    Private Sub TChart2Excel(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChart2Excel.Click
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
    Private Sub TChart2PNG(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChart2PNG.Click
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
    Private Sub TChartSave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChartSave.Click
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

                Call Testprobleme1.DiagInitialise(Me.EVO_Einstellungen1.Settings, Me.Hauptdiagramm1)

            Case ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                Select Case Me.mProblem.Method

                    Case METH_SENSIPLOT 'SensiPlot
                        'XXXXXXXXXXXXXXXXXXXXXXXXX

                        If (SensiPlot1.Selected_OptParameter.GetLength(0) = 1) Then

                            '1 OptParameter:
                            '---------------

                            'Achsen:
                            '-------
                            'X-Achse = QWert
                            Achse.Title = Me.mProblem.List_Penaltyfunctions(SensiPlot1.Selected_Penaltyfunction).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)
                            'Y-Achse = OptParameter
                            Achse.Title = Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)

                            'Achsenzuordnung
                            'BUG 327!
                            For i = 0 To Me.mProblem.NumFeatures - 1
                                If (Me.mProblem.List_Featurefunctions(i).Bezeichnung = Me.mProblem.List_Penaltyfunctions(SensiPlot1.Selected_Penaltyfunction).Bezeichnung) Then
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
                            Achse.Title = Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)
                            'Y-Achse = QWert
                            Achse.Title = Me.mProblem.List_Penaltyfunctions(SensiPlot1.Selected_Penaltyfunction).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)
                            'Z-Achse = OptParameter2
                            Achse.Title = Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)

                            'Achsenzuordnung
                            'BUG 327!
                            Me.Hauptdiagramm1.ZielIndexX = -1
                            For i = 0 To Me.mProblem.NumFeatures - 1
                                If (Me.mProblem.List_Featurefunctions(i).Bezeichnung = Me.mProblem.List_Penaltyfunctions(SensiPlot1.Selected_Penaltyfunction).Bezeichnung) Then
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
                        If (Me.mProblem.NumPenalties = 1) Then

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
                            For i = 0 To Me.mProblem.NumFeatures - 1
                                If (Me.mProblem.List_Featurefunctions(i).isPenalty) Then
                                    Achse.Title = Me.mProblem.List_Featurefunctions(i).Bezeichnung
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
                            For i = 0 To Me.mProblem.NumFeatures - 1
                                If (Me.mProblem.List_Featurefunctions(i).isPenalty) Then
                                    Achse.Title = Me.mProblem.List_Featurefunctions(i).Bezeichnung
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
                            If (Me.mProblem.NumPenalties > 3) Then
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
    Private Sub showScatterplot(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Scatterplot.Click

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
            For Each feature As Common.Featurefunction In Me.mProblem.List_Featurefunctions

                With feature

                    'Referenzreihe in Wave laden
                    '---------------------------
                    If (.Typ = "Reihe" Or .Typ = "IHA") Then
                        'Referenzreihen nur jeweils ein Mal zeichnen
                        If (Not RefSeries.Contains(.RefReiheDatei & .RefGr)) Then
                            RefSeries.Add(.RefGr, .RefReiheDatei & .RefGr)
                            'Referenzreihe in Wave laden
                            Wave1.Display_Series(.RefReihe)
                        End If
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

    'Ergebnisdatenbank speichern
    '***************************
    Private Sub saveMDB(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_saveMDB.Click

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
    Private Sub loadFromMDB(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_openMDB.Click

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
                        tmpAchse.Title = Me.mProblem.List_Featurefunctions(Me.Hauptdiagramm1.ZielIndexX).Bezeichnung
                        Achsen.Add(tmpAchse)
                    Else
                        'Multi-objective
                        '---------------
                        'X-Achse
                        tmpAchse.Title = Me.mProblem.List_Featurefunctions(Me.Hauptdiagramm1.ZielIndexX).Bezeichnung
                        Achsen.Add(tmpAchse)
                        'Y-Achse
                        tmpAchse.Title = Me.mProblem.List_Featurefunctions(Me.Hauptdiagramm1.ZielIndexY).Bezeichnung
                        Achsen.Add(tmpAchse)
                        If (Not Me.Hauptdiagramm1.ZielIndexZ = -1) Then
                            'Z-Achse
                            tmpAchse.Title = Me.mProblem.List_Featurefunctions(Me.Hauptdiagramm1.ZielIndexZ).Bezeichnung
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
                                serie.Add(ind.ID, ind.Features(Me.Hauptdiagramm1.ZielIndexX), ind.ID.ToString())
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
                                serie.Add(ind.Features(Me.Hauptdiagramm1.ZielIndexX), ind.Features(Me.Hauptdiagramm1.ZielIndexY), ind.ID.ToString())
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
                                serie3D.Add(ind.Features(Me.Hauptdiagramm1.ZielIndexX), ind.Features(Me.Hauptdiagramm1.ZielIndexY), ind.Features(Me.Hauptdiagramm1.ZielIndexZ), ind.ID.ToString())
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
                                serie.Add(sekpopind.Features(Me.Hauptdiagramm1.ZielIndexX), sekpopind.Features(Me.Hauptdiagramm1.ZielIndexY), sekpopind.ID.ToString())
                            Else
                                '3D
                                '--
                                serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Sekundäre Population", "Green")
                                serie3D.Add(sekpopind.Features(Me.Hauptdiagramm1.ZielIndexX), sekpopind.Features(Me.Hauptdiagramm1.ZielIndexY), sekpopind.Features(Me.Hauptdiagramm1.ZielIndexZ), sekpopind.ID.ToString())
                            End If
                        Next

                    End If

                    Call My.Application.DoEvents()

                    'Hypervolumen
                    '============
                    If (importDialog.CheckBox_Hypervol.Checked) Then

                        'Hypervolumen instanzieren
                        Dim Hypervolume As EVO.MO_Indicators.Indicators
                        Hypervolume = EVO.MO_Indicators.MO_IndicatorFabrik.GetInstance(EVO.MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, Me.mProblem.NumPenalties)
                        Dim indicator As Double
                        Dim nadir() As Double

                        'Alle Generationen durchlaufen
                        For Each sekpop As EVO.OptResult.OptResult.Struct_SekPop In Sim1.OptResult.SekPops

                            'Hypervolumen berechnen
                            Call Hypervolume.update_dataset(Sim1.OptResult.getSekPopValues(sekpop.iGen))
                            indicator = Math.Abs(Hypervolume.calc_indicator())
                            nadir = Hypervolume.nadir

                            'Hypervolumen zeichnen
                            Call Me.Hauptdiagramm1.ZeichneNadirpunkt(nadir)
                            Call Me.Monitor1.ZeichneHyperVolumen(sekpop.iGen, indicator)

                            Call My.Application.DoEvents()

                        Next

                    End If

                    'Ergebnis-Buttons
                    Me.Button_Scatterplot.Enabled = True
                    Me.Button_loadRefResult.Enabled = True

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
    Private Sub loadRefFromMDB(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_loadRefResult.Click

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
                        serie.Add(sekpopind.Features(Me.Hauptdiagramm1.ZielIndexX), sekpopind.Features(Me.Hauptdiagramm1.ZielIndexY), "Vergleichsergebnis " & sekpopind.ID)
                    Else
                        '3D
                        '--
                        serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Vergleichsergebnis", "Blue")
                        serie3D.Add(sekpopind.Features(Me.Hauptdiagramm1.ZielIndexX), sekpopind.Features(Me.Hauptdiagramm1.ZielIndexY), sekpopind.Features(Me.Hauptdiagramm1.ZielIndexZ), sekpopind.ID & " (Vergleichsergebnis)")
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
                ReDim nadir(Me.mProblem.NumPenalties - 1)
                ReDim minmax(Me.mProblem.NumPenalties - 1)
                For i = 0 To Me.mProblem.NumPenalties - 1
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
                Call Me.Monitor1.ZeichneReferenzHypervolumen(indicatorRef)

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
            n_Threads = 3
        ElseIf LogCPU = 2 Then
            n_Threads = 4
        ElseIf LogCPU = 4 Then
            n_Threads = 7
        End If

        Return n_Threads

    End Function

#End Region 'Methoden

End Class
