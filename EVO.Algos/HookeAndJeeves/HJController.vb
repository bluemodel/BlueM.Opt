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
Imports IHWB.EVO.Common.Constants

''' <summary>
''' Controller für Hooke And Jeeves
''' </summary>
Public Class HJController
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
    Private myProgress As EVO.Common.Progress 'TODO: Verlaufsanzeige für H&J
    Private myMonitor As EVO.Diagramm.Monitor
    Private myHauptDiagramm As EVO.Diagramm.Hauptdiagramm

    Private myAppType As EVO.Common.ApplicationTypes
    Private WithEvents Sim1 As EVO.Apps.Sim
    Private Testprobleme1 As EVO.Apps.Testprobleme

    Private stopped As Boolean

#Region "Methoden"

    ''' <summary>
    ''' Initialisiert den HJ-Controller und übergibt alle erforderlichen Objekte
    ''' </summary>
    ''' <param name="inputProblem"></param>
    ''' <param name="inputSettings"></param>
    ''' <param name="inputProgress"></param>
    ''' <param name="inputHptDiagramm"></param>
    Public Sub Init(ByRef inputProblem As Common.Problem, _
                    ByRef inputSettings As Common.Settings, _
                    ByRef inputProgress As Common.Progress, _
                    ByRef inputHptDiagramm As Diagramm.Hauptdiagramm) Implements IController.Init

        Me.myProblem = inputProblem
        Me.mySettings = inputSettings
        Me.myProgress = inputProgress
        Me.myHauptDiagramm = inputHptDiagramm

        Me.myMonitor = EVO.Diagramm.Monitor.getInstance()
    End Sub

    ''' <summary>
    ''' Initialisiert den Controller für Sim-Anwendungen
    ''' </summary>
    ''' <param name="inputSim">die Simulationsanwendung</param>
    Public Sub InitApp(ByRef inputSim As EVO.Apps.Sim) Implements IController.InitApp
        Me.myAppType = ApplicationTypes.Sim
        Me.Sim1 = inputSim
    End Sub

    ''' <summary>
    ''' Initialisiert den Controller für Testprobleme
    ''' </summary>
    Public Sub InitApp(ByRef inputTestprobleme As EVO.Apps.Testprobleme) Implements IController.InitApp
        Me.myAppType = ApplicationTypes.Testproblems
        Me.Testprobleme1 = inputTestprobleme
    End Sub

    ''' <summary>
    ''' Optimierung starten
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Start() Implements IController.Start

        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim b As Boolean
        Dim ind As Common.Individuum
        Dim QNBest() As Double
        Dim QBest() As Double
        Dim aktuellePara(Me.myProblem.NumOptParams - 1) As Double
        Dim SIM_Eval_is_OK As Boolean
        Dim durchlauf As Long
        Dim Iterationen As Long
        Dim Tastschritte_aktuell As Long
        Dim Tastschritte_gesamt As Long
        Dim Extrapolationsschritte As Long
        Dim Rueckschritte As Long
        Dim serie As Steema.TeeChart.Styles.Series

        Dim HookJeeves As New EVO.HookeAndJeeves.HookeAndJeeves(Me.myProblem.NumOptParams, Me.mySettings.HookeJeeves.DnStart, Me.mySettings.HookeJeeves.DnFinish)

        Me.stopped = False

        'Monitor anzeigen
        Call Me.myMonitor.SelectTabLog()
        Call Me.myMonitor.Show()

        'Los gehts
        ReDim QNBest(Me.myProblem.NumPrimObjective - 1)
        ReDim QBest(Me.myProblem.NumPrimObjective - 1)

        durchlauf = 1
        Tastschritte_aktuell = 0
        Tastschritte_gesamt = 0
        Extrapolationsschritte = 0
        Rueckschritte = 0
        Iterationen = 0
        b = False

        Call HookJeeves.Initialize(Me.myProblem.List_OptParameter)

        'Initialisierungssimulation
        For i = 0 To Me.myProblem.NumOptParams - 1
            aktuellePara(i) = Me.myProblem.List_OptParameter(i).Xn
        Next
        QNBest(0) = 1.79E+308
        QBest(0) = 1.79E+308
        k = 0

        Do While (HookJeeves.AktuelleSchrittweite > HookJeeves.MinimaleSchrittweite)

            'Stop?
            If (Me.stopped) Then Exit Sub

            Iterationen += 1
            durchlauf += 1

            'Bestimmen der Ausgangsgüte
            '==========================
            'Individuum instanzieren
            ind = New Common.Individuum_PES("HJ", durchlauf)

            'OptParameter ins Individuum kopieren
            For i = 0 To ind.OptParameter.Length - 1
                ind.OptParameter(i).Xn = aktuellePara(i)
            Next

            'Evaluierung
            '-----------
            If (Me.myAppType = ApplicationTypes.Sim) Then

                'Evaluierung Sim
                SIM_Eval_is_OK = Me.Sim1.Evaluate(ind)
                'TODO: Evaluierungsfehler behandeln

                'Lösung im TeeChart einzeichnen
                serie = Me.myHauptDiagramm.getSeriesPoint("Hooke and Jeeves")
                Call serie.Add(durchlauf, ind.PrimObjectives(0), durchlauf.ToString())

            Else
                'Evaluierung Testproblem
                Call Me.Testprobleme1.Evaluate(ind, 0, Me.myHauptDiagramm)

            End If
            Call System.Windows.Forms.Application.DoEvents()

            'Penalties in Bestwert kopieren
            Call ind.PrimObjectives.CopyTo(QNBest, 0)

            'Tastschritte
            '============
            For j = 0 To HookJeeves.AnzahlParameter - 1

                'Stop?
                If (Me.stopped) Then Exit Sub

                aktuellePara = HookJeeves.Tastschritt(j, EVO.HookeAndJeeves.HookeAndJeeves.TastschrittRichtung.Vorwärts)

                Tastschritte_aktuell += 1
                durchlauf += 1

                'Monitor
                Call Me.myMonitor.LogAppend("Tastschritte aktuell: " & Tastschritte_aktuell.ToString())

                'Individuum instanzieren
                ind = New Common.Individuum_PES("HJ", durchlauf)

                'OptParameter ins Individuum kopieren
                For i = 0 To ind.OptParameter.Length - 1
                    ind.OptParameter(i).Xn = aktuellePara(i)
                Next

                'Evaluierung
                '-----------
                If (Me.myAppType = ApplicationTypes.Sim) Then

                    'Evaluierung Sim
                    SIM_Eval_is_OK = Me.Sim1.Evaluate(ind)
                    'TODO: Evaluierungsfehler behandeln

                    'Lösung im TeeChart einzeichnen
                    serie = Me.myHauptDiagramm.getSeriesPoint("Hooke and Jeeves")
                    Call serie.Add(durchlauf, ind.PrimObjectives(0), durchlauf.ToString())

                Else
                    'Evaluierung Testproblem
                    Call Me.Testprobleme1.Evaluate(ind, 0, Me.myHauptDiagramm)

                End If
                Call System.Windows.Forms.Application.DoEvents()

                If (ind.PrimObjectives(0) >= QNBest(0)) Then

                    aktuellePara = HookJeeves.Tastschritt(j, EVO.HookeAndJeeves.HookeAndJeeves.TastschrittRichtung.Rückwärts)

                    Tastschritte_aktuell += 1
                    durchlauf += 1

                    'Monitor
                    Call Me.myMonitor.LogAppend("Tastschritte aktuell: " & Tastschritte_aktuell.ToString())

                    'Individuum instanzieren
                    ind = New Common.Individuum_PES("HJ", durchlauf)

                    'OptParameter ins Individuum kopieren
                    For i = 0 To ind.OptParameter.Length - 1
                        ind.OptParameter(i).Xn = aktuellePara(i)
                    Next

                    'Evaluierung
                    '-----------
                    If (Me.myAppType = ApplicationTypes.Sim) Then

                        'Evaluierung Sim
                        SIM_Eval_is_OK = Me.Sim1.Evaluate(ind)

                        'TODO: Evaluierungsfehler behandeln

                        'Lösung im TeeChart einzeichnen
                        serie = Me.myHauptDiagramm.getSeriesPoint("Hooke and Jeeves")
                        Call serie.Add(durchlauf, ind.PrimObjectives(0), durchlauf.ToString())

                    Else
                        'Evaluierung Testproblem
                        Call Me.Testprobleme1.Evaluate(ind, 0, Me.myHauptDiagramm)

                    End If
                    Call System.Windows.Forms.Application.DoEvents()

                    If (ind.PrimObjectives(0) >= QNBest(0)) Then
                        aktuellePara = HookJeeves.TastschrittResetParameter(j)
                    Else
                        Call ind.PrimObjectives.CopyTo(QNBest, 0)
                    End If
                Else
                    Call ind.PrimObjectives.CopyTo(QNBest, 0)
                End If
            Next

            Tastschritte_gesamt += Tastschritte_aktuell
            Tastschritte_aktuell = 0

            'Monitor
            Call Me.myMonitor.LogAppend("Tastschritte gesamt: " & Tastschritte_gesamt.ToString())
            Call Me.myMonitor.LogAppend("Tastschritte aktuell: " & Tastschritte_aktuell.ToString())
            Call Me.myMonitor.LogAppend("Tastschritte mittel: " & Math.Round((Tastschritte_gesamt / Iterationen), 2).ToString())

            'Extrapolationsschritt
            If (QNBest(0) < QBest(0)) Then

                'Best-Lösung im TeeChart einzeichnen
                '-----------------------------------
                serie = Me.myHauptDiagramm.getSeriesPoint("Hooke and Jeeves Best", "Green")
                Call serie.Add(durchlauf, ind.PrimObjectives(0), durchlauf.ToString())

                Call System.Windows.Forms.Application.DoEvents()

                Call QNBest.CopyTo(QBest, 0)
                Call HookJeeves.Extrapolationsschritt()
                Extrapolationsschritte += 1

                'Monitor
                Call Me.myMonitor.LogAppend("Extrapolationsschritte: " & Extrapolationsschritte.ToString())

                k += 1
                aktuellePara = HookJeeves.getLetzteParameter
                For i = 0 To HookJeeves.AnzahlParameter - 1
                    If aktuellePara(i) < 0 Or aktuellePara(i) > 1 Then
                        HookJeeves.Rueckschritt()
                        Rueckschritte += 1

                        'Monitor
                        Call Me.myMonitor.LogAppend("Rückschritte: " & Rueckschritte.ToString())

                        k += -1
                        HookJeeves.Schrittweitenhalbierung()
                        aktuellePara = HookJeeves.getLetzteParameter()
                        Exit For
                    End If
                Next
                b = True
            Else
                'If b Then
                '    HookJeeves.Rueckschritt()
                '    b = False
                'Else
                '    HookJeeves.Schrittweitenhalbierung()
                'End If
                If k > 0 Then
                    HookJeeves.Rueckschritt()

                    'Monitor
                    Call Me.myMonitor.LogAppend("Rückschritte: " & Rueckschritte.ToString())

                    HookJeeves.Schrittweitenhalbierung()
                    aktuellePara = HookJeeves.getLetzteParameter()
                Else
                    HookJeeves.Schrittweitenhalbierung()
                End If
            End If
        Loop
    End Sub

    Public Sub Stoppen() Implements IController.Stoppen
        Me.stopped = True
    End Sub

#End Region 'Methoden

End Class
