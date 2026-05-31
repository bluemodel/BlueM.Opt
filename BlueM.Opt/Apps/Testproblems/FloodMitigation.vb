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
''' Multicriteria Problem Flood Mitigation and Hydropower Generation
''' </summary>
Public Class FloodMitigation
    Inherits Testproblem

    ''' <summary>
    ''' Problem description
    ''' </summary>
    Public Overrides ReadOnly Property Description As String
        Get
            Return "Multicriteria Problem Flood Mitigation and Hydropower Generation"
        End Get
    End Property

    ''' <summary>
    ''' Defines the optimization problem
    ''' </summary>
    ''' <param name="prob">the problem</param>
    Public Overrides Sub DefineProblem(ByRef prob As Problem)

        'store the problem
        Me.mProblem = prob

        Me.mAnzOptPara = 8                'Parameters
        Me.mAnzZiele = 2                    'Objective
        Me.mAnzConstraints = 4               'Constraints
        ReDim Me.mOptPara(Me.mAnzOptPara - 1)
        Randomize()
        For i = 0 To Me.mAnzOptPara - 1
            Me.mOptPara(i) = New OptParameter With {
                .Xn = Rnd()
            }
        Next
        For i = 0 To 3
            Me.mOptPara(i).Min = 470424
            Me.mOptPara(i).Max = 48407547
        Next
        For i = 4 To Me.mAnzOptPara - 1
            Me.mOptPara(i).Min = 648000
            Me.mOptPara(i).Max = 2592000
        Next

        Call MyBase.FillProblemDefinition()

    End Sub

    ''' <summary>
    ''' Initializes the diagram for this problem
    ''' </summary>
    ''' <param name="Diag">chart</param>
    Public Overrides Sub InitializeChart(ByRef Diag As BlueM.Opt.Diagramm.Hauptdiagramm)

        Dim title As String
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

        title = "Flood mitigation"

        yachse.Maximum = -15000000
        yachse.Minimum = -55000000

        xachse.Maximum = 10000000
        xachse.Minimum = -15000000

        Call achsen.Add(xachse)
        Call achsen.Add(yachse)

        'Diagramm initialisieren
        Call Diag.DiagInitialise(title, achsen, Me.mProblem)

    End Sub

    ''' <summary>
    ''' Evaluates the objective function values for the given solution
    ''' </summary>
    ''' <param name="ind">solution to evaluate</param>
    Public Overrides Sub Evaluate(ByRef ind As Individuum)

        'Getting the new Parameters
        Dim x_arr() As Double
        ReDim x_arr(7)
        For i = 0 To x_arr.GetUpperBound(0)
            x_arr(i) = ind.OptParameter(i).RWert
        Next

        'Calculating the Objective Function
        '----------------------------------
        Dim Storage As Double
        Storage = 650000
        'float sconst=650000;

        Dim p() As Double = {9449568.0, 9069713.044, 2441388.773, 1556876.392}
        'double p[4] = { 9449568.000,9069713.044,2441388.773,1556876.392};
        Dim f1 As Double = 0
        Dim f2 As Double = 0

        'Objective Function 1 and 2
        f1 = -((p(0) - x_arr(4)) - (x_arr(0) - x_arr(1)))
        f2 = -(0.09651 * (((8.0E-22 * Math.Pow(x_arr(0), 3)) - (0.00000000000008 * Math.Pow(x_arr(0), 2)) + (0.000003 * x_arr(0)) + 6.2034) * x_arr(4)))

        f1 = f1 - ((p(1) - x_arr(5)) - (x_arr(1) - x_arr(2)))
        f2 = f2 - (0.09651 * (((8.0E-22 * Math.Pow(x_arr(1), 3)) - (0.00000000000008 * Math.Pow(x_arr(1), 2)) + (0.000003 * x_arr(1)) + 6.2034) * x_arr(5)))

        f1 = f1 - ((p(2) - x_arr(6)) - (x_arr(2) - x_arr(3)))
        f2 = f2 - (0.09651 * (((8.0E-22 * Math.Pow(x_arr(2), 3)) - (0.00000000000008 * Math.Pow(x_arr(2), 2)) + (0.000003 * x_arr(2)) + 6.2034) * x_arr(6)))

        f1 = f1 - ((p(3) - x_arr(7)) - (x_arr(3) - x_arr(4)))
        f2 = f2 - (0.09651 * (((8.0E-22 * Math.Pow(x_arr(3), 3)) - (0.00000000000008 * Math.Pow(x_arr(3), 2)) + (0.000003 * x_arr(3)) + 6.2034) * x_arr(7)))

        'Constraints
        '-----------
        Dim contrain(3) As Double
        contrain(0) = (x_arr(0) - Storage - p(0) + x_arr(4))
        contrain(1) = (x_arr(1) - x_arr(0) - p(1) + x_arr(5))
        contrain(2) = (x_arr(2) - x_arr(1) - p(2) + x_arr(6))
        contrain(3) = (x_arr(3) - x_arr(2) - p(3) + x_arr(7))

        'Give Back the Penalties and Constraints
        ind.Objectives(0) = f1
        ind.Objectives(1) = f2
        ind.Constraints(0) = contrain(0)
        ind.Constraints(1) = contrain(1)
        ind.Constraints(2) = contrain(2)
        ind.Constraints(3) = contrain(3)

    End Sub

    ''' <summary>
    ''' Paint a solution in the diagram
    ''' </summary>
    ''' <param name="ind">solution</param>
    ''' <param name="ipop">population number (0-based)</param>
    ''' <param name="Diag">chart</param>
    Public Overrides Sub PaintSolution(ind As Individuum, ipop As Short, ByRef Diag As BlueM.Opt.Diagramm.Hauptdiagramm)

        Dim serie As Steema.TeeChart.Styles.Points
        If ind.Is_Feasible Then
            serie = Diag.getSeriesPoint("Population", "Orange")
        Else
            serie = Diag.getSeriesPoint("Population (invalid)", "Gray")
        End If
        serie.Add(ind.Objectives(0), ind.Objectives(1))

    End Sub

End Class
