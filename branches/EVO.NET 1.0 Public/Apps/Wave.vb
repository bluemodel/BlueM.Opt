Public Class Wave

    Public Structure Wave
        Public Bezeichnung As String                'Bezeichnung
        Public Einheit As String                    'Einheit
        Public Typ As String                        'Q, N, W ....
        Public Wave(,) As Object
    End Structure

    Public WaveList() As Wave = {}                  'Liste der Waves

    'Bug 85: nach Klasse Diagramm auslagern!
    Public Sub TeeChart_initialise()
        Dim i As Integer

        With TChart1
            .Clear()
            .Header.Text = "Waves"
            .Aspect.View3D = False
            .Legend.Visible = False

            'Series(1) Eingangswelle
            Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
            Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point1.Pointer.HorizSize = 3
            Point1.Pointer.VertSize = 3

            'Series(0 bis n)
            For i = 0 To WaveList.GetUpperBound(0)
                Dim Point2 As New Steema.TeeChart.Styles.Points(.Chart)
                Point2.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                Point2.Pointer.HorizSize = 2
                Point2.Pointer.VertSize = 2
            Next i

            'Formatierung der Axen
            .Chart.Axes.Bottom.Title.Caption = "Time"
            .Chart.Axes.Bottom.Automatic = True
            '.Chart.Axes.Bottom.Maximum = Anzahl_Kalkulationen
            '.Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Left.Title.Caption = "Welle"
            .Chart.Axes.Left.Automatic = True
            .Chart.Axes.Left.Minimum = 0
        End With
    End Sub

    Public Sub TeeChart_draw()

        Dim i As Integer
        Dim j As Integer

        For i = 0 To WaveList.GetUpperBound(0)
            For j = 0 To WaveList(i).Wave.GetUpperBound(0)
                'WaveList(i).Wave(0, j) = Convert.ToDouble(WaveList(i).Wave(0, j))
                TChart1.Series(i).Add(j, WaveList(i).Wave(j, 1), WaveList(i).Wave(j, 0))
            Next j
        Next i

    End Sub
End Class