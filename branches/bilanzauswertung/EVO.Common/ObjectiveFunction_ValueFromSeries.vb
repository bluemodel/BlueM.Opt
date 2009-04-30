Public Class ObjectiveFunction_ValueFromSeries
    Inherits ObjectiveFunction

    ''' <summary>
    ''' Returns the type of the ObjectiveFunction
    ''' </summary>
    Public Overrides ReadOnly Property GetObjType() As ObjectiveType
        Get
            Return ObjectiveType.ValueFromSeries
        End Get
    End Property

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

    ''' <summary>
    ''' Calculate ObjectiveFunction value 
    ''' </summary>
    ''' <param name="SimErgebnis">collection of simulation results</param>
    ''' <returns>objective function value</returns>
    Public Overrides Function calculateObjective(ByVal SimErgebnis As SimErgebnis_Structure) As Double

        Dim SimWert As Double
        Dim SimReihe As Wave.Zeitreihe
        Dim QWert As Double

        'SimReihe aus SimErgebnis rausholen
        SimReihe = SimErgebnis.Reihen(Me.SimGr).Clone()

        'SimReihe auf Evaluierungszeitraum kürzen
        Call SimReihe.Cut(Me.EvalStart, Me.EvalEnde)

        'SimReihe zu SimWert konvertieren
        SimWert = SimReihe.getWert(Me.WertFunktion)

        'Wertevergleich durchführen
        QWert = ObjectiveFunction.compareValues(SimWert, Me.RefWert, Me.Funktion)

        'Zielrichtung berücksichtigen
        QWert *= Me.Richtung

        Return QWert

    End Function

End Class
