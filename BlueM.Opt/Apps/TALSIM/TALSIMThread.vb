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
''' Klasse beinhaltet alle Infomationen für einen Simulationslauf im Thread
''' </summary>
''' <remarks></remarks>
Public Class TalsimThread

    Private Thread_ID As Integer
    Private Child_ID As Integer
    Private WorkFolder As String
    Private DS_Name As String
    Private SimIsOK As Boolean
    Private launchReady As Boolean
    Public Shared exe_path As String

    Public Sub New(ByVal _Thread_ID As Integer, ByVal _Child_ID As Integer, ByVal _WorkFolder As String, ByVal _DS_Name As String)
        Me.Thread_ID = _Thread_ID
        Me.Child_ID = _Child_ID
        Me.WorkFolder = _WorkFolder
        Me.DS_Name = _DS_Name
    End Sub

    ''' <summary>
    ''' Die Funktion startet die Simulation mit dem entsprechendem WorkingDir
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub launchSim()

        Dim filestr As IO.FileStream
        Dim strread As IO.StreamReader

        Me.SimIsOK = False
        Dim isFinished As Boolean
        Me.launchReady = False

        'Priority
        System.Threading.Thread.CurrentThread.Priority = Threading.ThreadPriority.Normal

        Try
            'write the path to the dataset and the dataset name into a new run file
            'this is done for every simulation because otherwise we would have to keep track of runfiles and thread IDs separately
            Dim runfile As String = IO.Path.Combine(IO.Path.GetDirectoryName(exe_path), "talsim.run")
            If (Not IO.File.Exists(runfile)) Then
                Throw New Exception(runfile & " not found!")
            End If
            Dim line As String
            'read the template run file
            filestr = New IO.FileStream(runfile, IO.FileMode.Open, IO.FileAccess.Read)
            strread = New IO.StreamReader(filestr, System.Text.Encoding.GetEncoding("iso8859-1"))
            Dim lines As New Collections.Generic.List(Of String)
            Do
                line = strread.ReadLine()
                lines.Add(line)
            Loop Until strread.Peek = -1
            strread.Close()
            filestr.Close()

            'write a new run file
            Dim runfilename As String = $"{Me.DS_Name}_{Me.Thread_ID}.run"
            runfile = IO.Path.Combine(IO.Path.GetDirectoryName(TalsimThread.exe_path), runfilename)
            Dim strwrite As New IO.StreamWriter(runfile, False, System.Text.Encoding.GetEncoding("iso8859-1"))
            For Each line In lines
                If line.StartsWith("Path=") Then
                    'update the sim path
                    line = "Path=" & Me.WorkFolder
                ElseIf line.StartsWith("System=") Then
                    'update the dataset name
                    line = "System=" & Me.DS_Name
                End If
                strwrite.WriteLine(line)
            Next
            strwrite.Close()

            'TALSIM starten
            Dim errfile As String = IO.Path.Combine(Me.WorkFolder, Me.DS_Name & ".err")
            Dim errmsg As String
            Dim simendfile As String = IO.Path.Combine(Me.WorkFolder, Me.DS_Name & ".SIMEND")
            Dim proc As Process
            Dim startInfo As New ProcessStartInfo()
            startInfo.FileName = TalsimThread.exe_path
            startInfo.Arguments = runfilename
            startInfo.UseShellExecute = True
            startInfo.WindowStyle = ProcessWindowStyle.Hidden
            startInfo.WorkingDirectory = IO.Path.GetDirectoryName(TalsimThread.exe_path)

            'Carry out up to 5 simulation attempts, because TALSIM sometimes blocks access to the time series files in multithreading mode
            Dim n_attempts As Integer = 5
            For i_attempt As Integer = 1 To n_attempts

                errmsg = ""
                Me.SimIsOK = False

                'start
                proc = Process.Start(startInfo)
                'DEBUG: write to log
                'BlueM.Opt.Common.Log.AddMessage($"Thread {Me.Thread_ID}: {startInfo.FileName} {startInfo.Arguments}")
                'wait until finished
                Do
                    isFinished = proc.WaitForExit(100)
                    System.Windows.Forms.Application.DoEvents()
                Loop Until isFinished
                'close the process
                proc.Close()

                'Simulation erfolgreich?
                If Not IO.File.Exists(errfile) And IO.File.Exists(simendfile) Then
                    Me.SimIsOK = True
                    Exit For
                End If

                'if .ERR file exists, simulation finished with errors
                If IO.File.Exists(errfile) Then
                    'read err-file
                    errmsg = $"Thread {Me.Thread_ID}: TALSIM simulation ended with errors:"
                    filestr = New IO.FileStream(errfile, IO.FileMode.Open, IO.FileAccess.Read)
                    strread = New IO.StreamReader(filestr, System.Text.Encoding.GetEncoding("iso8859-1"))
                    Do
                        line = strread.ReadLine()
                        errmsg &= BlueM.Opt.Common.eol & line
                    Loop Until strread.Peek = -1
                    strread.Close()
                    filestr.Close()
                End If

                'if .SIMEND does not exist, simulation aborted prematurely
                If Not IO.File.Exists(simendfile) Then
                    errmsg = $"Thread {Me.Thread_ID}: TALSIM simulation aborted prematurely!"
                End If

                'Log error message
                BlueM.Opt.Common.Log.AddMessage(errmsg)

                If i_attempt < n_attempts Then
                    BlueM.Opt.Common.Log.AddMessage($"Thread {Me.Thread_ID}: TALSIM simulation attempt {i_attempt} was unsuccessful, trying again...")
                    System.Threading.Thread.Sleep(100)
                Else
                    BlueM.Opt.Common.Log.AddMessage($"Thread {Me.Thread_ID}: TALSIM simulation attempt {i_attempt} was unsuccessful, parameter set will be discarded!")
                End If

            Next

        Catch ex As Exception

            'Simulationsfehler aufgetreten
            BlueM.Opt.Common.Log.AddMessage(ex.Message)

            'Simulation nicht erfolgreich
            Me.SimIsOK = False

        Finally

            'ready for next sim
            Me.launchReady = True

        End Try

    End Sub

    Public Function Sim_Is_OK() As Boolean

        Sim_Is_OK = Me.SimIsOK
    End Function

    Public Function launch_Ready() As Boolean

        launch_Ready = Me.launchReady
    End Function

    Public Sub set_is_OK()

        Me.SimIsOK = True
    End Sub

    Public Function get_Thread_ID() As Integer

        get_Thread_ID = Me.Thread_ID
    End Function

    Public Function get_Child_ID() As Integer

        get_Child_ID = Me.Child_ID
    End Function

End Class