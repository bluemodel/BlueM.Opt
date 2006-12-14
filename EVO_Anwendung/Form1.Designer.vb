<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.textbox_Datensatz = New System.Windows.Forms.TextBox
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
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
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TChart1
        '
        '
        '
        '
        Me.TChart1.Aspect.ElevationFloat = 345
        Me.TChart1.Aspect.RotationFloat = 345
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Bottom.Automatic = True
        '
        '
        '
        Me.TChart1.Axes.Bottom.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.TChart1.Axes.Bottom.Grid.ZPosition = 0
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Bottom.Labels.Font.Shadow.Visible = False
        Me.TChart1.Axes.Bottom.Labels.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        '
        '
        Me.TChart1.Axes.Bottom.Labels.Shadow.Visible = False
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Bottom.Title.Font.Shadow.Visible = False
        Me.TChart1.Axes.Bottom.Title.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        '
        '
        Me.TChart1.Axes.Bottom.Title.Shadow.Visible = False
        '
        '
        '
        Me.TChart1.Axes.Depth.Automatic = True
        '
        '
        '
        Me.TChart1.Axes.Depth.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.TChart1.Axes.Depth.Grid.ZPosition = 0
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Depth.Labels.Font.Shadow.Visible = False
        Me.TChart1.Axes.Depth.Labels.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        '
        '
        Me.TChart1.Axes.Depth.Labels.Shadow.Visible = False
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Depth.Title.Font.Shadow.Visible = False
        Me.TChart1.Axes.Depth.Title.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        '
        '
        Me.TChart1.Axes.Depth.Title.Shadow.Visible = False
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Automatic = True
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.TChart1.Axes.DepthTop.Grid.ZPosition = 0
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Labels.Font.Shadow.Visible = False
        Me.TChart1.Axes.DepthTop.Labels.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Labels.Shadow.Visible = False
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Title.Font.Shadow.Visible = False
        Me.TChart1.Axes.DepthTop.Title.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        '
        '
        Me.TChart1.Axes.DepthTop.Title.Shadow.Visible = False
        '
        '
        '
        Me.TChart1.Axes.Left.Automatic = True
        '
        '
        '
        Me.TChart1.Axes.Left.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.TChart1.Axes.Left.Grid.ZPosition = 0
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Left.Labels.Font.Shadow.Visible = False
        Me.TChart1.Axes.Left.Labels.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        '
        '
        Me.TChart1.Axes.Left.Labels.Shadow.Visible = False
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Left.Title.Font.Shadow.Visible = False
        Me.TChart1.Axes.Left.Title.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        '
        '
        Me.TChart1.Axes.Left.Title.Shadow.Visible = False
        '
        '
        '
        Me.TChart1.Axes.Right.Automatic = True
        '
        '
        '
        Me.TChart1.Axes.Right.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.TChart1.Axes.Right.Grid.ZPosition = 0
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Right.Labels.Font.Shadow.Visible = False
        Me.TChart1.Axes.Right.Labels.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        '
        '
        Me.TChart1.Axes.Right.Labels.Shadow.Visible = False
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Right.Title.Font.Shadow.Visible = False
        Me.TChart1.Axes.Right.Title.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        '
        '
        Me.TChart1.Axes.Right.Title.Shadow.Visible = False
        '
        '
        '
        Me.TChart1.Axes.Top.Automatic = True
        '
        '
        '
        Me.TChart1.Axes.Top.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dash
        Me.TChart1.Axes.Top.Grid.ZPosition = 0
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Top.Labels.Font.Shadow.Visible = False
        Me.TChart1.Axes.Top.Labels.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        '
        '
        Me.TChart1.Axes.Top.Labels.Shadow.Visible = False
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Top.Title.Font.Shadow.Visible = False
        Me.TChart1.Axes.Top.Title.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        '
        '
        Me.TChart1.Axes.Top.Title.Shadow.Visible = False
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Footer.Font.Shadow.Visible = False
        Me.TChart1.Footer.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        '
        '
        Me.TChart1.Footer.Shadow.Visible = False
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Header.Font.Shadow.Visible = False
        Me.TChart1.Header.Font.Unit = System.Drawing.GraphicsUnit.World
        Me.TChart1.Header.Lines = New String() {"TeeChart"}
        '
        '
        '
        Me.TChart1.Header.Shadow.Visible = False
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.Legend.Font.Shadow.Visible = False
        Me.TChart1.Legend.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        '
        '
        '
        '
        '
        Me.TChart1.Legend.Title.Font.Bold = True
        '
        '
        '
        Me.TChart1.Legend.Title.Font.Shadow.Visible = False
        Me.TChart1.Legend.Title.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        '
        '
        Me.TChart1.Legend.Title.Pen.Visible = False
        '
        '
        '
        Me.TChart1.Legend.Title.Shadow.Visible = False
        Me.TChart1.Location = New System.Drawing.Point(232, 0)
        Me.TChart1.Name = "TChart1"
        '
        '
        '
        '
        '
        '
        Me.TChart1.Panel.Shadow.Visible = False
        Me.TChart1.Size = New System.Drawing.Size(465, 584)
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.SubFooter.Font.Shadow.Visible = False
        Me.TChart1.SubFooter.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        '
        '
        Me.TChart1.SubFooter.Shadow.Visible = False
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.TChart1.SubHeader.Font.Shadow.Visible = False
        Me.TChart1.SubHeader.Font.Unit = System.Drawing.GraphicsUnit.World
        '
        '
        '
        Me.TChart1.SubHeader.Shadow.Visible = False
        Me.TChart1.TabIndex = 29
        '
        '
        '
        '
        '
        '
        Me.TChart1.Walls.Back.AutoHide = False
        '
        '
        '
        Me.TChart1.Walls.Back.Shadow.Visible = False
        '
        '
        '
        Me.TChart1.Walls.Bottom.AutoHide = False
        '
        '
        '
        Me.TChart1.Walls.Bottom.Shadow.Visible = False
        '
        '
        '
        Me.TChart1.Walls.Left.AutoHide = False
        '
        '
        '
        Me.TChart1.Walls.Left.Shadow.Visible = False
        '
        '
        '
        Me.TChart1.Walls.Right.AutoHide = False
        '
        '
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
        Me._Frame_Problem_10.Text = "BlauesModell"
        '
        'Text11
        '
        Me.Text11.AcceptsReturn = True
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
        Me.Text11.Size = New System.Drawing.Size(203, 49)
        Me.Text11.TabIndex = 28
        Me.Text11.Text = "Blaues Modell" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Einstellungen bitte rechts vornehmen"
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
        Me.Text10.Text = "Multikriterielles Testproblem (Kreis)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "mit zwei Randbedingungen" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
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
        Me.Text9.Text = "Multikriterielles Testproblem (Konvex)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "mit zwei Randbedingungen" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
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
        Me.Text8.Text = "Multikriterielles Testproblem (Konvex)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
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
        Me.Text7.Text = "Multikriterielles Testproblem (Konvex, " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "nicht stetig)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
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
        Me.Text6.Text = "Multikriterielles Testproblem (Konkav)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
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
        Me.Text4.Text = "Multikriterielles Testproblem (Konvex)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
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
        Me.Text5.Text = "Multikriterielles Testproblem (Konvex)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
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
        Me.Par_Sinus.BackColor = System.Drawing.SystemColors.Window
        Me.Par_Sinus.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Par_Sinus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Par_Sinus.Location = New System.Drawing.Point(152, 40)
        Me.Par_Sinus.MaxLength = 0
        Me.Par_Sinus.Name = "Par_Sinus"
        Me.Par_Sinus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Par_Sinus.Size = New System.Drawing.Size(41, 20)
        Me.Par_Sinus.TabIndex = 3
        Me.Par_Sinus.Text = "50"
        '
        'Text1
        '
        Me.Text1.AcceptsReturn = True
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
        Me.Text1.Text = "Parameter an Sinusfunktion anpassen." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
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
        Me.Command1.UseVisualStyleBackColor = False
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
        Me.Text2.Text = "Es wird das Minimum des Beale-Problems gesucht ( x = (3,  0.5),  F(x) = 0)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
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
        Me.Text3.Text = "Minimum der Problemstellung wird gesucht (xi=1, F(x)=0)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Par_Schwefel
        '
        Me.Par_Schwefel.AcceptsReturn = True
        Me.Par_Schwefel.BackColor = System.Drawing.SystemColors.Window
        Me.Par_Schwefel.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Par_Schwefel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Par_Schwefel.Location = New System.Drawing.Point(136, 48)
        Me.Par_Schwefel.MaxLength = 0
        Me.Par_Schwefel.Name = "Par_Schwefel"
        Me.Par_Schwefel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Par_Schwefel.Size = New System.Drawing.Size(41, 20)
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
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.textbox_Datensatz)
        Me.GroupBox1.Location = New System.Drawing.Point(703, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(285, 310)
        Me.GroupBox1.TabIndex = 34
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "BlauesModell"
        Me.GroupBox1.Visible = False
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(255, 26)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(24, 23)
        Me.Button1.TabIndex = 26
        Me.Button1.Text = "..."
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "Datensatz:"
        '
        'textbox_Datensatz
        '
        Me.textbox_Datensatz.Location = New System.Drawing.Point(70, 28)
        Me.textbox_Datensatz.Name = "textbox_Datensatz"
        Me.textbox_Datensatz.Size = New System.Drawing.Size(179, 20)
        Me.textbox_Datensatz.TabIndex = 24
        Me.textbox_Datensatz.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = "ALL-Dateien|*.ALL"
        Me.OpenFileDialog1.Title = "Datensatz auswählen"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(992, 719)
        Me.Controls.Add(Me.GroupBox1)
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
        Me.Controls.Add(Me._Frame_Problem_2)
        Me.Controls.Add(Me._Frame_Problem_1)
        Me.Controls.Add(Me._Frame_Problem_0)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.Command1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Location = New System.Drawing.Point(156, 170)
        Me.Name = "Form1"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Evolutionsstrategie"
        Me._Frame_Problem_10.ResumeLayout(False)
        Me._Frame_Problem_10.PerformLayout()
        Me._Frame_Problem_9.ResumeLayout(False)
        Me._Frame_Problem_9.PerformLayout()
        Me._Frame_Problem_8.ResumeLayout(False)
        Me._Frame_Problem_8.PerformLayout()
        Me._Frame_Problem_7.ResumeLayout(False)
        Me._Frame_Problem_7.PerformLayout()
        Me._Frame_Problem_6.ResumeLayout(False)
        Me._Frame_Problem_6.PerformLayout()
        Me._Frame_Problem_5.ResumeLayout(False)
        Me._Frame_Problem_5.PerformLayout()
        Me._Frame_Problem_4.ResumeLayout(False)
        Me._Frame_Problem_4.PerformLayout()
        Me._Frame_Problem_3.ResumeLayout(False)
        Me._Frame_Problem_3.PerformLayout()
        Me.Frame1.ResumeLayout(False)
        Me._Frame_Problem_0.ResumeLayout(False)
        Me._Frame_Problem_0.PerformLayout()
        Me._Frame_Problem_1.ResumeLayout(False)
        Me._Frame_Problem_1.PerformLayout()
        Me._Frame_Problem_2.ResumeLayout(False)
        Me._Frame_Problem_2.PerformLayout()
        CType(Me.Frame_Problem, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region
#Region "Aktualisierungssupport "
    Private Shared m_vb6FormDefInstance As Form1
    Private Shared m_InitializingDefInstance As Boolean
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents textbox_Datensatz As System.Windows.Forms.TextBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Public Shared Property DefInstance() As Form1
        Get
            If m_vb6FormDefInstance Is Nothing OrElse m_vb6FormDefInstance.IsDisposed Then
                m_InitializingDefInstance = True
                m_vb6FormDefInstance = New Form1()
                m_InitializingDefInstance = False
            End If
            DefInstance = m_vb6FormDefInstance
        End Get
        Set(ByVal value As Form1)
            m_vb6FormDefInstance = value
        End Set
    End Property
#End Region
End Class
