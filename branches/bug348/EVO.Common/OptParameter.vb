Public Class OptParameter

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse OptParameter                                                   ****
    '**** für das Speichern eines Optimierungsparameters                        ****
    '**** und zugehöriger Informationen                                         ****
    '****                                                                       ****
    '**** Autoren:                                                              ****
    '**** Felix Froehlich                                                       ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

#Region "Eigenschaften"

    'Metadaten
    Public Bezeichnung As String
    Public Einheit As String

    'Parameterwerte
    Public Xn As Double                         'Skalierter Parameterwert
    Public Min As Double                        'Minwert für die Umrechnung in reellen Parameterwert
    Public Max As Double                        'Maxwert für die Umrechnung in reellen Parameterwert
    Public Property RWert() As Double           'Reeller Parameterwert
        Get
            Return Me.Min + (Me.Max - Me.Min) * Me.Xn
        End Get
        Set(ByVal value As Double)
            Me.Xn = (value - Me.Min) / (Me.Max - Me.Min)
        End Set
    End Property
    Public StartWert As Double                  'Reeller Startwert

    'Schrittweite
    Public Dn As Double

    'Beziehung
    Public Beziehung As Common.Constants.Beziehung

#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

    'Konstruktor
    '***********
    Public Sub New()

        'Default Werte setzen
        Me.Bezeichnung = "[nicht gesetzt]"
        Me.Einheit = "[-]"
        Me.Min = 0
        Me.Max = 1
        Me.Xn = 0.5
        Me.StartWert = 0.5
        Me.Dn = 0.1
        Me.Beziehung = Constants.Beziehung.keine

    End Sub

    'ToString
    '********
    Public Overrides Function ToString() As String
        Return Me.Bezeichnung
    End Function

    'Duplizieren eines OptParameters
    '*******************************
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

    'Kopiert ein Array von OptParametern
    '*********************************
    Public Shared Sub Clone_OptPara_Array(ByVal Source() As OptParameter, ByRef Dest() As OptParameter)
        Dim i As Integer

        For i = 0 To Source.GetUpperBound(0)
            Dest(i) = Source(i).Clone()
        Next
    End Sub

    'Konvertiert eine Liste von OptParametern in ein Array von Doubles (Xn)
    '**********************************************************************
    Public Shared Function MyParaDouble(ByVal OptParamer() As EVO.Common.OptParameter) As Double()
        Dim i As Integer
        Dim Array(-1) As Double
        ReDim MyParaDouble(-1)

        For i = 0 To OptParamer.GetUpperBound(0)
            ReDim Preserve Array(Array.GetUpperBound(0) + 1)
            Array(i) = OptParamer(i).Xn
        Next
        MyParaDouble = Array.Clone()

    End Function

#End Region 'Methoden

End Class