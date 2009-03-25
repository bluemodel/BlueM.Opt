''' <summary>
''' Klasse für die Definition von Objective Funktionen
''' </summary>
Public MustInherit Class Objectivefunktion

Public Enum ObjectiveType As Integer
   Reihe = 1
   Wert = 2
   Reihenwert = 3
   Unknown = 4
End Enum

    ''' <summary>
    ''' Gibt an ob es sich um eine PrimaryObjective Function handelt
    ''' </summary>
    Public isPrimObjective As Boolean

    ''' <summary>
    ''' Bezeichnung
    ''' </summary>
    Public Bezeichnung As String

    ''' <summary>
    ''' Gruppe
    ''' </summary>
    Public Gruppe As String

    ''' <summary>
    ''' Richtung der ObjectiveFunction (d.h. zu maximieren oder zu minimieren)
    ''' </summary>
    Public Richtung As Constants.EVO_RICHTUNG

    ''' <summary>
    ''' Operator bzw Faktor
    ''' </summary>
    Public OpFact As Double

    ''' <summary>
    ''' Gibt an ob es sich um einen Wert oder um eine Reihe handelt
    ''' </summary>
    ''' <remarks>Erlaubte Werte: "Wert" und "Reihe"</remarks>
    Public Typ As String

    ''' <summary>
    ''' Die Dateiendung der Ergebnisdatei, aus der das Simulationsergebnis ausgelesen werden soll
    ''' </summary>
    ''' <remarks>Erlaubte Werte: z.B. "WEL" oder "ASC"</remarks>
    Public Datei As String

    ''' <summary>
    ''' Die Simulationsgröße, auf dessen Basis der Objectivewert berechnet werden soll
    ''' </summary>
    Public SimGr As String

    ''' <summary>
    ''' Name der Funktion, mit der der Objectivewert berechnet werden soll
    ''' </summary>
    ''' <remarks>Erlaubte Werte: "AbQuad", "Diff", "nÜber", "sÜber", "nUnter", "sUnter", "Volf", "IHA". Siehe auch Wiki</remarks>
    Public Funktion As String

    ''' <summary>
    ''' Gibt an, ob die Objective Function einen IstWert besitzt
    ''' </summary>
    Public hasIstWert As Boolean

    ''' <summary>
    ''' Objective Wert im Istzustand
    ''' </summary>
    Public IstWert As Double

    ''' <summary>
    ''' Gibt die Bezeichnung zurück
    ''' </summary>
    ''' <returns>Bezeichnung</returns>
    ''' 
    Public Overrides Function ToString() As String
        Return Me.Bezeichnung
    End Function

    ''' <summary>
    ''' Gibt an ob es ein GruppenLeader ist
    ''' </summary>
    ''' <returns>GroupLeader</returns>

    Public ReadOnly Property isGroupLeader()
        Get
            If Me.Bezeichnung = Me.Gruppe Then
                Return True
            End If
            Return False
        End Get
    End Property

    Public ReadOnly Property isGroupMember()
        Get
            If Me.Gruppe = "" Then
                Return False
            End If
            Return True
        End Get
    End Property

   Public ReadOnly Property GetObjType() As ObjectiveType
      Get
         If (TypeOf (Me) Is ObjectiveFunction_Series) Then
            Return ObjectiveType.Reihe
         ElseIf (TypeOf (Me) Is Objectivefunction_Value) Then
            Return ObjectiveType.Wert
         ElseIf (TypeOf (Me) Is ObjectiveFunction_SeriesValue) Then
            Return ObjectiveType.Reihenwert
         Else
            Return ObjectiveType.Unknown
         End If
      End Get

   End Property
      
   

End Class
