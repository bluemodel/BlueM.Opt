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
''' Parameter dependency: Y > X
''' </summary>
Public Class DependentParameters
    Inherits Testproblem

    ''' <summary>
    ''' Problem description
    ''' </summary>
    Public Overrides ReadOnly Property Description As String
        Get
            Return "Parameter dependency: Y > X"
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
                                .Xn = 1
                            }
        Next
        'Beziehungen
        Me.mOptPara(0).Beziehung = Constants.Relationship.none
        Me.mOptPara(1).Beziehung = Constants.Relationship.larger_than

        Call MyBase.FillProblemDefinition()

    End Sub

    ''' <summary>
    ''' Initializes the diagram for this problem
    ''' </summary>
    ''' <param name="Diag">chart</param>
    Public Overrides Sub InitializeChart(ByRef Diag As BlueM.Opt.Diagramm.Hauptdiagramm)

        Dim i, j, n As Integer
        Dim ArrayX() As Double
        Dim ArrayY() As Double
        Dim ArrayZ() As Double
        Const surfaceRes As Integer = 11
        Dim surface As Steema.TeeChart.Styles.Surface
        Dim achsen As Collection
        Dim achse As BlueM.Opt.Diagramm.Diagramm.Achse

        'Achsen
        '------
        achsen = New Collection()

        'X-Achse
        achse.Title = "X"
        achse.Automatic = False
        achse.Minimum = 0
        achse.Maximum = 1
        achse.Increment = 0.2
        Call achsen.Add(achse)

        'Y-Achse
        achse.Title = "Y"
        achse.Automatic = False
        achse.Minimum = 0
        achse.Maximum = 1
        achse.Increment = 0.2
        Call achsen.Add(achse)

        'Z-Achse
        achse.Title = "Objective function"
        achse.Automatic = False
        achse.Minimum = 0
        achse.Maximum = 2
        achse.Increment = 0.5
        Call achsen.Add(achse)

        'Diagramm initialisieren
        Call Diag.DiagInitialise("Parameter dependency: Y > X", achsen, Me.mProblem)

        'Serien
        '------

        'Ebene x = y
        ReDim ArrayX(surfaceRes ^ 2 - 1)
        ReDim ArrayY(surfaceRes ^ 2 - 1)
        ReDim ArrayZ(surfaceRes ^ 2 - 1)

        n = 0
        For i = 0 To surfaceRes - 1
            For j = 0 To (surfaceRes - 1)
                ArrayX(n) = i * (1.1 / surfaceRes)
                ArrayZ(n) = j * (2.1 / surfaceRes)
                ArrayY(n) = ArrayX(n)
                n += 1
            Next
        Next

        surface = New Steema.TeeChart.Styles.Surface(Diag.Chart) With {
            .Title = "X = Y",
            .IrregularGrid = True,
            .NumXValues = surfaceRes,
            .NumZValues = surfaceRes,
            .UseColorRange = False,
            .UsePalette = False
        }
        surface.Brush.Solid = True
        surface.Brush.Color = Color.Green
        surface.Brush.Transparency = 70
        surface.Pen.Color = Color.Green
        surface.SideBrush.Visible = True
        surface.SideBrush.Color = Color.Red
        surface.SideBrush.Transparency = 70
        surface.Add(ArrayX, ArrayY, ArrayZ)


    End Sub

    ''' <summary>
    ''' Evaluates the objective function values for the given solution
    ''' </summary>
    ''' <param name="ind">solution to evaluate</param>
    Public Overrides Sub Evaluate(ByRef ind As Individuum)

        ind.Objectives(0) = ind.OptParameter(0).Xn ^ 2 + ind.OptParameter(1).Xn ^ 2

    End Sub

    ''' <summary>
    ''' Paint a solution in the diagram
    ''' </summary>
    ''' <param name="ind">solution</param>
    ''' <param name="ipop">population number (0-based)</param>
    ''' <param name="Diag">chart</param>
    Public Overrides Sub PaintSolution(ind As Individuum, ipop As Short, ByRef Diag As BlueM.Opt.Diagramm.Hauptdiagramm)

        Dim serie3D As Steema.TeeChart.Styles.Points3D
        serie3D = Diag.getSeries3DPoint("Population " & ipop + 1)
        serie3D.Add(ind.OptParameter(0).Xn, ind.OptParameter(1).Xn, ind.Objectives(0))

    End Sub

End Class
