Public Class Diagramm
    Inherits Steema.TeeChart.TChart

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse Diagramm                                                       ****
    '****                                                                       ****
    '**** Erweiterung der Klasse Steema.TeeChart.TChart                         ****
    '****                                                                       ****
    '**** Autoren: Felix Froehlich, Christoph Hübner                            ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

#Region "Eigenschaften"

    Public Structure Achse
        Public Title As String
        Public Automatic As Boolean
        Public Minimum As Double
        Public Maximum As Double
        Public Increment As Double
    End Structure

#End Region


#Region "Diagrammfunktionen"

    'Diagramm zurücksetzen
    '*********************
    Public Sub Reset()
        With Me
            .Clear()
            .Header.Text = "EVO"
            .Chart.Axes.Bottom.Title.Caption = ""
            .Chart.Axes.Left.Title.Caption = ""
        End With
    End Sub

    'Diagramm bearbeiten
    '*******************
    Public Sub TChartEdit(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.DoubleClick
        Try
            Me.ShowEditor()
        Catch ex As Exception
            MsgBox("Fehler in TeeChart!" & EVO.Common.eol & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

#End Region 'Diagramfunktionen

#Region "Serienverwaltung"

    'Serien-Initialisierung (Punkt)
    'gibt die Serie zurück
    '******************************
    Public Function getSeriesPoint(ByVal title As String, _
                                      Optional ByVal colorName As String = "", _
                                      Optional ByVal style As Steema.TeeChart.Styles.PointerStyles = Steema.TeeChart.Styles.PointerStyles.Circle, _
                                      Optional ByVal size As Integer = 3, _
                                      Optional ByVal ColEach As Boolean = False) As Steema.TeeChart.Styles.Points

        Dim i As Integer
        Dim serie As Steema.TeeChart.Styles.Points

        'Überprüfen, ob Serie bereits existiert
        For i = 0 To Me.Chart.Series.Count - 1
            If (Me.Chart.Series(i).Title = title) Then
                serie = Me.Chart.Series(i)
                Return serie
            End If
        Next

        'Sonst Serie neu hinzufügen
        serie = New Steema.TeeChart.Styles.Points(Me.Chart)
        serie.Title = title
        serie.Pointer.Style = style
        serie.Pointer.HorizSize = size
        serie.Pointer.VertSize = size
        serie.ColorEach = ColEach
        If (Not colorName = "") Then
            serie.Color = Drawing.Color.FromName(colorName)
        End If

        Call Me.add_MarksTips(serie)
        serie.Cursor = Windows.Forms.Cursors.Hand

        Return serie

    End Function

    'Serien-Initialisierung (Linie)
    'gibt die Serie zurück
    '******************************
    Public Function getSeriesLine(ByVal title As String, _
                                      Optional ByVal colorName As String = "") As Steema.TeeChart.Styles.Line

        Dim i As Integer
        Dim serie As Steema.TeeChart.Styles.Line

        'Überprüfen, ob Serie bereits existiert
        For i = 0 To Me.Chart.Series.Count - 1
            If (Me.Chart.Series(i).Title = title) Then
                serie = Me.Chart.Series(i)
                Return serie
            End If
        Next

        'Sonst Serie neu hinzufügen
        serie = New Steema.TeeChart.Styles.Line(Me.Chart)
        serie.Title = title
        If (Not colorName = "") Then
            serie.Color = Drawing.Color.FromName(colorName)
        End If

        Call Me.add_MarksTips(serie, Steema.TeeChart.Styles.MarksStyles.XY)

        Return serie

    End Function

    'Serien-Initialisierung (3DPunkt)
    'gibt die Serie zurück
    '********************************
    Public Function getSeries3DPoint(ByVal title As String, _
                                      Optional ByVal colorName As String = "", _
                                      Optional ByVal style As Steema.TeeChart.Styles.PointerStyles = Steema.TeeChart.Styles.PointerStyles.Circle, _
                                      Optional ByVal size As Integer = 3, _
                                      Optional ByVal ColEach As Boolean = False) As Steema.TeeChart.Styles.Points3D

        Dim i As Integer
        Dim serie As New Steema.TeeChart.Styles.Points3D

        'Überprüfen, ob Serie bereits existiert
        For i = 0 To Me.Chart.Series.Count - 1
            If (Me.Chart.Series(i).Title = title) Then
                serie = Me.Chart.Series(i)
                Return serie
            End If
        Next

        'Sonst Serie neu hinzufügen
        serie = New Steema.TeeChart.Styles.Points3D(Me.Chart)
        serie.Title = title
        serie.Pointer.Style = style
        serie.Pointer.HorizSize = size
        serie.Pointer.VertSize = size
        serie.Pointer.Draw3D = True
        serie.Depth = size
        serie.LinePen.Visible = False
        serie.ColorEach = ColEach
        If (Not colorName = "") Then
            serie.Color = Drawing.Color.FromName(colorName)
        End If

        'BUG 234: MarksTip funktioniert momentan nur in der XY-Ebene korrekt
        Call Me.add_MarksTips(serie)
        serie.Cursor = Windows.Forms.Cursors.Hand

        Return serie

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

    'MarksTips zu einer Serie hinzufügen
    '***********************************
    Public Sub add_MarksTips(ByVal serie As Steema.TeeChart.Styles.Series, Optional ByVal style As Steema.TeeChart.Styles.MarksStyles = Steema.TeeChart.Styles.MarksStyles.Label)

        Dim myMarksTip As Steema.TeeChart.Tools.MarksTip
        myMarksTip = New Steema.TeeChart.Tools.MarksTip(Me.Chart)
        myMarksTip.Series = serie
        myMarksTip.MouseAction = Steema.TeeChart.Tools.MarksTipMouseAction.Move
        myMarksTip.MouseDelay = 10 'millisekunden
        myMarksTip.Style = style

    End Sub

#End Region 'Serienverwaltung

End Class
