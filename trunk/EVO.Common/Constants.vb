Option Strict Off
Option Explicit On

Public Module Constants

    '*******************************************************************************
    '*******************************************************************************
    '**** Modul mit Konstanten                                                  ****
    '****                                                                       ****
    '**** Autoren: Felix Fr�hlich, Christoph H�bner                             ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

    'Verschiedenes
    Public Const eol As String = Chr(13) & Chr(10)             'Zeilenumbruch

    'Optimierungsmethoden
    Public Const METH_PES As String = "PES"
    Public Const METH_CES As String = "CES"
    Public Const METH_HYBRID As String = "HYBRID"
    Public Const METH_SENSIPLOT As String = "SensiPlot"
    Public Const METH_HOOKJEEVES As String = "Hooke & Jeeves"
    Public Const METH_Hybrid2008 As String = "Hybrid2008"

    'Option f�r Startvorgabe f�r Optimierung
    Public Enum EVO_STARTPARAMETER As Integer
        Zufall = 1
        Original = 2
    End Enum

    'Modus der Optimierung
    Public Enum EVO_MODUS As Integer
        Single_Objective = 0
        Multi_Objective = 1
    End Enum

    'Evo-Strategie-Typ
    Public Enum EVO_STRATEGIE As Integer
        Plus_Strategie = 1                                'Eltern + Nachfolger
        Komma_Strategie = 2                               'nur Nachfolger
    End Enum

    'Reproduktionsoperator
    Public Enum CES_REPRODOP as Integer
        Selt_Rand_Uniform = 1
        Order_Crossover = 2
        Part_Mapped_Cross = 3
    End Enum

    'Mutationsoperator
    Public Enum CES_MUTATION as Integer
        RND_Switch = 1
        Dyn_Switch = 2
    End Enum

    'Option zur Erzeugung der Eltern
    Public Enum EVO_ELTERN As Integer
        Selektion = 1                           'zuf�llige Selektion as Bestwertspeicher
        XX_Diskret = 2                          'Rekombination nach x/x-Schema, diskretes vertauschen der Gene
        XX_Mitteln = 3                          'Rekombination nach x/x-Schema, mittelwertbildung der Gene �ber y-Eltern
        XY_Diskret = 4                          'Rekombination nach x/y-Schema, diskretes vertauschen der Gene
        XY_Mitteln = 5                          'Rekombination nach x/y-Schema, mittelwertbildung der Gene �ber y-Eltern
        Neighbourhood = 6                       'Neighboorghood-Rekombination
        XX_Mitteln_Diskret = 7                  'Rekombination nach x/x-Schema, diskretes vertauschen der Gene, miteln der Strategieparameter 
        XY_Mitteln_Diskret = 8                  'Rekombination nach x/y-Schema, diskretes vertauschen der Gene, miteln der Strategieparameter
    End Enum

    'Option f�r die Mutation
    Public Enum EVO_DnMutation As Integer
        Rechenberg = 1
        Schwefel = 2
    End Enum


    'Option zur Erzeugung der Pop-Eltern
    Public Enum EVO_POP_ELTERN As Integer
        Rekombination = 1                       'Rekombination aus den PopEltern
        Mittelwert = 2                          'Mittelwertbildung aus den PopEltern
        Selektion = 3                           'zuf�llige Selektion aus Bestwertspeicher
    End Enum

    'Option zur Ermittlung der Populationsqualit�t
    Public Enum EVO_POP_PENALTY As Integer
        Mittelwert = 1                          'SingleObjective
        Schlechtester = 2                       'SingleObjective
        Crowding = 3                            'MultiObjective
        Spannweite = 4                          'MultiObjective
    End Enum

    'Option zur Wahl des Hybrid Verfahrens
    Public Enum HYBRID_TYPE as Integer
        Mixed_Integer = 1                       'Mixed Integer durch Memory
        Sequencial_1 = 2                        'Erst CES dann PES
    End Enum

    'Option zur Wahl des Hybrid Verfahrens
    Public Enum CES_T_MODUS as Integer
        No_Test = 0                       'Mixed Integer durch Memory
        One_Combi = 1                        'Erst CES dann PES
        All_Combis = 2                        'Erst CES dann PES
    End Enum

    'Option zur Wahl des Hybrid Verfahrens
    Public Enum MEMORY_STRATEGY as Integer
        A_Two_Loc_Up = -2
        B_One_Loc_Up = -1
        C_This_Loc = 0
        D_One_Loc_Down = 1
        E_Two_Loc_Down = 2
    End Enum

    'Beziehung
    Public Enum Beziehung As Integer
        keine = 0
        kleiner = 1
        kleinergleich = 2
        groesser = 3
        groessergleich = 4
    End Enum

    'String in der Form < >, <=, >= in Beziehung umwandeln
    '*****************************************************
    Public Function getBeziehung(ByVal bez_str As String) As Beziehung
        Select Case bez_str
            Case "<"
                Return Beziehung.kleiner
            Case "<="
                Return Beziehung.kleinergleich
            Case ">"
                Return Beziehung.groesser
            Case ">="
                Return Beziehung.groessergleich
            Case Else
                Throw New Exception("Beziehung '" & bez_str & "' nicht erkannt!")
        End Select
    End Function

    'Richtung einer Zielfunktion
    Public Enum EVO_RICHTUNG As Integer
        Minimierung = 1
        Maximierung = -1
    End Enum

End Module