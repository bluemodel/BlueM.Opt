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
    Public WertFunktion As String

    ''' <summary>
    ''' Calculate ObjectiveFunction value 
    ''' </summary>
    ''' <param name="SimErgebnis">collection of simulation results</param>
    ''' <returns>objective function value</returns>
    Public Overrides Function calculateObjective(ByVal SimErgebnis As SimResults) As Double

        Dim SimWert As Double
        Dim SimReihe As Wave.TimeSeries
        Dim QWert As Double

        'Check
        If Not SimErgebnis.Series.ContainsKey(Me.SimResult) Then
            Throw New Exception($"Unable to find '{Me.SimResult}' in simulation result! Please check the dataset.")
        End If

        'SimReihe aus SimErgebnis rausholen
        SimReihe = SimErgebnis.Series(Me.SimResult).Clone()

        'SimReihe auf Evaluierungszeitraum kürzen
        Call SimReihe.Cut(Me.EvalStart, Me.EvalEnde)

        'Calculate SimValue from SimSeries
        Select Case Me.WertFunktion.ToUpper()
            Case "MAX", "MAXWERT"
                SimWert = SimReihe.Maximum
            Case "MIN", "MINWERT"
                SimWert = SimReihe.Minimum
            Case "AVG", "AVERAGE"
                SimWert = SimReihe.Average
            Case "START", "ANFWERT"
                SimWert = SimReihe.Values.First
            Case "END", "ENDWERT"
                SimWert = SimReihe.Values.Last
            Case "SUM", "SUMME"
                SimWert = SimReihe.Sum
            Case Else
                Throw New Exception($"Unknown value type '{Me.WertFunktion}' for objective function {Me.Description}!")
        End Select

        'Wertevergleich durchführen
        QWert = ObjectiveFunction.compareValues(SimWert, Me.RefWert, Me.Function)

        'Zielrichtung berücksichtigen
        QWert *= Me.Direction

        Return QWert

    End Function

End Class
