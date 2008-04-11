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
    Public WithEvents Button_Start As System.Windows.Forms.Button
    'Hinweis: Die folgende Prozedur wird vom Windows Form-Designer benötigt.
    'Das Verändern mit dem Windows Form-Designer ist nicht möglich.
    'Das Verändern mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Button_Start = New System.Windows.Forms.Button
        Me.LinkLabel_WorkDir = New System.Windows.Forms.LinkLabel
        Me.Button_openMDB = New System.Windows.Forms.Button
        Me.Button_Scatterplot = New System.Windows.Forms.Button
        Me.Button_saveMDB = New System.Windows.Forms.Button
        Me.Button_MCS = New System.Windows.Forms.Button
        Me.Button_genP = New System.Windows.Forms.Button
        Me.Button_loadRefResult = New System.Windows.Forms.Button
        Me.GroupBox_Anwendung = New System.Windows.Forms.GroupBox
        Me.Label_Anwendung = New System.Windows.Forms.Label
        Me.ComboBox_Anwendung = New System.Windows.Forms.ComboBox
        Me.Label_Methode = New System.Windows.Forms.Label
        Me.ComboBox_Methode = New System.Windows.Forms.ComboBox
        Me.Label_Datensatz = New System.Windows.Forms.Label
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.GroupBox_ErgebnisDB = New System.Windows.Forms.GroupBox
        Me.EVO_Einstellungen1 = New IHWB.EVO.EVO_Einstellungen
        Me.EVO_Opt_Verlauf1 = New IHWB.EVO.EVO_Opt_Verlauf
        Me.DForm = New IHWB.EVO.DiagrammForm
        Me.LabelZeitHeader = New System.Windows.Forms.Label
        Me.LabelZeit = New System.Windows.Forms.Label
        Me.Numeric_MCS_bis = New System.Windows.Forms.NumericUpDown
        Me.Numeric_MCS_von = New System.Windows.Forms.NumericUpDown
        Me.Label_Anzahl_MCS = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.GroupBox_Anwendung.SuspendLayout()
        Me.GroupBox_ErgebnisDB.SuspendLayout()
        CType(Me.Numeric_MCS_bis, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_MCS_von, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_Start
        '
        Me.Button_Start.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Start.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_Start.Enabled = False
        Me.Button_Start.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_Start.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_Start.Location = New System.Drawing.Point(594, 638)
        Me.Button_Start.Name = "Button_Start"
        Me.Button_Start.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Start.Size = New System.Drawing.Size(112, 40)
        Me.Button_Start.TabIndex = 3
        Me.Button_Start.Text = ">"
        Me.ToolTip1.SetToolTip(Me.Button_Start, "Optimierung starten")
        Me.Button_Start.UseVisualStyleBackColor = True
        '
        'LinkLabel_WorkDir
        '
        Me.LinkLabel_WorkDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel_WorkDir.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel_WorkDir.Location = New System.Drawing.Point(208, 22)
        Me.LinkLabel_WorkDir.MaximumSize = New System.Drawing.Size(5000, 13)
        Me.LinkLabel_WorkDir.Name = "LinkLabel_WorkDir"
        Me.LinkLabel_WorkDir.Size = New System.Drawing.Size(336, 13)
        Me.LinkLabel_WorkDir.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.LinkLabel_WorkDir, "Datensatz ändern")
        '
        'Button_openMDB
        '
        Me.Button_openMDB.Enabled = False
        Me.Button_openMDB.Image = Global.IHWB.EVO.My.Resources.Resources.database_connect
        Me.Button_openMDB.Location = New System.Drawing.Point(37, 16)
        Me.Button_openMDB.Name = "Button_openMDB"
        Me.Button_openMDB.Size = New System.Drawing.Size(25, 25)
        Me.Button_openMDB.TabIndex = 13
        Me.ToolTip1.SetToolTip(Me.Button_openMDB, "Optimierungsergebnis aus Datenbank laden")
        Me.Button_openMDB.UseVisualStyleBackColor = True
        '
        'Button_Scatterplot
        '
        Me.Button_Scatterplot.Enabled = False
        Me.Button_Scatterplot.Image = Global.IHWB.EVO.My.Resources.Resources.scatterplot
        Me.Button_Scatterplot.Location = New System.Drawing.Point(99, 16)
        Me.Button_Scatterplot.Name = "Button_Scatterplot"
        Me.Button_Scatterplot.Size = New System.Drawing.Size(25, 25)
        Me.Button_Scatterplot.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.Button_Scatterplot, "Scatterplot-Matrix anzeigen")
        Me.Button_Scatterplot.UseVisualStyleBackColor = True
        '
        'Button_saveMDB
        '
        Me.Button_saveMDB.Enabled = False
        Me.Button_saveMDB.Image = Global.IHWB.EVO.My.Resources.Resources.database_save
        Me.Button_saveMDB.Location = New System.Drawing.Point(6, 16)
        Me.Button_saveMDB.Name = "Button_saveMDB"
        Me.Button_saveMDB.Size = New System.Drawing.Size(25, 25)
        Me.Button_saveMDB.TabIndex = 14
        Me.ToolTip1.SetToolTip(Me.Button_saveMDB, "Ergebnisdatenbank speichern")
        Me.Button_saveMDB.UseVisualStyleBackColor = True
        '
        'Button_MCS
        '
        Me.Button_MCS.BackColor = System.Drawing.SystemColors.Control
        Me.Button_MCS.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_MCS.Enabled = False
        Me.Button_MCS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_MCS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_MCS.Location = New System.Drawing.Point(722, 648)
        Me.Button_MCS.Name = "Button_MCS"
        Me.Button_MCS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_MCS.Size = New System.Drawing.Size(82, 31)
        Me.Button_MCS.TabIndex = 17
        Me.Button_MCS.Text = "> MCS"
        Me.ToolTip1.SetToolTip(Me.Button_MCS, "Optimierung starten")
        Me.Button_MCS.UseVisualStyleBackColor = True
        '
        'Button_genP
        '
        Me.Button_genP.BackColor = System.Drawing.SystemColors.Control
        Me.Button_genP.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_genP.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_genP.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_genP.Location = New System.Drawing.Point(745, 590)
        Me.Button_genP.Name = "Button_genP"
        Me.Button_genP.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_genP.Size = New System.Drawing.Size(59, 23)
        Me.Button_genP.TabIndex = 22
        Me.Button_genP.Text = "gen. P"
        Me.ToolTip1.SetToolTip(Me.Button_genP, "Optimierung starten")
        Me.Button_genP.UseVisualStyleBackColor = True
        '
        'Button_loadRefResult
        '
        Me.Button_loadRefResult.Enabled = False
        Me.Button_loadRefResult.Image = Global.IHWB.EVO.My.Resources.Resources.database_go
        Me.Button_loadRefResult.Location = New System.Drawing.Point(68, 16)
        Me.Button_loadRefResult.Name = "Button_loadRefResult"
        Me.Button_loadRefResult.Size = New System.Drawing.Size(25, 25)
        Me.Button_loadRefResult.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.Button_loadRefResult, "Vergleichsergebnis aus Ergebnisdatenbank laden")
        Me.Button_loadRefResult.UseVisualStyleBackColor = True
        '
        'GroupBox_Anwendung
        '
        Me.GroupBox_Anwendung.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox_Anwendung.Controls.Add(Me.Label_Anwendung)
        Me.GroupBox_Anwendung.Controls.Add(Me.ComboBox_Anwendung)
        Me.GroupBox_Anwendung.Controls.Add(Me.Label_Methode)
        Me.GroupBox_Anwendung.Controls.Add(Me.ComboBox_Methode)
        Me.GroupBox_Anwendung.Controls.Add(Me.Label_Datensatz)
        Me.GroupBox_Anwendung.Controls.Add(Me.LinkLabel_WorkDir)
        Me.GroupBox_Anwendung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox_Anwendung.Location = New System.Drawing.Point(4, 4)
        Me.GroupBox_Anwendung.Name = "GroupBox_Anwendung"
        Me.GroupBox_Anwendung.Size = New System.Drawing.Size(710, 50)
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
        Me.Label_Anwendung.Size = New System.Drawing.Size(29, 13)
        Me.Label_Anwendung.TabIndex = 2
        Me.Label_Anwendung.Text = "App:"
        '
        'ComboBox_Anwendung
        '
        Me.ComboBox_Anwendung.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Anwendung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.ComboBox_Anwendung.FormattingEnabled = True
        Me.ComboBox_Anwendung.Location = New System.Drawing.Point(36, 19)
        Me.ComboBox_Anwendung.Name = "ComboBox_Anwendung"
        Me.ComboBox_Anwendung.Size = New System.Drawing.Size(124, 21)
        Me.ComboBox_Anwendung.TabIndex = 0
        '
        'Label_Methode
        '
        Me.Label_Methode.AutoSize = True
        Me.Label_Methode.Enabled = False
        Me.Label_Methode.Location = New System.Drawing.Point(546, 22)
        Me.Label_Methode.Name = "Label_Methode"
        Me.Label_Methode.Size = New System.Drawing.Size(37, 13)
        Me.Label_Methode.TabIndex = 11
        Me.Label_Methode.Text = "Meth.:"
        '
        'ComboBox_Methode
        '
        Me.ComboBox_Methode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Methode.FormattingEnabled = True
        Me.ComboBox_Methode.Location = New System.Drawing.Point(583, 19)
        Me.ComboBox_Methode.Name = "ComboBox_Methode"
        Me.ComboBox_Methode.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox_Methode.TabIndex = 10
        '
        'Label_Datensatz
        '
        Me.Label_Datensatz.AutoSize = True
        Me.Label_Datensatz.Enabled = False
        Me.Label_Datensatz.Location = New System.Drawing.Point(164, 22)
        Me.Label_Datensatz.Name = "Label_Datensatz"
        Me.Label_Datensatz.Size = New System.Drawing.Size(47, 13)
        Me.Label_Datensatz.TabIndex = 12
        Me.Label_Datensatz.Text = "Datens.:"
        '
        'GroupBox_ErgebnisDB
        '
        Me.GroupBox_ErgebnisDB.Controls.Add(Me.Button_saveMDB)
        Me.GroupBox_ErgebnisDB.Controls.Add(Me.Button_openMDB)
        Me.GroupBox_ErgebnisDB.Controls.Add(Me.Button_loadRefResult)
        Me.GroupBox_ErgebnisDB.Controls.Add(Me.Button_Scatterplot)
        Me.GroupBox_ErgebnisDB.Location = New System.Drawing.Point(431, 631)
        Me.GroupBox_ErgebnisDB.Name = "GroupBox_ErgebnisDB"
        Me.GroupBox_ErgebnisDB.Size = New System.Drawing.Size(131, 50)
        Me.GroupBox_ErgebnisDB.TabIndex = 14
        Me.GroupBox_ErgebnisDB.TabStop = False
        Me.GroupBox_ErgebnisDB.Text = "Ergebnis"
        '
        'EVO_Einstellungen1
        '
        Me.EVO_Einstellungen1.Enabled = False
        Me.EVO_Einstellungen1.Location = New System.Drawing.Point(4, 61)
        Me.EVO_Einstellungen1.Name = "EVO_Einstellungen1"
        Me.EVO_Einstellungen1.Size = New System.Drawing.Size(230, 700)
        Me.EVO_Einstellungen1.TabIndex = 2
        '
        'EVO_Opt_Verlauf1
        '
        Me.EVO_Opt_Verlauf1.Location = New System.Drawing.Point(241, 690)
        Me.EVO_Opt_Verlauf1.Name = "EVO_Opt_Verlauf1"
        Me.EVO_Opt_Verlauf1.Size = New System.Drawing.Size(471, 73)
        Me.EVO_Opt_Verlauf1.TabIndex = 6
        '
        'DForm
        '
        Me.DForm.Location = New System.Drawing.Point(241, 60)
        Me.DForm.Name = "DForm"
        Me.DForm.Size = New System.Drawing.Size(473, 625)
        Me.DForm.TabIndex = 8
        '
        'LabelZeitHeader
        '
        Me.LabelZeitHeader.AutoSize = True
        Me.LabelZeitHeader.Location = New System.Drawing.Point(719, 701)
        Me.LabelZeitHeader.Name = "LabelZeitHeader"
        Me.LabelZeitHeader.Size = New System.Drawing.Size(78, 13)
        Me.LabelZeitHeader.TabIndex = 15
        Me.LabelZeitHeader.Text = "Zeit [hh:mm:ss]"
        '
        'LabelZeit
        '
        Me.LabelZeit.AutoSize = True
        Me.LabelZeit.ForeColor = System.Drawing.SystemColors.MenuHighlight
        Me.LabelZeit.Location = New System.Drawing.Point(719, 731)
        Me.LabelZeit.Name = "LabelZeit"
        Me.LabelZeit.Size = New System.Drawing.Size(49, 13)
        Me.LabelZeit.TabIndex = 16
        Me.LabelZeit.Text = "00:00:00"
        '
        'Numeric_MCS_bis
        '
        Me.Numeric_MCS_bis.Location = New System.Drawing.Point(748, 553)
        Me.Numeric_MCS_bis.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.Numeric_MCS_bis.Name = "Numeric_MCS_bis"
        Me.Numeric_MCS_bis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Numeric_MCS_bis.Size = New System.Drawing.Size(56, 20)
        Me.Numeric_MCS_bis.TabIndex = 18
        Me.Numeric_MCS_bis.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_MCS_bis.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'Numeric_MCS_von
        '
        Me.Numeric_MCS_von.Location = New System.Drawing.Point(748, 527)
        Me.Numeric_MCS_von.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.Numeric_MCS_von.Name = "Numeric_MCS_von"
        Me.Numeric_MCS_von.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Numeric_MCS_von.Size = New System.Drawing.Size(56, 20)
        Me.Numeric_MCS_von.TabIndex = 19
        Me.Numeric_MCS_von.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_MCS_von.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label_Anzahl_MCS
        '
        Me.Label_Anzahl_MCS.AutoSize = True
        Me.Label_Anzahl_MCS.Location = New System.Drawing.Point(714, 529)
        Me.Label_Anzahl_MCS.Name = "Label_Anzahl_MCS"
        Me.Label_Anzahl_MCS.Size = New System.Drawing.Size(28, 13)
        Me.Label_Anzahl_MCS.TabIndex = 20
        Me.Label_Anzahl_MCS.Text = "von:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(714, 555)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(23, 13)
        Me.Label1.TabIndex = 21
        Me.Label1.Text = "bis:"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(819, 753)
        Me.Controls.Add(Me.Button_genP)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label_Anzahl_MCS)
        Me.Controls.Add(Me.Numeric_MCS_von)
        Me.Controls.Add(Me.Numeric_MCS_bis)
        Me.Controls.Add(Me.Button_MCS)
        Me.Controls.Add(Me.LabelZeit)
        Me.Controls.Add(Me.LabelZeitHeader)
        Me.Controls.Add(Me.GroupBox_ErgebnisDB)
        Me.Controls.Add(Me.Button_Start)
        Me.Controls.Add(Me.EVO_Einstellungen1)
        Me.Controls.Add(Me.GroupBox_Anwendung)
        Me.Controls.Add(Me.EVO_Opt_Verlauf1)
        Me.Controls.Add(Me.DForm)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(100, 100)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Evolutionsstrategie"
        Me.GroupBox_Anwendung.ResumeLayout(False)
        Me.GroupBox_Anwendung.PerformLayout()
        Me.GroupBox_ErgebnisDB.ResumeLayout(False)
        CType(Me.Numeric_MCS_bis, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_MCS_von, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DForm As EVO.DiagrammForm
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Private WithEvents EVO_Einstellungen1 As IHWB.EVO.EVO_Einstellungen
    Private WithEvents GroupBox_ErgebnisDB As System.Windows.Forms.GroupBox
    Private WithEvents Button_saveMDB As System.Windows.Forms.Button
    Private WithEvents Button_openMDB As System.Windows.Forms.Button
    Private WithEvents Button_Scatterplot As System.Windows.Forms.Button
    Private WithEvents LinkLabel_WorkDir As System.Windows.Forms.LinkLabel
    Private WithEvents Label_Datensatz As System.Windows.Forms.Label
    Private WithEvents GroupBox_Anwendung As System.Windows.Forms.GroupBox
    Private WithEvents ComboBox_Anwendung As System.Windows.Forms.ComboBox
    Private WithEvents Label_Anwendung As System.Windows.Forms.Label
    Private WithEvents ComboBox_Methode As System.Windows.Forms.ComboBox
    Private WithEvents Label_Methode As System.Windows.Forms.Label
    Private WithEvents LabelZeitHeader As System.Windows.Forms.Label
    Private WithEvents LabelZeit As System.Windows.Forms.Label
    Public WithEvents Button_MCS As System.Windows.Forms.Button
    Friend WithEvents Numeric_MCS_bis As System.Windows.Forms.NumericUpDown
    Friend WithEvents Numeric_MCS_von As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label_Anzahl_MCS As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Button_genP As System.Windows.Forms.Button
    Private WithEvents Button_loadRefResult As System.Windows.Forms.Button
End Class
