Imports IHWB.EVO.Common

Public Class Functions


    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse Functions                                                      ****
    '****                                                                       ****
    '**** Wird derzeit f�r das NDSorting verwendet um es f�r die verschiedenen  ****
    '**** Kerne anwenden zu k�nnen                                              ****
    '****                                                                       ****
    '**** Autoren: Christoph H�bner, Felix Froehlich                            ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

    'Die Statische Variablen werden im Konstruktor �bergeben
    '*******************************************************
    Dim NNachf As Integer
    Dim NEltern As Integer
    Dim isSekPopBegrenzung As Boolean
    Dim NMaxMemberSekPop As Integer
    Dim NInteract As Integer
    Dim isInteract As Boolean
    Dim iAktGen As Integer
    Dim iAktPop As Integer

    'Die Statische Variablen werden im Konstruktor �bergeben
    '*******************************************************
    Public Sub New(ByVal _NNachf As Integer, ByVal _NEltern As Integer, ByVal _isSekPopBegrenzung As Boolean, ByVal _NMaxMemberSekPop As Integer, ByVal _NInteract As Integer, ByVal _isInteract As Boolean, ByVal _iAktGen As Integer)

        NNachf = _NNachf
        NEltern = _NEltern
        isSekPopBegrenzung = _isSekPopBegrenzung
        NMaxMemberSekPop = _NMaxMemberSekPop
        NInteract = _NInteract
        isInteract = _isInteract
        iAktGen = _iAktGen

    End Sub

    'Dieser Teil besch�ftigt sich nur mit Sekund�rQb und NDSorting
    '2. Die einzelnen Fronten werden bestimmt
    '3. Der Bestwertspeicher wird entsprechend der Fronten oder der sekund�ren Population gef�llt
    '4: Sekund�re Population wird bestimmt und gespeichert
    '--------------------------------------------------------------------------------------------
    Public Sub EsEltern_Pareto(ByVal NDSorting() As Individuum, ByRef Sekund�rQb() As Individuum, ByRef Best() As Individuum)

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
        Call Individuum.Clone_Indi_Array(NDSorting, Temp)

        'Schleife l�uft �ber die Zahl der Fronten die hier auch bestimmt werden
        Do
            'Entscheidet welche Werte dominiert werden und welche nicht
            Call Pareto_Non_Dominated_Sorting(Temp, rang)
            'Sortiert die nicht dominanten L�sungen nach oben,
            'die dominanten nach unten und z�hlt die Mitglieder der aktuellen Front
            NFrontMember_aktuell = Pareto_Non_Dominated_Count_and_Sort(Temp)
            'NFrontMember_aktuell: Anzahl der Mitglieder der gerade bestimmten Front
            'NFrontMember_gesamt: Alle bisher als nicht dominiert klassifizierten Individuum
            NFrontMember_gesamt += NFrontMember_aktuell
            'Hier wird pro durchlauf die nicht dominierte Front in NDSResult geschaufelt
            'und die bereits klassifizierten L�sungen aus Temp Array gel�scht
            Call Pareto_Non_Dominated_Result(Temp, NDSResult, NFrontMember_aktuell, NFrontMember_gesamt)
            'Rang ist hier die Nummer der Front
            rang += 1
        Loop While Not (NFrontMember_gesamt = NEltern + NNachf)

        '3. Der Bestwertspeicher wird entsprechend der Fronten oder der sekund�ren Population gef�llt
        '--------------------------------------------------------------------------------------------
        NFrontMember_aktuell = 0
        NFrontMember_gesamt = 0
        aktuelle_Front = 0

        Do
            NFrontMember_aktuell = Pareto_Count_Front_Members(aktuelle_Front, NDSResult)

            'Es sind mehr Elterpl�tze f�r die n�chste Generation verf�gaber
            '-> schiss wird einfach r�berkopiert
            If NFrontMember_aktuell <= NEltern - NFrontMember_gesamt Then
                For i = NFrontMember_gesamt To NFrontMember_aktuell + NFrontMember_gesamt - 1

                    'NDSResult wird in den Bestwertspeicher kopiert
                    Best(i) = NDSResult(i).Clone()

                Next i
                NFrontMember_gesamt = NFrontMember_gesamt + NFrontMember_aktuell

            Else
                'Es sind weniger Elterpl�tze f�r die n�chste Generation verf�gber
                'als Mitglieder der aktuellen Front. Nur f�r diesen Rest wird crowding distance
                'gemacht um zu bestimmen wer noch mitspielen darf und wer noch a biserl was druff hat
                Call Pareto_Crowding_Distance_Sort(NDSResult, NFrontMember_gesamt, NFrontMember_gesamt + NFrontMember_aktuell - 1)

                For i = NFrontMember_gesamt To NEltern - 1
                    'NDSResult wird in den Bestwertspeicher kopiert
                    Best(i) = NDSResult(i).Clone()
                Next i

                NFrontMember_gesamt = NEltern

            End If

            aktuelle_Front += 1

        Loop While Not (NFrontMember_gesamt = NEltern)

        '4: Sekund�re Population wird aktualisiert
        '-----------------------------------------
        Call Sekund�rQb_Allocation(NDSResult, Sekund�rQb)

        'Pr�fen, ob die Population jetzt mit Mitgliedern aus der Sekund�ren Population aufgef�llt werden soll
        '----------------------------------------------------------------------------------------------------
        If (iAktGen Mod NInteract) = 0 And isInteract Then
            NFrontMember_aktuell = Pareto_Count_Front_Members(1, Sekund�rQb)
            If NFrontMember_aktuell > NEltern Then
                'Crowding Distance
                Call Pareto_Crowding_Distance_Sort(Sekund�rQb, 0, Sekund�rQb.GetUpperBound(0))
                'Anzahl Eltern wird aus Sekund�rQb in den Bestwertspeicher kopiert
                For i = 0 To NEltern - 1
                    Best(i) = Sekund�rQb(i).Clone()
                Next i
            End If
        End If

    End Sub

    'NON_DOMINATED_SORTING - Entscheidet welche Werte dominiert werden und welche nicht
    '**********************************************************************************
    Private Sub Pareto_Non_Dominated_Sorting(ByRef NDSorting() As Individuum, ByVal rang As Integer)

        Dim j, i, k As Integer
        Dim isDominated As Boolean
        Dim Summe_Constrain(1) As Double

        If (Common.Manager.AnzConstraints > 0) Then
            'Mit Constraints
            '===============
            For i = 0 To NDSorting.GetUpperBound(0)
                For j = 0 To NDSorting.GetUpperBound(0)

                    '�berp�fen, ob NDSorting(j) von NDSorting(i) dominiert wird
                    '----------------------------------------------------------
                    If (NDSorting(i).Is_Feasible And Not NDSorting(j).Is_Feasible) Then

                        'i g�ltig und j ung�ltig
                        '-----------------------
                        NDSorting(j).dominated = True

                    ElseIf ((Not NDSorting(i).Is_Feasible) And (Not NDSorting(j).Is_Feasible)) Then

                        'beide ung�ltig
                        '--------------
                        Summe_Constrain(0) = 0
                        Summe_Constrain(1) = 0

                        For k = 0 To Common.Manager.AnzConstraints - 1
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

                    ElseIf (NDSorting(i).Is_Feasible And NDSorting(j).Is_Feasible) Then

                        'beide g�ltig
                        '------------
                        isDominated = False

                        For k = 0 To Manager.AnzPenalty - 1
                            isDominated = isDominated Or (NDSorting(i).Penalties(k) < NDSorting(j).Penalties(k))
                        Next k

                        For k = 0 To Manager.AnzPenalty - 1
                            isDominated = isDominated And (NDSorting(i).Penalties(k) <= NDSorting(j).Penalties(k))
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

                    For k = 0 To Manager.AnzPenalty - 1
                        isDominated = isDominated Or (NDSorting(i).Penalties(k) < NDSorting(j).Penalties(k))
                    Next k

                    For k = 0 To Manager.AnzPenalty - 1
                        isDominated = isDominated And (NDSorting(i).Penalties(k) <= NDSorting(j).Penalties(k))
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

    'NON_DOMINATED_COUNT_AND_SORT - Sortiert die nicht dominanten L�sungen nach oben,
    'die dominanten nach unten, gibt die Zahl der dominanten L�sungen zur�ck (Front)
    '*******************************************************************************
    Private Function Pareto_Non_Dominated_Count_and_Sort(ByRef NDSorting() As Individuum) As Integer

        Dim i As Integer
        Dim Temp() As Individuum
        Dim counter As Integer
        Dim NFrontMember As Integer

        ReDim Temp(NDSorting.GetUpperBound(0))

        NFrontMember = 0
        counter = 0

        'Die dominierten L�sungen werden nach oben kopiert
        For i = 0 To NDSorting.GetUpperBound(0)
            If (NDSorting(i).dominated = True) Then
                counter += 1
                Temp(counter - 1) = NDSorting(i).Clone()
            End If
        Next i

        'Zahl der dominanten L�sungen wird errechnet
        NFrontMember = NDSorting.GetUpperBound(0) + 1 - counter

        'Die nicht dominierten L�sungen werden nach unten kopiert
        For i = 0 To NDSorting.GetUpperBound(0)
            If (NDSorting(i).dominated = False) Then
                counter += 1
                Temp(counter - 1) = NDSorting(i).Clone()
            End If
        Next i

        Call Individuum.Clone_Indi_Array(Temp, NDSorting)

        Return NFrontMember

    End Function

    'NON_DOMINATED_RESULT - Hier wird pro durchlauf die nicht dominierte Front in NDSResult
    'geschaufelt und die bereits klassifizierten L�sungen aus Temp Array gel�scht
    '**************************************************************************************
    Private Sub Pareto_Non_Dominated_Result(ByRef Temp() As Individuum, ByRef NDSResult() As Individuum, ByVal NFrontMember_aktuell As Integer, ByVal NFrontMember_gesamt As Integer)

        Dim i, Position As Integer

        Position = NFrontMember_gesamt - NFrontMember_aktuell

        'In NDSResult werden die nicht dominierten L�sungen eingef�gt
        For i = Temp.GetLength(0) - NFrontMember_aktuell To Temp.GetUpperBound(0)
            'NDSResult alle bisher gefundene Fronten
            NDSResult(Position) = Temp(i).Clone()
            Position += 1
        Next i

        'Die bereits klassifizierten Member werden aus dem Temp Array gel�scht
        If (NNachf + NEltern - NFrontMember_gesamt > 0) Then
            ReDim Preserve Temp(NNachf + NEltern - NFrontMember_gesamt - 1)
            'Der Flag wird zur klassifizierung in der n�chsten Runde zur�ckgesetzt
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
        Dim swap As Individuum
        Dim fmin, fmax As Double

        swap = _Individ(0).Create("swap", 0)

        For k = 0 To Manager.AnzPenalty - 1
            For i = StartIndex To EndIndex
                For j = StartIndex To EndIndex
                    If (_Individ(i).Penalties(k) < _Individ(j).Penalties(k)) Then
                        swap = _Individ(i).Clone()
                        _Individ(i) = _Individ(j).Clone()
                        _Individ(j) = swap.Clone()
                    End If
                Next j
            Next i

            fmin = _Individ(StartIndex).Penalties(k)
            fmax = _Individ(EndIndex).Penalties(k)

            _Individ(StartIndex).Distance = 1.0E+300
            _Individ(EndIndex).Distance = 1.0E+300

            For i = StartIndex + 1 To EndIndex - 1
                _Individ(i).Distance = _Individ(i).Distance + (_Individ(i + 1).Penalties(k) - _Individ(i - 1).Penalties(k)) / (fmax - fmin)
            Next i
        Next k

        For i = StartIndex To EndIndex
            For j = StartIndex To EndIndex
                If (_Individ(i).Distance > _Individ(j).Distance) Then
                    swap = _Individ(i).Clone()
                    _Individ(i) = _Individ(j).Clone()
                    _Individ(j) = swap.Clone()
                End If
            Next j
        Next i

    End Sub

    'NDS_Crowding_Distance
    '**************************
    Public Sub Pareto_Crowding_Distance(ByRef _Individ() As Individuum)

        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim swap As Individuum
        Dim fmin, fmax As Double
        Dim StartIndex As Integer
        Dim EndIndex As Integer

        swap = _Individ(0).Create("Swap", 0)

        StartIndex = 0
        EndIndex = _Individ.GetUpperBound(0)
        For i = StartIndex To EndIndex
            _Individ(i).Distance = 0.0
        Next i

        For k = 0 To Manager.AnzPenalty - 1
            For i = StartIndex To EndIndex
                For j = StartIndex To EndIndex
                    If (_Individ(i).Penalties(k) < _Individ(j).Penalties(k)) Then
                        swap = _Individ(i).Clone()
                        _Individ(i) = _Individ(j).Clone()
                        _Individ(j) = swap.Clone()
                    End If
                Next j
            Next i

            fmin = _Individ(StartIndex).Penalties(k)
            fmax = _Individ(EndIndex).Penalties(k)

            _Individ(StartIndex).Distance = 1.0E+300
            _Individ(EndIndex).Distance = 1.0E+300

            For i = StartIndex + 1 To EndIndex - 1
                If Not _Individ(i).Distance = 1.0E+300 Then
                    _Individ(i).Distance = _Individ(i).Distance + (_Individ(i + 1).Penalties(k) - _Individ(i - 1).Penalties(k)) / (fmax - fmin)
                End If
            Next i
        Next k
    End Sub

    '4: Sekund�re Population wird aktualisiert
    '-----------------------------------------
    Private Sub Sekund�rQb_Allocation(ByVal NDSResult() As Common.Individuum, ByRef Sekund�rQb() As Common.Individuum)

        Dim i, NFrontMember_aktuell, NMember_SekPop As Integer

        'Anzahl Frontmember in NDSResult bestimmen
        NFrontMember_aktuell = Pareto_Count_Front_Members(1, NDSResult)

        'Aktuelle Anzahl Mitglieder in SekPop bestimmen
        NMember_SekPop = Sekund�rQb.GetLength(0)

        'SekPop um die aktuelle Front erweitern
        ReDim Preserve Sekund�rQb(NMember_SekPop + NFrontMember_aktuell - 1)
        For i = NMember_SekPop To NMember_SekPop + NFrontMember_aktuell - 1
            Sekund�rQb(i) = NDSResult(i - NMember_SekPop)
        Next i

        'SekPop neu sortieren und hinteren R�nge entfernen
        Call Pareto_Non_Dominated_Sorting(Sekund�rQb, 1)
        NFrontMember_aktuell = Sekund�rQb_Non_Dominated_Count_and_Sort(Sekund�rQb)
        ReDim Preserve Sekund�rQb(NFrontMember_aktuell - 1)

        'Dubletten aus SekPop entfernen
        Call Sekund�rQb_Dubletten(Sekund�rQb)
        NFrontMember_aktuell = Sekund�rQb_Non_Dominated_Count_and_Sort(Sekund�rQb)
        ReDim Preserve Sekund�rQb(NFrontMember_aktuell - 1)

        'SekPop ggf. auf Maximalanzahl Mitglieder begrenzen (mit Crowding Distance)
        If (Me.isSekPopBegrenzung And Sekund�rQb.GetLength(0) > Me.NMaxMemberSekPop) Then
            Call Pareto_Crowding_Distance_Sort(Sekund�rQb, 0, Sekund�rQb.GetUpperBound(0))
            ReDim Preserve Sekund�rQb(Me.NMaxMemberSekPop - 1)
        End If

    End Sub

    'Individuen mit identischen Penalties als dominiert markieren
    '************************************************************
    Private Sub Sekund�rQb_Dubletten(ByRef Sekund�rQb() As Common.Individuum)

        Dim i, j, k As Integer
        Dim Logical As Boolean

        For i = 0 To Sekund�rQb.GetUpperBound(0) - 1
            For j = i + 1 To Sekund�rQb.GetUpperBound(0)
                Logical = True
                For k = 0 To Manager.AnzPenalty - 1
                    Logical = Logical And (Sekund�rQb(i).Penalties(k) = Sekund�rQb(j).Penalties(k))
                Next k
                If (Logical) Then
                    'Duplikat gefunden: als dominiert markieren
                    Sekund�rQb(i).dominated = True
                End If
            Next j
        Next i
    End Sub

    'NON_DOMINATED_COUNT_AND_SORT_SEKUND�RE_POPULATION
    'Sortiert die dominanten L�sungen nach oben, die nicht dominanten nach unten
    'Gibt die Zahl der dominanten L�sungen zur�ck (Front) hier f�r die Sekund�re Population
    '**************************************************************************************
    Private Function Sekund�rQb_Non_Dominated_Count_and_Sort(ByRef Sekund�rQb() As Individuum) As Integer

        Dim i As Integer
        Dim Temp() As Individuum
        Dim counter As Integer
        Dim NFrontMember As Integer

        ReDim Temp(Sekund�rQb.GetUpperBound(0))

        NFrontMember = 0
        counter = 0

        'Die nicht dominierten L�sungen werden nach oben kopiert
        For i = 0 To Sekund�rQb.GetUpperBound(0)
            If (Sekund�rQb(i).dominated = False) Then
                counter += 1
                Temp(counter - 1) = Sekund�rQb(i).Clone()
            End If
        Next i

        NFrontMember = counter

        'Die dominierten L�sungen werden nach unten kopiert
        For i = 0 To Sekund�rQb.GetUpperBound(0)
            If (Sekund�rQb(i).dominated = True) Then
                counter += 1
                Temp(counter - 1) = Sekund�rQb(i).Clone()
            End If
        Next i

        Call Individuum.Clone_Indi_Array(Temp, Sekund�rQb)

        Return NFrontMember

    End Function

End Class
