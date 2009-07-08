'*******************************************************************************
'*******************************************************************************
'**** Klasse EVO_Settings                                                   ****
'**** zum Speichern aller EVO-Einstellungen aus dem Form                    ****
'****                                                                       ****
'**** Autoren: Christoph Hübner, Felix Fröhlich, Dirk Muschalla             ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'****                                                                       ****
'**** November 2007                                                         ****
'****                                                                       ****
'**** Letzte Änderung: Dezember 2008                                        ****
'*******************************************************************************
'*******************************************************************************

Imports System.Xml.Serialization

Public Class EVO_Settings

    'Struct für generelle Settings
    '-----------------------------
    Public Structure General_Settings
        <System.ComponentModel.DefaultValueAttribute(True)> _
        Public useMultithreading As Boolean
        Public useMPC As Boolean
        Public Application As String
        Public Dataset As String
        Public Method As String
        <System.ComponentModel.DefaultValueAttribute(False)> _
        Public drawOnlyCurrentGeneration As Boolean
        Public Sub setStandard()
            Me.useMultithreading = True
            Me.drawOnlyCurrentGeneration = True
            Me.useMPC = False
            Me.Application = ""
            Me.Dataset = ""
            Me.Method = ""
        End Sub
    End Structure

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

        Public SekPop As Struct_SekPop

        Public Structure Struct_SekPop
            Public is_Interact As Boolean              'Mit Austausch zwischen Population und Sekundärer Population
            Public n_Interact As Integer               'Alle wieviel Generationen soll die aktuelle Population mit Mitgliedern der sekundären Population aufgefüllt werden
            Public is_Begrenzung As Boolean            'Soll die Anzahl Mitglieder in der SekPop begrenzt werden?
            Public n_MaxMembers As Integer             'Maximale Anzahl Mitglieder der Sekundärpopulation
        End Structure

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

                    Me.SekPop.is_Interact = False
                    Me.SekPop.n_Interact = 0
                    Me.SekPop.is_Begrenzung = False
                    Me.SekPop.n_MaxMembers = 0

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

                    Me.SekPop.is_Interact = True
                    Me.SekPop.n_Interact = 10
                    Me.SekPop.is_Begrenzung = True
                    Me.SekPop.n_MaxMembers = 50

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

    End Structure

    Public Structure DDS_Settings
        Public maxiter As Integer                  'Number of iterations
        Public r_val As Double                     'DDS perturbation parameter
        Public optStartparameter As Boolean

        'Standardwerte setzen
        '********************
        Public Sub setStandard()
            Me.maxiter = 1000
            Me.r_val = 0.2
            Me.optStartparameter = True
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

    Public Structure MPC_Settings
        Public Problempfad As String        'Pfad zum Problem
        Public Problemname As String        'Dateiname des Problems
        Public Inflowpfad As String         'Zuflüsse (Inflow-Datei)
        Public Ausgabepfad As String        'In welchem Ordner die Ausgaben erzeugt werden sollen
        Public SteuerungsAdaption As TimeSpan 'In welchen Abständen die Steuerung aktualisiert werden soll
        Public SteuerungsSchritt As TimeSpan 'In welchen Abständen eine Steuerung erfolgt
        Public MPC_Round As Integer         'Wie oft MPC schon gelaufen ist
        Public Vorhersage As TimeSpan       'Welcher Zeitraum zur Vorhersage herangezogen werden soll 
        Public BackupVorhersage As String   'Welches Verfahren genutzt wird, falls keine Daten zur Vorhersage bekannt sind
        Public Start As DateTime            'Wann die Berechnung der Steuerung gestartet werden soll
        Public Ende As DateTime             'Wann die Berechnung der Steuerung enden soll

    End Structure

    Public Structure MetaEvo_Settings

        Public Role As String                       'Was stellt dieser PC da: Single PC, Network Server, Network Client
        Public OpMode As String                     'Optimierungen: Local Optimizer, Global Optimizer, Both
        Public AlgoMode As String                   'Zustand des Algomanagers

        'Einstellungen für einen PC der das Problem berechnet (Global)
        Public PopulationSize As Integer            'Populationsgrösse
        Public NumberGenerations As Integer         'Anzahl der Generationen die berechnet werden
        Public CurrentGeneration As Integer         'Aktuelle Generationenzahl
        Public NumberResults As Integer             'Anzahl der Lösungen nach der Globalen Optimierung und am Ende

        'Einstellungen für einen PC der das Problem berechnet (Global)
        Public HJStepsize As Integer                '1/x minimale Schrittweite der Optparas bei Hook&Jeeves 

        'Datenbank-Connection
        Public MySQL_Host As String
        Public MySQL_Database As String
        Public MySQL_User As String
        Public MySQL_Password As String

        'Versteckte Optionen
        Public ChildsPerParent As Integer

        'Standardwerte setzen
        '********************
        Public Sub setStandard()
            Me.Role = "Single PC"
            Me.OpMode = "Both"

            Me.PopulationSize = 15
            Me.NumberGenerations = 50
            Me.CurrentGeneration = 0
            Me.NumberResults = 10

            Me.HJStepsize = 50

            Me.MySQL_Host = "localhost"
            Me.MySQL_Database = "metaevo_db"
            Me.MySQL_User = "remoteuser"
            Me.MySQL_Password = ""

            Me.ChildsPerParent = 3
        End Sub

    End Structure

    Public Structure SensiPlot_Settings

        Public Enum SensiType As Integer
            discrete = 1
            normaldistribution = 2
        End Enum

        Public Selected_OptParameters() As Integer
        Public Selected_Objective As Integer
        Public Selected_SensiType As SensiType
        Public Num_Steps As Integer
        Public show_Wave As Boolean

        Public Sub setStandard()
            'Standardmäßig ersten OptParameter und erstes Objective auswählen
            ReDim Me.Selected_OptParameters(0)
            Me.Selected_OptParameters(0) = 0
            Me.Selected_Objective = 0
            Me.Selected_SensiType = SensiType.discrete
            Me.Num_Steps = 10
            Me.show_Wave = False
        End Sub

    End Structure

    Public General As General_Settings
    Public PES As PES_Settings
    Public CES As CES_Settings
    Public HookJeeves As HookJeeves_Settings
    Public MetaEvo As MetaEvo_Settings
    Public MPC As MPC_Settings
    Public DDS As DDS_Settings
    Public SensiPlot As SensiPlot_Settings

End Class
