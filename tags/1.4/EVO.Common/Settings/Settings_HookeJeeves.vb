Imports System.Xml.Serialization

Public Class Settings_HookeJeeves

    Private _DnStart As Double
    Private _Is_DnVektor As Boolean
    Private _DnFinish As Double

    ''' <summary>
    ''' Startschrittweite
    ''' </summary>
    Public Property DnStart() As Double
        Get
            Return _DnStart
        End Get
        Set(ByVal value As Double)
            _DnStart = value
        End Set
    End Property

    ''' <summary>
    ''' Abbruchschrittweite
    ''' </summary>
    Public Property DnFinish() As Double
        Get
            Return _DnFinish
        End Get
        Set(ByVal value As Double)
            _DnFinish = value
        End Set
    End Property

    ''' <summary>
    ''' Soll ein Schrittweitenvektor benutzt werden
    ''' TODO: Schrittweitenvektor bei Hooke-Jeeves nicht genutzt!
    ''' </summary>
    Public Property Is_DnVektor() As Boolean
        Get
            Return _Is_DnVektor
        End Get
        Set(ByVal value As Boolean)
            _Is_DnVektor = value
        End Set
    End Property

    'Standardwerte setzen
    '********************
    Public Sub setStandard()
        Me.DnStart = 0.1
        Me.Is_DnVektor = False
        Me.DnFinish = 0.0001
    End Sub

End Class
