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
''' Fit parameters to sine function
''' </summary>
Public Class Sinus
    Inherits Testproblem

    ''' <summary>
    ''' Problem description
    ''' </summary>
    Public Overrides ReadOnly Property Description As String
        Get
            Return "Fit parameters to sine function"
        End Get
    End Property

    Private ReadOnly Property Unterteilung_X As Double
        Get
            Return 2 * Math.PI / (Me.mAnzOptPara - 1)
        End Get
    End Property

    ''' <summary>
    ''' Defines the optimization problem
    ''' </summary>
    ''' <param name="prob">the problem</param>
    Public Overrides Sub DefineProblem(ByRef prob As Problem)

        'store the problem
        Me.mProblem = prob

        Me.mAnzOptPara = 50
        Me.mAnzZiele = 1
        Me.mAnzConstraints = 0
        ReDim Me.mOptPara(Me.mAnzOptPara - 1)
        For i = 0 To Me.mAnzOptPara - 1
            Me.mOptPara(i) = New OptParameter With {
                .Xn = 0
            }
        Next

        Call MyBase.FillProblemDefinition()

    End Sub

    ''' <summary>
    ''' Initializes the diagram for this problem
    ''' </summary>
    ''' <param name="Diag">chart</param>
    Public Overrides Sub InitializeChart(ByRef Diag As BlueM.Opt.Diagramm.Hauptdiagramm)

        Dim array_x() As Double
        Dim array_y() As Double
        Dim i As Integer
        Dim Unterteilung_X As Double
        Dim serie As Steema.TeeChart.Styles.Series
        Dim achsen As Collection
        Dim achse As BlueM.Opt.Diagramm.Diagramm.Achse

        'Achsen vorbereiten
        '------------------
        achsen = New Collection()

        'X-Achse
        achse.Title = "X value"
        achse.Automatic = False
        achse.Minimum = 0
        achse.Maximum = 2 * Math.PI
        achse.Increment = Math.PI
        achsen.Add(achse)

        'Y-Achse
        achse.Title = "Y value"
        achse.Automatic = False
        achse.Minimum = -1
        achse.Maximum = 1
        achse.Increment = 0.2
        achsen.Add(achse)

        'Diagramm initialisieren
        '-----------------------
        Call Diag.DiagInitialise("Sine function", achsen, Me.mProblem)

        'Sinuslinie zeichnen
        '-------------------
        Unterteilung_X = 2 * Math.PI / (Me.mAnzOptPara - 1)

        ReDim array_x(Me.mAnzOptPara - 1)
        ReDim array_y(Me.mAnzOptPara - 1)

        For i = 0 To Me.mAnzOptPara - 1
            array_x(i) = Math.Round(i * Unterteilung_X, 2)
            array_y(i) = Math.Sin(i * Unterteilung_X)
        Next i

        serie = Diag.getSeriesLine("Sine function", "Green")
        Call serie.Add(array_x, array_y)

    End Sub

    ''' <summary>
    ''' Evaluates the objective function values for the given solution
    ''' </summary>
    ''' <param name="ind">solution to evaluate</param>
    Public Overrides Sub Evaluate(ByRef ind As Individuum)

        'Fehlerquadrate zur Sinusfunktion |0-2pi|
        '----------------------------------------
        ind.Objectives(0) = 0
        For i = 0 To Me.mAnzOptPara - 1
            ind.Objectives(0) += (Math.Sin(i * Unterteilung_X) - (-1 + (ind.OptParameter(i).Xn * 2))) ^ 2
        Next i

    End Sub

    ''' <summary>
    ''' Paint a solution in the diagram
    ''' </summary>
    ''' <param name="ind">solution</param>
    ''' <param name="ipop">population number (0-based)</param>
    ''' <param name="Diag">chart</param>
    Public Overrides Sub PaintSolution(ind As Individuum, ipop As Short, ByRef Diag As BlueM.Opt.Diagramm.Hauptdiagramm)

        Dim array_x() As Double = {}
        Dim array_y() As Double = {}

        ReDim array_x(Me.mAnzOptPara - 1)
        ReDim array_y(Me.mAnzOptPara - 1)
        For i = 0 To Me.mAnzOptPara - 1
            array_x(i) = Math.Round(i * Unterteilung_X, 2)
            array_y(i) = (-1 + ind.OptParameter(i).Xn * 2)
        Next i

        Dim serie = Diag.getSeriesPoint("Population " & ipop + 1)
        serie.Add(array_x, array_y)

    End Sub

End Class
