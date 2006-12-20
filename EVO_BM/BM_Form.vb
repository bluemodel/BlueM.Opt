Imports System.IO
Public Class BM_Form

    'Public Properties
    '------------------
    Public Datensatz As String          'Name des zu simulierenden Datensatzes
    Public WorkDir As String            'Arbeitsverzeichnis für das Blaue Modell
    Public BM_Exe As String             'Pfad zu BlauesModell.exe
    Public Messung(,) As String         'Array mit den gemessenen Werten (Datum, Wert)
    Public Ergebnis() As Single         'Array mit den berechneten Werten
    Public OptParameter(,) As String    'Array mit den Optimierungsparametern

    'Private Properties
    '-------------------
    Dim OptParameter_Pfad As String     'Pfad zur Datei mit den Optimierungsparametern (*.OPT)
    Dim OptZiel_Pfad As String          'Pfad zur Datei mit den Zielfunktionen (*.ZIE)

    'Private Methoden
    '----------------

    'Initialisierung
    Private Sub BM_Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'EVO.ini lesen
        Call ReadEVOIni()
    End Sub

    'EVO.ini Datei einlesen
    Private Sub ReadEVOIni()
        If File.Exists("EVO.ini") Then
            Try
                'Datei einlesen
                Dim FiStr As FileStream = New FileStream("EVO.ini", FileMode.Open, IO.FileAccess.Read)
                Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

                Dim Configs(9, 1) As String
                Dim Line As String
                Dim Pairs() As String
                Dim i As Integer = 0
                Do
                    Line = StrRead.ReadLine.ToString()
                    If (Line.StartsWith("[") = False And Line.StartsWith(";") = False) Then
                        Pairs = Line.Split("=")
                        Configs(i, 0) = Pairs(0)
                        Configs(i, 1) = Pairs(1)
                        i += 1
                    End If
                Loop Until StrRead.Peek() = -1

                StrRead.Close()
                FiStr.Close()

                'Default-Werte setzen
                For i = 0 To Configs.GetUpperBound(0)
                    Select Case Configs(i, 0)
                        Case "BM_Exe"
                            BM_Exe = Configs(i, 1)
                            TextBox_EXE.Text = BM_Exe
                        Case "Datensatz"
                            'Dateiname vom Ende abtrennen
                            Datensatz = Configs(i, 1).Substring(Configs(i, 1).LastIndexOf("\") + 1)
                            'Dateiendung entfernen
                            Datensatz = Datensatz.Substring(0, Datensatz.Length - 4)
                            'Arbeitsverzeichnis bestimmen
                            WorkDir = Configs(i, 1).Substring(0, Configs(i, 1).LastIndexOf("\") + 1)
                            TextBox_Datensatz.Text = Configs(i, 1)
                        Case "OptParameter"
                            OptParameter_Pfad = Configs(i, 1)
                            TextBox_OptParameter_Pfad.Text = OptParameter_Pfad
                        Case "OptZiel"
                            OptZiel_Pfad = Configs(i, 1)
                            Me.TextBox_OptZiel_Pfad.Text = Me.OptZiel_Pfad
                        Case Else
                            'nix
                    End Select
                Next

            Catch except As Exception
                MsgBox(except.Message, MsgBoxStyle.Exclamation, "Fehler beim lesen der EVO.ini Datei")
            End Try
        End If

    End Sub

    'Exe-Datei
    Private Sub Button_Exe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Exe.Click
        Me.OpenFile_EXE.ShowDialog()
    End Sub

    Private Sub OpenFile_EXE_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFile_EXE.FileOk
        'Pfad zur Exe auslesen
        Me.BM_Exe = Me.OpenFile_EXE.FileName
        'Pfad in Textbox schreiben
        Me.TextBox_EXE.Clear()
        Me.TextBox_EXE.AppendText(Me.BM_Exe)
    End Sub

    'Datensatz
    Private Sub Button_Datensatz_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Datensatz.Click
        Me.OpenFile_Datensatz.ShowDialog()
    End Sub

    Private Sub OpenFile_Datensatz_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFile_Datensatz.FileOk

        'kompletten Pfad zur ALL-Datei auslesen
        Dim Datensatz_tmp As String = Me.OpenFile_Datensatz.FileName

        'Pfad in Textbox schreiben
        Me.TextBox_Datensatz.Text = Datensatz_tmp

        'Dateiname vom Ende abtrennen
        Datensatz = Datensatz_tmp.Substring(Datensatz_tmp.LastIndexOf("\") + 1)
        'Dateiendung entfernen
        Datensatz = Datensatz.Substring(0, Datensatz.Length - 4)
        'Arbeitsverzeichnis bestimmen
        WorkDir = Datensatz_tmp.Substring(0, Datensatz_tmp.LastIndexOf("\") + 1)

    End Sub

    'Optimierungsparameter
    Private Sub Button_OptParameter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_OptParameter.Click
        Me.OpenFile_OptParameter.ShowDialog()
    End Sub

    Private Sub OpenFile_OptParameter_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFile_OptParameter.FileOk
        OptParameter_Pfad = OpenFile_OptParameter.FileName()
        TextBox_OptParameter_Pfad.Text = OptParameter_Pfad
    End Sub

    'Zeitreihe
    Private Sub Button_OptZiel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_OptZiel.Click
        Me.OpenFile_OptZiel.ShowDialog()
    End Sub

    Private Sub OpenFile_OptZiel_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFile_OptZiel.FileOk

        'Pfad zur Zeitreihe auslesen
        Me.OptZiel_Pfad = Me.OpenFile_OptZiel.FileName

        'Pfad in Textbox schreiben
        Me.TextBox_OptZiel_Pfad.Clear()
        Me.TextBox_OptZiel_Pfad.AppendText(Me.OptZiel_Pfad)

    End Sub

    'Public Methoden
    '-------------------------------------

    'Optimierungsparameter einlesen (*.OPT-Datei)
    Public Sub ReadOptParameter()
        Try
            Dim FiStr As FileStream = New FileStream(OptParameter_Pfad, FileMode.Open, IO.FileAccess.ReadWrite)
            Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

            Dim Zeile As String
            Dim AnzParam As Integer = 0

            'Anzahl der Parameter feststellen
            Do
                Zeile = StrRead.ReadLine.ToString()
                If (Zeile.StartsWith("*") = False) Then
                    AnzParam += 1
                End If
            Loop Until StrRead.Peek() = -1

            ReDim OptParameter(AnzParam - 1, 8)

            'Zurück zum Dateianfang und lesen
            FiStr.Seek(0, SeekOrigin.Begin)

            Dim Parameter(8) As String
            Dim i As Integer = 0
            Dim j As Integer
            Do
                Zeile = StrRead.ReadLine.ToString()
                If (Zeile.StartsWith("*") = False) Then
                    Parameter = Zeile.Split("|")
                    For j = 0 To 8
                        OptParameter(i, j) = Parameter(j + 1)
                    Next
                    i += 1
                End If
            Loop Until StrRead.Peek() = -1

        Catch except As Exception
            MsgBox(except.Message, MsgBoxStyle.Exclamation, "Fehler beim Lesen der Optimierungsparameter")
        End Try
    End Sub

    Public Sub Parameter_deskalieren()
        'ToDo: Hier müssen die Parameter vor jeder Simulation deskaliert werden

    End Sub

    'Public Sub Messung_einlesen()
    '    'ToDo: Hier muss die Vergleichzeitreihe für die Messung eingelesen werden
    '    Dim AnzZeil As Integer = 0
    '    Dim j As Integer = 0
    '    Dim Datei() As String           'Array mit allen Zeilen der Datei
    '    Dim Zeile As String

    '    Dim FileExt As String = Zeitreihe.Substring(Zeitreihe.LastIndexOf(".") + 1)

    '    Select Case FileExt
    '        Case "zre"
    '            'Lesen einer ZRE-Datei
    '            Try
    '                Dim FiStr As FileStream = New FileStream(Zeitreihe, FileMode.Open, IO.FileAccess.ReadWrite)
    '                Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

    '                'Anzahl der Zeilen feststellen
    '                Do
    '                    Zeile = StrRead.ReadLine.ToString()
    '                    AnzZeil += 1
    '                Loop Until StrRead.Peek() = -1

    '                ReDim Messung(AnzZeil - 5, 1) 'Die ersten 4 Zeilen der ZRE-Datei gehören zum Header

    '                'Zurück zum Dateianfang und lesen
    '                FiStr.Seek(0, SeekOrigin.Begin)

    '                For j = 0 To AnzZeil - 1
    '                    Zeile = StrRead.ReadLine.ToString()
    '                    If (j >= 4) Then
    '                        Messung(j - 4, 0) = Zeile.Substring(0, 14)          'Datum
    '                        Messung(j - 4, 1) = Zeile.Substring(15, 14).Trim()  'Wert
    '                    End If
    '                Next

    '                StrRead.Close()
    '                FiStr.Close()

    '            Catch except As Exception
    '                MsgBox(except.Message, MsgBoxStyle.Exclamation, "Fehler beim lesen der ZRE-Datei")
    '            End Try

    '        Case "wel"
    '            'Lesen einer WEL-Datei
    '            'TODO: WEL-Datei lesen: zu lesende Spalte über UI (und/oder EVO.ini) definieren 
    '            Try
    '                Dim FiStr As FileStream = New FileStream(Zeitreihe, FileMode.Open, IO.FileAccess.ReadWrite)
    '                Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

    '                'Anzahl der Zeilen feststellen
    '                Do
    '                    Zeile = StrRead.ReadLine.ToString
    '                    AnzZeil += 1
    '                Loop Until StrRead.Peek() = -1

    '                'Auf Anfang setzen und lesen
    '                FiStr.Seek(0, SeekOrigin.Begin)
    '                ReDim Datei(AnzZeil)
    '                For j = 1 To AnzZeil
    '                    Datei(j) = StrRead.ReadLine.ToString
    '                Next

    '                StrRead.Close()
    '                FiStr.Close()

    '                'Werte an Messung übergeben
    '                ReDim Messung(AnzZeil - 3, 1)
    '                For j = 1 To AnzZeil - 3
    '                    'TODO: Datum in Messung(j, 0) speichern
    '                    Messung(j, 1) = Mid(Datei(j + 3), 333, 5)
    '                Next

    '            Catch except As Exception
    '                MsgBox(except.Message, MsgBoxStyle.Exclamation, "Fehler beim lesen der WEL-Datei")
    '            End Try

    '        Case Else
    '            'Zeitreihe ist weder WEL noch ZRE Datei
    '            MsgBox("Die Zeitreihe hat ein ungültiges Format", MsgBoxStyle.Exclamation, "Fehler beim Lesen der Zeitreihe")
    '    End Select

    'End Sub

    'Die vom Optimierungsalgorithmus mutierten Parameter werden geschrieben
    Public Sub Mutierte_Parameter_schreiben(ByRef Par(,) As Double)
        Dim Parameter As String
        Dim AnzZeil As Integer = 0
        Dim j As Integer = 0
        Dim Datei() As String
        Dim Text As String
        Dim StrLeft As String
        Dim StrRight As String

        Try
            Dim FiStr As FileStream = New FileStream(WorkDir + Datensatz + ".fkt", FileMode.Open, IO.FileAccess.ReadWrite)
            Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

            'Anzahl der Zeilen feststellen
            Do
                Text = StrRead.ReadLine.ToString
                AnzZeil += 1
            Loop Until StrRead.Peek() = -1

            'Auf Anfang setzen und lesen
            FiStr.Seek(0, SeekOrigin.Begin)
            ReDim Datei(AnzZeil)
            For j = 1 To AnzZeil
                Datei(j) = StrRead.ReadLine.ToString
            Next
            StrRead.Close()
            FiStr.Close()

            'Zeile im Datei Array ändern
            For j = 1 To 3
                StrLeft = Microsoft.VisualBasic.Left(Datei(j + 26), 60)
                StrRight = Microsoft.VisualBasic.Right(Datei(j + 26), 19)
                Parameter = (Par(j, 1)).ToString
                Parameter = Microsoft.VisualBasic.Left(Parameter, 8)
                Datei(j + 26) = StrLeft + Parameter + StrRight
            Next

            'Alle Zeilen in Datei schreiben
            Dim StrWrite As StreamWriter = New StreamWriter(WorkDir + Datensatz + ".fkt", False, System.Text.Encoding.GetEncoding("iso8859-1"))
            For j = 1 To AnzZeil
                StrWrite.WriteLine(Datei(j))
            Next
            StrWrite.Close()

        Catch except As Exception
            MsgBox(except.Message, MsgBoxStyle.Exclamation, "Fehler beim Schreiben der Mutierten Parameter")
        End Try

    End Sub

    Public Sub launchBM()
        'starte Programm mit neuen Parametern
        Dim ProcID As Integer
        'Aktuelles Arbeitsverzeichnis feststellen
        Dim currentDir As String = CurDir()
        'zum gewünschten Arbeitsverzeichnis navigieren
        ChDrive(WorkDir) 'nur nötig falls Arbeitsverzeichnis und aktuelles Verzeichnis auf verschiedenen Laufwerken sind
        ChDir(WorkDir)
        'EXE aufrufen
        ProcID = Shell("""" & BM_Exe & """ " & Datensatz, AppWinStyle.MinimizedNoFocus, True)
        'Arbeitsverzeichnis wieder zurücksetzen (optional)
        ChDrive(currentDir)
        ChDir(currentDir)
    End Sub

    'Hier wird die Ergebnisdatei nach jeder Simulation ausgelesen
    Public Sub Ergebnis_lesen()
        Dim AnzZeil As Integer = 0
        Dim j As Integer = 0
        Dim Datei() As String
        Dim Text As String

        Try
            Dim FiStr As FileStream = New FileStream(WorkDir + Datensatz + ".wel", FileMode.Open, IO.FileAccess.ReadWrite)
            Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

            'Anzahl der Zeilen feststellen
            Do
                Text = StrRead.ReadLine.ToString
                AnzZeil += 1
            Loop Until StrRead.Peek() = -1

            'Auf Anfang setzen und lesen
            FiStr.Seek(0, SeekOrigin.Begin)
            ReDim Datei(AnzZeil)
            For j = 1 To AnzZeil
                Datei(j) = StrRead.ReadLine.ToString
            Next

            StrRead.Close()
            FiStr.Close()

            'Werte an Ergebnis übergeben
            ReDim Ergebnis(AnzZeil - 3)
            For j = 1 To AnzZeil - 3
                Ergebnis(j) = Mid(Datei(j + 3), 333, 5)
            Next

        Catch except As Exception
            MsgBox(except.Message, MsgBoxStyle.Exclamation, "Fehler beim lesen der .WEL Datei")
        End Try

    End Sub

    'Der Qualitätswert wird durch Vergleich von Calculation Berechnet.
    'TODO: Array Messung ist vom Typ String - für Berechnung Konvertierung zu Double erforderlich!
    Public Function Qualitaetswert() As Double
        Dim CalcTyp As String = "Fehlerquadrate"
        Dim i As Integer

        If Messung.GetUpperBound(0) = Ergebnis.GetUpperBound(0) Then
            Select Case CalcTyp
                Case "Fehlerquadrate"
                    For i = 1 To Messung.Length - 1
                        Qualitaetswert = (Messung(i, 1) - Ergebnis(i)) * (Messung(i, 1) - Ergebnis(i))
                    Next
                Case "Letzter_Wert"
                    Qualitaetswert = (Messung(Messung.Length - 1, 1) - Ergebnis(Messung.Length - 1)) * (Messung(Messung.Length - 1, 1) - Ergebnis(Messung.Length - 1))

                Case "Maximaler_Wert"

                Case "Quadratischer_Maximaler_Wert"

                Case ""

            End Select
        Else
            MessageBox.Show("Die Anzahl der Zeitschritte zwischen Messung und Ergebnis stimmt nicht überein", "Zeitreihenfehler", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Function

End Class
