<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
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
        Me.components = New System.ComponentModel.Container()
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
        Me.CheckBox_drawOnlyCurrentGen = New System.Windows.Forms.CheckBox()
        Me.BindingSource_General = New System.Windows.Forms.BindingSource(Me.components)
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage_General = New System.Windows.Forms.TabPage()
        Me.GroupBox_Sim = New System.Windows.Forms.GroupBox()
        Me.CheckBox_useMultithreading = New System.Windows.Forms.CheckBox()
        Me.Label_NThreads = New System.Windows.Forms.Label()
        Me.NumericUpDown_NThreads = New System.Windows.Forms.NumericUpDown()
        Me.TabPage_PES = New System.Windows.Forms.TabPage()
        Me.PES_Label_OptModus = New System.Windows.Forms.Label()
        Me.BindingSource_PES = New System.Windows.Forms.BindingSource(Me.components)
        Me.PES_Combo_Strategie = New System.Windows.Forms.ComboBox()
        Me.PES_Combo_Startparameter = New System.Windows.Forms.ComboBox()
        Me.PES_Combo_DnMutation = New System.Windows.Forms.ComboBox()
        Me.PES_Numeric_DnStart = New System.Windows.Forms.NumericUpDown()
        Me.BindingSource_PES_Schrittweite = New System.Windows.Forms.BindingSource(Me.components)
        Me.PES_Checkbox_isDnVektor = New System.Windows.Forms.CheckBox()
        Me.GroupBox_Generationen = New System.Windows.Forms.GroupBox()
        Me.PES_Numeric_AnzNachf = New System.Windows.Forms.NumericUpDown()
        Me.PES_Numeric_AnzEltern = New System.Windows.Forms.NumericUpDown()
        Me.PES_Numeric_AnzGen = New System.Windows.Forms.NumericUpDown()
        Me.LabelAnzEltern = New System.Windows.Forms.Label()
        Me.GroupBox_Eltern = New System.Windows.Forms.GroupBox()
        Me.PES_Combo_OptEltern = New System.Windows.Forms.ComboBox()
        Me.PES_Checkbox_isTournamentSelection = New System.Windows.Forms.CheckBox()
        Me.PES_Numeric_Rekombxy = New System.Windows.Forms.NumericUpDown()
        Me.LabelRekombxy1 = New System.Windows.Forms.Label()
        Me.LabelRekombxy3 = New System.Windows.Forms.Label()
        Me.GroupBox_SekPop = New System.Windows.Forms.GroupBox()
        Me.PES_CheckBox_SekPop_isBegrenzung = New System.Windows.Forms.CheckBox()
        Me.BindingSource_PES_SekPop = New System.Windows.Forms.BindingSource(Me.components)
        Me.PES_Numeric_nInteract = New System.Windows.Forms.NumericUpDown()
        Me.PES_Numeric_MaxMemberSekPop = New System.Windows.Forms.NumericUpDown()
        Me.LabelMaxMemberSekPop = New System.Windows.Forms.Label()
        Me.PES_Checkbox_isPopul = New System.Windows.Forms.CheckBox()
        Me.BindingSource_PES_Pop = New System.Windows.Forms.BindingSource(Me.components)
        Me.PES_GroupBox_Populationen = New System.Windows.Forms.GroupBox()
        Me.PES_Numeric_AnzRunden = New System.Windows.Forms.NumericUpDown()
        Me.PES_Numeric_AnzPop = New System.Windows.Forms.NumericUpDown()
        Me.PES_Numeric_AnzPopEltern = New System.Windows.Forms.NumericUpDown()
        Me.PES_Combo_PopEltern = New System.Windows.Forms.ComboBox()
        Me.PES_Combo_PopStrategie = New System.Windows.Forms.ComboBox()
        Me.PES_Combo_PopPenalty = New System.Windows.Forms.ComboBox()
        Me.BindingSource_PES_PopPenaltyOptions = New System.Windows.Forms.BindingSource(Me.components)
        Me.TabPage_HookeJeeves = New System.Windows.Forms.TabPage()
        Me.HJ_Numeric_DeltaFinish = New System.Windows.Forms.NumericUpDown()
        Me.BindingSource_HookeJeeves = New System.Windows.Forms.BindingSource(Me.components)
        Me.HJ_Numeric_DeltaStart = New System.Windows.Forms.NumericUpDown()
        Me.HJ_CheckBox_DNVektor = New System.Windows.Forms.CheckBox()
        Me.TabPage_MetaEvo = New System.Windows.Forms.TabPage()
        Me.GroupBox_MetaEvo_TransferOptions = New System.Windows.Forms.GroupBox()
        Me.MetaEvo_Numeric_NumberResults = New System.Windows.Forms.NumericUpDown()
        Me.BindingSource_MetaEvo = New System.Windows.Forms.BindingSource(Me.components)
        Me.GroupBox_MetaEvo_LocalOptions = New System.Windows.Forms.GroupBox()
        Me.MetaEvo_Numeric_HJStepsize = New System.Windows.Forms.NumericUpDown()
        Me.MetaEvo_Combo_OpMode = New System.Windows.Forms.ComboBox()
        Me.GroupBox_MetaEvo_MySQLOptions = New System.Windows.Forms.GroupBox()
        Me.MetaEvo_TextBox_MySQL_DB = New System.Windows.Forms.TextBox()
        Me.MetaEvo_TextBox_MySQL_Password = New System.Windows.Forms.TextBox()
        Me.MetaEvo_TextBox_MySQL_User = New System.Windows.Forms.TextBox()
        Me.MetaEvo_TextBox_MySQL_Host = New System.Windows.Forms.TextBox()
        Me.GroupBox_MetaEvo_BasicOptions = New System.Windows.Forms.GroupBox()
        Me.MetaEvo_Numeric_PopulationSize = New System.Windows.Forms.NumericUpDown()
        Me.MetaEvo_Numeric_Numbergenerations = New System.Windows.Forms.NumericUpDown()
        Me.MetaEvo_Combo_Role = New System.Windows.Forms.ComboBox()
        Me.TabPage_DDS = New System.Windows.Forms.TabPage()
        Me.DDS_CheckBox_RandomStartparameters = New System.Windows.Forms.CheckBox()
        Me.BindingSource_DDS = New System.Windows.Forms.BindingSource(Me.components)
        Me.DDS_Numeric_maxiter = New System.Windows.Forms.NumericUpDown()
        Me.DDS_Numeric_r_val = New System.Windows.Forms.NumericUpDown()
        Me.TabPage_SensiPlot = New System.Windows.Forms.TabPage()
        Me.SensiPlot_Label_NumSims = New System.Windows.Forms.Label()
        Me.SensiPlot_CheckBox_SaveResults = New System.Windows.Forms.CheckBox()
        Me.BindingSource_Sensiplot = New System.Windows.Forms.BindingSource(Me.components)
        Me.SensiPlot_CheckBox_wave = New System.Windows.Forms.CheckBox()
        Me.SensiPlot_Label_NumSteps = New System.Windows.Forms.Label()
        Me.SensiPlot_NumericUpDown_NumSteps = New System.Windows.Forms.NumericUpDown()
        Me.SensiPlot_GroupBox_Modus = New System.Windows.Forms.GroupBox()
        Me.SensiPlot_RadioButton_ModeLatinHypercube = New System.Windows.Forms.RadioButton()
        Me.SensiPlot_RadioButton_ModeEvenDistribution = New System.Windows.Forms.RadioButton()
        Me.SensiPlot_RadioButton_ModeRandom = New System.Windows.Forms.RadioButton()
        Me.SensiPlot_Label_Objectives = New System.Windows.Forms.Label()
        Me.SensiPlot_ListBox_Objectives = New System.Windows.Forms.ListBox()
        Me.SensiPlot_Label_OptParameter = New System.Windows.Forms.Label()
        Me.SensiPlot_ListBox_OptParameter = New System.Windows.Forms.ListBox()
        Me.TabPage_TSP = New System.Windows.Forms.TabPage()
        Me.TSP_ComboBox_prob_instance = New System.Windows.Forms.ComboBox()
        Me.BindingSource_TSP = New System.Windows.Forms.BindingSource(Me.components)
        Me.TSP_Label_Instance = New System.Windows.Forms.Label()
        Me.TSP_ComboBox_Mutationoperator = New System.Windows.Forms.ComboBox()
        Me.TSP_Label_Mutationoperator = New System.Windows.Forms.Label()
        Me.TSP_Label_Reproductionoperator = New System.Windows.Forms.Label()
        Me.TSP_ComboBox_Reproductionoperator = New System.Windows.Forms.ComboBox()
        Me.TSP_Numeric_n_generations = New System.Windows.Forms.NumericUpDown()
        Me.TSP_Label_n_generations = New System.Windows.Forms.Label()
        Me.TSP_Numeric_n_children = New System.Windows.Forms.NumericUpDown()
        Me.TSP_Label_n_children = New System.Windows.Forms.Label()
        Me.TSP_Numeric_n_parents = New System.Windows.Forms.NumericUpDown()
        Me.TSP_Label_n_parents = New System.Windows.Forms.Label()
        Me.TSP_Numeric_n_cities = New System.Windows.Forms.NumericUpDown()
        Me.TSP_Label_n_cities = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox_Einstellungen = New System.Windows.Forms.GroupBox()
        Label_OptModus = New System.Windows.Forms.Label()
        LabelStrategie = New System.Windows.Forms.Label()
        LabelStartwerte = New System.Windows.Forms.Label()
        LabelMutation = New System.Windows.Forms.Label()
        LabelStartSchrittweite = New System.Windows.Forms.Label()
        LabelAnzNachf = New System.Windows.Forms.Label()
        LabelAnzGen = New System.Windows.Forms.Label()
        Label2 = New System.Windows.Forms.Label()
        LabelInteract = New System.Windows.Forms.Label()
        LabelAnzRunden = New System.Windows.Forms.Label()
        LabelAnzPop = New System.Windows.Forms.Label()
        LabelAnzPopEltern = New System.Windows.Forms.Label()
        LabelOptPopEltern = New System.Windows.Forms.Label()
        LabelPopStrategie = New System.Windows.Forms.Label()
        LabelPopPenalty = New System.Windows.Forms.Label()
        Label_Line = New System.Windows.Forms.Label()
        Label3 = New System.Windows.Forms.Label()
        Label1 = New System.Windows.Forms.Label()
        Label11 = New System.Windows.Forms.Label()
        Label5 = New System.Windows.Forms.Label()
        Label10 = New System.Windows.Forms.Label()
        Label13 = New System.Windows.Forms.Label()
        Label14 = New System.Windows.Forms.Label()
        Label15 = New System.Windows.Forms.Label()
        Label16 = New System.Windows.Forms.Label()
        Label18 = New System.Windows.Forms.Label()
        Label20 = New System.Windows.Forms.Label()
        Label19 = New System.Windows.Forms.Label()
        Label_Meta11 = New System.Windows.Forms.Label()
        Label_Meta5 = New System.Windows.Forms.Label()
        Label_Meta10 = New System.Windows.Forms.Label()
        GroupBox_Diagramm = New System.Windows.Forms.GroupBox()
        GroupBox_Diagramm.SuspendLayout()
        CType(Me.BindingSource_General, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage_General.SuspendLayout()
        Me.GroupBox_Sim.SuspendLayout()
        CType(Me.NumericUpDown_NThreads, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage_PES.SuspendLayout()
        CType(Me.BindingSource_PES, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PES_Numeric_DnStart, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource_PES_Schrittweite, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_Generationen.SuspendLayout()
        CType(Me.PES_Numeric_AnzNachf, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PES_Numeric_AnzEltern, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PES_Numeric_AnzGen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_Eltern.SuspendLayout()
        CType(Me.PES_Numeric_Rekombxy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_SekPop.SuspendLayout()
        CType(Me.BindingSource_PES_SekPop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PES_Numeric_nInteract, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PES_Numeric_MaxMemberSekPop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource_PES_Pop, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PES_GroupBox_Populationen.SuspendLayout()
        CType(Me.PES_Numeric_AnzRunden, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PES_Numeric_AnzPop, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PES_Numeric_AnzPopEltern, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource_PES_PopPenaltyOptions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage_HookeJeeves.SuspendLayout()
        CType(Me.HJ_Numeric_DeltaFinish, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource_HookeJeeves, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HJ_Numeric_DeltaStart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage_MetaEvo.SuspendLayout()
        Me.GroupBox_MetaEvo_TransferOptions.SuspendLayout()
        CType(Me.MetaEvo_Numeric_NumberResults, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource_MetaEvo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_MetaEvo_LocalOptions.SuspendLayout()
        CType(Me.MetaEvo_Numeric_HJStepsize, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_MetaEvo_MySQLOptions.SuspendLayout()
        Me.GroupBox_MetaEvo_BasicOptions.SuspendLayout()
        CType(Me.MetaEvo_Numeric_PopulationSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MetaEvo_Numeric_Numbergenerations, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage_DDS.SuspendLayout()
        CType(Me.BindingSource_DDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DDS_Numeric_maxiter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DDS_Numeric_r_val, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage_SensiPlot.SuspendLayout()
        CType(Me.BindingSource_Sensiplot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SensiPlot_NumericUpDown_NumSteps, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SensiPlot_GroupBox_Modus.SuspendLayout()
        Me.TabPage_TSP.SuspendLayout()
        CType(Me.BindingSource_TSP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TSP_Numeric_n_generations, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TSP_Numeric_n_children, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TSP_Numeric_n_parents, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TSP_Numeric_n_cities, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_Einstellungen.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label_OptModus
        '
        Label_OptModus.AutoSize = True
        Label_OptModus.Location = New System.Drawing.Point(2, 9)
        Label_OptModus.Name = "Label_OptModus"
        Label_OptModus.Size = New System.Drawing.Size(37, 13)
        Label_OptModus.TabIndex = 0
        Label_OptModus.Text = "Mode:"
        '
        'LabelStrategie
        '
        LabelStrategie.AutoSize = True
        LabelStrategie.Location = New System.Drawing.Point(2, 37)
        LabelStrategie.Name = "LabelStrategie"
        LabelStrategie.Size = New System.Drawing.Size(54, 13)
        LabelStrategie.TabIndex = 2
        LabelStrategie.Text = "Selection:"
        '
        'LabelStartwerte
        '
        LabelStartwerte.AutoSize = True
        LabelStartwerte.Location = New System.Drawing.Point(2, 64)
        LabelStartwerte.Name = "LabelStartwerte"
        LabelStartwerte.Size = New System.Drawing.Size(66, 13)
        LabelStartwerte.TabIndex = 4
        LabelStartwerte.Text = "Start values:"
        '
        'LabelMutation
        '
        LabelMutation.AutoSize = True
        LabelMutation.Location = New System.Drawing.Point(2, 91)
        LabelMutation.Name = "LabelMutation"
        LabelMutation.Size = New System.Drawing.Size(51, 13)
        LabelMutation.TabIndex = 6
        LabelMutation.Text = "Mutation:"
        '
        'LabelStartSchrittweite
        '
        LabelStartSchrittweite.AutoSize = True
        LabelStartSchrittweite.Location = New System.Drawing.Point(3, 117)
        LabelStartSchrittweite.Name = "LabelStartSchrittweite"
        LabelStartSchrittweite.Size = New System.Drawing.Size(90, 13)
        LabelStartSchrittweite.TabIndex = 8
        LabelStartSchrittweite.Text = "Starting step size:"
        '
        'LabelAnzNachf
        '
        LabelAnzNachf.AutoSize = True
        LabelAnzNachf.Location = New System.Drawing.Point(8, 64)
        LabelAnzNachf.Name = "LabelAnzNachf"
        LabelAnzNachf.Size = New System.Drawing.Size(76, 13)
        LabelAnzNachf.TabIndex = 4
        LabelAnzNachf.Text = "No of children:"
        '
        'LabelAnzGen
        '
        LabelAnzGen.AutoSize = True
        LabelAnzGen.Location = New System.Drawing.Point(7, 20)
        LabelAnzGen.Name = "LabelAnzGen"
        LabelAnzGen.Size = New System.Drawing.Size(94, 13)
        LabelAnzGen.TabIndex = 0
        LabelAnzGen.Text = "No of generations:"
        '
        'Label2
        '
        Label2.Location = New System.Drawing.Point(8, 20)
        Label2.Name = "Label2"
        Label2.Size = New System.Drawing.Size(72, 26)
        Label2.TabIndex = 0
        Label2.Text = "Determination of parents:"
        '
        'LabelInteract
        '
        LabelInteract.AutoSize = True
        LabelInteract.Location = New System.Drawing.Point(8, 24)
        LabelInteract.Name = "LabelInteract"
        LabelInteract.Size = New System.Drawing.Size(121, 13)
        LabelInteract.TabIndex = 0
        LabelInteract.Text = "Exchange with sec pop:"
        '
        'LabelAnzRunden
        '
        LabelAnzRunden.AutoSize = True
        LabelAnzRunden.Location = New System.Drawing.Point(8, 16)
        LabelAnzRunden.Name = "LabelAnzRunden"
        LabelAnzRunden.Size = New System.Drawing.Size(71, 13)
        LabelAnzRunden.TabIndex = 0
        LabelAnzRunden.Text = "No of rounds:"
        '
        'LabelAnzPop
        '
        LabelAnzPop.AutoSize = True
        LabelAnzPop.Location = New System.Drawing.Point(7, 36)
        LabelAnzPop.Name = "LabelAnzPop"
        LabelAnzPop.Size = New System.Drawing.Size(93, 13)
        LabelAnzPop.TabIndex = 2
        LabelAnzPop.Text = "No of populations:"
        '
        'LabelAnzPopEltern
        '
        LabelAnzPopEltern.AutoSize = True
        LabelAnzPopEltern.Location = New System.Drawing.Point(7, 58)
        LabelAnzPopEltern.Name = "LabelAnzPopEltern"
        LabelAnzPopEltern.Size = New System.Drawing.Size(117, 13)
        LabelAnzPopEltern.TabIndex = 4
        LabelAnzPopEltern.Text = "No. of parents [max=5]:"
        '
        'LabelOptPopEltern
        '
        LabelOptPopEltern.Location = New System.Drawing.Point(8, 80)
        LabelOptPopEltern.Name = "LabelOptPopEltern"
        LabelOptPopEltern.Size = New System.Drawing.Size(76, 28)
        LabelOptPopEltern.TabIndex = 6
        LabelOptPopEltern.Text = "Determination pop parents:"
        '
        'LabelPopStrategie
        '
        LabelPopStrategie.Location = New System.Drawing.Point(8, 110)
        LabelPopStrategie.Name = "LabelPopStrategie"
        LabelPopStrategie.Size = New System.Drawing.Size(76, 17)
        LabelPopStrategie.TabIndex = 8
        LabelPopStrategie.Text = "Selection:"
        '
        'LabelPopPenalty
        '
        LabelPopPenalty.Location = New System.Drawing.Point(8, 127)
        LabelPopPenalty.Name = "LabelPopPenalty"
        LabelPopPenalty.Size = New System.Drawing.Size(76, 29)
        LabelPopPenalty.TabIndex = 10
        LabelPopPenalty.Text = "Determination pop quality:"
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
        GroupBox_Diagramm.Location = New System.Drawing.Point(6, 90)
        GroupBox_Diagramm.Name = "GroupBox_Diagramm"
        GroupBox_Diagramm.Size = New System.Drawing.Size(209, 62)
        GroupBox_Diagramm.TabIndex = 1
        GroupBox_Diagramm.TabStop = False
        GroupBox_Diagramm.Text = "Diagram"
        '
        'CheckBox_drawOnlyCurrentGen
        '
        Me.CheckBox_drawOnlyCurrentGen.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox_drawOnlyCurrentGen.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.BindingSource_General, "DrawOnlyCurrentGeneration", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.CheckBox_drawOnlyCurrentGen.Location = New System.Drawing.Point(6, 19)
        Me.CheckBox_drawOnlyCurrentGen.Name = "CheckBox_drawOnlyCurrentGen"
        Me.CheckBox_drawOnlyCurrentGen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_drawOnlyCurrentGen.Size = New System.Drawing.Size(197, 37)
        Me.CheckBox_drawOnlyCurrentGen.TabIndex = 0
        Me.CheckBox_drawOnlyCurrentGen.Text = "Only display the current generation:"
        Me.CheckBox_drawOnlyCurrentGen.UseVisualStyleBackColor = True
        '
        'BindingSource_General
        '
        Me.BindingSource_General.DataSource = GetType(BlueM.Opt.Common.Settings.Settings_General)
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage_General)
        Me.TabControl1.Controls.Add(Me.TabPage_PES)
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
        Me.TabControl1.TabIndex = 0
        '
        'TabPage_General
        '
        Me.TabPage_General.AutoScroll = True
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
        Me.GroupBox_Sim.Controls.Add(Me.Label_NThreads)
        Me.GroupBox_Sim.Controls.Add(Me.NumericUpDown_NThreads)
        Me.GroupBox_Sim.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox_Sim.Name = "GroupBox_Sim"
        Me.GroupBox_Sim.Size = New System.Drawing.Size(209, 78)
        Me.GroupBox_Sim.TabIndex = 0
        Me.GroupBox_Sim.TabStop = False
        Me.GroupBox_Sim.Text = "Simulations"
        '
        'CheckBox_useMultithreading
        '
        Me.CheckBox_useMultithreading.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox_useMultithreading.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.BindingSource_General, "UseMultithreading", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.CheckBox_useMultithreading.DataBindings.Add(New System.Windows.Forms.Binding("Enabled", Me.BindingSource_General, "MultithreadingAllowed", True, System.Windows.Forms.DataSourceUpdateMode.Never))
        Me.CheckBox_useMultithreading.Location = New System.Drawing.Point(6, 20)
        Me.CheckBox_useMultithreading.Name = "CheckBox_useMultithreading"
        Me.CheckBox_useMultithreading.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_useMultithreading.Size = New System.Drawing.Size(197, 24)
        Me.CheckBox_useMultithreading.TabIndex = 0
        Me.CheckBox_useMultithreading.Text = "Use multithreading:"
        Me.CheckBox_useMultithreading.UseVisualStyleBackColor = True
        '
        'Label_NThreads
        '
        Me.Label_NThreads.AutoSize = True
        Me.Label_NThreads.Location = New System.Drawing.Point(7, 52)
        Me.Label_NThreads.Name = "Label_NThreads"
        Me.Label_NThreads.Size = New System.Drawing.Size(97, 13)
        Me.Label_NThreads.TabIndex = 1
        Me.Label_NThreads.Text = "Number of threads:"
        '
        'NumericUpDown_NThreads
        '
        Me.NumericUpDown_NThreads.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NumericUpDown_NThreads.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_General, "NThreads", True))
        Me.NumericUpDown_NThreads.Location = New System.Drawing.Point(146, 50)
        Me.NumericUpDown_NThreads.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.NumericUpDown_NThreads.Name = "NumericUpDown_NThreads"
        Me.NumericUpDown_NThreads.Size = New System.Drawing.Size(57, 20)
        Me.NumericUpDown_NThreads.TabIndex = 2
        Me.NumericUpDown_NThreads.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'TabPage_PES
        '
        Me.TabPage_PES.AutoScroll = True
        Me.TabPage_PES.BackColor = System.Drawing.Color.Transparent
        Me.TabPage_PES.Controls.Add(Label_OptModus)
        Me.TabPage_PES.Controls.Add(Me.PES_Label_OptModus)
        Me.TabPage_PES.Controls.Add(LabelStrategie)
        Me.TabPage_PES.Controls.Add(Me.PES_Combo_Strategie)
        Me.TabPage_PES.Controls.Add(LabelStartwerte)
        Me.TabPage_PES.Controls.Add(Me.PES_Combo_Startparameter)
        Me.TabPage_PES.Controls.Add(LabelMutation)
        Me.TabPage_PES.Controls.Add(Me.PES_Combo_DnMutation)
        Me.TabPage_PES.Controls.Add(LabelStartSchrittweite)
        Me.TabPage_PES.Controls.Add(Me.PES_Numeric_DnStart)
        Me.TabPage_PES.Controls.Add(Me.PES_Checkbox_isDnVektor)
        Me.TabPage_PES.Controls.Add(Me.GroupBox_Generationen)
        Me.TabPage_PES.Controls.Add(Me.GroupBox_Eltern)
        Me.TabPage_PES.Controls.Add(Me.GroupBox_SekPop)
        Me.TabPage_PES.Controls.Add(Me.PES_Checkbox_isPopul)
        Me.TabPage_PES.Controls.Add(Me.PES_GroupBox_Populationen)
        Me.TabPage_PES.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_PES.Margin = New System.Windows.Forms.Padding(0)
        Me.TabPage_PES.Name = "TabPage_PES"
        Me.TabPage_PES.Size = New System.Drawing.Size(221, 668)
        Me.TabPage_PES.TabIndex = 0
        Me.TabPage_PES.Text = "PES"
        Me.TabPage_PES.UseVisualStyleBackColor = True
        '
        'PES_Label_OptModus
        '
        Me.PES_Label_OptModus.AutoSize = True
        Me.PES_Label_OptModus.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.BindingSource_PES, "OptModusString", True, System.Windows.Forms.DataSourceUpdateMode.Never))
        Me.PES_Label_OptModus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PES_Label_OptModus.Location = New System.Drawing.Point(77, 9)
        Me.PES_Label_OptModus.Name = "PES_Label_OptModus"
        Me.PES_Label_OptModus.Size = New System.Drawing.Size(38, 13)
        Me.PES_Label_OptModus.TabIndex = 1
        Me.PES_Label_OptModus.Text = "Mode"
        '
        'BindingSource_PES
        '
        Me.BindingSource_PES.DataSource = GetType(BlueM.Opt.Common.Settings_PES)
        '
        'PES_Combo_Strategie
        '
        Me.PES_Combo_Strategie.DataBindings.Add(New System.Windows.Forms.Binding("SelectedItem", Me.BindingSource_PES, "Strategie", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Combo_Strategie.DataBindings.Add(New System.Windows.Forms.Binding("Enabled", Me.BindingSource_PES, "StrategieEnabled", True, System.Windows.Forms.DataSourceUpdateMode.Never))
        Me.PES_Combo_Strategie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.PES_Combo_Strategie.Location = New System.Drawing.Point(80, 34)
        Me.PES_Combo_Strategie.Name = "PES_Combo_Strategie"
        Me.PES_Combo_Strategie.Size = New System.Drawing.Size(123, 21)
        Me.PES_Combo_Strategie.TabIndex = 3
        '
        'PES_Combo_Startparameter
        '
        Me.PES_Combo_Startparameter.DataBindings.Add(New System.Windows.Forms.Binding("SelectedItem", Me.BindingSource_PES, "Startparameter", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Combo_Startparameter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.PES_Combo_Startparameter.Location = New System.Drawing.Point(80, 61)
        Me.PES_Combo_Startparameter.Name = "PES_Combo_Startparameter"
        Me.PES_Combo_Startparameter.Size = New System.Drawing.Size(123, 21)
        Me.PES_Combo_Startparameter.TabIndex = 5
        '
        'PES_Combo_DnMutation
        '
        Me.PES_Combo_DnMutation.DataBindings.Add(New System.Windows.Forms.Binding("SelectedItem", Me.BindingSource_PES, "Mutationsop", True))
        Me.PES_Combo_DnMutation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.PES_Combo_DnMutation.FormattingEnabled = True
        Me.PES_Combo_DnMutation.Location = New System.Drawing.Point(80, 88)
        Me.PES_Combo_DnMutation.Name = "PES_Combo_DnMutation"
        Me.PES_Combo_DnMutation.Size = New System.Drawing.Size(123, 21)
        Me.PES_Combo_DnMutation.TabIndex = 7
        '
        'PES_Numeric_DnStart
        '
        Me.PES_Numeric_DnStart.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_PES_Schrittweite, "DnStart", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Numeric_DnStart.DecimalPlaces = 2
        Me.PES_Numeric_DnStart.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.PES_Numeric_DnStart.Location = New System.Drawing.Point(150, 115)
        Me.PES_Numeric_DnStart.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.PES_Numeric_DnStart.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.PES_Numeric_DnStart.Name = "PES_Numeric_DnStart"
        Me.PES_Numeric_DnStart.Size = New System.Drawing.Size(53, 20)
        Me.PES_Numeric_DnStart.TabIndex = 9
        Me.PES_Numeric_DnStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.PES_Numeric_DnStart.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'BindingSource_PES_Schrittweite
        '
        Me.BindingSource_PES_Schrittweite.DataSource = GetType(BlueM.Opt.Common.Settings_PES.Settings_Mutation)
        '
        'PES_Checkbox_isDnVektor
        '
        Me.PES_Checkbox_isDnVektor.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.BindingSource_PES_Schrittweite, "IsDnVektor", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Checkbox_isDnVektor.Location = New System.Drawing.Point(5, 141)
        Me.PES_Checkbox_isDnVektor.Name = "PES_Checkbox_isDnVektor"
        Me.PES_Checkbox_isDnVektor.Size = New System.Drawing.Size(144, 18)
        Me.PES_Checkbox_isDnVektor.TabIndex = 10
        Me.PES_Checkbox_isDnVektor.Text = "Use step size vector"
        '
        'GroupBox_Generationen
        '
        Me.GroupBox_Generationen.Controls.Add(Me.PES_Numeric_AnzNachf)
        Me.GroupBox_Generationen.Controls.Add(Me.PES_Numeric_AnzEltern)
        Me.GroupBox_Generationen.Controls.Add(Me.PES_Numeric_AnzGen)
        Me.GroupBox_Generationen.Controls.Add(LabelAnzNachf)
        Me.GroupBox_Generationen.Controls.Add(Me.LabelAnzEltern)
        Me.GroupBox_Generationen.Controls.Add(LabelAnzGen)
        Me.GroupBox_Generationen.Location = New System.Drawing.Point(4, 165)
        Me.GroupBox_Generationen.Name = "GroupBox_Generationen"
        Me.GroupBox_Generationen.Size = New System.Drawing.Size(200, 90)
        Me.GroupBox_Generationen.TabIndex = 11
        Me.GroupBox_Generationen.TabStop = False
        Me.GroupBox_Generationen.Text = "Generations"
        '
        'PES_Numeric_AnzNachf
        '
        Me.PES_Numeric_AnzNachf.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_PES, "N_Nachf", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Numeric_AnzNachf.Location = New System.Drawing.Point(140, 61)
        Me.PES_Numeric_AnzNachf.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.PES_Numeric_AnzNachf.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.PES_Numeric_AnzNachf.Name = "PES_Numeric_AnzNachf"
        Me.PES_Numeric_AnzNachf.Size = New System.Drawing.Size(53, 20)
        Me.PES_Numeric_AnzNachf.TabIndex = 5
        Me.PES_Numeric_AnzNachf.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.PES_Numeric_AnzNachf.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'PES_Numeric_AnzEltern
        '
        Me.PES_Numeric_AnzEltern.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_PES, "N_Eltern", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Numeric_AnzEltern.Location = New System.Drawing.Point(140, 39)
        Me.PES_Numeric_AnzEltern.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.PES_Numeric_AnzEltern.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.PES_Numeric_AnzEltern.Name = "PES_Numeric_AnzEltern"
        Me.PES_Numeric_AnzEltern.Size = New System.Drawing.Size(53, 20)
        Me.PES_Numeric_AnzEltern.TabIndex = 3
        Me.PES_Numeric_AnzEltern.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.PES_Numeric_AnzEltern.Value = New Decimal(New Integer() {3, 0, 0, 0})
        '
        'PES_Numeric_AnzGen
        '
        Me.PES_Numeric_AnzGen.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_PES, "N_Gen", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Numeric_AnzGen.Location = New System.Drawing.Point(140, 16)
        Me.PES_Numeric_AnzGen.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
        Me.PES_Numeric_AnzGen.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.PES_Numeric_AnzGen.Name = "PES_Numeric_AnzGen"
        Me.PES_Numeric_AnzGen.Size = New System.Drawing.Size(53, 20)
        Me.PES_Numeric_AnzGen.TabIndex = 1
        Me.PES_Numeric_AnzGen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.PES_Numeric_AnzGen.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'LabelAnzEltern
        '
        Me.LabelAnzEltern.AutoSize = True
        Me.LabelAnzEltern.Location = New System.Drawing.Point(7, 42)
        Me.LabelAnzEltern.Name = "LabelAnzEltern"
        Me.LabelAnzEltern.Size = New System.Drawing.Size(74, 13)
        Me.LabelAnzEltern.TabIndex = 2
        Me.LabelAnzEltern.Text = "No of parents:"
        '
        'GroupBox_Eltern
        '
        Me.GroupBox_Eltern.Controls.Add(Me.PES_Combo_OptEltern)
        Me.GroupBox_Eltern.Controls.Add(Me.PES_Checkbox_isTournamentSelection)
        Me.GroupBox_Eltern.Controls.Add(Me.PES_Numeric_Rekombxy)
        Me.GroupBox_Eltern.Controls.Add(Label2)
        Me.GroupBox_Eltern.Controls.Add(Me.LabelRekombxy1)
        Me.GroupBox_Eltern.Controls.Add(Me.LabelRekombxy3)
        Me.GroupBox_Eltern.Location = New System.Drawing.Point(4, 261)
        Me.GroupBox_Eltern.Name = "GroupBox_Eltern"
        Me.GroupBox_Eltern.Size = New System.Drawing.Size(200, 104)
        Me.GroupBox_Eltern.TabIndex = 12
        Me.GroupBox_Eltern.TabStop = False
        Me.GroupBox_Eltern.Text = "Parents"
        '
        'PES_Combo_OptEltern
        '
        Me.PES_Combo_OptEltern.DataBindings.Add(New System.Windows.Forms.Binding("SelectedItem", Me.BindingSource_PES, "Reproduktionsop", True))
        Me.PES_Combo_OptEltern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.PES_Combo_OptEltern.Location = New System.Drawing.Point(85, 23)
        Me.PES_Combo_OptEltern.Name = "PES_Combo_OptEltern"
        Me.PES_Combo_OptEltern.Size = New System.Drawing.Size(109, 21)
        Me.PES_Combo_OptEltern.TabIndex = 1
        '
        'PES_Checkbox_isTournamentSelection
        '
        Me.PES_Checkbox_isTournamentSelection.AutoSize = True
        Me.PES_Checkbox_isTournamentSelection.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.BindingSource_PES, "Is_DiversityTournament", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Checkbox_isTournamentSelection.DataBindings.Add(New System.Windows.Forms.Binding("Enabled", Me.BindingSource_PES, "RecombXYEnabled", True, System.Windows.Forms.DataSourceUpdateMode.Never))
        Me.PES_Checkbox_isTournamentSelection.Location = New System.Drawing.Point(11, 81)
        Me.PES_Checkbox_isTournamentSelection.Name = "PES_Checkbox_isTournamentSelection"
        Me.PES_Checkbox_isTournamentSelection.Size = New System.Drawing.Size(128, 17)
        Me.PES_Checkbox_isTournamentSelection.TabIndex = 5
        Me.PES_Checkbox_isTournamentSelection.Text = "Tournament selection"
        Me.PES_Checkbox_isTournamentSelection.UseVisualStyleBackColor = True
        '
        'PES_Numeric_Rekombxy
        '
        Me.PES_Numeric_Rekombxy.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_PES, "N_RekombXY", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Numeric_Rekombxy.DataBindings.Add(New System.Windows.Forms.Binding("Enabled", Me.BindingSource_PES, "RecombXYEnabled", True, System.Windows.Forms.DataSourceUpdateMode.Never))
        Me.PES_Numeric_Rekombxy.Location = New System.Drawing.Point(33, 55)
        Me.PES_Numeric_Rekombxy.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.PES_Numeric_Rekombxy.Name = "PES_Numeric_Rekombxy"
        Me.PES_Numeric_Rekombxy.Size = New System.Drawing.Size(40, 20)
        Me.PES_Numeric_Rekombxy.TabIndex = 3
        Me.PES_Numeric_Rekombxy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.PES_Numeric_Rekombxy.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'LabelRekombxy1
        '
        Me.LabelRekombxy1.AutoSize = True
        Me.LabelRekombxy1.DataBindings.Add(New System.Windows.Forms.Binding("Enabled", Me.BindingSource_PES, "RecombXYEnabled", True, System.Windows.Forms.DataSourceUpdateMode.Never))
        Me.LabelRekombxy1.Location = New System.Drawing.Point(9, 58)
        Me.LabelRekombxy1.Name = "LabelRekombxy1"
        Me.LabelRekombxy1.Size = New System.Drawing.Size(22, 13)
        Me.LabelRekombxy1.TabIndex = 2
        Me.LabelRekombxy1.Text = "X /"
        '
        'LabelRekombxy3
        '
        Me.LabelRekombxy3.AutoSize = True
        Me.LabelRekombxy3.DataBindings.Add(New System.Windows.Forms.Binding("Enabled", Me.BindingSource_PES, "RecombXYEnabled", True, System.Windows.Forms.DataSourceUpdateMode.Never))
        Me.LabelRekombxy3.Location = New System.Drawing.Point(75, 57)
        Me.LabelRekombxy3.Name = "LabelRekombxy3"
        Me.LabelRekombxy3.Size = New System.Drawing.Size(79, 13)
        Me.LabelRekombxy3.TabIndex = 4
        Me.LabelRekombxy3.Text = "- recombination"
        '
        'GroupBox_SekPop
        '
        Me.GroupBox_SekPop.Controls.Add(Me.PES_CheckBox_SekPop_isBegrenzung)
        Me.GroupBox_SekPop.Controls.Add(Me.PES_Numeric_nInteract)
        Me.GroupBox_SekPop.Controls.Add(Me.PES_Numeric_MaxMemberSekPop)
        Me.GroupBox_SekPop.Controls.Add(LabelInteract)
        Me.GroupBox_SekPop.Controls.Add(Me.LabelMaxMemberSekPop)
        Me.GroupBox_SekPop.DataBindings.Add(New System.Windows.Forms.Binding("Enabled", Me.BindingSource_PES, "SekPopEnabled", True, System.Windows.Forms.DataSourceUpdateMode.Never))
        Me.GroupBox_SekPop.Location = New System.Drawing.Point(4, 371)
        Me.GroupBox_SekPop.Name = "GroupBox_SekPop"
        Me.GroupBox_SekPop.Size = New System.Drawing.Size(200, 93)
        Me.GroupBox_SekPop.TabIndex = 13
        Me.GroupBox_SekPop.TabStop = False
        Me.GroupBox_SekPop.Text = "Secondary population"
        '
        'PES_CheckBox_SekPop_isBegrenzung
        '
        Me.PES_CheckBox_SekPop_isBegrenzung.AutoSize = True
        Me.PES_CheckBox_SekPop_isBegrenzung.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.BindingSource_PES_SekPop, "Is_Begrenzung", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_CheckBox_SekPop_isBegrenzung.Location = New System.Drawing.Point(10, 45)
        Me.PES_CheckBox_SekPop_isBegrenzung.Name = "PES_CheckBox_SekPop_isBegrenzung"
        Me.PES_CheckBox_SekPop_isBegrenzung.Size = New System.Drawing.Size(121, 17)
        Me.PES_CheckBox_SekPop_isBegrenzung.TabIndex = 2
        Me.PES_CheckBox_SekPop_isBegrenzung.Text = "Limit size of sec pop"
        Me.PES_CheckBox_SekPop_isBegrenzung.UseVisualStyleBackColor = True
        '
        'BindingSource_PES_SekPop
        '
        Me.BindingSource_PES_SekPop.DataSource = GetType(BlueM.Opt.Common.Settings_PES.Settings_SekPop)
        '
        'PES_Numeric_nInteract
        '
        Me.PES_Numeric_nInteract.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_PES_SekPop, "N_Interact", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Numeric_nInteract.Location = New System.Drawing.Point(140, 22)
        Me.PES_Numeric_nInteract.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.PES_Numeric_nInteract.Name = "PES_Numeric_nInteract"
        Me.PES_Numeric_nInteract.Size = New System.Drawing.Size(53, 20)
        Me.PES_Numeric_nInteract.TabIndex = 1
        Me.PES_Numeric_nInteract.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip1.SetToolTip(Me.PES_Numeric_nInteract, "Every n generations")
        Me.PES_Numeric_nInteract.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'PES_Numeric_MaxMemberSekPop
        '
        Me.PES_Numeric_MaxMemberSekPop.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_PES_SekPop, "N_MaxMembers", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Numeric_MaxMemberSekPop.DataBindings.Add(New System.Windows.Forms.Binding("Enabled", Me.BindingSource_PES_SekPop, "Is_Begrenzung", True, System.Windows.Forms.DataSourceUpdateMode.Never))
        Me.PES_Numeric_MaxMemberSekPop.Location = New System.Drawing.Point(140, 65)
        Me.PES_Numeric_MaxMemberSekPop.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.PES_Numeric_MaxMemberSekPop.Name = "PES_Numeric_MaxMemberSekPop"
        Me.PES_Numeric_MaxMemberSekPop.Size = New System.Drawing.Size(53, 20)
        Me.PES_Numeric_MaxMemberSekPop.TabIndex = 4
        Me.PES_Numeric_MaxMemberSekPop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.PES_Numeric_MaxMemberSekPop.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'LabelMaxMemberSekPop
        '
        Me.LabelMaxMemberSekPop.AutoSize = True
        Me.LabelMaxMemberSekPop.DataBindings.Add(New System.Windows.Forms.Binding("Enabled", Me.BindingSource_PES_SekPop, "Is_Begrenzung", True, System.Windows.Forms.DataSourceUpdateMode.Never))
        Me.LabelMaxMemberSekPop.Location = New System.Drawing.Point(26, 67)
        Me.LabelMaxMemberSekPop.Name = "LabelMaxMemberSekPop"
        Me.LabelMaxMemberSekPop.Size = New System.Drawing.Size(108, 13)
        Me.LabelMaxMemberSekPop.TabIndex = 3
        Me.LabelMaxMemberSekPop.Text = "Max. no. of members:"
        '
        'PES_Checkbox_isPopul
        '
        Me.PES_Checkbox_isPopul.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.BindingSource_PES_Pop, "Is_POPUL", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Checkbox_isPopul.DataBindings.Add(New System.Windows.Forms.Binding("Enabled", Me.BindingSource_PES, "PopulEnabled", True, System.Windows.Forms.DataSourceUpdateMode.Never))
        Me.PES_Checkbox_isPopul.Location = New System.Drawing.Point(5, 469)
        Me.PES_Checkbox_isPopul.Name = "PES_Checkbox_isPopul"
        Me.PES_Checkbox_isPopul.Size = New System.Drawing.Size(112, 18)
        Me.PES_Checkbox_isPopul.TabIndex = 14
        Me.PES_Checkbox_isPopul.Text = "Use populations"
        '
        'BindingSource_PES_Pop
        '
        Me.BindingSource_PES_Pop.DataSource = GetType(BlueM.Opt.Common.Settings_PES.Settings_Pop)
        '
        'PES_GroupBox_Populationen
        '
        Me.PES_GroupBox_Populationen.Controls.Add(LabelAnzRunden)
        Me.PES_GroupBox_Populationen.Controls.Add(Me.PES_Numeric_AnzRunden)
        Me.PES_GroupBox_Populationen.Controls.Add(LabelAnzPop)
        Me.PES_GroupBox_Populationen.Controls.Add(Me.PES_Numeric_AnzPop)
        Me.PES_GroupBox_Populationen.Controls.Add(LabelAnzPopEltern)
        Me.PES_GroupBox_Populationen.Controls.Add(Me.PES_Numeric_AnzPopEltern)
        Me.PES_GroupBox_Populationen.Controls.Add(LabelOptPopEltern)
        Me.PES_GroupBox_Populationen.Controls.Add(Me.PES_Combo_PopEltern)
        Me.PES_GroupBox_Populationen.Controls.Add(LabelPopStrategie)
        Me.PES_GroupBox_Populationen.Controls.Add(Me.PES_Combo_PopStrategie)
        Me.PES_GroupBox_Populationen.Controls.Add(LabelPopPenalty)
        Me.PES_GroupBox_Populationen.Controls.Add(Me.PES_Combo_PopPenalty)
        Me.PES_GroupBox_Populationen.DataBindings.Add(New System.Windows.Forms.Binding("Enabled", Me.BindingSource_PES_Pop, "Is_POPUL", True, System.Windows.Forms.DataSourceUpdateMode.Never))
        Me.PES_GroupBox_Populationen.Location = New System.Drawing.Point(4, 493)
        Me.PES_GroupBox_Populationen.Name = "PES_GroupBox_Populationen"
        Me.PES_GroupBox_Populationen.Size = New System.Drawing.Size(200, 158)
        Me.PES_GroupBox_Populationen.TabIndex = 0
        Me.PES_GroupBox_Populationen.TabStop = False
        Me.PES_GroupBox_Populationen.Text = "Populations"
        '
        'PES_Numeric_AnzRunden
        '
        Me.PES_Numeric_AnzRunden.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_PES_Pop, "N_Runden", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Numeric_AnzRunden.Location = New System.Drawing.Point(140, 13)
        Me.PES_Numeric_AnzRunden.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
        Me.PES_Numeric_AnzRunden.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.PES_Numeric_AnzRunden.Name = "PES_Numeric_AnzRunden"
        Me.PES_Numeric_AnzRunden.Size = New System.Drawing.Size(53, 20)
        Me.PES_Numeric_AnzRunden.TabIndex = 1
        Me.PES_Numeric_AnzRunden.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.PES_Numeric_AnzRunden.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'PES_Numeric_AnzPop
        '
        Me.PES_Numeric_AnzPop.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_PES_Pop, "N_Popul", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Numeric_AnzPop.Location = New System.Drawing.Point(140, 36)
        Me.PES_Numeric_AnzPop.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.PES_Numeric_AnzPop.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.PES_Numeric_AnzPop.Name = "PES_Numeric_AnzPop"
        Me.PES_Numeric_AnzPop.Size = New System.Drawing.Size(53, 20)
        Me.PES_Numeric_AnzPop.TabIndex = 3
        Me.PES_Numeric_AnzPop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.PES_Numeric_AnzPop.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'PES_Numeric_AnzPopEltern
        '
        Me.PES_Numeric_AnzPopEltern.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_PES_Pop, "N_PopEltern", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Numeric_AnzPopEltern.Location = New System.Drawing.Point(140, 58)
        Me.PES_Numeric_AnzPopEltern.Maximum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.PES_Numeric_AnzPopEltern.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.PES_Numeric_AnzPopEltern.Name = "PES_Numeric_AnzPopEltern"
        Me.PES_Numeric_AnzPopEltern.Size = New System.Drawing.Size(53, 20)
        Me.PES_Numeric_AnzPopEltern.TabIndex = 5
        Me.PES_Numeric_AnzPopEltern.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.PES_Numeric_AnzPopEltern.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'PES_Combo_PopEltern
        '
        Me.PES_Combo_PopEltern.DataBindings.Add(New System.Windows.Forms.Binding("SelectedItem", Me.BindingSource_PES_Pop, "PopEltern", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Combo_PopEltern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.PES_Combo_PopEltern.Location = New System.Drawing.Point(85, 84)
        Me.PES_Combo_PopEltern.Name = "PES_Combo_PopEltern"
        Me.PES_Combo_PopEltern.Size = New System.Drawing.Size(108, 21)
        Me.PES_Combo_PopEltern.TabIndex = 7
        '
        'PES_Combo_PopStrategie
        '
        Me.PES_Combo_PopStrategie.DataBindings.Add(New System.Windows.Forms.Binding("SelectedItem", Me.BindingSource_PES_Pop, "PopStrategie", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Combo_PopStrategie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.PES_Combo_PopStrategie.Location = New System.Drawing.Point(85, 108)
        Me.PES_Combo_PopStrategie.Name = "PES_Combo_PopStrategie"
        Me.PES_Combo_PopStrategie.Size = New System.Drawing.Size(108, 21)
        Me.PES_Combo_PopStrategie.TabIndex = 9
        '
        'PES_Combo_PopPenalty
        '
        Me.PES_Combo_PopPenalty.DataBindings.Add(New System.Windows.Forms.Binding("SelectedItem", Me.BindingSource_PES_Pop, "PopPenalty", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.PES_Combo_PopPenalty.DataSource = Me.BindingSource_PES_PopPenaltyOptions
        Me.PES_Combo_PopPenalty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.PES_Combo_PopPenalty.Location = New System.Drawing.Point(85, 132)
        Me.PES_Combo_PopPenalty.Name = "PES_Combo_PopPenalty"
        Me.PES_Combo_PopPenalty.Size = New System.Drawing.Size(108, 21)
        Me.PES_Combo_PopPenalty.TabIndex = 11
        '
        'BindingSource_PES_PopPenaltyOptions
        '
        Me.BindingSource_PES_PopPenaltyOptions.DataMember = "PopPenaltyOptions"
        Me.BindingSource_PES_PopPenaltyOptions.DataSource = Me.BindingSource_PES
        '
        'TabPage_HookeJeeves
        '
        Me.TabPage_HookeJeeves.AutoScroll = True
        Me.TabPage_HookeJeeves.BackColor = System.Drawing.Color.Transparent
        Me.TabPage_HookeJeeves.Controls.Add(Label3)
        Me.TabPage_HookeJeeves.Controls.Add(Me.HJ_Numeric_DeltaFinish)
        Me.TabPage_HookeJeeves.Controls.Add(Label1)
        Me.TabPage_HookeJeeves.Controls.Add(Me.HJ_Numeric_DeltaStart)
        Me.TabPage_HookeJeeves.Controls.Add(Me.HJ_CheckBox_DNVektor)
        Me.TabPage_HookeJeeves.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage_HookeJeeves.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_HookeJeeves.Name = "TabPage_HookeJeeves"
        Me.TabPage_HookeJeeves.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_HookeJeeves.Size = New System.Drawing.Size(221, 668)
        Me.TabPage_HookeJeeves.TabIndex = 2
        Me.TabPage_HookeJeeves.Text = "Hooke&Jeeves"
        Me.TabPage_HookeJeeves.UseVisualStyleBackColor = True
        '
        'HJ_Numeric_DeltaFinish
        '
        Me.HJ_Numeric_DeltaFinish.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_HookeJeeves, "DnFinish", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.HJ_Numeric_DeltaFinish.DecimalPlaces = 5
        Me.HJ_Numeric_DeltaFinish.Increment = New Decimal(New Integer() {1, 0, 0, 327680})
        Me.HJ_Numeric_DeltaFinish.Location = New System.Drawing.Point(142, 39)
        Me.HJ_Numeric_DeltaFinish.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.HJ_Numeric_DeltaFinish.Minimum = New Decimal(New Integer() {1, 0, 0, 327680})
        Me.HJ_Numeric_DeltaFinish.Name = "HJ_Numeric_DeltaFinish"
        Me.HJ_Numeric_DeltaFinish.Size = New System.Drawing.Size(65, 20)
        Me.HJ_Numeric_DeltaFinish.TabIndex = 1
        Me.HJ_Numeric_DeltaFinish.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.HJ_Numeric_DeltaFinish.Value = New Decimal(New Integer() {1, 0, 0, 262144})
        '
        'BindingSource_HookeJeeves
        '
        Me.BindingSource_HookeJeeves.DataSource = GetType(BlueM.Opt.Common.Settings_HookeJeeves)
        '
        'HJ_Numeric_DeltaStart
        '
        Me.HJ_Numeric_DeltaStart.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_HookeJeeves, "DnStart", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.HJ_Numeric_DeltaStart.DecimalPlaces = 2
        Me.HJ_Numeric_DeltaStart.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.HJ_Numeric_DeltaStart.Location = New System.Drawing.Point(142, 9)
        Me.HJ_Numeric_DeltaStart.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.HJ_Numeric_DeltaStart.Minimum = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.HJ_Numeric_DeltaStart.Name = "HJ_Numeric_DeltaStart"
        Me.HJ_Numeric_DeltaStart.Size = New System.Drawing.Size(65, 20)
        Me.HJ_Numeric_DeltaStart.TabIndex = 0
        Me.HJ_Numeric_DeltaStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.HJ_Numeric_DeltaStart.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'HJ_CheckBox_DNVektor
        '
        Me.HJ_CheckBox_DNVektor.AutoSize = True
        Me.HJ_CheckBox_DNVektor.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.BindingSource_HookeJeeves, "Is_DnVektor", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.HJ_CheckBox_DNVektor.Enabled = False
        Me.HJ_CheckBox_DNVektor.Location = New System.Drawing.Point(15, 76)
        Me.HJ_CheckBox_DNVektor.Name = "HJ_CheckBox_DNVektor"
        Me.HJ_CheckBox_DNVektor.Size = New System.Drawing.Size(133, 17)
        Me.HJ_CheckBox_DNVektor.TabIndex = 2
        Me.HJ_CheckBox_DNVektor.Text = "mit Schrittweitenvektor"
        '
        'TabPage_MetaEvo
        '
        Me.TabPage_MetaEvo.AutoScroll = True
        Me.TabPage_MetaEvo.BackColor = System.Drawing.Color.Transparent
        Me.TabPage_MetaEvo.Controls.Add(Me.GroupBox_MetaEvo_TransferOptions)
        Me.TabPage_MetaEvo.Controls.Add(Me.GroupBox_MetaEvo_LocalOptions)
        Me.TabPage_MetaEvo.Controls.Add(Label18)
        Me.TabPage_MetaEvo.Controls.Add(Me.MetaEvo_Combo_OpMode)
        Me.TabPage_MetaEvo.Controls.Add(Me.GroupBox_MetaEvo_MySQLOptions)
        Me.TabPage_MetaEvo.Controls.Add(Label_Meta5)
        Me.TabPage_MetaEvo.Controls.Add(Me.GroupBox_MetaEvo_BasicOptions)
        Me.TabPage_MetaEvo.Controls.Add(Me.MetaEvo_Combo_Role)
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
        Me.GroupBox_MetaEvo_TransferOptions.Controls.Add(Me.MetaEvo_Numeric_NumberResults)
        Me.GroupBox_MetaEvo_TransferOptions.DataBindings.Add(New System.Windows.Forms.Binding("Enabled", Me.BindingSource_MetaEvo, "TransferOptionsEnabled", True, System.Windows.Forms.DataSourceUpdateMode.Never))
        Me.GroupBox_MetaEvo_TransferOptions.Location = New System.Drawing.Point(6, 159)
        Me.GroupBox_MetaEvo_TransferOptions.Name = "GroupBox_MetaEvo_TransferOptions"
        Me.GroupBox_MetaEvo_TransferOptions.Size = New System.Drawing.Size(200, 48)
        Me.GroupBox_MetaEvo_TransferOptions.TabIndex = 54
        Me.GroupBox_MetaEvo_TransferOptions.TabStop = False
        Me.GroupBox_MetaEvo_TransferOptions.Text = "Transfer Options"
        '
        'MetaEvo_Numeric_NumberResults
        '
        Me.MetaEvo_Numeric_NumberResults.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_MetaEvo, "NumberResults", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.MetaEvo_Numeric_NumberResults.Location = New System.Drawing.Point(139, 14)
        Me.MetaEvo_Numeric_NumberResults.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
        Me.MetaEvo_Numeric_NumberResults.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.MetaEvo_Numeric_NumberResults.Name = "MetaEvo_Numeric_NumberResults"
        Me.MetaEvo_Numeric_NumberResults.Size = New System.Drawing.Size(53, 20)
        Me.MetaEvo_Numeric_NumberResults.TabIndex = 56
        Me.MetaEvo_Numeric_NumberResults.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.MetaEvo_Numeric_NumberResults.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'BindingSource_MetaEvo
        '
        Me.BindingSource_MetaEvo.DataSource = GetType(BlueM.Opt.Common.Settings_MetaEvo)
        '
        'GroupBox_MetaEvo_LocalOptions
        '
        Me.GroupBox_MetaEvo_LocalOptions.Controls.Add(Label20)
        Me.GroupBox_MetaEvo_LocalOptions.Controls.Add(Me.MetaEvo_Numeric_HJStepsize)
        Me.GroupBox_MetaEvo_LocalOptions.DataBindings.Add(New System.Windows.Forms.Binding("Enabled", Me.BindingSource_MetaEvo, "LocalOptionsEnabled", True, System.Windows.Forms.DataSourceUpdateMode.Never))
        Me.GroupBox_MetaEvo_LocalOptions.Enabled = False
        Me.GroupBox_MetaEvo_LocalOptions.Location = New System.Drawing.Point(6, 213)
        Me.GroupBox_MetaEvo_LocalOptions.Name = "GroupBox_MetaEvo_LocalOptions"
        Me.GroupBox_MetaEvo_LocalOptions.Size = New System.Drawing.Size(200, 40)
        Me.GroupBox_MetaEvo_LocalOptions.TabIndex = 53
        Me.GroupBox_MetaEvo_LocalOptions.TabStop = False
        Me.GroupBox_MetaEvo_LocalOptions.Text = "Local Options"
        '
        'MetaEvo_Numeric_HJStepsize
        '
        Me.MetaEvo_Numeric_HJStepsize.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_MetaEvo, "HJStepsize", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.MetaEvo_Numeric_HJStepsize.Location = New System.Drawing.Point(139, 13)
        Me.MetaEvo_Numeric_HJStepsize.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
        Me.MetaEvo_Numeric_HJStepsize.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.MetaEvo_Numeric_HJStepsize.Name = "MetaEvo_Numeric_HJStepsize"
        Me.MetaEvo_Numeric_HJStepsize.Size = New System.Drawing.Size(53, 20)
        Me.MetaEvo_Numeric_HJStepsize.TabIndex = 50
        Me.MetaEvo_Numeric_HJStepsize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.MetaEvo_Numeric_HJStepsize.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'MetaEvo_Combo_OpMode
        '
        Me.MetaEvo_Combo_OpMode.DataBindings.Add(New System.Windows.Forms.Binding("SelectedItem", Me.BindingSource_MetaEvo, "OpMode", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.MetaEvo_Combo_OpMode.DataBindings.Add(New System.Windows.Forms.Binding("Enabled", Me.BindingSource_MetaEvo, "OpModeEnabled", True, System.Windows.Forms.DataSourceUpdateMode.Never))
        Me.MetaEvo_Combo_OpMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.MetaEvo_Combo_OpMode.FormattingEnabled = True
        Me.MetaEvo_Combo_OpMode.Items.AddRange(New Object() {"Local Optimizer", "Global Optimizer", "Both"})
        Me.MetaEvo_Combo_OpMode.Location = New System.Drawing.Point(103, 43)
        Me.MetaEvo_Combo_OpMode.Name = "MetaEvo_Combo_OpMode"
        Me.MetaEvo_Combo_OpMode.Size = New System.Drawing.Size(100, 21)
        Me.MetaEvo_Combo_OpMode.TabIndex = 53
        '
        'GroupBox_MetaEvo_MySQLOptions
        '
        Me.GroupBox_MetaEvo_MySQLOptions.Controls.Add(Me.MetaEvo_TextBox_MySQL_DB)
        Me.GroupBox_MetaEvo_MySQLOptions.Controls.Add(Label16)
        Me.GroupBox_MetaEvo_MySQLOptions.Controls.Add(Me.MetaEvo_TextBox_MySQL_Password)
        Me.GroupBox_MetaEvo_MySQLOptions.Controls.Add(Me.MetaEvo_TextBox_MySQL_User)
        Me.GroupBox_MetaEvo_MySQLOptions.Controls.Add(Me.MetaEvo_TextBox_MySQL_Host)
        Me.GroupBox_MetaEvo_MySQLOptions.Controls.Add(Label13)
        Me.GroupBox_MetaEvo_MySQLOptions.Controls.Add(Label14)
        Me.GroupBox_MetaEvo_MySQLOptions.Controls.Add(Label15)
        Me.GroupBox_MetaEvo_MySQLOptions.DataBindings.Add(New System.Windows.Forms.Binding("Enabled", Me.BindingSource_MetaEvo, "MysqlOptionsEnabled", True, System.Windows.Forms.DataSourceUpdateMode.Never))
        Me.GroupBox_MetaEvo_MySQLOptions.Enabled = False
        Me.GroupBox_MetaEvo_MySQLOptions.Location = New System.Drawing.Point(6, 259)
        Me.GroupBox_MetaEvo_MySQLOptions.Name = "GroupBox_MetaEvo_MySQLOptions"
        Me.GroupBox_MetaEvo_MySQLOptions.Size = New System.Drawing.Size(200, 125)
        Me.GroupBox_MetaEvo_MySQLOptions.TabIndex = 52
        Me.GroupBox_MetaEvo_MySQLOptions.TabStop = False
        Me.GroupBox_MetaEvo_MySQLOptions.Text = "MySQL Options"
        '
        'MetaEvo_TextBox_MySQL_DB
        '
        Me.MetaEvo_TextBox_MySQL_DB.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.BindingSource_MetaEvo, "MySQL_Database", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.MetaEvo_TextBox_MySQL_DB.Location = New System.Drawing.Point(92, 45)
        Me.MetaEvo_TextBox_MySQL_DB.Name = "MetaEvo_TextBox_MySQL_DB"
        Me.MetaEvo_TextBox_MySQL_DB.Size = New System.Drawing.Size(100, 20)
        Me.MetaEvo_TextBox_MySQL_DB.TabIndex = 56
        '
        'MetaEvo_TextBox_MySQL_Password
        '
        Me.MetaEvo_TextBox_MySQL_Password.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.BindingSource_MetaEvo, "MySQL_Password", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.MetaEvo_TextBox_MySQL_Password.Location = New System.Drawing.Point(92, 97)
        Me.MetaEvo_TextBox_MySQL_Password.Name = "MetaEvo_TextBox_MySQL_Password"
        Me.MetaEvo_TextBox_MySQL_Password.Size = New System.Drawing.Size(100, 20)
        Me.MetaEvo_TextBox_MySQL_Password.TabIndex = 54
        Me.MetaEvo_TextBox_MySQL_Password.UseSystemPasswordChar = True
        '
        'MetaEvo_TextBox_MySQL_User
        '
        Me.MetaEvo_TextBox_MySQL_User.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.BindingSource_MetaEvo, "MySQL_User", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.MetaEvo_TextBox_MySQL_User.Location = New System.Drawing.Point(92, 71)
        Me.MetaEvo_TextBox_MySQL_User.Name = "MetaEvo_TextBox_MySQL_User"
        Me.MetaEvo_TextBox_MySQL_User.Size = New System.Drawing.Size(100, 20)
        Me.MetaEvo_TextBox_MySQL_User.TabIndex = 53
        '
        'MetaEvo_TextBox_MySQL_Host
        '
        Me.MetaEvo_TextBox_MySQL_Host.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.BindingSource_MetaEvo, "MySQL_Host", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.MetaEvo_TextBox_MySQL_Host.Location = New System.Drawing.Point(92, 19)
        Me.MetaEvo_TextBox_MySQL_Host.Name = "MetaEvo_TextBox_MySQL_Host"
        Me.MetaEvo_TextBox_MySQL_Host.Size = New System.Drawing.Size(100, 20)
        Me.MetaEvo_TextBox_MySQL_Host.TabIndex = 52
        '
        'GroupBox_MetaEvo_BasicOptions
        '
        Me.GroupBox_MetaEvo_BasicOptions.Controls.Add(Label_Meta10)
        Me.GroupBox_MetaEvo_BasicOptions.Controls.Add(Me.MetaEvo_Numeric_PopulationSize)
        Me.GroupBox_MetaEvo_BasicOptions.Controls.Add(Label_Meta11)
        Me.GroupBox_MetaEvo_BasicOptions.Controls.Add(Me.MetaEvo_Numeric_Numbergenerations)
        Me.GroupBox_MetaEvo_BasicOptions.DataBindings.Add(New System.Windows.Forms.Binding("Enabled", Me.BindingSource_MetaEvo, "GlobalOptionsEnabled", True, System.Windows.Forms.DataSourceUpdateMode.Never))
        Me.GroupBox_MetaEvo_BasicOptions.Enabled = False
        Me.GroupBox_MetaEvo_BasicOptions.Location = New System.Drawing.Point(6, 70)
        Me.GroupBox_MetaEvo_BasicOptions.Name = "GroupBox_MetaEvo_BasicOptions"
        Me.GroupBox_MetaEvo_BasicOptions.Size = New System.Drawing.Size(200, 83)
        Me.GroupBox_MetaEvo_BasicOptions.TabIndex = 36
        Me.GroupBox_MetaEvo_BasicOptions.TabStop = False
        Me.GroupBox_MetaEvo_BasicOptions.Text = "Global Options"
        '
        'MetaEvo_Numeric_PopulationSize
        '
        Me.MetaEvo_Numeric_PopulationSize.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_MetaEvo, "PopulationSize", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.MetaEvo_Numeric_PopulationSize.Location = New System.Drawing.Point(139, 19)
        Me.MetaEvo_Numeric_PopulationSize.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
        Me.MetaEvo_Numeric_PopulationSize.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.MetaEvo_Numeric_PopulationSize.Name = "MetaEvo_Numeric_PopulationSize"
        Me.MetaEvo_Numeric_PopulationSize.Size = New System.Drawing.Size(53, 20)
        Me.MetaEvo_Numeric_PopulationSize.TabIndex = 50
        Me.MetaEvo_Numeric_PopulationSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.MetaEvo_Numeric_PopulationSize.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'MetaEvo_Numeric_Numbergenerations
        '
        Me.MetaEvo_Numeric_Numbergenerations.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_MetaEvo, "NumberGenerations", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.MetaEvo_Numeric_Numbergenerations.Location = New System.Drawing.Point(139, 54)
        Me.MetaEvo_Numeric_Numbergenerations.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
        Me.MetaEvo_Numeric_Numbergenerations.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.MetaEvo_Numeric_Numbergenerations.Name = "MetaEvo_Numeric_Numbergenerations"
        Me.MetaEvo_Numeric_Numbergenerations.Size = New System.Drawing.Size(53, 20)
        Me.MetaEvo_Numeric_Numbergenerations.TabIndex = 34
        Me.MetaEvo_Numeric_Numbergenerations.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.MetaEvo_Numeric_Numbergenerations.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'MetaEvo_Combo_Role
        '
        Me.MetaEvo_Combo_Role.DataBindings.Add(New System.Windows.Forms.Binding("SelectedItem", Me.BindingSource_MetaEvo, "Role", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.MetaEvo_Combo_Role.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.MetaEvo_Combo_Role.FormattingEnabled = True
        Me.MetaEvo_Combo_Role.Items.AddRange(New Object() {"Single PC", "Network Server", "Network Client"})
        Me.MetaEvo_Combo_Role.Location = New System.Drawing.Point(103, 16)
        Me.MetaEvo_Combo_Role.Name = "MetaEvo_Combo_Role"
        Me.MetaEvo_Combo_Role.Size = New System.Drawing.Size(100, 21)
        Me.MetaEvo_Combo_Role.TabIndex = 0
        '
        'TabPage_DDS
        '
        Me.TabPage_DDS.AutoScroll = True
        Me.TabPage_DDS.Controls.Add(Me.DDS_CheckBox_RandomStartparameters)
        Me.TabPage_DDS.Controls.Add(Label11)
        Me.TabPage_DDS.Controls.Add(Me.DDS_Numeric_maxiter)
        Me.TabPage_DDS.Controls.Add(Label10)
        Me.TabPage_DDS.Controls.Add(Me.DDS_Numeric_r_val)
        Me.TabPage_DDS.Controls.Add(Label5)
        Me.TabPage_DDS.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_DDS.Name = "TabPage_DDS"
        Me.TabPage_DDS.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_DDS.Size = New System.Drawing.Size(221, 668)
        Me.TabPage_DDS.TabIndex = 4
        Me.TabPage_DDS.Text = "DDS"
        Me.TabPage_DDS.UseVisualStyleBackColor = True
        '
        'DDS_CheckBox_RandomStartparameters
        '
        Me.DDS_CheckBox_RandomStartparameters.AutoSize = True
        Me.DDS_CheckBox_RandomStartparameters.Checked = True
        Me.DDS_CheckBox_RandomStartparameters.CheckState = System.Windows.Forms.CheckState.Checked
        Me.DDS_CheckBox_RandomStartparameters.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.BindingSource_DDS, "RandomStartparameters", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.DDS_CheckBox_RandomStartparameters.Location = New System.Drawing.Point(161, 66)
        Me.DDS_CheckBox_RandomStartparameters.Name = "DDS_CheckBox_RandomStartparameters"
        Me.DDS_CheckBox_RandomStartparameters.Size = New System.Drawing.Size(15, 14)
        Me.DDS_CheckBox_RandomStartparameters.TabIndex = 5
        Me.DDS_CheckBox_RandomStartparameters.UseVisualStyleBackColor = True
        '
        'BindingSource_DDS
        '
        Me.BindingSource_DDS.DataSource = GetType(BlueM.Opt.Common.Settings_DDS)
        '
        'DDS_Numeric_maxiter
        '
        Me.DDS_Numeric_maxiter.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_DDS, "MaxIter", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.DDS_Numeric_maxiter.Increment = New Decimal(New Integer() {100, 0, 0, 0})
        Me.DDS_Numeric_maxiter.Location = New System.Drawing.Point(161, 42)
        Me.DDS_Numeric_maxiter.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.DDS_Numeric_maxiter.Minimum = New Decimal(New Integer() {100, 0, 0, 0})
        Me.DDS_Numeric_maxiter.Name = "DDS_Numeric_maxiter"
        Me.DDS_Numeric_maxiter.Size = New System.Drawing.Size(55, 20)
        Me.DDS_Numeric_maxiter.TabIndex = 3
        Me.DDS_Numeric_maxiter.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'DDS_Numeric_r_val
        '
        Me.DDS_Numeric_r_val.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_DDS, "R_val", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.DDS_Numeric_r_val.DecimalPlaces = 3
        Me.DDS_Numeric_r_val.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DDS_Numeric_r_val.Increment = New Decimal(New Integer() {25, 0, 0, 196608})
        Me.DDS_Numeric_r_val.Location = New System.Drawing.Point(161, 16)
        Me.DDS_Numeric_r_val.Maximum = New Decimal(New Integer() {10, 0, 0, 65536})
        Me.DDS_Numeric_r_val.Name = "DDS_Numeric_r_val"
        Me.DDS_Numeric_r_val.Size = New System.Drawing.Size(55, 20)
        Me.DDS_Numeric_r_val.TabIndex = 1
        Me.DDS_Numeric_r_val.Value = New Decimal(New Integer() {2, 0, 0, 65536})
        '
        'TabPage_SensiPlot
        '
        Me.TabPage_SensiPlot.AutoScroll = True
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_Label_NumSims)
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_CheckBox_SaveResults)
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_CheckBox_wave)
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_Label_NumSteps)
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_NumericUpDown_NumSteps)
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_GroupBox_Modus)
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_Label_Objectives)
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_ListBox_Objectives)
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_Label_OptParameter)
        Me.TabPage_SensiPlot.Controls.Add(Me.SensiPlot_ListBox_OptParameter)
        Me.TabPage_SensiPlot.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_SensiPlot.Margin = New System.Windows.Forms.Padding(0)
        Me.TabPage_SensiPlot.Name = "TabPage_SensiPlot"
        Me.TabPage_SensiPlot.Size = New System.Drawing.Size(221, 668)
        Me.TabPage_SensiPlot.TabIndex = 5
        Me.TabPage_SensiPlot.Text = "SensiPlot"
        Me.TabPage_SensiPlot.UseVisualStyleBackColor = True
        '
        'SensiPlot_Label_NumSims
        '
        Me.SensiPlot_Label_NumSims.AutoSize = True
        Me.SensiPlot_Label_NumSims.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.SensiPlot_Label_NumSims.Location = New System.Drawing.Point(126, 444)
        Me.SensiPlot_Label_NumSims.Name = "SensiPlot_Label_NumSims"
        Me.SensiPlot_Label_NumSims.Size = New System.Drawing.Size(74, 13)
        Me.SensiPlot_Label_NumSims.TabIndex = 17
        Me.SensiPlot_Label_NumSims.Text = "(X simulations)"
        '
        'SensiPlot_CheckBox_SaveResults
        '
        Me.SensiPlot_CheckBox_SaveResults.AutoSize = True
        Me.SensiPlot_CheckBox_SaveResults.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.BindingSource_Sensiplot, "Save_Results", True))
        Me.SensiPlot_CheckBox_SaveResults.Location = New System.Drawing.Point(6, 491)
        Me.SensiPlot_CheckBox_SaveResults.Name = "SensiPlot_CheckBox_SaveResults"
        Me.SensiPlot_CheckBox_SaveResults.Size = New System.Drawing.Size(141, 17)
        Me.SensiPlot_CheckBox_SaveResults.TabIndex = 16
        Me.SensiPlot_CheckBox_SaveResults.Text = "Save individual datasets"
        Me.ToolTip1.SetToolTip(Me.SensiPlot_CheckBox_SaveResults, "If activated, simulations are carried out in subfolders named ""sensiplot_XXXX""")
        Me.SensiPlot_CheckBox_SaveResults.UseVisualStyleBackColor = True
        '
        'BindingSource_Sensiplot
        '
        Me.BindingSource_Sensiplot.DataSource = GetType(BlueM.Opt.Common.Settings_Sensiplot)
        '
        'SensiPlot_CheckBox_wave
        '
        Me.SensiPlot_CheckBox_wave.AutoSize = True
        Me.SensiPlot_CheckBox_wave.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.BindingSource_Sensiplot, "Show_Wave", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.SensiPlot_CheckBox_wave.Location = New System.Drawing.Point(6, 468)
        Me.SensiPlot_CheckBox_wave.Name = "SensiPlot_CheckBox_wave"
        Me.SensiPlot_CheckBox_wave.Size = New System.Drawing.Size(148, 17)
        Me.SensiPlot_CheckBox_wave.TabIndex = 15
        Me.SensiPlot_CheckBox_wave.Text = "Show time series in Wave"
        Me.ToolTip1.SetToolTip(Me.SensiPlot_CheckBox_wave, "Show the hydrographs of all simulation results in BlueM.Wave after the sensitivit" &
        "y analysis has completed")
        Me.SensiPlot_CheckBox_wave.UseVisualStyleBackColor = True
        '
        'SensiPlot_Label_NumSteps
        '
        Me.SensiPlot_Label_NumSteps.AutoSize = True
        Me.SensiPlot_Label_NumSteps.Location = New System.Drawing.Point(3, 444)
        Me.SensiPlot_Label_NumSteps.Name = "SensiPlot_Label_NumSteps"
        Me.SensiPlot_Label_NumSteps.Size = New System.Drawing.Size(67, 13)
        Me.SensiPlot_Label_NumSteps.TabIndex = 14
        Me.SensiPlot_Label_NumSteps.Text = "No. of steps:"
        '
        'SensiPlot_NumericUpDown_NumSteps
        '
        Me.SensiPlot_NumericUpDown_NumSteps.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_Sensiplot, "Num_Steps", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.SensiPlot_NumericUpDown_NumSteps.Location = New System.Drawing.Point(76, 442)
        Me.SensiPlot_NumericUpDown_NumSteps.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.SensiPlot_NumericUpDown_NumSteps.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.SensiPlot_NumericUpDown_NumSteps.Name = "SensiPlot_NumericUpDown_NumSteps"
        Me.SensiPlot_NumericUpDown_NumSteps.Size = New System.Drawing.Size(44, 20)
        Me.SensiPlot_NumericUpDown_NumSteps.TabIndex = 13
        Me.SensiPlot_NumericUpDown_NumSteps.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.SensiPlot_NumericUpDown_NumSteps.Value = New Decimal(New Integer() {11, 0, 0, 0})
        '
        'SensiPlot_GroupBox_Modus
        '
        Me.SensiPlot_GroupBox_Modus.Controls.Add(Me.SensiPlot_RadioButton_ModeLatinHypercube)
        Me.SensiPlot_GroupBox_Modus.Controls.Add(Me.SensiPlot_RadioButton_ModeEvenDistribution)
        Me.SensiPlot_GroupBox_Modus.Controls.Add(Me.SensiPlot_RadioButton_ModeRandom)
        Me.SensiPlot_GroupBox_Modus.Location = New System.Drawing.Point(3, 344)
        Me.SensiPlot_GroupBox_Modus.Name = "SensiPlot_GroupBox_Modus"
        Me.SensiPlot_GroupBox_Modus.Size = New System.Drawing.Size(209, 92)
        Me.SensiPlot_GroupBox_Modus.TabIndex = 12
        Me.SensiPlot_GroupBox_Modus.TabStop = False
        Me.SensiPlot_GroupBox_Modus.Text = "Mode"
        '
        'SensiPlot_RadioButton_ModeLatinHypercube
        '
        Me.SensiPlot_RadioButton_ModeLatinHypercube.AutoSize = True
        Me.SensiPlot_RadioButton_ModeLatinHypercube.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.BindingSource_Sensiplot, "ModeLatinHypercube", True))
        Me.SensiPlot_RadioButton_ModeLatinHypercube.Location = New System.Drawing.Point(9, 65)
        Me.SensiPlot_RadioButton_ModeLatinHypercube.Name = "SensiPlot_RadioButton_ModeLatinHypercube"
        Me.SensiPlot_RadioButton_ModeLatinHypercube.Size = New System.Drawing.Size(149, 17)
        Me.SensiPlot_RadioButton_ModeLatinHypercube.TabIndex = 11
        Me.SensiPlot_RadioButton_ModeLatinHypercube.Text = "Latin Hypercube Sampling"
        Me.SensiPlot_RadioButton_ModeLatinHypercube.UseVisualStyleBackColor = True
        '
        'SensiPlot_RadioButton_ModeEvenDistribution
        '
        Me.SensiPlot_RadioButton_ModeEvenDistribution.AutoSize = True
        Me.SensiPlot_RadioButton_ModeEvenDistribution.Checked = True
        Me.SensiPlot_RadioButton_ModeEvenDistribution.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.BindingSource_Sensiplot, "ModeEvenDistribution", True))
        Me.SensiPlot_RadioButton_ModeEvenDistribution.Location = New System.Drawing.Point(9, 42)
        Me.SensiPlot_RadioButton_ModeEvenDistribution.Name = "SensiPlot_RadioButton_ModeEvenDistribution"
        Me.SensiPlot_RadioButton_ModeEvenDistribution.Size = New System.Drawing.Size(108, 17)
        Me.SensiPlot_RadioButton_ModeEvenDistribution.TabIndex = 10
        Me.SensiPlot_RadioButton_ModeEvenDistribution.TabStop = True
        Me.SensiPlot_RadioButton_ModeEvenDistribution.Text = "Evenly distributed"
        Me.SensiPlot_RadioButton_ModeEvenDistribution.UseVisualStyleBackColor = True
        '
        'SensiPlot_RadioButton_ModeRandom
        '
        Me.SensiPlot_RadioButton_ModeRandom.AutoSize = True
        Me.SensiPlot_RadioButton_ModeRandom.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.BindingSource_Sensiplot, "ModeRandomDistribution", True))
        Me.SensiPlot_RadioButton_ModeRandom.Location = New System.Drawing.Point(9, 19)
        Me.SensiPlot_RadioButton_ModeRandom.Name = "SensiPlot_RadioButton_ModeRandom"
        Me.SensiPlot_RadioButton_ModeRandom.Size = New System.Drawing.Size(123, 17)
        Me.SensiPlot_RadioButton_ModeRandom.TabIndex = 9
        Me.SensiPlot_RadioButton_ModeRandom.Text = "Randomly distributed"
        Me.SensiPlot_RadioButton_ModeRandom.UseVisualStyleBackColor = True
        '
        'SensiPlot_Label_Objectives
        '
        Me.SensiPlot_Label_Objectives.AutoSize = True
        Me.SensiPlot_Label_Objectives.Location = New System.Drawing.Point(3, 175)
        Me.SensiPlot_Label_Objectives.Name = "SensiPlot_Label_Objectives"
        Me.SensiPlot_Label_Objectives.Size = New System.Drawing.Size(212, 13)
        Me.SensiPlot_Label_Objectives.TabIndex = 6
        Me.SensiPlot_Label_Objectives.Text = "Objective function to show in main diagram:"
        '
        'SensiPlot_ListBox_Objectives
        '
        Me.SensiPlot_ListBox_Objectives.FormattingEnabled = True
        Me.SensiPlot_ListBox_Objectives.Location = New System.Drawing.Point(3, 191)
        Me.SensiPlot_ListBox_Objectives.Name = "SensiPlot_ListBox_Objectives"
        Me.SensiPlot_ListBox_Objectives.Size = New System.Drawing.Size(209, 147)
        Me.SensiPlot_ListBox_Objectives.TabIndex = 5
        '
        'SensiPlot_Label_OptParameter
        '
        Me.SensiPlot_Label_OptParameter.AutoSize = True
        Me.SensiPlot_Label_OptParameter.Location = New System.Drawing.Point(2, 9)
        Me.SensiPlot_Label_OptParameter.Name = "SensiPlot_Label_OptParameter"
        Me.SensiPlot_Label_OptParameter.Size = New System.Drawing.Size(157, 13)
        Me.SensiPlot_Label_OptParameter.TabIndex = 4
        Me.SensiPlot_Label_OptParameter.Text = "Optimization parameters to vary:"
        '
        'SensiPlot_ListBox_OptParameter
        '
        Me.SensiPlot_ListBox_OptParameter.Location = New System.Drawing.Point(3, 25)
        Me.SensiPlot_ListBox_OptParameter.Name = "SensiPlot_ListBox_OptParameter"
        Me.SensiPlot_ListBox_OptParameter.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.SensiPlot_ListBox_OptParameter.Size = New System.Drawing.Size(209, 147)
        Me.SensiPlot_ListBox_OptParameter.TabIndex = 3
        '
        'TabPage_TSP
        '
        Me.TabPage_TSP.AutoScroll = True
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
        Me.TSP_ComboBox_prob_instance.DataBindings.Add(New System.Windows.Forms.Binding("SelectedItem", Me.BindingSource_TSP, "Problem", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.TSP_ComboBox_prob_instance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.TSP_ComboBox_prob_instance.FormattingEnabled = True
        Me.TSP_ComboBox_prob_instance.Items.AddRange(New Object() {"Random", "Circle"})
        Me.TSP_ComboBox_prob_instance.Location = New System.Drawing.Point(105, 39)
        Me.TSP_ComboBox_prob_instance.Name = "TSP_ComboBox_prob_instance"
        Me.TSP_ComboBox_prob_instance.Size = New System.Drawing.Size(110, 21)
        Me.TSP_ComboBox_prob_instance.TabIndex = 57
        '
        'BindingSource_TSP
        '
        Me.BindingSource_TSP.DataSource = GetType(BlueM.Opt.Common.Settings_TSP)
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
        Me.TSP_ComboBox_Mutationoperator.DataBindings.Add(New System.Windows.Forms.Binding("SelectedItem", Me.BindingSource_TSP, "MutOperator", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
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
        Me.TSP_ComboBox_Reproductionoperator.DataBindings.Add(New System.Windows.Forms.Binding("SelectedItem", Me.BindingSource_TSP, "ReprodOperator", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
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
        Me.TSP_Numeric_n_generations.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_TSP, "N_Gen", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
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
        Me.TSP_Numeric_n_children.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_TSP, "N_Children", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
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
        Me.TSP_Numeric_n_parents.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_TSP, "N_Parents", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
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
        Me.TSP_Numeric_n_cities.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.BindingSource_TSP, "N_Cities", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
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
        Me.GroupBox_Einstellungen.Text = "Settings:"
        '
        'EVO_Einstellungen
        '
        Me.Controls.Add(Me.GroupBox_Einstellungen)
        Me.Name = "EVO_Einstellungen"
        Me.Size = New System.Drawing.Size(244, 753)
        GroupBox_Diagramm.ResumeLayout(False)
        CType(Me.BindingSource_General, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage_General.ResumeLayout(False)
        Me.GroupBox_Sim.ResumeLayout(False)
        Me.GroupBox_Sim.PerformLayout()
        CType(Me.NumericUpDown_NThreads, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage_PES.ResumeLayout(False)
        Me.TabPage_PES.PerformLayout()
        CType(Me.BindingSource_PES, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PES_Numeric_DnStart, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource_PES_Schrittweite, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_Generationen.ResumeLayout(False)
        Me.GroupBox_Generationen.PerformLayout()
        CType(Me.PES_Numeric_AnzNachf, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PES_Numeric_AnzEltern, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PES_Numeric_AnzGen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_Eltern.ResumeLayout(False)
        Me.GroupBox_Eltern.PerformLayout()
        CType(Me.PES_Numeric_Rekombxy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_SekPop.ResumeLayout(False)
        Me.GroupBox_SekPop.PerformLayout()
        CType(Me.BindingSource_PES_SekPop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PES_Numeric_nInteract, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PES_Numeric_MaxMemberSekPop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource_PES_Pop, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PES_GroupBox_Populationen.ResumeLayout(False)
        Me.PES_GroupBox_Populationen.PerformLayout()
        CType(Me.PES_Numeric_AnzRunden, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PES_Numeric_AnzPop, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PES_Numeric_AnzPopEltern, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource_PES_PopPenaltyOptions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage_HookeJeeves.ResumeLayout(False)
        Me.TabPage_HookeJeeves.PerformLayout()
        CType(Me.HJ_Numeric_DeltaFinish, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource_HookeJeeves, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HJ_Numeric_DeltaStart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage_MetaEvo.ResumeLayout(False)
        Me.TabPage_MetaEvo.PerformLayout()
        Me.GroupBox_MetaEvo_TransferOptions.ResumeLayout(False)
        Me.GroupBox_MetaEvo_TransferOptions.PerformLayout()
        CType(Me.MetaEvo_Numeric_NumberResults, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource_MetaEvo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_MetaEvo_LocalOptions.ResumeLayout(False)
        Me.GroupBox_MetaEvo_LocalOptions.PerformLayout()
        CType(Me.MetaEvo_Numeric_HJStepsize, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_MetaEvo_MySQLOptions.ResumeLayout(False)
        Me.GroupBox_MetaEvo_MySQLOptions.PerformLayout()
        Me.GroupBox_MetaEvo_BasicOptions.ResumeLayout(False)
        Me.GroupBox_MetaEvo_BasicOptions.PerformLayout()
        CType(Me.MetaEvo_Numeric_PopulationSize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MetaEvo_Numeric_Numbergenerations, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage_DDS.ResumeLayout(False)
        Me.TabPage_DDS.PerformLayout()
        CType(Me.BindingSource_DDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DDS_Numeric_maxiter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DDS_Numeric_r_val, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage_SensiPlot.ResumeLayout(False)
        Me.TabPage_SensiPlot.PerformLayout()
        CType(Me.BindingSource_Sensiplot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SensiPlot_NumericUpDown_NumSteps, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SensiPlot_GroupBox_Modus.ResumeLayout(False)
        Me.SensiPlot_GroupBox_Modus.PerformLayout()
        Me.TabPage_TSP.ResumeLayout(False)
        Me.TabPage_TSP.PerformLayout()
        CType(Me.BindingSource_TSP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TSP_Numeric_n_generations, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TSP_Numeric_n_children, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TSP_Numeric_n_parents, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TSP_Numeric_n_cities, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_Einstellungen.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Private WithEvents PES_Label_OptModus As System.Windows.Forms.Label
    Private WithEvents PES_Combo_Startparameter As System.Windows.Forms.ComboBox
    Private WithEvents PES_Numeric_DnStart As System.Windows.Forms.NumericUpDown
    Private WithEvents PES_Checkbox_isDnVektor As System.Windows.Forms.CheckBox
    Private WithEvents GroupBox_Generationen As System.Windows.Forms.GroupBox
    Private WithEvents PES_Numeric_MaxMemberSekPop As System.Windows.Forms.NumericUpDown
    Private WithEvents PES_Numeric_nInteract As System.Windows.Forms.NumericUpDown
    Private WithEvents PES_Numeric_Rekombxy As System.Windows.Forms.NumericUpDown
    Private WithEvents PES_Combo_OptEltern As System.Windows.Forms.ComboBox
    Private WithEvents PES_Numeric_AnzNachf As System.Windows.Forms.NumericUpDown
    Private WithEvents PES_Numeric_AnzEltern As System.Windows.Forms.NumericUpDown
    Private WithEvents PES_Numeric_AnzGen As System.Windows.Forms.NumericUpDown
    Private WithEvents LabelMaxMemberSekPop As System.Windows.Forms.Label
    Private WithEvents LabelRekombxy3 As System.Windows.Forms.Label
    Private WithEvents LabelRekombxy1 As System.Windows.Forms.Label
    Private WithEvents PES_Checkbox_isPopul As System.Windows.Forms.CheckBox
    Private WithEvents PES_GroupBox_Populationen As System.Windows.Forms.GroupBox
    Private WithEvents PES_Numeric_AnzRunden As System.Windows.Forms.NumericUpDown
    Private WithEvents PES_Numeric_AnzPop As System.Windows.Forms.NumericUpDown
    Private WithEvents PES_Numeric_AnzPopEltern As System.Windows.Forms.NumericUpDown
    Private WithEvents PES_Combo_PopEltern As System.Windows.Forms.ComboBox
    Private WithEvents PES_Combo_PopStrategie As System.Windows.Forms.ComboBox
    Private WithEvents PES_Combo_PopPenalty As System.Windows.Forms.ComboBox
    Private WithEvents GroupBox_Einstellungen As System.Windows.Forms.GroupBox
    Private WithEvents TabPage_HookeJeeves As System.Windows.Forms.TabPage
    Private WithEvents HJ_Numeric_DeltaFinish As System.Windows.Forms.NumericUpDown
    Private WithEvents HJ_Numeric_DeltaStart As System.Windows.Forms.NumericUpDown
    Private WithEvents HJ_CheckBox_DNVektor As System.Windows.Forms.CheckBox
    Private WithEvents TabPage_PES As System.Windows.Forms.TabPage
    Private WithEvents PES_Checkbox_isTournamentSelection As System.Windows.Forms.CheckBox
    Private WithEvents PES_Combo_DnMutation As System.Windows.Forms.ComboBox
    Private WithEvents GroupBox_SekPop As System.Windows.Forms.GroupBox
    Private WithEvents GroupBox_Eltern As System.Windows.Forms.GroupBox
    Private WithEvents PES_CheckBox_SekPop_isBegrenzung As System.Windows.Forms.CheckBox
    Private WithEvents LabelAnzEltern As System.Windows.Forms.Label
    Private WithEvents MetaEvo_Combo_Role As System.Windows.Forms.ComboBox
    Private WithEvents MetaEvo_Numeric_Numbergenerations As System.Windows.Forms.NumericUpDown
    Private WithEvents GroupBox_MetaEvo_BasicOptions As System.Windows.Forms.GroupBox
    Private WithEvents MetaEvo_Numeric_PopulationSize As System.Windows.Forms.NumericUpDown
    Private WithEvents GroupBox_MetaEvo_MySQLOptions As System.Windows.Forms.GroupBox
    Private WithEvents MetaEvo_TextBox_MySQL_Password As System.Windows.Forms.TextBox
    Private WithEvents MetaEvo_TextBox_MySQL_User As System.Windows.Forms.TextBox
    Private WithEvents MetaEvo_TextBox_MySQL_Host As System.Windows.Forms.TextBox
    Private WithEvents MetaEvo_TextBox_MySQL_DB As System.Windows.Forms.TextBox
    Private WithEvents TabPage_MetaEvo As System.Windows.Forms.TabPage
    Private WithEvents MetaEvo_Combo_OpMode As System.Windows.Forms.ComboBox
    Private WithEvents GroupBox_MetaEvo_LocalOptions As System.Windows.Forms.GroupBox
    Private WithEvents MetaEvo_Numeric_HJStepsize As System.Windows.Forms.NumericUpDown
    Private WithEvents GroupBox_MetaEvo_TransferOptions As System.Windows.Forms.GroupBox
    Private WithEvents MetaEvo_Numeric_NumberResults As System.Windows.Forms.NumericUpDown
    Private WithEvents TabPage_DDS As System.Windows.Forms.TabPage
    Private WithEvents DDS_Numeric_r_val As System.Windows.Forms.NumericUpDown
    Private WithEvents DDS_Numeric_maxiter As System.Windows.Forms.NumericUpDown
    Private WithEvents DDS_CheckBox_RandomStartparameters As System.Windows.Forms.CheckBox
    Private WithEvents TabPage_SensiPlot As System.Windows.Forms.TabPage
    Private WithEvents SensiPlot_Label_OptParameter As System.Windows.Forms.Label
    Private WithEvents SensiPlot_ListBox_OptParameter As System.Windows.Forms.ListBox
    Private WithEvents SensiPlot_ListBox_Objectives As System.Windows.Forms.ListBox
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
    Private WithEvents TSP_Label_n_cities As System.Windows.Forms.Label
    Private WithEvents TSP_Numeric_n_generations As System.Windows.Forms.NumericUpDown
    Private WithEvents TSP_Label_n_generations As System.Windows.Forms.Label
    Private WithEvents TSP_Numeric_n_children As System.Windows.Forms.NumericUpDown
    Private WithEvents TSP_Label_n_children As System.Windows.Forms.Label
    Private WithEvents TSP_Numeric_n_parents As System.Windows.Forms.NumericUpDown
    Private WithEvents TSP_Label_n_parents As System.Windows.Forms.Label
    Private WithEvents TSP_Numeric_n_cities As System.Windows.Forms.NumericUpDown
    Private WithEvents TSP_ComboBox_Mutationoperator As System.Windows.Forms.ComboBox
    Private WithEvents TSP_Label_Mutationoperator As System.Windows.Forms.Label
    Private WithEvents TSP_Label_Reproductionoperator As System.Windows.Forms.Label
    Private WithEvents TSP_ComboBox_Reproductionoperator As System.Windows.Forms.ComboBox
    Private WithEvents TSP_ComboBox_prob_instance As System.Windows.Forms.ComboBox
    Private WithEvents TSP_Label_Instance As System.Windows.Forms.Label
    Private WithEvents BindingSource_HookeJeeves As System.Windows.Forms.BindingSource
    Private WithEvents BindingSource_General As System.Windows.Forms.BindingSource
    Private WithEvents BindingSource_PES As System.Windows.Forms.BindingSource
    Private WithEvents BindingSource_PES_PopPenaltyOptions As System.Windows.Forms.BindingSource
    Private WithEvents BindingSource_PES_SekPop As System.Windows.Forms.BindingSource
    Private WithEvents BindingSource_PES_Pop As System.Windows.Forms.BindingSource
    Private WithEvents BindingSource_PES_Schrittweite As System.Windows.Forms.BindingSource
    Friend WithEvents BindingSource_MetaEvo As System.Windows.Forms.BindingSource
    Private WithEvents BindingSource_DDS As System.Windows.Forms.BindingSource
    Private WithEvents BindingSource_Sensiplot As System.Windows.Forms.BindingSource
    Friend WithEvents BindingSource_TSP As System.Windows.Forms.BindingSource
    Friend WithEvents SensiPlot_CheckBox_SaveResults As CheckBox
    Private WithEvents PES_Combo_Strategie As ComboBox
    Friend WithEvents Label_NThreads As Label
    Friend WithEvents NumericUpDown_NThreads As NumericUpDown
    Private WithEvents SensiPlot_RadioButton_ModeEvenDistribution As RadioButton
    Private WithEvents SensiPlot_RadioButton_ModeRandom As RadioButton
    Private WithEvents SensiPlot_RadioButton_ModeLatinHypercube As RadioButton
    Friend WithEvents SensiPlot_Label_NumSims As Label
End Class
