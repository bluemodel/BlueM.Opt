Imports System.IO
Public Class BM_Form

    'Public Properties
    '------------------
    Public Datensatz As String          'Name des zu simulierenden Datensatzes
    Public WorkDir As String            'Arbeitsverzeichnis für das Blaue Modell
    Public BM_Exe As String             'Pfad zu BlauesModell.exe
    Public Messung(,) As String         'Array mit den gemessenen Werten (Datum, Wert)
    Public Ergebnis() As Single         'Array mit den berechneten Werten
    'Optimierungsparameter
    Public OptParameter(,) As Object            'Array mit den Optimierungsparametern
    Public Const OPTPARA_BEZ As Integer = 0         'Bezeichnung
    Public Const OPTPARA_EINH As Integer = 1        'Einheit
    Public Const OPTPARA_DATEI As Integer = 2       'Datei
    Public Const OPTPARA_ZEILE As Integer = 3       'Zeile
    Public Const OPTPARA_SP1 As Integer = 4         'Anfangsspalte
    Public Const OPTPARA_SP2 As Integer = 5         'Endspalte
    Public Const OPTPARA_AWERT As Integer = 6       'Anfangswert
    Public Const OPTPARA_MIN As Integer = 7         'Minimum
    Public Const OPTPARA_MAX As Integer = 8         'Maximum
    Public Const OPTPARA_SKWERT As Integer = 9      'Skalierter Wert
    Public Const OPTPARA_LEN As Integer = 10    'Anzahl der für jeden Parameter gespeicherten Variablen

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

            ReDim OptParameter(AnzParam - 1, OPTPARA_LEN - 1)

            'Zurück zum Dateianfang und lesen
            FiStr.Seek(0, SeekOrigin.Begin)

            Dim Parameter(OPTPARA_LEN) As String
            Dim i As Integer = 0
            Dim j As Integer
            Do
                Zeile = StrRead.ReadLine.ToString()
                If (Zeile.StartsWith("*") = False) Then
                    Parameter = Zeile.Split("|")
                    For j = 0 To OPTPARA_LEN - 1
                        OptParameter(i, j) = Parameter(j + 1).Trim()
                    Next
                    'Typen verändern
                    OptParameter(i, OPTPARA_ZEILE) = Convert.ToInt16(OptParameter(i, OPTPARA_ZEILE))
                    OptParameter(i, OPTPARA_SP1) = Convert.ToInt16(OptParameter(i, OPTPARA_SP1))
                    OptParameter(i, OPTPARA_SP2) = Convert.ToInt16(OptParameter(i, OPTPARA_SP2))
                    OptParameter(i, OPTPARA_AWERT) = Convert.ToDouble(OptParameter(i, OPTPARA_AWERT))
                    OptParameter(i, OPTPARA_MIN) = Convert.ToDouble(OptParameter(i, OPTPARA_MIN))
                    OptParameter(i, OPTPARA_MAX) = Convert.ToDouble(OptParameter(i, OPTPARA_MAX))
                    i += 1
                End If
            Loop Until StrRead.Peek() = -1

        Catch except As Exception
            MsgBox(except.Message, MsgBoxStyle.Exclamation, "Fehler beim Lesen der Optimierungsparameter")
        End Try
    End Sub

    'skaliert AWERT und schreibt ihn in SKWERT
    Public Sub OptParameter_skalieren()
        Dim Min As Double
        Dim Max As Double
        Dim Param As Double
        'Schleife über alle Parameter
        For i As Integer = 0 To OptParameter.GetUpperBound(0)
            Param = OptParameter(i, OPTPARA_AWERT)
            Min = OptParameter(i, OPTPARA_MIN)
            Max = OptParameter(i, OPTPARA_MAX)
            OptParameter(i, OPTPARA_SKWERT) = (Param - Min) / (Max - Min)
        Next
    End Sub

    'deskaliert SKWERT und schreibt ihn in AWERT
    Public Sub OptParameter_deskalieren()
        Dim Min As Double
        Dim Max As Double
        Dim Param As Double
        For i As Integer = 0 To OptParameter.GetUpperBound(0)
            Param = OptParameter(i, OPTPARA_SKWERT)
            Min = OptParameter(i, OPTPARA_MIN)
            Max = OptParameter(i, OPTPARA_MAX)
            OptParameter(i, OPTPARA_AWERT) = Param * (Max - Min) + Min
        Next
    End Sub

    'Die vom Optimierungsalgorithmus mutierten Parameter werden geschrieben
    Public Sub Mutierte_Parameter_schreiben()
        Dim Parameter As String
        Dim AnzZeil As Integer
        Dim j As Integer
        Dim Datei() As String
        Dim Zeile As String
        Dim StrLeft As String
        Dim StrRight As String
        Dim DateiPfad As String

        'Alle Parameter durchlaufen
        For i As Integer = 0 To OptParameter.GetUpperBound(0)
            Try
                DateiPfad = WorkDir & Datensatz & "." & OptParameter(i, OPTPARA_DATEI)
                'Datei öffnen
                Dim FiStr As FileStream = New FileStream(DateiPfad, FileMode.Open, IO.FileAccess.Read)
                Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

                'Anzahl der Zeilen feststellen
                AnzZeil = 0
                Do
                    Zeile = StrRead.ReadLine.ToString
                    AnzZeil += 1
                Loop Until StrRead.Peek() = -1

                ReDim Datei(AnzZeil - 1)

                'Datei komplett einlesen
                FiStr.Seek(0, SeekOrigin.Begin)
                For j = 0 To AnzZeil - 1
                    Datei(j) = StrRead.ReadLine.ToString
                Next

                StrRead.Close()
                FiStr.Close()

                'Zeile ändern
                Zeile = Datei(OptParameter(i, OPTPARA_ZEILE) - 1)
                Dim Length As Short = OptParameter(i, OPTPARA_SP2) - OptParameter(i, OPTPARA_SP1)
                StrLeft = Microsoft.VisualBasic.Left(Zeile, OptParameter(i, OPTPARA_SP1) - 1)
                StrRight = Microsoft.VisualBasic.Right(Zeile, Len(Zeile) - OptParameter(i, OPTPARA_SP2) + 1)
                Parameter = OptParameter(i, OPTPARA_AWERT).ToString.Substring(0, Length)
                Datei(OptParameter(i, OPTPARA_ZEILE) - 1) = StrLeft & Parameter & StrRight

                'Alle Zeilen wieder in Datei schreiben
                Dim StrWrite As StreamWriter = New StreamWriter(DateiPfad, False, System.Text.Encoding.GetEncoding("iso8859-1"))
                For j = 0 To AnzZeil - 1
                    StrWrite.WriteLine(Datei(j))
                Next

                StrWrite.Close()

            Catch except As Exception
                MsgBox("Fehler beim Schreiben der Mutierten Parameter" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
            End Try
        Next

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

    Public Function ReadZRE(ByVal DateiPfad As String, ByRef ZRE(,) As Object) As Boolean

        'Lesen einer ZRE-Datei
        Dim AnzZeil As Integer = 0
        Dim j As Integer = 0
        Dim Zeile As String
        Const ZREHEaderLen As Integer = 4     'Die ersten 4 Zeilen der ZRE-Datei gehören zum Header
        ReadZRE = True

        Try
            Dim FiStr As FileStream = New FileStream(DateiPfad, FileMode.Open, IO.FileAccess.ReadWrite)
            Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

            'Anzahl der Zeilen feststellen
            Do
                Zeile = StrRead.ReadLine.ToString()
                AnzZeil += 1
            Loop Until StrRead.Peek() = -1

            ReDim ZRE(AnzZeil - ZREHEaderLen - 1, 1)

            'Zurück zum Dateianfang und lesen
            FiStr.Seek(0, SeekOrigin.Begin)

            For j = 0 To AnzZeil - 1
                Zeile = StrRead.ReadLine.ToString()
                If (j >= ZREHEaderLen) Then
                    ZRE(j - ZREHEaderLen, 0) = Zeile.Substring(0, 14)                       'Datum
                    ZRE(j - ZREHEaderLen, 1) = Convert.ToDouble(Zeile.Substring(15, 14))    'Wert
                End If
            Next

            StrRead.Close()
            FiStr.Close()

        Catch except As Exception
            MsgBox("Fehler beim lesen der ZRE-Datei" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
            ReadZRE = False
        End Try

    End Function

    Public Function ReadWEL(ByVal Dateipfad As String, ByVal Spalte As String, ByRef WEL(,) As Object) As Boolean

        'Lesen einer WEL-Datei (muss im CSV-Format mit Semikola vorliegen)
        Dim AnzZeil As Integer = 0
        Dim j As Integer = 0
        Dim Zeile As String
        Dim Werte() As String
        Dim SpalteNr As Integer
        Const WELHeaderLen As Integer = 3       'Die ersten 3 Zeilen der WEL-Datei gehören zum Header
        ReadWEL = True

        Try
            Dim FiStr As FileStream = New FileStream(Dateipfad, FileMode.Open, IO.FileAccess.ReadWrite)
            Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

            'Anzahl der Zeilen feststellen
            Do
                Zeile = StrRead.ReadLine.ToString
                AnzZeil += 1
            Loop Until StrRead.Peek() = -1

            ReDim WEL(AnzZeil - WELHeaderLen - 1, 1)

            'Position der zu lesenden Spalte bestimmen
            FiStr.Seek(0, SeekOrigin.Begin)
            ' Zeile mit den Spaltenüberschriften auslesen
            For j = 0 To 1
                Werte = StrRead.ReadLine.ToString.Split(";")
            Next
            ' Spaltenüberschriften vergleichen
            For j = 0 To Werte.GetUpperBound(0)
                If (Werte(j).Trim() = Spalte) Then
                    SpalteNr = j
                End If
            Next

            'Auf Anfang setzen und lesen
            FiStr.Seek(0, SeekOrigin.Begin)
            For j = 0 To AnzZeil - 1
                Werte = StrRead.ReadLine.ToString.Split(";")
                If (j >= WELHeaderLen) Then
                    WEL(j - WELHeaderLen, 0) = Werte(1)                             'Datum
                    WEL(j - WELHeaderLen, 1) = Convert.ToDouble(Werte(SpalteNr))    'Wert
                End If
            Next

            StrRead.Close()
            FiStr.Close()

        Catch except As Exception
            MsgBox("Fehler beim lesen der WEL-Datei" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
            ReadWEL = False
        End Try

    End Function


End Class
