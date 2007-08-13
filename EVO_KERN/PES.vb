Option Strict Off
Option Explicit On
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
	'*******************************************************************************
	
    Public Structure Struct_PES_Options
        Dim NEltern As Short                'Anzahl Eltern
        Dim NNachf As Short                 'Anzahl Kinder
        Dim NGen As Short                   'Anzahl Generationen
        Dim iEvoTyp As Short                'Typ der Evolutionsstrategie (+ oder ,)
        Dim iPopEvoTyp As Short             'Typ der Evolutionsstrategie (+ oder ,) auf Populationsebene
        Dim iPopPenalty As Short            'Art der Beurteilung der Populationsgüte (Multiobjective)
        Dim isPOPUL As Boolean              'Mit Populationen
        Dim isMultiObjective As Boolean     'Mit zweiter Objective-function
        Dim isPareto As Boolean             'mit Non-Dominated Sorting (nur falls multiobjective)
        Dim isPareto3D As Boolean           'mit Non-Dominated Sorting (3 Objectives)
        Dim NRunden As Short                'Anzahl Runden
        Dim NPopul As Short                 'Anzahl Populationen
        Dim NPopEltern As Short             'Anzahl Populationseltern
        Dim iOptPopEltern As Short          'Ermittlung der Populationseltern (Mittelwert, Rekombination, Selektion)
        Dim iOptEltern As Short             'Ermittlung der Individuum-Eltern (Mittelwert, Rekombination, einfache Auswahl)
        Dim NRekombXY As Short              'X/Y-Schema Rekombination
        Dim rDeltaStart As Single           'Startschrittweite
        Dim iStartPar As Short              'Startaparameter (zufällig, Originalparameter)
        Dim isDnVektor As Boolean           'Soll ein Schrittweitenvektor benutz werden
        Dim interact As Short               'Alle wieviel Generationen soll die aktuelle Population mit Mitgliedern der sekundären Population aufgefüllt werden
        Dim isInteract As Boolean           'Mit Austausch zwischen Population und Sekundärer Population
        Dim NMemberSecondPop As Short       'Maximale Anzahl Mitglieder der Sekundärpopulation

        Dim varanz As Short                 'Anzahl Parameter
        Dim NPenalty As Short               'Anzahl der Penaltyfunktionen
        Dim NConstrains As Short            'Anzahl der Randbedingungen
        Dim Dn() As Double                  'Schrittweitenvektor
        Dim Xn() As Double                  'aktuelle Variablenwerte
        Dim Xmin() As Double                'untere Schranke
        Dim Xmax() As Double                'Obere Schranke
        Dim iaktuelleRunde As Short         'Zähler für aktuelle Runde
        Dim iaktuellePopulation As Short    'Zähler für aktuelle Population
        Dim iaktuelleGeneration As Short    'Zähler für aktuelle Generation
        Dim iaktuellerNachfahre As Short    'Zähler für aktuellen Nachfahre
    End Structure

    Dim PES_Options As Struct_PES_Options

    Private Xp(,,) As Double                'PopulationsElternwert der Variable
    Private Dp(,,) As Double                'PopulationsElternschrittweite
    Private Xbpop(,,) As Double             'Bestwertspeicher Variablenwerte Generationsebene
    Private Dbpop(,,) As Double             'Bestwertspeicher Schrittweite Generationsebene
    Private Qbpop(,) As Double              'Bestwertspeicher auf Populationsebene
    Private QbpopD() As Double              'Bestwertspeicher Crowdings Distance
    '---------------------
    Private Xe(,,) As Double                'Elternwerte der Variablen
    Private De(,,) As Double                'Elternschrittweite
    Private Xb(,,) As Double                'Bestwertspeicher Variablenwerte Generationsebene
    Private Db(,,) As Double                'Bestwertspeicher Schrittweite Generationsebene
    Private Qb(,,) As Double                'Bestwertspeicher Generationsebene
    Private Rb(,,) As Double                'Restriktionen auf Generationsebene
    '---------------------
    Private SekundärQb() As Struct_NDSorting = {}   'Sekundäre Population
    Private expo As Short                   'Exponent für Schrittweite (+/-1)
    Private DnTemp As Double                'Temporäre Schrittweite für Nachkomme
    Private XnTemp As Double                'Temporärer Parameterwert für Nachkomme
    Private DeTemp As Double                'Temporäre Schrittweite für Elter
    Private XeTemp As Double                'Temporäre Parameterwert für Elter
    Private R As Short
    Private Z As Double
    Private PenaltyDistance(,) As Double    'Array für normierte Raumabstände (Neighbourhood-Rekomb.)
    Private IndexEltern() As Short          'Array mit Index der Eltern (Neighbourhood-Rekomb.)
    Private Distanceb() As Double           'Array mit Crowding-Distance (Neighbourhood-Rekomb.)

    Const galpha As Double = 1.3            'Faktor alpha=1.3 auf Generationsebene nach Rechenberg
    Const palpha As Double = 1.1            'Faktor alpha=1.1 auf Populationsebene nach Rechenberg

    '*******************************************************************************
    'Deklarationsteil für Non-Dominated Sorting
    '*******************************************************************************

    Private Structure Struct_NDSorting
        Dim penalty() As Double             'Werte der Penaltyfunktion(en)
        Dim constrain() As Double           'Werte der Randbedingung(en)
        Dim feasible As Boolean             'Gültiges Ergebnis ?
        Dim dominated As Boolean            'Kennzeichnung ob dominiert
        Dim Front As Short                  'Nummer der Pareto Front
        Dim X() As Double                   'Wert der Variablen
        Dim d() As Double                   'Schrittweite der Variablen
        Dim distance As Double              'Distanzwert für Crowding distance sort
    End Structure

    Dim List_NDSorting() As Struct_NDSorting

    Private Structure Struct_Sortierung
        Dim Index As Short
        Dim penalty() As Double
    End Structure

    Private Structure Struct_Neighbourhood
        Dim distance As Double
        Dim Index As Short
    End Structure

    '*******************************************************************************
    'Deklarationsteil allgemein
    '*******************************************************************************

    'Evo-Strategie-Typ:
    Private Const EVO_PLUS As Short = 1
    Private Const EVO_KOMMA As Short = 2

    'Property NNachf
    '***********************************************
    Public Property NNachf() As Short
        Get
            Return PES_Options.NNachf
        End Get
        Set(ByVal Value As Short)
            PES_Options.NNachf = Value
        End Set
    End Property

    'Property NGen
    '***********************************************
    Public Property NGen() As Short
        Get
            Return PES_Options.NGen
        End Get
        Set(ByVal Value As Short)
            PES_Options.NGen = Value
        End Set
    End Property

    'Property isMultiObjective
    '***********************************************
    Public Property isMultiObjective() As Boolean
        Get
            Return PES_Options.isMultiObjective
        End Get
        Set(ByVal Value As Boolean)
            PES_Options.isMultiObjective = Value
        End Set
    End Property

    'Property NRunden
    '***********************************************
    Public Property NRunden() As Short
        Get
            Return PES_Options.NRunden
        End Get
        Set(ByVal Value As Short)
            PES_Options.NRunden = Value
        End Set
    End Property

    'Property NPopul
    '***********************************************
    Public Property NPopul() As Short
        Get
            Return PES_Options.NPopul
        End Get
        Set(ByVal Value As Short)
            PES_Options.NPopul = Value
        End Set
    End Property

    'Property iaktuelleRunde
    '***********************************************
    Public Property iaktuelleRunde() As Short
        Get
            Return PES_Options.iaktuelleRunde
        End Get
        Set(ByVal Value As Short)
            PES_Options.iaktuelleRunde = Value
        End Set
    End Property

    'Property iaktuelleGeneration
    '***********************************************
    Public Property iaktuelleGeneration() As Short
        Get
            Return PES_Options.iaktuelleGeneration
        End Get
        Set(ByVal Value As Short)
            PES_Options.iaktuelleGeneration = Value
        End Set
    End Property

    'Property iaktuellePopulation
    '***********************************************
    Public Property iaktuellePopulation() As Short
        Get
            Return PES_Options.iaktuellePopulation
        End Get
        Set(ByVal Value As Short)
            PES_Options.iaktuellePopulation = Value
        End Set
    End Property

    'Property iaktuellerNachfahre
    '***********************************************
    Public Property iaktuellerNachfahre() As Short
        Get
            Return PES_Options.iaktuellerNachfahre
        End Get
        Set(ByVal Value As Short)
            PES_Options.iaktuellerNachfahre = Value
        End Set
    End Property

    '*******************************************************************************
    '*******************************************************************************
    'Funktionen
    '*******************************************************************************
    '*******************************************************************************

    '*******************************************************************************
    'ES_INI
    '*******************************************************************************

    'Function ES_INI Initialisiert benötigte dynamische Arrays und legt Anzahl
    'der Zielfunktionen fest
    Public Function EsIni(ByRef AnzahlParameter As Short, ByRef AnzahlPenaltyfunktionen As Short, ByRef AnzahlRandbedingungen As Short) As Boolean

        EsIni = False

        On Error GoTo ES_INI_ERROR

        'Überprüfung der Eingabeparameter (es muss mindestens ein Parameter variiert und eine
        'Penaltyfunktion ausgewertet werden)

        If AnzahlParameter <= 0 Or AnzahlPenaltyfunktionen <= 0 Then GoTo ES_INI_ERROR

        PES_Options.varanz = AnzahlParameter 'Anzahl der Parameter wird übergeben
        PES_Options.NPenalty = AnzahlPenaltyfunktionen 'Anzahl der Zielfunktionen wird
        'übergeben
        PES_Options.NConstrains = AnzahlRandbedingungen 'Anzahl der Randbedingungen wird
        'übergeben

        ReDim PES_Options.Xn(PES_Options.varanz) 'Variablenvektor wird initialisiert
        ReDim PES_Options.Xmin(PES_Options.varanz) 'UntereSchrankenvektor wird initialisiert
        ReDim PES_Options.Xmax(PES_Options.varanz) 'ObereSchrankenvektor wird initialisiert
        ReDim PES_Options.Dn(PES_Options.varanz) 'Schrittweitenvektor wird initialisiert

        EsIni = True
        Exit Function

ES_INI_ERROR:
        Exit Function

    End Function

    '*******************************************************************************
    'ES_OPTIONS
    '*******************************************************************************

    'Function ES_OPTIONS übergibt Optionen für Evolutionsstrategie und Prüft die eingestellten Optionen
    Public Function EsOptions(ByRef iEvoTyp As Integer, ByRef iPopEvoTyp As Integer, ByRef isPOPUL As Boolean, ByRef NRunden As Integer, ByRef NPopul As Integer, ByRef NPopEltern As Integer, ByRef iOptPopEltern As Integer, ByRef iOptEltern As Integer, ByRef iPopPenalty As Integer, ByRef NGen As Integer, ByRef NEltern As Integer, ByRef NNachf As Integer, ByRef NRekombXY As Integer, ByRef rDeltaStart As Single, ByRef iStartPar As Integer, ByRef isDnVektor As Boolean, ByRef isMultiObjective As Boolean, ByRef isPareto As Boolean, ByRef isPareto3D As Boolean, ByRef interact As Short, ByRef isInteract As Boolean, ByRef NMemberSecondPop As Short, ByRef globalAnzPar as Short) As Boolean

        EsOptions = False

        On Error GoTo ES_OPTIONS_ERROR

        'Überprüfung der Übergebenen Werte
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        If (iEvoTyp < 1 Or iEvoTyp > 2) Then
            Throw New Exception("Typ der Evolutionsstrategie ist nicht '+' oder ','")
        End If
        If (iPopEvoTyp < 1 Or iPopEvoTyp > 2) Then
            Throw New Exception("Typ der Evolutionsstrategie auf Pupulationsebene ist nicht '+' oder ','")
        End If
        If NRunden < 1 Then
            Throw New Exception("Die Anzahl der Runden ist kleiner 1")
        End If
        If NPopul < 1 Then
            Throw New Exception("Die Anzahl der Populationen ist kleiner 1")
        End If
        If NPopEltern < 1 Then
            Throw New Exception("Die Anzahl der Populationseltern ist kleiner 1")
        End If
        If (iOptPopEltern < 1 Or iOptPopEltern > 3) Then
            Throw New Exception("Ermittlung der Populationseltern ist nicht Mittelwert, Rekombination oder Selektion!")
        End If
        If (iOptEltern < 1 Or iOptEltern > 6) Then
            Throw New Exception("Strategie zur Ermittlung der Eltern ist nicht möglich!")
        End If
        If NEltern < 1 Then
            Throw New Exception("Die Anzahl der Eltern ist kleiner 1!")
        End If
        If NNachf < 1 Then
            Throw New Exception("Die Anzahl der Nachfahren ist kleiner 1!")
        End If
        If NGen < 1 Then
            Throw New Exception("Die Anzahl der Generationen ist kleiner 1!")
        End If
        If NRekombXY < 1 Then
            Throw New Exception("Der Wert für die X/Y-Schema Rekombination ist kleiner 1!")
        End If
        If rDeltaStart < 0 Then
            Throw New Exception("Die Startschrittweite darf nicht kleiner 0 sein!")
        End If
        If (iStartPar < 1 Or iStartPar > 2) Then
            Throw New Exception("Die Startaparameter dürfen nur zufällig sein oder aus den Originalparameter bestehen!")
        End If
        If NPopul < NPopEltern Then
            Throw New Exception("Die Anzahl der Populationseltern darf nicht größer als die Anzahl der Populationen!")
        End If
        If NNachf <= NEltern Then
            Throw New Exception("Die Anzahl der Eltern kann nicht größer als die Anzahl der Nachfahren sein!" & Chr(13) & Chr(10) & "'Rechenberg 73' schlägt ein Verhältnis von 1:3 bis 1:5 vor.")
        End If
        If globalAnzPar < 1 Then
            Throw New Exception("Die Anzahl der Parameter ist kleiner 1!")
        End If

        'Übergabe der Optionen
        'xxxxxxxxxxxxxxxxxxxxx
        PES_Options.NEltern = NEltern
        PES_Options.NNachf = NNachf
        PES_Options.NGen = NGen
        PES_Options.iEvoTyp = iEvoTyp
        PES_Options.iPopEvoTyp = iPopEvoTyp
        PES_Options.iPopPenalty = iPopPenalty
        PES_Options.isPOPUL = isPOPUL
        PES_Options.isMultiObjective = isMultiObjective
        PES_Options.isPareto = isPareto
        PES_Options.isPareto3D = isPareto3D
        PES_Options.NRunden = NRunden
        PES_Options.NPopul = NPopul
        PES_Options.NPopEltern = NPopEltern
        PES_Options.iOptPopEltern = iOptPopEltern
        PES_Options.iOptEltern = iOptEltern
        PES_Options.NRekombXY = NRekombXY
        PES_Options.rDeltaStart = rDeltaStart
        PES_Options.iStartPar = iStartPar
        PES_Options.isDnVektor = isDnVektor
        PES_Options.isInteract = isInteract
        PES_Options.interact = interact
        PES_Options.NMemberSecondPop = NMemberSecondPop

        If Not PES_Options.isPOPUL Then
            PES_Options.NPopul = 1
            PES_Options.NPopEltern = 1
            PES_Options.NRunden = 1
        End If

        EsOptions = True
        Exit Function

ES_OPTIONS_ERROR:
        Exit Function

    End Function

    '*******************************************************************************
    'ES_LET_PARAMETER
    '*******************************************************************************

    'Function ES_LET_PARAMETER dient zur Übergabe der Ausgangsparameter
    'an die Evolutionsstrategie, i ist der Index des Parameters,
    'es wird genau ein Parameter übergeben
    Public Function EsLetParameter(ByRef i As Integer, ByRef Parameter As Double) As Boolean

        EsLetParameter = False

        On Error GoTo ES_LET_PARAMETER_ERROR

        PES_Options.Xn(i) = Parameter
        PES_Options.Xmin(i) = 0
        PES_Options.Xmax(i) = 1
        PES_Options.Xn(i) = BestimmeMinWert(PES_Options.Xn(i), PES_Options.Xmax(i))
        PES_Options.Xn(i) = BestimmeMaxWert(PES_Options.Xn(i), PES_Options.Xmin(i))

        EsLetParameter = True
        Exit Function

ES_LET_PARAMETER_ERROR:
        Exit Function

    End Function

    '*******************************************************************************
    'ES_GET_PARAMETER
    '*******************************************************************************

    'Function ES_GET_PARAMETER dient zur Übergabe der mutierten Parameter
    'Alle Parameter werden in ein Array geschrieben
    'globalAnzPar ist die Anzahl der mutierten Parameter,

    Public Function EsGetParameter(ByRef globalAnzPar As Short, ByRef mypara(,) As Double) As Boolean
        Dim i As Short

        EsGetParameter = False

        On Error GoTo ES_GET_PARAMETER_ERROR

        For i = 1 To globalAnzPar
            mypara(i, 1) = PES_Options.Xn(i)
        Next i

        EsGetParameter = True
        Exit Function

ES_GET_PARAMETER_ERROR:
        Exit Function
    End Function

    '*******************************************************************************
    'ES_GET_SCHRITTWEITE
    '*******************************************************************************

    'Function ES_GET_SCHRITTWEITE gibt die aktuellen Schrittweiten aus

    Public Function EsGetSchrittweite(ByRef globalAnzPar As Short, ByRef mystep() As Double) As Boolean
        Dim i As Short

        EsGetSchrittweite = False

        On Error GoTo ES_GET_SCHRITTWEITE_ERROR

        For i = 1 To globalAnzPar
            mystep(i) = PES_Options.Dn(i)
        Next i

        EsGetSchrittweite = True

ES_GET_SCHRITTWEITE_ERROR:
        Exit Function
    End Function

    '*******************************************************************************
    'ES_GET_BESTWERT
    '*******************************************************************************

    'Function ES_GET_BESTWERT gibt den kompletten Bestwertspeicher aus
    'Bestwert(,) muss ein dynamisches Array sein

    Public Function EsGetBestwert(ByRef Bestwert(,) As Double) As Boolean

        Dim i, j As Short

        EsGetBestwert = False

        ReDim Bestwert(PES_Options.NEltern, PES_Options.NPenalty)
        For i = 1 To PES_Options.NPenalty ' dm 29.04.05
            For j = 1 To PES_Options.NEltern
                Bestwert(j, i) = Qb(j, PES_Options.iaktuellePopulation, i)
            Next j
        Next i

        EsGetBestwert = True

    End Function

    '*******************************************************************************
    'ES_GET_SEKUNDÄRE_POPULATIONEN
    'Sekundäre Population speichert immer die angegebene Anzahl von Bestwerten und
    'kann den Bestwertspeicher alle x Generationen überschreiben
    '*******************************************************************************

    Public Function esGetSekundärePopulation(ByRef Population(,) As Double) As Boolean

        Dim j, i As Short

        esGetSekundärePopulation = False

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

        ReDim Population(UBound(SekundärQb), PES_Options.NPenalty)
        '!Wenn Fehler hier "SekundäreQb = Nothing" auftritt wurde TeeChart mit der falschen Serie bzw. zu wenig Serien gestartet!!!

        For i = 1 To UBound(SekundärQb)
            For j = 1 To PES_Options.NPenalty
                Population(i, j) = SekundärQb(i).penalty(j)
            Next j
        Next i

        esGetSekundärePopulation = True

    End Function

    '*******************************************************************************
    'ES_GET_POP_BESTWERT
    '*******************************************************************************

    'Function ES_GET_POP_BESTWERT gibt den kompletten Bestwertspeicher aus
    'Bestwert() muss ein dynamisches Array sein
    'TODO: diese Funktion wird derzeit nicht verwendet

    Public Function EsGetPopBestWert(ByRef POP_Bestwert(,,,) As Double) As Boolean

        Dim k, i, j, l As Short

        ReDim POP_Bestwert(PES_Options.NPopul, PES_Options.NEltern, PES_Options.varanz, PES_Options.NPenalty)
        ReDim POP_Bestwert(PES_Options.NPopul, PES_Options.NEltern, PES_Options.varanz, PES_Options.NPenalty + 1)
        For i = 1 To PES_Options.NPopul
            For j = 1 To PES_Options.NEltern
                For k = 1 To PES_Options.varanz
                    POP_Bestwert(i, j, k, 1) = Xb(k, j, i)
                    For l = 1 To PES_Options.NPenalty
                        POP_Bestwert(i, j, k, l + 1) = Qb(j, i, l)
                    Next l
                Next k
            Next j
        Next i

    End Function



    '*******************************************************************************
    'ES_PREPARE
    '*******************************************************************************

    'Function ES_PREPARE initialisiert alle internen Arrays und setzt den
    'Bestwertspeicher auf sehr großen Wert (Strategie minimiert in dieser
    'Umsetzung)
    'TODO: ESPrepare Für Paretooptimierung noch nicht fertig!!!!

    Public Function EsPrepare() As Boolean
        Dim m, n, l As Short
        Dim w, v, i As Short

        EsPrepare = False

        On Error GoTo ES_PREPARE_ERROR

        For i = 1 To PES_Options.varanz
            PES_Options.Dn(i) = PES_Options.rDeltaStart
        Next i

        'Parametervektoren initialisieren
        ReDim Dp(PES_Options.varanz, PES_Options.NEltern, PES_Options.NPopEltern)
        ReDim Xp(PES_Options.varanz, PES_Options.NEltern, PES_Options.NPopEltern)
        ReDim Qbpop(PES_Options.NPopul, PES_Options.NPenalty)
        ReDim QbpopD(PES_Options.NPopul)
        ReDim Dbpop(PES_Options.varanz, PES_Options.NEltern, PES_Options.NPopul)
        ReDim Xbpop(PES_Options.varanz, PES_Options.NEltern, PES_Options.NPopul)
        '---------------------
        ReDim De(PES_Options.varanz, PES_Options.NEltern, PES_Options.NPopul)
        ReDim Xe(PES_Options.varanz, PES_Options.NEltern, PES_Options.NPopul)
        ReDim Db(PES_Options.varanz, PES_Options.NEltern, PES_Options.NPopul)
        ReDim Xb(PES_Options.varanz, PES_Options.NEltern, PES_Options.NPopul)
        ReDim Qb(PES_Options.NEltern, PES_Options.NPopul, PES_Options.NPenalty)
        ReDim Rb(PES_Options.NEltern, PES_Options.NPopul, PES_Options.NPenalty)
        '---------------------
        'NDSorting wird nur benötigt, falls eine Paretofront approximiert wird
        If PES_Options.isPareto Then
            ReDim List_NDSorting(PES_Options.NEltern + PES_Options.NNachf)
            For i = 1 To PES_Options.NEltern + PES_Options.NNachf
                ReDim List_NDSorting(i).penalty(PES_Options.NPenalty)
                If PES_Options.NConstrains > 0 Then
                    ReDim List_NDSorting(i).constrain(PES_Options.NConstrains)
                End If
                ReDim List_NDSorting(i).d(PES_Options.varanz)
                ReDim List_NDSorting(i).X(PES_Options.varanz)
            Next i
            If PES_Options.iOptEltern = EVO_ELTERN_Neighbourhood Then
                ReDim PenaltyDistance(PES_Options.NEltern, PES_Options.NEltern)
                ReDim IndexEltern(PES_Options.NEltern - 1)
                ReDim Distanceb(PES_Options.NEltern)
            End If
        End If

        For n = 1 To PES_Options.NEltern
            For m = 1 To PES_Options.NPopul
                For l = 1 To PES_Options.NPenalty
                    'Qualität der Eltern (Anzahl = parents) wird auf sehr großen Wert gesetzt
                    Qb(n, m, l) = 1.0E+300
                Next l
                If PES_Options.NConstrains > 0 Then
                    For l = 1 To PES_Options.NPenalty
                        'Restriktion der Eltern (Anzahl = parents) wird auf sehr kleinen Wert gesetzt
                        Rb(n, m, l) = -1.0E+300
                    Next l
                End If
            Next m
        Next

        If PES_Options.isMultiObjective And PES_Options.isPareto Then
            For n = 1 To PES_Options.NPopul
                For m = 1 To PES_Options.NPenalty
                    Select Case PES_Options.iPopPenalty
                        Case 1 'Crowding
                            'Qualität der Populationseltern wird 0 gesetzt
                            Qbpop(n, m) = 1.0E+300
                        Case 2 'Spannweite
                            'Qualität der Populationseltern wird auf sehr großen Wert gesetzt
                            Qbpop(n, m) = 0
                    End Select
                Next m
            Next n
        Else
            For n = 1 To PES_Options.NPopul
                For m = 1 To PES_Options.NPenalty
                    'Qualität der Populationseltern wird auf sehr großen Wert gesetzt
                    Qbpop(n, m) = 1.0E+300
                Next m
            Next n
        End If

        'Zufallsgenerator initialisieren
        Randomize()

        'Informationen über aktuelle Runden übergeben
        PES_Options.iaktuelleRunde = 0
        PES_Options.iaktuellePopulation = 0
        PES_Options.iaktuelleGeneration = 0
        PES_Options.iaktuellerNachfahre = 0

        EsPrepare = True
        Exit Function

ES_PREPARE_ERROR:
        Exit Function

    End Function

    '*******************************************************************************
    'ES_STARTVALUES
    '*******************************************************************************

    'Function ES_STARTVALUES setzt die Startwerte
    'Option 1: Zufällige Startwert -> Schrittweite = Startschrittweite
    '                              -> Parameterwert = zufällig [0,1]
    'Option 2: Originalparameter   -> Schrittweite = Startschrittweite
    '                              -> Parameterwert = Originalparameter

    Public Function EsStartvalues() As Boolean
        Dim Zufallszahl01 As Single
        Dim w, n, v, m As Short

        EsStartvalues = False

        On Error GoTo ES_STARTVALUES_ERROR

        Select Case PES_Options.iStartPar
            Case 1 'Zufälligen Startwerte
                For v = 1 To PES_Options.varanz
                    For n = 1 To PES_Options.NEltern
                        For m = 1 To PES_Options.NPopEltern
                            'Startwert für die Elternschrittweite wird zugewiesen
                            Dp(v, n, m) = PES_Options.Dn(1)
                            'Startwert für die Eltern werden zugewiesen
                            '(Zufallszahl zwischen 0 und 1)
                            Xp(v, n, m) = Rnd()
                        Next m
                    Next n
                Next v
            Case 2 'Originalparameter
                For v = 1 To PES_Options.varanz
                    For n = 1 To PES_Options.NEltern
                        For m = 1 To PES_Options.NPopEltern
                            'Startwert für die Elternschrittweite wird zugewiesen
                            Dp(v, n, m) = PES_Options.Dn(1)
                            'Startwert für die Eltern werden zugewiesen
                            '(alle gleich Anfangswerte)
                            Xp(v, n, m) = PES_Options.Xn(v)
                        Next m
                    Next n
                Next v
        End Select
        EsStartvalues = True
        Exit Function

ES_STARTVALUES_ERROR:
        Exit Function

    End Function

    '*******************************************************************************
    'ES_isNEXTPOP
    '*******************************************************************************
    'Funktion zählt die Popultationenschleifen und ermittelt, ob die maximale Anzahl
    'an Populationen erreicht ist
    Public Function EsIsNextPop() As Boolean

        EsIsNextPop = False

        On Error GoTo ES_isNEXTPOP_ERROR

        'Anzahl der Populationenschleifen wird hochgezählt
        PES_Options.iaktuellePopulation = PES_Options.iaktuellePopulation + 1

        'Abfrage ob die maximale Anzahl an Populationenschleifen erreicht ist
        If PES_Options.iaktuellePopulation <= PES_Options.NPopul Then
            EsIsNextPop = True
        Else
            PES_Options.iaktuellePopulation = 0
        End If

        Exit Function

ES_isNEXTPOP_ERROR:
        Exit Function
    End Function

    '*******************************************************************************
    'ES_isNEXTRUNDE
    '*******************************************************************************
    'Funktion zählt die Runden und ermittelt, ob die maximale Anzahl
    'an Runden erreicht ist
    Public Function EsIsNextRunde(ByVal Method As String) As Boolean

        EsIsNextRunde = False

        On Error GoTo ES_isNEXTRUNDE_ERROR

        'Anzahl der Runden wird hochgezählt
        PES_Options.iaktuelleRunde = PES_Options.iaktuelleRunde + 1

        'Abfrage ob die maximale Anzahl an Runden erreicht ist
        If PES_Options.iaktuelleRunde <= PES_Options.NRunden Then
            EsIsNextRunde = True
        ElseIf Not Method = "CES + PES" Then
            MsgBox("Optimierung beendet", MsgBoxStyle.Information, "Info")
        End If

        Exit Function

ES_isNEXTRUNDE_ERROR:
        Exit Function
    End Function

    '*******************************************************************************
    'ES_isNEXTGEN
    '*******************************************************************************
    'Funktion zählt die Generationen und ermittelt, ob die maximale Anzahl
    'an Generationen erreicht ist
    Public Function EsIsNextGen() As Boolean

        EsIsNextGen = False

        On Error GoTo ES_isNEXTGEN_ERROR

        'Anzahl der Generation wird hochgezählt
        PES_Options.iaktuelleGeneration = PES_Options.iaktuelleGeneration + 1

        'Abfrage ob die aktuelle Generation abgeschlossen ist
        If PES_Options.iaktuelleGeneration <= PES_Options.NGen Then
            EsIsNextGen = True
        Else
            PES_Options.iaktuelleGeneration = 0
        End If

        Exit Function

ES_isNEXTGEN_ERROR:
        Exit Function
    End Function

    '*******************************************************************************
    'ES_isNEXTNACHF
    '*******************************************************************************
    'Funktion zählt die Nachfahren und ermittelt, ob die maximale Anzahl
    'an Nachfahren erreicht ist
    Public Function EsIsNextNachf() As Boolean

        EsIsNextNachf = False

        On Error GoTo ES_isNEXTNACHF_ERROR

        'Anzahl der Nachfahren wird hochgezählt
        PES_Options.iaktuellerNachfahre = PES_Options.iaktuellerNachfahre + 1

        If PES_Options.iaktuellerNachfahre <= PES_Options.NNachf Then
            EsIsNextNachf = True
        Else
            PES_Options.iaktuellerNachfahre = 0
        End If

        Exit Function

ES_isNEXTNACHF_ERROR:
        Exit Function
    End Function

    '*******************************************************************************
    'ES_POP_VARIA
    '*******************************************************************************

    Public Function EsPopVaria() As Boolean
        Dim w, m, n, v, i As Short
        Dim j As Short
        Dim Realisierungsspeicher() As Short
        Dim Elternspeicher() As Short

        EsPopVaria = False

        On Error GoTo ES_POP_VARIA_ERROR

        '===========================================================================
        'Start Ermittlung der zu mutierenden Eltern
        '===========================================================================

        Select Case PES_Options.iOptPopEltern
            Case 1 'MultiRekombination über alle Eltern (x/x,y) oder (x/x+y)
                For n = 1 To PES_Options.NEltern
                    R = Int(PES_Options.NPopEltern * Rnd()) + 1
                    For v = 1 To PES_Options.varanz
                        'Selektion der Schrittweite
                        De(v, n, PES_Options.iaktuellePopulation) = Dp(v, n, R)
                        'Selektion des Elter
                        Xe(v, n, PES_Options.iaktuellePopulation) = Xp(v, n, R)
                    Next v
                Next n
            Case 2 'Mittelwertbildung über alle Eltern
                'Ermitteln der Elter und Schrittweite über Mittelung der Elternschrittweiten
                For v = 1 To PES_Options.varanz
                    For n = 1 To PES_Options.NEltern
                        De(v, n, PES_Options.iaktuellePopulation) = 0
                        Xe(v, n, PES_Options.iaktuellePopulation) = 0
                        For m = 1 To PES_Options.NPopEltern
                            'Mittelung der Schrittweite,
                            De(v, n, PES_Options.iaktuellePopulation) = De(v, n, PES_Options.iaktuellePopulation) + (Dp(v, n, m) / PES_Options.NPopEltern)
                            'Mittelung der Eltern,
                            Xe(v, n, PES_Options.iaktuellePopulation) = Xe(v, n, PES_Options.iaktuellePopulation) + (Xp(v, n, m) / PES_Options.NPopEltern)
                        Next m
                    Next n
                Next v
            Case 3 'Zufallswahl über alle Eltern
                R = Int(PES_Options.NPopEltern * Rnd()) + 1 'Zufallszahl entscheidet welcher
                'Elternteil vererbt wird
                For v = 1 To PES_Options.varanz
                    For n = 1 To PES_Options.NEltern
                        'Selektion der Schrittweite
                        De(v, n, PES_Options.iaktuellePopulation) = Dp(v, n, R)
                        'Selektion des Elter
                        Xe(v, n, PES_Options.iaktuellePopulation) = Xp(v, n, R)
                    Next n
                Next v
        End Select

        EsPopVaria = True
        Exit Function

ES_POP_VARIA_ERROR:
        Exit Function
    End Function

    '*******************************************************************************
    'ES_VARIA
    '*******************************************************************************

    Public Function EsVaria() As Boolean
        Dim i, v, n, w, j As Short
        Dim Realisierungsspeicher() As Short
        Dim Elternspeicher() As Short
        Dim Z1, Elter, Z2 As Short
        Dim Faktor As Double


        EsVaria = False

        On Error GoTo ES_VARIA_ERROR

        '===========================================================================
        'Start Ermittlung der zu mutierenden Eltern
        '===========================================================================

        Select Case PES_Options.iOptEltern

            Case 1 'Zufallswahl über alle Eltern

                R = Int(PES_Options.NEltern * Rnd()) + 1 'Zufallszahl entscheidet
                'welcher Enternteil vererbt wird
                For v = 1 To PES_Options.varanz
                    'Selektion der Schrittweite
                    PES_Options.Dn(v) = De(v, R, PES_Options.iaktuellePopulation)
                    'Selektion des Elter
                    PES_Options.Xn(v) = Xe(v, R, PES_Options.iaktuellePopulation)
                Next v

            Case 2 'Multi-Rekombination, diskret

                For v = 1 To PES_Options.varanz
                    R = Int(PES_Options.NEltern * Rnd()) + 1
                    'Selektion der Schrittweite
                    PES_Options.Dn(v) = De(v, R, PES_Options.iaktuellePopulation)
                    'Selektion des Elter
                    PES_Options.Xn(v) = Xe(v, R, PES_Options.iaktuellePopulation)
                Next v

            Case 3 'Multi-Rekombination, gemittelt

                For v = 1 To PES_Options.varanz
                    PES_Options.Dn(v) = 0
                    PES_Options.Xn(v) = 0

                    For n = 1 To PES_Options.NEltern
                        'Mittelung der Schrittweite,
                        PES_Options.Dn(v) = PES_Options.Dn(v) + (De(v, n, PES_Options.iaktuellePopulation) / PES_Options.NRekombXY)
                        'Mittelung der Eltern,
                        PES_Options.Xn(v) = PES_Options.Xn(v) + (Xe(v, n, PES_Options.iaktuellePopulation) / PES_Options.NRekombXY)
                    Next

                Next v

            Case 4 'Multi-Rekombination nach X/Y-Schema, diskrete Vertauschung

                ReDim Realisierungsspeicher(PES_Options.NRekombXY)
                ReDim Elternspeicher(PES_Options.NEltern)

                For i = 1 To PES_Options.NEltern
                    Elternspeicher(i) = i
                Next i

                For i = 1 To PES_Options.NRekombXY
                    R = Int((PES_Options.NEltern - (i - 1)) * Rnd()) + 1
                    Realisierungsspeicher(i) = Elternspeicher(R)

                    For j = R To (PES_Options.NEltern - 1)
                        Elternspeicher(R) = Elternspeicher(R + 1)
                    Next j

                Next i

                For v = 1 To PES_Options.varanz
                    R = Int(PES_Options.NRekombXY * Rnd()) + 1
                    'Selektion der Schrittweite
                    PES_Options.Dn(v) = De(v, Realisierungsspeicher(R), PES_Options.iaktuellePopulation)
                    'Selektion des Elter
                    PES_Options.Xn(v) = Xe(v, Realisierungsspeicher(R), PES_Options.iaktuellePopulation)
                Next v

            Case 5 'Multi-Rekombination nach X/Y-Schema, Mittelung der Gene

                ReDim Realisierungsspeicher(PES_Options.NRekombXY)
                ReDim Elternspeicher(PES_Options.NEltern)

                For i = 1 To PES_Options.NEltern
                    Elternspeicher(i) = i
                Next i

                For i = 1 To PES_Options.NRekombXY
                    R = Int((PES_Options.NEltern - (i - 1)) * Rnd()) + 1
                    Realisierungsspeicher(i) = Elternspeicher(R)

                    For j = R To (PES_Options.NEltern - 1)
                        Elternspeicher(R) = Elternspeicher(R + 1)
                    Next j

                Next i

                For v = 1 To PES_Options.varanz
                    PES_Options.Dn(v) = 0
                    PES_Options.Xn(v) = 0

                    For n = 1 To PES_Options.NRekombXY
                        'Mittelung der Schrittweite,
                        PES_Options.Dn(v) = PES_Options.Dn(v) + (De(v, Elternspeicher(n), PES_Options.iaktuellePopulation) / PES_Options.NRekombXY)
                        'Mittelung der Eltern,
                        PES_Options.Xn(v) = PES_Options.Xn(v) + (Xe(v, Elternspeicher(n), PES_Options.iaktuellePopulation) / PES_Options.NRekombXY)
                    Next

                Next v

            Case 6 'Neighbourhood Rekombination

                Z1 = Int(PES_Options.NEltern * Rnd()) + 1
                Do
                    Z2 = Int(PES_Options.NEltern * Rnd()) + 1
                Loop While Z1 = Z2

                'Tournament über Crowding Distance

                If Distanceb(Z1) > Distanceb(Z2) Then
                    Elter = Z1
                Else
                    Elter = Z2
                End If

                If (Elter = 1 Or Elter = PES_Options.NEltern) Then

                    For v = 1 To PES_Options.varanz
                        'Selektion der Schrittweite
                        PES_Options.Dn(v) = De(v, Elter, PES_Options.iaktuellePopulation)
                        'Selektion des Elter
                        PES_Options.Xn(v) = Xe(v, Elter, PES_Options.iaktuellePopulation)
                    Next

                Else

                    Call Neighbourhood_Eltern(PenaltyDistance, Elter, PES_Options.NRekombXY, IndexEltern)
                    For v = 1 To PES_Options.varanz
                        'Do
                        '    Faktor = Rnd
                        '    Faktor = (-1) * Eigenschaft.d + Faktor * (1 + Eigenschaft.d)
                        '    'Selektion der Schrittweite
                        '    Eigenschaft.Dn(v) = De(v, IndexEltern(1), Eigenschaft.iaktuellePopulation) * Faktor + _
                        '                     De(v, IndexEltern(2), Eigenschaft.iaktuellePopulation) * (1 - Faktor)
                        '    Eigenschaft.Xn(v) = Xe(v, IndexEltern(1), Eigenschaft.iaktuellePopulation) * Faktor + _
                        '                     Xe(v, IndexEltern(2), Eigenschaft.iaktuellePopulation) * (1 - Faktor)
                        'Loop While (Eigenschaft.Xn(v) <= Eigenschaft.Xmin(v) Or Eigenschaft.Xn(v) > Eigenschaft.Xmax(v))

                        R = Int(PES_Options.NRekombXY * Rnd() + 1)
                        'Selektion der Schrittweite
                        PES_Options.Dn(v) = De(v, IndexEltern(R), PES_Options.iaktuellePopulation)
                        'Selektion des Elter
                        PES_Options.Xn(v) = Xe(v, IndexEltern(R), PES_Options.iaktuellePopulation)
                    Next

                End If

        End Select

        EsVaria = True
        Exit Function

ES_VARIA_ERROR:
        Exit Function

    End Function

    '*******************************************************************************
    'ES_POP_MUTATION
    '*******************************************************************************

    Public Function EsPopMutation() As Boolean
        Dim v, n As Short
        '===========================================================================
        'Start Mutation
        '===========================================================================
        EsPopMutation = False

        On Error GoTo ES_POP_MUTATION_ERROR

        If Not PES_Options.isDnVektor Then
            '+/-1
            expo = (2 * Int(Rnd() + 0.5) - 1)
            'Schrittweite wird mutiert
            DeTemp = De(1, 1, PES_Options.iaktuellePopulation) * palpha ^ expo
        End If

        For v = 1 To PES_Options.varanz

            For n = 1 To PES_Options.NEltern

                Do

                    If PES_Options.isDnVektor Then
                        '+/-1
                        expo = (2 * Int(Rnd() + 0.5) - 1)
                        'Schrittweite wird mutiert
                        DeTemp = De(v, n, PES_Options.iaktuellePopulation) * palpha ^ expo
                    End If
                    'Normalverteilte Zufallszahl mit
                    'Standardabweichung 1/sqr(varanz)
                    Z = System.Math.Sqrt(-2 * System.Math.Log(1 - Rnd()) / PES_Options.varanz) * System.Math.Sin(6.2832 * Rnd())
                    'Mutation wird durchgeführt
                    XeTemp = Xe(v, n, PES_Options.iaktuellePopulation) + DeTemp * Z

                    ' Restriktion für die mutierten Werte
                Loop While (XeTemp <= PES_Options.Xmin(v) Or XeTemp > PES_Options.Xmax(v))

                De(v, n, PES_Options.iaktuellePopulation) = DeTemp
                Xe(v, n, PES_Options.iaktuellePopulation) = XeTemp

            Next n

        Next v

        EsPopMutation = True
        Exit Function

ES_POP_MUTATION_ERROR:
        Exit Function

    End Function

    '*******************************************************************************
    'ES_MUTATION
    '*******************************************************************************

    Public Function EsMutation() As Boolean
        Dim v As Short
        '===========================================================================
        'Start Mutation
        '===========================================================================
        EsMutation = False

        On Error GoTo ES_MUTATION_ERROR

        If Not PES_Options.isDnVektor Then
            '+/-1
            expo = (2 * Int(Rnd() + 0.5) - 1)
            'Schrittweite wird mutiert
            DnTemp = PES_Options.Dn(1) * galpha ^ expo
        End If

        For v = 1 To PES_Options.varanz
            Do
                If PES_Options.isDnVektor Then
                    '+/-1
                    expo = (2 * Int(Rnd() + 0.5) - 1)
                    'Schrittweite wird mutiert
                    DnTemp = PES_Options.Dn(v) * galpha ^ expo
                End If
                'Normalverteilte Zufallszahl mit
                'Standardabweichung 1/sqr(varanz)
                Z = System.Math.Sqrt(-2 * System.Math.Log(1 - Rnd()) / PES_Options.varanz) * System.Math.Sin(6.2832 * Rnd())
                'Mutation wird durchgeführt
                XnTemp = PES_Options.Xn(v) + DnTemp * Z
                ' Restriktion für die mutierten Werte
            Loop While (XnTemp <= PES_Options.Xmin(v) Or XnTemp > PES_Options.Xmax(v))

            PES_Options.Dn(v) = DnTemp
            PES_Options.Xn(v) = XnTemp
        Next v

        EsMutation = True
        Exit Function

ES_MUTATION_ERROR:
        Exit Function

    End Function

    '*******************************************************************************
    'ES_POP_BEST
    '*******************************************************************************

    Public Function EsPopBest() As Boolean
        Dim m, i, j, n As Short
        Dim h1, h2 As Double

        EsPopBest = False

        On Error GoTo ES_POP_BEST_ERROR

        'Der schlechtetste der besten Qualitätswerte wird bestimmt ; Position -> i
        '(höchster Wert der Peanaltyfunktion, niedrigster Wert der Crowding Distance)
        i = 1
        h1 = Qbpop(1, 1)
        For m = 2 To PES_Options.NPopul
            If Not PES_Options.isPareto Then
                If Qbpop(m, 1) > h1 Then
                    h1 = Qbpop(m, 1)
                    i = m
                End If
            Else
                Select Case PES_Options.iPopPenalty
                    Case 1 'Crowding
                        If Qbpop(m, 1) > h1 Then
                            h1 = Qbpop(m, 1)
                            i = m
                        End If
                    Case 2 'Spannweite
                        If Qbpop(m, 1) < h1 Then
                            h2 = Qbpop(m, 2)
                            i = m
                        End If
                End Select
            End If
        Next m

        'Der schlechtetste der besten Qualitätswerte wird bestimmt ; Position -> i
        '(höchster Wert der Kostenfunktion, niedrigster Wert der Spannweite)
        If PES_Options.isMultiObjective Then
            j = 1
            h2 = Qbpop(1, 2)
            For m = 2 To PES_Options.NPopul
                If Not PES_Options.isPareto Then
                    If Qbpop(m, 2) > h2 Then
                        h2 = Qbpop(m, 2)
                        j = m
                    End If
                Else
                    If Qbpop(m, 2) < h2 Then
                        h2 = Qbpop(m, 2)
                        j = m
                    End If
                End If
            Next m
        End If

        'Qualität der aktuellen Population wird bestimmt
        h1 = 0
        If PES_Options.isMultiObjective Then h2 = 0

        For m = 1 To PES_Options.NEltern
            If Not PES_Options.isMultiObjective Then
                h1 = h1 + Qb(m, PES_Options.iaktuellePopulation, 1) / PES_Options.NEltern
            Else
                If Not PES_Options.isPareto Then

                    Select Case PES_Options.iPopPenalty

                        Case 1 'Mittelwert
                            h1 = h1 + Qb(m, PES_Options.iaktuellePopulation, 1) / PES_Options.NEltern
                            h2 = h2 + Qb(m, PES_Options.iaktuellePopulation, 2) / PES_Options.NEltern

                        Case 2 'teuerster und schlechtester Wert der Population als Bewertung
                            If h1 < Qb(m, PES_Options.iaktuellePopulation, 1) Then
                                h1 = Qb(m, PES_Options.iaktuellePopulation, 1)
                            End If

                            If h2 < Qb(m, PES_Options.iaktuellePopulation, 2) Then
                                h2 = Qb(m, PES_Options.iaktuellePopulation, 2)
                            End If

                    End Select

                End If
            End If
        Next m

        If PES_Options.isPareto Then
            h1 = NDS_Crowding_Distance_Count(Qb, h2)
        End If

        'Falls die Qualität des aktuellen Population besser ist (Penaltyfunktion geringer)
        'als die schlechteste im Bestwertspeicher, wird diese ersetzt
        If Not PES_Options.isMultiObjective Then
            If h1 < Qbpop(i, 1) Then
                Qbpop(i, 1) = h1
                For m = 1 To PES_Options.varanz
                    For n = 1 To PES_Options.NEltern
                        'Die Schrittweite wird ebenfalls übernommen
                        Dbpop(m, n, i) = Db(m, n, PES_Options.iaktuellePopulation)
                        'Die eigentlichen Parameterwerte werden übernommen
                        Xbpop(m, n, i) = Xb(m, n, PES_Options.iaktuellePopulation)
                    Next n
                Next m
            End If
        Else
            If Not PES_Options.isPareto Then
                If h1 <= Qbpop(i, 1) And h2 < Qbpop(i, 2) Then
                    Qbpop(j, 1) = h1
                    Qbpop(j, 2) = h2
                    For m = 1 To PES_Options.varanz
                        For n = 1 To PES_Options.NEltern
                            'Die Schrittweite wird ebenfalls übernommen
                            Dbpop(m, n, i) = Db(m, n, PES_Options.iaktuellePopulation)
                            'Die eigentlichen Parameterwerte werden übernommen
                            Xbpop(m, n, i) = Xb(m, n, PES_Options.iaktuellePopulation)
                        Next n
                    Next m
                End If
            Else
                Select Case PES_Options.iPopPenalty
                    Case 1 'Crowding
                        If h1 < Qbpop(i, 1) Then
                            Qbpop(i, 1) = h1
                            For m = 1 To PES_Options.varanz
                                For n = 1 To PES_Options.NEltern
                                    'Die Schrittweite wird ebenfalls übernommen
                                    Dbpop(m, n, i) = Db(m, n, PES_Options.iaktuellePopulation)
                                    'Die eigentlichen Parameterwerte werden übernommen
                                    Xbpop(m, n, i) = Xb(m, n, PES_Options.iaktuellePopulation)
                                Next n
                            Next m
                        End If
                    Case 2
                        If h2 > Qbpop(j, 2) Then
                            Qbpop(j, 2) = h2
                            For m = 1 To PES_Options.varanz
                                For n = 1 To PES_Options.NEltern
                                    'Die Schrittweite wird ebenfalls übernommen
                                    Dbpop(m, n, j) = Db(m, n, PES_Options.iaktuellePopulation)
                                    'Die eigentlichen Parameterwerte werden übernommen
                                    Xbpop(m, n, j) = Xb(m, n, PES_Options.iaktuellePopulation)
                                Next n
                            Next m
                        End If
                End Select
            End If
        End If

        EsPopBest = True

ES_POP_BEST_ERROR:
        Exit Function
    End Function

    '*******************************************************************************
    'ES_BEST
    '*******************************************************************************

    Public Function EsBest(ByRef ZF() As Double, ByRef RF() As Double) As Boolean

        'ZF ist der berechnete Zielfunktionswert, z derTyp der Zielfunktion
        'momentan z = 1 - okölogisch orientierte Zielfunktion
        '         z = 2 - monetär orientierte Zielfunktion
        '         z = 3 - weitere okölogisch orientierte Zielfunktion

        Dim m, i, j, v As Short
        Dim h As Double

        EsBest = False

        On Error GoTo ES_BEST_ERROR


        If Not PES_Options.isPareto Then 'Standard ES nach Rechenberg
            'Der schlechtetste der besten Qualitätswerte wird bestimmt ; Position -> j
            '(höchster Wert der Peanaltyfunktion)
            j = 1
            h = Qb(1, PES_Options.iaktuellePopulation, 1)

            For m = 2 To PES_Options.NEltern
                If Qb(m, PES_Options.iaktuellePopulation, 1) > h Then
                    h = Qb(m, PES_Options.iaktuellePopulation, 1)
                    j = m
                End If
            Next m

            'Falls die Qualität des aktuellen Nachkommen besser ist (Penaltyfunktion geringer)
            'als die schlechteste im Bestwertspeicher, wird diese ersetz
            If ZF(1) < Qb(j, PES_Options.iaktuellePopulation, 1) Then
                Qb(j, PES_Options.iaktuellePopulation, 1) = ZF(1)
                For v = 1 To PES_Options.varanz
                    'Die Schrittweite wird ebenfalls übernommen
                    Db(v, j, PES_Options.iaktuellePopulation) = PES_Options.Dn(v)
                    'Die eigentlichen Parameterwerte werden übernommen
                    Xb(v, j, PES_Options.iaktuellePopulation) = PES_Options.Xn(v)
                Next v
                If PES_Options.NPenalty = 2 Then
                    Qb(j, PES_Options.iaktuellePopulation, 2) = ZF(2)
                End If
            End If

        Else 'Multi-objective mit paretofront
            With List_NDSorting(PES_Options.iaktuellerNachfahre)
                For i = 1 To PES_Options.NPenalty
                    .penalty(i) = ZF(i)
                Next i
                .feasible = True
                For i = 1 To PES_Options.NConstrains
                    .constrain(i) = RF(i)
                    If .constrain(i) < 0 Then .feasible = False
                Next i
                .dominated = False
                .Front = 0
                For v = 1 To PES_Options.varanz
                    .d(v) = PES_Options.Dn(v)
                    .X(v) = PES_Options.Xn(v)
                Next v
                .distance = 0
            End With
        End If

        EsBest = True

ES_BEST_ERROR:
        Exit Function
    End Function

    '*******************************************************************************
    'ES_BESTWERTSPEICHER
    '
    'Führt einen Reset des Bestwertspeicher durch,
    'falls eine Komma-Strategie gewählt ist
    '*******************************************************************************

    Public Function EsBestwertspeicher() As Boolean
        Dim n, i As Short

        EsBestwertspeicher = False

        On Error GoTo ES_BESTWERTSPEICHER_ERROR

        If PES_Options.iEvoTyp = EVO_KOMMA Then
            If Not PES_Options.isPareto Then
                For n = 1 To PES_Options.NEltern
                    For i = 1 To PES_Options.NPenalty 'dm 29.04.05
                        Qb(n, PES_Options.iaktuellePopulation, i) = 1.0E+300 'dm 29.04.05
                    Next i 'dm 29.04.05
                Next n
            Else
                For n = 1 To PES_Options.NEltern
                    For i = 1 To PES_Options.NPenalty 'dm 29.04.05
                        Qb(n, PES_Options.iaktuellePopulation, i) = 0 'dm 29.04.05
                    Next i
                Next n
            End If
        End If

        EsBestwertspeicher = True
        Exit Function

ES_BESTWERTSPEICHER_ERROR:
        Exit Function

    End Function

    '*******************************************************************************
    'ES_POP_BESTWERTSPEICHER
    '
    'Führt einen Reset des Bestwertspeicher auf Populationsebene durch,
    'falls eine Komma-Strategie gewählt ist
    '*******************************************************************************

    Public Function EsPopBestwertspeicher() As Boolean
        Dim n, i As Short

        EsPopBestwertspeicher = False

        On Error GoTo ES_POP_BESTWERTSPEICHER_ERROR

        If PES_Options.iPopEvoTyp = EVO_KOMMA Then
            If Not PES_Options.isPareto Then
                For n = 1 To PES_Options.NPopul
                    For i = 1 To PES_Options.NPenalty 'dm 29.04.05
                        Qbpop(n, i) = 1.0E+300 'dm 29.04.05
                    Next i 'dm 29.04.05
                Next n
            Else
                For n = 1 To PES_Options.NPopul
                    For i = 1 To PES_Options.NPenalty 'dm 29.04.05
                        Qbpop(n, i) = 0 'dm 29.04.05
                    Next i 'dm 29.04.05
                Next n
            End If
        End If

        EsPopBestwertspeicher = True
        Exit Function

ES_POP_BESTWERTSPEICHER_ERROR:
        Exit Function

    End Function

    '*******************************************************************************
    'ES_POP_ELTERN  Eltern Population
    '*******************************************************************************

    Public Function EsPopEltern() As Boolean
        Dim n, m, v As Short
        Dim swap(2) As Double
        Dim Realisierungsspeicher(,) As Double
        Dim Z As Short

        Select Case PES_Options.iPopPenalty
            Case 1 'Crowding
                Z = 1 '
            Case 2 'Spannweite
                Z = 2
        End Select

        EsPopEltern = False

        On Error GoTo ES_POP_ELTERN_ERROR

        ReDim Realisierungsspeicher(PES_Options.NPopul, 2)

        'Die NPopEltern besten Individium-Sätze werden ermittelt
        For m = 1 To PES_Options.NPopul
            Realisierungsspeicher(m, 1) = Qbpop(m, Z)
            Realisierungsspeicher(m, 2) = m
        Next m

        If Not PES_Options.isPareto Then
            For m = 1 To PES_Options.NPopul
                For n = m To PES_Options.NPopul
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
            Select Case PES_Options.iPopPenalty
                Case 1 'Crowding
                    For m = 1 To PES_Options.NPopul
                        For n = m To PES_Options.NPopul
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
                Case 2 'Spannweite
                    For m = 1 To PES_Options.NPopul
                        For n = m To PES_Options.NPopul
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
        For m = 1 To PES_Options.NPopEltern
            For n = 1 To PES_Options.NEltern
                For v = 1 To PES_Options.varanz
                    Dp(v, n, m) = Dbpop(v, n, Int(Realisierungsspeicher(m, 2)))
                    Xp(v, n, m) = Xbpop(v, n, Int(Realisierungsspeicher(m, 2)))
                Next v
            Next n
        Next m


        EsPopEltern = True
        Exit Function

ES_POP_ELTERN_ERROR:
        Exit Function

    End Function

    '*******************************************************************************
    'ES_ELTERN
    '*******************************************************************************

    Public Function EsEltern() As Boolean

        Dim l, n, m, v, i, j As Short
        Dim NFrontMember_aktuell, NFrontMember_gesamt As Short
        Dim durchlauf As Short
        Dim Temp() As Struct_NDSorting
        Dim NDSResult() As Struct_NDSorting
        Dim Count, aktuelle_Front As Short
        Dim Member_Sekundärefront As Short


        EsEltern = False

        On Error GoTo ES_ELTERN_ERROR

        '*** Standard ES nach Rechenberg/Schwefel ***
        If Not PES_Options.isPareto Then

            'Die Eltern werden gleich der besten Kinder gesetzt (Schrittweite und Parameterwert)
            For m = 1 To PES_Options.NEltern
                For v = 1 To PES_Options.varanz
                    De(v, m, PES_Options.iaktuellePopulation) = Db(v, m, PES_Options.iaktuellePopulation)
                    Xe(v, m, PES_Options.iaktuellePopulation) = Xb(v, m, PES_Options.iaktuellePopulation)
                Next v
            Next m

        Else
            '*** Multi-objective mit Paretofront ***
            '1. Eltern und Nachfolger werden gemeinsam betrachtet
            'Nur Eltern werden NDSorting hinzugefügt, Kinder sind schon oben drin
            '---------------------------------------------------------------------

            For m = PES_Options.NNachf + 1 To PES_Options.NNachf + PES_Options.NEltern
                With List_NDSorting(m)
                    For l = 1 To PES_Options.NPenalty
                        .penalty(l) = Qb(m - PES_Options.NNachf, PES_Options.iaktuellePopulation, l)
                    Next l
                    If PES_Options.NConstrains > 0 Then
                        .feasible = True
                        For l = 1 To PES_Options.NConstrains
                            .constrain(l) = Rb(m - PES_Options.NNachf, PES_Options.iaktuellePopulation, l)
                            If .constrain(l) < 0 Then .feasible = False
                        Next l
                    End If
                    .dominated = False
                    .Front = 0
                    For v = 1 To PES_Options.varanz
                        'Die Schrittweite wird ebenfalls übernommen
                        .d(v) = Db(v, m - PES_Options.NNachf, PES_Options.iaktuellePopulation)
                        'Die eigentlichen Parameterwerte werden übernommen
                        .X(v) = Xb(v, m - PES_Options.NNachf, PES_Options.iaktuellePopulation)
                    Next v
                    .distance = 0
                End With
            Next m

            '2. Die einzelnen Fronten werden bestimmt
            '----------------------------------------
            durchlauf = 1
            NFrontMember_gesamt = 0

            'Initialisierung von Temp (NDSorting)
            ReDim Temp(PES_Options.NNachf + PES_Options.NEltern)

            For i = 1 To (PES_Options.NNachf + PES_Options.NEltern)
                ReDim Temp(i).d(PES_Options.varanz)
                ReDim Temp(i).X(PES_Options.varanz)
            Next i
            'Initialisierung von NDSResult (NDSorting)
            ReDim NDSResult(PES_Options.NNachf + PES_Options.NEltern)

            For i = 1 To (PES_Options.NNachf + PES_Options.NEltern)
                ReDim NDSResult(i).d(PES_Options.varanz)
                ReDim NDSResult(i).X(PES_Options.varanz)
            Next i

            'NDSorting wird in Temp kopiert
            Array.Copy(List_NDSorting, Temp, List_NDSorting.GetLength(0))

            'Schleife läuft über die Zahl der Fronten die hier auch bestimmte werden
            Do
                'Entscheidet welche Werte dominiert werden und welche nicht
                Call Non_Dominated_Sorting(Temp, durchlauf) 'aktuallisiert auf n Objectives dm 10.05.05
                'Sortiert die nicht dominanten Lösungen nach oben,
                'die dominanten nach unten und zählt die Mitglieder der aktuellen Front
                NFrontMember_aktuell = Non_Dominated_Count_and_Sort(Temp)
                'NFrontMember_aktuell: Anzahl der Mitglieder der gerade bestimmten Front
                'NFrontMember_gesamt: Alle bisher als nicht dominiert klassifizierten Individuum
                NFrontMember_gesamt = NFrontMember_gesamt + NFrontMember_aktuell
                'Hier wird pro durchlauf die nicht dominierte Front in NDSResult geschaufelt
                'und die bereits klassifizierten Lösungen aus Temp Array gelöscht
                Call Non_Dominated_Result(Temp, NDSResult, NFrontMember_aktuell, NFrontMember_gesamt)
                'Durchlauf ist hier die Nummer der Front
                durchlauf = durchlauf + 1
            Loop While Not (NFrontMember_gesamt = PES_Options.NEltern + PES_Options.NNachf)

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
                If NFrontMember_aktuell <= PES_Options.NEltern - NFrontMember_gesamt Then
                    For i = NFrontMember_gesamt + 1 To NFrontMember_aktuell + NFrontMember_gesamt
                        For j = 1 To PES_Options.NPenalty
                            Qb(i, PES_Options.iaktuellePopulation, j) = NDSResult(i).penalty(j)
                        Next j
                        If PES_Options.NConstrains > 0 Then
                            For j = 1 To PES_Options.NConstrains
                                Rb(i, PES_Options.iaktuellePopulation, j) = NDSResult(i).constrain(j)
                            Next j
                        End If
                        For v = 1 To PES_Options.varanz
                            Db(v, i, PES_Options.iaktuellePopulation) = NDSResult(i).d(v)
                            Xb(v, i, PES_Options.iaktuellePopulation) = NDSResult(i).X(v)
                        Next v

                    Next i
                    NFrontMember_gesamt = NFrontMember_gesamt + NFrontMember_aktuell

                Else
                    'Es sind weniger Elterplätze für die nächste Generation verfügber
                    'als Mitglieder der aktuellen Front. Nur für diesen Rest wird crowding distance
                    'gemacht um zu bestimmen wer noch mitspielen darf und wer noch a biserl was druff hat
                    Call NDS_Crowding_Distance_Sort(NDSResult, NFrontMember_gesamt + 1, NFrontMember_gesamt + NFrontMember_aktuell)

                    For i = NFrontMember_gesamt + 1 To PES_Options.NEltern

                        For j = 1 To PES_Options.NPenalty
                            Qb(i, PES_Options.iaktuellePopulation, j) = NDSResult(i).penalty(j)
                        Next j
                        If PES_Options.NConstrains > 0 Then
                            For j = 1 To PES_Options.NConstrains
                                Rb(i, PES_Options.iaktuellePopulation, j) = NDSResult(i).constrain(j)
                            Next j
                        End If
                        For v = 1 To PES_Options.varanz
                            Db(v, i, PES_Options.iaktuellePopulation) = NDSResult(i).d(v)
                            Xb(v, i, PES_Options.iaktuellePopulation) = NDSResult(i).X(v)
                        Next v

                    Next i

                    NFrontMember_gesamt = PES_Options.NEltern

                End If

                aktuelle_Front = aktuelle_Front + 1

            Loop While Not (NFrontMember_gesamt = PES_Options.NEltern)

            '4: Sekundäre Population wird bestimmt und gespeichert
            '-------------------------------------------------------
            NFrontMember_aktuell = Count_Front_Members(1, NDSResult)

            If PES_Options.iaktuelleRunde = 1 And PES_Options.iaktuellePopulation = 1 And PES_Options.iaktuelleGeneration = 1 Then
                ReDim Preserve SekundärQb(Member_Sekundärefront + NFrontMember_aktuell)
            Else
                Member_Sekundärefront = UBound(SekundärQb)
                ReDim Preserve SekundärQb(Member_Sekundärefront + NFrontMember_aktuell)
            End If

            For i = Member_Sekundärefront + 1 To Member_Sekundärefront + NFrontMember_aktuell
                SekundärQb(i) = NDSResult(i - Member_Sekundärefront)
            Next i

            Call Non_Dominated_Sorting(SekundärQb, 1)
            NFrontMember_aktuell = Non_Dominated_Count_and_Sort_Sekundäre_Population(SekundärQb)
            ReDim Preserve SekundärQb(NFrontMember_aktuell)
            Call SekundärQb_Duplettten(SekundärQb)
            NFrontMember_aktuell = Non_Dominated_Count_and_Sort_Sekundäre_Population(SekundärQb)
            ReDim Preserve SekundärQb(NFrontMember_aktuell)

            If UBound(SekundärQb) > PES_Options.NMemberSecondPop Then
                Call NDS_Crowding_Distance_Sort(SekundärQb, 1, UBound(SekundärQb))
                ReDim Preserve SekundärQb(PES_Options.NMemberSecondPop)
            End If

            If (PES_Options.iaktuelleGeneration Mod PES_Options.interact) = 0 And PES_Options.isInteract Then
                NFrontMember_aktuell = Count_Front_Members(1, SekundärQb)
                If NFrontMember_aktuell > PES_Options.NEltern Then
                    Call NDS_Crowding_Distance_Sort(SekundärQb, 1, UBound(SekundärQb))
                    For i = 1 To PES_Options.NEltern

                        For j = 1 To PES_Options.NPenalty
                            Qb(i, PES_Options.iaktuellePopulation, j) = SekundärQb(i).penalty(j)
                        Next j
                        If PES_Options.NConstrains > 0 Then
                            For j = 1 To PES_Options.NConstrains
                                Rb(i, PES_Options.iaktuellePopulation, j) = SekundärQb(i).constrain(j)
                            Next j
                        End If
                        For v = 1 To PES_Options.varanz
                            Db(v, i, PES_Options.iaktuellePopulation) = SekundärQb(i).d(v)
                            Xb(v, i, PES_Options.iaktuellePopulation) = SekundärQb(i).X(v)
                        Next v

                    Next i
                End If
            End If

            'Neue Eltern werden gleich dem Bestwertspeicher gesetzt
            For m = 1 To PES_Options.NEltern
                For v = 1 To PES_Options.varanz
                    De(v, m, PES_Options.iaktuellePopulation) = Db(v, m, PES_Options.iaktuellePopulation)
                    Xe(v, m, PES_Options.iaktuellePopulation) = Xb(v, m, PES_Options.iaktuellePopulation)
                Next v
            Next m

            'Sortierung der Lösungen ist nur für Neighbourhood-Rekombination notwendig
            If PES_Options.iOptEltern = EVO_ELTERN_Neighbourhood Then
                Call Neighbourhood_AbstandsArray(PenaltyDistance, Qb)
                Call Neighbourhood_Crowding_Distance(Distanceb, Qb)
            End If

        End If

        EsEltern = True
        Exit Function

ES_ELTERN_ERROR:
        Exit Function

    End Function

    '*******************************************************************************
    'Non_Dominated_Sorting
    'Entscheidet welche Werte dominiert werden und welche nicht
    '*******************************************************************************
    Private Sub Non_Dominated_Sorting(ByRef NDSorting() As Struct_NDSorting, ByRef durchlauf As Short)

        Dim j, i, k As Short
        Dim Logical As Boolean
        Dim Summe_Constrain(2) As Double

        If PES_Options.NConstrains > 0 Then
            For i = 1 To UBound(NDSorting)
                For j = 1 To UBound(NDSorting)
                    If NDSorting(i).feasible And Not NDSorting(j).feasible Then

                        If NDSorting(j).dominated = False Then NDSorting(j).dominated = True

                    ElseIf ((Not NDSorting(i).feasible) And (Not NDSorting(j).feasible)) Then

                        Summe_Constrain(1) = 0
                        Summe_Constrain(2) = 0

                        For k = 1 To PES_Options.NConstrains
                            If NDSorting(i).constrain(k) < 0 Then
                                Summe_Constrain(1) = Summe_Constrain(1) + NDSorting(i).constrain(k)
                            End If
                            If NDSorting(j).constrain(k) < 0 Then
                                Summe_Constrain(2) = Summe_Constrain(2) + NDSorting(j).constrain(k)
                            End If
                        Next k

                        If Summe_Constrain(1) > Summe_Constrain(2) Then
                            If NDSorting(j).dominated = False Then NDSorting(j).dominated = True
                        End If

                    ElseIf (NDSorting(i).feasible And NDSorting(j).feasible) Then

                        Logical = False

                        For k = 1 To PES_Options.NPenalty
                            Logical = Logical Or (NDSorting(i).penalty(k) < NDSorting(j).penalty(k))
                        Next k

                        For k = 1 To PES_Options.NPenalty
                            Logical = Logical And (NDSorting(i).penalty(k) <= NDSorting(j).penalty(k))
                        Next k

                        If Logical Then
                            If NDSorting(j).dominated = False Then NDSorting(j).dominated = True
                        End If

                    End If
                Next j
            Next i
        Else
            For i = 1 To UBound(NDSorting)
                For j = 1 To UBound(NDSorting)

                    Logical = False

                    For k = 1 To PES_Options.NPenalty
                        Logical = Logical Or (NDSorting(i).penalty(k) < NDSorting(j).penalty(k))
                    Next k

                    For k = 1 To PES_Options.NPenalty
                        Logical = Logical And (NDSorting(i).penalty(k) <= NDSorting(j).penalty(k))
                    Next k

                    If Logical Then
                        If NDSorting(j).dominated = False Then NDSorting(j).dominated = True
                    End If
                Next j
            Next i
        End If

        For i = 1 To UBound(NDSorting)
            'Hier wird die Nummer der Front geschrieben
            If NDSorting(i).dominated = False Then NDSorting(i).Front = durchlauf
        Next i

    End Sub

    '*******************************************************************************
    'Non_Dominated_Count_and_Sort
    'Sortiert die nicht dominanten Lösungen nach oben, die dominanten nach unten
    '*******************************************************************************

    Private Function Non_Dominated_Count_and_Sort(ByRef NDSorting() As Struct_NDSorting) As Short
        Dim i As Short
        Dim Temp() As Struct_NDSorting
        Dim counter As Short

        ReDim Temp(UBound(NDSorting))

        For i = 1 To (UBound(NDSorting))
            ReDim Temp(i).d(PES_Options.varanz)
            ReDim Temp(i).X(PES_Options.varanz)
        Next i

        Non_Dominated_Count_and_Sort = 0
        counter = 0

        'Die nicht dominanten Lösungen werden nach oben kopiert
        For i = 1 To UBound(NDSorting)
            If NDSorting(i).dominated = True Then
                counter = counter + 1
                Temp(counter) = NDSorting(i)
            End If
        Next i

        'Zahl der dominanten wird errechnet und zurückgegeben
        Non_Dominated_Count_and_Sort = UBound(NDSorting) - counter

        'Die dominanten Lösungen werden nach unten kopiert
        For i = 1 To UBound(NDSorting)
            If NDSorting(i).dominated = False Then
                counter = counter + 1
                Temp(counter) = NDSorting(i)
            End If
        Next i

        Array.Copy(Temp, NDSorting, NDSorting.GetLength(0))

    End Function

    '*******************************************************************************
    'Non_Dominated_Count_and_Sort_Sekundäre_Population
    'Sortiert die nicht dominanten Lösungen nach oben, die dominanten nach unten
    'hier für die Sekundäre Population
    '*******************************************************************************

    Private Function Non_Dominated_Count_and_Sort_Sekundäre_Population(ByRef NDSorting() As Struct_NDSorting) As Short
        Dim i As Short
        Dim Temp() As Struct_NDSorting
        Dim counter As Short

        ReDim Temp(UBound(NDSorting))

        For i = 1 To (UBound(NDSorting))
            ReDim Temp(i).d(PES_Options.varanz)
            ReDim Temp(i).X(PES_Options.varanz)
        Next i

        Non_Dominated_Count_and_Sort_Sekundäre_Population = 0
        counter = 0

        For i = 1 To UBound(NDSorting)
            If NDSorting(i).dominated = False Then
                counter = counter + 1
                Temp(counter) = NDSorting(i)
            End If
        Next i

        Non_Dominated_Count_and_Sort_Sekundäre_Population = counter

        For i = 1 To UBound(NDSorting)
            If NDSorting(i).dominated = True Then
                counter = counter + 1
                Temp(counter) = NDSorting(i)
            End If
        Next i

        Array.Copy(Temp, NDSorting, NDSorting.GetLength(0))

    End Function

    '*******************************************************************************
    'Non_Dominated_Result
    'Hier wird pro durchlauf die nicht dominierte Front in NDSResult geschaufelt
    'und die bereits klassifizierten Lösungen aus Temp Array gelöscht
    '*******************************************************************************

    Private Sub Non_Dominated_Result(ByRef Temp() As Struct_NDSorting, ByRef NDSResult() As Struct_NDSorting, ByRef NFrontMember_aktuell As Short, ByRef NFrontMember_gesamt As Short)

        Dim i, Position As Short

        Position = NFrontMember_gesamt - NFrontMember_aktuell + 1

        'In NDSResult werden die nicht dominierten Lösungen eingefügt
        For i = UBound(Temp) + 1 - NFrontMember_aktuell To UBound(Temp)
            'NDSResult alle bisher gefundene Fronten
            NDSResult(Position) = Temp(i)
            Position = Position + 1
        Next i

        'Die bereits klassifizierten Member werden aus dem Temp Array gelöscht
        If PES_Options.NNachf + PES_Options.NEltern - NFrontMember_gesamt > 0 Then
            ReDim Preserve Temp(PES_Options.NNachf + PES_Options.NEltern - NFrontMember_gesamt)
            'Der Flag wird zur klassifizierung in der nächsten Runde zurückgesetzt
            For i = 1 To UBound(Temp)
                Temp(i).dominated = False
            Next i
        End If

    End Sub

    '*******************************************************************************
    'Count_Front_Members
    '*******************************************************************************

    Private Function Count_Front_Members(ByRef aktuell_Front As Short, ByRef NDSResult() As Struct_NDSorting) As Integer
        Dim i As Short

        Count_Front_Members = 0

        For i = 1 To UBound(NDSResult)
            If NDSResult(i).Front = aktuell_Front Then
                Count_Front_Members = Count_Front_Members + 1
            End If
        Next i

    End Function

    '*******************************************************************************
    'NDS_Crowding_Distance_Sort
    '*******************************************************************************

    Private Sub NDS_Crowding_Distance_Sort(ByRef NDSorting() As Struct_NDSorting, ByRef start As Short, ByRef ende As Short)
        Dim i As Integer
        Dim j As Integer
        Dim k As Short

        Dim swap As Struct_NDSorting
        ReDim swap.d(PES_Options.varanz)
        ReDim swap.X(PES_Options.varanz)

        Dim fmin, fmax As Double

        For k = 1 To PES_Options.NPenalty
            For i = start To ende
                For j = start To ende
                    If NDSorting(i).penalty(k) < NDSorting(j).penalty(k) Then
                        swap = NDSorting(i)
                        NDSorting(i) = NDSorting(j)
                        NDSorting(j) = swap
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
                If NDSorting(i).distance > NDSorting(j).distance Then
                    swap = NDSorting(i)
                    NDSorting(i) = NDSorting(j)
                    NDSorting(j) = swap
                End If
            Next j
        Next i

    End Sub


    '*******************************************************************************
    'NDS_Crowding_Distance_Count
    '*******************************************************************************

    Private Function NDS_Crowding_Distance_Count(ByRef Qb(,,) As Double, ByRef Spannweite As Double) As Double

        Dim i As Short
        Dim j As Short
        Dim k As Short
        Dim TempDistance() As Double
        Dim PenaltyDistance(,) As Double

        Dim d() As Double
        Dim d_mean As Double

        ReDim TempDistance(PES_Options.NPenalty)
        ReDim PenaltyDistance(PES_Options.NEltern, PES_Options.NEltern)
        ReDim d(PES_Options.NEltern - 1)
        'Bestimmen der normierten Raumabstände zwischen allen Elternindividuen

        For i = 1 To PES_Options.NEltern
            PenaltyDistance(i, i) = 0
            For j = i + 1 To PES_Options.NEltern
                PenaltyDistance(i, j) = 0
                For k = 1 To PES_Options.NPenalty
                    TempDistance(k) = Qb(i, PES_Options.iaktuellePopulation, k) - Qb(j, PES_Options.iaktuellePopulation, k)
                    TempDistance(k) = TempDistance(k) * TempDistance(k)
                    PenaltyDistance(i, j) = PenaltyDistance(i, j) + TempDistance(k)
                Next k
                PenaltyDistance(i, j) = System.Math.Sqrt(PenaltyDistance(i, j))
                PenaltyDistance(j, i) = PenaltyDistance(i, j)
            Next j
        Next i

        d_mean = 0

        For i = 1 To PES_Options.NEltern - 1
            d(i) = 1.0E+300
            For j = 1 To i - 1
                If PenaltyDistance(i, j) < d(i) Then d(i) = PenaltyDistance(i, j)
            Next j
            For j = i + 1 To PES_Options.NEltern
                If PenaltyDistance(i, j) < d(i) Then d(i) = PenaltyDistance(i, j)
            Next j
            d_mean = d_mean + d(i)
        Next i

        d_mean = d_mean / PES_Options.NEltern

        NDS_Crowding_Distance_Count = 0

        For i = 1 To PES_Options.NEltern - 1
            NDS_Crowding_Distance_Count = NDS_Crowding_Distance_Count + (d_mean - d(i)) * (d_mean - d(i))
        Next i

        NDS_Crowding_Distance_Count = NDS_Crowding_Distance_Count / PES_Options.NEltern

        NDS_Crowding_Distance_Count = System.Math.Sqrt(NDS_Crowding_Distance_Count)

        Spannweite = 0
        For i = 1 To PES_Options.NEltern
            'TODO: sollte hier nicht j = i + 1 stehen?
            For j = i To PES_Options.NEltern
                If PenaltyDistance(i, j) > Spannweite Then Spannweite = PenaltyDistance(i, j)
            Next j
        Next i

    End Function
    '*******************************************************************************
    'Neighbourhood_AbstandsArray
    'Bestimme Array der Raumabstände für Neighbourhood-Rekombination
    '*******************************************************************************
    Private Sub Neighbourhood_AbstandsArray(ByRef PenaltyDistance(,) As Double, ByRef Qb(,,) As Double)

        Dim i As Short
        Dim j As Short
        Dim k As Short
        Dim MinMax() As Double
        Dim Min, Max As Double
        Dim TempDistance() As Double

        'Bestimmen des Normierungsfaktors für jede Dimension des Lösungsraums (MinMax)
        ReDim MinMax(PES_Options.NPenalty)
        For k = 1 To PES_Options.NPenalty
            MinMax(k) = 0
            Min = Qb(1, PES_Options.iaktuellePopulation, k)
            Max = Qb(1, PES_Options.iaktuellePopulation, k)
            For j = 1 To PES_Options.NEltern
                If Min > Qb(j, PES_Options.iaktuellePopulation, k) Then Min = Qb(j, PES_Options.iaktuellePopulation, k)
                If Max < Qb(j, PES_Options.iaktuellePopulation, k) Then Max = Qb(j, PES_Options.iaktuellePopulation, k)
            Next j
            MinMax(k) = Max - Min
        Next k

        'Bestimmen der normierten Raumabstände zwischen allen Elternindividuen
        ReDim TempDistance(PES_Options.NPenalty)

        For i = 1 To PES_Options.NEltern

            PenaltyDistance(i, i) = 0

            For j = i + 1 To PES_Options.NEltern

                PenaltyDistance(i, j) = 0

                For k = 1 To PES_Options.NPenalty

                    TempDistance(k) = Qb(i, PES_Options.iaktuellePopulation, k) - Qb(j, PES_Options.iaktuellePopulation, k)
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

    '*******************************************************************************
    'SekundärQb_Duplettten
    '
    '*******************************************************************************
    Private Sub SekundärQb_Duplettten(ByRef SekundärQb() As Struct_NDSorting)
        Dim i As Short
        Dim j As Short
        Dim k As Short
        Dim Logical As Boolean

        For i = 1 To UBound(SekundärQb) - 1
            For j = i + 1 To UBound(SekundärQb)
                Logical = True
                For k = 1 To PES_Options.NPenalty
                    Logical = Logical And (SekundärQb(i).penalty(k) = SekundärQb(j).penalty(k))
                Next k
                If Logical Then SekundärQb(i).dominated = True
            Next j
        Next i
    End Sub
    '*******************************************************************************
    'Neighbourhood_Eltern
    'Bestimme die NAnzahlEltern mit geringsten Raumabständen für Neighbourhood-Rekombination
    '*******************************************************************************
    Private Sub Neighbourhood_Eltern(ByRef PenaltyDistance(,) As Double, ByRef IndexElter As Short, ByRef NAnzahlEltern As Short, ByRef IndexEltern() As Short)

        Dim i As Short
        Dim j As Short
        Dim Nachbarn() As Struct_Neighbourhood
        Dim swap As Struct_Neighbourhood

        ReDim Nachbarn(PES_Options.NEltern - 1)

        For i = 1 To IndexElter - 1
            Nachbarn(i).distance = PenaltyDistance(IndexElter, i)
            Nachbarn(i).Index = i
        Next i
        For i = IndexElter + 1 To PES_Options.NEltern
            Nachbarn(i - 1).distance = PenaltyDistance(IndexElter, i)
            Nachbarn(i - 1).Index = i
        Next i

        For i = 1 To UBound(Nachbarn)
            For j = i To UBound(Nachbarn)
                If Nachbarn(i).distance > Nachbarn(j).distance Then
                    swap = Nachbarn(i)
                    Nachbarn(i) = Nachbarn(j)
                    Nachbarn(j) = swap
                End If
            Next
        Next

        For i = 1 To NAnzahlEltern
            IndexEltern(i) = Nachbarn(i).Index
        Next i

    End Sub

    '*******************************************************************************
    'Neighbourhood_Crowding_Distance
    'Bestimme die NAnzahlEltern mit geringsten Raumabständen für Neighbourhood-Rekombination
    '*******************************************************************************
    Private Sub Neighbourhood_Crowding_Distance(ByRef Distanceb() As Double, ByRef Qb(,,) As Double)
        Dim i As Integer
        Dim j As Integer
        Dim k As Short
        Dim QbTemp(,,) As Double
        Dim swap As Double
        Dim fmin, fmax As Double

        ReDim QbTemp(PES_Options.NEltern, PES_Options.NPopul, PES_Options.NPenalty)

        Array.Copy(Qb, QbTemp, Qb.GetLength(0))
        For i = 1 To PES_Options.NEltern
            Distanceb(i) = 0
        Next i

        For k = 1 To PES_Options.NPenalty
            For i = 1 To PES_Options.NEltern
                For j = 1 To PES_Options.NEltern
                    If QbTemp(i, PES_Options.iaktuellePopulation, k) < QbTemp(j, PES_Options.iaktuellePopulation, k) Then
                        swap = QbTemp(i, PES_Options.iaktuellePopulation, k)
                        QbTemp(i, PES_Options.iaktuellePopulation, k) = QbTemp(j, PES_Options.iaktuellePopulation, k)
                        QbTemp(j, PES_Options.iaktuellePopulation, k) = swap
                    End If
                Next j
            Next i

            fmin = QbTemp(1, PES_Options.iaktuellePopulation, k)
            fmax = QbTemp(PES_Options.NEltern, PES_Options.iaktuellePopulation, k)

            Distanceb(1) = 1.0E+300
            Distanceb(PES_Options.NEltern) = 1.0E+300

            For i = 2 To PES_Options.NEltern - 1
                Distanceb(i) = Distanceb(i) + (QbTemp(i + 1, PES_Options.iaktuellePopulation, k) - QbTemp(i - 1, PES_Options.iaktuellePopulation, k)) / (fmax - fmin)
            Next i
        Next k
    End Sub

    '*******************************************************************************
    'BestimmeMinWert
    '*******************************************************************************

    Private Function BestimmeMinWert(ByRef Wert1 As Double, ByRef Wert2 As Double) As Double
        If Wert1 > Wert2 Then
            BestimmeMinWert = Wert2
        Else
            BestimmeMinWert = Wert1
        End If
    End Function

    '*******************************************************************************
    'BestimmeMaxWert
    '*******************************************************************************

    Private Function BestimmeMaxWert(ByRef Wert1 As Double, ByRef Wert2 As Double) As Double

        If Wert1 < Wert2 Then
            BestimmeMaxWert = Wert2
        Else
            BestimmeMaxWert = Wert1
        End If
    End Function

    '*******************************************************************************
    'Sortiere
    '*******************************************************************************

    Private Sub Sortiere(ByRef t() As Double)
        Dim i As Integer
        Dim j As Integer
        Dim swap As String
        For i = 0 To UBound(t) - 1
            For j = i To UBound(t) - 1
                If t(i) > t(j) Then
                    swap = t(i)
                    t(i) = t(j)
                    t(j) = swap
                End If
            Next
        Next
    End Sub

    '*******************************************************************************
    'Class_Initialisieren
    '*******************************************************************************

    Private Sub Class_Initialisieren()

    End Sub
    Public Sub New()
        MyBase.New()
        Class_Initialisieren()
    End Sub

    '*******************************************************************************
    'Class_Terminieren
    '*******************************************************************************

    Private Sub Class_Terminieren()

    End Sub
    Protected Overrides Sub Finalize()
        Class_Terminieren()
        MyBase.Finalize()
    End Sub
End Class