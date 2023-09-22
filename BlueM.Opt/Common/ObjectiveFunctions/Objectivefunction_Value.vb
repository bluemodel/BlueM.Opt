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
Public Class Objectivefunction_Value
    Inherits ObjectiveFunction

    ''' <summary>
    ''' Returns the type of the ObjectiveFunction
    ''' </summary>
    Public Overrides ReadOnly Property GetObjType() As ObjectiveType
        Get
            Return ObjectiveType.Value
        End Get
    End Property

    ''' <summary>
    ''' Der zu vergleichende Referenzwert
    ''' </summary>
    ''' <remarks>siehe Wiki</remarks>
    Public RefWert As Double

    ''' <summary>
    ''' Gibt den Block an, in der die Zielfunktionswerte stehen
    ''' </summary>
    ''' <remarks>Erlaubte Werte abhängig von Application: siehe Wiki</remarks>
    Public Block As String

    ''' <summary>
    ''' Gibt die Spalte im Block an, in dem díe Zielfunktionswerte stehen
    ''' </summary>
    ''' <remarks>Erlaubte Werte abhängig von Application: siehe Wiki</remarks>
    Public Spalte As String

    ''' <summary>
    ''' Calculate ObjectiveFunction value
    ''' </summary>
    ''' <param name="SimErgebnis">collection of simulation results</param>
    ''' <returns>objective function value</returns>
    Public Overrides Function calculateObjective(ByVal SimErgebnis As SimResults) As Double

        Dim SimWert As Double
        Dim QWert As Double

        'SimWert aus SimErgebnis rausholen
        SimWert = SimErgebnis.Values(Me.Bezeichnung)

        'Wertevergleich ausführen
        QWert = ObjectiveFunction.compareValues(SimWert, Me.RefWert, Me.Funktion)

        'Zielrichtung berücksichtigen
        QWert *= Me.Richtung

        Return QWert

    End Function

End Class
