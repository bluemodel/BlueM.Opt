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
Imports System.Globalization
Imports BlueM

''' <summary>
''' Klasse für die Definition von Objective Funktionen
''' </summary>
Public MustInherit Class ObjectiveFunction

    ''' <summary>
    ''' Structure for holding simulation results
    ''' </summary>
    Public Structure SimResults
        ''' <summary>
        ''' Result values (key is ObjectiveFunction name)
        ''' </summary>
        Public Values As Dictionary(Of String, Double)
        ''' <summary>
        ''' Result time series (key is ObjectiveFunction name)
        ''' </summary>
        Public Series As Dictionary(Of String, Wave.TimeSeries)
        ''' <summary>
        ''' CLears all results
        ''' </summary>
        Public Sub Clear()
            Me.Values = New Dictionary(Of String, Double)
            Me.Series = New Dictionary(Of String, Wave.TimeSeries)
        End Sub
    End Structure

    Public Enum ObjectiveType As Integer
        Series = 1
        Value = 2
        ValueFromSeries = 3
        Aggregate = 5
    End Enum

    ''' <summary>
    ''' Gibt an ob es sich um eine PrimaryObjective Function handelt
    ''' </summary>
    Public isPrimObjective As Boolean

    ''' <summary>
    ''' Description
    ''' </summary>
    Public Description As String

    ''' <summary>
    ''' Group
    ''' </summary>
    Public Group As String

    ''' <summary>
    ''' Directon of the ObjectiveFunction (i.e. maximize or minimize)
    ''' </summary>
    Public Direction As Constants.EVO_DIRECTION

    ''' <summary>
    ''' Factor for aggregation
    ''' </summary>
    Public Factor As Double

    ''' <summary>
    ''' File extension of the result file from which to read the simulation result, e.g. "WEL", "ASC", "KTR.WEL", "WBL", etc.
    ''' </summary>
    Public FileExtension As String

    ''' <summary>
    ''' Name of the simulation result from which to calculate the objective function value
    ''' </summary>
    Public SimResultName As String

    ''' <summary>
    ''' Name of the function with which to calculate the objective function value
    ''' </summary>
    ''' <remarks>See https://wiki.bluemodel.org/index.php/OBF-file</remarks>
    Public [Function] As String

    ''' <summary>
    ''' Indicates whether the objective function has a "current value"
    ''' </summary>
    Public hasCurrentValue As Boolean

    ''' <summary>
    ''' Current objective function value
    ''' </summary>
    Public CurrentValue As Double

    ''' <summary>
    ''' Returns the description
    ''' </summary>
    Public Overrides Function ToString() As String
        Return Me.Description
    End Function

    ''' <summary>
    ''' Indicates whether the objective function is the group leader for aggregation
    ''' </summary>
    Public ReadOnly Property isGroupLeader() As Boolean
        Get
            If (Me.GetObjType = ObjectiveType.Aggregate) Then
                Return True
            End If
            Return False
        End Get
    End Property

    ''' <summary>
    ''' Returns the type of the ObjectiveFunction
    ''' </summary>
    Public MustOverride ReadOnly Property GetObjType() As ObjectiveType

    ''' <summary>
    ''' Calculate the objective function value
    ''' </summary>
    ''' <param name="SimResult">simulation result</param>
    ''' <returns>objective function value</returns>
    Public MustOverride Function calculateObjective(ByVal SimResult As SimResults) As Double

    ''' <summary>
    ''' Compare two values using a function
    ''' </summary>
    ''' <param name="SimValue">simulation value</param>
    ''' <param name="RefValue">reference value</param>
    ''' <param name="[Function]">comparison function</param>
    ''' <returns>function value</returns>
    ''' <remarks>Konstante und gleiche Zeitschrittweiten vorausgesetzt! (#151)</remarks>
    Protected Shared Function compareValues(ByVal SimValue As Double, ByVal RefValue As Double, ByVal [Function] As String)

        Dim objectiveValue As Double

        'Fallunterscheidung Zielfunktion
        Select Case [Function].ToUpper()

            Case "SE", "ABQUAD"
                'squared error
                '-------------
                objectiveValue = (RefValue - SimValue) ^ 2

            Case "AE", "DIFF"
                'absolute error
                '--------------
                objectiveValue = Math.Abs(RefValue - SimValue)

            Case Else
                Throw New Exception($"The objective function '{[Function]}' is not supported for value comparisons!")

        End Select

        Return objectiveValue

    End Function

    ''' <summary>
    ''' Compare two series using a function
    ''' </summary>
    ''' <param name="SimSeries">simulation series</param>
    ''' <param name="RefSeries">reference series</param>
    ''' <param name="[Function]">comparison function</param>
    ''' <returns>function value</returns>
    ''' <remarks>Konstante und gleiche Zeitschrittweiten vorausgesetzt! (#151)</remarks>
    Protected Shared Function compareSeries(ByVal SimSeries As Wave.TimeSeries, ByVal RefSeries As Wave.TimeSeries, ByVal [Function] As String) As Double

        Dim objectiveValue As Double
        Dim i As Integer

        'Kontrolle (#151)
        If (RefSeries.Length <> SimSeries.Length) Then
            Throw New Exception($"The series '{SimSeries.Title}' and '{RefSeries.Title}' are not compatible! Different length/timestep? (see #151)")
        End If

        'remove NaN values
        If RefSeries.NaNCount > 0 Then
            RefSeries = RefSeries.removeNaNValues()
            BlueM.Wave.TimeSeries.Synchronize(SimSeries, RefSeries)
        End If

        'Fallunterscheidung Zielfunktion
        Select Case [Function].ToUpper()

            Case "APFB"
                'Annual Peak Flow Bias
                'Mizukami et al., 2019 https://hess.copernicus.org/articles/23/2601/2019/
                '---------------------
                Dim simYears As Dictionary(Of Integer, Wave.TimeSeries) = SimSeries.SplitHydroYears(SimSeries.StartDate.Month)
                Dim refYears As Dictionary(Of Integer, Wave.TimeSeries) = RefSeries.SplitHydroYears(SimSeries.StartDate.Month)
                Dim simMaxMean As Double = simYears.Values.Select(Function(ts) ts.Maximum()).Average()
                Dim refMaxMean As Double = refYears.Values.Select(Function(ts) ts.Maximum()).Average()
                objectiveValue = Math.Sqrt(Math.Pow((simMaxMean / refMaxMean) - 1.0, 2))

            Case "SSE", "ABQUAD"
                'Sum of squared errors
                '---------------------
                objectiveValue = 0
                For i = 0 To SimSeries.Length - 1
                    objectiveValue += (RefSeries.Values(i) - SimSeries.Values(i)) ^ 2
                Next

            Case "MSE"
                'Mean squared error
                '------------------
                objectiveValue = 0
                For i = 0 To SimSeries.Length - 1
                    objectiveValue += (RefSeries.Values(i) - SimSeries.Values(i)) ^ 2
                Next
                objectiveValue = objectiveValue / SimSeries.Length

            Case "SAE", "DIFF"
                'Sum of abolute errors
                '---------------------
                objectiveValue = 0
                For i = 0 To SimSeries.Length - 1
                    objectiveValue += Math.Abs(RefSeries.Values(i) - SimSeries.Values(i))
                Next

            Case "MAE"
                'Mean absolute error
                '------------------
                objectiveValue = 0
                For i = 0 To SimSeries.Length - 1
                    objectiveValue += Math.Abs(RefSeries.Values(i) - SimSeries.Values(i))
                Next
                objectiveValue = objectiveValue / SimSeries.Length

            Case "BIAS", "VOLF"
                'Absolute volume error
                '---------------------
                Dim VolSim As Double = 0
                Dim VolZiel As Double = 0
                For i = 0 To SimSeries.Length - 1
                    VolSim += SimSeries.Values(i)
                    VolZiel += RefSeries.Values(i)
                Next
                'Differenz bilden und auf ZielVolumen beziehen
                objectiveValue = Math.Abs(VolZiel - VolSim) / VolZiel * 100

            Case "NLT", "NUNTER"
                'Relative number of timesteps where simulation is less than reference [%]
                '------------------------------------------------------------------------
                Dim nUnter As Integer = 0
                For i = 0 To SimSeries.Length - 1
                    If (SimSeries.Values(i) < RefSeries.Values(i)) Then
                        nUnter += 1
                    End If
                Next
                objectiveValue = nUnter / SimSeries.Length * 100

            Case "SLT", "SUNTER"
                'Sum of simulation values less than reference
                '--------------------------------------------
                Dim sUnter As Double = 0
                For i = 0 To SimSeries.Length - 1
                    If (SimSeries.Values(i) < RefSeries.Values(i)) Then
                        sUnter += RefSeries.Values(i) - SimSeries.Values(i)
                    End If
                Next
                objectiveValue = sUnter

            Case "NGT", "NÜBER"
                'Relative number of timesteps where simulation is greater than reference [%]
                '---------------------------------------------------------------------------
                Dim nUeber As Integer = 0
                For i = 0 To SimSeries.Length - 1
                    If (SimSeries.Values(i) > RefSeries.Values(i)) Then
                        nUeber += 1
                    End If
                Next
                objectiveValue = nUeber / SimSeries.Length * 100

            Case "SGT", "SÜBER"
                'Sum of simulation values greater than reference
                '-----------------------------------------------
                Dim sUeber As Double = 0
                For i = 0 To SimSeries.Length - 1
                    If (SimSeries.Values(i) > RefSeries.Values(i)) Then
                        sUeber += SimSeries.Values(i) - RefSeries.Values(i)
                    End If
                Next
                objectiveValue = sUeber

            Case "NSE"
                'Nash-Sutcliffe efficiency
                '-------------------------
                Dim avg_obs, zaehler, nenner As Double
                avg_obs = RefSeries.Average
                zaehler = 0.0
                nenner = 0.0
                For i = 0 To SimSeries.Length - 1
                    zaehler += (RefSeries.Values(i) - SimSeries.Values(i)) ^ 2
                    nenner += (RefSeries.Values(i) - avg_obs) ^ 2
                Next
                objectiveValue = 1 - zaehler / nenner

            Case "LNNSE"
                'Logarithmic Nash-Sutcliffe efficiency
                '-------------------------------------
                Dim epsilon, avg_ln_obs As Double
                ' negligible constant to prevent Math.Log(0) = -Infinity
                ' Pushpalatha et al. (2012) DOI:10.1016/j.jhydrol.2011.11.055
                epsilon = RefSeries.Average / 100.0

                ' transform all values by adding epsilon and then logarithmisizing
                Dim values_ref As New List(Of Double)
                Dim values_sim As New List(Of Double)
                For i = 0 To RefSeries.Length - 1
                    values_ref.Add(Math.Log(RefSeries.Values(i) + epsilon))
                    values_sim.Add(Math.Log(SimSeries.Values(i) + epsilon))
                Next

                avg_ln_obs = values_ref.Sum() / values_ref.Count

                Dim sum_ln_diff_squared As Double = 0.0
                Dim sum_ln_diff_avg_squared As Double = 0.0

                For i = 0 To values_ref.Count - 1
                    sum_ln_diff_squared += (values_ref(i) - values_sim(i)) ^ 2
                    sum_ln_diff_avg_squared += (values_ref(i) - avg_ln_obs) ^ 2
                Next

                objectiveValue = 1 - sum_ln_diff_squared / sum_ln_diff_avg_squared

            Case "DET", "KORR"
                'Coefficient of determination r^2 (linear regression)
                '----------------------------------------------------
                Dim covar, var_x, var_y, x_avg, y_avg As Double
                Dim n As Integer = SimSeries.Length

                x_avg = SimSeries.Average
                y_avg = RefSeries.Average
                'r^2 = sxy^2 / (sx^2 * sy^2)
                'standard deviation: var_x = sx^2 = 1 / (n-1) * SUMME[(x_i - x_avg)^2]
                'covariance: covar= sxy = 1 / (n-1) * SUMME[(x_i - x_avg) * (y_i - y_avg)]
                covar = 0
                var_x = 0
                var_y = 0
                For i = 0 To n - 1
                    covar += (SimSeries.Values(i) - x_avg) * (RefSeries.Values(i) - y_avg)
                    var_x += (SimSeries.Values(i) - x_avg) ^ 2
                    var_y += (RefSeries.Values(i) - y_avg) ^ 2
                Next
                var_x = 1 / (n - 1) * var_x
                var_y = 1 / (n - 1) * var_y
                covar = 1 / (n - 1) * covar
                'coefficient of determination = correlation coefficient ^ 2
                objectiveValue = covar ^ 2 / (var_x * var_y)

            Case "KGE"
                'Modified Kling-Gupta Efficiency, Kling et al. 2012 https://doi.org/10.1016/j.jhydrol.2012.01.011
                'Formula: https://thibhlln.github.io/hydroeval/functions/hydroeval.kgeprime.html
                Dim corr, covar, avg_sim, avg_obs, std_sim, std_obs As Double
                Dim n As Integer = SimSeries.Length

                avg_sim = SimSeries.Average
                avg_obs = RefSeries.Average

                covar = 0
                std_sim = 0
                std_obs = 0
                For i = 0 To n - 1
                    covar += (SimSeries.Values(i) - avg_sim) * (RefSeries.Values(i) - avg_obs)
                    std_sim += (SimSeries.Values(i) - avg_sim) ^ 2
                    std_obs += (RefSeries.Values(i) - avg_obs) ^ 2
                Next
                std_sim = Math.Sqrt(1 / (n - 1) * std_sim)
                std_obs = Math.Sqrt(1 / (n - 1) * std_obs)
                covar = 1 / (n - 1) * covar
                corr = covar / (std_sim * std_obs) 'correlation coefficient

                Dim biasratio As Double = avg_sim / avg_obs
                Dim variabilityratio As Double = (std_sim / avg_sim) / (std_obs / avg_obs)
                objectiveValue = 1 - Math.Sqrt((corr - 1) ^ 2 + (biasratio - 1) ^ 2 + (variabilityratio - 1) ^ 2)

            Case "NASHSUTT"
                'Modified Nash Sutcliffe (deprecated)
                '1 - NSE
                '------------------------------------
                objectiveValue = 1.0 - compareSeries(SimSeries, RefSeries, "NSE")

            Case "LNNASHSUTT"
                'Modified Logarithmic Nash Sutcliffe (deprecated)
                '1 - lnNSE
                '------------------------------------------------
                objectiveValue = 1.0 - compareSeries(SimSeries, RefSeries, "lnNSE")

            Case Else
                Throw New Exception($"The objective function '{[Function]}' is not supported for series comparisons!")

        End Select

        Return objectiveValue

    End Function

End Class
