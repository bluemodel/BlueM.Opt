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

''' <summary>
''' Class for sampling the parameter space using different methods
''' </summary>
Public Class ParameterSampler

    ''' <summary>
    ''' Generates a sample of parameter values using the given method
    ''' </summary>
    ''' <param name="NumParams">number of parameters</param>
    ''' <param name="NumSteps">number of steps</param>
    ''' <param name="method">sampling method</param>
    ''' <returns>list of parameter combinations</returns>
    Public Function Sample(NumParams As Integer, NumSteps As Integer, method As Common.Settings_Sensiplot.SensiType) As List(Of Double())

        Dim random As New Random()
        Dim parameterCombinations As New List(Of Double())

        Select Case method

            Case Common.Settings_Sensiplot.SensiType.randomDistribution

                ' no. of combinations: NumSteps
                For n = 0 To NumSteps - 1
                    Dim parameterCombination(NumParams - 1) As Double
                    For i = 0 To NumParams - 1
                        Dim value As Double = random.NextDouble()
                        parameterCombination(i) = value
                    Next
                    parameterCombinations.Add(parameterCombination)
                Next

            Case Common.Settings_Sensiplot.SensiType.evenDistribution

                ' no. of combinations: NumSteps ^ NumParams
                Dim totalCombinations As Integer = NumSteps ^ NumParams

                For n = 0 To totalCombinations - 1
                    Dim parameterCombination(NumParams - 1) As Double
                    For i = 0 To NumParams - 1
                        Dim index As Integer = (n \ (NumSteps ^ (NumParams - i - 1))) Mod NumSteps
                        parameterCombination(i) = CDbl(index) / CDbl(NumSteps - 1)
                    Next
                    parameterCombinations.Add(parameterCombination)
                Next

            Case Common.Settings_Sensiplot.SensiType.latinHypercube

                ' no. of combinations: NumSteps
                Dim lhsMatrix(NumSteps - 1)() As Double

                ' Generate evenly spaced intervals for each dimension
                Dim intervalSize As Double = 1.0 / NumSteps

                For i = 0 To NumSteps - 1
                    lhsMatrix(i) = New Double(NumParams - 1) {}

                    For j = 0 To NumParams - 1
                        lhsMatrix(i)(j) = (i + random.NextDouble()) * intervalSize
                    Next
                Next

                ' Shuffle each column to eliminate any remaining correlations
                For j = 0 To NumParams - 1
                    For i = 0 To NumSteps - 1
                        Dim randomIndex As Integer = random.Next(i, NumSteps)
                        Dim temp As Double = lhsMatrix(i)(j)
                        lhsMatrix(i)(j) = lhsMatrix(randomIndex)(j)
                        lhsMatrix(randomIndex)(j) = temp
                    Next
                Next

                'store parameter combinations
                For i = 0 To NumSteps - 1
                    Dim parameterCombination(NumParams - 1) As Double
                    parameterCombination = lhsMatrix(i)
                    parameterCombinations.Add(parameterCombination)
                Next

        End Select

        Return parameterCombinations

    End Function

End Class
