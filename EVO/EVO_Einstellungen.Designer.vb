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
        Me.Label_OptModus = New System.Windows.Forms.Label
        Me.Label_OptModusValue = New System.Windows.Forms.Label
        Me.LabelStrategie = New System.Windows.Forms.Label
        Me.ComboOptStrategie = New System.Windows.Forms.ComboBox
        Me.LabelStartwerte = New System.Windows.Forms.Label
        Me.ComboOptStartparameter = New System.Windows.Forms.ComboBox
        Me.LabelMutation = New System.Windows.Forms.Label
        Me.ComboOptDnMutation = New System.Windows.Forms.ComboBox
        Me.LabelStartSchrittweite = New System.Windows.Forms.Label
        Me.TextDeltaStart = New System.Windows.Forms.NumericUpDown
        Me.CheckisDnVektor = New System.Windows.Forms.CheckBox
        Me.GroupBox_Generationen = New System.Windows.Forms.GroupBox
        Me.CheckisTournamentSelection = New System.Windows.Forms.CheckBox
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
        Me.ComboOptPopStrategie = New System.Windows.Forms.ComboBox
        Me.LabelPopPenalty = New System.Windows.Forms.Label
        Me.ComboOptPopPenalty = New System.Windows.Forms.ComboBox
        Me.checkpaintconstrained = New System.Windows.Forms.CheckBox
        Me.TabPage_CES = New System.Windows.Forms.TabPage
        Me.GroupBox_Hybrid = New System.Windows.Forms.GroupBox
        Me.CheckBox_use_sec_pop_PES = New System.Windows.Forms.CheckBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.CheckBox2 = New System.Windows.Forms.CheckBox
        Me.Numeric_n_member_SecPop_PES = New System.Windows.Forms.NumericUpDown
        Me.Numeric_max_PES_Parents = New System.Windows.Forms.NumericUpDown
        Me.Numeric_n_exchange_SecPop_PES = New System.Windows.Forms.NumericUpDown
        Me.Label_max_PES_Parents = New System.Windows.Forms.Label
        Me.Label_n_memebers_SecPop_PES = New System.Windows.Forms.Label
        Me.Numeric_mem_Strength = New System.Windows.Forms.NumericUpDown
        Me.Label_n_exchange_secPop_PES = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.ComboBox_Hybrid_Type = New System.Windows.Forms.ComboBox
        Me.Label_MemStrength = New System.Windows.Forms.Label
        Me.Label_Hybrid_Type = New System.Windows.Forms.Label
        Me.CheckBox_Use_Sec_Pop = New System.Windows.Forms.CheckBox
        Me.GroupBoxCES = New System.Windows.Forms.GroupBox
        Me.Use_ = New System.Windows.Forms.CheckBox
        Me.Numeric_MutRate = New System.Windows.Forms.NumericUpDown
        Me.Label_MutationRate = New System.Windows.Forms.Label
        Me.ComboCESReproduction = New System.Windows.Forms.ComboBox
        Me.LabelCESReproduction = New System.Windows.Forms.Label
        Me.Numeric_n_member_SecPop = New System.Windows.Forms.NumericUpDown
        Me.Numeric_n_exchange_SecPop = New System.Windows.Forms.NumericUpDown
        Me.Label_n_memebers_SecPop = New System.Windows.Forms.Label
        Me.Label_n_exchange_secPop = New System.Windows.Forms.Label
        Me.Numeric_n_childs = New System.Windows.Forms.NumericUpDown
        Me.Numeric_n_Parents = New System.Windows.Forms.NumericUpDown
        Me.ComboCESMutation = New System.Windows.Forms.ComboBox
        Me.Numeric_n_Generations = New System.Windows.Forms.NumericUpDown
        Me.ComboCESSelection = New System.Windows.Forms.ComboBox
        Me.Label_n_childs = New System.Windows.Forms.Label
        Me.Label_n_parents = New System.Windows.Forms.Label
        Me.Label_n_Generations = New System.Windows.Forms.Label
        Me.LabelCESMutation = New System.Windows.Forms.Label
        Me.LabelCESSelection = New System.Windows.Forms.Label
        Me.ComboCESIniValues = New System.Windows.Forms.ComboBox
        Me.LabelCESIniValues = New System.Windows.Forms.Label
        Me.Label_CES_OptModus = New System.Windows.Forms.Label
        Me.TabPage_HookeJeeves = New System.Windows.Forms.TabPage
        Me.LabelRSHJ = New System.Windows.Forms.Label
        Me.LabelESHJ = New System.Windows.Forms.Label
        Me.LabelTSHJgesamt = New System.Windows.Forms.Label
        Me.LabelTSHJmittel = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.LabelTSHJaktuelle = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextDeltaFinishHJ = New System.Windows.Forms.NumericUpDown
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextDeltaStartHJ = New System.Windows.Forms.NumericUpDown
        Me.CheckBoxDNVektorHJ = New System.Windows.Forms.CheckBox
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.ÖffnenToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.SpeichernToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox_Einstellungen = New System.Windows.Forms.GroupBox
        Me.TabControl1.SuspendLayout()
        Me.TabPage_PES.SuspendLayout()
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
        Me.TabPage_CES.SuspendLayout()
        Me.GroupBox_Hybrid.SuspendLayout()
        CType(Me.Numeric_n_member_SecPop_PES, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_max_PES_Parents, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_n_exchange_SecPop_PES, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_mem_Strength, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxCES.SuspendLayout()
        CType(Me.Numeric_MutRate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_n_member_SecPop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_n_exchange_SecPop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_n_childs, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_n_Parents, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_n_Generations, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage_HookeJeeves.SuspendLayout()
        CType(Me.TextDeltaFinishHJ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextDeltaStartHJ, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.GroupBox_Einstellungen.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage_PES)
        Me.TabControl1.Controls.Add(Me.TabPage_CES)
        Me.TabControl1.Controls.Add(Me.TabPage_HookeJeeves)
        Me.TabControl1.Location = New System.Drawing.Point(1, 41)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.Padding = New System.Drawing.Point(0, 0)
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(218, 656)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage_PES
        '
        Me.TabPage_PES.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage_PES.Controls.Add(Me.Label_OptModus)
        Me.TabPage_PES.Controls.Add(Me.Label_OptModusValue)
        Me.TabPage_PES.Controls.Add(Me.LabelStrategie)
        Me.TabPage_PES.Controls.Add(Me.ComboOptStrategie)
        Me.TabPage_PES.Controls.Add(Me.LabelStartwerte)
        Me.TabPage_PES.Controls.Add(Me.ComboOptStartparameter)
        Me.TabPage_PES.Controls.Add(Me.LabelMutation)
        Me.TabPage_PES.Controls.Add(Me.ComboOptDnMutation)
        Me.TabPage_PES.Controls.Add(Me.LabelStartSchrittweite)
        Me.TabPage_PES.Controls.Add(Me.TextDeltaStart)
        Me.TabPage_PES.Controls.Add(Me.CheckisDnVektor)
        Me.TabPage_PES.Controls.Add(Me.GroupBox_Generationen)
        Me.TabPage_PES.Controls.Add(Me.CheckisPopul)
        Me.TabPage_PES.Controls.Add(Me.GroupBox_Populationen)
        Me.TabPage_PES.Controls.Add(Me.checkpaintconstrained)
        Me.TabPage_PES.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_PES.Margin = New System.Windows.Forms.Padding(0)
        Me.TabPage_PES.Name = "TabPage_PES"
        Me.TabPage_PES.Size = New System.Drawing.Size(210, 630)
        Me.TabPage_PES.TabIndex = 0
        Me.TabPage_PES.Text = "PES"
        '
        'Label_OptModus
        '
        Me.Label_OptModus.AutoSize = True
        Me.Label_OptModus.Location = New System.Drawing.Point(2, 9)
        Me.Label_OptModus.Name = "Label_OptModus"
        Me.Label_OptModus.Size = New System.Drawing.Size(42, 13)
        Me.Label_OptModus.TabIndex = 31
        Me.Label_OptModus.Text = "Modus:"
        '
        'Label_OptModusValue
        '
        Me.Label_OptModusValue.AutoSize = True
        Me.Label_OptModusValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_OptModusValue.Location = New System.Drawing.Point(77, 8)
        Me.Label_OptModusValue.Name = "Label_OptModusValue"
        Me.Label_OptModusValue.Size = New System.Drawing.Size(0, 13)
        Me.Label_OptModusValue.TabIndex = 32
        '
        'LabelStrategie
        '
        Me.LabelStrategie.AutoSize = True
        Me.LabelStrategie.Location = New System.Drawing.Point(2, 37)
        Me.LabelStrategie.Name = "LabelStrategie"
        Me.LabelStrategie.Size = New System.Drawing.Size(54, 13)
        Me.LabelStrategie.TabIndex = 19
        Me.LabelStrategie.Text = "Selektion:"
        '
        'ComboOptStrategie
        '
        Me.ComboOptStrategie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptStrategie.Location = New System.Drawing.Point(80, 34)
        Me.ComboOptStrategie.Name = "ComboOptStrategie"
        Me.ComboOptStrategie.Size = New System.Drawing.Size(123, 21)
        Me.ComboOptStrategie.TabIndex = 0
        '
        'LabelStartwerte
        '
        Me.LabelStartwerte.AutoSize = True
        Me.LabelStartwerte.Location = New System.Drawing.Point(2, 64)
        Me.LabelStartwerte.Name = "LabelStartwerte"
        Me.LabelStartwerte.Size = New System.Drawing.Size(58, 13)
        Me.LabelStartwerte.TabIndex = 29
        Me.LabelStartwerte.Text = "Startwerte:"
        '
        'ComboOptStartparameter
        '
        Me.ComboOptStartparameter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptStartparameter.Location = New System.Drawing.Point(80, 61)
        Me.ComboOptStartparameter.Name = "ComboOptStartparameter"
        Me.ComboOptStartparameter.Size = New System.Drawing.Size(123, 21)
        Me.ComboOptStartparameter.TabIndex = 1
        '
        'LabelMutation
        '
        Me.LabelMutation.AutoSize = True
        Me.LabelMutation.Location = New System.Drawing.Point(2, 91)
        Me.LabelMutation.Name = "LabelMutation"
        Me.LabelMutation.Size = New System.Drawing.Size(51, 13)
        Me.LabelMutation.TabIndex = 34
        Me.LabelMutation.Text = "Mutation:"
        '
        'ComboOptDnMutation
        '
        Me.ComboOptDnMutation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptDnMutation.FormattingEnabled = True
        Me.ComboOptDnMutation.Location = New System.Drawing.Point(80, 88)
        Me.ComboOptDnMutation.Name = "ComboOptDnMutation"
        Me.ComboOptDnMutation.Size = New System.Drawing.Size(123, 21)
        Me.ComboOptDnMutation.TabIndex = 35
        '
        'LabelStartSchrittweite
        '
        Me.LabelStartSchrittweite.AutoSize = True
        Me.LabelStartSchrittweite.Location = New System.Drawing.Point(2, 115)
        Me.LabelStartSchrittweite.Name = "LabelStartSchrittweite"
        Me.LabelStartSchrittweite.Size = New System.Drawing.Size(90, 13)
        Me.LabelStartSchrittweite.TabIndex = 27
        Me.LabelStartSchrittweite.Text = "Start-Schrittweite:"
        '
        'TextDeltaStart
        '
        Me.TextDeltaStart.DecimalPlaces = 2
        Me.TextDeltaStart.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.TextDeltaStart.Location = New System.Drawing.Point(151, 115)
        Me.TextDeltaStart.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TextDeltaStart.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.TextDeltaStart.Name = "TextDeltaStart"
        Me.TextDeltaStart.Size = New System.Drawing.Size(53, 20)
        Me.TextDeltaStart.TabIndex = 2
        Me.TextDeltaStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextDeltaStart.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'CheckisDnVektor
        '
        Me.CheckisDnVektor.Location = New System.Drawing.Point(5, 141)
        Me.CheckisDnVektor.Name = "CheckisDnVektor"
        Me.CheckisDnVektor.Size = New System.Drawing.Size(144, 18)
        Me.CheckisDnVektor.TabIndex = 3
        Me.CheckisDnVektor.Text = "mit Schrittweitenvektor"
        '
        'GroupBox_Generationen
        '
        Me.GroupBox_Generationen.Controls.Add(Me.CheckisTournamentSelection)
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
        Me.GroupBox_Generationen.Location = New System.Drawing.Point(5, 165)
        Me.GroupBox_Generationen.Name = "GroupBox_Generationen"
        Me.GroupBox_Generationen.Size = New System.Drawing.Size(199, 247)
        Me.GroupBox_Generationen.TabIndex = 4
        Me.GroupBox_Generationen.TabStop = False
        Me.GroupBox_Generationen.Text = "Generationen:"
        '
        'CheckisTournamentSelection
        '
        Me.CheckisTournamentSelection.AutoSize = True
        Me.CheckisTournamentSelection.Location = New System.Drawing.Point(10, 218)
        Me.CheckisTournamentSelection.Name = "CheckisTournamentSelection"
        Me.CheckisTournamentSelection.Size = New System.Drawing.Size(130, 17)
        Me.CheckisTournamentSelection.TabIndex = 43
        Me.CheckisTournamentSelection.Text = "Tournament Selection"
        Me.CheckisTournamentSelection.UseVisualStyleBackColor = True
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
        Me.CheckisPopul.Location = New System.Drawing.Point(5, 418)
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
        Me.GroupBox_Populationen.Controls.Add(Me.ComboOptPopStrategie)
        Me.GroupBox_Populationen.Controls.Add(Me.LabelPopPenalty)
        Me.GroupBox_Populationen.Controls.Add(Me.ComboOptPopPenalty)
        Me.GroupBox_Populationen.Enabled = False
        Me.GroupBox_Populationen.Location = New System.Drawing.Point(5, 442)
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
        'ComboOptPopStrategie
        '
        Me.ComboOptPopStrategie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptPopStrategie.Location = New System.Drawing.Point(85, 108)
        Me.ComboOptPopStrategie.Name = "ComboOptPopStrategie"
        Me.ComboOptPopStrategie.Size = New System.Drawing.Size(108, 21)
        Me.ComboOptPopStrategie.TabIndex = 4
        '
        'LabelPopPenalty
        '
        Me.LabelPopPenalty.Location = New System.Drawing.Point(8, 127)
        Me.LabelPopPenalty.Name = "LabelPopPenalty"
        Me.LabelPopPenalty.Size = New System.Drawing.Size(76, 29)
        Me.LabelPopPenalty.TabIndex = 36
        Me.LabelPopPenalty.Text = "Ermittlung der Pop-Güte:"
        '
        'ComboOptPopPenalty
        '
        Me.ComboOptPopPenalty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptPopPenalty.Location = New System.Drawing.Point(85, 132)
        Me.ComboOptPopPenalty.Name = "ComboOptPopPenalty"
        Me.ComboOptPopPenalty.Size = New System.Drawing.Size(108, 21)
        Me.ComboOptPopPenalty.TabIndex = 5
        '
        'checkpaintconstrained
        '
        Me.checkpaintconstrained.AutoSize = True
        Me.checkpaintconstrained.Location = New System.Drawing.Point(4, 606)
        Me.checkpaintconstrained.Name = "checkpaintconstrained"
        Me.checkpaintconstrained.Size = New System.Drawing.Size(133, 17)
        Me.checkpaintconstrained.TabIndex = 33
        Me.checkpaintconstrained.Text = "Reduzierte Darstellung"
        Me.ToolTip1.SetToolTip(Me.checkpaintconstrained, "Nur die Individuuen der aktuellen Generation werden gezeichnet")
        Me.checkpaintconstrained.UseVisualStyleBackColor = True
        '
        'TabPage_CES
        '
        Me.TabPage_CES.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage_CES.Controls.Add(Me.GroupBox_Hybrid)
        Me.TabPage_CES.Controls.Add(Me.CheckBox_Use_Sec_Pop)
        Me.TabPage_CES.Controls.Add(Me.GroupBoxCES)
        Me.TabPage_CES.Controls.Add(Me.ComboCESIniValues)
        Me.TabPage_CES.Controls.Add(Me.LabelCESIniValues)
        Me.TabPage_CES.Controls.Add(Me.Label_CES_OptModus)
        Me.TabPage_CES.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_CES.Name = "TabPage_CES"
        Me.TabPage_CES.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_CES.Size = New System.Drawing.Size(210, 630)
        Me.TabPage_CES.TabIndex = 1
        Me.TabPage_CES.Text = "CES"
        '
        'GroupBox_Hybrid
        '
        Me.GroupBox_Hybrid.Controls.Add(Me.CheckBox_use_sec_pop_PES)
        Me.GroupBox_Hybrid.Controls.Add(Me.Label10)
        Me.GroupBox_Hybrid.Controls.Add(Me.CheckBox2)
        Me.GroupBox_Hybrid.Controls.Add(Me.Numeric_n_member_SecPop_PES)
        Me.GroupBox_Hybrid.Controls.Add(Me.Numeric_max_PES_Parents)
        Me.GroupBox_Hybrid.Controls.Add(Me.Numeric_n_exchange_SecPop_PES)
        Me.GroupBox_Hybrid.Controls.Add(Me.Label_max_PES_Parents)
        Me.GroupBox_Hybrid.Controls.Add(Me.Label_n_memebers_SecPop_PES)
        Me.GroupBox_Hybrid.Controls.Add(Me.Numeric_mem_Strength)
        Me.GroupBox_Hybrid.Controls.Add(Me.Label_n_exchange_secPop_PES)
        Me.GroupBox_Hybrid.Controls.Add(Me.Label12)
        Me.GroupBox_Hybrid.Controls.Add(Me.ComboBox_Hybrid_Type)
        Me.GroupBox_Hybrid.Controls.Add(Me.Label_MemStrength)
        Me.GroupBox_Hybrid.Controls.Add(Me.Label_Hybrid_Type)
        Me.GroupBox_Hybrid.Location = New System.Drawing.Point(6, 407)
        Me.GroupBox_Hybrid.Name = "GroupBox_Hybrid"
        Me.GroupBox_Hybrid.Size = New System.Drawing.Size(199, 217)
        Me.GroupBox_Hybrid.TabIndex = 12
        Me.GroupBox_Hybrid.TabStop = False
        Me.GroupBox_Hybrid.Text = "Hybrid Options"
        '
        'CheckBox_use_sec_pop_PES
        '
        Me.CheckBox_use_sec_pop_PES.AutoSize = True
        Me.CheckBox_use_sec_pop_PES.Location = New System.Drawing.Point(17, 138)
        Me.CheckBox_use_sec_pop_PES.Name = "CheckBox_use_sec_pop_PES"
        Me.CheckBox_use_sec_pop_PES.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.CheckBox_use_sec_pop_PES.Size = New System.Drawing.Size(153, 17)
        Me.CheckBox_use_sec_pop_PES.TabIndex = 13
        Me.CheckBox_use_sec_pop_PES.Text = "Use secondary Population:"
        Me.CheckBox_use_sec_pop_PES.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(4, 103)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(128, 16)
        Me.Label10.TabIndex = 55
        Me.Label10.Text = "Start with Pop Mutation:"
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(177, 103)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.CheckBox2.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox2.TabIndex = 13
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'Numeric_n_member_SecPop_PES
        '
        Me.Numeric_n_member_SecPop_PES.Location = New System.Drawing.Point(139, 187)
        Me.Numeric_n_member_SecPop_PES.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.Numeric_n_member_SecPop_PES.Name = "Numeric_n_member_SecPop_PES"
        Me.Numeric_n_member_SecPop_PES.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_n_member_SecPop_PES.TabIndex = 52
        Me.Numeric_n_member_SecPop_PES.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_n_member_SecPop_PES.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'Numeric_max_PES_Parents
        '
        Me.Numeric_max_PES_Parents.Location = New System.Drawing.Point(140, 77)
        Me.Numeric_max_PES_Parents.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
        Me.Numeric_max_PES_Parents.Name = "Numeric_max_PES_Parents"
        Me.Numeric_max_PES_Parents.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_max_PES_Parents.TabIndex = 53
        Me.Numeric_max_PES_Parents.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_max_PES_Parents.Value = New Decimal(New Integer() {12, 0, 0, 0})
        '
        'Numeric_n_exchange_SecPop_PES
        '
        Me.Numeric_n_exchange_SecPop_PES.Location = New System.Drawing.Point(139, 161)
        Me.Numeric_n_exchange_SecPop_PES.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.Numeric_n_exchange_SecPop_PES.Name = "Numeric_n_exchange_SecPop_PES"
        Me.Numeric_n_exchange_SecPop_PES.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_n_exchange_SecPop_PES.TabIndex = 51
        Me.Numeric_n_exchange_SecPop_PES.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_n_exchange_SecPop_PES.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'Label_max_PES_Parents
        '
        Me.Label_max_PES_Parents.AutoSize = True
        Me.Label_max_PES_Parents.Location = New System.Drawing.Point(3, 79)
        Me.Label_max_PES_Parents.Name = "Label_max_PES_Parents"
        Me.Label_max_PES_Parents.Size = New System.Drawing.Size(116, 13)
        Me.Label_max_PES_Parents.TabIndex = 52
        Me.Label_max_PES_Parents.Text = "Maximal No of Parents:"
        '
        'Label_n_memebers_SecPop_PES
        '
        Me.Label_n_memebers_SecPop_PES.Location = New System.Drawing.Point(3, 189)
        Me.Label_n_memebers_SecPop_PES.Name = "Label_n_memebers_SecPop_PES"
        Me.Label_n_memebers_SecPop_PES.Size = New System.Drawing.Size(128, 16)
        Me.Label_n_memebers_SecPop_PES.TabIndex = 54
        Me.Label_n_memebers_SecPop_PES.Text = "No of members SecPop:"
        '
        'Numeric_mem_Strength
        '
        Me.Numeric_mem_Strength.Location = New System.Drawing.Point(140, 51)
        Me.Numeric_mem_Strength.Maximum = New Decimal(New Integer() {3, 0, 0, 0})
        Me.Numeric_mem_Strength.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.Numeric_mem_Strength.Name = "Numeric_mem_Strength"
        Me.Numeric_mem_Strength.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_mem_Strength.TabIndex = 51
        Me.Numeric_mem_Strength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_mem_Strength.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label_n_exchange_secPop_PES
        '
        Me.Label_n_exchange_secPop_PES.AutoSize = True
        Me.Label_n_exchange_secPop_PES.Location = New System.Drawing.Point(3, 163)
        Me.Label_n_exchange_secPop_PES.Name = "Label_n_exchange_secPop_PES"
        Me.Label_n_exchange_secPop_PES.Size = New System.Drawing.Size(121, 13)
        Me.Label_n_exchange_secPop_PES.TabIndex = 53
        Me.Label_n_exchange_secPop_PES.Text = "Exchange with SecPop:"
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.SystemColors.WindowText
        Me.Label12.Location = New System.Drawing.Point(15, 128)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(165, 1)
        Me.Label12.TabIndex = 50
        '
        'ComboBox_Hybrid_Type
        '
        Me.ComboBox_Hybrid_Type.FormattingEnabled = True
        Me.ComboBox_Hybrid_Type.Location = New System.Drawing.Point(69, 24)
        Me.ComboBox_Hybrid_Type.Name = "ComboBox_Hybrid_Type"
        Me.ComboBox_Hybrid_Type.Size = New System.Drawing.Size(123, 21)
        Me.ComboBox_Hybrid_Type.TabIndex = 13
        '
        'Label_MemStrength
        '
        Me.Label_MemStrength.AutoSize = True
        Me.Label_MemStrength.Location = New System.Drawing.Point(3, 53)
        Me.Label_MemStrength.Name = "Label_MemStrength"
        Me.Label_MemStrength.Size = New System.Drawing.Size(90, 13)
        Me.Label_MemStrength.TabIndex = 50
        Me.Label_MemStrength.Text = "Memory Strength:"
        '
        'Label_Hybrid_Type
        '
        Me.Label_Hybrid_Type.AutoSize = True
        Me.Label_Hybrid_Type.Location = New System.Drawing.Point(3, 27)
        Me.Label_Hybrid_Type.Name = "Label_Hybrid_Type"
        Me.Label_Hybrid_Type.Size = New System.Drawing.Size(67, 13)
        Me.Label_Hybrid_Type.TabIndex = 0
        Me.Label_Hybrid_Type.Text = "Hybrid Type:"
        '
        'CheckBox_Use_Sec_Pop
        '
        Me.CheckBox_Use_Sec_Pop.AutoSize = True
        Me.CheckBox_Use_Sec_Pop.Location = New System.Drawing.Point(22, 384)
        Me.CheckBox_Use_Sec_Pop.Name = "CheckBox_Use_Sec_Pop"
        Me.CheckBox_Use_Sec_Pop.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.CheckBox_Use_Sec_Pop.Size = New System.Drawing.Size(157, 17)
        Me.CheckBox_Use_Sec_Pop.TabIndex = 11
        Me.CheckBox_Use_Sec_Pop.Text = "Including Real Optimisation:"
        Me.CheckBox_Use_Sec_Pop.UseVisualStyleBackColor = True
        '
        'GroupBoxCES
        '
        Me.GroupBoxCES.Controls.Add(Me.Use_)
        Me.GroupBoxCES.Controls.Add(Me.Numeric_MutRate)
        Me.GroupBoxCES.Controls.Add(Me.Label_MutationRate)
        Me.GroupBoxCES.Controls.Add(Me.ComboCESReproduction)
        Me.GroupBoxCES.Controls.Add(Me.LabelCESReproduction)
        Me.GroupBoxCES.Controls.Add(Me.Numeric_n_member_SecPop)
        Me.GroupBoxCES.Controls.Add(Me.Numeric_n_exchange_SecPop)
        Me.GroupBoxCES.Controls.Add(Me.Label_n_memebers_SecPop)
        Me.GroupBoxCES.Controls.Add(Me.Label_n_exchange_secPop)
        Me.GroupBoxCES.Controls.Add(Me.Numeric_n_childs)
        Me.GroupBoxCES.Controls.Add(Me.Numeric_n_Parents)
        Me.GroupBoxCES.Controls.Add(Me.ComboCESMutation)
        Me.GroupBoxCES.Controls.Add(Me.Numeric_n_Generations)
        Me.GroupBoxCES.Controls.Add(Me.ComboCESSelection)
        Me.GroupBoxCES.Controls.Add(Me.Label_n_childs)
        Me.GroupBoxCES.Controls.Add(Me.Label_n_parents)
        Me.GroupBoxCES.Controls.Add(Me.Label_n_Generations)
        Me.GroupBoxCES.Controls.Add(Me.LabelCESMutation)
        Me.GroupBoxCES.Controls.Add(Me.LabelCESSelection)
        Me.GroupBoxCES.Location = New System.Drawing.Point(5, 69)
        Me.GroupBoxCES.Name = "GroupBoxCES"
        Me.GroupBoxCES.Size = New System.Drawing.Size(200, 295)
        Me.GroupBoxCES.TabIndex = 9
        Me.GroupBoxCES.TabStop = False
        Me.GroupBoxCES.Text = "Mixed Integer Evolution Strategy"
        '
        'Use_
        '
        Me.Use_.AutoSize = True
        Me.Use_.Checked = True
        Me.Use_.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Use_.Location = New System.Drawing.Point(17, 218)
        Me.Use_.Name = "Use_"
        Me.Use_.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Use_.Size = New System.Drawing.Size(153, 17)
        Me.Use_.TabIndex = 56
        Me.Use_.Text = "Use secondary Population:"
        Me.Use_.UseVisualStyleBackColor = True
        '
        'Numeric_MutRate
        '
        Me.Numeric_MutRate.Increment = New Decimal(New Integer() {5, 0, 0, 0})
        Me.Numeric_MutRate.Location = New System.Drawing.Point(139, 178)
        Me.Numeric_MutRate.Name = "Numeric_MutRate"
        Me.Numeric_MutRate.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_MutRate.TabIndex = 49
        Me.Numeric_MutRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_MutRate.Value = New Decimal(New Integer() {25, 0, 0, 0})
        '
        'Label_MutationRate
        '
        Me.Label_MutationRate.AutoSize = True
        Me.Label_MutationRate.Location = New System.Drawing.Point(3, 181)
        Me.Label_MutationRate.Name = "Label_MutationRate"
        Me.Label_MutationRate.Size = New System.Drawing.Size(77, 13)
        Me.Label_MutationRate.TabIndex = 48
        Me.Label_MutationRate.Text = "Mutation Rate:"
        '
        'ComboCESReproduction
        '
        Me.ComboCESReproduction.FormattingEnabled = True
        Me.ComboCESReproduction.Location = New System.Drawing.Point(69, 124)
        Me.ComboCESReproduction.Name = "ComboCESReproduction"
        Me.ComboCESReproduction.Size = New System.Drawing.Size(123, 21)
        Me.ComboCESReproduction.TabIndex = 8
        '
        'LabelCESReproduction
        '
        Me.LabelCESReproduction.AutoSize = True
        Me.LabelCESReproduction.Location = New System.Drawing.Point(3, 127)
        Me.LabelCESReproduction.Name = "LabelCESReproduction"
        Me.LabelCESReproduction.Size = New System.Drawing.Size(63, 13)
        Me.LabelCESReproduction.TabIndex = 4
        Me.LabelCESReproduction.Text = "Reproduct.:"
        '
        'Numeric_n_member_SecPop
        '
        Me.Numeric_n_member_SecPop.Location = New System.Drawing.Point(139, 267)
        Me.Numeric_n_member_SecPop.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.Numeric_n_member_SecPop.Name = "Numeric_n_member_SecPop"
        Me.Numeric_n_member_SecPop.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_n_member_SecPop.TabIndex = 45
        Me.Numeric_n_member_SecPop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_n_member_SecPop.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'Numeric_n_exchange_SecPop
        '
        Me.Numeric_n_exchange_SecPop.Location = New System.Drawing.Point(139, 241)
        Me.Numeric_n_exchange_SecPop.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.Numeric_n_exchange_SecPop.Name = "Numeric_n_exchange_SecPop"
        Me.Numeric_n_exchange_SecPop.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_n_exchange_SecPop.TabIndex = 44
        Me.Numeric_n_exchange_SecPop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_n_exchange_SecPop.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'Label_n_memebers_SecPop
        '
        Me.Label_n_memebers_SecPop.Location = New System.Drawing.Point(3, 269)
        Me.Label_n_memebers_SecPop.Name = "Label_n_memebers_SecPop"
        Me.Label_n_memebers_SecPop.Size = New System.Drawing.Size(128, 16)
        Me.Label_n_memebers_SecPop.TabIndex = 47
        Me.Label_n_memebers_SecPop.Text = "No of members SecPop:"
        '
        'Label_n_exchange_secPop
        '
        Me.Label_n_exchange_secPop.AutoSize = True
        Me.Label_n_exchange_secPop.Location = New System.Drawing.Point(3, 243)
        Me.Label_n_exchange_secPop.Name = "Label_n_exchange_secPop"
        Me.Label_n_exchange_secPop.Size = New System.Drawing.Size(121, 13)
        Me.Label_n_exchange_secPop.TabIndex = 46
        Me.Label_n_exchange_secPop.Text = "Exchange with SecPop:"
        '
        'Numeric_n_childs
        '
        Me.Numeric_n_childs.Location = New System.Drawing.Point(139, 72)
        Me.Numeric_n_childs.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.Numeric_n_childs.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.Numeric_n_childs.Name = "Numeric_n_childs"
        Me.Numeric_n_childs.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_n_childs.TabIndex = 5
        Me.Numeric_n_childs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_n_childs.Value = New Decimal(New Integer() {15, 0, 0, 0})
        '
        'Numeric_n_Parents
        '
        Me.Numeric_n_Parents.Location = New System.Drawing.Point(139, 46)
        Me.Numeric_n_Parents.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.Numeric_n_Parents.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.Numeric_n_Parents.Name = "Numeric_n_Parents"
        Me.Numeric_n_Parents.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_n_Parents.TabIndex = 4
        Me.Numeric_n_Parents.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_n_Parents.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'ComboCESMutation
        '
        Me.ComboCESMutation.FormattingEnabled = True
        Me.ComboCESMutation.Location = New System.Drawing.Point(69, 151)
        Me.ComboCESMutation.Name = "ComboCESMutation"
        Me.ComboCESMutation.Size = New System.Drawing.Size(123, 21)
        Me.ComboCESMutation.TabIndex = 7
        '
        'Numeric_n_Generations
        '
        Me.Numeric_n_Generations.Location = New System.Drawing.Point(139, 20)
        Me.Numeric_n_Generations.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.Numeric_n_Generations.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.Numeric_n_Generations.Name = "Numeric_n_Generations"
        Me.Numeric_n_Generations.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_n_Generations.TabIndex = 3
        Me.Numeric_n_Generations.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_n_Generations.Value = New Decimal(New Integer() {500, 0, 0, 0})
        '
        'ComboCESSelection
        '
        Me.ComboCESSelection.FormattingEnabled = True
        Me.ComboCESSelection.Location = New System.Drawing.Point(69, 97)
        Me.ComboCESSelection.Name = "ComboCESSelection"
        Me.ComboCESSelection.Size = New System.Drawing.Size(123, 21)
        Me.ComboCESSelection.TabIndex = 6
        '
        'Label_n_childs
        '
        Me.Label_n_childs.AutoSize = True
        Me.Label_n_childs.Location = New System.Drawing.Point(2, 74)
        Me.Label_n_childs.Name = "Label_n_childs"
        Me.Label_n_childs.Size = New System.Drawing.Size(90, 13)
        Me.Label_n_childs.TabIndex = 2
        Me.Label_n_childs.Text = "Number of Childs:"
        '
        'Label_n_parents
        '
        Me.Label_n_parents.AutoSize = True
        Me.Label_n_parents.Location = New System.Drawing.Point(2, 48)
        Me.Label_n_parents.Name = "Label_n_parents"
        Me.Label_n_parents.Size = New System.Drawing.Size(98, 13)
        Me.Label_n_parents.TabIndex = 1
        Me.Label_n_parents.Text = "Number of Parents:"
        '
        'Label_n_Generations
        '
        Me.Label_n_Generations.AutoSize = True
        Me.Label_n_Generations.Location = New System.Drawing.Point(2, 22)
        Me.Label_n_Generations.Name = "Label_n_Generations"
        Me.Label_n_Generations.Size = New System.Drawing.Size(119, 13)
        Me.Label_n_Generations.TabIndex = 0
        Me.Label_n_Generations.Text = "Number of Generations:"
        '
        'LabelCESMutation
        '
        Me.LabelCESMutation.AutoSize = True
        Me.LabelCESMutation.Location = New System.Drawing.Point(3, 154)
        Me.LabelCESMutation.Name = "LabelCESMutation"
        Me.LabelCESMutation.Size = New System.Drawing.Size(51, 13)
        Me.LabelCESMutation.TabIndex = 3
        Me.LabelCESMutation.Text = "Mutation:"
        '
        'LabelCESSelection
        '
        Me.LabelCESSelection.AutoSize = True
        Me.LabelCESSelection.Location = New System.Drawing.Point(2, 100)
        Me.LabelCESSelection.Name = "LabelCESSelection"
        Me.LabelCESSelection.Size = New System.Drawing.Size(54, 13)
        Me.LabelCESSelection.TabIndex = 1
        Me.LabelCESSelection.Text = "Selection:"
        '
        'ComboCESIniValues
        '
        Me.ComboCESIniValues.FormattingEnabled = True
        Me.ComboCESIniValues.Location = New System.Drawing.Point(80, 32)
        Me.ComboCESIniValues.Name = "ComboCESIniValues"
        Me.ComboCESIniValues.Size = New System.Drawing.Size(123, 21)
        Me.ComboCESIniValues.TabIndex = 5
        '
        'LabelCESIniValues
        '
        Me.LabelCESIniValues.AutoSize = True
        Me.LabelCESIniValues.Location = New System.Drawing.Point(2, 35)
        Me.LabelCESIniValues.Name = "LabelCESIniValues"
        Me.LabelCESIniValues.Size = New System.Drawing.Size(68, 13)
        Me.LabelCESIniValues.TabIndex = 2
        Me.LabelCESIniValues.Text = "Initial values:"
        '
        'Label_CES_OptModus
        '
        Me.Label_CES_OptModus.AutoSize = True
        Me.Label_CES_OptModus.Location = New System.Drawing.Point(2, 9)
        Me.Label_CES_OptModus.Name = "Label_CES_OptModus"
        Me.Label_CES_OptModus.Size = New System.Drawing.Size(42, 13)
        Me.Label_CES_OptModus.TabIndex = 0
        Me.Label_CES_OptModus.Text = "Modus:"
        '
        'TabPage_HookeJeeves
        '
        Me.TabPage_HookeJeeves.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage_HookeJeeves.Controls.Add(Me.LabelRSHJ)
        Me.TabPage_HookeJeeves.Controls.Add(Me.LabelESHJ)
        Me.TabPage_HookeJeeves.Controls.Add(Me.LabelTSHJgesamt)
        Me.TabPage_HookeJeeves.Controls.Add(Me.LabelTSHJmittel)
        Me.TabPage_HookeJeeves.Controls.Add(Me.Label9)
        Me.TabPage_HookeJeeves.Controls.Add(Me.Label8)
        Me.TabPage_HookeJeeves.Controls.Add(Me.Label7)
        Me.TabPage_HookeJeeves.Controls.Add(Me.Label6)
        Me.TabPage_HookeJeeves.Controls.Add(Me.LabelTSHJaktuelle)
        Me.TabPage_HookeJeeves.Controls.Add(Me.Label4)
        Me.TabPage_HookeJeeves.Controls.Add(Me.Label3)
        Me.TabPage_HookeJeeves.Controls.Add(Me.TextDeltaFinishHJ)
        Me.TabPage_HookeJeeves.Controls.Add(Me.Label1)
        Me.TabPage_HookeJeeves.Controls.Add(Me.TextDeltaStartHJ)
        Me.TabPage_HookeJeeves.Controls.Add(Me.CheckBoxDNVektorHJ)
        Me.TabPage_HookeJeeves.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_HookeJeeves.Name = "TabPage_HookeJeeves"
        Me.TabPage_HookeJeeves.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_HookeJeeves.Size = New System.Drawing.Size(210, 630)
        Me.TabPage_HookeJeeves.TabIndex = 2
        Me.TabPage_HookeJeeves.Text = "Hooke & Jeeves"
        '
        'LabelRSHJ
        '
        Me.LabelRSHJ.AutoSize = True
        Me.LabelRSHJ.ForeColor = System.Drawing.Color.Blue
        Me.LabelRSHJ.Location = New System.Drawing.Point(139, 223)
        Me.LabelRSHJ.Name = "LabelRSHJ"
        Me.LabelRSHJ.Size = New System.Drawing.Size(13, 13)
        Me.LabelRSHJ.TabIndex = 42
        Me.LabelRSHJ.Text = "0"
        Me.LabelRSHJ.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LabelESHJ
        '
        Me.LabelESHJ.AutoSize = True
        Me.LabelESHJ.ForeColor = System.Drawing.Color.Blue
        Me.LabelESHJ.Location = New System.Drawing.Point(139, 199)
        Me.LabelESHJ.Name = "LabelESHJ"
        Me.LabelESHJ.Size = New System.Drawing.Size(13, 13)
        Me.LabelESHJ.TabIndex = 41
        Me.LabelESHJ.Text = "0"
        Me.LabelESHJ.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LabelTSHJgesamt
        '
        Me.LabelTSHJgesamt.AutoSize = True
        Me.LabelTSHJgesamt.ForeColor = System.Drawing.Color.Blue
        Me.LabelTSHJgesamt.Location = New System.Drawing.Point(139, 175)
        Me.LabelTSHJgesamt.Name = "LabelTSHJgesamt"
        Me.LabelTSHJgesamt.Size = New System.Drawing.Size(13, 13)
        Me.LabelTSHJgesamt.TabIndex = 40
        Me.LabelTSHJgesamt.Text = "0"
        Me.LabelTSHJgesamt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LabelTSHJmittel
        '
        Me.LabelTSHJmittel.AutoSize = True
        Me.LabelTSHJmittel.ForeColor = System.Drawing.Color.Blue
        Me.LabelTSHJmittel.Location = New System.Drawing.Point(139, 151)
        Me.LabelTSHJmittel.Name = "LabelTSHJmittel"
        Me.LabelTSHJmittel.Size = New System.Drawing.Size(13, 13)
        Me.LabelTSHJmittel.TabIndex = 39
        Me.LabelTSHJmittel.Text = "0"
        Me.LabelTSHJmittel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.Color.Blue
        Me.Label9.Location = New System.Drawing.Point(6, 223)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(67, 13)
        Me.Label9.TabIndex = 38
        Me.Label9.Text = "Rückschritte"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(6, 199)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(107, 13)
        Me.Label8.TabIndex = 37
        Me.Label8.Text = "Extrapolationsschritte"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.Color.Blue
        Me.Label7.Location = New System.Drawing.Point(6, 175)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(99, 13)
        Me.Label7.TabIndex = 36
        Me.Label7.Text = "Tastschritte gesamt"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.Color.Blue
        Me.Label6.Location = New System.Drawing.Point(6, 151)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(89, 13)
        Me.Label6.TabIndex = 35
        Me.Label6.Text = "Tastschritte mittel"
        '
        'LabelTSHJaktuelle
        '
        Me.LabelTSHJaktuelle.AutoSize = True
        Me.LabelTSHJaktuelle.ForeColor = System.Drawing.Color.Blue
        Me.LabelTSHJaktuelle.Location = New System.Drawing.Point(139, 127)
        Me.LabelTSHJaktuelle.Name = "LabelTSHJaktuelle"
        Me.LabelTSHJaktuelle.Size = New System.Drawing.Size(13, 13)
        Me.LabelTSHJaktuelle.TabIndex = 34
        Me.LabelTSHJaktuelle.Text = "0"
        Me.LabelTSHJaktuelle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(6, 127)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(96, 13)
        Me.Label4.TabIndex = 33
        Me.Label4.Text = "Tastschritte aktuell"
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
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ÖffnenToolStripButton, Me.SpeichernToolStripButton})
        Me.ToolStrip1.Location = New System.Drawing.Point(3, 16)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Padding = New System.Windows.Forms.Padding(0)
        Me.ToolStrip1.Size = New System.Drawing.Size(214, 25)
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
        'GroupBox_Einstellungen
        '
        Me.GroupBox_Einstellungen.Controls.Add(Me.ToolStrip1)
        Me.GroupBox_Einstellungen.Controls.Add(Me.TabControl1)
        Me.GroupBox_Einstellungen.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox_Einstellungen.Name = "GroupBox_Einstellungen"
        Me.GroupBox_Einstellungen.Size = New System.Drawing.Size(220, 700)
        Me.GroupBox_Einstellungen.TabIndex = 37
        Me.GroupBox_Einstellungen.TabStop = False
        Me.GroupBox_Einstellungen.Text = "Einstellungen:"
        '
        'EVO_Einstellungen
        '
        Me.Controls.Add(Me.GroupBox_Einstellungen)
        Me.Name = "EVO_Einstellungen"
        Me.Size = New System.Drawing.Size(236, 738)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage_PES.ResumeLayout(False)
        Me.TabPage_PES.PerformLayout()
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
        Me.TabPage_CES.ResumeLayout(False)
        Me.TabPage_CES.PerformLayout()
        Me.GroupBox_Hybrid.ResumeLayout(False)
        Me.GroupBox_Hybrid.PerformLayout()
        CType(Me.Numeric_n_member_SecPop_PES, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_max_PES_Parents, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_n_exchange_SecPop_PES, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_mem_Strength, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxCES.ResumeLayout(False)
        Me.GroupBoxCES.PerformLayout()
        CType(Me.Numeric_MutRate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_n_member_SecPop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_n_exchange_SecPop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_n_childs, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_n_Parents, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_n_Generations, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage_HookeJeeves.ResumeLayout(False)
        Me.TabPage_HookeJeeves.PerformLayout()
        CType(Me.TextDeltaFinishHJ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextDeltaStartHJ, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.GroupBox_Einstellungen.ResumeLayout(False)
        Me.GroupBox_Einstellungen.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Private WithEvents Label_OptModus As System.Windows.Forms.Label
    Private WithEvents Label_OptModusValue As System.Windows.Forms.Label
    Private WithEvents LabelStrategie As System.Windows.Forms.Label
    Private WithEvents ComboOptStrategie As System.Windows.Forms.ComboBox
    Private WithEvents LabelStartwerte As System.Windows.Forms.Label
    Private WithEvents ComboOptStartparameter As System.Windows.Forms.ComboBox
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
    Private WithEvents ComboOptPopStrategie As System.Windows.Forms.ComboBox
    Private WithEvents LabelPopPenalty As System.Windows.Forms.Label
    Private WithEvents ComboOptPopPenalty As System.Windows.Forms.ComboBox
    Private WithEvents TabPage_CES As System.Windows.Forms.TabPage
    Friend WithEvents ÖffnenToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents SpeichernToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents GroupBox_Einstellungen As System.Windows.Forms.GroupBox
    Friend WithEvents TabPage_HookeJeeves As System.Windows.Forms.TabPage
    Private WithEvents Label3 As System.Windows.Forms.Label
    Private WithEvents TextDeltaFinishHJ As System.Windows.Forms.NumericUpDown
    Private WithEvents Label1 As System.Windows.Forms.Label
    Private WithEvents TextDeltaStartHJ As System.Windows.Forms.NumericUpDown
    Private WithEvents CheckBoxDNVektorHJ As System.Windows.Forms.CheckBox
    Public WithEvents TabPage_PES As System.Windows.Forms.TabPage
    Public WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents LabelTSHJaktuelle As System.Windows.Forms.Label
    Public WithEvents LabelRSHJ As System.Windows.Forms.Label
    Public WithEvents LabelESHJ As System.Windows.Forms.Label
    Public WithEvents LabelTSHJgesamt As System.Windows.Forms.Label
    Public WithEvents LabelTSHJmittel As System.Windows.Forms.Label
    Friend WithEvents checkpaintconstrained As System.Windows.Forms.CheckBox
    Friend WithEvents CheckisTournamentSelection As System.Windows.Forms.CheckBox
    Friend WithEvents ComboOptDnMutation As System.Windows.Forms.ComboBox
    Private WithEvents LabelMutation As System.Windows.Forms.Label
    Friend WithEvents Label_CES_OptModus As System.Windows.Forms.Label
    Friend WithEvents LabelCESMutation As System.Windows.Forms.Label
    Friend WithEvents LabelCESIniValues As System.Windows.Forms.Label
    Friend WithEvents LabelCESSelection As System.Windows.Forms.Label
    Friend WithEvents LabelCESReproduction As System.Windows.Forms.Label
    Friend WithEvents ComboCESSelection As System.Windows.Forms.ComboBox
    Friend WithEvents ComboCESIniValues As System.Windows.Forms.ComboBox
    Friend WithEvents ComboCESReproduction As System.Windows.Forms.ComboBox
    Friend WithEvents ComboCESMutation As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBoxCES As System.Windows.Forms.GroupBox
    Friend WithEvents Label_n_childs As System.Windows.Forms.Label
    Friend WithEvents Label_n_parents As System.Windows.Forms.Label
    Friend WithEvents Label_n_Generations As System.Windows.Forms.Label
    Friend WithEvents Numeric_n_childs As System.Windows.Forms.NumericUpDown
    Friend WithEvents Numeric_n_Parents As System.Windows.Forms.NumericUpDown
    Friend WithEvents Numeric_n_Generations As System.Windows.Forms.NumericUpDown
    Private WithEvents Numeric_n_member_SecPop As System.Windows.Forms.NumericUpDown
    Private WithEvents Numeric_n_exchange_SecPop As System.Windows.Forms.NumericUpDown
    Private WithEvents Label_n_memebers_SecPop As System.Windows.Forms.Label
    Private WithEvents Label_n_exchange_secPop As System.Windows.Forms.Label
    Friend WithEvents CheckBox_Use_Sec_Pop As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox_Hybrid As System.Windows.Forms.GroupBox
    Private WithEvents Numeric_MutRate As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label_MutationRate As System.Windows.Forms.Label
    Friend WithEvents ComboBox_Hybrid_Type As System.Windows.Forms.ComboBox
    Friend WithEvents Label_Hybrid_Type As System.Windows.Forms.Label
    Friend WithEvents Numeric_mem_Strength As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label_MemStrength As System.Windows.Forms.Label
    Friend WithEvents Numeric_max_PES_Parents As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label_max_PES_Parents As System.Windows.Forms.Label
    Private WithEvents Numeric_n_member_SecPop_PES As System.Windows.Forms.NumericUpDown
    Private WithEvents Numeric_n_exchange_SecPop_PES As System.Windows.Forms.NumericUpDown
    Private WithEvents Label_n_memebers_SecPop_PES As System.Windows.Forms.Label
    Private WithEvents Label_n_exchange_secPop_PES As System.Windows.Forms.Label
    Private WithEvents Label12 As System.Windows.Forms.Label
    Private WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_use_sec_pop_PES As System.Windows.Forms.CheckBox
    Friend WithEvents Use_ As System.Windows.Forms.CheckBox
End Class
