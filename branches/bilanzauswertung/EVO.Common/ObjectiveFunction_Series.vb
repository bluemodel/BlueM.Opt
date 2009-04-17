Public Class ObjectiveFunction_Series
    Inherits ObjectiveFunction


    ''' <summary>
    ''' Der Dateiname der Referenzreihe
    ''' </summary>
    ''' <remarks>Nur bei Typ = "Reihe". Pfadangabe relativ zum Datensatz</remarks>
    Public RefReiheDatei As String

    ''' <summary>
    ''' Zu verwendender Spaltenname falls Referenzreihe eine .WEL Datei ist
    ''' </summary>
    ''' <remarks>Nur bei Typ = "Reihe"</remarks>
    Public RefGr As String

    ''' <summary>
    ''' Die Referenzreihe
    ''' </summary>
    ''' <remarks>Nur bei Typ = "Reihe"</remarks>
    Public RefReihe As Wave.Zeitreihe

    ''' <summary>
    ''' Start des Evaluierungszeitraums
    ''' </summary>
    Public EvalStart As DateTime

    ''' <summary>
    ''' Ende des Evaluierungszeitraums
    ''' </summary>
    Public EvalEnde As DateTime

End Class
