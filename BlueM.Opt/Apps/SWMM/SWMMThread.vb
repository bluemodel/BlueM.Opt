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
'Klasse beinhaltet alle Infomationen für einen Simulationslauf im Thread
'***********************************************************************
Imports IHWB.SWMM.DllAdapter

Public Class SWMMThread

    Private Thread_ID As Integer
    Private Child_ID As Integer
    Private WorkFolder As String
    Private DS_Name As String
    Private SWMM_dll As SWMM_EngineDotNetAccess
    Private SimIsOK As Boolean
    Private launchReady As Boolean

    Public Sub New(ByVal _Thread_ID As Integer, ByVal _Child_ID As Integer, ByVal _WorkFolder As String, ByVal _DS_Name As String, ByRef _SWMM_dll As SWMM_EngineDotNetAccess)
        Me.Thread_ID = _Thread_ID
        Me.Child_ID = _Child_ID
        Me.WorkFolder = _WorkFolder
        Me.DS_Name = _DS_Name
        Me.SWMM_dll = _SWMM_dll
    End Sub

    'Die Funktion startet die Simulation mit dem entsprechendem WorkingDir
    '*********************************************************************
    Public Sub launchSim()

        Me.SimIsOK = False
        Me.launchReady = False

        'Priority
        System.Threading.Thread.CurrentThread.Priority = Threading.ThreadPriority.BelowNormal

        Try
            'Datensatz übergeben und initialisieren
            Call SWMM_dll.Initialize(IO.Path.Combine(Me.WorkFolder, Me.DS_Name))
            Call SWMM_dll.Start(1)

            'Simulationszeitraum
            Dim elapsedTime As Double = 0.0

            Do
                Call SWMM_dll.PerformTimeStep(elapsedTime)
            Loop While (elapsedTime > 0.0)

            'Simulation abschliessen
            Call SWMM_dll.Finish()

            'Simulation erfolgreich
            Me.SimIsOK = True

        Catch ex As Exception

            'Simulationsfehler aufgetreten
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "SWMM")

            ''Simulation abschliessen
            'Call SWMM_dll.Finish()

            'Simulation nicht erfolgreich
            Me.SimIsOK = False

        Finally

            'Ressourcen deallokieren
            Call SWMM_dll.Dispose()

        End Try

        'Me.SimIsOK = False
        Me.launchReady = True

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
