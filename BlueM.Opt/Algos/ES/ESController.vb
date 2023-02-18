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
Option Strict Off 'allows permissive type semantics. explicit narrowing conversions are not required 

Imports BlueM.Opt.Common.Constants
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Drawing
Imports System.Threading

''' <summary>
''' Kontrolliert den Ablauf der Evolutionsstrategie
''' </summary>
Public Class ESController
    Implements IController

    ''' <summary>
    ''' Multithreading Support
    ''' </summary>
    Public ReadOnly Property MultithreadingSupported() As Boolean Implements IController.MultithreadingSupported
        Get
            Return True
        End Get
    End Property

    Private myProblem As BlueM.Opt.Common.Problem
    Private mySettings As BlueM.Opt.Common.Settings
    Private myProgress As BlueM.Opt.Common.Progress
    Private myMonitor As BlueM.Opt.Diagramm.Monitor
    Private myHauptDiagramm As BlueM.Opt.Diagramm.Hauptdiagramm

    Private myAppType As BlueM.Opt.Common.ApplicationTypes
    Private WithEvents Sim1 As BlueM.Opt.Apps.Sim
    Private Testprobleme1 As BlueM.Opt.Apps.Testprobleme

    Private PES1 As PES

    Private stopped As Boolean

    'Serien für Monitor
    Private Line_Dn() As Steema.TeeChart.Styles.Line
    Private Line_Hypervolume As Steema.TeeChart.Styles.Line

#Region "Methoden"

    ''' <summary>
    ''' Initialisiert den ES-Controller und übergibt alle erforderlichen Objekte
    ''' </summary>
    ''' <param name="inputProblem"></param>
    ''' <param name="inputSettings"></param>
    ''' <param name="inputProgress"></param>
    ''' <param name="inputHptDiagramm"></param>
    Public Sub Init(ByRef inputProblem As BlueM.Opt.Common.Problem, _
                    ByRef inputSettings As BlueM.Opt.Common.Settings, _
                    ByRef inputProgress As BlueM.Opt.Common.Progress, _
                    ByRef inputHptDiagramm As BlueM.Opt.Diagramm.Hauptdiagramm) Implements IController.Init

        Me.myProblem = inputProblem
        Me.mySettings = inputSettings
        Me.myProgress = inputProgress
        Me.myHauptDiagramm = inputHptDiagramm

        Me.myMonitor = BlueM.Opt.Diagramm.Monitor.getInstance()

    End Sub

    ''' <summary>
    ''' Initialisiert den Controller für Sim-Anwendungen
    ''' </summary>
    ''' <param name="inputSim">die Simulationsanwendung</param>
    Public Sub InitApp(ByRef inputSim As BlueM.Opt.Apps.Sim) Implements IController.InitApp
        Me.myAppType = ApplicationTypes.Sim
        Me.Sim1 = inputSim
    End Sub

    ''' <summary>
    ''' Initialisiert den Controller für Testprobleme
    ''' </summary>
    Public Sub InitApp(ByRef inputTestprobleme As BlueM.Opt.Apps.Testprobleme) Implements IController.InitApp
        Me.myAppType = ApplicationTypes.Testproblems
        Me.Testprobleme1 = inputTestprobleme
    End Sub

    ''' <summary>
    ''' Optimierung starten
    ''' </summary>
    Public Sub Start() Implements IController.Start

        Me.stopped = False

        'Monitor initialisieren und anzeigen
        Call Me.InitMonitor()
        Call Me.myMonitor.SelectTabDiagramm()
        Call Me.myMonitor.Show()

        'Optimierung starten
        Call Me.STARTEN_PES()

    End Sub

    Public Sub Stoppen() Implements IController.Stoppen
        Me.stopped = True
    End Sub

    ''' <summary>
    ''' Verarbeitet ein evaluiertes Individuum
    ''' </summary>
    ''' <param name="ind">zu verarbeitendes Individuum</param>
    ''' <param name="iNachfahre">0-basierte Nachfahre-Nummer</param>
    ''' <remarks>Fängt auch das Multithreading-Event Sim.IndividuumEvaluated ab</remarks>
    Private Sub processIndividuum(ByRef ind As Common.Individuum, ByVal iNachfahre As Integer) Handles Sim1.IndividuumEvaluated
        Call Me.processIndividuum_PES(ind, iNachfahre)
    End Sub

#Region "PES"

    'Anwendung Evolutionsstrategie für Parameter Optimierung - hier Steuerung       
    '************************************************************************
    Private Sub STARTEN_PES()

        Dim i_Nachf, durchlauf As Integer
        Dim inds() As Common.Individuum_PES
        Dim isOK() As Boolean

        'Hypervolumen instanzieren
        Dim Hypervolume As BlueM.Opt.MO_Indicators.Indicators
        Hypervolume = BlueM.Opt.MO_Indicators.MO_IndicatorFabrik.GetInstance(BlueM.Opt.MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, Me.myProblem.NumPrimObjective)

        'Schritte 0: Objekt der Klasse PES wird erzeugt
        '**********************************************
        PES1 = New PES()

        'Schritte 1 - 3: ES wird initialisiert (Weiteres siehe dort ;-)
        '**************************************************************
        Call PES1.PesInitialise(Me.mySettings, Me.myProblem)

        'Verlaufsanzeige initialisieren
        Call Me.myProgress.Initialize(Me.mySettings.PES.Pop.N_Runden, Me.mySettings.PES.Pop.N_Popul, Me.mySettings.PES.N_Gen, Me.mySettings.PES.N_Nachf)

        durchlauf = 1

        'Über alle Runden
        'xxxxxxxxxxxxxxxx
        For PES1.PES_iAkt.iAktRunde = 0 To Me.mySettings.PES.Pop.N_Runden - 1

            BlueM.Opt.Common.Log.AddMessage(BlueM.Opt.Common.Log.levels.info, $"Starting round {PES1.PES_iAkt.iAktRunde}...")

            Call PES1.EsResetPopBWSpeicher() 'Nur bei Komma Strategie

            'Über alle Populationen
            'xxxxxxxxxxxxxxxxxxxxxx
            For PES1.PES_iAkt.iAktPop = 0 To Me.mySettings.PES.Pop.N_Popul - 1

                BlueM.Opt.Common.Log.AddMessage(BlueM.Opt.Common.Log.levels.info, $"Starting population {PES1.PES_iAkt.iAktPop}...")

                'POPULATIONS REPRODUKTIONSPROZESS
                '################################
                'Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern der Population
                Call PES1.EsPopReproduktion()

                'POPULATIONS MUTATIONSPROZESS
                '############################
                'Mutieren der Ausgangswerte der Population
                Call PES1.EsPopMutation()

                'Über alle Generationen
                'xxxxxxxxxxxxxxxxxxxxxx
                For PES1.PES_iAkt.iAktGen = 0 To Me.mySettings.PES.N_Gen - 1

                    BlueM.Opt.Common.Log.AddMessage(BlueM.Opt.Common.Log.levels.info, $"Starting generation {PES1.PES_iAkt.iAktGen}...")

                    Call PES1.EsResetBWSpeicher()  'Nur bei Komma Strategie

                    ReDim inds(Me.mySettings.PES.N_Nachf - 1)
                    ReDim isOK(Me.mySettings.PES.N_Nachf - 1)

                    'Schleife über alle Nachkommen
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                    For i_Nachf = 0 To Me.mySettings.PES.N_Nachf - 1

                        'Stop?
                        If (Me.stopped) Then Exit Sub

                        durchlauf += 1

                        'Neues Individuum instanzieren
                        inds(i_Nachf) = New Common.Individuum_PES("PES", durchlauf)

                        'Neue Parameter holen
                        Call PES_getNewParameters(inds(i_Nachf))

                        If (myAppType = ApplicationTypes.Testproblems) Then

                            'Testprobleme immer direkt auswerten
                            '===================================

                            'Lösung evaluieren und zeichnen
                            Call Testprobleme1.Evaluate(inds(i_Nachf), PES1.PES_iAkt.iAktPop, Me.myHauptDiagramm)

                            'Evaluierung verarbeiten
                            Call Me.processIndividuum_PES(inds(i_Nachf), i_Nachf)

                        ElseIf (Not Me.mySettings.General.UseMultithreading) Then

                            'Simulationsanwendungen ohne Multithreading
                            'auch sofort auswerten
                            '==========================================

                            isOK(i_Nachf) = Sim1.Evaluate(inds(i_Nachf), True)

                            'Stop?
                            If (Me.stopped) Then Exit Sub

                            'Evaluierungsfehler behandeln
                            '----------------------------
                            If (Not isOK(i_Nachf)) Then

                                BlueM.Opt.Common.Log.AddMessage(BlueM.Opt.Common.Log.levels.warning, $"Evaluation of child {i_Nachf} was unsuccessful, a new parameter set will be generated and evaluated...")

                                Dim n_Tries As Integer = 0

                                Do
                                    'Abbruchkriterium
                                    n_Tries += 1
                                    If (n_Tries > 10) Then
                                        Throw New Exception("Unable to generate a valid dataset after 10 tries!")
                                    End If

                                    'Parametersatz erneuern
                                    Call PES_getNewParameters(inds(i_Nachf))

                                    'Neu evaluieren
                                    isOK(i_Nachf) = Sim1.Evaluate(inds(i_Nachf), True)

                                Loop Until (isOK(i_Nachf) = True)

                            End If

                            'Evaluierung verarbeiten
                            Call processIndividuum_PES(inds(i_Nachf), i_Nachf)

                        End If

                    Next 'Ende alle Nachfahren (singlethread)
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    If (Me.mySettings.General.UseMultithreading) Then

                        'Simulationsanwendungen mit Multithreading
                        'nachträglich auswerten
                        '=========================================

                        'Alle Individuen evaluieren
                        isOK = Sim1.Evaluate(inds, True)

                        'Stop?
                        If (Me.stopped) Then Exit Sub

                        'Anzahl Evaluierungsfehler bestimmen und in Log schreiben
                        Dim errorcount As Integer = 0
                        For Each success As Boolean In isOK
                            If Not success Then errorcount += 1
                        Next
                        If errorcount > 0 Then
                            Dim msg As String
                            msg = $"Evaluation of {errorcount} children of the current generation failed, will generate and evaluate new parameter sets..."
                            BlueM.Opt.Common.Log.AddMessage(BlueM.Opt.Common.Log.levels.warning, msg)
                        End If

                        'Alle evaluierten Individuen durchlaufen
                        For i_Nachf = 0 To inds.GetUpperBound(0)

                            'Evaluierungsfehler behandeln
                            '----------------------------
                            If (Not isOK(i_Nachf)) Then

                                Dim n_Tries As Integer = 0

                                Do
                                    'Abbruchkriterium
                                    n_Tries += 1
                                    If (n_Tries > 10) Then
                                        Throw New Exception("Unable to generate a valid dataset after 10 tries!")
                                    End If

                                    'Parametersatz erneuern
                                    Call PES_getNewParameters(inds(i_Nachf))

                                    'Neu evaluieren (ohne Multithreading)
                                    isOK(i_Nachf) = Sim1.Evaluate(inds(i_Nachf), True)

                                Loop Until (isOK(i_Nachf) = True)

                                'Evaluierung verarbeiten
                                Call processIndividuum_PES(inds(i_Nachf), i_Nachf)

                            End If

                            'erfolgreich evaluierte Individuen wurden bereits über Event verarbeitet

                        Next

                    End If 'Ende alle Nachfahren (multithread)
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    BlueM.Opt.Common.Log.AddMessage(BlueM.Opt.Common.Log.levels.info, $"Generation {PES1.PES_iAkt.iAktGen} completed.")

                    'SELEKTIONSPROZESS Schritt 2 für NDSorting sonst Xe = Xb
                    'Die neuen Eltern werden generiert
                    BlueM.Opt.Common.Log.AddMessage(BlueM.Opt.Common.Log.levels.info, "Determining new parents...")
                    Call PES1.EsEltern()

                    'Sekundäre Population
                    '====================
                    If (Me.mySettings.PES.OptModus = Common.Constants.EVO_MODE.Multi_Objective) Then

                        BlueM.Opt.Common.Log.AddMessage(BlueM.Opt.Common.Log.levels.info, "Updating secondary population...")

                        'SekPop abspeichern
                        '------------------
                        If (Not IsNothing(Sim1)) Then
                            Call Sim1.OptResult.setSekPop(PES1.SekundärQb, PES1.PES_iAkt.iAktGen)
                        End If

                        'SekPop zeichnen
                        '---------------
                        If (Not IsNothing(Sim1)) Then
                            'Umweg über Sim1.OptResult gehen, weil es im PES keine Individuum-IDs gibt (#177)
                            Call Me.myHauptDiagramm.ZeichneSekPopulation(Sim1.OptResult.getSekPop())
                        Else
                            Call Me.myHauptDiagramm.ZeichneSekPopulation(PES1.SekundärQb)
                        End If

                        'Hypervolumen berechnen und Zeichnen
                        '-----------------------------------
                        Call Hypervolume.update_dataset(Common.Individuum.Get_All_Penalty_of_Array(PES1.SekundärQb))
                        Call Me.ZeichneNadirpunkt(Hypervolume.nadir)
                        Call Me.ZeichneHyperVolumen(PES1.PES_iAkt.iAktGen + 1, Math.Abs(Hypervolume.calc_indicator()))

                    End If

                    'ggf. alte Generation aus Diagramm löschen
                    If (Me.mySettings.General.DrawOnlyCurrentGeneration _
                        And PES1.PES_iAkt.iAktGen < Me.mySettings.PES.N_Gen - 1) Then
                        Call Me.myHauptDiagramm.LöscheLetzteGeneration(PES1.PES_iAkt.iAktPop)
                    End If

                    'Verlauf aktualisieren
                    Me.myProgress.iNachf = 0
                    Me.myProgress.iGen = PES1.PES_iAkt.iAktGen + 1

                    System.Windows.Forms.Application.DoEvents()

                Next 'Ende alle Generationen
                'xxxxxxxxxxxxxxxxxxxxxxxxxxx
                System.Windows.Forms.Application.DoEvents()

                'POPULATIONS SELEKTIONSPROZESS  Schritt 1
                '########################################
                'Einordnen der Qualitätsfunktion im PopulationsBestwertspeicher
                Call PES1.EsPopBest()

                'Verlauf aktualisieren
                Me.myProgress.iPopul = PES1.PES_iAkt.iAktPop + 1

            Next 'Ende alle Populationen
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

            'POPULATIONS SELEKTIONSPROZESS  Schritt 2
            '########################################
            'Die neuen Populationseltern werden generiert
            Call PES1.EsPopEltern()

            'Verlauf aktualisieren
            Me.myProgress.iRunde = PES1.PES_iAkt.iAktRunde + 1

        Next 'Ende alle Runden
        'xxxxxxxxxxxxxxxxxxxxx

    End Sub

    ''' <summary>
    ''' Führt Reproduktion und Mutation aus und kopiert die neu gewonnenen Parameter ins Individuum
    ''' </summary>
    ''' <param name="ind">Das Individuum, dessen Parameter erneuert werden soll</param>
    Private Sub PES_getNewParameters(ByRef ind As BlueM.Opt.Common.Individuum_PES)

        'REPRODUKTIONSPROZESS
        'Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern
        Call PES1.EsReproduktion()

        'MUTATIONSPROZESS
        'Mutieren der Ausgangswerte
        Call PES1.EsMutation()

        'Auslesen der variierten Parameter und in Individuum kopieren
        ind.OptParameter = BlueM.Opt.Common.OptParameter.Clone_Array(PES1.EsGetParameter())

    End Sub

    ''' <summary>
    ''' Verarbeitet ein evaluiertes Individuum für PES:
    ''' * Individuum im PES-Bestwertspeicher einordnen
    ''' * Lösung im Hauptdiagramm zeichnen
    ''' * Dn im Monitor zeichnen
    ''' * Verlaufsanzeige aktualisieren
    ''' </summary>
    ''' <param name="ind">Das zu verarbeitende Individuum</param>
    ''' <param name="iNachfahre">0-basierte Nachfahrens-Nummer</param>
    Private Sub processIndividuum_PES(ByRef ind As Common.Individuum_PES, ByVal iNachfahre As Integer)

        'Lösung im Hauptdiagramm zeichnen (Testprobleme zeichnen sich selber)
        If (myAppType = ApplicationTypes.Sim) Then
            Call Me.myHauptDiagramm.ZeichneIndividuum(ind, PES1.PES_iAkt.iAktRunde, PES1.PES_iAkt.iAktPop, PES1.PES_iAkt.iAktGen, iNachfahre + 1, Color.Orange)
        End If

        'Dn in Monitor zeichnen
        Me.Zeichne_Dn(PES1.PES_iAkt.iAktGen * Me.mySettings.PES.N_Nachf + iNachfahre + 1, ind)

        'SELEKTIONSPROZESS Schritt 1
        'Einordnen der Qualitätsfunktion im Bestwertspeicher bei SO
        'Falls MO Einordnen der Qualitätsfunktion in NDSorting
        PES1.PES_iAkt.iAktNachf = iNachfahre
        Call PES1.EsBest(ind)

        'Verlauf aktualisieren (don't use iNachf as they are not always processed in order)
        Me.myProgress.NextNachf()

        System.Windows.Forms.Application.DoEvents()

    End Sub

#End Region 'PES

#Region "Ausgabe"

    ''' <summary>
    ''' Bereitet das Monitordiagramm vor
    ''' </summary>
    Private Sub InitMonitor()

        'Schrittweite(n)
        '---------------
        With Me.myMonitor.Diag

            'Durchlaufachse (unten)
            .Axes.Bottom.Title.Caption = "Iteration"

            'Schrittweitenachse (links)
            .Axes.Left.Title.Caption = "Step size"

        End With

        If (Me.mySettings.PES.SetMutation.IsDnVektor) Then

            'Bei PES-Schrittweitenvektor eine Linie für jeden Parameter
            ReDim Me.Line_Dn(Me.myProblem.List_OptParameter.Length - 1)
            For i As Integer = 0 To Me.myProblem.List_OptParameter.Length - 1
                Me.Line_Dn(i) = Me.myMonitor.Diag.getSeriesLine("Step size " & Me.myProblem.List_OptParameter(i).Bezeichnung)
            Next
        Else
            'Ansonsten nur eine Linie
            ReDim Me.Line_Dn(0)
            Me.Line_Dn(0) = Me.myMonitor.Diag.getSeriesLine("Step size", "Blue")
        End If

        'Linien formatieren
        For Each line As Steema.TeeChart.Styles.Line In Me.Line_Dn
            line.Pointer.Visible = True
            line.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            line.Pointer.HorizSize = 2
            line.Pointer.VertSize = 2
            line.Pointer.Pen.Visible = False
        Next

        'Hypervolumen
        '------------
        If (Me.myProblem.NumPrimObjective > 1) Then
            With Me.myMonitor.Diag

                'Generationsachse (oben)
                .Axes.Top.Visible = True
                .Axes.Top.Title.Caption = "Generation"
                .Axes.Top.Horizontal = True
                .Axes.Top.Automatic = True
                .Axes.Top.Grid.Visible = False

                'Hypervolumenachse (rechts)
                .Axes.Right.Visible = True
                .Axes.Right.Title.Caption = "Hypervolume"
                .Axes.Right.Title.Angle = 90
                .Axes.Right.Automatic = True
                .Axes.Right.Grid.Visible = False

            End With

            'Linie
            Me.Line_Hypervolume = Me.myMonitor.Diag.getSeriesLine("Hypervolume", "Red")
            Me.Line_Hypervolume.CustomHorizAxis = Me.myMonitor.Diag.Axes.Top
            Me.Line_Hypervolume.CustomVertAxis = Me.myMonitor.Diag.Axes.Right
            Me.Line_Hypervolume.Color = System.Drawing.Color.Red
            Me.Line_Hypervolume.Pointer.Visible = True
            Me.Line_Hypervolume.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Me.Line_Hypervolume.Pointer.Brush.Color = System.Drawing.Color.Red
            Me.Line_Hypervolume.Pointer.HorizSize = 2
            Me.Line_Hypervolume.Pointer.VertSize = 2
            Me.Line_Hypervolume.Pointer.Pen.Visible = False

        End If
    End Sub

    ''' <summary>
    ''' Hypervolumenwert in Monitordiagramm eintragen
    ''' </summary>
    ''' <param name="gen">Generationsnummer</param>
    ''' <param name="indicator">Hypervolumenwert</param>
    Private Sub ZeichneHyperVolumen(ByVal gen As Integer, ByVal indicator As Double)
        Me.Line_Hypervolume.Add(gen, indicator, gen.ToString())
    End Sub

    ''' <summary>
    ''' Schrittweite(n) in Monitordiagramm eintragen
    ''' </summary>
    ''' <param name="durchlauf">Durchlaufnummer (für X-Achse)</param>
    ''' <param name="ind">Individuum, dessen Schrittweite(n) gezeichnet werden sollen</param>
    Private Sub Zeichne_Dn(ByVal durchlauf As Integer, ByVal ind As Common.Individuum)

        Dim i As Integer

        If (Me.mySettings.PES.SetMutation.IsDnVektor) Then
            'Bei Schrittweitenvektor mehrere Linien
            For i = 0 To ind.OptParameter.Length - 1
                Me.Line_Dn(i).Add(durchlauf, ind.OptParameter(i).Dn, durchlauf.ToString)
            Next
        Else
            'Ansonsten nur eine Schrittweite
            Me.Line_Dn(0).Add(durchlauf, ind.OptParameter(0).Dn, durchlauf.ToString)
        End If

    End Sub

    ''' <summary>
    ''' Nadirpunkt in Hauptdiagramm eintragen
    ''' </summary>
    ''' <param name="nadir">Koordinaten des Nadirpunkts</param>
    Private Sub ZeichneNadirpunkt(ByVal nadir() As Double)

        If (Me.myProblem.NumPrimObjective = 2) Then
            '2D
            '--
            Dim serie2 As Steema.TeeChart.Styles.Points
            serie2 = Me.myHauptDiagramm.getSeriesPoint("Nadir point", "Blue", Steema.TeeChart.Styles.PointerStyles.Diamond)
            serie2.Clear()
            serie2.Add(nadir(0), nadir(1), "Nadir point")
        Else
            '3D
            '--
            Dim serie3 As Steema.TeeChart.Styles.Points3D
            serie3 = Me.myHauptDiagramm.getSeries3DPoint("Nadir point", "Blue", Steema.TeeChart.Styles.PointerStyles.Diamond)
            serie3.Clear()
            serie3.Add(nadir(0), nadir(1), nadir(2), "Nadir point")
        End If

    End Sub

#End Region 'Ausgabe

#End Region 'Methoden

End Class
