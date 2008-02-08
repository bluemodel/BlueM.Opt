Imports System.IO
Imports System.Globalization

'*******************************************************************************
'*******************************************************************************
'**** Klasse Sim                                                            ****
'****                                                                       ****
'**** Basisklasse f�r Simulationsmodelle wie BlueM und SMUSI                ****
'****                                                                       ****
'**** Autoren: Christoph Huebner, Felix Froehlich                           ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'*******************************************************************************
'*******************************************************************************

Public MustInherit Class Sim

#Region "Eigenschaften"

    'Eigenschaften
    '#############

    'Information
    '-----------

    Public Method as String                              'Verwendete Methode

    'Generelle Eigenschaften
    '-----------------------
    Public Datensatz As String                           'Name des zu simulierenden Datensatzes
    Public MustOverride ReadOnly Property Datensatzendung() As String
    Public WorkDir As String                             'Arbeitsverzeichnis f�r das Blaue Modell
    Public Event WorkDirChange()                         'Event f�r �nderung des Arbeitsverzeichnisses

    Public SimStart As DateTime                          'Anfangsdatum der Simulation
    Public SimEnde As DateTime                           'Enddatum der Simulation
    Public SimDT As TimeSpan                             'Zeitschrittweite der Simulation

    Public SimErgebnis As Collection                     'Simulationsergebnis als Collection von Wave.Zeitreihe Objekten

    Public Shared FortranProvider As NumberFormatInfo    'Zahlenformatierungsanweisung f�r Fortran

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
        Public Bezeichnung As String                        'Bezeichnung
        Public Einheit As String                            'Einheit
        Public Wert As Double                               'Parameterwert
        Public Min As Double                                'Minimum
        Public Max As Double                                'Maximum
        Public Beziehung As EVO.Kern.PES.Beziehung          'Beziehung zum vorherigen OptParameter
        Public Property SKWert() As Double                  'skalierter Wert (0 bis 1)
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
        Public ZielWert As Double                   'Der vorgegeben Zielwert - muss doch eigentlich ein double sein dm 11.2007??
        Public ZielReiheDatei As String             'Der Dateiname der Zielreihe
        Public ZielGr As String                     'Spalte der .wel Datei falls ZielReihe .wel Datei ist
        Public ZielReihe As Wave.Zeitreihe          'Die Werte der Zielreihe
        Public QWertTmp As Double                   'Qualit�tswert der letzten Simulation wird hier zwischengespeichert 
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
        Public SimGr As String                      'Die Simulationsgr��e, die auf Verletzung der Grenze �berpr�ft werden soll
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

    'Ergebnisspeicher
    '----------------
    Public OptResult As OptResult

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

        'Benutzereinstellungen einlesen
        Call Me.ReadSettings()

        'Provider einrichten
        Sim.FortranProvider = New NumberFormatInfo()
        Sim.FortranProvider.NumberDecimalSeparator = "."
        Sim.FortranProvider.NumberGroupSeparator = ""
        Sim.FortranProvider.NumberGroupSizes = New Integer() {3}

        'Simulationsergebnis instanzieren
        Me.SimErgebnis = New Collection

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
            Call My.Settings.Save()
        End If

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
        'Constraints einlesen
        Call Me.Read_Constraints()
        'Optimierungsparameter einlesen
        Call Me.Read_OptParameter()
        'ModellParameter einlesen
        Call Me.Read_ModellParameter()
        'Modell-/Optparameter validieren
        Call Me.Validate_OPT_fits_to_MOD()
        'Pr�fen der Anfangswerte
        Call Me.Validate_Startvalues()
        'Ergebnisspeicher initialisieren
        Me.OptResult = New OptResult(Me)

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
        '�berpr�fen der Kombinatorik
        Call Me.Validate_Combinatoric()
        'Pr�fen ob Kombinatorik und Verzweigungsdatei zusammenpassen
        Call Me.Validate_CES_fits_to_VER()
        'Ergebnisspeicher initialisieren
        Me.OptResult = New OptResult(Me)

    End Sub

    Public Sub read_and_valid_INI_Files_CES_PES()

        'CES vorbereiten
        'Erforderliche Dateien werden eingelesen
        '---------------------------------------
        'Zielfunktionen einlesen
        Call Me.Read_OptZiele()
        'Constraints einlesen
        Call Me.Read_Constraints()
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
        'zus�tzliche Dateien werden eingelesen
        '-------------------------------------
        'Simulationsdaten einlesen
        Call Me.Read_SimParameter()
        'Optimierungsparameter einlesen
        Call Me.Read_OptParameter()
        'ModellParameter einlesen
        Call Me.Read_ModellParameter()
        'Modell-/Optparameter validieren
        Call Me.Validate_OPT_fits_to_MOD()
        'Pr�fen der Anfangswerte
        Call Me.Validate_Startvalues()

        'Ergebnisspeicher initialisieren
        '-------------------------------
        Me.OptResult = New OptResult(Me)

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

        'Format:
        '*|--------------|-------|-----------|--------|--------|-----------|
        '*| Bezeichnung  | Einh. | Anfangsw. |  Min   |  Max   | Beziehung |
        '*|-<---------->-|-<--->-|-<------->-|-<---->-|-<---->-|-<------->-|

        Dim Datei As String = WorkDir & Datensatz & "." & OptParameter_Ext

        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        Dim Zeile, array() As String
        Dim AnzParam As Integer = 0
        Dim AnzParam0 As Integer = 0
        Dim i As Integer = 0
        Dim k As Integer = 0
        Dim Bez_str As String = ""
        Dim tmp As Integer
       
        'Anzahl der Parameter feststellen
        '---------------------------------
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                array = Zeile.Split("|")

                If (array(1).Trim().StartsWith("Zeitreihe")) Then
                    tmp = Convert.ToInt16(array(2).Trim(), Sim.FortranProvider)
                    AnzParam = AnzParam + tmp + tmp - 1 'ermitteln der Parameteranzahl nAbschn + nAbschn - 1
                    If AnzParam > 0 Then ReDim Preserve List_OptParameter(AnzParam - 1)
                    If AnzParam > 0 Then ReDim Preserve List_OptParameter_Save(AnzParam - 1)
                    For k = AnzParam0 To AnzParam - 1
                        List_OptParameter(k).Bezeichnung = "Zeitreihe" + k.ToString
                        List_OptParameter(k).Wert = Convert.ToDouble(array(3).Trim(), Sim.FortranProvider)
                        List_OptParameter(k).Min = Convert.ToDouble(array(4).Trim(), Sim.FortranProvider)
                        List_OptParameter(k).Max = Convert.ToDouble(array(5).Trim(), Sim.FortranProvider)
                    Next
                    AnzParam0 = AnzParam

                Else
                    If i > 0 Then ReDim Preserve List_OptParameter(i)
                    If i > 0 Then ReDim Preserve List_OptParameter_Save(i)

                    List_OptParameter(i).Bezeichnung = array(1).Trim()
                    List_OptParameter(i).Einheit = array(2).Trim()
                    List_OptParameter(i).Wert = Convert.ToDouble(array(3).Trim(), Sim.FortranProvider)
                    List_OptParameter(i).Min = Convert.ToDouble(array(4).Trim(), Sim.FortranProvider)
                    List_OptParameter(i).Max = Convert.ToDouble(array(5).Trim(), Sim.FortranProvider)

                    'liegt eine Beziehung vor?
                    If (i > 0 And Not array(6).Trim() = "") Then
                        Me.List_OptParameter(i).Beziehung = getBeziehung(array(6).Trim())
                    Else
                        Me.List_OptParameter(i).Beziehung = EVO.Kern.PES.Beziehung.keine
                    End If

                    i += 1

                End If
            End If
        Loop Until StrRead.Peek() = -1
  
        '-----------------------------------------------------------------------------------------

        StrRead.Close()
        FiStr.Close()

        'OptParameter werden hier gesichert
        For i = 0 To List_OptParameter.GetUpperBound(0)
            Call copy_Struct_OptParameter(List_OptParameter(i), List_OptParameter_Save(i))
        Next

    End Sub


    'String in der Form < >, <=, >= in Beziehung umwandeln
    '*****************************************************
    Private Shared Function getBeziehung(ByVal bez_str As String) As EVO.Kern.PES.Beziehung
        Select Case bez_str
            Case "<"
                Return EVO.Kern.PES.Beziehung.kleiner
            Case "<="
                Return EVO.Kern.PES.Beziehung.kleinergleich
            Case ">"
                Return EVO.Kern.PES.Beziehung.groesser
            Case ">="
                Return EVO.Kern.PES.Beziehung.groessergleich
            Case Else
                Throw New Exception("Beziehung '" & bez_str & "' nicht erkannt!")
        End Select
    End Function

    'Modellparameter einlesen
    '************************
    Private Sub Read_ModellParameter()

        'Format:
        '*|--------------|--------------|-------|-------|-------|-------|-----|-----|--------|
        '*| OptParameter | Bezeichnung  | Einh. | Datei | Elem  | Zeile | von | bis | Faktor |
        '*|-<---------->-|-<---------->-|-<--->-|-<--->-|-<--->-|-<--->-|-<->-|-<->-|-<---->-|

        Dim Datei As String = WorkDir & Datensatz & "." & ModParameter_Ext
        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        Dim Zeile As String
        Dim AnzParam As Integer = 0
        Dim AnzParam0 As Integer = 0

        Dim array() As String
        Dim i As Integer = 0
        Dim k As Integer = 0
        Dim tmp As Integer = 0
  
        'Anzahl der Parameter feststellen
        '------------------------------------
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then

                array = Zeile.Split("|")

                If (array(1).Trim().StartsWith("Zeitreihe")) Then
                    tmp = Convert.ToInt16(array(3).Trim(), Sim.FortranProvider)
                    AnzParam = AnzParam + tmp + tmp - 1 'ermitteln der Parameteranzahl nAbschn + nAbschn - 1
                    If AnzParam > 0 Then ReDim Preserve List_ModellParameter(AnzParam - 1)
                    If AnzParam > 0 Then ReDim Preserve List_ModellParameter_Save(AnzParam - 1)
                    For k = AnzParam0 To AnzParam - 1
                        List_ModellParameter(k).OptParameter = "Zeitreihe" + k.ToString
                        List_ModellParameter(k).Element = i + 1
                        List_ModellParameter(k).Faktor = Convert.ToDouble(array(9).Trim(), Sim.FortranProvider)
                        List_ModellParameter(k).Bezeichnung = array(2).Trim()
                        List_ModellParameter(i).ZeileNr = Convert.ToInt16(array(3).Trim(), Sim.FortranProvider)
                        List_ModellParameter(i).SpVon = Convert.ToInt16(array(7).Trim())
                    Next
                    AnzParam0 = AnzParam
                    List_ModellParameter(i).Datei = array(4).Trim()
                Else
                    If i > 0 Then ReDim Preserve List_ModellParameter(i)
                    If i > 0 Then ReDim Preserve List_ModellParameter_Save(i)
                    List_ModellParameter(i).OptParameter = array(1).Trim()
                    List_ModellParameter(i).Bezeichnung = array(2).Trim()
                    List_ModellParameter(i).Einheit = array(3).Trim()
                    List_ModellParameter(i).Datei = array(4).Trim()
                    List_ModellParameter(i).Element = array(5).Trim()
                    List_ModellParameter(i).ZeileNr = Convert.ToInt16(array(6).Trim())
                    List_ModellParameter(i).SpVon = Convert.ToInt16(array(7).Trim())
                    List_ModellParameter(i).SpBis = Convert.ToInt16(array(8).Trim())
                    List_ModellParameter(i).Faktor = Convert.ToDouble(array(9).Trim(), Sim.FortranProvider)
                End If

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
            MsgBox("Die Anzahl der Ziele betr�gt mehr als 3!" & eol _
                    & "Es werden nur die ersten drei Zielfunktionen im Hauptdiagramm angezeigt!", MsgBoxStyle.Information, "Info")
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
                If (ZeilenArray(7).Trim() <> "") Then List_OptZiele(i).ZielWert = Convert.ToDouble(ZeilenArray(7).Trim(), Sim.FortranProvider)
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
                            Dim WEL As New Wave.WEL(Me.WorkDir & .ZielReiheDatei, True)
                            .ZielReihe = WEL.getReihe(.ZielGr)
                        Case ".ASC"
                            Dim ASC As New Wave.ASC(Me.WorkDir & .ZielReiheDatei, True)
                            .ZielReihe = ASC.getReihe(.ZielGr)
                        Case ".ZRE"
                            Dim ZRE As New Wave.ZRE(Me.WorkDir & .ZielReiheDatei, True)
                            .ZielReihe = ZRE.Zeitreihen(0)
                        Case ".PRB"
                            'BUG 183: geht nicht mehr, weil PRB-Dateien keine Zeitreihen sind!
                            'IsOK = Read_PRB(Me.WorkDir & .ZielReiheDatei, .ZielGr, .ZielReihe)
                        Case Else
                            Throw New Exception("Das Format der Zielreihe '" & .ZielReiheDatei & "' wurde nicht erkannt!")
                    End Select

                    'Zeitraum der Zielreihe �berpr�fen (nur bei WEL und ZRE)
                    '-------------------------------------------------------
                    If (ext.ToUpper = ".WEL" Or ext.ToUpper = ".ZRE" Or ext.ToUpper = ".ASC") Then

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

            'Zur�ck zum Dateianfang und lesen
            FiStr.Seek(0, SeekOrigin.Begin)

            'Einlesen der Zeile und �bergeben an die Constraints Liste
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
                    If (Not .Datei = "WEL") Then Throw New Exception("Constraints: Als Datei wird momentan nur 'WEL' unterst�tzt!")
                    If (Not .GrenzPos = "Obergrenze" And Not .GrenzPos = "Untergrenze") Then Throw New Exception("Constraints: F�r Oben/Unten muss entweder 'Obergrenze' oder 'Untergrenze' angegeben sein!")
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
                                Dim WEL As New Wave.WEL(Me.WorkDir & .GrenzReiheDatei, True)
                                .GrenzReihe = WEL.getReihe(.GrenzGr)
                            Case ".ZRE"
                                Dim ZRE As New Wave.ZRE(Me.WorkDir & .GrenzReiheDatei, True)
                                .GrenzReihe = ZRE.Zeitreihen(0)
                            Case Else
                                Throw New Exception("Das Format der Grenzwertreihe '" & .GrenzReiheDatei & "' wurde nicht erkannt!")
                        End Select

                        'Zeitraum der Grenzwertreihe �berpr�fen
                        '--------------------------------------
                        GrenzStart = .GrenzReihe.XWerte(0)
                        GrenzEnde = .GrenzReihe.XWerte(.GrenzReihe.Length - 1)

                        If (GrenzStart > Me.SimStart Or GrenzEnde < Me.SimEnde) Then
                            'Grenzwertreihe deckt Simulationszeitraum nicht ab
                            Throw New Exception("Die Grenzwertreihe '" & .GrenzReiheDatei & "' deckt den Simulationszeitraum nicht ab!")
                        Else
                            'Zielreihe auf Simulationszeitraum k�rzen
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

    'Pr�ft ob der Startwert der OptPara in der .OPT innerhalb der Min und Max Grenzen liegt
    '**************************************************************************************
    Public Sub Validate_Startvalues()
        Dim i As Integer

        For i = 0 To List_OptParameter.GetUpperBound(0)
            If Not List_OptParameter(i).Wert <= List_OptParameter(i).Max Or Not List_OptParameter(i).Wert >= List_OptParameter(i).Min Then
                Throw New Exception("Der Optimierungsparameter " & List_OptParameter(i).Bezeichnung & " in der .OPT Datei liegt nicht innerhalb der dort genannten Grenzen.")
            End If
        Next
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

    'Die Elemente werden pro Location im Child gespeichert
    '*****************************************************
    Public Sub Identify_Measures_Elements_Parameters(ByVal No_Loc As Integer, ByVal No_Measure As Integer, ByRef Measure As String, ByRef Elements() As String, ByRef Para(,) As Object)

        Dim i, j As Integer
        Dim x As Integer

        '1. Die Ma�nahme wird ermittelt
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


    'Struct und Methoden welche aktuellen Informationen zur Verf�gung stellen
    '#########################################################################
    Public Structure Aktuell
        Public Path() As Integer
        Public Measures() As String
        Public VER_ONOFF(,) As Object
    End Structure

    Public Akt As Aktuell

    'Bereitet das SimModell f�r Kombinatorik Optimierung vor
    '*******************************************************
    Public Sub PREPARE_Evaluation_CES(ByVal Path() As Integer, ByVal Elements() As String)

        'Setzt den Aktuellen Pfad
        Akt.Path = Path

        'Die elemente werden an die Kostenkalkulation �bergeben
        SKos1.Akt_Elemente = Elements

        'Ermittelt das aktuelle_ON_OFF array
        Call Prepare_Verzweigung_ON_OFF()

        'Schreibt die neuen Verzweigungen
        Call Me.Write_Verzweigungen()

    End Sub

    'ToDo: nicht besonders h�bsch �berladen
    '*************************************
    Public Sub PREPARE_Evaluation_CES()

        'Wandelt die Ma�nahmen Namen wieder in einen Pfad zur�ck
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
        Call Reset_OptPara_and_ModPara()

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
                Throw New Exception("Die aktuelle Kombination enth�lt keine Bauwerke, f�r die OptimierungsParameter vorliegen")
            End If

            Array.Resize(TMP_OptPara, count)
            Array.Resize(List_OptParameter, count)

            For i = 0 To TMP_OptPara.GetUpperBound(0)
                Call copy_Struct_OptParameter(TMP_OptPara(i), List_OptParameter(i))
            Next

        End If

    End Function

    'Setzt die Listen nach der Evaluierung wieder zur�ck auf alles was in den Eingabedateien steht
    '*********************************************************************************************
    Public Sub Reset_OptPara_and_ModPara()
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

    End Sub

    'Schreibt die passenden OptParameter f�r jede Location ins Child
    'ToDo alles ist da!
    '***************************************************************
    Dim n_tmp As Integer = 0

    'Funktion wahrscheinlich �berfl�ssig
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

        'Pr�fung ob alle Parameter verteilt wurden
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
        Destination.Beziehung = Source.Beziehung

    End Sub

    'EVO-Parameter�bergabe die Standard Parameter werden aus den Listen der OptPara und OptZiele ermittelt
    '*****************************************************************************************************
    Public Sub Parameter_Uebergabe(ByRef globalAnzPar As Short, ByRef globalAnzZiel As Short, ByRef globalAnzRand As Short, ByRef mypara() As Double, ByRef beziehungen() As EVO.Kern.PES.Beziehung)

        Dim i As Integer

        'Anzahl Optimierungsparameter �bergeben
        globalAnzPar = Me.List_OptParameter.GetLength(0)

        'Parameterwerte und Beziehungen �bergeben
        ReDim mypara(globalAnzPar - 1)
        ReDim beziehungen(globalAnzPar - 1)
        For i = 0 To globalAnzPar - 1
            mypara(i) = Me.List_OptParameter(i).SKWert
            beziehungen(i) = Me.List_OptParameter(i).Beziehung
        Next

        'Anzahl Optimierungsziele �bergeben
        globalAnzZiel = Me.List_OptZiele.GetLength(0)

        'Anzahl Randbedingungen �bergeben
        globalAnzRand = Me.List_Constraints.GetLength(0)

    End Sub

    'Evaluierung des SimModells f�r ParameterOptimierung - Steuerungseinheit
    '***********************************************************************
    Public Sub PREPARE_Evaluation_PES(ByVal myPara() As Double)

        Dim i As Short

        'Mutierte Parameter an OptParameter �bergeben
        For i = 0 To Me.List_OptParameter.GetUpperBound(0)
            List_OptParameter(i).SKWert = myPara(i)
        Next

        'Mutierte Parameter in Eingabedateien schreiben
        Call Write_ModellParameter()

    End Sub

    'Die ModellParameter in die Eingabedateien des SimModells schreiben
    '******************************************************************
    Public Sub Write_ModellParameter()

        Dim WertStr As String
        Dim AnzZeichen As Short
        Dim AnzZeil As Integer
        Dim j As Integer
        Dim Zeilenarray() As String
        Dim Zeile As String
        Dim StrLeft As String
        Dim StrRight As String
        Dim DateiPfad As String

        Dim jend, nZRE, ndauer, k, n As Integer
        Dim AnzParam, param As Integer
        Dim Zeitpunkt(1000) As Double
        Dim PfadZRE As String
        Dim Qab(1000) As Double
        Dim i, m As Integer
        Dim actDate, date2 As DateTime
        Dim Q1, Q2, dQ, dt, tmin, tmax As Double

        Dim Vorlaufzeit As Integer = 47



        'Evaluiere ob Zeitreihe
        If List_OptParameter(0).Bezeichnung.Contains("Zeitreihe") Then

            AnzParam = List_ModellParameter.Length

            'ermittle Anzahl der Zeitreihen
            For n = 0 To AnzParam - 1
                If List_ModellParameter(n).OptParameter.Contains("Zeitreihe") Then
                    If List_ModellParameter(n).Element > nZRE Then nZRE = List_ModellParameter(n).Element
                End If
            Next

            'Bestimmen der Simulationschritte
            actDate = Me.SimStart
            date2 = actDate.AddHours(Vorlaufzeit)

            While date2 <= Me.SimEnde
                ndauer += 1
                date2 = date2.Add(Me.SimDT)
            End While

            For j = 0 To Vorlaufzeit
                Qab(j) = 1.0
            Next j

            param = 0

            'Loop �ber alle Zeitreihen
            For n = 0 To nZRE - 1

                jend = List_ModellParameter(n).ZeileNr - 1 'nZeit

                'erster und letzter Zeitschritt
                Zeitpunkt(0) = 1
                Zeitpunkt(jend + 1) = ndauer

                For j = 1 To jend
                    param += 1
                    tmin = Zeitpunkt(j - 1) + 1
                    tmax = ndauer - jend + j - 1
                    Zeitpunkt(j) = tmin + List_OptParameter(param - 1).SKWert * (tmax - tmin)
                    Zeitpunkt(j) = Math.Round(Zeitpunkt(j), 0)
                    'Console.Out.WriteLine(Zeitpunkt(j))
                Next j
            
                '2.Q eintragen
                If List_ModellParameter(n).SpVon = 0 Then
                    For k = 0 To jend
                        param += 1
                        For j = Zeitpunkt(k) + Vorlaufzeit To Zeitpunkt(k + 1) + Vorlaufzeit
                            Qab(j) = List_OptParameter(param - 1).SKWert * List_OptParameter(n).Max
                            'Console.Out.WriteLine(Qab(j))
                        Next j
                    Next k

                ElseIf List_ModellParameter(n).SpVon = 1 Then

                    For k = 0 To jend
                        param += 1
                        Q1 = List_OptParameter(param - 2).SKWert * List_OptParameter(n).Max
                        Q2 = List_OptParameter(param - 1).SKWert * List_OptParameter(n).Max
                        dt = Zeitpunkt(k + 1) - Zeitpunkt(k)
                        If dt <> 0 Then dQ = (Q2 - Q1) / dt
                        m = 0
                        For j = Zeitpunkt(k) To Zeitpunkt(k + 1)
                            Qab(j) = Q1 + dQ * m
                            m += 1
                        Next j
                    Next k
                End If

                'Pfad f�r jede Zeitreihe aus *.EXT ermitteln
                PfadZRE = get_path_EXT(List_ModellParameter(n).Datei)

                'ZRE-Datei schreiben

                Call writeZRE(Qab, PfadZRE)

            Next
        Else

            'ModellParameter aus OptParametern kalkulieren()
            Call OptParameter_to_ModellParameter()

            'Alle ModellParameter durchlaufen
            For i = 0 To List_ModellParameter.GetUpperBound(0)

                DateiPfad = WorkDir & Datensatz & "." & List_ModellParameter(i).Datei
                'Datei �ffnen
                Dim FiStr As FileStream = New FileStream(DateiPfad, FileMode.Open, IO.FileAccess.ReadWrite)
                Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
                Dim StrReadSync As TextReader = TextReader.Synchronized(StrRead)

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

                StrReadSync.Close()
                StrRead.Close()
                FiStr.Close()

                'Anzahl verf�gbarer Zeichen
                AnzZeichen = List_ModellParameter(i).SpBis - List_ModellParameter(i).SpVon + 1

                'Zeile einlesen und splitten
                Zeile = Zeilenarray(List_ModellParameter(i).ZeileNr - 1)
                StrLeft = Zeile.Substring(0, List_ModellParameter(i).SpVon - 1)
                If (Zeile.Length > List_ModellParameter(i).SpBis) Then
                    StrRight = Zeile.Substring(List_ModellParameter(i).SpBis)
                Else
                    StrRight = ""
                End If

                'Wert auf verf�gbare Stellen k�rzen
                '----------------------------------
                'bestimmen des ganzzahligen Anteils, \-Operator ginge zwar theoretisch, ist aber f�r Zahlen < 1 nicht robust (warum auch immer)
                WertStr = Convert.ToString(List_ModellParameter(i).Wert - List_ModellParameter(i).Wert Mod 1.0, Sim.FortranProvider)

                If (WertStr.Length > AnzZeichen) Then
                    'Wert zu lang
                    Throw New Exception("Der Wert des Modellparameters '" & List_ModellParameter(i).Bezeichnung & "' (" & WertStr & ") ist l�nger als die zur Verf�gung stehende Anzahl von Zeichen!")

                ElseIf (WertStr.Length < AnzZeichen - 1) Then
                    'Runden auf verf�gbare Stellen: Anzahl der Stellen - Anzahl der Vorkommastellen - Komma
                    WertStr = Convert.ToString(Math.Round(List_ModellParameter(i).Wert, AnzZeichen - WertStr.Length - 1), Sim.FortranProvider)

                Else
                    'Ganzzahligen Wert benutzen
                End If

                'Falls erforderlich, Wert mit Leerzeichen f�llen
                If (WertStr.Length < AnzZeichen) Then
                    For j = 1 To AnzZeichen - WertStr.Length
                        WertStr &= " "
                    Next
                End If

                'Zeile wieder zusammensetzen
                Zeile = StrLeft & WertStr & StrRight

                Zeilenarray(List_ModellParameter(i).ZeileNr - 1) = Zeile

                'Alle Zeilen wieder in Datei schreiben
                Dim StrWrite As StreamWriter = New StreamWriter(DateiPfad, False, System.Text.Encoding.GetEncoding("iso8859-1"))
                Dim StrWriteSync As TextWriter = TextWriter.Synchronized(StrWrite)

                For j = 0 To AnzZeil - 1
                    StrWrite.WriteLine(Zeilenarray(j))
                Next

                StrWriteSync.Close()
                StrWrite.Close()

            Next

        End If

    End Sub

    'Evaluiert die Kinderchen mit Hilfe des Simulationsmodells
    '*********************************************************
    Public Function SIM_Evaluierung(ByVal ID As Integer, ByRef QN() As Double, ByRef RN() As Double) As Boolean

        Dim i As Short

        SIM_Evaluierung = False

        'Modell Starten
        If Not launchSim() Then Exit Function

        'Qualit�tswerte berechnen
        For i = 0 To Me.List_OptZiele.GetUpperBound(0)
            List_OptZiele(i).QWertTmp = QWert(List_OptZiele(i))
            QN(i) = List_OptZiele(i).QWertTmp
        Next

        'Constraints berechnen
        For i = 0 To Me.List_Constraints.GetUpperBound(0)
            List_Constraints(i).ConstTmp = Constraint(List_Constraints(i))
            RN(i) = List_Constraints(i).ConstTmp
        Next

        'L�sung abspeichern
        Call Me.OptResult.addSolution(ID, Me.List_OptZiele, Me.List_Constraints, Me.List_OptParameter)

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
                    Console.Out.WriteLine(List_OptParameter(j).Wert.ToString + " " + List_ModellParameter(i).Faktor.ToString)
                End If
            Next
        Next
    End Sub

    'Ermittelt Pfad von Nummer aus *.EXT-Datei 
    '*******************************************
    Public Function get_path_EXT(ByVal num As Integer) As String
        Dim Zeile As String
        Dim Array() As String
        Dim Pfad As String = ""

        Dim Datei As String = WorkDir & Datensatz & ".EXT"
        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                Array = Zeile.Split("|")
                If (Array(1).Trim() = num) Then
                    Pfad = Array(4).Trim()
                    Exit Do
                End If
            End If
        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()
        Return Pfad
    End Function

    'Schreibt ZRE Datei
    '*******************************************
    Public Sub writeZRE(ByVal Qab() As Double, ByVal PfadZRE As String)
        Dim i As Integer = 0
        Dim Text As String = ""
        Dim actDAte, date2 As Date

        Text = "*ZRE" + vbCrLf
        Text += "ZRE-Format m3/s   1" + vbCrLf
        Text += "1 1   1" + vbCrLf
        Text += Me.SimStart.ToString("yyyyMMdd HH:mm") + " " + Me.SimEnde.ToString("yyyyMMdd HH:mm") + vbCrLf

        actDAte = Me.SimStart
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


    'SimModell ausf�hren (simulieren)
    '********************************
    Public MustOverride Function launchSim() As Boolean

#End Region 'Evaluierung

#Region "Qualit�tswertberechnung"

    'Qualit�tswertberechnung
    '#######################

    'Berechnung des Qualit�tswerts (Zielwert)
    '****************************************
    Public MustOverride Function QWert(ByVal OptZiel As Struct_OptZiel) As Double

    'Qualit�tswert berechnen: Zieltyp = Reihe
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

            Case "n�ber"
                'Relative Anzahl der Zeitschritte mit �berschreitungen (in Prozent)
                Dim nUeber As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) > OptZiel.ZielReihe.YWerte(i)) Then
                        nUeber += 1
                    End If
                Next
                QWert = nUeber / SimReihe.Length * 100

            Case "s�ber"
                'Summe der �berschreitungen
                Dim sUeber As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) > OptZiel.ZielReihe.YWerte(i)) Then
                        sUeber += SimReihe.YWerte(i) - OptZiel.ZielReihe.YWerte(i)
                    End If
                Next
                QWert = sUeber

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

        'BUG 220: PRB geht nicht, weil keine Zeitreihe
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

#Region "Constraintberechnung"

    'Constraint berechnen (Constraint < 0 ist Grenzverletzung)
    '****************************************************
    Public Function Constraint(ByVal constr As Struct_Constraint) As Double

        Dim i As Integer

        'Simulationsergebnis auslesen
        Dim SimReihe As Wave.Zeitreihe
        SimReihe = Me.SimErgebnis(constr.SimGr)

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
        Dim StrReadSync As TextReader = TextReader.Synchronized(StrRead)

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
        StrReadSync.Close()
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

#End Region 'Methoden

End Class