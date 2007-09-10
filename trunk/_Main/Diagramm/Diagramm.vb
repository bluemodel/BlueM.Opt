Public Class Diagramm
    Inherits Steema.TeeChart.TChart

    Public Structure Achse
            Public Name as String
            Public Auto as Boolean
            Public Max as Double
    End Structure

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
                Dim rotate as New Steema.TeeChart.Tools.Rotate(.Chart)
                rotate.Button = Windows.Forms.MouseButtons.Right

            End If

        End With

    End Sub

    'Serien-Initialisierung (Punkt)
    'gibt die SeriesNo zur�ck
    '********************************
    Public Function prepareSeriesPoint(ByVal title As String, _
                                      Optional ByVal colorName As String = "", _
                                      Optional ByVal style As Steema.TeeChart.Styles.PointerStyles = Steema.TeeChart.Styles.PointerStyles.Circle, _
                                      Optional ByVal size As Integer = 3) As Integer

        Dim SeriesNo As Integer

        If (Not seriesExists(title, SeriesNo)) Then
            'Serie neu hinzuf�gen
            Dim tmpSeries As New Steema.TeeChart.Styles.Points(Me.Chart)
            tmpSeries.Title = title
            tmpSeries.Pointer.Style = style
            tmpSeries.Pointer.HorizSize = size
            tmpSeries.Pointer.VertSize = size
            If (Not colorName = "") Then
                tmpSeries.Color = Drawing.Color.FromName(colorName)
            End If

            Call Me.add_MarksTips(tmpSeries)
            tmpSeries.Cursor = Cursors.Hand

            SeriesNo = Me.Chart.Series.Count - 1
        Else
            'Serie besteht schon
        End If

        Return SeriesNo

    End Function

    'Serien-Initialisierung (Linie)
    'gibt die SeriesNo zur�ck
    '********************************
    Public Function prepareSeriesLine(ByVal title As String, _
                                      Optional ByVal colorName As String = "") As Integer

        Dim SeriesNo As Integer

        If (Not seriesExists(title, SeriesNo)) Then
            'Serie neu hinzuf�gen
            Dim tmpSeries As New Steema.TeeChart.Styles.Line(Me.Chart)
            tmpSeries.Title = title
            If (Not colorName = "") Then
                tmpSeries.Color = Drawing.Color.FromName(colorName)
            End If

            Call Me.add_MarksTips(tmpSeries)

            SeriesNo = Me.Chart.Series.Count - 1
        Else
            'Serie besteht schon
        End If

        Return SeriesNo

    End Function

    'Serien-Initialisierung (3DPunkt)
    'gibt die SeriesNo zur�ck
    '********************************
    Public Function prepareSeries3DPoint(ByVal title As String, _
                                      Optional ByVal colorName As String = "", _
                                      Optional ByVal style As Steema.TeeChart.Styles.PointerStyles = Steema.TeeChart.Styles.PointerStyles.Circle, _
                                      Optional ByVal size As Integer = 3) As Integer

        Dim SeriesNo As Integer

        If (Not seriesExists(title, SeriesNo)) Then
            'Serie neu hinzuf�gen
            Dim tmpSeries As New Steema.TeeChart.Styles.Points3D(Me.Chart)
            tmpSeries.Title = title
            tmpSeries.Pointer.Style = style
            tmpSeries.Pointer.HorizSize = size
            tmpSeries.Pointer.VertSize = size
            tmpSeries.Pointer.Draw3D = True
            tmpSeries.Depth = size
            tmpSeries.LinePen.Visible = False
            tmpSeries.ColorEach = False
            If (Not colorName = "") Then
                tmpSeries.Color = Drawing.Color.FromName(colorName)
            End If

            'BUG: TeeChart MarksTip funktioniert momentan nur in der XY-Ebene korrekt
            'Siehe http://www.teechart.net/support/viewtopic.php?t=5982&highlight=&sid=4db52d0d1a4b78f30842ede881ce5bef
            Call Me.add_MarksTips(tmpSeries)
            tmpSeries.Cursor = Cursors.Hand

            SeriesNo = Me.Chart.Series.Count - 1
        Else
            'Serie besteht schon
        End If

        Return SeriesNo

    End Function

    '�berpr�fen, ob Serie bereits existiert
    '**************************************
    Private Function seriesExists(ByVal title As String, ByRef SeriesNo As Integer) As Boolean

        Dim i As Integer

        seriesExists = False

        For i = 0 To Me.Chart.Series.Count - 1
            If (Me.Chart.Series(i).Title = title) Then
                seriesExists = True
                SeriesNo = i
                Exit For
            End If
        Next

        Return seriesExists

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
    Public Sub add_MarksTips(ByVal serie As Steema.TeeChart.Styles.Series)

        Dim tmpMarksTip As Steema.TeeChart.Tools.MarksTip
        tmpMarksTip = New Steema.TeeChart.Tools.MarksTip(Me.Chart)
        tmpMarksTip.Series = serie
        tmpMarksTip.MouseAction = Steema.TeeChart.Tools.MarksTipMouseAction.Move
        tmpMarksTip.MouseDelay = 10 'millisekunden
        tmpMarksTip.Style = Steema.TeeChart.Styles.MarksStyles.XY

    End Sub

End Class
