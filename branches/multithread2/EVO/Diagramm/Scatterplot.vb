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

    Private Diags(,) As EVO.Diagramm
    Private Zielauswahl() As Integer
    Private SekPopOnly, ShowRef As Boolean
    Public Event pointSelected(ByVal ind As Common.Individuum)

    'Konstruktor
    '***********
    Public Sub New(ByVal _zielauswahl() As Integer, ByVal _sekpoponly As Boolean, ByVal _showRef As Boolean)

        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        'Optionen übernehmen
        Me.SekPopOnly = _sekpoponly
        Me.ShowRef = _showref

        'Zielauswahl speichern
        Me.Zielauswahl = _zielauswahl

        'Diagramme zeichnen
        Call Me.zeichnen()

        'Bereits ausgewählte Lösungen anzeigen
        For Each ind As Common.Individuum In ResultManager.getSelectedSolutions
            Call Me.showSelectedSolution(ind)
        Next

    End Sub

    'Diagramme zeichnen
    '******************
    Private Sub zeichnen()

        Dim i, j As Integer
        Dim xAchse, yAchse As String
        Dim min() As Double
        Dim max() As Double
        Dim serie, serie_inv As Steema.TeeChart.Styles.Series
        Dim colorline1 As Steema.TeeChart.Tools.ColorLine

        'Matrix dimensionieren
        '---------------------
        Call Me.dimensionieren()

        'Min und Max für Achsen bestimmen
        '--------------------------------
        ReDim min(Me.Zielauswahl.GetUpperBound(0))
        ReDim max(Me.Zielauswahl.GetUpperBound(0))
        For i = 0 To Me.Zielauswahl.GetUpperBound(0)
            min(i) = Double.MaxValue
            max(i) = Double.MinValue
            If (Me.SekPopOnly) Then
                'Nur Sekundäre Population
                For Each ind As Common.Individuum In ResultManager.OptResult.getSekPop()
                    min(i) = Math.Min(ind.Zielwerte(Me.Zielauswahl(i)), min(i))
                    max(i) = Math.Max(ind.Zielwerte(Me.Zielauswahl(i)), max(i))
                Next
            Else
                'Alle Lösungen
                For Each ind As Common.Individuum In ResultManager.OptResult.Solutions
                    min(i) = Math.Min(ind.Zielwerte(Me.Zielauswahl(i)), min(i))
                    max(i) = Math.Max(ind.Zielwerte(Me.Zielauswahl(i)), max(i))
                Next
            End If
            'IstWerte
            If (Common.Manager.List_Ziele(Me.Zielauswahl(i)).hasIstWert) Then
                min(i) = Math.Min(Common.Manager.List_Ziele(Me.Zielauswahl(i)).IstWert, min(i))
                max(i) = Math.Max(Common.Manager.List_Ziele(Me.Zielauswahl(i)).IstWert, max(i))
            End If
            'Vergleichsergebnis
            If (Me.ShowRef) Then
                For Each ind As Common.Individuum In ResultManager.OptResultRef.getSekPop()
                    min(i) = Math.Min(ind.Zielwerte(Me.Zielauswahl(i)), min(i))
                    max(i) = Math.Max(ind.Zielwerte(Me.Zielauswahl(i)), max(i))
                Next
            End If

        Next

        'Schleife über Spalten
        '---------------------
        For i = 0 To Me.matrix.ColumnCount - 1
            'Schleife über Reihen
            '--------------------
            For j = 0 To Me.matrix.RowCount - 1

                'Neues Diagramm erstellen
                Me.Diags(i, j) = New EVO.Diagramm()
                Me.matrix.Controls.Add(Me.Diags(i, j), i, j)

                With Me.Diags(i, j)

                    AddHandler .DoubleClick, AddressOf Me.ShowEditor

                    'Diagramm formatieren
                    '====================
                    .Header.Visible = False
                    .Aspect.View3D = False
                    .Legend.Visible = False

                    'Achsen
                    '------
                    'Titel
                    xAchse = Common.Manager.List_Ziele(Me.Zielauswahl(i)).Bezeichnung
                    yAchse = Common.Manager.List_Ziele(Me.Zielauswahl(j)).Bezeichnung

                    .Axes.Bottom.Title.Caption = xAchse
                    .Axes.Left.Title.Caption = yAchse

                    'Labels
                    .Axes.Left.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
                    .Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value

                    'Min und Max
                    .Axes.Bottom.Automatic = False
                    .Axes.Bottom.Minimum = min(i)
                    .Axes.Bottom.Maximum = max(i)
                    .Axes.Bottom.MinimumOffset = 2
                    .Axes.Bottom.MaximumOffset = 2

                    .Axes.Left.Automatic = False
                    .Axes.Left.Minimum = min(j)
                    .Axes.Left.Maximum = max(j)
                    .Axes.Left.MinimumOffset = 2
                    .Axes.Left.MaximumOffset = 2

                    'Beschriftungsformat
                    If (max(i) >= 1000 Or min(i) <= -1000) Then .Axes.Bottom.Labels.ValueFormat = "0.##E0"
                    If (max(j) >= 1000 Or min(j) <= -1000) Then .Axes.Left.Labels.ValueFormat = "0.##E0"

                    'Achsen nur an den Rändern anzeigen
                    '----------------------------------
                    'YAchsen
                    If (i = 0) Then
                        'Achse standardmäßig anzeigen
                    ElseIf (i = Me.Zielauswahl.Length - 1) Then
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
                    ElseIf (j = Me.Zielauswahl.Length - 1) Then
                        'Achse standardmäßig anzeigen
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
                        For Each ind As Common.Individuum In ResultManager.OptResult.getSekPop()
                            serie.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID.ToString())
                        Next
                    Else
                        'Alle Lösungen
                        '-------------
                        serie = .getSeriesPoint(xAchse & ", " & yAchse, "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        serie_inv = .getSeriesPoint(xAchse & ", " & yAchse & " (ungültig)", "Gray", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind As Common.Individuum In ResultManager.OptResult.Solutions
                            'Constraintverletzung prüfen
                            If (ind.Is_Feasible) Then
                                'gültige Lösung Zeichnen
                                serie.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID.ToString())
                            Else
                                'ungültige Lösung zeichnen
                                serie_inv.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID.ToString())
                            End If
                        Next
                    End If

                    'IstWerte eintragen
                    '==================
                    If (Common.Manager.List_Ziele(Me.Zielauswahl(i)).hasIstWert) Then
                        'X-Achse:
                        '--------
                        colorline1 = New Steema.TeeChart.Tools.ColorLine(.Chart)
                        colorline1.Pen.Color = Color.Red
                        colorline1.Axis = .Axes.Bottom
                        colorline1.AllowDrag = False
                        colorline1.NoLimitDrag = True
                        colorline1.Value = Common.Manager.List_Ziele(Me.Zielauswahl(i)).IstWert
                    End If

                    If (Common.Manager.List_Ziele(Me.Zielauswahl(j)).hasIstWert) Then
                        'Y-Achse:
                        '--------
                        colorline1 = New Steema.TeeChart.Tools.ColorLine(.Chart)
                        colorline1.Pen.Color = Color.Red
                        colorline1.Axis = .Axes.Left
                        colorline1.AllowDrag = False
                        colorline1.NoLimitDrag = True
                        colorline1.Value = Common.Manager.List_Ziele(Me.Zielauswahl(j)).IstWert

                    End If

                    'Vergleichsergebnis anzeigen
                    '===========================
                    If (Me.ShowRef) Then
                        serie = .getSeriesPoint(xAchse & ", " & yAchse & " (Vergleichsergebnis)", "Blue", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind As Common.Individuum In ResultManager.OptResultRef.getSekPop()
                            serie.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID & " (Vergleichsergebnis)")
                        Next
                    End If


                    'Diagramme auf der Diagonalen ausblenden
                    '=======================================
                    If (i = j) Then
                        'Hintergrund grau anzeigen
                        .Walls.Back.Transparent = False
                        .Walls.Back.Gradient.Visible = False
                        'MarksTips entfernen
                        .Tools.Clear(True)
                        'Serien unsichtbar machen
                        For Each s As Steema.TeeChart.Styles.Series In .Series
                            s.Cursor = Cursors.Default      'Kein Hand-Cursor
                            s.Color = Color.Empty           'Punkte unsichtbar
                        Next
                    Else
                        'alle anderen kriegen Handler für seriesClick
                        AddHandler .ClickSeries, AddressOf Me.seriesClick
                    End If

                End With
            Next
        Next

    End Sub

    'Matrix dimensionieren
    '*********************
    Private Sub dimensionieren()

        ReDim Me.Diags(Me.Zielauswahl.Length - 1, Me.Zielauswahl.Length - 1)

        Dim i As Integer

        Me.matrix.Name = "Matrix"

        Me.matrix.ColumnCount = Me.Zielauswahl.Length
        For i = 1 To Me.Zielauswahl.Length
            Me.matrix.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100 / Me.Zielauswahl.Length))
        Next

        Me.matrix.RowCount = Me.Zielauswahl.Length
        For i = 1 To Me.Zielauswahl.Length
            Me.matrix.RowStyles.Add(New RowStyle(SizeType.Percent, 100 / Me.Zielauswahl.Length))
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
        Try
            'Solution-ID
            indID_clicked = s.Labels(valueIndex)

            'Lösung holen
            '------------
            ind = ResultManager.OptResult.getSolution(indID_clicked)

            If (ind.ID = indID_clicked) Then

                'Lösung auswählen (wird von Form1.selectSolution() verarbeitet)
                RaiseEvent pointSelected(ind)

            End If
        Catch
            MsgBox("Lösung nicht auswählbar!", MsgBoxStyle.Information, "Info")
        End Try

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

                    'Roten Punkt zeichnen
                    serie = .getSeriesPoint("ausgewählte Lösungen", "Red", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
                    serie.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID.ToString())

                    'Mark anzeigen
                    serie.Marks.Visible = True
                    serie.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Label
                    serie.Marks.Transparency = 25
                    serie.Marks.ArrowLength = 10
                    serie.Marks.Arrow.Visible = False

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

                    'Serie löschen
                    serie = .getSeriesPoint("ausgewählte Lösungen")
                    serie.Dispose()

                End With
            Next j
        Next i

    End Sub

#End Region 'Lösungsauswahl

End Class