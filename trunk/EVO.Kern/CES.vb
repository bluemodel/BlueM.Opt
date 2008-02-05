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

#Region "Eigenschaften"
    '##################

    'Die Settings für alles aus dem Form
    Public Settings As Evo_Settings             'TODO: sollte private sein!
    
    'Modell Setting
    Public Structure ModSettings
        Public n_Penalty As Integer             'Anzahl der Ziele
        Public n_Constrain As Integer           'Anzahl der Randbedingungen
        Public n_Locations As Integer           'Anzahl der Locations
        Public n_Verzweig As Integer            'Anzahl der Verzweigungen in der Verzweigungsdatei
        Public n_Combinations As Integer        'Anzahl aller Kombinationen
        'Test Modus müsste als ENUM abgelegt werden
        Public TestModus As Integer             'Gibt den Testmodus
        Public n_PathDimension() As Integer     'Anzahl der Maßnahmen an jedem Ort
    End Structure

    Public ModSett as ModSettings

    'Listen für die Individuen
    '*************************
    Public Childs() As EVO.Kern.Individuum
    Public Parents() As EVO.Kern.Individuum
    Private SekundärQb(-1) As EVO.Kern.Individuum
    Public NDSorting() As EVO.Kern.Individuum
    'Checken ob es verwendet wird
    Public NDSResult() As EVO.Kern.Individuum

    'Für Hybrid
    Public Memory() As EVO.Kern.Individuum
    Public PES_Parents_pChild() As EVO.Kern.Individuum
    Public PES_Parents_pLoc() As EVO.Kern.Individuum
    Private PES_SekundärQb(-1) As EVO.Kern.Individuum

#End Region 'Eigenschaften

#Region "Methoden"
    '#############

    'Initialisierung der PES
    '***************************************
    Public Sub CESInitialise(ByRef Settings As evo_settings, ByVal Method As String, ByVal AnzPenalty As Integer, ByVal AnzConstr As Integer, byval AnzLocations as Integer, byval AnzVerzweig as Integer, byval AnzCombinations as Integer, byval TypeTestModus as Integer, byval AnzPathDimension() as Integer)

        'Schritt 1: CES - FORM SETTINGS
        'Optionen der Evolutionsstrategie werden übergeben
        Call CES_Form_Settings(Settings, Method)

        'Schritt 2: CES - MODELL SETTINGS
        'Optionen der Evolutionsstrategie werden übergeben
        Call CES_Modell_Settings(AnzPenalty, AnzConstr, AnzLocations, AnzVerzweig, AnzCombinations, TypeTestModus, AnzPathDimension)

        'Schritt 3: CES - ReDim
        'Einige ReDims die erst mit den FormSetting oder ModelSetting möglich sind
        Call CES_ReDim()

    End Sub

    'Schritt 1: FORM SETTINGS
    'Function Form SETTINGS übergibt Optionen für Evolutionsstrategie und Prüft die eingestellten Optionen
    '***************************************************************************************************
    Private Sub CES_Form_Settings(ByRef Settings As Evo_Settings, ByVal Method As String)

        'Überprüfung der Übergebenen Werte
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        If (Settings.CES.n_Parents < 1) Then
            Throw New Exception("Die Anzahl der Eltern ist kleiner 1!")
        End If
        If (Settings.CES.n_Childs < 1) Then
            Throw New Exception("Die Anzahl der Nachfahren ist kleiner 1!")
        End If
        If (Settings.CES.n_Generations < 1) Then
            Throw New Exception("Die Anzahl der Generationen ist kleiner 1!")
        End If
        If (Settings.CES.n_Childs <= Settings.CES.n_Parents And Method <> "HYBRID") Then
            Throw New Exception("Die Anzahl der Eltern muss kleiner als die Anzahl der Nachfahren!" & Chr(13) & Chr(10) & "'Rechenberg 73' schlägt ein Verhältnis von 1:3 bis 1:5 vor.")
        End If
        If (Settings.CES.OptStrategie <> EVO_STRATEGIE.Komma_Strategie And Settings.CES.OptStrategie <> EVO_STRATEGIE.Plus_Strategie) Then
            Throw New Exception("Typ der Evolutionsstrategie ist nicht '+' oder ','")
        End If
        If (Settings.CES.OptReprodOp <> CES_REPRODOP.Selt_Rand_Uniform And CES_REPRODOP.Order_Crossover AND CES_REPRODOP.Part_Mapped_Cross)
            Throw New Exception("Typ der Reproduction ist nicht richtig!")
        End If        
        If (Settings.CES.OptMutOperator <> CES_MUTATION.RND_Switch And CES_MUTATION.Dyn_Switch)
            Throw New Exception("Typ der Mutation ist nicht richtig!")
        End If
        If (Settings.CES.n_MemberSecondPop < 1)
            Throw New Exception("Die Zahl der Mitglieder der sekundären Population ist kleiner 1!")
        End If
        If (Settings.CES.n_PartsMem < 1 or Settings.CES.n_PartsMem > 3)
            Throw New Exception("Die Zahl der Memory Parts muss zwischen 1und 3 liegen!")
        End If
        If (Settings.CES.n_Interact < 1) Then
            Throw New Exception("Die Anzahl der Mitglieder des sekundären Population muss mindestens 1 sein!")
        End If
        If (Settings.CES.pr_MutRate < 0 or Settings.CES.pr_MutRate > 100) Then
            Throw New Exception("Der Prozentsatz der Mutationrate muss zwischen 1 und 100 liegen!")
        End If
        If (Settings.CES.n_PES_MaxParents < 1) Then
            Throw New Exception("Die Anzahl der Eltern für PES muss mindestens 1 sein!")
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
    private Sub CES_Modell_Settings(ByVal AnzPenalty As Integer, ByVal AnzConstr As Integer, byval AnzLocations as Integer, byval AnzVerzweig as Integer, byval AnzCombinations as Integer, byval TypeTestModus as Integer, byval AnzPathDimension() as integer)

        'Prüft ob die Zahl mög. Kombinationen < Zahl Eltern + Nachfolger
        If (Settings.CES.n_Childs + Settings.CES.n_Parents) > AnzCombinations Then
            Throw New Exception("Die Zahl der Eltern + die Zahl der Kinder ist größer als die mögliche Zahl der Kombinationen.")
        End If
        
        'Übergabe
        ModSett.n_Penalty = AnzPenalty
        modsett.n_Constrain = AnzConstr
        modsett.n_Locations = AnzLocations
        modsett.n_Verzweig = AnzVerzweig
        modsett.n_Combinations = AnzCombinations
        modsett.TestModus = TypeTestModus
        modsett.n_PathDimension = AnzPathDimension.Clone

    End Sub

    'Schritt 3: ReDim
    'Einige ReDims die erst mit den FormSetting oder ModelSetting möglich sind
    '*************************************************************************
    Private Sub CES_ReDim()

        Redim NDSorting(Settings.CES.n_Childs + Settings.CES.n_Parents - 1)
        'Checken ob es verwendet wird
        Redim NDSResult(Settings.CES.n_Childs + Settings.CES.n_Parents - 1)
        
    End Sub


    'Normaler Modus: Generiert zufällige Paths für alle Kinder BM Problem
    '*********************************************************************
    Public Sub Generate_Random_Path()
        Dim i, j As Integer
        Dim tmp As Integer
        Dim lowerb As Integer = 0
        Dim upperb As Integer
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
            Loop While Is_Twin(i) = True Or isnot_nullvariante(Childs(i).Path) = False
        Next

    End Sub

    'Testmodus 2: Funktion zum testen aller Kombinationen
    '***************************************************
    Public Sub Generate_All_Test_Paths()
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

    End Sub
    'Hier kann man Pfade wie z.B. Nullvarianten die nicht erlaubt sind hard vercoden (ToDo!)
    '***************************************************************************************
    Public Function isnot_nullvariante(ByVal path() As Integer) As Boolean
        isnot_nullvariante = True

        Dim i, j As Integer
        Dim count As Integer = 0

        'Dim firstValues()() As Byte = {New Byte() {2, 1}, New Byte() {3, 0}}
        'Dim vector_array()() As Integer = {New Integer() {1, 1, 1}}
        Dim vector_array()() As Integer = {New Integer() {0, 1, 1}}

        For i = 0 To vector_array.GetUpperBound(0)
            If vector_array(i).GetUpperBound(0) = path.GetUpperBound(0) Then
                For j = 0 To vector_array(i).GetUpperBound(0)
                    If vector_array(i)(j) = path(j) Then
                        count += 1
                    End If
                Next
                If count = path.GetLength(0) Then
                    isnot_nullvariante = False
                End If
            End If
        Next
    End Function

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
                Parents(i) = Childs(i).Copy
            Next i

            'Strategie PLUS
            'xxxxxxxxxxxxxx
        ElseIf Settings.CES.OptStrategie = "plus" Then 'CHECK: sollte das nicht = EVO_STRATEGIE.Plus_Strategie sein?

            For i = 0 To Settings.CES.n_Childs - 1
                'Des schlechteste Elter wird bestimmt
                Dim bad_no As Integer = 0
                Dim bad_penalty As Double = Parents(0).Penalty(0)
                For j = 1 To Settings.CES.n_Parents - 1
                    If bad_penalty < Parents(j).Penalty(0) Then
                        bad_no = j
                        bad_penalty = Parents(j).Penalty(0)
                    End If
                Next

                'Falls der schlechteste Parent schlechter als der Child ist wird er durch den Child ersetzt
                If Parents(bad_no).Penalty(0) > Childs(i).Penalty(0) Then
                    Parents(bad_no) = Childs(i).Copy
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
            Loop While Is_Twin(i) = True Or Is_Clone(i) = True Or isnot_nullvariante(Childs(i).Path) = False
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
            ReDim Memory(0)
        Else
            ReDim Preserve Memory(Memory.GetLength(0))
            neu = Memory.GetUpperBound(0)
        End If

        Memory(neu) = New Individuum("Memory", neu)

        Memory(neu) = Childs(Child_No).Copy
        Memory(neu).Generation = Gen_No

    End Sub

    'Durchsucht den Memory - Der PES_Parantsatz für jedes Child wird hier ermittelt
    'Eine Liste (PES_Parents_pChild) mit Eltern für ein Child incl. der Location Information wird erstellt
    '**********************************************************************************************
    Sub Memory_Search_per_Child(ByVal Child As Individuum)

        Dim j, m As Integer
        Dim count_a(ModSett.n_Locations - 1) As Integer
        Dim count_b(ModSett.n_Locations - 1) As Integer
        Dim count_c(ModSett.n_Locations - 1) As Integer

        ReDim PES_Parents_pChild(0)
        PES_Parents_pChild(0) = New Individuum("PES_Parent", 0)

        Dim akt As Integer = 0

        For j = 0 To ModSett.n_Locations - 1
            count_a(j) = 0
            count_b(j) = 0
            count_c(j) = 0
        Next

        For j = 0 To ModSett.n_Locations - 1
            For m = 0 To Memory.GetUpperBound(0)

                'Rank Nummer 1 (Lediglich Übereinstimmung in der Location selbst)
                If Child.Path(j) = Memory(m).Path(j) Then
                    ReDim Preserve PES_Parents_pChild(PES_Parents_pChild.GetLength(0))
                    PES_Parents_pChild(PES_Parents_pChild.GetUpperBound(0)) = New Individuum("PES_Parent", PES_Parents_pChild.GetUpperBound(0))
                    akt = PES_Parents_pChild.GetUpperBound(0)
                    PES_Parents_pChild(akt) = Memory(m).Copy
                    PES_Parents_pChild(akt).iLocation = j + 1
                    PES_Parents_pChild(akt).Memory_Rank = 1
                    count_a(j) += 1
                End If

                'Rank Nummer 2 (Übereinstimmung in Location 1 und 2)
                If Not j = ModSett.n_Locations - 1 And Settings.CES.n_PartsMem > 1 Then
                    If Child.Path(j) = Memory(m).Path(j) And Child.Path(j + 1) = Memory(m).Path(j + 1) Then
                        ReDim Preserve PES_Parents_pChild(PES_Parents_pChild.GetLength(0))
                        PES_Parents_pChild(PES_Parents_pChild.GetUpperBound(0)) = New Individuum("PES_Parent", PES_Parents_pChild.GetUpperBound(0))
                        akt = PES_Parents_pChild.GetUpperBound(0)
                        PES_Parents_pChild(akt) = Memory(m).Copy
                        PES_Parents_pChild(akt).iLocation = j + 1
                        PES_Parents_pChild(akt).Memory_Rank = 2
                        count_b(j) += 1
                    End If
                End If

                'Rank Nummer 3 (Übereinstimmung in Location 1, 2 und 3)
                If Not (j = ModSett.n_Locations - 1 Or j = ModSett.n_Locations - 2) And Settings.CES.n_PartsMem > 2 Then
                    If Child.Path(j) = Memory(m).Path(j) And Child.Path(j + 1) = Memory(m).Path(j + 1) And Child.Path(j + 2) = Memory(m).Path(j + 2) Then
                        ReDim Preserve PES_Parents_pChild(PES_Parents_pChild.GetLength(0))
                        PES_Parents_pChild(PES_Parents_pChild.GetUpperBound(0)) = New Individuum("PES_Parent", PES_Parents_pChild.GetUpperBound(0))
                        akt = PES_Parents_pChild.GetUpperBound(0)
                        PES_Parents_pChild(akt) = Memory(m).Copy
                        PES_Parents_pChild(akt).iLocation = j + 1
                        PES_Parents_pChild(akt).Memory_Rank = 3
                        count_c(j) += 1
                    End If
                End If
            Next
        Next

        'Die Doppelten niedrigeren Ränge werden gelöscht - und der erste leere Datensatz
        Call PES_Memory_Dubletten_loeschen(PES_Parents_pChild)

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
                PES_Parents_pLoc(x) = PES_Parents_pChild(i).Copy
                x += 1
            End If
        Next
    End Sub

    Sub Memory_NDSorting()

        Dim i As Short

        Dim NDSorting(PES_Parents_pLoc.GetUpperBound(0)) As Individuum
        Call Individuum.New_Array("NDSorting", NDSorting)

        '1. ALLE werden reinkopiert (anders als beim normalen Verfahren)
        '---------------------------------------------------------------
        For i = 0 To PES_Parents_pLoc.GetUpperBound(0)
            NDSorting(i) = PES_Parents_pLoc(i).Copy
            NDSorting(i).dominated = False
            NDSorting(i).Front = 0
            NDSorting(i).Distance = 0
        Next i

        'Die Anzahlen werden hier speziell errechnet
        Dim n_PES_Childs As Integer
        n_PES_Childs = PES_Parents_pLoc.GetLength(0) - Settings.CES.n_PES_MaxParents

        'Die Eltern werden zurückgesetzt
        ReDim PES_Parents_pLoc(Settings.CES.n_PES_MaxParents - 1)

        '********************* Alles in der Klasse Functions ****************************************
        '2. Die einzelnen Fronten werden bestimmt
        '3. Der Bestwertspeicher wird entsprechend der Fronten oder der sekundären Population gefüllt
        '4: Sekundäre Population wird bestimmt und gespeichert
        '--------------------------------
        Dim Func1 As New Kern.Functions(n_PES_Childs, Settings.CES.n_PES_MaxParents, Settings.CES.n_PES_MemSecPop, Settings.CES.n_PES_Interact, Settings.CES.is_PES_SecPop, ModSett.n_Penalty, ModSett.n_Constrain, 1)
        Call Func1.EsEltern_Pareto_SekundärQb(PES_Parents_pLoc, NDSorting, PES_SekundärQb)
        '********************************************************************************************

        'SekundärQB Mach tin dieser Form noch keinen Sinn !!!!!!!!!!!!!!!!!!!

    End Sub

    'Löscht wenn ein Individuum bei der gleichen Lokation einmal als Rank 1 und einmal als Rank 2 definiert.
    'Bei Rank 2 entsprechnd Rank 3. Außerdem wird der erste leere Datensatz geloescht.
    '*******************************************************************************************************
    Private Sub PES_Memory_Dubletten_loeschen(ByRef PES_Parents_pChild() As Individuum)

        Dim tmp(PES_Parents_pChild.GetUpperBound(0) - 1) As Individuum
        Individuum.New_Array("tmp", tmp)
        Dim isDouble As Boolean
        Dim i, j, x As Integer

        x = 0
        For i = 1 To PES_Parents_pChild.GetUpperBound(0)
            isDouble = False
            For j = 1 To PES_Parents_pChild.GetUpperBound(0)
                If i <> j And PES_Parents_pChild(i).iLocation And is_PES_Double(PES_Parents_pChild(i), PES_Parents_pChild(j)) Then
                    isDouble = True
                End If
            Next
            If isDouble = False Then
                tmp(x) = PES_Parents_pChild(i).Copy
                x += 1
            End If
        Next

        ReDim Preserve tmp(x - 1)
        ReDim Preserve PES_Parents_pChild(x - 1)

        For i = 0 To tmp.GetUpperBound(0)
            PES_Parents_pChild(i) = tmp(i).Copy
        Next

    End Sub

    'Prüft ob die beiden den gleichen Pfad und zur gleichen location gehören
    '***********************************************************************
    Private Function is_PES_Double(ByVal First As Individuum, ByVal Second As Individuum) As Boolean
        is_PES_Double = False

        Dim count As Integer = 0
        Dim i As Integer

        For i = 0 To First.Path.GetUpperBound(0)
            If First.Path(i) = Second.Path(i) Then
                count += 1
            End If
        Next

        If count = First.Path.GetLength(0) Then
            If First.iLocation = Second.iLocation And First.Memory_Rank < Second.Memory_Rank Then
                is_PES_Double = True
            End If
        End If

    End Function


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
        Dim swap As New EVO.Kern.Individuum("swap", 0)

        For i = 0 To IndividuumList.GetUpperBound(0)
            For j = 0 To IndividuumList.GetUpperBound(0)
                If IndividuumList(i).Penalty(0) < IndividuumList(j).Penalty(0) Then
                    swap = IndividuumList(i)
                    IndividuumList(i) = IndividuumList(j)
                    IndividuumList(j) = swap
                End If
            Next j
        Next i

    End Sub

    'Hilfsfunktion checkt ob die neuen Childs Zwillinge sind
    '*******************************************************
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

    'Hilfsfunktion checkt ob die neuen Childs Klone sind
    '***************************************************
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
    Public Sub NDSorting_Control(ByVal iAktGen As Integer)
        Dim i As Short

        Dim NDSorting(Settings.CES.n_Childs + Settings.CES.n_Parents - 1) As Individuum
        Call Individuum.New_Array("NDSorting", NDSorting)

        '0. Eltern und Nachfolger werden gemeinsam betrachtet
        'Die Kinder werden NDSorting hinzugefügt
        '-------------------------------------------

        For i = 0 To Settings.CES.n_Childs - 1
            NDSorting(i) = Childs(i).Copy
            NDSorting(i).dominated = False
            NDSorting(i).Front = 0
            NDSorting(i).Distance = 0
        Next i

        '1. Eltern und Nachfolger werden gemeinsam betrachtet
        'Nur Eltern werden NDSorting hinzugefügt, Kinder sind schon oben drin
        '--------------------------------------------------------------------

        For i = Settings.CES.n_Childs To Settings.CES.n_Childs + Settings.CES.n_Parents - 1
            NDSorting(i) = Parents(i - Settings.CES.n_Childs).Copy
            NDSorting(i).dominated = False
            NDSorting(i).Front = 0
            NDSorting(i).Distance = 0
        Next i

        '********************* Alles in der Klasse Functions ****************************************
        '2. Die einzelnen Fronten werden bestimmt
        '3. Der Bestwertspeicher wird entsprechend der Fronten oder der sekundären Population gefüllt
        '4: Sekundäre Population wird bestimmt und gespeichert
        '--------------------------------
        Dim Func1 As New Kern.Functions(Settings.CES.n_Childs, Settings.CES.n_Parents, Settings.CES.n_MemberSecondPop, Settings.CES.n_Interact, Settings.CES.is_SecPop, ModSett.n_Penalty, ModSett.n_Constrain, iAktGen)
        Call Func1.EsEltern_Pareto_SekundärQb(Parents, NDSorting, SekundärQb)
        '********************************************************************************************

        'Schritt 5 und 6 sind für CES noch nicht implementiert

        ''5: Neue Eltern werden gleich dem Bestwertspeicher gesetzt
        ''---------------------------------------------------------
        'For i = 0 To PES_Settings.NEltern - 1
        '    For v = 0 To Anz.Para - 1
        '        De(v, i, PES_iAkt.iAktPop) = Best.Db(v, i, PES_iAkt.iAktPop)
        '        Xe(v, i, PES_iAkt.iAktPop) = Best.Xb(v, i, PES_iAkt.iAktPop)
        '    Next v
        'Next i

        ''6: Sortierung der Lösungen ist nur für Neighbourhood-Rekombination notwendig
        ''----------------------------------------------------------------------------
        'If (PES_Settings.iOptEltern = EVO_ELTERN.Neighbourhood) Then
        '    Call Neighbourhood_AbstandsArray()
        '    Call Neighbourhood_Crowding_Distance()
        'End If

    End Sub

    'ES_GET_SEKUNDÄRE_POPULATIONEN - Sekundäre Population speichert immer die angegebene
    'Anzahl von Bestwerten und kann den Bestwertspeicher alle x Generationen überschreiben
    '*************************************************************************************
    Public Function SekundärQb_Get() As Double(,)

        Dim j, i As Integer
        Dim SekPopulation(,) As Double

        ReDim SekPopulation(SekundärQb.GetUpperBound(0), SekundärQb(0).Penalty.GetUpperBound(0))

        For i = 0 To SekundärQb.GetUpperBound(0)
            For j = 0 To SekundärQb(0).Penalty.GetUpperBound(0)
                SekPopulation(i, j) = SekundärQb(i).Penalty(j)
            Next j
        Next i

        Return SekPopulation

    End Function

#End Region 'Methoden

End Class



