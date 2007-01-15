<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    Public Sub New()
        MyBase.New()
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
    Public WithEvents Text_TKNFunktion As System.Windows.Forms.TextBox
    Public WithEvents Text_CONSTRFunktion As System.Windows.Forms.TextBox
    Public WithEvents Text_T4Funktion As System.Windows.Forms.TextBox
    Public WithEvents Text_T3Funktion As System.Windows.Forms.TextBox
    Public WithEvents Text_T2Funktion As System.Windows.Forms.TextBox
    Public WithEvents Text_T1Funktion As System.Windows.Forms.TextBox
    Public WithEvents Text_D1Funktion As System.Windows.Forms.TextBox
    Public WithEvents Combo_Testproblem As System.Windows.Forms.ComboBox
    Public WithEvents Text_Sinusfunktion_Par As System.Windows.Forms.TextBox
    Public WithEvents Text_Sinusfunktion As System.Windows.Forms.TextBox
    Public WithEvents Label_Sinusfunktion As System.Windows.Forms.Label
    Public WithEvents Button_Start As System.Windows.Forms.Button
    Public WithEvents Text_BealeProblem As System.Windows.Forms.TextBox
    Public WithEvents Text_Schwefel24 As System.Windows.Forms.TextBox
    Public WithEvents Text_Schwefel24_Par As System.Windows.Forms.TextBox
    Public WithEvents Label_Schwefel24 As System.Windows.Forms.Label
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
        Me.Problem_TKNFunktion = New System.Windows.Forms.GroupBox
        Me.Text_TKNFunktion = New System.Windows.Forms.TextBox
        Me.Problem_CONSTRFunktion = New System.Windows.Forms.GroupBox
        Me.Text_CONSTRFunktion = New System.Windows.Forms.TextBox
        Me.Problem_T4Funktion = New System.Windows.Forms.GroupBox
        Me.Text_T4Funktion = New System.Windows.Forms.TextBox
        Me.GroupBox_Testproblem = New System.Windows.Forms.GroupBox
        Me.Combo_Testproblem = New System.Windows.Forms.ComboBox
        Me.Problem_SinusFunktion = New System.Windows.Forms.GroupBox
        Me.Text_Sinusfunktion_Par = New System.Windows.Forms.TextBox
        Me.Text_Sinusfunktion = New System.Windows.Forms.TextBox
        Me.Label_Sinusfunktion = New System.Windows.Forms.Label
        Me.Problem_BealeProblem = New System.Windows.Forms.GroupBox
        Me.Text_BealeProblem = New System.Windows.Forms.TextBox
        Me.Problem_Schwefel24 = New System.Windows.Forms.GroupBox
        Me.Text_Schwefel24 = New System.Windows.Forms.TextBox
        Me.Text_Schwefel24_Par = New System.Windows.Forms.TextBox
        Me.Label_Schwefel24 = New System.Windows.Forms.Label
        Me.Problem_D1Funktion = New System.Windows.Forms.GroupBox
        Me.Text_D1Funktion = New System.Windows.Forms.TextBox
        Me.Problem_T1Funktion = New System.Windows.Forms.GroupBox
        Me.Text_T1Funktion = New System.Windows.Forms.TextBox
        Me.Problem_T2Funktion = New System.Windows.Forms.GroupBox
        Me.Text_T2Funktion = New System.Windows.Forms.TextBox
        Me.Problem_T3Funktion = New System.Windows.Forms.GroupBox
        Me.Text_T3Funktion = New System.Windows.Forms.TextBox
        Me.Button_Start = New System.Windows.Forms.Button
        Me.Frame_Problem = New Microsoft.VisualBasic.Compatibility.VB6.GroupBoxArray(Me.components)
        Me.TeeCommander1 = New Steema.TeeChart.Commander
        Me.GroupBox_Anwendung = New System.Windows.Forms.GroupBox
        Me.Label_Anwendung = New System.Windows.Forms.Label
        Me.ComboBox_Anwendung = New System.Windows.Forms.ComboBox
        Me.Problem_TKNFunktion.SuspendLayout()
        Me.Problem_CONSTRFunktion.SuspendLayout()
        Me.Problem_T4Funktion.SuspendLayout()
        Me.GroupBox_Testproblem.SuspendLayout()
        Me.Problem_SinusFunktion.SuspendLayout()
        Me.Problem_BealeProblem.SuspendLayout()
        Me.Problem_Schwefel24.SuspendLayout()
        Me.Problem_D1Funktion.SuspendLayout()
        Me.Problem_T1Funktion.SuspendLayout()
        Me.Problem_T2Funktion.SuspendLayout()
        Me.Problem_T3Funktion.SuspendLayout()
        CType(Me.Frame_Problem, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_Anwendung.SuspendLayout()
        Me.SuspendLayout()
        '
        'TChart1
        '
        '
        '
        '
        Me.TChart1.Aspect.ElevationFloat = 345
        Me.TChart1.Aspect.RotationFloat = 345
        Me.TChart1.Aspect.View3D = False
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
        Me.TChart1.Cursor = System.Windows.Forms.Cursors.Default
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
        Me.TChart1.Location = New System.Drawing.Point(233, 12)
        Me.TChart1.Name = "TChart1"
        '
        '
        '
        '
        '
        '
        Me.TChart1.Panel.Shadow.Visible = False
        Me.TChart1.Size = New System.Drawing.Size(465, 640)
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
        Me.EVO_Opt_Verlauf1.Location = New System.Drawing.Point(233, 698)
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
        Me.EVO_Einstellungen1.Location = New System.Drawing.Point(8, 187)
        Me.EVO_Einstellungen1.Name = "EVO_Einstellungen1"
        Me.EVO_Einstellungen1.Size = New System.Drawing.Size(225, 585)
        Me.EVO_Einstellungen1.TabIndex = 31
        '
        'Problem_TKNFunktion
        '
        Me.Problem_TKNFunktion.BackColor = System.Drawing.SystemColors.Control
        Me.Problem_TKNFunktion.Controls.Add(Me.Text_TKNFunktion)
        Me.Problem_TKNFunktion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me.Problem_TKNFunktion, CType(9, Short))
        Me.Problem_TKNFunktion.Location = New System.Drawing.Point(0, 43)
        Me.Problem_TKNFunktion.Name = "Problem_TKNFunktion"
        Me.Problem_TKNFunktion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Problem_TKNFunktion.Size = New System.Drawing.Size(217, 70)
        Me.Problem_TKNFunktion.TabIndex = 25
        Me.Problem_TKNFunktion.TabStop = False
        Me.Problem_TKNFunktion.Text = "TKN-Funktion"
        '
        'Text_TKNFunktion
        '
        Me.Text_TKNFunktion.AcceptsReturn = True
        Me.Text_TKNFunktion.BackColor = System.Drawing.SystemColors.Control
        Me.Text_TKNFunktion.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text_TKNFunktion.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text_TKNFunktion.ForeColor = System.Drawing.Color.Red
        Me.Text_TKNFunktion.Location = New System.Drawing.Point(8, 16)
        Me.Text_TKNFunktion.MaxLength = 0
        Me.Text_TKNFunktion.Multiline = True
        Me.Text_TKNFunktion.Name = "Text_TKNFunktion"
        Me.Text_TKNFunktion.ReadOnly = True
        Me.Text_TKNFunktion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text_TKNFunktion.Size = New System.Drawing.Size(177, 49)
        Me.Text_TKNFunktion.TabIndex = 26
        Me.Text_TKNFunktion.Text = "Multikriterielles Testproblem (Kreis)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "mit zwei Randbedingungen" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Problem_CONSTRFunktion
        '
        Me.Problem_CONSTRFunktion.BackColor = System.Drawing.SystemColors.Control
        Me.Problem_CONSTRFunktion.Controls.Add(Me.Text_CONSTRFunktion)
        Me.Problem_CONSTRFunktion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me.Problem_CONSTRFunktion, CType(8, Short))
        Me.Problem_CONSTRFunktion.Location = New System.Drawing.Point(0, 43)
        Me.Problem_CONSTRFunktion.Name = "Problem_CONSTRFunktion"
        Me.Problem_CONSTRFunktion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Problem_CONSTRFunktion.Size = New System.Drawing.Size(217, 70)
        Me.Problem_CONSTRFunktion.TabIndex = 23
        Me.Problem_CONSTRFunktion.TabStop = False
        Me.Problem_CONSTRFunktion.Text = "CONSTR-Funktion"
        '
        'Text_CONSTRFunktion
        '
        Me.Text_CONSTRFunktion.AcceptsReturn = True
        Me.Text_CONSTRFunktion.BackColor = System.Drawing.SystemColors.Control
        Me.Text_CONSTRFunktion.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text_CONSTRFunktion.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text_CONSTRFunktion.ForeColor = System.Drawing.Color.Red
        Me.Text_CONSTRFunktion.Location = New System.Drawing.Point(8, 16)
        Me.Text_CONSTRFunktion.MaxLength = 0
        Me.Text_CONSTRFunktion.Multiline = True
        Me.Text_CONSTRFunktion.Name = "Text_CONSTRFunktion"
        Me.Text_CONSTRFunktion.ReadOnly = True
        Me.Text_CONSTRFunktion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text_CONSTRFunktion.Size = New System.Drawing.Size(177, 49)
        Me.Text_CONSTRFunktion.TabIndex = 24
        Me.Text_CONSTRFunktion.Text = "Multikriterielles Testproblem (Konvex)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "mit zwei Randbedingungen" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Problem_T4Funktion
        '
        Me.Problem_T4Funktion.BackColor = System.Drawing.SystemColors.Control
        Me.Problem_T4Funktion.Controls.Add(Me.Text_T4Funktion)
        Me.Problem_T4Funktion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me.Problem_T4Funktion, CType(7, Short))
        Me.Problem_T4Funktion.Location = New System.Drawing.Point(0, 43)
        Me.Problem_T4Funktion.Name = "Problem_T4Funktion"
        Me.Problem_T4Funktion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Problem_T4Funktion.Size = New System.Drawing.Size(217, 70)
        Me.Problem_T4Funktion.TabIndex = 21
        Me.Problem_T4Funktion.TabStop = False
        Me.Problem_T4Funktion.Text = "T4-Funktion"
        '
        'Text_T4Funktion
        '
        Me.Text_T4Funktion.AcceptsReturn = True
        Me.Text_T4Funktion.BackColor = System.Drawing.SystemColors.Control
        Me.Text_T4Funktion.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text_T4Funktion.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text_T4Funktion.ForeColor = System.Drawing.Color.Red
        Me.Text_T4Funktion.Location = New System.Drawing.Point(8, 16)
        Me.Text_T4Funktion.MaxLength = 0
        Me.Text_T4Funktion.Multiline = True
        Me.Text_T4Funktion.Name = "Text_T4Funktion"
        Me.Text_T4Funktion.ReadOnly = True
        Me.Text_T4Funktion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text_T4Funktion.Size = New System.Drawing.Size(177, 25)
        Me.Text_T4Funktion.TabIndex = 22
        Me.Text_T4Funktion.Text = "Multikriterielles Testproblem (Konvex)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'GroupBox_Testproblem
        '
        Me.GroupBox_Testproblem.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox_Testproblem.Controls.Add(Me.Combo_Testproblem)
        Me.GroupBox_Testproblem.Controls.Add(Me.Problem_SinusFunktion)
        Me.GroupBox_Testproblem.Controls.Add(Me.Problem_BealeProblem)
        Me.GroupBox_Testproblem.Controls.Add(Me.Problem_Schwefel24)
        Me.GroupBox_Testproblem.Controls.Add(Me.Problem_D1Funktion)
        Me.GroupBox_Testproblem.Controls.Add(Me.Problem_T1Funktion)
        Me.GroupBox_Testproblem.Controls.Add(Me.Problem_T2Funktion)
        Me.GroupBox_Testproblem.Controls.Add(Me.Problem_T3Funktion)
        Me.GroupBox_Testproblem.Controls.Add(Me.Problem_T4Funktion)
        Me.GroupBox_Testproblem.Controls.Add(Me.Problem_CONSTRFunktion)
        Me.GroupBox_Testproblem.Controls.Add(Me.Problem_TKNFunktion)
        Me.GroupBox_Testproblem.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me.GroupBox_Testproblem, CType(11, Short))
        Me.GroupBox_Testproblem.Location = New System.Drawing.Point(10, 68)
        Me.GroupBox_Testproblem.Name = "GroupBox_Testproblem"
        Me.GroupBox_Testproblem.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox_Testproblem.Size = New System.Drawing.Size(217, 113)
        Me.GroupBox_Testproblem.TabIndex = 11
        Me.GroupBox_Testproblem.TabStop = False
        Me.GroupBox_Testproblem.Text = "Test-Probleme"
        '
        'Combo_Testproblem
        '
        Me.Combo_Testproblem.BackColor = System.Drawing.SystemColors.Window
        Me.Combo_Testproblem.Cursor = System.Windows.Forms.Cursors.Default
        Me.Combo_Testproblem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Combo_Testproblem.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Combo_Testproblem.Location = New System.Drawing.Point(8, 16)
        Me.Combo_Testproblem.Name = "Combo_Testproblem"
        Me.Combo_Testproblem.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Combo_Testproblem.Size = New System.Drawing.Size(201, 21)
        Me.Combo_Testproblem.TabIndex = 12
        '
        'Problem_SinusFunktion
        '
        Me.Problem_SinusFunktion.BackColor = System.Drawing.SystemColors.Control
        Me.Problem_SinusFunktion.Controls.Add(Me.Text_Sinusfunktion_Par)
        Me.Problem_SinusFunktion.Controls.Add(Me.Text_Sinusfunktion)
        Me.Problem_SinusFunktion.Controls.Add(Me.Label_Sinusfunktion)
        Me.Problem_SinusFunktion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me.Problem_SinusFunktion, CType(0, Short))
        Me.Problem_SinusFunktion.Location = New System.Drawing.Point(0, 43)
        Me.Problem_SinusFunktion.Name = "Problem_SinusFunktion"
        Me.Problem_SinusFunktion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Problem_SinusFunktion.Size = New System.Drawing.Size(217, 70)
        Me.Problem_SinusFunktion.TabIndex = 1
        Me.Problem_SinusFunktion.TabStop = False
        Me.Problem_SinusFunktion.Text = "Sinus-Funktion"
        '
        'Text_Sinusfunktion_Par
        '
        Me.Text_Sinusfunktion_Par.AcceptsReturn = True
        Me.Text_Sinusfunktion_Par.BackColor = System.Drawing.SystemColors.Window
        Me.Text_Sinusfunktion_Par.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text_Sinusfunktion_Par.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text_Sinusfunktion_Par.Location = New System.Drawing.Point(152, 40)
        Me.Text_Sinusfunktion_Par.MaxLength = 0
        Me.Text_Sinusfunktion_Par.Name = "Text_Sinusfunktion_Par"
        Me.Text_Sinusfunktion_Par.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text_Sinusfunktion_Par.Size = New System.Drawing.Size(41, 20)
        Me.Text_Sinusfunktion_Par.TabIndex = 3
        Me.Text_Sinusfunktion_Par.Text = "50"
        '
        'Text_Sinusfunktion
        '
        Me.Text_Sinusfunktion.AcceptsReturn = True
        Me.Text_Sinusfunktion.BackColor = System.Drawing.SystemColors.Control
        Me.Text_Sinusfunktion.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text_Sinusfunktion.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text_Sinusfunktion.ForeColor = System.Drawing.Color.Red
        Me.Text_Sinusfunktion.Location = New System.Drawing.Point(8, 16)
        Me.Text_Sinusfunktion.MaxLength = 0
        Me.Text_Sinusfunktion.Multiline = True
        Me.Text_Sinusfunktion.Name = "Text_Sinusfunktion"
        Me.Text_Sinusfunktion.ReadOnly = True
        Me.Text_Sinusfunktion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text_Sinusfunktion.Size = New System.Drawing.Size(193, 25)
        Me.Text_Sinusfunktion.TabIndex = 2
        Me.Text_Sinusfunktion.Text = "Parameter an Sinusfunktion anpassen." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Label_Sinusfunktion
        '
        Me.Label_Sinusfunktion.BackColor = System.Drawing.SystemColors.Control
        Me.Label_Sinusfunktion.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_Sinusfunktion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_Sinusfunktion.Location = New System.Drawing.Point(16, 40)
        Me.Label_Sinusfunktion.Name = "Label_Sinusfunktion"
        Me.Label_Sinusfunktion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_Sinusfunktion.Size = New System.Drawing.Size(121, 17)
        Me.Label_Sinusfunktion.TabIndex = 4
        Me.Label_Sinusfunktion.Text = " Anzahl der Parameter"
        '
        'Problem_BealeProblem
        '
        Me.Problem_BealeProblem.BackColor = System.Drawing.SystemColors.Control
        Me.Problem_BealeProblem.Controls.Add(Me.Text_BealeProblem)
        Me.Problem_BealeProblem.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me.Problem_BealeProblem, CType(1, Short))
        Me.Problem_BealeProblem.Location = New System.Drawing.Point(0, 43)
        Me.Problem_BealeProblem.Name = "Problem_BealeProblem"
        Me.Problem_BealeProblem.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Problem_BealeProblem.Size = New System.Drawing.Size(217, 70)
        Me.Problem_BealeProblem.TabIndex = 5
        Me.Problem_BealeProblem.TabStop = False
        Me.Problem_BealeProblem.Text = "Beale-Problem"
        '
        'Text_BealeProblem
        '
        Me.Text_BealeProblem.AcceptsReturn = True
        Me.Text_BealeProblem.BackColor = System.Drawing.SystemColors.Control
        Me.Text_BealeProblem.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text_BealeProblem.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text_BealeProblem.ForeColor = System.Drawing.Color.Red
        Me.Text_BealeProblem.Location = New System.Drawing.Point(8, 16)
        Me.Text_BealeProblem.MaxLength = 0
        Me.Text_BealeProblem.Multiline = True
        Me.Text_BealeProblem.Name = "Text_BealeProblem"
        Me.Text_BealeProblem.ReadOnly = True
        Me.Text_BealeProblem.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text_BealeProblem.Size = New System.Drawing.Size(201, 49)
        Me.Text_BealeProblem.TabIndex = 6
        Me.Text_BealeProblem.Text = "Es wird das Minimum des Beale-Problems gesucht ( x = (3,  0.5),  F(x) = 0)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Problem_Schwefel24
        '
        Me.Problem_Schwefel24.BackColor = System.Drawing.SystemColors.Control
        Me.Problem_Schwefel24.Controls.Add(Me.Text_Schwefel24)
        Me.Problem_Schwefel24.Controls.Add(Me.Text_Schwefel24_Par)
        Me.Problem_Schwefel24.Controls.Add(Me.Label_Schwefel24)
        Me.Problem_Schwefel24.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me.Problem_Schwefel24, CType(2, Short))
        Me.Problem_Schwefel24.Location = New System.Drawing.Point(0, 43)
        Me.Problem_Schwefel24.Name = "Problem_Schwefel24"
        Me.Problem_Schwefel24.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Problem_Schwefel24.Size = New System.Drawing.Size(217, 70)
        Me.Problem_Schwefel24.TabIndex = 7
        Me.Problem_Schwefel24.TabStop = False
        Me.Problem_Schwefel24.Text = "Schwefel 2.4"
        '
        'Text_Schwefel24
        '
        Me.Text_Schwefel24.AcceptsReturn = True
        Me.Text_Schwefel24.BackColor = System.Drawing.SystemColors.Control
        Me.Text_Schwefel24.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text_Schwefel24.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text_Schwefel24.ForeColor = System.Drawing.Color.Red
        Me.Text_Schwefel24.Location = New System.Drawing.Point(8, 16)
        Me.Text_Schwefel24.MaxLength = 0
        Me.Text_Schwefel24.Multiline = True
        Me.Text_Schwefel24.Name = "Text_Schwefel24"
        Me.Text_Schwefel24.ReadOnly = True
        Me.Text_Schwefel24.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text_Schwefel24.Size = New System.Drawing.Size(185, 33)
        Me.Text_Schwefel24.TabIndex = 9
        Me.Text_Schwefel24.Text = "Minimum der Problemstellung wird gesucht (xi=1, F(x)=0)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Text_Schwefel24_Par
        '
        Me.Text_Schwefel24_Par.AcceptsReturn = True
        Me.Text_Schwefel24_Par.BackColor = System.Drawing.SystemColors.Window
        Me.Text_Schwefel24_Par.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text_Schwefel24_Par.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Text_Schwefel24_Par.Location = New System.Drawing.Point(136, 48)
        Me.Text_Schwefel24_Par.MaxLength = 0
        Me.Text_Schwefel24_Par.Name = "Text_Schwefel24_Par"
        Me.Text_Schwefel24_Par.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text_Schwefel24_Par.Size = New System.Drawing.Size(41, 20)
        Me.Text_Schwefel24_Par.TabIndex = 8
        Me.Text_Schwefel24_Par.Text = "5"
        '
        'Label_Schwefel24
        '
        Me.Label_Schwefel24.BackColor = System.Drawing.SystemColors.Control
        Me.Label_Schwefel24.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_Schwefel24.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_Schwefel24.Location = New System.Drawing.Point(16, 48)
        Me.Label_Schwefel24.Name = "Label_Schwefel24"
        Me.Label_Schwefel24.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_Schwefel24.Size = New System.Drawing.Size(121, 17)
        Me.Label_Schwefel24.TabIndex = 10
        Me.Label_Schwefel24.Text = " Anzahl der Parameter"
        '
        'Problem_D1Funktion
        '
        Me.Problem_D1Funktion.BackColor = System.Drawing.SystemColors.Control
        Me.Problem_D1Funktion.Controls.Add(Me.Text_D1Funktion)
        Me.Problem_D1Funktion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me.Problem_D1Funktion, CType(3, Short))
        Me.Problem_D1Funktion.Location = New System.Drawing.Point(0, 43)
        Me.Problem_D1Funktion.Name = "Problem_D1Funktion"
        Me.Problem_D1Funktion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Problem_D1Funktion.Size = New System.Drawing.Size(217, 70)
        Me.Problem_D1Funktion.TabIndex = 13
        Me.Problem_D1Funktion.TabStop = False
        Me.Problem_D1Funktion.Text = "D1-Funktion"
        '
        'Text_D1Funktion
        '
        Me.Text_D1Funktion.AcceptsReturn = True
        Me.Text_D1Funktion.BackColor = System.Drawing.SystemColors.Control
        Me.Text_D1Funktion.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text_D1Funktion.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text_D1Funktion.ForeColor = System.Drawing.Color.Red
        Me.Text_D1Funktion.Location = New System.Drawing.Point(8, 16)
        Me.Text_D1Funktion.MaxLength = 0
        Me.Text_D1Funktion.Multiline = True
        Me.Text_D1Funktion.Name = "Text_D1Funktion"
        Me.Text_D1Funktion.ReadOnly = True
        Me.Text_D1Funktion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text_D1Funktion.Size = New System.Drawing.Size(185, 49)
        Me.Text_D1Funktion.TabIndex = 14
        Me.Text_D1Funktion.Text = "Multikriterielles Testproblem (Konvex)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Problem_T1Funktion
        '
        Me.Problem_T1Funktion.BackColor = System.Drawing.SystemColors.Control
        Me.Problem_T1Funktion.Controls.Add(Me.Text_T1Funktion)
        Me.Problem_T1Funktion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me.Problem_T1Funktion, CType(4, Short))
        Me.Problem_T1Funktion.Location = New System.Drawing.Point(0, 43)
        Me.Problem_T1Funktion.Name = "Problem_T1Funktion"
        Me.Problem_T1Funktion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Problem_T1Funktion.Size = New System.Drawing.Size(217, 70)
        Me.Problem_T1Funktion.TabIndex = 15
        Me.Problem_T1Funktion.TabStop = False
        Me.Problem_T1Funktion.Text = "T1-Funktion"
        '
        'Text_T1Funktion
        '
        Me.Text_T1Funktion.AcceptsReturn = True
        Me.Text_T1Funktion.BackColor = System.Drawing.SystemColors.Control
        Me.Text_T1Funktion.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text_T1Funktion.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text_T1Funktion.ForeColor = System.Drawing.Color.Red
        Me.Text_T1Funktion.Location = New System.Drawing.Point(8, 16)
        Me.Text_T1Funktion.MaxLength = 0
        Me.Text_T1Funktion.Multiline = True
        Me.Text_T1Funktion.Name = "Text_T1Funktion"
        Me.Text_T1Funktion.ReadOnly = True
        Me.Text_T1Funktion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text_T1Funktion.Size = New System.Drawing.Size(177, 25)
        Me.Text_T1Funktion.TabIndex = 16
        Me.Text_T1Funktion.Text = "Multikriterielles Testproblem (Konvex)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Problem_T2Funktion
        '
        Me.Problem_T2Funktion.BackColor = System.Drawing.SystemColors.Control
        Me.Problem_T2Funktion.Controls.Add(Me.Text_T2Funktion)
        Me.Problem_T2Funktion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me.Problem_T2Funktion, CType(5, Short))
        Me.Problem_T2Funktion.Location = New System.Drawing.Point(0, 43)
        Me.Problem_T2Funktion.Name = "Problem_T2Funktion"
        Me.Problem_T2Funktion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Problem_T2Funktion.Size = New System.Drawing.Size(217, 70)
        Me.Problem_T2Funktion.TabIndex = 17
        Me.Problem_T2Funktion.TabStop = False
        Me.Problem_T2Funktion.Text = "T2-Funktion"
        '
        'Text_T2Funktion
        '
        Me.Text_T2Funktion.AcceptsReturn = True
        Me.Text_T2Funktion.BackColor = System.Drawing.SystemColors.Control
        Me.Text_T2Funktion.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text_T2Funktion.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text_T2Funktion.ForeColor = System.Drawing.Color.Red
        Me.Text_T2Funktion.Location = New System.Drawing.Point(8, 16)
        Me.Text_T2Funktion.MaxLength = 0
        Me.Text_T2Funktion.Multiline = True
        Me.Text_T2Funktion.Name = "Text_T2Funktion"
        Me.Text_T2Funktion.ReadOnly = True
        Me.Text_T2Funktion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text_T2Funktion.Size = New System.Drawing.Size(177, 25)
        Me.Text_T2Funktion.TabIndex = 18
        Me.Text_T2Funktion.Text = "Multikriterielles Testproblem (Konkav)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Problem_T3Funktion
        '
        Me.Problem_T3Funktion.BackColor = System.Drawing.SystemColors.Control
        Me.Problem_T3Funktion.Controls.Add(Me.Text_T3Funktion)
        Me.Problem_T3Funktion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame_Problem.SetIndex(Me.Problem_T3Funktion, CType(6, Short))
        Me.Problem_T3Funktion.Location = New System.Drawing.Point(0, 43)
        Me.Problem_T3Funktion.Name = "Problem_T3Funktion"
        Me.Problem_T3Funktion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Problem_T3Funktion.Size = New System.Drawing.Size(217, 70)
        Me.Problem_T3Funktion.TabIndex = 19
        Me.Problem_T3Funktion.TabStop = False
        Me.Problem_T3Funktion.Text = "T3-Funktion"
        '
        'Text_T3Funktion
        '
        Me.Text_T3Funktion.AcceptsReturn = True
        Me.Text_T3Funktion.BackColor = System.Drawing.SystemColors.Control
        Me.Text_T3Funktion.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Text_T3Funktion.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Text_T3Funktion.ForeColor = System.Drawing.Color.Red
        Me.Text_T3Funktion.Location = New System.Drawing.Point(8, 16)
        Me.Text_T3Funktion.MaxLength = 0
        Me.Text_T3Funktion.Multiline = True
        Me.Text_T3Funktion.Name = "Text_T3Funktion"
        Me.Text_T3Funktion.ReadOnly = True
        Me.Text_T3Funktion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text_T3Funktion.Size = New System.Drawing.Size(177, 45)
        Me.Text_T3Funktion.TabIndex = 20
        Me.Text_T3Funktion.Text = "Multikriterielles Testproblem (Konvex, " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "nicht stetig)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Button_Start
        '
        Me.Button_Start.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Start.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_Start.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_Start.Location = New System.Drawing.Point(584, 658)
        Me.Button_Start.Name = "Button_Start"
        Me.Button_Start.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Start.Size = New System.Drawing.Size(112, 33)
        Me.Button_Start.TabIndex = 0
        Me.Button_Start.Text = ">"
        Me.Button_Start.UseVisualStyleBackColor = False
        '
        'TeeCommander1
        '
        Me.TeeCommander1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
        Me.TeeCommander1.Chart = Me.TChart1
        Me.TeeCommander1.Divider = False
        Me.TeeCommander1.Dock = System.Windows.Forms.DockStyle.None
        Me.TeeCommander1.DropDownArrows = True
        Me.TeeCommander1.LabelValues = True
        Me.TeeCommander1.Location = New System.Drawing.Point(239, 657)
        Me.TeeCommander1.Name = "TeeCommander1"
        Me.TeeCommander1.ShowToolTips = True
        Me.TeeCommander1.Size = New System.Drawing.Size(344, 35)
        Me.TeeCommander1.TabIndex = 33
        '
        'GroupBox_Anwendung
        '
        Me.GroupBox_Anwendung.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox_Anwendung.Controls.Add(Me.Label_Anwendung)
        Me.GroupBox_Anwendung.Controls.Add(Me.ComboBox_Anwendung)
        Me.GroupBox_Anwendung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox_Anwendung.Location = New System.Drawing.Point(10, 12)
        Me.GroupBox_Anwendung.Name = "GroupBox_Anwendung"
        Me.GroupBox_Anwendung.Size = New System.Drawing.Size(217, 50)
        Me.GroupBox_Anwendung.TabIndex = 35
        Me.GroupBox_Anwendung.TabStop = False
        Me.GroupBox_Anwendung.Text = "Anwendung"
        '
        'Label_Anwendung
        '
        Me.Label_Anwendung.AutoSize = True
        Me.Label_Anwendung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label_Anwendung.Location = New System.Drawing.Point(6, 22)
        Me.Label_Anwendung.Name = "Label_Anwendung"
        Me.Label_Anwendung.Size = New System.Drawing.Size(67, 13)
        Me.Label_Anwendung.TabIndex = 3
        Me.Label_Anwendung.Text = "Anwendung:"
        '
        'ComboBox_Anwendung
        '
        Me.ComboBox_Anwendung.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Anwendung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.ComboBox_Anwendung.FormattingEnabled = True
        Me.ComboBox_Anwendung.Location = New System.Drawing.Point(89, 19)
        Me.ComboBox_Anwendung.Name = "ComboBox_Anwendung"
        Me.ComboBox_Anwendung.Size = New System.Drawing.Size(122, 21)
        Me.ComboBox_Anwendung.TabIndex = 2
        '
        'Form1
        '
        Me.AcceptButton = Me.Button_Start
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(712, 780)
        Me.Controls.Add(Me.GroupBox_Anwendung)
        Me.Controls.Add(Me.TeeCommander1)
        Me.Controls.Add(Me.TChart1)
        Me.Controls.Add(Me.EVO_Opt_Verlauf1)
        Me.Controls.Add(Me.EVO_Einstellungen1)
        Me.Controls.Add(Me.Button_Start)
        Me.Controls.Add(Me.GroupBox_Testproblem)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Location = New System.Drawing.Point(100, 100)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Evolutionsstrategie"
        Me.Problem_TKNFunktion.ResumeLayout(False)
        Me.Problem_TKNFunktion.PerformLayout()
        Me.Problem_CONSTRFunktion.ResumeLayout(False)
        Me.Problem_CONSTRFunktion.PerformLayout()
        Me.Problem_T4Funktion.ResumeLayout(False)
        Me.Problem_T4Funktion.PerformLayout()
        Me.GroupBox_Testproblem.ResumeLayout(False)
        Me.Problem_SinusFunktion.ResumeLayout(False)
        Me.Problem_SinusFunktion.PerformLayout()
        Me.Problem_BealeProblem.ResumeLayout(False)
        Me.Problem_BealeProblem.PerformLayout()
        Me.Problem_Schwefel24.ResumeLayout(False)
        Me.Problem_Schwefel24.PerformLayout()
        Me.Problem_D1Funktion.ResumeLayout(False)
        Me.Problem_D1Funktion.PerformLayout()
        Me.Problem_T1Funktion.ResumeLayout(False)
        Me.Problem_T1Funktion.PerformLayout()
        Me.Problem_T2Funktion.ResumeLayout(False)
        Me.Problem_T2Funktion.PerformLayout()
        Me.Problem_T3Funktion.ResumeLayout(False)
        Me.Problem_T3Funktion.PerformLayout()
        CType(Me.Frame_Problem, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_Anwendung.ResumeLayout(False)
        Me.GroupBox_Anwendung.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox_Anwendung As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBox_Anwendung As System.Windows.Forms.ComboBox
    Friend WithEvents Label_Anwendung As System.Windows.Forms.Label
    Friend WithEvents GroupBox_Testproblem As System.Windows.Forms.GroupBox
    Friend WithEvents Problem_SinusFunktion As System.Windows.Forms.GroupBox
    Friend WithEvents Problem_TKNFunktion As System.Windows.Forms.GroupBox
    Friend WithEvents Problem_CONSTRFunktion As System.Windows.Forms.GroupBox
    Friend WithEvents Problem_T4Funktion As System.Windows.Forms.GroupBox
    Friend WithEvents Problem_T3Funktion As System.Windows.Forms.GroupBox
    Friend WithEvents Problem_T2Funktion As System.Windows.Forms.GroupBox
    Friend WithEvents Problem_T1Funktion As System.Windows.Forms.GroupBox
    Friend WithEvents Problem_D1Funktion As System.Windows.Forms.GroupBox
    Friend WithEvents Problem_BealeProblem As System.Windows.Forms.GroupBox
    Friend WithEvents Problem_Schwefel24 As System.Windows.Forms.GroupBox
End Class
