Public Class ObjectiveFunction_SeriesValue
Inherits Objectivefunktion

 ''' <summary>
    ''' Start des Evaluierungszeitraums
    ''' </summary>
    Public EvalStart As DateTime

    ''' <summary>
    ''' Ende des Evaluierungszeitraums
    ''' </summary>
    Public EvalEnde As DateTime

    ''' <summary>
    ''' Der zu vergleichende Referenzwert
    ''' </summary>
    ''' <remarks>siehe Wiki</remarks>
    Public RefWert As Double

    ''' <summary>
    ''' Gibt an wie der Wert, der mit dem Referenzwert verglichen werden soll, aus dem Simulationsergebnis berechnet werden soll.
    ''' </summary>
    ''' <remarks>Nur bei Typ = "Wert". Erlaubte Werte: "MaxWert", "MinWert", "Average", "AnfWert", "EndWert". Siehe auch Wiki</remarks>
    Public WertFunktion As String

End Class
