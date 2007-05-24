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
        Me.GroupBox_Anwendung = New System.Windows.Forms.GroupBox
        Me.Label_Anwendung = New System.Windows.Forms.Label
        Me.ComboBox_Anwendung = New System.Windows.Forms.ComboBox
        Me.Testprobleme1 = New Apps.Testprobleme
        Me.EVO_Opt_Verlauf1 = New EvoForm.EVO_Opt_Verlauf
        Me.EVO_Einstellungen1 = New EvoForm.EVO_Einstellungen
        Me.DForm = New Main.DiagrammForm
        Me.GroupBox_Anwendung.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_Start
        '
        Me.Button_Start.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Start.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_Start.Enabled = False
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
        'Testprobleme1
        '
        Me.Testprobleme1.Location = New System.Drawing.Point(8, 68)
        Me.Testprobleme1.Name = "Testprobleme1"
        Me.Testprobleme1.Size = New System.Drawing.Size(225, 121)
        Me.Testprobleme1.TabIndex = 7
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
        'DForm
        '
        Me.DForm.Location = New System.Drawing.Point(233, 12)
        Me.DForm.Name = "DForm"
        Me.DForm.Size = New System.Drawing.Size(473, 625)
        Me.DForm.TabIndex = 8
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(712, 780)
        Me.Controls.Add(Me.DForm)
        Me.Controls.Add(Me.Testprobleme1)
        Me.Controls.Add(Me.Button_Start)
        Me.Controls.Add(Me.GroupBox_Anwendung)
        Me.Controls.Add(Me.EVO_Opt_Verlauf1)
        Me.Controls.Add(Me.EVO_Einstellungen1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Location = New System.Drawing.Point(100, 100)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Evolutionsstrategie"
        Me.GroupBox_Anwendung.ResumeLayout(False)
        Me.GroupBox_Anwendung.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox_Anwendung As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBox_Anwendung As System.Windows.Forms.ComboBox
    Friend WithEvents Label_Anwendung As System.Windows.Forms.Label
    Friend WithEvents Button_IniApp As System.Windows.Forms.Button
    Friend WithEvents Testprobleme1 As Apps.Testprobleme
    Friend WithEvents DForm As Main.DiagrammForm
End Class
