Public Class Indicatordiagramm
    Inherits Diagramm

    Private Line_Hypervolume As Steema.TeeChart.Styles.Line

    'Konstruktor
    '***********
    Public Sub New()

        'Hypervolume-Linie initialisieren
        Me.Line_Hypervolume = New Steema.TeeChart.Styles.Line(Me.Chart)
        Me.Line_Hypervolume.Title = "Hypervolume"
        Me.Line_Hypervolume.Color = System.Drawing.Color.Red
        Me.Line_Hypervolume.Pointer.Visible = True
        Me.Line_Hypervolume.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
        Me.Line_Hypervolume.Pointer.Brush.Color = System.Drawing.Color.Red
        Me.Line_Hypervolume.Pointer.HorizSize = 2
        Me.Line_Hypervolume.Pointer.VertSize = 2
        Me.Line_Hypervolume.Pointer.Pen.Visible = False

    End Sub

    'Ergebnisse der Hypervolumenberechnung anzeigen
    '**********************************************
    Public Sub ZeichneHyperVolumen(ByVal gen As Integer, ByVal indicator As Double)

        'Indicator in Indikatordiagramm eintragen
        Me.Line_Hypervolume.Add(gen, indicator, gen.ToString())

    End Sub

End Class
