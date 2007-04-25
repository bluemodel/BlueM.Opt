<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    Public Sub New()
        MyBase.New()
        'Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        IsInitializing = True
        InitializeComponent()
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
    Public WithEvents Diag As Main.Diagramm
    Public WithEvents EVO_Opt_Verlauf1 As EvoForm.EVO_Opt_Verlauf
    Public WithEvents EVO_Einstellungen1 As EvoForm.EVO_Einstellungen
    Public WithEvents Button_Start As System.Windows.Forms.Button
    'Hinweis: Die folgende Prozedur wird vom Windows Form-Designer benötigt.
    'Das Verändern mit dem Windows Form-Designer ist nicht möglich.
    'Das Verändern mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Button_Start = New System.Windows.Forms.Button
        Me.Button_IniApp = New System.Windows.Forms.Button
        Me.Diag = New Main.Diagramm
        Me.GroupBox_Anwendung = New System.Windows.Forms.GroupBox
        Me.Label_Anwendung = New System.Windows.Forms.Label
        Me.ComboBox_Anwendung = New System.Windows.Forms.ComboBox
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.EVO_Opt_Verlauf1 = New EvoForm.EVO_Opt_Verlauf
        Me.EVO_Einstellungen1 = New EvoForm.EVO_Einstellungen
        Me.Testprobleme1 = New Apps.Testprobleme
        Me.Button_TChart2Excel = New System.Windows.Forms.Button
        Me.Button_TChartSave = New System.Windows.Forms.Button
        Me.Button_TChart2PNG = New System.Windows.Forms.Button
        Me.Button_TChartEdit = New System.Windows.Forms.Button
        Me.GroupBox_TChartButtons = New System.Windows.Forms.GroupBox
        Me.GroupBox_Anwendung.SuspendLayout()
        Me.GroupBox_TChartButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_Start
        '
        Me.Button_Start.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Start.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_Start.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_Start.Location = New System.Drawing.Point(584, 652)
        Me.Button_Start.Name = "Button_Start"
        Me.Button_Start.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Start.Size = New System.Drawing.Size(112, 39)
        Me.Button_Start.TabIndex = 3
        Me.Button_Start.Text = ">"
        Me.ToolTip1.SetToolTip(Me.Button_Start, "Optimierung starten")
        Me.Button_Start.UseVisualStyleBackColor = True
        '
        'Button_IniApp
        '
        Me.Button_IniApp.Location = New System.Drawing.Point(176, 19)
        Me.Button_IniApp.Name = "Button_IniApp"
        Me.Button_IniApp.Size = New System.Drawing.Size(35, 21)
        Me.Button_IniApp.TabIndex = 1
        Me.Button_IniApp.Text = "Ini"
        Me.ToolTip1.SetToolTip(Me.Button_IniApp, "Anwendung initialisieren")
        Me.Button_IniApp.UseVisualStyleBackColor = True
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
        Me.Diag.Header.Lines = New String() {"TeeChart"}
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
        Me.Diag.Location = New System.Drawing.Point(233, 12)
        Me.Diag.Name = "Diag"
        Me.Diag.Size = New System.Drawing.Size(465, 625)
        Me.Diag.TabIndex = 5
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
        '
        'GroupBox_Anwendung
        '
        Me.GroupBox_Anwendung.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox_Anwendung.Controls.Add(Me.Button_IniApp)
        Me.GroupBox_Anwendung.Controls.Add(Me.Label_Anwendung)
        Me.GroupBox_Anwendung.Controls.Add(Me.ComboBox_Anwendung)
        Me.GroupBox_Anwendung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox_Anwendung.Location = New System.Drawing.Point(10, 12)
        Me.GroupBox_Anwendung.Name = "GroupBox_Anwendung"
        Me.GroupBox_Anwendung.Size = New System.Drawing.Size(217, 50)
        Me.GroupBox_Anwendung.TabIndex = 0
        Me.GroupBox_Anwendung.TabStop = False
        Me.GroupBox_Anwendung.Text = "Anwendung"
        '
        'Label_Anwendung
        '
        Me.Label_Anwendung.AutoSize = True
        Me.Label_Anwendung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label_Anwendung.Location = New System.Drawing.Point(6, 22)
        Me.Label_Anwendung.Name = "Label_Anwendung"
        Me.Label_Anwendung.Size = New System.Drawing.Size(34, 13)
        Me.Label_Anwendung.TabIndex = 2
        Me.Label_Anwendung.Text = "Apps:"
        '
        'ComboBox_Anwendung
        '
        Me.ComboBox_Anwendung.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Anwendung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.ComboBox_Anwendung.FormattingEnabled = True
        Me.ComboBox_Anwendung.Location = New System.Drawing.Point(46, 19)
        Me.ComboBox_Anwendung.Name = "ComboBox_Anwendung"
        Me.ComboBox_Anwendung.Size = New System.Drawing.Size(124, 21)
        Me.ComboBox_Anwendung.TabIndex = 0
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Title = "Datei speichern"
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
        Me.EVO_Opt_Verlauf1.TabIndex = 6
        '
        'EVO_Einstellungen1
        '
        Me.EVO_Einstellungen1.Location = New System.Drawing.Point(8, 187)
        Me.EVO_Einstellungen1.Name = "EVO_Einstellungen1"
        Me.EVO_Einstellungen1.OptModus = CType(0, Short)
        Me.EVO_Einstellungen1.Size = New System.Drawing.Size(225, 585)
        Me.EVO_Einstellungen1.TabIndex = 2
        '
        'Testprobleme1
        '
        Me.Testprobleme1.Location = New System.Drawing.Point(8, 68)
        Me.Testprobleme1.Name = "Testprobleme1"
        Me.Testprobleme1.Size = New System.Drawing.Size(225, 121)
        Me.Testprobleme1.TabIndex = 7
        '
        'Button_TChart2Excel
        '
        Me.Button_TChart2Excel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.Button_TChart2Excel.Image = Global.Main.My.Resources.Resources.icon_excel
        Me.Button_TChart2Excel.Location = New System.Drawing.Point(49, 19)
        Me.Button_TChart2Excel.Name = "Button_TChart2Excel"
        Me.Button_TChart2Excel.Size = New System.Drawing.Size(25, 25)
        Me.Button_TChart2Excel.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.Button_TChart2Excel, "als Excel-Datei speichern")
        Me.Button_TChart2Excel.UseVisualStyleBackColor = False
        '
        'Button_TChartSave
        '
        Me.Button_TChartSave.Image = Global.Main.My.Resources.Resources.icon_teechart
        Me.Button_TChartSave.Location = New System.Drawing.Point(80, 19)
        Me.Button_TChartSave.Name = "Button_TChartSave"
        Me.Button_TChartSave.Size = New System.Drawing.Size(25, 25)
        Me.Button_TChartSave.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.Button_TChartSave, "als natives TeeChart-Format speichern")
        Me.Button_TChartSave.UseVisualStyleBackColor = True
        '
        'Button_TChart2PNG
        '
        Me.Button_TChart2PNG.Image = Global.Main.My.Resources.Resources.icon_png
        Me.Button_TChart2PNG.Location = New System.Drawing.Point(111, 19)
        Me.Button_TChart2PNG.Name = "Button_TChart2PNG"
        Me.Button_TChart2PNG.Size = New System.Drawing.Size(25, 25)
        Me.Button_TChart2PNG.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.Button_TChart2PNG, "als PNG speichern")
        Me.Button_TChart2PNG.UseVisualStyleBackColor = True
        '
        'Button_TChartEdit
        '
        Me.Button_TChartEdit.Image = Global.Main.My.Resources.Resources.icon_edit
        Me.Button_TChartEdit.Location = New System.Drawing.Point(6, 19)
        Me.Button_TChartEdit.Name = "Button_TChartEdit"
        Me.Button_TChartEdit.Size = New System.Drawing.Size(25, 25)
        Me.Button_TChartEdit.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.Button_TChartEdit, "TChart editieren")
        Me.Button_TChartEdit.UseVisualStyleBackColor = True
        '
        'GroupBox_TChartButtons
        '
        Me.GroupBox_TChartButtons.Controls.Add(Me.Button_TChartEdit)
        Me.GroupBox_TChartButtons.Controls.Add(Me.Button_TChart2PNG)
        Me.GroupBox_TChartButtons.Controls.Add(Me.Button_TChartSave)
        Me.GroupBox_TChartButtons.Controls.Add(Me.Button_TChart2Excel)
        Me.GroupBox_TChartButtons.Location = New System.Drawing.Point(233, 643)
        Me.GroupBox_TChartButtons.Name = "GroupBox_TChartButtons"
        Me.GroupBox_TChartButtons.Size = New System.Drawing.Size(345, 49)
        Me.GroupBox_TChartButtons.TabIndex = 4
        Me.GroupBox_TChartButtons.TabStop = False
        Me.GroupBox_TChartButtons.Text = "TChart"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(712, 780)
        Me.Controls.Add(Me.Testprobleme1)
        Me.Controls.Add(Me.GroupBox_TChartButtons)
        Me.Controls.Add(Me.GroupBox_Anwendung)
        Me.Controls.Add(Me.Diag)
        Me.Controls.Add(Me.EVO_Opt_Verlauf1)
        Me.Controls.Add(Me.EVO_Einstellungen1)
        Me.Controls.Add(Me.Button_Start)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Location = New System.Drawing.Point(100, 100)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Evolutionsstrategie"
        Me.GroupBox_Anwendung.ResumeLayout(False)
        Me.GroupBox_Anwendung.PerformLayout()
        Me.GroupBox_TChartButtons.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox_Anwendung As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBox_Anwendung As System.Windows.Forms.ComboBox
    Friend WithEvents Label_Anwendung As System.Windows.Forms.Label
    Friend WithEvents Button_IniApp As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents Testprobleme1 As Apps.Testprobleme
    Friend WithEvents Button_TChart2Excel As System.Windows.Forms.Button
    Friend WithEvents Button_TChartSave As System.Windows.Forms.Button
    Friend WithEvents Button_TChart2PNG As System.Windows.Forms.Button
    Friend WithEvents Button_TChartEdit As System.Windows.Forms.Button
    Friend WithEvents GroupBox_TChartButtons As System.Windows.Forms.GroupBox
End Class
