Public Class BM_Form

    'Public Properties
    '-----------------
    Public Datensatz As String
    Public WorkDir As String
    Public Exe As String 'Pfad zu BlauesMOdell.exe

    'Private Methoden
    '----------------

    'Exe-Datei
    Private Sub Button_Exe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Exe.Click
        Me.OpenFile_EXE.ShowDialog()
    End Sub

    Private Sub OpenFile_EXE_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFile_EXE.FileOk
        'Pfad zur Exe auslesen
        Me.Exe = Me.OpenFile_EXE.FileName
        'Pfad in Textbox schreiben
        Me.TextBox_EXE.Clear()
        Me.TextBox_EXE.AppendText(Me.Exe)
    End Sub

    'Datensatz
    Private Sub Button_Datensatz_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Datensatz.Click
        Me.OpenFile_Datensatz.ShowDialog()
    End Sub

    Private Sub OpenFile_Datensatz_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFile_Datensatz.FileOk

        'kompletten Pfad zur ALL-Datei auslesen
        Dim Datensatz_tmp As String = Me.OpenFile_Datensatz.FileName

        'Pfad in Textbox schreiben
        Me.TextBox_Datensatz.Clear()
        Me.TextBox_Datensatz.AppendText(Datensatz_tmp)

        'Dateiname vom Ende abtrennen
        Datensatz = Datensatz_tmp.Substring(Datensatz_tmp.LastIndexOf("\") + 1)
        'Dateiendung entfernen
        Datensatz = Datensatz.Substring(0, Datensatz.Length - 4)
        'Arbeitsverzeichnis bestimmen
        WorkDir = Datensatz_tmp.Substring(0, Datensatz_tmp.LastIndexOf("\") + 1)

        'Datensatz einlesen aufrufen
        Call ReadSys()
    End Sub

    'Pegeldaten
    Private Sub Button_Pegel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Pegel.Click
        Me.OpenFile_Pegel.ShowDialog()
    End Sub

    Private Sub OpenFile_Pegel_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFile_Pegel.FileOk
        Me.TextBox_Pegel.Clear()
        Me.TextBox_Pegel.AppendText(Me.OpenFile_Pegel.FileName)
    End Sub

    'Optimierungsmodus
    Private Sub Radio_Autokalibrierung_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Radio_Autokalibrierung.CheckedChanged
        Me.Label_Pegel.Enabled = True
        Me.TextBox_Pegel.Enabled = True
        Me.Button_Pegel.Enabled = True
    End Sub

    Private Sub Radio_Optimierung_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Radio_Optimierung.CheckedChanged
        Me.Label_Pegel.Enabled = False
        Me.TextBox_Pegel.Enabled = False
        Me.Button_Pegel.Enabled = False
    End Sub

    'Public Methoden
    '-------------------------------------

    Public Sub Anfangsparameter_auslesen()

        'Select Case ComboXY_Text
        '    Case "Auto_Kalibrierung"

        '    Case "HW_Optimierung"

        'End Select

    End Sub

    Public Sub Messung_einlesen()

    End Sub

    Public Sub Parameter_schreiben()

        'Todo: Parameter m�ssen vor der Simulation geschrieben werden

    End Sub

    Public Sub launchBM()
        'starte Programm mit neuen Parametern
        Dim ProcID As Integer
        'Aktuelles Arbeitsverzeichnis feststellen
        Dim currentDir As String = CurDir()
        'zum gew�nschten Arbeitsverzeichnis navigieren
        ChDrive(WorkDir) 'nur n�tig falls Arbeitsverzeichnis und aktuelles Verzeichnis auf verschiedenen Laufwerken sind
        ChDir(WorkDir)
        'EXE aufrufen
        ProcID = Shell("""" & Exe & """ " & Datensatz, AppWinStyle.NormalFocus, True)
        'Arbeitsverzeichnis wieder zur�cksetzen (optional)
        ChDrive(currentDir)
        ChDir(currentDir)
    End Sub

    'Hier wird die Ergebnisdatei nach jeder Simulation ausgelesen
    Public Sub Ergebnisdatei_auslesen()

        'ToDo: lesen der Ergebnisdatei

    End Sub

    'Der Qualit�tswert wird durch Vergleich von Calculation Berechnet.
    'ToDo: Evtl. Cases f�r die Verschiedenen Berechnungsarten einbauen
    Public Function Qualitaetswert() As Double

        'ToDo: Berechnen des Qualit�tswertes

    End Function

    Public Sub ReadSys()
        Dim sysFile = WorkDir & Datensatz & ".SYS"
        'TODO: SYS-Datei einlesen
    End Sub



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

End Class
