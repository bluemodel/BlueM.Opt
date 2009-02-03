Imports IHWB.EVO.Common.Constants

Public Class Testprobleme

    'Eigenschaften
    '#############

    'Testprobleme:
    Private Const TP_SinusFunktion As String = "Sinus-Funktion"
    Private Const TP_BealeProblem As String = "Beale-Problem"
    Private Const TP_Schwefel24Problem As String = "Schwefel 2.4-Problem"
    Private Const TP_Deb1 As String = "Deb 1"
    Private Const TP_ZitzlerDebT1 As String = "Zitzler/Deb T1"
    Private Const TP_ZitzlerDebT2 As String = "Zitzler/Deb T2"
    Private Const TP_ZitzlerDebT3 As String = "Zitzler/Deb T3"
    Private Const TP_ZitzlerDebT4 As String = "Zitzler/Deb T4"
    Private Const TP_CONSTR As String = "CONSTR"
    Private Const TP_Box As String = "Box"
    Private Const TP_Abh�ngigeParameter As String = "Abh�ngige Parameter"
    Private Const TP_FloodMitigation As String = "Flood Mitigation"

    Private mSelectedTestproblem As String
    Private mTestProblemDescription As String

    Private mProblem As EVO.Common.Problem

    Private mAnzParameter As Integer
    Private mAnzZiele As Integer
    Private mAnzConstraints As Integer
    Private mOptPara() As Common.OptParameter

    'Properties
    '##########

    'Liste von Testproblemen
    Public ReadOnly Property Testprobleme() As String()
        Get
            Dim array() As String

            ReDim array(11)

            array(0) = TP_SinusFunktion
            array(1) = TP_BealeProblem
            array(2) = TP_Schwefel24Problem
            array(3) = TP_Deb1
            array(4) = TP_ZitzlerDebT1
            array(5) = TP_ZitzlerDebT2
            array(6) = TP_ZitzlerDebT3
            array(7) = TP_ZitzlerDebT4
            array(8) = TP_CONSTR
            array(9) = TP_Box
            array(10) = TP_Abh�ngigeParameter
			array(11) = TP_FloodMitigation

            Return array

        End Get
    End Property

    'gew�hltes Testproblem holen
    '***************************
    Public ReadOnly Property selectedTestproblem() As String
        Get
            Return mSelectedTestproblem
        End Get
    End Property

    'Problembeschreibung
    Public ReadOnly Property TestProblemDescription() As String
        Get
            Return Me.mTestProblemDescription
        End Get
    End Property

    'Testproblem setzen
    '******************
    Public Sub setTestproblem(ByVal name As String)

        Me.mSelectedTestproblem = name

    End Sub

    'Parameter�bergabe
    '*****************
    Public Sub getProblem(ByRef prob As EVO.Common.Problem)

        Dim i As Integer

        'Das Problem setzen
        Me.mProblem = prob

        'Je nach Datensatz/Testproblem initialisierungen durchf�hren
        Select Case Me.mSelectedTestproblem

            Case TP_SinusFunktion
                Me.mTestProblemDescription = "Parameter an Sinusfunktion anpassen"
                Me.mAnzParameter = 50
                Me.mAnzZiele = 1
                Me.mAnzConstraints = 0
                ReDim Me.mOptPara(Me.mAnzParameter - 1)
                For i = 0 To Me.mAnzParameter - 1
                    Me.mOptPara(i) = New EVO.Common.OptParameter()
                    Me.mOptPara(i).Xn = 0
                Next

            Case TP_BealeProblem
                Me.mTestProblemDescription = "Es wird das Minimum des Beale-Problems gesucht (x=(3, 0.5), F(x)=0)"
                Me.mAnzParameter = 2
                Me.mAnzZiele = 1
                Me.mAnzConstraints = 0
                ReDim Me.mOptPara(Me.mAnzParameter - 1)
                For i = 0 To Me.mAnzParameter - 1
                    Me.mOptPara(i) = New EVO.Common.OptParameter()
                    Me.mOptPara(i).Xn = 0.5
                Next

            Case TP_Schwefel24Problem
                Me.mTestProblemDescription = "Minimum der Problemstellung wird gesucht (xi=1, F(x)=0)"
                Me.mAnzParameter = 5
                Me.mAnzZiele = 1
                Me.mAnzConstraints = 0
                ReDim Me.mOptPara(Me.mAnzParameter - 1)
                For i = 0 To Me.mAnzParameter - 1
                    Me.mOptPara(i) = New EVO.Common.OptParameter()
                    Me.mOptPara(i).Xn = 1
                Next

            Case TP_Deb1
                Me.mTestProblemDescription = "Multikriterielles Testproblem (konvex)"
                Me.mAnzParameter = 2
                Me.mAnzZiele = 2
                Me.mAnzConstraints = 0
                ReDim Me.mOptPara(Me.mAnzParameter - 1)
                Randomize()
                For i = 0 To Me.mAnzParameter - 1
                    Me.mOptPara(i) = New EVO.Common.OptParameter()
                    Me.mOptPara(i).Xn = Rnd()
                Next

            Case TP_ZitzlerDebT1
                Me.mTestProblemDescription = "Multikriterielles Testproblem (konvex)"
                Me.mAnzParameter = 30
                Me.mAnzZiele = 2
                Me.mAnzConstraints = 0
                ReDim Me.mOptPara(Me.mAnzParameter - 1)
                Randomize()
                For i = 0 To Me.mAnzParameter - 1
                    Me.mOptPara(i) = New EVO.Common.OptParameter()
                    Me.mOptPara(i).Xn = Rnd()
                Next

            Case TP_ZitzlerDebT2
                Me.mTestProblemDescription = "Multikriterielles Testproblem (konkav)"
                Me.mAnzParameter = 30
                Me.mAnzZiele = 2
                Me.mAnzConstraints = 0
                ReDim Me.mOptPara(Me.mAnzParameter - 1)
                Randomize()
                For i = 0 To Me.mAnzParameter - 1
                    Me.mOptPara(i) = New EVO.Common.OptParameter()
                    Me.mOptPara(i).Xn = Rnd()
                Next

            Case TP_ZitzlerDebT3
                Me.mTestProblemDescription = "Multikriterielles Testproblem (konvex, nicht stetig)"
                Me.mAnzParameter = 15
                Me.mAnzZiele = 2
                Me.mAnzConstraints = 0
                ReDim Me.mOptPara(Me.mAnzParameter - 1)
                Randomize()
                For i = 0 To Me.mAnzParameter - 1
                    Me.mOptPara(i) = New EVO.Common.OptParameter()
                    Me.mOptPara(i).Xn = Rnd()
                Next

            Case TP_ZitzlerDebT4
                Me.mTestProblemDescription = "Multikriterielles Testproblem (konvex)"
                Me.mAnzParameter = 10
                Me.mAnzZiele = 2
                Me.mAnzConstraints = 0
                ReDim Me.mOptPara(Me.mAnzParameter - 1)
                Randomize()
                For i = 0 To Me.mAnzParameter - 1
                    Me.mOptPara(i) = New EVO.Common.OptParameter()
                    Me.mOptPara(i).Xn = Rnd()
                Next

            Case TP_CONSTR
                Me.mTestProblemDescription = "Multikriterielles Testproblem (konvex) mit zwei Randbedingungen"
                Me.mAnzParameter = 2
                Me.mAnzZiele = 2
                Me.mAnzConstraints = 2
                ReDim Me.mOptPara(Me.mAnzParameter - 1)
                Randomize()
                For i = 0 To Me.mAnzParameter - 1
                    Me.mOptPara(i) = New EVO.Common.OptParameter()
                    Me.mOptPara(i).Xn = Rnd()
                Next

            Case TP_Box
                Me.mTestProblemDescription = "Multikriterielles Testproblem (Kreis) mit zwei Randbedingungen"
                Me.mAnzParameter = 3
                Me.mAnzZiele = 3
                Me.mAnzConstraints = 2
                ReDim Me.mOptPara(Me.mAnzParameter - 1)
                Randomize()
                For i = 0 To Me.mAnzParameter - 1
                    Me.mOptPara(i) = New EVO.Common.OptParameter()
                    Me.mOptPara(i).Xn = Rnd()
                Next

            Case TP_Abh�ngigeParameter
                Me.mTestProblemDescription = "Bedingung: Y > X"
                Me.mAnzParameter = 2
                Me.mAnzZiele = 1
                Me.mAnzConstraints = 0
                ReDim Me.mOptPara(Me.mAnzParameter - 1)
                For i = 0 To Me.mAnzParameter - 1
                    Me.mOptPara(i) = New EVO.Common.OptParameter()
                    Me.mOptPara(i).Xn = 1
                Next
                'Beziehungen
                Me.mOptPara(0).Beziehung = Common.Constants.Beziehung.keine
                Me.mOptPara(1).Beziehung = Common.Constants.Beziehung.groesser

            Case TP_FloodMitigation 'Ajay
                Me.mTestProblemDescription = "Multicriteria Problem Flood Mitigation and Hydropower Generation"
                Me.mAnzParameter = 8                'Parameters
                Me.mAnzZiele = 2                    'Objective
                Me.mAnzConstraints = 4               'Constraints
                ReDim Me.mOptPara(Me.mAnzParameter - 1)
                Randomize()
                For i = 0 To Me.mAnzParameter - 1
                    Me.mOptPara(i) = New EVO.Common.OptParameter()
                    Me.mOptPara(i).Xn = Rnd()

                Next
                For i = 0 To 3
                    Me.mOptPara(i).Min = 470424
                    Me.mOptPara(i).Max = 48407547
                Next
                For i = 4 To Me.mAnzParameter - 1
                    Me.mOptPara(i).Min = 648000
                    Me.mOptPara(i).Max = 2592000
                Next

        End Select

        'Das Problem mit Pseudo-Werten f�llen
        ReDim Me.mProblem.List_Featurefunctions(Me.mAnzZiele - 1)
        For i = 0 To Me.mProblem.NumFeatures - 1
            Me.mProblem.List_Featurefunctions(i) = New Common.Featurefunction()
            Me.mProblem.List_Featurefunctions(i).isPenalty = True
        Next
        ReDim Me.mProblem.List_Constraintfunctions(Me.mAnzConstraints - 1)
        For i = 0 To Me.mProblem.NumConstraints - 1
            Me.mProblem.List_Constraintfunctions(i) = New Common.Constraintfunction()
        Next
        ReDim Me.mProblem.List_OptParameter(Me.mAnzParameter - 1)
        ReDim Me.mProblem.List_OptParameter_Save(Me.mAnzParameter - 1)
        For i = 0 To Me.mProblem.NumParams - 1
            Me.mProblem.List_OptParameter(i) = Me.mOptPara(i)
            Me.mProblem.List_OptParameter_Save(i) = Me.mOptPara(i)
        Next

    End Sub

#Region "Diagrammfunktionen"

    'Diagramm initialisieren
    '***********************
    Public Sub DiagInitialise(ByRef Diag As EVO.Diagramm.Hauptdiagramm)

        Select Case Me.selectedTestproblem

            Case TP_SinusFunktion
                Call Me.DiagInitialise_SinusFunktion(Diag)

            Case TP_BealeProblem 'x1 = [-5;5], x2=[-2;2]
                Call Me.DiagInitialise_BealeProblem(Diag)

            Case TP_Schwefel24Problem 'xi = [-10,10]
                Call Me.DiagInitialise_SchwefelProblem(Diag)

            Case TP_Box
                Call Me.DiagInitialise_3D_Box(Diag)

            Case TP_Abh�ngigeParameter
                Call Me.DiagInitialise_AbhParameter(Diag)

            Case Else
                Call Me.DiagInitialise_MultiTestProb(Diag)

        End Select

    End Sub

    'Diagramm f�r Sinus-Funktion initialisieren
    '*******************************************
    Private Sub DiagInitialise_SinusFunktion(ByRef Diag As EVO.Diagramm.Hauptdiagramm)

        Dim array_x() As Double
        Dim array_y() As Double
        Dim i As Integer
        Dim Unterteilung_X As Double
        Dim serie As Steema.TeeChart.Styles.Series
        Dim achsen As Collection
        Dim achse As EVO.Diagramm.Diagramm.Achse

        'Achsen vorbereiten
        '------------------
        achsen = New Collection()

        'X-Achse
        achse.Title = "X-Wert"
        achse.Automatic = False
        achse.Minimum = 0
        achse.Maximum = 2 * Math.PI
        achse.Increment = Math.PI
        achsen.Add(achse)

        'Y-Achse
        achse.Title = "Y-Wert"
        achse.Automatic = False
        achse.Minimum = -1
        achse.Maximum = 1
        achse.Increment = 0.2
        achsen.Add(achse)

        'Diagramm initialisieren
        '-----------------------
        Call Diag.DiagInitialise("Sinus Funktion", achsen, Me.mProblem)

        'Sinuslinie zeichnen
        '-------------------
        Unterteilung_X = 2 * Math.PI / (Me.mAnzParameter - 1)

        ReDim array_x(Me.mAnzParameter - 1)
        ReDim array_y(Me.mAnzParameter - 1)

        For i = 0 To Me.mAnzParameter - 1
            array_x(i) = Math.Round(i * Unterteilung_X, 2)
            array_y(i) = Math.Sin(i * Unterteilung_X)
        Next i

        serie = Diag.getSeriesLine("Sinusfunktion", "Green")
        Call serie.Add(array_x, array_y)

    End Sub

    'Diagramm f�r Beale-Problem initialisieren
    '*****************************************
    Private Sub DiagInitialise_BealeProblem(ByRef Diag As EVO.Diagramm.Hauptdiagramm)

        Dim Ausgangswert As Double
        Dim colorline1 As Steema.TeeChart.Tools.ColorLine
        Dim achsen As Collection
        Dim achse As EVO.Diagramm.Diagramm.Achse

        'Ausgangswert berechnen
        Ausgangswert = (1.5 - 0.5 * (1 - 0.5)) ^ 2 + (2.25 - 0.5 * (1 - 0.5) ^ 2) ^ 2 + (2.625 - 0.5 * (1 - 0.5) ^ 3) ^ 2

        'Achsen
        '------
        achsen = New Collection()

        'X-Achse
        achse.Title = "Berechnungsschritt"
        achse.Automatic = True
        achse.Minimum = 0
        Call achsen.Add(achse)

        'Y-Achse
        achse.Title = "Funktionswert"
        achse.Automatic = False
        achse.Minimum = 0
        achse.Maximum = Ausgangswert * 1.3
        Call achsen.Add(achse)

        Call Diag.DiagInitialise("Beale Problem", achsen, Me.mProblem)

        'Linie f�r den Ausgangswert anzeigen
        colorline1 = New Steema.TeeChart.Tools.ColorLine(Diag.Chart)
        colorline1.AllowDrag = False
        colorline1.Axis = Diag.Axes.Left
        colorline1.Value = Ausgangswert
        colorline1.Pen.Color = Drawing.Color.Green

    End Sub

    'Diagramm f�r Schwefel-Problem initialisieren
    '********************************************
    Private Sub DiagInitialise_SchwefelProblem(ByRef Diag As EVO.Diagramm.Hauptdiagramm)

        Dim Ausgangswert As Double
        Dim i As Integer
        Dim X() As Double
        Dim colorline1 As Steema.TeeChart.Tools.ColorLine
        Dim achsen As Collection
        Dim achse As EVO.Diagramm.Diagramm.Achse

        'Ausgangswert berechnen
        ReDim X(Me.mAnzParameter)
        For i = 1 To Me.mAnzParameter
            X(i) = 10
        Next i
        Ausgangswert = 0
        For i = 1 To Me.mAnzParameter
            Ausgangswert += ((X(1) - X(i) ^ 2) ^ 2 + (X(i) - 1) ^ 2)
        Next i

        'Achsen
        '------
        achsen = New Collection()

        'X-Achse
        achse.Title = "Berechnungsschritt"
        achse.Automatic = True
        achse.Minimum = 0
        Call achsen.Add(achse)

        'Y-Achse
        achse.Title = "Funktionswert"
        achse.Automatic = False
        achse.Minimum = 0
        achse.Maximum = Ausgangswert * 1.3
        Call achsen.Add(achse)

        Call Diag.DiagInitialise("Schwefel 2.4 Problem", achsen, Me.mProblem)

        'Linie f�r den Ausgangswert anzeigen
        colorline1 = New Steema.TeeChart.Tools.ColorLine(Diag.Chart)
        colorline1.AllowDrag = False
        colorline1.Axis = Diag.Axes.Left
        colorline1.Value = Ausgangswert
        colorline1.Pen.Color = Drawing.Color.Red

    End Sub

    'Diagramm f�r MultiObjective-Probleme initialisieren
    '***************************************************
    Private Sub DiagInitialise_MultiTestProb(ByRef Diag As EVO.Diagramm.Hauptdiagramm)

        Dim i, j As Short
        Dim title As String
        Dim serie As Steema.TeeChart.Styles.Series
        Dim achsen As Collection
        Dim xachse, yachse As EVO.Diagramm.Diagramm.Achse

        title = "Testproblem"

        'Achsen
        '------
        achsen = New Collection()

        'X-Achse
        xachse.Title = "X"
        xachse.Automatic = False
        xachse.Minimum = 0
        xachse.Maximum = 1
        xachse.Increment = 0.1

        'Y-Achse
        yachse.Title = "Y"
        yachse.Automatic = False
        yachse.Minimum = 0
        yachse.Maximum = 1
        yachse.Increment = 0.1

        'Problemspezifische Anpassungen
        Select Case Me.selectedTestproblem

            Case TP_Deb1
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                title = "Deb D1 - MO-konvex"
                yachse.Automatic = True

            Case TP_ZitzlerDebT1
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                title = "Zitzler/Deb/Theile T1"
                yachse.Maximum = 7
                yachse.Increment = 0.5

            Case TP_ZitzlerDebT2
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                title = "Zitzler/Deb/Theile T2"
                yachse.Maximum = 7

            Case TP_ZitzlerDebT3
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                title = "Zitzler/Deb/Theile T3"
                xachse.Increment = 0.2
                yachse.Maximum = 7
                yachse.Minimum = -1
                yachse.Increment = 0.5

            Case TP_ZitzlerDebT4
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                title = "Zitzler/Deb/Theile T4"
                xachse.Automatic = True
                yachse.Automatic = True

            Case TP_CONSTR
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                title = TP_CONSTR
                yachse.Maximum = 15

            Case TP_FloodMitigation
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                title = "Flood mitigation ajay"

                yachse.Maximum = -15000000
                yachse.Minimum = -55000000

                xachse.Maximum = 10000000
                xachse.Minimum = -15000000

        End Select

        Call achsen.Add(xachse)
        Call achsen.Add(yachse)

        'Diagramm initialisieren
        Call Diag.DiagInitialise(title, achsen, Me.mProblem)

        'Problemspezifische Serien zeichnen
        '----------------------------------
        Select Case Me.selectedTestproblem

            Case TP_Deb1
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim Array1X(100) As Double
                Dim Array1Y(100) As Double
                Dim Array2X(100) As Double
                Dim Array2Y(100) As Double

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


            Case TP_ZitzlerDebT1
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim ArrayX(1000) As Double
                Dim ArrayY(1000) As Double

                'Paretofront berechnen und zeichnen
                For j = 0 To 1000
                    ArrayX(j) = j / 1000
                    ArrayY(j) = 1 - Math.Sqrt(ArrayX(j))
                Next j
                serie = Diag.getSeriesLine("Paretofront", "Green")
                serie.Add(ArrayX, ArrayY)


            Case TP_ZitzlerDebT2
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim ArrayX(100) As Double
                Dim ArrayY(100) As Double

                'Paretofront berechnen und zeichnen
                For j = 0 To 100
                    ArrayX(j) = j / 100
                    ArrayY(j) = 1 - (ArrayX(j) * ArrayX(j))
                Next j
                serie = Diag.getSeriesLine("Paretofront", "Green")
                serie.Add(ArrayX, ArrayY)


            Case TP_ZitzlerDebT3
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                'TODO: Titel der Serien (f�r Export)
                Dim ArrayX(100) As Double
                Dim ArrayY(100) As Double

                'Paretofront berechnen und zeichnen
                For j = 0 To 100
                    ArrayX(j) = j / 100
                    ArrayY(j) = 1 - Math.Sqrt(ArrayX(j)) - ArrayX(j) * Math.Sin(10 * Math.PI * ArrayX(j))
                Next j
                serie = Diag.getSeriesLine("Paretofront", "Green")
                serie.Add(ArrayX, ArrayY)


            Case TP_ZitzlerDebT4
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim ArrayX(1000) As Double
                Dim ArrayY(1000) As Double

                'Lokale Optima berechnen und zeichnen
                For i = 1 To 10
                    For j = 0 To 1000
                        ArrayX(j) = j / 1000
                        ArrayY(j) = (1 + (i - 1) / 4) * (1 - Math.Sqrt(ArrayX(j) / (1 + (i - 1) / 4)))
                    Next
                    serie = Diag.getSeriesLine("Lokales Optimum " & i)
                    serie.Add(ArrayX, ArrayY)
                Next


            Case TP_CONSTR
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim Array1X(100) As Double
                Dim Array1Y(100) As Double
                Dim Array2X(100) As Double
                Dim Array2Y(100) As Double
                Dim Array3X(61) As Double
                Dim Array3Y(61) As Double
                Dim Array4X(61) As Double
                Dim Array4Y(61) As Double

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

            Case TP_FloodMitigation
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                'keine Serien

        End Select

    End Sub

    'Diagramm f�r Box-Problem (3D) initialisieren
    '********************************************
    Private Sub DiagInitialise_3D_Box(ByRef Diag As EVO.Diagramm.Hauptdiagramm)

        Dim i, j, n As Integer
        Dim ArrayX() As Double
        Dim ArrayY() As Double
        Dim ArrayZ() As Double
        Dim achsen As Collection
        Dim achse As EVO.Diagramm.Diagramm.Achse

        'Achsen
        '------
        achsen = New Collection()

        'X-Achse
        achse.Title = "X"
        achse.Automatic = False
        achse.Minimum = 0
        achse.Maximum = 1
        achse.Increment = 0.2
        Call achsen.Add(achse)

        'Y-Achse
        achse.Title = "Y"
        achse.Automatic = False
        achse.Minimum = 0
        achse.Maximum = 1
        achse.Increment = 0.2
        Call achsen.Add(achse)

        'Z-Achse
        achse.Title = "Z"
        achse.Automatic = False
        achse.Minimum = 0
        achse.Maximum = 1
        achse.Increment = 0.2
        Call achsen.Add(achse)

        'Diagramm initialisieren
        Call Diag.DiagInitialise(TP_Box, achsen, Me.mProblem)

        'Serien
        '------
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
        surface.Brush.Color = System.Drawing.Color.Green
        surface.Brush.Transparency = 70
        surface.Pen.Color = System.Drawing.Color.Green
        surface.SideBrush.Visible = True
        surface.SideBrush.Color = System.Drawing.Color.Red
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
        surface.Brush.Color = System.Drawing.Color.Blue
        surface.Brush.Transparency = 70
        surface.Pen.Color = System.Drawing.Color.Blue
        surface.SideBrush.Visible = True
        surface.SideBrush.Color = System.Drawing.Color.Red
        surface.SideBrush.Transparency = 70

        'Schnittgerade zwischen den Constraints
        series3D = New Steema.TeeChart.Styles.Points3D(Diag.Chart)
        series3D.Title = "Schnittgerade"
        series3D.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Nothing
        series3D.LinePen.Visible = True
        series3D.LinePen.Width = 1
        series3D.LinePen.Color = System.Drawing.Color.Red
        series3D.Add(0.5, 0, 0.3)
        series3D.Add(0, 0.5, 0.3)

    End Sub


    'Diagramm f�r Abh�ngige Parameter initialisieren
    '***********************************************
    Private Sub DiagInitialise_AbhParameter(ByRef Diag As EVO.Diagramm.Hauptdiagramm)

        Dim i, j, n As Integer
        Dim ArrayX() As Double
        Dim ArrayY() As Double
        Dim ArrayZ() As Double
        Const surfaceRes As Integer = 11
        Dim surface As Steema.TeeChart.Styles.Surface
        Dim achsen As Collection
        Dim achse As EVO.Diagramm.Diagramm.Achse

        'Achsen
        '------
        achsen = New Collection()

        'X-Achse
        achse.Title = "X"
        achse.Automatic = False
        achse.Minimum = 0
        achse.Maximum = 1
        achse.Increment = 0.2
        Call achsen.Add(achse)

        'Y-Achse
        achse.Title = "Y"
        achse.Automatic = False
        achse.Minimum = 0
        achse.Maximum = 1
        achse.Increment = 0.2
        Call achsen.Add(achse)

        'Z-Achse
        achse.Title = "Zielfunktion"
        achse.Automatic = False
        achse.Minimum = 0
        achse.Maximum = 2
        achse.Increment = 0.5
        Call achsen.Add(achse)

        'Diagramm initialisieren
        Call Diag.DiagInitialise(TP_Abh�ngigeParameter, achsen, Me.mProblem)

        'Serien
        '------

        'Ebene x = y
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
        surface.Brush.Color = System.Drawing.Color.Green
        surface.Brush.Transparency = 70
        surface.Pen.Color = System.Drawing.Color.Green
        surface.SideBrush.Visible = True
        surface.SideBrush.Color = System.Drawing.Color.Red
        surface.SideBrush.Transparency = 70

    End Sub

#End Region 'Diagrammfunktionen

#Region "Evaluierung"

    'Evaluierung und Zeichnen der Testprobleme
    '*****************************************
    Public Sub Evaluierung_TestProbleme_MetaEvo(ByRef ind_input As Common.Individuum_MetaEvo, ByVal ipop As Short, ByRef Diag As EVO.Diagramm.Hauptdiagramm)
        'Umbauen
        Dim ind As Common.Individuum_PES
        ind = New Common.Individuum_PES("MetaEvo", ind_input.ID)
        ind.OptParameter = ind_input.get_mOptparas

        Evaluate(ind, ipop, Diag)

        ind_input.Features = ind.Features
        ind_input.Constraints = ind.Constraints
        If (ind.Dominated = "true") Then ind_input.set_status("false")
    End Sub

    ''' <summary>
    ''' Evaluiert (und zeichnet!) das Testproblem
    ''' </summary>
    ''' <param name="ind">das zu evaluierende Individuum</param>
    ''' <param name="ipop">Populationsnummer (0-basiert)</param>
    ''' <param name="Diag">Referenz auf das Hauptdiagramm</param>
    ''' <remarks></remarks>
    Public Sub Evaluate(ByRef ind As Common.Individuum, ByVal ipop As Short, ByRef Diag As EVO.Diagramm.Hauptdiagramm)

        Dim i As Integer
        Dim Unterteilung_X As Double
        Dim x1, x2 As Double
        Dim X() As Double
        Dim f1, f2 As Double
        Dim g1, g2 As Double
        Dim globalAnzPar As Integer = Me.mProblem.NumParams
        Dim serie As Steema.TeeChart.Styles.Series

        Select Case Me.selectedTestproblem

            '*************************************
            '* Single-Objective Problemstellungen *
            '*************************************

            Case TP_SinusFunktion
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Fehlerquadrate zur Sinusfunktion |0-2pi|
                '----------------------------------------
                Unterteilung_X = 2 * Math.PI / (globalAnzPar - 1)

                ind.Features(0) = 0
                For i = 0 To globalAnzPar - 1
                    ind.Features(0) += (Math.Sin(i * Unterteilung_X) - (-1 + (ind.OptParameter(i).Xn * 2))) ^ 2
                Next i

                'Zeichnen
                '--------
                Dim array_x() As Double = {}
                Dim array_y() As Double = {}

                ReDim array_x(globalAnzPar - 1)
                ReDim array_y(globalAnzPar - 1)
                For i = 0 To globalAnzPar - 1
                    array_x(i) = Math.Round(i * Unterteilung_X, 2)
                    array_y(i) = (-1 + ind.OptParameter(i).Xn * 2)
                Next i

                serie = Diag.getSeriesPoint("Population " & ipop + 1)
                serie.Add(array_x, array_y)


            Case TP_BealeProblem
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualit�tswert berechnen
                '-----------------------
                x1 = -5 + (ind.OptParameter(0).Xn * 10)
                x2 = -2 + (ind.OptParameter(1).Xn * 4)

                ind.Features(0) = (1.5 - x1 * (1 - x2)) ^ 2 + (2.25 - x1 * (1 - x2) ^ 2) ^ 2 + (2.625 - x1 * (1 - x2) ^ 3) ^ 2

                'Zeichnen
                '--------
                serie = Diag.getSeriesPoint("Population " & ipop + 1)
                serie.Add(ind.ID, ind.Features(0))

            Case TP_Schwefel24Problem
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualit�tswert berechnen
                '-----------------------
                ReDim X(globalAnzPar - 1)
                For i = 0 To globalAnzPar - 1
                    X(i) = -10 + ind.OptParameter(i).Xn * 20
                Next i
                ind.Features(0) = 0
                For i = 0 To globalAnzPar - 1
                    ind.Features(0) += ((X(0) - X(i) ^ 2) ^ 2 + (X(i) - 1) ^ 2)
                Next i

                'Zeichnen
                '--------
                serie = Diag.getSeriesPoint("Population " & ipop + 1)
                serie.Add(ind.ID, ind.Features(0))

                '*************************************
                '* Multi-Objective Problemstellungen *
                '*************************************

            Case TP_Deb1 'Deb 2000, D1 (Konvexe Pareto-Front)
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualit�tswert berechnen
                '-----------------------
                ind.Features(0) = ind.OptParameter(0).Xn * (9 / 10) + 0.1
                ind.Features(1) = (1 + 5 * ind.OptParameter(1).Xn) / (ind.OptParameter(0).Xn * (9 / 10) + 0.1)

                'Zeichnen
                '--------
                serie = Diag.getSeriesPoint("Population", "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                serie.Add(ind.Features(0), ind.Features(1))

            Case TP_ZitzlerDebT1 'Zitzler/Deb/Thiele 2000, T1 (Konvexe Pareto-Front)
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualit�tswert berechnen
                '-----------------------
                f1 = ind.OptParameter(0).Xn
                f2 = 0
                For i = 1 To globalAnzPar - 1
                    f2 = f2 + ind.OptParameter(i).Xn
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - System.Math.Sqrt(f1 / f2))
                ind.Features(0) = f1
                ind.Features(1) = f2

                'Zeichnen
                '--------
                serie = Diag.getSeriesPoint("Population", "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                serie.Add(ind.Features(0), ind.Features(1))

            Case TP_ZitzlerDebT2 'Zitzler/Deb/Thiele 2000, T2 (Non-Konvexe Pareto-Front)
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualit�tswerte berechnen
                '------------------------
                f1 = ind.OptParameter(0).Xn
                f2 = 0
                For i = 1 To globalAnzPar - 1
                    f2 = f2 + ind.OptParameter(i).Xn
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - (f1 / f2) * (f1 / f2))
                ind.Features(0) = f1
                ind.Features(1) = f2

                'Zeichnen
                '--------
                serie = Diag.getSeriesPoint("Population", "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                serie.Add(ind.Features(0), ind.Features(1))

            Case TP_ZitzlerDebT3 'Zitzler/Deb/Thiele 2000, T3 (disconected Pareto-Front)
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualit�tswerte berechnen
                '------------------------
                f1 = ind.OptParameter(0).Xn
                f2 = 0
                For i = 1 To globalAnzPar - 1
                    f2 = f2 + ind.OptParameter(i).Xn
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - Math.Sqrt(f1 / f2) - (f1 / f2) * Math.Sin(10 * Math.PI * f1))
                ind.Features(0) = f1
                ind.Features(1) = f2

                'Zeichnen
                '--------
                serie = Diag.getSeriesPoint("Population", "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                serie.Add(ind.Features(0), ind.Features(1))

            Case TP_ZitzlerDebT4 'Zitzler/Deb/Thiele 2000, T4 (local/global Pareto-Fronts)
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualit�tswerte berechnen
                '------------------------
                f1 = ind.OptParameter(0).Xn
                f2 = 0
                For i = 1 To globalAnzPar - 1
                    x2 = -5 + (ind.OptParameter(i).Xn * 10)
                    f2 = f2 + (x2 * x2 - 10 * Math.Cos(4 * Math.PI * x2))
                Next i
                f2 = 1 + 10 * (globalAnzPar - 1) + f2
                f2 = f2 * (1 - System.Math.Sqrt(f1 / f2))
                ind.Features(0) = f1
                ind.Features(1) = f2

                'Zeichnen
                '--------
                serie = Diag.getSeriesPoint("Population", "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                serie.Add(ind.Features(0), ind.Features(1))

            Case TP_CONSTR
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualit�tswerte berechnen
                '------------------------
                f1 = ind.OptParameter(0).Xn * (9 / 10) + 0.1
                f2 = (1 + 5 * ind.OptParameter(1).Xn) / (ind.OptParameter(0).Xn * (9 / 10) + 0.1)

                ind.Features(0) = f1
                ind.Features(1) = f2

                'Constraints berechnen
                '---------------------
                g1 = (5 * ind.OptParameter(1).Xn) + 9 * (ind.OptParameter(0).Xn * (9 / 10) + 0.1) - 6
                g2 = (-1) * (5 * ind.OptParameter(1).Xn) + 9 * (ind.OptParameter(0).Xn * (9 / 10) + 0.1) - 1

                ind.Constraints(0) = g1
                ind.Constraints(1) = g2

                'Zeichnen
                '--------
                If (Not ind.Is_Feasible) Then
                    'Ung�ltige L�sung
                    serie = Diag.getSeriesPoint("Population (ung�ltig)", "Gray", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                Else
                    'G�ltige L�sung
                    serie = Diag.getSeriesPoint("Population", "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                End If
                serie.Add(ind.Features(0), ind.Features(1))

            Case TP_Box
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualit�tswerte berechnen
                '------------------------
                ind.Features(0) = ind.OptParameter(0).Xn
                ind.Features(1) = ind.OptParameter(1).Xn
                ind.Features(2) = ind.OptParameter(2).Xn

                'Constraints berechnen
                '---------------------
                ind.Constraints(0) = ind.OptParameter(0).Xn + ind.OptParameter(1).Xn - 0.5
                ind.Constraints(1) = ind.OptParameter(0).Xn + ind.OptParameter(1).Xn + ind.OptParameter(2).Xn - 0.8

                'Zeichnen
                '--------
                Dim serie3D As Steema.TeeChart.Styles.Points3D
                If (Not ind.Is_Feasible) Then
                    'Ung�ltige L�sung
                    serie3D = Diag.getSeries3DPoint("Population (ung�ltig)", "Gray")
                Else
                    'G�ltige L�sung
                    serie3D = Diag.getSeries3DPoint("Population", "Orange")
                End If
                serie3D.Add(ind.Features(0), ind.Features(1), ind.Features(2))

            Case TP_Abh�ngigeParameter
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Qualit�tswerte berechnen
                '------------------------
                ind.Features(0) = ind.OptParameter(0).Xn ^ 2 + ind.OptParameter(1).Xn ^ 2

                'Zeichnen
                '--------
                Dim serie3D As Steema.TeeChart.Styles.Points3D
                serie3D = Diag.getSeries3DPoint("Population " & ipop + 1)
                serie3D.Add(ind.OptParameter(0).Xn, ind.OptParameter(1).Xn, ind.Features(0))

            Case TP_FloodMitigation
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Getting the new Parameters
                ReDim X(7)
                For i = 0 To X.GetUpperBound(0)
                    X(i) = ind.OptParameter(i).RWert
                Next

                'Calculating the Objective Function
                '----------------------------------
                Dim Storage As Double
                Storage = 650000
                'float sconst=650000;

                Dim p() As Double = {9449568.0, 9069713.044, 2441388.773, 1556876.392}
                'double p[4] = { 9449568.000,9069713.044,2441388.773,1556876.392};
                f1 = 0
                f2 = 0

                'Objective Function 1 and 2
                f1 = -((p(0) - X(4)) - (X(0) - X(1)))
                f2 = -(0.09651 * (((8.0E-22 * Math.Pow(X(0), 3)) - (0.00000000000008 * Math.Pow(X(0), 2)) + (0.000003 * X(0)) + 6.2034) * X(4)))

                f1 = f1 - ((p(1) - X(5)) - (X(1) - X(2)))
                f2 = f2 - (0.09651 * (((8.0E-22 * Math.Pow(X(1), 3)) - (0.00000000000008 * Math.Pow(X(1), 2)) + (0.000003 * X(1)) + 6.2034) * X(5)))

                f1 = f1 - ((p(2) - X(6)) - (X(2) - X(3)))
                f2 = f2 - (0.09651 * (((8.0E-22 * Math.Pow(X(2), 3)) - (0.00000000000008 * Math.Pow(X(2), 2)) + (0.000003 * X(2)) + 6.2034) * X(6)))

                f1 = f1 - ((p(3) - X(7)) - (X(3) - X(4)))
                f2 = f2 - (0.09651 * (((8.0E-22 * Math.Pow(X(3), 3)) - (0.00000000000008 * Math.Pow(X(3), 2)) + (0.000003 * X(3)) + 6.2034) * X(7)))

                'Constraints
                '-----------
                Dim contrain(3) As Double
                contrain(0) = (X(0) - Storage - p(0) + X(4))
                contrain(1) = (X(1) - X(0) - p(1) + X(5))
                contrain(2) = (X(2) - X(1) - p(2) + X(6))
                contrain(3) = (X(3) - X(2) - p(3) + X(7))

                'Give Back the Penalties and Constraints
                ind.Features(0) = f1
                ind.Features(1) = f2
                ind.Constraints(0) = contrain(0)
                ind.Constraints(1) = contrain(1)
                ind.Constraints(2) = contrain(2)
                ind.Constraints(3) = contrain(3)

                'Drawing
                '--------

                If ind.Is_Feasible Then
                    serie = Diag.getSeriesPoint("Population", "Orange")
                Else
                    serie = Diag.getSeriesPoint("Population (ung�ltig)", "Gray")
                End If
                serie.Add(ind.Features(0), ind.Features(1))

        End Select
    End Sub

#End Region 'Evaluierung

End Class
