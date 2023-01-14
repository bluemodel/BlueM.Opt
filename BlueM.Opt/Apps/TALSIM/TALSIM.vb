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
Imports System.Globalization
Imports BlueM
Imports BlueM.Opt.Common

''' <summary>
''' Class TALSIM for carrying out simulations using TALSIM
''' </summary>
''' <remarks></remarks>
Public Class Talsim
    Inherits Sim

#Region "Eigenschaften"

    Private exe_path As String

    'Misc
    '----
    Private WELfiles As List(Of String) 'List of WEL files to use (e.g. "WEL", "KTR.WEL", "CHLO.WEL", etc.)

    '**** Multithreading ****
    Dim MyTalsimThreads() As TalsimThread
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

            exts.AddRange(New String() {"ALL", "SYS", "FKT", "KTR", "EXT", "JGG", "WGG",
                                        "TGG", "TAL", "HYA", "TRS", "EZG", "EIN", "URB",
                                        "VER", "RUE", "BEK", "BOA", "BOD", "LNZ", "EFL",
                                        "DIF", "FKA", "SCE", "QAB", "UPD", "OPF", "KAL",
                                        "HYO", "TEM", "ZIE", "QUA", "PRO", "GRW", "IRR",
                                        "RFD"})

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
        Me.WELfiles = New List(Of String)

        'Pfad zu talsimw64.exe bestimmen
        '-------------------------------
        'attempt to get exe_path from UserSettings
        exe_path = My.Settings.TALSIM_path

        If (Not File.Exists(exe_path)) Then
            'use default location instead
            exe_path = IO.Path.Combine(System.Windows.Forms.Application.StartupPath(), "TALSIM\talsimw64.exe")
            If My.Settings.TALSIM_path.Trim() <> "" Then
                MsgBox($"UserSetting for TALSIM_path {My.Settings.TALSIM_path} was not found.{eol}Using default {exe_path} instead.", MsgBoxStyle.Information)
            End If
        End If

        If (Not File.Exists(exe_path)) Then
            Throw New Exception(exe_path & " not found!")
        End If

    End Sub

    ''' <summary>
    ''' TALSIM Simulationen vorbereiten
    ''' </summary>
    Public Overrides Sub prepareSimulation()

        Call MyBase.prepareSimulation()

        'Thread-Objekte instanzieren
        TalsimThread.exe_path = Me.exe_path
        ReDim MyTalsimThreads(n_Threads - 1)
        For i = 0 To n_Threads - 1
            MyTalsimThreads(i) = New TalsimThread(i, -1, "Folder", Datensatz)
            MyTalsimThreads(i).set_is_OK()
        Next
        ReDim MyThreads(n_Threads - 1)

    End Sub

    Public Overrides Sub setProblem(ByRef prob As BlueM.Opt.Common.Problem)

        Call MyBase.setProblem(prob)

        'TALSIM-spezifische Weiterverarbeitung von ZielReihen:
        Dim objective As Common.ObjectiveFunction

        'Feststellen, welche WEL-Dateien in Zielfunktionen genutzt werden
        For Each objective In Me.mProblem.List_ObjectiveFunctions
            If Not IsNothing(objective.Datei) Then
                Dim welfile As String = objective.Datei.ToUpper()
                If Not Me.WELfiles.Contains(welfile) Then
                    Me.WELfiles.Add(welfile)
                End If
            End If
        Next

        'Feststellen, welche WEL-Dateien in Constraints genutzt werden
        For Each constr As Constraintfunction In Me.mProblem.List_Constraintfunctions
            Dim welfile As String = constr.Datei.ToUpper()
            If Not Me.WELfiles.Contains(welfile) Then
                Me.WELfiles.Add(welfile)
            End If
        Next

    End Sub

#Region "Eingabedateien lesen"

    'Simulationsparameter einlesen
    '*****************************
    Protected Overrides Sub Read_SimParameter()

        Dim line As String
        Dim kvp As String()
        Dim settings As New Dictionary(Of String, String)

        'open the .ALL file
        '------------------
        Dim Datei As String = IO.Path.Combine(Me.WorkDir_Original, Me.Datensatz & ".ALL")

        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.Read)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        'read all settings
        Do
            line = StrRead.ReadLine.ToString().Trim()

            If line.Length = 0 Then Continue Do

            If line.Contains("=") And Not (line.StartsWith("*") Or line.StartsWith("#") Or line.StartsWith("[")) Then
                kvp = line.Split("=")
                settings.Add(kvp(0).ToUpper(), kvp(1).ToUpper())
            End If

        Loop Until StrRead.Peek() = -1

        'check settings
        'WEL output
        If Not settings.ContainsKey("WEL") Then
            Throw New Exception("Key ""WEL"" not found in .ALL file!")
        End If
        If Not settings("WEL") = "J" Then
            Throw New Exception("Die Ganglinienausgabe ist in der .ALL Datei nicht eingeschaltet! Es muss 'WEL=J' eingestellt sein!")
        End If

        'Simstart and Simend
        If Not settings.ContainsKey("SIMSTART") Or Not settings.ContainsKey("SIMEND") Then
            Throw New Exception("Key ""SimStart"" and/or ""SimEnd"" not found in .ALL file!")
        End If
        'parse and store SimStart and SimEnd
        'date format can be "dd.MM.yyyy HH:mm" or "dd/MM/yyyy HH:mm"
        Me.SimStart = New DateTime(settings("SIMSTART").Substring(6, 4),
                                   settings("SIMSTART").Substring(3, 2),
                                   settings("SIMSTART").Substring(0, 2),
                                   settings("SIMSTART").Substring(11, 2),
                                   settings("SIMSTART").Substring(14, 2),
                                   0)
        Me.SimEnde = New DateTime(settings("SIMEND").Substring(6, 4),
                                   settings("SIMEND").Substring(3, 2),
                                   settings("SIMEND").Substring(0, 2),
                                   settings("SIMEND").Substring(11, 2),
                                   settings("SIMEND").Substring(14, 2),
                                   0)
        If Not settings.ContainsKey("TIMESTEP_MIN") Then
            Throw New Exception("Key ""TimeStep_min"" not found in .ALL file!")
        End If
        'store timestep length
        'TODO: what if TIMESTEP_MONTH=J?
        Me.SimDT = New TimeSpan(0, settings("TIMESTEP_MIN"), 0)

    End Sub

#End Region 'Eingabedateien lesen

#Region "Evaluierung"

    ''' <summary>
    ''' Gibt zurück ob ein beliebiger Thread beendet ist und gibt die ID diesen freien Threads zurück
    ''' </summary>
    ''' <param name="Thread_ID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides Function ThreadFree(ByRef Thread_ID As Integer) As Boolean
        ThreadFree = False

        For Each Thr_C As TalsimThread In MyTalsimThreads
            If Thr_C.Sim_Is_OK = True And Thr_C.get_Child_ID = -1 Then
                ThreadFree = True
                Thread_ID = Thr_C.get_Thread_ID
                Exit For
            End If
        Next

    End Function

    ''' <summary>
    ''' Carry out a multithreaded simulation
    ''' </summary>
    ''' <param name="Thread_ID"></param>
    ''' <param name="Child_ID"></param>
    ''' <returns></returns>
    ''' <remarks>starts a new thread and gives it the Child_ID</remarks>
    Protected Overrides Function launchSim(ByVal Thread_ID As Integer, ByVal Child_ID As Integer) As Boolean

        launchSim = False
        Dim Folder As String

        Folder = getThreadWorkDir(Thread_ID)
        MyTalsimThreads(Thread_ID) = New TalsimThread(Thread_ID, Child_ID, Folder, Datensatz)
        MyThreads(Thread_ID) = New Thread(AddressOf MyTalsimThreads(Thread_ID).launchSim)
        MyThreads(Thread_ID).IsBackground = True
        MyThreads(Thread_ID).Start()
        launchSim = True

        Return launchSim

    End Function

    ''' <summary>
    ''' Carry out a simulation (single-threaded)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides Function launchSim() As Boolean

        Dim filestr As IO.FileStream
        Dim strread As IO.StreamReader
        Dim simOK As Boolean
        Dim isFinished As Boolean

        Try

            'write the path to the dataset and the dataset name into a new run file
            'this is done for every simulation because the workdir may change
            Dim runfile As String = IO.Path.Combine(IO.Path.GetDirectoryName(exe_path), "talsim.run")
            If (Not File.Exists(runfile)) Then
                Throw New Exception(runfile & " not found!")
            End If
            Dim line As String
            'read the template run file
            filestr = New FileStream(runfile, FileMode.Open, IO.FileAccess.Read)
            strread = New StreamReader(filestr, System.Text.Encoding.GetEncoding("iso8859-1"))
            Dim lines As New Collections.Generic.List(Of String)
            Do
                line = strread.ReadLine()
                lines.Add(line)
            Loop Until strread.Peek = -1
            strread.Close()
            filestr.Close()

            'write a new run file
            Dim runfilename As String = MyBase.Datensatz & ".run"
            runfile = IO.Path.Combine(IO.Path.GetDirectoryName(Me.exe_path), runfilename)
            Dim strwrite As New StreamWriter(runfile, False, System.Text.Encoding.GetEncoding("iso8859-1"))
            For Each line In lines
                If line.StartsWith("Path=") Then
                    'update the sim path
                    line = "Path=" & MyBase.WorkDir_Current
                ElseIf line.StartsWith("System=") Then
                    'update the dataset name
                    line = "System=" & MyBase.Datensatz
                End If
                strwrite.WriteLine(line)
            Next
            strwrite.Close()

            'TALSIM starten
            Dim errfile As String = IO.Path.Combine(Me.WorkDir_Current, Me.Datensatz & ".err")
            Dim simendfile As String = IO.Path.Combine(Me.WorkDir_Current, Me.Datensatz & ".SIMEND")
            Dim proc As Process
            Dim startInfo As New ProcessStartInfo()
            startInfo.FileName = Me.exe_path
            startInfo.Arguments = runfilename
            startInfo.UseShellExecute = True
            startInfo.WindowStyle = ProcessWindowStyle.Hidden
            startInfo.WorkingDirectory = IO.Path.GetDirectoryName(Me.exe_path)
            'start
            proc = Process.Start(startInfo)
            'DEBUG: write to log
            'BlueM.Opt.Diagramm.Monitor.getInstance().LogAppend(startInfo.FileName & " " & startInfo.Arguments)
            'wait until finished
            Do
                isFinished = proc.WaitForExit(100)
                System.Windows.Forms.Application.DoEvents()
            Loop Until isFinished
            'close the process
            proc.Close()

            'if .ERR file exists, simulation finished with errors
            If IO.File.Exists(errfile) Then
                'read err-file
                Dim errmsg As String = "TALSIM simulation ended with errors:"
                filestr = New IO.FileStream(errfile, IO.FileMode.Open, IO.FileAccess.Read)
                strread = New IO.StreamReader(filestr, System.Text.Encoding.GetEncoding("iso8859-1"))
                Do
                    line = strread.ReadLine()
                    errmsg &= BlueM.Opt.Common.eol & line
                Loop Until strread.Peek = -1
                strread.Close()
                filestr.Close()

                Throw New Exception(errmsg)
            End If

            'if .SIMEND does not exist, simulation aborted prematurely
            If Not IO.File.Exists(simendfile) Then
                Throw New Exception("TALSIM simulation aborted prematurely!")
            End If

            'Simulation erfolgreich
            simOK = True

        Catch ex As Exception

            'Simulationsfehler aufgetreten
            BlueM.Opt.Diagramm.Monitor.getInstance().LogAppend(ex.Message)

            'Simulation nicht erfolgreich
            simOK = False

        Finally

            'nothing to do

        End Try

        Return simOK

    End Function

    ''' <summary>
    ''' Prüft ob das aktuelle Child mit der ID die oben übergeben wurde fertig ist
    ''' Gibt die Thread ID zurück um zum auswerten in das Arbeitsverzeichnis zu wechseln
    ''' </summary>
    ''' <param name="Thread_ID"></param>
    ''' <param name="SimIsOK"></param>
    ''' <param name="Child_ID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides Function ThreadReady(ByRef Thread_ID As Integer, ByRef SimIsOK As Boolean, ByVal Child_ID As Integer) As Boolean
        ThreadReady = False

        For Each Thr_C As TalsimThread In MyTalsimThreads
            If Thr_C.launch_Ready = True And Thr_C.get_Child_ID = Child_ID Then
                ThreadReady = True
                SimIsOK = Thr_C.Sim_Is_OK
                Thread_ID = Thr_C.get_Thread_ID
                MyThreads(Thread_ID).Join()
                MyTalsimThreads(Thread_ID) = New TalsimThread(Thread_ID, -1, "Folder", Datensatz)
                MyTalsimThreads(Thread_ID).set_is_OK()
            End If
        Next

    End Function

    ''' <summary>
    ''' Simulationsergebnis lesen
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub SIM_Ergebnis_Lesen()

        'Altes Simulationsergebnis löschen
        Me.SimErgebnis.Clear()

        'Benötigte SimReihen zusammenstellen
        'TODO: das braucht eigentlich nicht nach jeder Simulation nochmal neu getan zu werden
        Dim SimReihen As New Dictionary(Of String, List(Of String)) '{file: [series]}
        For Each welfile As String In Me.WELfiles
            SimReihen.Add(welfile, New List(Of String))
        Next
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

        'WEL-Dateien einlesen
        '--------------------
        For Each welfile As String In Me.WELfiles

            Dim WELtmp As Wave.FileFormats.WEL = New Wave.FileFormats.WEL(IO.Path.Combine(Me.WorkDir_Current, Me.Datensatz & "." & welfile))

            'Benötigte Reihen für Import selektieren
            For Each series As String In SimReihen(welfile)
                WELtmp.selectSeries(series)
            Next
            'Datei einlesen
            WELtmp.readFile()
            'Zeitreihen übernehmen
            For Each zre As Wave.TimeSeries In WELtmp.TimeSeries.Values
                Me.SimErgebnis.Reihen.Add(zre.Title, zre)
            Next
        Next

    End Sub

#End Region 'Evaluierung

#End Region 'Methoden

End Class