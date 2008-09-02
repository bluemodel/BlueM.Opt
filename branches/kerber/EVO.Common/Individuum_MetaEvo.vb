Public Class Individuum_MetaEvo
    Inherits Individuum


    Private Genes() As Double   'Gene des Individuums
    Private Generator As String  'von welchem Algorithmus das Individuum gemacht wurde

    '### Initialisierung
    Public Sub New(ByVal type As String, ByVal id As Integer)

        'Basisindividuum instanzieren
        Call MyBase.New(type, id)



    End Sub
    '### Überschriebene Methoden

    Public Overrides Function Clone() As Individuum
        Dim Dest As New Individuum_MetaEvo(Me.mType, Me.ID)
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
    Public Function set_genes(ByVal genes_input As Double())
        Dim i As Integer

        For i = 0 To Genes.Length - 1
            Me.Genes(i) = genes_input(i)
        Next

        Return True
    End Function

    'Gene zurückgeben
    Public Function get_genes() As Double()
        Return Me.Genes
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

End Class
