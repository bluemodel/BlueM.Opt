' Copyright (c) 2011, ihwb, TU Darmstadt
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
