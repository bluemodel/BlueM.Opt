Public Class CES

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse CES Kombinatorische Evolutionsstrategie                        ****
    '****                                                                       ****
    '**** Christoph Hübner                                                      ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '****                                                                       ****
    '**** Februar 2007                                                          ****
    '****                                                                       ****
    '**** Letzte Änderung: März 2007                                            ****
    '*******************************************************************************
    '*******************************************************************************

    'Public Variablen
    Public n_Location As Integer       'Anzahl der Locations wird von außen gesetzt
    Public n_Penalty As Integer           'Anzahl der Ziele wird von außen gesetzt
    Public n_Verzweig As Integer        'Anzahl der Verzweigungen in der Verzweigungsdatei
    Public n_PathSize() As Integer     'Anzahl der "Städte/Maßnahmen" an jeder Stelle
    Public n_Generation As Integer = 10

    'Private Variablen
    Private n_Parents As Integer = 3
    Private n_Childs As Integer = 10

    Private ReprodOperator_TSP As String = "Order_Crossover_OX"
    Private ReprodOperator_BM As String = "Select_Random_Uniform"
    Private MutOperator_TSP As String = "Translocation"
    Private MutOperator_BM As String = "Random_Switch"
    Private Strategy As String = "plus"         '"plus" oder "minus" Strategie
    Private MutRate As Integer = 30              'Definiert die Wahrscheinlichkeit der Mutationsrate in %

    '************************************* Struktur *****************************
    Public Structure Faksimile_Type
        Dim No As Short
        Dim Path() As Integer      'auf 0 ist die Stadt, auf 1 die Anzahl der Städte
        Dim Penalty_SO As Double    'HACK zum Ausgleich von MO und SO
        Dim Penalty_MO() As Double
        Dim Front As Short
    End Structure

    '************************************* Listen ******************************
    Public ChildList() As Faksimile_Type
    Public ParentList() As Faksimile_Type

    '******************************** NDSorting Struktur **************************
    Public Structure NDSortingType
        Dim Penalty_MO() As Double             'DM: Werte der Penaltyfunktion(en)
        Dim Constrain() As Double           'DM: Werte der Randbedingung(en)
        Dim Feasible As Boolean             'DM: Gültiges Ergebnis ?
        Dim dominated As Boolean            'DM: Kennzeichnung ob dominiert
        Dim Front As Short                  'DM: Nummer der Pareto Front
        Dim Distance As Double              'DM: Distanzwert für Crowding distance sort
        Dim No As Short                     'CH: Nummer des Faksimile
        Dim Path() As Integer               'CH: Der Pfad
    End Structure

    Dim NDSorting() As NDSortingType ' NDSorting Liste ***************************


    '*********************************** Programm ******************************************

    'Dimensionieren des ChildStructs
    Public Sub Dim_Childs()
        Dim i, j As Integer
        ReDim ChildList(n_Childs - 1)

        For i = 0 To n_Childs - 1
            ChildList(i).No = i + 1
            ChildList(i).Penalty_SO = 999999999999999999

            ReDim ChildList(i).Penalty_MO(n_Penalty - 1)
            For j = 0 To n_Penalty - 1
                ChildList(i).Penalty_MO(j) = 999999999999999999
            Next
            ReDim ChildList(i).Path(n_Location - 1)
        Next

    End Sub

    'Dimensionieren des ParentStructs
    Public Sub Dim_Parents_BM()
        Dim i, j As Integer

        ReDim ParentList(n_Parents - 1)

        For i = 0 To n_Parents - 1
            ParentList(i).No = i + 1
            ParentList(i).Penalty_SO = 999999999999999999
            ReDim ParentList(i).Penalty_MO(n_Penalty - 1)
            For j = 0 To n_Penalty - 1
                ParentList(i).Penalty_MO(j) = 999999999999999999
            Next
            ReDim ParentList(i).Path(n_Location - 1)
        Next

    End Sub

    'Dimensionieren des NDSortingStructs BM Problem
    Public Sub Dim_NDSorting_Type(ByRef TMP() As NDSortingType)
        Dim i, j As Integer

        For i = 0 To TMP.GetUpperBound(0)
            TMP(i).No = 0
            'NDSorting(i).Penalty_SO = 999999999999999999
            ReDim TMP(i).Penalty_MO(n_Penalty - 1)
            For j = 0 To n_Penalty - 1
                TMP(i).Penalty_MO(j) = 999999999999999999
            Next
            ReDim TMP(i).Path(n_Location - 1)
        Next

    End Sub

    'Generiert zufällige Paths für alle Kinder BM Problem
    Public Sub Generate_Random_Path()
        Dim i, j As Integer
        Dim tmp As Integer
        Dim lowerb As Integer = 0
        Dim upperb As Integer
        Randomize()

        For i = 0 To n_Childs - 1
            For j = 0 To n_Location - 1
                upperb = n_PathSize(j) - 1
                'Randomize() nicht vergessen
                tmp = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
                ChildList(i).Path(j) = tmp
            Next
        Next

    End Sub

    'HACK
    'Funktion zum manuellen Testen der Paths in der ersten Generation
    Public Sub Generate_Test_Path()
        Dim i, j As Integer
        Dim Grenze As Integer

        'Achtung n_Childs sollte Größer als die Möglichen Kombinationen an einer Stelle sein
        For i = 0 To n_Childs - 1
            For j = 0 To n_Location - 1
                Grenze = n_PathSize(j) - 1
                If i <= Grenze Then
                    ChildList(i).Path(j) = i
                Else
                    ChildList(i).Path(j) = 0
                End If
            Next
        Next

    End Sub

    'HACK
    'Funktion zum manuellen Testen der Paths in der ersten Generation
    Public Sub Generate_All_Test_Path()
        Dim i As Integer
        Dim x, y, z As Integer

        x = 0
        y = 0
        z = 0

        For i = 0 To n_Childs - 1

            ChildList(i).Path(0) = x
            ChildList(i).Path(1) = y
            ChildList(i).Path(2) = z
            x += 1
            If x > n_PathSize(0) - 1 Then
                x = 0
                y += 1
            End If
            If y > n_PathSize(1) - 1 Then
                y = 0
                z += 1
            End If
            If z > n_PathSize(2) - 1 Then
                z = 0
            End If

        Next

    End Sub

    '*************************** Functionen innerhalb der Generationsschleife ****************************

    'Selectionsprozess je nach "plus" oder "minus" Strategie
    Public Sub Selection_Process_BM()
        Dim i, j As Integer
        Dim ChildQ_TMP As Double = 0
        Dim ParentQ_TMP As Double = 0

        If Strategy = "minus" Then
            For i = 0 To n_Parents - 1
                ParentList(i).Penalty_SO = ChildList(i).Penalty_SO
                Array.Copy(ChildList(i).Path, ParentList(i).Path, ChildList(i).Path.Length)
            Next i

        ElseIf Strategy = "plus" Then
            j = 0
            For i = 0 To n_Parents - 1
                If ParentList(i).Penalty_SO < ChildList(j).Penalty_SO Then
                    j -= 1
                Else
                    ParentList(i).Penalty_SO = ChildList(j).Penalty_SO
                    ParentList(i).Penalty_MO = ChildList(j).Penalty_MO 'HACK: hier Qualität Doppelt
                    Array.Copy(ChildList(j).Path, ParentList(i).Path, ChildList(j).Path.Length)
                End If
                j += 1
            Next i
        End If

    End Sub

    'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
    Public Sub Reset_Childs_BM()
        Dim i, j As Integer

        For i = 0 To n_Childs - 1
            ChildList(i).No = i + 1
            ChildList(i).Penalty_SO = 999999999999999999
            For j = 0 To ChildList(i).Penalty_MO.GetUpperBound(0)
                ChildList(i).Penalty_MO(j) = 999999999999999999
            Next
            Array.Clear(ChildList(i).Path, 0, ChildList(i).Path.GetLength(0))
        Next

    End Sub

    '**************************************** Reproductionsfunktionen ****************************************

    'Steuerung der Reproduktionsoperatoren
    Public Sub Reproduction_Control_BM()
        Dim i As Integer
        Dim x, y As Integer
        Dim Einzelkind(n_Location - 1) As Integer

        Select Case ReprodOperator_BM
            'ToDo: Eltern werden nicht zufällig gewählt sondern immer in Top Down Reihenfolge
            Case "Select_Random_Uniform"
                x = 0
                y = 1
                For i = 0 To n_Childs - 2 Step 2
                    Call ReprodOp_Select_Random_Uniform(ParentList(x).Path, ParentList(y).Path, ChildList(i).Path, ChildList(i + 1).Path)
                    x += 1
                    y += 1
                    If x = n_Parents - 1 Then x = 0
                    If y = n_Parents - 1 Then y = 0
                Next i
                If Even_Number(n_Childs) = False Then
                    Call ReprodOp_Select_Random_Uniform(ParentList(x).Path, ParentList(y).Path, ChildList(n_Childs - 1).Path, Einzelkind)
                End If
        End Select

    End Sub

    'Reproductionsoperator: "Select_Random_Uniform"
    'Entscheidet zufällig welcher ob der Wert aus dem Path des Elter_A oder Elter_B verwendet wird
    Private Sub ReprodOp_Select_Random_Uniform(ByVal ParPath_A() As Integer, ByVal ParPath_B() As Integer, ByRef ChildPath_A() As Integer, ByRef ChildPath_B() As Integer)

        Dim i As Integer

        For i = 0 To ChildPath_A.GetUpperBound(0)    'ToDo: Es müsste eigentlich eine definierte Pfadlänge geben
            If Bernoulli() = True Then
                ChildPath_A(i) = ParPath_B(i)
            Else
                ChildPath_A(i) = ParPath_A(i)
            End If
        Next

        For i = 0 To ChildPath_B.GetUpperBound(0)    'ToDo: Es müsste eigentlich eine definierte Pfadlänge geben
            If Bernoulli() = True Then
                ChildPath_B(i) = ParPath_A(i)
            Else
                ChildPath_B(i) = ParPath_B(i)
            End If
        Next

    End Sub


    '****************************************** Mutationsfunktionen ****************************************

    'Steuerung der Mutationsoperatoren
    Public Sub Mutation_Control_BM()
        Dim i As Integer

        Select Case MutOperator_BM
            Case "Random_Switch"
                For i = 0 To n_Childs - 1
                    Call MutOp_RND_Switch(ChildList(i).Path)
                    'If PathValid(ChildList(i).Path) = False Then MsgBox("Fehler im Path", MsgBoxStyle.Information, "Fehler")
                Next i
        End Select

    End Sub

    'Mutationsoperator "Random_Switch"
    'Verändert zufällig ein gen des Paths
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
                upperb_b = n_PathSize(i) - 1
                'Randomize() nicht vergessen
                Tmp_b = CInt(Int((upperb_b - lowerb_b + 1) * Rnd() + lowerb_b))
                Path(i) = Tmp_b
            End If
        Next

    End Sub

    '******************************************* Hilfsfunktionen *******************************************

    'Hilfsfunktion um zu Prüfen ob eine Zahl bereits in einem Array vorhanden ist oder nicht
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
    Public Sub Sort_Faksimile(ByRef FaksimileList() As Faksimile_Type)
        'Sortiert die Fiksimile anhand des Abstandes
        Dim i, j As Integer
        Dim swap As EvoKern.CES.Faksimile_Type

        For i = 0 To FaksimileList.GetUpperBound(0)
            For j = 0 To FaksimileList.GetUpperBound(0)
                If FaksimileList(i).Penalty_SO < FaksimileList(j).Penalty_SO Then
                    swap = FaksimileList(i)
                    FaksimileList(i) = FaksimileList(j)
                    FaksimileList(j) = swap
                End If
            Next j
        Next i

    End Sub

    'Hilfsfunktion generiert Bernoulli verteilte Zufallszahl
    Public Function Bernoulli() As Boolean
        Dim lowerb As Integer = 0
        Dim upperbo As Integer = 1
        Bernoulli = CInt(Int(2 * Rnd()))
    End Function

    'Hilfsfunktion: Gerade oder Ungerade Zahl
    Public Function Even_Number(ByVal Number As Integer) As Boolean
        Dim tmp_a As Double
        Dim tmp_b As Double
        Dim tmp_c As Double
        tmp_a = Number / 2
        tmp_b = Math.Ceiling(tmp_a)
        tmp_c = tmp_a - tmp_b
        If tmp_c = 0 Then Even_Number = True
    End Function

    'HACK zur Reduzierung auf eine Zielfunktion
    Public Sub MO_TO_SO(ByRef FaksimileList As Faksimile_Type)
        Dim x As Integer
        FaksimileList.Penalty_SO = 0
        For x = 0 To FaksimileList.Penalty_MO.GetUpperBound(0)
            FaksimileList.Penalty_SO = FaksimileList.Penalty_SO + FaksimileList.Penalty_MO(x)
        Next
        'FaksimileList.Quality_SO = FaksimileList.Quality_SO / n_Ziele

    End Sub

    '                          Steuerung des NDSorting
    '********************************************************************************

    Public Sub NDSorting_Control()
        Dim i As Short
        Dim NFrontMember_aktuell, NFrontMember_gesamt As Short
        Dim Durchlauf_Front As Short
        'Dim Count as short
        Dim aktuelle_Front As Short
        'Dim Member_Sekundärefront As Short

        ReDim NDSorting(n_Childs + n_Parents - 1)
        Call Dim_NDSorting_Type(NDSorting)

        '0. Eltern und Nachfolger werden gemeinsam betrachtet
        'Die Kinder werden NDSorting hinzugefügt
        '-------------------------------------------

        For i = 0 To n_Childs - 1
            With NDSorting(i)

                ''NConstrains ********************************
                'If Eigenschaft.NConstrains > 0 Then
                '    .Feasible = True
                '    For l = 1 To Eigenschaft.NConstrains
                '        .Constrain(l) = Rb(m - Eigenschaft.NNachf, Eigenschaft.iaktuellePopulation, l)
                '        If .Constrain(l) < 0 Then .Feasible = False
                '    Next l
                'End If

                Array.Copy(ChildList(i).Penalty_MO, .Penalty_MO, .Penalty_MO.GetLength(0))
                .dominated = False
                .Front = 0
                .Distance = 0
                Array.Copy(ChildList(i).Path, .Path, .Path.GetLength(0))
            End With
        Next i

        '1. Eltern und Nachfolger werden gemeinsam betrachtet
        'Die Eltern werden NDSorting hinzugefügt
        '-------------------------------------------

        For i = n_Childs To n_Childs + n_Parents - 1
            With NDSorting(i)

                ''NConstrains ********************************
                'If Eigenschaft.NConstrains > 0 Then
                '    .Feasible = True
                '    For l = 1 To Eigenschaft.NConstrains
                '        .Constrain(l) = Rb(m - Eigenschaft.NNachf, Eigenschaft.iaktuellePopulation, l)
                '        If .Constrain(l) < 0 Then .Feasible = False
                '    Next l
                'End If

                array.Copy(parentlist(i - n_Childs).Penalty_MO, .Penalty_MO, .Penalty_MO.GetLength(0))
                .dominated = False
                .Front = 0
                .Distance = 0
                array.Copy(parentlist(i - n_Childs).Path, .Path, .Path.GetLength(0))
            End With
        Next i

        '2. Die einzelnen Fronten werden bestimmt
        '----------------------------------------
        Durchlauf_Front = 1
        NFrontMember_gesamt = 0

        'Initialisierung von Temp (NDSorting)
        Dim Temp(n_Childs + n_Parents - 1) As NDSortingType
        Call Dim_NDSorting_Type(Temp)

        'Initialisierung von NDSResult (NDSorting)
        Dim NDSResult(n_Childs + n_Parents - 1) As NDSortingType
        Call Dim_NDSorting_Type(NDSResult)

        'NDSorting wird in Temp kopiert
        Array.Copy(NDSorting, Temp, NDSorting.GetLength(0))

        'Schleife läuft über die Zahl der Fronten die hier auch bestimmte werden
        Do
            'A: Entscheidet welche Werte dominiert werden und welche nicht
            Call Non_Dominated_Sorting(Temp, Durchlauf_Front) 'aktuallisiert auf n Objectives dm 10.05.05

            'B: Sortiert die nicht dominanten Lösungen nach oben,
            'die dominanten nach unten und zählt die Mitglieder der aktuellen Front
            NFrontMember_aktuell = Non_Dominated_Count_and_Sort(Temp)
            'NFrontMember_aktuell: Anzahl der Mitglieder der gerade bestimmten Front
            'NFrontMember_gesamt: Alle bisher als nicht dominiert klassifizierten Individuum
            NFrontMember_gesamt = NFrontMember_gesamt + NFrontMember_aktuell

            'C: Hier wird pro durchlauf die nicht dominierte Front in NDSResult geschaufelt
            'und die bereits klassifizierten Lösungen aus Temp Array gelöscht
            Call Non_Dominated_Result(Temp, NDSResult, NFrontMember_aktuell, NFrontMember_gesamt)
            'Durchlauf ist hier die Nummer der Front
            Durchlauf_Front = Durchlauf_Front + 1
        Loop While Not (NFrontMember_gesamt = n_Parents + n_Childs)

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
            If NFrontMember_aktuell <= n_Parents - NFrontMember_gesamt Then
                'ToDo: Grenzen müssen überprüft werden
                For i = NFrontMember_gesamt + 1 To NFrontMember_aktuell + NFrontMember_gesamt
                    Array.Copy(NDSResult(i).Penalty_MO, ParentList(i).Penalty_MO, n_Penalty)
                    Array.Copy(NDSResult(i).Path, ParentList(i).Path, n_Location)
                    ParentList(i).No = NDSResult(i).No
                    ParentList(i).Front = NDSResult(i).Front
                Next i
                NFrontMember_gesamt = NFrontMember_gesamt + NFrontMember_aktuell

            Else
                'Es sind weniger Elterplätze für die nächste Generation verfügber
                'als Mitglieder der aktuellen Front. Nur für diesen Rest wird crowding distance
                'gemacht um zu bestimmen wer noch mitspielen darf und wer noch a biserl was druff hat
                Call NDS_Crowding_Distance_Sort(NDSResult, NFrontMember_gesamt, NFrontMember_gesamt + NFrontMember_aktuell - 1)

                For i = NFrontMember_gesamt To n_Parents - 1
                    Array.Copy(NDSResult(i).Penalty_MO, ParentList(i).Penalty_MO, n_Penalty)
                    Array.Copy(NDSResult(i).Path, ParentList(i).Path, n_Location)
                    ParentList(i).No = NDSResult(i).No
                    ParentList(i).Front = NDSResult(i).Front
                Next i

                NFrontMember_gesamt = n_Parents

            End If

            aktuelle_Front = aktuelle_Front + 1

        Loop While Not (NFrontMember_gesamt = n_Parents)

        '        '4: Sekundäre Population wird bestimmt und gespeichert
        '        NFrontMember_aktuell = Count_Front_Members(1, NDSResult)

        '        If Eigenschaft.iaktuelleRunde = 1 And Eigenschaft.iaktuellePopulation = 1 And Eigenschaft.iaktuelleGeneration = 1 Then
        '            ReDim Preserve SekundärQb(Member_Sekundärefront + NFrontMember_aktuell)
        '        Else
        '            Member_Sekundärefront = UBound(SekundärQb)
        '            ReDim Preserve SekundärQb(Member_Sekundärefront + NFrontMember_aktuell)
        '        End If

        '        For i = Member_Sekundärefront + 1 To Member_Sekundärefront + NFrontMember_aktuell
        '            SekundärQb(i) = NDSResult(i - Member_Sekundärefront)
        '        Next i

        '        Call Non_Dominated_Sorting(SekundärQb, 1)
        '        NFrontMember_aktuell = Non_Dominated_Count_and_Sort_Sekundäre_Population(SekundärQb)
        '        ReDim Preserve SekundärQb(NFrontMember_aktuell)
        '        Call SekundärQb_Duplettten(SekundärQb)
        '        NFrontMember_aktuell = Non_Dominated_Count_and_Sort_Sekundäre_Population(SekundärQb)
        '        ReDim Preserve SekundärQb(NFrontMember_aktuell)

        '        If UBound(SekundärQb) > Eigenschaft.NMemberSecondPop Then
        '            Call NDS_Crowding_Distance_Sort(SekundärQb, 1, UBound(SekundärQb))
        '            ReDim Preserve SekundärQb(Eigenschaft.NMemberSecondPop)
        '        End If

        '        If (Eigenschaft.iaktuelleGeneration Mod Eigenschaft.interact) = 0 And Eigenschaft.isInteract Then
        '            NFrontMember_aktuell = Count_Front_Members(1, SekundärQb)
        '            If NFrontMember_aktuell > Eigenschaft.NEltern Then
        '                Call NDS_Crowding_Distance_Sort(SekundärQb, 1, UBound(SekundärQb))
        '                For i = 1 To Eigenschaft.NEltern

        '                    For j = 1 To Eigenschaft.NPenalty
        '                        Qb(i, Eigenschaft.iaktuellePopulation, j) = SekundärQb(i).penalty(j)
        '                    Next j
        '                    If Eigenschaft.NConstrains > 0 Then
        '                        For j = 1 To Eigenschaft.NConstrains
        '                            Rb(i, Eigenschaft.iaktuellePopulation, j) = SekundärQb(i).constrain(j)
        '                        Next j
        '                    End If
        '                    For v = 1 To Eigenschaft.varanz
        '                        Db(v, i, Eigenschaft.iaktuellePopulation) = SekundärQb(i).d(v)
        '                        Xb(v, i, Eigenschaft.iaktuellePopulation) = SekundärQb(i).X(v)
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

        '        'Sortierung der Lösungen ist nur für Neighbourhood-Rekombination notwendig
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


    '*******************************************************************************
    'A: Non_Dominated_Sorting
    'Entscheidet welche Werte dominiert werden und welche nicht
    '*******************************************************************************
    Private Sub Non_Dominated_Sorting(ByRef NDSorting() As NDSortingType, ByRef Durchlauf_Front As Short)

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
                    Logical = Logical Or (NDSorting(i).Penalty_MO(k) < NDSorting(j).Penalty_MO(k))
                Next k

                For k = 0 To n_Penalty - 1
                    Logical = Logical And (NDSorting(i).Penalty_MO(k) <= NDSorting(j).Penalty_MO(k))
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


    '*******************************************************************************
    'B: Non_Dominated_Count_and_Sort
    'Sortiert die nicht dominanten Lösungen nach oben, die dominanten nach unten
    '*******************************************************************************

    Private Function Non_Dominated_Count_and_Sort(ByRef NDSorting() As NDSortingType) As Short
        Dim i As Short
        Dim Temp() As NDSortingType
        Dim counter As Short

        ReDim Temp(NDSorting.GetUpperBound(0))
        Call Dim_NDSorting_Type(Temp)

        Non_Dominated_Count_and_Sort = 0
        counter = 0

        'Die nicht dominanten Lösungen werden nach oben kopiert
        For i = 0 To NDSorting.GetUpperBound(0)
            If NDSorting(i).dominated = True Then
                Temp(counter) = NDSorting(i)
                counter = counter + 1
            End If
        Next i

        'Zahl der dominanten wird errechnet und zurückgegeben
        Non_Dominated_Count_and_Sort = NDSorting.GetLength(0) - counter

        'Die dominanten Lösungen werden nach unten kopiert
        For i = 0 To NDSorting.GetUpperBound(0)
            If NDSorting(i).dominated = False Then
                Temp(counter) = NDSorting(i)
                counter = counter + 1
            End If
        Next i

        Array.Copy(Temp, NDSorting, NDSorting.GetLength(0))

    End Function


    '*******************************************************************************
    'Non_Dominated_Count_and_Sort_Sekundäre_Population
    'Sortiert die nicht dominanten Lösungen nach oben, die dominanten nach unten
    'hier für die Sekundäre Population
    '*******************************************************************************

    'Private Function Non_Dominated_Count_and_Sort_Sekundäre_Population(ByRef NDSorting() As NDSortingType) As Short


    'End Function


    '*******************************************************************************
    'C: Non_Dominated_Result
    'Hier wird pro durchlauf die nicht dominierte Front in NDSResult geschaufelt
    'und die bereits klassifizierten Lösungen aus Temp Array gelöscht
    '*******************************************************************************

    Private Sub Non_Dominated_Result(ByRef Temp() As NDSortingType, ByRef NDSResult() As NDSortingType, ByRef NFrontMember_aktuell As Short, ByRef NFrontMember_gesamt As Short)

        Dim i, Position As Short

        Position = NFrontMember_gesamt - NFrontMember_aktuell

        'In NDSResult werden die nicht dominierten Lösungen eingefügt
        For i = Temp.GetUpperBound(0)+1 - NFrontMember_aktuell To Temp.GetUpperBound(0)
            'NDSResult alle bisher gefundene Fronten
            NDSResult(Position) = Temp(i)
            Position = Position + 1
        Next i

        'Die bereits klassifizierten Member werden aus dem Temp Array gelöscht
        If n_Childs + n_Parents - NFrontMember_gesamt > 0 Then

            ReDim Preserve Temp(n_Childs + n_Parents - NFrontMember_gesamt - 1)
            'Der Flag wird zur klassifizierung in der nächsten Runde zurückgesetzt
            For i = 0 To Temp.GetUpperBound(0)
                Temp(i).dominated = False
            Next i
        End If

    End Sub

    '*******************************************************************************
    'Count_Front_Members
    '*******************************************************************************

    Private Function Count_Front_Members(ByVal aktuell_Front As Short, ByRef NDSResult() As NDSortingType) As Integer
        Dim i As Short

        Count_Front_Members = 0

        For i = 0 To NDSResult.GetUpperBound(0)
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

        Dim swap As NDSortingType
        ReDim swap.Penalty_MO(n_Penalty - 1)
        ReDim swap.Path(n_Location - 1)

        Dim fmin, fmax As Double

        For k = 0 To n_Penalty - 1
            For i = start To ende
                For j = start To ende
                    If NDSorting(i).Penalty_MO(k) < NDSorting(j).Penalty_MO(k) Then
                        swap = NDSorting(i)
                        NDSorting(i) = NDSorting(j)
                        NDSorting(j) = swap
                    End If
                Next j
            Next i

            fmin = NDSorting(start).Penalty_MO(k)
            fmax = NDSorting(ende).Penalty_MO(k)

            NDSorting(start).Distance = 1.0E+300
            NDSorting(ende).Distance = 1.0E+300

            For i = start + 1 To ende - 1
                NDSorting(i).Distance = NDSorting(i).Distance + (NDSorting(i + 1).Penalty_MO(k) - NDSorting(i - 1).Penalty_MO(k)) / (fmax - fmin)
            Next i
        Next k

        For i = start To ende
            For j = start To ende
                If NDSorting(i).Distance > NDSorting(j).Distance Then
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

        ReDim TempDistance(n_Penalty - 1)
        ReDim PenaltyDistance(n_Parents - 1, n_Parents - 1)
        ReDim d(n_Parents - 1)

        'Bestimmen der normierten Raumabstände zwischen allen Elternindividuen
        For i = 0 To n_Parents - 1
            PenaltyDistance(i, i) = 0
            For j = i + 1 To n_Parents - 1
                PenaltyDistance(i, j) = 0
                For k = 0 To n_Penalty - 1
                    TempDistance(k) = ParentList(i).Penalty_MO(k) - ParentList(j).Penalty_MO(k)
                    TempDistance(k) = TempDistance(k) * TempDistance(k)
                    PenaltyDistance(i, j) = PenaltyDistance(i, j) + TempDistance(k)
                Next k
                PenaltyDistance(i, j) = System.Math.Sqrt(PenaltyDistance(i, j))
                PenaltyDistance(j, i) = PenaltyDistance(i, j)
            Next j
        Next i

        d_mean = 0

        For i = 0 To n_parents - 2
            d(i) = 1.0E+300
            For j = 0 To i - 1
                If PenaltyDistance(i, j) < d(i) Then d(i) = PenaltyDistance(i, j)
            Next j
            For j = i + 1 To n_Parents - 1
                If PenaltyDistance(i, j) < d(i) Then d(i) = PenaltyDistance(i, j)
            Next j
            d_mean = d_mean + d(i)
        Next i

        d_mean = d_mean / n_parents

        NDS_Crowding_Distance_Count = 0

        For i = 0 To n_Parents - 2
            NDS_Crowding_Distance_Count = NDS_Crowding_Distance_Count + (d_mean - d(i)) * (d_mean - d(i))
        Next i

        NDS_Crowding_Distance_Count = NDS_Crowding_Distance_Count / n_parents

        NDS_Crowding_Distance_Count = System.Math.Sqrt(NDS_Crowding_Distance_Count)

        Spannweite = 0
        For i = 0 To n_Parents - 1
            'ToDo : sollte hier nicht j = i + 1 stehen?
            For j = i To n_Parents - 1
                If PenaltyDistance(i, j) > Spannweite Then Spannweite = PenaltyDistance(i, j)
            Next j
        Next i

    End Function

End Class


