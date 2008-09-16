Public Class Individuum_MetaEvo
    Inherits Individuum

    Private generator As String             'von welchem Algorithmus das Individuum gemacht wurde
    Private Client As String                'Welcher Rechner dieses Individuum berechnen soll [ip oder Rechnername]
    Private numberOptparas As Integer       'Anzahl der Optoaras des Problems
    Private status As String                '{raw, calculate, ready, false}
    Private basisIndividuum As Individuum

    '### Initialisierung
    Public Sub New(ByVal type As String, ByVal id As Integer, ByVal numberOptparas_input As Integer)

        'Basisindividuum instanzieren
        Call MyBase.New(type, id)

        numberOptparas = numberOptparas_input

    End Sub
    '### Überschriebene Methoden

    Public Overrides Function Clone() As Individuum
        Dim Dest As New Individuum_MetaEvo(Me.mType, Me.ID, Me.numberOptparas)
        Return Dest
    End Function

    Public Overrides Function Create(Optional ByVal type As String = "tmp", Optional ByVal id As Integer = 0) As Individuum
        Dim ind As New Individuum_CES(type, id)
        Return ind
    End Function

    Public Overrides Property OptParameter() As OptParameter()
        Get

        End Get
        Set(ByVal value As OptParameter())
           
        End Set
    End Property

    '### Neue Methoden
    'Gene setzen
    Public Function set_optparas(ByVal optparas_input As Double())
        Me.OptParameter_RWerte = optparas_input
        Return True
    End Function

    'Gene zurückgeben
    Public Function get_optparas() As Double()
        Return Me.OptParameter_RWerte
    End Function

    'Generator setzen
    Public Function set_generator(ByVal generator_input As String)
        Me.generator = generator_input
        Return True
    End Function

    'Generator auslesen
    Public Function get_generator() As String
        Return Me.generator
    End Function

    'IPWorker setzen
    Public Function set_Client(ByVal client_input As String)
        Me.Client = client_input
        Return True
    End Function

    'IPWorker auslesen
    Public Function get_client() As String
        Return Me.Client
    End Function

    'Status setzen
    Public Function set_status(ByVal status_input As String)
        Me.status = status_input
        Return True
    End Function

    'Status auslesen
    Public Function get_status() As String
        Return Me.status
    End Function

End Class
