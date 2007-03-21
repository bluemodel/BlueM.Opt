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
'**** TU Darmstadt                               Dezember 2006              ****
'****                                                                       ****
'**** Dezember 2003                                                         ****
'****                                                                       ****
'**** Letzte Änderung: März 2007                                            ****
'*******************************************************************************
'*******************************************************************************

Friend Class Form1

    '************************************************************************************
    '****** Form1 wird initialisiert bzw. geladen; weitere Module werden deklariert *****
    '************************************************************************************

    Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean

    Private Anwendung As String                'zu optimierende Anwendung
    Private Const ANW_RESETPARA_RUNBM As String = "ResetPara & RunBM"
    Private Const ANW_SENSIPLOT_MODPARA As String = "SensiPlot ModPara"
    Private Const ANW_BLAUESMODELL As String = "Blaues Modell"
    Private Const ANW_COMBIBM As String = "BM Combinatorics"
    Private Const ANW_TESTPROBLEME As String = "Test-Probleme"
    Private Const ANW_TSP As String = "TS-Problem"

    Private AppIniOK As Boolean = False

    '**** Deklarationen der Module *****
    Public BM_Form1 As New Apps.BM_Form
    Public SensiPlot1 As New Apps.SensiPlot
    Public Wave1 As New Apps.Wave
    Public CES1 As New EvoKern.CES

    Dim myIsOK As Boolean
    Dim myisrun As Boolean
    Dim globalAnzPar As Short
    Dim globalAnzZiel As Short
    Dim globalAnzRand As Short
    Dim array_x() As Double
    Dim array_y() As Double
    Dim Bestwert(,) As Double = {}
    Dim Population(,) As Double
    Dim mypara(,) As Double

    Private Sub Form1_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        'XP-look
        System.Windows.Forms.Application.EnableVisualStyles()

        'Liste der Anwendungen in ComboBox schreiben und Anfangseinstellung wählen
        ComboBox_Anwendung.Items.AddRange(New Object() {"", ANW_RESETPARA_RUNBM, ANW_SENSIPLOT_MODPARA, ANW_BLAUESMODELL, ANW_COMBIBM, ANW_TESTPROBLEME, ANW_TSP})
        ComboBox_Anwendung.SelectedIndex = 0


        'Ende der Initialisierung
        IsInitializing = False

    End Sub

    '************************************************************************************
    '*********** Die Anwendung wurde ausgewählt und wird jetzt initialisiert ************
    '************************************************************************************

    'Auswahl der zu optimierenden Anwendung geändert
    Private Sub IniApp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_IniApp.Click, ComboBox_Anwendung.SelectedIndexChanged, Testprobleme1.Testproblem_Changed
        If Me.IsInitializing = True Then
            'Testprobleme und Evo Deaktivieren
            Testprobleme1.Enabled = False
            EVO_Einstellungen1.Enabled = False
            Exit Sub
        Else
            AppIniOK = True
            Anwendung = ComboBox_Anwendung.SelectedItem

            Select Case Anwendung
                Case ""
                    'Testprobleme und Evo Deaktivieren
                    Testprobleme1.Enabled = False
                    EVO_Einstellungen1.Enabled = False

                Case ANW_RESETPARA_RUNBM
                    'Voreinstellungen lesen EVO.INI
                    Call ReadEVOIni()
                    'Testprobleme und Evo Deaktivieren
                    Testprobleme1.Enabled = False
                    EVO_Einstellungen1.Enabled = False
                    'Einlesen OptPara, ModellPara, Zielfunktionen
                    Call BM_Form1.OptParameter_einlesen()
                    Call BM_Form1.ModellParameter_einlesen()
                    Call BM_Form1.OptZiele_einlesen()

                    'Original ModellParameter werden geschrieben
                    Call BM_Form1.ModellParameter_schreiben()

                Case ANW_SENSIPLOT_MODPARA
                    'Voreinstellungen lesen EVO.INI
                    Call ReadEVOIni()
                    'Testprobleme und Evo Deaktivieren
                    Testprobleme1.Enabled = False
                    EVO_Einstellungen1.Enabled = False
                    'Einlesen OptPara, ModellPara, Zielfunktionen
                    Call BM_Form1.OptParameter_einlesen()
                    Call BM_Form1.ModellParameter_einlesen()
                    Call BM_Form1.OptZiele_einlesen()
                    ''Datenbank vorbereiten
                    'Call BM_Form1.db_prepare()
                    'Sensi Plot Dialog starten und List_Boxen füllen
                    Dim i As Integer
                    Dim IsOK As Boolean

                    For i = 0 To BM_Form1.OptParameterListe.GetUpperBound(0)
                        IsOK = SensiPlot1.ListBox_OptParameter_add(BM_Form1.OptParameterListe(i).Bezeichnung)
                    Next
                    For i = 0 To BM_Form1.OptZieleListe.GetUpperBound(0)
                        IsOK = SensiPlot1.ListBox_OptZiele_add(BM_Form1.OptZieleListe(i).Bezeichnung)
                    Next
                    Call SensiPlot1.ShowDialog()

                Case ANW_BLAUESMODELL
                    'Voreinstellungen lesen EVO.INI
                    Call ReadEVOIni()
                    'Evo aktivieren
                    EVO_Einstellungen1.Enabled = True
                    'Testprobleme ausschalten
                    Testprobleme1.Enabled = False
                    'BM_Form anzeigen
                    Dim BM_OK As DialogResult = BM_Form1.ShowDialog()
                    If (BM_OK = Windows.Forms.DialogResult.OK) Then
                        'Je nach Anzahl der Zielfunktionen von MO auf SO umschalten
                        If BM_Form1.OptZieleListe.GetLength(0) = 1 Then
                            EVO_Einstellungen1.OptModus = 0
                        ElseIf BM_Form1.OptZieleListe.GetLength(0) > 1 Then
                            EVO_Einstellungen1.OptModus = 1
                        End If
                        Call Initialisierung_BlauesModell()
                    End If

                Case ANW_COMBIBM
                    Dim isOK As Boolean
                    'Voreinstellungen lesen EVO.INI
                    Call ReadEVOIni()
                    'Testprobleme ausschalten
                    Testprobleme1.Enabled = False
                    ''Einlesen OptPara, ModellPara, Zielfunktionen
                    'Call BM_Form1.OptParameter_einlesen()
                    'Call BM_Form1.ModellParameter_einlesen()
                    'Call BM_Form1.OptZiele_einlesen()

                    'Einlesen der CombiOpt Datei
                    Call BM_Form1.Kombinatorik_einlesen()

                    'Überprüfen der Kombinatorik
                    'ToDo: Hier Message Box einbauen
                    isOK = BM_Form1.Kombinatorik_is_Valid

                    'Einlesen der Verbraucher Datei
                    Call BM_Form1.Verzweigung_Read()

                    ''BM_Form anzeigen
                    'Normalerweise werden hier die Daten eingelesen
                    'BM_Form1.ShowDialog()

                Case ANW_TESTPROBLEME
                    'Test-Probleme und Evo aktivieren
                    Testprobleme1.Enabled = True
                    EVO_Einstellungen1.Enabled = True
                    Call Testprobleme_Initialisierung()

                Case ANW_TSP
                    Call CES1.TSP_Initialize(TChart1)
            End Select
        End If
    End Sub

    

    '******************** Initialisierung der Anwendung *********************************
    '************************************************************************************

    '******************* Initialisierung der Testprobleme *******************************

    Private Sub Testprobleme_Initialisierung()

        'OptModus wird festgelegt um die richtigen EVO.Einstellungen anzuzeigen
        EVO_Einstellungen1.OptModus = Testprobleme1.OptModus

        'BUG: Bug 57: Für alle Testprobleme ReDim mypara(globalAnzPar - 1, 0) ! (wegen Array-Anfang bei 0)
        'Globale Parameter werden gesetzt
        Call Testprobleme1.Parameter_Uebergabe(Testprobleme1.Combo_Testproblem.Text, Testprobleme1.Text_Sinusfunktion_Par.Text, Testprobleme1.Text_Schwefel24_Par.Text, globalAnzPar, globalAnzZiel, globalAnzRand, mypara)

        Select Case Testprobleme1.Combo_Testproblem.Text
            Case "Sinus-Funktion"
                Call Testprobleme1.TeeChartIni_SinusFunktion(EVO_Einstellungen1, globalAnzPar, Testprobleme1.Text_Sinusfunktion_Par.Text, TChart1)
            Case "Beale-Problem" 'x1 = [-5;5], x2=[-2;2]
                Call Testprobleme1.TeeChartIni_BealeProblem(EVO_Einstellungen1, globalAnzPar, TChart1)
            Case "Schwefel 2.4-Problem" 'xi = [-10,10]
                Call Testprobleme1.TeeChartIni_SchwefelProblem(EVO_Einstellungen1, globalAnzPar, TChart1)
            Case Else
                Call Testprobleme1.TeeChartIni_MultiTestProb(EVO_Einstellungen1, Testprobleme1.Combo_Testproblem.Text, TChart1)
        End Select
    End Sub

    '*************************** EVO.ini Datei einlesen *********************************

    'TODO: ReadEVO.ini müsste hier raus nach BM_FORM und "Read_Model_OptConfig" heißen **
    Private Sub ReadEVOIni()
        If File.Exists("EVO.ini") Then
            Try
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
                            BM_Form1.BM_Exe = Configs(i, 1)
                            BM_Form1.TextBox_EXE.Text = BM_Form1.BM_Exe
                        Case "Datensatz"
                            'Dateiname vom Ende abtrennen
                            BM_Form1.Datensatz = Configs(i, 1).Substring(Configs(i, 1).LastIndexOf("\") + 1)
                            'Dateiendung entfernen
                            BM_Form1.Datensatz = BM_Form1.Datensatz.Substring(0, BM_Form1.Datensatz.Length - 4)
                            'Arbeitsverzeichnis bestimmen
                            BM_Form1.WorkDir = Configs(i, 1).Substring(0, Configs(i, 1).LastIndexOf("\") + 1)
                            BM_Form1.TextBox_Datensatz.Text = Configs(i, 1)
                        Case "OptParameter"
                            BM_Form1.OptParameter_Pfad = Configs(i, 1)
                            BM_Form1.TextBox_OptParameter_Pfad.Text = BM_Form1.OptParameter_Pfad
                        Case "ModellParameter"
                            BM_Form1.ModellParameter_Pfad = Configs(i, 1)
                            BM_Form1.TextBox_ModellParameter_Pfad.Text = BM_Form1.ModellParameter_Pfad
                        Case "OptZiele"
                            BM_Form1.OptZiele_Pfad = Configs(i, 1)
                            BM_Form1.TextBox_OptZiele_Pfad.Text = BM_Form1.OptZiele_Pfad
                        Case "CombiOpt"
                            BM_Form1.Combi_Pfad = Configs(i, 1)
                            BM_Form1.TextBox_Combi_Pfad.Text = BM_Form1.Combi_Pfad
                        Case Else
                            'nix
                    End Select
                Next

            Catch except As Exception
                MsgBox("Fehler beim lesen der EVO.ini Datei:" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
            End Try
        End If

    End Sub

    '************************* Initialisierung es BlauenModells *************************

    Private Sub Initialisierung_BlauesModell()
        Dim i As Integer
        Dim isMultiObjective As Boolean
        Dim n_Kalkulationen As Integer
        Dim n_Populationen As Integer

        isMultiObjective = EVO_Einstellungen1.isMultiObjective

        'Anzahl Optimierungsparameter übergeben
        '-----------------------------------------------------
        globalAnzPar = BM_Form1.OptParameterListe.GetLength(0)

        'Parameterwerte übergeben
        'BUG 57: mypara() fängt bei 1 an!
        ReDim mypara(globalAnzPar, 1)
        For i = 1 To globalAnzPar
            mypara(i, 1) = BM_Form1.OptParameterListe(i - 1).SKWert
        Next

        'globale Anzahl der Ziele muss hier auf Länge der Zielliste gesetzt werden
        globalAnzZiel = BM_Form1.OptZieleListe.GetLength(0)

        'TODO: Randbedingungen
        globalAnzRand = 2

        'Anzahl Kalkulationen
        'Ob das hier die richtige Stelle ist?
        If EVO_Einstellungen1.isPOPUL Then
            n_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf * EVO_Einstellungen1.NRunden
        Else
            n_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf
        End If

        'Anzahl Populationen
        'Ob das hier die richtige Stelle ist?
        n_Populationen = 1
        If EVO_Einstellungen1.isPOPUL Then
            n_Populationen = EVO_Einstellungen1.NPopul
        End If

        'Initialisierung der TeeChart Serien je nach SO oder MO
        If (isMultiObjective) = False Then
            Call BM_Form1.TeeChartInitialise_SO_BlauesModell(n_Populationen, n_Kalkulationen, TChart1)
        Else
            Call BM_Form1.TeeChartInitialise_MO_BlauesModell(TChart1)
        End If

    End Sub

    '************************************************************************************
    '************************* Start BUTTON wurde pressed *******************************
    '************************************************************************************

    Private Sub Button_Start_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Button_Start.Click
        If Not AppIniOK Then
            MsgBox("Bitte zuerst Anwendung Initialisieren", MsgBoxStyle.Exclamation, "Fehler")
            Exit Sub
        End If
        AppIniOK = False
        myisrun = True
        Select Case Anwendung
            Case ANW_RESETPARA_RUNBM
                Call BM_Form1.launchBM()
            Case ANW_SENSIPLOT_MODPARA
                myIsOK = SensiPlot_STARTEN(SensiPlot1.Selected_OptParameter, SensiPlot1.Selected_OptZiel, SensiPlot1.Selected_SensiType, SensiPlot1.Anz_Sim)
            Case ANW_BLAUESMODELL
                myIsOK = ES_STARTEN()
            Case ANW_TESTPROBLEME
                myIsOK = ES_STARTEN()
            Case ANW_TSP
                myIsOK = TSP_STARTEN()
        End Select
    End Sub

    'TODO: Das wird nie aufgerufen
    Private Sub Command2_Click()
        myisrun = False
    End Sub


    '           Anwendung SensiPlot - START; läuft ohne Evolutionsstrategie             
    '************************************************************************************

    Private Function SensiPlot_STARTEN(ByRef Selected_OptParameter As String, ByRef Selected_OptZiel As String, ByRef Selected_SensiType As String, ByRef Anz_Sim As Integer) As Boolean
        SensiPlot_STARTEN = False

        globalAnzZiel = 2
        globalAnzRand = 0

        'Anpassung der Arrays für "Call BM_Form1.ModellParameter_schreiben()"
        'TODO: Sehr kompliziert, ModellParameter_schreiben und weitere Funktionen sollten pro Prameter funzen.

        Dim i As Integer
        Dim j As Integer
        Dim AnzModPara As Integer = 0

        Dim OptParameterListeOrig() As Apps.BM_Form.OptParameter = {}
        Dim ModellParameterListeOrig() As Apps.BM_Form.ModellParameter = {}
        Dim OptZieleListeOrig() As Apps.BM_Form.OptZiel = {}

        OptParameterListeOrig = BM_Form1.OptParameterListe
        ModellParameterListeOrig = BM_Form1.ModellParameterListe
        OptZieleListeOrig = BM_Form1.OptZieleListe

        For i = 0 To ModellParameterListeOrig.GetUpperBound(0)
            If ModellParameterListeOrig(i).OptParameter = Selected_OptParameter Then
                AnzModPara += 1
            End If
        Next

        ReDim BM_Form1.OptParameterListe(0)
        ReDim BM_Form1.ModellParameterListe(AnzModPara - 1)
        ReDim BM_Form1.OptZieleListe(0)

        For i = 0 To OptParameterListeOrig.GetUpperBound(0)
            If OptParameterListeOrig(i).Bezeichnung = Selected_OptParameter Then
                BM_Form1.OptParameterListe(0) = OptParameterListeOrig(i)
            End If
        Next

        For j = 0 To AnzModPara - 1
            For i = 0 To ModellParameterListeOrig.GetUpperBound(0)
                If ModellParameterListeOrig(i).OptParameter = Selected_OptParameter Then
                    BM_Form1.ModellParameterListe(j) = ModellParameterListeOrig(i)
                    j += 1
                End If
            Next
        Next

        For i = 0 To OptZieleListeOrig.GetUpperBound(0)
            If OptZieleListeOrig(i).Bezeichnung = Selected_OptZiel Then
                BM_Form1.OptZieleListe(0) = OptZieleListeOrig(i)
            End If
        Next

        Dim globalAnzPar As Integer = 1
        Dim durchlauf As Integer = 1
        Dim ipop As Integer = 1

        'TODO: Das hier muss neuer TeeChartInitialise_Werden
        'TODO: TeeChart Initialise muss generalisiert werden
        Call SensiPlot1.TeeChart_Ini_SensiPlot(TChart1, EVO_Einstellungen1.NPopul, BM_Form1.OptZieleListe(0).Bezeichnung, BM_Form1.OptParameterListe(0).Bezeichnung)

        ReDim Wave1.WaveList(Anz_Sim + 1)
        'HACK: Die Spalte der WEL-Datei ist hartvercodet!
        BM_Form1.ReadWEL(BM_Form1.WorkDir & BM_Form1.Datensatz & ".wel", "S201_1ZU", Wave1.WaveList(0).Wave)

        Randomize()

        For i = 0 To Anz_Sim

            Select Case Selected_SensiType
                Case "Gleichverteilt"
                    BM_Form1.OptParameterListe(0).SKWert = Rnd()
                Case "Diskret"
                    BM_Form1.OptParameterListe(0).SKWert = i / Anz_Sim
            End Select

            Call BM_Form1.ModellParameter_schreiben()
            Call BM_Form1.launchBM()

            'Speichern der ersten und letzten Wave
            Wave1.WaveList(i).Bezeichnung = BM_Form1.OptZieleListe(0).SimGr
            'Wave1.WaveList(1).Bezeichnung = BM_Form1.OptZieleListe(0).SpalteWel
            'If i = 0 Then
            BM_Form1.ReadWEL(BM_Form1.WorkDir & BM_Form1.Datensatz & ".wel", BM_Form1.OptZieleListe(0).SimGr, Wave1.WaveList(i + 1).Wave)
            'ElseIf i = Anz_Sim Then
            'BM_Form1.ReadWEL(BM_Form1.WorkDir & BM_Form1.Datensatz & ".wel", BM_Form1.OptZieleListe(0).SpalteWel, Wave1.WaveList(1).Wave)
            'End If

            BM_Form1.OptZieleListe(0).QWertTmp = BM_Form1.QualitaetsWert_berechnen(0)
            TChart1.Series(0).Add(BM_Form1.OptZieleListe(0).QWertTmp, BM_Form1.OptParameterListe(0).Wert, "")
            'Call BM_Form1.db_update(durchlauf, ipop)
            durchlauf += 1
            System.Windows.Forms.Application.DoEvents()

        Next

        BM_Form1.OptParameterListe = OptParameterListeOrig
        BM_Form1.ModellParameterListe = ModellParameterListeOrig
        BM_Form1.OptZieleListe = OptZieleListeOrig

        Call Wave1.TeeChart_initialise()
        Call Wave1.TeeChart_draw()
        Call Wave1.ShowDialog()
        System.Windows.Forms.Application.DoEvents()

        SensiPlot_STARTEN = True
    End Function


    '                      Anwendung Traveling Salesman - Start                         
    '************************************************************************************

    Private Function TSP_STARTEN() As Boolean
        Dim gen As Integer              'Laufvariable für die Generationen

        'ToDo: nochmal Prüfen wie das mit den Kids REDIMS ist.
        Call CES1.TeeChart_Initialise_TSP(TChart1)

        'Arrays werden Dimensioniert
        Call CES1.Dim_Parents()
        Call CES1.Dim_Childs()

        'Zufällige Kinderpfade werden generiert
        Call CES1.Generate_Random_Path()

        'Generationsschleife
        For gen = 1 To CES1.AnzGen

            'Den Kindern werden die Städte Ihres Pfades entsprechend zugewiesen
            Call CES1.Cities_according_ChildPath()

            'Bestimmung des der Qualität der Kinder
            Call CES1.Evaluate_child_Quality()

            'Sortieren der Kinden anhand der Qualität
            Call CES1.Sort_Faksimile(CES1.ChildList_TSP)

            'Selections Prozess (Übergabe der Kinder an die Eltern je nach Strategie)
            Call CES1.Selection_Process()

            'Zeichnen des besten Elter
            'TODO: funzt nur, wenn ganz am ende gezeichnet wird
            If gen = CES1.AnzGen Then
                Call CES1.TeeChart_Zeichnen_TSP(TChart1, CES1.ParentList_TSP(0).CityList)
            End If

            'Kinder werden Hier vollständig gelöscht
            Call CES1.Reset_Childs()

            'Reproductionsoperatoren, hier gehts dezent zur Sache
            Call CES1.Reproduction_Operations()

            'Mutationsoperatoren
            Call CES1.Mutation_Operations()

        Next gen

    End Function

    '           Anwendung CombiBM - START; läuft ohne Evolutionsstrategie             
    '************************************************************************************

    Private Function Combi_BM_STARTEN() As Boolean
        Dim gen As Integer               'Laufvariable für die Generationen

        'TeeChart initialisieren
        Call BM_Form1.TeeChartInitialise_SO_BlauesModell(1, gen, TChart1)

        'Arrays werden Dimensioniert
        Call CES1.Dim_Parents()
        Call CES1.Dim_Childs()


    End Function


    '     Anwendung Evolutionsstrategie für Parameter Optimierung - hier Steuerung       
    '************************************************************************************

    Private Function ES_STARTEN() As Boolean
        '==========================
        Dim isOK As Boolean
        Dim i As Integer
        Dim Txt As String
        '--------------------------
        Dim durchlauf As Integer
        '--------------------------
        Dim evolutionsstrategie As EvoKern.CEvolutionsstrategie
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

        'TODO: On Error GoTo Err_ES_STARTEN

        ES_STARTEN = False

        'TODO: If (ipop + igen + inachf + irunde) > 4 Then GoTo Start_Evolutionsrunden

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

        ReDim QN(globalAnzZiel)
        ReDim RN(globalAnzRand)

        'Kontrolle der Variablen
        If NRunden = 0 Or NPopul = 0 Or NPopEltern = 0 Then
            Txt = "Anzahl der Runden, Populationen oder Populationseltern ist zu klein!"
            GoTo ErrCode_ES_STARTEN
        End If
        If NGen = 0 Or NEltern = 0 Or NNachf = 0 Then
            Txt = "Anzahl der Generationen, Eltern oder Nachfolger ist zu klein!"
            GoTo ErrCode_ES_STARTEN
        End If
        If rDeltaStart < 0 Then
            Txt = "Die Startschrittweite ist unzulässig oder kleiner als die minimale Schrittweite!"
            GoTo ErrCode_ES_STARTEN
        End If
        If globalAnzPar = 0 Then
            Txt = "Die Anzahl der Parameter ist unzulässig!"
            GoTo ErrCode_ES_STARTEN
        End If
        If NPopul < NPopEltern Then
            Txt = "Die Anzahl der Populationseltern darf nicht größer als die Anzahl der Populationen sein!"
            GoTo ErrCode_ES_STARTEN
        End If

        '1. Schritt: CEvolutionsstrategie
        'Objekt der Klasse CEvolutionsstrategie wird erzeugen
        '******************************************************************************************
        evolutionsstrategie = New EvoKern.CEvolutionsstrategie

        '2. Schritt: CEvolutionsstrategie - ES_INI
        'Die öffentlichen dynamischen Arrays werden initialisiert (Dn, An, Xn, Xmin, Xmax)
        'und die Anzahl der Zielfunktionen wird festgelegt
        '******************************************************************************************
        isOK = evolutionsstrategie.EsIni(globalAnzPar, globalAnzZiel, globalAnzRand)

        '3. Schritt: CEvolutionsstrategie - ES_OPTIONS
        'Optionen der Evolutionsstrategie werden übergeben
        '******************************************************************************************
        isOK = evolutionsstrategie.EsOptions(iEvoTyp, iPopEvoTyp, isPOPUL, NRunden, NPopul, NPopEltern, iOptPopEltern, iOptEltern, iPopPenalty, NGen, NEltern, NNachf, NRekombXY, rDeltaStart, iStartPar, isdnvektor, isMultiObjective, isPareto, isPareto3D, Interact, isInteract, NMemberSecondPop)

        '4. Schritt: CEvolutionsstrategie - ES_LET_PARAMETER
        'Ausgangsparameter werden übergeben
        '******************************************************************************************
        For i = 1 To globalAnzPar
            myIsOK = evolutionsstrategie.EsLetParameter(i, mypara(i, 1))
        Next i

        '5. Schritt: CEvolutionsstrategie - ES_PREPARE
        'Interne Variablen werden initialisiert, Zufallsgenerator wird initialisiert
        '******************************************************************************************
        myIsOK = evolutionsstrategie.EsPrepare()

        '6. Schritt: CEvolutionsstrategie - ES_STARTVALUES
        'Startwerte werden zugewiesen
        '******************************************************************************************
        myIsOK = evolutionsstrategie.EsStartvalues()

        'Startwerte werden der Bedienoberfläche zugewiesen
        '******************************************************************************************
        EVO_Opt_Verlauf1.NRunden = evolutionsstrategie.NRunden
        EVO_Opt_Verlauf1.NPopul = evolutionsstrategie.NPopul
        EVO_Opt_Verlauf1.NGen = evolutionsstrategie.NGen
        EVO_Opt_Verlauf1.NNachf = evolutionsstrategie.NNachf
        EVO_Opt_Verlauf1.Initialisieren()

        durchlauf = 0

Start_Evolutionsrunden:
        'Cursor setzen
        'System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        'Loop über alle Runden
        '*******************************************************************************************
        Do While (evolutionsstrategie.EsIsNextRunde)

            irunde = evolutionsstrategie.iaktuelleRunde
            Call EVO_Opt_Verlauf1.Runden(irunde)

            myIsOK = evolutionsstrategie.EsPopBestwertspeicher()
            'Loop über alle Populationen
            '***********************************************************************************************
            Do While (evolutionsstrategie.EsIsNextPop)

                ipop = evolutionsstrategie.iaktuellePopulation
                Call EVO_Opt_Verlauf1.Populationen(ipop)

                myIsOK = evolutionsstrategie.EsPopVaria

                myIsOK = evolutionsstrategie.EsPopMutation

                durchlauf = NGen * NNachf * (irunde - 1)

                'Loop über alle Generationen
                '***********************************************************************************************
                Do While (evolutionsstrategie.EsIsNextGen)

                    igen = evolutionsstrategie.iaktuelleGeneration
                    Call EVO_Opt_Verlauf1.Generation(igen)

                    myIsOK = evolutionsstrategie.EsBestwertspeicher()

                    'Loop über alle Nachkommen
                    '********************************************************************
                    Do While (evolutionsstrategie.EsIsNextNachf)

                        inachf = evolutionsstrategie.iaktuellerNachfahre
                        Call EVO_Opt_Verlauf1.Nachfolger(inachf)

                        durchlauf = durchlauf + 1

                        'Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern
                        myIsOK = evolutionsstrategie.EsVaria

                        'Mutieren der Ausgangswerte
                        myIsOK = evolutionsstrategie.EsMutation

                        'Auslesen der Variierten Parameter
                        myIsOK = evolutionsstrategie.EsGetParameter(globalAnzPar, mypara)

                        'Auslesen des Bestwertspeichers
                        If Not evolutionsstrategie.isMultiObjective Then
                            myIsOK = evolutionsstrategie.EsGetBestwert(Bestwert)
                        End If

                        '************************************************************************************
                        '******************* Ansteuerung der zu optimierenden Anwendung *********************
                        '************************************************************************************
                        Select Case Anwendung
                            Case ANW_TESTPROBLEME
                                myIsOK = Testprobleme1.Evaluierung_TestProbleme(Testprobleme1.Combo_Testproblem.Text, globalAnzPar, mypara, durchlauf, ipop, QN, RN, TChart1)
                            Case ANW_BLAUESMODELL
                                myIsOK = BM_Form1.Evaluierung_BlauesModell(globalAnzPar, globalAnzZiel, mypara, durchlauf, ipop, QN, TChart1)
                        End Select

                        'Einordnen der Qualitätsfunktion im Bestwertspeicher
                        '**************************************************************************
                        myIsOK = evolutionsstrategie.EsBest(QN, RN)

                        System.Windows.Forms.Application.DoEvents()

                        'Ende Loop über alle Nachkommen
                        '**************************************************************************
                    Loop


                    'Die neuen Eltern werden generiert
                    myIsOK = evolutionsstrategie.EsEltern()

                    'Bestwerte und sekundäre Population
                    If evolutionsstrategie.isMultiObjective Then
                        myIsOK = evolutionsstrategie.EsGetBestwert(Bestwert)
                        'TODO: Call Bestwertzeichnen_Pareto(Bestwert, ipop)
                        myIsOK = evolutionsstrategie.esGetSekundärePopulation(Population)
                        Call SekundärePopulationZeichnen(Population)
                    End If

                    System.Windows.Forms.Application.DoEvents()

                    'Ende Loop über alle Generationen
                    '***********************************************************************************************
                Loop 'Schleife über alle Generationen

                System.Windows.Forms.Application.DoEvents()

                'Einordnen der Qualitätsfunktion im PopulationsBestwertspeicher
                myIsOK = evolutionsstrategie.EsPopBest()

                'Ende Loop über alle Populationen
                '***********************************************************************************************
            Loop 'Schleife über alle Populationen



            'Die neuen Populationseltern werden generiert
            myIsOK = evolutionsstrategie.EsPopEltern

            System.Windows.Forms.Application.DoEvents()

            'Ende Loop über alle Runden
            '***********************************************************************************************
        Loop 'Schleife über alle Runden

        'CEvolutionsstrategie, letzter. Schritt
        'Objekt der Klasse CEvolutionsstrategie wird vernichtet
        '***************************************************************************************************
        'UPGRADE_NOTE: Das Objekt evolutionsstrategie kann erst dann gelöscht werden, wenn die Garbagecollection durchgeführt wurde. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1029"'
        'TODO: Ersetzen durch dispose funzt net
        evolutionsstrategie = Nothing
        ES_STARTEN = True

EXIT_ES_STARTEN:
        'Cursor setzen
        'System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Exit Function
        'xxxxxxxxxxxxxxxxxxxxxxxxxx
Err_ES_STARTEN:
        Beep()
        MsgBox("ES_STARTEN: " & ErrorToString(), MsgBoxStyle.Critical)
        GoTo EXIT_ES_STARTEN
ErrCode_ES_STARTEN:
        Beep()
        MsgBox("ES_STARTEN: " & Txt, MsgBoxStyle.Information)
        GoTo EXIT_ES_STARTEN
    End Function


    '************************************************************************************
    '                          Zeichenfunktionen                                        *
    '************************************************************************************

    Private Sub Bestwertzeichnen_Pareto(ByRef Bestwert(,) As Double, ByRef ipop As Short)
        Dim i As Short
        With TChart1
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
        With TChart1
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

    'Überprüfung der Eingabe von "Anzahl Parameter" bei Sinus-Funktion
    Private Sub Par_Sinus_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs)
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        'UPGRADE_ISSUE: Zuweisung wird nicht unterstützt: KeyAscii an Nicht-Null-Wert Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1058"'
        KeyAscii = KEYOK(KeyAscii, AllowIntegerOnly)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    '******************************** TChart Funktionen *******************************************
    '**********************************************************************************************

    'Chart bearbeiten
    Private Sub TChartEdit(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChartEdit.Click
        TChart1.ShowEditor()
    End Sub

    'Chart nach Excel exportieren
    Private Sub TChart2Excel(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChart2Excel.Click
        SaveFileDialog1.DefaultExt = TChart1.Export.Data.Excel.FileExtension
        SaveFileDialog1.FileName = TChart1.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "Excel-Dateien (*.xls)|*.xls"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            TChart1.Export.Data.Excel.Series = Nothing 'export all series
            TChart1.Export.Data.Excel.IncludeLabels = True
            TChart1.Export.Data.Excel.IncludeIndex = True
            TChart1.Export.Data.Excel.IncludeHeader = True
            TChart1.Export.Data.Excel.IncludeSeriesTitle = True
            TChart1.Export.Data.Excel.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

    'Chart als PNG exportieren
    Private Sub TChart2PNG(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChart2PNG.Click
        SaveFileDialog1.DefaultExt = TChart1.Export.Image.PNG.FileExtension
        SaveFileDialog1.FileName = TChart1.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "PNG-Dateien (*.png)|*.png"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            TChart1.Export.Image.PNG.GrayScale = False
            TChart1.Export.Image.PNG.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

    'Chart in nativem TeeChart-Format abspeichern
    Private Sub TChartSave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChartSave.Click
        SaveFileDialog1.DefaultExt = TChart1.Export.Template.FileExtension
        SaveFileDialog1.FileName = TChart1.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "TeeChart-Dateien (*.ten)|*.ten"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            TChart1.Export.Template.IncludeData = True
            TChart1.Export.Template.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub
End Class
