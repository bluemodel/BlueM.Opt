' Copyright (c) 2011, ihwb, TU Darmstadt
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
Imports System.IO

Public Class TSP

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse TSP Traveling Salesman Problem                                 ****
    '****                                                                       ****
    '**** Autor: Christoph Hübner                                               ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '****                                                                       ****
    '**** Februar 2007                                                          ****
    '****                                                                       ****
    '**** Letzte Änderung: März 2007                                            ****
    '*******************************************************************************
    '*******************************************************************************

    '******* Konvention *******
    'Cities:      1 2 3 4 5 6 7
    'Pathindex:   0 1 2 3 4 5 6
    'CutPoint:     0 1 2 3 4 5
    'LB + UB(n-2)  x         x
    '**************************

    'PNG Export exportiert alle 100 Generationen png Bilder
    Public pngExport As Boolean = True
    'Standardmäig unter den bin Verzeichnis
    Public ExPath As String

    'Batch_Mode
    Public Mode As EnMode = EnMode.Standard_Opt
    'Anzahl der Tests
    Public nTests As Integer = 3

    Enum EnMode
        Standard_Opt = 1
        Batch_OPpt = 2
        Just_Calc = 3
    End Enum

    'Settings
    Private mySettings As Common.Settings_TSP

    Public ListOfCities(,) As Object

    Public circumference As Double 'Kreisumfang

    'Anzahl der SubPaths bei beim n_Opt 0perator
    Dim n_SP As Integer = 2

    '************************************* Struktur *****************************
    Public Structure Faksimile_Type
        Dim No As Integer
        Dim Path() As Integer
        Dim Penalty As Double
        Dim Image(,) As Object
    End Structure

    '************************************* Listen ******************************
    Public ChildrenList() As Faksimile_Type = {}
    Public ParentList() As Faksimile_Type = {}

    '******************************** Initialisierung *************************************

    Public Sub TSP_Initialize(ByRef mySettingsInput As Common.Settings_TSP)

        mySettings = mySettingsInput

        Dim i As Integer

        ReDim ListOfCities(mySettings.N_Cities - 1, 2)
        'Dim Problem As String = "Circle"

        Randomize()

        Select Case mySettings.Problem

            Case Common.EnProblem.circle
                Dim Radius As Integer = 45
                Dim factor As Double = (Math.PI * 2) / mySettings.N_Cities
                For i = 0 To mySettings.N_Cities - 1
                    ListOfCities(i, 0) = i + 1
                    ListOfCities(i, 1) = Math.Cos(i * factor) * Radius + 50
                    ListOfCities(i, 2) = Math.Sin(i * factor) * Radius + 65
                Next
                circumference = 2 * Math.PI * Radius

            Case Common.EnProblem.random
                Dim lowerb As Integer = 2
                Dim upperb1 As Integer = 98
                Dim upperb2 As Integer = 128
                For i = 0 To mySettings.N_Cities - 1
                    ListOfCities(i, 0) = i + 1
                    ListOfCities(i, 1) = CInt(Int((upperb1 - lowerb + 1) * Rnd() + lowerb))
                    ListOfCities(i, 2) = CInt(Int((upperb2 - lowerb + 1) * Rnd() + lowerb))
                Next
        End Select

        If pngExport = True Then
            Directory.CreateDirectory(Directory.GetCurrentDirectory & "\TSP_Export").ToString()
            ExPath = Directory.GetCurrentDirectory & "\TSP_Export\"
        End If

    End Sub

    '*********************************** Programm ******************************************

    'Dimensionieren des ChildStructs
    Public Sub Dim_Children()
        Dim i As Integer
        ReDim ChildrenList(mySettings.N_Children - 1)

        For i = 0 To mySettings.N_Children - 1
            ChildrenList(i).No = i + 1
            ChildrenList(i).Penalty = 999999999999999999
            ReDim ChildrenList(i).Image(mySettings.N_Cities - 1, 2)
            ReDim ChildrenList(i).Path(mySettings.N_Cities - 1)
        Next

    End Sub

    'Dimensionieren des ParentStructs
    Public Sub Dim_Parents_TSP()
        Dim i As Integer

        ReDim ParentList(mySettings.N_Parents - 1)

        For i = 0 To mySettings.N_Parents - 1
            ParentList(i).No = i + 1
            ParentList(i).Penalty = 999999999999999999
            ReDim ParentList(i).Image(mySettings.N_Cities - 1, 2)
            ReDim ParentList(i).Path(mySettings.N_Cities - 1)
        Next

    End Sub

    'Generiert zufällige Paths für alle Kinder
    Public Sub Generate_Random_Path_TSP()
        Dim i, j As Integer
        Dim tmp As Integer
        Dim lowerb As Integer = 1
        Dim upperb As Integer = mySettings.N_Cities
        Randomize()

        For i = 0 To mySettings.N_Children - 1
            For j = 0 To ChildrenList(i).Path.GetUpperBound(0)
                Do
                    'Randomize() nicht vergessen
                    tmp = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
                Loop While Is_No_OK(tmp, ChildrenList(i).Path) = False
                ChildrenList(i).Path(j) = tmp
            Next
        Next i

    End Sub

    '************************ Functionen innerhalb der Generationsschleife ****************************

    'Weist den KinderPfaden die Städte zu
    Public Sub Cities_according_ChildPath()
        Dim i, j As Integer

        For i = 0 To mySettings.N_Children - 1
            ReDim ChildrenList(i).Image(mySettings.N_Cities - 1, 2)
            For j = 0 To mySettings.N_Cities - 1
                ChildrenList(i).Image(j, 0) = ListOfCities(ChildrenList(i).Path(j) - 1, 0)
                ChildrenList(i).Image(j, 1) = ListOfCities(ChildrenList(i).Path(j) - 1, 1)
                ChildrenList(i).Image(j, 2) = ListOfCities(ChildrenList(i).Path(j) - 1, 2)
            Next
        Next i

    End Sub

    'Ermittelt die Qualität bzw. die Länge des Weges Für TSP
    Public Sub Evaluate_child_Quality()
        Dim i, j As Integer
        Dim distance As Double
        Dim distanceX As Double
        Dim distanceY As Double

        For i = 0 To mySettings.N_Children - 1
            distance = 0
            distanceX = 0
            distanceY = 0
            For j = 0 To mySettings.N_Cities - 2
                ChildrenList(i).Penalty = 999999999999999999
                distanceX = (ChildrenList(i).Image(j, 1) - ChildrenList(i).Image(j + 1, 1))
                distanceX = distanceX * distanceX
                distanceY = (ChildrenList(i).Image(j, 2) - ChildrenList(i).Image(j + 1, 2))
                distanceY = distanceY * distanceY
                distance = distance + Math.Sqrt(distanceX + distanceY)
            Next j
            distanceX = (ChildrenList(i).Image(0, 1) - ChildrenList(i).Image(mySettings.N_Cities - 1, 1))
            distanceX = distanceX * distanceX
            distanceY = (ChildrenList(i).Image(0, 2) - ChildrenList(i).Image(mySettings.N_Cities - 1, 2))
            distanceY = distanceY * distanceY
            distance = distance + Math.Sqrt(distanceX + distanceY)
            ChildrenList(i).Penalty = distance
        Next i

    End Sub

    'Selectionsprozess je nach "plus" oder "minus" Strategie
    Public Sub Selection_Process()
        Dim i, j As Integer

        If mySettings.Strategy = Common.EVO_STRATEGIE.Komma_Strategie Then
            For i = 0 To mySettings.N_Parents - 1
                ParentList(i).Penalty = ChildrenList(i).Penalty
                Array.Copy(ChildrenList(i).Image, ParentList(i).Image, ChildrenList(i).Image.Length)
                Array.Copy(ChildrenList(i).Path, ParentList(i).Path, ChildrenList(i).Path.Length)
            Next i

        ElseIf mySettings.Strategy = Common.EVO_STRATEGIE.Plus_Strategie Then
            j = 0
            For i = 0 To mySettings.N_Parents - 1
                If ParentList(i).Penalty < ChildrenList(j).Penalty Then
                    j -= 1
                Else
                    ParentList(i).Penalty = ChildrenList(j).Penalty
                    Array.Copy(ChildrenList(j).Image, ParentList(i).Image, ChildrenList(j).Image.Length)
                    Array.Copy(ChildrenList(j).Path, ParentList(i).Path, ChildrenList(j).Path.Length)
                End If
                j += 1
            Next i
        End If

    End Sub

    'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
    Public Sub Reset_Children()
        Dim i As Integer

        For i = 0 To mySettings.N_Children - 1
            ChildrenList(i).No = i + 1
            ChildrenList(i).Penalty = 999999999999999999
            Array.Clear(ChildrenList(i).Path, 0, ChildrenList(i).Path.GetLength(0))
            ReDim ChildrenList(i).Image(mySettings.N_Cities, 2)
        Next

    End Sub

    '**************************************** Reproductionsfunktionen ****************************************

    'Steuerung der Reproduktionsoperatoren
    Public Sub Reproduction_Control()
        Dim i As Integer
        Dim x, y As Integer
        Dim Einzelkind(mySettings.N_Cities - 1) As Integer

        Select Case mySettings.ReprodOperator
            'UPGRADE: Eltern werden nicht zufällig gewählt sondern immer in Top Down Reihenfolge
            Case Common.EnReprodOperator.Order_Crossover_OX
                x = 0
                y = 1
                For i = 0 To mySettings.N_Children - 2 Step 2
                    Call ReprodOp_OX(ParentList(x).Path, ParentList(y).Path, ChildrenList(i).Path, ChildrenList(i + 1).Path)
                    x += 1
                    y += 1
                    If x = mySettings.N_Parents - 1 Then x = 0
                    If y = mySettings.N_Parents - 1 Then y = 0
                Next i
                If Even_Number(mySettings.N_Children) = False Then
                    Call ReprodOp_OX(ParentList(x).Path, ParentList(y).Path, ChildrenList(mySettings.N_Children - 1).Path, Einzelkind)
                End If

            Case Common.EnReprodOperator.Partially_Mapped_Crossover_PMX
                x = 0
                y = 1
                For i = 0 To mySettings.N_Children - 2 Step 2
                    Call ReprodOp_PMX(ParentList(x).Path, ParentList(y).Path, ChildrenList(i).Path, ChildrenList(i + 1).Path)
                    x += 1
                    y += 1
                    If x = mySettings.N_Parents - 1 Then x = 0
                    If y = mySettings.N_Parents - 1 Then y = 0
                Next i
                If Even_Number(mySettings.N_Children) = False Then
                    Call ReprodOp_PMX(ParentList(x).Path, ParentList(y).Path, ChildrenList(mySettings.N_Children - 1).Path, Einzelkind)
                End If
        End Select

    End Sub

    'Reproductionsoperator "Order_Crossover (OX)"
    'Kopiert den mittleren Teil des einen Elter und füllt den Rest aus der Reihenfolge des anderen Elter auf
    'UPGRADE: Es wird immer nur der mittlere Teil Kopiert, könnte auch mal ein einderer sein
    Private Sub ReprodOp_OX(ByVal ParPath_A() As Integer, ByVal ParPath_B() As Integer, ByRef ChildPath_A() As Integer, ByRef ChildPath_B() As Integer)

        Dim i As Integer
        Dim x, y As Integer

        Dim CutPoint(1) As Integer
        Call Create_n_Cutpoints(CutPoint)

        'Kopieren des mittleren Paths
        For i = CutPoint(0) + 1 To CutPoint(1)
            ChildPath_A(i) = ParPath_A(i)
            ChildPath_B(i) = ParPath_B(i)
        Next
        'Auffüllen des Paths Teil 3 des Child A mit dem anderen Elter beginnend bei 0
        x = 0
        For i = CutPoint(1) + 1 To mySettings.N_Cities - 1
            If Is_No_OK(ParPath_B(x), ChildPath_A) Then
                ChildPath_A(i) = ParPath_B(x)
            Else
                i -= 1
            End If
            x += 1
        Next
        'Auffüllen des Paths Teil 3 des Child B mit dem anderen Elter beginnend bei 0
        y = 0
        For i = CutPoint(1) + 1 To mySettings.N_Cities - 1
            If Is_No_OK(ParPath_A(y), ChildPath_B) Then
                ChildPath_B(i) = ParPath_A(y)
            Else
                i -= 1
            End If
            y += 1
        Next
        'Auffüllen des Paths Teil 1 des Child A mit dem anderen Elter beginnend bei 0
        For i = 0 To CutPoint(0)
            If Is_No_OK(ParPath_B(x), ChildPath_A) Then
                ChildPath_A(i) = ParPath_B(x)
            Else
                i -= 1
            End If
            x += 1
        Next
        'Auffüllen des Paths Teil 1 des Child B mit dem anderen Elter beginnend bei 0
        For i = 0 To CutPoint(0)
            If Is_No_OK(ParPath_A(y), ChildPath_B) Then
                ChildPath_B(i) = ParPath_A(y)
            Else
                i -= 1
            End If
            y += 1
        Next
    End Sub

    'Reproductionsoperator: "Partially_Mapped_Crossover_(PMX)"
    'Kopiert den mittleren Teil des anderen Elter und füllt den Rest mit dem eigenen auf. Falls Doppelt wird gemaped.
    Public Sub ReprodOp_PMX(ByVal ParPath_A() As Integer, ByVal ParPath_B() As Integer, ByRef ChildPath_A() As Integer, ByRef ChildPath_B() As Integer)
        Dim i As Integer
        Dim x As Integer
        Dim Index As Integer
        Dim mapper As Integer

        Dim CutPoint(1) As Integer
        For i = 0 To 10
            Call Create_n_Cutpoints(CutPoint)
        Next

        'Kopieren des mittleren Paths und füllen des Mappers
        x = 0
        For i = CutPoint(0) + 1 To CutPoint(1)
            ChildPath_B(i) = ParPath_A(i)
            ChildPath_A(i) = ParPath_B(i)
            x += 1
        Next

        'Auffüllen des Paths Teil 1 des Child A und B mit dem anderen Elter beginnend bei 0
        For i = 0 To CutPoint(0)
            'für Child A
            If Is_No_OK(ParPath_A(i), ChildPath_A) Then
                ChildPath_A(i) = ParPath_A(i)
            Else
                mapper = ParPath_A(i)
                Do Until (Is_No_OK(mapper, ChildPath_A) = True)
                    Index = Array.IndexOf(ParPath_B, mapper)
                    mapper = ParPath_A(Index)
                Loop
                ChildPath_A(i) = mapper
            End If

            'für Child B
            If Is_No_OK(ParPath_B(i), ChildPath_B) Then
                ChildPath_B(i) = ParPath_B(i)
            Else
                mapper = ParPath_B(i)
                Do Until (Is_No_OK(mapper, ChildPath_B) = True)
                    Index = Array.IndexOf(ParPath_A, mapper)
                    mapper = ParPath_B(Index)
                Loop
                ChildPath_B(i) = mapper
            End If
        Next i

        'Auffüllen des Paths Teil 3 des Child A und B mit dem anderen Elter beginnend bei 0
        For i = CutPoint(1) + 1 To mySettings.N_Cities - 1
            'für Child A
            If Is_No_OK(ParPath_A(i), ChildPath_A) Then
                ChildPath_A(i) = ParPath_A(i)
            Else
                mapper = ParPath_A(i)
                Do Until (Is_No_OK(mapper, ChildPath_A) = True)
                    Index = Array.IndexOf(ParPath_B, mapper)
                    mapper = ParPath_A(Index)
                Loop
                ChildPath_A(i) = mapper
            End If

            'für Child B
            If Is_No_OK(ParPath_B(i), ChildPath_B) Then
                ChildPath_B(i) = ParPath_B(i)
            Else
                mapper = ParPath_B(i)
                Do Until (Is_No_OK(mapper, ChildPath_B) = True)
                    Index = Array.IndexOf(ParPath_A, mapper)
                    mapper = ParPath_B(Index)
                Loop
                ChildPath_B(i) = mapper
            End If
        Next
    End Sub

    '****************************************** Mutationsfunktionen ****************************************

    'Steuerung der Mutationsoperatoren
    Public Sub Mutation_Control()
        Dim i As Integer

        Select Case mySettings.MutOperator
            Case Common.EnMutOperator.Inversion_SIM
                For i = 0 To mySettings.N_Children - 1
                    Call MutOp_SIM(ChildrenList(i).Path)
                    'If PathValid(ChildList(i).Path) = False Then Throw New Exception("Fehler im Path")
                Next i
            Case Common.EnMutOperator.Translocation_3_Opt
                For i = 0 To mySettings.N_Children - 1
                    Call MutOp_3_opt(ChildrenList(i).Path)
                    'If PathValid(ChildList(i).Path) = False Then Throw New Exception("Fehler im Path")
                Next i
            Case Common.EnMutOperator.Translocation_n_Opt
                For i = 0 To mySettings.N_Children - 1
                    Call MutOp_n_opt(ChildrenList(i).Path)
                    'If PathValid(ChildList(i).Path) = False Then Throw New Exception("Fehler im Path")
                Next i

            Case Common.EnMutOperator.Exchange_Mutation_EM
                For i = 0 To mySettings.N_Children - 1
                    Call MutOp_EM(ChildrenList(i).Path)
                Next
        End Select

    End Sub

    'Mutationsoperator "Inversion (SIM)"
    'Schneidet ein Segment aus dem Path heraus und fügt es invers wieder ein
    'UPGRADE: Wird bis jetzt nur auf den mittleren Teil angewendet
    Private Sub MutOp_SIM(ByVal Path() As Integer)
        Dim i As Integer
        Dim x As Integer

        Dim CutPoint(1) As Integer
        Call Create_n_Cutpoints(CutPoint)

        Dim SubPath(CutPoint(1) - CutPoint(0) - 1) As Integer

        'Kopieren des Substrings
        x = 0
        For i = CutPoint(0) + 1 To CutPoint(1)
            SubPath(x) = Path(i)
            x += 1
        Next

        'Invertiertes einfügen
        For i = CutPoint(0) + 1 To CutPoint(1)
            x -= 1
            Path(i) = SubPath(x)
        Next

    End Sub

    'Mutationsoperator "Translocation (3-Opt"
    'Vertauscht zufällig 3 Abschnitte aus dem String und verwendet Bernoulli verteilt die Inverse
    'UPGRADE: Jetzt werden immer 3 Translocation durchgeführt könnte man auf n-Ausbauen
    Private Sub MutOp_3_opt(ByVal Path() As Integer)
        Dim i, j As Integer
        Dim x As Integer
        Dim tmp As Integer
        Dim SwapPath(2) As Integer
        Dim CutPoint(1) As Integer
        Call Create_n_Cutpoints(CutPoint)

        Dim SubPath(2)() As Integer
        ReDim SubPath(0)(CutPoint(0))
        ReDim SubPath(1)(CutPoint(1) - CutPoint(0) - 1)
        ReDim SubPath(2)(mySettings.N_Cities - CutPoint(1) - 2)

        j = SubPath(0).GetLength(0) + SubPath(1).GetLength(0) + SubPath(2).GetLength(0)

        'Kopieren der Substrings
        x = 0
        For i = 0 To CutPoint(0)
            SubPath(0)(x) = Path(i)
            x += 1
        Next
        x = 0
        For i = CutPoint(0) + 1 To CutPoint(1)
            SubPath(1)(x) = Path(i)
            x += 1
        Next
        x = 0
        For i = CutPoint(1) + 1 To mySettings.N_Cities - 1
            SubPath(2)(x) = Path(i)
            x += 1
        Next

        'Bernloulli Verteilte Inversion der Subpaths
        If Bernoulli() = True Then Array.Reverse(SubPath(0))
        If Bernoulli() = True Then Array.Reverse(SubPath(1))
        If Bernoulli() = True Then Array.Reverse(SubPath(2))

        'Generieren der neuen Reihenfolge
        For i = 0 To 2
            Do
                tmp = CInt(Int(3 * Rnd() + 1))
            Loop While Is_No_OK(tmp, SwapPath) = False
            SwapPath(i) = tmp
        Next
        For i = 0 To 2
            SwapPath(i) -= 1
        Next

        'Übertragen der Substrings in den Path
        x = 0
        For i = 0 To 2
            For j = 0 To SubPath(SwapPath(i)).GetUpperBound(0)
                Path(x) = SubPath(SwapPath(i))(j)
                x += 1
            Next
        Next

    End Sub
    'Mutationsoperator "Translocation (n-Opt)"
    'Vertauscht zufällig n Abschnitte aus dem String und verwendet Bernoulli verteilt die Inverse
    Private Sub MutOp_n_opt(ByVal Path() As Integer)
        Dim i, j As Integer
        Dim x As Integer
        Dim tmp As Integer
        Dim SwapPath(n_SP - 1) As Integer
        Dim CutPoint(n_SP - 2) As Integer
        Call Create_n_Cutpoints(CutPoint)

        Dim SubPath(n_SP - 1)() As Integer

        ReDim SubPath(0)(CutPoint(0))
        For i = 1 To n_SP - 2
            ReDim SubPath(i)(CutPoint(i) - CutPoint(i - 1) - 1)
        Next
        'ReDim SubPath(1)(CutPoint(1) - CutPoint(0) - 1)
        ReDim SubPath(n_SP - 1)(mySettings.N_Cities - CutPoint(n_SP - 2) - 2)

        'Kopieren der Substrings
        x = 0
        For i = 0 To CutPoint(0)
            SubPath(0)(x) = Path(i)
            x += 1
        Next
        For j = 0 To n_SP - 3
            x = 0
            For i = CutPoint(j) + 1 To CutPoint(j + 1)
                SubPath(j + 1)(x) = Path(i)
                x += 1
            Next
        Next
        x = 0
        For i = CutPoint(n_SP - 2) + 1 To mySettings.N_Cities - 1
            SubPath(n_SP - 1)(x) = Path(i)
            x += 1
        Next

        'Bernloulli Verteilte Inversion der Subpaths
        For i = 0 To n_SP - 1
            If Bernoulli() = True Then Array.Reverse(SubPath(i))
        Next

        'Generieren der neuen Reihenfolge
        For i = 0 To n_SP - 1
            Do
                tmp = CInt(Int(n_SP * Rnd() + 1))
            Loop While Is_No_OK(tmp, SwapPath) = False
            SwapPath(i) = tmp
        Next
        For i = 0 To n_SP - 1
            SwapPath(i) -= 1
        Next

        'Übertragen der Substrings in den Path
        x = 0
        For i = 0 To n_SP - 1
            For j = 0 To SubPath(SwapPath(i)).GetUpperBound(0)
                Path(x) = SubPath(SwapPath(i))(j)
                x += 1
            Next
        Next

    End Sub

    'Mutationsoperator "Exchange Mutation (EM)"
    'Vertauscht n-mal zwei Werte innerhalb des Paths
    Private Sub MutOp_EM(ByVal Path() As Integer)
        Dim i As Integer
        Dim TransRate As Integer = 4               'Transpositionsrate in Prozent(!Achtung keine echte "Rate"!)
        Dim n_trans As Integer                     'Anzahl der Transpositionen
        Dim Point1, Point2 As Integer
        Dim Swap As Integer
        Dim lowerb As Integer = 1
        Dim upperbo As Integer = mySettings.N_Cities - 2

        n_trans = Math.Round(mySettings.N_Cities * TransRate / 100)

        For i = 0 To n_trans
            Point1 = CInt(Int((upperbo - lowerb + 1) * Rnd() + lowerb))
            Do
                Point2 = CInt(Int((upperbo - lowerb + 1) * Rnd() + lowerb))
            Loop Until Point1 <> Point2
            Swap = Path(Point1)
            Path(Point1) = Path(Point2)
            Path(Point2) = Swap
        Next

    End Sub


    '******************************************* Hilfsfunktionen *******************************************

    'Hilfsfunktion: Validierung der Paths
    'UPGRADE:Option zum ein und Ausschalten dieser Function
    Public Function PathValid(ByVal Path() As Integer) As Boolean
        Dim i As Integer
        Array.Sort(Path)
        For i = 0 To Path.GetUpperBound(0)
            If Path(i) <> i + 1 Then
                Exit Function
            End If
        Next
        PathValid = True
    End Function

    'Hilfsfunktion um zu Prüfen ob eine Zahl bereits in einem Array vorhanden ist oder nicht
    Public Function Is_No_OK(ByRef No As Integer, ByRef Path() As Integer) As Boolean
        Is_No_OK = True

        'Dim i As Integer
        'For i = 0 To Path.GetUpperBound(0)
        '    If No = Path(i) Then
        '        Is_No_OK = False
        '        'Exit Function
        '    End If
        'Next

        Dim Index As Integer = -7
        Index = Array.IndexOf(Path, No)
        If Index <> -1 Then
            Is_No_OK = False
        End If

    End Function

    'Hilfsfunktion zum sortieren der Faksimile
    Public Sub Sort_Faksimile(ByRef FaksimileList() As Faksimile_Type)
        'Sortiert die Fiksimile anhand des Abstandes
        Dim i, j As Integer
        Dim swap As TSP.Faksimile_Type

        For i = 0 To FaksimileList.GetUpperBound(0)
            For j = 0 To FaksimileList.GetUpperBound(0)
                If FaksimileList(i).Penalty < FaksimileList(j).Penalty Then
                    swap = FaksimileList(i)
                    FaksimileList(i) = FaksimileList(j)
                    FaksimileList(j) = swap
                End If
            Next j
        Next i

    End Sub

    'Hilfsfunktion zum generieren von zufälligen Schnittpunkten innerhalb eines Pfades
    Public Sub Create_n_Cutpoints(ByRef CutPoint() As Integer)

        Dim i As Integer
        Dim lowerb As Integer = 0
        Dim upperb As Integer = mySettings.N_Cities - 2

        For i = 0 To CutPoint.GetUpperBound(0)
            CutPoint(i) = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
        Next

        Array.Sort(CutPoint)

    End Sub

    'Hilfsfunktion generiert Bernoulli verteilte Zufallszahl
    Public Function Bernoulli() As Boolean
        Dim lowerb As Integer = 0
        Dim upperbo As Integer = 1
        Bernoulli = CInt(Int(2 * Rnd()))
    End Function

    'Hilfsfunktion: Gerade oder Ungerade Zahl
    Public Function Even_Number(ByVal Number As Integer) As Boolean
        Dim tmp_a As Double
        Dim tmp_b As Double
        Dim tmp_c As Double
        tmp_a = Number / 2
        tmp_b = Math.Ceiling(tmp_a)
        tmp_c = tmp_a - tmp_b
        If tmp_c = 0 Then Even_Number = True
    End Function

    'Anzahl der mglichen Kombinationen
    'mit n_comb=(n-1)/2
    Public Function n_Comb(ByVal n As Double) As String

        n = n - 1

        Dim i As Integer
        Dim Summe As Double
        Dim Mant As Double
        Dim Expo As Double
        Dim Prod As Double

        If n < 170 Then
            Prod = 1
            For i = 1 To n
                Prod = Prod * i
            Next i
            n_Comb = Trim$(Str$(Prod / 2))
        Else
            Summe = 0
            For i = 1 To n
                Summe = Summe + math.log(i)
            Next i
            Summe = Summe / math.log(10)
            Expo = Int(Summe)
            Mant = 10 ^ (Summe - Expo)
            n_Comb = (Mant/2) & " E+" & Expo
        End If

    End Function

End Class
