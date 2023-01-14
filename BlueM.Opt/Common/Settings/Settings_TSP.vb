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
