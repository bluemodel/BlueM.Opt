Option Strict Off ' Off ist Default
Option Explicit On
Imports System.IO

'*******************************************************************************
'*******************************************************************************
'**** ihwb Optimierung                                                      ****
'****                                                                       ****
'**** Dirk Muschalla, Christoph Huebner, Felix Froehlich                    ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'****                                                                       ****
'**** Erstellt: Dezember 2003                                               ****
'****                                                                       ****
'**** Letzte Änderung: April 2007                                           ****
'*******************************************************************************
'*******************************************************************************

Partial Class Form1
    Inherits System.Windows.Forms.Form

#Region "Eigenschaften"

    Private IsInitializing As Boolean

    'zu optimierende Anwendung
    Private Anwendung As String
    Private Const ANW_BM_RESET As String = "BlueM Reset"
    Private Const ANW_BM_SENSIPLOT As String = "BlueM SensiPlot"
    Private Const ANW_BM_PES As String = "BlueM PES"
    Private Const ANW_BM_CES As String = "BlueM CES"
    Private Const ANW_TESTPROBLEME As String = "Testprobleme"
    Private Const ANW_TSP As String = "Traveling Salesman"

    '**** Deklarationen der Module *****
    Public BlueM1 As New Apps.BlueM
    Public SensiPlot1 As New Apps.SensiPlot
    Public CES1 As New EvoKern.CES
    Public TSP1 as New Apps.TSP

    '**** Globale Parameter Parameter Optimierung ****
    Dim myIsOK As Boolean
    Dim myisrun As Boolean
    Dim globalAnzPar As Short
    Dim globalAnzZiel_ParaOpt As Short
    Dim globalAnzRand As Short
    Dim array_x() As Double
    Dim array_y() As Double
    Dim Bestwert(,) As Double = {}
    Dim Population(,) As Double
    Dim mypara(,) As Double

#End Region 'Eigenschaften

#Region "Methoden"

    'Initialisierung von Form1
    '*************************
    Private Sub Form1_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        'XP-look
        System.Windows.Forms.Application.EnableVisualStyles()

        'Liste der Anwendungen in ComboBox schreiben und Anfangseinstellung wählen
        ComboBox_Anwendung.Items.AddRange(New Object() {"", ANW_BM_RESET, ANW_BM_PES, ANW_BM_CES, ANW_BM_SENSIPLOT, ANW_TESTPROBLEME, ANW_TSP})
        ComboBox_Anwendung.SelectedIndex = 0

        'Ende der Initialisierung
        IsInitializing = False

    End Sub

#Region "Initialisierung der Anwendungen"

    'Die Anwendung wurde ausgewählt und wird jetzt initialisiert
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    'Auswahl der zu optimierenden Anwendung geändert
    '***********************************************
    Private Sub IniApp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_IniApp.Click, ComboBox_Anwendung.SelectedIndexChanged, Testprobleme1.Testproblem_Changed

        If (Me.IsInitializing = True) Then

            'Testprobleme und Evo Deaktivieren
            Testprobleme1.Enabled = False
            EVO_Einstellungen1.Enabled = False
            Exit Sub

        Else

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Mauszeiger busy
            Cursor = System.Windows.Forms.Cursors.WaitCursor

            Anwendung = ComboBox_Anwendung.SelectedItem

            Select Case Anwendung

                Case "" 'Keine Anwendung ausgewählt
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Testprobleme und Evo Deaktivieren
                    Testprobleme1.Enabled = False
                    EVO_Einstellungen1.Enabled = False
                    'Mauszeiger wieder normal
                    Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub


                Case ANW_BM_RESET 'Anwendung ResetPara & RunBM
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'eingestelltes Dezimaltrennzeichen überprüfen
                    Call CheckDezimaltrennzeichen()
                    'Voreinstellungen lesen EVO.INI
                    Call ReadEVOIni()
                    'Testprobleme und Evo Deaktivieren
                    Testprobleme1.Enabled = False
                    EVO_Einstellungen1.Enabled = False

                    'Ergebnisdatenbank ausschalten
                    BlueM1.Ergebnisdb = False
                    'BM-Einstellungen initialisieren 
                    Call BlueM1.Sim_Ini()

                    'Original ModellParameter werden geschrieben
                    Call BlueM1.ModellParameter_schreiben()

                    MsgBox("Die Startwerte der Optimierungsparameter wurden in die Eingabedateien geschrieben.", MsgBoxStyle.Information, "Info")


                Case ANW_BM_SENSIPLOT 'Anwendung SensiPlot
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'eingestelltes Dezimaltrennzeichen überprüfen
                    Call CheckDezimaltrennzeichen()
                    'Voreinstellungen lesen EVO.INI
                    Call ReadEVOIni()
                    'Testprobleme und Evo Deaktivieren
                    Testprobleme1.Enabled = False
                    EVO_Einstellungen1.Enabled = False

                    'Ergebnisdatenbank ausschalten
                    BlueM1.Ergebnisdb = False
                    'BM-Einstellungen initialisieren 
                    Call BlueM1.Sim_Ini()

                    'SensiPlot Dialog anzeigen:
                    '--------------------------
                    'List_Boxen füllen
                    Dim i As Integer
                    For i = 0 To BlueM1.OptParameterListe.GetUpperBound(0)
                        Call SensiPlot1.ListBox_OptParameter_add(BlueM1.OptParameterListe(i))
                    Next
                    For i = 0 To BlueM1.OptZieleListe.GetUpperBound(0)
                        Call SensiPlot1.ListBox_OptZiele_add(BlueM1.OptZieleListe(i))
                    Next
                    'Dialog anzeigen
                    Dim SensiPlotDiagResult As Windows.Forms.DialogResult
                    SensiPlotDiagResult = SensiPlot1.ShowDialog()
                    If (Not SensiPlotDiagResult = Windows.Forms.DialogResult.OK) Then
                        'Mauszeiger wieder normal
                        Cursor = System.Windows.Forms.Cursors.Default
                        Exit Sub
                    End If


                Case ANW_BM_PES 'Anwendung BlauesModell PES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'eingestelltes Dezimaltrennzeichen überprüfen
                    Call CheckDezimaltrennzeichen()
                    'Voreinstellungen lesen EVO.INI
                    Call ReadEVOIni()
                    'Evo aktivieren
                    EVO_Einstellungen1.Enabled = True
                    'Testprobleme ausschalten
                    Testprobleme1.Enabled = False

                    'BM-Einstellungen initialisieren 
                    Call BlueM1.Sim_Ini()

                    'Je nach Anzahl der Zielfunktionen von MO auf SO umschalten
                    If BlueM1.OptZieleListe.GetLength(0) = 1 Then
                        EVO_Einstellungen1.OptModus = 0
                    ElseIf BlueM1.OptZieleListe.GetLength(0) > 1 Then
                        EVO_Einstellungen1.OptModus = 1
                    End If

                    'Parameterübergabe an ES
                    Call BlueM1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel_ParaOpt, globalAnzRand, mypara)

                    'WEL-Chart vorbereiten


                Case ANW_BM_CES 'Anwendung BlauesModell CES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'eingestelltes Dezimaltrennzeichen überprüfen
                    Call CheckDezimaltrennzeichen()
                    'Voreinstellungen lesen EVO.INI
                    Call ReadEVOIni()
                    'Evo deaktiviern
                    EVO_Einstellungen1.Enabled = False
                    'Testprobleme ausschalten
                    Testprobleme1.Enabled = False
                    'Ergebnisdatenbank ausschalten
                    BlueM1.Ergebnisdb = False

                    'BM-Einstellungen initialisieren 
                    Call BlueM1.Sim_Ini()

                    'Je nach Anzahl der Zielfunktionen von MO auf SO umschalten
                    If BlueM1.OptZieleListe.GetLength(0) = 1 Then
                        EVO_Einstellungen1.OptModus = 0
                    ElseIf BlueM1.OptZieleListe.GetLength(0) > 1 Then
                        EVO_Einstellungen1.OptModus = 1
                    End If

                    'Einlesen der CombiOpt Datei
                    Call BlueM1.Read_CES()

                    'Überprüfen der Kombinatorik
                    Call BlueM1.Combinatoric_is_Valid()

                    'Einlesen der Verbraucher Datei
                    Call BlueM1.Verzweigung_Read()

                    'Prüfen ob Kombinatorik und Verzweigungsdatei zusammenpassen
                    Call BlueM1.CES_fits_to_VER()

                    'Call Initialisierung_BlauesModell_CombiOpt()

                    'Anzahl der Ziele, Locations und Verzeigungen wird an CES übergeben
                    CES1.n_Penalty = BlueM1.OptZieleListe.GetLength(0)
                    CES1.n_Location = BlueM1.LocationList.GetLength(0)
                    CES1.n_Verzweig = BlueM1.VerzweigungsDatei.GetLength(0)

                    'Gibt die PathSize an für jede Pfadstelle
                    Dim i As Integer
                    ReDim CES1.n_PathSize(CES1.n_Location - 1)
                    For i = 0 To CES1.n_Location - 1
                        CES1.n_PathSize(i) = BlueM1.LocationList(i).MassnahmeListe.GetLength(0)
                    Next

                Case ANW_TESTPROBLEME 'Anwendung Testprobleme
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Test-Probleme und Evo aktivieren
                    Testprobleme1.Enabled = True
                    EVO_Einstellungen1.Enabled = True
                    EVO_Einstellungen1.OptModus = Testprobleme1.OptModus
                    'Globale Parameter werden gesetzt
                    Call Testprobleme1.Parameter_Uebergabe(Testprobleme1.Combo_Testproblem.Text, Testprobleme1.Text_Sinusfunktion_Par.Text, Testprobleme1.Text_Schwefel24_Par.Text, globalAnzPar, globalAnzZiel_ParaOpt, globalAnzRand, mypara)


                Case ANW_TSP 'Anwendung Traveling Salesman Problem (TSP)
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    Call TSP1.TSP_Initialize(DForm.Diag)

            End Select

            'IniApp OK -> Start Button aktivieren
            Me.Button_Start.Enabled = True

            'Mauszeiger wieder normal
            Cursor = System.Windows.Forms.Cursors.Default

        End If

    End Sub

    'EVO.ini Datei einlesen 
    '**********************
    'BUG 94: ReadEVO.ini müsste hier raus nach BlueM und "Read_Model_OptConfig" heißen **
    Private Sub ReadEVOIni()

        If File.Exists("EVO.ini") Then

            'Datei einlesen
            Dim FiStr As FileStream = New FileStream("EVO.ini", FileMode.Open, IO.FileAccess.Read)
            Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

            Dim Configs(9, 1) As String
            Dim Line As String
            Dim Pairs() As String
            Dim i As Integer = 0
            Do
                Line = StrRead.ReadLine.ToString()
                If (Line.StartsWith("[") = False And Line.StartsWith(";") = False) Then
                    Pairs = Line.Split("=")
                    Configs(i, 0) = Pairs(0)
                    Configs(i, 1) = Pairs(1)
                    i += 1
                End If
            Loop Until StrRead.Peek() = -1

            StrRead.Close()
            FiStr.Close()

            'Default-Werte setzen
            For i = 0 To Configs.GetUpperBound(0)
                Select Case Configs(i, 0)
                    Case "BM_Exe"
                        BlueM1.Exe = Configs(i, 1)
                    Case "Datensatz"
                        'Dateiname vom Ende abtrennen
                        BlueM1.Datensatz = Configs(i, 1).Substring(Configs(i, 1).LastIndexOf("\") + 1)
                        'Dateiendung entfernen
                        BlueM1.Datensatz = BlueM1.Datensatz.Substring(0, BlueM1.Datensatz.Length - 4)
                        'Arbeitsverzeichnis bestimmen
                        BlueM1.WorkDir = Configs(i, 1).Substring(0, Configs(i, 1).LastIndexOf("\") + 1)
                    Case Else
                        'weitere Voreinstellungen
                End Select
            Next

            'Datensatzanzeige aktualisieren
            Me.LinkLabel_WorkDir.Text = BlueM1.WorkDir & BlueM1.Datensatz & ".ALL"
            Me.LinkLabel_WorkDir.Links(0).LinkData = BlueM1.WorkDir

        Else
            'Datei EVO.ini existiert nicht
            Throw New Exception("Die Datei ""EVO.ini"" konnte nicht gefunden werden!" & Chr(13) & Chr(10) & "Bitte gemäß Dokumentation eine Datei ""EVO.ini"" erstellen.")
        End If


    End Sub

    'Klick auf Datensatzanzeige
    '**************************
    Private Sub LinkLabel_WorkDir_LinkClicked( ByVal sender As System.Object,  ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel_WorkDir.LinkClicked
        System.Diagnostics.Process.Start(Me.LinkLabel_WorkDir.Links(0).LinkData)
    End Sub

#End Region

#Region "Start Button Pressed"

    'Start BUTTON wurde pressed
    'XXXXXXXXXXXXXXXXXXXXXXXXXX

    Private Sub STARTEN_Button_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Button_Start.Click

        'Try
        myisrun = True
        Select Case Anwendung
            Case ANW_BM_RESET
                Call BlueM1.launchSim()
            Case ANW_BM_SENSIPLOT
                Call STARTEN_SensiPlot()
            Case ANW_BM_PES
                Call STARTEN_BM_PES()
            Case ANW_BM_CES
                Call STARTEN_BM_CES()
            Case ANW_TESTPROBLEME
                Call STARTEN_BM_PES()
            Case ANW_TSP
                Call STARTEN_TSP()
        End Select

        ''Globale Fehlerbehandlung für Optimierungslauf:
        'Catch ex As Exception
        '    MsgBox(ex.Message, MsgBoxStyle.Critical, "Fehler")
        'End Try
    End Sub

    'Anwendung SensiPlot - START; läuft ohne Evolutionsstrategie             
    '***********************************************************
    Private Sub STARTEN_SensiPlot()

        'Einschränkung:
        '------------------------------------------------------------------------
        'Die Modellparameter werden auch für die nicht ausgewählten OptParameter 
        'geschrieben, und zwar mit den in der OPT-Datei angegebenen Startwerten
        '------------------------------------------------------------------------

        'Wave deklarieren
        Dim Wave1 As New Main.Wave
        ReDim Wave1.WaveList(SensiPlot1.Anz_Sim - 1)

        'Parameterübergabe an ES
        Me.globalAnzZiel_ParaOpt = 1
        Me.globalAnzRand = 0
        Me.globalAnzPar = 1

        Dim i As Integer
        Dim durchlauf As Integer = 1
        Dim ipop As Integer = 1

        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        'Simulationsschleife
        '-------------------
        Randomize()

        For i = 0 To SensiPlot1.Anz_Sim - 1

            'OptParameterwert variieren
            Select Case SensiPlot1.Selected_SensiType
                Case "Gleichverteilt"
                    BlueM1.OptParameterListe(SensiPlot1.Selected_OptParameter).SKWert = Rnd()
                Case "Diskret"
                    BlueM1.OptParameterListe(SensiPlot1.Selected_OptParameter).SKWert = i / SensiPlot1.Anz_Sim
            End Select

            'Modellparameter schreiben
            Call BlueM1.ModellParameter_schreiben()

            'Simulieren
            Call BlueM1.launchSim()

            'Qwert berechnen
            BlueM1.OptZieleListe(SensiPlot1.Selected_OptZiel).QWertTmp = BlueM1.QWert(BlueM1.OptZieleListe(SensiPlot1.Selected_OptZiel))

            'Diagramm aktualisieren
            DForm.Diag.Series(0).Add(BlueM1.OptZieleListe(SensiPlot1.Selected_OptZiel).QWertTmp, BlueM1.OptParameterListe(SensiPlot1.Selected_OptParameter).Wert, "")

            'Speichern des Simulationsergebnisses für Wave
            'Wave1.WaveList(i).Bezeichnung = SensiPlot1.Selected_OptZiel.SimGr & "(Sim " & i & ")"
            Apps.Sim.Read_WEL(BlueM1.WorkDir & BlueM1.Datensatz & ".wel", BlueM1.OptZieleListe(SensiPlot1.Selected_OptZiel).SimGr, Wave1.WaveList(i).Wave)

            durchlauf += 1

            System.Windows.Forms.Application.DoEvents()

        Next

        'Wave Diagramm anzeigen:
        '-----------------------
        'Achsen:
        Dim Achsen As New Collection
        Dim xAchse, yAchse As Diagramm.Achse
        xAchse.Name = "Zeit"
        xAchse.Auto = True
        Achsen.Add(xAchse)
        yAchse.Name = BlueM1.OptZieleListe(SensiPlot1.Selected_OptZiel).SimGr
        yAchse.Auto = True
        Achsen.Add(yAchse)

        Call Wave1.WForm.Diag.DiagInitialise("SensiPlot", Achsen)

        'Serien initialisieren
        Dim tmpSeries As Steema.TeeChart.Styles.Line
        For i = 1 To SensiPlot1.Anz_Sim
            tmpSeries = New Steema.TeeChart.Styles.Line(Wave1.WForm.Diag.Chart)
            tmpSeries.Title = "Sim " & i.ToString()
            tmpSeries.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Nothing
        Next

        'Serien zeichnen
        Call Wave1.Wave_draw()

        'Dialog anzeigen
        Call Wave1.Show()

    End Sub


    'Anwendung Traveling Salesman - Start                         
    '************************************
    Private Sub STARTEN_TSP()

        'Laufvariable für die Generationen
        Dim gen As Integer

        'BUG 85: Nach Klasse Diagramm auslagern!
        Call TSP1.TeeChart_Initialise_TSP(DForm.Diag)

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
                Call TSP1.TeeChart_Zeichnen_TSP(DForm.Diag, TSP1.ParentList(0).Image)
            End If

            'Kinder werden Hier vollständig gelöscht
            Call TSP1.Reset_Childs_TSP()

            'Reproduktionsoperatoren, hier gehts dezent zur Sache
            Call TSP1.Reproduction_Control()

            'Mutationsoperatoren
            Call TSP1.Mutation_Control()

        Next gen

    End Sub

    'Anwendung CombiBM - START; läuft ohne Evolutionsstrategie             
    '*********************************************************
    Private Sub STARTEN_BM_CES()

        'Fehlerabfragen
        If (BlueM1.OptZieleListe.GetLength(0) > 2) Then
            Throw New Exception("Zu viele Ziele für CES. Max=2")
        End If

        Dim durchlauf_all As Integer = 0

        'Laufvariable für die Generationen
        Dim gen As Integer
        Dim i As Integer

        'Parents und Child werden Dimensioniert
        Call CES1.Dim_Parents_BM()
        Call CES1.Dim_Childs()

        'Diagramm vorbereiten und initialisieren *****************************
        Call PrepareDiagramm()

        'Zufällige Kinderpfade werden generiert
        Call CES1.Generate_Random_Path()

        'HACK: Funktion zum manuellen testen aller Kombinationen
        'Call CES1.Generate_All_Test_Path()

        'Generationsschleife
        For gen = 0 To CES1.n_Generation - 1

            'Child Schleife
            For i = 0 To CES1.n_Childs - 1
                durchlauf_all += 1

                'Erstellt die aktuelle Bauerksliste und überträgt sie zu SKos
                Call BlueM1.Define_aktuelle_Bauwerke(CES1.ChildList(i).Path)

                'Ermittelt das aktuelle_ON_OFF array
                Call BlueM1.Verzweigung_ON_OFF(CES1.ChildList(i).Path)

                'Schreibt die neuen Verzweigungen
                Call BlueM1.Verzweigung_Write()

                'Evaluiert das Blaue Modell
                Call BlueM1.Eval_Sim_CombiOpt(CES1.n_Penalty, durchlauf_all, 1, CES1.ChildList(i).Penalty, DForm.Diag)

                'Zeichnen der Kinder
                Call DForm.Diag.prepareSeries(0, "Childs", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
                If CES1.n_Penalty = 1 Then
                    Call DForm.Diag.Series(0).Add(durchlauf_all, CES1.ChildList(i).Penalty(0))
                ElseIf CES1.n_Penalty = 2 Then
                    Call DForm.Diag.Series(0).Add(CES1.ChildList(i).Penalty(0), CES1.ChildList(i).Penalty(1))
                End If

                ''HACK zum zeichnen aller Qualitäten
                'Call TChart1.Series(2).Add(durchlauf, CES1.ChildList_BM(i).Quality_MO(0))
                'Call TChart1.Series(3).Add(durchlauf, CES1.ChildList_BM(i).Quality_MO(1))
                'Call TChart1.Series(4).Add(durchlauf, CES1.ChildList_BM(i).Quality_MO(2))
                System.Windows.Forms.Application.DoEvents()
            Next

            'MO oder SO
            If CES1.n_Penalty = 1 Then
                'Sortieren der Kinden anhand der Qualität
                Call CES1.Sort_Faksimile(CES1.ChildList)
                'Selectionsprozess je nach "plus" oder "minus" Strategie
                Call CES1.Selection_Process()
            ElseIf CES1.n_Penalty = 2 Then
                'NDSorting
                Call CES1.NDSorting_Control()

            End If

            'MO oder SO
            If CES1.n_Penalty = 1 Then
                'Zeichnen des besten Elter
                For i = 0 To CES1.n_Parents - 1
                    'durchlauf += 1
                    Call DForm.Diag.prepareSeries(1, "Parent", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                    Call DForm.Diag.Series(1).Add(durchlauf_all, CES1.ParentList(i).Penalty(0))
                Next
            ElseIf CES1.n_Penalty = 2 Then
                'Zeichnen von NDSortingResult
                Call DForm.Diag.DeleteSeries(CES1.n_Childs - 1, 1)

                Dim f As Integer
                For i = 0 To CES1.n_Childs - 1
                    f = CES1.NDSResult(i).Front
                    Call DForm.Diag.prepareSeries(f, "Front:" & f, Steema.TeeChart.Styles.PointerStyles.Circle, 4)
                    Call DForm.Diag.Series(f).Add(CES1.NDSResult(i).Penalty(0), CES1.NDSResult(i).Penalty(1))
                Next
            End If

            'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
            Call CES1.Reset_Childs_BM()

            'Reproduktionsoperatoren, hier gehts dezent zur Sache
            Call CES1.Reproduction_Control_BM()

            'Mutationsoperatoren
            Call CES1.Mutation_Control_BM()

        Next

    End Sub


    'Anwendung Evolutionsstrategie für Parameter Optimierung - hier Steuerung       
    '************************************************************************

    Private Sub STARTEN_BM_PES()
        '==========================
        Dim i As Integer
        '--------------------------
        Dim durchlauf As Integer
        '--------------------------
        Dim PES1 As EvoKern.PES
        '--------------------------
        'Variablen für Optionen Evostrategie
        Dim iEvoTyp, iPopEvoTyp As Integer
        Dim isPOPUL As Boolean
        Dim isMultiObjective, isPareto, isPareto3D As Boolean
        Dim NPopEltern, NRunden, NPopul, iOptPopEltern As Integer
        Dim iOptEltern, iPopPenalty As Integer
        Dim NEltern As Integer
        Dim NRekombXY As Integer
        Dim rDeltaStart As Single
        Dim iStartPar As Integer
        Dim isdnvektor As Boolean
        Dim NGen, NNachf As Integer
        Dim Interact As Short
        Dim isInteract As Boolean
        Dim NMemberSecondPop As Short
        '--------------------------
        Dim ipop As Short = 0
        Dim igen As Short
        Dim inachf As Short
        Dim irunde As Short
        Dim QN() As Double = {}
        Dim RN() As Double = {}
        '--------------------------

        'TODO: If (ipop + igen + inachf + irunde) > 4 Then GoTo Start_Evolutionsrunden
        '????? Wie?

        myisrun = True

        'Werte an Variablen übergeben
        iEvoTyp = EVO_Einstellungen1.iEvoTyp
        iPopEvoTyp = EVO_Einstellungen1.iPopEvoTyp
        isPOPUL = EVO_Einstellungen1.isPOPUL
        isMultiObjective = EVO_Einstellungen1.isMultiObjective
        isPareto = EVO_Einstellungen1.isPareto
        isPareto3D = False
        NRunden = EVO_Einstellungen1.NRunden
        NPopul = EVO_Einstellungen1.NPopul
        NPopEltern = EVO_Einstellungen1.NPopEltern
        iOptPopEltern = EVO_Einstellungen1.iOptPopEltern
        iOptEltern = EVO_Einstellungen1.iOptEltern
        iPopPenalty = EVO_Einstellungen1.iPopPenalty
        NGen = EVO_Einstellungen1.NGen
        NEltern = EVO_Einstellungen1.NEltern
        NNachf = EVO_Einstellungen1.NNachf
        NRekombXY = EVO_Einstellungen1.NRekombXY
        rDeltaStart = EVO_Einstellungen1.rDeltaStart
        isdnvektor = EVO_Einstellungen1.isDnVektor
        iStartPar = EVO_Einstellungen1.globalOPTVORGABE
        Interact = EVO_Einstellungen1.Interact
        isInteract = EVO_Einstellungen1.isInteract
        NMemberSecondPop = EVO_Einstellungen1.NMemberSecondPop

        ReDim QN(globalAnzZiel_ParaOpt)
        ReDim RN(globalAnzRand)

        'Kontrolle der Variablen
        If NRunden = 0 Or NPopul = 0 Or NPopEltern = 0 Then
            Throw New Exception("Anzahl der Runden, Populationen oder Populationseltern ist zu klein!")
        End If
        If NGen = 0 Or NEltern = 0 Or NNachf = 0 Then
            Throw New Exception("Anzahl der Generationen, Eltern oder Nachfolger ist zu klein!")
        End If
        If rDeltaStart < 0 Then
            Throw New Exception("Die Startschrittweite ist unzulässig oder kleiner als die minimale Schrittweite!")
        End If
        If globalAnzPar = 0 Then
            Throw New Exception("Die Anzahl der Parameter ist unzulässig!")
        End If
        If NPopul < NPopEltern Then
            Throw New Exception("Die Anzahl der Populationseltern darf nicht größer als die Anzahl der Populationen sein!")
        End If

        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        '1. Schritt: CEvolutionsstrategie
        'Objekt der Klasse CEvolutionsstrategie wird erzeugen
        '******************************************************************************************
        PES1 = New EvoKern.PES

        '2. Schritt: CEvolutionsstrategie - ES_INI
        'Die öffentlichen dynamischen Arrays werden initialisiert (Dn, An, Xn, Xmin, Xmax)
        'und die Anzahl der Zielfunktionen wird festgelegt
        '******************************************************************************************
        myIsOK = PES1.EsIni(globalAnzPar, globalAnzZiel_ParaOpt, globalAnzRand)

        '3. Schritt: CEvolutionsstrategie - ES_OPTIONS
        'Optionen der Evolutionsstrategie werden übergeben
        '******************************************************************************************
        myIsOK = PES1.EsOptions(iEvoTyp, iPopEvoTyp, isPOPUL, NRunden, NPopul, NPopEltern, iOptPopEltern, iOptEltern, iPopPenalty, NGen, NEltern, NNachf, NRekombXY, rDeltaStart, iStartPar, isdnvektor, isMultiObjective, isPareto, isPareto3D, Interact, isInteract, NMemberSecondPop)

        '4. Schritt: CEvolutionsstrategie - ES_LET_PARAMETER
        'Ausgangsparameter werden übergeben
        '******************************************************************************************
        For i = 1 To globalAnzPar
            myIsOK = PES1.EsLetParameter(i, mypara(i, 1))
        Next i

        '5. Schritt: CEvolutionsstrategie - ES_PREPARE
        'Interne Variablen werden initialisiert, Zufallsgenerator wird initialisiert
        '******************************************************************************************
        myIsOK = PES1.EsPrepare()

        '6. Schritt: CEvolutionsstrategie - ES_STARTVALUES
        'Startwerte werden zugewiesen
        '******************************************************************************************
        myIsOK = PES1.EsStartvalues()

        'Startwerte werden der Bedienoberfläche zugewiesen
        '******************************************************************************************
        EVO_Opt_Verlauf1.NRunden = PES1.NRunden
        EVO_Opt_Verlauf1.NPopul = PES1.NPopul
        EVO_Opt_Verlauf1.NGen = PES1.NGen
        EVO_Opt_Verlauf1.NNachf = PES1.NNachf
        EVO_Opt_Verlauf1.Initialisieren()

        durchlauf = 0

Start_Evolutionsrunden:
        'Cursor setzen
        'System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        'Loop über alle Runden
        '*******************************************************************************************
        Do While (PES1.EsIsNextRunde)

            irunde = PES1.iaktuelleRunde
            Call EVO_Opt_Verlauf1.Runden(irunde)

            myIsOK = PES1.EsPopBestwertspeicher()
            'Loop über alle Populationen
            '***********************************************************************************************
            Do While (PES1.EsIsNextPop)

                ipop = PES1.iaktuellePopulation
                Call EVO_Opt_Verlauf1.Populationen(ipop)

                myIsOK = PES1.EsPopVaria

                myIsOK = PES1.EsPopMutation

                'TODO: Scheint mir Schwachsinnig an dieser Stelle Weil es überschrieben wird
                durchlauf = NGen * NNachf * (irunde - 1)

                'Loop über alle Generationen
                '***********************************************************************************************
                Do While (PES1.EsIsNextGen)

                    igen = PES1.iaktuelleGeneration
                    Call EVO_Opt_Verlauf1.Generation(igen)

                    myIsOK = PES1.EsBestwertspeicher()

                    'Loop über alle Nachkommen
                    '********************************************************************
                    Do While (PES1.EsIsNextNachf)

                        inachf = PES1.iaktuellerNachfahre
                        Call EVO_Opt_Verlauf1.Nachfolger(inachf)

                        durchlauf = durchlauf + 1

                        'Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern
                        myIsOK = PES1.EsVaria

                        'Mutieren der Ausgangswerte
                        myIsOK = PES1.EsMutation

                        'Auslesen der Variierten Parameter
                        myIsOK = PES1.EsGetParameter(globalAnzPar, mypara)

                        'Auslesen des Bestwertspeichers
                        If Not PES1.isMultiObjective Then
                            myIsOK = PES1.EsGetBestwert(Bestwert)
                        End If

                        '************************************************************************************
                        '******************* Ansteuerung der zu optimierenden Anwendung *********************
                        '************************************************************************************
                        Select Case Anwendung
                            Case ANW_TESTPROBLEME
                                Call Testprobleme1.Evaluierung_TestProbleme(Testprobleme1.Combo_Testproblem.Text, globalAnzPar, mypara, durchlauf, ipop, QN, RN, DForm.Diag)
                            Case ANW_BM_PES
                                Call BlueM1.Eval_Sim_ParaOpt(globalAnzPar, globalAnzZiel_ParaOpt, mypara, durchlauf, ipop, QN, DForm.Diag)
                        End Select

                        'Einordnen der Qualitätsfunktion im Bestwertspeicher
                        '**************************************************************************
                        myIsOK = PES1.EsBest(QN, RN)

                        System.Windows.Forms.Application.DoEvents()

                        'Ende Loop über alle Nachkommen
                        '**************************************************************************
                    Loop


                    'Die neuen Eltern werden generiert
                    myIsOK = PES1.EsEltern()

                    'Bestwerte und sekundäre Population
                    If PES1.isMultiObjective Then
                        myIsOK = PES1.EsGetBestwert(Bestwert)
                        'TODO: Call Bestwertzeichnen_Pareto(Bestwert, ipop)
                        myIsOK = PES1.esGetSekundärePopulation(Population)
                        Call SekundärePopulationZeichnen(Population)
                    End If

                    System.Windows.Forms.Application.DoEvents()

                    'Ende Loop über alle Generationen
                    '***********************************************************************************************
                Loop 'Schleife über alle Generationen

                System.Windows.Forms.Application.DoEvents()

                'Einordnen der Qualitätsfunktion im PopulationsBestwertspeicher
                myIsOK = PES1.EsPopBest()

                'Ende Loop über alle Populationen
                '***********************************************************************************************
            Loop 'Schleife über alle Populationen



            'Die neuen Populationseltern werden generiert
            myIsOK = PES1.EsPopEltern

            System.Windows.Forms.Application.DoEvents()

            'Ende Loop über alle Runden
            '***********************************************************************************************
        Loop 'Schleife über alle Runden

        'CEvolutionsstrategie, letzter. Schritt
        'Objekt der Klasse CEvolutionsstrategie wird vernichtet
        '***************************************************************************************************
        'UPGRADE_NOTE: Das Objekt evolutionsstrategie kann erst dann gelöscht werden, wenn die Garbagecollection durchgeführt wurde. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1029"'
        'TODO: Ersetzen durch dispose funzt net
        PES1 = Nothing

    End Sub


    'Zeichenfunktionen
    'XXXXXXXXXXXXXXXXX

    Private Sub Bestwertzeichnen_Pareto(ByRef Bestwert(,) As Double, ByRef ipop As Short)
        Dim i As Short
        With DForm.Diag
            .Series(ipop).Clear()
            If UBound(Bestwert, 2) = 2 Then
                For i = 1 To UBound(Bestwert, 1)
                    .Series(ipop).Add(Bestwert(i, 1), Bestwert(i, 2), "")
                Next i
            ElseIf UBound(Bestwert, 2) = 3 Then
                For i = 1 To UBound(Bestwert, 1)
                    'TODO: Hier muss eine 3D-Punkt angezeigt werden
                    '.Series(ipop).Add(Bestwert(i, 1), Bestwert(i, 2), Bestwert(i, 2), "")
                Next i
            End If
        End With
    End Sub

    Private Sub SekundärePopulationZeichnen(ByRef Population(,) As Double)
        Dim i As Short
        Dim Datenreihe As Short
        With DForm.Diag
            If EVO_Einstellungen1.isPOPUL Then
                Datenreihe = EVO_Einstellungen1.NPopul + 1
            Else
                Datenreihe = 1
            End If
            .Series(Datenreihe).Clear()
            If UBound(Population, 2) = 2 Then
                For i = 1 To UBound(Population, 1)
                    .Series(Datenreihe).Add(Population(i, 1), Population(i, 2), "")
                Next i
            ElseIf UBound(Population, 2) = 3 Then
                For i = 1 To UBound(Population, 1)
                    'TODO: Hier muss eine 3D-Reihe angezeigt werden
                    '.Series(Datenreihe).Add(Population(i, 1), Population(i, 2), Population(i, 3), "") 
                Next i
            End If
        End With
    End Sub

#End Region 'Start Button Pressed

    'Überprüfen, ob Punkt als Dezimaltrennzeichen eingestellt ist
    '***********************************************************
    Private Sub CheckDezimaltrennzeichen()

        Dim ci As System.Globalization.CultureInfo
        Dim nfi As System.Globalization.NumberFormatInfo

        'Aktuelle Einstellungen lesen
        ci = System.Globalization.CultureInfo.CurrentCulture
        nfi = ci.NumberFormat

        'Dezimaltrennzeichen überprüfen
        If (Not nfi.NumberDecimalSeparator = ".") Then
            Throw New Exception("Um mit BlueM arbeiten zu können, muss in der Systemsteuerung" & Chr(13) & Chr(10) & "als Dezimaltrennzeichen Punkt (.) eingestellt sein!")
        End If

    End Sub

#Region "Diagrammfunktionen"

    'Diagrammfunktionen
    '###################

    'Achsen und Standard-Series initialisieren
    '*****************************************
    Private Sub PrepareDiagramm()

        Dim i As Integer

        Select Case Anwendung

            Case ANW_TESTPROBLEME 'Testprobleme
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                Select Case Testprobleme1.Combo_Testproblem.Text
                    Case "Sinus-Funktion"
                        Call DForm.Diag.DiagInitialise_SinusFunktion(EVO_Einstellungen1, globalAnzPar, Testprobleme1.Text_Sinusfunktion_Par.Text)
                    Case "Beale-Problem" 'x1 = [-5;5], x2=[-2;2]
                        Call DForm.Diag.DiagInitialise_BealeProblem(EVO_Einstellungen1, globalAnzPar)
                    Case "Schwefel 2.4-Problem" 'xi = [-10,10]
                        Call DForm.Diag.DiagInitialise_SchwefelProblem(EVO_Einstellungen1, globalAnzPar)
                    Case Else
                        Call DForm.Diag.DiagInitialise_MultiTestProb(EVO_Einstellungen1, Testprobleme1.Combo_Testproblem.Text)
                End Select


            Case ANW_BM_SENSIPLOT 'SensiPlot
                'XXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Achsen:
                '-------
                Dim Achse As Diagramm.Achse
                Dim Achsen As New Collection
                'X-Achse = QWert
                Achse.Name = BlueM1.OptZieleListe(SensiPlot1.Selected_OptZiel).Bezeichnung
                Achse.Auto = True
                Achse.Max = 0
                Achsen.Add(Achse)
                'Y-Achse = OptParameter
                Achse.Name = BlueM1.OptParameterListe(SensiPlot1.Selected_OptParameter).Bezeichnung
                Achse.Auto = True
                Achse.Max = 0
                Achsen.Add(Achse)

                'Diagramm initialisieren
                Call DForm.Diag.DiagInitialise(Anwendung, Achsen)

                'Series initialisieren
                Dim tmpPoint As New Steema.TeeChart.Styles.Points(Me.DForm.Diag.Chart)
                tmpPoint.Title = "Simulationsergebnis"
                tmpPoint.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                tmpPoint.Color = System.Drawing.Color.Orange
                tmpPoint.Pointer.HorizSize = 2
                tmpPoint.Pointer.VertSize = 2


            Case ANW_BM_CES 'BlueM CES
                'XXXXXXXXXXXXXXXXXXXXX

                'Achsen:
                '-------
                Dim Achse As Diagramm.Achse
                Dim Achsen As New Collection
                'Bei SO: X-Achse = Simulationen
                If (EVO_Einstellungen1.isMultiObjective = False) Then
                    Achse.Name = "Simulation"
                    Achse.Auto = False
                    Achse.Max = CES1.n_Childs * CES1.n_Generation
                    Achsen.Add(Achse)
                End If
                'für jede Zielfunktion eine weitere Achse hinzufügen
                'HACK: Diagramm-Achsen bisher nur für Anwendung BlueM!
                For i = 0 To BlueM1.OptZieleListe.GetUpperBound(0)
                    Achse.Name = BlueM1.OptZieleListe(i).Bezeichnung
                    Achse.Auto = True
                    Achse.Max = 0
                    Achsen.Add(Achse)
                Next

                'Diagramm initialisieren
                Call DForm.Diag.DiagInitialise(Anwendung, Achsen)


            Case ANW_BM_PES 'BlueM PES
                'XXXXXXXXXXXXXXXXXXXXX

                Dim n_Kalkulationen As Integer
                Dim n_Populationen As Integer

                'Anzahl Kalkulationen
                n_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf

                'Anzahl Populationen
                n_Populationen = 1
                If EVO_Einstellungen1.isPOPUL Then
                    n_Populationen = EVO_Einstellungen1.NPopul
                End If

                'Achsen:
                '-------
                Dim Achse As Diagramm.Achse
                Dim Achsen As New Collection
                'Bei SO: X-Achse = Simulationen
                If (EVO_Einstellungen1.isMultiObjective = False) Then
                    Achse.Name = "Simulation"
                    Achse.Auto = False
                    Achse.Max = n_Kalkulationen
                    Achsen.Add(Achse)
                End If
                'für jede Zielfunktion eine weitere Achse hinzufügen
                'HACK: Diagramm-Achsen bisher nur für Anwendung BlueM!
                For i = 0 To BlueM1.OptZieleListe.GetUpperBound(0)
                    Achse.Name = BlueM1.OptZieleListe(i).Bezeichnung
                    Achse.Auto = True
                    Achse.Max = 0
                    Achsen.Add(Achse)
                Next

                'Diagramm initialisieren
                Call DForm.Diag.DiagInitialise(Anwendung, Achsen)

                'Standard-Series initialisieren
                If (EVO_Einstellungen1.isMultiObjective = False) Then
                    Call DForm.Diag.prepareSeries_SO(n_Populationen)
                Else
                    Call DForm.Diag.prepareSeries_MO()
                End If


                Case Else 'andere Anwendungen
                    'XXXXXXXXXXXXXXXXXXXXXXXX
                    Throw New Exception("Diese Funktion ist für die Anwendung '" & Anwendung & "' nicht vorgesehen")

        End Select

    End Sub

#End Region 'Diagrammfunktionen

#End Region 'Methoden

End Class
