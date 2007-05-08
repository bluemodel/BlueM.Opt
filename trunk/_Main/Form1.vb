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

            Try
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
                        Exit Try


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
                            Exit Try
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

                        CES1.n_Ziele = BlueM1.OptZieleListe.GetLength(0)

                        'Einlesen der CombiOpt Datei
                        Call BlueM1.Read_CES()

                        'Überprüfen der Kombinatorik
                        'ToDo: Hier Message Box einbauen
                        Call BlueM1.Combinatoric_is_Valid()

                        'Einlesen der Verbraucher Datei
                        Call BlueM1.Verzweigung_Read()

                        'Prüfen ob Kombinatorik und Verzweigungsdatei zusammenpassen
                        'ToDo: Hier Message Box einbauen
                        Call BlueM1.CES_fits_to_VER()

                        'Call Initialisierung_BlauesModell_CombiOpt()

                        'CES MOdus setzen -> "TSP" oder "BM" Optimierung
                        CES1.CES_Modus = "BM"


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

                        Call CES1.TSP_Initialize(Diag)
                        'CES MOdus setzen -> "TSP" oder "BM" Optimierung
                        CES1.CES_Modus = "TSP"

                End Select

                'IniApp OK -> Start Button aktivieren
                Me.Button_Start.Enabled = True

            Catch except As Exception
                'Fehlerbehandlung
                MsgBox("Initialisierung der Anwendung fehlgeschlagen!" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Critical, "Fehler")

            End Try

            'Mauszeiger wieder normal
            Cursor = System.Windows.Forms.Cursors.Default

        End If

    End Sub

    'EVO.ini Datei einlesen 
    '**********************
    'Bug 94: ReadEVO.ini müsste hier raus nach BlueM und "Read_Model_OptConfig" heißen **
    Private Sub ReadEVOIni()

        Dim IsOK As Boolean = True

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

            Catch except As Exception
                MsgBox("Fehler beim lesen der EVO.ini Datei:" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
                IsOK = False
            End Try

        Else
            'Datei EVO.ini existiert nicht
            MsgBox("Die Datei ""EVO.ini"" konnte nicht gefunden werden!" & Chr(13) & Chr(10) & "Bitte gemäß Dokumentation eine Datei ""EVO.ini"" erstellen.", MsgBoxStyle.Critical, "Fehler")
            IsOK = False
        End If

        'Ausnahme werfen
        '---------------
        If (IsOK = False) Then
            Throw New Exception("Fehler beim Einlesen der Datei ""EVO.ini""!")
        End If

    End Sub

#End Region

#Region "Start Button Pressed"

    'Start BUTTON wurde pressed
    'XXXXXXXXXXXXXXXXXXXXXXXXXX

    Private Sub Button_Start_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Button_Start.Click

        Try
            myisrun = True
            Select Case Anwendung
                Case ANW_BM_RESET
                    Call BlueM1.launchSim()
                Case ANW_BM_SENSIPLOT
                    Call SensiPlot_STARTEN()
                Case ANW_BM_PES
                    Call ES_STARTEN()
                Case ANW_BM_CES
                    Call BM_CES_STARTEN()
                Case ANW_TESTPROBLEME
                    Call ES_STARTEN()
                Case ANW_TSP
                    Call TSP_STARTEN()
            End Select

            'Fehlerbehandlung:
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Fehler")
        End Try
    End Sub

    'TODO: Das wird nie aufgerufen
    Private Sub Command2_Click()
        myisrun = False
    End Sub


    'Anwendung SensiPlot - START; läuft ohne Evolutionsstrategie             
    '***********************************************************
    Private Sub SensiPlot_STARTEN()

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
            Diag.Series(0).Add(BlueM1.OptZieleListe(SensiPlot1.Selected_OptZiel).QWertTmp, BlueM1.OptParameterListe(SensiPlot1.Selected_OptParameter).Wert, "")

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

        Call Wave1.Diag.DiagInitialise("SensiPlot", Achsen)

        'Serien initialisieren
        Dim tmpSeries As Steema.TeeChart.Styles.Line
        For i = 1 To SensiPlot1.Anz_Sim
            tmpSeries = New Steema.TeeChart.Styles.Line(Wave1.Diag.Chart)
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
    Private Sub TSP_STARTEN()

        'Laufvariable für die Generationen
        Dim gen As Integer

        'ToDo: nochmal Prüfen wie das mit den Kids REDIMS ist.
        'BUG 85: Nach Klasse Diagramm auslagern!
        Call CES1.TeeChart_Initialise_TSP(Diag)

        'Arrays werden Dimensioniert
        Call CES1.Dim_Parents_TSP()
        Call CES1.Dim_Childs_TSP()

        'Zufällige Kinderpfade werden generiert
        Call CES1.Generate_Random_Path_TSP()

        'Generationsschleife
        For gen = 1 To CES1.n_Gen

            'Den Kindern werden die Städte Ihres Pfades entsprechend zugewiesen
            Call CES1.Cities_according_ChildPath_TSP()

            'Bestimmung des der Qualität der Kinder
            Call CES1.Evaluate_child_Quality_TSP()

            'Sortieren der Kinden anhand der Qualität
            Call CES1.Sort_Faksimile_TSP(CES1.ChildList_TSP)

            'Selections Prozess (Übergabe der Kinder an die Eltern je nach Strategie)
            Call CES1.Selection_Process_TSP()

            'Zeichnen des besten Elter
            'TODO: funzt nur, wenn ganz am ende gezeichnet wird
            If gen = CES1.n_Gen Then
                Call CES1.TeeChart_Zeichnen_TSP(Diag, CES1.ParentList_TSP(0).Image)
            End If

            'Kinder werden Hier vollständig gelöscht
            Call CES1.Reset_Childs_TSP()

            'Reproduktionsoperatoren, hier gehts dezent zur Sache
            Call CES1.Reproduction_Operations_TSP()

            'Mutationsoperatoren
            Call CES1.Mutation_Operations_TSP()

        Next gen

    End Sub

    'Anwendung CombiBM - START; läuft ohne Evolutionsstrategie             
    '*********************************************************
    Private Sub BM_CES_STARTEN()

        'Fehlerabfragen
        If (BlueM1.OptZieleListe.GetLength(0) > 2) Then
            Throw New Exception("Zu viele Ziele für CES. Max=2")
        End If

        Dim durchlauf As Integer = 0

        'BlueM wird an CES übergeben um Zugriff auf alle Objekte zu haben
        CES1.BlueM1 = BlueM1

        'Laufvariable für die Generationen
        Dim gen As Integer
        Dim i As Integer

        'Arrays werden Dimensioniert
        Call CES1.Dim_Parents_BM()
        Call CES1.Dim_Childs_BM()

        'Diagramm vorbereiten und initialisieren        
        Call PrepareDiagramm()

        'Zufällige Kinderpfade werden generiert
        Call CES1.Generate_Random_Path_BM()

        'HACK: Funktion zum manuellen Testen der Paths in der ersten Generation
        'Call CES1.Generate_Test_Path_BM()

        'HACK: zum testen aller Kombinationen
        'Call CES1.Generate_All_Test_Path_BM()

        'Generationsschleife
        For gen = 1 To CES1.n_Gen

            'Ermittelt Verzweigung ON_OFF
            Call CES1.Verzweigung_ON_OFF()

            'Child Schleife
            For i = 0 To CES1.ChildList_BM.GetUpperBound(0)
                durchlauf += 1

                'ToDo: Dieser Teil steht im Moment im BM Form muss aber ins CES!
                'Erstellt die aktuelle Bauerksliste und überträgt sie zu SKos
                Call BlueM1.Define_aktuelle_Bauwerke(CES1.ChildList_BM(i).Path)
                'Schreibt die neuen Verzweigungen
                Call BlueM1.Verzweigung_Write(CES1.ChildList_BM(i).VER_ONOFF)
                'Evaluiert das Blaue Modell
                Call BlueM1.Eval_Sim_CombiOpt(CES1.n_Ziele, durchlauf, 1, CES1.ChildList_BM(i).Quality_MO, Diag)

                ''HACK zur Reduzierung auf eine Zielfunktion
                'Call CES1.MO_TO_SO(CES1.ChildList_BM(i))

                'Zeichnen der Kinder
                If BlueM1.OptZieleListe.GetLength(0) = 1 Then
                    Call Diag.Series(0).Add(durchlauf, CES1.ChildList_BM(i).Quality_SO)
                Else
                    Call Diag.Series(0).Add(CES1.ChildList_BM(i).Quality_MO(0), CES1.ChildList_BM(i).Quality_MO(1))
                End If

                ''HACK zum zeichnen aller Qualitäten
                'Call TChart1.Series(2).Add(durchlauf, CES1.ChildList_BM(i).Quality_MO(0))
                'Call TChart1.Series(3).Add(durchlauf, CES1.ChildList_BM(i).Quality_MO(1))
                'Call TChart1.Series(4).Add(durchlauf, CES1.ChildList_BM(i).Quality_MO(2))
                System.Windows.Forms.Application.DoEvents()
            Next

            If BlueM1.OptZieleListe.GetLength(0) = 1 Then
                'Sortieren der Kinden anhand der Qualität
                Call CES1.Sort_Faksimile_BM(CES1.ChildList_BM)
            ElseIf BlueM1.OptZieleListe.GetLength(0) = 2 Then
                'ndSorting
                
            End If

            'Selectionsprozess je nach "plus" oder "minus" Strategie
            'ToDO: Soll auch funzen by ndSorting
            Call CES1.Selection_Process_BM()

            ''Zeichnen des besten Elter
            'For i = 0 To CES1.ParentList_BM.GetUpperBound(0)
            '    'durchlauf += 1
            '    Call TChart1.Series(1).Add(durchlauf, CES1.ParentList_BM(i).Quality_SO)
            'Next

            'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
            Call CES1.Reset_Childs_BM()

            'Reproduktionsoperatoren, hier gehts dezent zur Sache
            Call CES1.Reproduction_Operations_BM()

            'Mutationsoperatoren
            Call CES1.Mutation_Operations_BM()

        Next

    End Sub


    'Anwendung Evolutionsstrategie für Parameter Optimierung - hier Steuerung       
    '************************************************************************

    Private Sub ES_STARTEN()
        '==========================
        Dim i As Integer
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
        evolutionsstrategie = New EvoKern.CEvolutionsstrategie

        '2. Schritt: CEvolutionsstrategie - ES_INI
        'Die öffentlichen dynamischen Arrays werden initialisiert (Dn, An, Xn, Xmin, Xmax)
        'und die Anzahl der Zielfunktionen wird festgelegt
        '******************************************************************************************
        myIsOK = evolutionsstrategie.EsIni(globalAnzPar, globalAnzZiel_ParaOpt, globalAnzRand)

        '3. Schritt: CEvolutionsstrategie - ES_OPTIONS
        'Optionen der Evolutionsstrategie werden übergeben
        '******************************************************************************************
        myIsOK = evolutionsstrategie.EsOptions(iEvoTyp, iPopEvoTyp, isPOPUL, NRunden, NPopul, NPopEltern, iOptPopEltern, iOptEltern, iPopPenalty, NGen, NEltern, NNachf, NRekombXY, rDeltaStart, iStartPar, isdnvektor, isMultiObjective, isPareto, isPareto3D, Interact, isInteract, NMemberSecondPop)

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

                'ToDo: Scheint mir Schwachsinnig an dieser Stelle Weil es überschrieben wird
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
                                Call Testprobleme1.Evaluierung_TestProbleme(Testprobleme1.Combo_Testproblem.Text, globalAnzPar, mypara, durchlauf, ipop, QN, RN, Diag)
                            Case ANW_BM_PES
                                Call BlueM1.Eval_Sim_ParaOpt(globalAnzPar, globalAnzZiel_ParaOpt, mypara, durchlauf, ipop, QN, Diag)
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

    End Sub


    'Zeichenfunktionen
    'XXXXXXXXXXXXXXXXX

    Private Sub Bestwertzeichnen_Pareto(ByRef Bestwert(,) As Double, ByRef ipop As Short)
        Dim i As Short
        With Diag
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
        With Diag
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

    'Diagramm vorbereiten und Initialisierung aufrufen
    '*************************************************
    Private Sub PrepareDiagramm()

        Dim i As Integer

        Select Case Anwendung

            'Testprobleme:
            '-------------
            Case ANW_TESTPROBLEME

                Select Case Testprobleme1.Combo_Testproblem.Text
                    Case "Sinus-Funktion"
                        Call Diag.DiagInitialise_SinusFunktion(EVO_Einstellungen1, globalAnzPar, Testprobleme1.Text_Sinusfunktion_Par.Text)
                    Case "Beale-Problem" 'x1 = [-5;5], x2=[-2;2]
                        Call Diag.DiagInitialise_BealeProblem(EVO_Einstellungen1, globalAnzPar)
                    Case "Schwefel 2.4-Problem" 'xi = [-10,10]
                        Call Diag.DiagInitialise_SchwefelProblem(EVO_Einstellungen1, globalAnzPar)
                    Case Else
                        Call Diag.DiagInitialise_MultiTestProb(EVO_Einstellungen1, Testprobleme1.Combo_Testproblem.Text)
                End Select

                'SensiPlot:
                '----------
            Case ANW_BM_SENSIPLOT

                'Achsen:
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
                Call Diag.DiagInitialise(Anwendung, Achsen)

                'Series initialisieren
                Dim tmpPoint As New Steema.TeeChart.Styles.Points(Me.Diag.Chart)
                tmpPoint.Title = "Simulationsergebnis"
                tmpPoint.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                tmpPoint.Color = System.Drawing.Color.Orange
                tmpPoint.Pointer.HorizSize = 2
                tmpPoint.Pointer.VertSize = 2

                'Alle anderen Anwendungen:
                '-------------------------
            Case Else

                Dim n_Kalkulationen As Integer
                Dim n_Populationen As Integer

                'Anzahl Kalkulationen
                '--------------------
                If EVO_Einstellungen1.isPOPUL Then
                    n_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf * EVO_Einstellungen1.NRunden
                Else
                    n_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf
                End If

                'Anzahl Populationen
                '-------------------
                n_Populationen = 1
                If EVO_Einstellungen1.isPOPUL Then
                    n_Populationen = EVO_Einstellungen1.NPopul
                End If

                'Initialisierung von TeeChart
                '----------------------------

                'Achsen:
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
                Call Diag.DiagInitialise(Anwendung, Achsen)

                'Series initialisieren
                If (EVO_Einstellungen1.isMultiObjective = False) Then
                    Call Diag.prepareSeries_SO(n_Populationen)
                Else
                    Call Diag.prepareSeries_MO()
                End If

        End Select

    End Sub

    'Chart bearbeiten
    '****************
    Private Sub TChartEdit(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChartEdit.Click
        Diag.ShowEditor()
    End Sub

    'Chart nach Excel exportieren
    '****************************
    Private Sub TChart2Excel(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChart2Excel.Click
        SaveFileDialog1.DefaultExt = Diag.Export.Data.Excel.FileExtension
        SaveFileDialog1.FileName = Diag.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "Excel-Dateien (*.xls)|*.xls"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            Diag.Export.Data.Excel.Series = Nothing 'export all series
            Diag.Export.Data.Excel.IncludeLabels = True
            Diag.Export.Data.Excel.IncludeIndex = True
            Diag.Export.Data.Excel.IncludeHeader = True
            Diag.Export.Data.Excel.IncludeSeriesTitle = True
            Diag.Export.Data.Excel.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

    'Chart als PNG exportieren
    '*************************
    Private Sub TChart2PNG(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChart2PNG.Click
        SaveFileDialog1.DefaultExt = Diag.Export.Image.PNG.FileExtension
        SaveFileDialog1.FileName = Diag.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "PNG-Dateien (*.png)|*.png"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            Diag.Export.Image.PNG.GrayScale = False
            Diag.Export.Image.PNG.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

    'Chart in nativem TeeChart-Format abspeichern
    '********************************************
    Private Sub TChartSave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChartSave.Click
        SaveFileDialog1.DefaultExt = Diag.Export.Template.FileExtension
        SaveFileDialog1.FileName = Diag.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "TeeChart-Dateien (*.ten)|*.ten"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            Diag.Export.Template.IncludeData = True
            Diag.Export.Template.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

#End Region 'Diagrammfunktionen

#End Region 'Methoden

End Class
