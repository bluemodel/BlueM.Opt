Public Class CombES
    Public ListOfCities(,) As Double
    Public NoOfCities As Integer
    Public ChildPaths(,) As Integer
    Public ParentPaths(,) As Integer

    Public Structure Faksimile
        Dim No As Integer
        Dim Path() As Integer
        Dim Distance As Double
        Dim CityList(,) As Double
    End Structure

    Public ChildList() As Faksimile = {}
    Public ParentList() As Faksimile = {}

    'Generiert eine zufälligen Path
    Public Sub Generate_Path(ByRef Path() As Integer)
        Dim i As Integer
        Dim j As Integer
        Dim Check As Boolean
        Dim lowerb As Integer = 1
        Dim upperbo As Integer = NoOfCities
        'Dim PathNew(Path.GetUpperBound(0)) As Integer

        Randomize()
        'randomvalue = CInt(Int((upperbound - lowerbound + 1) * Rnd() + lowerbound))

        Path(0) = CInt(Int((upperbo - lowerb + 1) * Rnd() + lowerb))
        For i = 1 To Path.GetUpperBound(0)
            Do
                Path(i) = CInt(Int((upperbo - lowerb + 1) * Rnd() + lowerb))
                Check = True
                For j = 0 To i - 1
                    If Path(i) = Path(j) Then
                        Check = False
                    End If
                Next j
            Loop While Check = False
        Next i
    End Sub

    Public Sub Create_Two_Cutpoints(ByVal NoC As Integer, ByRef CutPoint() As Integer)
        'Generiert zwei CutPoints
        Dim lowerb As Integer = 1
        Dim upperbo As Integer = NoC - 2
        CutPoint(0) = CInt(Int((upperbo - lowerb + 1) * Rnd() + lowerb))
        lowerb = CutPoint(0) + 1
        upperbo = NoC - 2
        CutPoint(1) = CInt(Int((upperbo - lowerb + 1) * Rnd() + lowerb))
    End Sub

    Public Sub Operator_Order_Crossover(ByVal ParPath1() As Integer, ByVal ParPath2() As Integer, ByRef ChildPath1() As Integer, ByRef ChildPath2() As Integer)
        Dim i As Integer
        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim CutPoint(1) As Integer

        Call Create_Two_Cutpoints(NoOfCities, CutPoint)

        For i = (CutPoint(0)) To CutPoint(1) - 1
            ChildPath1(i) = ParPath1(i)
            ChildPath2(i) = ParPath2(i)
        Next

        'Die Cut Point müssen nochmal geprüft werden
        For i = CutPoint(1) To NoOfCities - 1
            If Is_No_OK(ParPath2(x), ChildPath1) Then
                ChildPath1(i) = ParPath2(x)
                x += 1
            Else
                x += 1
                i -= 1
            End If
        Next

        For i = CutPoint(1) To NoOfCities - 1
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

        For i = 0 To 0

        Next

    End Sub

    Public Function Is_No_OK(ByVal No As Integer, ByVal Path() As Integer) As Boolean
        Is_No_OK = True
        Dim i As Integer
        For i = 1 To Path.GetUpperBound(0)
            If No = Path(i) Then
                Is_No_OK = False
            End If
        Next
    End Function
End Class

