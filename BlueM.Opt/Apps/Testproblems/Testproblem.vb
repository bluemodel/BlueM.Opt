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

Public MustInherit Class Testproblem

    Protected mTestProblemType As TestproblemFactory.Testproblems

    Protected mProblem As Problem

    Protected mAnzOptPara As Integer
    Protected mAnzZiele As Integer
    Protected mAnzConstraints As Integer
    Protected mOptPara() As OptParameter

    ''' <summary>
    ''' Problem description
    ''' </summary>
    Public MustOverride ReadOnly Property Description As String

    ''' <summary>
    ''' Defines the optimization problem
    ''' </summary>
    ''' <param name="prob">problem</param>
    Public MustOverride Sub DefineProblem(ByRef prob As Problem)

    ''' <summary>
    ''' Fills the problem properties with the values defined in this class
    ''' </summary>
    Protected Sub FillProblemDefinition()

        'fill required problem properties
        ReDim Me.mProblem.List_ObjectiveFunctions(Me.mAnzZiele - 1)
        For i = 0 To Me.mProblem.NumObjectives - 1
            Me.mProblem.List_ObjectiveFunctions(i) = New ObjectiveFunction_Series With {
                .isPrimObjective = True,
                .Direction = EVO_DIRECTION.Minimization
            }
        Next
        ReDim Me.mProblem.List_Constraintfunctions(Me.mAnzConstraints - 1)
        For i = 0 To Me.mProblem.NumConstraints - 1
            Me.mProblem.List_Constraintfunctions(i) = New Constraintfunction()
        Next
        ReDim Me.mProblem.List_OptParameter(Me.mAnzOptPara - 1)
        For i = 0 To Me.mAnzOptPara - 1
            Me.mProblem.List_OptParameter(i) = Me.mOptPara(i)
        Next

    End Sub

    ''' <summary>
    ''' Initialize the chart for this problem
    ''' </summary>
    ''' <param name="Diag">chart</param>
    Public Overridable Sub InitializeChart(ByRef Diag As BlueM.Opt.Diagramm.Hauptdiagramm)

        Dim title As String
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

        Call achsen.Add(xachse)
        Call achsen.Add(yachse)

        'Diagramm initialisieren
        Call Diag.DiagInitialise(title, achsen, Me.mProblem)

    End Sub

    ''' <summary>
    ''' Evaluates the objective function values for the given solution
    ''' </summary>
    ''' <param name="ind">solution to evaluate</param>
    Public MustOverride Sub Evaluate(ByRef ind As Individuum)

    ''' <summary>
    ''' Paint a solution in the diagram
    ''' </summary>
    ''' <param name="ind">solution</param>
    ''' <param name="ipop">population number (0-based)</param>
    ''' <param name="Diag">chart</param>
    Public MustOverride Sub PaintSolution(ind As Individuum, ipop As Short, ByRef Diag As BlueM.Opt.Diagramm.Hauptdiagramm)

End Class
