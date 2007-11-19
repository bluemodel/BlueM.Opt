Public Class Solution

    Public ID As Integer
    Public QWerte() As Double
    Public OptPara() As Double
    Public Constraints() As Double

    Public ReadOnly Property isValid() As Boolean
        Get
            For i As Integer = 0 To Me.Constraints.GetUpperBound(0)
                If (Me.Constraints(i) < 0) Then Return False
            Next
            Return True
        End Get
    End Property

End Class
