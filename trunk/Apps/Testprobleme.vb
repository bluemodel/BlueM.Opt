Public Class Testprobleme
    Inherits System.Windows.Forms.UserControl

    Private IsInitializing As Boolean
    Public OptModus As Short
    Event Testproblem_Changed(ByVal sender As Object, ByVal e As System.EventArgs)


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

        Combo_Testproblem.SelectedIndex = 0

        'Ende der Initialisierung
        IsInitializing = False
    End Sub

    'Steuerung des Testproblem Forms auf dem Form1
    Private Sub Combo_Testproblem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Combo_Testproblem.SelectedIndexChanged

        If IsInitializing = True Then
            Exit Sub
        Else
            Select Case Combo_Testproblem.Text
                Case "Sinus-Funktion"
                    Problem_SinusFunktion.BringToFront()
                    OptModus = 0
                Case "Beale-Problem"
                    Problem_BealeProblem.BringToFront()
                    OptModus = 0
                Case "Schwefel 2.4-Problem"
                    Problem_Schwefel24.BringToFront()
                    OptModus = 0
                Case "Deb 1"
                    Problem_D1Funktion.BringToFront()
                    OptModus = 1
                Case "Zitzler/Deb T1"
                    Problem_T1Funktion.BringToFront()
                    OptModus = 1
                Case "Zitzler/Deb T2"
                    Problem_T2Funktion.BringToFront()
                    OptModus = 1
                Case "Zitzler/Deb T3"
                    Problem_T3Funktion.BringToFront()
                    OptModus = 1
                Case "Zitzler/Deb T4"
                    Problem_T4Funktion.BringToFront()
                    OptModus = 1
                Case "CONSTR"
                    Problem_CONSTRFunktion.BringToFront()
                    OptModus = 1
                Case "Box"
                    Problem_TKNFunktion.BringToFront()
                    OptModus = 1
            End Select

            RaiseEvent Testproblem_Changed(sender, e) 'wird in Form1 von IniApp() verarbeitet

        End If
    End Sub

    '************************************************************************************
    '                        Setzen der Anfangsparameter                                *
    '************************************************************************************

    'Startparameter werden festgesetzt
    Public Sub Parameter_Uebergabe(ByVal Testproblem As String, ByVal globAnzPar_Sin As String, ByVal globAnzPar_Schw As String, ByRef globalAnzPar As Short, ByRef globalAnzZiel As Short, ByRef globalAnzRand As Short, ByRef mypara(,) As Double)

        Dim i As Integer

        Select Case Testproblem
            Case "Sinus-Funktion"
                globalAnzPar = CShort(globAnzPar_Sin)
                globalAnzZiel = 1
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                For i = 1 To globalAnzPar
                    mypara(i, 1) = 0
                Next
            Case "Beale-Problem" 'x1 = [-5;5], x2=[-2;2]
                globalAnzPar = 2
                globalAnzZiel = 1
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                mypara(1, 1) = 0.5
                mypara(2, 1) = 0.5
            Case "Schwefel 2.4-Problem" 'xi = [-10,10]
                globalAnzPar = CShort(globAnzPar_Schw)
                globalAnzZiel = 1
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                For i = 1 To globalAnzPar
                    mypara(i, 1) = 1
                Next i
            Case "Deb 1" 'x1 = [0.1;1], x2=[0;5]
                globalAnzPar = 2
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                mypara(1, 1) = Rnd()
                mypara(2, 1) = Rnd()
            Case "Zitzler/Deb T1" 'xi = [0,1]
                globalAnzPar = 30
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                For i = 1 To globalAnzPar
                    mypara(i, 1) = Rnd()
                Next i
            Case "Zitzler/Deb T2" 'xi = [0,1]
                globalAnzPar = 30
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                For i = 1 To globalAnzPar
                    mypara(i, 1) = Rnd()
                Next i
            Case "Zitzler/Deb T3" 'xi = [0,1]
                globalAnzPar = 15
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                For i = 1 To globalAnzPar
                    mypara(i, 1) = Rnd()
                Next i
            Case "Zitzler/Deb T4" 'x1 = [0,1], xi=[-5,5]
                globalAnzPar = 10
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                For i = 1 To globalAnzPar
                    mypara(i, 1) = Rnd()
                Next i
            Case "CONSTR" 'x1 = [0.1;1], x2=[0;5]
                globalAnzPar = 2
                globalAnzZiel = 2
                globalAnzRand = 2
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                mypara(1, 1) = Rnd()
                mypara(2, 1) = Rnd()
            Case "Box"
                globalAnzPar = 3
                globalAnzZiel = 3
                globalAnzRand = 2
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                mypara(1, 1) = Rnd()
                mypara(2, 1) = Rnd()
                mypara(3, 1) = Rnd()
        End Select
    End Sub


    '************************************************************************************
    '                        Initialisierung der TCharts                                *
    '************************************************************************************

    'Alle Series für TeeChart werden initialisiert
    'Teilweise werden die Ziel bzw. Ausgangslinien berechnet und gezeichnet
    Public Sub TeeChartIni_SinusFunktion(ByVal EVO_Einstellungen1 As EvoForm.EVO_Einstellungen, ByVal globalAnzPar As Short, ByVal AnzPara As Integer, ByRef TChart1 As Steema.TeeChart.TChart)
        Dim array_x() As Double = {}
        Dim array_y() As Double = {}
        Dim Ausgangsergebnis As Double
        Dim Anzahl_Kalkulationen As Integer
        Dim Populationen As Short
        Dim i As Short
        Dim Datenmenge As Short
        Dim Unterteilung_X As Double

        If EVO_Einstellungen1.isPOPUL Then
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf * EVO_Einstellungen1.NRunden
        Else
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf
        End If

        'Ausgengsergebnisse für die Linien im TeeChart Rechnen
        Datenmenge = AnzPara
        Unterteilung_X = 2 * 3.141592654 / (Datenmenge - 1)

        'Linien für die Ausgangsergebnisse im TeeChart zeichnen
        ReDim array_x(Datenmenge - 1)
        ReDim array_y(Datenmenge - 1)

        For i = 0 To Datenmenge - 1
            array_x(i) = System.Math.Round((i) * Unterteilung_X, 2)
            array_y(i) = System.Math.Sin((i) * Unterteilung_X)
        Next i

        'TeeChart Einrichten und Series generieren
        With TChart1
            .Clear()
            .Header.Text = "Sinus Funktion"
            .Chart.Axes.Left.Title.Caption = "Funktionswert"
            .Chart.Axes.Bottom.Title.Caption = "Berechnungsschritt"
            .Aspect.View3D = False
            .Legend.Visible = False

            'S0: Die Ausgangs- oder Ziellinien
            Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
            Line1.Title = "Ausgangs-/Ziellinie"
            Line1.Add(array_x, array_y)
            Line1.Brush.Color = System.Drawing.Color.Red
            Line1.ClickableLine = True

            'S1: Generieren der Series für die Populationen
            Populationen = 1
            If EVO_Einstellungen1.isPOPUL Then
                Populationen = EVO_Einstellungen1.NPopul
            End If
            For i = 1 To Populationen
                Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
                Point1.Title = "Population " & i.ToString()
                Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                Point1.Pointer.HorizSize = 3
                Point1.Pointer.VertSize = 3
            Next

            'Axen Formatieren
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Maximum = Anzahl_Kalkulationen
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Maximum = Ausgangsergebnis * 1.3
            .Chart.Axes.Left.Minimum = 0
            .Chart.Axes.Left.Logarithmic = False
            .Chart.Axes.Bottom.Automatic = True
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Minimum = -1
            .Chart.Axes.Left.Maximum = 1
            .Chart.Axes.Left.Increment = 0.2

        End With
    End Sub

    Public Sub TeeChartIni_BealeProblem(ByVal EVO_Einstellungen1 As EvoForm.EVO_Einstellungen, ByVal globalAnzPar As Short, ByRef TChart1 As Steema.TeeChart.TChart)
        Dim array_x() As Double = {}
        Dim array_y() As Double = {}
        Dim Ausgangsergebnis As Double
        Dim Anzahl_Kalkulationen As Integer
        Dim Populationen As Short
        Dim i As Short
        Dim OptErg() As Double

        If EVO_Einstellungen1.isPOPUL Then
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf * EVO_Einstellungen1.NRunden
        Else
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf
        End If

        'Ausgengsergebnisse für die Linien im TeeChart Rechnen
        ReDim OptErg(Anzahl_Kalkulationen)
        Ausgangsergebnis = (1.5 - 0.5 * (1 - 0.5)) ^ 2 + (2.25 - 0.5 * (1 - 0.5) ^ 2) ^ 2 + (2.625 - 0.5 * (1 - 0.5) ^ 3) ^ 2

        'Linien für die Ausgangsergebnisse im TeeChart zeichnen
        ReDim array_y(Anzahl_Kalkulationen - 1)
        ReDim array_x(Anzahl_Kalkulationen - 1)
        For i = 0 To Anzahl_Kalkulationen - 1
            array_y(i) = Ausgangsergebnis
            array_x(i) = i + 1
        Next i

        'TeeChart Einrichten und Series generieren
        With TChart1
            .Clear()
            .Header.Text = "Beale Problem"
            .Chart.Axes.Left.Title.Caption = "Funktionswert"
            .Chart.Axes.Bottom.Title.Caption = "Berechnungsschritt"
            .Aspect.View3D = False
            .Legend.Visible = False

            'S0: Die Ausgangs- oder Ziellinien
            Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
            Line1.Title = "Ausgangs-/Ziellinie"
            Line1.Add(array_x, array_y)
            Line1.Brush.Color = System.Drawing.Color.Red
            Line1.ClickableLine = True

            'S1: Generieren der Series für die Populationen
            Populationen = 1
            If EVO_Einstellungen1.isPOPUL Then
                Populationen = EVO_Einstellungen1.NPopul
            End If
            For i = 1 To Populationen
                Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
                Point1.Title = "Population " & i.ToString()
                Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                Point1.Pointer.HorizSize = 3
                Point1.Pointer.VertSize = 3
            Next

            'Axen Formatieren
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Maximum = Anzahl_Kalkulationen
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Maximum = Ausgangsergebnis * 1.3
            .Chart.Axes.Left.Minimum = 0
            .Chart.Axes.Left.Logarithmic = False

        End With
    End Sub

    Public Sub TeeChartIni_SchwefelProblem(ByVal EVO_Einstellungen1 As EvoForm.EVO_Einstellungen, ByVal globalAnzPar As Short, ByRef TChart1 As Steema.TeeChart.TChart)
        Dim array_x() As Double = {}
        Dim array_y() As Double = {}
        Dim Ausgangsergebnis As Double
        Dim Anzahl_Kalkulationen As Integer
        Dim Populationen As Short
        Dim i As Short
        Dim X() As Double

        If EVO_Einstellungen1.isPOPUL Then
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf * EVO_Einstellungen1.NRunden
        Else
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf
        End If

        'Ausgengsergebnisse für die Linien im TeeChart Rechnen
        ReDim X(globalAnzPar)
        For i = 1 To globalAnzPar
            X(i) = 10
        Next i
        Ausgangsergebnis = 0
        For i = 1 To globalAnzPar
            Ausgangsergebnis = Ausgangsergebnis + ((X(1) - X(i) ^ 2) ^ 2 + (X(i) - 1) ^ 2)
        Next i

        'Linien für die Ausgangsergebnisse im TeeChart zeichnen
        ReDim array_y(Anzahl_Kalkulationen - 1)
        ReDim array_x(Anzahl_Kalkulationen - 1)
        For i = 0 To Anzahl_Kalkulationen - 1
            array_y(i) = Ausgangsergebnis
            array_x(i) = i + 1
        Next i

        'TeeChart Einrichten und Series generieren
        With TChart1
            .Clear()
            .Header.Text = "Schwefel 2.4 Problem"
            .Chart.Axes.Left.Title.Caption = "Funktionswert"
            .Chart.Axes.Bottom.Title.Caption = "Berechnungsschritt"
            .Aspect.View3D = False
            .Legend.Visible = False

            'S0: Die Ausgangs- oder Ziellinien
            Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
            Line1.Title = "Ausgangs-/Ziellinie"
            Line1.Add(array_x, array_y)
            Line1.Brush.Color = System.Drawing.Color.Red
            Line1.ClickableLine = True

            'S1: Generieren der Series für die Populationen
            Populationen = 1
            If EVO_Einstellungen1.isPOPUL Then
                Populationen = EVO_Einstellungen1.NPopul
            End If
            For i = 1 To Populationen
                Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
                Point1.Title = "Population " & i.ToString()
                Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                Point1.Pointer.HorizSize = 3
                Point1.Pointer.VertSize = 3
            Next

            'Axen Formatieren
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Maximum = Anzahl_Kalkulationen
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Maximum = Ausgangsergebnis * 1.3
            .Chart.Axes.Left.Minimum = 0
            .Chart.Axes.Left.Logarithmic = False
        End With
    End Sub

    Public Sub TeeChartIni_MultiTestProb(ByVal EVO_Einstellungen1 As EvoForm.EVO_Einstellungen, ByVal Testproblem As String, ByRef TChart1 As Steema.TeeChart.TChart)
        Dim Populationen As Short
        Dim i, j As Short

        Populationen = EVO_Einstellungen1.NPopul

        With TChart1
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

            'S0: Series für die Population.
            Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
            Point1.Title = "Population"
            Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point1.Color = System.Drawing.Color.Orange
            Point1.Pointer.HorizSize = 2
            Point1.Pointer.VertSize = 2

            'S1: Series für die Sekundäre Population
            Dim Point2 As New Steema.TeeChart.Styles.Points(.Chart)
            Point2.Title = "Sekundäre Population"
            Point2.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point2.Color = System.Drawing.Color.Blue
            Point2.Pointer.HorizSize = 3
            Point2.Pointer.VertSize = 3

            'S2: Series für Bestwert
            Dim Point3 As New Steema.TeeChart.Styles.Points(.Chart)
            Point3.Title = "Bestwerte"
            Point3.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point3.Color = System.Drawing.Color.Green
            Point3.Pointer.HorizSize = 3
            Point3.Pointer.VertSize = 3

            Select Case Testproblem

                Case "Deb 1"
                    'TODO: Titel der Serien (für Export)
                    Dim Array1X(100) As Double
                    Dim Array1Y(100) As Double
                    Dim Array2X(100) As Double
                    Dim Array2Y(100) As Double
                    .Header.Text = "Deb D1 - MO-konvex"

                    'S3: Linie 1 wird errechnet und gezeichnet
                    For j = 0 To 100
                        Array1X(j) = 0.1 + j * 0.009
                        Array1Y(j) = 1 / Array1X(j)
                    Next j
                    Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
                    Line1.Brush.Color = System.Drawing.Color.Green
                    Line1.ClickableLine = True
                    .Series(3).Add(Array1X, Array1Y)

                    'S4: Linie 2 wird errechnet und gezeichnet
                    For j = 0 To 100
                        Array2X(j) = 0.1 + j * 0.009
                        Array2Y(j) = (1 + 5) / Array2X(j)
                    Next j
                    Dim Line2 As New Steema.TeeChart.Styles.Line(.Chart)
                    Line2.Brush.Color = System.Drawing.Color.Red
                    Line2.ClickableLine = True
                    .Series(4).Add(Array2X, Array2Y)

                Case "Zitzler/Deb T1"
                    'TODO: Titel der Serien (für Export)
                    Dim ArrayX(1000) As Double
                    Dim ArrayY(1000) As Double
                    .Header.Text = "Zitzler/Deb/Theile T1"
                    .Chart.Axes.Left.Maximum = 7
                    .Chart.Axes.Left.Increment = 0.5

                    'S3: Serie für die Grenze
                    For j = 0 To 1000
                        ArrayX(j) = j / 1000
                        ArrayY(j) = 1 - System.Math.Sqrt(ArrayX(j))
                    Next j
                    Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
                    Line1.Brush.Color = System.Drawing.Color.Green
                    Line1.ClickableLine = True
                    .Series(3).Add(ArrayX, ArrayY)

                Case "Zitzler/Deb T2"
                    'TODO: Titel der Serien (für Export)
                    Dim ArrayX(100) As Double
                    Dim ArrayY(100) As Double
                    .Header.Text = "Zitzler/Deb/Theile T2"
                    .Chart.Axes.Left.Maximum = 7

                    'S3: Serie für die Grenze
                    For j = 0 To 100
                        ArrayX(j) = j / 100
                        ArrayY(j) = 1 - (ArrayX(j) * ArrayX(j))
                    Next j
                    Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
                    Line1.Brush.Color = System.Drawing.Color.Green
                    Line1.ClickableLine = True
                    .Series(3).Add(ArrayX, ArrayY)

                Case "Zitzler/Deb T3"
                    'TODO: Titel der Serien (für Export)
                    Dim ArrayX(100) As Double
                    Dim ArrayY(100) As Double
                    .Header.Text = "Zitzler/Deb/Theile T3"
                    .Chart.Axes.Bottom.Increment = 0.2
                    .Chart.Axes.Left.Maximum = 7
                    .Chart.Axes.Left.Minimum = -1
                    .Chart.Axes.Left.Increment = 0.5

                    'S3: Serie für die Grenze
                    For j = 0 To 100
                        ArrayX(j) = j / 100
                        ArrayY(j) = 1 - System.Math.Sqrt(ArrayX(j)) - ArrayX(j) * System.Math.Sin(10 * 3.14159265358979 * ArrayX(j))
                    Next j
                    Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
                    Line1.Brush.Color = System.Drawing.Color.Green
                    Line1.ClickableLine = True
                    .Series(3).Add(ArrayX, ArrayY)

                Case "Zitzler/Deb T4"
                    'TODO: Titel der Serien (für Export)
                    Dim ArrayX(1000) As Double
                    Dim ArrayY(1000) As Double
                    .Header.Text = "Zitzler/Deb/Theile T4"
                    .Chart.Axes.Bottom.Automatic = True
                    .Chart.Axes.Left.Automatic = True

                    'S3 bis S13: Serie für die Grenze
                    'Sieht nach einer schwachsinnigen Berechnung für ArrayY aus
                    For i = 1 To 10
                        Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
                        Line1.Brush.Color = System.Drawing.Color.Green
                        Line1.ClickableLine = True
                        For j = 0 To 1000
                            ArrayX(j) = j / 1000
                            ArrayY(j) = (1 + (i - 1) / 4) * (1 - System.Math.Sqrt(ArrayX(j) / (1 + (i - 1) / 4)))
                        Next
                        .Series(2 + i).Add(ArrayX, ArrayY)
                    Next

                    ''Original Code
                    'For i = 1 To 10
                    '    .AddSeries(TeeChart.ESeriesClass.scLine)
                    '    .Series(Populationen + i).asLine.LinePen.Width = 2
                    '    .Series(Populationen + i).Color = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue))
                    '    For j = 0 To 1000
                    '        ArrayX(j) = j / 1000
                    '        ArrayY(j) = (1 + (i - 1) / 4) * (1 - System.Math.Sqrt(ArrayX(j) / (1 + (i - 1) / 4)))
                    '    Next j
                    '    .Series(Populationen + i).AddArray(1000, ArrayY, ArrayX)
                    'Next i
                Case "CONSTR"
                    'TODO: Titel der Serien (für Export)
                    Dim Array1X(100) As Double
                    Dim Array1Y(100) As Double
                    Dim Array2X(100) As Double
                    Dim Array2Y(100) As Double
                    Dim Array3X(61) As Double
                    Dim Array3Y(61) As Double
                    Dim Array4X(61) As Double
                    Dim Array4Y(61) As Double
                    .Header.Text = "CONSTR"
                    'S3: Serie für die Grenze 1
                    For j = 0 To 100
                        Array1X(j) = 0.1 + j * 0.009
                        Array1Y(j) = 1 / Array1X(j)
                    Next j
                    Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
                    Line1.Brush.Color = System.Drawing.Color.Red
                    Line1.ClickableLine = True
                    .Series(3).Add(Array1X, Array1Y)

                    'S4: Serie für die Grenze 2
                    For j = 0 To 100
                        Array2X(j) = 0.1 + j * 0.009
                        Array2Y(j) = (1 + 5) / Array2X(j)
                    Next j
                    Dim Line2 As New Steema.TeeChart.Styles.Line(.Chart)
                    Line2.Brush.Color = System.Drawing.Color.Red
                    Line2.ClickableLine = True
                    .Series(4).Add(Array2X, Array2Y)

                    'S5: Serie für die Grenze 3
                    ReDim Array3X(61)
                    ReDim Array3Y(61)
                    For j = 0 To 61
                        Array3X(j) = 0.1 + (j + 2) * 0.009
                        Array3Y(j) = (7 - 9 * Array3X(j)) / Array3X(j)
                    Next j
                    Dim Line3 As New Steema.TeeChart.Styles.Line(.Chart)
                    Line3.Brush.Color = System.Drawing.Color.Blue
                    Line3.ClickableLine = True
                    .Series(5).Add(Array3X, Array3Y)

                    'S6: Serie für die Grenze 4
                    ReDim Array4X(61)
                    ReDim Array4Y(61)
                    For j = 0 To 61
                        Array4X(j) = 0.1 + (j + 2) * 0.009
                        Array4Y(j) = (9 * Array4X(j)) / Array4X(j)
                    Next j
                    Dim Line4 As New Steema.TeeChart.Styles.Line(.Chart)
                    Line4.Brush.Color = System.Drawing.Color.Red
                    Line4.ClickableLine = True
                    .Series(6).Add(Array4X, Array4Y)
            End Select
        End With
    End Sub

    Public Sub TeeChartIni_3D_Box(ByRef TChart1 As Steema.TeeChart.TChart, ByVal isPopul As Boolean, ByVal NPopul As Short)
        'TODO: Zeichnen muss auf 3D erweitert werden. Hier 3D Testproblem.
        Dim Populationen As Short
        Dim ArrayX(100) As Double
        Dim ArrayY(100) As Double

        If isPopul Then
            Populationen = NPopul
        Else
            Populationen = 1
        End If

        With TChart1
            .Clear()
            .Header.Text = "Box"
            .Aspect.View3D = True
            .Aspect.Chart3DPercent = 100
            .Legend.Visible = False
            .Chart.Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality
            .Chart.Axes.Bottom.Automatic = True
            .Chart.Axes.Bottom.Visible = True
            .Chart.Aspect.Zoom = 86
            '.Chart.Axes.Bottom.Maximum = 1
            '.Chart.Axes.Bottom.Minimum = 0
            '.Chart.Axes.Bottom.Increment = 0.2
            .Chart.Axes.Left.Automatic = True
            .Chart.Axes.Left.Visible = True
            '.Chart.Axes.Left.Maximum = 1
            '.Chart.Axes.Left.Minimum = 0
            '.Chart.Axes.Left.Increment = 0.2
            .Chart.Axes.Depth.Automatic = True
            .Chart.Axes.Depth.Visible = True
            '.Chart.Axes.Depth.Maximum = 1
            '.Chart.Axes.Depth.Minimum = 0
            '.Chart.Axes.Depth.Increment = 0.2
            '---------------------------------------------------------------
            'SO: Series für die Population
            Dim Point3D_0 As New Steema.TeeChart.Styles.Points(.Chart)
            Point3D_0.Title = "Population"
            Point3D_0.FillSampleValues(100)
            Point3D_0.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point3D_0.LinePen.Visible = False
            Point3D_0.Pointer.HorizSize = 1
            Point3D_0.Pointer.VertSize = 1

            'S1: Series für die Sekundäre Population
            Dim Point3D_1 As New Steema.TeeChart.Styles.Points(.Chart)
            Point3D_1.Title = "Sekundäre Population"
            Point3D_1.FillSampleValues(100)
            Point3D_1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point3D_1.LinePen.Visible = False
            Point3D_1.Pointer.HorizSize = 1
            Point3D_1.Pointer.VertSize = 1

            '.AddSeries(TeeChart.ESeriesClass.scPoint3D)
            '.Series(0).asPoint3D.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '.Series(0).asPoint3D.LinePen.Visible = False
            '.Series(0).asPoint3D.Pointer.HorizontalSize = 1
            '.Series(0).asPoint3D.Pointer.VerticalSize = 1


            'For i = 1 To Populationen
            '    .AddSeries(TeeChart.ESeriesClass.scPoint3D)
            '    .Series(i).asPoint3D.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '    .Series(i).asPoint3D.LinePen.Visible = False
            '    .Series(i).asPoint3D.Pointer.HorizontalSize = 3
            '    .Series(i).asPoint3D.Pointer.VerticalSize = 3
            'Next i

            '.AddSeries(TeeChart.ESeriesClass.scPoint3D)
            '.AddSeries(TeeChart.ESeriesClass.scPoint3D)
            '.Series(Populationen + 2).asPoint3D.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '.Series(Populationen + 2).asPoint3D.LinePen.Visible = False
            '.Series(Populationen + 2).asPoint3D.Pointer.HorizontalSize = 2
            '.Series(Populationen + 2).asPoint3D.Pointer.VerticalSize = 2
            '.Series(Populationen + 2).Color = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red))
        End With
    End Sub
    '*****************************************************************'******************
    '                      Evaluierung der Testprobleme                                 *
    '************************************************************************************

    Public Function Evaluierung_TestProbleme(ByRef Testproblem As String, ByVal globalAnzPar As Short, ByVal mypara(,) As Double, ByVal durchlauf As Integer, ByVal ipop As Short, ByRef QN() As Double, ByRef RN() As Double, ByRef tchart1 As Steema.TeeChart.TChart) As Boolean

        Dim i As Short
        Dim Unterteilung_X As Double
        Dim x1, x2 As Double
        Dim X() As Double
        Dim f2, f1, f3 As Double
        Dim g1, g2 As Double

        '* Single-Objective Problemstellungen *
        Select Case Testproblem

            Case "Sinus-Funktion" 'Fehlerquadrate zur Sinusfunktion |0-2pi|
                Unterteilung_X = 2 * 3.1415926535898 / (globalAnzPar - 1)
                QN(1) = 0
                For i = 1 To globalAnzPar
                    QN(1) = QN(1) + (System.Math.Sin((i - 1) * Unterteilung_X) - (-1 + (mypara(i, 1) * 2))) * (System.Math.Sin((i - 1) * Unterteilung_X) - (-1 + mypara(i, 1) * 2))
                Next i
                Call Zielfunktion_zeichnen_Sinus(ipop, globalAnzPar, mypara, tchart1)
            Case "Beale-Problem" 'Beale-Problem
                x1 = -5 + (mypara(1, 1) * 10)
                x2 = -2 + (mypara(2, 1) * 4)

                QN(1) = (1.5 - x1 * (1 - x2)) ^ 2 + (2.25 - x1 * (1 - x2) ^ 2) ^ 2 + (2.625 - x1 * (1 - x2) ^ 3) ^ 2
                tchart1.Series(ipop).Add(durchlauf, QN(1))
            Case "Schwefel 2.4-Problem" 'Schwefel 2.4 S. 329
                ReDim X(globalAnzPar)
                For i = 1 To globalAnzPar
                    X(i) = -10 + mypara(i, 1) * 20
                Next i
                QN(1) = 0
                For i = 1 To globalAnzPar
                    QN(1) = QN(1) + ((X(1) - X(i) ^ 2) ^ 2 + (X(i) - 1) ^ 2)
                Next i
                tchart1.Series(ipop).Add(durchlauf, QN(1))
                '*************************************
                '* Multi-Objective Problemstellungen *
                '*************************************
                'Deb 2000, D1 (Konvexe Pareto-Front)
            Case "Deb 1"
                f1 = mypara(1, 1) * (9 / 10) + 0.1
                f2 = (1 + 5 * mypara(2, 1)) / (mypara(1, 1) * (9 / 10) + 0.1)
                QN(1) = f1
                QN(2) = f2
                tchart1.Series(0).Add(f1, f2, "")

                'Zitzler/Deb/Thiele 2000, T1 (Konvexe Pareto-Front)
            Case "Zitzler/Deb T1"
                f1 = mypara(1, 1)
                f2 = 0
                For i = 2 To globalAnzPar
                    f2 = f2 + mypara(i, 1)
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - System.Math.Sqrt(f1 / f2))
                QN(1) = f1
                QN(2) = f2
                tchart1.Series(0).Add(f1, f2, "")

                'Zitzler/Deb/Thiele 2000, T2 (Non-Konvexe Pareto-Front)
            Case "Zitzler/Deb T2"
                f1 = mypara(1, 1)
                f2 = 0
                For i = 2 To globalAnzPar
                    f2 = f2 + mypara(i, 1)
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - (f1 / f2) * (f1 / f2))
                QN(1) = f1
                QN(2) = f2
                tchart1.Series(0).Add(f1, f2, "")

                'Zitzler/Deb/Thiele 2000, T3 (disconected Pareto-Front)
            Case "Zitzler/Deb T3"
                f1 = mypara(1, 1)
                f2 = 0
                For i = 2 To globalAnzPar
                    f2 = f2 + mypara(i, 1)
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - System.Math.Sqrt(f1 / f2) - (f1 / f2) * System.Math.Sin(10 * 3.14159265358979 * f1))
                QN(1) = f1
                QN(2) = f2
                tchart1.Series(0).Add(f1, f2, "")

                'Zitzler/Deb/Thiele 2000, T4 (local/global Pareto-Fronts)
            Case "Zitzler/Deb T4"
                f1 = mypara(1, 1)
                f2 = 0
                For i = 2 To globalAnzPar
                    x2 = -5 + (mypara(i, 1) * 10)
                    f2 = f2 + (x2 * x2 - 10 * System.Math.Cos(4 * 3.14159265358979 * x2))
                Next i
                f2 = 1 + 10 * (globalAnzPar - 1) + f2
                f2 = f2 * (1 - System.Math.Sqrt(f1 / f2))
                QN(1) = f1
                QN(2) = f2
                tchart1.Series(0).Add(f1, f2, "")

            Case "CONSTR"
                f1 = mypara(1, 1) * (9 / 10) + 0.1
                f2 = (1 + 5 * mypara(2, 1)) / (mypara(1, 1) * (9 / 10) + 0.1)

                g1 = (5 * mypara(2, 1)) + 9 * (mypara(1, 1) * (9 / 10) + 0.1) - 6
                g2 = (-1) * (5 * mypara(2, 1)) + 9 * (mypara(1, 1) * (9 / 10) + 0.1) - 1

                QN(1) = f1
                QN(2) = f2
                RN(1) = g1
                RN(2) = g2
                tchart1.Series(0).Add(f1, f2, "")

            Case "Box"
                f1 = mypara(1, 1) ^ 2
                f2 = mypara(2, 1) ^ 2
                f3 = mypara(3, 1) ^ 2
                g1 = mypara(1, 1) + mypara(3, 1) - 0.5
                g2 = mypara(1, 1) + mypara(2, 1) + mypara(3, 1) - 0.8

                '                f1 = 1 + (1 - Par(1, 1)) ^ 5
                '                f2 = Par(2, 1)
                '                f3 = Par(3, 1)
                '
                '                g1 = Par(1, 1) ^ 2 + Par(3, 1) ^ 2 - 0.5
                '                g2 = Par(2, 1) ^ 2 + Par(3, 1) ^ 2 - 0.5

                QN(1) = f1
                QN(2) = f2
                QN(3) = f3
                RN(1) = g1
                RN(2) = g2
                Call Zielfunktion_zeichnen_MultiObPar_3D(f1, f2, f3, tchart1)
        End Select
        Evaluierung_TestProbleme = True
    End Function

    '************************************************************************************
    '                          Zeichenfunktionen                                        *
    '************************************************************************************

    Public Sub Zielfunktion_zeichnen_Sinus(ByVal ipop As Short, ByVal AnzPar As Short, ByVal Par As Double(,), ByRef TChart1 As Steema.TeeChart.TChart)
        Dim i As Short
        Dim Unterteilung_X As Double
        Dim array_x() As Double = {}
        Dim array_y() As Double = {}

        Unterteilung_X = 2 * 3.141592654 / (AnzPar - 1)
        ReDim array_x(AnzPar - 1)
        ReDim array_y(AnzPar - 1)
        For i = 0 To AnzPar - 1
            array_x(i) = System.Math.Round((i) * Unterteilung_X, 2)
            array_y(i) = (-1 + Par(i + 1, 1) * 2)
        Next i

        With TChart1
            .Series(ipop).Clear()
            .Series(ipop).Add(array_x, array_y)
        End With
    End Sub

    Private Sub Zielfunktion_zeichnen_MultiObPar_3D(ByVal f1 As Double, ByVal f2 As Double, ByVal f3 As Double, ByRef tchart1 As Steema.TeeChart.TChart)

        tchart1.Series(0).FillSampleValues()
        tchart1.Series(1).FillSampleValues()
    End Sub

End Class
