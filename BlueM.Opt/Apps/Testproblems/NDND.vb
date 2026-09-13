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
''' NDND
''' https://github.com/chen0040/cs-moea/blob/master/cs-moea/Benchmarks/NDNDProblem.cs
''' </summary>
Public Class NDND
    Inherits Testproblem

    ''' <summary>
    ''' Problem description
    ''' </summary>
    Public Overrides ReadOnly Property Description As String
        Get
            Return "NDND"
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
        Me.mAnzConstraints = 0
        ReDim Me.mOptPara(Me.mAnzOptPara - 1)
        For i = 0 To Me.mAnzOptPara - 1
            Me.mOptPara(i) = New OptParameter()
        Next
        Randomize()
        With Me.mOptPara(0)
            .Min = 0
            .Max = 1
            .Bezeichnung = "X"
            'set random starting value
            .Xn = Rnd()
        End With
        With Me.mOptPara(1)
            .Min = 0
            .Max = 1
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

        title = "NDND"
        xachse.Automatic = True
        yachse.Automatic = True

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

        Dim x As Double = ind.OptParameter(0).RWert
        Dim y As Double = ind.OptParameter(1).RWert

        Dim f1 As Double = 1 - Math.Exp((-4) * x) * Math.Pow(Math.Sin(5 * Math.PI * x), 4)

        Dim g, h As Double
        If (y > 0 And y < 0.4) Then
            g = 4 - 3 * Math.Exp(-2500 * (y - 0.2) * (y - 0.2))
        Else
            g = 4 - 3 * Math.Exp(-25 * (y - 0.7) * (y - 0.7))
        End If
        Dim a As Double = 4
        If (f1 < g) Then
            h = 1 - Math.Pow(f1 / g, a)
        Else
            h = 0
        End If
        Dim f2 As Double = g * h

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
