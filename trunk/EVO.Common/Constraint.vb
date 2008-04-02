Public Class Constraint

    'Constraints
    '-----------
    Public Bezeichnung As String                'Bezeichnung
    Public GrenzTyp As String                   'Gibt an ob es sich um einen Wert oder um eine Reihe handelt
    Public Datei As String                      'Die Ergebnisdatei, aus der das Simulationsergebnis ausgelesen werden soll [WEL]
    Public SimGr As String                      'Die Simulationsgröße, die auf Verletzung der Grenze überprüft werden soll
    Public GrenzPos As String                   'Grenzposition (Ober-/Untergrenze)
    Public WertTyp As String                    'Gibt an wie der Wert, der mit dem Grenzwert verglichen werden soll, aus dem Simulationsergebnis berechnet werden soll
    Public GrenzWert As Double                  'Der vorgegeben Grenzwert
    Public GrenzReiheDatei As String            'Der Dateiname der Grenzwertreihe
    Public GrenzGr As String                    'Spalte der .wel Datei falls Grenzwertreihe .wel Datei ist
    Public GrenzReihe As Wave.Zeitreihe         'Die Werte der Grenzwertreihe

    Public Overrides Function ToString() As String
        Return Bezeichnung
    End Function

End Class
