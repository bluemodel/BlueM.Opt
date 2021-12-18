' Copyright (c) BlueM Dev Group
' Website: http://bluemodel.org
' 
' All rights reserved.
' 
' Released under the BSD-2-Clause License:
' 
' Redistribution and use in source and binary forms, with or without modification, 
' are permitted provided that the following conditions are met:
' 
' * Redistributions of source code must retain the above copyright notice, this list 
'   of conditions and the following disclaimer.
' * Redistributions in binary form must reproduce the above copyright notice, this list 
'   of conditions and the following disclaimer in the documentation and/or other materials 
'   provided with the distribution.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
' EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
' OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
' SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT 
' OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
' HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
' TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
' EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'--------------------------------------------------------------------------------------------
'
Imports BlueM

''' <summary>
''' Klasse f�r die Definition von Objective Funktionen
''' </summary>
Public MustInherit Class ObjectiveFunction

    ''' <summary>
    ''' Struktur f�r Simulationsergebnisse
    ''' </summary>
    Public Structure SimErgebnis_Structure
        ''' <summary>
        ''' Ergebniswerte (Key ist ObjectiveFunction Name)
        ''' </summary>
        Public Werte As Dictionary(Of String, Double)
        ''' <summary>
        ''' Ergebnisreihen (Key ist ObjectiveFunction Name)
        ''' </summary>
        Public Reihen As Dictionary(Of String, Wave.TimeSeries)
        ''' <summary>
        ''' L�scht alle vorhandenen Ergebnisse
        ''' </summary>
        Public Sub Clear()
            Me.Werte = New Dictionary(Of String, Double)
            Me.Reihen = New Dictionary(Of String, Wave.TimeSeries)
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
    ''' Bezeichnung
    ''' </summary>
    Public Bezeichnung As String

    ''' <summary>
    ''' Gruppe
    ''' </summary>
    Public Gruppe As String

    ''' <summary>
    ''' Richtung der ObjectiveFunction (d.h. zu maximieren oder zu minimieren)
    ''' </summary>
    Public Richtung As Constants.EVO_DIRECTION

    ''' <summary>
    ''' Operator bzw Faktor
    ''' </summary>
    Public OpFact As Double

    ''' <summary>
    ''' Die Dateiendung der Ergebnisdatei, aus der das Simulationsergebnis ausgelesen werden soll
    ''' </summary>
    ''' <remarks>Erlaubte Werte: z.B. "WEL" oder "ASC"</remarks>
    Public Datei As String

    ''' <summary>
    ''' Die Simulationsgr��e, auf dessen Basis der Objectivewert berechnet werden soll
    ''' </summary>
    Public SimGr As String

    ''' <summary>
    ''' Name der Funktion, mit der der Objectivewert berechnet werden soll
    ''' </summary>
    ''' <remarks>Erlaubte Werte: "AbQuad", "Diff", "n�ber", "s�ber", "nUnter", "sUnter", "Volf", etc. Siehe auch Wiki</remarks>
    Public Funktion As String

    ''' <summary>
    ''' Gibt an, ob die Objective Function einen IstWert besitzt
    ''' </summary>
    Public hasIstWert As Boolean

    ''' <summary>
    ''' Objective Wert im Istzustand
    ''' </summary>
    Public IstWert As Double

    ''' <summary>
    ''' Gibt die Bezeichnung zur�ck
    ''' </summary>
    ''' <returns>Bezeichnung</returns>
    Public Overrides Function ToString() As String
        Return Me.Bezeichnung
    End Function

    ''' <summary>
    ''' Gibt an ob es ein GruppenLeader ist
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
    ''' <param name="SimErgebnis">collection of simulation results</param>
    ''' <returns>objective function value</returns>
    Public MustOverride Function calculateObjective(ByVal SimErgebnis As SimErgebnis_Structure) As Double

    ''' <summary>
    ''' compare two values using a function
    ''' </summary>
    ''' <param name="SimWert">simulation value</param>
    ''' <param name="RefWert">reference value</param>
    ''' <param name="Funktion">comparison function</param>
    ''' <returns>function value</returns>
    ''' <remarks>BUG 218: Konstante und gleiche Zeitschrittweiten vorausgesetzt!</remarks>
    Protected Shared Function compareValues(ByVal SimWert As Double, ByVal RefWert As Double, ByVal Funktion As String)

        Dim QWert As Double

        'Fallunterscheidung Zielfunktion
        Select Case Funktion.ToUpper()

            Case "SE", "ABQUAD"
                'squared error
                '-------------
                QWert = (RefWert - SimWert) ^ 2

            Case "AE", "DIFF"
                'absolute error
                '--------------
                QWert = Math.Abs(RefWert - SimWert)

            Case Else
                Throw New Exception($"The objective function '{Funktion}' is not supported for value comparisons!")

        End Select

        Return QWert

    End Function

    ''' <summary>
    ''' compare two series using a function
    ''' </summary>
    ''' <param name="SimReihe">simulation series</param>
    ''' <param name="RefReihe">reference series</param>
    ''' <param name="Funktion">comparison function</param>
    ''' <returns>function value</returns>
    ''' <remarks>BUG 218: Konstante und gleiche Zeitschrittweiten vorausgesetzt!</remarks>
    Protected Shared Function compareSeries(ByVal SimReihe As Wave.TimeSeries, ByVal RefReihe As Wave.TimeSeries, ByVal Funktion As String) As Double

        Dim QWert As Double
        Dim i As Integer

        'BUG 218: Kontrolle
        If (RefReihe.Length <> SimReihe.Length) Then
            Throw New Exception($"The series '{SimReihe.Title}' and '{RefReihe.Title}' are not compatible! Different length/timestep? (see Bug 218)")
        End If

        'Fallunterscheidung Zielfunktion
        Select Case Funktion.ToUpper()

            Case "SSE", "ABQUAD"
                'Sum of squared errors
                '---------------------
                QWert = 0
                For i = 0 To SimReihe.Length - 1
                    QWert += (RefReihe.Values(i) - SimReihe.Values(i)) ^ 2
                Next

            Case "MSE"
                'Mean squared error
                '------------------
                QWert = 0
                For i = 0 To SimReihe.Length - 1
                    QWert += (RefReihe.Values(i) - SimReihe.Values(i)) ^ 2
                Next
                QWert = QWert / SimReihe.Length

            Case "SAE", "DIFF"
                'Sum of abolute errors
                '---------------------
                QWert = 0
                For i = 0 To SimReihe.Length - 1
                    QWert += Math.Abs(RefReihe.Values(i) - SimReihe.Values(i))
                Next

            Case "MAE"
                'Mean abolute error
                '------------------
                QWert = 0
                For i = 0 To SimReihe.Length - 1
                    QWert += Math.Abs(RefReihe.Values(i) - SimReihe.Values(i))
                Next
                QWert = QWert / SimReihe.Length

            Case "BIAS", "VOLF"
                'Absolute volume error
                '---------------------
                Dim VolSim As Double = 0
                Dim VolZiel As Double = 0
                For i = 0 To SimReihe.Length - 1
                    VolSim += SimReihe.Values(i)
                    VolZiel += RefReihe.Values(i)
                Next
                'Differenz bilden und auf ZielVolumen beziehen
                QWert = Math.Abs(VolZiel - VolSim) / VolZiel * 100

            Case "NLT", "NUNTER"
                'Relative number of timesteps where simulation is less than reference [%]
                '------------------------------------------------------------------------
                Dim nUnter As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.Values(i) < RefReihe.Values(i)) Then
                        nUnter += 1
                    End If
                Next
                QWert = nUnter / SimReihe.Length * 100

            Case "SLT", "SUNTER"
                'Sum of simulation values less than reference
                '--------------------------------------------
                Dim sUnter As Double = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.Values(i) < RefReihe.Values(i)) Then
                        sUnter += RefReihe.Values(i) - SimReihe.Values(i)
                    End If
                Next
                QWert = sUnter

            Case "NGT", "N�BER"
                'Relative number of timesteps where simulation is greater than reference [%]
                '---------------------------------------------------------------------------
                Dim nUeber As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.Values(i) > RefReihe.Values(i)) Then
                        nUeber += 1
                    End If
                Next
                QWert = nUeber / SimReihe.Length * 100

            Case "SGT", "S�BER"
                'Sum of simulation values greater than reference
                '-----------------------------------------------
                Dim sUeber As Double = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.Values(i) > RefReihe.Values(i)) Then
                        sUeber += SimReihe.Values(i) - RefReihe.Values(i)
                    End If
                Next
                QWert = sUeber

            Case "NSE"
                'Nash-Sutcliffe efficiency
                '-------------------------
                Dim avg_obs, zaehler, nenner As Double
                avg_obs = RefReihe.Average
                zaehler = 0.0
                nenner = 0.0
                For i = 0 To SimReihe.Length - 1
                    zaehler += (RefReihe.Values(i) - SimReihe.Values(i)) ^ 2
                    nenner += (RefReihe.Values(i) - avg_obs) ^ 2
                Next
                QWert = 1 - zaehler / nenner

            Case "LNNSE"
                'Logarithmic Nash-Sutcliffe efficiency
                '-------------------------------------
                Dim obs_avg, zaehler, nenner, Qobs, Qsim, minValue As Double
                obs_avg = RefReihe.Average
                minValue = obs_avg / 100.0 ' define a minimum value, to prevent Math.Log(0) = -Infinity
                zaehler = 0.0
                nenner = 0.0
                For i = 0 To SimReihe.Length - 1
                    Qobs = Math.Max(minValue, RefReihe.Values(i))
                    Qsim = Math.Max(minValue, SimReihe.Values(i))
                    zaehler += (Math.Log(Qobs) - Math.Log(Qsim)) ^ 2
                    nenner += (Math.Log(Qobs) - Math.Log(obs_avg)) ^ 2
                Next
                QWert = 1 - zaehler / nenner

            Case "DET", "KORR"
                'Coefficient of determination r^2 (linear regression)
                '----------------------------------------------------
                Dim covar, var_x, var_y, x_avg, y_avg As Double
                Dim n As Integer = SimReihe.Length

                x_avg = SimReihe.Average
                y_avg = RefReihe.Average
                'r^2 = sxy^2 / (sx^2 * sy^2)
                'standard deviation: var_x = sx^2 = 1 / (n-1) * SUMME[(x_i - x_avg)^2]
                'covariance: covar= sxy = 1 / (n-1) * SUMME[(x_i - x_avg) * (y_i - y_avg)]
                covar = 0
                var_x = 0
                var_y = 0
                For i = 0 To n - 1
                    covar += (SimReihe.Values(i) - x_avg) * (RefReihe.Values(i) - y_avg)
                    var_x += (SimReihe.Values(i) - x_avg) ^ 2
                    var_y += (RefReihe.Values(i) - y_avg) ^ 2
                Next
                var_x = 1 / (n - 1) * var_x
                var_y = 1 / (n - 1) * var_y
                covar = 1 / (n - 1) * covar
                'coefficient of determination = correlation coefficient ^ 2
                QWert = covar ^ 2 / (var_x * var_y)

            Case "KGE"
                'Kling-Gupta efficiency
                'https://permetrics.readthedocs.io/pages/regression/KGE.html
                Dim corr, covar, avg_sim, avg_obs, std_sim, std_obs As Double
                Dim n As Integer = SimReihe.Length

                avg_sim = SimReihe.Average
                avg_obs = RefReihe.Average

                covar = 0
                std_sim = 0
                std_obs = 0
                For i = 0 To n - 1
                    covar += (SimReihe.Values(i) - avg_sim) * (RefReihe.Values(i) - avg_obs)
                    std_sim += (SimReihe.Values(i) - avg_sim) ^ 2
                    std_obs += (RefReihe.Values(i) - avg_obs) ^ 2
                Next
                std_sim = Math.Sqrt(1 / (n - 1) * std_sim)
                std_obs = Math.Sqrt(1 / (n - 1) * std_obs)
                covar = 1 / (n - 1) * covar
                corr = covar / (std_sim * std_obs) 'correlation coefficient

                Dim biasratio As Double = avg_sim / avg_obs
                Dim variabilityratio As Double = (std_sim / avg_sim) / (std_obs / avg_obs)
                QWert = 1 - Math.Sqrt((corr - 1) ^ 2 + (biasratio - 1) ^ 2 + (variabilityratio - 1) ^ 2)

            Case "NASHSUTT"
                'Modified Nash Sutcliffe (deprecated)
                '1 - NSE
                '------------------------------------
                QWert = 1.0 - compareSeries(SimReihe, RefReihe, "NSE")

            Case "LNNASHSUTT"
                'Modified Logarithmic Nash Sutcliffe (deprecated)
                '1 - lnNSE
                '------------------------------------------------
                QWert = 1.0 - compareSeries(SimReihe, RefReihe, "lnNSE")

            Case Else
                Throw New Exception($"The objective function '{Funktion}' is not supported for series comparisons!")

        End Select

        Return QWert

    End Function

End Class
