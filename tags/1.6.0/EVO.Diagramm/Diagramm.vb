' Copyright (c) BlueM Dev Group
' Website: http://bluemodel.org
' 
' All rights reserved.
' 
' Released under the BSD-2-Clause License:
' 
' Redistribution and use in source and binary forms, with or without modification, 
' are permitted provided that the following conditions are met:
' 
' * Redistributions of source code must retain the above copyright notice, this list 
'   of conditions and the following disclaimer.
' * Redistributions in binary form must reproduce the above copyright notice, this list 
'   of conditions and the following disclaimer in the documentation and/or other materials 
'   provided with the distribution.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
' EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
' OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
' SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT 
' OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
' HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
' TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
' EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'--------------------------------------------------------------------------------------------
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
            MsgBox("Fehler in TeeChart!" & EVO.Common.eol & ex.Message, MsgBoxStyle.Critical)
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

#Region "Misc"

    ''' <summary>
    ''' Speichert die verwendeten Farben für die bisherigen Pfade und generiert neue, falls erforderlich
    ''' </summary>
    ''' <param name="ColorArray">Array von Farben (Objects)???</param>
    ''' <param name="ind">Das CES-Individuum, für welches eine Farbe bestimmt werden soll</param>
    ''' <returns>ein Farbe</returns>
    ''' <remarks>nur für CES!</remarks>
    Public Shared Function ColorManagement(ByRef ColorArray(,) As Object, ByVal ind As Common.Individuum_CES) As Color

        'TODO: Funktion ColorManagement generisch machen!

        Dim i, j As Integer
        Dim count As Integer
        Dim Farbe As Color = Color.White

        'Falls der Pfad schon vorhanden ist wird diese Farbe verwendet
        For i = 0 To ColorArray.GetUpperBound(1)
            count = 0
            For j = 1 To ColorArray.GetUpperBound(0)
                If ColorArray(j, i) = ind.Path(j - 1) Then
                    count += 1
                End If
            Next
            If count = ind.Path.GetLength(0) Then
                Farbe = ColorArray(0, i)
            End If
        Next


        'Für Farbverläufe __________________________________________________________________________________

        'If ColorAray.GetLength(1) = 0 then
        '    ReDim Preserve ColorAray(ColorAray.GetUpperBound(0), ColorAray.GetLength(1))
        '    Farbe = Color.FromArgb(255, 0, 255, 255)
        'ElseIf Farbe = Color.White Then

        '    Farbe = ColorAray(0, ColorAray.GetUpperBound(1))
        '    ReDim Preserve ColorAray(ColorAray.GetUpperBound(0), ColorAray.GetLength(1))

        '    Dim R As Integer = Farbe.R
        '    Dim G As Integer = Farbe.G
        '    Dim B As Integer = Farbe.B

        '    G = G - 50
        '    If G < 0 then
        '        G = 255
        '        B = B - 50
        '        If B < 100
        '            B = 255
        '            R = R + 50
        '            If R > 255
        '                Throw New Exception("Die Anzahl der farben für die verschiedenen Pfade ist erschöpft")
        '            End If
        '        End If
        '    End If

        '    Farbe = color.FromArgb(255, R, G, B)
        '    ColorAray(0, ColorAray.GetUpperBound(1)) = Farbe

        '    For i = 1 To ColorAray.GetUpperBound(0)
        '        ColorAray(i, ColorAray.GetUpperBound(1)) = ind.Path(i - 1)
        '    Next

        'End If


        'Für zufällige Farben _________________________________________________________________________________

        If Farbe = Color.White Then
            ReDim Preserve ColorArray(ColorArray.GetUpperBound(0), ColorArray.GetLength(1))
            Dim NeueFarbe As Boolean = True
            Dim CountFarbe As Integer = 0
            Do
                Randomize()
                'Genriert Zahl zwischen
                Farbe = Drawing.Color.FromArgb(255, CInt(Int((50 * Rnd()) + 1)) * 5, _
                                                    CInt(Int((50 * Rnd()) + 1)) * 5, _
                                                    CInt(Int((50 * Rnd()) + 1)) * 5)
                For i = 0 To ColorArray.GetUpperBound(1)
                    If Farbe = ColorArray(0, i) Then
                        NeueFarbe = False
                    End If
                Next
                CountFarbe += 1

                If CountFarbe > 15000 Then
                    Farbe = Color.White
                    NeueFarbe = True
                End If
                'If CountFarbe > 15000 Then Throw New Exception("Die Anzahl der farben für die verschiedenen Pfade ist erschöpft")
            Loop Until NeueFarbe = True
            ColorArray(0, ColorArray.GetUpperBound(1)) = Farbe
            For i = 1 To ColorArray.GetUpperBound(0)
                ColorArray(i, ColorArray.GetUpperBound(1)) = ind.Path(i - 1)
            Next
        End If

        Return Farbe

    End Function

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
