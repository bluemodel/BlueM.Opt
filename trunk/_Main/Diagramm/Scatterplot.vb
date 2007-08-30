Partial Public Class Scatterplot

    Public Diags(,) As Diagramm

    'Matrix dimensionieren
    '*********************
    Private Sub dimensionieren(ByVal numZiele As Integer)

        ReDim Me.Diags(numZiele, numZiele)

        Dim i As Integer

        Me.matrix.Name = "Matrix"

        Me.matrix.ColumnCount = numZiele
        For i = 1 To numZiele
            Me.matrix.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100 / numZiele))
        Next

        Me.matrix.RowCount = numZiele
        For i = 1 To numZiele
            Me.matrix.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100 / numZiele))
        Next

    End Sub

    Public Sub zeichnen(ByVal series As Collection)

        'Matrix dimensionieren
        Me.dimensionieren(series.Count)

        Dim i, j, k As Integer

        'Schleife über Spalten
        For i = 0 To Me.matrix.ColumnCount - 1
            'Schleife über Reihen
            For j = 0 To Me.matrix.RowCount - 1
                If (Not i = j) Then
                    'Neues Diagramm erstellen
                    Me.Diags(i, j) = New Diagramm
                    Me.matrix.Controls.Add(Me.Diags(i, j), i, j)
                    With Me.Diags(i, j)
                        'Diagramm
                        .Header.Visible = False
                        .Aspect.View3D = False
                        .Legend.Visible = False
                        'Achsen
                        .Axes.Bottom.Title.Caption = series(i + 1).name
                        .Axes.Left.Title.Caption = series(j + 1).name
                        'Serie initialisieren
                        Dim SeriesNo As Integer = .prepareSeries(i & ", " & j, "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 1)
                        'Punkte einzeichnen
                        For k = 0 To series(i + 1).values.getUpperBound(0)
                            .Chart.Series(SeriesNo).Add(series(i + 1).values(k), series(j + 1).values(k))
                        Next
                    End With
                End If
            Next
        Next

    End Sub

End Class