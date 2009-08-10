Imports System.Xml.Serialization

Public Class Settings_DDS

    Private Dim _MaxIter As Integer
    Private Dim _R_val As Double
    Private Dim _RandomStartparameters As Boolean

    ''' <summary>
    ''' Number of iterations
    ''' </summary>
    Public Property MaxIter() As Integer
        Get
            Return _MaxIter
        End Get
        Set(ByVal value As Integer)
            _MaxIter = value
        End Set
    End Property

    ''' <summary>
    ''' DDS perturbation parameter
    ''' </summary>
    Public Property R_val() As Double
        Get
            Return _R_val
        End Get
        Set(ByVal value As Double)
            _R_val = value
        End Set
    End Property

    Public Property RandomStartparameters() As Boolean
        Get
            Return _RandomStartparameters
        End Get
        Set(ByVal value As Boolean)
            _RandomStartparameters = value
        End Set
    End Property

    'Standardwerte setzen
    '********************
    Public Sub setStandard()
        Me.MaxIter = 1000
        Me.R_val = 0.2
        Me.RandomStartparameters = True
    End Sub

End Class
