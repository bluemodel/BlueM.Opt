Imports IHWB.EVO.Common

Public Class CES

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse CES Kombinatorische Evolutionsstrategie                        ****
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

#Region "Eigenschaften"
    '##################

    'Die Settings für alles aus dem Form
    Public Settings As EVO_Settings             'TODO: sollte private sein!

    'Modell Setting
    Public Structure ModSettings
        Public n_Locations As Integer           'Anzahl der Locations
        Public n_Verzweig As Integer            'Anzahl der Verzweigungen in der Verzweigungsdatei
        Public n_PathDimension() As Integer     'Anzahl der Maßnahmen an jedem Ort
    End Structure

    Public ModSett As ModSettings

    'Listen für die Individuen
    '*************************
    Public Childs() As Individuum
    Public Parents() As Individuum
    Public SekundärQb(-1) As Individuum
    Public NDSorting() As Individuum
    'Checken ob es verwendet wird
    Public NDSResult() As Individuum

    'Für Hybrid
    Public PES_Memory() As Individuum
    Public PES_Parents_pChild() As Individuum
    Public PES_Parents_pLoc() As Individuum
    Private PES_Mem_SekundärQb(-1) As Individuum

#End Region 'Eigenschaften

#Region "Methoden"
    '#############

    'Initialisierung der PES
    '***************************************
    Public Sub CESInitialise(ByRef Settings As EVO_Settings, ByVal Method As String, ByVal TestModus As CES_T_MODUS, ByVal AnzPenalty As Integer, ByVal AnzConstr As Integer, ByVal AnzLocations As Integer, ByVal AnzVerzweig As Integer, ByVal AnzCombinations As Integer, ByVal AnzPathDimension() As Integer)

        'Schritt 1: CES - FORM SETTINGS
        'Optionen der Evolutionsstrategie werden übergeben
        Call CES_Form_Settings(Settings, Method, TestModus)

        'Schritt 2: CES - MODELL SETTINGS
        'Optionen der Evolutionsstrategie werden übergeben
        Call CES_Modell_Settings(TestModus, AnzPenalty, AnzConstr, AnzLocations, AnzVerzweig, AnzCombinations, AnzPathDimension, Method)

        'Schritt 3: CES - ReDim
        'Einige ReDims die erst mit den FormSetting oder ModelSetting möglich sind
        Call CES_ReDim()

    End Sub

    'Schritt 1: FORM SETTINGS
    'Function Form SETTINGS übergibt Optionen für Evolutionsstrategie und Prüft die eingestellten Optionen
    '***************************************************************************************************
    Private Sub CES_Form_Settings(ByRef Settings As EVO_Settings, ByVal Method As String, ByVal TestModus As CES_T_MODUS)

        'Überprüfung der Übergebenen Werte
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        If TestModus = CES_T_MODUS.No_Test Then

            If (Settings.CES.n_Parents < 3) Then
                Throw New Exception("Die Anzahl muss mindestens 3 sein!")
            End If
            If (Settings.CES.n_Childs < 3) Then
                Throw New Exception("Die Anzahl der Nachfahren muss mindestens 3 sein!")
            End If
            If (Settings.CES.n_Childs <= Settings.CES.n_Parents And Method <> "HYBRID") Then
                Throw New Exception("Die Anzahl der Eltern muss kleiner als die Anzahl der Nachfahren!" & Chr(13) & Chr(10) & "'Rechenberg 73' schlägt ein Verhältnis von 1:3 bis 1:5 vor.")
            End If

        End If

        If (Settings.CES.n_Generations < 1) Then
            Throw New Exception("Die Anzahl der Generationen ist kleiner 1!")
        End If
        If (Settings.CES.OptStrategie <> EVO_STRATEGIE.Komma_Strategie And Settings.CES.OptStrategie <> EVO_STRATEGIE.Plus_Strategie) Then
            Throw New Exception("Typ der Evolutionsstrategie ist nicht '+' oder ','")
        End If
        If (Settings.CES.OptReprodOp <> CES_REPRODOP.Selt_Rand_Uniform And CES_REPRODOP.Order_Crossover And CES_REPRODOP.Part_Mapped_Cross) Then
            Throw New Exception("Typ der Reproduction ist nicht richtig!")
        End If
        If (Settings.CES.OptMutOperator <> CES_MUTATION.RND_Switch And CES_MUTATION.Dyn_Switch) Then
            Throw New Exception("Typ der Mutation ist nicht richtig!")
        End If
        If (Settings.CES.n_MemberSecondPop < 1) Then
            Throw New Exception("Die Zahl der Mitglieder der sekundären Population ist kleiner 1!")
        End If
        If (Settings.CES.n_Interact < 1) Then
            Throw New Exception("Die Anzahl der Mitglieder des sekundären Population muss mindestens 1 sein!")
        End If
        If (Settings.CES.pr_MutRate < 0 Or Settings.CES.pr_MutRate > 100) Then
            Throw New Exception("Der Prozentsatz der Mutationrate muss zwischen 1 und 100 liegen!")
        End If
        If (Settings.CES.n_PES_MemSecPop < 1) Then
            Throw New Exception("Die Anzahl der Memeber für PES sekundäre Population muss mindestens 1 sein!")
        End If
        If (Settings.CES.n_PES_Interact < 1) Then
            Throw New Exception("Der Austausch mit der sekundären Population von PES muss mindestens 1 sein!")
        End If

        'Übergabe der Optionen
        'xxxxxxxxxxxxxxxxxxxxx

        Me.Settings = Settings

    End Sub

    'Schritt 3: PREPARE
    'A: Prüfung der ModellSetting in Kombination mit den Form Setting
    'B: Übergabe der ModellSettings
    '****************************************************************
    Private Sub CES_Modell_Settings(ByVal TestModus As CES_T_MODUS, ByVal AnzPenalty As Integer, ByVal AnzConstr As Integer, ByVal AnzLocations As Integer, ByVal AnzVerzweig As Integer, ByVal AnzCombinations As Integer, ByVal AnzPathDimension() As Integer, ByVal Method As String)

        If TestModus = CES_T_MODUS.No_Test Then

            'Prüft ob die Zahl mög. Kombinationen < Zahl Eltern + Nachfolger
            If (Settings.CES.n_Childs + Settings.CES.n_Parents) > AnzCombinations And Not Method = "HYBRID" Then
                Throw New Exception("Die Zahl der Eltern + die Zahl der Kinder ist größer als die mögliche Zahl der Kombinationen.")
            End If

        End If

        'Übergabe
        ModSett.n_Locations = AnzLocations
        ModSett.n_Verzweig = AnzVerzweig
        ModSett.n_PathDimension = AnzPathDimension.Clone

    End Sub

    'Schritt 3: ReDim
    'Einige ReDims die erst mit den FormSetting oder ModelSetting möglich sind
    '*************************************************************************
    Private Sub CES_ReDim()

        'Die Variablen für die Individuuen werden gesetzt
        '************************************************
        Call Individuum.Initialise(2, ModSett.n_Locations, 0)

        'Parents werden dimensioniert
        ReDim Parents(Settings.CES.n_Parents - 1)
        Call Individuum.New_Indi_Array("Parent", Parents)

        'Childs werden dimensioniert
        ReDim Childs(Settings.CES.n_Childs - 1)
        Call Individuum.New_Indi_Array("Child", Childs)

        'NDSorting wird dimensioniert
        ReDim NDSorting(Settings.CES.n_Childs + Settings.CES.n_Parents - 1)

        'NDSResult - Checken ob es verwendet wird
        ReDim NDSResult(Settings.CES.n_Childs + Settings.CES.n_Parents - 1)

    End Sub


    'Normaler Modus: Generiert zufällige Paths für alle Kinder BM Problem
    '*********************************************************************
    Public Sub Generate_Random_Path()
        Dim i, j As Integer
        Dim tmp As Integer
        Dim lowerb As Integer = 0
        Dim upperb As Integer
        Dim LoopCount As Integer = 0
        Randomize()

        For i = 0 To Settings.CES.n_Childs - 1
            Do
                For j = 0 To ModSett.n_Locations - 1
                    upperb = ModSett.n_PathDimension(j) - 1
                    'Randomize() nicht vergessen
                    tmp = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
                    Childs(i).Path(j) = tmp
                Next
                Childs(i).mutated = True
                Childs(i).ID = i + 1
                LoopCount += 1
                'If LoopCount = 100000 then
                '    throw new Exception("100000")
                'End If
            Loop While (Is_Twin(i) = True Or is_nullvariante(Childs(i).Path) = True) And Not LoopCount >= 1000
        Next

    End Sub

    'Testmodus 2: Funktion zum testen aller Kombinationen
    '****************************************************
    Public Sub Generate_Paths_for_Tests(ByVal Path() As Integer, ByVal Modus As CES_T_MODUS)

        Select Case Modus

            Case CES_T_MODUS.One_Combi
                'Testmodus 1: Funktion zum testen einer Kombination
                '**************************************************
                Childs(0).Path = Path.Clone

            Case CES_T_MODUS.All_Combis
                'Testmodus 2: Funktion zum testen aller Kombinationen
                '****************************************************
                Dim i, j As Integer

                Dim array() As Integer
                ReDim array(Childs(i).Path.GetUpperBound(0))
                For i = 0 To array.GetUpperBound(0)
                    array(i) = 0
                Next

                For i = 0 To Settings.CES.n_Childs - 1
                    For j = 0 To Childs(i).Path.GetUpperBound(0)
                        Childs(i).Path(j) = array(j)
                    Next
                    array(0) += 1
                    If Not i = Settings.CES.n_Childs - 1 Then
                        For j = 0 To Childs(i).Path.GetUpperBound(0)
                            If array(j) > ModSett.n_PathDimension(j) - 1 Then
                                array(j) = 0
                                array(j + 1) += 1
                            End If
                        Next
                    End If
                Next

        End Select

    End Sub
    'Hier kann man Pfade wie z.B. Nullvarianten die nicht erlaubt sind hard vercoden (ToDo!)
    '***************************************************************************************
    Public Function is_nullvariante(ByVal path() As Integer) As Boolean
        is_nullvariante = False

        'Dim i, j As Integer
        'Dim count As Integer = 0

        ''Dim firstValues()() As Byte = {New Byte() {2, 1}, New Byte() {3, 0}}
        ''Dim vector_array()() As Integer = {New Integer() {1, 1, 1}}
        'Dim vector_array()() As Integer = {New Integer() {0, 1, 1}}

        'For i = 0 To vector_array.GetUpperBound(0)
        '    If vector_array(i).GetUpperBound(0) = path.GetUpperBound(0) Then
        '        For j = 0 To vector_array(i).GetUpperBound(0)
        '            If vector_array(i)(j) = path(j) Then
        '                count += 1
        '            End If
        '        Next
        '        If count = path.GetLength(0) Then
        '            is_nullvariante = True
        '        End If
        '    End If
        'Next
    End Function

    'Setzt das Xn auf originale oder zufällige Parameter; und den Dn auf den Wert aus PES
    '************************************************************************************
    Public Sub Set_Xn_And_Dn_per_Location()
        Dim i, j, m As Integer
        'pro Child
        For i = 0 To Settings.CES.n_Childs - 1
            'und pro Location
            For j = 0 To ModSett.n_Locations - 1
                'Die Parameter (falls vorhanden) werden überschrieben
                If Not Childs(i).Loc(j).PES_OptPara.GetLength(0) = 0 Then
                    'Dem Child wird der Schrittweitenvektor zugewiesen und gegebenenfalls der Parameter zufällig gewählt
                    '***************************************************************************************************
                    For m = 0 To Childs(i).Loc(j).PES_OptPara.GetUpperBound(0)
                        Childs(i).Loc(j).PES_OptPara(m).Dn = Settings.PES.Schrittweite.DnStart
                        If Settings.PES.OptStartparameter = EVO_STARTPARAMETER.Zufall Then
                            Randomize()
                            Childs(i).Loc(j).PES_OptPara(m).Xn = Rnd()
                        End If
                    Next
                End If
            Next
        Next
    End Sub

    'Selectionsprozess je nach "plus" oder "minus" Strategie (Die beiden Listen sind schon vorsortiert!)
    '***************************************************************************************************
    Public Sub Selection_Process()
        Dim i, j As Integer
        Dim ChildQ_TMP As Double = 0
        Dim ParentQ_TMP As Double = 0

        'Strategie MINUS
        'xxxxxxxxxxxxxxx
        If Settings.CES.OptStrategie = "minus" Then 'CHECK: sollte das nicht = EVO_STRATEGIE.Komma_Strategie sein?
            For i = 0 To Settings.CES.n_Parents - 1
                Parents(i) = Childs(i).Clone()
            Next i

            'Strategie PLUS
            'xxxxxxxxxxxxxx
        ElseIf Settings.CES.OptStrategie = "plus" Then 'CHECK: sollte das nicht = EVO_STRATEGIE.Plus_Strategie sein?

            For i = 0 To Settings.CES.n_Childs - 1
                'Des schlechteste Elter wird bestimmt
                Dim bad_no As Integer = 0
                Dim bad_penalty As Double = Parents(0).Penalties(0)
                For j = 1 To Settings.CES.n_Parents - 1
                    If bad_penalty < Parents(j).Penalties(0) Then
                        bad_no = j
                        bad_penalty = Parents(j).Penalties(0)
                    End If
                Next

                'Falls der schlechteste Parent schlechter als der Child ist wird er durch den Child ersetzt
                If Parents(bad_no).Penalties(0) > Childs(i).Penalties(0) Then
                    Parents(bad_no) = Childs(i).Clone()
                End If
            Next

        End If

    End Sub

    'Reproductionsfunktionen
    'XXXXXXXXXXXXXXXXXXXXXXX

    'Steuerung der Reproduktionsoperatoren
    '*************************************
    Public Sub Reproduction_Control()
        Dim i As Integer
        Dim x, y As Integer
        Dim Einzelkind(ModSett.n_Locations - 1) As Integer

        Select Case Settings.CES.OptReprodOp
            'UPGRADE: Eltern werden nicht zufällig gewählt sondern immer in Top Down Reihenfolge
            Case CES_REPRODOP.Selt_Rand_Uniform
                x = 0
                y = 1
                For i = 0 To Settings.CES.n_Childs - 2 Step 2
                    Call ReprodOp_Select_Random_Uniform(Parents(x).Path, Parents(y).Path, Childs(i).Path, Childs(i + 1).Path)
                    x += 1
                    y += 1
                    If x = Settings.CES.n_Parents - 1 Then x = 0
                    If y = Settings.CES.n_Parents - 1 Then y = 0
                Next i
                If Even_Number(Settings.CES.n_Childs) = False Then
                    Call ReprodOp_Select_Random_Uniform(Parents(x).Path, Parents(y).Path, Childs(Settings.CES.n_Childs - 1).Path, Einzelkind)
                End If

            Case CES_REPRODOP.Order_Crossover

                x = 0
                y = 1
                For i = 0 To Settings.CES.n_Childs - 2 Step 2
                    Call ReprodOp_Order_Crossover(Parents(x).Path, Parents(y).Path, Childs(i).Path, Childs(i + 1).Path)
                    x += 1
                    y += 1
                    If x = Settings.CES.n_Parents - 1 Then x = 0
                    If y = Settings.CES.n_Parents - 1 Then y = 0
                Next i
                If Even_Number(Settings.CES.n_Childs) = False Then
                    Call ReprodOp_Order_Crossover(Parents(x).Path, Parents(y).Path, Childs(Settings.CES.n_Childs - 1).Path, Einzelkind)
                End If

            Case CES_REPRODOP.Part_Mapped_Cross
                x = 0
                y = 1
                For i = 0 To Settings.CES.n_Childs - 2 Step 2
                    Call ReprodOp_Part_Mapped_Crossover(Parents(x).Path, Parents(y).Path, Childs(i).Path, Childs(i + 1).Path)
                    x += 1
                    y += 1
                    If x = Settings.CES.n_Parents - 1 Then x = 0
                    If y = Settings.CES.n_Parents - 1 Then y = 0
                Next i
                If Even_Number(Settings.CES.n_Childs) = False Then
                    Call ReprodOp_Part_Mapped_Crossover(Parents(x).Path, Parents(y).Path, Childs(Settings.CES.n_Childs - 1).Path, Einzelkind)
                End If

        End Select

    End Sub

    'Reproductionsoperator: "Select_Random_Uniform"
    'Entscheidet zufällig ob der Wert aus dem Path des Elter_A oder Elter_B verwendet wird
    '*************************************************************************************
    Private Sub ReprodOp_Select_Random_Uniform(ByVal ParPath_A() As Integer, ByVal ParPath_B() As Integer, ByRef ChildPath_A() As Integer, ByRef ChildPath_B() As Integer)

        Dim i As Integer

        For i = 0 To ChildPath_A.GetUpperBound(0)    'TODO: Es müsste eigentlich eine definierte Pfadlänge geben
            If Bernoulli() = True Then
                ChildPath_A(i) = ParPath_B(i)
            Else
                ChildPath_A(i) = ParPath_A(i)
            End If
        Next

        For i = 0 To ChildPath_B.GetUpperBound(0)    'TODO: Es müsste eigentlich eine definierte Pfadlänge geben
            If Bernoulli() = True Then
                ChildPath_B(i) = ParPath_A(i)
            Else
                ChildPath_B(i) = ParPath_B(i)
            End If
        Next

    End Sub


    'Reproductionsoperator "Order_Crossover (OX)"
    'Kopiert den mittleren Teil des einen Elter und füllt den Rest aus der Reihenfolge des anderen Elter auf
    'UPGRADE: Es wird immer nur der mittlere Teil Kopiert, könnte auch mal ein einderer sein
    Private Sub ReprodOp_Order_Crossover(ByVal ParPath_A() As Integer, ByVal ParPath_B() As Integer, ByRef ChildPath_A() As Integer, ByRef ChildPath_B() As Integer)

        Dim i As Integer
        Dim x, y As Integer

        Dim CutPoint(1) As Integer
        Call Create_n_Cutpoints(CutPoint)

        'Kopieren des mittleren Paths
        For i = CutPoint(0) + 1 To CutPoint(1)
            ChildPath_A(i) = ParPath_A(i)
            ChildPath_B(i) = ParPath_B(i)
        Next
        'Auffüllen des Paths Teil 3 des Childs A mit dem anderen Elter beginnend bei 0
        x = 0
        For i = CutPoint(1) + 1 To ModSett.n_Locations - 1
            If Is_No_OK(ParPath_B(x), ChildPath_A) Then
                ChildPath_A(i) = ParPath_B(x)
            Else
                i -= 1
            End If
            x += 1
        Next
        'Auffüllen des Paths Teil 3 des Childs B mit dem anderen Elter beginnend bei 0
        y = 0
        For i = CutPoint(1) + 1 To ModSett.n_Locations - 1
            If Is_No_OK(ParPath_A(y), ChildPath_B) Then
                ChildPath_B(i) = ParPath_A(y)
            Else
                i -= 1
            End If
            y += 1
        Next
        'Auffüllen des Paths Teil 1 des Childs A mit dem anderen Elter beginnend bei 0
        For i = 0 To CutPoint(0)
            If Is_No_OK(ParPath_B(x), ChildPath_A) Then
                ChildPath_A(i) = ParPath_B(x)
            Else
                i -= 1
            End If
            x += 1
        Next
        'Auffüllen des Paths Teil 1 des Childs B mit dem anderen Elter beginnend bei 0
        For i = 0 To CutPoint(0)
            If Is_No_OK(ParPath_A(y), ChildPath_B) Then
                ChildPath_B(i) = ParPath_A(y)
            Else
                i -= 1
            End If
            y += 1
        Next
    End Sub

    'Reproductionsoperator: "Partially_Mapped_Crossover (PMX)"
    'Kopiert den mittleren Teil des anderen Elter und füllt den Rest mit dem eigenen auf. Falls Doppelt wird gemaped.
    Public Sub ReprodOp_Part_Mapped_Crossover(ByVal ParPath_A() As Integer, ByVal ParPath_B() As Integer, ByRef ChildPath_A() As Integer, ByRef ChildPath_B() As Integer)
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

        'Auffüllen des Paths Teil 1 des Childs A und B mit dem anderen Elter beginnend bei 0
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

        'Auffüllen des Paths Teil 3 des Childs A und B mit dem anderen Elter beginnend bei 0
        For i = CutPoint(1) + 1 To ModSett.n_Locations - 1
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


    'Mutationsfunktionen
    'XXXXXXXXXXXXXXXXXXX

    'Steuerung der Mutationsoperatoren
    '*********************************
    Public Sub Mutation_Control()
        Dim i As Integer

        For i = 0 To Settings.CES.n_Childs - 1
            Dim count As Integer = 0
            Do
                Select Case Settings.CES.OptMutOperator
                    Case CES_MUTATION.RND_Switch
                        'Verändert zufällig ein gen des Paths
                        Call MutOp_RND_Switch(Childs(i).Path)
                    Case CES_MUTATION.Dyn_Switch
                        'Verändert zufällig ein gen des Paths mit dynamisch erhöhter Mutationsrate
                        Call MutOp_Dyn_Switch(Childs(i).Path, count)
                End Select
                count += 1
            Loop While is_nullvariante(Childs(i).Path) = True And Not count >= 1000
            Childs(i).mutated = True
        Next

    End Sub

    'Mutationsoperator "RND_Switch"
    'Verändert zufällig ein gen des Paths
    '************************************
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
            If Tmp_a <= Settings.CES.pr_MutRate Then
                upperb_b = ModSett.n_PathDimension(i) - 1
                'Randomize() nicht vergessen
                Tmp_b = CInt(Int((upperb_b - lowerb_b + 1) * Rnd() + lowerb_b))
                Path(i) = Tmp_b
            End If
        Next

    End Sub

    'Mutationsoperator "Dyn_Switch"
    'Verändert zufällig ein gen des Paths mit dynamisch erhöhter Mutationsrate
    '*************************************************************************
    Private Sub MutOp_Dyn_Switch(ByVal Path() As Integer, ByVal Dyn_MutRate As Integer)
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
            If Tmp_a <= Dyn_MutRate Then
                upperb_b = ModSett.n_PathDimension(i) - 1
                'Randomize() nicht vergessen
                Tmp_b = CInt(Int((upperb_b - lowerb_b + 1) * Rnd() + lowerb_b))
                Path(i) = Tmp_b
            End If
        Next

    End Sub

    'Speichert die Child Ergebnisse im PES Memory
    '********************************************
    Sub Memory_Store(ByVal Child_No As Integer, ByVal Gen_No As Integer)

        Dim neu As Integer

        If (Gen_No = 0 And Child_No = 0) Then
            ReDim PES_Memory(0)
        Else
            ReDim Preserve PES_Memory(PES_Memory.GetLength(0))
            neu = PES_Memory.GetUpperBound(0)
        End If

        PES_Memory(neu) = New Individuum("Memory", neu)

        PES_Memory(neu) = Childs(Child_No).Clone()
        PES_Memory(neu).Generation = Gen_No

    End Sub

    'Durchsucht den Memory - Der PES_Parantsatz für jedes Child wird hier ermittelt
    'Eine Liste (PES_Parents_pChild) mit Eltern für ein Child incl. der Location Information wird erstellt
    '**********************************************************************************************
    Sub Memory_Search_per_Child(ByVal Child As Individuum)

        Dim i_loc, i_mem As Integer
        Dim akt As Integer = 0

        ReDim PES_Parents_pChild(0)
        PES_Parents_pChild(0) = New Individuum("PES_Parent", 0)

        Select Case settings.CES.Mem_Strategy

            Case MEMORY_STRATEGY.B_One_Loc_Up, MEMORY_STRATEGY.A_Two_Loc_Up

                For i_loc = 0 To ModSett.n_Locations - 1
                    For i_mem = 0 To PES_Memory.GetUpperBound(0)

                        'Rank Nummer 0 (Lediglich Übereinstimmung in der Location selbst)
                        If Child.Path(i_loc) = PES_Memory(i_mem).Path(i_loc) Then
                            ReDim Preserve PES_Parents_pChild(PES_Parents_pChild.GetLength(0))
                            akt = PES_Parents_pChild.GetUpperBound(0)
                            PES_Parents_pChild(akt) = New Individuum("PES_Parent", PES_Parents_pChild.GetUpperBound(0))
                            PES_Parents_pChild(akt) = PES_Memory(i_mem).Clone()
                            PES_Parents_pChild(akt).iLocation = i_loc + 1
                            PES_Parents_pChild(akt).Memory_Strat = MEMORY_STRATEGY.C_This_Loc
                        End If

                        'Rank Nummer -1 (Übereinstimmung in der Downsteam Location 1 und 2)
                        If Not i_loc = 0 And Settings.CES.Mem_Strategy < 0 Then
                            If Child.Path(i_loc) = PES_Memory(i_mem).Path(i_loc) And Child.Path(i_loc - 1) = PES_Memory(i_mem).Path(i_loc - 1) Then
                                ReDim Preserve PES_Parents_pChild(PES_Parents_pChild.GetLength(0))
                                akt = PES_Parents_pChild.GetUpperBound(0)
                                PES_Parents_pChild(akt) = New Individuum("PES_Parent", PES_Parents_pChild.GetUpperBound(0))
                                PES_Parents_pChild(akt) = PES_Memory(i_mem).Clone()
                                PES_Parents_pChild(akt).iLocation = i_loc + 1
                                PES_Parents_pChild(akt).Memory_Strat = MEMORY_STRATEGY.B_One_Loc_Up
                            End If
                        End If

                        'Rank Nummer -2 (Übereinstimmung in Downstream Location 1, 2 und 3)
                        If Not (i_loc = 0 Or i_loc = 1) And Settings.CES.Mem_Strategy < -1 Then
                            If Child.Path(i_loc) = PES_Memory(i_mem).Path(i_loc) And Child.Path(i_loc - 1) = PES_Memory(i_mem).Path(i_loc - 1) And Child.Path(i_loc - 2) = PES_Memory(i_mem).Path(i_loc - 2) Then
                                ReDim Preserve PES_Parents_pChild(PES_Parents_pChild.GetLength(0))
                                akt = PES_Parents_pChild.GetUpperBound(0)
                                PES_Parents_pChild(akt) = New Individuum("PES_Parent", PES_Parents_pChild.GetUpperBound(0))
                                PES_Parents_pChild(akt) = PES_Memory(i_mem).Clone()
                                PES_Parents_pChild(akt).iLocation = i_loc + 1
                                PES_Parents_pChild(akt).Memory_Strat = MEMORY_STRATEGY.A_Two_Loc_Up
                            End If
                        End If

                    Next
                Next

            Case MEMORY_STRATEGY.C_This_Loc, MEMORY_STRATEGY.D_One_Loc_Down, MEMORY_STRATEGY.E_Two_Loc_Down

                For i_loc = 0 To ModSett.n_Locations - 1
                    For i_mem = 0 To PES_Memory.GetUpperBound(0)

                        'Strategy Nummer 0 (Lediglich Übereinstimmung in der Location selbst)
                        If Child.Path(i_loc) = PES_Memory(i_mem).Path(i_loc) Then
                            ReDim Preserve PES_Parents_pChild(PES_Parents_pChild.GetLength(0))
                            akt = PES_Parents_pChild.GetUpperBound(0)
                            PES_Parents_pChild(akt) = New Individuum("PES_Parent", PES_Parents_pChild.GetUpperBound(0))
                            PES_Parents_pChild(akt) = PES_Memory(i_mem).Clone()
                            PES_Parents_pChild(akt).iLocation = i_loc + 1
                            PES_Parents_pChild(akt).Memory_Strat = MEMORY_STRATEGY.C_This_Loc
                        End If

                        'Strategy Nummer 1 (Übereinstimmung in der Downsteam Location 1 und 2)
                        If Not i_loc = ModSett.n_Locations - 1 And Settings.CES.Mem_Strategy > 0 Then
                            If Child.Path(i_loc) = PES_Memory(i_mem).Path(i_loc) And Child.Path(i_loc + 1) = PES_Memory(i_mem).Path(i_loc + 1) Then
                                ReDim Preserve PES_Parents_pChild(PES_Parents_pChild.GetLength(0))
                                akt = PES_Parents_pChild.GetUpperBound(0)
                                PES_Parents_pChild(akt) = New Individuum("PES_Parent", PES_Parents_pChild.GetUpperBound(0))
                                PES_Parents_pChild(akt) = PES_Memory(i_mem).Clone()
                                PES_Parents_pChild(akt).iLocation = i_loc + 1
                                PES_Parents_pChild(akt).Memory_Strat = MEMORY_STRATEGY.D_One_Loc_Down
                            End If
                        End If

                        'Strategy Nummer 3 (Übereinstimmung in Downstream Location 1, 2 und 3)
                        If Not (i_loc = ModSett.n_Locations - 1 Or i_loc = ModSett.n_Locations - 2) And Settings.CES.Mem_Strategy > 1 Then
                            If Child.Path(i_loc) = PES_Memory(i_mem).Path(i_loc) And Child.Path(i_loc + 1) = PES_Memory(i_mem).Path(i_loc + 1) And Child.Path(i_loc + 2) = PES_Memory(i_mem).Path(i_loc + 2) Then
                                ReDim Preserve PES_Parents_pChild(PES_Parents_pChild.GetLength(0))
                                akt = PES_Parents_pChild.GetUpperBound(0)
                                PES_Parents_pChild(akt) = New Individuum("PES_Parent", PES_Parents_pChild.GetUpperBound(0))
                                PES_Parents_pChild(akt) = PES_Memory(i_mem).Clone()
                                PES_Parents_pChild(akt).iLocation = i_loc + 1
                                PES_Parents_pChild(akt).Memory_Strat = MEMORY_STRATEGY.E_Two_Loc_Down
                            End If
                        End If
                    Next
                Next

        End Select

        'Die Doppelten niedrigeren Ränge werden gelöscht - und der erste leere Datensatz
        '!Die Liste kann größer als die Liste des Momory sein, ein eine Lösung für verschiedene Lokations verwendet werden kann
        Call Memory_Dubletten_loeschen(PES_Parents_pChild)

        'Die die nicht der eingestellten Memory Strategy entsprechen werden entfernt
        Call Memory_Delete_too_weak(PES_Parents_pChild)

    End Sub

    'Löscht wenn ein Individuum bei der gleichen Lokation einmal als Rank 1 und einmal als Rank 2 definiert.
    'Bei Rank 2 entsprechnd Rank 3. Außerdem wird der erste leere Datensatz geloescht.
    '*******************************************************************************************************
    Private Sub Memory_Dubletten_loeschen(ByRef PES_Parents_pChild() As Individuum)

        Dim tmp(PES_Parents_pChild.GetUpperBound(0) - 1) As Individuum
        Call Individuum.New_Indi_Array("tmp", tmp)
        Dim isDouble As Boolean
        Dim i, j, x As Integer

        x = 0
        For i = 1 To PES_Parents_pChild.GetUpperBound(0)
            isDouble = False
            For j = 1 To PES_Parents_pChild.GetUpperBound(0)
                If i <> j And PES_Parents_pChild(i).iLocation And Memory_is_PES_Double(PES_Parents_pChild(i), PES_Parents_pChild(j)) Then
                    isDouble = True
                End If
            Next
            If isDouble = False Then
                tmp(x) = PES_Parents_pChild(i).Clone()
                x += 1
            End If
        Next

        ReDim Preserve tmp(x - 1)
        ReDim Preserve PES_Parents_pChild(x - 1)

        For i = 0 To tmp.GetUpperBound(0)
            PES_Parents_pChild(i) = tmp(i).Clone()
        Next

    End Sub

    'Prüft ob die beiden den gleichen Pfad und zur gleichen location gehören
    '***********************************************************************
    Private Function Memory_is_PES_Double(ByVal First As Individuum, ByVal Second As Individuum) As Boolean
        Memory_is_PES_Double = False

        Dim count As Integer = 0
        Dim i As Integer

        'Prüfung ob beide den gleichen Pfad haben, und lediglich die Memory Strategy unterschiedlich ist
        For i = 0 To First.Path.GetUpperBound(0)
            If First.Path(i) = Second.Path(i) Then
                count += 1
            End If
        Next

        'Falls der Pfad der gleiche ist und die Location die gleiche ist wird einer aussortiert
        If count = First.Path.GetLength(0) Then
            If First.iLocation = Second.iLocation Then
                'Fallunterscheidung
                Select Case settings.CES.Mem_Strategy
                    Case MEMORY_STRATEGY.B_One_Loc_Up, MEMORY_STRATEGY.A_Two_Loc_Up
                        'UPSTREAM
                        If First.Memory_Strat > Second.Memory_Strat Then
                            Memory_is_PES_Double = True
                        End If
                    Case MEMORY_STRATEGY.C_This_Loc, MEMORY_STRATEGY.D_One_Loc_Down, MEMORY_STRATEGY.E_Two_Loc_Down
                        'DOWNSTREAM
                        If First.Memory_Strat < Second.Memory_Strat Then
                            Memory_is_PES_Double = True
                        End If
                End Select
            End If
        End If

    End Function

    'Die die nicht die entsprechende Memory Strategy haben werden aussortiert
    '************************************************************************
    Private Sub Memory_Delete_too_weak(ByRef PES_Parents_pChild() As Individuum)

        Dim i As Integer
        Dim Tmp(-1) As Individuum

        For i = 0 To PES_Parents_pChild.GetUpperBound(0)

            Select Case Settings.CES.Mem_Strategy

                Case MEMORY_STRATEGY.B_One_Loc_Up, MEMORY_STRATEGY.A_Two_Loc_Up
                    'UPSTREAM
                    If PES_Parents_pChild(i).Memory_Strat <= settings.CES.Mem_Strategy - Math.Min(0, PES_Parents_pChild(i).iLocation + settings.CES.Mem_Strategy - 1) Then
                        ReDim Preserve Tmp(Tmp.GetLength(0))
                        Tmp(Tmp.GetUpperBound(0)) = New Individuum("PES_Parent", Tmp.GetUpperBound(0))
                        Tmp(Tmp.GetUpperBound(0)) = PES_Parents_pChild(i).Clone
                    End If

                Case MEMORY_STRATEGY.C_This_Loc
                    Tmp = PES_Parents_pChild.Clone

                Case MEMORY_STRATEGY.D_One_Loc_Down, MEMORY_STRATEGY.E_Two_Loc_Down
                    'DOWNSTREAM
                    If PES_Parents_pChild(i).Memory_Strat >= settings.CES.Mem_Strategy + Math.Min(0, Modsett.n_Locations - settings.CES.Mem_Strategy - PES_Parents_pChild(i).iLocation) Then
                        ReDim Preserve Tmp(Tmp.GetLength(0))
                        Tmp(Tmp.GetUpperBound(0)) = New Individuum("PES_Parent", Tmp.GetUpperBound(0))
                        Tmp(Tmp.GetUpperBound(0)) = PES_Parents_pChild(i).Clone
                    End If
            End Select
        Next

        PES_Parents_pChild = Tmp.Clone

    End Sub

    'Durchsucht des PES_Perent_pChild - Der PES_Parantsatz für jede Location wird hier ermittelt
    'Eine Liste (PES_Parents_pLoc) für jede Location wird erstellt
    '**********************************************************************************************
    Sub Memory_Search_per_Location(ByVal iLoc As Integer)

        Dim i As Integer
        ReDim PES_Parents_pLoc(-1)

        Dim x As Integer = 0
        For i = 0 To PES_Parents_pChild.GetUpperBound(0)
            If PES_Parents_pChild(i).iLocation = iLoc + 1 Then
                ReDim Preserve PES_Parents_pLoc(x)
                PES_Parents_pLoc(x) = PES_Parents_pChild(i).Clone()
                x += 1
            End If
        Next
    End Sub

    'Füllt die PES Parents per Location auf die erforderliche Anzahl auf
    '*******************************************************************
    Public Sub fill_Parents_per_Loc(ByRef Parents_pLoc() As Individuum, ByVal n_eltern As Integer)

        Dim i, x As Integer
        Dim n As Integer = Parents_pLoc.GetLength(0)
        ReDim Preserve Parents_pLoc(n_eltern - 1)

        x = 0
        For i = n To n_eltern - 1
            Parents_pLoc(i) = Parents_pLoc(x).Clone
            x += 1
            If x = n Then x = 0
        Next

    End Sub

    'Hilfsfunktionen
    'XXXXXXXXXXXXXXX

    'Hilfsfunktion um zu Prüfen ob eine Zahl bereits in einem Array vorhanden ist oder nicht
    '***************************************************************************************
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

    'Hilfsfunktion zum sortieren der Individuum
    '******************************************
    Public Sub Sort_Individuum(ByRef IndividuumList() As Individuum)
        'Sortiert die Fiksimile anhand des Abstandes
        Dim i, j As Integer
        Dim swap As New Individuum("swap", 0)

        For i = 0 To IndividuumList.GetUpperBound(0)
            For j = 0 To IndividuumList.GetUpperBound(0)
                If IndividuumList(i).Penalties(0) < IndividuumList(j).Penalties(0) Then
                    swap = IndividuumList(i)
                    IndividuumList(i) = IndividuumList(j)
                    IndividuumList(j) = swap
                End If
            Next j
        Next i

    End Sub

    'Hilfsfunktion checkt ob die neuen Childs Zwillinge sind. Nur beim Generieren der ersten Pfade.
    '**********************************************************************************************
    Private Function Is_Twin(ByVal ChildIndex As Integer) As Boolean
        Dim n As Integer = 0
        Dim i, j As Integer
        Dim PathOK As Boolean
        PathOK = False
        Is_Twin = False

        For i = 0 To Settings.CES.n_Childs - 1
            If ChildIndex <> i And Childs(i).mutated = True Then
                PathOK = False
                For j = 0 To Childs(ChildIndex).Path.GetUpperBound(0)
                    If Childs(ChildIndex).Path(j) <> Childs(i).Path(j) Then
                        PathOK = True
                    End If
                Next
                If PathOK = False Then
                    Is_Twin = True
                End If
            End If
        Next
    End Function

    'Hilfsfunktion checkt ob die neuen Childs Klone sind - Deaktiviert!
    '******************************************************************
    Private Function Is_Clone(ByVal ChildIndex As Integer) As Boolean
        Dim i, j As Integer
        Dim PathOK As Boolean
        PathOK = False
        Is_Clone = False

        For i = 0 To Settings.CES.n_Parents - 1
            PathOK = False
            For j = 0 To Childs(ChildIndex).Path.GetUpperBound(0)
                If Childs(ChildIndex).Path(j) <> Parents(i).Path(j) Then
                    PathOK = True
                End If
            Next
            If PathOK = False Then
                Is_Clone = True
            End If
        Next

    End Function

    'Hilfsfunktion generiert Bernoulli verteilte Zufallszahl
    '*******************************************************
    Public Function Bernoulli() As Boolean
        Dim lowerb As Integer = 0
        Dim upperbo As Integer = 1
        Bernoulli = CInt(Int(2 * Rnd()))
    End Function

    'Hilfsfunktion: Gerade oder Ungerade Zahl
    '****************************************
    Public Function Even_Number(ByVal Number As Integer) As Boolean
        Dim tmp_a As Double
        Dim tmp_b As Double
        Dim tmp_c As Double
        tmp_a = Number / 2
        tmp_b = Math.Ceiling(tmp_a)
        tmp_c = tmp_a - tmp_b
        If tmp_c = 0 Then Even_Number = True
    End Function

    'Hilfsfunktion zum generieren von zufälligen Schnittpunkten innerhalb eines Pfades
    'Mit Bernoulli Verteilung mal von rechts mal von links
    '*****************************************************
    Public Sub Create_n_Cutpoints(ByRef CutPoint() As Integer)
        'Generiert zwei CutPoints
        Dim i As Integer
        Dim lowerb As Integer
        Dim upperb As Integer

        'wird zufällig entweder von Link oder von Rechts geschnitten
        If Bernoulli() = True Then
            lowerb = 0
            For i = 0 To CutPoint.GetUpperBound(0)
                upperb = ModSett.n_Locations - CutPoint.GetLength(0) - 1 + i
                CutPoint(i) = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
                lowerb = CutPoint(i) + 1
            Next i
        Else
            upperb = ModSett.n_Locations - 2
            For i = CutPoint.GetUpperBound(0) To 0 Step -1
                lowerb = i
                CutPoint(i) = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
                upperb = CutPoint(i) - 1
            Next i
        End If

    End Sub

    'NonDominated Sorting
    'XXXXXXXXXXXXXXXXXXXX

    'Steuerung des NDSorting (Ursprünglich aus ES Eltern)
    '****************************************************
    Public Sub NDSorting_CES_Control(ByVal iAktGen As Integer)
        Dim i As Short

        Dim NDSorting(Settings.CES.n_Childs + Settings.CES.n_Parents - 1) As Individuum
        Call Individuum.New_Indi_Array("NDSorting", NDSorting)

        '0. Eltern und Nachfolger werden gemeinsam betrachtet
        'Die Kinder werden NDSorting hinzugefügt
        '-------------------------------------------

        For i = 0 To Settings.CES.n_Childs - 1
            NDSorting(i) = Childs(i).Clone()
            NDSorting(i).dominated = False
            NDSorting(i).Front = 0
            NDSorting(i).Distance = 0
        Next i

        '1. Eltern und Nachfolger werden gemeinsam betrachtet
        'Nur Eltern werden NDSorting hinzugefügt, Kinder sind schon oben drin
        '--------------------------------------------------------------------

        For i = Settings.CES.n_Childs To Settings.CES.n_Childs + Settings.CES.n_Parents - 1
            NDSorting(i) = Parents(i - Settings.CES.n_Childs).Clone()
            NDSorting(i).dominated = False
            NDSorting(i).Front = 0
            NDSorting(i).Distance = 0
        Next i

        '********************* Alles in der Klasse Functions ****************************************
        '2. Die einzelnen Fronten werden bestimmt
        '3. Der Bestwertspeicher wird entsprechend der Fronten oder der sekundären Population gefüllt
        '4: Sekundäre Population wird bestimmt und gespeichert
        '--------------------------------
        Dim Func1 As New Kern.Functions(Settings.CES.n_Childs, Settings.CES.n_Parents, Settings.CES.n_MemberSecondPop, Settings.CES.n_Interact, Settings.CES.is_SecPop, iAktGen + 1)
        Call Func1.EsEltern_Pareto(Parents, NDSorting, SekundärQb)
        '********************************************************************************************

        'Schritt 5: ist für CES nicht notwenig, da die Parents ByRef zurückgegeben werden
        'und das rüberschreiebn der SekundärQB in Functions erfolgt

        ''5: Neue Eltern werden gleich dem Bestwertspeicher gesetzt
        ''---------------------------------------------------------
        'For i = 0 To PES_Settings.NEltern - 1
        '    For v = 0 To Anz.Para - 1
        '        De(v, i, PES_iAkt.iAktPop) = Best.Db(v, i, PES_iAkt.iAktPop)
        '        Xe(v, i, PES_iAkt.iAktPop) = Best.Xb(v, i, PES_iAkt.iAktPop)
        '    Next v
        'Next i

        'Schritt 6: nicht so spannend

        ''6: Sortierung der Lösungen ist nur für Neighbourhood-Rekombination notwendig
        ''----------------------------------------------------------------------------
        'If (PES_Settings.iOptEltern = EVO_ELTERN.Neighbourhood) Then
        '    Call Neighbourhood_AbstandsArray()
        '    Call Neighbourhood_Crowding_Distance()
        'End If

    End Sub

    'Sortiert das PES Parents per Location
    'Sekundär_QB wird hier nicht berücksichtigt (schlicht ausgeschaltet mit no_interact)
    '***********************************************************************************
    Sub NDSorting_PES_Parents_per_Loc(ByVal iAktGen As Integer)

        Dim i As Short

        Dim NDSorting(PES_Parents_pLoc.GetUpperBound(0)) As Individuum
        Call Individuum.New_Indi_Array("NDSorting", NDSorting)

        '1. ALLE werden reinkopiert (anders als beim normalen Verfahren)
        '---------------------------------------------------------------
        For i = 0 To PES_Parents_pLoc.GetUpperBound(0)
            NDSorting(i) = PES_Parents_pLoc(i).Clone()
            NDSorting(i).dominated = False
            NDSorting(i).Front = 0
            NDSorting(i).Distance = 0
        Next i

        'Die Anzahlen werden hier speziell errechnet
        Dim n_PES_Childs As Integer
        n_PES_Childs = PES_Parents_pLoc.GetLength(0) - Settings.PES.n_Eltern

        'Die Eltern werden zurückgesetzt
        ReDim PES_Parents_pLoc(Settings.PES.n_Eltern - 1)

        '********************* Alles in der Klasse Functions ****************************************
        '2. Die einzelnen Fronten werden bestimmt
        '3. Der Bestwertspeicher wird entsprechend der Fronten oder der sekundären Population gefüllt
        '4: Sekundäre Population wird bestimmt und gespeichert
        '--------------------------------

        '! Sekundär_QB wird hier nicht berücksichtigt da die PES Generationen !
        '! wegen der reduzierung auf Locations entkoppelt ist                 !
        Dim Fake_SekundärQb(-1) As Individuum
        Dim Func1 As New Kern.Functions(n_PES_Childs, Settings.PES.n_Eltern, Settings.CES.n_PES_MemSecPop, Settings.CES.n_PES_Interact, False, iAktGen + 1)
        Call Func1.EsEltern_Pareto(PES_Parents_pLoc, NDSorting, Fake_SekundärQb)
        '********************************************************************************************

    End Sub

    'NDSorting incl. SekundärQB mit den EInstellungen aus dem PES
    '*************************************************************
    Sub NDSorting_Memory(ByVal iAktGen As Integer)

        Dim i As Short

        Dim NDSorting(PES_Memory.GetUpperBound(0)) As Individuum
        Call Individuum.New_Indi_Array("NDSorting", NDSorting)

        '1. ALLE werden reinkopiert (anders als beim normalen Verfahren, aber wie bei per Location)
        '------------------------------------------------------------------------------------------
        For i = 0 To PES_Memory.GetUpperBound(0)
            NDSorting(i) = PES_Memory(i).Clone()
            NDSorting(i).dominated = False
            NDSorting(i).Front = 0
            NDSorting(i).Distance = 0
        Next i

        'Die Anzahlen werden hier speziell errechnet
        Dim n_PES_Mem_Childs As Integer
        n_PES_Mem_Childs = PES_Memory.GetLength(0) - Settings.CES.n_PES_MemSize

        'Die Eltern werden zurückgesetzt
        ReDim PES_Memory(Settings.CES.n_PES_MemSize - 1)

        '********************* Alles in der Klasse Functions ****************************************
        '2. Die einzelnen Fronten werden bestimmt
        '3. Der Bestwertspeicher wird entsprechend der Fronten oder der sekundären Population gefüllt
        '4: Sekundäre Population wird bestimmt und gespeichert
        '--------------------------------
        'Sekundär_QB wird hier berücksichtigt!
        Dim Func1 As New Kern.Functions(n_PES_Mem_Childs, Settings.CES.n_PES_MemSize, Settings.PES.n_MemberSekPop, Settings.PES.n_Interact, Settings.PES.is_Interact, iAktGen + 1)
        Call Func1.EsEltern_Pareto(PES_Memory, NDSorting, PES_Mem_SekundärQb)
        '********************************************************************************************

    End Sub

#End Region 'Methoden

End Class



