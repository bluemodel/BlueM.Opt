Public Class Indicatordiagramm
    Inherits Diagramm

    'Ergebnisse der Hypervolumenberechnung anzeigen
    '**********************************************
    Public Sub ZeichneHyperVolumen(ByVal gen As Integer, ByVal indicator As Double)

        'Indicator in Indikatordiagramm eintragen
        Dim serie1 As Steema.TeeChart.Styles.Line
        serie1 = Me.getSeriesLine("Hypervolume")
        serie1.Add(gen, indicator, gen.ToString())

    End Sub

End Class
