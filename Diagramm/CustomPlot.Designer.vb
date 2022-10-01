<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CustomPlot
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CustomPlot))
        Me.ComboBox_OptParameters = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ComboBox_ObjectiveFunctions = New System.Windows.Forms.ComboBox()
        Me.Diag = New BlueM.Opt.Diagramm.Diagramm()
        Me.SuspendLayout()
        '
        'ComboBox_OptParameters
        '
        Me.ComboBox_OptParameters.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_OptParameters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_OptParameters.FormattingEnabled = True
        Me.ComboBox_OptParameters.Location = New System.Drawing.Point(167, 454)
        Me.ComboBox_OptParameters.Name = "ComboBox_OptParameters"
        Me.ComboBox_OptParameters.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox_OptParameters.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 457)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(152, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "X axis (optimization parameter):"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(131, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Y axis (objective function):"
        '
        'ComboBox_ObjectiveFunctions
        '
        Me.ComboBox_ObjectiveFunctions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_ObjectiveFunctions.FormattingEnabled = True
        Me.ComboBox_ObjectiveFunctions.Location = New System.Drawing.Point(167, 6)
        Me.ComboBox_ObjectiveFunctions.Name = "ComboBox_ObjectiveFunctions"
        Me.ComboBox_ObjectiveFunctions.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox_ObjectiveFunctions.TabIndex = 2
        '
        'Diagramm_CustomPlot
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
        Me.Diag.Axes.Bottom.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Bottom.Labels.Brush.Color = System.Drawing.Color.White
        Me.Diag.Axes.Bottom.Labels.Brush.Solid = True
        Me.Diag.Axes.Bottom.Labels.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.Bottom.Labels.Font.Bold = False
        '
        '
        '
        Me.Diag.Axes.Bottom.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.Bottom.Labels.Font.Brush.Solid = True
        Me.Diag.Axes.Bottom.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Bottom.Labels.Font.Shadow.Brush.Solid = True
        Me.Diag.Axes.Bottom.Labels.Font.Shadow.Brush.Visible = True
        Me.Diag.Axes.Bottom.Labels.Font.Size = 9
        Me.Diag.Axes.Bottom.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Axes.Bottom.Labels.ImageBevel.Brush.Solid = True
        Me.Diag.Axes.Bottom.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Bottom.Labels.Shadow.Brush.Solid = True
        Me.Diag.Axes.Bottom.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.Bottom.Title.Angle = 0
        '
        '
        '
        Me.Diag.Axes.Bottom.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Bottom.Title.Brush.Color = System.Drawing.Color.Silver
        Me.Diag.Axes.Bottom.Title.Brush.Solid = True
        Me.Diag.Axes.Bottom.Title.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.Bottom.Title.Font.Bold = False
        '
        '
        '
        Me.Diag.Axes.Bottom.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Diag.Axes.Bottom.Title.Font.Brush.Solid = True
        Me.Diag.Axes.Bottom.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Bottom.Title.Font.Shadow.Brush.Solid = True
        Me.Diag.Axes.Bottom.Title.Font.Shadow.Brush.Visible = True
        Me.Diag.Axes.Bottom.Title.Font.Size = 11
        Me.Diag.Axes.Bottom.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Axes.Bottom.Title.ImageBevel.Brush.Solid = True
        Me.Diag.Axes.Bottom.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Bottom.Title.Shadow.Brush.Solid = True
        Me.Diag.Axes.Bottom.Title.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Depth.Labels.Brush.Color = System.Drawing.Color.White
        Me.Diag.Axes.Depth.Labels.Brush.Solid = True
        Me.Diag.Axes.Depth.Labels.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.Depth.Labels.Font.Bold = False
        '
        '
        '
        Me.Diag.Axes.Depth.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.Depth.Labels.Font.Brush.Solid = True
        Me.Diag.Axes.Depth.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Depth.Labels.Font.Shadow.Brush.Solid = True
        Me.Diag.Axes.Depth.Labels.Font.Shadow.Brush.Visible = True
        Me.Diag.Axes.Depth.Labels.Font.Size = 9
        Me.Diag.Axes.Depth.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Axes.Depth.Labels.ImageBevel.Brush.Solid = True
        Me.Diag.Axes.Depth.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Depth.Labels.Shadow.Brush.Solid = True
        Me.Diag.Axes.Depth.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.Depth.Title.Angle = 0
        '
        '
        '
        Me.Diag.Axes.Depth.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Depth.Title.Brush.Color = System.Drawing.Color.Silver
        Me.Diag.Axes.Depth.Title.Brush.Solid = True
        Me.Diag.Axes.Depth.Title.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.Depth.Title.Font.Bold = False
        '
        '
        '
        Me.Diag.Axes.Depth.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Diag.Axes.Depth.Title.Font.Brush.Solid = True
        Me.Diag.Axes.Depth.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Depth.Title.Font.Shadow.Brush.Solid = True
        Me.Diag.Axes.Depth.Title.Font.Shadow.Brush.Visible = True
        Me.Diag.Axes.Depth.Title.Font.Size = 11
        Me.Diag.Axes.Depth.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Axes.Depth.Title.ImageBevel.Brush.Solid = True
        Me.Diag.Axes.Depth.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Depth.Title.Shadow.Brush.Solid = True
        Me.Diag.Axes.Depth.Title.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.DepthTop.Labels.Brush.Color = System.Drawing.Color.White
        Me.Diag.Axes.DepthTop.Labels.Brush.Solid = True
        Me.Diag.Axes.DepthTop.Labels.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.DepthTop.Labels.Font.Bold = False
        '
        '
        '
        Me.Diag.Axes.DepthTop.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.DepthTop.Labels.Font.Brush.Solid = True
        Me.Diag.Axes.DepthTop.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.DepthTop.Labels.Font.Shadow.Brush.Solid = True
        Me.Diag.Axes.DepthTop.Labels.Font.Shadow.Brush.Visible = True
        Me.Diag.Axes.DepthTop.Labels.Font.Size = 9
        Me.Diag.Axes.DepthTop.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Axes.DepthTop.Labels.ImageBevel.Brush.Solid = True
        Me.Diag.Axes.DepthTop.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.DepthTop.Labels.Shadow.Brush.Solid = True
        Me.Diag.Axes.DepthTop.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.DepthTop.Title.Angle = 0
        '
        '
        '
        Me.Diag.Axes.DepthTop.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.DepthTop.Title.Brush.Color = System.Drawing.Color.Silver
        Me.Diag.Axes.DepthTop.Title.Brush.Solid = True
        Me.Diag.Axes.DepthTop.Title.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.DepthTop.Title.Font.Bold = False
        '
        '
        '
        Me.Diag.Axes.DepthTop.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Diag.Axes.DepthTop.Title.Font.Brush.Solid = True
        Me.Diag.Axes.DepthTop.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.DepthTop.Title.Font.Shadow.Brush.Solid = True
        Me.Diag.Axes.DepthTop.Title.Font.Shadow.Brush.Visible = True
        Me.Diag.Axes.DepthTop.Title.Font.Size = 11
        Me.Diag.Axes.DepthTop.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Axes.DepthTop.Title.ImageBevel.Brush.Solid = True
        Me.Diag.Axes.DepthTop.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.DepthTop.Title.Shadow.Brush.Solid = True
        Me.Diag.Axes.DepthTop.Title.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Left.Labels.Brush.Color = System.Drawing.Color.White
        Me.Diag.Axes.Left.Labels.Brush.Solid = True
        Me.Diag.Axes.Left.Labels.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.Left.Labels.Font.Bold = False
        '
        '
        '
        Me.Diag.Axes.Left.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.Left.Labels.Font.Brush.Solid = True
        Me.Diag.Axes.Left.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Left.Labels.Font.Shadow.Brush.Solid = True
        Me.Diag.Axes.Left.Labels.Font.Shadow.Brush.Visible = True
        Me.Diag.Axes.Left.Labels.Font.Size = 9
        Me.Diag.Axes.Left.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Axes.Left.Labels.ImageBevel.Brush.Solid = True
        Me.Diag.Axes.Left.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Left.Labels.Shadow.Brush.Solid = True
        Me.Diag.Axes.Left.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.Left.Title.Angle = 90
        '
        '
        '
        Me.Diag.Axes.Left.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Left.Title.Brush.Color = System.Drawing.Color.Silver
        Me.Diag.Axes.Left.Title.Brush.Solid = True
        Me.Diag.Axes.Left.Title.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.Left.Title.Font.Bold = False
        '
        '
        '
        Me.Diag.Axes.Left.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Diag.Axes.Left.Title.Font.Brush.Solid = True
        Me.Diag.Axes.Left.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Left.Title.Font.Shadow.Brush.Solid = True
        Me.Diag.Axes.Left.Title.Font.Shadow.Brush.Visible = True
        Me.Diag.Axes.Left.Title.Font.Size = 11
        Me.Diag.Axes.Left.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Axes.Left.Title.ImageBevel.Brush.Solid = True
        Me.Diag.Axes.Left.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Left.Title.Shadow.Brush.Solid = True
        Me.Diag.Axes.Left.Title.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Right.Labels.Brush.Color = System.Drawing.Color.White
        Me.Diag.Axes.Right.Labels.Brush.Solid = True
        Me.Diag.Axes.Right.Labels.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.Right.Labels.Font.Bold = False
        '
        '
        '
        Me.Diag.Axes.Right.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.Right.Labels.Font.Brush.Solid = True
        Me.Diag.Axes.Right.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Right.Labels.Font.Shadow.Brush.Solid = True
        Me.Diag.Axes.Right.Labels.Font.Shadow.Brush.Visible = True
        Me.Diag.Axes.Right.Labels.Font.Size = 9
        Me.Diag.Axes.Right.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Axes.Right.Labels.ImageBevel.Brush.Solid = True
        Me.Diag.Axes.Right.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Right.Labels.Shadow.Brush.Solid = True
        Me.Diag.Axes.Right.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.Right.Title.Angle = 270
        '
        '
        '
        Me.Diag.Axes.Right.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Right.Title.Brush.Color = System.Drawing.Color.Silver
        Me.Diag.Axes.Right.Title.Brush.Solid = True
        Me.Diag.Axes.Right.Title.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.Right.Title.Font.Bold = False
        '
        '
        '
        Me.Diag.Axes.Right.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Diag.Axes.Right.Title.Font.Brush.Solid = True
        Me.Diag.Axes.Right.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Right.Title.Font.Shadow.Brush.Solid = True
        Me.Diag.Axes.Right.Title.Font.Shadow.Brush.Visible = True
        Me.Diag.Axes.Right.Title.Font.Size = 11
        Me.Diag.Axes.Right.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Axes.Right.Title.ImageBevel.Brush.Solid = True
        Me.Diag.Axes.Right.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Right.Title.Shadow.Brush.Solid = True
        Me.Diag.Axes.Right.Title.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Top.Labels.Brush.Color = System.Drawing.Color.White
        Me.Diag.Axes.Top.Labels.Brush.Solid = True
        Me.Diag.Axes.Top.Labels.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.Top.Labels.Font.Bold = False
        '
        '
        '
        Me.Diag.Axes.Top.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Axes.Top.Labels.Font.Brush.Solid = True
        Me.Diag.Axes.Top.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Top.Labels.Font.Shadow.Brush.Solid = True
        Me.Diag.Axes.Top.Labels.Font.Shadow.Brush.Visible = True
        Me.Diag.Axes.Top.Labels.Font.Size = 9
        Me.Diag.Axes.Top.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Axes.Top.Labels.ImageBevel.Brush.Solid = True
        Me.Diag.Axes.Top.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Top.Labels.Shadow.Brush.Solid = True
        Me.Diag.Axes.Top.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.Top.Title.Angle = 0
        '
        '
        '
        Me.Diag.Axes.Top.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Axes.Top.Title.Brush.Color = System.Drawing.Color.Silver
        Me.Diag.Axes.Top.Title.Brush.Solid = True
        Me.Diag.Axes.Top.Title.Brush.Visible = True
        '
        '
        '
        Me.Diag.Axes.Top.Title.Font.Bold = False
        '
        '
        '
        Me.Diag.Axes.Top.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Diag.Axes.Top.Title.Font.Brush.Solid = True
        Me.Diag.Axes.Top.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Top.Title.Font.Shadow.Brush.Solid = True
        Me.Diag.Axes.Top.Title.Font.Shadow.Brush.Visible = True
        Me.Diag.Axes.Top.Title.Font.Size = 11
        Me.Diag.Axes.Top.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Axes.Top.Title.ImageBevel.Brush.Solid = True
        Me.Diag.Axes.Top.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Axes.Top.Title.Shadow.Brush.Solid = True
        Me.Diag.Axes.Top.Title.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Footer.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Footer.Brush.Color = System.Drawing.Color.Silver
        Me.Diag.Footer.Brush.Solid = True
        Me.Diag.Footer.Brush.Visible = True
        '
        '
        '
        Me.Diag.Footer.Font.Bold = False
        '
        '
        '
        Me.Diag.Footer.Font.Brush.Color = System.Drawing.Color.Red
        Me.Diag.Footer.Font.Brush.Solid = True
        Me.Diag.Footer.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Footer.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Footer.Font.Shadow.Brush.Solid = True
        Me.Diag.Footer.Font.Shadow.Brush.Visible = True
        Me.Diag.Footer.Font.Size = 8
        Me.Diag.Footer.Font.SizeFloat = 8.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Footer.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Footer.ImageBevel.Brush.Solid = True
        Me.Diag.Footer.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Footer.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Footer.Shadow.Brush.Solid = True
        Me.Diag.Footer.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Header.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Header.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Diag.Header.Brush.Solid = True
        Me.Diag.Header.Brush.Visible = True
        '
        '
        '
        Me.Diag.Header.Font.Bold = False
        '
        '
        '
        Me.Diag.Header.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.Header.Font.Brush.Solid = True
        Me.Diag.Header.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Header.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Header.Font.Shadow.Brush.Solid = True
        Me.Diag.Header.Font.Shadow.Brush.Visible = True
        Me.Diag.Header.Font.Size = 12
        Me.Diag.Header.Font.SizeFloat = 12.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Header.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Header.ImageBevel.Brush.Solid = True
        Me.Diag.Header.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Header.Shadow.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.Diag.Header.Shadow.Brush.Solid = True
        Me.Diag.Header.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Legend.Brush.Color = System.Drawing.Color.White
        Me.Diag.Legend.Brush.Solid = True
        Me.Diag.Legend.Brush.Visible = True
        '
        '
        '
        Me.Diag.Legend.Font.Bold = False
        '
        '
        '
        Me.Diag.Legend.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Diag.Legend.Font.Brush.Solid = True
        Me.Diag.Legend.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Legend.Font.Shadow.Brush.Solid = True
        Me.Diag.Legend.Font.Shadow.Brush.Visible = True
        Me.Diag.Legend.Font.Size = 9
        Me.Diag.Legend.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Legend.ImageBevel.Brush.Solid = True
        Me.Diag.Legend.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Shadow.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Diag.Legend.Shadow.Brush.Solid = True
        Me.Diag.Legend.Shadow.Brush.Visible = True
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
        Me.Diag.Legend.Symbol.Shadow.Brush.Solid = True
        Me.Diag.Legend.Symbol.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Legend.Title.Brush.Color = System.Drawing.Color.White
        Me.Diag.Legend.Title.Brush.Solid = True
        Me.Diag.Legend.Title.Brush.Visible = True
        '
        '
        '
        Me.Diag.Legend.Title.Font.Bold = True
        '
        '
        '
        Me.Diag.Legend.Title.Font.Brush.Color = System.Drawing.Color.Black
        Me.Diag.Legend.Title.Font.Brush.Solid = True
        Me.Diag.Legend.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Legend.Title.Font.Shadow.Brush.Solid = True
        Me.Diag.Legend.Title.Font.Shadow.Brush.Visible = True
        Me.Diag.Legend.Title.Font.Size = 8
        Me.Diag.Legend.Title.Font.SizeFloat = 8.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Legend.Title.ImageBevel.Brush.Solid = True
        Me.Diag.Legend.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Legend.Title.Shadow.Brush.Solid = True
        Me.Diag.Legend.Title.Shadow.Brush.Visible = True
        Me.Diag.Location = New System.Drawing.Point(12, 33)
        Me.Diag.Name = "Diagramm_CustomPlot"
        '
        '
        '
        '
        '
        '
        Me.Diag.Panel.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Panel.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Diag.Panel.Brush.Solid = True
        Me.Diag.Panel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Panel.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Panel.ImageBevel.Brush.Solid = True
        Me.Diag.Panel.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Panel.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Panel.Shadow.Brush.Solid = True
        Me.Diag.Panel.Shadow.Brush.Visible = True
        Me.Diag.Size = New System.Drawing.Size(611, 415)
        '
        '
        '
        '
        '
        '
        Me.Diag.SubFooter.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.SubFooter.Brush.Color = System.Drawing.Color.Silver
        Me.Diag.SubFooter.Brush.Solid = True
        Me.Diag.SubFooter.Brush.Visible = True
        '
        '
        '
        Me.Diag.SubFooter.Font.Bold = False
        '
        '
        '
        Me.Diag.SubFooter.Font.Brush.Color = System.Drawing.Color.Red
        Me.Diag.SubFooter.Font.Brush.Solid = True
        Me.Diag.SubFooter.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.SubFooter.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.SubFooter.Font.Shadow.Brush.Solid = True
        Me.Diag.SubFooter.Font.Shadow.Brush.Visible = True
        Me.Diag.SubFooter.Font.Size = 8
        Me.Diag.SubFooter.Font.SizeFloat = 8.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.SubFooter.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.SubFooter.ImageBevel.Brush.Solid = True
        Me.Diag.SubFooter.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.SubFooter.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.SubFooter.Shadow.Brush.Solid = True
        Me.Diag.SubFooter.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.SubHeader.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.SubHeader.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Diag.SubHeader.Brush.Solid = True
        Me.Diag.SubHeader.Brush.Visible = True
        '
        '
        '
        Me.Diag.SubHeader.Font.Bold = False
        '
        '
        '
        Me.Diag.SubHeader.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Diag.SubHeader.Font.Brush.Solid = True
        Me.Diag.SubHeader.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.SubHeader.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.SubHeader.Font.Shadow.Brush.Solid = True
        Me.Diag.SubHeader.Font.Shadow.Brush.Visible = True
        Me.Diag.SubHeader.Font.Size = 12
        Me.Diag.SubHeader.Font.SizeFloat = 12.0!
        '
        '
        '
        '
        '
        '
        Me.Diag.SubHeader.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.SubHeader.ImageBevel.Brush.Solid = True
        Me.Diag.SubHeader.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.SubHeader.Shadow.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.Diag.SubHeader.Shadow.Brush.Solid = True
        Me.Diag.SubHeader.Shadow.Brush.Visible = True
        Me.Diag.TabIndex = 4
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Back.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Walls.Back.Brush.Color = System.Drawing.Color.Silver
        Me.Diag.Walls.Back.Brush.Solid = True
        Me.Diag.Walls.Back.Brush.Visible = False
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Back.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Walls.Back.ImageBevel.Brush.Solid = True
        Me.Diag.Walls.Back.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Back.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Walls.Back.Shadow.Brush.Solid = True
        Me.Diag.Walls.Back.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Bottom.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Walls.Bottom.Brush.Color = System.Drawing.Color.White
        Me.Diag.Walls.Bottom.Brush.Solid = True
        Me.Diag.Walls.Bottom.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Bottom.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Walls.Bottom.ImageBevel.Brush.Solid = True
        Me.Diag.Walls.Bottom.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Bottom.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Walls.Bottom.Shadow.Brush.Solid = True
        Me.Diag.Walls.Bottom.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Left.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Walls.Left.Brush.Color = System.Drawing.Color.LightYellow
        Me.Diag.Walls.Left.Brush.Solid = True
        Me.Diag.Walls.Left.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Left.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Walls.Left.ImageBevel.Brush.Solid = True
        Me.Diag.Walls.Left.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Left.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Walls.Left.Shadow.Brush.Solid = True
        Me.Diag.Walls.Left.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Right.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Diag.Walls.Right.Brush.Color = System.Drawing.Color.LightYellow
        Me.Diag.Walls.Right.Brush.Solid = True
        Me.Diag.Walls.Right.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Right.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Diag.Walls.Right.ImageBevel.Brush.Solid = True
        Me.Diag.Walls.Right.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Right.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Diag.Walls.Right.Shadow.Brush.Solid = True
        Me.Diag.Walls.Right.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Diag.Zoom.Brush.Color = System.Drawing.Color.LightBlue
        Me.Diag.Zoom.Brush.Solid = True
        Me.Diag.Zoom.Brush.Visible = True
        '
        'CustomPlot
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(635, 487)
        Me.Controls.Add(Me.Diag)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ComboBox_ObjectiveFunctions)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboBox_OptParameters)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "CustomPlot"
        Me.Text = "Custom Plot"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ComboBox_OptParameters As Windows.Forms.ComboBox
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents ComboBox_ObjectiveFunctions As Windows.Forms.ComboBox
    Friend WithEvents Diag As Diagramm
End Class
