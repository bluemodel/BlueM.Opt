Public Class OptParameter

    'Metadaten
    Public Bezeichnung as String
    Public Einheit As String

    'Parameterwerte
    Public Xn As Double                         'Skalierter Parameterwert
    Public Min As Double                        'Minwert für die Umrechnung in reellen Parameterwert
    Public Max as Double                        'Maxwert für die Umrechnung in reellen Parameterwert
    Public Property RWert As Double             'Reeller Parameterwert
        Get
            Return Me.Min + (Me.Max - Me.Min) * Me.Xn
        End Get
        Set (value As Double)
            Me.Xn = (value - Me.Min) / (Me.Max - Me.Min)
        End Set
    End Property
    Public StartWert As Double                  'Reeller Startwert

    'Schrittweite
    Public Dn As Double

    'Beziehung
    Public Beziehung As EVO.Kern.PES.Beziehung

    Public Overrides Function ToString() As String
        Return Me.Bezeichnung
    End Function

    Public Function Clone() As OptParameter
        
        Clone = New OptParameter()

        Clone.Bezeichnung = Me.Bezeichnung
        Clone.Einheit = Me.Einheit
        Clone.Xn = Me.Xn
        Clone.Dn = Me.Dn
        Clone.Min = Me.Min
        Clone.Max = Me.Max
        Clone.StartWert = Me.StartWert
        Clone.Beziehung = Me.Beziehung

        Return Clone

    End Function

    Public Shared Function MyParaDouble(ByVal OptParamer() As EVO.Kern.OptParameter) As Double()
        Dim i As Integer
        Dim Array(-1) As Double
        ReDim MyParaDouble(-1)

        For i = 0 To OptParamer.GetUpperBound(0)
            ReDim Preserve Array(Array.GetUpperBound(0))
            Array(i) = OptParamer(i).Xn
        Next
        MyParaDouble = Array.Clone
    End Function

End Class