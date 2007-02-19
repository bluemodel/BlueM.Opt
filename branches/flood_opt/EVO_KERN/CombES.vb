Public Class CombES
    Public ListOfCities(,) As Double
    Public NoOfCities As Integer
    Public ChildPaths(,) As Integer
    Public ParentPaths(,) As Integer

    Public Structure Child
        Dim No As Integer
        Dim Path() As Integer
        Dim Quality As Double
        Dim CityList(,) As Double
    End Structure

    Public ChildList() As Child = {}

    Public Structure Parent
        Dim No As Integer
        Dim Path As Integer
        Dim Quality As Integer
        Dim CityList(,) As Double
    End Structure

    Public ParentList() As Parent = {}


    Public Function Generate_Child(ByRef Path() As Integer) As Boolean
        Generate_Child = True
        Dim i As Integer
        Dim j As Integer
        Dim Check As Boolean
        Dim lowerbound As Integer = 0
        Dim upperbound As Integer = NoOfCities - 1
        'Dim PathNew(Path.GetUpperBound(0)) As Integer

        Randomize()
        'randomvalue = CInt(Int((upperbound - lowerbound + 1) * Rnd() + lowerbound))

        Path(0) = CInt(Int((upperbound - lowerbound + 1) * Rnd() + lowerbound))
        For i = 1 To Path.GetUpperBound(0)
            Do
                Path(i) = CInt(Int((upperbound - lowerbound + 1) * Rnd() + lowerbound))
                Check = True
                For j = 0 To i - 1
                    If Path(i) = Path(j) Then
                        Check = False
                    End If
                Next j
            Loop While Check = False
        Next i
    End Function

End Class
