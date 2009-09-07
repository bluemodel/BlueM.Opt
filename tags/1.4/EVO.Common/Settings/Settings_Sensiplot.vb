Imports System.Xml.Serialization

Public Class Settings_Sensiplot

    Public Enum SensiType As Integer
        discrete = 1
        normaldistribution = 2
    End Enum

    Private _Selected_OptParameters As Collections.Generic.List(Of Integer)
    Private _Selected_Objective As Integer
    Private _Selected_SensiType As SensiType
    Private _Num_Steps As Integer
    Private _show_Wave As Boolean

    <XmlIgnore()> _
    Public Property Selected_OptParameters() As Collections.Generic.List(Of Integer)
        Get
            Return _Selected_OptParameters
        End Get
        Set(ByVal value As Collections.Generic.List(Of Integer))
            _Selected_OptParameters = value
        End Set
    End Property

    <XmlIgnore()> _
    Public Property Selected_Objective() As Integer
        Get
            Return _Selected_Objective
        End Get
        Set(ByVal value As Integer)
            _Selected_Objective = value
        End Set
    End Property

    Public Property Selected_SensiType() As SensiType
        Get
            Return _Selected_SensiType
        End Get
        Set(ByVal value As SensiType)
            _Selected_SensiType = value
        End Set
    End Property

    <XmlIgnore()> _
    Public Property SensiType_Discrete() As Boolean
        Get
            Return (Me.Selected_SensiType = SensiType.discrete)
        End Get
        Set(ByVal value As Boolean)
            If (value = True) Then
                Me.Selected_SensiType = SensiType.discrete
            Else
                Me.Selected_SensiType = SensiType.normaldistribution
            End If
        End Set
    End Property

    <XmlIgnore()> _
    Public Property SensiType_NormalDistribution() As Boolean
        Get
            Return (Me.Selected_SensiType = SensiType.normaldistribution)
        End Get
        Set(ByVal value As Boolean)
            If (value = True) Then
                Me.Selected_SensiType = SensiType.normaldistribution
            Else
                Me.Selected_SensiType = SensiType.discrete
            End If
        End Set
    End Property

    Public Property Num_Steps() As Integer
        Get
            Return _Num_Steps
        End Get
        Set(ByVal value As Integer)
            _Num_Steps = value
        End Set
    End Property

    Public Property Show_Wave() As Boolean
        Get
            Return _show_Wave
        End Get
        Set(ByVal value As Boolean)
            _show_Wave = value
        End Set
    End Property

    Public Sub New()
        Me._Selected_OptParameters = New Collections.Generic.List(Of Integer)
    End Sub

    Public ReadOnly Property SensiTypeNormalDistributionEnabled As Boolean
        Get
            Return (Me.Selected_OptParameters.Count < 2)
        End Get
    End Property

    Public Sub setStandard()

        Me.Selected_SensiType = SensiType.discrete
        Me.Num_Steps = 10
        Me.Show_Wave = False

    End Sub

End Class
