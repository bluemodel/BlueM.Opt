Option Strict Off
Option Explicit On
Module EVOMOD
	
	Public Const AppName As String = "EVO-Strategie 1.0"
	
	'Evo-Strategie-Typ:
	Public Const EVO_PLUS As Short = 1
	Public Const EVO_KOMMA As Short = 2
	'Option zur Erzeugung der Pop-Eltern:
	Public Const EVO_POPELTERN_REKOMB As Short = 1 'Rekombination aus den PopEltern
	Public Const EVO_POPELTERN_MITTEL As Short = 2 'Mittelwertbildung aus den PopEltern
	Public Const EVO_POPELTERN_SELEKT As Short = 3 'zufällige Selektion aus Bestwertspeicher
	'Option zur Erzeugung der Eltern:
	Public Const EVO_ELTERN_SELEKT As Short = 1 'zufällige Selektion as Bestwertspeicher
	Public Const EVO_ELTERN_XX_DISKRET As Short = 2 'Rekombination nach x/x-Schema, diskretes vertauschen der Gene
	Public Const EVO_ELTERN_XX_MITTELN As Short = 3 'Rekombination nach x/x-Schema, mittelwertbildung der Gene über y-Eltern
	Public Const EVO_ELTERN_XY_DISKRET As Short = 4 'Rekombination nach x/y-Schema, diskretes vertauschen der Gene
	Public Const EVO_ELTERN_XY_MITTELN As Short = 5 'Rekombination nach x/y-Schema, mittelwertbildung der Gene über y-Eltern
	Public Const EVO_ELTERN_Neighbourhood As Short = 6 'Neighboorghood-Rekombination
	'Option zur Erzeugung der Startwerte aller Parameter
	Public Const EVO_START_ZUF As Short = 0
	Public Const EVO_START_INI As Short = 1
	'Maximale Anzahl an Parametern
	Public Const globalMAXPAR As Short = 200
	'Maximaler Parameterwert
	Public Const globalMINWERT As Short = 0
	Public Const globalMAXWERT As Short = 100
	'Option für Startvorgabe für Optimierung
	Public Const globalVORGABE_ZUFALL As Short = 1
	Public Const globalVORGABE_Original As Short = 2
	'Optionen für Ermittlung der Populationsqualität
	Public Const EVO_POP_PENALTY_MITTELWERT As Short = 1
	Public Const EVO_POP_PENALTY_SCHLECHTESTER As Short = 2
	Public Const EVO_POP_PENALTY_CROWDING As Short = 1
	Public Const EVO_POP_PENALTY_SPANNWEITE As Short = 2
	'Modus der Optimierung
	Public Const EVO_MODUS_SINGEL_OBJECTIVE As Short = 1
	Public Const EVO_MODUS_MULTIOBJECTIVE As Short = 2
	Public Const EVO_MODUS_MULTIOBJECTIVE_PARETO As Short = 3
End Module