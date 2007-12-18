<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EVO_Einstellungen

    Inherits System.Windows.Forms.UserControl

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
    'Hinweis: Die folgende Prozedur wird vom Windows Form-Designer benötigt.
    'Das Verändern mit dem Windows Form-Designer ist nicht möglich.
    'Das Verändern mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EVO_Einstellungen))
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage_PES = New System.Windows.Forms.TabPage
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.ÖffnenToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.SpeichernToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.Label_OptModus = New System.Windows.Forms.Label
        Me.Label_OptModusValue = New System.Windows.Forms.Label
        Me.LabelStrategie = New System.Windows.Forms.Label
        Me.ComboStrategie = New System.Windows.Forms.ComboBox
        Me.LabelStartwerte = New System.Windows.Forms.Label
        Me.ComboOptVorgabe = New System.Windows.Forms.ComboBox
        Me.LabelStartSchrittweite = New System.Windows.Forms.Label
        Me.TextDeltaStart = New System.Windows.Forms.NumericUpDown
        Me.CheckisDnVektor = New System.Windows.Forms.CheckBox
        Me.GroupBox_Generationen = New System.Windows.Forms.GroupBox
        Me.TextNMemberSecondPop = New System.Windows.Forms.NumericUpDown
        Me.TextInteract = New System.Windows.Forms.NumericUpDown
        Me.TextRekombxy = New System.Windows.Forms.NumericUpDown
        Me.ComboOptEltern = New System.Windows.Forms.ComboBox
        Me.TextAnzNachf = New System.Windows.Forms.NumericUpDown
        Me.TextAnzEltern = New System.Windows.Forms.NumericUpDown
        Me.TextAnzGen = New System.Windows.Forms.NumericUpDown
        Me.LabelNMemberSecondPop = New System.Windows.Forms.Label
        Me.LabelInteract = New System.Windows.Forms.Label
        Me.Line2 = New System.Windows.Forms.Label
        Me.LabelRekombxy3 = New System.Windows.Forms.Label
        Me.LabelRekombxy1 = New System.Windows.Forms.Label
        Me.Line1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.LabelAnzNachf = New System.Windows.Forms.Label
        Me.LabelAnzEltern = New System.Windows.Forms.Label
        Me.LabelAnzGen = New System.Windows.Forms.Label
        Me.CheckisPopul = New System.Windows.Forms.CheckBox
        Me.GroupBox_Populationen = New System.Windows.Forms.GroupBox
        Me.LabelAnzRunden = New System.Windows.Forms.Label
        Me.TextAnzRunden = New System.Windows.Forms.NumericUpDown
        Me.LabelAnzPop = New System.Windows.Forms.Label
        Me.TextAnzPop = New System.Windows.Forms.NumericUpDown
        Me.LabelAnzPopEltern = New System.Windows.Forms.Label
        Me.TextAnzPopEltern = New System.Windows.Forms.NumericUpDown
        Me.LabelOptPopEltern = New System.Windows.Forms.Label
        Me.ComboOptPopEltern = New System.Windows.Forms.ComboBox
        Me.LabelPopStrategie = New System.Windows.Forms.Label
        Me.ComboPopStrategie = New System.Windows.Forms.ComboBox
        Me.LabelPopPenalty = New System.Windows.Forms.Label
        Me.ComboPopPenalty = New System.Windows.Forms.ComboBox
        Me.TabPage_CES = New System.Windows.Forms.TabPage
        Me.TabPage_HookJeeves = New System.Windows.Forms.TabPage
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextDeltaFinishHJ = New System.Windows.Forms.NumericUpDown
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextDeltaStartHJ = New System.Windows.Forms.NumericUpDown
        Me.CheckBoxDNVektorHJ = New System.Windows.Forms.CheckBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox_AllgSettings = New System.Windows.Forms.GroupBox
        Me.TabControl1.SuspendLayout()
        Me.TabPage_PES.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.TextDeltaStart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_Generationen.SuspendLayout()
        CType(Me.TextNMemberSecondPop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextInteract, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextRekombxy, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextAnzNachf, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextAnzEltern, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextAnzGen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_Populationen.SuspendLayout()
        CType(Me.TextAnzRunden, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextAnzPop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextAnzPopEltern, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage_HookJeeves.SuspendLayout()
        CType(Me.TextDeltaFinishHJ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextDeltaStartHJ, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage_PES)
        Me.TabControl1.Controls.Add(Me.TabPage_CES)
        Me.TabControl1.Controls.Add(Me.TabPage_HookJeeves)
        Me.TabControl1.Location = New System.Drawing.Point(0, 105)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.Padding = New System.Drawing.Point(0, 0)
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(220, 595)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage_PES
        '
        Me.TabPage_PES.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage_PES.Controls.Add(Me.ToolStrip1)
        Me.TabPage_PES.Controls.Add(Me.Label_OptModus)
        Me.TabPage_PES.Controls.Add(Me.Label_OptModusValue)
        Me.TabPage_PES.Controls.Add(Me.LabelStrategie)
        Me.TabPage_PES.Controls.Add(Me.ComboStrategie)
        Me.TabPage_PES.Controls.Add(Me.LabelStartwerte)
        Me.TabPage_PES.Controls.Add(Me.ComboOptVorgabe)
        Me.TabPage_PES.Controls.Add(Me.LabelStartSchrittweite)
        Me.TabPage_PES.Controls.Add(Me.TextDeltaStart)
        Me.TabPage_PES.Controls.Add(Me.CheckisDnVektor)
        Me.TabPage_PES.Controls.Add(Me.GroupBox_Generationen)
        Me.TabPage_PES.Controls.Add(Me.CheckisPopul)
        Me.TabPage_PES.Controls.Add(Me.GroupBox_Populationen)
        Me.TabPage_PES.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_PES.Margin = New System.Windows.Forms.Padding(0)
        Me.TabPage_PES.Name = "TabPage_PES"
        Me.TabPage_PES.Size = New System.Drawing.Size(212, 569)
        Me.TabPage_PES.TabIndex = 0
        Me.TabPage_PES.Text = "PES"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ÖffnenToolStripButton, Me.SpeichernToolStripButton})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Padding = New System.Windows.Forms.Padding(0)
        Me.ToolStrip1.Size = New System.Drawing.Size(212, 25)
        Me.ToolStrip1.Stretch = True
        Me.ToolStrip1.TabIndex = 36
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ÖffnenToolStripButton
        '
        Me.ÖffnenToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ÖffnenToolStripButton.Image = CType(resources.GetObject("ÖffnenToolStripButton.Image"), System.Drawing.Image)
        Me.ÖffnenToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ÖffnenToolStripButton.Name = "ÖffnenToolStripButton"
        Me.ÖffnenToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.ÖffnenToolStripButton.Text = "Ö&ffnen"
        '
        'SpeichernToolStripButton
        '
        Me.SpeichernToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.SpeichernToolStripButton.Image = CType(resources.GetObject("SpeichernToolStripButton.Image"), System.Drawing.Image)
        Me.SpeichernToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SpeichernToolStripButton.Name = "SpeichernToolStripButton"
        Me.SpeichernToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.SpeichernToolStripButton.Text = "&Speichern"
        '
        'Label_OptModus
        '
        Me.Label_OptModus.AutoSize = True
        Me.Label_OptModus.Location = New System.Drawing.Point(2, 32)
        Me.Label_OptModus.Name = "Label_OptModus"
        Me.Label_OptModus.Size = New System.Drawing.Size(42, 13)
        Me.Label_OptModus.TabIndex = 31
        Me.Label_OptModus.Text = "Modus:"
        '
        'Label_OptModusValue
        '
        Me.Label_OptModusValue.AutoSize = True
        Me.Label_OptModusValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_OptModusValue.Location = New System.Drawing.Point(77, 31)
        Me.Label_OptModusValue.Name = "Label_OptModusValue"
        Me.Label_OptModusValue.Size = New System.Drawing.Size(0, 13)
        Me.Label_OptModusValue.TabIndex = 32
        '
        'LabelStrategie
        '
        Me.LabelStrategie.AutoSize = True
        Me.LabelStrategie.Location = New System.Drawing.Point(2, 58)
        Me.LabelStrategie.Name = "LabelStrategie"
        Me.LabelStrategie.Size = New System.Drawing.Size(54, 13)
        Me.LabelStrategie.TabIndex = 19
        Me.LabelStrategie.Text = "Selektion:"
        '
        'ComboStrategie
        '
        Me.ComboStrategie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboStrategie.Location = New System.Drawing.Point(80, 57)
        Me.ComboStrategie.Name = "ComboStrategie"
        Me.ComboStrategie.Size = New System.Drawing.Size(123, 21)
        Me.ComboStrategie.TabIndex = 0
        '
        'LabelStartwerte
        '
        Me.LabelStartwerte.AutoSize = True
        Me.LabelStartwerte.Location = New System.Drawing.Point(2, 85)
        Me.LabelStartwerte.Name = "LabelStartwerte"
        Me.LabelStartwerte.Size = New System.Drawing.Size(58, 13)
        Me.LabelStartwerte.TabIndex = 29
        Me.LabelStartwerte.Text = "Startwerte:"
        '
        'ComboOptVorgabe
        '
        Me.ComboOptVorgabe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptVorgabe.Location = New System.Drawing.Point(80, 84)
        Me.ComboOptVorgabe.Name = "ComboOptVorgabe"
        Me.ComboOptVorgabe.Size = New System.Drawing.Size(123, 21)
        Me.ComboOptVorgabe.TabIndex = 1
        '
        'LabelStartSchrittweite
        '
        Me.LabelStartSchrittweite.AutoSize = True
        Me.LabelStartSchrittweite.Location = New System.Drawing.Point(2, 111)
        Me.LabelStartSchrittweite.Name = "LabelStartSchrittweite"
        Me.LabelStartSchrittweite.Size = New System.Drawing.Size(90, 13)
        Me.LabelStartSchrittweite.TabIndex = 27
        Me.LabelStartSchrittweite.Text = "Start-Schrittweite:"
        '
        'TextDeltaStart
        '
        Me.TextDeltaStart.DecimalPlaces = 2
        Me.TextDeltaStart.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.TextDeltaStart.Location = New System.Drawing.Point(150, 111)
        Me.TextDeltaStart.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.TextDeltaStart.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.TextDeltaStart.Name = "TextDeltaStart"
        Me.TextDeltaStart.Size = New System.Drawing.Size(53, 20)
        Me.TextDeltaStart.TabIndex = 2
        Me.TextDeltaStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextDeltaStart.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'CheckisDnVektor
        '
        Me.CheckisDnVektor.Location = New System.Drawing.Point(11, 136)
        Me.CheckisDnVektor.Name = "CheckisDnVektor"
        Me.CheckisDnVektor.Size = New System.Drawing.Size(144, 18)
        Me.CheckisDnVektor.TabIndex = 3
        Me.CheckisDnVektor.Text = "mit Schrittweitenvektor"
        '
        'GroupBox_Generationen
        '
        Me.GroupBox_Generationen.Controls.Add(Me.TextNMemberSecondPop)
        Me.GroupBox_Generationen.Controls.Add(Me.TextInteract)
        Me.GroupBox_Generationen.Controls.Add(Me.TextRekombxy)
        Me.GroupBox_Generationen.Controls.Add(Me.ComboOptEltern)
        Me.GroupBox_Generationen.Controls.Add(Me.TextAnzNachf)
        Me.GroupBox_Generationen.Controls.Add(Me.TextAnzEltern)
        Me.GroupBox_Generationen.Controls.Add(Me.TextAnzGen)
        Me.GroupBox_Generationen.Controls.Add(Me.LabelNMemberSecondPop)
        Me.GroupBox_Generationen.Controls.Add(Me.LabelInteract)
        Me.GroupBox_Generationen.Controls.Add(Me.Line2)
        Me.GroupBox_Generationen.Controls.Add(Me.LabelRekombxy3)
        Me.GroupBox_Generationen.Controls.Add(Me.LabelRekombxy1)
        Me.GroupBox_Generationen.Controls.Add(Me.Line1)
        Me.GroupBox_Generationen.Controls.Add(Me.Label2)
        Me.GroupBox_Generationen.Controls.Add(Me.LabelAnzNachf)
        Me.GroupBox_Generationen.Controls.Add(Me.LabelAnzEltern)
        Me.GroupBox_Generationen.Controls.Add(Me.LabelAnzGen)
        Me.GroupBox_Generationen.Location = New System.Drawing.Point(5, 159)
        Me.GroupBox_Generationen.Name = "GroupBox_Generationen"
        Me.GroupBox_Generationen.Size = New System.Drawing.Size(199, 217)
        Me.GroupBox_Generationen.TabIndex = 4
        Me.GroupBox_Generationen.TabStop = False
        Me.GroupBox_Generationen.Text = "Generationen:"
        '
        'TextNMemberSecondPop
        '
        Me.TextNMemberSecondPop.Location = New System.Drawing.Point(140, 120)
        Me.TextNMemberSecondPop.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.TextNMemberSecondPop.Name = "TextNMemberSecondPop"
        Me.TextNMemberSecondPop.Size = New System.Drawing.Size(53, 20)
        Me.TextNMemberSecondPop.TabIndex = 4
        Me.TextNMemberSecondPop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextNMemberSecondPop.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'TextInteract
        '
        Me.TextInteract.Location = New System.Drawing.Point(140, 96)
        Me.TextInteract.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.TextInteract.Name = "TextInteract"
        Me.TextInteract.Size = New System.Drawing.Size(53, 20)
        Me.TextInteract.TabIndex = 3
        Me.TextInteract.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextInteract.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'TextRekombxy
        '
        Me.TextRekombxy.Location = New System.Drawing.Point(32, 192)
        Me.TextRekombxy.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TextRekombxy.Name = "TextRekombxy"
        Me.TextRekombxy.Size = New System.Drawing.Size(40, 20)
        Me.TextRekombxy.TabIndex = 6
        Me.TextRekombxy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextRekombxy.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'ComboOptEltern
        '
        Me.ComboOptEltern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptEltern.Location = New System.Drawing.Point(64, 160)
        Me.ComboOptEltern.Name = "ComboOptEltern"
        Me.ComboOptEltern.Size = New System.Drawing.Size(129, 21)
        Me.ComboOptEltern.TabIndex = 5
        '
        'TextAnzNachf
        '
        Me.TextAnzNachf.Location = New System.Drawing.Point(140, 61)
        Me.TextAnzNachf.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.TextAnzNachf.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TextAnzNachf.Name = "TextAnzNachf"
        Me.TextAnzNachf.Size = New System.Drawing.Size(53, 20)
        Me.TextAnzNachf.TabIndex = 2
        Me.TextAnzNachf.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextAnzNachf.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'TextAnzEltern
        '
        Me.TextAnzEltern.Location = New System.Drawing.Point(140, 39)
        Me.TextAnzEltern.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.TextAnzEltern.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TextAnzEltern.Name = "TextAnzEltern"
        Me.TextAnzEltern.Size = New System.Drawing.Size(53, 20)
        Me.TextAnzEltern.TabIndex = 1
        Me.TextAnzEltern.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextAnzEltern.Value = New Decimal(New Integer() {3, 0, 0, 0})
        '
        'TextAnzGen
        '
        Me.TextAnzGen.Location = New System.Drawing.Point(140, 16)
        Me.TextAnzGen.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
        Me.TextAnzGen.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TextAnzGen.Name = "TextAnzGen"
        Me.TextAnzGen.Size = New System.Drawing.Size(53, 20)
        Me.TextAnzGen.TabIndex = 0
        Me.TextAnzGen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextAnzGen.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'LabelNMemberSecondPop
        '
        Me.LabelNMemberSecondPop.Location = New System.Drawing.Point(8, 120)
        Me.LabelNMemberSecondPop.Name = "LabelNMemberSecondPop"
        Me.LabelNMemberSecondPop.Size = New System.Drawing.Size(128, 16)
        Me.LabelNMemberSecondPop.TabIndex = 40
        Me.LabelNMemberSecondPop.Text = "Max. Mitglieder 2nd Pop"
        '
        'LabelInteract
        '
        Me.LabelInteract.AutoSize = True
        Me.LabelInteract.Location = New System.Drawing.Point(8, 98)
        Me.LabelInteract.Name = "LabelInteract"
        Me.LabelInteract.Size = New System.Drawing.Size(121, 13)
        Me.LabelInteract.TabIndex = 38
        Me.LabelInteract.Text = "Austausch mit sek. Pop:"
        '
        'Line2
        '
        Me.Line2.BackColor = System.Drawing.SystemColors.WindowText
        Me.Line2.Location = New System.Drawing.Point(8, 148)
        Me.Line2.Name = "Line2"
        Me.Line2.Size = New System.Drawing.Size(176, 1)
        Me.Line2.TabIndex = 41
        '
        'LabelRekombxy3
        '
        Me.LabelRekombxy3.Location = New System.Drawing.Point(74, 194)
        Me.LabelRekombxy3.Name = "LabelRekombxy3"
        Me.LabelRekombxy3.Size = New System.Drawing.Size(98, 17)
        Me.LabelRekombxy3.TabIndex = 25
        Me.LabelRekombxy3.Text = "-Rekombination"
        '
        'LabelRekombxy1
        '
        Me.LabelRekombxy1.Location = New System.Drawing.Point(8, 195)
        Me.LabelRekombxy1.Name = "LabelRekombxy1"
        Me.LabelRekombxy1.Size = New System.Drawing.Size(24, 17)
        Me.LabelRekombxy1.TabIndex = 6
        Me.LabelRekombxy1.Text = "X /"
        '
        'Line1
        '
        Me.Line1.BackColor = System.Drawing.SystemColors.WindowText
        Me.Line1.Location = New System.Drawing.Point(8, 88)
        Me.Line1.Name = "Line1"
        Me.Line1.Size = New System.Drawing.Size(176, 1)
        Me.Line1.TabIndex = 42
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 155)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 34)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "Ermitteln der Eltern:"
        '
        'LabelAnzNachf
        '
        Me.LabelAnzNachf.AutoSize = True
        Me.LabelAnzNachf.Location = New System.Drawing.Point(8, 64)
        Me.LabelAnzNachf.Name = "LabelAnzNachf"
        Me.LabelAnzNachf.Size = New System.Drawing.Size(97, 13)
        Me.LabelAnzNachf.TabIndex = 16
        Me.LabelAnzNachf.Text = "Anzahl Nachfolger:"
        '
        'LabelAnzEltern
        '
        Me.LabelAnzEltern.AutoSize = True
        Me.LabelAnzEltern.Location = New System.Drawing.Point(7, 42)
        Me.LabelAnzEltern.Name = "LabelAnzEltern"
        Me.LabelAnzEltern.Size = New System.Drawing.Size(72, 13)
        Me.LabelAnzEltern.TabIndex = 15
        Me.LabelAnzEltern.Text = "Anzahl Eltern:"
        '
        'LabelAnzGen
        '
        Me.LabelAnzGen.AutoSize = True
        Me.LabelAnzGen.Location = New System.Drawing.Point(7, 20)
        Me.LabelAnzGen.Name = "LabelAnzGen"
        Me.LabelAnzGen.Size = New System.Drawing.Size(127, 13)
        Me.LabelAnzGen.TabIndex = 14
        Me.LabelAnzGen.Text = "Anzahl der Generationen:"
        '
        'CheckisPopul
        '
        Me.CheckisPopul.Location = New System.Drawing.Point(5, 385)
        Me.CheckisPopul.Name = "CheckisPopul"
        Me.CheckisPopul.Size = New System.Drawing.Size(112, 18)
        Me.CheckisPopul.TabIndex = 5
        Me.CheckisPopul.Text = "mit Populationen"
        '
        'GroupBox_Populationen
        '
        Me.GroupBox_Populationen.Controls.Add(Me.LabelAnzRunden)
        Me.GroupBox_Populationen.Controls.Add(Me.TextAnzRunden)
        Me.GroupBox_Populationen.Controls.Add(Me.LabelAnzPop)
        Me.GroupBox_Populationen.Controls.Add(Me.TextAnzPop)
        Me.GroupBox_Populationen.Controls.Add(Me.LabelAnzPopEltern)
        Me.GroupBox_Populationen.Controls.Add(Me.TextAnzPopEltern)
        Me.GroupBox_Populationen.Controls.Add(Me.LabelOptPopEltern)
        Me.GroupBox_Populationen.Controls.Add(Me.ComboOptPopEltern)
        Me.GroupBox_Populationen.Controls.Add(Me.LabelPopStrategie)
        Me.GroupBox_Populationen.Controls.Add(Me.ComboPopStrategie)
        Me.GroupBox_Populationen.Controls.Add(Me.LabelPopPenalty)
        Me.GroupBox_Populationen.Controls.Add(Me.ComboPopPenalty)
        Me.GroupBox_Populationen.Enabled = False
        Me.GroupBox_Populationen.Location = New System.Drawing.Point(5, 407)
        Me.GroupBox_Populationen.Name = "GroupBox_Populationen"
        Me.GroupBox_Populationen.Size = New System.Drawing.Size(199, 158)
        Me.GroupBox_Populationen.TabIndex = 6
        Me.GroupBox_Populationen.TabStop = False
        Me.GroupBox_Populationen.Text = "Populationen:"
        '
        'LabelAnzRunden
        '
        Me.LabelAnzRunden.AutoSize = True
        Me.LabelAnzRunden.Location = New System.Drawing.Point(8, 16)
        Me.LabelAnzRunden.Name = "LabelAnzRunden"
        Me.LabelAnzRunden.Size = New System.Drawing.Size(101, 13)
        Me.LabelAnzRunden.TabIndex = 6
        Me.LabelAnzRunden.Text = "Anzahl der Runden:"
        '
        'TextAnzRunden
        '
        Me.TextAnzRunden.Location = New System.Drawing.Point(140, 13)
        Me.TextAnzRunden.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
        Me.TextAnzRunden.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TextAnzRunden.Name = "TextAnzRunden"
        Me.TextAnzRunden.Size = New System.Drawing.Size(53, 20)
        Me.TextAnzRunden.TabIndex = 0
        Me.TextAnzRunden.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextAnzRunden.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'LabelAnzPop
        '
        Me.LabelAnzPop.AutoSize = True
        Me.LabelAnzPop.Location = New System.Drawing.Point(7, 36)
        Me.LabelAnzPop.Name = "LabelAnzPop"
        Me.LabelAnzPop.Size = New System.Drawing.Size(107, 13)
        Me.LabelAnzPop.TabIndex = 7
        Me.LabelAnzPop.Text = "Anzahl Populationen:"
        '
        'TextAnzPop
        '
        Me.TextAnzPop.Location = New System.Drawing.Point(140, 36)
        Me.TextAnzPop.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.TextAnzPop.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TextAnzPop.Name = "TextAnzPop"
        Me.TextAnzPop.Size = New System.Drawing.Size(53, 20)
        Me.TextAnzPop.TabIndex = 1
        Me.TextAnzPop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextAnzPop.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'LabelAnzPopEltern
        '
        Me.LabelAnzPopEltern.AutoSize = True
        Me.LabelAnzPopEltern.Location = New System.Drawing.Point(7, 58)
        Me.LabelAnzPopEltern.Name = "LabelAnzPopEltern"
        Me.LabelAnzPopEltern.Size = New System.Drawing.Size(112, 13)
        Me.LabelAnzPopEltern.TabIndex = 8
        Me.LabelAnzPopEltern.Text = "Anzahl Eltern [max=5]:"
        '
        'TextAnzPopEltern
        '
        Me.TextAnzPopEltern.Location = New System.Drawing.Point(140, 58)
        Me.TextAnzPopEltern.Maximum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.TextAnzPopEltern.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TextAnzPopEltern.Name = "TextAnzPopEltern"
        Me.TextAnzPopEltern.Size = New System.Drawing.Size(53, 20)
        Me.TextAnzPopEltern.TabIndex = 2
        Me.TextAnzPopEltern.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextAnzPopEltern.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'LabelOptPopEltern
        '
        Me.LabelOptPopEltern.Location = New System.Drawing.Point(8, 80)
        Me.LabelOptPopEltern.Name = "LabelOptPopEltern"
        Me.LabelOptPopEltern.Size = New System.Drawing.Size(76, 28)
        Me.LabelOptPopEltern.TabIndex = 9
        Me.LabelOptPopEltern.Text = "Ermittlung der Pop-Eltern:"
        '
        'ComboOptPopEltern
        '
        Me.ComboOptPopEltern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptPopEltern.Location = New System.Drawing.Point(85, 84)
        Me.ComboOptPopEltern.Name = "ComboOptPopEltern"
        Me.ComboOptPopEltern.Size = New System.Drawing.Size(108, 21)
        Me.ComboOptPopEltern.TabIndex = 3
        '
        'LabelPopStrategie
        '
        Me.LabelPopStrategie.Location = New System.Drawing.Point(8, 110)
        Me.LabelPopStrategie.Name = "LabelPopStrategie"
        Me.LabelPopStrategie.Size = New System.Drawing.Size(76, 17)
        Me.LabelPopStrategie.TabIndex = 31
        Me.LabelPopStrategie.Text = "Selektion:"
        '
        'ComboPopStrategie
        '
        Me.ComboPopStrategie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboPopStrategie.Location = New System.Drawing.Point(85, 108)
        Me.ComboPopStrategie.Name = "ComboPopStrategie"
        Me.ComboPopStrategie.Size = New System.Drawing.Size(108, 21)
        Me.ComboPopStrategie.TabIndex = 4
        '
        'LabelPopPenalty
        '
        Me.LabelPopPenalty.Location = New System.Drawing.Point(8, 127)
        Me.LabelPopPenalty.Name = "LabelPopPenalty"
        Me.LabelPopPenalty.Size = New System.Drawing.Size(76, 29)
        Me.LabelPopPenalty.TabIndex = 36
        Me.LabelPopPenalty.Text = "Ermittlung der Pop-Güte:"
        '
        'ComboPopPenalty
        '
        Me.ComboPopPenalty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboPopPenalty.Location = New System.Drawing.Point(85, 132)
        Me.ComboPopPenalty.Name = "ComboPopPenalty"
        Me.ComboPopPenalty.Size = New System.Drawing.Size(108, 21)
        Me.ComboPopPenalty.TabIndex = 5
        '
        'TabPage_CES
        '
        Me.TabPage_CES.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_CES.Name = "TabPage_CES"
        Me.TabPage_CES.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_CES.Size = New System.Drawing.Size(212, 569)
        Me.TabPage_CES.TabIndex = 1
        Me.TabPage_CES.Text = "CES"
        Me.TabPage_CES.UseVisualStyleBackColor = True
        '
        'TabPage_HookJeeves
        '
        Me.TabPage_HookJeeves.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage_HookJeeves.Controls.Add(Me.Label3)
        Me.TabPage_HookJeeves.Controls.Add(Me.TextDeltaFinishHJ)
        Me.TabPage_HookJeeves.Controls.Add(Me.Label1)
        Me.TabPage_HookJeeves.Controls.Add(Me.TextDeltaStartHJ)
        Me.TabPage_HookJeeves.Controls.Add(Me.CheckBoxDNVektorHJ)
        Me.TabPage_HookJeeves.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_HookJeeves.Name = "TabPage_HookJeeves"
        Me.TabPage_HookJeeves.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_HookJeeves.Size = New System.Drawing.Size(212, 569)
        Me.TabPage_HookJeeves.TabIndex = 2
        Me.TabPage_HookJeeves.Text = "HookJeeves"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 43)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(87, 13)
        Me.Label3.TabIndex = 32
        Me.Label3.Text = "End-Schrittweite:"
        '
        'TextDeltaFinishHJ
        '
        Me.TextDeltaFinishHJ.DecimalPlaces = 5
        Me.TextDeltaFinishHJ.Increment = New Decimal(New Integer() {1, 0, 0, 327680})
        Me.TextDeltaFinishHJ.Location = New System.Drawing.Point(142, 39)
        Me.TextDeltaFinishHJ.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TextDeltaFinishHJ.Minimum = New Decimal(New Integer() {1, 0, 0, 327680})
        Me.TextDeltaFinishHJ.Name = "TextDeltaFinishHJ"
        Me.TextDeltaFinishHJ.Size = New System.Drawing.Size(65, 20)
        Me.TextDeltaFinishHJ.TabIndex = 1
        Me.TextDeltaFinishHJ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextDeltaFinishHJ.Value = New Decimal(New Integer() {1, 0, 0, 262144})
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 13)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "Start-Schrittweite:"
        '
        'TextDeltaStartHJ
        '
        Me.TextDeltaStartHJ.DecimalPlaces = 2
        Me.TextDeltaStartHJ.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.TextDeltaStartHJ.Location = New System.Drawing.Point(142, 9)
        Me.TextDeltaStartHJ.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.TextDeltaStartHJ.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.TextDeltaStartHJ.Name = "TextDeltaStartHJ"
        Me.TextDeltaStartHJ.Size = New System.Drawing.Size(65, 20)
        Me.TextDeltaStartHJ.TabIndex = 0
        Me.TextDeltaStartHJ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextDeltaStartHJ.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'CheckBoxDNVektorHJ
        '
        Me.CheckBoxDNVektorHJ.Enabled = False
        Me.CheckBoxDNVektorHJ.Location = New System.Drawing.Point(15, 76)
        Me.CheckBoxDNVektorHJ.Name = "CheckBoxDNVektorHJ"
        Me.CheckBoxDNVektorHJ.Size = New System.Drawing.Size(144, 18)
        Me.CheckBoxDNVektorHJ.TabIndex = 2
        Me.CheckBoxDNVektorHJ.Text = "mit Schrittweitenvektor"
        '
        'GroupBox_AllgSettings
        '
        Me.GroupBox_AllgSettings.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox_AllgSettings.Name = "GroupBox_AllgSettings"
        Me.GroupBox_AllgSettings.Size = New System.Drawing.Size(220, 102)
        Me.GroupBox_AllgSettings.TabIndex = 37
        Me.GroupBox_AllgSettings.TabStop = False
        Me.GroupBox_AllgSettings.Text = "Allgemeine Einstellungen"
        '
        'EVO_Einstellungen
        '
        Me.Controls.Add(Me.GroupBox_AllgSettings)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "EVO_Einstellungen"
        Me.Size = New System.Drawing.Size(236, 738)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage_PES.ResumeLayout(False)
        Me.TabPage_PES.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.TextDeltaStart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_Generationen.ResumeLayout(False)
        Me.GroupBox_Generationen.PerformLayout()
        CType(Me.TextNMemberSecondPop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextInteract, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextRekombxy, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextAnzNachf, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextAnzEltern, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextAnzGen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_Populationen.ResumeLayout(False)
        Me.GroupBox_Populationen.PerformLayout()
        CType(Me.TextAnzRunden, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextAnzPop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextAnzPopEltern, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage_HookJeeves.ResumeLayout(False)
        Me.TabPage_HookJeeves.PerformLayout()
        CType(Me.TextDeltaFinishHJ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextDeltaStartHJ, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Private WithEvents Label_OptModus As System.Windows.Forms.Label
    Private WithEvents Label_OptModusValue As System.Windows.Forms.Label
    Private WithEvents LabelStrategie As System.Windows.Forms.Label
    Private WithEvents ComboStrategie As System.Windows.Forms.ComboBox
    Private WithEvents LabelStartwerte As System.Windows.Forms.Label
    Private WithEvents ComboOptVorgabe As System.Windows.Forms.ComboBox
    Private WithEvents LabelStartSchrittweite As System.Windows.Forms.Label
    Private WithEvents TextDeltaStart As System.Windows.Forms.NumericUpDown
    Private WithEvents CheckisDnVektor As System.Windows.Forms.CheckBox
    Private WithEvents GroupBox_Generationen As System.Windows.Forms.GroupBox
    Private WithEvents TextNMemberSecondPop As System.Windows.Forms.NumericUpDown
    Private WithEvents TextInteract As System.Windows.Forms.NumericUpDown
    Private WithEvents TextRekombxy As System.Windows.Forms.NumericUpDown
    Private WithEvents ComboOptEltern As System.Windows.Forms.ComboBox
    Private WithEvents TextAnzNachf As System.Windows.Forms.NumericUpDown
    Private WithEvents TextAnzEltern As System.Windows.Forms.NumericUpDown
    Private WithEvents TextAnzGen As System.Windows.Forms.NumericUpDown
    Private WithEvents LabelNMemberSecondPop As System.Windows.Forms.Label
    Private WithEvents LabelInteract As System.Windows.Forms.Label
    Private WithEvents Line2 As System.Windows.Forms.Label
    Private WithEvents LabelRekombxy3 As System.Windows.Forms.Label
    Private WithEvents LabelRekombxy1 As System.Windows.Forms.Label
    Private WithEvents Line1 As System.Windows.Forms.Label
    Private WithEvents Label2 As System.Windows.Forms.Label
    Private WithEvents LabelAnzNachf As System.Windows.Forms.Label
    Private WithEvents LabelAnzEltern As System.Windows.Forms.Label
    Private WithEvents LabelAnzGen As System.Windows.Forms.Label
    Private WithEvents CheckisPopul As System.Windows.Forms.CheckBox
    Private WithEvents GroupBox_Populationen As System.Windows.Forms.GroupBox
    Private WithEvents LabelAnzRunden As System.Windows.Forms.Label
    Private WithEvents TextAnzRunden As System.Windows.Forms.NumericUpDown
    Private WithEvents LabelAnzPop As System.Windows.Forms.Label
    Private WithEvents TextAnzPop As System.Windows.Forms.NumericUpDown
    Private WithEvents LabelAnzPopEltern As System.Windows.Forms.Label
    Private WithEvents TextAnzPopEltern As System.Windows.Forms.NumericUpDown
    Private WithEvents LabelOptPopEltern As System.Windows.Forms.Label
    Private WithEvents ComboOptPopEltern As System.Windows.Forms.ComboBox
    Private WithEvents LabelPopStrategie As System.Windows.Forms.Label
    Private WithEvents ComboPopStrategie As System.Windows.Forms.ComboBox
    Private WithEvents LabelPopPenalty As System.Windows.Forms.Label
    Private WithEvents ComboPopPenalty As System.Windows.Forms.ComboBox
    Private WithEvents TabPage_CES As System.Windows.Forms.TabPage
    Friend WithEvents ÖffnenToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents SpeichernToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents GroupBox_AllgSettings As System.Windows.Forms.GroupBox
    Friend WithEvents TabPage_HookJeeves As System.Windows.Forms.TabPage
    Private WithEvents Label3 As System.Windows.Forms.Label
    Private WithEvents TextDeltaFinishHJ As System.Windows.Forms.NumericUpDown
    Private WithEvents Label1 As System.Windows.Forms.Label
    Private WithEvents TextDeltaStartHJ As System.Windows.Forms.NumericUpDown
    Private WithEvents CheckBoxDNVektorHJ As System.Windows.Forms.CheckBox
    Public WithEvents TabPage_PES As System.Windows.Forms.TabPage
    Public WithEvents TabControl1 As System.Windows.Forms.TabControl
End Class