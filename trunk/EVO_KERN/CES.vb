Public Class CES

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse CES Kombinatorische Evolutionsstrategie                        ****
    '****                                                                       ****
    '**** Christoph Hübner                                                      ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '****                                                                       ****
    '**** Februar 2007                                                          ****
    '****                                                                       ****
    '**** Letzte Änderung: März 2007                                            ****
    '*******************************************************************************
    '*******************************************************************************

    '********************************************* Konvention *****************************
    'Cities:      1 2 3 4 5 6 7
    'Pathindex:   0 1 2 3 4 5 6
    'CutPoint:     1 2 3 4 5 6
    'LB + UB n=2   x         x

    'Public Variablen
    Public n_Locations As Integer       'Anzahl der Locations wird von außen gesetzt
    Public n_Ziele As Integer           'Anzahl der Ziele wird von außen gesetzt
    Public n_Verzweig As Integer        'Anzahl der Verzweigungen in der Verzweigungsdatei
    Public n_PathSize () as Integer     'Anzahl der "Städte/Maßnahmen" an jeder Stelle
    Public n_Gen As Integer = 10

    'Private Variablen
    Private n_Parents As Integer = 3
    Private n_Childs As Integer = 10

    Private ReprodOperator_TSP As String = "Order_Crossover_OX"
    Private ReprodOperator_BM As String = "Select_Random_Uniform"
    Private MutOperator_TSP As String = "Translocation"
    Private MutOperator_BM As String = "Random_Switch"
    Private Strategy As String = "plus"         '"plus" oder "minus" Strategie
    Private MutRate As Integer = 30              'Definiert die Wahrscheinlichkeit der Mutationsrate in %

    '************************************* Struktur *****************************
    Public Structure Faksimile_Type
        Dim No As Short
        Dim Path() As Integer      'auf 0 ist die Stadt, auf 1 die Anzahl der Städte
        Dim Penalty_SO As Double    'HACK zum Ausgleich von MO und SO
        Dim Penalty_MO() As Double
    End Structure

    '************************************* Listen ******************************
    Public ChildList() As Faksimile_Type
    Public ParentList() As Faksimile_Type

    '******************************** NDSorting Struktur **************************
    Public Structure NDSortingType
        Dim Penalty_MO() As Double             'DM: Werte der Penaltyfunktion(en)
        Dim Constrain() As Double           'DM: Werte der Randbedingung(en)
        Dim Feasible As Boolean             'DM: Gültiges Ergebnis ?
        Dim dominated As Boolean            'DM: Kennzeichnung ob dominiert
        Dim Front As Short                  'DM: Nummer der Pareto Front
        Dim Distance As Double              'DM: Distanzwert für Crowding distance sort
        Dim No As Short                     'CH: Nummer des Faksimile
        Dim Path() As Integer               'CH: Der Pfad
    End Structure

    Dim NDSorting() As NDSortingType ' NDSorting Liste ***************************


    '*********************************** Programm ******************************************

    'Dimensionieren des ChildStructs
    Public Sub Dim_Childs()
        Dim i, j As Integer
        ReDim ChildList(n_Childs - 1)

        For i = 0 To n_Childs - 1
            ChildList(i).No = i + 1
            ChildList(i).Penalty_SO = 999999999999999999

            ReDim ChildList(i).Penalty_MO(n_Ziele - 1)
            For j = 0 To n_Ziele - 1
                ChildList(i).Penalty_MO(j) = 999999999999999999
            Next
            ReDim ChildList(i).Path(n_Locations - 1)
        Next

    End Sub

    'Dimensionieren des ParentStructs
    Public Sub Dim_Parents_BM()
        Dim i, j As Integer

        ReDim ParentList(n_Parents - 1)

        For i = 0 To n_Parents - 1
            ParentList(i).No = i + 1
            ParentList(i).Penalty_SO = 999999999999999999
            ReDim ParentList(i).Penalty_MO(n_Ziele - 1)
            For j = 0 To n_Ziele - 1
                ParentList(i).Penalty_MO(j) = 999999999999999999
            Next
            ReDim ParentList(i).Path(n_Locations - 1)
        Next

    End Sub

    'Dimensionieren des NDSortingStructs BM Problem
    Public Sub Dim_NDSorting()
        Dim i, j As Integer

        ReDim NDSorting(n_Childs + n_Parents - 1)

        For i = 0 To n_Childs + n_Parents - 1
            NDSorting(i).No = 0
            'NDSorting(i).Penalty_SO = 999999999999999999
            ReDim NDSorting(i).Penalty_MO(n_Ziele - 1)
            For j = 0 To n_Ziele - 1
                NDSorting(i).Penalty_MO(j) = 999999999999999999
            Next
            ReDim NDSorting(i).Path(n_Locations - 1)
            'ReDim NDSorting(i).VER_ONOFF(n_Verzweig - 1, 1)
        Next

    End Sub

    'Generiert zufällige Paths für alle Kinder BM Problem
    Public Sub Generate_Random_Path()
        Dim i, j As Integer
        Dim tmp As Integer
        Dim lowerb As Integer = 0
        Dim upperb As Integer
        Randomize()

        For i = 0 To n_Childs - 1
            For j = 0 To n_Locations - 1
                upperb = n_PathSize(j)
                'Randomize() nicht vergessen
                tmp = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
                ChildList(i).Path(j) = tmp
            Next
        Next

    End Sub

    'HACK
    'Funktion zum manuellen Testen der Paths in der ersten Generation
    Public Sub Generate_Test_Path()
        Dim i, j As Integer
        Dim Grenze As Integer

        'Achtung n_Childs sollte Größer als die Möglichen Kombinationen an einer Stelle sein
        For i = 0 To n_Childs - 1
            For j = 0 To n_Locations - 1
                Grenze = n_PathSize(j) - 1
                If i <= Grenze Then
                    ChildList(i).Path(j) = i
                Else
                    ChildList(i).Path(j) = 0
                End If
            Next
        Next

    End Sub

    'HACK
    'Funktion zum manuellen Testen der Paths in der ersten Generation
    Public Sub Generate_All_Test_Path()
        Dim i As Integer
        Dim x, y, z As Integer

        x = 0
        y = 0
        z = 0

        For i = 0 To n_Childs - 1

            ChildList(i).Path(0) = x
            ChildList(i).Path(1) = y
            ChildList(i).Path(2) = z
            x += 1
            If x > n_PathSize(0) - 1 Then
                x = 0
                y += 1
            End If
            If y > n_PathSize(1) - 1 Then
                y = 0
                z += 1
            End If
            If z > n_PathSize(2) - 1 Then
                z = 0
            End If

        Next

    End Sub

    '*************************** Functionen innerhalb der Generationsschleife ****************************

    'Selectionsprozess je nach "plus" oder "minus" Strategie
    Public Sub Selection_Process_BM()
        Dim i, j As Integer
        Dim ChildQ_TMP As Double = 0
        Dim ParentQ_TMP As Double = 0

        If Strategy = "minus" Then
            For i = 0 To n_Parents - 1
                ParentList(i).Penalty_SO = ChildList(i).Penalty_SO
                Array.Copy(ChildList(i).Path, ParentList(i).Path, ChildList(i).Path.Length)
            Next i

        ElseIf Strategy = "plus" Then
            j = 0
            For i = 0 To n_Parents - 1
                If ParentList(i).Penalty_SO < ChildList(j).Penalty_SO Then
                    j -= 1
                Else
                    ParentList(i).Penalty_SO = ChildList(j).Penalty_SO
                    ParentList(i).Penalty_MO = ChildList(j).Penalty_MO 'HACK: hier Qualität Doppelt
                    Array.Copy(ChildList(j).Path, ParentList(i).Path, ChildList(j).Path.Length)
                End If
                j += 1
            Next i
        End If

    End Sub

    'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
    Public Sub Reset_Childs_BM()
        Dim i, j As Integer

        For i = 0 To n_Childs - 1
            ChildList(i).No = i + 1
            ChildList(i).Penalty_SO = 999999999999999999
            For j = 0 To ChildList(i).Penalty_MO.GetUpperBound(0)
                ChildList(i).Penalty_MO(j) = 999999999999999999
            Next
            Array.Clear(ChildList(i).Path, 0, ChildList(i).Path.GetLength(0))
        Next

    End Sub

    '**************************************** Reproductionsfunktionen ****************************************

    'Steuerung der Reproduktionsoperatoren
    Public Sub Reproduction_Control_BM()
        Dim i As Integer
        Dim x, y As Integer
        Dim Einzelkind(n_Locations - 1) As Integer

        Select Case ReprodOperator_BM
            'ToDo: Eltern werden nicht zufällig gewählt sondern immer in Top Down Reihenfolge
            Case "Select_Random_Uniform"
                x = 0
                y = 1
                For i = 0 To n_Childs - 2 Step 2
                    Call ReprodOp_Select_Random_Uniform(ParentList(x).Path, ParentList(y).Path, ChildList(i).Path, ChildList(i + 1).Path)
                    x += 1
                    y += 1
                    If x = n_Parents - 1 Then x = 0
                    If y = n_Parents - 1 Then y = 0
                Next i
                If Even_Number(n_Childs) = False Then
                    Call ReprodOp_Select_Random_Uniform(ParentList(x).Path, ParentList(y).Path, ChildList(n_Childs - 1).Path, Einzelkind)
                End If
        End Select

    End Sub

    'Reproductionsoperator: "Select_Random_Uniform"
    'Entscheidet zufällig welcher ob der Wert aus dem Path des Elter_A oder Elter_B verwendet wird
    Private Sub ReprodOp_Select_Random_Uniform(ByVal ParPath_A() As Integer, ByVal ParPath_B() As Integer, ByRef ChildPath_A() As Integer, ByRef ChildPath_B() As Integer)

        Dim i As Integer

        For i = 0 To ChildPath_A.GetUpperBound(0)    'ToDo: Es müsste eigentlich eine definierte Pfadlänge geben
            If Bernoulli() = True Then
                ChildPath_A(i) = ParPath_B(i)
            Else
                ChildPath_A(i) = ParPath_A(i)
            End If
        Next

        For i = 0 To ChildPath_B.GetUpperBound(0)    'ToDo: Es müsste eigentlich eine definierte Pfadlänge geben
            If Bernoulli() = True Then
                ChildPath_B(i) = ParPath_A(i)
            Else
                ChildPath_B(i) = ParPath_B(i)
            End If
        Next

    End Sub


    '****************************************** Mutationsfunktionen ****************************************

    'Steuerung der Mutationsoperatoren
    Public Sub Mutation_Control_BM()
        Dim i As Integer

        Select Case MutOperator_BM
            Case "Random_Switch"
                For i = 0 To n_Childs - 1
                    Call MutOp_RND_Switch(ChildList(i).Path)
                    'If PathValid(ChildList(i).Path) = False Then MsgBox("Fehler im Path", MsgBoxStyle.Information, "Fehler")
                Next i
        End Select

    End Sub

    'Mutationsoperator "Random_Switch"
    'Verändert zufällig ein gen des Paths
    Private Sub MutOp_RND_Switch(ByVal Path() As Integer)
        Dim i As Integer
        Dim Tmp_a As Integer
        Dim Tmp_b As Integer
        Dim lowerb_a As Integer = 1
        Dim upperb_a As Integer = 100
        Dim lowerb_b As Integer = 0
        Dim upperb_b As Integer
        Randomize()

        For i = 0 To Path.GetUpperBound(0)
            Tmp_a = CInt(Int((upperb_a - lowerb_a + 1) * Rnd() + lowerb_a))
            If Tmp_a <= MutRate Then
                upperb_b = n_PathSize(i) - 1
                'Randomize() nicht vergessen
                Tmp_b = CInt(Int((upperb_b - lowerb_b + 1) * Rnd() + lowerb_b))
                Path(i) = Tmp_b
            End If
        Next

    End Sub

    '******************************************* Hilfsfunktionen *******************************************

    'Hilfsfunktion um zu Prüfen ob eine Zahl bereits in einem Array vorhanden ist oder nicht
    Public Function Is_No_OK(ByVal No As Integer, ByVal Path() As Integer) As Boolean
        Is_No_OK = True
        Dim i As Integer
        For i = 0 To Path.GetUpperBound(0)
            If No = Path(i) Then
                Is_No_OK = False
                Exit Function
            End If
        Next
    End Function

    'Hilfsfunktion zum sortieren der Faksimile
    Public Sub Sort_Faksimile(ByRef FaksimileList() As Faksimile_Type)
        'Sortiert die Fiksimile anhand des Abstandes
        Dim i, j As Integer
        Dim swap As EvoKern.CES.Faksimile_Type

        For i = 0 To FaksimileList.GetUpperBound(0)
            For j = 0 To FaksimileList.GetUpperBound(0)
                If FaksimileList(i).Penalty_SO < FaksimileList(j).Penalty_SO Then
                    swap = FaksimileList(i)
                    FaksimileList(i) = FaksimileList(j)
                    FaksimileList(j) = swap
                End If
            Next j
        Next i

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

    'HACK zur Reduzierung auf eine Zielfunktion
    Public Sub MO_TO_SO(ByRef FaksimileList As Faksimile_Type)
        Dim x As Integer
        FaksimileList.Penalty_SO = 0
        For x = 0 To FaksimileList.Penalty_MO.GetUpperBound(0)
            FaksimileList.Penalty_SO = FaksimileList.Penalty_SO + FaksimileList.Penalty_MO(x)
        Next
        'FaksimileList.Quality_SO = FaksimileList.Quality_SO / n_Ziele

    End Sub

    '                          Steuerung des NDSorting
    '********************************************************************************

    'Public Sub NDSorting_Control()
    '    Dim i, j As Short
    '    Dim NFrontMember_aktuell, NFrontMember_gesamt As Short
    '    Dim Durchlauf_Front As Short
    '    Dim Temp() As NDSortingType
    '    Dim NDSResult() As NDSortingType
    '    Dim Count, aktuelle_Front As Short
    '    Dim Member_Sekundärefront As Short

    '    '1. Eltern und Nachfolger werden gemeinsam betrachtet
    '    'Nur Eltern werden NDSorting hinzugefügt, Kinder sind schon oben drin
    '    '---------------------------------------------------------------------
    '    For i = n_Childs To n_Childs + n_Parents - 1
    '        With NDSorting(i)
    '            For j = 0 To n_Ziele - 1
    '                .Penalty_MO(j) = ChildList_BM(i - n_Childs).Penalty_MO(j)
    '            Next j

    '            'Constraints bisher nicht eingebaut
    '            '***************************************
    '            'If Eigenschaft.NConstrains > 0 Then
    '            '    .Feasible = True
    '            '    For l = 1 To Eigenschaft.NConstrains
    '            '        .Constrain(l) = Rb(m - Eigenschaft.NNachf, Eigenschaft.iaktuellePopulation, l)
    '            '        If .Constrain(l) < 0 Then .Feasible = False
    '            '    Next l
    '            'End If

    '            .dominated = False
    '            .Front = 0
    '            .Distance = 0
    '            For j = 1 To Eigenschaft.varanz
    '                'Die Schrittweite wird ebenfalls übernommen
    '                .d(v) = Db(v, i - Eigenschaft.NNachf, Eigenschaft.iaktuellePopulation)
    '                'Die eigentlichen Parameterwerte werden übernommen
    '                .X(v) = Xb(v, i - Eigenschaft.NNachf, Eigenschaft.iaktuellePopulation)
    '            Next v

    '        End With
    '    Next i



    'End Sub

End Class


