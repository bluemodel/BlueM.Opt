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

        Dim i, n, NumParams, NumSteps, NumSims As Integer
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

        'Anzahl Simulationen
        'TODO: update this!
        If (NumParams = 1) Then
            '1 Parameter
            NumSims = NumSteps
        Else
            '2 Parameter
            NumSims = NumSteps ^ 2
        End If

        'Progress initialisieren
        Call Me.myProgress.Initialize(0, 0, 0, NumSims)

        'Bei 2 OptParametern 3D-Diagramm vorbereiten
        If (NumParams > 1) Then
            'Oberfläche
            'TODO: make irregular triangle for LatinHypercube
            surface = New Steema.TeeChart.Styles.Surface(Me.myHauptDiagramm.Chart)
            surface.IrregularGrid = True
            surface.NumXValues = NumSteps
            surface.NumZValues = NumSteps
            '3D-Punkte
            serie3D = Me.myHauptDiagramm.getSeries3DPoint("Sensiplot", "Orange")
            'Diagramm drehen (rechter Mausbutton)
            Dim rotate1 As New Steema.TeeChart.Tools.Rotate
            rotate1.Button = Windows.Forms.MouseButtons.Right
            Me.myHauptDiagramm.Tools.Add(rotate1)
            'MarksTips
            Me.myHauptDiagramm.add_MarksTips(serie3D, Steema.TeeChart.Styles.MarksStyles.Label)
            surface.Title = "SensiPlot"
            surface.Cursor = System.Windows.Forms.Cursors.Hand
        End If

        'sample optparameters
        Dim parameterCombinations As New List(Of Double())
        Dim sampler As New ParameterSampler()
        parameterCombinations = sampler.Sample(NumParams, NumSteps, Me.mySettings.SensiPlot.Selected_Mode)

        'Simulationsschleife
        '-------------------
        For i = 0 To parameterCombinations.Count - 1

            'Stop?
            If (Me.stopped) Then Exit Sub

            n = i + 1
            Common.Log.AddMessage(Common.Log.levels.info, $"Sensiplot simulation {n}:")

            Dim parameterCombination As Double() = parameterCombinations(i)

            'OptParameterwerte setzen
            For p As Integer = 0 To NumParams - 1
                With Me.myProblem.List_OptParameter(Me.mySettings.SensiPlot.Selected_OptParameters(p))
                    .Xn = parameterCombination(p)
                    Common.Log.AddMessage(Common.Log.levels.info, $"* OptParameter { .Bezeichnung}: {Convert.ToString(.RWert, Common.Provider.FortranProvider)}")
                End With
            Next

            'Verlaufsanzeige aktualisieren
            Call Me.myProgress.NextNachf()

            'Einhaltung von OptParameter-Beziehung überprüfen
            isOK = True
            If (NumParams > 1) Then
                'Es muss nur der zweite Parameter auf eine Beziehung geprüft werden
                If (Me.myProblem.List_OptParameter(Me.mySettings.SensiPlot.Selected_OptParameters(1)).Beziehung <> Relationship.none) Then
                    'Beziehung bezieht sich immer auf den in der Liste vorherigen Parameter
                    If (Me.mySettings.SensiPlot.Selected_OptParameters(0) = Me.mySettings.SensiPlot.Selected_OptParameters(1) - 1) Then

                        isOK = False

                        Dim ref As Double = Me.myProblem.List_OptParameter(Me.mySettings.SensiPlot.Selected_OptParameters(0)).RWert
                        Dim wert As Double = Me.myProblem.List_OptParameter(Me.mySettings.SensiPlot.Selected_OptParameters(1)).RWert

                        Select Case Me.myProblem.List_OptParameter(Me.mySettings.SensiPlot.Selected_OptParameters(1)).Beziehung
                            Case Relationship.smaller_than
                                If (wert < ref) Then isOK = True
                            Case Relationship.smaller_equal
                                If (wert <= ref) Then isOK = True
                            Case Relationship.larger_than
                                If (wert > ref) Then isOK = True
                            Case Relationship.larger_equal
                                If (wert >= ref) Then isOK = True
                        End Select

                    End If
                End If
            End If

            'Evaluierung nur bei isOK
            If (isOK) Then

                'Individuum instanzieren
                ind = New Common.Individuum_PES("SensiPlot", n)

                'OptParameter ins Individuum kopieren
                ind.OptParameter = Me.myProblem.List_OptParameter

                'WorkDir einrichten
                If Me.mySettings.SensiPlot.Save_Results Then
                    'Unterverzeichnis einrichten
                    WorkDir = IO.Path.Combine(Sim1.WorkDir_Original, $"sensiplot_{n:0000}")
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
                        '1 Parameter
                        serie = Me.myHauptDiagramm.getSeriesPoint("SensiPlot", "Orange")
                        serie.Add(ind.Objectives(Me.mySettings.SensiPlot.Selected_Objective), ind.OptParameter_RWerte(Me.mySettings.SensiPlot.Selected_OptParameters(0)), n.ToString())
                    Else
                        '> 1 Parameter, plot first two opt parameters
                        surface.Add(ind.OptParameter_RWerte(Me.mySettings.SensiPlot.Selected_OptParameters(0)), ind.Objectives(Me.mySettings.SensiPlot.Selected_Objective), ind.OptParameter_RWerte(Me.mySettings.SensiPlot.Selected_OptParameters(1)), n.ToString())
                        serie3D.Add(ind.OptParameter_RWerte(Me.mySettings.SensiPlot.Selected_OptParameters(0)), ind.Objectives(Me.mySettings.SensiPlot.Selected_Objective), ind.OptParameter_RWerte(Me.mySettings.SensiPlot.Selected_OptParameters(1)), n.ToString())
                    End If

                    'Simulationsergebnis in Wave laden
                    If (Me.mySettings.SensiPlot.Show_Wave) Then
                        'SimReihe auslesen
                        SimReihe = Sim1.SimErgebnis.Reihen(Me.myProblem.List_ObjectiveFunctions(Me.mySettings.SensiPlot.Selected_Objective).SimGr)
                        'Lösungs-ID an Titel anhängen
                        SimReihe.Title &= $" (Solution {n})"
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
