Partial Public Class Scatterplot

    Private Diags(,) As Diagramm
    Private OptResult As EVO.OptResult
    Private nOptZiele As Integer

    'Konstruktor
    '***********
    Public Sub New(ByVal optres as EVO.OptResult)

        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        'Optimierungsergebnis übergeben
        Me.OptResult = optres
        Me.nOptZiele = optres.List_OptZiele.Length()

        'Diagramme zeichnen
        Call Me.zeichnen()

    End Sub

    'Diagramme zeichnen
    '******************
    Private Sub zeichnen()

        'Matrix dimensionieren
        Call Me.dimensionieren()

        Dim i, j, n As Integer
        Dim xAchse, yAchse As String
        Dim serie As Steema.TeeChart.Styles.Series

        'Schleife über Spalten
        For i = 0 To Me.matrix.ColumnCount - 1
            'Schleife über Reihen
            For j = 0 To Me.matrix.RowCount - 1
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
                    xAchse = OptResult.List_OptZiele(i).Bezeichnung
                    yAchse = OptResult.List_OptZiele(j).Bezeichnung

                    .Axes.Bottom.Title.Caption = xAchse
                    .Axes.Left.Title.Caption = yAchse

                    .Axes.Left.Labels.ValueFormat = "0.00E+00"
                    .Axes.Bottom.Labels.ValueFormat = "0.00E+00"

                    'Achsen nur an den Rändern anzeigen
                    '----------------------------------
                    'YAchsen
                    If (i = 0) Then
                        'Achse standardmäßig anzeigen
                    ElseIf (i = Me.nOptZiele - 1) Then
                        'Achse rechts anzeigen
                        .Axes.Left.OtherSide = True
                    Else
                        'Achse verstecken
                        .Axes.Left.Title.Visible = False
                        .Axes.Left.Labels.CustomSize = 1
                        .Axes.Left.Labels.Font.Color = Color.Empty
                    End If
                    'XAchsen
                    If (j = 0) Then
                        'Achse oben anzeigen
                        .Axes.Bottom.OtherSide = True
                    ElseIf (j = Me.nOptZiele - 1) Then
                        'Achse standardmäßig anzeigen
                    Else
                        'Achse verstecken
                        .Axes.Bottom.Title.Visible = False
                        .Axes.Bottom.Labels.CustomSize = 1
                        .Axes.Bottom.Labels.Font.Color = Color.Empty
                    End If

                    'Punkte eintragen
                    '----------------
                    serie = .getSeriesPoint(xAchse & ", " & yAchse, "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)

                    For n = 0 To OptResult.Solutions.GetUpperBound(0)
                        'Constraintverletzung prüfen
                        If (Not OptResult.Solutions(n).isValid) Then
                            serie = .getSeriesPoint(xAchse & ", " & yAchse & " (ungültig)", "Gray", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        End If
                        'Zeichnen
                        serie.Add(OptResult.Solutions(n).QWerte(i), OptResult.Solutions(n).QWerte(j))
                    Next

                    If (i = j) Then
                        'Diagramme auf der Diagonalen ausblenden
                        .Walls.Back.Transparent = False     'Grau anzeigen
                        .Tools.Clear(True)                  'Um MarksTips zu entfernen
                        serie.Cursor = Cursors.Default      'Kein Hand-Cursor
                        serie.Color = Color.Empty           'Punkte unsichtbar
                    Else
                        'alle anderen kriegen Handler für highlightPoint
                        AddHandler .ClickSeries, AddressOf Me.highlightPoint
                    End If

                End With
            Next
        Next

    End Sub

    'Matrix dimensionieren
    '*********************
    Private Sub dimensionieren()

        ReDim Me.Diags(Me.nOptZiele - 1, Me.nOptZiele - 1)

        Dim i As Integer

        Me.matrix.Name = "Matrix"

        Me.matrix.ColumnCount = Me.nOptZiele
        For i = 1 To Me.nOptZiele
            Me.matrix.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100 / Me.nOptZiele))
        Next

        Me.matrix.RowCount = Me.nOptZiele
        For i = 1 To Me.nOptZiele
            Me.matrix.RowStyles.Add(New RowStyle(SizeType.Percent, 100 / Me.nOptZiele))
        Next

    End Sub

    'Angeklickten Punkt in allen Diagrammen hervorheben
    '**************************************************
    Private Sub highlightPoint(ByVal sender As Object, ByVal s As Steema.TeeChart.Styles.Series, ByVal valueIndex As Integer, ByVal e As System.Windows.Forms.MouseEventArgs)

        Dim i, j As Integer
        Dim isOK As Boolean
        Dim xWert, yWert As Double
        Dim xAchse, yAchse As String
        Dim iOptZielx, iOptZiely As Integer
        Dim Solution As EVO.OptResult.Struct_Solution
        Dim serie As Steema.TeeChart.Styles.Series
        'Dim anno As Steema.TeeChart.Tools.Annotation

        ReDim Solution.QWerte(Me.nOptZiele - 1)

        'Punkt-Informationen bestimmen
        '-----------------------------
        'X und Y Werte
        xWert = s.XValues(valueIndex)
        yWert = s.YValues(valueIndex)
        'X und Y Achsen (Zielfunktionen)
        xAchse = sender.Axes.Bottom.Title.Caption
        yAchse = sender.Axes.Left.Title.Caption

        With Me.OptResult
            'OptZiele zuordnen
            '-----------------
            iOptZielx = -1
            iOptZiely = -1
            For i = 0 To .List_OptZiele.GetUpperBound(0)
                If (.List_OptZiele(i).Bezeichnung = xAchse) Then
                    iOptZielx = i
                ElseIf (.List_OptZiele(i).Bezeichnung = yAchse) Then
                    iOptZiely = i
                End If
            Next
            If (iOptZielx < 0 Or iOptZiely < 0) Then
                'OptZiele konnten nicht zugeordnet werden
                Exit Sub
            End If

            'Lösung identifizieren
            '---------------------
            isOK = False
            For i = 0 To .Solutions.GetUpperBound(0)
                If (.Solutions(i).QWerte(iOptZielx) = xWert And _
                    .Solutions(i).QWerte(iOptZiely) = yWert) Then
                    Solution = .Solutions(i)
                    isOK = True
                    Exit For
                End If
            Next
            If (Not isOK) Then
                'Lösung konnte nicht indentifiziert werden
                Exit Sub
            End If

        End With

        'Lösung in alle Diagramme eintragen
        '----------------------------------
        For i = 0 To Me.Diags.GetUpperBound(0)
            For j = 0 To Me.Diags.GetUpperBound(1)
                With Me.Diags(i, j)

                    'Roten Punkt zeichnen
                    serie = .getSeriesPoint("Highlight", "red", Steema.TeeChart.Styles.PointerStyles.Circle, 4)
                    serie.Clear()
                    serie.Add(Solution.QWerte(i), Solution.QWerte(j))

                    'Mark anzeigen
                    serie.Marks.Visible = True
                    serie.Marks.Style = Steema.TeeChart.Styles.MarksStyles.XY
                    serie.Marks.MultiLine = True
                    serie.Marks.Transparency = 25
                    serie.Marks.ArrowLength = 10
                    serie.Marks.Arrow.Visible = False
                    If (i = j) Then
                        serie.Marks.Style = Steema.TeeChart.Styles.MarksStyles.XValue
                    End If

                    ''Annotation anzeigen (funzt nicht so gut!)
                    'Call .Tools.Clear(True)
                    'anno = New Steema.TeeChart.Tools.Annotation(.Chart)
                    'anno.Position = Steema.TeeChart.Tools.AnnotationPositions.RightTop
                    'anno.Text = Me.OptResult.List_OptZiele(i).Bezeichnung & ": " & Solution.QWerte(i).ToString("g3")
                    'anno.Text &= Chr(13) & Chr(10)
                    'anno.Text &= Me.OptResult.List_OptZiele(j).Bezeichnung & ": " & Solution.QWerte(j).ToString("g3")
                End With
            Next j
        Next i

    End Sub

    'Ruft bei Doppelklick auf Diagramm den TeeChart Editor auf
    '*********************************************************
    Private Sub ShowEditor(ByVal sender As Object, ByVal e As System.EventArgs)

        Call sender.ShowEditor()

    End Sub

    'Form Resize
    '***********
    Private Sub ScatterplotResize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Me.matrix.Width = Me.Width - 10
        Me.matrix.Height = Me.Height - 10 - 25

    End Sub

End Class