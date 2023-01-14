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

Public Class Settings_Sensiplot

    Public Enum SensiType As Integer
        evenDistribution = 1
        randomDistribution = 2
    End Enum

    Private _Selected_OptParameters As Collections.Generic.List(Of Integer)
    Private _Selected_Objective As Integer
    Private _selected_Mode As SensiType
    Private _Num_Steps As Integer
    Private _show_Wave As Boolean
    Private _save_Results As Boolean

    <XmlIgnore()>
    Public Property Selected_OptParameters() As Collections.Generic.List(Of Integer)
        Get
            Return _Selected_OptParameters
        End Get
        Set(ByVal value As Collections.Generic.List(Of Integer))
            _Selected_OptParameters = value
        End Set
    End Property

    <XmlIgnore()>
    Public Property Selected_Objective() As Integer
        Get
            Return _Selected_Objective
        End Get
        Set(ByVal value As Integer)
            _Selected_Objective = value
        End Set
    End Property

    Public Property Selected_Mode() As SensiType
        Get
            Return _selected_Mode
        End Get
        Set(ByVal value As SensiType)
            _selected_Mode = value
        End Set
    End Property

    <XmlIgnore()>
    Public Property ModeEvenDistribution() As Boolean
        Get
            Return (Me.Selected_Mode = SensiType.evenDistribution)
        End Get
        Set(ByVal value As Boolean)
            If (value = True) Then
                Me.Selected_Mode = SensiType.evenDistribution
            Else
                Me.Selected_Mode = SensiType.randomDistribution
            End If
        End Set
    End Property

    <XmlIgnore()>
    Public Property ModeRandomDistribution() As Boolean
        Get
            Return (Me.Selected_Mode = SensiType.randomDistribution)
        End Get
        Set(ByVal value As Boolean)
            If (value = True) Then
                Me.Selected_Mode = SensiType.randomDistribution
            Else
                Me.Selected_Mode = SensiType.evenDistribution
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

    Public Property Save_Results() As Boolean
        Get
            Return _save_Results
        End Get
        Set(value As Boolean)
            _save_Results = value
        End Set
    End Property

    Public Sub New()
        Me._Selected_OptParameters = New Collections.Generic.List(Of Integer)
    End Sub

    Public ReadOnly Property ModeRandomDistributionEnabled As Boolean
        Get
            Return (Me.Selected_OptParameters.Count < 2)
        End Get
    End Property

    Public Sub setStandard()

        Me.Selected_Mode = SensiType.evenDistribution
        Me.Num_Steps = 10
        Me.Show_Wave = False
        Me.Save_Results = False

    End Sub

End Class
