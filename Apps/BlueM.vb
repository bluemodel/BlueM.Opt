Imports System.IO
Imports System.Data.OleDb

'*******************************************************************************
'*******************************************************************************
'**** Klasse BlueM                                                        ****
'****                                                                       ****
'**** Funktionen zur Kontrolle des BlauenModells                            ****
'****                                                                       ****
'**** Christoph Huebner, Felix Froehlich                                    ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'****                                                       Dezember 2006   ****
'****                                                                       ****
'**** Letzte Änderung: März 2007                                            ****
'*******************************************************************************
'*******************************************************************************

Public Class BlueM


    '************************** Funktionen für ParaOpt **********************************
    '************************************************************************************

    Public Datensatz As String                           'Name des zu simulierenden Datensatzes
    Public WorkDir As String                             'Arbeitsverzeichnis für das Blaue Modell
    Public BM_Exe As String                              'Pfad zu BlauesModell.exe

    Public Const OptParameter_Ext As String = "OPT"      'Erweiterung der Datei mit den Optimierungsparametern (*.OPT)
    Public Const ModParameter_Ext As String = "MOD"      'Erweiterung der Datei mit den Modellparametern (*.MOD)
    Public Const OptZiele_Ext As String = "ZIE"          'Erweiterung der Datei mit den Zielfunktionen (*.ZIE)
    Public Const Combi_Ext As String = "CES"             'Erweiterung der Datei mit der Kombinatorik  (*.CES)

    '---------------------------------------------------------------------------------
    'Optimierungsparameter
    Public Structure OptParameter
        '*| Bezeichnung | Einh. | Anfangsw. | Min | Max |
        Public Bezeichnung As String                'Bezeichnung
        Public Einheit As String                    'Einheit
        Public Wert As Double                       'Parameterwert
        Public Min As Double                        'Minimum
        Public Max As Double                        'Maximum
        Public Property SKWert() As Double          'skalierter Wert (0 bis 1)
            Get
                SKWert = (Wert - Min) / (Max - Min)
                Exit Property
            End Get
            Set(ByVal value As Double)
                Wert = value * (Max - Min) + Min
            End Set
        End Property
    End Structure

    Public OptParameterListe() As OptParameter = {} 'Liste der Optimierungsparameter

    'ModellParameter
    Public Structure ModellParameter
        '*| OptParameter | Bezeichnung  | Einh. | Datei | Zeile | von | bis | Faktor |
        Public OptParameter As String               'Optimierungsparameter, aus dem dieser Modellparameter errechnet wird
        Public Bezeichnung As String                'Bezeichnung
        Public Einheit As String                    'Einheit
        Public Datei As String                      'Dateiendung der BM-Eingabedatei
        Public ZeileNr As Short                     'Zeile
        Public SpVon As Short                       'Anfangsspalte
        Public SpBis As Short                       'Endspalte
        Public Faktor As Double                     'Faktor fuer das Umrechnen zwischen OptParameter und ModellParameter
        Public Wert As Double                       'Aus OptParameter errechneter Wert
    End Structure

    Public ModellParameterListe() As ModellParameter = {} 'Liste der Modellparameter

    'Optimierungsziele
    '*| Bezeichnung   | ZielTyp  | Datei |  SimGröße | ZielFkt  | WertTyp  | ZielWert | ZielGröße  | PfadReihe
    Public Structure OptZiel
        Public Bezeichnung As String                'Bezeichnung
        Public ZielTyp As String                    'Gibt an ob es sich um einen Wert oder um eine Reihe handelt
        Public Datei As String                      'Die Ergebnisdatei, aus der das Simulationsergebnis ausgelesen werden soll [WEL, BIL, PRB]
        Public SimGr As String                      'Die Simulationsgröße, auf dessen Basis der Qualitätswert berechnet werden soll
        Public ZielFkt As String                    'Zielfunktion
        Public WertTyp As String                    'Gibt an wie der Wert, der mit dem Zielwert verglichen werden soll, aus dem Simulationsergebnis berechnet werden soll.
        Public ZielWert As String                   'Der vorgegeben Zielwert
        Public ZielReihePfad As String              'Der Pfad zur Zielreihe
        Public ZielGr As String                     'Spalte der .wel Datei falls ZielReihe .wel Datei ist
        Public ZielReihe(,) As Object               'Die Zielreihe
        Public QWertTmp As Double                   'Qualitätswert der letzten Simulation wird hier zwischengespeichert 
    End Structure

    Public OptZieleListe() As OptZiel = {}         'Liste der Zielfunktionnen

    ''Kombinatorik **************************************
    'Public Schaltung(2, 1) As Object
    'Public Maßnahme As Collection
    'Public Kombinatorik As Collection

    'DB
    Dim db As OleDb.OleDbConnection

    'BM-Einstellungen initialisieren
    Public Sub BM_Ini()
        'Optimierungsparameter einlesen
        Call OptParameter_einlesen()
        'ModellParameter einlesen
        Call ModellParameter_einlesen()
        'Zielfunktionen einlesen
        Call OptZiele_einlesen()
        'Datenbank vorbereiten
        Call db_prepare()
    End Sub

    'Ergebnisdatenbank vorbereiten
    Public Sub db_prepare()
        'Leere/Neue Ergebnisdatenbank in Arbeitsverzeichnis kopieren
        Dim ZielDatei As String = WorkDir & Datensatz & "_EVO.mdb"

        Try
            Dim currentDir As String = CurDir()     'sollte das /bin Verzeichnis von _Main sein
            ChDir("../../Apps")                     'wechselt in das /Apps Verzeichnis 
            My.Computer.FileSystem.CopyFile("EVO.mdb", ZielDatei, True)
            ChDir(currentDir)                       'zurück in das Ausgangsverzeichnis wechseln
        Catch except As Exception
            MsgBox("Ergebnisdatenbank konnte nicht ins Arbeitsverzeichnis kopiert werden:" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
        End Try

        'Tabellen anpassen
        Dim i As Integer
        Try
            Call db_connect()
            Dim command As OleDbCommand = New OleDbCommand("", db)
            'Tabelle 'QWerte'
            'Spalten festlegen:
            Dim fieldnames As String = ""
            For i = 0 To OptZieleListe.GetUpperBound(0)
                If (i > 0) Then
                    fieldnames &= ", "
                End If
                fieldnames &= "'" & OptZieleListe(i).Bezeichnung & "' DOUBLE"
            Next
            'Tabelle anlegen
            command.CommandText = "ALTER TABLE QWerte ADD COLUMN " & fieldnames
            command.ExecuteNonQuery()

            'Tabelle 'OptParameter'
            'Spalten festlegen:
            fieldnames = ""
            For i = 0 To OptParameterListe.GetUpperBound(0)
                If (i > 0) Then
                    fieldnames &= ", "
                End If
                fieldnames &= "'" & OptParameterListe(i).Bezeichnung & "' DOUBLE"
            Next
            'Tabelle anlegen
            command.CommandText = "ALTER TABLE OptParameter ADD COLUMN " & fieldnames
            command.ExecuteNonQuery()
            Call db_disconnect()
        Catch except As Exception
            MsgBox("Konnte Tabellen nicht anpassen:" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
        End Try

    End Sub

    'Optimierungsparameter einlesen
    Public Sub OptParameter_einlesen()

        Try
            Dim Datei As String = WorkDir & Datensatz & "." & OptParameter_Ext

            Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
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
                    OptParameterListe(i).Wert = Convert.ToDouble(array(3).Trim())
                    OptParameterListe(i).Min = Convert.ToDouble(array(4).Trim())
                    OptParameterListe(i).Max = Convert.ToDouble(array(5).Trim())
                    i += 1
                End If
            Loop Until StrRead.Peek() = -1

            StrRead.Close()
            FiStr.Close()

        Catch except As Exception
            MsgBox("Fehler beim Lesen der Optimierungsparameter:" & Chr(13) & Chr(10) & except.Message & Chr(13) & Chr(10) & "Ein Fehler könnten Leerzeichen in der letzten Zeile der Datei sein.", MsgBoxStyle.Exclamation, "Fehler")
        End Try
    End Sub

    'Modellparameter einlesen
    Public Sub ModellParameter_einlesen()
        Try
            Dim Datei As String = WorkDir & Datensatz & "." & ModParameter_Ext

            Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
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

            ReDim ModellParameterListe(AnzParam - 1)

            'Zurück zum Dateianfang und lesen
            FiStr.Seek(0, SeekOrigin.Begin)

            Dim array() As String
            Dim i As Integer = 0
            Do
                Zeile = StrRead.ReadLine.ToString()
                If (Zeile.StartsWith("*") = False) Then
                    array = Zeile.Split("|")
                    'Werte zuweisen
                    ModellParameterListe(i).OptParameter = array(1).Trim()
                    ModellParameterListe(i).Bezeichnung = array(2).Trim()
                    ModellParameterListe(i).Einheit = array(3).Trim()
                    ModellParameterListe(i).Datei = array(4).Trim()
                    ModellParameterListe(i).ZeileNr = Convert.ToInt16(array(5).Trim())
                    ModellParameterListe(i).SpVon = Convert.ToInt16(array(6).Trim())
                    ModellParameterListe(i).SpBis = Convert.ToInt16(array(7).Trim())
                    ModellParameterListe(i).Faktor = Convert.ToDouble(array(8).Trim())
                    i += 1
                End If
            Loop Until StrRead.Peek() = -1

            StrRead.Close()
            FiStr.Close()

        Catch except As Exception
            MsgBox(except.Message, MsgBoxStyle.Exclamation, "Fehler beim Lesen der Optimierungsparameter" & Chr(13) & Chr(10) & "Ein Fehler könnten Leerzeichen in der letzten Zeile der Datei sein")
        End Try

    End Sub

    'Optimierungsziele einlesen
    Public Sub OptZiele_einlesen()
        Dim AnzZiele As Integer = 0
        Dim IsOK As Boolean
        Dim ext As String
        Dim i As Integer = 0
        Dim j As Integer = 0

        Try
            Dim Datei As String = WorkDir & Datensatz & "." & OptZiele_Ext

            Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
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
            Dim ZeilenArray(9) As String

            Do
                Zeile = StrRead.ReadLine.ToString()
                If (Zeile.StartsWith("*") = False) Then
                    ZeilenArray = Zeile.Split("|")
                    'Werte zuweisen
                    OptZieleListe(i).Bezeichnung = ZeilenArray(1).Trim()
                    OptZieleListe(i).ZielTyp = ZeilenArray(2).Trim()
                    OptZieleListe(i).Datei = ZeilenArray(3).Trim()
                    OptZieleListe(i).SimGr = ZeilenArray(4).Trim()
                    OptZieleListe(i).ZielFkt = ZeilenArray(5).Trim()
                    OptZieleListe(i).WertTyp = ZeilenArray(6).Trim()
                    OptZieleListe(i).ZielWert = ZeilenArray(7).Trim()
                    OptZieleListe(i).ZielGr = ZeilenArray(8).Trim()
                    OptZieleListe(i).ZielReihePfad = ZeilenArray(9).Trim()
                    i += 1
                End If
            Loop Until StrRead.Peek() = -1

            StrRead.Close()
            FiStr.Close()

        Catch except As Exception
            MsgBox("Fehler beim lesen der Optimierungsziel-Datei:" & Chr(13) & Chr(10) & except.Message & Chr(13) & Chr(10) & "Ein Fehler könnten Leerzeichen in der letzten Zeile der Datei sein", MsgBoxStyle.Exclamation, "Fehler")
        End Try

        'Falls mit Reihen verglichen werden soll werden hier die Reihen eingelesen
        For i = 0 To AnzZiele - 1
            If OptZieleListe(i).ZielTyp = "Reihe" Then

                'Dateiendung der Zielreihe bestimmen
                ext = OptZieleListe(i).ZielReihePfad.Substring(OptZieleListe(i).ZielReihePfad.LastIndexOf(".") + 1)
                Select Case (ext.ToUpper)
                    Case "WEL"
                        IsOK = ReadWEL(OptZieleListe(i).ZielReihePfad, OptZieleListe(i).ZielGr, OptZieleListe(i).ZielReihe)
                    Case "ZRE"
                        IsOK = ReadZRE(OptZieleListe(i).ZielReihePfad, OptZieleListe(i).ZielReihe)
                    Case "PRB"
                        IsOK = ReadPRB(OptZieleListe(i).ZielReihePfad, OptZieleListe(i).ZielGr, OptZieleListe(i).ZielReihe)
                    Case Else
                        IsOK = False
                End Select

                If (IsOK = False) Then
                    MsgBox("Fehler beim einlesen der Zielreihe '" & OptZieleListe(i).ZielReihePfad & "'" & Chr(13) & Chr(10) & "Ein Fehler könnten Leerzeichen in der letzten Zeile der Datei sein", MsgBoxStyle.Exclamation, "Fehler")
                End If

            End If
        Next
    End Sub

    Private Sub db_connect()
        Dim ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & WorkDir & Datensatz & "_EVO.mdb"
        db = New OleDb.OleDbConnection(ConnectionString)
        db.Open()
    End Sub

    Private Sub db_disconnect()
        db.Close()
    End Sub

    'ModellParameter werden aus OptParametern errechnet
    Public Sub OptParameter_to_ModellParameter()
        Dim i As Integer
        Dim j As Integer
        For i = 0 To ModellParameterListe.GetUpperBound(0)
            For j = 0 To OptParameterListe.GetUpperBound(0)
                If ModellParameterListe(i).OptParameter = OptParameterListe(j).Bezeichnung Then
                    ModellParameterListe(i).Wert = OptParameterListe(j).Wert * ModellParameterListe(i).Faktor
                End If
            Next
        Next
    End Sub

    'Die ModellParameter in die BM-Eingabedateien schreiben
    Public Sub ModellParameter_schreiben()
        Dim Wert As String
        Dim AnzZeil As Integer
        Dim j As Integer
        Dim Zeilenarray() As String
        Dim Zeile As String
        Dim StrLeft As String
        Dim StrRight As String
        Dim DateiPfad As String

        'ModellParameter aus OptParametern kalkulieren()
        Call OptParameter_to_ModellParameter()

        'Alle ModellParameter durchlaufen
        For i As Integer = 0 To ModellParameterListe.GetUpperBound(0)
            Try
                DateiPfad = WorkDir & Datensatz & "." & ModellParameterListe(i).Datei
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
                Zeile = Zeilenarray(ModellParameterListe(i).ZeileNr - 1)
                Dim Length As Short = ModellParameterListe(i).SpBis - ModellParameterListe(i).SpVon
                StrLeft = Microsoft.VisualBasic.Left(Zeile, ModellParameterListe(i).SpVon - 1)
                StrRight = Microsoft.VisualBasic.Right(Zeile, Len(Zeile) - ModellParameterListe(i).SpBis + 1)

                Wert = ModellParameterListe(i).Wert.ToString()
                If (Wert.Length > Length) Then
                    'TODO: Parameter wird für erforderliche Stringlänge einfach abgeschnitten, sollte aber gerundet werden!
                    Wert = Wert.Substring(0, Length)
                Else
                    Wert = Wert.PadLeft(Length)
                End If
                Zeilenarray(ModellParameterListe(i).ZeileNr - 1) = StrLeft & Wert & StrRight

                'Alle Zeilen wieder in Datei schreiben
                Dim StrWrite As StreamWriter = New StreamWriter(DateiPfad, False, System.Text.Encoding.GetEncoding("iso8859-1"))
                For j = 0 To AnzZeil - 1
                    StrWrite.WriteLine(Zeilenarray(j))
                Next

                StrWrite.Close()
                FiStr.Close()

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

        'überprüfen, ob Simulation erfolgreich
        If (File.Exists(WorkDir & "$FEHL.TMP")) Then

            'Fehler aufgetreten
            Dim DateiInhalt As String = ""

            Try
                Dim FiStr As FileStream = New FileStream(WorkDir & "$fehl.tmp", FileMode.Open, IO.FileAccess.Read)
                Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

                Do
                    DateiInhalt = DateiInhalt & Chr(13) & Chr(10) & StrRead.ReadLine.ToString
                Loop Until StrRead.Peek() = -1

                MsgBox("Das BlaueModell hat einen Fehler zurückgegeben:" & Chr(13) & Chr(10) & DateiInhalt, MsgBoxStyle.Exclamation, "Simulationsfehler")

            Catch except As Exception
                MsgBox("Konnte Datei ""$FEHL.TMP"" nicht lesen!" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
            End Try

        End If

    End Sub

    'Berechnung des Qualitätswerts (Zielwert)
    Public Function QualitaetsWert_berechnen(ByVal ZielNr As Integer) As Double

        Dim QWert As Double = 0
        Dim OptZiel As OptZiel = OptZieleListe(ZielNr)

        If (OptZiel.ZielTyp = "EcoFlood") Then
            Dim i As Integer
            'QualitaetsWert_berechnen = 0
            For i = 0 To OptParameterListe.GetUpperBound(0)
                QWert = QWert + (OptParameterListe(i).Wert * OptParameterListe(i).Wert) / 1000
            Next
            'QWert = (1 / QWert) * 100
        Else

            Dim i As Integer
            Dim SimReihe(,) As Object = {}
            Dim SimWert As Single
            Dim IsOK As Boolean

            Select Case OptZiel.Datei
                Case "WEL"
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                    'Qualitätswert aus WEL-Datei
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Simulationsergebnis auslesen
                    IsOK = ReadWEL(WorkDir & Datensatz & ".wel", OptZiel.SimGr, SimReihe)

                    '--------------------------------------------------------
                    'bei Werten zuerst Wert aus Simulationsergebnis berechnen
                    '--------------------------------------------------------
                    If (OptZiel.ZielTyp = "Wert") Then

                        Select Case OptZiel.WertTyp

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
                    'Berechnung des Qualitätswerts
                    '--------------------------------------------------------
                    Select Case OptZiel.ZielFkt

                        Case "AbQuad"
                            'Summe der Fehlerquadrate
                            '------------------------
                            Select Case OptZiel.ZielTyp
                                Case "Wert"
                                    QWert = (OptZiel.ZielWert - SimWert) * (OptZiel.ZielWert - SimWert)

                                Case "Reihe"
                                    For i = 0 To SimReihe.GetUpperBound(0)
                                        QWert += (OptZiel.ZielReihe(i, 1) - SimReihe(i, 1)) * (OptZiel.ZielReihe(i, 1) - SimReihe(i, 1))
                                    Next
                            End Select
                            '------------------------

                        Case "Diff"
                            'Summe der Fehler
                            '------------------------
                            Select Case OptZiel.ZielTyp
                                Case "Wert"
                                    QWert = Math.Abs(OptZiel.ZielWert - SimWert)

                                Case "Reihe"
                                    For i = 0 To SimReihe.GetUpperBound(0)
                                        QWert += Math.Abs(OptZiel.ZielReihe(i, 1) - SimReihe(i, 1))
                                    Next
                            End Select
                            '------------------------

                        Case "Volf"
                            'Volumenfehler
                            '--------------------------
                            Select Case OptZiel.ZielTyp
                                Case "Wert"
                                    'TODO: MSGBox Fehler in der Zielfunktionsdatei: Volumenfehler kann nicht mit einzelnen Werten gerechnet werden

                                Case "Reihe"
                                    'TODO: Volumenfehler rechnet noch nicht echtes Volumen, dazu ist Zeitschrittweite notwendig
                                    Dim VolSim As Double = 0
                                    Dim VolZiel As Double = 0
                                    For i = 0 To SimReihe.GetUpperBound(0)
                                        VolSim += SimReihe(i, 1)
                                        VolZiel += OptZiel.ZielReihe(i, 1)
                                    Next
                                    QWert = Math.Abs(VolZiel - VolSim)
                            End Select
                            '------------------------

                        Case Else
                            'TODO: MsgBox Fehler in der Zielfunktionsdatei
                    End Select

                Case "PRB"
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                    'Qualitätswert aus PRB-Datei
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Simulationsergebnis auslesen
                    IsOK = ReadPRB(WorkDir & Datensatz & ".PRB", OptZiel.SimGr, SimReihe)

                    '--------------------------------------------------------
                    'Berechnung des Qualitätswerts
                    '--------------------------------------------------------
                    'Diff
                    '----
                    'Überflüssige Stützstellen (P) entfernen
                    'Anzahl Stützstellen bestimmen
                    Dim stuetz As Integer = 0
                    Dim P_vorher As Double = -99
                    For i = 0 To SimReihe.GetUpperBound(0)
                        If (i = 0 Or Not SimReihe(i, 1) = P_vorher) Then
                            stuetz += 1
                            P_vorher = SimReihe(i, 1)
                        End If
                    Next
                    'Werte in neues Array schreiben
                    Dim PRBtmp(stuetz, 1) As Object
                    stuetz = 0
                    For i = 0 To SimReihe.GetUpperBound(0)
                        If (i = 0 Or Not SimReihe(i, 1) = P_vorher) Then
                            PRBtmp(stuetz, 0) = SimReihe(i, 0)
                            PRBtmp(stuetz, 1) = SimReihe(i, 1)
                            P_vorher = SimReihe(i, 1)
                            stuetz += 1
                        End If
                    Next
                    'Reihe um eine Stützstelle erweitern
                    'PRBtmp(stuetz, 0) = PRBtmp(stuetz - 1, 0)
                    'PRBtmp(stuetz, 1) = PRBtmp(stuetz - 1, 1)

                    'An Stützstellen der ZielReihe interpolieren
                    Dim PRBintp(OptZiel.ZielReihe.GetUpperBound(0), 1) As Object
                    Dim j As Integer
                    For i = 0 To OptZiel.ZielReihe.GetUpperBound(0)
                        'zugehörige Lamelle in SimReihe finden
                        j = 0
                        Do While (PRBtmp(j, 1) < OptZiel.ZielReihe(i, 1))
                            j += 1
                        Loop
                        'interpolieren
                        PRBintp(i, 0) = (PRBtmp(j + 1, 0) - PRBtmp(j, 0)) / (PRBtmp(j + 1, 1) - PRBtmp(j, 1)) * (OptZiel.ZielReihe(i, 1) - PRBtmp(j, 1)) + PRBtmp(j, 0)
                        PRBintp(i, 1) = OptZiel.ZielReihe(i, 1)
                    Next

                    For i = 0 To OptZiel.ZielReihe.GetUpperBound(0)
                        QWert += Math.Abs(OptZiel.ZielReihe(i, 0) - PRBintp(i, 0))
                    Next

                Case Else
                    IsOK = False
            End Select

            If (IsOK = False) Then
                'TODO: Fehlerbehandlung
            End If

        End If

        QualitaetsWert_berechnen = QWert

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
                    'Datum
                    ZRE(j - ZREHEaderLen, 0) = New System.DateTime(Zeile.Substring(0, 4), Zeile.Substring(4, 2), Zeile.Substring(6, 2), Zeile.Substring(9, 2), Zeile.Substring(12, 2), 0, New System.Globalization.GregorianCalendar())
                    'Wert
                    ZRE(j - ZREHEaderLen, 1) = Convert.ToDouble(Zeile.Substring(15, 14))
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
                If Werte(j).Trim() = Spalte Then
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
                    'Datum
                    WEL(j - WELHeaderLen, 0) = New System.DateTime(Werte(1).Substring(6, 4), Werte(1).Substring(3, 2), Werte(1).Substring(0, 2), Werte(1).Substring(11, 2), Werte(1).Substring(14, 2), 0, New System.Globalization.GregorianCalendar())
                    'Wert
                    WEL(j - WELHeaderLen, 1) = Convert.ToDouble(Werte(SpalteNr))
                End If
            Next

            StrRead.Close()
            FiStr.Close()

        Catch except As Exception
            MsgBox("Fehler beim lesen der WEL-Datei" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
            ReadWEL = False
        End Try

    End Function

    Public Function ReadPRB(ByVal DateiPfad As String, ByVal ZielGr As String, ByRef PRB(,) As Object) As Boolean

        'Lesen einer PRB-Datei
        Dim ZeileStart As Integer = 0
        Dim AnzZeil = 26                   'Anzahl der Zeilen ist immer 26, definiert durch MAXSTZ in BM
        Dim j As Integer = 0
        Dim Zeile As String
        ReadPRB = True

        Try
            Dim FiStr As FileStream = New FileStream(DateiPfad, FileMode.Open, IO.FileAccess.ReadWrite)
            Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

            'Array redimensionieren
            ReDim PRB(AnzZeil - 1, 1)

            'Anfangszeile suchen
            Do
                Zeile = StrRead.ReadLine.ToString
                If (Zeile.Contains("+ Wahrscheinlichkeitskeitsverteilung: " & ZielGr)) Then
                    Exit Do
                End If
            Loop Until StrRead.Peek() = -1

            'Zeile mit Spaltenüberschriften überspringen
            Zeile = StrRead.ReadLine.ToString

            For j = 0 To AnzZeil - 1
                Zeile = StrRead.ReadLine.ToString()
                PRB(j, 0) = Convert.ToDouble(Zeile.Substring(2, 10))        'X-Wert
                PRB(j, 1) = Convert.ToDouble(Zeile.Substring(13, 8))        'P(Jahr)
            Next

            StrRead.Close()
            FiStr.Close()

            'Überflüssige Stützstellen (P) entfernen
            'Anzahl Stützstellen bestimmen
            Dim stuetz As Integer = 0
            Dim P_vorher As Double = -99
            For j = 0 To PRB.GetUpperBound(0)
                If (j = 0 Or Not PRB(j, 1) = P_vorher) Then
                    stuetz += 1
                    P_vorher = PRB(j, 1)
                End If
            Next
            'Werte in neues Array schreiben
            Dim PRBtmp(stuetz - 1, 1) As Object
            stuetz = 0
            For j = 0 To PRB.GetUpperBound(0)
                If (j = 0 Or Not PRB(j, 1) = P_vorher) Then
                    PRBtmp(stuetz, 0) = PRB(j, 0)
                    PRBtmp(stuetz, 1) = PRB(j, 1)
                    P_vorher = PRB(j, 1)
                    stuetz += 1
                End If
            Next
            PRB = PRBtmp

        Catch except As Exception
            MsgBox("Fehler beim lesen der PRB-Datei:" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
            ReadPRB = False
        End Try

    End Function

    'Update der DB mit QWerten und OptParametern
    Public Function db_update(ByVal durchlauf As Integer, ByVal ipop As Short) As Boolean
        Call db_connect()

        Dim i As Integer

        Try
            Dim command As OleDbCommand = New OleDbCommand("", db)
            'QWert schreiben 
            'Spalten der Tabelle 'Qwerte' bestimmen:
            Dim fieldnames As String = ""
            Dim fieldvalues As String = ""
            For i = 0 To OptZieleListe.GetUpperBound(0)
                fieldnames &= ", '" & OptZieleListe(i).Bezeichnung & "'"
                fieldvalues &= ", " & OptZieleListe(i).QWertTmp
            Next
            command.CommandText = "INSERT INTO QWerte (durchlauf, ipop " & fieldnames & ") VALUES (" & durchlauf & ", " & ipop & fieldvalues & ")"
            command.ExecuteNonQuery()
            'ID des zuletzt geschriebenen QWerts holen
            command.CommandText = "SELECT @@IDENTITY AS ID"
            Dim QWert_ID As Integer = command.ExecuteScalar()

            'Zugehörige OptParameter schreiben
            fieldnames = ""
            fieldvalues = ""
            For i = 0 To OptParameterListe.GetUpperBound(0)
                fieldnames &= ", '" & OptParameterListe(i).Bezeichnung & "'"
                fieldvalues &= ", " & OptParameterListe(i).Wert
            Next
            command.CommandText = "INSERT INTO OptParameter (QWert_ID" & fieldnames & ") VALUES (" & QWert_ID & fieldvalues & ")"
            command.ExecuteNonQuery()
        Catch except As Exception
            MsgBox("Fehler beim schreiben in die Ergebnisdatenbank" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
        End Try

        Call db_disconnect()
    End Function

    '********************* TeeChart Initialisierung für das BlaueModell *****************

    Public Sub TeeChartInitialise_SO_BlauesModell(ByVal n_Populationen As Integer, ByVal n_Kalkulationen As Integer, ByRef TChart1 As Steema.TeeChart.TChart)
        'Dim Anzahl_Kalkulationen As Integer
        'Dim Populationen As Short
        Dim i As Short

        With TChart1
            .Clear()
            .Header.Text = "BlauesModell"
            .Aspect.View3D = False
            .Legend.Visible = False

            'Series(0): Anfangswert
            Dim Point0 As New Steema.TeeChart.Styles.Points(.Chart)
            Point0.Title = "Anfangswert"
            Point0.Color = System.Drawing.Color.Red
            Point0.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point0.Pointer.HorizSize = 3
            Point0.Pointer.VertSize = 3

            'Series(1 bis n): Für jede Population eine Series 'TODO: es würde auch eine Series für alle reichen!
            For i = 0 To n_Populationen
                Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
                Point1.Title = "Population " & i.ToString()
                Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                Point1.Pointer.HorizSize = 3
                Point1.Pointer.VertSize = 3
            Next i

            'Formatierung der Axen
            .Chart.Axes.Bottom.Title.Caption = "Simulation"
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Maximum = n_Kalkulationen
            .Chart.Axes.Bottom.Minimum = 0
            '.Chart.Axes.Left.Title.Caption = OptZieleListe(0).Bezeichnung
            .Chart.Axes.Left.Automatic = True
            .Chart.Axes.Left.Minimum = 0
        End With
    End Sub

    Public Sub TeeChartInitialise_MO_BlauesModell(ByRef TChart1 As Steema.TeeChart.TChart)

        With TChart1
            .Clear()
            .Header.Text = "BlauesModell"
            .Aspect.View3D = False
            .Legend.Visible = False

            'Formatierung der Axen
            .Chart.Axes.Bottom.Title.Caption = OptZieleListe(0).Bezeichnung 'HACK: Beschriftung der Axen
            .Chart.Axes.Bottom.Automatic = True
            .Chart.Axes.Left.Title.Caption = OptZieleListe(1).Bezeichnung 'HACK: Beschriftung der Axen
            .Chart.Axes.Left.Automatic = True

            'Series(0): Series für die Population.
            Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
            Point1.Title = "Population"
            Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point1.Color = System.Drawing.Color.Orange
            Point1.Pointer.HorizSize = 2
            Point1.Pointer.VertSize = 2

            'Series(1): Series für die Sekundäre Population
            Dim Point2 As New Steema.TeeChart.Styles.Points(.Chart)
            Point2.Title = "Sekundäre Population"
            Point2.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point2.Color = System.Drawing.Color.Blue
            Point2.Pointer.HorizSize = 3
            Point2.Pointer.VertSize = 3

            'Series(2): Series für Bestwert
            Dim Point3 As New Steema.TeeChart.Styles.Points(.Chart)
            Point3.Title = "Bestwerte"
            Point3.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point3.Color = System.Drawing.Color.Green
            Point3.Pointer.HorizSize = 3
            Point3.Pointer.VertSize = 3

        End With
    End Sub

    '    Evaluierung des Blauen Modells für Parameter Optimierung - Steuerungseinheit
    '************************************************************************************

    Public Function Evaluierung_BlauesModell_ParaOpt(ByVal GlobalAnzPar As Short, ByVal GlobalAnzZiel As Short, ByVal mypara As Double(,), ByVal durchlauf As Integer, ByVal ipop As Short, ByRef QN As Double(), ByRef TChart1 As Steema.TeeChart.TChart) As Boolean
        Dim i As Short

        'Mutierte Parameter an OptParameter übergeben
        For i = 1 To GlobalAnzPar 'BUG 57: Par(,) fängt bei 1 an!
            OptParameterListe(i - 1).SKWert = mypara(i, 1)     'OptParameterListe(i-1,*) weil Array bei 0 anfängt!
        Next

        'Mutierte Parameter in Eingabedateien schreiben
        Call ModellParameter_schreiben()

        'Modell Starten
        Call launchBM()

        'Qualitätswerte berechnen und Rückgabe an den OptiAlgo
        'BUG 57: QN() fängt bei 1 an!
        For i = 0 To GlobalAnzZiel - 1
            OptZieleListe(i).QWertTmp = QualitaetsWert_berechnen(i)
            QN(i + 1) = OptZieleListe(i).QWertTmp
        Next

        'Qualitätswerte im TeeChart zeichnen
        Select Case GlobalAnzZiel
            Case 1
                TChart1.Series(ipop).Add(durchlauf, OptZieleListe(0).QWertTmp)
            Case 2
                TChart1.Series(0).Add(OptZieleListe(0).QWertTmp, OptZieleListe(1).QWertTmp, "")
            Case 3
                'TODO MsgBox: Das Zeichnen von mehr als 2 Zielfunktionen wird bisher nicht unterstützt
                'Call Zielfunktion_zeichnen_MultiObPar_3D(BlueM1.OptZieleListe(0).QWertTmp, BlueM1.OptZieleListe(1).QWertTmp, BlueM1.OptZieleListe(2).QWertTmp)
            Case Else
                'TODO MsgBox: Das Zeichnen von mehr als 2 Zielfunktionen wird bisher nicht unterstützt
                'TODO: Call Zielfunktion_zeichnen_MultiObPar_XD()
        End Select

        'Qualitätswerte und OptParameter in DB speichern
        Call db_update(durchlauf, ipop)

    End Function

    '************************** Funktionen für CombiOpt *********************************
    '************************************************************************************


    '    Evaluierung des Blauen Modells für Kombinatorik Optimierung - Steuerungseinheit
    '************************************************************************************

    Public Function Evaluierung_BlauesModell_CombiOpt(ByVal n_Ziele As Short, ByVal durchlauf As Integer, ByVal ipop As Short, ByRef Quality As Double(), ByRef TChart1 As Steema.TeeChart.TChart) As Boolean
        Dim i As Short

        'Modell Starten
        Call launchBM()

        'Qualitätswerte berechnen und Rückgabe an den OptiAlgo
        'BUG 57: QN() fängt bei 1 an!
        For i = 0 To n_Ziele - 1
            OptZieleListe(i).QWertTmp = QualitaetsWert_berechnen(i)
            Quality(i) = OptZieleListe(i).QWertTmp
        Next

        'Qualitätswerte im TeeChart zeichnen
        Select Case n_Ziele
            Case 1
                TChart1.Series(ipop).Add(durchlauf, OptZieleListe(0).QWertTmp)
            Case 2
                TChart1.Series(0).Add(OptZieleListe(0).QWertTmp, OptZieleListe(1).QWertTmp, "")
            Case 3
                'TODO MsgBox: Das Zeichnen von mehr als 2 Zielfunktionen wird bisher nicht unterstützt
                'Call Zielfunktion_zeichnen_MultiObPar_3D(BlueM1.OptZieleListe(0).QWertTmp, BlueM1.OptZieleListe(1).QWertTmp, BlueM1.OptZieleListe(2).QWertTmp)
            Case Else
                'TODO MsgBox: Das Zeichnen von mehr als 2 Zielfunktionen wird bisher nicht unterstützt
                'TODO: Call Zielfunktion_zeichnen_MultiObPar_XD()
        End Select

        ''Qualitätswerte und OptParameter in DB speichern
        'Call db_update(durchlauf, ipop)

    End Function

    'Kombinatorik Struktur **************************************

    Public Structure Massnahme
        Public Name As String
        Public Schaltung(,) As String
    End Structure

    Public Structure Lokation
        Public Name As String
        Public MassnahmeListe() As Massnahme
    End Structure

    Public LocationList() As Lokation

    Public VerzweigungsDatei(,) As String

    'Kombinatorik Funktionen **************************************

    'Kombinatorik einlesen
    Public Sub Kombinatorik_einlesen()
        Try
            Dim Datei As String = WorkDir & Datensatz & "." & Combi_Ext

            Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
            Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

            Dim Zeile As String
            Dim Anz As Integer = 0

            'Anzahl der Parameter feststellen
            Do
                Zeile = StrRead.ReadLine.ToString()
                If (Zeile.StartsWith("*") = False) Then
                    Anz += 1
                End If
            Loop Until StrRead.Peek() = -1

            Dim i As Integer = -1
            Dim j As Integer = 0
            ReDim LocationList(0)
            ReDim LocationList(0).MassnahmeListe(0)

            'Zurück zum Dateianfang und lesen
            FiStr.Seek(0, SeekOrigin.Begin)

            Dim array() As String
            Do
                Zeile = StrRead.ReadLine.ToString()
                If (Zeile.StartsWith("*") = False) Then
                    array = Zeile.Split("|")
                    'Werte zuweisen

                    If Not Is_Name_IN(array(1).Trim(), LocationList) Then
                        i += 1
                        j = 0
                        System.Array.Resize(LocationList, i + 1)
                        LocationList(i).Name = array(1).Trim()
                    End If
                    System.Array.Resize(LocationList(i).MassnahmeListe, j + 1)
                    ReDim LocationList(i).MassnahmeListe(j).Schaltung(2, 1)
                    LocationList(i).MassnahmeListe(j).Name = array(2).Trim()
                    LocationList(i).MassnahmeListe(j).Schaltung(0, 0) = array(3).Trim()
                    LocationList(i).MassnahmeListe(j).Schaltung(0, 1) = array(4).Trim()
                    LocationList(i).MassnahmeListe(j).Schaltung(1, 0) = array(5).Trim()
                    LocationList(i).MassnahmeListe(j).Schaltung(1, 1) = array(6).Trim()
                    LocationList(i).MassnahmeListe(j).Schaltung(2, 0) = array(7).Trim()
                    LocationList(i).MassnahmeListe(j).Schaltung(2, 1) = array(8).Trim()
                    'i += 1
                    j += 1
                End If

            Loop Until StrRead.Peek() = -1

            StrRead.Close()
            FiStr.Close()

        Catch except As Exception
            MsgBox(except.Message, MsgBoxStyle.Exclamation, "Fehler beim Lesen der Kombinatorik")
        End Try

    End Sub

    'Validierungsfunktion der Kombinatorik Prüft ob Verbraucher an zwei Standorten Dopp vorhanden sind
    Public Function Combinatoric_is_Valid() As Boolean
        Combinatoric_is_Valid = True
        Dim i, j, x, y, m, n As Integer

        For i = 0 To LocationList.GetUpperBound(0)
            For j = 1 To LocationList.GetUpperBound(0)
                For x = 0 To LocationList(i).MassnahmeListe.GetUpperBound(0)
                    For y = 0 To LocationList(j).MassnahmeListe.GetUpperBound(0)
                        For m = 0 To 2
                            For n = 0 To 2
                                If Not LocationList(i).MassnahmeListe(x).Schaltung(m, 0) = "X" And LocationList(j).MassnahmeListe(y).Schaltung(n, 0) = "X" Then
                                    If LocationList(i).MassnahmeListe(x).Schaltung(m, 0) = LocationList(j).MassnahmeListe(y).Schaltung(n, 0) Then
                                        Combinatoric_is_Valid = False
                                    End If
                                End If
                            Next
                        Next
                    Next
                Next
            Next
        Next
    End Function

    'Liest die Verzweigungen aus dem BModel in ein Array ein
    Public Sub Verzweigung_Read()
        Dim i As Integer

        Try
            Dim FiStr As FileStream = New FileStream(WorkDir & Datensatz & ".ver", FileMode.Open, IO.FileAccess.ReadWrite)
            Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

            'Anzahl der Parameter feststellen
            Dim Zeile As String
            Dim Anz As Integer = 0

            Do
                Zeile = StrRead.ReadLine.ToString()
                If (Zeile.StartsWith("*") = False) Then
                    Anz += 1
                End If
            Loop Until StrRead.Peek() = -1
            ReDim VerzweigungsDatei(Anz - 1, 3)

            'Zurück zum Dateianfang und lesen
            FiStr.Seek(0, SeekOrigin.Begin)

            'Einlesen der Zeile und übergeben an das Verzweidungsarray
            Dim ZeilenArray() As String

            Do
                Zeile = StrRead.ReadLine.ToString()
                If (Zeile.StartsWith("*") = False) Then
                    ZeilenArray = Zeile.Split("|")
                    'Verbraucher Array füllen
                    VerzweigungsDatei(i, 0) = ZeilenArray(1).Trim
                    VerzweigungsDatei(i, 1) = ZeilenArray(2).Trim
                    VerzweigungsDatei(i, 2) = ZeilenArray(3).Trim
                    VerzweigungsDatei(i, 3) = ZeilenArray(4).Trim
                    i += 1
                End If

            Loop Until StrRead.Peek() = -1

            StrRead.Close()
            FiStr.Close()

        Catch except As Exception
            MsgBox(except.Message, MsgBoxStyle.Exclamation, "Fehler beim Lesen der Kombinatorik")
        End Try
    End Sub
    Public Function Combinatoric_fits_to_Verzweisungsdatei() As Boolean
        Combinatoric_fits_to_Verzweisungsdatei = True
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim x As Integer = 0
        Dim y As Integer = 0

        Dim FoundA(VerzweigungsDatei.GetUpperBound(0)) As Boolean

        'Prüft ob jede Verzweigung einmal in der LocationList vorkommt
        For i = 0 To VerzweigungsDatei.GetUpperBound(0)
            For j = 0 To LocationList.GetUpperBound(0)
                For x = 0 To LocationList(j).MassnahmeListe.GetUpperBound(0)
                    For y = 0 To LocationList(j).MassnahmeListe(x).Schaltung.GetUpperBound(0)
                        If VerzweigungsDatei(i, 0) = LocationList(j).MassnahmeListe(x).Schaltung(y, 0) And VerzweigungsDatei(i, 1) = "2" Then
                            FoundA(i) = True
                        End If
                    Next
                Next
            Next
        Next

        'Prüft ob die nicht vorkommenden Verzweigungen Verzweigungen anderer Art sind
        For i = 0 To VerzweigungsDatei.GetUpperBound(0)
            If Not VerzweigungsDatei(i, 1) = "2" And FoundA(i) = False Then
                FoundA(i) = True
            End If
        Next

        Dim FoundB As Boolean = True
        Dim TmpBool As Boolean = False

        'Prüft ob alle in der LocationList Vorkommenden Verzweigungen auch in der Verzweigungsdatei sind
        For j = 0 To LocationList.GetUpperBound(0)
            For x = 0 To LocationList(j).MassnahmeListe.GetUpperBound(0)
                For y = 0 To LocationList(j).MassnahmeListe(x).Schaltung.GetUpperBound(0)
                    If Not LocationList(j).MassnahmeListe(x).Schaltung(y, 0) = "X" Then
                        TmpBool = False
                        For i = 0 To VerzweigungsDatei.GetUpperBound(0)
                            If VerzweigungsDatei(i, 0) = LocationList(j).MassnahmeListe(x).Schaltung(y, 0) And VerzweigungsDatei(i, 1) = "2" Then
                                TmpBool = True
                            End If
                        Next
                        If Not TmpBool Then
                            FoundB = False
                        End If
                    End If

                Next
            Next
        Next

        'Übergabe
        If FoundB = False Then
            Combinatoric_fits_to_Verzweisungsdatei = False
        Else
            For i = 0 To FoundA.GetUpperBound(0)
                If FoundA(i) = False Then
                    Combinatoric_fits_to_Verzweisungsdatei = False
                End If
            Next
        End If

    End Function

    'Schreibt die neuen Verzweigungen
    Public Sub Verzweigung_Write(ByVal SchaltArray(,))

        Dim AnzZeil As Integer
        Dim i, j As Integer
        Dim Zeilenarray() As String
        Dim Zeile As String
        Dim StrLeft As String
        Dim StrRight As String
        Dim DateiPfad As String
        Dim SplitZeile() As String

        Try
            DateiPfad = WorkDir & Datensatz & ".ver"
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

            'ZeilenArray wird zu neuer Datei zusammen gebaut
            For i = 0 To SchaltArray.GetUpperBound(0)
                If Not SchaltArray(i, 1) = Nothing Then
                    For j = 0 To Zeilenarray.GetUpperBound(0)
                        If Not Zeilenarray(j).StartsWith("*") Then
                            SplitZeile = Zeilenarray(j).Split("|")
                            If SchaltArray(i, 0) = SplitZeile(1).Trim Then
                                StrLeft = Microsoft.VisualBasic.Left(Zeilenarray(j), 31)
                                StrRight = Microsoft.VisualBasic.Right(Zeilenarray(j), 49)
                                If SchaltArray(i, 1) = "1" Then
                                    Zeilenarray(j) = StrLeft & "      100     " & StrRight
                                ElseIf (SchaltArray(i, 1) = "0") Then
                                    Zeilenarray(j) = StrLeft & "        0     " & StrRight
                                End If
                            End If
                        End If
                    Next
                End If
            Next

            'Alle Zeilen wieder in Datei schreiben
            Dim StrWrite As StreamWriter = New StreamWriter(DateiPfad, False, System.Text.Encoding.GetEncoding("iso8859-1"))
            For j = 0 To AnzZeil - 1
                StrWrite.WriteLine(Zeilenarray(j))
            Next

            StrWrite.Close()

        Catch except As Exception
            MsgBox("Fehler beim Schreiben der Mutierten Parameter" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
        End Try

    End Sub

    '***************************** Hilfs Funktionen *************************************
    '************************************************************************************

    'Hilfsfunktion um zu Prüfen ob der Name bereits vorhanden ist oder nicht
    Private Function Is_Name_IN(ByVal Name As String, ByVal Array() As Lokation) As Boolean
        Is_Name_IN = False
        Dim i As Integer
        For i = 0 To Array.GetUpperBound(0)
            If Name = Array(i).Name Then
                Is_Name_IN = True
                Exit Function
            End If
        Next
    End Function

End Class
