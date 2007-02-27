Public Class CES

    'Public Variablen
    Public n_Cities As Integer = 40
    Public ListOfCities(,) As Double
    Public AnzGen As Integer = 10000

    'Private Variablen
    Private ReprodOperator As String = "Order_Crossover_OX"
    Private MutOperator As String = "Translocation"
    Private n_Parents As Integer = 3
    Private n_Childs As Integer = 12
    Private Strategy As String = "plus"                                 '"plus" oder "minus" Strategie

    Public Structure Faksimile
        Dim No As Integer
        Dim Path() As Integer
        Dim Distance As Double
        Dim CityList(,) As Double
    End Structure

    Public ChildList() As Faksimile = {}
    Public ParentList() As Faksimile = {}

    'Dimensionieren des ParentStructs
    Public Sub Dim_Parents()
        Dim i As Integer

        ReDim ParentList(n_Parents - 1)

        For i = 0 To n_Parents - 1
            ParentList(i).Distance = 999999999999999999
            ReDim ParentList(i).CityList(n_Cities - 1, 2)
            ReDim ParentList(i).Path(n_Cities - 1)
        Next

    End Sub

    'Dimensionieren des ChildStructs
    Public Sub Dim_Childs()
        Dim i As Integer
        ReDim ChildList(n_Childs - 1)

        For i = 0 To n_Childs - 1
            ChildList(i).Distance = 999999999999999999
            ReDim ChildList(i).CityList(n_Cities - 1, 2)
            ReDim ChildList(i).Path(n_Cities - 1)
        Next

    End Sub

    'Generiert einen zufällige Paths für alle Kinder
    Public Sub Generate_Random_Path()
        Dim i, j As Integer
        Dim tmp As Integer
        Dim lowerb As Integer = 1
        Dim upperbo As Integer = n_Cities
        Randomize()

        For i = 0 To n_Childs - 1
            ReDim ChildList(i).Path(n_Cities - 1)
            For j = 0 To ChildList(i).Path.GetUpperBound(0)
                Do
                    tmp = CInt(Int((upperbo - lowerb + 1) * Rnd() + lowerb))
                Loop While Is_No_OK(tmp, ChildList(i).Path) = False
                ChildList(i).Path(j) = tmp
            Next
        Next i

    End Sub

    'Weist den KinderPfaden die Städte zu
    Public Sub Cities_according_ChildPath()
        Dim i, j As Integer

        For i = 0 To n_Childs - 1
            ReDim ChildList(i).CityList(n_Cities - 1, 2)
            For j = 0 To n_Cities - 1
                ChildList(i).CityList(j, 0) = ListOfCities(ChildList(i).Path(j) - 1, 0)
                ChildList(i).CityList(j, 1) = ListOfCities(ChildList(i).Path(j) - 1, 1)
                ChildList(i).CityList(j, 2) = ListOfCities(ChildList(i).Path(j) - 1, 2)
            Next
        Next i

    End Sub

    'Ermittelt die Qualität bzw. die Länge des Weges
    Public Sub Evaluate_child_Quality()
        Dim i, j As Integer
        Dim distance As Double
        Dim distanceX As Double
        Dim distanceY As Double

        For i = 0 To n_Childs - 1
            distance = 0
            distanceX = 0
            distanceY = 0
            For j = 0 To n_Cities - 2
                ChildList(i).Distance = 999999999999999999
                distanceX = (ChildList(i).CityList(j, 1) - ChildList(i).CityList(j + 1, 1))
                distanceX = distanceX * distanceX
                distanceY = (ChildList(i).CityList(j, 2) - ChildList(i).CityList(j + 1, 2))
                distanceY = distanceY * distanceY
                distance = distance + Math.Sqrt(distanceX + distanceY)
            Next j
            distanceX = (ChildList(i).CityList(0, 1) - ChildList(i).CityList(n_Cities - 1, 1))
            distanceX = distanceX * distanceX
            distanceY = (ChildList(i).CityList(0, 2) - ChildList(i).CityList(n_Cities - 1, 2))
            distanceY = distanceY * distanceY
            distance = distance + Math.Sqrt(distanceX + distanceY)
            ChildList(i).Distance = distance
        Next i

    End Sub

    'Selectionsprozess je nach "plus" oder "minus" Strategie
    Public Sub Selection_Process()
        Dim i, j As Integer

        If Strategy = "minus" Then
            For i = 0 To n_Parents - 1
                ParentList(i).Distance = ChildList(i).Distance
                Array.Copy(ChildList(i).CityList, ParentList(i).CityList, ChildList(i).CityList.Length)
                Array.Copy(ChildList(i).Path, ParentList(i).Path, ChildList(i).Path.Length)
            Next i

        ElseIf Strategy = "plus" Then
            j = 0
            For i = 0 To n_Parents - 1
                If ParentList(i).Distance < ChildList(j).Distance Then
                    j -= 1
                Else
                    ParentList(i).Distance = ChildList(j).Distance
                    Array.Copy(ChildList(j).CityList, ParentList(i).CityList, ChildList(j).CityList.Length)
                    Array.Copy(ChildList(j).Path, ParentList(i).Path, ChildList(j).Path.Length)
                End If
                j += 1
            Next i
        End If

    End Sub

    'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
    Public Sub Reset_Childs()
        Dim i As Integer

        For i = 0 To n_Childs - 1
            ChildList(i).No = 0
            ChildList(i).Distance = 999999999999999999
            Array.Clear(ChildList(i).Path, 0, ChildList(i).Path.GetLength(0))
            ReDim ChildList(i).CityList(n_Cities, 2)
        Next

    End Sub

    'Steuerung der Reproduktionsoperatoren
    Public Sub Reproduction_Operations()
        Dim i As Integer
        Dim x, y As Integer

        Select Case ReprodOperator
            Case "Order_Crossover_OX"
                x = 0
                y = 1
                For i = 0 To n_Childs - 1 Step 2
                    Call ReprodOp_Order_Crossover(ParentList(x).Path, ParentList(y).Path, ChildList(i).Path, ChildList(i + 1).Path)
                    x += 1
                    y += 1
                    If x = n_Parents - 1 Then x = 0
                    If y = n_Parents - 1 Then y = 0
                Next i
        End Select

    End Sub

    'TODO: funzt nur wenn Anzahl der Kinder Gerade, sollte man verbessern
    'Reproductionsoperator "Order_Crossover"
    Private Sub ReprodOp_Order_Crossover(ByVal ParPath1() As Integer, ByVal ParPath2() As Integer, ByRef ChildPath1() As Integer, ByRef ChildPath2() As Integer)

        Dim i As Integer
        Dim x, y As Integer

        Dim CutPoint(1) As Integer
        Call Create_n_Cutpoints(CutPoint)

        For i = (CutPoint(0)) To CutPoint(1) - 1
            ChildPath1(i) = ParPath1(i)
            ChildPath2(i) = ParPath2(i)
        Next

        x = 0
        For i = CutPoint(1) To n_Cities - 1
            If Is_No_OK(ParPath2(x), ChildPath1) Then
                ChildPath1(i) = ParPath2(x)
                x += 1
            Else
                x += 1
                i -= 1
            End If
        Next

        y = 0
        For i = CutPoint(1) To n_Cities - 1
            If Is_No_OK(ParPath1(y), ChildPath2) Then
                ChildPath2(i) = ParPath1(y)
                y += 1
            Else
                y += 1
                i -= 1
            End If
        Next

        For i = 0 To CutPoint(0) - 1
            If Is_No_OK(ParPath2(x), ChildPath1) Then
                ChildPath1(i) = ParPath2(x)
                x += 1
            Else
                x += 1
                i -= 1
            End If
        Next

        For i = 0 To CutPoint(0) - 1
            If Is_No_OK(ParPath1(y), ChildPath2) Then
                ChildPath2(i) = ParPath1(y)
                y += 1
            Else
                y += 1
                i -= 1
            End If
        Next
    End Sub

    'Steuerung der Mutationsoperatoren
    Public Sub Mutation_Operations()
        Dim i As Integer

        Select Case MutOperator
            Case "Inversion"
                For i = 0 To n_Childs - 1
                    Call MutOp_Inversion(ChildList(i).Path)
                Next i
            Case "Translocation"
                For i = 0 To n_Childs - 1
                    Call MutOp_Translocation(ChildList(i).Path)
                Next i
            Case "Transposition"
                For i = 0 To n_Childs - 1
                    Call MutOp_Transposition(ChildList(i).Path)
                Next
        End Select

    End Sub

    'Mutationsoperator "Inversion"
    'Schneidet ein Segment aus dem Path heraus und fügt es invers wieder ein
    Private Sub MutOp_Inversion(ByVal Path() As Integer)
        Dim i As Integer
        Dim x As Integer

        Dim CutPoint(1) As Integer
        Call Create_n_Cutpoints(CutPoint)

        Dim SubPath(CutPoint(1) - CutPoint(0) - 1) As Integer

        'Kopieren des Substrings
        x = 0
        For i = CutPoint(0) To CutPoint(1) - 1
            SubPath(x) = Path(i)
            x += 1
        Next

        'Invertiertes einfügen
        For i = CutPoint(0) To CutPoint(1) - 1
            x -= 1
            Path(i) = SubPath(x)
        Next

    End Sub


    'Mutationsoperator "Translocation"
    Private Sub MutOp_Translocation(ByVal Path() As Integer)
        Dim i, j As Integer
        Dim x As Integer
        Dim CutPoint(1) As Integer
        Call Create_n_Cutpoints(CutPoint)

        Dim SubPath1(CutPoint(0) - 1) As Integer
        Dim SubPath2(CutPoint(1) - CutPoint(0) - 1) As Integer
        Dim SubPath3(n_Cities - CutPoint(1) - 1) As Integer

        j = SubPath1.GetLength(0) + SubPath2.GetLength(0) + SubPath3.GetLength(0)

        'Kopieren der Substrings
        x = 0
        For i = 0 To CutPoint(0) - 1
            SubPath1(x) = Path(i)
            x += 1
        Next
        x = 0
        For i = CutPoint(0) To CutPoint(1) - 1
            SubPath2(x) = Path(i)
            x += 1
        Next
        x = 0
        For i = CutPoint(1) To n_Cities - 1
            SubPath3(x) = Path(i)
            x += 1
        Next

        'Einfügen des Substrings
        x = 0
        For i = 0 To CutPoint(0) - 1
            x += 1
            Path(i) = SubPath1(x)
        Next
        x = 0
        For i = CutPoint(0) To CutPoint(1) - 1
            x += 1
            Path(i) = SubPath1(x)
        Next
        x = 0
        For i = CutPoint(1) To n_Cities - 1
            x += 1
            Path(i) = SubPath1(x)
        Next

        'ToDo: Zufällig Invertieren
        'Zufälligen einfügepfad generiern evtl. mit obigen path generator

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
    Public Sub Sort_Faksimile(ByRef FaksimileList() As Faksimile)
        'Sortiert die Fiksimile anhand des Abstandes
        Dim i, j As Integer
        Dim swap As dmevodll.CES.Faksimile

        For i = 0 To FaksimileList.GetUpperBound(0)
            For j = 0 To FaksimileList.GetUpperBound(0)
                If FaksimileList(i).Distance < FaksimileList(j).Distance Then
                    swap = FaksimileList(i)
                    FaksimileList(i) = FaksimileList(j)
                    FaksimileList(j) = swap
                End If
            Next j
        Next i

    End Sub

    'Hilfsfunktion zum generieren von zwei zufälligen Schnittpunkten innerhalb eines Pfades
    'ToDo: bisher werden die CutPoints asymetrisch gewählt, man könnte auch mal von rechts starten
    'ToDo: mann sollte den Range mit upper and lower Bound übergeben übergeben
    Public Sub Create_n_Cutpoints(ByRef CutPoint() As Integer)
        'Generiert zwei CutPoints
        Dim i As Integer
        Dim lowerb As Integer = 1
        Dim upperbo As Integer = n_Cities - 2

        For i = 0 To CutPoint.GetUpperBound(0)
            CutPoint(i) = CInt(Int((upperbo - lowerb + 1) * Rnd() + lowerb))
            lowerb = CutPoint(i) + 1
        Next i

    End Sub

End Class

