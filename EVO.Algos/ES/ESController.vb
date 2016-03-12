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
Option Strict Off 'allows permissive type semantics. explicit narrowing conversions are not required 

Imports IHWB.EVO.Common.Constants
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Drawing
Imports System.Threading

''' <summary>
''' Kontrolliert den Ablauf der Evolutionsstrategie (PES, CES und HYBRID)
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

    Private myProblem As EVO.Common.Problem
    Private mySettings As EVO.Common.Settings
    Private myProgress As EVO.Common.Progress
    Private myMonitor As EVO.Diagramm.Monitor
    Private myHauptDiagramm As EVO.Diagramm.Hauptdiagramm

    Private myAppType As EVO.Common.ApplicationTypes
    Private WithEvents Sim1 As EVO.Apps.Sim
    Private Testprobleme1 As EVO.Apps.Testprobleme

    Private PES1 As PES
    Private CES1 As CES

    Private stopped As Boolean

    '**** CES-spezifische Sachen ****
    Private CES_i_gen As Integer 'ginge alternativ auch mit Me.myProgress.iGen
    Private ColorArray As Object(,)
    Private MI_Thread_OK As Boolean = False

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
    Public Sub Init(ByRef inputProblem As EVO.Common.Problem, _
                    ByRef inputSettings As EVO.Common.Settings, _
                    ByRef inputProgress As EVO.Common.Progress, _
                    ByRef inputHptDiagramm As EVO.Diagramm.Hauptdiagramm) Implements IController.Init

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
        Me.myAppType = ApplicationTypes.Testprobleme
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

        'Je nach Methode Optimierung starten
        Select Case Me.myProblem.Method

            Case METH_PES
                Call Me.STARTEN_PES()

            Case METH_CES, METH_HYBRID
                Call Me.STARTEN_CES_or_HYBRID()
        End Select

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

        Select Case Me.myProblem.Method

            Case METH_PES

                Call Me.processIndividuum_PES(ind, iNachfahre)

            Case METH_CES, METH_HYBRID

                Call Me.processIndividuum_CES(iNachfahre)

        End Select
    End Sub

#Region "CES"

    'Anwendung CES und CES_PES
    '*************************
    Private Sub STARTEN_CES_or_HYBRID()

        Dim durchlauf_all As Integer
        Dim isOK() As Boolean
        Dim TimePerGeneration() As TimeSpan
        Dim OptTimeParaPerGen As New Stopwatch()

        'Hypervolumen instanzieren
        Dim Hypervolume As EVO.MO_Indicators.Indicators
        Hypervolume = EVO.MO_Indicators.MO_IndicatorFabrik.GetInstance(EVO.MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, Me.myProblem.NumPrimObjective)

        'CES initialisieren
        CES1 = New EVO.ES.CES()
        Call CES1.CESInitialise(Me.mySettings, Me.myProblem, Sim1.VerzweigungsDatei.GetLength(0))

        'Progress initialisieren
        Call Me.myProgress.Initialize(1, 1, Me.mySettings.CES.N_Generations, Me.mySettings.CES.N_Children)

        'ColorArray dimensionieren
        ReDim Me.ColorArray(CES1.ModSett.n_Locations, -1)

        'Laufvariable für die Generationen
        Dim i_ch, i_loc As Integer

        'Zufällige Kinderpfade werden generiert
        '**************************************
        If Me.myProblem.CES_T_Modus = Common.Constants.CES_T_MODUS.No_Test Then
            Call CES1.Generate_Random_Path()
        ElseIf (Not Me.myProblem.CES_T_Modus = Common.Constants.CES_T_MODUS.No_Test) Then
            'Falls TESTMODUS werden sie überschrieben
            Call CES1.Generate_Paths_for_Tests(Sim1.TestPath, Me.myProblem.CES_T_Modus)
        End If
        '**************************************

        'Hier werden dem Child die passenden Massnahmen und deren Elemente pro Location zugewiesen
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        For i_ch = 0 To CES1.mSettings.CES.N_Children - 1
            'Das Dn wird gesetzt
            If Me.mySettings.PES.SetMutation.IsDnVektor = False Then
                CES1.Children(i_ch).CES_Dn = Me.mySettings.PES.SetMutation.DnStart
            End If
            For i_loc = 0 To CES1.ModSett.n_Locations - 1
                Call Sim1.Identify_Measures_Elements_Parameters(i_loc, CES1.Children(i_ch).Path(i_loc), CES1.Children(i_ch).Measures(i_loc), CES1.Children(i_ch).Loc(i_loc).Loc_Elem, CES1.Children(i_ch).Loc(i_loc).PES_OptPara)
            Next
        Next

        'Falls HYBRID werden entprechend der Einstellung im PES die Parameter auf Zufällig oder Start gesetzt
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        If (Me.myProblem.Method = METH_HYBRID And Me.mySettings.CES.HybridType = Common.Constants.HYBRID_TYPE.Mixed_Integer) Then
            CES1.Set_Xn_And_Dn_per_Location()
        End If

        'Startwerte werden der Verlaufsanzeige zugewiesen
        Call Me.myProgress.Initialize(1, 1, Me.mySettings.CES.N_Generations, Me.mySettings.CES.N_Children)

        'xxxx Optimierung xxxxxx
        'Generationsschleife CES
        'xxxxxxxxxxxxxxxxxxxxxxx
        ReDim TimePerGeneration(CES1.mSettings.CES.N_Generations)

        durchlauf_all = 0

        For Me.CES_i_gen = 0 To CES1.mSettings.CES.N_Generations - 1

            'HACK: IDs an Individuen vergeben
            For Each ind As EVO.Common.Individuum_CES In CES1.Children
                durchlauf_all += 1
                ind.ID = durchlauf_all + 1
            Next

            OptTimeParaPerGen.Reset()
            OptTimeParaPerGen.Start()

            'Individuen mit Multithreading evaluieren
            '****************************************
            isOK = Sim1.Evaluate(CES1.Children, True)
            '****************************************

            OptTimeParaPerGen.Stop()
            EVO.Diagramm.Monitor.getInstance().LogAppend("Die Evaluierung der Generation " & Me.CES_i_gen & " dauerte:   " & OptTimeParaPerGen.Elapsed.Hours & "h  " & OptTimeParaPerGen.Elapsed.Minutes & "m  " & OptTimeParaPerGen.Elapsed.Seconds & "s     " & OptTimeParaPerGen.Elapsed.Seconds & "ms")
            TimePerGeneration(Me.CES_i_gen + 1) = OptTimeParaPerGen.Elapsed
            TimePerGeneration(0) += OptTimeParaPerGen.Elapsed

            'Stop?
            If (Me.stopped) Then Exit Sub

            'Evaluierte Individuen verarbeiten
            For i_Child As Integer = 0 To CES1.Children.Length - 1

                If (Not isOK(i_Child)) Then
                    'Simulationsfehler aufgetreten!
                    Dim msg As String = "Der Nachfahre mit der ID " & CES1.Children(i_Child).ID & " wurde nicht richtig evaluiert! Die Simulation wurde mit Fehlern abgebrochen."
                    MsgBox(msg, MsgBoxStyle.Critical, "ESController")
                    'Abbruch!
                    Exit Sub
                End If
                'erfolgreich evaluierte Individuen wurden bereits über Event verarbeitet

                'Abbruchkriterium für die Qualitätsprüfung (1372.82)
                If mySettings.General.BatchMode And CES1.Children(i_Child).PrimObjectives(0) < 1306 Then
                    Me.myMonitor.LogAppend("Generation: " & Me.CES_i_gen & "   Child: " & i_Child)
                    Exit Sub
                End If

            Next

            'Generation hochzählen
            Me.myProgress.iGen = CES_i_gen + 1

            'Die Listen müssen nach der letzten Evaluierung wieder zurückgesetzt werden
            'Sicher ob das benötigt wird?
            Call Me.myProblem.Reset_OptPara_and_ModPara()

            'MO oder SO SELEKTIONSPROZESS oder NDSorting SELEKTION
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            'BUG 259: CES: Punkt-Labels der Sekundärpopulation fehlen noch!
            If (Me.myProblem.NumPrimObjective = 1) Then
                'Sortieren der Kinden anhand der Qualität
                Call CES1.Sort_Individuum(CES1.Children)
                'Selectionsprozess je nach "plus" oder "minus" Strategie
                Call CES1.Selection_Process()
                'Zeichnen der besten Eltern
                'For i_ch = 0 To Me.mySettings.CES.n_Parents - 1
                '    'durchlauf += 1
                '    serie = Me.myHauptdiagramm.getSeriesPoint("Parent", "green")
                '    Call serie.Add(durchlauf_all, CES1.Parents(i_ch).Penalties(0))
                'Next
            Else
                'NDSorting ******************
                Call CES1.NDSorting_CES_Control(CES_i_gen)

                'Sekundäre Population
                '--------------------
                If (Not IsNothing(Sim1)) Then
                    'SekPop abspeichern
                    Call Sim1.OptResult.setSekPop(CES1.SekundärQb, CES_i_gen)
                End If

                'SekPop zeichnen
                Call Me.myHauptDiagramm.ZeichneSekPopulation(CES1.SekundärQb)

                'Hypervolumen berechnen und zeichnen
                '-----------------------------------
                Call Hypervolume.update_dataset(Common.Individuum.Get_All_Penalty_of_Array(CES1.SekundärQb))
                Call Me.ZeichneNadirpunkt(Hypervolume.nadir)
                Call Me.ZeichneHyperVolumen(CES_i_gen, Math.Abs(Hypervolume.calc_indicator()))
            End If
            ' ^ ENDE Selectionsprozess
            'xxxxxxxxxxxxxxxxxxxxxxxxx

            'Stop?
            If (Me.stopped) Then Exit Sub

            'REPRODUKTION und MUTATION Nicht wenn Testmodus
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            If (Me.myProblem.CES_T_Modus = Common.Constants.CES_T_MODUS.No_Test) Then
                'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
                CES1.Children = Common.Individuum.New_Indi_Array(EVO.Common.Individuum.Individuumsklassen.Individuum_CES, CES1.Children.GetLength(0), "Child")
                'Reproduktionsoperatoren, hier gehts dezent zur Sache
                Call CES1.Reproduction_Control()
                'Mutationsoperatoren
                Call CES1.Mutation_Control()
            End If

            'Hier werden dem Child die passenden Massnahmen und deren Elemente pro Location zugewiesen
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            For i_ch = 0 To Me.mySettings.CES.N_Children - 1
                For i_loc = 0 To CES1.ModSett.n_Locations - 1
                    Call Sim1.Identify_Measures_Elements_Parameters(i_loc, CES1.Children(i_ch).Path(i_loc), CES1.Children(i_ch).Measures(i_loc), CES1.Children(i_ch).Loc(i_loc).Loc_Elem, CES1.Children(i_ch).Loc(i_loc).PES_OptPara)
                Next
            Next

            'HYBRID: REPRODUKTION und MUTATION
            '*********************************
            If (Me.myProblem.Method = METH_HYBRID And Me.mySettings.CES.HybridType = Common.Constants.HYBRID_TYPE.Mixed_Integer) Then
                MI_Thread_OK = False
                Dim MI_Thread As Thread
                MI_Thread = New Thread(AddressOf Me.Mixed_Integer_PES)
                MI_Thread.IsBackground = True
                MI_Thread.Start(CES_i_gen)
                While MI_Thread_OK = False
                    Thread.Sleep(100)
                    Application.DoEvents()
                End While
            End If
        Next
        'Ende der Generationsschleife CES

        EVO.Diagramm.Monitor.getInstance().LogAppend("Die Evaluierung aller Generationen dauerte:   " & TimePerGeneration(0).Hours.ToString & "h  " & TimePerGeneration(0).Minutes.ToString & "m  " & TimePerGeneration(0).Seconds.ToString & "s     " & TimePerGeneration(0).Milliseconds.ToString & "ms")

        'Falls jetzt noch PES ausgeführt werden soll
        'Starten der PES mit der Front von CES
        '*******************************************
        If (Me.myProblem.Method = METH_HYBRID And Me.mySettings.CES.HybridType = Common.Constants.HYBRID_TYPE.Sequencial_1) Then
            Call Start_PES_after_CES()
        End If

    End Sub

    'Mixed_Integer Teil ermittelt die PES Parameter für jedes neues Child und jede Location
    'Achtung! wird auch als Thread gestartet um weiter aufs Form zugreifen zu können
    '**************************************************************************************
    Private Sub Mixed_Integer_PES(ByVal i_gen As Integer)

        System.Threading.Thread.CurrentThread.Priority = Threading.ThreadPriority.BelowNormal

        Dim i_Child, i_loc As Integer

        'Selection oder NDSorting für den PES Memory
        '*******************************************
        If CES1.PES_Memory.GetLength(0) > CES1.mSettings.CES.N_PES_MemSize Then
            If (Me.myProblem.NumPrimObjective = 1) Then
                'Sortieren des PES_Memory anhande der Qualität
                Call CES1.Sort_Individuum(CES1.PES_Memory)
                'Kürzen des PES_Memory
                ReDim Preserve CES1.PES_Memory(CES1.mSettings.CES.N_PES_MemSize - 1)
            Else
                Call CES1.NDSorting_Memory(i_gen)
            End If
        End If

        'pro Child
        'xxxxxxxxx
        For i_Child = 0 To CES1.Children.GetUpperBound(0)

            'Das Dn des Child mutieren
            If Me.mySettings.PES.SetMutation.IsDnVektor = False Then
                Dim PESX As EVO.ES.PES
                PESX = New EVO.ES.PES
                Call PESX.PesInitialise(Me.mySettings, Me.myProblem)
                CES1.Children(i_Child).CES_Dn = PESX.CES_Dn_Mutation(CES1.Children(i_Child).CES_Dn)
            End If

            'Ermittelt fuer jedes Child den PES Parent Satz (PES_Parents ist das Ergebnis)
            Call CES1.Memory_Search_per_Child(CES1.Children(i_Child))

            'und pro Location
            'xxxxxxxxxxxxxxxx
            For i_loc = 0 To CES1.ModSett.n_Locations - 1

                'Die Parameter (falls vorhanden) werden überschrieben
                If Not CES1.Children(i_Child).Loc(i_loc).PES_OptPara.GetLength(0) = 0 Then

                    'Ermittelt fuer jede Location den PES Parent Satz (PES_Parents ist das Ergebnis)
                    '*******************************************************************************
                    Call CES1.Memory_Search_per_Location(i_loc)

                    'Führt das Sortieren oder NDSorting für diesen Satz durch
                    '********************************************************
                    If CES1.PES_Parents_pLoc.GetLength(0) > CES1.mSettings.PES.N_Eltern Then
                        If (Me.myProblem.NumPrimObjective = 1) Then
                            'Sortieren der Parents anhand der Qualität
                            Call CES1.Sort_Individuum(CES1.PES_Parents_pLoc)
                            'Kürzen der Parents
                            ReDim Preserve CES1.PES_Parents_pLoc(CES1.mSettings.PES.N_Eltern - 1)
                        Else
                            Call CES1.NDSorting_PES_Parents_per_Loc(i_gen)
                        End If
                    End If

                    Dim m As Integer
                    Select Case CES1.PES_Parents_pLoc.GetLength(0)

                        Case Is = 0
                            'Noch keine Eltern vorhanden (die Child Location bekommt neue - zufällige Werte oder original Parameter)
                            '*******************************************************************************************************
                            For m = 0 To CES1.Children(i_Child).Loc(i_loc).PES_OptPara.GetUpperBound(0)
                                CES1.Children(i_Child).Loc(i_loc).PES_OptPara(m).Dn = CES1.mSettings.PES.SetMutation.DnStart
                                'Falls zufällige Startwerte
                                If CES1.mSettings.PES.Startparameter = Common.Constants.EVO_STARTPARAMETER.Zufall Then
                                    Randomize()
                                    CES1.Children(i_Child).Loc(i_loc).PES_OptPara(m).Xn = Rnd()
                                End If
                            Next

                        Case Is > 0
                            'Eltern vorhanden (das PES wird gestartet)
                            '*****************************************
                            If CES1.PES_Parents_pLoc.GetLength(0) < CES1.mSettings.PES.N_Eltern Then
                                'Falls es zu wenige sind wird mit den vorhandenen aufgefüllt
                                Call CES1.fill_Parents_per_Loc(CES1.PES_Parents_pLoc, CES1.mSettings.PES.N_Eltern)
                            End If

                            'Schritt 0: PES - Objekt der Klasse PES wird erzeugt PES wird erzeugt
                            '*********************************************************************
                            Dim PES1 As EVO.ES.PES
                            PES1 = New EVO.ES.PES()

                            'Vorbereitung um das PES zu initieren
                            '************************************
                            Me.myProblem.List_OptParameter = CES1.Children(i_Child).Loc(i_loc).PES_OptPara

                            'Schritte 1 - 3: PES wird initialisiert (Weiteres siehe dort ;-)
                            '**************************************************************
                            Call PES1.PesInitialise(Me.mySettings, Me.myProblem)

                            'Die PopulationsEltern des PES werden gefüllt
                            For m = 0 To CES1.PES_Parents_pLoc.GetUpperBound(0)
                                Call PES1.EsStartvalues(CES1.mSettings.CES.Is_PopMutStart, CES1.PES_Parents_pLoc(m).Loc(i_loc).PES_OptPara, m)
                            Next

                            'Startet die Prozesse evolutionstheoretischen Prozesse nacheinander
                            Call PES1.EsReproMut(CES1.Children(i_Child).CES_Dn, Me.mySettings.CES.Is_PopMutStart)

                            'Auslesen der Variierten Parameter
                            CES1.Children(i_Child).Loc(i_loc).PES_OptPara = EVO.Common.OptParameter.Clone_Array(PES1.EsGetParameter())

                    End Select
                End If

                'Hier wird mean Dn auf alle Locations und PES OptParas übertragen
                'CES1.Children(i_ch).Set_mean_PES_Dn = CES1.Children(i_ch).Get_mean_PES_Dn
            Next
        Next

        MI_Thread_OK = True
    End Sub

    'Starten der PES mit der Front von CES
    '(MaxAnzahl ist die Zahl der Eltern -> ToDo: SecPop oder Bestwertspeicher)
    '*************************************************************************
    Private Sub Start_PES_after_CES()

        Dim i As Integer

        For i = 0 To Me.mySettings.CES.N_Parents - 1
            If CES1.Parents(i).Front = 1 Then

                '****************************************
                'Aktueller Pfad wird an Sim zurückgegeben
                'Bereitet das BlaueModell für die Kombinatorik vor
                Call Sim1.PREPARE_Evaluation_CES(CES1.Children(i))

                'Hier werden Child die passenden Elemente zugewiesen
                Dim j As Integer
                For j = 0 To CES1.ModSett.n_Locations - 1
                    Call Sim1.Identify_Measures_Elements_Parameters(j, CES1.Children(i).Path(j), CES1.Children(i).Measures(j), CES1.Children(i).Loc(j).Loc_Elem, CES1.Children(i).Loc(j).PES_OptPara)
                Next

                'Reduktion der OptimierungsParameter und immer dann wenn nicht Nullvariante
                'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                If (Me.myProblem.Reduce_OptPara_and_ModPara(CES1.Children(i).Get_All_Loc_Elem)) Then

                    'Starten der PES
                    '***************
                    Call STARTEN_PES()

                End If
            End If
        Next
    End Sub


    ''' <summary>
    ''' Verarbeitet ein evaluiertes Individuum für CES:
    ''' * Individuum im Memory-Store speichern (HYBRID)
    ''' * Lösung im Hauptdiagramm zeichnen
    ''' * Dn im Monitor zeichnen
    ''' * Verlaufsanzeige aktualisieren
    ''' </summary>
    ''' <param name="i_Child">0-basierte Nachfahrens-Nummer</param>
    Private Sub processIndividuum_CES(ByVal i_Child As Integer)

        'HYBRID: Speichert die PES Erfahrung dieses Child im PES Memory
        '**************************************************************
        If (Me.myProblem.Method = METH_HYBRID And Me.mySettings.CES.HybridType = Common.Constants.HYBRID_TYPE.Mixed_Integer) Then
            Call CES1.Memory_Store(i_Child, Me.CES_i_gen)
        End If

        'Lösung im TeeChart einzeichnen und Dn zeichnen
        '==============================================
        If myProblem.CES_T_Modus = CES_T_MODUS.No_Test Then
            If myProblem.Method = METH_CES Then
                Call Me.myHauptDiagramm.ZeichneIndividuum(CES1.Children(i_Child), 0, 0, Me.CES_i_gen, i_Child + 1, Color.Orange)
            ElseIf myProblem.Method = METH_HYBRID Then
                Call Me.myHauptDiagramm.ZeichneIndividuum(CES1.Children(i_Child), 0, 0, Me.CES_i_gen, i_Child + 1, EVO.Diagramm.Diagramm.ColorManagement(ColorArray, CES1.Children(i_Child)))
            End If
        Else
            Call Me.myHauptDiagramm.ZeichneIndividuum(CES1.Children(i_Child), 0, 0, Me.CES_i_gen, i_Child + 1, Color.Orange)
        End If
        Me.Zeichne_Dn(CES1.Children(i_Child).ID, CES1.Children(i_Child))

        'Verlauf aktualisieren
        Me.myProgress.iNachf = i_Child + 1

    End Sub

#End Region 'CES

#Region "PES"

    'Anwendung Evolutionsstrategie für Parameter Optimierung - hier Steuerung       
    '************************************************************************
    Private Sub STARTEN_PES()

        Dim i_Nachf, durchlauf As Integer
        Dim inds() As Common.Individuum_PES
        Dim isOK() As Boolean

        'Hypervolumen instanzieren
        Dim Hypervolume As EVO.MO_Indicators.Indicators
        Hypervolume = EVO.MO_Indicators.MO_IndicatorFabrik.GetInstance(EVO.MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, Me.myProblem.NumPrimObjective)

        'Diagramm vorbereiten und initialisieren
        'TODO: If (Not Me.myProblem.Method = METH_HYBRID And Not Me.mySettings.CES.ty_Hybrid = Common.Constants.HYBRID_TYPE.Sequencial_1) Then
        '    Call PrepareDiagramm()
        'End If

        'Schritte 0: Objekt der Klasse PES wird erzeugt
        '**********************************************
        PES1 = New EVO.ES.PES()

        'Schritte 1 - 3: ES wird initialisiert (Weiteres siehe dort ;-)
        '**************************************************************
        Call PES1.PesInitialise(Me.mySettings, Me.myProblem)

        'Verlaufsanzeige initialisieren
        Call Me.myProgress.Initialize(Me.mySettings.PES.Pop.N_Runden, Me.mySettings.PES.Pop.N_Popul, Me.mySettings.PES.N_Gen, Me.mySettings.PES.N_Nachf)

        durchlauf = 1

        'Über alle Runden
        'xxxxxxxxxxxxxxxx
        For PES1.PES_iAkt.iAktRunde = 0 To Me.mySettings.PES.Pop.N_Runden - 1

            Call PES1.EsResetPopBWSpeicher() 'Nur bei Komma Strategie

            'Über alle Populationen
            'xxxxxxxxxxxxxxxxxxxxxx
            For PES1.PES_iAkt.iAktPop = 0 To Me.mySettings.PES.Pop.N_Popul - 1

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

                        If (myAppType = ApplicationTypes.Testprobleme) Then

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

                                Dim n_Tries As Integer = 0

                                Do
                                    'Abbruchkriterium
                                    n_Tries += 1
                                    If (n_Tries > 10) Then
                                        Throw New Exception("Es konnte auch nach 10 Versuchen kein gültiger Datensatz erzeugt werden!")
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
                                        Throw New Exception("Es konnte auch nach 10 Versuchen kein gültiger Datensatz erzeugt werden!")
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

                    'SELEKTIONSPROZESS Schritt 2 für NDSorting sonst Xe = Xb
                    'Die neuen Eltern werden generiert
                    Call PES1.EsEltern()

                    'Sekundäre Population
                    '====================
                    If (Me.mySettings.PES.OptModus = Common.Constants.EVO_MODUS.Multi_Objective) Then

                        'SekPop abspeichern
                        '------------------
                        If (Not IsNothing(Sim1)) Then
                            Call Sim1.OptResult.setSekPop(PES1.SekundärQb, PES1.PES_iAkt.iAktGen)
                        End If

                        'SekPop zeichnen
                        '---------------
                        If (Not IsNothing(Sim1)) Then
                            'BUG 257: Umweg über Sim1.OptResult gehen, weil es im PES keine Individuum-IDs gibt
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
    Private Sub PES_getNewParameters(ByRef ind As EVO.Common.Individuum_PES)

        'REPRODUKTIONSPROZESS
        'Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern
        Call PES1.EsReproduktion()

        'MUTATIONSPROZESS
        'Mutieren der Ausgangswerte
        Call PES1.EsMutation()

        'Auslesen der variierten Parameter und in Individuum kopieren
        ind.OptParameter = EVO.Common.OptParameter.Clone_Array(PES1.EsGetParameter())

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

        'Verlauf aktualisieren
        Me.myProgress.iNachf = iNachfahre + 1

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
        If (Me.myProblem.Method <> METH_CES) Then

            With Me.myMonitor.Diag

                'Durchlaufachse (unten)
                .Axes.Bottom.Title.Caption = "Durchlauf"

                'Schrittweitenachse (links)
                .Axes.Left.Title.Caption = "Schrittweite"

            End With

            If (Me.mySettings.PES.SetMutation.IsDnVektor) Then

                'Bei PES-Schrittweitenvektor eine Linie für jeden Parameter
                ReDim Me.Line_Dn(Me.myProblem.List_OptParameter_Save.Length - 1)
                For i As Integer = 0 To Me.myProblem.List_OptParameter_Save.Length - 1
                    Me.Line_Dn(i) = Me.myMonitor.Diag.getSeriesLine("Schrittweite " & Me.myProblem.List_OptParameter_Save(i).Bezeichnung)
                Next
            Else
                'Ansonsten nur eine Linie
                ReDim Me.Line_Dn(0)
                Me.Line_Dn(0) = Me.myMonitor.Diag.getSeriesLine("Schrittweite", "Blue")
            End If

            'Linien formatieren
            For Each line As Steema.TeeChart.Styles.Line In Me.Line_Dn
                line.Pointer.Visible = True
                line.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                line.Pointer.HorizSize = 2
                line.Pointer.VertSize = 2
                line.Pointer.Pen.Visible = False
            Next

        End If

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
                .Axes.Right.Title.Caption = "Hypervolumen"
                .Axes.Right.Title.Angle = 90
                .Axes.Right.Automatic = True
                .Axes.Right.Grid.Visible = False

            End With

            'Linie
            Me.Line_Hypervolume = Me.myMonitor.Diag.getSeriesLine("Hypervolumen", "Red")
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

        Dim i, j As Integer

        Select Case Me.myProblem.Method
            Case METH_CES
                'CES: keine Schrittweite
                '-----------------------

            Case METH_HYBRID
                'HYBRID:
                '-------
                If (ind.OptParameter.Length > 0) Then
                    If (Me.mySettings.PES.SetMutation.IsDnVektor) Then
                        'Bei Schrittweitenvektor mehrere Linien
                        For i = 0 To ind.OptParameter.Length - 1
                            'Parameter zuordnen
                            For j = 0 To Me.myProblem.List_OptParameter_Save.Length - 1
                                If (ind.OptParameter(i).Bezeichnung = Me.myProblem.List_OptParameter_Save(j).Bezeichnung) Then
                                    Me.Line_Dn(j).Add(durchlauf, ind.OptParameter(i).Dn, durchlauf.ToString)
                                    Exit For
                                End If
                            Next
                        Next
                    Else
                        'Ansonsten nur eine Schrittweite
                        Me.Line_Dn(0).Add(durchlauf, ind.OptParameter(0).Dn, durchlauf.ToString)
                    End If
                End If

            Case METH_PES
                'PES
                '---
                If (Me.mySettings.PES.SetMutation.IsDnVektor) Then
                    'Bei Schrittweitenvektor mehrere Linien
                    For i = 0 To ind.OptParameter.Length - 1
                        Me.Line_Dn(i).Add(durchlauf, ind.OptParameter(i).Dn, durchlauf.ToString)
                    Next
                Else
                    'Ansonsten nur eine Schrittweite
                    Me.Line_Dn(0).Add(durchlauf, ind.OptParameter(0).Dn, durchlauf.ToString)
                End If

        End Select

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
            serie2 = Me.myHauptDiagramm.getSeriesPoint("Nadirpunkt", "Blue", Steema.TeeChart.Styles.PointerStyles.Diamond)
            serie2.Clear()
            serie2.Add(nadir(0), nadir(1), "Nadirpunkt")
        Else
            '3D
            '--
            Dim serie3 As Steema.TeeChart.Styles.Points3D
            serie3 = Me.myHauptDiagramm.getSeries3DPoint("Nadirpunkt", "Blue", Steema.TeeChart.Styles.PointerStyles.Diamond)
            serie3.Clear()
            serie3.Add(nadir(0), nadir(1), nadir(2), "Nadirpunkt")
        End If

    End Sub

#End Region 'Ausgabe

#End Region 'Methoden

End Class
