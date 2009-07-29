Imports System.Xml.Serialization

Public Class Settings_HookeJeeves

    Public DnStart As Double                   'Startschrittweite
    Public is_DnVektor As Boolean              'Soll ein Schrittweitenvektor benutzt werden
    Public DnFinish As Double                  'Abbruchschrittweite

    'Standardwerte setzen
    '********************
    Public Sub setStandard()
        Me.DnStart = 0.1
        Me.is_DnVektor = False
        Me.DnFinish = 0.0001
    End Sub

End Class
