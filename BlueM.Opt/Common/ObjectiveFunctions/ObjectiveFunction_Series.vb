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

Public Class ObjectiveFunction_Series
    Inherits ObjectiveFunction

    ''' <summary>
    ''' Returns the type of the ObjectiveFunction
    ''' </summary>
    Public Overrides ReadOnly Property GetObjType() As ObjectiveType
        Get
            Return ObjectiveType.Series
        End Get
    End Property

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
    Public RefReihe As Wave.TimeSeries

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
    Public Overrides Function calculateObjective(ByVal SimErgebnis As SimResults) As Double

        Dim QWert As Double
        Dim SimReihe As Wave.TimeSeries

        'Check
        If Not SimErgebnis.Series.ContainsKey(Me.SimResult) Then
            Throw New Exception($"Unable to find SimResult '{Me.SimResult}' in simulation result! Please check the dataset.")
        End If

        'SimReihe aus SimErgebnis rausholen
        SimReihe = SimErgebnis.Series(Me.SimResult).Clone()

        'Simulationszeitreihe auf Evaluierungszeitraum zuschneiden
        Call SimReihe.Cut(Me.EvalStart, Me.EvalEnde)

        'Reihenvergleich durchführen
        QWert = ObjectiveFunction.compareSeries(SimReihe, Me.RefReihe, Me.Function)

        'Zielrichtung berücksichtigen
        QWert *= Me.Direction

        Return QWert

    End Function

End Class
