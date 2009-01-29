'*******************************************************************************
'*******************************************************************************
'**** Klasse Problem                                                        ****
'****                                                                       ****
'**** Autoren: Christoph Hübner, Felix Fröhlich                             ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'*******************************************************************************
'*******************************************************************************

Imports System.IO

''' <summary>
''' Definiert das zu lösende Optimierungsproblem
''' </summary>
Public Class Problem

    'Konstanten
    '##########
    ''' <summary>
    ''' Erweiterung der Datei mit den Optimierungsparametern (*.OPT)
    ''' </summary>
    Public Const FILEEXT_OPT As String = "OPT"
    ''' <summary>
    ''' Erweiterung der Datei mit den Modellparametern (*.MOD)
    ''' </summary>
    Public Const FILEEXT_MOD As String = "MOD"
    ''' <summary>
    ''' Erweiterung der Datei mit den Zielfunktionen (*.ZIE)
    ''' </summary>
    Public Const FILEEXT_ZIE As String = "ZIE"
    ''' <summary>
    ''' Erweiterung der Datei mit den Constraints (*.CON)
    ''' </summary>
    Public Const FILEEXT_CON As String = "CON"
    ''' <summary>
    ''' Erweiterung der Datei mit der Kombinatorik  (*.CES)
    ''' </summary>
    Public Const FILEEXT_CES As String = "CES"

    'Eigenschaften
    '#############

    Private mWorkDir As String
    Private mDatensatz As String

    Private mMethod As String

    ''' <summary>
    ''' Aktuelle Liste der Modellparameter
    ''' </summary>
    Public List_ModellParameter() As Struct_ModellParameter
    ''' <summary>
    ''' Original-Liste der Modellparameter, die nicht verändert wird
    ''' </summary>
    Public List_ModellParameter_Save() As Struct_ModellParameter
    ''' <summary>
    ''' Aktuelle Liste der OptParameter
    ''' </summary>
    Public List_OptParameter() As OptParameter
    ''' <summary>
    ''' Original-Liste der OptParameter, die nicht verändert wird
    ''' </summary>
    Public List_OptParameter_Save() As OptParameter
    ''' <summary>
    ''' Liste der Feature Functions
    ''' </summary>
    ''' <remarks>Enthält sowohl Feature Functions als auch Penalty Functions</remarks>
    Public List_Featurefunctions() As Featurefunction
    ''' <summary>
    ''' Liste der Constraint Functions
    ''' </summary>
    Public List_Constraintfunctions() As Constraintfunction
    ''' <summary>
    ''' Liste der Locations
    ''' </summary>
    ''' <remarks>nur bei Kombinatorik verwendet</remarks>
    Public List_Locations() As Struct_Lokation
    ''' <summary>
    ''' Zeigt ob der CES-TestModus aktiv ist
    ''' </summary>
    ''' <remarks>nur bei Kombinatorik verwendet</remarks>
    Public CES_T_Modus As Constants.CES_T_MODUS

    'Properties
    '##########

    ''' <summary>
    ''' Arbeitsverzeichnis
    ''' </summary>
    ''' <remarks>nur bei Sim-Anwendungen relevant</remarks>
    Public Property WorkDir() As String
        Get
            Return Me.mWorkDir
        End Get
        Set(ByVal value As String)
            Me.mWorkDir = value
        End Set
    End Property

    ''' <summary>
    ''' Name des zu optimierenden Datensatzes
    ''' </summary>
    ''' <remarks>nur bei Sim-Anwendungen relevant</remarks>
    Public Property Datensatz() As String
        Get
            Return Me.mDatensatz
        End Get
        Set(ByVal value As String)
            Me.mDatensatz = value
        End Set
    End Property

    ''' <summary>
    ''' Name der verwendeten Optimierungsmethode
    ''' </summary>
    Public ReadOnly Property Method() As String
        Get
            Return Me.mMethod
        End Get
    End Property

    ''' <summary>
    ''' Anzahl Optparameter
    ''' </summary>
    Public ReadOnly Property NumParams() As Integer
        Get
            Return Me.List_OptParameter.Length
        End Get
    End Property

    ''' <summary>
    ''' Anzahl Feature Functions
    ''' </summary>
    ''' <remarks>Inklusive Penalty Functions!</remarks>
    Public ReadOnly Property NumFeatures() As Integer
        Get
            Return Me.List_Featurefunctions.Length
        End Get
    End Property

    ''' <summary>
    ''' Optimierungsmodus
    ''' </summary>
    ''' <returns>Single-Objective oder Multi-Objective</returns>
    Public ReadOnly Property Modus() As EVO.Common.Constants.EVO_MODUS
        Get
            Select Case Me.NumPenalties
                Case 1
                    Return EVO_MODUS.Single_Objective
                Case Is > 1
                    Return EVO_MODUS.Multi_Objective
                Case Else
                    Throw New Exception("Es sind keine Penalty-Functions definiert!")
            End Select
        End Get
    End Property

    ''' <summary>
    ''' Anzahl Penalty Functions
    ''' </summary>
    Public ReadOnly Property NumPenalties() As Integer
        Get
            Dim n As Integer

            n = 0
            For Each feature As Featurefunction In Me.List_Featurefunctions
                If (feature.isPenalty) Then n += 1
            Next

            Return n
        End Get
    End Property

    ''' <summary>
    ''' Liste der Penalty Functions
    ''' </summary>
    ''' <remarks>ReadOnly! Zum Setzen von Werten die List_Featurefunctions verwenden!</remarks>
    Public ReadOnly Property List_Penaltyfunctions() As Featurefunction()
        Get
            Dim i As Integer
            Dim array() As Featurefunction

            ReDim array(Me.NumPenalties - 1)

            i = 0
            For Each feature As Featurefunction In Me.List_Featurefunctions
                If (feature.isPenalty) Then
                    array(i) = feature
                    i += 1
                End If
            Next

            Return array
        End Get
    End Property

    ''' <summary>
    ''' Anzahl Constraint Functions
    ''' </summary>
    Public ReadOnly Property NumConstraints() As Integer
        Get
            Return Me.List_Constraintfunctions.Length
        End Get
    End Property

    ''' <summary>
    ''' Pfad Dimension
    ''' </summary>
    ''' <remarks>nur bei Kombinatorik verwendet</remarks>
    Public ReadOnly Property n_PathDimension() As Integer()
        Get
            Dim i As Integer
            Dim tmpArray() As Integer = {}

            If List_Locations.GetLength(0) = 0 Then
                Throw New Exception("Die Element Gesamtliste wurde abgerufen bevor die Elemente pro Location ermittelt wurden")
            End If

            ReDim tmpArray(List_Locations.GetUpperBound(0))
            For i = 0 To List_Locations.GetUpperBound(0)
                tmpArray(i) = List_Locations(i).List_Massnahmen.GetLength(0)
            Next

            n_PathDimension = tmpArray.Clone
        End Get
    End Property

#Region "Methoden"

    ''' <summary>
    ''' Konstruktor
    ''' </summary>
    ''' <param name="Method">zu verwendende Methode</param>
    Public Sub New(ByVal Method As String)

        'Methode setzen
        Me.mMethod = Method

        'Datenstrukturen initialisieren
        ReDim Me.List_Featurefunctions(-1)
        ReDim Me.List_Constraintfunctions(-1)
        ReDim Me.List_OptParameter(-1)
        ReDim Me.List_OptParameter_Save(-1)
        ReDim Me.List_ModellParameter(-1)
        ReDim Me.List_ModellParameter_Save(-1)
        ReDim Me.List_Locations(-1)

    End Sub

    ''' <summary>
    ''' Alle EVO-Eingabedateien einlesen
    ''' </summary>
    ''' <param name="simstart">Startzeitpunkt der Simulation</param>
    ''' <param name="simende">Endzeitpunkt der Simulation</param>
    ''' <remarks>Liest je nach eingestellter Methode die jeweils erforderlichen Dateien ein</remarks>
    Public Sub Read_InputFiles(ByVal simstart As DateTime, ByVal simende As DateTime)

        'EVO-Eingabedateien einlesen
        '---------------------------
        'Zielfunktionen einlesen
        Call Me.Read_ZIE(simstart, simende)

        'Constraints einlesen
        Call Me.Read_CON(simstart, simende)

        'Optimierungsparameter einlesen
        If (Me.Method <> METH_CES) Then
            Call Me.Read_OPT()
            'ModellParameter einlesen
            Call Me.Read_MOD()
        End If

        'Kombinatorik einlesen
        If (Me.Method = METH_HYBRID Or Me.Method = METH_CES) Then
            Call Me.Read_CES()
            'Testmodus wird ermittelt
            Me.CES_T_Modus = Set_TestModus()
        End If

        'Validierung
        '-----------
        'Modell-/Optparameter validieren
        Call Me.Validate_OPT_fits_to_MOD()
        'Prüfen der Anfangswerte
        Call Me.Validate_Startvalues()


    End Sub

    ''' <summary>
    ''' Optimierungsparameter (*.OPT-Datei) einlesen
    ''' </summary>
    ''' <remarks>http://130.83.196.154/BlueM/wiki/index.php/OPT-Datei</remarks>
    Private Sub Read_OPT()

        'Format:
        '*|--------------|-------|-----------|--------|--------|-----------|
        '*| Bezeichnung  | Einh. | Anfangsw. |  Min   |  Max   | Beziehung |
        '*|-<---------->-|-<--->-|-<------->-|-<---->-|-<---->-|-<------->-|

        Dim Datei As String = Me.mWorkDir & Me.Datensatz & "." & FILEEXT_OPT

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
        Dim Bez_str As String = ""
        Dim i As Integer = 0
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                'OptParameter instanzieren
                List_OptParameter(i) = New EVO.Common.OptParameter()
                array = Zeile.Split("|")
                'Werte zuweisen
                List_OptParameter(i).Bezeichnung = array(1).Trim()
                List_OptParameter(i).Einheit = array(2).Trim()
                List_OptParameter(i).StartWert = Convert.ToDouble(array(3).Trim(), Common.Provider.FortranProvider)
                List_OptParameter(i).Min = Convert.ToDouble(array(4).Trim(), Common.Provider.FortranProvider)
                List_OptParameter(i).Max = Convert.ToDouble(array(5).Trim(), Common.Provider.FortranProvider)
                'liegt eine Beziehung vor?
                If (i > 0 And Not array(6).Trim() = "") Then
                    Me.List_OptParameter(i).Beziehung = Common.Constants.getBeziehung(array(6).Trim())
                End If
                'Eingelesenen Startwert setzen
                List_OptParameter(i).RWert = List_OptParameter(i).StartWert
                i += 1
            End If
        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

        'OptParameter werden hier gesichert
        For i = 0 To List_OptParameter.GetUpperBound(0)
            List_OptParameter_Save(i) = List_OptParameter(i).Clone()
        Next

    End Sub

    ''' <summary>
    ''' Modellparameter (*.MOD-Datei) einlesen
    ''' </summary>
    ''' <remarks>http://130.83.196.154/BlueM/wiki/index.php/MOD-Datei</remarks>
    Private Sub Read_MOD()

        'Format:
        '*|--------------|--------------|-------|-------|-------|-------|-----|-----|--------|
        '*| OptParameter | Bezeichnung  | Einh. | Datei | Elem  | Zeile | von | bis | Faktor |
        '*|-<---------->-|-<---------->-|-<--->-|-<--->-|-<--->-|-<--->-|-<->-|-<->-|-<---->-|

        Dim Datei As String = Me.mWorkDir & Me.Datensatz & "." & FILEEXT_MOD

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

        ReDim Me.List_ModellParameter(AnzParam - 1)
        ReDim Me.List_ModellParameter_Save(AnzParam - 1)

        'Zurück zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        Dim array() As String
        Dim i As Integer = 0

        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                array = Zeile.Split("|")
                'Werte zuweisen
                With Me.List_ModellParameter(i)
                    .OptParameter = array(1).Trim()
                    .Bezeichnung = array(2).Trim()
                    .Einheit = array(3).Trim()
                    .Datei = array(4).Trim()
                    .Element = array(5).Trim()
                    .ZeileNr = Convert.ToInt16(array(6).Trim())
                    .SpVon = Convert.ToInt16(array(7).Trim())
                    .SpBis = Convert.ToInt16(array(8).Trim())
                    .Faktor = Convert.ToDouble(array(9).Trim(), Common.Provider.FortranProvider)
                End With
                i += 1
            End If
        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

        'ModellParameter werden hier gesichert
        For i = 0 To Me.List_ModellParameter.GetUpperBound(0)
            Me.List_ModellParameter_Save(i) = Me.List_ModellParameter(i).Clone()
        Next

    End Sub

    ''' <summary>
    ''' Optimierungsziele / Feature Functions (*.ZIE) einlesen
    ''' </summary>
    ''' <param name="SimStart">Startzeitpunkt der Simulation</param>
    ''' <param name="SimEnde">Endzeitpunkt der Simulation</param>
    ''' <remarks>http://130.83.196.154/BlueM/wiki/index.php/ZIE-Datei</remarks>
    Private Sub Read_ZIE(ByVal SimStart As DateTime, ByVal SimEnde As DateTime)

        Dim ZIE_Datei As String = Me.mWorkDir & Me.Datensatz & "." & FILEEXT_ZIE

        'Format:
        '*|-----|-------------|---|---------|-------|----------|---------|--------------|-------------------|--------------------|---------|
        '*| Opt | Bezeichnung | R | ZielTyp | Datei | SimGröße | ZielFkt | EvalZeitraum |    Referenzwert  ODER    Referenzreihe | IstWert |
        '*|     |             |   |         |       |          |         | Start | Ende | WertTyp | RefWert | RefGröße | Datei   |         |
        '*|-----|-------------|---|---------|-------|----------|---------|-------|------|---------|---------|----------|---------|---------|

        Const AnzSpalten As Integer = 14                       'Anzahl Spalten in der ZIE-Datei
        Dim i As Integer
        Dim Zeile As String
        Dim WerteArray() As String

        ReDim Me.List_Featurefunctions(-1)

        'Einlesen aller Ziele und Speichern im Manager
        '#############################################
        Dim FiStr As FileStream = New FileStream(ZIE_Datei, FileMode.Open, IO.FileAccess.ReadWrite)

        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        i = 0
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False And Zeile.Contains("|")) Then
                WerteArray = Zeile.Split("|")
                'Kontrolle
                If (WerteArray.GetUpperBound(0) <> AnzSpalten + 1) Then
                    Throw New Exception("Die ZIE-Datei hat die falsche Anzahl Spalten!")
                End If
                'Neue Feature-Function anlegen
                ReDim Preserve Me.List_Featurefunctions(i)
                Me.List_Featurefunctions(i) = New Common.Featurefunction()
                'Werte einlesen
                With Me.List_Featurefunctions(i)
                    If (WerteArray(1).Trim().ToUpper() = "J") Then
                        .isPenalty = True
                    Else
                        .isPenalty = False
                    End If
                    .Bezeichnung = WerteArray(2).Trim()
                    If (WerteArray(3).Trim() = "+") Then
                        .Richtung = Common.EVO_RICHTUNG.Maximierung
                    Else
                        .Richtung = Common.EVO_RICHTUNG.Minimierung
                    End If
                    .Typ = WerteArray(4).Trim()
                    .Datei = WerteArray(5).Trim()
                    .SimGr = WerteArray(6).Trim()
                    .Funktion = WerteArray(7).Trim()
                    If (WerteArray(8).Trim() <> "") Then
                        .EvalStart = WerteArray(8).Trim()
                    Else
                        .EvalStart = SimStart
                    End If
                    If WerteArray(9).Trim() <> "" Then
                        .EvalEnde = WerteArray(9).Trim()
                    Else
                        .EvalEnde = SimEnde
                    End If
                    .WertFunktion = WerteArray(10).Trim()
                    If (WerteArray(11).Trim() <> "") Then
                        .RefWert = Convert.ToDouble(WerteArray(11).Trim(), Common.Provider.FortranProvider)
                    End If
                    .RefGr = WerteArray(12).Trim()
                    .RefReiheDatei = WerteArray(13).Trim()
                    If (WerteArray(14).Trim() <> "") Then
                        .hasIstWert = True
                        .IstWert = Convert.ToDouble(WerteArray(14).Trim(), Common.Provider.FortranProvider)
                    Else
                        .hasIstWert = False
                    End If
                End With
                i += 1
            End If
        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

        'Referenzreihen einlesen
        '#######################
        Dim ZielStart As Date
        Dim ZielEnde As Date
        Dim ext As String

        For i = 0 To Me.List_Featurefunctions.GetUpperBound(0)
            With Me.List_Featurefunctions(i)
                If (.Typ = "Reihe" Or .Typ = "IHA") Then

                    'Dateiendung der Referenzreihendatei bestimmen und Reihe einlesen
                    '----------------------------------------------------------------
                    ext = System.IO.Path.GetExtension(.RefReiheDatei)
                    Select Case (ext.ToUpper)
                        Case ".WEL"
                            Dim WEL As New Wave.WEL(Me.mWorkDir & .RefReiheDatei)
                            .RefReihe = WEL.getReihe(.RefGr)
                        Case ".ASC"
                            Dim ASC As New Wave.ASC(Me.mWorkDir & .RefReiheDatei)
                            .RefReihe = ASC.getReihe(.RefGr)
                        Case ".ZRE"
                            Dim ZRE As New Wave.ZRE(Me.mWorkDir & .RefReiheDatei)
                            .RefReihe = ZRE.getReihe(0)
                            'Case ".PRB"
                            'BUG 183: geht nicht mehr, weil PRB-Dateien keine Zeitreihen sind!
                            'IsOK = Read_PRB(Me.WorkDir & .RefReiheDatei, .RefGr, .RefReihe)
                        Case Else
                            Throw New Exception("Das Format der Referenzreihe '" & .RefReiheDatei & "' wird nicht unterstützt!")
                    End Select

                    'Zeitraum der Referenzreihe überprüfen (nur bei WEL und ZRE)
                    '-----------------------------------------------------------
                    If (ext.ToUpper = ".WEL" Or ext.ToUpper = ".ZRE" Or ext.ToUpper = ".ASC") Then

                        ZielStart = .RefReihe.XWerte(0)
                        ZielEnde = .RefReihe.XWerte(.RefReihe.Length - 1)

                        If (ZielStart > .EvalStart Or ZielEnde < .EvalEnde) Then
                            'Referenzreihe deckt Evaluierungszeitraum nicht ab
                            Throw New Exception("Die Referenzreihe '" & .RefReiheDatei & "' deckt den Evaluierungszeitraum nicht ab!")
                        Else
                            'Referenzreihe auf Evaluierungszeitraum kürzen
                            Call .RefReihe.Cut(.EvalStart, .EvalEnde)
                        End If

                    End If

                    'Referenzreihe umbenennen
                    .RefReihe.Title += " (Referenz)"

                End If
            End With
        Next

    End Sub

    ''' <summary>
    ''' Constraint Functions (*.CON) einlesen
    ''' </summary>
    ''' <param name="SimStart">Startzeitpunkt der Simulation</param>
    ''' <param name="SimEnde">Endzeitpunkt der Simulation</param>
    ''' <remarks>http://130.83.196.154/BlueM/wiki/index.php/CON-Datei</remarks>
    Private Sub Read_CON(ByVal SimStart As DateTime, ByVal SimEnde As DateTime)

        'Format:
        '*|---------------|----------|-------|-----------|------------|----------------------|-----------------------------|
        '*|               |          |       |           |            |      Grenzwert       |        Grenzreihe           |
        '*| Bezeichnung   | GrenzTyp | Datei | SimGröße  | Oben/Unten | WertTyp  | Grenzwert | Grenzgröße | Datei          |
        '*|---------------|----------|-------|-----------|------------|----------|-----------|------------|----------------|

        Dim ext As String
        Dim i As Integer
        Dim Zeile As String
        Dim WerteArray() As String
        Const AnzSpalten As Integer = 9

        Dim Datei As String = Me.mWorkDir & Me.Datensatz & "." & FILEEXT_CON

        If (File.Exists(Datei)) Then

            Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.Read)
            Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

            i = 0
            Do
                'Zeile einlesen
                Zeile = StrRead.ReadLine.ToString()
                If (Not Zeile.StartsWith("*") And Zeile.Contains("|")) Then
                    WerteArray = Zeile.Split("|")
                    'Kontrolle
                    If (WerteArray.GetUpperBound(0) <> AnzSpalten + 1) Then
                        Throw New Exception("Die CON-Datei hat die falsche Anzahl Spalten!")
                    End If
                    'Neues Constraint anlegen
                    ReDim Preserve Me.List_Constraintfunctions(i)
                    Me.List_Constraintfunctions(i) = New Common.Constraintfunction()
                    'Werte zuweisen
                    With Me.List_Constraintfunctions(i)
                        .Bezeichnung = WerteArray(1).Trim()
                        .Typ = WerteArray(2).Trim()
                        .Datei = WerteArray(3).Trim()
                        .SimGr = WerteArray(4).Trim()
                        .GrenzPos = WerteArray(5).Trim()
                        .WertFunktion = WerteArray(6).Trim()
                        If (WerteArray(7).Trim() <> "") Then
                            .GrenzWert = Convert.ToDouble(WerteArray(7).Trim(), Common.Provider.FortranProvider)
                        End If
                        .GrenzGr = WerteArray(8).Trim()
                        .GrenzReiheDatei = WerteArray(9).Trim()
                    End With
                    i += 1
                End If
            Loop Until StrRead.Peek() = -1

            StrRead.Close()
            FiStr.Close()

            'Kontrolle
            '---------
            For i = 0 To Me.NumConstraints - 1
                With Me.List_Constraintfunctions(i)
                    If (Not .Typ = "Wert" And Not .Typ = "Reihe") Then Throw New Exception("Constraints: GrenzTyp muss entweder 'Wert' oder 'Reihe' sein!")
                    If (Not .Datei = "WEL") Then Throw New Exception("Constraints: Als Datei wird momentan nur 'WEL' unterstützt!")
                    If (Not .GrenzPos = "Obergrenze" And Not .GrenzPos = "Untergrenze") Then Throw New Exception("Constraints: Für Oben/Unten muss entweder 'Obergrenze' oder 'Untergrenze' angegeben sein!")
                End With
            Next

            'Falls mit Reihen verglichen werden soll werden hier die Reihen eingelesen
            Dim GrenzStart As Date
            Dim GrenzEnde As Date

            For i = 0 To Me.NumConstraints - 1
                With Me.List_Constraintfunctions(i)
                    If (.Typ = "Reihe") Then

                        'Dateiendung der Grenzwertdatei bestimmen und Reihe einlesen
                        ext = System.IO.Path.GetExtension(.GrenzReiheDatei)
                        Select Case (ext.ToUpper)
                            Case ".WEL"
                                Dim WEL As New Wave.WEL(Me.mWorkDir & .GrenzReiheDatei)
                                .GrenzReihe = WEL.getReihe(.GrenzGr)
                            Case ".ZRE"
                                Dim ZRE As New Wave.ZRE(Me.mWorkDir & .GrenzReiheDatei)
                                .GrenzReihe = ZRE.getReihe(0)
                            Case Else
                                Throw New Exception("Das Format der Grenzwertreihe '" & .GrenzReiheDatei & "' wurde nicht erkannt!")
                        End Select

                        'Zeitraum der Grenzwertreihe überprüfen
                        '--------------------------------------
                        GrenzStart = .GrenzReihe.XWerte(0)
                        GrenzEnde = .GrenzReihe.XWerte(.GrenzReihe.Length - 1)

                        If (GrenzStart > SimStart Or GrenzEnde < SimEnde) Then
                            'Grenzwertreihe deckt Simulationszeitraum nicht ab
                            Throw New Exception("Die Grenzwertreihe '" & .GrenzReiheDatei & "' deckt den Simulationszeitraum nicht ab!")
                        Else
                            'Zielreihe auf Simulationszeitraum kürzen
                            Call .GrenzReihe.Cut(SimStart, SimEnde)
                        End If

                        'Grenzwertreihe umbenennen
                        .GrenzReihe.Title += " (Grenze)"

                    End If
                End With
            Next

        Else
            'CON-Datei existiert nicht -> keine Constraints
            ReDim Me.List_Constraintfunctions(-1)
        End If

    End Sub

    ''' <summary>
    ''' Kombinatorik (*.CES) einlesen
    ''' </summary>
    ''' <remarks>Nur bei Kombinatorik verwendet. http://130.83.196.154/BlueM/wiki/index.php/CES-Datei</remarks>
    Private Sub Read_CES()

        Dim Datei As String = Me.mWorkDir & Me.Datensatz & "." & EVO.Common.Problem.FILEEXT_CES

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
        ReDim Me.List_Locations(0)
        ReDim Me.List_Locations(0).List_Massnahmen(0)

        'Zurück zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        Dim array() As String
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                array = Zeile.Split("|")
                'Werte zuweisen

                If Not Is_Name_IN(array(1).Trim(), Me.List_Locations) Then
                    i += 1
                    j = 0
                    System.Array.Resize(Me.List_Locations, i + 1)
                    Me.List_Locations(i).Name = array(1).Trim()
                End If
                System.Array.Resize(Me.List_Locations(i).List_Massnahmen, j + 1)
                ReDim Me.List_Locations(i).List_Massnahmen(j).Schaltung(2, 1)
                ReDim Me.List_Locations(i).List_Massnahmen(j).Bauwerke(3)
                With Me.List_Locations(i).List_Massnahmen(j)
                    .Name = array(2).Trim()
                    .Schaltung(0, 0) = array(3).Trim()
                    .Schaltung(0, 1) = array(4).Trim()
                    .Schaltung(1, 0) = array(5).Trim()
                    .Schaltung(1, 1) = array(6).Trim()
                    .Schaltung(2, 0) = array(7).Trim()
                    .Schaltung(2, 1) = array(8).Trim()
                    .KostenTyp = array(9).Trim()
                    .Bauwerke(0) = array(10).Trim()
                    .Bauwerke(1) = array(11).Trim()
                    .Bauwerke(2) = array(12).Trim()
                    .Bauwerke(3) = array(13).Trim()
                    .TestModus = Convert.ToInt16(array(14).Trim())
                End With
                j += 1
            End If

        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

    End Sub

    ''' <summary>
    ''' Prüft ob Optparameter und Modellparameter zusammenpassen
    ''' </summary>
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

    ''' <summary>
    ''' Prüft ob die Startwerte der OptParameter innerhalb der Min und Max Grenzen liegen
    ''' </summary>
    Private Sub Validate_Startvalues()
        Dim i As Integer

        For i = 0 To List_OptParameter.GetUpperBound(0)
            If Not List_OptParameter(i).RWert <= List_OptParameter(i).Max Or Not List_OptParameter(i).RWert >= List_OptParameter(i).Min Then
                Throw New Exception("Der Optimierungsparameter " & List_OptParameter(i).Bezeichnung & " in der .OPT Datei liegt nicht innerhalb der dort genannten Grenzen.")
            End If
        Next
    End Sub

    ''' <summary>
    ''' Validierungsfunktion der Kombinatorik Prüft ob Verbraucher an zwei Standorten doppelt vorhanden sind
    ''' </summary>
    ''' <remarks>Nur bei Kombinatorik verwendet</remarks>
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

    ''' <summary>
    ''' Reduziert die OptParameter und die ModellParameter auf die aktiven Elemente. 
    ''' !Wird jetzt aus den Elementen des Child generiert!
    ''' </summary>
    ''' <param name="Elements">???</param>
    ''' <returns>???</returns>
    ''' <remarks>Nur bei Kombinatorik verwendet</remarks>
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
                    TMP_ModPara(count) = List_ModellParameter(i).Clone()
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
                List_ModellParameter(i) = TMP_ModPara(i).Clone()
            Next

            'Reduzierung der OptParameter
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxx
            Dim TMP_OptPara() As EVO.Common.OptParameter
            ReDim TMP_OptPara(List_OptParameter.GetUpperBound(0))

            count = 0
            For i = 0 To List_OptParameter.GetUpperBound(0)
                For j = 0 To List_ModellParameter.GetUpperBound(0)
                    If List_OptParameter(i).Bezeichnung = List_ModellParameter(j).OptParameter Then
                        TMP_OptPara(count) = List_OptParameter(i).Clone()
                        count += 1
                        Exit For
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
                List_OptParameter(i) = TMP_OptPara(i).Clone()
            Next

        End If

    End Function

    ''' <summary>
    ''' Setzt die Listen der OptParameter und Modellparameter wieder zurück auf alles was in den Eingabedateien steht
    ''' </summary>
    Public Sub Reset_OptPara_and_ModPara()
        Dim i As Integer

        'Kopieren der Listen aus den Sicherungen
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        ReDim List_ModellParameter(List_ModellParameter_Save.GetUpperBound(0))
        For i = 0 To List_ModellParameter_Save.GetUpperBound(0)
            List_ModellParameter(i) = List_ModellParameter_Save(i).Clone()
        Next
        ReDim List_OptParameter(List_OptParameter_Save.GetUpperBound(0))
        For i = 0 To List_OptParameter_Save.GetUpperBound(0)
            List_OptParameter(i) = List_OptParameter_Save(i).Clone()
        Next

    End Sub

    ''' <summary>
    ''' Anzahl maximal möglicher Kombinationen
    ''' </summary>
    ''' <remarks>Nur bei Kombinatorik verwendet</remarks>
    Public ReadOnly Property NumCombinations() As Integer
        Get
            If (Me.CES_T_Modus = EVO.Common.Constants.CES_T_MODUS.One_Combi) Then
                NumCombinations = 1
            Else
                Dim i As Integer
                NumCombinations = Me.List_Locations(0).List_Massnahmen.GetLength(0)
                For i = 1 To Me.List_Locations.GetUpperBound(0)
                    NumCombinations = NumCombinations * Me.List_Locations(i).List_Massnahmen.GetLength(0)
                Next
            End If
        End Get
    End Property

    ''' <summary>
    ''' Anzahl Locations
    ''' </summary>
    ''' <remarks>Nur bei Kombinatorik verwendet</remarks>
    Public ReadOnly Property NumLocations() As Integer
        Get
            Return Me.List_Locations.Length
        End Get
    End Property

    ''' <summary>
    ''' Hilfsfunktion um zu Prüfen ob der Name bereits vorhanden ist oder nicht
    ''' </summary>
    ''' <param name="name">???</param>
    ''' <param name="array_modellparameter">???</param>
    ''' <returns>???</returns>
    ''' <remarks>Nur bei Kombinatorik verwendet</remarks>
    Public Shared Function Is_Name_IN(ByVal name As String, ByVal array_modellparameter() As EVO.Common.Struct_Lokation) As Boolean
        Is_Name_IN = False
        Dim i As Integer
        For i = 0 To array_modellparameter.GetUpperBound(0)
            If name = array_modellparameter(i).Name Then
                Is_Name_IN = True
                Exit Function
            End If
        Next
    End Function

    ''' <summary>
    ''' Test Tagesganglinie mit Autokalibrierung
    ''' </summary>
    ''' <remarks>
    ''' Beta-Version - erlaubt Kalirbierung der Tagesganlinie
    ''' dafür muss für den jeweiligen Tagesgangwert in der .mod Datei in der Spalte "Elem" "TGG_QH" eingetragen werden
    ''' Vorschlag: Aktivierung der kalibrierung des Tagesganlinie über einen Schalter, damit diese Funktion nicht bei jeder optimierung aufgerufen wird
    ''' Kontakt: Valentin Gamerith
    ''' </remarks>
    Private Sub VG_Kalibrierung_Tagesganglinie()

        Dim i, j As Integer
        Dim VG_sum_TGG As Double
        Dim VG_check_24 As Integer
        Dim VG_Faktor As Double

        VG_check_24 = 0
        VG_sum_TGG = 0

        'Bestimmen der Paramterersumme zum berechenen des notwendigen Faktors um auf 24 zu kommen
        For i = 0 To List_ModellParameter.GetUpperBound(0)
            If Trim(List_ModellParameter(i).Element) = "TGG_QH" Then
                VG_check_24 = VG_check_24 + 1

                For j = 0 To List_OptParameter.GetUpperBound(0)
                    If Trim(List_OptParameter(j).Bezeichnung) = Trim(List_ModellParameter(i).OptParameter) Then 'Parameter gefunden
                        VG_sum_TGG = VG_sum_TGG + List_OptParameter(j).RWert 'aufsummieren
                    End If
                Next
            End If
        Next
        'Überprüft ob 24 Werte zugeordnet wurden
        If VG_check_24 = 24 Then
            'Faktor um auf 24 zu kommen:Xi = Xsim,i * n/Summe(Xi,Sim)
            VG_Faktor = VG_check_24 / VG_sum_TGG
            For i = 0 To List_ModellParameter.GetUpperBound(0)
                If Trim(List_ModellParameter(i).Element) = "TGG_QH" Then
                    List_ModellParameter(i).Faktor = VG_Faktor 'setzt den Faktor für den jeweiligen Tagesgangwert
                End If
            Next
        Else
        End If
    End Sub

    ''' <summary>
    ''' Überprüft ob und welcher CES-TestModus aktiv ist
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Nur bei Kombinatorik verwendet</remarks>
    Public Function Set_TestModus() As Common.Constants.CES_T_MODUS

        Dim i, j As Integer
        Dim count_A As Integer
        Dim count_B As Integer
        Dim Bool As Boolean = False

        'Prüft auf den Modus "0" kein TestModus
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        count_A = 0
        count_B = 0
        For i = 0 To Me.List_Locations.GetUpperBound(0)
            For j = 0 To Me.List_Locations(i).List_Massnahmen.GetUpperBound(0)
                count_A += 1
                If (Me.List_Locations(i).List_Massnahmen(j).TestModus = 0) Then
                    count_B += 1
                End If
            Next
        Next

        If count_A = count_B Then
            Set_TestModus = Common.Constants.CES_T_MODUS.No_Test
            Exit Function
        End If

        'Prüft aus Testen einer definierten Kombination
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        count_A = 0
        count_B = 0
        For i = 0 To Me.List_Locations.GetUpperBound(0)
            count_A += 1
            For j = 0 To Me.List_Locations(i).List_Massnahmen.GetUpperBound(0)
                If Me.List_Locations(i).List_Massnahmen(j).TestModus = 1 Then
                    count_B += 1
                End If
            Next
        Next

        If count_A = count_B Then
            Set_TestModus = Common.Constants.CES_T_MODUS.One_Combi
            Exit Function
        End If

        'Prüft auf einmaliges Testen aller möglichen Kombinationen
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        count_A = 0
        count_B = 0
        For i = 0 To Me.List_Locations.GetUpperBound(0)
            For j = 0 To Me.List_Locations(i).List_Massnahmen.GetUpperBound(0)
                count_A += 1
                If Me.List_Locations(i).List_Massnahmen(j).TestModus = 1 Then
                    count_B += 1
                End If
            Next
        Next

        If count_A = count_B Then
            Set_TestModus = Constants.CES_T_MODUS.All_Combis
            Exit Function
        End If

        Throw New Exception("Fehler bei der Angabe des Testmodus")

    End Function

#End Region 'Methoden

End Class
