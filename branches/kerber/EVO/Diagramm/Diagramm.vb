Public Class Diagramm
    Inherits Steema.TeeChart.TChart

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse Diagramm                                                       ****
    '****                                                                       ****
    '**** Erweiterung der Klasse Steema.TeeChart.TChart                         ****
    '****                                                                       ****
    '**** Autoren: Felix Froehlich, Christoph H�bner                            ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

#Region "Eigenschaften"

    'Zuordnung zwischen Zielfunktionen und Achsen
    Public ZielIndexX, ZielIndexY, ZielIndexZ As Integer

    'Flag, der anzeigt, ob es sich um ein 3D-Diagramm handelt
    Public is3D As Boolean

    Public Structure Achse
        Public Name As String
        Public Auto As Boolean
        Public Max As Double
    End Structure

#End Region

    'TeeChart zur�cksetzen
    '*********************
    Public Sub Reset()
        With Me
            .Clear()
            .Header.Text = "EVO"
            .Chart.Axes.Bottom.Title.Caption = ""
            .Chart.Axes.Left.Title.Caption = ""
        End With
    End Sub

    'TeeChart Initialisierung (Titel und Achsen)
    '*******************************************
    Public Sub DiagInitialise(ByVal Titel As String, ByVal Achsen As Collection)

        With Me

            .Clear()
            .is3D = False
            .Header.Text = Titel
            .Aspect.View3D = False
            .Legend.Visible = False

            'Formatierung der Axen
            '---------------------
            'X-Achse:
            .Chart.Axes.Bottom.Title.Caption = Achsen(1).Name
            .Chart.Axes.Bottom.Automatic = Achsen(1).Auto
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Bottom.Maximum = Achsen(1).Max
            .Chart.Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
            'Y-Achse:
            .Chart.Axes.Left.Title.Caption = Achsen(2).Name
            .Chart.Axes.Left.Automatic = Achsen(2).Auto
            .Chart.Axes.Left.Minimum = 0
            .Chart.Axes.Left.Maximum = Achsen(2).Max
            .Chart.Axes.Left.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value

            'Bei mehr als 2 Achsen 3D-Diagramm
            '---------------------------------
            If (Achsen.Count > 2) Then

                .is3D = True

                'Z-Achse:
                .Chart.Axes.Depth.Title.Caption = Achsen(3).Name
                .Chart.Axes.Depth.Automatic = Achsen(3).Auto
                .Chart.Axes.Depth.Minimum = 0
                .Chart.Axes.Depth.Maximum = Achsen(3).Max
                .Chart.Axes.Depth.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
                .Chart.Axes.Depth.Visible = True

                '3D-Diagramm vorbereiten
                .Chart.Aspect.View3D = True
                .Chart.Aspect.Chart3DPercent = 90
                .Chart.Aspect.Elevation = 348
                .Chart.Aspect.Orthogonal = False
                .Chart.Aspect.Perspective = 62
                .Chart.Aspect.Rotation = 329
                .Chart.Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality
                .Chart.Aspect.VertOffset = -20
                .Chart.Aspect.Zoom = 66

                'Rotate Tool
                Dim rotate As New Steema.TeeChart.Tools.Rotate(.Chart)
                rotate.Button = Windows.Forms.MouseButtons.Right

            End If

        End With

    End Sub

    'Serien-Initialisierung (Punkt)
    'gibt die Serie zur�ck
    '******************************
    Public Function getSeriesPoint(ByVal title As String, _
                                      Optional ByVal colorName As String = "", _
                                      Optional ByVal style As Steema.TeeChart.Styles.PointerStyles = Steema.TeeChart.Styles.PointerStyles.Circle, _
                                      Optional ByVal size As Integer = 3) As Steema.TeeChart.Styles.Points

        Dim i As Integer
        Dim serie As Steema.TeeChart.Styles.Points

        '�berpr�fen, ob Serie bereits existiert
        For i = 0 To Me.Chart.Series.Count - 1
            If (Me.Chart.Series(i).Title = title) Then
                serie = Me.Chart.Series(i)
                Return serie
            End If
        Next

        'Sonst Serie neu hinzuf�gen
        serie = New Steema.TeeChart.Styles.Points(Me.Chart)
        serie.Title = title
        serie.Pointer.Style = style
        serie.Pointer.HorizSize = size
        serie.Pointer.VertSize = size
        If (Not colorName = "") Then
            serie.Color = Drawing.Color.FromName(colorName)
        End If

        Call Me.add_MarksTips(serie)
        serie.Cursor = Cursors.Hand

        Return serie

    End Function

    'Serien-Initialisierung (Linie)
    'gibt die Serie zur�ck
    '******************************
    Public Function getSeriesLine(ByVal title As String, _
                                      Optional ByVal colorName As String = "") As Steema.TeeChart.Styles.Line

        Dim i As Integer
        Dim serie As Steema.TeeChart.Styles.Line

        '�berpr�fen, ob Serie bereits existiert
        For i = 0 To Me.Chart.Series.Count - 1
            If (Me.Chart.Series(i).Title = title) Then
                serie = Me.Chart.Series(i)
                Return serie
            End If
        Next

        'Sonst Serie neu hinzuf�gen
        serie = New Steema.TeeChart.Styles.Line(Me.Chart)
        serie.Title = title
        If (Not colorName = "") Then
            serie.Color = Drawing.Color.FromName(colorName)
        End If

        Call Me.add_MarksTips(serie, Steema.TeeChart.Styles.MarksStyles.XY)

        Return serie

    End Function

    'Serien-Initialisierung (3DPunkt)
    'gibt die Serie zur�ck
    '********************************
    Public Function getSeries3DPoint(ByVal title As String, _
                                      Optional ByVal colorName As String = "", _
                                      Optional ByVal style As Steema.TeeChart.Styles.PointerStyles = Steema.TeeChart.Styles.PointerStyles.Circle, _
                                      Optional ByVal size As Integer = 3) As Steema.TeeChart.Styles.Points3D

        Dim i As Integer
        Dim serie As New Steema.TeeChart.Styles.Points3D

        '�berpr�fen, ob Serie bereits existiert
        For i = 0 To Me.Chart.Series.Count - 1
            If (Me.Chart.Series(i).Title = title) Then
                serie = Me.Chart.Series(i)
                Return serie
            End If
        Next

        'Sonst Serie neu hinzuf�gen
        serie = New Steema.TeeChart.Styles.Points3D(Me.Chart)
        serie.Title = title
        serie.Pointer.Style = style
        serie.Pointer.HorizSize = size
        serie.Pointer.VertSize = size
        serie.Pointer.Draw3D = True
        serie.Depth = size
        serie.LinePen.Visible = False
        serie.ColorEach = False
        If (Not colorName = "") Then
            serie.Color = Drawing.Color.FromName(colorName)
        End If

        'BUG 234: MarksTip funktioniert momentan nur in der XY-Ebene korrekt
        Call Me.add_MarksTips(serie)
        serie.Cursor = Cursors.Hand

        Return serie

    End Function

    'Serien werden von Hinten gel�scht
    '*********************************
    Sub DeleteSeries(ByVal Max As Integer, ByVal Min As Integer)

        Dim i As Integer
        For i = Max To Min Step -1
            If Me.Chart.Series.Count - 1 = i Then
                Me.Chart.Series.Remove(Me.Chart.Series(i))
            End If
        Next

    End Sub

    'MarksTips zu einer Serie hinzuf�gen
    '***********************************
    Public Sub add_MarksTips(ByVal serie As Steema.TeeChart.Styles.Series, Optional ByVal style As Steema.TeeChart.Styles.MarksStyles = Steema.TeeChart.Styles.MarksStyles.Label)

        Dim myMarksTip As Steema.TeeChart.Tools.MarksTip
        myMarksTip = New Steema.TeeChart.Tools.MarksTip(Me.Chart)
        myMarksTip.Series = serie
        myMarksTip.MouseAction = Steema.TeeChart.Tools.MarksTipMouseAction.Move
        myMarksTip.MouseDelay = 10 'millisekunden
        myMarksTip.Style = style

    End Sub

#Region "L�sungsauswahl"

    'ausgew�hlte L�sung anzeigen
    '***************************
    Friend Sub showSelectedSolution(ByVal ind As Common.Individuum)

        'Sonderfall Sensiplot
        If (Form1.Method = METH_SENSIPLOT) Then
            'BUG 327!
            Exit Sub
        End If

        '2D oder 3D?
        If (Not Me.is3D) Then

            '2D-Diagramm
            '-----------
            Dim serie As Steema.TeeChart.Styles.Series
            serie = Me.getSeriesPoint("ausgew�hlte L�sungen", "Red", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
            serie.Marks.Visible = True
            serie.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Label
            serie.Marks.Transparency = 50
            serie.Marks.ArrowLength = 10
            If (Me.ZielIndexX = -1) Then
                'X-Achse ist Simulations-ID (Single-Objective)
                serie.Add(ind.ID, ind.Zielwerte(Me.ZielIndexY), ind.ID.ToString())
            Else
                'X- und Y-Achsen sind beides Zielwerte
                serie.Add(ind.Zielwerte(Me.ZielIndexX), ind.Zielwerte(Me.ZielIndexY), ind.ID.ToString())
            End If

        Else
            '3D-Diagramm
            '-----------
            Dim serie3D As Steema.TeeChart.Styles.Points3D
            serie3D = Me.getSeries3DPoint("ausgew�hlte L�sungen", "Red", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
            serie3D.Add(ind.Zielwerte(Me.ZielIndexX), ind.Zielwerte(Me.ZielIndexY), ind.Zielwerte(Me.ZielIndexZ), ind.ID.ToString())
            serie3D.Marks.Visible = True
            serie3D.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Label
            serie3D.Marks.Transparency = 50
            serie3D.Marks.ArrowLength = 10

        End If

    End Sub

    'Serie der ausgew�hlten L�sungen l�schen
    '***************************************
    Friend Sub clearSelection()

        If (Not Me.is3D) Then
            '2D-Diagramm
            '-----------
            Dim serie As Steema.TeeChart.Styles.Series
            serie = Me.getSeriesPoint("ausgew�hlte L�sungen")
            serie.Dispose()

        Else
            '3D-Diagramm
            '-----------
            Dim serie3D As Steema.TeeChart.Styles.Points3D
            serie3D = Me.getSeries3DPoint("ausgew�hlte L�sungen")
            serie3D.Dispose()
        End If

        Call Me.Refresh()

    End Sub


#End Region

End Class