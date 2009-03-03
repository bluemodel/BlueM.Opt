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

Imports System.Windows.Forms
Imports System.Drawing

''' <summary>
''' Zeigt den Lösungs- oder Entscheidungsraum in Form einer Scatterplot-Matrix an
''' </summary>
Partial Public Class Scatterplot
    Inherits System.Windows.Forms.Form

#Region "Eigenschaften"

    'Das Problem
    Private mProblem As EVO.Common.Problem

    Private Diags(,) As EVO.Diagramm.Diagramm
    Private dimension As Integer
    Private OptResult, OptResultRef As EVO.OptResult.OptResult
    Private Auswahl() As Integer
    Private ShowSekPopOnly, ShowRefResult As Boolean
    Private ShownSpace As EVO.Common.SPACE

#End Region 'Eigenschaften

#Region "Events"

    ''' <summary>
    ''' Event wird ausgelöst, wenn in der Scatterplot-Matrix eine Lösung ausgewählt wird
    ''' </summary>
    ''' <param name="ind">Das ausgewählte Individuum</param>
    ''' <remarks>wird von Form1.selectSolution() verarbeitet</remarks>
    Public Event pointSelected(ByVal ind As Common.Individuum)

#End Region 'Events

#Region "Methoden"

    ''' <summary>
    ''' Konstruktor
    ''' </summary>
    ''' <param name="prob">Das Optimierungsproblem</param>
    ''' <param name="optres">Das Optimierungsergebnis</param>
    ''' <param name="optresref">Ein Referenz-Optimierungsergebnis (darf Nothing sein)</param>
    Public Sub New(ByRef prob As EVO.Common.Problem, ByVal optres As EVO.OptResult.OptResult, ByVal optresref As EVO.OptResult.OptResult)

        Dim Dialog As ScatterplotDialog
        Dim diagresult As DialogResult

        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        'Problem speichern
        Me.mProblem = prob

        'Optimierungsergebnis übergeben
        Me.OptResult = optres
        Me.OptResultRef = optresref

        'Scatterplot-Dialog aufrufen
        Dialog = New EVO.Diagramm.ScatterplotDialog(Me.mProblem, Not IsNothing(Me.OptResultRef))
        diagresult = Dialog.ShowDialog()

        If (diagresult = DialogResult.OK) Then
            'Einstellungen übernehmen
            Me.ShownSpace = Dialog.selectedSpace
            Me.Auswahl = Dialog.selectedVariables
            Me.ShowSekPopOnly = Dialog.ShowSekPopOnly
            Me.ShowRefResult = Dialog.ShowRefResult

            Application.DoEvents()

            'Anzeigen
            Call Me.Display()
        Else
            'Abbruch
            Me.Dispose()
        End If

    End Sub

    'Scatterplot-Matrix anzeigen
    '***************************
    Private Sub Display()

        'Matrix dimensionieren
        Call Me.dimensionieren(Me.Auswahl.Length)

        'Diagramme zeichnen
        Select Case Me.ShownSpace

            Case Common.SPACE.SolutionSpace
                Me.Text &= " - Solution Space"
                Call Me.draw_solutionspace()

            Case Common.SPACE.DecisionSpace
                Me.Text &= " - Decision Space"
                Call Me.draw_decisionspace()

        End Select

        'Bereits ausgewählte Lösungen anzeigen
        For Each ind As Common.Individuum In Me.OptResult.getSelectedSolutions
            Call Me.showSelectedSolution(ind)
        Next

        Call Me.Show()
        Call Me.BringToFront()

    End Sub

    'Lösungsraum zeichnen
    '********************
    Private Sub draw_solutionspace()

        Dim i, j As Integer
        Dim xAchse, yAchse As String
        Dim min() As Double
        Dim max() As Double
        Dim ind As EVO.Common.Individuum
        Dim serie, serie_inv As Steema.TeeChart.Styles.Series
        Dim shape1 As Steema.TeeChart.Styles.Shape

        'Min und Max für Achsen bestimmen
        '--------------------------------
        ReDim min(Me.dimension - 1)
        ReDim max(Me.dimension - 1)
        For i = 0 To Me.dimension - 1
            min(i) = Double.MaxValue
            max(i) = Double.MinValue
            If (Me.ShowSekPopOnly) Then
                'Nur Sekundäre Population
                For Each ind In Me.OptResult.getSekPop()
                    min(i) = Math.Min(ind.Objectives(Me.Auswahl(i)), min(i))
                    max(i) = Math.Max(ind.Objectives(Me.Auswahl(i)), max(i))
                Next
            Else
                'Alle Lösungen
                For Each ind In Me.OptResult.Solutions
                    min(i) = Math.Min(ind.Objectives(Me.Auswahl(i)), min(i))
                    max(i) = Math.Max(ind.Objectives(Me.Auswahl(i)), max(i))
                Next
            End If
            'Startwert
            ind = Me.OptResult.getSolution(1)
            min(i) = Math.Min(ind.Objectives(Me.Auswahl(i)), min(i))
            max(i) = Math.Max(ind.Objectives(Me.Auswahl(i)), max(i))
            'IstWerte
            If (Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).hasIstWert) Then
                min(i) = Math.Min(Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).IstWert, min(i))
                max(i) = Math.Max(Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).IstWert, max(i))
            End If
            'Vergleichsergebnis
            If (Me.ShowRefResult) Then
                For Each ind In Me.OptResultRef.getSekPop()
                    min(i) = Math.Min(ind.Objectives(Me.Auswahl(i)), min(i))
                    max(i) = Math.Max(ind.Objectives(Me.Auswahl(i)), max(i))
                Next
            End If

        Next

        'Schleife über Spalten
        '---------------------
        For i = 0 To Me.dimension - 1
            'Schleife über Reihen
            '--------------------
            For j = 0 To Me.dimension - 1

                'Neues Diagramm erstellen
                Me.Diags(i, j) = New EVO.Diagramm.Diagramm()
                Me.matrix.Controls.Add(Me.Diags(i, j), i, j)

                With Me.Diags(i, j)

                    AddHandler .DoubleClick, AddressOf Me.ShowEditor

                    'Diagramm formatieren
                    '====================
                    .Header.Visible = False
                    .Aspect.View3D = False
                    .Legend.Visible = False
                    .BackColor = Color.White
                    .Panel.Gradient.Visible = False
                    .Panel.Brush.Color = Color.White
                    .Walls.Back.Transparent = False
                    .Walls.Back.Gradient.Visible = False
                    .Walls.Back.Color = Color.White

                    'Achsen
                    '------
                    'Titel
                    xAchse = Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).Bezeichnung
                    yAchse = Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(j)).Bezeichnung

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
                    ElseIf (i = Me.dimension - 1) Then
                        'Achse rechts anzeigen
                        .Axes.Left.OtherSide = True
                    Else
                        'Achse verstecken
                        .Axes.Left.Title.Visible = False
                        .Axes.Left.Labels.CustomSize = 1
                        .Axes.Left.Labels.Font.Color = System.Drawing.Color.Empty
                    End If
                    'XAchsen
                    If (j = 0) Then
                        'Achse oben anzeigen
                        .Axes.Bottom.OtherSide = True
                    ElseIf (j = Me.dimension - 1) Then
                        'Achse standardmäßig anzeigen
                    Else
                        'Achse verstecken
                        .Axes.Bottom.Title.Visible = False
                        .Axes.Bottom.Labels.CustomSize = 1
                        .Axes.Bottom.Labels.Font.Color = System.Drawing.Color.Empty
                    End If

                    'IstWerte eintragen
                    '==================
                    If (i <> j And _
                        (Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).hasIstWert Or _
                        Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(j)).hasIstWert)) Then

                        shape1 = New Steema.TeeChart.Styles.Shape(.Chart)
                        shape1.Style = Steema.TeeChart.Styles.ShapeStyles.Rectangle
                        shape1.Title = "Verbesserungsbereich"

                        'Shape formatieren
                        shape1.Color = Color.Black
                        shape1.Brush.Style = Drawing2D.HatchStyle.DarkUpwardDiagonal
                        shape1.Brush.Color = Color.Black
                        shape1.Brush.ForegroundColor = Color.White
                        shape1.Brush.Transparency = 75
                        shape1.Pen.Transparency = 0
                        shape1.Pen.Color = Color.Red
                        shape1.Pen.Width = 1

                        If (Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).hasIstWert And _
                             Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(j)).hasIstWert) Then
                            'X- und Y-Werte:
                            '---------------
                            shape1.X0 = min(i) * 0.9 ^ (min(i) / Math.Abs(min(i)))
                            shape1.X1 = Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).IstWert
                            shape1.Y0 = min(j) * 0.9 ^ (min(j) / Math.Abs(min(j)))
                            shape1.Y1 = Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(j)).IstWert

                        ElseIf (Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).hasIstWert) Then
                            'Nur X-Wert:
                            '-----------
                            shape1.X0 = min(i) * 0.9 ^ (min(i) / Math.Abs(min(i)))
                            shape1.X1 = Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).IstWert
                            shape1.Y0 = min(j) * 0.9 ^ (min(j) / Math.Abs(min(j)))
                            shape1.Y1 = max(j) * 1.1 ^ (max(j) / Math.Abs(max(j)))

                        ElseIf (Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(j)).hasIstWert) Then
                            'Nur Y-Wert:
                            '-----------
                            shape1.X0 = min(i) * 0.9 ^ (min(i) / Math.Abs(min(i)))
                            shape1.X1 = max(i) * 1.1 ^ (max(i) / Math.Abs(max(i)))
                            shape1.Y0 = min(j) * 0.9 ^ (min(j) / Math.Abs(min(j)))
                            shape1.Y1 = Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(j)).IstWert
                        End If

                    End If

                    'Lösungen eintragen
                    '==================
                    If (Me.ShowSekPopOnly) Then
                        'Nur Sekundäre Population
                        '------------------------
                        serie = .getSeriesPoint(xAchse & ", " & yAchse, "Green", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind In Me.OptResult.getSekPop()
                            serie.Add(ind.Objectives(Me.Auswahl(i)), ind.Objectives(Me.Auswahl(j)), ind.ID.ToString())
                        Next
                    Else
                        'Alle Lösungen
                        '-------------
                        serie = .getSeriesPoint(xAchse & ", " & yAchse, "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        serie_inv = .getSeriesPoint(xAchse & ", " & yAchse & " (ungültig)", "Gray", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind In Me.OptResult.Solutions
                            'Constraintverletzung prüfen
                            If (ind.Is_Feasible) Then
                                'gültige Lösung Zeichnen
                                serie.Add(ind.Objectives(Me.Auswahl(i)), ind.Objectives(Me.Auswahl(j)), ind.ID.ToString())
                            Else
                                'ungültige Lösung zeichnen
                                serie_inv.Add(ind.Objectives(Me.Auswahl(i)), ind.Objectives(Me.Auswahl(j)), ind.ID.ToString())
                            End If
                        Next
                    End If

                    'Startwert
                    '=========
                    serie = .getSeriesPoint("Startwert", "Yellow", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                    ind = Me.OptResult.getSolution(1)
                    serie.Add(ind.Objectives(Me.Auswahl(i)), ind.Objectives(Me.Auswahl(j)), ind.ID.ToString())


                    'Vergleichsergebnis anzeigen
                    '===========================
                    If (Me.ShowRefResult) Then
                        serie = .getSeriesPoint(xAchse & ", " & yAchse & " (Vergleichsergebnis)", "Blue", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind In Me.OptResultRef.getSekPop()
                            serie.Add(ind.Objectives(Me.Auswahl(i)), ind.Objectives(Me.Auswahl(j)), ind.ID & " (Vergleichsergebnis)")
                        Next
                    End If

                    'Diagramme auf der Diagonalen ausblenden
                    '=======================================
                    If (i = j) Then
                        'Hintergrund grau anzeigen
                        .Walls.Back.Color = Color.LightGray
                        'MarksTips entfernen
                        .Tools.Clear(True)
                        'Serien unsichtbar machen
                        For Each s As Steema.TeeChart.Styles.Points In .Series
                            s.Cursor = Cursors.Default          'Kein Hand-Cursor
                            s.Pointer.Color = Color.Empty       'Punkte unsichtbar
                            s.Pointer.Pen.Color = Color.Empty   'Punkte unsichtbar
                        Next
                    Else
                        'alle anderen kriegen Handler für seriesClick
                        AddHandler .ClickSeries, AddressOf Me.seriesClick
                    End If

                End With
            Next
        Next

    End Sub

    'Entscheidungsraum zeichnen
    '**************************
    Private Sub draw_decisionspace()

        Dim i, j As Integer
        Dim xAchse, yAchse As String
        Dim min() As Double
        Dim max() As Double
        Dim serie, serie_inv As Steema.TeeChart.Styles.Series

        'Min und Max für Achsen bestimmen
        '--------------------------------
        ReDim min(Me.dimension - 1)
        ReDim max(Me.dimension - 1)
        For i = 0 To Me.dimension - 1
            min(i) = Me.mProblem.List_OptParameter(Me.Auswahl(i)).Min
            max(i) = Me.mProblem.List_OptParameter(Me.Auswahl(i)).Max
        Next

        'Schleife über Spalten
        '---------------------
        For i = 0 To Me.dimension - 1
            'Schleife über Reihen
            '--------------------
            For j = 0 To Me.dimension - 1

                'Neues Diagramm erstellen
                Me.Diags(i, j) = New EVO.Diagramm.Diagramm()
                Me.matrix.Controls.Add(Me.Diags(i, j), i, j)

                With Me.Diags(i, j)

                    AddHandler .DoubleClick, AddressOf Me.ShowEditor

                    'Diagramm formatieren
                    '====================
                    .Header.Visible = False
                    .Aspect.View3D = False
                    .Legend.Visible = False
                    .BackColor = Color.White
                    .Panel.Gradient.Visible = False
                    .Panel.Brush.Color = Color.White
                    .Walls.Back.Transparent = False
                    .Walls.Back.Gradient.Visible = False
                    .Walls.Back.Color = Color.White

                    'Achsen
                    '------
                    'Titel
                    xAchse = Me.mProblem.List_OptParameter(Me.Auswahl(i)).Bezeichnung
                    yAchse = Me.mProblem.List_OptParameter(Me.Auswahl(j)).Bezeichnung

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

                    ''Beschriftungsformat
                    'If (max(i) >= 1000 Or min(i) <= -1000) Then .Axes.Bottom.Labels.ValueFormat = "0.##E0"
                    'If (max(j) >= 1000 Or min(j) <= -1000) Then .Axes.Left.Labels.ValueFormat = "0.##E0"

                    'Achsen nur an den Rändern anzeigen
                    '----------------------------------
                    'YAchsen
                    If (i = 0) Then
                        'Achse standardmäßig anzeigen
                    ElseIf (i = Me.dimension - 1) Then
                        'Achse rechts anzeigen
                        .Axes.Left.OtherSide = True
                    Else
                        'Achse verstecken
                        .Axes.Left.Title.Visible = False
                        .Axes.Left.Labels.CustomSize = 1
                        .Axes.Left.Labels.Font.Color = System.Drawing.Color.Empty
                    End If
                    'XAchsen
                    If (j = 0) Then
                        'Achse oben anzeigen
                        .Axes.Bottom.OtherSide = True
                    ElseIf (j = dimension - 1) Then
                        'Achse standardmäßig anzeigen
                    Else
                        'Achse verstecken
                        .Axes.Bottom.Title.Visible = False
                        .Axes.Bottom.Labels.CustomSize = 1
                        .Axes.Bottom.Labels.Font.Color = System.Drawing.Color.Empty
                    End If

                    'Punkte eintragen
                    '================
                    If (Me.ShowSekPopOnly) Then
                        'Nur Sekundäre Population
                        '------------------------
                        serie = .getSeriesPoint(xAchse & ", " & yAchse, "Green", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind As Common.Individuum In Me.OptResult.getSekPop()
                            serie.Add(ind.OptParameter_RWerte(Me.Auswahl(i)), ind.OptParameter_RWerte(Me.Auswahl(j)), ind.ID.ToString())
                        Next
                    Else
                        'Alle Lösungen
                        '-------------
                        serie = .getSeriesPoint(xAchse & ", " & yAchse, "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        serie_inv = .getSeriesPoint(xAchse & ", " & yAchse & " (ungültig)", "Gray", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind As Common.Individuum In Me.OptResult.Solutions
                            'Constraintverletzung prüfen
                            If (ind.Is_Feasible) Then
                                'gültige Lösung Zeichnen
                                serie.Add(ind.OptParameter_RWerte(Me.Auswahl(i)), ind.OptParameter_RWerte(Me.Auswahl(j)), ind.ID.ToString())
                            Else
                                'ungültige Lösung zeichnen
                                serie_inv.Add(ind.OptParameter_RWerte(Me.Auswahl(i)), ind.OptParameter_RWerte(Me.Auswahl(j)), ind.ID.ToString())
                            End If
                        Next
                    End If

                    'Vergleichsergebnis anzeigen
                    '===========================
                    If (Me.ShowRefResult) Then
                        serie = .getSeriesPoint(xAchse & ", " & yAchse & " (Vergleichsergebnis)", "Blue", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind As Common.Individuum In Me.OptResultRef.getSekPop()
                            serie.Add(ind.OptParameter_RWerte(Me.Auswahl(i)), ind.OptParameter_RWerte(Me.Auswahl(j)), ind.ID & " (Vergleichsergebnis)")
                        Next
                    End If

                    'Startwerte der Parameter eintragen
                    '==================================
                    serie = .getSeriesPoint("Startwert", "Yellow", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                    serie.Add(Me.mProblem.List_OptParameter(Me.Auswahl(i)).StartWert, Me.mProblem.List_OptParameter(Me.Auswahl(j)).StartWert, "Startwert")

                    'Diagramme auf der Diagonalen ausblenden
                    '=======================================
                    If (i = j) Then
                        'Hintergrund grau anzeigen
                        .Walls.Back.Color = Color.LightGray
                        'MarksTips entfernen
                        .Tools.Clear(True)
                        'Serien unsichtbar machen
                        For Each s As Steema.TeeChart.Styles.Points In .Series
                            s.Cursor = Cursors.Default          'Kein Hand-Cursor
                            s.Pointer.Color = Color.Empty       'Punkte unsichtbar
                            s.Pointer.Pen.Color = Color.Empty   'Punkte unsichtbar
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
    Private Sub dimensionieren(ByVal _dimension As Integer)

        Me.dimension = _dimension

        ReDim Me.Diags(Me.dimension - 1, Me.dimension - 1)

        Dim i As Integer

        Me.matrix.Name = "Matrix"

        Me.matrix.ColumnCount = Me.dimension
        For i = 1 To Me.dimension
            Me.matrix.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100 / Me.dimension))
        Next

        Me.matrix.RowCount = Me.dimension
        For i = 1 To Me.dimension
            Me.matrix.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100 / Me.dimension))
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
            ind = Me.OptResult.getSolution(indID_clicked)

            If (ind.ID = indID_clicked) Then

                'Lösung auswählen (wird von Form1.selectSolution() verarbeitet)
                RaiseEvent pointSelected(ind)

            End If

        Catch ex As Exception
            MsgBox("Lösung nicht auswählbar!", MsgBoxStyle.Information)
        End Try

    End Sub

    ''' <summary>
    ''' Eine ausgewählte Lösung in den Diagrammen anzeigen
    ''' </summary>
    ''' <param name="ind">das ausgewählte Individuum</param>
    ''' <remarks>wird von Form1.selectSolution() aufgerufen</remarks>
    Public Sub showSelectedSolution(ByVal ind As Common.Individuum)

        Dim serie As Steema.TeeChart.Styles.Series
        Dim i, j As Integer

        'Lösung in alle Diagramme eintragen
        '----------------------------------
        For i = 0 To dimension - 1
            For j = 0 To dimension - 1

                With Me.Diags(i, j)

                    'Roten Punkt zeichnen
                    serie = .getSeriesPoint("ausgewählte Lösungen", "Red", Steema.TeeChart.Styles.PointerStyles.Circle, 3)

                    Select Case Me.ShownSpace
                        Case Common.SPACE.SolutionSpace
                            serie.Add(ind.Objectives(Me.Auswahl(i)), ind.Objectives(Me.Auswahl(j)), ind.ID.ToString())
                        Case Common.SPACE.DecisionSpace
                            serie.Add(ind.OptParameter_RWerte(i), ind.OptParameter_RWerte(j), ind.ID.ToString())
                    End Select

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

    ''' <summary>
    ''' Serie der ausgewählten Lösungen löschen
    ''' </summary>
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

#End Region 'Methoden

End Class