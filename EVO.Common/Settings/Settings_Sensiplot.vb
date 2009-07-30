Imports System.Xml.Serialization

Public Class Settings_Sensiplot

    Public Enum SensiType As Integer
        discrete = 1
        normaldistribution = 2
    End Enum

    Public Selected_OptParameters() As Integer
    Public Selected_Objective As Integer
    Public Selected_SensiType As SensiType
    Public Num_Steps As Integer
    Public show_Wave As Boolean

    Public Sub setStandard()

        'Standardm‰ﬂig ersten OptParameter und erstes Objective ausw‰hlen
        ReDim Me.Selected_OptParameters(0)
        Me.Selected_OptParameters(0) = 0
        Me.Selected_Objective = 0
        Me.Selected_SensiType = SensiType.discrete
        Me.Num_Steps = 10
        Me.show_Wave = False

    End Sub

End Class
