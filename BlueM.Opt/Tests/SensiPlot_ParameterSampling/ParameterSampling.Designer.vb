<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ParameterSampling
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
        Me.TChart1 = New Steema.TeeChart.TChart()
        Me.Button_Generate = New System.Windows.Forms.Button()
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TChart1
        '
        Me.TChart1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
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
        Me.TChart1.Axes.Bottom.Grid.DrawEvery = 1
        Me.TChart1.Axes.Bottom.Grid.Visible = True
        Me.TChart1.Axes.Bottom.Increment = 0.1R
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Bottom.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Axes.Bottom.Labels.Brush.Color = System.Drawing.Color.White
        Me.TChart1.Axes.Bottom.Labels.Brush.Solid = True
        Me.TChart1.Axes.Bottom.Labels.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Axes.Bottom.Labels.Font.Bold = False
        '
        '
        '
        Me.TChart1.Axes.Bottom.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.TChart1.Axes.Bottom.Labels.Font.Brush.Solid = True
        Me.TChart1.Axes.Bottom.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Bottom.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Bottom.Labels.Font.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Bottom.Labels.Font.Shadow.Brush.Visible = True
        Me.TChart1.Axes.Bottom.Labels.Font.Size = 9
        Me.TChart1.Axes.Bottom.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Bottom.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Axes.Bottom.Labels.ImageBevel.Brush.Solid = True
        Me.TChart1.Axes.Bottom.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Bottom.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Bottom.Labels.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Bottom.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Axes.Bottom.Title.Angle = 0
        '
        '
        '
        Me.TChart1.Axes.Bottom.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Axes.Bottom.Title.Brush.Color = System.Drawing.Color.Silver
        Me.TChart1.Axes.Bottom.Title.Brush.Solid = True
        Me.TChart1.Axes.Bottom.Title.Brush.Visible = True
        Me.TChart1.Axes.Bottom.Title.Caption = "Parameter 1"
        '
        '
        '
        Me.TChart1.Axes.Bottom.Title.Font.Bold = False
        '
        '
        '
        Me.TChart1.Axes.Bottom.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.TChart1.Axes.Bottom.Title.Font.Brush.Solid = True
        Me.TChart1.Axes.Bottom.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Bottom.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Bottom.Title.Font.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Bottom.Title.Font.Shadow.Brush.Visible = True
        Me.TChart1.Axes.Bottom.Title.Font.Size = 11
        Me.TChart1.Axes.Bottom.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Bottom.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Axes.Bottom.Title.ImageBevel.Brush.Solid = True
        Me.TChart1.Axes.Bottom.Title.ImageBevel.Brush.Visible = True
        Me.TChart1.Axes.Bottom.Title.Lines = New String() {"Parameter 1"}
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Bottom.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Bottom.Title.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Bottom.Title.Shadow.Brush.Visible = True
        Me.TChart1.Axes.Bottom.UseMaxPixelPos = True
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Depth.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Axes.Depth.Labels.Brush.Color = System.Drawing.Color.White
        Me.TChart1.Axes.Depth.Labels.Brush.Solid = True
        Me.TChart1.Axes.Depth.Labels.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Axes.Depth.Labels.Font.Bold = False
        '
        '
        '
        Me.TChart1.Axes.Depth.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.TChart1.Axes.Depth.Labels.Font.Brush.Solid = True
        Me.TChart1.Axes.Depth.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Depth.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Depth.Labels.Font.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Depth.Labels.Font.Shadow.Brush.Visible = True
        Me.TChart1.Axes.Depth.Labels.Font.Size = 9
        Me.TChart1.Axes.Depth.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Depth.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Axes.Depth.Labels.ImageBevel.Brush.Solid = True
        Me.TChart1.Axes.Depth.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Depth.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Depth.Labels.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Depth.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Axes.Depth.Title.Angle = 0
        '
        '
        '
        Me.TChart1.Axes.Depth.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Axes.Depth.Title.Brush.Color = System.Drawing.Color.Silver
        Me.TChart1.Axes.Depth.Title.Brush.Solid = True
        Me.TChart1.Axes.Depth.Title.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Axes.Depth.Title.Font.Bold = False
        '
        '
        '
        Me.TChart1.Axes.Depth.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.TChart1.Axes.Depth.Title.Font.Brush.Solid = True
        Me.TChart1.Axes.Depth.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Depth.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Depth.Title.Font.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Depth.Title.Font.Shadow.Brush.Visible = True
        Me.TChart1.Axes.Depth.Title.Font.Size = 11
        Me.TChart1.Axes.Depth.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Depth.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Axes.Depth.Title.ImageBevel.Brush.Solid = True
        Me.TChart1.Axes.Depth.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Depth.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Depth.Title.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Depth.Title.Shadow.Brush.Visible = True
        Me.TChart1.Axes.Depth.UseMaxPixelPos = True
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Labels.Brush.Color = System.Drawing.Color.White
        Me.TChart1.Axes.DepthTop.Labels.Brush.Solid = True
        Me.TChart1.Axes.DepthTop.Labels.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Labels.Font.Bold = False
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.TChart1.Axes.DepthTop.Labels.Font.Brush.Solid = True
        Me.TChart1.Axes.DepthTop.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.DepthTop.Labels.Font.Shadow.Brush.Solid = True
        Me.TChart1.Axes.DepthTop.Labels.Font.Shadow.Brush.Visible = True
        Me.TChart1.Axes.DepthTop.Labels.Font.Size = 9
        Me.TChart1.Axes.DepthTop.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Axes.DepthTop.Labels.ImageBevel.Brush.Solid = True
        Me.TChart1.Axes.DepthTop.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.DepthTop.Labels.Shadow.Brush.Solid = True
        Me.TChart1.Axes.DepthTop.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Title.Angle = 0
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Title.Brush.Color = System.Drawing.Color.Silver
        Me.TChart1.Axes.DepthTop.Title.Brush.Solid = True
        Me.TChart1.Axes.DepthTop.Title.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Title.Font.Bold = False
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.TChart1.Axes.DepthTop.Title.Font.Brush.Solid = True
        Me.TChart1.Axes.DepthTop.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.DepthTop.Title.Font.Shadow.Brush.Solid = True
        Me.TChart1.Axes.DepthTop.Title.Font.Shadow.Brush.Visible = True
        Me.TChart1.Axes.DepthTop.Title.Font.Size = 11
        Me.TChart1.Axes.DepthTop.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Axes.DepthTop.Title.ImageBevel.Brush.Solid = True
        Me.TChart1.Axes.DepthTop.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.DepthTop.Title.Shadow.Brush.Solid = True
        Me.TChart1.Axes.DepthTop.Title.Shadow.Brush.Visible = True
        Me.TChart1.Axes.DepthTop.UseMaxPixelPos = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Left.Grid.DrawEvery = 1
        Me.TChart1.Axes.Left.Increment = 0.1R
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Left.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Axes.Left.Labels.Brush.Color = System.Drawing.Color.White
        Me.TChart1.Axes.Left.Labels.Brush.Solid = True
        Me.TChart1.Axes.Left.Labels.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Axes.Left.Labels.Font.Bold = False
        '
        '
        '
        Me.TChart1.Axes.Left.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.TChart1.Axes.Left.Labels.Font.Brush.Solid = True
        Me.TChart1.Axes.Left.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Left.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Left.Labels.Font.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Left.Labels.Font.Shadow.Brush.Visible = True
        Me.TChart1.Axes.Left.Labels.Font.Size = 9
        Me.TChart1.Axes.Left.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Left.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Axes.Left.Labels.ImageBevel.Brush.Solid = True
        Me.TChart1.Axes.Left.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Left.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Left.Labels.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Left.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Axes.Left.Title.Angle = 90
        '
        '
        '
        Me.TChart1.Axes.Left.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Axes.Left.Title.Brush.Color = System.Drawing.Color.Silver
        Me.TChart1.Axes.Left.Title.Brush.Solid = True
        Me.TChart1.Axes.Left.Title.Brush.Visible = True
        Me.TChart1.Axes.Left.Title.Caption = "Parameter 2"
        '
        '
        '
        Me.TChart1.Axes.Left.Title.Font.Bold = False
        '
        '
        '
        Me.TChart1.Axes.Left.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.TChart1.Axes.Left.Title.Font.Brush.Solid = True
        Me.TChart1.Axes.Left.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Left.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Left.Title.Font.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Left.Title.Font.Shadow.Brush.Visible = True
        Me.TChart1.Axes.Left.Title.Font.Size = 11
        Me.TChart1.Axes.Left.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Left.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Axes.Left.Title.ImageBevel.Brush.Solid = True
        Me.TChart1.Axes.Left.Title.ImageBevel.Brush.Visible = True
        Me.TChart1.Axes.Left.Title.Lines = New String() {"Parameter 2"}
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Left.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Left.Title.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Left.Title.Shadow.Brush.Visible = True
        Me.TChart1.Axes.Left.UseMaxPixelPos = True
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Right.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Axes.Right.Labels.Brush.Color = System.Drawing.Color.White
        Me.TChart1.Axes.Right.Labels.Brush.Solid = True
        Me.TChart1.Axes.Right.Labels.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Axes.Right.Labels.Font.Bold = False
        '
        '
        '
        Me.TChart1.Axes.Right.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.TChart1.Axes.Right.Labels.Font.Brush.Solid = True
        Me.TChart1.Axes.Right.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Right.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Right.Labels.Font.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Right.Labels.Font.Shadow.Brush.Visible = True
        Me.TChart1.Axes.Right.Labels.Font.Size = 9
        Me.TChart1.Axes.Right.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Right.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Axes.Right.Labels.ImageBevel.Brush.Solid = True
        Me.TChart1.Axes.Right.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Right.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Right.Labels.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Right.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Axes.Right.Title.Angle = 270
        '
        '
        '
        Me.TChart1.Axes.Right.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Axes.Right.Title.Brush.Color = System.Drawing.Color.Silver
        Me.TChart1.Axes.Right.Title.Brush.Solid = True
        Me.TChart1.Axes.Right.Title.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Axes.Right.Title.Font.Bold = False
        '
        '
        '
        Me.TChart1.Axes.Right.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.TChart1.Axes.Right.Title.Font.Brush.Solid = True
        Me.TChart1.Axes.Right.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Right.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Right.Title.Font.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Right.Title.Font.Shadow.Brush.Visible = True
        Me.TChart1.Axes.Right.Title.Font.Size = 11
        Me.TChart1.Axes.Right.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Right.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Axes.Right.Title.ImageBevel.Brush.Solid = True
        Me.TChart1.Axes.Right.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Right.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Right.Title.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Right.Title.Shadow.Brush.Visible = True
        Me.TChart1.Axes.Right.UseMaxPixelPos = True
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Top.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Axes.Top.Labels.Brush.Color = System.Drawing.Color.White
        Me.TChart1.Axes.Top.Labels.Brush.Solid = True
        Me.TChart1.Axes.Top.Labels.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Axes.Top.Labels.Font.Bold = False
        '
        '
        '
        Me.TChart1.Axes.Top.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.TChart1.Axes.Top.Labels.Font.Brush.Solid = True
        Me.TChart1.Axes.Top.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Top.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Top.Labels.Font.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Top.Labels.Font.Shadow.Brush.Visible = True
        Me.TChart1.Axes.Top.Labels.Font.Size = 9
        Me.TChart1.Axes.Top.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Top.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Axes.Top.Labels.ImageBevel.Brush.Solid = True
        Me.TChart1.Axes.Top.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Top.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Top.Labels.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Top.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Axes.Top.Title.Angle = 0
        '
        '
        '
        Me.TChart1.Axes.Top.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Axes.Top.Title.Brush.Color = System.Drawing.Color.Silver
        Me.TChart1.Axes.Top.Title.Brush.Solid = True
        Me.TChart1.Axes.Top.Title.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Axes.Top.Title.Font.Bold = False
        '
        '
        '
        Me.TChart1.Axes.Top.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.TChart1.Axes.Top.Title.Font.Brush.Solid = True
        Me.TChart1.Axes.Top.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Top.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Top.Title.Font.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Top.Title.Font.Shadow.Brush.Visible = True
        Me.TChart1.Axes.Top.Title.Font.Size = 11
        Me.TChart1.Axes.Top.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Top.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Axes.Top.Title.ImageBevel.Brush.Solid = True
        Me.TChart1.Axes.Top.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Top.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Axes.Top.Title.Shadow.Brush.Solid = True
        Me.TChart1.Axes.Top.Title.Shadow.Brush.Visible = True
        Me.TChart1.Axes.Top.UseMaxPixelPos = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Footer.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Footer.Brush.Color = System.Drawing.Color.Silver
        Me.TChart1.Footer.Brush.Solid = True
        Me.TChart1.Footer.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Footer.Font.Bold = False
        '
        '
        '
        Me.TChart1.Footer.Font.Brush.Color = System.Drawing.Color.Red
        Me.TChart1.Footer.Font.Brush.Solid = True
        Me.TChart1.Footer.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Footer.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Footer.Font.Shadow.Brush.Solid = True
        Me.TChart1.Footer.Font.Shadow.Brush.Visible = True
        Me.TChart1.Footer.Font.Size = 8
        Me.TChart1.Footer.Font.SizeFloat = 8.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.Footer.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Footer.ImageBevel.Brush.Solid = True
        Me.TChart1.Footer.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Footer.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Footer.Shadow.Brush.Solid = True
        Me.TChart1.Footer.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Header.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Header.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TChart1.Header.Brush.Solid = True
        Me.TChart1.Header.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Header.Font.Bold = False
        '
        '
        '
        Me.TChart1.Header.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.TChart1.Header.Font.Brush.Solid = True
        Me.TChart1.Header.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Header.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Header.Font.Shadow.Brush.Solid = True
        Me.TChart1.Header.Font.Shadow.Brush.Visible = True
        Me.TChart1.Header.Font.Size = 12
        Me.TChart1.Header.Font.SizeFloat = 12.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.Header.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Header.ImageBevel.Brush.Solid = True
        Me.TChart1.Header.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Header.Shadow.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.TChart1.Header.Shadow.Brush.Solid = True
        Me.TChart1.Header.Shadow.Brush.Visible = True
        Me.TChart1.Header.Visible = False
        '
        '
        '
        '
        '
        '
        Me.TChart1.Legend.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Legend.Brush.Color = System.Drawing.Color.White
        Me.TChart1.Legend.Brush.Solid = True
        Me.TChart1.Legend.Brush.Visible = True
        Me.TChart1.Legend.CheckBoxes = True
        '
        '
        '
        Me.TChart1.Legend.Font.Bold = False
        '
        '
        '
        Me.TChart1.Legend.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.TChart1.Legend.Font.Brush.Solid = True
        Me.TChart1.Legend.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Legend.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Legend.Font.Shadow.Brush.Solid = True
        Me.TChart1.Legend.Font.Shadow.Brush.Visible = True
        Me.TChart1.Legend.Font.Size = 9
        Me.TChart1.Legend.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.Legend.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Legend.ImageBevel.Brush.Solid = True
        Me.TChart1.Legend.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Legend.Shadow.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.TChart1.Legend.Shadow.Brush.Solid = True
        Me.TChart1.Legend.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Legend.Symbol.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Legend.Symbol.Shadow.Brush.Solid = True
        Me.TChart1.Legend.Symbol.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Legend.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Legend.Title.Brush.Color = System.Drawing.Color.White
        Me.TChart1.Legend.Title.Brush.Solid = True
        Me.TChart1.Legend.Title.Brush.Visible = True
        '
        '
        '
        Me.TChart1.Legend.Title.Font.Bold = True
        '
        '
        '
        Me.TChart1.Legend.Title.Font.Brush.Color = System.Drawing.Color.Black
        Me.TChart1.Legend.Title.Font.Brush.Solid = True
        Me.TChart1.Legend.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Legend.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Legend.Title.Font.Shadow.Brush.Solid = True
        Me.TChart1.Legend.Title.Font.Shadow.Brush.Visible = True
        Me.TChart1.Legend.Title.Font.Size = 8
        Me.TChart1.Legend.Title.Font.SizeFloat = 8.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.Legend.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Legend.Title.ImageBevel.Brush.Solid = True
        Me.TChart1.Legend.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Legend.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Legend.Title.Shadow.Brush.Solid = True
        Me.TChart1.Legend.Title.Shadow.Brush.Visible = True
        Me.TChart1.Location = New System.Drawing.Point(12, 12)
        Me.TChart1.Name = "TChart1"
        '
        '
        '
        '
        '
        '
        Me.TChart1.Panel.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Panel.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.TChart1.Panel.Brush.Solid = True
        Me.TChart1.Panel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Panel.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Panel.ImageBevel.Brush.Solid = True
        Me.TChart1.Panel.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Panel.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Panel.Shadow.Brush.Solid = True
        Me.TChart1.Panel.Shadow.Brush.Visible = True
        '
        '
        '
        Margins1.Bottom = 100
        Margins1.Left = 100
        Margins1.Right = 100
        Margins1.Top = 100
        Me.TChart1.Printer.Margins = Margins1
        Me.TChart1.Size = New System.Drawing.Size(776, 390)
        '
        '
        '
        '
        '
        '
        Me.TChart1.SubFooter.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.SubFooter.Brush.Color = System.Drawing.Color.Silver
        Me.TChart1.SubFooter.Brush.Solid = True
        Me.TChart1.SubFooter.Brush.Visible = True
        '
        '
        '
        Me.TChart1.SubFooter.Font.Bold = False
        '
        '
        '
        Me.TChart1.SubFooter.Font.Brush.Color = System.Drawing.Color.Red
        Me.TChart1.SubFooter.Font.Brush.Solid = True
        Me.TChart1.SubFooter.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.SubFooter.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.SubFooter.Font.Shadow.Brush.Solid = True
        Me.TChart1.SubFooter.Font.Shadow.Brush.Visible = True
        Me.TChart1.SubFooter.Font.Size = 8
        Me.TChart1.SubFooter.Font.SizeFloat = 8.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.SubFooter.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.SubFooter.ImageBevel.Brush.Solid = True
        Me.TChart1.SubFooter.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.SubFooter.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.SubFooter.Shadow.Brush.Solid = True
        Me.TChart1.SubFooter.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.SubHeader.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.SubHeader.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TChart1.SubHeader.Brush.Solid = True
        Me.TChart1.SubHeader.Brush.Visible = True
        '
        '
        '
        Me.TChart1.SubHeader.Font.Bold = False
        '
        '
        '
        Me.TChart1.SubHeader.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.TChart1.SubHeader.Font.Brush.Solid = True
        Me.TChart1.SubHeader.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.SubHeader.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.SubHeader.Font.Shadow.Brush.Solid = True
        Me.TChart1.SubHeader.Font.Shadow.Brush.Visible = True
        Me.TChart1.SubHeader.Font.Size = 12
        Me.TChart1.SubHeader.Font.SizeFloat = 12.0!
        '
        '
        '
        '
        '
        '
        Me.TChart1.SubHeader.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.SubHeader.ImageBevel.Brush.Solid = True
        Me.TChart1.SubHeader.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.SubHeader.Shadow.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.TChart1.SubHeader.Shadow.Brush.Solid = True
        Me.TChart1.SubHeader.Shadow.Brush.Visible = True
        Me.TChart1.TabIndex = 0
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Walls.Back.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Walls.Back.Brush.Color = System.Drawing.Color.Silver
        Me.TChart1.Walls.Back.Brush.Solid = True
        Me.TChart1.Walls.Back.Brush.Visible = False
        '
        '
        '
        '
        '
        '
        Me.TChart1.Walls.Back.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Walls.Back.ImageBevel.Brush.Solid = True
        Me.TChart1.Walls.Back.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Walls.Back.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Walls.Back.Shadow.Brush.Solid = True
        Me.TChart1.Walls.Back.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Walls.Bottom.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Walls.Bottom.Brush.Color = System.Drawing.Color.White
        Me.TChart1.Walls.Bottom.Brush.Solid = True
        Me.TChart1.Walls.Bottom.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Walls.Bottom.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Walls.Bottom.ImageBevel.Brush.Solid = True
        Me.TChart1.Walls.Bottom.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Walls.Bottom.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Walls.Bottom.Shadow.Brush.Solid = True
        Me.TChart1.Walls.Bottom.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Walls.Left.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Walls.Left.Brush.Color = System.Drawing.Color.LightYellow
        Me.TChart1.Walls.Left.Brush.Solid = True
        Me.TChart1.Walls.Left.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Walls.Left.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Walls.Left.ImageBevel.Brush.Solid = True
        Me.TChart1.Walls.Left.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Walls.Left.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Walls.Left.Shadow.Brush.Solid = True
        Me.TChart1.Walls.Left.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Walls.Right.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.TChart1.Walls.Right.Brush.Color = System.Drawing.Color.LightYellow
        Me.TChart1.Walls.Right.Brush.Solid = True
        Me.TChart1.Walls.Right.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Walls.Right.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.TChart1.Walls.Right.ImageBevel.Brush.Solid = True
        Me.TChart1.Walls.Right.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Walls.Right.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.TChart1.Walls.Right.Shadow.Brush.Solid = True
        Me.TChart1.Walls.Right.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.TChart1.Zoom.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.TChart1.Zoom.Brush.Solid = True
        Me.TChart1.Zoom.Brush.Visible = False
        '
        'Button_Generate
        '
        Me.Button_Generate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_Generate.Location = New System.Drawing.Point(148, 418)
        Me.Button_Generate.Name = "Button_Generate"
        Me.Button_Generate.Size = New System.Drawing.Size(75, 23)
        Me.Button_Generate.TabIndex = 1
        Me.Button_Generate.Text = "Generate samples"
        Me.Button_Generate.UseVisualStyleBackColor = True
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.NumericUpDown1.Location = New System.Drawing.Point(105, 418)
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(37, 20)
        Me.NumericUpDown1.TabIndex = 2
        Me.NumericUpDown1.Value = New Decimal(New Integer() {11, 0, 0, 0})
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 421)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Number of steps:"
        '
        'ParameterSampling
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.NumericUpDown1)
        Me.Controls.Add(Me.Button_Generate)
        Me.Controls.Add(Me.TChart1)
        Me.Name = "ParameterSampling"
        Me.Text = "Test ParameterSampling"
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TChart1 As Steema.TeeChart.TChart
    Friend WithEvents Button_Generate As Windows.Forms.Button
    Friend WithEvents NumericUpDown1 As Windows.Forms.NumericUpDown
    Friend WithEvents Label1 As Windows.Forms.Label
End Class
