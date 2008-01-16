Imports System.Xml.Serialization

Public Class EVO_Settings

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse EVO_Settings                                                   ****
    '**** zum Speichern aller EVO-Einstellungen aus dem Form                    ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
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
        Public Sub setStandard(ByVal modus As Kern.EVO_MODUS)

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
        Public n_Generations As Integer  
        Public n_Parents As Integer
        Public n_Childs As Integer
        Public OptStrategie As EVO_STRATEGIE           '"plus" oder "minus" Strategie
        Public OptReprodOp As CES_REPRODOP
        Public OptMutOperator As CES_MUTATION
        Public pr_MutRate As Integer                   'Definiert die Wahrscheinlichkeit der Mutationsrate in %
        
        Public is_SecPop As Boolean
        Public n_MemberSecondPop As Integer
        Public n_Interact As Integer

        'Hybrid
        Public n_PartsMem As Integer           'Länge des Gedächtnispfades Achtung Maximum ist 3
        Public n_PES_MaxParents As Integer             'Anzahl der Eltern für PES Hybrid
        
        Public is_PES_SecPop As Boolean
        Public n_PES_MemSecPop As Integer
        Public n_PES_Interact As Integer

        Public is_PopMutStart As Boolean                    'Gibt an ob bei der Population oder bei den Eltern in die PES gestiegen wird.

        'Standardwerte setzen
        '********************
        Public Sub setStandard(ByVal modus As Kern.EVO_MODUS)

            Select Case modus

                Case EVO_MODUS.Single_Objective

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
                    Me.n_PartsMem = 3
                    Me.n_PES_MaxParents = 5
        
                    Me.is_PES_SecPop = False
                    Me.n_PES_MemSecPop = 50
                    Me.n_PES_Interact = 5

                    Me.is_PopMutStart = False

                Case EVO_MODUS.Multi_Objective

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
                    Me.n_PartsMem = 3
                    Me.n_PES_MaxParents = 5
        
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
    Public HookJeeves As HookJeeves_Settings
End Class
