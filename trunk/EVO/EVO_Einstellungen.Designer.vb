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
        Dim Label3 As System.Windows.Forms.Label
        Dim Label1 As System.Windows.Forms.Label
        Dim Label11 As System.Windows.Forms.Label
        Dim Label5 As System.Windows.Forms.Label
        Dim Label10 As System.Windows.Forms.Label
        Dim Label13 As System.Windows.Forms.Label
        Dim Label14 As System.Windows.Forms.Label
        Dim Label15 As System.Windows.Forms.Label
        Dim Label16 As System.Windows.Forms.Label
        Dim Label18 As System.Windows.Forms.Label
        Dim Label20 As System.Windows.Forms.Label
        Dim Label19 As System.Windows.Forms.Label
        Dim Label_Meta11 As System.Windows.Forms.Label
        Dim Label_Meta5 As System.Windows.Forms.Label
        Dim Label_Meta10 As System.Windows.Forms.Label
        Dim GroupBox_Diagramm As System.Windows.Forms.GroupBox
        Me.CheckBox_drawOnlyCurrentGen = New System.Windows.Forms.CheckBox
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage_General = New System.Windows.Forms.TabPage
        Me.GroupBox_Sim = New System.Windows.Forms.GroupBox
        Me.CheckBox_useMultithreading = New System.Windows.Forms.CheckBox
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
        Me.Numeric_HJ_DeltaFinish = New System.Windows.Forms.NumericUpDown
        Me.Numeric_HJ_DeltaStart = New System.Windows.Forms.NumericUpDown
        Me.CheckBox_HJ_DNVektor = New System.Windows.Forms.CheckBox
        Me.TabPage_MetaEvo = New System.Windows.Forms.TabPage
        Me.GroupBox_MetaEvo_TransferOptions = New System.Windows.Forms.GroupBox
        Me.Numeric_MetaEvo_NumberResults = New System.Windows.Forms.NumericUpDown
        Me.GroupBox_MetaEvo_LocalOptions = New System.Windows.Forms.GroupBox
        Me.Numeric_MetaEvo_HJStepsize = New System.Windows.Forms.NumericUpDown
        Me.Combo_MetaEvo_OpMode = New System.Windows.Forms.ComboBox
        Me.GroupBox_MetaEvo_MySQLOptions = New System.Windows.Forms.GroupBox
        Me.TextBox_MetaEvo_MySQL_DB = New System.Windows.Forms.TextBox
        Me.TextBox_MetaEvo_MySQL_Password = New System.Windows.Forms.TextBox
        Me.TextBox_MetaEvo_MySQL_User = New System.Windows.Forms.TextBox
        Me.TextBox_MetaEvo_MySQL_Host = New System.Windows.Forms.TextBox
        Me.GroupBox_MetaEvo_BasicOptions = New System.Windows.Forms.GroupBox
        Me.Numeric_MetaEvo_PopulationSize = New System.Windows.Forms.NumericUpDown
        Me.Numeric_MetaEvo_Numbergenerations = New System.Windows.Forms.NumericUpDown
        Me.Combo_MetaEvo_Role = New System.Windows.Forms.ComboBox
        Me.TabPage_DDS = New System.Windows.Forms.TabPage
        Me.CheckBox_DDS_ini = New System.Windows.Forms.CheckBox
        Me.Numeric_DDS_maxiter = New System.Windows.Forms.NumericUpDown
        Me.Numeric_DDS_r_val = New System.Windows.Forms.NumericUpDown
        Me.TabPage_SensiPlot = New System.Windows.Forms.TabPage
        Me.SensiPlot_CheckBox_wave = New System.Windows.Forms.CheckBox
        Me.SensiPlot_Label_NumSteps = New System.Windows.Forms.Label
        Me.SensiPlot_NumericUpDown_NumSteps = New System.Windows.Forms.NumericUpDown
        Me.SensiPlot_GroupBox_Modus = New System.Windows.Forms.GroupBox
        Me.SensiPlot_RadioButton_Discrete = New System.Windows.Forms.RadioButton
        Me.SensiPlot_RadioButton_NormalDistribution = New System.Windows.Forms.RadioButton
        Me.SensiPlot_Label_Objectives = New System.Windows.Forms.Label
        Me.SensiPlot_ListBox_Objectives = New System.Windows.Forms.ListBox
        Me.SensiPlot_Label_OptParameter = New System.Windows.Forms.Label
        Me.SensiPlot_ListBox_OptParameter = New System.Windows.Forms.ListBox
        Me.TabPage_TSP = New System.Windows.Forms.TabPage
        Me.TSP_ComboBox_prob_instance = New System.Windows.Forms.ComboBox
        Me.TSP_Label_Instance = New System.Windows.Forms.Label
        Me.TSP_ComboBox_Mutationoperator = New System.Windows.Forms.ComboBox
        Me.TSP_Label_Mutationoperator = New System.Windows.Forms.Label
        Me.TSP_Label_Reproductionoperator = New System.Windows.Forms.Label
        Me.TSP_ComboBox_Reproductionoperator = New System.Windows.Forms.ComboBox
        Me.TSP_Numeric_n_generations = New System.Windows.Forms.NumericUpDown
        Me.TSP_Label_n_generations = New System.Windows.Forms.Label
        Me.TSP_Numeric_n_children = New System.Windows.Forms.NumericUpDown
        Me.TSP_Label_n_children = New System.Windows.Forms.Label
        Me.TSP_Numeric_n_parents = New System.Windows.Forms.NumericUpDown
        Me.TSP_Label_n_parents = New System.Windows.Forms.Label
        Me.TSP_Numeric_n_cities = New System.Windows.Forms.NumericUpDown
        Me.TSP_Label_n_cities = New System.Windows.Forms.Label
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
        Label3 = New System.Windows.Forms.Label
        Label1 = New System.Windows.Forms.Label
        Label11 = New System.Windows.Forms.Label
        Label5 = New System.Windows.Forms.Label
        Label10 = New System.Windows.Forms.Label
        Label13 = New System.Windows.Forms.Label
        Label14 = New System.Windows.Forms.Label
        Label15 = New System.Windows.Forms.Label
        Label16 = New System.Windows.Forms.Label
        Label18 = New System.Windows.Forms.Label
        Label20 = New System.Windows.Forms.Label
        Label19 = New System.Windows.Forms.Label
        Label_Meta11 = New System.Windows.Forms.Label
        Label_Meta5 = New System.Windows.Forms.Label
        Label_Meta10 = New System.Windows.Forms.Label
        GroupBox_Diagramm = New System.Windows.Forms.GroupBox
        GroupBox_Diagramm.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage_General.SuspendLayout()
        Me.GroupBox_Sim.SuspendLayout()
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
        Me.TabPage_MetaEvo.SuspendLayout()
        Me.GroupBox_MetaEvo_TransferOptions.SuspendLayout()
        CType(Me.Numeric_MetaEvo_NumberResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_MetaEvo_LocalOptions.SuspendLayout()
        CType(Me.Numeric_MetaEvo_HJStepsize, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_MetaEvo_MySQLOptions.SuspendLayout()
        Me.GroupBox_MetaEvo_BasicOptions.SuspendLayout()
        CType(Me.Numeric_MetaEvo_PopulationSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_MetaEvo_Numbergenerations, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage_DDS.SuspendLayout()
        CType(Me.Numeric_DDS_maxiter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Numeric_DDS_r_val, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage_SensiPlot.SuspendLayout()
        CType(Me.SensiPlot_NumericUpDown_NumSteps, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SensiPlot_GroupBox_Modus.SuspendLayout()
        Me.TabPage_TSP.SuspendLayout()
        CType(Me.TSP_Numeric_n_generations, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TSP_Numeric_n_children, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TSP_Numeric_n_parents, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TSP_Numeric_n_cities, System.ComponentModel.ISupportInitialize).BeginInit()
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
        'Label11
        '
        Label11.AutoSize = True
        Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Label11.Location = New System.Drawing.Point(6, 66)
        Label11.Name = "Label11"
        Label11.Size = New System.Drawing.Size(110, 13)
        Label11.TabIndex = 4
        Label11.Text = "Random initial values:"
        '
        'Label5
        '
        Label5.AutoSize = True
        Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Label5.Location = New System.Drawing.Point(6, 18)
        Label5.Name = "Label5"
        Label5.Size = New System.Drawing.Size(154, 13)
        Label5.TabIndex = 0
        Label5.Text = "Neighborhood pertubation size:"
        '
        'Label10
        '
        Label10.AutoSize = True
        Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Label10.Location = New System.Drawing.Point(6, 44)
        Label10.Name = "Label10"
        Label10.Size = New System.Drawing.Size(78, 13)
        Label10.TabIndex = 2
        Label10.Text = "Max. iterations:"
        '
        'Label13
        '
        Label13.AutoSize = True
        Label13.Location = New System.Drawing.Point(4, 22)
        Label13.Name = "Label13"
        Label13.Size = New System.Drawing.Size(32, 13)
        Label13.TabIndex = 51
        Label13.Text = "Host:"
        '
        'Label14
        '
        Label14.AutoSize = True
        Label14.Location = New System.Drawing.Point(4, 74)
        Label14.Name = "Label14"
        Label14.Size = New System.Drawing.Size(30, 13)
        Label14.TabIndex = 35
        Label14.Text = "user:"
        '
        'Label15
        '
        Label15.AutoSize = True
        Label15.Location = New System.Drawing.Point(4, 100)
        Label15.Name = "Label15"
        Label15.Size = New System.Drawing.Size(55, 13)
        Label15.TabIndex = 48
        Label15.Text = "password:"
        '
        'Label16
        '
        Label16.AutoSize = True
        Label16.Location = New System.Drawing.Point(4, 48)
        Label16.Name = "Label16"
        Label16.Size = New System.Drawing.Size(54, 13)
        Label16.TabIndex = 55
        Label16.Text = "DB-name:"
        '
        'Label18
        '
        Label18.AutoSize = True
        Label18.Location = New System.Drawing.Point(15, 46)
        Label18.Name = "Label18"
        Label18.Size = New System.Drawing.Size(86, 13)
        Label18.TabIndex = 54
        Label18.Text = "Operation Mode:"
        '
        'Label20
        '
        Label20.AutoSize = True
        Label20.Location = New System.Drawing.Point(4, 16)
        Label20.Name = "Label20"
        Label20.Size = New System.Drawing.Size(135, 13)
        Label20.TabIndex = 51
        Label20.Text = "HJ minimum Stepsize:     1/"
        '
        'Label19
        '
        Label19.AutoSize = True
        Label19.Location = New System.Drawing.Point(4, 16)
        Label19.Name = "Label19"
        Label19.Size = New System.Drawing.Size(115, 26)
        Label19.TabIndex = 57
        Label19.Text = "Number of results:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(<= Number of parents)"
        '
        'Label_Meta11
        '
        Label_Meta11.AutoSize = True
        Label_Meta11.Location = New System.Drawing.Point(4, 57)
        Label_Meta11.Name = "Label_Meta11"
        Label_Meta11.Size = New System.Drawing.Size(117, 13)
        Label_Meta11.TabIndex = 35
        Label_Meta11.Text = "Number of generations:"
        '
        'Label_Meta5
        '
        Label_Meta5.AutoSize = True
        Label_Meta5.Location = New System.Drawing.Point(15, 19)
        Label_Meta5.Name = "Label_Meta5"
        Label_Meta5.Size = New System.Drawing.Size(49, 13)
        Label_Meta5.TabIndex = 51
        Label_Meta5.Text = "PC Role:"
        '
        'Label_Meta10
        '
        Label_Meta10.AutoSize = True
        Label_Meta10.Location = New System.Drawing.Point(4, 22)
        Label_Meta10.Name = "Label_Meta10"
        Label_Meta10.Size = New System.Drawing.Size(97, 26)
        Label_Meta10.TabIndex = 51
        Label_Meta10.Text = "Number of parents:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(1:3 Children)"
        '
        'GroupBox_Diagramm
        '
        GroupBox_Diagramm.Controls.Add(Me.CheckBox_drawOnlyCurrentGen)
        GroupBox_Diagramm.Location = New System.Drawing.Point(6, 65)
        GroupBox_Diagramm.Name = "GroupBox_Diagramm"
        GroupBox_Diagramm.Size = New System.Drawing.Size(209, 62)
        GroupBox_Diagramm.TabIndex = 4
        GroupBox_Diagramm.TabStop = False
        GroupBox_Diagramm.Text = "Diagramm"
        '
        'CheckBox_drawOnlyCurrentGen
        '
        Me.CheckBox_drawOnlyCurrentGen.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox_drawOnlyCurrentGen.Location = New System.Drawing.Point(6, 19)
        Me.CheckBox_drawOnlyCurrentGen.Name = "CheckBox_drawOnlyCurrentGen"
        Me.CheckBox_drawOnlyCurrentGen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_drawOnlyCurrentGen.Size = New System.Drawing.Size(197, 37)
        Me.CheckBox_drawOnlyCurrentGen.TabIndex = 1
        Me.CheckBox_drawOnlyCurrentGen.Text = "Nur die aktuelle Generation anzeigen:"
        Me.CheckBox_drawOnlyCurrentGen.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage_General)
        Me.TabControl1.Controls.Add(Me.TabPage_PES)
        Me.TabControl1.Controls.Add(Me.TabPage_CES)
        Me.TabControl1.Controls.Add(Me.TabPage_HookeJeeves)
        Me.TabControl1.Controls.Add(Me.TabPage_MetaEvo)
        Me.TabControl1.Controls.Add(Me.TabPage_DDS)
        Me.TabControl1.Controls.Add(Me.TabPage_SensiPlot)
        Me.TabControl1.Controls.Add(Me.TabPage_TSP)
        Me.TabControl1.Location = New System.Drawing.Point(2, 16)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.Padding = New System.Drawing.Point(0, 0)
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(229, 694)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage_General
        '
        Me.TabPage_General.Controls.Add(GroupBox_Diagramm)
        Me.TabPage_General.Controls.Add(Me.GroupBox_Sim)
        Me.TabPage_General.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_General.Name = "TabPage_General"
        Me.TabPage_General.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_General.Size = New System.Drawing.Size(221, 668)
        Me.TabPage_General.TabIndex = 6
        Me.TabPage_General.Text = "General"
        Me.TabPage_General.UseVisualStyleBackColor = True
        '
        'GroupBox_Sim
        '
        Me.GroupBox_Sim.Controls.Add(Me.CheckBox_useMultithreading)
        Me.GroupBox_Sim.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox_Sim.Name = "GroupBox_Sim"
        Me.GroupBox_Sim.Size = New System.Drawing.Size(209, 53)
        Me.GroupBox_Sim.TabIndex = 3
        Me.GroupBox_Sim.TabStop = False
        Me.GroupBox_Sim.Text = "Simulationen"
        '
        'CheckBox_useMultithreading
        '
        Me.CheckBox_useMultithreading.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox_useMultithreading.Location = New System.Drawing.Point(6, 20)
        Me.CheckBox_useMultithreading.Name = "CheckBox_useMultithreading"
        Me.CheckBox_useMultithreading.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_useMultithreading.Size = New System.Drawing.Size(197, 24)
        Me.CheckBox_useMultithreading.TabIndex = 0
        Me.CheckBox_useMultithreading.Text = "Multithreading benutzen:"
        Me.CheckBox_useMultithreading.UseVisualStyleBackColor = True
        '
        'TabPage_PES
        '
        Me.TabPage_PES.AutoScroll = True
        Me.TabPage_PES.BackColor = System.Drawing.Color.Transparent
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
        Me.TabPage_PES.Size = New System.Drawing.Size(221, 668)
        Me.TabPage_PES.TabIndex = 0
        Me.TabPage_PES.Text = "PES"
        Me.TabPage_PES.UseVisualStyleBackColor = True
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
        Me.TabPage_CES.BackColor = System.Drawing.Color.Transparent
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
        Me.TabPage_CES.Size = New System.Drawing.Size(221, 668)
        Me.TabPage_CES.TabIndex = 1
        Me.TabPage_CES.Text = "CES"
        Me.TabPage_CES.UseVisualStyleBackColor = True
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
        Me.Numeric_CES_n_childs.Maximum = New Decimal(New Integer() {1000000000, 0, 0, 0})
        Me.Numeric_CES_n_childs.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
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
        Me.TabPage_HookeJeeves.BackColor = System.Drawing.Color.Transparent
        Me.TabPage_HookeJeeves.Controls.Add(Label3)
        Me.TabPage_HookeJeeves.Controls.Add(Me.Numeric_HJ_DeltaFinish)
        Me.TabPage_HookeJeeves.Controls.Add(Label1)
        Me.TabPage_HookeJeeves.Controls.Add(Me.Numeric_HJ_DeltaStart)
        Me.TabPage_HookeJeeves.Controls.Add(Me.CheckBox_HJ_DNVektor)
        Me.TabPage_HookeJeeves.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage_HookeJeeves.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_HookeJeeves.Name = "TabPage_HookeJeeves"
        Me.TabPage_HookeJeeves.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_HookeJeeves.Size = New System.Drawing.Size(221, 668)
        Me.TabPage_HookeJeeves.TabIndex = 2
        Me.TabPage_HookeJeeves.Text = "Hooke&Jeeves"
        Me.TabPage_HookeJeeves.UseVisualStyleBackColor = True
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
        'TabPage_MetaEvo
        '
        Me.TabPage_MetaEvo.BackColor = System.Drawing.Color.Transparent
        Me.TabPage_MetaEvo.Controls.Add(Me.GroupBox_MetaEvo_TransferOptions)
        Me.TabPage_MetaEvo.Controls.Add(Me.GroupBox_MetaEvo_LocalOptions)
        Me.TabPage_MetaEvo.Controls.Add(Label18)
        Me.TabPage_MetaEvo.Controls.Add(Me.Combo_MetaEvo_OpMode)
        Me.TabPage_MetaEvo.Controls.Add(Me.GroupBox_MetaEvo_MySQLOptions)
        Me.TabPage_MetaEvo.Controls.Add(Label_Meta5)
        Me.TabPage_MetaEvo.Controls.Add(Me.GroupBox_MetaEvo_BasicOptions)
        Me.TabPage_MetaEvo.Controls.Add(Me.Combo_MetaEvo_Role)
        Me.TabPage_MetaEvo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage_MetaEvo.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_MetaEvo.Name = "TabPage_MetaEvo"
        Me.TabPage_MetaEvo.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_MetaEvo.Size = New System.Drawing.Size(221, 668)
        Me.TabPage_MetaEvo.TabIndex = 3
        Me.TabPage_MetaEvo.Text = "MetaEvo"
        Me.TabPage_MetaEvo.UseVisualStyleBackColor = True
        '
        'GroupBox_MetaEvo_TransferOptions
        '
        Me.GroupBox_MetaEvo_TransferOptions.Controls.Add(Label19)
        Me.GroupBox_MetaEvo_TransferOptions.Controls.Add(Me.Numeric_MetaEvo_NumberResults)
        Me.GroupBox_MetaEvo_TransferOptions.Location = New System.Drawing.Point(6, 159)
        Me.GroupBox_MetaEvo_TransferOptions.Name = "GroupBox_MetaEvo_TransferOptions"
        Me.GroupBox_MetaEvo_TransferOptions.Size = New System.Drawing.Size(200, 48)
        Me.GroupBox_MetaEvo_TransferOptions.TabIndex = 54
        Me.GroupBox_MetaEvo_TransferOptions.TabStop = False
        Me.GroupBox_MetaEvo_TransferOptions.Text = "Transfer Options"
        '
        'Numeric_MetaEvo_NumberResults
        '
        Me.Numeric_MetaEvo_NumberResults.Location = New System.Drawing.Point(139, 14)
        Me.Numeric_MetaEvo_NumberResults.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
        Me.Numeric_MetaEvo_NumberResults.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.Numeric_MetaEvo_NumberResults.Name = "Numeric_MetaEvo_NumberResults"
        Me.Numeric_MetaEvo_NumberResults.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_MetaEvo_NumberResults.TabIndex = 56
        Me.Numeric_MetaEvo_NumberResults.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_MetaEvo_NumberResults.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'GroupBox_MetaEvo_LocalOptions
        '
        Me.GroupBox_MetaEvo_LocalOptions.Controls.Add(Label20)
        Me.GroupBox_MetaEvo_LocalOptions.Controls.Add(Me.Numeric_MetaEvo_HJStepsize)
        Me.GroupBox_MetaEvo_LocalOptions.Enabled = False
        Me.GroupBox_MetaEvo_LocalOptions.Location = New System.Drawing.Point(6, 213)
        Me.GroupBox_MetaEvo_LocalOptions.Name = "GroupBox_MetaEvo_LocalOptions"
        Me.GroupBox_MetaEvo_LocalOptions.Size = New System.Drawing.Size(200, 40)
        Me.GroupBox_MetaEvo_LocalOptions.TabIndex = 53
        Me.GroupBox_MetaEvo_LocalOptions.TabStop = False
        Me.GroupBox_MetaEvo_LocalOptions.Text = "Local Options"
        '
        'Numeric_MetaEvo_HJStepsize
        '
        Me.Numeric_MetaEvo_HJStepsize.Location = New System.Drawing.Point(139, 13)
        Me.Numeric_MetaEvo_HJStepsize.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
        Me.Numeric_MetaEvo_HJStepsize.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.Numeric_MetaEvo_HJStepsize.Name = "Numeric_MetaEvo_HJStepsize"
        Me.Numeric_MetaEvo_HJStepsize.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_MetaEvo_HJStepsize.TabIndex = 50
        Me.Numeric_MetaEvo_HJStepsize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_MetaEvo_HJStepsize.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Combo_MetaEvo_OpMode
        '
        Me.Combo_MetaEvo_OpMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Combo_MetaEvo_OpMode.FormattingEnabled = True
        Me.Combo_MetaEvo_OpMode.Items.AddRange(New Object() {"Local Optimizer", "Global Optimizer", "Both"})
        Me.Combo_MetaEvo_OpMode.Location = New System.Drawing.Point(103, 43)
        Me.Combo_MetaEvo_OpMode.Name = "Combo_MetaEvo_OpMode"
        Me.Combo_MetaEvo_OpMode.Size = New System.Drawing.Size(100, 21)
        Me.Combo_MetaEvo_OpMode.TabIndex = 53
        '
        'GroupBox_MetaEvo_MySQLOptions
        '
        Me.GroupBox_MetaEvo_MySQLOptions.Controls.Add(Me.TextBox_MetaEvo_MySQL_DB)
        Me.GroupBox_MetaEvo_MySQLOptions.Controls.Add(Label16)
        Me.GroupBox_MetaEvo_MySQLOptions.Controls.Add(Me.TextBox_MetaEvo_MySQL_Password)
        Me.GroupBox_MetaEvo_MySQLOptions.Controls.Add(Me.TextBox_MetaEvo_MySQL_User)
        Me.GroupBox_MetaEvo_MySQLOptions.Controls.Add(Me.TextBox_MetaEvo_MySQL_Host)
        Me.GroupBox_MetaEvo_MySQLOptions.Controls.Add(Label13)
        Me.GroupBox_MetaEvo_MySQLOptions.Controls.Add(Label14)
        Me.GroupBox_MetaEvo_MySQLOptions.Controls.Add(Label15)
        Me.GroupBox_MetaEvo_MySQLOptions.Enabled = False
        Me.GroupBox_MetaEvo_MySQLOptions.Location = New System.Drawing.Point(6, 259)
        Me.GroupBox_MetaEvo_MySQLOptions.Name = "GroupBox_MetaEvo_MySQLOptions"
        Me.GroupBox_MetaEvo_MySQLOptions.Size = New System.Drawing.Size(200, 125)
        Me.GroupBox_MetaEvo_MySQLOptions.TabIndex = 52
        Me.GroupBox_MetaEvo_MySQLOptions.TabStop = False
        Me.GroupBox_MetaEvo_MySQLOptions.Text = "MySQL Options"
        '
        'TextBox_MetaEvo_MySQL_DB
        '
        Me.TextBox_MetaEvo_MySQL_DB.Location = New System.Drawing.Point(92, 45)
        Me.TextBox_MetaEvo_MySQL_DB.Name = "TextBox_MetaEvo_MySQL_DB"
        Me.TextBox_MetaEvo_MySQL_DB.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_MetaEvo_MySQL_DB.TabIndex = 56
        '
        'TextBox_MetaEvo_MySQL_Password
        '
        Me.TextBox_MetaEvo_MySQL_Password.Location = New System.Drawing.Point(92, 97)
        Me.TextBox_MetaEvo_MySQL_Password.Name = "TextBox_MetaEvo_MySQL_Password"
        Me.TextBox_MetaEvo_MySQL_Password.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_MetaEvo_MySQL_Password.TabIndex = 54
        Me.TextBox_MetaEvo_MySQL_Password.UseSystemPasswordChar = True
        '
        'TextBox_MetaEvo_MySQL_User
        '
        Me.TextBox_MetaEvo_MySQL_User.Location = New System.Drawing.Point(92, 71)
        Me.TextBox_MetaEvo_MySQL_User.Name = "TextBox_MetaEvo_MySQL_User"
        Me.TextBox_MetaEvo_MySQL_User.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_MetaEvo_MySQL_User.TabIndex = 53
        '
        'TextBox_MetaEvo_MySQL_Host
        '
        Me.TextBox_MetaEvo_MySQL_Host.Location = New System.Drawing.Point(92, 19)
        Me.TextBox_MetaEvo_MySQL_Host.Name = "TextBox_MetaEvo_MySQL_Host"
        Me.TextBox_MetaEvo_MySQL_Host.Size = New System.Drawing.Size(100, 20)
        Me.TextBox_MetaEvo_MySQL_Host.TabIndex = 52
        '
        'GroupBox_MetaEvo_BasicOptions
        '
        Me.GroupBox_MetaEvo_BasicOptions.Controls.Add(Label_Meta10)
        Me.GroupBox_MetaEvo_BasicOptions.Controls.Add(Me.Numeric_MetaEvo_PopulationSize)
        Me.GroupBox_MetaEvo_BasicOptions.Controls.Add(Label_Meta11)
        Me.GroupBox_MetaEvo_BasicOptions.Controls.Add(Me.Numeric_MetaEvo_Numbergenerations)
        Me.GroupBox_MetaEvo_BasicOptions.Enabled = False
        Me.GroupBox_MetaEvo_BasicOptions.Location = New System.Drawing.Point(6, 70)
        Me.GroupBox_MetaEvo_BasicOptions.Name = "GroupBox_MetaEvo_BasicOptions"
        Me.GroupBox_MetaEvo_BasicOptions.Size = New System.Drawing.Size(200, 83)
        Me.GroupBox_MetaEvo_BasicOptions.TabIndex = 36
        Me.GroupBox_MetaEvo_BasicOptions.TabStop = False
        Me.GroupBox_MetaEvo_BasicOptions.Text = "Global Options"
        '
        'Numeric_MetaEvo_PopulationSize
        '
        Me.Numeric_MetaEvo_PopulationSize.Location = New System.Drawing.Point(139, 19)
        Me.Numeric_MetaEvo_PopulationSize.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
        Me.Numeric_MetaEvo_PopulationSize.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.Numeric_MetaEvo_PopulationSize.Name = "Numeric_MetaEvo_PopulationSize"
        Me.Numeric_MetaEvo_PopulationSize.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_MetaEvo_PopulationSize.TabIndex = 50
        Me.Numeric_MetaEvo_PopulationSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_MetaEvo_PopulationSize.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Numeric_MetaEvo_Numbergenerations
        '
        Me.Numeric_MetaEvo_Numbergenerations.Location = New System.Drawing.Point(139, 54)
        Me.Numeric_MetaEvo_Numbergenerations.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
        Me.Numeric_MetaEvo_Numbergenerations.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.Numeric_MetaEvo_Numbergenerations.Name = "Numeric_MetaEvo_Numbergenerations"
        Me.Numeric_MetaEvo_Numbergenerations.Size = New System.Drawing.Size(53, 20)
        Me.Numeric_MetaEvo_Numbergenerations.TabIndex = 34
        Me.Numeric_MetaEvo_Numbergenerations.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Numeric_MetaEvo_Numbergenerations.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Combo_MetaEvo_Role
        '
        Me.Combo_MetaEvo_Role.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Combo_MetaEvo_Role.FormattingEnabled = True
        Me.Combo_MetaEvo_Role.Items.AddRange(New Object() {"Single PC", "Network Server", "Network Client"})
        Me.Combo_MetaEvo_Role.Location = New System.Drawing.Point(103, 16)
        Me.Combo_MetaEvo_Role.Name = "Combo_MetaEvo_Role"
        Me.Combo_MetaEvo_Role.Size = New System.Drawing.Size(100, 21)
        Me.Combo_MetaEvo_Role.TabIndex = 0
        '
        'TabPage_DDS
        '
        Me.TabPage_DDS.Controls.Add(Me.CheckBox_DDS_ini)
        Me.TabPage_DDS.Controls.Add(Label11)
        Me.TabPage_DDS.Controls.Add(Me.Numeric_DDS_maxiter)
        Me.TabPage_DDS.Controls.Add(Label10)
        Me.TabPage_DDS.Controls.Add(Me.Numeric_DDS_r_val)
        Me.TabPage_DDS.Controls.Add(Label5)
        Me.TabPage_DDS.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_DDS.Name = "TabPage_DDS"
        Me.TabPage_DDS.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_DDS.Size = New System.Drawing.Size(221, 668)
        Me.TabPage_DDS.TabIndex = 4
        Me.TabPage_DDS.Text = "DDS"
        Me.TabPage_DDS.UseVisualStyleBackColor = True
        '
        'CheckBox_DDS_ini
        '
        Me.CheckBox_DDS_ini.AutoSize = True
        Me.CheckBox_DDS_ini.Checked = True
        Me.CheckBox_DDS_ini.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox_DDS_ini.Location = New System.Drawing.Point(161, 66)
        Me.CheckBox_DDS_ini.Name = "CheckBox_DDS_ini"
        Me.CheckBox_DDS_ini.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox_DDS_ini.TabIndex = 5
        Me.CheckBox_DDS_ini.UseVisualStyleBackColor = True
        '
        'Numeric_DDS_maxiter
        '
        Me.Numeric_DDS_maxiter.Increment = New Decimal(New Integer() {100, 0, 0, 0})
        Me.Numeric_DDS_maxiter.Location = New System.Drawing.Point(161, 42)
        Me.Numeric_DDS_maxiter.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.Numeric_DDS_maxiter.Minimum = New Decimal(New Integer() {100, 0, 0, 0})
        Me.Numeric_DDS_maxiter.Name = "Numeric_DDS_maxiter"
        Me.Numeric_DDS_maxiter.Size = New System.Drawing.Size(55, 20)
        Me.Numeric_DDS_maxiter.TabIndex = 3
        Me.Numeric_DDS_maxiter.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'Numeric_DDS_r_val
        '
        Me.Numeric_DDS_r_val.DecimalPlaces = 3
        Me.Numeric_DDS_r_val.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Numeric_DDS_r_val.Increment = New Decimal(New Integer() {25, 0, 0, 196608})
        Me.Numeric_DDS_r_val.Location = New System.Drawing.Point(161, 16)
        Me.Numeric_DDS_r_val.Maximum = New Decimal(New Integer() {10, 0, 0, 65536})
        Me.Numeric_DDS_r_val.Name = "Numeric_DDS_r_val"
        Me.Numeric_DDS_r_val.Size = New System.Drawing.Size(55, 20)
        Me.Numeric_DDS_r_val.TabIndex = 1
        Me.Numeric_DDS_r_val.Value = New Decimal(New Integer() {2, 0, 0, 65536})
        '
        'TabPage_SensiPlot
        '
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_CheckBox_wave)
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_Label_NumSteps)
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_NumericUpDown_NumSteps)
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_GroupBox_Modus)
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_Label_Objectives)
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_ListBox_Objectives)
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_Label_OptParameter)
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_ListBox_OptParameter)
        Me.TabPage_SensiPlot.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_SensiPlot.Name = "TabPage_SensiPlot"
        Me.TabPage_SensiPlot.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_SensiPlot.Size = New System.Drawing.Size(221, 668)
        Me.TabPage_SensiPlot.TabIndex = 5
        Me.TabPage_SensiPlot.Text = "SensiPlot"
        Me.TabPage_SensiPlot.UseVisualStyleBackColor = True
        '
        'SensiPlot_CheckBox_wave
        '
        Me.SensiPlot_CheckBox_wave.AutoSize = True
        Me.SensiPlot_CheckBox_wave.Location = New System.Drawing.Point(9, 495)
        Me.SensiPlot_CheckBox_wave.Name = "SensiPlot_CheckBox_wave"
        Me.SensiPlot_CheckBox_wave.Size = New System.Drawing.Size(101, 17)
        Me.SensiPlot_CheckBox_wave.TabIndex = 15
        Me.SensiPlot_CheckBox_wave.Text = "Wave anzeigen"
        Me.ToolTip1.SetToolTip(Me.SensiPlot_CheckBox_wave, "Im Anschluss an die Sensitivitätsanalyse die Ganglinien aller Simulationen in Wav" & _
                "e anzeigen")
        Me.SensiPlot_CheckBox_wave.UseVisualStyleBackColor = True
        '
        'SensiPlot_Label_NumSteps
        '
        Me.SensiPlot_Label_NumSteps.AutoSize = True
        Me.SensiPlot_Label_NumSteps.Location = New System.Drawing.Point(6, 462)
        Me.SensiPlot_Label_NumSteps.Name = "SensiPlot_Label_NumSteps"
        Me.SensiPlot_Label_NumSteps.Size = New System.Drawing.Size(81, 13)
        Me.SensiPlot_Label_NumSteps.TabIndex = 14
        Me.SensiPlot_Label_NumSteps.Text = "Anzahl Schritte:"
        '
        'SensiPlot_NumericUpDown_NumSteps
        '
        Me.SensiPlot_NumericUpDown_NumSteps.Location = New System.Drawing.Point(93, 459)
        Me.SensiPlot_NumericUpDown_NumSteps.Name = "SensiPlot_NumericUpDown_NumSteps"
        Me.SensiPlot_NumericUpDown_NumSteps.Size = New System.Drawing.Size(56, 20)
        Me.SensiPlot_NumericUpDown_NumSteps.TabIndex = 13
        Me.SensiPlot_NumericUpDown_NumSteps.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'SensiPlot_GroupBox_Modus
        '
        Me.SensiPlot_GroupBox_Modus.Controls.Add(Me.SensiPlot_RadioButton_Discrete)
        Me.SensiPlot_GroupBox_Modus.Controls.Add(Me.SensiPlot_RadioButton_NormalDistribution)
        Me.SensiPlot_GroupBox_Modus.Location = New System.Drawing.Point(6, 380)
        Me.SensiPlot_GroupBox_Modus.Name = "SensiPlot_GroupBox_Modus"
        Me.SensiPlot_GroupBox_Modus.Size = New System.Drawing.Size(209, 70)
        Me.SensiPlot_GroupBox_Modus.TabIndex = 12
        Me.SensiPlot_GroupBox_Modus.TabStop = False
        Me.SensiPlot_GroupBox_Modus.Text = "Modus"
        '
        'SensiPlot_RadioButton_Discrete
        '
        Me.SensiPlot_RadioButton_Discrete.AutoSize = True
        Me.SensiPlot_RadioButton_Discrete.Checked = True
        Me.SensiPlot_RadioButton_Discrete.Location = New System.Drawing.Point(9, 42)
        Me.SensiPlot_RadioButton_Discrete.Name = "SensiPlot_RadioButton_Discrete"
        Me.SensiPlot_RadioButton_Discrete.Size = New System.Drawing.Size(58, 17)
        Me.SensiPlot_RadioButton_Discrete.TabIndex = 10
        Me.SensiPlot_RadioButton_Discrete.TabStop = True
        Me.SensiPlot_RadioButton_Discrete.Text = "Diskret"
        Me.SensiPlot_RadioButton_Discrete.UseVisualStyleBackColor = True
        '
        'SensiPlot_RadioButton_NormalDistribution
        '
        Me.SensiPlot_RadioButton_NormalDistribution.AutoSize = True
        Me.SensiPlot_RadioButton_NormalDistribution.Location = New System.Drawing.Point(9, 19)
        Me.SensiPlot_RadioButton_NormalDistribution.Name = "SensiPlot_RadioButton_NormalDistribution"
        Me.SensiPlot_RadioButton_NormalDistribution.Size = New System.Drawing.Size(86, 17)
        Me.SensiPlot_RadioButton_NormalDistribution.TabIndex = 9
        Me.SensiPlot_RadioButton_NormalDistribution.Text = "Gleichverteilt"
        Me.SensiPlot_RadioButton_NormalDistribution.UseVisualStyleBackColor = True
        '
        'SensiPlot_Label_Objectives
        '
        Me.SensiPlot_Label_Objectives.AutoSize = True
        Me.SensiPlot_Label_Objectives.Location = New System.Drawing.Point(6, 198)
        Me.SensiPlot_Label_Objectives.Name = "SensiPlot_Label_Objectives"
        Me.SensiPlot_Label_Objectives.Size = New System.Drawing.Size(116, 13)
        Me.SensiPlot_Label_Objectives.TabIndex = 6
        Me.SensiPlot_Label_Objectives.Text = "Objective functions: (1)"
        '
        'SensiPlot_ListBox_Objectives
        '
        Me.SensiPlot_ListBox_Objectives.FormattingEnabled = True
        Me.SensiPlot_ListBox_Objectives.Location = New System.Drawing.Point(6, 214)
        Me.SensiPlot_ListBox_Objectives.Name = "SensiPlot_ListBox_Objectives"
        Me.SensiPlot_ListBox_Objectives.Size = New System.Drawing.Size(209, 160)
        Me.SensiPlot_ListBox_Objectives.TabIndex = 5
        '
        'SensiPlot_Label_OptParameter
        '
        Me.SensiPlot_Label_OptParameter.AutoSize = True
        Me.SensiPlot_Label_OptParameter.Location = New System.Drawing.Point(6, 3)
        Me.SensiPlot_Label_OptParameter.Name = "SensiPlot_Label_OptParameter"
        Me.SensiPlot_Label_OptParameter.Size = New System.Drawing.Size(115, 13)
        Me.SensiPlot_Label_OptParameter.TabIndex = 4
        Me.SensiPlot_Label_OptParameter.Text = "OptParameter: (1 bis 2)"
        '
        'SensiPlot_ListBox_OptParameter
        '
        Me.SensiPlot_ListBox_OptParameter.Location = New System.Drawing.Point(6, 22)
        Me.SensiPlot_ListBox_OptParameter.Name = "SensiPlot_ListBox_OptParameter"
        Me.SensiPlot_ListBox_OptParameter.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.SensiPlot_ListBox_OptParameter.Size = New System.Drawing.Size(209, 173)
        Me.SensiPlot_ListBox_OptParameter.TabIndex = 3
        '
        'TabPage_TSP
        '
        Me.TabPage_TSP.Controls.Add(Me.TSP_ComboBox_prob_instance)
        Me.TabPage_TSP.Controls.Add(Me.TSP_Label_Instance)
        Me.TabPage_TSP.Controls.Add(Me.TSP_ComboBox_Mutationoperator)
        Me.TabPage_TSP.Controls.Add(Me.TSP_Label_Mutationoperator)
        Me.TabPage_TSP.Controls.Add(Me.TSP_Label_Reproductionoperator)
        Me.TabPage_TSP.Controls.Add(Me.TSP_ComboBox_Reproductionoperator)
        Me.TabPage_TSP.Controls.Add(Me.TSP_Numeric_n_generations)
        Me.TabPage_TSP.Controls.Add(Me.TSP_Label_n_generations)
        Me.TabPage_TSP.Controls.Add(Me.TSP_Numeric_n_children)
        Me.TabPage_TSP.Controls.Add(Me.TSP_Label_n_children)
        Me.TabPage_TSP.Controls.Add(Me.TSP_Numeric_n_parents)
        Me.TabPage_TSP.Controls.Add(Me.TSP_Label_n_parents)
        Me.TabPage_TSP.Controls.Add(Me.TSP_Numeric_n_cities)
        Me.TabPage_TSP.Controls.Add(Me.TSP_Label_n_cities)
        Me.TabPage_TSP.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_TSP.Name = "TabPage_TSP"
        Me.TabPage_TSP.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_TSP.Size = New System.Drawing.Size(221, 668)
        Me.TabPage_TSP.TabIndex = 7
        Me.TabPage_TSP.Text = "TSP"
        Me.TabPage_TSP.UseVisualStyleBackColor = True
        '
        'TSP_ComboBox_prob_instance
        '
        Me.TSP_ComboBox_prob_instance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.TSP_ComboBox_prob_instance.FormattingEnabled = True
        Me.TSP_ComboBox_prob_instance.Items.AddRange(New Object() {"Random", "Circle"})
        Me.TSP_ComboBox_prob_instance.Location = New System.Drawing.Point(105, 39)
        Me.TSP_ComboBox_prob_instance.Name = "TSP_ComboBox_prob_instance"
        Me.TSP_ComboBox_prob_instance.Size = New System.Drawing.Size(110, 21)
        Me.TSP_ComboBox_prob_instance.TabIndex = 57
        '
        'TSP_Label_Instance
        '
        Me.TSP_Label_Instance.AutoSize = True
        Me.TSP_Label_Instance.Location = New System.Drawing.Point(10, 42)
        Me.TSP_Label_Instance.Name = "TSP_Label_Instance"
        Me.TSP_Label_Instance.Size = New System.Drawing.Size(92, 13)
        Me.TSP_Label_Instance.TabIndex = 56
        Me.TSP_Label_Instance.Text = "Problem Instance:"
        '
        'TSP_ComboBox_Mutationoperator
        '
        Me.TSP_ComboBox_Mutationoperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.TSP_ComboBox_Mutationoperator.FormattingEnabled = True
        Me.TSP_ComboBox_Mutationoperator.Items.AddRange(New Object() {"Local Optimizer", "Global Optimizer", "Both"})
        Me.TSP_ComboBox_Mutationoperator.Location = New System.Drawing.Point(81, 189)
        Me.TSP_ComboBox_Mutationoperator.Name = "TSP_ComboBox_Mutationoperator"
        Me.TSP_ComboBox_Mutationoperator.Size = New System.Drawing.Size(134, 21)
        Me.TSP_ComboBox_Mutationoperator.TabIndex = 57
        '
        'TSP_Label_Mutationoperator
        '
        Me.TSP_Label_Mutationoperator.AutoSize = True
        Me.TSP_Label_Mutationoperator.Location = New System.Drawing.Point(10, 192)
        Me.TSP_Label_Mutationoperator.Name = "TSP_Label_Mutationoperator"
        Me.TSP_Label_Mutationoperator.Size = New System.Drawing.Size(68, 13)
        Me.TSP_Label_Mutationoperator.TabIndex = 56
        Me.TSP_Label_Mutationoperator.Text = "MutationOp.:"
        '
        'TSP_Label_Reproductionoperator
        '
        Me.TSP_Label_Reproductionoperator.AutoSize = True
        Me.TSP_Label_Reproductionoperator.Location = New System.Drawing.Point(10, 165)
        Me.TSP_Label_Reproductionoperator.Name = "TSP_Label_Reproductionoperator"
        Me.TSP_Label_Reproductionoperator.Size = New System.Drawing.Size(62, 13)
        Me.TSP_Label_Reproductionoperator.TabIndex = 55
        Me.TSP_Label_Reproductionoperator.Text = "ReprodOp.:"
        '
        'TSP_ComboBox_Reproductionoperator
        '
        Me.TSP_ComboBox_Reproductionoperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.TSP_ComboBox_Reproductionoperator.FormattingEnabled = True
        Me.TSP_ComboBox_Reproductionoperator.Items.AddRange(New Object() {"PMX", "OX"})
        Me.TSP_ComboBox_Reproductionoperator.Location = New System.Drawing.Point(81, 162)
        Me.TSP_ComboBox_Reproductionoperator.Name = "TSP_ComboBox_Reproductionoperator"
        Me.TSP_ComboBox_Reproductionoperator.Size = New System.Drawing.Size(134, 21)
        Me.TSP_ComboBox_Reproductionoperator.TabIndex = 54
        '
        'TSP_Numeric_n_generations
        '
        Me.TSP_Numeric_n_generations.Location = New System.Drawing.Point(125, 136)
        Me.TSP_Numeric_n_generations.Maximum = New Decimal(New Integer() {-1981284353, -1966660860, 0, 0})
        Me.TSP_Numeric_n_generations.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TSP_Numeric_n_generations.Name = "TSP_Numeric_n_generations"
        Me.TSP_Numeric_n_generations.Size = New System.Drawing.Size(90, 20)
        Me.TSP_Numeric_n_generations.TabIndex = 11
        Me.TSP_Numeric_n_generations.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TSP_Numeric_n_generations.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'TSP_Label_n_generations
        '
        Me.TSP_Label_n_generations.AutoSize = True
        Me.TSP_Label_n_generations.Location = New System.Drawing.Point(10, 138)
        Me.TSP_Label_n_generations.Name = "TSP_Label_n_generations"
        Me.TSP_Label_n_generations.Size = New System.Drawing.Size(99, 13)
        Me.TSP_Label_n_generations.TabIndex = 10
        Me.TSP_Label_n_generations.Text = "No. of Generations:"
        '
        'TSP_Numeric_n_children
        '
        Me.TSP_Numeric_n_children.Location = New System.Drawing.Point(125, 110)
        Me.TSP_Numeric_n_children.Maximum = New Decimal(New Integer() {-727379968, 232, 0, 0})
        Me.TSP_Numeric_n_children.Minimum = New Decimal(New Integer() {7, 0, 0, 0})
        Me.TSP_Numeric_n_children.Name = "TSP_Numeric_n_children"
        Me.TSP_Numeric_n_children.Size = New System.Drawing.Size(90, 20)
        Me.TSP_Numeric_n_children.TabIndex = 9
        Me.TSP_Numeric_n_children.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TSP_Numeric_n_children.Value = New Decimal(New Integer() {7, 0, 0, 0})
        '
        'TSP_Label_n_children
        '
        Me.TSP_Label_n_children.AutoSize = True
        Me.TSP_Label_n_children.Location = New System.Drawing.Point(10, 112)
        Me.TSP_Label_n_children.Name = "TSP_Label_n_children"
        Me.TSP_Label_n_children.Size = New System.Drawing.Size(80, 13)
        Me.TSP_Label_n_children.TabIndex = 8
        Me.TSP_Label_n_children.Text = "No. of Children:"
        '
        'TSP_Numeric_n_parents
        '
        Me.TSP_Numeric_n_parents.Location = New System.Drawing.Point(125, 84)
        Me.TSP_Numeric_n_parents.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.TSP_Numeric_n_parents.Minimum = New Decimal(New Integer() {3, 0, 0, 0})
        Me.TSP_Numeric_n_parents.Name = "TSP_Numeric_n_parents"
        Me.TSP_Numeric_n_parents.Size = New System.Drawing.Size(90, 20)
        Me.TSP_Numeric_n_parents.TabIndex = 7
        Me.TSP_Numeric_n_parents.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TSP_Numeric_n_parents.Value = New Decimal(New Integer() {3, 0, 0, 0})
        '
        'TSP_Label_n_parents
        '
        Me.TSP_Label_n_parents.AutoSize = True
        Me.TSP_Label_n_parents.Location = New System.Drawing.Point(10, 86)
        Me.TSP_Label_n_parents.Name = "TSP_Label_n_parents"
        Me.TSP_Label_n_parents.Size = New System.Drawing.Size(78, 13)
        Me.TSP_Label_n_parents.TabIndex = 6
        Me.TSP_Label_n_parents.Text = "No. of Parents:"
        '
        'TSP_Numeric_n_cities
        '
        Me.TSP_Numeric_n_cities.Location = New System.Drawing.Point(162, 13)
        Me.TSP_Numeric_n_cities.Maximum = New Decimal(New Integer() {1215752192, 23, 0, 0})
        Me.TSP_Numeric_n_cities.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TSP_Numeric_n_cities.Name = "TSP_Numeric_n_cities"
        Me.TSP_Numeric_n_cities.Size = New System.Drawing.Size(53, 20)
        Me.TSP_Numeric_n_cities.TabIndex = 5
        Me.TSP_Numeric_n_cities.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TSP_Numeric_n_cities.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'TSP_Label_n_cities
        '
        Me.TSP_Label_n_cities.AutoSize = True
        Me.TSP_Label_n_cities.Location = New System.Drawing.Point(10, 15)
        Me.TSP_Label_n_cities.Name = "TSP_Label_n_cities"
        Me.TSP_Label_n_cities.Size = New System.Drawing.Size(67, 13)
        Me.TSP_Label_n_cities.TabIndex = 0
        Me.TSP_Label_n_cities.Text = "No. of Cities:"
        '
        'GroupBox_Einstellungen
        '
        Me.GroupBox_Einstellungen.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GroupBox_Einstellungen.Controls.Add(Me.TabControl1)
        Me.GroupBox_Einstellungen.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox_Einstellungen.Name = "GroupBox_Einstellungen"
        Me.GroupBox_Einstellungen.Size = New System.Drawing.Size(234, 713)
        Me.GroupBox_Einstellungen.TabIndex = 0
        Me.GroupBox_Einstellungen.TabStop = False
        Me.GroupBox_Einstellungen.Text = "Einstellungen:"
        '
        'EVO_Einstellungen
        '
        Me.Controls.Add(Me.GroupBox_Einstellungen)
        Me.Name = "EVO_Einstellungen"
        Me.Size = New System.Drawing.Size(244, 753)
        GroupBox_Diagramm.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage_General.ResumeLayout(False)
        Me.GroupBox_Sim.ResumeLayout(False)
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
        Me.TabPage_MetaEvo.ResumeLayout(False)
        Me.TabPage_MetaEvo.PerformLayout()
        Me.GroupBox_MetaEvo_TransferOptions.ResumeLayout(False)
        Me.GroupBox_MetaEvo_TransferOptions.PerformLayout()
        CType(Me.Numeric_MetaEvo_NumberResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_MetaEvo_LocalOptions.ResumeLayout(False)
        Me.GroupBox_MetaEvo_LocalOptions.PerformLayout()
        CType(Me.Numeric_MetaEvo_HJStepsize, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_MetaEvo_MySQLOptions.ResumeLayout(False)
        Me.GroupBox_MetaEvo_MySQLOptions.PerformLayout()
        Me.GroupBox_MetaEvo_BasicOptions.ResumeLayout(False)
        Me.GroupBox_MetaEvo_BasicOptions.PerformLayout()
        CType(Me.Numeric_MetaEvo_PopulationSize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_MetaEvo_Numbergenerations, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage_DDS.ResumeLayout(False)
        Me.TabPage_DDS.PerformLayout()
        CType(Me.Numeric_DDS_maxiter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Numeric_DDS_r_val, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage_SensiPlot.ResumeLayout(False)
        Me.TabPage_SensiPlot.PerformLayout()
        CType(Me.SensiPlot_NumericUpDown_NumSteps, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SensiPlot_GroupBox_Modus.ResumeLayout(False)
        Me.SensiPlot_GroupBox_Modus.PerformLayout()
        Me.TabPage_TSP.ResumeLayout(False)
        Me.TabPage_TSP.PerformLayout()
        CType(Me.TSP_Numeric_n_generations, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TSP_Numeric_n_children, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TSP_Numeric_n_parents, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TSP_Numeric_n_cities, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_Einstellungen.ResumeLayout(False)
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
    Private WithEvents GroupBox_Einstellungen As System.Windows.Forms.GroupBox
    Private WithEvents TabPage_HookeJeeves As System.Windows.Forms.TabPage
    Private WithEvents Numeric_HJ_DeltaFinish As System.Windows.Forms.NumericUpDown
    Private WithEvents Numeric_HJ_DeltaStart As System.Windows.Forms.NumericUpDown
    Private WithEvents CheckBox_HJ_DNVektor As System.Windows.Forms.CheckBox
    Private WithEvents TabPage_PES As System.Windows.Forms.TabPage
    Private WithEvents CheckisTournamentSelection As System.Windows.Forms.CheckBox
    Private WithEvents ComboOptDnMutation As System.Windows.Forms.ComboBox
    Private WithEvents Label_CES_OptModus As System.Windows.Forms.Label
    Private WithEvents Combo_CES_Selection As System.Windows.Forms.ComboBox
    Private WithEvents Combo_CES_IniValues As System.Windows.Forms.ComboBox
    Private WithEvents Combo_CES_Reproduction As System.Windows.Forms.ComboBox
    Private WithEvents Combo_CES_Mutation As System.Windows.Forms.ComboBox
    Private WithEvents GroupBoxCES As System.Windows.Forms.GroupBox
    Private WithEvents Numeric_CES_n_childs As System.Windows.Forms.NumericUpDown
    Private WithEvents Numeric_CES_n_Parents As System.Windows.Forms.NumericUpDown
    Private WithEvents Numeric_CES_n_Generations As System.Windows.Forms.NumericUpDown
    Private WithEvents Numeric_CES_n_member_SecPop As System.Windows.Forms.NumericUpDown
    Private WithEvents Numeric_CES_n_exchange_SecPop As System.Windows.Forms.NumericUpDown
    Private WithEvents Label_CES_NMembersSecPop As System.Windows.Forms.Label
    Private WithEvents GroupBox_CES_Hybrid As System.Windows.Forms.GroupBox
    Private WithEvents Numeric_CES_MutRate As System.Windows.Forms.NumericUpDown
    Private WithEvents Combo_CES_HybridType As System.Windows.Forms.ComboBox
    Private WithEvents Numeric_CES_n_member_SecPop_PES As System.Windows.Forms.NumericUpDown
    Private WithEvents Numeric_CES_NExchange_SecPop_PES As System.Windows.Forms.NumericUpDown
    Private WithEvents CheckBox_CES_UseSecPop_PES As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox_CES_UseSecPop_CES As System.Windows.Forms.CheckBox
    Private WithEvents TabPage_CES As System.Windows.Forms.TabPage
    Private WithEvents Combo_CES_MemStrategy As System.Windows.Forms.ComboBox
    Private WithEvents Numeric_CES_n_MemSize As System.Windows.Forms.NumericUpDown
    Private WithEvents GroupBox_SekPop As System.Windows.Forms.GroupBox
    Private WithEvents GroupBox_Eltern As System.Windows.Forms.GroupBox
    Private WithEvents CheckBox_isSekPopBegrenzung As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox_CES_isSecPopRestriction As System.Windows.Forms.CheckBox
    Private WithEvents GroupBox_CES_SecPop As System.Windows.Forms.GroupBox
    Private WithEvents CheckBox_CES_RealOptimisation As System.Windows.Forms.CheckBox
    Private WithEvents LabelAnzEltern As System.Windows.Forms.Label
    Private WithEvents CheckBox_CES_StartPESPop As System.Windows.Forms.CheckBox
    Private WithEvents Combo_MetaEvo_Role As System.Windows.Forms.ComboBox
    Private WithEvents Numeric_MetaEvo_Numbergenerations As System.Windows.Forms.NumericUpDown
    Private WithEvents GroupBox_MetaEvo_BasicOptions As System.Windows.Forms.GroupBox
    Private WithEvents Numeric_MetaEvo_PopulationSize As System.Windows.Forms.NumericUpDown
    Private WithEvents GroupBox_MetaEvo_MySQLOptions As System.Windows.Forms.GroupBox
    Private WithEvents TextBox_MetaEvo_MySQL_Password As System.Windows.Forms.TextBox
    Private WithEvents TextBox_MetaEvo_MySQL_User As System.Windows.Forms.TextBox
    Private WithEvents TextBox_MetaEvo_MySQL_Host As System.Windows.Forms.TextBox
    Private WithEvents TextBox_MetaEvo_MySQL_DB As System.Windows.Forms.TextBox
    Private WithEvents TabPage_MetaEvo As System.Windows.Forms.TabPage
    Private WithEvents Combo_MetaEvo_OpMode As System.Windows.Forms.ComboBox
    Private WithEvents GroupBox_MetaEvo_LocalOptions As System.Windows.Forms.GroupBox
    Private WithEvents Numeric_MetaEvo_HJStepsize As System.Windows.Forms.NumericUpDown
    Private WithEvents GroupBox_MetaEvo_TransferOptions As System.Windows.Forms.GroupBox
    Private WithEvents Numeric_MetaEvo_NumberResults As System.Windows.Forms.NumericUpDown
    Private WithEvents TabPage_DDS As System.Windows.Forms.TabPage
    Private WithEvents Numeric_DDS_r_val As System.Windows.Forms.NumericUpDown
    Private WithEvents Numeric_DDS_maxiter As System.Windows.Forms.NumericUpDown
    Private WithEvents CheckBox_DDS_ini As System.Windows.Forms.CheckBox
    Private WithEvents TabPage_SensiPlot As System.Windows.Forms.TabPage
    Private WithEvents SensiPlot_Label_OptParameter As System.Windows.Forms.Label
    Private WithEvents SensiPlot_ListBox_OptParameter As System.Windows.Forms.ListBox
    Private WithEvents SensiPlot_ListBox_Objectives As System.Windows.Forms.ListBox
    Private WithEvents SensiPlot_RadioButton_Discrete As System.Windows.Forms.RadioButton
    Private WithEvents SensiPlot_RadioButton_NormalDistribution As System.Windows.Forms.RadioButton
    Private WithEvents SensiPlot_Label_NumSteps As System.Windows.Forms.Label
    Private WithEvents SensiPlot_NumericUpDown_NumSteps As System.Windows.Forms.NumericUpDown
    Private WithEvents SensiPlot_Label_Objectives As System.Windows.Forms.Label
    Private WithEvents SensiPlot_GroupBox_Modus As System.Windows.Forms.GroupBox
    Private WithEvents SensiPlot_CheckBox_wave As System.Windows.Forms.CheckBox
    Private WithEvents TabControl1 As System.Windows.Forms.TabControl
    Private WithEvents GroupBox_Sim As System.Windows.Forms.GroupBox
    Private WithEvents CheckBox_useMultithreading As System.Windows.Forms.CheckBox
    Private WithEvents TabPage_General As System.Windows.Forms.TabPage
    Private WithEvents CheckBox_drawOnlyCurrentGen As System.Windows.Forms.CheckBox
    Private WithEvents TabPage_TSP As System.Windows.Forms.TabPage
    Friend WithEvents TSP_Label_n_cities As System.Windows.Forms.Label
    Private WithEvents TSP_Numeric_n_generations As System.Windows.Forms.NumericUpDown
    Friend WithEvents TSP_Label_n_generations As System.Windows.Forms.Label
    Private WithEvents TSP_Numeric_n_children As System.Windows.Forms.NumericUpDown
    Friend WithEvents TSP_Label_n_children As System.Windows.Forms.Label
    Private WithEvents TSP_Numeric_n_parents As System.Windows.Forms.NumericUpDown
    Friend WithEvents TSP_Label_n_parents As System.Windows.Forms.Label
    Private WithEvents TSP_Numeric_n_cities As System.Windows.Forms.NumericUpDown
    Private WithEvents TSP_ComboBox_Mutationoperator As System.Windows.Forms.ComboBox
    Friend WithEvents TSP_Label_Mutationoperator As System.Windows.Forms.Label
    Friend WithEvents TSP_Label_Reproductionoperator As System.Windows.Forms.Label
    Private WithEvents TSP_ComboBox_Reproductionoperator As System.Windows.Forms.ComboBox
    Private WithEvents TSP_ComboBox_prob_instance As System.Windows.Forms.ComboBox
    Friend WithEvents TSP_Label_Instance As System.Windows.Forms.Label
End Class
