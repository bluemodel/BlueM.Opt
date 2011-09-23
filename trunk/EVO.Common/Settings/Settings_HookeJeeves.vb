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
