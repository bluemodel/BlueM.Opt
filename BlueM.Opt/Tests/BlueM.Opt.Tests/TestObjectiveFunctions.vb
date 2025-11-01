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
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports BlueM.Opt.Common
Imports BlueM.Wave

<TestClass>
Public Class TestObjectiveFunctions

    Private Function CreateTimeSeries(values As Double(), Optional title As String = "Test") As TimeSeries
        Dim ts As New TimeSeries()
        ts.Title = title
        ts.Interpretation = TimeSeries.InterpretationEnum.BlockRight
        Dim startdate As New DateTime(2020, 1, 1)
        For i = 0 To values.Length - 1
            ts.AddNode(startdate.AddDays(i), values(i))
        Next
        Return ts
    End Function

    <TestMethod>
    Public Sub CompareSeries_SSE_ReturnsSumOfSquaredErrors()
        Dim sim As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        Dim ref As TimeSeries = CreateTimeSeries({2.0, 2.0, 2.0})
        Dim result = ObjectiveFunction.compareSeries(sim, ref, "SSE")
        Assert.AreEqual(2.0, result, 0.000001)
    End Sub

    <TestMethod>
    Public Sub CompareSeries_MSE_ReturnsMeanSquaredError()
        Dim sim As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        Dim ref As TimeSeries = CreateTimeSeries({2.0, 2.0, 2.0})
        Dim result = ObjectiveFunction.compareSeries(sim, ref, "MSE")
        Assert.AreEqual(2.0 / 3.0, result, 0.000001)
    End Sub

    <TestMethod>
    Public Sub CompareSeries_SAE_ReturnsSumOfAbsoluteErrors()
        Dim sim As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        Dim ref As TimeSeries = CreateTimeSeries({2.0, 2.0, 2.0})
        Dim result = ObjectiveFunction.compareSeries(sim, ref, "SAE")
        Assert.AreEqual(2.0, result, 0.000001)
    End Sub

    <TestMethod>
    Public Sub CompareSeries_MAE_ReturnsMeanAbsoluteError()
        Dim sim As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        Dim ref As TimeSeries = CreateTimeSeries({2.0, 2.0, 2.0})
        Dim result = ObjectiveFunction.compareSeries(sim, ref, "MAE")
        Assert.AreEqual(2.0 / 3.0, result, 0.000001)
    End Sub

    <TestMethod>
    Public Sub CompareSeries_BIAS_ReturnsAbsoluteVolumeError()
        Dim sim As TimeSeries = CreateTimeSeries({2.0, 3.0, 4.0})
        Dim ref As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        Dim result = ObjectiveFunction.compareSeries(sim, ref, "BIAS")
        Assert.AreEqual((9.0 - 6.0) / 6.0 * 100.0, result, 0.000001)
    End Sub

    <TestMethod>
    Public Sub CompareSeries_NLT_ReturnsRelativeNumberOfTimestepsLessThan()
        Dim sim As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        Dim ref As TimeSeries = CreateTimeSeries({2.0, 2.0, 2.0})
        Dim result = ObjectiveFunction.compareSeries(sim, ref, "NLT")
        Assert.AreEqual(1.0 / 3.0 * 100, result, 0.000001)
    End Sub

    <TestMethod>
    Public Sub CompareSeries_SLT_ReturnsSumOfTimestepsLessThan()
        Dim sim As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        Dim ref As TimeSeries = CreateTimeSeries({2.0, 2.0, 2.0})
        Dim result = ObjectiveFunction.compareSeries(sim, ref, "SLT")
        Assert.AreEqual(1.0, result, 0.000001)
    End Sub

    <TestMethod>
    Public Sub CompareSeries_NGT_ReturnsRelativeNumberOfTimestepsGreaterThan()
        Dim sim As TimeSeries = CreateTimeSeries({3.0, 2.0, 1.0})
        Dim ref As TimeSeries = CreateTimeSeries({2.0, 2.0, 2.0})
        Dim result = ObjectiveFunction.compareSeries(sim, ref, "NGT")
        Assert.AreEqual(1.0 / 3.0 * 100, result, 0.000001)
    End Sub

    <TestMethod>
    Public Sub CompareSeries_SGT_ReturnsSumOfTimestepsGreaterThan()
        Dim sim As TimeSeries = CreateTimeSeries({3.0, 2.0, 1.0})
        Dim ref As TimeSeries = CreateTimeSeries({2.0, 2.0, 2.0})
        Dim result = ObjectiveFunction.compareSeries(sim, ref, "SGT")
        Assert.AreEqual(1.0, result, 0.000001)
    End Sub

    <TestMethod>
    Public Sub CompareSeries_APFB_ReturnsAnnualPeakFlowBias()
        'create timeseries with two years of monthly data
        Dim sim As New TimeSeries("sim")
        sim.Interpretation = TimeSeries.InterpretationEnum.BlockRight
        For i As Integer = 1 To 12
            sim.AddNode(New DateTime(2020, i, 1), i)
            sim.AddNode(New DateTime(2021, i, 1), i * 2)
        Next
        Dim ref As New TimeSeries("ref")
        ref.Interpretation = TimeSeries.InterpretationEnum.BlockRight
        For i As Integer = 1 To 12
            ref.AddNode(New DateTime(2020, i, 1), i * 1.1)
            ref.AddNode(New DateTime(2021, i, 1), i * 0.9 * 2)
        Next
        Dim peakSim = {12.0, 24.0}
        Dim peakRef = {13.2, 21.6}
        Dim expected = Math.Sqrt(Math.Pow((peakSim.Average() / peakRef.Average()) - 1.0, 2))
        Dim result = ObjectiveFunction.compareSeries(sim, ref, "APFB")
        Assert.AreEqual(expected, result, 0.000001)
    End Sub

    <TestMethod>
    Public Sub CompareSeries_NSE_ReturnsNashSutcliffeEfficiency()
        Dim sim As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        Dim ref As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        ' Identical series, NSE = 1
        Dim result = ObjectiveFunction.compareSeries(sim, ref, "NSE")
        Assert.AreEqual(1.0, result, 0.000001)
    End Sub

    <TestMethod>
    Public Sub CompareSeries_LNNSE_ReturnsLnNashSutcliffeEfficiency()
        Dim sim As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        Dim ref As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        ' Identical series, lnNSE = 1
        Dim result = ObjectiveFunction.compareSeries(sim, ref, "LNNSE")
        Assert.AreEqual(1.0, result, 0.000001)
    End Sub

    <TestMethod>
    Public Sub CompareSeries_DET_ReturnsCoefficientOfDetermination()
        Dim sim As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        Dim ref As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        ' Identical series, DET = 1
        Dim result = ObjectiveFunction.compareSeries(sim, ref, "DET")
        Assert.AreEqual(1.0, result, 0.000001)
    End Sub

    <TestMethod>
    Public Sub CompareSeries_KGE_ReturnsKlingGuptaEfficiency()
        Dim sim As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        Dim ref As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        ' Identical series, KGE = 1
        Dim result = ObjectiveFunction.compareSeries(sim, ref, "KGE")
        Assert.AreEqual(1.0, result, 0.000001)
    End Sub

    <TestMethod>
    <ExpectedException(GetType(Exception))>
    Public Sub CompareSeries_DifferentLength_ThrowsException()
        Dim sim As TimeSeries = CreateTimeSeries({1.0, 2.0})
        Dim ref As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        ObjectiveFunction.compareSeries(sim, ref, "SSE")
    End Sub

    <TestMethod>
    <ExpectedException(GetType(Exception))>
    Public Sub CompareSeries_UnsupportedFunction_ThrowsException()
        Dim sim As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        Dim ref As TimeSeries = CreateTimeSeries({1.0, 2.0, 3.0})
        ObjectiveFunction.compareSeries(sim, ref, "UNSUPPORTED")
    End Sub

End Class