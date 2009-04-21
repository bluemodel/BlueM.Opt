Public Class ObjectiveFunction_Series
    Inherits ObjectiveFunction

    ''' <summary>
    ''' Der Dateiname der Referenzreihe
    ''' </summary>
    ''' <remarks>Pfadangabe relativ zum Datensatz</remarks>
    Public RefReiheDatei As String

    ''' <summary>
    ''' Zu verwendender Spaltenname falls Referenzreihe eine .WEL Datei ist
    ''' </summary>
    Public RefGr As String

    ''' <summary>
    ''' Die Referenzreihe
    ''' </summary>
    Public RefReihe As Wave.Zeitreihe

    ''' <summary>
    ''' Start des Evaluierungszeitraums
    ''' </summary>
    Public EvalStart As DateTime

    ''' <summary>
    ''' Ende des Evaluierungszeitraums
    ''' </summary>
    Public EvalEnde As DateTime

    ''' <summary>
    ''' Calculate the ObjectiveFunction value
    ''' </summary>
    ''' <param name="SimErgebnis">collection of simulation results</param>
    ''' <returns>objective function value</returns>
    Public Overrides Function calculateObjective(ByVal SimErgebnis As Collection) As Double

        Dim QWert As Double
        Dim SimReihe As Wave.Zeitreihe

        'SimReihe aus SimErgebnis rausholen
        SimReihe = SimErgebnis(Me.SimGr).Clone()

        'Simulationszeitreihe auf Evaluierungszeitraum zuschneiden
        Call SimReihe.Cut(Me.EvalStart, Me.EvalEnde)

        'Reihenvergleich durchführen
        QWert = ObjectiveFunction.compareSeries(SimReihe, Me.RefReihe, Me.Funktion)

        'Zielrichtung berücksichtigen
        QWert *= Me.Richtung

        Return QWert

    End Function

End Class
