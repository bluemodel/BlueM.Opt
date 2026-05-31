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
Imports BlueM.Opt.Common

''' <summary>
''' Find the minimum of the Beale problem (x=(3, 0.5), F(x)=0)
''' </summary>
Public Class Beale
    Inherits Testproblem

    ''' <summary>
    ''' Problem description
    ''' </summary>
    Public Overrides ReadOnly Property Description As String
        Get
            Return "Find the minimum of the Beale problem (x=(3, 0.5), F(x)=0)"
        End Get
    End Property

    ''' <summary>
    ''' Defines the optimization problem
    ''' </summary>
    ''' <param name="prob">the problem</param>
    Public Overrides Sub DefineProblem(ByRef prob As Problem)

        'store the problem
        Me.mProblem = prob

        Me.mAnzOptPara = 2
        Me.mAnzZiele = 1
        Me.mAnzConstraints = 0
        ReDim Me.mOptPara(Me.mAnzOptPara - 1)
        For i = 0 To Me.mAnzOptPara - 1
            Me.mOptPara(i) = New OptParameter With {
                .Xn = 0.5
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
        Dim colorline1 As Steema.TeeChart.Tools.ColorLine
        Dim achsen As Collection
        Dim achse As BlueM.Opt.Diagramm.Diagramm.Achse

        'Ausgangswert berechnen
        Ausgangswert = (1.5 - 0.5 * (1 - 0.5)) ^ 2 + (2.25 - 0.5 * (1 - 0.5) ^ 2) ^ 2 + (2.625 - 0.5 * (1 - 0.5) ^ 3) ^ 2

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

        Call Diag.DiagInitialise("Beale problem", achsen, Me.mProblem)

        'Linie für den Ausgangswert anzeigen
        colorline1 = New Steema.TeeChart.Tools.ColorLine(Diag.Chart) With {
            .AllowDrag = False,
            .Axis = Diag.Axes.Left,
            .Value = Ausgangswert
        }
        colorline1.Pen.Color = Drawing.Color.Green

    End Sub

    ''' <summary>
    ''' Evaluates the objective function values for the given solution
    ''' </summary>
    ''' <param name="ind">solution to evaluate</param>
    Public Overrides Sub Evaluate(ByRef ind As Individuum)

        Dim x1 As Double = -5 + (ind.OptParameter(0).Xn * 10)
        Dim x2 As Double = -2 + (ind.OptParameter(1).Xn * 4)

        ind.Objectives(0) = (1.5 - x1 * (1 - x2)) ^ 2 + (2.25 - x1 * (1 - x2) ^ 2) ^ 2 + (2.625 - x1 * (1 - x2) ^ 3) ^ 2

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
