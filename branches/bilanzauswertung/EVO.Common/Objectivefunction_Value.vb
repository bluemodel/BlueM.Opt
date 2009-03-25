Public Class Objectivefunction_Value
Inherits Objectivefunktion

    ''' <summary>
    ''' Der zu vergleichende Referenzwert
    ''' </summary>
    ''' <remarks>siehe Wiki</remarks>
    Public RefWert As Double

    

    ''' <summary>
    ''' Gibt den Block an, in der die Zielfunktionswerte stehen
    ''' </summary>
    ''' <remarks>Erlaubte Werte abhängig von Application: siehe Wiki</remarks>
    Public Block As String

    ''' <summary>
    ''' Gibt die Spalte im Block an, in dem díe Zielfunktionswerte stehen
    ''' </summary>
    ''' <remarks>Erlaubte Werte abhängig von Application: siehe Wiki</remarks>
    Public Spalte As String

End Class
