Public Class Diagramm
    Inherits Steema.TeeChart.TChart

    Public Structure Achse
            Public Name as String
            Public Auto as Boolean
            Public Max as Double
    End Structure

    'TeeChart Initialisierung (Titel und Achsen)
    '*******************************************
    Public Sub DiagInitialise(ByVal Titel As String, ByVal Achsen As Collection)

        With Me
            .Clear()
            .Header.Text = Titel
            .Aspect.View3D = False
            .Legend.Visible = False

            'Formatierung der Axen
            '---------------------
            'X-Achse:
            .Chart.Axes.Bottom.Title.Caption = Achsen(1).Name
            .Chart.Axes.Bottom.Automatic = Achsen(1).Auto
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Bottom.Maximum = Achsen(1).Max
            .Chart.Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
            'Y-Achse:
            .Chart.Axes.Left.Title.Caption = Achsen(2).Name
            .Chart.Axes.Left.Automatic = Achsen(2).Auto
            .Chart.Axes.Left.Minimum = 0
            .Chart.Axes.Left.Maximum = Achsen(2).Max
            .Chart.Axes.Left.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
        End With

    End Sub

    'Serien-Initialisierung-Dynamisch
    '********************************
    Public Sub prepareSeries(ByVal SeriesNo As Integer, ByVal Title As String, Optional ByVal Style As Steema.TeeChart.Styles.PointerStyles = Steema.TeeChart.Styles.PointerStyles.Circle, Optional ByVal Size As Integer = 3)

        'Neue Series nur dann zum Chart hinzufügen, 
        'wenn SeriesNo dem nächsten freien Index entspricht
        If (Me.Chart.Series.Count = SeriesNo) Then
            'Series hinzufügen
            Dim tmpSeries As New Steema.TeeChart.Styles.Points(Me.Chart)
            tmpSeries.Title = Title
            tmpSeries.Pointer.Style = Style
            tmpSeries.Pointer.HorizSize = Size
            tmpSeries.Pointer.VertSize = Size

            Call Me.add_MarksTips()

        ElseIf (Me.Chart.Series.Count < SeriesNo) Then
            'Es wurde eine SeriesNo angegeben, 
            'die größer als der nächste freie Index ist!
            Throw New Exception("SeriesNo ist größer als nächster freier Index in SeriesCollection!")

        Else
            'Series besteht schon
        End If

    End Sub

    'Serien werden von Hinten gelöscht
    '*********************************
    Sub DeleteSeries(ByVal Max As Integer, ByVal Min As Integer)

        Dim i As Integer
        For i = Max To Min Step -1
            If Me.Chart.Series.Count - 1 = i Then
                Me.Chart.Series.Remove(Me.Chart.Series(i))
            End If
        Next

    End Sub

    'Serien-Initialisierung für SingleObjective
    '******************************************
    Public Sub prepareSeries_SO(ByVal n_Populationen As Integer)

        Dim i As Integer

        'Series(0): Anfangswert
        Dim tmpSeries As New Steema.TeeChart.Styles.Points(Me.Chart)
        tmpSeries.Title = "Anfangswert"
        tmpSeries.Color = System.Drawing.Color.Red
        tmpSeries.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
        tmpSeries.Pointer.HorizSize = 3
        tmpSeries.Pointer.VertSize = 3

        'Series(1 bis n): Für jede Population eine Series
        For i = 1 To n_Populationen
            tmpSeries = New Steema.TeeChart.Styles.Points(Me.Chart)
            tmpSeries.Title = "Population " & i.ToString()
            tmpSeries.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            tmpSeries.Pointer.HorizSize = 2
            tmpSeries.Pointer.VertSize = 2
            tmpSeries.Cursor = Cursors.Hand
        Next

        Call Me.add_MarksTips()

    End Sub

    'Serien-Initialisierung für MultiObjective
    '*****************************************
    Public Sub prepareSeries_MO()

        'Series(0): Series für die Population.
        Dim tmpSeries As New Steema.TeeChart.Styles.Points(Me.Chart)
        tmpSeries.Title = "Population"
        tmpSeries.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
        tmpSeries.Color = System.Drawing.Color.Orange
        tmpSeries.Pointer.HorizSize = 2
        tmpSeries.Pointer.VertSize = 2
        tmpSeries.Cursor = Cursors.Hand

        'Series(1): Series für die Sekundäre Population
        tmpSeries = New Steema.TeeChart.Styles.Points(Me.Chart)
        tmpSeries.Title = "Sekundäre Population"
        tmpSeries.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
        tmpSeries.Color = System.Drawing.Color.Blue
        tmpSeries.Pointer.HorizSize = 3
        tmpSeries.Pointer.VertSize = 3

        Call Me.add_MarksTips()

    End Sub

    'MarksTips zu Serien hinzufügen
    '******************************
    Public Sub add_MarksTips()
        Dim tmpMarksTip As Steema.TeeChart.Tools.MarksTip
        For i As Integer = 0 To Me.Chart.Series.Count - 1
            tmpMarksTip = New Steema.TeeChart.Tools.MarksTip(Me.Chart)
            tmpMarksTip.Series = Me.Chart.Series(i)
            tmpMarksTip.MouseAction = Steema.TeeChart.Tools.MarksTipMouseAction.Move
            tmpMarksTip.MouseDelay = 10 'millisekunden
            tmpMarksTip.Style = Steema.TeeChart.Styles.MarksStyles.XY
        Next
    End Sub

#Region "Testprobleme"

    'Initialisierung der TCharts für Testprobleme
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    'Alle Series für TeeChart werden initialisiert
    'Teilweise werden die Ziel bzw. Ausgangslinien berechnet und gezeichnet
    Public Sub DiagInitialise_SinusFunktion(ByVal EVO_Einstellungen1 As EVO_Einstellungen, ByVal globalAnzPar As Short, ByVal AnzPara As Integer)
        Dim array_x() As Double = {}
        Dim array_y() As Double = {}
        Dim Populationen As Short
        Dim i As Short
        Dim Datenmenge As Short
        Dim Unterteilung_X As Double
        Dim Pi As Double = 3.141592654

        'Ausgengsergebnisse für die Linien im TeeChart Rechnen
        Datenmenge = AnzPara
        Unterteilung_X = 2 * Pi / (Datenmenge - 1)

        'Linien für die Ausgangsergebnisse im TeeChart zeichnen
        ReDim array_x(Datenmenge - 1)
        ReDim array_y(Datenmenge - 1)

        For i = 0 To Datenmenge - 1
            array_x(i) = System.Math.Round((i) * Unterteilung_X, 2)
            array_y(i) = System.Math.Sin((i) * Unterteilung_X)
        Next i

        'TeeChart Einrichten und Series generieren
        With Me
            .Clear()
            .Header.Text = "Sinus Funktion"
            .Chart.Axes.Left.Title.Caption = "Y-Wert"
            .Chart.Axes.Bottom.Title.Caption = "X-Wert"
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
            .Chart.Axes.Bottom.Maximum = 2 * Pi
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Bottom.Increment = Pi
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Minimum = -1
            .Chart.Axes.Left.Maximum = 1
            .Chart.Axes.Left.Increment = 0.2

            Call Me.add_MarksTips()

        End With
    End Sub

    Public Sub DiagInitialise_BealeProblem(ByVal EVO_Einstellungen1 As EVO_Einstellungen, ByVal globalAnzPar As Short)
        Dim array_x() As Double = {}
        Dim array_y() As Double = {}
        Dim Ausgangsergebnis As Double
        Dim Anzahl_Kalkulationen As Integer
        Dim Populationen As Short
        Dim i As Short
        Dim OptErg() As Double

        Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf

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
        With Me
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

            Call Me.add_MarksTips()

        End With
    End Sub

    Public Sub DiagInitialise_SchwefelProblem(ByVal EVO_Einstellungen1 As EVO_Einstellungen, ByVal globalAnzPar As Short)
        Dim array_x() As Double = {}
        Dim array_y() As Double = {}
        Dim Ausgangsergebnis As Double
        Dim Anzahl_Kalkulationen As Integer
        Dim Populationen As Short
        Dim i As Short
        Dim X() As Double

        Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf

        'Ausgangsergebnisse für die Linien im TeeChart Rechnen
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
        With Me
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

            Call Me.add_MarksTips()

        End With
    End Sub

    Public Sub DiagInitialise_MultiTestProb(ByVal EVO_Einstellungen1 As EVO_Einstellungen, ByVal Testproblem As String)
        Dim Populationen As Short
        Dim i, j As Short

        Populationen = EVO_Einstellungen1.NPopul

        With Me
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

            Call Me.add_MarksTips()

        End With
    End Sub

    Public Sub DiagInitialise_3D_Box(ByVal isPopul As Boolean, ByVal NPopul As Short)
        'UPGRADE: Zeichnen muss auf 3D erweitert werden. Hier 3D Testproblem.
        Dim Populationen As Short
        Dim ArrayX(100) As Double
        Dim ArrayY(100) As Double

        If isPopul Then
            Populationen = NPopul
        Else
            Populationen = 1
        End If

        With Me
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

        Call Me.add_MarksTips()

    End Sub

#End Region 'Testprobleme

End Class
