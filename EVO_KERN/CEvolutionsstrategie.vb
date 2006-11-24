Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("CEvolutionsstrategie_NET.CEvolutionsstrategie")> Public Class CEvolutionsstrategie


	'*******************************************************************************
	'*******************************************************************************
	'**** Klasse CEvolutionsstrategie                                           ****
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
	'**** Dirk Muschalla                                                        ****
	'****                                                                       ****
	'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
	'**** TU Darmstadt                                                          ****
	'****                                                                       ****
	'**** Dezember 2003                                                         ****
	'****                                                                       ****
	'**** Letzte Änderung: Mai 2005                                             ****
	'*******************************************************************************
	'*******************************************************************************
	
	
	'*******************************************************************************
	'*******************************************************************************
	'Deklarationsteil
	'*******************************************************************************
	'*******************************************************************************
	
    Private Structure EigenschaftTyp
        Dim varanz As Short                 'Anzahl Parameter
        Dim NEltern As Short                'Anzahl Eltern
        Dim NNachf As Short                 'Anzahl Kinder
        Dim NGen As Short                   'Anzahl Generationen
        Dim NPenalty As Short               'Anzahl der Penaltyfunktionen
        Dim NConstrains As Short            'Anzahl der Randbedingungen
        Dim iEvoTyp As Short                'Typ der Evolutionsstrategie (+ oder ,)
        Dim iPopEvoTyp As Short             'Typ der Evolutionsstrategie (+ oder ,) auf
                                            'Populationsebene
        Dim iPopPenalty As Short            'Art der Beurteilung der Populationsgüte (Multiobjective)
        Dim isPOPUL As Boolean              'Mit Populationen
        Dim isMultiObjective As Boolean     'Mit zweiter Objective-function
        Dim isPareto As Boolean             'mit Non-Dominated Sorting (nur falls multiobjective)
        Dim isPareto3D As Boolean           'mit Non-Dominated Sorting (3 Objectives)
        Dim NRunden As Short                'Anzahl Runden
        Dim NPopul As Short                 'Anzahl Populationen
        Dim NPopEltern As Short             'Anzahl Populationseltern
        Dim iOptPopEltern As Short          'Ermittlung der Populationseltern (Mittelwert,
                                            'Rekombination)
        Dim iOptEltern As Short             'Ermittlung der Individuum-Eltern (Mittelwert,
                                            'Rekombination, einfache Auswahl)
        Dim NRekombXY As Short              'X/Y-Schema Rekombination
        Dim rDeltaMin As Single             'Mindestschrittweite
        Dim rDeltaStart As Single           'Startschrittweite
        Dim iStartPar As Short              'Startaparameter (zufällig, Originalparameter)
        Dim isDnVektor As Boolean           'Soll ein Schrittweitenvektor benutz werden
        Dim Dn() As Double                  'Schrittweitenvektor
        Dim An() As Double                  'Drehwinkelmatrix
        Dim Xn() As Double                  'aktuelle Variablenwerte
        Dim Xmin() As Double                'untere Schranke
        Dim Xmax() As Double                'Obere Schranke
        Dim iaktuelleRunde As Short         'Zähler für aktuelle Runde
        Dim iaktuellePopulation As Short    'Zähler für aktuelle Population
        Dim iaktuelleGeneration As Short    'Zähler für aktuelle Generation
        Dim iaktuellerNachfahre As Short    'Zähler für aktuellen Nachfahre
        Dim d As Double                     'Faktor für Rekombinationsoperator
        Dim interact As Short               'Alle wieviel Generationen soll die aktuelle Population
                                            'mit Mitgliedern der sekundären Population aufgefüllt werden
        Dim isInteract As Boolean           'Mit Austausch zwischen Population und Sekundärer Population
        Dim NMemberSecondPop As Short       'Maximale Anzahl Mitglieder der Sekundärpopulation
    End Structure

    'UPGRADE_WARNING: Arrays in Struktur Property müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1063"'
    Dim Eigenschaft As EigenschaftTyp

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
    Private SekundärQb() As NDSortingType   'Sekundäre Population
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
    '*******************************************************************************
    'Deklarationsteil für Non-Dominated Sorting
    '*******************************************************************************
    '*******************************************************************************

    Private Structure NDSortingType
        Dim penalty() As Double             'Werte der Penaltyfunktion(en)
        Dim constrain() As Double           'Werte der Randbedingung(en)
        Dim feasible As Boolean             'Gültiges Ergebnis ?
        Dim dominated As Boolean            'Kennzeichnung ob dominiert
        Dim Front As Short                  'Nummer der Pareto Front
        Dim X() As Double                   'Wert der Variablen
        Dim d() As Double                   'Schrittweite der Variablen
        Dim distance As Double              'Distanzwert für Crowding distance sort
    End Structure

    Dim NDSorting() As NDSortingType

    Private Structure Sortierung
        Dim Index As Short
        Dim penalty() As Double
    End Structure

    Private Structure Neighbourhood
        Dim distance As Double
        Dim Index As Short
    End Structure

    '*******************************************************************************
    '*******************************************************************************
    'Deklarationsteil allgemein
    '*******************************************************************************
    '*******************************************************************************

    'Evo-Strategie-Typ:
    Private Const EVO_PLUS As Short = 1
    Private Const EVO_KOMMA As Short = 2


    'Property varanz
    '***********************************************
    Public Property varanz() As Short
        Get
            Return Eigenschaft.varanz
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.varanz = Value
        End Set
    End Property

    'Property NEltern
    '***********************************************
    Public Property NEltern() As Short
        Get
            Return Eigenschaft.NEltern
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.NEltern = Value
        End Set
    End Property

    'Property NNachf
    '***********************************************
    Public Property NNachf() As Short
        Get
            Return Eigenschaft.NNachf
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.NNachf = Value
        End Set
    End Property

    'Property NGen
    '***********************************************
    Public Property NGen() As Short
        Get
            Return Eigenschaft.NGen
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.NGen = Value
        End Set
    End Property

    'Property NPenalty
    '***********************************************
    Public Property NPenalty() As Short
        Get
            Return Eigenschaft.NPenalty
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.NPenalty = Value
        End Set
    End Property

    'Property iEvoTyp
    '***********************************************
    Public Property iEvoTyp() As Short
        Get
            Return Eigenschaft.iEvoTyp
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.iEvoTyp = Value
        End Set
    End Property

    'Property iPopEvoTyp
    '***********************************************
    Public Property iPopEvoTyp() As Short
        Get
            Return Eigenschaft.iPopEvoTyp
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.iPopEvoTyp = Value
        End Set
    End Property

    'Property iPopPenalty
    '***********************************************
    Public Property iPopPenalty() As Short
        Get
            Return Eigenschaft.iPopPenalty
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.iPopPenalty = Value
        End Set
    End Property

    'Property isPOPUL
    '***********************************************
    Public Property isPOPUL() As Boolean
        Get
            Return Eigenschaft.isPOPUL
        End Get
        Set(ByVal Value As Boolean)
            Eigenschaft.isPOPUL = Value
        End Set
    End Property

    'Property isMultiObjective
    '***********************************************
    Public Property isMultiObjective() As Boolean
        Get
            Return Eigenschaft.isMultiObjective
        End Get
        Set(ByVal Value As Boolean)
            Eigenschaft.isMultiObjective = Value
        End Set
    End Property

    'Property isPareto
    '***********************************************
    Public Property isPareto() As Boolean
        Get
            Return Eigenschaft.isPareto
        End Get
        Set(ByVal Value As Boolean)
            Eigenschaft.isPareto = Value
        End Set
    End Property

    'Property isPareto3D
    '***********************************************
    Public Property isPareto3D() As Boolean
        Get
            Return Eigenschaft.isPareto3D
        End Get
        Set(ByVal Value As Boolean)
            Eigenschaft.isPareto3D = Value
        End Set
    End Property

    'Property NRunden
    '***********************************************
    Public Property NRunden() As Short
        Get
            Return Eigenschaft.NRunden
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.NRunden = Value
        End Set
    End Property

    'Property NPopul
    '***********************************************
    Public Property NPopul() As Short
        Get
            Return Eigenschaft.NPopul
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.NPopul = Value
        End Set
    End Property

    'Property NPopEltern
    '***********************************************
    Public Property NPopEltern() As Short
        Get
            Return Eigenschaft.NPopEltern
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.NPopEltern = Value
        End Set
    End Property

    'Property iOptPopEltern
    '***********************************************
    Public Property iOptPopEltern() As Short
        Get
            Return Eigenschaft.iOptPopEltern
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.iOptPopEltern = Value
        End Set
    End Property

    'Property iOptEltern
    '***********************************************
    Public Property iOptEltern() As Short
        Get
            Return Eigenschaft.iOptEltern
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.iOptEltern = Value
        End Set
    End Property

    'Property rDeltaMin
    '***********************************************
    Public Property rDeltaMin() As Single
        Get
            Return Eigenschaft.rDeltaMin
        End Get
        Set(ByVal Value As Single)
            Eigenschaft.rDeltaMin = Value
        End Set
    End Property

    'Property rDeltaStart
    '***********************************************
    Public Property rDeltaStart() As Single
        Get
            Return Eigenschaft.rDeltaStart
        End Get
        Set(ByVal Value As Single)
            Eigenschaft.rDeltaStart = Value
        End Set
    End Property

    'Property iStartPar
    '***********************************************
    Public Property iStartPar() As Short
        Get
            Return Eigenschaft.iStartPar
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.iStartPar = Value
        End Set
    End Property

    'Property isDnVektor
    '***********************************************
    Public Property isDnVektor() As Boolean
        Get
            Return Eigenschaft.isDnVektor
        End Get
        Set(ByVal Value As Boolean)
            Eigenschaft.isDnVektor = Value
        End Set
    End Property

    'Property iaktuelleRunde
    '***********************************************
    Public Property iaktuelleRunde() As Short
        Get
            Return Eigenschaft.iaktuelleRunde
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.iaktuelleRunde = Value
        End Set
    End Property

    'Property iaktuelleGeneration
    '***********************************************
    Public Property iaktuelleGeneration() As Short
        Get
            Return Eigenschaft.iaktuelleGeneration
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.iaktuelleGeneration = Value
        End Set
    End Property

    'Property iaktuellePopulation
    '***********************************************
    Public Property iaktuellePopulation() As Short
        Get
            Return Eigenschaft.iaktuellePopulation
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.iaktuellePopulation = Value
        End Set
    End Property

    'Property iaktuellerNachfahre
    '***********************************************
    Public Property iaktuellerNachfahre() As Short
        Get
            Return Eigenschaft.iaktuellerNachfahre
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.iaktuellerNachfahre = Value
        End Set
    End Property

   'Property interact
    '***********************************************
    Public Property interact() As Short
        Get
            Return Eigenschaft.interact
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.interact = Value
        End Set
    End Property

    'Property isInteract
    '***********************************************
    Private Property isInteract() As Boolean
        Get
            Return Eigenschaft.isInteract
        End Get
        Set(ByVal Value As Boolean)
            Eigenschaft.isInteract = Value
        End Set
    End Property

    'Property NMemberSecondPop
    '***********************************************
    Private Property NMemberSecondPop() As Short
        Get
            Return Eigenschaft.NMemberSecondPop
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.NMemberSecondPop = Value
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

        Eigenschaft.varanz = AnzahlParameter 'Anzahl der Parameter wird übergeben
        Eigenschaft.NPenalty = AnzahlPenaltyfunktionen 'Anzahl der Zielfunktionen wird
        'übergeben
        Eigenschaft.NConstrains = AnzahlRandbedingungen 'Anzahl der Randbedingungen wird
        'übergeben

        ReDim Eigenschaft.Xn(Eigenschaft.varanz) 'Variablenvektor wird initialisiert
        ReDim Eigenschaft.Xmin(Eigenschaft.varanz) 'UntereSchrankenvektor wird initialisiert
        ReDim Eigenschaft.Xmax(Eigenschaft.varanz) 'ObereSchrankenvektor wird initialisiert
        ReDim Eigenschaft.Dn(Eigenschaft.varanz) 'Schrittweitenvektor wird initialisiert

        EsIni = True
        Exit Function

ES_INI_ERROR:
        Exit Function

    End Function

    '*******************************************************************************
    'ES_OPTIONS
    '*******************************************************************************

    'Function ES_OPTIONS übergibt Optionen für Evolutionsstrategie
    Public Function EsOptions(ByRef iEvoTyp As Integer, ByRef iPopEvoTyp As Integer, ByRef isPOPUL As Boolean, ByRef NRunden As Integer, ByRef NPopul As Integer, ByRef NPopEltern As Integer, ByRef iOptPopEltern As Integer, ByRef iOptEltern As Integer, ByRef iPopPenalty As Integer, ByRef NGen As Integer, ByRef NEltern As Integer, ByRef NNachf As Integer, ByRef NRekombXY As Integer, ByRef rDeltaStart As Single, ByRef iStartPar As Integer, ByRef isDnVektor As Boolean, ByRef isMultiObjective As Boolean, ByRef isPareto As Boolean, ByRef isPareto3D As Boolean, ByRef interact As Short, ByRef isInteract As Boolean, ByRef NMemberSecondPop As Short) As Boolean

        EsOptions = False

        On Error GoTo ES_OPTIONS_ERROR

        'Überprüfung der Übergebenen Werte
        If ((iEvoTyp < 1 Or iEvoTyp > 2) Or (iPopEvoTyp < 1 Or iPopEvoTyp > 2) Or NRunden < 1 Or NPopul < 1 Or NPopEltern < 1 Or (iOptPopEltern < 1 Or iOptPopEltern > 3) Or (iOptEltern < 1 Or iOptEltern > 6) Or NEltern < 1 Or NNachf < 1 Or NGen < 1 Or NRekombXY < 1 Or rDeltaStart < 0 Or (iStartPar < 1 Or iStartPar > 2)) Then GoTo ES_OPTIONS_ERROR

        'Übergabe der Optionen
        Eigenschaft.iEvoTyp = iEvoTyp
        Eigenschaft.iPopEvoTyp = iPopEvoTyp
        Eigenschaft.isPOPUL = isPOPUL
        Eigenschaft.NRunden = NRunden
        Eigenschaft.NPopul = NPopul
        Eigenschaft.NPopEltern = NPopEltern
        Eigenschaft.iOptPopEltern = iOptPopEltern
        Eigenschaft.iOptEltern = iOptEltern
        Eigenschaft.iPopPenalty = iPopPenalty
        Eigenschaft.NEltern = NEltern
        Eigenschaft.NNachf = NNachf
        Eigenschaft.NGen = NGen
        Eigenschaft.NRekombXY = NRekombXY
        Eigenschaft.rDeltaStart = rDeltaStart
        Eigenschaft.iStartPar = iStartPar
        Eigenschaft.isDnVektor = isDnVektor
        Eigenschaft.isMultiObjective = isMultiObjective
        Eigenschaft.isPareto = isPareto
        Eigenschaft.isPareto3D = isPareto3D
        Eigenschaft.d = 0.25
        Eigenschaft.interact = interact
        Eigenschaft.NMemberSecondPop = NMemberSecondPop

        If Not Eigenschaft.isPOPUL Then
            Eigenschaft.NPopul = 1
            Eigenschaft.NPopEltern = 1
            Eigenschaft.NRunden = 1
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

        Eigenschaft.Xn(i) = Parameter
        Eigenschaft.Xmin(i) = 0
        Eigenschaft.Xmax(i) = 1
        Eigenschaft.Xn(i) = BestimmeMinWert(Eigenschaft.Xn(i), Eigenschaft.Xmax(i))
        Eigenschaft.Xn(i) = BestimmeMaxWert(Eigenschaft.Xn(i), Eigenschaft.Xmin(i))

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
            mypara(i, 1) = Eigenschaft.Xn(i)
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
            mystep(i) = Eigenschaft.Dn(i)
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

        ReDim Bestwert(Eigenschaft.NEltern, Eigenschaft.NPenalty)
        For i = 1 To Eigenschaft.NPenalty ' dm 29.04.05
            For j = 1 To Eigenschaft.NEltern
                Bestwert(j, i) = Qb(j, Eigenschaft.iaktuellePopulation, i)
            Next j
        Next i

        EsGetBestwert = True

    End Function

    '*******************************************************************************
    'ES_GET_SEKUNDÄRE_POPULATIONEN
    '*******************************************************************************

    Public Function esGetSekundärePopulation(ByRef Population(,) As Double) As Boolean

        Dim j, i, k As Short

        esGetSekundärePopulation = False

        ''Notwendig, falls auch die Variablenwerte für eine spätere Datensatzerstellung ausgelesen werden sollen (Dirk)

        '    ReDim Population(UBound(SekundärQb), Property.varanz, Property.NPenalty + 1)
        '    For i = 1 To UBound(SekundärQb)
        '        For j = 1 To Property.varanz
        '            Population(i, j, 1) = SekundärQb(i).X(j)
        '            For k = 1 To Property.NPenalty
        '                Population(i, j, k + 1) = SekundärQb(i).penalty(k)
        '            Next k
        '        Next j
        '    Next i

        ReDim Population(UBound(SekundärQb), Eigenschaft.NPenalty)
        For i = 1 To UBound(SekundärQb)
            For j = 1 To Eigenschaft.NPenalty
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

    Public Function EsGetPopBestWert(ByRef POP_Bestwert(,,,) As Double) As Boolean

        Dim k, i, j, l As Short

        ReDim POP_Bestwert(Eigenschaft.NPopul, Eigenschaft.NEltern, Eigenschaft.varanz, Eigenschaft.NPenalty)
        ReDim POP_Bestwert(Eigenschaft.NPopul, Eigenschaft.NEltern, Eigenschaft.varanz, Eigenschaft.NPenalty + 1)
        For i = 1 To Eigenschaft.NPopul
            For j = 1 To Eigenschaft.NEltern
                For k = 1 To Eigenschaft.varanz
                    POP_Bestwert(i, j, k, 1) = Xb(k, j, i)
                    For l = 1 To Eigenschaft.NPenalty
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
    '$ Für Paretooptimierung noch nicht fertig!!!!

    Public Function EsPrepare() As Boolean
        Dim m, n, l As Short
        Dim w, v, i As Short

        EsPrepare = False

        On Error GoTo ES_PREPARE_ERROR

        For i = 1 To Eigenschaft.varanz
            Eigenschaft.Dn(i) = Eigenschaft.rDeltaStart
        Next i

        'Parametervektoren initialisieren
        ReDim Dp(Eigenschaft.varanz, Eigenschaft.NEltern, Eigenschaft.NPopEltern)
        ReDim Xp(Eigenschaft.varanz, Eigenschaft.NEltern, Eigenschaft.NPopEltern)
        ReDim Qbpop(Eigenschaft.NPopul, Eigenschaft.NPenalty)
        ReDim QbpopD(Eigenschaft.NPopul)
        ReDim Dbpop(Eigenschaft.varanz, Eigenschaft.NEltern, Eigenschaft.NPopul)
        ReDim Xbpop(Eigenschaft.varanz, Eigenschaft.NEltern, Eigenschaft.NPopul)
        '---------------------
        ReDim De(Eigenschaft.varanz, Eigenschaft.NEltern, Eigenschaft.NPopul)
        ReDim Xe(Eigenschaft.varanz, Eigenschaft.NEltern, Eigenschaft.NPopul)
        ReDim Db(Eigenschaft.varanz, Eigenschaft.NEltern, Eigenschaft.NPopul)
        ReDim Xb(Eigenschaft.varanz, Eigenschaft.NEltern, Eigenschaft.NPopul)
        ReDim Qb(Eigenschaft.NEltern, Eigenschaft.NPopul, Eigenschaft.NPenalty)
        ReDim Rb(Eigenschaft.NEltern, Eigenschaft.NPopul, Eigenschaft.NPenalty)
        '---------------------
        'NDSorting wird nur benötigt, falls eine Paretofront approximiert wird
        If Eigenschaft.isPareto Then
            ReDim NDSorting(Eigenschaft.NEltern + Eigenschaft.NNachf)
            For i = 1 To Eigenschaft.NEltern + Eigenschaft.NNachf
                ReDim NDSorting(i).penalty(Eigenschaft.NPenalty)
                If Eigenschaft.NConstrains > 0 Then
                    ReDim NDSorting(i).constrain(Eigenschaft.NConstrains)
                End If
                ReDim NDSorting(i).d(Eigenschaft.varanz)
                ReDim NDSorting(i).X(Eigenschaft.varanz)
            Next i
            If Eigenschaft.iOptEltern = EVO_ELTERN_Neighbourhood Then
                ReDim PenaltyDistance(Eigenschaft.NEltern, Eigenschaft.NEltern)
                ReDim IndexEltern(Eigenschaft.NEltern - 1)
                ReDim Distanceb(Eigenschaft.NEltern)
            End If
        End If

        For n = 1 To Eigenschaft.NEltern
            For m = 1 To Eigenschaft.NPopul
                For l = 1 To Eigenschaft.NPenalty
                    'Qualität der Eltern (Anzahl = parents) wird auf sehr großen Wert gesetzt
                    Qb(n, m, l) = 1.0E+300
                Next l
                If Eigenschaft.NConstrains > 0 Then
                    For l = 1 To Eigenschaft.NPenalty
                        'Restriktion der Eltern (Anzahl = parents) wird auf sehr kleinen Wert gesetzt
                        Rb(n, m, l) = -1.0E+300
                    Next l
                End If
            Next m
        Next

        If Eigenschaft.isMultiObjective And Eigenschaft.isPareto Then
            For n = 1 To Eigenschaft.NPopul
                For m = 1 To Eigenschaft.NPenalty
                    Select Case Eigenschaft.iPopPenalty
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
            For n = 1 To Eigenschaft.NPopul
                For m = 1 To Eigenschaft.NPenalty
                    'Qualität der Populationseltern wird auf sehr großen Wert gesetzt
                    Qbpop(n, m) = 1.0E+300
                Next m
            Next n
        End If

        'Zufallsgenerator initialisieren
        Randomize()

        'Informationen über aktuelle Runden übergeben
        Eigenschaft.iaktuelleRunde = 0
        Eigenschaft.iaktuellePopulation = 0
        Eigenschaft.iaktuelleGeneration = 0
        Eigenschaft.iaktuellerNachfahre = 0

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

        Select Case Eigenschaft.iStartPar
            Case 1 'Zufälligen Startwerte
                For v = 1 To Eigenschaft.varanz
                    For n = 1 To Eigenschaft.NEltern
                        For m = 1 To Eigenschaft.NPopEltern
                            'Startwert für die Elternschrittweite wird zugewiesen
                            Dp(v, n, m) = Eigenschaft.Dn(1)
                            'Startwert für die Eltern werden zugewiesen
                            '(Zufallszahl zwischen 0 und 1)
                            Xp(v, n, m) = Rnd()
                        Next m
                    Next n
                Next v
            Case 2 'Originalparameter
                For v = 1 To Eigenschaft.varanz
                    For n = 1 To Eigenschaft.NEltern
                        For m = 1 To Eigenschaft.NPopEltern
                            'Startwert für die Elternschrittweite wird zugewiesen
                            Dp(v, n, m) = Eigenschaft.Dn(1)
                            'Startwert für die Eltern werden zugewiesen
                            '(alle gleich Anfangswerte)
                            Xp(v, n, m) = Eigenschaft.Xn(v)
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
        Eigenschaft.iaktuellePopulation = Eigenschaft.iaktuellePopulation + 1

        'Abfrage ob die maximale Anzahl an Populationenschleifen erreicht ist
        If Eigenschaft.iaktuellePopulation <= Eigenschaft.NPopul Then
            EsIsNextPop = True
        Else
            Eigenschaft.iaktuellePopulation = 0
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
    Public Function EsIsNextRunde() As Boolean

        EsIsNextRunde = False

        On Error GoTo ES_isNEXTRUNDE_ERROR

        'Anzahl der Runden wird hochgezählt
        Eigenschaft.iaktuelleRunde = Eigenschaft.iaktuelleRunde + 1

        'Abfrage ob die maximale Anzahl an Runden erreicht ist
        If Eigenschaft.iaktuelleRunde <= Eigenschaft.NRunden Then
            EsIsNextRunde = True
        Else
            MsgBox("Optimierung beendet")
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
        Eigenschaft.iaktuelleGeneration = Eigenschaft.iaktuelleGeneration + 1

        'Abfrage ob die aktuelle Generation abgeschlossen ist
        If Eigenschaft.iaktuelleGeneration <= Eigenschaft.NGen Then
            EsIsNextGen = True
        Else
            Eigenschaft.iaktuelleGeneration = 0
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
        Eigenschaft.iaktuellerNachfahre = Eigenschaft.iaktuellerNachfahre + 1

        If Eigenschaft.iaktuellerNachfahre <= Eigenschaft.NNachf Then
            EsIsNextNachf = True
        Else
            Eigenschaft.iaktuellerNachfahre = 0
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

        Select Case Eigenschaft.iOptPopEltern
            Case 1 'MultiRekombination über alle Eltern (x/x,y) oder (x/x+y)
                For n = 1 To Eigenschaft.NEltern
                    R = Int(Eigenschaft.NPopEltern * Rnd()) + 1
                    For v = 1 To Eigenschaft.varanz
                        'Selektion der Schrittweite
                        De(v, n, Eigenschaft.iaktuellePopulation) = Dp(v, n, R)
                        'Selektion des Elter
                        Xe(v, n, Eigenschaft.iaktuellePopulation) = Xp(v, n, R)
                    Next v
                Next n
            Case 2 'Mittelwertbildung über alle Eltern
                'Ermitteln der Elter und Schrittweite über Mittelung der Elternschrittweiten
                For v = 1 To Eigenschaft.varanz
                    For n = 1 To Eigenschaft.NEltern
                        De(v, n, Eigenschaft.iaktuellePopulation) = 0
                        Xe(v, n, Eigenschaft.iaktuellePopulation) = 0
                        For m = 1 To Eigenschaft.NPopEltern
                            'Mittelung der Schrittweite,
                            De(v, n, Eigenschaft.iaktuellePopulation) = De(v, n, Eigenschaft.iaktuellePopulation) + (Dp(v, n, m) / Eigenschaft.NPopEltern)
                            'Mittelung der Eltern,
                            Xe(v, n, Eigenschaft.iaktuellePopulation) = Xe(v, n, Eigenschaft.iaktuellePopulation) + (Xp(v, n, m) / Eigenschaft.NPopEltern)
                        Next m
                    Next n
                Next v
            Case 3 'Zufallswahl über alle Eltern
                R = Int(Eigenschaft.NPopEltern * Rnd()) + 1 'Zufallszahl entscheidet welcher
                'Elternteil vererbt wird
                For v = 1 To Eigenschaft.varanz
                    For n = 1 To Eigenschaft.NEltern
                        'Selektion der Schrittweite
                        De(v, n, Eigenschaft.iaktuellePopulation) = Dp(v, n, R)
                        'Selektion des Elter
                        Xe(v, n, Eigenschaft.iaktuellePopulation) = Xp(v, n, R)
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

        Select Case Eigenschaft.iOptEltern

            Case 1 'Zufallswahl über alle Eltern

                R = Int(Eigenschaft.NEltern * Rnd()) + 1 'Zufallszahl entscheidet
                'welcher Enternteil vererbt wird
                For v = 1 To Eigenschaft.varanz
                    'Selektion der Schrittweite
                    Eigenschaft.Dn(v) = De(v, R, Eigenschaft.iaktuellePopulation)
                    'Selektion des Elter
                    Eigenschaft.Xn(v) = Xe(v, R, Eigenschaft.iaktuellePopulation)
                Next v

            Case 2 'Multi-Rekombination, diskret

                For v = 1 To Eigenschaft.varanz
                    R = Int(Eigenschaft.NEltern * Rnd()) + 1
                    'Selektion der Schrittweite
                    Eigenschaft.Dn(v) = De(v, R, Eigenschaft.iaktuellePopulation)
                    'Selektion des Elter
                    Eigenschaft.Xn(v) = Xe(v, R, Eigenschaft.iaktuellePopulation)
                Next v

            Case 3 'Multi-Rekombination, gemittelt

                For v = 1 To Eigenschaft.varanz
                    Eigenschaft.Dn(v) = 0
                    Eigenschaft.Xn(v) = 0

                    For n = 1 To Eigenschaft.NEltern
                        'Mittelung der Schrittweite,
                        Eigenschaft.Dn(v) = Eigenschaft.Dn(v) + (De(v, n, Eigenschaft.iaktuellePopulation) / Eigenschaft.NRekombXY)
                        'Mittelung der Eltern,
                        Eigenschaft.Xn(v) = Eigenschaft.Xn(v) + (Xe(v, n, Eigenschaft.iaktuellePopulation) / Eigenschaft.NRekombXY)
                    Next

                Next v

            Case 4 'Multi-Rekombination nach X/Y-Schema, diskrete Vertauschung

                ReDim Realisierungsspeicher(Eigenschaft.NRekombXY)
                ReDim Elternspeicher(Eigenschaft.NEltern)

                For i = 1 To Eigenschaft.NEltern
                    Elternspeicher(i) = i
                Next i

                For i = 1 To Eigenschaft.NRekombXY
                    R = Int((Eigenschaft.NEltern - (i - 1)) * Rnd()) + 1
                    Realisierungsspeicher(i) = Elternspeicher(R)

                    For j = R To (Eigenschaft.NEltern - 1)
                        Elternspeicher(R) = Elternspeicher(R + 1)
                    Next j

                Next i

                For v = 1 To Eigenschaft.varanz
                    R = Int(Eigenschaft.NRekombXY * Rnd()) + 1
                    'Selektion der Schrittweite
                    Eigenschaft.Dn(v) = De(v, Realisierungsspeicher(R), Eigenschaft.iaktuellePopulation)
                    'Selektion des Elter
                    Eigenschaft.Xn(v) = Xe(v, Realisierungsspeicher(R), Eigenschaft.iaktuellePopulation)
                Next v

            Case 5 'Multi-Rekombination nach X/Y-Schema, Mittelung der Gene

                ReDim Realisierungsspeicher(Eigenschaft.NRekombXY)
                ReDim Elternspeicher(Eigenschaft.NEltern)

                For i = 1 To Eigenschaft.NEltern
                    Elternspeicher(i) = i
                Next i

                For i = 1 To Eigenschaft.NRekombXY
                    R = Int((Eigenschaft.NEltern - (i - 1)) * Rnd()) + 1
                    Realisierungsspeicher(i) = Elternspeicher(R)

                    For j = R To (Eigenschaft.NEltern - 1)
                        Elternspeicher(R) = Elternspeicher(R + 1)
                    Next j

                Next i

                For v = 1 To Eigenschaft.varanz
                    Eigenschaft.Dn(v) = 0
                    Eigenschaft.Xn(v) = 0

                    For n = 1 To Eigenschaft.NRekombXY
                        'Mittelung der Schrittweite,
                        Eigenschaft.Dn(v) = Eigenschaft.Dn(v) + (De(v, Elternspeicher(n), Eigenschaft.iaktuellePopulation) / Eigenschaft.NRekombXY)
                        'Mittelung der Eltern,
                        Eigenschaft.Xn(v) = Eigenschaft.Xn(v) + (Xe(v, Elternspeicher(n), Eigenschaft.iaktuellePopulation) / Eigenschaft.NRekombXY)
                    Next

                Next v

            Case 6 'Neighbourhood Rekombination

                Z1 = Int(Eigenschaft.NEltern * Rnd()) + 1
                Do
                    Z2 = Int(Eigenschaft.NEltern * Rnd()) + 1
                Loop While Z1 = Z2

                'Tournament über Crowding Distance

                If Distanceb(Z1) > Distanceb(Z2) Then
                    Elter = Z1
                Else
                    Elter = Z2
                End If

                If (Elter = 1 Or Elter = Eigenschaft.NEltern) Then

                    For v = 1 To Eigenschaft.varanz
                        'Selektion der Schrittweite
                        Eigenschaft.Dn(v) = De(v, Elter, Eigenschaft.iaktuellePopulation)
                        'Selektion des Elter
                        Eigenschaft.Xn(v) = Xe(v, Elter, Eigenschaft.iaktuellePopulation)
                    Next

                Else

                    Call Neighbourhood_Eltern(PenaltyDistance, Elter, Eigenschaft.NRekombXY, IndexEltern)
                    For v = 1 To Eigenschaft.varanz
                        '                    Do
                        '                        Faktor = Rnd
                        '                        Faktor = (-1) * Property.d + Faktor * (1 + Property.d)
                        '                        'Selektion der Schrittweite
                        '                        Property.Dn(v) = De(v, IndexEltern(1), Property.iaktuellePopulation) * Faktor + _
                        ''                                         De(v, IndexEltern(2), Property.iaktuellePopulation) * (1 - Faktor)
                        '                        Property.Xn(v) = Xe(v, IndexEltern(1), Property.iaktuellePopulation) * Faktor + _
                        ''                                         Xe(v, IndexEltern(2), Property.iaktuellePopulation) * (1 - Faktor)
                        '                    Loop While (Property.Xn(v) <= Property.Xmin(v) Or Property.Xn(v) > Property.Xmax(v))

                        R = Int(Eigenschaft.NRekombXY * Rnd() + 1)
                        'Selektion der Schrittweite
                        Eigenschaft.Dn(v) = De(v, IndexEltern(R), Eigenschaft.iaktuellePopulation)
                        'Selektion des Elter
                        Eigenschaft.Xn(v) = Xe(v, IndexEltern(R), Eigenschaft.iaktuellePopulation)
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

        If Not Eigenschaft.isDnVektor Then
            '+/-1
            expo = (2 * Int(Rnd() + 0.5) - 1)
            'Schrittweite wird mutiert
            DeTemp = De(1, 1, Eigenschaft.iaktuellePopulation) * palpha ^ expo
        End If

        For v = 1 To Eigenschaft.varanz

            For n = 1 To Eigenschaft.NEltern

                Do

                    If Eigenschaft.isDnVektor Then
                        '+/-1
                        expo = (2 * Int(Rnd() + 0.5) - 1)
                        'Schrittweite wird mutiert
                        DeTemp = De(v, n, Eigenschaft.iaktuellePopulation) * palpha ^ expo
                    End If
                    'Normalverteilte Zufallszahl mit
                    'Standardabweichung 1/sqr(varanz)
                    Z = System.Math.Sqrt(-2 * System.Math.Log(1 - Rnd()) / Eigenschaft.varanz) * System.Math.Sin(6.2832 * Rnd())
                    'Mutation wird durchgeführt
                    XeTemp = Xe(v, n, Eigenschaft.iaktuellePopulation) + DeTemp * Z

                    ' Restriktion für die mutierten Werte
                Loop While (XeTemp <= Eigenschaft.Xmin(v) Or XeTemp > Eigenschaft.Xmax(v))

                De(v, n, Eigenschaft.iaktuellePopulation) = DeTemp
                Xe(v, n, Eigenschaft.iaktuellePopulation) = XeTemp

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

        If Not Eigenschaft.isDnVektor Then
            '+/-1
            expo = (2 * Int(Rnd() + 0.5) - 1)
            'Schrittweite wird mutiert
            DnTemp = Eigenschaft.Dn(1) * galpha ^ expo
        End If

        For v = 1 To Eigenschaft.varanz
            Do
                If Eigenschaft.isDnVektor Then
                    '+/-1
                    expo = (2 * Int(Rnd() + 0.5) - 1)
                    'Schrittweite wird mutiert
                    DnTemp = Eigenschaft.Dn(v) * galpha ^ expo
                End If
                'Normalverteilte Zufallszahl mit
                'Standardabweichung 1/sqr(varanz)
                Z = System.Math.Sqrt(-2 * System.Math.Log(1 - Rnd()) / Eigenschaft.varanz) * System.Math.Sin(6.2832 * Rnd())
                'Mutation wird durchgeführt
                XnTemp = Eigenschaft.Xn(v) + DnTemp * Z
                ' Restriktion für die mutierten Werte
            Loop While (XnTemp <= Eigenschaft.Xmin(v) Or XnTemp > Eigenschaft.Xmax(v))

            Eigenschaft.Dn(v) = DnTemp
            Eigenschaft.Xn(v) = XnTemp
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
        For m = 2 To Eigenschaft.NPopul
            If Not Eigenschaft.isPareto Then
                If Qbpop(m, 1) > h1 Then
                    h1 = Qbpop(m, 1)
                    i = m
                End If
            Else
                Select Case Eigenschaft.iPopPenalty
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
        If Eigenschaft.isMultiObjective Then
            j = 1
            h2 = Qbpop(1, 2)
            For m = 2 To Eigenschaft.NPopul
                If Not Eigenschaft.isPareto Then
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
        If Eigenschaft.isMultiObjective Then h2 = 0

        For m = 1 To Eigenschaft.NEltern
            If Not Eigenschaft.isMultiObjective Then
                h1 = h1 + Qb(m, Eigenschaft.iaktuellePopulation, 1) / Eigenschaft.NEltern
            Else
                If Not Eigenschaft.isPareto Then

                    Select Case Eigenschaft.iPopPenalty

                        Case 1 'Mittelwert
                            h1 = h1 + Qb(m, Eigenschaft.iaktuellePopulation, 1) / Eigenschaft.NEltern
                            h2 = h2 + Qb(m, Eigenschaft.iaktuellePopulation, 2) / Eigenschaft.NEltern

                        Case 2 'teuerster und schlechtester Wert der Population als Bewertung
                            If h1 < Qb(m, Eigenschaft.iaktuellePopulation, 1) Then
                                h1 = Qb(m, Eigenschaft.iaktuellePopulation, 1)
                            End If

                            If h2 < Qb(m, Eigenschaft.iaktuellePopulation, 2) Then
                                h2 = Qb(m, Eigenschaft.iaktuellePopulation, 2)
                            End If

                    End Select

                End If
            End If
        Next m

        If Eigenschaft.isPareto Then
            h1 = NDS_Crowding_Distance_Count(Qb, h2)
        End If

        'Falls die Qualität des aktuellen Population besser ist (Penaltyfunktion geringer)
        'als die schlechteste im Bestwertspeicher, wird diese ersetzt
        If Not Eigenschaft.isMultiObjective Then
            If h1 < Qbpop(i, 1) Then
                Qbpop(i, 1) = h1
                For m = 1 To Eigenschaft.varanz
                    For n = 1 To Eigenschaft.NEltern
                        'Die Schrittweite wird ebenfalls übernommen
                        Dbpop(m, n, i) = Db(m, n, Eigenschaft.iaktuellePopulation)
                        'Die eigentlichen Parameterwerte werden übernommen
                        Xbpop(m, n, i) = Xb(m, n, Eigenschaft.iaktuellePopulation)
                    Next n
                Next m
            End If
        Else
            If Not Eigenschaft.isPareto Then
                If h1 <= Qbpop(i, 1) And h2 < Qbpop(i, 2) Then
                    Qbpop(j, 1) = h1
                    Qbpop(j, 2) = h2
                    For m = 1 To Eigenschaft.varanz
                        For n = 1 To Eigenschaft.NEltern
                            'Die Schrittweite wird ebenfalls übernommen
                            Dbpop(m, n, i) = Db(m, n, Eigenschaft.iaktuellePopulation)
                            'Die eigentlichen Parameterwerte werden übernommen
                            Xbpop(m, n, i) = Xb(m, n, Eigenschaft.iaktuellePopulation)
                        Next n
                    Next m
                End If
            Else
                Select Case Eigenschaft.iPopPenalty
                    Case 1 'Crowding
                        If h1 < Qbpop(i, 1) Then
                            Qbpop(i, 1) = h1
                            For m = 1 To Eigenschaft.varanz
                                For n = 1 To Eigenschaft.NEltern
                                    'Die Schrittweite wird ebenfalls übernommen
                                    Dbpop(m, n, i) = Db(m, n, Eigenschaft.iaktuellePopulation)
                                    'Die eigentlichen Parameterwerte werden übernommen
                                    Xbpop(m, n, i) = Xb(m, n, Eigenschaft.iaktuellePopulation)
                                Next n
                            Next m
                        End If
                    Case 2
                        If h2 > Qbpop(j, 2) Then
                            Qbpop(j, 2) = h2
                            For m = 1 To Eigenschaft.varanz
                                For n = 1 To Eigenschaft.NEltern
                                    'Die Schrittweite wird ebenfalls übernommen
                                    Dbpop(m, n, j) = Db(m, n, Eigenschaft.iaktuellePopulation)
                                    'Die eigentlichen Parameterwerte werden übernommen
                                    Xbpop(m, n, j) = Xb(m, n, Eigenschaft.iaktuellePopulation)
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


        If Not Eigenschaft.isPareto Then 'Standard ES nach Rechenberg
            'Der schlechtetste der besten Qualitätswerte wird bestimmt ; Position -> j
            '(höchster Wert der Peanaltyfunktion)
            j = 1
            h = Qb(1, Eigenschaft.iaktuellePopulation, 1)

            For m = 2 To Eigenschaft.NEltern
                If Qb(m, Eigenschaft.iaktuellePopulation, 1) > h Then
                    h = Qb(m, Eigenschaft.iaktuellePopulation, 1)
                    j = m
                End If
            Next m

            'Falls die Qualität des aktuellen Nachkommen besser ist (Penaltyfunktion geringer)
            'als die schlechteste im Bestwertspeicher, wird diese ersetz
            If ZF(1) < Qb(j, Eigenschaft.iaktuellePopulation, 1) Then
                Qb(j, Eigenschaft.iaktuellePopulation, 1) = ZF(1)
                For v = 1 To Eigenschaft.varanz
                    'Die Schrittweite wird ebenfalls übernommen
                    Db(v, j, Eigenschaft.iaktuellePopulation) = Eigenschaft.Dn(v)
                    'Die eigentlichen Parameterwerte werden übernommen
                    Xb(v, j, Eigenschaft.iaktuellePopulation) = Eigenschaft.Xn(v)
                Next v
                If Eigenschaft.NPenalty = 2 Then
                    Qb(j, Eigenschaft.iaktuellePopulation, 2) = ZF(2)
                End If
            End If

        Else 'Multi-objective mit paretofront
            With NDSorting(Eigenschaft.iaktuellerNachfahre)
                For i = 1 To Eigenschaft.NPenalty
                    .penalty(i) = ZF(i)
                Next i
                .feasible = True
                For i = 1 To Eigenschaft.NConstrains
                    .constrain(i) = RF(i)
                    If .constrain(i) < 0 Then .feasible = False
                Next i
                .dominated = False
                .Front = 0
                For v = 1 To Eigenschaft.varanz
                    .d(v) = Eigenschaft.Dn(v)
                    .X(v) = Eigenschaft.Xn(v)
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

        If Eigenschaft.iEvoTyp = EVO_KOMMA Then
            If Not Eigenschaft.isPareto Then
                For n = 1 To Eigenschaft.NEltern
                    For i = 1 To Eigenschaft.NPenalty 'dm 29.04.05
                        Qb(n, Eigenschaft.iaktuellePopulation, i) = 1.0E+300 'dm 29.04.05
                    Next i 'dm 29.04.05
                Next n
            Else
                For n = 1 To Eigenschaft.NEltern
                    For i = 1 To Eigenschaft.NPenalty 'dm 29.04.05
                        Qb(n, Eigenschaft.iaktuellePopulation, i) = 0 'dm 29.04.05
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

        If Eigenschaft.iPopEvoTyp = EVO_KOMMA Then
            If Not Eigenschaft.isPareto Then
                For n = 1 To Eigenschaft.NPopul
                    For i = 1 To Eigenschaft.NPenalty 'dm 29.04.05
                        Qbpop(n, i) = 1.0E+300 'dm 29.04.05
                    Next i 'dm 29.04.05
                Next n
            Else
                For n = 1 To Eigenschaft.NPopul
                    For i = 1 To Eigenschaft.NPenalty 'dm 29.04.05
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
    'ES_POP_ELTERN
    '*******************************************************************************

    Public Function EsPopEltern() As Boolean
        Dim n, m, v As Short
        Dim swap(2) As Double
        Dim Realisierungsspeicher(,) As Double
        Dim Z As Short

        If Eigenschaft.isMultiObjective And Not Eigenschaft.isPareto Then
            Z = 2 'Kosten werden maßgeblich
        Else
            Select Case Eigenschaft.iPopPenalty
                Case 1 'Crowding
                    Z = 1 '
                Case 2 'Spannweite
                    Z = 2
            End Select
        End If

        EsPopEltern = False

        On Error GoTo ES_POP_ELTERN_ERROR

        ReDim Realisierungsspeicher(Eigenschaft.NPopul, 2)

        'Die NPopEltern besten Individium-Sätze werden ermittelt
        For m = 1 To Eigenschaft.NPopul
            Realisierungsspeicher(m, 1) = Qbpop(m, Z)
            Realisierungsspeicher(m, 2) = m
        Next m

        If Not Eigenschaft.isPareto Then
            For m = 1 To Eigenschaft.NPopul
                For n = m To Eigenschaft.NPopul
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
            Select Case Eigenschaft.iPopPenalty
                Case 1 'Crowding
                    For m = 1 To Eigenschaft.NPopul
                        For n = m To Eigenschaft.NPopul
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
                    For m = 1 To Eigenschaft.NPopul
                        For n = m To Eigenschaft.NPopul
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
        For m = 1 To Eigenschaft.NPopEltern
            For n = 1 To Eigenschaft.NEltern
                For v = 1 To Eigenschaft.varanz
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
        Dim Temp() As NDSortingType
        Dim NDSResult() As NDSortingType
        Dim Count, aktuelle_Front As Short
        Dim Member_Sekundärefront As Short


        EsEltern = False

        On Error GoTo ES_ELTERN_ERROR

        If Not Eigenschaft.isPareto Then 'Standard ES nach Rechenberg/Schwefel

            'Die Eltern werden gleich der besten Kinder gesetzt (Schrittweite und Parameterwert)
            For m = 1 To Eigenschaft.NEltern
                For v = 1 To Eigenschaft.varanz
                    De(v, m, Eigenschaft.iaktuellePopulation) = Db(v, m, Eigenschaft.iaktuellePopulation)
                    Xe(v, m, Eigenschaft.iaktuellePopulation) = Xb(v, m, Eigenschaft.iaktuellePopulation)
                Next v
            Next m

        Else 'Multi-objective mit Paretofront

            '1. Eltern und Nachfolger werden gemeinsam betrachtet
            '----------------------------------------------------

            For m = Eigenschaft.NNachf + 1 To Eigenschaft.NNachf + Eigenschaft.NEltern
                With NDSorting(m)
                    For l = 1 To Eigenschaft.NPenalty
                        .penalty(l) = Qb(m - Eigenschaft.NNachf, Eigenschaft.iaktuellePopulation, l)
                    Next l
                    If Eigenschaft.NConstrains > 0 Then
                        .feasible = True
                        For l = 1 To Eigenschaft.NConstrains
                            .constrain(l) = Rb(m - Eigenschaft.NNachf, Eigenschaft.iaktuellePopulation, l)
                            If .constrain(l) < 0 Then .feasible = False
                        Next l
                    End If
                    .dominated = False
                    .Front = 0
                    For v = 1 To Eigenschaft.varanz
                        'Die Schrittweite wird ebenfalls übernommen
                        .d(v) = Db(v, m - Eigenschaft.NNachf, Eigenschaft.iaktuellePopulation)
                        'Die eigentlichen Parameterwerte werden übernommen
                        .X(v) = Xb(v, m - Eigenschaft.NNachf, Eigenschaft.iaktuellePopulation)
                    Next v
                    .distance = 0
                End With
            Next m

            '2. Die einzelnen Fronten werden bestimmt
            '----------------------------------------
            durchlauf = 1
            NFrontMember_gesamt = 0

            ReDim Temp(Eigenschaft.NNachf + Eigenschaft.NEltern)

            For i = 1 To (Eigenschaft.NNachf + Eigenschaft.NEltern)
                ReDim Temp(i).d(Eigenschaft.varanz)
                ReDim Temp(i).X(Eigenschaft.varanz)
            Next i

            ReDim NDSResult(Eigenschaft.NNachf + Eigenschaft.NEltern)

            For i = 1 To (Eigenschaft.NNachf + Eigenschaft.NEltern)
                ReDim NDSResult(i).d(Eigenschaft.varanz)
                ReDim NDSResult(i).X(Eigenschaft.varanz)
            Next i

            Temp = VB6.CopyArray(NDSorting)

            Do
                Call Non_Dominated_Sorting(Temp, durchlauf) 'aktuallisiert auf n Objectives dm 10.05.05
                NFrontMember_aktuell = Non_Dominated_Count_and_Sort(Temp)
                NFrontMember_gesamt = NFrontMember_gesamt + NFrontMember_aktuell
                Call Non_Dominated_Result(Temp, NDSResult, NFrontMember_aktuell, NFrontMember_gesamt)
                durchlauf = durchlauf + 1
            Loop While Not (NFrontMember_gesamt = Eigenschaft.NEltern + Eigenschaft.NNachf)

            '3. Der Bestwertspeicher wird entsprechend der Fronten oder der
            'sekundären Population gefüllt
            '-------------------------------------------------------------
            NFrontMember_aktuell = 0
            NFrontMember_gesamt = 0
            aktuelle_Front = 1

            Do
                NFrontMember_aktuell = Count_Front_Members(aktuelle_Front, NDSResult)

                If NFrontMember_aktuell <= Eigenschaft.NEltern - NFrontMember_gesamt Then
                    For i = NFrontMember_gesamt + 1 To NFrontMember_aktuell + NFrontMember_gesamt
                        For j = 1 To Eigenschaft.NPenalty
                            Qb(i, Eigenschaft.iaktuellePopulation, j) = NDSResult(i).penalty(j)
                        Next j
                        If Eigenschaft.NConstrains > 0 Then
                            For j = 1 To Eigenschaft.NConstrains
                                Rb(i, Eigenschaft.iaktuellePopulation, j) = NDSResult(i).constrain(j)
                            Next j
                        End If
                        For v = 1 To Eigenschaft.varanz
                            Db(v, i, Eigenschaft.iaktuellePopulation) = NDSResult(i).d(v)
                            Xb(v, i, Eigenschaft.iaktuellePopulation) = NDSResult(i).X(v)
                        Next v

                    Next i
                    NFrontMember_gesamt = NFrontMember_gesamt + NFrontMember_aktuell
                Else
                    Call NDS_Crowding_Distance_Sort(NDSResult, NFrontMember_gesamt + 1, NFrontMember_gesamt + NFrontMember_aktuell)

                    For i = NFrontMember_gesamt + 1 To Eigenschaft.NEltern

                        For j = 1 To Eigenschaft.NPenalty
                            Qb(i, Eigenschaft.iaktuellePopulation, j) = NDSResult(i).penalty(j)
                        Next j
                        If Eigenschaft.NConstrains > 0 Then
                            For j = 1 To Eigenschaft.NConstrains
                                Rb(i, Eigenschaft.iaktuellePopulation, j) = NDSResult(i).constrain(j)
                            Next j
                        End If
                        For v = 1 To Eigenschaft.varanz
                            Db(v, i, Eigenschaft.iaktuellePopulation) = NDSResult(i).d(v)
                            Xb(v, i, Eigenschaft.iaktuellePopulation) = NDSResult(i).X(v)
                        Next v

                    Next i

                    NFrontMember_gesamt = Eigenschaft.NEltern

                End If

                aktuelle_Front = aktuelle_Front + 1

            Loop While Not (NFrontMember_gesamt = Eigenschaft.NEltern)

            'Sekundäre Population wird bestimmt und gespeichert
            NFrontMember_aktuell = Count_Front_Members(1, NDSResult)

            If Eigenschaft.iaktuelleRunde = 1 And Eigenschaft.iaktuellePopulation = 1 And Eigenschaft.iaktuelleGeneration = 1 Then
                ReDim Preserve SekundärQb(Member_Sekundärefront + NFrontMember_aktuell)
            Else
                Member_Sekundärefront = UBound(SekundärQb)
                ReDim Preserve SekundärQb(Member_Sekundärefront + NFrontMember_aktuell)
            End If

            For i = Member_Sekundärefront + 1 To Member_Sekundärefront + NFrontMember_aktuell
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts SekundärQb(i) konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                SekundärQb(i) = NDSResult(i - Member_Sekundärefront)
            Next i

            Call Non_Dominated_Sorting(SekundärQb, 1)
            NFrontMember_aktuell = Non_Dominated_Count_and_Sort_Sekundäre_Population(SekundärQb)
            ReDim Preserve SekundärQb(NFrontMember_aktuell)
            Call SekundärQb_Duplettten(SekundärQb)
            NFrontMember_aktuell = Non_Dominated_Count_and_Sort_Sekundäre_Population(SekundärQb)
            ReDim Preserve SekundärQb(NFrontMember_aktuell)

            If UBound(SekundärQb) > Eigenschaft.NMemberSecondPop Then
                Call NDS_Crowding_Distance_Sort(SekundärQb, 1, UBound(SekundärQb))
                ReDim Preserve SekundärQb(Eigenschaft.NMemberSecondPop)
            End If

            If (Eigenschaft.iaktuelleGeneration Mod Eigenschaft.interact) = 0 And Eigenschaft.isInteract Then
                NFrontMember_aktuell = Count_Front_Members(1, SekundärQb)
                If NFrontMember_aktuell > Eigenschaft.NEltern Then
                    Call NDS_Crowding_Distance_Sort(SekundärQb, 1, UBound(SekundärQb))
                    For i = 1 To Eigenschaft.NEltern

                        For j = 1 To Eigenschaft.NPenalty
                            Qb(i, Eigenschaft.iaktuellePopulation, j) = SekundärQb(i).penalty(j)
                        Next j
                        If Eigenschaft.NConstrains > 0 Then
                            For j = 1 To Eigenschaft.NConstrains
                                Rb(i, Eigenschaft.iaktuellePopulation, j) = SekundärQb(i).constrain(j)
                            Next j
                        End If
                        For v = 1 To Eigenschaft.varanz
                            Db(v, i, Eigenschaft.iaktuellePopulation) = SekundärQb(i).d(v)
                            Xb(v, i, Eigenschaft.iaktuellePopulation) = SekundärQb(i).X(v)
                        Next v

                    Next i
                End If
            End If

            'Neue Eltern werden gleich dem Bestwertspeicher gesetzt
            For m = 1 To Eigenschaft.NEltern
                For v = 1 To Eigenschaft.varanz
                    De(v, m, Eigenschaft.iaktuellePopulation) = Db(v, m, Eigenschaft.iaktuellePopulation)
                    Xe(v, m, Eigenschaft.iaktuellePopulation) = Xb(v, m, Eigenschaft.iaktuellePopulation)
                Next v
            Next m

            'Sortierung der Lösungen ist nur für Neighbourhood-Rekombination notwendig
            If Eigenschaft.iOptEltern = EVO_ELTERN_Neighbourhood Then
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
    '*******************************************************************************
    Private Sub Non_Dominated_Sorting(ByRef NDSorting() As NDSortingType, ByRef durchlauf As Short)

        Dim j, i, k As Short
        Dim Logical As Boolean
        Dim Summe_Constrain(2) As Double

        If Eigenschaft.NConstrains > 0 Then
            For i = 1 To UBound(NDSorting)
                For j = 1 To UBound(NDSorting)
                    If NDSorting(i).feasible And Not NDSorting(j).feasible Then

                        If NDSorting(j).dominated = False Then NDSorting(j).dominated = True

                    ElseIf ((Not NDSorting(i).feasible) And (Not NDSorting(j).feasible)) Then

                        Summe_Constrain(1) = 0
                        Summe_Constrain(2) = 0

                        For k = 1 To Eigenschaft.NConstrains
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

                        For k = 1 To Eigenschaft.NPenalty
                            Logical = Logical Or (NDSorting(i).penalty(k) < NDSorting(j).penalty(k))
                        Next k

                        For k = 1 To Eigenschaft.NPenalty
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

                    For k = 1 To Eigenschaft.NPenalty
                        Logical = Logical Or (NDSorting(i).penalty(k) < NDSorting(j).penalty(k))
                    Next k

                    For k = 1 To Eigenschaft.NPenalty
                        Logical = Logical And (NDSorting(i).penalty(k) <= NDSorting(j).penalty(k))
                    Next k

                    If Logical Then
                        If NDSorting(j).dominated = False Then NDSorting(j).dominated = True
                    End If
                Next j
            Next i
        End If

        For i = 1 To UBound(NDSorting)
            If NDSorting(i).dominated = False Then NDSorting(i).Front = durchlauf
        Next i

    End Sub

    '*******************************************************************************
    'Non_Dominated_Count_and_Sort
    '*******************************************************************************

    Private Function Non_Dominated_Count_and_Sort(ByRef NDSorting() As NDSortingType) As Short
        Dim j, i, v As Short
        Dim Temp() As NDSortingType
        Dim counter As Short

        ReDim Temp(UBound(NDSorting))

        For i = 1 To (UBound(NDSorting))
            ReDim Temp(i).d(Eigenschaft.varanz)
            ReDim Temp(i).X(Eigenschaft.varanz)
        Next i

        Non_Dominated_Count_and_Sort = 0
        counter = 0

        For i = 1 To UBound(NDSorting)
            If NDSorting(i).dominated = True Then
                counter = counter + 1
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Temp(counter) konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                Temp(counter) = NDSorting(i)
            End If
        Next i

        Non_Dominated_Count_and_Sort = UBound(NDSorting) - counter

        For i = 1 To UBound(NDSorting)
            If NDSorting(i).dominated = False Then
                counter = counter + 1
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Temp(counter) konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                Temp(counter) = NDSorting(i)
            End If
        Next i

        NDSorting = VB6.CopyArray(Temp)

    End Function

    '*******************************************************************************
    'Non_Dominated_Count_and_Sort
    '*******************************************************************************

    Private Function Non_Dominated_Count_and_Sort_Sekundäre_Population(ByRef NDSorting() As NDSortingType) As Short
        Dim j, i, v As Short
        Dim Temp() As NDSortingType
        Dim counter As Short

        ReDim Temp(UBound(NDSorting))

        For i = 1 To (UBound(NDSorting))
            ReDim Temp(i).d(Eigenschaft.varanz)
            ReDim Temp(i).X(Eigenschaft.varanz)
        Next i

        Non_Dominated_Count_and_Sort_Sekundäre_Population = 0
        counter = 0

        For i = 1 To UBound(NDSorting)
            If NDSorting(i).dominated = False Then
                counter = counter + 1
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Temp(counter) konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                Temp(counter) = NDSorting(i)
            End If
        Next i

        Non_Dominated_Count_and_Sort_Sekundäre_Population = counter

        For i = 1 To UBound(NDSorting)
            If NDSorting(i).dominated = True Then
                counter = counter + 1
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Temp(counter) konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                Temp(counter) = NDSorting(i)
            End If
        Next i


        NDSorting = VB6.CopyArray(Temp)

    End Function

    '*******************************************************************************
    'Non_Dominated_Result
    '*******************************************************************************

    Private Sub Non_Dominated_Result(ByRef Temp() As NDSortingType, ByRef NDSResult() As NDSortingType, ByRef NFrontMember_aktuell As Short, ByRef NFrontMember_gesamt As Short)

        Dim i, Position As Short

        Position = NFrontMember_gesamt - NFrontMember_aktuell + 1

        For i = UBound(Temp) + 1 - NFrontMember_aktuell To UBound(Temp)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NDSResult(Position) konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
            NDSResult(Position) = Temp(i)
            Position = Position + 1
        Next i
        If Eigenschaft.NNachf + Eigenschaft.NEltern - NFrontMember_gesamt > 0 Then
            ReDim Preserve Temp(Eigenschaft.NNachf + Eigenschaft.NEltern - NFrontMember_gesamt)
            For i = 1 To UBound(Temp)
                Temp(i).dominated = False
            Next i
        End If

    End Sub

    '*******************************************************************************
    'Count_Front_Members
    '*******************************************************************************

    Private Function Count_Front_Members(ByRef aktuell_Front As Short, ByRef NDSResult() As NDSortingType) As Integer
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

    Private Sub NDS_Crowding_Distance_Sort(ByRef NDSorting() As NDSortingType, ByRef start As Short, ByRef ende As Short)
        Dim i As Integer
        Dim j As Integer
        Dim k As Short
        'UPGRADE_WARNING: Arrays in Struktur swap müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1063"'
        Dim swap As NDSortingType
        Dim fmin, fmax As Double

        For k = 1 To Eigenschaft.NPenalty
            For i = start To ende
                For j = start To ende
                    If NDSorting(i).penalty(k) < NDSorting(j).penalty(k) Then
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts swap konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                        swap = NDSorting(i)
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NDSorting(i) konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                        NDSorting(i) = NDSorting(j)
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NDSorting(j) konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
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
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts swap konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                    swap = NDSorting(i)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NDSorting(i) konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                    NDSorting(i) = NDSorting(j)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NDSorting(j) konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
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
        Dim Min As Double
        Dim TempDistance() As Double
        Dim PenaltyDistance(,) As Double

        Dim d() As Double
        Dim d_mean As Double

        ReDim TempDistance(Eigenschaft.NPenalty)
        ReDim PenaltyDistance(Eigenschaft.NEltern, Eigenschaft.NEltern)
        ReDim d(Eigenschaft.NEltern - 1)
        'Bestimmen der normierten Raumabstände zwischen allen Elternindividuen

        For i = 1 To Eigenschaft.NEltern
            PenaltyDistance(i, i) = 0
            For j = i + 1 To Eigenschaft.NEltern
                PenaltyDistance(i, j) = 0
                For k = 1 To Eigenschaft.NPenalty
                    TempDistance(k) = Qb(i, Eigenschaft.iaktuellePopulation, k) - Qb(j, Eigenschaft.iaktuellePopulation, k)
                    TempDistance(k) = TempDistance(k) * TempDistance(k)
                    PenaltyDistance(i, j) = PenaltyDistance(i, j) + TempDistance(k)
                Next k
                PenaltyDistance(i, j) = System.Math.Sqrt(PenaltyDistance(i, j))
                PenaltyDistance(j, i) = PenaltyDistance(i, j)
            Next j
        Next i

        d_mean = 0

        For i = 1 To Eigenschaft.NEltern - 1
            d(i) = 1.0E+300
            For j = 1 To i - 1
                If PenaltyDistance(i, j) < d(i) Then d(i) = PenaltyDistance(i, j)
            Next j
            For j = i + 1 To Eigenschaft.NEltern
                If PenaltyDistance(i, j) < d(i) Then d(i) = PenaltyDistance(i, j)
            Next j
            d_mean = d_mean + d(i)
        Next i

        d_mean = d_mean / Eigenschaft.NEltern

        NDS_Crowding_Distance_Count = 0

        For i = 1 To Eigenschaft.NEltern - 1
            NDS_Crowding_Distance_Count = NDS_Crowding_Distance_Count + (d_mean - d(i)) * (d_mean - d(i))
        Next i

        NDS_Crowding_Distance_Count = NDS_Crowding_Distance_Count / Eigenschaft.NEltern

        NDS_Crowding_Distance_Count = System.Math.Sqrt(NDS_Crowding_Distance_Count)

        Spannweite = 0
        For i = 1 To Eigenschaft.NEltern
            For j = i To Eigenschaft.NEltern
                If PenaltyDistance(i, j) > Spannweite Then Spannweite = PenaltyDistance(i, j)
            Next j
        Next i

    End Function
    '*******************************************************************************
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
        ReDim MinMax(Eigenschaft.NPenalty)
        For k = 1 To Eigenschaft.NPenalty
            MinMax(k) = 0
            Min = Qb(1, Eigenschaft.iaktuellePopulation, k)
            Max = Qb(1, Eigenschaft.iaktuellePopulation, k)
            For j = 1 To Eigenschaft.NEltern
                If Min > Qb(j, Eigenschaft.iaktuellePopulation, k) Then Min = Qb(j, Eigenschaft.iaktuellePopulation, k)
                If Max < Qb(j, Eigenschaft.iaktuellePopulation, k) Then Max = Qb(j, Eigenschaft.iaktuellePopulation, k)
            Next j
            MinMax(k) = Max - Min
        Next k

        'Bestimmen der normierten Raumabstände zwischen allen Elternindividuen
        ReDim TempDistance(Eigenschaft.NPenalty)

        For i = 1 To Eigenschaft.NEltern

            PenaltyDistance(i, i) = 0

            For j = i + 1 To Eigenschaft.NEltern

                PenaltyDistance(i, j) = 0

                For k = 1 To Eigenschaft.NPenalty

                    TempDistance(k) = Qb(i, Eigenschaft.iaktuellePopulation, k) - Qb(j, Eigenschaft.iaktuellePopulation, k)
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

    Private Sub SekundärQb_Duplettten(ByRef SekundärQb() As NDSortingType)
        Dim i As Short
        Dim j As Short
        Dim k As Short
        Dim Logical As Boolean

        For i = 1 To UBound(SekundärQb) - 1
            For j = i + 1 To UBound(SekundärQb)
                Logical = True
                For k = 1 To Eigenschaft.NPenalty
                    Logical = Logical And (SekundärQb(i).penalty(k) = SekundärQb(j).penalty(k))
                Next k
                If Logical Then SekundärQb(i).dominated = True
            Next j
        Next i
    End Sub
    '*******************************************************************************
    'Bestimme die NAnzahlEltern mit geringsten Raumabständen für Neighbourhood-Rekombination
    '*******************************************************************************
    Private Sub Neighbourhood_Eltern(ByRef PenaltyDistance(,) As Double, ByRef IndexElter As Short, ByRef NAnzahlEltern As Short, ByRef IndexEltern() As Short)

        Dim i As Short
        Dim j As Short
        Dim Nachbarn() As Neighbourhood
        Dim swap As Neighbourhood

        ReDim Nachbarn(Eigenschaft.NEltern - 1)

        For i = 1 To IndexElter - 1
            Nachbarn(i).distance = PenaltyDistance(IndexElter, i)
            Nachbarn(i).Index = i
        Next i
        For i = IndexElter + 1 To Eigenschaft.NEltern
            Nachbarn(i - 1).distance = PenaltyDistance(IndexElter, i)
            Nachbarn(i - 1).Index = i
        Next i

        For i = 1 To UBound(Nachbarn)
            For j = i To UBound(Nachbarn)
                If Nachbarn(i).distance > Nachbarn(j).distance Then
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts swap konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                    swap = Nachbarn(i)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Nachbarn(i) konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                    Nachbarn(i) = Nachbarn(j)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Nachbarn(j) konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                    Nachbarn(j) = swap
                End If
            Next
        Next

        For i = 1 To NAnzahlEltern
            IndexEltern(i) = Nachbarn(i).Index
        Next i

    End Sub

    Private Sub Neighbourhood_Crowding_Distance(ByRef Distanceb() As Double, ByRef Qb(,,) As Double)
        Dim i As Integer
        Dim j As Integer
        Dim k As Short
        Dim QbTemp(,,) As Double
        Dim swap As Double
        Dim fmin, fmax As Double

        ReDim QbTemp(Eigenschaft.NEltern, Eigenschaft.NPopul, Eigenschaft.NPenalty)

        QbTemp = VB6.CopyArray(Qb)
        For i = 1 To Eigenschaft.NEltern
            Distanceb(i) = 0
        Next i

        For k = 1 To Eigenschaft.NPenalty
            For i = 1 To Eigenschaft.NEltern
                For j = 1 To Eigenschaft.NEltern
                    If QbTemp(i, Eigenschaft.iaktuellePopulation, k) < QbTemp(j, Eigenschaft.iaktuellePopulation, k) Then
                        swap = QbTemp(i, Eigenschaft.iaktuellePopulation, k)
                        QbTemp(i, Eigenschaft.iaktuellePopulation, k) = QbTemp(j, Eigenschaft.iaktuellePopulation, k)
                        QbTemp(j, Eigenschaft.iaktuellePopulation, k) = swap
                    End If
                Next j
            Next i

            fmin = QbTemp(1, Eigenschaft.iaktuellePopulation, k)
            fmax = QbTemp(Eigenschaft.NEltern, Eigenschaft.iaktuellePopulation, k)

            Distanceb(1) = 1.0E+300
            Distanceb(Eigenschaft.NEltern) = 1.0E+300

            For i = 2 To Eigenschaft.NEltern - 1
                Distanceb(i) = Distanceb(i) + (QbTemp(i + 1, Eigenschaft.iaktuellePopulation, k) - QbTemp(i - 1, Eigenschaft.iaktuellePopulation, k)) / (fmax - fmin)
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