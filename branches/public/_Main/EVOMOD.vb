Option Strict Off
Option Explicit On

Module EVOMOD
	
    '******** Konstanten aus früherem myEVOS.vb ***************************
    '************* Hauptsächlich für KeyOK ********************************

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