' Copyright (c) BlueM Dev Group
' Website: http://bluemodel.org
' 
' All rights reserved.
' 
' Released under the BSD-2-Clause License:
' 
' Redistribution and use in source and binary forms, with or without modification, 
' are permitted provided that the following conditions are met:
' 
' * Redistributions of source code must retain the above copyright notice, this list 
'   of conditions and the following disclaimer.
' * Redistributions in binary form must reproduce the above copyright notice, this list 
'   of conditions and the following disclaimer in the documentation and/or other materials 
'   provided with the distribution.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
' EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
' OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
' SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT 
' OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
' HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
' TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
' EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'--------------------------------------------------------------------------------------------
'
Imports IHWB.EVO.Common.Constants

Public Class SensiPlotController
    Implements EVO.IController

    ''' <summary>
    ''' Multithreading Support
    ''' </summary>
    Public ReadOnly Property MultithreadingSupported() As Boolean Implements IController.MultithreadingSupported
        Get
            Return False
        End Get
    End Property

    Private myProblem As EVO.Common.Problem
    Private mySettings As EVO.Common.Settings
    Private myProgress As EVO.Common.Progress
    Private myMonitor As EVO.Diagramm.Monitor
    Private myHauptDiagramm As EVO.Diagramm.Hauptdiagramm

    Private myAppType As EVO.Common.ApplicationTypes
    Private WithEvents Sim1 As EVO.Apps.Sim

    Private stopped As Boolean

    Public Sub Init(ByRef inputProblem As Common.Problem, ByRef inputSettings As Common.Settings, ByRef inputProgress As Common.Progress, ByRef inputHptDiagramm As Diagramm.Hauptdiagramm) Implements IController.Init
        Me.myProblem = inputProblem
        Me.mySettings = inputSettings
        Me.myProgress = inputProgress
        Me.myHauptDiagramm = inputHptDiagramm

        Me.myMonitor = EVO.Diagramm.Monitor.getInstance()
    End Sub

    Public Sub InitApp(ByRef inputSim As Apps.Sim) Implements IController.InitApp
        Me.myAppType = ApplicationTypes.Sim
        Me.Sim1 = inputSim
    End Sub

    Public Sub InitApp(ByRef inputTestprobleme As Apps.Testprobleme) Implements IController.InitApp
        Throw New Exception("SensiPlot kann keine Testprobleme berechnen!")
    End Sub

    Public Sub Start() Implements IController.Start

        'Hinweis:
        '------------------------------------------------------------------------
        'Die Modellparameter werden auch für die nicht ausgewählten OptParameter 
        'geschrieben, und zwar mit den in der OPT-Datei angegebenen Startwerten
        '------------------------------------------------------------------------

        Dim i, j, n, Anz_SensiPara, Anz_Sim As Integer
        Dim isOK As Boolean
        Dim ind As Common.Individuum
        Dim serie As Steema.TeeChart.Styles.Points
        Dim serie3D As New Steema.TeeChart.Styles.Points3D
        Dim surface As New Steema.TeeChart.Styles.Surface
        Dim SimReihe As Wave.Zeitreihe
        Dim SimReihen As Collection
        Dim Wave1 As Wave.Wave

        'Simulationen in Originalverzeichnis ausführen (keine Threads)
        Sim1.WorkDir_Current = Sim1.WorkDir_Original

        'Instanzieren
        SimReihen = New Collection()

        'Parameter
        Anz_SensiPara = Me.mySettings.SensiPlot.Selected_OptParameters.Count

        'Anzahl Simulationen
        If (Anz_SensiPara = 1) Then
            '1 Parameter
            Anz_Sim = Me.mySettings.SensiPlot.Num_Steps
        Else
            '2 Parameter
            Anz_Sim = Me.mySettings.SensiPlot.Num_Steps ^ 2
        End If

        'Progress initialisieren
        Call Me.myProgress.Initialize(0, 0, 0, Anz_Sim)

        'Bei 2 OptParametern 3D-Diagramm vorbereiten
        If (Anz_SensiPara > 1) Then
            'Oberfläche
            surface = New Steema.TeeChart.Styles.Surface(Me.myHauptDiagramm.Chart)
            surface.IrregularGrid = True
            surface.NumXValues = Me.mySettings.SensiPlot.Num_Steps
            surface.NumZValues = Me.mySettings.SensiPlot.Num_Steps
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

        'Simulationsschleife
        '-------------------
        Randomize()

        n = 1

        'Äussere Schleife (2. OptParameter)
        '----------------------------------
        For i = 0 To ((Me.mySettings.SensiPlot.Num_Steps - 1) * (Anz_SensiPara - 1))

            '2. OptParameterwert variieren
            If (Anz_SensiPara > 1) Then
                Select Case Me.mySettings.SensiPlot.Selected_SensiType
                    Case Common.Settings_Sensiplot.SensiType.normaldistribution
                        Me.myProblem.List_OptParameter(Me.mySettings.SensiPlot.Selected_OptParameters(1)).Xn = Rnd()
                    Case Common.Settings_Sensiplot.SensiType.discrete
                        Me.myProblem.List_OptParameter(Me.mySettings.SensiPlot.Selected_OptParameters(1)).Xn = i / (Me.mySettings.SensiPlot.Num_Steps - 1)
                End Select
            End If

            'Innere Schleife (1. OptParameter)
            '---------------------------------
            For j = 0 To Me.mySettings.SensiPlot.Num_Steps - 1

                'Stop?
                If (Me.stopped) Then Exit Sub

                '1. OptParameterwert variieren
                Select Case Me.mySettings.SensiPlot.Selected_SensiType
                    Case Common.Settings_Sensiplot.SensiType.normaldistribution
                        Me.myProblem.List_OptParameter(Me.mySettings.SensiPlot.Selected_OptParameters(0)).Xn = Rnd()
                    Case Common.Settings_Sensiplot.SensiType.discrete
                        Me.myProblem.List_OptParameter(Me.mySettings.SensiPlot.Selected_OptParameters(0)).Xn = j / (Me.mySettings.SensiPlot.Num_Steps - 1)
                End Select

                n += 1

                'Verlaufsanzeige aktualisieren
                Call Me.myProgress.NextNachf()

                'Einhaltung von OptParameter-Beziehung überprüfen
                isOK = True
                If (Anz_SensiPara > 1) Then
                    'Es muss nur der zweite Parameter auf eine Beziehung geprüft werden
                    If (Me.myProblem.List_OptParameter(Me.mySettings.SensiPlot.Selected_OptParameters(1)).Beziehung <> Beziehung.keine) Then
                        'Beziehung bezieht sich immer auf den in der Liste vorherigen Parameter
                        If (Me.mySettings.SensiPlot.Selected_OptParameters(0) = Me.mySettings.SensiPlot.Selected_OptParameters(1) - 1) Then

                            isOK = False

                            Dim ref As Double = Me.myProblem.List_OptParameter(Me.mySettings.SensiPlot.Selected_OptParameters(0)).RWert
                            Dim wert As Double = Me.myProblem.List_OptParameter(Me.mySettings.SensiPlot.Selected_OptParameters(1)).RWert

                            Select Case Me.myProblem.List_OptParameter(Me.mySettings.SensiPlot.Selected_OptParameters(1)).Beziehung
                                Case Beziehung.kleiner
                                    If (wert < ref) Then isOK = True
                                Case Beziehung.kleinergleich
                                    If (wert <= ref) Then isOK = True
                                Case Beziehung.groesser
                                    If (wert > ref) Then isOK = True
                                Case Beziehung.groessergleich
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

                    'Individuum in Sim evaluieren
                    isOK = Sim1.Evaluate(ind)

                    'BUG 253: Verletzte Constraints bei SensiPlot kenntlich machen?

                    'Erfolgreich evaluiertes Individuum in Diagramm eintragen
                    If (isOK) Then
                        If (Anz_SensiPara = 1) Then
                            '1 Parameter
                            serie = Me.myHauptDiagramm.getSeriesPoint("SensiPlot", "Orange")
                            serie.Add(ind.Objectives(Me.mySettings.SensiPlot.Selected_Objective), ind.OptParameter_RWerte(Me.mySettings.SensiPlot.Selected_OptParameters(0)), n.ToString())
                        Else
                            '2 Parameter
                            surface.Add(ind.OptParameter_RWerte(Me.mySettings.SensiPlot.Selected_OptParameters(0)), ind.Objectives(Me.mySettings.SensiPlot.Selected_Objective), ind.OptParameter_RWerte(Me.mySettings.SensiPlot.Selected_OptParameters(1)), n.ToString())
                            serie3D.Add(ind.OptParameter_RWerte(Me.mySettings.SensiPlot.Selected_OptParameters(0)), ind.Objectives(Me.mySettings.SensiPlot.Selected_Objective), ind.OptParameter_RWerte(Me.mySettings.SensiPlot.Selected_OptParameters(1)), n.ToString())
                        End If

                        'Simulationsergebnis in Wave laden
                        If (Me.mySettings.SensiPlot.Show_Wave) Then
                            'SimReihe auslesen
                            SimReihe = Sim1.SimErgebnis.Reihen(Me.myProblem.List_ObjectiveFunctions(Me.mySettings.SensiPlot.Selected_Objective).SimGr)
                            'Lösungs-ID an Titel anhängen
                            SimReihe.Title += " (Lösung " & n.ToString() & ")"
                            'SimReihe zu Collection hinzufügen
                            SimReihen.Add(SimReihe)
                        End If
                    End If

                End If

                System.Windows.Forms.Application.DoEvents()

            Next
        Next

        'Wave Diagramm anzeigen:
        'TODO: ggf. Referenzreihe der Objectivefunction anzeigen
        '-----------------------
        If (Me.mySettings.SensiPlot.Show_Wave) Then
            Wave1 = New Wave.Wave()
            For Each zre As Wave.Zeitreihe In SimReihen
                Wave1.Import_Series(zre)
            Next
            Call Wave1.Show()
        End If
    End Sub

    Public Sub Stoppen() Implements IController.Stoppen
        Me.stopped = True
    End Sub

End Class
