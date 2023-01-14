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

Public Class Settings_PES

#Region "Eigenschaften"

    Private _OptModus As EVO_MODE
    Private _Strategie As EVO_STRATEGY
    Private _Startparameter As EVO_STARTPARAMETERS
    Private _N_Gen As Integer
    Private _N_Eltern As Integer
    Private _N_Nachf As Integer
    Private _Reproduktionsoperator As PES_REPRODOP
    Private _Mutationsoperator As PES_MUTATIONSOP
    Private _N_RekombXY As Integer
    Private _Is_DiversityTournament As Boolean

#End Region 'Eigenschaften

#Region "Properties"

    ''' <summary>
    ''' Single- oder Multi-Objective
    ''' </summary>
    Public Property OptModus() As EVO_MODE
        Get
            Return _OptModus
        End Get
        Set(ByVal value As EVO_MODE)
            _OptModus = value
            'Neuen Standardwert für PopPenalty setzen
            Select Case _OptModus
                Case EVO_MODE.Single_Objective
                    Me.Pop.PopPenalty = EVO_POP_PENALTY.Average
                Case EVO_MODE.Multi_Objective
                    Me.Pop.PopPenalty = EVO_POP_PENALTY.Crowding
            End Select
        End Set
    End Property

    ''' <summary>
    ''' Startparameter
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
    ''' Typ der Evolutionsstrategie (+ oder ,)
    ''' </summary>
    Public Property Strategie() As EVO_STRATEGY
        Get
            Return _Strategie
        End Get
        Set(ByVal value As EVO_STRATEGY)
            _Strategie = value
        End Set
    End Property

    ''' <summary>
    ''' Anzahl Generationen
    ''' </summary>
    Public Property N_Gen() As Integer
        Get
            Return _N_Gen
        End Get
        Set(ByVal value As Integer)
            _N_Gen = value
        End Set
    End Property

    ''' <summary>
    ''' Anzahl Eltern
    ''' </summary>
    Public Property N_Eltern() As Integer
        Get
            Return _N_Eltern
        End Get
        Set(ByVal value As Integer)
            _N_Eltern = value
        End Set
    End Property

    ''' <summary>
    ''' Anzahl Nachfahren
    ''' </summary>
    Public Property N_Nachf() As Integer
        Get
            Return _N_Nachf
        End Get
        Set(ByVal value As Integer)
            _N_Nachf = value
        End Set
    End Property

    ''' <summary>
    ''' Ermittlung der Individuum-Eltern
    ''' </summary>
    Public Property Reproduktionsop() As PES_REPRODOP
        Get
            Return _Reproduktionsoperator
        End Get
        Set(ByVal value As PES_REPRODOP)
            _Reproduktionsoperator = value
            'Diversity Tournament aktualisieren
            Select Case Me.Reproduktionsop
                Case PES_REPRODOP.XY_Discrete, PES_REPRODOP.XY_Average, PES_REPRODOP.Neighborhood, PES_REPRODOP.XY_Average_Discrete
                    Me.Is_DiversityTournament = True
                Case Else
                    Me.Is_DiversityTournament = False
            End Select
        End Set
    End Property

    ''' <summary>
    ''' X/Y-Schema Rekombination
    ''' </summary>
    Public Property N_RekombXY() As Integer
        Get
            Return _N_RekombXY
        End Get
        Set(ByVal value As Integer)
            _N_RekombXY = value
        End Set
    End Property

    ''' <summary>
    ''' Art der Mutation
    ''' </summary>
    Public Property Mutationsop() As PES_MUTATIONSOP
        Get
            Return _Mutationsoperator
        End Get
        Set(ByVal value As PES_MUTATIONSOP)
            _Mutationsoperator = value
        End Set
    End Property

    ''' <summary>
    ''' Vor der eigentlichen Auswahl eines Elter wird zunächst nach der besseren Diversität geschaut
    ''' </summary>
    Public Property Is_DiversityTournament() As Boolean
        Get
            Return _Is_DiversityTournament
        End Get
        Set(ByVal value As Boolean)
            _Is_DiversityTournament = value
        End Set
    End Property

#End Region 'Properties

    'Unterklassen
    '************
    Public SetMutation As Settings_Mutation
    Public SekPop As Settings_SekPop
    Public Pop As Settings_Pop

    Public Class Settings_Mutation
        Private _DnStart As Double
        Private _DnEpsilon As Double
        Private _IsDnVektor As Boolean
        Private _DnC As Double

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
        ''' Minimale Schrittweite
        ''' </summary>
        Public Property DnEpsilon() As Double
            Get
                Return _DnEpsilon
            End Get
            Set(ByVal value As Double)
                _DnEpsilon = value
            End Set
        End Property

        ''' <summary>
        ''' Soll ein Schrittweitenvektor benutzt werden
        ''' </summary>
        Public Property IsDnVektor() As Boolean
            Get
                Return _IsDnVektor
            End Get
            Set(ByVal value As Boolean)
                _IsDnVektor = value
            End Set
        End Property

        ''' <summary>
        ''' Skalierung des learning Parameters
        ''' </summary>
        Public Property DnC() As Double
            Get
                Return _DnC
            End Get
            Set(ByVal value As Double)
                _DnC = value
            End Set
        End Property
    End Class

    Public Class Settings_SekPop
        Private _N_Interact As Integer
        Private _Is_Begrenzung As Boolean
        Private _N_MaxMembers As Integer

        ''' <summary>
        ''' Austausch zwischen Population und Sekundärer Population?
        ''' </summary>
        Public ReadOnly Property Is_Interact() As Boolean
            Get
                Return (Me._N_Interact > 0)
            End Get
        End Property

        ''' <summary>
        ''' Alle wieviel Generationen soll die aktuelle Population mit Mitgliedern der sekundären Population aufgefüllt werden
        ''' </summary>
        Public Property N_Interact() As Integer
            Get
                Return _N_Interact
            End Get
            Set(ByVal value As Integer)
                _N_Interact = value
            End Set
        End Property

        ''' <summary>
        ''' Soll die Anzahl Mitglieder in der SekPop begrenzt werden?
        ''' </summary>
        Public Property Is_Begrenzung() As Boolean
            Get
                Return _Is_Begrenzung
            End Get
            Set(ByVal value As Boolean)
                _Is_Begrenzung = value
            End Set
        End Property

        ''' <summary>
        ''' Maximale Anzahl Mitglieder der Sekundärpopulation
        ''' </summary>
        Public Property N_MaxMembers() As Integer
            Get
                Return _N_MaxMembers
            End Get
            Set(ByVal value As Integer)
                _N_MaxMembers = value
            End Set
        End Property

    End Class

    Public Class Settings_Pop
        Private _Is_POPUL As Boolean
        Private _N_Runden As Integer
        Private _N_Popul As Integer
        Private _N_PopEltern As Integer
        Private _PopEltern As EVO_POP_ELTERN
        Private _PopStrategie As EVO_STRATEGY
        Private _PopPenalty As EVO_POP_PENALTY

        ''' <summary>
        ''' Mit Populationen?
        ''' </summary>
        <XmlAttribute()> _
        Public Property Is_POPUL() As Boolean
            Get
                Return _Is_POPUL
            End Get
            Set(ByVal value As Boolean)
                _Is_POPUL = value
                'Standardwerte setzen
                If (_Is_POPUL) Then
                    Me.N_Runden = 10
                    Me.N_Popul = 3
                    Me.N_PopEltern = 2
                Else
                    Me.N_Runden = 1
                    Me.N_Popul = 1
                    Me.N_PopEltern = 1
                End If
            End Set
        End Property

        ''' <summary>
        ''' Anzahl Runden
        ''' </summary>
        Public Property N_Runden() As Integer
            Get
                Return _N_Runden
            End Get
            Set(ByVal value As Integer)
                _N_Runden = value
            End Set
        End Property

        ''' <summary>
        ''' Anzahl Populationen
        ''' </summary>
        Public Property N_Popul() As Integer
            Get
                Return _N_Popul
            End Get
            Set(ByVal value As Integer)
                _N_Popul = value
            End Set
        End Property

        ''' <summary>
        ''' Anzahl Populationseltern
        ''' </summary>
        Public Property N_PopEltern() As Integer
            Get
                Return _N_PopEltern
            End Get
            Set(ByVal value As Integer)
                _N_PopEltern = value
            End Set
        End Property

        ''' <summary>
        ''' Ermittlung der Populationseltern
        ''' </summary>
        Public Property PopEltern() As EVO_POP_ELTERN
            Get
                Return _PopEltern
            End Get
            Set(ByVal value As EVO_POP_ELTERN)
                _PopEltern = value
            End Set
        End Property

        ''' <summary>
        ''' Typ der Evolutionsstrategie (+ oder ,) auf Populationsebene
        ''' </summary>
        Public Property PopStrategie() As EVO_STRATEGY
            Get
                Return _PopStrategie
            End Get
            Set(ByVal value As EVO_STRATEGY)
                _PopStrategie = value
            End Set
        End Property

        ''' <summary>
        ''' Art der Beurteilung der Populationsgüte (Multiobjective)
        ''' </summary>
        Public Property PopPenalty() As EVO_POP_PENALTY
            Get
                Return _PopPenalty
            End Get
            Set(ByVal value As EVO_POP_PENALTY)
                _PopPenalty = value
            End Set
        End Property
    End Class


#Region "Methoden"

    'Standardwerte setzen
    '********************
    Public Sub setStandard(ByVal modus As EVO_MODE)

        Me.OptModus = modus

        Select Case Me.OptModus

            Case EVO_MODE.Single_Objective

                Me.OptModus = EVO_MODE.Single_Objective
                Me.Strategie = EVO_STRATEGY.Plus_Strategy
                Me.Startparameter = EVO_STARTPARAMETERS.Original

                Me.Mutationsop = PES_MUTATIONSOP.Rechenberg
                Me.SetMutation.DnStart = 0.1
                Me.SetMutation.DnEpsilon = 0.001
                Me.SetMutation.IsDnVektor = False
                Me.SetMutation.DnC = 1.0

                Me.N_Gen = 100
                Me.N_Eltern = 3
                Me.N_Nachf = 10

                Me.SekPop.N_Interact = 0
                Me.SekPop.Is_Begrenzung = False
                Me.SekPop.N_MaxMembers = 0

                Me.Reproduktionsop = PES_REPRODOP.XX_Average_Discrete
                Me.N_RekombXY = 3
                Me.Is_DiversityTournament = False

                Me.Pop.Is_POPUL = False
                Me.Pop.PopEltern = EVO_POP_ELTERN.Recombination
                Me.Pop.PopStrategie = EVO_STRATEGY.Plus_Strategy
                Me.Pop.PopPenalty = EVO_POP_PENALTY.Average


            Case EVO_MODE.Multi_Objective

                Me.OptModus = EVO_MODE.Multi_Objective
                Me.Strategie = EVO_STRATEGY.Plus_Strategy
                Me.Startparameter = EVO_STARTPARAMETERS.Original

                Me.Mutationsop = PES_MUTATIONSOP.Rechenberg
                Me.SetMutation.DnStart = 0.1
                Me.SetMutation.IsDnVektor = False
                Me.SetMutation.DnC = 1.0

                Me.N_Gen = 100
                Me.N_Eltern = 15
                Me.N_Nachf = 50

                Me.SekPop.N_Interact = 10
                Me.SekPop.Is_Begrenzung = True
                Me.SekPop.N_MaxMembers = 50

                Me.Reproduktionsop = PES_REPRODOP.XX_Average_Discrete
                Me.N_RekombXY = 3
                Me.Is_DiversityTournament = True

                Me.Pop.Is_POPUL = False
                Me.Pop.PopEltern = EVO_POP_ELTERN.Recombination
                Me.Pop.PopStrategie = EVO_STRATEGY.Plus_Strategy
                Me.Pop.PopPenalty = EVO_POP_PENALTY.Average

        End Select
    End Sub

    Public Sub New()
        Me.SetMutation = New Settings_Mutation()
        Me.SekPop = New Settings_SekPop()
        Me.Pop = New Settings_Pop()
    End Sub

#End Region

#Region "FormBindingProperties"

    Public ReadOnly Property StrategieEnabled() As Boolean
        Get
            Return (Me.OptModus = EVO_MODE.Single_Objective)
        End Get
    End Property

    Public ReadOnly Property SekPopEnabled() As Boolean
        Get
            Return (Me.OptModus = EVO_MODE.Multi_Objective)
        End Get
    End Property

    Public ReadOnly Property PopulEnabled() As Boolean
        Get
            If (Me.OptModus = EVO_MODE.Single_Objective) Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property PopPenaltyOptions() As EVO_POP_PENALTY()
        Get
            Dim Items() As EVO_POP_PENALTY
            Items = New EVO_POP_PENALTY() {}
            Select Case Me._OptModus
                Case EVO_MODE.Single_Objective
                    Items = New EVO_POP_PENALTY() {EVO_POP_PENALTY.Average, EVO_POP_PENALTY.Worst}
                Case EVO_MODE.Multi_Objective
                    'TODO: Popgüte bei MultiObjective überflüssig? (#183)
                    Items = New EVO_POP_PENALTY() {EVO_POP_PENALTY.Crowding, EVO_POP_PENALTY.Span}
            End Select
            Return Items
        End Get
    End Property

    Public ReadOnly Property OptModusString() As String
        Get
            Return _OptModus.ToString()
        End Get
    End Property

    Public ReadOnly Property RecombXYEnabled() As Boolean
        Get
            Select Case Me.Reproduktionsop
                Case PES_REPRODOP.XY_Discrete, PES_REPRODOP.XY_Average, PES_REPRODOP.Neighborhood, PES_REPRODOP.XY_Average_Discrete
                    Return True
                Case Else
                    Return False
            End Select
        End Get
    End Property

#End Region 'FormBindingProperties

End Class
