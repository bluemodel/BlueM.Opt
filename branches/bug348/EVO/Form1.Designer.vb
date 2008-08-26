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
        Dim MenuStrip1 As System.Windows.Forms.MenuStrip
        Dim MenuItem_Tools As System.Windows.Forms.ToolStripMenuItem
        Dim ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.MenuItem_DatensatzZurücksetzen = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItem_Optionen = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItem_Hilfe = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItem_Wiki = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItem_About = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Button_Start = New System.Windows.Forms.Button
        Me.Button_openMDB = New System.Windows.Forms.Button
        Me.Button_Scatterplot = New System.Windows.Forms.Button
        Me.Button_saveMDB = New System.Windows.Forms.Button
        Me.Button_loadRefResult = New System.Windows.Forms.Button
        Me.Button_TChartEdit = New System.Windows.Forms.Button
        Me.Button_TChart2PNG = New System.Windows.Forms.Button
        Me.Button_TChartSave = New System.Windows.Forms.Button
        Me.Button_TChart2Excel = New System.Windows.Forms.Button
        Me.Indicatordiagramm1 = New IHWB.EVO.Diagramm.Indicatordiagramm
        Me.GroupBox_Anwendung = New System.Windows.Forms.GroupBox
        Me.Button_BrowseDatensatz = New System.Windows.Forms.Button
        Me.ComboBox_Datensatz = New System.Windows.Forms.ComboBox
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
        Me.GroupBox_TChartButtons = New System.Windows.Forms.GroupBox
        Me.Hauptdiagramm1 = New IHWB.EVO.Diagramm.Hauptdiagramm
        Me.Info = New System.Windows.Forms.GroupBox
        Me.Label_Dn_Wert = New System.Windows.Forms.Label
        Me.Label_Dn = New System.Windows.Forms.Label
        MenuStrip1 = New System.Windows.Forms.MenuStrip
        MenuItem_Tools = New System.Windows.Forms.ToolStripMenuItem
        ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        MenuStrip1.SuspendLayout()
        Me.GroupBox_Anwendung.SuspendLayout()
        Me.GroupBox_ErgebnisDB.SuspendLayout()
        Me.GroupBox_TChartButtons.SuspendLayout()
        Me.Info.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {MenuItem_Tools, Me.MenuItem_Hilfe})
        MenuStrip1.Location = New System.Drawing.Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No
        MenuStrip1.Size = New System.Drawing.Size(718, 24)
        MenuStrip1.TabIndex = 15
        MenuStrip1.Text = "MenuStrip1"
        '
        'MenuItem_Tools
        '
        MenuItem_Tools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem_DatensatzZurücksetzen, ToolStripSeparator1, Me.MenuItem_Optionen})
        MenuItem_Tools.Name = "MenuItem_Tools"
        MenuItem_Tools.Size = New System.Drawing.Size(44, 20)
        MenuItem_Tools.Text = "Tools"
        '
        'MenuItem_DatensatzZurücksetzen
        '
        Me.MenuItem_DatensatzZurücksetzen.Enabled = False
        Me.MenuItem_DatensatzZurücksetzen.Name = "MenuItem_DatensatzZurücksetzen"
        Me.MenuItem_DatensatzZurücksetzen.Size = New System.Drawing.Size(200, 22)
        Me.MenuItem_DatensatzZurücksetzen.Text = "Datensatz zurücksetzen"
        '
        'ToolStripSeparator1
        '
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New System.Drawing.Size(197, 6)
        '
        'MenuItem_Optionen
        '
        Me.MenuItem_Optionen.Name = "MenuItem_Optionen"
        Me.MenuItem_Optionen.Size = New System.Drawing.Size(200, 22)
        Me.MenuItem_Optionen.Text = "Optionen..."
        '
        'MenuItem_Hilfe
        '
        Me.MenuItem_Hilfe.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem_Wiki, ToolStripSeparator2, Me.MenuItem_About})
        Me.MenuItem_Hilfe.Name = "MenuItem_Hilfe"
        Me.MenuItem_Hilfe.Size = New System.Drawing.Size(40, 20)
        Me.MenuItem_Hilfe.Text = "Hilfe"
        '
        'MenuItem_Wiki
        '
        Me.MenuItem_Wiki.Name = "MenuItem_Wiki"
        Me.MenuItem_Wiki.Size = New System.Drawing.Size(172, 22)
        Me.MenuItem_Wiki.Text = "Wiki"
        '
        'ToolStripSeparator2
        '
        ToolStripSeparator2.Name = "ToolStripSeparator2"
        ToolStripSeparator2.Size = New System.Drawing.Size(169, 6)
        '
        'MenuItem_About
        '
        Me.MenuItem_About.Name = "MenuItem_About"
        Me.MenuItem_About.Size = New System.Drawing.Size(172, 22)
        Me.MenuItem_About.Text = "About EVO.NET..."
        '
        'Button_Start
        '
        Me.Button_Start.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Start.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_Start.Enabled = False
        Me.Button_Start.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_Start.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_Start.Location = New System.Drawing.Point(594, 662)
        Me.Button_Start.Name = "Button_Start"
        Me.Button_Start.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button_Start.Size = New System.Drawing.Size(112, 40)
        Me.Button_Start.TabIndex = 3
        Me.Button_Start.Text = "Run"
        Me.ToolTip1.SetToolTip(Me.Button_Start, "Optimierung starten")
        Me.Button_Start.UseVisualStyleBackColor = True
        '
        'Button_openMDB
        '
        Me.Button_openMDB.Enabled = False
        Me.Button_openMDB.Image = Global.IHWB.EVO.My.Resources.Resources.database_connect
        Me.Button_openMDB.Location = New System.Drawing.Point(37, 19)
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
        Me.Button_Scatterplot.Location = New System.Drawing.Point(99, 19)
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
        Me.Button_saveMDB.Location = New System.Drawing.Point(6, 19)
        Me.Button_saveMDB.Name = "Button_saveMDB"
        Me.Button_saveMDB.Size = New System.Drawing.Size(25, 25)
        Me.Button_saveMDB.TabIndex = 14
        Me.ToolTip1.SetToolTip(Me.Button_saveMDB, "Ergebnisdatenbank speichern")
        Me.Button_saveMDB.UseVisualStyleBackColor = True
        '
        'Button_loadRefResult
        '
        Me.Button_loadRefResult.Enabled = False
        Me.Button_loadRefResult.Image = Global.IHWB.EVO.My.Resources.Resources.database_go
        Me.Button_loadRefResult.Location = New System.Drawing.Point(68, 19)
        Me.Button_loadRefResult.Name = "Button_loadRefResult"
        Me.Button_loadRefResult.Size = New System.Drawing.Size(25, 25)
        Me.Button_loadRefResult.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.Button_loadRefResult, "Vergleichsergebnis aus Ergebnisdatenbank laden")
        Me.Button_loadRefResult.UseVisualStyleBackColor = True
        '
        'Button_TChartEdit
        '
        Me.Button_TChartEdit.Image = Global.IHWB.EVO.My.Resources.Resources.chart_curve_edit
        Me.Button_TChartEdit.Location = New System.Drawing.Point(6, 19)
        Me.Button_TChartEdit.Name = "Button_TChartEdit"
        Me.Button_TChartEdit.Size = New System.Drawing.Size(25, 25)
        Me.Button_TChartEdit.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.Button_TChartEdit, "Diagramm bearbeiten")
        '
        'Button_TChart2PNG
        '
        Me.Button_TChart2PNG.Image = Global.IHWB.EVO.My.Resources.Resources.icon_png
        Me.Button_TChart2PNG.Location = New System.Drawing.Point(111, 19)
        Me.Button_TChart2PNG.Name = "Button_TChart2PNG"
        Me.Button_TChart2PNG.Size = New System.Drawing.Size(25, 25)
        Me.Button_TChart2PNG.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.Button_TChart2PNG, "als PNG exportieren")
        Me.Button_TChart2PNG.UseVisualStyleBackColor = True
        '
        'Button_TChartSave
        '
        Me.Button_TChartSave.Image = Global.IHWB.EVO.My.Resources.Resources.icon_teechart
        Me.Button_TChartSave.Location = New System.Drawing.Point(80, 19)
        Me.Button_TChartSave.Name = "Button_TChartSave"
        Me.Button_TChartSave.Size = New System.Drawing.Size(25, 25)
        Me.Button_TChartSave.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.Button_TChartSave, "als natives TeeChart-Format exportieren")
        Me.Button_TChartSave.UseVisualStyleBackColor = True
        '
        'Button_TChart2Excel
        '
        Me.Button_TChart2Excel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.Button_TChart2Excel.Image = Global.IHWB.EVO.My.Resources.Resources.icon_excel
        Me.Button_TChart2Excel.Location = New System.Drawing.Point(49, 19)
        Me.Button_TChart2Excel.Name = "Button_TChart2Excel"
        Me.Button_TChart2Excel.Size = New System.Drawing.Size(25, 25)
        Me.Button_TChart2Excel.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.Button_TChart2Excel, "nach Excel exportieren")
        Me.Button_TChart2Excel.UseVisualStyleBackColor = False
        '
        'Indicatordiagramm1
        '
        '
        '
        '
        Me.Indicatordiagramm1.Aspect.View3D = False
        Me.Indicatordiagramm1.Aspect.ZOffset = 0
        '
        '
        '
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Indicatordiagramm1.Axes.Bottom.Labels.Font.Size = 6
        Me.Indicatordiagramm1.Axes.Bottom.Labels.Font.SizeFloat = 6.0!
        Me.Indicatordiagramm1.Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
        Me.Indicatordiagramm1.Axes.Bottom.MaximumOffset = 3
        Me.Indicatordiagramm1.Axes.Bottom.MinimumOffset = 3
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Indicatordiagramm1.Axes.Left.Labels.Font.Size = 6
        Me.Indicatordiagramm1.Axes.Left.Labels.Font.SizeFloat = 6.0!
        Me.Indicatordiagramm1.Axes.Left.MaximumOffset = 3
        Me.Indicatordiagramm1.Axes.Left.MinimumOffset = 3
        Me.Indicatordiagramm1.Cursor = System.Windows.Forms.Cursors.Default
        '
        '
        '
        Me.Indicatordiagramm1.Header.Visible = False
        '
        '
        '
        Me.Indicatordiagramm1.Legend.Visible = False
        Me.Indicatordiagramm1.Location = New System.Drawing.Point(241, 582)
        Me.Indicatordiagramm1.Name = "Indicatordiagramm1"
        '
        '
        '
        Me.Indicatordiagramm1.Panel.MarginTop = 20
        '
        '
        '
        Me.Indicatordiagramm1.Panning.Allow = Steema.TeeChart.ScrollModes.None
        Me.Indicatordiagramm1.Size = New System.Drawing.Size(473, 70)
        Me.Indicatordiagramm1.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.Indicatordiagramm1, "Hypervolumen")
        Me.Indicatordiagramm1.Visible = False
        '
        'GroupBox_Anwendung
        '
        Me.GroupBox_Anwendung.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox_Anwendung.Controls.Add(Me.Button_BrowseDatensatz)
        Me.GroupBox_Anwendung.Controls.Add(Me.ComboBox_Datensatz)
        Me.GroupBox_Anwendung.Controls.Add(Me.Label_Anwendung)
        Me.GroupBox_Anwendung.Controls.Add(Me.ComboBox_Anwendung)
        Me.GroupBox_Anwendung.Controls.Add(Me.Label_Methode)
        Me.GroupBox_Anwendung.Controls.Add(Me.ComboBox_Methode)
        Me.GroupBox_Anwendung.Controls.Add(Me.Label_Datensatz)
        Me.GroupBox_Anwendung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox_Anwendung.Location = New System.Drawing.Point(4, 28)
        Me.GroupBox_Anwendung.Name = "GroupBox_Anwendung"
        Me.GroupBox_Anwendung.Size = New System.Drawing.Size(710, 50)
        Me.GroupBox_Anwendung.TabIndex = 0
        Me.GroupBox_Anwendung.TabStop = False
        Me.GroupBox_Anwendung.Text = "Anwendung"
        '
        'Button_BrowseDatensatz
        '
        Me.Button_BrowseDatensatz.Enabled = False
        Me.Button_BrowseDatensatz.Location = New System.Drawing.Point(516, 17)
        Me.Button_BrowseDatensatz.Name = "Button_BrowseDatensatz"
        Me.Button_BrowseDatensatz.Size = New System.Drawing.Size(24, 23)
        Me.Button_BrowseDatensatz.TabIndex = 14
        Me.Button_BrowseDatensatz.Text = "..."
        Me.Button_BrowseDatensatz.UseVisualStyleBackColor = True
        '
        'ComboBox_Datensatz
        '
        Me.ComboBox_Datensatz.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Datensatz.Enabled = False
        Me.ComboBox_Datensatz.FormattingEnabled = True
        Me.ComboBox_Datensatz.Location = New System.Drawing.Point(206, 18)
        Me.ComboBox_Datensatz.Name = "ComboBox_Datensatz"
        Me.ComboBox_Datensatz.Size = New System.Drawing.Size(304, 21)
        Me.ComboBox_Datensatz.TabIndex = 13
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
        Me.ComboBox_Anwendung.Location = New System.Drawing.Point(36, 18)
        Me.ComboBox_Anwendung.Name = "ComboBox_Anwendung"
        Me.ComboBox_Anwendung.Size = New System.Drawing.Size(100, 21)
        Me.ComboBox_Anwendung.TabIndex = 0
        '
        'Label_Methode
        '
        Me.Label_Methode.AutoSize = True
        Me.Label_Methode.Enabled = False
        Me.Label_Methode.Location = New System.Drawing.Point(546, 22)
        Me.Label_Methode.Name = "Label_Methode"
        Me.Label_Methode.Size = New System.Drawing.Size(52, 13)
        Me.Label_Methode.TabIndex = 11
        Me.Label_Methode.Text = "Methode:"
        '
        'ComboBox_Methode
        '
        Me.ComboBox_Methode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Methode.Enabled = False
        Me.ComboBox_Methode.FormattingEnabled = True
        Me.ComboBox_Methode.Location = New System.Drawing.Point(604, 18)
        Me.ComboBox_Methode.Name = "ComboBox_Methode"
        Me.ComboBox_Methode.Size = New System.Drawing.Size(100, 21)
        Me.ComboBox_Methode.TabIndex = 10
        '
        'Label_Datensatz
        '
        Me.Label_Datensatz.AutoSize = True
        Me.Label_Datensatz.Enabled = False
        Me.Label_Datensatz.Location = New System.Drawing.Point(142, 22)
        Me.Label_Datensatz.Name = "Label_Datensatz"
        Me.Label_Datensatz.Size = New System.Drawing.Size(58, 13)
        Me.Label_Datensatz.TabIndex = 12
        Me.Label_Datensatz.Text = "Datensatz:"
        '
        'GroupBox_ErgebnisDB
        '
        Me.GroupBox_ErgebnisDB.Controls.Add(Me.Button_saveMDB)
        Me.GroupBox_ErgebnisDB.Controls.Add(Me.Button_openMDB)
        Me.GroupBox_ErgebnisDB.Controls.Add(Me.Button_loadRefResult)
        Me.GroupBox_ErgebnisDB.Controls.Add(Me.Button_Scatterplot)
        Me.GroupBox_ErgebnisDB.Location = New System.Drawing.Point(390, 658)
        Me.GroupBox_ErgebnisDB.Name = "GroupBox_ErgebnisDB"
        Me.GroupBox_ErgebnisDB.Size = New System.Drawing.Size(131, 50)
        Me.GroupBox_ErgebnisDB.TabIndex = 14
        Me.GroupBox_ErgebnisDB.TabStop = False
        Me.GroupBox_ErgebnisDB.Text = "Ergebnis"
        '
        'EVO_Einstellungen1
        '
        Me.EVO_Einstellungen1.Enabled = False
        Me.EVO_Einstellungen1.Location = New System.Drawing.Point(4, 85)
        Me.EVO_Einstellungen1.Name = "EVO_Einstellungen1"
        Me.EVO_Einstellungen1.Size = New System.Drawing.Size(230, 700)
        Me.EVO_Einstellungen1.TabIndex = 2
        '
        'EVO_Opt_Verlauf1
        '
        Me.EVO_Opt_Verlauf1.Location = New System.Drawing.Point(241, 714)
        Me.EVO_Opt_Verlauf1.Name = "EVO_Opt_Verlauf1"
        Me.EVO_Opt_Verlauf1.Size = New System.Drawing.Size(467, 73)
        Me.EVO_Opt_Verlauf1.TabIndex = 6
        '
        'GroupBox_TChartButtons
        '
        Me.GroupBox_TChartButtons.Controls.Add(Me.Button_TChartEdit)
        Me.GroupBox_TChartButtons.Controls.Add(Me.Button_TChart2PNG)
        Me.GroupBox_TChartButtons.Controls.Add(Me.Button_TChartSave)
        Me.GroupBox_TChartButtons.Controls.Add(Me.Button_TChart2Excel)
        Me.GroupBox_TChartButtons.Location = New System.Drawing.Point(241, 658)
        Me.GroupBox_TChartButtons.Name = "GroupBox_TChartButtons"
        Me.GroupBox_TChartButtons.Size = New System.Drawing.Size(143, 50)
        Me.GroupBox_TChartButtons.TabIndex = 16
        Me.GroupBox_TChartButtons.TabStop = False
        Me.GroupBox_TChartButtons.Text = "Diagramm"
        '
        'Hauptdiagramm1
        '
        '
        '
        '
        Me.Hauptdiagramm1.Aspect.View3D = False
        Me.Hauptdiagramm1.Aspect.ZOffset = 0
        Me.Hauptdiagramm1.Cursor = System.Windows.Forms.Cursors.Default
        '
        '
        '
        Me.Hauptdiagramm1.Header.Lines = New String() {"EVO.NET"}
        Me.Hauptdiagramm1.Location = New System.Drawing.Point(241, 86)
        Me.Hauptdiagramm1.Name = "Hauptdiagramm1"
        Me.Hauptdiagramm1.Size = New System.Drawing.Size(473, 566)
        Me.Hauptdiagramm1.TabIndex = 17
        '
        '
        '
        Me.Hauptdiagramm1.Walls.View3D = False
        '
        'Info
        '
        Me.Info.Controls.Add(Me.Label_Dn_Wert)
        Me.Info.Controls.Add(Me.Label_Dn)
        Me.Info.Location = New System.Drawing.Point(527, 658)
        Me.Info.Name = "Info"
        Me.Info.Size = New System.Drawing.Size(59, 50)
        Me.Info.TabIndex = 18
        Me.Info.TabStop = False
        Me.Info.Text = "Info"
        '
        'Label_Dn_Wert
        '
        Me.Label_Dn_Wert.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label_Dn_Wert.AutoSize = True
        Me.Label_Dn_Wert.ForeColor = System.Drawing.Color.Blue
        Me.Label_Dn_Wert.Location = New System.Drawing.Point(3, 31)
        Me.Label_Dn_Wert.Name = "Label_Dn_Wert"
        Me.Label_Dn_Wert.Size = New System.Drawing.Size(0, 13)
        Me.Label_Dn_Wert.TabIndex = 1
        '
        'Label_Dn
        '
        Me.Label_Dn.AutoSize = True
        Me.Label_Dn.Location = New System.Drawing.Point(3, 13)
        Me.Label_Dn.Name = "Label_Dn"
        Me.Label_Dn.Size = New System.Drawing.Size(24, 13)
        Me.Label_Dn.TabIndex = 0
        Me.Label_Dn.Text = "Dn:"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(718, 792)
        Me.Controls.Add(Me.Info)
        Me.Controls.Add(Me.Indicatordiagramm1)
        Me.Controls.Add(Me.Hauptdiagramm1)
        Me.Controls.Add(Me.GroupBox_TChartButtons)
        Me.Controls.Add(MenuStrip1)
        Me.Controls.Add(Me.GroupBox_ErgebnisDB)
        Me.Controls.Add(Me.Button_Start)
        Me.Controls.Add(Me.EVO_Einstellungen1)
        Me.Controls.Add(Me.GroupBox_Anwendung)
        Me.Controls.Add(Me.EVO_Opt_Verlauf1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(100, 100)
        Me.MainMenuStrip = MenuStrip1
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "EVO.NET"
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        Me.GroupBox_Anwendung.ResumeLayout(False)
        Me.GroupBox_Anwendung.PerformLayout()
        Me.GroupBox_ErgebnisDB.ResumeLayout(False)
        Me.GroupBox_TChartButtons.ResumeLayout(False)
        Me.Info.ResumeLayout(False)
        Me.Info.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Private WithEvents EVO_Einstellungen1 As IHWB.EVO.EVO_Einstellungen
    Private WithEvents GroupBox_ErgebnisDB As System.Windows.Forms.GroupBox
    Private WithEvents Button_saveMDB As System.Windows.Forms.Button
    Private WithEvents Button_openMDB As System.Windows.Forms.Button
    Private WithEvents Button_Scatterplot As System.Windows.Forms.Button
    Private WithEvents Label_Datensatz As System.Windows.Forms.Label
    Private WithEvents GroupBox_Anwendung As System.Windows.Forms.GroupBox
    Private WithEvents ComboBox_Anwendung As System.Windows.Forms.ComboBox
    Private WithEvents Label_Anwendung As System.Windows.Forms.Label
    Private WithEvents ComboBox_Methode As System.Windows.Forms.ComboBox
    Private WithEvents Label_Methode As System.Windows.Forms.Label
    Private WithEvents Button_loadRefResult As System.Windows.Forms.Button
    Friend WithEvents Button_BrowseDatensatz As System.Windows.Forms.Button
    Friend WithEvents ComboBox_Datensatz As System.Windows.Forms.ComboBox
    Friend WithEvents MenuItem_DatensatzZurücksetzen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItem_Optionen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItem_Hilfe As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItem_About As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItem_Wiki As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBox_TChartButtons As System.Windows.Forms.GroupBox
    Friend WithEvents Button_TChartEdit As System.Windows.Forms.Button
    Friend WithEvents Button_TChart2PNG As System.Windows.Forms.Button
    Friend WithEvents Button_TChartSave As System.Windows.Forms.Button
    Friend WithEvents Button_TChart2Excel As System.Windows.Forms.Button
    Friend WithEvents Info As System.Windows.Forms.GroupBox
    Friend WithEvents Label_Dn As System.Windows.Forms.Label
    Friend WithEvents Label_Dn_Wert As System.Windows.Forms.Label
End Class
