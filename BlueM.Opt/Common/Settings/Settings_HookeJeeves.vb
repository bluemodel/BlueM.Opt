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
