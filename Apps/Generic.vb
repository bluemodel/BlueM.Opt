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
    ''' TODO: this should not be MustOverride in the base class
    ''' </summary>
    Protected Overrides Sub Read_Verzweigungen()
        'pass
    End Sub

    ''' <summary>
    ''' TODO: this should not be MustOverride in the base class
    ''' </summary>
    Protected Overrides Sub Write_Verzweigungen()
        'pass
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
