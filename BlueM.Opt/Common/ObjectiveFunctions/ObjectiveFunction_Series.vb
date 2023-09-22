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
    ''' Path to the file containing the reference series
    ''' </summary>
    ''' <remarks>can be relative to the dataset</remarks>
    Public RefSeriesFile As String

    ''' <summary>
    ''' The name of the reference time series. Only necessary if RefSeriesFile contains multiple series
    ''' </summary>
    Public RefName As String

    ''' <summary>
    ''' Reference time series
    ''' </summary>
    Public RefSeries As Wave.TimeSeries

    ''' <summary>
    ''' Start of the evaluation period
    ''' </summary>
    Public EvalStart As DateTime

    ''' <summary>
    ''' End of the evaluation period
    ''' </summary>
    Public EvalEnde As DateTime

    ''' <summary>
    ''' Calculate the ObjectiveFunction value
    ''' </summary>
    ''' <param name="SimResult">simulation result</param>
    ''' <returns>objective function value</returns>
    Public Overrides Function calculateObjective(ByVal SimResult As SimResults) As Double

        Dim objectiveValue As Double
        Dim SimSeries As Wave.TimeSeries

        'Check
        If Not SimResult.Series.ContainsKey(Me.SimResultName) Then
            Throw New Exception($"Unable to find SimResult '{Me.SimResultName}' in simulation result! Please check the dataset.")
        End If

        'SimReihe aus SimErgebnis rausholen
        SimSeries = SimResult.Series(Me.SimResultName).Clone()

        'Simulationszeitreihe auf Evaluierungszeitraum zuschneiden
        Call SimSeries.Cut(Me.EvalStart, Me.EvalEnde)

        'Reihenvergleich durchführen
        objectiveValue = ObjectiveFunction.compareSeries(SimSeries, Me.RefSeries, Me.Function)

        'Zielrichtung berücksichtigen
        objectiveValue *= Me.Direction

        Return objectiveValue

    End Function

End Class
