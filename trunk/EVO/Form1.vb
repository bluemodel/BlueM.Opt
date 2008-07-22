Option Strict Off ' Off ist Default
Option Explicit On
Imports System.IO
Imports System.Management
Imports IHWB.EVO.Common
Imports System.ComponentModel

'*******************************************************************************
'*******************************************************************************
'**** ihwb Optimierung                                                      ****
'****                                                                       ****
'**** Christoph Huebner, Felix Froehlich, Dirk Muschalla, Dominik Kerber    ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'****                                                                       ****
'**** Erstellt: Dezember 2003                                               ****
'****                                                                       ****
'**** Letzte Änderung: Juli 2008                                            ****
'*******************************************************************************
'*******************************************************************************

Partial Class Form1
    Inherits System.Windows.Forms.Form

#Region "Eigenschaften"

    Private IsInitializing As Boolean

    'Anwendung
    Private Anwendung As String

    'Optimierungsmethode
    Public Shared Method As String

    '**** Deklarationen der Module *****
    Private WithEvents Testprobleme1 As Testprobleme
    Friend WithEvents Sim1 As Sim
    Private SensiPlot1 As SensiPlot
    Private CES1 As EVO.Kern.CES
    Private TSP1 As TSP

    'New'
    'Dim hybrid2008 As EVO.Hybrid2008.Main.cs

    '**** Globale Parameter Parameter Optimierung ****
    'TODO: diese Werte sollten eigentlich nur in CES bzw PES vorgehalten werden
    Dim globalAnzPar As Short
    Dim array_x() As Double
    Dim array_y() As Double
    Dim myPara() As EVO.Common.OptParameter

    '**** Verschiedenes ****
    Dim isrun As Boolean = False                        'Optimierung läuft
    Dim ispause As Boolean = False                      'Optimierung ist pausiert

    '**** Multithreading ****
    Dim SIM_Eval_is_OK As Boolean
    Dim BackgroundWorker1 As System.ComponentModel.BackgroundWorker 'Threads für Backgroundworker
    Private PhysCPU As Integer                                      'Anzahl physikalischer Prozessoren
    Private LogCPU As Integer                                       'Anzahl logischer Prozessoren

    'Dialoge
    Private WithEvents solutionDialog As SolutionDialog
    Private WithEvents scatterplot1 As Scatterplot

    'Hauptdiagramm
    Public ReadOnly Property Hauptdiagramm() As EVO.Diagramm
        Get
            Return Me.DForm.Diag
        End Get
    End Property

    'Indicatordiagramm
    Public ReadOnly Property Indicatordiagramm() As EVO.Diagramm
        Get
            Return Me.DForm.DiagIndicator
        End Get
    End Property

#End Region 'Eigenschaften

#Region "Methoden"

    'Initialisierung von Form1
    '*************************
    Private Sub Form1_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        'Anzahl der Prozessoren wird ermittelt
        Anzahl_Prozessoren(PhysCPU, LogCPU)

        'XP-look
        System.Windows.Forms.Application.EnableVisualStyles()

        'Liste der Anwendungen in ComboBox schreiben und Anfangseinstellung wählen
        ComboBox_Anwendung.Items.AddRange(New Object() {"", ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM, ANW_TESTPROBLEME, ANW_TSP})
        ComboBox_Anwendung.SelectedIndex = 0

        'Liste der Methoden in ComboBox schreiben und Anfangseinstellung wählen
        ComboBox_Methode.Items.AddRange(New Object() {"", METH_PES, METH_CES, METH_HYBRID, METH_SENSIPLOT, METH_HOOKJEEVES})
        ComboBox_Methode.SelectedIndex = 0

        'Ende der Initialisierung
        IsInitializing = False

    End Sub

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
            Me.Hauptdiagramm.Reset()

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
                    Sim1 = New BlueM()


                Case ANW_SMUSI 'Anwendung Smusi
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse Smusi initialisieren
                    Sim1 = New Smusi()


                Case ANW_SCAN 'Anwendung S:CAN
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse Scan initialisieren
                    Sim1 = New Scan()


                Case ANW_SWMM   'Anwendung SWMM
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse SWMM initialisieren
                    Sim1 = New SWMM()


                Case ANW_TESTPROBLEME 'Anwendung Testprobleme
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Testprobleme instanzieren
                    Testprobleme1 = New Testprobleme()


                Case ANW_TSP 'Anwendung Traveling Salesman Problem (TSP)
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    TSP1 = New TSP()

                    Call TSP1.TSP_Initialize(Me.Hauptdiagramm)

                    'Start-Button aktivieren (keine Methodenauswahl erforderlich)
                    Button_Start.Enabled = True

            End Select

            'Datensatz UI aktivieren
            Call Me.Datensatz_initUI()

            'EVO_Verlauf zurücksetzen
            Call Me.EVO_Opt_Verlauf1.Initialisieren(EVO_Einstellungen1.Settings.PES.Pop.n_Runden, EVO_Einstellungen1.Settings.PES.Pop.n_Popul, EVO_Einstellungen1.Settings.PES.n_Gen, EVO_Einstellungen1.Settings.PES.n_Nachf)

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

            'zuletzt benutzten Datensatz setzen
            If (My.Settings.MRUSimDatensaetze.Count > 0) Then
                pfad = My.Settings.MRUSimDatensaetze.Item(My.Settings.MRUSimDatensaetze.Count - 1)
                Me.ComboBox_Datensatz.SelectedItem = pfad
                Call Sim1.setDatensatz(pfad)
            End If

            'Browse-Button aktivieren
            Me.Button_BrowseDatensatz.Enabled = True
        End If

    End Sub

    'Combo_Datensatz auffüllen
    '*************************
    Private Sub Datensatz_populateCombo()

        Dim i As Integer

        'vorherige Einträge löschen
        Me.ComboBox_Datensatz.Items.Clear()

        Select Case Me.Anwendung

            Case ANW_TESTPROBLEME

                'Mit Tesproblemen füllen
                Me.ComboBox_Datensatz.Items.AddRange(Testprobleme1.Testprobleme)

            Case Else '(Sim-Anwendungen)

                'Mit Benutzer-MRUSimDatensätze füllen
                If (My.Settings.MRUSimDatensaetze.Count > 0) Then

                    'Combobox rückwärts füllen
                    For i = My.Settings.MRUSimDatensaetze.Count - 1 To 0 Step -1
                        Me.ComboBox_Datensatz.Items.Add(My.Settings.MRUSimDatensaetze.Item(i))
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
        OpenFileDialog1.Filter = Sim1.Datensatzendung.Substring(1) & "-Dateien (*" & Sim1.Datensatzendung & ")|*" & Sim1.Datensatzendung
        OpenFileDialog1.Title = "Datensatz auswählen"

        'Alten Datensatz dem Dialog zuweisen
        OpenFileDialog1.InitialDirectory = Sim1.WorkDir
        OpenFileDialog1.FileName = Sim1.WorkDir & Sim1.Datensatz & Sim1.Datensatzendung

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

                    'Tesproblem setzen
                    Testprobleme1.setTestproblem(Me.ComboBox_Datensatz.SelectedItem)

                    'Tooltip anzeigen
                    Me.ToolTip1.SetToolTip(Me.ComboBox_Datensatz, Testprobleme1.TestProblemDescription)

                Case Else '(Alle Sim-Anwendungen)

                    Call Sim1.setDatensatz(Me.ComboBox_Datensatz.SelectedItem)

            End Select

            'Methodenauswahl aktivieren und zurücksetzen
            '-------------------------------------------
            Me.Label_Methode.Enabled = True
            Me.ComboBox_Methode.Enabled = True
            Me.ComboBox_Methode.SelectedItem = ""

        End If

    End Sub

    'Methode wurde ausgewählt
    '************************
    Private Sub INI_Method(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Methode.SelectedIndexChanged

        If (Me.IsInitializing = True) Then

            Exit Sub

        Else

            'Diagramm zurücksetzen
            Me.Hauptdiagramm.Reset()

            'Alles deaktivieren, danach je nach Methode aktivieren
            '-----------------------------------------------------

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Ergebnis-Buttons
            Me.Button_saveMDB.Enabled = False
            Me.Button_openMDB.Enabled = False
            Me.Button_Scatterplot.Enabled = False

            'EVO_Einstellungen deaktivieren
            EVO_Einstellungen1.Enabled = False

            'EVO_Einstellungen zurücksetzen
            EVO_Einstellungen1.isSaved = False

            'Mauszeiger busy
            Cursor = Cursors.WaitCursor

            'Methode setzen
            Form1.Method = ComboBox_Methode.SelectedItem

            Select Case Form1.Method

                Case "" 'Keine Methode ausgewählt
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Mauszeiger wieder normal
                    Cursor = Cursors.Default

                    'Ende
                    Exit Sub

                Case METH_SENSIPLOT 'Methode SensiPlot
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    SensiPlot1 = New SensiPlot()

                    'SensiPlot für Sim vorbereiten
                    Call Sim1.read_and_valid_INI_Files_PES()

                    'SensiPlot Dialog anzeigen:
                    '--------------------------
                    'List_Boxen füllen
                    Dim i As Integer
                    For i = 0 To Sim1.List_OptParameter.GetUpperBound(0)
                        Call SensiPlot1.ListBox_OptParameter_add(Sim1.List_OptParameter(i))
                    Next
                    For Each optziel As Common.Ziel In Common.Manager.List_OptZiele
                        Call SensiPlot1.ListBox_OptZiele_add(optziel)
                    Next
                    'Dialog anzeigen
                    Dim SensiPlotDiagResult As Windows.Forms.DialogResult
                    SensiPlotDiagResult = SensiPlot1.ShowDialog()
                    If (Not SensiPlotDiagResult = Windows.Forms.DialogResult.OK) Then
                        'Mauszeiger wieder normal
                        Cursor = Cursors.Default
                        Exit Sub
                    End If


                Case METH_PES 'Methode PES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'EVO_Einstellungen aktivieren
                    EVO_Einstellungen1.Enabled = True

                    'Tabcontrols entfernen die man nicht braucht
                    With EVO_Einstellungen1
                        .TabControl1.TabPages.Remove(.TabPage_CES)
                        .TabControl1.TabPages.Remove(.TabPage_HookeJeeves)
                        .TabControl1.TabPages.Remove(.TabPage_Hybrid2008)
                    End With

                    'Fallunterscheidung Anwendung
                    '============================
                    If (Me.Anwendung = ANW_TESTPROBLEME) Then
                        'Testprobleme
                        '------------

                        'EVO_Einstellungen einrichten
                        Call EVO_Einstellungen1.setStandard_PES(Testprobleme1.OptModus)

                        'Globale Parameter werden gesetzt
                        Call Testprobleme1.Parameter_Uebergabe(globalAnzPar, myPara)

                    Else
                        'Alle SIM-Anwendungen
                        '--------------------

                        'Ergebnis-Buttons
                        Me.Button_openMDB.Enabled = True

                        'PES für Sim vorbereiten
                        Call Sim1.read_and_valid_INI_Files_PES()

                        'EVO_Einstellungen einrichten
                        If (Common.Manager.AnzPenalty = 1) Then
                            'Single-Objective
                            Call EVO_Einstellungen1.setStandard_PES(Common.Constants.EVO_MODUS.Single_Objective)
                        ElseIf (Common.Manager.AnzPenalty > 1) Then
                            'Multi-Objective
                            Call EVO_Einstellungen1.setStandard_PES(Common.Constants.EVO_MODUS.Multi_Objective)
                        End If

                        'Parameterübergabe an PES
                        Call Sim1.Parameter_Uebergabe(globalAnzPar, myPara)

                    End If

                    'EVO_Verlauf zurücksetzen
                    Call Me.EVO_Opt_Verlauf1.Initialisieren(EVO_Einstellungen1.Settings.PES.Pop.n_Runden, EVO_Einstellungen1.Settings.PES.Pop.n_Popul, EVO_Einstellungen1.Settings.PES.n_Gen, EVO_Einstellungen1.Settings.PES.n_Nachf)


                Case METH_HOOKJEEVES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'EVO_Einstellungen aktivieren
                    EVO_Einstellungen1.Enabled = True

                    'Tabcontrols entfernen die man nicht braucht
                    With EVO_Einstellungen1
                        .TabControl1.TabPages.Remove(.TabPage_PES)
                        .TabControl1.TabPages.Remove(.TabPage_CES)
                        .TabControl1.TabPages.Remove(.TabPage_Hybrid2008)
                    End With

                    'TODO: eigenen read and valid methode für hookJeeves
                    Call Sim1.read_and_valid_INI_Files_PES()

                    'Kontrolle: Nur SO möglich!
                    If (Common.Manager.AnzPenalty = 1) Then
                        Call EVO_Einstellungen1.setStandard_HJ()
                    ElseIf (Common.Manager.AnzPenalty > 1) Then
                        Throw New Exception("Methode von Hook und Jeeves erlaubt nur SO-Optimierung!")
                    End If

                    'TODO: eigenen Parameterübergabe an HookJeeves (evtl.überladen von Parameter_Uebergabe)
                    Call Sim1.Parameter_Uebergabe(globalAnzPar, myPara)


                Case METH_CES, METH_HYBRID 'Methode CES und Methode CES_PES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Funktioniert nur bei BlueM!
                    If (Not Anwendung = ANW_BLUEM) Then
                        Throw New Exception("CES funktioniert bisher nur mit BlueM!")
                    End If

                    'EVO_Einstellungen aktivieren
                    EVO_Einstellungen1.Enabled = True

                    'Tabcontrols entfernen die man nicht braucht
                    With EVO_Einstellungen1
                        .TabControl1.TabPages.Remove(.TabPage_HookeJeeves)
                        .TabControl1.TabPages.Remove(.TabPage_Hybrid2008)
                    End With

                    'Ergebnis-Buttons
                    Me.Button_openMDB.Enabled = True

                    'Fallunterscheidung CES oder Hybrid
                    Select Case Form1.Method
                        Case METH_CES

                            'Tabcontrol PES auch entfernen
                            With EVO_Einstellungen1
                                .TabControl1.TabPages.Remove(.TabPage_PES)
                            End With

                            'CES für Sim vorbereiten (Files lesen und Validieren)
                            Call Sim1.read_and_valid_INI_Files_CES()

                        Case METH_HYBRID

                            'CES für Sim vorbereiten (Files lesen und Validieren)
                            Call Sim1.read_and_valid_INI_Files_HYBRID()

                            'Original ModellParameter schreiben
                            Call Sim1.Write_ModellParameter()

                            'Original Transportstrecken einlesen
                            Call Sim1.SKos1.Read_TRS_Orig_Daten(Sim1)

                            'Die Original Gerinneflaechen für SKos einlesen
                            'Call Sim1.skos.

                    End Select

                    'EVO_Einstellungen einrichten
                    '----------------------------
                    'Je nach Methode nur CES oder HYBRID
                    Call EVO_Einstellungen1.setStandard_CES()

                    'Je nach Anzahl der OptZiele von MO auf SO umschalten PES
                    If (Common.Manager.AnzPenalty = 1) Then
                        'Single-Objective
                        Call EVO_Einstellungen1.setStandard_PES(Common.Constants.EVO_MODUS.Single_Objective)
                    ElseIf (Common.Manager.AnzPenalty > 1) Then
                        'Multi-Objective
                        Call EVO_Einstellungen1.setStandard_PES(Common.Constants.EVO_MODUS.Multi_Objective)
                    End If

                    'Bei Testmodus wird die Anzahl der Kinder und Generationen überschrieben
                    If Not Sim1.CES_T_Modus = Common.Constants.CES_T_MODUS.No_Test Then
                        Call EVO_Einstellungen1.setTestModus(Sim1.CES_T_Modus, Sim1.TestPath, 1, 1, Sim1.n_Combinations)
                    End If


                Case METH_Hybrid2008
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'EVO_Einstellungen aktivieren
                    EVO_Einstellungen1.Enabled = True

                    'Tabcontrols entfernen die man nicht braucht
                    With EVO_Einstellungen1
                        .TabControl1.TabPages.Remove(.TabPage_PES)
                        .TabControl1.TabPages.Remove(.TabPage_CES)
                        .TabControl1.TabPages.Remove(.TabPage_HookeJeeves)
                    End With

                    If (Me.Anwendung = ANW_TESTPROBLEME) Then
                        'Testprobleme mit Hybrid2008 Verfahren berechnen
                        MsgBox("Berechnung der Testprobleme mit Hybrid2008", MsgBoxStyle.Information, "Info")

                    Else
                        'Modelle mit Hybrid2008 berechnen
                        MsgBox("Berechnung der Modelle mit Hybrid2008", MsgBoxStyle.Information, "Info")


                    End If

                    'Ergebnis-Buttons
                    'Me.Button_openMDB.Enabled = True

                    'PES für Sim vorbereiten
                    'Call Sim1.read_and_valid_INI_Files_PES()

                    'EVO_Einstellungen einrichten
                    'Me.EVO_Einstellungen1.TabControl1.SelectedTab = Me.EVO_Einstellungen1.TabPage_PES
                    'If (Common.Manager.AnzPenalty = 1) Then
                    'Single-Objective
                    'Call EVO_Einstellungen1.setStandard_PES(Common.Constants.EVO_MODUS.Single_Objective)
                    'ElseIf (Common.Manager.AnzPenalty > 1) Then
                    'Multi-Objective
                    'Call EVO_Einstellungen1.setStandard_PES(Common.Constants.EVO_MODUS.Multi_Objective)
                    'End If

                    'Parameterübergabe an PES
                    'Call Sim1.Parameter_Uebergabe(globalAnzPar, myPara)

                    'EVO_Verlauf zurücksetzen
                    'Call Me.EVO_Opt_Verlauf1.Initialisieren(EVO_Einstellungen1.Settings.PES.Pop.n_Runden, EVO_Einstellungen1.Settings.PES.Pop.n_Popul, EVO_Einstellungen1.Settings.PES.n_Gen, EVO_Einstellungen1.Settings.PES.n_Nachf)

            End Select

            'IniMethod OK -> Start Button aktivieren
            Me.Button_Start.Enabled = True

            'Mauszeiger wieder normal
            Cursor = Cursors.Default

            If (Me.Anwendung <> ANW_TESTPROBLEME) Then
                'Datensatz-Reset aktivieren
                Me.MenuItem_DatensatzZurücksetzen.Enabled = True
            End If

        End If

    End Sub

    'EVO_Einstellungen laden
    '***********************
    Friend Sub Load_EVO_Settings(ByVal sender As Object, ByVal e As System.EventArgs)

        'Dialog einrichten
        OpenFileDialog1.Filter = "XML-Dateien (*.xml)|*.xml"
        OpenFileDialog1.FileName = "EVO_Settings.xml"
        OpenFileDialog1.Title = "Einstellungsdatei auswählen"
        If (Not IsNothing(Sim1)) Then
            OpenFileDialog1.InitialDirectory = Sim1.WorkDir
        Else
            OpenFileDialog1.InitialDirectory = CurDir()
        End If

        'Dialog anzeigen
        If (OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            Call EVO_Einstellungen1.loadSettings(OpenFileDialog1.FileName)
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
            SaveFileDialog1.InitialDirectory = Sim1.WorkDir
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
            Me.Button_Start.Text = ">"
            Do While (Me.ispause)
                Application.DoEvents()
            Loop

        ElseIf (Me.isrun) Then
            'Optimierung weiterlaufen lassen
            '-------------------------------
            Me.ispause = False
            Me.Button_Start.Text = "||"

        Else
            'Optimierung starten
            '-------------------
            Me.isrun = True
            Me.Button_Start.Text = "||"

            'Ergebnis-Buttons
            If (Not IsNothing(Sim1)) Then
                Me.Button_saveMDB.Enabled = True
                Me.Button_Scatterplot.Enabled = True
                Me.Button_loadRefResult.Enabled = True
            End If

            'EVO_Einstellungen temporär speichern
            Dim dir As String
            dir = My.Computer.FileSystem.SpecialDirectories.Temp & "\"
            Call Me.EVO_Einstellungen1.saveSettings(dir & "EVO_Settings.xml")

            'BackgroundWorker für Multithreading einrichten
            BackgroundWorker1 = New System.ComponentModel.BackgroundWorker
            AddHandler BackgroundWorker1.DoWork, AddressOf BackgroundWorker1_DoWork
            AddHandler BackgroundWorker1.RunWorkerCompleted, AddressOf BackgroundWorker1_RunWorkerCompleted
            'AddHandler BackgroundWorker1.ProgressChanged, AddressOf BackgroundWorker1_ProgressChanged

            Select Case Anwendung

                Case ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM

                    Select Case Method
                        Case METH_SENSIPLOT
                            Call STARTEN_SensiPlot()
                        Case METH_PES
                            Call STARTEN_PES()
                        Case METH_CES
                            Call STARTEN_CES_or_HYBRID()
                        Case METH_HYBRID
                            Call STARTEN_CES_or_HYBRID()
                        Case METH_HOOKJEEVES
                            Call STARTEN_HookJeeves()
                    End Select

                Case ANW_TESTPROBLEME
                    Call STARTEN_PES()

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
            Me.Button_Start.Text = ">"
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
        Dim ind As Common.Individuum
        Dim serie As Steema.TeeChart.Styles.Series
        Dim surface As New Steema.TeeChart.Styles.Surface
        Dim SimReihe As Wave.Zeitreihe
        Dim SimReihen As Collection
        Dim Wave1 As Wave.Wave

        'Instanzieren
        SimReihen = New Collection

        'Parameter
        Me.globalAnzPar = Sim1.List_OptParameter.Length
        Anz_SensiPara = SensiPlot1.Selected_OptParameter.GetLength(0)

        'Individuum wird initialisiert
        Call Common.Individuum.Initialise(1, 0, Me.globalAnzPar)

        'Anzahl Simulationen
        If (Anz_SensiPara = 1) Then
            '1 Parameter
            Anz_Sim = SensiPlot1.Anz_Steps
        Else
            '2 Parameter
            Anz_Sim = SensiPlot1.Anz_Steps ^ 2
        End If

        'Startwerte werden der Verlaufsanzeige zugewiesen
        Call Me.EVO_Opt_Verlauf1.Initialisieren(1, 1, 1, Anz_Sim)
        Call Me.EVO_Opt_Verlauf1.Runden(1)
        Call Me.EVO_Opt_Verlauf1.Population(1)
        Call Me.EVO_Opt_Verlauf1.Generation(1)

        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        'Oberflächendiagramm
        If (Anz_SensiPara > 1) Then
            surface = New Steema.TeeChart.Styles.Surface(Me.Hauptdiagramm.Chart)
            surface.IrregularGrid = True
            surface.NumXValues = SensiPlot1.Anz_Steps
            surface.NumZValues = SensiPlot1.Anz_Steps
            'Diagramm drehen (rechter Mausbutton)
            Dim rotate1 As New Steema.TeeChart.Tools.Rotate
            rotate1.Button = Windows.Forms.MouseButtons.Right
            Me.Hauptdiagramm.Tools.Add(rotate1)
            'MarksTips
            Me.Hauptdiagramm.add_MarksTips(surface)
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
                        Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Xn = Rnd()
                    Case "Diskret"
                        Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Xn = i / (SensiPlot1.Anz_Steps - 1)
                End Select
            End If

            'Innere Schleife (1. OptParameter)
            '---------------------------------
            For j = 0 To SensiPlot1.Anz_Steps - 1

                '1. OptParameterwert variieren
                Select Case SensiPlot1.Selected_SensiType
                    Case "Gleichverteilt"
                        Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Xn = Rnd()
                    Case "Diskret"
                        Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Xn = j / (SensiPlot1.Anz_Steps - 1)
                End Select

                n += 1

                'Verlaufsanzeige aktualisieren
                Me.EVO_Opt_Verlauf1.Nachfolger(n)

                'Individuum instanzieren
                ind = New Common.Individuum("SensiPlot", n)

                'OptParameter ins Individuum kopieren
                ind.PES_OptParas = Sim1.List_OptParameter

                'Modellparameter schreiben
                Call Sim1.Write_ModellParameter()

                'Evaluieren
                'TODO: Fehlerbehandlung bei Simulationsfehler
                isOK = Sim1.SIM_Evaluierung(ind)

                'BUG 253: Verletzte Constraints bei SensiPlot kenntlich machen?

                'Diagramm aktualisieren
                If (Anz_SensiPara = 1) Then
                    '1 Parameter
                    serie = Me.Hauptdiagramm.getSeriesPoint("SensiPlot", "Orange")
                    serie.Add(ind.Penalties(SensiPlot1.Selected_OptZiel), Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).RWert, n.ToString())
                Else
                    '2 Parameter
                    surface.Add(Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).RWert, ind.Penalties(SensiPlot1.Selected_OptZiel), Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).RWert, n.ToString())
                End If

                'Simulationsergebnis in Wave laden
                If (SensiPlot1.show_Wave) Then
                    'SimReihe auslesen
                    SimReihe = Sim1.SimErgebnis(Common.Manager.List_OptZiele(SensiPlot1.Selected_OptZiel).SimGr)
                    'Lösungs-ID an Titel anhängen
                    SimReihe.Title += " (Lösung " & n.ToString() & ")"
                    'SimReihe zu Collection hinzufügen
                    SimReihen.Add(SimReihe)
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
        Call TSP1.TeeChart_Initialise_TSP(Me.Hauptdiagramm)

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
                Call TSP1.TeeChart_Zeichnen_TSP(Me.Hauptdiagramm, TSP1.ParentList(0).Image)
            End If

            'Kinder werden Hier vollständig gelöscht
            Call TSP1.Reset_Childs()

            'Reproduktionsoperatoren, hier gehts dezent zur Sache
            Call TSP1.Reproduction_Control()

            'Mutationsoperatoren
            Call TSP1.Mutation_Control()

        Next gen

    End Sub

    'Anwendung CES und CES_PES             
    '*************************
    Private Sub STARTEN_CES_or_HYBRID()

        'Hypervolumen instanzieren
        '-------------------------
        Dim Hypervolume As EVO.MO_Indicators.Indicators
        Hypervolume = EVO.MO_Indicators.MO_IndicatorFabrik.GetInstance(EVO.MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, Common.Manager.AnzPenalty)

        'CES initialisieren
        '******************
        CES1 = New EVO.Kern.CES()
        Call CES1.CESInitialise(EVO_Einstellungen1.Settings, Method, Sim1.CES_T_Modus, Common.Manager.AnzPenalty, Common.Manager.AnzConstraints, Sim1.List_Locations.GetLength(0), Sim1.VerzweigungsDatei.GetLength(0), Sim1.n_Combinations, Sim1.n_PathDimension)

        'EVO_Verlauf zurücksetzen
        '************************
        Call Me.EVO_Opt_Verlauf1.Initialisieren(1, 1, EVO_Einstellungen1.Settings.CES.n_Generations, EVO_Einstellungen1.Settings.CES.n_Childs)

        Dim durchlauf_all As Integer = 0
        Dim serie As Steema.TeeChart.Styles.Series
        Dim ColorArray(CES1.ModSett.n_Locations, -1) As Object

        'Laufvariable für die Generationen
        Dim i_gen, i_ch, i_loc As Integer

        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        'Zufällige Kinderpfade werden generiert
        '**************************************
        Call CES1.Generate_Random_Path()
        'Falls TESTMODUS werden sie überschrieben
        If Not Sim1.CES_T_Modus = Common.Constants.CES_T_MODUS.No_Test Then
            Call CES1.Generate_Paths_for_Tests(Sim1.TestPath, Sim1.CES_T_Modus)
        End If
        '**************************************

        'Hier werden dem Child die passenden Massnahmen und deren Elemente pro Location zugewiesen
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        For i_ch = 0 To CES1.Settings.CES.n_Childs - 1
            For i_loc = 0 To CES1.ModSett.n_Locations - 1
                Call Sim1.Identify_Measures_Elements_Parameters(i_loc, CES1.Childs(i_ch).Path(i_loc), CES1.Childs(i_ch).Measures(i_loc), CES1.Childs(i_ch).Loc(i_loc).Loc_Elem, CES1.Childs(i_ch).Loc(i_loc).PES_OptPara)
            Next
        Next

        'Falls HYBRID werden entprechend der Einstellung im PES die Parameter auf Zufällig oder Start gesetzt
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        If Method = METH_HYBRID And EVO_Einstellungen1.Settings.CES.ty_Hybrid = Common.Constants.HYBRID_TYPE.Mixed_Integer Then
            CES1.Set_Xn_And_Dn_per_Location()
        End If

        'Startwerte werden der Verlaufsanzeige zugewiesen
        Call Me.EVO_Opt_Verlauf1.Initialisieren(1, 1, EVO_Einstellungen1.Settings.CES.n_Generations, EVO_Einstellungen1.Settings.CES.n_Childs)

        'xxxx Optimierung xxxxxx
        'Generationsschleife CES
        'xxxxxxxxxxxxxxxxxxxxxxx
        For i_gen = 0 To CES1.Settings.CES.n_Generations - 1

            Call EVO_Opt_Verlauf1.Generation(i_gen + 1)

            'Child Schleife
            'xxxxxxxxxxxxxx
            For i_ch = 0 To CES1.Settings.CES.n_Childs - 1
                durchlauf_all += 1

                'Do Schleife: Um Modellfehler bzw. Evaluierungsabbrüche abzufangen
                Dim Eval_Count As Integer = 0
                Do
                    CES1.Childs(i_ch).ID = durchlauf_all
                    Call EVO_Opt_Verlauf1.Nachfolger(i_ch + 1)

                    '****************************************
                    'Aktueller Pfad wird an Sim zurückgegeben
                    'Bereitet das BlaueModell für die Kombinatorik vor
                    Call Sim1.PREPARE_Evaluation_CES(CES1.Childs(i_ch).Path, CES1.Childs(i_ch).Get_All_Loc_Elem)

                    'HYBRID: Bereitet für die Optimierung mit den PES Parametern vor
                    '***************************************************************
                    If Method = METH_HYBRID And EVO_Einstellungen1.Settings.CES.ty_Hybrid = Common.Constants.HYBRID_TYPE.Mixed_Integer Then
                        If Sim1.Reduce_OptPara_and_ModPara(CES1.Childs(i_ch).Get_All_Loc_Elem) Then
                            Call Sim1.PREPARE_Evaluation_PES(CES1.Childs(i_ch).Get_All_Loc_PES_Para)
                        End If
                    End If

                    'Simulation *************************************************************************
                    SIM_Eval_is_OK = False

                    'Der BackgroundWorker startet die Simulation **********
                    Call BackgroundWorker1.RunWorkerAsync(CES1.Childs(i_ch))

                    While Me.BackgroundWorker1.IsBusy
                        System.Threading.Thread.Sleep(20)
                        Application.DoEvents()
                    End While
                    '************************************************************************************

                    'HYBRID: Speichert die PES Erfahrung diesen Childs im PES Memory
                    '***************************************************************
                    If Method = METH_HYBRID And EVO_Einstellungen1.Settings.CES.ty_Hybrid = Common.Constants.HYBRID_TYPE.Mixed_Integer Then
                        Call CES1.Memory_Store(i_ch, i_gen)
                    End If

                    'Lösung im TeeChart einzeichnen
                    '==============================
                    If (SIM_Eval_is_OK) Then
                        Call Me.LösungZeichnen(CES1.Childs(i_ch), 0, 0, i_gen, i_ch, ColorManagement(ColorArray, CES1.Childs(i_ch)))
                    End If

                    Eval_Count += 1
                    If (Eval_Count >= 10) Then
                        Throw New Exception("Es konnte kein gültiger Datensatz erzeugt werden!")
                    End If

                Loop While SIM_Eval_is_OK = False

                System.Windows.Forms.Application.DoEvents()
            Next
            '^ ENDE der Child Schleife
            'xxxxxxxxxxxxxxxxxxxxxxx

            'Die Listen müssen nach der letzten Evaluierung wieder zurückgesetzt werden
            'Sicher ob das benötigt wird?
            Call Sim1.Reset_OptPara_and_ModPara()

            'MO oder SO SELEKTIONSPROZESS oder NDSorting SELEKTION
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            'BUG 259: CES: Punkt-Labels der Sekundärpopulation fehlen noch!
            If (Common.Manager.AnzPenalty = 1) Then
                'Sortieren der Kinden anhand der Qualität
                Call CES1.Sort_Individuum(CES1.Childs)
                'Selectionsprozess je nach "plus" oder "minus" Strategie
                Call CES1.Selection_Process()
                'Zeichnen der besten Eltern
                For i_ch = 0 To EVO_Einstellungen1.Settings.CES.n_Parents - 1
                    'durchlauf += 1
                    serie = Me.Hauptdiagramm.getSeriesPoint("Parent", "green")
                    Call serie.Add(durchlauf_all, CES1.Parents(i_ch).Penalties(0))
                Next
            Else
                'NDSorting ******************
                Call CES1.NDSorting_CES_Control(i_gen)

                'Sekundäre Population
                '--------------------
                If (Not IsNothing(Sim1)) Then
                    'SekPop abspeichern
                    Call Sim1.OptResult.setSekPop(CES1.SekundärQb, i_gen)
                End If

                'SekPop zeichnen
                Call PopulationZeichnen(CES1.SekundärQb)

                'Hypervolumen berechnen und zeichnen
                '-----------------------------------
                Call Hypervolume.update_dataset(Common.Individuum.Get_All_Penalty_of_Array(CES1.SekundärQb))
                Call Me.HyperVolumenZeichnen(i_gen, Math.Abs(Hypervolume.calc_indicator()), Hypervolume.nadir)
            End If
            ' ^ ENDE Selectionsprozess
            'xxxxxxxxxxxxxxxxxxxxxxxxx

            'REPRODUKTION und MUTATION Nicht wenn Testmodus
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            If Sim1.CES_T_Modus = Common.Constants.CES_T_MODUS.No_Test Then
                'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
                Call Common.Individuum.New_Indi_Array("Child", CES1.Childs)
                'Reproduktionsoperatoren, hier gehts dezent zur Sache
                Call CES1.Reproduction_Control()
                'Mutationsoperatoren
                Call CES1.Mutation_Control()
            End If

            'Hier werden dem Child die passenden Massnahmen und deren Elemente pro Location zugewiesen
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            For i_ch = 0 To EVO_Einstellungen1.Settings.CES.n_Childs - 1
                For i_loc = 0 To CES1.ModSett.n_Locations - 1
                    Call Sim1.Identify_Measures_Elements_Parameters(i_loc, CES1.Childs(i_ch).Path(i_loc), CES1.Childs(i_ch).Measures(i_loc), CES1.Childs(i_ch).Loc(i_loc).Loc_Elem, CES1.Childs(i_ch).Loc(i_loc).PES_OptPara)
                Next
            Next

            'HYBRID: REPRODUKTION und MUTATION
            '*********************************
            If Method = METH_HYBRID And EVO_Einstellungen1.Settings.CES.ty_Hybrid = Common.Constants.HYBRID_TYPE.Mixed_Integer Then
                Call Mixed_Integer_PES(i_gen)
            End If

        Next
        'Ende der Generationsschleife CES

        'Falls jetzt noch PES ausgeführt werden soll
        'Starten der PES mit der Front von CES
        '*******************************************
        If Method = METH_HYBRID And EVO_Einstellungen1.Settings.CES.ty_Hybrid = Common.Constants.HYBRID_TYPE.Sequencial_1 Then
            Call Start_PES_after_CES()
        End If

    End Sub

    'Mixed_Integer Teil ermittelt die PES Parameter für jedes neues Child und jede Location
    '**************************************************************************************
    Private Sub Mixed_Integer_PES(ByVal i_gen As Integer)

        Dim i_ch, i_loc As Integer

        'NDSorting für den PES Memory
        '****************************
        If CES1.PES_Memory.GetLength(0) > CES1.Settings.CES.n_PES_MemSize Then
            Call CES1.NDSorting_Memory(i_gen)
        End If

        'pro Child
        'xxxxxxxxx
        For i_ch = 0 To CES1.Childs.GetUpperBound(0)

            'Ermittelt fuer jedes Child den PES Parent Satz (PES_Parents ist das Ergebnis)
            Call CES1.Memory_Search_per_Child(CES1.Childs(i_ch))

            'und pro Location
            'xxxxxxxxxxxxxxxx
            For i_loc = 0 To CES1.ModSett.n_Locations - 1

                'Die Parameter (falls vorhanden) werden überschrieben
                If Not CES1.Childs(i_ch).Loc(i_loc).PES_OptPara.GetLength(0) = 0 Then

                    'Ermittelt fuer jede Location den PES Parent Satz (PES_Parents ist das Ergebnis)
                    '*******************************************************************************
                    Call CES1.Memory_Search_per_Location(i_loc)

                    'Führt das NDSorting für diesen Satz durch
                    '*****************************************
                    If CES1.PES_Parents_pLoc.GetLength(0) > CES1.Settings.PES.n_Eltern Then
                        Call CES1.NDSorting_PES_Parents_per_Loc(i_gen)
                    End If

                    Dim m As Integer
                    Select Case CES1.PES_Parents_pLoc.GetLength(0)

                        Case Is = 0
                            'Noch keine Eltern vorhanden (die Child Location bekommt neue - zufällige Werte oder original Parameter)
                            '*******************************************************************************************************
                            For m = 0 To CES1.Childs(i_ch).Loc(i_loc).PES_OptPara.GetUpperBound(0)
                                CES1.Childs(i_ch).Loc(i_loc).PES_OptPara(m).Dn = CES1.Settings.PES.Schrittweite.DnStart
                                'Falls zufällige Startwerte
                                If CES1.Settings.PES.OptStartparameter = Common.Constants.EVO_STARTPARAMETER.Zufall Then
                                    Randomize()
                                    CES1.Childs(i_ch).Loc(i_loc).PES_OptPara(m).Xn = Rnd()
                                End If
                            Next

                        Case Is > 0
                            'Eltern vorhanden (das PES wird gestartet)
                            '*****************************************
                            If CES1.PES_Parents_pLoc.GetLength(0) < CES1.Settings.PES.n_Eltern Then
                                'Falls es zu wenige sind wird mit den vorhandenen aufgefüllt
                                Call CES1.fill_Parents_per_Loc(CES1.PES_Parents_pLoc, CES1.Settings.PES.n_Eltern)
                            End If

                            'Schritt 0: PES - Objekt der Klasse PES wird erzeugt PES wird erzeugt
                            '*********************************************************************
                            Dim PES1 As EVO.Kern.PES
                            PES1 = New EVO.Kern.PES

                            'Vorbereitung um das PES zu initieren
                            '************************************
                            globalAnzPar = CES1.Childs(i_ch).Loc(i_loc).PES_OptPara.GetLength(0)
                            myPara = CES1.Childs(i_ch).Loc(i_loc).PES_OptPara.Clone

                            'Schritte 1 - 3: PES wird initialisiert (Weiteres siehe dort ;-)
                            '**************************************************************
                            Call PES1.PesInitialise(EVO_Einstellungen1.Settings, globalAnzPar, Common.Manager.AnzPenalty, Common.Manager.AnzConstraints, myPara, Method)

                            'Die PopulationsEltern des PES werden gefüllt
                            For m = 0 To CES1.PES_Parents_pLoc.GetUpperBound(0)
                                Call PES1.EsStartvalues(CES1.Settings.CES.is_PopMutStart, CES1.PES_Parents_pLoc(m).Loc(i_loc).PES_OptPara, m)
                            Next

                            'Startet die Prozesse evolutionstheoretischen Prozesse nacheinander
                            Call PES1.EsReproMut(EVO_Einstellungen1.Settings.CES.is_PopMutStart)

                            'Auslesen der Variierten Parameter
                            CES1.Childs(i_ch).Loc(i_loc).PES_OptPara = PES1.EsGetParameter()

                    End Select
                End If
            Next
        Next
    End Sub

    'Starten der PES mit der Front von CES
    '(MaxAnzahl ist die Zahl der Eltern -> ToDo: SecPop oder Bestwertspeicher)
    '*************************************************************************
    Private Sub Start_PES_after_CES()
        Dim i As Integer

        For i = 0 To EVO_Einstellungen1.Settings.CES.n_Parents - 1
            If CES1.Parents(i).Front = 1 Then

                '****************************************
                'Aktueller Pfad wird an Sim zurückgegeben
                'Bereitet das BlaueModell für die Kombinatorik vor
                Call Sim1.PREPARE_Evaluation_CES(CES1.Childs(i).Path, CES1.Childs(i).Get_All_Loc_Elem)

                'Hier werden Child die passenden Elemente zugewiesen
                Dim j As Integer
                For j = 0 To CES1.ModSett.n_Locations - 1
                    Call Sim1.Identify_Measures_Elements_Parameters(j, CES1.Childs(i).Path(j), CES1.Childs(i).Measures(j), CES1.Childs(i).Loc(j).Loc_Elem, CES1.Childs(i).Loc(j).PES_OptPara)
                Next

                'Reduktion der OptimierungsParameter und immer dann wenn nicht Nullvariante
                'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                If Sim1.Reduce_OptPara_and_ModPara(CES1.Childs(i).Get_All_Loc_Elem) Then

                    'Parameterübergabe an PES
                    '************************
                    Call Sim1.Parameter_Uebergabe(globalAnzPar, myPara)
                    'Starten der PES
                    '***************
                    Call STARTEN_PES()

                End If
            End If
        Next
    End Sub

    'Anwendung des Verfahrens von Hook und Jeeves zur Parameteroptimierung
    '*********************************************************************
    Private Sub STARTEN_HookJeeves()

        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim b As Boolean
        Dim ind As Common.Individuum
        Dim QNBest() As Double = {}
        Dim QBest() As Double = {}
        Dim aktuellePara(Me.globalAnzPar - 1) As Double
        Dim SIM_Eval_is_OK As Boolean
        Dim durchlauf As Long
        Dim Iterationen As Long
        Dim Tastschritte_aktuell As Long
        Dim Tastschritte_gesamt As Long
        Dim Extrapolationsschritte As Long
        Dim Rueckschritte As Long

        Dim HookJeeves As EVO.Kern.HookeAndJeeves = New EVO.Kern.HookeAndJeeves(globalAnzPar, EVO_Einstellungen1.Settings.HookJeeves.DnStart, EVO_Einstellungen1.Settings.HookJeeves.DnFinish)

        'Individuum wird initialisiert
        Call Common.Individuum.Initialise(1, 0, Me.globalAnzPar)

        ReDim QNBest(Common.Manager.AnzPenalty - 1)
        ReDim QBest(Common.Manager.AnzPenalty - 1)

        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        durchlauf = 0
        Tastschritte_aktuell = 0
        Tastschritte_gesamt = 0
        Extrapolationsschritte = 0
        Rueckschritte = 0
        Iterationen = 0
        b = False

        Call HookJeeves.Initialize(Common.OptParameter.MyParaDouble(myPara))

        'Initialisierungssimulation
        Call Common.OptParameter.MyParaDouble(myPara).CopyTo(aktuellePara, 0)
        QNBest(0) = 1.79E+308
        QBest(0) = 1.79E+308
        k = 0

        Do While (HookJeeves.AktuelleSchrittweite > HookJeeves.MinimaleSchrittweite)

            Iterationen += 1
            durchlauf += 1

            'Bestimmen der Ausgangsgüte
            '==========================
            'Individuum instanzieren
            ind = New Common.Individuum("HJ", durchlauf)

            'HACK: OptParameter ins Individuum kopieren
            For i = 0 To ind.PES_OptParas.Length - 1
                ind.PES_OptParas(i).Xn = aktuellePara(i)
            Next

            'Vorbereiten des Modelldatensatzes
            Call Sim1.PREPARE_Evaluation_PES(aktuellePara)

            'Evaluierung des Simulationsmodells (ToDo: Validätsprüfung fehlt)
            SIM_Eval_is_OK = Sim1.SIM_Evaluierung(ind)

            'Lösung im TeeChart einzeichnen
            '------------------------------
            Dim serie As Steema.TeeChart.Styles.Series
            serie = Me.Hauptdiagramm.getSeriesPoint("Hook and Jeeves")
            Call serie.Add(durchlauf, ind.Penalties(0), durchlauf.ToString())

            Call My.Application.DoEvents()

            'Penalties in Bestwert kopieren
            Call ind.Penalties.CopyTo(QNBest, 0)

            'Tastschritte
            '============
            For j = 0 To HookJeeves.AnzahlParameter - 1

                aktuellePara = HookJeeves.Tastschritt(j, Kern.HookeAndJeeves.TastschrittRichtung.Vorwärts)

                Tastschritte_aktuell += 1
                durchlauf += 1
                Me.EVO_Einstellungen1.Label_HJ_TSaktuelle.Text = Tastschritte_aktuell.ToString

                'Individuum instanzieren
                ind = New Common.Individuum("HJ", durchlauf)

                'HACK: OptParameter ins Individuum kopieren
                For i = 0 To ind.PES_OptParas.Length - 1
                    ind.PES_OptParas(i).Xn = aktuellePara(i)
                Next

                'Vorbereiten des Modelldatensatzes
                Call Sim1.PREPARE_Evaluation_PES(aktuellePara)

                'Evaluierung des Simulationsmodells
                SIM_Eval_is_OK = Sim1.SIM_Evaluierung(ind)

                'Lösung im TeeChart einzeichnen
                '------------------------------
                serie = Me.Hauptdiagramm.getSeriesPoint("Hook and Jeeves")
                Call serie.Add(durchlauf, ind.Penalties(0), durchlauf.ToString())

                Call My.Application.DoEvents()

                If (ind.Penalties(0) >= QNBest(0)) Then

                    aktuellePara = HookJeeves.Tastschritt(j, Kern.HookeAndJeeves.TastschrittRichtung.Rückwärts)

                    Tastschritte_aktuell += 1
                    durchlauf += 1
                    Me.EVO_Einstellungen1.Label_HJ_TSaktuelle.Text = Tastschritte_aktuell.ToString

                    'Individuum instanzieren
                    ind = New Common.Individuum("HJ", durchlauf)

                    'HACK: OptParameter ins Individuum kopieren
                    For i = 0 To ind.PES_OptParas.Length - 1
                        ind.PES_OptParas(i).Xn = aktuellePara(i)
                    Next

                    'Vorbereiten des Modelldatensatzes
                    Call Sim1.PREPARE_Evaluation_PES(aktuellePara)

                    'Evaluierung des Simulationsmodells
                    SIM_Eval_is_OK = Sim1.SIM_Evaluierung(ind)

                    'Lösung im TeeChart einzeichnen
                    '------------------------------
                    serie = Me.Hauptdiagramm.getSeriesPoint("Hook and Jeeves")
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
                serie = Me.Hauptdiagramm.getSeriesPoint("Hook and Jeeves Best", "Green")
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

    'Anwendung Evolutionsstrategie für Parameter Optimierung - hier Steuerung       
    '************************************************************************
    Private Sub STARTEN_PES()

        Dim durchlauf As Integer
        Dim ind As Common.Individuum
        Dim PES1 As EVO.Kern.PES

        'Hypervolumen instanzieren
        '-------------------------
        Dim Hypervolume As EVO.MO_Indicators.Indicators
        Hypervolume = EVO.MO_Indicators.MO_IndicatorFabrik.GetInstance(EVO.MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, Common.Manager.AnzPenalty)

        '--------------------------
        'Dimensionierung der Variablen für Optionen Evostrategie
        'Das Struct aus PES wird hier verwendet
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        'TODO: If (ipop + igen + inachf + irunde) > 4 Then GoTo Start_Evolutionsrunden '????? Wie?
        'Werte an Variablen übergeben auskommentiert Werte finden sich im PES werden hier aber nicht zugewiesen
        'Kann der Kommentar nicht weg?
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        'Diagramm vorbereiten und initialisieren
        If (Not Form1.Method = METH_HYBRID And Not EVO_Einstellungen1.Settings.CES.ty_Hybrid = Common.Constants.HYBRID_TYPE.Sequencial_1) Then
            Call PrepareDiagramm()
        End If

        'Individuum wird initialisiert
        Call Common.Individuum.Initialise(1, 0, globalAnzPar)

        'Schritte 0: Objekt der Klasse PES wird erzeugt
        '**********************************************
        PES1 = New EVO.Kern.PES()

        'Schritte 1 - 3: ES wird initialisiert (Weiteres siehe dort ;-)
        '**************************************************************
        Call PES1.PesInitialise(EVO_Einstellungen1.Settings, globalAnzPar, Common.Manager.AnzPenalty, Common.Manager.AnzConstraints, myPara, Method)

        'Startwerte werden der Verlaufsanzeige zugewiesen
        Call Me.EVO_Opt_Verlauf1.Initialisieren(EVO_Einstellungen1.Settings.PES.Pop.n_Runden, EVO_Einstellungen1.Settings.PES.Pop.n_Popul, EVO_Einstellungen1.Settings.PES.n_Gen, EVO_Einstellungen1.Settings.PES.n_Nachf)

        durchlauf = 0

Start_Evolutionsrunden:

        'Über alle Runden
        'xxxxxxxxxxxxxxxx
        For PES1.PES_iAkt.iAktRunde = 0 To EVO_Einstellungen1.Settings.PES.Pop.n_Runden - 1

            Call EVO_Opt_Verlauf1.Runden(PES1.PES_iAkt.iAktRunde + 1)
            Call PES1.EsResetPopBWSpeicher() 'Nur bei Komma Strategie

            'Über alle Populationen
            'xxxxxxxxxxxxxxxxxxxxxx
            For PES1.PES_iAkt.iAktPop = 0 To EVO_Einstellungen1.Settings.PES.Pop.n_Popul - 1

                Call EVO_Opt_Verlauf1.Population(PES1.PES_iAkt.iAktPop + 1)

                'POPULATIONS REPRODUKTIONSPROZESS
                '################################
                'Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern der Population
                Call PES1.EsPopReproduktion()

                'POPULATIONS MUTATIONSPROZESS
                '############################
                'Mutieren der Ausgangswerte der Population
                Call PES1.EsPopMutation()

                'Über alle Generationen
                'xxxxxxxxxxxxxxxxxxxxxx
                For PES1.PES_iAkt.iAktGen = 0 To EVO_Einstellungen1.Settings.PES.n_Gen - 1

                    Call EVO_Opt_Verlauf1.Generation(PES1.PES_iAkt.iAktGen + 1)
                    Call PES1.EsResetBWSpeicher()  'Nur bei Komma Strategie

                    'Über alle Nachkommen
                    'xxxxxxxxxxxxxxxxxxxxxxxxx
                    For PES1.PES_iAkt.iAktNachf = 0 To EVO_Einstellungen1.Settings.PES.n_Nachf - 1

                        Call EVO_Opt_Verlauf1.Nachfolger(PES1.PES_iAkt.iAktNachf + 1)
                        durchlauf += 1

                        'Do Schleife: Um Modellfehler bzw. Evaluierungsabbrüche abzufangen
                        Dim Eval_Count As Integer = 0
                        Do
                            'Neues Individuum instanzieren
                            ind = New Common.Individuum("PES", durchlauf)

                            'REPRODUKTIONSPROZESS
                            '####################
                            'Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern
                            Call PES1.EsReproduktion()

                            'MUTATIONSPROZESS
                            '################
                            'Mutieren der Ausgangswerte
                            Call PES1.EsMutation()

                            'Auslesen der Variierten Parameter
                            myPara = PES1.EsGetParameter()

                            'OptParameter in Individuum kopieren
                            ind.PES_OptParas = myPara

                            'Ansteuerung der zu optimierenden Anwendung
                            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                            Select Case Anwendung

                                Case ANW_TESTPROBLEME

                                    Call Testprobleme1.Evaluierung_TestProbleme(ind, PES1.PES_iAkt.iAktPop, Me.Hauptdiagramm)
                                    SIM_Eval_is_OK = True

                                Case ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM

                                    'Vorbereiten des Modelldatensatzes
                                    Call Sim1.PREPARE_Evaluation_PES(myPara)

                                    'Simulation *************************************************************************
                                    SIM_Eval_is_OK = False

                                    'Der BackgroundWorker startet die Simulation **********
                                    Call BackgroundWorker1.RunWorkerAsync(ind)   '*********

                                    While Me.BackgroundWorker1.IsBusy
                                        System.Threading.Thread.Sleep(20)
                                        Application.DoEvents()
                                    End While
                                    '************************************************************************************

                                    'Lösung zeichnen
                                    If (SIM_Eval_is_OK) Then
                                        Call Me.LösungZeichnen(ind, PES1.PES_iAkt.iAktRunde, PES1.PES_iAkt.iAktPop, PES1.PES_iAkt.iAktGen, PES1.PES_iAkt.iAktNachf, Color.Orange)
                                    End If
                            End Select

                            Eval_Count += 1
                            If (Eval_Count >= 10) Then
                                Throw New Exception("Es konnte kein gültiger Datensatz erzeugt werden!")
                            End If

                        Loop While SIM_Eval_is_OK = False

                        'SELEKTIONSPROZESS Schritt 1
                        '###########################
                        'Einordnen der Qualitätsfunktion im Bestwertspeicher bei SO
                        'Falls MO Einordnen der Qualitätsfunktion in NDSorting
                        Call PES1.EsBest(ind)

                        System.Windows.Forms.Application.DoEvents()

                    Next 'Ende Schleife über alle Nachkommen

                    'SELEKTIONSPROZESS Schritt 2 für NDSorting sonst Xe = Xb
                    '#######################################################
                    'Die neuen Eltern werden generiert
                    Call PES1.EsEltern()

                    'Sekundäre Population
                    '====================
                    If (EVO_Einstellungen1.Settings.PES.OptModus = Common.Constants.EVO_MODUS.Multi_Objective) Then

                        'SekPop abspeichern
                        '---------------
                        If (Not IsNothing(Sim1)) Then
                            Call Sim1.OptResult.setSekPop(PES1.SekundärQb, PES1.PES_iAkt.iAktGen)
                        End If

                        'SekPop zeichnen
                        '---------------
                        Call PopulationZeichnen(PES1.SekundärQb)

                        'Hypervolumen berechnen und Zeichnen
                        '-----------------------------------
                        Call Hypervolume.update_dataset(Common.Individuum.Get_All_Penalty_of_Array(PES1.SekundärQb))
                        Call Me.HyperVolumenZeichnen(PES1.PES_iAkt.iAktGen, Math.Abs(Hypervolume.calc_indicator()), Hypervolume.nadir)

                    End If

                    'ggf. alte Generation im Diagramm löschen
                    If (EVO_Einstellungen1.Settings.PES.is_paint_constraint) Then
                        Call Me.ClearLastGeneration(PES1.PES_iAkt.iAktPop)
                    End If

                    System.Windows.Forms.Application.DoEvents()

                Next 'Ende alle Generationen
                'xxxxxxxxxxxxxxxxxxxxxxxxxxx
                System.Windows.Forms.Application.DoEvents()

                'POPULATIONS SELEKTIONSPROZESS  Schritt 1
                '########################################
                'Einordnen der Qualitätsfunktion im PopulationsBestwertspeicher
                Call PES1.EsPopBest()

            Next 'Ende alle Populationen
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

            'POPULATIONS SELEKTIONSPROZESS  Schritt 2
            '########################################
            'Die neuen Populationseltern werden generiert
            Call PES1.EsPopEltern()
            System.Windows.Forms.Application.DoEvents()

        Next 'Ende alle Runden
        'xxxxxxxxxxxxxxxxxxxxx

    End Sub

#End Region 'Start Button Pressed

#Region "Diagrammfunktionen"

    'Diagrammfunktionen
    '###################

    'Zeichenfunktionen
    'XXXXXXXXXXXXXXXXX

    'Lösung im Hauptdiagramm eintragen
    '**********************************
    Private Sub LösungZeichnen(ByVal ind As Common.Individuum, ByVal runde As Integer, ByVal pop As Integer, ByVal gen As Integer, ByVal nachf As Integer, _
                               ByVal Farbe As Color, Optional ByVal ColEach As Boolean = False)

        Dim serie As Steema.TeeChart.Styles.Series

        If (Common.Manager.AnzPenalty = 1) Then
            'SingleObjective
            'xxxxxxxxxxxxxxx
            If (Not ind.Is_Feasible) Then
                serie = Me.Hauptdiagramm.getSeriesPoint("Population " & (pop + 1).ToString() & " (ungültig)", "Gray", , , ColEach)
            Else
                serie = Me.Hauptdiagramm.getSeriesPoint("Population " & (pop + 1).ToString(), , , , ColEach)
            End If
            Call serie.Add(runde * EVO_Einstellungen1.Settings.PES.n_Gen * EVO_Einstellungen1.Settings.PES.n_Nachf + gen * EVO_Einstellungen1.Settings.PES.n_Nachf + nachf, ind.Penalties(0), ind.ID.ToString())

        Else
            'MultiObjective
            'xxxxxxxxxxxxxx
            If (Common.Manager.AnzPenalty = 2) Then
                '2D-Diagramm
                '------------------------------------------------------------------------
                If (Not ind.Is_Feasible) Then
                    serie = Me.Hauptdiagramm.getSeriesPoint("Population" & " (ungültig)", "Gray", , , ColEach)
                Else
                    serie = Me.Hauptdiagramm.getSeriesPoint("Population", "Orange", , , ColEach)
                End If
                Call serie.Add(ind.Penalties(0), ind.Penalties(1), ind.ID.ToString(), Farbe)

            Else
                '3D-Diagramm (Es werden die ersten drei Zielfunktionswerte eingezeichnet)
                '------------------------------------------------------------------------
                Dim serie3D As Steema.TeeChart.Styles.Points3D
                If (Not ind.Is_Feasible) Then
                    serie3D = Me.Hauptdiagramm.getSeries3DPoint("Population" & " (ungültig)", "Gray", , , ColEach)
                Else
                    serie3D = Me.Hauptdiagramm.getSeries3DPoint("Population", "Orange", , , ColEach)
                End If
                Call serie3D.Add(ind.Penalties(0), ind.Penalties(1), ind.Penalties(2), ind.ID.ToString(), Farbe)
            End If
        End If
    End Sub

    'Population zeichnen
    '*******************
    Private Sub PopulationZeichnen(ByVal pop() As Common.Individuum)

        Dim i As Short
        Dim serie As Steema.TeeChart.Styles.Series
        Dim serie3D As Steema.TeeChart.Styles.Points3D
        Dim values(,) As Double

        'Population in Array von Penalties transformieren
        values = Common.Individuum.Get_All_Penalty_of_Array(pop)

        If (Common.Manager.AnzPenalty = 2) Then
            '2 Zielfunktionen
            '----------------------------------------------------------------
            serie = Me.Hauptdiagramm.getSeriesPoint("Sekundäre Population", "Green")
            serie.Clear()
            For i = 0 To values.GetUpperBound(0)
                serie.Add(values(i, 0), values(i, 1))
            Next i

        ElseIf (Common.Manager.AnzPenalty >= 3) Then
            '3 oder mehr Zielfunktionen (es werden die ersten drei angezeigt)
            '----------------------------------------------------------------
            serie3D = Me.Hauptdiagramm.getSeries3DPoint("Sekundäre Population", "Green")
            serie3D.Clear()
            For i = 0 To values.GetUpperBound(0)
                serie3D.Add(values(i, 0), values(i, 1), values(i, 2))
            Next i
        End If

    End Sub

    'Ergebnisse der Hypervolumenberechnung anzeigen
    '**********************************************
    Private Sub HyperVolumenZeichnen(ByVal gen As Integer, ByVal indicator As Double, ByVal nadir() As Double)

        'Indicator in Indikatordiagramm eintragen
        Dim serie1 As Steema.TeeChart.Styles.Line
        serie1 = Me.Indicatordiagramm.getSeriesLine("Hypervolume")
        serie1.Add(gen, indicator, gen.ToString())

        'Nadirpunkt in Hauptdiagramm anzeigen
        If (Common.Manager.AnzPenalty = 2) Then
            '2D
            '--
            Dim serie2 As Steema.TeeChart.Styles.Points
            serie2 = Me.Hauptdiagramm.getSeriesPoint("Nadirpunkt", "Blue", Steema.TeeChart.Styles.PointerStyles.Diamond)
            serie2.Clear()
            serie2.Add(nadir(0), nadir(1), "Nadirpunkt")
        Else
            '3D
            '--
            Dim serie3 As Steema.TeeChart.Styles.Points3D
            serie3 = Me.Hauptdiagramm.getSeries3DPoint("Nadirpunkt", "Blue", Steema.TeeChart.Styles.PointerStyles.Diamond)
            serie3.Clear()
            serie3.Add(nadir(0), nadir(1), nadir(2), "Nadirpunkt")
        End If

    End Sub

    'Alte Generation im Hauptdiagramm löschen
    '****************************************
    Private Sub ClearLastGeneration(ByVal pop As Integer)

        Dim serie As Steema.TeeChart.Styles.Series

        If (Common.Manager.AnzPenalty = 1) Then
            'SingleObjective
            'xxxxxxxxxxxxxxx
            serie = Me.Hauptdiagramm.getSeriesPoint("Population " & (pop + 1).ToString() & " (ungültig)", "Gray")
            serie.Clear()
            serie = Me.Hauptdiagramm.getSeriesPoint("Population " & (pop + 1).ToString())
            serie.Clear()
        Else
            'MultiObjective
            'xxxxxxxxxxxxxx
            If (Common.Manager.AnzPenalty = 2) Then
                '2D-Diagramm
                '------------------------------------------------------------------------
                serie = Me.Hauptdiagramm.getSeriesPoint("Population (ungültig)", "Gray")
                serie.Clear()
                serie = Me.Hauptdiagramm.getSeriesPoint("Population", "Orange")
                serie.Clear()
            Else
                '3D-Diagramm
                '-----------
                Dim serie3D As Steema.TeeChart.Styles.Points3D
                serie3D = Me.Hauptdiagramm.getSeries3DPoint("Population (ungültig)", "Gray")
                serie3D.Clear()
                serie3D = Me.Hauptdiagramm.getSeries3DPoint("Population", "Orange")
                serie3D.Clear()
            End If
        End If

    End Sub

    'Hauptdiagramm vorbereiten
    '*************************
    Private Sub PrepareDiagramm()

        Dim i, j, tmpZielindex() As Integer
        Dim Achse As Diagramm.Achse
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

                Call Testprobleme1.DiagInitialise(Me.EVO_Einstellungen1.Settings, Me.Hauptdiagramm)

            Case ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                Select Case Method

                    Case METH_SENSIPLOT 'SensiPlot
                        'XXXXXXXXXXXXXXXXXXXXXXXXX

                        If (SensiPlot1.Selected_OptParameter.GetLength(0) = 1) Then

                            '1 OptParameter:
                            '---------------

                            'Achsen:
                            '-------
                            'X-Achse = QWert
                            Achse.Name = Common.Manager.List_OptZiele(SensiPlot1.Selected_OptZiel).Bezeichnung
                            Achse.Auto = True
                            Achse.Max = 0
                            Achsen.Add(Achse)
                            'Y-Achse = OptParameter
                            Achse.Name = Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Bezeichnung
                            Achse.Auto = True
                            Achse.Max = 0
                            Achsen.Add(Achse)

                            'Achsenzuordnung
                            'BUG 327!
                            For i = 0 To Common.Manager.AnzZiele - 1
                                If (Common.Manager.List_Ziele(i).Bezeichnung = Common.Manager.List_OptZiele(SensiPlot1.Selected_OptZiel).Bezeichnung) Then
                                    Me.Hauptdiagramm.ZielIndexX = i
                                    Exit For 'Abbruch
                                End If
                            Next
                            Me.Hauptdiagramm.ZielIndexY = -1
                            Me.Hauptdiagramm.ZielIndexZ = -1

                        Else
                            '2 OptParameter:
                            '---------------

                            'Achsen:
                            '-------
                            'X-Achse = OptParameter1
                            Achse.Name = Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Bezeichnung
                            Achse.Auto = True
                            Achse.Max = 0
                            Achsen.Add(Achse)
                            'Y-Achse = QWert
                            Achse.Name = Common.Manager.List_OptZiele(SensiPlot1.Selected_OptZiel).Bezeichnung
                            Achse.Auto = True
                            Achse.Max = 0
                            Achsen.Add(Achse)
                            'Z-Achse = OptParameter2
                            Achse.Name = Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Bezeichnung
                            Achse.Auto = True
                            Achse.Max = 0
                            Achsen.Add(Achse)

                            'Achsenzuordnung
                            'BUG 327!
                            Me.Hauptdiagramm.ZielIndexX = -1
                            For i = 0 To Common.Manager.AnzZiele - 1
                                If (Common.Manager.List_Ziele(i).Bezeichnung = Common.Manager.List_OptZiele(SensiPlot1.Selected_OptZiel).Bezeichnung) Then
                                    Me.Hauptdiagramm.ZielIndexY = i
                                    Exit For 'Abbruch
                                End If
                            Next
                            Me.Hauptdiagramm.ZielIndexZ = -1

                        End If

                        'Diagramm initialisieren
                        Call Me.Hauptdiagramm.DiagInitialise(Anwendung, Achsen)


                    Case Else 'PES, CES, CES + PES, HYBRID, HOOK & JEEVES
                        'XXXXXXXXXXXXXXXXXXXXX

                        'Achsen:
                        '-------
                        If (Common.Manager.AnzPenalty = 1) Then

                            'Single-Objective
                            '================

                            'X-Achse (Nr. der Simulation (Durchlauf))
                            '----------------------------------------
                            Achse.Name = "Simulation"
                            Achse.Auto = False
                            If (Form1.Method = METH_PES) Then
                                'Bei PES:
                                If (EVO_Einstellungen1.Settings.PES.Pop.is_POPUL) Then
                                    Achse.Max = EVO_Einstellungen1.Settings.PES.n_Gen * EVO_Einstellungen1.Settings.PES.n_Nachf * EVO_Einstellungen1.Settings.PES.Pop.n_Runden + 1
                                Else
                                    Achse.Max = EVO_Einstellungen1.Settings.PES.n_Gen * EVO_Einstellungen1.Settings.PES.n_Nachf + 1
                                End If

                            ElseIf (Form1.Method = METH_HOOKJEEVES) Then
                                'Bei Hooke & Jeeves:
                                Achse.Auto = True

                            Else
                                'Bei CES etc.:
                                Achse.Max = EVO_Einstellungen1.Settings.CES.n_Childs * EVO_Einstellungen1.Settings.CES.n_Generations
                            End If

                            Achsen.Add(Achse)

                            'Y-Achse: erste (und einzige) Zielfunktion
                            '-----------------------------------------
                            For i = 0 To Common.Manager.AnzZiele - 1
                                If (Common.Manager.List_Ziele(i).isOpt) Then
                                    Achse.Name = Common.Manager.List_Ziele(i).Bezeichnung
                                    Achse.Auto = True
                                    Achse.Max = 0
                                    Achsen.Add(Achse)
                                    Exit For 'Abbruch nach erstem OptZiel
                                End If
                            Next

                            Achsen.Add(Achse)

                            'Achsenzuordnung speichern
                            '-------------------------
                            Me.Hauptdiagramm.ZielIndexX = -1
                            Me.Hauptdiagramm.ZielIndexY = i
                            Me.Hauptdiagramm.ZielIndexZ = -1

                        Else

                            'Multi-Objective
                            '===============

                            'für jedes OptZiel eine Achse hinzufügen
                            j = 0
                            For i = 0 To Common.Manager.AnzZiele - 1
                                If (Common.Manager.List_Ziele(i).isOpt) Then
                                    Achse.Name = Common.Manager.List_Ziele(i).Bezeichnung
                                    Achse.Auto = True
                                    Achse.Max = 0
                                    Achsen.Add(Achse)
                                    tmpZielindex(j) = i
                                    j += 1
                                    If (j > 2) Then Exit For 'Maximal 3 Achsen
                                End If
                            Next

                            'Achsenzuordnung speichern:
                            '-------------------------
                            Me.Hauptdiagramm.ZielIndexX = tmpZielindex(0)
                            Me.Hauptdiagramm.ZielIndexY = tmpZielindex(1)
                            Me.Hauptdiagramm.ZielIndexZ = tmpZielindex(2)

                            'Warnung bei mehr als 3 OptZielen
                            If (Common.Manager.AnzPenalty > 3) Then
                                MsgBox("Die Anzahl der Optimierungsziele beträgt mehr als 3!" & eol _
                                        & "Es werden nur die ersten drei Zielfunktionen im Hauptdiagramm angezeigt!", MsgBoxStyle.Information, "Info")
                            End If

                        End If

                        'Diagramm initialisieren
                        Call Me.Hauptdiagramm.DiagInitialise(Anwendung, Achsen)

                        'IstWerte in Diagramm einzeichnen
                        Call Me.drawIstWerte()

                End Select

        End Select

        'Bei MultiObjective: 
        '-------------------
        If (Common.Manager.AnzPenalty > 1 _
            And Form1.Method <> METH_SENSIPLOT) Then

            'Indicator-Diagramm initialisieren
            '---------------------------------
            Call Me.DForm.showIndicatorDiagramm()
            Call Me.Indicatordiagramm.getSeriesLine("Hypervolume").Clear()

        End If

        Call Application.DoEvents()

    End Sub

    'IstWerte im Hauptdiagramm darstellen
    '************************************
    Private Sub drawIstWerte()

        'Hinweis:
        'Die Zuordnung von Achsen zu Zielfunktionen 
        'muss im Diagramm bereits hinterlegt sein!
        '-------------------------------------------

        Dim colorline1 As Steema.TeeChart.Tools.ColorLine

        'X-Achse:
        If (Me.Hauptdiagramm.ZielIndexX <> -1) Then
            If (Common.Manager.List_Ziele(Me.Hauptdiagramm.ZielIndexX).hasIstWert) Then
                colorline1 = New Steema.TeeChart.Tools.ColorLine(Me.Hauptdiagramm.Chart)
                colorline1.Pen.Color = Color.Red
                colorline1.AllowDrag = False
                colorline1.Draw3D = True
                colorline1.Axis = Me.Hauptdiagramm.Axes.Bottom
                colorline1.Value = Common.Manager.List_Ziele(Me.Hauptdiagramm.ZielIndexX).IstWert
            End If
        End If

        'Y-Achse:
        If (Me.Hauptdiagramm.ZielIndexY <> -1) Then
            If (Common.Manager.List_Ziele(Me.Hauptdiagramm.ZielIndexY).hasIstWert) Then
                colorline1 = New Steema.TeeChart.Tools.ColorLine(Me.Hauptdiagramm.Chart)
                colorline1.Pen.Color = Color.Red
                colorline1.AllowDrag = False
                colorline1.Draw3D = True
                colorline1.Axis = Me.Hauptdiagramm.Axes.Left
                colorline1.Value = Common.Manager.List_Ziele(Me.Hauptdiagramm.ZielIndexY).IstWert
            End If
        End If

        'Z-Achse:
        If (Me.Hauptdiagramm.ZielIndexZ <> -1) Then
            If (Common.Manager.List_Ziele(Me.Hauptdiagramm.ZielIndexZ).hasIstWert) Then
                'BUG 317: ColorLine auf Depth-Axis geht nicht!
                MsgBox("Der IstWert auf der Z-Achse (" & Common.Manager.List_Ziele(Me.Hauptdiagramm.ZielIndexZ).Bezeichnung & ") kann leider nicht angezeigt werden (Bug 317)", MsgBoxStyle.Information, "Info")
                'colorline1 = New Steema.TeeChart.Tools.ColorLine(Me.Hauptdiagramm.Chart)
                'colorline1.Pen.Color = Color.Red
                'colorline1.AllowDrag = False
                'colorline1.Draw3D = True
                'colorline1.Axis = Me.Hauptdiagramm.Axes.Depth
                'colorline1.Value = Common.Manager.List_Ziele(Me.Hauptdiagramm.ZielIndexZ).IstWert
            End If
        End If

    End Sub

    'Scatterplot-Matrix anzeigen
    '****************************
    Private Sub showScatterplot(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Scatterplot.Click

        Dim Dialog As ScatterplotDialog
        Dim diagresult As DialogResult
        Dim sekpoponly, showRef As Boolean
        Dim zielauswahl() As Integer

        'Scatterplot-Dialog aufrufen
        Dialog = New ScatterplotDialog()
        If (IsNothing(Sim1.OptResultRef)) Then Dialog.GroupBox_Ref.Enabled = False
        diagresult = Dialog.ShowDialog()

        If (Not diagresult = Windows.Forms.DialogResult.OK) Then
            Exit Sub
        End If

        'Einstellungen übernehmen
        sekpoponly = Dialog.CheckBox_SekPopOnly.Checked
        showRef = Dialog.CheckBox_showRef.Checked
        ReDim zielauswahl(-1)
        For Each indexChecked As Integer In Dialog.CheckedListBox_Ziele.CheckedIndices
            ReDim Preserve zielauswahl(zielauswahl.GetUpperBound(0) + 1)
            zielauswahl(zielauswahl.GetUpperBound(0)) = indexChecked
        Next

        'Scatterplot-Matrix anzeigen
        Cursor = Cursors.WaitCursor

        scatterplot1 = New Scatterplot(Sim1.OptResult, Sim1.OptResultRef, zielauswahl, sekpoponly, showRef)
        Call scatterplot1.Show()

        Cursor = Cursors.Default

        Call scatterplot1.BringToFront()

    End Sub

    'Speichert die verwendeten Farben für die bisherigen Pfade und generiert neue, falls erforderlich
    '************************************************************************************************
    Private Function ColorManagement(ByRef ColorAray(,) As Object, ByVal ind As Common.Individuum) As Color
        Dim i, j As Integer
        Dim count As Integer
        Dim Farbe As Color = Color.White

        'Falls der Pfad schon vorhanden ist wird diese Farbe verwendet
        For i = 0 To ColorAray.GetUpperBound(1)
            count = 0
            For j = 1 To ColorAray.GetUpperBound(0)
                If ColorAray(j, i) = ind.Path(j - 1) Then
                    count += 1
                End If
            Next
            If count = ind.Path.GetLength(0) Then
                Farbe = ColorAray(0, i)
            End If
        Next

        'Falls der Pfad nicht vorhanden ist wird eine neue generiert
        If Farbe = Color.White Then
            ReDim Preserve ColorAray(ColorAray.GetUpperBound(0), ColorAray.GetLength(1))
            Dim NeueFarbe As Boolean = True
            Dim CountFarbe As Integer = 0
            Do
                Randomize()
                Farbe = Drawing.Color.FromArgb(255, CInt(Int((25 * Rnd()) + 1)) * 10, _
                                                    CInt(Int((25 * Rnd()) + 1)) * 10, _
                                                    CInt(Int((25 * Rnd()) + 1)) * 10)
                For i = 0 To ColorAray.GetUpperBound(1)
                    If Farbe = ColorAray(0, i) Then
                        NeueFarbe = False
                    End If
                Next
                CountFarbe += 1
                If CountFarbe > 15000 Then Throw New Exception("Die Anzahl der farben für die verschiedenen Pfade ist erschöpft")
            Loop Until NeueFarbe = True
            ColorAray(0, ColorAray.GetUpperBound(1)) = Farbe
            For i = 1 To ColorAray.GetUpperBound(0)
                ColorAray(i, ColorAray.GetUpperBound(1)) = ind.Path(i - 1)
            Next
        End If

        Return Farbe

    End Function


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
                MsgBox("Lösung nicht auswählbar!", MsgBoxStyle.Information, "Info")
            End Try

        End If

    End Sub

    'Eine Lösung auswählen
    '*********************
    Private Sub selectSolution(ByVal ind As Common.Individuum) Handles scatterplot1.pointSelected

        Dim isOK As Boolean

        'Lösung zu ausgewählten Lösungen hinzufügen
        isOK = Sim1.OptResult.selectSolution(ind.ID)

        If (isOK) Then

            'Lösungsdialog initialisieren
            If (IsNothing(Me.solutionDialog)) Then
                Me.solutionDialog = New SolutionDialog(Sim1.List_OptParameter_Save, Sim1.List_Locations)
            End If

            'Lösungsdialog anzeigen
            Call Me.solutionDialog.Show()

            'Lösung zum Lösungsdialog hinzufügen
            Call Me.solutionDialog.addSolution(ind)

            'Lösung im Hauptdiagramm anzeigen
            Call Me.Hauptdiagramm.showSelectedSolution(ind)

            'Lösung im Scatterplot anzeigen
            If (Not IsNothing(Me.scatterplot1)) Then
                Call Me.scatterplot1.showSelectedSolution(ind)
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
        Call Me.Hauptdiagramm.clearSelection()

        'In der Scatterplot-Matrix
        '-------------------------
        If (Not IsNothing(Me.scatterplot1)) Then
            Call scatterplot1.clearSelection()
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

        Dim zre As Wave.Zeitreihe
        Dim SimSeries As New Collection                 'zu zeichnende Simulationsreihen
        Dim RefSeries As New Collection                 'zu zeichnende Referenzreihen

        'Wait cursor
        Cursor = Cursors.WaitCursor

        'Wave instanzieren
        Dim Wave1 As New Wave.Wave()

        'Sonderfall BlueM mit IHA-Berechnung 
        'ein 2. Wave für RVA-Diagramme instanzieren
        Dim Wave2 As Wave.Wave = Nothing
        If (TypeOf Me.Sim1 Is IHWB.EVO.BlueM) Then
            If (CType(Me.Sim1, IHWB.EVO.BlueM).isIHA) Then
                isIHA = True
                Wave2 = New Wave.Wave()
                Call Wave2.PrepareChart_RVA()
                'IHA-Vergleichsmodus?
                If (CType(Me.Sim1, IHWB.EVO.BlueM).IHAProc.isComparison) Then
                    'Referenz-RVAErgebnis in Wave2 laden
                    Call Wave2.Display_RVA(CType(Me.Sim1, IHWB.EVO.BlueM).IHAProc.RVABase)
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

            'Simulation vorbereiten
            'xxxxxxxxxxxxxxxxxxxxxx

            Select Case Form1.Method

                Case METH_PES

                    'Bereitet das BlueM für PES vor
                    Call Sim1.PREPARE_Evaluation_PES(ind.Get_All_PES_Para)

                Case METH_CES, METH_HYBRID

                    'Aktueller Pfad wird an Sim zurückgegeben
                    'Bereitet das BlaueModell für die Kombinatorik vor
                    Call Sim1.PREPARE_Evaluation_CES(ind.Path, ind.Get_All_Loc_Elem)

                    'HYBRID: Bereitet für die Optimierung mit den PES Parametern vor
                    If Form1.Method = METH_HYBRID And EVO_Einstellungen1.Settings.CES.ty_Hybrid = Common.Constants.HYBRID_TYPE.Mixed_Integer Then
                        Call Sim1.Reduce_OptPara_and_ModPara(ind.Get_All_Loc_Elem)
                        Call Sim1.PREPARE_Evaluation_PES(ind.Get_All_Loc_PES_Para)
                    End If

            End Select

            'Simulation ausführen
            'xxxxxxxxxxxxxxxxxxxx

            'Simulieren
            isOK = Sim1.launchSim()

            'Sonderfall IHA-Berechnung
            If (isIHA) Then
                'RVA-Ergebnis in Wave2 laden
                Dim RVAResult As Wave.RVA.Struct_RVAValues
                RVAResult = CType(Me.Sim1, IHWB.EVO.BlueM).IHASys.RVAResult
                'Lösungsnummer an Titel anhängen
                RVAResult.Title = "Lösung " & ind.ID.ToString()
                Call Wave2.Display_RVA(RVAResult)
            End If

            'Zu zeichnenden Simulationsreihen zurücksetzen
            SimSeries.Clear()

            'zu zeichnenden Reihen aus Liste der Ziele raussuchen
            '----------------------------------------------------
            For Each ziel As Common.Ziel In Common.Manager.List_Ziele

                With ziel

                    'Referenzreihe in Wave laden
                    '---------------------------
                    If (.ZielTyp = "Reihe" Or .ZielTyp = "IHA") Then
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
        Me.SaveFileDialog1.InitialDirectory = Sim1.WorkDir
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

        'Datei-öffnen Dialog anzeigen
        Me.OpenFileDialog1.Filter = "Access-Datenbanken (*.mdb)|*.mdb"
        Me.OpenFileDialog1.Title = "Ergebnisdatenbank auswählen"
        Me.OpenFileDialog1.FileName = ""
        Me.OpenFileDialog1.InitialDirectory = Sim1.WorkDir
        diagresult = Me.OpenFileDialog1.ShowDialog()

        If (diagresult = Windows.Forms.DialogResult.OK) Then

            sourceFile = Me.OpenFileDialog1.FileName

            'MDBImportDialog
            '---------------
            Dim importDialog As New MDBImportDialog()

            diagresult = importDialog.ShowDialog()

            If (diagresult = Windows.Forms.DialogResult.OK) Then

                'Cursor Wait
                Cursor = Cursors.WaitCursor

                'Daten einlesen
                '==============
                Call Sim1.OptResult.db_load(sourceFile)

                'Hauptdiagramm
                '=============

                'Achsenzuordnung
                '---------------
                Me.Hauptdiagramm.ZielIndexX = importDialog.ListBox_ZieleX.SelectedIndex
                Me.Hauptdiagramm.ZielIndexY = importDialog.ListBox_ZieleY.SelectedIndex
                Me.Hauptdiagramm.ZielIndexZ = importDialog.ListBox_ZieleZ.SelectedIndex

                'Achsen
                '------
                Dim Achsen As New Collection
                Dim tmpAchse As EVO.Diagramm.Achse
                tmpAchse.Auto = True
                If (Me.Hauptdiagramm.ZielIndexZ = -1 And Me.Hauptdiagramm.ZielIndexY = -1) Then
                    'Single-objective
                    '----------------
                    'X-Achse
                    tmpAchse.Name = "Simulation"
                    Achsen.Add(tmpAchse)
                    'Y-Achse
                    tmpAchse.Name = Common.Manager.List_Ziele(Me.Hauptdiagramm.ZielIndexX).Bezeichnung
                    Achsen.Add(tmpAchse)
                Else
                    'Multi-objective
                    '---------------
                    'X-Achse
                    tmpAchse.Name = Common.Manager.List_Ziele(Me.Hauptdiagramm.ZielIndexX).Bezeichnung
                    Achsen.Add(tmpAchse)
                    'Y-Achse
                    tmpAchse.Name = Common.Manager.List_Ziele(Me.Hauptdiagramm.ZielIndexY).Bezeichnung
                    Achsen.Add(tmpAchse)
                    If (Not Me.Hauptdiagramm.ZielIndexZ = -1) Then
                        'Z-Achse
                        tmpAchse.Name = Common.Manager.List_Ziele(Me.Hauptdiagramm.ZielIndexZ).Bezeichnung
                        Achsen.Add(tmpAchse)
                    End If
                End If

                'Diagramm initialisieren
                '-----------------------
                Me.Hauptdiagramm.Clear()
                Me.Hauptdiagramm.DiagInitialise(Path.GetFileName(sourceFile), Achsen)

                'IstWerte in Diagramm einzeichnen
                Call Me.drawIstWerte()

                Call My.Application.DoEvents()

                'Punkte eintragen
                '----------------
                Dim serie As Steema.TeeChart.Styles.Series
                Dim serie3D As Steema.TeeChart.Styles.Points3D

                'Lösungen
                '========
                If (importDialog.ComboBox_SekPop.SelectedItem <> "ausschließlich") Then

                    For Each ind As Common.Individuum In Sim1.OptResult.Solutions

                        If (Me.Hauptdiagramm.ZielIndexZ = -1 And Me.Hauptdiagramm.ZielIndexY = -1) Then
                            '1D
                            '--
                            'Constraintverletzung prüfen
                            If (ind.Is_Feasible) Then
                                serie = Me.Hauptdiagramm.getSeriesPoint("Population", "red")
                            Else
                                serie = Me.Hauptdiagramm.getSeriesPoint("Population (ungültig)", "Gray")
                            End If
                            'Zeichnen
                            serie.Add(ind.ID, ind.Zielwerte(Me.Hauptdiagramm.ZielIndexX), ind.ID.ToString())
                        ElseIf (Me.Hauptdiagramm.ZielIndexZ = -1) Then
                            '2D
                            '--
                            'Constraintverletzung prüfen
                            If (ind.Is_Feasible) Then
                                serie = Me.Hauptdiagramm.getSeriesPoint("Population", "Orange")
                            Else
                                serie = Me.Hauptdiagramm.getSeriesPoint("Population (ungültig)", "Gray")
                            End If
                            'Zeichnen
                            serie.Add(ind.Zielwerte(Me.Hauptdiagramm.ZielIndexX), ind.Zielwerte(Me.Hauptdiagramm.ZielIndexY), ind.ID.ToString())
                        Else
                            '3D
                            '--
                            'Constraintverletzung prüfen
                            If (ind.Is_Feasible) Then
                                serie3D = Me.Hauptdiagramm.getSeries3DPoint("Population", "Orange")
                            Else
                                serie3D = Me.Hauptdiagramm.getSeries3DPoint("Population (ungültig)", "Gray")
                            End If
                            'Zeichnen
                            serie3D.Add(ind.Zielwerte(Me.Hauptdiagramm.ZielIndexX), ind.Zielwerte(Me.Hauptdiagramm.ZielIndexY), ind.Zielwerte(Me.Hauptdiagramm.ZielIndexZ), ind.ID.ToString())
                        End If

                    Next

                End If

                Call My.Application.DoEvents()

                'Sekundärpopulation
                '==================
                If (importDialog.ComboBox_SekPop.SelectedItem <> "keine") Then

                    For Each sekpopind As Common.Individuum In Sim1.OptResult.getSekPop()
                        If (Me.Hauptdiagramm.ZielIndexZ = -1) Then
                            '2D
                            '--
                            serie = Me.Hauptdiagramm.getSeriesPoint("Sekundäre Population", "Green")
                            serie.Add(sekpopind.Zielwerte(Me.Hauptdiagramm.ZielIndexX), sekpopind.Zielwerte(Me.Hauptdiagramm.ZielIndexY), sekpopind.ID.ToString())
                        Else
                            '3D
                            '--
                            serie3D = Me.Hauptdiagramm.getSeries3DPoint("Sekundäre Population", "Green")
                            serie3D.Add(sekpopind.Zielwerte(Me.Hauptdiagramm.ZielIndexX), sekpopind.Zielwerte(Me.Hauptdiagramm.ZielIndexY), sekpopind.Zielwerte(Me.Hauptdiagramm.ZielIndexZ), sekpopind.ID.ToString())
                        End If
                    Next

                End If

                Call My.Application.DoEvents()

                'Hypervolumen
                '============
                If (importDialog.CheckBox_Hypervol.Checked) Then

                    'Indicator-Diagramm anzeigen
                    Call Me.DForm.showIndicatorDiagramm()
                    Call Me.Indicatordiagramm.getSeriesLine("Hypervolume").Clear()

                    'Hypervolumen instanzieren
                    Dim Hypervolume As EVO.MO_Indicators.Indicators
                    Hypervolume = EVO.MO_Indicators.MO_IndicatorFabrik.GetInstance(EVO.MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, Common.Manager.AnzPenalty)
                    Dim indicator As Double
                    Dim nadir() As Double

                    'Alle Generationen durchlaufen
                    For Each sekpop As OptResult.Struct_SekPop In Sim1.OptResult.SekPops

                        'Hypervolumen berechnen
                        Call Hypervolume.update_dataset(Sim1.OptResult.getSekPopValues(sekpop.iGen))
                        indicator = Math.Abs(Hypervolume.calc_indicator())
                        nadir = Hypervolume.nadir

                        'Hypervolumen zeichnen
                        Call Me.HyperVolumenZeichnen(sekpop.iGen, indicator, nadir)

                        Call My.Application.DoEvents()

                    Next
                End If

                'Ergebnis-Buttons
                Me.Button_Scatterplot.Enabled = True
                Me.Button_loadRefResult.Enabled = True

                'Start-Button deaktivieren
                Me.Button_Start.Enabled = False

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

        'Datei-öffnen Dialog anzeigen
        Me.OpenFileDialog1.Filter = "Access-Datenbanken (*.mdb)|*.mdb"
        Me.OpenFileDialog1.Title = "Vergleichsergebnis: Ergebnisdatenbank auswählen"
        Me.OpenFileDialog1.FileName = ""
        Me.OpenFileDialog1.InitialDirectory = Sim1.WorkDir
        diagresult = Me.OpenFileDialog1.ShowDialog()

        If (diagresult = Windows.Forms.DialogResult.OK) Then

            sourceFile = Me.OpenFileDialog1.FileName

            'Cursor Wait
            Cursor = Cursors.WaitCursor

            'Daten einlesen
            '==============
            Sim1.OptResultRef = New EVO.OptResult()
            Call Sim1.OptResultRef.db_load(sourceFile, True)

            'In Diagramm anzeigen
            '====================
            Dim serie As Steema.TeeChart.Styles.Points
            Dim serie3D As Steema.TeeChart.Styles.Points3D

            For Each sekpopind As Common.Individuum In Sim1.OptResultRef.getSekPop()
                If (Me.Hauptdiagramm.ZielIndexZ = -1) Then
                    '2D
                    '--
                    serie = Me.Hauptdiagramm.getSeriesPoint("Vergleichsergebnis", "Blue")
                    serie.Add(sekpopind.Zielwerte(Me.Hauptdiagramm.ZielIndexX), sekpopind.Zielwerte(Me.Hauptdiagramm.ZielIndexY), "Vergleichsergebnis " & sekpopind.ID)
                Else
                    '3D
                    '--
                    serie3D = Me.Hauptdiagramm.getSeries3DPoint("Vergleichsergebnis", "Blue")
                    serie3D.Add(sekpopind.Zielwerte(Me.Hauptdiagramm.ZielIndexX), sekpopind.Zielwerte(Me.Hauptdiagramm.ZielIndexY), sekpopind.Zielwerte(Me.Hauptdiagramm.ZielIndexZ), sekpopind.ID & " (Vergleichsergebnis)")
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
            ReDim nadir(Common.Manager.AnzPenalty - 1)
            ReDim minmax(Common.Manager.AnzPenalty - 1)
            For i = 0 To Common.Manager.AnzPenalty - 1
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
            If (Me.Indicatordiagramm.Visible) Then

                'Instanzierung
                HypervolumeRef = EVO.MO_Indicators.MO_IndicatorFabrik.GetInstance(MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, minmax, nadir, sekpopvaluesRef)
                indicatorRef = -HypervolumeRef.calc_indicator()

                'Anzeige in IndicatorDiagramm
                Dim colorline1 As New Steema.TeeChart.Tools.ColorLine(Me.Indicatordiagramm.Chart)
                colorline1.Pen.Color = Color.Blue
                colorline1.Pen.Width = 2
                colorline1.AllowDrag = False
                colorline1.Axis = Me.Indicatordiagramm.Axes.Left
                colorline1.Value = indicatorRef
            End If

            'Cursor Default
            Cursor = Cursors.Default

        End If

    End Sub

#End Region 'Ergebnisdatenbank

    'Ermittelt beim Start die Anzahl der Physikalischen Prozessoren
    '**************************************************************
    Public Sub Anzahl_Prozessoren(ByRef PhysCPU As Integer, ByRef LogCPU As Integer)
        Dim mc As ManagementClass = New ManagementClass("Win32_Processor")
        Dim moc As ManagementObjectCollection = mc.GetInstances()
        Dim SocketDesignation As String = String.Empty
        Dim PhysCPUarray As ArrayList = New ArrayList

        Dim mo As ManagementObject
        For Each mo In moc
            LogCPU += 1
            SocketDesignation = mo.Properties("SocketDesignation").Value.ToString()
            If Not PhysCPUarray.Contains(SocketDesignation) Then
                PhysCPUarray.Add(SocketDesignation)
            End If
        Next
        PhysCPU = PhysCPUarray.Count

    End Sub

#Region "BackgroundWorker"

    'BackgroundWorker1 DoWork
    '************************
    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs)

        Dim SIM_Eval_is_OK As Boolean

        'Retrieve the input arguments *********************************************************
        Dim Input As Individuum = CType(e.Argument, Individuum)
        '**************************************************************************************

        'Priority
        System.Threading.Thread.CurrentThread.Priority = Threading.ThreadPriority.BelowNormal

        ''Settings für den Backgroundworker
        'BackgroundWorker1.WorkerReportsProgress = True
        'BackgroundWorker1.WorkerSupportsCancellation = True

        'Job **************************************
        SIM_Eval_is_OK = Sim1.SIM_Evaluierung(Input)

        ''Progress ********************************
        'Call BackgroundWorker1.ReportProgress(100)
        ''*****************************************

        'Return the complete string ******
        e.Result = SIM_Eval_is_OK
        '*********************************

    End Sub

    'BackgroundWorker1 RunWorkerCompleted
    '************************************
    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As System.Object, _
                                                     ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)

        SIM_Eval_is_OK = CType(e.Result, Boolean)

    End Sub

    ''BackgroundWorker1 ProgressChanged
    ''*********************************
    'Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As System.Object, _
    '                                              ByVal e As System.ComponentModel.ProgressChangedEventArgs)

    '    SIM_Eval_is_OK = e.ProgressPercentage

    'End Sub

#End Region 'BackgroundWorker

#End Region 'Methoden

End Class
