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
    Public Const EVO_MODUS_SINGEL_OBJECTIVE As Short = 0
    Public Const EVO_MODUS_MULTIOBJECTIVE_PARETO As Short = 1


    '******** Konstanten aus früherem myEVOS.vb ***************************
    '************* Hauptsächlich für KeyOK ********************************

    'Global globalrDatum() As Double
    'Global globalParWert() As Single
    '
    'Global QMorg(12) As Double
    'Global QSTDorg(12) As Double
    'Global CSorg(12) As Double
    'Global KORorg(12) As Double
    'Global REGMorg(12) As Double
    'Global REGBorg(12) As Double
    '
    'Global Const myoptRND = 0
    'Global Const myoptGENQMON = 1
    'Global Const myTypZRE = 1
    '
    'Global optRND As Integer
    'Global optGENQMON As Integer
    'Global EVOOPTION As Integer

    'Konstanten für erlaubte/unerlaubte Eingaben in Steuerelemente (TextBoxen)
    'Function: KeyOK() benutzt die Konstanten
    Public Const AllowFiguresOnly As Short = 1
    Public Const AllowPositiveFigures As Short = 2
    Public Const AllowFormatString As Short = 3
    Public Const AllowIntegerOnly As Short = 4
    Public Const AllowDateOnly As Short = 5


    Public Function KEYOK(ByRef Key As Short, ByRef nr As Short) As Short
        Dim IntegerZahlen, PositiveZahlen, AlleZahlen, Formatierung, DatumsFormat As String
        AlleZahlen = "-0123456789."
        PositiveZahlen = "0123456789."
        IntegerZahlen = "0123456789"
        DatumsFormat = "0123456789./: "
        Formatierung = "#.0"
        KEYOK = Key
        Select Case Key
            Case System.Windows.Forms.Keys.Return
                GoTo Err_KeyOK
            Case 1 To 31 'Spezielle Steuerzeichen
                'GoTo Err_KeyOK
            Case Else
                Select Case nr
                    Case AllowFiguresOnly 'Alle Zahlen
                        If InStr(AlleZahlen, Chr(Key)) = 0 Then GoTo Err_KeyOK
                    Case AllowPositiveFigures 'Alle Zahlen
                        If InStr(PositiveZahlen, Chr(Key)) = 0 Then GoTo Err_KeyOK
                    Case AllowFormatString 'Formatierung
                        If InStr(Formatierung, Chr(Key)) = 0 Then GoTo Err_KeyOK
                    Case AllowIntegerOnly 'Alle Zahlen
                        If InStr(IntegerZahlen, Chr(Key)) = 0 Then GoTo Err_KeyOK
                    Case AllowDateOnly 'Alle Zahlen
                        If InStr(DatumsFormat, Chr(Key)) = 0 Then GoTo Err_KeyOK
                End Select
        End Select
EXIT_KeyOK:
        Exit Function
Err_KeyOK:
        '  Beep
        KEYOK = 0
        GoTo EXIT_KeyOK
    End Function

End Module