Public Class Diagramm
    Inherits Steema.TeeChart.TChart

    Public Structure Achse
            Public Name as String
            Public Auto as Boolean
            Public Max as Double
    End Structure

    'TeeChart zurücksetzen
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
        End With

    End Sub

    'Serien-Initialisierung (Punkt)
    'gibt die SeriesNo zurück
    '********************************
    Public Function prepareSeriesPoint(ByVal title As String, _
                                      Optional ByVal colorName As String = "", _
                                      Optional ByVal style As Steema.TeeChart.Styles.PointerStyles = Steema.TeeChart.Styles.PointerStyles.Circle, _
                                      Optional ByVal size As Integer = 3) As Integer

        Dim SeriesNo As Integer

        If (Not seriesExists(title, SeriesNo)) Then
            'Serie neu hinzufügen
            Dim tmpSeries As New Steema.TeeChart.Styles.Points(Me.Chart)
            tmpSeries.Title = title
            tmpSeries.Pointer.Style = style
            tmpSeries.Pointer.HorizSize = size
            tmpSeries.Pointer.VertSize = size
            If (Not colorName = "") Then
                tmpSeries.Color = Drawing.Color.FromName(colorName)
            End If

            Call Me.add_MarksTips()

            SeriesNo = Me.Chart.Series.Count - 1
        Else
            'Serie besteht schon
        End If

        Return SeriesNo

    End Function

    'Serien-Initialisierung (Linie)
    'gibt die SeriesNo zurück
    '********************************
    Public Function prepareSeriesLine(ByVal title As String, _
                                      Optional ByVal colorName As String = "") As Integer

        Dim SeriesNo As Integer

        If (Not seriesExists(title, SeriesNo)) Then
            'Serie neu hinzufügen
            Dim tmpSeries As New Steema.TeeChart.Styles.Line(Me.Chart)
            tmpSeries.Title = title
            If (Not colorName = "") Then
                tmpSeries.Color = Drawing.Color.FromName(colorName)
            End If

            Call Me.add_MarksTips()

            SeriesNo = Me.Chart.Series.Count - 1
        Else
            'Serie besteht schon
        End If

        Return SeriesNo

    End Function

    'Überprüfen, ob Serie bereits existiert
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

    'Serien werden von Hinten gelöscht
    '*********************************
    Sub DeleteSeries(ByVal Max As Integer, ByVal Min As Integer)

        Dim i As Integer
        For i = Max To Min Step -1
            If Me.Chart.Series.Count - 1 = i Then
                Me.Chart.Series.Remove(Me.Chart.Series(i))
            End If
        Next

    End Sub

    'MarksTips zu Serien hinzufügen
    '******************************
    Public Sub add_MarksTips()
        Dim tmpMarksTip As Steema.TeeChart.Tools.MarksTip
        For i As Integer = 0 To Me.Chart.Series.Count - 1
            tmpMarksTip = New Steema.TeeChart.Tools.MarksTip(Me.Chart)
            tmpMarksTip.Series = Me.Chart.Series(i)
            tmpMarksTip.MouseAction = Steema.TeeChart.Tools.MarksTipMouseAction.Move
            tmpMarksTip.MouseDelay = 10 'millisekunden
            tmpMarksTip.Style = Steema.TeeChart.Styles.MarksStyles.XY
        Next
    End Sub

End Class
