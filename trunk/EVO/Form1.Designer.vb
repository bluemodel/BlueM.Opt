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
        Me.components = New System.ComponentModel.Container
        Dim MenuStrip1 As System.Windows.Forms.MenuStrip
        Dim MenuItem_Tools As System.Windows.Forms.ToolStripMenuItem
        Dim ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Dim MenuItem_Hilfe As System.Windows.Forms.ToolStripMenuItem
        Dim ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
        Dim ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.MenuItem_DatensatzZurücksetzen = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItem_Optionen = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItem_Wiki = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItem_About = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Button_Start = New System.Windows.Forms.Button
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
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.ToolStripButton_Options = New System.Windows.Forms.ToolStripButton
        Me.ToolStripButton_Monitor = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSplitButton_Diagramm = New System.Windows.Forms.ToolStripSplitButton
        Me.ToolStripMenuItem_TChartEdit = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem_TChartSave = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem_TChart2PNG = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem_Tchart2CSV = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSplitButton_ErgebnisDB = New System.Windows.Forms.ToolStripSplitButton
        Me.ToolStripMenuItem_ErgebnisDBSave = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem_ErgebnisDBLoad = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem_ErgebnisDBCompare = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripButton_Scatterplot = New System.Windows.Forms.ToolStripButton
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.Hauptdiagramm1 = New IHWB.EVO.Diagramm.Hauptdiagramm
        Me.EVO_Opt_Verlauf1 = New IHWB.EVO.EVO_Opt_Verlauf
        Me.EVO_Einstellungen1 = New IHWB.EVO.EVO_Einstellungen
        MenuStrip1 = New System.Windows.Forms.MenuStrip
        MenuItem_Tools = New System.Windows.Forms.ToolStripMenuItem
        ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        MenuItem_Hilfe = New System.Windows.Forms.ToolStripMenuItem
        ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator
        ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator
        MenuStrip1.SuspendLayout()
        Me.GroupBox_Anwendung.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {MenuItem_Tools, MenuItem_Hilfe})
        MenuStrip1.Location = New System.Drawing.Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No
        MenuStrip1.Size = New System.Drawing.Size(722, 24)
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
        MenuItem_Hilfe.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem_Wiki, ToolStripSeparator2, Me.MenuItem_About})
        MenuItem_Hilfe.Name = "MenuItem_Hilfe"
        MenuItem_Hilfe.Size = New System.Drawing.Size(40, 20)
        MenuItem_Hilfe.Text = "Hilfe"
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
        'Button_Start
        '
        Me.Button_Start.BackColor = System.Drawing.SystemColors.Control
        Me.Button_Start.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_Start.Enabled = False
        Me.Button_Start.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_Start.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_Start.Location = New System.Drawing.Point(40, 104)
        Me.Button_Start.Name = "Button_Start"
        Me.Button_Start.Size = New System.Drawing.Size(165, 38)
        Me.Button_Start.TabIndex = 3
        Me.Button_Start.Text = "Run"
        Me.ToolTip1.SetToolTip(Me.Button_Start, "Optimierung starten")
        Me.Button_Start.UseVisualStyleBackColor = True
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
        Me.GroupBox_Anwendung.Text = "Anwendung"
        '
        'Button_BrowseDatensatz
        '
        Me.Button_BrowseDatensatz.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_BrowseDatensatz.Enabled = False
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
        Me.ComboBox_Datensatz.Enabled = False
        Me.ComboBox_Datensatz.FormattingEnabled = True
        Me.ComboBox_Datensatz.Location = New System.Drawing.Point(206, 18)
        Me.ComboBox_Datensatz.Name = "ComboBox_Datensatz"
        Me.ComboBox_Datensatz.Size = New System.Drawing.Size(308, 21)
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
        Me.Label_Methode.Enabled = False
        Me.Label_Methode.Location = New System.Drawing.Point(555, 22)
        Me.Label_Methode.Name = "Label_Methode"
        Me.Label_Methode.Size = New System.Drawing.Size(52, 13)
        Me.Label_Methode.TabIndex = 11
        Me.Label_Methode.Text = "Methode:"
        '
        'ComboBox_Methode
        '
        Me.ComboBox_Methode.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_Methode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Methode.Enabled = False
        Me.ComboBox_Methode.FormattingEnabled = True
        Me.ComboBox_Methode.Location = New System.Drawing.Point(608, 18)
        Me.ComboBox_Methode.Name = "ComboBox_Methode"
        Me.ComboBox_Methode.Size = New System.Drawing.Size(100, 21)
        Me.ComboBox_Methode.TabIndex = 10
        '
        'Label_Datensatz
        '
        Me.Label_Datensatz.AutoSize = True
        Me.Label_Datensatz.Enabled = False
        Me.Label_Datensatz.Location = New System.Drawing.Point(147, 22)
        Me.Label_Datensatz.Name = "Label_Datensatz"
        Me.Label_Datensatz.Size = New System.Drawing.Size(58, 13)
        Me.Label_Datensatz.TabIndex = 12
        Me.Label_Datensatz.Text = "Datensatz:"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton_Options, ToolStripSeparator5, Me.ToolStripButton_Monitor, ToolStripSeparator6, Me.ToolStripSplitButton_ErgebnisDB, ToolStripSeparator4, Me.ToolStripSplitButton_Diagramm, ToolStripSeparator3, Me.ToolStripButton_Scatterplot, ToolStripSeparator8})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(722, 25)
        Me.ToolStrip1.TabIndex = 19
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton_Options
        '
        Me.ToolStripButton_Options.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_Options.Image = Global.IHWB.EVO.My.Resources.Resources.wrench
        Me.ToolStripButton_Options.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_Options.Name = "ToolStripButton_Options"
        Me.ToolStripButton_Options.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_Options.Text = "Optionen"
        '
        'ToolStripButton_Monitor
        '
        Me.ToolStripButton_Monitor.CheckOnClick = True
        Me.ToolStripButton_Monitor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_Monitor.Image = Global.IHWB.EVO.My.Resources.Resources.monitor
        Me.ToolStripButton_Monitor.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_Monitor.Name = "ToolStripButton_Monitor"
        Me.ToolStripButton_Monitor.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_Monitor.Text = "Monitor anzeigen/ausblenden"
        Me.ToolStripButton_Monitor.ToolTipText = "Monitor anzeigen/ausblenden"
        '
        'ToolStripSplitButton_Diagramm
        '
        Me.ToolStripSplitButton_Diagramm.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_TChartEdit, Me.ToolStripMenuItem_TChartSave, Me.ToolStripMenuItem_TChart2PNG, Me.ToolStripMenuItem_Tchart2CSV})
        Me.ToolStripSplitButton_Diagramm.Image = Global.IHWB.EVO.My.Resources.Resources.wave
        Me.ToolStripSplitButton_Diagramm.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripSplitButton_Diagramm.Name = "ToolStripSplitButton_Diagramm"
        Me.ToolStripSplitButton_Diagramm.Size = New System.Drawing.Size(86, 22)
        Me.ToolStripSplitButton_Diagramm.Text = "Diagramm"
        Me.ToolStripSplitButton_Diagramm.ToolTipText = "Diagramm"
        '
        'ToolStripMenuItem_TChartEdit
        '
        Me.ToolStripMenuItem_TChartEdit.Image = Global.IHWB.EVO.My.Resources.Resources.chart_curve_edit
        Me.ToolStripMenuItem_TChartEdit.Name = "ToolStripMenuItem_TChartEdit"
        Me.ToolStripMenuItem_TChartEdit.Size = New System.Drawing.Size(199, 22)
        Me.ToolStripMenuItem_TChartEdit.Text = "Diagramm bearbeiten..."
        Me.ToolStripMenuItem_TChartEdit.ToolTipText = "Diagramm bearbeiten"
        '
        'ToolStripMenuItem_TChartSave
        '
        Me.ToolStripMenuItem_TChartSave.Image = Global.IHWB.EVO.My.Resources.Resources.icon_teechart
        Me.ToolStripMenuItem_TChartSave.Name = "ToolStripMenuItem_TChartSave"
        Me.ToolStripMenuItem_TChartSave.Size = New System.Drawing.Size(199, 22)
        Me.ToolStripMenuItem_TChartSave.Text = "TEN speichern..."
        Me.ToolStripMenuItem_TChartSave.ToolTipText = "Diagramm im TEN-Format speichern"
        '
        'ToolStripMenuItem_TChart2PNG
        '
        Me.ToolStripMenuItem_TChart2PNG.Image = Global.IHWB.EVO.My.Resources.Resources.icon_png
        Me.ToolStripMenuItem_TChart2PNG.Name = "ToolStripMenuItem_TChart2PNG"
        Me.ToolStripMenuItem_TChart2PNG.Size = New System.Drawing.Size(199, 22)
        Me.ToolStripMenuItem_TChart2PNG.Text = "PNG speichern..."
        Me.ToolStripMenuItem_TChart2PNG.ToolTipText = "Diagramm als PNG-Datei speichern"
        '
        'ToolStripMenuItem_Tchart2CSV
        '
        Me.ToolStripMenuItem_Tchart2CSV.Image = Global.IHWB.EVO.My.Resources.Resources.icon_excel
        Me.ToolStripMenuItem_Tchart2CSV.Name = "ToolStripMenuItem_Tchart2CSV"
        Me.ToolStripMenuItem_Tchart2CSV.Size = New System.Drawing.Size(199, 22)
        Me.ToolStripMenuItem_Tchart2CSV.Text = "CSV speichern..."
        Me.ToolStripMenuItem_Tchart2CSV.ToolTipText = "Diagrammdaten als CSV-Datei speichern"
        '
        'ToolStripSplitButton_ErgebnisDB
        '
        Me.ToolStripSplitButton_ErgebnisDB.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_ErgebnisDBSave, Me.ToolStripMenuItem_ErgebnisDBLoad, Me.ToolStripMenuItem_ErgebnisDBCompare})
        Me.ToolStripSplitButton_ErgebnisDB.Image = Global.IHWB.EVO.My.Resources.Resources.database
        Me.ToolStripSplitButton_ErgebnisDB.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripSplitButton_ErgebnisDB.Name = "ToolStripSplitButton_ErgebnisDB"
        Me.ToolStripSplitButton_ErgebnisDB.Size = New System.Drawing.Size(80, 22)
        Me.ToolStripSplitButton_ErgebnisDB.Text = "Ergebnis"
        Me.ToolStripSplitButton_ErgebnisDB.ToolTipText = "Optimierungsergebnis"
        '
        'ToolStripMenuItem_ErgebnisDBSave
        '
        Me.ToolStripMenuItem_ErgebnisDBSave.Image = Global.IHWB.EVO.My.Resources.Resources.database_save
        Me.ToolStripMenuItem_ErgebnisDBSave.Name = "ToolStripMenuItem_ErgebnisDBSave"
        Me.ToolStripMenuItem_ErgebnisDBSave.Size = New System.Drawing.Size(200, 22)
        Me.ToolStripMenuItem_ErgebnisDBSave.Text = "ErgebnisDB speichern..."
        Me.ToolStripMenuItem_ErgebnisDBSave.ToolTipText = "Ergebnisdatenbank speichern..."
        '
        'ToolStripMenuItem_ErgebnisDBLoad
        '
        Me.ToolStripMenuItem_ErgebnisDBLoad.Image = Global.IHWB.EVO.My.Resources.Resources.database_connect
        Me.ToolStripMenuItem_ErgebnisDBLoad.Name = "ToolStripMenuItem_ErgebnisDBLoad"
        Me.ToolStripMenuItem_ErgebnisDBLoad.Size = New System.Drawing.Size(200, 22)
        Me.ToolStripMenuItem_ErgebnisDBLoad.Text = "ErgebnisDB laden..."
        Me.ToolStripMenuItem_ErgebnisDBLoad.ToolTipText = "Optimierungsergebnis aus bestehender DB laden"
        '
        'ToolStripMenuItem_ErgebnisDBCompare
        '
        Me.ToolStripMenuItem_ErgebnisDBCompare.Image = Global.IHWB.EVO.My.Resources.Resources.database_go
        Me.ToolStripMenuItem_ErgebnisDBCompare.Name = "ToolStripMenuItem_ErgebnisDBCompare"
        Me.ToolStripMenuItem_ErgebnisDBCompare.Size = New System.Drawing.Size(200, 22)
        Me.ToolStripMenuItem_ErgebnisDBCompare.Text = "Ergebnis vergleichen..."
        Me.ToolStripMenuItem_ErgebnisDBCompare.ToolTipText = "mit einem anderen Optimierungsergebnis vergleichen"
        '
        'ToolStripButton_Scatterplot
        '
        Me.ToolStripButton_Scatterplot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_Scatterplot.Image = Global.IHWB.EVO.My.Resources.Resources.scatterplot
        Me.ToolStripButton_Scatterplot.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_Scatterplot.Name = "ToolStripButton_Scatterplot"
        Me.ToolStripButton_Scatterplot.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_Scatterplot.Text = "Scatterplot"
        Me.ToolStripButton_Scatterplot.ToolTipText = "Scatterplotmatrix anzeigen"
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
        Me.Hauptdiagramm1.Aspect.View3D = False
        Me.Hauptdiagramm1.Aspect.ZOffset = 0
        '
        '
        '
        Me.Hauptdiagramm1.Header.Lines = New String() {"BlueM.Opt"}
        Me.Hauptdiagramm1.Location = New System.Drawing.Point(241, 106)
        Me.Hauptdiagramm1.MinimumSize = New System.Drawing.Size(477, 0)
        Me.Hauptdiagramm1.Name = "Hauptdiagramm1"
        Me.Hauptdiagramm1.Size = New System.Drawing.Size(477, 627)
        Me.Hauptdiagramm1.TabIndex = 17
        '
        '
        '
        Me.Hauptdiagramm1.Walls.View3D = False
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
        Me.EVO_Einstellungen1.Enabled = False
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
    Private WithEvents EVO_Einstellungen1 As IHWB.EVO.EVO_Einstellungen
    Private WithEvents Label_Datensatz As System.Windows.Forms.Label
    Private WithEvents GroupBox_Anwendung As System.Windows.Forms.GroupBox
    Private WithEvents ComboBox_Anwendung As System.Windows.Forms.ComboBox
    Private WithEvents Label_Anwendung As System.Windows.Forms.Label
    Private WithEvents ComboBox_Methode As System.Windows.Forms.ComboBox
    Private WithEvents Label_Methode As System.Windows.Forms.Label
    Friend WithEvents Button_BrowseDatensatz As System.Windows.Forms.Button
    Friend WithEvents ComboBox_Datensatz As System.Windows.Forms.ComboBox
    Private WithEvents MenuItem_DatensatzZurücksetzen As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents MenuItem_Optionen As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents MenuItem_About As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents MenuItem_Wiki As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Private WithEvents ToolStripButton_Monitor As System.Windows.Forms.ToolStripButton
    Private WithEvents ToolStripButton_Options As System.Windows.Forms.ToolStripButton
    Private WithEvents ToolStripSplitButton_Diagramm As System.Windows.Forms.ToolStripSplitButton
    Private WithEvents ToolStripMenuItem_TChartSave As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents ToolStripMenuItem_TChart2PNG As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents ToolStripMenuItem_Tchart2CSV As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents ToolStripSplitButton_ErgebnisDB As System.Windows.Forms.ToolStripSplitButton
    Private WithEvents ToolStripMenuItem_ErgebnisDBSave As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents ToolStripMenuItem_ErgebnisDBLoad As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents ToolStripMenuItem_ErgebnisDBCompare As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents ToolStripMenuItem_TChartEdit As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents ToolStripButton_Scatterplot As System.Windows.Forms.ToolStripButton
    Private WithEvents Button_Start As System.Windows.Forms.Button
    Private WithEvents EVO_Opt_Verlauf1 As IHWB.EVO.EVO_Opt_Verlauf
End Class
