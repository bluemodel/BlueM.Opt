<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DiagrammForm
    Inherits System.Windows.Forms.UserControl

    'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.GroupBox_TChartButtons = New System.Windows.Forms.GroupBox
        Me.Button_TChartEdit = New System.Windows.Forms.Button
        Me.Button_TChart2PNG = New System.Windows.Forms.Button
        Me.Button_TChartSave = New System.Windows.Forms.Button
        Me.Button_TChart2Excel = New System.Windows.Forms.Button
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.DiagIndicator = New IHWB.EVO.Diagramm
        Me.Line_Hypervolume = New Steema.TeeChart.Styles.Line
        Me.MarksTip_Hypervolume = New Steema.TeeChart.Tools.MarksTip
        Me.Diag = New IHWB.EVO.Diagramm
        Me.GroupBox_TChartButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox_TChartButtons
        '
        Me.GroupBox_TChartButtons.Controls.Add(Me.Button_TChartEdit)
        Me.GroupBox_TChartButtons.Controls.Add(Me.Button_TChart2PNG)
        Me.GroupBox_TChartButtons.Controls.Add(Me.Button_TChartSave)
        Me.GroupBox_TChartButtons.Controls.Add(Me.Button_TChart2Excel)
        Me.GroupBox_TChartButtons.Location = New System.Drawing.Point(3, 570)
        Me.GroupBox_TChartButtons.Name = "GroupBox_TChartButtons"
        Me.GroupBox_TChartButtons.Size = New System.Drawing.Size(143, 50)
        Me.GroupBox_TChartButtons.TabIndex = 5
        Me.GroupBox_TChartButtons.TabStop = False
        Me.GroupBox_TChartButtons.Text = "Diagramm"
        '
        'Button_TChartEdit
        '
        Me.Button_TChartEdit.Image = Global.IHWB.EVO.My.Resources.Resources.icon_edit
        Me.Button_TChartEdit.Location = New System.Drawing.Point(6, 19)
        Me.Button_TChartEdit.Name = "Button_TChartEdit"
        Me.Button_TChartEdit.Size = New System.Drawing.Size(25, 25)
        Me.Button_TChartEdit.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.Button_TChartEdit, "Diagramm bearbeiten")
        Me.Button_TChartEdit.UseVisualStyleBackColor = True
        '
        'Button_TChart2PNG
        '
        Me.Button_TChart2PNG.Image = Global.IHWB.EVO.My.Resources.Resources.icon_png
        Me.Button_TChart2PNG.Location = New System.Drawing.Point(111, 19)
        Me.Button_TChart2PNG.Name = "Button_TChart2PNG"
        Me.Button_TChart2PNG.Size = New System.Drawing.Size(25, 25)
        Me.Button_TChart2PNG.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.Button_TChart2PNG, "als PNG exportieren")
        Me.Button_TChart2PNG.UseVisualStyleBackColor = True
        '
        'Button_TChartSave
        '
        Me.Button_TChartSave.Image = Global.IHWB.EVO.My.Resources.Resources.icon_teechart
        Me.Button_TChartSave.Location = New System.Drawing.Point(80, 19)
        Me.Button_TChartSave.Name = "Button_TChartSave"
        Me.Button_TChartSave.Size = New System.Drawing.Size(25, 25)
        Me.Button_TChartSave.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.Button_TChartSave, "als natives TeeChart-Format exportieren")
        Me.Button_TChartSave.UseVisualStyleBackColor = True
        '
        'Button_TChart2Excel
        '
        Me.Button_TChart2Excel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.Button_TChart2Excel.Image = Global.IHWB.EVO.My.Resources.Resources.icon_excel
        Me.Button_TChart2Excel.Location = New System.Drawing.Point(49, 19)
        Me.Button_TChart2Excel.Name = "Button_TChart2Excel"
        Me.Button_TChart2Excel.Size = New System.Drawing.Size(25, 25)
        Me.Button_TChart2Excel.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.Button_TChart2Excel, "nach Excel exportieren")
        Me.Button_TChart2Excel.UseVisualStyleBackColor = False
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Title = "Datei speichern"
        '
        'DiagIndicator
        '
        '
        '
        '
        Me.DiagIndicator.Aspect.ElevationFloat = 345
        Me.DiagIndicator.Aspect.RotationFloat = 345
        Me.DiagIndicator.Aspect.View3D = False
        '
        '
        '
        '
        '
        '
        Me.DiagIndicator.Axes.Bottom.Automatic = True
        '
        '
        '
        Me.DiagIndicator.Axes.Bottom.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.DiagIndicator.Axes.Bottom.Grid.ZPosition = 0
        '
        '
        '
        '
        '
        '
        Me.DiagIndicator.Axes.Bottom.Labels.Font.Size = 6
        Me.DiagIndicator.Axes.Bottom.Labels.Font.SizeFloat = 6.0!
        Me.DiagIndicator.Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
        '
        '
        '
        Me.DiagIndicator.Axes.Depth.Automatic = True
        '
        '
        '
        Me.DiagIndicator.Axes.Depth.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.DiagIndicator.Axes.Depth.Grid.ZPosition = 0
        '
        '
        '
        Me.DiagIndicator.Axes.DepthTop.Automatic = True
        '
        '
        '
        Me.DiagIndicator.Axes.DepthTop.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.DiagIndicator.Axes.DepthTop.Grid.ZPosition = 0
        '
        '
        '
        Me.DiagIndicator.Axes.Left.Automatic = True
        '
        '
        '
        Me.DiagIndicator.Axes.Left.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.DiagIndicator.Axes.Left.Grid.ZPosition = 0
        '
        '
        '
        '
        '
        '
        Me.DiagIndicator.Axes.Left.Labels.Font.Size = 6
        Me.DiagIndicator.Axes.Left.Labels.Font.SizeFloat = 6.0!
        Me.DiagIndicator.Axes.Left.MaximumOffset = 1
        '
        '
        '
        Me.DiagIndicator.Axes.Right.Automatic = True
        '
        '
        '
        Me.DiagIndicator.Axes.Right.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.DiagIndicator.Axes.Right.Grid.ZPosition = 0
        '
        '
        '
        Me.DiagIndicator.Axes.Top.Automatic = True
        '
        '
        '
        Me.DiagIndicator.Axes.Top.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.DiagIndicator.Axes.Top.Grid.ZPosition = 0
        Me.DiagIndicator.Cursor = System.Windows.Forms.Cursors.Default
        '
        '
        '
        Me.DiagIndicator.Header.Lines = New String() {"TeeChart"}
        Me.DiagIndicator.Header.Visible = False
        '
        '
        '
        '
        '
        '
        Me.DiagIndicator.Legend.Shadow.Visible = True
        '
        '
        '
        '
        '
        '
        Me.DiagIndicator.Legend.Title.Font.Bold = True
        '
        '
        '
        Me.DiagIndicator.Legend.Title.Pen.Visible = False
        Me.DiagIndicator.Legend.Visible = False
        Me.DiagIndicator.Location = New System.Drawing.Point(0, 496)
        Me.DiagIndicator.Name = "DiagIndicator"
        '
        '
        '
        Me.DiagIndicator.Panel.MarginTop = 20
        '
        '
        '
        Me.DiagIndicator.Panning.Allow = Steema.TeeChart.ScrollModes.None
        Me.DiagIndicator.Series.Add(Me.Line_Hypervolume)
        Me.DiagIndicator.Size = New System.Drawing.Size(473, 70)
        Me.DiagIndicator.TabIndex = 6
        Me.DiagIndicator.Tools.Add(Me.MarksTip_Hypervolume)
        Me.ToolTip1.SetToolTip(Me.DiagIndicator, "Hypervolumen")
        Me.DiagIndicator.Visible = False
        '
        '
        '
        '
        '
        '
        Me.DiagIndicator.Walls.Back.AutoHide = False
        '
        '
        '
        Me.DiagIndicator.Walls.Bottom.AutoHide = False
        '
        '
        '
        Me.DiagIndicator.Walls.Left.AutoHide = False
        '
        '
        '
        Me.DiagIndicator.Walls.Right.AutoHide = False
        '
        'Line_Hypervolume
        '
        '
        '
        '
        Me.Line_Hypervolume.Brush.Color = System.Drawing.Color.Red
        '
        '
        '
        Me.Line_Hypervolume.LinePen.Color = System.Drawing.Color.FromArgb(CType(CType(153, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Line_Hypervolume.Marks.Callout.ArrowHead = Steema.TeeChart.Styles.ArrowHeadStyles.None
        Me.Line_Hypervolume.Marks.Callout.ArrowHeadSize = 8
        '
        '
        '
        Me.Line_Hypervolume.Marks.Callout.Brush.Color = System.Drawing.Color.Black
        Me.Line_Hypervolume.Marks.Callout.Distance = 0
        Me.Line_Hypervolume.Marks.Callout.Draw3D = False
        Me.Line_Hypervolume.Marks.Callout.Length = 10
        Me.Line_Hypervolume.Marks.Callout.Style = Steema.TeeChart.Styles.PointerStyles.Rectangle
        '
        '
        '
        Me.Line_Hypervolume.Marks.Shadow.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Line_Hypervolume.Marks.Symbol.Shadow.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Line_Hypervolume.Pointer.Brush.Color = System.Drawing.Color.Red
        Me.Line_Hypervolume.Pointer.Dark3D = False
        Me.Line_Hypervolume.Pointer.Draw3D = False
        Me.Line_Hypervolume.Pointer.HorizSize = 2
        '
        '
        '
        Me.Line_Hypervolume.Pointer.Pen.Visible = False
        Me.Line_Hypervolume.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
        Me.Line_Hypervolume.Pointer.VertSize = 2
        Me.Line_Hypervolume.Pointer.Visible = True
        Me.Line_Hypervolume.Title = "Hypervolume"
        '
        '
        '
        Me.Line_Hypervolume.XValues.DataMember = "X"
        Me.Line_Hypervolume.XValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending
        '
        '
        '
        Me.Line_Hypervolume.YValues.DataMember = "Y"
        '
        'MarksTip_Hypervolume
        '
        Me.MarksTip_Hypervolume.Style = Steema.TeeChart.Styles.MarksStyles.XY
        '
        'Diag
        '
        '
        '
        '
        Me.Diag.Aspect.ElevationFloat = 345
        Me.Diag.Aspect.RotationFloat = 345
        Me.Diag.Aspect.View3D = False
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Automatic = True
        '
        '
        '
        Me.Diag.Axes.Bottom.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.Diag.Axes.Bottom.Grid.ZPosition = 0
        '
        '
        '
        Me.Diag.Axes.Depth.Automatic = True
        '
        '
        '
        Me.Diag.Axes.Depth.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.Diag.Axes.Depth.Grid.ZPosition = 0
        '
        '
        '
        Me.Diag.Axes.DepthTop.Automatic = True
        '
        '
        '
        Me.Diag.Axes.DepthTop.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.Diag.Axes.DepthTop.Grid.ZPosition = 0
        '
        '
        '
        Me.Diag.Axes.Left.Automatic = True
        '
        '
        '
        Me.Diag.Axes.Left.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.Diag.Axes.Left.Grid.ZPosition = 0
        '
        '
        '
        Me.Diag.Axes.Right.Automatic = True
        '
        '
        '
        Me.Diag.Axes.Right.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.Diag.Axes.Right.Grid.ZPosition = 0
        '
        '
        '
        Me.Diag.Axes.Top.Automatic = True
        '
        '
        '
        Me.Diag.Axes.Top.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.Diag.Axes.Top.Grid.ZPosition = 0
        Me.Diag.Cursor = System.Windows.Forms.Cursors.Default
        '
        '
        '
        Me.Diag.Header.Lines = New String() {"EVO"}
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Shadow.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Title.Font.Bold = True
        '
        '
        '
        Me.Diag.Legend.Title.Pen.Visible = False
        Me.Diag.Location = New System.Drawing.Point(0, 0)
        Me.Diag.Name = "Diag"
        Me.Diag.Size = New System.Drawing.Size(473, 566)
        Me.Diag.TabIndex = 0
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Back.AutoHide = False
        '
        '
        '
        Me.Diag.Walls.Bottom.AutoHide = False
        '
        '
        '
        Me.Diag.Walls.Left.AutoHide = False
        '
        '
        '
        Me.Diag.Walls.Right.AutoHide = False
        Me.Diag.Walls.View3D = False
        '
        'DiagrammForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.DiagIndicator)
        Me.Controls.Add(Me.GroupBox_TChartButtons)
        Me.Controls.Add(Me.Diag)
        Me.Name = "DiagrammForm"
        Me.Size = New System.Drawing.Size(473, 625)
        Me.GroupBox_TChartButtons.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Diag As EVO.Diagramm
    Friend WithEvents GroupBox_TChartButtons As System.Windows.Forms.GroupBox
    Friend WithEvents Button_TChartEdit As System.Windows.Forms.Button
    Friend WithEvents Button_TChart2PNG As System.Windows.Forms.Button
    Friend WithEvents Button_TChartSave As System.Windows.Forms.Button
    Friend WithEvents Button_TChart2Excel As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents DiagIndicator As EVO.Diagramm
    Friend WithEvents Line_Hypervolume As Steema.TeeChart.Styles.Line
    Friend WithEvents MarksTip_Hypervolume As Steema.TeeChart.Tools.MarksTip

End Class
