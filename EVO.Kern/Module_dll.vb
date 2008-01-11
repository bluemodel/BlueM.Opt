Option Strict Off
Option Explicit On

Public Module Constants

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
        Plus = 1                                'Eltern + Nachfolger
        Komma = 2                               'nur Nachfolger
    End Enum

    'Reproduktionsoperator
    Public Enum CES_REPRODOP as Integer
        Select_Random_Uniform = 1
        Order_Crossover_OX = 2
        Partially_Mapped_Crossover = 3
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
        Crowding = 1                            'MultiObjective
        Spannweite = 2                          'MultiObjective
    End Enum

End Module
