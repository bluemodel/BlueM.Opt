Public Class TSPController
    Implements EVO.IController

    Private myHauptDiagramm As EVO.Diagramm.Hauptdiagramm
    Private myMonitor As EVO.Diagramm.Monitor
    Private myProblem As EVO.Common.Problem
    Private mySettings As EVO.Common.EVO_Settings
    Private myProgress As EVO.Common.Progress

    Private TSP1 As TSP

    ''' <summary>
    ''' TSP Controller initialisieren
    ''' </summary>
    ''' <param name="inputProblem"></param>
    ''' <param name="inputSettings"></param>
    ''' <param name="inputProgress"></param>
    ''' <param name="inputHptDiagramm"></param>
    ''' <remarks></remarks>
    Public Sub Init(ByRef inputProblem As Common.Problem, ByRef inputSettings As Common.EVO_Settings, ByRef inputProgress As Common.Progress, ByRef inputHptDiagramm As Diagramm.Hauptdiagramm) Implements IController.Init
        Me.myProblem = inputProblem
        Me.mySettings = inputSettings
        Me.myProgress = inputProgress
        Me.myHauptDiagramm = inputHptDiagramm
        Me.myMonitor = EVO.Diagramm.Monitor.getInstance()
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

        Call TSP1.TSP_Initialize()

        'Batch_Mode
        Dim Batch_Mode As Boolean = True
        'Anzahl der Tests
        Dim n As Integer = 3

        'Progress
        Me.myProgress.Initialize(0, 0, TSP1.n_Gen, TSP1.n_Childs)

        'Monitor Stuff
        With Me.myMonitor
            .SelectTabLog()
            .Show()
            .LogAppend("Cities: " & TSP1.n_Cities)
            .LogAppend("Combinations: " & TSP1.Faculty(TSP1.n_Cities) / 2)
            .LogAppend("Parents: " & TSP1.n_Parents)
            .LogAppend("Childs: " & TSP1.n_Childs)
            .LogAppend("Generations: " & TSP1.n_Gen)
            .LogAppend("Evaluations: " & TSP1.n_Childs * TSP1.n_Gen)
            If TSP1.Problem = TSP.EnProblem.circle Then
                .LogAppend("Quality Aim: " & Conversion.Int(TSP1.circumference))
            End If
        End With

        Select Case Batch_Mode

            Case False
                'Progress
                Me.myProgress.Initialize(0, 0, TSP1.n_Gen, TSP1.n_Childs)
                Call TSP_Controller(False)

            Case True
                'Progress
                Me.myProgress.Initialize(n, 8, TSP1.n_Gen, TSP1.n_Childs)
                Dim i, M, R As Integer

                For R = 1 To 2
                    TSP1.ReprodOperator = R

                    For M = 1 To 4
                        TSP1.MutOperator = M
                        Me.myMonitor.LogAppend("ReprodOperator: " & TSP1.ReprodOperator & "; MutationOperator: " & TSP1.MutOperator)

                        'n Wiederholungen
                        For i = 1 To n
                            Call TSP_Controller(True)
                        Next
                        '~~~~~~~~~~~~~~~~
                    Next
                Next
        End Select

    End Sub

    Public Sub Stoppen() Implements IController.Stoppen
        'TODO: TSP Stoppen
    End Sub

    Private Sub TSP_Controller(ByVal Batch_Mode As Boolean)

        'Laufvariable für die Generationen
        Dim gen As Integer
        Dim i As Integer
        Dim GoToExit As Boolean

        'Intervall zum Updaten des Diagramms
        Dim increm As Integer = 100
        Dim jepp As Integer = 0

		'Diagramm initialisieren
        Call Me.InitDiagramm()

        'Arrays werden Dimensioniert
        Call TSP1.Dim_Parents_TSP()
        Call TSP1.Dim_Childs()

        'Zufällige Kinderpfade werden generiert
        Call TSP1.Generate_Random_Path_TSP()

        'Generationsschleife
        For gen = 1 To TSP1.n_Gen

            'Den Kindern werden die Städte Ihres Pfades entsprechend zugewiesen
            Call TSP1.Cities_according_ChildPath()

            'Bestimmung des der Qualität der Kinder
            Call TSP1.Evaluate_child_Quality()

            'Sortieren der Kinden anhand der Qualität
            Call TSP1.Sort_Faksimile(TSP1.ChildList)

            'Selections Prozess (Übergabe der Kinder an die Eltern je nach Strategie)
            Call TSP1.Selection_Process()

            'Zeichnen des besten Elter
            If gen >= jepp Then
                Call Me.Zeichnen_TSP(TSP1.ParentList(0).Image)
                jepp += increm
                Me.myProgress.iGen() = gen
                If Batch_Mode = False Then
                    Me.myMonitor.LogAppend("Generation: " & gen & "; Quality: " & Conversion.Int(TSP1.ParentList(0).Penalty))
                End If
            End If

            'Fall die Problemstellung ein Kreis ist wird abgebrochen, wenn das Optimum erreicht ist
            If TSP1.Problem = TSP.EnProblem.circle And TSP1.ParentList(0).Penalty < TSP1.circumference Then
                GoToExit = True
                Select Case TSP1.ParentList(0).Path(0) < TSP1.ParentList(0).Path(1)
                    Case True
                        For i = 0 To TSP1.n_Cities - 2
                            If Not TSP1.ParentList(0).Path(i) + 1 = TSP1.ParentList(0).Path(i + 1) And Not (TSP1.ParentList(0).Path(i) = TSP1.n_Cities And TSP1.ParentList(0).Path(i + 1) = 1) Then
                                GoToExit = False
                                Exit For
                            End If
                        Next
                    Case Else
                        For i = 0 To TSP1.n_Cities - 2
                            If Not TSP1.ParentList(0).Path(i) - 1 = TSP1.ParentList(0).Path(i + 1) And Not (TSP1.ParentList(0).Path(i) = 1 And TSP1.ParentList(0).Path(i + 1) = TSP1.n_Cities) Then
                                GoToExit = False
                                Exit For
                            End If
                        Next
                End Select

            End If

            If GoToExit = True Then
                Exit For
            End If

            'Kinder werden Hier vollständig gelöscht
            Call TSP1.Reset_Childs()

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
            For i = 1 To TSP1.n_Cities
                Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
                Line1.Title = "Reisen"
                Line1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                Line1.Color = System.Drawing.Color.Blue
                Line1.Pointer.HorizSize = 3
                Line1.Pointer.VertSize = 3
            Next

        End With

        'Zeichnen der Punkte für die Städte
        For i = 0 To TSP1.n_Cities - 1
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
            For i = 1 To TSP1.n_Cities - 1
                .Series(i + 1).Clear()
                .Series(i + 1).Add(TmpListOfCities(i, 1), TmpListOfCities(i, 2), Drawing.Color.Blue)
                .Series(i).Add(TmpListOfCities(i, 1), TmpListOfCities(i, 2), Drawing.Color.Blue)
            Next

            'Zeichnen der Verbindung von der ersten bis zur letzten Stadt
            .Series(1).Add(TmpListOfCities(0, 1), TmpListOfCities(0, 2), "")
            .Series(TSP1.n_Cities).Add(TmpListOfCities(0, 1), TmpListOfCities(0, 2), "")

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
        For i = 0 To TSP1.n_Cities - 1
            Me.myHauptDiagramm.Series(0).Add(TmpListOfCities(i, 1), TmpListOfCities(i, 2), "")
        Next

    End Sub

#End Region 'Diagrammfunktionen

End Class
