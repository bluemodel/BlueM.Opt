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

    'Generelle Eigenschaften
    '-----------------------
    ''' <summary>
    ''' Eine StringCollection mit allen Dateiendungen (ohne Punkt), die in einem Datensatz vorkommen k�nnen
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Der erste Wert des Arrays wird als Filter f�r OpenFile-Dialoge verwendet</remarks>
    Public MustOverride ReadOnly Property DatensatzDateiendungen() As Collections.Specialized.StringCollection

    Public Datensatz As String                           'Name des zu simulierenden Datensatzes

    Public WorkDir_Current As String                     'aktuelles Arbeits-/Datensatzverzeichnis
    Public WorkDir_Original As String                    'Original-Arbeits-/Datensatzverzeichnis

    Public SimStart As DateTime                          'Anfangsdatum der Simulation
    Public SimEnde As DateTime                           'Enddatum der Simulation
    Public SimDT As TimeSpan                             'Zeitschrittweite der Simulation

    Public SimErgebnis As Collection                     'Simulationsergebnis als Collection von Wave.Zeitreihe Objekten

    'Das Problem
    '-----------
    Protected mProblem As EVO.Common.Problem

    'Die Einstellungen
    Protected mSettings As EVO.Common.EVO_Settings

    Protected Structure Aktuell
        Public OptPara() As Double
        Public ModPara() As Double
        Public Path() As Integer
        Public Measures() As String
        Public VER_ONOFF(,) As Object
    End Structure

    Protected Akt As Aktuell

    'Ergebnisspeicher
    '----------------
    Public OptResult As EVO.OptResult.OptResult             'Optimierungsergebnis
    Public OptResultRef As EVO.OptResult.OptResult          'Vergleichsergebnis

    Public VerzweigungsDatei(,) As String                   'Gibt die PathSize an f�r jede Pfadstelle


#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

#Region "Initialisierung"

    'Konstruktor
    '***********
    Public Sub New()

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
            Me.WorkDir_Current = Path.GetDirectoryName(pfad) & "\"
            Me.WorkDir_Original = Path.GetDirectoryName(pfad) & "\"
        Else
            Throw New Exception("Der Datensatz '" & pfad & "' existiert nicht!")
        End If

        'Simulationsdaten einlesen
        Call Me.Read_SimParameter()

    End Sub

    Public Overridable Sub setProblem(ByRef prob As EVO.Common.Problem)

        'Problem speichern
        Me.mProblem = prob

        'Je nach Problem weitere Vorbereitungen treffen
        Select Case Me.mProblem.Method

            Case Common.METH_PES, Common.METH_SENSIPLOT
                'nix

            Case Common.METH_CES, Common.METH_HYBRID

                'Verzweigungs Datei einlesen
                Call Me.Read_Verzweigungen()
                '�berpr�fen der Kombinatorik
                Call Me.mProblem.Validate_Combinatoric()
                If (TypeOf Me Is BlueM) Then
                    'Pr�fen ob Kombinatorik und BlueM-Verzweigungsdatei zusammenpassen
                    Call CType(Me, BlueM).Validate_CES_fits_to_VER()
                End If

        End Select

        'Aktuelle Parameterlisten dimensionieren
        ReDim Me.Akt.OptPara(Me.mProblem.NumParams - 1)
        ReDim Me.Akt.ModPara(Me.mProblem.List_ModellParameter.GetUpperBound(0))

        'Startwerte der OptParameter setzen
        For i = 0 To Me.mProblem.NumParams - 1
            Me.Akt.OptPara(i) = Me.mProblem.List_OptParameter(i).StartWert
        Next

        'Ergebnisspeicher initialisieren
        Me.OptResult = New EVO.OptResult.OptResult(Me.Datensatz, Me.mProblem)

    End Sub

    'Einstellungen setzen
    '********************
    Public Sub setSettings(ByRef settings As EVO.Common.EVO_Settings)

        'Settings speichern
        Me.mSettings = settings

    End Sub

#End Region 'Initialisierung

#Region "Eingabedateien lesen"

    'Simulationsparameter einlesen
    '*****************************
    Protected MustOverride Sub Read_SimParameter()

    'Liest die Verzweigungen aus BlueM in ein Array ein
    'Und Dimensioniert das Verzweigungsarray
    '*******************************************************
    Protected MustOverride Sub Read_Verzweigungen()

#End Region 'Eingabedateien einlesen

#Region "Pr�fung der Eingabedateien"

#End Region 'Pr�fung der Eingabedateien

#Region "Kombinatorik"

    'Holt sich im Falle des Testmodus 1 den Pfad aus der .CES Datei
    '**************************************************************
    Public Sub get_TestPath(ByRef Path() As Integer)
        Dim i, j As Integer

        For i = 0 To Path.GetUpperBound(0)
            Path(i) = -7
            For j = 0 To Me.mProblem.List_Locations(i).List_Massnahmen.GetUpperBound(0)
                If Me.mProblem.List_Locations(i).List_Massnahmen(j).TestModus = 1 Then
                    Path(i) = j
                End If
            Next
        Next

    End Sub


    'Holt sich im Falle des Testmodus 1 den Pfad aus der .CES Datei
    '**************************************************************
    Public Function TestPath() As Integer()

        Dim Array(Me.mProblem.List_Locations.GetUpperBound(0)) As Integer

        Dim i, j As Integer

        For i = 0 To Array.GetUpperBound(0)
            Dim count As Integer = 0
            Array(i) = -7
            For j = 0 To Me.mProblem.List_Locations(i).List_Massnahmen.GetUpperBound(0)
                If (Me.mProblem.List_Locations(i).List_Massnahmen(j).TestModus = 1) Then
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

        '1. Die Ma�nahme wird ermittelt
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        Measure = Me.mProblem.List_Locations(No_Loc).List_Massnahmen(No_Measure).Name
        'ToDo: Measure aktuell ist hier noch redundant!
        ReDim Preserve Akt.Measures(Me.mProblem.List_Locations.GetUpperBound(0))
        Akt.Measures(No_Loc) = Measure

        '2. Die Elemente werden Ermittelt
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        x = 0
        For i = 0 To Me.mProblem.List_Locations(No_Loc).List_Massnahmen(No_Measure).Bauwerke.GetUpperBound(0)
            If (Not Me.mProblem.List_Locations(No_Loc).List_Massnahmen(No_Measure).Bauwerke(i) = "X") Then
                ReDim Preserve Elements(x)
                Elements(x) = Me.mProblem.List_Locations(No_Loc).List_Massnahmen(No_Measure).Bauwerke(i)
                x += 1
            End If
        Next

        '3. Die Parameter werden Ermittelt
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        x = 0
        For i = 0 To Elements.GetUpperBound(0)
            For j = 0 To Me.mProblem.List_OptParameter.GetUpperBound(0)
                If Elements(i) = Left(Me.mProblem.List_OptParameter(j).Bezeichnung, 4) Then
                    ReDim Preserve PES_OptPara(x)
                    PES_OptPara(x) = Me.mProblem.List_OptParameter(j).Clone()
                    x += 1
                End If
            Next
        Next
        If x = 0 Then ReDim Preserve PES_OptPara(-1)

    End Sub

    'Bereitet das SimModell f�r Kombinatorik Optimierung vor
    '*******************************************************
    Public Sub PREPARE_Evaluation_CES(ByVal Path() As Integer, ByVal Elements() As String)

        'Setzt den Aktuellen Pfad
        Akt.Path = Path

        'Aktuelle Parameterlisten neu dimensionieren
        ReDim Me.Akt.OptPara(Me.mProblem.NumParams - 1)
        ReDim Me.Akt.ModPara(Me.mProblem.List_ModellParameter.GetUpperBound(0))

        'Die elemente werden an die Kostenkalkulation �bergeben
        CType(Me, BlueM).SKos1.Akt_Elemente = Elements

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
            For y = 0 To Me.mProblem.List_Locations(x).List_Massnahmen(No).Schaltung.GetUpperBound(0)
                For z = 0 To Akt.VER_ONOFF.GetUpperBound(0)
                    If (Me.mProblem.List_Locations(x).List_Massnahmen(No).Schaltung(y, 0) = Akt.VER_ONOFF(z, 0)) Then
                        Akt.VER_ONOFF(z, 1) = Me.mProblem.List_Locations(x).List_Massnahmen(No).Schaltung(y, 1)
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

    ''' <summary>
    ''' Evaluiert ein Individuum. Durchl�uft alle Schritte vom Schreiben der Modellparameter bis zum Berechnen der Features.
    ''' </summary>
    ''' <param name="ind">das zu evaluierende Individuum</param>
    ''' <param name="storeInDB">Ob das Individuum in OptResult-DB gespeichert werden soll</param>
    ''' <returns>True wenn erfolgreich, False wenn fehlgeschlagen</returns>
    Public Overloads Function Evaluate(ByRef ind As EVO.Common.Individuum, Optional ByVal storeInDB As Boolean = True) As Boolean

        Dim isOK As Boolean

        isOK = False

        'Simulation vorbereiten
        '----------------------
        Select Case Me.mProblem.Method

            Case EVO.Common.METH_PES, EVO.Common.METH_SENSIPLOT, EVO.Common.METH_HOOKJEEVES

                'Bereitet das Sim f�r PES vor
                Call Me.PREPARE_Evaluation_PES(ind.OptParameter)

            Case EVO.Common.METH_CES, EVO.Common.METH_HYBRID

                'Bereitet das Sim f�r die Kombinatorik vor
                Call Me.PREPARE_Evaluation_CES(CType(ind, EVO.Common.Individuum_CES).Path, CType(ind, EVO.Common.Individuum_CES).Get_All_Loc_Elem)

                'HYBRID: Bereitet f�r die Optimierung mit den PES Parametern vor
                If (Me.mProblem.Method = EVO.Common.METH_HYBRID And Me.mSettings.CES.ty_Hybrid = Common.Constants.HYBRID_TYPE.Mixed_Integer) Then
                    Call Me.mProblem.Reduce_OptPara_and_ModPara(CType(ind, EVO.Common.Individuum_CES).Get_All_Loc_Elem)
                    Call Me.PREPARE_Evaluation_PES(ind.OptParameter)
                End If

            Case Else

                Throw New Exception("Funktion Sim.Evaluate() f�r Methode '" & Me.mProblem.Method & "' noch nicht implementiert!")

        End Select

        'Simulation ausf�hren
        '--------------------
        isOK = Me.launchSim()

        If (Not isOK) Then Return False

        'Simulationsergebnis einlesen und verarbeiten
        '--------------------------------------------
        Call Me.SIM_Ergebnis_auswerten(ind, storeInDB)

        Return isOK

    End Function

    ''' <summary>
    ''' Evaluiert ein Array von Individuen. Durchl�uft alle Schritte vom Schreiben der Modellparameter bis zum Berechnen der Features.
    ''' </summary>
    ''' <param name="inds">Ein Array von zu evaluierenden Individuen</param>
    ''' <param name="storeInDB">Ob das Individuum in OptResult-DB gespeichert werden soll</param>
    ''' <returns>True/False f�r jedes Individuum</returns>
    Public Overloads Function Evaluate(ByRef inds() As EVO.Common.Individuum, Optional ByVal storeInDB As Boolean = True) As Boolean()

        Dim i As Integer
        Dim isOK() As Boolean

        ReDim isOK(inds.GetUpperBound(0))

        For i = 0 To inds.GetUpperBound(0)
            isOK(i) = Me.Evaluate(inds(i), storeInDB)
        Next

        Return isOK

    End Function

    'Evaluierung des SimModells f�r ParameterOptimierung - Steuerungseinheit
    '***********************************************************************
    Public Sub PREPARE_Evaluation_PES(ByVal OptParams() As EVO.Common.OptParameter)

        Dim i As Integer

        'Aktuelle Parameterlisten neu dimensionieren (wegen HYBRID)
        ReDim Me.Akt.OptPara(Me.mProblem.NumParams - 1)
        ReDim Me.Akt.ModPara(Me.mProblem.List_ModellParameter.GetUpperBound(0))

        'Aktuelle Parameter speichern
        For i = 0 To Me.mProblem.NumParams - 1
            Me.Akt.OptPara(i) = OptParams(i).RWert
        Next

        'Parameter in Eingabedateien schreiben
        Call Write_ModellParameter()

    End Sub

    ''' <summary>
    ''' Evaluiert ein Individuum mit Hilfe des Simulationsmodells. Es werden alle im Problem definierten Feature- und Constraint-Werte berechnet und im Individuum gespeichert.
    ''' </summary>
    ''' <param name="ind">das zu evaluierende Individuum</param>
    ''' <param name="storeInDB">Ob das Individuum in OptResult-DB gespeichert werden soll</param>
    ''' <remarks>Die Simulation muss bereits erfolgt sein</remarks>
    Public Sub SIM_Ergebnis_auswerten(ByRef ind As Common.Individuum, Optional ByVal storeInDB As Boolean = True)

        Dim i As Short

        'Lesen der Relevanten Parameter aus der wel Datei
        Call SIM_Ergebnis_Lesen()

        'Qualit�tswerte berechnen
        For i = 0 To Me.mProblem.NumFeatures - 1
            ind.Features(i) = CalculateFeature(Me.mProblem.List_Featurefunctions(i))
        Next

        'Constraints berechnen
        For i = 0 To Me.mProblem.NumConstraints - 1
            ind.Constraints(i) = CalculateConstraint(Me.mProblem.List_Constraintfunctions(i))
        Next

        If (storeInDB) Then
            'L�sung im OptResult abspeichern (und zu DB hinzuf�gen)
            Call Me.OptResult.addSolution(ind)
        End If

    End Sub

    'ModellParameter aus OptParametern errechnen
    '*******************************************
    Private Sub OptParameter_to_ModellParameter()
        Dim i As Integer
        Dim j As Integer

        'VG ---------- Zusatzroutine f�r kalibrierung des Tagesgangs
        'VG Call VG_Kalibrierung_Tagesganglinie()
        'VG ---------- Ende

        For i = 0 To Me.mProblem.List_ModellParameter.GetUpperBound(0)
            For j = 0 To Me.mProblem.List_OptParameter.GetUpperBound(0)
                If (Me.mProblem.List_ModellParameter(i).OptParameter = Me.mProblem.List_OptParameter(j).Bezeichnung) Then
                    Me.Akt.ModPara(i) = Me.Akt.OptPara(j) * Me.mProblem.List_ModellParameter(i).Faktor
                    Exit For
                End If
            Next
        Next

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
        Call Me.OptParameter_to_ModellParameter()

        'Alle ModellParameter durchlaufen
        For i = 0 To Me.mProblem.List_ModellParameter.GetUpperBound(0)
            WriteCheck = True

            DateiPfad = Me.WorkDir_Current & Me.Datensatz & "." & Me.mProblem.List_ModellParameter(i).Datei
            'Datei �ffnen
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
                Throw New Exception("Fehler beim lesen der Datei '" & DateiPfad & "'. Sie k�nnte leer sein.")
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

            'Anzahl verf�gbarer Zeichen
            AnzZeichen = Me.mProblem.List_ModellParameter(i).SpBis - Me.mProblem.List_ModellParameter(i).SpVon + 1

            'Zeile einlesen und splitten
            Zeile = Zeilenarray(Me.mProblem.List_ModellParameter(i).ZeileNr - 1)
            StrLeft = Zeile.Substring(0, Me.mProblem.List_ModellParameter(i).SpVon - 1)
            If (Zeile.Length > Me.mProblem.List_ModellParameter(i).SpBis) Then
                StrRight = Zeile.Substring(Me.mProblem.List_ModellParameter(i).SpBis)
            Else
                StrRight = ""
            End If

            'Wert auf verf�gbare Stellen k�rzen
            '----------------------------------
            'bestimmen des ganzzahligen Anteils, \-Operator ginge zwar theoretisch, ist aber f�r Zahlen < 1 nicht robust (warum auch immer)
            WertStr = Convert.ToString(Me.Akt.ModPara(i) - Me.Akt.ModPara(i) Mod 1.0, Common.Provider.FortranProvider)

            If (WertStr.Length > AnzZeichen) Then
                'Wert zu lang
                Throw New Exception("Der Wert des Modellparameters '" & Me.mProblem.List_ModellParameter(i).Bezeichnung & "' (" & WertStr & ") ist l�nger als die zur Verf�gung stehende Anzahl von Zeichen!")

            ElseIf (WertStr.Length < AnzZeichen - 1) Then
                'Runden auf verf�gbare Stellen: Anzahl der Stellen - Anzahl der Vorkommastellen - Komma
                WertStr = Convert.ToString(Math.Round(Me.Akt.ModPara(i), AnzZeichen - WertStr.Length - 1), Common.Provider.FortranProvider)

            Else
                'Ganzzahligen Wert benutzen
            End If

            'Falls erforderlich, Wert mit Leerzeichen f�llen
            If (WertStr.Length < AnzZeichen) Then
                For j = 1 To AnzZeichen - WertStr.Length
                    WertStr = " " & WertStr
                Next
            End If

            'Zeile wieder zusammensetzen
            Zeile = StrLeft & WertStr & StrRight

            Zeilenarray(Me.mProblem.List_ModellParameter(i).ZeileNr - 1) = Zeile

            'Alle Zeilen wieder in Datei schreiben
            Dim StrWrite As StreamWriter = New StreamWriter(DateiPfad, False, System.Text.Encoding.GetEncoding("iso8859-1"))
            Dim StrWriteSync As TextWriter = TextWriter.Synchronized(StrWrite)

            For j = 0 To AnzZeil - 1
                StrWrite.WriteLine(Zeilenarray(j))
            Next

            StrWriteSync.Close()
            StrWrite.Close()
        Next

        If (Not WriteCheck) Then
            Throw New Exception("Es wurde kein Parameter geschrieben.")
        End If

    End Sub

    'SimModell ausf�hren (simulieren)
    '********************************
    Public MustOverride Overloads Function launchSim() As Boolean
    'mit Threads:
    Public MustOverride Overloads Function launchSim(ByVal Thread_ID As Integer, ByVal Child_ID As Integer) As Boolean
    Public MustOverride Function launchFree(ByRef Thread_ID As Integer) As Boolean
    Public MustOverride Function launchReady(ByRef Thread_ID As Integer, ByRef SimIsOK As Boolean, ByVal Child_ID As Integer) As Boolean


    'Simulationsergebnis einlesen
    '----------------------------
    Protected MustOverride Sub SIM_Ergebnis_Lesen()

#End Region 'Evaluierung

#Region "Qualit�tswertberechnung"

    'Ph�notypberechnung
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

        '�berpr�fen ob simulierte Zeitreihe evtl. anderen Startzeitpunkt 
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

            Case "n�ber"
                'Relative Anzahl der Zeitschritte mit �berschreitungen (in Prozent)
                '------------------------------------------------------------------
                Dim nUeber As Integer = 0
                For i = ZeitschritteBisStart To ZeitschritteBisStart + ZeitschritteEval
                    If (SimReihe.YWerte(i + j) > feature.RefReihe.YWerte(i)) Then
                        nUeber += 1
                    End If
                Next
                QWert = nUeber / ZeitschritteEval * 100

            Case "s�ber"
                'Summe der �berschreitungen
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
                'abge�nderte Nash-Sutcliffe Formel: 0 als Zielwert (1- weggelassen)
                QWert = zaehler / nenner

            Case Else
                Throw New Exception("Die Zielfunktion '" & feature.Funktion & "' wird nicht unterst�tzt!")

        End Select

        Return QWert

    End Function

    'Qualit�tswert berechnen: Feature Typ = Wert
    '*******************************************
    Protected Function CalculateFeature_Wert(ByVal feature As Common.Featurefunction, ByVal SimReihe As Wave.Zeitreihe) As Double

        Dim QWert As Double
        Dim i As Integer

        'Simulationsreihe auf Evaluierungszeitraum k�rzen
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

            Case "n�ber"
                'Relative Anzahl der Zeitschritte mit �berschreitungen (in Prozent)
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

            Case "s�ber"
                'Summe der �berschreitungen
                '--------------------------
                Dim sUeber As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) > feature.RefWert) Then
                        sUeber += SimReihe.YWerte(i) - feature.RefWert
                    End If
                Next
                QWert = sUeber

            Case Else
                Throw New Exception("Die Zielfunktion '" & feature.Funktion & "' wird f�r Werte nicht unterst�tzt!")

        End Select

        Return QWert

    End Function

#End Region 'Qualit�tswertberechnung

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

#Region "Multithreading"

    'Datens�tze f�r Multithreading kopieren
    '**************************************
    Public Sub createThreadWorkDirs(ByVal n_Threads As Integer)

        Dim i As Integer = 1
        Dim Source, Dest, relPaths() As String
        Dim binPath As String = System.Windows.Forms.Application.StartupPath()

        Source = Me.WorkDir_Original
        Dest = binPath & "\Thread_0\"

        'Alte Thread-Ordner l�schen
        Call Me.deleteThreadWorkDirs()

        'zu kopierende Dateien bestimmen
        relPaths = Me.getDatensatzFiles(Source)

        'Dateien in Ordner Thread_0 kopieren
        For Each relPath As String In relPaths
            My.Computer.FileSystem.CopyFile(Source & relPath, Dest & relPath, True)
        Next

        'F�r die weiteren Threads den Ordner Thread_0 kopieren
        Source = binPath & "\Thread_0\"

        For i = 1 To n_Threads - 1

            Dest = binPath & "\Thread_" & i.ToString() & "\"
            My.Computer.FileSystem.CopyDirectory(Source, Dest, True)

        Next

    End Sub

    ''' <summary>
    ''' Gibt die relativen Pfade aller Datensatz-Dateien zur�ck
    ''' </summary>
    ''' <param name="rootdirectory">Das zu durchsuchende Verzeichnis</param>
    ''' <returns></returns>
    Protected Function getDatensatzFiles(ByVal rootdirectory As String) As String()

        Dim Files() As IO.FileInfo
        Dim DirInfo, Dirs() As IO.DirectoryInfo
        Dim paths(), subpaths(), ext As String

        If (rootdirectory.LastIndexOf("\") <> rootdirectory.Length - 1) Then
            rootdirectory &= "\"
        End If

        ReDim paths(-1)

        DirInfo = New IO.DirectoryInfo(rootdirectory)

        'zu kopierende Dateien anhand der Dateiendung bestimmen
        Files = DirInfo.GetFiles("*.*")
        For Each File As IO.FileInfo In Files
            'Dateiendung bestimmen
            If (File.Extension.Length > 0) Then
                ext = File.Extension.Substring(1).ToUpper()
                'Pr�fen, ob es sich ume eine zu kopierende Datei handelt
                If (Me.DatensatzDateiendungen.Contains(ext)) Then
                    'Relativen Pfad der Datei zu Array hinzuf�gen
                    ReDim Preserve paths(paths.Length)
                    paths(paths.Length - 1) = File.Name
                End If
            End If
        Next

        'Unterverzeichnisse rekursiv durchsuchen
        Dirs = DirInfo.GetDirectories("*.*")
        For Each dir As IO.DirectoryInfo In Dirs
            '.svn-Verzeichnisse �berspringen
            If (dir.Name <> ".svn") Then
                'Pfade aus Unterverzeichnis holen
                subpaths = Me.getDatensatzFiles(dir.FullName)
                'Pfade zu Array hinzuf�gen
                For Each subpath As String In subpaths
                    subpath = dir.Name & "\" & subpath
                    ReDim Preserve paths(paths.Length)
                    paths(paths.Length - 1) = subpath
                Next
            End If
        Next

        Return paths

    End Function

    ''' <summary>
    ''' Datens�tze f�r Multithreading l�schen
    ''' </summary>
    ''' <remarks>l�scht die Ordner Thread_0 bis Thread_9 im bin-Verzeichnis</remarks>
    Public Sub deleteThreadWorkDirs()

        Dim i As Integer
        Dim dir As String

        For i = 0 To 9

            dir = System.Windows.Forms.Application.StartupPath() & "\Thread_" & i.ToString() & "\"

            If Directory.Exists(dir) Then
                Call EVO.Common.FileHelper.purgeReadOnly(dir)
                Directory.Delete(dir, True)
            End If
        Next

    End Sub

    ''' <summary>
    ''' Gibt den Datensatz Ordner eines Threads zur�ck
    ''' </summary>
    ''' <param name="Thread_ID"></param>
    Public Function getThreadWorkDir(ByVal Thread_ID As Integer) As String

        Dim dir As String

        dir = System.Windows.Forms.Application.StartupPath() & "\Thread_" & Thread_ID.ToString() & "\"

        Return dir

    End Function

#End Region  'Multithreading

#End Region 'Methoden

End Class