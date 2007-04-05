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

    '********************************************* Konvention *****************************
    'Cities:      1 2 3 4 5 6 7
    'Pathindex:   0 1 2 3 4 5 6
    'CutPoint:     1 2 3 4 5 6
    'LB + UB n=2   x         x

    'BlueM wird kopiert, achtung kein Zeiger
    Public BlueM1 As Apps.BlueM

    'Modus: Unterscheidet zwischen BM und TSP
    Public CES_Modus As String           '"TSP" oder "BM" Optimierung

    'Public Variablen
    Public n_Cities As Integer = 80
    Public ListOfCities(,) As Object
    Public n_Gen As Integer = 1
    Public n_Ziele As Integer

    'Private Variablen
    Private ReprodOperator_TSP As String = "Order_Crossover_OX"
    Private ReprodOperator_BM As String = "Select_Random_Uniform"
    Private MutOperator_TSP As String = "Translocation"
    Private MutOperator_BM As String = "Random_Switch"
    Private n_Parents As Integer = 3
    Private n_Childs As Integer = 36
    Private Strategy As String = "plus"         '"plus" oder "minus" Strategie
    Private MutRate As Integer = 30              'Definiert die Wahrscheinlichkeit der Mutationsrate in %

    '************************************* TSP Struktur *****************************
    Public Structure Faksimile
        Dim No As Short
        Dim Path() As Integer
        Dim Quality_SO As Double    'HACK zum Ausgleich von MO und SO
        Dim Quality_MO() As Double
        Dim Image(,) As Object
    End Structure

    Public ChildList_TSP() As Faksimile = {}
    Public ParentList_TSP() As Faksimile = {}

    '************************************* BM Listen ******************************

    Public ChildList_BM() As Faksimile
    Public ParentList_BM() As Faksimile

    '******************************** Initialisierung *************************************

    Public Sub TSP_Initialize(ByRef TChart1 As Steema.TeeChart.TChart)

        Dim i As Integer
        ReDim ListOfCities(n_Cities - 1, 2)

        Randomize()
        Call TeeChart_Initialise_TSP(Tchart1)

        For i = 0 To n_Cities - 1
            ListOfCities(i, 0) = i + 1
            ListOfCities(i, 1) = Math.Round(Rnd() * 100)
            ListOfCities(i, 2) = Math.Round(Rnd() * 100)
        Next

        Call TeeChart_Zeichnen_TSP(Tchart1, ListOfCities)

    End Sub

    '*********************************** Programm ******************************************

    'Dimensionieren des ChildStructs TSP Problem
    Public Sub Dim_Childs_TSP()
        Dim i As Integer
        ReDim ChildList_TSP(n_Childs - 1)

        For i = 0 To n_Childs - 1
            ChildList_TSP(i).No = i + 1
            ChildList_TSP(i).Quality_SO = 999999999999999999
            ReDim ChildList_TSP(i).Image(n_Cities - 1, 2)
            ReDim ChildList_TSP(i).Path(n_Cities - 1)
        Next

    End Sub

    'Dimensionieren des ChildStructs BM Problem
    Public Sub Dim_Childs_BM()
        Dim i, j As Integer
        ReDim ChildList_BM(n_Childs - 1)

        For i = 0 To n_Childs - 1
            ChildList_BM(i).No = i + 1
            ChildList_BM(i).Quality_SO = 999999999999999999

            ReDim ChildList_BM(i).Quality_MO(n_Ziele - 1)
            For j = 0 To ChildList_BM(i).Quality_MO.GetUpperBound(0)
                ChildList_BM(i).Quality_MO(j) = 999999999999999999
            Next
            ReDim ChildList_BM(i).Path(BlueM1.LocationList.GetUpperBound(0))
            ReDim ChildList_BM(i).Image(BlueM1.VerzweigungsDatei.GetUpperBound(0), 1)
        Next

    End Sub

    'Dimensionieren des ParentStructs TSP Problem
    Public Sub Dim_Parents_TSP()
        Dim i As Integer

        ReDim ParentList_TSP(n_Parents - 1)

        For i = 0 To n_Parents - 1
            ParentList_TSP(i).No = i + 1
            ParentList_TSP(i).Quality_SO = 999999999999999999
            ReDim ParentList_TSP(i).Image(n_Cities - 1, 2)
            ReDim ParentList_TSP(i).Path(n_Cities - 1)
        Next

    End Sub

    'Dimensionieren des ParentStructs BM Problem
    Public Sub Dim_Parents_BM()
        Dim i, j As Integer

        ReDim ParentList_BM(n_Parents - 1)

        For i = 0 To n_Parents - 1
            ParentList_BM(i).No = i + 1
            ParentList_BM(i).Quality_SO = 999999999999999999
            ReDim ParentList_BM(i).Quality_MO(n_Ziele - 1)
            For j = 0 To ParentList_BM(i).Quality_MO.GetUpperBound(0)
                ParentList_BM(i).Quality_MO(j) = 999999999999999999
            Next
            ReDim ParentList_BM(i).Path(BlueM1.LocationList.GetUpperBound(0))
            ReDim ParentList_BM(i).Image(BlueM1.VerzweigungsDatei.GetUpperBound(0), 1)
        Next

    End Sub

    'Generiert zufällige Paths für alle Kinder TSP Problem
    Public Sub Generate_Random_Path_TSP()
        Dim i, j As Integer
        Dim tmp As Integer
        Dim lowerb As Integer = 1
        Dim upperb As Integer = n_Cities
        Randomize()

        For i = 0 To n_Childs - 1
            For j = 0 To ChildList_TSP(i).Path.GetUpperBound(0)
                Do
                    'Randomize() nicht vergessen
                    tmp = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
                Loop While Is_No_OK(tmp, ChildList_TSP(i).Path) = False
                ChildList_TSP(i).Path(j) = tmp
            Next
        Next i

    End Sub

    'Generiert zufällige Paths für alle Kinder BM Problem
    Public Sub Generate_Random_Path_BM()
        Dim i, j As Integer
        Dim tmp As Integer
        Dim lowerb As Integer = 0
        Dim upperb As Integer
        Randomize()

        For i = 0 To n_Childs - 1
            For j = 0 To ChildList_BM(i).Path.GetUpperBound(0)
                upperb = BlueM1.LocationList(j).MassnahmeListe.GetUpperBound(0)
                'Randomize() nicht vergessen
                tmp = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
                ChildList_BM(i).Path(j) = tmp
            Next
        Next

    End Sub

    'HACK
    'Funktion zum manuellen Testen der Paths in der ersten Generation
    Public Sub Generate_Test_Path_BM()
        Dim i, j As Integer
        Dim Grenze As Integer

        'Achtung n_Childs sollte Größer als die Möglichen Kombinationen an einer Stelle sein
        For i = 0 To n_Childs - 1
            For j = 0 To ChildList_BM(i).Path.GetUpperBound(0)
                Grenze = BlueM1.LocationList(j).MassnahmeListe.GetUpperBound(0)
                If i <= Grenze Then
                    ChildList_BM(i).Path(j) = i
                Else
                    ChildList_BM(i).Path(j) = 0
                End If
            Next
        Next

    End Sub

    'HACK
    'Funktion zum manuellen Testen der Paths in der ersten Generation
    Public Sub Generate_All_Test_Path_BM()
        Dim i As Integer
        Dim x, y, z As Integer

        x = 0
        y = 0
        z = 0

        For i = 0 To n_Childs - 1

            ChildList_BM(i).Path(0) = x
            ChildList_BM(i).Path(1) = y
            ChildList_BM(i).Path(2) = z
            x += 1
            If x > BlueM1.LocationList(0).MassnahmeListe.GetUpperBound(0) Then
                x = 0
                y += 1
            End If
            If y > BlueM1.LocationList(1).MassnahmeListe.GetUpperBound(0) Then
                y = 0
                z += 1
            End If
            If z > BlueM1.LocationList(2).MassnahmeListe.GetUpperBound(0) Then
                z = 0
            End If

        Next

    End Sub

    '*************************** Functionen innerhalb der Generationsschleife ****************************

    'Weist den KinderPfaden die Städte zu für TSP
    Public Sub Cities_according_ChildPath_TSP()
        Dim i, j As Integer

        For i = 0 To n_Childs - 1
            ReDim ChildList_TSP(i).Image(n_Cities - 1, 2)
            For j = 0 To n_Cities - 1
                ChildList_TSP(i).Image(j, 0) = ListOfCities(ChildList_TSP(i).Path(j) - 1, 0)
                ChildList_TSP(i).Image(j, 1) = ListOfCities(ChildList_TSP(i).Path(j) - 1, 1)
                ChildList_TSP(i).Image(j, 2) = ListOfCities(ChildList_TSP(i).Path(j) - 1, 2)
            Next
        Next i

    End Sub

    'Ermittelt die Qualität bzw. die Länge des Weges Für TSP
    Public Sub Evaluate_child_Quality_TSP()
        Dim i, j As Integer
        Dim distance As Double
        Dim distanceX As Double
        Dim distanceY As Double

        For i = 0 To n_Childs - 1
            distance = 0
            distanceX = 0
            distanceY = 0
            For j = 0 To n_Cities - 2
                ChildList_TSP(i).Quality_SO = 999999999999999999
                distanceX = (ChildList_TSP(i).Image(j, 1) - ChildList_TSP(i).Image(j + 1, 1))
                distanceX = distanceX * distanceX
                distanceY = (ChildList_TSP(i).Image(j, 2) - ChildList_TSP(i).Image(j + 1, 2))
                distanceY = distanceY * distanceY
                distance = distance + Math.Sqrt(distanceX + distanceY)
            Next j
            distanceX = (ChildList_TSP(i).Image(0, 1) - ChildList_TSP(i).Image(n_Cities - 1, 1))
            distanceX = distanceX * distanceX
            distanceY = (ChildList_TSP(i).Image(0, 2) - ChildList_TSP(i).Image(n_Cities - 1, 2))
            distanceY = distanceY * distanceY
            distance = distance + Math.Sqrt(distanceX + distanceY)
            ChildList_TSP(i).Quality_SO = distance
        Next i

    End Sub

    'Ermittelt die Qualität der Kinder mit dem BlueM
    'Achtung! dies Funktionen liegen jetzt im BlueM
    Public Sub Evaluate_Child_Quality_BM()


    End Sub

    'Selectionsprozess je nach "plus" oder "minus" Strategie
    Public Sub Selection_Process_TSP()
        Dim i, j As Integer

        If Strategy = "minus" Then
            For i = 0 To n_Parents - 1
                ParentList_TSP(i).Quality_SO = ChildList_TSP(i).Quality_SO
                Array.Copy(ChildList_TSP(i).Image, ParentList_TSP(i).Image, ChildList_TSP(i).Image.Length)
                Array.Copy(ChildList_TSP(i).Path, ParentList_TSP(i).Path, ChildList_TSP(i).Path.Length)
            Next i

        ElseIf Strategy = "plus" Then
            j = 0
            For i = 0 To n_Parents - 1
                If ParentList_TSP(i).Quality_SO < ChildList_TSP(j).Quality_SO Then
                    j -= 1
                Else
                    ParentList_TSP(i).Quality_SO = ChildList_TSP(j).Quality_SO
                    Array.Copy(ChildList_TSP(j).Image, ParentList_TSP(i).Image, ChildList_TSP(j).Image.Length)
                    Array.Copy(ChildList_TSP(j).Path, ParentList_TSP(i).Path, ChildList_TSP(j).Path.Length)
                End If
                j += 1
            Next i
        End If

    End Sub

    'Selectionsprozess je nach "plus" oder "minus" Strategie
    Public Sub Selection_Process_BM()
        Dim i, j As Integer
        Dim ChildQ_TMP As Double = 0
        Dim ParentQ_TMP As Double = 0

        If Strategy = "minus" Then
            For i = 0 To n_Parents - 1
                ParentList_BM(i).Quality_SO = ChildList_BM(i).Quality_SO
                Array.Copy(ChildList_BM(i).Image, ParentList_BM(i).Image, ChildList_BM(i).Image.Length)
                Array.Copy(ChildList_BM(i).Path, ParentList_BM(i).Path, ChildList_BM(i).Path.Length)
            Next i

        ElseIf Strategy = "plus" Then
            j = 0
            For i = 0 To n_Parents - 1
                If ParentList_BM(i).Quality_SO < ChildList_BM(j).Quality_SO Then
                    j -= 1
                Else
                    ParentList_BM(i).Quality_SO = ChildList_BM(j).Quality_SO
                    ParentList_BM(i).Quality_MO = ChildList_BM(j).Quality_MO 'HACK: hier Qualität Doppelt
                    Array.Copy(ChildList_BM(j).Image, ParentList_BM(i).Image, ChildList_BM(j).Image.Length)
                    Array.Copy(ChildList_BM(j).Path, ParentList_BM(i).Path, ChildList_BM(j).Path.Length)
                End If
                j += 1
            Next i
        End If

    End Sub

    'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
    Public Sub Reset_Childs_TSP()
        Dim i As Integer

        For i = 0 To n_Childs - 1
            ChildList_TSP(i).No = i + 1
            ChildList_TSP(i).Quality_SO = 999999999999999999
            Array.Clear(ChildList_TSP(i).Path, 0, ChildList_TSP(i).Path.GetLength(0))
            ReDim ChildList_TSP(i).Image(n_Cities, 2)
        Next

    End Sub

    'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
    Public Sub Reset_Childs_BM()
        Dim i, j As Integer

        For i = 0 To n_Childs - 1
            ChildList_BM(i).No = i + 1
            ChildList_BM(i).Quality_SO = 999999999999999999
            For j = 0 To ChildList_BM(i).Quality_MO.GetUpperBound(0)
                ChildList_BM(i).Quality_MO(j) = 999999999999999999
            Next
            Array.Clear(ChildList_BM(i).Path, 0, ChildList_BM(i).Path.GetLength(0))
            ReDim ChildList_BM(i).Image(BlueM1.VerzweigungsDatei.GetUpperBound(0), 1)
        Next

    End Sub

    '**************************************** Reproductionsfunktionen ****************************************

    'Steuerung der Reproduktionsoperatoren
    Public Sub Reproduction_Operations_TSP()
        Dim i As Integer
        Dim x, y As Integer
        Dim Einzelkind(n_Cities - 1) As Integer

        Select Case ReprodOperator_TSP
            'ToDo: Eltern werden nicht zufällig gewählt sondern immer in Top Down Reihenfolge
            Case "Order_Crossover_OX"
                x = 0
                y = 1
                For i = 0 To n_Childs - 2 Step 2
                    Call ReprodOp_Order_Crossover(ParentList_TSP(x).Path, ParentList_TSP(y).Path, ChildList_TSP(i).Path, ChildList_TSP(i + 1).Path)
                    x += 1
                    y += 1
                    If x = n_Parents - 1 Then x = 0
                    If y = n_Parents - 1 Then y = 0
                Next i
                If Even_Number(n_Childs) = False Then
                    Call ReprodOp_Order_Crossover(ParentList_TSP(x).Path, ParentList_TSP(y).Path, ChildList_TSP(n_Childs - 1).Path, Einzelkind)
                End If

            Case "Partially_Mapped_Crossover"
                x = 0
                y = 1
                For i = 0 To n_Childs - 2 Step 2
                    Call ReprodOp_Part_Mapped_Crossover(ParentList_TSP(x).Path, ParentList_TSP(y).Path, ChildList_TSP(i).Path, ChildList_TSP(i + 1).Path)
                    x += 1
                    y += 1
                    If x = n_Parents - 1 Then x = 0
                    If y = n_Parents - 1 Then y = 0
                Next i
                If Even_Number(n_Childs) = False Then
                    Call ReprodOp_Part_Mapped_Crossover(ParentList_TSP(x).Path, ParentList_TSP(y).Path, ChildList_TSP(n_Childs - 1).Path, Einzelkind)
                End If

        End Select

    End Sub

    'Steuerung der Reproduktionsoperatoren
    Public Sub Reproduction_Operations_BM()
        Dim i As Integer
        Dim x, y As Integer
        Dim Einzelkind(BlueM1.LocationList.GetUpperBound(0)) As Integer

        Select Case ReprodOperator_BM
            'ToDo: Eltern werden nicht zufällig gewählt sondern immer in Top Down Reihenfolge
            Case "Select_Random_Uniform"
                x = 0
                y = 1
                For i = 0 To n_Childs - 2 Step 2
                    Call ReprodOp_Select_Random_Uniform(ParentList_BM(x).Path, ParentList_BM(y).Path, ChildList_BM(i).Path, ChildList_BM(i + 1).Path)
                    x += 1
                    y += 1
                    If x = n_Parents - 1 Then x = 0
                    If y = n_Parents - 1 Then y = 0
                Next i
                If Even_Number(n_Childs) = False Then
                    Call ReprodOp_Select_Random_Uniform(ParentList_BM(x).Path, ParentList_BM(y).Path, ChildList_BM(n_Childs - 1).Path, Einzelkind)
                End If
        End Select

    End Sub

    'Reproductionsoperator "Order_Crossover (CX)"
    'Kopiert den mittleren Teil des einen Elter und füllt den Rest aus der Reihenfolge des anderen Elter auf
    'ToDo: Es wird immer nur der mittlere Teil Kopiert, könnte auch mal ein einderer sein
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
        For i = CutPoint(1) + 1 To n_Cities - 1
            If Is_No_OK(ParPath_B(x), ChildPath_A) Then
                ChildPath_A(i) = ParPath_B(x)
            Else
                i -= 1
            End If
            x += 1
        Next
        'Auffüllen des Paths Teil 3 des Childs B mit dem anderen Elter beginnend bei 0
        y = 0
        For i = CutPoint(1) + 1 To n_Cities - 1
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
        For i = CutPoint(1) + 1 To n_Cities - 1
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
    Public Sub Mutation_Operations_TSP()
        Dim i As Integer

        Select Case MutOperator_TSP
            Case "Inversion"
                For i = 0 To n_Childs - 1
                    Call MutOp_Inversion(ChildList_TSP(i).Path)
                    'If PathValid(ChildList(i).Path) = False Then MsgBox("Fehler im Path", MsgBoxStyle.Information, "Fehler")
                Next i
            Case "Translocation"
                For i = 0 To n_Childs - 1
                    Call MutOp_Translocation(ChildList_TSP(i).Path)
                    'If PathValid(ChildList(i).Path) = False Then MsgBox("Fehler im Path", MsgBoxStyle.Information, "Fehler")
                Next i
            Case "Transposition"
                For i = 0 To n_Childs - 1
                    Call MutOp_Transposition(ChildList_TSP(i).Path)
                Next
        End Select

    End Sub

    'Steuerung der Mutationsoperatoren
    Public Sub Mutation_Operations_BM()
        Dim i As Integer

        Select Case MutOperator_BM
            Case "Random_Switch"
                For i = 0 To n_Childs - 1
                    Call MutOp_RND_Switch(ChildList_BM(i).Path)
                    'If PathValid(ChildList(i).Path) = False Then MsgBox("Fehler im Path", MsgBoxStyle.Information, "Fehler")
                Next i
        End Select

    End Sub

    'Mutationsoperator "Inversion"
    'Schneidet ein Segment aus dem Path heraus und fügt es invers wieder ein
    'ToDo: Wird bis jetzt nur auf den mittleren Teil angewendet
    Private Sub MutOp_Inversion(ByVal Path() As Integer)
        Dim i As Integer
        Dim x As Integer

        Dim CutPoint(1) As Integer
        Call Create_n_Cutpoints(CutPoint)

        Dim SubPath(CutPoint(1) - CutPoint(0) - 1) As Integer

        'Kopieren des Substrings
        x = 0
        For i = CutPoint(0) + 1 To CutPoint(1)
            SubPath(x) = Path(i)
            x += 1
        Next

        'Invertiertes einfügen
        For i = CutPoint(0) + 1 To CutPoint(1)
            x -= 1
            Path(i) = SubPath(x)
        Next

    End Sub

    'Mutationsoperator "Translocation"
    'Vertauscht zufällig 3 Abschnitte aus dem String und verwendet Bernoulli verteilt die Inverse
    'ToDo: Jetzt werden immer 3 Translocation durchgeführt könnte man auf n-Ausbauen
    Private Sub MutOp_Translocation(ByVal Path() As Integer)
        Dim i, j As Integer
        Dim x As Integer
        Dim tmp As Integer
        Dim SwapPath(2) As Integer
        Dim CutPoint(1) As Integer
        Call Create_n_Cutpoints(CutPoint)

        Dim SubPath(2)() As Integer
        ReDim SubPath(0)(CutPoint(0))
        ReDim SubPath(1)(CutPoint(1) - CutPoint(0) - 1)
        ReDim SubPath(2)(n_Cities - CutPoint(1) - 2)

        j = SubPath(0).GetLength(0) + SubPath(1).GetLength(0) + SubPath(2).GetLength(0)

        'Kopieren der Substrings
        x = 0
        For i = 0 To CutPoint(0)
            SubPath(0)(x) = Path(i)
            x += 1
        Next
        x = 0
        For i = CutPoint(0) + 1 To CutPoint(1)
            SubPath(1)(x) = Path(i)
            x += 1
        Next
        x = 0
        For i = CutPoint(1) + 1 To n_Cities - 1
            SubPath(2)(x) = Path(i)
            x += 1
        Next

        'Bernloulli Verteilte Inversion der Subpaths
        If Bernoulli() = True Then Array.Reverse(SubPath(0))
        If Bernoulli() = True Then Array.Reverse(SubPath(1))
        If Bernoulli() = True Then Array.Reverse(SubPath(2))

        'Generieren der neuen Reihenfolge
        For i = 0 To 2
            Do
                tmp = CInt(Int(3 * Rnd() + 1))
            Loop While Is_No_OK(tmp, SwapPath) = False
            SwapPath(i) = tmp
        Next
        For i = 0 To 2
            SwapPath(i) -= 1
        Next

        'Übertragen der Substrings in den Path
        x = 0
        For i = 0 To 2
            For j = 0 To SubPath(SwapPath(i)).GetUpperBound(0)
                Path(x) = SubPath(SwapPath(i))(j)
                x += 1
            Next
        Next

    End Sub

    'Mutationsoperator "Transposition"
    'Vertauscht n-mal zwei Werte innerhalb des Paths
    Private Sub MutOp_Transposition(ByVal Path() As Integer)
        Dim i As Integer
        Dim TransRate As Integer = 4             'Transpositionsrate in Prozent(!Achtung keine echte "Rate"!)
        Dim n_trans As Integer                     'Anzahl der Transpositionen
        Dim Point1, Point2 As Integer
        Dim Swap As Integer
        Dim lowerb As Integer = 1
        Dim upperbo As Integer = n_Cities - 2

        n_trans = Decimal.Round(n_Cities * TransRate / 100)

        For i = 0 To n_trans
            Point1 = CInt(Int((upperbo - lowerb + 1) * Rnd() + lowerb))
            Do
                Point2 = CInt(Int((upperbo - lowerb + 1) * Rnd() + lowerb))
            Loop Until Point1 <> Point2
            Swap = Path(Point1)
            Path(Point1) = Path(Point2)
            Path(Point2) = Swap
        Next

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
                upperb_b = BlueM1.LocationList(i).MassnahmeListe.GetUpperBound(0)
                'Randomize() nicht vergessen
                Tmp_b = CInt(Int((upperb_b - lowerb_b + 1) * Rnd() + lowerb_b))
                Path(i) = Tmp_b
            End If
        Next

    End Sub

    '******************************************* Hilfsfunktionen *******************************************

    'Hilfsfunktion: Validierung der Paths
    'ToDo:Option zum ein und Ausschalten dieser Function
    Public Function PathValid(ByVal Path() As Integer) As Boolean
        Dim i As Integer
        Array.Sort(Path)
        For i = 0 To Path.GetUpperBound(0)
            If Path(i) <> i + 1 Then
                Exit Function
            End If
        Next
        PathValid = True
    End Function

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
    Public Sub Sort_Faksimile_TSP(ByRef FaksimileList() As Faksimile)
        'Sortiert die Fiksimile anhand des Abstandes
        Dim i, j As Integer
        Dim swap As EvoKern.CES.Faksimile

        For i = 0 To FaksimileList.GetUpperBound(0)
            For j = 0 To FaksimileList.GetUpperBound(0)
                If FaksimileList(i).Quality_SO < FaksimileList(j).Quality_SO Then
                    swap = FaksimileList(i)
                    FaksimileList(i) = FaksimileList(j)
                    FaksimileList(j) = swap
                End If
            Next j
        Next i

    End Sub

    'Hilfsfunktion zum sortieren der Faksimile
    Public Sub Sort_Faksimile_BM(ByRef FaksimileList() As Faksimile)
        'Sortiert die Fiksimile anhand des Abstandes
        Dim i, j As Integer
        Dim swap As EvoKern.CES.Faksimile

        For i = 0 To FaksimileList.GetUpperBound(0)
            For j = 0 To FaksimileList.GetUpperBound(0)
                If FaksimileList(i).Quality_SO < FaksimileList(j).Quality_SO Then
                    swap = FaksimileList(i)
                    FaksimileList(i) = FaksimileList(j)
                    FaksimileList(j) = swap
                End If
            Next j
        Next i

    End Sub

    'Hilfsfunktion zum generieren von zufälligen Schnittpunkten innerhalb eines Pfades
    'Mit Bernoulli Verteilung mal von rechts mal von links
    Public Sub Create_n_Cutpoints(ByRef CutPoint() As Integer)
        'Generiert zwei CutPoints
        Dim i As Integer
        Dim lowerb As Integer
        Dim upperb As Integer

        'wird zufällig entweder von Link oder von Rechts geschnitten
        If Bernoulli() = True Then
            lowerb = 0
            For i = 0 To CutPoint.GetUpperBound(0)
                upperb = n_Cities - CutPoint.GetLength(0) - 1 + i
                CutPoint(i) = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
                lowerb = CutPoint(i) + 1
            Next i
        Else
            upperb = n_Cities - 2
            For i = CutPoint.GetUpperBound(0) To 0 Step -1
                lowerb = i
                CutPoint(i) = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
                upperb = CutPoint(i) - 1
            Next i
        End If

    End Sub

    'Hilffunktion generiert Bernoulli verteilte Zufallszahl
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
    Public Sub MO_TO_SO(ByRef FaksimileList As Faksimile)
        Dim x As Integer
        FaksimileList.Quality_SO = 0
        For x = 0 To FaksimileList.Quality_MO.GetUpperBound(0)
            FaksimileList.Quality_SO = FaksimileList.Quality_SO + FaksimileList.Quality_MO(x)
        Next
        FaksimileList.Quality_SO = FaksimileList.Quality_SO / n_Ziele

    End Sub

    '******************************************* TeeChart Funktionen **********************************

    Public Sub TeeChart_Initialise_TSP(ByRef TChart1 As Steema.TeeChart.TChart)
        Dim i As Integer

        With TChart1
            .Clear()
            .Header.Text = "Traveling Salesman Problem"
            .Aspect.View3D = False
            .Legend.Visible = False

            'Formatierung der Axen
            '.Chart.Axes.Bottom.Title.Caption = BlueM1.OptZieleListe(0).Bezeichnung 'HACK: Beschriftung der Axen
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Bottom.Maximum = 100
            '.Chart.Axes.Left.Title.Caption = BlueM1.OptParameterListe(0).Bezeichnung 'HACK: Beschriftung der Axen
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Minimum = 0
            .Chart.Axes.Left.Maximum = 100

            'Series(0): Series für die Sädte.
            Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
            Point1.Title = "Städte"
            Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point1.Color = System.Drawing.Color.Orange
            Point1.Pointer.HorizSize = 2
            Point1.Pointer.VertSize = 2

            'Series(n): für die Reisen
            For i = 1 To n_Cities
                Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
                Line1.Title = "Reisen"
                Line1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                Line1.Color = System.Drawing.Color.Blue
                Line1.Pointer.HorizSize = 3
                Line1.Pointer.VertSize = 3
            Next

        End With
    End Sub

    Public Sub TeeChart_Zeichnen_TSP(ByRef TChart1 As Steema.TeeChart.TChart, ByVal TmpListOfCities(,) As Object)

        Dim i As Integer

        'Zeichnene der Punkte für die Städte
        For i = 0 To n_Cities - 1
            TChart1.Series(0).Add(TmpListOfCities(i, 1), TmpListOfCities(i, 2), "")
        Next

        'Zeichnen der Verbindung von der ersten bis zur letzten Stadt
        TChart1.Series(1).Add(TmpListOfCities(0, 1), TmpListOfCities(0, 2), "")
        TChart1.Series(n_Cities).Add(TmpListOfCities(0, 1), TmpListOfCities(0, 2), "")

        For i = 1 To n_Cities - 1
            TChart1.Series(i).Add(TmpListOfCities(i, 1), TmpListOfCities(i, 2), "")
            TChart1.Series(i + 1).Add(TmpListOfCities(i, 1), TmpListOfCities(i, 2), "")
        Next

    End Sub

    '                          Funktionen für CombiBM
    '********************************************************************************

    'Ermittelt des Erforderlichen Zustands für die Verzweigungen
    Public Sub Verzweigung_ON_OFF()
        Dim i, j, x, y, z As Integer
        Dim No As Short

        'Schreibt alle Verzweigungen ins Array
        'kann man auch früher machen!!!!
        For i = 0 To n_Childs - 1
            For j = 0 To ChildList_BM(i).Image.GetUpperBound(0)
                ChildList_BM(i).Image(j, 0) = BlueM1.VerzweigungsDatei(j, 0)
            Next
            For x = 0 To ChildList_BM(i).Path.GetUpperBound(0)
                No = ChildList_BM(i).Path(x)
                For y = 0 To BlueM1.LocationList(x).MassnahmeListe(No).Schaltung.GetUpperBound(0)
                    For z = 0 To ChildList_BM(i).Image.GetUpperBound(0)
                        If BlueM1.LocationList(x).MassnahmeListe(No).Schaltung(y, 0) = ChildList_BM(i).Image(z, 0) Then
                            ChildList_BM(i).Image(z, 1) = BlueM1.LocationList(x).MassnahmeListe(No).Schaltung(y, 1)
                        End If
                    Next
                Next
            Next
        Next
    End Sub


End Class


