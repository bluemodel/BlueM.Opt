Imports System.Xml.Serialization

Public Class EVO_Settings

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse EVO_Settings                                                   ****
    '**** zum Speichern aller EVO-Einstellungen aus dem Form                    ****
    '****                                                                       ****
    '**** Autoren: Christoph Hübner, Felix Fröhlich                             ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '****                                                                       ****
    '**** November 2007                                                         ****
    '****                                                                       ****
    '**** Letzte Änderung: März 2008                                            ****
    '*******************************************************************************
    '*******************************************************************************

    'Struct für die PES_Settings
    '---------------------------
    Public Structure PES_Settings

        Public OptModus As EVO_MODUS                       'Single- oder Multi-Objective
        Public OptStrategie As EVO_STRATEGIE               'Typ der Evolutionsstrategie (+ oder ,)
        Public OptStartparameter As EVO_STARTPARAMETER     'Startparameter

        Public Schrittweite As Struct_Schrittweite

        Public Structure Struct_Schrittweite
            Public DnStart As Double                   'Startschrittweite
            Public DnEpsilon As Double                 'Minimale Schrittweite
            Public is_DnVektor As Boolean              'Soll ein Schrittweitenvektor benutzt werden
            Public OptDnMutation As EVO_DnMutation     'Art der Mutation
            Public DnC As Double                       'Skalierung des learning Parameters
        End Structure

        Public n_Gen As Integer                    'Anzahl Generationen
        Public n_Eltern As Integer                 'Anzahl Eltern
        Public n_Nachf As Integer                  'Anzahl Kinder

        Public is_Interact As Boolean              'Mit Austausch zwischen Population und Sekundärer Population
        Public n_Interact As Integer               'Alle wieviel Generationen soll die aktuelle Population mit Mitgliedern der sekundären Population aufgefüllt werden

        Public n_MemberSekPop As Integer           'Maximale Anzahl Mitglieder der Sekundärpopulation

        Public OptEltern As EVO_ELTERN             'Ermittlung der Individuum-Eltern
        Public n_RekombXY As Integer               'X/Y-Schema Rekombination
        Public is_DiversityTournament As Boolean   'Vor der eigentlichen Auswahl eines Elter wird zunächst nach der besseren Diversity geschaut

        Public Pop As Struct_Pop_Settings

        Public Structure Struct_Pop_Settings
            <XmlAttribute()> _
            Public is_POPUL As Boolean                  'Mit Populationen
            Public n_Runden As Integer                  'Anzahl Runden
            Public n_Popul As Integer                   'Anzahl Populationen
            Public n_PopEltern As Integer               'Anzahl Populationseltern
            Public OptPopEltern As EVO_POP_ELTERN       'Ermittlung der Populationseltern
            Public OptPopStrategie As EVO_STRATEGIE     'Typ der Evolutionsstrategie (+ oder ,) auf Populationsebene
            Public OptPopPenalty As EVO_POP_PENALTY     'Art der Beurteilung der Populationsgüte (Multiobjective)
        End Structure

        Public is_paint_constraint As Boolean      'Nur die Individuuen der aktuellen Generation werden gezeichnet

        'Standardwerte setzen
        '********************
        Public Sub setStandard(ByVal modus As EVO_MODUS)

            Select Case modus

                Case EVO_MODUS.Single_Objective

                    Me.OptModus = EVO_MODUS.Single_Objective
                    Me.OptStrategie = EVO_STRATEGIE.Plus_Strategie
                    Me.OptStartparameter = EVO_STARTPARAMETER.Original

                    Me.Schrittweite.OptDnMutation = EVO_DnMutation.Schwefel
                    Me.Schrittweite.DnStart = 0.1
                    Me.Schrittweite.DnEpsilon = 0.001
                    Me.Schrittweite.is_DnVektor = False
                    Me.Schrittweite.DnC = 1.0

                    Me.n_Gen = 100
                    Me.n_Eltern = 3
                    Me.n_Nachf = 10

                    Me.is_Interact = False
                    Me.n_Interact = 0
                    Me.n_MemberSekPop = 0

                    Me.OptEltern = EVO_ELTERN.XX_Mitteln_Diskret
                    Me.n_RekombXY = 3
                    Me.is_DiversityTournament = False

                    Me.Pop.is_POPUL = False
                    Me.Pop.n_Runden = 10
                    Me.Pop.n_Popul = 3
                    Me.Pop.n_PopEltern = 2
                    Me.Pop.OptPopEltern = EVO_POP_ELTERN.Rekombination
                    Me.Pop.OptPopStrategie = EVO_STRATEGIE.Plus_Strategie
                    Me.Pop.OptPopPenalty = EVO_POP_PENALTY.Mittelwert

                    Me.is_paint_constraint = False


                Case EVO_MODUS.Multi_Objective

                    Me.OptModus = EVO_MODUS.Multi_Objective
                    Me.OptStrategie = EVO_STRATEGIE.Plus_Strategie
                    Me.OptStartparameter = EVO_STARTPARAMETER.Original

                    Me.Schrittweite.DnStart = 0.1
                    Me.Schrittweite.is_DnVektor = False
                    Me.Schrittweite.is_DnVektor = False
                    Me.Schrittweite.DnC = 1.0

                    Me.n_Gen = 100
                    Me.n_Eltern = 15
                    Me.n_Nachf = 50

                    Me.is_Interact = True
                    Me.n_Interact = 10
                    Me.n_MemberSekPop = 50

                    Me.OptEltern = EVO_ELTERN.XX_Mitteln_Diskret
                    Me.n_RekombXY = 3
                    Me.is_DiversityTournament = True

                    Me.Pop.is_POPUL = False
                    Me.Pop.n_Runden = 1
                    Me.Pop.n_Popul = 1
                    Me.Pop.n_PopEltern = 1
                    Me.Pop.OptPopEltern = EVO_POP_ELTERN.Rekombination
                    Me.Pop.OptPopStrategie = EVO_STRATEGIE.Plus_Strategie
                    Me.Pop.OptPopPenalty = EVO_POP_PENALTY.Mittelwert

                    Me.is_paint_constraint = True

            End Select
        End Sub

    End Structure

    'Struct für die CES_Settings
    '---------------------------
    Public Structure CES_Settings

        'CES
        Public n_Generations As Integer         'Anzahl der Generationen
        Public n_Parents As Integer             'Anzahl der Eltern
        Public n_Childs As Integer              'Anzahl der Kinder
        Public OptStrategie As EVO_STRATEGIE    '"plus" oder "minus" Strategie
        Public OptReprodOp As CES_REPRODOP      'Reprofuktionaoperator
        Public OptMutOperator As CES_MUTATION   'Mutationsoperator
        Public pr_MutRate As Integer            'Definiert die Wahrscheinlichkeit der Mutationsrate in %
        
        Public is_SecPop As Boolean             'SekundärePopulation an oder aus
        Public n_MemberSecondPop As Integer     'Anzahl der Mitglieder der Sekundären Population
        Public n_Interact As Integer            'Austausch mit SekPop nach n Generationen

        'Hybrid
        Public is_RealOpt as Boolean            'gibt an ob auch die Real Parameter optimiert werden sollen
        Public ty_Hybrid as HYBRID_TYPE         'gibt den Hybrid Typ an
        Public Mem_Strategy as MEMORY_STRATEGY  'Gibt die Memory Strategy an
        Public n_PES_MemSize as Integer         'Die Größe des PES Memory
        Public is_PES_SecPop As Boolean         'SekundärePopulation für PES an oder aus
        Public n_PES_MemSecPop As Integer       'Anzahl der Mitglieder der Sekundären Population für PES
        Public n_PES_Interact As Integer        'Austausch mit SekPop für PES nach n Generationen
        Public is_PopMutStart As Boolean        'Gibt an ob die PES bei der Population oder bei den Eltern gestartet wird.

        'Standardwerte setzen
        '********************
        Public Sub setStandard(ByVal Method As String)

            Select Case Method

                Case "CES"

                    'CES
                    Me.n_Generations = 500
                    Me.n_Parents = 5
                    Me.n_Childs = 15
                    Me.OptStrategie = EVO_STRATEGIE.Plus_Strategie
                    Me.OptReprodOp = CES_REPRODOP.Selt_Rand_Uniform
                    Me.OptMutOperator = CES_MUTATION.RND_Switch
                    Me.pr_MutRate = 25
        
                    Me.is_SecPop = True
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

                Case "HYBRID"

                    'CES
                    Me.n_Generations = 100
                    Me.n_Parents = 3
                    Me.n_Childs = 7
                    Me.OptStrategie = EVO_STRATEGIE.Plus_Strategie
                    Me.OptReprodOp = CES_REPRODOP.Selt_Rand_Uniform
                    Me.OptMutOperator = CES_MUTATION.RND_Switch
                    Me.pr_MutRate = 25

                    Me.is_SecPop = True
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

    End Structure

    Public Structure HookJeeves_Settings
        Public DnStart As Double                   'Startschrittweite
        Public is_DnVektor As Boolean              'Soll ein Schrittweitenvektor benutzt werden
        Public DnFinish As Double                  'Abbruchschrittweite

        'Standardwerte setzen
        '********************
        Public Sub setStandard()
            Me.DnStart = 0.1
            Me.is_DnVektor = False
            Me.DnFinish = 0.0001
        End Sub

    End Structure

    Public PES As PES_Settings
    Public CES As CES_Settings
    Public HookJeeves As HookJeeves_Settings
End Class
