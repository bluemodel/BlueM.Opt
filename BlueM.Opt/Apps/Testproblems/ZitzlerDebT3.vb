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
''' Zitzler/Deb/Thiele T3: multicriteria test problem (convex, non-continuous)
''' </summary>
Public Class ZitzlerDebT3
    Inherits Testproblem

    ''' <summary>
    ''' Problem description
    ''' </summary>
    Public Overrides ReadOnly Property Description As String
        Get
            Return "Zitzler/Deb/Thiele T3: multicriteria test problem (convex, non-continuous)"
        End Get
    End Property

    ''' <summary>
    ''' Defines the optimization problem
    ''' </summary>
    ''' <param name="prob">the problem</param>
    Public Overrides Sub DefineProblem(ByRef prob As Problem)

        'store the problem
        Me.mProblem = prob

        Me.mAnzOptPara = 15
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

        Dim j As Integer
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

        title = "Zitzler/Deb/Thiele T3"
        xachse.Increment = 0.2
        yachse.Maximum = 7
        yachse.Minimum = -1
        yachse.Increment = 0.5

        Call achsen.Add(xachse)
        Call achsen.Add(yachse)

        'Diagramm initialisieren
        Call Diag.DiagInitialise(title, achsen, Me.mProblem)

        'TODO: Titel der Serien (für Export)
        Dim ArrayX(100) As Double
        Dim ArrayY(100) As Double

        'Paretofront berechnen und zeichnen
        For j = 0 To 100
            ArrayX(j) = j / 100
            ArrayY(j) = 1 - Math.Sqrt(ArrayX(j)) - ArrayX(j) * Math.Sin(10 * Math.PI * ArrayX(j))
        Next j
        serie = Diag.getSeriesLine("Pareto front", "Green")
        serie.Add(ArrayX, ArrayY)

    End Sub

    ''' <summary>
    ''' Evaluates the objective function values for the given solution
    ''' </summary>
    ''' <param name="ind">solution to evaluate</param>
    Public Overrides Sub Evaluate(ByRef ind As Individuum)

        Dim f1 As Double = ind.OptParameter(0).Xn
        Dim f2 As Double = 0
        For i = 1 To Me.mAnzOptPara - 1
            f2 = f2 + ind.OptParameter(i).Xn
        Next i
        f2 = 1 + 9 / (Me.mAnzOptPara - 1) * f2
        f2 = f2 * (1 - Math.Sqrt(f1 / f2) - (f1 / f2) * Math.Sin(10 * Math.PI * f1))
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
