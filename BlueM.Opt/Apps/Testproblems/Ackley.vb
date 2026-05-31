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
''' A non-convex function used as a performance test problem for optimization algorithms proposed by Ackley 1987
''' https://en.wikipedia.org/wiki/Ackley_function
''' </summary>
Public Class Ackley
    Inherits Testproblem

    ''' <summary>
    ''' Problem description
    ''' </summary>
    Public Overrides ReadOnly Property Description As String
        Get
            Return "A non-convex function used as a performance test problem for optimization algorithms proposed by Ackley 1987"
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
            Me.mOptPara(i) = New OptParameter()
        Next
        Randomize()
        With Me.mOptPara(0)
            .Min = -5
            .Max = 5
            .Bezeichnung = "X"
            'set random starting value
            .Xn = Rnd()
        End With
        With Me.mOptPara(1)
            .Min = -5
            .Max = 5
            .Bezeichnung = "Y"
            'set random starting value
            .Xn = Rnd()
        End With

        Call MyBase.FillProblemDefinition()

    End Sub

    ''' <summary>
    ''' Initializes the diagram for this problem
    ''' </summary>
    ''' <param name="Diag">chart</param>
    Public Overrides Sub InitializeChart(ByRef Diag As BlueM.Opt.Diagramm.Hauptdiagramm)

        Dim n As Integer
        Dim x, y As Double
        Dim ArrayX() As Double
        Dim ArrayY() As Double
        Dim ArrayZ() As Double
        Dim achsen As Collection
        Dim achse As BlueM.Opt.Diagramm.Diagramm.Achse

        Dim paramMin As Double = -5.0
        Dim paramMax As Double = 5.0

        'Achsen
        '------
        achsen = New Collection()

        'Bottom axis
        achse.Title = "X"
        achse.Automatic = False
        achse.Minimum = paramMin
        achse.Maximum = paramMax
        achse.Increment = 1
        Call achsen.Add(achse)

        'Left axis
        achse.Title = "Z"
        achse.Automatic = False
        achse.Minimum = 0
        achse.Maximum = 15
        achse.Increment = 1
        Call achsen.Add(achse)

        'Depth bottom axis
        achse.Title = "Y"
        achse.Automatic = False
        achse.Minimum = paramMin
        achse.Maximum = paramMax
        achse.Increment = 1
        Call achsen.Add(achse)

        'Diagramm initialisieren
        Call Diag.DiagInitialise("Ackley", achsen, Me.mProblem)

        'Serien
        '------
        Dim surface As Steema.TeeChart.Styles.Surface

        'Ackley function surface
        Dim resolution As Double = 0.2
        Dim length As Integer = (paramMax - paramMin) / resolution
        ReDim ArrayX(length ^ 2 - 1)
        ReDim ArrayY(length ^ 2 - 1)
        ReDim ArrayZ(length ^ 2 - 1)

        n = 0
        For x = paramMin To paramMax Step resolution
            For y = paramMin To paramMax Step resolution
                ArrayX(n) = x
                ArrayY(n) = y
                'Formula taken from https://en.wikipedia.org/wiki/Ackley_function
                ArrayZ(n) = -20.0 * Math.Exp(-0.2 * Math.Sqrt(0.5 * (x ^ 2 + y ^ 2))) _
                    - Math.Exp(0.5 * (Math.Cos(2 * Math.PI * x) + Math.Cos(2 * Math.PI * y))) _
                    + Math.E + 20
                n += 1
            Next
        Next

        surface = New Steema.TeeChart.Styles.Surface(Diag.Chart) With {
            .Title = "Ackley function",
            .IrregularGrid = True,
            .NumXValues = length,
            .NumZValues = length,
            .UseColorRange = False,
            .UsePalette = True,
            .PaletteStyle = Steema.TeeChart.Styles.PaletteStyles.Rainbow
        }
        surface.Brush.Solid = True
        surface.Brush.Transparency = 70
        surface.Pen.Visible = False
        surface.Add(ArrayX, ArrayZ, ArrayY)

    End Sub

    ''' <summary>
    ''' Evaluates the objective function values for the given solution
    ''' </summary>
    ''' <param name="ind">solution to evaluate</param>
    Public Overrides Sub Evaluate(ByRef ind As Individuum)
        Dim x As Double = ind.OptParameter(0).RWert
        Dim y As Double = ind.OptParameter(1).RWert
        'Formula taken from https://en.wikipedia.org/wiki/Ackley_function
        ind.Objectives(0) = -20.0 * Math.Exp(-0.2 * Math.Sqrt(0.5 * (x ^ 2 + y ^ 2))) _
            - Math.Exp(0.5 * (Math.Cos(2 * Math.PI * x) + Math.Cos(2 * Math.PI * y))) _
            + Math.E + 20
    End Sub

    ''' <summary>
    ''' Paint a solution in the diagram
    ''' </summary>
    ''' <param name="ind">solution</param>
    ''' <param name="ipop">population number (0-based)</param>
    ''' <param name="Diag">chart</param>
    Public Overrides Sub PaintSolution(ind As Individuum, ipop As Short, ByRef Diag As BlueM.Opt.Diagramm.Hauptdiagramm)

        Dim x As Double = ind.OptParameter(0).RWert
        Dim y As Double = ind.OptParameter(1).RWert
        Dim serie3D As Steema.TeeChart.Styles.Points3D
        serie3D = Diag.getSeries3DPoint("Population", "Orange")
        serie3D.Add(x, ind.Objectives(0), y)

    End Sub

End Class
