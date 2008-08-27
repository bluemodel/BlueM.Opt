Public Class Indicatordiagramm
    Inherits Diagramm

    Private Line_Hypervolume As Steema.TeeChart.Styles.Line
    Private Line_Dn As Steema.TeeChart.Styles.Line

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

        'Neue Achsen
        Dim sec_x_axis As Steema.TeeChart.Axis
        sec_x_axis = Steema.TeeChart.Axes.CreateNewAxis(Me.Chart)
        sec_x_axis.Horizontal = True
        sec_x_axis.Visible = False
        sec_x_axis.OtherSide = True
        sec_x_axis.Automatic = True

        Dim sec_y_axis As Steema.TeeChart.Axis
        sec_y_axis = Steema.TeeChart.Axes.CreateNewAxis(Me.Chart)
        sec_y_axis.Visible = True
        sec_y_axis.OtherSide = True
        sec_y_axis.Automatic = True

        'Dn Verlauf initialisieren
        Me.Line_Dn = New Steema.TeeChart.Styles.Line(Me.Chart)
        Me.Line_Dn.CustomHorizAxis = sec_x_axis
        Me.Line_Dn.CustomVertAxis = sec_y_axis
        Me.Line_Dn.Title = "Dn"
        Me.Line_Dn.Color = System.Drawing.Color.Blue
        Me.Line_Dn.Pointer.Visible = True
        Me.Line_Dn.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
        Me.Line_Dn.Pointer.Brush.Color = System.Drawing.Color.Blue
        Me.Line_Dn.Pointer.HorizSize = 2
        Me.Line_Dn.Pointer.VertSize = 2
        Me.Line_Dn.Pointer.Pen.Visible = False

    End Sub

    'Ergebnisse der Hypervolumenberechnung anzeigen
    '**********************************************
    Public Sub ZeichneHyperVolumen(ByVal gen As Integer, ByVal indicator As Double)

        'Indicator in Indikatordiagramm eintragen
        Me.Line_Hypervolume.Add(gen, indicator, gen.ToString())

    End Sub

    'Dn's anzeigen
    '*************
    Public Sub Zeichne_Dn(ByVal durchlauf As Integer, ByVal Dn As Double)

        'Indicator in Indikatordiagramm eintragen
        Me.Line_Dn.Add(durchlauf, Dn, durchlauf.ToString())

    End Sub

End Class
