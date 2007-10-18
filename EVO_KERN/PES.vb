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

    'Deklarationsteil
    '################

    'Structure zum Speichern aller Einstellungen aus dem Form
    Public Structure Struct_Settings
        Dim NEltern As Short                'Anzahl Eltern
        Dim NNachf As Short                 'Anzahl Kinder
        Dim NGen As Short                   'Anzahl Generationen
        Dim iEvoTyp As EVO_STRATEGIE        'Typ der Evolutionsstrategie (+ oder ,)
        Dim iPopEvoTyp As EVO_STRATEGIE     'Typ der Evolutionsstrategie (+ oder ,) auf Populationsebene
        Dim iPopPenalty As EVO_POP_PENALTY  'Art der Beurteilung der Populationsgüte (Multiobjective)
        Dim isPOPUL As Boolean              'Mit Populationen
        Dim is_MO_Pareto As Boolean         'Multi-Objective mit Pareto Front
        Dim NRunden As Short                'Anzahl Runden
        Dim NPopul As Short                 'Anzahl Populationen
        Dim NPopEltern As Short             'Anzahl Populationseltern
        Dim iOptPopEltern As EVO_POP_ELTERN 'Ermittlung der Populationseltern
        Dim iOptEltern As EVO_ELTERN        'Ermittlung der Individuum-Eltern
        Dim NRekombXY As Short              'X/Y-Schema Rekombination
        Dim DnStart As Single               'Startschrittweite
        Dim iStartPar As EVO_STARTPARAMETER 'Startparameter
        Dim isDnVektor As Boolean           'Soll ein Schrittweitenvektor benutzt werden
        Dim interact As Short               'Alle wieviel Generationen soll die aktuelle Population mit Mitgliedern der sekundären Population aufgefüllt werden
        Dim isInteract As Boolean           'Mit Austausch zwischen Population und Sekundärer Population
        Dim NMemberSecondPop As Short       'Maximale Anzahl Mitglieder der Sekundärpopulation
    End Structure

    Public PES_Settings As Struct_Settings

    'Structure zum Speichern der Werte die aus den OptDateien generiert werden
    Private Structure Struct_Initial
        Dim varanz As Short                 'Anzahl Parameter
        Dim NPenalty As Short               'Anzahl der Penaltyfunktionen
        Dim NConstrains As Short            'Anzahl der Randbedingungen
        Dim Dn() As Double                  'Schrittweitenvektor (Dimension varanz)
        Dim Xn() As Double                  'aktuelle Variablenwerte (Dimension varanz)
        Dim Xmin() As Double                'untere Schranke (Dimension varanz)
        Dim Xmax() As Double                'Obere Schranke (Dimension varanz)
    End Structure

    Private Initial As Struct_Initial

    'Diese Struktur speichert den aktuellen Zustand
    'ToDo: Könnte man auch entfernen wenn man die Schleifenkontrolle ins Form legt
    Public Structure Struct_iAkt
        Dim iAktRunde As Short              'Zähler für aktuelle Runde
        Dim iAktPop As Short                'Zähler für aktuelle Population
        Dim iAktGen As Short                'Zähler für aktuelle Generation
        Dim iAktNachf As Short              'Zähler für aktuellen Nachfahre
    End Structure

    'Muss Public sein, da das Form hiermit die Schleifen kontrolliert
    Public PES_iAkt As Struct_iAkt

    Private Xp(,,) As Double                'PopulationsElternwert der Variable
    Private Dp(,,) As Double                'PopulationsElternschrittweite
    '----------------------
    Private Xbpop(,,) As Double             'Bestwertspeicher Variablenwerte für eine Population
    Private Dbpop(,,) As Double             'Bestwertspeicher Schrittweite für eine Population
    Private Qbpop(,) As Double              'Bestwertspeicher  für eine Population
    Private QbpopD() As Double              'Bestwertspeicher Crowdings Distance  für eine Population
    '---------------------
    Private Xe(,,) As Double                'Elternwerte der Variablen
    Private De(,,) As Double                'Elternschrittweite
    '---------------------
    Private Xb(,,) As Double                'Bestwertspeicher Variablenwerte für eine Generation
    Private Db(,,) As Double                'Bestwertspeicher Schrittweite für eine Generation
    Private Qb(,,) As Double                'Bestwertspeicher für eine Generation
    Private Rb(,,) As Double                'Restriktionen für eine Generation
    '---------------------
    Private SekundärQb() As Struct_NDSorting = {}   'Sekundäre Population
    '---------------------
    Private expo As Short                   'Exponent für Schrittweite (+/-1)
    Private DnTemp As Double                'Temporäre Schrittweite für Nachkomme
    Private XnTemp As Double                'Temporärer Parameterwert für Nachkomme
    Private DeTemp As Double                'Temporäre Schrittweite für Elter
    Private XeTemp As Double                'Temporäre Parameterwert für Elter
    Private PenaltyDistance(,) As Double    'Array für normierte Raumabstände (Neighbourhood-Rekomb.)
    Private IndexEltern() As Short          'Array mit Index der Eltern (Neighbourhood-Rekomb.)
    Private Distanceb() As Double           'Array mit Crowding-Distance (Neighbourhood-Rekomb.)

    Const galpha As Double = 1.3            'Faktor alpha=1.3 auf Generationsebene nach Rechenberg
    Const palpha As Double = 1.1            'Faktor alpha=1.1 auf Populationsebene nach Rechenberg


    'Deklarationsteil für Non-Dominated Sorting
    '******************************************
    Private Structure Struct_NDSorting
        Dim penalty() As Double             '01 Werte der Penaltyfunktion(en)
        Dim constrain() As Double           '02 Werte der Randbedingung(en)
        Dim feasible As Boolean             '03 Gültiges Ergebnis
        Dim dominated As Boolean            '04 Kennzeichnung ob dominiert
        Dim Front As Short                  '05 Nummer der Pareto Front
        Dim X() As Double                   '06 Wert der Variablen
        Dim d() As Double                   '07 Schrittweite der Variablen
        Dim distance As Double              '08 Distanzwert für Crowding distance sort

        'Überladene Methode um ein NDSorting zu Dimensionieren
        Public Sub Dimit(ByVal NPenalty As Integer, ByVal NConstrains As Integer, ByVal varanz As Integer, ByRef TMP As Struct_NDSorting)
            Dim i As Integer

            ReDim TMP.penalty(NPenalty)                      '01 Werte der Penaltyfunktion(en)
            For i = 1 To TMP.penalty.GetUpperBound(0)
                TMP.penalty(i) = 1.0E+300
            Next
            ReDim TMP.constrain(NConstrains)                 '02 Werte der Randbedingung(en)
            For i = 1 To TMP.constrain.GetUpperBound(0)
                TMP.constrain(i) = -1.0E+300
            Next
            TMP.feasible = False                             '03 Gültiges Ergebnis
            TMP.dominated = False                            '04 Kennzeichnung ob dominiert
            TMP.Front = 0                                    '05 Nummer der Pareto Front
            ReDim TMP.X(varanz)                              '06 Wert der Variablen
            For i = 1 To TMP.X.GetUpperBound(0)
                TMP.X(i) = 0
            Next
            ReDim TMP.d(varanz)                              '07 Schrittweite der Variablen
            For i = 1 To TMP.d.GetUpperBound(0)
                TMP.d(i) = 0
            Next
            TMP.distance = 0                                 '08 Distanzwert für Crowding distance sort
        End Sub

        'Überladene Methode um ein Array aus NDSorting zu Dimensionieren
        Public Sub Dimit(ByVal NPenalty As Integer, ByVal NConstrains As Integer, ByVal varanz As Integer, ByRef TMP() As Struct_NDSorting)
            Dim i As Integer

            For i = 1 To TMP.GetUpperBound(0)
                Call Dimit(NPenalty, NConstrains, varanz, TMP(i))
            Next

        End Sub

        'Überladen Methode die ein Struct NDSorting kopiert
        Public Sub Copy(ByVal Source As Struct_NDSorting, ByRef Dest As Struct_NDSorting)
            Dest.penalty = Source.penalty.Clone       '01 Werte der Penaltyfunktion(en)
            Dest.constrain = Source.constrain.Clone   '02 Werte der Randbedingung(en)
            Dest.feasible = Source.feasible           '03 Gültiges Ergebnis ?
            Dest.dominated = Source.dominated         '04 Kennzeichnung ob dominiert
            Dest.Front = Source.Front                 '05 Nummer der Pareto Front
            Dest.X = Source.X.Clone                   '06 Wert der Variablen
            Dest.d = Source.d.Clone                   '07 Schrittweite der Variablen
            Dest.distance = Source.distance           '08 Distanzwert für Crowding distance sort
        End Sub

        'Überladen Methode die ein Array aus Struct NDSorting kopiert
        Public Sub Copy(ByVal Source() As Struct_NDSorting, ByRef Dest() As Struct_NDSorting)
            Dim i As Integer
            For i = 1 To Source.GetUpperBound(0)
                Call Copy(Source(i), Dest(i))
            Next
        End Sub

    End Structure

    Dim NDSorting() As Struct_NDSorting
    Dim swap As Struct_NDSorting

    Private Structure Struct_Sortierung
        Dim Index As Short
        Dim penalty() As Double
    End Structure

    Private Structure Struct_Neighbourhood
        Dim Index As Short
        Dim distance As Double
    End Structure

    'Deklarationsteil allgemein
    '**************************

    'Methoden
    '########

    'Schritt 2 -5 zum Initialisieren der PES
    '***************************************
    Public Sub PesInitialise(ByRef PES_Settings As Struct_Settings, ByVal AnzPara As Short, ByVal AnzPenalty As Short, ByVal AnzConstr As Short, ByRef mypara() As Double, ByVal Method As String)

        '2. Schritt: PES - ES_OPTIONS
        'Optionen der Evolutionsstrategie werden übergeben
        Call EsSettings(PES_Settings, Method)

        '3. Schritt: PES - ES_INI
        'Die öffentlichen dynamischen Arrays werden initialisiert (Dn, An, Xn, Xmin, Xmax), die Anzahl der Zielfunktionen wird festgelegt und Ausgangsparameter werden übergeben (War früher ES_Let Parameter)
        Call EsDim(AnzPara, AnzPenalty, AnzConstr, mypara)

        '4. Schritt: PES - ES_PREPARE
        'Interne Variablen werden initialisiert, Zufallsgenerator wird initialisiert
        Call EsPrepare()

        '5. Schritt: PES - ES_STARTVALUES
        'Startwerte werden zugewiesen
        Call EsStartvalues()

    End Sub

    'Schritt 2: ES_SETTINGS
    'Function ES_SETTINGS übergibt Optionen für Evolutionsstrategie und Prüft die eingestellten Optionen
    '***************************************************************************************************
    Private Sub EsSettings(ByRef Settings As Struct_Settings, ByVal Method As String)

        'Überprüfung der Übergebenen Werte
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        If (Settings.iEvoTyp < 1 Or Settings.iEvoTyp > 2) Then
            Throw New Exception("Typ der Evolutionsstrategie ist nicht '+' oder ','")
        End If
        If (Settings.iPopEvoTyp < 1 Or Settings.iPopEvoTyp > 2) Then
            Throw New Exception("Typ der Evolutionsstrategie auf Pupulationsebene ist nicht '+' oder ','")
        End If
        If (Settings.NRunden < 1) Then
            Throw New Exception("Die Anzahl der Runden ist kleiner 1")
        End If
        If (Settings.NPopul < 1) Then
            Throw New Exception("Die Anzahl der Populationen ist kleiner 1")
        End If
        If (Settings.NPopEltern < 1) Then
            Throw New Exception("Die Anzahl der Populationseltern ist kleiner 1")
        End If
        If (Settings.iOptPopEltern < 1 Or Settings.iOptPopEltern > 3) Then
            Throw New Exception("Ermittlung der Populationseltern ist nicht Mittelwert, Rekombination oder Selektion!")
        End If
        If (Settings.iOptEltern < 1 Or Settings.iOptEltern > 6) Then
            Throw New Exception("Strategie zur Ermittlung der Eltern ist nicht möglich!")
        End If
        If (Settings.NEltern < 1) Then
            Throw New Exception("Die Anzahl der Eltern ist kleiner 1!")
        End If
        If (Settings.NNachf < 1) Then
            Throw New Exception("Die Anzahl der Nachfahren ist kleiner 1!")
        End If
        If (Settings.NGen < 1) Then
            Throw New Exception("Die Anzahl der Generationen ist kleiner 1!")
        End If
        If (Settings.NRekombXY < 1) Then
            Throw New Exception("Der Wert für die X/Y-Schema Rekombination ist kleiner 1!")
        End If
        If (Settings.DnStart < 0) Then
            Throw New Exception("Die Startschrittweite darf nicht kleiner 0 sein!")
        End If
        If (Settings.iStartPar < 1 Or Settings.iStartPar > 2) Then
            Throw New Exception("Die Startaparameter dürfen nur zufällig sein oder aus den Originalparameter bestehen!")
        End If
        If (Settings.NPopul < Settings.NPopEltern) Then
            Throw New Exception("Die Anzahl der Populationseltern darf nicht größer als die Anzahl der Populationen!")
        End If
        If (Settings.NNachf <= Settings.NEltern) And Not Method = "HYBRID" Then
            Throw New Exception("Die Anzahl der Eltern muss kleiner als die Anzahl der Nachfahren!" & Chr(13) & Chr(10) & "'Rechenberg 73' schlägt ein Verhältnis von 1:3 bis 1:5 vor.")
        End If

        'Übergabe der Optionen
        'xxxxxxxxxxxxxxxxxxxxx

        Me.PES_Settings = Settings

    End Sub


    'Schritt 3: ES_INI
    'Function ES_INI Initialisiert benötigte dynamische Arrays und legt Anzahl der Zielfunktionen fest
    '*************************************************************************************************
    Private Sub EsDim(ByVal AnzPara As Short, ByVal AnzPenalty As Short, ByVal AnzConstr As Short, ByVal mypara() As Double)
        Dim i As Integer

        'Überprüfung der Eingabeparameter (es muss mindestens ein Parameter variiert und eine
        'Penaltyfunktion ausgewertet werden)

        If (AnzPara <= 0 Or AnzPenalty <= 0) Then
            Throw New Exception("Es muss mindestens ein Parameter variiert und eine Penaltyfunktion ausgewertet werden")
        End If

        Initial.varanz = AnzPara                        'Anzahl der Parameter wird übergeben
        Initial.NPenalty = AnzPenalty                   'Anzahl der Zielfunktionen wird übergeben
        Initial.NConstrains = AnzConstr                 'Anzahl der Randbedingungen wird übergeben

        ReDim Initial.Xn(Initial.varanz)                'Variablenvektor wird initialisiert
        ReDim Initial.Xmin(Initial.varanz)              'UntereSchrankenvektor wird initialisiert
        ReDim Initial.Xmax(Initial.varanz)              'ObereSchrankenvektor wird initialisiert
        ReDim Initial.Dn(Initial.varanz)                'Schrittweitenvektor wird initialisiert

        For i = 1 To Initial.varanz
            Initial.Xn(i) = mypara(i)
            Initial.Xmin(i) = 0
            Initial.Xmax(i) = 1
            'ToDo: Welchen Zweck hat diese Prüfung hier. Die Grenzen sollten zu jeder Zeit eingehalten werden
            Initial.Xn(i) = Math.Min(Initial.Xn(i), Initial.Xmax(i))
            Initial.Xn(i) = Math.Max(Initial.Xn(i), Initial.Xmin(i))
        Next

    End Sub

    'Schritt 4: ES_PREPARE
    'Function ES_PREPARE initialisiert alle internen Arrays und setzt den
    'Bestwertspeicher auf sehr großen Wert (Strategie minimiert in dieser Umsetzung)
    'TODO: ESPrepare Für Paretooptimierung noch nicht fertig!!!!
    '*******************************************************************************
    Private Sub EsPrepare()

        Dim m, n, l, i As Short

        For i = 1 To Initial.varanz
            Initial.Dn(i) = PES_Settings.DnStart
        Next i

        'Parametervektoren initialisieren
        ReDim Dp(Initial.varanz, PES_Settings.NEltern, PES_Settings.NPopEltern)
        ReDim Xp(Initial.varanz, PES_Settings.NEltern, PES_Settings.NPopEltern)
        ReDim Qbpop(PES_Settings.NPopul, Initial.NPenalty)
        ReDim QbpopD(PES_Settings.NPopul)
        ReDim Dbpop(Initial.varanz, PES_Settings.NEltern, PES_Settings.NPopul)
        ReDim Xbpop(Initial.varanz, PES_Settings.NEltern, PES_Settings.NPopul)
        '---------------------
        ReDim De(Initial.varanz, PES_Settings.NEltern, PES_Settings.NPopul)
        ReDim Xe(Initial.varanz, PES_Settings.NEltern, PES_Settings.NPopul)
        '---------------------
        ReDim Db(Initial.varanz, PES_Settings.NEltern, PES_Settings.NPopul)
        ReDim Xb(Initial.varanz, PES_Settings.NEltern, PES_Settings.NPopul)
        ReDim Qb(PES_Settings.NEltern, PES_Settings.NPopul, Initial.NPenalty)
        ReDim Rb(PES_Settings.NEltern, PES_Settings.NPopul, Initial.NConstrains)
        '---------------------
        'NDSorting wird nur benötigt, falls eine Paretofront approximiert wird
        If PES_Settings.is_MO_Pareto Then
            ReDim NDSorting(PES_Settings.NEltern + PES_Settings.NNachf)
            For i = 1 To PES_Settings.NEltern + PES_Settings.NNachf
                ReDim NDSorting(i).penalty(Initial.NPenalty)
                ReDim NDSorting(i).constrain(Initial.NConstrains)
                ReDim NDSorting(i).d(Initial.varanz)
                ReDim NDSorting(i).X(Initial.varanz)
            Next i
            If PES_Settings.iOptEltern = EVO_ELTERN.Neighbourhood Then
                ReDim PenaltyDistance(PES_Settings.NEltern, PES_Settings.NEltern)
                ReDim IndexEltern(PES_Settings.NEltern - 1)
                ReDim Distanceb(PES_Settings.NEltern)
            End If
        End If

        For n = 1 To PES_Settings.NEltern
            For m = 1 To PES_Settings.NPopul
                For l = 1 To Initial.NPenalty
                    'Qualität der Eltern (Anzahl = parents) wird auf sehr großen Wert gesetzt
                    Qb(n, m, l) = 1.0E+300
                Next l
                If Initial.NConstrains > 0 Then
                    For l = 1 To Initial.NConstrains
                        'Restriktion der Eltern (Anzahl = parents) wird auf sehr kleinen Wert gesetzt
                        Rb(n, m, l) = -1.0E+300
                    Next l
                End If
            Next m
        Next

        If PES_Settings.is_MO_Pareto Then
            For n = 1 To PES_Settings.NPopul
                For m = 1 To Initial.NPenalty
                    Select Case PES_Settings.iPopPenalty

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
            For n = 1 To PES_Settings.NPopul
                For m = 1 To Initial.NPenalty
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

    'Schritt 5: ES_STARTVALUES
    'Function ES_STARTVALUES setzt die Startwerte
    'PES_Settings.iStartPar 1: Zufällige Startwert  -> Schrittweite = Startschrittweite
    '                                               -> Parameterwert = zufällig [0,1]
    'PES_Settings.iStartPar 2: Originalparameter    -> Schrittweite = Startschrittweite
    '                                               -> Parameterwert = Originalparameter
    '***********************************************************************************
    Public Sub EsStartvalues()

        Dim n, v, m As Short

        'Zufallsgenerator initialisieren
        Randomize()

        Select Case PES_Settings.iStartPar

            Case EVO_STARTPARAMETER.Zufall 'Zufällige Startwerte
                For v = 1 To Initial.varanz
                    For n = 1 To PES_Settings.NEltern
                        For m = 1 To PES_Settings.NPopEltern
                            'Startwert für die Elternschrittweite wird zugewiesen
                            Dp(v, n, m) = Initial.Dn(1)
                            'Startwert für die Eltern werden zugewiesen
                            '(Zufallszahl zwischen 0 und 1)
                            Xp(v, n, m) = Rnd()
                        Next m
                    Next n
                Next v

            Case EVO_STARTPARAMETER.Original 'Originalparameter
                For v = 1 To Initial.varanz
                    For n = 1 To PES_Settings.NEltern
                        For m = 1 To PES_Settings.NPopEltern
                            'Startwert für die Elternschrittweite wird zugewiesen
                            Dp(v, n, m) = Initial.Dn(1)
                            'Startwert für die Eltern werden zugewiesen
                            '(alle gleich Anfangswerte)
                            Xp(v, n, m) = Initial.Xn(v)
                        Next m
                    Next n
                Next v
        End Select

    End Sub

    'ES_GET_PARAMETER - dient zur Rückgabe der mutierten Parameter
    '*************************************************************
    Public Function EsGetParameter() As Double()

        Dim i As Short
        Dim mypara() As Double

        ReDim mypara(Initial.varanz)

        For i = 1 To Initial.varanz
            mypara(i) = Initial.Xn(i)
        Next i

        Return mypara

    End Function

    'ES_GET_BESTWERT - gibt den kompletten Bestwertspeicher aus
    '**********************************************************
    Public Function EsGetBestwert() As Double(,)

        Dim i, j As Short
        Dim Bestwert(,) As Double

        ReDim Bestwert(PES_Settings.NEltern, Initial.NPenalty)

        For i = 1 To Initial.NPenalty
            For j = 1 To PES_Settings.NEltern
                Bestwert(j, i) = Qb(j, PES_iAkt.iAktPop, i)
            Next j
        Next i

        Return Bestwert

    End Function

    'ES_GET_SEKUNDÄRE_POPULATIONEN - Sekundäre Population speichert immer die angegebene
    'Anzahl von Bestwerten und kann den Bestwertspeicher alle x Generationen überschreiben
    '*************************************************************************************
    Public Function EsGetSekundärePopulation() As Double(,)

        Dim j, i As Short
        Dim SekPopulation(,) As Double

        ''Notwendig, falls auch die Variablenwerte für eine spätere Datensatzerstellung ausgelesen werden sollen (Dirk)

        'ReDim Population(UBound(SekundärQb), Eigenschaft.varanz, Eigenschaft.NPenalty + 1)
        'For i = 1 To UBound(SekundärQb)
        '    For j = 1 To Eigenschaft.varanz
        '        Population(i, j, 1) = SekundärQb(i).X(j)
        '        For k = 1 To Eigenschaft.NPenalty
        '            Population(i, j, k + 1) = SekundärQb(i).penalty(k)
        '        Next k
        '    Next j
        'Next i

        ReDim SekPopulation(UBound(SekundärQb), Initial.NPenalty)
        '!Wenn Fehler hier "SekundäreQb = Nothing" auftritt wurde TeeChart mit der falschen Serie bzw. zu wenig Serien gestartet!!!

        For i = 1 To UBound(SekundärQb)
            For j = 1 To Initial.NPenalty
                SekPopulation(i, j) = SekundärQb(i).penalty(j)
            Next j
        Next i

        Return SekPopulation

    End Function


    'ES_POP_VARIA - ToDO: Beschreibung fehlt
    '*******************************************************************************
    Public Sub EsPopVaria()

        Dim m, n, v As Short
        Dim R As Short                      'Zufälliger Integer Wert

        Select Case PES_Settings.iOptPopEltern

            Case EVO_POP_ELTERN.Rekombination 'MultiRekombination über alle Eltern (x/x,y) oder (x/x+y)
                For n = 1 To PES_Settings.NEltern
                    R = Int(PES_Settings.NPopEltern * Rnd()) + 1
                    For v = 1 To Initial.varanz
                        'Selektion der Schrittweite
                        De(v, n, PES_iAkt.iAktPop) = Dp(v, n, R)
                        'Selektion des Elter
                        Xe(v, n, PES_iAkt.iAktPop) = Xp(v, n, R)
                    Next v
                Next n

            Case EVO_POP_ELTERN.Mittelwert 'Mittelwertbildung über alle Eltern
                'Ermitteln der Elter und Schrittweite über Mittelung der Elternschrittweiten
                For v = 1 To Initial.varanz
                    For n = 1 To PES_Settings.NEltern
                        De(v, n, PES_iAkt.iAktPop) = 0
                        Xe(v, n, PES_iAkt.iAktPop) = 0
                        For m = 1 To PES_Settings.NPopEltern
                            'Mittelung der Schrittweite,
                            De(v, n, PES_iAkt.iAktPop) = De(v, n, PES_iAkt.iAktPop) + (Dp(v, n, m) / PES_Settings.NPopEltern)
                            'Mittelung der Eltern,
                            Xe(v, n, PES_iAkt.iAktPop) = Xe(v, n, PES_iAkt.iAktPop) + (Xp(v, n, m) / PES_Settings.NPopEltern)
                        Next m
                    Next n
                Next v

            Case EVO_POP_ELTERN.Selektion 'Zufallswahl über alle Eltern
                R = Int(PES_Settings.NPopEltern * Rnd()) + 1 'Zufallszahl entscheidet welcher
                'Elternteil vererbt wird
                For v = 1 To Initial.varanz
                    For n = 1 To PES_Settings.NEltern
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
    Public Sub EsVaria()

        Dim i, v, n, j As Short
        Dim R As Short                      'Zufälliger Integer Wert
        Dim Realisierungsspeicher() As Short
        Dim Elternspeicher() As Short
        Dim Z1, Elter, Z2 As Short

        Select Case PES_Settings.iOptEltern

            Case EVO_ELTERN.Selektion 'Zufallswahl über alle Eltern

                R = Int(PES_Settings.NEltern * Rnd()) + 1 'Zufallszahl entscheidet
                'welcher Enternteil vererbt wird
                For v = 1 To Initial.varanz
                    'Selektion der Schrittweite
                    Initial.Dn(v) = De(v, R, PES_iAkt.iAktPop)
                    'Selektion des Elter
                    Initial.Xn(v) = Xe(v, R, PES_iAkt.iAktPop)
                Next v

            Case EVO_ELTERN.XX_Diskret 'Multi-Rekombination, diskret

                For v = 1 To Initial.varanz
                    R = Int(PES_Settings.NEltern * Rnd()) + 1
                    'Selektion der Schrittweite
                    Initial.Dn(v) = De(v, R, PES_iAkt.iAktPop)
                    'Selektion des Elter
                    Initial.Xn(v) = Xe(v, R, PES_iAkt.iAktPop)
                Next v

            Case EVO_ELTERN.XX_Mitteln 'Multi-Rekombination, gemittelt

                For v = 1 To Initial.varanz
                    Initial.Dn(v) = 0
                    Initial.Xn(v) = 0

                    For n = 1 To PES_Settings.NEltern
                        'Mittelung der Schrittweite,
                        Initial.Dn(v) = Initial.Dn(v) + (De(v, n, PES_iAkt.iAktPop) / PES_Settings.NRekombXY)
                        'Mittelung der Eltern,
                        Initial.Xn(v) = Initial.Xn(v) + (Xe(v, n, PES_iAkt.iAktPop) / PES_Settings.NRekombXY)
                    Next

                Next v

            Case EVO_ELTERN.XY_Diskret 'Multi-Rekombination nach X/Y-Schema, diskrete Vertauschung

                ReDim Realisierungsspeicher(PES_Settings.NRekombXY)
                ReDim Elternspeicher(PES_Settings.NEltern)

                For i = 1 To PES_Settings.NEltern
                    Elternspeicher(i) = i
                Next i

                For i = 1 To PES_Settings.NRekombXY
                    R = Int((PES_Settings.NEltern - (i - 1)) * Rnd()) + 1
                    Realisierungsspeicher(i) = Elternspeicher(R)

                    For j = R To (PES_Settings.NEltern - 1)
                        Elternspeicher(R) = Elternspeicher(R + 1)
                    Next j

                Next i

                For v = 1 To Initial.varanz
                    R = Int(PES_Settings.NRekombXY * Rnd()) + 1
                    'Selektion der Schrittweite
                    Initial.Dn(v) = De(v, Realisierungsspeicher(R), PES_iAkt.iAktPop)
                    'Selektion des Elter
                    Initial.Xn(v) = Xe(v, Realisierungsspeicher(R), PES_iAkt.iAktPop)
                Next v

            Case EVO_ELTERN.XY_Mitteln 'Multi-Rekombination nach X/Y-Schema, Mittelung der Gene

                ReDim Realisierungsspeicher(PES_Settings.NRekombXY)
                ReDim Elternspeicher(PES_Settings.NEltern)

                For i = 1 To PES_Settings.NEltern
                    Elternspeicher(i) = i
                Next i

                For i = 1 To PES_Settings.NRekombXY
                    R = Int((PES_Settings.NEltern - (i - 1)) * Rnd()) + 1
                    Realisierungsspeicher(i) = Elternspeicher(R)

                    For j = R To (PES_Settings.NEltern - 1)
                        Elternspeicher(R) = Elternspeicher(R + 1)
                    Next j

                Next i

                For v = 1 To Initial.varanz
                    Initial.Dn(v) = 0
                    Initial.Xn(v) = 0

                    For n = 1 To PES_Settings.NRekombXY
                        'Mittelung der Schrittweite,
                        Initial.Dn(v) = Initial.Dn(v) + (De(v, Elternspeicher(n), PES_iAkt.iAktPop) / PES_Settings.NRekombXY)
                        'Mittelung der Eltern,
                        Initial.Xn(v) = Initial.Xn(v) + (Xe(v, Elternspeicher(n), PES_iAkt.iAktPop) / PES_Settings.NRekombXY)
                    Next

                Next v

            Case EVO_ELTERN.Neighbourhood 'Neighbourhood Rekombination

                Z1 = Int(PES_Settings.NEltern * Rnd()) + 1
                Do
                    Z2 = Int(PES_Settings.NEltern * Rnd()) + 1
                Loop While Z1 = Z2

                'Tournament über Crowding Distance

                If Distanceb(Z1) > Distanceb(Z2) Then
                    Elter = Z1
                Else
                    Elter = Z2
                End If

                If (Elter = 1 Or Elter = PES_Settings.NEltern) Then

                    For v = 1 To Initial.varanz
                        'Selektion der Schrittweite
                        Initial.Dn(v) = De(v, Elter, PES_iAkt.iAktPop)
                        'Selektion des Elter
                        Initial.Xn(v) = Xe(v, Elter, PES_iAkt.iAktPop)
                    Next

                Else

                    Call Neighbourhood_Eltern(Elter)
                    For v = 1 To Initial.varanz
                        'Do
                        '    Faktor = Rnd
                        '    Faktor = (-1) * Eigenschaft.d + Faktor * (1 + Eigenschaft.d)
                        '    'Selektion der Schrittweite
                        '    Eigenschaft.Dn(v) = De(v, IndexEltern(1), Eigenschaft.iaktuellePopulation) * Faktor + _
                        '                     De(v, IndexEltern(2), Eigenschaft.iaktuellePopulation) * (1 - Faktor)
                        '    Eigenschaft.Xn(v) = Xe(v, IndexEltern(1), Eigenschaft.iaktuellePopulation) * Faktor + _
                        '                     Xe(v, IndexEltern(2), Eigenschaft.iaktuellePopulation) * (1 - Faktor)
                        'Loop While (Eigenschaft.Xn(v) <= Eigenschaft.Xmin(v) Or Eigenschaft.Xn(v) > Eigenschaft.Xmax(v))

                        R = Int(PES_Settings.NRekombXY * Rnd() + 1)
                        'Selektion der Schrittweite
                        Initial.Dn(v) = De(v, IndexEltern(R), PES_iAkt.iAktPop)
                        'Selektion des Elter
                        Initial.Xn(v) = Xe(v, IndexEltern(R), PES_iAkt.iAktPop)
                    Next

                End If

        End Select

    End Sub

    'ES_POP_MUTATION - ToDo: Beschreibung fehlt
    '******************************************
    Public Sub EsPopMutation()

        Dim v, n As Short

        If Not PES_Settings.isDnVektor Then
            '+/-1
            expo = (2 * Int(Rnd() + 0.5) - 1)
            'Schrittweite wird mutiert
            DeTemp = De(1, 1, PES_iAkt.iAktPop) * palpha ^ expo
        End If

        For v = 1 To Initial.varanz

            For n = 1 To PES_Settings.NEltern

                Do

                    If PES_Settings.isDnVektor Then
                        '+/-1
                        expo = (2 * Int(Rnd() + 0.5) - 1)
                        'Schrittweite wird mutiert
                        DeTemp = De(v, n, PES_iAkt.iAktPop) * palpha ^ expo
                    End If
                    'Normalverteilte Zufallszahl mit Standardabweichung 1/sqr(varanz)
                    Dim Z As Double
                    Z = System.Math.Sqrt(-2 * System.Math.Log(1 - Rnd()) / Initial.varanz) * System.Math.Sin(6.2832 * Rnd())
                    'Mutation wird durchgeführt
                    XeTemp = Xe(v, n, PES_iAkt.iAktPop) + DeTemp * Z

                    ' Restriktion für die mutierten Werte
                Loop While (XeTemp <= Initial.Xmin(v) Or XeTemp > Initial.Xmax(v))

                De(v, n, PES_iAkt.iAktPop) = DeTemp
                Xe(v, n, PES_iAkt.iAktPop) = XeTemp

            Next n

        Next v

    End Sub

    'ES_MUTATION - Mutieren der Ausgangswerte
    '****************************************
    Public Sub EsMutation()

        Dim v As Short

        If Not PES_Settings.isDnVektor Then
            '+/-1
            expo = (2 * Int(Rnd() + 0.5) - 1)
            'Schrittweite wird mutiert
            DnTemp = Initial.Dn(1) * galpha ^ expo
        End If

        For v = 1 To Initial.varanz
            Do
                If PES_Settings.isDnVektor Then
                    '+/-1
                    expo = (2 * Int(Rnd() + 0.5) - 1)
                    'Schrittweite wird mutiert
                    DnTemp = Initial.Dn(v) * galpha ^ expo
                End If
                'Normalverteilte Zufallszahl mit Standardabweichung 1/sqr(varanz)
                Dim Z As Double
                Z = System.Math.Sqrt(-2 * System.Math.Log(1 - Rnd()) / Initial.varanz) * System.Math.Sin(6.2832 * Rnd())
                'Mutation wird durchgeführt
                XnTemp = Initial.Xn(v) + DnTemp * Z
                'Restriktion für die mutierten Werte
            Loop While (XnTemp <= Initial.Xmin(v) Or XnTemp > Initial.Xmax(v))

            Initial.Dn(v) = DnTemp
            Initial.Xn(v) = XnTemp
        Next v

    End Sub

    'ES_POP_BEST - Einordnen der Qualitätsfunktion im PopulationsBestwertspeicher
    '****************************************************************************
    Public Sub EsPopBest()

        Dim m, i, j, n As Short
        Dim h1, h2 As Double

        'Der schlechtetste der besten Qualitätswerte wird bestimmt ; Position -> i
        '(höchster Wert der Penaltyfunktion, niedrigster Wert der Crowding Distance)
        i = 1
        h1 = Qbpop(1, 1)
        For m = 2 To PES_Settings.NPopul
            If Not PES_Settings.is_MO_Pareto Then
                If Qbpop(m, 1) > h1 Then
                    h1 = Qbpop(m, 1)
                    i = m
                End If
            Else
                Select Case PES_Settings.iPopPenalty

                    Case EVO_POP_PENALTY.Crowding
                        If Qbpop(m, 1) > h1 Then
                            h1 = Qbpop(m, 1)
                            i = m
                        End If

                    Case EVO_POP_PENALTY.Spannweite
                        If Qbpop(m, 1) < h1 Then
                            h2 = Qbpop(m, 2)
                            i = m
                        End If
                End Select
            End If
        Next m

        'Der schlechtetste der besten Qualitätswerte wird bestimmt ; Position -> i
        '(höchster Wert der Kostenfunktion, niedrigster Wert der Spannweite)
        If PES_Settings.is_MO_Pareto Then
            j = 1
            h2 = Qbpop(1, 2)
            For m = 2 To PES_Settings.NPopul
                If Qbpop(m, 2) < h2 Then
                    h2 = Qbpop(m, 2)
                    j = m
                End If
            Next m
        End If

        'Qualität der aktuellen Population wird bestimmt
        h1 = 0
        If Not PES_Settings.is_MO_Pareto Then
            For m = 1 To PES_Settings.NEltern
                h1 = h1 + Qb(m, PES_iAkt.iAktPop, 1) / PES_Settings.NEltern
            Next m
        Else
            h2 = 0
            h1 = NDS_Crowding_Distance_Count(h2)
        End If

        'Falls die Qualität des aktuellen Population besser ist (Penaltyfunktion geringer)
        'als die schlechteste im Bestwertspeicher, wird diese ersetzt
        If Not PES_Settings.is_MO_Pareto Then
            If h1 < Qbpop(i, 1) Then
                Qbpop(i, 1) = h1
                For m = 1 To Initial.varanz
                    For n = 1 To PES_Settings.NEltern
                        'Die Schrittweite wird ebenfalls übernommen
                        Dbpop(m, n, i) = Db(m, n, PES_iAkt.iAktPop)
                        'Die eigentlichen Parameterwerte werden übernommen
                        Xbpop(m, n, i) = Xb(m, n, PES_iAkt.iAktPop)
                    Next n
                Next m
            End If
        Else
            Select Case PES_Settings.iPopPenalty

                Case EVO_POP_PENALTY.Crowding
                    If h1 < Qbpop(i, 1) Then
                        Qbpop(i, 1) = h1
                        For m = 1 To Initial.varanz
                            For n = 1 To PES_Settings.NEltern
                                'Die Schrittweite wird ebenfalls übernommen
                                Dbpop(m, n, i) = Db(m, n, PES_iAkt.iAktPop)
                                'Die eigentlichen Parameterwerte werden übernommen
                                Xbpop(m, n, i) = Xb(m, n, PES_iAkt.iAktPop)
                            Next n
                        Next m
                    End If

                Case EVO_POP_PENALTY.Spannweite
                    If h2 > Qbpop(j, 2) Then
                        Qbpop(j, 2) = h2
                        For m = 1 To Initial.varanz
                            For n = 1 To PES_Settings.NEltern
                                'Die Schrittweite wird ebenfalls übernommen
                                Dbpop(m, n, j) = Db(m, n, PES_iAkt.iAktPop)
                                'Die eigentlichen Parameterwerte werden übernommen
                                Xbpop(m, n, j) = Xb(m, n, PES_iAkt.iAktPop)
                            Next n
                        Next m
                    End If
            End Select
        End If

    End Sub

    'ES_BEST - Einordnen der Qualitätsfunktion im Bestwertspeicher
    '*************************************************************
    Public Sub EsBest(ByVal QN() As Double, ByVal RN() As Double)

        Dim m, i, j, v As Short
        Dim h As Double

        If (Not PES_Settings.is_MO_Pareto) Then
            'SO - Standard ES nach Rechenberg
            '--------------------------------
            'Der schlechteste der besten Qualitätswerte wird bestimmt ; Position -> j
            '(höchster Wert der Penaltyfunktion)
            j = 1
            h = Qb(1, PES_iAkt.iAktPop, 1)

            For m = 2 To PES_Settings.NEltern
                If Qb(m, PES_iAkt.iAktPop, 1) > h Then
                    h = Qb(m, PES_iAkt.iAktPop, 1)
                    j = m
                End If
            Next m

            'Falls die Qualität des aktuellen Nachkommen besser ist (Penaltyfunktion geringer)
            'als die schlechteste im Bestwertspeicher, wird dieser ersetzt
            If QN(0) < Qb(j, PES_iAkt.iAktPop, 1) Then
                Qb(j, PES_iAkt.iAktPop, 1) = QN(0)
                For v = 1 To Initial.varanz
                    'Die Schrittweite wird ebenfalls übernommen
                    Db(v, j, PES_iAkt.iAktPop) = Initial.Dn(v)
                    'Die eigentlichen Parameterwerte werden übernommen
                    Xb(v, j, PES_iAkt.iAktPop) = Initial.Xn(v)
                Next v
            End If

        Else
            'Multi-Objective Pareto
            '----------------------
            With NDSorting(PES_iAkt.iAktNachf)
                For i = 1 To Initial.NPenalty
                    .penalty(i) = QN(i - 1)             'Bug 135: .penalty fängt bei 1 an!
                Next i
                .feasible = True
                For i = 1 To Initial.NConstrains
                    .constrain(i) = RN(i - 1)           'Bug 135: .constrain fängt bei 1 an!
                    If .constrain(i) < 0 Then .feasible = False
                Next i
                .dominated = False
                .Front = 0
                For v = 1 To Initial.varanz
                    .d(v) = Initial.Dn(v)
                    .X(v) = Initial.Xn(v)
                Next v
                .distance = 0
            End With
        End If

    End Sub

    'ES_BESTWERTSPEICHER
    'Führt einen Reset des Bestwertspeicher durch, falls eine Komma-Strategie gewählt ist
    '************************************************************************************
    Public Sub EsResetBWSpeicher()
        Dim n, i As Short

        If (PES_Settings.iEvoTyp = EVO_STRATEGIE.Komma) Then
            For n = 1 To PES_Settings.NEltern
                For i = 1 To Initial.NPenalty
                    Qb(n, PES_iAkt.iAktPop, i) = 1.0E+300
                Next i
            Next n
        End If

    End Sub

    'ES_POP_BESTWERTSPEICHER
    'Führt einen Reset des Bestwertspeicher auf Populationsebene durch, falls eine Komma-Strategie gewählt ist
    '*********************************************************************************************************
    Public Sub EsResetPopBWSpeicher()
        Dim n, i As Short

        If (PES_Settings.iPopEvoTyp = EVO_STRATEGIE.Komma) Then
            For n = 1 To PES_Settings.NPopul
                For i = 1 To Initial.NPenalty
                    Qbpop(n, i) = 1.0E+300
                Next i
            Next n
        End If

    End Sub

    'ES_POP_ELTERN - Eltern Population
    '*********************************
    Public Sub EsPopEltern()

        Dim n, m, v As Short
        Dim swap(2) As Double
        Dim Realisierungsspeicher(,) As Double
        Dim Z As Short

        Select Case PES_Settings.iPopPenalty
            Case EVO_POP_PENALTY.Crowding
                Z = 1
            Case EVO_POP_PENALTY.Spannweite
                Z = 2
        End Select

        ReDim Realisierungsspeicher(PES_Settings.NPopul, 2)

        'Die NPopEltern besten Individium-Sätze werden ermittelt
        For m = 1 To PES_Settings.NPopul
            Realisierungsspeicher(m, 1) = Qbpop(m, Z)
            Realisierungsspeicher(m, 2) = m
        Next m

        If (Not PES_Settings.is_MO_Pareto) Then
            'Standard ES nach Rechenberg
            '---------------------------
            For m = 1 To PES_Settings.NPopul
                For n = m To PES_Settings.NPopul
                    If Realisierungsspeicher(m, 1) > Realisierungsspeicher(n, 1) Then
                        swap(1) = Realisierungsspeicher(m, 1)
                        swap(2) = Realisierungsspeicher(m, 2)
                        Realisierungsspeicher(m, 1) = Realisierungsspeicher(n, 1)
                        Realisierungsspeicher(m, 2) = Realisierungsspeicher(n, 2)
                        Realisierungsspeicher(n, 1) = swap(1)
                        Realisierungsspeicher(n, 2) = swap(2)
                    End If
                Next
            Next

        Else
            'Multi-Objective mit Paretofront
            '-------------------------------
            Select Case PES_Settings.iPopPenalty

                Case EVO_POP_PENALTY.Crowding
                    For m = 1 To PES_Settings.NPopul
                        For n = m To PES_Settings.NPopul
                            If Realisierungsspeicher(m, 1) > Realisierungsspeicher(n, 1) Then
                                swap(1) = Realisierungsspeicher(m, 1)
                                swap(2) = Realisierungsspeicher(m, 2)
                                Realisierungsspeicher(m, 1) = Realisierungsspeicher(n, 1)
                                Realisierungsspeicher(m, 2) = Realisierungsspeicher(n, 2)
                                Realisierungsspeicher(n, 1) = swap(1)
                                Realisierungsspeicher(n, 2) = swap(2)
                            End If
                        Next
                    Next

                Case EVO_POP_PENALTY.Spannweite
                    For m = 1 To PES_Settings.NPopul
                        For n = m To PES_Settings.NPopul
                            If Realisierungsspeicher(m, 1) < Realisierungsspeicher(n, 1) Then
                                swap(1) = Realisierungsspeicher(m, 1)
                                swap(2) = Realisierungsspeicher(m, 2)
                                Realisierungsspeicher(m, 1) = Realisierungsspeicher(n, 1)
                                Realisierungsspeicher(m, 2) = Realisierungsspeicher(n, 2)
                                Realisierungsspeicher(n, 1) = swap(1)
                                Realisierungsspeicher(n, 2) = swap(2)
                            End If
                        Next
                    Next
            End Select
        End If

        'Die Eltern werden gleich der besten Kinder gesetzt (Schrittweite und Parameterwert)
        For m = 1 To PES_Settings.NPopEltern
            For n = 1 To PES_Settings.NEltern
                For v = 1 To Initial.varanz
                    Dp(v, n, m) = Dbpop(v, n, Int(Realisierungsspeicher(m, 2)))
                    Xp(v, n, m) = Xbpop(v, n, Int(Realisierungsspeicher(m, 2)))
                Next v
            Next n
        Next m

    End Sub

    'ES_ELTERN - Die neuen Eltern werden generiert
    '*********************************************
    Public Sub EsEltern()

        Dim l, m, v, i, j As Short
        Dim NFrontMember_aktuell, NFrontMember_gesamt As Short
        Dim rang As Short
        Dim Temp() As Struct_NDSorting
        Dim NDSResult() As Struct_NDSorting
        Dim aktuelle_Front As Short
        Dim Member_Sekundärefront As Short

        If (Not PES_Settings.is_MO_Pareto) Then
            'Standard ES nach Rechenberg
            'xxxxxxxxxxxxxxxxxxxxxxxxxxx
            'Die Eltern werden gleich der besten Kinder gesetzt (Schrittweite und Parameterwert)
            '---------------------------------------------------------------------
            For m = 1 To PES_Settings.NEltern
                For v = 1 To Initial.varanz
                    De(v, m, PES_iAkt.iAktPop) = Db(v, m, PES_iAkt.iAktPop)
                    Xe(v, m, PES_iAkt.iAktPop) = Xb(v, m, PES_iAkt.iAktPop)
                Next v
            Next m

        Else
            'Multi-Objective Pareto
            'xxxxxxxxxxxxxxxxxxxxxx
            '1. Eltern und Nachfolger werden gemeinsam betrachtet
            'Nur Eltern werden NDSorting hinzugefügt, Kinder sind schon oben drin
            '--------------------------------------------------------------------
            For m = PES_Settings.NNachf + 1 To PES_Settings.NNachf + PES_Settings.NEltern
                With NDSorting(m)
                    For l = 1 To Initial.NPenalty
                        .penalty(l) = Qb(m - PES_Settings.NNachf, PES_iAkt.iAktPop, l)
                    Next l
                    If Initial.NConstrains > 0 Then
                        .feasible = True
                        For l = 1 To Initial.NConstrains
                            .constrain(l) = Rb(m - PES_Settings.NNachf, PES_iAkt.iAktPop, l)
                            If .constrain(l) < 0 Then .feasible = False
                        Next l
                    End If
                    .dominated = False
                    .Front = 0
                    For v = 1 To Initial.varanz
                        'Die Schrittweite wird ebenfalls übernommen
                        .d(v) = Db(v, m - PES_Settings.NNachf, PES_iAkt.iAktPop)
                        'Die eigentlichen Parameterwerte werden übernommen
                        .X(v) = Xb(v, m - PES_Settings.NNachf, PES_iAkt.iAktPop)
                    Next v
                    .distance = 0
                End With
            Next m

            '2. Die einzelnen Fronten werden bestimmt
            '----------------------------------------
            rang = 1
            NFrontMember_gesamt = 0

            'Initialisierung von Temp (NDSorting)
            ReDim Temp(PES_Settings.NNachf + PES_Settings.NEltern)

            For i = 1 To (PES_Settings.NNachf + PES_Settings.NEltern)
                ReDim Temp(i).d(Initial.varanz)
                ReDim Temp(i).X(Initial.varanz)
            Next i
            'Initialisierung von NDSResult (NDSorting)
            ReDim NDSResult(PES_Settings.NNachf + PES_Settings.NEltern)

            For i = 1 To (PES_Settings.NNachf + PES_Settings.NEltern)
                ReDim NDSResult(i).d(Initial.varanz)
                ReDim NDSResult(i).X(Initial.varanz)
            Next i

            'NDSorting wird in Temp kopiert
            Call Temp(0).Copy(NDSorting, Temp)

            'Schleife läuft über die Zahl der Fronten die hier auch bestimmt werden
            Do
                'Entscheidet welche Werte dominiert werden und welche nicht
                Call Non_Dominated_Sorting(Temp, rang) 'aktualisiert auf n Objectives dm 10.05.05
                'Sortiert die nicht dominanten Lösungen nach oben,
                'die dominanten nach unten und zählt die Mitglieder der aktuellen Front
                NFrontMember_aktuell = Non_Dominated_Count_and_Sort(Temp)
                'NFrontMember_aktuell: Anzahl der Mitglieder der gerade bestimmten Front
                'NFrontMember_gesamt: Alle bisher als nicht dominiert klassifizierten Individuum
                NFrontMember_gesamt += NFrontMember_aktuell
                'Hier wird pro durchlauf die nicht dominierte Front in NDSResult geschaufelt
                'und die bereits klassifizierten Lösungen aus Temp Array gelöscht
                Call Non_Dominated_Result(Temp, NDSResult, NFrontMember_aktuell, NFrontMember_gesamt)
                'Rang ist hier die Nummer der Front
                rang += 1
            Loop While Not (NFrontMember_gesamt = PES_Settings.NEltern + PES_Settings.NNachf)

            '3. Der Bestwertspeicher wird entsprechend der Fronten oder der
            'sekundären Population gefüllt
            '-------------------------------------------------------------
            NFrontMember_aktuell = 0
            NFrontMember_gesamt = 0
            aktuelle_Front = 1

            Do
                NFrontMember_aktuell = Count_Front_Members(aktuelle_Front, NDSResult)

                'Es sind mehr Elterplätze für die nächste Generation verfügaber
                '-> schiss wird einfach rüberkopiert
                If NFrontMember_aktuell <= PES_Settings.NEltern - NFrontMember_gesamt Then
                    For i = NFrontMember_gesamt + 1 To NFrontMember_aktuell + NFrontMember_gesamt
                        For j = 1 To Initial.NPenalty
                            Qb(i, PES_iAkt.iAktPop, j) = NDSResult(i).penalty(j)
                        Next j
                        If Initial.NConstrains > 0 Then
                            For j = 1 To Initial.NConstrains
                                Rb(i, PES_iAkt.iAktPop, j) = NDSResult(i).constrain(j)
                            Next j
                        End If
                        For v = 1 To Initial.varanz
                            Db(v, i, PES_iAkt.iAktPop) = NDSResult(i).d(v)
                            Xb(v, i, PES_iAkt.iAktPop) = NDSResult(i).X(v)
                        Next v
                    Next i
                    NFrontMember_gesamt = NFrontMember_gesamt + NFrontMember_aktuell

                Else
                    'Es sind weniger Elterplätze für die nächste Generation verfügber
                    'als Mitglieder der aktuellen Front. Nur für diesen Rest wird crowding distance
                    'gemacht um zu bestimmen wer noch mitspielen darf und wer noch a biserl was druff hat
                    Call NDS_Crowding_Distance_Sort(NDSResult, NFrontMember_gesamt + 1, NFrontMember_gesamt + NFrontMember_aktuell)

                    For i = NFrontMember_gesamt + 1 To PES_Settings.NEltern

                        For j = 1 To Initial.NPenalty
                            Qb(i, PES_iAkt.iAktPop, j) = NDSResult(i).penalty(j)
                        Next j
                        If Initial.NConstrains > 0 Then
                            For j = 1 To Initial.NConstrains
                                Rb(i, PES_iAkt.iAktPop, j) = NDSResult(i).constrain(j)
                            Next j
                        End If
                        For v = 1 To Initial.varanz
                            Db(v, i, PES_iAkt.iAktPop) = NDSResult(i).d(v)
                            Xb(v, i, PES_iAkt.iAktPop) = NDSResult(i).X(v)
                        Next v

                    Next i

                    NFrontMember_gesamt = PES_Settings.NEltern

                End If

                aktuelle_Front += 1

            Loop While Not (NFrontMember_gesamt = PES_Settings.NEltern)

            '4: Sekundäre Population wird bestimmt und gespeichert
            '-------------------------------------------------------
            NFrontMember_aktuell = Count_Front_Members(1, NDSResult)

            Member_Sekundärefront = Math.Max(UBound(SekundärQb), 0) 'Weil wenn die Länge von SekundärQb 0 ist, gibt UBound -1 zurück!

            'SekPop wird um die aktuelle Front erweitert
            ReDim Preserve SekundärQb(Member_Sekundärefront + NFrontMember_aktuell)

            'Neue Member der SekPop bestimmen
            For i = Member_Sekundärefront + 1 To Member_Sekundärefront + NFrontMember_aktuell
                SekundärQb(i) = NDSResult(i - Member_Sekundärefront)
            Next i

            Call Non_Dominated_Sorting(SekundärQb, 1)
            NFrontMember_aktuell = Non_Dominated_Count_and_Sort_Sekundäre_Population(SekundärQb)
            ReDim Preserve SekundärQb(NFrontMember_aktuell)
            Call SekundärQb_Dubletten()
            NFrontMember_aktuell = Non_Dominated_Count_and_Sort_Sekundäre_Population(SekundärQb)
            ReDim Preserve SekundärQb(NFrontMember_aktuell)

            If (UBound(SekundärQb) > PES_Settings.NMemberSecondPop) Then
                Call NDS_Crowding_Distance_Sort(SekundärQb, 1, UBound(SekundärQb))
                ReDim Preserve SekundärQb(PES_Settings.NMemberSecondPop)
            End If

            'Prüfen, ob die Population jetzt mit Mitgliedern aus der Sekundären Population aufgefüllt werden soll
            If (PES_iAkt.iAktGen Mod PES_Settings.interact) = 0 And PES_Settings.isInteract Then
                NFrontMember_aktuell = Count_Front_Members(1, SekundärQb)
                If NFrontMember_aktuell > PES_Settings.NEltern Then
                    Call NDS_Crowding_Distance_Sort(SekundärQb, 1, UBound(SekundärQb))
                    For i = 1 To PES_Settings.NEltern

                        For j = 1 To Initial.NPenalty
                            Qb(i, PES_iAkt.iAktPop, j) = SekundärQb(i).penalty(j)
                        Next j
                        If Initial.NConstrains > 0 Then
                            For j = 1 To Initial.NConstrains
                                Rb(i, PES_iAkt.iAktPop, j) = SekundärQb(i).constrain(j)
                            Next j
                        End If
                        For v = 1 To Initial.varanz
                            Db(v, i, PES_iAkt.iAktPop) = SekundärQb(i).d(v)
                            Xb(v, i, PES_iAkt.iAktPop) = SekundärQb(i).X(v)
                        Next v

                    Next i
                End If
            End If

            'Neue Eltern werden gleich dem Bestwertspeicher gesetzt
            For m = 1 To PES_Settings.NEltern
                For v = 1 To Initial.varanz
                    De(v, m, PES_iAkt.iAktPop) = Db(v, m, PES_iAkt.iAktPop)
                    Xe(v, m, PES_iAkt.iAktPop) = Xb(v, m, PES_iAkt.iAktPop)
                Next v
            Next m

            'Sortierung der Lösungen ist nur für Neighbourhood-Rekombination notwendig
            If (PES_Settings.iOptEltern = EVO_ELTERN.Neighbourhood) Then
                Call Neighbourhood_AbstandsArray()
                Call Neighbourhood_Crowding_Distance()
            End If

        End If

    End Sub

    'NON_DOMINATED_SORTING - Entscheidet welche Werte dominiert werden und welche nicht
    '**********************************************************************************
    Private Sub Non_Dominated_Sorting(ByRef NDSorting() As Struct_NDSorting, ByVal rang As Short)

        Dim j, i, k As Short
        Dim isDominated As Boolean
        Dim Summe_Constrain(2) As Double

        If (Initial.NConstrains > 0) Then
            'Mit Constraints
            '===============
            For i = 1 To UBound(NDSorting)
                For j = 1 To UBound(NDSorting)

                    'Überpüfen, ob NDSorting(j) von NDSorting(i) dominiert wird
                    '----------------------------------------------------------
                    If (NDSorting(i).feasible And Not NDSorting(j).feasible) Then

                        'i gültig und j ungültig
                        '-----------------------
                        NDSorting(j).dominated = True

                    ElseIf ((Not NDSorting(i).feasible) And (Not NDSorting(j).feasible)) Then

                        'beide ungültig
                        '--------------
                        Summe_Constrain(1) = 0
                        Summe_Constrain(2) = 0

                        For k = 1 To Initial.NConstrains
                            If (NDSorting(i).constrain(k) < 0) Then
                                Summe_Constrain(1) += NDSorting(i).constrain(k)
                            End If
                            If (NDSorting(j).constrain(k) < 0) Then
                                Summe_Constrain(2) += NDSorting(j).constrain(k)
                            End If
                        Next k

                        If (Summe_Constrain(1) > Summe_Constrain(2)) Then
                            NDSorting(j).dominated = True
                        End If

                    ElseIf (NDSorting(i).feasible And NDSorting(j).feasible) Then

                        'beide gültig
                        '------------
                        isDominated = False

                        For k = 1 To Initial.NPenalty
                            isDominated = isDominated Or (NDSorting(i).penalty(k) < NDSorting(j).penalty(k))
                        Next k

                        For k = 1 To Initial.NPenalty
                            isDominated = isDominated And (NDSorting(i).penalty(k) <= NDSorting(j).penalty(k))
                        Next k

                        If (isDominated) Then
                            NDSorting(j).dominated = True
                        End If


                    End If
                Next j
            Next i

        Else
            'Ohne Constraints
            '----------------
            For i = 1 To UBound(NDSorting)
                For j = 1 To UBound(NDSorting)

                    isDominated = False

                    For k = 1 To Initial.NPenalty
                        isDominated = isDominated Or (NDSorting(i).penalty(k) < NDSorting(j).penalty(k))
                    Next k

                    For k = 1 To Initial.NPenalty
                        isDominated = isDominated And (NDSorting(i).penalty(k) <= NDSorting(j).penalty(k))
                    Next k

                    If (isDominated) Then
                        NDSorting(j).dominated = True
                    End If
                Next j
            Next i
        End If

        For i = 1 To UBound(NDSorting)
            'Hier wird die Nummer der Front geschrieben
            If NDSorting(i).dominated = False Then NDSorting(i).Front = rang
        Next i

    End Sub

    'NON_DOMINATED_COUNT_AND_SORT - Sortiert die nicht dominanten Lösungen nach oben,
    'die dominanten nach unten, gibt die Zahl der dominanten Lösungen zurück (Front)
    '*******************************************************************************
    Private Function Non_Dominated_Count_and_Sort(ByRef NDSorting() As Struct_NDSorting) As Short

        Dim i As Short
        Dim Temp() As Struct_NDSorting
        Dim counter As Short
        Dim NFrontMember As Short

        ReDim Temp(UBound(NDSorting))
        Call Temp(0).Dimit(Initial.NPenalty, Initial.NConstrains, Initial.varanz, Temp)

        NFrontMember = 0
        counter = 0

        'Die nicht dominanten Lösungen werden nach oben kopiert
        For i = 1 To UBound(NDSorting)
            If (NDSorting(i).dominated = True) Then
                counter += 1
                call temp(0).Copy(NDSorting(i),Temp(counter))
            End If
        Next i

        'Zahl der dominanten Lösungen wird errechnet
        NFrontMember = UBound(NDSorting) - counter

        'Die dominanten Lösungen werden nach unten kopiert
        For i = 1 To UBound(NDSorting)
            If (NDSorting(i).dominated = False) Then
                counter += 1
                Call Temp(0).Copy(NDSorting(i), Temp(counter))
            End If
        Next i

        Call Temp(0).Copy(Temp, NDSorting)

        Return NFrontMember

    End Function

    'NON_DOMINATED_COUNT_AND_SORT_SEKUNDÄRE_POPULATION
    'Sortiert die nicht dominanten Lösungen nach oben, die dominanten nach unten
    'Gibt die Zahl der dominanten Lösungen zurück (Front) hier für die Sekundäre Population
    '**************************************************************************************
    Private Function Non_Dominated_Count_and_Sort_Sekundäre_Population(ByRef NDSorting() As Struct_NDSorting) As Short

        Dim i As Short
        Dim Temp() As Struct_NDSorting
        Dim counter As Short
        Dim NFrontMember As Short

        ReDim Temp(UBound(NDSorting))
        Call Temp(0).Dimit(Initial.NPenalty, Initial.NConstrains, Initial.varanz, Temp)

        NFrontMember = 0
        counter = 0

        For i = 1 To UBound(NDSorting)
            If (NDSorting(i).dominated = False) Then
                counter += 1
                Call Temp(0).Copy(NDSorting(i), Temp(counter))
            End If
        Next i

        NFrontMember = counter

        For i = 1 To UBound(NDSorting)
            If (NDSorting(i).dominated = True) Then
                counter += 1
                Call Temp(0).Copy(NDSorting(i), Temp(counter))
            End If
        Next i

        Call Temp(0).Copy(Temp, NDSorting)

        Return NFrontMember

    End Function

    'NON_DOMINATED_RESULT - Hier wird pro durchlauf die nicht dominierte Front in NDSResult
    'geschaufelt und die bereits klassifizierten Lösungen aus Temp Array gelöscht
    '**************************************************************************************
    Private Sub Non_Dominated_Result(ByRef Temp() As Struct_NDSorting, ByRef NDSResult() As Struct_NDSorting, ByVal NFrontMember_aktuell As Short, ByVal NFrontMember_gesamt As Short)

        Dim i, Position As Short

        Position = NFrontMember_gesamt - NFrontMember_aktuell + 1

        'In NDSResult werden die nicht dominierten Lösungen eingefügt
        For i = UBound(Temp) + 1 - NFrontMember_aktuell To UBound(Temp)
            'NDSResult alle bisher gefundene Fronten
            Call Temp(0).Copy(Temp(i), NDSResult(Position))
            Position += 1
        Next i

        'Die bereits klassifizierten Member werden aus dem Temp Array gelöscht
        If (PES_Settings.NNachf + PES_Settings.NEltern - NFrontMember_gesamt > 0) Then
            ReDim Preserve Temp(PES_Settings.NNachf + PES_Settings.NEltern - NFrontMember_gesamt)
            'Der Flag wird zur klassifizierung in der nächsten Runde zurückgesetzt
            For i = 1 To UBound(Temp)
                Temp(i).dominated = False
            Next i
        End If

    End Sub

    'COUNT_FRONT_MEMBERS
    '*******************
    Private Function Count_Front_Members(ByVal aktuell_Front As Short, ByRef NDSResult() As Struct_NDSorting) As Integer

        Dim i As Short
        Count_Front_Members = 0

        For i = 1 To UBound(NDSResult)
            If (NDSResult(i).Front = aktuell_Front) Then
                Count_Front_Members += 1
            End If
        Next i

    End Function

    'NDS_Crowding_Distance_Sort
    '**************************
    Private Sub NDS_Crowding_Distance_Sort(ByRef NDSorting() As Struct_NDSorting, ByVal start As Short, ByVal ende As Short)

        Dim i As Integer
        Dim j As Integer
        Dim k As Short

        Call swap.Dimit(Initial.NPenalty, Initial.NConstrains, Initial.varanz, swap)

        Dim fmin, fmax As Double

        For k = 1 To Initial.NPenalty
            For i = start To ende
                For j = start To ende
                    If (NDSorting(i).penalty(k) < NDSorting(j).penalty(k)) Then
                        Call NDSorting(0).Copy(NDSorting(i), swap)
                        Call NDSorting(0).Copy(NDSorting(j), NDSorting(i))
                        Call NDSorting(0).Copy(swap, NDSorting(j))
                    End If
                Next j
            Next i

            fmin = NDSorting(start).penalty(k)
            fmax = NDSorting(ende).penalty(k)

            NDSorting(start).distance = 1.0E+300
            NDSorting(ende).distance = 1.0E+300

            For i = start + 1 To ende - 1
                NDSorting(i).distance = NDSorting(i).distance + (NDSorting(i + 1).penalty(k) - NDSorting(i - 1).penalty(k)) / (fmax - fmin)
            Next i
        Next k

        For i = start To ende
            For j = start To ende
                If (NDSorting(i).distance > NDSorting(j).distance) Then

                    'Call swap.NDSorting_Copy(NDSorting(i), swap)

                    Call swap.Copy(NDSorting(i), swap)
                    Call swap.Copy(NDSorting(j), NDSorting(i))
                    Call swap.Copy(swap, NDSorting(j))
                End If
            Next j
        Next i

    End Sub

    'NDS_Crowding_Distance_Count
    '***************************
    Private Function NDS_Crowding_Distance_Count(ByRef Spannweite As Double) As Double

        Dim i As Short
        Dim j As Short
        Dim k As Short
        Dim TempDistance() As Double
        Dim PenaltyDistance(,) As Double

        Dim d() As Double
        Dim d_mean As Double

        ReDim TempDistance(Initial.NPenalty)
        ReDim PenaltyDistance(PES_Settings.NEltern, PES_Settings.NEltern)
        ReDim d(PES_Settings.NEltern - 1)

        'Bestimmen der normierten Raumabstände zwischen allen Elternindividuen
        For i = 1 To PES_Settings.NEltern
            PenaltyDistance(i, i) = 0
            For j = i + 1 To PES_Settings.NEltern
                PenaltyDistance(i, j) = 0
                For k = 1 To Initial.NPenalty
                    TempDistance(k) = Qb(i, PES_iAkt.iAktPop, k) - Qb(j, PES_iAkt.iAktPop, k)
                    TempDistance(k) = TempDistance(k) * TempDistance(k)
                    PenaltyDistance(i, j) = PenaltyDistance(i, j) + TempDistance(k)
                Next k
                PenaltyDistance(i, j) = System.Math.Sqrt(PenaltyDistance(i, j))
                PenaltyDistance(j, i) = PenaltyDistance(i, j)
            Next j
        Next i

        d_mean = 0

        For i = 1 To PES_Settings.NEltern - 1
            d(i) = 1.0E+300
            For j = 1 To i - 1
                If (PenaltyDistance(i, j) < d(i)) Then d(i) = PenaltyDistance(i, j)
            Next j
            For j = i + 1 To PES_Settings.NEltern
                If (PenaltyDistance(i, j) < d(i)) Then d(i) = PenaltyDistance(i, j)
            Next j
            d_mean = d_mean + d(i)
        Next i

        d_mean = d_mean / PES_Settings.NEltern

        NDS_Crowding_Distance_Count = 0

        For i = 1 To PES_Settings.NEltern - 1
            NDS_Crowding_Distance_Count = NDS_Crowding_Distance_Count + (d_mean - d(i)) * (d_mean - d(i))
        Next i

        NDS_Crowding_Distance_Count = NDS_Crowding_Distance_Count / PES_Settings.NEltern

        NDS_Crowding_Distance_Count = System.Math.Sqrt(NDS_Crowding_Distance_Count)

        Spannweite = 0
        For i = 1 To PES_Settings.NEltern
            'TODO: sollte hier nicht j = i + 1 stehen?
            For j = i To PES_Settings.NEltern
                If PenaltyDistance(i, j) > Spannweite Then Spannweite = PenaltyDistance(i, j)
            Next j
        Next i

    End Function

    'Neighbourhood_AbstandsArray - Bestimme Array der Raumabstände für Neighbourhood-Rekombination
    '*********************************************************************************************
    Private Sub Neighbourhood_AbstandsArray()

        Dim i As Short
        Dim j As Short
        Dim k As Short
        Dim MinMax() As Double
        Dim Min, Max As Double
        Dim TempDistance() As Double

        'Bestimmen des Normierungsfaktors für jede Dimension des Lösungsraums (MinMax)
        ReDim MinMax(Initial.NPenalty)
        For k = 1 To Initial.NPenalty
            MinMax(k) = 0
            Min = Qb(1, PES_iAkt.iAktPop, k)
            Max = Qb(1, PES_iAkt.iAktPop, k)
            For j = 1 To PES_Settings.NEltern
                If (Min > Qb(j, PES_iAkt.iAktPop, k)) Then Min = Qb(j, PES_iAkt.iAktPop, k)
                If (Max < Qb(j, PES_iAkt.iAktPop, k)) Then Max = Qb(j, PES_iAkt.iAktPop, k)
            Next j
            MinMax(k) = Max - Min
        Next k

        'Bestimmen der normierten Raumabstände zwischen allen Elternindividuen
        ReDim TempDistance(Initial.NPenalty)

        For i = 1 To PES_Settings.NEltern

            PenaltyDistance(i, i) = 0

            For j = i + 1 To PES_Settings.NEltern

                PenaltyDistance(i, j) = 0

                For k = 1 To Initial.NPenalty

                    TempDistance(k) = Qb(i, PES_iAkt.iAktPop, k) - Qb(j, PES_iAkt.iAktPop, k)
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

    'SekundärQb_Dubletten
    '********************
    Private Sub SekundärQb_Dubletten()

        Dim i, j, k As Short
        Dim Logical As Boolean

        For i = 1 To UBound(SekundärQb) - 1
            For j = i + 1 To UBound(SekundärQb)
                Logical = True
                For k = 1 To Initial.NPenalty
                    Logical = Logical And (SekundärQb(i).penalty(k) = SekundärQb(j).penalty(k))
                Next k
                If (Logical) Then SekundärQb(i).dominated = True
            Next j
        Next i
    End Sub

    'Neighbourhood_Eltern
    'Bestimme die NAnzahlEltern mit geringsten Raumabständen für Neighbourhood-Rekombination
    '***************************************************************************************
    Private Sub Neighbourhood_Eltern(ByVal IndexElter As Short)

        Dim i As Short
        Dim j As Short
        Dim Nachbarn() As Struct_Neighbourhood
        Dim swap As Struct_Neighbourhood

        ReDim Nachbarn(PES_Settings.NEltern - 1)

        For i = 1 To IndexElter - 1
            Nachbarn(i).distance = PenaltyDistance(IndexElter, i)
            Nachbarn(i).Index = i
        Next i
        For i = IndexElter + 1 To PES_Settings.NEltern
            Nachbarn(i - 1).distance = PenaltyDistance(IndexElter, i)
            Nachbarn(i - 1).Index = i
        Next i

        For i = 1 To UBound(Nachbarn)
            For j = i To UBound(Nachbarn)
                If (Nachbarn(i).distance > Nachbarn(j).distance) Then
                    swap = Nachbarn(i)
                    Nachbarn(i) = Nachbarn(j)
                    Nachbarn(j) = swap
                End If
            Next
        Next

        For i = 1 To PES_Settings.NRekombXY
            IndexEltern(i) = Nachbarn(i).Index
        Next i

    End Sub

    'Neighbourhood_Crowding_Distance
    'Bestimme die NAnzahlEltern mit geringsten Raumabständen für Neighbourhood-Rekombination
    '***************************************************************************************
    Private Sub Neighbourhood_Crowding_Distance()

        Dim i As Integer
        Dim j As Integer
        Dim k As Short
        Dim QbTemp(,,) As Double
        Dim swap As Double
        Dim fmin, fmax As Double

        ReDim QbTemp(PES_Settings.NEltern, PES_Settings.NPopul, Initial.NPenalty)

        Array.Copy(Qb, QbTemp, Qb.GetLength(0))
        For i = 1 To PES_Settings.NEltern
            Distanceb(i) = 0
        Next i

        For k = 1 To Initial.NPenalty
            For i = 1 To PES_Settings.NEltern
                For j = 1 To PES_Settings.NEltern
                    If (QbTemp(i, PES_iAkt.iAktPop, k) < QbTemp(j, PES_iAkt.iAktPop, k)) Then
                        swap = QbTemp(i, PES_iAkt.iAktPop, k)
                        QbTemp(i, PES_iAkt.iAktPop, k) = QbTemp(j, PES_iAkt.iAktPop, k)
                        QbTemp(j, PES_iAkt.iAktPop, k) = swap
                    End If
                Next j
            Next i

            fmin = QbTemp(1, PES_iAkt.iAktPop, k)
            fmax = QbTemp(PES_Settings.NEltern, PES_iAkt.iAktPop, k)

            Distanceb(1) = 1.0E+300
            Distanceb(PES_Settings.NEltern) = 1.0E+300

            For i = 2 To PES_Settings.NEltern - 1
                Distanceb(i) = Distanceb(i) + (QbTemp(i + 1, PES_iAkt.iAktPop, k) - QbTemp(i - 1, PES_iAkt.iAktPop, k)) / (fmax - fmin)
            Next i
        Next k

    End Sub


End Class
