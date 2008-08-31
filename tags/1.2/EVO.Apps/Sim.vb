Imports System.IO
Imports System.Globalization

'*******************************************************************************
'*******************************************************************************
'**** Klasse Sim                                                            ****
'****                                                                       ****
'**** Basisklasse für Simulationsmodelle wie BlueM und SMUSI                ****
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

    'Generelle Eigenschaften
    '-----------------------
    Public Datensatz As String                           'Name des zu simulierenden Datensatzes
    Protected mDatensatzendung As String                 'Dateiendung des Datensatzes (inkl. Punkt)
    Public ReadOnly Property Datensatzendung() As String
        Get
            Return Me.mDatensatzendung
        End Get
    End Property
    Public WorkDir As String                             'Arbeitsverzeichnis/Datensatz für BlueM
    Public WorkDirSave As String									'Arbeitsverzeichnis/Datensatz für BlueM

    Public SimStart As DateTime                          'Anfangsdatum der Simulation
    Public SimEnde As DateTime                           'Enddatum der Simulation
    Public SimDT As TimeSpan                             'Zeitschrittweite der Simulation

    Public SimErgebnis As Collection                     'Simulationsergebnis als Collection von Wave.Zeitreihe Objekten

    'Konstanten
    '----------
    Public Const OptParameter_Ext As String = "OPT"      'Erweiterung der Datei mit den Optimierungsparametern (*.OPT)
    Public Const ModParameter_Ext As String = "MOD"      'Erweiterung der Datei mit den Modellparametern (*.MOD)
    Public Const OptZiele_Ext As String = "ZIE"          'Erweiterung der Datei mit den Zielfunktionen (*.ZIE)
    Public Const Constraints_Ext As String = "CON"       'Erweiterung der Datei mit den Constraints (*.CON)
    Public Const Combi_Ext As String = "CES"             'Erweiterung der Datei mit der Kombinatorik  (*.CES)

    'OptParameter
    '------------
    Public List_OptParameter() As EVO.Common.OptParameter             'Liste der Optimierungsparameter
    Public List_OptParameter_Save() As EVO.Common.OptParameter        'Liste der Optimierungsparameter die nicht verändert wird

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

    Public List_ModellParameter() As Struct_ModellParameter      'Liste der Modellparameter
    Public List_ModellParameter_Save() As Struct_ModellParameter 'Liste der Modellparameter die nicht verändert wird

    'Ergebnisspeicher
    '----------------
    Public OptResult As EVO.OptResult.OptResult             'Optimierungsergebnis
    Public OptResultRef As EVO.OptResult.OptResult          'Vergleichsergebnis

    Public List_Locations() As EVO.Common.Struct_Lokation

    Public VerzweigungsDatei(,) As String                   'Gibt die PathSize an für jede Pfadstelle
    Public CES_T_Modus As Common.Constants.CES_T_MODUS      'Zeigt ob der TestModus aktiv ist
    Public n_Combinations As Integer                        'Die Anzahl der Möglichen Kombinationen


#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

#Region "Initialisierung"

    'Konstruktor
    '***********
    Public Sub New()

        'Datenstrukturen initialisieren
        '------------------------------
        ReDim Me.List_OptParameter(-1)
        ReDim Me.List_OptParameter_Save(-1)
        ReDim Me.List_ModellParameter(-1)
        ReDim Me.List_ModellParameter_Save(-1)
        ReDim Me.List_Locations(-1)

        'Simulationsergebnis instanzieren
        Me.SimErgebnis = New Collection()

    End Sub

    'Pfad zum Datensatz verarbeiten und speichern
    '********************************************
    Public Sub setDatensatz(ByVal pfad As String)

        If (File.Exists(pfad)) Then
            'Datensatzname bestimmen
            Me.Datensatz = Path.GetFileNameWithoutExtension(pfad)
            'Arbeitsverzeichnis bestimmen
            Me.WorkDir = Path.GetDirectoryName(pfad) & "\"
				Me.WorkDirSave = Path.GetDirectoryName(Pfad) & "\"
        Else
            Throw New Exception("Der Datensatz '" & pfad & "' existiert nicht!")
        End If

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
        Call Me.Read_ZIE()
        'Constraints einlesen
        Call Me.Read_CON()
        'Optimierungsparameter einlesen
        Call Me.Read_OPT()
        'ModellParameter einlesen
        Call Me.Read_MOD()
        'Modell-/Optparameter validieren
        Call Me.Validate_OPT_fits_to_MOD()
        'Prüfen der Anfangswerte
        Call Me.Validate_Startvalues()
        'Ergebnisspeicher initialisieren
        Me.OptResult = New EVO.OptResult.OptResult(Me.Datensatz, Me.List_OptParameter, Me.List_OptParameter_Save, Me.List_Locations)

    End Sub

    'CES vorbereiten
    'Erforderliche Dateien werden eingelesen
    '***************************************
    Public Sub read_and_valid_INI_Files_CES()

        'Simulationsdaten einlesen
        Call Me.Read_SimParameter()
        'Zielfunktionen einlesen
        Call Me.Read_ZIE()
        'Constraints einlesen
        Call Me.Read_CON()
        'Kombinatorik Datei einlesen
        Call Me.Read_Kombinatorik()
        'Verzweigungs Datei einlesen
        Call Me.Read_Verzweigungen()
        'Überprüfen der Kombinatorik
        Call Me.Validate_Combinatoric()
        'Prüfen ob Kombinatorik und Verzweigungsdatei zusammenpassen
        Call Me.Validate_CES_fits_to_VER()
        'Testmodus wird ermittelt
        CES_T_Modus = Set_TestModus()
        'Die Zahl der Kombinationen wird ermittelt
        n_Combinations = No_of_Combinations()
        'Ergebnisspeicher initialisieren
        Me.OptResult = New OptResult.Optresult(Me.Datensatz, Me.List_OptParameter, Me.List_OptParameter_Save, Me.List_Locations)

    End Sub

    Public Sub read_and_valid_INI_Files_HYBRID()

        'Simulationsdaten einlesen
        Call Me.Read_SimParameter()

        'CES vorbereiten
        'Erforderliche Dateien werden eingelesen
        '---------------------------------------
        'Zielfunktionen einlesen
        Call Me.Read_ZIE()
        'Constraints einlesen
        Call Me.Read_CON()
        'Kombinatorik Datei einlesen
        Call Me.Read_Kombinatorik()
        'Verzweigungs Datei einlesen
        Call Me.Read_Verzweigungen()
        'Überprüfen der Kombinatorik
        Call Me.Validate_Combinatoric()
        'Prüfen ob Kombinatorik und Verzweigungsdatei zusammenpassen
        Call Me.Validate_CES_fits_to_VER()
        'Testmodus wird ermittelt
        CES_T_Modus = Set_TestModus()
        'Die Zahl der Kombinationen wird ermittelt
        n_Combinations = No_of_Combinations()
        'Datenbank vorbereiten

        'PES vorbereiten
        'zusätzliche Dateien werden eingelesen
        '-------------------------------------
        'Optimierungsparameter einlesen
        Call Me.Read_OPT()
        'ModellParameter einlesen
        Call Me.Read_MOD()
        'Modell-/Optparameter validieren
        Call Me.Validate_OPT_fits_to_MOD()
        'Prüfen der Anfangswerte
        Call Me.Validate_Startvalues()

        'Ergebnisspeicher initialisieren
        '-------------------------------
        Me.OptResult = New EVO.OptResult.OptResult(Me.Datensatz, Me.List_OptParameter, Me.List_OptParameter_Save, Me.List_Locations)

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
    Private Sub Read_OPT()

        'Format:
        '*|--------------|-------|-----------|--------|--------|-----------|
        '*| Bezeichnung  | Einh. | Anfangsw. |  Min   |  Max   | Beziehung |
        '*|-<---------->-|-<--->-|-<------->-|-<---->-|-<---->-|-<------->-|

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

    'Modellparameter einlesen
    '************************
    Private Sub Read_MOD()

        'Format:
        '*|--------------|--------------|-------|-------|-------|-------|-----|-----|--------|
        '*| OptParameter | Bezeichnung  | Einh. | Datei | Elem  | Zeile | von | bis | Faktor |
        '*|-<---------->-|-<---------->-|-<--->-|-<--->-|-<--->-|-<--->-|-<->-|-<->-|-<---->-|

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
                List_ModellParameter(i).Faktor = Convert.ToDouble(array(9).Trim(), Common.Provider.FortranProvider)
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
    Protected Overridable Sub Read_ZIE()

        Dim ZIE_Datei As String = Me.WorkDir & Me.Datensatz & "." & OptZiele_Ext

        'Format:
        '*|-----|-------------|---|---------|-------|----------|---------|--------------|-------------------|--------------------|---------|
        '*| Opt | Bezeichnung | R | ZielTyp | Datei | SimGröße | ZielFkt | EvalZeitraum |    Referenzwert  ODER    Referenzreihe | IstWert |
        '*|     |             |   |         |       |          |         | Start | Ende | WertTyp | RefWert | RefGröße | Datei   |         |
        '*|-----|-------------|---|---------|-------|----------|---------|-------|------|---------|---------|----------|---------|---------|

        Const AnzSpalten As Integer = 14                       'Anzahl Spalten in der ZIE-Datei
        Dim i As Integer
        Dim Zeile As String
        Dim WerteArray() As String

        ReDim Common.Manager.List_Featurefunctions(-1)

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
                ReDim Preserve Common.Manager.List_Featurefunctions(i)
                Common.Manager.List_Featurefunctions(i) = New Common.Featurefunction()
                'Werte einlesen
                With Common.Manager.List_Featurefunctions(i)
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
                        .EvalStart = Me.SimStart
                    End If
                    If WerteArray(9).Trim() <> "" Then
                        .EvalEnde = WerteArray(9).Trim()
                    Else
                        .EvalEnde = Me.SimEnde
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

        For i = 0 To Common.Manager.List_Featurefunctions.GetUpperBound(0)
            With Common.Manager.List_Featurefunctions(i)
                If (.Typ = "Reihe" Or .Typ = "IHA") Then

                    'Dateiendung der Referenzreihendatei bestimmen und Reihe einlesen
                    '----------------------------------------------------------------
                    ext = Path.GetExtension(.RefReiheDatei)
                    Select Case (ext.ToUpper)
                        Case ".WEL"
                            Dim WEL As New Wave.WEL(Me.WorkDir & .RefReiheDatei)
                            .RefReihe = WEL.getReihe(.RefGr)
                        Case ".ASC"
                            Dim ASC As New Wave.ASC(Me.WorkDir & .RefReiheDatei)
                            .RefReihe = ASC.getReihe(.RefGr)
                        Case ".ZRE"
                            Dim ZRE As New Wave.ZRE(Me.WorkDir & .RefReiheDatei)
                            .RefReihe = ZRE.Zeitreihen(0)
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

    'Constraints einlesen
    '********************
    Private Sub Read_CON()

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

        Dim Datei As String = WorkDir & Datensatz & "." & Constraints_Ext

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
                    ReDim Preserve Common.Manager.List_Constraintfunctions(i)
                    Common.Manager.List_Constraintfunctions(i) = New Common.Constraintfunction()
                    'Werte zuweisen
                    With Common.Manager.List_Constraintfunctions(i)
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
            For i = 0 To Common.Manager.NumConstraints - 1
                With Common.Manager.List_Constraintfunctions(i)
                    If (Not .Typ = "Wert" And Not .Typ = "Reihe") Then Throw New Exception("Constraints: GrenzTyp muss entweder 'Wert' oder 'Reihe' sein!")
                    If (Not .Datei = "WEL") Then Throw New Exception("Constraints: Als Datei wird momentan nur 'WEL' unterstützt!")
                    If (Not .GrenzPos = "Obergrenze" And Not .GrenzPos = "Untergrenze") Then Throw New Exception("Constraints: Für Oben/Unten muss entweder 'Obergrenze' oder 'Untergrenze' angegeben sein!")
                End With
            Next

            'Falls mit Reihen verglichen werden soll werden hier die Reihen eingelesen
            Dim GrenzStart As Date
            Dim GrenzEnde As Date

            For i = 0 To Common.Manager.NumConstraints - 1
                With Common.Manager.List_Constraintfunctions(i)
                    If (.Typ = "Reihe") Then

                        'Dateiendung der Grenzwertdatei bestimmen und Reihe einlesen
                        ext = Path.GetExtension(.GrenzReiheDatei)
                        Select Case (ext.ToUpper)
                            Case ".WEL"
                                Dim WEL As New Wave.WEL(Me.WorkDir & .GrenzReiheDatei)
                                .GrenzReihe = WEL.getReihe(.GrenzGr)
                            Case ".ZRE"
                                Dim ZRE As New Wave.ZRE(Me.WorkDir & .GrenzReiheDatei)
                                .GrenzReihe = ZRE.Zeitreihen(0)
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
                            Call .GrenzReihe.Cut(Me.SimStart, Me.SimEnde)
                        End If

                        'Grenzwertreihe umbenennen
                        .GrenzReihe.Title += " (Grenze)"

                    End If
                End With
            Next

        Else
            'CON-Datei existiert nicht
            ReDim Common.Manager.List_Constraintfunctions(-1)
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
            If Not List_OptParameter(i).RWert <= List_OptParameter(i).Max Or Not List_OptParameter(i).RWert >= List_OptParameter(i).Min Then
                Throw New Exception("Der Optimierungsparameter " & List_OptParameter(i).Bezeichnung & " in der .OPT Datei liegt nicht innerhalb der dort genannten Grenzen.")
            End If
        Next
    End Sub




#End Region 'Prüfung der Eingabedateien

#Region "Kombinatorik"

    'Kombinatorik
    '############

    'Gibt die Pfad Dimensionen zurück
    '********************************
    Public Function n_PathDimension() As Integer()
        Dim i As Integer
        Dim Array() As Integer = {}

        If List_Locations.GetLength(0) = 0 Then
            Throw New Exception("Die Element Gesamtliste wurde abgerufen bevor die Elemente pro Location ermittelt wurden")
        End If

        ReDim Array(List_Locations.GetUpperBound(0))
        For i = 0 To List_Locations.GetUpperBound(0)
            Array(i) = List_Locations(i).List_Massnahmen.GetLength(0)
        Next

        n_PathDimension = Array.Clone

    End Function

    'Berechnet die Anzahl maximal möglicher Kombinationen
    '****************************************************
    Public Function No_of_Combinations() As Integer

        If CES_T_Modus = Common.Constants.CES_T_MODUS.One_Combi Then
            No_of_Combinations = 1
        Else
            Dim i As Integer
            No_of_Combinations = List_Locations(0).List_Massnahmen.GetLength(0)
            For i = 1 To List_Locations.GetUpperBound(0)
                No_of_Combinations = No_of_Combinations * List_Locations(i).List_Massnahmen.GetLength(0)
            Next
        End If

    End Function

    'Überprüft ob und welcher TestModus aktiv ist
    'Beschreibung:
    '********************************************
    Public Function Set_TestModus() As Common.Constants.CES_T_MODUS

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
            Set_TestModus = Common.Constants.CES_T_MODUS.No_Test
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
            Set_TestModus = Common.Constants.CES_T_MODUS.One_Combi
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
            Set_TestModus = Common.Constants.CES_T_MODUS.All_Combis
            Exit Function
        End If

        Throw New Exception("Fehler bei der angabe des Testmodus")

    End Function

    'Holt sich im Falle des Testmodus 1 den Pfad aus der .CES Datei
    '**************************************************************
    Public Sub get_TestPath(ByRef Path() As Integer)
        Dim i, j As Integer

        For i = 0 To Path.GetUpperBound(0)
            Path(i) = -7
            For j = 0 To List_Locations(i).List_Massnahmen.GetUpperBound(0)
                If List_Locations(i).List_Massnahmen(j).TestModus = 1 Then
                    Path(i) = j
                End If
            Next
        Next

    End Sub


    'Holt sich im Falle des Testmodus 1 den Pfad aus der .CES Datei
    '**************************************************************
    Public Function TestPath() As Integer()

        Dim Array(List_Locations.GetUpperBound(0)) As Integer

        Dim i, j As Integer

        For i = 0 To Array.GetUpperBound(0)
            Dim count As Integer = 0
            Array(i) = -7
            For j = 0 To List_Locations(i).List_Massnahmen.GetUpperBound(0)
                If List_Locations(i).List_Massnahmen(j).TestModus = 1 Then
                    Array(i) = j
                    count += 1
                End If
            Next
            If count > 1 Then Array(i) = -7
        Next

        TestPath = Array.Clone

    End Function

    'Die Elemente werden pro Location im Child gespeichert
    '*****************************************************
    Public Sub Identify_Measures_Elements_Parameters(ByVal No_Loc As Integer, ByVal No_Measure As Integer, ByRef Measure As String, ByRef Elements() As String, ByRef PES_OptPara() As Common.OptParameter)

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

        '3. Die Parameter werden Ermittelt
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        x = 0
        For i = 0 To Elements.GetUpperBound(0)
            For j = 0 To List_OptParameter.GetUpperBound(0)
                If Elements(i) = Left(List_OptParameter(j).Bezeichnung, 4) Then
                    ReDim Preserve PES_OptPara(x)
                    PES_OptPara(x) = List_OptParameter(j).Clone()
                    x += 1
                End If
            Next
        Next
        If x = 0 Then ReDim Preserve PES_OptPara(-1)

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
    Public Sub PREPARE_Evaluation_CES(ByVal Path() As Integer, ByVal Elements() As String)

        'Setzt den Aktuellen Pfad
        Akt.Path = Path

        'Die elemente werden an die Kostenkalkulation übergeben
        CType(Me, EVO.Apps.BlueM).SKos1.Akt_Elemente = Elements

        'Ermittelt das aktuelle_ON_OFF array
        Call Prepare_Verzweigung_ON_OFF()

        'Schreibt die neuen Verzweigungen
        Call Me.Write_Verzweigungen()

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
            Dim TMP_OptPara() As EVO.Common.OptParameter
            ReDim TMP_OptPara(List_OptParameter.GetUpperBound(0))

            count = 0
            For i = 0 To List_OptParameter.GetUpperBound(0)
                For j = 0 To List_ModellParameter.GetUpperBound(0)
                    If List_OptParameter(i).Bezeichnung = List_ModellParameter(j).OptParameter Then
                        TMP_OptPara(count) = List_OptParameter(i).Clone()
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
                List_OptParameter(i) = TMP_OptPara(i).Clone()
            Next

        End If

    End Function

    'Setzt die Listen nach der Evaluierung wieder zurück auf alles was in den Eingabedateien steht
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
            List_OptParameter(i) = List_OptParameter_Save(i).Clone()
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

    'EVO-Parameterübergabe die Standard Parameter werden aus den Listen der OptPara und OptZiele ermittelt
    '*****************************************************************************************************
    Public Sub Parameter_Uebergabe(ByRef globalAnzPar As Short, ByRef mypara() As EVO.Common.OptParameter)

        Dim i As Integer

        'Anzahl Optimierungsparameter übergeben
        globalAnzPar = Me.List_OptParameter.GetLength(0)

        'Parameter übergeben
        ReDim mypara(globalAnzPar - 1)
        For i = 0 To globalAnzPar - 1
            mypara(i) = Me.List_OptParameter(i).Clone()
        Next

    End Sub

    'Evaluierung des SimModells für ParameterOptimierung - Steuerungseinheit
    '***********************************************************************
    Public Sub PREPARE_Evaluation_PES(ByVal myPara() As EVO.Common.OptParameter)

        Dim i As Short

        'Mutierte Parameter an OptParameter übergeben
        For i = 0 To Me.List_OptParameter.GetUpperBound(0)
            Me.List_OptParameter(i).Xn = myPara(i).Xn
        Next

        'Mutierte Parameter in Eingabedateien schreiben
        Call Write_ModellParameter()

    End Sub

    'Evaluierung des SimModells für ParameterOptimierung - Steuerungseinheit
    'HACK: Überladene Methode für altes Parameterarray (BUG 257)
    '***********************************************************************
    Public Sub PREPARE_Evaluation_PES(ByVal myPara() As Double)

        Dim i As Short

        'Mutierte Parameter an OptParameter übergeben
        For i = 0 To Me.List_OptParameter.GetUpperBound(0)
            List_OptParameter(i).Xn = myPara(i)
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
        Dim i, j As Integer
        Dim Zeilenarray() As String
        Dim Zeile As String
        Dim StrLeft As String
        Dim StrRight As String
        Dim DateiPfad As String
        Dim WriteCheck As Boolean = False

        'ModellParameter aus OptParametern kalkulieren()
        Call OptParameter_to_ModellParameter()

        'Alle ModellParameter durchlaufen
        For i = 0 To List_ModellParameter.GetUpperBound(0)
            WriteCheck = True

            DateiPfad = WorkDir & Datensatz & "." & List_ModellParameter(i).Datei
            'Datei öffnen
            Dim FiStr As FileStream = New FileStream(DateiPfad, FileMode.Open, IO.FileAccess.ReadWrite)
            Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
            Dim StrReadSync As TextReader = TextReader.Synchronized(StrRead)

            'Anzahl der Zeilen feststellen
            AnzZeil = 0
            On Error GoTo Handler
            Do
                Zeile = StrRead.ReadLine.ToString
                AnzZeil += 1
            Loop Until StrRead.Peek() = -1
Handler:
            If AnzZeil = 0 Then
                Throw New Exception("Fehler beim lesen der Transportstreckendatei (.TRS). Sie könnte leer sein.")
            End If

            ReDim Zeilenarray(AnzZeil - 1)

            'Datei komplett einlesen
            FiStr.Seek(0, SeekOrigin.Begin)
            For j = 0 To AnzZeil - 1
                Zeilenarray(j) = StrRead.ReadLine.ToString
            Next

            StrReadSync.Close()
            StrRead.Close()
            FiStr.Close()

            'Anzahl verfügbarer Zeichen
            AnzZeichen = List_ModellParameter(i).SpBis - List_ModellParameter(i).SpVon + 1

            'Zeile einlesen und splitten
            Zeile = Zeilenarray(List_ModellParameter(i).ZeileNr - 1)
            StrLeft = Zeile.Substring(0, List_ModellParameter(i).SpVon - 1)
            If (Zeile.Length > List_ModellParameter(i).SpBis) Then
                StrRight = Zeile.Substring(List_ModellParameter(i).SpBis)
            Else
                StrRight = ""
            End If

            'Wert auf verfügbare Stellen kürzen
            '----------------------------------
            'bestimmen des ganzzahligen Anteils, \-Operator ginge zwar theoretisch, ist aber für Zahlen < 1 nicht robust (warum auch immer)
            WertStr = Convert.ToString(List_ModellParameter(i).Wert - List_ModellParameter(i).Wert Mod 1.0, Common.Provider.FortranProvider)

            If (WertStr.Length > AnzZeichen) Then
                'Wert zu lang
                Throw New Exception("Der Wert des Modellparameters '" & List_ModellParameter(i).Bezeichnung & "' (" & WertStr & ") ist länger als die zur Verfügung stehende Anzahl von Zeichen!")

            ElseIf (WertStr.Length < AnzZeichen - 1) Then
                'Runden auf verfügbare Stellen: Anzahl der Stellen - Anzahl der Vorkommastellen - Komma
                WertStr = Convert.ToString(Math.Round(List_ModellParameter(i).Wert, AnzZeichen - WertStr.Length - 1), Common.Provider.FortranProvider)

            Else
                'Ganzzahligen Wert benutzen
            End If

            'Falls erforderlich, Wert mit Leerzeichen füllen
            If (WertStr.Length < AnzZeichen) Then
                For j = 1 To AnzZeichen - WertStr.Length
                    WertStr = " " & WertStr
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

        If Not WriteCheck Then
            Throw New Exception("Es wurde kein Parameter geschrieben.")
        End If

    End Sub

    'Evaluiert die Kinderchen mit Hilfe des Simulationsmodells
    '*********************************************************
    Public Sub SIM_Ergebnis_auswerten(ByRef Indi As Common.Individuum)

        Dim i As Short

        'Lesen der Relevanten Parameter aus der wel Datei
        Call ReadSimResult()

        'Qualitätswerte berechnen
        For i = 0 To Common.Manager.NumFeatures - 1
            Indi.Features(i) = CalculateFeature(Common.Manager.List_Featurefunctions(i))
        Next

        'Constraints berechnen
        For i = 0 To Common.Manager.NumConstraints - 1
            Indi.Constraints(i) = CalculateConstraint(Common.Manager.List_Constraintfunctions(i))
        Next

        'Lösung abspeichern
        Call Me.OptResult.addSolution(Indi)

    End Sub

    'VG_ Test Tagesganglinie mit Autokalibrierung
    'VG *****************************************
    'VG Beta-Version - erlaubt Kalirbierung der Tagesganlinie
    'VG dafür muss für den jeweiligen Tagesgangwert in der .mod Datei in der Spalte "Elem" "TGG_QH" eingetragen werden
    'VG Vorschlag: Aktivierung der kalibrierung des Tagesganlinie über einen Schalter, damit diese Funktion nicht bei jeder optimierung aufgerufen wird
    Protected Sub VG_Kalibrierung_Tagesganglinie()
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

    'ModellParameter aus OptParametern errechnen
    '*******************************************
    Protected Sub OptParameter_to_ModellParameter()
        Dim i As Integer
        Dim j As Integer

        'VG ---------- Zusatzroutine für kalibrierung des Tagesgangs
        VG_Kalibrierung_Tagesganglinie()
        'VG ---------- Ende

        For i = 0 To List_ModellParameter.GetUpperBound(0)
            For j = 0 To List_OptParameter.GetUpperBound(0)
                If List_ModellParameter(i).OptParameter = List_OptParameter(j).Bezeichnung Then
                    List_ModellParameter(i).Wert = List_OptParameter(j).RWert * List_ModellParameter(i).Faktor
                End If
            Next
        Next
    End Sub


    'SimModell ausführen (simulieren)
    '********************************
    Public MustOverride Function launchSim(ByVal Thread_ID As Integer, ByVal Child_ID As Integer) As Boolean
    Public MustOverride Function launchFree(ByRef Thread_ID As Integer) As Boolean
    Public MustOverride Function launchReady(ByRef Thread_ID As Integer, ByRef SimIsOK As Boolean, ByVal Child_ID As Integer) As Boolean


    'Simulationsergebnis verarbeiten
    '-------------------------------
    Public MustOverride Sub ReadSimResult()

#End Region 'Evaluierung

#Region "Qualitätswertberechnung"

    'Phänotypberechnung
    '##################

    'Berechnung der Feature Funktionen
    '*********************************
    Public MustOverride Function CalculateFeature(ByVal feature As Common.Featurefunction) As Double

    'Featurewert berechnen: Feature Typ = Reihe
    '******************************************
    'BUG 218: Konstante und gleiche Zeitschrittweiten vorausgesetzt!
    Protected Function CalculateFeature_Reihe(ByVal feature As Common.Featurefunction, ByVal SimReihe As Wave.Zeitreihe) As Double

        Dim QWert As Double
        Dim i, j As Integer
        Dim ZeitschritteBisStart As Integer
        Dim ZeitschritteEval As Integer
        Dim Versatz As Integer

        'Bestimmen der Zeitschritte bis Start des Evaluierungszeitraums
        ZeitschritteBisStart = (feature.EvalStart - feature.RefReihe.XWerte(0)).TotalMinutes / Me.SimDT.TotalMinutes
        'Bestimmen der Zeitschritte des Evaluierungszeitraums
        ZeitschritteEval = (feature.EvalEnde - feature.EvalStart).TotalMinutes / Me.SimDT.TotalMinutes

        'Überprüfen ob simulierte Zeitreihe evtl. anderen Startzeitpunkt 
        'als Simulations Startzeitpunkt hat (kann bei SMUSI vorkommen!)
        '
        'Falls ein Versatz der beiden Zeitreihen vorliegt wird j
        'zum entsprechenden Verschieben der Laufvariable i benutzt
        '---------------------------------------------------------------
        'Fallunterscheidung je nach Zeitschrittweite
        If (Me.SimDT.TotalMinutes >= 1440) Then
            'Bei dt >= 1d ist Versatz unerheblich
            j = 0
        Else
            Versatz = (SimReihe.XWerte(0) - feature.RefReihe.XWerte(0)).TotalMinutes / Me.SimDT.TotalMinutes
            If Versatz < 0 Then
                j = -1 * Versatz
            ElseIf Versatz = 0 Then
                j = 0
            Else
                j = Versatz
            End If
        End If

        'Fallunterscheidung Zielfunktion
        '-------------------------------
        Select Case feature.Funktion

            Case "AbQuad"
                'Summe der Fehlerquadrate
                '------------------------
                For i = ZeitschritteBisStart To ZeitschritteBisStart + ZeitschritteEval
                    QWert += (feature.RefReihe.YWerte(i) - SimReihe.YWerte(i + j)) * (feature.RefReihe.YWerte(i) - SimReihe.YWerte(i + j))
                Next

            Case "Diff"
                'Summe der Fehler
                '----------------
                For i = ZeitschritteBisStart To ZeitschritteBisStart + ZeitschritteEval
                    QWert += Math.Abs(feature.RefReihe.YWerte(i) - SimReihe.YWerte(i + j))
                Next

            Case "Volf"
                'Volumenfehler
                '-------------
                Dim VolSim As Double = 0
                Dim VolZiel As Double = 0
                For i = ZeitschritteBisStart To ZeitschritteBisStart + ZeitschritteEval
                    VolSim += SimReihe.YWerte(i + j)
                    VolZiel += feature.RefReihe.YWerte(i)
                Next
                'Umrechnen in echtes Volumen
                VolSim *= Me.SimDT.TotalSeconds
                VolZiel *= Me.SimDT.TotalSeconds
                'Differenz bilden
                QWert = Math.Abs(VolZiel - VolSim)

            Case "nUnter"
                'Relative Anzahl der Zeitschritte mit Unterschreitungen (in Prozent)
                '-------------------------------------------------------------------
                Dim nUnter As Integer = 0
                For i = ZeitschritteBisStart To ZeitschritteBisStart + ZeitschritteEval
                    If (SimReihe.YWerte(i + j) < feature.RefReihe.YWerte(i)) Then
                        nUnter += 1
                    End If
                Next
                QWert = nUnter / ZeitschritteEval * 100

            Case "sUnter"
                'Summe der Unterschreitungen
                '---------------------------
                Dim sUnter As Double = 0
                For i = ZeitschritteBisStart To ZeitschritteBisStart + ZeitschritteEval
                    If (SimReihe.YWerte(i + j) < feature.RefReihe.YWerte(i)) Then
                        sUnter += feature.RefReihe.YWerte(i) - SimReihe.YWerte(i + j)
                    End If
                Next
                QWert = sUnter

            Case "nÜber"
                'Relative Anzahl der Zeitschritte mit Überschreitungen (in Prozent)
                '------------------------------------------------------------------
                Dim nUeber As Integer = 0
                For i = ZeitschritteBisStart To ZeitschritteBisStart + ZeitschritteEval
                    If (SimReihe.YWerte(i + j) > feature.RefReihe.YWerte(i)) Then
                        nUeber += 1
                    End If
                Next
                QWert = nUeber / ZeitschritteEval * 100

            Case "sÜber"
                'Summe der Überschreitungen
                '--------------------------
                Dim sUeber As Double = 0
                For i = ZeitschritteBisStart To ZeitschritteBisStart + ZeitschritteEval
                    If (SimReihe.YWerte(i + j) > feature.RefReihe.YWerte(i)) Then
                        sUeber += SimReihe.YWerte(i + j) - feature.RefReihe.YWerte(i)
                    End If
                Next
                QWert = sUeber

            Case "NashSutt"
                'Nash Sutcliffe
                '--------------
                'Mittelwert bilden
                Dim Qobs_quer, zaehler, nenner As Double
                For i = ZeitschritteBisStart To ZeitschritteBisStart + ZeitschritteEval
                    Qobs_quer += feature.RefReihe.YWerte(i)
                Next
                Qobs_quer = Qobs_quer / (ZeitschritteEval)
                For i = ZeitschritteBisStart To ZeitschritteBisStart + ZeitschritteEval
                    zaehler += (feature.RefReihe.YWerte(i) - SimReihe.YWerte(i + j)) * (feature.RefReihe.YWerte(i) - SimReihe.YWerte(i + j))
                    nenner += (feature.RefReihe.YWerte(i) - Qobs_quer) * (feature.RefReihe.YWerte(i) - Qobs_quer)
                Next
                'abgeänderte Nash-Sutcliffe Formel: 0 als Zielwert (1- weggelassen)
                QWert = zaehler / nenner

            Case Else
                Throw New Exception("Die Zielfunktion '" & feature.Funktion & "' wird nicht unterstützt!")

        End Select

        Return QWert

    End Function

    'Qualitätswert berechnen: Feature Typ = Wert
    '*******************************************
    Protected Function CalculateFeature_Wert(ByVal feature As Common.Featurefunction, ByVal SimReihe As Wave.Zeitreihe) As Double

        Dim QWert As Double
        Dim i As Integer

        'Simulationsreihe auf Evaluierungszeitraum kürzen
        Call SimReihe.Cut(feature.EvalStart, feature.EvalEnde)

        'Simulationswert aus Simulationsergebnis berechnen
        Dim SimWert As Double
        SimWert = SimReihe.getWert(feature.WertFunktion)

        'QWert berechnen
        '---------------
        'Fallunterscheidung Zielfunktion
        Select Case feature.Funktion

            Case "AbQuad"
                'Summe der Fehlerquadrate
                '------------------------
                QWert = (feature.RefWert - SimWert) * (feature.RefWert - SimWert)

            Case "Diff"
                'Summe der Fehler
                '----------------
                QWert = Math.Abs(feature.RefWert - SimWert)

            Case "nUnter"
                'Relative Anzahl der Zeitschritte mit Unterschreitungen (in Prozent)
                '-------------------------------------------------------------------
                Dim nUnter As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) < feature.RefWert) Then
                        nUnter += 1
                    End If
                Next
                QWert = nUnter / SimReihe.Length * 100

            Case "nÜber"
                'Relative Anzahl der Zeitschritte mit Überschreitungen (in Prozent)
                '------------------------------------------------------------------
                Dim nUeber As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) > feature.RefWert) Then
                        nUeber += 1
                    End If
                Next
                QWert = nUeber / SimReihe.Length * 100

            Case "sUnter"
                'Summe der Unterschreitungen
                '---------------------------
                Dim sUnter As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) < feature.RefWert) Then
                        sUnter += feature.RefWert - SimReihe.YWerte(i)
                    End If
                Next
                QWert = sUnter

            Case "sÜber"
                'Summe der Überschreitungen
                '--------------------------
                Dim sUeber As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) > feature.RefWert) Then
                        sUeber += SimReihe.YWerte(i) - feature.RefWert
                    End If
                Next
                QWert = sUeber

            Case Else
                Throw New Exception("Die Zielfunktion '" & feature.Funktion & "' wird für Werte nicht unterstützt!")

        End Select

        Return QWert

    End Function

#End Region 'Qualitätswertberechnung

#Region "Constraintberechnung"

    'Constraint berechnen (Constraint < 0 ist Grenzverletzung)
    '*********************************************************
    Public Function CalculateConstraint(ByVal constr As Common.Constraintfunction) As Double

        Dim i As Integer

        'Simulationsergebnis auslesen
        Dim SimReihe As Wave.Zeitreihe
        SimReihe = Me.SimErgebnis(constr.SimGr)

        'Fallunterscheidung GrenzTyp (Wert/Reihe)
        Select Case constr.Typ

            Case "Wert"
                'zuerst Simulationswert aus Simulationsergebnis berechnen
                Dim SimWert As Double
                SimWert = SimReihe.getWert(constr.WertFunktion)

                'Grenzverletzung berechnen
                If (constr.GrenzPos = "Obergrenze") Then
                    CalculateConstraint = constr.GrenzWert - SimWert

                ElseIf (constr.GrenzPos = "Untergrenze") Then
                    CalculateConstraint = SimWert - constr.GrenzWert

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

                CalculateConstraint = summe

        End Select

        Return CalculateConstraint

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

        'Zeile mit Spaltenüberschriften überspringen
        Zeile = StrRead.ReadLine.ToString

        For j = 0 To AnzZeil - 1
            Zeile = StrRead.ReadLine.ToString()
            PRB(j, 0) = Convert.ToDouble(Zeile.Substring(2, 10))        'X-Wert
            PRB(j, 1) = Convert.ToDouble(Zeile.Substring(13, 8))        'P(Jahr)
        Next
        StrReadSync.Close()
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

#Region "Multithreading"

    'Datensätze für Multithreading kopieren
    '**************************************
    Public Sub copyDatensatz(ByVal n_Proz As Integer)

        Dim i As Integer = 1
        'Dim j As Integer

        For i = 0 To n_Proz - 1
            Dim Source As String = WorkDir
            Dim Dest As String = System.Windows.Forms.Application.StartupPath() & "\Thread_" & i & "\"

            'Löschen um den Inhalt zu entsorgen
            If Directory.Exists(Dest) Then
                Call EVO.Common.FileHelper.purgeReadOnly(Dest)
                Directory.Delete(Dest, True)
            End If

            My.Computer.FileSystem.CopyDirectory(Source, Dest, True)
            Call EVO.Common.FileHelper.purgeReadOnly(Dest)
            Call Directory.Delete(Dest & "\.svn", true)

        Next

    End Sub

    'Datensätze für Multithreading löschen
    '*************************************
    Public Sub deleteDatensatz(byVal n_Proz As Integer)
        Dim i As Integer
        For i = 1 to n_Proz

            If Directory.Exists(System.Windows.Forms.Application.StartupPath() & "\Thread_" & i) Then
                Directory.Delete(System.Windows.Forms.Application.StartupPath() & "\Thread_" & i, True)
            End If
        Next
    End Sub

    'Gibt den aktuellen Datensatz zurück
    '***********************************
    Public Function getWorkDir(ByVal Thread_ID as Integer) As String

        getWorkDir = ""
        
        getWorkDir = System.Windows.Forms.Application.StartupPath() & "\Thread_" & Thread_ID & "\"

        Return getWorkDir
        
    End Function

#End Region  'Multithreading

#End Region 'Methoden

End Class
