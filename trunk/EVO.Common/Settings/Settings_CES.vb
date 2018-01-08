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

Public Class Settings_CES

#Region "Eigenschaften"

    Private _Startparameter As EVO_STARTPARAMETERS

    Private _OptModus As String

    'CES
    Private _n_Generations As Integer
    Private _n_Parents As Integer
    Private _n_Children As Integer
    Private _OptStrategie As EVO_STRATEGY
    Private _OptReprodOp As CES_REPRODOP
    Private _k_Value As Integer
    Private _OptMutOperator As CES_MUTATION
    Private _pr_MutRate As Integer

    'Sekpop
    Private _Is_SecPop As Boolean
    Private _Is_SecPopRestriction As Boolean
    Private _n_MemberSecondPop As Integer
    Private _n_Interact As Integer

    'Hybrid
    Private _RealOptEnabled As Boolean
    Private _is_RealOpt As Boolean
    Private _HybridType As HYBRID_TYPE
    Private _Mem_Strategy As MEMORY_STRATEGY
    Private _n_PES_MemSize As Integer
    Private _is_PES_SecPop As Boolean
    Private _n_PES_MemSecPop As Integer
    Private _n_PES_Interact As Integer
    Private _is_PopMutStart As Boolean

#End Region 'Eigenschaften

#Region "Properties"

    ''' <summary>
    ''' Startparameter (NICHT GENUTZT!)
    ''' </summary>
    Public Property Startparameter() As EVO_STARTPARAMETERS
        Get
            Return _Startparameter
        End Get
        Set(ByVal value As EVO_STARTPARAMETERS)
            _Startparameter = value
        End Set
    End Property

    ''' <summary>
    ''' Modus (CES, HYBRID, Testmodus...)
    ''' </summary>
    Public Property OptModus() As String
        Get
            Return _OptModus
        End Get
        Set(ByVal value As String)
            _OptModus = value
        End Set
    End Property

    ''' <summary>
    ''' Anzahl der Generationen
    ''' </summary>
    Public Property N_Generations() As Integer
        Get
            Return _n_Generations
        End Get
        Set(ByVal value As Integer)
            _n_Generations = value
        End Set
    End Property

    ''' <summary>
    ''' Anzahl der Eltern
    ''' </summary>
    Public Property N_Parents() As Integer
        Get
            Return _n_Parents
        End Get
        Set(ByVal value As Integer)
            _n_Parents = value
        End Set
    End Property

    ''' <summary>
    ''' Anzahl der Kinder
    ''' </summary>
    Public Property N_Children() As Integer
        Get
            Return _n_Children
        End Get
        Set(ByVal value As Integer)
            _n_Children = value
        End Set
    End Property

    ''' <summary>
    ''' "plus" oder "minus" Strategie
    ''' </summary>
    Public Property OptStrategie() As EVO_STRATEGY
        Get
            Return _OptStrategie
        End Get
        Set(ByVal value As EVO_STRATEGY)
            _OptStrategie = value
        End Set
    End Property

    ''' <summary>
    ''' Reproduktionsoperator
    ''' </summary>
    Public Property OptReprodOp() As CES_REPRODOP
        Get
            Return _OptReprodOp
        End Get
        Set(ByVal value As CES_REPRODOP)
            _OptReprodOp = value

            'Neuen K-Value setzen
            If (_OptReprodOp = EVO.Common.CES_REPRODOP.k_Point_Crossover) Then
                _k_Value = 3
            Else
                _k_Value = 0
            End If

        End Set
    End Property

    ''' <summary>
    ''' Anzahl der Schnittpunkte
    ''' </summary>
    Public Property K_Value() As Integer
        Get
            Return _k_Value
        End Get
        Set(ByVal value As Integer)
            _k_Value = value
        End Set
    End Property

    Public ReadOnly Property K_ValueEnabled() As Boolean
        Get
            Return (_OptReprodOp = CES_REPRODOP.k_Point_Crossover)
        End Get
    End Property

    ''' <summary>
    ''' Mutationsoperator
    ''' </summary>
    Public Property OptMutOperator() As CES_MUTATION
        Get
            Return _OptMutOperator
        End Get
        Set(ByVal value As CES_MUTATION)
            _OptMutOperator = value
        End Set
    End Property

    ''' <summary>
    ''' Definiert die Wahrscheinlichkeit der Mutationsrate in %
    ''' </summary>
    Public Property Pr_MutRate() As Integer
        Get
            Return _pr_MutRate
        End Get
        Set(ByVal value As Integer)
            _pr_MutRate = value
        End Set
    End Property

#Region "SekPop"

    ''' <summary>
    ''' Sekundäre Population an oder aus
    ''' </summary>
    Public Property Is_SecPop() As Boolean
        Get
            Return _Is_SecPop
        End Get
        Set(ByVal value As Boolean)
            _Is_SecPop = value
        End Set
    End Property

    ''' <summary>
    ''' Sekundäre Population begrenzen
    ''' </summary>
    Public Property Is_SecPopRestriction() As Boolean
        Get
            Return _Is_SecPopRestriction
        End Get
        Set(ByVal value As Boolean)
            _Is_SecPopRestriction = value
        End Set
    End Property

    ''' <summary>
    ''' Max Anzahl der Mitglieder der Sekundären Population
    ''' </summary>
    Public Property N_MemberSecondPop() As Integer
        Get
            Return _n_MemberSecondPop
        End Get
        Set(ByVal value As Integer)
            _n_MemberSecondPop = value
        End Set
    End Property

    ''' <summary>
    ''' Austausch mit SekPop nach n Generationen
    ''' </summary>
    Public Property N_Interact() As Integer
        Get
            Return _n_Interact
        End Get
        Set(ByVal value As Integer)
            _n_Interact = value
        End Set
    End Property

#End Region 'Sekpop

#Region "Hybrid"

    ''' <summary>
    ''' Ob Hybrid als Methode möglich ist
    ''' </summary>
    <XmlIgnore()> _
    Public Property RealOptEnabled() As Boolean
        Get
            Return _RealOptEnabled
        End Get
        Set(ByVal value As Boolean)
            _RealOptEnabled = value
            'Wenn Hybrid nicht möglich, ausschalten
            If (_RealOptEnabled = False) Then
                Me.Is_RealOpt = False
            End If
        End Set
    End Property

    ''' <summary>
    ''' gibt an ob auch die Real Parameter optimiert werden sollen
    ''' </summary>
    Public Property Is_RealOpt() As Boolean
        Get
            Return _is_RealOpt
        End Get
        Set(ByVal value As Boolean)
            _is_RealOpt = value
        End Set
    End Property

    ''' <summary>
    ''' gibt den Hybrid Typ an
    ''' </summary>
    Public Property HybridType() As HYBRID_TYPE
        Get
            Return _HybridType
        End Get
        Set(ByVal value As HYBRID_TYPE)
            _HybridType = value
        End Set
    End Property

    ''' <summary>
    ''' Gibt die Memory Strategy an
    ''' </summary>
    Public Property Mem_Strategy() As MEMORY_STRATEGY
        Get
            Return _Mem_Strategy
        End Get
        Set(ByVal value As MEMORY_STRATEGY)
            _Mem_Strategy = value
        End Set
    End Property

    ''' <summary>
    ''' Die Größe des PES Memory
    ''' </summary>
    Public Property N_PES_MemSize() As Integer
        Get
            Return _n_PES_MemSize
        End Get
        Set(ByVal value As Integer)
            _n_PES_MemSize = value
        End Set
    End Property

    ''' <summary>
    ''' SekundärePopulation für PES an oder aus
    ''' </summary>
    Public Property Is_PES_SecPop() As Boolean
        Get
            Return _is_PES_SecPop
        End Get
        Set(ByVal value As Boolean)
            _is_PES_SecPop = value
        End Set
    End Property

    ''' <summary>
    ''' Anzahl der Mitglieder der Sekundären Population für PES
    ''' </summary>
    Public Property N_PES_MemSecPop() As Integer
        Get
            Return _n_PES_MemSecPop
        End Get
        Set(ByVal value As Integer)
            _n_PES_MemSecPop = value
        End Set
    End Property

    ''' <summary>
    ''' Austausch mit SekPop für PES nach n Generationen
    ''' </summary>
    Public Property N_PES_Interact() As Integer
        Get
            Return _n_PES_Interact
        End Get
        Set(ByVal value As Integer)
            _n_PES_Interact = value
        End Set
    End Property

    ''' <summary>
    ''' Gibt an ob die PES bei der Population oder bei den Eltern gestartet wird
    ''' </summary>
    Public Property Is_PopMutStart() As Boolean
        Get
            Return _is_PopMutStart
        End Get
        Set(ByVal value As Boolean)
            _is_PopMutStart = value
        End Set
    End Property

#End Region 'Hybrid

#End Region 'Properties

    'Standardwerte setzen
    '********************
    Public Sub setStandard(ByVal Method As String)

        'wird nicht genutzt!
        Me.Startparameter = EVO_STARTPARAMETERS.Original

        Me.OptModus = Method

        Select Case Method

            Case METH_CES

                'CES
                Me.N_Generations = 10
                Me.N_Parents = 5
                Me.N_Children = 20
                Me.OptStrategie = EVO_STRATEGY.Plus_Strategy
                Me.OptReprodOp = CES_REPRODOP.Uniform_Crossover
                Me.OptMutOperator = CES_MUTATION.RND_Switch
                Me.Pr_MutRate = 10

                Me.Is_SecPop = True
                Me.Is_SecPopRestriction = True
                Me.N_MemberSecondPop = 50
                Me.N_Interact = 5

                'Hybrid
                Me.RealOptEnabled = False
                Me.HybridType = HYBRID_TYPE.Mixed_Integer
                Me.Mem_Strategy = MEMORY_STRATEGY.C_This_Loc
                Me.N_PES_MemSize = 50
                Me.Is_PES_SecPop = False
                Me.N_PES_MemSecPop = 50
                Me.N_PES_Interact = 5
                Me.Is_PopMutStart = False

            Case METH_HYBRID

                'CES
                Me.N_Generations = 100
                Me.N_Parents = 3
                Me.N_Children = 7
                Me.OptStrategie = EVO_STRATEGY.Plus_Strategy
                Me.OptReprodOp = CES_REPRODOP.Uniform_Crossover
                Me.OptMutOperator = CES_MUTATION.RND_Switch
                Me.Pr_MutRate = 10

                Me.Is_SecPop = True
                Me.Is_SecPopRestriction = True
                Me.N_MemberSecondPop = 50
                Me.N_Interact = 5

                'Hybrid
                Me.RealOptEnabled = True
                Me.Is_RealOpt = True
                Me.HybridType = HYBRID_TYPE.Mixed_Integer
                Me.Mem_Strategy = MEMORY_STRATEGY.C_This_Loc
                Me.N_PES_MemSize = 50

                Me.Is_PES_SecPop = False
                Me.N_PES_MemSecPop = 50
                Me.N_PES_Interact = 5
                Me.Is_PopMutStart = False

        End Select

    End Sub

    'Settings für TestModus
    '**********************
    Public Sub setTestModus(ByVal Modus As CES_T_MODUS, ByVal Path() As Integer, ByVal nGen As Integer, ByVal nParents As Integer, ByVal NChildren As Integer)

        Dim i As Integer
        Dim PathStr As String

        If NChildren = 1 Then
            PathStr = "   Path: "
            For i = 0 To Path.GetUpperBound(0)
                PathStr = PathStr & Path(i) & " "
            Next
            PathStr = PathStr.TrimEnd
        Else
            PathStr = " n:"
            PathStr = PathStr & NChildren
        End If

        Me.OptModus = Modus.ToString & PathStr
        Me.N_Generations = nGen
        Me.N_Parents = nParents
        Me.N_Children = NChildren

    End Sub

End Class
