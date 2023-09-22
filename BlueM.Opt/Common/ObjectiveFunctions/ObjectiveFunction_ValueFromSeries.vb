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
    ''' Start of the evaluation period
    ''' </summary>
    Public EvalStart As DateTime

    ''' <summary>
    ''' End of the evaluation period
    ''' </summary>
    Public EvalEnd As DateTime

    ''' <summary>
    ''' Reference value
    ''' </summary>
    Public RefValue As Double

    ''' <summary>
    ''' Function with which to calculate the simulation value from the simulation time series
    ''' </summary>
    Public ValueFunction As String

    ''' <summary>
    ''' Calculate ObjectiveFunction value 
    ''' </summary>
    ''' <param name="SimResult">simulation result</param>
    ''' <returns>objective function value</returns>
    Public Overrides Function calculateObjective(ByVal SimResult As SimResults) As Double

        Dim SimValue As Double
        Dim SimSeries As Wave.TimeSeries
        Dim objectiveValue As Double

        'Check
        If Not SimResult.Series.ContainsKey(Me.SimResultName) Then
            Throw New Exception($"Unable to find '{Me.SimResultName}' in simulation result! Please check the dataset.")
        End If

        'SimReihe aus SimErgebnis rausholen
        SimSeries = SimResult.Series(Me.SimResultName).Clone()

        'SimReihe auf Evaluierungszeitraum kürzen
        Call SimSeries.Cut(Me.EvalStart, Me.EvalEnd)

        'Calculate SimValue from SimSeries
        Select Case Me.ValueFunction.ToUpper()
            Case "MAX", "MAXWERT"
                SimValue = SimSeries.Maximum
            Case "MIN", "MINWERT"
                SimValue = SimSeries.Minimum
            Case "AVG", "AVERAGE"
                SimValue = SimSeries.Average
            Case "START", "ANFWERT"
                SimValue = SimSeries.Values.First
            Case "END", "ENDWERT"
                SimValue = SimSeries.Values.Last
            Case "SUM", "SUMME"
                SimValue = SimSeries.Sum
            Case Else
                Throw New Exception($"Unknown value type '{Me.ValueFunction}' for objective function {Me.Description}!")
        End Select

        'Wertevergleich durchführen
        objectiveValue = ObjectiveFunction.compareValues(SimValue, Me.RefValue, Me.Function)

        'Zielrichtung berücksichtigen
        objectiveValue *= Me.Direction

        Return objectiveValue

    End Function

End Class
