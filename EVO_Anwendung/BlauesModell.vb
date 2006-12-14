Imports System.IO
Module BlauesModell

    Public Sub Anfangsparameter_auslesen()

        'Select Case ComboXY_Text
        '    Case "Auto_Kalibrierung"

        '    Case "HW_Optimierung"

        'End Select

    End Sub

    Public Sub Messung_einlesen()

    End Sub

    Public Sub Parameter_schreiben()

        'Too Parameter müssen vor der Simulation geschrieben werden

    End Sub

    Public Sub launchBM(ByVal Exe As String, ByVal Pfad As String, ByVal Datensatz As String)
        'starte Programm mit neuen Parametern
        Dim ProcID As Integer
        'Aktuelles Arbeitsverzeichnis feststellen
        Dim currentDir As String = CurDir()
        'gewünschtes Arbeitsverzeichnis definieren
        Dim workingDir As String = Pfad
        'zum gewünschten Arbeitsverzeichnis navigieren
        ChDrive(workingDir) 'nur nötig falls Arbeitsverzeichnis und aktuelles Verzeichnis auf verschiedenen Laufwerken sind
        ChDir(workingDir)
        'EXE aufrufen
        ProcID = Shell("""" & Exe & """ " & Datensatz, AppWinStyle.NormalFocus, True)
        'Arbeitsverzeichnis wieder zurücksetzen (optional)
        ChDrive(currentDir)
        ChDir(currentDir)
    End Sub

    'Hier wird die Ergebnisdatei nach jeder Simulation ausgelesen
    Public Sub Ergebisdatei_auslesen()

        'ToDo: lesen der Ergebnisdatei

    End Sub

    'Der Qualitätswert wird durch Vergleich von Calculation Berechnet.
    'ToDo: Evtl. Cases für die Verschiedenen Berechnungsarten einbauen
    Public Function Qualitaetswert() As Double

        'ToDo: Berechnen des Qualitätswertes

    End Function



    'Public Sub modifyCN()
    '    Dim Text, TextVR, TextCN, TextBla, Textnew, fill, TextExpo, out As String

    '    Try
    '        Dim FiStr As FileStream = New FileStream(Form1.Pfad + Form1.Datensatz + ".EZG", FileMode.Open, FileAccess.ReadWrite)
    '        Dim StrRe As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
    '        Dim i, j, param As Integer

    '        param = 0
    '        Text = ""
    '        out = ""

    '        Do
    '            Text = StrRe.ReadLine.ToString
    '            i += 1
    '            TextBla = ""

    '            If i > 11 And i < 17 Then

    '                param += 1
    '                Dim strarray() As String = Text.Split("|"c)
    '                strarray(1) = " |" + strarray(1)
    '                For j = 1 To 8
    '                    TextBla = TextBla + strarray(j) + "|"
    '                Next
    '                TextVR = strarray(9).ToString.Substring(4, 5).Trim().ToString
    '                TextCN = (60 + Int(Form1.mypara(param, 1) * (90 - 60))).ToString
    '                param += 1
    '                TextExpo = (0.9 + (Form1.mypara(param, 1) * (1.1 - 0.9))).ToString.Substring(0, 4)

    '                'CN und Vorregen
    '                fill = "   "
    '                Text = TextBla + "  " + TextCN + fill + TextVR + " |"
    '                Text = Text + strarray(10) + "|"

    '                'Exponent
    '                TextBla = strarray(11).ToString.Substring(0, strarray(11).Length - 4)
    '                Text = Text + TextBla + TextExpo + "|"
    '                Text = Text + strarray(12) + "|"

    '                'Console.Out.WriteLine(Text)

    '            End If
    '            Textnew = Textnew + Text + Environment.NewLine
    '        Loop Until StrRe.Peek() = -1

    '        StrRe.Close()
    '        FiStr.Close()

    '        Dim StrWri As StreamWriter = New StreamWriter(Form1.Pfad + Form1.Datensatz + ".EZG", False, System.Text.Encoding.GetEncoding("iso8859-1"))
    '        StrWri.Write(Textnew)
    '        StrWri.Close()

    '    Catch except As Exception
    '        MsgBox(except.Message, "Fehler in modifyVR", MsgBoxStyle.Exclamation)
    '    End Try



    'End Sub

    'Public Sub modifyBOF()
    '    Dim Text, KVG, K1, K2, Int, Bas, Expo, Beta1, Beta2, TextBla, Textnew, out As String

    '    Try
    '        Dim FiStr As FileStream = New FileStream(Form1.Pfad + Form1.Datensatz + ".EZG", FileMode.Open, FileAccess.ReadWrite)
    '        Dim StrRe As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
    '        Dim i, j, param As Integer

    '        param = 0
    '        Text = ""
    '        out = ""

    '        Do
    '            Text = StrRe.ReadLine.ToString
    '            i += 1
    '            TextBla = ""

    '            If i > 11 And i < 17 Then


    '                Dim strarray() As String = Text.Split("|"c)
    '                strarray(1) = " |" + strarray(1)
    '                For j = 1 To 10
    '                    TextBla = TextBla + strarray(j) + "|"
    '                Next

    '                param += 1
    '                KVG = (0.01 + (Form1.mypara(param, 1) * (2.0 - 0.01))).ToString
    '                param += 1
    '                K1 = (0.1 + (Form1.mypara(param, 1) * (10.0 - 0.1))).ToString
    '                param += 1
    '                K2 = (1.0 + (Form1.mypara(param, 1) * (20.0 - 1.0))).ToString
    '                param += 1
    '                Int = (1.0 + (Form1.mypara(param, 1) * (50.0 - 1.0))).ToString
    '                param += 1
    '                Bas = (100.0 + (Form1.mypara(param, 1) * (5000.0 - 1.0))).ToString
    '                param += 1
    '                Expo = (0.9 + (Form1.mypara(param, 1) * (1.1 - 0.9))).ToString
    '                'param += 1
    '                'Beta1 = (0.05 + (Form1.mypara(param, 1) * (0.9 - 0.05))).ToString
    '                'param += 1
    '                'Beta2 = (0.9 + (Form1.mypara(param, 1) * (1.1 - 0.9))).ToString


    '                Text = TextBla + " J  " + KVG.Substring(0, 5) + "  " + K1.Substring(0, 5) + "  " + K2.Substring(0, 5)
    '                Text = Text + "  " + Int.Substring(0, 5) + "  " + Bas.Substring(0, 5) + "  J  " + Expo.Substring(0, 4) + "|"
    '                Text = Text + strarray(12) + "|"

    '                'Console.Out.WriteLine(Text)

    '            End If
    '            Textnew = Textnew + Text + Environment.NewLine
    '        Loop Until StrRe.Peek() = -1

    '        StrRe.Close()
    '        FiStr.Close()

    '        Dim StrWri As StreamWriter = New StreamWriter(Form1.Pfad + Form1.Datensatz + ".EZG", False, System.Text.Encoding.GetEncoding("iso8859-1"))
    '        StrWri.Write(Textnew)
    '        StrWri.Close()

    '    Catch except As Exception
    '        MsgBox(except.Message, "Fehler in modifyVR", MsgBoxStyle.Exclamation)
    '    End Try



    'End Sub

    'Public Sub modifyBOA()
    '    Dim Text, kf, maxInf, TextBla, Textnew, out As String

    '    Try
    '        Dim FiStr As FileStream = New FileStream(Form1.Pfad + Form1.Datensatz + ".BOA", FileMode.Open, FileAccess.ReadWrite)
    '        Dim StrRe As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
    '        Dim i, j, param As Integer

    '        param = 0
    '        Text = ""
    '        out = ""

    '        Do
    '            Text = StrRe.ReadLine.ToString
    '            i += 1
    '            TextBla = ""

    '            If i = 11 Or i = 12 Or i = 14 Then

    '                Dim strarray() As String = Text.Split("|"c)
    '                strarray(1) = " |" + strarray(1)
    '                TextBla = TextBla + strarray(1) + "|"
    '                TextBla = TextBla + strarray(2).Substring(0, 25)

    '                param += 1
    '                kf = (0.01 + (Form1.mypara(param, 1) * (20.0 - 0.01))).ToString
    '                param += 1
    '                maxInf = (kf + (Form1.mypara(param, 1) * (40.0 - kf))).ToString

    '                Text = TextBla + kf.Substring(0, 5) + "  " + maxInf.Substring(0, 5) + "      0 |"
    '                Text = Text + strarray(3) + "|"

    '                'Console.Out.WriteLine(Text)

    '            End If
    '            Textnew = Textnew + Text + Environment.NewLine
    '        Loop Until StrRe.Peek() = -1

    '        StrRe.Close()
    '        FiStr.Close()

    '        Dim StrWri As StreamWriter = New StreamWriter(Form1.Pfad + Form1.Datensatz + ".BOA", False, System.Text.Encoding.GetEncoding("iso8859-1"))
    '        StrWri.Write(Textnew)
    '        StrWri.Close()

    '    Catch except As Exception
    '        MsgBox(except.Message, "Fehler in modifyVR", MsgBoxStyle.Exclamation)
    '    End Try



    'End Sub

End Module
