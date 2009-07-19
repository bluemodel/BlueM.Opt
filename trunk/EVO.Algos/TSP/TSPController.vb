Public Class TSPController
    Implements EVO.IController

    Private myHauptDiagramm As EVO.Diagramm.Hauptdiagramm
    Private myMonitor As EVO.Diagramm.Monitor
    Private myProblem As EVO.Common.Problem
    Private mySettings As EVO.Common.EVO_Settings
    Private myProgress As EVO.Common.Progress

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
    Public Sub Init(ByRef inputProblem As Common.Problem, ByRef inputSettings As Common.EVO_Settings, ByRef inputProgress As Common.Progress, ByRef inputHptDiagramm As Diagramm.Hauptdiagramm) Implements IController.Init
        Me.myProblem = inputProblem
        Me.mySettings = inputSettings
        Me.myProgress = inputProgress
        Me.myHauptDiagramm = inputHptDiagramm
        Me.myMonitor = EVO.Diagramm.Monitor.getInstance()

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

        Call TSP1.TSP_Initialize(me.mySettings.TSP)
        Call InitDiagramm()
        Call Zeichnen_TSP_cities(TSP1.ListOfCities)

        'Batch_Mode
        Dim Batch_Mode As Boolean = True
        'Anzahl der Tests
        Dim n As Integer = 3

        'Progress
        Me.myProgress.Initialize(0, 0, me.mySettings.TSP.n_Gen, me.mySettings.TSP.n_Childs)

        'Monitor Stuff
        With Me.myMonitor
            .SelectTabLog()
            .Show()
            .LogAppend("Cities: " & me.mySettings.TSP.n_Cities)
            .LogAppend("Combinations: " & TSP1.n_Comb(Me.mySettings.TSP.n_Cities))
            .LogAppend("Parents: " & Me.mySettings.TSP.n_Parents)
            .LogAppend("Childs: " & Me.mySettings.TSP.n_Childs)
            .LogAppend("Generations: " & Me.mySettings.TSP.n_Gen)
            .LogAppend("Evaluations: " & Me.mySettings.TSP.n_Childs * Me.mySettings.TSP.n_Gen)
            If Me.mySettings.TSP.Problem = common.EnProblem.circle Then
                .LogAppend("Quality Aim: " & Conversion.Int(TSP1.circumference))
            End If
        End With

        Select Case TSP1.Mode

            Case TSP.EnMode.Standard_Opt
                'Progress
                Me.myProgress.Initialize(0, 0, Me.mySettings.TSP.n_Gen, Me.mySettings.TSP.n_Childs)
                Call TSP_Controller(False)

            Case TSP.EnMode.Batch_OPpt
                'Progress
                Me.myProgress.Initialize(n, 8, Me.mySettings.TSP.n_Gen, Me.mySettings.TSP.n_Childs)
                Dim i, M, R As Integer

                For R = 1 To 2
                    Me.mySettings.TSP.ReprodOperator = R

                    For M = 1 To 4
                        Me.mySettings.TSP.MutOperator = M
                        Me.myMonitor.LogAppend("ReprodOperator: " & Me.mySettings.TSP.ReprodOperator & "; MutationOperator: " & Me.mySettings.TSP.MutOperator)

                        'n Wiederholungen
                        For i = 1 To TSP1.nTests
                            Call TSP_Controller(True)
                        Next
                        '~~~~~~~~~~~~~~~~
                    Next
                Next
            Case TSP.EnMode.Just_Calc
                Me.mySettings.TSP.n_Childs = 1
                Dim i, j As Double

                j = 10000000

                Dim Time As New Stopwatch
                Call TSP1.Dim_Childs()
                Call TSP1.Generate_Random_Path_TSP()
                Call TSP1.Cities_according_ChildPath()

                Time.Start()
                For i = 1 To j
                    TSP1.Evaluate_child_Quality()
                Next
                Time.Stop()

                Me.myMonitor.LogAppend("Zahl der Berechnungen: " & j)
                Me.myMonitor.LogAppend("Die Berechnung dauerte:   " & Time.Elapsed.Hours & "h  " & Time.Elapsed.Minutes & "m  " & Time.Elapsed.Seconds & "s     " & Time.Elapsed.Milliseconds & "ms")

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
        Call TSP1.Dim_Childs()

        'Zufällige Kinderpfade werden generiert
        Call TSP1.Generate_Random_Path_TSP()

        'Generationsschleife
        For gen = 1 To me.mySettings.TSP.n_Gen

            'Den Kindern werden die Städte Ihres Pfades entsprechend zugewiesen
            Call TSP1.Cities_according_ChildPath()

            'Zeichnen des ersten Childs
            If gen = 1 Then
                Call Zeichnen_TSP(TSP1.ChildList(0).Image)
                Me.myHauptDiagramm.Update()
                'png Export
                If TSP1.pngExport = True Then
                    Me.myHauptDiagramm.Export.Image.PNG.Save(TSP1.ExPath & gen.ToString.PadLeft(7, "0") & " Qualität " & Conversion.Int(TSP1.ChildList(0).Penalty).ToString.PadLeft(5, "0") & ".png")
                End If
            End If

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
                    Me.myMonitor.LogAppend("Gen.: " & gen & "; Länge: " & Conversion.Int(TSP1.ParentList(0).Penalty) & "; Faktor: " _
                    & math.Round(TSP1.ParentList(0).Penalty/TSP1.circumference, 3,MidpointRounding.ToEven))
                    'png Export
                    If TSP1.pngExport = True and Conversion.Int(TSP1.ParentList(0).Penalty) < PenaltyTMP Then
                        Me.myHauptDiagramm.Export.Image.PNG.Save(TSP1.ExPath & gen.ToString.PadLeft(7, "0") & " Qualität " & Conversion.Int(TSP1.ParentList(0).Penalty).ToString.PadLeft(5, "0") & ".png")
                        PenaltyTMP = Conversion.Int(TSP1.ParentList(0).Penalty)
                    End If
                End If
            End If

            'Fall die Problemstellung ein Kreis ist wird abgebrochen, wenn das Optimum erreicht ist
            If me.mySettings.TSP.Problem = common.EnProblem.circle And TSP1.ParentList(0).Penalty < TSP1.circumference Then
                GoToExit = True
                Select Case TSP1.ParentList(0).Path(0) < TSP1.ParentList(0).Path(1)
                    Case True
                        For i = 0 To me.mySettings.TSP.n_Cities - 2
                            If Not TSP1.ParentList(0).Path(i) + 1 = TSP1.ParentList(0).Path(i + 1) And Not (TSP1.ParentList(0).Path(i) = me.mySettings.TSP.n_Cities And TSP1.ParentList(0).Path(i + 1) = 1) Then
                                GoToExit = False
                                Exit For
                            End If
                        Next
                    Case Else
                        For i = 0 To me.mySettings.TSP.n_Cities - 2
                            If Not TSP1.ParentList(0).Path(i) - 1 = TSP1.ParentList(0).Path(i + 1) And Not (TSP1.ParentList(0).Path(i) = 1 And TSP1.ParentList(0).Path(i + 1) = me.mySettings.TSP.n_Cities) Then
                                GoToExit = False
                                Exit For
                            End If
                        Next
                End Select

            End If

            If GoToExit = True or Stopp Then
                If Batch_Mode = False Then
                    Call Zeichnen_TSP(TSP1.ParentList(0).Image)
                    Me.myHauptDiagramm.Update()
                    Me.myMonitor.LogAppend("Gen.: " & gen & "; Länge: " & Conversion.Int(TSP1.ParentList(0).Penalty) & "; Faktor: " _
                    & math.Round(TSP1.ParentList(0).Penalty/TSP1.circumference, 3,MidpointRounding.ToEven))
                    'png Export
                    If TSP1.pngExport = True Then
                        Me.myHauptdiagramm.Export.Image.PNG.Save(TSP1.ExPath & gen.ToString.PadLeft(7, "0") & " Qualität " & Conversion.Int(TSP1.ParentList(0).Penalty).ToString.PadLeft(5, "0") & ".png")
                    End If
                End If
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
            For i = 1 To me.mySettings.TSP.n_Cities
                Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
                Line1.Title = "Reisen"
                Line1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                Line1.Color = System.Drawing.Color.Blue
                Line1.Pointer.HorizSize = 3
                Line1.Pointer.VertSize = 3
            Next

        End With

        'Zeichnen der Punkte für die Städte
        For i = 0 To me.mySettings.TSP.n_Cities - 1
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
            For i = 1 To me.mySettings.TSP.n_Cities - 1
                .Series(i + 1).Clear()
                .Series(i + 1).Add(TmpListOfCities(i, 1), TmpListOfCities(i, 2), Drawing.Color.Blue)
                .Series(i).Add(TmpListOfCities(i, 1), TmpListOfCities(i, 2), Drawing.Color.Blue)
            Next

            'Zeichnen der Verbindung von der ersten bis zur letzten Stadt
            .Series(1).Add(TmpListOfCities(0, 1), TmpListOfCities(0, 2), "")
            .Series(me.mySettings.TSP.n_Cities).Add(TmpListOfCities(0, 1), TmpListOfCities(0, 2), "")

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
        For i = 0 To me.mySettings.TSP.n_Cities - 1
            Me.myHauptDiagramm.Series(0).Add(TmpListOfCities(i, 1), TmpListOfCities(i, 2), "")
        Next

    End Sub

#End Region 'Diagrammfunktionen

End Class
