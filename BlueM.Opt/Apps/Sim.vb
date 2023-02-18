'BlueM.Opt
'Copyright (C) BlueM Dev Group
'Website: <https://www.bluemodel.org>
'
'This program is free software: you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.
'
'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
'GNU General Public License for more details.
'
'You should have received a copy of the GNU General Public License
'along with this program. If not, see <https://www.gnu.org/licenses/>.
'
Imports System.IO
Imports System.Windows.Forms
Imports System.Globalization
Imports System.Threading
Imports BlueM.Opt.Common.Constants
Imports BlueM

''' <summary>
''' Klasse Sim
''' Basisklasse für Simulationsmodelle wie BlueM und SMUSI
''' </summary>
Public MustInherit Class Sim

#Region "Eigenschaften"

    'Eigenschaften
    '#############

    'Generelle Eigenschaften
    '-----------------------
    ''' <summary>
    ''' Eine StringCollection mit allen Dateiendungen (ohne Punkt), die in einem Datensatz vorkommen können
    ''' </summary>
    ''' <remarks>Die erste Dateiendung in dieser Collection repräsentiert den Datensatz (wird z.B. als Filter für OpenFile-Dialoge verwendet)</remarks>
    Public MustOverride ReadOnly Property DatensatzDateiendungen() As Collections.Specialized.StringCollection

    ''' <summary>
    ''' Die einen Datensatz repräsentierende Dateiendung (mit Punkt)
    ''' </summary>
    Public ReadOnly Property DatensatzExtension() As String
        Get
            Return "." & Me.DatensatzDateiendungen(0)
        End Get
    End Property

    Public Datensatz As String                           'Name des zu simulierenden Datensatzes

    Public WorkDir_Current As String                     'aktuelles Arbeits-/Datensatzverzeichnis
    Public WorkDir_Original As String                    'Original-Arbeits-/Datensatzverzeichnis

    Public SimStart As DateTime                          'Anfangsdatum der Simulation
    Public SimEnde As DateTime                           'Enddatum der Simulation
    Public SimDT As TimeSpan                             'Zeitschrittweite der Simulation

    ''' <summary>
    ''' Das Simulationsergebnis
    ''' </summary>
    Public SimErgebnis As BlueM.Opt.Common.ObjectiveFunction.SimErgebnis_Structure

    'Das Problem
    '-----------
    Protected mProblem As BlueM.Opt.Common.Problem

    'Die Einstellungen
    Protected mSettings As BlueM.Opt.Common.Settings

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
    Private mStoreIndividuals As Boolean
    Public OptResult As BlueM.Opt.OptResult.OptResult             'Optimierungsergebnis
    Public OptResultRef As BlueM.Opt.OptResult.OptResult          'Vergleichsergebnis

    'Multithreading
    '--------------
    Public MustOverride ReadOnly Property MultithreadingSupported() As Boolean
    Private _isPaused As Boolean
    Private _isStopped As Boolean

#End Region 'Eigenschaften

#Region "Events"

    ''' <summary>
    ''' Wird ausgelöst, wenn ein Individuum,
    ''' das in einem Array an die Evaluate() Methode übergeben wurde,
    ''' erfolgreich evaluiert wurde
    ''' </summary>
    ''' <param name="ind">das evaluierte Individuum</param>
    ''' <param name="i_Nachf">0-basierte Nachfahren-Nummer</param>
    Public Event IndividuumEvaluated(ByRef ind As BlueM.Opt.Common.Individuum, ByVal i_Nachf As Integer)

#End Region

#Region "Properties"

    ''' <summary>
    ''' Gibt an, ob evaluierte Individuen abgespeichert werden sollen
    ''' </summary>
    ''' <remarks>standardmässig True</remarks>
    Public Property StoreIndividuals() As Boolean
        Get
            Return Me.mStoreIndividuals
        End Get
        Set(ByVal value As Boolean)
            Me.mStoreIndividuals = value
        End Set
    End Property

    ''' <summary>
    ''' Pause
    ''' </summary>
    Public Property isPause() As Boolean
        Get
            Return Me._isPaused
        End Get
        Set(ByVal value As Boolean)
            Me._isPaused = value
        End Set
    End Property

    ''' <summary>
    ''' Stop
    ''' </summary>
    Public Property isStopped() As Boolean
        Get
            Return Me._isStopped
        End Get
        Set(ByVal value As Boolean)
            Me._isStopped = value
        End Set
    End Property


#End Region 'Properties

#Region "Methoden"

#Region "Initialisierung"

    ''' <summary>
    ''' Konstruktor
    ''' </summary>
    Public Sub New()

        'Simulationsergebnis instanzieren
        Me.SimErgebnis.Clear()

        'Standardmässig OptResult verwenden
        Me.mStoreIndividuals = True

        Me.isPause = False
        Me.isStopped = False

    End Sub

    ''' <summary>
    ''' Die Sim-Anwendung für die Simulation vorbereiten
    ''' </summary>
    Public Overridable Sub prepareSimulation()

        'Datensätze für Multithreading kopieren
        Call Me.createThreadWorkDirs()

        'Standardmäßig in Ordner Thread_0 simulieren
        Me.WorkDir_Current = Me.getThreadWorkDir(0)

    End Sub

    ''' <summary>
    ''' Pfad zum Datensatz verarbeiten und speichern
    ''' </summary>
    ''' <param name="pfad">Der Pfad</param>
    Public Sub setDatensatz(ByVal pfad As String)

        If (File.Exists(pfad)) Then
            'Datensatzname bestimmen
            Me.Datensatz = Path.GetFileNameWithoutExtension(pfad)
            'Arbeitsverzeichnis bestimmen
            Me.WorkDir_Current = Path.GetDirectoryName(pfad)
            Me.WorkDir_Original = Path.GetDirectoryName(pfad)
        Else
            Throw New Exception($"Dataset '{pfad}' not found!")
        End If

        'Simulationsdaten einlesen
        Try
            Call Me.Read_SimParameter()
        Catch ex As Exception
            Throw New Exception("Unable to read simulation parameters!" & eol & ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Das Problem übergeben
    ''' </summary>
    ''' <param name="prob">Das Problem</param>
    Public Overridable Sub setProblem(ByRef prob As BlueM.Opt.Common.Problem)

        'Problem speichern
        Me.mProblem = prob

        'Original-WorkDir benutzen (bei Neustart wichtig)
        Me.WorkDir_Current = Me.WorkDir_Original

        'Aktuelle Parameterlisten dimensionieren
        ReDim Me.Akt.OptPara(Me.mProblem.NumOptParams - 1)
        ReDim Me.Akt.ModPara(Me.mProblem.List_ModellParameter.GetUpperBound(0))

        'Startwerte der OptParameter setzen
        For i = 0 To Me.mProblem.NumOptParams - 1
            Me.Akt.OptPara(i) = Me.mProblem.List_OptParameter(i).StartWert
        Next

    End Sub

    ''' <summary>
    ''' Einstellungen setzen
    ''' </summary>
    ''' <param name="settings">Die Einstellungen</param>
    Public Sub setSettings(ByRef settings As BlueM.Opt.Common.Settings)

        'Settings speichern
        Me.mSettings = settings

    End Sub

    ''' <summary>
    ''' Simulationsparameter einlesen
    ''' </summary>
    Protected MustOverride Sub Read_SimParameter()

    ''' <summary>
    ''' Prepares the OptResult instance by initializing the database
    ''' </summary>
    ''' <param name="starttime">start time to use for the database filename</param>
    Public Sub PrepareOptResult(starttime As DateTime)
        'Ergebnisspeicher initialisieren
        If (Me.StoreIndividuals) Then
            Me.OptResult = New BlueM.Opt.OptResult.OptResult(Me.Datensatz, Me.mProblem, True, starttime)
        End If
    End Sub

#End Region 'Initialisierung

#Region "Evaluierung"

    ''' <summary>
    ''' Evaluiert ein einzelnes Individuum. 
    ''' Durchläuft alle Schritte vom Schreiben der Modellparameter bis zum Berechnen der Objectives.
    ''' </summary>
    ''' <param name="ind">das zu evaluierende Individuum</param>
    ''' <param name="storeInDB">Ob das Individuum in OptResult-DB gespeichert werden soll</param>
    ''' <returns>True wenn erfolgreich, False wenn fehlgeschlagen</returns>
    Public Overloads Function Evaluate(ByRef ind As BlueM.Opt.Common.Individuum, Optional ByVal storeInDB As Boolean = True) As Boolean

        Dim isOK As Boolean

        isOK = False

        'Simulation vorbereiten
        '----------------------
        Call Me.PREPARE_Evaluation_PES(ind.OptParameter)

        'Simulation ausführen
        '--------------------
        isOK = Me.launchSim()

        If (Not isOK) Then Return False

        'Simulationsergebnis einlesen und verarbeiten
        '--------------------------------------------
        Try
            Call Me.SIM_Ergebnis_auswerten(ind, storeInDB)
        Catch e As Exception
            BlueM.Opt.Common.Log.AddMessage(BlueM.Opt.Common.Log.levels.error, "Failed to evaluate simulation: " & eol & e.Message)
            Return False
        End Try

        Return isOK

    End Function

    ''' <summary>
    ''' Evaluiert ein Array von Individuen 
    ''' Durchläuft alle Schritte vom Schreiben der Modellparameter bis zum Berechnen der Objectives.
    ''' Erfolgreich evaluierte Individuen werden mit dem Event IndividuumEvaluated zurückgegeben.
    ''' </summary>
    ''' <param name="inds">Ein Array von zu evaluierenden Individuen</param>
    ''' <param name="storeInDB">Ob das Individuum in OptResult-DB gespeichert werden soll</param>
    ''' <returns>True/False für jedes Individuum</returns>
    ''' <remarks>je nach Einstellung läuft die Evaluierung in multiplen Threads oder single-threaded ab</remarks>
    Public Overloads Function Evaluate(ByRef inds() As BlueM.Opt.Common.Individuum, Optional ByVal storeInDB As Boolean = True) As Boolean()

        Dim isOK() As Boolean
        Dim tmpind As BlueM.Opt.Common.Individuum
        Dim n_individuals As Integer
        Dim ThreadID_Free As Integer = 0
        Dim ThreadID_Ready As Integer = 0
        Dim n_ind_Run As Integer = 0
        Dim n_ind_Ready As Integer = 0
        Dim Ready As Boolean = False
        Dim SIM_Eval_is_OK As Boolean
        Dim OptTimePara As New Stopwatch

        'Anzahl Individuen
        n_individuals = inds.Length

        ReDim isOK(n_individuals - 1)

        If (Me.mSettings.General.UseMultithreading) Then

            'Mit Multithreading
            '==================
            System.Threading.Thread.CurrentThread.Priority = Threading.ThreadPriority.Normal
            OptTimePara.Start()

            Do
                'Stoppen?
                If (Me.isStopped) Then Return isOK

                If (Me.ThreadFree(ThreadID_Free) _
                    And (n_ind_Run < n_individuals) _
                    And (n_ind_Ready + Me.n_Threads > n_ind_Run) _
                    And Me.isPause = False) Then
                    'Falls eine Simulation frei, verarbeitet und nicht Pause
                    '-------------------------------------------------------

                    Me.WorkDir_Current = Me.getThreadWorkDir(ThreadID_Free)

                    'Simulation vorbereiten
                    '----------------------
                    Call Me.PREPARE_Evaluation_PES(inds(n_ind_Run).OptParameter)

                    'Simulation ausführen
                    '--------------------
                    SIM_Eval_is_OK = Me.launchSim(ThreadID_Free, n_ind_Run)

                    n_ind_Run += 1

                ElseIf (Me.ThreadReady(ThreadID_Ready, SIM_Eval_is_OK, n_ind_Ready) = True _
                        And SIM_Eval_is_OK) Then
                    'Falls Simulation fertig und erfolgreich
                    '---------------------------------------

                    Me.WorkDir_Current = Me.getThreadWorkDir(ThreadID_Ready)

                    'HACK: Individuum für Auswertung temporär kopieren um ArrayMismatchException zu umgehen
                    tmpind = inds(n_ind_Ready)

                    'Individuum auswerten
                    Try
                        Call Me.SIM_Ergebnis_auswerten(tmpind, storeInDB)

                        'Individuum per Event zurückgeben
                        RaiseEvent IndividuumEvaluated(tmpind, n_ind_Ready)

                        isOK(n_ind_Ready) = True

                    Catch e As Exception
                        BlueM.Opt.Common.Log.AddMessage(BlueM.Opt.Common.Log.levels.error, "Failed to evaluate simulation: " & eol & e.Message)
                        isOK(n_ind_Ready) = False
                        SIM_Eval_is_OK = False
                    End Try

                    'HACK: zurückkopieren (nötig?)
                    inds(n_ind_Ready) = tmpind

                    'Prüfen, ob alle Individuen fertig
                    If (n_ind_Ready = n_individuals - 1) Then
                        Ready = True
                    End If

                    n_ind_Ready += 1

                ElseIf (Me.ThreadReady(ThreadID_Ready, SIM_Eval_is_OK, n_ind_Ready) = False _
                        And SIM_Eval_is_OK = False) Then
                    'Falls Simulation fertig aber nicht erfolgreich
                    '----------------------------------------------

                    isOK(n_ind_Ready) = False

                    If (n_ind_Ready = n_individuals - 1) Then
                        Ready = True
                    End If

                    n_ind_Ready += 1

                ElseIf (n_ind_Ready = n_ind_Run _
                        And Me.isPause = True) Then
                    'Falls Pause und alle simulierten auch verarbeitet
                    '-------------------------------------------------

                    Do While (Me.isPause)
                        System.Threading.Thread.Sleep(20)
                        Application.DoEvents()
                    Loop

                Else
                    'Falls total im Stress
                    '---------------------
                    System.Threading.Thread.Sleep(400)
                    Application.DoEvents()

                End If

            Loop While (Ready = False)

        Else
            'Ohne Multithreading
            '===================
            For i = 0 To inds.Length - 1
                'Evaluieren
                isOK(i) = Me.Evaluate(inds(i), storeInDB)
                If (isOK(i)) Then
                    'erfolgreich evaluiertes Individuum per Event zurückgeben
                    RaiseEvent IndividuumEvaluated(inds(i), i)
                End If
            Next

        End If

        OptTimePara.Stop()
        'BlueM.Opt.Common.Log.AddMessage($"Die Evaluierung der Generation dauerte:   {OptTimePara.Elapsed.Hours}h  {OptTimePara.Elapsed.Minutes}m  {OptTimePara.Elapsed.Seconds}s {OptTimePara.Elapsed}ms")

        Return isOK

    End Function

    'Evaluierung des SimModells für ParameterOptimierung - Steuerungseinheit
    '***********************************************************************
    Private Sub PREPARE_Evaluation_PES(ByVal OptParams() As BlueM.Opt.Common.OptParameter)

        'Wenn Fehler: guckst du ob der Elementname richtig angegeben ist!!

        Dim i As Integer

        'Aktuelle Parameter speichern
        For i = 0 To Me.mProblem.NumOptParams - 1
            Me.Akt.OptPara(i) = OptParams(i).RWert
        Next

        'Parameter in Eingabedateien schreiben
        Call Write_ModellParameter()

    End Sub

    ''' <summary>
    ''' Evaluiert ein Individuum mit Hilfe des Simulationsmodells. Es werden alle im Problem definierten Objective- und Constraint-Werte berechnet und im Individuum gespeichert.
    ''' </summary>
    ''' <param name="ind">das zu evaluierende Individuum</param>
    ''' <param name="storeInDB">Ob das Individuum in OptResult-DB gespeichert werden soll</param>
    ''' <remarks>Die Simulation muss bereits erfolgt sein</remarks>
    Private Sub SIM_Ergebnis_auswerten(ByRef ind As Common.Individuum, Optional ByVal storeInDB As Boolean = True)

        Dim i, j, k As Short
        Dim aggroziel As BlueM.Opt.Common.ObjectiveFunction_Aggregate
        Dim aggregateIndices As New Collections.Generic.List(Of Integer)

        'Simulationsergebnis einlesen
        Call SIM_Ergebnis_Lesen()

        'ObjectiveFunctions berechnen
        For i = 0 To Me.mProblem.NumObjectives - 1

            With Me.mProblem.List_ObjectiveFunctions(i)

                If (.isGroupLeader) Then
                    'Aggregierte Ziele für später aufheben
                    aggregateIndices.Add(i)
                Else
                    'andere Ziele auswerten
                    ind.Objectives(i) = .calculateObjective(Me.SimErgebnis)
                End If

            End With
        Next

        'Aggregierte Ziele berechnen
        For Each j In aggregateIndices

            aggroziel = Me.mProblem.List_ObjectiveFunctions(j)

            'Zunächst zu Null setzen
            ind.Objectives(j) = 0

            'Alle Gruppenmitglieder suchen
            For k = 0 To Me.mProblem.NumObjectives - 1

                With Me.mProblem.List_ObjectiveFunctions(k)

                    If (Not .isGroupLeader _
                        And .Gruppe = aggroziel.Gruppe) Then

                        'gefundenes Gruppenmitglied hinzuaddieren
                        ind.Objectives(j) += ind.Objectives(k) * .OpFact
                    End If

                End With
            Next

            'Zielrichtung berücksichtigen
            ind.Objectives(j) *= aggroziel.Richtung
        Next

        'Constraints berechnen
        For i = 0 To Me.mProblem.NumConstraints - 1
            ind.Constraints(i) = CalculateConstraint(Me.mProblem.List_Constraintfunctions(i))
        Next

        'Lösung im OptResult abspeichern (und zu DB hinzufügen)
        If (Me.StoreIndividuals And storeInDB) Then
            Call Me.OptResult.addSolution(ind)
        End If

    End Sub

    'ModellParameter aus OptParametern errechnen
    '*******************************************
    Private Sub OptParameter_to_ModellParameter()
        Dim i As Integer
        Dim j As Integer

        'VG ---------- Zusatzroutine für kalibrierung des Tagesgangs
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
        Dim AnzZeichen, AnzNachkomma As Short
        Dim AnzZeil As Integer
        Dim i As Integer
        Dim Zeilen As Collections.Generic.Dictionary(Of Integer, String)
        Dim Zeile As String
        Dim StrLeft As String
        Dim StrRight As String
        Dim DateiPfad As String
        Dim WriteCheck As Boolean = False
        Dim FiStr As FileStream
        Dim StrRead As StreamReader
        Dim StrReadSync As TextReader
        Dim StrWrite As StreamWriter
        Dim StrWriteSync As TextWriter

        'ModellParameter aus OptParametern kalkulieren()
        Call Me.OptParameter_to_ModellParameter()

        'Alle ModellParameter durchlaufen
        For i = 0 To Me.mProblem.List_ModellParameter.GetUpperBound(0)
            WriteCheck = True

            DateiPfad = IO.Path.Combine(Me.WorkDir_Current, Me.Datensatz & "." & Me.mProblem.List_ModellParameter(i).Datei)
            'Datei öffnen
            FiStr = New FileStream(DateiPfad, FileMode.Open, IO.FileAccess.Read)
            StrRead = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
            StrReadSync = TextReader.Synchronized(StrRead)

            Zeilen = New Collections.Generic.Dictionary(Of Integer, String)

            'Datei komplett einlesen
            AnzZeil = 0
            Do
                Zeile = StrRead.ReadLine.ToString
                AnzZeil += 1
                Zeilen.Add(AnzZeil, Zeile)
            Loop Until StrRead.Peek() = -1

            StrReadSync.Close()
            StrRead.Close()
            FiStr.Close()

            'Anzahl verfügbarer Zeichen
            AnzZeichen = Me.mProblem.List_ModellParameter(i).SpBis - Me.mProblem.List_ModellParameter(i).SpVon + 1

            'Zeile einlesen und splitten
            Zeile = Zeilen(Me.mProblem.List_ModellParameter(i).ZeileNr)
            If (Zeile.Length < Me.mProblem.List_ModellParameter(i).SpVon) Then
                Throw New Exception($"Unable to write model parameter '{Me.mProblem.List_ModellParameter(i).Bezeichnung}', there is not enough space at the given line and column!")
            End If
            StrLeft = Zeile.Substring(0, Me.mProblem.List_ModellParameter(i).SpVon - 1)
            If (Zeile.Length > Me.mProblem.List_ModellParameter(i).SpBis) Then
                StrRight = Zeile.Substring(Me.mProblem.List_ModellParameter(i).SpBis)
            Else
                StrRight = ""
            End If

            'Wert auf verfügbare Stellen kürzen
            '----------------------------------
            'Auf ganze Zahl runden und zu String konvertieren
            WertStr = Convert.ToString(Convert.ToInt32(Me.Akt.ModPara(i), Common.Provider.FortranProvider))

            If (WertStr.Length > AnzZeichen) Then
                'Wert zu lang
                Throw New Exception($"The whole number part of the model parameter '{Me.mProblem.List_ModellParameter(i).Bezeichnung}' ({WertStr}) is longer than the number of available spaces!")
            End If
            'Anzahl verfügbarer Nachkommastellen = (Anzahl Zeichen) - (Anzahl Vorkommastellen) - (Punkt)
            'Math.Round() akzeptiert max 15 Nachkommastellen
            AnzNachkomma = Math.Min(AnzZeichen - WertStr.Length - 1, 15)
            'Bei negativen Werten noch ein Zeichen für das Minuszeichen abziehen
            If Me.Akt.ModPara(i) < 0 Then
                AnzNachkomma -= 1
            End If
            If AnzNachkomma > 0 Then
                'Runden auf verfügbare Stellen: 
                WertStr = Convert.ToString(Math.Round(Me.Akt.ModPara(i), AnzNachkomma), Common.Provider.FortranProvider)
            Else
                'Ganzzahligen Wert benutzen
            End If

            'Falls erforderlich, Wert mit Leerzeichen füllen
            WertStr = WertStr.PadLeft(AnzZeichen)

            'Zeile wieder zusammensetzen
            Zeile = StrLeft & WertStr & StrRight

            Zeilen(Me.mProblem.List_ModellParameter(i).ZeileNr) = Zeile

            'Alle Zeilen wieder in Datei schreiben
            StrWrite = New StreamWriter(DateiPfad, False, System.Text.Encoding.GetEncoding("iso8859-1"))
            StrWriteSync = TextWriter.Synchronized(StrWrite)

            For Each Zeile In Zeilen.Values
                StrWrite.WriteLine(Zeile)
            Next

            StrWriteSync.Close()
            StrWrite.Close()
        Next

        If (Not WriteCheck) Then
            Throw New Exception("Es wurde kein Parameter geschrieben.")
        End If

    End Sub

    'SimModell ausführen (simulieren)
    '********************************
    Protected MustOverride Overloads Function launchSim() As Boolean
    'mit Threads:
    Protected MustOverride Overloads Function launchSim(ByVal Thread_ID As Integer, ByVal Child_ID As Integer) As Boolean
    Protected MustOverride Function ThreadFree(ByRef Thread_ID As Integer) As Boolean
    Protected MustOverride Function ThreadReady(ByRef Thread_ID As Integer, ByRef SimIsOK As Boolean, ByVal Child_ID As Integer) As Boolean

    'Simulationsergebnis einlesen
    '----------------------------
    Protected MustOverride Sub SIM_Ergebnis_Lesen()

#End Region 'Evaluierung

#Region "Constraintberechnung"

    'Constraint berechnen (Constraint < 0 ist Grenzverletzung)
    '*********************************************************
    Private Function CalculateConstraint(ByVal constr As Common.Constraintfunction) As Double

        Dim i As Integer

        'Simulationsergebnis auslesen
        Dim SimReihe As Wave.TimeSeries
        SimReihe = Me.SimErgebnis.Reihen(constr.SimGr)

        'Fallunterscheidung GrenzTyp (Wert/Reihe)
        Select Case constr.Typ.ToUpper()

            Case "WERT"
                'Calculate SimValue from SimSeries
                Dim SimWert As Double
                Select Case constr.WertFunktion.ToUpper()
                    Case "MAX", "MAXWERT"
                        SimWert = SimReihe.Maximum
                    Case "MIN", "MINWERT"
                        SimWert = SimReihe.Minimum
                    Case "AVG", "AVERAGE"
                        SimWert = SimReihe.Average
                    Case "START", "ANFWERT"
                        SimWert = SimReihe.Values.First
                    Case "END", "ENDWERT"
                        SimWert = SimReihe.Values.Last
                    Case "SUM", "SUMME"
                        SimWert = SimReihe.Sum
                    Case Else
                        Throw New Exception($"Unknown value type '{constr.WertFunktion}' for constraint {constr.Bezeichnung}!")
                End Select

                'Grenzverletzung berechnen
                Select Case constr.GrenzPos.ToUpper()
                    Case "UPPER", "OBERGRENZE"
                        CalculateConstraint = constr.GrenzWert - SimWert
                    Case "LOWER", "UNTERGRENZE"
                        CalculateConstraint = SimWert - constr.GrenzWert
                    Case Else
                        Throw New Exception($"Unknown bound '{constr.GrenzPos}' for constraint {constr.Bezeichnung}!")
                End Select

            Case "REIHE"
                'Es wird die Summe der Grenzwertverletzungen verwendet

                Dim thresholdSeries As BlueM.Wave.TimeSeries
                thresholdSeries = constr.GrenzReihe.Clone()

                'remove NaN values
                If constr.GrenzReihe.NaNCount > 0 Then
                    thresholdSeries = thresholdSeries.removeNaNValues()
                    BlueM.Wave.TimeSeries.Synchronize(SimReihe, thresholdSeries)
                End If

                Dim summe As Double = 0

                For i = 0 To SimReihe.Length - 1

                    Select Case constr.GrenzPos.ToUpper()
                        Case "UPPER", "OBERGRENZE"
                            summe += Math.Min(thresholdSeries.Values(i) - SimReihe.Values(i), 0)

                        Case "LOWER", "UNTERGRENZE"
                            summe += Math.Min(SimReihe.Values(i) - thresholdSeries.Values(i), 0)
                        Case Else
                            Throw New Exception($"Unknown bound '{constr.GrenzPos}' for constraint {constr.Bezeichnung}!")
                    End Select
                Next

                CalculateConstraint = summe

        End Select

        Return CalculateConstraint

    End Function

#End Region 'Constraintberechnung

#Region "Multithreading"

    ''' <summary>
    ''' Number of threads to use for simulation
    ''' </summary>
    ''' <remarks>Returns 1 if multithreading is switched off</remarks>
    Protected ReadOnly Property n_Threads() As Integer
        Get

            If Not Me.mSettings.General.UseMultithreading Then
                'Wenn Multithreading ausgeschaltet ist, nur 1 Thread benutzen
                Return 1
            Else
                Return Me.mSettings.General.NThreads
            End If

        End Get
    End Property

    ''' <summary>
    ''' Kopiert alle Datensatz-Dateien vom Original WorkDir in das angegebene Zielverzeichnis
    ''' </summary>
    ''' <param name="destination">Zielverzeichnis</param>
    Public Sub copyDateset(destination As String)

        Dim relPaths() As String

        'zu kopierende Dateien bestimmen
        relPaths = Me.getDatensatzFiles(Me.WorkDir_Original)

        'Dateien kopieren
        For Each relPath As String In relPaths
            My.Computer.FileSystem.CopyFile(IO.Path.Combine(Me.WorkDir_Original, relPath), IO.Path.Combine(destination, relPath), True)
        Next

    End Sub

    ''' <summary>
    ''' Datensätze für Multithreading kopieren
    ''' </summary>
    ''' <returns>True wenn fertig</returns>
    ''' <remarks>Erstellt im bin-Ordner Verzeichnisse Thread_0 bis Thread_n mit Kopien des Original-Datensatzes</remarks>
    Private Function createThreadWorkDirs() As Boolean

        Dim i As Integer
        Dim isOK As Boolean
        Dim threadDir As String
        Dim binPath As String = System.Windows.Forms.Application.StartupPath()

        'Alte Thread-Ordner löschen
        isOK = Me.deleteThreadWorkDirs()

        'Datensatz in alle Thread-Ordner kopieren
        For i = 0 To Me.n_Threads - 1
            threadDir = IO.Path.Combine(binPath, "Thread_" & i)
            Call Me.copyDateset(threadDir)
        Next

        Return True

    End Function

    ''' <summary>
    ''' Gibt die relativen Pfade aller Datensatz-Dateien zurück
    ''' </summary>
    ''' <param name="rootdirectory">Das zu durchsuchende Verzeichnis</param>
    ''' <returns></returns>
    Private Function getDatensatzFiles(ByVal rootdirectory As String) As String()

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
                'Prüfen, ob es sich ume eine zu kopierende Datei handelt
                If (Me.DatensatzDateiendungen.Contains(ext)) Then
                    'Relativen Pfad der Datei zu Array hinzufügen
                    ReDim Preserve paths(paths.Length)
                    paths(paths.Length - 1) = File.Name
                End If
            End If
        Next

        'Unterverzeichnisse rekursiv durchsuchen
        Dirs = DirInfo.GetDirectories("*.*")
        For Each dir As IO.DirectoryInfo In Dirs
            'versteckte und solution- sowie sensiplot-Verzeichnisse überspringen
            If Not (dir.Name.StartsWith(".") Or dir.Name.StartsWith("solution_") Or dir.Name.StartsWith("sensiplot_")) Then
                'Pfade aus Unterverzeichnis holen
                subpaths = Me.getDatensatzFiles(dir.FullName)
                'Pfade zu Array hinzufügen
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
    ''' Datensätze für Multithreading löschen
    ''' </summary>
    ''' <returns>True wenn fertig</returns>
    ''' <remarks>löscht die Ordner Thread_0 bis Thread_9 im bin-Verzeichnis</remarks>
    Private Function deleteThreadWorkDirs() As Boolean

        Dim i As Integer
        Dim dir As String

        For i = 0 To 9

            dir = IO.Path.Combine(System.Windows.Forms.Application.StartupPath(), "Thread_" & i.ToString())

            If Directory.Exists(dir) Then
                Call BlueM.Opt.Common.FileHelper.purgeReadOnly(dir)
                Directory.Delete(dir, True)
            End If
        Next

        Return True

    End Function

    ''' <summary>
    ''' Gibt den Datensatz Ordner eines Threads zurück
    ''' </summary>
    ''' <param name="Thread_ID">Die ID des Threads</param>
    Protected Function getThreadWorkDir(ByVal Thread_ID As Integer) As String

        Dim dir As String

        dir = IO.Path.Combine(System.Windows.Forms.Application.StartupPath(), "Thread_" & Thread_ID.ToString())

        Return dir

    End Function

#End Region  'Multithreading

#Region "Misc"

    ''' <summary>
    ''' Setzt den Datensatz zurück, 
    ''' indem die Startwerte der OptParameter 
    ''' in die Eingabedateien des Original-Datensatzordners
    ''' geschrieben werden
    ''' </summary>
    Public Sub resetDatensatz()

        Dim tmp As String
        Dim i As Integer

        'Aktuelles Arbeitsverzeichnis merken
        tmp = Me.WorkDir_Current

        'Arbeitsverzeichnis auf Original-Verzeichnis setzen
        Me.WorkDir_Current = Me.WorkDir_Original

        'Startwerte der OptParameter setzen
        For i = 0 To Me.mProblem.NumOptParams - 1
            Me.Akt.OptPara(i) = Me.mProblem.List_OptParameter(i).StartWert
        Next

        'Modellparameter berechnen
        Call Me.OptParameter_to_ModellParameter()

        'Modellparameter schreiben
        Call Me.Write_ModellParameter()

        'Arbeitsverzeichnis zurücksetzen
        Me.WorkDir_Current = tmp

    End Sub

#End Region 'Misc

#End Region 'Methoden
End Class
