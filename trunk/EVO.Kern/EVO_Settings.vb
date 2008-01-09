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

        Public ty_EvoModus As EVO_MODUS            'Single- oder Multi-Objective
        Public ty_EvoStrategie As EVO_STRATEGIE    'Typ der Evolutionsstrategie (+ oder ,)
        Public ty_StartPar As EVO_STARTPARAMETER   'Startparameter

        Public DnStart As Double                   'Startschrittweite
        Public DNepsilon As Double              'Minimale Schrittweite
        Public is_DnVektor As Boolean              'Soll ein Schrittweitenvektor benutzt werden
        Public ty_DNMutation As EVO_DNMutation   'Art der Mutation
        Public DNC As Double                       'Skalierung des learning Parameters

        Public n_Gen As Integer                    'Anzahl Generationen
        Public n_Eltern As Integer                 'Anzahl Eltern
        Public n_Nachf As Integer                  'Anzahl Kinder

        Public is_Interact As Boolean              'Mit Austausch zwischen Population und Sekundärer Population
        Public n_Interact As Integer               'Alle wieviel Generationen soll die aktuelle Population mit Mitgliedern der sekundären Population aufgefüllt werden

        Public n_MemberSekPop As Integer           'Maximale Anzahl Mitglieder der Sekundärpopulation

        Public ty_OptEltern As EVO_ELTERN          'Ermittlung der Individuum-Eltern
        Public n_RekombXY As Integer               'X/Y-Schema Rekombination

        Public Pop As Struct_Pop_Settings

        Public Structure Struct_Pop_Settings
            <XmlAttribute()> _
            Public is_POPUL As Boolean                 'Mit Populationen
            Public n_Runden As Integer                 'Anzahl Runden
            Public n_Popul As Integer                  'Anzahl Populationen
            Public n_PopEltern As Integer              'Anzahl Populationseltern
            Public ty_OptPopEltern As EVO_POP_ELTERN   'Ermittlung der Populationseltern
            Public ty_PopEvoTyp As EVO_STRATEGIE       'Typ der Evolutionsstrategie (+ oder ,) auf Populationsebene
            Public ty_PopPenalty As EVO_POP_PENALTY    'Art der Beurteilung der Populationsgüte (Multiobjective)
        End Structure

        'Standardwerte setzen
        '********************
        Public Sub setStandard(ByVal modus As Kern.EVO_MODUS)

            Select Case modus

                Case EVO_MODUS.Single_Objective

                    Me.ty_EvoModus = EVO_MODUS.Single_Objective
                    Me.ty_EvoStrategie = EVO_STRATEGIE.Plus
                    Me.ty_StartPar = EVO_STARTPARAMETER.Original

                    Me.ty_DNMutation = EVO_DNMutation.Schwefel
                    Me.DnStart = 0.1
                    Me.DNepsilon = 0.001
                    Me.is_DnVektor = False
                    Me.DNC = 1.0

                    Me.n_Gen = 100
                    Me.n_Eltern = 3
                    Me.n_Nachf = 10

                    Me.is_Interact = False
                    Me.n_Interact = 0
                    Me.n_MemberSekPop = 0

                    Me.ty_OptEltern = EVO_ELTERN.XX_Mitteln_Diskret
                    Me.n_RekombXY = 3

                    Me.Pop.is_POPUL = False
                    Me.Pop.n_Runden = 10
                    Me.Pop.n_Popul = 3
                    Me.Pop.n_PopEltern = 2
                    Me.Pop.ty_OptPopEltern = EVO_POP_ELTERN.Rekombination
                    Me.Pop.ty_PopEvoTyp = EVO_STRATEGIE.Plus
                    Me.Pop.ty_PopPenalty = EVO_POP_PENALTY.Mittelwert


                Case EVO_MODUS.Multi_Objective

                    Me.ty_EvoModus = EVO_MODUS.Multi_Objective
                    Me.ty_EvoStrategie = EVO_STRATEGIE.Plus
                    Me.ty_StartPar = EVO_STARTPARAMETER.Original

                    Me.DnStart = 0.1
                    Me.is_DnVektor = False
                    Me.is_DnVektor = False
                    Me.DNC = 1.0

                    Me.n_Gen = 100
                    Me.n_Eltern = 15
                    Me.n_Nachf = 50

                    Me.is_Interact = True
                    Me.n_Interact = 10
                    Me.n_MemberSekPop = 50

                    Me.ty_OptEltern = EVO_ELTERN.XX_Diskret
                    Me.n_RekombXY = 3

                    Me.Pop.is_POPUL = False
                    Me.Pop.n_Runden = 1
                    Me.Pop.n_Popul = 1
                    Me.Pop.n_PopEltern = 1
                    Me.Pop.ty_OptPopEltern = EVO_POP_ELTERN.Rekombination
                    Me.Pop.ty_PopEvoTyp = EVO_STRATEGIE.Plus
                    Me.Pop.ty_PopPenalty = EVO_POP_PENALTY.Mittelwert
            End Select
        End Sub

    End Structure

    'Struct für die CES_Settings
    '---------------------------
    Public Structure CES_Settings

        'CES
        Public n_Parts_of_Path As Integer           'Länge des Gedächtnispfades Achtung Maximum ist 3
        Public n_Generations As Integer             'Anzahl der Generationen
        Public n_Parents As Integer
        Public n_Childs As Integer
        Public n_MemberSecondPop As Integer
        Public n_Interact As Integer
        Public is_Interact As Boolean
        Public ReprodOperator As String
        Public MutOperator As String
        Public Strategy As String                   '"plus" oder "minus" Strategie
        Public MutRate As Integer                   'Definiert die Wahrscheinlichkeit der Mutationsrate in %

        'Hybrid
        Public n_PES_Parents As Integer             'Anzahl der Eltern für PES Hybrid
        Public n_PES_MemberSecondPop As Integer
        Public n_PES_Interact As Integer
        Public is_PES_Interact As Boolean
        Public is_Pop As Boolean                    'Gibt an ob bei der Population oder bei den Eltern in die PES gestiegen wird.

        'Standardwerte setzen
        '********************
        Public Sub setStandard(ByVal modus As Kern.EVO_MODUS)

            Select Case modus

                Case EVO_MODUS.Single_Objective

                    'CES
                    Me.n_Parts_of_Path = 3
                    Me.n_Generations = 500
                    Me.n_Parents = 5
                    Me.n_Childs = 15
                    Me.n_MemberSecondPop = 50
                    Me.n_Interact = 5
                    Me.is_Interact = True
                    Me.ReprodOperator = "Select_Random_Uniform"
                    Me.MutOperator = "RND_Switch"
                    Me.Strategy = "plus"
                    Me.MutRate = 25

                    'Hybrid
                    Me.n_PES_Parents = 5
                    Me.n_PES_MemberSecondPop = 50
                    Me.n_PES_Interact = 3
                    Me.is_PES_Interact = False
                    Me.is_Pop = False

                Case EVO_MODUS.Multi_Objective

                    'CES
                    Me.n_Parts_of_Path = 3
                    Me.n_Generations = 500
                    Me.n_Parents = 5
                    Me.n_Childs = 15
                    Me.n_MemberSecondPop = 50
                    Me.n_Interact = 5
                    Me.is_Interact = True
                    Me.ReprodOperator = "Select_Random_Uniform"
                    Me.MutOperator = "RND_Switch"
                    Me.Strategy = "plus"
                    Me.MutRate = 25

                    'Hybrid
                    Me.n_PES_Parents = 5
                    Me.n_PES_MemberSecondPop = 50
                    Me.n_PES_Interact = 3
                    Me.is_PES_Interact = False
                    Me.is_Pop = False

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
