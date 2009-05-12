'Klasse beinhaltet alle Infomationen für einen Simulationslauf im Thread
'***********************************************************************
Imports IHWB.BlueM.DllAdapter

Public Class BlueMThread

    Private Thread_ID As Integer
    Private Child_ID As Integer
    Private WorkFolder As String
    Private DS_Name As String
    Private bluem_dll As BlueM_EngineDotNetAccess
    Private SimIsOK As Boolean
    Private launchReady As Boolean

    Public Sub New(ByVal _Thread_ID As Integer, ByVal _Child_ID As Integer, ByVal _WorkFolder As String, ByVal _DS_Name As String, ByRef _bluem_dll As BlueM_EngineDotNetAccess)
        Me.Thread_ID = _Thread_ID
        Me.Child_ID = _Child_ID
        Me.WorkFolder = _WorkFolder
        Me.DS_Name = _DS_Name
        Me.bluem_dll = _bluem_dll
    End Sub

    'Die Funktion startet die Simulation mit dem entsprechendem WorkingDir
    '*********************************************************************
    Public Sub launchSim()

        Me.SimIsOK = False
        Me.launchReady = False

        'Priority
        System.Threading.Thread.CurrentThread.Priority = Threading.ThreadPriority.Normal

        Try
            'Datensatz übergeben und initialisieren
            Call bluem_dll.Initialize(Me.WorkFolder & Me.DS_Name)

            Dim SimEnde As DateTime = BlueM_EngineDotNetAccess.BlueMDate2DateTime(bluem_dll.GetSimulationEndDate())

            'Simulationszeitraum
            Do While (BlueM_EngineDotNetAccess.BlueMDate2DateTime(bluem_dll.GetCurrentTime) <= SimEnde)
                Call bluem_dll.PerformTimeStep()
            Loop

            'Simulation abschliessen
            Call bluem_dll.Finish()

            'Simulation erfolgreich
            Me.SimIsOK = True

        Catch ex As Exception

            'Simulationsfehler aufgetreten
            EVO.Diagramm.Monitor.getInstance().LogAppend(ex.Message)

            'Simulation abschliessen
            Call bluem_dll.Finish()

            'Simulation nicht erfolgreich
            Me.SimIsOK = False

        Finally

            'Ressourcen deallokieren
            Call bluem_dll.Dispose()

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