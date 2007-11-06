Option Strict Off
Option Explicit On

Public Module Constants

    'Option f�r Startvorgabe f�r Optimierung
    Public Enum EVO_STARTPARAMETER As Short
        Zufall = 1
        Original = 2
    End Enum

    'Modus der Optimierung
    Public Enum EVO_MODUS As Short
        Single_Objective = 0
        Multi_Objective = 1
    End Enum

    'Evo-Strategie-Typ
    Public Enum EVO_STRATEGIE As Short
        Plus = 1                                'Eltern + Nachfolger
        Komma = 2                               'nur Nachfolger
    End Enum

    'Option zur Erzeugung der Eltern
    Public Enum EVO_ELTERN As Short
        Selektion = 1                           'zuf�llige Selektion as Bestwertspeicher
        XX_Diskret = 2                          'Rekombination nach x/x-Schema, diskretes vertauschen der Gene
        XX_Mitteln = 3                          'Rekombination nach x/x-Schema, mittelwertbildung der Gene �ber y-Eltern
        XY_Diskret = 4                          'Rekombination nach x/y-Schema, diskretes vertauschen der Gene
        XY_Mitteln = 5                          'Rekombination nach x/y-Schema, mittelwertbildung der Gene �ber y-Eltern
        Neighbourhood = 6                       'Neighboorghood-Rekombination
    End Enum

    'Option zur Erzeugung der Pop-Eltern
    Public Enum EVO_POP_ELTERN As Short
        Rekombination = 1                       'Rekombination aus den PopEltern
        Mittelwert = 2                          'Mittelwertbildung aus den PopEltern
        Selektion = 3                           'zuf�llige Selektion aus Bestwertspeicher
    End Enum

    'Option zur Ermittlung der Populationsqualit�t
    Public Enum EVO_POP_PENALTY As Short
        Mittelwert = 1                          'SingleObjective
        Schlechtester = 2                       'SingleObjective
        Crowding = 1                            'MultiObjective
        Spannweite = 2                          'MultiObjective
    End Enum

End Module
