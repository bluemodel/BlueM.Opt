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
    ''' Reference value
    ''' </summary>
    Public RefValue As Double

    ''' <summary>
    ''' Section (block) in which the target variable is located
    ''' </summary>
    Public Block As String

    ''' <summary>
    ''' Target variable (column) within the block
    ''' </summary>
    Public Column As String

    ''' <summary>
    ''' Calculate ObjectiveFunction value
    ''' </summary>
    ''' <param name="SimResult">simulation result</param>
    ''' <returns>objective function value</returns>
    Public Overrides Function calculateObjective(ByVal SimResult As SimResults) As Double

        Dim SimValue As Double
        Dim objectiveValue As Double

        'SimWert aus SimErgebnis rausholen
        SimValue = SimResult.Values(Me.Description)

        'Wertevergleich ausführen
        objectiveValue = ObjectiveFunction.compareValues(SimValue, Me.RefValue, Me.Function)

        'Zielrichtung berücksichtigen
        objectiveValue *= Me.Direction

        Return objectiveValue

    End Function

End Class
