' Copyright (c) BlueM Dev Group
' Website: http://bluemodel.org
' 
' All rights reserved.
' 
' Released under the BSD-2-Clause License:
' 
' Redistribution and use in source and binary forms, with or without modification, 
' are permitted provided that the following conditions are met:
' 
' * Redistributions of source code must retain the above copyright notice, this list 
'   of conditions and the following disclaimer.
' * Redistributions in binary form must reproduce the above copyright notice, this list 
'   of conditions and the following disclaimer in the documentation and/or other materials 
'   provided with the distribution.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
' EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
' OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
' SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT 
' OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
' HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
' TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
' EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'--------------------------------------------------------------------------------------------
'
Imports System.IO
Imports System.Threading
Imports System.Globalization
Imports BlueM

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
    Private useKWL As Boolean       'gibt an, ob die KTR.WEL-Datei benutzt wird

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

            exts.AddRange(New String() {"ALL", "SYS", "FKT", "KTR", "EXT", "JGG", "WGG", _
                                        "TGG", "TAL", "HYA", "TRS", "EZG", "EIN", "URB", _
                                        "VER", "RUE", "BEK", "BOA", "BOD", "LNZ", "EFL", _
                                        "DIF", "FKA", "SCE", "QAB", "UPD"})

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

        'Pfad zu talsimw.exe bestimmen
        '-----------------------------
        exe_path = System.Windows.Forms.Application.StartupPath() & "\TALSIM\talsimw64.exe"

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

    Public Overrides Sub setProblem(ByRef prob As EVO.Common.Problem)

        Call MyBase.setProblem(prob)

        'TALSIM-spezifische Weiterverarbeitung von ZielReihen:
        Dim objective As Common.ObjectiveFunction

        'KTR.WEL: Feststellen, ob irgendeine Zielfunktion die KTR.WEL-Datei benutzt
        For Each objective In Me.mProblem.List_ObjectiveFunctions
            If (objective.Datei = "KTR.WEL") Then
                Me.useKWL = True
                Exit For
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

        Const dateformat As String = "dd.MM.yyyy HH:mm"

        'open the .ALL file
        '------------------
        Dim Datei As String = Me.WorkDir_Original & Me.Datensatz & ".ALL"

        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.Read)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        'read all settings
        Do
            line = StrRead.ReadLine.ToString().Trim()

            If line.Length = 0 Then Continue Do

            If Not (line.StartsWith("*") Or line.StartsWith("#") Or line.StartsWith("["))
                kvp = line.Split("=")
                settings.Add(kvp(0).ToUpper(), kvp(1).ToUpper())
            End If

        Loop Until StrRead.Peek() = -1

        'check settings

        'WEL output
        If Not settings("WEL") = "J"
            Throw New Exception("Die Ganglinienausgabe ist in der .ALL Datei nicht eingeschaltet! Es muss 'WEL=J' eingestellt sein!")
        End If

        'store SimStart and SimEnd
        Me.SimStart = DateTime.ParseExact(settings("SIMSTART"), dateformat, New NumberformatInfo())
        Me.SimEnde = DateTime.ParseExact(settings("SIMEND"), dateformat, New NumberformatInfo())

        'store timestep length
        'TODO: what if TIMESTEP_MONTH=J?
        Me.SimDT = New TimeSpan(0, settings("TIMESTEP_MIN"), 0)

    End Sub

    ''' <summary>
    ''' Liest die Verzweigungen
    ''' </summary>
    ''' <remarks>not used</remarks>
    Protected Overrides Sub Read_Verzweigungen()

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
            'EVO.Diagramm.Monitor.getInstance().LogAppend(startInfo.FileName & " " & startInfo.Arguments)
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
                    errmsg &= EVO.Common.eol & line
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
            EVO.Diagramm.Monitor.getInstance().LogAppend(ex.Message)

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

        'TODO: hier nur die Reihen einlesen, die auch für objfunctions gebraucht werden

        'Altes Simulationsergebnis löschen
        Me.SimErgebnis.Clear()

        'WEL-Datei einlesen
        '------------------
        Dim WELtmp As Wave.WEL = New Wave.WEL(Me.WorkDir_Current & Me.Datensatz & ".WEL", True)

        'Reihen zu Simulationsergebnis hinzufügen
        For Each zre As Wave.TimeSeries In WELtmp.TimeSeries
            Me.SimErgebnis.Reihen.Add(zre.Title, zre)
        Next

        'ggf. KWL-Datei einlesen
        '-----------------------
        If (Me.useKWL) Then

            Dim KWLpath As String = Me.WorkDir_Current & Me.Datensatz & ".KTR.WEL"

            Dim KWLtmp As Wave.WEL = New Wave.WEL(KWLpath, True)

            'Reihen zu Simulationsergebnis hinzufügen
            For Each zre As Wave.TimeSeries In KWLtmp.TimeSeries
                Me.SimErgebnis.Reihen.Add(zre.Title, zre)
            Next

        End If

    End Sub

#End Region 'Evaluierung

    ''' <summary>
    ''' Schreibt die neuen Verzweigungen
    ''' </summary>
    ''' <remarks>not used</remarks>
    Protected Overrides Sub Write_Verzweigungen()

    End Sub

#End Region 'Methoden

End Class