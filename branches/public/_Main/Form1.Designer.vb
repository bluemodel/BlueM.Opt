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
    Public WithEvents EVO_Opt_Verlauf1 As EVO_Opt_Verlauf
    Public WithEvents EVO_Settings1 As EVO_Einstellungen
    Public WithEvents Button_Start As System.Windows.Forms.Button
    'Hinweis: Die folgende Prozedur wird vom Windows Form-Designer benötigt.
    'Das Verändern mit dem Windows Form-Designer ist nicht möglich.
    'Das Verändern mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Button_Start = New System.Windows.Forms.Button
        Me.Button_IniMethod = New System.Windows.Forms.Button
        Me.LinkLabel_WorkDir = New System.Windows.Forms.LinkLabel
        Me.Button_Scatterplot = New System.Windows.Forms.Button
        Me.GroupBox_Anwendung = New System.Windows.Forms.GroupBox
        Me.Label_Methode = New System.Windows.Forms.Label
        Me.ComboBox_Methode = New System.Windows.Forms.ComboBox
        Me.Label_Anwendung = New System.Windows.Forms.Label
        Me.ComboBox_Anwendung = New System.Windows.Forms.ComboBox
        Me.EVO_Opt_Verlauf1 = New EVO.EVO_Opt_Verlauf
        Me.EVO_Settings1 = New EVO.EVO_Einstellungen
        Me.Testprobleme1 = New EVO.Testprobleme
        Me.OpenFileDialog_Datensatz = New System.Windows.Forms.OpenFileDialog
        Me.OpenFileDialog_MDB = New System.Windows.Forms.OpenFileDialog
        Me.DForm = New EVO.DiagrammForm
        Me.GroupBox_Anwendung.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_Start
        '
        Me.Button_Start.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Start.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_Start.Enabled = False
        Me.Button_Start.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_Start.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_Start.Location = New System.Drawing.Point(587, 648)
        Me.Button_Start.Name = "Button_Start"
        Me.Button_Start.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Start.Size = New System.Drawing.Size(112, 40)
        Me.Button_Start.TabIndex = 3
        Me.Button_Start.Text = ">"
        Me.ToolTip1.SetToolTip(Me.Button_Start, "Optimierung starten")
        Me.Button_Start.UseVisualStyleBackColor = True
        '
        'Button_IniMethod
        '
        Me.Button_IniMethod.Location = New System.Drawing.Point(357, 19)
        Me.Button_IniMethod.Name = "Button_IniMethod"
        Me.Button_IniMethod.Size = New System.Drawing.Size(35, 21)
        Me.Button_IniMethod.TabIndex = 1
        Me.Button_IniMethod.Text = "Ini"
        Me.ToolTip1.SetToolTip(Me.Button_IniMethod, "Initialisieren")
        Me.Button_IniMethod.UseVisualStyleBackColor = True
        '
        'LinkLabel_WorkDir
        '
        Me.LinkLabel_WorkDir.AutoSize = True
        Me.LinkLabel_WorkDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel_WorkDir.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel_WorkDir.Location = New System.Drawing.Point(406, 23)
        Me.LinkLabel_WorkDir.Name = "LinkLabel_WorkDir"
        Me.LinkLabel_WorkDir.Size = New System.Drawing.Size(0, 13)
        Me.LinkLabel_WorkDir.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.LinkLabel_WorkDir, "Datensatz ändern")
        '
        'Button_Scatterplot
        '
        Me.Button_Scatterplot.Enabled = False
        Me.Button_Scatterplot.Image = My.Resources.Resources.scatterplot
        Me.Button_Scatterplot.Location = New System.Drawing.Point(546, 656)
        Me.Button_Scatterplot.Name = "Button_Scatterplot"
        Me.Button_Scatterplot.Size = New System.Drawing.Size(25, 25)
        Me.Button_Scatterplot.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.Button_Scatterplot, "Scatterplot-Matrix anzeigen")
        Me.Button_Scatterplot.UseVisualStyleBackColor = True
        '
        'GroupBox_Anwendung
        '
        Me.GroupBox_Anwendung.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox_Anwendung.Controls.Add(Me.Label_Methode)
        Me.GroupBox_Anwendung.Controls.Add(Me.ComboBox_Methode)
        Me.GroupBox_Anwendung.Controls.Add(Me.Label_Anwendung)
        Me.GroupBox_Anwendung.Controls.Add(Me.ComboBox_Anwendung)
        Me.GroupBox_Anwendung.Controls.Add(Me.Button_IniMethod)
        Me.GroupBox_Anwendung.Controls.Add(Me.LinkLabel_WorkDir)
        Me.GroupBox_Anwendung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox_Anwendung.Location = New System.Drawing.Point(8, 12)
        Me.GroupBox_Anwendung.Name = "GroupBox_Anwendung"
        Me.GroupBox_Anwendung.Size = New System.Drawing.Size(698, 50)
        Me.GroupBox_Anwendung.TabIndex = 0
        Me.GroupBox_Anwendung.TabStop = False
        Me.GroupBox_Anwendung.Text = "Anwendung"
        '
        'Label_Methode
        '
        Me.Label_Methode.AutoSize = True
        Me.Label_Methode.Location = New System.Drawing.Point(172, 22)
        Me.Label_Methode.Name = "Label_Methode"
        Me.Label_Methode.Size = New System.Drawing.Size(52, 13)
        Me.Label_Methode.TabIndex = 11
        Me.Label_Methode.Text = "Methode:"
        '
        'ComboBox_Methode
        '
        Me.ComboBox_Methode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Methode.FormattingEnabled = True
        Me.ComboBox_Methode.Location = New System.Drawing.Point(230, 19)
        Me.ComboBox_Methode.Name = "ComboBox_Methode"
        Me.ComboBox_Methode.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox_Methode.TabIndex = 10
        '
        'Label_Anwendung
        '
        Me.Label_Anwendung.AutoSize = True
        Me.Label_Anwendung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label_Anwendung.Location = New System.Drawing.Point(6, 22)
        Me.Label_Anwendung.Name = "Label_Anwendung"
        Me.Label_Anwendung.Size = New System.Drawing.Size(29, 13)
        Me.Label_Anwendung.TabIndex = 2
        Me.Label_Anwendung.Text = "App:"
        '
        'ComboBox_Anwendung
        '
        Me.ComboBox_Anwendung.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Anwendung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.ComboBox_Anwendung.FormattingEnabled = True
        Me.ComboBox_Anwendung.Location = New System.Drawing.Point(42, 19)
        Me.ComboBox_Anwendung.Name = "ComboBox_Anwendung"
        Me.ComboBox_Anwendung.Size = New System.Drawing.Size(124, 21)
        Me.ComboBox_Anwendung.TabIndex = 0
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
        Me.EVO_Settings1.Enabled = False
        Me.EVO_Settings1.Location = New System.Drawing.Point(8, 187)
        Me.EVO_Settings1.Name = "EVO_Einstellungen1"
        Me.EVO_Settings1.OptModus = CType(0, Short)
        Me.EVO_Settings1.Size = New System.Drawing.Size(225, 585)
        Me.EVO_Settings1.TabIndex = 2
        '
        'Testprobleme1
        '
        Me.Testprobleme1.Enabled = False
        Me.Testprobleme1.Location = New System.Drawing.Point(5, 68)
        Me.Testprobleme1.Name = "Testprobleme1"
        Me.Testprobleme1.Size = New System.Drawing.Size(225, 121)
        Me.Testprobleme1.TabIndex = 7
        '
        'OpenFileDialog_Datensatz
        '
        Me.OpenFileDialog_Datensatz.DefaultExt = "ALL"
        Me.OpenFileDialog_Datensatz.Filter = "ALL-Dateien (*.ALL)|*.ALL"
        Me.OpenFileDialog_Datensatz.Title = "Datensatz auswählen"
        '
        'OpenFileDialog_MDB
        '
        Me.OpenFileDialog_MDB.Filter = "Access-Datenbanken|*.mdb"
        Me.OpenFileDialog_MDB.Title = "Ergebnisdatenbank auswählen"
        '
        'DForm
        '
        Me.DForm.Location = New System.Drawing.Point(233, 68)
        Me.DForm.Name = "DForm"
        Me.DForm.Size = New System.Drawing.Size(473, 625)
        Me.DForm.TabIndex = 8
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(712, 780)
        Me.Controls.Add(Me.Button_Scatterplot)
        Me.Controls.Add(Me.Button_Start)
        Me.Controls.Add(Me.Testprobleme1)
        Me.Controls.Add(Me.GroupBox_Anwendung)
        Me.Controls.Add(Me.EVO_Opt_Verlauf1)
        Me.Controls.Add(Me.EVO_Settings1)
        Me.Controls.Add(Me.DForm)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
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
    Friend WithEvents Button_IniMethod As System.Windows.Forms.Button
    Friend WithEvents Testprobleme1 As Testprobleme
    Friend WithEvents DForm As EVO.DiagrammForm
    Friend WithEvents LinkLabel_WorkDir As System.Windows.Forms.LinkLabel
    Friend WithEvents ComboBox_Methode As System.Windows.Forms.ComboBox
    Friend WithEvents Label_Methode As System.Windows.Forms.Label
    Friend WithEvents OpenFileDialog_Datensatz As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Button_Scatterplot As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog_MDB As System.Windows.Forms.OpenFileDialog
End Class
