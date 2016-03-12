' Copyright (c) BlueM Dev Group
' Website: http://bluemodel.org
' 
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
    '**** Letzte Änderung: Dezember 2009                                        ****
    '*******************************************************************************
    '*******************************************************************************

    '******* Konvention *******
    'Cities:      1 2 3 4 5 6 7
    'Pathindex:   0 1 2 3 4 5 6
    'CutPoint:  -1 0 1 2 3 4 5 6
    'LB + UB(n-2)  x         x
    '**************************

#Region "Eigenschaften"
    '##################

    'Das Problem
    Private mProblem As EVO.Common.Problem

    'Die Settings für alles aus dem Form
    Public mSettings As Settings             'TODO: sollte private sein!

    'Modell Setting
    Public Structure ModSettings
        Public n_Locations As Integer           'Anzahl der Locations
        Public n_Verzweig As Integer            'Anzahl der Verzweigungen in der Verzweigungsdatei
        Public n_PathDimension() As Integer     'Anzahl der Maßnahmen an jedem Ort
    End Structure

    Public ModSett As ModSettings

    'Listen für die Individuen
    '*************************
    Public Children() As Individuum_CES
    Public Parents() As Individuum_CES
    Public Property IParents() As Individuum()
        Get
            Dim inds() As Individuum
            ReDim inds(Me.Parents.GetUpperBound(0))
            Call Array.Copy(Me.Parents, inds, Me.Parents.Length)
            Return inds
        End Get
        Set(ByVal value As Individuum())
            ReDim Me.Parents(value.GetUpperBound(0))
            Call Array.Copy(value, Me.Parents, value.Length)
        End Set
    End Property
    Public SekundärQb(-1) As Individuum
    Public NDSorting() As Individuum_CES
    'Checken ob es verwendet wird
    Public NDSResult() As Individuum_CES

    'Für Hybrid
    Public PES_Memory(-1) As Individuum_CES
    Public Property IPES_Memory() As Individuum()
        Get
            Dim inds() As Individuum
            ReDim inds(Me.PES_Memory.GetUpperBound(0))
            Call Array.Copy(Me.PES_Memory, inds, Me.PES_Memory.Length)
            Return inds
        End Get
        Set(ByVal value As Individuum())
            ReDim Me.PES_Memory(value.GetUpperBound(0))
            Call Array.Copy(value, Me.PES_Memory, value.Length)
        End Set
    End Property
    Public PES_Parents_pChild() As Individuum_CES
    Public PES_Parents_pLoc() As Individuum_CES
    Public Property IPES_Parents_pLoc() As Individuum()
        Get
            Dim inds() As Individuum
            ReDim inds(Me.PES_Parents_pLoc.GetUpperBound(0))
            Call Array.Copy(Me.PES_Parents_pLoc, inds, Me.PES_Parents_pLoc.Length)
            Return inds
        End Get
        Set(ByVal value As Individuum())
            ReDim Me.PES_Parents_pLoc(value.GetUpperBound(0))
            Call Array.Copy(value, Me.PES_Parents_pLoc, value.Length)
        End Set
    End Property
    Private PES_Mem_SekundärQb(-1) As Individuum

#End Region 'Eigenschaften

#Region "Methoden"
    '#############

    'Initialisierung der PES
    '***********************
    Public Sub CESInitialise(ByRef settings As Settings, ByRef prob As EVO.Common.Problem, ByVal AnzVerzweig As Integer)

        'Problem speichern
        Me.mProblem = prob

        'Schritt 1: CES - FORM SETTINGS
        'Optionen der Evolutionsstrategie werden übergeben
        Call CES_Form_Settings(settings)

        'Schritt 2: CES - MODELL SETTINGS
        'Optionen der Evolutionsstrategie werden übergeben
        Call CES_Modell_Settings(AnzVerzweig)

        'Schritt 3: CES - ReDim
        'Einige ReDims die erst mit den FormSetting oder ModelSetting möglich sind
        Call CES_ReDim()

    End Sub

    'Schritt 1: FORM SETTINGS
    'Function Form SETTINGS übergibt Optionen für Evolutionsstrategie und Prüft die eingestellten Optionen
    '***************************************************************************************************
    Private Sub CES_Form_Settings(ByRef Settings As Settings)

        'Überprüfung der Übergebenen Werte
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        If (Me.mProblem.CES_T_Modus = CES_T_MODUS.No_Test) Then

            If (Settings.CES.N_Parents < 3) Then
                Throw New Exception("Die Anzahl muss mindestens 3 sein!")
            End If
            If (Settings.CES.N_Children < 3) Then
                Throw New Exception("Die Anzahl der Nachfahren muss mindestens 3 sein!")
            End If
            If (Settings.CES.N_Children <= Settings.CES.N_Parents And Me.mProblem.Method <> "HYBRID") Then
                Throw New Exception("Die Anzahl der Eltern muss kleiner als die Anzahl der Nachfahren!" & Chr(13) & Chr(10) & "'Rechenberg 73' schlägt ein Verhältnis von 1:3 bis 1:5 vor.")
            End If

        End If

        If (Settings.CES.N_Generations < 1) Then
            Throw New Exception("Die Anzahl der Generationen ist kleiner 1!")
        End If
        If (Settings.CES.OptStrategie <> EVO_STRATEGIE.Komma_Strategie And Settings.CES.OptStrategie <> EVO_STRATEGIE.Plus_Strategie) Then
            Throw New Exception("Typ der Evolutionsstrategie ist nicht '+' oder ','")
        End If
        'If (Settings.CES.OptReprodOp <> CES_REPRODOP.Uniform_Crossover And CES_REPRODOP.Order_Crossover And CES_REPRODOP.Part_Mapped_Cross) Then
        '    Throw New Exception("Typ der Reproduction ist nicht richtig!")
        'End If
        'If (Settings.CES.OptMutOperator <> CES_MUTATION.RND_Switch And CES_MUTATION.Dyn_Switch) Then
        '    Throw New Exception("Typ der Mutation ist nicht richtig!")
        'End If
        If (Settings.CES.N_MemberSecondPop < 1) Then
            Throw New Exception("Die Zahl der Mitglieder der sekundären Population ist kleiner 1!")
        End If
        If (Settings.CES.N_Interact < 1) Then
            Throw New Exception("Die Anzahl der Mitglieder des sekundären Population muss mindestens 1 sein!")
        End If
        If (Settings.CES.Pr_MutRate < 0 Or Settings.CES.Pr_MutRate > 100) Then
            Throw New Exception("Der Prozentsatz der Mutationrate muss zwischen 1 und 100 liegen!")
        End If
        If (Settings.CES.N_PES_MemSecPop < 1) Then
            Throw New Exception("Die Anzahl der Memeber für PES sekundäre Population muss mindestens 1 sein!")
        End If
        If (Settings.CES.N_PES_Interact < 1) Then
            Throw New Exception("Der Austausch mit der sekundären Population von PES muss mindestens 1 sein!")
        End If

        'Übergabe der Optionen
        'xxxxxxxxxxxxxxxxxxxxx

        Me.mSettings = Settings

    End Sub

    'Schritt 3: PREPARE
    'A: Prüfung der ModellSetting in Kombination mit den Form Setting
    'B: Übergabe der ModellSettings
    '****************************************************************
    Private Sub CES_Modell_Settings(ByVal AnzVerzweig As Integer)

        If (Me.mProblem.CES_T_Modus = CES_T_MODUS.No_Test) Then

            'Prüft ob die Zahl mög. Kombinationen < Zahl Eltern + Nachfolger
            If ((mSettings.CES.N_Children + mSettings.CES.N_Parents) > Me.mProblem.NumCombinations And Not Me.mProblem.Method = "HYBRID") Then
                Throw New Exception("Die Zahl der Eltern + die Zahl der Kinder ist größer als die mögliche Zahl der Kombinationen.")
            End If

        End If

        'Übergabe
        ModSett.n_Locations = Me.mProblem.NumLocations
        ModSett.n_Verzweig = AnzVerzweig
        ModSett.n_PathDimension = Me.mProblem.n_PathDimension

    End Sub

    'Schritt 3: ReDim
    'Einige ReDims die erst mit den FormSetting oder ModelSetting möglich sind
    '*************************************************************************
    Private Sub CES_ReDim()

        'Die Variablen für die Individuuen werden gesetzt
        '************************************************
        'TODO: Wenn sich am Problem nix geändert hat ist dieser Aufruf überflüssig!
        Call Individuum.Initialise(Me.mProblem)

        'Parents werden dimensioniert
        Parents = Individuum.New_Indi_Array(Individuum.Individuumsklassen.Individuum_CES, mSettings.CES.N_Parents, "Parent")

        'Children werden dimensioniert
        Children = Individuum.New_Indi_Array(Individuum.Individuumsklassen.Individuum_CES, mSettings.CES.N_Children, "Child")

        'NDSorting wird dimensioniert
        ReDim NDSorting(mSettings.CES.N_Children + mSettings.CES.N_Parents - 1)

        'NDSResult - Checken ob es verwendet wird
        ReDim NDSResult(mSettings.CES.N_Children + mSettings.CES.N_Parents - 1)

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

        For i = 0 To mSettings.CES.N_Children - 1
            Do
                For j = 0 To ModSett.n_Locations - 1
                    upperb = ModSett.n_PathDimension(j) - 1
                    'Randomize() nicht vergessen
                    tmp = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
                    Children(i).Path(j) = tmp
                Next
                Children(i).mutated = True
                Children(i).ID = i + 1
                LoopCount += 1
                'If LoopCount = 100000 then
                '    throw new Exception("100000")
                'End If
            Loop While (Is_Twin(i) = True Or is_nullvariante(Children(i).Path) = True) And Not LoopCount >= 1000
        Next

    End Sub

    'Testmodus 2: Funktion zum testen aller Kombinationen
    '****************************************************
    Public Sub Generate_Paths_for_Tests(ByVal Path() As Integer, ByVal Modus As CES_T_MODUS)

        Select Case Modus

            Case CES_T_MODUS.One_Combi
                'Testmodus 1: Funktion zum testen einer Kombination
                '**************************************************
                Children(0).Path = Path.Clone

            Case CES_T_MODUS.All_Combis
                'Testmodus 2: Funktion zum testen aller Kombinationen
                '****************************************************
                Dim i, j As Integer

                Dim array() As Integer
                ReDim array(Children(i).Path.GetUpperBound(0))
                For i = 0 To array.GetUpperBound(0)
                    array(i) = 0
                Next

                For i = 0 To mSettings.CES.N_Children - 1
                    For j = 0 To Children(i).Path.GetUpperBound(0)
                        Children(i).Path(j) = array(j)
                    Next
                    array(0) += 1
                    If Not i = mSettings.CES.N_Children - 1 Then
                        For j = 0 To Children(i).Path.GetUpperBound(0)
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
        For i = 0 To mSettings.CES.N_Children - 1
            'und pro Location
            For j = 0 To ModSett.n_Locations - 1
                'Die Parameter (falls vorhanden) werden überschrieben
                If Not Children(i).Loc(j).PES_OptPara.GetLength(0) = 0 Then
                    'Dem Child wird der Schrittweitenvektor zugewiesen und gegebenenfalls der Parameter zufällig gewählt
                    '***************************************************************************************************
                    For m = 0 To Children(i).Loc(j).PES_OptPara.GetUpperBound(0)
                        Children(i).Loc(j).PES_OptPara(m).Dn = mSettings.PES.SetMutation.DnStart
                        If mSettings.PES.Startparameter = EVO_STARTPARAMETER.Zufall Then
                            Randomize()
                            Children(i).Loc(j).PES_OptPara(m).Xn = Rnd()
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
        If mSettings.CES.OptStrategie = EVO_STRATEGIE.Komma_Strategie Then
            For i = 0 To mSettings.CES.N_Parents - 1
                Parents(i) = Children(i).Clone()
            Next i

            'Strategie PLUS
            'xxxxxxxxxxxxxx
        ElseIf mSettings.CES.OptStrategie = EVO_STRATEGIE.Plus_Strategie Then

            For i = 0 To mSettings.CES.N_Children - 1
                'Des schlechteste Elter wird bestimmt
                Dim bad_no As Integer = 0
                Dim bad_penalty As Double = Parents(0).PrimObjectives(0)
                For j = 1 To mSettings.CES.N_Parents - 1
                    If bad_penalty < Parents(j).PrimObjectives(0) Then
                        bad_no = j
                        bad_penalty = Parents(j).PrimObjectives(0)
                    End If
                Next

                'Falls der schlechteste Parent schlechter als der Child ist wird er durch den Child ersetzt
                If Parents(bad_no).PrimObjectives(0) > Children(i).PrimObjectives(0) Then
                    Parents(bad_no) = Children(i).Clone()
                End If
            Next

        End If

    End Sub

    'Reproductionsfunktionen
    'XXXXXXXXXXXXXXXXXXXXXXX

    'Steuerung der Reproduktionsoperatoren
    '************************************************************
    Public Sub Reproduction_Control()
        Dim i As Integer
        Dim x, y As Integer
        Dim Einzelkind_Path(ModSett.n_Locations - 1) As Integer
        Dim Einzelkind_Dn_CES As Double
        'UPGRADE: Eltern werden nicht zufällig gewählt sondern immer in Top Down Reihenfolge

        'Beim ersten Durchgang wird nur der Elter verwendet
        Dim DoubleFirst As Boolean = True

        x = 0
        y = 1
        If DoubleFirst then y = 0

        For i = 0 To mSettings.CES.N_Children - 2 Step 2
            Select Case mSettings.CES.OptReprodOp
                Case CES_REPRODOP.One_Point_Crossover
                    mSettings.CES.K_Value = 1
                    Call ReprodOp_k_Point_Crossover(Parents(x).Path, Parents(y).Path, Children(i).Path, Children(i + 1).Path)
                Case CES_REPRODOP.Two_Point_Crossover
                    mSettings.CES.K_Value = 2
                    Call ReprodOp_k_Point_Crossover(Parents(x).Path, Parents(y).Path, Children(i).Path, Children(i + 1).Path)
                Case CES_REPRODOP.k_Point_Crossover
                    Call ReprodOp_k_Point_Crossover(Parents(x).Path, Parents(y).Path, Children(i).Path, Children(i + 1).Path)
                Case CES_REPRODOP.Uniform_Crossover
                    Call ReprodOp_Uniform_Crossover(Parents(x).Path, Parents(y).Path, Children(i).Path, Children(i + 1).Path)
                    'Case CES_REPRODOP.Order_Crossover
                    '    Call ReprodOp_Order_Crossover(Parents(x).Path, Parents(y).Path, Children(i).Path, Children(i + 1).Path)
                    'Case CES_REPRODOP.Part_Mapped_Cross
                    '    Call ReprodOp_Part_Mapped_Crossover(Parents(x).Path, Parents(y).Path, Children(i).Path, Children(i + 1).Path)
            End Select
            Call ReprodOp_Dn_Mitteln(Parents(x).CES_Dn, Parents(y).CES_Dn, Children(i).CES_Dn, Children(i + 1).CES_Dn)
            If y = x and DoubleFirst then y =+ 1
            x += 1
            y += 1
            If x = mSettings.CES.N_Parents - 1 Then x = 0
            If y = mSettings.CES.N_Parents - 1 Then y = 0
        Next i

        If Even_Number(mSettings.CES.N_Children) = False Then
            Select Case mSettings.CES.OptReprodOp
                Case CES_REPRODOP.One_Point_Crossover
                    mSettings.CES.K_Value = 1
                    Call ReprodOp_k_Point_Crossover(Parents(x).Path, Parents(y).Path, Children(mSettings.CES.N_Children - 1).Path, Einzelkind_Path)
                Case CES_REPRODOP.Two_Point_Crossover
                    mSettings.CES.K_Value = 2
                    Call ReprodOp_k_Point_Crossover(Parents(x).Path, Parents(y).Path, Children(mSettings.CES.N_Children - 1).Path, Einzelkind_Path)
                Case CES_REPRODOP.k_Point_Crossover
                    Call ReprodOp_k_Point_Crossover(Parents(x).Path, Parents(y).Path, Children(mSettings.CES.N_Children - 1).Path, Einzelkind_Path)
                Case CES_REPRODOP.Uniform_Crossover
                    Call ReprodOp_Uniform_Crossover(Parents(x).Path, Parents(y).Path, Children(mSettings.CES.N_Children - 1).Path, Einzelkind_Path)
                    'Case CES_REPRODOP.Order_Crossover
                    '    Call ReprodOp_Order_Crossover(Parents(x).Path, Parents(y).Path, Children(mSettings.CES.n_Children - 1).Path, Einzelkind_Path)
                    'Case CES_REPRODOP.Part_Mapped_Cross
                    '    Call ReprodOp_Part_Mapped_Crossover(Parents(x).Path, Parents(y).Path, Children(mSettings.CES.n_Children - 1).Path, Einzelkind_Path)
            End Select
            Call ReprodOp_Dn_Mitteln(Parents(x).CES_Dn, Parents(y).CES_Dn, Children(mSettings.CES.N_Children - 1).CES_Dn, Einzelkind_Dn_CES)
        End If

    End Sub

    'Reproductionsoperator: "Uniform_Crossover"
    'Entscheidet zufällig ob der Wert aus dem Path des Elter_A oder Elter_B für das Allel verwendet wird
    '***************************************************************************************************
    Private Sub ReprodOp_Uniform_Crossover(ByVal ParPath_A() As Integer, ByVal ParPath_B() As Integer, ByRef ChildPath_A() As Integer, ByRef ChildPath_B() As Integer)

        Dim i As Integer

        For i = 0 To ChildPath_A.GetUpperBound(0)
            If BernoulliVer() = True Then
                ChildPath_A(i) = ParPath_B(i)
            Else
                ChildPath_A(i) = ParPath_A(i)
            End If
        Next

        For i = 0 To ChildPath_B.GetUpperBound(0)
            If BernoulliVer() = True Then
                ChildPath_B(i) = ParPath_A(i)
            Else
                ChildPath_B(i) = ParPath_B(i)
            End If
        Next

    End Sub

    'Reproductionsoperator: "k_Point_Crossover"
    'Entscheidet zufällig ob der Wert aus dem Path des Elter_A oder Elter_B für das Allel verwendet wird
    '***************************************************************************************************
    Private Sub ReprodOp_k_Point_Crossover(ByVal ParPath_A() As Integer, ByVal ParPath_B() As Integer, ByRef ChildPath_A() As Integer, ByRef ChildPath_B() As Integer)

        Dim i As Integer
        Dim x As Integer = 0

        Dim FromSame As Boolean = True

        Dim CutPoint(mSettings.CES.K_Value - 1) As Integer
        Call Create_n_Cutpoints(CutPoint)

        For i = 0 To ChildPath_A.GetUpperBound(0)
            If FromSame Then
                ChildPath_A(i) = ParPath_A(i)
                ChildPath_B(i) = ParPath_B(i)
            Else
                ChildPath_A(i) = ParPath_B(i)
                ChildPath_B(i) = ParPath_A(i)
            End If
            If i = CutPoint(x) Then
                If FromSame Then
                    FromSame = False
                Else
                    FromSame = True
                End If
                If x < CutPoint.GetUpperBound(0) Then
                    x += 1
                End If
            End If
        Next
    End Sub


    'Reproductionsoperator "Order_Crossover (OX)"
    'Kopiert den mittleren Teil des einen Elter und füllt den Rest aus der Reihenfolge des anderen Elter auf
    'UPGRADE: Es wird immer nur der mittlere Teil Kopiert, könnte auch mal ein einderer sein
    Private Sub ReprodOp_Order_Crossover(ByVal ParPath_A() As Integer, ByVal ParPath_B() As Integer, ByRef ChildPath_A() As Integer, ByRef ChildPath_B() As Integer)

        Dim i As Integer
        Dim x, y As Integer

        'Geht nur mit zwei Schnittpunkten
        Dim CutPoint(1) As Integer
        Call Create_n_Cutpoints(CutPoint)

        'Kopieren des mittleren Paths Teil 2
        For i = CutPoint(0) + 1 To CutPoint(1)
            ChildPath_A(i) = ParPath_A(i)
            ChildPath_B(i) = ParPath_B(i)
        Next

        'Auffüllen des Paths Teil 3 des Child A mit dem anderen Elter beginnend bei 0
        x = 0
        For i = CutPoint(1) + 1 To ModSett.n_Locations - 1
            If Is_No_OK(ParPath_B(x), ChildPath_A) Then
                ChildPath_A(i) = ParPath_B(x)
            Else
                i -= 1
            End If
            x += 1
        Next
        'Auffüllen des Paths Teil 3 des Child B mit dem anderen Elter beginnend bei 0
        y = 0
        For i = CutPoint(1) + 1 To ModSett.n_Locations - 1
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

    'Das Dn des CES muss auch ermittelt werden
    'Dn Mitteln wird hier gemacht
    Public Sub ReprodOp_Dn_Mitteln(ByVal ParDn_A As Double, ByVal ParDn_B As Double, ByRef ChildDn_A As Double, ByRef ChildDn_B As Double)
        Dim Dn_neu As Double

        Dn_neu = (ParDn_A + ParDn_B) / 2

        ChildDn_A = Dn_neu
        ChildDn_B = Dn_neu
    End Sub


    'Mutationsfunktionen
    'XXXXXXXXXXXXXXXXXXX

    'Steuerung der Mutationsoperatoren
    '*********************************
    Public Sub Mutation_Control()
        Dim i As Integer

        For i = 0 To mSettings.CES.N_Children - 1
            Dim count As Integer = 0
            Do
                Select Case mSettings.CES.OptMutOperator
                    Case CES_MUTATION.RND_Switch
                        'Verändert zufällig ein gen des Paths
                        Call MutOp_RND_Switch(Children(i).Path)
                    Case CES_MUTATION.Gene_Insertion
                        'Selektiert einen Subpath und generiert diesen neu
                        Call MutOp_Gene_Insertion(Children(i).Path)
                End Select
                count += 1
            Loop While is_nullvariante(Children(i).Path) = True And Not count >= 1000
            Children(i).mutated = True
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
            If Tmp_a <= mSettings.CES.Pr_MutRate Then
                upperb_b = ModSett.n_PathDimension(i) - 1
                'Randomize() nicht vergessen
                Tmp_b = CInt(Int((upperb_b - lowerb_b + 1) * Rnd() + lowerb_b))
                Path(i) = Tmp_b
            End If
        Next

    End Sub

    'Mutationsoperator "Gene_Insertion"
    'Selektiert einen Subpath und setzt diesen neu
    '********************************************
    Private Sub MutOp_Gene_Insertion(ByVal Path() As Integer)

        Dim l, i As Integer
        Dim BegEnde(0) As Integer
        Dim lowerb As Integer = 0
        Dim upperb As Integer
        Randomize()

        Call Create_n_SubPath(BegEnde)
        ReDim Preserve BegEnde(1)

        l = CInt(Int(ModSett.n_Locations * mSettings.CES.Pr_MutRate / 100))

        If BernoulliVer() Then
            BegEnde(1) = BegEnde(0) + l
            If BegEnde(1) > ModSett.n_Locations - 1 Then
                BegEnde(1) = ModSett.n_Locations - 1
            End If
        Else
            BegEnde(1) = BegEnde(0) - l
            If BegEnde(1) < 0 Then
                BegEnde(1) = 0
            End If
        End If

        Array.Sort(BegEnde)

        For i = BegEnde(0) To BegEnde(1)
            upperb = ModSett.n_PathDimension(i) - 1
            Path(i) = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
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

        ReDim Preserve PES_Memory(PES_Memory.GetLength(0))
        neu = PES_Memory.GetUpperBound(0)

        PES_Memory(neu) = New Individuum_CES("Memory", neu)

        PES_Memory(neu) = Children(Child_No).Clone()
        PES_Memory(neu).Generation = Gen_No

    End Sub

    'Durchsucht den Memory - Der PES_Parantsatz für jedes Child wird hier ermittelt
    'Eine Liste (PES_Parents_pChild) mit Eltern für ein Child incl. der Location Information wird erstellt
    '**********************************************************************************************
    Sub Memory_Search_per_Child(ByVal Child As Individuum_CES)

        Dim i_loc, i_mem As Integer
        Dim akt As Integer = 0

        ReDim PES_Parents_pChild(0)
        PES_Parents_pChild(0) = New Individuum_CES("PES_Parent", 0)

        Select Case mSettings.CES.Mem_Strategy

            Case MEMORY_STRATEGY.B_One_Loc_Up, MEMORY_STRATEGY.A_Two_Loc_Up

                For i_loc = 0 To ModSett.n_Locations - 1
                    For i_mem = 0 To PES_Memory.GetUpperBound(0)

                        'Rank Nummer 0 (Lediglich Übereinstimmung in der Location selbst)
                        If Child.Path(i_loc) = PES_Memory(i_mem).Path(i_loc) Then
                            ReDim Preserve PES_Parents_pChild(PES_Parents_pChild.GetLength(0))
                            akt = PES_Parents_pChild.GetUpperBound(0)
                            PES_Parents_pChild(akt) = New Individuum_CES("PES_Parent", PES_Parents_pChild.GetUpperBound(0))
                            PES_Parents_pChild(akt) = PES_Memory(i_mem).Clone()
                            PES_Parents_pChild(akt).iLocation = i_loc + 1
                            PES_Parents_pChild(akt).Memory_Strat = MEMORY_STRATEGY.C_This_Loc
                        End If

                        'Rank Nummer -1 (Übereinstimmung in der Downsteam Location 1 und 2)
                        If Not i_loc = 0 And mSettings.CES.Mem_Strategy < 0 Then
                            If Child.Path(i_loc) = PES_Memory(i_mem).Path(i_loc) And Child.Path(i_loc - 1) = PES_Memory(i_mem).Path(i_loc - 1) Then
                                ReDim Preserve PES_Parents_pChild(PES_Parents_pChild.GetLength(0))
                                akt = PES_Parents_pChild.GetUpperBound(0)
                                PES_Parents_pChild(akt) = New Individuum_CES("PES_Parent", PES_Parents_pChild.GetUpperBound(0))
                                PES_Parents_pChild(akt) = PES_Memory(i_mem).Clone()
                                PES_Parents_pChild(akt).iLocation = i_loc + 1
                                PES_Parents_pChild(akt).Memory_Strat = MEMORY_STRATEGY.B_One_Loc_Up
                            End If
                        End If

                        'Rank Nummer -2 (Übereinstimmung in Downstream Location 1, 2 und 3)
                        If Not (i_loc = 0 Or i_loc = 1) And mSettings.CES.Mem_Strategy < -1 Then
                            If Child.Path(i_loc) = PES_Memory(i_mem).Path(i_loc) And Child.Path(i_loc - 1) = PES_Memory(i_mem).Path(i_loc - 1) And Child.Path(i_loc - 2) = PES_Memory(i_mem).Path(i_loc - 2) Then
                                ReDim Preserve PES_Parents_pChild(PES_Parents_pChild.GetLength(0))
                                akt = PES_Parents_pChild.GetUpperBound(0)
                                PES_Parents_pChild(akt) = New Individuum_CES("PES_Parent", PES_Parents_pChild.GetUpperBound(0))
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
                            PES_Parents_pChild(akt) = New Individuum_CES("PES_Parent", PES_Parents_pChild.GetUpperBound(0))
                            PES_Parents_pChild(akt) = PES_Memory(i_mem).Clone()
                            PES_Parents_pChild(akt).iLocation = i_loc + 1
                            PES_Parents_pChild(akt).Memory_Strat = MEMORY_STRATEGY.C_This_Loc
                        End If

                        'Strategy Nummer 1 (Übereinstimmung in der Downsteam Location 1 und 2)
                        If Not i_loc = ModSett.n_Locations - 1 And mSettings.CES.Mem_Strategy > 0 Then
                            If Child.Path(i_loc) = PES_Memory(i_mem).Path(i_loc) And Child.Path(i_loc + 1) = PES_Memory(i_mem).Path(i_loc + 1) Then
                                ReDim Preserve PES_Parents_pChild(PES_Parents_pChild.GetLength(0))
                                akt = PES_Parents_pChild.GetUpperBound(0)
                                PES_Parents_pChild(akt) = New Individuum_CES("PES_Parent", PES_Parents_pChild.GetUpperBound(0))
                                PES_Parents_pChild(akt) = PES_Memory(i_mem).Clone()
                                PES_Parents_pChild(akt).iLocation = i_loc + 1
                                PES_Parents_pChild(akt).Memory_Strat = MEMORY_STRATEGY.D_One_Loc_Down
                            End If
                        End If

                        'Strategy Nummer 3 (Übereinstimmung in Downstream Location 1, 2 und 3)
                        If Not (i_loc = ModSett.n_Locations - 1 Or i_loc = ModSett.n_Locations - 2) And mSettings.CES.Mem_Strategy > 1 Then
                            If Child.Path(i_loc) = PES_Memory(i_mem).Path(i_loc) And Child.Path(i_loc + 1) = PES_Memory(i_mem).Path(i_loc + 1) And Child.Path(i_loc + 2) = PES_Memory(i_mem).Path(i_loc + 2) Then
                                ReDim Preserve PES_Parents_pChild(PES_Parents_pChild.GetLength(0))
                                akt = PES_Parents_pChild.GetUpperBound(0)
                                PES_Parents_pChild(akt) = New Individuum_CES("PES_Parent", PES_Parents_pChild.GetUpperBound(0))
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
    Private Sub Memory_Dubletten_loeschen(ByRef PES_Parents_pChild() As Individuum_CES)

        Dim tmp() As Individuum_CES
        tmp = Individuum.New_Indi_Array(Individuum.Individuumsklassen.Individuum_CES, PES_Parents_pChild.GetLength(0), "tmp")
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
    Private Function Memory_is_PES_Double(ByVal First As Individuum_CES, ByVal Second As Individuum_CES) As Boolean
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
                Select Case mSettings.CES.Mem_Strategy
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
    Private Sub Memory_Delete_too_weak(ByRef PES_Parents_pChild() As Individuum_CES)

        Dim i As Integer
        Dim Tmp(-1) As Individuum_CES

        For i = 0 To PES_Parents_pChild.GetUpperBound(0)

            Select Case mSettings.CES.Mem_Strategy

                Case MEMORY_STRATEGY.B_One_Loc_Up, MEMORY_STRATEGY.A_Two_Loc_Up
                    'UPSTREAM
                    If PES_Parents_pChild(i).Memory_Strat <= mSettings.CES.Mem_Strategy - Math.Min(0, PES_Parents_pChild(i).iLocation + mSettings.CES.Mem_Strategy - 1) Then
                        ReDim Preserve Tmp(Tmp.GetLength(0))
                        Tmp(Tmp.GetUpperBound(0)) = New Individuum_CES("PES_Parent", Tmp.GetUpperBound(0))
                        Tmp(Tmp.GetUpperBound(0)) = PES_Parents_pChild(i).Clone
                    End If

                Case MEMORY_STRATEGY.C_This_Loc
                    Tmp = PES_Parents_pChild.Clone

                Case MEMORY_STRATEGY.D_One_Loc_Down, MEMORY_STRATEGY.E_Two_Loc_Down
                    'DOWNSTREAM
                    If PES_Parents_pChild(i).Memory_Strat >= mSettings.CES.Mem_Strategy + Math.Min(0, ModSett.n_Locations - mSettings.CES.Mem_Strategy - PES_Parents_pChild(i).iLocation) Then
                        ReDim Preserve Tmp(Tmp.GetLength(0))
                        Tmp(Tmp.GetUpperBound(0)) = New Individuum_CES("PES_Parent", Tmp.GetUpperBound(0))
                        Tmp(Tmp.GetUpperBound(0)) = PES_Parents_pChild(i).Clone
                    End If
            End Select
        Next

        'Tmp zurück nach PES_Parents_pChild kopieren
        PES_Parents_pChild = Individuum_CES.Clone_Indi_Array(Tmp)

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
    Public Sub fill_Parents_per_Loc(ByRef Parents_pLoc() As Individuum_CES, ByVal n_eltern As Integer)

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
        'Sortiert die Individuen anhand des Penalty Wertes
        Dim i, j As Integer
        Dim swap As New Individuum_CES("swap", 0)

        For i = 0 To IndividuumList.GetUpperBound(0)
            For j = 0 To IndividuumList.GetUpperBound(0)
                If IndividuumList(i).PrimObjectives(0) < IndividuumList(j).PrimObjectives(0) Then
                    swap = IndividuumList(i)
                    IndividuumList(i) = IndividuumList(j)
                    IndividuumList(j) = swap
                End If
            Next j
        Next i

    End Sub

    'Hilfsfunktion checkt ob die neuen Child Zwillinge sind. Nur beim Generieren der ersten Pfade.
    '**********************************************************************************************
    Private Function Is_Twin(ByVal ChildIndex As Integer) As Boolean
        Dim n As Integer = 0
        Dim i, j As Integer
        Dim PathOK As Boolean
        PathOK = False
        Is_Twin = False

        For i = 0 To mSettings.CES.N_Children - 1
            If ChildIndex <> i And Children(i).mutated = True Then
                PathOK = False
                For j = 0 To Children(ChildIndex).Path.GetUpperBound(0)
                    If Children(ChildIndex).Path(j) <> Children(i).Path(j) Then
                        PathOK = True
                    End If
                Next
                If PathOK = False Then
                    Is_Twin = True
                End If
            End If
        Next
    End Function

    'Hilfsfunktion checkt ob die neuen Child Klone sind - Deaktiviert!
    '******************************************************************
    Private Function Is_Clone(ByVal ChildIndex As Integer) As Boolean
        Dim i, j As Integer
        Dim PathOK As Boolean
        PathOK = False
        Is_Clone = False

        For i = 0 To mSettings.CES.N_Parents - 1
            PathOK = False
            For j = 0 To Children(ChildIndex).Path.GetUpperBound(0)
                If Children(ChildIndex).Path(j) <> Parents(i).Path(j) Then
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
    Public Function BernoulliVer() As Boolean
        Dim lowerb As Integer = 0
        Dim upperbo As Integer = 1
        BernoulliVer = CInt(Int(2 * Rnd()))
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

        Dim i As Integer
        Dim lowerb As Integer = 0
        Dim upperb As Integer = ModSett.n_Locations - 2

        For i = 0 To CutPoint.GetUpperBound(0)
            CutPoint(i) = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
        Next

        Array.Sort(CutPoint)

    End Sub

    'Hilfsfunktion generiert eine Subpath mit Beginn und ende
    'Mit Bernoulli Verteilung mal von rechts mal von links
    '*****************************************************
    Public Sub Create_n_SubPath(ByRef BegEnde() As Integer)

        Dim i As Integer
        Dim lowerb As Integer = 0
        Dim upperb As Integer = ModSett.n_Locations - 1

        For i = 0 To BegEnde.GetUpperBound(0)
            BegEnde(i) = CInt(Int((upperb - lowerb + 1) * Rnd() + lowerb))
        Next

        Array.Sort(BegEnde)

    End Sub


    'NonDominated Sorting
    'XXXXXXXXXXXXXXXXXXXX

    'Steuerung des NDSorting (Ursprünglich aus ES Eltern)
    '****************************************************
    Public Sub NDSorting_CES_Control(ByVal iAktGen As Integer)
        Dim i As Integer

        Dim NDSorting() As Individuum_CES
        NDSorting = Individuum.New_Indi_Array(Individuum.Individuumsklassen.Individuum_CES, mSettings.CES.N_Children + mSettings.CES.N_Parents, "NDSorting")

        '0. Eltern und Nachfolger werden gemeinsam betrachtet
        'Die Kinder werden NDSorting hinzugefügt
        '-------------------------------------------

        For i = 0 To mSettings.CES.N_Children - 1
            NDSorting(i) = Children(i).Clone()
            NDSorting(i).Dominated = False
            NDSorting(i).Front = 0
            NDSorting(i).Distance = 0
        Next i

        '1. Eltern und Nachfolger werden gemeinsam betrachtet
        'Nur Eltern werden NDSorting hinzugefügt, Kinder sind schon oben drin
        '--------------------------------------------------------------------

        For i = mSettings.CES.N_Children To mSettings.CES.N_Children + mSettings.CES.N_Parents - 1
            NDSorting(i) = Parents(i - mSettings.CES.N_Children).Clone()
            NDSorting(i).Dominated = False
            NDSorting(i).Front = 0
            NDSorting(i).Distance = 0
        Next i

        '********************* Alles in der Klasse Functions ****************************************
        '2. Die einzelnen Fronten werden bestimmt
        '3. Der Bestwertspeicher wird entsprechend der Fronten oder der sekundären Population gefüllt
        '4: Sekundäre Population wird bestimmt und gespeichert
        '--------------------------------
        Dim Func1 As New ES.Functions(Me.mProblem, mSettings.CES.N_Children, mSettings.CES.N_Parents, mSettings.CES.Is_SecPopRestriction, mSettings.CES.N_MemberSecondPop, mSettings.CES.N_Interact, mSettings.CES.Is_SecPop, iAktGen + 1)
        Call Func1.EsEltern_Pareto(NDSorting, SekundärQb, IParents)
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

        Dim i As Integer

        Dim NDSorting() As Individuum_CES
        NDSorting = Individuum.New_Indi_Array(Individuum.Individuumsklassen.Individuum_CES, PES_Parents_pLoc.GetLength(0), "NDSorting")

        '1. ALLE werden reinkopiert (anders als beim normalen Verfahren)
        '---------------------------------------------------------------
        For i = 0 To PES_Parents_pLoc.GetUpperBound(0)
            NDSorting(i) = PES_Parents_pLoc(i).Clone()
            NDSorting(i).Dominated = False
            NDSorting(i).Front = 0
            NDSorting(i).Distance = 0
        Next i

        'Die Anzahlen werden hier speziell errechnet
        Dim n_PES_Children As Integer
        n_PES_Children = PES_Parents_pLoc.GetLength(0) - mSettings.PES.N_Eltern

        'Die Eltern werden zurückgesetzt
        ReDim PES_Parents_pLoc(mSettings.PES.N_Eltern - 1)

        '********************* Alles in der Klasse Functions ****************************************
        '2. Die einzelnen Fronten werden bestimmt
        '3. Der Bestwertspeicher wird entsprechend der Fronten oder der sekundären Population gefüllt
        '4: Sekundäre Population wird bestimmt und gespeichert
        '--------------------------------

        '! Sekundär_QB wird hier nicht berücksichtigt da die PES Generationen !
        '! wegen der Reduzierung auf Locations entkoppelt                     !
        Dim Fake_SekundärQb(-1) As Individuum
        Dim Func1 As New ES.Functions(Me.mProblem, n_PES_Children, mSettings.PES.N_Eltern, mSettings.PES.SekPop.Is_Begrenzung, mSettings.CES.N_PES_MemSecPop, mSettings.CES.N_PES_Interact, False, iAktGen + 1)
        Call Func1.EsEltern_Pareto(NDSorting, Fake_SekundärQb, IPES_Parents_pLoc)
        '********************************************************************************************

    End Sub

    'NDSorting incl. SekundärQB mit den EInstellungen aus dem PES
    '*************************************************************
    Sub NDSorting_Memory(ByVal iAktGen As Integer)

        Dim i As Integer

        Dim NDSorting() As Individuum_CES
        NDSorting = Individuum.New_Indi_Array(Individuum.Individuumsklassen.Individuum_CES, PES_Memory.GetLength(0), "NDSorting")

        '1. ALLE werden reinkopiert (anders als beim normalen Verfahren, aber wie bei per Location)
        '------------------------------------------------------------------------------------------
        For i = 0 To PES_Memory.GetUpperBound(0)
            NDSorting(i) = PES_Memory(i).Clone()
            NDSorting(i).Dominated = False
            NDSorting(i).Front = 0
            NDSorting(i).Distance = 0
        Next i

        'Die Anzahlen werden hier speziell errechnet
        Dim n_PES_Mem_Children As Integer
        n_PES_Mem_Children = PES_Memory.GetLength(0) - mSettings.CES.N_PES_MemSize

        'Die Eltern werden zurückgesetzt
        ReDim PES_Memory(mSettings.CES.N_PES_MemSize - 1)

        '********************* Alles in der Klasse Functions ****************************************
        '2. Die einzelnen Fronten werden bestimmt
        '3. Der Bestwertspeicher wird entsprechend der Fronten oder der sekundären Population gefüllt
        '4: Sekundäre Population wird bestimmt und gespeichert
        '--------------------------------
        'Sekundär_QB wird hier berücksichtigt!
        Dim Func1 As New ES.Functions(Me.mProblem, n_PES_Mem_Children, mSettings.CES.N_PES_MemSize, mSettings.PES.SekPop.Is_Begrenzung, mSettings.PES.SekPop.N_MaxMembers, mSettings.PES.SekPop.N_Interact, mSettings.PES.SekPop.Is_Interact, iAktGen + 1)
        Call Func1.EsEltern_Pareto(NDSorting, PES_Mem_SekundärQb, IPES_Memory)
        '********************************************************************************************

    End Sub

#End Region 'Methoden

End Class



