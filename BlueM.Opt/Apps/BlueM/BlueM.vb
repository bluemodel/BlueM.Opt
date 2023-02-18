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
Imports System.Threading
Imports BlueM.DllAdapter
Imports BlueM
Imports BlueM.Opt.Common

''' <summary>
''' Klasse BlueMSim
''' Funktionen zur Kontrolle von BlueMSim
''' </summary>
Public Class BlueMSim
    Inherits Sim

#Region "Eigenschaften"

    'Eigenschaften
    '#############

    'BlueM DLL
    '---------
    Private dll_path As String
    Private bluem_dll() As BlueM_EngineDotNetAccess

    'Misc
    '----
    Private useKWL As Boolean       'gibt an, ob die KWL-Datei benutzt wird

    '**** Multithreading ****
    Dim MyBlueMThreads() As BlueMSimThread
    Dim MyThreads() As Thread

#End Region 'Eigenschaften

#Region "Properties"

    ''' <summary>
    ''' Alle Dateiendungen (ohne Punkt), die in einem Datensatz vorkommen können
    ''' </summary>
    ''' <remarks>Die erste Dateiendung in dieser Collection repräsentiert den Datensatz (wird z.B. als Filter für OpenFile-Dialoge verwendet)</remarks>
    Public Overrides ReadOnly Property DatensatzDateiendungen() As Collections.Specialized.StringCollection
        Get
            Dim exts As New Collections.Specialized.StringCollection()

            exts.AddRange(New String() {"ALL", "SYS", "FKT", "KTR", "EXT", "JGG", "WGG", _
                                        "TGG", "TAL", "HYA", "TRS", "EZG", "EIN", "URB", _
                                        "VER", "RUE", "BEK", "BOA", "BOD", "LNZ", "EFL", _
                                        "DIF", "ZRE", "BIN", "FKA"})

            Return exts

        End Get
    End Property

    ''' <summary>
    ''' Ob die Anwendung Multithreading unterstützt
    ''' </summary>
    ''' <returns>True</returns>
    Public Overrides ReadOnly Property MultithreadingSupported() As Boolean
        Get
            Return True
        End Get
    End Property

#End Region 'Properties

#Region "Methoden"

    'Methoden
    '########

    'Konstruktor
    '***********
    Public Sub New()

        Call MyBase.New()

        'Daten belegen
        '-------------
        Me.useKWL = False

        'Pfad zu BlueM.DLL bestimmen
        '---------------------------
        dll_path = IO.Path.Combine(System.Windows.Forms.Application.StartupPath(), "BlueM\BlueM.Sim.dll")

        If (Not File.Exists(dll_path)) Then
            Throw New Exception("BlueM.Sim.dll nicht gefunden!")
        End If

    End Sub

    ''' <summary>
    ''' BlueM.Sim Simulationen vorbereiten
    ''' </summary>
    Public Overrides Sub prepareSimulation()

        Call MyBase.prepareSimulation()

        'BlueM DLL instanzieren je nach Anzahl der Threads
        '-------------------------------------------------
        ReDim bluem_dll(n_Threads - 1)
        Dim i As Integer

        For i = 0 To n_Threads - 1
            bluem_dll(i) = New BlueM_EngineDotNetAccess(dll_path)
        Next

        'Thread-Objekte instanzieren
        ReDim MyBlueMThreads(n_Threads - 1)
        For i = 0 To n_Threads - 1
            MyBlueMThreads(i) = New BlueMSimThread(i, -1, "Folder", Datensatz, bluem_dll(i))
            MyBlueMThreads(i).set_is_OK()
        Next
        ReDim MyThreads(n_Threads - 1)

    End Sub

    Public Overrides Sub setProblem(ByRef prob As BlueM.Opt.Common.Problem)

        Call MyBase.setProblem(prob)

        'BlueM-spezifische Weiterverarbeitung von ZielReihen:
        '====================================================
        Dim objective As Common.ObjectiveFunction

        'KWL: Feststellen, ob irgendeine Zielfunktion die KWL-Datei benutzt
        '------------------------------------------------------------------
        For Each objective In Me.mProblem.List_ObjectiveFunctions
            If objective.Datei.ToUpper() = "KWL" Then
                Me.useKWL = True
                Exit For
            End If
        Next
        For Each constr As Constraintfunction In Me.mProblem.List_Constraintfunctions
            If constr.Datei.ToUpper() = "KWL" Then
                Me.useKWL = True
                Exit For
            End If
        Next

    End Sub

#Region "Eingabedateien lesen"

    'Simulationsparameter einlesen
    '*****************************
    Protected Overrides Sub Read_SimParameter()

        Dim SimStart_str As String = ""
        Dim SimEnde_str As String = ""
        Dim SimDT_str As String = ""
        Dim Ganglinie As String = ""
        Dim CSV_Format As String = ""

        'ALL-Datei öffnen
        '----------------
        Dim Datei As String = IO.Path.Combine(Me.WorkDir_Original, Me.Datensatz & ".ALL")

        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.Read)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        'Alle Zeilen durchlaufen
        Dim Zeile As String
        Do
            Zeile = StrRead.ReadLine.ToString()

            'Simulationszeitraum auslesen
            If (Zeile.StartsWith(" SimBeginn - SimEnde ............:")) Then
                SimStart_str = Zeile.Substring(35, 16)
                SimEnde_str = Zeile.Substring(54, 16)
            End If

            'Zeitschrittweite auslesen
            If (Zeile.StartsWith(" Zeitschrittlaenge [min] ........:")) Then
                SimDT_str = Zeile.Substring(35).Trim
            End If

            'Überprüfen ob die Ganglinien (.WEL Datei) ausgegeben wird
            If (Zeile.StartsWith(" Ganglinienausgabe ....... [J/N] :")) Then
                Ganglinie = Zeile.Substring(35).Trim
            End If

            'Überprüfen ob CSV Format eingeschaltet ist
            If (Zeile.StartsWith(" ... CSV-Format .......... [J/N] :")) Then
                CSV_Format = Zeile.Substring(35).Trim
            End If

        Loop Until StrRead.Peek() = -1

        'SimStart und SimEnde in echtes Datum konvertieren
        Me.SimStart = New DateTime(SimStart_str.Substring(6, 4), SimStart_str.Substring(3, 2), SimStart_str.Substring(0, 2), SimStart_str.Substring(11, 2), SimStart_str.Substring(14, 2), 0)
        Me.SimEnde = New DateTime(SimEnde_str.Substring(6, 4), SimEnde_str.Substring(3, 2), SimEnde_str.Substring(0, 2), SimEnde_str.Substring(11, 2), SimEnde_str.Substring(14, 2), 0)

        'Zeitschrittweite in echte Dauer konvertieren
        Me.SimDT = New TimeSpan(0, Convert.ToInt16(SimDT_str), 0)

        'Fehlermeldung Ganglinie nicht eingeschaltet
        If Ganglinie <> "J" Then
            Throw New Exception("Die Ganglinienausgabe (.WEL Datei) ist nicht eingeschaltet. Bitte in .ALL Datei unter 'Ganglinienausgabe' einschalten")
        End If

        'Fehlermeldung CSv Format nicht eingeschaltet
        If CSV_Format <> "J" Then
            Throw New Exception("Das CSV Format für die .WEL Datei ist nicht eingeschaltet. Bitte in .ALL unter '... CSV-Format' einschalten.")
        End If


    End Sub

#End Region 'Eingabedateien lesen

#Region "Evaluierung"

    'Gibt zurück ob ein beliebiger Thread beendet ist und ibt die ID diesen freien Threads zurück
    '********************************************************************************************
    Protected Overrides Function ThreadFree(ByRef Thread_ID As Integer) As Boolean
        ThreadFree = False

        For Each Thr_C As BlueMSimThread In MyBlueMThreads
            If Thr_C.Sim_Is_OK = True And Thr_C.get_Child_ID = -1 Then
                ThreadFree = True
                Thread_ID = Thr_C.get_Thread_ID
                Exit For
            End If
        Next

    End Function

    'BlauesModell ausführen (simulieren)
    'Startet einen neuen Thread und übergibt ihm die Child ID
    '********************************************************
    Protected Overrides Function launchSim(ByVal Thread_ID As Integer, ByVal Child_ID As Integer) As Boolean

        launchSim = False
        Dim Folder As String

        Folder = getThreadWorkDir(Thread_ID)
        MyBlueMThreads(Thread_ID) = New BlueMSimThread(Thread_ID, Child_ID, Folder, Datensatz, bluem_dll(Thread_ID))
        MyThreads(Thread_ID) = New Thread(AddressOf MyBlueMThreads(Thread_ID).launchSim)
        MyThreads(Thread_ID).IsBackground = True
        MyThreads(Thread_ID).Start()
        launchSim = True

        Return launchSim

    End Function

    'BlueM ohne Thread ausführen
    '***************************
    Protected Overrides Function launchSim() As Boolean

        Dim simOK As Boolean

        Try

            'Datensatz übergeben und initialisieren
            Call bluem_dll(0).Initialize(IO.Path.Combine(Me.WorkDir_Current, Me.Datensatz))

            Dim SimEnde As DateTime = BlueM_EngineDotNetAccess.BlueMDate2DateTime(bluem_dll(0).GetSimulationEndDate())

            'Simulationszeitraum 
            Do While (BlueM_EngineDotNetAccess.BlueMDate2DateTime(bluem_dll(0).GetCurrentTime) <= SimEnde)
                Call bluem_dll(0).PerformTimeStep()
            Loop

            'Simulation abschliessen
            Call bluem_dll(0).Finish()

            'Simulation erfolgreich
            simOK = True

        Catch ex As Exception

            'Simulationsfehler aufgetreten
            Common.Log.AddMessage(Common.Log.levels.error, ex.Message)

            'Simulation abschliessen
            Call bluem_dll(0).Finish()

            'Simulation nicht erfolgreich
            simOK = False

        Finally

            'Ressourcen deallokieren
            Call bluem_dll(0).Dispose()

        End Try

        Return simOK

    End Function

    'Prüft ob des aktuelle Child mit der ID die oben übergeben wurde fertig ist
    'Gibt die Thread ID zurück um zum auswerten in das Arbeitsverzeichnis zu wechseln
    '********************************************************************************
    Protected Overrides Function ThreadReady(ByRef Thread_ID As Integer, ByRef SimIsOK As Boolean, ByVal Child_ID As Integer) As Boolean
        ThreadReady = False

        For Each Thr_C As BlueMSimThread In MyBlueMThreads
            If Thr_C.launch_Ready = True And Thr_C.get_Child_ID = Child_ID Then
                ThreadReady = True
                SimIsOK = Thr_C.Sim_Is_OK
                Thread_ID = Thr_C.get_Thread_ID
                MyThreads(Thread_ID).Join()
                MyBlueMThreads(Thread_ID) = New BlueMSimThread(Thread_ID, -1, "Folder", Datensatz, bluem_dll(Thread_ID))
                MyBlueMThreads(Thread_ID).set_is_OK()
            End If
        Next

    End Function

    'Simulationsergebnis verarbeiten
    '-------------------------------
    Protected Overrides Sub SIM_Ergebnis_Lesen()

        'Altes Simulationsergebnis löschen
        Me.SimErgebnis.Clear()

        'Benötigte SimReihen zusammenstellen
        'TODO: das braucht eigentlich nicht nach jeder Simulation nochmal neu getan zu werden
        Dim SimReihen As New Dictionary(Of String, List(Of String)) '{file: [series]}
        SimReihen.Add("WEL", New List(Of String))
        If Me.useKWL Then
            SimReihen.Add("KWL", New List(Of String))
        End If
        For Each objfunc As ObjectiveFunction In Me.mProblem.List_ObjectiveFunctions
            If objfunc.GetObjType = ObjectiveFunction.ObjectiveType.Series Or
                objfunc.GetObjType = ObjectiveFunction.ObjectiveType.ValueFromSeries Then
                If Not SimReihen(objfunc.Datei.ToUpper()).Contains(objfunc.SimGr) Then
                    SimReihen(objfunc.Datei.ToUpper()).Add(objfunc.SimGr)
                End If
            End If
        Next
        For Each constr As Constraintfunction In Me.mProblem.List_Constraintfunctions
            If Not SimReihen(constr.Datei.ToUpper()).Contains(constr.SimGr) Then
                SimReihen(constr.Datei.ToUpper()).Add(constr.SimGr)
            End If
        Next

        'WEL-Datei einlesen
        '------------------
        Dim WELtmp As Wave.Fileformats.WEL = New Wave.Fileformats.WEL(IO.Path.Combine(Me.WorkDir_Current, Me.Datensatz & ".WEL"))

        'Benötigte Reihen für Import selektieren
        For Each series As String In SimReihen("WEL")
            WELtmp.selectSeries(series)
        Next
        'Datei einlesen
        WELtmp.readFile()
        'Zeitreihen übernehmen
        For Each zre As Wave.TimeSeries In WELtmp.TimeSeries.Values
            Me.SimErgebnis.Reihen.Add(zre.Title, zre)
        Next

        'ggf. KWL-Datei einlesen
        '-----------------------
        If (Me.useKWL) Then

            Dim KWLpath As String = IO.Path.Combine(Me.WorkDir_Current, Me.Datensatz & ".KWL")
            Dim KWLtmp As Wave.Fileformats.WEL = New Wave.Fileformats.WEL(KWLpath)

            'Benötigte Reihen für Import selektieren
            For Each series As String In SimReihen("KWL")
                KWLtmp.selectSeries(series)
            Next
            'Datei einlesen
            KWLtmp.readFile()
            'Zeitreihen übernehmen
            For Each zre As Wave.TimeSeries In KWLtmp.TimeSeries.Values
                Me.SimErgebnis.Reihen.Add(zre.Title, zre)
            Next

        End If

    End Sub

#End Region 'Evaluierung

#Region "Qualitätswertberechnung"

    'Qualitätswert aus PRB-Datei
    'TODO: PRB geht nicht (#153)
    '***********************
    Private Function CalculateObjective_PRB(ByVal objective As Common.ObjectiveFunction) As Double

        'Dim i As Integer
        'Dim IsOK As Boolean
        'Dim QWert As Double
        'Dim SimReihe As Object(,) = {}

        ''Simulationsergebnis auslesen
        'IsOK = Read_PRB(IO.Path.Combine(WorkDir, Datensatz & ".PRB"), ziel.SimGr, SimReihe)

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
        'Dim PRBintp(ziel.ZielReihe.GetUpperBound(0), 1) As Object
        'Dim j As Integer
        'For i = 0 To ziel.ZielReihe.GetUpperBound(0)
        '    'zugehörige Lamelle in SimReihe finden
        '    j = 0
        '    Do While (PRBtmp(j, 1) < ziel.ZielReihe(i, 1))
        '        j += 1
        '    Loop
        '    'interpolieren
        '    PRBintp(i, 0) = (PRBtmp(j + 1, 0) - PRBtmp(j, 0)) / (PRBtmp(j + 1, 1) - PRBtmp(j, 1)) * (ziel.ZielReihe(i, 1) - PRBtmp(j, 1)) + PRBtmp(j, 0)
        '    PRBintp(i, 1) = ziel.ZielReihe(i, 1)
        'Next

        'For i = 0 To ziel.ZielReihe.GetUpperBound(0)
        '    QWert += Math.Abs(ziel.ZielReihe(i, 0) - PRBintp(i, 0))
        'Next

        'Return QWert

    End Function

    'Ein Ergebnis aus einer PRB-Datei einlesen
    '*****************************************
    Private Function Read_PRB(ByVal DateiPfad As String, ByVal ZielGr As String, ByRef PRB(,) As Object) As Boolean

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

#End Region 'Qualitätswertberechnung

#End Region 'Methoden

End Class