Imports System.Xml.Serialization

Public Class Settings_MetaEvo

    Private _Role As String
    Private _OpMode As String

    Private _PopulationSize As Integer
    Private _NumberGenerations As Integer
    Private _NumberResults As Integer

    Private _HJStepsize As Integer

    'Datenbank-Connection
    Private _MySQL_Host As String
    Private _MySQL_Database As String
    Private _MySQL_User As String
    Private _MySQL_Password As String

    'Versteckte Optionen
    Public ChildrenPerParent As Integer

    'Zustandsvariablen
    '-----------------
    'Aktuelle Generationenzahl
    <XmlIgnore()> _
    Public CurrentGeneration As Integer
    'Aktueller Zustand des Algomanagers
    <XmlIgnore()> _
    Public AlgoMode As String

#Region "Properties"

    ''' <summary>
    ''' Was stellt dieser PC da: Single PC, Network Server, Network Client
    ''' </summary>
    Public Property Role() As String
        Get
            Return _Role
        End Get
        Set(ByVal value As String)
            _Role = value
        End Set
    End Property

    ''' <summary>
    ''' Optimierungen: Local Optimizer, Global Optimizer, Both
    ''' </summary>
    Public Property OpMode() As String
        Get
            Return _OpMode
        End Get
        Set(ByVal value As String)
            _OpMode = value
        End Set
    End Property

    ''' <summary>
    ''' Populationsgrösse
    ''' </summary>
    Public Property PopulationSize() As Integer
        Get
            Return _PopulationSize
        End Get
        Set(ByVal value As Integer)
            _PopulationSize = value
        End Set
    End Property

    ''' <summary>
    ''' Anzahl der Generationen die berechnet werden
    ''' </summary>
    Public Property NumberGenerations() As Integer
        Get
            Return _NumberGenerations
        End Get
        Set(ByVal value As Integer)
            _NumberGenerations = value
        End Set
    End Property

    ''' <summary>
    ''' Anzahl der Lösungen nach der Globalen Optimierung und am Ende
    ''' </summary>
    Public Property NumberResults() As Integer
        Get
            Return _NumberResults
        End Get
        Set(ByVal value As Integer)
            _NumberResults = value
        End Set
    End Property

    ''' <summary>
    ''' 1/x minimale Schrittweite der Optparas bei Hooke-Jeeves 
    ''' </summary>
    Public Property HJStepsize() As Integer
        Get
            Return _HJStepsize
        End Get
        Set(ByVal value As Integer)
            _HJStepsize = value
        End Set
    End Property

#Region "MySQL"

    Public Property MySQL_Host() As String
        Get
            Return _MySQL_Host
        End Get
        Set(ByVal value As String)
            _MySQL_Host = value
        End Set
    End Property

    Public Property MySQL_Database() As String
        Get
            Return _MySQL_Database
        End Get
        Set(ByVal value As String)
            _MySQL_Database = value
        End Set
    End Property

    Public Property MySQL_User() As String
        Get
            Return _MySQL_User
        End Get
        Set(ByVal value As String)
            _MySQL_User = value
        End Set
    End Property

    Public Property MySQL_Password() As String
        Get
            Return _MySQL_Password
        End Get
        Set(ByVal value As String)
            _MySQL_Password = value
        End Set
    End Property

#End Region 'MySQL

#Region "BindingProperties"

    Public ReadOnly Property OpModeEnabled As Boolean
        Get
            If (Me.Role = "Network Client") Then Return False
            Return True
        End Get
    End Property

    Public ReadOnly Property GlobalOptionsEnabled As Boolean
        Get
            If (Me.OpMode = "Local Optimizer") Then Return False
            If (Me.Role = "Network Client") Then Return False
            Return True
        End Get
    End Property

    Public ReadOnly Property TransferOptionsEnabled As Boolean
        Get
            If (Me.Role = "Network Client") Then Return False
            Return True
        End Get
    End Property

    Public ReadOnly Property LocalOptionsEnabled As Boolean
        Get
            If (Me.OpMode = "Global Optimizer") Then Return False
            If (Me.Role = "Network Client") Then Return False
            Return True
        End Get
    End Property

    Public ReadOnly Property MysqlOptionsEnabled As Boolean
        Get
            If (Me.Role = "Single PC") Then Return False
            Return True
        End Get
    End Property

#End Region

#End Region 'Properties

    'Standardwerte setzen
    '********************
    Public Sub setStandard()

        Me.Role = "Single PC"
        Me.OpMode = "Both"

        Me.PopulationSize = 15
        Me.NumberGenerations = 50
        Me.CurrentGeneration = 0
        Me.NumberResults = 10

        Me.HJStepsize = 50

        Me.MySQL_Host = "localhost"
        Me.MySQL_Database = "metaevo_db"
        Me.MySQL_User = "remoteuser"
        Me.MySQL_Password = ""

        Me.ChildrenPerParent = 3
    End Sub

End Class
