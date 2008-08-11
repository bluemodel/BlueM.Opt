Option Strict Off

Imports IHWB.EVO.Common

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
    '**** Autoren: Dirk Muschalla, Christoph Hübner                             ****
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

    Private Settings As EVO_Settings

    'Structure zum Speichern der Werte die aus den OptDateien generiert werden
    Private Structure Struct_AktPara
        Dim Dn() As Double                  'Schrittweitenvektor (Dimension varanz)
        Dim Xn() As Double                  'aktuelle Variablenwerte (Dimension varanz)
        Dim Bez() As Beziehung              'Beziehungen
    End Structure

    Private AktPara() As OptParameter

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
    Private Div(,) As Double                'Diversitätsmass
    Private Front(,) As Integer             'Front

    '---Bestwerte----------
    Public Structure Bestwerte
        Dim Xb(,,) As Double                'Bestwertspeicher Variablenwerte für eine Generation
        Dim Db(,,) As Double                'Bestwertspeicher Schrittweite für eine Generation
        Dim Qb(,,) As Double                'Bestwertspeicher für eine Generation
        Dim Rb(,,) As Double                'Restriktionen für eine Generation
        Dim Div(,) As Double                'Diversität der Individuen für eine Generation
        Dim Front(,) As Double              'Front der Individuen
    End Structure

    Public Best As Bestwerte

    '---Stuff--------------
    Private Distanceb() As Double           'Array mit Crowding-Distance (Neighbourhood-Rekomb.)
    Private PenaltyDistance(,) As Double    'Array für normierte Raumabstände (Neighbourhood-Rekomb.)
    '---------------------
    Public SekundärQb(-1) As Individuum_PES 'Sekundäre Population wird mit -1 initialisiert dann länge 0

    Const galpha As Double = 1.3            'Faktor alpha = 1.3 auf Generationsebene nach Rechenberg
    Const palpha As Double = 1.1            'Faktor alpha = 1.1 auf Populationsebene nach Rechenberg

    Dim NDSorting() As Individuum_PES

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
    Public Sub PesInitialise(ByRef settings As EVO_Settings, ByVal AnzPara As Integer, ByVal AnzPenalty As Integer, ByVal AnzConstr As Integer, ByRef Parameter() As OptParameter, ByVal Method As String)

        'Schritt 1: PES - SETTINGS
        'Optionen der Evolutionsstrategie werden übergeben
        Call PES_Form_Settings(settings, Method)

        'Schritt 2: PES - PREPARE
        'Interne Variablen werden initialisiert, Zufallsgenerator wird initialisiert
        Call PES_Prepare(AnzPara, AnzPenalty, AnzConstr)

        'Schritt 3: PES - STARTVALUES
        'Startwerte werden zugewiesen
        Call PES_Startvalues(Parameter)

    End Sub

    'Schritt 1: SETTINGS
    'Function ES_SETTINGS übergibt Optionen für Evolutionsstrategie und Prüft die eingestellten Optionen
    '***************************************************************************************************
    Private Sub PES_Form_Settings(ByRef settings As EVO_Settings, ByVal method As String)

        'Überprüfung der Übergebenen Werte
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        If (Not System.Enum.IsDefined(GetType(EVO_STRATEGIE), settings.PES.OptStrategie)) Then
            Throw New Exception("Ungültige Einstellung für 'Strategie'!")
        End If
        If (Not System.Enum.IsDefined(GetType(EVO_STARTPARAMETER), settings.PES.OptStartparameter)) Then
            Throw New Exception("Ungültige Einstellung für 'Startparameter'!")
        End If
        'Schrittweite
        If (Not System.Enum.IsDefined(GetType(EVO_DnMutation), settings.PES.Schrittweite.OptDnMutation)) Then
            Throw New Exception("Ungültige Einstellung für 'Mutation'!")
        End If
        If (settings.PES.Schrittweite.DnStart < 0) Then
            Throw New Exception("Die Startschrittweite darf nicht kleiner 0 sein!")
        End If
        'Generationen
        If (settings.PES.n_Gen < 1) Then
            Throw New Exception("Die Anzahl der Generationen ist kleiner 1!")
        End If
        If (settings.PES.n_Eltern < 1) Then
            Throw New Exception("Die Anzahl der Eltern ist kleiner 1!")
        End If
        If (settings.PES.n_Nachf <= settings.PES.n_Eltern And method <> "HYBRID") Then
            Throw New Exception("Die Anzahl der Eltern muss kleiner als die Anzahl der Nachfahren!" & Chr(13) & Chr(10) & "'Rechenberg 73' schlägt ein Verhältnis von 1:3 bis 1:5 vor.")
        End If
        If (settings.PES.n_Nachf < 1) Then
            Throw New Exception("Die Anzahl der Nachfahren ist kleiner 1!")
        End If
        'Eltern
        If (Not System.Enum.IsDefined(GetType(EVO_ELTERN), settings.PES.OptEltern)) Then
            Throw New Exception("Ungültige Einstellung für die Ermittlung der Eltern!")
        End If
        If (settings.PES.OptModus = EVO_MODUS.Single_Objective And settings.PES.OptEltern = EVO_ELTERN.Neighbourhood) Then
            Throw New Exception("Die Option 'Neighbourhood' für die Ermittlung der Eltern ist bei Single-Objective nicht zulässig!")
        End If
        If (settings.PES.n_RekombXY < 1) Then
            Throw New Exception("Der Wert für die X/Y-Schema Rekombination ist kleiner 1!")
        End If
        'Populationen
        If (settings.PES.Pop.n_Runden < 1) Then
            Throw New Exception("Die Anzahl der Runden ist kleiner 1")
        End If
        If (settings.PES.Pop.n_Popul < 1) Then
            Throw New Exception("Die Anzahl der Populationen ist kleiner 1")
        End If
        If (settings.PES.Pop.n_PopEltern < 1) Then
            Throw New Exception("Die Anzahl der Populationseltern ist kleiner 1")
        End If
        If (settings.PES.Pop.n_Popul < settings.PES.Pop.n_PopEltern) Then
            Throw New Exception("Die Anzahl der Populationseltern darf nicht größer als die Anzahl der Populationen!")
        End If
        If (Not System.Enum.IsDefined(GetType(EVO_POP_ELTERN), settings.PES.Pop.OptPopEltern)) Then
            Throw New Exception("Ungültige Einstellung für die Ermittlung der Populationseltern!")
        End If
        If (Not System.Enum.IsDefined(GetType(EVO_STRATEGIE), settings.PES.Pop.OptPopStrategie)) Then
            Throw New Exception("Ungültige Einstellung für 'Selektion' auf Populationsebene!")
        End If

        'Übergabe der Optionen
        'xxxxxxxxxxxxxxxxxxxxx

        Me.Settings = settings

    End Sub

    'Schritt 2: PREPARE
    'Initialisiert benötigte dynamische Arrays und legt Anzahl der Zielfunktionen fest
    'Initialisiert alle internen Arrays und setzt den
    'Bestwertspeicher auf sehr großen Wert (Strategie minimiert in dieser Umsetzung)
    'TODO: ESPrepare Für Paretooptimierung noch nicht fertig!!!!
    '*******************************************************************************
    Private Sub PES_Prepare(ByVal AnzPara As Integer, ByVal AnzPenalty As Integer, ByVal AnzConstr As Integer)
        Dim m, n, l As Integer

        'Überprüfung der Eingabeparameter (es muss mindestens ein Parameter variiert und eine
        'Penaltyfunktion ausgewertet werden)

        If (AnzPara <= 0 Or AnzPenalty <= 0) Then
            Throw New Exception("Es muss mindestens ein Parameter variiert und eine Penaltyfunktion ausgewertet werden")
        End If

        'Anzahlen werden gesetzt
        Anz.Para = AnzPara                         'Anzahl der Parameter wird übergeben
        Anz.Penalty = AnzPenalty                   'Anzahl der Zielfunktionen wird übergeben
        Anz.Constr = AnzConstr                     'Anzahl der Randbedingungen wird übergeben

        'Dynamisches Array Initialisieren
        ReDim AktPara(Anz.Para - 1)                'Variablenvektor wird initialisiert

        'Parametervektoren initialisieren
        ReDim Dp(Anz.Para - 1, Settings.PES.n_Eltern - 1, Settings.PES.Pop.n_PopEltern - 1)
        ReDim Xp(Anz.Para - 1, Settings.PES.n_Eltern - 1, Settings.PES.Pop.n_PopEltern - 1)
        ReDim Qbpop(Settings.PES.Pop.n_Popul - 1, Anz.Penalty - 1)
        ReDim Dbpop(Anz.Para - 1, Settings.PES.n_Eltern - 1, Settings.PES.Pop.n_Popul - 1)
        ReDim Xbpop(Anz.Para - 1, Settings.PES.n_Eltern - 1, Settings.PES.Pop.n_Popul - 1)
        '---------------------
        ReDim De(Anz.Para - 1, Settings.PES.n_Eltern - 1, Settings.PES.Pop.n_Popul - 1)
        ReDim Xe(Anz.Para - 1, Settings.PES.n_Eltern - 1, Settings.PES.Pop.n_Popul - 1)
        ReDim Div(Settings.PES.n_Eltern - 1, Settings.PES.Pop.n_Popul - 1)
        ReDim Front(Settings.PES.n_Eltern - 1, Settings.PES.Pop.n_Popul - 1)
        '---------------------
        ReDim Best.Db(Anz.Para - 1, Settings.PES.n_Eltern - 1, Settings.PES.Pop.n_Popul - 1)
        ReDim Best.Xb(Anz.Para - 1, Settings.PES.n_Eltern - 1, Settings.PES.Pop.n_Popul - 1)
        ReDim Best.Qb(Settings.PES.n_Eltern - 1, Settings.PES.Pop.n_Popul - 1, Anz.Penalty - 1)
        ReDim Best.Rb(Settings.PES.n_Eltern - 1, Settings.PES.Pop.n_Popul - 1, Anz.Constr - 1)
        ReDim Best.Div(Settings.PES.n_Eltern - 1, Settings.PES.Pop.n_Popul - 1)
        ReDim Best.Front(Settings.PES.n_Eltern - 1, Settings.PES.Pop.n_Popul - 1)

        'NDSorting wird nur benötigt, falls eine Paretofront approximiert wird
        If (Settings.PES.OptModus = EVO_MODUS.Multi_Objective) Then
            ReDim NDSorting(Settings.PES.n_Eltern + Settings.PES.n_Nachf - 1)
            Call Individuum_PES.New_Indi_Array("NDSorting", NDSorting)
            If (Settings.PES.OptEltern = EVO_ELTERN.Neighbourhood) Then
                ReDim PenaltyDistance(Settings.PES.n_Eltern - 1, Settings.PES.n_Eltern - 1)
                ReDim Distanceb(Settings.PES.n_Eltern - 1)
            End If
        End If

        For n = 0 To Settings.PES.n_Eltern - 1
            For m = 0 To Settings.PES.Pop.n_Popul - 1
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
        If (Settings.PES.OptModus = EVO_MODUS.Multi_Objective) Then
            For n = 0 To Settings.PES.Pop.n_Popul - 1
                For m = 0 To Anz.Penalty - 1
                    Select Case Settings.PES.Pop.OptPopPenalty

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
            For n = 0 To Settings.PES.Pop.n_Popul - 1
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
    Public Sub PES_Startvalues(ByVal Parameter() As OptParameter)

        Dim i As Integer

        'Check
        If (Parameter.Length <> Anz.Para) Then
            Throw New Exception("Falsche Anzahl Parameter übergeben!")
        End If

        'Dynamisches Array wird mit Werten belegt
        For i = 0 To Anz.Para - 1
            If (Parameter(i).Xn < 0 Or Parameter(i).Xn > 1) Then
                Throw New Exception("Der Startparameter '" & Parameter(i).Bezeichnung & "' liegt nicht zwischen 0 und 1. Sie müssen hier skaliert vorliegen")
            End If
            AktPara(i) = Parameter(i).Clone()
            'Startschrittweite übernehmen
            AktPara(i).Dn = Settings.PES.Schrittweite.DnStart
        Next

        Dim n, v, m As Integer

        'Zufallsgenerator initialisieren
        Randomize()

        'Die Startparameter für die Eltern werden gesetzt
        Select Case Settings.PES.OptStartparameter

            Case EVO_STARTPARAMETER.Zufall 'Zufällige Startwerte
                For v = 0 To Anz.Para - 1
                    For n = 0 To Settings.PES.n_Eltern - 1
                        For m = 0 To Settings.PES.Pop.n_PopEltern - 1
                            'Startwert für die Elternschrittweite wird zugewiesen
                            Dp(v, n, m) = AktPara(0).Dn
                            'Startwert für die Eltern werden zugewiesen
                            '(Zufallszahl zwischen 0 und 1)
                            Xp(v, n, m) = Rnd()
                        Next m
                    Next n
                Next v

            Case EVO_STARTPARAMETER.Original 'Originalparameter
                For v = 0 To Anz.Para - 1
                    For n = 0 To Settings.PES.n_Eltern - 1
                        For m = 0 To Settings.PES.Pop.n_PopEltern - 1
                            'Startwert für die Elternschrittweite wird zugewiesen
                            Dp(v, n, m) = AktPara(0).Dn
                            'Startwert für die Eltern werden zugewiesen
                            '(alle gleich Anfangswerte)
                            Xp(v, n, m) = AktPara(v).Xn
                        Next m
                    Next n
                Next v
        End Select

    End Sub

    'Überladen: Falls Startwerte aus CES kommen!
    Public Sub EsStartvalues(ByVal is_Pop As Boolean, ByVal Parameter() As OptParameter, ByVal IndexElter As Integer)

        Dim v As Integer

        For v = 0 To Anz.Para - 1
            If is_Pop = True Then
                'Startwert für die Elternschrittweite wird zugewiesen
                Dp(v, IndexElter, 0) = Parameter(v).Dn
                'Startwert für die Eltern werden zugewiesen
                '(alle gleich Anfangswerte)
                Xp(v, IndexElter, 0) = Parameter(v).Xn
            Else
                'Startwert für die Elternschrittweite wird zugewiesen
                De(v, IndexElter, 0) = Parameter(v).Dn
                'Startwert für die Eltern werden zugewiesen
                '(alle gleich Anfangswerte)
                Xe(v, IndexElter, 0) = Parameter(v).Xn

            End If
        Next v

        'Die Startparameter werden aus den PES_Parents aus der CES gesetzt
    End Sub

    'ES_GET_PARAMETER - dient zur Rückgabe der mutierten Parameter
    '*************************************************************
    Public Function EsGetParameter() As OptParameter()

        Return AktPara

    End Function

    'ES_GET_BESTWERT - gibt den kompletten Bestwertspeicher aus
    '**********************************************************
    Public Function EsGetBestwert() As Double(,)

        Dim i, j As Integer
        Dim Bestwert(,) As Double

        ReDim Bestwert(Settings.PES.n_Eltern - 1, Anz.Penalty - 1)

        For i = 0 To Anz.Penalty - 1
            For j = 0 To Settings.PES.n_Eltern - 1
                Bestwert(j, i) = Best.Qb(j, PES_iAkt.iAktPop, i)
            Next j
        Next i

        Return Bestwert

    End Function

    'Function um PopReproduktion, PopMutation, Reproduktion und Mutio n direkt ablaufen zu lassen
    '********************************************************************************************
    Public Sub EsReproMut(ByVal is_Pop As Boolean)

        If is_Pop = True Then

            'POPULATIONS REPRODUKTIONSPROZESS
            '################################
            'Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern der Population
            Call EsPopReproduktion()

            'POPULATIONS MUTATIONSPROZESS
            '############################
            'Mutieren der Ausgangswerte der Population
            Call EsPopMutation()

        End If

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

        Select Case Settings.PES.Pop.OptPopEltern

            Case EVO_POP_ELTERN.Rekombination 'MultiRekombination über alle Eltern (x/x,y) oder (x/x+y)
                For n = 0 To Settings.PES.n_Eltern - 1
                    R = Int(Settings.PES.Pop.n_PopEltern * Rnd())
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
                    For n = 0 To Settings.PES.n_Eltern - 1
                        De(v, n, PES_iAkt.iAktPop) = 0
                        Xe(v, n, PES_iAkt.iAktPop) = 0
                        For m = 0 To Settings.PES.Pop.n_PopEltern - 1
                            'Mittelung der Schrittweite,
                            De(v, n, PES_iAkt.iAktPop) = De(v, n, PES_iAkt.iAktPop) + (Dp(v, n, m) / Settings.PES.Pop.n_PopEltern)
                            'Mittelung der Eltern,
                            Xe(v, n, PES_iAkt.iAktPop) = Xe(v, n, PES_iAkt.iAktPop) + (Xp(v, n, m) / Settings.PES.Pop.n_PopEltern)
                        Next m
                    Next n
                Next v

            Case EVO_POP_ELTERN.Selektion 'Zufallswahl über alle Eltern
                R = Int(Settings.PES.Pop.n_PopEltern * Rnd()) 'Zufallszahl entscheidet welcher
                'Elternteil vererbt wird
                For v = 0 To Anz.Para - 1
                    For n = 0 To Settings.PES.n_Eltern - 1
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
        Dim Elter As Integer
        Dim TournamentElter1 As Integer
        Dim TournamentElter2 As Integer

        Select Case Settings.PES.OptEltern

            Case EVO_ELTERN.Selektion 'Zufallswahl über alle Eltern

                R = Int(Settings.PES.n_Eltern * Rnd())    'Zufallszahl entscheidet
                'welcher Enternteil vererbt wird
                For v = 0 To Anz.Para - 1
                    'Selektion der Schrittweite
                    AktPara(v).Dn = De(v, R, PES_iAkt.iAktPop)
                    'Selektion des Elter
                    AktPara(v).Xn = Xe(v, R, PES_iAkt.iAktPop)
                Next v

            Case EVO_ELTERN.XX_Diskret 'Multi-Rekombination, diskret

                For v = 0 To Anz.Para - 1
                    R = Int(Settings.PES.n_Eltern * Rnd())
                    'Selektion der Schrittweite
                    AktPara(v).Dn = De(v, R, PES_iAkt.iAktPop)
                    'Selektion des Elter
                    AktPara(v).Xn = Xe(v, R, PES_iAkt.iAktPop)
                Next v

            Case EVO_ELTERN.XX_Mitteln 'Multi-Rekombination, gemittelt

                For v = 0 To Anz.Para - 1
                    AktPara(v).Dn = 0
                    AktPara(v).Xn = 0

                    For n = 0 To Settings.PES.n_Eltern - 1
                        'Mittelung der Schrittweite,
                        AktPara(v).Dn = AktPara(v).Dn + (De(v, n, PES_iAkt.iAktPop) / Settings.PES.n_Eltern)
                        'Mittelung der Eltern,
                        AktPara(v).Xn = AktPara(v).Xn + (Xe(v, n, PES_iAkt.iAktPop) / Settings.PES.n_Eltern)
                    Next
                Next v

            Case EVO_ELTERN.XX_Mitteln_Diskret
                For v = 0 To Anz.Para - 1
                    AktPara(v).Dn = 0
                    R = Int(Settings.PES.n_Eltern * Rnd())
                    'Mittelung der Schrittweite
                    For n = 0 To Settings.PES.n_Eltern - 1
                        AktPara(v).Dn = AktPara(v).Dn + (De(v, n, PES_iAkt.iAktPop) / Settings.PES.n_Eltern)
                    Next
                    'Selektion des Elter
                    AktPara(v).Xn = Xe(v, R, PES_iAkt.iAktPop)
                Next v


            Case EVO_ELTERN.XY_Diskret 'Multi-Rekombination nach X/Y-Schema, diskrete Vertauschung
                'Realisierungsspeicher und Elternspeicher initialisieren
                'Anzahl der benötigten Eltern (Y)
                ReDim Realisierungsspeicher(Settings.PES.n_RekombXY - 1)
                'Anzahl der Verfügbaren Eltern (n_Eltern)
                ReDim Elternspeicher(Settings.PES.n_Eltern - 1)
                'Setzen der Eltern Indizes
                For i = 0 To (Settings.PES.n_Eltern - 1)
                    Elternspeicher(i) = i
                Next
                'Auswahl der Y-Eltern
                For i = 0 To Settings.PES.n_RekombXY - 1
                    '1. Runde erlaubt Auswahl aus allen Eltern
                    '2. Runde hat nur noch n_Eltern - 1 zur Verfügung
                    'usw.
                    'Kein Elter darf doppelt gezogen werden
                    If (Settings.PES.is_DiversityTournament) Then

                        R = CInt(Int((Settings.PES.n_Eltern - i) * Rnd()))
                        TournamentElter1 = Elternspeicher(R)

                        Do
                            R = CInt(Int((Settings.PES.n_Eltern - i) * Rnd()))
                        Loop While (R = TournamentElter1)
                        TournamentElter2 = Elternspeicher(R)

                        If Front(TournamentElter1, PES_iAkt.iAktPop) < Front(TournamentElter2, PES_iAkt.iAktPop) Then
                            R = TournamentElter1
                        ElseIf Front(TournamentElter1, PES_iAkt.iAktPop) > Front(TournamentElter2, PES_iAkt.iAktPop) Then
                            R = TournamentElter2
                        Else
                            If Div(TournamentElter1, PES_iAkt.iAktPop) > Div(TournamentElter2, PES_iAkt.iAktPop) Then
                                R = TournamentElter1
                            ElseIf Div(TournamentElter1, PES_iAkt.iAktPop) = Div(TournamentElter2, PES_iAkt.iAktPop) Then
                                R = CInt(Int(2 * Rnd())) 'Zufallsszahl zwischen 0 und 1 
                                If R = 0 Then
                                    R = TournamentElter1
                                Else
                                    R = TournamentElter2
                                End If
                            Else
                                R = TournamentElter2
                            End If
                        End If

                    Else
                        R = CInt(Int((Settings.PES.n_Eltern - (i)) * Rnd()))
                    End If
                    'Kein Elter darf doppelt gezogen werden
                    Realisierungsspeicher(i) = Elternspeicher(R)
                    For j = R To Settings.PES.n_Eltern - 2
                        Elternspeicher(j) = Elternspeicher(j + 1)
                    Next j
                Next i
                For v = 0 To Anz.Para - 1
                    R = CInt(Int(Settings.PES.n_RekombXY * Rnd()))
                    'Selektion der Schrittweite
                    AktPara(v).Dn = De(v, Realisierungsspeicher(R), PES_iAkt.iAktPop)
                    'Selektion des Elter
                    AktPara(v).Xn = Xe(v, Realisierungsspeicher(R), PES_iAkt.iAktPop)
                Next v


            Case EVO_ELTERN.XY_Mitteln 'Multi-Rekombination nach X/Y-Schema, Mittelung der Gene

                'Realisierungsspeicher und Elternspeicher initialisieren
                'Anzahl der benötigten Eltern (Y)
                ReDim Realisierungsspeicher(Settings.PES.n_RekombXY - 1)
                'Anzahl der Verfügbaren Eltern (n_Eltern)
                ReDim Elternspeicher(Settings.PES.n_Eltern - 1)
                'Setzen der Eltern Indizes
                For i = 0 To (Settings.PES.n_Eltern - 1)
                    Elternspeicher(i) = i
                Next
                'Auswahl der Y-Eltern
                For i = 0 To Settings.PES.n_RekombXY - 1
                    '1. Runde erlaubt Auswahl aus allen Eltern
                    '2. Runde hat nur noch n_Eltern - 1 zur Verfügung
                    'usw.
                    'Kein Elter darf doppelt gezogen werden
                    If (Settings.PES.is_DiversityTournament) Then

                        R = CInt(Int((Settings.PES.n_Eltern - i) * Rnd()))
                        TournamentElter1 = Elternspeicher(R)

                        Do
                            R = CInt(Int((Settings.PES.n_Eltern - i) * Rnd()))
                        Loop While (R = TournamentElter1)
                        TournamentElter2 = Elternspeicher(R)

                        If Front(TournamentElter1, PES_iAkt.iAktPop) < Front(TournamentElter2, PES_iAkt.iAktPop) Then
                            R = TournamentElter1
                        ElseIf Front(TournamentElter1, PES_iAkt.iAktPop) > Front(TournamentElter2, PES_iAkt.iAktPop) Then
                            R = TournamentElter2
                        Else
                            If Div(TournamentElter1, PES_iAkt.iAktPop) > Div(TournamentElter2, PES_iAkt.iAktPop) Then
                                R = TournamentElter1
                            ElseIf Div(TournamentElter1, PES_iAkt.iAktPop) = Div(TournamentElter2, PES_iAkt.iAktPop) Then
                                R = CInt(Int(2 * Rnd())) 'Zufallsszahl zwischen 0 und 1 
                                If R = 0 Then
                                    R = TournamentElter1
                                Else
                                    R = TournamentElter2
                                End If
                            Else
                                R = TournamentElter2
                            End If
                        End If

                    Else
                        R = CInt(Int((Settings.PES.n_Eltern - (i)) * Rnd()))
                    End If
                    'Kein Elter darf doppelt gezogen werden
                    Realisierungsspeicher(i) = Elternspeicher(R)
                    For j = R To Settings.PES.n_Eltern - 2
                        Elternspeicher(j) = Elternspeicher(j + 1)
                    Next j
                Next i

                For v = 0 To Anz.Para - 1
                    AktPara(v).Dn = 0
                    AktPara(v).Xn = 0
                    For n = 0 To Settings.PES.n_RekombXY - 1
                        'Mittelung der Schrittweite,
                        AktPara(v).Dn = AktPara(v).Dn + (De(v, Realisierungsspeicher(n), PES_iAkt.iAktPop) / Settings.PES.n_RekombXY)
                        'Mittelung der Eltern,
                        AktPara(v).Xn = AktPara(v).Xn + (Xe(v, Realisierungsspeicher(n), PES_iAkt.iAktPop) / Settings.PES.n_RekombXY)
                    Next
                Next v

            Case EVO_ELTERN.XY_Mitteln_Diskret
                'Realisierungsspeicher und Elternspeicher initialisieren
                'Anzahl der benötigten Eltern (Y)
                ReDim Realisierungsspeicher(Settings.PES.n_RekombXY - 1)
                'Anzahl der Verfügbaren Eltern (n_Eltern)
                ReDim Elternspeicher(Settings.PES.n_Eltern - 1)
                'Setzen der Eltern Indizes
                For i = 0 To (Settings.PES.n_Eltern - 1)
                    Elternspeicher(i) = i
                Next
                'Auswahl der Y-Eltern
                For i = 0 To Settings.PES.n_RekombXY - 1
                    '1. Runde erlaubt Auswahl aus allen Eltern
                    '2. Runde hat nur noch n_Eltern - 1 zur Verfügung
                    'usw.
                    'Kein Elter darf doppelt gezogen werden
                    If (Settings.PES.is_DiversityTournament) Then

                        R = CInt(Int((Settings.PES.n_Eltern - i) * Rnd()))
                        TournamentElter1 = Elternspeicher(R)

                        Do
                            R = CInt(Int((Settings.PES.n_Eltern - i) * Rnd()))
                        Loop While (R = TournamentElter1)
                        TournamentElter2 = Elternspeicher(R)

                        If Front(TournamentElter1, PES_iAkt.iAktPop) < Front(TournamentElter2, PES_iAkt.iAktPop) Then
                            R = TournamentElter1
                        ElseIf Front(TournamentElter1, PES_iAkt.iAktPop) > Front(TournamentElter2, PES_iAkt.iAktPop) Then
                            R = TournamentElter2
                        Else
                            If Div(TournamentElter1, PES_iAkt.iAktPop) > Div(TournamentElter2, PES_iAkt.iAktPop) Then
                                R = TournamentElter1
                            ElseIf Div(TournamentElter1, PES_iAkt.iAktPop) = Div(TournamentElter2, PES_iAkt.iAktPop) Then
                                R = CInt(Int(2 * Rnd())) 'Zufallsszahl zwischen 0 und 1 
                                If R = 0 Then
                                    R = TournamentElter1
                                Else
                                    R = TournamentElter2
                                End If
                            Else
                                R = TournamentElter2
                            End If
                        End If

                    Else
                        R = CInt(Int((Settings.PES.n_Eltern - (i)) * Rnd()))
                    End If
                    'Kein Elter darf doppelt gezogen werden
                    Realisierungsspeicher(i) = Elternspeicher(R)
                    For j = R To Settings.PES.n_Eltern - 2
                        Elternspeicher(j) = Elternspeicher(j + 1)
                    Next j

                Next i
                For v = 0 To Anz.Para - 1
                    AktPara(v).Dn = 0
                    R = CInt(Int(Settings.PES.n_RekombXY * Rnd()))
                    'Mittelung der Schrittweite
                    For n = 0 To Settings.PES.n_RekombXY - 1
                        AktPara(v).Dn = AktPara(v).Dn + (De(v, Realisierungsspeicher(n), PES_iAkt.iAktPop) / Settings.PES.n_RekombXY)
                    Next
                    'Selektion des Elter
                    AktPara(v).Xn = Xe(v, Realisierungsspeicher(R), PES_iAkt.iAktPop)
                Next v


            Case EVO_ELTERN.Neighbourhood 'Neighbourhood Rekombination

                'Z1 = Int(Settings.PES.n_Eltern * Rnd())
                'Do
                '    Z2 = Int(Settings.PES.n_Eltern * Rnd())
                'Loop While Z1 = Z2

                ''Tournament über Crowding Distance
                'If Distanceb(Z1) > Distanceb(Z2) Then
                '    Elter = Z1
                'Else
                '    Elter = Z2
                'End If

                'Anzahl der Verfügbaren Eltern (n_Eltern)
                ReDim Elternspeicher(Settings.PES.n_Eltern - 1)
                'Setzen der Eltern Indizes
                For i = 0 To (Settings.PES.n_Eltern - 1)
                    Elternspeicher(i) = i
                Next
                If (Settings.PES.is_DiversityTournament) Then

                    R = CInt(Int((Settings.PES.n_Eltern) * Rnd()))
                    TournamentElter1 = Elternspeicher(R)

                    Do
                        R = CInt(Int((Settings.PES.n_Eltern) * Rnd()))
                    Loop While (R = TournamentElter1)
                    TournamentElter2 = Elternspeicher(R)

                    If Front(TournamentElter1, PES_iAkt.iAktPop) < Front(TournamentElter2, PES_iAkt.iAktPop) Then
                        R = TournamentElter1
                    ElseIf Front(TournamentElter1, PES_iAkt.iAktPop) > Front(TournamentElter2, PES_iAkt.iAktPop) Then
                        R = TournamentElter2
                    Else
                        If Div(TournamentElter1, PES_iAkt.iAktPop) > Div(TournamentElter2, PES_iAkt.iAktPop) Then
                            R = TournamentElter1
                        ElseIf Div(TournamentElter1, PES_iAkt.iAktPop) = Div(TournamentElter2, PES_iAkt.iAktPop) Then
                            R = CInt(Int(2 * Rnd())) 'Zufallsszahl zwischen 0 und 1 
                            If R = 0 Then
                                R = TournamentElter1
                            Else
                                R = TournamentElter2
                            End If
                        Else
                            R = TournamentElter2
                        End If
                    End If
                Else
                    R = CInt(Int((Settings.PES.n_Eltern) * Rnd()))
                End If

                Elter = R

                If (Elter = 0 Or Elter = Settings.PES.n_Eltern - 1) Then
                    For v = 0 To Anz.Para - 1
                        'Selektion der Schrittweite
                        AktPara(v).Dn = De(v, Elter, PES_iAkt.iAktPop)
                        'Selektion des Elter
                        AktPara(v).Xn = Xe(v, Elter, PES_iAkt.iAktPop)
                    Next
                Else
                    'BUG 135
                    Dim IndexEltern(Settings.PES.n_Eltern - 1) As Integer          'Array mit Index der Eltern (Neighbourhood-Rekomb.)
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

                        R = Int(Settings.PES.n_RekombXY * Rnd())
                        'Selektion der Schrittweite
                        AktPara(v).Dn = De(v, IndexEltern(R), PES_iAkt.iAktPop)
                        'Selektion des Elter
                        AktPara(v).Xn = Xe(v, IndexEltern(R), PES_iAkt.iAktPop)
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

        ReDim DeTemp(Anz.Para - 1, Settings.PES.n_Eltern - 1, Settings.PES.Pop.n_Popul - 1)
        ReDim XeTemp(Anz.Para - 1, Settings.PES.n_Eltern - 1, Settings.PES.Pop.n_Popul - 1)

StartMutation:

        'Einheitliche Schrittweite
        '-------------------------
        If (Not Settings.PES.Schrittweite.is_DnVektor) Then
            '+/-1
            expo = (2 * Int(Rnd() + 0.5) - 1)
            'Schrittweite wird mutiert
            DeTemp(0, 0, PES_iAkt.iAktPop) = De(0, 0, PES_iAkt.iAktPop) * palpha ^ expo
            'Schrittweite für alle übernehmen
            For n = 0 To Settings.PES.n_Eltern - 1
                For v = 0 To Anz.Para - 1
                    DeTemp(v, n, PES_iAkt.iAktPop) = DeTemp(0, 0, PES_iAkt.iAktPop)
                Next
            Next
        End If

        'Mutation
        '--------
        For n = 0 To Settings.PES.n_Eltern - 1
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
                    If (Settings.PES.Schrittweite.is_DnVektor) Then
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

            Div(n, PES_iAkt.iAktPop) = 0 'Diversität wird erst nach der ersten Generation bestimmt

        Next n

    End Sub

    'ES_MUTATION - Mutieren der Ausgangswerte
    '****************************************
    Public Sub EsMutation()

        Dim v, i As Integer
        Dim Z As Double

        Dim DnTemp() As Double             'Temporäre Schrittweiten für Nachkomme
        Dim XnTemp() As Double             'Temporäre Parameterwerte für Nachkomme
        Dim expo As Integer                  'Exponent für Schrittweite (+/-1)
        Dim tau As Double
        Dim taufix As Double
        Dim ZFix As Double

        ReDim DnTemp(Anz.Para - 1)
        ReDim XnTemp(Anz.Para - 1)

StartMutation:

        'Einheitliche Schrittweite
        '-------------------------
        If (Not Settings.PES.Schrittweite.is_DnVektor) Then
            If (Settings.PES.Schrittweite.OptDnMutation = EVO_DnMutation.Rechenberg) Then
                '+/-1
                expo = (2 * Int(Rnd() + 0.5) - 1)
                'Schrittweite wird mutiert
                DnTemp(0) = AktPara(0).Dn * galpha ^ expo
            ElseIf (Settings.PES.Schrittweite.OptDnMutation = EVO_DnMutation.Schwefel) Then
                tau = Settings.PES.Schrittweite.DnC / Math.Sqrt(Anz.Para)
                'Normalverteilte Zufallszahl (SD = 1, mean = 0)
                Z = Me.NormalDistributationRND(1.0, 0.0)
                'Neue Schrittweite
                DnTemp(0) = AktPara(0).Dn * Math.Exp(tau * Z)
                'Mindestschrittweite muss eingehalten werden
                If DnTemp(0) < Settings.PES.Schrittweite.DnEpsilon Then DnTemp(0) = Settings.PES.Schrittweite.DnEpsilon
            End If
            'Schrittweite für alle übernehmen
            For v = 1 To Anz.Para - 1
                DnTemp(v) = DnTemp(0)
            Next v
        End If

        'Mutation
        '--------
        If (Settings.PES.Schrittweite.is_DnVektor And Settings.PES.Schrittweite.OptDnMutation = EVO_DnMutation.Schwefel) Then
            taufix = Settings.PES.Schrittweite.DnC / Math.Sqrt(2 * Anz.Para)
            ZFix = Me.NormalDistributationRND(1.0, 0.0)
        End If
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
                If (Settings.PES.Schrittweite.is_DnVektor) Then

                    If (Settings.PES.Schrittweite.OptDnMutation = EVO_DnMutation.Rechenberg) Then
                        '+/-1
                        expo = (2 * Int(Rnd() + 0.5) - 1)
                        'Schrittweite wird mutiert
                        DnTemp(v) = AktPara(v).Dn * galpha ^ expo
                    ElseIf (Settings.PES.Schrittweite.OptDnMutation = EVO_DnMutation.Schwefel) Then
                        tau = Settings.PES.Schrittweite.DnC / Math.Sqrt(2 * Math.Sqrt(Anz.Para))
                        'Normalverteilte Zufallszahl (SD = 1, mean = 0)
                        Z = Me.NormalDistributationRND(1.0, 0.0)
                        'Neue Schrittweite
                        DnTemp(v) = AktPara(v).Dn * Math.Exp(taufix * ZFix + tau * Z)
                        'Mindestschrittweite muss eingehalten werden
                        If DnTemp(v) < Settings.PES.Schrittweite.DnEpsilon Then DnTemp(v) = Settings.PES.Schrittweite.DnEpsilon
                    End If
                End If

                'Normalverteilte Zufallszahl mit Standardabweichung 1/sqr(varanz)

                'Z = System.Math.Sqrt(-2 * System.Math.Log(1 - Rnd()) / Anz.Para) * System.Math.Sin(6.2832 * Rnd())
                If (Settings.PES.Schrittweite.OptDnMutation = EVO_DnMutation.Rechenberg) Then
                    'Normalverteilte Zufallszahl mit Standardabweichung 1/sqr(var.anz), , Mittelwert 0
                    Z = Me.NormalDistributationRND(1 / Math.Sqrt(Anz.Para), 0.0)
                ElseIf (Settings.PES.Schrittweite.OptDnMutation = EVO_DnMutation.Schwefel) Then
                    'Normalverteilte Zufallszahl mit Standardabweichung 1, Mittelwert 0
                    Z = Me.NormalDistributationRND(1.0, 0.0)
                End If
                'Mutation wird durchgeführt
                XnTemp(v) = AktPara(v).Xn + DnTemp(v) * Z

                'Restriktion für die mutierten Werte
            Loop While (XnTemp(v) <= 0 Or XnTemp(v) > 1 Or Not checkBeziehung(v, XnTemp))

        Next v

        'Mutierte Werte übernehmen
        '-------------------------
        For v = 0 To Anz.Para - 1
            AktPara(v).Dn = DnTemp(v)
            AktPara(v).Xn = XnTemp(v)
        Next v

    End Sub

    Public Function NormalDistributationRND(ByVal sd As Double, ByVal mean As Double) As Double
        Dim fac As Double
        Dim r As Double
        Dim V1 As Double
        Dim V2 As Double
        Dim gauss As Double
        Do
            V1 = 2 * Rnd() - 1
            V2 = 2 * Rnd() - 1
            r = V1 * V1 + V2 * V2
        Loop Until (r <= 1)
        fac = Math.Sqrt(-2 * Math.Log(r) / r)
        gauss = V2 * fac
        gauss = gauss * sd + mean
        Return gauss
    End Function

    'ES_POP_BEST - Einordnen der Qualitätsfunktion im PopulationsBestwertspeicher
    '****************************************************************************
    Public Sub EsPopBest()

        Dim m, i, j, n As Integer
        Dim h1, h2 As Double

        'Der schlechtetste der besten Qualitätswerte wird bestimmt ; Position -> i
        '(höchster Wert der Penaltyfunktion, niedrigster Wert der Crowding Distance)
        i = 0
        h1 = Qbpop(0, 0)
        For m = 1 To Settings.PES.Pop.n_Popul - 1
            If (Settings.PES.OptModus = EVO_MODUS.Single_Objective) Then
                If Qbpop(m, 0) > h1 Then
                    h1 = Qbpop(m, 0)
                    i = m
                End If
            Else
                Select Case Settings.PES.Pop.OptPopPenalty

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
        If (Settings.PES.OptModus = EVO_MODUS.Multi_Objective) Then
            j = 0
            h2 = Qbpop(0, 1)
            For m = 2 To Settings.PES.Pop.n_Popul
                If Qbpop(m, 1) < h2 Then
                    h2 = Qbpop(m, 1)
                    j = m
                End If
            Next m
        End If

        'Qualität der aktuellen Population wird bestimmt
        h1 = 0
        If (Settings.PES.OptModus = EVO_MODUS.Single_Objective) Then
            For m = 0 To Settings.PES.n_Eltern - 1
                h1 = h1 + Best.Qb(m, PES_iAkt.iAktPop, 0) / Settings.PES.n_Eltern
            Next m
        Else
            h2 = 0
            h1 = NDS_Crowding_Distance_Count(h2)
        End If

        'Falls die Qualität des aktuellen Population besser ist (Penaltyfunktion geringer)
        'als die schlechteste im Bestwertspeicher, wird diese ersetzt
        If (Settings.PES.OptModus = EVO_MODUS.Single_Objective) Then
            If h1 < Qbpop(i, 0) Then
                Qbpop(i, 0) = h1
                For m = 0 To Anz.Para - 1
                    For n = 0 To Settings.PES.n_Eltern - 1
                        'Die Schrittweite wird ebenfalls übernommen
                        Dbpop(m, n, i) = Best.Db(m, n, PES_iAkt.iAktPop)
                        'Die eigentlichen Parameterwerte werden übernommen
                        Xbpop(m, n, i) = Best.Xb(m, n, PES_iAkt.iAktPop)
                    Next n
                Next m
            End If
        Else
            Select Case Settings.PES.Pop.OptPopPenalty

                Case EVO_POP_PENALTY.Crowding
                    If h1 < Qbpop(i, 0) Then
                        Qbpop(i, 0) = h1
                        For m = 0 To Anz.Para - 1
                            For n = 0 To Settings.PES.n_Eltern - 1
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
                            For n = 0 To Settings.PES.n_Eltern - 1
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
    Public Sub EsBest(ByVal ind As Individuum)

        Dim m, i, j, v As Integer
        Dim h As Double

        If (Settings.PES.OptModus = EVO_MODUS.Single_Objective) Then
            'SO - Standard ES nach Rechenberg
            '--------------------------------
            'Der schlechteste der besten Qualitätswerte wird bestimmt ; Position -> j
            '(höchster Wert der Penaltyfunktion)
            j = 0
            h = Best.Qb(0, PES_iAkt.iAktPop, 0)

            For m = 1 To Settings.PES.n_Eltern - 1
                If Best.Qb(m, PES_iAkt.iAktPop, 0) > h Then
                    h = Best.Qb(m, PES_iAkt.iAktPop, 0)
                    j = m
                End If
            Next m

            'Falls die Qualität des aktuellen Nachkommen besser ist (Penaltyfunktion geringer)
            'als die schlechteste im Bestwertspeicher, wird dieser ersetzt
            If ind.Penalties(0) < Best.Qb(j, PES_iAkt.iAktPop, 0) Then
                Best.Qb(j, PES_iAkt.iAktPop, 0) = ind.Penalties(0)
                For v = 0 To Anz.Para - 1
                    'Die Schrittweite wird ebenfalls übernommen
                    Best.Db(v, j, PES_iAkt.iAktPop) = AktPara(v).Dn
                    'Die eigentlichen Parameterwerte werden übernommen
                    Best.Xb(v, j, PES_iAkt.iAktPop) = AktPara(v).Xn 'TODO: Hier die OptPara des übergebenen Individuums nehmen? 
                Next v
            End If

        Else
            'Multi-Objective Pareto
            '----------------------
            With NDSorting(PES_iAkt.iAktNachf)
                For i = 0 To Manager.AnzZiele - 1
                    .Zielwerte(i) = ind.Zielwerte(i)
                Next i
                For i = 0 To Anz.Constr - 1
                    .Constrain(i) = ind.Constrain(i)
                Next i
                .dominated = False
                .Front = 0
                For v = 0 To Anz.Para - 1
                    .PES_OptParas(v).Dn = AktPara(v).Dn 'TODO: Hier die OptPara des übergebenen Individuums nehmen? 
                    .PES_OptParas(v).Xn = AktPara(v).Xn
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

        If (Settings.PES.OptStrategie = EVO_STRATEGIE.Komma_Strategie) Then
            For n = 0 To Settings.PES.n_Eltern - 1
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

        If (Settings.PES.Pop.OptPopStrategie = EVO_STRATEGIE.Komma_Strategie) Then
            For n = 0 To Settings.PES.Pop.n_Popul - 1
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

        Select Case Settings.PES.Pop.OptPopPenalty
            Case EVO_POP_PENALTY.Crowding
                Z = 0
            Case EVO_POP_PENALTY.Spannweite
                Z = 1
        End Select

        ReDim Realisierungsspeicher(Settings.PES.Pop.n_Popul - 1, 1)

        'Die NPopEltern besten Individium-Sätze werden ermittelt
        For m = 0 To Settings.PES.Pop.n_Popul - 1
            Realisierungsspeicher(m, 0) = Qbpop(m, Z)
            Realisierungsspeicher(m, 1) = m
        Next m

        If (Settings.PES.OptModus = EVO_MODUS.Single_Objective) Then
            'Standard ES nach Rechenberg
            '---------------------------
            For m = 0 To Settings.PES.Pop.n_Popul - 1
                For n = m To Settings.PES.Pop.n_Popul - 1
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
            Select Case Settings.PES.Pop.OptPopPenalty

                Case EVO_POP_PENALTY.Crowding
                    For m = 0 To Settings.PES.Pop.n_Popul - 1
                        For n = m To Settings.PES.Pop.n_Popul - 1
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
                    For m = 0 To Settings.PES.Pop.n_Popul - 1
                        For n = m To Settings.PES.Pop.n_Popul - 1
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
        For m = 0 To Settings.PES.Pop.n_PopEltern - 1
            For n = 0 To Settings.PES.n_Eltern - 1
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


        If (Settings.PES.OptModus = EVO_MODUS.Single_Objective) Then
            'Standard ES nach Rechenberg
            'xxxxxxxxxxxxxxxxxxxxxxxxxxx
            'Die Eltern werden gleich der besten Kinder gesetzt (Schrittweite und Parameterwert)
            '---------------------------------------------------------------------
            For i = 0 To Settings.PES.n_Eltern - 1
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
            For i = Settings.PES.n_Nachf To Settings.PES.n_Nachf + Settings.PES.n_Eltern - 1
                Call Copy_Bestwert_to_Individuum(i, i - Settings.PES.n_Nachf, NDSorting)
            Next i

            '********************* Alles in der Klasse Functions ****************************************
            'Zu Beginn den Bestwertspeicher in ein Individuum packen
            'Dimensionieren des Best_Indi
            Dim Best_Indi(Best.Qb.GetUpperBound(0)) As Individuum_PES
            Call Individuum_PES.New_Indi_Array("Bestwerte", Best_Indi)
            'Kopieren in Best_Indi
            For i = 0 To Best.Qb.GetUpperBound(0)
                Call Copy_Bestwert_to_Individuum(i, i, Best_Indi)
            Next
            '---------------------------------
            '2. Die einzelnen Fronten werden bestimmt
            '3. Der Bestwertspeicher wird entsprechend der Fronten oder der sekundären Population gefüllt
            '4: Sekundäre Population wird bestimmt und gespeichert
            '--------------------------------
            Dim Func1 As Kern.Functions = New Kern.Functions(Settings.PES.n_Nachf, Settings.PES.n_Eltern, Settings.PES.SekPop.is_Begrenzung, Settings.PES.SekPop.n_MaxMembers, Settings.PES.SekPop.n_Interact, Settings.PES.SekPop.is_Interact, PES_iAkt.iAktGen)
            Call Func1.EsEltern_Pareto(NDSorting, SekundärQb, Best_Indi)
            'Bestimmen der Crowding Distance falls Diversity-Tournament
            '----------------------------------------------------------
            If (Settings.PES.is_DiversityTournament) Then
                Call Func1.Pareto_Crowding_Distance(Best_Indi)
            End If

            'Am ende die Bestwerte wieder zurück
            For i = 0 To Best.Qb.GetUpperBound(0)
                Call Copy_Individuum_to_Bestwert(i, Best_Indi)
            Next
            '********************************************************************************************

            '5: Neue Eltern werden gleich dem Bestwertspeicher gesetzt
            '---------------------------------------------------------
            For i = 0 To Settings.PES.n_Eltern - 1
                For v = 0 To Anz.Para - 1
                    De(v, i, PES_iAkt.iAktPop) = Best.Db(v, i, PES_iAkt.iAktPop)
                    Xe(v, i, PES_iAkt.iAktPop) = Best.Xb(v, i, PES_iAkt.iAktPop)
                Next v
                If (Settings.PES.is_DiversityTournament) Then
                    Div(i, PES_iAkt.iAktPop) = Best.Div(i, PES_iAkt.iAktPop)
                    Front(i, PES_iAkt.iAktPop) = Best.Front(i, PES_iAkt.iAktPop)
                End If
            Next i

            '6: Sortierung der Lösungen ist nur für Neighbourhood-Rekombination notwendig
            '----------------------------------------------------------------------------
            If (Settings.PES.OptEltern = EVO_ELTERN.Neighbourhood) Then
                Call Neighbourhood_AbstandsArray()
                Call Neighbourhood_Crowding_Distance()
            End If


        End If

    End Sub

    'Kopiert ein Individuum in den Bestwertspeicher
    '----------------------------------------------
    Public Sub Copy_Individuum_to_Bestwert(ByVal i As Integer, ByVal Individ() As Individuum_PES)
        Dim j, v As Integer

        For j = 0 To Anz.Penalty - 1
            Best.Qb(i, PES_iAkt.iAktPop, j) = Individ(i).Penalties(j)
        Next j

        If Anz.Constr > 0 Then
            For j = 0 To Anz.Constr - 1
                Best.Rb(i, PES_iAkt.iAktPop, j) = Individ(i).Constrain(j)
            Next j
        End If

        For v = 0 To Anz.Para - 1
            Best.Db(v, i, PES_iAkt.iAktPop) = Individ(i).PES_OptParas(v).Dn
            Best.Xb(v, i, PES_iAkt.iAktPop) = Individ(i).PES_OptParas(v).Xn
        Next v
        If (Settings.PES.is_DiversityTournament) Then
            Best.Div(i, PES_iAkt.iAktPop) = Individ(i).Distance
            Best.Front(i, PES_iAkt.iAktPop) = Individ(i).Front
        End If
    End Sub

    'Kopiert den Bestwertspeicher in ein Individuum
    '----------------------------------------------
    Public Sub Copy_Bestwert_to_Individuum(ByVal i_indi As Integer, ByVal i_best As Integer, ByRef Individ() As Individuum_PES)

        Dim i, j, v As Integer

        j = 0
        For i = 0 To Manager.AnzZiele - 1
            'HACK: Nur Zielwerte von OptZielen (d.h. Penalty) werden kopiert!
            If (Manager.List_Ziele(i).isOpt) Then
                Individ(i_indi).Zielwerte(i) = Best.Qb(i_best, PES_iAkt.iAktPop, j)
                j += 1
            End If
        Next i

        If Anz.Constr > 0 Then
            For j = 0 To Anz.Constr - 1
                Individ(i_indi).Constrain(j) = Best.Rb(i_best, PES_iAkt.iAktPop, j)
            Next j
        End If

        For v = 0 To Anz.Para - 1
            Individ(i_indi).PES_OptParas(v).Dn = Best.Db(v, i_best, PES_iAkt.iAktPop)
            Individ(i_indi).PES_OptParas(v).Xn = Best.Xb(v, i_best, PES_iAkt.iAktPop)
        Next v

        Individ(i_indi).dominated = False
        Individ(i_indi).Front = 0
        Individ(i_indi).Distance = 0

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

        ReDim TempDistance(Anz.Penalty - 1)
        ReDim PenaltyDistance(Settings.PES.n_Eltern - 1, Settings.PES.n_Eltern - 1)
        ReDim d(Settings.PES.n_Eltern - 1)

        'Bestimmen der normierten Raumabstände zwischen allen Elternindividuen
        For i = 0 To Settings.PES.n_Eltern - 1
            PenaltyDistance(i, i) = 0
            For j = i + 1 To Settings.PES.n_Eltern - 1
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

        For i = 0 To Settings.PES.n_Eltern - 2
            d(i) = 1.0E+300
            For j = 0 To i - 1
                If (PenaltyDistance(i, j) < d(i)) Then d(i) = PenaltyDistance(i, j)
            Next j
            For j = i + 1 To Settings.PES.n_Eltern - 1
                If (PenaltyDistance(i, j) < d(i)) Then d(i) = PenaltyDistance(i, j)
            Next j
            d_mean += d(i)
        Next i

        d_mean = d_mean / Settings.PES.n_Eltern
        NDS_Crowding_Distance_Count = 0

        For i = 0 To Settings.PES.n_Eltern - 2
            NDS_Crowding_Distance_Count += (d_mean - d(i)) * (d_mean - d(i))
        Next i

        NDS_Crowding_Distance_Count = NDS_Crowding_Distance_Count / Settings.PES.n_Eltern
        NDS_Crowding_Distance_Count = System.Math.Sqrt(NDS_Crowding_Distance_Count)

        Spannweite = 0
        For i = 0 To Settings.PES.n_Eltern - 1
            'TODO: sollte hier nicht j = i + 1 stehen?
            For j = i To Settings.PES.n_Eltern - 1
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
            For j = 0 To Settings.PES.n_Eltern - 1
                If (Min > Best.Qb(j, PES_iAkt.iAktPop, k)) Then Min = Best.Qb(j, PES_iAkt.iAktPop, k)
                If (Max < Best.Qb(j, PES_iAkt.iAktPop, k)) Then Max = Best.Qb(j, PES_iAkt.iAktPop, k)
            Next j
            MinMax(k) = Max - Min
        Next k

        'Bestimmen der normierten Raumabstände zwischen allen Elternindividuen
        ReDim TempDistance(Anz.Penalty)

        For i = 0 To Settings.PES.n_Eltern - 1
            PenaltyDistance(i, i) = 0
            For j = i + 1 To Settings.PES.n_Eltern - 1
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
        ReDim Nachbarn(Settings.PES.n_Eltern - 2)

        For i = 0 To IndexElter - 2
            Nachbarn(i).distance = PenaltyDistance(IndexElter, i)
            Nachbarn(i).Index = i
        Next i
        For i = IndexElter To Settings.PES.n_Eltern - 1
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

        For i = 0 To Settings.PES.n_RekombXY - 1
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

        ReDim QbTemp(Settings.PES.n_Eltern - 1, Settings.PES.Pop.n_Popul - 1, Anz.Penalty - 1)

        Array.Copy(Best.Qb, QbTemp, Best.Qb.GetLength(0))
        For i = 0 To Settings.PES.n_Eltern - 1
            Distanceb(i) = 0
        Next i

        For k = 0 To Anz.Penalty - 1
            For i = 0 To Settings.PES.n_Eltern - 1
                For j = 0 To Settings.PES.n_Eltern - 1
                    If (QbTemp(i, PES_iAkt.iAktPop, k) < QbTemp(j, PES_iAkt.iAktPop, k)) Then
                        swap = QbTemp(i, PES_iAkt.iAktPop, k)
                        QbTemp(i, PES_iAkt.iAktPop, k) = QbTemp(j, PES_iAkt.iAktPop, k)
                        QbTemp(j, PES_iAkt.iAktPop, k) = swap
                    End If
                Next j
            Next i

            fmin = QbTemp(0, PES_iAkt.iAktPop, k)
            fmax = QbTemp(Settings.PES.n_Eltern - 1, PES_iAkt.iAktPop, k)

            Distanceb(0) = 1.0E+300
            Distanceb(Settings.PES.n_Eltern - 1) = 1.0E+300

            For i = 1 To Settings.PES.n_Eltern - 2
                Distanceb(i) = Distanceb(i) + (QbTemp(i + 1, PES_iAkt.iAktPop, k) - QbTemp(i - 1, PES_iAkt.iAktPop, k)) / (fmax - fmin)
            Next i
        Next k

    End Sub

    'Einen Parameterwert auf Einhaltung der Beziehung überprüfen
    '***********************************************************
    Private Function checkBeziehung(ByVal ipara As Integer, ByVal XnTemp() As Double) As Boolean

        'ipara ist der Index des zu überprüfenden Parameters,
        'XnTemp() die zu prüfenden (skalierten) Werte

        Dim isOK As Boolean = False
        If (AktPara(ipara).Beziehung = Beziehung.keine) Then
            'Keine Beziehung vorhanden
            isOK = True
        Else
            'Referenzierten Parameterwert vergleichen (reelle Werte!)
            Dim wert As Double = AktPara(ipara).Min + (AktPara(ipara).Max - AktPara(ipara).Min) * XnTemp(ipara)
            Dim ref As Double = AktPara(ipara - 1).Min + (AktPara(ipara - 1).Max - AktPara(ipara - 1).Min) * XnTemp(ipara - 1)
            Select Case AktPara(ipara).Beziehung
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
        If (AktPara(ipara).Beziehung = Beziehung.keine) Then
            'Keine Beziehung vorhanden
            isOK = True
        Else
            'Referenzierten Parameterwert vergleichen
            Dim wert As Double = AktPara(ipara).Min + (AktPara(ipara).Max - AktPara(ipara).Min) * XeTemp(ipara, iElter, PES_iAkt.iAktPop)
            Dim ref As Double = AktPara(ipara - 1).Min + (AktPara(ipara - 1).Max - AktPara(ipara - 1).Min) * XeTemp(ipara - 1, iElter, PES_iAkt.iAktPop)
            Select Case AktPara(ipara).Beziehung
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
