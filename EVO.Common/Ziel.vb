Public Class Ziel

    Public isOpt As Boolean                     'Gibt an ob es sich um ein OptZiel oder ein SekZiel handelt
    Public Bezeichnung As String                'Bezeichnung
    Public Richtung As Constants.EVO_RICHTUNG   'Richtung der Zielfunktion   
    Public ZielTyp As String                    'Gibt an ob es sich um einen Wert oder um eine Reihe handelt
    Public Datei As String                      'Die Ergebnisdatei, aus der das Simulationsergebnis ausgelesen werden soll [WEL, BIL, PRB]
    Public SimGr As String                      'Die Simulationsgröße, auf dessen Basis der Qualitätswert berechnet werden soll
    Public ZielFkt As String                    'Zielfunktion
    Public WertTyp As String                    'Gibt an wie der Wert, der mit dem Referenzwert verglichen werden soll, aus dem Simulationsergebnis berechnet werden soll.
    Public RefWert As Double                    'Der zu vergleichende Referenzwert
    Public RefReiheDatei As String              'Der Dateiname der Referenzreihe
    Public RefGr As String                      'Spalte der .wel Datei falls Referenzreihe .wel Datei ist
    Public RefReihe As Wave.Zeitreihe           'Die Referenzreihe
    Public EvalStart As DateTime                'Start des Evaluierungszeitraums
    Public EvalEnde As DateTime                 'Ende des Evaluierungszeitraums

    Public Overrides Function ToString() As String
        Return Bezeichnung
    End Function

End Class
