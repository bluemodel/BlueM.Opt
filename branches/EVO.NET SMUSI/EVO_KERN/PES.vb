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
        Dim NEltern As Integer                'Anzahl Eltern
        Dim NNachf As Integer                 'Anzahl Kinder
        Dim NGen As Integer                   'Anzahl Generationen
        Dim iEvoTyp As EVO_STRATEGIE          'Typ der Evolutionsstrategie (+ oder ,)
        Dim iPopEvoTyp As EVO_STRATEGIE       'Typ der Evolutionsstrategie (+ oder ,) auf Populationsebene
        Dim iPopPenalty As EVO_POP_PENALTY    'Art der Beurteilung der Populationsgüte (Multiobjective)
        Dim isPOPUL As Boolean                'Mit Populationen
        Dim is_MO_Pareto As Boolean           'Multi-Objective mit Pareto Front
        Dim NRunden As Integer                'Anzahl Runden
        Dim NPopul As Integer                 'Anzahl Populationen
        Dim NPopEltern As Integer             'Anzahl Populationseltern
        Dim iOptPopEltern As EVO_POP_ELTERN   'Ermittlung der Populationseltern
        Dim iOptEltern As EVO_ELTERN          'Ermittlung der Individuum-Eltern
        Dim NRekombXY As Integer              'X/Y-Schema Rekombination
        Dim DnStart As Single                 'Startschrittweite
        Dim iStartPar As EVO_STARTPARAMETER   'Startparameter
        Dim isDnVektor As Boolean             'Soll ein Schrittweitenvektor benutzt werden
        Dim NInteract As Integer               'Alle wieviel Generationen soll die aktuelle Population mit Mitgliedern der sekundären Population aufgefüllt werden
        Dim isInteract As Boolean             'Mit Austausch zwischen Population und Sekundärer Population
        Dim NMemberSecondPop As Integer       'Maximale Anzahl Mitglieder der Sekundärpopulation
    End Structure

    Public PES_Settings As Struct_Settings

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
    Private NPara As Integer                  'Anzahl Parameter
    Private NPenalty As Integer               'Anzahl der Penaltyfunktionen
    Private NConstrains As Integer            'Anzahl der Randbedingungen

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
    Private Xb(,,) As Double                'Bestwertspeicher Variablenwerte für eine Generation
    Private Db(,,) As Double                'Bestwertspeicher Schrittweite für eine Generation
    Private Qb(,,) As Double                'Bestwertspeicher für eine Generation
    Private Rb(,,) As Double                'Restriktionen für eine Generation

    '---Stuff--------------
    Private Distanceb() As Double           'Array mit Crowding-Distance (Neighbourhood-Rekomb.)
    Private PenaltyDistance(,) As Double    'Array für normierte Raumabstände (Neighbourhood-Rekomb.)
    '---------------------
    Private SekundärQb() As Struct_NDSorting = {}   'Sekundäre Population

    Const galpha As Double = 1.3            'Faktor alpha=1.3 auf Generationsebene nach Rechenberg
    Const palpha As Double = 1.1            'Faktor alpha=1.1 auf Populationsebene nach Rechenberg

    'Deklarationsteil für Non-Dominated Sorting
    '******************************************
    Private Structure Struct_NDSorting
        Dim Penalty() As Double             '01 Werte der Penaltyfunktion(en)
        Dim Constrain() As Double           '02 Werte der Randbedingung(en)
        Dim feasible As Boolean             '03 Gültiges Ergebnis fehlt im Individuum
        Dim dominated As Boolean            '04 Kennzeichnung ob dominiert
        Dim Front As Integer                  '05 Nummer der Pareto Front
        Dim X() As Double                   '06 Wert der Variablen
        Dim d() As Double                   '07 Schrittweite der Variablen
        Dim distance As Double              '08 Distanzwert für Crowding distance sort

        'Überladene Methode um ein NDSorting zu Dimensionieren
        Public Shared Sub Dimit(ByVal NPenalty As Integer, ByVal NConstrains As Integer, ByVal NPara As Integer, ByRef TMP As Struct_NDSorting)
            Dim i As Integer

            ReDim TMP.penalty(NPenalty - 1)                      '01 Werte der Penaltyfunktion(en)
            For i = 0 To TMP.penalty.GetUpperBound(0)
                TMP.penalty(i) = 1.0E+300
            Next
            'Bug 135
            ReDim TMP.constrain(NConstrains - 1)                 '02 Werte der Randbedingung(en)
            For i = 0 To TMP.constrain.GetUpperBound(0)
                TMP.constrain(i) = -1.0E+300
            Next
            TMP.feasible = False                                 '03 Gültiges Ergebnis
            TMP.dominated = False                                '04 Kennzeichnung ob dominiert
            TMP.Front = 0                                        '05 Nummer der Pareto Front
            ReDim TMP.X(NPara - 1)                               '06 Wert der Variablen
            'Bug 135:
            For i = 0 To TMP.X.GetUpperBound(0)
                TMP.X(i) = 0
            Next
            ReDim TMP.d(NPara - 1)                               '07 Schrittweite der Variablen
            'Bug 135:
            For i = 0 To TMP.d.GetUpperBound(0)
                TMP.d(i) = 0
            Next
            TMP.distance = 0                                     '08 Distanzwert für Crowding distance sort
        End Sub

        'Überladene Methode um ein Array aus NDSorting zu Dimensionieren
        Public Shared Sub Dimit(ByVal NPenalty As Integer, ByVal NConstrains As Integer, ByVal NPara As Integer, ByRef TMP() As Struct_NDSorting)
            Dim i As Integer

            'Bug 135
            For i = 0 To TMP.GetUpperBound(0)
                Call Dimit(NPenalty, NConstrains, NPara, TMP(i))
            Next

        End Sub

        'Überladen Methode die ein Struct NDSorting kopiert
        Public Shared Sub Copy(ByVal Source As Struct_NDSorting, ByRef Dest As Struct_NDSorting)
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
        Public Shared Sub Copy(ByVal Source() As Struct_NDSorting, ByRef Dest() As Struct_NDSorting)
            Dim i As Integer
            'Bug 135:
            For i = 0 To Source.GetUpperBound(0)
                Call Copy(Source(i), Dest(i))
            Next
        End Sub

    End Structure

    Dim NDSorting() As Struct_NDSorting
    Dim swap As Struct_NDSorting

    Private Structure Struct_Sortierung
        Dim Index As Integer
        Dim penalty() As Double
    End Structure

    Private Structure Struct_Neighbourhood
        Dim Index As Integer
        Dim distance As Double
    End Structure

    'Deklarationsteil allgemein
    '**************************

    'Methoden
    '########

    'Initialisierung der PES
    '***************************************
    Public Sub PesInitialise(ByRef PES_Settings As Struct_Settings, ByVal AnzPara As Integer, ByVal AnzPenalty As Integer, ByVal AnzConstr As Integer, ByRef Parameter() As Double, ByVal beziehungen() As Beziehung, ByVal Method As String)

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
        NPara = AnzPara                         'Anzahl der Parameter wird übergeben
        NPenalty = AnzPenalty                   'Anzahl der Zielfunktionen wird übergeben
        NConstrains = AnzConstr                 'Anzahl der Randbedingungen wird übergeben

        'Dynamisches Array Initialisieren
        ReDim AktPara.Xn(NPara - 1)                'Variablenvektor wird initialisiert
        ReDim AktPara.Bez(NPara - 1)
        ReDim AktPara.Dn(NPara - 1)                'Schrittweitenvektor wird initialisiert

        'Parametervektoren initialisieren
        ReDim Dp(NPara - 1, PES_Settings.NEltern - 1, PES_Settings.NPopEltern - 1)
        ReDim Xp(NPara - 1, PES_Settings.NEltern - 1, PES_Settings.NPopEltern - 1)
        ReDim Qbpop(PES_Settings.NPopul - 1, NPenalty - 1)
        ReDim Dbpop(NPara - 1, PES_Settings.NEltern - 1, PES_Settings.NPopul - 1)
        ReDim Xbpop(NPara - 1, PES_Settings.NEltern - 1, PES_Settings.NPopul - 1)
        '---------------------
        ReDim De(NPara - 1, PES_Settings.NEltern - 1, PES_Settings.NPopul - 1)
        ReDim Xe(NPara - 1, PES_Settings.NEltern - 1, PES_Settings.NPopul - 1)
        '---------------------
        ReDim Db(NPara - 1, PES_Settings.NEltern - 1, PES_Settings.NPopul - 1)
        ReDim Xb(NPara - 1, PES_Settings.NEltern - 1, PES_Settings.NPopul - 1)
        ReDim Qb(PES_Settings.NEltern - 1, PES_Settings.NPopul - 1, NPenalty - 1)
        ReDim Rb(PES_Settings.NEltern - 1, PES_Settings.NPopul - 1, NConstrains - 1)

        'NDSorting wird nur benötigt, falls eine Paretofront approximiert wird
        If PES_Settings.is_MO_Pareto Then
            ReDim NDSorting(PES_Settings.NEltern + PES_Settings.NNachf - 1)
            For i = 0 To PES_Settings.NEltern + PES_Settings.NNachf - 1
                ReDim NDSorting(i).penalty(NPenalty - 1)
                ReDim NDSorting(i).constrain(NConstrains - 1)
                ReDim NDSorting(i).d(NPara - 1)
                ReDim NDSorting(i).X(NPara - 1)
            Next i
            If PES_Settings.iOptEltern = EVO_ELTERN.Neighbourhood Then
                ReDim PenaltyDistance(PES_Settings.NEltern - 1, PES_Settings.NEltern - 1)
                ReDim Distanceb(PES_Settings.NEltern - 1)
            End If
        End If

        For n = 0 To PES_Settings.NEltern - 1
            For m = 0 To PES_Settings.NPopul - 1
                For l = 0 To NPenalty - 1
                    'Qualität der Eltern (Anzahl = parents) wird auf sehr großen Wert gesetzt
                    Qb(n, m, l) = 1.0E+300
                Next l
                If NConstrains > 0 Then
                    For l = 0 To NConstrains - 1
                        'Restriktion der Eltern (Anzahl = parents) wird auf sehr kleinen Wert gesetzt
                        Rb(n, m, l) = -1.0E+300
                    Next l
                End If
            Next m
        Next

        'Falls NDSorting Crowding Distance wird initialisiert
        If PES_Settings.is_MO_Pareto Then
            For n = 0 To PES_Settings.NPopul - 1
                For m = 0 To NPenalty - 1
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
            For n = 0 To PES_Settings.NPopul - 1
                For m = 0 To NPenalty - 1
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
        For i = 0 To NPara - 1
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
        Select Case PES_Settings.iStartPar

            Case EVO_STARTPARAMETER.Zufall 'Zufällige Startwerte
                For v = 0 To NPara - 1
                    For n = 0 To PES_Settings.NEltern - 1
                        For m = 0 To PES_Settings.NPopEltern - 1
                            'Startwert für die Elternschrittweite wird zugewiesen
                            Dp(v, n, m) = AktPara.Dn(0)
                            'Startwert für die Eltern werden zugewiesen
                            '(Zufallszahl zwischen 0 und 1)
                            Xp(v, n, m) = Rnd()
                        Next m
                    Next n
                Next v

            Case EVO_STARTPARAMETER.Original 'Originalparameter
                For v = 0 To NPara - 1
                    For n = 0 To PES_Settings.NEltern - 1
                        For m = 0 To PES_Settings.NPopEltern - 1
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
        For v = 0 To NPara - 1
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

        ReDim Bestwert(PES_Settings.NEltern - 1, NPenalty - 1)

        For i = 0 To NPenalty - 1
            For j = 0 To PES_Settings.NEltern - 1
                Bestwert(j, i) = Qb(j, PES_iAkt.iAktPop, i)
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

        Select Case PES_Settings.iOptPopEltern

            Case EVO_POP_ELTERN.Rekombination 'MultiRekombination über alle Eltern (x/x,y) oder (x/x+y)
                For n = 0 To PES_Settings.NEltern - 1
                    R = Int(PES_Settings.NPopEltern * Rnd())
                    For v = 0 To NPara - 1
                        'Selektion der Schrittweite
                        De(v, n, PES_iAkt.iAktPop) = Dp(v, n, R)
                        'Selektion des Elter
                        Xe(v, n, PES_iAkt.iAktPop) = Xp(v, n, R)
                    Next v
                Next n

            Case EVO_POP_ELTERN.Mittelwert 'Mittelwertbildung über alle Eltern
                'Ermitteln der Elter und Schrittweite über Mittelung der Elternschrittweiten
                For v = 0 To NPara - 1
                    For n = 0 To PES_Settings.NEltern - 1
                        De(v, n, PES_iAkt.iAktPop) = 0
                        Xe(v, n, PES_iAkt.iAktPop) = 0
                        For m = 0 To PES_Settings.NPopEltern - 1
                            'Mittelung der Schrittweite,
                            De(v, n, PES_iAkt.iAktPop) = De(v, n, PES_iAkt.iAktPop) + (Dp(v, n, m) / PES_Settings.NPopEltern)
                            'Mittelung der Eltern,
                            Xe(v, n, PES_iAkt.iAktPop) = Xe(v, n, PES_iAkt.iAktPop) + (Xp(v, n, m) / PES_Settings.NPopEltern)
                        Next m
                    Next n
                Next v

            Case EVO_POP_ELTERN.Selektion 'Zufallswahl über alle Eltern
                R = Int(PES_Settings.NPopEltern * Rnd()) 'Zufallszahl entscheidet welcher
                'Elternteil vererbt wird
                For v = 0 To NPara - 1
                    For n = 0 To PES_Settings.NEltern - 1
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

        Select Case PES_Settings.iOptEltern

            Case EVO_ELTERN.Selektion 'Zufallswahl über alle Eltern

                R = Int(PES_Settings.NEltern * Rnd())    'Zufallszahl entscheidet
                'welcher Enternteil vererbt wird
                For v = 0 To NPara - 1
                    'Selektion der Schrittweite
                    AktPara.Dn(v) = De(v, R, PES_iAkt.iAktPop)
                    'Selektion des Elter
                    AktPara.Xn(v) = Xe(v, R, PES_iAkt.iAktPop)
                Next v

            Case EVO_ELTERN.XX_Diskret 'Multi-Rekombination, diskret

                For v = 0 To NPara - 1
                    R = Int(PES_Settings.NEltern * Rnd())
                    'Selektion der Schrittweite
                    AktPara.Dn(v) = De(v, R, PES_iAkt.iAktPop)
                    'Selektion des Elter
                    AktPara.Xn(v) = Xe(v, R, PES_iAkt.iAktPop)
                Next v

            Case EVO_ELTERN.XX_Mitteln 'Multi-Rekombination, gemittelt

                For v = 0 To NPara - 1
                    AktPara.Dn(v) = 0
                    AktPara.Xn(v) = 0

                    For n = 0 To PES_Settings.NEltern - 1
                        'Mittelung der Schrittweite,
                        AktPara.Dn(v) = AktPara.Dn(v) + (De(v, n, PES_iAkt.iAktPop) / PES_Settings.NRekombXY)
                        'Mittelung der Eltern,
                        AktPara.Xn(v) = AktPara.Xn(v) + (Xe(v, n, PES_iAkt.iAktPop) / PES_Settings.NRekombXY)
                    Next
                Next v

            Case EVO_ELTERN.XY_Diskret 'Multi-Rekombination nach X/Y-Schema, diskrete Vertauschung

                ReDim Realisierungsspeicher(PES_Settings.NRekombXY)
                ReDim Elternspeicher(PES_Settings.NEltern)

                For i = 0 To PES_Settings.NEltern - 1
                    Elternspeicher(i) = i
                Next i

                For i = 0 To PES_Settings.NRekombXY - 1
                    R = Int((PES_Settings.NEltern - (i)) * Rnd())
                    Realisierungsspeicher(i) = Elternspeicher(R)

                    For j = R To PES_Settings.NEltern - 1
                        Elternspeicher(R) = Elternspeicher(R + 1)
                    Next j

                Next i

                For v = 0 To NPara - 1
                    R = Int(PES_Settings.NRekombXY * Rnd())
                    'Selektion der Schrittweite
                    AktPara.Dn(v) = De(v, Realisierungsspeicher(R), PES_iAkt.iAktPop)
                    'Selektion des Elter
                    AktPara.Xn(v) = Xe(v, Realisierungsspeicher(R), PES_iAkt.iAktPop)
                Next v

            Case EVO_ELTERN.XY_Mitteln 'Multi-Rekombination nach X/Y-Schema, Mittelung der Gene

                ReDim Realisierungsspeicher(PES_Settings.NRekombXY)
                ReDim Elternspeicher(PES_Settings.NEltern)

                For i = 0 To PES_Settings.NEltern - 1
                    Elternspeicher(i) = i
                Next i

                For i = 0 To PES_Settings.NRekombXY - 1
                    R = Int((PES_Settings.NEltern - (i)) * Rnd())
                    Realisierungsspeicher(i) = Elternspeicher(R)
                    For j = R To (PES_Settings.NEltern - 2)
                        Elternspeicher(R) = Elternspeicher(R + 1)
                    Next j
                Next i

                For v = 0 To NPara - 1
                    AktPara.Dn(v) = 0
                    AktPara.Xn(v) = 0
                    For n = 0 To PES_Settings.NRekombXY - 1
                        'Mittelung der Schrittweite,
                        AktPara.Dn(v) = AktPara.Dn(v) + (De(v, Elternspeicher(n), PES_iAkt.iAktPop) / PES_Settings.NRekombXY)
                        'Mittelung der Eltern,
                        AktPara.Xn(v) = AktPara.Xn(v) + (Xe(v, Elternspeicher(n), PES_iAkt.iAktPop) / PES_Settings.NRekombXY)
                    Next
                Next v

            Case EVO_ELTERN.Neighbourhood 'Neighbourhood Rekombination

                Z1 = Int(PES_Settings.NEltern * Rnd())
                Do
                    Z2 = Int(PES_Settings.NEltern * Rnd())
                Loop While Z1 = Z2

                'Tournament über Crowding Distance
                If Distanceb(Z1) > Distanceb(Z2) Then
                    Elter = Z1
                Else
                    Elter = Z2
                End If

                If (Elter = 0 Or Elter = PES_Settings.NEltern - 1) Then
                    For v = 0 To NPara - 1
                        'Selektion der Schrittweite
                        AktPara.Dn(v) = De(v, Elter, PES_iAkt.iAktPop)
                        'Selektion des Elter
                        AktPara.Xn(v) = Xe(v, Elter, PES_iAkt.iAktPop)
                    Next
                Else
                    'BUG 135
                    Dim IndexEltern(PES_Settings.NEltern - 1) As Integer          'Array mit Index der Eltern (Neighbourhood-Rekomb.)
                    Call Neighbourhood_Eltern(Elter, IndexEltern)
                    For v = 0 To NPara - 1
                        'Do
                        '    Faktor = Rnd
                        '    Faktor = (-1) * Eigenschaft.d + Faktor * (1 + Eigenschaft.d)
                        '    'Selektion der Schrittweite
                        '    Eigenschaft.Dn(v) = De(v, IndexEltern(1), Eigenschaft.iaktuellePopulation) * Faktor + _
                        '                     De(v, IndexEltern(2), Eigenschaft.iaktuellePopulation) * (1 - Faktor)
                        '    Eigenschaft.Xn(v) = Xe(v, IndexEltern(1), Eigenschaft.iaktuellePopulation) * Faktor + _
                        '                     Xe(v, IndexEltern(2), Eigenschaft.iaktuellePopulation) * (1 - Faktor)
                        'Loop While (Eigenschaft.Xn(v) <= Eigenschaft.Xmin(v) Or Eigenschaft.Xn(v) > Eigenschaft.Xmax(v))

                        R = Int(PES_Settings.NRekombXY * Rnd())
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

        ReDim DeTemp(NPara - 1, PES_Settings.NEltern - 1, PES_Settings.NPopul - 1)
        ReDim XeTemp(NPara - 1, PES_Settings.NEltern - 1, PES_Settings.NPopul - 1)

StartMutation:

        'Einheitliche Schrittweite
        '-------------------------
        If (Not PES_Settings.isDnVektor) Then
            '+/-1
            expo = (2 * Int(Rnd() + 0.5) - 1)
            'Schrittweite wird mutiert
            DeTemp(0, 0, PES_iAkt.iAktPop) = De(0, 0, PES_iAkt.iAktPop) * palpha ^ expo
            'Schrittweite für alle übernehmen
            For n = 0 To PES_Settings.NEltern - 1
                For v = 0 To NPara - 1
                    DeTemp(v, n, PES_iAkt.iAktPop) = DeTemp(0, 0, PES_iAkt.iAktPop)
                Next
            Next
        End If


        'Mutation
        '--------
        For n = 0 To PES_Settings.NEltern - 1
            For v = 0 To NPara - 1
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
                    If (PES_Settings.isDnVektor) Then
                        '+/-1
                        expo = (2 * Int(Rnd() + 0.5) - 1)
                        'Schrittweite wird mutiert
                        DeTemp(v, n, PES_iAkt.iAktPop) = De(v, n, PES_iAkt.iAktPop) * palpha ^ expo
                    End If

                    'Normalverteilte Zufallszahl mit Standardabweichung 1/sqr(varanz)
                    Dim Z As Double
                    Z = System.Math.Sqrt(-2 * System.Math.Log(1 - Rnd()) / NPara) * System.Math.Sin(6.2832 * Rnd())
                    'Mutation wird durchgeführt
                    XeTemp(v, n, PES_iAkt.iAktPop) = Xe(v, n, PES_iAkt.iAktPop) + DeTemp(v, n, PES_iAkt.iAktPop) * Z

                    ' Restriktion für die mutierten Werte
                Loop While (XeTemp(v, n, PES_iAkt.iAktPop) <= 0 Or XeTemp(v, n, PES_iAkt.iAktPop) > 1 Or Not checkBeziehungPop(v, n, XeTemp))

            Next v

            'Mutierte Werte übernehmen
            '-------------------------
            For v = 0 To NPara - 1
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

        ReDim DnTemp(NPara - 1)
        ReDim XnTemp(NPara - 1)

StartMutation:

        'Einheitliche Schrittweite
        '-------------------------
        If (Not PES_Settings.isDnVektor) Then
            '+/-1
            expo = (2 * Int(Rnd() + 0.5) - 1)
            'Schrittweite wird mutiert
            DnTemp(0) = AktPara.Dn(0) * galpha ^ expo
            'Schrittweite für alle übernehmen
            For v = 1 To NPara - 1
                DnTemp(v) = DnTemp(0)
            Next
        End If

        'Mutation
        '--------
        For v = 0 To NPara - 1
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
                If (PES_Settings.isDnVektor) Then
                    '+/-1
                    expo = (2 * Int(Rnd() + 0.5) - 1)
                    'Schrittweite wird mutiert
                    DnTemp(v) = AktPara.Dn(v) * galpha ^ expo
                End If

                'Normalverteilte Zufallszahl mit Standardabweichung 1/sqr(varanz)
                Dim Z As Double
                Z = System.Math.Sqrt(-2 * System.Math.Log(1 - Rnd()) / NPara) * System.Math.Sin(6.2832 * Rnd())
                'Mutation wird durchgeführt
                XnTemp(v) = AktPara.Xn(v) + DnTemp(v) * Z

                'Restriktion für die mutierten Werte
            Loop While (XnTemp(v) <= 0 Or XnTemp(v) > 1 Or Not checkBeziehung(v, XnTemp))

        Next v

        'Mutierte Werte übernehmen
        '-------------------------
        For v = 0 To NPara - 1
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
        For m = 1 To PES_Settings.NPopul - 1
            If Not PES_Settings.is_MO_Pareto Then
                If Qbpop(m, 0) > h1 Then
                    h1 = Qbpop(m, 0)
                    i = m
                End If
            Else
                Select Case PES_Settings.iPopPenalty

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
            For m = 2 To PES_Settings.NPopul
                If Qbpop(m, 1) < h2 Then
                    h2 = Qbpop(m, 1)
                    j = m
                End If
            Next m
        End If

        'Qualität der aktuellen Population wird bestimmt
        h1 = 0
        If Not PES_Settings.is_MO_Pareto Then
            For m = 0 To PES_Settings.NEltern - 1
                h1 = h1 + Qb(m, PES_iAkt.iAktPop, 0) / PES_Settings.NEltern
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
                For m = 0 To NPara - 1
                    For n = 0 To PES_Settings.NEltern - 1
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
                    If h1 < Qbpop(i, 0) Then
                        Qbpop(i, 0) = h1
                        For m = 0 To NPara - 1
                            For n = 0 To PES_Settings.NEltern - 1
                                'Die Schrittweite wird ebenfalls übernommen
                                Dbpop(m, n, i) = Db(m, n, PES_iAkt.iAktPop)
                                'Die eigentlichen Parameterwerte werden übernommen
                                Xbpop(m, n, i) = Xb(m, n, PES_iAkt.iAktPop)
                            Next n
                        Next m
                    End If

                Case EVO_POP_PENALTY.Spannweite
                    If h2 > Qbpop(j, 1) Then
                        Qbpop(j, 1) = h2
                        For m = 0 To NPara - 1
                            For n = 0 To PES_Settings.NEltern - 1
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

        Dim m, i, j, v As Integer
        Dim h As Double

        If (Not PES_Settings.is_MO_Pareto) Then
            'SO - Standard ES nach Rechenberg
            '--------------------------------
            'Der schlechteste der besten Qualitätswerte wird bestimmt ; Position -> j
            '(höchster Wert der Penaltyfunktion)
            j = 0
            h = Qb(0, PES_iAkt.iAktPop, 0)

            For m = 1 To PES_Settings.NEltern - 1
                If Qb(m, PES_iAkt.iAktPop, 0) > h Then
                    h = Qb(m, PES_iAkt.iAktPop, 0)
                    j = m
                End If
            Next m

            'Falls die Qualität des aktuellen Nachkommen besser ist (Penaltyfunktion geringer)
            'als die schlechteste im Bestwertspeicher, wird dieser ersetzt
            If QN(0) < Qb(j, PES_iAkt.iAktPop, 0) Then
                Qb(j, PES_iAkt.iAktPop, 0) = QN(0)
                For v = 0 To NPara - 1
                    'Die Schrittweite wird ebenfalls übernommen
                    Db(v, j, PES_iAkt.iAktPop) = AktPara.Dn(v)
                    'Die eigentlichen Parameterwerte werden übernommen
                    Xb(v, j, PES_iAkt.iAktPop) = AktPara.Xn(v)
                Next v
            End If

        Else
            'Multi-Objective Pareto
            '----------------------
            With NDSorting(PES_iAkt.iAktNachf)
                For i = 0 To NPenalty - 1
                    .penalty(i) = QN(i)
                Next i
                .feasible = True
                For i = 0 To NConstrains - 1
                    .constrain(i) = RN(i)
                    If .constrain(i) < 0 Then .feasible = False
                Next i
                .dominated = False
                .Front = 0
                For v = 0 To NPara - 1
                    .d(v) = AktPara.Dn(v)
                    .X(v) = AktPara.Xn(v)
                Next v
                .distance = 0
            End With
        End If

    End Sub

    'ES_BESTWERTSPEICHER
    'Führt einen Reset des Bestwertspeicher durch, falls eine Komma-Strategie gewählt ist
    '************************************************************************************
    Public Sub EsResetBWSpeicher()
        Dim n, i As Integer

        If (PES_Settings.iEvoTyp = EVO_STRATEGIE.Komma) Then
            For n = 0 To PES_Settings.NEltern - 1
                For i = 0 To NPenalty - 1
                    Qb(n, PES_iAkt.iAktPop, i) = 1.0E+300
                Next i
            Next n
        End If

    End Sub

    'ES_POP_BESTWERTSPEICHER
    'Führt einen Reset des Bestwertspeicher auf Populationsebene durch, falls eine Komma-Strategie gewählt ist
    '*********************************************************************************************************
    Public Sub EsResetPopBWSpeicher()
        Dim n, i As Integer

        If (PES_Settings.iPopEvoTyp = EVO_STRATEGIE.Komma) Then
            For n = 0 To PES_Settings.NPopul - 1
                For i = 0 To NPenalty - 1
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

        Select Case PES_Settings.iPopPenalty
            Case EVO_POP_PENALTY.Crowding
                Z = 0
            Case EVO_POP_PENALTY.Spannweite
                Z = 1
        End Select

        ReDim Realisierungsspeicher(PES_Settings.NPopul - 1, 1)

        'Die NPopEltern besten Individium-Sätze werden ermittelt
        For m = 0 To PES_Settings.NPopul - 1
            Realisierungsspeicher(m, 0) = Qbpop(m, Z)
            Realisierungsspeicher(m, 1) = m
        Next m

        If (Not PES_Settings.is_MO_Pareto) Then
            'Standard ES nach Rechenberg
            '---------------------------
            For m = 0 To PES_Settings.NPopul - 1
                For n = m To PES_Settings.NPopul - 1
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
            Select Case PES_Settings.iPopPenalty

                Case EVO_POP_PENALTY.Crowding
                    For m = 0 To PES_Settings.NPopul - 1
                        For n = m To PES_Settings.NPopul - 1
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
                    For m = 0 To PES_Settings.NPopul - 1
                        For n = m To PES_Settings.NPopul - 1
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
        For m = 0 To PES_Settings.NPopEltern - 1
            For n = 0 To PES_Settings.NEltern - 1
                For v = 0 To NPara - 1
                    Dp(v, n, m) = Dbpop(v, n, Int(Realisierungsspeicher(m, 1)))
                    Xp(v, n, m) = Xbpop(v, n, Int(Realisierungsspeicher(m, 1)))
                Next v
            Next n
        Next m

    End Sub

    'ES_ELTERN - Die neuen Eltern werden generiert
    '*********************************************
    Public Sub EsEltern()

        Dim l, m, v, i As Integer
        Dim NFrontMember_aktuell, NFrontMember_gesamt As Integer
        Dim rang As Integer
        Dim Temp() As Struct_NDSorting
        Dim NDSResult() As Struct_NDSorting
        Dim aktuelle_Front As Integer

        If (Not PES_Settings.is_MO_Pareto) Then
            'Standard ES nach Rechenberg
            'xxxxxxxxxxxxxxxxxxxxxxxxxxx
            'Die Eltern werden gleich der besten Kinder gesetzt (Schrittweite und Parameterwert)
            '---------------------------------------------------------------------
            For m = 0 To PES_Settings.NEltern - 1
                For v = 0 To NPara - 1
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
            For m = PES_Settings.NNachf To PES_Settings.NNachf + PES_Settings.NEltern - 1
                With NDSorting(m)
                    For l = 0 To NPenalty - 1
                        .penalty(l) = Qb(m - PES_Settings.NNachf, PES_iAkt.iAktPop, l)
                    Next l
                    If NConstrains > 0 Then
                        .feasible = True
                        For l = 0 To NConstrains - 1
                            .constrain(l) = Rb(m - PES_Settings.NNachf, PES_iAkt.iAktPop, l)
                            If .constrain(l) < 0 Then .feasible = False
                        Next l
                    End If
                    .dominated = False
                    .Front = 0
                    For v = 0 To NPara - 1
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
            ReDim Temp(PES_Settings.NNachf + PES_Settings.NEltern - 1)

            For i = 0 To (PES_Settings.NNachf + PES_Settings.NEltern - 1)
                ReDim Temp(i).d(NPara - 1)
                ReDim Temp(i).X(NPara - 1)
            Next i
            'Initialisierung von NDSResult (NDSorting)
            ReDim NDSResult(PES_Settings.NNachf + PES_Settings.NEltern - 1)

            For i = 0 To (PES_Settings.NNachf + PES_Settings.NEltern - 1)
                ReDim NDSResult(i).d(NPara - 1)
                ReDim NDSResult(i).X(NPara - 1)
            Next i

            'NDSorting wird in Temp kopiert
            Call PES.Struct_NDSorting.Copy(NDSorting, Temp)

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
            aktuelle_Front = 0

            Do
                NFrontMember_aktuell = Count_Front_Members(aktuelle_Front, NDSResult)

                'Es sind mehr Elterplätze für die nächste Generation verfügaber
                '-> schiss wird einfach rüberkopiert
                If NFrontMember_aktuell <= PES_Settings.NEltern - NFrontMember_gesamt Then
                    For i = NFrontMember_gesamt To NFrontMember_aktuell + NFrontMember_gesamt - 1

                        'NDSResult wird in den Bestwertspeicher kopiert
                        Call Struct_NDSorting_to_Bestwert(i, NDSResult)

                    Next i
                    NFrontMember_gesamt = NFrontMember_gesamt + NFrontMember_aktuell

                Else
                    'Es sind weniger Elterplätze für die nächste Generation verfügber
                    'als Mitglieder der aktuellen Front. Nur für diesen Rest wird crowding distance
                    'gemacht um zu bestimmen wer noch mitspielen darf und wer noch a biserl was druff hat
                    Call NDS_Crowding_Distance_Sort(NDSResult, NFrontMember_gesamt, NFrontMember_gesamt + NFrontMember_aktuell - 1)

                    For i = NFrontMember_gesamt To PES_Settings.NEltern - 1

                        'NDSResult wird in den Bestwertspeicher kopiert
                        Call Struct_NDSorting_to_Bestwert(i, NDSResult)

                    Next i

                    NFrontMember_gesamt = PES_Settings.NEltern

                End If

                aktuelle_Front += 1

            Loop While Not (NFrontMember_gesamt = PES_Settings.NEltern)

            '4: Sekundäre Population wird bestimmt und gespeichert
            '-----------------------------------------------------
            SekundärQb_Allocation(NFrontMember_aktuell, NDSResult)


            '5: Neue Eltern werden gleich dem Bestwertspeicher gesetzt
            '---------------------------------------------------------
            For m = 0 To PES_Settings.NEltern - 1
                For v = 0 To NPara - 1
                    De(v, m, PES_iAkt.iAktPop) = Db(v, m, PES_iAkt.iAktPop)
                    Xe(v, m, PES_iAkt.iAktPop) = Xb(v, m, PES_iAkt.iAktPop)
                Next v
            Next m

            '6: Sortierung der Lösungen ist nur für Neighbourhood-Rekombination notwendig
            '----------------------------------------------------------------------------
            If (PES_Settings.iOptEltern = EVO_ELTERN.Neighbourhood) Then
                Call Neighbourhood_AbstandsArray()
                Call Neighbourhood_Crowding_Distance()
            End If

        End If

    End Sub

    '4: Sekundäre Population wird bestimmt und gespeichert ggf gespeichert
    '---------------------------------------------------------------------
    Private Sub SekundärQb_Allocation(ByVal NFrontMember_aktuell As Integer, ByVal NDSResult As Struct_NDSorting())

        Dim i As Integer
        Dim Member_Sekundärefront As Integer

        NFrontMember_aktuell = Count_Front_Members(1, NDSResult)

        'BUG 135
        'Weil wenn die Länge von SekundärQb 0 ist, gibt UBound -1 zurück!
        'Problem: Upperbound liefert nicht die Anzahl! Was wird benötigt?
        Member_Sekundärefront = Math.Max(SekundärQb.GetUpperBound(0), 0)

        'SekPop wird um die aktuelle Front erweitert
        ReDim Preserve SekundärQb(Member_Sekundärefront + NFrontMember_aktuell - 1)

        'Neue Member der SekPop bestimmen
        For i = Member_Sekundärefront To Member_Sekundärefront + NFrontMember_aktuell - 1
            SekundärQb(i) = NDSResult(i - Member_Sekundärefront)
        Next i

        Call Non_Dominated_Sorting(SekundärQb, 1)

        NFrontMember_aktuell = Non_Dominated_Count_and_Sort_Sekundäre_Population(SekundärQb)
        ReDim Preserve SekundärQb(NFrontMember_aktuell - 1)

        'Dubletten werden gelöscht
        Call SekundärQb_Dubletten()
        NFrontMember_aktuell = Non_Dominated_Count_and_Sort_Sekundäre_Population(SekundärQb)
        ReDim Preserve SekundärQb(NFrontMember_aktuell - 1)

        'Crowding Distance
        If (SekundärQb.GetUpperBound(0) > PES_Settings.NMemberSecondPop - 1) Then
            Call NDS_Crowding_Distance_Sort(SekundärQb, 0, SekundärQb.GetUpperBound(0))
            ReDim Preserve SekundärQb(PES_Settings.NMemberSecondPop - 1)
        End If

        'Prüfen, ob die Population jetzt mit Mitgliedern aus der Sekundären Population aufgefüllt werden soll
        '----------------------------------------------------------------------------------------------------
        If (PES_iAkt.iAktGen Mod PES_Settings.NInteract) = 0 And PES_Settings.isInteract Then
            NFrontMember_aktuell = Count_Front_Members(1, SekundärQb)
            If NFrontMember_aktuell > PES_Settings.NEltern Then
                'Crowding Distance
                Call NDS_Crowding_Distance_Sort(SekundärQb, 0, SekundärQb.GetUpperBound(0))
                For i = 0 To PES_Settings.NEltern - 1

                    'NDSResult wird in den Bestwertspeicher kopiert
                    Call Struct_NDSorting_to_Bestwert(i, SekundärQb)

                Next i
            End If
        End If
    End Sub

    'Kopiert ein Struct_NDSorting in den Bestwertspeicher
    '----------------------------------------------------
    Private Sub Struct_NDSorting_to_Bestwert(ByVal i As Integer, ByVal NDSorting_Struct As Struct_NDSorting())
        Dim j, v As Integer

        For j = 0 To NPenalty - 1
            Qb(i, PES_iAkt.iAktPop, j) = NDSorting_Struct(i).penalty(j)
        Next j

        If NConstrains > 0 Then
            For j = 0 To NConstrains - 1
                Rb(i, PES_iAkt.iAktPop, j) = NDSorting_Struct(i).constrain(j)
            Next j
        End If

        For v = 0 To NPara - 1
            Db(v, i, PES_iAkt.iAktPop) = NDSorting_Struct(i).d(v)
            Xb(v, i, PES_iAkt.iAktPop) = NDSorting_Struct(i).X(v)
        Next v

    End Sub

    'SekundärQb_Dubletten
    '********************
    Private Sub SekundärQb_Dubletten()

        Dim i, j, k As Integer
        Dim Logical As Boolean

        For i = 0 To SekundärQb.GetUpperBound(0) - 2
            For j = i + 1 To SekundärQb.GetUpperBound(0)
                Logical = True
                For k = 0 To NPenalty - 1
                    Logical = Logical And (SekundärQb(i).penalty(k) = SekundärQb(j).penalty(k))
                Next k
                If (Logical) Then SekundärQb(i).dominated = True
            Next j
        Next i
    End Sub


    'ES_GET_SEKUNDÄRE_POPULATIONEN - Sekundäre Population speichert immer die angegebene
    'Anzahl von Bestwerten und kann den Bestwertspeicher alle x Generationen überschreiben
    '*************************************************************************************
    Public Function SekundärQb_Get() As Double(,)

        Dim j, i As Integer
        Dim SekPopulation(,) As Double

        ReDim SekPopulation(SekundärQb.GetUpperBound(0), NPenalty - 1)

        For i = 0 To SekundärQb.GetUpperBound(0)
            For j = 0 To NPenalty - 1
                SekPopulation(i, j) = SekundärQb(i).penalty(j)
            Next j
        Next i

        Return SekPopulation

    End Function

    'NON_DOMINATED_SORTING - Entscheidet welche Werte dominiert werden und welche nicht
    '**********************************************************************************
    Private Sub Non_Dominated_Sorting(ByRef NDSorting() As Struct_NDSorting, ByVal rang As Integer)

        Dim j, i, k As Integer
        Dim isDominated As Boolean
        Dim Summe_Constrain(1) As Double

        If (NConstrains > 0) Then
            'Mit Constraints
            '===============
            For i = 0 To NDSorting.GetUpperBound(0)
                For j = 0 To NDSorting.GetUpperBound(0)

                    'Überpüfen, ob NDSorting(j) von NDSorting(i) dominiert wird
                    '----------------------------------------------------------
                    If (NDSorting(i).feasible And Not NDSorting(j).feasible) Then

                        'i gültig und j ungültig
                        '-----------------------
                        NDSorting(j).dominated = True

                    ElseIf ((Not NDSorting(i).feasible) And (Not NDSorting(j).feasible)) Then

                        'beide ungültig
                        '--------------
                        Summe_Constrain(0) = 0
                        Summe_Constrain(1) = 0

                        For k = 0 To NConstrains - 1
                            If (NDSorting(i).constrain(k) < 0) Then
                                Summe_Constrain(0) += NDSorting(i).constrain(k)
                            End If
                            If (NDSorting(j).constrain(k) < 0) Then
                                Summe_Constrain(1) += NDSorting(j).constrain(k)
                            End If
                        Next k

                        If (Summe_Constrain(0) > Summe_Constrain(1)) Then
                            NDSorting(j).dominated = True
                        End If

                    ElseIf (NDSorting(i).feasible And NDSorting(j).feasible) Then

                        'beide gültig
                        '------------
                        isDominated = False

                        For k = 0 To NPenalty - 1
                            isDominated = isDominated Or (NDSorting(i).penalty(k) < NDSorting(j).penalty(k))
                        Next k

                        For k = 0 To NPenalty - 1
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
            For i = 0 To NDSorting.GetUpperBound(0)
                For j = 0 To NDSorting.GetUpperBound(0)

                    isDominated = False

                    For k = 0 To NPenalty - 1
                        isDominated = isDominated Or (NDSorting(i).penalty(k) < NDSorting(j).penalty(k))
                    Next k

                    For k = 0 To NPenalty - 1
                        isDominated = isDominated And (NDSorting(i).penalty(k) <= NDSorting(j).penalty(k))
                    Next k

                    If (isDominated) Then
                        NDSorting(j).dominated = True
                    End If
                Next j
            Next i
        End If

        For i = 0 To NDSorting.GetUpperBound(0)
            'Hier wird die Nummer der Front geschrieben
            If NDSorting(i).dominated = False Then NDSorting(i).Front = rang
        Next i

    End Sub

    'NON_DOMINATED_COUNT_AND_SORT - Sortiert die nicht dominanten Lösungen nach oben,
    'die dominanten nach unten, gibt die Zahl der dominanten Lösungen zurück (Front)
    '*******************************************************************************
    Private Function Non_Dominated_Count_and_Sort(ByRef NDSorting() As Struct_NDSorting) As Integer

        Dim i As Integer
        Dim Temp() As Struct_NDSorting
        Dim counter As Integer
        Dim NFrontMember As Integer

        ReDim Temp(NDSorting.GetUpperBound(0))
        Call PES.Struct_NDSorting.Dimit(NPenalty, NConstrains, NPara, Temp)

        NFrontMember = 0
        counter = 0

        'Die nicht dominanten Lösungen werden nach oben kopiert
        For i = 0 To NDSorting.GetUpperBound(0)
            If (NDSorting(i).dominated = True) Then
                counter += 1
                Call PES.Struct_NDSorting.Copy(NDSorting(i), Temp(counter - 1))
            End If
        Next i

        'Zahl der dominanten Lösungen wird errechnet
        NFrontMember = NDSorting.GetUpperBound(0) + 1 - counter

        'Die dominanten Lösungen werden nach unten kopiert
        For i = 0 To NDSorting.GetUpperBound(0)
            If (NDSorting(i).dominated = False) Then
                counter += 1
                Call PES.Struct_NDSorting.Copy(NDSorting(i), Temp(counter - 1))
            End If
        Next i

        Call PES.Struct_NDSorting.Copy(Temp, NDSorting)

        Return NFrontMember

    End Function

    'NON_DOMINATED_COUNT_AND_SORT_SEKUNDÄRE_POPULATION
    'Sortiert die nicht dominanten Lösungen nach oben, die dominanten nach unten
    'Gibt die Zahl der dominanten Lösungen zurück (Front) hier für die Sekundäre Population
    '**************************************************************************************
    Private Function Non_Dominated_Count_and_Sort_Sekundäre_Population(ByRef NDSorting() As Struct_NDSorting) As Integer

        Dim i As Integer
        Dim Temp() As Struct_NDSorting
        Dim counter As Integer
        Dim NFrontMember As Integer

        ReDim Temp(NDSorting.GetUpperBound(0))
        Call PES.Struct_NDSorting.Dimit(NPenalty, NConstrains, NPara, Temp)

        NFrontMember = 0
        counter = 0

        For i = 0 To NDSorting.GetUpperBound(0)
            If (NDSorting(i).dominated = False) Then
                counter += 1
                Call PES.Struct_NDSorting.Copy(NDSorting(i), Temp(counter - 1))
            End If
        Next i

        NFrontMember = counter

        For i = 0 To NDSorting.GetUpperBound(0)
            If (NDSorting(i).dominated = True) Then
                counter += 1
                Call PES.Struct_NDSorting.Copy(NDSorting(i), Temp(counter - 1))
            End If
        Next i

        Call PES.Struct_NDSorting.Copy(Temp, NDSorting)

        Return NFrontMember

    End Function

    'NON_DOMINATED_RESULT - Hier wird pro durchlauf die nicht dominierte Front in NDSResult
    'geschaufelt und die bereits klassifizierten Lösungen aus Temp Array gelöscht
    '**************************************************************************************
    Private Sub Non_Dominated_Result(ByRef Temp() As Struct_NDSorting, ByRef NDSResult() As Struct_NDSorting, ByVal NFrontMember_aktuell As Integer, ByVal NFrontMember_gesamt As Integer)

        Dim i, Position As Integer

        Position = NFrontMember_gesamt - NFrontMember_aktuell

        'In NDSResult werden die nicht dominierten Lösungen eingefügt
        For i = Temp.GetLength(0) - NFrontMember_aktuell To Temp.GetUpperBound(0)
            'NDSResult alle bisher gefundene Fronten
            Call PES.Struct_NDSorting.Copy(Temp(i), NDSResult(Position))
            Position += 1
        Next i

        'Die bereits klassifizierten Member werden aus dem Temp Array gelöscht
        If (PES_Settings.NNachf + PES_Settings.NEltern - NFrontMember_gesamt > 0) Then
            ReDim Preserve Temp(PES_Settings.NNachf + PES_Settings.NEltern - NFrontMember_gesamt - 1)
            'Der Flag wird zur klassifizierung in der nächsten Runde zurückgesetzt
            For i = 0 To Temp.GetUpperBound(0)
                Temp(i).dominated = False
            Next i
        End If

    End Sub

    'COUNT_FRONT_MEMBERS
    '*******************
    Private Function Count_Front_Members(ByVal aktuell_Front As Integer, ByVal NDSResult() As Struct_NDSorting) As Integer

        Dim i As Integer
        Count_Front_Members = 0

        For i = 0 To NDSResult.GetUpperBound(0)
            If (NDSResult(i).Front = aktuell_Front) Then
                Count_Front_Members += 1
            End If
        Next i

    End Function

    'NDS_Crowding_Distance_Sort
    '**************************
    Private Sub NDS_Crowding_Distance_Sort(ByRef NDSorting() As Struct_NDSorting, ByVal StartIndex As Integer, ByVal EndIndex As Integer)

        Dim i As Integer
        Dim j As Integer
        Dim k As Integer

        Call PES.Struct_NDSorting.Dimit(NPenalty, NConstrains, NPara, swap)

        Dim fmin, fmax As Double

        For k = 0 To NPenalty - 1
            For i = StartIndex To EndIndex
                For j = StartIndex To EndIndex
                    If (NDSorting(i).penalty(k) < NDSorting(j).penalty(k)) Then
                        Call PES.Struct_NDSorting.Copy(NDSorting(i), swap)
                        Call PES.Struct_NDSorting.Copy(NDSorting(j), NDSorting(i))
                        Call PES.Struct_NDSorting.Copy(swap, NDSorting(j))
                    End If
                Next j
            Next i

            fmin = NDSorting(StartIndex).penalty(k)
            fmax = NDSorting(EndIndex).penalty(k)

            NDSorting(StartIndex).distance = 1.0E+300
            NDSorting(EndIndex).distance = 1.0E+300

            For i = StartIndex + 1 To EndIndex - 1
                NDSorting(i).distance = NDSorting(i).distance + (NDSorting(i + 1).penalty(k) - NDSorting(i - 1).penalty(k)) / (fmax - fmin)
            Next i
        Next k

        For i = StartIndex To EndIndex
            For j = StartIndex To EndIndex
                If (NDSorting(i).distance > NDSorting(j).distance) Then
                    Call PES.Struct_NDSorting.Copy(NDSorting(i), swap)
                    Call PES.Struct_NDSorting.Copy(NDSorting(j), NDSorting(i))
                    Call PES.Struct_NDSorting.Copy(swap, NDSorting(j))
                End If
            Next j
        Next i

    End Sub

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

        ReDim TempDistance(NPenalty - 1)
        ReDim PenaltyDistance(PES_Settings.NEltern - 1, PES_Settings.NEltern - 1)
        ReDim d(PES_Settings.NEltern - 1)

        'Bestimmen der normierten Raumabstände zwischen allen Elternindividuen
        For i = 0 To PES_Settings.NEltern - 1
            PenaltyDistance(i, i) = 0
            For j = i + 1 To PES_Settings.NEltern - 1
                PenaltyDistance(i, j) = 0
                For k = 0 To NPenalty - 1
                    TempDistance(k) = Qb(i, PES_iAkt.iAktPop, k) - Qb(j, PES_iAkt.iAktPop, k)
                    TempDistance(k) = TempDistance(k) * TempDistance(k)
                    PenaltyDistance(i, j) += TempDistance(k)
                Next k
                PenaltyDistance(i, j) = System.Math.Sqrt(PenaltyDistance(i, j))
                PenaltyDistance(j, i) = PenaltyDistance(i, j)
            Next j
        Next i

        d_mean = 0

        For i = 0 To PES_Settings.NEltern - 2
            d(i) = 1.0E+300
            For j = 0 To i - 1
                If (PenaltyDistance(i, j) < d(i)) Then d(i) = PenaltyDistance(i, j)
            Next j
            For j = i + 1 To PES_Settings.NEltern - 1
                If (PenaltyDistance(i, j) < d(i)) Then d(i) = PenaltyDistance(i, j)
            Next j
            d_mean += d(i)
        Next i

        d_mean = d_mean / PES_Settings.NEltern
        NDS_Crowding_Distance_Count = 0

        For i = 0 To PES_Settings.NEltern - 2
            NDS_Crowding_Distance_Count += (d_mean - d(i)) * (d_mean - d(i))
        Next i

        NDS_Crowding_Distance_Count = NDS_Crowding_Distance_Count / PES_Settings.NEltern
        NDS_Crowding_Distance_Count = System.Math.Sqrt(NDS_Crowding_Distance_Count)

        Spannweite = 0
        For i = 0 To PES_Settings.NEltern - 1
            'TODO: sollte hier nicht j = i + 1 stehen?
            For j = i To PES_Settings.NEltern - 1
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
        ReDim MinMax(NPenalty - 1)
        For k = 0 To NPenalty - 1
            MinMax(k) = 0
            Min = Qb(0, PES_iAkt.iAktPop, k)
            Max = Qb(0, PES_iAkt.iAktPop, k)
            For j = 0 To PES_Settings.NEltern - 1
                If (Min > Qb(j, PES_iAkt.iAktPop, k)) Then Min = Qb(j, PES_iAkt.iAktPop, k)
                If (Max < Qb(j, PES_iAkt.iAktPop, k)) Then Max = Qb(j, PES_iAkt.iAktPop, k)
            Next j
            MinMax(k) = Max - Min
        Next k

        'Bestimmen der normierten Raumabstände zwischen allen Elternindividuen
        ReDim TempDistance(NPenalty)

        For i = 0 To PES_Settings.NEltern - 1
            PenaltyDistance(i, i) = 0
            For j = i + 1 To PES_Settings.NEltern - 1
                PenaltyDistance(i, j) = 0
                For k = 0 To NPenalty - 1
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

    'Neighbourhood_Eltern
    'Bestimme die NAnzahlEltern mit geringsten Raumabständen für Neighbourhood-Rekombination
    '***************************************************************************************
    Private Sub Neighbourhood_Eltern(ByVal IndexElter As Integer, ByRef IndexEltern() As Integer)

        Dim i As Integer
        Dim j As Integer
        Dim Nachbarn() As Struct_Neighbourhood
        Dim swap As Struct_Neighbourhood

        'BUG 135 ganze Funktion
        ReDim Nachbarn(PES_Settings.NEltern - 2)

        For i = 0 To IndexElter - 2
            Nachbarn(i).distance = PenaltyDistance(IndexElter, i)
            Nachbarn(i).Index = i
        Next i
        For i = IndexElter To PES_Settings.NEltern - 1
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

        For i = 0 To PES_Settings.NRekombXY - 1
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

        ReDim QbTemp(PES_Settings.NEltern - 1, PES_Settings.NPopul - 1, NPenalty - 1)

        Array.Copy(Qb, QbTemp, Qb.GetLength(0))
        For i = 0 To PES_Settings.NEltern - 1
            Distanceb(i) = 0
        Next i

        For k = 0 To NPenalty - 1
            For i = 0 To PES_Settings.NEltern - 1
                For j = 0 To PES_Settings.NEltern - 1
                    If (QbTemp(i, PES_iAkt.iAktPop, k) < QbTemp(j, PES_iAkt.iAktPop, k)) Then
                        swap = QbTemp(i, PES_iAkt.iAktPop, k)
                        QbTemp(i, PES_iAkt.iAktPop, k) = QbTemp(j, PES_iAkt.iAktPop, k)
                        QbTemp(j, PES_iAkt.iAktPop, k) = swap
                    End If
                Next j
            Next i

            fmin = QbTemp(0, PES_iAkt.iAktPop, k)
            fmax = QbTemp(PES_Settings.NEltern - 1, PES_iAkt.iAktPop, k)

            Distanceb(0) = 1.0E+300
            Distanceb(PES_Settings.NEltern - 1) = 1.0E+300

            For i = 1 To PES_Settings.NEltern - 2
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
