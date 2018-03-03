' Copyright (c) BlueM Dev Group
' Website: http://bluemodel.org
' 
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
Public Class Individuum_PES
    Inherits Individuum

    Private mOptParas() As OptParameter  '06a Parameterarray für PES

    ''' <summary>
    ''' Die OptParameter als Objekte
    ''' </summary>
    Public Overrides Property OptParameter() As BlueM.Opt.Common.OptParameter()
        Get
            Return Me.mOptParas
        End Get
        Set(ByVal value As OptParameter())
            Me.mOptParas = value
        End Set
    End Property

    ''' <summary>
    ''' Konstruktor
    ''' </summary>
    ''' <param name="type">Frei definierbarer String</param>
    ''' <param name="id">Eindeutige Nummer</param>
    Public Sub New(ByVal type As String, ByVal id As Integer)

        'Basisindividuum instanzieren
        Call MyBase.New(type, id)

        'zusätzliche Individuum_PES-Eigenschaften:
        '-----------------------------------------
        Dim i As Integer

        'Parameterarray für PES aus Problem kopieren
        ReDim Me.mOptParas(Individuum.mProblem.NumOptParams - 1)
        For i = 0 To Individuum.mProblem.NumOptParams - 1
            Me.mOptParas(i) = Individuum.mProblem.List_OptParameter(i).Clone()
        Next

    End Sub

    ''' <summary>
    ''' Kopiert ein Individuum
    ''' </summary>
    ''' <returns>Individuum</returns>
    Public Overrides Function Clone() As Individuum

        Dim i As Integer

        Clone = New Individuum_PES(Me.mType, Me.mID)

        'Objective-Werte
        Call Array.Copy(Me.Objectives, Clone.Objectives, Me.Objectives.Length)

        'Constraint-Werte
        If (Not Me.Constraints.GetLength(0) = -1) Then
            Array.Copy(Me.Constraints, Clone.Constraints, Me.Constraints.Length)
        End If

        'Kennzeichnung ob Dominiert
        Clone.Dominated = Me.Dominated

        'Nummer der Pareto Front
        Clone.Front = Me.Front

        'Für crowding distance
        Clone.Distance = Me.Distance

        'Array für PES Parameter
        For i = 0 To Me.OptParameter.GetUpperBound(0)
            CType(Clone, Individuum_PES).mOptParas(i) = Me.mOptParas(i).Clone()
        Next

        Return Clone

    End Function

    ''' <summary>
    ''' Erzeugt ein neues (leeres) Individuum von der gleichen Klasse
    ''' </summary>
    ''' <returns>Das neue Individuum</returns>
    Public Overrides Function Create(Optional ByVal type As String = "tmp", Optional ByVal id As Integer = 0) As Individuum
        Dim ind As New Individuum_PES(type, id)
        Return ind
    End Function

End Class
