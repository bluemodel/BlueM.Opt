Public Class Individuum_MetaEvo
    Inherits Individuum

    Private generator_id As Integer         'von welchem Algorithmus das Individuum gemacht wurde
    Private Client As String                'Welcher Rechner dieses Individuum berechnen soll [ip oder Rechnername]
    Private numberOptparas As Integer       'Anzahl der Optparas des Problems
    Private status As String                '{raw, calculate, true, false}
    Private statusreason As String          '{false: dominated, crowding}
    Private statusopponent As Integer        'Individuen-ID durch den dieses Individuum gelöscht wurde
    Private mOptparameter() As OptParameter
    Public feedbackdata(,) As Double        'Spezifischer Feedback für einen Algorithmus
    Private toSimulate As Boolean           'Ob dieses Individuum simuliert werden soll

    '### Initialisierung
    Public Sub New(ByVal type As String, ByVal id As Integer, ByVal numberOptparas_input As Integer)


        'Basisindividuum instanzieren
        Call MyBase.New(type, id)

        Dim i As Integer
        ReDim Me.mOptparameter(numberOptparas_input)
        numberOptparas = numberOptparas_input
        Me.status = "false"
        Me.toSimulate = True

        'Initialisieren der Optparameter
        For i = 0 To numberOptparas - 1
            Me.mOptparameter(i) = New OptParameter()
            Me.mOptParameter(i).Min = Individuum.mProblem.List_OptParameter(i).Min
            Me.mOptParameter(i).Max = Individuum.mProblem.List_OptParameter(i).Max
            Me.mOptParameter(i).Bezeichnung = Individuum.mProblem.List_OptParameter(i).Bezeichnung
        Next

        'Constraints mit Teilmenge Penalties auf 0 setzen
        For i = 0 To MyBase.Constraints.Length - 1
            Me.Constraints(i) = 0
        Next

        'Features auf 0 setzen
        For i = 0 To MyBase.Features.Length - 1
            Me.Features(i) = 0
        Next

    End Sub
    '### Überschriebene Methoden

    Public Overrides Function Clone() As Individuum
        Dim Dest As New Individuum_MetaEvo(Me.mType, Me.ID, Me.numberOptparas)
        Return Dest
    End Function

    Public Function Clone_MetaEvo() As Individuum_MetaEvo
        Dim Dest As New Individuum_MetaEvo(Me.mType, Me.ID, Me.numberOptparas)
        Dim i As Integer

        Dest.Client = Me.Client
        Dest.status = Me.status
        Dest.statusreason = Me.statusreason
        Dest.statusopponent = Me.statusopponent
        For i = 0 To numberOptparas - 1
            Dest.mOptparameter = Me.mOptparameter
        Next

        For i = 0 To mFeatures.Length - 1
            Dest.mFeatures = Me.mFeatures
        Next

        For i = 0 To mConstraints.Length - 1
            Dest.mConstraints = Me.mConstraints
        Next

        Dest.feedbackdata = Me.feedbackdata
        Dest.toSimulate = False

        Return Dest
    End Function

    Public Overrides Function Create(Optional ByVal type As String = "tmp", Optional ByVal id As Integer = 0) As Individuum
        Dim ind As New Individuum_CES(type, id)
        Return ind
    End Function

    Public Overrides Property OptParameter() As OptParameter()
        Get
            Return Me.mOptparameter
        End Get
        Set(ByVal value As OptParameter())
            Me.mOptparameter = value
            Me.toSimulate = True
        End Set
    End Property

    '### Neue Methoden
    'Gene setzen
    Public Function set_optparas(ByVal optparas_input As Double())
        Me.OptParameter_RWerte = optparas_input
        Me.toSimulate = True
        Return True
    End Function

    'Gene zurückgeben
    Public Function get_optparas() As Double()
        Return Me.OptParameter_RWerte
    End Function

    'Optparameter nach Definition zurückgaben
    Public Function get_mOptparas() As OptParameter()
        Return Me.mOptparameter
    End Function

    'Generator setzen
    Public Function set_generator(ByVal generator_input As Integer)
        Me.generator_id = generator_input
        Return True
    End Function

    'Generator auslesen
    Public Function get_generator() As Integer
        Return Me.generator_id
    End Function

    'Client setzen
    Public Function set_Client(ByVal client_input As String)
        Me.Client = client_input
        Return True
    End Function

    'Client auslesen
    Public Function get_client() As String
        Return Me.Client
    End Function

    'Status setzen
    Public Function set_status(ByVal status_input As String)
        If (status_input.Contains("false")) Then
            'Dim stringarray As String(3)
            Me.status = "false"
            Me.statusreason = status_input.Split("#")(1)
            Try
                Me.statusopponent = CInt(status_input.Split("#")(2))
            Catch ex As Exception

            End Try
            Return True
        Else
            Me.status = status_input
            Me.statusreason = ""
            Me.statusopponent = -1
            Return True
        End If
    End Function

    'Status auslesen
    Public Function get_status() As String
        Return Me.status
    End Function

    Public Function get_status_reason() As String
        Return Me.statusreason
    End Function

    Public Function get_status_opponent() As Integer
        Return Me.statusopponent
    End Function

    'Simulieren?
    Public Function set_toSimulate(ByVal toSimulate_input As Boolean)
        Me.toSimulate = toSimulate_input
        Return True
    End Function

    Public Function get_toSimulate() As Boolean
        Return Me.toSimulate
    End Function
End Class
