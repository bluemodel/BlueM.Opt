Option Strict Off ' Off ist Default
Option Explicit On
Friend Class Form1

    Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean
    'option Base 1O

    Dim myIsOK As Boolean
    Dim myisrun As Boolean
    Dim globalAnzPar As Short
    Dim globalAnzZiel As Short
    Dim globalAnzRand As Short
    Dim OptErg() As Double
    Dim array_x() As Double
    Dim array_y() As Double
    Dim Bestwert(,) As Double
    Dim Population(,) As Double
    Public mypara(,) As Double
    'Variablen f�r das Blaue Modell
    Public Datensatz As String
    Public Pfad As String

    Private Sub Combo1_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Combo1.SelectedIndexChanged
        If Form1.DefInstance.IsInitializing = True Then
            Exit Sub
        Else
            Select Case Combo1.Text
                Case "Sinus-Funktion"
                    Frame_Problem(0).BringToFront()
                Case "Beale-Problem"
                    Frame_Problem(1).BringToFront()
                Case "Schwefel 2.4-Problem"
                    Frame_Problem(2).BringToFront()
                Case "DEB 1"
                    Frame_Problem(3).BringToFront()
                Case "Zitzler/Deb T1"
                    Frame_Problem(4).BringToFront()
                Case "Zitzler/Deb T2"
                    Frame_Problem(5).BringToFront()
                Case "Zitzler/Deb T3"
                    Frame_Problem(6).BringToFront()
                Case "Zitzler/Deb T4"
                    Frame_Problem(7).BringToFront()
                Case "CONSTR"
                    Frame_Problem(8).BringToFront()
                Case "Box"
                    Frame_Problem(9).BringToFront()
                Case "BlauesModell"
                    Frame_Problem(10).BringToFront()
                    Me.Width = 1000
                    Me.GroupBox1.Show()
            End Select
        End If
    End Sub

    Private Sub Command1_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command1.Click
        myisrun = True
        myIsOK = ES_STARTEN()
    End Sub

    Private Sub Command2_Click()
        myisrun = False
    End Sub



    Private Sub EVO_Einstellungen1_ModusChanges(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles EVO_Einstellungen1.ModusChanges
        Dim OptimierungsModus As Integer
        OptimierungsModus = EVO_Einstellungen1.OptModus
        Select Case OptimierungsModus
            Case 1
                Combo1.Items.Clear()
                Combo1.Items.Add("Sinus-Funktion")
                Combo1.Items.Add("Beale-Problem")
                Combo1.Items.Add("Schwefel 2.4-Problem")
                Combo1.Items.Add("BlauesModell")
                Combo1.SelectedIndex = 0
            Case 2
                Combo1.Items.Clear()
                Combo1.Items.Add("Deb 1")
                Combo1.Items.Add("Zitzler/Deb T1")
                Combo1.Items.Add("Zitzler/Deb T2")
                Combo1.Items.Add("Zitzler/Deb T3")
                Combo1.Items.Add("Zitzler/Deb T4")
                Combo1.Items.Add("CONSTR")
                Combo1.Items.Add("Box")
                Combo1.Items.Add("BlauesModell")
                Combo1.SelectedIndex = 0
        End Select
    End Sub

    Private Sub Form1_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Me.Width = 707
        Combo1.Items.Add("Sinus-Funktion")
        Combo1.Items.Add("Beale-Problem")
        Combo1.Items.Add("Schwefel 2.4-Problem")
        Combo1.Items.Add("BlauesModell")
        Combo1.SelectedIndex = 0
        TeeCommander1.Chart = TChart1
    End Sub

    Private Function ES_STARTEN() As Boolean
        '==========================
        Dim isOK As Boolean
        Dim i As Integer
        Dim j As Short
        Dim Txt As String
        '--------------------------
        Dim durchlauf As Integer
        '--------------------------
        Dim evolutionsstrategie As dmevodll.CEvolutionsstrategie
        '--------------------------
        'Variablen f�r Optionen Evostrategie
        Dim iEvoTyp, iPopEvoTyp As Integer
        Dim isPOPUL, isMultiObjective As Boolean
        Dim NPopEltern, NRunden, NPopul, iOptPopEltern As Integer
        Dim iOptEltern, iPopPenalty As Integer
        Dim NEltern As Integer
        Dim NRekomb, NRekombXY As Integer
        Dim rDeltaStart As Single
        Dim iStartPar As Integer
        Dim isMUTREKOMB As Boolean
        Dim isdnvektor, isINIVARIA, isPareto As Boolean
        Dim isPareto3D As Boolean
        Dim NGen, NNachf As Integer
        Dim Interact As Short
        Dim isInteract As Boolean
        Dim NMemberSecondPop As Short

        '--------------------------
        Dim ipop As Short
        Dim igen As Short
        Dim inachf As Short
        Dim irunde As Short
        Dim QN() As Double
        Dim RN() As Double
        Dim Versuch As Short
        '--------------------------

        'TODO: On Error GoTo Err_ES_STARTEN

        ES_STARTEN = False

        'TODO: If (ipop + igen + inachf + irunde) > 4 Then GoTo Start_Evolutionsrunden

        myisrun = True

        'Werte an Variablen �bergeben
        iEvoTyp = EVO_Einstellungen1.iEvoTyp
        iPopEvoTyp = EVO_Einstellungen1.iPopEvoTyp
        isPOPUL = EVO_Einstellungen1.isPOPUL
        isMultiObjective = EVO_Einstellungen1.isMultiObjective
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
        isPareto = EVO_Einstellungen1.isPareto
        isPareto3D = False
        Interact = EVO_Einstellungen1.Interact
        isInteract = EVO_Einstellungen1.isInteract
        NMemberSecondPop = EVO_Einstellungen1.NMemberSecondPop

        Select Case Combo1.Text
            Case "Sinus-Funktion"
                globalAnzPar = CShort(Par_Sinus.Text)
                globalAnzZiel = 1
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                For i = 1 To globalAnzPar
                    mypara(i, 1) = 0
                Next
                Call Sinuskurve()
            Case "Beale-Problem" 'x1 = [-5;5], x2=[-2;2]
                globalAnzPar = 2
                globalAnzZiel = 1
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                mypara(1, 1) = 0.5
                mypara(2, 1) = 0.5
                Call Ausgangswert_Beale()
            Case "Schwefel 2.4-Problem" 'xi = [-10,10]
                globalAnzPar = CShort(Par_Schwefel.Text)
                globalAnzZiel = 1
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                For i = 1 To globalAnzPar
                    mypara(i, 1) = 1
                Next i
                Call Ausgangswert_Schwefel24()
            Case "Deb 1" 'x1 = [0.1;1], x2=[0;5]
                globalAnzPar = 2
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                mypara(1, 1) = Rnd()
                mypara(2, 1) = Rnd()
                Call Ausgangswert_MultiObPareto()
            Case "Zitzler/Deb T1" 'xi = [0,1]
                globalAnzPar = 30
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                For i = 1 To globalAnzPar
                    mypara(i, 1) = Rnd()
                Next i
                Call Ausgangswert_MultiObPareto()
            Case "Zitzler/Deb T2" 'xi = [0,1]
                globalAnzPar = 30
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                For i = 1 To globalAnzPar
                    mypara(i, 1) = Rnd()
                Next i
                Call Ausgangswert_MultiObPareto()
            Case "Zitzler/Deb T3" 'xi = [0,1]
                globalAnzPar = 15
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                For i = 1 To globalAnzPar
                    mypara(i, 1) = Rnd()
                Next i
                Call Ausgangswert_MultiObPareto()
            Case "Zitzler/Deb T4" 'x1 = [0,1], xi=[-5,5]
                globalAnzPar = 10
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                For i = 1 To globalAnzPar
                    mypara(i, 1) = Rnd()
                Next i
                Call Ausgangswert_MultiObPareto()
            Case "CONSTR" 'x1 = [0.1;1], x2=[0;5]
                globalAnzPar = 2
                globalAnzZiel = 2
                globalAnzRand = 2
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                mypara(1, 1) = Rnd()
                mypara(2, 1) = Rnd()
                Call Ausgangswert_CONSTR()
            Case "Box"
                globalAnzPar = 3
                globalAnzZiel = 3
                globalAnzRand = 2
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                mypara(1, 1) = Rnd()
                mypara(2, 1) = Rnd()
                mypara(3, 1) = Rnd()
                Call Ausgangswert_Box()
            Case "BlauesModell"
                globalAnzPar = 3
                globalAnzZiel = 3
                globalAnzRand = 2
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                mypara(1, 1) = Rnd()
                mypara(2, 1) = Rnd()
                mypara(3, 1) = Rnd()
                Call Ausgangswert_BlauesModell()
        End Select

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
            Txt = "Die Startschrittweite ist unzul�ssig oder kleiner als die minimale Schrittweite!"
            GoTo ErrCode_ES_STARTEN
        End If
        If globalAnzPar = 0 Then
            Txt = "Die Anzahl der Parameter ist unzul�ssig!"
            GoTo ErrCode_ES_STARTEN
        End If
        If NPopul < NPopEltern Then
            Txt = "Die Anzahl der Populationseltern darf nicht gr��er als die Anzahl der Populationen sein!"
            GoTo ErrCode_ES_STARTEN
        End If

        '***************************************************************************************************
        'CEvolutionsstrategie, 1. Schritt
        '***************************************************************************************************
        'Objekt der Klasse CEvolutionsstrategie wird erzeugen
        '***************************************************************************************************
        '***************************************************************************************************
        evolutionsstrategie = New dmevodll.CEvolutionsstrategie

        '***************************************************************************************************
        'CEvolutionsstrategie - ES_INI, 2. Schritt
        '***************************************************************************************************
        'Die �ffentlichen dynamischen Arrays werden initialisiert (Dn, An, Xn, Xmin, Xmax)
        'und die Anzahl der Zuielfunktionen wird festgelegt
        '***************************************************************************************************
        '***************************************************************************************************
        isOK = evolutionsstrategie.EsIni(globalAnzPar, globalAnzZiel, globalAnzRand)

        '***************************************************************************************************
        'CEvolutionsstrategie - ES_OPTIONS, 3. Schritt
        '***************************************************************************************************
        'Optionen der Evolutionsstrategie werden �bergeben
        '***************************************************************************************************
        '***************************************************************************************************
        isOK = evolutionsstrategie.EsOptions(iEvoTyp, iPopEvoTyp, isPOPUL, NRunden, NPopul, NPopEltern, iOptPopEltern, iOptEltern, iPopPenalty, NGen, NEltern, NNachf, NRekombXY, rDeltaStart, iStartPar, isdnvektor, isMultiObjective, isPareto, isPareto3D, Interact, isInteract, NMemberSecondPop)

        '***************************************************************************************************
        'CEvolutionsstrategie - ES_ES_LET_PARAMETER, 4. Schritt
        '***************************************************************************************************
        'Ausgangsparameter werden �bergeben
        '***************************************************************************************************
        '***************************************************************************************************
        For i = 1 To globalAnzPar
            myIsOK = evolutionsstrategie.EsLetParameter(i, mypara(i, 1))
        Next i

        '***************************************************************************************************
        'CEvolutionsstrategie - ES_PREPARE, 5. Schritt
        '***************************************************************************************************
        'Interne Variablen werden initialisiert, Zufallsgenerator wird initialisiert
        '***************************************************************************************************
        '***************************************************************************************************
        myIsOK = evolutionsstrategie.EsPrepare

        '***************************************************************************************************
        'CEvolutionsstrategie - ES_STARTVALUES, 6. Schritt
        '***************************************************************************************************
        'Startwerte werden zugewiesen
        '***************************************************************************************************
        '***************************************************************************************************
        myIsOK = evolutionsstrategie.EsStartvalues

        '***************************************************************************************************
        '***************************************************************************************************
        'Startwerte werden der Bedienoberfl�che zugewiesen
        '***************************************************************************************************
        '***************************************************************************************************
        EVO_Opt_Verlauf1.NRunden = evolutionsstrategie.NRunden
        EVO_Opt_Verlauf1.NPopul = evolutionsstrategie.NPopul
        EVO_Opt_Verlauf1.NGen = evolutionsstrategie.NGen
        EVO_Opt_Verlauf1.NNachf = evolutionsstrategie.NNachf
        EVO_Opt_Verlauf1.Initialisieren()

        durchlauf = 0

Start_Evolutionsrunden:
        'Cursor setzen
        'System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        '***********************************************************************************************
        'Loop �ber alle Runden
        '***********************************************************************************************
        Do While (evolutionsstrategie.EsIsNextRunde)

            irunde = evolutionsstrategie.iaktuelleRunde
            Call EVO_Opt_Verlauf1.Runden(irunde)

            myIsOK = evolutionsstrategie.EsPopBestwertspeicher()
            '***********************************************************************************************
            'Loop �ber alle Populationen
            '***********************************************************************************************
            Do While (evolutionsstrategie.EsIsNextPop)

                ipop = evolutionsstrategie.iaktuellePopulation
                Call EVO_Opt_Verlauf1.Populationen(ipop)

                myIsOK = evolutionsstrategie.EsPopVaria

                myIsOK = evolutionsstrategie.EsPopMutation

                durchlauf = NGen * NNachf * (irunde - 1)

                '***********************************************************************************************
                'Loop �ber alle Generationen
                '***********************************************************************************************
                Do While (evolutionsstrategie.EsIsNextGen)

                    igen = evolutionsstrategie.iaktuelleGeneration
                    Call EVO_Opt_Verlauf1.Generation(igen)

                    myIsOK = evolutionsstrategie.EsBestwertspeicher()
                    '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    'Loop �ber alle Nachkommen
                    '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    Do While (evolutionsstrategie.EsIsNextNachf)

                        inachf = evolutionsstrategie.iaktuellerNachfahre
                        Call EVO_Opt_Verlauf1.Nachfolger(inachf)

                        durchlauf = durchlauf + 1

                        'Ermitteln der neuen Ausgangswerte f�r Nachkommen aus den Eltern
                        myIsOK = evolutionsstrategie.EsVaria

                        'Mutieren der Ausgangswerte
                        myIsOK = evolutionsstrategie.EsMutation

                        'Auslesen der Variierten Parameter
                        myIsOK = evolutionsstrategie.EsGetParameter(globalAnzPar, mypara)

                        'Auslesen des Bestwertspeichers
                        If Not evolutionsstrategie.isMultiObjective Then
                            myIsOK = evolutionsstrategie.EsGetBestwert(Bestwert)
                        End If

                        'Bestimmen der Qualit�tsfunktion
                        myIsOK = Zielfunktion(globalAnzPar, mypara, durchlauf, Bestwert, ipop, QN, RN)

                        'Einordnen der Qualit�tsfunktion im Bestwertspeicher
                        myIsOK = evolutionsstrategie.EsBest(QN, RN)

                        System.Windows.Forms.Application.DoEvents()

                        '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        'Ende Loop �ber alle Nachkommen
                        '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    Loop


                    'Die neuen Eltern werden generiert
                    myIsOK = evolutionsstrategie.EsEltern()

                    If evolutionsstrategie.isMultiObjective Then
                        myIsOK = evolutionsstrategie.EsGetBestwert(Bestwert)
                        myIsOK = evolutionsstrategie.esGetSekund�rePopulation(Population)
                    End If

                    'Bestwerte und sekund�re Population werden gezeichnet
                    If evolutionsstrategie.isMultiObjective Then
                        'TODO: Call Bestwertzeichnen_Pareto(Bestwert, ipop)
                        Call Sekund�rePopulationZeichnen(Population)
                    End If

                    System.Windows.Forms.Application.DoEvents()

                    '***********************************************************************************************
                    'Ende Loop �ber alle Generationen
                    '***********************************************************************************************
                Loop 'Schleife �ber alle Generationen

                System.Windows.Forms.Application.DoEvents()

                'Einordnen der Qualit�tsfunktion im PopulationsBestwertspeicher
                myIsOK = evolutionsstrategie.EsPopBest

                '***********************************************************************************************
                'Ende Loop �ber alle Populationen
                '***********************************************************************************************
            Loop 'Schleife �ber alle Populationen



            'Die neuen Populationseltern werden generiert
            myIsOK = evolutionsstrategie.EsPopEltern

            System.Windows.Forms.Application.DoEvents()

            '***********************************************************************************************
            'Ende Loop �ber alle Runden
            '***********************************************************************************************
        Loop 'Schleife �ber alle Runden

        '***************************************************************************************************
        'CEvolutionsstrategie, letzter. Schritt
        '***************************************************************************************************
        'Objekt der Klasse CEvolutionsstrategie wird vernichtet
        '***************************************************************************************************
        '***************************************************************************************************
        'UPGRADE_NOTE: Das Objekt evolutionsstrategie kann erst dann gel�scht werden, wenn die Garbagecollection durchgef�hrt wurde. Klicken Sie hier f�r weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1029"'
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
        Resume EXIT_ES_STARTEN
ErrCode_ES_STARTEN:
        Beep()
        MsgBox("ES_STARTEN: " & Txt, MsgBoxStyle.Information)
        GoTo EXIT_ES_STARTEN
    End Function
    'Private Function Zielfunktion(AnzPar As Integer, Par() As Double, durchlauf As Long, Bestwert() As Double, ipop As Integer, Optional QN2 As Double) As double
    Private Function Zielfunktion(ByRef AnzPar As Short, ByRef Par(,) As Double, ByRef durchlauf As Integer, ByRef Bestwert(,) As Double, ByRef ipop As Short, ByRef QN() As Double, ByRef RN() As Double) As Boolean
        Dim i As Short
        Dim Unterteilung_X As Double
        Dim x1, x2 As Double
        Dim X() As Double
        Dim f2, f1, f3 As Double
        Dim g1, g2 As Double

        Select Case Combo1.Text

            'Single-Objective Problemstellungen
            Case "Sinus-Funktion" 'Fehlerquadrate zur Sinusfunktion |0-2pi|
                Unterteilung_X = 2 * 3.1415926535898 / (AnzPar - 1)
                QN(1) = 0
                For i = 1 To AnzPar
                    QN(1) = QN(1) + (System.Math.Sin((i - 1) * Unterteilung_X) - (-1 + (Par(i, 1) * 2))) * (System.Math.Sin((i - 1) * Unterteilung_X) - (-1 + Par(i, 1) * 2))
                Next i
                'If durchlauf Mod 25 = 0 Then
                Call Zielfunktion_zeichnen_Sinus(AnzPar, Par, durchlauf, ipop)
                'End If
            Case "Beale-Problem" 'Beale-Problem
                x1 = -5 + (Par(1, 1) * 10)
                x2 = -2 + (Par(2, 1) * 4)

                QN(1) = (1.5 - x1 * (1 - x2)) ^ 2 + (2.25 - x1 * (1 - x2) ^ 2) ^ 2 + (2.625 - x1 * (1 - x2) ^ 3) ^ 2
                Call Zielfunktion_zeichnen_SingleOb(QN(1), durchlauf, ipop)
            Case "Schwefel 2.4-Problem" 'Schwefel 2.4 S. 329
                ReDim X(globalAnzPar)
                For i = 1 To globalAnzPar
                    X(i) = -10 + Par(i, 1) * 20
                Next i
                QN(1) = 0
                For i = 1 To globalAnzPar
                    QN(1) = QN(1) + ((X(1) - X(i) ^ 2) ^ 2 + (X(i) - 1) ^ 2)
                Next i
                Call Zielfunktion_zeichnen_SingleOb(QN(1), durchlauf, ipop)

                'Multi-Objective Problemstellungen
                'Deb 2000, D1 (Konvexe Pareto-Front)
            Case "Deb 1"
                f1 = Par(1, 1) * (9 / 10) + 0.1
                f2 = (1 + 5 * Par(2, 1)) / (Par(1, 1) * (9 / 10) + 0.1)
                QN(1) = f1
                QN(2) = f2
                Call Zielfunktion_zeichnen_MultiObPar_2D(f1, f2, ipop)

                'Zitzler/Deb/Thiele 2000, T1 (Konvexe Pareto-Front)
            Case "Zitzler/Deb T1"
                f1 = Par(1, 1)
                f2 = 0
                For i = 2 To globalAnzPar
                    f2 = f2 + Par(i, 1)
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - System.Math.Sqrt(f1 / f2))
                QN(1) = f1
                QN(2) = f2
                Call Zielfunktion_zeichnen_MultiObPar_2D(f1, f2, ipop)

                'Zitzler/Deb/Thiele 2000, T2 (Non-Konvexe Pareto-Front)
            Case "Zitzler/Deb T2"
                f1 = Par(1, 1)
                f2 = 0
                For i = 2 To globalAnzPar
                    f2 = f2 + Par(i, 1)
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - (f1 / f2) * (f1 / f2))
                QN(1) = f1
                QN(2) = f2
                Call Zielfunktion_zeichnen_MultiObPar_2D(f1, f2, ipop)

                'Zitzler/Deb/Thiele 2000, T3 (disconected Pareto-Front)
            Case "Zitzler/Deb T3"
                f1 = Par(1, 1)
                f2 = 0
                For i = 2 To globalAnzPar
                    f2 = f2 + Par(i, 1)
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - System.Math.Sqrt(f1 / f2) - (f1 / f2) * System.Math.Sin(10 * 3.14159265358979 * f1))
                QN(1) = f1
                QN(2) = f2
                Call Zielfunktion_zeichnen_MultiObPar_2D(f1, f2, ipop)

                'Zitzler/Deb/Thiele 2000, T4 (local/global Pareto-Fronts)
            Case "Zitzler/Deb T4"
                f1 = Par(1, 1)
                f2 = 0
                For i = 2 To globalAnzPar
                    x2 = -5 + (Par(i, 1) * 10)
                    f2 = f2 + (x2 * x2 - 10 * System.Math.Cos(4 * 3.14159265358979 * x2))
                Next i
                f2 = 1 + 10 * (globalAnzPar - 1) + f2
                f2 = f2 * (1 - System.Math.Sqrt(f1 / f2))
                QN(1) = f1
                QN(2) = f2
                'Call Zielfunktion_zeichnen3(f1, f2, ipop)

            Case "CONSTR"
                f1 = Par(1, 1) * (9 / 10) + 0.1
                f2 = (1 + 5 * Par(2, 1)) / (Par(1, 1) * (9 / 10) + 0.1)

                g1 = (5 * Par(2, 1)) + 9 * (Par(1, 1) * (9 / 10) + 0.1) - 6
                g2 = (-1) * (5 * Par(2, 1)) + 9 * (Par(1, 1) * (9 / 10) + 0.1) - 1

                QN(1) = f1
                QN(2) = f2
                RN(1) = g1
                RN(2) = g2
                Call Zielfunktion_zeichnen_MultiObPar_2D(f1, f2, ipop)

            Case "Box"
                f1 = Par(1, 1) ^ 2
                f2 = Par(2, 1) ^ 2
                f3 = Par(3, 1) ^ 2
                g1 = Par(1, 1) + Par(3, 1) - 0.5
                g2 = Par(1, 1) + Par(2, 1) + Par(3, 1) - 0.8

                '                f1 = 1 + (1 - Par(1, 1)) ^ 5
                '                f2 = Par(2, 1)
                '                f3 = Par(3, 1)
                '
                '                g1 = Par(1, 1) ^ 2 + Par(3, 1) ^ 2 - 0.5
                '                g2 = Par(2, 1) ^ 2 + Par(3, 1) ^ 2 - 0.5

                QN(1) = f1
                QN(2) = f2
                QN(3) = f3
                RN(1) = g1
                RN(2) = g2
                Call Zielfunktion_zeichnen_MultiObPar_2D(f1, f2, f3)
            Case "BlauesModell"
                'Dateiname auslesen
                Datensatz = Me.textbox_Datensatz.Text.Substring(Me.textbox_Datensatz.Text.LastIndexOf("\") + 1)
                'Dateiendung entfernen
                Datensatz = Datensatz.Substring(0, Datensatz.Length - 4)
                Pfad = Me.textbox_Datensatz.Text.Substring(0, Me.textbox_Datensatz.Text.LastIndexOf("\") + 1)

                'modifyCN()
                'modifyBOF()
                'modifyBOA()

                BlauesModell.launchBM(Pfad, Datensatz)

                'readAmmel2002()

                'readWel()

                'Fehlerquadrate 
                'QN(1) = 0
                'For i = 1 To 336
                '    QN(1) = QN(1) + (Math.Abs(Form2.QsimAmmel(i) - Form2.QbeobAmmel(i))) ^ 2
                'Next i

                'Console.Out.WriteLine(QN(1))
        End Select
    End Function

    Private Sub Sinuskurve()
        Dim i As Short
        Dim Datenmenge As Short
        Dim Unterteilung_X As Double
        Dim Anzahl_Kalkulationen As Integer
        Dim Populationen As Short

        Datenmenge = CShort(Par_Sinus.Text)
        If EVO_Einstellungen1.isPOPUL Then
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf * EVO_Einstellungen1.NRunden
        Else
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf
        End If

        Unterteilung_X = 2 * 3.141592654 / (Datenmenge - 1)

        ReDim array_x(Datenmenge - 1)
        ReDim array_y(Datenmenge - 1)

        For i = 0 To Datenmenge - 1
            array_x(i) = System.Math.Round((i) * Unterteilung_X, 2)
            array_y(i) = System.Math.Sin((i) * Unterteilung_X)
        Next i

        With TChart1
            .Clear()
            .Header.Text = "Anpassung an Sinus-Kurve"
            .Chart.Axes.Left.Title.Caption = "Funktionswert"
            .Chart.Axes.Bottom.Title.Caption = "St�tzstelle"
            .Aspect.View3D = False
            .Legend.Visible = False
            Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
            Line1.Add(array_x, array_y)
            Populationen = 1
            If EVO_Einstellungen1.isPOPUL Then
                Populationen = EVO_Einstellungen1.NPopul
            End If
            For i = 1 To Populationen
                Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)   '$ Variablenname Point1 wird in der Schleife mehrmals verwendet!
            Next i
            .Chart.Axes.Bottom.Automatic = True
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Minimum = -1
            .Chart.Axes.Left.Maximum = 1
            .Chart.Axes.Left.Increment = 0.2
        End With

    End Sub

    Private Sub Ausgangswert_Beale()
        Dim Ausgangsergebnis As Double
        Dim Anzahl_Kalkulationen As Integer
        Dim Populationen As Short
        Dim i As Short

        If EVO_Einstellungen1.isPOPUL Then
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf * EVO_Einstellungen1.NRunden
        Else
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf
        End If

        ReDim OptErg(Anzahl_Kalkulationen)

        Ausgangsergebnis = (1.5 - 0.5 * (1 - 0.5)) ^ 2 + (2.25 - 0.5 * (1 - 0.5) ^ 2) ^ 2 + (2.625 - 0.5 * (1 - 0.5) ^ 3) ^ 2

        ReDim array_y(Anzahl_Kalkulationen - 1)
        ReDim array_x(Anzahl_Kalkulationen - 1)
        For i = 0 To Anzahl_Kalkulationen - 1
            array_y(i) = Ausgangsergebnis
            array_x(i) = i + 1
        Next i

        With TChart1
            .Clear()
            .Header.Text = "Beale-Problem-Funktionswerte"
            .Chart.Axes.Left.Title.Caption = "Funktionswert"
            .Chart.Axes.Bottom.Title.Caption = "Berechnungsschritt"
            .Aspect.View3D = False
            .Legend.Visible = False

            'Linie zeichen
            Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
            Line1.Add(array_x, array_y)
            Line1.Brush.Color = System.Drawing.Color.Red
            Line1.ClickableLine = True

            'Punkt einf�gen
            Populationen = 1
            If EVO_Einstellungen1.isPOPUL Then
                Populationen = EVO_Einstellungen1.NPopul
            End If

            For i = 1 To Populationen
                Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
                Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                Point1.Pointer.HorizSize = 3
                Point1.Pointer.VertSize = 3
            Next

            'Axen Formatieren
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Maximum = Anzahl_Kalkulationen
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Maximum = Ausgangsergebnis * 1.3
            .Chart.Axes.Left.Minimum = 0
            .Chart.Axes.Left.Logarithmic = False
        End With

    End Sub

    Private Sub Ausgangswert_Schwefel24()
        Dim Ausgangsergebnis As Double
        Dim Anzahl_Kalkulationen As Integer
        Dim Populationen As Short
        Dim i As Short
        Dim X() As Double

        If EVO_Einstellungen1.isPOPUL Then
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf * EVO_Einstellungen1.NRunden
        Else
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf
        End If

        ReDim X(globalAnzPar)
        For i = 1 To globalAnzPar
            X(i) = 10
        Next i

        Ausgangsergebnis = 0
        For i = 1 To globalAnzPar
            Ausgangsergebnis = Ausgangsergebnis + ((X(1) - X(i) ^ 2) ^ 2 + (X(i) - 1) ^ 2)
        Next i

        ReDim array_y(Anzahl_Kalkulationen - 1)
        ReDim array_x(Anzahl_Kalkulationen - 1)
        For i = 0 To Anzahl_Kalkulationen - 1
            array_y(i) = Ausgangsergebnis
            array_x(i) = i + 1
        Next i

        With TChart1
            .Clear()
            .Header.Text = "Schwefel-Problem 2.4"
            .Aspect.View3D = False
            .Legend.Visible = False

            'Linie der Anfangswerte 
            Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
            Line1.Add(array_x, array_y)
            Line1.Color = System.Drawing.Color.Red

            'Anzahl Populationen
            Populationen = 1
            If EVO_Einstellungen1.isPOPUL Then
                Populationen = EVO_Einstellungen1.NPopul
            End If

            'F�r jede Population eine Series
            For i = 1 To Populationen
                Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
                Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                Point1.Pointer.HorizSize = 3
                Point1.Pointer.VertSize = 3
            Next i

            'Formatierung der Axen
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Maximum = Anzahl_Kalkulationen
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Maximum = Ausgangsergebnis * 1.2
            .Chart.Axes.Left.Minimum = -1
            .Chart.Axes.Left.Logarithmic = False
        End With
    End Sub

    Private Sub Ausgangswert_BlauesModell()
        Dim Ausgangsergebnis As Double
        Dim Anzahl_Kalkulationen As Integer
        Dim Populationen As Short
        Dim i As Short
        Dim X() As Double

        If EVO_Einstellungen1.isPOPUL Then
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf * EVO_Einstellungen1.NRunden
        Else
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf
        End If

        ReDim X(globalAnzPar)
        For i = 1 To globalAnzPar
            X(i) = 10
        Next i

        Ausgangsergebnis = 0
        For i = 1 To globalAnzPar
            Ausgangsergebnis = Ausgangsergebnis + ((X(1) - X(i) ^ 2) ^ 2 + (X(i) - 1) ^ 2)
        Next i

        ReDim array_y(Anzahl_Kalkulationen - 1)
        ReDim array_x(Anzahl_Kalkulationen - 1)
        For i = 0 To Anzahl_Kalkulationen - 1
            array_y(i) = Ausgangsergebnis
            array_x(i) = i + 1
        Next i

        With TChart1
            .Clear()
            .Header.Text = "BlauesModell"
            .Aspect.View3D = False
            .Legend.Visible = False

            'Linie der Anfangswerte 
            Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
            Line1.Add(array_x, array_y)
            Line1.Color = System.Drawing.Color.Red

            'Anzahl Populationen
            Populationen = 1
            If EVO_Einstellungen1.isPOPUL Then
                Populationen = EVO_Einstellungen1.NPopul
            End If

            'F�r jede Population eine Series
            For i = 1 To Populationen
                Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
                Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                Point1.Pointer.HorizSize = 3
                Point1.Pointer.VertSize = 3
            Next i

            'Formatierung der Axen
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Maximum = Anzahl_Kalkulationen
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Maximum = Ausgangsergebnis * 1.2
            .Chart.Axes.Left.Minimum = -1
            .Chart.Axes.Left.Logarithmic = False
        End With
    End Sub

    Private Sub Ausgangswert_MultiObPareto()
        Dim Populationen As Short
        Dim i, j As Short

        Populationen = EVO_Einstellungen1.NPopul

        With TChart1
            .Clear()
            .Aspect.View3D = False
            .Legend.Visible = False
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Maximum = 1
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Bottom.Increment = 0.1
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Maximum = 10
            .Chart.Axes.Left.Minimum = 0
            .Chart.Axes.Left.Increment = 2

            'S0: Series f�r die Population.
            Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
            Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point1.Color = System.Drawing.Color.Orange
            Point1.Pointer.HorizSize = 2
            Point1.Pointer.VertSize = 2

            'S1: Series f�r die Sekund�re Population
            Dim Point2 As New Steema.TeeChart.Styles.Points(.Chart)
            Point2.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point2.Color = System.Drawing.Color.Blue
            Point2.Pointer.HorizSize = 3
            Point2.Pointer.VertSize = 3

            'S2: Series f�r Bestwert
            Dim Point3 As New Steema.TeeChart.Styles.Points(.Chart)
            Point3.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point3.Color = System.Drawing.Color.Green
            Point3.Pointer.HorizSize = 3
            Point3.Pointer.VertSize = 3

            Select Case Combo1.Text

                Case "Deb 1"
                    Dim Array1X(100) As Double
                    Dim Array1Y(100) As Double
                    Dim Array2X(100) As Double
                    Dim Array2Y(100) As Double
                    .Header.Text = "Deb D1 - MO-konvex"

                    'S3: Linie 1 wird errechnet und gezeichnet
                    For j = 0 To 100
                        Array1X(j) = 0.1 + j * 0.009
                        Array1Y(j) = 1 / Array1X(j)
                    Next j
                    Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
                    Line1.Brush.Color = System.Drawing.Color.Green
                    Line1.ClickableLine = True
                    .Series(3).Add(Array1X, Array1Y)

                    'S4: Linie 2 wird errechnet und gezeichnet
                    For j = 0 To 100
                        Array2X(j) = 0.1 + j * 0.009
                        Array2Y(j) = (1 + 5) / Array2X(j)
                    Next j
                    Dim Line2 As New Steema.TeeChart.Styles.Line(.Chart)
                    Line2.Brush.Color = System.Drawing.Color.Red
                    Line2.ClickableLine = True
                    .Series(4).Add(Array2X, Array2Y)

                Case "Zitzler/Deb T1"
                    Dim ArrayX(1000) As Double
                    Dim ArrayY(1000) As Double
                    .Header.Text = "Zitzler/Deb/Theile T1"
                    .Chart.Axes.Left.Maximum = 7
                    .Chart.Axes.Left.Increment = 0.5

                    'S3: Serie f�r die Grenze
                    For j = 0 To 1000
                        ArrayX(j) = j / 1000
                        ArrayY(j) = 1 - System.Math.Sqrt(ArrayX(j))
                    Next j
                    Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
                    Line1.Brush.Color = System.Drawing.Color.Green
                    Line1.ClickableLine = True
                    .Series(3).Add(ArrayX, ArrayY)

                Case "Zitzler/Deb T2"
                    Dim ArrayX(100) As Double
                    Dim ArrayY(100) As Double
                    .Header.Text = "Zitzler/Deb/Theile T2"
                    .Chart.Axes.Left.Maximum = 7

                    'S3: Serie f�r die Grenze
                    For j = 0 To 100
                        ArrayX(j) = j / 100
                        ArrayY(j) = 1 - (ArrayX(j) * ArrayX(j))
                    Next j
                    Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
                    Line1.Brush.Color = System.Drawing.Color.Green
                    Line1.ClickableLine = True
                    .Series(3).Add(ArrayX, ArrayY)

                Case "Zitzler/Deb T3"
                    Dim ArrayX(100) As Double
                    Dim ArrayY(100) As Double
                    .Header.Text = "Zitzler/Deb/Theile T3"
                    .Chart.Axes.Bottom.Increment = 0.2
                    .Chart.Axes.Left.Maximum = 7
                    .Chart.Axes.Left.Minimum = -1
                    .Chart.Axes.Left.Increment = 0.5

                    'S3: Serie f�r die Grenze
                    For j = 0 To 100
                        ArrayX(j) = j / 100
                        ArrayY(j) = 1 - System.Math.Sqrt(ArrayX(j)) - ArrayX(j) * System.Math.Sin(10 * 3.14159265358979 * ArrayX(j))
                    Next j
                    Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
                    Line1.Brush.Color = System.Drawing.Color.Green
                    Line1.ClickableLine = True
                    .Series(3).Add(ArrayX, ArrayY)

                Case "Zitzler/Deb T4"
                    Dim ArrayX(10, 101) As Double
                    Dim ArrayY(10, 101) As Double
                    .Header.Text = "Zitzler/Deb/Theile T1"
                    .Chart.Axes.Bottom.Increment = 0.2
                    .Chart.Axes.Left.Maximum = 7
                    .Chart.Axes.Left.Increment = 0.5

                    'TODO: funzt net!
                    'S3 bis S13: Serie f�r die Grenze
                    For i = 1 To 10
                        For j = 1 To 100
                            ArrayX(i, j) = j / 100
                            ArrayY(i, j) = 1 - System.Math.Sqrt(ArrayX(i, j)) - ArrayX(i, j) * System.Math.Sin(10 * 3.14159265358979 * ArrayX(i, j))
                        Next j
                        Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
                        'Line1.Brush.Color = System.Drawing.Color.Green
                        Line1.ClickableLine = True
                        .Series(i + 2).Add(ArrayX(i, j), ArrayY(i, j))
                    Next i

                    'For i = 1 To 10
                    '    .AddSeries(TeeChart.ESeriesClass.scLine)
                    '    .Series(Populationen + i).asLine.LinePen.Width = 2
                    '    .Series(Populationen + i).Color = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue))
                    '    For j = 0 To 1000
                    '        ArrayX(j) = j / 1000
                    '        ArrayY(j) = (1 + (i - 1) / 4) * (1 - System.Math.Sqrt(ArrayX(j) / (1 + (i - 1) / 4)))
                    '    Next j
                    '    .Series(Populationen + i).AddArray(1000, ArrayY, ArrayX)
                    'Next i

            End Select
        End With
    End Sub

    Private Sub Ausgangswert_CONSTR()
        Dim Populationen As Short
        Dim i, j As Short
        Dim Array1X(100) As Double
        Dim Array1Y(100) As Double
        Dim Array2X(100) As Double
        Dim Array2Y(100) As Double
        Dim Array3X(61) As Double
        Dim Array3Y(61) As Double
        Dim Array4X(61) As Double
        Dim Array4Y(61) As Double

        If EVO_Einstellungen1.isPOPUL Then
            Populationen = EVO_Einstellungen1.NPopul
        Else
            Populationen = 1
        End If

        With TChart1
            .Clear()
            .Header.Text = "CONSTR"
            .Aspect.View3D = False
            .Legend.Visible = False

            'S0: Hier wird nur eine Population.
            Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
            Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point1.Color = System.Drawing.Color.Orange
            Point1.Pointer.HorizSize = 2
            Point1.Pointer.VertSize = 2

            'S1: Series f�r die Sekund�re Population
            Dim Point2 As New Steema.TeeChart.Styles.Points(.Chart)
            Point2.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point2.Color = System.Drawing.Color.Blue
            Point2.Pointer.HorizSize = 3
            Point2.Pointer.VertSize = 3

            'S2: Series f�r die Sekund�re Population
            Dim Point3 As New Steema.TeeChart.Styles.Points(.Chart)
            Point3.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point3.Color = System.Drawing.Color.Green
            Point3.Pointer.HorizSize = 3
            Point3.Pointer.VertSize = 3

            'S3: Serie f�r die Grenze 1
            For j = 0 To 100
                Array1X(j) = 0.1 + j * 0.009
                Array1Y(j) = 1 / Array1X(j)
            Next j
            Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
            Line1.Brush.Color = System.Drawing.Color.Red
            Line1.ClickableLine = True
            .Series(3).Add(Array1X, Array1Y)

            'S4: Serie f�r die Grenze 2
            For j = 0 To 100
                Array2X(j) = 0.1 + j * 0.009
                Array2Y(j) = (1 + 5) / Array2X(j)
            Next j
            Dim Line2 As New Steema.TeeChart.Styles.Line(.Chart)
            Line2.Brush.Color = System.Drawing.Color.Red
            Line2.ClickableLine = True
            .Series(4).Add(Array2X, Array2Y)

            'S5: Serie f�r die Grenze 3
            ReDim Array3X(61)
            ReDim Array3Y(61)
            For j = 0 To 61
                Array3X(j) = 0.1 + (j + 2) * 0.009
                Array3Y(j) = (7 - 9 * Array3X(j)) / Array3X(j)
            Next j
            Dim Line3 As New Steema.TeeChart.Styles.Line(.Chart)
            Line3.Brush.Color = System.Drawing.Color.Blue
            Line3.ClickableLine = True
            .Series(5).Add(Array3X, Array3Y)

            'S6: Serie f�r die Grenze 4
            ReDim Array4X(61)
            ReDim Array4Y(61)
            For j = 0 To 61
                Array4X(j) = 0.1 + (j + 2) * 0.009
                Array4Y(j) = (9 * Array4X(j)) / Array4X(j)
            Next j
            Dim Line4 As New Steema.TeeChart.Styles.Line(.Chart)
            Line4.Brush.Color = System.Drawing.Color.Red
            Line4.ClickableLine = True
            .Series(6).Add(Array4X, Array4Y)

            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Maximum = 1
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Bottom.Increment = 0.2
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Maximum = 10
            .Chart.Axes.Left.Minimum = 0
            .Chart.Axes.Left.Increment = 2
        End With
    End Sub

    Private Sub Ausgangswert_Box()
        Dim Populationen As Short
        Dim i, j As Short
        Dim ArrayX(100) As Double
        Dim ArrayY(100) As Double

        If EVO_Einstellungen1.isPOPUL Then
            Populationen = EVO_Einstellungen1.NPopul
        Else
            Populationen = 1
        End If

        With TChart1
            .Clear()
            .Header.Text = "Box"
            .Aspect.View3D = True
            .Aspect.Chart3DPercent = 60
            .Legend.Visible = False
            '.AddSeries(TeeChart.ESeriesClass.scPoint3D)
            '.Series(0).asPoint3D.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '.Series(0).asPoint3D.LinePen.Visible = False
            '.Series(0).asPoint3D.Pointer.HorizontalSize = 1
            '.Series(0).asPoint3D.Pointer.VerticalSize = 1
            'For i = 1 To Populationen
            '    .AddSeries(TeeChart.ESeriesClass.scPoint3D)
            '    .Series(i).asPoint3D.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '    .Series(i).asPoint3D.LinePen.Visible = False
            '    .Series(i).asPoint3D.Pointer.HorizontalSize = 3
            '    .Series(i).asPoint3D.Pointer.VerticalSize = 3
            'Next i
            '.AddSeries(TeeChart.ESeriesClass.scPoint3D)
            '.AddSeries(TeeChart.ESeriesClass.scPoint3D)
            '.Series(Populationen + 2).asPoint3D.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '.Series(Populationen + 2).asPoint3D.LinePen.Visible = False
            '.Series(Populationen + 2).asPoint3D.Pointer.HorizontalSize = 2
            '.Series(Populationen + 2).asPoint3D.Pointer.VerticalSize = 2
            '.Series(Populationen + 2).Color = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red))

            '.Axis.Bottom.Automatic = False
            '.Axis.Bottom.Maximum = 1
            '.Axis.Bottom.Minimum = 0
            '.Axis.Bottom.Increment = 0.2
            '.Axis.Left.Automatic = False
            '.Axis.Left.Maximum = 1
            '.Axis.Left.Minimum = 0
            '.Axis.Left.Increment = 0.2
            '.Axis.Depth.Automatic = False
            '.Axis.Depth.Visible = True
            '.Axis.Depth.Maximum = 1
            '.Axis.Depth.Minimum = 0
            '.Axis.Depth.Increment = 0.2
        End With
    End Sub

    Private Sub Zielfunktion_zeichnen_Sinus(ByRef AnzPar As Short, ByRef Par(,) As Double, ByRef durchlauf As Integer, ByRef ipop As Short)
        Dim i As Short
        Dim x1, x2 As Double
        Dim Zielfunktion As Double
        Dim Datenmenge As Short
        Dim Unterteilung_X As Double

        Unterteilung_X = 2 * 3.141592654 / (AnzPar - 1)
        ReDim array_x(AnzPar - 1) 'TODO: jetzt richtig?
        ReDim array_y(AnzPar - 1) 'TODO: jetzt richtig?
        For i = 0 To AnzPar - 1
            array_x(i) = System.Math.Round((i) * Unterteilung_X, 2)
            array_y(i) = (-1 + Par(i + 1, 1) * 2)
        Next i

        With TChart1
            .Series(ipop).Clear()
            .Series(ipop).Add(array_x, array_y)
        End With
    End Sub

    Private Sub Zielfunktion_zeichnen_SingleOb(ByRef Wert As Double, ByRef durchlauf As Integer, ByRef ipop As Short)

        TChart1.Series(ipop).Add(durchlauf, Wert, "")

    End Sub

    Private Sub Zielfunktion_zeichnen_MultiObPar_2D(ByRef f1 As Double, ByRef f2 As Double, ByRef ipop As Short)

        TChart1.Series(0).Add(f1, f2, "")

    End Sub

    Private Sub Zielfunktion_zeichnen_MultiObPar_3D(ByRef f1 As Double, ByRef f2 As Double, ByRef f3 As Double)

        'TODO: Hier muss eine 3D-Reihe angezeigt werden
        'TChart1.Series(0).Add(f1, f2, f3, "")

    End Sub

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

    Private Sub Sekund�rePopulationZeichnen(ByRef Population(,) As Double)
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

    'TODO: Welchen Zweck hat das?
    Private Sub Par_Sinus_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles Par_Sinus.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        'TODO: UPGRADE_ISSUE: Zuweisung wird nicht unterst�tzt: KeyAscii an Nicht-Null-Wert Klicken Sie hier f�r weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1058"'
        KeyAscii = KEYOK(KeyAscii, AllowIntegerOnly)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.OpenFileDialog1.ShowDialog()
        Me.textbox_Datensatz.Clear()
        Me.textbox_Datensatz.AppendText(Me.OpenFileDialog1.FileName)
    End Sub

End Class