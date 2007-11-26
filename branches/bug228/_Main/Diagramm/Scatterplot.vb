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
    Private SekPopOnly As Boolean
    Private ReadOnly Property nOptZiele() As Integer
        Get
            Return Me.OptResult.List_OptZiele.Length()
        End Get
    End Property
    Public Event solutionSelected(sol As Solution)

    'Konstruktor
    '***********
    Public Sub New(ByVal optres As IHWB.EVO.OptResult, ByVal _sekpoponly As Boolean)

        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        'Optimierungsergebnis übergeben
        Me.OptResult = optres

        'SekPop-Flag setzen
        Me.SekPopOnly = _sekpoponly

        'Diagramme zeichnen
        Call Me.zeichnen()

    End Sub

    'Diagramme zeichnen
    '******************
    Private Sub zeichnen()

        'Matrix dimensionieren
        Call Me.dimensionieren()

        Dim i, j As Integer
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
                    '================
                    If (Me.SekPopOnly) Then
                        'Nur Sekundäre Population
                        '------------------------
                        serie = .getSeriesPoint(xAchse & ", " & yAchse, "Green", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each sol As Solution In Me.OptResult.getSekPop()
                            serie.Add(sol.QWerte(i), sol.QWerte(j), sol.ID)
                        Next
                    Else
                        'Alle Lösungen
                        '-------------
                        serie = .getSeriesPoint(xAchse & ", " & yAchse, "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each sol As Solution In Me.OptResult.Solutions
                            'Constraintverletzung prüfen
                            If (Not sol.isValid) Then
                                serie = .getSeriesPoint(xAchse & ", " & yAchse & " (ungültig)", "Gray", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                            End If
                            'Zeichnen
                            serie.Add(sol.QWerte(i), sol.QWerte(j), sol.ID)
                        Next
                    End If

                    If (i = j) Then
                        'Diagramme auf der Diagonalen ausblenden
                        .Walls.Back.Transparent = False     'Grau anzeigen
                        .Tools.Clear(True)                  'Um MarksTips zu entfernen
                        serie.Cursor = Cursors.Default      'Kein Hand-Cursor
                        serie.Color = Color.Empty           'Punkte unsichtbar
                    Else
                        'alle anderen kriegen Handler für selectPoint
                        AddHandler .ClickSeries, AddressOf Me.selectPoint
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

    'Einen Punkt auswählen
    '*********************
    Private Sub selectPoint(ByVal sender As Object, ByVal s As Steema.TeeChart.Styles.Series, ByVal valueIndex As Integer, ByVal e As System.Windows.Forms.MouseEventArgs)

        Dim i, j, solutionID As Integer
        Dim sol As New Solution
        Dim serie As Steema.TeeChart.Styles.Series

        ReDim sol.QWerte(Me.nOptZiele - 1)

        'Punkt-Informationen bestimmen
        '-----------------------------
        'Solution-ID
        solutionID = s.Labels(valueIndex)

        'Lösung holen
        '------------
        sol = Me.OptResult.getSolution(solutionID)

        If (sol.ID = solutionID) Then

            'Lösung in alle Diagramme eintragen
            '----------------------------------
            For i = 0 To Me.Diags.GetUpperBound(0)
                For j = 0 To Me.Diags.GetUpperBound(1)
                    With Me.Diags(i, j)

                        'Roten Punkt zeichnen
                        serie = .getSeriesPoint("ausgewählte Lösungen", "Red", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
                        serie.Add(sol.QWerte(i), sol.QWerte(j), sol.ID)

                        'Mark anzeigen
                        serie.Marks.Visible = True
                        serie.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Label
                        serie.Marks.Transparency = 25
                        serie.Marks.ArrowLength = 10
                        serie.Marks.Arrow.Visible = False

                    End With
                Next j
            Next i

            'Lösung auswählen (wird von Form1.selectSolution() verarbeitet)
            RaiseEvent solutionSelected(sol)

        End If

    End Sub

    'Lösungsauswahl zurücksetzen
    '***************************
    Public Sub clearSelection()

        Dim i, j As Integer
        Dim serie as Steema.TeeChart.Styles.Series

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

End Class