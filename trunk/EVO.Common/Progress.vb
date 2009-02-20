''' <summary>
''' Klasse speichert den Optimierungsverlauf ab 
''' und löst bei Änderungen Events aus
''' </summary>
''' <remarks>
''' Konvention: Alle Zähler fangen bei 0 an 
''' und sollten immer erst nach Abschluss 
''' des jeweiligen Schrittes hochgezählt werden.
''' </remarks>
Public Class Progress

#Region "Events"

    ''' <summary>
    ''' Es wurde eine Neu-Initialisierung durchgeführt
    ''' </summary>
    Public Event Initialized()

    ''' <summary>
    ''' Der Rundenzähler wurde aktualisiert
    ''' </summary>
    Public Event iRundeChanged()

    ''' <summary>
    ''' Der Populationszähler wurde aktualisiert
    ''' </summary>
    Public Event iPopulChanged()

    ''' <summary>
    ''' Der Generationenzähler wurde aktualisiert
    ''' </summary>
    Public Event iGenChanged()

    ''' <summary>
    ''' Der Nachfahrenzähler wurde aktualisiert
    ''' </summary>
    Public Event iNachfChanged()

#End Region 'Events

#Region "Eigenschaften"

    Private m_NRunden As Integer
    Private m_NPopul As Integer
    Private m_NGen As Integer
    Private m_NNachf As Integer

    Private m_iRunde As Integer
    Private m_iPopul As Integer
    Private m_iGen As Integer
    Private m_iNachf As Integer

#End Region 'Eigenschaften

#Region "Properties"

    ''' <summary>
    ''' aktuelle Runde
    ''' </summary>
    ''' <remarks>0-basiert</remarks>
    Public Property iRunde() As Integer
        Get
            Return m_iRunde
        End Get
        Set(ByVal value As Integer)
            'MaxWert abfangen
            value = Math.Min(value, Me.NRunden - 1)
            m_iRunde = value
            RaiseEvent iRundeChanged()
        End Set
    End Property

    ''' <summary>
    ''' aktuelle Population
    ''' </summary>
    ''' <remarks>0-basiert</remarks>
    Public Property iPopul() As Integer
        Get
            Return m_iPopul
        End Get
        Set(ByVal value As Integer)
            'MaxWert abfangen
            value = Math.Min(value, Me.NPopul - 1)
            m_iPopul = value
            RaiseEvent iPopulChanged()
        End Set
    End Property

    ''' <summary>
    ''' aktuelle Generation
    ''' </summary>
    ''' <remarks>0-basiert</remarks>
    Public Property iGen() As Integer
        Get
            Return m_iGen
        End Get
        Set(ByVal value As Integer)
            'MaxWert abfangen
            value = Math.Min(value, Me.NGen - 1)
            m_iGen = value
            RaiseEvent iGenChanged()
        End Set
    End Property

    ''' <summary>
    ''' aktueller Nachfahre
    ''' </summary>
    ''' <remarks>0-basiert</remarks>
    Public Property iNachf() As Integer
        Get
            Return m_iNachf
        End Get
        Set(ByVal value As Integer)
            'MaxWert abfangen
            value = Math.Min(value, Me.NNachf - 1)
            m_iNachf = value
            RaiseEvent iNachfChanged()
        End Set
    End Property

    ''' <summary>
    ''' Gesamtanzahl Runden
    ''' </summary>
    Public ReadOnly Property NRunden() As Integer
        Get
            Return m_NRunden
        End Get
    End Property

    ''' <summary>
    ''' Gesamtanzahl Populationen
    ''' </summary>
    Public ReadOnly Property NPopul() As Integer
        Get
            Return m_NPopul
        End Get
    End Property

    ''' <summary>
    ''' Gesamtanzahl Generationen
    ''' </summary>
    Public ReadOnly Property NGen() As Integer
        Get
            Return m_NGen
        End Get
    End Property

    ''' <summary>
    ''' Gesamtanzahl Nachfahren
    ''' </summary>
    Public ReadOnly Property NNachf() As Integer
        Get
            Return m_NNachf
        End Get
    End Property

#End Region 'Properties

#Region "Methoden"

    ''' <summary>
    ''' Konstruktor
    ''' </summary>
    Public Sub New()
        Call Me.Initialize()
    End Sub

    ''' <summary>
    ''' Initialisiert den Verlauf neu mit allen Werten = 0
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub Initialize()

        Call Me.Initialize(0, 0, 0, 0)

    End Sub

    ''' <summary>
    ''' Initialisiert den Verlauf neu
    ''' </summary>
    ''' <param name="NRunden">Gesamtanzahl Runden</param>
    ''' <param name="NPopul">Gesamtanzahl Populationen (pro Runde)</param>
    ''' <param name="NGen">Gesamtanzahl Generationen (pro Population)</param>
    ''' <param name="NNachf">Gesamtanzahl Nachfahren (pro Generation)</param>
    ''' <remarks>alle Zähler werden auf 0 gesetzt</remarks>
    Public Overloads Sub Initialize(ByVal NRunden As Integer, ByVal NPopul As Integer, ByVal NGen As Integer, ByVal NNachf As Integer)

        Me.m_NRunden = NRunden
        Me.m_NPopul = NPopul
        Me.m_NGen = NGen
        Me.m_NNachf = NNachf

        Me.m_iRunde = 0
        Me.m_iPopul = 0
        Me.m_iGen = 0
        Me.m_iNachf = 0

        RaiseEvent Initialized()

    End Sub

    ''' <summary>
    ''' Aktuelle Runde um 1 hochzählen
    ''' </summary>
    Public Sub NextRunde()
        Me.iRunde += 1
    End Sub

    ''' <summary>
    ''' Aktuelle Population um 1 hochzählen
    ''' </summary>
    Public Sub NextPopul()
        Me.iPopul += 1
    End Sub

    ''' <summary>
    ''' Aktuelle Generation um 1 hochzählen
    ''' </summary>
    Public Sub NextGen()
        Me.iGen += 1
    End Sub

    ''' <summary>
    ''' Aktuellen Nachfahre um 1 hochzählen
    ''' </summary>
    Public Sub NextNachf()
        Me.iNachf += 1
    End Sub

#End Region 'Methoden

End Class
