<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Monitor
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim Margins1 As Steema.TeeChart.Margins = New Steema.TeeChart.Margins()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Monitor))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage_Diagramm = New System.Windows.Forms.TabPage()
        Me.Diag = New BlueM.Opt.Diagramm.Diagramm()
        Me.TabPage_Log = New System.Windows.Forms.TabPage()
        Me.TextBox_Log = New System.Windows.Forms.TextBox()
        Me.TabControl1.SuspendLayout()
        Me.TabPage_Diagramm.SuspendLayout()
        Me.TabPage_Log.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage_Diagramm)
        Me.TabControl1.Controls.Add(Me.TabPage_Log)
        Me.TabControl1.Location = New System.Drawing.Point(0, 2)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(496, 465)
        Me.TabControl1.TabIndex = 9
        '
        'TabPage_Diagramm
        '
        Me.TabPage_Diagramm.Controls.Add(Me.Diag)
        Me.TabPage_Diagramm.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Diagramm.Name = "TabPage_Diagramm"
        Me.TabPage_Diagramm.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_Diagramm.Size = New System.Drawing.Size(488, 439)
        Me.TabPage_Diagramm.TabIndex = 0
        Me.TabPage_Diagramm.Text = "Chart"
        Me.TabPage_Diagramm.UseVisualStyleBackColor = True
        '
        'Diag
        '
        Me.Diag.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Grid.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Axes.Bottom.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.Bottom.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Bottom.Labels.Brush.Color = System.Drawing.Color.White
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Bottom.Labels.Font.Size = 9
        Me.Diag.Axes.Bottom.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
        Me.Diag.Axes.Bottom.MaximumOffset = 3
        Me.Diag.Axes.Bottom.MinimumOffset = 3
        '
        '
        '
        Me.Diag.Axes.Bottom.Title.Angle = 0
        '
        '
        '
        Me.Diag.Axes.Bottom.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Axes.Bottom.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.Bottom.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Bottom.Title.Brush.Color = System.Drawing.Color.Silver
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Bottom.Title.Font.Size = 11
        Me.Diag.Axes.Bottom.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Axes.Depth.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.Depth.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Depth.Labels.Brush.Color = System.Drawing.Color.White
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Depth.Labels.Font.Size = 9
        Me.Diag.Axes.Depth.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        '
        '
        '
        Me.Diag.Axes.Depth.Title.Angle = 0
        '
        '
        '
        Me.Diag.Axes.Depth.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Axes.Depth.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.Depth.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Depth.Title.Brush.Color = System.Drawing.Color.Silver
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Depth.Title.Font.Size = 11
        Me.Diag.Axes.Depth.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Axes.DepthTop.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.DepthTop.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.DepthTop.Labels.Brush.Color = System.Drawing.Color.White
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.DepthTop.Labels.Font.Size = 9
        Me.Diag.Axes.DepthTop.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        '
        '
        '
        Me.Diag.Axes.DepthTop.Title.Angle = 0
        '
        '
        '
        Me.Diag.Axes.DepthTop.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Axes.DepthTop.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.DepthTop.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.DepthTop.Title.Brush.Color = System.Drawing.Color.Silver
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.DepthTop.Title.Font.Size = 11
        Me.Diag.Axes.DepthTop.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Axes.Left.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.Left.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Left.Labels.Brush.Color = System.Drawing.Color.White
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Left.Labels.Font.Size = 9
        Me.Diag.Axes.Left.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Left.MaximumOffset = 3
        Me.Diag.Axes.Left.MinimumOffset = 3
        '
        '
        '
        Me.Diag.Axes.Left.Title.Angle = 90
        '
        '
        '
        Me.Diag.Axes.Left.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Axes.Left.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.Left.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Left.Title.Brush.Color = System.Drawing.Color.Silver
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Left.Title.Font.Size = 11
        Me.Diag.Axes.Left.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Axes.Right.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.Right.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Right.Labels.Brush.Color = System.Drawing.Color.White
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Right.Labels.Font.Size = 9
        Me.Diag.Axes.Right.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        '
        '
        '
        Me.Diag.Axes.Right.Title.Angle = 270
        '
        '
        '
        Me.Diag.Axes.Right.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Axes.Right.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.Right.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Right.Title.Brush.Color = System.Drawing.Color.Silver
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Right.Title.Font.Size = 11
        Me.Diag.Axes.Right.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Right.Visible = False
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Axes.Top.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.Top.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Top.Labels.Brush.Color = System.Drawing.Color.White
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Top.Labels.Font.Size = 9
        Me.Diag.Axes.Top.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        '
        '
        '
        Me.Diag.Axes.Top.Title.Angle = 0
        '
        '
        '
        Me.Diag.Axes.Top.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Axes.Top.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.Top.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Top.Title.Brush.Color = System.Drawing.Color.Silver
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Top.Title.Font.Size = 11
        Me.Diag.Axes.Top.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Top.Visible = False
        Me.Diag.BackColor = System.Drawing.Color.Transparent
        Me.Diag.Cursor = System.Windows.Forms.Cursors.Default
        '
        '
        '
        '
        '
        '
        Me.Diag.Footer.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Footer.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Footer.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Footer.Brush.Color = System.Drawing.Color.Silver
        '
        '
        '
        '
        '
        '
        Me.Diag.Footer.Font.Brush.Color = System.Drawing.Color.Red
        '
        '
        '
        '
        '
        '
        Me.Diag.Footer.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Footer.Font.Size = 8
        Me.Diag.Footer.Font.SizeFloat = 8.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Footer.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Footer.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Header.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Header.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Header.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Header.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Diag.Header.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Diag.Header.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Header.Font.Size = 12
        Me.Diag.Header.Font.SizeFloat = 12.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Header.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Header.Shadow.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.Diag.Header.Visible = False
        '
        '
        '
        Me.Diag.Legend.Alignment = Steema.TeeChart.LegendAlignments.Bottom
        '
        '
        '
        Me.Diag.Legend.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Legend.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Legend.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Legend.Brush.Color = System.Drawing.Color.White
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Legend.Font.Size = 9
        Me.Diag.Legend.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Series
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Shadow.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Symbol.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Legend.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Legend.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Legend.Title.Brush.Color = System.Drawing.Color.White
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Title.Font.Brush.Color = System.Drawing.Color.Black
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Legend.Title.Font.Size = 8
        Me.Diag.Legend.Title.Font.SizeFloat = 8.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Location = New System.Drawing.Point(0, 0)
        Me.Diag.Name = "Diag"
        '
        '
        '
        '
        '
        '
        Me.Diag.Panel.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Panel.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Panel.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Panel.Brush.Color = System.Drawing.Color.Transparent
        '
        '
        '
        '
        '
        '
        Me.Diag.Panel.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Panel.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        '
        '
        '
        Me.Diag.Panning.Allow = Steema.TeeChart.ScrollModes.None
        '
        '
        '
        Margins1.Bottom = 100
        Margins1.Left = 100
        Margins1.Right = 100
        Margins1.Top = 100
        Me.Diag.Printer.Margins = Margins1
        Me.Diag.Size = New System.Drawing.Size(488, 439)
        '
        '
        '
        '
        '
        '
        Me.Diag.SubFooter.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.SubFooter.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.SubFooter.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.SubFooter.Brush.Color = System.Drawing.Color.Silver
        '
        '
        '
        '
        '
        '
        Me.Diag.SubFooter.Font.Brush.Color = System.Drawing.Color.Red
        '
        '
        '
        '
        '
        '
        Me.Diag.SubFooter.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.SubFooter.Font.Size = 8
        Me.Diag.SubFooter.Font.SizeFloat = 8.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.SubFooter.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.SubFooter.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        '
        '
        '
        '
        '
        '
        Me.Diag.SubHeader.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.SubHeader.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.SubHeader.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.SubHeader.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Diag.SubHeader.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        '
        '
        '
        '
        '
        '
        Me.Diag.SubHeader.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.SubHeader.Font.Size = 12
        Me.Diag.SubHeader.Font.SizeFloat = 12.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.SubHeader.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.SubHeader.Shadow.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.Diag.TabIndex = 8
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Back.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Walls.Back.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Walls.Back.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Walls.Back.Brush.Color = System.Drawing.Color.Silver
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Back.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Back.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Bottom.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Walls.Bottom.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Walls.Bottom.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Walls.Bottom.Brush.Color = System.Drawing.Color.White
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Bottom.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Bottom.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Left.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Walls.Left.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Walls.Left.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Walls.Left.Brush.Color = System.Drawing.Color.LightYellow
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Left.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Left.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Right.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Walls.Right.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Walls.Right.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Walls.Right.Brush.Color = System.Drawing.Color.LightYellow
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Right.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Right.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        '
        '
        '
        '
        '
        '
        Me.Diag.Zoom.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(255, Byte), Integer))
        '
        'TabPage_Log
        '
        Me.TabPage_Log.Controls.Add(Me.TextBox_Log)
        Me.TabPage_Log.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Log.Name = "TabPage_Log"
        Me.TabPage_Log.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_Log.Size = New System.Drawing.Size(488, 439)
        Me.TabPage_Log.TabIndex = 1
        Me.TabPage_Log.Text = "Log"
        Me.TabPage_Log.UseVisualStyleBackColor = True
        '
        'TextBox_Log
        '
        Me.TextBox_Log.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_Log.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox_Log.Location = New System.Drawing.Point(6, 6)
        Me.TextBox_Log.Multiline = True
        Me.TextBox_Log.Name = "TextBox_Log"
        Me.TextBox_Log.ReadOnly = True
        Me.TextBox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox_Log.Size = New System.Drawing.Size(476, 427)
        Me.TextBox_Log.TabIndex = 0
        '
        'Monitor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(492, 466)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Monitor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "BlueM.Opt Monitor"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage_Diagramm.ResumeLayout(False)
        Me.TabPage_Log.ResumeLayout(False)
        Me.TabPage_Log.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents TabControl1 As System.Windows.Forms.TabControl
    Private WithEvents TabPage_Diagramm As System.Windows.Forms.TabPage
    Private WithEvents TabPage_Log As System.Windows.Forms.TabPage
    Private WithEvents TextBox_Log As System.Windows.Forms.TextBox
End Class
