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
Imports System.Windows.Forms

''' <summary>
''' Form with diagram for testing SensiPlot ParameterSampling
''' </summary>
Public Class ParameterSampling

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    ''' <summary>
    ''' Generates new parameter samples and plots them
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Generate(sender As Object, e As EventArgs) Handles Button_Generate.Click

        Me.TChart1.Series.Clear()

        Dim NumSteps As Integer = NumericUpDown1.Value
        Dim NumParams As Integer = 2

        'sample optparameters
        Dim parameterCombinations As List(Of Double())
        Dim sampler As New BlueM.Opt.Algos.SensiPlot.ParameterSampler()

        parameterCombinations = sampler.Sample(NumParams, NumSteps, BlueM.Opt.Common.Settings_Sensiplot.SensiType.evenDistribution)
        plotParameterCombinations(parameterCombinations, "even distribution")

        parameterCombinations = sampler.Sample(NumParams, NumSteps, BlueM.Opt.Common.Settings_Sensiplot.SensiType.randomDistribution)
        plotParameterCombinations(parameterCombinations, "random distribution")

        parameterCombinations = sampler.Sample(NumParams, NumSteps, BlueM.Opt.Common.Settings_Sensiplot.SensiType.latinHypercube)
        plotParameterCombinations(parameterCombinations, "latin hypercube sampling")

    End Sub

    ''' <summary>
    ''' Plots a list of parameter combinations in the chart
    ''' </summary>
    ''' <param name="parameterCombinations"></param>
    ''' <param name="title"></param>
    Private Sub plotParameterCombinations(parameterCombinations As List(Of Double()), title As String)
        With Me.TChart1
            Dim series As New Steema.TeeChart.Styles.Points()
            series.Title = title
            .Series.Add(series)
            For Each combination As Double() In parameterCombinations
                series.Add(combination(0), combination(1))
            Next
        End With
    End Sub

    Private Sub TChart1_DoubleClick(sender As Object, e As EventArgs) Handles TChart1.DoubleClick
        Me.TChart1.ShowEditor()
    End Sub

End Class