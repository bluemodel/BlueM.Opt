Partial Public Class Scatterplot
    Inherits System.Windows.Forms.Form

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse Scatterplot                                                    ****
    '****                                                                       ****
    '**** Stellt ein Optimierungsergebnis als Scatterplot-Matrix dar            ****
    '****                                                                       ****
    '**** Autor: Felix Froehlich                                                ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

    Private Diags(,) As Diagramm
    Private OptResult As IHWB.EVO.OptResult
    Private Zielauswahl() As Integer
    Private SekPopOnly As Boolean
    Public Event pointSelected(ByVal ind As Common.Individuum)

    'Konstruktor
    '***********
    Public Sub New(ByVal optres As IHWB.EVO.OptResult, _zielauswahl() As Integer, _sekpoponly As Boolean)

        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        'Optimierungsergebnis übergeben
        Me.OptResult = optres

        'SekPop-Flag setzen
        Me.SekPopOnly = _sekpoponly

        'Zielauswahl speichern
        Me.Zielauswahl = _zielauswahl

        'Diagramme zeichnen
        Call Me.zeichnen()

        'Bereits ausgewählte Lösungen anzeigen
        For Each ind As Common.Individuum In Me.OptResult.getSelectedSolutions
            Call Me.showSelectedSolution(ind)
        Next

    End Sub

    'Diagramme zeichnen
    '******************
    Private Sub zeichnen()

        'Matrix dimensionieren
        Call Me.dimensionieren()

        Dim i, j As Integer
        Dim xAchse, yAchse As String
        Dim serieMin_i As Double = 99999999999
        Dim serieMax_i As Double = 0
        Dim serieMin_j As Double = 99999999999
        Dim serieMax_j As Double = 0

        Dim serie, serie_inv As Steema.TeeChart.Styles.Series
        Dim serieFh, serieFv, serieIst As Steema.TeeChart.Styles.Line
     
        'Schleife über Spalten
        For i = 0 To Me.matrix.ColumnCount - 2
            serieMin_i = 99999999999
            serieMax_i = 0
            'Schleife über Reihen
            For j = 1 To Me.matrix.RowCount - 1
                'Console.Out.WriteLine(matrix.ColumnCount.ToString + " " + matrix.RowCount.ToString)
                serieMin_j = 99999999999
                serieMax_j = 0
                'Console.Out.WriteLine(i.ToString + " " + j.ToString)

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
                    xAchse = Common.Manager.List_Ziele(Me.Zielauswahl(i)).Bezeichnung
                    yAchse = Common.Manager.List_Ziele(Me.Zielauswahl(j)).Bezeichnung

                    If (xAchse.StartsWith("f")) Then
                        .Axes.Bottom.Labels.ValueFormat = "   0.0"
                    Else
                        .Axes.Bottom.Labels.ValueFormat = "0.0+E00"
                    End If

                    If (yAchse.StartsWith("f")) Then
                        .Axes.Left.Labels.ValueFormat = "   0.0"
                    Else
                        .Axes.Left.Labels.ValueFormat = "0.0+E00"
                    End If


                    .Axes.Bottom.Title.Caption = xAchse
                    .Axes.Left.Title.Caption = yAchse

                    .Axes.Left.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
                    .Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value


                    'Achsen nur an den Rändern anzeigen
                    '----------------------------------
                    'YAchsen
                    If (i = 0) Then
                        'Achse standardmäßig anzeigen
                    	'ElseIf (i = Me.Zielauswahl.Length - 1) Then
                        '	'Achse rechts anzeigen
                        '	.Axes.Left.OtherSide = True
                    Else
                        'Achse verstecken
                        .Axes.Left.Title.Visible = False
                        .Axes.Left.Labels.CustomSize = 1
                        .Axes.Left.Labels.Font.Color = Color.Empty
                    End If
                    'XAchsen
                    .Axes.Bottom.Labels.Angle = 90

                    If (j = 1) Then
                        'Achse oben anzeigen
                        .Axes.Bottom.OtherSide = True
                    Else
                        'Achse verstecken
                        .Axes.Bottom.Title.Visible = False
                        .Axes.Bottom.Labels.CustomSize = 1
                        .Axes.Bottom.Labels.Font.Color = Color.Empty
                    End If

                    'Punkte eintragen
                    '================
                    If (Me.SekPopOnly) Then
                        'Nur Sekundäre Population
                        '------------------------
                        serie = .getSeriesPoint(xAchse & ", " & yAchse, "Green", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind As Common.Individuum In Me.OptResult.getSekPop()
                            serie.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID)
                            If ind.Zielwerte(Me.Zielauswahl(i)) < serieMin_i Then serieMin_i = ind.Zielwerte(Me.Zielauswahl(i))
                            If ind.Zielwerte(Me.Zielauswahl(i)) > serieMax_i Then serieMax_i = ind.Zielwerte(Me.Zielauswahl(i))
                            If ind.Zielwerte(Me.Zielauswahl(j)) < serieMin_j Then serieMin_j = ind.Zielwerte(Me.Zielauswahl(j))
                            If ind.Zielwerte(Me.Zielauswahl(j)) > serieMax_j Then serieMax_j = ind.Zielwerte(Me.Zielauswahl(j))
                        Next

                        'Freibord einzeichnen - horizontal
                        '----------------------------------
                        serieFh = .getSeriesLine("line", "Red")
                        serieFh.LinePen.Width = 2

                        If (Common.List_Ziele(Me.Zielauswahl(j)).Bezeichnung.StartsWith("fMalter")) Then
                            serieFh.Add(serieMin_i, 334, "freibord")
                            serieFh.Add(serieMax_i, 334, "freibord")
                        End If
                        '---
                        If (Common.List_Ziele(Me.Zielauswahl(j)).Bezeichnung.StartsWith("fLehnm")) Then
                            serieFh.Add(serieMin_i, 525.5, "freibord")
                            serieFh.Add(serieMax_i, 525.5, "freibord")
                        End If
                        '---
                        If (Common.List_Ziele(Me.Zielauswahl(j)).Bezeichnung.StartsWith("fKling")) Then
                            serieFh.Add(serieMin_i, 393, "freibord")
                            serieFh.Add(serieMax_i, 393, "freibord")
                        End If
                        '---
                        'vertikal

                        serieFv = .getSeriesLine("line2", "Red")
                        serieFv.LinePen.Width = 2

                        If (Common.List_Ziele(Me.Zielauswahl(i)).Bezeichnung.StartsWith("fMalter")) Then
                            serieFv.Add(334, serieMin_j, "freibord")
                            serieFv.Add(334, serieMax_j, "freibord")
                        End If
                        '---
                        If (Common.List_Ziele(Me.Zielauswahl(i)).Bezeichnung.StartsWith("fLehnm")) Then
                            serieFv.Add(526, serieMin_j, "freibord")
                            serieFv.Add(526, serieMax_j, "freibord")
                        End If
                        '---
                        If (Common.List_Ziele(Me.Zielauswahl(i)).Bezeichnung.StartsWith("fKling")) Then
                            serieFv.Add(393, serieMin_j, "freibord")
                            serieFv.Add(393, serieMax_j, "freibord")
                        End If

                        'Ist-Zustand 2002 einzeichnen
                        '-----------------------------
                        serieIst = .getSeriesLine("line3", "Black")
                        serieIst.LinePen.Width = 2
                        '---
                        If (Common.List_Ziele(Me.Zielauswahl(j)).Bezeichnung.StartsWith("fMalter")) Then
                            serieIst.Add(serieMin_i, 334.12, "status quo")
                            serieIst.Add(serieMax_i, 334.12, "status quo")
                        End If
                        '---
                        If (Common.List_Ziele(Me.Zielauswahl(j)).Bezeichnung.StartsWith("fLehnm")) Then
                            serieIst.Add(serieMin_i, 525.07, "status quo")
                            serieIst.Add(serieMax_i, 525.07, "status quo")
                        End If
                        '---
                        If (Common.List_Ziele(Me.Zielauswahl(j)).Bezeichnung.StartsWith("fKling")) Then
                            serieIst.Add(serieMin_i, 393.78, "status quo")
                            serieIst.Add(serieMax_i, 393.78, "status quo")
                        End If
                        '---
                        If (Common.List_Ziele(Me.Zielauswahl(j)).Bezeichnung.StartsWith("Gesamt")) Then
                            serieIst.Add(serieMin_i, 108064362, "status quo")
                            serieIst.Add(serieMax_i, 108064362, "status quo")
                        End If
                        '---
                        If (Common.List_Ziele(Me.Zielauswahl(j)).Bezeichnung.StartsWith("GA")) Then
                            serieIst.Add(serieMin_i, 10188634, "status quo")
                            serieIst.Add(serieMax_i, 10188634, "status quo")
                        End If

                        serieIst = .getSeriesLine("line4", "Black")
                        serieIst.LinePen.Width = 2
                        '---
                        If (Common.List_Ziele(Me.Zielauswahl(i)).Bezeichnung.StartsWith("Gesamt")) Then
                            serieIst.Add(108064362, serieMin_j, "status quo")
                            serieIst.Add(108064362, serieMax_j, "status quo")
                        End If
                        '---
                        If (Common.List_Ziele(Me.Zielauswahl(i)).Bezeichnung.StartsWith("GA")) Then
                            serieIst.Add(10188634, serieMin_j, "status quo")
                            serieIst.Add(10188634, serieMax_j, "status quo")
                        End If



                    Else
                        'Alle Lösungen
                        '-------------
                        serie = .getSeriesPoint(xAchse & ", " & yAchse, "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        serie_inv = .getSeriesPoint(xAchse & ", " & yAchse & " (ungültig)", "Gray", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind As Common.Individuum In Me.OptResult.Solutions
                            'Constraintverletzung prüfen
                            If (ind.Is_Feasible) Then
                                'gültige Lösung Zeichnen
                                serie.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID)
                            Else
                                'ungültige Lösung zeichnen
                                serie_inv.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID)
                            End If
                        Next
                    End If


                    If (i >= j) Then
                        'Diagramme ausblenden
                        .Walls.Back.Transparent = False     'Grau anzeigen
                        .Tools.Clear(True)                  'Um MarksTips zu entfernen
                        For Each s As Steema.TeeChart.Styles.Series In .Series
                            s.Cursor = Cursors.Default      'Kein Hand-Cursor
                            s.Color = Color.Empty           'Punkte unsichtbar
                        Next
                    Else
                        'alle anderen kriegen Handler für seriesClick
                        AddHandler .ClickSeries, AddressOf Me.seriesClick
                        .Walls.Back.Transparent = False
                        .Walls.Back.Visible = True
                        .Walls.Back.Color = Color.White
                    End If

                End With
            Next
        Next

    End Sub

    'Matrix dimensionieren
    '*********************
    Private Sub dimensionieren()

        ReDim Me.Diags(Me.Zielauswahl.Length - 1, Me.Zielauswahl.Length - 1)

        Dim i, fstColFac As Integer

        Me.matrix.Name = "Matrix"

        fstColFac = 100 / Me.Zielauswahl.Length * 0.15

        Me.matrix.ColumnCount = Me.Zielauswahl.Length
        For i = 1 To Me.Zielauswahl.Length
            Me.matrix.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100 / Me.Zielauswahl.Length))
            Console.Out.WriteLine(100 / Me.Zielauswahl.Length)
            'If i = 1 Then 'erste Spalte
            'Me.matrix.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100 / Me.Zielauswahl.Length + fstColFac))
            'Console.Out.WriteLine(SizeType.Percent, 100 / Me.Zielauswahl.Length + fstColFac)
            ' Else
            'Me.matrix.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100 - (100 / Me.Zielauswahl.Length + fstColFac) / (Me.Zielauswahl.Length - 1)))
            'Console.Out.WriteLine(100 - (100 / Me.Zielauswahl.Length + fstColFac) / (Me.Zielauswahl.Length - 1))
            'End If
        Next

        Me.matrix.RowCount = Me.Zielauswahl.Length
        For i = 1 To Me.Zielauswahl.Length
            Me.matrix.RowStyles.Add(New RowStyle(SizeType.Percent, 100 / Me.Zielauswahl.Length))
            Console.Out.WriteLine(100 / Me.Zielauswahl.Length)

            'If i = 2 Then
            'Me.matrix.RowStyles.Add(New RowStyle(SizeType.Percent, 100 / Me.Zielauswahl.Length + fstColFac))
            ' Else
            'Me.matrix.RowStyles.Add(New RowStyle(SizeType.Percent, 100 - (100 / Me.Zielauswahl.Length + fstColFac) / Me.Zielauswahl.Length))
            ' End If
        Next

    End Sub

    'Ruft bei Doppelklick auf Diagramm den TeeChart Editor auf
    '*********************************************************
    Private Sub ShowEditor(ByVal sender As Object, ByVal e As System.EventArgs)

        Call sender.ShowEditor()

    End Sub

    'Form Resize
    '***********
    Private Sub ScatterplotResize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Me.matrix.Width = Me.ClientSize.Width
        Me.matrix.Height = Me.ClientSize.Height

    End Sub

#Region "Lösungsauswahl"

    'Einen Punkt auswählen
    '*********************
    Private Sub seriesClick(ByVal sender As Object, ByVal s As Steema.TeeChart.Styles.Series, ByVal valueIndex As Integer, ByVal e As System.Windows.Forms.MouseEventArgs)

        Dim indID_clicked As Integer
        Dim ind As Common.Individuum

        'Punkt-Informationen bestimmen
        '-----------------------------
        'Solution-ID
        indID_clicked = s.Labels(valueIndex)

        'Lösung holen
        '------------
        ind = Me.OptResult.getSolution(indID_clicked)

        If (ind.ID = indID_clicked) Then

            'Lösung auswählen (wird von Form1.selectSolution() verarbeitet)
            RaiseEvent pointSelected(ind)

        End If

    End Sub

    'Eine ausgewählte Lösung in den Diagrammen anzeigen
    'wird von Form1.selectSolution() aufgerufen
    '**************************************************
    Friend Sub showSelectedSolution(ByVal ind As Common.Individuum)

        Dim serie As Steema.TeeChart.Styles.Series
        Dim i, j As Integer

        'Lösung in alle Diagramme eintragen
        '----------------------------------
        For i = 0 To Me.Diags.GetUpperBound(0)
            For j = 0 To Me.Diags.GetUpperBound(1)
                With Me.Diags(i, j)
                    If (i < j) Then
                        'Roten Punkt zeichnen
                        serie = .getSeriesPoint("ausgewählte Lösungen", "Red", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
                        serie.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID)

                        'Mark anzeigen
                        serie.Marks.Visible = True
                        serie.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Label
                        serie.Marks.Transparency = 25
                        serie.Marks.ArrowLength = 10
                        serie.Marks.Arrow.Visible = False
                    End If
                End With
            Next j
        Next i
    End Sub

    'Serie der ausgewählten Lösungen löschen
    '***************************************
    Public Sub clearSelection()

        Dim i, j As Integer
        Dim serie As Steema.TeeChart.Styles.Series

        For i = 0 To Me.Diags.GetUpperBound(0)
            For j = 0 To Me.Diags.GetUpperBound(1)
                With Me.Diags(i, j)
                    If (i < j) Then
                        'Serie löschen
                        serie = .getSeriesPoint("ausgewählte Lösungen")
                        serie.Dispose()
                    End If

                End With
            Next j
        Next i

    End Sub

#End Region 'Lösungsauswahl

End Class
