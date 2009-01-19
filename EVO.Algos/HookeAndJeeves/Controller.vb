Imports IHWB.EVO.Common.Constants

''' <summary>
''' Controller für Hooke And Jeeves
''' </summary>
Public Class Controller
    Implements EVO.IController

    Private myProblem As EVO.Common.Problem
    Private mySettings As EVO.Common.EVO_Settings
    Private myProgress As EVO.Common.Progress
    Private myMonitor As EVO.Diagramm.Monitor
    Private myHauptDiagramm As EVO.Diagramm.Hauptdiagramm

    Private myAppType As EVO.Common.ApplicationTypes
    Private WithEvents Sim1 As EVO.Apps.Sim
    Private Testprobleme1 As EVO.Apps.Testprobleme

#Region "Methoden"

#End Region

    ''' <summary>
    ''' Initialisiert den HJ-Controller und übergibt alle erforderlichen Objekte
    ''' </summary>
    ''' <param name="inputProblem"></param>
    ''' <param name="inputSettings"></param>
    ''' <param name="inputProgress"></param>
    ''' <param name="inputMonitor"></param>
    ''' <param name="inputHptDiagramm"></param>
    Public Sub Init(ByRef inputProblem As Common.Problem, ByRef inputSettings As Common.EVO_Settings, ByRef inputProgress As Common.Progress, ByRef inputMonitor As Diagramm.Monitor, ByRef inputHptDiagramm As Diagramm.Hauptdiagramm) Implements IController.Init

        Me.myProblem = inputProblem
        Me.mySettings = inputSettings
        Me.myProgress = inputProgress
        Me.myMonitor = inputMonitor
        Me.myHauptDiagramm = inputHptDiagramm

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
    ''' <remarks></remarks>
    Public Sub Start() Implements IController.Start

        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim b As Boolean
        Dim ind As Common.Individuum
        Dim QNBest() As Double
        Dim QBest() As Double
        Dim aktuellePara(Me.myProblem.NumParams - 1) As Double
        Dim SIM_Eval_is_OK As Boolean
        Dim durchlauf As Long
        Dim Iterationen As Long
        Dim Tastschritte_aktuell As Long
        Dim Tastschritte_gesamt As Long
        Dim Extrapolationsschritte As Long
        Dim Rueckschritte As Long

        Dim HookJeeves As New EVO.HookeAndJeeves.HookeAndJeeves(Me.myProblem.NumParams, Me.mySettings.HookJeeves.DnStart, Me.mySettings.HookJeeves.DnFinish)

        ReDim QNBest(Me.myProblem.NumPenalties - 1)
        ReDim QBest(Me.myProblem.NumPenalties - 1)

        durchlauf = 0
        Tastschritte_aktuell = 0
        Tastschritte_gesamt = 0
        Extrapolationsschritte = 0
        Rueckschritte = 0
        Iterationen = 0
        b = False

        Call HookJeeves.Initialize(Me.myProblem.List_OptParameter)

        'Initialisierungssimulation
        For i = 0 To Me.myProblem.NumParams - 1
            aktuellePara(i) = Me.myProblem.List_OptParameter(i).Xn
        Next
        QNBest(0) = 1.79E+308
        QBest(0) = 1.79E+308
        k = 0

        Do While (HookJeeves.AktuelleSchrittweite > HookJeeves.MinimaleSchrittweite)

            Iterationen += 1
            durchlauf += 1

            'Bestimmen der Ausgangsgüte
            '==========================
            'Individuum instanzieren
            ind = New Common.Individuum_PES("HJ", durchlauf)

            'HACK: OptParameter ins Individuum kopieren
            For i = 0 To ind.OptParameter.Length - 1
                ind.OptParameter(i).Xn = aktuellePara(i)
            Next

            'Evaluierung
            SIM_Eval_is_OK = Me.Sim1.Evaluate(ind)

            'TODO: Evaluierungsfehler behandeln

            'Lösung im TeeChart einzeichnen
            '------------------------------
            Dim serie As Steema.TeeChart.Styles.Series
            serie = Me.myHauptdiagramm.getSeriesPoint("Hook and Jeeves")
            Call serie.Add(durchlauf, ind.Penalties(0), durchlauf.ToString())

            Call System.Windows.Forms.Application.DoEvents()

            'Penalties in Bestwert kopieren
            Call ind.Penalties.CopyTo(QNBest, 0)

            'Tastschritte
            '============
            For j = 0 To HookJeeves.AnzahlParameter - 1

                aktuellePara = HookJeeves.Tastschritt(j, EVO.HookeAndJeeves.HookeAndJeeves.TastschrittRichtung.Vorwärts)

                Tastschritte_aktuell += 1
                durchlauf += 1

                'Monitor
                Call Me.myMonitor.AppendText("Tastschritte aktuell: " & Tastschritte_aktuell.ToString())

                'Individuum instanzieren
                ind = New Common.Individuum_PES("HJ", durchlauf)

                'HACK: OptParameter ins Individuum kopieren
                For i = 0 To ind.OptParameter.Length - 1
                    ind.OptParameter(i).Xn = aktuellePara(i)
                Next

                'Evaluierung
                SIM_Eval_is_OK = Me.Sim1.Evaluate(ind)

                'TODO: Evaluierungsfehler behandeln

                'Lösung im TeeChart einzeichnen
                '------------------------------
                serie = Me.myHauptdiagramm.getSeriesPoint("Hook and Jeeves")
                Call serie.Add(durchlauf, ind.Penalties(0), durchlauf.ToString())

                Call System.Windows.Forms.Application.DoEvents()

                If (ind.Penalties(0) >= QNBest(0)) Then

                    aktuellePara = HookJeeves.Tastschritt(j, EVO.HookeAndJeeves.HookeAndJeeves.TastschrittRichtung.Rückwärts)

                    Tastschritte_aktuell += 1
                    durchlauf += 1

                    'Monitor
                    Call Me.myMonitor.AppendText("Tastschritte aktuell: " & Tastschritte_aktuell.ToString())

                    'Individuum instanzieren
                    ind = New Common.Individuum_PES("HJ", durchlauf)

                    'HACK: OptParameter ins Individuum kopieren
                    For i = 0 To ind.OptParameter.Length - 1
                        ind.OptParameter(i).Xn = aktuellePara(i)
                    Next

                    'Evaluierung
                    SIM_Eval_is_OK = Me.Sim1.Evaluate(ind)

                    'TODO: Evaluierungsfehler behandeln

                    'Lösung im TeeChart einzeichnen
                    '------------------------------
                    serie = Me.myHauptdiagramm.getSeriesPoint("Hook and Jeeves")
                    Call serie.Add(durchlauf, ind.Penalties(0), durchlauf.ToString())

                    Call System.Windows.Forms.Application.DoEvents()

                    If (ind.Penalties(0) >= QNBest(0)) Then
                        aktuellePara = HookJeeves.TastschrittResetParameter(j)
                    Else
                        Call ind.Penalties.CopyTo(QNBest, 0)
                    End If
                Else
                    Call ind.Penalties.CopyTo(QNBest, 0)
                End If
            Next

            Tastschritte_gesamt += Tastschritte_aktuell
            Tastschritte_aktuell = 0

            'Monitor
            Call Me.myMonitor.AppendText("Tastschritte gesamt: " & Tastschritte_gesamt.ToString())
            Call Me.myMonitor.AppendText("Tastschritte aktuell: " & Tastschritte_aktuell.ToString())
            Call Me.myMonitor.AppendText("Tastschritte mittel: " & Math.Round((Tastschritte_gesamt / Iterationen), 2).ToString())

            'Extrapolationsschritt
            If (QNBest(0) < QBest(0)) Then

                'Lösung im TeeChart einzeichnen
                '------------------------------
                serie = Me.myHauptDiagramm.getSeriesPoint("Hook and Jeeves Best", "Green")
                Call serie.Add(durchlauf, ind.Penalties(0), durchlauf.ToString())

                Call System.Windows.Forms.Application.DoEvents()

                Call QNBest.CopyTo(QBest, 0)
                Call HookJeeves.Extrapolationsschritt()
                Extrapolationsschritte += 1

                'Monitor
                Call Me.myMonitor.AppendText("Extrapolationsschritte: " & Extrapolationsschritte.ToString())

                k += 1
                aktuellePara = HookJeeves.getLetzteParameter
                For i = 0 To HookJeeves.AnzahlParameter - 1
                    If aktuellePara(i) < 0 Or aktuellePara(i) > 1 Then
                        HookJeeves.Rueckschritt()
                        Rueckschritte += 1

                        'Monitor
                        Call Me.myMonitor.AppendText("Rückschritte: " & Rueckschritte.ToString())

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
                    Call Me.myMonitor.AppendText("Rückschritte: " & Rueckschritte.ToString())

                    HookJeeves.Schrittweitenhalbierung()
                    aktuellePara = HookJeeves.getLetzteParameter()
                Else
                    HookJeeves.Schrittweitenhalbierung()
                End If
            End If
        Loop
    End Sub

End Class
