Option Strict Off

Public Class PES

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse PES (Parametric Evolution Strategy)                            ****
    '****                                                                       ****
    '**** Modifizierte Evolutionsstrategie nach Rechenberg und Schwefel         ****
    '**** Klasse enthält alle Funktionen und Methoden zur Anwendung             ****
    '**** der Evolutionsstategie                                                ****
    '****                                                                       ****
    '**** Literatur:                                                            ****
    '**** 1) Rechenberg, Ingo, Evolutionsstrategie '94, Fromman-Holzboog, 1994  ****
    '**** 2) Schwefel, Hans-Paul, Evolution and Optimum Seeking, Wiley, 1995    ****
    '**** 3) Deb, Kalyanmoy, Multi-Objective Optimization using Evolutionary    ****
    '****    Algorithms, Wiley, 2001                                            ****
    '****                                                                       ****
    '**** Dirk Muschalla, Christoph Huebner                                     ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '****                                                                       ****
    '**** Dezember 2003                                                         ****
    '****                                                                       ****
    '**** Letzte Änderung: Juli 2007                                            ****
    '*******************************************************************************
    '*******************************************************************************

    'Deklarationsteil Variablen und Strukturen
    '#########################################

    Public PES_Settings As PES_Settings

    'Beziehung
    '--------------
    Public Enum Beziehung As Integer
        keine = 0
        kleiner = 1
        kleinergleich = 2
        groesser = 3
        groessergleich = 4
    End Enum

    'Structure zum Speichern der Werte die aus den OptDateien generiert werden
    Private Structure Struct_AktPara
        Dim Dn() As Double                  'Schrittweitenvektor (Dimension varanz)
        Dim Xn() As Double                  'aktuelle Variablenwerte (Dimension varanz)
        Dim Bez() As Beziehung              'Beziehungen
    End Structure

    Private AktPara As Struct_AktPara

    'Diese Struktur speichert den aktuellen Zustand
    'ToDo: Könnte man auch entfernen wenn man die Schleifenkontrolle ins Form legt
    Public Structure Struct_iAkt
        Dim iAktRunde As Integer              'Zähler für aktuelle Runde
        Dim iAktPop As Integer                'Zähler für aktuelle Population
        Dim iAktGen As Integer                'Zähler für aktuelle Generation
        Dim iAktNachf As Integer              'Zähler für aktuellen Nachfahren
    End Structure

    'Muss Public sein, da das Form hiermit die Schleifen kontrolliert
    Public PES_iAkt As Struct_iAkt

    '---Anzahlen-----------
    Public Structure Anzahl
        Dim Para As Integer                 'Anzahl Parameter
        Dim Penalty As Integer              'Anzahl der Penaltyfunktionen
        Dim Constr As Integer               'Anzahl der Randbedingungen
    End Structure

    Public Anz As Anzahl                    'Struct für die Anzahlen

    '---PopElternwerte-----
    Private Xp(,,) As Double                'PopulationsElternwert der Variable
    Private Dp(,,) As Double                'PopulationsElternschrittweite

    '---PopBestwerte-------
    Private Xbpop(,,) As Double             'Bestwertspeicher Variablenwerte für eine Population
    Private Dbpop(,,) As Double             'Bestwertspeicher Schrittweite für eine Population
    Private Qbpop(,) As Double              'Bestwertspeicher für eine Population

    '---Elternwerte--------
    Private Xe(,,) As Double                'Elternwerte der Variablen
    Private De(,,) As Double                'Elternschrittweite

    '---Bestwerte----------
    Public Structure Bestwerte
        Dim Xb(,,) As Double                'Bestwertspeicher Variablenwerte für eine Generation
        Dim Db(,,) As Double                'Bestwertspeicher Schrittweite für eine Generation
        Dim Qb(,,) As Double                'Bestwertspeicher für eine Generation
        Dim Rb(,,) As Double                'Restriktionen für eine Generation
    End Structure

    Public Best As Bestwerte

    '---Stuff--------------
    Private Distanceb() As Double           'Array mit Crowding-Distance (Neighbourhood-Rekomb.)
    Private PenaltyDistance(,) As Double    'Array für normierte Raumabstände (Neighbourhood-Rekomb.)
    '---------------------
    Private SekundärQb(-1) As Individuum    'Sekundäre Population wird mit -1 initialisiert dann länge 0

    Const galpha As Double = 1.3            'Faktor alpha = 1.3 auf Generationsebene nach Rechenberg
    Const palpha As Double = 1.1            'Faktor alpha = 1.1 auf Populationsebene nach Rechenberg

    Dim NDSorting() As Individuum

    Private Structure Struct_Sortierung
        Dim Index As Integer
        Dim penalty() As Double
    End Structure

    Private Structure Struct_Neighbourhood
        Dim Index As Integer
        Dim distance As Double
    End Structure

    'Methoden
    '########

    'Initialisierung der PES
    '***************************************
    Public Sub PesInitialise(ByRef PES_Settings As PES_Settings, ByVal AnzPara As Integer, ByVal AnzPenalty As Integer, ByVal AnzConstr As Integer, ByRef Parameter() As Double, ByVal beziehungen() As Beziehung, ByVal Method As String)

        'Schritt 1: PES - ES_OPTIONS
        'Optionen der Evolutionsstrategie werden übergeben
        Call EsSettings(PES_Settings, Method)

        'Schritt 2: PES - ES_PREPARE
        'Interne Variablen werden initialisiert, Zufallsgenerator wird initialisiert
        Call EsPrepare(AnzPara, AnzPenalty, AnzConstr)

        'Schritt 3: PES - ES_STARTVALUES
        'Startwerte werden zugewiesen
        Call EsStartvalues(Parameter, beziehungen)

    End Sub

    'Schritt 1: ES_SETTINGS
    'Function ES_SETTINGS übergibt Optionen für Evolutionsstrategie und Prüft die eingestellten Optionen
    '***************************************************************************************************
    Private Sub EsSettings(ByRef Settings As PES_Settings, ByVal Method As String)

        'Überprüfung der Übergebenen Werte
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        If (Settings.ty_EvoTyp < 1 Or Settings.ty_EvoTyp > 2) Then
            Throw New Exception("Typ der Evolutionsstrategie ist nicht '+' oder ','")
        End If
        If (Settings.Pop.ty_PopEvoTyp < 1 Or Settings.Pop.ty_PopEvoTyp > 2) Then
            Throw New Exception("Typ der Evolutionsstrategie auf Pupulationsebene ist nicht '+' oder ','")
        End If
        If (Settings.Pop.n_Runden < 1) Then
            Throw New Exception("Die Anzahl der Runden ist kleiner 1")
        End If
        If (Settings.Pop.n_Popul < 1) Then
            Throw New Exception("Die Anzahl der Populationen ist kleiner 1")
        End If
        If (Settings.Pop.n_PopEltern < 1) Then
            Throw New Exception("Die Anzahl der Populationseltern ist kleiner 1")
        End If
        If (Settings.Pop.ty_OptPopEltern < 1 Or Settings.Pop.ty_OptPopEltern > 3) Then
            Throw New Exception("Ermittlung der Populationseltern ist nicht Mittelwert, Rekombination oder Selektion!")
        End If
        If (Settings.ty_OptEltern < 1 Or Settings.ty_OptEltern > 6) Then
            Throw New Exception("Strategie zur Ermittlung der Eltern ist nicht möglich!")
        End If
        If (Settings.n_Eltern < 1) Then
            Throw New Exception("Die Anzahl der Eltern ist kleiner 1!")
        End If
        If (Settings.n_Nachf < 1) Then
            Throw New Exception("Die Anzahl der Nachfahren ist kleiner 1!")
        End If
        If (Settings.n_Gen < 1) Then
            Throw New Exception("Die Anzahl der Generationen ist kleiner 1!")
        End If
        If (Settings.n_RekombXY < 1) Then
            Throw New Exception("Der Wert für die X/Y-Schema Rekombination ist kleiner 1!")
        End If
        If (Settings.DnStart < 0) Then
            Throw New Exception("Die Startschrittweite darf nicht kleiner 0 sein!")
        End If
        If (Settings.ty_StartPar < 1 Or Settings.ty_StartPar > 2) Then
            Throw New Exception("Die Startaparameter dürfen nur zufällig sein oder aus den Originalparameter bestehen!")
        End If
        If (Settings.Pop.n_Popul < Settings.Pop.n_PopEltern) Then
            Throw New Exception("Die Anzahl der Populationseltern darf nicht größer als die Anzahl der Populationen!")
        End If
        If (Settings.n_Nachf <= Settings.n_Eltern) And Not Method = "HYBRID" Then
            Throw New Exception("Die Anzahl der Eltern muss kleiner als die Anzahl der Nachfahren!" & Chr(13) & Chr(10) & "'Rechenberg 73' schlägt ein Verhältnis von 1:3 bis 1:5 vor.")
        End If

        'Übergabe der Optionen
        'xxxxxxxxxxxxxxxxxxxxx

        Me.PES_Settings = Settings

    End Sub

    'Schritt 2: ES_PREPARE
    'Initialisiert benötigte dynamische Arrays und legt Anzahl der Zielfunktionen fest
    'Initialisiert alle internen Arrays und setzt den
    'Bestwertspeicher auf sehr großen Wert (Strategie minimiert in dieser Umsetzung)
    'TODO: ESPrepare Für Paretooptimierung noch nicht fertig!!!!
    '*******************************************************************************
    Private Sub EsPrepare(ByVal AnzPara As Integer, ByVal AnzPenalty As Integer, ByVal AnzConstr As Integer)
        Dim m, n, l, i As Integer

        'Überprüfung der Eingabeparameter (es muss mindestens ein Parameter variiert und eine
        'Penaltyfunktion ausgewertet werden)

        If (AnzPara <= 0 Or AnzPenalty <= 0) Then
            Throw New Exception("Es muss mindestens ein Parameter variiert und eine Penaltyfunktion ausgewertet werden")
        End If

        'Anzahlen werden gesetzt
        Anz.Para = AnzPara                         'Anzahl der Parameter wird übergeben
        Anz.Penalty = AnzPenalty                   'Anzahl der Zielfunktionen wird übergeben
        Anz.Constr = AnzConstr                 'Anzahl der Randbedingungen wird übergeben

        'Dynamisches Array Initialisieren
        ReDim AktPara.Xn(Anz.Para - 1)                'Variablenvektor wird initialisiert
        ReDim AktPara.Bez(Anz.Para - 1)
        ReDim AktPara.Dn(Anz.Para - 1)                'Schrittweitenvektor wird initialisiert

        'Parametervektoren initialisieren
        ReDim Dp(Anz.Para - 1, PES_Settings.n_Eltern - 1, PES_Settings.Pop.n_PopEltern - 1)
        ReDim Xp(Anz.Para - 1, PES_Settings.n_Eltern - 1, PES_Settings.Pop.n_PopEltern - 1)
        ReDim Qbpop(PES_Settings.Pop.n_Popul - 1, Anz.Penalty - 1)
        ReDim Dbpop(Anz.Para - 1, PES_Settings.n_Eltern - 1, PES_Settings.Pop.n_Popul - 1)
        ReDim Xbpop(Anz.Para - 1, PES_Settings.n_Eltern - 1, PES_Settings.Pop.n_Popul - 1)
        '---------------------
        ReDim De(Anz.Para - 1, PES_Settings.n_Eltern - 1, PES_Settings.Pop.n_Popul - 1)
        ReDim Xe(Anz.Para - 1, PES_Settings.n_Eltern - 1, PES_Settings.Pop.n_Popul - 1)
        '---------------------
        ReDim Best.Db(Anz.Para - 1, PES_Settings.n_Eltern - 1, PES_Settings.Pop.n_Popul - 1)
        ReDim Best.Xb(Anz.Para - 1, PES_Settings.n_Eltern - 1, PES_Settings.Pop.n_Popul - 1)
        ReDim Best.Qb(PES_Settings.n_Eltern - 1, PES_Settings.Pop.n_Popul - 1, Anz.Penalty - 1)
        ReDim Best.Rb(PES_Settings.n_Eltern - 1, PES_Settings.Pop.n_Popul - 1, Anz.Constr - 1)

        'NDSorting wird nur benötigt, falls eine Paretofront approximiert wird
        If PES_Settings.is_MO_Pareto Then
            ReDim NDSorting(PES_Settings.n_Eltern + PES_Settings.n_Nachf - 1)
            Call Individuum.New_Array("NDSorting", NDSorting)
            For i = 0 To PES_Settings.n_Eltern + PES_Settings.n_Nachf - 1
                ReDim NDSorting(i).PES_d(Anz.Para - 1)
                ReDim NDSorting(i).PES_X(Anz.Para - 1)
            Next i
            If PES_Settings.ty_OptEltern = EVO_ELTERN.Neighbourhood Then
                ReDim PenaltyDistance(PES_Settings.n_Eltern - 1, PES_Settings.n_Eltern - 1)
                ReDim Distanceb(PES_Settings.n_Eltern - 1)
            End If
        End If

        For n = 0 To PES_Settings.n_Eltern - 1
            For m = 0 To PES_Settings.Pop.n_Popul - 1
                For l = 0 To Anz.Penalty - 1
                    'Qualität der Eltern (Anzahl = parents) wird auf sehr großen Wert gesetzt
                    Best.Qb(n, m, l) = 1.0E+300
                Next l
                If Anz.Constr > 0 Then
                    For l = 0 To Anz.Constr - 1
                        'Restriktion der Eltern (Anzahl = parents) wird auf sehr kleinen Wert gesetzt
                        Best.Rb(n, m, l) = -1.0E+300
                    Next l
                End If
            Next m
        Next

        'Falls NDSorting Crowding Distance wird initialisiert
        If PES_Settings.is_MO_Pareto Then
            For n = 0 To PES_Settings.Pop.n_Popul - 1
                For m = 0 To Anz.Penalty - 1
                    Select Case PES_Settings.Pop.ty_PopPenalty

                        Case EVO_POP_PENALTY.Crowding
                            'Qualität der Populationseltern wird auf sehr großen Wert gesetzt
                            Qbpop(n, m) = 1.0E+300

                        Case EVO_POP_PENALTY.Spannweite
                            'Qualität der Populationseltern wird auf 0 gesetzt
                            Qbpop(n, m) = 0
                    End Select
                Next m
            Next n
        Else
            For n = 0 To PES_Settings.Pop.n_Popul - 1
                For m = 0 To Anz.Penalty - 1
                    'Qualität der Populationseltern wird auf sehr großen Wert gesetzt
                    Qbpop(n, m) = 1.0E+300
                Next m
            Next n
        End If

        'Zufallsgenerator initialisieren
        Randomize()

        'Informationen über aktuelle Runden übergeben
        PES_iAkt.iAktRunde = 0
        PES_iAkt.iAktPop = 0
        PES_iAkt.iAktGen = 0
        PES_iAkt.iAktNachf = 0

    End Sub

    'Schritt 1: Function ES_STARTVALUES setzt die Startwerte - Vorsicht: ÜBERLADEN
    'PES_Settings.iStartPar 1: Zufällige Startwert  -> Schrittweite = Startschrittweite
    '                                               -> Parameterwert = zufällig [0,1]
    'PES_Settings.iStartPar 2: Originalparameter    -> Schrittweite = Startschrittweite
    '                                               -> Parameterwert = Originalparameter
    '***********************************************************************************
    Public Sub EsStartvalues(ByVal Parameter() As Double, ByVal beziehungen() As Beziehung)

        Dim i As Integer

        'Dynamisches Array wird mit Werten belegt
        For i = 0 To Anz.Para - 1
            If Parameter(i) < 0 Or Parameter(i) > 1 Then
                Throw New Exception("Der Startparameter " & i & " liegt nicht zwischen 0 und 1. Sie müssen hier skaliert vorliegen")
            End If
            AktPara.Xn(i) = Parameter(i)
            AktPara.Bez(i) = beziehungen(i)
            AktPara.Dn(i) = PES_Settings.DnStart
        Next

        Dim n, v, m As Integer

        'Zufallsgenerator initialisieren
        Randomize()

        'Die Startparameter für die Eltern werden gesetzt
        Select Case PES_Settings.ty_StartPar

            Case EVO_STARTPARAMETER.Zufall 'Zufällige Startwerte
                For v = 0 To Anz.Para - 1
                    For n = 0 To PES_Settings.n_Eltern - 1
                        For m = 0 To PES_Settings.Pop.n_PopEltern - 1
                            'Startwert für die Elternschrittweite wird zugewiesen
                            Dp(v, n, m) = AktPara.Dn(0)
                            'Startwert für die Eltern werden zugewiesen
                            '(Zufallszahl zwischen 0 und 1)
                            Xp(v, n, m) = Rnd()
                        Next m
                    Next n
                Next v

            Case EVO_STARTPARAMETER.Original 'Originalparameter
                For v = 0 To Anz.Para - 1
                    For n = 0 To PES_Settings.n_Eltern - 1
                        For m = 0 To PES_Settings.Pop.n_PopEltern - 1
                            'Startwert für die Elternschrittweite wird zugewiesen
                            Dp(v, n, m) = AktPara.Dn(0)
                            'Startwert für die Eltern werden zugewiesen
                            '(alle gleich Anfangswerte)
                            Xp(v, n, m) = AktPara.Xn(v)
                        Next m
                    Next n
                Next v
        End Select

    End Sub

    'Überladen: Falls Startwerte aus CES kommen!
    Public Sub EsStartvalues(ByVal AktDn() As Double, ByVal AktXn() As Double, ByVal IndexElter As Integer)

        Dim v As Integer

        'Die Startparameter werden aus den PES_Parents aus der CES gesetzt
        For v = 0 To Anz.Para - 1
            'Startwert für die Elternschrittweite wird zugewiesen
            Dp(v, IndexElter, 0) = AktDn(v)
            'Startwert für die Eltern werden zugewiesen
            '(alle gleich Anfangswerte)
            Xp(v, IndexElter, 0) = AktXn(v)
        Next v


    End Sub

    'ES_GET_PARAMETER - dient zur Rückgabe der mutierten Parameter
    '*************************************************************
    Public Function EsGetParameter() As Double()

        Return AktPara.Xn

    End Function

    'ES_GET_DN - dient zur Rückgabe der Schrittweite
    '*************************************************************
    Public Function EsGetDN() As Double()

        Return AktPara.Dn

    End Function

    'ES_GET_BESTWERT - gibt den kompletten Bestwertspeicher aus
    '**********************************************************
    Public Function EsGetBestwert() As Double(,)

        Dim i, j As Integer
        Dim Bestwert(,) As Double

        ReDim Bestwert(PES_Settings.n_Eltern - 1, Anz.Penalty - 1)

        For i = 0 To Anz.Penalty - 1
            For j = 0 To PES_Settings.n_Eltern - 1
                Bestwert(j, i) = Best.Qb(j, PES_iAkt.iAktPop, i)
            Next j
        Next i

        Return Bestwert

    End Function

    'Function um PopReproduktion, PopMutation, Reproduktion und Mutio n direkt ablaufen zu lassen
    '********************************************************************************************
    Public Sub EsReproMut()

        'POPULATIONS REPRODUKTIONSPROZESS
        '################################
        'Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern der Population
        Call EsPopReproduktion()

        'POPULATIONS MUTATIONSPROZESS
        '############################
        'Mutieren der Ausgangswerte der Population
        Call EsPopMutation()

        'REPRODUKTIONSPROZESS
        '####################
        'Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern
        Call EsReproduktion()

        'MUTATIONSPROZESS
        '################
        'Mutieren der Ausgangswerte
        Call EsMutation()

    End Sub

    'ES_POP_VARIA - REPRODUKTIONSPROZESS - ToDo: Beschreibung fehlt
    '*******************************************************************************
    Public Sub EsPopReproduktion()

        Dim m, n, v As Integer
        Dim R As Integer                      'Zufälliger Integer Wert

        Select Case PES_Settings.Pop.ty_OptPopEltern

            Case EVO_POP_ELTERN.Rekombination 'MultiRekombination über alle Eltern (x/x,y) oder (x/x+y)
                For n = 0 To PES_Settings.n_Eltern - 1
                    R = Int(PES_Settings.Pop.n_PopEltern * Rnd())
                    For v = 0 To Anz.Para - 1
                        'Selektion der Schrittweite
                        De(v, n, PES_iAkt.iAktPop) = Dp(v, n, R)
                        'Selektion des Elter
                        Xe(v, n, PES_iAkt.iAktPop) = Xp(v, n, R)
                    Next v
                Next n

            Case EVO_POP_ELTERN.Mittelwert 'Mittelwertbildung über alle Eltern
                'Ermitteln der Elter und Schrittweite über Mittelung der Elternschrittweiten
                For v = 0 To Anz.Para - 1
                    For n = 0 To PES_Settings.n_Eltern - 1
                        De(v, n, PES_iAkt.iAktPop) = 0
                        Xe(v, n, PES_iAkt.iAktPop) = 0
                        For m = 0 To PES_Settings.Pop.n_PopEltern - 1
                            'Mittelung der Schrittweite,
                            De(v, n, PES_iAkt.iAktPop) = De(v, n, PES_iAkt.iAktPop) + (Dp(v, n, m) / PES_Settings.Pop.n_PopEltern)
                            'Mittelung der Eltern,
                            Xe(v, n, PES_iAkt.iAktPop) = Xe(v, n, PES_iAkt.iAktPop) + (Xp(v, n, m) / PES_Settings.Pop.n_PopEltern)
                        Next m
                    Next n
                Next v

            Case EVO_POP_ELTERN.Selektion 'Zufallswahl über alle Eltern
                R = Int(PES_Settings.Pop.n_PopEltern * Rnd()) 'Zufallszahl entscheidet welcher
                'Elternteil vererbt wird
                For v = 0 To Anz.Para - 1
                    For n = 0 To PES_Settings.n_Eltern - 1
                        'Selektion der Schrittweite
                        De(v, n, PES_iAkt.iAktPop) = Dp(v, n, R)
                        'Selektion des Elter
                        Xe(v, n, PES_iAkt.iAktPop) = Xp(v, n, R)
                    Next n
                Next v

        End Select

    End Sub

    'ES_VARIA - REPRODUKTIONSPROZESS - Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern
    '*************************************************************************************************
    Public Sub EsReproduktion()

        Dim i, v, n, j As Integer
        Dim R As Integer                                   'Zufälliger Integer Wert
        Dim Realisierungsspeicher() As Integer
        Dim Elternspeicher() As Integer
        Dim Z1, Elter, Z2 As Integer

        Select Case PES_Settings.ty_OptEltern

            Case EVO_ELTERN.Selektion 'Zufallswahl über alle Eltern

                R = Int(PES_Settings.n_Eltern * Rnd())    'Zufallszahl entscheidet
                'welcher Enternteil vererbt wird
                For v = 0 To Anz.Para - 1
                    'Selektion der Schrittweite
                    AktPara.Dn(v) = De(v, R, PES_iAkt.iAktPop)
                    'Selektion des Elter
                    AktPara.Xn(v) = Xe(v, R, PES_iAkt.iAktPop)
                Next v

            Case EVO_ELTERN.XX_Diskret 'Multi-Rekombination, diskret

                For v = 0 To Anz.Para - 1
                    R = Int(PES_Settings.n_Eltern * Rnd())
                    'Selektion der Schrittweite
                    AktPara.Dn(v) = De(v, R, PES_iAkt.iAktPop)
                    'Selektion des Elter
                    AktPara.Xn(v) = Xe(v, R, PES_iAkt.iAktPop)
                Next v

            Case EVO_ELTERN.XX_Mitteln 'Multi-Rekombination, gemittelt

                For v = 0 To Anz.Para - 1
                    AktPara.Dn(v) = 0
                    AktPara.Xn(v) = 0

                    For n = 0 To PES_Settings.n_Eltern - 1
                        'Mittelung der Schrittweite,
                        AktPara.Dn(v) = AktPara.Dn(v) + (De(v, n, PES_iAkt.iAktPop) / PES_Settings.n_Eltern)
                        'Mittelung der Eltern,
                        AktPara.Xn(v) = AktPara.Xn(v) + (Xe(v, n, PES_iAkt.iAktPop) / PES_Settings.n_Eltern)
                    Next
                Next v

            Case EVO_ELTERN.XY_Diskret 'Multi-Rekombination nach X/Y-Schema, diskrete Vertauschung

                ReDim Realisierungsspeicher(PES_Settings.n_RekombXY)
                ReDim Elternspeicher(PES_Settings.n_Eltern)

                For i = 0 To PES_Settings.n_Eltern - 1
                    Elternspeicher(i) = i
                Next i

                For i = 0 To PES_Settings.n_RekombXY - 1
                    R = Int((PES_Settings.n_Eltern - (i)) * Rnd())
                    Realisierungsspeicher(i) = Elternspeicher(R)

                    For j = R To PES_Settings.n_Eltern - 1
                        Elternspeicher(R) = Elternspeicher(R + 1)
                    Next j

                Next i

                For v = 0 To Anz.Para - 1
                    R = Int(PES_Settings.n_RekombXY * Rnd())
                    'Selektion der Schrittweite
                    AktPara.Dn(v) = De(v, Realisierungsspeicher(R), PES_iAkt.iAktPop)
                    'Selektion des Elter
                    AktPara.Xn(v) = Xe(v, Realisierungsspeicher(R), PES_iAkt.iAktPop)
                Next v

            Case EVO_ELTERN.XY_Mitteln 'Multi-Rekombination nach X/Y-Schema, Mittelung der Gene

                ReDim Realisierungsspeicher(PES_Settings.n_RekombXY)
                ReDim Elternspeicher(PES_Settings.n_Eltern)

                For i = 0 To PES_Settings.n_Eltern - 1
                    Elternspeicher(i) = i
                Next i

                For i = 0 To PES_Settings.n_RekombXY - 1
                    R = Int((PES_Settings.n_Eltern - (i)) * Rnd())
                    Realisierungsspeicher(i) = Elternspeicher(R)
                    For j = R To (PES_Settings.n_Eltern - 2)
                        Elternspeicher(R) = Elternspeicher(R + 1)
                    Next j
                Next i

                For v = 0 To Anz.Para - 1
                    AktPara.Dn(v) = 0
                    AktPara.Xn(v) = 0
                    For n = 0 To PES_Settings.n_RekombXY - 1
                        'Mittelung der Schrittweite,
                        AktPara.Dn(v) = AktPara.Dn(v) + (De(v, Elternspeicher(n), PES_iAkt.iAktPop) / PES_Settings.n_RekombXY)
                        'Mittelung der Eltern,
                        AktPara.Xn(v) = AktPara.Xn(v) + (Xe(v, Elternspeicher(n), PES_iAkt.iAktPop) / PES_Settings.n_RekombXY)
                    Next
                Next v

            Case EVO_ELTERN.Neighbourhood 'Neighbourhood Rekombination

                Z1 = Int(PES_Settings.n_Eltern * Rnd())
                Do
                    Z2 = Int(PES_Settings.n_Eltern * Rnd())
                Loop While Z1 = Z2

                'Tournament über Crowding Distance
                If Distanceb(Z1) > Distanceb(Z2) Then
                    Elter = Z1
                Else
                    Elter = Z2
                End If

                If (Elter = 0 Or Elter = PES_Settings.n_Eltern - 1) Then
                    For v = 0 To Anz.Para - 1
                        'Selektion der Schrittweite
                        AktPara.Dn(v) = De(v, Elter, PES_iAkt.iAktPop)
                        'Selektion des Elter
                        AktPara.Xn(v) = Xe(v, Elter, PES_iAkt.iAktPop)
                    Next
                Else
                    'BUG 135
                    Dim IndexEltern(PES_Settings.n_Eltern - 1) As Integer          'Array mit Index der Eltern (Neighbourhood-Rekomb.)
                    Call Neighbourhood_Eltern(Elter, IndexEltern)
                    For v = 0 To Anz.Para - 1
                        'Do
                        '    Faktor = Rnd
                        '    Faktor = (-1) * Eigenschaft.d + Faktor * (1 + Eigenschaft.d)
                        '    'Selektion der Schrittweite
                        '    Eigenschaft.Dn(v) = De(v, IndexEltern(1), Eigenschaft.iaktuellePopulation) * Faktor + _
                        '                     De(v, IndexEltern(2), Eigenschaft.iaktuellePopulation) * (1 - Faktor)
                        '    Eigenschaft.Xn(v) = Xe(v, IndexEltern(1), Eigenschaft.iaktuellePopulation) * Faktor + _
                        '                     Xe(v, IndexEltern(2), Eigenschaft.iaktuellePopulation) * (1 - Faktor)
                        'Loop While (Eigenschaft.Xn(v) <= Eigenschaft.Xmin(v) Or Eigenschaft.Xn(v) > Eigenschaft.Xmax(v))

                        R = Int(PES_Settings.n_RekombXY * Rnd())
                        'Selektion der Schrittweite
                        AktPara.Dn(v) = De(v, IndexEltern(R), PES_iAkt.iAktPop)
                        'Selektion des Elter
                        AktPara.Xn(v) = Xe(v, IndexEltern(R), PES_iAkt.iAktPop)
                    Next
                End If
        End Select

    End Sub

    'ES_POP_MUTATION - ToDo: Beschreibung fehlt
    '******************************************
    Public Sub EsPopMutation()

        Dim i, v, n As Integer
        Dim DeTemp(,,) As Double      'Temporäre Schrittweiten für Eltern
        Dim XeTemp(,,) As Double      'Temporäre Parameterwerte für Eltern
        Dim expo As Integer             'Exponent für Schrittweite (+/-1)

        ReDim DeTemp(Anz.Para - 1, PES_Settings.n_Eltern - 1, PES_Settings.Pop.n_Popul - 1)
        ReDim XeTemp(Anz.Para - 1, PES_Settings.n_Eltern - 1, PES_Settings.Pop.n_Popul - 1)

StartMutation:

        'Einheitliche Schrittweite
        '-------------------------
        If (Not PES_Settings.is_DnVektor) Then
            '+/-1
            expo = (2 * Int(Rnd() + 0.5) - 1)
            'Schrittweite wird mutiert
            DeTemp(0, 0, PES_iAkt.iAktPop) = De(0, 0, PES_iAkt.iAktPop) * palpha ^ expo
            'Schrittweite für alle übernehmen
            For n = 0 To PES_Settings.n_Eltern - 1
                For v = 0 To Anz.Para - 1
                    DeTemp(v, n, PES_iAkt.iAktPop) = DeTemp(0, 0, PES_iAkt.iAktPop)
                Next
            Next
        End If


        'Mutation
        '--------
        For n = 0 To PES_Settings.n_Eltern - 1
            For v = 0 To Anz.Para - 1
                i = 0
                Do
                    i += 1
                    'Abbruchkriterium
                    '----------------
                    If (i >= 1000) Then
                        'Es konnte kein gültiger Parametersatz generiert werden!
                        'Wieder von vorne anfangen
                        GoTo StartMutation
                    End If

                    'Schrittweitenvektor
                    '-------------------
                    If (PES_Settings.is_DnVektor) Then
                        '+/-1
                        expo = (2 * Int(Rnd() + 0.5) - 1)
                        'Schrittweite wird mutiert
                        DeTemp(v, n, PES_iAkt.iAktPop) = De(v, n, PES_iAkt.iAktPop) * palpha ^ expo
                    End If

                    'Normalverteilte Zufallszahl mit Standardabweichung 1/sqr(varanz)
                    Dim Z As Double
                    Z = System.Math.Sqrt(-2 * System.Math.Log(1 - Rnd()) / Anz.Para) * System.Math.Sin(6.2832 * Rnd())
                    'Mutation wird durchgeführt
                    XeTemp(v, n, PES_iAkt.iAktPop) = Xe(v, n, PES_iAkt.iAktPop) + DeTemp(v, n, PES_iAkt.iAktPop) * Z

                    ' Restriktion für die mutierten Werte
                Loop While (XeTemp(v, n, PES_iAkt.iAktPop) <= 0 Or XeTemp(v, n, PES_iAkt.iAktPop) > 1 Or Not checkBeziehungPop(v, n, XeTemp))

            Next v

            'Mutierte Werte übernehmen
            '-------------------------
            For v = 0 To Anz.Para - 1
                De(v, n, PES_iAkt.iAktPop) = DeTemp(v, n, PES_iAkt.iAktPop)
                Xe(v, n, PES_iAkt.iAktPop) = XeTemp(v, n, PES_iAkt.iAktPop)
            Next v

        Next n

    End Sub

    'ES_MUTATION - Mutieren der Ausgangswerte
    '****************************************
    Public Sub EsMutation()

        Dim v, i As Integer
        Dim DnTemp() As Double             'Temporäre Schrittweiten für Nachkomme
        Dim XnTemp() As Double             'Temporäre Parameterwerte für Nachkomme
        Dim expo As Integer                  'Exponent für Schrittweite (+/-1)

        ReDim DnTemp(Anz.Para - 1)
        ReDim XnTemp(Anz.Para - 1)

StartMutation:

        'Einheitliche Schrittweite
        '-------------------------
        If (Not PES_Settings.is_DnVektor) Then
            '+/-1
            expo = (2 * Int(Rnd() + 0.5) - 1)
            'Schrittweite wird mutiert
            DnTemp(0) = AktPara.Dn(0) * galpha ^ expo
            'Schrittweite für alle übernehmen
            For v = 1 To Anz.Para - 1
                DnTemp(v) = DnTemp(0)
            Next
        End If

        'Mutation
        '--------
        For v = 0 To Anz.Para - 1
            i = 0
            Do
                i += 1
                'Abbruchkriterium
                '----------------
                If (i >= 1000) Then
                    'Es konnte kein gültiger Parametersatz generiert werden!
                    'Wieder von vorne anfangen
                    GoTo StartMutation
                End If

                'Schrittweitenvektor
                '-------------------
                If (PES_Settings.is_DnVektor) Then
                    '+/-1
                    expo = (2 * Int(Rnd() + 0.5) - 1)
                    'Schrittweite wird mutiert
                    DnTemp(v) = AktPara.Dn(v) * galpha ^ expo
                End If

                'Normalverteilte Zufallszahl mit Standardabweichung 1/sqr(varanz)
                Dim Z As Double
                Z = System.Math.Sqrt(-2 * System.Math.Log(1 - Rnd()) / Anz.Para) * System.Math.Sin(6.2832 * Rnd())
                'Mutation wird durchgeführt
                XnTemp(v) = AktPara.Xn(v) + DnTemp(v) * Z

                'Restriktion für die mutierten Werte
            Loop While (XnTemp(v) <= 0 Or XnTemp(v) > 1 Or Not checkBeziehung(v, XnTemp))

        Next v

        'Mutierte Werte übernehmen
        '-------------------------
        For v = 0 To Anz.Para - 1
            AktPara.Dn(v) = DnTemp(v)
            AktPara.Xn(v) = XnTemp(v)
        Next v

    End Sub

    'ES_POP_BEST - Einordnen der Qualitätsfunktion im PopulationsBestwertspeicher
    '****************************************************************************
    Public Sub EsPopBest()

        Dim m, i, j, n As Integer
        Dim h1, h2 As Double

        'Der schlechtetste der besten Qualitätswerte wird bestimmt ; Position -> i
        '(höchster Wert der Penaltyfunktion, niedrigster Wert der Crowding Distance)
        i = 0
        h1 = Qbpop(0, 0)
        For m = 1 To PES_Settings.Pop.n_Popul - 1
            If Not PES_Settings.is_MO_Pareto Then
                If Qbpop(m, 0) > h1 Then
                    h1 = Qbpop(m, 0)
                    i = m
                End If
            Else
                Select Case PES_Settings.Pop.ty_PopPenalty

                    Case EVO_POP_PENALTY.Crowding
                        If Qbpop(m, 0) > h1 Then
                            h1 = Qbpop(m, 0)
                            i = m
                        End If

                    Case EVO_POP_PENALTY.Spannweite
                        If Qbpop(m, 0) < h1 Then
                            h2 = Qbpop(m, 1)
                            i = m
                        End If
                End Select
            End If
        Next m

        'Der schlechtetste der besten Qualitätswerte wird bestimmt ; Position -> i
        '(höchster Wert der Kostenfunktion, niedrigster Wert der Spannweite)
        If PES_Settings.is_MO_Pareto Then
            j = 0
            h2 = Qbpop(0, 1)
            For m = 2 To PES_Settings.Pop.n_Popul
                If Qbpop(m, 1) < h2 Then
                    h2 = Qbpop(m, 1)
                    j = m
                End If
            Next m
        End If

        'Qualität der aktuellen Population wird bestimmt
        h1 = 0
        If Not PES_Settings.is_MO_Pareto Then
            For m = 0 To PES_Settings.n_Eltern - 1
                h1 = h1 + Best.Qb(m, PES_iAkt.iAktPop, 0) / PES_Settings.n_Eltern
            Next m
        Else
            h2 = 0
            h1 = NDS_Crowding_Distance_Count(h2)
        End If

        'Falls die Qualität des aktuellen Population besser ist (Penaltyfunktion geringer)
        'als die schlechteste im Bestwertspeicher, wird diese ersetzt
        If Not PES_Settings.is_MO_Pareto Then
            If h1 < Qbpop(i, 0) Then
                Qbpop(i, 0) = h1
                For m = 0 To Anz.Para - 1
                    For n = 0 To PES_Settings.n_Eltern - 1
                        'Die Schrittweite wird ebenfalls übernommen
                        Dbpop(m, n, i) = Best.Db(m, n, PES_iAkt.iAktPop)
                        'Die eigentlichen Parameterwerte werden übernommen
                        Xbpop(m, n, i) = Best.Xb(m, n, PES_iAkt.iAktPop)
                    Next n
                Next m
            End If
        Else
            Select Case PES_Settings.Pop.ty_PopPenalty

                Case EVO_POP_PENALTY.Crowding
                    If h1 < Qbpop(i, 0) Then
                        Qbpop(i, 0) = h1
                        For m = 0 To Anz.Para - 1
                            For n = 0 To PES_Settings.n_Eltern - 1
                                'Die Schrittweite wird ebenfalls übernommen
                                Dbpop(m, n, i) = Best.Db(m, n, PES_iAkt.iAktPop)
                                'Die eigentlichen Parameterwerte werden übernommen
                                Xbpop(m, n, i) = Best.Xb(m, n, PES_iAkt.iAktPop)
                            Next n
                        Next m
                    End If

                Case EVO_POP_PENALTY.Spannweite
                    If h2 > Qbpop(j, 1) Then
                        Qbpop(j, 1) = h2
                        For m = 0 To Anz.Para - 1
                            For n = 0 To PES_Settings.n_Eltern - 1
                                'Die Schrittweite wird ebenfalls übernommen
                                Dbpop(m, n, j) = Best.Db(m, n, PES_iAkt.iAktPop)
                                'Die eigentlichen Parameterwerte werden übernommen
                                Xbpop(m, n, j) = Best.Xb(m, n, PES_iAkt.iAktPop)
                            Next n
                        Next m
                    End If
            End Select
        End If

    End Sub

    'ES_BEST - Einordnen der Qualitätsfunktion im Bestwertspeicher
    '*************************************************************
    Public Sub EsBest(ByVal QN() As Double, ByVal RN() As Double)

        Dim m, i, j, v As Integer
        Dim h As Double

        If (Not PES_Settings.is_MO_Pareto) Then
            'SO - Standard ES nach Rechenberg
            '--------------------------------
            'Der schlechteste der besten Qualitätswerte wird bestimmt ; Position -> j
            '(höchster Wert der Penaltyfunktion)
            j = 0
            h = Best.Qb(0, PES_iAkt.iAktPop, 0)

            For m = 1 To PES_Settings.n_Eltern - 1
                If Best.Qb(m, PES_iAkt.iAktPop, 0) > h Then
                    h = Best.Qb(m, PES_iAkt.iAktPop, 0)
                    j = m
                End If
            Next m

            'Falls die Qualität des aktuellen Nachkommen besser ist (Penaltyfunktion geringer)
            'als die schlechteste im Bestwertspeicher, wird dieser ersetzt
            If QN(0) < Best.Qb(j, PES_iAkt.iAktPop, 0) Then
                Best.Qb(j, PES_iAkt.iAktPop, 0) = QN(0)
                For v = 0 To Anz.Para - 1
                    'Die Schrittweite wird ebenfalls übernommen
                    Best.Db(v, j, PES_iAkt.iAktPop) = AktPara.Dn(v)
                    'Die eigentlichen Parameterwerte werden übernommen
                    Best.Xb(v, j, PES_iAkt.iAktPop) = AktPara.Xn(v)
                Next v
            End If

        Else
            'Multi-Objective Pareto
            '----------------------
            With NDSorting(PES_iAkt.iAktNachf)
                For i = 0 To Anz.Penalty - 1
                    .Penalty(i) = QN(i)
                Next i
                .feasible = True
                For i = 0 To Anz.Constr - 1
                    .Constrain(i) = RN(i)
                    If .Constrain(i) < 0 Then .feasible = False
                Next i
                .dominated = False
                .Front = 0
                For v = 0 To Anz.Para - 1
                    .PES_d(v) = AktPara.Dn(v)
                    .PES_X(v) = AktPara.Xn(v)
                Next v
                .Distance = 0
            End With
        End If

    End Sub

    'ES_BESTWERTSPEICHER
    'Führt einen Reset des Bestwertspeicher durch, falls eine Komma-Strategie gewählt ist
    '************************************************************************************
    Public Sub EsResetBWSpeicher()
        Dim n, i As Integer

        If (PES_Settings.ty_EvoTyp = EVO_STRATEGIE.Komma) Then
            For n = 0 To PES_Settings.n_Eltern - 1
                For i = 0 To Anz.Penalty - 1
                    Best.Qb(n, PES_iAkt.iAktPop, i) = 1.0E+300
                Next i
            Next n
        End If

    End Sub

    'ES_POP_BESTWERTSPEICHER
    'Führt einen Reset des Bestwertspeicher auf Populationsebene durch, falls eine Komma-Strategie gewählt ist
    '*********************************************************************************************************
    Public Sub EsResetPopBWSpeicher()
        Dim n, i As Integer

        If (PES_Settings.Pop.ty_PopEvoTyp = EVO_STRATEGIE.Komma) Then
            For n = 0 To PES_Settings.Pop.n_Popul - 1
                For i = 0 To Anz.Penalty - 1
                    Qbpop(n, i) = 1.0E+300
                Next i
            Next n
        End If

    End Sub

    'ES_POP_ELTERN - Eltern Population
    '*********************************
    Public Sub EsPopEltern()

        Dim n, m, v As Integer
        Dim swap(1) As Double
        Dim Realisierungsspeicher(,) As Double
        Dim Z As Integer

        Select Case PES_Settings.Pop.ty_PopPenalty
            Case EVO_POP_PENALTY.Crowding
                Z = 0
            Case EVO_POP_PENALTY.Spannweite
                Z = 1
        End Select

        ReDim Realisierungsspeicher(PES_Settings.Pop.n_Popul - 1, 1)

        'Die NPopEltern besten Individium-Sätze werden ermittelt
        For m = 0 To PES_Settings.Pop.n_Popul - 1
            Realisierungsspeicher(m, 0) = Qbpop(m, Z)
            Realisierungsspeicher(m, 1) = m
        Next m

        If (Not PES_Settings.is_MO_Pareto) Then
            'Standard ES nach Rechenberg
            '---------------------------
            For m = 0 To PES_Settings.Pop.n_Popul - 1
                For n = m To PES_Settings.Pop.n_Popul - 1
                    If Realisierungsspeicher(m, 0) > Realisierungsspeicher(n, 0) Then
                        swap(0) = Realisierungsspeicher(m, 0)
                        swap(1) = Realisierungsspeicher(m, 1)
                        Realisierungsspeicher(m, 0) = Realisierungsspeicher(n, 0)
                        Realisierungsspeicher(m, 1) = Realisierungsspeicher(n, 1)
                        Realisierungsspeicher(n, 0) = swap(0)
                        Realisierungsspeicher(n, 1) = swap(1)
                    End If
                Next
            Next

        Else
            'Multi-Objective mit Paretofront
            '-------------------------------
            Select Case PES_Settings.Pop.ty_PopPenalty

                Case EVO_POP_PENALTY.Crowding
                    For m = 0 To PES_Settings.Pop.n_Popul - 1
                        For n = m To PES_Settings.Pop.n_Popul - 1
                            If Realisierungsspeicher(m, 0) > Realisierungsspeicher(n, 0) Then
                                swap(0) = Realisierungsspeicher(m, 0)
                                swap(1) = Realisierungsspeicher(m, 1)
                                Realisierungsspeicher(m, 0) = Realisierungsspeicher(n, 0)
                                Realisierungsspeicher(m, 1) = Realisierungsspeicher(n, 1)
                                Realisierungsspeicher(n, 0) = swap(0)
                                Realisierungsspeicher(n, 1) = swap(1)
                            End If
                        Next
                    Next

                Case EVO_POP_PENALTY.Spannweite
                    For m = 0 To PES_Settings.Pop.n_Popul - 1
                        For n = m To PES_Settings.Pop.n_Popul - 1
                            If Realisierungsspeicher(m, 0) < Realisierungsspeicher(n, 0) Then
                                swap(0) = Realisierungsspeicher(m, 0)
                                swap(1) = Realisierungsspeicher(m, 1)
                                Realisierungsspeicher(m, 0) = Realisierungsspeicher(n, 0)
                                Realisierungsspeicher(m, 1) = Realisierungsspeicher(n, 1)
                                Realisierungsspeicher(n, 0) = swap(0)
                                Realisierungsspeicher(n, 1) = swap(1)
                            End If
                        Next
                    Next
            End Select
        End If

        'Die Eltern werden gleich der besten Kinder gesetzt (Schrittweite und Parameterwert)
        For m = 0 To PES_Settings.Pop.n_PopEltern - 1
            For n = 0 To PES_Settings.n_Eltern - 1
                For v = 0 To Anz.Para - 1
                    Dp(v, n, m) = Dbpop(v, n, Int(Realisierungsspeicher(m, 1)))
                    Xp(v, n, m) = Xbpop(v, n, Int(Realisierungsspeicher(m, 1)))
                Next v
            Next n
        Next m

    End Sub

    'ES_ELTERN - Die neuen Eltern werden generiert
    '*********************************************
    Public Sub EsEltern()
        Dim i, v As Integer


        If (Not PES_Settings.is_MO_Pareto) Then
            'Standard ES nach Rechenberg
            'xxxxxxxxxxxxxxxxxxxxxxxxxxx
            'Die Eltern werden gleich der besten Kinder gesetzt (Schrittweite und Parameterwert)
            '---------------------------------------------------------------------
            For i = 0 To PES_Settings.n_Eltern - 1
                For v = 0 To Anz.Para - 1
                    De(v, i, PES_iAkt.iAktPop) = Best.Db(v, i, PES_iAkt.iAktPop)
                    Xe(v, i, PES_iAkt.iAktPop) = Best.Xb(v, i, PES_iAkt.iAktPop)
                Next v
            Next i

        Else
            'Multi-Objective Pareto
            'xxxxxxxxxxxxxxxxxxxxxx
            '1. Eltern und Nachfolger werden gemeinsam betrachtet
            'Nur Eltern werden NDSorting hinzugefügt, Kinder sind schon oben drin
            '--------------------------------------------------------------------
            For i = PES_Settings.n_Nachf To PES_Settings.n_Nachf + PES_Settings.n_Eltern - 1
                Call Copy_Bestwert_to_Individuum(i, i - PES_Settings.n_Nachf, NDSorting)
            Next i

            '********************* Alles in der Klasse Functions ****************************************
            'Zu Beginn den Bestwertspeicher in ein Individuum packen
            'Dimensionieren des Best_Indi
            Dim Best_Indi(Best.Qb.GetUpperBound(0)) As Individuum
            Individuum.New_Array("Bestwerte", Best_Indi)
            For i = 0 To Best_Indi.GetUpperBound(0)
                ReDim Best_Indi(i).PES_d(Anz.Para - 1)
                ReDim Best_Indi(i).PES_X(Anz.Para - 1)
            Next i
            'Kopieren in Best_Indi
            For i = 0 To Best.Qb.GetUpperBound(0)
                Call Copy_Bestwert_to_Individuum(i, i, Best_Indi)
            Next
            '---------------------------------
            '2. Die einzelnen Fronten werden bestimmt
            '3. Der Bestwertspeicher wird entsprechend der Fronten oder der sekundären Population gefüllt
            '4: Sekundäre Population wird bestimmt und gespeichert
            '--------------------------------
            Dim Func1 As Kern.Functions = New Kern.Functions(PES_Settings.n_Nachf, PES_Settings.n_Eltern, PES_Settings.n_MemberSekPop, PES_Settings.n_Interact, PES_Settings.is_Interact, Anz.Penalty, Anz.Constr, PES_iAkt.iAktGen)
            Call Func1.EsEltern_Pareto_SekundärQb(Best_Indi, NDSorting, SekundärQb)
            'Am ende die Bestwerte wieder zurück
            For i = 0 To Best.Qb.GetUpperBound(0)
                Call Copy_Individuum_to_Bestwert(i, Best_Indi)
            Next
            '********************************************************************************************

            '5: Neue Eltern werden gleich dem Bestwertspeicher gesetzt
            '---------------------------------------------------------
            For i = 0 To PES_Settings.n_Eltern - 1
                For v = 0 To Anz.Para - 1
                    De(v, i, PES_iAkt.iAktPop) = Best.Db(v, i, PES_iAkt.iAktPop)
                    Xe(v, i, PES_iAkt.iAktPop) = Best.Xb(v, i, PES_iAkt.iAktPop)
                Next v
            Next i

            '6: Sortierung der Lösungen ist nur für Neighbourhood-Rekombination notwendig
            '----------------------------------------------------------------------------
            If (PES_Settings.ty_OptEltern = EVO_ELTERN.Neighbourhood) Then
                Call Neighbourhood_AbstandsArray()
                Call Neighbourhood_Crowding_Distance()
            End If

        End If

    End Sub

    'Kopiert ein Individuum in den Bestwertspeicher
    '----------------------------------------------
    Public Sub Copy_Individuum_to_Bestwert(ByVal i As Integer, ByVal Individ As Individuum())
        Dim j, v As Integer

        For j = 0 To Anz.Penalty - 1
            Best.Qb(i, PES_iAkt.iAktPop, j) = Individ(i).Penalty(j)
        Next j

        If Anz.Constr > 0 Then
            For j = 0 To Anz.Constr - 1
                Best.Rb(i, PES_iAkt.iAktPop, j) = Individ(i).Constrain(j)
            Next j
        End If

        For v = 0 To Anz.Para - 1
            Best.Db(v, i, PES_iAkt.iAktPop) = Individ(i).PES_d(v)
            Best.Xb(v, i, PES_iAkt.iAktPop) = Individ(i).PES_X(v)
        Next v

    End Sub

    'Kopiert den Bestwertspeicher in ein Individuum
    '----------------------------------------------
    Public Sub Copy_Bestwert_to_Individuum(ByVal i_indi As Integer, ByVal i_best As Integer, ByRef Individ As Individuum())
        Dim j, v As Integer

        For j = 0 To Anz.Penalty - 1
            Individ(i_indi).Penalty(j) = Best.Qb(i_best, PES_iAkt.iAktPop, j)
        Next j

        If Anz.Constr > 0 Then
            Individ(i_indi).feasible = True
            For j = 0 To Anz.Constr - 1
                Individ(i_indi).Constrain(j) = Best.Rb(i_best, PES_iAkt.iAktPop, j)
                If Individ(i_indi).Constrain(j) < 0 Then Individ(i_indi).feasible = False
            Next j
        End If

        For v = 0 To Anz.Para - 1
            Individ(i_indi).PES_d(v) = Best.Db(v, i_best, PES_iAkt.iAktPop)
            Individ(i_indi).PES_X(v) = Best.Xb(v, i_best, PES_iAkt.iAktPop)
        Next v

        Individ(i_indi).dominated = False
        Individ(i_indi).Front = 0
        Individ(i_indi).Distance = 0

    End Sub

    'ES_GET_SEKUNDÄRE_POPULATIONEN - Sekundäre Population speichert immer die angegebene
    'Anzahl von Bestwerten und kann den Bestwertspeicher alle x Generationen überschreiben
    '*************************************************************************************
    Public Function SekundärQb_Get() As Double(,)

        Dim j, i As Integer
        Dim SekPopulation(,) As Double

        ReDim SekPopulation(SekundärQb.GetUpperBound(0), SekundärQb(0).Penalty.GetUpperBound(0))

        For i = 0 To SekundärQb.GetUpperBound(0)
            For j = 0 To SekundärQb(0).Penalty.GetUpperBound(0)
                SekPopulation(i, j) = SekundärQb(i).Penalty(j)
            Next j
        Next i

        Return SekPopulation

    End Function

    'NDS_Crowding_Distance_Count
    '***************************
    Private Function NDS_Crowding_Distance_Count(ByRef Spannweite As Double) As Double

        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim TempDistance() As Double
        Dim PenaltyDistance(,) As Double

        Dim d() As Double
        Dim d_mean As Double

        ReDim TempDistance(Anz.Penalty - 1)
        ReDim PenaltyDistance(PES_Settings.n_Eltern - 1, PES_Settings.n_Eltern - 1)
        ReDim d(PES_Settings.n_Eltern - 1)

        'Bestimmen der normierten Raumabstände zwischen allen Elternindividuen
        For i = 0 To PES_Settings.n_Eltern - 1
            PenaltyDistance(i, i) = 0
            For j = i + 1 To PES_Settings.n_Eltern - 1
                PenaltyDistance(i, j) = 0
                For k = 0 To Anz.Penalty - 1
                    TempDistance(k) = Best.Qb(i, PES_iAkt.iAktPop, k) - Best.Qb(j, PES_iAkt.iAktPop, k)
                    TempDistance(k) = TempDistance(k) * TempDistance(k)
                    PenaltyDistance(i, j) += TempDistance(k)
                Next k
                PenaltyDistance(i, j) = System.Math.Sqrt(PenaltyDistance(i, j))
                PenaltyDistance(j, i) = PenaltyDistance(i, j)
            Next j
        Next i

        d_mean = 0

        For i = 0 To PES_Settings.n_Eltern - 2
            d(i) = 1.0E+300
            For j = 0 To i - 1
                If (PenaltyDistance(i, j) < d(i)) Then d(i) = PenaltyDistance(i, j)
            Next j
            For j = i + 1 To PES_Settings.n_Eltern - 1
                If (PenaltyDistance(i, j) < d(i)) Then d(i) = PenaltyDistance(i, j)
            Next j
            d_mean += d(i)
        Next i

        d_mean = d_mean / PES_Settings.n_Eltern
        NDS_Crowding_Distance_Count = 0

        For i = 0 To PES_Settings.n_Eltern - 2
            NDS_Crowding_Distance_Count += (d_mean - d(i)) * (d_mean - d(i))
        Next i

        NDS_Crowding_Distance_Count = NDS_Crowding_Distance_Count / PES_Settings.n_Eltern
        NDS_Crowding_Distance_Count = System.Math.Sqrt(NDS_Crowding_Distance_Count)

        Spannweite = 0
        For i = 0 To PES_Settings.n_Eltern - 1
            'TODO: sollte hier nicht j = i + 1 stehen?
            For j = i To PES_Settings.n_Eltern - 1
                If PenaltyDistance(i, j) > Spannweite Then Spannweite = PenaltyDistance(i, j)
            Next j
        Next i

    End Function

    'Neighbourhood_AbstandsArray - Bestimme Array der Raumabstände für Neighbourhood-Rekombination
    '*********************************************************************************************
    Private Sub Neighbourhood_AbstandsArray()

        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim MinMax() As Double
        Dim Min, Max As Double
        Dim TempDistance() As Double

        'Bestimmen des Normierungsfaktors für jede Dimension des Lösungsraums (MinMax)
        ReDim MinMax(Anz.Penalty - 1)
        For k = 0 To Anz.Penalty - 1
            MinMax(k) = 0
            Min = Best.Qb(0, PES_iAkt.iAktPop, k)
            Max = Best.Qb(0, PES_iAkt.iAktPop, k)
            For j = 0 To PES_Settings.n_Eltern - 1
                If (Min > Best.Qb(j, PES_iAkt.iAktPop, k)) Then Min = Best.Qb(j, PES_iAkt.iAktPop, k)
                If (Max < Best.Qb(j, PES_iAkt.iAktPop, k)) Then Max = Best.Qb(j, PES_iAkt.iAktPop, k)
            Next j
            MinMax(k) = Max - Min
        Next k

        'Bestimmen der normierten Raumabstände zwischen allen Elternindividuen
        ReDim TempDistance(Anz.Penalty)

        For i = 0 To PES_Settings.n_Eltern - 1
            PenaltyDistance(i, i) = 0
            For j = i + 1 To PES_Settings.n_Eltern - 1
                PenaltyDistance(i, j) = 0
                For k = 0 To Anz.Penalty - 1
                    TempDistance(k) = Best.Qb(i, PES_iAkt.iAktPop, k) - Best.Qb(j, PES_iAkt.iAktPop, k)
                    TempDistance(k) = TempDistance(k) '/ MinMax(k)
                    TempDistance(k) = TempDistance(k) * TempDistance(k)
                    PenaltyDistance(i, j) = PenaltyDistance(i, j) + TempDistance(k)
                Next k
                PenaltyDistance(i, j) = System.Math.Sqrt(PenaltyDistance(i, j))
                'Die obere Diagonale wird eigentlich nicht benötigt - dient nur der Sicherheit, falls Indizes vertauscht werden!!!
                PenaltyDistance(j, i) = PenaltyDistance(i, j)
            Next j
        Next i

    End Sub

    'Neighbourhood_Eltern
    'Bestimme die NAnzahlEltern mit geringsten Raumabständen für Neighbourhood-Rekombination
    '***************************************************************************************
    Private Sub Neighbourhood_Eltern(ByVal IndexElter As Integer, ByRef IndexEltern() As Integer)

        Dim i As Integer
        Dim j As Integer
        Dim Nachbarn() As Struct_Neighbourhood
        Dim swap As Struct_Neighbourhood

        'BUG 135 ganze Funktion
        ReDim Nachbarn(PES_Settings.n_Eltern - 2)

        For i = 0 To IndexElter - 2
            Nachbarn(i).distance = PenaltyDistance(IndexElter, i)
            Nachbarn(i).Index = i
        Next i
        For i = IndexElter To PES_Settings.n_Eltern - 1
            Nachbarn(i - 1).distance = PenaltyDistance(IndexElter, i)
            Nachbarn(i - 1).Index = i
        Next i

        For i = 0 To Nachbarn.GetUpperBound(0)
            For j = i To Nachbarn.GetUpperBound(0)
                If (Nachbarn(i).distance > Nachbarn(j).distance) Then
                    swap = Nachbarn(i)
                    Nachbarn(i) = Nachbarn(j)
                    Nachbarn(j) = swap
                End If
            Next
        Next

        For i = 0 To PES_Settings.n_RekombXY - 1
            IndexEltern(i) = Nachbarn(i).Index
        Next i

    End Sub

    'Neighbourhood_Crowding_Distance
    'Bestimme die NAnzahlEltern mit geringsten Raumabständen für Neighbourhood-Rekombination
    '***************************************************************************************
    Private Sub Neighbourhood_Crowding_Distance()

        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim QbTemp(,,) As Double
        Dim swap As Double
        Dim fmin, fmax As Double

        ReDim QbTemp(PES_Settings.n_Eltern - 1, PES_Settings.Pop.n_Popul - 1, Anz.Penalty - 1)

        Array.Copy(Best.Qb, QbTemp, Best.Qb.GetLength(0))
        For i = 0 To PES_Settings.n_Eltern - 1
            Distanceb(i) = 0
        Next i

        For k = 0 To Anz.Penalty - 1
            For i = 0 To PES_Settings.n_Eltern - 1
                For j = 0 To PES_Settings.n_Eltern - 1
                    If (QbTemp(i, PES_iAkt.iAktPop, k) < QbTemp(j, PES_iAkt.iAktPop, k)) Then
                        swap = QbTemp(i, PES_iAkt.iAktPop, k)
                        QbTemp(i, PES_iAkt.iAktPop, k) = QbTemp(j, PES_iAkt.iAktPop, k)
                        QbTemp(j, PES_iAkt.iAktPop, k) = swap
                    End If
                Next j
            Next i

            fmin = QbTemp(0, PES_iAkt.iAktPop, k)
            fmax = QbTemp(PES_Settings.n_Eltern - 1, PES_iAkt.iAktPop, k)

            Distanceb(0) = 1.0E+300
            Distanceb(PES_Settings.n_Eltern - 1) = 1.0E+300

            For i = 1 To PES_Settings.n_Eltern - 2
                Distanceb(i) = Distanceb(i) + (QbTemp(i + 1, PES_iAkt.iAktPop, k) - QbTemp(i - 1, PES_iAkt.iAktPop, k)) / (fmax - fmin)
            Next i
        Next k

    End Sub

    'Einen Parameterwert auf Einhaltung der Beziehung überprüfen
    '***********************************************************
    Private Function checkBeziehung(ByVal ipara As Integer, ByVal XnTemp() As Double) As Boolean

        'ipara ist der Index des zu überprüfenden Parameters,
        'XnTemp() die aktuellen Werte

        Dim isOK As Boolean = False
        If (AktPara.Bez(ipara) = Beziehung.keine) Then
            'Keine Beziehung vorhanden
            isOK = True
        Else
            'Referenzierten Parameterwert vergleichen
            Dim wert As Double = XnTemp(ipara)
            Dim ref As Double = XnTemp(ipara - 1)
            Select Case AktPara.Bez(ipara)
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

        Return isOK

    End Function

    'Einen Parameterwert auf Einhaltung der Beziehung überprüfen (Populationsebene)
    '******************************************************************************
    Private Function checkBeziehungPop(ByVal ipara As Integer, ByVal iElter As Integer, ByVal XeTemp(,,) As Double) As Boolean

        'ipara ist der Index des zu überprüfenden Parameters,
        'iElter der Index des Elters,
        'XeTemp die aktuellen Werte

        Dim isOK As Boolean = False
        If (AktPara.Bez(ipara) = Beziehung.keine) Then
            'Keine Beziehung vorhanden
            isOK = True
        Else
            'Referenzierten Parameterwert vergleichen
            Dim wert As Double = XeTemp(ipara, iElter, PES_iAkt.iAktPop)
            Dim ref As Double = XeTemp(ipara - 1, iElter, PES_iAkt.iAktPop)
            Select Case AktPara.Bez(ipara)
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

        Return isOK

    End Function

End Class
