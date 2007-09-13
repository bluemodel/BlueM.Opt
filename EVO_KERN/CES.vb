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

#Region "Eigenschaften"
    '##################

    'Public Variablen
    Public TestModus As Integer             'Gibt den Testmodus an
    Public n_Combinations As Integer        'Anzahl aller Kombinationen
    Public n_Locations As Integer           'Anzahl der Locations wird von außen gesetzt
    Public n_Penalty As Integer             'Anzahl der Ziele wird von außen gesetzt
    Public n_Constrain as integer           'Anzahl der Rabdbedingungen
    Public n_Verzweig As Integer            'Anzahl der Verzweigungen in der Verzweigungsdatei
    Public n_PathDimension() As Integer     'Anzahl der Maßnahmen an jeder Stelle

    'Eingabe
    Public n_Generations As Integer = 2     'Anzahl der Generationen
    Public n_Parts_of_Path As Integer = 3   'Länge des Gedächtnispfades Achtung Maximum ist 3
    Public n_Parents As Integer = 3
    Public n_Childs As Integer = 5

    'Private Variablen
    Private ReprodOperator As String = "Select_Random_Uniform"
    Private MutOperator As String = "RND_Switch"
    Private Strategy As String = "plus"         '"plus" oder "minus" Strategie
    Private MutRate As Integer = 10              'Definiert die Wahrscheinlichkeit der Mutationsrate in %

    'Faksimile Struktur
    '******************
    Public Structure Struct_Faksimile
        Dim No As Short                     'Nummer des NDSorting
        Dim Path() As Integer               'Der Pfad
        Dim Penalty() As Double             'Werte der Penaltyfunktion(en)
        Dim Constrain() As Double           'Wert der Randbedingung(en)
        Dim mutated As Boolean              'Gibt an ob der Wert bereits mutiert ist oder nicht
        Dim Front As Short                  'Nummer der Pareto Front
        Dim myPara(,) As Object             'Die Optimierungsparameter
    End Structure

    Public List_Childs() As Struct_Faksimile
    Public List_Parents() As Struct_Faksimile

    'NDSorting Struktur
    '******************
    Public Structure Struct_NDSorting
        Dim No As Short                     'CH: Nummer des NDSorting
        Dim Path() As Integer               'CH: Der Pfad
        Dim Penalty() As Double             'DM: Werte der Penaltyfunktion(en)
        Dim Constrain() As Double          'DM: Werte der Randbedingung(en)
        'Dim Feasible As Boolean            'DM: Gültiges Ergebnis ?
        Dim dominated As Boolean            'DM: Kennzeichnung ob dominiert
        Dim Front As Short                  'DM: Nummer der Pareto Front
        Dim Distance As Double              'DM: Distanzwert für Crowding distance sort
    End Structure

    Public NDSorting() As Struct_NDSorting
    Public NDSResult(n_Childs + n_Parents - 1) As Struct_NDSorting

    'Memory zum Speichern der PES Parametersätze
    '*******************************************
    Public Structure Struct_PES_Memory
        Dim Path() As Integer
        Dim Parameter(,) As Object
        Dim D() As Object
        Dim Penalty() As Double
        Dim Constrain() As Double
        Dim Generation as Integer
    End Structure

    Public Memory() As Struct_PES_Memory

    Public Structure Struct_PES_Parent
        Dim Memory_Rank As Integer
        Dim Path() As Integer
        Dim Parameter(,) As Object
        Dim D() As Object
        Dim Penalty() As Double
        Dim iLocation As Integer
        Dim Generation As Integer
    End Structure

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
            For j = 0 To n_Penalty - 1
                TMP(i).Penalty(j) = 999999999999999999
            Next
            If n_Constrain = 0 Then
                ReDim TMP(i).Constrain(-1)
            End If
            ReDim TMP(i).Path(n_Locations - 1)
        Next

        'myPara wird Dynamisch generiert
        'ReDim TMP(i).myPara(n_Parameters, 1)

    End Sub

    'Dimensionieren des NDSortingStructs
    '***********************************
    Public Sub Dim_NDSorting_Type(ByRef TMP() As Struct_NDSorting)
        Dim i, j As Integer

        For i = 0 To TMP.GetUpperBound(0)
            TMP(i).No = i + 1
            ReDim TMP(i).Path(n_Locations - 1)
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
        Array.Copy(Source.Penalty, Destination.Penalty, Source.Penalty.Length)
        If Not n_Constrain = 0 Then
            Array.Copy(Source.Constrain, Destination.Constrain, Source.Constrain.Length)
        End If
        Destination.mutated = Source.mutated
        Destination.Front = Source.Front

    End Sub

    'Kopiert ein Faksimile soweit möglich in ein NDSorting
    '*****************************************************
    Public Sub Copy_Faksimile_NDSorting(ByVal Source As Struct_Faksimile, ByRef Destination As Struct_NDSorting)

        Destination.No = Source.No
        Array.Copy(Source.Path, Destination.Path, Source.Path.Length)
        Array.Copy(Source.Penalty, Destination.Penalty, Source.Penalty.Length)
        If Not n_Constrain = 0 Then
            Array.Copy(Source.Constrain, Destination.Constrain, Source.Constrain.Length)
        End If
        Destination.Front = Source.Front

    End Sub

    'Kopiert ein NDSOrting in ein Faksimile
    '**************************************
    Public Sub Copy_Faksimile_NDSorting(ByVal Source As Struct_NDSorting, ByRef Destination As Struct_Faksimile)

        Destination.No = Source.No
        Array.Copy(Source.Path, Destination.Path, Source.Path.Length)
        Array.Copy(Source.Penalty, Destination.Penalty, Source.Penalty.Length)
        If Not n_Constrain = 0 Then
            Array.Copy(Source.Constrain, Destination.Constrain, Source.Constrain.Length)
        End If
        Destination.Front = Source.Front

    End Sub

    'Kopiert ein NDSOrting
    '*********************
    Public Sub Copy_Faksimile_NDSorting(ByVal Source As Struct_NDSorting, ByRef Destination As Struct_NDSorting)

        Destination.No = Source.No
        Array.Copy(Source.Path, Destination.Path, Source.Path.Length)
        Array.Copy(Source.Penalty, Destination.Penalty, Source.Penalty.Length)
        If Not n_Constrain = 0 Then
            Array.Copy(Source.Constrain, Destination.Constrain, Source.Constrain.Length)
        End If
        Destination.dominated = Source.dominated
        Destination.Front = Source.Front
        Destination.Distance = Source.Distance

    End Sub


    'Normaler Modus: Generiert zufällige Paths für alle Kinder BM Problem
    '*********************************************************************
    Public Sub Generate_Random_Path()
        Dim i, j As Integer
        Dim tmp As Integer
        Dim lowerb As Integer = 0
        Dim upperb As Integer
        Randomize()

        For i = 0 To n_Childs - 1
            Do
                For j = 0 To n_Locations - 1
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

    'Testmodus 2: Funktion zum testen aller Kombinationen
    '***************************************************
    Public Sub Generate_All_Test_Paths()
        Dim i, j As Integer

        Dim array() As Integer
        ReDim array(List_Childs(i).Path.GetUpperBound(0))
        For i = 0 To array.GetUpperBound(0)
            array(i) = 0
        Next

        For i = 0 To n_Childs - 1
            For j = 0 To List_Childs(i).Path.GetUpperBound(0)
                List_Childs(i).Path(j) = array(j)
            Next
            array(0) += 1
            If Not i = n_Childs - 1 Then
                For j = 0 To List_Childs(i).Path.GetUpperBound(0)
                    If array(j) > n_PathDimension(j) - 1 Then
                        array(j) = 0
                        array(j + 1) += 1
                    End If
                Next
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

    'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
    '*************************************************************
    Public Sub Reset_Childs()
        Dim i, j As Integer

        For i = 0 To n_Childs - 1
            List_Childs(i).No = i + 1
            For j = 0 To List_Childs(i).Penalty.GetUpperBound(0)
                List_Childs(i).Penalty(j) = 999999999999999999
            Next
            For j = 0 To List_Childs(i).Constrain.GetUpperBound(0)
                List_Childs(i).Constrain(j) = 0
            Next
            Array.Clear(List_Childs(i).Path, 0, List_Childs(i).Path.GetLength(0))
            List_Childs(i).mutated = False
        Next

    End Sub

    'Reproductionsfunktionen
    'XXXXXXXXXXXXXXXXXXXXXXX

    'Steuerung der Reproduktionsoperatoren
    '*************************************
    Public Sub Reproduction_Control()
        Dim i As Integer
        Dim x, y As Integer
        Dim Einzelkind(n_Locations - 1) As Integer

        Select Case ReprodOperator
            'UPGRADE: Eltern werden nicht zufällig gewählt sondern immer in Top Down Reihenfolge
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

            Case "Order_Crossover (OX)"

                x = 0
                y = 1
                For i = 0 To n_Childs - 2 Step 2
                    Call ReprodOp_Order_Crossover(List_Parents(x).Path, List_Parents(y).Path, List_Childs(i).Path, List_Childs(i + 1).Path)
                    x += 1
                    y += 1
                    If x = n_Parents - 1 Then x = 0
                    If y = n_Parents - 1 Then y = 0
                Next i
                If Even_Number(n_Childs) = False Then
                    Call ReprodOp_Order_Crossover(List_Parents(x).Path, List_Parents(y).Path, List_Childs(n_Childs - 1).Path, Einzelkind)
                End If

            Case "Partially_Mapped_Crossover"
                x = 0
                y = 1
                For i = 0 To n_Childs - 2 Step 2
                    Call ReprodOp_Part_Mapped_Crossover(List_Parents(x).Path, List_Parents(y).Path, List_Childs(i).Path, List_Childs(i + 1).Path)
                    x += 1
                    y += 1
                    If x = n_Parents - 1 Then x = 0
                    If y = n_Parents - 1 Then y = 0
                Next i
                If Even_Number(n_Childs) = False Then
                    Call ReprodOp_Part_Mapped_Crossover(List_Parents(x).Path, List_Parents(y).Path, List_Childs(n_Childs - 1).Path, Einzelkind)
                End If

        End Select

    End Sub

    'Reproductionsoperator: "Select_Random_Uniform"
    'Entscheidet zufällig ob der Wert aus dem Path des Elter_A oder Elter_B verwendet wird
    '*************************************************************************************
    Private Sub ReprodOp_Select_Random_Uniform(ByVal ParPath_A() As Integer, ByVal ParPath_B() As Integer, ByRef ChildPath_A() As Integer, ByRef ChildPath_B() As Integer)

        Dim i As Integer

        For i = 0 To ChildPath_A.GetUpperBound(0)    'TODO: Es müsste eigentlich eine definierte Pfadlänge geben
            If Bernoulli() = True Then
                ChildPath_A(i) = ParPath_B(i)
            Else
                ChildPath_A(i) = ParPath_A(i)
            End If
        Next

        For i = 0 To ChildPath_B.GetUpperBound(0)    'TODO: Es müsste eigentlich eine definierte Pfadlänge geben
            If Bernoulli() = True Then
                ChildPath_B(i) = ParPath_A(i)
            Else
                ChildPath_B(i) = ParPath_B(i)
            End If
        Next

    End Sub


    'Reproductionsoperator "Order_Crossover (OX)"
    'Kopiert den mittleren Teil des einen Elter und füllt den Rest aus der Reihenfolge des anderen Elter auf
    'UPGRADE: Es wird immer nur der mittlere Teil Kopiert, könnte auch mal ein einderer sein
    Private Sub ReprodOp_Order_Crossover(ByVal ParPath_A() As Integer, ByVal ParPath_B() As Integer, ByRef ChildPath_A() As Integer, ByRef ChildPath_B() As Integer)

        Dim i As Integer
        Dim x, y As Integer

        Dim CutPoint(1) As Integer
        Call Create_n_Cutpoints(CutPoint)

        'Kopieren des mittleren Paths
        For i = CutPoint(0) + 1 To CutPoint(1)
            ChildPath_A(i) = ParPath_A(i)
            ChildPath_B(i) = ParPath_B(i)
        Next
        'Auffüllen des Paths Teil 3 des Childs A mit dem anderen Elter beginnend bei 0
        x = 0
        For i = CutPoint(1) + 1 To n_Locations - 1
            If Is_No_OK(ParPath_B(x), ChildPath_A) Then
                ChildPath_A(i) = ParPath_B(x)
            Else
                i -= 1
            End If
            x += 1
        Next
        'Auffüllen des Paths Teil 3 des Childs B mit dem anderen Elter beginnend bei 0
        y = 0
        For i = CutPoint(1) + 1 To n_Locations - 1
            If Is_No_OK(ParPath_A(y), ChildPath_B) Then
                ChildPath_B(i) = ParPath_A(y)
            Else
                i -= 1
            End If
            y += 1
        Next
        'Auffüllen des Paths Teil 1 des Childs A mit dem anderen Elter beginnend bei 0
        For i = 0 To CutPoint(0)
            If Is_No_OK(ParPath_B(x), ChildPath_A) Then
                ChildPath_A(i) = ParPath_B(x)
            Else
                i -= 1
            End If
            x += 1
        Next
        'Auffüllen des Paths Teil 1 des Childs B mit dem anderen Elter beginnend bei 0
        For i = 0 To CutPoint(0)
            If Is_No_OK(ParPath_A(y), ChildPath_B) Then
                ChildPath_B(i) = ParPath_A(y)
            Else
                i -= 1
            End If
            y += 1
        Next
    End Sub

    'Reproductionsoperator: "Partially_Mapped_Crossover (PMX)"
    'Kopiert den mittleren Teil des anderen Elter und füllt den Rest mit dem eigenen auf. Falls Doppelt wird gemaped.
    Public Sub ReprodOp_Part_Mapped_Crossover(ByVal ParPath_A() As Integer, ByVal ParPath_B() As Integer, ByRef ChildPath_A() As Integer, ByRef ChildPath_B() As Integer)
        Dim i As Integer
        Dim x As Integer
        Dim Index As Integer
        Dim mapper As Integer

        Dim CutPoint(1) As Integer
        For i = 0 To 10
            Call Create_n_Cutpoints(CutPoint)
        Next

        'Kopieren des mittleren Paths und füllen des Mappers
        x = 0
        For i = CutPoint(0) + 1 To CutPoint(1)
            ChildPath_B(i) = ParPath_A(i)
            ChildPath_A(i) = ParPath_B(i)
            x += 1
        Next

        'Auffüllen des Paths Teil 1 des Childs A und B mit dem anderen Elter beginnend bei 0
        For i = 0 To CutPoint(0)
            'für Child A
            If Is_No_OK(ParPath_A(i), ChildPath_A) Then
                ChildPath_A(i) = ParPath_A(i)
            Else
                mapper = ParPath_A(i)
                Do Until (Is_No_OK(mapper, ChildPath_A) = True)
                    Index = Array.IndexOf(ParPath_B, mapper)
                    mapper = ParPath_A(Index)
                Loop
                ChildPath_A(i) = mapper
            End If

            'für Child B
            If Is_No_OK(ParPath_B(i), ChildPath_B) Then
                ChildPath_B(i) = ParPath_B(i)
            Else
                mapper = ParPath_B(i)
                Do Until (Is_No_OK(mapper, ChildPath_B) = True)
                    Index = Array.IndexOf(ParPath_A, mapper)
                    mapper = ParPath_B(Index)
                Loop
                ChildPath_B(i) = mapper
            End If
        Next i

        'Auffüllen des Paths Teil 3 des Childs A und B mit dem anderen Elter beginnend bei 0
        For i = CutPoint(1) + 1 To n_Locations - 1
            'für Child A
            If Is_No_OK(ParPath_A(i), ChildPath_A) Then
                ChildPath_A(i) = ParPath_A(i)
            Else
                mapper = ParPath_A(i)
                Do Until (Is_No_OK(mapper, ChildPath_A) = True)
                    Index = Array.IndexOf(ParPath_B, mapper)
                    mapper = ParPath_A(Index)
                Loop
                ChildPath_A(i) = mapper
            End If

            'für Child B
            If Is_No_OK(ParPath_B(i), ChildPath_B) Then
                ChildPath_B(i) = ParPath_B(i)
            Else
                mapper = ParPath_B(i)
                Do Until (Is_No_OK(mapper, ChildPath_B) = True)
                    Index = Array.IndexOf(ParPath_A, mapper)
                    mapper = ParPath_B(Index)
                Loop
                ChildPath_B(i) = mapper
            End If
        Next
    End Sub


    'Mutationsfunktionen
    'XXXXXXXXXXXXXXXXXXX

    'Steuerung der Mutationsoperatoren
    '*********************************
    Public Sub Mutation_Control()
        Dim i As Integer

        For i = 0 To n_Childs - 1
            Dim count As Integer = 0
            Do
                Select Case MutOperator
                    Case "RND_Switch"
                        'Verändert zufällig ein gen des Paths
                        Call MutOp_RND_Switch(List_Childs(i).Path)

                    Case "Dyn_Switch"
                        'Verändert zufällig ein gen des Paths mit dynamisch erhöhter Mutationsrate
                        Call MutOp_Dyn_Switch(List_Childs(i).Path, count)
                End Select
                count += 1
            Loop While Is_Twin(i) = True Or Is_Clone(i) = True
            List_Childs(i).mutated = True
        Next

    End Sub

    'Mutationsoperator "RND_Switch"
    'Verändert zufällig ein gen des Paths
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
    'Verändert zufällig ein gen des Paths mit dynamisch erhöhter Mutationsrate
    '*************************************************************************
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

    'Speichert die Child Ergebnisse im PES Memory
    '********************************************
    Sub Memory_Store(ByVal Child_No As Integer, ByVal Gen_No As Integer)

        Dim neu As Integer

        If (Gen_No = 0 And Child_No = 0) Then
            ReDim Memory(0)
        Else
            ReDim Preserve Memory(Memory.GetLength(0))
            neu = Memory.GetUpperBound(0)
        End If

        ReDim Memory(neu).Path(n_Locations - 1)
        ReDim Memory(neu).Parameter(List_Childs(Child_No).myPara.GetUpperBound(0), 1)
        Redim Memory(neu).D(List_Childs(Child_No).myPara.GetUpperBound(0))
        ReDim Memory(neu).Penalty(n_Penalty - 1)

        Memory(neu).Generation = Gen_No
        Array.Copy(List_Childs(Child_No).Path, Memory(neu).Path, List_Childs(Child_No).Path.Length)
        Array.Copy(List_Childs(Child_No).myPara, Memory(neu).Parameter, List_Childs(Child_No).myPara.Length)
        'ToDo: im Child fehlt auch das Dn
        'Array.Copy(List_Childs(Child_No).myPara, Memory(neu).D, List_Childs(Child_No).myPara.Length)
        Array.Copy(List_Childs(Child_No).Penalty, Memory(neu).Penalty, List_Childs(Child_No).Penalty.Length)
        Array.Copy(List_Childs(Child_No).Constrain, Memory(neu).Constrain, List_Childs(Child_No).Constrain.Length)

    End Sub

    'Durchsucht den Memory
    '*********************
    Sub Memory_Search(byref Child as  Struct_Faksimile)

        Dim j, m As Integer
        Dim count_a(n_Locations - 1) As Integer
        Dim count_b(n_Locations - 1) As Integer
        Dim count_c(n_Locations - 1) As Integer

            Dim PES_Parents(0) As Struct_PES_Parent
            DIm akt as Integer = 0

            For j = 0 To n_Locations - 1
                count_a(j) = 0
                count_b(j) = 0
                count_c(j) = 0
            Next

            For j = 0 To n_Locations - 1
                For m = 0 To Memory.GetUpperBound(0)

                    'Rank Nummer 1
                    If Child.Path(j) = Memory(m).Path(j) Then
                        ReDim Preserve PES_Parents(PES_Parents.GetLength(0))
                        akt = PES_Parents.GetUpperBound(0)
                        Call coppy_PES_Struct(Memory(m), PES_Parents(akt))
                        PES_Parents(akt).iLocation = j + 1
                        PES_Parents(akt).Memory_Rank = 1
                        count_a(j) += 1
                    End If

                    'Rank Nummer 2
                    If Not j = n_Locations - 1 And n_Parts_of_Path > 1 Then
                        If Child.Path(j) = Memory(m).Path(j) And Child.Path(j + 1) = Memory(m).Path(j + 1) Then
                            ReDim Preserve PES_Parents(PES_Parents.GetLength(0))
                            akt = PES_Parents.GetUpperBound(0)
                            Call coppy_PES_Struct(Memory(m), PES_Parents(akt))
                            PES_Parents(akt).iLocation = j + 1
                            PES_Parents(akt).Memory_Rank = 2
                            count_b(j) += 1
                        End If
                    End If

                    'Rank Nummer 3
                    If Not (j = n_Locations - 1 Or j = n_Locations - 2) And n_Parts_of_Path > 2 Then
                        If Child.Path(j) = Memory(m).Path(j) And Child.Path(j + 1) = Memory(m).Path(j + 1) And Child.Path(j + 2) = Memory(m).Path(j + 2) Then
                            ReDim Preserve PES_Parents(PES_Parents.GetLength(0))
                            akt = PES_Parents.GetUpperBound(0)
                            Call coppy_PES_Struct(Memory(m), PES_Parents(akt))
                            PES_Parents(akt).iLocation = j + 1
                            PES_Parents(akt).Memory_Rank = 3
                            count_c(j) += 1
                        End If
                    End If
                Next
        Next

            'Die Doppelten niedrigeren Ränge werden gelöscht
        Call PES_Memory_Dubletten_loeschen(PES_Parents)

    End Sub

    'Löscht wenn ein Individuum bei der gleichen Lokation einmal als Rank 1 und einmal als Rank 2 definiert. Bei Rank 2 entsprechnd Rank 3. Außerdem wird der erste leere Datensatz geloescht.
    '***************************************************************************************
    Private Sub PES_Memory_Dubletten_loeschen(ByRef PES_Parents() As Struct_PES_Parent)

        Dim tmp(PES_Parents.GetUpperBound(0) - 1) As Struct_PES_Parent
        Dim isDouble As Boolean
        Dim i, j, x As Integer

        x = 0
        For i = 1 To PES_Parents.GetUpperBound(0)
            isDouble = False
            For j = 1 To PES_Parents.GetUpperBound(0)
                If i <> j And PES_Parents(i).iLocation And is_PES_Double(PES_Parents(i), PES_Parents(j)) Then
                    isDouble = True
                End If
            Next
            If isDouble = False Then
                coppy_PES_Struct(PES_Parents(i), tmp(x))
                x += 1
            End If
        Next

        ReDim Preserve tmp(x - 1)
        ReDim Preserve PES_Parents(x - 1)

        For i = 0 To tmp.GetUpperBound(0)
            coppy_PES_Struct(tmp(i), PES_Parents(i))
        Next

    End Sub

    'Prüft ob die beiden den gleichen Pfad und zur gleichen location gehören
    '***********************************************************************
    Private Function is_PES_Double(ByVal First As Struct_PES_Parent, ByVal Second As Struct_PES_Parent) As Boolean
        is_PES_Double = False

        Dim count as Integer = 0
        Dim i As Integer

        For i = 0 To First.Path.GetUpperBound(0)
            If First.Path(i) = Second.Path(i) Then
                count += 1
            End If
        Next

        If count = First.Path.GetLength(0) Then
            If First.iLocation = Second.iLocation and first.Memory_Rank < second.Memory_Rank Then
                is_PES_Double = True
            End If
        End If

    End Function


    'Hilfsfunktion: Kopiert ein Memory Struct ins Parent
    '***************************************************
    Private Sub coppy_PES_Struct(ByVal Source As Struct_PES_Memory, ByRef Desti As Struct_PES_Parent)

        ReDim Desti.Path(n_Locations - 1)
        ReDim Desti.Parameter(Source.Parameter.GetLength(0), 1)
        ReDim Desti.D(Source.D.GetLength(0))
        ReDim Desti.Penalty(n_Penalty - 1)

        Desti.Generation = Source.Generation
        Array.Copy(Source.Path, Desti.Path, Source.Path.Length)
        Array.Copy(Source.Parameter, Desti.Parameter, Source.Parameter.Length)
        Array.Copy(Source.D, Desti.D, Source.Parameter.Length)
        Array.Copy(Source.Penalty, Desti.Penalty, Source.Penalty.Length)

    End Sub

    'Hilfsfunktion: koppiert ein PES Parent Struct
    '***************************************************
    Private Sub coppy_PES_Struct(ByVal Source As Struct_PES_Parent, ByRef Desti As Struct_PES_Parent)

        ReDim Desti.Path(n_Locations - 1)
        ReDim Desti.Parameter(Source.Parameter.GetLength(0), 1)
        ReDim Desti.Penalty(n_Penalty - 1)

        Desti.iLocation = Source.iLocation
        Desti.Memory_Rank = Source.Memory_Rank
        Desti.Generation = Source.Generation
        Array.Copy(Source.Path, Desti.Path, Source.Path.Length)
        Array.Copy(Source.Parameter, Desti.Parameter, Source.Parameter.Length)
        Array.Copy(Source.Penalty, Desti.Penalty, Source.Penalty.Length)

    End Sub

    'Hilfsfunktionen
    'XXXXXXXXXXXXXXX

    'Hilfsfunktion um zu Prüfen ob eine Zahl bereits in einem Array vorhanden ist oder nicht
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

    'Hilfsfunktion checkt ob die neuen Childs Klone sind
    '***************************************************
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

    'Hilfsfunktion zum generieren von zufälligen Schnittpunkten innerhalb eines Pfades
    'Mit Bernoulli Verteilung mal von rechts mal von links
    '*****************************************************
    Public Sub Create_n_Cutpoints(ByRef CutPoint() As Integer)
        'Generiert zwei CutPoints
        Dim i As Integer
        Dim lowerb As Integer
        Dim upperb As Integer

        'wird zufällig entweder von Link oder von Rechts geschnitten
        If Bernoulli() = True Then
            lowerb = 0
            For i = 0 To CutPoint.GetUpperBound(0)
                upperb = n_Locations - CutPoint.GetLength(0) - 1 + i
                CutPoint(i) = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
                lowerb = CutPoint(i) + 1
            Next i
        Else
            upperb = n_Locations - 2
            For i = CutPoint.GetUpperBound(0) To 0 Step -1
                lowerb = i
                CutPoint(i) = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
                upperb = CutPoint(i) - 1
            Next i
        End If

    End Sub

    'NonDominated Sorting
    'XXXXXXXXXXXXXXXXXXXX

    'Steuerung des NDSorting (Ursprünglich aus ES Eltern)
    '****************************************************
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
        'Nur Eltern werden NDSorting hinzugefügt, Kinder sind schon oben drin
        '--------------------------------------------------------------------

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
            Call Copy_Faksimile_NDSorting(NDSorting(i), Temp(i))
        Next

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
                For i = NFrontMember_gesamt To NFrontMember_aktuell + NFrontMember_gesamt - 1
                    Call Copy_Faksimile_NDSorting(NDSResult(i), List_Parents(i))
                Next i
                NFrontMember_gesamt = NFrontMember_gesamt + NFrontMember_aktuell

            Else
                'Es sind weniger Elterplätze für die nächste Generation verfügber
                'als Mitglieder der aktuellen Front. Nur für diesen Rest wird crowding distance
                'gemacht um zu bestimmen wer noch mitspielen darf und wer noch a biserl was druff hat
                Call NDS_Crowding_Distance_Sort(NDSResult, NFrontMember_gesamt, NFrontMember_gesamt + NFrontMember_aktuell - 1)

                For i = NFrontMember_gesamt To n_Parents - 1
                    Call Copy_Faksimile_NDSorting(NDSResult(i), List_Parents(i))
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
    'Sortiert die nicht dominanten Lösungen nach oben, die dominanten nach unten
    '*******************************************************************************
    Private Function Non_Dominated_Count_and_Sort(ByRef NDSorting() As Struct_NDSorting) As Short
        Dim i As Short
        Dim Temp() As Struct_NDSorting
        Dim counter As Short

        ReDim Temp(NDSorting.GetUpperBound(0))
        Call Dim_NDSorting_Type(Temp)

        Non_Dominated_Count_and_Sort = 0
        counter = 0

        'Die nicht dominanten Lösungen werden nach oben kopiert
        For i = 0 To NDSorting.GetUpperBound(0)
            If NDSorting(i).dominated = True Then
                Call Copy_Faksimile_NDSorting(NDSorting(i), Temp(counter))
                counter = counter + 1
            End If
        Next i

        'Zahl der dominanten wird errechnet und zurückgegeben
        Non_Dominated_Count_and_Sort = NDSorting.GetLength(0) - counter

        'Die dominanten Lösungen werden nach unten kopiert
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

    'Non_Dominated_Count_and_Sort_Sekundäre_Population
    'Sortiert die nicht dominanten Lösungen nach oben, die dominanten nach unten
    'hier für die Sekundäre Population
    '*******************************************************************************

    'Private Function Non_Dominated_Count_and_Sort_Sekundäre_Population(ByRef NDSorting() As NDSortingType) As Short

    'End Function


    'C: Non_Dominated_Result
    'Hier wird pro durchlauf die nicht dominierte Front in NDSResult geschaufelt
    'und die bereits klassifizierten Lösungen aus Temp Array gelöscht
    '*******************************************************************************
    Private Sub Non_Dominated_Result(ByRef Temp() As Struct_NDSorting, ByRef NDSResult() As Struct_NDSorting, ByRef NFrontMember_aktuell As Short, ByRef NFrontMember_gesamt As Short)

        Dim i, Position As Short

        Position = NFrontMember_gesamt - NFrontMember_aktuell

        'In NDSResult werden die nicht dominierten Lösungen eingefügt
        For i = Temp.GetUpperBound(0) + 1 - NFrontMember_aktuell To Temp.GetUpperBound(0)
            'NDSResult alle bisher gefundene Fronten
            Call Copy_Faksimile_NDSorting(Temp(i), NDSResult(Position))
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
        Call Dim_NDSorting_Type(swap)

        Dim fmin, fmax As Double

        For k = 0 To n_Penalty - 1
            For i = start To ende
                For j = start To ende
                    If NDSorting(i).Penalty(k) < NDSorting(j).Penalty(k) Then
                        Call Copy_Faksimile_NDSorting(NDSorting(i), swap(0))
                        Call Copy_Faksimile_NDSorting(NDSorting(j), NDSorting(i))
                        Call Copy_Faksimile_NDSorting(swap(0), NDSorting(j))
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

        'Bestimmen der normierten Raumabstände zwischen allen Elternindividuen
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


