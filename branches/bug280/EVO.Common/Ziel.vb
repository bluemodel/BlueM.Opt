Imports System.Xml.Serialization

'Klasse Ziel
'###########
Public Class Ziel

    <XmlAttribute()> _
    Public isOpt As Boolean                     'Gibt an ob es sich um ein OptZiel oder ein SekZiel handelt

    Private _ZielTyp As String
    <XmlAttribute()> _
    Public Property ZielTyp() As String         'Gibt an ob es sich um einen Wert oder um eine Reihe handelt
        Get
            Return _ZielTyp
        End Get
        Set(ByVal value As String)
            _ZielTyp = value
            If (Me._ZielTyp = "Wert") Then
                Me.ZielWert = New ZielWert()
                Me.ZielReihe = Nothing
            Else
                Me.ZielReihe = New Reihe()
                Me.ZielWert = Nothing
            End If
        End Set
    End Property

    Public Bezeichnung As String                'Bezeichnung

    Public ZielFkt As String                    'Zielfunktion

    Public SimReihe As Reihe                    'Zu evaluierende Simulationsreihe

    Public Structure Struct_EvalZeitraum
        Public Start As DateTime                'Start des Evaluierungszeitraums
        Public Ende As DateTime                 'Ende des Evaluierungszeitraums
    End Structure

    <XmlElement("Evaluierungszeitraum")> _
    Public Eval As Struct_EvalZeitraum

    Public ZielWert As ZielWert

    Public ZielReihe As Reihe

    'Konstruktor
    '***********
    Public Sub New()
        Me.SimReihe = New Reihe()
    End Sub

    Public Overrides Function ToString() As String
        Return Bezeichnung
    End Function

End Class

'Klasse Zielwert
'###############
Public Class ZielWert
    <XmlAttribute()> _
    Public WertTyp As String                        'Gibt an wie der Wert, der mit dem Zielwert verglichen werden soll, aus dem Simulationsergebnis berechnet werden soll.
    <XmlText()> _
    Public ZielWert As Double                       'Der vorgegeben Zielwert
End Class

'Klasse Reihe
'############
<XmlInclude(GetType(Wave.Zeitreihe))> _
Public Class Reihe

    Public Structure Struct_ReihenInfo
        <XmlText()> _
        Public Pfad As String                       'Pfad zur Datei
        <XmlAttribute()> _
        Public Spalte As String                     'ggf. Spalte
    End Structure

    Public Datei As Struct_ReihenInfo
    <XmlIgnore()> _
    Public ZRE As Wave.Zeitreihe                    'Die Zeitreihe
End Class



