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
        Dim Label_CES_MemSize As System.Windows.Forms.Label
        Dim Label_CES_NMembers_SecPop_PES As System.Windows.Forms.Label
        Dim Label_CES_NExchange_secPop_PES As System.Windows.Forms.Label
        Dim Label_CES_MemRank As System.Windows.Forms.Label
        Dim Label_CES_Hybrid_Type As System.Windows.Forms.Label
        Dim Label_CES_MutationRate As System.Windows.Forms.Label
        Dim Label_CES_Reproduction As System.Windows.Forms.Label
        Dim Label_CES_NExchangeSecPop As System.Windows.Forms.Label
        Dim Label_CES_NChilds As System.Windows.Forms.Label
        Dim Label_CES_NParents As System.Windows.Forms.Label
        Dim Label_CES_NGenerations As System.Windows.Forms.Label
        Dim Label_CES_Mutation As System.Windows.Forms.Label
        Dim Label_CES_Selection As System.Windows.Forms.Label
        Dim Label_CES_IniValues As System.Windows.Forms.Label
        Dim Label_OptModus As System.Windows.Forms.Label
        Dim LabelStrategie As System.Windows.Forms.Label
        Dim LabelStartwerte As System.Windows.Forms.Label
        Dim LabelMutation As System.Windows.Forms.Label
        Dim LabelStartSchrittweite As System.Windows.Forms.Label
        Dim LabelAnzNachf As System.Windows.Forms.Label
        Dim LabelAnzGen As System.Windows.Forms.Label
        Dim Label2 As System.Windows.Forms.Label
        Dim LabelInteract As System.Windows.Forms.Label
        Dim LabelAnzRunden As System.Windows.Forms.Label
        Dim LabelAnzPop As System.Windows.Forms.Label
        Dim LabelAnzPopEltern As System.Windows.Forms.Label
        Dim LabelOptPopEltern As System.Windows.Forms.Label
        Dim LabelPopStrategie As System.Windows.Forms.Label
        Dim LabelPopPenalty As System.Windows.Forms.Label
        Dim Label_Line As System.Windows.Forms.Label
        Dim Label9 As System.Windows.Forms.Label
        Dim Label8 As System.Windows.Forms.Label
        Dim Label7 As System.Windows.Forms.Label
        Dim Label6 As System.Windows.Forms.Label
        Dim Label4 As System.Windows.Forms.Label
        Dim Label3 As System.Windows.Forms.Label
        Dim Label1 As System.Windows.Forms.Label
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EVO_Einstellungen))
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage_PES = New System.Windows.Forms.TabPage
        Me.Label_OptModusValue = New System.Windows.Forms.Label
        Me.ComboOptStrategie = New System.Windows.Forms.ComboBox
        Me.ComboOptStartparameter = New System.Windows.Forms.ComboBox
        Me.ComboOptDnMutation = New System.Windows.Forms.ComboBox
        Me.TextDeltaStart = New System.Windows.Forms.NumericUpDown
        Me.CheckisDnVektor = New System.Windows.Forms.CheckBox
        Me.GroupBox_Generationen = New System.Windows.Forms.GroupBox
        Me.TextAnzNachf = New System.Windows.Forms.NumericUpDown
        Me.TextAnzEltern = New System.Windows.Forms.NumericUpDown
        Me.TextAnzGen = New System.Windows.Forms.NumericUpDown
        Me.LabelAnzEltern = New System.Windows.Forms.Label
        Me.GroupBox_Eltern = New System.Windows.Forms.GroupBox
        Me.ComboOptEltern = New System.Windows.Forms.ComboBox
        Me.CheckisTournamentSelection = New System.Windows.Forms.CheckBox
        Me.TextRekombxy = New System.Windows.Forms.NumericUpDown
        Me.LabelRekombxy1 = New System.Windows.Forms.Label
        Me.LabelRekombxy3 = New System.Windows.Forms.Label
        Me.GroupBox_SekPop = New System.Windows.Forms.GroupBox
        Me.CheckBox_isSekPopBegrenzung = New System.Windows.Forms.CheckBox
        Me.TextInteract = New System.Windows.Forms.NumericUpDown
        Me.TextMaxMemberSekPop = New System.Windows.Forms.NumericUpDown
        Me.LabelMaxMemberSekPop = New System.Windows.Forms.Label
        Me.CheckisPopul = New System.Windows.Forms.CheckBox
        Me.GroupBox_Populationen = New System.Windows.Forms.GroupBox
        Me.TextAnzRunden = New System.Windows.Forms.NumericUpDown
        Me.TextAnzPop = New System.Windows.Forms.NumericUpDown
        Me.TextAnzPopEltern = New System.Windows.Forms.NumericUpDown
        Me.ComboOptPopEltern = New System.Windows.Forms.ComboBox
        Me.ComboOptPopStrategie = New System.Windows.Forms.ComboBox
        Me.ComboOptPopPenalty = New System.Windows.Forms.ComboBox
        Me.TabPage_CES = New System.Windows.Forms.TabPage
        Me.CheckBox_CES_UseSecPop_CES = New System.Windows.Forms.CheckBox
        Me.GroupBox_CES_SecPop = New System.Windows.Forms.GroupBox
        Me.CheckBox_CES_isSecPopRestriction = New System.Windows.Forms.CheckBox
        Me.Label_CES_NMembersSecPop = New System.Windows.Forms.Label
        Me.Numeric_CES_n_exchange_SecPop = New System.Windows.Forms.NumericUpDown
        Me.Numeric_CES_n_member_SecPop = New System.Windows.Forms.NumericUpDown
        Me.GroupBox_CES_Hybrid = New System.Windows.Forms.GroupBox
        Me.Numeric_CES_n_MemSize = New System.Windows.Forms.NumericUpDown
        Me.Combo_CES_MemStrategy = New System.Windows.Forms.ComboBox
        Me.CheckBox_CES_UseSecPop_PES = New System.Windows.Forms.CheckBox
        Me.CheckBox_CES_StartPESPop = New System.Windows.Forms.CheckBox
        Me.Numeric_CES_n_member_SecPop_PES = New System.Windows.Forms.NumericUpDown
        Me.Numeric_CES_NExchange_SecPop_PES = New System.Windows.Forms.NumericUpDown
        Me.Combo_CES_HybridType = New System.Windows.Forms.ComboBox
        Me.CheckBox_CES_RealOptimisation = New System.Windows.Forms.CheckBox
        Me.GroupBoxCES = New System.Windows.Forms.GroupBox
        Me.Numeric_CES_MutRate = New System.Windows.Forms.NumericUpDown
        Me.Combo_CES_Reproduction = New System.Windows.Forms.ComboBox
        Me.Numeric_CES_n_childs = New System.Windows.Forms.NumericUpDown
        Me.Numeric_CES_n_Parents = New System.Windows.Forms.NumericUpDown
        Me.Combo_CES_Mutation = New System.Windows.Forms.ComboBox
        Me.Numeric_CES_n_Generations = New System.Windows.Forms.NumericUpDown
        Me.Combo_CES_Selection = New System.Windows.Forms.ComboBox
        Me.Combo_CES_IniValues = New System.Windows.Forms.ComboBox
        Me.Label_CES_OptModus = New System.Windows.Forms.Label
        Me.TabPage_HookeJeeves = New System.Windows.Forms.TabPage
        Me.Label_HJ_RS = New System.Windows.Forms.Label
        Me.Label_HJ_ES = New System.Windows.Forms.Label
        Me.Label_HJ_TSgesamt = New System.Windows.Forms.Label
        Me.Label_HJ_TSmittel = New System.Windows.Forms.Label
        Me.Label_HJ_TSaktuelle = New System.Windows.Forms.Label
        Me.Numeric_HJ_DeltaFinish = New System.Windows.Forms.NumericUpDown
        Me.Numeric_HJ_DeltaStart = New System.Windows.Forms.NumericUpDown
        Me.CheckBox_HJ_DNVektor = New System.Windows.Forms.CheckBox
        Me.TabPage_Hybrid2008 = New System.Windows.Forms.TabPage
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.ÖffnenToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.SpeichernToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox_Einstellungen = New System.Windows.Forms.GroupBox
        Label_CES_MemSize = New System.Windows.Forms.Label
        Label_CES_NMembers_SecPop_PES = New System.Windows.Forms.Label
        Label_CES_NExchange_secPop_PES = New System.Windows.Forms.Label
        Label_CES_MemRank = New System.Windows.Forms.Label
        Label_CES_Hybrid_Type = New System.Windows.Forms.Label
        Label_CES_MutationRate = New System.Windows.Forms.Label
        Label_CES_Reproduction = New System.Windows.Forms.Label
        Label_CES_NExchangeSecPop = New System.Windows.Forms.Label
        Label_CES_NChilds = New System.Windows.Forms.Label
        Label_CES_NParents = New System.Windows.Forms.Label
        Label_CES_NGenerations = New System.Windows.Forms.Label
        Label_CES_Mutation = New System.Windows.Forms.Label
        Label_CES_Selection = New System.Windows.Forms.Label
        Label_CES_IniValues = New System.Windows.Forms.Label
        Label_OptModus = New System.Windows.Forms.Label
        LabelStrategie = New System.Windows.Forms.Label
        LabelStartwerte = New System.Windows.Forms.Label
        LabelMutation = New System.Windows.Forms.Label
        LabelStartSchrittweite = New System.Windows.Forms.Label
        LabelAnzNachf = New System.Windows.Forms.Label
        LabelAnzGen = New System.Windows.Forms.Label
        Label2 = New System.Windows.Forms.Label
        LabelInteract = New System.Windows.Forms.Label
        LabelAnzRunden = New System.Windows.Forms.Label
        LabelAnzPop = New System.Windows.Forms.Label
        LabelAnzPopEltern = New System.Windows.Forms.Label
        LabelOptPopEltern = New System.Windows.Forms.Label
        LabelPopStrategie = New System.Windows.Forms.Label
        LabelPopPenalty = New System.Windows.Forms.Label
        Label_Line = New System.Windows.Forms.Label
        Label9 = New System.Windows.Forms.Label
        Label8 = New System.Windows.Forms.Label
        Label7 = New System.Windows.Forms.Label
        Label6 = New System.Windows.Forms.Label
        Label4 = New System.Windows.Forms.Label
        Label3 = New System.Windows.Forms.Label
        Label1 = New System.Windows.Forms.Label
        Me.TabControl1.SuspendLayout()
        Me.TabPage_PES.SuspendLayout()
        CType(Me.TextDeltaStart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_Generationen.SuspendLayout()
        CType(Me.TextAnzNachf, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextAnzEltern, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextAnzGen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_Eltern.SuspendLayout()
        CType(Me.TextRekombxy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_SekPop.SuspendLayout()
        CType(Me.TextInteract, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextMaxMemberSekPop, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_Populationen.SuspendLayout()
        CType(Me.TextAnzRunden, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextAnzPop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextAnzPopEltern, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage_CES.SuspendLayout()
        Me.GroupBox_CES_SecPop.SuspendLayout()
        CType(Me.Numeric_CES_n_exchange_SecPop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_CES_n_member_SecPop, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_CES_Hybrid.SuspendLayout()
        CType(Me.Numeric_CES_n_MemSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_CES_n_member_SecPop_PES, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_CES_NExchange_SecPop_PES, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxCES.SuspendLayout()
        CType(Me.Numeric_CES_MutRate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_CES_n_childs, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_CES_n_Parents, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_CES_n_Generations, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage_HookeJeeves.SuspendLayout()
        CType(Me.Numeric_HJ_DeltaFinish, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_HJ_DeltaStart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.GroupBox_Einstellungen.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label_CES_MemSize
        '
        Label_CES_MemSize.AutoSize = True
        Label_CES_MemSize.Location = New System.Drawing.Point(3, 79)
        Label_CES_MemSize.Name = "Label_CES_MemSize"
        Label_CES_MemSize.Size = New System.Drawing.Size(70, 13)
        Label_CES_MemSize.TabIndex = 57
        Label_CES_MemSize.Text = "Memory Size:"
        '
        'Label_CES_NMembers_SecPop_PES
        '
        Label_CES_NMembers_SecPop_PES.Location = New System.Drawing.Point(3, 189)
        Label_CES_NMembers_SecPop_PES.Name = "Label_CES_NMembers_SecPop_PES"
        Label_CES_NMembers_SecPop_PES.Size = New System.Drawing.Size(128, 16)
        Label_CES_NMembers_SecPop_PES.TabIndex = 54
        Label_CES_NMembers_SecPop_PES.Text = "No of members SecPop:"
        '
        'Label_CES_NExchange_secPop_PES
        '
        Label_CES_NExchange_secPop_PES.AutoSize = True
        Label_CES_NExchange_secPop_PES.Location = New System.Drawing.Point(3, 163)
        Label_CES_NExchange_secPop_PES.Name = "Label_CES_NExchange_secPop_PES"
        Label_CES_NExchange_secPop_PES.Size = New System.Drawing.Size(121, 13)
        Label_CES_NExchange_secPop_PES.TabIndex = 53
        Label_CES_NExchange_secPop_PES.Text = "Exchange with SecPop:"
        '
        'Label_CES_MemRank
        '
        Label_CES_MemRank.AutoSize = True
        Label_CES_MemRank.Location = New System.Drawing.Point(3, 53)
        Label_CES_MemRank.Name = "Label_CES_MemRank"
        Label_CES_MemRank.Size = New System.Drawing.Size(76, 13)
        Label_CES_MemRank.TabIndex = 50
        Label_CES_MemRank.Text = "Memory Rank:"
        '
        'Label_CES_Hybrid_Type
        '
        Label_CES_Hybrid_Type.AutoSize = True
        Label_CES_Hybrid_Type.Location = New System.Drawing.Point(3, 27)
        Label_CES_Hybrid_Type.Name = "Label_CES_Hybrid_Type"
        Label_CES_Hybrid_Type.Size = New System.Drawing.Size(67, 13)
        Label_CES_Hybrid_Type.TabIndex = 0
        Label_CES_Hybrid_Type.Text = "Hybrid Type:"
        '
        'Label_CES_MutationRate
        '
        Label_CES_MutationRate.AutoSize = True
        Label_CES_MutationRate.Location = New System.Drawing.Point(3, 181)
        Label_CES_MutationRate.Name = "Label_CES_MutationRate"
        Label_CES_MutationRate.Size = New System.Drawing.Size(77, 13)
        Label_CES_MutationRate.TabIndex = 48
        Label_CES_MutationRate.Text = "Mutation Rate:"
        '
        'Label_CES_Reproduction
        '
        Label_CES_Reproduction.AutoSize = True
        Label_CES_Reproduction.Location = New System.Drawing.Point(3, 127)
        Label_CES_Reproduction.Name = "Label_CES_Reproduction"
        Label_CES_Reproduction.Size = New System.Drawing.Size(63, 13)
        Label_CES_Reproduction.TabIndex = 4
        Label_CES_Reproduction.Text = "Reproduct.:"
        '
        'Label_CES_NExchangeSecPop
        '
        Label_CES_NExchangeSecPop.AutoSize = True
        Label_CES_NExchangeSecPop.Location = New System.Drawing.Point(4, 16)
        Label_CES_NExchangeSecPop.Name = "Label_CES_NExchangeSecPop"
        Label_CES_NExchangeSecPop.Size = New System.Drawing.Size(121, 13)
        Label_CES_NExchangeSecPop.TabIndex = 46
        Label_CES_NExchangeSecPop.Text = "Exchange with SecPop:"
        '
        'Label_CES_NChilds
        '
        Label_CES_NChilds.AutoSize = True
        Label_CES_NChilds.Location = New System.Drawing.Point(2, 74)
        Label_CES_NChilds.Name = "Label_CES_NChilds"
        Label_CES_NChilds.Size = New System.Drawing.Size(90, 13)
        Label_CES_NChilds.TabIndex = 2
        Label_CES_NChilds.Text = "Number of Childs:"
        '
        'Label_CES_NParents
        '
        Label_CES_NParents.AutoSize = True
        Label_CES_NParents.Location = New System.Drawing.Point(2, 48)
        Label_CES_NParents.Name = "Label_CES_NParents"
        Label_CES_NParents.Size = New System.Drawing.Size(98, 13)
        Label_CES_NParents.TabIndex = 1
        Label_CES_NParents.Text = "Number of Parents:"
        '
        'Label_CES_NGenerations
        '
        Label_CES_NGenerations.AutoSize = True
        Label_CES_NGenerations.Location = New System.Drawing.Point(2, 22)
        Label_CES_NGenerations.Name = "Label_CES_NGenerations"
        Label_CES_NGenerations.Size = New System.Drawing.Size(119, 13)
        Label_CES_NGenerations.TabIndex = 0
        Label_CES_NGenerations.Text = "Number of Generations:"
        '
        'Label_CES_Mutation
        '
        Label_CES_Mutation.AutoSize = True
        Label_CES_Mutation.Location = New System.Drawing.Point(3, 154)
        Label_CES_Mutation.Name = "Label_CES_Mutation"
        Label_CES_Mutation.Size = New System.Drawing.Size(51, 13)
        Label_CES_Mutation.TabIndex = 3
        Label_CES_Mutation.Text = "Mutation:"
        '
        'Label_CES_Selection
        '
        Label_CES_Selection.AutoSize = True
        Label_CES_Selection.Location = New System.Drawing.Point(2, 100)
        Label_CES_Selection.Name = "Label_CES_Selection"
        Label_CES_Selection.Size = New System.Drawing.Size(54, 13)
        Label_CES_Selection.TabIndex = 1
        Label_CES_Selection.Text = "Selection:"
        '
        'Label_CES_IniValues
        '
        Label_CES_IniValues.AutoSize = True
        Label_CES_IniValues.Location = New System.Drawing.Point(4, 32)
        Label_CES_IniValues.Name = "Label_CES_IniValues"
        Label_CES_IniValues.Size = New System.Drawing.Size(68, 13)
        Label_CES_IniValues.TabIndex = 2
        Label_CES_IniValues.Text = "Initial values:"
        '
        'Label_OptModus
        '
        Label_OptModus.AutoSize = True
        Label_OptModus.Location = New System.Drawing.Point(2, 9)
        Label_OptModus.Name = "Label_OptModus"
        Label_OptModus.Size = New System.Drawing.Size(42, 13)
        Label_OptModus.TabIndex = 31
        Label_OptModus.Text = "Modus:"
        '
        'LabelStrategie
        '
        LabelStrategie.AutoSize = True
        LabelStrategie.Location = New System.Drawing.Point(2, 37)
        LabelStrategie.Name = "LabelStrategie"
        LabelStrategie.Size = New System.Drawing.Size(54, 13)
        LabelStrategie.TabIndex = 19
        LabelStrategie.Text = "Selektion:"
        '
        'LabelStartwerte
        '
        LabelStartwerte.AutoSize = True
        LabelStartwerte.Location = New System.Drawing.Point(2, 64)
        LabelStartwerte.Name = "LabelStartwerte"
        LabelStartwerte.Size = New System.Drawing.Size(58, 13)
        LabelStartwerte.TabIndex = 29
        LabelStartwerte.Text = "Startwerte:"
        '
        'LabelMutation
        '
        LabelMutation.AutoSize = True
        LabelMutation.Location = New System.Drawing.Point(2, 91)
        LabelMutation.Name = "LabelMutation"
        LabelMutation.Size = New System.Drawing.Size(51, 13)
        LabelMutation.TabIndex = 34
        LabelMutation.Text = "Mutation:"
        '
        'LabelStartSchrittweite
        '
        LabelStartSchrittweite.AutoSize = True
        LabelStartSchrittweite.Location = New System.Drawing.Point(3, 117)
        LabelStartSchrittweite.Name = "LabelStartSchrittweite"
        LabelStartSchrittweite.Size = New System.Drawing.Size(90, 13)
        LabelStartSchrittweite.TabIndex = 27
        LabelStartSchrittweite.Text = "Start-Schrittweite:"
        '
        'LabelAnzNachf
        '
        LabelAnzNachf.AutoSize = True
        LabelAnzNachf.Location = New System.Drawing.Point(8, 64)
        LabelAnzNachf.Name = "LabelAnzNachf"
        LabelAnzNachf.Size = New System.Drawing.Size(115, 13)
        LabelAnzNachf.TabIndex = 16
        LabelAnzNachf.Text = "Anzahl der Nachfolger:"
        '
        'LabelAnzGen
        '
        LabelAnzGen.AutoSize = True
        LabelAnzGen.Location = New System.Drawing.Point(7, 20)
        LabelAnzGen.Name = "LabelAnzGen"
        LabelAnzGen.Size = New System.Drawing.Size(127, 13)
        LabelAnzGen.TabIndex = 14
        LabelAnzGen.Text = "Anzahl der Generationen:"
        '
        'Label2
        '
        Label2.Location = New System.Drawing.Point(9, 18)
        Label2.Name = "Label2"
        Label2.Size = New System.Drawing.Size(58, 34)
        Label2.TabIndex = 21
        Label2.Text = "Ermitteln der Eltern:"
        '
        'LabelInteract
        '
        LabelInteract.AutoSize = True
        LabelInteract.Location = New System.Drawing.Point(8, 24)
        LabelInteract.Name = "LabelInteract"
        LabelInteract.Size = New System.Drawing.Size(117, 13)
        LabelInteract.TabIndex = 38
        LabelInteract.Text = "Austausch mit SekPop:"
        '
        'LabelAnzRunden
        '
        LabelAnzRunden.AutoSize = True
        LabelAnzRunden.Location = New System.Drawing.Point(8, 16)
        LabelAnzRunden.Name = "LabelAnzRunden"
        LabelAnzRunden.Size = New System.Drawing.Size(101, 13)
        LabelAnzRunden.TabIndex = 6
        LabelAnzRunden.Text = "Anzahl der Runden:"
        '
        'LabelAnzPop
        '
        LabelAnzPop.AutoSize = True
        LabelAnzPop.Location = New System.Drawing.Point(7, 36)
        LabelAnzPop.Name = "LabelAnzPop"
        LabelAnzPop.Size = New System.Drawing.Size(107, 13)
        LabelAnzPop.TabIndex = 7
        LabelAnzPop.Text = "Anzahl Populationen:"
        '
        'LabelAnzPopEltern
        '
        LabelAnzPopEltern.AutoSize = True
        LabelAnzPopEltern.Location = New System.Drawing.Point(7, 58)
        LabelAnzPopEltern.Name = "LabelAnzPopEltern"
        LabelAnzPopEltern.Size = New System.Drawing.Size(112, 13)
        LabelAnzPopEltern.TabIndex = 8
        LabelAnzPopEltern.Text = "Anzahl Eltern [max=5]:"
        '
        'LabelOptPopEltern
        '
        LabelOptPopEltern.Location = New System.Drawing.Point(8, 80)
        LabelOptPopEltern.Name = "LabelOptPopEltern"
        LabelOptPopEltern.Size = New System.Drawing.Size(76, 28)
        LabelOptPopEltern.TabIndex = 9
        LabelOptPopEltern.Text = "Ermittlung der Pop-Eltern:"
        '
        'LabelPopStrategie
        '
        LabelPopStrategie.Location = New System.Drawing.Point(8, 110)
        LabelPopStrategie.Name = "LabelPopStrategie"
        LabelPopStrategie.Size = New System.Drawing.Size(76, 17)
        LabelPopStrategie.TabIndex = 10
        LabelPopStrategie.Text = "Selektion:"
        '
        'LabelPopPenalty
        '
        LabelPopPenalty.Location = New System.Drawing.Point(8, 127)
        LabelPopPenalty.Name = "LabelPopPenalty"
        LabelPopPenalty.Size = New System.Drawing.Size(76, 29)
        LabelPopPenalty.TabIndex = 11
        LabelPopPenalty.Text = "Ermittlung der Pop-Güte:"
        '
        'Label_Line
        '
        Label_Line.BackColor = System.Drawing.SystemColors.WindowText
        Label_Line.Location = New System.Drawing.Point(15, 128)
        Label_Line.Name = "Label_Line"
        Label_Line.Size = New System.Drawing.Size(165, 1)
        Label_Line.TabIndex = 50
        '
        'Label9
        '
        Label9.AutoSize = True
        Label9.ForeColor = System.Drawing.Color.Blue
        Label9.Location = New System.Drawing.Point(6, 223)
        Label9.Name = "Label9"
        Label9.Size = New System.Drawing.Size(67, 13)
        Label9.TabIndex = 38
        Label9.Text = "Rückschritte"
        '
        'Label8
        '
        Label8.AutoSize = True
        Label8.ForeColor = System.Drawing.Color.Blue
        Label8.Location = New System.Drawing.Point(6, 199)
        Label8.Name = "Label8"
        Label8.Size = New System.Drawing.Size(107, 13)
        Label8.TabIndex = 37
        Label8.Text = "Extrapolationsschritte"
        '
        'Label7
        '
        Label7.AutoSize = True
        Label7.ForeColor = System.Drawing.Color.Blue
        Label7.Location = New System.Drawing.Point(6, 175)
        Label7.Name = "Label7"
        Label7.Size = New System.Drawing.Size(99, 13)
        Label7.TabIndex = 36
        Label7.Text = "Tastschritte gesamt"
        '
        'Label6
        '
        Label6.AutoSize = True
        Label6.ForeColor = System.Drawing.Color.Blue
        Label6.Location = New System.Drawing.Point(6, 151)
        Label6.Name = "Label6"
        Label6.Size = New System.Drawing.Size(89, 13)
        Label6.TabIndex = 35
        Label6.Text = "Tastschritte mittel"
        '
        'Label4
        '
        Label4.AutoSize = True
        Label4.ForeColor = System.Drawing.Color.Blue
        Label4.Location = New System.Drawing.Point(6, 127)
        Label4.Name = "Label4"
        Label4.Size = New System.Drawing.Size(96, 13)
        Label4.TabIndex = 33
        Label4.Text = "Tastschritte aktuell"
        '
        'Label3
        '
        Label3.AutoSize = True
        Label3.Location = New System.Drawing.Point(6, 43)
        Label3.Name = "Label3"
        Label3.Size = New System.Drawing.Size(87, 13)
        Label3.TabIndex = 32
        Label3.Text = "End-Schrittweite:"
        '
        'Label1
        '
        Label1.AutoSize = True
        Label1.Location = New System.Drawing.Point(6, 13)
        Label1.Name = "Label1"
        Label1.Size = New System.Drawing.Size(90, 13)
        Label1.TabIndex = 30
        Label1.Text = "Start-Schrittweite:"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage_PES)
        Me.TabControl1.Controls.Add(Me.TabPage_CES)
        Me.TabControl1.Controls.Add(Me.TabPage_HookeJeeves)
        Me.TabControl1.Controls.Add(Me.TabPage_Hybrid2008)
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.Location = New System.Drawing.Point(1, 41)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.Padding = New System.Drawing.Point(0, 0)
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(230, 656)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage_PES
        '
        Me.TabPage_PES.AutoScroll = True
        Me.TabPage_PES.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage_PES.Controls.Add(Label_OptModus)
        Me.TabPage_PES.Controls.Add(Me.Label_OptModusValue)
        Me.TabPage_PES.Controls.Add(LabelStrategie)
        Me.TabPage_PES.Controls.Add(Me.ComboOptStrategie)
        Me.TabPage_PES.Controls.Add(LabelStartwerte)
        Me.TabPage_PES.Controls.Add(Me.ComboOptStartparameter)
        Me.TabPage_PES.Controls.Add(LabelMutation)
        Me.TabPage_PES.Controls.Add(Me.ComboOptDnMutation)
        Me.TabPage_PES.Controls.Add(LabelStartSchrittweite)
        Me.TabPage_PES.Controls.Add(Me.TextDeltaStart)
        Me.TabPage_PES.Controls.Add(Me.CheckisDnVektor)
        Me.TabPage_PES.Controls.Add(Me.GroupBox_Generationen)
        Me.TabPage_PES.Controls.Add(Me.GroupBox_Eltern)
        Me.TabPage_PES.Controls.Add(Me.GroupBox_SekPop)
        Me.TabPage_PES.Controls.Add(Me.CheckisPopul)
        Me.TabPage_PES.Controls.Add(Me.GroupBox_Populationen)
        Me.TabPage_PES.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage_PES.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_PES.Margin = New System.Windows.Forms.Padding(0)
        Me.TabPage_PES.Name = "TabPage_PES"
        Me.TabPage_PES.Size = New System.Drawing.Size(222, 630)
        Me.TabPage_PES.TabIndex = 0
        Me.TabPage_PES.Text = "PES"
        '
        'Label_OptModusValue
        '
        Me.Label_OptModusValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_OptModusValue.Location = New System.Drawing.Point(59, 9)
        Me.Label_OptModusValue.Name = "Label_OptModusValue"
        Me.Label_OptModusValue.Size = New System.Drawing.Size(140, 13)
        Me.Label_OptModusValue.TabIndex = 0
        Me.Label_OptModusValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboOptStrategie
        '
        Me.ComboOptStrategie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptStrategie.Location = New System.Drawing.Point(80, 34)
        Me.ComboOptStrategie.Name = "ComboOptStrategie"
        Me.ComboOptStrategie.Size = New System.Drawing.Size(123, 21)
        Me.ComboOptStrategie.TabIndex = 1
        '
        'ComboOptStartparameter
        '
        Me.ComboOptStartparameter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptStartparameter.Location = New System.Drawing.Point(80, 61)
        Me.ComboOptStartparameter.Name = "ComboOptStartparameter"
        Me.ComboOptStartparameter.Size = New System.Drawing.Size(123, 21)
        Me.ComboOptStartparameter.TabIndex = 2
        '
        'ComboOptDnMutation
        '
        Me.ComboOptDnMutation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptDnMutation.FormattingEnabled = True
        Me.ComboOptDnMutation.Location = New System.Drawing.Point(80, 88)
        Me.ComboOptDnMutation.Name = "ComboOptDnMutation"
        Me.ComboOptDnMutation.Size = New System.Drawing.Size(123, 21)
        Me.ComboOptDnMutation.TabIndex = 3
        '
        'TextDeltaStart
        '
        Me.TextDeltaStart.DecimalPlaces = 2
        Me.TextDeltaStart.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.TextDeltaStart.Location = New System.Drawing.Point(150, 115)
        Me.TextDeltaStart.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TextDeltaStart.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.TextDeltaStart.Name = "TextDeltaStart"
        Me.TextDeltaStart.Size = New System.Drawing.Size(53, 20)
        Me.TextDeltaStart.TabIndex = 4
        Me.TextDeltaStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextDeltaStart.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'CheckisDnVektor
        '
        Me.CheckisDnVektor.Location = New System.Drawing.Point(5, 141)
        Me.CheckisDnVektor.Name = "CheckisDnVektor"
        Me.CheckisDnVektor.Size = New System.Drawing.Size(144, 18)
        Me.CheckisDnVektor.TabIndex = 5
        Me.CheckisDnVektor.Text = "mit Schrittweitenvektor"
        '
        'GroupBox_Generationen
        '
        Me.GroupBox_Generationen.Controls.Add(Me.TextAnzNachf)
        Me.GroupBox_Generationen.Controls.Add(Me.TextAnzEltern)
        Me.GroupBox_Generationen.Controls.Add(Me.TextAnzGen)
        Me.GroupBox_Generationen.Controls.Add(LabelAnzNachf)
        Me.GroupBox_Generationen.Controls.Add(Me.LabelAnzEltern)
        Me.GroupBox_Generationen.Controls.Add(LabelAnzGen)
        Me.GroupBox_Generationen.Location = New System.Drawing.Point(4, 165)
        Me.GroupBox_Generationen.Name = "GroupBox_Generationen"
        Me.GroupBox_Generationen.Size = New System.Drawing.Size(200, 90)
        Me.GroupBox_Generationen.TabIndex = 6
        Me.GroupBox_Generationen.TabStop = False
        Me.GroupBox_Generationen.Text = "Generationen"
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
        'LabelAnzEltern
        '
        Me.LabelAnzEltern.AutoSize = True
        Me.LabelAnzEltern.Location = New System.Drawing.Point(7, 42)
        Me.LabelAnzEltern.Name = "LabelAnzEltern"
        Me.LabelAnzEltern.Size = New System.Drawing.Size(90, 13)
        Me.LabelAnzEltern.TabIndex = 15
        Me.LabelAnzEltern.Text = "Anzahl der Eltern:"
        '
        'GroupBox_Eltern
        '
        Me.GroupBox_Eltern.Controls.Add(Me.ComboOptEltern)
        Me.GroupBox_Eltern.Controls.Add(Me.CheckisTournamentSelection)
        Me.GroupBox_Eltern.Controls.Add(Me.TextRekombxy)
        Me.GroupBox_Eltern.Controls.Add(Label2)
        Me.GroupBox_Eltern.Controls.Add(Me.LabelRekombxy1)
        Me.GroupBox_Eltern.Controls.Add(Me.LabelRekombxy3)
        Me.GroupBox_Eltern.Location = New System.Drawing.Point(4, 261)
        Me.GroupBox_Eltern.Name = "GroupBox_Eltern"
        Me.GroupBox_Eltern.Size = New System.Drawing.Size(200, 104)
        Me.GroupBox_Eltern.TabIndex = 7
        Me.GroupBox_Eltern.TabStop = False
        Me.GroupBox_Eltern.Text = "Eltern"
        '
        'ComboOptEltern
        '
        Me.ComboOptEltern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptEltern.Location = New System.Drawing.Point(64, 23)
        Me.ComboOptEltern.Name = "ComboOptEltern"
        Me.ComboOptEltern.Size = New System.Drawing.Size(130, 21)
        Me.ComboOptEltern.TabIndex = 0
        '
        'CheckisTournamentSelection
        '
        Me.CheckisTournamentSelection.AutoSize = True
        Me.CheckisTournamentSelection.Location = New System.Drawing.Point(11, 81)
        Me.CheckisTournamentSelection.Name = "CheckisTournamentSelection"
        Me.CheckisTournamentSelection.Size = New System.Drawing.Size(130, 17)
        Me.CheckisTournamentSelection.TabIndex = 2
        Me.CheckisTournamentSelection.Text = "Tournament Selection"
        Me.CheckisTournamentSelection.UseVisualStyleBackColor = True
        '
        'TextRekombxy
        '
        Me.TextRekombxy.Location = New System.Drawing.Point(33, 55)
        Me.TextRekombxy.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TextRekombxy.Name = "TextRekombxy"
        Me.TextRekombxy.Size = New System.Drawing.Size(40, 20)
        Me.TextRekombxy.TabIndex = 1
        Me.TextRekombxy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextRekombxy.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'LabelRekombxy1
        '
        Me.LabelRekombxy1.AutoSize = True
        Me.LabelRekombxy1.Location = New System.Drawing.Point(9, 58)
        Me.LabelRekombxy1.Name = "LabelRekombxy1"
        Me.LabelRekombxy1.Size = New System.Drawing.Size(22, 13)
        Me.LabelRekombxy1.TabIndex = 6
        Me.LabelRekombxy1.Text = "X /"
        '
        'LabelRekombxy3
        '
        Me.LabelRekombxy3.AutoSize = True
        Me.LabelRekombxy3.Location = New System.Drawing.Point(75, 57)
        Me.LabelRekombxy3.Name = "LabelRekombxy3"
        Me.LabelRekombxy3.Size = New System.Drawing.Size(81, 13)
        Me.LabelRekombxy3.TabIndex = 25
        Me.LabelRekombxy3.Text = "-Rekombination"
        '
        'GroupBox_SekPop
        '
        Me.GroupBox_SekPop.Controls.Add(Me.CheckBox_isSekPopBegrenzung)
        Me.GroupBox_SekPop.Controls.Add(Me.TextInteract)
        Me.GroupBox_SekPop.Controls.Add(Me.TextMaxMemberSekPop)
        Me.GroupBox_SekPop.Controls.Add(LabelInteract)
        Me.GroupBox_SekPop.Controls.Add(Me.LabelMaxMemberSekPop)
        Me.GroupBox_SekPop.Location = New System.Drawing.Point(4, 371)
        Me.GroupBox_SekPop.Name = "GroupBox_SekPop"
        Me.GroupBox_SekPop.Size = New System.Drawing.Size(200, 93)
        Me.GroupBox_SekPop.TabIndex = 8
        Me.GroupBox_SekPop.TabStop = False
        Me.GroupBox_SekPop.Text = "Sekundäre Population"
        '
        'CheckBox_isSekPopBegrenzung
        '
        Me.CheckBox_isSekPopBegrenzung.AutoSize = True
        Me.CheckBox_isSekPopBegrenzung.Location = New System.Drawing.Point(10, 45)
        Me.CheckBox_isSekPopBegrenzung.Name = "CheckBox_isSekPopBegrenzung"
        Me.CheckBox_isSekPopBegrenzung.Size = New System.Drawing.Size(149, 17)
        Me.CheckBox_isSekPopBegrenzung.TabIndex = 1
        Me.CheckBox_isSekPopBegrenzung.Text = "SekPop-Größe begrenzen"
        Me.CheckBox_isSekPopBegrenzung.UseVisualStyleBackColor = True
        '
        'TextInteract
        '
        Me.TextInteract.Location = New System.Drawing.Point(140, 22)
        Me.TextInteract.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.TextInteract.Name = "TextInteract"
        Me.TextInteract.Size = New System.Drawing.Size(53, 20)
        Me.TextInteract.TabIndex = 0
        Me.TextInteract.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip1.SetToolTip(Me.TextInteract, "Alle n Generationen")
        Me.TextInteract.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'TextMaxMemberSekPop
        '
        Me.TextMaxMemberSekPop.Location = New System.Drawing.Point(140, 65)
        Me.TextMaxMemberSekPop.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.TextMaxMemberSekPop.Name = "TextMaxMemberSekPop"
        Me.TextMaxMemberSekPop.Size = New System.Drawing.Size(53, 20)
        Me.TextMaxMemberSekPop.TabIndex = 2
        Me.TextMaxMemberSekPop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextMaxMemberSekPop.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'LabelMaxMemberSekPop
        '
        Me.LabelMaxMemberSekPop.AutoSize = True
        Me.LabelMaxMemberSekPop.Location = New System.Drawing.Point(53, 67)
        Me.LabelMaxMemberSekPop.Name = "LabelMaxMemberSekPop"
        Me.LabelMaxMemberSekPop.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.LabelMaxMemberSekPop.Size = New System.Drawing.Size(81, 13)
        Me.LabelMaxMemberSekPop.TabIndex = 40
        Me.LabelMaxMemberSekPop.Text = "Max. Mitglieder:"
        '
        'CheckisPopul
        '
        Me.CheckisPopul.Location = New System.Drawing.Point(5, 469)
        Me.CheckisPopul.Name = "CheckisPopul"
        Me.CheckisPopul.Size = New System.Drawing.Size(112, 18)
        Me.CheckisPopul.TabIndex = 9
        Me.CheckisPopul.Text = "mit Populationen"
        '
        'GroupBox_Populationen
        '
        Me.GroupBox_Populationen.Controls.Add(LabelAnzRunden)
        Me.GroupBox_Populationen.Controls.Add(Me.TextAnzRunden)
        Me.GroupBox_Populationen.Controls.Add(LabelAnzPop)
        Me.GroupBox_Populationen.Controls.Add(Me.TextAnzPop)
        Me.GroupBox_Populationen.Controls.Add(LabelAnzPopEltern)
        Me.GroupBox_Populationen.Controls.Add(Me.TextAnzPopEltern)
        Me.GroupBox_Populationen.Controls.Add(LabelOptPopEltern)
        Me.GroupBox_Populationen.Controls.Add(Me.ComboOptPopEltern)
        Me.GroupBox_Populationen.Controls.Add(LabelPopStrategie)
        Me.GroupBox_Populationen.Controls.Add(Me.ComboOptPopStrategie)
        Me.GroupBox_Populationen.Controls.Add(LabelPopPenalty)
        Me.GroupBox_Populationen.Controls.Add(Me.ComboOptPopPenalty)
        Me.GroupBox_Populationen.Enabled = False
        Me.GroupBox_Populationen.Location = New System.Drawing.Point(4, 493)
        Me.GroupBox_Populationen.Name = "GroupBox_Populationen"
        Me.GroupBox_Populationen.Size = New System.Drawing.Size(200, 158)
        Me.GroupBox_Populationen.TabIndex = 10
        Me.GroupBox_Populationen.TabStop = False
        Me.GroupBox_Populationen.Text = "Populationen"
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
        'ComboOptPopEltern
        '
        Me.ComboOptPopEltern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptPopEltern.Location = New System.Drawing.Point(85, 84)
        Me.ComboOptPopEltern.Name = "ComboOptPopEltern"
        Me.ComboOptPopEltern.Size = New System.Drawing.Size(108, 21)
        Me.ComboOptPopEltern.TabIndex = 3
        '
        'ComboOptPopStrategie
        '
        Me.ComboOptPopStrategie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptPopStrategie.Location = New System.Drawing.Point(85, 108)
        Me.ComboOptPopStrategie.Name = "ComboOptPopStrategie"
        Me.ComboOptPopStrategie.Size = New System.Drawing.Size(108, 21)
        Me.ComboOptPopStrategie.TabIndex = 4
        '
        'ComboOptPopPenalty
        '
        Me.ComboOptPopPenalty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptPopPenalty.Location = New System.Drawing.Point(85, 132)
        Me.ComboOptPopPenalty.Name = "ComboOptPopPenalty"
        Me.ComboOptPopPenalty.Size = New System.Drawing.Size(108, 21)
        Me.ComboOptPopPenalty.TabIndex = 5
        '
        'TabPage_CES
        '
        Me.TabPage_CES.AutoScroll = True
        Me.TabPage_CES.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage_CES.Controls.Add(Me.CheckBox_CES_UseSecPop_CES)
        Me.TabPage_CES.Controls.Add(Me.GroupBox_CES_SecPop)
        Me.TabPage_CES.Controls.Add(Me.GroupBox_CES_Hybrid)
        Me.TabPage_CES.Controls.Add(Me.CheckBox_CES_RealOptimisation)
        Me.TabPage_CES.Controls.Add(Me.GroupBoxCES)
        Me.TabPage_CES.Controls.Add(Me.Combo_CES_IniValues)
        Me.TabPage_CES.Controls.Add(Label_CES_IniValues)
        Me.TabPage_CES.Controls.Add(Me.Label_CES_OptModus)
        Me.TabPage_CES.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage_CES.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_CES.Margin = New System.Windows.Forms.Padding(0)
        Me.TabPage_CES.Name = "TabPage_CES"
        Me.TabPage_CES.Size = New System.Drawing.Size(222, 630)
        Me.TabPage_CES.TabIndex = 1
        Me.TabPage_CES.Text = "CES"
        '
        'CheckBox_CES_UseSecPop_CES
        '
        Me.CheckBox_CES_UseSecPop_CES.AutoSize = True
        Me.CheckBox_CES_UseSecPop_CES.Checked = True
        Me.CheckBox_CES_UseSecPop_CES.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox_CES_UseSecPop_CES.Location = New System.Drawing.Point(4, 280)
        Me.CheckBox_CES_UseSecPop_CES.Name = "CheckBox_CES_UseSecPop_CES"
        Me.CheckBox_CES_UseSecPop_CES.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_CES_UseSecPop_CES.Size = New System.Drawing.Size(150, 17)
        Me.CheckBox_CES_UseSecPop_CES.TabIndex = 56
        Me.CheckBox_CES_UseSecPop_CES.Text = "Use secondary Population"
        Me.CheckBox_CES_UseSecPop_CES.UseVisualStyleBackColor = True
        '
        'GroupBox_CES_SecPop
        '
        Me.GroupBox_CES_SecPop.Controls.Add(Me.CheckBox_CES_isSecPopRestriction)
        Me.GroupBox_CES_SecPop.Controls.Add(Label_CES_NExchangeSecPop)
        Me.GroupBox_CES_SecPop.Controls.Add(Me.Label_CES_NMembersSecPop)
        Me.GroupBox_CES_SecPop.Controls.Add(Me.Numeric_CES_n_exchange_SecPop)
        Me.GroupBox_CES_SecPop.Controls.Add(Me.Numeric_CES_n_member_SecPop)
        Me.GroupBox_CES_SecPop.Location = New System.Drawing.Point(4, 303)
        Me.GroupBox_CES_SecPop.Name = "GroupBox_CES_SecPop"
        Me.GroupBox_CES_SecPop.Size = New System.Drawing.Size(200, 81)
        Me.GroupBox_CES_SecPop.TabIndex = 13
        Me.GroupBox_CES_SecPop.TabStop = False
        Me.GroupBox_CES_SecPop.Text = "Secondary Population"
        '
        'CheckBox_CES_isSecPopRestriction
        '
        Me.CheckBox_CES_isSecPopRestriction.AutoSize = True
        Me.CheckBox_CES_isSecPopRestriction.Location = New System.Drawing.Point(7, 35)
        Me.CheckBox_CES_isSecPopRestriction.Name = "CheckBox_CES_isSecPopRestriction"
        Me.CheckBox_CES_isSecPopRestriction.Size = New System.Drawing.Size(124, 17)
        Me.CheckBox_CES_isSecPopRestriction.TabIndex = 57
        Me.CheckBox_CES_isSecPopRestriction.Text = "Restrict SecPop size"
        Me.CheckBox_CES_isSecPopRestriction.UseVisualStyleBackColor = True
        '
        'Label_CES_NMembersSecPop
        '
        Me.Label_CES_NMembersSecPop.Location = New System.Drawing.Point(20, 54)
        Me.Label_CES_NMembersSecPop.Name = "Label_CES_NMembersSecPop"
        Me.Label_CES_NMembersSecPop.Size = New System.Drawing.Size(114, 18)
        Me.Label_CES_NMembersSecPop.TabIndex = 47
        Me.Label_CES_NMembersSecPop.Text = "Max SecPop size:"
        Me.Label_CES_NMembersSecPop.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Numeric_CES_n_exchange_SecPop
        '
        Me.Numeric_CES_n_exchange_SecPop.Location = New System.Drawing.Point(140, 14)
        Me.Numeric_CES_n_exchange_SecPop.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.Numeric_CES_n_exchange_SecPop.Name = "Numeric_CES_n_exchange_SecPop"
        Me.Numeric_CES_n_exchange_SecPop.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_CES_n_exchange_SecPop.TabIndex = 44
        Me.Numeric_CES_n_exchange_SecPop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_CES_n_exchange_SecPop.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'Numeric_CES_n_member_SecPop
        '
        Me.Numeric_CES_n_member_SecPop.Location = New System.Drawing.Point(140, 54)
        Me.Numeric_CES_n_member_SecPop.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.Numeric_CES_n_member_SecPop.Name = "Numeric_CES_n_member_SecPop"
        Me.Numeric_CES_n_member_SecPop.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_CES_n_member_SecPop.TabIndex = 45
        Me.Numeric_CES_n_member_SecPop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_CES_n_member_SecPop.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'GroupBox_CES_Hybrid
        '
        Me.GroupBox_CES_Hybrid.Controls.Add(Me.Numeric_CES_n_MemSize)
        Me.GroupBox_CES_Hybrid.Controls.Add(Label_CES_MemSize)
        Me.GroupBox_CES_Hybrid.Controls.Add(Me.Combo_CES_MemStrategy)
        Me.GroupBox_CES_Hybrid.Controls.Add(Me.CheckBox_CES_UseSecPop_PES)
        Me.GroupBox_CES_Hybrid.Controls.Add(Me.CheckBox_CES_StartPESPop)
        Me.GroupBox_CES_Hybrid.Controls.Add(Me.Numeric_CES_n_member_SecPop_PES)
        Me.GroupBox_CES_Hybrid.Controls.Add(Me.Numeric_CES_NExchange_SecPop_PES)
        Me.GroupBox_CES_Hybrid.Controls.Add(Label_CES_NExchange_secPop_PES)
        Me.GroupBox_CES_Hybrid.Controls.Add(Label_CES_NMembers_SecPop_PES)
        Me.GroupBox_CES_Hybrid.Controls.Add(Label_Line)
        Me.GroupBox_CES_Hybrid.Controls.Add(Me.Combo_CES_HybridType)
        Me.GroupBox_CES_Hybrid.Controls.Add(Label_CES_MemRank)
        Me.GroupBox_CES_Hybrid.Controls.Add(Label_CES_Hybrid_Type)
        Me.GroupBox_CES_Hybrid.Location = New System.Drawing.Point(4, 413)
        Me.GroupBox_CES_Hybrid.Name = "GroupBox_CES_Hybrid"
        Me.GroupBox_CES_Hybrid.Size = New System.Drawing.Size(200, 217)
        Me.GroupBox_CES_Hybrid.TabIndex = 12
        Me.GroupBox_CES_Hybrid.TabStop = False
        Me.GroupBox_CES_Hybrid.Text = "Hybrid Options"
        '
        'Numeric_CES_n_MemSize
        '
        Me.Numeric_CES_n_MemSize.Location = New System.Drawing.Point(138, 77)
        Me.Numeric_CES_n_MemSize.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.Numeric_CES_n_MemSize.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.Numeric_CES_n_MemSize.Name = "Numeric_CES_n_MemSize"
        Me.Numeric_CES_n_MemSize.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_CES_n_MemSize.TabIndex = 57
        Me.Numeric_CES_n_MemSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_CES_n_MemSize.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'Combo_CES_MemStrategy
        '
        Me.Combo_CES_MemStrategy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Combo_CES_MemStrategy.FormattingEnabled = True
        Me.Combo_CES_MemStrategy.Location = New System.Drawing.Point(88, 50)
        Me.Combo_CES_MemStrategy.Name = "Combo_CES_MemStrategy"
        Me.Combo_CES_MemStrategy.Size = New System.Drawing.Size(104, 21)
        Me.Combo_CES_MemStrategy.Sorted = True
        Me.Combo_CES_MemStrategy.TabIndex = 56
        '
        'CheckBox_CES_UseSecPop_PES
        '
        Me.CheckBox_CES_UseSecPop_PES.AutoSize = True
        Me.CheckBox_CES_UseSecPop_PES.Location = New System.Drawing.Point(3, 138)
        Me.CheckBox_CES_UseSecPop_PES.Name = "CheckBox_CES_UseSecPop_PES"
        Me.CheckBox_CES_UseSecPop_PES.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_CES_UseSecPop_PES.Size = New System.Drawing.Size(150, 17)
        Me.CheckBox_CES_UseSecPop_PES.TabIndex = 13
        Me.CheckBox_CES_UseSecPop_PES.Text = "Use secondary Population"
        Me.CheckBox_CES_UseSecPop_PES.UseVisualStyleBackColor = True
        '
        'CheckBox_CES_StartPESPop
        '
        Me.CheckBox_CES_StartPESPop.AutoSize = True
        Me.CheckBox_CES_StartPESPop.Location = New System.Drawing.Point(3, 103)
        Me.CheckBox_CES_StartPESPop.Name = "CheckBox_CES_StartPESPop"
        Me.CheckBox_CES_StartPESPop.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_CES_StartPESPop.Size = New System.Drawing.Size(136, 17)
        Me.CheckBox_CES_StartPESPop.TabIndex = 13
        Me.CheckBox_CES_StartPESPop.Text = "Start with Pop Mutation"
        Me.CheckBox_CES_StartPESPop.UseVisualStyleBackColor = True
        '
        'Numeric_CES_n_member_SecPop_PES
        '
        Me.Numeric_CES_n_member_SecPop_PES.Location = New System.Drawing.Point(139, 187)
        Me.Numeric_CES_n_member_SecPop_PES.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.Numeric_CES_n_member_SecPop_PES.Name = "Numeric_CES_n_member_SecPop_PES"
        Me.Numeric_CES_n_member_SecPop_PES.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_CES_n_member_SecPop_PES.TabIndex = 52
        Me.Numeric_CES_n_member_SecPop_PES.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_CES_n_member_SecPop_PES.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'Numeric_CES_NExchange_SecPop_PES
        '
        Me.Numeric_CES_NExchange_SecPop_PES.Location = New System.Drawing.Point(139, 161)
        Me.Numeric_CES_NExchange_SecPop_PES.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.Numeric_CES_NExchange_SecPop_PES.Name = "Numeric_CES_NExchange_SecPop_PES"
        Me.Numeric_CES_NExchange_SecPop_PES.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_CES_NExchange_SecPop_PES.TabIndex = 51
        Me.Numeric_CES_NExchange_SecPop_PES.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_CES_NExchange_SecPop_PES.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'Combo_CES_HybridType
        '
        Me.Combo_CES_HybridType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Combo_CES_HybridType.FormattingEnabled = True
        Me.Combo_CES_HybridType.Location = New System.Drawing.Point(88, 24)
        Me.Combo_CES_HybridType.Name = "Combo_CES_HybridType"
        Me.Combo_CES_HybridType.Size = New System.Drawing.Size(104, 21)
        Me.Combo_CES_HybridType.TabIndex = 13
        '
        'CheckBox_CES_RealOptimisation
        '
        Me.CheckBox_CES_RealOptimisation.AutoSize = True
        Me.CheckBox_CES_RealOptimisation.Location = New System.Drawing.Point(4, 390)
        Me.CheckBox_CES_RealOptimisation.Name = "CheckBox_CES_RealOptimisation"
        Me.CheckBox_CES_RealOptimisation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_CES_RealOptimisation.Size = New System.Drawing.Size(154, 17)
        Me.CheckBox_CES_RealOptimisation.TabIndex = 11
        Me.CheckBox_CES_RealOptimisation.Text = "Including Real Optimisation"
        Me.CheckBox_CES_RealOptimisation.UseVisualStyleBackColor = True
        '
        'GroupBoxCES
        '
        Me.GroupBoxCES.Controls.Add(Me.Numeric_CES_MutRate)
        Me.GroupBoxCES.Controls.Add(Label_CES_MutationRate)
        Me.GroupBoxCES.Controls.Add(Me.Combo_CES_Reproduction)
        Me.GroupBoxCES.Controls.Add(Label_CES_Reproduction)
        Me.GroupBoxCES.Controls.Add(Me.Numeric_CES_n_childs)
        Me.GroupBoxCES.Controls.Add(Me.Numeric_CES_n_Parents)
        Me.GroupBoxCES.Controls.Add(Me.Combo_CES_Mutation)
        Me.GroupBoxCES.Controls.Add(Me.Numeric_CES_n_Generations)
        Me.GroupBoxCES.Controls.Add(Me.Combo_CES_Selection)
        Me.GroupBoxCES.Controls.Add(Label_CES_NChilds)
        Me.GroupBoxCES.Controls.Add(Label_CES_NParents)
        Me.GroupBoxCES.Controls.Add(Label_CES_NGenerations)
        Me.GroupBoxCES.Controls.Add(Label_CES_Mutation)
        Me.GroupBoxCES.Controls.Add(Label_CES_Selection)
        Me.GroupBoxCES.Location = New System.Drawing.Point(4, 69)
        Me.GroupBoxCES.Name = "GroupBoxCES"
        Me.GroupBoxCES.Size = New System.Drawing.Size(200, 205)
        Me.GroupBoxCES.TabIndex = 9
        Me.GroupBoxCES.TabStop = False
        Me.GroupBoxCES.Text = "Mixed Integer Evolution Strategy"
        '
        'Numeric_CES_MutRate
        '
        Me.Numeric_CES_MutRate.Increment = New Decimal(New Integer() {5, 0, 0, 0})
        Me.Numeric_CES_MutRate.Location = New System.Drawing.Point(139, 178)
        Me.Numeric_CES_MutRate.Name = "Numeric_CES_MutRate"
        Me.Numeric_CES_MutRate.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_CES_MutRate.TabIndex = 49
        Me.Numeric_CES_MutRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_CES_MutRate.Value = New Decimal(New Integer() {25, 0, 0, 0})
        '
        'Combo_CES_Reproduction
        '
        Me.Combo_CES_Reproduction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Combo_CES_Reproduction.FormattingEnabled = True
        Me.Combo_CES_Reproduction.Location = New System.Drawing.Point(69, 124)
        Me.Combo_CES_Reproduction.Name = "Combo_CES_Reproduction"
        Me.Combo_CES_Reproduction.Size = New System.Drawing.Size(123, 21)
        Me.Combo_CES_Reproduction.TabIndex = 8
        '
        'Numeric_CES_n_childs
        '
        Me.Numeric_CES_n_childs.Location = New System.Drawing.Point(139, 72)
        Me.Numeric_CES_n_childs.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.Numeric_CES_n_childs.Minimum = New Decimal(New Integer() {3, 0, 0, 0})
        Me.Numeric_CES_n_childs.Name = "Numeric_CES_n_childs"
        Me.Numeric_CES_n_childs.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_CES_n_childs.TabIndex = 5
        Me.Numeric_CES_n_childs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_CES_n_childs.Value = New Decimal(New Integer() {15, 0, 0, 0})
        '
        'Numeric_CES_n_Parents
        '
        Me.Numeric_CES_n_Parents.Location = New System.Drawing.Point(139, 46)
        Me.Numeric_CES_n_Parents.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.Numeric_CES_n_Parents.Minimum = New Decimal(New Integer() {3, 0, 0, 0})
        Me.Numeric_CES_n_Parents.Name = "Numeric_CES_n_Parents"
        Me.Numeric_CES_n_Parents.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_CES_n_Parents.TabIndex = 4
        Me.Numeric_CES_n_Parents.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_CES_n_Parents.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'Combo_CES_Mutation
        '
        Me.Combo_CES_Mutation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Combo_CES_Mutation.FormattingEnabled = True
        Me.Combo_CES_Mutation.Location = New System.Drawing.Point(69, 151)
        Me.Combo_CES_Mutation.Name = "Combo_CES_Mutation"
        Me.Combo_CES_Mutation.Size = New System.Drawing.Size(123, 21)
        Me.Combo_CES_Mutation.TabIndex = 7
        '
        'Numeric_CES_n_Generations
        '
        Me.Numeric_CES_n_Generations.Location = New System.Drawing.Point(139, 20)
        Me.Numeric_CES_n_Generations.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.Numeric_CES_n_Generations.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.Numeric_CES_n_Generations.Name = "Numeric_CES_n_Generations"
        Me.Numeric_CES_n_Generations.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_CES_n_Generations.TabIndex = 3
        Me.Numeric_CES_n_Generations.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_CES_n_Generations.Value = New Decimal(New Integer() {500, 0, 0, 0})
        '
        'Combo_CES_Selection
        '
        Me.Combo_CES_Selection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Combo_CES_Selection.FormattingEnabled = True
        Me.Combo_CES_Selection.Location = New System.Drawing.Point(69, 97)
        Me.Combo_CES_Selection.Name = "Combo_CES_Selection"
        Me.Combo_CES_Selection.Size = New System.Drawing.Size(123, 21)
        Me.Combo_CES_Selection.TabIndex = 6
        '
        'Combo_CES_IniValues
        '
        Me.Combo_CES_IniValues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Combo_CES_IniValues.FormattingEnabled = True
        Me.Combo_CES_IniValues.Location = New System.Drawing.Point(81, 29)
        Me.Combo_CES_IniValues.Name = "Combo_CES_IniValues"
        Me.Combo_CES_IniValues.Size = New System.Drawing.Size(123, 21)
        Me.Combo_CES_IniValues.TabIndex = 5
        '
        'Label_CES_OptModus
        '
        Me.Label_CES_OptModus.AutoSize = True
        Me.Label_CES_OptModus.Location = New System.Drawing.Point(4, 6)
        Me.Label_CES_OptModus.Name = "Label_CES_OptModus"
        Me.Label_CES_OptModus.Size = New System.Drawing.Size(42, 13)
        Me.Label_CES_OptModus.TabIndex = 0
        Me.Label_CES_OptModus.Text = "Modus:"
        '
        'TabPage_HookeJeeves
        '
        Me.TabPage_HookeJeeves.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage_HookeJeeves.Controls.Add(Me.Label_HJ_RS)
        Me.TabPage_HookeJeeves.Controls.Add(Me.Label_HJ_ES)
        Me.TabPage_HookeJeeves.Controls.Add(Me.Label_HJ_TSgesamt)
        Me.TabPage_HookeJeeves.Controls.Add(Me.Label_HJ_TSmittel)
        Me.TabPage_HookeJeeves.Controls.Add(Label9)
        Me.TabPage_HookeJeeves.Controls.Add(Label8)
        Me.TabPage_HookeJeeves.Controls.Add(Label7)
        Me.TabPage_HookeJeeves.Controls.Add(Label6)
        Me.TabPage_HookeJeeves.Controls.Add(Me.Label_HJ_TSaktuelle)
        Me.TabPage_HookeJeeves.Controls.Add(Label4)
        Me.TabPage_HookeJeeves.Controls.Add(Label3)
        Me.TabPage_HookeJeeves.Controls.Add(Me.Numeric_HJ_DeltaFinish)
        Me.TabPage_HookeJeeves.Controls.Add(Label1)
        Me.TabPage_HookeJeeves.Controls.Add(Me.Numeric_HJ_DeltaStart)
        Me.TabPage_HookeJeeves.Controls.Add(Me.CheckBox_HJ_DNVektor)
        Me.TabPage_HookeJeeves.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage_HookeJeeves.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_HookeJeeves.Name = "TabPage_HookeJeeves"
        Me.TabPage_HookeJeeves.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_HookeJeeves.Size = New System.Drawing.Size(222, 630)
        Me.TabPage_HookeJeeves.TabIndex = 2
        Me.TabPage_HookeJeeves.Text = "Hooke&Jeeves"
        '
        'Label_HJ_RS
        '
        Me.Label_HJ_RS.AutoSize = True
        Me.Label_HJ_RS.ForeColor = System.Drawing.Color.Blue
        Me.Label_HJ_RS.Location = New System.Drawing.Point(139, 223)
        Me.Label_HJ_RS.Name = "Label_HJ_RS"
        Me.Label_HJ_RS.Size = New System.Drawing.Size(13, 13)
        Me.Label_HJ_RS.TabIndex = 42
        Me.Label_HJ_RS.Text = "0"
        Me.Label_HJ_RS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label_HJ_ES
        '
        Me.Label_HJ_ES.AutoSize = True
        Me.Label_HJ_ES.ForeColor = System.Drawing.Color.Blue
        Me.Label_HJ_ES.Location = New System.Drawing.Point(139, 199)
        Me.Label_HJ_ES.Name = "Label_HJ_ES"
        Me.Label_HJ_ES.Size = New System.Drawing.Size(13, 13)
        Me.Label_HJ_ES.TabIndex = 41
        Me.Label_HJ_ES.Text = "0"
        Me.Label_HJ_ES.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label_HJ_TSgesamt
        '
        Me.Label_HJ_TSgesamt.AutoSize = True
        Me.Label_HJ_TSgesamt.ForeColor = System.Drawing.Color.Blue
        Me.Label_HJ_TSgesamt.Location = New System.Drawing.Point(139, 175)
        Me.Label_HJ_TSgesamt.Name = "Label_HJ_TSgesamt"
        Me.Label_HJ_TSgesamt.Size = New System.Drawing.Size(13, 13)
        Me.Label_HJ_TSgesamt.TabIndex = 40
        Me.Label_HJ_TSgesamt.Text = "0"
        Me.Label_HJ_TSgesamt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label_HJ_TSmittel
        '
        Me.Label_HJ_TSmittel.AutoSize = True
        Me.Label_HJ_TSmittel.ForeColor = System.Drawing.Color.Blue
        Me.Label_HJ_TSmittel.Location = New System.Drawing.Point(139, 151)
        Me.Label_HJ_TSmittel.Name = "Label_HJ_TSmittel"
        Me.Label_HJ_TSmittel.Size = New System.Drawing.Size(13, 13)
        Me.Label_HJ_TSmittel.TabIndex = 39
        Me.Label_HJ_TSmittel.Text = "0"
        Me.Label_HJ_TSmittel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label_HJ_TSaktuelle
        '
        Me.Label_HJ_TSaktuelle.AutoSize = True
        Me.Label_HJ_TSaktuelle.ForeColor = System.Drawing.Color.Blue
        Me.Label_HJ_TSaktuelle.Location = New System.Drawing.Point(139, 127)
        Me.Label_HJ_TSaktuelle.Name = "Label_HJ_TSaktuelle"
        Me.Label_HJ_TSaktuelle.Size = New System.Drawing.Size(13, 13)
        Me.Label_HJ_TSaktuelle.TabIndex = 34
        Me.Label_HJ_TSaktuelle.Text = "0"
        Me.Label_HJ_TSaktuelle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Numeric_HJ_DeltaFinish
        '
        Me.Numeric_HJ_DeltaFinish.DecimalPlaces = 5
        Me.Numeric_HJ_DeltaFinish.Increment = New Decimal(New Integer() {1, 0, 0, 327680})
        Me.Numeric_HJ_DeltaFinish.Location = New System.Drawing.Point(142, 39)
        Me.Numeric_HJ_DeltaFinish.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.Numeric_HJ_DeltaFinish.Minimum = New Decimal(New Integer() {1, 0, 0, 327680})
        Me.Numeric_HJ_DeltaFinish.Name = "Numeric_HJ_DeltaFinish"
        Me.Numeric_HJ_DeltaFinish.Size = New System.Drawing.Size(65, 20)
        Me.Numeric_HJ_DeltaFinish.TabIndex = 1
        Me.Numeric_HJ_DeltaFinish.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_HJ_DeltaFinish.Value = New Decimal(New Integer() {1, 0, 0, 262144})
        '
        'Numeric_HJ_DeltaStart
        '
        Me.Numeric_HJ_DeltaStart.DecimalPlaces = 2
        Me.Numeric_HJ_DeltaStart.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.Numeric_HJ_DeltaStart.Location = New System.Drawing.Point(142, 9)
        Me.Numeric_HJ_DeltaStart.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.Numeric_HJ_DeltaStart.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.Numeric_HJ_DeltaStart.Name = "Numeric_HJ_DeltaStart"
        Me.Numeric_HJ_DeltaStart.Size = New System.Drawing.Size(65, 20)
        Me.Numeric_HJ_DeltaStart.TabIndex = 0
        Me.Numeric_HJ_DeltaStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_HJ_DeltaStart.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'CheckBox_HJ_DNVektor
        '
        Me.CheckBox_HJ_DNVektor.AutoSize = True
        Me.CheckBox_HJ_DNVektor.Enabled = False
        Me.CheckBox_HJ_DNVektor.Location = New System.Drawing.Point(15, 76)
        Me.CheckBox_HJ_DNVektor.Name = "CheckBox_HJ_DNVektor"
        Me.CheckBox_HJ_DNVektor.Size = New System.Drawing.Size(133, 17)
        Me.CheckBox_HJ_DNVektor.TabIndex = 2
        Me.CheckBox_HJ_DNVektor.Text = "mit Schrittweitenvektor"
        '
        'TabPage_Hybrid2008
        '
        Me.TabPage_Hybrid2008.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage_Hybrid2008.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage_Hybrid2008.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Hybrid2008.Name = "TabPage_Hybrid2008"
        Me.TabPage_Hybrid2008.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_Hybrid2008.Size = New System.Drawing.Size(222, 630)
        Me.TabPage_Hybrid2008.TabIndex = 3
        Me.TabPage_Hybrid2008.Text = "Hybrid2008"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ÖffnenToolStripButton, Me.SpeichernToolStripButton})
        Me.ToolStrip1.Location = New System.Drawing.Point(3, 16)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Padding = New System.Windows.Forms.Padding(0)
        Me.ToolStrip1.Size = New System.Drawing.Size(224, 25)
        Me.ToolStrip1.Stretch = True
        Me.ToolStrip1.TabIndex = 0
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
        Me.GroupBox_Einstellungen.Size = New System.Drawing.Size(230, 700)
        Me.GroupBox_Einstellungen.TabIndex = 0
        Me.GroupBox_Einstellungen.TabStop = False
        Me.GroupBox_Einstellungen.Text = "Einstellungen:"
        '
        'EVO_Einstellungen
        '
        Me.Controls.Add(Me.GroupBox_Einstellungen)
        Me.Name = "EVO_Einstellungen"
        Me.Size = New System.Drawing.Size(240, 740)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage_PES.ResumeLayout(False)
        Me.TabPage_PES.PerformLayout()
        CType(Me.TextDeltaStart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_Generationen.ResumeLayout(False)
        Me.GroupBox_Generationen.PerformLayout()
        CType(Me.TextAnzNachf, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextAnzEltern, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextAnzGen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_Eltern.ResumeLayout(False)
        Me.GroupBox_Eltern.PerformLayout()
        CType(Me.TextRekombxy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_SekPop.ResumeLayout(False)
        Me.GroupBox_SekPop.PerformLayout()
        CType(Me.TextInteract, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextMaxMemberSekPop, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_Populationen.ResumeLayout(False)
        Me.GroupBox_Populationen.PerformLayout()
        CType(Me.TextAnzRunden, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextAnzPop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextAnzPopEltern, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage_CES.ResumeLayout(False)
        Me.TabPage_CES.PerformLayout()
        Me.GroupBox_CES_SecPop.ResumeLayout(False)
        Me.GroupBox_CES_SecPop.PerformLayout()
        CType(Me.Numeric_CES_n_exchange_SecPop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_CES_n_member_SecPop, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_CES_Hybrid.ResumeLayout(False)
        Me.GroupBox_CES_Hybrid.PerformLayout()
        CType(Me.Numeric_CES_n_MemSize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_CES_n_member_SecPop_PES, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_CES_NExchange_SecPop_PES, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxCES.ResumeLayout(False)
        Me.GroupBoxCES.PerformLayout()
        CType(Me.Numeric_CES_MutRate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_CES_n_childs, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_CES_n_Parents, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_CES_n_Generations, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage_HookeJeeves.ResumeLayout(False)
        Me.TabPage_HookeJeeves.PerformLayout()
        CType(Me.Numeric_HJ_DeltaFinish, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_HJ_DeltaStart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.GroupBox_Einstellungen.ResumeLayout(False)
        Me.GroupBox_Einstellungen.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Private WithEvents Label_OptModusValue As System.Windows.Forms.Label
    Private WithEvents ComboOptStrategie As System.Windows.Forms.ComboBox
    Private WithEvents ComboOptStartparameter As System.Windows.Forms.ComboBox
    Private WithEvents TextDeltaStart As System.Windows.Forms.NumericUpDown
    Private WithEvents CheckisDnVektor As System.Windows.Forms.CheckBox
    Private WithEvents GroupBox_Generationen As System.Windows.Forms.GroupBox
    Private WithEvents TextMaxMemberSekPop As System.Windows.Forms.NumericUpDown
    Private WithEvents TextInteract As System.Windows.Forms.NumericUpDown
    Private WithEvents TextRekombxy As System.Windows.Forms.NumericUpDown
    Private WithEvents ComboOptEltern As System.Windows.Forms.ComboBox
    Private WithEvents TextAnzNachf As System.Windows.Forms.NumericUpDown
    Private WithEvents TextAnzEltern As System.Windows.Forms.NumericUpDown
    Private WithEvents TextAnzGen As System.Windows.Forms.NumericUpDown
    Private WithEvents LabelMaxMemberSekPop As System.Windows.Forms.Label
    Private WithEvents LabelRekombxy3 As System.Windows.Forms.Label
    Private WithEvents LabelRekombxy1 As System.Windows.Forms.Label
    Private WithEvents CheckisPopul As System.Windows.Forms.CheckBox
    Private WithEvents GroupBox_Populationen As System.Windows.Forms.GroupBox
    Private WithEvents TextAnzRunden As System.Windows.Forms.NumericUpDown
    Private WithEvents TextAnzPop As System.Windows.Forms.NumericUpDown
    Private WithEvents TextAnzPopEltern As System.Windows.Forms.NumericUpDown
    Private WithEvents ComboOptPopEltern As System.Windows.Forms.ComboBox
    Private WithEvents ComboOptPopStrategie As System.Windows.Forms.ComboBox
    Private WithEvents ComboOptPopPenalty As System.Windows.Forms.ComboBox
    Friend WithEvents ÖffnenToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents SpeichernToolStripButton As System.Windows.Forms.ToolStripButton
    Private WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents GroupBox_Einstellungen As System.Windows.Forms.GroupBox
    Friend WithEvents TabPage_HookeJeeves As System.Windows.Forms.TabPage
    Private WithEvents Numeric_HJ_DeltaFinish As System.Windows.Forms.NumericUpDown
    Private WithEvents Numeric_HJ_DeltaStart As System.Windows.Forms.NumericUpDown
    Private WithEvents CheckBox_HJ_DNVektor As System.Windows.Forms.CheckBox
    Public WithEvents TabPage_PES As System.Windows.Forms.TabPage
    Public WithEvents Label_HJ_TSaktuelle As System.Windows.Forms.Label
    Public WithEvents Label_HJ_RS As System.Windows.Forms.Label
    Public WithEvents Label_HJ_ES As System.Windows.Forms.Label
    Public WithEvents Label_HJ_TSgesamt As System.Windows.Forms.Label
    Public WithEvents Label_HJ_TSmittel As System.Windows.Forms.Label
    Friend WithEvents CheckisTournamentSelection As System.Windows.Forms.CheckBox
    Friend WithEvents ComboOptDnMutation As System.Windows.Forms.ComboBox
    Friend WithEvents Label_CES_OptModus As System.Windows.Forms.Label
    Friend WithEvents Combo_CES_Selection As System.Windows.Forms.ComboBox
    Friend WithEvents Combo_CES_IniValues As System.Windows.Forms.ComboBox
    Friend WithEvents Combo_CES_Reproduction As System.Windows.Forms.ComboBox
    Friend WithEvents Combo_CES_Mutation As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBoxCES As System.Windows.Forms.GroupBox
    Friend WithEvents Numeric_CES_n_childs As System.Windows.Forms.NumericUpDown
    Friend WithEvents Numeric_CES_n_Parents As System.Windows.Forms.NumericUpDown
    Friend WithEvents Numeric_CES_n_Generations As System.Windows.Forms.NumericUpDown
    Private WithEvents Numeric_CES_n_member_SecPop As System.Windows.Forms.NumericUpDown
    Private WithEvents Numeric_CES_n_exchange_SecPop As System.Windows.Forms.NumericUpDown
    Private WithEvents Label_CES_NMembersSecPop As System.Windows.Forms.Label
    Friend WithEvents GroupBox_CES_Hybrid As System.Windows.Forms.GroupBox
    Private WithEvents Numeric_CES_MutRate As System.Windows.Forms.NumericUpDown
    Friend WithEvents Combo_CES_HybridType As System.Windows.Forms.ComboBox
    Private WithEvents Numeric_CES_n_member_SecPop_PES As System.Windows.Forms.NumericUpDown
    Private WithEvents Numeric_CES_NExchange_SecPop_PES As System.Windows.Forms.NumericUpDown
    Friend WithEvents CheckBox_CES_UseSecPop_PES As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_CES_UseSecPop_CES As System.Windows.Forms.CheckBox
    Public WithEvents TabPage_CES As System.Windows.Forms.TabPage
    Friend WithEvents Combo_CES_MemStrategy As System.Windows.Forms.ComboBox
    Private WithEvents Numeric_CES_n_MemSize As System.Windows.Forms.NumericUpDown
    Private WithEvents GroupBox_SekPop As System.Windows.Forms.GroupBox
    Private WithEvents GroupBox_Eltern As System.Windows.Forms.GroupBox
    Private WithEvents CheckBox_isSekPopBegrenzung As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox_CES_isSecPopRestriction As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox_CES_SecPop As System.Windows.Forms.GroupBox
    Private WithEvents CheckBox_CES_RealOptimisation As System.Windows.Forms.CheckBox
    Private WithEvents LabelAnzEltern As System.Windows.Forms.Label
    Private WithEvents CheckBox_CES_StartPESPop As System.Windows.Forms.CheckBox
    Friend WithEvents TabPage_Hybrid2008 As System.Windows.Forms.TabPage
    Public WithEvents TabControl1 As System.Windows.Forms.TabControl
End Class
