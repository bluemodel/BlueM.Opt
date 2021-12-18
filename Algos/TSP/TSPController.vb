﻿' Copyright (c) BlueM Dev Group
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
Public Class TSPController
    Implements BlueM.Opt.Algos.IController

    ''' <summary>
    ''' Multithreading Support
    ''' </summary>
    Public ReadOnly Property MultithreadingSupported() As Boolean Implements IController.MultithreadingSupported
        Get
            Return False
        End Get
    End Property

    Private myHauptDiagramm As BlueM.Opt.Diagramm.Hauptdiagramm
    Private myMonitor As BlueM.Opt.Diagramm.Monitor
    Private myProblem As BlueM.Opt.Common.Problem
    Private mySettings As BlueM.Opt.Common.Settings
    Private myProgress As BlueM.Opt.Common.Progress

    Private TSP1 As TSP
    Private Stopp As Boolean

    ''' <summary>
    ''' TSP Controller initialisieren
    ''' </summary>
    ''' <param name="inputProblem"></param>
    ''' <param name="inputSettings"></param>
    ''' <param name="inputProgress"></param>
    ''' <param name="inputHptDiagramm"></param>
    ''' <remarks></remarks>
    Public Sub Init(ByRef inputProblem As Common.Problem, ByRef inputSettings As Common.Settings, ByRef inputProgress As Common.Progress, ByRef inputHptDiagramm As Diagramm.Hauptdiagramm) Implements IController.Init
        Me.myProblem = inputProblem
        Me.mySettings = inputSettings
        Me.myProgress = inputProgress
        Me.myHauptDiagramm = inputHptDiagramm
        Me.myMonitor = BlueM.Opt.Diagramm.Monitor.getInstance()

        Stopp = False
    End Sub

    Public Sub InitApp(ByRef inputSim As Apps.Sim) Implements IController.InitApp
        'not applicable
    End Sub

    Public Sub InitApp(ByRef inputTestprobleme As Apps.Testprobleme) Implements IController.InitApp
        'not applicable
    End Sub

    ''' <summary>
    ''' TSP Starten
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Start() Implements IController.Start

        Me.TSP1 = New TSP()

        Call TSP1.TSP_Initialize(Me.mySettings.TSP)
        Call InitDiagramm()
        Call Zeichnen_TSP_cities(TSP1.ListOfCities)

        'Batch_Mode
        Dim Batch_Mode As Boolean = True
        'Anzahl der Tests
        Dim n As Integer = 3

        'Progress
        Me.myProgress.Initialize(0, 0, Me.mySettings.TSP.N_Gen, Me.mySettings.TSP.N_Children)

        'Monitor Stuff
        With Me.myMonitor
            .SelectTabLog()
            .Show()
            .LogAppend("Cities: " & Me.mySettings.TSP.N_Cities)
            .LogAppend("Combinations: " & TSP1.n_Comb(Me.mySettings.TSP.N_Cities))
            .LogAppend("Parents: " & Me.mySettings.TSP.N_Parents)
            .LogAppend("Children: " & Me.mySettings.TSP.N_Children)
            .LogAppend("Generations: " & Me.mySettings.TSP.N_Gen)
            .LogAppend("Evaluations: " & Me.mySettings.TSP.N_Children * Me.mySettings.TSP.N_Gen)
            If Me.mySettings.TSP.Problem = Common.EnProblem.circle Then
                .LogAppend("Quality Aim: " & Conversion.Int(TSP1.circumference))
            End If
        End With

        Select Case TSP1.Mode

            Case TSP.EnMode.Standard_Opt
                'Progress
                Me.myProgress.Initialize(0, 0, Me.mySettings.TSP.N_Gen, Me.mySettings.TSP.N_Children)
                Call TSP_Controller(False)

            Case TSP.EnMode.Batch_OPpt
                'Progress
                Me.myProgress.Initialize(n, 8, Me.mySettings.TSP.N_Gen, Me.mySettings.TSP.N_Children)
                Dim i, M, R As Integer

                For R = 1 To 2
                    Me.mySettings.TSP.ReprodOperator = R

                    For M = 1 To 4
                        Me.mySettings.TSP.MutOperator = M
                        Me.myMonitor.LogAppend($"ReprodOperator: {Me.mySettings.TSP.ReprodOperator}; MutationOperator: {Me.mySettings.TSP.MutOperator}")

                        'n Wiederholungen
                        For i = 1 To TSP1.nTests
                            Call TSP_Controller(True)
                        Next
                        '~~~~~~~~~~~~~~~~
                    Next
                Next
            Case TSP.EnMode.Just_Calc
                Me.mySettings.TSP.N_Children = 1
                Dim i, j As Double

                j = 10000000

                Dim Time As New Stopwatch
                Call TSP1.Dim_Children()
                Call TSP1.Generate_Random_Path_TSP()
                Call TSP1.Cities_according_ChildPath()

                Time.Start()
                For i = 1 To j
                    TSP1.Evaluate_child_Quality()
                Next
                Time.Stop()

                Me.myMonitor.LogAppend($"Number of calculations: {j}")
                Me.myMonitor.LogAppend($"Time elapsed: {Time.Elapsed.Hours}h {Time.Elapsed.Minutes}m {Time.Elapsed.Seconds}s {Time.Elapsed.Milliseconds}ms")

        End Select
    End Sub

    Public Sub Stoppen() Implements IController.Stoppen

        Stopp = True

    End Sub

    Private Sub TSP_Controller(ByVal Batch_Mode As Boolean)

        'Laufvariable für die Generationen
        Dim gen As Integer
        Dim i As Integer
        Dim GoToExit As Boolean
        'Zwischenspeicher das Penalty zum drucken
        Dim PenaltyTMP As Integer = 2100000000

        'Intervall zum Updaten des Diagramms
        Dim increm As Integer = 100
        Dim jepp As Integer = 0

        'BUG 212: Nach Klasse Diagramm auslagern!
        Me.myHauptDiagramm.Export.Image.PNG.Width = 477
        Me.myHauptDiagramm.Export.Image.PNG.Height = 627

        'Arrays werden Dimensioniert
        Call TSP1.Dim_Parents_TSP()
        Call TSP1.Dim_Children()

        'Zufällige Kinderpfade werden generiert
        Call TSP1.Generate_Random_Path_TSP()

        'Generationsschleife
        For gen = 1 To Me.mySettings.TSP.N_Gen

            'Den Kindern werden die Städte Ihres Pfades entsprechend zugewiesen
            Call TSP1.Cities_according_ChildPath()

            'Zeichnen des ersten Child
            If gen = 1 Then
                Call Zeichnen_TSP(TSP1.ChildrenList(0).Image)
                Me.myHauptDiagramm.Update()
                'png Export
                If TSP1.pngExport = True Then
                    Me.myHauptDiagramm.Export.Image.PNG.Save(TSP1.ExPath & gen.ToString.PadLeft(7, "0") & " Qualität " & Conversion.Int(TSP1.ChildrenList(0).Penalty).ToString.PadLeft(5, "0") & ".png")
                End If
            End If

            'Bestimmung des der Qualität der Kinder
            Call TSP1.Evaluate_child_Quality()

            'Sortieren der Kinden anhand der Qualität
            Call TSP1.Sort_Faksimile(TSP1.ChildrenList)

            'Selections Prozess (Übergabe der Kinder an die Eltern je nach Strategie)
            Call TSP1.Selection_Process()

            'Zeichnen des besten Elter
            If gen >= jepp Then
                Call Me.Zeichnen_TSP(TSP1.ParentList(0).Image)
                jepp += increm
                Me.myProgress.iGen() = gen
                If Batch_Mode = False Then
                    Me.myMonitor.LogAppend($"Gen.: {gen}; Length: {Conversion.Int(TSP1.ParentList(0).Penalty)}; Factor: {Math.Round(TSP1.ParentList(0).Penalty / TSP1.circumference, 3, MidpointRounding.ToEven)}")
                    'png Export
                    If TSP1.pngExport = True And Conversion.Int(TSP1.ParentList(0).Penalty) < PenaltyTMP Then
                        Me.myHauptDiagramm.Export.Image.PNG.Save(TSP1.ExPath & gen.ToString.PadLeft(7, "0") & " Qualität " & Conversion.Int(TSP1.ParentList(0).Penalty).ToString.PadLeft(5, "0") & ".png")
                        PenaltyTMP = Conversion.Int(TSP1.ParentList(0).Penalty)
                    End If
                End If
            End If

            'Fall die Problemstellung ein Kreis ist wird abgebrochen, wenn das Optimum erreicht ist
            If Me.mySettings.TSP.Problem = Common.EnProblem.circle And TSP1.ParentList(0).Penalty < TSP1.circumference Then
                GoToExit = True
                Select Case TSP1.ParentList(0).Path(0) < TSP1.ParentList(0).Path(1)
                    Case True
                        For i = 0 To Me.mySettings.TSP.N_Cities - 2
                            If Not TSP1.ParentList(0).Path(i) + 1 = TSP1.ParentList(0).Path(i + 1) And Not (TSP1.ParentList(0).Path(i) = Me.mySettings.TSP.N_Cities And TSP1.ParentList(0).Path(i + 1) = 1) Then
                                GoToExit = False
                                Exit For
                            End If
                        Next
                    Case Else
                        For i = 0 To Me.mySettings.TSP.N_Cities - 2
                            If Not TSP1.ParentList(0).Path(i) - 1 = TSP1.ParentList(0).Path(i + 1) And Not (TSP1.ParentList(0).Path(i) = 1 And TSP1.ParentList(0).Path(i + 1) = Me.mySettings.TSP.N_Cities) Then
                                GoToExit = False
                                Exit For
                            End If
                        Next
                End Select

            End If

            If GoToExit = True Or Stopp Then
                If Batch_Mode = False Then
                    Call Zeichnen_TSP(TSP1.ParentList(0).Image)
                    Me.myHauptDiagramm.Update()
                    Me.myMonitor.LogAppend($"Gen.: {gen}; Length: {Conversion.Int(TSP1.ParentList(0).Penalty)}; Factor: {Math.Round(TSP1.ParentList(0).Penalty / TSP1.circumference, 3, MidpointRounding.ToEven)}")
                    'png Export
                    If TSP1.pngExport = True Then
                        Me.myHauptDiagramm.Export.Image.PNG.Save(TSP1.ExPath & gen.ToString.PadLeft(7, "0") & " Qualität " & Conversion.Int(TSP1.ParentList(0).Penalty).ToString.PadLeft(5, "0") & ".png")
                    End If
                End If
                Exit For
            End If

            'Kinder werden Hier vollständig gelöscht
            Call TSP1.Reset_Children()

            'Reproduktionsoperatoren, hier gehts dezent zur Sache
            Call TSP1.Reproduction_Control()

            'Mutationsoperatoren
            Call TSP1.Mutation_Control()

        Next gen

        Me.myMonitor.LogAppend("Final Quality: " & Conversion.Int(TSP1.ParentList(0).Penalty))

    End Sub

#Region "Diagrammfunktionen"

    ''' <summary>
    ''' Diagramm initialisieren
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitDiagramm()

        Dim i As Integer

        With Me.myHauptDiagramm
            .Clear()
            .Header.Text = "Traveling Salesman Problem"
            .Aspect.View3D = False
            .Legend.Visible = False

            'Alles etwas schlichter
            'Rahmenschattierung um das TeeChart Form
            .Panel.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
            .Panel.Bevel.Inner = Steema.TeeChart.Drawing.BevelStyles.None
            'Farbverlauf am Rand das Chart
            .Panel.Gradient.Visible = False
            .Walls.Visible = False

            'Printversion
            .Header.Visible = False
            .Panel.Color = Drawing.Color.White
            .Chart.Axes.Left.Ticks.Color = Drawing.Color.Black
            .Chart.Axes.Right.Ticks.Color = Drawing.Color.Black
            .Chart.Axes.Left.Ticks.Width = 1
            .Chart.Axes.Right.Ticks.Width = 1

            'Formatierung der Axen
            '.Chart.Axes.Bottom.Title.Caption = BlueM1.OptZieleListe(0).Bezeichnung 'HACK: Beschriftung der Axen
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Bottom.Maximum = 100
            '.Chart.Axes.Left.Title.Caption = BlueM1.OptParameterListe(0).Bezeichnung 'HACK: Beschriftung der Axen
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Minimum = 0
            .Chart.Axes.Left.Maximum = 130

            'Series(0): Series für die Sädte.
            Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
            Point1.Title = "Städte"
            Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point1.Color = System.Drawing.Color.Orange
            Point1.Pointer.HorizSize = 2
            Point1.Pointer.VertSize = 2

            'Series(n): für die Reisen
            For i = 1 To Me.mySettings.TSP.N_Cities
                Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
                Line1.Title = "Reisen"
                Line1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                Line1.Color = System.Drawing.Color.Blue
                Line1.Pointer.HorizSize = 3
                Line1.Pointer.VertSize = 3
            Next

        End With

        'Zeichnen der Punkte für die Städte
        For i = 0 To Me.mySettings.TSP.N_Cities - 1
            Me.myHauptDiagramm.Series(0).Add(TSP1.ListOfCities(i, 1), TSP1.ListOfCities(i, 2), "")
        Next

    End Sub

    ''' <summary>
    ''' Zeichnen der Verbindungen
    ''' </summary>
    ''' <param name="TmpListOfCities"></param>
    ''' <remarks></remarks>
    Private Sub Zeichnen_TSP(ByVal TmpListOfCities(,) As Object)

        Dim i As Integer

        'Zeichnen der einzelnen Verbindungen
        'Es werden einzelne Serien verwendet, da die Werte gerne mal der X-Achse entsprechend sortiert werden
        With Me.myHauptDiagramm
            .Series(1).Clear()
            For i = 1 To Me.mySettings.TSP.N_Cities - 1
                .Series(i + 1).Clear()
                .Series(i + 1).Add(TmpListOfCities(i, 1), TmpListOfCities(i, 2), Drawing.Color.Blue)
                .Series(i).Add(TmpListOfCities(i, 1), TmpListOfCities(i, 2), Drawing.Color.Blue)
            Next

            'Zeichnen der Verbindung von der ersten bis zur letzten Stadt
            .Series(1).Add(TmpListOfCities(0, 1), TmpListOfCities(0, 2), "")
            .Series(Me.mySettings.TSP.N_Cities).Add(TmpListOfCities(0, 1), TmpListOfCities(0, 2), "")

            .Update()
        End With

    End Sub

    ''' <summary>
    ''' Nicht genutzt?
    ''' </summary>
    ''' <param name="TmpListOfCities"></param>
    ''' <remarks></remarks>
    Private Sub Zeichnen_TSP_cities(ByVal TmpListOfCities(,) As Object)

        Dim i As Integer

        'Zeichnen der Punkte für die Städte
        For i = 0 To Me.mySettings.TSP.N_Cities - 1
            Me.myHauptDiagramm.Series(0).Add(TmpListOfCities(i, 1), TmpListOfCities(i, 2), "")
        Next

    End Sub

#End Region 'Diagrammfunktionen

End Class
