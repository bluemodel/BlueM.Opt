Imports System.Xml.Serialization

Public Class Settings_CES

    'CES
    Public n_Generations As Integer         'Anzahl der Generationen
    Public n_Parents As Integer             'Anzahl der Eltern
    Public n_Children As Integer            'Anzahl der Kinder
    Public OptStrategie As EVO_STRATEGIE    '"plus" oder "minus" Strategie
    Public OptReprodOp As CES_REPRODOP      'Reprofuktionaoperator
    Public k_Value As Integer               'Anzahl der Schnittpunkte
    Public OptMutOperator As CES_MUTATION   'Mutationsoperator
    Public pr_MutRate As Integer            'Definiert die Wahrscheinlichkeit der Mutationsrate in %

    Public is_SecPop As Boolean             'SekundärePopulation an oder aus
    Public is_SecPopRestriction As Boolean  'Sekundäre Population begrenzen
    Public n_MemberSecondPop As Integer     'Max Anzahl der Mitglieder der Sekundären Population
    Public n_Interact As Integer            'Austausch mit SekPop nach n Generationen

    'Hybrid
    Public is_RealOpt As Boolean            'gibt an ob auch die Real Parameter optimiert werden sollen
    Public ty_Hybrid As HYBRID_TYPE         'gibt den Hybrid Typ an
    Public Mem_Strategy As MEMORY_STRATEGY  'Gibt die Memory Strategy an
    Public n_PES_MemSize As Integer         'Die Größe des PES Memory
    Public is_PES_SecPop As Boolean         'SekundärePopulation für PES an oder aus
    Public n_PES_MemSecPop As Integer       'Anzahl der Mitglieder der Sekundären Population für PES
    Public n_PES_Interact As Integer        'Austausch mit SekPop für PES nach n Generationen
    Public is_PopMutStart As Boolean        'Gibt an ob die PES bei der Population oder bei den Eltern gestartet wird.

    'Standardwerte setzen
    '********************
    Public Sub setStandard(ByVal Method As String)

        Select Case Method

            Case METH_CES

                'CES
                Me.n_Generations = 500
                Me.n_Parents = 5
                Me.n_Children = 15
                Me.OptStrategie = EVO_STRATEGIE.Plus_Strategie
                Me.OptReprodOp = CES_REPRODOP.Uniform_Crossover
                Me.OptMutOperator = CES_MUTATION.RND_Switch
                Me.pr_MutRate = 25

                Me.is_SecPop = True
                Me.is_SecPopRestriction = True
                Me.n_MemberSecondPop = 50
                Me.n_Interact = 5

                'Hybrid
                Me.is_RealOpt = False
                Me.ty_Hybrid = HYBRID_TYPE.Mixed_Integer
                Me.Mem_Strategy = MEMORY_STRATEGY.C_This_Loc
                Me.n_PES_MemSize = 500

                Me.is_PES_SecPop = False
                Me.n_PES_MemSecPop = 50
                Me.n_PES_Interact = 5
                Me.is_PopMutStart = False

            Case METH_HYBRID

                'CES
                Me.n_Generations = 100
                Me.n_Parents = 3
                Me.n_Children = 7
                Me.OptStrategie = EVO_STRATEGIE.Plus_Strategie
                Me.OptReprodOp = CES_REPRODOP.Uniform_Crossover
                Me.OptMutOperator = CES_MUTATION.RND_Switch
                Me.pr_MutRate = 25

                Me.is_SecPop = True
                Me.is_SecPopRestriction = True
                Me.n_MemberSecondPop = 50
                Me.n_Interact = 5

                'Hybrid
                Me.is_RealOpt = True
                Me.ty_Hybrid = HYBRID_TYPE.Mixed_Integer
                Me.Mem_Strategy = MEMORY_STRATEGY.C_This_Loc
                Me.n_PES_MemSize = 500

                Me.is_PES_SecPop = False
                Me.n_PES_MemSecPop = 50
                Me.n_PES_Interact = 5
                Me.is_PopMutStart = False

        End Select

    End Sub

End Class
