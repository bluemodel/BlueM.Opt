Public Class Wave

    Public Structure Wave
        Public Bezeichnung As String                'Bezeichnung
        Public Einheit As String                    'Einheit
        Public Typ As String                        'Q, N, W ....
        Public Wave(,) As Object
    End Structure

    Public WaveList() As Wave = {}                  'Liste der Waves

    Private Sub Wave_Load(sender as Object, e as System.EventArgs) Handles MyBase.Load
        Me.WForm.Diag.Width = 800
        Me.WForm.Diag.Height = 600
    End Sub

    'gespeicherte Serien in Diagramm eintragen
    '*****************************************
    Public Sub Wave_draw()

        Dim i, j As Integer

        For i = 0 To WaveList.GetUpperBound(0)
            For j = 0 To WaveList(i).Wave.GetUpperBound(0)
                Me.WForm.Diag.Series(i).Add(WaveList(i).Wave(j, 0), WaveList(i).Wave(j, 1))
            Next j
        Next i

    End Sub

End Class