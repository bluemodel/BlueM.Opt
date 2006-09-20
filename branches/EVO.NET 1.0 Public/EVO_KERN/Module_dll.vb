Option Strict Off
Option Explicit On
Module Module1
	Public gdatServerStarted As Date
	
	'Option zur Erzeugung der Eltern:
	Public Const EVO_ELTERN_SELEKT As Short = 1 'zufällige Selektion as Bestwertspeicher
	Public Const EVO_ELTERN_XX_DISKRET As Short = 4 'Rekombination nach x/x-Schema, diskretes vertauschen der Gene
	Public Const EVO_ELTERN_XX_MITTELN As Short = 5 'Rekombination nach x/x-Schema, mittelwertbildung der Gene über y-Eltern
	Public Const EVO_ELTERN_XY_DISKRET As Short = 4 'Rekombination nach x/y-Schema, diskretes vertauschen der Gene
	Public Const EVO_ELTERN_XY_MITTELN As Short = 5 'Rekombination nach x/y-Schema, mittelwertbildung der Gene über y-Eltern
	Public Const EVO_ELTERN_Neighbourhood As Short = 6 'Neighboorghood-Rekombination
End Module