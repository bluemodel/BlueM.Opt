Public Class CES

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse CES Kombinatorische Evolutionsstrategie                        ****
    '****                                                                       ****
    '**** Christoph H�bner                                                      ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '****                                                                       ****
    '**** Februar 2007                                                          ****
    '****                                                                       ****
    '**** Letzte �nderung: M�rz 2007                                            ****
    '*******************************************************************************
    '*******************************************************************************

#Region "Eigenschaften"
    '##################

    'Public Variablen
    Public n_Location As Integer       'Anzahl der Locations wird von au�en gesetzt
    Public n_Penalty As Integer           'Anzahl der Ziele wird von au�en gesetzt
    Public n_Verzweig As Integer        'Anzahl der Verzweigungen in der Verzweigungsdatei
    Public n_PathDimension() As Integer     'Anzahl der "St�dte/Ma�nahmen" an jeder Stelle
    Public n_Generation As Integer = 2

    'Eingabe
    Public n_Parents As Integer = 3
    Public n_Childs As Integer = 7

    'Private Variablen
    Private ReprodOperator_TSP As String = "Order_Crossover_OX"
    Private ReprodOperator_BM As String = "Select_Random_Uniform"
    Private MutOperator_TSP As String = "Translocation"
    Private MutOperator_BM As String = "RND_Switch"
    Private Strategy As String = "plus"         '"plus" oder "minus" Strategie
    Private MutRate As Integer = 10              'Definiert die Wahrscheinlichkeit der Mutationsrate in %

    'Faksimile Struktur
    '******************
    Public Structure Struct_Faksimile
        Dim No As Short                     'Nummer des NDSorting
        Dim Path() As Integer               'Der Pfad
        Dim Elemente() As Object            'Liste der Elemente
        Dim Penalty() As Double             'Werte der Penaltyfunktion(en)
        Dim mutated As Boolean              'Gibt an ob der Wert bereits mutiert ist oder nicht
        Dim Front As Short                  'Nummer der Pareto Front
    End Structure

    Public List_Childs() As Struct_Faksimile
    Public List_Parents() As Struct_Faksimile

    'NDSorting Struktur
    '******************
    Public Structure Struct_NDSorting
        Dim No As Short                     'CH: Nummer des NDSorting
        Dim Path() As Integer               'CH: Der Pfad
        Dim Elemente() As Object            'CH: Liste der Elemente
        Dim Penalty() As Double             'DM: Werte der Penaltyfunktion(en)
        'Dim Constrain() As Double           'DM: Werte der Randbedingung(en)
        'Dim Feasible As Boolean             'DM: G�ltiges Ergebnis ?
        Dim dominated As Boolean            'DM: Kennzeichnung ob dominiert
        Dim Front As Short                  'DM: Nummer der Pareto Front
        Dim Distance As Double              'DM: Distanzwert f�r Crowding distance sort
    End Structure

    Public NDSorting() As Struct_NDSorting
    Public NDSResult(n_Childs + n_Parents - 1) As Struct_NDSorting

    'PES Struktur
    '************
    Public Structure Struct_PES
        Dim No As Short

    End Structure

    Public List_PES() as Struct_PES

#End Region 'Eigenschaften

#Region "Methoden"
    '#############

    'Dimensionieren des Faksimile
    '*******************************
    Public Sub Dim_Faksimile(ByRef TMP() As Struct_Faksimile)
        Dim i, j As Integer
        ReDim TMP(n_Childs - 1)

        For i = 0 To TMP.GetUpperBound(0)
            TMP(i).No = i + 1
            ReDim TMP(i).Penalty(n_Penalty - 1)
            Redim TMP(i).Elemente(0)
            For j = 0 To n_Penalty - 1
                TMP(i).Penalty(j) = 999999999999999999
            Next
            ReDim TMP(i).Path(n_Location - 1)
        Next

    End Sub

    'Dimensionieren des NDSortingStructs
    '***********************************
    Public Sub Dim_NDSorting_Type(ByRef TMP() As Struct_NDSorting)
        Dim i, j As Integer

        For i = 0 To TMP.GetUpperBound(0)
            TMP(i).No = i + 1
            ReDim TMP(i).Path(n_Location - 1)
            Redim tmp(i).Elemente(0)
            ReDim TMP(i).Penalty(n_Penalty - 1)
            For j = 0 To n_Penalty - 1
                TMP(i).Penalty(j) = 999999999999999999
            Next

        Next

    End Sub

    'Kopiert ein Faksimile
    '*********************
    Public Sub Copy_Faksimile_NDSorting(ByVal Source As Struct_Faksimile, ByRef Destination As Struct_Faksimile)

        Destination.No = Source.No
        Array.Copy(Source.Path, Destination.Path, Source.Path.Length)
        ReDim Destination.Elemente(Source.Elemente.GetUpperBound(0))
        Array.Copy(Source.Elemente, Destination.Elemente, Source.Elemente.Length)
        Array.Copy(Source.Penalty, Destination.Penalty, Source.Penalty.Length)
        Destination.mutated = Source.mutated
        Destination.Front = Source.Front

    End Sub

    'Kopiert ein Faksimile soweit m�glich in ein NDSorting
    '*****************************************************
    Public Sub Copy_Faksimile_NDSorting(ByVal Source As Struct_Faksimile, ByRef Destination As Struct_NDSorting)

        Destination.No = Source.No
        Array.Copy(Source.Path, Destination.Path, Source.Path.Length)
        ReDim Destination.Elemente(Source.Elemente.GetUpperBound(0))
        Array.Copy(Source.Elemente, Destination.Elemente, Source.Elemente.Length)
        Array.Copy(Source.Penalty, Destination.Penalty, Source.Penalty.Length)
        Destination.Front = Source.Front

    End Sub

    'Kopiert ein NDSOrting in ein Faksimile
    '**************************************
    Public Sub Copy_Faksimile_NDSorting(ByVal Source As Struct_NDSorting, ByRef Destination As Struct_Faksimile)

        Destination.No = Source.No
        Array.Copy(Source.Path, Destination.Path, Source.Path.Length)
        ReDim Destination.Elemente(Source.Elemente.GetUpperBound(0))
        Array.Copy(Source.Elemente, Destination.Elemente, Source.Elemente.Length)
        Array.Copy(Source.Penalty, Destination.Penalty, Source.Penalty.Length)
        Destination.Front = Source.Front

    End Sub

    'Kopiert ein NDSOrting
    '*********************
    Public Sub Copy_Faksimile_NDSorting(ByVal Source As Struct_NDSorting, ByRef Destination As Struct_NDSorting)

        Destination.No = Source.No
        Array.Copy(Source.Path, Destination.Path, Source.Path.Length)
        ReDim Destination.Elemente(Source.Elemente.GetUpperBound(0))
        Array.Copy(Source.Elemente, Destination.Elemente, Source.Elemente.Length)
        Array.Copy(Source.Penalty, Destination.Penalty, Source.Penalty.Length)
        Destination.dominated = Source.dominated
        Destination.Front = Source.Front

        Destination.Distance = Source.Distance

    End Sub

    'Generiert zuf�llige Paths f�r alle Kinder BM Problem
    '****************************************************
    Public Sub Generate_Random_Path()
        Dim i, j As Integer
        Dim tmp As Integer
        Dim lowerb As Integer = 0
        Dim upperb As Integer
        Randomize()

        For i = 0 To n_Childs - 1
            Do
                For j = 0 To n_Location - 1
                    upperb = n_PathDimension(j) - 1
                    'Randomize() nicht vergessen
                    tmp = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
                    List_Childs(i).Path(j) = tmp
                Next
                List_Childs(i).mutated = True
                List_Childs(i).No = i + 1
            Loop While Is_Twin(i) = True
        Next

    End Sub

    'HACK: Funktion zum manuellen testen aller Kombinationen
    '*******************************************************
    Public Sub Generate_All_Test_Path()
        Dim i As Integer
        Dim x, y, z As Integer

        x = 0
        y = 0
        z = 0

        For i = 0 To n_Childs - 1

            List_Childs(i).Path(0) = x
            List_Childs(i).Path(1) = y
            List_Childs(i).Path(2) = z
            x += 1
            If x > n_PathDimension(0) - 1 Then
                x = 0
                y += 1
            End If
            If y > n_PathDimension(1) - 1 Then
                y = 0
                z += 1
            End If
            If z > n_PathDimension(2) - 1 Then
                z = 0
            End If

        Next

    End Sub

    'Selectionsprozess je nach "plus" oder "minus" Strategie
    '*******************************************************
    Public Sub Selection_Process()
        Dim i, j As Integer
        Dim ChildQ_TMP As Double = 0
        Dim ParentQ_TMP As Double = 0

        If Strategy = "minus" Then
            For i = 0 To n_Parents - 1
                Call Copy_Faksimile_NDSorting(List_Childs(i), List_Parents(i))
            Next i

        ElseIf Strategy = "plus" Then
            j = 0
            For i = 0 To n_Parents - 1
                If List_Parents(i).Penalty(0) < List_Childs(j).Penalty(0) Then
                    j -= 1
                Else
                    Call Copy_Faksimile_NDSorting(List_Childs(i), List_Parents(i))
                End If
                j += 1
            Next i
        End If

    End Sub

    'Kinder werden zur Sicherheit gel�scht aber nicht zerst�rt ;-)
    '*************************************************************
    Public Sub Reset_Childs()
        Dim i, j As Integer

        For i = 0 To n_Childs - 1
            List_Childs(i).No = i + 1
            For j = 0 To List_Childs(i).Penalty.GetUpperBound(0)
                List_Childs(i).Penalty(j) = 999999999999999999
            Next
            Array.Clear(List_Childs(i).Path, 0, List_Childs(i).Path.GetLength(0))
            List_Childs(i).mutated = False
            For j = 0 To List_Childs(i).Elemente.GetUpperBound(0)
                List_Childs(i).Elemente(j) = ""
            Next
        Next

    End Sub

    'Reproductionsfunktionen
    'XXXXXXXXXXXXXXXXXXXXXXX

    'Steuerung der Reproduktionsoperatoren
    '*************************************
    Public Sub Reproduction_Control()
        Dim i As Integer
        Dim x, y As Integer
        Dim Einzelkind(n_Location - 1) As Integer

        Select Case ReprodOperator_BM
            'UPGRADE: Eltern werden nicht zuf�llig gew�hlt sondern immer in Top Down Reihenfolge
            Case "Select_Random_Uniform"
                x = 0
                y = 1
                For i = 0 To n_Childs - 2 Step 2
                    Call ReprodOp_Select_Random_Uniform(List_Parents(x).Path, List_Parents(y).Path, List_Childs(i).Path, List_Childs(i + 1).Path)
                    x += 1
                    y += 1
                    If x = n_Parents - 1 Then x = 0
                    If y = n_Parents - 1 Then y = 0
                Next i
                If Even_Number(n_Childs) = False Then
                    Call ReprodOp_Select_Random_Uniform(List_Parents(x).Path, List_Parents(y).Path, List_Childs(n_Childs - 1).Path, Einzelkind)
                End If
        End Select

    End Sub

    'Reproductionsoperator: "Select_Random_Uniform"
    'Entscheidet zuf�llig ob der Wert aus dem Path des Elter_A oder Elter_B verwendet wird
    '*************************************************************************************
    Private Sub ReprodOp_Select_Random_Uniform(ByVal ParPath_A() As Integer, ByVal ParPath_B() As Integer, ByRef ChildPath_A() As Integer, ByRef ChildPath_B() As Integer)

        Dim i As Integer

        For i = 0 To ChildPath_A.GetUpperBound(0)    'TODO: Es m�sste eigentlich eine definierte Pfadl�nge geben
            If Bernoulli() = True Then
                ChildPath_A(i) = ParPath_B(i)
            Else
                ChildPath_A(i) = ParPath_A(i)
            End If
        Next

        For i = 0 To ChildPath_B.GetUpperBound(0)    'TODO: Es m�sste eigentlich eine definierte Pfadl�nge geben
            If Bernoulli() = True Then
                ChildPath_B(i) = ParPath_A(i)
            Else
                ChildPath_B(i) = ParPath_B(i)
            End If
        Next

    End Sub

    'Mutationsfunktionen
    'XXXXXXXXXXXXXXXXXXX

    'Steuerung der Mutationsoperatoren
    '*********************************
    Public Sub Mutation_Control()
        Dim i As Integer

        Select Case MutOperator_BM
            Case "RND_Switch"
                For i = 0 To n_Childs - 1
                    Do
                        Call MutOp_RND_Switch(List_Childs(i).Path)
                        List_Childs(i).mutated = True
                    Loop While Is_Twin(i) = True Or Is_Clone(i) = True
                Next i
            Case "Dyn_Switch"
                Dim count As Integer = 0
                For i = 0 To n_Childs - 1
                    Do
                        Call MutOp_Dyn_Switch(List_Childs(i).Path, count)
                        List_Childs(i).mutated = True
                        count += 1
                    Loop While Is_Twin(i) = True Or Is_Clone(i) = True
                Next i
        End Select

    End Sub

    'Mutationsoperator "RND_Switch"
    'Ver�ndert zuf�llig ein gen des Paths
    '************************************
    Private Sub MutOp_RND_Switch(ByVal Path() As Integer)
        Dim i As Integer
        Dim Tmp_a As Integer
        Dim Tmp_b As Integer
        Dim lowerb_a As Integer = 1
        Dim upperb_a As Integer = 100
        Dim lowerb_b As Integer = 0
        Dim upperb_b As Integer
        Randomize()

        For i = 0 To Path.GetUpperBound(0)
            Tmp_a = CInt(Int((upperb_a - lowerb_a + 1) * Rnd() + lowerb_a))
            If Tmp_a <= MutRate Then
                upperb_b = n_PathDimension(i) - 1
                'Randomize() nicht vergessen
                Tmp_b = CInt(Int((upperb_b - lowerb_b + 1) * Rnd() + lowerb_b))
                Path(i) = Tmp_b
            End If
        Next

    End Sub

    'Mutationsoperator "Dyn_Switch"
    'Ver�ndert zuf�llig ein gen des Paths
    '************************************
    Private Sub MutOp_Dyn_Switch(ByVal Path() As Integer, ByVal Dyn_MutRate As Integer)
        Dim i As Integer
        Dim Tmp_a As Integer
        Dim Tmp_b As Integer
        Dim lowerb_a As Integer = 1
        Dim upperb_a As Integer = 100
        Dim lowerb_b As Integer = 0
        Dim upperb_b As Integer
        Randomize()

        For i = 0 To Path.GetUpperBound(0)
            Tmp_a = CInt(Int((upperb_a - lowerb_a + 1) * Rnd() + lowerb_a))
            If Tmp_a <= Dyn_MutRate Then
                upperb_b = n_PathDimension(i) - 1
                'Randomize() nicht vergessen
                Tmp_b = CInt(Int((upperb_b - lowerb_b + 1) * Rnd() + lowerb_b))
                Path(i) = Tmp_b
            End If
        Next

    End Sub

    'Hilfsfunktionen
    'XXXXXXXXXXXXXXX

    'Hilfsfunktion um zu Pr�fen ob eine Zahl bereits in einem Array vorhanden ist oder nicht
    '***************************************************************************************
    Public Function Is_No_OK(ByVal No As Integer, ByVal Path() As Integer) As Boolean
        Is_No_OK = True
        Dim i As Integer
        For i = 0 To Path.GetUpperBound(0)
            If No = Path(i) Then
                Is_No_OK = False
                Exit Function
            End If
        Next
    End Function

    'Hilfsfunktion zum sortieren der Faksimile
    '*****************************************
    Public Sub Sort_Faksimile(ByRef FaksimileList() As Struct_Faksimile)
        'Sortiert die Fiksimile anhand des Abstandes
        Dim i, j As Integer
        Dim swap As EvoKern.CES.Struct_Faksimile

        For i = 0 To FaksimileList.GetUpperBound(0)
            For j = 0 To FaksimileList.GetUpperBound(0)
                If FaksimileList(i).Penalty(0) < FaksimileList(j).Penalty(0) Then
                    swap = FaksimileList(i)
                    FaksimileList(i) = FaksimileList(j)
                    FaksimileList(j) = swap
                End If
            Next j
        Next i

    End Sub

    'Hilfsfunktion checkt ob die neuen Childs Zwillinge sind
    '*******************************************************
    Private Function Is_Twin(ByVal ChildIndex As Integer) As Boolean
        Dim n As Integer = 0
        Dim i, j As Integer
        Dim PathOK As Boolean
        PathOK = False
        Is_Twin = False

        For i = 0 To n_Childs - 1
            If ChildIndex <> i And List_Childs(i).mutated = True Then
                PathOK = False
                For j = 0 To List_Childs(ChildIndex).Path.GetUpperBound(0)
                    If List_Childs(ChildIndex).Path(j) <> List_Childs(i).Path(j) Then
                        PathOK = True
                    End If
                Next
                If PathOK = False Then
                    Is_Twin = True
                End If
            End If
        Next
    End Function

    'Hilfsfunktion checkt ob die neuen Childs Kone sind
    '**************************************************
    Private Function Is_Clone(ByVal ChildIndex As Integer) As Boolean
        Dim i, j As Integer
        Dim PathOK As Boolean
        PathOK = False
        Is_Clone = False

        For i = 0 To n_Parents - 1
            PathOK = False
            For j = 0 To List_Childs(ChildIndex).Path.GetUpperBound(0)
                If List_Childs(ChildIndex).Path(j) <> List_Parents(i).Path(j) Then
                    PathOK = True
                End If
            Next
            If PathOK = False Then
                Is_Clone = True
            End If
        Next

    End Function

    'Hilfsfunktion generiert Bernoulli verteilte Zufallszahl
    '*******************************************************
    Public Function Bernoulli() As Boolean
        Dim lowerb As Integer = 0
        Dim upperbo As Integer = 1
        Bernoulli = CInt(Int(2 * Rnd()))
    End Function

    'Hilfsfunktion: Gerade oder Ungerade Zahl
    '****************************************
    Public Function Even_Number(ByVal Number As Integer) As Boolean
        Dim tmp_a As Double
        Dim tmp_b As Double
        Dim tmp_c As Double
        tmp_a = Number / 2
        tmp_b = Math.Ceiling(tmp_a)
        tmp_c = tmp_a - tmp_b
        If tmp_c = 0 Then Even_Number = True
    End Function

    'NonDominated Sorting
    'XXXXXXXXXXXXXXXXXXXX

    'Steuerung des NDSorting
    '**************************************************
    Public Sub NDSorting_Control()
        Dim i As Short
        Dim NFrontMember_aktuell, NFrontMember_gesamt As Short
        Dim Durchlauf_Front As Short
        'Dim Count as short
        Dim aktuelle_Front As Short
        'Dim Member_Sekund�refront As Short

        ReDim NDSorting(n_Childs + n_Parents - 1)
        Call Dim_NDSorting_Type(NDSorting)

        '0. Eltern und Nachfolger werden gemeinsam betrachtet
        'Die Kinder werden NDSorting hinzugef�gt
        '-------------------------------------------

        For i = 0 To n_Childs - 1
 
            ''NConstrains ********************************
            'If Eigenschaft.NConstrains > 0 Then
            '    NDSorting(i).Feasible = True
            '    For l = 1 To Eigenschaft.NConstrains
            '        NDSorting(i).Constrain(l) = Rb(m - Eigenschaft.NNachf, Eigenschaft.iaktuellePopulation, l)
            '        If NDSorting(i).Constrain(l) < 0 Then NDSorting(i).Feasible = False
            '    Next l
            'End If

            Call Copy_Faksimile_NDSorting(List_Childs(i), NDSorting(i))

            NDSorting(i).dominated = False
            NDSorting(i).Front = 0
            NDSorting(i).Distance = 0
  
        Next i

        '1. Eltern und Nachfolger werden gemeinsam betrachtet
        'Die Eltern werden NDSorting hinzugef�gt
        '-------------------------------------------

        For i = n_Childs To n_Childs + n_Parents - 1

            ''NConstrains ********************************
            'If Eigenschaft.NConstrains > 0 Then
            '    NDSorting(i).Feasible = True
            '    For l = 1 To Eigenschaft.NConstrains
            '        NDSorting(i).Constrain(l) = Rb(m - Eigenschaft.NNachf, Eigenschaft.iaktuellePopulation, l)
            '        If NDSorting(i).Constrain(l) < 0 Then NDSorting(i).Feasible = False
            '    Next l
            'End If

            Call Copy_Faksimile_NDSorting(List_Parents(i - n_Childs), NDSorting(i))

            NDSorting(i).dominated = False
            NDSorting(i).Front = 0
            NDSorting(i).Distance = 0

        Next i

        '2. Die einzelnen Fronten werden bestimmt
        '----------------------------------------
        Durchlauf_Front = 1
        NFrontMember_gesamt = 0

        'Initialisierung von Temp (NDSorting)
        Dim Temp(n_Childs + n_Parents - 1) As Struct_NDSorting
        Call Dim_NDSorting_Type(Temp)

        'Initialisierung von NDSResult (NDSorting)
        Call Dim_NDSorting_Type(NDSResult)

        'NDSorting wird in Temp kopiert
        For i = 0 To NDSorting.GetUpperBound(0)
            call Copy_Faksimile_NDSorting(NDSOrting(i),temp(i))
        Next

        'Schleife l�uft �ber die Zahl der Fronten die hier auch bestimmte werden
        Do
            'A: Entscheidet welche Werte dominiert werden und welche nicht
            Call Non_Dominated_Sorting(Temp, Durchlauf_Front) 'aktuallisiert auf n Objectives dm 10.05.05

            'B: Sortiert die nicht dominanten L�sungen nach oben,
            'die dominanten nach unten und z�hlt die Mitglieder der aktuellen Front
            NFrontMember_aktuell = Non_Dominated_Count_and_Sort(Temp)
            'NFrontMember_aktuell: Anzahl der Mitglieder der gerade bestimmten Front
            'NFrontMember_gesamt: Alle bisher als nicht dominiert klassifizierten Individuum
            NFrontMember_gesamt = NFrontMember_gesamt + NFrontMember_aktuell

            'C: Hier wird pro durchlauf die nicht dominierte Front in NDSResult geschaufelt
            'und die bereits klassifizierten L�sungen aus Temp Array gel�scht
            Call Non_Dominated_Result(Temp, NDSResult, NFrontMember_aktuell, NFrontMember_gesamt)
            'Durchlauf ist hier die Nummer der Front
            Durchlauf_Front = Durchlauf_Front + 1
        Loop While Not (NFrontMember_gesamt = n_Parents + n_Childs)

        '3. Der Bestwertspeicher wird entsprechend der Fronten oder der
        'sekund�ren Population gef�llt
        '-------------------------------------------------------------
        NFrontMember_aktuell = 0
        NFrontMember_gesamt = 0
        aktuelle_Front = 1

        Do
            NFrontMember_aktuell = Count_Front_Members(aktuelle_Front, NDSResult)

            'Es sind mehr Elterpl�tze f�r die n�chste Generation verf�gaber
            '-> schiss wird einfach r�berkopiert
            If NFrontMember_aktuell <= n_Parents - NFrontMember_gesamt Then
                For i = NFrontMember_gesamt To NFrontMember_aktuell + NFrontMember_gesamt - 1
                    call Copy_Faksimile_NDSorting(NDSResult(i),List_Parents(i))
                Next i
                NFrontMember_gesamt = NFrontMember_gesamt + NFrontMember_aktuell

            Else
                'Es sind weniger Elterpl�tze f�r die n�chste Generation verf�gber
                'als Mitglieder der aktuellen Front. Nur f�r diesen Rest wird crowding distance
                'gemacht um zu bestimmen wer noch mitspielen darf und wer noch a biserl was druff hat
                Call NDS_Crowding_Distance_Sort(NDSResult, NFrontMember_gesamt, NFrontMember_gesamt + NFrontMember_aktuell - 1)

                For i = NFrontMember_gesamt To n_Parents - 1
                    Call Copy_Faksimile_NDSorting(NDSResult(i), List_Parents(i))
                Next i

                NFrontMember_gesamt = n_Parents

            End If

            aktuelle_Front = aktuelle_Front + 1

        Loop While Not (NFrontMember_gesamt = n_Parents)

        '        '4: Sekund�re Population wird bestimmt und gespeichert
        '        NFrontMember_aktuell = Count_Front_Members(1, NDSResult)

        '        If Eigenschaft.iaktuelleRunde = 1 And Eigenschaft.iaktuellePopulation = 1 And Eigenschaft.iaktuelleGeneration = 1 Then
        '            ReDim Preserve Sekund�rQb(Member_Sekund�refront + NFrontMember_aktuell)
        '        Else
        '            Member_Sekund�refront = UBound(Sekund�rQb)
        '            ReDim Preserve Sekund�rQb(Member_Sekund�refront + NFrontMember_aktuell)
        '        End If

        '        For i = Member_Sekund�refront + 1 To Member_Sekund�refront + NFrontMember_aktuell
        '            Sekund�rQb(i) = NDSResult(i - Member_Sekund�refront)
        '        Next i

        '        Call Non_Dominated_Sorting(Sekund�rQb, 1)
        '        NFrontMember_aktuell = Non_Dominated_Count_and_Sort_Sekund�re_Population(Sekund�rQb)
        '        ReDim Preserve Sekund�rQb(NFrontMember_aktuell)
        '        Call Sekund�rQb_Duplettten(Sekund�rQb)
        '        NFrontMember_aktuell = Non_Dominated_Count_and_Sort_Sekund�re_Population(Sekund�rQb)
        '        ReDim Preserve Sekund�rQb(NFrontMember_aktuell)

        '        If UBound(Sekund�rQb) > Eigenschaft.NMemberSecondPop Then
        '            Call NDS_Crowding_Distance_Sort(Sekund�rQb, 1, UBound(Sekund�rQb))
        '            ReDim Preserve Sekund�rQb(Eigenschaft.NMemberSecondPop)
        '        End If

        '        If (Eigenschaft.iaktuelleGeneration Mod Eigenschaft.interact) = 0 And Eigenschaft.isInteract Then
        '            NFrontMember_aktuell = Count_Front_Members(1, Sekund�rQb)
        '            If NFrontMember_aktuell > Eigenschaft.NEltern Then
        '                Call NDS_Crowding_Distance_Sort(Sekund�rQb, 1, UBound(Sekund�rQb))
        '                For i = 1 To Eigenschaft.NEltern

        '                    For j = 1 To Eigenschaft.NPenalty
        '                        Qb(i, Eigenschaft.iaktuellePopulation, j) = Sekund�rQb(i).penalty(j)
        '                    Next j
        '                    If Eigenschaft.NConstrains > 0 Then
        '                        For j = 1 To Eigenschaft.NConstrains
        '                            Rb(i, Eigenschaft.iaktuellePopulation, j) = Sekund�rQb(i).constrain(j)
        '                        Next j
        '                    End If
        '                    For v = 1 To Eigenschaft.varanz
        '                        Db(v, i, Eigenschaft.iaktuellePopulation) = Sekund�rQb(i).d(v)
        '                        Xb(v, i, Eigenschaft.iaktuellePopulation) = Sekund�rQb(i).X(v)
        '                    Next v

        '                Next i
        '            End If
        '        End If

        '        'Neue Eltern werden gleich dem Bestwertspeicher gesetzt
        '        For m = 1 To Eigenschaft.NEltern
        '            For v = 1 To Eigenschaft.varanz
        '                De(v, m, Eigenschaft.iaktuellePopulation) = Db(v, m, Eigenschaft.iaktuellePopulation)
        '                Xe(v, m, Eigenschaft.iaktuellePopulation) = Xb(v, m, Eigenschaft.iaktuellePopulation)
        '            Next v
        '        Next m

        '        'Sortierung der L�sungen ist nur f�r Neighbourhood-Rekombination notwendig
        '        If Eigenschaft.iOptEltern = EVO_ELTERN_Neighbourhood Then
        '            Call Neighbourhood_AbstandsArray(PenaltyDistance, Qb)
        '            Call Neighbourhood_Crowding_Distance(Distanceb, Qb)
        '        End If

        '        'End If

        '        EsEltern = True
        '        Exit Sub

        'ES_ELTERN_ERROR:
        '        Exit Sub

    End Sub

    'A: Non_Dominated_Sorting
    'Entscheidet welche Werte dominiert werden und welche nicht
    '*******************************************************************************
    Private Sub Non_Dominated_Sorting(ByRef NDSorting() As Struct_NDSorting, ByRef Durchlauf_Front As Short)

        Dim j, i, k As Short
        Dim Logical As Boolean
        Dim Summe_Constrain(2) As Double

        'If Eigenschaft.NConstrains > 0 Then
        '    For i = 1 To UBound(NDSorting)
        '        For j = 1 To UBound(NDSorting)
        '            If NDSorting(i).Feasible And Not NDSorting(j).Feasible Then
        '                If NDSorting(j).dominated = False Then NDSorting(j).dominated = True
        '            ElseIf ((Not NDSorting(i).Feasible) And (Not NDSorting(j).Feasible)) Then
        '                Summe_Constrain(1) = 0
        '                Summe_Constrain(2) = 0
        '                For k = 1 To Eigenschaft.NConstrains
        '                    If NDSorting(i).Constrain(k) < 0 Then
        '                        Summe_Constrain(1) = Summe_Constrain(1) + NDSorting(i).Constrain(k)
        '                    End If
        '                    If NDSorting(j).Constrain(k) < 0 Then
        '                        Summe_Constrain(2) = Summe_Constrain(2) + NDSorting(j).Constrain(k)
        '                    End If
        '                Next k
        '                If Summe_Constrain(1) > Summe_Constrain(2) Then
        '                    If NDSorting(j).dominated = False Then NDSorting(j).dominated = True
        '                End If
        '            ElseIf (NDSorting(i).Feasible And NDSorting(j).Feasible) Then
        '                Logical = False
        '                For k = 1 To Eigenschaft.NPenalty
        '                    Logical = Logical Or (NDSorting(i).penalty(k) < NDSorting(j).penalty(k))
        '                Next k
        '                For k = 1 To Eigenschaft.NPenalty
        '                    Logical = Logical And (NDSorting(i).penalty(k) <= NDSorting(j).penalty(k))
        '                Next k
        '                If Logical Then
        '                    If NDSorting(j).dominated = False Then NDSorting(j).dominated = True
        '                End If
        '            End If
        '        Next j
        '    Next i
        'Else
        For i = 0 To NDSorting.GetUpperBound(0)
            For j = 0 To NDSorting.GetUpperBound(0)
                Logical = False

                For k = 0 To n_Penalty - 1
                    Logical = Logical Or (NDSorting(i).Penalty(k) < NDSorting(j).Penalty(k))
                Next k

                For k = 0 To n_Penalty - 1
                    Logical = Logical And (NDSorting(i).Penalty(k) <= NDSorting(j).Penalty(k))
                Next k

                If Logical Then
                    If NDSorting(j).dominated = False Then NDSorting(j).dominated = True
                End If
            Next j
        Next i

        'End If

        For i = 0 To NDSorting.GetUpperBound(0)
            'Hier wird die Nummer der Front geschrieben
            If NDSorting(i).dominated = False Then NDSorting(i).Front = Durchlauf_Front
        Next i

    End Sub

    'B: Non_Dominated_Count_and_Sort
    'Sortiert die nicht dominanten L�sungen nach oben, die dominanten nach unten
    '*******************************************************************************
    Private Function Non_Dominated_Count_and_Sort(ByRef NDSorting() As Struct_NDSorting) As Short
        Dim i As Short
        Dim Temp() As Struct_NDSorting
        Dim counter As Short

        ReDim Temp(NDSorting.GetUpperBound(0))
        Call Dim_NDSorting_Type(Temp)

        Non_Dominated_Count_and_Sort = 0
        counter = 0

        'Die nicht dominanten L�sungen werden nach oben kopiert
        For i = 0 To NDSorting.GetUpperBound(0)
            If NDSorting(i).dominated = True Then
                Call Copy_Faksimile_NDSorting(NDSorting(i), Temp(counter))
                counter = counter + 1
            End If
        Next i

        'Zahl der dominanten wird errechnet und zur�ckgegeben
        Non_Dominated_Count_and_Sort = NDSorting.GetLength(0) - counter

        'Die dominanten L�sungen werden nach unten kopiert
        For i = 0 To NDSorting.GetUpperBound(0)
            If NDSorting(i).dominated = False Then
                Call Copy_Faksimile_NDSorting(NDSorting(i), Temp(counter))
                counter = counter + 1
            End If
        Next i

        For i = 0 To Temp.GetUpperBound(0)
            Call Copy_Faksimile_NDSorting(Temp(i), NDSorting(i))
        Next

    End Function

    'Non_Dominated_Count_and_Sort_Sekund�re_Population
    'Sortiert die nicht dominanten L�sungen nach oben, die dominanten nach unten
    'hier f�r die Sekund�re Population
    '*******************************************************************************

    'Private Function Non_Dominated_Count_and_Sort_Sekund�re_Population(ByRef NDSorting() As NDSortingType) As Short

    'End Function


    'C: Non_Dominated_Result
    'Hier wird pro durchlauf die nicht dominierte Front in NDSResult geschaufelt
    'und die bereits klassifizierten L�sungen aus Temp Array gel�scht
    '*******************************************************************************
    Private Sub Non_Dominated_Result(ByRef Temp() As Struct_NDSorting, ByRef NDSResult() As Struct_NDSorting, ByRef NFrontMember_aktuell As Short, ByRef NFrontMember_gesamt As Short)

        Dim i, Position As Short

        Position = NFrontMember_gesamt - NFrontMember_aktuell

        'In NDSResult werden die nicht dominierten L�sungen eingef�gt
        For i = Temp.GetUpperBound(0) + 1 - NFrontMember_aktuell To Temp.GetUpperBound(0)
            'NDSResult alle bisher gefundene Fronten
            Call Copy_Faksimile_NDSorting(Temp(i), NDSResult(Position))
            Position = Position + 1
        Next i

        'Die bereits klassifizierten Member werden aus dem Temp Array gel�scht
        If n_Childs + n_Parents - NFrontMember_gesamt > 0 Then

            ReDim Preserve Temp(n_Childs + n_Parents - NFrontMember_gesamt - 1)
            'Der Flag wird zur klassifizierung in der n�chsten Runde zur�ckgesetzt
            For i = 0 To Temp.GetUpperBound(0)
                Temp(i).dominated = False
            Next i
        End If

    End Sub

    'Count_Front_Members
    '*******************************************************************************
    Private Function Count_Front_Members(ByVal aktuell_Front As Short, ByRef NDSResult() As Struct_NDSorting) As Integer
        Dim i As Short

        Count_Front_Members = 0

        For i = 0 To NDSResult.GetUpperBound(0)
            If NDSResult(i).Front = aktuell_Front Then
                Count_Front_Members = Count_Front_Members + 1
            End If
        Next i

    End Function

    'NDS_Crowding_Distance_Sort
    '*******************************************************************************

    Private Sub NDS_Crowding_Distance_Sort(ByRef NDSorting() As Struct_NDSorting, ByRef start As Short, ByRef ende As Short)
        Dim i As Integer
        Dim j As Integer
        Dim k As Short

        Dim swap(0) As Struct_NDSorting
        call Dim_NDSorting_Type(swap)

        Dim fmin, fmax As Double

        For k = 0 To n_Penalty - 1
            For i = start To ende
                For j = start To ende
                    If NDSorting(i).Penalty(k) < NDSorting(j).Penalty(k) Then
                        call Copy_Faksimile_NDSorting(NDSorting(i),swap(0))
                        call Copy_Faksimile_NDSorting(NDSorting(j),NDSorting(i))
                        call Copy_Faksimile_NDSorting(swap(0),NDSorting(j))
                    End If
                Next j
            Next i

            fmin = NDSorting(start).Penalty(k)
            fmax = NDSorting(ende).Penalty(k)

            NDSorting(start).Distance = 1.0E+300
            NDSorting(ende).Distance = 1.0E+300

            For i = start + 1 To ende - 1
                NDSorting(i).Distance = NDSorting(i).Distance + (NDSorting(i + 1).Penalty(k) - NDSorting(i - 1).Penalty(k)) / (fmax - fmin)
            Next i
        Next k

        For i = start To ende
            For j = start To ende
                If NDSorting(i).Distance > NDSorting(j).Distance Then
                    Call Copy_Faksimile_NDSorting(NDSorting(i), swap(0))
                    Call Copy_Faksimile_NDSorting(NDSorting(j), NDSorting(i))
                    Call Copy_Faksimile_NDSorting(swap(0), NDSorting(j))
                End If
            Next j
        Next i

    End Sub

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

        ReDim TempDistance(n_Penalty - 1)
        ReDim PenaltyDistance(n_Parents - 1, n_Parents - 1)
        ReDim d(n_Parents - 1)

        'Bestimmen der normierten Raumabst�nde zwischen allen Elternindividuen
        For i = 0 To n_Parents - 1
            PenaltyDistance(i, i) = 0
            For j = i + 1 To n_Parents - 1
                PenaltyDistance(i, j) = 0
                For k = 0 To n_Penalty - 1
                    TempDistance(k) = List_Parents(i).Penalty(k) - List_Parents(j).Penalty(k)
                    TempDistance(k) = TempDistance(k) * TempDistance(k)
                    PenaltyDistance(i, j) = PenaltyDistance(i, j) + TempDistance(k)
                Next k
                PenaltyDistance(i, j) = System.Math.Sqrt(PenaltyDistance(i, j))
                PenaltyDistance(j, i) = PenaltyDistance(i, j)
            Next j
        Next i

        d_mean = 0

        For i = 0 To n_Parents - 2
            d(i) = 1.0E+300
            For j = 0 To i - 1
                If PenaltyDistance(i, j) < d(i) Then d(i) = PenaltyDistance(i, j)
            Next j
            For j = i + 1 To n_Parents - 1
                If PenaltyDistance(i, j) < d(i) Then d(i) = PenaltyDistance(i, j)
            Next j
            d_mean = d_mean + d(i)
        Next i

        d_mean = d_mean / n_Parents

        NDS_Crowding_Distance_Count = 0

        For i = 0 To n_Parents - 2
            NDS_Crowding_Distance_Count = NDS_Crowding_Distance_Count + (d_mean - d(i)) * (d_mean - d(i))
        Next i

        NDS_Crowding_Distance_Count = NDS_Crowding_Distance_Count / n_Parents

        NDS_Crowding_Distance_Count = System.Math.Sqrt(NDS_Crowding_Distance_Count)

        Spannweite = 0
        For i = 0 To n_Parents - 1
            'TODO: sollte hier nicht j = i + 1 stehen?
            For j = i To n_Parents - 1
                If PenaltyDistance(i, j) > Spannweite Then Spannweite = PenaltyDistance(i, j)
            Next j
        Next i

    End Function

#End Region 'Methoden

End Class


