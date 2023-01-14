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
''' Form for displaying a custom plot
''' </summary>
Public Class CustomPlot

    Private _isInitializing As Boolean

    Private _problem As Common.Problem
    Private _optresult As OptResult.OptResult

    Private _series_StartValue As Steema.TeeChart.Styles.Points
    Private _series_Population As Steema.TeeChart.Styles.Points
    Private _series_SekPop As Steema.TeeChart.Styles.Points
    Private _series_Selected As Steema.TeeChart.Styles.Points

    ''' <summary>
    ''' Is raised when a solution is selected
    ''' </summary>
    ''' <param name="ind">the selected individual</param>
    Public Event pointSelected(ByVal ind As Common.Individuum)

    ''' <summary>
    ''' Index of the currently set optimization parameter
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property iParameter As Integer
        Get
            Return Me.ComboBox_OptParameters.SelectedIndex
        End Get
    End Property

    ''' <summary>
    ''' Index of the currently set objective function
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property iObjective As Integer
        Get
            Return Me.ComboBox_ObjectiveFunctions.SelectedIndex
        End Get
    End Property

    ''' <summary>
    ''' Instantiates a new CustomPlot window
    ''' </summary>
    ''' <param name="problem">the optimization problem definition</param>
    ''' <param name="optresult">the optresult data to plot</param>
    Public Sub New(problem As Common.Problem, optresult As OptResult.OptResult)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        Me._isInitializing = True

        Me._problem = problem
        Me._optresult = optresult

        ' populate combo boxes
        Me.ComboBox_OptParameters.Items.Clear()
        For Each param As Common.OptParameter In Me._problem.List_OptParameter
            Me.ComboBox_OptParameters.Items.Add(param)
        Next
        Me.ComboBox_OptParameters.SelectedIndex = 0

        Me.ComboBox_ObjectiveFunctions.Items.Clear()
        For Each objective As Common.ObjectiveFunction In Me._problem.List_ObjectiveFunctions
            Me.ComboBox_ObjectiveFunctions.Items.Add(objective)
        Next
        Me.ComboBox_ObjectiveFunctions.SelectedIndex = 0

        'format chart
        With Me.Diag.Chart
            .Header.Text = ""
            .Legend.Visible = True
            .Legend.CheckBoxes = True
            .Legend.FontSeriesColor = True
            .Axes.Left.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
            .Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
        End With

        'instantiate series
        Me._series_StartValue = Me.Diag.getSeriesPoint("Start value", "Yellow", Steema.TeeChart.Styles.PointerStyles.Circle, 4)
        Me._series_Population = Me.Diag.getSeriesPoint("Population", "Orange")
        Me._series_SekPop = Me.Diag.getSeriesPoint("Secondary population", "Green")
        Me._series_Selected = Me.Diag.getSeriesPoint("Selected solutions", "Red")

        'format marks for selected solutions series
        Me._series_Selected.Marks.Visible = True
        Me._series_Selected.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Label
        Me._series_Selected.Marks.Transparency = 25
        Me._series_Selected.Marks.ArrowLength = 10
        Me._series_Selected.Marks.Arrow.Visible = False

        'add a handler for series clicks
        AddHandler Me.Diag.ClickSeries, AddressOf seriesClick

        'update the chart
        Call Me.UpdateChart()

        Me._isInitializing = False

    End Sub

    ''' <summary>
    ''' Updates the chart, sets axes and titles and finally replots
    ''' </summary>
    Private Sub UpdateChart()

        'get selection from comboboxes
        Dim param As Common.OptParameter = Me.ComboBox_OptParameters.SelectedItem
        Dim objective As Common.ObjectiveFunction = Me.ComboBox_ObjectiveFunctions.SelectedItem

        'set chart title
        Me.Diag.Chart.Header.Text = $"{param.Bezeichnung} vs. {objective.Bezeichnung}"

        'set axis titles
        Me.Diag.Chart.Axes.Bottom.Title.Text = param.Bezeichnung
        Me.Diag.Chart.Axes.Left.Title.Text = objective.Bezeichnung

        'set x axis min max
        Me.Diag.Chart.Axes.Bottom.Automatic = False
        Me.Diag.Chart.Axes.Bottom.Minimum = param.Min
        Me.Diag.Chart.Axes.Bottom.Maximum = param.Max

        'set y axis to automatic
        Me.Diag.Chart.Axes.Left.Automatic = True

        'replot everything
        Call Me.UpdatePlot()

    End Sub

    ''' <summary>
    ''' Update the plot with new data
    ''' </summary>
    ''' <param name="optresult">the optresult containing the data to plot</param>
    Public Sub UpdateData(optresult As OptResult.OptResult)
        Me._optresult = optresult
        Call Me.UpdatePlot()
    End Sub

    ''' <summary>
    ''' Clears any current data from the series and replots everything
    ''' </summary>
    Private Sub UpdatePlot()

        Dim ind As Common.Individuum

        'plot start value
        _series_StartValue.Clear()
        ind = _optresult.getSolution(1)
        _series_StartValue.Add(ind.OptParameter(iParameter).RWert, ind.Objectives(iObjective) * _problem.List_ObjectiveFunctions(iObjective).Richtung, ind.ID.ToString)

        'plot population
        _series_Population.Clear()
        For Each ind In Me._optresult.Solutions
            _series_Population.Add(ind.OptParameter(iParameter).RWert, ind.Objectives(iObjective) * _problem.List_ObjectiveFunctions(iObjective).Richtung, ind.ID.ToString)
        Next

        'plot secondary population
        _series_SekPop.Clear()
        For Each ind In Me._optresult.getSekPop
            _series_SekPop.Add(ind.OptParameter(iParameter).RWert, ind.Objectives(iObjective) * _problem.List_ObjectiveFunctions(iObjective).Richtung, ind.ID.ToString)
        Next

        'plot selected solutions
        _series_Selected.Clear()
        For Each ind In Me._optresult.getSelectedSolutions
            Call Me.showSelectedSolution(ind)
        Next

    End Sub

    ''' <summary>
    ''' Clears the selected solutions
    ''' </summary>
    Public Sub clearSelection()
        Me._series_Selected.Clear()
    End Sub

    ''' <summary>
    ''' Shows a selected solution in the plot
    ''' </summary>
    ''' <param name="ind">the selected individual</param>
    Public Sub showSelectedSolution(ind As Common.Individuum)
        Me._series_Selected.Add(ind.OptParameter(iParameter).RWert, ind.Objectives(iObjective) * _problem.List_ObjectiveFunctions(iObjective).Richtung, ind.ID.ToString)
    End Sub

    ''' <summary>
    ''' Handles series click event
    ''' Determines the selected solution and then raises the pointSelected event
    ''' </summary>
    Private Sub seriesClick(ByVal sender As Object, ByVal s As Steema.TeeChart.Styles.Series, ByVal valueIndex As Integer, ByVal e As System.Windows.Forms.MouseEventArgs)

        Dim indID_clicked As Integer
        Dim ind As Common.Individuum

        Try
            'get solution ID from series label
            indID_clicked = s.Labels(valueIndex)

            'retrieve the selected solution
            ind = Me._optresult.getSolution(indID_clicked)

            'raise the event (handled by Form1.selectSolution())
            RaiseEvent pointSelected(ind)

        Catch ex As Exception
            MsgBox("Solution is not selectable!", MsgBoxStyle.Information)
        End Try

    End Sub

    ''' <summary>
    ''' Handles comboboxes for axis selection being changed
    ''' Causes the chart to update
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ComboBox_Axes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_OptParameters.SelectedIndexChanged, ComboBox_ObjectiveFunctions.SelectedIndexChanged
        If Me._isInitializing Then
            Exit Sub
        End If

        Call Me.UpdateChart()
    End Sub

    ''' <summary>
    ''' Handles FormClosing event
    ''' Prevents the form from being disposed and hides it instead
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CustomPlot_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = True
        Call Me.Hide()
    End Sub

End Class