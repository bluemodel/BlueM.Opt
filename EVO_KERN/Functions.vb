Public Class Functions

    'Die Statische Variablen werden im Konstruktor übergeben
    '*******************************************************
    Dim NNachf As Integer
    Dim NEltern As Integer
    Dim NMemberSecondPop As Integer
    Dim NInteract As Integer
    Dim isInteract As Boolean
    Dim NPenalty As Integer
    Dim NConstr As Integer
    Dim iAktGen As Integer
    Dim iAktPop As Integer

    'Die Veränderliochen Structs werden in der Funktion übergeben
    '************************************************************
    Dim NDSorting() As Individuum
    Dim SekundärQb() As Individuum

    'Bestwerte werden zurückgeben
    Dim Best() As Individuum
    '****************************

    'Die Statische Variablen werden im Konstruktor übergeben
    '*******************************************************
    Public Sub New(ByVal _NNachf As Integer, ByVal _NEltern As Integer, ByVal _NMemberSecondPop As Integer, ByVal _NInteract As Integer, ByVal _isInteract As Boolean, ByVal _NPenalty As Integer, ByVal _NConstr As Integer, ByVal _iAktGen As Integer)

        NNachf = _NNachf
        NEltern = _NEltern
        NMemberSecondPop = _NMemberSecondPop
        NInteract = _NInteract
        isInteract = _isInteract
        NPenalty = _NPenalty
        NConstr = _NConstr
        iAktGen = _iAktGen

    End Sub

    'Dieser Teil beschäftigt sich nur mit SekundärQb und NDSorting
    '2. Die einzelnen Fronten werden bestimmt
    '3. Der Bestwertspeicher wird entsprechend der Fronten oder der sekundären Population gefüllt
    '4: Sekundäre Population wird bestimmt und gespeichert
    '--------------------------------------------------------------------------------------------
    Public Sub EsEltern_Pareto_SekundärQb(ByRef _Best() As Individuum, ByRef _NDSorting() As Individuum, ByRef _SekundärQb() As Individuum)

        'Veränderliche (ByRef):
        NDSorting = _NDSorting
        SekundärQb = _SekundärQb
        Best = _Best

        Dim i As Integer
        Dim NFrontMember_aktuell As Integer
        Dim NFrontMember_gesamt As Integer
        Dim rang As Integer
        Dim aktuelle_Front As Integer
        Dim Temp() As Individuum
        Dim NDSResult() As Individuum

        '2. Die einzelnen Fronten werden bestimmt
        '----------------------------------------
        rang = 1
        NFrontMember_gesamt = 0

        'Initialisierung von Temp (NDSorting)
        ReDim Temp(NNachf + NEltern - 1)

        'Initialisierung von NDSResult (NDSorting)
        ReDim NDSResult(NNachf + NEltern - 1)

        'NDSorting wird in Temp kopiert
        Call Individuum.Copy_Array(NDSorting, Temp)

        'Schleife läuft über die Zahl der Fronten die hier auch bestimmt werden
        Do
            'Entscheidet welche Werte dominiert werden und welche nicht
            Call Pareto_SekundärQb_Non_Dominated_Sorting(Temp, rang) 'aktualisiert auf n Objectives dm 10.05.05
            'Sortiert die nicht dominanten Lösungen nach oben,
            'die dominanten nach unten und zählt die Mitglieder der aktuellen Front
            NFrontMember_aktuell = Pareto_Non_Dominated_Count_and_Sort(Temp)
            'NFrontMember_aktuell: Anzahl der Mitglieder der gerade bestimmten Front
            'NFrontMember_gesamt: Alle bisher als nicht dominiert klassifizierten Individuum
            NFrontMember_gesamt += NFrontMember_aktuell
            'Hier wird pro durchlauf die nicht dominierte Front in NDSResult geschaufelt
            'und die bereits klassifizierten Lösungen aus Temp Array gelöscht
            Call Pareto_Non_Dominated_Result(Temp, NDSResult, NFrontMember_aktuell, NFrontMember_gesamt)
            'Rang ist hier die Nummer der Front
            rang += 1
        Loop While Not (NFrontMember_gesamt = NEltern + NNachf)

        '3. Der Bestwertspeicher wird entsprechend der Fronten oder der sekundären Population gefüllt
        '--------------------------------------------------------------------------------------------
        NFrontMember_aktuell = 0
        NFrontMember_gesamt = 0
        aktuelle_Front = 0

        Do
            NFrontMember_aktuell = Pareto_Count_Front_Members(aktuelle_Front, NDSResult)

            'Es sind mehr Elterplätze für die nächste Generation verfügaber
            '-> schiss wird einfach rüberkopiert
            If NFrontMember_aktuell <= NEltern - NFrontMember_gesamt Then
                For i = NFrontMember_gesamt To NFrontMember_aktuell + NFrontMember_gesamt - 1

                    'NDSResult wird in den Bestwertspeicher kopiert
                    Best(i) = NDSResult(i).Copy

                Next i
                NFrontMember_gesamt = NFrontMember_gesamt + NFrontMember_aktuell

            Else
                'Es sind weniger Elterplätze für die nächste Generation verfügber
                'als Mitglieder der aktuellen Front. Nur für diesen Rest wird crowding distance
                'gemacht um zu bestimmen wer noch mitspielen darf und wer noch a biserl was druff hat
                Call Pareto_Crowding_Distance_Sort(NDSResult, NFrontMember_gesamt, NFrontMember_gesamt + NFrontMember_aktuell - 1)

                For i = NFrontMember_gesamt To NEltern - 1
                    'NDSResult wird in den Bestwertspeicher kopiert
                    Best(i) = NDSResult(i).Copy
                Next i

                NFrontMember_gesamt = NEltern

            End If

            aktuelle_Front += 1

        Loop While Not (NFrontMember_gesamt = NEltern)

        '4: Sekundäre Population wird bestimmt und gespeichert
        '-----------------------------------------------------
        SekundärQb_Allocation(NFrontMember_aktuell, NDSResult)

        'Veränderliche (ByRef):
        _NDSorting = NDSorting
        _SekundärQb = SekundärQb
        _Best = Best
    End Sub

    '4: Sekundäre Population wird bestimmt und gespeichert ggf gespeichert
    '---------------------------------------------------------------------
    Private Sub SekundärQb_Allocation(ByVal NFrontMember_aktuell As Integer, ByVal _NDSResult As Individuum())

        Dim i As Integer
        Dim Member_Sekundärefront As Integer

        NFrontMember_aktuell = Pareto_Count_Front_Members(1, _NDSResult)

        'Am Anfang wird SekundärQb mit -1 initialisiert
        Member_Sekundärefront = SekundärQb.GetLength(0)

        'SekPop wird um die aktuelle Front erweitert
        ReDim Preserve SekundärQb(Member_Sekundärefront + NFrontMember_aktuell - 1)

        'Neue Member der SekPop bestimmen
        For i = Member_Sekundärefront To Member_Sekundärefront + NFrontMember_aktuell - 1
            SekundärQb(i) = _NDSResult(i - Member_Sekundärefront)
        Next i

        Call Pareto_SekundärQb_Non_Dominated_Sorting(SekundärQb, 1)

        NFrontMember_aktuell = SekundärQb_Non_Dominated_Count_and_Sort(SekundärQb)
        ReDim Preserve SekundärQb(NFrontMember_aktuell - 1)

        'Dubletten werden gelöscht
        Call SekundärQb_Dubletten()
        NFrontMember_aktuell = SekundärQb_Non_Dominated_Count_and_Sort(SekundärQb)
        ReDim Preserve SekundärQb(NFrontMember_aktuell - 1)

        'Crowding Distance
        If (SekundärQb.GetUpperBound(0) > NMemberSecondPop - 1) Then
            Call Pareto_Crowding_Distance_Sort(SekundärQb, 0, SekundärQb.GetUpperBound(0))
            ReDim Preserve SekundärQb(NMemberSecondPop - 1)
        End If

        'Prüfen, ob die Population jetzt mit Mitgliedern aus der Sekundären Population aufgefüllt werden soll
        '----------------------------------------------------------------------------------------------------
        If (iAktGen Mod NInteract) = 0 And isInteract Then
            NFrontMember_aktuell = Pareto_Count_Front_Members(1, SekundärQb)
            If NFrontMember_aktuell > NEltern Then
                'Crowding Distance
                Call Pareto_Crowding_Distance_Sort(SekundärQb, 0, SekundärQb.GetUpperBound(0))
                For i = 0 To NEltern - 1
                    'SekundärQb wird in den Bestwertspeicher kopiert
                    Best(i) = SekundärQb(i).Copy
                Next i
            End If
        End If
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
                    Logical = Logical And (SekundärQb(i).Penalty(k) = SekundärQb(j).Penalty(k))
                Next k
                If (Logical) Then SekundärQb(i).dominated = True
            Next j
        Next i
    End Sub

    'NON_DOMINATED_COUNT_AND_SORT_SEKUNDÄRE_POPULATION
    'Sortiert die nicht dominanten Lösungen nach oben, die dominanten nach unten
    'Gibt die Zahl der dominanten Lösungen zurück (Front) hier für die Sekundäre Population
    '**************************************************************************************
    Private Function SekundärQb_Non_Dominated_Count_and_Sort(ByRef _SekundärQb() As Individuum) As Integer

        Dim i As Integer
        Dim Temp() As Individuum
        Dim counter As Integer
        Dim NFrontMember As Integer

        ReDim Temp(_SekundärQb.GetUpperBound(0))

        NFrontMember = 0
        counter = 0

        For i = 0 To _SekundärQb.GetUpperBound(0)
            If (_SekundärQb(i).dominated = False) Then
                counter += 1
                Temp(counter - 1) = _SekundärQb(i).Copy
            End If
        Next i

        NFrontMember = counter

        For i = 0 To _SekundärQb.GetUpperBound(0)
            If (_SekundärQb(i).dominated = True) Then
                counter += 1
                Temp(counter - 1) = _SekundärQb(i).Copy
            End If
        Next i

        Call Individuum.Copy_Array(Temp, _SekundärQb)

        Return NFrontMember

    End Function

    'NON_DOMINATED_SORTING - Entscheidet welche Werte dominiert werden und welche nicht
    '**********************************************************************************
    Private Sub Pareto_SekundärQb_Non_Dominated_Sorting(ByRef NDSorting() As Individuum, ByVal rang As Integer)

        Dim j, i, k As Integer
        Dim isDominated As Boolean
        Dim Summe_Constrain(1) As Double

        If (NConstr > 0) Then
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

                        For k = 0 To NConstr - 1
                            If (NDSorting(i).Constrain(k) < 0) Then
                                Summe_Constrain(0) += NDSorting(i).Constrain(k)
                            End If
                            If (NDSorting(j).Constrain(k) < 0) Then
                                Summe_Constrain(1) += NDSorting(j).Constrain(k)
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
                            isDominated = isDominated Or (NDSorting(i).Penalty(k) < NDSorting(j).Penalty(k))
                        Next k

                        For k = 0 To NPenalty - 1
                            isDominated = isDominated And (NDSorting(i).Penalty(k) <= NDSorting(j).Penalty(k))
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
                        isDominated = isDominated Or (NDSorting(i).Penalty(k) < NDSorting(j).Penalty(k))
                    Next k

                    For k = 0 To NPenalty - 1
                        isDominated = isDominated And (NDSorting(i).Penalty(k) <= NDSorting(j).Penalty(k))
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
    Private Function Pareto_Non_Dominated_Count_and_Sort(ByRef NDSorting() As Individuum) As Integer

        Dim i As Integer
        Dim Temp() As Individuum
        Dim counter As Integer
        Dim NFrontMember As Integer

        ReDim Temp(NDSorting.GetUpperBound(0))

        NFrontMember = 0
        counter = 0

        'Die nicht dominanten Lösungen werden nach oben kopiert
        For i = 0 To NDSorting.GetUpperBound(0)
            If (NDSorting(i).dominated = True) Then
                counter += 1
                Temp(counter - 1) = NDSorting(i).Copy
            End If
        Next i

        'Zahl der dominanten Lösungen wird errechnet
        NFrontMember = NDSorting.GetUpperBound(0) + 1 - counter

        'Die dominanten Lösungen werden nach unten kopiert
        For i = 0 To NDSorting.GetUpperBound(0)
            If (NDSorting(i).dominated = False) Then
                counter += 1
                Temp(counter - 1) = NDSorting(i).Copy
            End If
        Next i

        Call Individuum.Copy_Array(Temp, NDSorting)

        Return NFrontMember

    End Function

    'NON_DOMINATED_RESULT - Hier wird pro durchlauf die nicht dominierte Front in NDSResult
    'geschaufelt und die bereits klassifizierten Lösungen aus Temp Array gelöscht
    '**************************************************************************************
    Private Sub Pareto_Non_Dominated_Result(ByRef Temp() As Individuum, ByRef NDSResult() As Individuum, ByVal NFrontMember_aktuell As Integer, ByVal NFrontMember_gesamt As Integer)

        Dim i, Position As Integer

        Position = NFrontMember_gesamt - NFrontMember_aktuell

        'In NDSResult werden die nicht dominierten Lösungen eingefügt
        For i = Temp.GetLength(0) - NFrontMember_aktuell To Temp.GetUpperBound(0)
            'NDSResult alle bisher gefundene Fronten
            NDSResult(Position) = Temp(i).Copy
            Position += 1
        Next i

        'Die bereits klassifizierten Member werden aus dem Temp Array gelöscht
        If (NNachf + NEltern - NFrontMember_gesamt > 0) Then
            ReDim Preserve Temp(NNachf + NEltern - NFrontMember_gesamt - 1)
            'Der Flag wird zur klassifizierung in der nächsten Runde zurückgesetzt
            For i = 0 To Temp.GetUpperBound(0)
                Temp(i).dominated = False
            Next i
        End If

    End Sub

    'COUNT_FRONT_MEMBERS
    '*******************
    Private Function Pareto_Count_Front_Members(ByVal aktuell_Front As Integer, ByVal _Individ() As Individuum) As Integer

        Dim i As Integer
        Pareto_Count_Front_Members = 0

        For i = 0 To _Individ.GetUpperBound(0)
            If (_Individ(i).Front = aktuell_Front) Then
                Pareto_Count_Front_Members += 1
            End If
        Next i

    End Function

    'NDS_Crowding_Distance_Sort
    '**************************
    Private Sub Pareto_Crowding_Distance_Sort(ByRef _Individ() As Individuum, ByVal StartIndex As Integer, ByVal EndIndex As Integer)

        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim swap As New Individuum("Swap", 0)
        Dim fmin, fmax As Double

        For k = 0 To NPenalty - 1
            For i = StartIndex To EndIndex
                For j = StartIndex To EndIndex
                    If (_Individ(i).Penalty(k) < _Individ(j).Penalty(k)) Then
                        swap = _Individ(i).Copy
                        _Individ(i) = _Individ(j).Copy
                        _Individ(j) = swap.Copy
                    End If
                Next j
            Next i

            fmin = _Individ(StartIndex).Penalty(k)
            fmax = _Individ(EndIndex).Penalty(k)

            _Individ(StartIndex).Distance = 1.0E+300
            _Individ(EndIndex).Distance = 1.0E+300

            For i = StartIndex + 1 To EndIndex - 1
                _Individ(i).Distance = _Individ(i).Distance + (_Individ(i + 1).Penalty(k) - _Individ(i - 1).Penalty(k)) / (fmax - fmin)
            Next i
        Next k

        For i = StartIndex To EndIndex
            For j = StartIndex To EndIndex
                If (_Individ(i).Distance > _Individ(j).Distance) Then
                    swap = _Individ(i).Copy
                    _Individ(i) = _Individ(j).Copy
                    _Individ(j) = swap.Copy
                End If
            Next j
        Next i

    End Sub

End Class
