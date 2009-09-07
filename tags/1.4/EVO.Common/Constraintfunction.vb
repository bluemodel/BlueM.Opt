''' <summary>
''' Klasse f�r die Definition von Constraint Funktionen
''' </summary>
Public Class Constraintfunction

    ''' <summary>
    ''' Bezeichnung
    ''' </summary>
    Public Bezeichnung As String

    ''' <summary>
    ''' Gibt an ob es sich um einen Wert oder um eine Reihe handelt
    ''' </summary>
    ''' <remarks>Erlaubte Werte: "Wert" und "Reihe"</remarks>
    Public Typ As String                   '

    ''' <summary>
    ''' Die Dateiendung der Ergebnisdatei, aus der das Simulationsergebnis ausgelesen werden soll
    ''' </summary>
    ''' <remarks>z.B. "WEL"</remarks>
    Public Datei As String

    ''' <summary>
    ''' Die Simulationsgr��e, die auf Verletzung der Grenze �berpr�ft werden soll
    ''' </summary>
    Public SimGr As String                      '

    ''' <summary>
    ''' Grenzposition (Ober-/Untergrenze)
    ''' </summary>
    ''' <remarks>Erlaubte Werte: "Obergrenze" und "Untergrenze"</remarks>
    Public GrenzPos As String

    ''' <summary>
    ''' Gibt an wie der Wert, der mit dem Grenzwert verglichen werden soll, aus dem Simulationsergebnis berechnet werden soll
    ''' </summary>
    ''' <remarks>Nur bei Typ = "Wert". Erlaubte Werte: "MaxWert", "MinWert", "Average", "AnfWert", "EndWert". Siehe auch Wiki</remarks>
    Public WertFunktion As String

    ''' <summary>
    ''' Der vorgegebene Grenzwert
    ''' </summary>
    ''' <remarks>Nur bei Typ = "Wert"</remarks>
    Public GrenzWert As Double

    ''' <summary>
    ''' Der Dateiname der Grenzwertreihe
    ''' </summary>
    ''' <remarks>Nur bei Typ = "Reihe". Pfadangabe relativ zum Datensatz</remarks>
    Public GrenzReiheDatei As String

    ''' <summary>
    ''' Zu verwendender Spaltenname falls Grenzwertreihe eine .WEL Datei ist
    ''' </summary>
    ''' <remarks>Nur bei Typ = "Reihe"</remarks>
    Public GrenzGr As String

    ''' <summary>
    ''' Die Grenzwertreihe
    ''' </summary>
    ''' <remarks>Nur bei Typ = "Reihe"</remarks>
    Public GrenzReihe As Wave.Zeitreihe

    ''' <summary>
    ''' Gibt die Bezeichnung zur�ck
    ''' </summary>
    ''' <returns>Bezeichnung</returns>
    Public Overrides Function ToString() As String
        Return Bezeichnung
    End Function

End Class
