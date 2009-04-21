Public Class Objectivefunction_Value
    Inherits ObjectiveFunction

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

    ''' <summary>
    ''' Calculate ObjectiveFunction value
    ''' </summary>
    ''' <param name="SimErgebnis">collection of simulation results</param>
    ''' <returns>objective function value</returns>
    Public Overrides Function calculateObjective(ByVal SimErgebnis As Collection) As Double

        Dim SimWert As Double
        Dim QWert As Double

        'SimWert aus SimErgebnis rausholen
        SimWert = SimErgebnis(Me.SimGr)

        'Wertevergleich ausführen
        QWert = ObjectiveFunction.compareValues(SimWert, Me.RefWert, Me.Funktion)

        'Zielrichtung berücksichtigen
        QWert *= Me.Richtung

        Return QWert

    End Function

End Class
