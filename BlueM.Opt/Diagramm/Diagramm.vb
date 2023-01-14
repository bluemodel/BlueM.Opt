'BlueM.Opt
'Copyright (C) BlueM Dev Group
'Website: <https://www.bluemodel.org>
'
'This program is free software: you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.
'
'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
'GNU General Public License for more details.
'
'You should have received a copy of the GNU General Public License
'along with this program. If not, see <https://www.gnu.org/licenses/>.
'
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

Imports System.Drawing

''' <summary>
''' Klasse stellt Diagrammfunktionalitäten zur Verfügung
''' </summary>
''' <remarks>Erweiterung der Klasse Steema.TeeChart.TChart</remarks>
Public Class Diagramm
    Inherits Steema.TeeChart.TChart

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

    ''' <summary>
    ''' Constructor overload which adds some default chart settings
    ''' </summary>
    Public Sub New()
        MyBase.New()
        Me.Chart.Axes.Bottom.Grid.Visible = True
    End Sub

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
            Call Steema.TeeChart.Editor.Show(Me)
        Catch ex As Exception
            MsgBox("Fehler in TeeChart!" & BlueM.Opt.Common.eol & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

#End Region 'Diagrammfunktionen

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
        Dim baseColor, borderColor As Color
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
            baseColor = Color.FromName(colorName)
            serie.Pointer.Color = baseColor
            'Border-Color etwas dunkler
            borderColor = getDarkerColor(baseColor)
            serie.Pointer.Pen.Color = borderColor
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
        Dim baseColor, borderColor As Color
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
            baseColor = Drawing.Color.FromName(colorName)
            serie.Color = baseColor
            'Border-Color etwas dunkler
            borderColor = getDarkerColor(baseColor)
            serie.Pointer.Pen.Color = borderColor
        End If

        'TODO: MarksTip funktioniert momentan nur in der XY-Ebene korrekt (#165)
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

#Region "Misc"

    ''' <summary>
    ''' Erzeugt von einer übergebenen Farbe eine etwas dunklere Farbe
    ''' </summary>
    ''' <param name="baseColor">die Basisfarbe</param>
    ''' <returns>eine etwas dunklere Farbe</returns>
    Private Shared Function getDarkerColor(ByVal baseColor As Color) As Color

        Const colorshift As Integer = 100
        Dim newColor As Color

        newColor = Color.FromArgb(baseColor.A, Math.Max(baseColor.R - colorshift, 0), Math.Max(baseColor.G - colorshift, 0), Math.Max(baseColor.B - colorshift, 0))

        Return newColor

    End Function

#End Region 'Misc

End Class
