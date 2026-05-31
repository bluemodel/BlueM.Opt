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
''' Multicriteria test problem (circle) with two contraints
''' </summary>
Public Class Box
    Inherits Testproblem

    ''' <summary>
    ''' Problem description
    ''' </summary>
    Public Overrides ReadOnly Property Description As String
        Get
            Return "Multicriteria test problem (circle) with two contraints"
        End Get
    End Property

    ''' <summary>
    ''' Defines the optimization problem
    ''' </summary>
    ''' <param name="prob">the problem</param>
    Public Overrides Sub DefineProblem(ByRef prob As Problem)

        'store the problem
        Me.mProblem = prob

        Me.mAnzOptPara = 3
        Me.mAnzZiele = 3
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

        Dim i, j, n As Integer
        Dim ArrayX() As Double
        Dim ArrayY() As Double
        Dim ArrayZ() As Double
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
        achse.Title = "Z"
        achse.Automatic = False
        achse.Minimum = 0
        achse.Maximum = 1
        achse.Increment = 0.2
        Call achsen.Add(achse)

        'Diagramm initialisieren
        Call Diag.DiagInitialise("Box", achsen, Me.mProblem)

        'Serien
        '------
        Dim surface As Steema.TeeChart.Styles.Surface
        Dim series3D As Steema.TeeChart.Styles.Points3D

        'Constraint 1
        'x + y + z <= 0.8
        Dim surfaceRes As Integer = 11
        ReDim ArrayX(surfaceRes ^ 2 - 1)
        ReDim ArrayY(surfaceRes ^ 2 - 1)
        ReDim ArrayZ(surfaceRes ^ 2 - 1)

        n = 0
        For i = 0 To surfaceRes - 1
            For j = 0 To (surfaceRes - 1)
                ArrayX(n) = i * (1.1 / surfaceRes)
                ArrayZ(n) = j * (1.1 / surfaceRes)
                ArrayY(n) = Math.Max(0.8 - ArrayX(n) - ArrayZ(n), 0)
                n += 1
            Next
        Next

        surface = New Steema.TeeChart.Styles.Surface(Diag.Chart) With {
            .Title = "Constraint 1",
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

        'Constraint 2
        'x + y <= 0.5
        ReDim ArrayX(65)
        ReDim ArrayY(65)
        ReDim ArrayZ(65)

        n = 0
        For i = 0 To 10
            For j = 0 To 5
                ArrayX(n) = j * 0.1
                ArrayZ(n) = i * 0.1
                ArrayY(n) = 0.5 - ArrayX(n)
                n += 1
            Next
        Next

        surface = New Steema.TeeChart.Styles.Surface(Diag.Chart) With {
            .Title = "Constraint 2",
            .IrregularGrid = True,
            .NumXValues = 10,
            .NumZValues = 10,
            .UseColorRange = False,
            .UsePalette = False
        }
        surface.Brush.Solid = True
        surface.Brush.Color = Color.Blue
        surface.Brush.Transparency = 70
        surface.Pen.Color = Color.Blue
        surface.SideBrush.Visible = True
        surface.SideBrush.Color = Color.Red
        surface.SideBrush.Transparency = 70
        surface.Add(ArrayX, ArrayY, ArrayZ)

        'Schnittgerade zwischen den Constraints
        series3D = New Steema.TeeChart.Styles.Points3D(Diag.Chart) With {
            .Title = "Intersection"
        }
        series3D.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Nothing
        series3D.LinePen.Visible = True
        series3D.LinePen.Width = 1
        series3D.LinePen.Color = Color.Red
        series3D.Add(0.5, 0, 0.3)
        series3D.Add(0, 0.5, 0.3)

    End Sub

    ''' <summary>
    ''' Evaluates the objective function values for the given solution
    ''' </summary>
    ''' <param name="ind">solution to evaluate</param>
    Public Overrides Sub Evaluate(ByRef ind As Individuum)

        'Qualitätswerte berechnen
        '------------------------
        ind.Objectives(0) = ind.OptParameter(0).Xn
        ind.Objectives(1) = ind.OptParameter(1).Xn
        ind.Objectives(2) = ind.OptParameter(2).Xn

        'Constraints berechnen
        '---------------------
        ind.Constraints(0) = ind.OptParameter(0).Xn + ind.OptParameter(1).Xn - 0.5
        ind.Constraints(1) = ind.OptParameter(0).Xn + ind.OptParameter(1).Xn + ind.OptParameter(2).Xn - 0.8

    End Sub

    ''' <summary>
    ''' Paint a solution in the diagram
    ''' </summary>
    ''' <param name="ind">solution</param>
    ''' <param name="ipop">population number (0-based)</param>
    ''' <param name="Diag">chart</param>
    Public Overrides Sub PaintSolution(ind As Individuum, ipop As Short, ByRef Diag As BlueM.Opt.Diagramm.Hauptdiagramm)

        Dim serie3D As Steema.TeeChart.Styles.Points3D
        If (Not ind.Is_Feasible) Then
            'Ungültige Lösung
            serie3D = Diag.getSeries3DPoint("Population (invalid)", "Gray")
        Else
            'Gültige Lösung
            serie3D = Diag.getSeries3DPoint("Population", "Orange")
        End If
        serie3D.Add(ind.Objectives(0), ind.Objectives(1), ind.Objectives(2))

    End Sub

End Class
