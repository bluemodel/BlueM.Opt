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
Imports System.Xml.Serialization

Public Class Settings_TSP

    Private _n_Cities As Integer
    Private _n_Gen As Integer
    Private _n_Parents As Integer
    Private _n_Children As Integer

    Private _Problem As EnProblem
    Private _ReprodOperator As EnReprodOperator
    Private _MutOperator As EnMutOperator
    Private _Strategy As EVO_STRATEGY

    Public Property N_Cities() As Integer
        Get
            Return _n_Cities
        End Get
        Set(ByVal value As Integer)
            _n_Cities = value
        End Set
    End Property

    Public Property N_Gen() As Integer
        Get
            Return _n_Gen
        End Get
        Set(ByVal value As Integer)
            _n_Gen = value
        End Set
    End Property

    Public Property N_Parents() As Integer
        Get
            Return _n_Parents
        End Get
        Set(ByVal value As Integer)
            _n_Parents = value
        End Set
    End Property

    Public Property N_Children() As Integer
        Get
            Return _n_Children
        End Get
        Set(ByVal value As Integer)
            _n_Children = value
        End Set
    End Property

    Public Property Problem() As EnProblem
        Get
            Return _Problem
        End Get
        Set(ByVal value As EnProblem)
            _Problem = value
        End Set
    End Property

    Public Property ReprodOperator() As EnReprodOperator
        Get
            Return _ReprodOperator
        End Get
        Set(ByVal value As EnReprodOperator)
            _ReprodOperator = value
        End Set
    End Property

    Public Property MutOperator() As EnMutOperator
        Get
            Return _MutOperator
        End Get
        Set(ByVal value As EnMutOperator)
            _MutOperator = value
        End Set
    End Property

    Public Property Strategy() As EVO_STRATEGY
        Get
            Return _Strategy
        End Get
        Set(ByVal value As EVO_STRATEGY)
            _Strategy = value
        End Set
    End Property

    Public Sub setStandard()

        N_Cities = 70
        N_Gen = 20000
        N_Parents = 5   'mindestens 3 Eltern!
        N_Children = 40

        Problem = EnProblem.circle
        ReprodOperator = EnReprodOperator.Order_Crossover_OX
        MutOperator = EnMutOperator.Translocation_3_Opt
        Strategy = EVO_STRATEGY.Plus_Strategy
    End Sub

End Class
