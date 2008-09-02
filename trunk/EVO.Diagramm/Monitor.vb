Partial Public Class Monitor
    Inherits System.Windows.Forms.Form

    Private WithEvents Monitordiagramm As Steema.TeeChart.TChart

    Private Line_Hypervolume As Steema.TeeChart.Styles.Line
    Private Line_Dn As Steema.TeeChart.Styles.Line

    Public Event MonitorClosed()

    'Konstruktor
    '***********
    Public Sub New()

        Call InitializeComponent()

        'Achsen
        '------
        'Durchlaufachse
        Me.Monitordiagramm.Axes.Bottom.Title.Caption = "Durchlauf"

        'Schrittweitenachse
        Me.Monitordiagramm.Axes.Left.Title.Caption = "Schrittweite"

        'Generationsachse (oben)
        Me.Monitordiagramm.Axes.Top.Visible = True
        Me.Monitordiagramm.Axes.Top.Title.Caption = "Generation"
        Me.Monitordiagramm.Axes.Top.Horizontal = True
        Me.Monitordiagramm.Axes.Top.Automatic = True
        Me.Monitordiagramm.Axes.Top.Grid.Visible = False

        'Hypervolumenachse (rechts)
        Me.Monitordiagramm.Axes.Right.Visible = True
        Me.Monitordiagramm.Axes.Right.Title.Caption = "Hypervolumen"
        Me.Monitordiagramm.Axes.Right.Title.Angle = 90
        Me.Monitordiagramm.Axes.Right.Automatic = True
        Me.Monitordiagramm.Axes.Right.Grid.Visible = False

        'Linien/Serien
        '-------------
        'Dn Verlauf initialisieren
        Me.Line_Dn = New Steema.TeeChart.Styles.Line(Me.Monitordiagramm.Chart)
        Me.Line_Dn.Title = "Schrittweite Dn"
        Me.Line_Dn.Color = System.Drawing.Color.Blue
        Me.Line_Dn.Pointer.Visible = True
        Me.Line_Dn.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
        Me.Line_Dn.Pointer.Brush.Color = System.Drawing.Color.Blue
        Me.Line_Dn.Pointer.HorizSize = 2
        Me.Line_Dn.Pointer.VertSize = 2
        Me.Line_Dn.Pointer.Pen.Visible = False

        'Hypervolume-Linie initialisieren
        Me.Line_Hypervolume = New Steema.TeeChart.Styles.Line(Me.Monitordiagramm.Chart)
        Me.Line_Hypervolume.Title = "Hypervolumen"
        Me.Line_Hypervolume.CustomHorizAxis = Me.Monitordiagramm.Axes.Top
        Me.Line_Hypervolume.CustomVertAxis = Me.Monitordiagramm.Axes.Right
        Me.Line_Hypervolume.Color = System.Drawing.Color.Red
        Me.Line_Hypervolume.Pointer.Visible = True
        Me.Line_Hypervolume.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
        Me.Line_Hypervolume.Pointer.Brush.Color = System.Drawing.Color.Red
        Me.Line_Hypervolume.Pointer.HorizSize = 2
        Me.Line_Hypervolume.Pointer.VertSize = 2
        Me.Line_Hypervolume.Pointer.Pen.Visible = False

    End Sub

#Region "UI"

    'Diagramm bearbeiten
    '-------------------
    Private Sub Monitordiagramm_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Monitordiagramm.DoubleClick
        Call Me.Monitordiagramm.ShowEditor()
    End Sub

    'Form schließen
    '**************
    Private Sub Monitor_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        'verhindern, dass das Form komplett gelöscht wird
        e.Cancel = True

        'Dialog verstecken
        Call Me.Hide()

        'Event auslösen (wird von Form1 verarbeitet)
        RaiseEvent MonitorClosed()

    End Sub

    'Form resize
    '***********
    Private Sub Monitor_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        Me.Monitordiagramm.Width = Me.ClientSize.Width
        Me.Monitordiagramm.Height = Me.ClientSize.Height
    End Sub

#End Region

    'Ergebnisse der Hypervolumenberechnung anzeigen
    '**********************************************
    Public Sub ZeichneHyperVolumen(ByVal gen As Integer, ByVal indicator As Double)

        'Indicator eintragen
        Me.Line_Hypervolume.Add(gen, indicator, gen.ToString())

    End Sub

    'Anzeige eines Referenz-Hypervolumens als ColorLine
    Public Sub ZeichneReferenzHypervolumen(ByVal indicatorRef As Double)

        Dim colorline1 As New Steema.TeeChart.Tools.ColorLine(Me.Monitordiagramm.Chart)
        colorline1.Pen.Color = System.Drawing.Color.Red
        colorline1.Pen.Width = 2
        colorline1.AllowDrag = False
        colorline1.Axis = Me.Monitordiagramm.Axes.Right
        colorline1.Value = indicatorRef

    End Sub

    'Dn's anzeigen
    '*************
    Public Sub Zeichne_Dn(ByVal durchlauf As Integer, ByVal Dn As Double)

        'Dn-Wert eintragen
        Me.Line_Dn.Add(durchlauf, Dn, durchlauf.ToString())

    End Sub

End Class