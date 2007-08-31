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

    Public Sub zeichnen(ByVal OptResult As Main.OptResult)

        'Matrix dimensionieren
        Me.dimensionieren(OptResult.Solutions(0).QWerte.GetLength(0))

        Dim i, j, n As Integer

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
                        '--------
                        .Header.Visible = False
                        .Aspect.View3D = False
                        .Legend.Visible = False
                        AddHandler .DoubleClick, AddressOf Me.ShowEditor
                        
                        'Achsen
                        '------
                        Dim xAchse As String = OptResult.List_OptZiele(i).Bezeichnung
                        Dim yAchse As String = OptResult.List_OptZiele(j).Bezeichnung
                        .Axes.Bottom.Title.Caption = xAchse
                        .Axes.Left.Title.Caption = yAchse

                        'Serien initialisieren
                        '---------------------
                        Dim SeriesNo, SeriesNoValid, SeriesNoInvalid As Integer
                        SeriesNoValid = .prepareSeries(xAchse & ", " & yAchse, "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 1)
                        SeriesNoInvalid = .prepareSeries(xAchse & ", " & yAchse & " (ungültig)", "Gray", Steema.TeeChart.Styles.PointerStyles.Circle, 1)

                        'Punkte eintragen
                        '----------------
                        For n = 0 To OptResult.Solutions.getUpperBound(0)
                            'Constraintverletzung prüfen
                            If (OptResult.Solutions(n).isValid) Then
                                SeriesNo = SeriesNoValid
                            Else
                                SeriesNo = SeriesNoInvalid
                            End If
                            .Chart.Series(SeriesNo).Add(OptResult.Solutions(n).QWerte(i), OptResult.Solutions(n).QWerte(j))
                        Next
                    End With
                End If
            Next
        Next

    End Sub

    'Ruft bei Doppelklick auf Diagramm den TeeChart Editor auf
    '*********************************************************
    Private Sub ShowEditor(ByVal sender As Object, ByVal e As System.EventArgs)
        Call sender.ShowEditor()
    End Sub

End Class