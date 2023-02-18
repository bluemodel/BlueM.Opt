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
    'Hinweis: Die folgende Prozedur wird vom Windows Form-Designer benötigt.
    'Das Verändern mit dem Windows Form-Designer ist nicht möglich.
    'Das Verändern mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim MenuStrip1 As System.Windows.Forms.MenuStrip
        Dim MenuItem_Tools As System.Windows.Forms.ToolStripMenuItem
        Dim MenuItem_Info As System.Windows.Forms.ToolStripMenuItem
        Dim ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Dim Margins1 As Steema.TeeChart.Margins = New Steema.TeeChart.Margins()
        Me.MenuItem_DatensatzZurücksetzen = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Help = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReleaseNotesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_About = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Button_Start = New System.Windows.Forms.Button()
        Me.Button_Stop = New System.Windows.Forms.Button()
        Me.GroupBox_Anwendung = New System.Windows.Forms.GroupBox()
        Me.Button_BrowseDatensatz = New System.Windows.Forms.Button()
        Me.ComboBox_Datensatz = New System.Windows.Forms.ComboBox()
        Me.Label_Anwendung = New System.Windows.Forms.Label()
        Me.ComboBox_Anwendung = New System.Windows.Forms.ComboBox()
        Me.Label_Methode = New System.Windows.Forms.Label()
        Me.ComboBox_Methode = New System.Windows.Forms.ComboBox()
        Me.Label_Datensatz = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton_New = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSplitButton_Settings = New System.Windows.Forms.ToolStripSplitButton()
        Me.ToolStripMenuItem_SettingsLoad = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSplitButton_ErgebnisDB = New System.Windows.Forms.ToolStripSplitButton()
        Me.ToolStripMenuItem_ErgebnisDBLoad = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ErgebnisDBCompare = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSplitButton_Diagramm = New System.Windows.Forms.ToolStripSplitButton()
        Me.ToolStripMenuItem_TChartEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TChartSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TChart2PNG = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Tchart2CSV = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripButton_Monitor = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton_Scatterplot = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton_CustomPlot = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton_SelectedSolutions = New System.Windows.Forms.ToolStripButton()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.Hauptdiagramm1 = New BlueM.Opt.Diagramm.Hauptdiagramm()
        Me.EVO_Opt_Verlauf1 = New BlueM.Opt.EVO_Opt_Verlauf()
        Me.EVO_Einstellungen1 = New BlueM.Opt.EVO_Einstellungen()
        MenuStrip1 = New System.Windows.Forms.MenuStrip()
        MenuItem_Tools = New System.Windows.Forms.ToolStripMenuItem()
        MenuItem_Info = New System.Windows.Forms.ToolStripMenuItem()
        ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        MenuStrip1.SuspendLayout()
        Me.GroupBox_Anwendung.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {MenuItem_Tools, MenuItem_Info})
        MenuStrip1.Location = New System.Drawing.Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No
        MenuStrip1.Size = New System.Drawing.Size(722, 24)
        MenuStrip1.TabIndex = 15
        MenuStrip1.Text = "MenuStrip1"
        '
        'MenuItem_Tools
        '
        MenuItem_Tools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem_DatensatzZurücksetzen})
        MenuItem_Tools.Name = "MenuItem_Tools"
        MenuItem_Tools.Size = New System.Drawing.Size(46, 20)
        MenuItem_Tools.Text = "Tools"
        '
        'MenuItem_DatensatzZurücksetzen
        '
        Me.MenuItem_DatensatzZurücksetzen.Enabled = False
        Me.MenuItem_DatensatzZurücksetzen.Name = "MenuItem_DatensatzZurücksetzen"
        Me.MenuItem_DatensatzZurücksetzen.Size = New System.Drawing.Size(143, 22)
        Me.MenuItem_DatensatzZurücksetzen.Text = "Reset dataset"
        '
        'MenuItem_Info
        '
        MenuItem_Info.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        MenuItem_Info.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_Help, Me.ReleaseNotesToolStripMenuItem, Me.ToolStripMenuItem_About})
        MenuItem_Info.Name = "MenuItem_Info"
        MenuItem_Info.Size = New System.Drawing.Size(24, 20)
        MenuItem_Info.Text = "?"
        '
        'ToolStripMenuItem_Help
        '
        Me.ToolStripMenuItem_Help.Name = "ToolStripMenuItem_Help"
        Me.ToolStripMenuItem_Help.Size = New System.Drawing.Size(145, 22)
        Me.ToolStripMenuItem_Help.Text = "Help"
        '
        'ReleaseNotesToolStripMenuItem
        '
        Me.ReleaseNotesToolStripMenuItem.Name = "ReleaseNotesToolStripMenuItem"
        Me.ReleaseNotesToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.ReleaseNotesToolStripMenuItem.Text = "Release notes"
        '
        'ToolStripMenuItem_About
        '
        Me.ToolStripMenuItem_About.Name = "ToolStripMenuItem_About"
        Me.ToolStripMenuItem_About.Size = New System.Drawing.Size(145, 22)
        Me.ToolStripMenuItem_About.Text = "About"
        '
        'ToolStripSeparator4
        '
        ToolStripSeparator4.Name = "ToolStripSeparator4"
        ToolStripSeparator4.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripSeparator3
        '
        ToolStripSeparator3.Name = "ToolStripSeparator3"
        ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripSeparator5
        '
        ToolStripSeparator5.Name = "ToolStripSeparator5"
        ToolStripSeparator5.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripSeparator6
        '
        ToolStripSeparator6.Name = "ToolStripSeparator6"
        ToolStripSeparator6.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripSeparator8
        '
        ToolStripSeparator8.Name = "ToolStripSeparator8"
        ToolStripSeparator8.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripSeparator7
        '
        ToolStripSeparator7.Name = "ToolStripSeparator7"
        ToolStripSeparator7.Size = New System.Drawing.Size(6, 25)
        '
        'Button_Start
        '
        Me.Button_Start.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Start.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_Start.Location = New System.Drawing.Point(4, 106)
        Me.Button_Start.Name = "Button_Start"
        Me.Button_Start.Size = New System.Drawing.Size(165, 38)
        Me.Button_Start.TabIndex = 3
        Me.Button_Start.Text = "Start"
        Me.ToolTip1.SetToolTip(Me.Button_Start, "Start optimization")
        Me.Button_Start.UseVisualStyleBackColor = True
        '
        'Button_Stop
        '
        Me.Button_Stop.Enabled = False
        Me.Button_Stop.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_Stop.Location = New System.Drawing.Point(176, 106)
        Me.Button_Stop.Name = "Button_Stop"
        Me.Button_Stop.Size = New System.Drawing.Size(59, 38)
        Me.Button_Stop.TabIndex = 21
        Me.Button_Stop.Text = "Stop"
        Me.ToolTip1.SetToolTip(Me.Button_Stop, "Abort optimization")
        Me.Button_Stop.UseVisualStyleBackColor = True
        '
        'GroupBox_Anwendung
        '
        Me.GroupBox_Anwendung.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox_Anwendung.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox_Anwendung.Controls.Add(Me.Button_BrowseDatensatz)
        Me.GroupBox_Anwendung.Controls.Add(Me.ComboBox_Datensatz)
        Me.GroupBox_Anwendung.Controls.Add(Me.Label_Anwendung)
        Me.GroupBox_Anwendung.Controls.Add(Me.ComboBox_Anwendung)
        Me.GroupBox_Anwendung.Controls.Add(Me.Label_Methode)
        Me.GroupBox_Anwendung.Controls.Add(Me.ComboBox_Methode)
        Me.GroupBox_Anwendung.Controls.Add(Me.Label_Datensatz)
        Me.GroupBox_Anwendung.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox_Anwendung.Location = New System.Drawing.Point(4, 48)
        Me.GroupBox_Anwendung.Name = "GroupBox_Anwendung"
        Me.GroupBox_Anwendung.Size = New System.Drawing.Size(714, 50)
        Me.GroupBox_Anwendung.TabIndex = 0
        Me.GroupBox_Anwendung.TabStop = False
        Me.GroupBox_Anwendung.Text = "Application"
        '
        'Button_BrowseDatensatz
        '
        Me.Button_BrowseDatensatz.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_BrowseDatensatz.Location = New System.Drawing.Point(520, 17)
        Me.Button_BrowseDatensatz.Name = "Button_BrowseDatensatz"
        Me.Button_BrowseDatensatz.Size = New System.Drawing.Size(24, 23)
        Me.Button_BrowseDatensatz.TabIndex = 14
        Me.Button_BrowseDatensatz.Text = "..."
        Me.Button_BrowseDatensatz.UseVisualStyleBackColor = True
        '
        'ComboBox_Datensatz
        '
        Me.ComboBox_Datensatz.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_Datensatz.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Datensatz.FormattingEnabled = True
        Me.ComboBox_Datensatz.Location = New System.Drawing.Point(200, 18)
        Me.ComboBox_Datensatz.Name = "ComboBox_Datensatz"
        Me.ComboBox_Datensatz.Size = New System.Drawing.Size(314, 21)
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
        Me.Label_Methode.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label_Methode.AutoSize = True
        Me.Label_Methode.Location = New System.Drawing.Point(555, 22)
        Me.Label_Methode.Name = "Label_Methode"
        Me.Label_Methode.Size = New System.Drawing.Size(46, 13)
        Me.Label_Methode.TabIndex = 11
        Me.Label_Methode.Text = "Method:"
        '
        'ComboBox_Methode
        '
        Me.ComboBox_Methode.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_Methode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Methode.FormattingEnabled = True
        Me.ComboBox_Methode.Location = New System.Drawing.Point(608, 18)
        Me.ComboBox_Methode.Name = "ComboBox_Methode"
        Me.ComboBox_Methode.Size = New System.Drawing.Size(100, 21)
        Me.ComboBox_Methode.TabIndex = 10
        '
        'Label_Datensatz
        '
        Me.Label_Datensatz.AutoSize = True
        Me.Label_Datensatz.Location = New System.Drawing.Point(147, 22)
        Me.Label_Datensatz.Name = "Label_Datensatz"
        Me.Label_Datensatz.Size = New System.Drawing.Size(47, 13)
        Me.Label_Datensatz.TabIndex = 12
        Me.Label_Datensatz.Text = "Dataset:"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton_New, ToolStripSeparator7, Me.ToolStripSplitButton_Settings, ToolStripSeparator5, Me.ToolStripSplitButton_ErgebnisDB, ToolStripSeparator4, Me.ToolStripSplitButton_Diagramm, ToolStripSeparator3, Me.ToolStripButton_Monitor, ToolStripSeparator6, Me.ToolStripButton_Scatterplot, Me.ToolStripButton_CustomPlot, ToolStripSeparator8, Me.ToolStripButton_SelectedSolutions})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(722, 25)
        Me.ToolStrip1.TabIndex = 19
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton_New
        '
        Me.ToolStripButton_New.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_New.Image = Global.BlueM.Opt.My.Resources.Resources.page_white
        Me.ToolStripButton_New.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_New.Name = "ToolStripButton_New"
        Me.ToolStripButton_New.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_New.Text = "New"
        Me.ToolStripButton_New.ToolTipText = "Start a new optimization"
        '
        'ToolStripSplitButton_Settings
        '
        Me.ToolStripSplitButton_Settings.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_SettingsLoad})
        Me.ToolStripSplitButton_Settings.Image = Global.BlueM.Opt.My.Resources.Resources.wrench
        Me.ToolStripSplitButton_Settings.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripSplitButton_Settings.Name = "ToolStripSplitButton_Settings"
        Me.ToolStripSplitButton_Settings.Size = New System.Drawing.Size(81, 22)
        Me.ToolStripSplitButton_Settings.Text = "Settings"
        '
        'ToolStripMenuItem_SettingsLoad
        '
        Me.ToolStripMenuItem_SettingsLoad.Image = Global.BlueM.Opt.My.Resources.Resources.page_white_get
        Me.ToolStripMenuItem_SettingsLoad.Name = "ToolStripMenuItem_SettingsLoad"
        Me.ToolStripMenuItem_SettingsLoad.Size = New System.Drawing.Size(180, 22)
        Me.ToolStripMenuItem_SettingsLoad.Text = "Load settings..."
        '
        'ToolStripSplitButton_ErgebnisDB
        '
        Me.ToolStripSplitButton_ErgebnisDB.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_ErgebnisDBLoad, Me.ToolStripMenuItem_ErgebnisDBCompare})
        Me.ToolStripSplitButton_ErgebnisDB.Image = Global.BlueM.Opt.My.Resources.Resources.database
        Me.ToolStripSplitButton_ErgebnisDB.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripSplitButton_ErgebnisDB.Name = "ToolStripSplitButton_ErgebnisDB"
        Me.ToolStripSplitButton_ErgebnisDB.Size = New System.Drawing.Size(71, 22)
        Me.ToolStripSplitButton_ErgebnisDB.Text = "Result"
        Me.ToolStripSplitButton_ErgebnisDB.ToolTipText = "Optimization result"
        '
        'ToolStripMenuItem_ErgebnisDBLoad
        '
        Me.ToolStripMenuItem_ErgebnisDBLoad.Image = Global.BlueM.Opt.My.Resources.Resources.database_connect
        Me.ToolStripMenuItem_ErgebnisDBLoad.Name = "ToolStripMenuItem_ErgebnisDBLoad"
        Me.ToolStripMenuItem_ErgebnisDBLoad.Size = New System.Drawing.Size(169, 22)
        Me.ToolStripMenuItem_ErgebnisDBLoad.Text = "Load result DB..."
        Me.ToolStripMenuItem_ErgebnisDBLoad.ToolTipText = "Optimierungsergebnis aus bestehender DB laden"
        '
        'ToolStripMenuItem_ErgebnisDBCompare
        '
        Me.ToolStripMenuItem_ErgebnisDBCompare.Image = Global.BlueM.Opt.My.Resources.Resources.database_go
        Me.ToolStripMenuItem_ErgebnisDBCompare.Name = "ToolStripMenuItem_ErgebnisDBCompare"
        Me.ToolStripMenuItem_ErgebnisDBCompare.Size = New System.Drawing.Size(169, 22)
        Me.ToolStripMenuItem_ErgebnisDBCompare.Text = "Compare results..."
        Me.ToolStripMenuItem_ErgebnisDBCompare.ToolTipText = "mit einem anderen Optimierungsergebnis vergleichen"
        '
        'ToolStripSplitButton_Diagramm
        '
        Me.ToolStripSplitButton_Diagramm.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_TChartEdit, Me.ToolStripMenuItem_TChartSave, Me.ToolStripMenuItem_TChart2PNG, Me.ToolStripMenuItem_Tchart2CSV})
        Me.ToolStripSplitButton_Diagramm.Image = Global.BlueM.Opt.My.Resources.Resources.chart_curve
        Me.ToolStripSplitButton_Diagramm.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripSplitButton_Diagramm.Name = "ToolStripSplitButton_Diagramm"
        Me.ToolStripSplitButton_Diagramm.Size = New System.Drawing.Size(68, 22)
        Me.ToolStripSplitButton_Diagramm.Text = "Chart"
        '
        'ToolStripMenuItem_TChartEdit
        '
        Me.ToolStripMenuItem_TChartEdit.Image = Global.BlueM.Opt.My.Resources.Resources.chart_curve_edit
        Me.ToolStripMenuItem_TChartEdit.Name = "ToolStripMenuItem_TChartEdit"
        Me.ToolStripMenuItem_TChartEdit.Size = New System.Drawing.Size(134, 22)
        Me.ToolStripMenuItem_TChartEdit.Text = "Edit chart..."
        Me.ToolStripMenuItem_TChartEdit.ToolTipText = "Diagramm bearbeiten"
        '
        'ToolStripMenuItem_TChartSave
        '
        Me.ToolStripMenuItem_TChartSave.Image = Global.BlueM.Opt.My.Resources.Resources.icon_teechart
        Me.ToolStripMenuItem_TChartSave.Name = "ToolStripMenuItem_TChartSave"
        Me.ToolStripMenuItem_TChartSave.Size = New System.Drawing.Size(134, 22)
        Me.ToolStripMenuItem_TChartSave.Text = "Save TEN..."
        Me.ToolStripMenuItem_TChartSave.ToolTipText = "Diagramm im TEN-Format speichern"
        '
        'ToolStripMenuItem_TChart2PNG
        '
        Me.ToolStripMenuItem_TChart2PNG.Image = Global.BlueM.Opt.My.Resources.Resources.icon_png
        Me.ToolStripMenuItem_TChart2PNG.Name = "ToolStripMenuItem_TChart2PNG"
        Me.ToolStripMenuItem_TChart2PNG.Size = New System.Drawing.Size(134, 22)
        Me.ToolStripMenuItem_TChart2PNG.Text = "Save PNG..."
        Me.ToolStripMenuItem_TChart2PNG.ToolTipText = "Diagramm als PNG-Datei speichern"
        '
        'ToolStripMenuItem_Tchart2CSV
        '
        Me.ToolStripMenuItem_Tchart2CSV.Image = Global.BlueM.Opt.My.Resources.Resources.icon_excel
        Me.ToolStripMenuItem_Tchart2CSV.Name = "ToolStripMenuItem_Tchart2CSV"
        Me.ToolStripMenuItem_Tchart2CSV.Size = New System.Drawing.Size(134, 22)
        Me.ToolStripMenuItem_Tchart2CSV.Text = "Save CSV..."
        Me.ToolStripMenuItem_Tchart2CSV.ToolTipText = "Diagrammdaten als CSV-Datei speichern"
        '
        'ToolStripButton_Monitor
        '
        Me.ToolStripButton_Monitor.CheckOnClick = True
        Me.ToolStripButton_Monitor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_Monitor.Image = Global.BlueM.Opt.My.Resources.Resources.monitor
        Me.ToolStripButton_Monitor.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_Monitor.Name = "ToolStripButton_Monitor"
        Me.ToolStripButton_Monitor.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_Monitor.Text = "Show/hide Monitor"
        '
        'ToolStripButton_Scatterplot
        '
        Me.ToolStripButton_Scatterplot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_Scatterplot.Image = Global.BlueM.Opt.My.Resources.Resources.scatterplot
        Me.ToolStripButton_Scatterplot.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_Scatterplot.Name = "ToolStripButton_Scatterplot"
        Me.ToolStripButton_Scatterplot.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_Scatterplot.Text = "Scatterplot"
        Me.ToolStripButton_Scatterplot.ToolTipText = "Display scatterplot matrix"
        '
        'ToolStripButton_CustomPlot
        '
        Me.ToolStripButton_CustomPlot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_CustomPlot.Image = CType(resources.GetObject("ToolStripButton_CustomPlot.Image"), System.Drawing.Image)
        Me.ToolStripButton_CustomPlot.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_CustomPlot.Name = "ToolStripButton_CustomPlot"
        Me.ToolStripButton_CustomPlot.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_CustomPlot.Text = "Display/update custom plot"
        Me.ToolStripButton_CustomPlot.ToolTipText = "Display/update custom plot"
        '
        'ToolStripButton_SelectedSolutions
        '
        Me.ToolStripButton_SelectedSolutions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_SelectedSolutions.Image = Global.BlueM.Opt.My.Resources.Resources.table
        Me.ToolStripButton_SelectedSolutions.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_SelectedSolutions.Name = "ToolStripButton_SelectedSolutions"
        Me.ToolStripButton_SelectedSolutions.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_SelectedSolutions.Text = "Show selected solutions window"
        Me.ToolStripButton_SelectedSolutions.ToolTipText = "Show selected solutions window"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 819)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(722, 22)
        Me.StatusStrip1.TabIndex = 20
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'Hauptdiagramm1
        '
        Me.Hauptdiagramm1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
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
        Me.Hauptdiagramm1.Axes.Bottom.Grid.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Brush.Color = System.Drawing.Color.White
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Font.Bold = False
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Font.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Font.Size = 9
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Bottom.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Axes.Bottom.Labels.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Bottom.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Bottom.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Bottom.Title.Angle = 0
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Bottom.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Bottom.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Bottom.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Bottom.Title.Brush.Color = System.Drawing.Color.Silver
        Me.Hauptdiagramm1.Axes.Bottom.Title.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Bottom.Title.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Bottom.Title.Font.Bold = False
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Bottom.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Bottom.Title.Font.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Bottom.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Bottom.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Bottom.Title.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Bottom.Title.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.Bottom.Title.Font.Size = 11
        Me.Hauptdiagramm1.Axes.Bottom.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Bottom.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Axes.Bottom.Title.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Bottom.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Bottom.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Bottom.Title.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Bottom.Title.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.Bottom.UseMaxPixelPos = True
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Depth.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Depth.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Depth.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Depth.Labels.Brush.Color = System.Drawing.Color.White
        Me.Hauptdiagramm1.Axes.Depth.Labels.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Depth.Labels.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Depth.Labels.Font.Bold = False
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Depth.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Depth.Labels.Font.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Depth.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Depth.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Depth.Labels.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Depth.Labels.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.Depth.Labels.Font.Size = 9
        Me.Hauptdiagramm1.Axes.Depth.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Depth.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Axes.Depth.Labels.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Depth.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Depth.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Depth.Labels.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Depth.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Depth.Title.Angle = 0
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Depth.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Depth.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Depth.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Depth.Title.Brush.Color = System.Drawing.Color.Silver
        Me.Hauptdiagramm1.Axes.Depth.Title.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Depth.Title.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Depth.Title.Font.Bold = False
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Depth.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Depth.Title.Font.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Depth.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Depth.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Depth.Title.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Depth.Title.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.Depth.Title.Font.Size = 11
        Me.Hauptdiagramm1.Axes.Depth.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Depth.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Axes.Depth.Title.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Depth.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Depth.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Depth.Title.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Depth.Title.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.Depth.UseMaxPixelPos = True
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Brush.Color = System.Drawing.Color.White
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Font.Bold = False
        '
        '
        '
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Font.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Font.Size = 9
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.DepthTop.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Angle = 0
        '
        '
        '
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Brush.Color = System.Drawing.Color.Silver
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Font.Bold = False
        '
        '
        '
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Font.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Font.Size = 11
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.DepthTop.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Axes.DepthTop.Title.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.DepthTop.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.DepthTop.Title.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.DepthTop.UseMaxPixelPos = True
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Left.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Left.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Left.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Left.Labels.Brush.Color = System.Drawing.Color.White
        Me.Hauptdiagramm1.Axes.Left.Labels.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Left.Labels.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Left.Labels.Font.Bold = False
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Left.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Left.Labels.Font.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Left.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Left.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Left.Labels.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Left.Labels.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.Left.Labels.Font.Size = 9
        Me.Hauptdiagramm1.Axes.Left.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Left.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Axes.Left.Labels.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Left.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Left.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Left.Labels.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Left.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Left.Title.Angle = 90
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Left.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Left.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Left.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Left.Title.Brush.Color = System.Drawing.Color.Silver
        Me.Hauptdiagramm1.Axes.Left.Title.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Left.Title.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Left.Title.Font.Bold = False
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Left.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Left.Title.Font.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Left.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Left.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Left.Title.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Left.Title.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.Left.Title.Font.Size = 11
        Me.Hauptdiagramm1.Axes.Left.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Left.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Axes.Left.Title.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Left.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Left.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Left.Title.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Left.Title.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.Left.UseMaxPixelPos = True
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Right.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Right.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Right.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Right.Labels.Brush.Color = System.Drawing.Color.White
        Me.Hauptdiagramm1.Axes.Right.Labels.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Right.Labels.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Right.Labels.Font.Bold = False
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Right.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Right.Labels.Font.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Right.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Right.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Right.Labels.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Right.Labels.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.Right.Labels.Font.Size = 9
        Me.Hauptdiagramm1.Axes.Right.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Right.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Axes.Right.Labels.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Right.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Right.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Right.Labels.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Right.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Right.Title.Angle = 270
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Right.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Right.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Right.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Right.Title.Brush.Color = System.Drawing.Color.Silver
        Me.Hauptdiagramm1.Axes.Right.Title.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Right.Title.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Right.Title.Font.Bold = False
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Right.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Right.Title.Font.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Right.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Right.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Right.Title.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Right.Title.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.Right.Title.Font.Size = 11
        Me.Hauptdiagramm1.Axes.Right.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Right.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Axes.Right.Title.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Right.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Right.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Right.Title.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Right.Title.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.Right.UseMaxPixelPos = True
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Top.Labels.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Top.Labels.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Top.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Top.Labels.Brush.Color = System.Drawing.Color.White
        Me.Hauptdiagramm1.Axes.Top.Labels.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Top.Labels.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Top.Labels.Font.Bold = False
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Top.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Top.Labels.Font.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Top.Labels.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Top.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Top.Labels.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Top.Labels.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.Top.Labels.Font.Size = 9
        Me.Hauptdiagramm1.Axes.Top.Labels.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Top.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Axes.Top.Labels.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Top.Labels.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Top.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Top.Labels.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Top.Labels.Shadow.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Top.Title.Angle = 0
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Top.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Top.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Top.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Top.Title.Brush.Color = System.Drawing.Color.Silver
        Me.Hauptdiagramm1.Axes.Top.Title.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Top.Title.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Top.Title.Font.Bold = False
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Top.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Hauptdiagramm1.Axes.Top.Title.Font.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Top.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Top.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Top.Title.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Top.Title.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.Top.Title.Font.Size = 11
        Me.Hauptdiagramm1.Axes.Top.Title.Font.SizeFloat = 11.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Top.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Axes.Top.Title.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Top.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Axes.Top.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Axes.Top.Title.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Axes.Top.Title.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Axes.Top.UseMaxPixelPos = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Footer.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Footer.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Footer.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Footer.Brush.Color = System.Drawing.Color.Silver
        Me.Hauptdiagramm1.Footer.Brush.Solid = True
        Me.Hauptdiagramm1.Footer.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Footer.Font.Bold = False
        '
        '
        '
        Me.Hauptdiagramm1.Footer.Font.Brush.Color = System.Drawing.Color.Red
        Me.Hauptdiagramm1.Footer.Font.Brush.Solid = True
        Me.Hauptdiagramm1.Footer.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Footer.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Footer.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Footer.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Footer.Font.Size = 8
        Me.Hauptdiagramm1.Footer.Font.SizeFloat = 8.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Footer.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Footer.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Footer.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Footer.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Footer.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Footer.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Header.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Header.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Header.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Header.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Hauptdiagramm1.Header.Brush.Solid = True
        Me.Hauptdiagramm1.Header.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Header.Font.Bold = False
        '
        '
        '
        Me.Hauptdiagramm1.Header.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Header.Font.Brush.Solid = True
        Me.Hauptdiagramm1.Header.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Header.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Header.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Header.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Header.Font.Size = 12
        Me.Hauptdiagramm1.Header.Font.SizeFloat = 12.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Header.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Header.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Header.ImageBevel.Brush.Visible = True
        Me.Hauptdiagramm1.Header.Lines = New String() {"BlueM.Opt"}
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Header.Shadow.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.Hauptdiagramm1.Header.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Header.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Legend.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Legend.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Legend.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Legend.Brush.Color = System.Drawing.Color.White
        Me.Hauptdiagramm1.Legend.Brush.Solid = True
        Me.Hauptdiagramm1.Legend.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Legend.Font.Bold = False
        '
        '
        '
        Me.Hauptdiagramm1.Legend.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Hauptdiagramm1.Legend.Font.Brush.Solid = True
        Me.Hauptdiagramm1.Legend.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Legend.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Legend.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Legend.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Legend.Font.Size = 9
        Me.Hauptdiagramm1.Legend.Font.SizeFloat = 9.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Legend.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Legend.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Legend.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Legend.Shadow.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Hauptdiagramm1.Legend.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Legend.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Legend.Symbol.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Legend.Symbol.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Legend.Symbol.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Legend.Title.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Legend.Title.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Legend.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Legend.Title.Brush.Color = System.Drawing.Color.White
        Me.Hauptdiagramm1.Legend.Title.Brush.Solid = True
        Me.Hauptdiagramm1.Legend.Title.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.Legend.Title.Font.Bold = True
        '
        '
        '
        Me.Hauptdiagramm1.Legend.Title.Font.Brush.Color = System.Drawing.Color.Black
        Me.Hauptdiagramm1.Legend.Title.Font.Brush.Solid = True
        Me.Hauptdiagramm1.Legend.Title.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Legend.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Legend.Title.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Legend.Title.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Legend.Title.Font.Size = 8
        Me.Hauptdiagramm1.Legend.Title.Font.SizeFloat = 8.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Legend.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Legend.Title.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Legend.Title.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Legend.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Legend.Title.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Legend.Title.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Location = New System.Drawing.Point(241, 106)
        Me.Hauptdiagramm1.MinimumSize = New System.Drawing.Size(477, 0)
        Me.Hauptdiagramm1.Name = "Hauptdiagramm1"
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Panel.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Panel.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Panel.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Panel.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Panel.Brush.Solid = True
        Me.Hauptdiagramm1.Panel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Panel.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Panel.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Panel.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Panel.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Panel.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Panel.Shadow.Brush.Visible = True
        '
        '
        '
        Margins1.Bottom = 100
        Margins1.Left = 100
        Margins1.Right = 100
        Margins1.Top = 100
        Me.Hauptdiagramm1.Printer.Margins = Margins1
        Me.Hauptdiagramm1.Size = New System.Drawing.Size(477, 627)
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.SubFooter.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.SubFooter.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.SubFooter.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.SubFooter.Brush.Color = System.Drawing.Color.Silver
        Me.Hauptdiagramm1.SubFooter.Brush.Solid = True
        Me.Hauptdiagramm1.SubFooter.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.SubFooter.Font.Bold = False
        '
        '
        '
        Me.Hauptdiagramm1.SubFooter.Font.Brush.Color = System.Drawing.Color.Red
        Me.Hauptdiagramm1.SubFooter.Font.Brush.Solid = True
        Me.Hauptdiagramm1.SubFooter.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.SubFooter.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.SubFooter.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.SubFooter.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.SubFooter.Font.Size = 8
        Me.Hauptdiagramm1.SubFooter.Font.SizeFloat = 8.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.SubFooter.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.SubFooter.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.SubFooter.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.SubFooter.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.SubFooter.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.SubFooter.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.SubHeader.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.SubHeader.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.SubHeader.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.SubHeader.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Hauptdiagramm1.SubHeader.Brush.Solid = True
        Me.Hauptdiagramm1.SubHeader.Brush.Visible = True
        '
        '
        '
        Me.Hauptdiagramm1.SubHeader.Font.Bold = False
        '
        '
        '
        Me.Hauptdiagramm1.SubHeader.Font.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.SubHeader.Font.Brush.Solid = True
        Me.Hauptdiagramm1.SubHeader.Font.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.SubHeader.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.SubHeader.Font.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.SubHeader.Font.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.SubHeader.Font.Size = 12
        Me.Hauptdiagramm1.SubHeader.Font.SizeFloat = 12.0!
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.SubHeader.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.SubHeader.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.SubHeader.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.SubHeader.Shadow.Brush.Color = System.Drawing.Color.FromArgb(CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.Hauptdiagramm1.SubHeader.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.SubHeader.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.TabIndex = 17
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Walls.Back.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Walls.Back.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Walls.Back.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Walls.Back.Brush.Color = System.Drawing.Color.Silver
        Me.Hauptdiagramm1.Walls.Back.Brush.Solid = True
        Me.Hauptdiagramm1.Walls.Back.Brush.Visible = False
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Walls.Back.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Walls.Back.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Walls.Back.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Walls.Back.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Walls.Back.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Walls.Back.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Walls.Bottom.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Walls.Bottom.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Walls.Bottom.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Walls.Bottom.Brush.Color = System.Drawing.Color.White
        Me.Hauptdiagramm1.Walls.Bottom.Brush.Solid = True
        Me.Hauptdiagramm1.Walls.Bottom.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Walls.Bottom.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Walls.Bottom.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Walls.Bottom.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Walls.Bottom.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Walls.Bottom.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Walls.Bottom.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Walls.Left.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Walls.Left.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Walls.Left.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Walls.Left.Brush.Color = System.Drawing.Color.LightYellow
        Me.Hauptdiagramm1.Walls.Left.Brush.Solid = True
        Me.Hauptdiagramm1.Walls.Left.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Walls.Left.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Walls.Left.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Walls.Left.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Walls.Left.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Walls.Left.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Walls.Left.Shadow.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Walls.Right.Bevel.ColorOne = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Hauptdiagramm1.Walls.Right.Bevel.ColorTwo = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Hauptdiagramm1.Walls.Right.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
        '
        '
        '
        Me.Hauptdiagramm1.Walls.Right.Brush.Color = System.Drawing.Color.LightYellow
        Me.Hauptdiagramm1.Walls.Right.Brush.Solid = True
        Me.Hauptdiagramm1.Walls.Right.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Walls.Right.ImageBevel.Brush.Color = System.Drawing.Color.LightGray
        Me.Hauptdiagramm1.Walls.Right.ImageBevel.Brush.Solid = True
        Me.Hauptdiagramm1.Walls.Right.ImageBevel.Brush.Visible = True
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Walls.Right.Shadow.Brush.Color = System.Drawing.Color.DarkGray
        Me.Hauptdiagramm1.Walls.Right.Shadow.Brush.Solid = True
        Me.Hauptdiagramm1.Walls.Right.Shadow.Brush.Visible = True
        Me.Hauptdiagramm1.Walls.View3D = False
        '
        '
        '
        '
        '
        '
        Me.Hauptdiagramm1.Zoom.Brush.Color = System.Drawing.Color.LightBlue
        Me.Hauptdiagramm1.Zoom.Brush.Solid = True
        Me.Hauptdiagramm1.Zoom.Brush.Visible = True
        '
        'EVO_Opt_Verlauf1
        '
        Me.EVO_Opt_Verlauf1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EVO_Opt_Verlauf1.Location = New System.Drawing.Point(241, 739)
        Me.EVO_Opt_Verlauf1.MinimumSize = New System.Drawing.Size(464, 76)
        Me.EVO_Opt_Verlauf1.Name = "EVO_Opt_Verlauf1"
        Me.EVO_Opt_Verlauf1.Size = New System.Drawing.Size(478, 79)
        Me.EVO_Opt_Verlauf1.TabIndex = 6
        '
        'EVO_Einstellungen1
        '
        Me.EVO_Einstellungen1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.EVO_Einstellungen1.Location = New System.Drawing.Point(4, 148)
        Me.EVO_Einstellungen1.Name = "EVO_Einstellungen1"
        Me.EVO_Einstellungen1.Size = New System.Drawing.Size(244, 707)
        Me.EVO_Einstellungen1.TabIndex = 2
        '
        'Form1
        '
        Me.AcceptButton = Me.Button_Start
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(722, 841)
        Me.Controls.Add(Me.Button_Stop)
        Me.Controls.Add(Me.GroupBox_Anwendung)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.Button_Start)
        Me.Controls.Add(Me.EVO_Opt_Verlauf1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.Hauptdiagramm1)
        Me.Controls.Add(MenuStrip1)
        Me.Controls.Add(Me.EVO_Einstellungen1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = MenuStrip1
        Me.MinimumSize = New System.Drawing.Size(730, 500)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BlueM.Opt"
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        Me.GroupBox_Anwendung.ResumeLayout(False)
        Me.GroupBox_Anwendung.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Public WithEvents EVO_Einstellungen1 As BlueM.Opt.EVO_Einstellungen
    Private WithEvents Label_Datensatz As System.Windows.Forms.Label
    Private WithEvents GroupBox_Anwendung As System.Windows.Forms.GroupBox
    Private WithEvents ComboBox_Anwendung As System.Windows.Forms.ComboBox
    Private WithEvents Label_Anwendung As System.Windows.Forms.Label
    Private WithEvents ComboBox_Methode As System.Windows.Forms.ComboBox
    Private WithEvents Label_Methode As System.Windows.Forms.Label
    Friend WithEvents Button_BrowseDatensatz As System.Windows.Forms.Button
    Friend WithEvents ComboBox_Datensatz As System.Windows.Forms.ComboBox
    Private WithEvents MenuItem_DatensatzZurücksetzen As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents ToolStripMenuItem_About As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents ToolStripMenuItem_Help As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Private WithEvents ToolStripSplitButton_Diagramm As System.Windows.Forms.ToolStripSplitButton
    Private WithEvents ToolStripMenuItem_TChartSave As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents ToolStripMenuItem_TChart2PNG As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents ToolStripMenuItem_Tchart2CSV As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents ToolStripSplitButton_ErgebnisDB As System.Windows.Forms.ToolStripSplitButton
    Private WithEvents ToolStripMenuItem_ErgebnisDBLoad As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents ToolStripMenuItem_ErgebnisDBCompare As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents ToolStripMenuItem_TChartEdit As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents ToolStripButton_Scatterplot As System.Windows.Forms.ToolStripButton
    Private WithEvents Button_Start As System.Windows.Forms.Button
    Private WithEvents EVO_Opt_Verlauf1 As BlueM.Opt.EVO_Opt_Verlauf
    Private WithEvents ToolStripSplitButton_Settings As System.Windows.Forms.ToolStripSplitButton
    Private WithEvents Button_Stop As System.Windows.Forms.Button
    Private WithEvents ToolStripButton_New As System.Windows.Forms.ToolStripButton
    Private WithEvents ToolStripButton_Monitor As System.Windows.Forms.ToolStripButton
    Private WithEvents ToolStripMenuItem_SettingsLoad As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripButton_SelectedSolutions As System.Windows.Forms.ToolStripButton
    Friend WithEvents ReleaseNotesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripButton_CustomPlot As ToolStripButton
End Class
