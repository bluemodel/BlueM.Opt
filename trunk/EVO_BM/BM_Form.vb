Imports System.IO
Imports System.Data.OleDb
Public Class BM_Form

    'Public Properties
    '------------------
    Public Datensatz As String                      'Name des zu simulierenden Datensatzes
    Public WorkDir As String                        'Arbeitsverzeichnis für das Blaue Modell
    Public BM_Exe As String                         'Pfad zu BlauesModell.exe

    'Optimierungsparameter
    Public Structure OptParameter       
        Public Bezeichnung As String                'Bezeichnung
        Public Einheit As String                    'Einheit
        Public Datei As String                      'Dateiendung der BM-Eingabedatei
        Public ZeileNr As Short                     'Zeile
        Public Sp1 As Short                         'Anfangsspalte
        Public Sp2 As Short                         'Endspalte
        Public Wert As Double                       'Parameterwert
        Public Min As Double                        'Minimum
        Public Max As Double                        'Maximum
        Public SKWert As Double                     'Skalierter Wert
        Public Sub deskalieren()                    'deskaliert SKWert und schreibt ihn in Wert
            Wert = SKWert * (Max - Min) + Min
        End Sub
        Public Sub skalieren()                      'skaliert Wert und schreibt ihn in SKWert
            SKWert = (Wert - Min) / (Max - Min)
        End Sub
    End Structure

    Public OptParameterListe() As OptParameter = {} 'Liste der Optimierungsparameter

    'Optimierungsziele
    Public Structure OptZiele
        Public Bezeichnung As String                'Bezeichnung
        Public ZielTyp As String                    'Gibt an ob es sich um einen Wert oder um eine Reihe handelt
        Public SpalteWel As String                  'Spalte der .wel Datei, die mit dem Ziel verglichen werden soll
        Public ZielFkt As String                    'Zielfunktion
        Public WertTyp As String                    'Gibt an welcher Wert aus der angegeben Spalte in der .wel Datei gewählt oder berechnet werden soll
        Public ZielWert As String                   'Der vorgegeben Zielwert
        Public SpalteZiel As String                 'Spalte der .wel Datei falls WelDatei
        Public ZielReihePfad As String              'Der Pfad zur Zielreihe
        Public ZielReihe(,) As Object               'Die Zielreihe
        Public TmpQualWert As Double                'Qualitätswerte der letzten Simulation werden hier zwischengespeichert 
    End Structure

    Public OptZieleListe() As OptZiele = {}         'Liste der Zielfunktionnen

    'Private Properties
    '-------------------
    Dim OptParameter_Pfad As String     'Pfad zur Datei mit den Optimierungsparametern (*.OPT)
    Dim OptZiele_Pfad As String         'Pfad zur Datei mit Einzelwerten für die Zielfunktionen (*.ZIE)
    'Dim OptZielReihe_Pfad As String     'Pfad zur Datei mit Reihen für die Zielfunktionen (*.ZIE)

    'DB
    Dim db As OleDb.OleDbConnection

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
                        Case "OptZiele"
                            OptZiele_Pfad = Configs(i, 1)
                            Me.TextBox_OptZiele_Pfad.Text = Me.OptZiele_Pfad
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

    'Optimierungsziele
    Private Sub Button_OptZielWert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_OptZielWert.Click
        Me.OpenFile_OptZiele.ShowDialog()
    End Sub

    Private Sub OpenFile_OptZielWert_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFile_OptZiele.FileOk

        'Pfad zur Datei auslesen
        Me.OptZiele_Pfad = Me.OpenFile_OptZiele.FileName

        'Pfad in Textbox schreiben
        Me.TextBox_OptZiele_Pfad.Text = Me.OptZiele_Pfad

    End Sub


    'Public Methoden
    '-------------------------------------

    'Optimierung initialisieren
    Public Sub Initialisierung()
        'Leere/Neue Ergebnisdatenbank in Arbeitsverzeichnis kopieren

        Dim ZielDatei As String = WorkDir & Datensatz & "_EVO.mdb"
        Dim overwrite As Boolean = True

        'bei bestehender Ergebnisdatenbank nachfragen, ob überschrieben werden soll
        If (File.Exists(ZielDatei)) Then
            Dim response As MsgBoxResult
            response = MsgBox("Bestehende Ergebnisdatenbank """ & ZielDatei & """ überschreiben?", MsgBoxStyle.YesNo, "Ergebnisdatenbank")
            If (response = MsgBoxResult.No) Then
                overwrite = False
            End If
        End If

        'Ergebnisdatenbank kopieren
        If (File.Exists(ZielDatei) = False Or overwrite = True) Then
            Try
                Dim currentDir As String = CurDir()     'sollte das /bin Verzeichnis von EVO_Anwendung sein
                ChDir("../../EVO_BM")                   'wechselt in das /EVO_BM Verzeichnis 
                My.Computer.FileSystem.CopyFile("EVO.mdb", ZielDatei, overwrite)
                ChDir(currentDir)                       'zurück in das Ausgangsverzeichnis wechseln
            Catch except As Exception
                MsgBox("Ergebnisdatenbank konnte nicht ins Arbeitsverzeichnis kopiert werden:" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
            End Try
        End If
    End Sub

    'Optimierungsparameter einlesen (*.OPT-Datei)
    Public Sub OptParameter_einlesen()
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

            ReDim OptParameterListe(AnzParam - 1)

            'Zurück zum Dateianfang und lesen
            FiStr.Seek(0, SeekOrigin.Begin)

            Dim array() As String
            Dim i As Integer = 0
            Do
                Zeile = StrRead.ReadLine.ToString()
                If (Zeile.StartsWith("*") = False) Then
                    array = Zeile.Split("|")
                    'Werte zuweisen
                    OptParameterListe(i).Bezeichnung = array(1).Trim()
                    OptParameterListe(i).Einheit = array(2).Trim()
                    OptParameterListe(i).Datei = array(3).Trim()
                    OptParameterListe(i).ZeileNr = Convert.ToInt16(array(4).Trim())
                    OptParameterListe(i).Sp1 = Convert.ToInt16(array(5).Trim())
                    OptParameterListe(i).Sp2 = Convert.ToInt16(array(6).Trim())
                    OptParameterListe(i).Wert = Convert.ToDouble(array(7).Trim())
                    OptParameterListe(i).Min = Convert.ToDouble(array(8).Trim())
                    OptParameterListe(i).Max = Convert.ToDouble(array(9).Trim())
                    i += 1
                End If
            Loop Until StrRead.Peek() = -1

        Catch except As Exception
            MsgBox(except.Message, MsgBoxStyle.Exclamation, "Fehler beim Lesen der Optimierungsparameter")
        End Try
    End Sub

    'skaliert alle OptParameter.Wert und schreibt sie in OptParameter.SKWert
    Public Sub OptParameter_skalieren()
        'Schleife über alle Parameter
        For i As Integer = 0 To OptParameterListe.GetUpperBound(0)
            Call OptParameterListe(i).skalieren()
        Next
    End Sub

    'deskaliert alle OptParameter.SKWert und schreibt sie in OptParameter.Wert
    Public Sub OptParameter_deskalieren()
        'Schleife über alle ParametDim tmpstr As Stringer
        For i As Integer = 0 To OptParameterListe.GetUpperBound(0)
            Call OptParameterListe(i).deskalieren()
        Next
    End Sub

    'Optimierungsziele - Werte - einlesen (*.zie-Datei)
    Public Sub OptZiele_einlesen()
        Dim AnzZiele As Integer = 0
        Dim IsOK As Boolean
        Dim tmpstr As String
        Dim i As Integer = 0
        Dim j As Integer = 0

        'Einlesen der Zielfunktionsdatei
        If OptZiele_Pfad Is Nothing Then
            Exit Sub
        Else

            Try
                Dim FiStr As FileStream = New FileStream(OptZiele_Pfad, FileMode.Open, IO.FileAccess.ReadWrite)
                Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

                Dim Zeile As String = ""

                'Anzahl der Zielfunktionen feststellen
                Do
                    Zeile = StrRead.ReadLine.ToString()
                    If (Zeile.StartsWith("*") = False) Then
                        AnzZiele += 1
                    End If
                Loop Until StrRead.Peek() = -1

                ReDim OptZieleListe(AnzZiele - 1)

                'Zurück zum Dateianfang und lesen
                FiStr.Seek(0, SeekOrigin.Begin)

                'Einlesen der Zeile und übergeben an die OptimierungsZiele Liste
                Dim ZeilenArray(7) As String

                Do
                    Zeile = StrRead.ReadLine.ToString()
                    If (Zeile.StartsWith("*") = False) Then
                        ZeilenArray = Zeile.Split("|")
                        'Werte zuweisen
                        OptZieleListe(i).Bezeichnung = ZeilenArray(0).Trim()
                        OptZieleListe(i).ZielTyp = ZeilenArray(1).Trim()
                        OptZieleListe(i).SpalteWel = ZeilenArray(2).Trim()
                        OptZieleListe(i).ZielFkt = ZeilenArray(3).Trim()
                        OptZieleListe(i).WertTyp = ZeilenArray(4).Trim()
                        OptZieleListe(i).ZielWert = ZeilenArray(5).Trim()
                        OptZieleListe(i).SpalteZiel = ZeilenArray(6).Trim()
                        OptZieleListe(i).ZielReihePfad = ZeilenArray(7).Trim()
                        i += 1
                    End If
                Loop Until StrRead.Peek() = -1

            Catch except As Exception
                MsgBox("Fehler beim lesen der Optimierungsziel-Datei (Werte)" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
            End Try
        End If

        'Falls mit Reihen verglichen werden soll werden hier die Reihen eingelesen
        For i = 0 To AnzZiele - 1
            If OptZieleListe(i).ZielTyp = "Reihe" Then

                tmpstr = OptZieleListe(i).ZielReihePfad.ToString.Substring(OptZieleListe(i).ZielReihePfad.ToString.LastIndexOf(".") + 1)
                If tmpstr = "wel" Then
                    IsOK = ReadWEL(OptZieleListe(i).ZielReihePfad.ToString, OptZieleListe(i).SpalteZiel, OptZieleListe(i).ZielReihe)
                ElseIf tmpstr = "zre" Then
                    IsOK = ReadZRE(OptZieleListe(i).ZielReihePfad.ToString, OptZieleListe(i).ZielReihe)
                End If

                If (IsOK = False) Then
                    'TODO: Fehlerbehandlung
                End If

            End If
        Next
    End Sub

    'Die Optimierungparameter in die BM-Eingabedateien schreiben
    Public Sub OptParameter_schreiben()
        Dim Wert As String
        Dim AnzZeil As Integer
        Dim j As Integer
        Dim Zeilenarray() As String
        Dim Zeile As String
        Dim StrLeft As String
        Dim StrRight As String
        Dim DateiPfad As String

        'Alle OptParameter durchlaufen
        For i As Integer = 0 To OptParameterListe.GetUpperBound(0)
            Try
                DateiPfad = WorkDir & Datensatz & "." & OptParameterListe(i).Datei
                'Datei öffnen
                Dim FiStr As FileStream = New FileStream(DateiPfad, FileMode.Open, IO.FileAccess.Read)
                Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

                'Anzahl der Zeilen feststellen
                AnzZeil = 0
                Do
                    Zeile = StrRead.ReadLine.ToString
                    AnzZeil += 1
                Loop Until StrRead.Peek() = -1

                ReDim Zeilenarray(AnzZeil - 1)

                'Datei komplett einlesen
                FiStr.Seek(0, SeekOrigin.Begin)
                For j = 0 To AnzZeil - 1
                    Zeilenarray(j) = StrRead.ReadLine.ToString
                Next

                StrRead.Close()
                FiStr.Close()

                'Zeile ändern
                Zeile = Zeilenarray(OptParameterListe(i).ZeileNr - 1)
                Dim Length As Short = OptParameterListe(i).Sp2 - OptParameterListe(i).Sp1
                StrLeft = Microsoft.VisualBasic.Left(Zeile, OptParameterListe(i).Sp1 - 1)
                StrRight = Microsoft.VisualBasic.Right(Zeile, Len(Zeile) - OptParameterListe(i).Sp2 + 1)
                'TODO: Parameter wird für erforderliche Stringlänge einfach abgeschnitten, sollte aber gerundet werden!
                Wert = OptParameterListe(i).Wert.ToString.Substring(0, Length)
                Zeilenarray(OptParameterListe(i).ZeileNr - 1) = StrLeft & Wert & StrRight

                'Alle Zeilen wieder in Datei schreiben
                Dim StrWrite As StreamWriter = New StreamWriter(DateiPfad, False, System.Text.Encoding.GetEncoding("iso8859-1"))
                For j = 0 To AnzZeil - 1
                    StrWrite.WriteLine(Zeilenarray(j))
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

    'Berechnung des Qualitätswerts (Zielwert)
    Public Function QualitaetsWert(ByVal ZielNr As Integer) As Double
        Dim i As Integer
        Dim SimReihe(,) As Object = {}
        Dim SimWert As Single

        Dim IsOK As Boolean
        IsOK = ReadWEL(WorkDir & Datensatz & ".wel", OptZieleListe(ZielNr).SpalteWel, SimReihe)
        If (IsOK = False) Then
            'TODO: Fehlerbehandlung
        End If

        '------------------------------------------
        'Ermittlung des Wertes Fall ZielTyp Reihe -
        '------------------------------------------

        If (OptZieleListe(ZielNr).ZielTyp = "Wert") Then
            Select Case OptZieleListe(ZielNr).WertTyp
                Case "MaxWert"
                    SimWert = 0
                    For i = 0 To SimReihe.GetUpperBound(0)
                        If SimReihe(i, 1) > SimWert Then
                            SimWert = SimReihe(i, 1)
                        End If
                    Next
                Case "MinWert"
                    SimWert = 999999999999999999
                    For i = 0 To SimReihe.GetUpperBound(0)
                        If SimReihe(i, 1) < SimWert Then
                            SimWert = SimReihe(i, 1)
                        End If
                    Next
                Case "Average"
                    SimWert = 0
                    For i = 0 To SimReihe.GetUpperBound(0)
                        SimWert += SimReihe(i, 1)
                    Next
                    SimWert = SimWert / SimReihe.GetLength(0)
                Case "AnfWert"
                    SimWert = SimReihe(0, 1)
                Case "EndWert"
                    SimWert = SimReihe(SimReihe.GetUpperBound(0), 1)
                Case Else
                    'TODO: Fehlerbehandlung
            End Select
        End If

        '--------------------------------------------------------
        'Berechnung der Zielfunktionen entweder Wert oder Reihe -
        '--------------------------------------------------------

        'Summe der Fehlerquadrate
        '----------------------------
        If OptZieleListe(ZielNr).ZielFkt = "AbQuad" And OptZieleListe(ZielNr).ZielTyp = "Wert" Then
            QualitaetsWert = (OptZieleListe(ZielNr).ZielWert - SimWert) * (OptZieleListe(ZielNr).ZielWert - SimWert)
        ElseIf OptZieleListe(ZielNr).ZielFkt = "AbQuad" And OptZieleListe(ZielNr).ZielTyp = "Reihe" Then
            For i = 0 To SimReihe.GetUpperBound(0)
                QualitaetsWert += (OptZieleListe(ZielNr).ZielReihe(i, 1) - SimReihe(i, 1)) * (OptZieleListe(ZielNr).ZielReihe(i, 1) - SimReihe(i, 1))
            Next

            'Summe der Fehler
            '--------------------------            
        ElseIf OptZieleListe(ZielNr).ZielFkt = "Diff" And OptZieleListe(ZielNr).ZielTyp = "Wert" Then
            QualitaetsWert = Math.Abs(OptZieleListe(ZielNr).ZielWert - SimWert)
        ElseIf OptZieleListe(ZielNr).ZielFkt = "Diff" And OptZieleListe(ZielNr).ZielTyp = "Reihe" Then
            For i = 0 To SimReihe.GetUpperBound(0)
                QualitaetsWert += Math.Abs(OptZieleListe(ZielNr).ZielReihe(i, 1) - SimReihe(i, 1))
            Next

            'Volumenfehler
            '--------------------------
        ElseIf OptZieleListe(ZielNr).ZielFkt = "Volf" And OptZieleListe(ZielNr).ZielTyp = "Wert" Then
            'TODO MSGBox Fehler in der Zielfunktionsdatei: Volumenfehler kann nicht mit einzelnen Werten gerechnet werden
        ElseIf OptZieleListe(ZielNr).ZielFkt = "Volf" And OptZieleListe(ZielNr).ZielTyp = "Reihe" Then
            'TODO: Volumenfehler rechnet noch nicht echtes Volumen, dazu ist Zeitschrittweite notwendig
            Dim VolSim As Double = 0
            Dim VolZiel As Double = 0
            For i = 0 To SimReihe.GetUpperBound(0)
                VolSim += SimReihe(i, 1)
                VolZiel += OptZieleListe(ZielNr).ZielReihe(i, 1)
            Next
            QualitaetsWert = Math.Abs(VolZiel - VolSim)
        Else
            'TODO:MsgBox Fehler in der Zielfunktionsdatei
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
        Dim Werte() As String = {}
        Dim SpalteNr As Integer = -1
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
            StrRead.ReadToEnd()
            ' Spaltenüberschriften vergleichen
            For j = 0 To Werte.GetUpperBound(0)
                If (Werte(j).Trim() = Spalte) Then
                    SpalteNr = j
                End If
            Next
            If (SpalteNr = -1) Then
                ReadWEL = False
                MsgBox("Konnte die Spalte """ & Spalte & """ in der WEL-Datei nicht finden!", MsgBoxStyle.Exclamation, "Fehler")
                Exit Function
            End If

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

    'Update der DB mit QWerten und OptParametern
    Public Function db_update(ByVal QWert_Bez As String, ByVal QWert() As Double, ByVal durchlauf As Integer, ByVal ipop As Short) As Boolean
        Call db_connect()

        Dim i, j As Integer

        Try
            Dim command As OleDbCommand = New OleDbCommand("", db)
            For i = 1 To QWert.GetUpperBound(0)
                'QWert schreiben 
                command.CommandText = "INSERT INTO QWerte (Bezeichnung, durchlauf, ipop, Qwert) VALUES ('" & QWert_Bez & "', " & durchlauf & ", " & ipop & ", " & QWert(i) & ")"
                command.ExecuteNonQuery()
                'ID des zuletzt geschriebenen QWerts holen
                command.CommandText = "SELECT @@IDENTITY AS id"
                Dim QWert_ID As Integer = command.ExecuteScalar()

                'Zugehörige OptParameter schreiben
                For j = 0 To OptParameterListe.GetUpperBound(0)
                    command.CommandText = "INSERT INTO OptParameter (Bezeichnung, Wert, QWert_ID) VALUES ('" & OptParameterListe(j).Bezeichnung & "', " & OptParameterListe(j).Wert & ", " & QWert_ID & ")"
                    command.ExecuteNonQuery()
                Next
            Next
        Catch except As Exception
            MsgBox("Fehler beim aktualisieren der Ergebnisdatenbank" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
        End Try

        Call db_disconnect()
    End Function

    Private Sub db_connect()
        Dim ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & WorkDir & Datensatz & "_EVO.mdb"
        db = New OleDb.OleDbConnection(ConnectionString)
        db.Open()
    End Sub

    Private Sub db_disconnect()
        db.Close()
    End Sub
End Class
