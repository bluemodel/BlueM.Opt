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
''' Zitzler/Deb/Thiele T4: multicriteria test problem (convex, local/global pareto fronts)
''' </summary>
Public Class ZitzlerDebT4
    Inherits Testproblem

    ''' <summary>
    ''' Problem description
    ''' </summary>
    Public Overrides ReadOnly Property Description As String
        Get
            Return "Zitzler/Deb/Thiele T4: multicriteria test problem (convex, local/global pareto fronts)"
        End Get
    End Property

    ''' <summary>
    ''' Defines the optimization problem
    ''' </summary>
    ''' <param name="prob">the problem</param>
    Public Overrides Sub DefineProblem(ByRef prob As Problem)

        'store the problem
        Me.mProblem = prob

        Me.mAnzOptPara = 10
        Me.mAnzZiele = 2
        Me.mAnzConstraints = 0
        ReDim Me.mOptPara(Me.mAnzOptPara - 1)
        Randomize()
        For i = 0 To Me.mAnzOptPara - 1
            Me.mOptPara(i) = New OptParameter With {
                .Xn = Rnd()
            }
        Next

        Call MyBase.FillProblemDefinition()

    End Sub

    ''' <summary>
    ''' Initializes the diagram for this problem
    ''' </summary>
    ''' <param name="Diag">chart</param>
    Public Overrides Sub InitializeChart(ByRef Diag As BlueM.Opt.Diagramm.Hauptdiagramm)

        Dim i, j As Integer
        Dim title As String
        Dim serie As Steema.TeeChart.Styles.Series
        Dim achsen As Collection
        Dim xachse, yachse As BlueM.Opt.Diagramm.Diagramm.Achse

        'Achsen
        '------
        achsen = New Collection()

        'X-Achse
        xachse.Title = "X"
        xachse.Automatic = False
        xachse.Minimum = 0
        xachse.Maximum = 1
        xachse.Increment = 0.1

        'Y-Achse
        yachse.Title = "Y"
        yachse.Automatic = False
        yachse.Minimum = 0
        yachse.Maximum = 1
        yachse.Increment = 0.1

        title = "Zitzler/Deb/Thiele T4"
        xachse.Automatic = True
        yachse.Automatic = True

        Call achsen.Add(xachse)
        Call achsen.Add(yachse)

        'Diagramm initialisieren
        Call Diag.DiagInitialise(title, achsen, Me.mProblem)

        Dim ArrayX(1000) As Double
        Dim ArrayY(1000) As Double

        'Lokale Optima berechnen und zeichnen
        For i = 1 To 10
            For j = 0 To 1000
                ArrayX(j) = j / 1000
                ArrayY(j) = (1 + (i - 1) / 4) * (1 - Math.Sqrt(ArrayX(j) / (1 + (i - 1) / 4)))
            Next
            serie = Diag.getSeriesLine("Local optimum " & i)
            serie.Add(ArrayX, ArrayY)
        Next

    End Sub

    ''' <summary>
    ''' Evaluates the objective function values for the given solution
    ''' </summary>
    ''' <param name="ind">solution to evaluate</param>
    Public Overrides Sub Evaluate(ByRef ind As Individuum)

        Dim f1 As Double = ind.OptParameter(0).Xn
        Dim f2 As Double = 0
        For i = 1 To Me.mAnzOptPara - 1
            Dim x2 As Double = -5 + (ind.OptParameter(i).Xn * 10)
            f2 = f2 + (x2 * x2 - 10 * Math.Cos(4 * Math.PI * x2))
        Next i
        f2 = 1 + 10 * (Me.mAnzOptPara - 1) + f2
        f2 = f2 * (1 - Math.Sqrt(f1 / f2))
        ind.Objectives(0) = f1
        ind.Objectives(1) = f2

    End Sub

    ''' <summary>
    ''' Paint a solution in the diagram
    ''' </summary>
    ''' <param name="ind">solution</param>
    ''' <param name="ipop">population number (0-based)</param>
    ''' <param name="Diag">chart</param>
    Public Overrides Sub PaintSolution(ind As Individuum, ipop As Short, ByRef Diag As BlueM.Opt.Diagramm.Hauptdiagramm)

        Dim serie = Diag.getSeriesPoint("Population", "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
        serie.Add(ind.Objectives(0), ind.Objectives(1))

    End Sub

End Class
