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
    Public Sub Parameter_Uebergabe(ByVal Testproblem As String, ByVal globAnzPar_Sin As String, ByVal globAnzPar_Schw As String, ByRef globalAnzPar As Short, ByRef globalAnzZiel As Short, ByRef globalAnzRand As Short, ByRef mypara() As Double)

        Dim i As Integer

        Select Case Testproblem
            Case "Sinus-Funktion"
                globalAnzPar = CShort(globAnzPar_Sin)
                globalAnzZiel = 1
                globalAnzRand = 0
                ReDim mypara(globalAnzPar)
                For i = 1 To globalAnzPar
                    mypara(i) = 0
                Next
            Case "Beale-Problem" 'x1 = [-5;5], x2=[-2;2]
                globalAnzPar = 2
                globalAnzZiel = 1
                globalAnzRand = 0
                ReDim mypara(globalAnzPar)
                mypara(1) = 0.5
                mypara(2) = 0.5
            Case "Schwefel 2.4-Problem" 'xi = [-10,10]
                globalAnzPar = CShort(globAnzPar_Schw)
                globalAnzZiel = 1
                globalAnzRand = 0
                ReDim mypara(globalAnzPar)
                For i = 1 To globalAnzPar
                    mypara(i) = 1
                Next i
            Case "Deb 1" 'x1 = [0.1;1], x2=[0;5]
                globalAnzPar = 2
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar)
                Randomize()
                mypara(1) = Rnd()
                mypara(2) = Rnd()
            Case "Zitzler/Deb T1" 'xi = [0,1]
                globalAnzPar = 30
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar)
                Randomize()
                For i = 1 To globalAnzPar
                    mypara(i) = Rnd()
                Next i
            Case "Zitzler/Deb T2" 'xi = [0,1]
                globalAnzPar = 30
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar)
                Randomize()
                For i = 1 To globalAnzPar
                    mypara(i) = Rnd()
                Next i
            Case "Zitzler/Deb T3" 'xi = [0,1]
                globalAnzPar = 15
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar)
                Randomize()
                For i = 1 To globalAnzPar
                    mypara(i) = Rnd()
                Next i
            Case "Zitzler/Deb T4" 'x1 = [0,1], xi=[-5,5]
                globalAnzPar = 10
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar)
                Randomize()
                For i = 1 To globalAnzPar
                    mypara(i) = Rnd()
                Next i
            Case "CONSTR" 'x1 = [0.1;1], x2=[0;5]
                globalAnzPar = 2
                globalAnzZiel = 2
                globalAnzRand = 2
                ReDim mypara(globalAnzPar)
                Randomize()
                mypara(1) = Rnd()
                mypara(2) = Rnd()
            Case "Box"
                globalAnzPar = 3
                globalAnzZiel = 3
                globalAnzRand = 2
                ReDim mypara(globalAnzPar)
                Randomize()
                mypara(1) = Rnd()
                mypara(2) = Rnd()
                mypara(3) = Rnd()
        End Select
    End Sub


    '*****************************************************************'******************
    '                      Evaluierung der Testprobleme                                 *
    '************************************************************************************

    Public Sub Evaluierung_TestProbleme(ByRef Testproblem As String, ByVal mypara() As Double, ByVal durchlauf As Integer, ByVal ipop As Short, ByRef QN() As Double, ByRef RN() As Double, ByRef Diag As Steema.TeeChart.TChart)

        Dim i As Short
        Dim Unterteilung_X As Double
        Dim x1, x2 As Double
        Dim X() As Double
        Dim f2, f1, f3 As Double
        Dim g1, g2 As Double
        Dim globalAnzPar as Short = UBound(mypara)

        '* Single-Objective Problemstellungen *
        Select Case Testproblem

            Case "Sinus-Funktion" 'Fehlerquadrate zur Sinusfunktion |0-2pi|
                Unterteilung_X = 2 * 3.1415926535898 / (globalAnzPar - 1)
                QN(1) = 0
                For i = 1 To globalAnzPar
                    QN(1) = QN(1) + (System.Math.Sin((i - 1) * Unterteilung_X) - (-1 + (mypara(i) * 2))) * (System.Math.Sin((i - 1) * Unterteilung_X) - (-1 + mypara(i) * 2))
                Next i
                Call Zielfunktion_zeichnen_Sinus(ipop, globalAnzPar, mypara, Diag)
            Case "Beale-Problem" 'Beale-Problem
                x1 = -5 + (mypara(1) * 10)
                x2 = -2 + (mypara(2) * 4)

                QN(1) = (1.5 - x1 * (1 - x2)) ^ 2 + (2.25 - x1 * (1 - x2) ^ 2) ^ 2 + (2.625 - x1 * (1 - x2) ^ 3) ^ 2
                Diag.Series(ipop).Add(durchlauf, QN(1))
            Case "Schwefel 2.4-Problem" 'Schwefel 2.4 S. 329
                ReDim X(globalAnzPar)
                For i = 1 To globalAnzPar
                    X(i) = -10 + mypara(i) * 20
                Next i
                QN(1) = 0
                For i = 1 To globalAnzPar
                    QN(1) = QN(1) + ((X(1) - X(i) ^ 2) ^ 2 + (X(i) - 1) ^ 2)
                Next i
                Diag.Series(ipop).Add(durchlauf, QN(1))
                '*************************************
                '* Multi-Objective Problemstellungen *
                '*************************************
                'Deb 2000, D1 (Konvexe Pareto-Front)
            Case "Deb 1"
                f1 = mypara(1) * (9 / 10) + 0.1
                f2 = (1 + 5 * mypara(2)) / (mypara(1) * (9 / 10) + 0.1)
                QN(1) = f1
                QN(2) = f2
                Diag.Series(0).Add(f1, f2, "")

                'Zitzler/Deb/Thiele 2000, T1 (Konvexe Pareto-Front)
            Case "Zitzler/Deb T1"
                f1 = mypara(1)
                f2 = 0
                For i = 2 To globalAnzPar
                    f2 = f2 + mypara(i)
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - System.Math.Sqrt(f1 / f2))
                QN(1) = f1
                QN(2) = f2
                Diag.Series(0).Add(f1, f2, "")

                'Zitzler/Deb/Thiele 2000, T2 (Non-Konvexe Pareto-Front)
            Case "Zitzler/Deb T2"
                f1 = mypara(1)
                f2 = 0
                For i = 2 To globalAnzPar
                    f2 = f2 + mypara(i)
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - (f1 / f2) * (f1 / f2))
                QN(1) = f1
                QN(2) = f2
                Diag.Series(0).Add(f1, f2, "")

                'Zitzler/Deb/Thiele 2000, T3 (disconected Pareto-Front)
            Case "Zitzler/Deb T3"
                f1 = mypara(1)
                f2 = 0
                For i = 2 To globalAnzPar
                    f2 = f2 + mypara(i)
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - System.Math.Sqrt(f1 / f2) - (f1 / f2) * System.Math.Sin(10 * 3.14159265358979 * f1))
                QN(1) = f1
                QN(2) = f2
                Diag.Series(0).Add(f1, f2, "")

                'Zitzler/Deb/Thiele 2000, T4 (local/global Pareto-Fronts)
            Case "Zitzler/Deb T4"
                f1 = mypara(1)
                f2 = 0
                For i = 2 To globalAnzPar
                    x2 = -5 + (mypara(i) * 10)
                    f2 = f2 + (x2 * x2 - 10 * System.Math.Cos(4 * 3.14159265358979 * x2))
                Next i
                f2 = 1 + 10 * (globalAnzPar - 1) + f2
                f2 = f2 * (1 - System.Math.Sqrt(f1 / f2))
                QN(1) = f1
                QN(2) = f2
                Diag.Series(0).Add(f1, f2, "")

            Case "CONSTR"
                f1 = mypara(1) * (9 / 10) + 0.1
                f2 = (1 + 5 * mypara(2)) / (mypara(1) * (9 / 10) + 0.1)

                g1 = (5 * mypara(2)) + 9 * (mypara(1) * (9 / 10) + 0.1) - 6
                g2 = (-1) * (5 * mypara(2)) + 9 * (mypara(1) * (9 / 10) + 0.1) - 1

                QN(1) = f1
                QN(2) = f2
                RN(1) = g1
                RN(2) = g2
                Diag.Series(0).Add(f1, f2, "")

            Case "Box"
                f1 = mypara(1) ^ 2
                f2 = mypara(2) ^ 2
                f3 = mypara(3) ^ 2
                g1 = mypara(1) + mypara(3) - 0.5
                g2 = mypara(1) + mypara(2) + mypara(3) - 0.8

                'f1 = 1 + (1 - Par(1, 1)) ^ 5
                'f2 = Par(2, 1)
                'f3 = Par(3, 1)
                '
                'g1 = Par(1, 1) ^ 2 + Par(3, 1) ^ 2 - 0.5
                'g2 = Par(2, 1) ^ 2 + Par(3, 1) ^ 2 - 0.5

                QN(1) = f1
                QN(2) = f2
                QN(3) = f3
                RN(1) = g1
                RN(2) = g2
                Call Zielfunktion_zeichnen_MultiObPar_3D(f1, f2, f3, Diag)
        End Select

    End Sub

    '************************************************************************************
    '                          Zeichenfunktionen                                        *
    '************************************************************************************

    Public Sub Zielfunktion_zeichnen_Sinus(ByVal ipop As Short, ByVal AnzPar As Short, ByVal mypara() As Double, ByRef TChart1 As Steema.TeeChart.TChart)
        Dim i As Short
        Dim Unterteilung_X As Double
        Dim array_x() As Double = {}
        Dim array_y() As Double = {}

        Unterteilung_X = 2 * 3.141592654 / (AnzPar - 1)
        ReDim array_x(AnzPar - 1)
        ReDim array_y(AnzPar - 1)
        For i = 0 To AnzPar - 1
            array_x(i) = System.Math.Round((i) * Unterteilung_X, 2)
            array_y(i) = (-1 + mypara(i + 1) * 2)
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
