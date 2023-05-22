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
Imports BlueM.Opt.Common.Constants

Public Class SensiPlotController
    Implements BlueM.Opt.Algos.IController

    ''' <summary>
    ''' Multithreading Support
    ''' </summary>
    Public ReadOnly Property MultithreadingSupported() As Boolean Implements IController.MultithreadingSupported
        Get
            Return False
        End Get
    End Property

    Private myProblem As BlueM.Opt.Common.Problem
    Private mySettings As BlueM.Opt.Common.Settings
    Private myProgress As BlueM.Opt.Common.Progress
    Private myHauptDiagramm As BlueM.Opt.Diagramm.Hauptdiagramm

    Private myAppType As BlueM.Opt.Common.ApplicationTypes
    Private WithEvents Sim1 As BlueM.Opt.Apps.Sim

    Private stopped As Boolean

    Public Sub Init(ByRef inputProblem As Common.Problem, ByRef inputSettings As Common.Settings, ByRef inputProgress As Common.Progress, ByRef inputHptDiagramm As Diagramm.Hauptdiagramm) Implements IController.Init
        Me.myProblem = inputProblem
        Me.mySettings = inputSettings
        Me.myProgress = inputProgress
        Me.myHauptDiagramm = inputHptDiagramm
    End Sub

    Public Sub InitApp(ByRef inputSim As Apps.Sim) Implements IController.InitApp
        Me.myAppType = ApplicationTypes.Sim
        Me.Sim1 = inputSim
    End Sub

    Public Sub InitApp(ByRef inputTestprobleme As Apps.Testprobleme) Implements IController.InitApp
        Throw New Exception("SensiPlot can not be used for test problems!")
    End Sub

    Public Sub Start() Implements IController.Start

        'Hinweis:
        '------------------------------------------------------------------------
        'Die Modellparameter werden auch für die nicht ausgewählten OptParameter 
        'geschrieben, und zwar mit den in der OPT-Datei angegebenen Startwerten
        '------------------------------------------------------------------------

        Dim i, n, NumParams, NumSteps As Integer
        Dim x, y, z As Double
        Dim isOK As Boolean
        Dim ind As Common.Individuum
        Dim serie As Steema.TeeChart.Styles.Points
        Dim serie3D As New Steema.TeeChart.Styles.Points3D
        Dim surface As New Steema.TeeChart.Styles.Surface
        Dim SimReihe As Wave.TimeSeries
        Dim SimReihen As Collection
        Dim Wave1 As Wave.Wave
        Dim WorkDir As String

        'Instanzieren
        SimReihen = New Collection()

        'Parameter
        NumParams = Me.mySettings.SensiPlot.Selected_OptParameters.Count

        'Steps
        NumSteps = Me.mySettings.SensiPlot.Num_Steps

        'Bei 2 OptParametern 3D-Diagramm vorbereiten
        If NumParams > 1 Then
            '3D-Punkte
            serie3D = Me.myHauptDiagramm.getSeries3DPoint("Sensiplot", "Orange")
            'Diagramm drehen (rechter Mausbutton)
            Dim rotate1 As New Steema.TeeChart.Tools.Rotate
            rotate1.Button = Windows.Forms.MouseButtons.Right
            Me.myHauptDiagramm.Tools.Add(rotate1)
            'MarksTips
            Me.myHauptDiagramm.add_MarksTips(serie3D, Steema.TeeChart.Styles.MarksStyles.Label)

            If NumParams = 2 Then
                'Add a 2D surface
                'TODO: make irregular triangle TriSurface for LatinHypercube, but always requires at least 4 points
                surface = New Steema.TeeChart.Styles.Surface(Me.myHauptDiagramm.Chart)
                surface.IrregularGrid = True
                surface.NumXValues = NumSteps
                surface.NumZValues = NumSteps
                surface.UseColorRange = False
                surface.UsePalette = True
                surface.PaletteStyle = Steema.TeeChart.Styles.PaletteStyles.Rainbow
                surface.Brush.Solid = True
                surface.Brush.Transparency = 70
                surface.Pen.Visible = False
                surface.Title = "SensiPlot"
                surface.Cursor = System.Windows.Forms.Cursors.Hand
            End If
        End If

        'sample optparameters
        Dim parameterCombinations As New List(Of Double())
        Dim sampler As New ParameterSampler()
        parameterCombinations = sampler.Sample(NumParams, NumSteps, Me.mySettings.SensiPlot.Selected_Mode)

        'Progress initialisieren
        Call Me.myProgress.Initialize(0, 0, 0, parameterCombinations.Count)

        'Simulationsschleife
        '-------------------
        For i = 0 To parameterCombinations.Count - 1

            'Stop?
            If (Me.stopped) Then Exit Sub

            n = i + 1
            Common.Log.AddMessage(Common.Log.levels.info, $"Sensiplot simulation {n}:")

            Dim parameterCombination As Double() = parameterCombinations(i)

            'OptParameterwerte setzen
            For j = 0 To NumParams - 1
                With Me.myProblem.List_OptParameter(Me.mySettings.SensiPlot.Selected_OptParameters(j))
                    .Xn = parameterCombination(j)
                    Common.Log.AddMessage(Common.Log.levels.info, $"* OptParameter { .Bezeichnung}: {Convert.ToString(.RWert, Common.Provider.FortranProvider)}")
                End With
            Next

            'Verlaufsanzeige aktualisieren
            Call Me.myProgress.NextNachf()

            'Check whether parameter relationships are satisfied
            Dim allRelationshipsSatisfied As Boolean = True
            For j = 1 To Me.myProblem.List_OptParameter.Length - 1 'start checking from the second parameter

                If Me.myProblem.List_OptParameter(j).Beziehung <> Relationship.none Then
                    Dim relationshipSatisfied As Boolean = False
                    Dim ref As Double = Me.myProblem.List_OptParameter(j - 1).RWert
                    Dim wert As Double = Me.myProblem.List_OptParameter(j).RWert

                    Select Case Me.myProblem.List_OptParameter(j).Beziehung
                        Case Relationship.smaller_than
                            If (wert < ref) Then relationshipSatisfied = True
                        Case Relationship.smaller_equal
                            If (wert <= ref) Then relationshipSatisfied = True
                        Case Relationship.larger_than
                            If (wert > ref) Then relationshipSatisfied = True
                        Case Relationship.larger_equal
                            If (wert >= ref) Then relationshipSatisfied = True
                    End Select

                    If Not relationshipSatisfied Then
                        allRelationshipsSatisfied = False
                        Common.Log.AddMessage(Common.Log.levels.warning, $"Relationship for optimization parameter {Me.myProblem.List_OptParameter(j).Bezeichnung} is not satisfied!")
                    End If
                End If
            Next

            If Not allRelationshipsSatisfied Then
                'Skip evaluation
                Common.Log.AddMessage(Common.Log.levels.warning, $"Skipping evaluation of parameter combination {n} because of parameter relationship violations!")
            Else
                'Evaluate parameter combination

                'Individuum instanzieren
                ind = New Common.Individuum_PES("SensiPlot", n)

                'OptParameter ins Individuum kopieren
                ind.OptParameter = Me.myProblem.List_OptParameter

                'WorkDir einrichten
                If Me.mySettings.SensiPlot.Save_Results Then
                    'Unterverzeichnis einrichten
                    WorkDir = IO.Path.Combine(Sim1.WorkDir_Original, $"sensiplot_{ind.ID:0000}")
                    If Not IO.Directory.Exists(WorkDir) Then
                        IO.Directory.CreateDirectory(WorkDir)
                    End If
                    Sim1.copyDateset(WorkDir)
                    Sim1.WorkDir_Current = WorkDir
                Else
                    'Simulation in Originalverzeichnis ausführen
                    Sim1.WorkDir_Current = Sim1.WorkDir_Original
                End If

                'Individuum in Sim evaluieren
                isOK = Sim1.Evaluate(ind)

                'TODO: Verletzte Constraints bei SensiPlot kenntlich machen? (#173)

                'Erfolgreich evaluiertes Individuum in Diagramm eintragen
                If (isOK) Then
                    If (NumParams = 1) Then
                        '1 Parameter, x is opt parameter, y is objective function
                        serie = Me.myHauptDiagramm.getSeriesPoint("SensiPlot", "Orange")
                        x = ind.OptParameter_RWerte(Me.mySettings.SensiPlot.Selected_OptParameters(0))
                        y = ind.Objectives(Me.mySettings.SensiPlot.Selected_Objective) * Me.myProblem.List_ObjectiveFunctions(Me.mySettings.SensiPlot.Selected_Objective).Richtung
                        serie.Add(x, y, ind.ID.ToString())
                    Else
                        '> 1 parameters, x and z are first two opt parameters, y is objective function
                        x = ind.OptParameter_RWerte(Me.mySettings.SensiPlot.Selected_OptParameters(0))
                        z = ind.OptParameter_RWerte(Me.mySettings.SensiPlot.Selected_OptParameters(1))
                        y = ind.Objectives(Me.mySettings.SensiPlot.Selected_Objective) * Me.myProblem.List_ObjectiveFunctions(Me.mySettings.SensiPlot.Selected_Objective).Richtung
                        serie3D.Add(x, y, z, ind.ID.ToString())
                        'if exactly 2 parameters, add a 2D surface
                        If NumParams = 2 Then
                            surface.Add(x, y, z, ind.ID.ToString())
                        End If
                    End If

                    'Simulationsergebnis in Wave laden
                    If (Me.mySettings.SensiPlot.Show_Wave) Then
                        'SimReihe auslesen
                        SimReihe = Sim1.SimErgebnis.Reihen(Me.myProblem.List_ObjectiveFunctions(Me.mySettings.SensiPlot.Selected_Objective).SimGr)
                        'Lösungs-ID an Titel anhängen
                        SimReihe.Title &= $" (Solution {ind.ID})"
                        'SimReihe zu Collection hinzufügen
                        SimReihen.Add(SimReihe)
                    End If
                End If

            End If

            System.Windows.Forms.Application.DoEvents()

        Next

        'Wave Diagramm anzeigen:
        'TODO: ggf. Referenzreihe der Objectivefunction anzeigen
        '-----------------------
        If (Me.mySettings.SensiPlot.Show_Wave) Then
            Wave1 = New Wave.Wave()
            For Each zre As Wave.TimeSeries In SimReihen
                Wave1.Import_Series(zre)
            Next
            Dim app As New BlueM.Wave.App(Wave1)
        End If
    End Sub

    Public Sub Stoppen() Implements IController.Stoppen
        Me.stopped = True
    End Sub

End Class
