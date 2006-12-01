Option Strict Off ' Off ist Default
Option Explicit On
Friend Class Form1
    Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean
#Region "Vom Windows Form-Designer generierter Code "
	Public Sub New()
		MyBase.New()
		If m_vb6FormDefInstance Is Nothing Then
			If m_InitializingDefInstance Then
				m_vb6FormDefInstance = Me
			Else
				Try 
					'Für das Startformular ist die zuerst erstellte Instanz die Standardinstanz.
					If System.Reflection.Assembly.GetExecutingAssembly.EntryPoint.DeclaringType Is Me.GetType Then
						m_vb6FormDefInstance = Me
					End If
				Catch
				End Try
			End If
		End If
        'Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        IsInitializing = True
        InitializeComponent()
        IsInitializing = False
	End Sub
	'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
	Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Wird vom Windows Form-Designer benötigt.
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents TChart1 As Steema.TeeChart.TChart
    Public WithEvents EVO_Opt_Verlauf1 As EvoForm.EVO_Opt_Verlauf
    Public WithEvents EVO_Einstellungen1 As EvoForm.EVO_Einstellungen
	Public WithEvents Text11 As System.Windows.Forms.TextBox
	Public WithEvents _Frame_Problem_10 As System.Windows.Forms.GroupBox
	Public WithEvents Text10 As System.Windows.Forms.TextBox
	Public WithEvents _Frame_Problem_9 As System.Windows.Forms.GroupBox
	Public WithEvents Text9 As System.Windows.Forms.TextBox
	Public WithEvents _Frame_Problem_8 As System.Windows.Forms.GroupBox
	Public WithEvents Text8 As System.Windows.Forms.TextBox
	Public WithEvents _Frame_Problem_7 As System.Windows.Forms.GroupBox
	Public WithEvents Text7 As System.Windows.Forms.TextBox
	Public WithEvents _Frame_Problem_6 As System.Windows.Forms.GroupBox
	Public WithEvents Text6 As System.Windows.Forms.TextBox
	Public WithEvents _Frame_Problem_5 As System.Windows.Forms.GroupBox
	Public WithEvents Text4 As System.Windows.Forms.TextBox
	Public WithEvents _Frame_Problem_4 As System.Windows.Forms.GroupBox
	Public WithEvents Text5 As System.Windows.Forms.TextBox
	Public WithEvents _Frame_Problem_3 As System.Windows.Forms.GroupBox
	Public WithEvents Combo1 As System.Windows.Forms.ComboBox
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents Par_Sinus As System.Windows.Forms.TextBox
	Public WithEvents Text1 As System.Windows.Forms.TextBox
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents _Frame_Problem_0 As System.Windows.Forms.GroupBox
	Public WithEvents Command1 As System.Windows.Forms.Button
	Public WithEvents Text2 As System.Windows.Forms.TextBox
	Public WithEvents _Frame_Problem_1 As System.Windows.Forms.GroupBox
	Public WithEvents Text3 As System.Windows.Forms.TextBox
	Public WithEvents Par_Schwefel As System.Windows.Forms.TextBox
	Public WithEvents Label6 As System.Windows.Forms.Label
	Public WithEvents _Frame_Problem_2 As System.Windows.Forms.GroupBox
	Public WithEvents Frame_Problem As Microsoft.VisualBasic.Compatibility.VB6.GroupBoxArray
	'Hinweis: Die folgende Prozedur wird vom Windows Form-Designer benötigt.
	'Das Verändern mit dem Windows Form-Designer ist nicht möglich.
	'Das Verändern mit dem Code-Editor ist nicht möglich.
    Public WithEvents TeeCommander1 As Steema.TeeChart.Commander
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TChart1 = New Steema.TeeChart.TChart
        Me.EVO_Opt_Verlauf1 = New EvoForm.EVO_Opt_Verlauf
        Me.EVO_Einstellungen1 = New EvoForm.EVO_Einstellungen
        Me._Frame_Problem_10 = New System.Windows.Forms.GroupBox
        Me.Text11 = New System.Windows.Forms.TextBox
        Me._Frame_Problem_9 = New System.Windows.Forms.GroupBox
        Me.Text10 = New System.Windows.Forms.TextBox
        Me._Frame_Problem_8 = New System.Windows.Forms.GroupBox
        Me.Text9 = New System.Windows.Forms.TextBox
        Me._Frame_Problem_7 = New System.Windows.Forms.GroupBox
        Me.Text8 = New System.Windows.Forms.TextBox
        Me._Frame_Problem_6 = New System.Windows.Forms.GroupBox
        Me.Text7 = New System.Windows.Forms.TextBox
        Me._Frame_Problem_5 = New System.Windows.Forms.GroupBox
        Me.Text6 = New System.Windows.Forms.TextBox
        Me._Frame_Problem_4 = New System.Windows.Forms.GroupBox
        Me.Text4 = New System.Windows.Forms.TextBox
        Me._Frame_Problem_3 = New System.Windows.Forms.GroupBox
        Me.Text5 = New System.Windows.Forms.TextBox
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.Combo1 = New System.Windows.Forms.ComboBox
        Me._Frame_Problem_0 = New System.Windows.Forms.GroupBox
        Me.Par_Sinus = New System.Windows.Forms.TextBox
        Me.Text1 = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Command1 = New System.Windows.Forms.Button
        Me._Frame_Problem_1 = New System.Windows.Forms.GroupBox
        Me.Text2 = New System.Windows.Forms.TextBox
        Me._Frame_Problem_2 = New System.Windows.Forms.GroupBox
        Me.Text3 = New System.Windows.Forms.TextBox
        Me.Par_Schwefel = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Frame_Problem = New Microsoft.VisualBasic.Compatibility.VB6.GroupBoxArray(Me.components)
        Me.TeeCommander1 = New Steema.TeeChart.Commander
        Me._Frame_Problem_10.SuspendLayout()
        Me._Frame_Problem_9.SuspendLayout()
        Me._Frame_Problem_8.SuspendLayout()
        Me._Frame_Problem_7.SuspendLayout()
        Me._Frame_Problem_6.SuspendLayout()
        Me._Frame_Problem_5.SuspendLayout()
        Me._Frame_Problem_4.SuspendLayout()
        Me._Frame_Problem_3.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me._Frame_Problem_0.SuspendLayout()
        Me._Frame_Problem_1.SuspendLayout()
        Me._Frame_Problem_2.SuspendLayout()
        CType(Me.Frame_Problem, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TChart1
        '
        '
        'TChart1.Aspect
        '
        Me.TChart1.Aspect.ElevationFloat = 345
        Me.TChart1.Aspect.RotationFloat = 345
        '
        'TChart1.Axes
        '
        '
        'TChart1.Axes.Bottom
        '
        Me.TChart1.Axes.Bottom.Automatic = True
        '
        'TChart1.Axes.Bottom.Grid
        '
        Me.TChart1.Axes.Bottom.Grid.ZPosition = 0
        '
        'TChart1.Axes.Bottom.Labels
        '
        '
        'TChart1.Axes.Bottom.Labels.Font
        '
        '
        'TChart1.Axes.Bottom.Labels.Font.Shadow
        '
        Me.TChart1.Axes.Bottom.Labels.Font.Shadow.Visible = False
        Me.TChart1.Axes.Bottom.Labels.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        'TChart1.Axes.Bottom.Labels.Shadow
        '
        Me.TChart1.Axes.Bottom.Labels.Shadow.Visible = False
        '
        'TChart1.Axes.Bottom.Title
        '
        '
        'TChart1.Axes.Bottom.Title.Font
        '
        '
        'TChart1.Axes.Bottom.Title.Font.Shadow
        '
        Me.TChart1.Axes.Bottom.Title.Font.Shadow.Visible = False
        Me.TChart1.Axes.Bottom.Title.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        'TChart1.Axes.Bottom.Title.Shadow
        '
        Me.TChart1.Axes.Bottom.Title.Shadow.Visible = False
        '
        'TChart1.Axes.Depth
        '
        Me.TChart1.Axes.Depth.Automatic = True
        '
        'TChart1.Axes.Depth.Grid
        '
        Me.TChart1.Axes.Depth.Grid.ZPosition = 0
        '
        'TChart1.Axes.Depth.Labels
        '
        '
        'TChart1.Axes.Depth.Labels.Font
        '
        '
        'TChart1.Axes.Depth.Labels.Font.Shadow
        '
        Me.TChart1.Axes.Depth.Labels.Font.Shadow.Visible = False
        Me.TChart1.Axes.Depth.Labels.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        'TChart1.Axes.Depth.Labels.Shadow
        '
        Me.TChart1.Axes.Depth.Labels.Shadow.Visible = False
        '
        'TChart1.Axes.Depth.Title
        '
        '
        'TChart1.Axes.Depth.Title.Font
        '
        '
        'TChart1.Axes.Depth.Title.Font.Shadow
        '
        Me.TChart1.Axes.Depth.Title.Font.Shadow.Visible = False
        Me.TChart1.Axes.Depth.Title.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        'TChart1.Axes.Depth.Title.Shadow
        '
        Me.TChart1.Axes.Depth.Title.Shadow.Visible = False
        '
        'TChart1.Axes.DepthTop
        '
        Me.TChart1.Axes.DepthTop.Automatic = True
        '
        'TChart1.Axes.DepthTop.Grid
        '
        Me.TChart1.Axes.DepthTop.Grid.ZPosition = 0
        '
        'TChart1.Axes.DepthTop.Labels
        '
        '
        'TChart1.Axes.DepthTop.Labels.Font
        '
        '
        'TChart1.Axes.DepthTop.Labels.Font.Shadow
        '
        Me.TChart1.Axes.DepthTop.Labels.Font.Shadow.Visible = False
        Me.TChart1.Axes.DepthTop.Labels.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        'TChart1.Axes.DepthTop.Labels.Shadow
        '
        Me.TChart1.Axes.DepthTop.Labels.Shadow.Visible = False
        '
        'TChart1.Axes.DepthTop.Title
        '
        '
        'TChart1.Axes.DepthTop.Title.Font
        '
        '
        'TChart1.Axes.DepthTop.Title.Font.Shadow
        '
        Me.TChart1.Axes.DepthTop.Title.Font.Shadow.Visible = False
        Me.TChart1.Axes.DepthTop.Title.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        'TChart1.Axes.DepthTop.Title.Shadow
        '
        Me.TChart1.Axes.DepthTop.Title.Shadow.Visible = False
        '
        'TChart1.Axes.Left
        '
        Me.TChart1.Axes.Left.Automatic = True
        '
        'TChart1.Axes.Left.Grid
        '
        Me.TChart1.Axes.Left.Grid.ZPosition = 0
        '
        'TChart1.Axes.Left.Labels
        '
        '
        'TChart1.Axes.Left.Labels.Font
        '
        '
        'TChart1.Axes.Left.Labels.Font.Shadow
        '
        Me.TChart1.Axes.Left.Labels.Font.Shadow.Visible = False
        Me.TChart1.Axes.Left.Labels.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        'TChart1.Axes.Left.Labels.Shadow
        '
        Me.TChart1.Axes.Left.Labels.Shadow.Visible = False
        '
        'TChart1.Axes.Left.Title
        '
        '
        'TChart1.Axes.Left.Title.Font
        '
        '
        'TChart1.Axes.Left.Title.Font.Shadow
        '
        Me.TChart1.Axes.Left.Title.Font.Shadow.Visible = False
        Me.TChart1.Axes.Left.Title.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        'TChart1.Axes.Left.Title.Shadow
        '
        Me.TChart1.Axes.Left.Title.Shadow.Visible = False
        '
        'TChart1.Axes.Right
        '
        Me.TChart1.Axes.Right.Automatic = True
        '
        'TChart1.Axes.Right.Grid
        '
        Me.TChart1.Axes.Right.Grid.ZPosition = 0
        '
        'TChart1.Axes.Right.Labels
        '
        '
        'TChart1.Axes.Right.Labels.Font
        '
        '
        'TChart1.Axes.Right.Labels.Font.Shadow
        '
        Me.TChart1.Axes.Right.Labels.Font.Shadow.Visible = False
        Me.TChart1.Axes.Right.Labels.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        'TChart1.Axes.Right.Labels.Shadow
        '
        Me.TChart1.Axes.Right.Labels.Shadow.Visible = False
        '
        'TChart1.Axes.Right.Title
        '
        '
        'TChart1.Axes.Right.Title.Font
        '
        '
        'TChart1.Axes.Right.Title.Font.Shadow
        '
        Me.TChart1.Axes.Right.Title.Font.Shadow.Visible = False
        Me.TChart1.Axes.Right.Title.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        'TChart1.Axes.Right.Title.Shadow
        '
        Me.TChart1.Axes.Right.Title.Shadow.Visible = False
        '
        'TChart1.Axes.Top
        '
        Me.TChart1.Axes.Top.Automatic = True
        '
        'TChart1.Axes.Top.Grid
        '
        Me.TChart1.Axes.Top.Grid.ZPosition = 0
        '
        'TChart1.Axes.Top.Labels
        '
        '
        'TChart1.Axes.Top.Labels.Font
        '
        '
        'TChart1.Axes.Top.Labels.Font.Shadow
        '
        Me.TChart1.Axes.Top.Labels.Font.Shadow.Visible = False
        Me.TChart1.Axes.Top.Labels.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        'TChart1.Axes.Top.Labels.Shadow
        '
        Me.TChart1.Axes.Top.Labels.Shadow.Visible = False
        '
        'TChart1.Axes.Top.Title
        '
        '
        'TChart1.Axes.Top.Title.Font
        '
        '
        'TChart1.Axes.Top.Title.Font.Shadow
        '
        Me.TChart1.Axes.Top.Title.Font.Shadow.Visible = False
        Me.TChart1.Axes.Top.Title.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        'TChart1.Axes.Top.Title.Shadow
        '
        Me.TChart1.Axes.Top.Title.Shadow.Visible = False
        '
        'TChart1.Footer
        '
        '
        'TChart1.Footer.Font
        '
        '
        'TChart1.Footer.Font.Shadow
        '
        Me.TChart1.Footer.Font.Shadow.Visible = False
        Me.TChart1.Footer.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        'TChart1.Footer.Shadow
        '
        Me.TChart1.Footer.Shadow.Visible = False
        '
        'TChart1.Header
        '
        '
        'TChart1.Header.Font
        '
        '
        'TChart1.Header.Font.Shadow
        '
        Me.TChart1.Header.Font.Shadow.Visible = False
        Me.TChart1.Header.Font.Unit = System.Drawing.GraphicsUnit.World
        Me.TChart1.Header.Lines = New String() {"TeeChart"}
        '
        'TChart1.Header.Shadow
        '
        Me.TChart1.Header.Shadow.Visible = False
        '
        'TChart1.Legend
        '
        '
        'TChart1.Legend.Font
        '
        '
        'TChart1.Legend.Font.Shadow
        '
        Me.TChart1.Legend.Font.Shadow.Visible = False
        Me.TChart1.Legend.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        'TChart1.Legend.Title
        '
        '
        'TChart1.Legend.Title.Font
        '
        Me.TChart1.Legend.Title.Font.Bold = True
        '
        'TChart1.Legend.Title.Font.Shadow
        '
        Me.TChart1.Legend.Title.Font.Shadow.Visible = False
        Me.TChart1.Legend.Title.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        'TChart1.Legend.Title.Pen
        '
        Me.TChart1.Legend.Title.Pen.Visible = False
        '
        'TChart1.Legend.Title.Shadow
        '
        Me.TChart1.Legend.Title.Shadow.Visible = False
        Me.TChart1.Location = New System.Drawing.Point(232, 0)
        Me.TChart1.Name = "TChart1"
        '
        'TChart1.Panel
        '
        '
        'TChart1.Panel.Shadow
        '
        Me.TChart1.Panel.Shadow.Visible = False
        Me.TChart1.Size = New System.Drawing.Size(465, 584)
        '
        'TChart1.SubFooter
        '
        '
        'TChart1.SubFooter.Font
        '
        '
        'TChart1.SubFooter.Font.Shadow
        '
        Me.TChart1.SubFooter.Font.Shadow.Visible = False
        Me.TChart1.SubFooter.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        'TChart1.SubFooter.Shadow
        '
        Me.TChart1.SubFooter.Shadow.Visible = False
        '
        'TChart1.SubHeader
        '
        '
        'TChart1.SubHeader.Font
        '
        '
        'TChart1.SubHeader.Font.Shadow
        '
        Me.TChart1.SubHeader.Font.Shadow.Visible = False
        Me.TChart1.SubHeader.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        'TChart1.SubHeader.Shadow
        '
        Me.TChart1.SubHeader.Shadow.Visible = False
        Me.TChart1.TabIndex = 29
        '
        'TChart1.Walls
        '
        '
        'TChart1.Walls.Back
        '
        Me.TChart1.Walls.Back.AutoHide = False
        '
        'TChart1.Walls.Back.Shadow
        '
        Me.TChart1.Walls.Back.Shadow.Visible = False
        '
        'TChart1.Walls.Bottom
        '
        Me.TChart1.Walls.Bottom.AutoHide = False
        '
        'TChart1.Walls.Bottom.Shadow
        '
        Me.TChart1.Walls.Bottom.Shadow.Visible = False
        '
        'TChart1.Walls.Left
        '
        Me.TChart1.Walls.Left.AutoHide = False
        '
        'TChart1.Walls.Left.Shadow
        '
        Me.TChart1.Walls.Left.Shadow.Visible = False
        '
        'TChart1.Walls.Right
        '
        Me.TChart1.Walls.Right.AutoHide = False
        '
        'TChart1.Walls.Right.Shadow
        '
        Me.TChart1.Walls.Right.Shadow.Visible = False
        '
        'EVO_Opt_Verlauf1
        '
        Me.EVO_Opt_Verlauf1.Location = New System.Drawing.Point(232, 640)
        Me.EVO_Opt_Verlauf1.Name = "EVO_Opt_Verlauf1"
        Me.EVO_Opt_Verlauf1.NGen = CType(0, Short)
        Me.EVO_Opt_Verlauf1.NNachf = CType(0, Short)
        Me.EVO_Opt_Verlauf1.NPopul = CType(0, Short)
        Me.EVO_Opt_Verlauf1.NRunden = CType(0, Short)
        Me.EVO_Opt_Verlauf1.Size = New System.Drawing.Size(489, 73)
        Me.EVO_Opt_Verlauf1.TabIndex = 32
        '
        'EVO_Einstellungen1
        '
        Me.EVO_Einstellungen1.Location = New System.Drawing.Point(0, 0)
        Me.EVO_Einstellungen1.Name = "EVO_Einstellungen1"
        Me.EVO_Einstellungen1.Size = New System.Drawing.Size(225, 585)
        Me.EVO_Einstellungen1.TabIndex = 31
        '
        '_Frame_Problem_10
        '
        Me._Frame_Problem_10.BackColor = System.Drawing.SystemColors.Control
        Me._Frame_Problem_10.Controls.Add(Me.Text11)
        Me._Frame_Problem_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me._Frame_Problem_10, CType(10, Short))
        Me._Frame_Problem_10.Location = New System.Drawing.Point(0, 640)
        Me._Frame_Problem_10.Name = "_Frame_Problem_10"
        Me._Frame_Problem_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Frame_Problem_10.Size = New System.Drawing.Size(217, 73)
        Me._Frame_Problem_10.TabIndex = 27
        Me._Frame_Problem_10.TabStop = False
        Me._Frame_Problem_10.Text = "Box-Funktion"
        '
        'Text11
        '
        Me.Text11.AcceptsReturn = True
        Me.Text11.AutoSize = False
        Me.Text11.BackColor = System.Drawing.SystemColors.Control
        Me.Text11.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text11.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text11.ForeColor = System.Drawing.Color.Red
        Me.Text11.Location = New System.Drawing.Point(8, 16)
        Me.Text11.MaxLength = 0
        Me.Text11.Multiline = True
        Me.Text11.Name = "Text11"
        Me.Text11.ReadOnly = True
        Me.Text11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text11.Size = New System.Drawing.Size(177, 49)
        Me.Text11.TabIndex = 28
        Me.Text11.Text = "Multikriterielles Testproblem (Box)" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "mit zwei Randbedingungen" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10)
        '
        '_Frame_Problem_9
        '
        Me._Frame_Problem_9.BackColor = System.Drawing.SystemColors.Control
        Me._Frame_Problem_9.Controls.Add(Me.Text10)
        Me._Frame_Problem_9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me._Frame_Problem_9, CType(9, Short))
        Me._Frame_Problem_9.Location = New System.Drawing.Point(0, 640)
        Me._Frame_Problem_9.Name = "_Frame_Problem_9"
        Me._Frame_Problem_9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Frame_Problem_9.Size = New System.Drawing.Size(217, 73)
        Me._Frame_Problem_9.TabIndex = 25
        Me._Frame_Problem_9.TabStop = False
        Me._Frame_Problem_9.Text = "TKN-Funktion"
        '
        'Text10
        '
        Me.Text10.AcceptsReturn = True
        Me.Text10.AutoSize = False
        Me.Text10.BackColor = System.Drawing.SystemColors.Control
        Me.Text10.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text10.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text10.ForeColor = System.Drawing.Color.Red
        Me.Text10.Location = New System.Drawing.Point(8, 16)
        Me.Text10.MaxLength = 0
        Me.Text10.Multiline = True
        Me.Text10.Name = "Text10"
        Me.Text10.ReadOnly = True
        Me.Text10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text10.Size = New System.Drawing.Size(177, 49)
        Me.Text10.TabIndex = 26
        Me.Text10.Text = "Multikriterielles Testproblem (Kreis)" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "mit zwei Randbedingungen" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10)
        '
        '_Frame_Problem_8
        '
        Me._Frame_Problem_8.BackColor = System.Drawing.SystemColors.Control
        Me._Frame_Problem_8.Controls.Add(Me.Text9)
        Me._Frame_Problem_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me._Frame_Problem_8, CType(8, Short))
        Me._Frame_Problem_8.Location = New System.Drawing.Point(0, 640)
        Me._Frame_Problem_8.Name = "_Frame_Problem_8"
        Me._Frame_Problem_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Frame_Problem_8.Size = New System.Drawing.Size(217, 73)
        Me._Frame_Problem_8.TabIndex = 23
        Me._Frame_Problem_8.TabStop = False
        Me._Frame_Problem_8.Text = "CONSTR-Funktion"
        '
        'Text9
        '
        Me.Text9.AcceptsReturn = True
        Me.Text9.AutoSize = False
        Me.Text9.BackColor = System.Drawing.SystemColors.Control
        Me.Text9.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text9.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text9.ForeColor = System.Drawing.Color.Red
        Me.Text9.Location = New System.Drawing.Point(8, 16)
        Me.Text9.MaxLength = 0
        Me.Text9.Multiline = True
        Me.Text9.Name = "Text9"
        Me.Text9.ReadOnly = True
        Me.Text9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text9.Size = New System.Drawing.Size(177, 49)
        Me.Text9.TabIndex = 24
        Me.Text9.Text = "Multikriterielles Testproblem (Konvex)" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "mit zwei Randbedingungen" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10)
        '
        '_Frame_Problem_7
        '
        Me._Frame_Problem_7.BackColor = System.Drawing.SystemColors.Control
        Me._Frame_Problem_7.Controls.Add(Me.Text8)
        Me._Frame_Problem_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me._Frame_Problem_7, CType(7, Short))
        Me._Frame_Problem_7.Location = New System.Drawing.Point(0, 640)
        Me._Frame_Problem_7.Name = "_Frame_Problem_7"
        Me._Frame_Problem_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Frame_Problem_7.Size = New System.Drawing.Size(217, 73)
        Me._Frame_Problem_7.TabIndex = 21
        Me._Frame_Problem_7.TabStop = False
        Me._Frame_Problem_7.Text = "T4-Funktion"
        '
        'Text8
        '
        Me.Text8.AcceptsReturn = True
        Me.Text8.AutoSize = False
        Me.Text8.BackColor = System.Drawing.SystemColors.Control
        Me.Text8.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text8.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text8.ForeColor = System.Drawing.Color.Red
        Me.Text8.Location = New System.Drawing.Point(8, 16)
        Me.Text8.MaxLength = 0
        Me.Text8.Multiline = True
        Me.Text8.Name = "Text8"
        Me.Text8.ReadOnly = True
        Me.Text8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text8.Size = New System.Drawing.Size(177, 25)
        Me.Text8.TabIndex = 22
        Me.Text8.Text = "Multikriterielles Testproblem (Konvex)" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10)
        '
        '_Frame_Problem_6
        '
        Me._Frame_Problem_6.BackColor = System.Drawing.SystemColors.Control
        Me._Frame_Problem_6.Controls.Add(Me.Text7)
        Me._Frame_Problem_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me._Frame_Problem_6, CType(6, Short))
        Me._Frame_Problem_6.Location = New System.Drawing.Point(0, 640)
        Me._Frame_Problem_6.Name = "_Frame_Problem_6"
        Me._Frame_Problem_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Frame_Problem_6.Size = New System.Drawing.Size(217, 73)
        Me._Frame_Problem_6.TabIndex = 19
        Me._Frame_Problem_6.TabStop = False
        Me._Frame_Problem_6.Text = "T3-Funktion"
        '
        'Text7
        '
        Me.Text7.AcceptsReturn = True
        Me.Text7.AutoSize = False
        Me.Text7.BackColor = System.Drawing.SystemColors.Control
        Me.Text7.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text7.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text7.ForeColor = System.Drawing.Color.Red
        Me.Text7.Location = New System.Drawing.Point(8, 16)
        Me.Text7.MaxLength = 0
        Me.Text7.Multiline = True
        Me.Text7.Name = "Text7"
        Me.Text7.ReadOnly = True
        Me.Text7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text7.Size = New System.Drawing.Size(177, 45)
        Me.Text7.TabIndex = 20
        Me.Text7.Text = "Multikriterielles Testproblem (Konvex, " & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "nicht stetig)" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10)
        '
        '_Frame_Problem_5
        '
        Me._Frame_Problem_5.BackColor = System.Drawing.SystemColors.Control
        Me._Frame_Problem_5.Controls.Add(Me.Text6)
        Me._Frame_Problem_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me._Frame_Problem_5, CType(5, Short))
        Me._Frame_Problem_5.Location = New System.Drawing.Point(0, 640)
        Me._Frame_Problem_5.Name = "_Frame_Problem_5"
        Me._Frame_Problem_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Frame_Problem_5.Size = New System.Drawing.Size(217, 73)
        Me._Frame_Problem_5.TabIndex = 17
        Me._Frame_Problem_5.TabStop = False
        Me._Frame_Problem_5.Text = "T2-Funktion"
        '
        'Text6
        '
        Me.Text6.AcceptsReturn = True
        Me.Text6.AutoSize = False
        Me.Text6.BackColor = System.Drawing.SystemColors.Control
        Me.Text6.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text6.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text6.ForeColor = System.Drawing.Color.Red
        Me.Text6.Location = New System.Drawing.Point(8, 16)
        Me.Text6.MaxLength = 0
        Me.Text6.Multiline = True
        Me.Text6.Name = "Text6"
        Me.Text6.ReadOnly = True
        Me.Text6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text6.Size = New System.Drawing.Size(177, 25)
        Me.Text6.TabIndex = 18
        Me.Text6.Text = "Multikriterielles Testproblem (Konkav)" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10)
        '
        '_Frame_Problem_4
        '
        Me._Frame_Problem_4.BackColor = System.Drawing.SystemColors.Control
        Me._Frame_Problem_4.Controls.Add(Me.Text4)
        Me._Frame_Problem_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me._Frame_Problem_4, CType(4, Short))
        Me._Frame_Problem_4.Location = New System.Drawing.Point(0, 640)
        Me._Frame_Problem_4.Name = "_Frame_Problem_4"
        Me._Frame_Problem_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Frame_Problem_4.Size = New System.Drawing.Size(217, 73)
        Me._Frame_Problem_4.TabIndex = 15
        Me._Frame_Problem_4.TabStop = False
        Me._Frame_Problem_4.Text = "T1-Funktion"
        '
        'Text4
        '
        Me.Text4.AcceptsReturn = True
        Me.Text4.AutoSize = False
        Me.Text4.BackColor = System.Drawing.SystemColors.Control
        Me.Text4.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text4.ForeColor = System.Drawing.Color.Red
        Me.Text4.Location = New System.Drawing.Point(8, 16)
        Me.Text4.MaxLength = 0
        Me.Text4.Multiline = True
        Me.Text4.Name = "Text4"
        Me.Text4.ReadOnly = True
        Me.Text4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text4.Size = New System.Drawing.Size(177, 25)
        Me.Text4.TabIndex = 16
        Me.Text4.Text = "Multikriterielles Testproblem (Konvex)" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10)
        '
        '_Frame_Problem_3
        '
        Me._Frame_Problem_3.BackColor = System.Drawing.SystemColors.Control
        Me._Frame_Problem_3.Controls.Add(Me.Text5)
        Me._Frame_Problem_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me._Frame_Problem_3, CType(3, Short))
        Me._Frame_Problem_3.Location = New System.Drawing.Point(0, 640)
        Me._Frame_Problem_3.Name = "_Frame_Problem_3"
        Me._Frame_Problem_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Frame_Problem_3.Size = New System.Drawing.Size(217, 73)
        Me._Frame_Problem_3.TabIndex = 13
        Me._Frame_Problem_3.TabStop = False
        Me._Frame_Problem_3.Text = "D1-Funktion"
        '
        'Text5
        '
        Me.Text5.AcceptsReturn = True
        Me.Text5.AutoSize = False
        Me.Text5.BackColor = System.Drawing.SystemColors.Control
        Me.Text5.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text5.ForeColor = System.Drawing.Color.Red
        Me.Text5.Location = New System.Drawing.Point(8, 16)
        Me.Text5.MaxLength = 0
        Me.Text5.Multiline = True
        Me.Text5.Name = "Text5"
        Me.Text5.ReadOnly = True
        Me.Text5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text5.Size = New System.Drawing.Size(185, 49)
        Me.Text5.TabIndex = 14
        Me.Text5.Text = "Multikriterielles Testproblem (Konvex)" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10)
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.Combo1)
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(0, 592)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(217, 49)
        Me.Frame1.TabIndex = 11
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Test-Funktion"
        '
        'Combo1
        '
        Me.Combo1.BackColor = System.Drawing.SystemColors.Window
        Me.Combo1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Combo1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Combo1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Combo1.Location = New System.Drawing.Point(8, 16)
        Me.Combo1.Name = "Combo1"
        Me.Combo1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Combo1.Size = New System.Drawing.Size(201, 21)
        Me.Combo1.TabIndex = 12
        '
        '_Frame_Problem_0
        '
        Me._Frame_Problem_0.BackColor = System.Drawing.SystemColors.Control
        Me._Frame_Problem_0.Controls.Add(Me.Par_Sinus)
        Me._Frame_Problem_0.Controls.Add(Me.Text1)
        Me._Frame_Problem_0.Controls.Add(Me.Label4)
        Me._Frame_Problem_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me._Frame_Problem_0, CType(0, Short))
        Me._Frame_Problem_0.Location = New System.Drawing.Point(0, 640)
        Me._Frame_Problem_0.Name = "_Frame_Problem_0"
        Me._Frame_Problem_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Frame_Problem_0.Size = New System.Drawing.Size(217, 73)
        Me._Frame_Problem_0.TabIndex = 1
        Me._Frame_Problem_0.TabStop = False
        Me._Frame_Problem_0.Text = "Sinus-Funktion"
        '
        'Par_Sinus
        '
        Me.Par_Sinus.AcceptsReturn = True
        Me.Par_Sinus.AutoSize = False
        Me.Par_Sinus.BackColor = System.Drawing.SystemColors.Window
        Me.Par_Sinus.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Par_Sinus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Par_Sinus.Location = New System.Drawing.Point(152, 40)
        Me.Par_Sinus.MaxLength = 0
        Me.Par_Sinus.Name = "Par_Sinus"
        Me.Par_Sinus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Par_Sinus.Size = New System.Drawing.Size(41, 19)
        Me.Par_Sinus.TabIndex = 3
        Me.Par_Sinus.Text = "50"
        '
        'Text1
        '
        Me.Text1.AcceptsReturn = True
        Me.Text1.AutoSize = False
        Me.Text1.BackColor = System.Drawing.SystemColors.Control
        Me.Text1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text1.ForeColor = System.Drawing.Color.Red
        Me.Text1.Location = New System.Drawing.Point(8, 16)
        Me.Text1.MaxLength = 0
        Me.Text1.Multiline = True
        Me.Text1.Name = "Text1"
        Me.Text1.ReadOnly = True
        Me.Text1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text1.Size = New System.Drawing.Size(193, 25)
        Me.Text1.TabIndex = 2
        Me.Text1.Text = "Parameter an Sinusfunktion anpassen." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10)
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(16, 40)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(121, 17)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = " Anzahl der Parameter"
        '
        'Command1
        '
        Me.Command1.BackColor = System.Drawing.SystemColors.Control
        Me.Command1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Command1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Command1.Location = New System.Drawing.Point(584, 600)
        Me.Command1.Name = "Command1"
        Me.Command1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Command1.Size = New System.Drawing.Size(112, 33)
        Me.Command1.TabIndex = 0
        Me.Command1.Text = ">"
        '
        '_Frame_Problem_1
        '
        Me._Frame_Problem_1.BackColor = System.Drawing.SystemColors.Control
        Me._Frame_Problem_1.Controls.Add(Me.Text2)
        Me._Frame_Problem_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me._Frame_Problem_1, CType(1, Short))
        Me._Frame_Problem_1.Location = New System.Drawing.Point(0, 640)
        Me._Frame_Problem_1.Name = "_Frame_Problem_1"
        Me._Frame_Problem_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Frame_Problem_1.Size = New System.Drawing.Size(217, 73)
        Me._Frame_Problem_1.TabIndex = 5
        Me._Frame_Problem_1.TabStop = False
        Me._Frame_Problem_1.Text = "Beale-Problem"
        '
        'Text2
        '
        Me.Text2.AcceptsReturn = True
        Me.Text2.AutoSize = False
        Me.Text2.BackColor = System.Drawing.SystemColors.Control
        Me.Text2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text2.ForeColor = System.Drawing.Color.Red
        Me.Text2.Location = New System.Drawing.Point(8, 16)
        Me.Text2.MaxLength = 0
        Me.Text2.Multiline = True
        Me.Text2.Name = "Text2"
        Me.Text2.ReadOnly = True
        Me.Text2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text2.Size = New System.Drawing.Size(201, 49)
        Me.Text2.TabIndex = 6
        Me.Text2.Text = "Es wird das Minimum des Beale-Problems gesucht ( x = (3,  0.5),  F(x) = 0)" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10)
        '
        '_Frame_Problem_2
        '
        Me._Frame_Problem_2.BackColor = System.Drawing.SystemColors.Control
        Me._Frame_Problem_2.Controls.Add(Me.Text3)
        Me._Frame_Problem_2.Controls.Add(Me.Par_Schwefel)
        Me._Frame_Problem_2.Controls.Add(Me.Label6)
        Me._Frame_Problem_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me._Frame_Problem_2, CType(2, Short))
        Me._Frame_Problem_2.Location = New System.Drawing.Point(0, 640)
        Me._Frame_Problem_2.Name = "_Frame_Problem_2"
        Me._Frame_Problem_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Frame_Problem_2.Size = New System.Drawing.Size(217, 73)
        Me._Frame_Problem_2.TabIndex = 7
        Me._Frame_Problem_2.TabStop = False
        Me._Frame_Problem_2.Text = "Schwefel 2.4"
        '
        'Text3
        '
        Me.Text3.AcceptsReturn = True
        Me.Text3.AutoSize = False
        Me.Text3.BackColor = System.Drawing.SystemColors.Control
        Me.Text3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text3.ForeColor = System.Drawing.Color.Red
        Me.Text3.Location = New System.Drawing.Point(8, 16)
        Me.Text3.MaxLength = 0
        Me.Text3.Multiline = True
        Me.Text3.Name = "Text3"
        Me.Text3.ReadOnly = True
        Me.Text3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text3.Size = New System.Drawing.Size(185, 33)
        Me.Text3.TabIndex = 9
        Me.Text3.Text = "Minimum der Problemstellung wird gesucht (xi=1, F(x)=0)" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10)
        '
        'Par_Schwefel
        '
        Me.Par_Schwefel.AcceptsReturn = True
        Me.Par_Schwefel.AutoSize = False
        Me.Par_Schwefel.BackColor = System.Drawing.SystemColors.Window
        Me.Par_Schwefel.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Par_Schwefel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Par_Schwefel.Location = New System.Drawing.Point(136, 48)
        Me.Par_Schwefel.MaxLength = 0
        Me.Par_Schwefel.Name = "Par_Schwefel"
        Me.Par_Schwefel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Par_Schwefel.Size = New System.Drawing.Size(41, 19)
        Me.Par_Schwefel.TabIndex = 8
        Me.Par_Schwefel.Text = "5"
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(16, 48)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(121, 17)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = " Anzahl der Parameter"
        '
        'TeeCommander1
        '
        Me.TeeCommander1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
        Me.TeeCommander1.Chart = Me.TChart1
        Me.TeeCommander1.Divider = False
        Me.TeeCommander1.Dock = System.Windows.Forms.DockStyle.None
        Me.TeeCommander1.DropDownArrows = True
        Me.TeeCommander1.LabelValues = True
        Me.TeeCommander1.Location = New System.Drawing.Point(232, 600)
        Me.TeeCommander1.Name = "TeeCommander1"
        Me.TeeCommander1.ShowToolTips = True
        Me.TeeCommander1.Size = New System.Drawing.Size(344, 35)
        Me.TeeCommander1.TabIndex = 33
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(704, 719)
        Me.Controls.Add(Me.TeeCommander1)
        Me.Controls.Add(Me.TChart1)
        Me.Controls.Add(Me.EVO_Opt_Verlauf1)
        Me.Controls.Add(Me.EVO_Einstellungen1)
        Me.Controls.Add(Me._Frame_Problem_10)
        Me.Controls.Add(Me._Frame_Problem_9)
        Me.Controls.Add(Me._Frame_Problem_8)
        Me.Controls.Add(Me._Frame_Problem_7)
        Me.Controls.Add(Me._Frame_Problem_6)
        Me.Controls.Add(Me._Frame_Problem_5)
        Me.Controls.Add(Me._Frame_Problem_4)
        Me.Controls.Add(Me._Frame_Problem_3)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me._Frame_Problem_0)
        Me.Controls.Add(Me.Command1)
        Me.Controls.Add(Me._Frame_Problem_1)
        Me.Controls.Add(Me._Frame_Problem_2)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Location = New System.Drawing.Point(156, 170)
        Me.Name = "Form1"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Evolutionsstrategie"
        Me._Frame_Problem_10.ResumeLayout(False)
        Me._Frame_Problem_9.ResumeLayout(False)
        Me._Frame_Problem_8.ResumeLayout(False)
        Me._Frame_Problem_7.ResumeLayout(False)
        Me._Frame_Problem_6.ResumeLayout(False)
        Me._Frame_Problem_5.ResumeLayout(False)
        Me._Frame_Problem_4.ResumeLayout(False)
        Me._Frame_Problem_3.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        Me._Frame_Problem_0.ResumeLayout(False)
        Me._Frame_Problem_1.ResumeLayout(False)
        Me._Frame_Problem_2.ResumeLayout(False)
        CType(Me.Frame_Problem, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
#Region "Aktualisierungssupport "
	Private Shared m_vb6FormDefInstance As Form1
	Private Shared m_InitializingDefInstance As Boolean
	Public Shared Property DefInstance() As Form1
		Get
			If m_vb6FormDefInstance Is Nothing OrElse m_vb6FormDefInstance.IsDisposed Then
				m_InitializingDefInstance = True
				m_vb6FormDefInstance = New Form1()
				m_InitializingDefInstance = False
			End If
			DefInstance = m_vb6FormDefInstance
		End Get
		Set
			m_vb6FormDefInstance = Value
		End Set
	End Property
#End Region 
	'option Base 1O
	
    Dim myIsOK As Boolean
    Dim myisrun As Boolean
    Dim globalAnzPar As Short
    Dim globalAnzZiel As Short
    Dim globalAnzRand As Short
    Dim OptErg() As Double
    Dim array_x() As Double
    Dim array_y() As Double
    Dim Bestwert(,) As Double
    Dim Population(,) As Double
    Dim mypara(,) As Double
	
    Private Sub Combo1_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Combo1.SelectedIndexChanged
        If Form1.DefInstance.IsInitializing = True Then
            Exit Sub
        Else
            Select Case Combo1.Text
                Case "Sinus-Funktion"
                    Frame_Problem(0).BringToFront()
                Case "Beale-Problem"
                    Frame_Problem(1).BringToFront()
                Case "Schwefel 2.4-Problem"
                    Frame_Problem(2).BringToFront()
                Case "DEB 1"
                    Frame_Problem(3).BringToFront()
                Case "Zitzler/Deb T1"
                    Frame_Problem(4).BringToFront()
                Case "Zitzler/Deb T2"
                    Frame_Problem(5).BringToFront()
                Case "Zitzler/Deb T3"
                    Frame_Problem(6).BringToFront()
                Case "Zitzler/Deb T4"
                    Frame_Problem(7).BringToFront()
                Case "CONSTR"
                    Frame_Problem(8).BringToFront()
                Case "Box"
                    Frame_Problem(9).BringToFront()
            End Select
        End If
    End Sub

    Private Sub Command1_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command1.Click
        myisrun = True
        myIsOK = ES_STARTEN()
    End Sub

    Private Sub Command2_Click()
        myisrun = False
    End Sub



    Private Sub EVO_Einstellungen1_ModusChanges(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles EVO_Einstellungen1.ModusChanges
        Dim OptimierungsModus As Integer
        OptimierungsModus = EVO_Einstellungen1.OptModus
        Select Case OptimierungsModus
            Case 1
                Combo1.Items.Clear()
                Combo1.Items.Add("Sinus-Funktion")
                Combo1.Items.Add("Beale-Problem")
                Combo1.Items.Add("Schwefel 2.4-Problem")
                Combo1.SelectedIndex = 0
            Case 2
                Combo1.Items.Clear()
                Combo1.Items.Add("Deb 1")
                Combo1.Items.Add("Zitzler/Deb T1")
                Combo1.Items.Add("Zitzler/Deb T2")
                Combo1.Items.Add("Zitzler/Deb T3")
                Combo1.Items.Add("Zitzler/Deb T4")
                Combo1.Items.Add("CONSTR")
                Combo1.Items.Add("Box")
                Combo1.SelectedIndex = 0
        End Select
    End Sub

    Private Sub Form1_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Combo1.Items.Add("Sinus-Funktion")
        Combo1.Items.Add("Beale-Problem")
        Combo1.Items.Add("Schwefel 2.4-Problem")
        'Combo1.Items.Add("Deb 1")
        'Combo1.Items.Add("Zitzler/Deb T1")
        'Combo1.Items.Add("Zitzler/Deb T2")
        'Combo1.Items.Add("Zitzler/Deb T3")
        'Combo1.Items.Add("Zitzler/Deb T4")
        'Combo1.Items.Add("CONSTR")
        'Combo1.Items.Add("Box")
        Combo1.SelectedIndex = 0
        TeeCommander1.Chart = TChart1
    End Sub

    Private Function ES_STARTEN() As Boolean
        '==========================
        Dim isOK As Boolean
        Dim i As Integer
        Dim j As Short
        Dim Txt As String
        '--------------------------
        Dim durchlauf As Integer
        '--------------------------
        Dim evolutionsstrategie As dmevodll.CEvolutionsstrategie
        '--------------------------
        'Variablen für Optionen Evostrategie
        Dim iEvoTyp, iPopEvoTyp As Integer
        Dim isPOPUL, isMultiObjective As Boolean
        Dim NPopEltern, NRunden, NPopul, iOptPopEltern As Integer
        Dim iOptEltern, iPopPenalty As Integer
        Dim NEltern As Integer
        Dim NRekomb, NRekombXY As Integer
        Dim rDeltaStart As Single
        Dim iStartPar As Integer
        Dim isMUTREKOMB As Boolean
        Dim isdnvektor, isINIVARIA, isPareto As Boolean
        Dim isPareto3D As Boolean
        Dim NGen, NNachf As Integer
        Dim Interact As Short
        Dim isInteract As Boolean
        Dim NMemberSecondPop As Short


        '--------------------------
        Dim ipop As Short
        Dim igen As Short
        Dim inachf As Short
        Dim irunde As Short
        Dim QN() As Double
        Dim RN() As Double
        Dim Versuch As Short
        '--------------------------


        'On Error GoTo Err_ES_STARTEN

        ES_STARTEN = False

        'If (ipop + igen + inachf + irunde) > 4 Then GoTo Start_Evolutionsrunden

        myisrun = True

        'Werte an Variablen übergeben
        iEvoTyp = EVO_Einstellungen1.iEvoTyp
        iPopEvoTyp = EVO_Einstellungen1.iPopEvoTyp
        isPOPUL = EVO_Einstellungen1.isPOPUL
        isMultiObjective = EVO_Einstellungen1.isMultiObjective
        NRunden = EVO_Einstellungen1.NRunden
        NPopul = EVO_Einstellungen1.NPopul
        NPopEltern = EVO_Einstellungen1.NPopEltern
        iOptPopEltern = EVO_Einstellungen1.iOptPopEltern
        iOptEltern = EVO_Einstellungen1.iOptEltern
        iPopPenalty = EVO_Einstellungen1.iPopPenalty
        NGen = EVO_Einstellungen1.NGen
        NEltern = EVO_Einstellungen1.NEltern
        NNachf = EVO_Einstellungen1.NNachf
        NRekombXY = EVO_Einstellungen1.NRekombXY
        rDeltaStart = EVO_Einstellungen1.rDeltaStart
        isdnvektor = EVO_Einstellungen1.isDnVektor
        iStartPar = EVO_Einstellungen1.globalOPTVORGABE
        isPareto = EVO_Einstellungen1.isPareto
        isPareto3D = False
        Interact = EVO_Einstellungen1.Interact
        isInteract = EVO_Einstellungen1.isInteract
        NMemberSecondPop = EVO_Einstellungen1.NMemberSecondPop

        Select Case Combo1.Text
            Case "Sinus-Funktion"
                globalAnzPar = CShort(Par_Sinus.Text)
                globalAnzZiel = 1
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                For i = 1 To globalAnzPar
                    mypara(i, 1) = 0
                Next
                Call Sinuskurve()
            Case "Beale-Problem" 'x1 = [-5;5], x2=[-2;2]
                globalAnzPar = 2
                globalAnzZiel = 1
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                mypara(1, 1) = 0.5
                mypara(2, 1) = 0.5
                Call Ausgangswert_Beale()
            Case "Schwefel 2.4-Problem" 'xi = [-10,10]
                globalAnzPar = CShort(Par_Schwefel.Text)
                globalAnzZiel = 1
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                For i = 1 To globalAnzPar
                    mypara(i, 1) = 1
                Next i
                Call Ausgangswert_Schwefel24()
            Case "Deb 1" 'x1 = [0.1;1], x2=[0;5]
                globalAnzPar = 2
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                mypara(1, 1) = Rnd()
                mypara(2, 1) = Rnd()
                Call Ausgangswert_D1()
            Case "Zitzler/Deb T1" 'xi = [0,1]
                globalAnzPar = 30
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                For i = 1 To globalAnzPar
                    mypara(i, 1) = Rnd()
                Next i
                Call Ausgangswert_T1()
            Case "Zitzler/Deb T2" 'xi = [0,1]
                globalAnzPar = 30
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                For i = 1 To globalAnzPar
                    mypara(i, 1) = Rnd()
                Next i
                Call Ausgangswert_T2()
            Case "Zitzler/Deb T3" 'xi = [0,1]
                globalAnzPar = 15
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                For i = 1 To globalAnzPar
                    mypara(i, 1) = Rnd()
                Next i
                Call Ausgangswert_T3()
            Case "Zitzler/Deb T4" 'x1 = [0,1], xi=[-5,5]
                globalAnzPar = 10
                globalAnzZiel = 2
                globalAnzRand = 0
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                For i = 1 To globalAnzPar
                    mypara(i, 1) = Rnd()
                Next i
                Call Ausgangswert_T4()
            Case "CONSTR" 'x1 = [0.1;1], x2=[0;5]
                globalAnzPar = 2
                globalAnzZiel = 2
                globalAnzRand = 2
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                mypara(1, 1) = Rnd()
                mypara(2, 1) = Rnd()
                Call Ausgangswert_CONSTR()
            Case "Box"
                globalAnzPar = 3
                globalAnzZiel = 3
                globalAnzRand = 2
                ReDim mypara(globalAnzPar, 1)
                Randomize()
                mypara(1, 1) = Rnd()
                mypara(2, 1) = Rnd()
                mypara(3, 1) = Rnd()
                Call Ausgangswert_Box()
        End Select

        ReDim QN(globalAnzZiel)
        ReDim RN(globalAnzRand)

        'Kontrolle der Variablen
        If NRunden = 0 Or NPopul = 0 Or NPopEltern = 0 Then
            Txt = "Anzahl der Runden, Populationen oder Populationseltern ist zu klein!"
            GoTo ErrCode_ES_STARTEN
        End If
        If NGen = 0 Or NEltern = 0 Or NNachf = 0 Then
            Txt = "Anzahl der Generationen, Eltern oder Nachfolger ist zu klein!"
            GoTo ErrCode_ES_STARTEN
        End If
        If rDeltaStart < 0 Then
            Txt = "Die Startschrittweite ist unzulässig oder kleiner als die minimale Schrittweite!"
            GoTo ErrCode_ES_STARTEN
        End If
        If globalAnzPar = 0 Then
            Txt = "Die Anzahl der Parameter ist unzulässig!"
            GoTo ErrCode_ES_STARTEN
        End If
        If NPopul < NPopEltern Then
            Txt = "Die Anzahl der Populationseltern darf nicht größer als die Anzahl der Populationen sein!"
            GoTo ErrCode_ES_STARTEN
        End If



        '***************************************************************************************************
        'CEvolutionsstrategie, 1. Schritt
        '***************************************************************************************************
        'Objekt der Klasse CEvolutionsstrategie wird erzeugen
        '***************************************************************************************************
        '***************************************************************************************************
        evolutionsstrategie = New dmevodll.CEvolutionsstrategie

        '***************************************************************************************************
        'CEvolutionsstrategie - ES_INI, 2. Schritt
        '***************************************************************************************************
        'Die öffentlichen dynamischen Arrays werden initialisiert (Dn, An, Xn, Xmin, Xmax)
        'und die Anzahl der Zuielfunktionen wird festgelegt
        '***************************************************************************************************
        '***************************************************************************************************
        isOK = evolutionsstrategie.EsIni(globalAnzPar, globalAnzZiel, globalAnzRand)

        '***************************************************************************************************
        'CEvolutionsstrategie - ES_OPTIONS, 3. Schritt
        '***************************************************************************************************
        'Optionen der Evolutionsstrategie werden übergeben
        '***************************************************************************************************
        '***************************************************************************************************
        isOK = evolutionsstrategie.EsOptions(iEvoTyp, iPopEvoTyp, isPOPUL, NRunden, NPopul, NPopEltern, iOptPopEltern, iOptEltern, iPopPenalty, NGen, NEltern, NNachf, NRekombXY, rDeltaStart, iStartPar, isdnvektor, isMultiObjective, isPareto, isPareto3D, Interact, isInteract, NMemberSecondPop)

        '***************************************************************************************************
        'CEvolutionsstrategie - ES_ES_LET_PARAMETER, 4. Schritt
        '***************************************************************************************************
        'Ausgangsparameter werden übergeben
        '***************************************************************************************************
        '***************************************************************************************************
        For i = 1 To globalAnzPar
            myIsOK = evolutionsstrategie.EsLetParameter(i, mypara(i, 1))
        Next i

        '***************************************************************************************************
        'CEvolutionsstrategie - ES_PREPARE, 5. Schritt
        '***************************************************************************************************
        'Interne Variablen werden initialisiert, Zufallsgenerator wird initialisiert
        '***************************************************************************************************
        '***************************************************************************************************
        myIsOK = evolutionsstrategie.EsPrepare

        '***************************************************************************************************
        'CEvolutionsstrategie - ES_STARTVALUES, 6. Schritt
        '***************************************************************************************************
        'Startwerte werden zugewiesen
        '***************************************************************************************************
        '***************************************************************************************************
        myIsOK = evolutionsstrategie.EsStartvalues


        '***************************************************************************************************
        '***************************************************************************************************
        'Startwerte werden der Bedienoberfläche zugewiesen
        '***************************************************************************************************
        '***************************************************************************************************
        EVO_Opt_Verlauf1.NRunden = evolutionsstrategie.NRunden
        EVO_Opt_Verlauf1.NPopul = evolutionsstrategie.NPopul
        EVO_Opt_Verlauf1.NGen = evolutionsstrategie.NGen
        EVO_Opt_Verlauf1.NNachf = evolutionsstrategie.NNachf
        EVO_Opt_Verlauf1.Initialisieren()

        durchlauf = 0

Start_Evolutionsrunden:
        'Cursor setzen
        'System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        '***********************************************************************************************
        'Loop über alle Runden
        '***********************************************************************************************
        Do While (evolutionsstrategie.EsIsNextRunde)

            irunde = evolutionsstrategie.iaktuelleRunde
            Call EVO_Opt_Verlauf1.Runden(irunde)

            myIsOK = evolutionsstrategie.EsPopBestwertspeicher()
            '***********************************************************************************************
            'Loop über alle Populationen
            '***********************************************************************************************
            Do While (evolutionsstrategie.EsIsNextPop)

                ipop = evolutionsstrategie.iaktuellePopulation
                Call EVO_Opt_Verlauf1.Populationen(ipop)

                myIsOK = evolutionsstrategie.EsPopVaria

                myIsOK = evolutionsstrategie.EsPopMutation

                durchlauf = NGen * NNachf * (irunde - 1)

                '***********************************************************************************************
                'Loop über alle Generationen
                '***********************************************************************************************
                Do While (evolutionsstrategie.EsIsNextGen)

                    igen = evolutionsstrategie.iaktuelleGeneration
                    Call EVO_Opt_Verlauf1.Generation(igen)

                    myIsOK = evolutionsstrategie.EsBestwertspeicher()
                    '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    'Loop über alle Nachkommen
                    '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    Do While (evolutionsstrategie.EsIsNextNachf)

                        inachf = evolutionsstrategie.iaktuellerNachfahre
                        Call EVO_Opt_Verlauf1.Nachfolger(inachf)

                        durchlauf = durchlauf + 1

                        'Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern
                        myIsOK = evolutionsstrategie.EsVaria

                        'Mutieren der Ausgangswerte
                        myIsOK = evolutionsstrategie.EsMutation

                        'Auslesen der Variierten Parameter
                        myIsOK = evolutionsstrategie.EsGetParameter(globalAnzPar, mypara)

                        'Auslesen des Bestwertspeichers
                        If Not evolutionsstrategie.isMultiObjective Then
                            myIsOK = evolutionsstrategie.EsGetBestwert(Bestwert)
                        End If

                        'Bestimmen der Qualitätsfunktion
                        myIsOK = Zielfunktion(globalAnzPar, mypara, durchlauf, Bestwert, ipop, QN, RN)

                        'Einordnen der Qualitätsfunktion im Bestwertspeicher
                        myIsOK = evolutionsstrategie.EsBest(QN, RN)

                        System.Windows.Forms.Application.DoEvents()

                        '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        'Ende Loop über alle Nachkommen
                        '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    Loop


                    'Die neuen Eltern werden generiert
                    myIsOK = evolutionsstrategie.EsEltern()

                    If evolutionsstrategie.isMultiObjective Then
                        myIsOK = evolutionsstrategie.EsGetBestwert(Bestwert)
                        myIsOK = evolutionsstrategie.esGetSekundärePopulation(Population)
                    End If

                    'Bestwerte und sekundäre Population werden gezeichnet
                    If evolutionsstrategie.isMultiObjective Then
                        'Call Bestwertzeichnen_Pareto(Bestwert, ipop)
                        Call SekundärePopulationZeichnen(Population)
                    End If

                    System.Windows.Forms.Application.DoEvents()

                    '***********************************************************************************************
                    'Ende Loop über alle Generationen
                    '***********************************************************************************************
                Loop 'Schleife über alle Generationen

                System.Windows.Forms.Application.DoEvents()

                'Einordnen der Qualitätsfunktion im PopulationsBestwertspeicher
                myIsOK = evolutionsstrategie.EsPopBest

                '***********************************************************************************************
                'Ende Loop über alle Populationen
                '***********************************************************************************************
            Loop 'Schleife über alle Populationen



            'Die neuen Populationseltern werden generiert
            myIsOK = evolutionsstrategie.EsPopEltern

            System.Windows.Forms.Application.DoEvents()

            '***********************************************************************************************
            'Ende Loop über alle Runden
            '***********************************************************************************************
        Loop 'Schleife über alle Runden

        '***************************************************************************************************
        'CEvolutionsstrategie, letzter. Schritt
        '***************************************************************************************************
        'Objekt der Klasse CEvolutionsstrategie wird vernichtet
        '***************************************************************************************************
        '***************************************************************************************************
        'UPGRADE_NOTE: Das Objekt evolutionsstrategie kann erst dann gelöscht werden, wenn die Garbagecollection durchgeführt wurde. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1029"'
        '$$Ersetzen durch dispose funzt net
        evolutionsstrategie = Nothing
        ES_STARTEN = True

EXIT_ES_STARTEN:
        'Cursor setzen
        'System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Exit Function
        'xxxxxxxxxxxxxxxxxxxxxxxxxx
Err_ES_STARTEN:
        Beep()
        MsgBox("ES_STARTEN: " & ErrorToString(), MsgBoxStyle.Critical)
        Resume EXIT_ES_STARTEN
ErrCode_ES_STARTEN:
        Beep()
        MsgBox("ES_STARTEN: " & Txt, MsgBoxStyle.Information)
        GoTo EXIT_ES_STARTEN
    End Function
    'Private Function Zielfunktion(AnzPar As Integer, Par() As Double, durchlauf As Long, Bestwert() As Double, ipop As Integer, Optional QN2 As Double) As double
    Private Function Zielfunktion(ByRef AnzPar As Short, ByRef Par(,) As Double, ByRef durchlauf As Integer, ByRef Bestwert(,) As Double, ByRef ipop As Short, ByRef QN() As Double, ByRef RN() As Double) As Boolean
        Dim i As Short
        Dim Unterteilung_X As Double
        Dim x1, x2 As Double
        Dim X() As Double
        Dim f2, f1, f3 As Double
        Dim g1, g2 As Double

        Select Case Combo1.Text
            Case "Sinus-Funktion" 'Fehlerquadrate zur Sinusfunktion |0-2pi|
                Unterteilung_X = 2 * 3.1415926535898 / (AnzPar - 1)
                QN(1) = 0
                For i = 1 To AnzPar
                    QN(1) = QN(1) + (System.Math.Sin((i - 1) * Unterteilung_X) - (-1 + (Par(i, 1) * 2))) * (System.Math.Sin((i - 1) * Unterteilung_X) - (-1 + Par(i, 1) * 2))
                Next i
                'If durchlauf Mod 25 = 0 Then
                Call Zielfunktion_zeichnen(AnzPar, Par, durchlauf, ipop)
                'End If
            Case "Beale-Problem" 'Beale-Problem
                x1 = -5 + (Par(1, 1) * 10)
                x2 = -2 + (Par(2, 1) * 4)

                QN(1) = (1.5 - x1 * (1 - x2)) ^ 2 + (2.25 - x1 * (1 - x2) ^ 2) ^ 2 + (2.625 - x1 * (1 - x2) ^ 3) ^ 2

                Call Zielfunktion_zeichnen2(QN(1), durchlauf, ipop)
            Case "Schwefel 2.4-Problem" 'Schwefel 2.4 S. 329
                ReDim X(globalAnzPar)
                For i = 1 To globalAnzPar
                    X(i) = -10 + Par(i, 1) * 20
                Next i
                QN(1) = 0
                For i = 1 To globalAnzPar
                    QN(1) = QN(1) + ((X(1) - X(i) ^ 2) ^ 2 + (X(i) - 1) ^ 2)
                Next i
                Call Zielfunktion_zeichnen2(QN(1), durchlauf, ipop)
            Case "Deb 1" 'Deb 2000, D1 (Konvexe Pareto-Front)
                f1 = Par(1, 1) * (9 / 10) + 0.1
                f2 = (1 + 5 * Par(2, 1)) / (Par(1, 1) * (9 / 10) + 0.1)
                QN(1) = f1
                QN(2) = f2
                Call Zielfunktion_zeichnen3(f1, f2, ipop)
            Case "Zitzler/Deb T1" 'Zitzler/Deb/Thiele 2000, T1 (Konvexe Pareto-Front)
                f1 = Par(1, 1)
                f2 = 0
                For i = 2 To globalAnzPar
                    f2 = f2 + Par(i, 1)
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - System.Math.Sqrt(f1 / f2))
                QN(1) = f1
                QN(2) = f2
                Call Zielfunktion_zeichnen3(f1, f2, ipop)
            Case "Zitzler/Deb T2" 'Zitzler/Deb/Thiele 2000, T2 (Non-Konvexe Pareto-Front)
                f1 = Par(1, 1)
                f2 = 0
                For i = 2 To globalAnzPar
                    f2 = f2 + Par(i, 1)
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - (f1 / f2) * (f1 / f2))
                QN(1) = f1
                QN(2) = f2
                'Call Zielfunktion_zeichnen3(f1, f2, ipop)
            Case "Zitzler/Deb T3" 'Zitzler/Deb/Thiele 2000, T3 (disconected Pareto-Front)
                f1 = Par(1, 1)
                f2 = 0
                For i = 2 To globalAnzPar
                    f2 = f2 + Par(i, 1)
                Next i
                f2 = 1 + 9 / (globalAnzPar - 1) * f2
                f2 = f2 * (1 - System.Math.Sqrt(f1 / f2) - (f1 / f2) * System.Math.Sin(10 * 3.14159265358979 * f1))
                QN(1) = f1
                QN(2) = f2
                Call Zielfunktion_zeichnen3(f1, f2, ipop)
            Case "Zitzler/Deb T4" 'Zitzler/Deb/Thiele 2000, T4 (local/global Pareto-Fronts)
                f1 = Par(1, 1)
                f2 = 0
                For i = 2 To globalAnzPar
                    x2 = -5 + (Par(i, 1) * 10)
                    f2 = f2 + (x2 * x2 - 10 * System.Math.Cos(4 * 3.14159265358979 * x2))
                Next i
                f2 = 1 + 10 * (globalAnzPar - 1) + f2
                f2 = f2 * (1 - System.Math.Sqrt(f1 / f2))
                QN(1) = f1
                QN(2) = f2
                'Call Zielfunktion_zeichnen3(f1, f2, ipop)
            Case "CONSTR"
                f1 = Par(1, 1) * (9 / 10) + 0.1
                f2 = (1 + 5 * Par(2, 1)) / (Par(1, 1) * (9 / 10) + 0.1)

                g1 = (5 * Par(2, 1)) + 9 * (Par(1, 1) * (9 / 10) + 0.1) - 6
                g2 = (-1) * (5 * Par(2, 1)) + 9 * (Par(1, 1) * (9 / 10) + 0.1) - 1

                QN(1) = f1
                QN(2) = f2
                RN(1) = g1
                RN(2) = g2
                Call Zielfunktion_zeichnen3(f1, f2, ipop)
            Case "Box"
                f1 = Par(1, 1) ^ 2
                f2 = Par(2, 1) ^ 2
                f3 = Par(3, 1) ^ 2
                g1 = Par(1, 1) + Par(3, 1) - 0.5
                g2 = Par(1, 1) + Par(2, 1) + Par(3, 1) - 0.8

                '                f1 = 1 + (1 - Par(1, 1)) ^ 5
                '                f2 = Par(2, 1)
                '                f3 = Par(3, 1)
                '
                '                g1 = Par(1, 1) ^ 2 + Par(3, 1) ^ 2 - 0.5
                '                g2 = Par(2, 1) ^ 2 + Par(3, 1) ^ 2 - 0.5

                QN(1) = f1
                QN(2) = f2
                QN(3) = f3
                RN(1) = g1
                RN(2) = g2
                'Call Zielfunktion_zeichnen4(f1, f2, f3)
        End Select
    End Function

    Private Sub Sinuskurve()
        Dim i As Short
        Dim Datenmenge As Short
        Dim Unterteilung_X As Double
        Dim Anzahl_Kalkulationen As Integer
        Dim Populationen As Short

        Datenmenge = CShort(Par_Sinus.Text)
        If EVO_Einstellungen1.isPOPUL Then
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf * EVO_Einstellungen1.NRunden
        Else
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf
        End If

        Unterteilung_X = 2 * 3.141592654 / (Datenmenge - 1)

        ReDim array_x(Datenmenge - 1)
        ReDim array_y(Datenmenge - 1)

        For i = 0 To Datenmenge - 1
            array_x(i) = System.Math.Round((i) * Unterteilung_X, 2)
            array_y(i) = System.Math.Sin((i) * Unterteilung_X)
        Next i

        With TChart1
            .Clear()
            .Header.Text = "Anpassung an Sinus-Kurve"
            .Chart.Axes.Left.Title.Caption = "Funktionswert"
            .Chart.Axes.Bottom.Title.Caption = "Stützstelle"
            .Aspect.View3D = False
            .Legend.Visible = False
            Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
            Line1.Add(array_x, array_y)
            Populationen = 1
            If EVO_Einstellungen1.isPOPUL Then
                Populationen = EVO_Einstellungen1.NPopul
            End If
            For i = 1 To Populationen
                Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)   '$ Variablenname Point1 wird in der Schleife mehrmals verwendet!
            Next i
            .Chart.Axes.Bottom.Automatic = True
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Minimum = -1
            .Chart.Axes.Left.Maximum = 1
            .Chart.Axes.Left.Increment = 0.2
        End With

    End Sub

    Private Sub Ausgangswert_Beale()
        Dim Ausgangsergebnis As Double
        Dim Anzahl_Kalkulationen As Short
        Dim Populationen As Short
        Dim i As Short


        If EVO_Einstellungen1.isPOPUL Then
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf * EVO_Einstellungen1.NRunden
        Else
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf
        End If

        ReDim OptErg(Anzahl_Kalkulationen)

        Ausgangsergebnis = (1.5 - 0.5 * (1 - 0.5)) ^ 2 + (2.25 - 0.5 * (1 - 0.5) ^ 2) ^ 2 + (2.625 - 0.5 * (1 - 0.5) ^ 3) ^ 2

        ReDim array_y(Anzahl_Kalkulationen - 1)
        ReDim array_x(Anzahl_Kalkulationen - 1)
        For i = 0 To Anzahl_Kalkulationen - 1
            array_y(i) = Ausgangsergebnis
            array_x(i) = i + 1
        Next i

        With TChart1
            .Clear()
            .Header.Text = "Beale-Problem-Funktionswerte"
            .Chart.Axes.Left.Title.Caption = "Funktionswert"
            .Chart.Axes.Bottom.Title.Caption = "Berechnungsschritt"
            .Aspect.View3D = False
            .Legend.Visible = False

            'Linie zeichen
            Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
            Line1.Add(array_x, array_y)
            Line1.Brush.Color = System.Drawing.Color.Red
            Line1.ClickableLine = True

            'Punkt einfügen
            Populationen = 1
            If EVO_Einstellungen1.isPOPUL Then
                Populationen = EVO_Einstellungen1.NPopul
            End If

            For i = 1 To Populationen
                Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
                Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                Point1.Pointer.HorizSize = 3
                Point1.Pointer.VertSize = 3
            Next

            'Axen Formatieren
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Maximum = Anzahl_Kalkulationen
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Maximum = Ausgangsergebnis * 1.3
            .Chart.Axes.Left.Minimum = 0
            .Chart.Axes.Left.Logarithmic = False
        End With

    End Sub

    Private Sub Ausgangswert_Schwefel24()
        Dim Ausgangsergebnis As Double
        Dim Populationen As Short

        Dim Anzahl_Kalkulationen As Integer
        Dim i As Short
        Dim X() As Double

        If EVO_Einstellungen1.isPOPUL Then
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf * EVO_Einstellungen1.NRunden
        Else
            Anzahl_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf
        End If

        ReDim X(globalAnzPar)
        For i = 1 To globalAnzPar
            X(i) = 10
        Next i

        Ausgangsergebnis = 0
        For i = 1 To globalAnzPar
            Ausgangsergebnis = Ausgangsergebnis + ((X(1) - X(i) ^ 2) ^ 2 + (X(i) - 1) ^ 2)
        Next i

        ReDim array_y(Anzahl_Kalkulationen - 1)
        ReDim array_x(Anzahl_Kalkulationen - 1)
        For i = 0 To Anzahl_Kalkulationen - 1
            array_y(i) = Ausgangsergebnis
            array_x(i) = i + 1
        Next i

        With TChart1
            .Clear()
            .Header.Text = "Schwefel-Problem 2.4"
            .Aspect.View3D = False
            .Legend.Visible = False

            'Linie der Anfangswerte 
            Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
            Line1.Add(array_x, array_y)
            Line1.Color = System.Drawing.Color.Red

            'Anzahl Populationen
            Populationen = 1
            If EVO_Einstellungen1.isPOPUL Then
                Populationen = EVO_Einstellungen1.NPopul
            End If

            'Für jede Population eine Series
            For i = 1 To Populationen
                Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
                Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                Point1.Pointer.HorizSize = 3
                Point1.Pointer.VertSize = 3
            Next i

            'Formatierung der Axen
            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Maximum = Anzahl_Kalkulationen
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Maximum = Ausgangsergebnis * 1.2
            .Chart.Axes.Left.Minimum = -1
            .Chart.Axes.Left.Logarithmic = False
        End With
    End Sub

    Private Sub Ausgangswert_D1()
        Dim Populationen As Short
        Dim i, j As Short
        Dim ArrayX(100) As Double
        Dim ArrayY(100) As Double

        Populationen = EVO_Einstellungen1.NPopul

        With TChart1
            .Clear()
            .Header.Text = "Deb D1 - MO-konvex"
            .Aspect.View3D = False
            .Legend.Visible = False

            'S0: Punkt einfügen dient nur dazu um die Series 0 zu besetzen
            Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
            Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point1.Pointer.HorizSize = 3
            Point1.Pointer.VertSize = 3

            'S1: Hier wird nur eine Population gezeichnet.
            Dim Point2 As New Steema.TeeChart.Styles.Points(.Chart)
            Point2.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point2.Color = System.Drawing.Color.Orange
            Point2.Pointer.HorizSize = 2
            Point2.Pointer.VertSize = 2

            'S2: Series für die Sekundäre Population
            Dim Point3 As New Steema.TeeChart.Styles.Points(.Chart)
            Point3.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point3.Color = System.Drawing.Color.Blue
            Point3.Pointer.HorizSize = 3
            Point3.Pointer.VertSize = 3

            'Hier muss ein Fehler in der Rechnung sein. die beiden Linien liegen aufeinander.
            'S3: Linie 1 wird errechnet und gezeichnet
            For j = 0 To 100
                ArrayX(j) = 0.1 + j * 0.009
                ArrayY(j) = 1 / ArrayX(j)
            Next j
            Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
            Line1.Brush.Color = System.Drawing.Color.Green
            Line1.ClickableLine = True
            .Series(3).Add(ArrayX, ArrayY)

            'S4: Linie 2 wird errechnet und gezeichnet
            For j = 0 To 100
                ArrayY(j) = (1 + 5) / ArrayX(j)
            Next j
            Dim Line2 As New Steema.TeeChart.Styles.Line(.Chart)
            Line2.Brush.Color = System.Drawing.Color.Red
            Line2.ClickableLine = True
            .Series(4).Add(ArrayX, ArrayY)

            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Maximum = 1
            .Chart.Axes.Bottom.Minimum = 0.1
            .Chart.Axes.Bottom.Increment = 0.1
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Maximum = 10
            .Chart.Axes.Left.Minimum = 0
            .Chart.Axes.Left.Increment = 2
        End With
    End Sub

    Private Sub Ausgangswert_T1()
        Dim Populationen As Short
        Dim i, j As Short
        Dim ArrayX(1000) As Double
        Dim ArrayY(1000) As Double

        If EVO_Einstellungen1.isPOPUL Then
            Populationen = EVO_Einstellungen1.NPopul
        Else
            Populationen = 1
        End If

        With TChart1
            .Clear()
            .Header.Text = "Zitzler/Deb/Theile T1"
            .Aspect.View3D = False
            .Legend.Visible = False

            'S0: Punkt einfügen dient nur dazu um die Series 0 zu besetzen
            Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
            Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point1.Pointer.HorizSize = 3
            Point1.Pointer.VertSize = 3

            'S1: Hier wird nur eine Population gezeichnet.
            Dim Point2 As New Steema.TeeChart.Styles.Points(.Chart)
            Point2.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point2.Color = System.Drawing.Color.Orange
            Point2.Pointer.HorizSize = 2
            Point2.Pointer.VertSize = 2

            'S2: Series für die Sekundäre Population
            Dim Point3 As New Steema.TeeChart.Styles.Points(.Chart)
            Point3.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point3.Color = System.Drawing.Color.Blue
            Point3.Pointer.HorizSize = 3
            Point3.Pointer.VertSize = 3

            'S3: Serie für die Grenze
            For j = 0 To 1000
                ArrayX(j) = j / 1000
                ArrayY(j) = 1 - System.Math.Sqrt(ArrayX(j))
            Next j

            Dim Line1 As New Steema.TeeChart.Styles.Line(.Chart)
            Line1.Brush.Color = System.Drawing.Color.Green
            Line1.ClickableLine = True
            .Series(3).Add(ArrayX, ArrayY)

            .Chart.Axes.Bottom.Automatic = False
            .Chart.Axes.Bottom.Maximum = 1
            .Chart.Axes.Bottom.Minimum = 0
            .Chart.Axes.Bottom.Increment = 0.2
            .Chart.Axes.Left.Automatic = False
            .Chart.Axes.Left.Maximum = 4
            .Chart.Axes.Left.Minimum = 0
            .Chart.Axes.Left.Increment = 0.5
        End With

    End Sub

    Private Sub Ausgangswert_T2()
        Dim Populationen As Short
        Dim i, j As Short
        Dim ArrayX(100) As Double
        Dim ArrayY(100) As Double

        If EVO_Einstellungen1.isPOPUL Then
            Populationen = EVO_Einstellungen1.NPopul
        Else
            Populationen = 1
        End If

        With TChart1
            .Clear()
            .Header.Text = "Zitzler/Deb/Theile T2"
            .Aspect.View3D = False
            .Legend.Visible = False
            '.AddSeries(TeeChart.ESeriesClass.scPoint)
            '.Series(0).asPoint.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '.Series(0).asPoint.Pointer.HorizontalSize = 1
            '.Series(0).asPoint.Pointer.VerticalSize = 1
            'For i = 1 To Populationen
            '    .AddSeries(TeeChart.ESeriesClass.scPoint)
            '    .Series(i).asPoint.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '    .Series(i).asPoint.Pointer.HorizontalSize = 3
            '    .Series(i).asPoint.Pointer.VerticalSize = 3
            'Next i
            '.AddSeries(TeeChart.ESeriesClass.scLine)
            'For j = 0 To 100
            '    ArrayX(j) = j / 100
            '    ArrayY(j) = 1 - (ArrayX(j) * ArrayX(j))
            'Next j
            '.Series(Populationen + 1).AddArray(100, ArrayY, ArrayX)

            '.AddSeries(TeeChart.ESeriesClass.scPoint)
            '.Series(Populationen + 2).asPoint.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '.Series(Populationen + 2).asPoint.Pointer.HorizontalSize = 2
            '.Series(Populationen + 2).asPoint.Pointer.VerticalSize = 2
            '.Series(Populationen + 2).Color = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red))

            '.Axis.Bottom.Automatic = False
            '.Axis.Bottom.Maximum = 1
            '.Axis.Bottom.Minimum = 0
            '.Axis.Bottom.Increment = 0.2
            ''        .Axis.Left.Automatic = True
            '.Axis.Left.Automatic = False
            '.Axis.Left.Maximum = 4
            '.Axis.Left.Minimum = 0
        End With
    End Sub

    Private Sub Ausgangswert_T3()
        Dim Populationen As Short
        Dim i, j As Short
        Dim ArrayX(100) As Double
        Dim ArrayY(100) As Double

        Populationen = EVO_Einstellungen1.NPopul

        With TChart1
            .Clear()
            .Header.Text = "Zitzler/Deb/Theile T3"
            .Aspect.View3D = False
            .Legend.Visible = False
            '.AddSeries(TeeChart.ESeriesClass.scPoint)
            '.Series(0).asPoint.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '.Series(0).asPoint.Pointer.HorizontalSize = 1
            '.Series(0).asPoint.Pointer.VerticalSize = 1
            'For i = 1 To Populationen
            '    .AddSeries(TeeChart.ESeriesClass.scPoint)
            '    .Series(i).asPoint.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '    .Series(i).asPoint.Pointer.HorizontalSize = 3
            '    .Series(i).asPoint.Pointer.VerticalSize = 3
            'Next i
            '.AddSeries(TeeChart.ESeriesClass.scLine)
            'For j = 0 To 100
            '    ArrayX(j) = j / 100
            '    ArrayY(j) = 1 - System.Math.Sqrt(ArrayX(j)) - ArrayX(j) * System.Math.Sin(10 * 3.14159265358979 * ArrayX(j))
            'Next j
            '.Series(Populationen + 1).AddArray(100, ArrayY, ArrayX)

            '.AddSeries(TeeChart.ESeriesClass.scPoint)
            '.Series(Populationen + 2).asPoint.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '.Series(Populationen + 2).asPoint.Pointer.HorizontalSize = 2
            '.Series(Populationen + 2).asPoint.Pointer.VerticalSize = 2
            '.Series(Populationen + 2).Color = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red))

            '.Axis.Bottom.Automatic = False
            '.Axis.Bottom.Maximum = 1
            '.Axis.Bottom.Minimum = 0
            '.Axis.Bottom.Increment = 0.2
            '.Axis.Left.Automatic = True
            '        .Axis.Left.Maximum = 2
            '        .Axis.Left.Minimum = -1
            '        .Axis.Left.Increment = 0.5
        End With

    End Sub

    Private Sub Ausgangswert_T4()
        Dim Populationen As Short
        Dim i, j As Short
        Dim ArrayX(1000) As Double
        Dim ArrayY(1000) As Double

        Populationen = EVO_Einstellungen1.NPopul

        With TChart1
            .Clear()
            .Header.Text = "Zitzler/Deb/Theile T1"
            .Aspect.View3D = False
            .Legend.Visible = False
            '.AddSeries(TeeChart.ESeriesClass.scPoint)
            '.Series(0).asPoint.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '.Series(0).asPoint.Pointer.HorizontalSize = 1
            '.Series(0).asPoint.Pointer.VerticalSize = 1
            'For i = 1 To Populationen
            '    .AddSeries(TeeChart.ESeriesClass.scPoint)
            '    .Series(i).asPoint.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '    .Series(i).asPoint.Pointer.HorizontalSize = 3
            '    .Series(i).asPoint.Pointer.VerticalSize = 3
            'Next i


            'For i = 1 To 10
            '    .AddSeries(TeeChart.ESeriesClass.scLine)
            '    .Series(Populationen + i).asLine.LinePen.Width = 2
            '    .Series(Populationen + i).Color = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue))
            '    For j = 0 To 1000
            '        ArrayX(j) = j / 1000
            '        ArrayY(j) = (1 + (i - 1) / 4) * (1 - System.Math.Sqrt(ArrayX(j) / (1 + (i - 1) / 4)))
            '    Next j
            '    .Series(Populationen + i).AddArray(1000, ArrayY, ArrayX)
            'Next i


            '.Axis.Bottom.Automatic = False
            '.Axis.Bottom.Maximum = 1
            '.Axis.Bottom.Minimum = 0
            '.Axis.Bottom.Increment = 0.2
            '.Axis.Left.Automatic = True
            '        .Axis.Left.Maximum = 2
            '        .Axis.Left.Minimum = 0
            '        .Axis.Left.Increment = 0.5
        End With

    End Sub

    Private Sub Ausgangswert_CONSTR()
        Dim Populationen As Short
        Dim i, j As Short
        Dim ArrayX() As Double
        Dim ArrayY() As Double

        ReDim ArrayX(100)
        ReDim ArrayY(100)

        If EVO_Einstellungen1.isPOPUL Then
            Populationen = EVO_Einstellungen1.NPopul
        Else
            Populationen = 1
        End If

        With TChart1
            .Clear()
            .Header.Text = "CONSTR"
            .Aspect.View3D = False
            .Legend.Visible = False
            '.AddSeries(TeeChart.ESeriesClass.scPoint)
            '.Series(0).asPoint.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '.Series(0).asPoint.Pointer.HorizontalSize = 1
            '.Series(0).asPoint.Pointer.VerticalSize = 1
            'For i = 1 To Populationen
            '    .AddSeries(TeeChart.ESeriesClass.scPoint)
            '    .Series(i).asPoint.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '    .Series(i).asPoint.Pointer.HorizontalSize = 3
            '    .Series(i).asPoint.Pointer.VerticalSize = 3
            'Next i

            '.AddSeries(TeeChart.ESeriesClass.scLine)
            '.Series(Populationen + 1).asLine.LinePen.Width = 2
            '.Series(Populationen + 1).Color = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue))
            'For j = 0 To 100
            '    ArrayX(j) = 0.1 + j * 0.009
            '    ArrayY(j) = 1 / ArrayX(j)
            'Next j
            '.Series(Populationen + 1).AddArray(100, ArrayY, ArrayX)


            '.AddSeries(TeeChart.ESeriesClass.scPoint)
            '.Series(Populationen + 2).asPoint.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '.Series(Populationen + 2).asPoint.Pointer.HorizontalSize = 2
            '.Series(Populationen + 2).asPoint.Pointer.VerticalSize = 2
            '.Series(Populationen + 2).Color = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red))

            '.AddSeries(TeeChart.ESeriesClass.scLine)
            '.Series(Populationen + 3).asLine.LinePen.Width = 2
            '.Series(Populationen + 3).Color = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue))
            'For j = 0 To 100
            '    ArrayY(j) = (1 + 5) / ArrayX(j)
            'Next j
            '.Series(Populationen + 3).AddArray(100, ArrayY, ArrayX)

            '.AddSeries(TeeChart.ESeriesClass.scLine)
            '.Series(Populationen + 4).asLine.LinePen.Width = 1
            '.Series(Populationen + 4).Color = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black))
            'ReDim ArrayX(61)
            'ReDim ArrayY(61)
            'For j = 0 To 61
            '    ArrayX(j) = 0.1 + (j + 2) * 0.009
            '    ArrayY(j) = (7 - 9 * ArrayX(j)) / ArrayX(j)
            'Next j
            '.Series(Populationen + 4).AddArray(61, ArrayY, ArrayX)

            '.AddSeries(TeeChart.ESeriesClass.scLine)
            '.Series(Populationen + 5).asLine.LinePen.Width = 1
            '.Series(Populationen + 5).Color = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black))
            'ReDim ArrayX(61)
            'ReDim ArrayY(61)
            'For j = 0 To 61
            '    ArrayX(j) = 0.1 + (j + 2) * 0.009
            '    ArrayY(j) = (9 * ArrayX(j)) / ArrayX(j)
            'Next j
            '.Series(Populationen + 5).AddArray(61, ArrayY, ArrayX)

            '.Axis.Bottom.Automatic = False
            '.Axis.Bottom.Maximum = 1
            '.Axis.Bottom.Minimum = 0
            '.Axis.Bottom.Increment = 0.2
            '.Axis.Left.Automatic = False
            '.Axis.Left.Maximum = 10
            '.Axis.Left.Minimum = 0
            '.Axis.Left.Increment = 2
        End With
    End Sub

    Private Sub Ausgangswert_Box()
        Dim Populationen As Short
        Dim i, j As Short
        Dim ArrayX(100) As Double
        Dim ArrayY(100) As Double

        If EVO_Einstellungen1.isPOPUL Then
            Populationen = EVO_Einstellungen1.NPopul
        Else
            Populationen = 1
        End If

        With TChart1
            .Clear()
            .Header.Text = "Box"
            .Aspect.View3D = True
            .Aspect.Chart3DPercent = 60
            .Legend.Visible = False
            '.AddSeries(TeeChart.ESeriesClass.scPoint3D)
            '.Series(0).asPoint3D.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '.Series(0).asPoint3D.LinePen.Visible = False
            '.Series(0).asPoint3D.Pointer.HorizontalSize = 1
            '.Series(0).asPoint3D.Pointer.VerticalSize = 1
            'For i = 1 To Populationen
            '    .AddSeries(TeeChart.ESeriesClass.scPoint3D)
            '    .Series(i).asPoint3D.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '    .Series(i).asPoint3D.LinePen.Visible = False
            '    .Series(i).asPoint3D.Pointer.HorizontalSize = 3
            '    .Series(i).asPoint3D.Pointer.VerticalSize = 3
            'Next i
            '.AddSeries(TeeChart.ESeriesClass.scPoint3D)
            '.AddSeries(TeeChart.ESeriesClass.scPoint3D)
            '.Series(Populationen + 2).asPoint3D.Pointer.Style = TeeChart.EPointerStyle.psCircle
            '.Series(Populationen + 2).asPoint3D.LinePen.Visible = False
            '.Series(Populationen + 2).asPoint3D.Pointer.HorizontalSize = 2
            '.Series(Populationen + 2).asPoint3D.Pointer.VerticalSize = 2
            '.Series(Populationen + 2).Color = System.Convert.ToUInt32(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red))

            '.Axis.Bottom.Automatic = False
            '.Axis.Bottom.Maximum = 1
            '.Axis.Bottom.Minimum = 0
            '.Axis.Bottom.Increment = 0.2
            '.Axis.Left.Automatic = False
            '.Axis.Left.Maximum = 1
            '.Axis.Left.Minimum = 0
            '.Axis.Left.Increment = 0.2
            '.Axis.Depth.Automatic = False
            '.Axis.Depth.Visible = True
            '.Axis.Depth.Maximum = 1
            '.Axis.Depth.Minimum = 0
            '.Axis.Depth.Increment = 0.2
        End With
    End Sub

    Private Sub Zielfunktion_zeichnen(ByRef AnzPar As Short, ByRef Par(,) As Double, ByRef durchlauf As Integer, ByRef ipop As Short)
        Dim i As Short
        Dim x1, x2 As Double
        Dim Zielfunktion As Double
        Dim Datenmenge As Short
        Dim Unterteilung_X As Double

        Unterteilung_X = 2 * 3.141592654 / (AnzPar - 1)
        ReDim array_x(AnzPar - 1) '$$ jetzt richtig?
        ReDim array_y(AnzPar - 1) '$$ jetzt richtig?
        For i = 0 To AnzPar - 1
            array_x(i) = System.Math.Round((i) * Unterteilung_X, 2)
            array_y(i) = (-1 + Par(i + 1, 1) * 2)
        Next i

        With TChart1
            .Series(ipop).Clear()
            .Series(ipop).Add(array_x, array_y)
        End With
    End Sub

    Private Sub Zielfunktion_zeichnen2(ByRef Wert As Double, ByRef durchlauf As Integer, ByRef ipop As Short)
        Dim i As Short
        Dim x1, x2 As Double
        Dim Zielfunktion As Double
        Dim Datenmenge As Short
        Dim Unterteilung_X As Double

        TChart1.Series(ipop).Add(durchlauf, Wert, "")

    End Sub

    Private Sub Zielfunktion_zeichnen3(ByRef f1 As Double, ByRef f2 As Double, ByRef ipop As Short)

        TChart1.Series(1).Add(f1, f2, "")

    End Sub

    Private Sub Zielfunktion_zeichnen4(ByRef f1 As Double, ByRef f2 As Double, ByRef f3 As Double)

        'TChart1.Series(0).Add(f1, f2, f3, "") '$$$ 3D-Reihe!

    End Sub

    Private Sub Bestwertzeichnen_Pareto(ByRef Bestwert(,) As Double, ByRef ipop As Short)
        Dim i As Short
        With TChart1
            .Series(ipop).Clear()
            If UBound(Bestwert, 2) = 2 Then
                For i = 1 To UBound(Bestwert, 1)
                    .Series(ipop).Add(Bestwert(i, 1), Bestwert(i, 2), "")
                Next i
            ElseIf UBound(Bestwert, 2) = 3 Then
                For i = 1 To UBound(Bestwert, 1)
                    '.Series(ipop).Add(Bestwert(i, 1), Bestwert(i, 2), Bestwert(i, 2), "") !$$$ 3D-Punkt!
                Next i
            End If
        End With
    End Sub

    Private Sub SekundärePopulationZeichnen(ByRef Population(,) As Double)
        Dim i As Short
        Dim Datenreihe As Short
        With TChart1
            If EVO_Einstellungen1.isPOPUL Then
                Datenreihe = EVO_Einstellungen1.NPopul + 2
            Else
                Datenreihe = 2
            End If
            .Series(Datenreihe).Clear()
            If UBound(Population, 2) = 2 Then
                For i = 1 To UBound(Population, 1)
                    .Series(Datenreihe).Add(Population(i, 1), Population(i, 2), "")
                Next i
            ElseIf UBound(Population, 2) = 3 Then
                For i = 1 To UBound(Population, 1)
                    '.Series(Datenreihe).Add(Population(i, 1), Population(i, 2), Population(i, 3), "") '$$$ 3D-Reihe!
                Next i
            End If
        End With
    End Sub

    '$$ Welchen zweck hat das?
    Private Sub Par_Sinus_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles Par_Sinus.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        'UPGRADE_ISSUE: Zuweisung wird nicht unterstützt: KeyAscii an Nicht-Null-Wert Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1058"'
        KeyAscii = KEYOK(KeyAscii, AllowIntegerOnly)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub
End Class