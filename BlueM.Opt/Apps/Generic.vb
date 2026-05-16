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
Public Class Generic
    Inherits Sim

    ''' <summary>
    ''' Eine StringCollection mit allen Dateiendungen (ohne Punkt), die in einem Datensatz vorkommen können
    ''' </summary>
    ''' <remarks>Die erste Dateiendung in dieser Collection repräsentiert den Datensatz (wird z.B. als Filter für OpenFile-Dialoge verwendet)</remarks>
    Public Overrides ReadOnly Property DatensatzDateiendungen() As Collections.Specialized.StringCollection
        Get
            Dim extensions As New Collections.Specialized.StringCollection()
            extensions.Add("bat")
            Return extensions
        End Get
    End Property

    ''' <summary>
    ''' Multithreading support flag
    ''' </summary>
    ''' <returns>False</returns>
    Public Overrides ReadOnly Property MultithreadingSupported() As Boolean
        Get
            'TODO: enable multithreading
            Return False
        End Get
    End Property

    ''' <summary>
    ''' Die Sim-Anwendung für die Simulation vorbereiten
    ''' </summary>
    Public Overrides Sub prepareSimulation()

        'TODO: Immer im Originalverzeichnis des Datensatzes simulieren?
        Me.WorkDir_Current = Me.WorkDir_Original

    End Sub

    ''' <summary>
    ''' Simulationsparameter einlesen
    ''' </summary>
    Protected Overrides Sub Read_SimParameter()
        'TODO
    End Sub

    ''' <summary>
    ''' Launch simulation
    ''' </summary>
    ''' <remarks>launches the batch file named launchSim.bat in the dataset directory</remarks>
    Protected Overloads Overrides Function launchSim() As Boolean

        Dim proc As Process
        Dim startInfo As New ProcessStartInfo()
        Dim isFinished As Boolean = False

        startInfo.FileName = Me.Datensatz & Me.DatensatzExtension
        'startInfo.Arguments = ""
        startInfo.UseShellExecute = True
        startInfo.WindowStyle = ProcessWindowStyle.Normal
        startInfo.WorkingDirectory = IO.Path.GetDirectoryName(Me.WorkDir_Current)
        'start
        proc = Process.Start(startInfo)
        'wait until finished
        Do
            isFinished = proc.WaitForExit(100)
            System.Windows.Forms.Application.DoEvents()
        Loop Until isFinished

        'close the process
        proc.Close()

    End Function

    'For Multithreading
    Protected Overloads Overrides Function launchSim(ByVal Thread_ID As Integer, ByVal Child_ID As Integer) As Boolean
        'pass
    End Function
    Protected Overloads Overrides Function ThreadFree(ByRef Thread_ID As Integer) As Boolean
        'pass
    End Function
    Protected Overloads Overrides Function ThreadReady(ByRef Thread_ID As Integer, ByRef SimIsOK As Boolean, ByVal Child_ID As Integer) As Boolean
        'pass
    End Function

    ''' <summary>
    ''' Simulationsergebnis einlesen
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub SIM_Ergebnis_Lesen()
        'TODO:

    End Sub

End Class
