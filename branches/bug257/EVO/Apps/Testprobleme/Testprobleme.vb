Imports IHWB.EVO.Kern

Partial Public Class Testprobleme
    Inherits System.Windows.Forms.Form

    'Eigenschaften
    '#############

    Private IsInitializing As Boolean
    Public OptModus As EVO_MODUS

    'Properties
    '##########
    Public ReadOnly Property AnzParameter() As Integer
        Get
            Return Convert.ToInt32(Me.TextBox_Einstellung.Value)
        End Get
    End Property

#Region "Form behavior"

    'Form laden
    '**********
    Private Sub Testprobleme_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Combobox füllen
        Combo_Testproblem.Items.Add("Sinus-Funktion")
        Combo_Testproblem.Items.Add("Beale-Problem")
        Combo_Testproblem.Items.Add("Schwefel 2.4-Problem")
        Combo_Testproblem.Items.Add("Deb 1")
        Combo_Testproblem.Items.Add("Zitzler/Deb T1")
        Combo_Testproblem.Items.Add("Zitzler/Deb T2")
        Combo_Testproblem.Items.Add("Zitzler/Deb T3")
        Combo_Testproblem.Items.Add("Zitzler/Deb T4")
        Combo_Testproblem.Items.Add("CONSTR")
        Combo_Testproblem.Items.Add("Box")
        Combo_Testproblem.Items.Add("Abhängige Parameter")

        Combo_Testproblem.SelectedIndex = 0

        'Ende der Initialisierung
        IsInitializing = False

        'Eventhandler einrichten
        AddHandler Me.Combo_Testproblem.SelectedIndexChanged, AddressOf Form1.INI_App

    End Sub

    'Auswahl eines neuen Testproblems
    '********************************
    Private Sub Combo_Testproblem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Combo_Testproblem.SelectedIndexChanged

        If IsInitializing = True Then
            Exit Sub
        Else
            'zusätzliche Einstellungen erstmal ausblenden
            Me.Label_Einstellung.Visible = False
            Me.TextBox_Einstellung.Visible = False

            Select Case Combo_Testproblem.Text

                Case "Sinus-Funktion"
                    Me.Label_Beschreibungstext.Text = "Parameter an Sinusfunktion anpassen"
                    Me.Label_Einstellung.Visible = True
                    Me.Label_Einstellung.Text = "Anzahl Parameter:"
                    Me.TextBox_Einstellung.Visible = True
                    Me.TextBox_Einstellung.Text = "50"
                    OptModus = EVO_MODUS.Single_Objective

                Case "Beale-Problem"
                    Me.Label_Beschreibungstext.Text = "Es wird das Minimum des Beale-Problems gesucht (x=(3, 0.5), F(x)=0)"
                    OptModus = EVO_MODUS.Single_Objective

                Case "Schwefel 2.4-Problem"
                    Me.Label_Beschreibungstext.Text = "Minimum der Problemstellung wird gesucht (xi=1, F(x)=0)"
                    Me.Label_Einstellung.Visible = True
                    Me.Label_Einstellung.Text = "Anzahl Parameter:"
                    Me.TextBox_Einstellung.Visible = True
                    Me.TextBox_Einstellung.Text = "5"
                    OptModus = EVO_MODUS.Single_Objective

                Case "Deb 1"
                    Me.Label_Beschreibungstext.Text = "Multikriterielles Testproblem (konvex)"
                    OptModus = EVO_MODUS.Multi_Objective

                Case "Zitzler/Deb T1"
                    Me.Label_Beschreibungstext.Text = "Multikriterielles Testproblem (konvex)"
                    OptModus = EVO_MODUS.Multi_Objective

                Case "Zitzler/Deb T2"
                    Me.Label_Beschreibungstext.Text = "Multikriterielles Testproblem (konkav)"
                    OptModus = EVO_MODUS.Multi_Objective

                Case "Zitzler/Deb T3"
                    Me.Label_Beschreibungstext.Text = "Multikriterielles Testproblem (konvex, nicht stetig)"
                    OptModus = EVO_MODUS.Multi_Objective

                Case "Zitzler/Deb T4"
                    Me.Label_Beschreibungstext.Text = "Multikriterielles Testproblem (konvex)"
                    OptModus = EVO_MODUS.Multi_Objective

                Case "CONSTR"
                    Me.Label_Beschreibungstext.Text = "Multikriterielles Testproblem (konvex) mit zwei Randbedingungen"
                    OptModus = EVO_MODUS.Multi_Objective

                Case "Box"
                    Me.Label_Beschreibungstext.Text = "Multikriterielles Testproblem (Kreis) mit zwei Randbedingungen"
                    OptModus = EVO_MODUS.Multi_Objective

                Case "Abhängige Parameter"
                    Me.Label_Beschreibungstext.Text = "Bedingung: Y > X"
                    OptModus = EVO_MODUS.Single_Objective

            End Select

        End If
    End Sub

    'Form schließen
    '**************
    Private Sub Testprobleme_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        'verhindern, dass das Formular komplett gelöscht wird
        e.Cancel = True

        'Formular verstecken
        Call Me.Hide()

    End Sub


#End Region 'Form behavior

    'Parameterübergabe
    '*****************
    Public Sub Parameter_Uebergabe(ByRef globalAnzPar As Short, ByRef globalAnzZiel As Short, ByRef globalAnzRand As Short, ByRef mypara() As Double, ByRef beziehungen() As EVO.Kern.PES.Beziehung)

        Dim i As Integer

        Select Case Me.Combo_Testproblem.Text

            Case "Sinus-Funktion"
                globalAnzPar = Me.AnzParameter
                globalAnzZiel = 1
                globalAnzRand = 0
                ReDim mypara(globalAnzPar - 1)
                ReDim beziehungen(globalAnzPar - 1)
                For i = 0 To globalAnzPar - 1
                    mypara(i) = 0
                    beziehungen(i) = PES.Beziehung.keine
                Next

            Case "Beale-Problem" 'x1 = [-5;5], x2=[-2;2]
                globalAnzPar = 2
                globalAnzZiel = 1
                globalAnzRand = 0
                ReDim mypara(globalAnzPar - 1)
                ReDim beziehungen(globalAnzPar - 1)
                For i = 0 To globalAnzPar - 1
                    mypara(i) = 0.5
                    beziehungen(i) = PES.Beziehung.keine
                Next

            Case "Schwefel 2.4-Problem" 'xi = [-10,10]
                globalAnzPar = Me.AnzParameter
                globalAnzZiel = 1
                globalAnzRand = 0
                ReDim mypara(globalAnzPar - 1)
                ReDim beziehungen(globalAnzPar - 1)
                For i = 0 To globalAnzPar - 1
                    mypara(i) = 1
                    beziehungen(i) = PES.Beziehung.keine
                Next

            Case "Deb 1" 'x1 = [0.1;1], x2=[0;5]
                globalAnzPar = 2
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar - 1)
                ReDim beziehungen(globalAnzPar - 1)
                Randomize()
                For i = 0 To globalAnzPar - 1
                    mypara(i) = Rnd()
                    beziehungen(i) = PES.Beziehung.keine
                Next

            Case "Zitzler/Deb T1" 'xi = [0,1]
                globalAnzPar = 30
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar - 1)
                ReDim beziehungen(globalAnzPar - 1)
                Randomize()
                For i = 0 To globalAnzPar - 1
                    mypara(i) = Rnd()
                    beziehungen(i) = PES.Beziehung.keine
                Next

            Case "Zitzler/Deb T2" 'xi = [0,1]
                globalAnzPar = 30
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar - 1)
                ReDim beziehungen(globalAnzPar - 1)
                Randomize()
                For i = 0 To globalAnzPar - 1
                    mypara(i) = Rnd()
                    beziehungen(i) = PES.Beziehung.keine
                Next

            Case "Zitzler/Deb T3" 'xi = [0,1]
                globalAnzPar = 15
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar - 1)
                ReDim beziehungen(globalAnzPar - 1)
                Randomize()
                For i = 0 To globalAnzPar - 1
                    mypara(i) = Rnd()
                    beziehungen(i) = PES.Beziehung.keine
                Next

            Case "Zitzler/Deb T4" 'x1 = [0,1], xi=[-5,5]
                globalAnzPar = 10
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar - 1)
                ReDim beziehungen(globalAnzPar - 1)
                Randomize()
                For i = 0 To globalAnzPar - 1
                    mypara(i) = Rnd()
                    beziehungen(i) = PES.Beziehung.keine
                Next

            Case "CONSTR" 'x1 = [0.1;1], x2=[0;5]
                globalAnzPar = 2
                globalAnzZiel = 2
                globalAnzRand = 2
                ReDim mypara(globalAnzPar - 1)
                ReDim beziehungen(globalAnzPar - 1)
                Randomize()
                For i = 0 To globalAnzPar - 1
                    mypara(i) = Rnd()
                    beziehungen(i) = PES.Beziehung.keine
                Next

            Case "Box"
                globalAnzPar = 3
                globalAnzZiel = 3
                globalAnzRand = 2
                ReDim mypara(globalAnzPar - 1)
                ReDim beziehungen(globalAnzPar - 1)
                Randomize()
                For i = 0 To globalAnzPar - 1
                    mypara(i) = Rnd()
                    beziehungen(i) = PES.Beziehung.keine
                Next

            Case "Abhängige Parameter"
                globalAnzPar = 2
                globalAnzZiel = 1
                globalAnzRand = 0
                ReDim mypara(globalAnzPar - 1)
                For i = 0 To globalAnzPar - 1
                    mypara(i) = 1
                Next
                'Beziehungen
                ReDim beziehungen(globalAnzPar - 1)
                beziehungen(0) = EVO.Kern.PES.Beziehung.keine
                beziehungen(1) = EVO.Kern.PES.Beziehung.groesser

        End Select

    End Sub

#Region "Diagrammfunktionen"

    'Diagramm initialisieren
    '***********************
    Public Sub DiagInitialise(ByVal PES_Settings As EVO.Kern.EVO_Settings, ByVal globalAnzPar As Integer, ByRef Diag As EVO.Diagramm)

        Select Case Me.Combo_Testproblem.Text

            Case "Sinus-Funktion"
                Call Me.DiagInitialise_SinusFunktion(globalAnzPar, Diag)

            Case "Beale-Problem" 'x1 = [-5;5], x2=[-2;2]
                Call Me.DiagInitialise_BealeProblem(PES_Settings, globalAnzPar, Diag)

            Case "Schwefel 2.4-Problem" 'xi = [-10,10]
                Call Me.DiagInitialise_SchwefelProblem(PES_Settings, globalAnzPar, Diag)

            Case "Box"
                Call Me.DiagInitialise_3D_Box(PES_Settings, globalAnzPar, Diag)

            Case "Abhängige Parameter"
                Call Me.DiagInitialise_AbhParameter(PES_Settings, globalAnzPar, Diag)

            Case Else
                Call Me.DiagInitialise_MultiTestProb(PES_Settings, Diag)

        End Select

    End Sub

    'Diagramm für Sinus-Funktion initialisieren
    '*******************************************
    Private Sub DiagInitialise_SinusFunktion(ByVal globalAnzPar As Short, ByRef Diag As EVO.Diagramm)

        Dim array_x() As Double = {}
        Dim array_y() As Double = {}
        Dim i As Short
        Dim Unterteilung_X As Double
        Dim serie As Steema.TeeChart.Styles.Series

        'TeeChart Einrichten und Series generieren
        With Diag
            .Clear()
            .Header.Text = "Sinus Funktion"
            .Chart.Axes.Left.Title.Caption = "Y-Wert"
            .Chart.Axes.Bottom.Title.Caption = "X-Wert"
            .Aspect.View3D = False
            .Legend.Visible = False

            'Axen Formatieren
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Maximum = 2 * Math.PI
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Bottom.Increment = Math.PI
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Minimum = -1
            .Chart.Axes.Left.Maximum = 1
            .Chart.Axes.Left.Increment = 0.2

            'Sinuslinie zeichnen
            Unterteilung_X = 2 * Math.PI / (globalAnzPar - 1)

            ReDim array_x(globalAnzPar - 1)
            ReDim array_y(globalAnzPar - 1)

            For i = 0 To globalAnzPar - 1
                array_x(i) = Math.Round(i * Unterteilung_X, 2)
                array_y(i) = Math.Sin(i * Unterteilung_X)
            Next i

            serie = .getSeriesLine("Sinusfunktion", "Green")
            serie.Add(array_x, array_y)

        End With
    End Sub

    'Diagramm für Beale-Problem initialisieren
    '*****************************************
    Private Sub DiagInitialise_BealeProblem(ByVal PES_Settings As EVO.Kern.EVO_Settings, ByVal globalAnzPar As Short, ByRef Diag As EVO.Diagramm)

        Dim array_x() As Double = {}
        Dim array_y() As Double = {}
        Dim Ausgangswert As Double
        Dim Anzahl_Kalkulationen As Integer
        Dim i As Short
        Dim serie As Steema.TeeChart.Styles.Series

        If (PES_Settings.PES.Pop.is_POPUL) Then
            Anzahl_Kalkulationen = PES_Settings.PES.n_Gen * PES_Settings.PES.n_Nachf * PES_Settings.PES.Pop.n_Runden + 1
        Else
            Anzahl_Kalkulationen = PES_Settings.PES.n_Gen * PES_Settings.PES.n_Nachf + 1
        End If

        'Ausgangswert berechnen
        Ausgangswert = (1.5 - 0.5 * (1 - 0.5)) ^ 2 + (2.25 - 0.5 * (1 - 0.5) ^ 2) ^ 2 + (2.625 - 0.5 * (1 - 0.5) ^ 3) ^ 2

        'TeeChart Einrichten und Linien zeichnen
        With Diag
            .Clear()
            .Header.Text = "Beale Problem"
            .Chart.Axes.Left.Title.Caption = "Funktionswert"
            .Chart.Axes.Bottom.Title.Caption = "Berechnungsschritt"
            .Aspect.View3D = False
            .Legend.Visible = False

            'Axen Formatieren
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Maximum = Anzahl_Kalkulationen
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Maximum = Ausgangswert * 1.3
            .Chart.Axes.Left.Minimum = 0

            'Linie für den Ausgangswert berechnen
            ReDim array_y(Anzahl_Kalkulationen - 1)
            ReDim array_x(Anzahl_Kalkulationen - 1)
            For i = 0 To Anzahl_Kalkulationen - 1
                array_y(i) = Ausgangswert
                array_x(i) = i + 1
            Next i

            'Den Ausgangswert zeichnen
            serie = .getSeriesLine("Ausgangswert", "Green")
            serie.Add(array_x, array_y)

        End With
    End Sub

    'Diagramm für Schwefel-Problem initialisieren
    '********************************************
    Private Sub DiagInitialise_SchwefelProblem(ByVal PES_Settings As EVO.Kern.EVO_Settings, ByVal globalAnzPar As Short, ByRef Diag As EVO.Diagramm)

        Dim array_x() As Double = {}
        Dim array_y() As Double = {}
        Dim Ausgangswert As Double
        Dim Anzahl_Kalkulationen As Integer
        Dim i As Short
        Dim X() As Double
        Dim serie As Steema.TeeChart.Styles.Series

        If (PES_Settings.PES.Pop.is_POPUL) Then
            Anzahl_Kalkulationen = PES_Settings.PES.n_Gen * PES_Settings.PES.n_Nachf * PES_Settings.PES.Pop.n_Runden + 1
        Else
            Anzahl_Kalkulationen = PES_Settings.PES.n_Gen * PES_Settings.PES.n_Nachf + 1
        End If

        'Ausgangswert berechnen
        ReDim X(globalAnzPar)
        For i = 1 To globalAnzPar
            X(i) = 10
        Next i
        Ausgangswert = 0
        For i = 1 To globalAnzPar
            Ausgangswert += ((X(1) - X(i) ^ 2) ^ 2 + (X(i) - 1) ^ 2)
        Next i

        'Linie für den Ausgangswert berechnen
        ReDim array_y(Anzahl_Kalkulationen - 1)
        ReDim array_x(Anzahl_Kalkulationen - 1)
        For i = 0 To Anzahl_Kalkulationen - 1
            array_y(i) = Ausgangswert
            array_x(i) = i + 1
        Next i

        'TeeChart Einrichten und Series generieren
        With Diag
            .Clear()
            .Header.Text = "Schwefel 2.4 Problem"
            .Chart.Axes.Left.Title.Caption = "Funktionswert"
            .Chart.Axes.Bottom.Title.Caption = "Berechnungsschritt"
            .Aspect.View3D = False
            .Legend.Visible = False

            'Axen Formatieren
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Maximum = Anzahl_Kalkulationen
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Maximum = Ausgangswert * 1.3
            .Chart.Axes.Left.Minimum = 0
            .Chart.Axes.Left.Logarithmic = False

            'Ausgangswert zeichnen
            serie = .getSeriesLine("Ausgangswert", "Red")
            serie.Add(array_x, array_y)

        End With

    End Sub

    'Diagramm für MultiObjective-Probleme initialisieren
    '***************************************************
    Private Sub DiagInitialise_MultiTestProb(ByVal PES_Settings As EVO.Kern.EVO_Settings, ByRef Diag As EVO.Diagramm)

        Dim i, j As Short
        Dim serie As Steema.TeeChart.Styles.Series

        With Diag
            .Clear()
            .Aspect.View3D = False
            .Legend.Visible = False
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Maximum = 1
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Bottom.Increment = 0.1
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Maximum = 10
            .Chart.Axes.Left.Minimum = 0
            .Chart.Axes.Left.Increment = 2
        End With

        Select Case Me.Combo_Testproblem.Text

            Case "Deb 1"
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim Array1X(100) As Double
                Dim Array1Y(100) As Double
                Dim Array2X(100) As Double
                Dim Array2Y(100) As Double
                Diag.Header.Text = "Deb D1 - MO-konvex"

                'Paretofront berechnen und zeichnen
                For j = 0 To 100
                    Array1X(j) = 0.1 + j * 0.009
                    Array1Y(j) = 1 / Array1X(j)
                Next j
                serie = Diag.getSeriesLine("Paretofront", "Green")
                serie.Add(Array1X, Array1Y)

                'Linie 2 berechnen und zeichnen
                For j = 0 To 100
                    Array2X(j) = 0.1 + j * 0.009
                    Array2Y(j) = (1 + 5) / Array2X(j)
                Next j
                serie = Diag.getSeriesLine("Linie 2", "Red")
                serie.Add(Array2X, Array2Y)


            Case "Zitzler/Deb T1"
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim ArrayX(1000) As Double
                Dim ArrayY(1000) As Double
                Diag.Header.Text = "Zitzler/Deb/Theile T1"
                Diag.Chart.Axes.Left.Maximum = 7
                Diag.Chart.Axes.Left.Increment = 0.5

                'Paretofront berechnen und zeichnen
                For j = 0 To 1000
                    ArrayX(j) = j / 1000
                    ArrayY(j) = 1 - Math.Sqrt(ArrayX(j))
                Next j
                serie = Diag.getSeriesLine("Paretofront", "Green")
                serie.Add(ArrayX, ArrayY)


            Case "Zitzler/Deb T2"
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim ArrayX(100) As Double
                Dim ArrayY(100) As Double
                Diag.Header.Text = "Zitzler/Deb/Theile T2"
                Diag.Chart.Axes.Left.Maximum = 7

                'Paretofront berechnen und zeichnen
                For j = 0 To 100
                    ArrayX(j) = j / 100
                    ArrayY(j) = 1 - (ArrayX(j) * ArrayX(j))
                Next j
                serie = Diag.getSeriesLine("Paretofront", "Green")
                serie.Add(ArrayX, ArrayY)


            Case "Zitzler/Deb T3"
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                'TODO: Titel der Serien (für Export)
                Dim ArrayX(100) As Double
                Dim ArrayY(100) As Double
                Diag.Header.Text = "Zitzler/Deb/Theile T3"
                Diag.Chart.Axes.Bottom.Increment = 0.2
                Diag.Chart.Axes.Left.Maximum = 7
                Diag.Chart.Axes.Left.Minimum = -1
                Diag.Chart.Axes.Left.Increment = 0.5

                'Paretofront berechnen und zeichnen
                For j = 0 To 100
                    ArrayX(j) = j / 100
                    ArrayY(j) = 1 - Math.Sqrt(ArrayX(j)) - ArrayX(j) * Math.Sin(10 * Math.PI * ArrayX(j))
                Next j
                serie = Diag.getSeriesLine("Paretofront", "Green")
                serie.Add(ArrayX, ArrayY)


            Case "Zitzler/Deb T4"
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim ArrayX(1000) As Double
                Dim ArrayY(1000) As Double
                Diag.Header.Text = "Zitzler/Deb/Theile T4"
                Diag.Chart.Axes.Bottom.Automatic = True
                Diag.Chart.Axes.Left.Automatic = True

                'Lokale Optima berechnen und zeichnen
                For i = 1 To 10
                    For j = 0 To 1000
                        ArrayX(j) = j / 1000
                        ArrayY(j) = (1 + (i - 1) / 4) * (1 - Math.Sqrt(ArrayX(j) / (1 + (i - 1) / 4)))
                    Next
                    serie = Diag.getSeriesLine("Lokales Optimum " & i)
                    serie.Add(ArrayX, ArrayY)
                Next


            Case "CONSTR"
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim Array1X(100) As Double
                Dim Array1Y(100) As Double
                Dim Array2X(100) As Double
                Dim Array2Y(100) As Double
                Dim Array3X(61) As Double
                Dim Array3Y(61) As Double
                Dim Array4X(61) As Double
                Dim Array4Y(61) As Double
                Diag.Header.Text = "CONSTR"

                'Grenze 1 berechnen und zeichnen
                For j = 0 To 100
                    Array1X(j) = 0.1 + j * 0.009
                    Array1Y(j) = 1 / Array1X(j)
                Next j
                serie = Diag.getSeriesLine("Grenze 1", "Red")
                serie.Add(Array1X, Array1Y)

                'Grenze 2 berechnen und zeichnen
                For j = 0 To 100
                    Array2X(j) = 0.1 + j * 0.009
                    Array2Y(j) = (1 + 5) / Array2X(j)
                Next j
                serie = Diag.getSeriesLine("Grenze 2", "Red")
                serie.Add(Array2X, Array2Y)

                'Grenze 3 berechnen und zeichnen
                ReDim Array3X(61)
                ReDim Array3Y(61)
                For j = 0 To 61
                    Array3X(j) = 0.1 + (j + 2) * 0.009
                    Array3Y(j) = (7 - 9 * Array3X(j)) / Array3X(j)
                Next j
                serie = Diag.getSeriesLine("Grenze 3", "Blue")
                serie.Add(Array3X, Array3Y)

                'Grenze 4 berechnen und zeichnen
                ReDim Array4X(61)
                ReDim Array4Y(61)
                For j = 0 To 61
                    Array4X(j) = 0.1 + (j + 2) * 0.009
                    Array4Y(j) = (9 * Array4X(j)) / Array4X(j)
                Next j
                serie = Diag.getSeriesLine("Grenze 4", "Red")
                serie.Add(Array4X, Array4Y)

        End Select

    End Sub

    'Diagramm für Box-Problem (3D) initialisieren
    '********************************************
    Private Sub DiagInitialise_3D_Box(ByVal PES_Settings As EVO.Kern.EVO_Settings, ByVal AnzPar As Integer, ByRef Diag As EVO.Diagramm)

        Dim i, j, n As Integer
        Dim ArrayX() As Double
        Dim ArrayY() As Double
        Dim ArrayZ() As Double

        With Diag
            .Clear()
            .Header.Text = "Box"
            .Legend.Visible = False
            .Aspect.View3D = True
            .Aspect.Chart3DPercent = 90
            .Aspect.Elevation = 348
            .Aspect.Orthogonal = False
            .Aspect.Perspective = 62
            .Aspect.Rotation = 329
            .Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality
            .Aspect.VertOffset = -20
            .Aspect.Zoom = 66
            .Tools.Add(New Steema.TeeChart.Tools.Rotate())

            'Achsen:
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Visible = True
            .Chart.Axes.Bottom.Title.Caption = "X"
            .Chart.Axes.Bottom.Maximum = 1
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Bottom.Increment = 0.2

            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Visible = True
            .Chart.Axes.Left.Title.Caption = "Y"
            .Chart.Axes.Left.Maximum = 1
            .Chart.Axes.Left.Minimum = 0
            .Chart.Axes.Left.Increment = 0.2

            .Chart.Axes.Depth.Automatic = False
            .Chart.Axes.Depth.Visible = True
            .Chart.Axes.Depth.Title.Caption = "Z"
            .Chart.Axes.Depth.Maximum = 1
            .Chart.Axes.Depth.Minimum = 0
            .Chart.Axes.Depth.Increment = 0.2

            'Serien
            '-----------
            Dim surface As Steema.TeeChart.Styles.Surface
            Dim series3D As Steema.TeeChart.Styles.Points3D

            'Constraint 1
            'x + y + z <= 0.8
            Dim surfaceRes As Integer = 11
            ReDim ArrayX(surfaceRes ^ 2 - 1)
            ReDim ArrayY(surfaceRes ^ 2 - 1)
            ReDim ArrayZ(surfaceRes ^ 2 - 1)

            n = 0
            For i = 0 To surfaceRes - 1
                For j = 0 To (surfaceRes - 1)
                    ArrayX(n) = i * (1.1 / surfaceRes)
                    ArrayZ(n) = j * (1.1 / surfaceRes)
                    ArrayY(n) = Math.Max(0.8 - ArrayX(n) - ArrayZ(n), 0)
                    n += 1
                Next
            Next

            surface = New Steema.TeeChart.Styles.Surface(Diag.Chart)
            surface.Title = "Constraint 1"
            surface.IrregularGrid = True
            surface.NumXValues = surfaceRes
            surface.NumZValues = surfaceRes
            surface.Add(ArrayX, ArrayY, ArrayZ)
            surface.UseColorRange = False
            surface.UsePalette = False
            surface.Brush.Solid = True
            surface.Brush.Color = Color.Green
            surface.Brush.Transparency = 70
            surface.Pen.Color = Color.Green
            surface.SideBrush.Visible = True
            surface.SideBrush.Color = Color.Red
            surface.SideBrush.Transparency = 70

            'Constraint 2
            'x + y <= 0.5
            ReDim ArrayX(65)
            ReDim ArrayY(65)
            ReDim ArrayZ(65)

            n = 0
            For i = 0 To 10
                For j = 0 To 5
                    ArrayX(n) = j * 0.1
                    ArrayZ(n) = i * 0.1
                    ArrayY(n) = 0.5 - ArrayX(n)
                    n += 1
                Next
            Next

            surface = New Steema.TeeChart.Styles.Surface(Diag.Chart)
            surface.Title = "Constraint 2"
            surface.IrregularGrid = True
            surface.NumXValues = 10
            surface.NumZValues = 10
            surface.Add(ArrayX, ArrayY, ArrayZ)
            surface.UseColorRange = False
            surface.UsePalette = False
            surface.Brush.Solid = True
            surface.Brush.Color = Color.Blue
            surface.Brush.Transparency = 70
            surface.Pen.Color = Color.Blue
            surface.SideBrush.Visible = True
            surface.SideBrush.Color = Color.Red
            surface.SideBrush.Transparency = 70

            'Schnittgerade zwischen den Constraints
            series3D = New Steema.TeeChart.Styles.Points3D(Diag.Chart)
            series3D.Title = "Schnittgerade"
            series3D.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Nothing
            series3D.LinePen.Visible = True
            series3D.LinePen.Width = 1
            series3D.LinePen.Color = Color.Red
            series3D.Add(0.5, 0, 0.3)
            series3D.Add(0, 0.5, 0.3)

        End With


    End Sub


    'Diagramm für Abhängige Parameter initialisieren
    '***********************************************
    Private Sub DiagInitialise_AbhParameter(ByVal PES_Settings As EVO.Kern.EVO_Settings, ByVal AnzPar As Integer, ByRef Diag As EVO.Diagramm)

        With Diag
            .Clear()
            .Header.Text = "Abhängige Parameter"
            .Legend.Visible = False
            .Aspect.View3D = True
            .Aspect.Chart3DPercent = 90
            .Aspect.Elevation = 348
            .Aspect.Orthogonal = False
            .Aspect.Perspective = 62
            .Aspect.Rotation = 360
            .Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality
            .Aspect.VertOffset = -20
            .Aspect.Zoom = 66
            .Tools.Add(New Steema.TeeChart.Tools.Rotate())

            'Achsen:
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Visible = True
            .Chart.Axes.Bottom.Title.Caption = "X"
            .Chart.Axes.Bottom.Maximum = 1
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Bottom.Increment = 0.2

            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Visible = True
            .Chart.Axes.Left.Title.Caption = "Y"
            .Chart.Axes.Left.Maximum = 1
            .Chart.Axes.Left.Minimum = 0
            .Chart.Axes.Left.Increment = 0.2

            .Chart.Axes.Depth.Automatic = False
            .Chart.Axes.Depth.Visible = True
            .Chart.Axes.Depth.Title.Caption = "Zielfunktion"
            .Chart.Axes.Depth.Maximum = 2
            .Chart.Axes.Depth.Minimum = 0
            .Chart.Axes.Depth.Increment = 0.5

            'Serien
            '-----------
            Dim surface As Steema.TeeChart.Styles.Surface

            'x = y
            Dim i, j, n As Integer
            Dim ArrayX() As Double
            Dim ArrayY() As Double
            Dim ArrayZ() As Double
            Dim surfaceRes As Integer = 11
            ReDim ArrayX(surfaceRes ^ 2 - 1)
            ReDim ArrayY(surfaceRes ^ 2 - 1)
            ReDim ArrayZ(surfaceRes ^ 2 - 1)

            n = 0
            For i = 0 To surfaceRes - 1
                For j = 0 To (surfaceRes - 1)
                    ArrayX(n) = i * (1.1 / surfaceRes)
                    ArrayZ(n) = j * (2.1 / surfaceRes)
                    ArrayY(n) = ArrayX(n)
                    n += 1
                Next
            Next

            surface = New Steema.TeeChart.Styles.Surface(Diag.Chart)
            surface.Title = "X = Y"
            surface.IrregularGrid = True
            surface.NumXValues = surfaceRes
            surface.NumZValues = surfaceRes
            surface.Add(ArrayX, ArrayY, ArrayZ)
            surface.UseColorRange = False
            surface.UsePalette = False
            surface.Brush.Solid = True
            surface.Brush.Color = Color.Green
            surface.Brush.Transparency = 70
            surface.Pen.Color = Color.Green
            surface.SideBrush.Visible = True
            surface.SideBrush.Color = Color.Red
            surface.SideBrush.Transparency = 70

        End With

    End Sub

#End Region 'Diagrammfunktionen

#Region "Evaluierung"

    'Evaluierung und Zeichnen der Testprobleme
    '*****************************************
    Public Sub Evaluierung_TestProbleme(ByVal mypara() As Double, ByVal durchlauf As Integer, ByVal ipop As Short, ByRef QN() As Double, ByRef RN() As Double, ByRef Diag As EVO.Diagramm)

        Dim i As Short
        Dim Unterteilung_X As Double
        Dim x1, x2 As Double
        Dim X() As Double
        Dim f2, f1, f3 As Double
        Dim g1, g2 As Double
        Dim globalAnzPar As Short = mypara.GetLength(0)
        Dim serie As Steema.TeeChart.Styles.Series

        Select Case Me.Combo_Testproblem.Text

            '*************************************
            '* Single-Objective Problemstellungen *
            '*************************************

            Case "Sinus-Funktion"
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Fehlerquadrate zur Sinusfunktion |0-2pi|
                '----------------------------------------
                Unterteilung_X = 2 * Math.PI / (globalAnzPar - 1)

                QN(0) = 0
                For i = 0 To globalAnzPar - 1
                    QN(0) += (Math.Sin(i * Unterteilung_X) - (-1 + (mypara(i) * 2))) ^ 2
                Next i

                'Zeichnen
                '--------
                Dim array_x() As Double = {}
                Dim array_y() As Double = {}

                ReDim array_x(globalAnzPar - 1)
                ReDim array_y(globalAnzPar - 1)
                For i = 0 To globalAnzPar - 1
                    array_x(i) = Math.Round(i * Unterteilung_X, 2)
                    array_y(i) = (-1 + mypara(i) * 2)
                Next i

                serie = Diag.getSeriesPoint("Population " & ipop + 1)
                serie.Add(array_x, array_y)


            Case "Beale-Problem"
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualitätswert berechnen
                '-----------------------
                x1 = -5 + (mypara(0) * 10)
                x2 = -2 + (mypara(1) * 4)

                QN(0) = (1.5 - x1 * (1 - x2)) ^ 2 + (2.25 - x1 * (1 - x2) ^ 2) ^ 2 + (2.625 - x1 * (1 - x2) ^ 3) ^ 2

                'Zeichnen
                '--------
                serie = Diag.getSeriesPoint("Population " & ipop + 1)
                serie.Add(durchlauf, QN(0))

            Case "Schwefel 2.4-Problem"
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualitätswert berechnen
                '-----------------------
                ReDim X(globalAnzPar - 1)
                For i = 0 To globalAnzPar - 1
                    X(i) = -10 + mypara(i) * 20
                Next i
                QN(0) = 0
                For i = 0 To globalAnzPar - 1
                    QN(0) = QN(0) + ((X(0) - X(i) ^ 2) ^ 2 + (X(i) - 1) ^ 2)
                Next i

                'Zeichnen
                '--------
                serie = Diag.getSeriesPoint("Population " & ipop + 1)
                serie.Add(durchlauf, QN(0))

                '*************************************
                '* Multi-Objective Problemstellungen *
                '*************************************

            Case "Deb 1" 'Deb 2000, D1 (Konvexe Pareto-Front)
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualitätswert berechnen
                '-----------------------
                f1 = mypara(0) * (9 / 10) + 0.1
                f2 = (1 + 5 * mypara(1)) / (mypara(0) * (9 / 10) + 0.1)
                QN(0) = f1
                QN(1) = f2

                'Zeichnen
                '--------
                serie = Diag.getSeriesPoint("Population", "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                serie.Add(f1, f2)

            Case "Zitzler/Deb T1" 'Zitzler/Deb/Thiele 2000, T1 (Konvexe Pareto-Front)
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualitätswert berechnen
                '-----------------------
                f1 = mypara(0)
                f2 = 0
                For i = 1 To globalAnzPar - 1
                    f2 = f2 + mypara(i)
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - System.Math.Sqrt(f1 / f2))
                QN(0) = f1
                QN(1) = f2

                'Zeichnen
                '--------
                serie = Diag.getSeriesPoint("Population", "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                serie.Add(f1, f2)

            Case "Zitzler/Deb T2" 'Zitzler/Deb/Thiele 2000, T2 (Non-Konvexe Pareto-Front)
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualitätswerte berechnen
                '------------------------
                f1 = mypara(0)
                f2 = 0
                For i = 1 To globalAnzPar - 1
                    f2 = f2 + mypara(i)
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - (f1 / f2) * (f1 / f2))
                QN(0) = f1
                QN(1) = f2

                'Zeichnen
                '--------
                serie = Diag.getSeriesPoint("Population", "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                serie.Add(f1, f2)

            Case "Zitzler/Deb T3" 'Zitzler/Deb/Thiele 2000, T3 (disconected Pareto-Front)
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualitätswerte berechnen
                '------------------------
                f1 = mypara(0)
                f2 = 0
                For i = 1 To globalAnzPar - 1
                    f2 = f2 + mypara(i)
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - Math.Sqrt(f1 / f2) - (f1 / f2) * Math.Sin(10 * Math.PI * f1))
                QN(0) = f1
                QN(1) = f2

                'Zeichnen
                '--------
                serie = Diag.getSeriesPoint("Population", "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                serie.Add(f1, f2)

            Case "Zitzler/Deb T4" 'Zitzler/Deb/Thiele 2000, T4 (local/global Pareto-Fronts)
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualitätswerte berechnen
                '------------------------
                f1 = mypara(0)
                f2 = 0
                For i = 1 To globalAnzPar - 1
                    x2 = -5 + (mypara(i) * 10)
                    f2 = f2 + (x2 * x2 - 10 * Math.Cos(4 * Math.PI * x2))
                Next i
                f2 = 1 + 10 * (globalAnzPar - 1) + f2
                f2 = f2 * (1 - System.Math.Sqrt(f1 / f2))
                QN(0) = f1
                QN(1) = f2

                'Zeichnen
                '--------
                serie = Diag.getSeriesPoint("Population", "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                serie.Add(f1, f2)

            Case "CONSTR"
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualitätswerte berechnen
                '------------------------
                f1 = mypara(0) * (9 / 10) + 0.1
                f2 = (1 + 5 * mypara(1)) / (mypara(0) * (9 / 10) + 0.1)

                QN(0) = f1
                QN(1) = f2

                'Constraints berechnen
                '---------------------
                g1 = (5 * mypara(1)) + 9 * (mypara(0) * (9 / 10) + 0.1) - 6
                g2 = (-1) * (5 * mypara(1)) + 9 * (mypara(0) * (9 / 10) + 0.1) - 1

                RN(0) = g1
                RN(1) = g2

                'Zeichnen
                '--------
                If (g1 < 0 Or g2 < 0) Then
                    'Ungültige Lösung
                    serie = Diag.getSeriesPoint("Population (ungültig)", "Gray", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                Else
                    'Gültige Lösung
                    serie = Diag.getSeriesPoint("Population", "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                End If
                serie.Add(f1, f2)

            Case "Box"
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualitätswerte berechnen
                '------------------------
                f1 = mypara(0)
                f2 = mypara(1)
                f3 = mypara(2)

                QN(0) = f1
                QN(1) = f2
                QN(2) = f3

                'Constraints berechnen
                '---------------------
                g1 = mypara(0) + mypara(1) - 0.5
                g2 = mypara(0) + mypara(1) + mypara(2) - 0.8

                RN(0) = g1
                RN(1) = g2

                'Zeichnen
                '--------
                Dim serie3D As Steema.TeeChart.Styles.Points3D
                If (g1 < 0 Or g2 < 0) Then
                    'Ungültige Lösung
                    serie3D = Diag.getSeries3DPoint("Population (ungültig)", "Gray")
                Else
                    'Gültige Lösung
                    serie3D = Diag.getSeries3DPoint("Population", "Orange")
                End If
                serie3D.Add(f1, f2, f3)

            Case "Abhängige Parameter"
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualitätswerte berechnen
                '------------------------
                QN(0) = mypara(0) ^ 2 + mypara(1) ^ 2

                'Zeichnen
                '--------
                Dim serie3D As Steema.TeeChart.Styles.Points3D
                serie3D = Diag.getSeries3DPoint("Population " & ipop + 1)
                serie3D.Add(mypara(0), mypara(1), QN(0))

        End Select

    End Sub

#End Region 'Evaluierung

End Class
