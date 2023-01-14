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
