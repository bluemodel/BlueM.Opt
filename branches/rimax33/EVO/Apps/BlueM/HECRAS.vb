Imports System.IO
Imports System.Diagnostics

Public Class HECRAS

    Dim wave(100) As Double
    Dim Qupstream(1000), Qdownstream(1000), QPolder(1000) As Double
    Public SimStart As DateTime                          'Anfangsdatum der Simulation
    Public SimEnde As DateTime                           'Enddatum der Simulation
    Public SimDT As TimeSpan                             'Zeitschrittweite der Simulation
    Dim ProcID As Integer

    Private Declare Sub Sleep Lib "kernel32" (ByVal lngMilliseconds As Int32)
    'DllImport("kernel32.dll", SetLastError:=True)
    Public Function TerminateProcess(ByVal hProcess As IntPtr, ByVal uExitCode As System.UInt32) As Boolean

    End Function

    Private Declare Function CloseHandle Lib "kernel32" ( _
        ByVal hObject As Long) As Long

    Private Declare Function OpenProcess Lib "kernel32" ( _
        ByVal dwDesiredAccess As Long, _
        ByVal bInheritHandle As Long, _
        ByVal dwProcessId As Long) As Long

    Private Declare Function GetExitCodeProcess Lib "kernel32" ( _
        ByVal hProcess As Long, _
        ByVal lpExitCode As Long) As Long

    Const STILL_ACTIVE As Integer = &H103
    Const PROCESS_ALL_ACCESS As Integer = &H1F0FFF
    Const PROCESS_TERMINATE As Integer = &H1

    Public Sub main()

        '  Dim SimPfad As String = "D:\BM\1001c-Polder-X10p7-1ZF-1Opt\"

        readDateFromBlueM(Common.globalWorkDir + Common.globalDatensatz + ".ALL")

        getWaveFromBlueM(Common.globalWorkDir + Common.globalDatensatz + ".wel")

        writeWaveToHecRas("D:\HEC\HecRas-Auto-Ini\AutoIni.u02")

        writeWehrhoheToHecRas("D:\HEC\HecRas-Auto-Ini\AutoIni.g05")

        runHecRAS()

        readResultFromHecRas()

        writeWaveToBlueM(QPolder, "D:\BM\ZRE_GEV\AbgabePolder.zre")

    End Sub

    Private Sub readDateFromBlueM(ByVal Pfad1 As String)

        Dim SimStart_str As String = ""
        Dim SimEnde_str As String = ""
        Dim SimDT_str As String = ""
        Dim Ganglinie As String = ""
        Dim CSV_Format As String = ""

        'ALL-Datei öffnen
        '----------------
      
        Dim FiStr As FileStream = New FileStream(Pfad1, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        'Alle Zeilen durchlaufen
        Dim Zeile As String
        Do
            Zeile = StrRead.ReadLine.ToString()

            'Simulationszeitraum auslesen
            If (Zeile.StartsWith(" SimBeginn - SimEnde ............:")) Then
                SimStart_str = Zeile.Substring(35, 16)
                SimEnde_str = Zeile.Substring(54, 16)
            End If

            'Zeitschrittweite auslesen
            If (Zeile.StartsWith(" Zeitschrittlaenge [min] ........:")) Then
                SimDT_str = Zeile.Substring(35).Trim
            End If

            'Überprüfen ob die Ganglinien (.WEL Datei) ausgegeben wird
            If (Zeile.StartsWith(" Ganglinienausgabe ....... [J/N] :")) Then
                Ganglinie = Zeile.Substring(35).Trim
            End If

            'Überprüfen ob CSV Format eingeschaltet ist
            If (Zeile.StartsWith(" ... CSV-Format .......... [J/N] :")) Then
                CSV_Format = Zeile.Substring(35).Trim
            End If

        Loop Until StrRead.Peek() = -1

        'SimStart und SimEnde in echtes Datum konvertieren
        Me.SimStart = New DateTime(SimStart_str.Substring(6, 4), SimStart_str.Substring(3, 2), SimStart_str.Substring(0, 2), SimStart_str.Substring(11, 2), SimStart_str.Substring(14, 2), 0)
        Me.SimEnde = New DateTime(SimEnde_str.Substring(6, 4), SimEnde_str.Substring(3, 2), SimEnde_str.Substring(0, 2), SimEnde_str.Substring(11, 2), SimEnde_str.Substring(14, 2), 0)

        'Zeitschrittweite in echte Dauer konvertieren
        Me.SimDT = New TimeSpan(0, Convert.ToInt16(SimDT_str), 0)

        'Fehlermeldung Ganglinie nicht eingeschaltet
        If Ganglinie <> "J" Then
            Throw New Exception("Die Ganglinienausgabe (.WEL Datei) ist nicht eingeschaltet. Bitte in .ALL Datei unter 'Ganglinienausgabe' einschalten")
        End If

        'Fehlermeldung CSv Format nicht eingeschaltet
        If CSV_Format <> "J" Then
            Throw New Exception("Das CSV Format für die .WEL Datei ist nicht eingeschaltet. Bitte in .ALL unter '... CSV-Format' einschalten.")
        End If

        FiStr.Close()
        StrRead.Close()


    End Sub

    Public Sub getWaveFromBlueM(ByVal Pfad1 As String)
        Dim FiStr As FileStream = New FileStream(Pfad1, FileMode.Open, FileAccess.ReadWrite)
        Dim StrRe As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
        Dim zeile, kp As Integer
        Dim Text As String

        Try
            zeile = 0
            kp = 1

            Do
                Text = StrRe.ReadLine.ToString
                zeile += 1

                If zeile = 2 Then
                    Dim strarray() As String = Text.Split(";"c)
                    Do While strarray(kp).Trim <> "S204_1ZU"
                        kp = kp + 1
                    Loop
                End If
                'Console.Out.WriteLine("hallo")

                If zeile > 3 Then
                    Dim strarray() As String = Text.Split(";"c)
                    wave(zeile - 3) = CDbl(strarray(kp))
                    'Console.Out.WriteLine(wave(zeile - 3))
                End If

            Loop Until StrRe.Peek() = -1

            StrRe.Close()
            FiStr.Close()

        Catch except As Exception
            MsgBox(except.Message, "Fehler in getWave", MsgBoxStyle.Exclamation)
        End Try

    End Sub


    Public Sub writeWaveToHecRas(ByVal Pfad As String)
        Dim Text, tmp As String

        Try
            Dim FiStr As FileStream = New FileStream(Pfad, FileMode.Open, FileAccess.ReadWrite)
            Dim StrRe As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
            Dim i, j, k As Integer

            Text = ""
            k = 0

            For i = 1 To 9
                Text = Text + StrRe.ReadLine.ToString + Environment.NewLine
            Next i

            For i = 10 To 19
                tmp = StrRe.ReadLine.ToString
                For j = 1 To 10
                    k = k + 1
                    tmp = "        " + Math.Round(wave(k), 2).ToString
                    ' Console.Out.WriteLine(wave(k))
                    Text = Text + tmp.Substring(tmp.Length - 8, 8)
                Next j
                Text = Text + Environment.NewLine
                'Console.Out.WriteLine(Text)
            Next i

            For i = 20 To 28
                Text = Text + StrRe.ReadLine.ToString + Environment.NewLine
            Next i

            StrRe.Close()
            FiStr.Close()

            Dim StrWri As StreamWriter = New StreamWriter(Pfad, False, System.Text.Encoding.GetEncoding("iso8859-1"))
            StrWri.Write(Text)
            StrWri.Close()

        Catch except As Exception
            MsgBox(except.Message, "Fehler in writeWave", MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Public Sub runHecRAS()
        Dim mouse As New MouseMove
        Dim ownPC As Boolean = True


        ProcID = Shell("""C:\Programme\HEC\HEC-RAS\4.0\ras.exe"" D:\HEC\HecRas-Auto-Ini\AutoIni.prj", AppWinStyle.NormalFocus, False)

        Sleep(500)
        Cursor.Position = New Point(250, 50)
        mouse.LeftClick()
        '
        ' set mixed flow
        '
        Cursor.Position = New Point(190, 550)
        Sleep(500)
        mouse.LeftClick()
        '
        ' run
        '
        If ownPC Then Cursor.Position = New Point(250, 620)
        If Not ownPC Then Cursor.Position = New Point(250, 435)
        Sleep(1000)
        mouse.LeftClick()
        
        '
        ' say no to pre processing
        '
        'If ownPC Then Cursor.Position = New Point(670, 530)
        'If Not ownPC Then Cursor.Position = New Point(670, 545)
        'Sleep(1000)
        'mouse.LeftClick()
        '
        ' run, und warte 10 sek.
        '
        Sleep(10000)

        'If ownPC Then Cursor.Position = New Point(460, 50)
        'If Not ownPC Then
        Cursor.Position = New Point(490, 55)
        mouse.LeftClick()
        'Table
        If ownPC Then Cursor.Position = New Point(1380, 120)
        If Not ownPC Then Cursor.Position = New Point(100, 120)
        Sleep(500)
        mouse.LeftClick()
        'lateral Strukcture
        If ownPC Then Cursor.Position = New Point(1330, 30)
        If Not ownPC Then Cursor.Position = New Point(50, 30)
        Sleep(100)
        mouse.LeftClick()
        If ownPC Then Cursor.Position = New Point(1330, 100)
        If Not ownPC Then Cursor.Position = New Point(50, 100)
        Sleep(500)
        mouse.LeftClick()
        'File
        If ownPC Then Cursor.Position = New Point(1290, 30)
        If Not ownPC Then Cursor.Position = New Point(20, 30)
        Sleep(500)
        mouse.LeftClick()
        'Export
        If ownPC Then Cursor.Position = New Point(1290, 70)
        If Not ownPC Then Cursor.Position = New Point(20, 70)
        Sleep(500)
        mouse.LeftClick()

        'Exit hec
        
        Cursor.Position = New Point(710, 10)
        Sleep(250)
        mouse.LeftClick()

        Cursor.Position = New Point(650, 530)
        Sleep(250)
        mouse.LeftClick()

    End Sub

    Public Sub getResult()
        Dim Text As String

        Try
            Dim FiStr As FileStream = New FileStream("N:\Polder\HecRas-Auto-Ini\AutoIni.bco", FileMode.Open, FileAccess.ReadWrite)
            Dim StrRe As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
            Dim i, k, n As Integer

            Text = ""
            '
            ' upstream
            '
            k = 0
            '
            Do
                Text = StrRe.ReadLine.ToString

                If Text.Trim = "/Weisseritz Vereinigte/10040" Then
                    For i = 1 To 13
                        Text = StrRe.ReadLine.ToString
                    Next i
                End If

                If i = 14 Then
                    If Text.Trim = "Weisseritz Vereinigte" Then Exit Do
                    k = k + 1
                    If Trim(Text.Substring(Text.Length - 7, 7)) <> "" Then
                        Qupstream(k) = CDbl(Trim(Text.Substring(Text.Length - 7, 7)))
                    End If
                End If

            Loop Until StrRe.Peek() = -1
            '
            ' downstream
            '
            k = 0
            '
            Do
                Text = StrRe.ReadLine.ToString
                If Text.Trim = "/Weisseritz Vereinigte/9900.696" Then
                    For i = 1 To 13
                        Text = StrRe.ReadLine.ToString
                    Next i
                End If

                If i = 14 Then
                    If Text.Trim = "Weisseritz Vereinigte" Then Exit Do
                    k = k + 1
                    If Trim(Text.Substring(Text.Length - 7, 7)) <> "" Then
                        Qdownstream(k) = CDbl(Trim(Text.Substring(Text.Length - 7, 7)))
                    End If
                End If


            Loop Until StrRe.Peek() = -1
            '
            ' Polder zufluss bestimmen
            '
            For n = 1 To k
                QPolder(n) = Qupstream(n) - Qdownstream(n)
                Console.Out.WriteLine(Qupstream(n).ToString + Chr(9) + Qdownstream(n).ToString + Chr(9) + QPolder(n).ToString)
            Next n

            StrRe.Close()
            FiStr.Close()



        Catch except As Exception
            MsgBox(except.Message, "Fehler in getResult", MsgBoxStyle.Exclamation)
        End Try

    End Sub

    Public Sub readResultFromHecRas()
        Dim Text, TextTeile() As String
        Dim i As Integer
        Dim intLetzte As Integer = 0
        Dim j As Integer = 0
        Dim Qmax As Double

        Qmax = 0.0


        If (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text)) Then
            Text = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString()
            'Console.Out.WriteLine(Text)
            '
            ' Zerlegung in Zeilen
            '
            For i = 0 To Text.Length - 1
                If Text.Substring(i, 1) = Chr(10) Then
                    ReDim Preserve TextTeile(j)
                    TextTeile(j) = Text.Substring(intLetzte, i - intLetzte)
                    Dim strarray() As String = TextTeile(j).Split(Chr(9))
                    If (strarray(4) <> "") Then QPolder(j) = Math.Max(0.0, CDbl(strarray(4).trim))

                    If (QPolder(j) > Qmax) Then Qmax = QPolder(j)
                    j += 1
                    intLetzte = i + 1
                End If
            Next
            Console.Out.WriteLine(Qmax)

        Else
            Console.Out.WriteLine("The clipboad does not contain any text")
        End If

    End Sub

    Public Sub writeWaveToBlueM(ByVal Qab() As Double, ByVal PfadZRE As String)
        Dim i As Integer = 0
        Dim Text As String = ""
        Dim actDate, date2 As Date

        Text = "*ZRE" + vbCrLf
        Text += "ZRE-Format     m3/s" + vbCrLf
        Text += "1 1   1" + vbCrLf
        Text += Me.SimStart.ToString("yyyyMMdd HH:mm") + " " + Me.SimEnde.ToString("yyyyMMdd HH:mm") + vbCrLf

        actDate = Me.SimStart
        date2 = actDAte

        While date2 <= Me.SimEnde
            i += 1
            Text += date2.ToString("yyyyMMdd HH:mm") + " " + Math.Round(Qab(i), 3).ToString + vbCrLf
            date2 = date2.Add(Me.SimDT)
        End While

        Dim StrWri As StreamWriter = New StreamWriter(PfadZRE)
        StrWri.Write(Text)
        StrWri.Close()
    End Sub

    Public Sub writeWehrhoheToHecRas(ByVal Pfad As String)
        Dim Text, Zeile, strWert As String

        Try
            Dim FiStr As FileStream = New FileStream(Pfad, FileMode.Open, FileAccess.ReadWrite)
            Dim StrRe As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

            Text = ""
            '
            Do
                Zeile = StrRe.ReadLine.ToString()
                Text = Text + Zeile + Environment.NewLine
                If Zeile.StartsWith("Lateral Weir SE= 2") Then
                    Zeile = StrRe.ReadLine.ToString()
                    strWert = Common.weirHeight.ToString
                    If strWert.Length = 3 Then strWert = strWert + ".00"
                    If strWert.Length = 5 Then strWert = strWert + "0"
                    Text = Text + "   10020  " + strWert + "   10040  " + strWert + Environment.NewLine
                End If

            Loop Until StrRe.Peek() = -1


            StrRe.Close()
            FiStr.Close()

            Dim StrWri As StreamWriter = New StreamWriter(Pfad, False, System.Text.Encoding.GetEncoding("iso8859-1"))
            StrWri.Write(Text)
            StrWri.Close()



        Catch except As Exception
            MsgBox(except.Message, "Fehler in getResult", MsgBoxStyle.Exclamation)
        End Try

    End Sub

End Class
