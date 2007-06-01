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
'**** Letzte Änderung: April 2007                                           ****
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
        Public Overrides Function toString() As String
            return Bezeichnung
        End Function
    End Structure

    Public OptParameterListe() As OptParameter = {} 'Liste der Optimierungsparameter

    'ModellParameter
    '---------------
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
    '-----------------
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
        Public Overrides Function toString() As String
            Return Bezeichnung
        End Function
    End Structure

    Public OptZieleListe() As OptZiel = {}          'Liste der Zielfunktionnen

    'Ergebnisdatenbank
    '-----------------
    Private db As OleDb.OleDbConnection

    'Kombinatorik
    '------------
    Public SKos1 As New SKos()
    Public Path_Aktuell() As Integer
    Public VER_ONOFF(,) As Object

    Public Structure Massnahme
        Public Name As String
        Public Schaltung(,) As String
        Public KostenTyp As Integer
        Public Bauwerke() As String
    End Structure

    Public Structure Lokation
        Public Name As String
        Public MassnahmeListe() As Massnahme
    End Structure

    Public LocationList() As Lokation
    Public VerzweigungsDatei(,) As String

    'Public Schaltung(2, 1) As Object
    'Public Maßnahme As Collection
    'Public Kombinatorik As Collection

#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

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

        If AnzZiele > 2 Then
            Throw New Exception("Entweder iste die Anzahl der Ziele zu Groß oder ein Fehler in der Ziele Datei (Z.B.: Leere Zeile am Ende der Datei")
        End If

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

        'Falls mit Reihen verglichen werden soll werden hier die Reihen eingelesen
        Dim ZielStart As Date
        Dim ZielEnde As Date

        For i = 0 To AnzZiele - 1
            If (OptZieleListe(i).ZielTyp = "Reihe" Or OptZieleListe(i).ZielTyp = "IHA") Then

                'Dateiendung der Zielreihendatei bestimmen und Reihe einlesen
                ext = OptZieleListe(i).ZielReihePfad.Substring(OptZieleListe(i).ZielReihePfad.LastIndexOf(".") + 1)
                Select Case (ext.ToUpper)
                    Case "WEL"
                        IsOK = Read_WEL(OptZieleListe(i).ZielReihePfad, OptZieleListe(i).ZielGr, OptZieleListe(i).ZielReihe)
                    Case "ZRE"
                        IsOK = Read_ZRE(OptZieleListe(i).ZielReihePfad, OptZieleListe(i).ZielReihe)
                    Case "PRB"
                        IsOK = Read_PRB(OptZieleListe(i).ZielReihePfad, OptZieleListe(i).ZielGr, OptZieleListe(i).ZielReihe)
                    Case Else
                        IsOK = False
                End Select

                If (IsOK = False) Then
                    Throw New Exception("Fehler beim einlesen der Zielreihe in '" & OptZieleListe(i).ZielReihePfad & "'" & Chr(13) & Chr(10) & "Ein Fehler könnten Leerzeichen in der letzten Zeile der Datei sein")
                End If

                'Zielreihe entsprechend dem Simulationszeitraum kürzen (nur bei WEL und ZRE)
                '---------------------------------------------------------------------------
                If (ext.ToUpper = "WEL" Or ext.ToUpper = "ZRE") Then

                    ZielStart = OptZieleListe(i).ZielReihe(0, 0)
                    ZielEnde = OptZieleListe(i).ZielReihe(OptZieleListe(i).ZielReihe.GetUpperBound(0), 0)

                    If (ZielStart > Me.SimStart Or ZielEnde < Me.SimEnde) Then
                        'Zielreihe deckt Simulationszeitraum nicht ab
                        Throw New Exception("Die Zielreihe in '" & OptZieleListe(i).ZielReihePfad & "' deckt den Simulationszeitraum nicht ab!")
                    Else
                        'Länge der Simulationszeitreihe:
                        Dim length_n As Integer = ((Me.SimEnde - Me.SimStart).TotalSeconds / Me.SimDT.TotalSeconds) + 1
                        'Zielreihe kopieren und redimensionieren
                        Dim tmpArray(,) As Object = OptZieleListe(i).ZielReihe
                        ReDim OptZieleListe(i).ZielReihe(length_n - 1, 1)
                        'Abstand zwischen Start von Simreihe und Zielreihe bestimmen
                        Dim offset_t As TimeSpan = Me.SimStart - ZielStart
                        Dim dt As TimeSpan = tmpArray(1, 0) - tmpArray(0, 0) 'BUG 105: Es wird von konstanten Zeitschritten ausgegangen
                        Dim offset_n As Integer = offset_t.TotalSeconds / dt.TotalSeconds
                        'Simulationszeitraum zurück in Zielreihe kopieren
                        Array.Copy(tmpArray, offset_n * 2, OptZieleListe(i).ZielReihe, 0, length_n * 2)
                    End If

                End If

            End If
        Next
    End Sub

#End Region 'Eingabedateien einlesen

#Region "Prüfung der Eingabedateien"

    'Validierungsfunktion der Kombinatorik Prüft ob Verbraucher an zwei Standorten Dopp vorhanden sind
    '*************************************************************************************************
    Public MustOverride Sub Combinatoric_is_Valid()

    'Mehrere Prüfungen ob die .VER Datei des BlueM und der .CES Datei auch zusammenpassen
    '************************************************************************************
    Public MustOverride Sub CES_fits_to_VER()

#End Region 'Prüfung der Eingabedateien

#Region "Evaluierung"

    'EVO-Parameterübergabe
    '*********************
    Public Sub Parameter_Uebergabe(ByRef globalAnzPar As Short, ByRef globalAnzZiel As Short, ByRef globalAnzRand As Short, ByRef mypara(,) As Double)

        Dim i As Integer

        'Anzahl Optimierungsparameter übergeben
        globalAnzPar = Me.OptParameterListe.GetLength(0)

        'Parameterwerte übergeben
        'BUG 57: mypara() fängt bei 1 an!
        ReDim mypara(globalAnzPar, 1)
        For i = 1 To globalAnzPar
            mypara(i, 1) = Me.OptParameterListe(i - 1).SKWert
        Next

        'globale Anzahl der Ziele muss hier auf Länge der Zielliste gesetzt werden
        globalAnzZiel = Me.OptZieleListe.GetLength(0)

        'TODO: Randbedingungen
        globalAnzRand = 2

    End Sub

    'Evaluierung des SimModells für Parameter Optimierung - Steuerungseinheit
    '************************************************************************
    Public Sub Eval_Sim_ParaOpt(ByVal GlobalAnzPar As Short, ByVal GlobalAnzZiel As Short, ByVal mypara As Double(,), ByVal durchlauf As Integer, ByVal ipop As Short, ByRef QN As Double(), ByRef TChart1 As Steema.TeeChart.TChart)

        Dim i As Short

        'Mutierte Parameter an OptParameter übergeben
        For i = 1 To GlobalAnzPar                                   'BUG 57: mypara(,) fängt bei 1 an!
            OptParameterListe(i - 1).SKWert = mypara(i, 1)          'OptParameterListe(i-1) weil Array bei 0 anfängt!
        Next

        'Mutierte Parameter in Eingabedateien schreiben
        Call ModellParameter_schreiben()

        'Modell Starten
        Call launchSim()

        'Qualitätswerte berechnen und Rückgabe an den OptiAlgo
        For i = 0 To GlobalAnzZiel - 1                              'BUG 57: QN() fängt bei 1 an!
            OptZieleListe(i).QWertTmp = QWert(OptZieleListe(i))
            QN(i + 1) = OptZieleListe(i).QWertTmp
        Next

        'Qualitätswerte im TeeChart zeichnen
        Select Case GlobalAnzZiel
            Case 1
                TChart1.Series(ipop).Add(durchlauf, OptZieleListe(0).QWertTmp)
            Case 2
                TChart1.Series(0).Add(OptZieleListe(0).QWertTmp, OptZieleListe(1).QWertTmp, "")
            Case 3
                'BUG 66: Zeichnen von mehr als 2 Zielfunktionen
                Throw New Exception("Das Zeichnen von mehr als 2 Zielfunktionen wird bisher nicht unterstützt")
                'Call Zielfunktion_zeichnen_MultiObPar_3D(BlueM1.OptZieleListe(0).QWertTmp, BlueM1.OptZieleListe(1).QWertTmp, BlueM1.OptZieleListe(2).QWertTmp)
            Case Else
                'BUG 66: Zeichnen von mehr als 2 Zielfunktionen
                Throw New Exception("Das Zeichnen von mehr als 2 Zielfunktionen wird bisher nicht unterstützt")
                'Call Zielfunktion_zeichnen_MultiObPar_XD()
        End Select

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
        For i As Integer = 0 To ModellParameterListe.GetUpperBound(0)

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

        Next

    End Sub

    'ModellParameter aus OptParametern errechnen
    '*******************************************
    Protected Sub OptParameter_to_ModellParameter()
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

    'Evaluierung des SimModells für Kombinatorik Optimierung - Steuerungseinheit
    '***************************************************************************
    Public Function Eval_Sim_CombiOpt(ByVal Path() As Integer, ByVal n_Ziele As Short, ByVal durchlauf As Integer, ByVal ipop As Short, ByRef Quality As Double(), ByRef TChart1 As Steema.TeeChart.TChart) As Boolean
        Dim i As Short

        'Erstellt die aktuelle Bauerksliste und überträgt sie zu SKos
        Call Define_aktuelle_Bauwerke(Path)

        'Ermittelt das aktuelle_ON_OFF array
        Call Verzweigung_ON_OFF(Path)

        'Schreibt die neuen Verzweigungen
        Call Write_Verzweigungen()

        'Modell Starten
        Call launchSim()

        'Qualitätswerte berechnen und Rückgabe an den OptiAlgo
        For i = 0 To n_Ziele - 1                                    'BUG 57: QN() fängt bei 1 an!
            OptZieleListe(i).QWertTmp = QWert(OptZieleListe(i))
            Quality(i) = OptZieleListe(i).QWertTmp
        Next

    End Function


    'Die Liste mit den aktuellen Bauwerken des Kindes wird erstellt und in SKos geschrieben
    '**************************************************************************************
    Public MustOverride Sub Define_aktuelle_Bauwerke(ByVal Path() As Integer)

    'Ermittelt das aktuelle Verzweigungsarray
    '****************************************
    Public MustOverride Sub Verzweigung_ON_OFF(ByVal Path() As Integer)

    'Schreibt die neuen Verzweigungen
    '********************************
    Public MustOverride Sub Write_Verzweigungen()

    'SimModell ausführen (simulieren)
    '********************************
    Public MustOverride Sub launchSim()

#End Region 'Evaluierung

#Region "Qualitätswertberechnung"

    'Qualitätswertberechnung
    '#######################

    'Berechnung des Qualitätswerts (Zielwert)
    '****************************************
    Public Function QWert(ByVal OptZiel As OptZiel) As Double

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
    Protected Overridable Function QWert_WEL(ByVal OptZiel As OptZiel) As Double

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
    Protected Function QWert_Reihe(ByVal OptZiel As OptZiel, ByVal SimReihe As Object(,)) As Double

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

            Case Else
                Throw New Exception("Die Zielfunktion '" & OptZiel.ZielFkt & "' wird nicht unterstützt!")

        End Select

        Return QWert

    End Function

    'Qualitätswert berechnen: Zieltyp = Wert
    '***************************************
    Protected Function QWert_Wert(ByVal OptZiel As OptZiel, ByVal SimReihe As Object(,)) As Double

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

            Case Else
                Throw New Exception("Die Zielfunktion '" & OptZiel.ZielFkt & "' wird für Werte nicht unterstützt!")

        End Select

        Return QWert

    End Function

    'Qualitätswert aus PRB-Datei
    '***************************
    Private Function QWert_PRB(ByVal OptZiel As OptZiel) As Double

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
        For i = 0 To OptZieleListe.GetUpperBound(0)
            If (i > 0) Then
                fieldnames &= ", "
            End If
            fieldnames &= "'" & OptZieleListe(i).Bezeichnung & "' DOUBLE"
        Next
        'Tabelle anpassen
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

        Call db_disconnect()

    End Function

#End Region 'Ergebnisdatenbank

#End Region 'Methoden

End Class
