Public Class Individuum_MetaEvo
    Inherits Individuum


    Private optparas() As Double   'Gene/Optimierungsparameter des Individuums
    Private Generator As String  'von welchem Algorithmus das Individuum gemacht wurde
    Private Client As String  'Welcher Rechner dieses Individuum berechnen soll [ip oder Rechnername]
    Private numberOptparas As Integer 'Anzahl der Optoaras des Problems

    '### Initialisierung
    Public Sub New(ByVal type As String, ByVal id As Integer, ByVal numberOptparas_input As Integer)

        'Basisindividuum instanzieren
        Call MyBase.New(type, id)

        numberOptparas = numberOptparas_input
        ReDim optparas(numberOptparas - 1)

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
        Dim i As Integer

        For i = 0 To optparas_input.Length - 1
            Me.optparas(i) = optparas_input(i)
        Next

        Return True
    End Function

    'Gene zurückgeben
    Public Function get_optparas() As Double()
        Return Me.optparas
    End Function

    'Generator setzen
    Public Function set_generator(ByVal generator_input As String)
        Me.Generator = generator_input
        Return True
    End Function

    'Generator auslesen
    Public Function get_generator() As String
        Return Me.Generator
    End Function

    'IPWorker setzen
    Public Function set_Client(ByVal Client_input As String)
        Me.Client = Client_input
        Return True
    End Function

    'IPWorker auslesen
    Public Function get_Client() As String
        Return Me.Client
    End Function

End Class
