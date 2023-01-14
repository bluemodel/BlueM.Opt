'BlueM.Opt
'Copyright (C) BlueM Dev Group
'Website: <https://www.bluemodel.org>
'
'This program is free software: you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.
'
'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
'GNU General Public License for more details.
'
'You should have received a copy of the GNU General Public License
'along with this program. If not, see <https://www.gnu.org/licenses/>.
'
Imports BlueM

''' <summary>
''' Klasse für die Definition von Constraint Funktionen
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
    ''' Die Simulationsgröße, die auf Verletzung der Grenze überprüft werden soll
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
    Public GrenzReihe As Wave.TimeSeries

    ''' <summary>
    ''' Gibt die Bezeichnung zurück
    ''' </summary>
    ''' <returns>Bezeichnung</returns>
    Public Overrides Function ToString() As String
        Return Bezeichnung
    End Function

End Class
