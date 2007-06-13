Imports System.IO
Imports System.Data.OleDb

'*******************************************************************************
'*******************************************************************************
'**** Klasse Sim                                                            ****
'****                                                                       ****
'**** Basisklasse für Simulationsmodelle wie BlueM und SMUSI                ****
'****                                                                       ****
'**** Christoph Huebner, Felix Froehlich                                    ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'****                                                                       ****
'**** Erstellt: April 2006                                                  ****
'****                                                                       ****
'**** Letzte Änderung: Juni 2007                                            ****
'*******************************************************************************
'*******************************************************************************

Public MustInherit Class Sim

#Region "Eigenschaften"

    'Eigenschaften
    '#############

    'Generelle Eigenschaften
    '-----------------------
    Public Datensatz As String                           'Name des zu simulierenden Datensatzes
    Public WorkDir As String                             'Arbeitsverzeichnis für das Blaue Modell
    Public Event WorkDirChange                           'Event für Änderung des Arbeitsverzeichnisses
    Public Exe As String                                 'Pfad zur EXE für die Simulation
    Public Ergebnisdb As Boolean = True                  'Gibt an, ob die Ergebnisdatenbank geschrieben werden soll
    Public SimStart As DateTime                          'Anfangsdatum der Simulation
    Public SimEnde As DateTime                           'Enddatum der Simulation
    Public SimDT As TimeSpan                             'Zeitschrittweite der Simulation

    'Konstanten
    '----------
    Public Const OptParameter_Ext As String = "OPT"      'Erweiterung der Datei mit den Optimierungsparametern (*.OPT)
    Public Const ModParameter_Ext As String = "MOD"      'Erweiterung der Datei mit den Modellparametern (*.MOD)
    Public Const OptZiele_Ext As String = "ZIE"          'Erweiterung der Datei mit den Zielfunktionen (*.ZIE)
    Public Const Combi_Ext As String = "CES"             'Erweiterung der Datei mit der Kombinatorik  (*.CES)

    'Optimierungsparameter
    '---------------------
    Public Structure Struct_OptParameter
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
        Public Overrides Function toString() As String
            Return Bezeichnung
        End Function
    End Structure

    Public List_OptParameter() As Struct_OptParameter = {} 'Liste der Optimierungsparameter

    'ModellParameter
    '---------------
    Public Structure Struct_ModellParameter
        '*| OptParameter | Bezeichnung  | Einh. | Datei | Zeile | von | bis | Faktor |
        Public OptParameter As String               'Optimierungsparameter, aus dem dieser Modellparameter errechnet wird
        Public Bezeichnung As String                'Bezeichnung
        Public Einheit As String                    'Einheit
        Public Datei As String                      'Dateiendung der BM-Eingabedatei
        Public Element As String                    'Optional: Das Element auf das sich der Modellparameter bezieht
        Public ZeileNr As Short                     'Zeile
        Public SpVon As Short                       'Anfangsspalte
        Public SpBis As Short                       'Endspalte
        Public Faktor As Double                     'Faktor fuer das Umrechnen zwischen OptParameter und ModellParameter
        Public Wert As Double                       'Aus OptParameter errechneter Wert
    End Structure

    Public List_ModellParameter() As Struct_ModellParameter = {} 'Liste der Modellparameter

    'Optimierungsziele
    '-----------------
    '*| Bezeichnung   | ZielTyp  | Datei |  SimGröße | ZielFkt  | WertTyp  | ZielWert | ZielGröße  | PfadReihe
    Public Structure Struct_OptZiel
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
        Public Overrides Function toString() As String
            Return Bezeichnung
        End Function
    End Structure

    Public List_OptZiele() As Struct_OptZiel = {}          'Liste der Zielfunktionnen

    'Ergebnisdatenbank
    '-----------------
    Private db As OleDb.OleDbConnection

    'Kombinatorik
    '------------
    Public SKos1 As New SKos()
    Public Path_Aktuell() As Integer
    Public VER_ONOFF(,) As Object

    Public Structure Struct_Massnahme
        Public Name As String
        Public Schaltung(,) As String
        Public KostenTyp As Integer
        Public Bauwerke() As String
        Public TestModus As Integer
    End Structure

    Public Structure Struct_Lokation
        Public Name As String
        Public List_Massnahmen() As Struct_Massnahme
    End Structure

    Public List_Locations() As Struct_Lokation
    Public VerzweigungsDatei(,) As String

    'Public Schaltung(2, 1) As Object
    'Public Maßnahme As Collection
    'Public Kombinatorik As Collection

#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

#Region "Initialisierung"

    'Bündelung von Initialisierungsfunktionen
    '****************************************
    Public Sub SimIni()

        'Dezimaltrennzeichen überprüfen
        Call Me.checkDezimaltrennzeichen()

        'EVO.ini Datei einlesen
        Call Me.ReadEVOIni()

    End Sub

    'Überprüfen, ob Punkt als Dezimaltrennzeichen eingestellt ist
    '***********************************************************
    Public Sub checkDezimaltrennzeichen()

        Dim ci As System.Globalization.CultureInfo
        Dim nfi As System.Globalization.NumberFormatInfo

        'Aktuelle Einstellungen lesen
        ci = System.Globalization.CultureInfo.CurrentCulture
        nfi = ci.NumberFormat

        'Dezimaltrennzeichen überprüfen
        If (Not nfi.NumberDecimalSeparator = ".") Then
            Throw New Exception("Um mit BlueM oder SMUSI arbeiten zu können, muss in der Systemsteuerung" & Chr(13) & Chr(10) & "als Dezimaltrennzeichen Punkt (.) eingestellt sein!")
        End If

    End Sub

    'EVO.ini Datei einlesen 
    '**********************
    Public Sub ReadEVOIni()

        'Einschränkung:
        '------------------------------------------------------------
        'Geht davon aus, dass das aktuelle Verzeichnis _Main\bin ist!
        '------------------------------------------------------------

        If File.Exists("EVO.ini") Then

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
                    Case "Exe"
                        Me.Exe = Configs(i, 1)
                    Case "Datensatz"
                        Call Me.saveDatensatz(Configs(i, 1))
                    Case Else
                        'weitere Voreinstellungen
                End Select
            Next

        Else
            'Datei EVO.ini existiert nicht
            Throw New Exception("Die Datei ""EVO.ini"" konnte nicht gefunden werden!" & Chr(13) & Chr(10) & "Bitte gemäß Dokumentation eine Datei ""EVO.ini"" erstellen.")
        End If

    End Sub

    'Pfad zum Datensatz verarbeiten und speichern
    '********************************************
    Public Sub saveDatensatz(ByVal Pfad As String)

        'Dateiname vom Ende abtrennen
        Me.Datensatz = Pfad.Substring(Pfad.LastIndexOf("\") + 1)
        'Dateiendung entfernen
        Me.Datensatz = Me.Datensatz.Substring(0, Me.Datensatz.Length - 4)
        'Arbeitsverzeichnis bestimmen
        Me.WorkDir = Pfad.Substring(0, Pfad.LastIndexOf("\") + 1)
        'Event auslösen (wird von Form1.displayWorkDir() verarbeitet)
        RaiseEvent WorkDirChange()

    End Sub

#End Region 'Initialisierung

#Region "Eingabedateien lesen"

    'PES vorbereiten (auch für SensiPlot)
    'Erforderliche Dateien werden eingelesen und DB vorbereitet
    '**********************************************************
    Public Sub prepare_Sim_PES()

        'Simulationsdaten einlesen
        Call Me.Read_SimParameter()
        'Zielfunktionen einlesen
        Call Me.Read_OptZiele()
        'Optimierungsparameter einlesen
        Call Me.Read_OptParameter()
        'ModellParameter einlesen
        Call Me.Read_ModellParameter()
        'Modell-/Optparameter validieren
        Call Me.Validate_OPT_fits_to_MOD()
        'Datenbank vorbereiten
        If Me.Ergebnisdb = True Then
            Call Me.db_prepare()
        End If

    End Sub

    'CES vorbereiten
    'Erforderliche Dateien werden eingelesen
    '***************************************
    Public Sub prepare_Sim_CES()

        'Zielfunktionen einlesen
        Call Me.Read_OptZiele()
        'Kombinatorik Datei einlesen
        Call Me.Read_Kombinatorik()
        'Verzweigungs Datei einlesen
        Call Me.Read_Verzweigungen()
        'Überprüfen der Kombinatorik
        Call Me.Validate_Combinatoric()
        'Prüfen ob Kombinatorik und Verzweigungsdatei zusammenpassen
        Call Me.Validate_CES_fits_to_VER()

    End Sub

    'Simulationsparameter einlesen
    '*****************************
    Protected MustOverride Sub Read_SimParameter()

    'Kombinatorik einlesen
    '*********************
    Protected MustOverride Sub Read_Kombinatorik()

    'Liest die Verzweigungen aus dem BModel in ein Array ein
    'Und Dimensioniert das Verzweigungsarray
    '*******************************************************
    Protected MustOverride Sub Read_Verzweigungen()

    'Optimierungsparameter einlesen
    '******************************
    Private Sub Read_OptParameter()

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

        ReDim List_OptParameter(AnzParam - 1)

        'Zurück zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        Dim array() As String
        Dim i As Integer = 0
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                array = Zeile.Split("|")
                'Werte zuweisen
                List_OptParameter(i).Bezeichnung = array(1).Trim()
                List_OptParameter(i).Einheit = array(2).Trim()
                List_OptParameter(i).Wert = Convert.ToDouble(array(3).Trim())
                List_OptParameter(i).Min = Convert.ToDouble(array(4).Trim())
                List_OptParameter(i).Max = Convert.ToDouble(array(5).Trim())
                i += 1
            End If
        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

    End Sub

    'Modellparameter einlesen
    '************************
    Private Sub Read_ModellParameter()

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

        ReDim List_ModellParameter(AnzParam - 1)

        'Zurück zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        Dim array() As String
        Dim i As Integer = 0
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                array = Zeile.Split("|")
                'Werte zuweisen
                List_ModellParameter(i).OptParameter = array(1).Trim()
                List_ModellParameter(i).Bezeichnung = array(2).Trim()
                List_ModellParameter(i).Einheit = array(3).Trim()
                List_ModellParameter(i).Datei = array(4).Trim()
                List_ModellParameter(i).Element = array(5).Trim()
                List_ModellParameter(i).ZeileNr = Convert.ToInt16(array(6).Trim())
                List_ModellParameter(i).SpVon = Convert.ToInt16(array(7).Trim())
                List_ModellParameter(i).SpBis = Convert.ToInt16(array(8).Trim())
                List_ModellParameter(i).Faktor = Convert.ToDouble(array(9).Trim())
                i += 1
            End If
        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

    End Sub

    'Optimierungsziele einlesen
    '**************************
    Protected Overridable Sub Read_OptZiele()
        Dim AnzZiele As Integer = 0
        Dim IsOK As Boolean
        Dim ext As String
        Dim i As Integer = 0
        Dim j As Integer = 0

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

        'BUG 66: nur die ersten beiden Zielfunktionen werden gezeichnet
        If (AnzZiele > 2) Then
            MsgBox("Die Anzahl der Ziele beträgt mehr als 2!" & Chr(13) & Chr(10) _
                    & "Es werden nur die ersten beiden Zielfunktionen im Hauptdiagramm angezeigt!", MsgBoxStyle.Information, "Info")
        End If

        ReDim List_OptZiele(AnzZiele - 1)

        'Zurück zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        'Einlesen der Zeile und übergeben an die OptimierungsZiele Liste
        Dim ZeilenArray(9) As String

        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                ZeilenArray = Zeile.Split("|")
                'Werte zuweisen
                List_OptZiele(i).Bezeichnung = ZeilenArray(1).Trim()
                List_OptZiele(i).ZielTyp = ZeilenArray(2).Trim()
                List_OptZiele(i).Datei = ZeilenArray(3).Trim()
                List_OptZiele(i).SimGr = ZeilenArray(4).Trim()
                List_OptZiele(i).ZielFkt = ZeilenArray(5).Trim()
                List_OptZiele(i).WertTyp = ZeilenArray(6).Trim()
                List_OptZiele(i).ZielWert = ZeilenArray(7).Trim()
                List_OptZiele(i).ZielGr = ZeilenArray(8).Trim()
                List_OptZiele(i).ZielReihePfad = ZeilenArray(9).Trim()
                i += 1
            End If
        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

        'Falls mit Reihen verglichen werden soll werden hier die Reihen eingelesen
        Dim ZielStart As Date
        Dim ZielEnde As Date

        For i = 0 To AnzZiele - 1
            If (List_OptZiele(i).ZielTyp = "Reihe" Or List_OptZiele(i).ZielTyp = "IHA") Then

                'Dateiendung der Zielreihendatei bestimmen und Reihe einlesen
                ext = List_OptZiele(i).ZielReihePfad.Substring(List_OptZiele(i).ZielReihePfad.LastIndexOf(".") + 1)
                Select Case (ext.ToUpper)
                    Case "WEL"
                        IsOK = Read_WEL(List_OptZiele(i).ZielReihePfad, List_OptZiele(i).ZielGr, List_OptZiele(i).ZielReihe)
                    Case "ZRE"
                        IsOK = Read_ZRE(List_OptZiele(i).ZielReihePfad, List_OptZiele(i).ZielReihe)
                    Case "PRB"
                        IsOK = Read_PRB(List_OptZiele(i).ZielReihePfad, List_OptZiele(i).ZielGr, List_OptZiele(i).ZielReihe)
                    Case Else
                        IsOK = False
                End Select

                If (IsOK = False) Then
                    Throw New Exception("Fehler beim einlesen der Zielreihe in '" & List_OptZiele(i).ZielReihePfad & "'" & Chr(13) & Chr(10) & "Ein Fehler könnten Leerzeichen in der letzten Zeile der Datei sein")
                End If

                'Zielreihe entsprechend dem Simulationszeitraum kürzen (nur bei WEL und ZRE)
                '---------------------------------------------------------------------------
                If (ext.ToUpper = "WEL" Or ext.ToUpper = "ZRE") Then

                    ZielStart = List_OptZiele(i).ZielReihe(0, 0)
                    ZielEnde = List_OptZiele(i).ZielReihe(List_OptZiele(i).ZielReihe.GetUpperBound(0), 0)

                    If (ZielStart > Me.SimStart Or ZielEnde < Me.SimEnde) Then
                        'Zielreihe deckt Simulationszeitraum nicht ab
                        Throw New Exception("Die Zielreihe in '" & List_OptZiele(i).ZielReihePfad & "' deckt den Simulationszeitraum nicht ab!")
                    Else
                        'Länge der Simulationszeitreihe:
                        Dim length_n As Integer = ((Me.SimEnde - Me.SimStart).TotalSeconds / Me.SimDT.TotalSeconds) + 1
                        'Zielreihe kopieren und redimensionieren
                        Dim tmpArray(,) As Object = List_OptZiele(i).ZielReihe
                        ReDim List_OptZiele(i).ZielReihe(length_n - 1, 1)
                        'Abstand zwischen Start von Simreihe und Zielreihe bestimmen
                        Dim offset_t As TimeSpan = Me.SimStart - ZielStart
                        Dim dt As TimeSpan = tmpArray(1, 0) - tmpArray(0, 0) 'BUG 105: Es wird von konstanten Zeitschritten ausgegangen
                        Dim offset_n As Integer = offset_t.TotalSeconds / dt.TotalSeconds
                        'Simulationszeitraum zurück in Zielreihe kopieren
                        Array.Copy(tmpArray, offset_n * 2, List_OptZiele(i).ZielReihe, 0, length_n * 2)
                    End If

                End If

            End If
        Next
    End Sub

#End Region 'Eingabedateien einlesen

#Region "Prüfung der Eingabedateien"

    'Prüft ob .OPT und .MOD Dateien zusammenpassen
    '*********************************************
    Private Sub Validate_OPT_fits_to_MOD()
        Dim i, j As Integer
        Dim isValid_A As Boolean = True
        Dim isValid_B As Boolean = True
        Dim isValid As Boolean = False

        'A: Prüfung ob für jeden OptParameter mindestens ein Modellparameter existiert
        For i = 0 To List_OptParameter.GetUpperBound(0)
            isvalid = False
            For j = 0 To List_ModellParameter.GetUpperBound(0)
                If List_OptParameter(i).Bezeichnung = List_ModellParameter(j).OptParameter Then
                    isValid = True
                End If
            Next
            If isValid = False Then
                isValid_A = False
            End If
        Next

        'B: Prüfung ob jeder ModellParameter einem richtigen OptParameter zugewiesen ist.
        For i = 0 To List_ModellParameter.GetUpperBound(0)
            isValid = False
            For j = 0 To List_OptParameter.GetUpperBound(0)
                If List_ModellParameter(i).OptParameter = List_OptParameter(j).Bezeichnung Then
                    isValid = True
                End If
            Next
            If isValid = False Then
                isValid_B = False
            End If
        Next

        If Not isValid_A Then
            Throw New Exception("Für einen OptParameter ist kein Modellparameter vorhanden!")
        End If

        If Not isValid_B Then
            Throw New Exception("Ein Modellparameter ist keinem OptParameter zugewiesen!")
        End If

    End Sub

    'Validierungsfunktion der Kombinatorik Prüft ob Verbraucher an zwei Standorten Dopp vorhanden sind
    '*************************************************************************************************
    Public Sub Validate_Combinatoric()

        Dim i, j, x, y, m, n As Integer

        For i = 0 To List_Locations.GetUpperBound(0)
            For j = 1 To List_Locations.GetUpperBound(0)
                For x = 0 To List_Locations(i).List_Massnahmen.GetUpperBound(0)
                    For y = 0 To List_Locations(j).List_Massnahmen.GetUpperBound(0)
                        For m = 0 To 2
                            For n = 0 To 2
                                If Not List_Locations(i).List_Massnahmen(x).Schaltung(m, 0) = "X" And List_Locations(j).List_Massnahmen(y).Schaltung(n, 0) = "X" Then
                                    If List_Locations(i).List_Massnahmen(x).Schaltung(m, 0) = List_Locations(j).List_Massnahmen(y).Schaltung(n, 0) Then
                                        Throw New Exception("Kombinatorik ist nicht valid!")
                                    End If
                                End If
                            Next
                        Next
                    Next
                Next
            Next
        Next

    End Sub

    'Mehrere Prüfungen ob die .VER Datei des BlueM und der .CES Datei auch zusammenpassen
    '************************************************************************************
    Public Sub Validate_CES_fits_to_VER()

        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim x As Integer = 0
        Dim y As Integer = 0

        Dim FoundA(VerzweigungsDatei.GetUpperBound(0)) As Boolean

        'Prüft ob jede Verzweigung einmal in der LocationList vorkommt
        For i = 0 To VerzweigungsDatei.GetUpperBound(0)
            For j = 0 To List_Locations.GetUpperBound(0)
                For x = 0 To List_Locations(j).List_Massnahmen.GetUpperBound(0)
                    For y = 0 To List_Locations(j).List_Massnahmen(x).Schaltung.GetUpperBound(0)
                        If VerzweigungsDatei(i, 0) = List_Locations(j).List_Massnahmen(x).Schaltung(y, 0) And VerzweigungsDatei(i, 1) = "2" Then
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
        For j = 0 To List_Locations.GetUpperBound(0)
            For x = 0 To List_Locations(j).List_Massnahmen.GetUpperBound(0)
                For y = 0 To List_Locations(j).List_Massnahmen(x).Schaltung.GetUpperBound(0)
                    If Not List_Locations(j).List_Massnahmen(x).Schaltung(y, 0) = "X" Then
                        TmpBool = False
                        For i = 0 To VerzweigungsDatei.GetUpperBound(0)
                            If VerzweigungsDatei(i, 0) = List_Locations(j).List_Massnahmen(x).Schaltung(y, 0) And VerzweigungsDatei(i, 1) = "2" Then
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
            Throw New Exception(".VER und .CES Dateien passen nicht zusammen!")
        Else
            For i = 0 To FoundA.GetUpperBound(0)
                If FoundA(i) = False Then
                    Throw New Exception(".VER und .CES Dateien passen nicht zusammen!")
                End If
            Next
        End If

    End Sub

#End Region 'Prüfung der Eingabedateien

#Region "Kombinatorik"

    'Kombinatorik
    '############

    'Berechnet die Anzahl maximal möglicher Kombinationen
    '****************************************************
    Public Function No_of_Combinations() As Integer
        Dim i As Integer

        No_of_Combinations = List_Locations(0).List_Massnahmen.GetLength(0)

        For i = 1 To List_Locations.GetUpperBound(0)
            No_of_Combinations = No_of_Combinations * List_Locations(i).List_Massnahmen.GetLength(0)
        Next

    End Function

    'Überprüft ob und welcher TestModus aktiv ist
    'Beschreibung:
    '********************************************
    Public Function Set_TestModus() As Integer

        Dim i, j As Integer
        Dim count_A As Integer
        Dim count_B As Integer
        Dim Bool As Boolean = False

        'Prüft auf den Modus "0" kein TestModus
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        count_A = 0
        count_B = 0
        For i = 0 To List_Locations.GetUpperBound(0)
            For j = 0 To List_Locations(i).List_Massnahmen.GetUpperBound(0)
                count_A += 1
                If List_Locations(i).List_Massnahmen(j).TestModus = 0 Then
                    count_B += 1
                End If
            Next
        Next

        If count_A = count_B Then
            Set_TestModus = 0
            Exit Function
        End If

        'Prüft aus Testen einer definierten Kombination
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        count_A = 0
        count_B = 0
        For i = 0 To List_Locations.GetUpperBound(0)
            count_A += 1
            For j = 0 To List_Locations(i).List_Massnahmen.GetUpperBound(0)
                If List_Locations(i).List_Massnahmen(j).TestModus = 1 Then
                    count_B += 1
                End If
            Next
        Next

        If count_A = count_B Then
            Set_TestModus = 1
            Exit Function
        End If

        'Prüft auf einmaliges Testen aller möglichen Kombinationen
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        count_A = 0
        count_B = 0
        For i = 0 To List_Locations.GetUpperBound(0)
            For j = 0 To List_Locations(i).List_Massnahmen.GetUpperBound(0)
                count_A += 1
                If List_Locations(i).List_Massnahmen(j).TestModus = 1 Then
                    count_B += 1
                End If
            Next
        Next

        If count_A = count_B Then
            Set_TestModus = 2
            Exit Function
        End If

        Throw New Exception("Fehler bei der angabe des Testmodus")

    End Function

    'Bereitet das SimModell für Kombinatorik Optimierung vor
    'TODO: Dieser Funktionsname ist sehr ähnlich mit "prepare_SIM_CES()"!
    '*******************************************************
    Public Sub Sim_Prepare(ByVal Path() As Integer)

        'Erstellt die aktuelle Bauerksliste und überträgt sie zu SKos
        Call Define_aktuelle_Elemente(Path)

        'Ermittelt das aktuelle_ON_OFF array
        Call Verzweigung_ON_OFF(Path)

        'Schreibt die neuen Verzweigungen
        Call Me.Write_Verzweigungen()

    End Sub

    'Die Liste mit den aktuellen Bauwerken des Kindes wird erstellt und in SKos geschrieben
    '**************************************************************************************
    Private Sub Define_aktuelle_Elemente(ByVal Path() As Integer)
        Dim i, j As Integer
        Dim No As Integer

        Dim x As Integer = 0
        For i = 0 To Path.GetUpperBound(0)
            No = Path(i)
            For j = 0 To List_Locations(i).List_Massnahmen(No).Bauwerke.GetUpperBound(0)
                Array.Resize(SKos1.AktuelleElemente, x + 1)
                SKos1.AktuelleElemente(x) = List_Locations(i).List_Massnahmen(No).Bauwerke(j)
                x += 1
            Next
        Next

        'Entfernt die X Einträge
        Call SKos1.Remove_X(SKos1.AktuelleElemente)
    End Sub

    'Die Liste mit den aktuellen Bauwerken wird an das Kind übergeben
    '**************************************************************************************
    Public Sub Set_Elemente(ByRef Path() As Object)

        ReDim Path(SKos1.AktuelleElemente.GetUpperBound(0))
        Array.Copy(SKos1.AktuelleElemente, Path, SKos1.AktuelleElemente.GetLength(0))

    End Sub

    'Ermittelt das aktuelle Verzweigungsarray
    '****************************************
    Private Sub Verzweigung_ON_OFF(ByVal Path() As Integer)
        Dim j, x, y, z As Integer
        Dim No As Short

        'Schreibt alle Bezeichnungen der Verzweigungen ins Array
        For j = 0 To VER_ONOFF.GetUpperBound(0)
            VER_ONOFF(j, 0) = VerzweigungsDatei(j, 0)
        Next
        'Weist die Werte das Pfades zu
        For x = 0 To Path.GetUpperBound(0)
            No = Path(x)
            For y = 0 To List_Locations(x).List_Massnahmen(No).Schaltung.GetUpperBound(0)
                For z = 0 To VER_ONOFF.GetUpperBound(0)
                    If List_Locations(x).List_Massnahmen(No).Schaltung(y, 0) = VER_ONOFF(z, 0) Then
                        VER_ONOFF(z, 1) = List_Locations(x).List_Massnahmen(No).Schaltung(y, 1)
                    End If
                Next
            Next
        Next

    End Sub

#End Region 'Kombinatorik

#Region "Evaluierung"

    'Reduziert die OptParameter und die ModellParameter auf die aktiven Elemente
    '***************************************************************************
    Public Sub Reduce_OptPara_ModPara(ByVal Bauwerksliste() As Object)
        Dim i, j, count As Integer
        Dim TMP() As Struct_ModellParameter
        ReDim TMP(List_ModellParameter.GetUpperBound(0))

        count = 0
        For i = 0 To List_ModellParameter.GetUpperBound(0)
            For j = 0 To Bauwerksliste.GetUpperBound(0)
                If List_ModellParameter(i).Element = Bauwerksliste(j) Then
                    Call copy_Struct_ModellParemeter(List_ModellParameter(i), TMP(count))
                    count += 1
                End If
            Next
        Next

        Array.Resize(TMP, count)
        Array.Resize(List_ModellParameter, count)

        For i = 0 To TMP.GetUpperBound(0)
            Call copy_Struct_ModellParemeter(TMP(i), List_ModellParameter(i))
        Next

    End Sub

    'Kopiert ein Strukt_ModellParameter
    '**********************************
    Private Sub copy_Struct_ModellParemeter(ByVal Source As Struct_ModellParameter, ByRef Destination As Struct_ModellParameter)

        Destination.OptParameter = Source.OptParameter
        Destination.Bezeichnung = Source.Bezeichnung
        Destination.Einheit = Source.Einheit
        Destination.Datei = Source.Datei
        Destination.Element = Source.Element
        Destination.ZeileNr = Source.ZeileNr
        Destination.SpVon = Source.SpVon
        Destination.SpBis = Source.SpBis
        Destination.Faktor = Source.Faktor
        Destination.Wert = Source.Wert

    End Sub

    'EVO-Parameterübergabe
    '*********************
    Public Sub Parameter_Uebergabe(ByRef globalAnzPar As Short, ByRef globalAnzZiel As Short, ByRef globalAnzRand As Short, ByRef mypara(,) As Double)

        Dim i As Integer

        'Anzahl Optimierungsparameter übergeben
        globalAnzPar = Me.List_OptParameter.GetLength(0)

        'Parameterwerte übergeben
        'BUG 57: mypara() fängt bei 1 an!
        ReDim mypara(globalAnzPar, 1)
        For i = 1 To globalAnzPar
            mypara(i, 1) = Me.List_OptParameter(i - 1).SKWert
        Next

        'globale Anzahl der Ziele muss hier auf Länge der Zielliste gesetzt werden
        globalAnzZiel = Me.List_OptZiele.GetLength(0)

        'TODO: Randbedingungen
        globalAnzRand = 2

    End Sub

    'Evaluierung des SimModells für ParameterOptimierung - Steuerungseinheit
    '***********************************************************************
    Public Sub Eval_Sim_ParaOpt(ByVal GlobalAnzPar As Short, ByVal GlobalAnzZiel As Short, ByVal mypara As Double(,), ByVal durchlauf As Integer, ByVal ipop As Short, ByRef QN As Double(), ByRef TChart1 As Steema.TeeChart.TChart)

        Dim i As Short

        'Mutierte Parameter an OptParameter übergeben
        For i = 1 To GlobalAnzPar                                   'BUG 57: mypara(,) fängt bei 1 an!
            List_OptParameter(i - 1).SKWert = mypara(i, 1)          'OptParameterListe(i-1) weil Array bei 0 anfängt!
        Next

        'Mutierte Parameter in Eingabedateien schreiben
        Call ModellParameter_schreiben()

        'Modell Starten
        Call launchSim()

        'Qualitätswerte berechnen und Rückgabe an den OptiAlgo
        For i = 0 To GlobalAnzZiel - 1                              'BUG 57: QN() fängt bei 1 an!
            List_OptZiele(i).QWertTmp = QWert(List_OptZiele(i))
            QN(i + 1) = List_OptZiele(i).QWertTmp
        Next

        'Qualitätswerte im TeeChart zeichnen
        If (GlobalAnzZiel = 1) Then
            'SingleObjective
            TChart1.Series(ipop).Add(durchlauf, List_OptZiele(0).QWertTmp)
        Else
            'MultiObjective
            'BUG 66: nur die ersten beiden Zielfunktionen werden gezeichnet
            TChart1.Series(0).Add(List_OptZiele(0).QWertTmp, List_OptZiele(1).QWertTmp, "")
        End If

        'Qualitätswerte und OptParameter in DB speichern
        If (Ergebnisdb = True) Then
            Call db_update(durchlauf, ipop)
        End If

    End Sub

    'Die ModellParameter in die Eingabedateien des SimModells schreiben
    '******************************************************************
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
        For i As Integer = 0 To List_ModellParameter.GetUpperBound(0)

            DateiPfad = WorkDir & Datensatz & "." & List_ModellParameter(i).Datei
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
            Zeile = Zeilenarray(List_ModellParameter(i).ZeileNr - 1)
            'BUG 120: richtig wäre: Length = SpBis - SpVon + 1
            Dim Length As Short = List_ModellParameter(i).SpBis - List_ModellParameter(i).SpVon
            StrLeft = Microsoft.VisualBasic.Left(Zeile, List_ModellParameter(i).SpVon - 1)
            StrRight = Microsoft.VisualBasic.Right(Zeile, Len(Zeile) - List_ModellParameter(i).SpBis + 1)

            Wert = List_ModellParameter(i).Wert.ToString()
            If (Wert.Length > Length) Then
                'TODO: Parameter wird für erforderliche Stringlänge einfach abgeschnitten, sollte aber gerundet werden!
                Wert = Wert.Substring(0, Length)
            Else
                Wert = Wert.PadLeft(Length)
            End If
            Zeilenarray(List_ModellParameter(i).ZeileNr - 1) = StrLeft & Wert & StrRight

            'Alle Zeilen wieder in Datei schreiben
            Dim StrWrite As StreamWriter = New StreamWriter(DateiPfad, False, System.Text.Encoding.GetEncoding("iso8859-1"))
            For j = 0 To AnzZeil - 1
                StrWrite.WriteLine(Zeilenarray(j))
            Next

            StrWrite.Close()

        Next

    End Sub

    'ModellParameter aus OptParametern errechnen
    '*******************************************
    Protected Sub OptParameter_to_ModellParameter()
        Dim i As Integer
        Dim j As Integer
        For i = 0 To List_ModellParameter.GetUpperBound(0)
            For j = 0 To List_OptParameter.GetUpperBound(0)
                If List_ModellParameter(i).OptParameter = List_OptParameter(j).Bezeichnung Then
                    List_ModellParameter(i).Wert = List_OptParameter(j).Wert * List_ModellParameter(i).Faktor
                End If
            Next
        Next
    End Sub

    'Evaluiert die Kinderchen für Kombinatorik Optimierung vor
    '*********************************************************
    Public Function Sim_Evaluierung_CombiOpt(ByVal n_Ziele As Short, ByRef Penalty As Double()) As Boolean
        Dim i As Short

        'Modell Starten
        Call launchSim()

        'Qualitätswerte berechnen und Rückgabe an den OptiAlgo
        For i = 0 To n_Ziele - 1                                    'BUG 57: QN() fängt bei 1 an!
            List_OptZiele(i).QWertTmp = QWert(List_OptZiele(i))
            Penalty(i) = List_OptZiele(i).QWertTmp
        Next

    End Function

    'Schreibt die neuen Verzweigungen
    '********************************
    Protected MustOverride Sub Write_Verzweigungen()

    'SimModell ausführen (simulieren)
    '********************************
    Public MustOverride Sub launchSim()

#End Region 'Evaluierung

#Region "Qualitätswertberechnung"

    'Qualitätswertberechnung
    '#######################

    'Berechnung des Qualitätswerts (Zielwert)
    '****************************************
    Public Function QWert(ByVal OptZiel As Struct_OptZiel) As Double

        QWert = 0

        Dim IsOK As Boolean

        'Fallunterscheidung Ergebnisdatei
        '--------------------------------
        Select Case OptZiel.Datei

            Case "WEL"
                'QWert aus WEL-Datei
                QWert = QWert_WEL(OptZiel)

            Case "PRB"
                'QWert aus PRB-Datei
                QWert = QWert_PRB(OptZiel)

            Case Else
                'es wurde eine nicht unterstützte Ergebnisdatei angegeben
                IsOK = False

        End Select

        If (IsOK = False) Then
            'TODO: Fehlerbehandlung
        End If

    End Function

    'Qualitätswert aus WEL-Datei
    '***************************
    Protected Overridable Function QWert_WEL(ByVal OptZiel As Struct_OptZiel) As Double

        Dim IsOK As Boolean
        Dim QWert As Double
        Dim SimReihe(,) As Object = {}

        'Simulationsergebnis auslesen
        IsOK = Read_WEL(WorkDir & Datensatz & ".wel", OptZiel.SimGr, SimReihe)

        'Fallunterscheidung Zieltyp
        '--------------------------
        Select Case OptZiel.ZielTyp

            Case "Wert"
                QWert = QWert_Wert(OptZiel, SimReihe)

            Case "Reihe"
                QWert = QWert_Reihe(OptZiel, SimReihe)

        End Select

        Return QWert

    End Function

    'Qualitätswert berechnen: Zieltyp = Reihe
    '****************************************
    'BUG 105: Konstante und gleiche Zeitschrittweiten vorausgesetzt!
    Protected Function QWert_Reihe(ByVal OptZiel As Struct_OptZiel, ByVal SimReihe As Object(,)) As Double

        Dim QWert As Double
        Dim i As Integer

        'Fallunterscheidung Zielfunktion
        '-------------------------------
        Select Case OptZiel.ZielFkt

            Case "AbQuad"
                'Summe der Fehlerquadrate
                For i = 0 To SimReihe.GetUpperBound(0)
                    QWert += (OptZiel.ZielReihe(i, 1) - SimReihe(i, 1)) * (OptZiel.ZielReihe(i, 1) - SimReihe(i, 1))
                Next

            Case "Diff"
                'Summe der Fehler
                For i = 0 To SimReihe.GetUpperBound(0)
                    QWert += Math.Abs(OptZiel.ZielReihe(i, 1) - SimReihe(i, 1))
                Next

            Case "Volf"
                'Volumenfehler
                'BUG 104: Volumenfehler rechnet noch nicht echtes Volumen, dazu ist Zeitschrittweite notwendig
                Dim VolSim As Double = 0
                Dim VolZiel As Double = 0
                For i = 0 To SimReihe.GetUpperBound(0)
                    VolSim += SimReihe(i, 1)
                    VolZiel += OptZiel.ZielReihe(i, 1)
                Next
                QWert = Math.Abs(VolZiel - VolSim)

            Case "nUnter"
                'Relative Anzahl der Zeitschritte mit Unterschreitungen (in Prozent)
                Dim nUnter As Integer = 0
                For i = 0 To SimReihe.GetUpperBound(0)
                    If (SimReihe(i, 1) < OptZiel.ZielReihe(i, 1)) Then
                        nUnter += 1
                    End If
                Next
                QWert = nUnter / SimReihe.GetUpperBound(0) * 100

            Case "nÜber"
                'Relative Anzahl der Zeitschritte mit Überschreitungen (in Prozent)
                Dim nUeber As Integer = 0
                For i = 0 To SimReihe.GetUpperBound(0)
                    If (SimReihe(i, 1) > OptZiel.ZielReihe(i, 1)) Then
                        nUeber += 1
                    End If
                Next
                QWert = nUeber / SimReihe.GetUpperBound(0) * 100

            Case Else
                Throw New Exception("Die Zielfunktion '" & OptZiel.ZielFkt & "' wird nicht unterstützt!")

        End Select

        Return QWert

    End Function

    'Qualitätswert berechnen: Zieltyp = Wert
    '***************************************
    Protected Function QWert_Wert(ByVal OptZiel As Struct_OptZiel, ByVal SimReihe As Object(,)) As Double

        Dim QWert As Double
        Dim i As Integer

        'Wert aus Simulationsergebnis berechnen
        '--------------------------------------
        Dim SimWert As Single

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
                Throw New Exception("Der Werttyp '" & OptZiel.WertTyp & "' wird nicht unterstützt!")

        End Select

        'QWert berechnen
        '---------------
        'Fallunterscheidung Zielfunktion
        '-------------------------------
        Select Case OptZiel.ZielFkt

            Case "AbQuad"
                'Summe der Fehlerquadrate
                QWert = (OptZiel.ZielWert - SimWert) * (OptZiel.ZielWert - SimWert)

            Case "Diff"
                'Summe der Fehler
                QWert = Math.Abs(OptZiel.ZielWert - SimWert)

            Case "nUnter"
                'Relative Anzahl der Zeitschritte mit Unterschreitungen (in Prozent)
                Dim nUnter As Integer = 0
                For i = 0 To SimReihe.GetUpperBound(0)
                    If (SimReihe(i, 1) < OptZiel.ZielWert) Then
                        nUnter += 1
                    End If
                Next
                QWert = nUnter / SimReihe.GetUpperBound(0) * 100

            Case "nÜber"
                'Relative Anzahl der Zeitschritte mit Überschreitungen (in Prozent)
                Dim nUeber As Integer = 0
                For i = 0 To SimReihe.GetUpperBound(0)
                    If (SimReihe(i, 1) > OptZiel.ZielWert) Then
                        nUeber += 1
                    End If
                Next
                QWert = nUeber / SimReihe.GetUpperBound(0) * 100

            Case Else
                Throw New Exception("Die Zielfunktion '" & OptZiel.ZielFkt & "' wird für Werte nicht unterstützt!")

        End Select

        Return QWert

    End Function

    'Qualitätswert aus PRB-Datei
    '***************************
    Private Function QWert_PRB(ByVal OptZiel As Struct_OptZiel) As Double

        Dim i As Integer
        Dim IsOK As Boolean
        Dim QWert As Double
        Dim SimReihe As Object(,) = {}

        'Simulationsergebnis auslesen
        IsOK = Read_PRB(WorkDir & Datensatz & ".PRB", OptZiel.SimGr, SimReihe)

        'Diff
        '----
        'Überflüssige Stützstellen (P) entfernen
        '---------------------------------------
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
        '-------------------------------------------
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

        Return QWert

    End Function

#End Region 'Qualitätswertberechnung

#Region "SimErgebnisse lesen"

    'SimErgebnisse lesen
    '###################

    'Eine ZRE-Datei einlesen
    '***********************
    Public Shared Function Read_ZRE(ByVal DateiPfad As String, ByRef ZRE(,) As Object) As Boolean

        Dim AnzZeil As Integer = 0
        Dim j As Integer = 0
        Dim Zeile As String
        Const ZREHEaderLen As Integer = 4     'Die ersten 4 Zeilen der ZRE-Datei gehören zum Header

        Read_ZRE = True

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

    End Function

    'Eine Spalte einer WEL-Datei einlesen
    '************************************
    Public Shared Function Read_WEL(ByVal Dateipfad As String, ByVal Spalte As String, ByRef WEL(,) As Object) As Boolean

        'Einschränkungen:
        '---------------------------------------------------
        'WEL-Datei muss im CSV-Format mit Semikola vorliegen

        Dim AnzZeil As Integer = 0
        Dim j As Integer = 0
        Dim Zeile As String
        Dim Werte() As String = {}
        Dim SpalteNr As Integer = -1
        Const WELHeaderLen As Integer = 3       'Die ersten 3 Zeilen der WEL-Datei gehören zum Header
        Read_WEL = True

        Dim FiStr As FileStream = New FileStream(Dateipfad, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        'Anzahl der Zeilen feststellen
        Do
            Zeile = StrRead.ReadLine.ToString
            AnzZeil += 1
        Loop Until StrRead.Peek() = -1

        ReDim WEL(AnzZeil - WELHeaderLen - 1, 1)

        'Position der zu lesenden Spalte bestimmen
        '-----------------------------------------
        FiStr.Seek(0, SeekOrigin.Begin)
        'Zeile mit den Spaltenüberschriften auslesen
        For j = 0 To 1
            Werte = StrRead.ReadLine.ToString.Split(";")
        Next
        StrRead.ReadToEnd()
        'Spaltenüberschriften vergleichen
        For j = 0 To Werte.GetUpperBound(0)
            If Werte(j).Trim() = Spalte Then
                SpalteNr = j
            End If
        Next

        'Wenn Spalte nicht gefunden
        '--------------------------
        If (SpalteNr = -1) Then
            Read_WEL = False
            Throw New Exception("Konnte die Spalte """ & Spalte & """ in der WEL-Datei nicht finden!")
        End If

        'Auf Anfang setzen und einlesen
        '------------------------------
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

    End Function

    'Ein Ergebnis aus einer PRB-Datei einlesen
    '*****************************************
    Public Shared Function Read_PRB(ByVal DateiPfad As String, ByVal ZielGr As String, ByRef PRB(,) As Object) As Boolean

        Dim ZeileStart As Integer = 0
        Dim AnzZeil = 26                   'Anzahl der Zeilen ist immer 26, definiert durch MAXSTZ in BM
        Dim j As Integer = 0
        Dim Zeile As String
        Read_PRB = True

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
        '---------------------------------------
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

    End Function

#End Region 'SimErgebnisse lesen

#Region "Ergebnisdatenbank"

    'Methoden für die Ergebnisdatenbank
    '##################################

    'Ergebnisdatenbank vorbereiten
    '*****************************
    Private Sub db_prepare()

        'Leere/Neue Ergebnisdatenbank in Arbeitsverzeichnis kopieren
        '-----------------------------------------------------------
        Dim ZielDatei As String = WorkDir & Datensatz & "_EVO.mdb"

        Dim currentDir As String = CurDir()     'sollte das /bin Verzeichnis von _Main sein
        ChDir("../../Apps")                     'wechselt in das /Apps Verzeichnis 
        My.Computer.FileSystem.CopyFile("EVO.mdb", ZielDatei, True)
        ChDir(currentDir)                       'zurück in das Ausgangsverzeichnis wechseln

        'Tabellen anpassen
        '-----------------
        Dim i As Integer

        Call db_connect()
        Dim command As OleDbCommand = New OleDbCommand("", db)
        'Tabelle 'QWerte'
        'Spalten festlegen:
        Dim fieldnames As String = ""
        For i = 0 To List_OptZiele.GetUpperBound(0)
            If (i > 0) Then
                fieldnames &= ", "
            End If
            fieldnames &= "'" & List_OptZiele(i).Bezeichnung & "' DOUBLE"
        Next
        'Tabelle anpassen
        command.CommandText = "ALTER TABLE QWerte ADD COLUMN " & fieldnames
        command.ExecuteNonQuery()

        'Tabelle 'OptParameter'
        'Spalten festlegen:
        fieldnames = ""
        For i = 0 To List_OptParameter.GetUpperBound(0)
            If (i > 0) Then
                fieldnames &= ", "
            End If
            fieldnames &= "'" & List_OptParameter(i).Bezeichnung & "' DOUBLE"
        Next
        'Tabelle anpassen
        command.CommandText = "ALTER TABLE OptParameter ADD COLUMN " & fieldnames
        command.ExecuteNonQuery()
        Call db_disconnect()

    End Sub

    'Mit Ergebnisdatenbank verbinden
    '*******************************
    Private Sub db_connect()
        Dim ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & WorkDir & Datensatz & "_EVO.mdb"
        db = New OleDb.OleDbConnection(ConnectionString)
        db.Open()
    End Sub

    'Verbindung zu Ergebnisdatenbank schließen
    '*****************************************
    Private Sub db_disconnect()
        db.Close()
    End Sub

    'Update der ErgebnisDB mit QWerten und OptParametern
    '***************************************************
    Private Function db_update(ByVal durchlauf As Integer, ByVal ipop As Short) As Boolean
        Call db_connect()

        Dim i As Integer

        Dim command As OleDbCommand = New OleDbCommand("", db)
        'QWert schreiben 
        'Spalten der Tabelle 'Qwerte' bestimmen:
        Dim fieldnames As String = ""
        Dim fieldvalues As String = ""
        For i = 0 To List_OptZiele.GetUpperBound(0)
            fieldnames &= ", '" & List_OptZiele(i).Bezeichnung & "'"
            fieldvalues &= ", " & List_OptZiele(i).QWertTmp
        Next
        command.CommandText = "INSERT INTO QWerte (durchlauf, ipop " & fieldnames & ") VALUES (" & durchlauf & ", " & ipop & fieldvalues & ")"
        command.ExecuteNonQuery()
        'ID des zuletzt geschriebenen QWerts holen
        command.CommandText = "SELECT @@IDENTITY AS ID"
        Dim QWert_ID As Integer = command.ExecuteScalar()

        'Zugehörige OptParameter schreiben
        fieldnames = ""
        fieldvalues = ""
        For i = 0 To List_OptParameter.GetUpperBound(0)
            fieldnames &= ", '" & List_OptParameter(i).Bezeichnung & "'"
            fieldvalues &= ", " & List_OptParameter(i).Wert
        Next
        command.CommandText = "INSERT INTO OptParameter (QWert_ID" & fieldnames & ") VALUES (" & QWert_ID & fieldvalues & ")"
        command.ExecuteNonQuery()

        Call db_disconnect()

    End Function

    'Einen Parametersatz auslesen
    '****************************
    Public Sub db_getOptPara(ByVal id As Integer)

        Call db_connect()

        Dim q As String = "SELECT * FROM OptParameter WHERE ID = " & id

        Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(q, db)

        Dim ds As New DataSet("EVO")
        adapter.Fill(ds, "OptParameter")

        'Parametersatz übergeben
        For i As Integer = 0 To Me.List_OptParameter.GetUpperBound(0)

            With Me.List_OptParameter(i)
                .Wert = ds.Tables("OptParameter").Rows(0).Item("'" & .Bezeichnung & "'")
            End With

        Next

        Call db_disconnect()

    End Sub

    'QWerte aus einer DB lesen
    '*************************
    Public Shared Function db_readQWerte(ByVal mdbfile As String) As Collection

        'Connect
        Dim ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & mdbfile
        Dim db As OleDb.OleDbConnection = New OleDb.OleDbConnection(ConnectionString)
        db.Open()

        'Read
        Dim q As String = "SELECT * FROM QWerte ORDER BY ID"

        Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(q, db)

        Dim ds As New DataSet("QWerte")
        adapter.Fill(ds, "QWerte")

        'Disconnect
        db.Close()

        'Werte einlesen
        '--------------
        Dim seriesCollection As New Collection
        'Schleife über Spalten (Zielfunktionen fangen erst bei 3 an!)
        For i As Integer = 3 To ds.Tables("QWerte").Columns.Count - 1
            Dim tmpserie As Serie = New Serie(ds.Tables("QWerte").Columns(i).Caption, ds.Tables("QWerte").Rows.Count)
            'Schleife über Reihen
            For j As Integer = 0 To ds.Tables("QWerte").Rows.Count - 1
                tmpserie.values(j) = ds.Tables("QWerte").Rows(j).Item(i)
            Next
            'Serie zu Collection hinzufügen
            seriesCollection.Add(tmpserie, tmpserie.name)
        Next

        Return seriesCollection

    End Function

#End Region 'Ergebnisdatenbank

#End Region 'Methoden

End Class

'zusätzliche Klassen
'###################

'Klasse Serie 
'enthält einen Namen und eine Liste von Werten
'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
Public Class Serie

    Public name As String
    Public values() As Double

    'Konstruktor
    '***********
    Public Sub New(ByVal name As String, ByVal length As Integer)
        Me.name = name
        ReDim values(length)
    End Sub

End Class


