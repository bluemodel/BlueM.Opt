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
'**** Letzte Änderung: Juli 2007                                            ****
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
    Public WorkDir As String                             'Arbeitsverzeichnis für das Blaue Modell
    Public Event WorkDirChange()                         'Event für Änderung des Arbeitsverzeichnisses

    Public SimStart As DateTime                          'Anfangsdatum der Simulation
    Public SimEnde As DateTime                           'Enddatum der Simulation
    Public SimDT As TimeSpan                             'Zeitschrittweite der Simulation

    'Konstanten
    '----------
    Public Const OptParameter_Ext As String = "OPT"      'Erweiterung der Datei mit den Optimierungsparametern (*.OPT)
    Public Const ModParameter_Ext As String = "MOD"      'Erweiterung der Datei mit den Modellparametern (*.MOD)
    Public Const OptZiele_Ext As String = "ZIE"          'Erweiterung der Datei mit den Zielfunktionen (*.ZIE)
    Public Const Constraints_Ext As String = "CON"       'Erweiterung der Datei mit den Constraints (*.CON)
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
    Public List_OptParameter_Save() As Struct_OptParameter = {} 'Liste der Optimierungsparameter die nicht verändert wird

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
    Public List_ModellParameter_Save() As Struct_ModellParameter = {} 'Liste der Modellparameter die nicht verändert wird

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
        Public ZielReiheDatei As String             'Der Dateiname der Zielreihe
        Public ZielGr As String                     'Spalte der .wel Datei falls ZielReihe .wel Datei ist
        Public ZielReihe As Wave.Zeitreihe          'Die Werte der Zielreihe
        Public QWertTmp As Double                   'Qualitätswert der letzten Simulation wird hier zwischengespeichert 
        Public Overrides Function toString() As String
            Return Bezeichnung
        End Function
    End Structure

    Public List_OptZiele() As Struct_OptZiel = {}   'Liste der Zielfunktionen

    'Constraints
    '-----------
    Public Structure Struct_Constraint
        Public Bezeichnung As String                'Bezeichnung
        Public GrenzTyp As String                   'Gibt an ob es sich um einen Wert oder um eine Reihe handelt
        Public Datei As String                      'Die Ergebnisdatei, aus der das Simulationsergebnis ausgelesen werden soll [WEL]
        Public SimGr As String                      'Die Simulationsgröße, die auf Verletzung der Grenze überprüft werden soll
        Public GrenzPos As String                   'Grenzposition (Ober-/Untergrenze)
        Public WertTyp As String                    'Gibt an wie der Wert, der mit dem Grenzwert verglichen werden soll, aus dem Simulationsergebnis berechnet werden soll
        Public GrenzWert As String                  'Der vorgegeben Grenzwert
        Public GrenzReiheDatei As String            'Der Dateiname der Grenzwertreihe
        Public GrenzGr As String                    'Spalte der .wel Datei falls Grenzwertreihe .wel Datei ist
        Public GrenzReihe As Wave.Zeitreihe         'Die Werte der Grenzwertreihe
        Public ConstTmp As Double                   'Constraintwert der letzten Simulation wird hier zwischengespeichert 
        Public Overrides Function toString() As String
            Return Bezeichnung
        End Function
    End Structure

    Public List_Constraints() As Struct_Constraint = {} 'Liste der Constraints

    'Ergebnisdatenbank
    '-----------------
    Public Ergebnisdb As Boolean = True             'Gibt an, ob die Ergebnisdatenbank geschrieben werden soll
    Public db_path As String                        'Pfad zur Ergebnisdatenbank
    Private db As OleDb.OleDbConnection

    'Kombinatorik
    '------------
    Protected SKos1 As New SKos()

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

#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

#Region "Initialisierung"

    'Konstruktor
    '***********
    Public Sub New()

        'Dezimaltrennzeichen überprüfen
        Call Me.checkDezimaltrennzeichen()

        'EVO.ini Datei einlesen
        Call Me.ReadSettings()

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

    'Benutzereinstellungen einlesen 
    '******************************
    Public Sub ReadSettings()

        'Datensatz
        '---------
        Dim pfad As String
        pfad = My.Settings.Datensatz
        Call Me.saveDatensatz(pfad)

    End Sub

    'Pfad zum Datensatz verarbeiten und speichern
    '********************************************
    Public Sub saveDatensatz(ByVal Pfad As String)

        If (File.Exists(Pfad)) Then
            'Datensatzname bestimmen
            Me.Datensatz = Path.GetFileNameWithoutExtension(Pfad)
            'Arbeitsverzeichnis bestimmen
            Me.WorkDir = Path.GetDirectoryName(Pfad) & "\"
            'Benutzereinstellungen speichern
            My.Settings.Datensatz = Pfad
        End If

        'Event auslösen (wird von Form1.displayWorkDir() verarbeitet)
        RaiseEvent WorkDirChange()

    End Sub

#End Region 'Initialisierung

#Region "Eingabedateien lesen"

    'PES vorbereiten (auch für SensiPlot)
    'Erforderliche Dateien werden eingelesen und DB vorbereitet
    '**********************************************************
    Public Sub read_and_valid_INI_Files_PES()

        'Simulationsdaten einlesen
        Call Me.Read_SimParameter()
        'Zielfunktionen einlesen
        Call Me.Read_OptZiele()
        'Constraints einlesen
        Call Me.Read_Constraints()
        'Optimierungsparameter einlesen
        Call Me.Read_OptParameter()
        'ModellParameter einlesen
        Call Me.Read_ModellParameter()
        'Modell-/Optparameter validieren
        Call Me.Validate_OPT_fits_to_MOD()
        'Prüfen der Anfangswerte
        Call Me.Validate_Startvalues()
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
        'Constraints einlesen
        Call Me.Read_Constraints()
        'Kombinatorik Datei einlesen
        Call Me.Read_Kombinatorik()
        'Verzweigungs Datei einlesen
        Call Me.Read_Verzweigungen()
        'Überprüfen der Kombinatorik
        Call Me.Validate_Combinatoric()
        'Prüfen ob Kombinatorik und Verzweigungsdatei zusammenpassen
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
        'Constraints einlesen
        Call Me.Read_Constraints()
        'Kombinatorik Datei einlesen
        Call Me.Read_Kombinatorik()
        'Verzweigungs Datei einlesen
        Call Me.Read_Verzweigungen()
        'Überprüfen der Kombinatorik
        Call Me.Validate_Combinatoric()
        'Prüfen ob Kombinatorik und Verzweigungsdatei zusammenpassen
        Call Me.Validate_CES_fits_to_VER()
        'Datenbank vorbereiten

        'PES vorbereiten
        'zusätzliche Dateien werden eingelesen
        '***************************************
        'Simulationsdaten einlesen
        Call Me.Read_SimParameter()
        'Optimierungsparameter einlesen
        Call Me.Read_OptParameter()
        'ModellParameter einlesen
        Call Me.Read_ModellParameter()
        'Modell-/Optparameter validieren
        Call Me.Validate_OPT_fits_to_MOD()
        'Prüfen der Anfangswerte
        Call Me.Validate_Startvalues()

        'Datenbank vorbereiten
        '*********************
        If Me.Ergebnisdb = True Then
            Call Me.db_prepare()
            Call Me.db_prepare_PES()
            Call Me.db_prepare_CES()
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

        'OptParameter werden hier gesichert
        For i = 0 To List_OptParameter.GetUpperBound(0)
            Call copy_Struct_OptParameter(List_OptParameter(i), List_OptParameter_Save(i))
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

        If (AnzZiele > 3) Then
            MsgBox("Die Anzahl der Ziele beträgt mehr als 3!" & Chr(13) & Chr(10) _
                    & "Es werden nur die ersten drei Zielfunktionen im Hauptdiagramm angezeigt!", MsgBoxStyle.Information, "Info")
        End If

        ReDim List_OptZiele(AnzZiele - 1)

        'Zurück zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        'Einlesen der Zeile und übergeben an die OptimierungsZiele Liste
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
                            'BUG 183: geht nicht mehr, weil PRB-Dateien keine Zeitreihen sind!
                            'IsOK = Read_PRB(Me.WorkDir & .ZielReiheDatei, .ZielGr, .ZielReihe)
                        Case Else
                            Throw New Exception("Das Format der Zielreihe '" & .ZielReiheDatei & "' wurde nicht erkannt!")
                    End Select

                    'Zeitraum der Zielreihe überprüfen (nur bei WEL und ZRE)
                    '-------------------------------------------------------
                    If (ext.ToUpper = ".WEL" Or ext.ToUpper = ".ZRE") Then

                        ZielStart = .ZielReihe.XWerte(0)
                        ZielEnde = .ZielReihe.XWerte(.ZielReihe.Length - 1)

                        If (ZielStart > Me.SimStart Or ZielEnde < Me.SimEnde) Then
                            'Zielreihe deckt Simulationszeitraum nicht ab
                            Throw New Exception("Die Zielreihe '" & .ZielReiheDatei & "' deckt den Simulationszeitraum nicht ab!")
                        Else
                            'Zielreihe auf Simulationszeitraum kürzen
                            Call .ZielReihe.cut(Me.SimStart, Me.SimEnde)
                        End If

                    End If

                    'Zielreihe umbenennen
                    .ZielReihe.Title += " (Referenz)"

                End If
            End With
        Next
    End Sub

    'Constraints einlesen
    '********************
    Private Sub Read_Constraints()

        Dim AnzConst As Integer = 0
        Dim ext As String
        Dim i As Integer

        Dim Datei As String = WorkDir & Datensatz & "." & Constraints_Ext

        If (File.Exists(Datei)) Then

            Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.Read)
            Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

            Dim Zeile As String = ""

            'Anzahl der Constraints feststellen
            Do
                Zeile = StrRead.ReadLine.ToString()
                If (Zeile.StartsWith("*") = False) Then
                    AnzConst += 1
                End If
            Loop Until StrRead.Peek() = -1

            ReDim List_Constraints(AnzConst - 1)

            'Zurück zum Dateianfang und lesen
            FiStr.Seek(0, SeekOrigin.Begin)

            'Einlesen der Zeile und übergeben an die Constraints Liste
            Dim ZeilenArray(9) As String

            i = 0
            Do
                Zeile = StrRead.ReadLine.ToString()
                If (Zeile.StartsWith("*") = False) Then
                    ZeilenArray = Zeile.Split("|")
                    'Werte zuweisen
                    List_Constraints(i).Bezeichnung = ZeilenArray(1).Trim()
                    List_Constraints(i).GrenzTyp = ZeilenArray(2).Trim()
                    List_Constraints(i).Datei = ZeilenArray(3).Trim()
                    List_Constraints(i).SimGr = ZeilenArray(4).Trim()
                    List_Constraints(i).GrenzPos = ZeilenArray(5).Trim()
                    List_Constraints(i).WertTyp = ZeilenArray(6).Trim()
                    List_Constraints(i).GrenzWert = ZeilenArray(7).Trim()
                    List_Constraints(i).GrenzGr = ZeilenArray(8).Trim()
                    List_Constraints(i).GrenzReiheDatei = ZeilenArray(9).Trim()
                    i += 1
                End If
            Loop Until StrRead.Peek() = -1

            StrRead.Close()
            FiStr.Close()

            'Kontrolle
            '---------
            For i = 0 To Me.List_Constraints.GetUpperBound(0)
                With Me.List_Constraints(i)
                    If (Not .GrenzTyp = "Wert" And Not .GrenzTyp = "Reihe") Then Throw New Exception("Constraints: GrenzTxyp muss entweder 'Wert' oder 'Reihe' sein!")
                    If (Not .Datei = "WEL") Then Throw New Exception("Constraints: Als Datei wird momentan nur 'WEL' unterstützt!")
                    If (Not .GrenzPos = "Obergrenze" And Not .GrenzPos = "Untergrenze") Then Throw New Exception("Constraints: Für Oben/Unten muss entweder 'Obergrenze' oder 'Untergrenze' angegeben sein!")
                End With
            Next

            'Falls mit Reihen verglichen werden soll werden hier die Reihen eingelesen
            Dim GrenzStart As Date
            Dim GrenzEnde As Date

            For i = 0 To AnzConst - 1
                With List_Constraints(i)
                    If (.GrenzTyp = "Reihe") Then

                        'Dateiendung der Grenzwertdatei bestimmen und Reihe einlesen
                        ext = Path.GetExtension(.GrenzReiheDatei)
                        Select Case (ext.ToUpper)
                            Case ".WEL"
                                Dim WEL As New Wave.WEL(Me.WorkDir & .GrenzReiheDatei, .GrenzGr)
                                .GrenzReihe = WEL.Read_WEL()(0)
                            Case ".ZRE"
                                Dim ZRE As New Wave.ZRE(Me.WorkDir & .GrenzReiheDatei)
                                .GrenzReihe = ZRE.Zeitreihe
                            Case Else
                                Throw New Exception("Das Format der Grenzwertreihe '" & .GrenzReiheDatei & "' wurde nicht erkannt!")
                        End Select

                        'Zeitraum der Grenzwertreihe überprüfen
                        '--------------------------------------
                        GrenzStart = .GrenzReihe.XWerte(0)
                        GrenzEnde = .GrenzReihe.XWerte(.GrenzReihe.Length - 1)

                        If (GrenzStart > Me.SimStart Or GrenzEnde < Me.SimEnde) Then
                            'Grenzwertreihe deckt Simulationszeitraum nicht ab
                            Throw New Exception("Die Grenzwertreihe '" & .GrenzReiheDatei & "' deckt den Simulationszeitraum nicht ab!")
                        Else
                            'Zielreihe auf Simulationszeitraum kürzen
                            Call .GrenzReihe.cut(Me.SimStart, Me.SimEnde)
                        End If

                        'Grenzwertreihe umbenennen
                        .GrenzReihe.Title += " (Grenze)"

                    End If
                End With
            Next

        Else
            'CON-Datei existiert nicht
            ReDim List_Constraints(-1)
        End If

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
            Throw New Exception(".VER und .CES Dateien passen nicht zusammen! Eine Verzweigung in der VER Datei kommt in der CES Datei nicht vor und ist nicht nicht vom Typ Prozentsatz (Kennung 2)")
        Else
            For i = 0 To FoundA.GetUpperBound(0)
                If FoundA(i) = False Then
                    Throw New Exception(".VER und .CES Dateien passen nicht zusammen! Eine in der CES Datei angegebene Verzeigung kommt in der VEr Datei nicht vor.")
                End If
            Next
        End If

    End Sub

    'Prüft ob der Startwert der OptPara in der .OPT innerhalb der Min und Max Grenzen liegt
    '**************************************************************************************
    Public Sub Validate_Startvalues()
        Dim i As Integer

        For i = 0 To List_OptParameter.GetUpperBound(0)
            If Not List_OptParameter(i).Wert <= List_OptParameter(i).Max Or Not List_OptParameter(i).Wert >= List_OptParameter(i).Min Then
                Throw New Exception("Der Optimierungsparameter " & List_OptParameter(i).Bezeichnung & " in der .OPT Datei liegt nicht innerhalb der dort genannten Grenzen.")
            End If
        Next
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

    'Die Elemente werden pro Location im Child gespeichert
    '*****************************************************
    Public Sub Identify_Measures_Elements_Parameters(ByVal No_Loc As Integer, ByVal No_Measure As Integer, ByRef Measure As String, ByRef Elements() As String, ByRef Para(,) As Object)

        Dim i, j As Integer
        Dim x As Integer

        '1. Die Maßnahme wird ermittelt
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        Measure = List_Locations(No_Loc).List_Massnahmen(No_Measure).Name
        'ToDo: Measure aktuell ist hier noch redundant!
        ReDim Preserve Akt.Measures(List_Locations.GetUpperBound(0))
        Akt.Measures(No_Loc) = Measure

        '2. Die Elemente werden Ermittelt
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        x = 0
        For i = 0 To List_Locations(No_Loc).List_Massnahmen(No_Measure).Bauwerke.GetUpperBound(0)
            If Not List_Locations(No_Loc).List_Massnahmen(No_Measure).Bauwerke(i) = "X" Then
                ReDim Preserve Elements(x)
                Elements(x) = List_Locations(No_Loc).List_Massnahmen(No_Measure).Bauwerke(i)
                x += 1
            End If
        Next

        'Kopiert die aktuelle ElementeListe in dieses Aktuell_Element Array
        'ToDo: sollte an eine bessere stelle!
        If No_Loc = 0 Then ReDim SKos1.Aktuell_Elemente(-1)
        ReDim Preserve SKos1.Aktuell_Elemente(SKos1.Aktuell_Elemente.GetUpperBound(0) + Elements.GetLength(0))
        Array.Copy(Elements, 0, SKos1.Aktuell_Elemente, SKos1.Aktuell_Elemente.GetUpperBound(0) - Elements.GetUpperBound(0), Elements.GetLength(0))

        '3. Die Parameter werden Ermittelt
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        x = 0
        For i = 0 To Elements.GetUpperBound(0)
            For j = 0 To List_OptParameter.GetUpperBound(0)
                If Elements(i) = Left(List_OptParameter(j).Bezeichnung, 4) Then
                    ReDim Preserve Para(1, x)
                    Para(0, x) = List_OptParameter(j).Bezeichnung
                    Para(1, x) = List_OptParameter(j).SKWert
                    x += 1
                End If
            Next
        Next
        If x = 0 Then ReDim Preserve Para(1, -1)

    End Sub



    'Struct und Methoden welche aktuellen Informationen zur Verfügung stellen
    '#########################################################################
    Public Structure Aktuell
        Public Path() As Integer
        Public Measures() As String
        Public VER_ONOFF(,) As Object
    End Structure

    Public Akt As Aktuell

    'Bereitet das SimModell für Kombinatorik Optimierung vor
    '*******************************************************
    Public Sub PREPARE_Evaluation_CES(ByVal Path() As Integer)

        'Setzt den Aktuellen Pfad
        Akt.Path = Path

        'Ermittelt das aktuelle_ON_OFF array
        Call Prepare_Verzweigung_ON_OFF()

        'Schreibt die neuen Verzweigungen
        Call Me.Write_Verzweigungen()

    End Sub

    'ToDo: nicht besonders hübsch überladen
    '*************************************
    Public Sub PREPARE_Evaluation_CES()

        'Wandelt die Maßnahmen Namen wieder in einen Pfad zurück
        Dim i, j As Integer
        For i = 0 To Akt.Measures.GetUpperBound(0)
            For j = 0 To List_Locations(i).List_Massnahmen.GetUpperBound(0)
                If (List_Locations(i).List_Massnahmen(j).Name = Akt.Measures(i)) Then
                    Akt.Path(i) = j
                End If
            Next
        Next

        'Ermittelt das aktuelle_ON_OFF array
        Call Prepare_Verzweigung_ON_OFF()

        'Schreibt die neuen Verzweigungen
        Call Write_Verzweigungen()

    End Sub

    'Ermittelt das aktuelle Verzweigungsarray
    '****************************************
    Private Sub Prepare_Verzweigung_ON_OFF()
        Dim j, x, y, z As Integer
        Dim No As Short

        'Schreibt alle Bezeichnungen der Verzweigungen ins Array
        For j = 0 To Akt.VER_ONOFF.GetUpperBound(0)
            Akt.VER_ONOFF(j, 0) = VerzweigungsDatei(j, 0)
        Next
        'Weist die Werte das Pfades zu
        For x = 0 To Akt.Path.GetUpperBound(0)
            No = Akt.Path(x)
            For y = 0 To List_Locations(x).List_Massnahmen(No).Schaltung.GetUpperBound(0)
                For z = 0 To Akt.VER_ONOFF.GetUpperBound(0)
                    If List_Locations(x).List_Massnahmen(No).Schaltung(y, 0) = Akt.VER_ONOFF(z, 0) Then
                        Akt.VER_ONOFF(z, 1) = List_Locations(x).List_Massnahmen(No).Schaltung(y, 1)
                    End If
                Next
            Next
        Next

    End Sub

    'Schreibt die neuen Verzweigungen
    '********************************
    Protected MustOverride Sub Write_Verzweigungen()

#End Region 'Kombinatorik

#Region "Evaluierung"

    'Reduziert die OptParameter und die ModellParameter auf die aktiven Elemente
    '!Wird jetzt aus den Elemten des Child generiert!
    '***************************************************************************
    Public Function Reduce_OptPara_and_ModPara(ByRef Elements() As String) As Boolean
        Reduce_OptPara_and_ModPara = True 'Wird wirklich abgefragt!
        Dim i As Integer

        'Kopieren der Listen aus den Sicherungen
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        ReDim List_ModellParameter(List_ModellParameter_Save.GetUpperBound(0))
        For i = 0 To List_ModellParameter_Save.GetUpperBound(0)
            copy_Struct_ModellParemeter(List_ModellParameter_Save(i), List_ModellParameter(i))
        Next
        ReDim List_OptParameter(List_OptParameter_Save.GetUpperBound(0))
        For i = 0 To List_OptParameter_Save.GetUpperBound(0)
            copy_Struct_OptParameter(List_OptParameter_Save(i), List_OptParameter(i))
        Next

        'Reduzierung der ModParameter
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxx
        Dim j, count As Integer
        Dim TMP_ModPara() As Struct_ModellParameter
        ReDim TMP_ModPara(List_ModellParameter.GetUpperBound(0))

        count = 0
        For i = 0 To List_ModellParameter.GetUpperBound(0)
            For j = 0 To Elements.GetUpperBound(0)
                If List_ModellParameter(i).Element = Elements(j) Then
                    Call copy_Struct_ModellParemeter(List_ModellParameter(i), TMP_ModPara(count))
                    count += 1
                End If
            Next
        Next

        'Immer dann wenn nicht Nullvariante
        '**********************************
        If count = 0 Then
            Reduce_OptPara_and_ModPara = False
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
                        Call copy_Struct_OptParameter(List_OptParameter(i), TMP_OptPara(count))
                        count += 1
                        j = List_ModellParameter.GetUpperBound(0)
                    End If
                Next
            Next

            'ToDo: Dieser Fall ist nur Relevant, wenn CES + PES sequentiell
            If count = 0 Then
                Throw New Exception("Die aktuelle Kombination enthält keine Bauwerke, für die OptimierungsParameter vorliegen")
            End If

            Array.Resize(TMP_OptPara, count)
            Array.Resize(List_OptParameter, count)

            For i = 0 To TMP_OptPara.GetUpperBound(0)
                Call copy_Struct_OptParameter(TMP_OptPara(i), List_OptParameter(i))
            Next

        End If

    End Function

    'Schreibt die passenden OptParameter für jede Location ins Child
    'ToDo alles ist da!
    '***************************************************************
    Dim n_tmp As Integer = 0

    'Funktion wahrscheinlich überflüssig
    Public Sub SaveParameter_to_Child(ByVal Loc As Integer, ByVal Measure As Integer, ByVal Parameter(,) As Object)

        Dim i, j As Integer
        Dim x As Integer = 0

        'Die Parameterliste wird auf die einzelnen Locations verteilt
        For i = 0 To List_OptParameter.GetUpperBound(0)
            For j = 0 To List_Locations(Loc).List_Massnahmen(Measure).Bauwerke.GetUpperBound(0)
                If Left(List_OptParameter(i).Bezeichnung, 4) = List_Locations(Loc).List_Massnahmen(Measure).Bauwerke(j) Then
                    Parameter(0, x) = List_OptParameter(i).Bezeichnung
                    Parameter(1, x) = List_OptParameter(i).SKWert
                    x += 1
                    ReDim Preserve Parameter(1, x)
                End If
            Next
        Next

        ReDim Preserve Parameter(1, x - 1)

        'Prüfung ob alle Parameter verteilt wurden
        n_tmp += Parameter.GetLength(1)

        If Loc = List_Locations.GetUpperBound(0) And Not List_OptParameter.GetLength(0) = n_tmp Then
            Throw New Exception("Die Zahl der Parameter in der OptParliste entspricht nicht der Zahl der Parameter des Faksimile")
        End If

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

    'Kopiert ein Strukt_OptParameter
    '**********************************
    Private Sub copy_Struct_OptParameter(ByVal Source As Struct_OptParameter, ByRef Destination As Struct_OptParameter)

        Destination.Bezeichnung = Source.Bezeichnung
        Destination.Einheit = Source.Einheit
        Destination.Wert = Source.Wert
        Destination.Min = Source.Min
        Destination.Max = Source.Max
        Destination.SKWert = Source.SKWert

    End Sub

    'EVO-Parameterübergabe die Standard Parameter werden aus den Listen der OptPara und OptZiele ermittelt
    '*****************************************************************************************************
    Public Sub Parameter_Uebergabe(ByRef globalAnzPar As Short, ByRef globalAnzZiel As Short, ByRef globalAnzRand As Short, ByRef mypara() As Double)

        Dim i As Integer

        'Anzahl Optimierungsparameter übergeben
        globalAnzPar = Me.List_OptParameter.GetLength(0)

        'Parameterwerte übergeben
        ReDim mypara(globalAnzPar - 1)
        For i = 0 To globalAnzPar  - 1
            mypara(i) = Me.List_OptParameter(i).SKWert
        Next

        'Anzahl Optimierungsziele übergeben
        globalAnzZiel = Me.List_OptZiele.GetLength(0)

        'Anzahl Randbedingungen übergeben
        globalAnzRand = Me.List_Constraints.GetLength(0)

    End Sub

    'Evaluierung des SimModells für ParameterOptimierung - Steuerungseinheit
    '***********************************************************************
    Public Sub PREPARE_Evaluation_PES(ByVal myPara() As Double)

        Dim i As Short

        'Mutierte Parameter an OptParameter übergeben
        For i = 0 To Me.List_OptParameter.GetUpperBound(0)
            List_OptParameter(i).SKWert = myPara(i)
        Next

        'Mutierte Parameter in Eingabedateien schreiben
        Call Write_ModellParameter()

    End Sub


    'Die ModellParameter in die Eingabedateien des SimModells schreiben
    '******************************************************************
    Public Sub Write_ModellParameter()
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
            'BUG 170: richtig wäre: Length = SpBis - SpVon + 1
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

    'Evaluiert die Kinderchen mit Hilfe des Simulationsmodells
    '*********************************************************
    Public Function SIM_Evaluierung(ByRef QN() As Double, ByRef RN() As Double) As Boolean

        Dim i As Short

        SIM_Evaluierung = False

        'Modell Starten
        If Not launchSim() Then Exit Function

        'Qualitätswerte berechnen
        For i = 0 To Me.List_OptZiele.GetUpperBound(0)
            List_OptZiele(i).QWertTmp = QWert(List_OptZiele(i))
            QN(i) = List_OptZiele(i).QWertTmp
        Next

        'Constraints berechnen
        For i = 0 To Me.List_Constraints.GetUpperBound(0)
            List_Constraints(i).ConstTmp = Constraint(List_Constraints(i))
            RN(i) = List_Constraints(i).ConstTmp
        Next

        'Lösung in DB speichern
        If (Ergebnisdb = True) Then
            Call Me.db_update()
        End If

        SIM_Evaluierung = True

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


    'SimModell ausführen (simulieren)
    '********************************
    Public MustOverride Function launchSim() As Boolean

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
                'BUG 220: PRB geht nicht, weil keine Zeitreihe
                Throw New Exception("PRB als OptZiel geht z.Zt. nicht (siehe Bug 138)")
                'QWert = QWert_PRB(OptZiel)

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

    'Qualitätswert berechnen: Zieltyp = Reihe
    '****************************************
    'BUG 218: Konstante und gleiche Zeitschrittweiten vorausgesetzt!
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
                Dim VolSim As Double = 0
                Dim VolZiel As Double = 0
                For i = 0 To SimReihe.Length - 1
                    VolSim += SimReihe.YWerte(i)
                    VolZiel += OptZiel.ZielReihe.YWerte(i)
                Next
                'Umrechnen in echtes Volumen
                VolSim *= Me.SimDT.TotalSeconds
                VolZiel *= Me.SimDT.TotalSeconds
                'Differenz bilden
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

            Case "sUnter"
                'Summe der Unterschreitungen
                Dim sUnter As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) < OptZiel.ZielReihe.YWerte(i)) Then
                        sUnter += OptZiel.ZielReihe.YWerte(i) - SimReihe.YWerte(i)
                    End If
                Next
                QWert = sUnter

            Case "nÜber"
                'Relative Anzahl der Zeitschritte mit Überschreitungen (in Prozent)
                Dim nUeber As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) > OptZiel.ZielReihe.YWerte(i)) Then
                        nUeber += 1
                    End If
                Next
                QWert = nUeber / SimReihe.Length * 100

            Case "sÜber"
                'Summe der Überschreitungen
                Dim sUeber As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) > OptZiel.ZielReihe.YWerte(i)) Then
                        sUeber += SimReihe.YWerte(i) - OptZiel.ZielReihe.YWerte(i)
                    End If
                Next
                QWert = sUeber

            Case Else
                Throw New Exception("Die Zielfunktion '" & OptZiel.ZielFkt & "' wird nicht unterstützt!")

        End Select

        Return QWert

    End Function

    'Qualitätswert berechnen: Zieltyp = Wert
    '***************************************
    Protected Function QWert_Wert(ByVal OptZiel As Struct_OptZiel, ByVal SimReihe As Wave.Zeitreihe) As Double

        Dim QWert As Double
        Dim i As Integer

        'Simulationswert aus Simulationsergebnis berechnen
        Dim SimWert As Double
        SimWert = SimReihe.getWert(OptZiel.WertTyp)

        'QWert berechnen
        '---------------
        'Fallunterscheidung Zielfunktion
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

            Case "nÜber"
                'Relative Anzahl der Zeitschritte mit Überschreitungen (in Prozent)
                Dim nUeber As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) > OptZiel.ZielWert) Then
                        nUeber += 1
                    End If
                Next
                QWert = nUeber / SimReihe.Length * 100

            Case Else
                Throw New Exception("Die Zielfunktion '" & OptZiel.ZielFkt & "' wird für Werte nicht unterstützt!")

        End Select

        Return QWert

    End Function

    'Qualitätswert aus PRB-Datei
    '***************************
    Private Function QWert_PRB(ByVal OptZiel As Struct_OptZiel) As Double

        'BUG 220: PRB geht nicht, weil keine Zeitreihe
        'Dim i As Integer
        'Dim IsOK As Boolean
        'Dim QWert As Double
        'Dim SimReihe As Object(,) = {}

        ''Simulationsergebnis auslesen
        'IsOK = Read_PRB(WorkDir & Datensatz & ".PRB", OptZiel.SimGr, SimReihe)

        ''Diff
        ''----
        ''Überflüssige Stützstellen (P) entfernen
        ''---------------------------------------
        ''Anzahl Stützstellen bestimmen
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
        ''Reihe um eine Stützstelle erweitern
        ''PRBtmp(stuetz, 0) = PRBtmp(stuetz - 1, 0)
        ''PRBtmp(stuetz, 1) = PRBtmp(stuetz - 1, 1)

        ''An Stützstellen der ZielReihe interpolieren
        ''-------------------------------------------
        'Dim PRBintp(OptZiel.ZielReihe.GetUpperBound(0), 1) As Object
        'Dim j As Integer
        'For i = 0 To OptZiel.ZielReihe.GetUpperBound(0)
        '    'zugehörige Lamelle in SimReihe finden
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

#End Region 'Qualitätswertberechnung

#Region "Constraintberechnung"

    'Constraint berechnen (Constraint < 0 ist Grenzverletzung)
    '****************************************************
    Public Function Constraint(ByVal constr As Struct_Constraint) As Double

        Dim i As Integer

        'Simulationsergebnis auslesen
        Dim SimReihe As New Wave.Zeitreihe(constr.SimGr)
        Dim WEL As New Wave.WEL(WorkDir & Datensatz & ".wel", constr.SimGr)
        SimReihe = WEL.Read_WEL()(0)

        'Fallunterscheidung GrenzTyp (Wert/Reihe)
        Select Case constr.GrenzTyp

            Case "Wert"
                'zuerst Simulationswert aus Simulationsergebnis berechnen
                Dim SimWert As Double
                SimWert = SimReihe.getWert(constr.WertTyp)

                'Grenzverletzung berechnen
                If (constr.GrenzPos = "Obergrenze") Then
                    Constraint = constr.GrenzWert - SimWert

                ElseIf (constr.GrenzPos = "Untergrenze") Then
                    Constraint = SimWert - constr.GrenzWert

                End If

            Case "Reihe"
                'BUG 112: TODO: Constraintberechnung bei einer Reihe!
                'Es wird die Summe der Grenzwertverletzungen verwendet
                Dim summe As Double = 0

                For i = 0 To SimReihe.Length - 1

                    If (constr.GrenzPos = "Obergrenze") Then
                        summe += Math.Min(constr.GrenzReihe.YWerte(i) - SimReihe.YWerte(i), 0)

                    ElseIf (constr.GrenzPos = "Untergrenze") Then
                        summe += Math.Min(SimReihe.YWerte(i) - constr.GrenzReihe.YWerte(i), 0)

                    End If

                Next

                Constraint = summe

        End Select

        Return Constraint

    End Function

#End Region 'Constraintberechnung

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

        'Pfad zur Vorlage
        Dim db_path_source As String = System.Windows.Forms.Application.StartupPath() & "\EVO.mdb"
        'Pfad zur Zieldatei
        Dim db_path_target As String = Me.WorkDir & Me.Datensatz & "_EVO.mdb"
        'Datei kopieren
        My.Computer.FileSystem.CopyFile(db_path_source, db_path_target, True)

        'Pfad setzen
        Me.db_path = db_path_target

        'Tabellen anpassen
        '=================
        Dim i As Integer

        Call db_connect()
        Dim command As OleDbCommand = New OleDbCommand("", db)

        'Tabelle 'QWerte'
        '----------------
        'Spalten festlegen:
        Dim fieldnames As String = ""
        For i = 0 To List_OptZiele.GetUpperBound(0)
            If (i > 0) Then
                fieldnames &= ", "
            End If
            fieldnames &= "[" & List_OptZiele(i).Bezeichnung & "] DOUBLE"
        Next
        'Tabelle anpassen
        command.CommandText = "ALTER TABLE QWerte ADD COLUMN " & fieldnames
        command.ExecuteNonQuery()

        'Tabelle 'Constraints'
        '----------------
        If (Me.List_Constraints.GetLength(0) > 0) Then
            'Spalten festlegen:
            fieldnames = ""
            For i = 0 To Me.List_Constraints.GetUpperBound(0)
                If (i > 0) Then
                    fieldnames &= ", "
                End If
                fieldnames &= "[" & Me.List_Constraints(i).Bezeichnung & "] DOUBLE"
            Next
            'Tabelle anpassen
            command.CommandText = "ALTER TABLE [Constraints] ADD COLUMN " & fieldnames
            command.ExecuteNonQuery()
        End If

        Call db_disconnect()

    End Sub

    'Ergebnisdatenbank für PES vorbereiten
    '*************************************
    Private Sub db_prepare_PES()

        Call db_connect()
        Dim command As OleDbCommand = New OleDbCommand("", db)

        'Tabelle 'OptParameter'
        '----------------------
        'Spalten festlegen:
        Dim fieldnames As String = ""
        Dim i As Integer

        For i = 0 To List_OptParameter.GetUpperBound(0)
            If (i > 0) Then
                fieldnames &= ", "
            End If
            fieldnames &= "[" & List_OptParameter(i).Bezeichnung & "] DOUBLE"
        Next
        'Tabelle anpassen
        command.CommandText = "ALTER TABLE OptParameter ADD COLUMN " & fieldnames
        command.ExecuteNonQuery()

        Call db_disconnect()

    End Sub

    'Ergebnisdatenbank für CES vorbereiten
    '*************************************
    Private Sub db_prepare_CES()

        Call db_connect()
        Dim command As OleDbCommand = New OleDbCommand("", db)

        'Tabelle 'Pfad'
        '--------------
        'Spalten festlegen:
        Dim fieldnames As String = ""
        Dim i As Integer

        For i = 0 To Me.List_Locations.GetUpperBound(0)
            If (i > 0) Then
                fieldnames &= ", "
            End If
            fieldnames &= "[" & Me.List_Locations(i).Name & "] TEXT"
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

    'Verbindung zu Ergebnisdatenbank schließen
    '*****************************************
    Private Sub db_disconnect()
        db.Close()
    End Sub

    'Update der ErgebnisDB mit QWerten und OptParametern
    '***************************************************
    Public Function db_update() As Boolean

        Call db_connect()

        Dim i As Integer

        Dim command As OleDbCommand = New OleDbCommand("", db)

        'Sim schreiben
        '-------------
        command.CommandText = "INSERT INTO Sim (Name) VALUES ('" & Me.Datensatz & "')"
        command.ExecuteNonQuery()
        'SimID holen
        command.CommandText = "SELECT @@IDENTITY AS ID"
        Dim Sim_ID As Integer = command.ExecuteScalar()

        'QWerte schreiben 
        '----------------
        Dim fieldnames As String = ""
        Dim fieldvalues As String = ""
        For i = 0 To List_OptZiele.GetUpperBound(0)
            fieldnames &= ", [" & List_OptZiele(i).Bezeichnung & "]"
            fieldvalues &= ", " & List_OptZiele(i).QWertTmp
        Next
        command.CommandText = "INSERT INTO QWerte (Sim_ID" & fieldnames & ") VALUES (" & Sim_ID & fieldvalues & ")"
        command.ExecuteNonQuery()

        'Constraints schreiben 
        '---------------------
        If (Me.List_Constraints.GetLength(0) > 0) Then
            fieldnames = ""
            fieldvalues = ""
            For i = 0 To Me.List_Constraints.GetUpperBound(0)
                fieldnames &= ", [" & Me.List_Constraints(i).Bezeichnung & "]"
                fieldvalues &= ", " & Me.List_Constraints(i).ConstTmp
            Next
            command.CommandText = "INSERT INTO [Constraints] (Sim_ID" & fieldnames & ") VALUES (" & Sim_ID & fieldvalues & ")"
            command.ExecuteNonQuery()
        End If

        If (Me.Method = "PES" Or Me.Method = "CES + PES" Or Me.Method = "SensiPlot") Then

            'OptParameter schreiben
            '----------------------
            fieldnames = ""
            fieldvalues = ""
            For i = 0 To Me.List_OptParameter.GetUpperBound(0)
                fieldnames &= ", [" & Me.List_OptParameter(i).Bezeichnung & "]"
                fieldvalues &= ", " & Me.List_OptParameter(i).Wert
            Next
            command.CommandText = "INSERT INTO OptParameter (Sim_ID" & fieldnames & ") VALUES (" & Sim_ID & fieldvalues & ")"
            command.ExecuteNonQuery()

        End If

        If (Me.Method = "CES" Or Me.Method = "CES + PES") Then

            'Pfad schreiben
            '--------------
            fieldnames = ""
            fieldvalues = ""
            For i = 0 To Me.List_Locations.GetUpperBound(0)
                fieldnames &= ", [" & Me.List_Locations(i).Name & "]"
                fieldvalues &= ", '" & Me.Akt.Measures(i) & "'"
            Next
            command.CommandText = "INSERT INTO Pfad (Sim_ID" & fieldnames & ") VALUES (" & Sim_ID & fieldvalues & ")"
            command.ExecuteNonQuery()

        End If

        Call db_disconnect()

    End Function

    'Sekundäre Population in DB speichern
    '************************************
    Public Sub db_setSekPop(ByVal SekPop(,) As Double, ByVal igen As Integer)

        Call db_connect()

        Dim command As OleDbCommand = New OleDbCommand("", db)

        ''Alte SekPop löschen
        'command.CommandText = "DELETE FROM SekPop"
        'command.ExecuteNonQuery()

        'Neue SekPop speichern
        Dim i, j As Integer
        Dim bedingung As String
        Dim Sim_ID As Integer
        For i = 1 To SekPop.GetUpperBound(0)    'BUG 135: SekPop(,) fängt bei 1 an!

            'zugehörige Sim_ID bestimmen
            bedingung = ""
            For j = 0 To Me.List_OptZiele.GetUpperBound(0)
                bedingung &= " AND QWerte.[" & Me.List_OptZiele(j).Bezeichnung & "] = " & SekPop(i, j + 1)
            Next
            command.CommandText = "SELECT Sim.ID FROM Sim INNER JOIN QWerte ON Sim.ID = QWerte.Sim_ID WHERE (1=1" & bedingung & ")"
            Sim_ID = command.ExecuteScalar()

            If (Sim_ID > 0) Then
                'SekPop Member speichern
                command.CommandText = "INSERT INTO SekPop (Generation, Sim_ID) VALUES (" & igen & ", " & Sim_ID & ")"
                command.ExecuteNonQuery()
            End If
        Next

        Call db_disconnect()

    End Sub

    'Einen Parametersatz aus der DB übernehmen
    '*****************************************
    Public Function db_getPara(ByVal xAchse As String, ByVal xWert As Double, ByVal yAchse As String, ByVal yWert As Double) As Boolean

        db_getPara = True
        Dim q As String
        Dim adapter As OleDbDataAdapter
        Dim ds As DataSet
        Dim numrows As Integer

        Call db_connect()

        'Fallunterscheidung nach Methode
        Select Case Me.Method

            Case "PES", "SensiPlot"

                'Unterscheidung für SO und SensiPlot
                If (Me.Method = "SensiPlot" Or Me.List_OptZiele.Length = 1) Then
                    'Nur ein QWert, und zwar auf der xAchse
                    q = "SELECT OptParameter.* FROM OptParameter INNER JOIN QWerte ON OptParameter.Sim_ID = QWerte.Sim_ID WHERE (QWerte.[" & xAchse & "] = " & xWert & ")"
                Else
                    'xAchse und yAchse sind beides QWerte
                    q = "SELECT OptParameter.* FROM OptParameter INNER JOIN QWerte ON OptParameter.Sim_ID = QWerte.Sim_ID WHERE (QWerte.[" & xAchse & "] = " & xWert & " AND QWerte.[" & yAchse & "] = " & yWert & ")"
                End If

                adapter = New OleDbDataAdapter(q, db)

                ds = New DataSet("EVO")
                numrows = adapter.Fill(ds, "OptParameter")

                'Anzahl Übereinstimmungen überprüfen
                If (numrows = 0) Then
                    MsgBox("Es wurde keine Übereinstimmung in der Datenbank gefunden!", MsgBoxStyle.Exclamation, "Problem")
                    Return False
                ElseIf (numrows > 1) Then
                    MsgBox("Es wurden mehr als eine Entsprechung von OptParametern für den gewählten Punkt gefunden!" & Chr(13) & Chr(10) & "Es wird nur das erste Ergebnis verwendet!", MsgBoxStyle.Exclamation, "Problem")
                End If

                'OptParametersatz übernehmen
                For i As Integer = 0 To Me.List_OptParameter.GetUpperBound(0)
                    With Me.List_OptParameter(i)
                        .Wert = ds.Tables("OptParameter").Rows(0).Item(.Bezeichnung)
                    End With
                Next

                'Modellparameter schreiben
                Call Me.Write_ModellParameter()


            Case "CES"

                q = "SELECT Pfad.* FROM Pfad INNER JOIN QWerte ON Pfad.Sim_ID = QWerte.Sim_ID WHERE (QWerte.[" & xAchse & "] = " & xWert & " AND QWerte.[" & yAchse & "] = " & yWert & ")"

                adapter = New OleDbDataAdapter(q, db)

                ds = New DataSet("EVO")
                numrows = adapter.Fill(ds, "Pfad")

                'Anzahl Übereinstimmungen überprüfen
                If (numrows = 0) Then
                    MsgBox("Es wurde keine Übereinstimmung in der Datenbank gefunden!", MsgBoxStyle.Exclamation, "Problem")
                    Return False
                ElseIf (numrows > 1) Then
                    MsgBox("Es wurden mehr als eine Entsprechung von Pfaden für den gewählten Punkt gefunden!" & Chr(13) & Chr(10) & "Es wird nur das erste Ergebnis verwendet!", MsgBoxStyle.Exclamation, "Problem")
                End If

                'Pfad übernehmen
                For i As Integer = 0 To Me.Akt.Measures.GetUpperBound(0)
                    Me.Akt.Measures(i) = ds.Tables("Pfad").Rows(0).Item(List_Locations(i).Name)
                Next

                'Bereitet das BlaueModell für die Kombinatorik vor
                Call Me.PREPARE_Evaluation_CES()


            Case "CES + PES"

                q = "SELECT OptParameter.*, Pfad.* FROM (((Sim LEFT JOIN Constraints ON Sim.ID = Constraints.Sim_ID) INNER JOIN OptParameter ON Sim.ID = OptParameter.Sim_ID) INNER JOIN Pfad ON Sim.ID = Pfad.Sim_ID) INNER JOIN QWerte ON Sim.ID = QWerte.Sim_ID WHERE (QWerte.[" & xAchse & "] = " & xWert & " AND QWerte.[" & yAchse & "] = " & yWert & ")"

                adapter = New OleDbDataAdapter(q, db)

                ds = New DataSet("EVO")
                adapter.Fill(ds, "OptParameter_Pfad")

                'Anzahl Übereinstimmungen überprüfen
                numrows = ds.Tables("OptParameter_Pfad").Rows.Count

                If (numrows = 0) Then
                    MsgBox("Es wurde keine Übereinstimmung in der Datenbank gefunden!", MsgBoxStyle.Exclamation, "Problem")
                    Return False
                ElseIf (numrows > 1) Then
                    MsgBox("Es wurden mehr als eine Entsprechung von OptParametern / Pfad für den gewählten Punkt gefunden!" & Chr(13) & Chr(10) & "Es wird nur das erste Ergebnis verwendet!", MsgBoxStyle.Exclamation, "Problem")
                End If

                'Pfad übernehmen
                For i As Integer = 0 To Me.Akt.Measures.GetUpperBound(0)
                    Me.Akt.Measures(i) = ds.Tables("OptParameter_Pfad").Rows(0).Item(List_Locations(i).Name)
                Next

                'Bereitet das BlaueModell für die Kombinatorik vor
                Call Me.PREPARE_Evaluation_CES()

                'OptParametersatz übernehmen
                For i As Integer = 0 To Me.List_OptParameter.GetUpperBound(0)
                    With Me.List_OptParameter(i)
                        .Wert = ds.Tables("OptParameter_Pfad").Rows(0).Item(.Bezeichnung)
                    End With
                Next

                'Modellparameter schreiben
                Call Me.Write_ModellParameter()

        End Select

        Call db_disconnect()

    End Function

    'Optimierungsergebnis aus einer DB lesen
    '***************************************
    Public Function db_getOptResult(Optional ByVal onlySekPop As Boolean = True) As OptResult

        '---------------------------------------------------------------------------
        'Hinweise:
        'Die EVO-Eingabedateien müssen eingelesen sein und mit der DB übereinstimmen
        'Funktioniert momentan nur für PES
        '---------------------------------------------------------------------------

        Dim i, j As Integer
        Dim OptResult As EVO.OptResult

        'Connect
        Call db_connect()

        'Read
        Dim q As String
        If (onlySekPop) Then
            'Nur die Lösungen aus der letzten Sekundären Population
            q = "SELECT SekPop.Generation, OptParameter.*, QWerte.*, Constraints.* FROM (((Sim LEFT JOIN [Constraints] ON Sim.ID=Constraints.Sim_ID) INNER JOIN OptParameter ON Sim.ID=OptParameter.Sim_ID) INNER JOIN QWerte ON Sim.ID=QWerte.Sim_ID) INNER JOIN SekPop ON Sim.ID=SekPop.Sim_ID WHERE (((SekPop.Generation)=(SELECT MAX(Generation) FROM SekPop)))"
        Else
            'Alle Lösungen
            q = "SELECT OptParameter.*, QWerte.*, Constraints.* FROM ((Sim LEFT JOIN [Constraints] ON Sim.ID=Constraints.Sim_ID) INNER JOIN OptParameter ON Sim.ID=OptParameter.Sim_ID) INNER JOIN QWerte ON Sim.ID=QWerte.Sim_ID ORDER BY Sim.ID"
        End If

        Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(q, db)

        Dim ds As New DataSet("EVO")
        Dim numRows As Integer = adapter.Fill(ds, "PESResult")

        'Disconnect
        Call db_disconnect()

        'Werte einlesen
        '==============
        OptResult = New EVO.OptResult()
        OptResult.List_OptParameter = Me.List_OptParameter
        OptResult.List_OptZiele = Me.List_OptZiele
        OptResult.List_Constraints = Me.List_Constraints

        ReDim OptResult.Solutions(numRows - 1)

        For i = 0 To numRows - 1
            With OptResult.Solutions(i)
                'OptParameter
                '------------
                ReDim .OptPara(Me.List_OptParameter.GetUpperBound(0))
                For j = 0 To Me.List_OptParameter.GetUpperBound(0)
                    .OptPara(j) = ds.Tables(0).Rows(i).Item(Me.List_OptParameter(j).Bezeichnung)
                Next
                'QWerte
                '------
                ReDim .QWerte(Me.List_OptZiele.GetUpperBound(0))
                For j = 0 To Me.List_OptZiele.GetUpperBound(0)
                    .QWerte(j) = ds.Tables(0).Rows(i).Item(Me.List_OptZiele(j).Bezeichnung)
                Next
                'Constraints
                '-----------
                ReDim .Constraints(Me.List_Constraints.GetUpperBound(0))
                For j = 0 To Me.List_Constraints.GetUpperBound(0)
                    .Constraints(j) = ds.Tables(0).Rows(i).Item(Me.List_Constraints(j).Bezeichnung)
                Next
            End With
        Next

        Return OptResult

    End Function

#End Region 'Ergebnisdatenbank

#End Region 'Methoden

End Class

'zusätzliche Klassen
'###################

'Klasse OptResult
'enthält die Ergebnisse eines Optimierungslaufs
'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
Public Class OptResult

    'Optimierungsbedingungen
    Public List_OptZiele() As Sim.Struct_OptZiel
    Public List_OptParameter() As Sim.Struct_OptParameter
    Public List_Constraints() As Sim.Struct_Constraint

    'Structure einer Lösung
    Public Structure Struct_Solution
        Public QWerte() As Double
        Public OptPara() As Double
        Public Constraints() As Double
        Public ReadOnly Property isValid() As Boolean
            Get
                For i As Integer = 0 To Me.Constraints.GetUpperBound(0)
                    If (Me.Constraints(i) < 0) Then Return False
                Next
                Return True
            End Get
        End Property
    End Structure

    'Array von Lösungen
    Public Solutions() As Struct_Solution

End Class
