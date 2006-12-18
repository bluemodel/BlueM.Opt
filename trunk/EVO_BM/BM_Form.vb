Imports System.IO
Public Class BM_Form

    'Public Properties
    '------------------
    Public Datensatz As String      'Name des zu simulierenden Datensatzes
    Public WorkDir As String        'Arbeitsverzeichnis für das Blaue Modell
    Public Exe As String            'Pfad zu BlauesModell.exe
    Public Elemente(,) As String    'Array mit allen im Datensatz enthaltenen Elementen (Beschreibung, Kennung)

    'Private Properties
    '-------------------
    Dim Zeitreihe As String         'Pfad zur ZRE-Zeitreihe (für Autokalibrierung)

    'Private Methoden
    '----------------

    'Initialisierung
    Private Sub BM_Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

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

        'Datensatz einlesen
        Call ReadSys()
    End Sub

    'Pegeldaten
    Private Sub Button_Pegel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Pegel.Click
        Me.OpenFile_Pegel.ShowDialog()
    End Sub

    Private Sub OpenFile_Pegel_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFile_Pegel.FileOk
        'Pfad zur Zeitreihe auslesen
        Me.Zeitreihe = Me.OpenFile_Pegel.FileName
        'Pfad in Textbox schreiben
        Me.TextBox_Pegel.Clear()
        Me.TextBox_Pegel.AppendText(Me.Zeitreihe)
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

    Public Sub Mutierte_Parameter_schreiben(ByRef Par(,) As Double)
        'Todo: Parameter müssen vor der Simulation geschrieben werden
        'Dim MyPara() As Double
        Dim Parameter As String
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim Zeile1, Zeile2, Zeile3 As String
        Zeile1 = ""
        Zeile2 = ""
        Zeile3 = ""
        Dim Text As String
        Dim StrLeft As String
        Dim StrRight As String
        Dim Datei() As String

        '---------------------------------------------------------------------
        'HACK: Zum schnelleren Arbeiten
        WorkDir = "D:\-03- AtWork #\BlauesModell_cons\Workfolder Speicher\"
        Exe = "D:\-03- AtWork #\BlauesModell_cons\Debug\blauesmodell_cons.exe"
        Datensatz = "TSIM"
        '---------------------------------------------------------------------

        Try
            Dim FiStr As FileStream = New FileStream(WorkDir + Datensatz + ".fkt", FileMode.Open, IO.FileAccess.ReadWrite)
            Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

            'Anzahl der Zeilen feststellen
            Do
                Text = StrRead.ReadLine.ToString
                i += 1
            Loop Until StrRead.Peek() = -1

            'Auf Anfang setzen und lesen
            FiStr.Seek(0, SeekOrigin.Begin)
            ReDim Datei(i)
            For j = 1 To i
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
            For j = 1 To i
                StrWrite.WriteLine(Datei(j))
            Next
            StrWrite.Close()

        Catch except As Exception
            MsgBox(except.Message, "Fehler beim Schreiben der Mutierten Parameter", MsgBoxStyle.Exclamation)
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
        ProcID = Shell("""" & Exe & """ " & Datensatz, AppWinStyle.NormalFocus, True)
        'Arbeitsverzeichnis wieder zurücksetzen (optional)
        ChDrive(currentDir)
        ChDir(currentDir)
    End Sub

    'Hier wird die Ergebnisdatei nach jeder Simulation ausgelesen
    Public Sub Ergebnisdatei_auslesen()

        'ToDo: lesen der Ergebnisdatei

    End Sub

    'Der Qualitätswert wird durch Vergleich von Calculation Berechnet.
    'ToDo: Evtl. Cases für die Verschiedenen Berechnungsarten einbauen
    Public Function Qualitaetswert() As Double

        'ToDo: Berechnen des Qualitätswertes

    End Function

    Public Sub ReadSys()
        Dim SysFile = WorkDir & Datensatz & ".SYS"
        'TODO: SYS-Datei einlesen
        Dim FiStr As FileStream = New FileStream(SysFile, FileMode.Open, FileAccess.Read)
        Dim StrRe As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        Dim Text As String
        Dim i As Integer = 0

        'Anzahl Elemente feststellen
        Do
            Text = StrRe.ReadLine.ToString
            If (Text.StartsWith("*") = False) Then
                i += 1
            End If
        Loop Until StrRe.Peek() = -1

        'Array neu dimensionieren
        ReDim Elemente(i - 1, 1)

        'auf Dateianfang zurücksetzen
        FiStr.Seek(0, SeekOrigin.Begin)

        i = 0

        Do
            Text = StrRe.ReadLine.ToString
            If (Text.StartsWith("*") = False) Then
                Elemente(i, 0) = Text.Substring(2, 23).Trim()
                Elemente(i, 1) = Text.Substring(28, 4).Trim()
                i += 1
            End If
        Loop Until StrRe.Peek() = -1

        StrRe.Close()
        FiStr.Close()
    End Sub

    Private Sub Button_Parameter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Parameter.Click

        'Elemente in ComboBox schreiben
        BM_Parameter.ComboBox_Elemente.BeginUpdate()

        Dim i As Integer
        For i = Elemente.GetLowerBound(0) To Elemente.GetUpperBound(0)
            BM_Parameter.ComboBox_Elemente.Items.Add(Elemente(i, 1))
        Next i

        BM_Parameter.ComboBox_Elemente.EndUpdate()

        'Dialog anzeigen
        BM_Parameter.ShowDialog()
    End Sub

End Class
