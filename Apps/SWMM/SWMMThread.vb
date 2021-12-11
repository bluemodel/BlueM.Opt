﻿' Copyright (c) BlueM Dev Group
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