Imports System.IO
Imports System.Windows.Forms
Imports System.Globalization
Imports System.Threading
Imports IHWB.EVO.Common.Constants

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
    Private mStoreIndividuals As Boolean
    Public OptResult As EVO.OptResult.OptResult             'Optimierungsergebnis
    Public OptResultRef As EVO.OptResult.OptResult          'Vergleichsergebnis

    Public VerzweigungsDatei(,) As String                   'Gibt die PathSize an für jede Pfadstelle

    'Multithreading
    '--------------
    Public MustOverride ReadOnly Property MultithreadingSupported() As Boolean
    Protected n_Threads As Integer   'Anzahl Threads
    Private _isPause As Boolean

#End Region 'Eigenschaften

#Region "Events"

    ''' <summary>
    ''' Wird ausgelöst, wenn ein Individuum mit Multithreading fertig evaluiert wurde
    ''' </summary>
    ''' <param name="ind">das evaluierte Individuum</param>
    ''' <param name="i_Nachf">0-basierte Nachfahren-Nummer</param>
    ''' <remarks></remarks>
    Public Event IndividuumEvaluated(ByRef ind As EVO.Common.Individuum, ByVal i_Nachf As Integer)

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
            Return Me._isPause
        End Get
        Set(ByVal value As Boolean)
            Me._isPause = value
        End Set
    End Property

#End Region 'Properties

#Region "Methoden"

    'Methoden
    '########

#Region "Initialisierung"

    ''' <summary>
    ''' Konstruktor
    ''' </summary>
    Public Sub New()

        'Simulationsergebnis instanzieren
        Me.SimErgebnis = New Collection()

        'Standardmässig OptResult verwenden
        Me.mStoreIndividuals = True

    End Sub

    ''' <summary>
    ''' Die Anwendung auf Multithreading vorbereiten
    ''' </summary>
    ''' <param name="input_n_Threads">Anzahl Threads</param>
    Public Overridable Sub prepareThreads(ByVal input_n_Threads As Integer)
        'in erbenden Klassen implementieren
    End Sub

    ''' <summary>
    ''' Pfad zum Datensatz verarbeiten und speichern
    ''' </summary>
    ''' <param name="pfad">Der Pfad</param>
    Public Sub setDatensatz(ByVal pfad As String)

        Dim isOK As Boolean

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

        'Datensätze für Multithreading kopieren
        isOK = Me.createThreadWorkDirs()

    End Sub

    ''' <summary>
    ''' Das Problem übergeben
    ''' </summary>
    ''' <param name="prob">Das Problem</param>
    Public Overridable Sub setProblem(ByRef prob As EVO.Common.Problem)

        'Problem speichern
        Me.mProblem = prob

        'Original-WorkDir benutzen (bei Neustart wichtig)
        Me.WorkDir_Current = Me.WorkDir_Original

        'Je nach Problem weitere Vorbereitungen treffen
        Select Case Me.mProblem.Method

            Case Common.METH_PES, Common.METH_SENSIPLOT
                'nix

            Case Common.METH_CES, Common.METH_HYBRID

                'Verzweigungs Datei einlesen
                Call Me.Read_Verzweigungen()
                'Überprüfen der Kombinatorik
                Call Me.mProblem.Validate_Combinatoric()
                If (TypeOf Me Is BlueM) Then
                    'Prüfen ob Kombinatorik und BlueM-Verzweigungsdatei zusammenpassen
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
        If (Me.StoreIndividuals) Then
            Me.OptResult = New EVO.OptResult.OptResult(Me.Datensatz, Me.mProblem)
        End If

    End Sub

    ''' <summary>
    ''' Einstellungen setzen
    ''' </summary>
    ''' <param name="settings">Die Einstellungen</param>
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

#Region "Prüfung der Eingabedateien"

#End Region 'Prüfung der Eingabedateien

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

        '1. Die Maßnahme wird ermittelt
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

    'Bereitet das SimModell für Kombinatorik Optimierung vor
    '*******************************************************
    Public Sub PREPARE_Evaluation_CES(ByRef ind As EVO.Common.Individuum_CES)

        'Setzt den Aktuellen Pfad
        Akt.Path = ind.Path

        'Aktuelle Parameterlisten neu dimensionieren
        ReDim Me.Akt.OptPara(Me.mProblem.NumParams - 1)
        ReDim Me.Akt.ModPara(Me.mProblem.List_ModellParameter.GetUpperBound(0))

        'Die elemente werden an die Kostenkalkulation übergeben
        CType(Me, BlueM).SKos1.Akt_Elemente = ind.Get_All_Loc_Elem

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
    ''' Evaluiert ein einzelnes Individuum. 
    ''' Durchläuft alle Schritte vom Schreiben der Modellparameter bis zum Berechnen der Objectives.
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

            Case METH_PES, METH_SENSIPLOT, METH_HOOKJEEVES, METH_DDS

                'Bereitet das Sim für Parameteroptimierung vor
                Call Me.PREPARE_Evaluation_PES(ind.OptParameter)

            Case METH_CES, METH_HYBRID

                'Bereitet das Sim für die Kombinatorik vor
                Call Me.PREPARE_Evaluation_CES(ind)

                'HYBRID: Bereitet für die Optimierung mit den PES Parametern vor
                If (Me.mProblem.Method = METH_HYBRID _
                    And Me.mSettings.CES.ty_Hybrid = HYBRID_TYPE.Mixed_Integer) Then
                    If (Me.mProblem.Reduce_OptPara_and_ModPara(CType(ind, EVO.Common.Individuum_CES).Get_All_Loc_Elem)) Then
                        Call Me.PREPARE_Evaluation_PES(ind.OptParameter)
                    End If
                End If

            Case Else

                Throw New Exception("Funktion Sim.Evaluate() für Methode '" & Me.mProblem.Method & "' noch nicht implementiert!")

        End Select

        'Simulation ausführen
        '--------------------
        isOK = Me.launchSim()

        If (Not isOK) Then Return False

        'Simulationsergebnis einlesen und verarbeiten
        '--------------------------------------------
        Call Me.SIM_Ergebnis_auswerten(ind, storeInDB)

        Return isOK

    End Function

    ''' <summary>
    ''' Evaluiert ein Array von Individuen in multiplen Threads. 
    ''' Durchläuft alle Schritte vom Schreiben der Modellparameter bis zum Berechnen der Objectives.
    ''' Erfolgreich evaluierte Individuen werden mit dem Event IndividuumEvaluated zurückgegeben.
    ''' </summary>
    ''' <param name="inds">Ein Array von zu evaluierenden Individuen</param>
    ''' <param name="storeInDB">Ob das Individuum in OptResult-DB gespeichert werden soll</param>
    ''' <returns>True/False für jedes Individuum</returns>
    Public Overloads Function Evaluate(ByRef inds() As EVO.Common.Individuum, Optional ByVal storeInDB As Boolean = True) As Boolean()

        Dim isOK() As Boolean
        Dim tmpind As EVO.Common.Individuum
        Dim n_individuals As Integer
        Dim ThreadID_Free As Integer = 0
        Dim ThreadID_Ready As Integer = 0
        Dim n_ind_Run As Integer = 0
        Dim n_ind_Ready As Integer = 0
        Dim Ready As Boolean = False
        Dim SIM_Eval_is_OK As Boolean

        'Anzahl Individuen
        n_individuals = inds.Length

        ReDim isOK(n_individuals - 1)

        System.Threading.Thread.CurrentThread.Priority = Threading.ThreadPriority.Normal

        Do
            If (Me.launchFree(ThreadID_Free) _
                And (n_ind_Run < n_individuals) _
                And (n_ind_Ready + Me.n_Threads > n_ind_Run) _
                And Me.isPause = False) Then
                'Falls eine Simulation frei, verarbeitet und nicht Pause
                '-------------------------------------------------------

                Me.WorkDir_Current = Me.getThreadWorkDir(ThreadID_Free)

                'Simulation vorbereiten
                '----------------------
                Select Case Me.mProblem.Method

                    Case METH_PES, METH_SENSIPLOT, METH_HOOKJEEVES, METH_DDS

                        'Bereitet das Sim für Parameteroptimierung vor
                        Call Me.PREPARE_Evaluation_PES(inds(n_ind_Run).OptParameter)

                    Case METH_CES, METH_HYBRID

                        'Bereitet das Sim für die Kombinatorik vor
                        Call Me.PREPARE_Evaluation_CES(inds(n_ind_Run))

                        'HYBRID: Bereitet für die Optimierung mit den PES Parametern vor
                        'TODO: Christoph: Dies ist die einzige Stelle im Sim, an der die EVO_Settings benötigt werden. Kann man das nicht umgehen?
                        If (Me.mProblem.Method = METH_HYBRID _
                            And Me.mSettings.CES.ty_Hybrid = HYBRID_TYPE.Mixed_Integer) Then
                            If (Me.mProblem.Reduce_OptPara_and_ModPara(CType(inds(n_ind_Run), EVO.Common.Individuum_CES).Get_All_Loc_Elem)) Then
                                Call Me.PREPARE_Evaluation_PES(inds(n_ind_Run).OptParameter)
                            End If
                        End If

                    Case Else

                        Throw New Exception("Funktion Sim.Evaluate() für Methode '" & Me.mProblem.Method & "' noch nicht implementiert!")

                End Select

                'Simulation ausführen
                '--------------------
                SIM_Eval_is_OK = Me.launchSim(ThreadID_Free, n_ind_Run)

                n_ind_Run += 1

            ElseIf (Me.launchReady(ThreadID_Ready, SIM_Eval_is_OK, n_ind_Ready) = True _
                    And SIM_Eval_is_OK) Then
                'Falls Simulation fertig und erfolgreich
                '---------------------------------------

                Me.WorkDir_Current = Me.getThreadWorkDir(ThreadID_Ready)

                'HACK: Individuum für Auswertung temporär kopieren um ArrayMismatchException zu umgehen
                tmpind = inds(n_ind_Ready)

                'Individuum auswerten
                Me.SIM_Ergebnis_auswerten(tmpind, storeInDB)

                'Individuum per Event zurückgeben
                RaiseEvent IndividuumEvaluated(tmpind, n_ind_Ready)

                'HACK: zurückkopieren (nötig?)
                inds(n_ind_Ready) = tmpind

                isOK(n_ind_Ready) = True

                'Prüfen, ob alle Individuen fertig
                If (n_ind_Ready = n_individuals - 1) Then
                    Ready = True
                End If

                n_ind_Ready += 1

            ElseIf (Me.launchReady(ThreadID_Ready, SIM_Eval_is_OK, n_ind_Ready) = False _
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

        Return isOK

    End Function

    'Evaluierung des SimModells für ParameterOptimierung - Steuerungseinheit
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
    ''' Evaluiert ein Individuum mit Hilfe des Simulationsmodells. Es werden alle im Problem definierten Objective- und Constraint-Werte berechnet und im Individuum gespeichert.
    ''' </summary>
    ''' <param name="ind">das zu evaluierende Individuum</param>
    ''' <param name="storeInDB">Ob das Individuum in OptResult-DB gespeichert werden soll</param>
    ''' <remarks>Die Simulation muss bereits erfolgt sein</remarks>
    Public Sub SIM_Ergebnis_auswerten(ByRef ind As Common.Individuum, Optional ByVal storeInDB As Boolean = True)

        Dim i As Short

        'Lesen der Relevanten Parameter aus der wel Datei
        Call SIM_Ergebnis_Lesen()

        'Qualitätswerte berechnen
        For i = 0 To Me.mProblem.NumObjectives - 1
            ind.Objectives(i) = CalculateObjective(Me.mProblem.List_ObjectiveFunctions(i))
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
                Throw New Exception("Fehler beim lesen der Datei '" & DateiPfad & "'. Sie könnte leer sein.")
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
            AnzZeichen = Me.mProblem.List_ModellParameter(i).SpBis - Me.mProblem.List_ModellParameter(i).SpVon + 1

            'Zeile einlesen und splitten
            Zeile = Zeilenarray(Me.mProblem.List_ModellParameter(i).ZeileNr - 1)
            StrLeft = Zeile.Substring(0, Me.mProblem.List_ModellParameter(i).SpVon - 1)
            If (Zeile.Length > Me.mProblem.List_ModellParameter(i).SpBis) Then
                StrRight = Zeile.Substring(Me.mProblem.List_ModellParameter(i).SpBis)
            Else
                StrRight = ""
            End If

            'Wert auf verfügbare Stellen kürzen
            '----------------------------------
            'bestimmen des ganzzahligen Anteils, \-Operator ginge zwar theoretisch, ist aber für Zahlen < 1 nicht robust (warum auch immer)
            WertStr = Convert.ToString(Me.Akt.ModPara(i) - Me.Akt.ModPara(i) Mod 1.0, Common.Provider.FortranProvider)

            If (WertStr.Length > AnzZeichen) Then
                'Wert zu lang
                Throw New Exception("Der Wert des Modellparameters '" & Me.mProblem.List_ModellParameter(i).Bezeichnung & "' (" & WertStr & ") ist länger als die zur Verfügung stehende Anzahl von Zeichen!")

            ElseIf (WertStr.Length < AnzZeichen - 1) Then
                'Runden auf verfügbare Stellen: Anzahl der Stellen - Anzahl der Vorkommastellen - Komma
                WertStr = Convert.ToString(Math.Round(Me.Akt.ModPara(i), AnzZeichen - WertStr.Length - 1), Common.Provider.FortranProvider)
                'TODO: wozu der Punkt im Folgenden?
                'If (Not WertStr.Contains(".")) Then
                '    WertStr += "."
                'End If


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

    'SimModell ausführen (simulieren)
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

#Region "Qualitätswertberechnung"

    'Phänotypberechnung
    '##################

    'Berechnung der Objective Funktionen
    '***********************************
    Public MustOverride Function CalculateObjective(ByVal objective As Common.Objectivefunktion) As Double

    'Objectivewert berechnen: Feature Typ = Reihe
    '******************************************
    'BUG 218: Konstante und gleiche Zeitschrittweiten vorausgesetzt!
    Protected Function CalculateObjective_Reihe(ByVal objective As Common.Objectivefunktion, ByVal SimReihe As Wave.Zeitreihe) As Double

        Dim QWert As Double
        Dim i As Integer

        'Simulationszeitreihe auf Evaluierungszeitraum zuschneiden
        Call SimReihe.Cut(objective.EvalStart, objective.EvalEnde)

        'BUG 218: Kontrolle
        If (objective.RefReihe.Length <> SimReihe.Length) Then
            Throw New Exception("Ziel '" & objective.Bezeichnung & "': Simulations- und Referenzzeitreihe sind nicht kompatibel! (Länge/Zeitschritt?) Siehe Bug 218")
        End If

        'Fallunterscheidung Zielfunktion
        '-------------------------------
        Select Case objective.Funktion

            Case "AbQuad"
                'Summe der Fehlerquadrate
                '------------------------
                QWert = 0
                For i = 0 To SimReihe.Length - 1
                    QWert += (objective.RefReihe.YWerte(i) - SimReihe.YWerte(i)) * (objective.RefReihe.YWerte(i) - SimReihe.YWerte(i))
                Next

            Case "Diff"
                'Summe der Fehler
                '----------------
                QWert = 0
                For i = 0 To SimReihe.Length - 1
                    QWert += Math.Abs(objective.RefReihe.YWerte(i) - SimReihe.YWerte(i))
                Next

            Case "Volf"
                'Volumenfehler
                '-------------
                Dim VolSim As Double = 0
                Dim VolZiel As Double = 0
                For i = 0 To SimReihe.Length - 1
                    VolSim += SimReihe.YWerte(i)
                    VolZiel += objective.RefReihe.YWerte(i)
                Next
                'Differenz bilden und auf ZielVolumen beziehen
                QWert = Math.Abs(VolZiel - VolSim) / VolZiel * 100

            Case "nUnter"
                'Relative Anzahl der Zeitschritte mit Unterschreitungen (in Prozent)
                '-------------------------------------------------------------------
                Dim nUnter As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) < objective.RefReihe.YWerte(i)) Then
                        nUnter += 1
                    End If
                Next
                QWert = nUnter / SimReihe.Length * 100

            Case "sUnter"
                'Summe der Unterschreitungen
                '---------------------------
                Dim sUnter As Double = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) < objective.RefReihe.YWerte(i)) Then
                        sUnter += objective.RefReihe.YWerte(i) - SimReihe.YWerte(i)
                    End If
                Next
                QWert = sUnter

            Case "nÜber"
                'Relative Anzahl der Zeitschritte mit Überschreitungen (in Prozent)
                '------------------------------------------------------------------
                Dim nUeber As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) > objective.RefReihe.YWerte(i)) Then
                        nUeber += 1
                    End If
                Next
                QWert = nUeber / SimReihe.Length * 100

            Case "sÜber"
                'Summe der Überschreitungen
                '--------------------------
                Dim sUeber As Double = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) > objective.RefReihe.YWerte(i)) Then
                        sUeber += SimReihe.YWerte(i) - objective.RefReihe.YWerte(i)
                    End If
                Next
                QWert = sUeber

            Case "NashSutt"
                'Nash Sutcliffe
                '--------------
                'Mittelwert bilden
                Dim Qobs_quer, zaehler, nenner As Double
                For i = 0 To SimReihe.Length - 1
                    Qobs_quer += objective.RefReihe.YWerte(i)
                Next
                Qobs_quer = Qobs_quer / (SimReihe.Length)
                For i = 0 To SimReihe.Length - 1
                    zaehler += (objective.RefReihe.YWerte(i) - SimReihe.YWerte(i)) * (objective.RefReihe.YWerte(i) - SimReihe.YWerte(i))
                    nenner += (objective.RefReihe.YWerte(i) - Qobs_quer) * (objective.RefReihe.YWerte(i) - Qobs_quer)
                Next
                'abgeänderte Nash-Sutcliffe Formel: 0 als Zielwert (1- weggelassen)
                QWert = zaehler / nenner

            Case "Korr"
                'Korrelationskoeffizient (lineare Regression)
                'Es wird das Bestimmtheitsmaß r^2 zurückgegeben [0-1]
                '----------------------------------------------------
                Dim kovar, var_x, var_y, avg_x, avg_y As Double
                'Mittelwerte
                avg_x = SimReihe.getWert("Average")
                avg_y = objective.RefReihe.getWert("Average")
                'r^2 = sxy^2 / (sx^2 * sy^2)
                'Standardabweichung: var_x = sx^2 = 1 / (n-1) * SUMME[(x_i - x_avg)^2]
                'Kovarianz: kovar= sxy = 1 / (n-1) * SUMME[(x_i - x_avg) * (y_i - y_avg)]
                kovar = 0
                var_x = 0
                var_y = 0
                For i = 0 To SimReihe.Length - 1
                    kovar += (SimReihe.YWerte(i) - avg_x) * (objective.RefReihe.YWerte(i) - avg_y)
                    var_x += (SimReihe.YWerte(i) - avg_x) ^ 2
                    var_y += (objective.RefReihe.YWerte(i) - avg_y) ^ 2
                Next
                var_x = 1 / (SimReihe.Length - 1) * var_x
                var_y = 1 / (SimReihe.Length - 1) * var_y
                kovar = 1 / (SimReihe.Length - 1) * kovar
                'Bestimmtheitsmaß = Korrelationskoeffizient^2
                QWert = kovar ^ 2 / (var_x * var_y)

            Case Else
                Throw New Exception("Die Zielfunktion '" & objective.Funktion & "' wird nicht unterstützt!")

        End Select

        Return QWert

    End Function

    'Qualitätswert berechnen: Objective Typ = Wert
    '*********************************************
    Protected Function CalculateObjective_Wert(ByVal objective As Common.Objectivefunktion, ByVal SimReihe As Wave.Zeitreihe) As Double

        Dim QWert As Double
        Dim i As Integer

        'Simulationsreihe auf Evaluierungszeitraum kürzen
        Call SimReihe.Cut(objective.EvalStart, objective.EvalEnde)

        'Simulationswert aus Simulationsergebnis berechnen
        Dim SimWert As Double
        SimWert = SimReihe.getWert(objective.WertFunktion)

        'QWert berechnen
        '---------------
        'Fallunterscheidung Zielfunktion
        Select Case objective.Funktion

            Case "AbQuad"
                'Summe der Fehlerquadrate
                '------------------------
                QWert = (objective.RefWert - SimWert) * (objective.RefWert - SimWert)

            Case "Diff"
                'Summe der Fehler
                '----------------
                QWert = Math.Abs(objective.RefWert - SimWert)

            Case "nUnter"
                'Relative Anzahl der Zeitschritte mit Unterschreitungen (in Prozent)
                '-------------------------------------------------------------------
                Dim nUnter As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) < objective.RefWert) Then
                        nUnter += 1
                    End If
                Next
                QWert = nUnter / SimReihe.Length * 100

            Case "nÜber"
                'Relative Anzahl der Zeitschritte mit Überschreitungen (in Prozent)
                '------------------------------------------------------------------
                Dim nUeber As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) > objective.RefWert) Then
                        nUeber += 1
                    End If
                Next
                QWert = nUeber / SimReihe.Length * 100

            Case "sUnter"
                'Summe der Unterschreitungen
                '---------------------------
                Dim sUnter As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) < objective.RefWert) Then
                        sUnter += objective.RefWert - SimReihe.YWerte(i)
                    End If
                Next
                QWert = sUnter

            Case "sÜber"
                'Summe der Überschreitungen
                '--------------------------
                Dim sUeber As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) > objective.RefWert) Then
                        sUeber += SimReihe.YWerte(i) - objective.RefWert
                    End If
                Next
                QWert = sUeber

            Case Else
                Throw New Exception("Die Zielfunktion '" & objective.Funktion & "' wird für Werte nicht unterstützt!")

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

    ''' <summary>
    ''' Datensätze für Multithreading kopieren
    ''' </summary>
    ''' <returns>True wenn fertig</returns>
    ''' <remarks>Erstellt im bin-Ordner Verzeichnisse Thread_1 bis Thread_n</remarks>
    Private Function createThreadWorkDirs() As Boolean

        Dim i As Integer
        Dim isOK As Boolean
        Dim Source, Dest, relPaths() As String
        Dim binPath As String = System.Windows.Forms.Application.StartupPath()

        Source = Me.WorkDir_Original
        Dest = binPath & "\Thread_0\"

        'Alte Thread-Ordner löschen
        isOK = Me.deleteThreadWorkDirs()

        'zu kopierende Dateien bestimmen
        relPaths = Me.getDatensatzFiles(Source)

        'Dateien in Ordner Thread_0 kopieren
        For Each relPath As String In relPaths
            My.Computer.FileSystem.CopyFile(Source & relPath, Dest & relPath, True)
        Next

        'Für die weiteren Threads den Ordner Thread_0 kopieren
        Source = binPath & "\Thread_0\"

        For i = 1 To Me.n_Threads - 1

            Dest = binPath & "\Thread_" & i.ToString() & "\"
            My.Computer.FileSystem.CopyDirectory(Source, Dest, True)

        Next

        Return True

    End Function

    ''' <summary>
    ''' Gibt die relativen Pfade aller Datensatz-Dateien zurück
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
            '.svn-Verzeichnisse überspringen
            If (dir.Name <> ".svn") Then
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

            dir = System.Windows.Forms.Application.StartupPath() & "\Thread_" & i.ToString() & "\"

            If Directory.Exists(dir) Then
                Call EVO.Common.FileHelper.purgeReadOnly(dir)
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

        dir = System.Windows.Forms.Application.StartupPath() & "\Thread_" & Thread_ID.ToString() & "\"

        Return dir

    End Function

#End Region  'Multithreading

#End Region 'Methoden
End Class
