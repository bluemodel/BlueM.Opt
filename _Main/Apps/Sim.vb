Imports System.IO
Imports System.Data.OleDb

'*******************************************************************************
'*******************************************************************************
'**** Klasse Sim                                                            ****
'****                                                                       ****
'**** Basisklasse f�r Simulationsmodelle wie BlueM und SMUSI                ****
'****                                                                       ****
'**** Christoph Huebner, Felix Froehlich                                    ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'****                                                                       ****
'**** Erstellt: April 2006                                                  ****
'****                                                                       ****
'**** Letzte �nderung: Juli 2007                                            ****
'*******************************************************************************
'*******************************************************************************

Public MustInherit Class Sim

#Region "Eigenschaften"

    'Eigenschaften
    '#############

    'Information
    '-----------

    Public Method as String                             'Verwendete Methode

    'Generelle Eigenschaften
    '-----------------------
    Public Datensatz As String                           'Name des zu simulierenden Datensatzes
    Public WorkDir As String                             'Arbeitsverzeichnis f�r das Blaue Modell
    Public Event WorkDirChange()                           'Event f�r �nderung des Arbeitsverzeichnisses
    Public Exe As String                                 'Pfad zur EXE f�r die Simulation
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
    Public List_OptParameter_Save() As Struct_OptParameter = {} 'Liste der Optimierungsparameter die nicht ver�ndert wird

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

    Public List_ModellParameter() As Struct_ModellParameter = {}      'Liste der Modellparameter
    Public List_ModellParameter_Save() As Struct_ModellParameter = {} 'Liste der Modellparameter die nicht ver�ndert wird

    'Optimierungsziele
    '-----------------
    '*| Bezeichnung   | ZielTyp  | Datei |  SimGr��e | ZielFkt  | WertTyp  | ZielWert | ZielGr��e  | PfadReihe
    Public Structure Struct_OptZiel
        Public Bezeichnung As String                'Bezeichnung
        Public ZielTyp As String                    'Gibt an ob es sich um einen Wert oder um eine Reihe handelt
        Public Datei As String                      'Die Ergebnisdatei, aus der das Simulationsergebnis ausgelesen werden soll [WEL, BIL, PRB]
        Public SimGr As String                      'Die Simulationsgr��e, auf dessen Basis der Qualit�tswert berechnet werden soll
        Public ZielFkt As String                    'Zielfunktion
        Public WertTyp As String                    'Gibt an wie der Wert, der mit dem Zielwert verglichen werden soll, aus dem Simulationsergebnis berechnet werden soll.
        Public ZielWert As String                   'Der vorgegeben Zielwert
        Public ZielReiheDatei As String             'Der Dateiname der Zielreihe
        Public ZielGr As String                     'Spalte der .wel Datei falls ZielReihe .wel Datei ist
        Public ZielReihe As Wave.Zeitreihe          'Die Werte der Zielreihe
        Public QWertTmp As Double                   'Qualit�tswert der letzten Simulation wird hier zwischengespeichert 
        Public Overrides Function toString() As String
            Return Bezeichnung
        End Function
    End Structure

    Public List_OptZiele() As Struct_OptZiel = {}   'Liste der Zielfunktionnen

    'Ergebnisdatenbank
    '-----------------
    Public Ergebnisdb As Boolean = True             'Gibt an, ob die Ergebnisdatenbank geschrieben werden soll
    Public db_path As String                        'Pfad zur Ergebnisdatenbank
    Private db As OleDb.OleDbConnection

    'Kombinatorik
    '------------
    Protected SKos1 As New SKos()
    Private Aktuell_Path() As Integer
    Public Aktuell_Measure() As String
    Private Aktuell_Elemente() As String
    'Private IsNewCombination As Boolean
    'Private Aktuell_db_Pfad_ID as Integer
    Protected VER_ONOFF(,) As Object

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
    'Public Ma�nahme As Collection
    'Public Kombinatorik As Collection

#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

#Region "Initialisierung"

    'B�ndelung von Initialisierungsfunktionen
    '****************************************
    Public Sub SimIni()

        'Dezimaltrennzeichen �berpr�fen
        Call Me.checkDezimaltrennzeichen()

        'EVO.ini Datei einlesen
        Call Me.ReadEVOIni()

    End Sub

    '�berpr�fen, ob Punkt als Dezimaltrennzeichen eingestellt ist
    '***********************************************************
    Public Sub checkDezimaltrennzeichen()

        Dim ci As System.Globalization.CultureInfo
        Dim nfi As System.Globalization.NumberFormatInfo

        'Aktuelle Einstellungen lesen
        ci = System.Globalization.CultureInfo.CurrentCulture
        nfi = ci.NumberFormat

        'Dezimaltrennzeichen �berpr�fen
        If (Not nfi.NumberDecimalSeparator = ".") Then
            Throw New Exception("Um mit BlueM oder SMUSI arbeiten zu k�nnen, muss in der Systemsteuerung" & Chr(13) & Chr(10) & "als Dezimaltrennzeichen Punkt (.) eingestellt sein!")
        End If

    End Sub

    'EVO.ini Datei einlesen 
    '**********************
    Public Sub ReadEVOIni()

        'Pfad zur Assembly bestimmen (\_Main\bin\)
        Dim binpath As String = System.Windows.Forms.Application.StartupPath()
        Dim inifilepath As String = binpath & "\EVO.ini"

        If File.Exists(inifilepath) Then

            'Datei einlesen
            Dim FiStr As FileStream = New FileStream(inifilepath, FileMode.Open, IO.FileAccess.Read)
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
            Throw New Exception("Die Datei ""EVO.ini"" konnte nicht gefunden werden!" & Chr(13) & Chr(10) & "Bitte gem�� Dokumentation eine Datei ""EVO.ini"" erstellen.")
        End If

    End Sub

    'Pfad zum Datensatz verarbeiten und speichern
    '********************************************
    Public Sub saveDatensatz(ByVal Pfad As String)

        'Datensatzname bestimmen
        Me.Datensatz = Path.GetFileNameWithoutExtension(Pfad)
        'Arbeitsverzeichnis bestimmen
        Me.WorkDir = Path.GetDirectoryName(Pfad) & "\"
        'Event ausl�sen (wird von Form1.displayWorkDir() verarbeitet)
        RaiseEvent WorkDirChange()

    End Sub

#End Region 'Initialisierung

#Region "Eingabedateien lesen"

    'PES vorbereiten (auch f�r SensiPlot)
    'Erforderliche Dateien werden eingelesen und DB vorbereitet
    '**********************************************************
    Public Sub read_and_valid_INI_Files_PES()

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
            Call Me.db_prepare_PES()
        End If

    End Sub

    'CES vorbereiten
    'Erforderliche Dateien werden eingelesen
    '***************************************
    Public Sub read_and_valid_INI_Files_CES()

        'Zielfunktionen einlesen
        Call Me.Read_OptZiele()
        'Kombinatorik Datei einlesen
        Call Me.Read_Kombinatorik()
        'Verzweigungs Datei einlesen
        Call Me.Read_Verzweigungen()
        '�berpr�fen der Kombinatorik
        Call Me.Validate_Combinatoric()
        'Pr�fen ob Kombinatorik und Verzweigungsdatei zusammenpassen
        Call Me.Validate_CES_fits_to_VER()
        'Datenbank vorbereiten
        If Me.Ergebnisdb = True Then
            Call Me.db_prepare()
            Call Me.db_prepare_CES()
        End If

    End Sub

    Public Sub read_and_valid_INI_Files_CES_PES()

        'CES vorbereiten
        'Erforderliche Dateien werden eingelesen
        '***************************************
        'Zielfunktionen einlesen
        Call Me.Read_OptZiele()
        'Kombinatorik Datei einlesen
        Call Me.Read_Kombinatorik()
        'Verzweigungs Datei einlesen
        Call Me.Read_Verzweigungen()
        '�berpr�fen der Kombinatorik
        Call Me.Validate_Combinatoric()
        'Pr�fen ob Kombinatorik und Verzweigungsdatei zusammenpassen
        Call Me.Validate_CES_fits_to_VER()
        'Datenbank vorbereiten

        'PES vorbereiten
        'Erforderliche Dateien werden eingelesen
        '***************************************
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
        '*********************
        If Me.Ergebnisdb = True Then
            Call Me.db_prepare()
            Call Me.db_prepare_CES_PES()
        End If
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
        ReDim List_OptParameter_Save(AnzParam - 1)

        'Zur�ck zum Dateianfang und lesen
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

        'OptParameter werden hier gesichert
        For i = 0 To List_OptParameter.GetUpperBound(0)
            Call copy_Struct_OptParemeter(List_OptParameter(i), List_OptParameter_Save(i))
        Next

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
        ReDim List_ModellParameter_Save(AnzParam - 1)

        'Zur�ck zum Dateianfang und lesen
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

        'ModellParameter werden hier gesichert
        For i = 0 To List_ModellParameter.GetUpperBound(0)
            Call copy_Struct_ModellParemeter(List_ModellParameter(i), List_ModellParameter_Save(i))
        Next

    End Sub

    'Optimierungsziele einlesen
    '**************************
    Protected Overridable Sub Read_OptZiele()

        Dim AnzZiele As Integer = 0
        Dim ext As String
        Dim i As Integer

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
            MsgBox("Die Anzahl der Ziele betr�gt mehr als 2!" & Chr(13) & Chr(10) _
                    & "Es werden nur die ersten beiden Zielfunktionen im Hauptdiagramm angezeigt!", MsgBoxStyle.Information, "Info")
        End If

        ReDim List_OptZiele(AnzZiele - 1)

        'Zur�ck zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        'Einlesen der Zeile und �bergeben an die OptimierungsZiele Liste
        Dim ZeilenArray(9) As String

        i = 0
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
                List_OptZiele(i).ZielReiheDatei = ZeilenArray(9).Trim()
                i += 1
            End If
        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

        'Falls mit Reihen verglichen werden soll werden hier die Reihen eingelesen
        Dim ZielStart As Date
        Dim ZielEnde As Date

        For i = 0 To AnzZiele - 1
            With List_OptZiele(i)
                If (.ZielTyp = "Reihe" Or .ZielTyp = "IHA") Then

                    'Dateiendung der Zielreihendatei bestimmen und Reihe einlesen
                    ext = Path.GetExtension(.ZielReiheDatei)
                    Select Case (ext.ToUpper)
                        Case ".WEL"
                            Dim WEL As New Wave.WEL(Me.WorkDir & .ZielReiheDatei, .ZielGr)
                            .ZielReihe = WEL.Read_WEL()(0)
                        Case ".ZRE"
                            Dim ZRE As New Wave.ZRE(Me.WorkDir & .ZielReiheDatei)
                            .ZielReihe = ZRE.Zeitreihe
                        Case ".PRB"
                            'BUG 136: geht nicht mehr, weil PRB-Dateien keine Zeitreihen sind!
                            'IsOK = Read_PRB(Me.WorkDir & .ZielReiheDatei, .ZielGr, .ZielReihe)
                        Case Else
                            Throw New Exception("Das Format der Zielreihe '" & .ZielReiheDatei & "' wurde nicht erkannt!")
                    End Select

                    'Zeitraum der Zielreihe �berpr�fen (nur bei WEL und ZRE)
                    '-------------------------------------------------------
                    If (ext.ToUpper = ".WEL" Or ext.ToUpper = ".ZRE") Then

                        ZielStart = .ZielReihe.XWerte(0)
                        ZielEnde = .ZielReihe.XWerte(.ZielReihe.Length - 1)

                        If (ZielStart > Me.SimStart Or ZielEnde < Me.SimEnde) Then
                            'Zielreihe deckt Simulationszeitraum nicht ab
                            Throw New Exception("Die Zielreihe '" & .ZielReiheDatei & "' deckt den Simulationszeitraum nicht ab!")
                        Else
                            'Zielreihe auf Simulationszeitraum k�rzen
                            Call .ZielReihe.cut(Me.SimStart, Me.SimEnde)
                        End If

                    End If

                    'Zielreihe umbenennen
                    .ZielReihe.Title += " (Referenz)"

                End If
            End With
        Next
    End Sub

#End Region 'Eingabedateien einlesen

#Region "Pr�fung der Eingabedateien"

    'Pr�ft ob .OPT und .MOD Dateien zusammenpassen
    '*********************************************
    Private Sub Validate_OPT_fits_to_MOD()
        Dim i, j As Integer
        Dim isValid_A As Boolean = True
        Dim isValid_B As Boolean = True
        Dim isValid As Boolean = False

        'A: Pr�fung ob f�r jeden OptParameter mindestens ein Modellparameter existiert
        For i = 0 To List_OptParameter.GetUpperBound(0)
            isValid = False
            For j = 0 To List_ModellParameter.GetUpperBound(0)
                If List_OptParameter(i).Bezeichnung = List_ModellParameter(j).OptParameter Then
                    isValid = True
                End If
            Next
            If isValid = False Then
                isValid_A = False
            End If
        Next

        'B: Pr�fung ob jeder ModellParameter einem richtigen OptParameter zugewiesen ist.
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
            Throw New Exception("F�r einen OptParameter ist kein Modellparameter vorhanden!")
        End If

        If Not isValid_B Then
            Throw New Exception("Ein Modellparameter ist keinem OptParameter zugewiesen!")
        End If

    End Sub

    'Validierungsfunktion der Kombinatorik Pr�ft ob Verbraucher an zwei Standorten Dopp vorhanden sind
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

    'Mehrere Pr�fungen ob die .VER Datei des BlueM und der .CES Datei auch zusammenpassen
    '************************************************************************************
    Public Sub Validate_CES_fits_to_VER()

        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim x As Integer = 0
        Dim y As Integer = 0

        Dim FoundA(VerzweigungsDatei.GetUpperBound(0)) As Boolean

        'Pr�ft ob jede Verzweigung einmal in der LocationList vorkommt
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

        'Pr�ft ob die nicht vorkommenden Verzweigungen Verzweigungen anderer Art sind
        For i = 0 To VerzweigungsDatei.GetUpperBound(0)
            If Not VerzweigungsDatei(i, 1) = "2" And FoundA(i) = False Then
                FoundA(i) = True
            End If
        Next

        Dim FoundB As Boolean = True
        Dim TmpBool As Boolean = False

        'Pr�ft ob alle in der LocationList Vorkommenden Verzweigungen auch in der Verzweigungsdatei sind
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

        '�bergabe
        If FoundB = False Then
            Throw New Exception(".VER und .CES Dateien passen nicht zusammen! Eine Verzweigung in der VER Datei kommt in der CES Datei nicht vor und ist nicht nicht vom Typ Prozentsatz (Kennung 2)")
        Else
            For i = 0 To FoundA.GetUpperBound(0)
                If FoundA(i) = False Then
                    Throw New Exception(".VER und .CES Dateien passen nicht zusammen! Eine in der CES Datei angegebene Verzeigung kommt in der VEr Datei nicht vor.")
                End If
            Next
        End If

    End Sub

#End Region 'Pr�fung der Eingabedateien

#Region "Kombinatorik"

    'Kombinatorik
    '############

    'Berechnet die Anzahl maximal m�glicher Kombinationen
    '****************************************************
    Public Function No_of_Combinations() As Integer
        Dim i As Integer

        No_of_Combinations = List_Locations(0).List_Massnahmen.GetLength(0)

        For i = 1 To List_Locations.GetUpperBound(0)
            No_of_Combinations = No_of_Combinations * List_Locations(i).List_Massnahmen.GetLength(0)
        Next

    End Function

    '�berpr�ft ob und welcher TestModus aktiv ist
    'Beschreibung:
    '********************************************
    Public Function Set_TestModus() As Integer

        Dim i, j As Integer
        Dim count_A As Integer
        Dim count_B As Integer
        Dim Bool As Boolean = False

        'Pr�ft auf den Modus "0" kein TestModus
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

        'Pr�ft aus Testen einer definierten Kombination
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

        'Pr�ft auf einmaliges Testen aller m�glichen Kombinationen
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

    'Holt sich im Falle des Testmodus 1 den Pfad aus der .CES Datei
    '**************************************************************
    Public Sub get_TestPath(ByRef Path() As Integer)
        Dim i, j, counter As Integer

        For i = 0 To Path.GetUpperBound(0)
            counter = 0
            For j = 0 To List_Locations(i).List_Massnahmen.GetUpperBound(0)
                If List_Locations(i).List_Massnahmen(j).TestModus = 1 Then
                    Path(i) = counter
                End If
                counter += 1
            Next
        Next

    End Sub

    'Bereitet das SimModell f�r Kombinatorik Optimierung vor
    '*******************************************************
    Public Sub PREPARE_Evaluation_CES(ByVal Path() As Integer)

        'Setzt den Aktuellen Pfad
        Aktuell_Path = Path

        'Erstellt die aktuelle Bauerksliste und �bertr�gt sie zu SKos
        Call Prepare_aktuelle_Elemente()

        'Ermittelt die Namen der Locations
        Call Prepare_aktuelle_Measures()

        'Ermittelt das aktuelle_ON_OFF array
        Call Prepare_Verzweigung_ON_OFF()

        'Schreibt die neuen Verzweigungen
        Call Me.Prepere_Write_Verzweigungen()

    End Sub

    '*******************************************************
    Public Sub PREPARE_Evaluation_CES()

        'Wandelt die Ma�nahmen Namen wieder in einen Pfad zur�ck
        Dim i, j As Integer
        For i = 0 To Aktuell_Measure.GetUpperBound(0)
            For j = 0 To List_Locations(i).List_Massnahmen.GetUpperBound(0)
                If (List_Locations(i).List_Massnahmen(j).Name = Aktuell_Measure(i)) Then
                    Aktuell_Path(i) = j
                End If
            Next
        Next

        'Erstellt die aktuelle Bauerksliste und �bertr�gt sie zu SKos
        Call Prepare_aktuelle_Elemente()

        'Ermittelt das aktuelle_ON_OFF array
        Call Prepare_Verzweigung_ON_OFF()

        'Schreibt die neuen Verzweigungen
        Call Prepere_Write_Verzweigungen()

    End Sub


    'Die Liste mit den aktuellen Bauwerken des Kindes wird erstellt und in SKos geschrieben
    '**************************************************************************************
    Private Sub Prepare_aktuelle_Elemente()
        Dim i, j As Integer
        Dim No As Integer

        Dim x As Integer = 0
        For i = 0 To Aktuell_Path.GetUpperBound(0)
            No = Aktuell_Path(i)
            For j = 0 To List_Locations(i).List_Massnahmen(No).Bauwerke.GetUpperBound(0)
                Array.Resize(Aktuell_Elemente, x + 1)
                Aktuell_Elemente(x) = List_Locations(i).List_Massnahmen(No).Bauwerke(j)
                x += 1
            Next
        Next

        'Entfernt die X Eintr�ge
        Call SKos1.Remove_X(Aktuell_Elemente)

        'Kopiert die aktuelle ElementeListe in dieses Aktuell_Element Array
        ReDim SKos1.Aktuell_Elemente(Aktuell_Elemente.GetUpperBound(0))
        Array.Copy(Aktuell_Elemente, SKos1.Aktuell_Elemente, Aktuell_Elemente.GetLength(0))
    End Sub

    'Ermittelt die Namen der aktuellen Bauwerke
    '******************************************
    Private Sub Prepare_aktuelle_Measures()
        Dim i, j As Integer

        ReDim Aktuell_Measure(List_Locations.GetUpperBound(0))

        For i = 0 To List_Locations.GetUpperBound(0)
            For j = 0 To List_Locations(i).List_Massnahmen.GetUpperBound(0)
                If j = Aktuell_Path(i) Then
                    Aktuell_Measure(i) = List_Locations(i).List_Massnahmen(j).Name
                End If
            Next
        Next
    End Sub

    'Ermittelt das aktuelle Verzweigungsarray
    '****************************************
    Private Sub Prepare_Verzweigung_ON_OFF()
        Dim j, x, y, z As Integer
        Dim No As Short

        'Schreibt alle Bezeichnungen der Verzweigungen ins Array
        For j = 0 To VER_ONOFF.GetUpperBound(0)
            VER_ONOFF(j, 0) = VerzweigungsDatei(j, 0)
        Next
        'Weist die Werte das Pfades zu
        For x = 0 To Aktuell_Path.GetUpperBound(0)
            No = Aktuell_Path(x)
            For y = 0 To List_Locations(x).List_Massnahmen(No).Schaltung.GetUpperBound(0)
                For z = 0 To VER_ONOFF.GetUpperBound(0)
                    If List_Locations(x).List_Massnahmen(No).Schaltung(y, 0) = VER_ONOFF(z, 0) Then
                        VER_ONOFF(z, 1) = List_Locations(x).List_Massnahmen(No).Schaltung(y, 1)
                    End If
                Next
            Next
        Next

    End Sub

    'Schreibt die neuen Verzweigungen
    '********************************
    Protected MustOverride Sub Prepere_Write_Verzweigungen()

    'Evaluiert die Kinderchen f�r Kombinatorik Optimierung vor
    '*********************************************************
    Public Function SIM_Evaluierung_CES(ByRef Penalty As Double()) As Boolean
        Dim i As Short

        'Modell Starten
        Call launchSim()

        'Qualit�tswerte berechnen und R�ckgabe an den OptiAlgo
        For i = 0 To Penalty.GetUpperBound(0)                                 'BUG 57: QN() f�ngt bei 1 an!
            List_OptZiele(i).QWertTmp = QWert(List_OptZiele(i))
            Penalty(i) = List_OptZiele(i).QWertTmp
        Next

        'Qualit�tswerte und OptParameter in DB speichern
        If (Ergebnisdb = True) Then
            Call db_update(12, 15)
        End If

    End Function

#End Region 'Kombinatorik

#Region "Evaluierung"

    'Reduziert die OptParameter und die ModellParameter auf die aktiven Elemente
    '***************************************************************************
    Public Function Reduce_OptPara_ModPara() As Boolean
        Reduce_OptPara_ModPara = True
        Dim i As Integer

        'Kopieren der Listen aus den Sicherungen
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        ReDim List_ModellParameter(List_ModellParameter_Save.GetUpperBound(0))
        For i = 0 To List_ModellParameter_Save.GetUpperBound(0)
            copy_Struct_ModellParemeter(List_ModellParameter_Save(i), List_ModellParameter(i))
        Next
        ReDim List_OptParameter(List_OptParameter_Save.GetUpperBound(0))
        For i = 0 To List_OptParameter_Save.GetUpperBound(0)
            copy_Struct_OptParemeter(List_OptParameter_Save(i), List_OptParameter(i))
        Next

        'Reduzierung der ModParameter
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxx
        Dim j, count As Integer
        Dim TMP_ModPara() As Struct_ModellParameter
        ReDim TMP_ModPara(List_ModellParameter.GetUpperBound(0))

        count = 0
        For i = 0 To List_ModellParameter.GetUpperBound(0)
            For j = 0 To Aktuell_Elemente.GetUpperBound(0)
                If List_ModellParameter(i).Element = Aktuell_Elemente(j) Then
                    Call copy_Struct_ModellParemeter(List_ModellParameter(i), TMP_ModPara(count))
                    count += 1
                End If
            Next
        Next

        'Immer dann wenn nicht Nullvariante
        '**********************************
        If count = 0 Then
            Reduce_OptPara_ModPara = False
        Else
            Array.Resize(TMP_ModPara, count)
            Array.Resize(List_ModellParameter, count)

            For i = 0 To TMP_ModPara.GetUpperBound(0)
                Call copy_Struct_ModellParemeter(TMP_ModPara(i), List_ModellParameter(i))
            Next

            'Reduzierung der OptParameter
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxx
            Dim TMP_OptPara() As Struct_OptParameter
            ReDim TMP_OptPara(List_OptParameter.GetUpperBound(0))

            count = 0
            For i = 0 To List_OptParameter.GetUpperBound(0)
                For j = 0 To List_ModellParameter.GetUpperBound(0)
                    If List_OptParameter(i).Bezeichnung = List_ModellParameter(j).OptParameter Then
                        Call copy_Struct_OptParemeter(List_OptParameter(i), TMP_OptPara(count))
                        count += 1
                        j = List_ModellParameter.GetUpperBound(0)
                    End If
                Next
            Next

            If count = 0 Then
                Throw New Exception("Die aktuelle Kombination enth�lt keine Bauwerke, f�r die OptimierungsParameter vorliegen")
            End If

            Array.Resize(TMP_OptPara, count)
            Array.Resize(List_OptParameter, count)

            For i = 0 To TMP_OptPara.GetUpperBound(0)
                Call copy_Struct_OptParemeter(TMP_OptPara(i), List_OptParameter(i))
            Next

        End If

    End Function

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

    'Kopiert ein Strukt_OptParameter
    '**********************************
    Private Sub copy_Struct_OptParemeter(ByVal Source As Struct_OptParameter, ByRef Destination As Struct_OptParameter)

        Destination.Bezeichnung = Source.Bezeichnung
        Destination.Einheit = Source.Einheit
        Destination.Wert = Source.Wert
        Destination.Min = Source.Min
        Destination.Max = Source.Max
        Destination.SKWert = Source.SKWert

    End Sub

    'EVO-Parameter�bergabe
    '*********************
    Public Sub Parameter_Uebergabe(ByRef globalAnzPar As Short, ByRef globalAnzZiel As Short, ByRef globalAnzRand As Short, ByRef mypara(,) As Double)

        Dim i As Integer

        'Anzahl Optimierungsparameter �bergeben
        globalAnzPar = Me.List_OptParameter.GetLength(0)

        'Parameterwerte �bergeben
        'BUG 57: mypara() f�ngt bei 1 an!
        ReDim mypara(globalAnzPar, 1)
        For i = 1 To globalAnzPar
            mypara(i, 1) = Me.List_OptParameter(i - 1).SKWert
        Next

        'globale Anzahl der Ziele muss hier auf L�nge der Zielliste gesetzt werden
        globalAnzZiel = Me.List_OptZiele.GetLength(0)

        'TODO: Randbedingungen
        globalAnzRand = 2

    End Sub

    'Evaluierung des SimModells f�r ParameterOptimierung - Steuerungseinheit
    '***********************************************************************
    Public Sub PREPARE_Evaluation_PES(ByVal mypara As Double(,))

        Dim i As Short

        'Mutierte Parameter an OptParameter �bergeben
        For i = 0 To Me.List_OptParameter.GetUpperBound(0)          'BUG 57: mypara(,) f�ngt bei 1 an!
            List_OptParameter(i).SKWert = mypara(i + 1, 1)          'OptParameterListe(i+1) weil Array bei 0 anf�ngt!
        Next

        'Mutierte Parameter in Eingabedateien schreiben
        Call Prepare_Write_ModellParameter()

    End Sub


    'Die ModellParameter in die Eingabedateien des SimModells schreiben
    '******************************************************************
    Public Sub Prepare_Write_ModellParameter()
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
            'Datei �ffnen
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

            'Zeile �ndern
            Zeile = Zeilenarray(List_ModellParameter(i).ZeileNr - 1)
            'BUG 120: richtig w�re: Length = SpBis - SpVon + 1
            Dim Length As Short = List_ModellParameter(i).SpBis - List_ModellParameter(i).SpVon
            StrLeft = Microsoft.VisualBasic.Left(Zeile, List_ModellParameter(i).SpVon - 1)
            StrRight = Microsoft.VisualBasic.Right(Zeile, Len(Zeile) - List_ModellParameter(i).SpBis + 1)

            Wert = List_ModellParameter(i).Wert.ToString()
            If (Wert.Length > Length) Then
                'TODO: Parameter wird f�r erforderliche Stringl�nge einfach abgeschnitten, sollte aber gerundet werden!
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

    'Evaluierung des SimModells f�r ParameterOptimierung - Steuerungseinheit
    '***********************************************************************
    Public Function SIM_Evaluierung_PES(ByVal iEvaluierung As Integer, ByVal ipop As Short, ByRef QN As Double()) As Boolean

        Dim i As Short

        SIM_Evaluierung_PES = False

        'Modell Starten
        If Not launchSim() Then Exit Function

        'Qualit�tswerte berechnen und R�ckgabe an den OptiAlgo
        For i = 0 To Me.List_OptZiele.GetUpperBound(0)              'BUG 57: QN() f�ngt bei 1 an!
            List_OptZiele(i).QWertTmp = QWert(List_OptZiele(i))
            QN(i + 1) = List_OptZiele(i).QWertTmp                   'QN(i+1) weil Array bei 0 anf�ngt!
        Next

        'Qualit�tswerte und OptParameter in DB speichern
        If (Ergebnisdb = True) Then
            Call db_update(iEvaluierung, ipop)
        End If

        SIM_Evaluierung_PES = True

    End Function


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


    'SimModell ausf�hren (simulieren)
    '********************************
    Public MustOverride Function launchSim() As Boolean

#End Region 'Evaluierung

#Region "Qualit�tswertberechnung"

    'Qualit�tswertberechnung
    '#######################

    'Berechnung des Qualit�tswerts (Zielwert)
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
                'BUG 138: PRB geht nicht, weil keine Zeitreihe
                Throw New Exception("PRB als OptZiel geht z.Zt. nicht (siehe Bug 138)")
                'QWert = QWert_PRB(OptZiel)

            Case Else
                'es wurde eine nicht unterst�tzte Ergebnisdatei angegeben
                IsOK = False

        End Select

        If (IsOK = False) Then
            'TODO: Fehlerbehandlung
        End If

    End Function

    'Qualit�tswert aus WEL-Datei
    '***************************
    Protected Overridable Function QWert_WEL(ByVal OptZiel As Struct_OptZiel) As Double

        Dim QWert As Double

        'Simulationsergebnis auslesen
        Dim SimReihe As New Wave.Zeitreihe(OptZiel.SimGr)
        Dim WEL As New Wave.WEL(WorkDir & Datensatz & ".wel", OptZiel.SimGr)
        SimReihe = WEL.Read_WEL()(0)

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

    'Qualit�tswert berechnen: Zieltyp = Reihe
    '****************************************
    'BUG 105: Konstante und gleiche Zeitschrittweiten vorausgesetzt!
    Protected Function QWert_Reihe(ByVal OptZiel As Struct_OptZiel, ByVal SimReihe As Wave.Zeitreihe) As Double

        Dim QWert As Double
        Dim i As Integer

        'Fallunterscheidung Zielfunktion
        '-------------------------------
        Select Case OptZiel.ZielFkt

            Case "AbQuad"
                'Summe der Fehlerquadrate
                For i = 0 To SimReihe.Length - 1
                    QWert += (OptZiel.ZielReihe.YWerte(i) - SimReihe.YWerte(i)) * (OptZiel.ZielReihe.YWerte(i) - SimReihe.YWerte(i))
                Next

            Case "Diff"
                'Summe der Fehler
                For i = 0 To SimReihe.Length - 1
                    QWert += Math.Abs(OptZiel.ZielReihe.YWerte(i) - SimReihe.YWerte(i))
                Next

            Case "Volf"
                'Volumenfehler
                'BUG 104: Volumenfehler rechnet noch nicht echtes Volumen, dazu ist Zeitschrittweite notwendig
                Dim VolSim As Double = 0
                Dim VolZiel As Double = 0
                For i = 0 To SimReihe.Length - 1
                    VolSim += SimReihe.YWerte(i)
                    VolZiel += OptZiel.ZielReihe.YWerte(i)
                Next
                QWert = Math.Abs(VolZiel - VolSim)

            Case "nUnter"
                'Relative Anzahl der Zeitschritte mit Unterschreitungen (in Prozent)
                Dim nUnter As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) < OptZiel.ZielReihe.YWerte(i)) Then
                        nUnter += 1
                    End If
                Next
                QWert = nUnter / SimReihe.Length * 100

            Case "n�ber"
                'Relative Anzahl der Zeitschritte mit �berschreitungen (in Prozent)
                Dim nUeber As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) > OptZiel.ZielReihe.YWerte(i)) Then
                        nUeber += 1
                    End If
                Next
                QWert = nUeber / SimReihe.Length * 100

            Case Else
                Throw New Exception("Die Zielfunktion '" & OptZiel.ZielFkt & "' wird nicht unterst�tzt!")

        End Select

        Return QWert

    End Function

    'Qualit�tswert berechnen: Zieltyp = Wert
    '***************************************
    Protected Function QWert_Wert(ByVal OptZiel As Struct_OptZiel, ByVal SimReihe As Wave.Zeitreihe) As Double

        Dim QWert As Double
        Dim i As Integer

        'Wert aus Simulationsergebnis berechnen
        '--------------------------------------
        Dim SimWert As Single

        Select Case OptZiel.WertTyp

            Case "MaxWert"
                SimWert = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) > SimWert) Then
                        SimWert = SimReihe.YWerte(i)
                    End If
                Next

            Case "MinWert"
                SimWert = 999999999999999999
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) < SimWert) Then
                        SimWert = SimReihe.YWerte(i)
                    End If
                Next

            Case "Average"
                SimWert = 0
                For i = 0 To SimReihe.Length - 1
                    SimWert += SimReihe.YWerte(i)
                Next
                SimWert = SimWert / SimReihe.Length

            Case "AnfWert"
                SimWert = SimReihe.YWerte(0)

            Case "EndWert"
                SimWert = SimReihe.YWerte(SimReihe.Length - 1)

            Case Else
                Throw New Exception("Der Werttyp '" & OptZiel.WertTyp & "' wird nicht unterst�tzt!")

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
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) < OptZiel.ZielWert) Then
                        nUnter += 1
                    End If
                Next
                QWert = nUnter / SimReihe.Length * 100

            Case "n�ber"
                'Relative Anzahl der Zeitschritte mit �berschreitungen (in Prozent)
                Dim nUeber As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) > OptZiel.ZielWert) Then
                        nUeber += 1
                    End If
                Next
                QWert = nUeber / SimReihe.Length * 100

            Case Else
                Throw New Exception("Die Zielfunktion '" & OptZiel.ZielFkt & "' wird f�r Werte nicht unterst�tzt!")

        End Select

        Return QWert

    End Function

    'Qualit�tswert aus PRB-Datei
    '***************************
    Private Function QWert_PRB(ByVal OptZiel As Struct_OptZiel) As Double

        'BUG 138: PRB geht nicht, weil keine Zeitreihe
        'Dim i As Integer
        'Dim IsOK As Boolean
        'Dim QWert As Double
        'Dim SimReihe As Object(,) = {}

        ''Simulationsergebnis auslesen
        'IsOK = Read_PRB(WorkDir & Datensatz & ".PRB", OptZiel.SimGr, SimReihe)

        ''Diff
        ''----
        ''�berfl�ssige St�tzstellen (P) entfernen
        ''---------------------------------------
        ''Anzahl St�tzstellen bestimmen
        'Dim stuetz As Integer = 0
        'Dim P_vorher As Double = -99
        'For i = 0 To SimReihe.GetUpperBound(0)
        '    If (i = 0 Or Not SimReihe(i, 1) = P_vorher) Then
        '        stuetz += 1
        '        P_vorher = SimReihe(i, 1)
        '    End If
        'Next
        ''Werte in neues Array schreiben
        'Dim PRBtmp(stuetz, 1) As Object
        'stuetz = 0
        'For i = 0 To SimReihe.GetUpperBound(0)
        '    If (i = 0 Or Not SimReihe(i, 1) = P_vorher) Then
        '        PRBtmp(stuetz, 0) = SimReihe(i, 0)
        '        PRBtmp(stuetz, 1) = SimReihe(i, 1)
        '        P_vorher = SimReihe(i, 1)
        '        stuetz += 1
        '    End If
        'Next
        ''Reihe um eine St�tzstelle erweitern
        ''PRBtmp(stuetz, 0) = PRBtmp(stuetz - 1, 0)
        ''PRBtmp(stuetz, 1) = PRBtmp(stuetz - 1, 1)

        ''An St�tzstellen der ZielReihe interpolieren
        ''-------------------------------------------
        'Dim PRBintp(OptZiel.ZielReihe.GetUpperBound(0), 1) As Object
        'Dim j As Integer
        'For i = 0 To OptZiel.ZielReihe.GetUpperBound(0)
        '    'zugeh�rige Lamelle in SimReihe finden
        '    j = 0
        '    Do While (PRBtmp(j, 1) < OptZiel.ZielReihe(i, 1))
        '        j += 1
        '    Loop
        '    'interpolieren
        '    PRBintp(i, 0) = (PRBtmp(j + 1, 0) - PRBtmp(j, 0)) / (PRBtmp(j + 1, 1) - PRBtmp(j, 1)) * (OptZiel.ZielReihe(i, 1) - PRBtmp(j, 1)) + PRBtmp(j, 0)
        '    PRBintp(i, 1) = OptZiel.ZielReihe(i, 1)
        'Next

        'For i = 0 To OptZiel.ZielReihe.GetUpperBound(0)
        '    QWert += Math.Abs(OptZiel.ZielReihe(i, 0) - PRBintp(i, 0))
        'Next

        'Return QWert

    End Function

#End Region 'Qualit�tswertberechnung

#Region "SimErgebnisse lesen"

    'SimErgebnisse lesen
    '###################

    'Ein Ergebnis aus einer PRB-Datei einlesen
    '*****************************************
    Public Shared Function Read_PRB(ByVal DateiPfad As String, ByVal ZielGr As String, ByRef PRB(,) As Object) As Boolean

        Dim ZeileStart As Integer = 0
        Dim AnzZeil As Integer = 26                   'Anzahl der Zeilen ist immer 26, definiert durch MAXSTZ in BM
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

        'Zeile mit Spalten�berschriften �berspringen
        Zeile = StrRead.ReadLine.ToString

        For j = 0 To AnzZeil - 1
            Zeile = StrRead.ReadLine.ToString()
            PRB(j, 0) = Convert.ToDouble(Zeile.Substring(2, 10))        'X-Wert
            PRB(j, 1) = Convert.ToDouble(Zeile.Substring(13, 8))        'P(Jahr)
        Next

        StrRead.Close()
        FiStr.Close()

        '�berfl�ssige St�tzstellen (P) entfernen
        '---------------------------------------
        'Anzahl St�tzstellen bestimmen
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

    'Methoden f�r die Ergebnisdatenbank
    '##################################

    'Ergebnisdatenbank vorbereiten
    '*****************************
    Private Sub db_prepare()

        'Leere/Neue Ergebnisdatenbank in Arbeitsverzeichnis kopieren
        '-----------------------------------------------------------
        Dim ZielDatei As String = WorkDir & Datensatz & "_EVO.mdb"

        'aktuelles Verzeichnis bestimmen
        Dim currentDir As String = CurDir()
        'Pfad zur Assembly bestimmen (\_Main\bin\)
        Dim binpath As String = System.Windows.Forms.Application.StartupPath()
        'in das \_Main\bin Verzeichnis wechseln
        ChDrive(binpath)
        ChDir(binpath)
        'in das \Apps Verzeichnis wechseln
        ChDir("..\Apps")
        'Datei kopieren
        My.Computer.FileSystem.CopyFile("EVO.mdb", ZielDatei, True)
        'zur�ck in das Ausgangsverzeichnis wechseln
        ChDrive(currentDir)
        ChDir(currentDir)

        'Pfad setzen
        db_path = ZielDatei

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

        Call db_disconnect()

    End Sub

    'Ergebnisdatenbank f�r PES vorbereiten
    '*************************************
    Private Sub db_prepare_PES()

        Call db_connect()
        Dim command As OleDbCommand = New OleDbCommand("", db)

        'Tabelle 'OptParameter'
        'Spalten festlegen:
        Dim fieldnames As String = ""
        Dim i As Integer

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

    'Ergebnisdatenbank f�r CES vorbereiten
    '*************************************
    Private Sub db_prepare_CES()

        Call db_connect()
        Dim command As OleDbCommand = New OleDbCommand("", db)

        'Tabelle 'Pfad'
        'Spalten festlegen:
        Dim fieldnames As String = ""
        Dim i As Integer

        command.CommandText = "ALTER TABLE Pfad ADD COLUMN 'QWert_ID' INTEGER"
        command.ExecuteNonQuery()

        For i = 0 To Me.List_Locations.GetUpperBound(0)
            If (i > 0) Then
                fieldnames &= ", "
            End If
            fieldnames &= Me.List_Locations(i).Name & " TEXT"
        Next
        'Tabelle anpassen
        command.CommandText = "ALTER TABLE Pfad ADD COLUMN " & fieldnames
        command.ExecuteNonQuery()

        Call db_disconnect()

    End Sub

    'Ergebnisdatenbank f�r CES & PES vorbereiten
    '*******************************************
    Private Sub db_prepare_CES_PES()

        Call db_connect()
        Dim command As OleDbCommand = New OleDbCommand("", db)

        'Tabelle 'OptParameter'
        'Spalten festlegen:
        Dim fieldnames As String = ""
        Dim i As Integer

        For i = 0 To List_OptParameter.GetUpperBound(0)
            If (i > 0) Then
                fieldnames &= ", "
            End If
            fieldnames &= "'" & List_OptParameter(i).Bezeichnung & "' DOUBLE"
        Next
        'Tabelle anpassen
        command.CommandText = "ALTER TABLE OptParameter ADD COLUMN " & fieldnames
        command.ExecuteNonQuery()

        'Tabelle 'Pfad'
        'Spalten festlegen:
        fieldnames = ""

        For i = 0 To Me.List_Locations.GetUpperBound(0)
            If (i > 0) Then
                fieldnames &= ", "
            End If
            fieldnames &= Me.List_Locations(i).Name & " TEXT"
        Next
        'Tabelle anpassen
        command.CommandText = "ALTER TABLE Pfad ADD COLUMN " & fieldnames
        command.ExecuteNonQuery()

        Call db_disconnect()

    End Sub


    'Mit Ergebnisdatenbank verbinden
    '*******************************
    Private Sub db_connect()
        Dim ConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Me.db_path
        db = New OleDb.OleDbConnection(ConnectionString)
        db.Open()
    End Sub

    'Verbindung zu Ergebnisdatenbank schlie�en
    '*****************************************
    Private Sub db_disconnect()
        db.Close()
    End Sub

    'Update der ErgebnisDB mit QWerten und OptParametern
    '***************************************************
    Public Function db_update(ByVal iEvaluierung As Integer, ByVal ipop As Short) As Boolean
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
        command.CommandText = "INSERT INTO QWerte (durchlauf, ipop " & fieldnames & ") VALUES (" & iEvaluierung & ", " & ipop & fieldvalues & ")"
        command.ExecuteNonQuery()
        'ID des zuletzt geschriebenen QWerts holen
        command.CommandText = "SELECT @@IDENTITY AS ID"
        Dim QWert_ID As Integer = command.ExecuteScalar()

        Select Case Me.Method

            Case "PES"
                'Zugeh�rige OptParameter schreiben
                fieldnames = ""
                fieldvalues = ""
                For i = 0 To List_OptParameter.GetUpperBound(0)
                    fieldnames &= ", '" & List_OptParameter(i).Bezeichnung & "'"
                    fieldvalues &= ", " & List_OptParameter(i).Wert
                Next
                command.CommandText = "INSERT INTO OptParameter (QWert_ID" & fieldnames & ") VALUES (" & QWert_ID & fieldvalues & ")"
                command.ExecuteNonQuery()

            Case "CES"

                'Zugeh�rigen Pfad schreiben
                fieldnames = ""
                fieldvalues = ""
                For i = 0 To Me.List_Locations.GetUpperBound(0)
                    fieldnames &= ", " & Me.List_Locations(i).Name
                    fieldvalues &= ", '" & Me.Aktuell_Measure(i) & "'"
                Next
                command.CommandText = "INSERT INTO Pfad ('QWert_ID'" & fieldnames & ") VALUES (" & QWert_ID & fieldvalues & ")"
                command.ExecuteNonQuery()


            Case "CES + PES"

                'OptParameter
                '------------

                'Zugeh�rige OptParameter schreiben
                fieldnames = ""
                fieldvalues = ""
                For i = 0 To List_OptParameter.GetUpperBound(0)
                    fieldnames &= ", '" & List_OptParameter(i).Bezeichnung & "'"
                    fieldvalues &= ", " & List_OptParameter(i).Wert
                Next
                command.CommandText = "INSERT INTO OptParameter (QWert_ID" & fieldnames & ") VALUES (" & QWert_ID & fieldvalues & ")"
                command.ExecuteNonQuery()

                'ID des zuletzt geschriebenen OptParameter holen
                command.CommandText = "SELECT @@IDENTITY AS ID"
                Dim OptParam_ID As Integer = command.ExecuteScalar()

                'Pfad
                '----

                '�berpr�fen, ob der Pfad schon existiert
                Dim Pfad_ID As Integer
                Dim condition As String = ""
                For i = 0 To Me.List_Locations.GetUpperBound(0)
                    If (i > 0) Then
                        condition &= " AND "
                    End If
                    condition &= Me.List_Locations(i).Name & " = '" & Me.Aktuell_Measure(i) & "'"
                Next
                command.CommandText = "SELECT ID FROM Pfad WHERE (" & condition & ")"
                If (Not IsNothing(command.ExecuteScalar())) Then
                    'Pfad_ID �bernehmen
                    Pfad_ID = command.ExecuteScalar()
                Else
                    'Neuen Pfad einf�gen
                    fieldnames = ""
                    fieldvalues = ""
                    For i = 0 To Me.List_Locations.GetUpperBound(0)
                        If (i > 0) Then
                            fieldnames &= ","
                            fieldvalues &= ","
                        End If
                        fieldnames &= " " & Me.List_Locations(i).Name
                        fieldvalues &= " '" & Me.Aktuell_Measure(i) & "'"
                    Next
                    command.CommandText = "INSERT INTO Pfad (" & fieldnames & ") VALUES (" & fieldvalues & ")"
                    command.ExecuteNonQuery()

                    'ID des zuletzt geschriebenen Pfads holen
                    command.CommandText = "SELECT @@IDENTITY AS ID"
                    Pfad_ID = command.ExecuteScalar()
                End If

                'Verkn�pfung
                '-----------

                'Verkn�pfung zwischen OptParameter und Pfad schreiben
                command.CommandText = "INSERT INTO Rel_Pfad_OptParameter (Pfad_ID, OptParameter_ID) VALUES (" & Pfad_ID & ", " & OptParam_ID & ")"
                command.ExecuteNonQuery()

        End Select

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

        'Parametersatz �bergeben
        For i As Integer = 0 To Me.List_OptParameter.GetUpperBound(0)

            With Me.List_OptParameter(i)
                .Wert = ds.Tables("OptParameter").Rows(0).Item("'" & .Bezeichnung & "'")
            End With

        Next

        Call db_disconnect()

    End Sub

    'Einen Pfad auslesen
    '*******************
    Public Sub db_getPfad(ByVal id As Integer)

        Call db_connect()

        Dim q As String = "SELECT * FROM Pfad WHERE ID = " & id

        Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(q, db)

        Dim ds As New DataSet("EVO")
        adapter.Fill(ds, "Pfad")

        'Parametersatz �bergeben
        For i As Integer = 0 To Me.Aktuell_Measure.GetUpperBound(0)

            Aktuell_Measure(i) = ds.Tables("Pfad").Rows(0).Item(List_Locations(i).Name)

        Next

        Call db_disconnect()

    End Sub

    'Erstmal die DB ID aus den Qualit�tswertn holen
    Public Function db_get_ID_QWert(ByVal xWert As Double, ByVal yWert As Double) As Integer

        Call db_connect()

        Dim q As String = "SELECT ID FROM QWerte WHERE ['" & List_OptZiele(0).Bezeichnung & "']=" & xWert & " AND ['" & List_OptZiele(1).Bezeichnung & "']=" & yWert

        Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(q, db)

        Dim ds As New DataSet("EVO")
        adapter.Fill(ds, "QWerteID")

        'Parametersatz �bergeben

        db_get_ID_QWert = ds.Tables("QWerteID").Rows(0).Item("ID")

        Call db_disconnect()

    End Function

    'Erstmal die DB ID aus den Qualit�tswertn holen
    Public Function db_get_ID_Pfad(ByVal QWert_ID as Integer) As Integer

        Call db_connect()

        Dim q As String = "SELECT Pfad_ID FROM Rel_Pfad_OptParameter WHERE OptParameter_ID=" & QWert_ID

        Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(q, db)

        Dim ds As New DataSet("EVO")
        adapter.Fill(ds, "PfadID")

        'Parametersatz �bergeben

        db_get_ID_Pfad = ds.Tables("PfadID").Rows(0).Item("Pfad_ID")

        Call db_disconnect()

    End Function

    'QWerte aus einer DB lesen
    '*************************
    Public Function db_readQWerte() As Collection

        'Connect
        Call db_connect()

        'Read
        Dim q As String = "SELECT * FROM QWerte ORDER BY ID"

        Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(q, db)

        Dim ds As New DataSet("QWerte")
        adapter.Fill(ds, "QWerte")

        'Disconnect
        Call db_disconnect()

        'Werte einlesen
        '--------------
        Dim seriesCollection As New Collection
        'Schleife �ber Spalten (Zielfunktionen fangen erst bei 3 an!)
        For i As Integer = 3 To ds.Tables("QWerte").Columns.Count - 1
            Dim tmpserie As Serie = New Serie(ds.Tables("QWerte").Columns(i).Caption, ds.Tables("QWerte").Rows.Count)
            'Schleife �ber Reihen
            For j As Integer = 0 To ds.Tables("QWerte").Rows.Count - 1
                tmpserie.values(j) = ds.Tables("QWerte").Rows(j).Item(i)
            Next
            'Serie zu Collection hinzuf�gen
            seriesCollection.Add(tmpserie, tmpserie.name)
        Next

        Return seriesCollection

    End Function

#End Region 'Ergebnisdatenbank

#End Region 'Methoden

End Class

'zus�tzliche Klassen
'###################

'Klasse Serie 
'enth�lt einen Namen und eine Liste von Werten
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


