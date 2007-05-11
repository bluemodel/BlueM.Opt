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

    'BlueM wird kopiert, achtung kein Zeiger
    Public BlueM1 As Apps.BlueM

    'Public Variablen
    Public n_Locations As Integer
    Public n_Gen As Integer = 10
    Public n_Ziele As Integer

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
        Dim Path() As Integer
        Dim Penalty_SO As Double    'HACK zum Ausgleich von MO und SO
        Dim Penalty_MO() As Double
        Dim VER_ONOFF(,) As Object
    End Structure

    '************************************* Listen ******************************
    Public ChildList_BM() As Faksimile_Type
    Public ParentList_BM() As Faksimile_Type

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

    'Dimensionieren des ChildStructs BM Problem
    Public Sub Dim_Childs()
        Dim i, j As Integer
        ReDim ChildList_BM(n_Childs - 1)

        For i = 0 To n_Childs - 1
            ChildList_BM(i).No = i + 1
            ChildList_BM(i).Penalty_SO = 999999999999999999

            ReDim ChildList_BM(i).Penalty_MO(n_Ziele - 1)
            For j = 0 To ChildList_BM(i).Penalty_MO.GetUpperBound(0)
                ChildList_BM(i).Penalty_MO(j) = 999999999999999999
            Next
            ReDim ChildList_BM(i).Path(BlueM1.LocationList.GetUpperBound(0))
            ReDim ChildList_BM(i).VER_ONOFF(BlueM1.VerzweigungsDatei.GetUpperBound(0), 1)
        Next

    End Sub

    'Dimensionieren des ParentStructs BM Problem
    Public Sub Dim_Parents_BM()
        Dim i, j As Integer

        ReDim ParentList_BM(n_Parents - 1)

        For i = 0 To n_Parents - 1
            ParentList_BM(i).No = i + 1
            ParentList_BM(i).Penalty_SO = 999999999999999999
            ReDim ParentList_BM(i).Penalty_MO(n_Ziele - 1)
            For j = 0 To ParentList_BM(i).Penalty_MO.GetUpperBound(0)
                ParentList_BM(i).Penalty_MO(j) = 999999999999999999
            Next
            ReDim ParentList_BM(i).Path(BlueM1.LocationList.GetUpperBound(0))
            ReDim ParentList_BM(i).VER_ONOFF(BlueM1.VerzweigungsDatei.GetUpperBound(0), 1)
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
            For j = 0 To NDSorting(i).Penalty_MO.GetUpperBound(0)
                NDSorting(i).Penalty_MO(j) = 999999999999999999
            Next
            ReDim NDSorting(i).Path(BlueM1.LocationList.GetUpperBound(0))
            ReDim ParentList_BM(i).VER_ONOFF(BlueM1.VerzweigungsDatei.GetUpperBound(0), 1)
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
            For j = 0 To ChildList_BM(i).Path.GetUpperBound(0)
                upperb = BlueM1.LocationList(j).MassnahmeListe.GetUpperBound(0)
                'Randomize() nicht vergessen
                tmp = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
                ChildList_BM(i).Path(j) = tmp
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
            For j = 0 To ChildList_BM(i).Path.GetUpperBound(0)
                Grenze = BlueM1.LocationList(j).MassnahmeListe.GetUpperBound(0)
                If i <= Grenze Then
                    ChildList_BM(i).Path(j) = i
                Else
                    ChildList_BM(i).Path(j) = 0
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

            ChildList_BM(i).Path(0) = x
            ChildList_BM(i).Path(1) = y
            ChildList_BM(i).Path(2) = z
            x += 1
            If x > BlueM1.LocationList(0).MassnahmeListe.GetUpperBound(0) Then
                x = 0
                y += 1
            End If
            If y > BlueM1.LocationList(1).MassnahmeListe.GetUpperBound(0) Then
                y = 0
                z += 1
            End If
            If z > BlueM1.LocationList(2).MassnahmeListe.GetUpperBound(0) Then
                z = 0
            End If

        Next

    End Sub

    '*************************** Functionen innerhalb der Generationsschleife ****************************


    'Ermittelt die Qualität der Kinder mit dem BlueM
    'Achtung! dies Funktionen liegen jetzt im BlueM
    Public Sub Evaluate_Child_Quality_BM()


    End Sub

    'Selectionsprozess je nach "plus" oder "minus" Strategie
    Public Sub Selection_Process_BM()
        Dim i, j As Integer
        Dim ChildQ_TMP As Double = 0
        Dim ParentQ_TMP As Double = 0

        If Strategy = "minus" Then
            For i = 0 To n_Parents - 1
                ParentList_BM(i).Penalty_SO = ChildList_BM(i).Penalty_SO
                Array.Copy(ChildList_BM(i).VER_ONOFF, ParentList_BM(i).VER_ONOFF, ChildList_BM(i).VER_ONOFF.Length)
                Array.Copy(ChildList_BM(i).Path, ParentList_BM(i).Path, ChildList_BM(i).Path.Length)
            Next i

        ElseIf Strategy = "plus" Then
            j = 0
            For i = 0 To n_Parents - 1
                If ParentList_BM(i).Penalty_SO < ChildList_BM(j).Penalty_SO Then
                    j -= 1
                Else
                    ParentList_BM(i).Penalty_SO = ChildList_BM(j).Penalty_SO
                    ParentList_BM(i).Penalty_MO = ChildList_BM(j).Penalty_MO 'HACK: hier Qualität Doppelt
                    Array.Copy(ChildList_BM(j).VER_ONOFF, ParentList_BM(i).VER_ONOFF, ChildList_BM(j).VER_ONOFF.Length)
                    Array.Copy(ChildList_BM(j).Path, ParentList_BM(i).Path, ChildList_BM(j).Path.Length)
                End If
                j += 1
            Next i
        End If

    End Sub

    'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
    Public Sub Reset_Childs_BM()
        Dim i, j As Integer

        For i = 0 To n_Childs - 1
            ChildList_BM(i).No = i + 1
            ChildList_BM(i).Penalty_SO = 999999999999999999
            For j = 0 To ChildList_BM(i).Penalty_MO.GetUpperBound(0)
                ChildList_BM(i).Penalty_MO(j) = 999999999999999999
            Next
            Array.Clear(ChildList_BM(i).Path, 0, ChildList_BM(i).Path.GetLength(0))
            ReDim ChildList_BM(i).VER_ONOFF(BlueM1.VerzweigungsDatei.GetUpperBound(0), 1)
        Next

    End Sub

    '**************************************** Reproductionsfunktionen ****************************************

    'Steuerung der Reproduktionsoperatoren
    Public Sub Reproduction_Control_BM()
        Dim i As Integer
        Dim x, y As Integer
        Dim Einzelkind(BlueM1.LocationList.GetUpperBound(0)) As Integer

        Select Case ReprodOperator_BM
            'ToDo: Eltern werden nicht zufällig gewählt sondern immer in Top Down Reihenfolge
            Case "Select_Random_Uniform"
                x = 0
                y = 1
                For i = 0 To n_Childs - 2 Step 2
                    Call ReprodOp_Select_Random_Uniform(ParentList_BM(x).Path, ParentList_BM(y).Path, ChildList_BM(i).Path, ChildList_BM(i + 1).Path)
                    x += 1
                    y += 1
                    If x = n_Parents - 1 Then x = 0
                    If y = n_Parents - 1 Then y = 0
                Next i
                If Even_Number(n_Childs) = False Then
                    Call ReprodOp_Select_Random_Uniform(ParentList_BM(x).Path, ParentList_BM(y).Path, ChildList_BM(n_Childs - 1).Path, Einzelkind)
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
                    Call MutOp_RND_Switch(ChildList_BM(i).Path)
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
                upperb_b = BlueM1.LocationList(i).MassnahmeListe.GetUpperBound(0)
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

    '                          Funktionen für CombiBM
    '********************************************************************************

    'Ermittelt des Erforderlichen Zustands für die Verzweigungen
    Public Sub Verzweigung_ON_OFF()
        Dim i, j, x, y, z As Integer
        Dim No As Short

        'Schreibt alle Verzweigungen ins Array
        'kann man auch früher machen!!!!
        For i = 0 To n_Childs - 1
            For j = 0 To ChildList_BM(i).VER_ONOFF.GetUpperBound(0)
                ChildList_BM(i).VER_ONOFF(j, 0) = BlueM1.VerzweigungsDatei(j, 0)
            Next
            For x = 0 To ChildList_BM(i).Path.GetUpperBound(0)
                No = ChildList_BM(i).Path(x)
                For y = 0 To BlueM1.LocationList(x).MassnahmeListe(No).Schaltung.GetUpperBound(0)
                    For z = 0 To ChildList_BM(i).VER_ONOFF.GetUpperBound(0)
                        If BlueM1.LocationList(x).MassnahmeListe(No).Schaltung(y, 0) = ChildList_BM(i).VER_ONOFF(z, 0) Then
                            ChildList_BM(i).VER_ONOFF(z, 1) = BlueM1.LocationList(x).MassnahmeListe(No).Schaltung(y, 1)
                        End If
                    Next
                Next
            Next
        Next
    End Sub

    ''                          Steuerung des NDSorting
    ''********************************************************************************

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
    '            For v = 1 To Eigenschaft.varanz
    '                'Die Schrittweite wird ebenfalls übernommen
    '                .d(v) = Db(v, i - Eigenschaft.NNachf, Eigenschaft.iaktuellePopulation)
    '                'Die eigentlichen Parameterwerte werden übernommen
    '                .X(v) = Xb(v, i - Eigenschaft.NNachf, Eigenschaft.iaktuellePopulation)
    '            Next v
    '            .Distance = 0
    '        End With
    '    Next i



    'End Sub

End Class


