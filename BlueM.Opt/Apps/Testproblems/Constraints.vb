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
Imports System.Drawing
Imports BlueM.Opt.Common

''' <summary>
''' Multicriteria test problem (convex) with two constraints
''' </summary>
Public Class Constraints
    Inherits Testproblem

    ''' <summary>
    ''' Problem description
    ''' </summary>
    Public Overrides ReadOnly Property Description As String
        Get
            Return "Multicriteria test problem (convex) with two constraints"
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
        Me.mAnzZiele = 2
        Me.mAnzConstraints = 2
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

        title = "Test problem"

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

        title = "CONSTR"
        yachse.Maximum = 15

        Call achsen.Add(xachse)
        Call achsen.Add(yachse)

        'Diagramm initialisieren
        Call Diag.DiagInitialise(title, achsen, Me.mProblem)

        'draw constraints
        Dim Array1X(100) As Double
        Dim Array1Y(100) As Double
        Dim Array2X(100) As Double
        Dim Array2Y(100) As Double
        Dim Array3X(61) As Double
        Dim Array3Y(61) As Double
        Dim Array4X(61) As Double
        Dim Array4Y(61) As Double

        'Grenze 1 berechnen und zeichnen
        For j = 0 To 100
            Array1X(j) = 0.1 + j * 0.009
            Array1Y(j) = 1 / Array1X(j)
        Next j
        serie = Diag.getSeriesLine("Constraint 1", "Red")
        serie.Add(Array1X, Array1Y)

        'Grenze 2 berechnen und zeichnen
        For j = 0 To 100
            Array2X(j) = 0.1 + j * 0.009
            Array2Y(j) = (1 + 5) / Array2X(j)
        Next j
        serie = Diag.getSeriesLine("Constraint 2", "Red")
        serie.Add(Array2X, Array2Y)

        'Grenze 3 berechnen und zeichnen
        ReDim Array3X(61)
        ReDim Array3Y(61)
        For j = 0 To 61
            Array3X(j) = 0.1 + (j + 2) * 0.009
            Array3Y(j) = (7 - 9 * Array3X(j)) / Array3X(j)
        Next j
        serie = Diag.getSeriesLine("Constraint 3", "Blue")
        serie.Add(Array3X, Array3Y)

        'Grenze 4 berechnen und zeichnen
        ReDim Array4X(61)
        ReDim Array4Y(61)
        For j = 0 To 61
            Array4X(j) = 0.1 + (j + 2) * 0.009
            Array4Y(j) = (9 * Array4X(j)) / Array4X(j)
        Next j
        serie = Diag.getSeriesLine("Constraint 4", "Red")
        serie.Add(Array4X, Array4Y)

    End Sub

    ''' <summary>
    ''' Evaluates the objective function values for the given solution
    ''' </summary>
    ''' <param name="ind">solution to evaluate</param>
    Public Overrides Sub Evaluate(ByRef ind As Individuum)

        'Qualitätswerte berechnen
        '------------------------
        Dim f1 As Double = ind.OptParameter(0).Xn * (9 / 10) + 0.1
        Dim f2 As Double = (1 + 5 * ind.OptParameter(1).Xn) / (ind.OptParameter(0).Xn * (9 / 10) + 0.1)

        ind.Objectives(0) = f1
        ind.Objectives(1) = f2

        'Constraints berechnen
        '---------------------
        Dim g1 As Double = (5 * ind.OptParameter(1).Xn) + 9 * (ind.OptParameter(0).Xn * (9 / 10) + 0.1) - 6
        Dim g2 As Double = (-1) * (5 * ind.OptParameter(1).Xn) + 9 * (ind.OptParameter(0).Xn * (9 / 10) + 0.1) - 1

        ind.Constraints(0) = g1
        ind.Constraints(1) = g2

    End Sub

    ''' <summary>
    ''' Paint a solution in the diagram
    ''' </summary>
    ''' <param name="ind">solution</param>
    ''' <param name="ipop">population number (0-based)</param>
    ''' <param name="Diag">chart</param>
    Public Overrides Sub PaintSolution(ind As Individuum, ipop As Short, ByRef Diag As BlueM.Opt.Diagramm.Hauptdiagramm)

        Dim serie As Steema.TeeChart.Styles.Points
        If (Not ind.Is_Feasible) Then
            'Ungültige Lösung
            serie = Diag.getSeriesPoint("Population (invalid)", "Gray", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
        Else
            'Gültige Lösung
            serie = Diag.getSeriesPoint("Population", "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
        End If
        serie.Add(ind.Objectives(0), ind.Objectives(1))

    End Sub

End Class
