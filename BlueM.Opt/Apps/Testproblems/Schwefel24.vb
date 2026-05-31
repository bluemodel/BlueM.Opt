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
Imports System.Data.Common
Imports System.Drawing
Imports BlueM.Opt.Common

''' <summary>
''' Schwefel 2.4: find the minimum (xi=1, F(x)=0)
''' </summary>
Public Class Schwefel24
    Inherits Testproblem

    ''' <summary>
    ''' Problem description
    ''' </summary>
    Public Overrides ReadOnly Property Description As String
        Get
            Return "Schwefel 2.4: find the minimum (xi=1, F(x)=0)"
        End Get
    End Property

    ''' <summary>
    ''' Defines the optimization problem
    ''' </summary>
    ''' <param name="prob">the problem</param>
    Public Overrides Sub DefineProblem(ByRef prob As Problem)

        'store the problem
        Me.mProblem = prob

        Me.mAnzOptPara = 5
        Me.mAnzZiele = 1
        Me.mAnzConstraints = 0
        ReDim Me.mOptPara(Me.mAnzOptPara - 1)
        For i = 0 To Me.mAnzOptPara - 1
            Me.mOptPara(i) = New OptParameter With {
                .Xn = 1
            }
        Next


        Call MyBase.FillProblemDefinition()

    End Sub

    ''' <summary>
    ''' Initializes the diagram for this problem
    ''' </summary>
    ''' <param name="Diag">chart</param>
    Public Overrides Sub InitializeChart(ByRef Diag As BlueM.Opt.Diagramm.Hauptdiagramm)

        Dim Ausgangswert As Double
        Dim i As Integer
        Dim X() As Double
        Dim colorline1 As Steema.TeeChart.Tools.ColorLine
        Dim achsen As Collection
        Dim achse As BlueM.Opt.Diagramm.Diagramm.Achse

        'Ausgangswert berechnen
        ReDim X(Me.mAnzOptPara)
        For i = 1 To Me.mAnzOptPara
            X(i) = 10
        Next i
        Ausgangswert = 0
        For i = 1 To Me.mAnzOptPara
            Ausgangswert += ((X(1) - X(i) ^ 2) ^ 2 + (X(i) - 1) ^ 2)
        Next i

        'Achsen
        '------
        achsen = New Collection()

        'X-Achse
        achse.Title = "Calculation step"
        achse.Automatic = True
        achse.Minimum = 0
        Call achsen.Add(achse)

        'Y-Achse
        achse.Title = "Function value"
        achse.Automatic = False
        achse.Minimum = 0
        achse.Maximum = Ausgangswert * 1.3
        Call achsen.Add(achse)

        Call Diag.DiagInitialise("Schwefel 2.4 problem", achsen, Me.mProblem)

        'Linie für den Ausgangswert anzeigen
        colorline1 = New Steema.TeeChart.Tools.ColorLine(Diag.Chart) With {
            .AllowDrag = False,
            .Axis = Diag.Axes.Left,
            .Value = Ausgangswert
        }
        colorline1.Pen.Color = Drawing.Color.Red

    End Sub

    ''' <summary>
    ''' Evaluates the objective function values for the given solution
    ''' </summary>
    ''' <param name="ind">solution to evaluate</param>
    Public Overrides Sub Evaluate(ByRef ind As Individuum)

        Dim x_arr() As Double
        ReDim x_arr(Me.mAnzOptPara - 1)
        For i = 0 To Me.mAnzOptPara - 1
            x_arr(i) = -10 + ind.OptParameter(i).Xn * 20
        Next i
        ind.Objectives(0) = 0
        For i = 0 To Me.mAnzOptPara - 1
            ind.Objectives(0) += ((x_arr(0) - x_arr(i) ^ 2) ^ 2 + (x_arr(i) - 1) ^ 2)
        Next i

    End Sub

    ''' <summary>
    ''' Paint a solution in the diagram
    ''' </summary>
    ''' <param name="ind">solution</param>
    ''' <param name="ipop">population number (0-based)</param>
    ''' <param name="Diag">chart</param>
    Public Overrides Sub PaintSolution(ind As Individuum, ipop As Short, ByRef Diag As BlueM.Opt.Diagramm.Hauptdiagramm)

        Dim serie = Diag.getSeriesPoint("Population " & ipop + 1)
        serie.Add(ind.ID, ind.Objectives(0))

    End Sub

End Class
