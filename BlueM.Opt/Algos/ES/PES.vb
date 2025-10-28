'BlueM.Opt
'Copyright (C) BlueM Dev Group
'Website: <https://www.bluemodel.org>
'
'This program is free software: you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.
'
'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
'GNU General Public License for more details.
'
'You should have received a copy of the GNU General Public License
'along with this program. If not, see <https://www.gnu.org/licenses/>.
'
Imports BlueM.Opt.Common

''' <summary>
''' Klasse PES (Parametric Evolution Strategy)
''' Modifizierte Evolutionsstrategie nach Rechenberg und Schwefel
''' Klasse enthält alle Funktionen und Methoden zur Anwendung der Evolutionsstrategie
''' Literatur:
''' 1) Rechenberg, Ingo, Evolutionsstrategie '94, Fromman-Holzboog, 1994
''' 2) Schwefel, Hans-Paul, Evolution and Optimum Seeking, Wiley, 1995
''' 3) Deb, Kalyanmoy, Multi-Objective Optimization using Evolutionary Algorithms, Wiley, 2001
''' </summary>
Public Class PES

    'Deklarationsteil Variablen und Strukturen
    '#########################################

    Private mSettings As Settings

    'Das Problem
    Private mProblem As Problem

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
    Public SekundärQb(-1) As Individuum     'Sekundäre Population wird mit -1 initialisiert dann länge 0

    Const galpha As Double = 1.3            'Faktor alpha = 1.3 auf Generationsebene nach Rechenberg
    Const palpha As Double = 1.1            'Faktor alpha = 1.1 auf Populationsebene nach Rechenberg

    Dim NDSorting() As Individuum_PES
    Private Property INDSorting() As Individuum()
        Get
            Dim inds() As Individuum
            ReDim inds(Me.NDSorting.GetUpperBound(0))
            Call Array.Copy(Me.NDSorting, inds, Me.NDSorting.Length)
            Return inds
        End Get
        Set(ByVal value As Individuum())
            ReDim Me.NDSorting(value.GetUpperBound(0))
            Call Array.Copy(value, Me.NDSorting, value.Length)
        End Set
    End Property

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
    Public Sub PesInitialise(ByRef settings As Settings, ByRef prob As Problem)

        'Problem speichern
        Me.mProblem = prob

        'Schritt 1: PES - SETTINGS
        'Optionen der Evolutionsstrategie werden übergeben
        Call PES_Form_Settings(settings)

        'Schritt 2: PES - PREPARE
        'Interne Variablen werden initialisiert, Zufallsgenerator wird initialisiert
        Call PES_Prepare()

        'Schritt 3: PES - STARTVALUES
        'Startwerte werden zugewiesen
        Call PES_Startvalues()

    End Sub

    'Schritt 1: SETTINGS
    'Function ES_SETTINGS übergibt Optionen für Evolutionsstrategie und Prüft die eingestellten Optionen
    '***************************************************************************************************
    Private Sub PES_Form_Settings(ByRef settings As Settings)

        'Überprüfung der Übergebenen Werte
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        If (Not System.Enum.IsDefined(GetType(EVO_STRATEGY), settings.PES.Strategie)) Then
            Throw New Exception("Invalid setting for 'Strategy'!")
        End If
        If (Not System.Enum.IsDefined(GetType(EVO_STARTPARAMETERS), settings.PES.Startparameter)) Then
            Throw New Exception("Invalid setting for 'Start values'!")
        End If
        'Schrittweite
        If (Not System.Enum.IsDefined(GetType(PES_MUTATIONSOP), settings.PES.Mutationsop)) Then
            Throw New Exception("Invalid setting for 'Mutation'!")
        End If
        If (settings.PES.SetMutation.DnStart < 0) Then
            Throw New Exception("The starting step size can not be less than 0!")
        End If
        'Generationen
        If (settings.PES.N_Gen < 1) Then
            Throw New Exception("The number of generations can not be less than 1!")
        End If
        If (settings.PES.N_Eltern < 1) Then
            Throw New Exception("The number of parents can not be less than 1!")
        End If
        If (settings.PES.N_Nachf <= settings.PES.N_Eltern) Then
            Throw New Exception("The number of parents must be less than the number of children!" & Chr(13) & Chr(10) & "Rechenberg (1973) recommends using ratios between 1:3 and 1:5.")
        End If
        If (settings.PES.N_Nachf < 1) Then
            Throw New Exception("The number of children can not be less than 1!")
        End If
        'Eltern
        If (Not System.Enum.IsDefined(GetType(PES_REPRODOP), settings.PES.Reproduktionsop)) Then
            Throw New Exception("Invalid setting for the determination of parents!")
        End If
        If (settings.PES.OptModus = EVO_MODE.Single_Objective And settings.PES.Reproduktionsop = PES_REPRODOP.Neighborhood) Then
            Throw New Exception("The option 'Neighborhood' for the determination of parents can not be used for single objective problems!")
        End If
        If (settings.PES.N_RekombXY < 1) Then
            Throw New Exception("The value for the X/Y recombination can not be less than 1!")
        End If
        'Populationen
        If (settings.PES.Pop.N_Runden < 1) Then
            Throw New Exception("The number of rounds can not be less than 1!")
        End If
        If (settings.PES.Pop.N_Popul < 1) Then
            Throw New Exception("The number of populations can not be less than 1!")
        End If
        If (settings.PES.Pop.N_PopEltern < 1) Then
            Throw New Exception("The number of population parents can not be less than 1!")
        End If
        If (settings.PES.Pop.N_Popul < settings.PES.Pop.N_PopEltern) Then
            Throw New Exception("The number of population parents can not be larger than the number of populations!")
        End If
        If (Not System.Enum.IsDefined(GetType(EVO_POP_ELTERN), settings.PES.Pop.PopEltern)) Then
            Throw New Exception("Invalid setting for the determination of population parents!")
        End If
        If (Not System.Enum.IsDefined(GetType(EVO_STRATEGY), settings.PES.Pop.PopStrategie)) Then
            Throw New Exception("Invalid setting for 'Selection' at the population level!")
        End If

        'Übergabe der Optionen
        'xxxxxxxxxxxxxxxxxxxxx

        Me.mSettings = settings

    End Sub

    'Schritt 2: PREPARE
    'Initialisiert benötigte dynamische Arrays und legt Anzahl der Zielfunktionen fest
    'Initialisiert alle internen Arrays und setzt den
    'Bestwertspeicher auf sehr großen Wert (Strategie minimiert in dieser Umsetzung)
    'TODO: ESPrepare Für Paretooptimierung noch nicht fertig!!!!
    '*******************************************************************************
    Private Sub PES_Prepare()
        Dim m, n, l As Integer

        'Überprüfung der Eingabeparameter (es muss mindestens ein Parameter variiert und eine
        'Penaltyfunktion ausgewertet werden)

        If (Me.mProblem.NumOptParams <= 0 Or Me.mProblem.NumPrimObjective <= 0) Then
            Throw New Exception("You must have at least one optimization parameter and one primary objective!")
        End If

        'Dynamisches Array Initialisieren
        ReDim AktPara(Me.mProblem.NumOptParams - 1)                'Variablenvektor wird initialisiert

        'Parametervektoren initialisieren
        ReDim Dp(Me.mProblem.NumOptParams - 1, mSettings.PES.N_Eltern - 1, mSettings.PES.Pop.N_PopEltern - 1)
        ReDim Xp(Me.mProblem.NumOptParams - 1, mSettings.PES.N_Eltern - 1, mSettings.PES.Pop.N_PopEltern - 1)
        ReDim Qbpop(mSettings.PES.Pop.N_Popul - 1, Me.mProblem.NumPrimObjective - 1)
        ReDim Dbpop(Me.mProblem.NumOptParams - 1, mSettings.PES.N_Eltern - 1, mSettings.PES.Pop.N_Popul - 1)
        ReDim Xbpop(Me.mProblem.NumOptParams - 1, mSettings.PES.N_Eltern - 1, mSettings.PES.Pop.N_Popul - 1)
        '---------------------
        ReDim De(Me.mProblem.NumOptParams - 1, mSettings.PES.N_Eltern - 1, mSettings.PES.Pop.N_Popul - 1)
        ReDim Xe(Me.mProblem.NumOptParams - 1, mSettings.PES.N_Eltern - 1, mSettings.PES.Pop.N_Popul - 1)
        ReDim Div(mSettings.PES.N_Eltern - 1, mSettings.PES.Pop.N_Popul - 1)
        ReDim Front(mSettings.PES.N_Eltern - 1, mSettings.PES.Pop.N_Popul - 1)
        '---------------------
        ReDim Best.Db(Me.mProblem.NumOptParams - 1, mSettings.PES.N_Eltern - 1, mSettings.PES.Pop.N_Popul - 1)
        ReDim Best.Xb(Me.mProblem.NumOptParams - 1, mSettings.PES.N_Eltern - 1, mSettings.PES.Pop.N_Popul - 1)
        ReDim Best.Qb(mSettings.PES.N_Eltern - 1, mSettings.PES.Pop.N_Popul - 1, Me.mProblem.NumPrimObjective - 1)
        ReDim Best.Rb(mSettings.PES.N_Eltern - 1, mSettings.PES.Pop.N_Popul - 1, Me.mProblem.NumConstraints - 1)
        ReDim Best.Div(mSettings.PES.N_Eltern - 1, mSettings.PES.Pop.N_Popul - 1)
        ReDim Best.Front(mSettings.PES.N_Eltern - 1, mSettings.PES.Pop.N_Popul - 1)

        'NDSorting wird nur benötigt, falls eine Paretofront approximiert wird
        If (mSettings.PES.OptModus = EVO_MODE.Multi_Objective) Then
            NDSorting = Individuum.New_Indi_Array(Individuum.Individuumsklassen.Individuum_PES, mSettings.PES.N_Eltern + mSettings.PES.N_Nachf, "NDSorting")
            If (mSettings.PES.Reproduktionsop = PES_REPRODOP.Neighborhood) Then
                ReDim PenaltyDistance(mSettings.PES.N_Eltern - 1, mSettings.PES.N_Eltern - 1)
                ReDim Distanceb(mSettings.PES.N_Eltern - 1)
            End If
        End If

        For n = 0 To mSettings.PES.N_Eltern - 1
            For m = 0 To mSettings.PES.Pop.N_Popul - 1
                For l = 0 To Me.mProblem.NumPrimObjective - 1
                    'Qualität der Eltern (Anzahl = parents) wird auf sehr großen Wert gesetzt
                    Best.Qb(n, m, l) = 1.0E+300
                Next l
                If (Me.mProblem.NumConstraints > 0) Then
                    For l = 0 To Me.mProblem.NumConstraints - 1
                        'Restriktion der Eltern (Anzahl = parents) wird auf sehr kleinen Wert gesetzt
                        Best.Rb(n, m, l) = -1.0E+300
                    Next l
                End If
            Next m
        Next

        'Falls NDSorting Crowding Distance wird initialisiert
        If (mSettings.PES.OptModus = EVO_MODE.Multi_Objective) Then
            For n = 0 To mSettings.PES.Pop.N_Popul - 1
                For m = 0 To Me.mProblem.NumPrimObjective - 1
                    Select Case mSettings.PES.Pop.PopPenalty

                        Case EVO_POP_PENALTY.Crowding
                            'Qualität der Populationseltern wird auf sehr großen Wert gesetzt
                            Qbpop(n, m) = 1.0E+300

                        Case EVO_POP_PENALTY.Span
                            'Qualität der Populationseltern wird auf 0 gesetzt
                            Qbpop(n, m) = 0
                    End Select
                Next m
            Next n
        Else
            For n = 0 To mSettings.PES.Pop.N_Popul - 1
                For m = 0 To Me.mProblem.NumPrimObjective - 1
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
    Public Sub PES_Startvalues()

        Dim i As Integer

        'Dynamisches Array wird mit Werten belegt
        For i = 0 To Me.mProblem.NumOptParams - 1
            If (Me.mProblem.List_OptParameter(i).Xn < 0 Or Me.mProblem.List_OptParameter(i).Xn > 1) Then
                Throw New Exception($"The start value of the optimization parameter '{Me.mProblem.List_OptParameter(i).Bezeichnung}' is not between 0 und 1. It must be scaled to this range!")
            End If
            AktPara(i) = Me.mProblem.List_OptParameter(i).Clone()
            'Startschrittweite übernehmen
            AktPara(i).Dn = mSettings.PES.SetMutation.DnStart
        Next

        Dim n, v, m As Integer

        'Zufallsgenerator initialisieren
        Randomize()

        'Die Startparameter für die Eltern werden gesetzt
        Select Case Me.mSettings.PES.Startparameter

            Case EVO_STARTPARAMETERS.Random 'Zufällige Startwerte
                For v = 0 To Me.mProblem.NumOptParams - 1
                    For n = 0 To mSettings.PES.N_Eltern - 1
                        For m = 0 To mSettings.PES.Pop.N_PopEltern - 1
                            'Startwert für die Elternschrittweite wird zugewiesen
                            Dp(v, n, m) = AktPara(0).Dn
                            'Startwert für die Eltern werden zugewiesen
                            '(Zufallszahl zwischen 0 und 1)
                            Xp(v, n, m) = Rnd()
                        Next m
                    Next n
                Next v

            Case EVO_STARTPARAMETERS.Original 'Originalparameter
                For v = 0 To Me.mProblem.NumOptParams - 1
                    For n = 0 To mSettings.PES.N_Eltern - 1
                        For m = 0 To mSettings.PES.Pop.N_PopEltern - 1
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

        ReDim Bestwert(mSettings.PES.N_Eltern - 1, Me.mProblem.NumPrimObjective - 1)

        For i = 0 To Me.mProblem.NumPrimObjective - 1
            For j = 0 To mSettings.PES.N_Eltern - 1
                Bestwert(j, i) = Best.Qb(j, PES_iAkt.iAktPop, i)
            Next j
        Next i

        Return Bestwert

    End Function

    'ES_POP_VARIA - REPRODUKTIONSPROZESS - ToDo: Beschreibung fehlt
    '*******************************************************************************
    Public Sub EsPopReproduktion()

        Dim m, n, v As Integer
        Dim R As Integer                      'Zufälliger Integer Wert

        Select Case mSettings.PES.Pop.PopEltern

            Case EVO_POP_ELTERN.Recombination 'MultiRekombination über alle Eltern (x/x,y) oder (x/x+y)
                For n = 0 To mSettings.PES.N_Eltern - 1
                    R = Int(mSettings.PES.Pop.N_PopEltern * Rnd())
                    For v = 0 To Me.mProblem.NumOptParams - 1
                        'Selektion der Schrittweite
                        De(v, n, PES_iAkt.iAktPop) = Dp(v, n, R)
                        'Selektion des Elter
                        Xe(v, n, PES_iAkt.iAktPop) = Xp(v, n, R)
                    Next v
                Next n

            Case EVO_POP_ELTERN.Average 'Mittelwertbildung über alle Eltern
                'Ermitteln der Elter und Schrittweite über Mittelung der Elternschrittweiten
                For v = 0 To Me.mProblem.NumOptParams - 1
                    For n = 0 To mSettings.PES.N_Eltern - 1
                        De(v, n, PES_iAkt.iAktPop) = 0
                        Xe(v, n, PES_iAkt.iAktPop) = 0
                        For m = 0 To mSettings.PES.Pop.N_PopEltern - 1
                            'Mittelung der Schrittweite,
                            De(v, n, PES_iAkt.iAktPop) = De(v, n, PES_iAkt.iAktPop) + (Dp(v, n, m) / mSettings.PES.Pop.N_PopEltern)
                            'Mittelung der Eltern,
                            Xe(v, n, PES_iAkt.iAktPop) = Xe(v, n, PES_iAkt.iAktPop) + (Xp(v, n, m) / mSettings.PES.Pop.N_PopEltern)
                        Next m
                    Next n
                Next v

            Case EVO_POP_ELTERN.Selection 'Zufallswahl über alle Eltern
                R = Int(mSettings.PES.Pop.N_PopEltern * Rnd()) 'Zufallszahl entscheidet welcher
                'Elternteil vererbt wird
                For v = 0 To Me.mProblem.NumOptParams - 1
                    For n = 0 To mSettings.PES.N_Eltern - 1
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

        Select Case mSettings.PES.Reproduktionsop

            Case PES_REPRODOP.Selection 'Zufallswahl über alle Eltern

                R = Int(mSettings.PES.N_Eltern * Rnd())    'Zufallszahl entscheidet
                'welcher Enternteil vererbt wird
                For v = 0 To Me.mProblem.NumOptParams - 1
                    'Selektion der Schrittweite
                    AktPara(v).Dn = De(v, R, PES_iAkt.iAktPop)
                    'Selektion des Elter
                    AktPara(v).Xn = Xe(v, R, PES_iAkt.iAktPop)
                Next v

            Case PES_REPRODOP.XX_Discrete 'Multi-Rekombination, diskret

                For v = 0 To Me.mProblem.NumOptParams - 1
                    R = Int(mSettings.PES.N_Eltern * Rnd())
                    'Selektion der Schrittweite
                    AktPara(v).Dn = De(v, R, PES_iAkt.iAktPop)
                    'Selektion des Elter
                    AktPara(v).Xn = Xe(v, R, PES_iAkt.iAktPop)
                Next v

            Case PES_REPRODOP.XX_Average 'Multi-Rekombination, gemittelt

                For v = 0 To Me.mProblem.NumOptParams - 1
                    AktPara(v).Dn = 0
                    AktPara(v).Xn = 0

                    For n = 0 To mSettings.PES.N_Eltern - 1
                        'Mittelung der Schrittweite,
                        AktPara(v).Dn = AktPara(v).Dn + (De(v, n, PES_iAkt.iAktPop) / mSettings.PES.N_Eltern)
                        'Mittelung der Eltern,
                        AktPara(v).Xn = AktPara(v).Xn + (Xe(v, n, PES_iAkt.iAktPop) / mSettings.PES.N_Eltern)
                    Next
                Next v

            Case PES_REPRODOP.XX_Average_Discrete
                For v = 0 To Me.mProblem.NumOptParams - 1
                    AktPara(v).Dn = 0
                    R = Int(mSettings.PES.N_Eltern * Rnd())
                    'Mittelung der Schrittweite
                    For n = 0 To mSettings.PES.N_Eltern - 1
                        AktPara(v).Dn = AktPara(v).Dn + (De(v, n, PES_iAkt.iAktPop) / mSettings.PES.N_Eltern)
                    Next
                    'Selektion des Elter
                    AktPara(v).Xn = Xe(v, R, PES_iAkt.iAktPop)
                Next v


            Case PES_REPRODOP.XY_Discrete 'Multi-Rekombination nach X/Y-Schema, diskrete Vertauschung
                'Realisierungsspeicher und Elternspeicher initialisieren
                'Anzahl der benötigten Eltern (Y)
                ReDim Realisierungsspeicher(mSettings.PES.N_RekombXY - 1)
                'Anzahl der Verfügbaren Eltern (n_Eltern)
                ReDim Elternspeicher(mSettings.PES.N_Eltern - 1)
                'Setzen der Eltern Indizes
                For i = 0 To (mSettings.PES.N_Eltern - 1)
                    Elternspeicher(i) = i
                Next
                'Auswahl der Y-Eltern
                For i = 0 To mSettings.PES.N_RekombXY - 1
                    '1. Runde erlaubt Auswahl aus allen Eltern
                    '2. Runde hat nur noch n_Eltern - 1 zur Verfügung
                    'usw.
                    'Kein Elter darf doppelt gezogen werden
                    If (mSettings.PES.Is_DiversityTournament) Then

                        R = CInt(Int((mSettings.PES.N_Eltern - i) * Rnd()))
                        TournamentElter1 = Elternspeicher(R)

                        Do
                            R = CInt(Int((mSettings.PES.N_Eltern - i) * Rnd()))
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
                        R = CInt(Int((mSettings.PES.N_Eltern - (i)) * Rnd()))
                    End If
                    'Kein Elter darf doppelt gezogen werden
                    Realisierungsspeicher(i) = Elternspeicher(R)
                    For j = R To mSettings.PES.N_Eltern - 2
                        Elternspeicher(j) = Elternspeicher(j + 1)
                    Next j
                Next i
                For v = 0 To Me.mProblem.NumOptParams - 1
                    R = CInt(Int(mSettings.PES.N_RekombXY * Rnd()))
                    'Selektion der Schrittweite
                    AktPara(v).Dn = De(v, Realisierungsspeicher(R), PES_iAkt.iAktPop)
                    'Selektion des Elter
                    AktPara(v).Xn = Xe(v, Realisierungsspeicher(R), PES_iAkt.iAktPop)
                Next v


            Case PES_REPRODOP.XY_Average 'Multi-Rekombination nach X/Y-Schema, Mittelung der Gene

                'Realisierungsspeicher und Elternspeicher initialisieren
                'Anzahl der benötigten Eltern (Y)
                ReDim Realisierungsspeicher(mSettings.PES.N_RekombXY - 1)
                'Anzahl der Verfügbaren Eltern (n_Eltern)
                ReDim Elternspeicher(mSettings.PES.N_Eltern - 1)
                'Setzen der Eltern Indizes
                For i = 0 To (mSettings.PES.N_Eltern - 1)
                    Elternspeicher(i) = i
                Next
                'Auswahl der Y-Eltern
                For i = 0 To mSettings.PES.N_RekombXY - 1
                    '1. Runde erlaubt Auswahl aus allen Eltern
                    '2. Runde hat nur noch n_Eltern - 1 zur Verfügung
                    'usw.
                    'Kein Elter darf doppelt gezogen werden
                    If (mSettings.PES.Is_DiversityTournament) Then

                        R = CInt(Int((mSettings.PES.N_Eltern - i) * Rnd()))
                        TournamentElter1 = Elternspeicher(R)

                        Do
                            R = CInt(Int((mSettings.PES.N_Eltern - i) * Rnd()))
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
                        R = CInt(Int((mSettings.PES.N_Eltern - (i)) * Rnd()))
                    End If
                    'Kein Elter darf doppelt gezogen werden
                    Realisierungsspeicher(i) = Elternspeicher(R)
                    For j = R To mSettings.PES.N_Eltern - 2
                        Elternspeicher(j) = Elternspeicher(j + 1)
                    Next j
                Next i

                For v = 0 To Me.mProblem.NumOptParams - 1
                    AktPara(v).Dn = 0
                    AktPara(v).Xn = 0
                    For n = 0 To mSettings.PES.N_RekombXY - 1
                        'Mittelung der Schrittweite,
                        AktPara(v).Dn = AktPara(v).Dn + (De(v, Realisierungsspeicher(n), PES_iAkt.iAktPop) / mSettings.PES.N_RekombXY)
                        'Mittelung der Eltern,
                        AktPara(v).Xn = AktPara(v).Xn + (Xe(v, Realisierungsspeicher(n), PES_iAkt.iAktPop) / mSettings.PES.N_RekombXY)
                    Next
                Next v

            Case PES_REPRODOP.XY_Average_Discrete
                'Realisierungsspeicher und Elternspeicher initialisieren
                'Anzahl der benötigten Eltern (Y)
                ReDim Realisierungsspeicher(mSettings.PES.N_RekombXY - 1)
                'Anzahl der Verfügbaren Eltern (n_Eltern)
                ReDim Elternspeicher(mSettings.PES.N_Eltern - 1)
                'Setzen der Eltern Indizes
                For i = 0 To (mSettings.PES.N_Eltern - 1)
                    Elternspeicher(i) = i
                Next
                'Auswahl der Y-Eltern
                For i = 0 To mSettings.PES.N_RekombXY - 1
                    '1. Runde erlaubt Auswahl aus allen Eltern
                    '2. Runde hat nur noch n_Eltern - 1 zur Verfügung
                    'usw.
                    'Kein Elter darf doppelt gezogen werden
                    If (mSettings.PES.Is_DiversityTournament) Then

                        R = CInt(Int((mSettings.PES.N_Eltern - i) * Rnd()))
                        TournamentElter1 = Elternspeicher(R)

                        Do
                            R = CInt(Int((mSettings.PES.N_Eltern - i) * Rnd()))
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
                        R = CInt(Int((mSettings.PES.N_Eltern - (i)) * Rnd()))
                    End If
                    'Kein Elter darf doppelt gezogen werden
                    Realisierungsspeicher(i) = Elternspeicher(R)
                    For j = R To mSettings.PES.N_Eltern - 2
                        Elternspeicher(j) = Elternspeicher(j + 1)
                    Next j

                Next i
                For v = 0 To Me.mProblem.NumOptParams - 1
                    AktPara(v).Dn = 0
                    R = CInt(Int(mSettings.PES.N_RekombXY * Rnd()))
                    'Mittelung der Schrittweite
                    For n = 0 To mSettings.PES.N_RekombXY - 1
                        AktPara(v).Dn = AktPara(v).Dn + (De(v, Realisierungsspeicher(n), PES_iAkt.iAktPop) / mSettings.PES.N_RekombXY)
                    Next
                    'Selektion des Elter
                    AktPara(v).Xn = Xe(v, Realisierungsspeicher(R), PES_iAkt.iAktPop)
                Next v


            Case PES_REPRODOP.Neighborhood 'Neighbourhood Rekombination

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
                ReDim Elternspeicher(mSettings.PES.N_Eltern - 1)
                'Setzen der Eltern Indizes
                For i = 0 To (mSettings.PES.N_Eltern - 1)
                    Elternspeicher(i) = i
                Next
                If (mSettings.PES.Is_DiversityTournament) Then

                    R = CInt(Int((mSettings.PES.N_Eltern) * Rnd()))
                    TournamentElter1 = Elternspeicher(R)

                    Do
                        R = CInt(Int((mSettings.PES.N_Eltern) * Rnd()))
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
                    R = CInt(Int((mSettings.PES.N_Eltern) * Rnd()))
                End If

                Elter = R

                If (Elter = 0 Or Elter = mSettings.PES.N_Eltern - 1) Then
                    For v = 0 To Me.mProblem.NumOptParams - 1
                        'Selektion der Schrittweite
                        AktPara(v).Dn = De(v, Elter, PES_iAkt.iAktPop)
                        'Selektion des Elter
                        AktPara(v).Xn = Xe(v, Elter, PES_iAkt.iAktPop)
                    Next
                Else
                    '#68
                    Dim IndexEltern(mSettings.PES.N_Eltern - 1) As Integer          'Array mit Index der Eltern (Neighbourhood-Rekomb.)
                    Call Neighbourhood_Eltern(Elter, IndexEltern)
                    For v = 0 To Me.mProblem.NumOptParams - 1
                        'Do
                        '    Faktor = Rnd
                        '    Faktor = (-1) * Eigenschaft.d + Faktor * (1 + Eigenschaft.d)
                        '    'Selektion der Schrittweite
                        '    Eigenschaft.Dn(v) = De(v, IndexEltern(1), Eigenschaft.iaktuellePopulation) * Faktor + _
                        '                     De(v, IndexEltern(2), Eigenschaft.iaktuellePopulation) * (1 - Faktor)
                        '    Eigenschaft.Xn(v) = Xe(v, IndexEltern(1), Eigenschaft.iaktuellePopulation) * Faktor + _
                        '                     Xe(v, IndexEltern(2), Eigenschaft.iaktuellePopulation) * (1 - Faktor)
                        'Loop While (Eigenschaft.Xn(v) <= Eigenschaft.Xmin(v) Or Eigenschaft.Xn(v) > Eigenschaft.Xmax(v))

                        R = Int(mSettings.PES.N_RekombXY * Rnd())
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
        Dim expo As Integer           'Exponent für Schrittweite (+/-1)

        ReDim DeTemp(Me.mProblem.NumOptParams - 1, mSettings.PES.N_Eltern - 1, mSettings.PES.Pop.N_Popul - 1)
        ReDim XeTemp(Me.mProblem.NumOptParams - 1, mSettings.PES.N_Eltern - 1, mSettings.PES.Pop.N_Popul - 1)

        'Einheitliche Schrittweite
        '-------------------------
        If (Not mSettings.PES.SetMutation.IsDnVektor) Then
            '+/-1
            expo = (2 * Int(Rnd() + 0.5) - 1)
            'Schrittweite wird mutiert
            DeTemp(0, 0, PES_iAkt.iAktPop) = De(0, 0, PES_iAkt.iAktPop) * palpha ^ expo
            'Schrittweite für alle übernehmen
            For n = 0 To mSettings.PES.N_Eltern - 1
                For v = 0 To Me.mProblem.NumOptParams - 1
                    DeTemp(v, n, PES_iAkt.iAktPop) = DeTemp(0, 0, PES_iAkt.iAktPop)
                Next
            Next
        End If

        'Mutation
        '--------
        For n = 0 To mSettings.PES.N_Eltern - 1
            For v = 0 To Me.mProblem.NumOptParams - 1
                i = 0
                Do
                    i += 1
                    'Abbruchkriterium für abhängige Parameter
                    '----------------------------------------
                    If (i >= 1000) Then
                        'Es konnte kein gültiger Parametersatz generiert werden!
                        'Vermutlich ist die aktuelle Schrittweite nicht groß genug.
                        'Elterwert des aktuellen Parameters auf aktuellen Wert 
                        'des Parameters setzen, von dem der aktuelle Parameter abhängig ist
                        Xe(v, n, PES_iAkt.iAktPop) = XeTemp(v - 1, n, PES_iAkt.iAktPop)
                        i = 0
                    End If

                    'Schrittweitenvektor
                    '-------------------
                    If (mSettings.PES.SetMutation.IsDnVektor) Then
                        '+/-1
                        expo = (2 * Int(Rnd() + 0.5) - 1)
                        'Schrittweite wird mutiert
                        DeTemp(v, n, PES_iAkt.iAktPop) = De(v, n, PES_iAkt.iAktPop) * palpha ^ expo
                    End If

                    'Normalverteilte Zufallszahl mit Standardabweichung 1/sqr(varanz)
                    Dim Z As Double
                    Z = System.Math.Sqrt(-2 * System.Math.Log(1 - Rnd()) / Me.mProblem.NumOptParams) * System.Math.Sin(6.2832 * Rnd())
                    'Mutation wird durchgeführt
                    XeTemp(v, n, PES_iAkt.iAktPop) = Xe(v, n, PES_iAkt.iAktPop) + DeTemp(v, n, PES_iAkt.iAktPop) * Z

                    ' Restriktion für die mutierten Werte
                Loop While (XeTemp(v, n, PES_iAkt.iAktPop) < 0 Or XeTemp(v, n, PES_iAkt.iAktPop) > 1 Or Not checkBeziehungPop(v, n, XeTemp))

            Next v

            'Mutierte Werte übernehmen
            '-------------------------
            For v = 0 To Me.mProblem.NumOptParams - 1
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

        'Allgemeine Variablen
        Dim XnTemp() As Double             'Temporäre Parameterwerte für Nachkomme
        Dim DnTemp() As Double             'Temporäre Schrittweiten für Nachkomme
        Dim CTemp() As Double              'Temporäres Schiefemaß für Nachkomme
        Dim expo As Integer                'Exponent für Schrittweite (+/-1)

        ReDim XnTemp(Me.mProblem.NumOptParams - 1)
        ReDim DnTemp(Me.mProblem.NumOptParams - 1)
        ReDim CTemp(Me.mProblem.NumOptParams - 1)

        'Unterscheidung zwischen den Mutationsoperatoren
        '***********************************************
        Select Case mSettings.PES.Mutationsop
            Case PES_MUTATIONSOP.Rechenberg
                '*********************
                'Rechenberg Mutation *
                '*********************
                Dim Z As Double
                'Vorbereitung falls kein Vektor
                If Not mSettings.PES.SetMutation.IsDnVektor Then
                    '+/-1
                    expo = (2 * Int(Rnd() + 0.5) - 1)
                    'Schrittweite wird mutiert
                    DnTemp(0) = AktPara(0).Dn * galpha ^ expo
                End If

                'Über alle Parameter
                For v = 0 To Me.mProblem.NumOptParams - 1
                    i = 0
                    Do
                        i += 1
                        'Abbruchkriterium für abhängige Parameter
                        '----------------------------------------
                        If (i >= 1000) Then
                            'Es konnte kein gültiger Parametersatz generiert werden! Vermutlich ist die aktuelle Schrittweite nicht groß genug.
                            'Elterwert des aktuellen Parameters auf aktuellen Wert des Parameters setzen, von dem der aktuelle Parameter abhängig ist
                            i = 0
                            AktPara(v).Xn = XnTemp(v - 1)
                        End If

                        'Schrittweitenvektor oder nicht
                        '------------------------------
                        If (mSettings.PES.SetMutation.IsDnVektor) Then
                            '+/-1
                            expo = (2 * Int(Rnd() + 0.5) - 1)
                            'Schrittweite wird mutiert
                            DnTemp(v) = AktPara(v).Dn * galpha ^ expo
                        Else
                            DnTemp(v) = DnTemp(0)
                        End If

                        'Normalverteilte Zufallszahl mit Standardabweichung 1/sqr(varanz)
                        'Z = System.Math.Sqrt(-2 * System.Math.Log(1 - Rnd()) / Me.mProblem.NumParams) * System.Math.Sin(6.2832 * Rnd())
                        'Normalverteilte Zufallszahl mit Standardabweichung 1/sqr(var.anz), , Mittelwert 0
                        Z = Me.NormalDistributationRND(1 / Math.Sqrt(Me.mProblem.NumOptParams), 0.0)

                        'Mutation wird durchgeführt
                        '**************************
                        XnTemp(v) = AktPara(v).Xn + DnTemp(v) * Z
                        'Restriktion für die mutierten Werte
                    Loop While (XnTemp(v) < 0 Or XnTemp(v) > 1 Or Not checkBeziehung(v, XnTemp))
                Next v

            Case PES_MUTATIONSOP.Schwefel
                '*******************
                'SCHWEFEL Mutation *
                '*******************
                Dim Z As Double
                Dim tau As Double
                Dim taufix As Double
                Dim ZFix As Double
                'Vorbereitung falls kein Dn Vektor
                If Not mSettings.PES.SetMutation.IsDnVektor Then
                    tau = mSettings.PES.SetMutation.DnC / Math.Sqrt(Me.mProblem.NumOptParams)
                    'Normalverteilte Zufallszahl (SD = 1, mean = 0)
                    Z = Me.NormalDistributationRND(1.0, 0.0)
                    'Neue Schrittweite
                    DnTemp(0) = AktPara(0).Dn * Math.Exp(tau * Z)
                    'Mindestschrittweite muss eingehalten werden
                    If DnTemp(0) < mSettings.PES.SetMutation.DnEpsilon Then DnTemp(0) = mSettings.PES.SetMutation.DnEpsilon

                ElseIf mSettings.PES.SetMutation.IsDnVektor Then
                    'mit Vektor
                    taufix = mSettings.PES.SetMutation.DnC / Math.Sqrt(2 * Me.mProblem.NumOptParams)
                    ZFix = Me.NormalDistributationRND(1.0, 0.0)
                End If

                'Über alle Parameter
                '*******************
                For v = 0 To Me.mProblem.NumOptParams - 1
                    i = 0
                    Do
                        i += 1
                        'Abbruchkriterium für abhängige Parameter
                        '----------------------------------------
                        If (i >= 1000) Then
                            'Es konnte kein gültiger Parametersatz generiert werden! Vermutlich ist die aktuelle Schrittweite nicht groß genug.
                            'Elterwert des aktuellen Parameters auf aktuellen Wert des Parameters setzen, von dem der aktuelle Parameter abhängig ist
                            i = 0
                            AktPara(v).Xn = XnTemp(v - 1)
                        End If

                        'Schrittweitenvektor oder nicht
                        '------------------------------
                        If mSettings.PES.SetMutation.IsDnVektor Then
                            tau = mSettings.PES.SetMutation.DnC / Math.Sqrt(2 * Math.Sqrt(Me.mProblem.NumOptParams))
                            'Normalverteilte Zufallszahl (SD = 1, mean = 0)
                            Z = Me.NormalDistributationRND(1.0, 0.0)
                            'Neue Schrittweite
                            DnTemp(v) = AktPara(v).Dn * Math.Exp(taufix * ZFix + tau * Z)
                            'Mindestschrittweite muss eingehalten werden
                            If DnTemp(v) < mSettings.PES.SetMutation.DnEpsilon Then DnTemp(v) = mSettings.PES.SetMutation.DnEpsilon
                        Else
                            DnTemp(v) = DnTemp(0)
                        End If

                        'Normalverteilte Zufallszahl mit Standardabweichung 1/sqr(varanz)
                        'Z = System.Math.Sqrt(-2 * System.Math.Log(1 - Rnd()) / Me.mProblem.NumParams) * System.Math.Sin(6.2832 * Rnd())
                        'Normalverteilte Zufallszahl mit Standardabweichung 1, Mittelwert 0
                        Z = Me.NormalDistributationRND(1.0, 0.0)

                        'Mutation wird durchgeführt
                        '**************************
                        XnTemp(v) = AktPara(v).Xn + DnTemp(v) * Z
                        'Restriktion für die mutierten Werte
                    Loop While (XnTemp(v) < 0 Or XnTemp(v) > 1 Or Not checkBeziehung(v, XnTemp))
                Next v

        End Select


        'Mutierte Werte übernehmen
        '-------------------------
        For v = 0 To Me.mProblem.NumOptParams - 1
            AktPara(v).Dn = DnTemp(v)
            AktPara(v).Xn = XnTemp(v)
        Next v

    End Sub

    'normalverteilte Zufallszahl für Schwefelmutation
    '************************************************
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
        For m = 1 To mSettings.PES.Pop.N_Popul - 1
            If (mSettings.PES.OptModus = EVO_MODE.Single_Objective) Then
                If Qbpop(m, 0) > h1 Then
                    h1 = Qbpop(m, 0)
                    i = m
                End If
            Else
                Select Case mSettings.PES.Pop.PopPenalty

                    Case EVO_POP_PENALTY.Crowding
                        If Qbpop(m, 0) > h1 Then
                            h1 = Qbpop(m, 0)
                            i = m
                        End If

                    Case EVO_POP_PENALTY.Span
                        If Qbpop(m, 0) < h1 Then
                            h2 = Qbpop(m, 1)
                            i = m
                        End If
                End Select
            End If
        Next m

        'Der schlechtetste der besten Qualitätswerte wird bestimmt ; Position -> i
        '(höchster Wert der Kostenfunktion, niedrigster Wert der Spannweite)
        If (mSettings.PES.OptModus = EVO_MODE.Multi_Objective) Then
            j = 0
            h2 = Qbpop(0, 1)
            For m = 2 To mSettings.PES.Pop.N_Popul
                If Qbpop(m, 1) < h2 Then
                    h2 = Qbpop(m, 1)
                    j = m
                End If
            Next m
        End If

        'Qualität der aktuellen Population wird bestimmt
        h1 = 0
        If (mSettings.PES.OptModus = EVO_MODE.Single_Objective) Then
            For m = 0 To mSettings.PES.N_Eltern - 1
                h1 = h1 + Best.Qb(m, PES_iAkt.iAktPop, 0) / mSettings.PES.N_Eltern
            Next m
        Else
            h2 = 0
            h1 = NDS_Crowding_Distance_Count(h2)
        End If

        'Falls die Qualität des aktuellen Population besser ist (Penaltyfunktion geringer)
        'als die schlechteste im Bestwertspeicher, wird diese ersetzt
        If (mSettings.PES.OptModus = EVO_MODE.Single_Objective) Then
            If h1 < Qbpop(i, 0) Then
                Qbpop(i, 0) = h1
                For m = 0 To Me.mProblem.NumOptParams - 1
                    For n = 0 To mSettings.PES.N_Eltern - 1
                        'Die Schrittweite wird ebenfalls übernommen
                        Dbpop(m, n, i) = Best.Db(m, n, PES_iAkt.iAktPop)
                        'Die eigentlichen Parameterwerte werden übernommen
                        Xbpop(m, n, i) = Best.Xb(m, n, PES_iAkt.iAktPop)
                    Next n
                Next m
            End If
        Else
            Select Case mSettings.PES.Pop.PopPenalty

                Case EVO_POP_PENALTY.Crowding
                    If h1 < Qbpop(i, 0) Then
                        Qbpop(i, 0) = h1
                        For m = 0 To Me.mProblem.NumOptParams - 1
                            For n = 0 To mSettings.PES.N_Eltern - 1
                                'Die Schrittweite wird ebenfalls übernommen
                                Dbpop(m, n, i) = Best.Db(m, n, PES_iAkt.iAktPop)
                                'Die eigentlichen Parameterwerte werden übernommen
                                Xbpop(m, n, i) = Best.Xb(m, n, PES_iAkt.iAktPop)
                            Next n
                        Next m
                    End If

                Case EVO_POP_PENALTY.Span
                    If h2 > Qbpop(j, 1) Then
                        Qbpop(j, 1) = h2
                        For m = 0 To Me.mProblem.NumOptParams - 1
                            For n = 0 To mSettings.PES.N_Eltern - 1
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
    Public Sub EsBest(ByVal Ind As Individuum)

        Dim m, j, v As Integer
        Dim h As Double

        If (mSettings.PES.OptModus = EVO_MODE.Single_Objective) Then
            'SO - Standard ES nach Rechenberg
            '--------------------------------
            'Der schlechteste der besten Qualitätswerte wird bestimmt ; Position -> j
            '(höchster Wert der Penaltyfunktion)
            j = 0
            h = Best.Qb(0, PES_iAkt.iAktPop, 0)

            For m = 1 To mSettings.PES.N_Eltern - 1
                If Best.Qb(m, PES_iAkt.iAktPop, 0) > h Then
                    h = Best.Qb(m, PES_iAkt.iAktPop, 0)
                    j = m
                End If
            Next m

            'Falls die Qualität des aktuellen Nachkommen besser ist (Penaltyfunktion geringer)
            'als die schlechteste im Bestwertspeicher, wird dieser ersetzt
            If Ind.PrimObjectives(0) < Best.Qb(j, PES_iAkt.iAktPop, 0) Then
                Best.Qb(j, PES_iAkt.iAktPop, 0) = Ind.PrimObjectives(0)
                For v = 0 To Me.mProblem.NumOptParams - 1
                    'Die Schrittweite wird ebenfalls übernommen
                    Best.Db(v, j, PES_iAkt.iAktPop) = Ind.OptParameter(v).Dn
                    'Die eigentlichen Parameterwerte werden übernommen
                    Best.Xb(v, j, PES_iAkt.iAktPop) = Ind.OptParameter(v).Xn
                Next v
            End If

        Else
            'Multi-Objective Pareto
            '----------------------
            NDSorting(PES_iAkt.iAktNachf) = Ind             'Quasi ByReferenz!?
            NDSorting(PES_iAkt.iAktNachf).Distance = 0
            NDSorting(PES_iAkt.iAktNachf).Dominated = False
            NDSorting(PES_iAkt.iAktNachf).Front = 0
        End If

    End Sub

    'ES_BESTWERTSPEICHER
    'Führt einen Reset des Bestwertspeicher durch, falls eine Komma-Strategie gewählt ist
    '************************************************************************************
    Public Sub EsResetBWSpeicher()
        Dim n, i As Integer

        If (mSettings.PES.Strategie = EVO_STRATEGY.Comma_Strategy) Then
            For n = 0 To mSettings.PES.N_Eltern - 1
                For i = 0 To Me.mProblem.NumPrimObjective - 1
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

        If (mSettings.PES.Pop.PopStrategie = EVO_STRATEGY.Comma_Strategy) Then
            For n = 0 To mSettings.PES.Pop.N_Popul - 1
                For i = 0 To Me.mProblem.NumPrimObjective - 1
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

        Select Case mSettings.PES.Pop.PopPenalty
            Case EVO_POP_PENALTY.Crowding
                Z = 0
            Case EVO_POP_PENALTY.Span
                Z = 1
        End Select

        ReDim Realisierungsspeicher(mSettings.PES.Pop.N_Popul - 1, 1)

        'Die NPopEltern besten Individium-Sätze werden ermittelt
        For m = 0 To mSettings.PES.Pop.N_Popul - 1
            Realisierungsspeicher(m, 0) = Qbpop(m, Z)
            Realisierungsspeicher(m, 1) = m
        Next m

        If (mSettings.PES.OptModus = EVO_MODE.Single_Objective) Then
            'Standard ES nach Rechenberg
            '---------------------------
            For m = 0 To mSettings.PES.Pop.N_Popul - 1
                For n = m To mSettings.PES.Pop.N_Popul - 1
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
            Select Case mSettings.PES.Pop.PopPenalty

                Case EVO_POP_PENALTY.Crowding
                    For m = 0 To mSettings.PES.Pop.N_Popul - 1
                        For n = m To mSettings.PES.Pop.N_Popul - 1
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

                Case EVO_POP_PENALTY.Span
                    For m = 0 To mSettings.PES.Pop.N_Popul - 1
                        For n = m To mSettings.PES.Pop.N_Popul - 1
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
        For m = 0 To mSettings.PES.Pop.N_PopEltern - 1
            For n = 0 To mSettings.PES.N_Eltern - 1
                For v = 0 To Me.mProblem.NumOptParams - 1
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


        If (mSettings.PES.OptModus = EVO_MODE.Single_Objective) Then
            'Standard ES nach Rechenberg
            'xxxxxxxxxxxxxxxxxxxxxxxxxxx
            'Die Eltern werden gleich der besten Kinder gesetzt (Schrittweite und Parameterwert)
            '---------------------------------------------------------------------
            For i = 0 To mSettings.PES.N_Eltern - 1
                For v = 0 To Me.mProblem.NumOptParams - 1
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
            For i = mSettings.PES.N_Nachf To mSettings.PES.N_Nachf + mSettings.PES.N_Eltern - 1
                Call Copy_Bestwert_to_Individuum(i, i - mSettings.PES.N_Nachf, INDSorting)
            Next i

            '********************* Alles in der Klasse Functions ****************************************
            'Zu Beginn den Bestwertspeicher in ein Array von Individuen packen
            Dim Best_Indi() As Individuum
            Best_Indi = Individuum.New_Indi_Array(Individuum.Individuumsklassen.Individuum_PES, Best.Qb.GetLength(0), "Bestwerte")
            'Kopieren in Best_Indi
            For i = 0 To Best.Qb.GetUpperBound(0)
                Call Copy_Bestwert_to_Individuum(i, i, Best_Indi)
            Next
            '---------------------------------
            '2. Die einzelnen Fronten werden bestimmt
            '3. Der Bestwertspeicher wird entsprechend der Fronten oder der sekundären Population gefüllt
            '4: Sekundäre Population wird bestimmt und gespeichert
            '--------------------------------
            Dim Func1 As ES.Functions = New ES.Functions(Me.mProblem, Me.mSettings.PES.N_Nachf, Me.mSettings.PES.N_Eltern, Me.mSettings.PES.SekPop.Is_Begrenzung, Me.mSettings.PES.SekPop.N_MaxMembers, Me.mSettings.PES.SekPop.N_Interact, Me.mSettings.PES.SekPop.Is_Interact, PES_iAkt.iAktGen)
            Call Func1.EsEltern_Pareto(NDSorting, SekundärQb, Best_Indi)
            'Bestimmen der Crowding Distance falls Diversity-Tournament
            '----------------------------------------------------------
            If (mSettings.PES.Is_DiversityTournament) Then
                Call Func1.Pareto_Crowding_Distance(Best_Indi)
            End If

            'Am ende die Bestwerte wieder zurück
            For i = 0 To Best.Qb.GetUpperBound(0)
                Call Copy_Individuum_to_Bestwert(i, Best_Indi)
            Next
            '********************************************************************************************

            '5: Neue Eltern werden gleich dem Bestwertspeicher gesetzt
            '---------------------------------------------------------
            For i = 0 To mSettings.PES.N_Eltern - 1
                For v = 0 To Me.mProblem.NumOptParams - 1
                    De(v, i, PES_iAkt.iAktPop) = Best.Db(v, i, PES_iAkt.iAktPop)
                    Xe(v, i, PES_iAkt.iAktPop) = Best.Xb(v, i, PES_iAkt.iAktPop)
                Next v
                If (mSettings.PES.Is_DiversityTournament) Then
                    Div(i, PES_iAkt.iAktPop) = Best.Div(i, PES_iAkt.iAktPop)
                    Front(i, PES_iAkt.iAktPop) = Best.Front(i, PES_iAkt.iAktPop)
                End If
            Next i

            '6: Sortierung der Lösungen ist nur für Neighbourhood-Rekombination notwendig
            '----------------------------------------------------------------------------
            If (mSettings.PES.Reproduktionsop = PES_REPRODOP.Neighborhood) Then
                Call Neighbourhood_AbstandsArray()
                Call Neighbourhood_Crowding_Distance()
            End If


        End If

    End Sub

    'Kopiert ein Individuum in den Bestwertspeicher
    '----------------------------------------------
    Public Sub Copy_Individuum_to_Bestwert(ByVal i As Integer, ByVal Individ() As Individuum)
        Dim j, v As Integer

        For j = 0 To Me.mProblem.NumPrimObjective - 1
            Best.Qb(i, PES_iAkt.iAktPop, j) = Individ(i).PrimObjectives(j)
        Next j

        If (Me.mProblem.NumConstraints > 0) Then
            For j = 0 To Me.mProblem.NumConstraints - 1
                Best.Rb(i, PES_iAkt.iAktPop, j) = Individ(i).Constraints(j)
            Next j
        End If

        For v = 0 To Me.mProblem.NumOptParams - 1
            With CType(Individ(i), Individuum_PES)
                Best.Db(v, i, PES_iAkt.iAktPop) = .OptParameter(v).Dn
                Best.Xb(v, i, PES_iAkt.iAktPop) = .OptParameter(v).Xn
            End With
        Next v
        If (Me.mSettings.PES.Is_DiversityTournament) Then
            Best.Div(i, PES_iAkt.iAktPop) = Individ(i).Distance
            Best.Front(i, PES_iAkt.iAktPop) = Individ(i).Front
        End If
    End Sub

    'Kopiert den Bestwertspeicher in ein Individuum
    '----------------------------------------------
    Public Sub Copy_Bestwert_to_Individuum(ByVal i_indi As Integer, ByVal i_best As Integer, ByRef Individ() As Individuum)

        Dim i, j, v As Integer

        j = 0
        For i = 0 To Me.mProblem.NumObjectives - 1
            'HACK: Nur Penalties werden kopiert!
            If (Me.mProblem.List_ObjectiveFunctions(i).isPrimObjective) Then
                Individ(i_indi).Objectives(i) = Best.Qb(i_best, PES_iAkt.iAktPop, j)
                j += 1
            End If
        Next i

        If (Me.mProblem.NumConstraints > 0) Then
            For j = 0 To Me.mProblem.NumConstraints - 1
                Individ(i_indi).Constraints(j) = Best.Rb(i_best, PES_iAkt.iAktPop, j)
            Next j
        End If

        For v = 0 To Me.mProblem.NumOptParams - 1
            With CType(Individ(i_indi), Individuum_PES)
                .OptParameter(v).Dn = Best.Db(v, i_best, PES_iAkt.iAktPop)
                .OptParameter(v).Xn = Best.Xb(v, i_best, PES_iAkt.iAktPop)
            End With
        Next v

        Individ(i_indi).Dominated = False
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

        ReDim TempDistance(Me.mProblem.NumPrimObjective - 1)
        ReDim PenaltyDistance(mSettings.PES.N_Eltern - 1, mSettings.PES.N_Eltern - 1)
        ReDim d(mSettings.PES.N_Eltern - 1)

        'Bestimmen der normierten Raumabstände zwischen allen Elternindividuen
        For i = 0 To mSettings.PES.N_Eltern - 1
            PenaltyDistance(i, i) = 0
            For j = i + 1 To mSettings.PES.N_Eltern - 1
                PenaltyDistance(i, j) = 0
                For k = 0 To Me.mProblem.NumPrimObjective - 1
                    TempDistance(k) = Best.Qb(i, PES_iAkt.iAktPop, k) - Best.Qb(j, PES_iAkt.iAktPop, k)
                    TempDistance(k) = TempDistance(k) * TempDistance(k)
                    PenaltyDistance(i, j) += TempDistance(k)
                Next k
                PenaltyDistance(i, j) = System.Math.Sqrt(PenaltyDistance(i, j))
                PenaltyDistance(j, i) = PenaltyDistance(i, j)
            Next j
        Next i

        d_mean = 0

        For i = 0 To mSettings.PES.N_Eltern - 2
            d(i) = 1.0E+300
            For j = 0 To i - 1
                If (PenaltyDistance(i, j) < d(i)) Then d(i) = PenaltyDistance(i, j)
            Next j
            For j = i + 1 To mSettings.PES.N_Eltern - 1
                If (PenaltyDistance(i, j) < d(i)) Then d(i) = PenaltyDistance(i, j)
            Next j
            d_mean += d(i)
        Next i

        d_mean = d_mean / mSettings.PES.N_Eltern
        NDS_Crowding_Distance_Count = 0

        For i = 0 To mSettings.PES.N_Eltern - 2
            NDS_Crowding_Distance_Count += (d_mean - d(i)) * (d_mean - d(i))
        Next i

        NDS_Crowding_Distance_Count = NDS_Crowding_Distance_Count / mSettings.PES.N_Eltern
        NDS_Crowding_Distance_Count = System.Math.Sqrt(NDS_Crowding_Distance_Count)

        Spannweite = 0
        For i = 0 To mSettings.PES.N_Eltern - 1
            'TODO: sollte hier nicht j = i + 1 stehen?
            For j = i To mSettings.PES.N_Eltern - 1
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
        ReDim MinMax(Me.mProblem.NumPrimObjective - 1)
        For k = 0 To Me.mProblem.NumPrimObjective - 1
            MinMax(k) = 0
            Min = Best.Qb(0, PES_iAkt.iAktPop, k)
            Max = Best.Qb(0, PES_iAkt.iAktPop, k)
            For j = 0 To mSettings.PES.N_Eltern - 1
                If (Min > Best.Qb(j, PES_iAkt.iAktPop, k)) Then Min = Best.Qb(j, PES_iAkt.iAktPop, k)
                If (Max < Best.Qb(j, PES_iAkt.iAktPop, k)) Then Max = Best.Qb(j, PES_iAkt.iAktPop, k)
            Next j
            MinMax(k) = Max - Min
        Next k

        'Bestimmen der normierten Raumabstände zwischen allen Elternindividuen
        ReDim TempDistance(Me.mProblem.NumPrimObjective)

        For i = 0 To mSettings.PES.N_Eltern - 1
            PenaltyDistance(i, i) = 0
            For j = i + 1 To mSettings.PES.N_Eltern - 1
                PenaltyDistance(i, j) = 0
                For k = 0 To Me.mProblem.NumPrimObjective - 1
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

        '#68 ganze Funktion
        ReDim Nachbarn(mSettings.PES.N_Eltern - 2)

        For i = 0 To IndexElter - 2
            Nachbarn(i).distance = PenaltyDistance(IndexElter, i)
            Nachbarn(i).Index = i
        Next i
        For i = IndexElter To mSettings.PES.N_Eltern - 1
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

        For i = 0 To mSettings.PES.N_RekombXY - 1
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

        ReDim QbTemp(mSettings.PES.N_Eltern - 1, mSettings.PES.Pop.N_Popul - 1, Me.mProblem.NumPrimObjective - 1)

        Array.Copy(Best.Qb, QbTemp, Best.Qb.GetLength(0))
        For i = 0 To mSettings.PES.N_Eltern - 1
            Distanceb(i) = 0
        Next i

        For k = 0 To Me.mProblem.NumPrimObjective - 1
            For i = 0 To mSettings.PES.N_Eltern - 1
                For j = 0 To mSettings.PES.N_Eltern - 1
                    If (QbTemp(i, PES_iAkt.iAktPop, k) < QbTemp(j, PES_iAkt.iAktPop, k)) Then
                        swap = QbTemp(i, PES_iAkt.iAktPop, k)
                        QbTemp(i, PES_iAkt.iAktPop, k) = QbTemp(j, PES_iAkt.iAktPop, k)
                        QbTemp(j, PES_iAkt.iAktPop, k) = swap
                    End If
                Next j
            Next i

            fmin = QbTemp(0, PES_iAkt.iAktPop, k)
            fmax = QbTemp(mSettings.PES.N_Eltern - 1, PES_iAkt.iAktPop, k)

            Distanceb(0) = 1.0E+300
            Distanceb(mSettings.PES.N_Eltern - 1) = 1.0E+300

            For i = 1 To mSettings.PES.N_Eltern - 2
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
        If (AktPara(ipara).Beziehung = Relationship.none) Then
            'Keine Beziehung vorhanden
            isOK = True
        Else
            'Referenzierten Parameterwert vergleichen (reelle Werte!)
            Dim wert As Double = AktPara(ipara).Min + (AktPara(ipara).Max - AktPara(ipara).Min) * XnTemp(ipara)
            Dim ref As Double = AktPara(ipara - 1).Min + (AktPara(ipara - 1).Max - AktPara(ipara - 1).Min) * XnTemp(ipara - 1)
            Select Case AktPara(ipara).Beziehung
                Case Relationship.smaller_than
                    If (wert < ref) Then isOK = True
                Case Relationship.smaller_equal
                    If (wert <= ref) Then isOK = True
                Case Relationship.larger_than
                    If (wert > ref) Then isOK = True
                Case Relationship.larger_equal
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
        If (AktPara(ipara).Beziehung = Relationship.none) Then
            'Keine Beziehung vorhanden
            isOK = True
        Else
            'Referenzierten Parameterwert vergleichen
            Dim wert As Double = AktPara(ipara).Min + (AktPara(ipara).Max - AktPara(ipara).Min) * XeTemp(ipara, iElter, PES_iAkt.iAktPop)
            Dim ref As Double = AktPara(ipara - 1).Min + (AktPara(ipara - 1).Max - AktPara(ipara - 1).Min) * XeTemp(ipara - 1, iElter, PES_iAkt.iAktPop)
            Select Case AktPara(ipara).Beziehung
                Case Relationship.smaller_than
                    If (wert < ref) Then isOK = True
                Case Relationship.smaller_equal
                    If (wert <= ref) Then isOK = True
                Case Relationship.larger_than
                    If (wert > ref) Then isOK = True
                Case Relationship.larger_equal
                    If (wert >= ref) Then isOK = True
            End Select
        End If

        Return isOK

    End Function

End Class
