'*******************************************************************************
'*******************************************************************************
'**** Klasse EVO_Einstellungen                                              ****
'****                                                                       ****
'**** Autoren: Christoph Hübner, Felix Fröhlich, Dirk Muschalla             ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'****                                                                       ****
'**** November 2007                                                         ****
'****                                                                       ****
'**** Letzte Änderung: November 2007                                        ****
'*******************************************************************************
'*******************************************************************************

Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports IHWB.EVO.Common.Constants

Public Class EVO_Einstellungen
    Inherits System.Windows.Forms.UserControl

#Region "Eigenschaften"

    Private mSettings As EVO.Common.Settings         'Sicherung sämtlicher Einstellungen
    Private mProblem As EVO.Common.Problem           'Das Problem
    Private mMultithreadingAllowed As Boolean
    Private isInitializing As Boolean

    Public isSaved As Boolean = False                'Flag der anzeigt, ob die Einstellungen bereits gesichert wurden
    Public isLoad As Boolean = False                 'Flag der anzeigt, ob die Settings aus einer XML Datei gelesen werden

    ''' <summary>
    ''' Liste der momentan verwendeten Methoden
    ''' </summary>
    Private usedMethods As Collections.Specialized.StringCollection

#End Region

#Region "Properties"

    ''' <summary>
    ''' Multithreading erlauben/verbieten und gleichzeitig ein-/ausschalten
    ''' </summary>
    Public Property MultithreadingAllowed() As Boolean
        Get
            Return Me.mMultithreadingAllowed
        End Get
        Set(ByVal allow As Boolean)
            Me.mMultithreadingAllowed = allow
            If (allow) Then
                Me.CheckBox_useMultithreading.Enabled = True
                Me.CheckBox_useMultithreading.Checked = True
                Me.mSettings.General.useMultithreading = True
            Else
                Me.CheckBox_useMultithreading.Checked = False
                Me.CheckBox_useMultithreading.Enabled = False
                Me.mSettings.General.useMultithreading = False
            End If
        End Set
    End Property

    ''' <summary>
    ''' Liest die Settings aus dem Form ein und gibt sie zurück
    ''' </summary>
    ''' <returns>die Settings</returns>
    Public ReadOnly Property getSettings() As EVO.Common.Settings
        Get
            'Wenn Einstellungen noch nicht gespeichert, zuerst aus Form einlesen
            If (Not Me.isSaved) Then
                Call Me.readForm()
            End If
            Return Me.mSettings
        End Get
    End Property

#End Region 'Properties

#Region "Methoden"

    'Konstruktor
    '***********
    Public Sub New()

        Me.isInitializing = True

        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        Call Me.InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        'Settings instanzieren
        Me.mSettings = New Common.Settings()

        Me.usedMethods = New Collections.Specialized.StringCollection()

        Me.isInitializing = False

    End Sub

    ''' <summary>
    ''' Einstellungen zurücksetzen
    ''' </summary>
    Public Sub Reset()

        'Settings zurücksetzen
        Me.mSettings.Reset()

        'Alle TabPages entfernen
        Call Me.TabControl1.TabPages.Clear()

        'Nur "General" aktivieren
        Me.usedMethods.Clear()
        Me.usedMethods.Add("General")
        Me.mSettings.General.setStandard()

        Me.TabPage_General.Enabled = True
        Me.TabControl1.TabPages.Add(Me.TabPage_General)

    End Sub

    ''' <summary>
    ''' Deaktiviert alle angezeigten TabPages
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub freeze()
        For Each page As TabPage In Me.TabControl1.TabPages
            page.Enabled = False
        Next
    End Sub

    'Initialisierung
    '***************
    Public Sub Initialise(ByRef prob As EVO.Common.Problem)

        'Problem speichern
        Me.mProblem = prob

        'EVO_Einstellungen zurücksetzen
        Me.isSaved = False

        'Je nach Methode initialisieren
        Select Case Me.mProblem.Method

            Case METH_PES
                Call Me.Initialise_PES()

            Case METH_HOOKEJEEVES
                Call Me.Initialise_HookeJeeves()

            Case METH_DDS
                Call Me.Initialise_DDS()

            Case METH_CES, METH_HYBRID
                Call Me.Initialise_PES()
                Call Me.Initialise_CES()

            Case METH_METAEVO
                Call Me.Initialise_MetaEvo()

            Case METH_SENSIPLOT
                Call Me.Initialise_Sensiplot()

            Case METH_TSP
                Call Me.Initialise_TSP()

            Case Else
                Throw New Exception("Unbekannte Methode '" & Me.mProblem.Method & "' in EVO_Einstellungen.Initialise()")

        End Select

        'Werte ins Form schreiben
        Call Me.writeForm()

        'Alle TabPages aktivieren
        For Each page As TabPage In Me.TabControl1.TabPages
            page.Enabled = True
        Next

        'Letztes TabPage nach vorne holen
        Call Me.TabControl1.SelectTab(Me.TabControl1.TabPages.Count - 1)

    End Sub

    Private Sub Initialise_PES()

        Me.usedMethods.Add(METH_PES)

        'PES-Settings initialisieren
        Me.mSettings.PES = New Common.Settings_PES()
        Me.mSettings.PES.setStandard(Me.mProblem.Modus)

        'Tabpage anzeigen
        Me.TabControl1.TabPages.Add(Me.TabPage_PES)

        'Form initialisieren
        Me.PES_Combo_Strategie.DataSource = System.Enum.GetValues(GetType(EVO_STRATEGIE))
        Me.PES_Combo_Startparameter.DataSource = System.Enum.GetValues(GetType(EVO_STARTPARAMETER))
        Me.PES_Combo_DnMutation.DataSource = System.Enum.GetValues(GetType(EVO_DnMutation))
        Me.PES_Combo_OptEltern.DataSource = System.Enum.GetValues(GetType(EVO_ELTERN))
        Me.PES_Combo_PopEltern.DataSource = System.Enum.GetValues(GetType(EVO_POP_ELTERN))
        Me.PES_Combo_PopStrategie.DataSource = System.Enum.GetValues(GetType(EVO_STRATEGIE))

        Call PES_FillComboPopPenalty()

    End Sub

    Private Sub Initialise_CES()

        Me.usedMethods.Add(METH_CES)

        'CES-Settings initialisieren
        Me.mSettings.CES = New Common.Settings_CES()
        Me.mSettings.CES.setStandard(Me.mProblem.Method)

        'Tabpage anzeigen
        Me.TabControl1.TabPages.Add(Me.TabPage_CES)

        'Form initialisieren
        Me.CES_Combo_Reproduction.DataSource = System.Enum.GetValues(GetType(CES_REPRODOP))
        Me.CES_Combo_Mutation.DataSource = System.Enum.GetValues(GetType(CES_MUTATION))
        Me.CES_Combo_Selection.DataSource = System.Enum.GetValues(GetType(EVO_STRATEGIE))
        Me.Combo_CES_IniValues.DataSource = System.Enum.GetValues(GetType(EVO_STARTPARAMETER))
        Me.CES_Combo_HybridType.DataSource = System.Enum.GetValues(GetType(HYBRID_TYPE))
        Me.CES_Combo_MemStrategy.DataSource = System.Enum.GetValues(GetType(MEMORY_STRATEGY))

    End Sub

    Private Sub Initialise_HookeJeeves()

        Me.usedMethods.Add(METH_HOOKEJEEVES)

        'HJ-Settings initialisieren
        Me.mSettings.HookeJeeves = New Common.Settings_HookeJeeves()
        Me.mSettings.HookeJeeves.setStandard()

        'Tabpage anzeigen
        Me.TabControl1.TabPages.Add(Me.TabPage_HookeJeeves)

    End Sub

    Private Sub Initialise_MetaEvo()

        Me.usedMethods.Add(METH_METAEVO)

        'MetaEvo-Settings initialisieren
        Me.mSettings.MetaEvo = New Common.Settings_MetaEvo()
        Me.mSettings.MetaEvo.setStandard()

        'Tabpage anzeigen
        Me.TabControl1.TabPages.Add(Me.TabPage_MetaEvo)

    End Sub

    Private Sub Initialise_DDS()

        Me.usedMethods.Add(METH_DDS)

        'DDS-Settings initialisieren
        Me.mSettings.DDS = New Common.Settings_DDS()
        Me.mSettings.DDS.setStandard()

        'Tabpage anzeigen
        Me.TabControl1.TabPages.Add(Me.TabPage_DDS)

    End Sub

    Private Sub Initialise_Sensiplot()

        Me.usedMethods.Add(METH_SENSIPLOT)

        'Sensiplot-Settings initialisieren
        Me.mSettings.SensiPlot = New Common.Settings_Sensiplot()
        Me.mSettings.SensiPlot.setStandard()

        'Tabpage anzeigen
        Me.TabControl1.TabPages.Add(Me.TabPage_SensiPlot)

    End Sub

    Private Sub Initialise_TSP()

        Me.usedMethods.Add(METH_TSP)

        'TSP-Settings initialisieren
        Me.mSettings.TSP = New Common.Settings_TSP()
        Me.mSettings.TSP.setStandard()

        'Tabpage anzeigen
        Me.TabControl1.TabPages.Add(Me.TabPage_TSP)

        'Combobox init
        Me.TSP_ComboBox_prob_instance.DataSource = System.Enum.GetValues(GetType(EnProblem))
        Me.TSP_ComboBox_Reproductionoperator.DataSource = System.Enum.GetValues(GetType(EnReprodOperator))
        Me.TSP_ComboBox_Mutationoperator.DataSource = System.Enum.GetValues(GetType(EnMutOperator))

    End Sub

    ''' <summary>
    ''' Einstellungen aus Form einlesen
    ''' </summary>
    Private Sub readForm()

        For Each method As String In Me.usedMethods

            Select Case method

                Case "General"
                    With Me.mSettings.General
                        .useMultithreading = Me.CheckBox_useMultithreading.Checked
                        .drawOnlyCurrentGeneration = Me.CheckBox_drawOnlyCurrentGen.Checked
                    End With

                Case METH_PES
                    With Me.mSettings.PES

                        .Strategie = PES_Combo_Strategie.SelectedItem
                        .Startparameter = PES_Combo_Startparameter.SelectedItem
                        'Schrittweite
                        .Schrittweite.DnMutation = PES_Combo_DnMutation.SelectedItem
                        .Schrittweite.DnStart = PES_Numeric_DnStart.Value
                        .Schrittweite.isDnVektor = PES_Checkbox_isDnVektor.Checked
                        'Generationen
                        .n_Gen = PES_Numeric_AnzGen.Value
                        .n_Eltern = PES_Numeric_AnzEltern.Value
                        .n_Nachf = PES_Numeric_AnzNachf.Value
                        'SekPop
                        If (PES_Numeric_nInteract.Value <= 0) Then
                            .SekPop.is_Interact = False
                            .SekPop.n_Interact = 1
                        Else
                            .SekPop.is_Interact = True
                            .SekPop.n_Interact = PES_Numeric_nInteract.Value
                        End If
                        .SekPop.is_Begrenzung = Me.PES_CheckBox_isSekPopBegrenzung.Checked
                        .SekPop.n_MaxMembers = PES_Numeric_MaxMemberSekPop.Value
                        'Eltern
                        .OptEltern = PES_Combo_OptEltern.SelectedItem
                        .n_RekombXY = PES_Numeric_Rekombxy.Value
                        .is_DiversityTournament = PES_Checkbox_isTournamentSelection.Checked
                        'Populationen
                        .Pop.is_POPUL = PES_Checkbox_isPopul.Checked
                        .Pop.PopStrategie = PES_Combo_PopStrategie.SelectedItem
                        .Pop.PopPenalty = PES_Combo_PopPenalty.SelectedItem
                        .Pop.PopEltern = PES_Combo_PopEltern.SelectedItem
                        If (.Pop.is_POPUL) Then
                            .Pop.n_Runden = PES_Numeric_AnzRunden.Value
                            .Pop.n_Popul = PES_Numeric_AnzPop.Value
                            .Pop.n_PopEltern = PES_Numeric_AnzPopEltern.Value
                        Else
                            .Pop.n_Runden = 1
                            .Pop.n_Popul = 1
                            .Pop.n_PopEltern = 1
                        End If

                    End With

                Case METH_CES
                    With Me.mSettings.CES

                        ' = me.Combo_CES_IniValues.SelectedItem
                        .n_Generations = Me.CES_Numeric_n_Generations.Value
                        .n_Parents = Me.CES_Numeric_n_Parents.Value
                        .n_Children = Me.CES_Numeric_n_Children.Value
                        .OptStrategie = Me.CES_Combo_Selection.SelectedItem
                        .OptReprodOp = Me.CES_Combo_Reproduction.SelectedItem
                        .k_Value = Me.CES_Numeric_k_Value.Value
                        .OptMutOperator = Me.CES_Combo_Mutation.SelectedItem
                        .pr_MutRate = Me.CES_Numeric_MutRate.Value
                        .is_SecPop = Me.CES_CheckBox_UseSecPop_CES.Checked
                        .n_Interact = Me.CES_Numeric_n_exchange_SecPop.Value
                        .is_SecPopRestriction = Me.CES_CheckBox_isSecPopRestriction.Checked
                        .n_MemberSecondPop = Me.CES_Numeric_n_member_SecPop.Value

                        'HYBRID hängt von der Methode ab
                        .is_RealOpt = Me.CES_CheckBox_RealOptimisation.Checked
                        .ty_Hybrid = Me.CES_Combo_HybridType.SelectedItem
                        .Mem_Strategy = Me.CES_Combo_MemStrategy.SelectedItem
                        .n_PES_MemSize = Me.CES_Numeric_n_MemSize.Value
                        .is_PopMutStart = Me.CES_CheckBox_StartPESPop.Checked
                        .is_PES_SecPop = Me.CES_CheckBox_UseSecPop_PES.Checked
                        .n_PES_Interact = Me.CES_Numeric_NExchange_SecPop_PES.Value
                        .n_PES_MemSecPop = Me.CES_Numeric_n_member_SecPop_PES.Value

                    End With

                Case METH_HOOKEJEEVES
                    With Me.mSettings.HookeJeeves
                        .DnStart = Me.HJ_Numeric_DeltaStart.Value
                        .DnFinish = Me.HJ_Numeric_DeltaFinish.Value
                        .is_DnVektor = Me.HJ_CheckBox_DNVektor.Checked
                    End With

                Case METH_METAEVO
                    With Me.mSettings.MetaEvo
                        .Role = Me.MetaEvo_Combo_Role.SelectedItem
                        .OpMode = Me.MetaEvo_Combo_OpMode.SelectedItem
                        .NumberGenerations = Me.MetaEvo_Numeric_Numbergenerations.Value
                        .PopulationSize = Me.MetaEvo_Numeric_PopulationSize.Value
                        .NumberResults = Me.MetaEvo_Numeric_NumberResults.Value
                        .HJStepsize = Me.MetaEvo_Numeric_HJStepsize.Value
                        .MySQL_Host = Me.MetaEvo_TextBox_MySQL_Host.Text
                        .MySQL_Database = Me.MetaEvo_TextBox_MySQL_DB.Text
                        .MySQL_User = Me.MetaEvo_TextBox_MySQL_User.Text
                        .MySQL_Password = Me.MetaEvo_TextBox_MySQL_Password.Text

                    End With

                Case METH_DDS
                    With Me.mSettings.DDS
                        .maxiter = Me.DDS_Numeric_maxiter.Value
                        .r_val = Me.DDS_Numeric_r_val.Value
                        .optStartparameter = Me.DDS_CheckBox_ini.Checked
                    End With

                Case METH_SENSIPLOT
                    With Me.mSettings.SensiPlot

                        'OptParameter
                        ReDim .Selected_OptParameters(Me.SensiPlot_ListBox_OptParameter.SelectedIndices.Count - 1)
                        For i As Integer = 0 To Me.SensiPlot_ListBox_OptParameter.SelectedIndices.Count - 1
                            .Selected_OptParameters(i) = Me.SensiPlot_ListBox_OptParameter.SelectedIndices(i)
                        Next

                        'Objective
                        .Selected_Objective = Me.SensiPlot_ListBox_Objectives.SelectedIndex

                        'Modus
                        If (Me.SensiPlot_RadioButton_Discrete.Checked) Then
                            .Selected_SensiType = Common.Settings_Sensiplot.SensiType.discrete
                        Else
                            .Selected_SensiType = Common.Settings_Sensiplot.SensiType.normaldistribution
                        End If

                        'Anzahl Schritte
                        .Num_Steps = Me.SensiPlot_NumericUpDown_NumSteps.Value

                        'Wave anzeigen
                        .show_Wave = Me.SensiPlot_CheckBox_wave.Checked

                    End With

                Case METH_TSP
                    With Me.mSettings.TSP
                        .n_Cities = Me.TSP_Numeric_n_cities.Value
                        .Problem = Me.TSP_ComboBox_prob_instance.SelectedItem
                        .n_Parents = Me.TSP_Numeric_n_parents.Value
                        .n_Children = Me.TSP_Numeric_n_children.Value
                        .n_Gen = Me.TSP_Numeric_n_generations.Value
                        .ReprodOperator = Me.TSP_ComboBox_Reproductionoperator.SelectedItem
                        .MutOperator = Me.TSP_ComboBox_Mutationoperator.SelectedItem
                    End With

                Case Else
                    Throw New Exception("unbekannte Methode '" & method & "'in EVO_Einstellungen.readForm()")

            End Select
        Next

    End Sub

    ''' <summary>
    ''' Einstellungen in Form schreiben
    ''' </summary>
    Private Sub writeForm()

        For Each method As String In Me.usedMethods

            Select Case method

                Case "General"
                    'General
                    With Me.mSettings.General
                        Me.CheckBox_useMultithreading.Checked = .useMultithreading
                        Me.CheckBox_drawOnlyCurrentGen.Checked = .drawOnlyCurrentGeneration
                    End With

                Case METH_PES
                    With Me.mSettings.PES

                        Call PES_OptModus_Changed()

                        Me.PES_Combo_Strategie.SelectedItem = .Strategie
                        Me.PES_Combo_Startparameter.SelectedItem = .Startparameter
                        'Schrittweite
                        Me.PES_Numeric_DnStart.Value = .Schrittweite.DnStart
                        Me.PES_Combo_DnMutation.SelectedItem = .Schrittweite.DnMutation
                        Me.PES_Checkbox_isDnVektor.Checked = .Schrittweite.isDnVektor
                        'Generationen
                        Me.PES_Numeric_AnzGen.Value = .n_Gen
                        Me.PES_Numeric_AnzEltern.Value = .n_Eltern
                        Me.PES_Numeric_AnzNachf.Value = .n_Nachf
                        'SekPop
                        If (Me.mSettings.PES.SekPop.is_Interact) Then
                            Me.PES_Numeric_nInteract.Value = .SekPop.n_Interact
                        Else
                            Me.PES_Numeric_nInteract.Value = 0
                        End If
                        Me.PES_CheckBox_isSekPopBegrenzung.Checked = .SekPop.is_Begrenzung
                        Me.PES_Numeric_MaxMemberSekPop.Value = .SekPop.n_MaxMembers
                        'Eltern
                        Me.PES_Combo_OptEltern.SelectedItem = .OptEltern
                        Me.PES_Numeric_Rekombxy.Value = .n_RekombXY
                        Me.PES_Checkbox_isTournamentSelection.Checked = .is_DiversityTournament
                        'Populationen
                        Me.PES_Checkbox_isPopul.Checked = .Pop.is_POPUL
                        Me.PES_Combo_PopStrategie.SelectedItem = .Pop.PopStrategie
                        Me.PES_Combo_PopPenalty.SelectedItem = .Pop.PopPenalty
                        Me.PES_Combo_PopEltern.SelectedItem = .Pop.PopEltern
                        Me.PES_Numeric_AnzRunden.Value = .Pop.n_Runden
                        Me.PES_Numeric_AnzPop.Value = .Pop.n_Popul
                        Me.PES_Numeric_AnzPopEltern.Value = .Pop.n_PopEltern

                    End With

                Case METH_CES
                    With Me.mSettings.CES

                        Call CES_OptModus_Changed()

                        'me.Combo_CES_IniValues.SelectedItem = .
                        Me.CES_Numeric_n_Generations.Value = .n_Generations
                        Me.CES_Numeric_n_Parents.Value = .n_Parents
                        Me.CES_Numeric_n_Children.Value = .n_Children
                        Me.CES_Combo_Selection.SelectedItem = .OptStrategie
                        Me.CES_Combo_Reproduction.SelectedItem = .OptReprodOp
                        Me.CES_Numeric_k_Value.Value = .k_Value
                        Me.CES_Combo_Mutation.SelectedItem = .OptMutOperator
                        Me.CES_Numeric_MutRate.Value = .pr_MutRate
                        Me.CES_CheckBox_UseSecPop_CES.Checked = .is_SecPop
                        Me.CES_Numeric_n_exchange_SecPop.Value = .n_Interact
                        Me.CES_CheckBox_isSecPopRestriction.Checked = .is_SecPopRestriction
                        Me.CES_Numeric_n_member_SecPop.Value = .n_MemberSecondPop

                        'HYBRID hängt von der Methode ab
                        Me.CES_CheckBox_RealOptimisation.Checked = .is_RealOpt
                        Me.CES_Combo_HybridType.SelectedItem = .ty_Hybrid
                        Me.CES_Combo_MemStrategy.SelectedItem = .Mem_Strategy
                        Me.CES_Numeric_n_MemSize.Value = .n_PES_MemSize
                        Me.CES_CheckBox_StartPESPop.Checked = .is_PopMutStart
                        Me.CES_CheckBox_UseSecPop_PES.Checked = .is_PES_SecPop
                        Me.CES_Numeric_NExchange_SecPop_PES.Value = .n_PES_Interact
                        Me.CES_Numeric_n_member_SecPop_PES.Value = .n_PES_MemSecPop
                    End With

                Case METH_HOOKEJEEVES
                    With Me.mSettings.HookeJeeves
                        Me.HJ_Numeric_DeltaStart.Value = .DnStart
                        Me.HJ_Numeric_DeltaFinish.Value = .DnFinish
                        Me.HJ_CheckBox_DNVektor.Checked = .is_DnVektor
                    End With

                Case METH_METAEVO
                    With Me.mSettings.MetaEvo
                        Me.MetaEvo_Combo_Role.SelectedItem = .Role
                        Me.MetaEvo_Combo_OpMode.SelectedItem = .OpMode
                        Me.MetaEvo_Numeric_PopulationSize.Value = .PopulationSize
                        Me.MetaEvo_Numeric_Numbergenerations.Value = .NumberGenerations
                        Me.MetaEvo_Numeric_NumberResults.Value = .NumberResults
                        Me.MetaEvo_Numeric_HJStepsize.Value = .HJStepsize
                        Me.MetaEvo_TextBox_MySQL_Host.Text = .MySQL_Host
                        Me.MetaEvo_TextBox_MySQL_DB.Text = .MySQL_Database
                        Me.MetaEvo_TextBox_MySQL_User.Text = .MySQL_User
                        Me.MetaEvo_TextBox_MySQL_Password.Text = .MySQL_Password
                    End With

                Case METH_DDS
                    With Me.mSettings.DDS
                        Me.DDS_Numeric_maxiter.Value = .maxiter
                        Me.DDS_Numeric_r_val.Value = .r_val
                        Me.DDS_CheckBox_ini.Checked = .optStartparameter
                    End With

                Case METH_SENSIPLOT
                    With Me.mSettings.SensiPlot

                        'Listboxen zurücksetzen
                        Call Me.SensiPlot_ListBox_OptParameter.Items.Clear()
                        Call Me.SensiPlot_ListBox_Objectives.Items.Clear()

                        'Listboxen füllen
                        For Each optpara As Common.OptParameter In Me.mProblem.List_OptParameter
                            Call Me.SensiPlot_ListBox_OptParameter.Items.Add(optpara)
                        Next
                        For Each objective As Common.ObjectiveFunction In Me.mProblem.List_ObjectiveFunctions
                            Call Me.SensiPlot_ListBox_Objectives.Items.Add(objective)
                        Next

                        'Selected OptParameters
                        For Each selectedIndex As Integer In .Selected_OptParameters
                            Me.SensiPlot_ListBox_OptParameter.SetSelected(selectedIndex, True)
                        Next

                        'Selected Objective
                        Me.SensiPlot_ListBox_Objectives.SetSelected(.Selected_Objective, True)

                        'Modus
                        If (.Selected_SensiType = Common.Settings_Sensiplot.SensiType.discrete) Then
                            Me.SensiPlot_RadioButton_Discrete.Checked = True
                        Else
                            Me.SensiPlot_RadioButton_NormalDistribution.Checked = True
                        End If

                        'Anzahl Schritte
                        Me.SensiPlot_NumericUpDown_NumSteps.Value = .Num_Steps

                        'Wave anzeigen
                        Me.SensiPlot_CheckBox_wave.Checked = .show_Wave

                    End With

                Case METH_TSP
                    With Me.mSettings.TSP
                        Me.TSP_Numeric_n_cities.Value = .n_Cities
                        Me.TSP_ComboBox_prob_instance.SelectedItem = .Problem
                        Me.TSP_Numeric_n_parents.Value = .n_Parents
                        Me.TSP_Numeric_n_children.Value = .n_Children
                        Me.TSP_Numeric_n_generations.Value = .n_Gen
                        Me.TSP_ComboBox_Reproductionoperator.SelectedItem = .ReprodOperator
                        Me.TSP_ComboBox_Mutationoperator.SelectedItem = .MutOperator
                    End With

                Case Else
                    Throw New Exception("unbekannte Methode '" & method & "'in EVO_Einstellungen.writeForm()")

            End Select
        Next

        Call Application.DoEvents()

    End Sub

#Region "Schnittstelle"

    ''' <summary>
    ''' Speichern der Settings in einer XML-Datei
    ''' </summary>
    ''' <param name="filename">Pfad zur XML-Datei</param>
    Public Sub saveSettings(ByVal filename As String)

        Dim writer As StreamWriter
        Dim serializer As XmlSerializer

        'Neu einlesen
        Call Me.readForm()

        'Streamwriter öffnen
        writer = New StreamWriter(filename)

        serializer = New XmlSerializer(GetType(Common.Settings), New XmlRootAttribute("Settings"))
        serializer.Serialize(writer, Me.mSettings)

        writer.Close()

        Me.isSaved = True

    End Sub

    'Laden der Settings aus einer XML-Datei
    '**************************************
    Public Sub loadSettings(ByVal filename As String)

        Dim serializer As New XmlSerializer(GetType(Common.Settings))

        AddHandler serializer.UnknownElement, AddressOf serializerUnknownElement
        AddHandler serializer.UnknownAttribute, AddressOf serializerUnknownAttribute

        'Filestream öffnen
        Dim fs As New FileStream(filename, FileMode.Open)

        Try
            'Deserialisieren
            'TODO: XmlDeserializationEvents ms-help://MS.VSCC.v90/MS.MSDNQTR.v90.en/fxref_system.xml/html/e0657840-5678-bf57-6e7a-1bd93b2b27d1.htm
            Me.mSettings = CType(serializer.Deserialize(fs), Common.Settings)

            'Geladene Settings in Form schreiben
            isLoad = True
            Call Me.writeForm()
            isLoad = False

        Catch e As Exception
            MsgBox("Fehler beim Einlesen der Einstellungen!" & eol & e.Message, MsgBoxStyle.Exclamation)

        Finally
            fs.Close()

        End Try

    End Sub

    'Fehlerbehandlung Serialisierung
    '*******************************
    Private Sub serializerUnknownElement(ByVal sender As Object, ByVal e As XmlElementEventArgs)
        MsgBox("Fehler beim Einlesen der Einstellungen:" & eol _
            & "Das Element '" & e.Element.Name & "' ist unbekannt!", MsgBoxStyle.Exclamation)
    End Sub

    Private Sub serializerUnknownAttribute(ByVal sender As Object, ByVal e As XmlAttributeEventArgs)
        MsgBox("Fehler beim Einlesen der Einstellungen:" & eol _
            & "Das Attribut '" & e.Attr.Name & "' ist unbekannt!", MsgBoxStyle.Exclamation)
    End Sub

#End Region 'Schnittstelle

#Region "PES"

    'PES-Optimierungsmodus wurde geändert
    '------------------------------------
    Private Sub PES_OptModus_Changed()

        Select Case Me.mSettings.PES.OptModus

            Case EVO_MODUS.Single_Objective
                'Vorgaben und Anzeige
                Me.PES_Label_OptModus.Text = "Single Objective"
                'Strategie
                Me.PES_Combo_Strategie.Enabled = True
                'Sekundäre Population
                Me.GroupBox_SekPop.Enabled = False
                'Populationen
                Me.PES_Checkbox_isPopul.Enabled = True
                'Neuen Standardwert für PopPenalty setzen
                Me.mSettings.PES.Pop.PopPenalty = EVO_POP_PENALTY.Mittelwert

            Case EVO_MODUS.Multi_Objective
                'Vorgaben und Anzeige
                Me.PES_Label_OptModus.Text = "MultiObjective Pareto"
                'Strategie
                Me.PES_Combo_Strategie.Enabled = False
                'Sekundäre Population
                Me.GroupBox_SekPop.Enabled = True
                'Populationen
                Me.PES_Checkbox_isPopul.Enabled = False
                Me.PES_Checkbox_isPopul.Checked = False
                Me.PES_GroupBox_Populationen.Enabled = False
                'Neuen Standardwert für PopPenalty setzen
                Me.mSettings.PES.Pop.PopPenalty = EVO_POP_PENALTY.Crowding

        End Select

        Call PES_FillComboPopPenalty()

    End Sub

    Private Sub PES_Combo_OptEltern_Changed(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles PES_Combo_OptEltern.SelectedIndexChanged

        Select Case CType(PES_Combo_OptEltern.SelectedItem, EVO_ELTERN)
            Case EVO_ELTERN.XY_Diskret, EVO_ELTERN.XY_Mitteln, EVO_ELTERN.Neighbourhood, EVO_ELTERN.XY_Mitteln_Diskret
                LabelRekombxy1.Enabled = True
                LabelRekombxy3.Enabled = True
                PES_Numeric_Rekombxy.Enabled = True
                PES_Checkbox_isTournamentSelection.Enabled = True
                PES_Checkbox_isTournamentSelection.Checked = True
            Case Else
                LabelRekombxy1.Enabled = False
                LabelRekombxy3.Enabled = False
                PES_Numeric_Rekombxy.Enabled = False
                PES_Checkbox_isTournamentSelection.Enabled = False
                PES_Checkbox_isTournamentSelection.Checked = False
        End Select

    End Sub

    Private Sub PES_SekPopBegrenzung_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PES_CheckBox_isSekPopBegrenzung.CheckedChanged

        If (Me.PES_CheckBox_isSekPopBegrenzung.Checked) Then
            Me.LabelMaxMemberSekPop.Enabled = True
            Me.PES_Numeric_MaxMemberSekPop.Enabled = True
        Else
            Me.LabelMaxMemberSekPop.Enabled = False
            Me.PES_Numeric_MaxMemberSekPop.Enabled = False
        End If

    End Sub

    Private Sub PES_isPopul_Changed(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles PES_Checkbox_isPopul.CheckStateChanged

        If (PES_Checkbox_isPopul.Checked) Then
            PES_GroupBox_Populationen.Enabled = True
        Else
            PES_GroupBox_Populationen.Enabled = False
        End If

    End Sub

    Private Sub PES_FillComboPopPenalty()

        Me.PES_Combo_PopPenalty.Items.Clear()

        Select Case Me.mSettings.PES.OptModus

            Case EVO_MODUS.Single_Objective
                Me.PES_Combo_PopPenalty.Items.Add(EVO_POP_PENALTY.Mittelwert)
                Me.PES_Combo_PopPenalty.Items.Add(EVO_POP_PENALTY.Schlechtester)

            Case EVO_MODUS.Multi_Objective
                'BUG 264: Popgüte bei MultiObjective überflüssig?
                Me.PES_Combo_PopPenalty.Items.Add(EVO_POP_PENALTY.Crowding)
                Me.PES_Combo_PopPenalty.Items.Add(EVO_POP_PENALTY.Spannweite)
        End Select

        Me.PES_Combo_PopPenalty.SelectedIndex = 0

    End Sub

#End Region 'PES

#Region "CES"

    Private Sub CES_OptModus_Changed()

        Select Case Me.mProblem.Method

            Case METH_CES
                CES_GroupBox_Hybrid.Enabled = False

            Case METH_HYBRID
                CES_GroupBox_Hybrid.Enabled = True

                Call CES_HybridType_Changed(New Object(), System.EventArgs.Empty)

        End Select

    End Sub

    'Settings für TestModus
    '**********************
    Public Sub CES_setTestModus(ByVal Modus As CES_T_MODUS, ByVal Path() As Integer, ByVal nGen As Integer, ByVal nParents As Integer, ByVal NChildren As Integer)

        Dim i As Integer
        Dim PathStr As String

        If NChildren = 1 Then
            PathStr = "   Path: "
            For i = 0 To Path.GetUpperBound(0)
                PathStr = PathStr & Path(i) & " "
            Next
            PathStr = PathStr.TrimEnd
        Else
            PathStr = "   n_combi: "
            PathStr = PathStr & NChildren
        End If

        Me.CES_Label_OptModus.Text = "Modus: " & Modus.ToString & PathStr
        Me.CES_Numeric_n_Generations.Value = nGen
        Me.CES_Numeric_n_Parents.Minimum = 1
        Me.CES_Numeric_n_Parents.Value = nParents
        Me.CES_Numeric_n_Children.Minimum = 1
        Me.CES_Numeric_n_Children.Value = NChildren

    End Sub

    'Setzt/Aktiviert/Deaktiviert die Einstellungen auf den PES Settings
    '******************************************************************
    Private Sub CES_HybridType_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CES_Combo_HybridType.SelectedIndexChanged

        If (Me.isInitializing) Then Exit Sub

        If (Me.mProblem.Method = METH_HYBRID And Not isLoad) Then

            Dim Item As HYBRID_TYPE
            Item = Me.CES_Combo_HybridType.SelectedItem

            Select Case Item

                Case HYBRID_TYPE.Mixed_Integer

                    'Generationen
                    Me.PES_Numeric_AnzGen.Text = 1
                    Me.PES_Numeric_AnzGen.Enabled = False

                    'Eltern
                    Me.PES_Numeric_AnzEltern.Text = 5
                    Me.PES_Numeric_AnzEltern.Enabled = True
                    Me.LabelAnzEltern.Text = "Maximal Zahl der Eltern:"

                    'Children
                    Me.PES_Numeric_AnzNachf.Text = 1
                    Me.PES_Numeric_AnzNachf.Enabled = False

                Case HYBRID_TYPE.Sequencial_1

                    'Generationen
                    Me.PES_Numeric_AnzGen.Text = 10
                    Me.PES_Numeric_AnzGen.Enabled = True

                    'Eltern
                    Me.PES_Numeric_AnzEltern.Text = 5
                    Me.PES_Numeric_AnzEltern.Enabled = True
                    Me.LabelAnzEltern.Text = "Anzahl der Eltern:"

                    'Children
                    Me.PES_Numeric_AnzNachf.Text = 15
                    Me.PES_Numeric_AnzNachf.Enabled = True

            End Select

        End If

    End Sub

    Private Sub CES_Combo_Reproduction_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CES_Combo_Reproduction.SelectedIndexChanged

        If Me.CES_Combo_Reproduction.SelectedItem = EVO.Common.CES_REPRODOP.k_Point_Crossover Then
            Me.CES_Numeric_k_Value.Enabled = True
            Me.CES_Numeric_k_Value.Value = 3
        Else
            Me.CES_Numeric_k_Value.Enabled = False
            Me.CES_Numeric_k_Value.Value = 0
        End If
    End Sub

    Private Sub CES_RealOptimisation_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CES_CheckBox_RealOptimisation.CheckedChanged

        If (CES_CheckBox_RealOptimisation.Checked) Then
            Me.CES_GroupBox_Hybrid.Enabled = True
        Else
            Me.CES_GroupBox_Hybrid.Enabled = False
        End If

    End Sub

    Private Sub CES_UseSecPop_CES_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CES_CheckBox_UseSecPop_CES.CheckedChanged

        If (Me.CES_CheckBox_UseSecPop_CES.Checked) Then
            Me.GroupBox_CES_SecPop.Enabled = True
        Else
            Me.GroupBox_CES_SecPop.Enabled = False
        End If

    End Sub

    Private Sub CES_isMaxMemberSekPop_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CES_CheckBox_isSecPopRestriction.CheckedChanged

        If (Me.CES_CheckBox_isSecPopRestriction.Checked) Then
            Me.Label_CES_NMembersSecPop.Enabled = True
            Me.CES_Numeric_n_member_SecPop.Enabled = True
        Else
            Me.Label_CES_NMembersSecPop.Enabled = False
            Me.CES_Numeric_n_member_SecPop.Enabled = False
        End If

    End Sub

#End Region 'CES

#Region "MetaEvo"

    'MetaEvo: Voreinstellungen des Formulars aufgrund der Wahl der Rolle
    Private Sub MetaEvo_Role_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MetaEvo_Combo_Role.SelectedIndexChanged

        If (Me.MetaEvo_Combo_Role.SelectedItem = "Single PC") Then
            Me.MetaEvo_Combo_OpMode.Enabled = True
            MetaEvo_OpMode_Changed(sender, e)
            Me.GroupBox_MetaEvo_MySQLOptions.Enabled = False
        ElseIf (Me.MetaEvo_Combo_Role.SelectedItem = "Network Client") Then
            Me.MetaEvo_Combo_OpMode.Enabled = False
            Me.GroupBox_MetaEvo_BasicOptions.Enabled = False
            Me.GroupBox_MetaEvo_TransferOptions.Enabled = False
            Me.GroupBox_MetaEvo_LocalOptions.Enabled = False
            Me.GroupBox_MetaEvo_MySQLOptions.Enabled = True
        ElseIf (Me.MetaEvo_Combo_Role.SelectedItem = "Network Server") Then
            Me.MetaEvo_Combo_OpMode.Enabled = True
            MetaEvo_OpMode_Changed(sender, e)
            Me.GroupBox_MetaEvo_MySQLOptions.Enabled = True
        End If
    End Sub

    Private Sub MetaEvo_OpMode_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MetaEvo_Combo_OpMode.SelectedIndexChanged

        If (Me.MetaEvo_Combo_OpMode.SelectedItem = "Both") Then
            Me.GroupBox_MetaEvo_BasicOptions.Enabled = True
            Me.GroupBox_MetaEvo_TransferOptions.Enabled = True
            Me.GroupBox_MetaEvo_LocalOptions.Enabled = True
        ElseIf (Me.MetaEvo_Combo_OpMode.SelectedItem = "Global Optimizer") Then
            Me.GroupBox_MetaEvo_BasicOptions.Enabled = True
            Me.GroupBox_MetaEvo_TransferOptions.Enabled = True
            Me.GroupBox_MetaEvo_LocalOptions.Enabled = False
        ElseIf (Me.MetaEvo_Combo_OpMode.SelectedItem = "Local Optimizer") Then
            Me.GroupBox_MetaEvo_BasicOptions.Enabled = False
            Me.GroupBox_MetaEvo_TransferOptions.Enabled = True
            Me.GroupBox_MetaEvo_LocalOptions.Enabled = True
        End If
    End Sub

#End Region 'MetaEvo

#Region "Sensiplot"

    Private Sub SensiPlot_CheckForm() Handles SensiPlot_ListBox_OptParameter.SelectedIndexChanged, SensiPlot_RadioButton_Discrete.CheckedChanged

        If (Me.isInitializing) Then Exit Sub

        'Entweder 1 oder 2 OptParameter
        If (Me.SensiPlot_ListBox_OptParameter.SelectedIndices.Count = 0 _
            Or Me.SensiPlot_ListBox_OptParameter.SelectedIndices.Count > 2) Then
            MsgBox("Bitte zwischen 1 und 2 OptParameter auswählen!", MsgBoxStyle.Exclamation, "SensiPlot")
            'Auswahl zurücksetzen
            Me.isInitializing = True
            For i As Integer = 0 To Me.SensiPlot_ListBox_OptParameter.Items.Count - 1
                Call Me.SensiPlot_ListBox_OptParameter.SetSelected(i, False)
            Next
            Me.SensiPlot_ListBox_OptParameter.SetSelected(0, True)
            Me.isInitializing = False
            Exit Sub
        End If

        'bei 2 OptParametern geht nur diskret!
        If (Me.SensiPlot_ListBox_OptParameter.SelectedIndices.Count = 2 And _
            Me.SensiPlot_RadioButton_NormalDistribution.Checked) Then
            MsgBox("Bei mehr als einem OptParameter muss 'Diskret' als Modus ausgewählt sein!", MsgBoxStyle.Exclamation, "SensiPlot")
            Me.SensiPlot_RadioButton_Discrete.Checked = True
            Exit Sub
        End If

    End Sub

#End Region 'Sensiplot

#End Region 'Methoden

End Class
