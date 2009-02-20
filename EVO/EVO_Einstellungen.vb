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

    Private msettings As EVO.Common.EVO_Settings     'Sicherung sämtlicher Einstellungen
    Private mProblem As EVO.Common.Problem           'Das Problem
    Public isSaved As Boolean = False                'Flag der anzeigt, ob die Einstellungen bereits gesichert wurden
    Public isLoad As Boolean = False                 'Flag der anzeigt, ob die Settings aus einer XML Datei gelesen werden
    Private isInitializing As Boolean

#End Region

#Region "Properties"

    'EVO_Settings Property
    '*********************
    Public ReadOnly Property Settings() As EVO.Common.EVO_Settings
        Get
            'Wenn Einstellungen noch nicht gespeichert, zuerst aus Form einlesen
            If (Not Me.isSaved) Then
                Call Me.readForm()
            End If
            Return Me.msettings
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
        Me.msettings = New Common.EVO_Settings()

        'Comboboxen füllen
        Call Me.InitComboboxes()

        Call Me.setStandard_All()

        Me.isInitializing = False

    End Sub

    ''' <summary>
    ''' Alle Standard-Settings setzen
    ''' </summary>
    Public Sub setStandard_All()

        Me.isInitializing = True
        Call Me.msettings.PES.setStandard(EVO_MODUS.Single_Objective)
        Call Me.msettings.CES.setStandard(METH_CES)
        Call Me.msettings.HookJeeves.setStandard()
        Call Me.msettings.MetaEvo.setStandard()
        Call Me.msettings.DDS.setStandard()
        Call Me.msettings.SensiPlot.setStandard()
        'Call Me.writeForm()
        Me.isInitializing = False

    End Sub


    'Initialisierung
    '***************
    Public Sub Initialise(ByRef prob As EVO.Common.Problem)

        'Problem speichern
        Me.mProblem = prob

        'EVO_Einstellungen zurücksetzen
        Me.isSaved = False

        'Zunächst alle TabPages entfernen, 
        'dann je nach Bedarf wieder hinzufügen
        Call Me.TabControl1.TabPages.Clear()

        'Anzeige je nach Methode anpassen
        Select Case Me.mProblem.Method

            Case METH_PES

                'EVO_Einstellungen aktivieren
                Me.Enabled = True

                'Tabpage anzeigen
                Me.TabControl1.TabPages.Add(Me.TabPage_PES)

                'Standardeinstellungen setzen
                Call Me.setStandard_PES(Me.mProblem.Modus)

            Case METH_HOOKJEEVES

                'EVO_Einstellungen aktivieren
                Me.Enabled = True

                'Tabpage anzeigen
                Me.TabControl1.TabPages.Add(Me.TabPage_HookeJeeves)

                'Standardeinstellungen setzen
                Call Me.setStandard_HJ()

            Case METH_DDS

                'EVO_Einstellungen aktivieren
                Me.Enabled = True

                'Tabpage anzeigen
                Me.TabControl1.TabPages.Add(Me.TabPage_DDS)

                'Standardeinstellungen setzen
                Call Me.setStandard_DDS()

            Case METH_CES

                'EVO_Einstellungen aktivieren
                Me.Enabled = True

                'Tabpage anzeigen
                Me.TabControl1.TabPages.Add(Me.TabPage_CES)

                'Standardeinstellungen setzen
                Call Me.setStandard_CES()

            Case METH_HYBRID

                'EVO_Einstellungen aktivieren
                Me.Enabled = True

                'Tabpage anzeigen
                Me.TabControl1.TabPages.Add(Me.TabPage_PES)
                Me.TabControl1.TabPages.Add(Me.TabPage_CES)

                'Standardeinstellungen setzen
                Call Me.setStandard_CES()
                Call Me.setStandard_PES(Me.mProblem.Modus)

            Case METH_MetaEvo

                'EVO_Einstellungen aktivieren
                Me.Enabled = True

                'Tabpage anzeigen
                Me.TabControl1.TabPages.Add(Me.TabPage_MetaEvo)

                'Standardeinstellungen setzen
                Call Me.setStandard_MetaEvo()

            Case METH_SENSIPLOT

                'EVO_Einstellungen aktivieren
                Me.Enabled = True

                'Tabpage anzeigen
                Me.TabControl1.TabPages.Add(Me.TabPage_SensiPlot)

                'Standardeinstellungen setzen
                Call Me.setStandard_SensiPlot()

            Case Else

                Me.Enabled = False

        End Select

    End Sub

    'Optimierungsmodus wurde geändert
    '********************************

    'PES
    '---
    Private Sub OptModus_Change_ActDeact_PES()

        Select Case Me.msettings.PES.OptModus

            Case EVO_MODUS.Single_Objective
                'Vorgaben und Anzeige
                Label_OptModusValue.Text = "Single Objective"
                'Strategie
                ComboOptStrategie.Enabled = True
                'Sekundäre Population
                Me.GroupBox_SekPop.Enabled = False
                'Populationen
                CheckisPopul.Enabled = True
                'Neuen Standardwert für PopPenalty setzen
                Me.msettings.PES.Pop.OptPopPenalty = EVO_POP_PENALTY.Mittelwert

            Case EVO_MODUS.Multi_Objective
                'Vorgaben und Anzeige
                Label_OptModusValue.Text = "MultiObjective Pareto"
                'Strategie
                ComboOptStrategie.Enabled = False
                'Sekundäre Population
                Me.GroupBox_SekPop.Enabled = True
                'Populationen
                CheckisPopul.Enabled = False
                CheckisPopul.Checked = False
                GroupBox_Populationen.Enabled = False
                'Neuen Standardwert für PopPenalty setzen
                Me.msettings.PES.Pop.OptPopPenalty = EVO_POP_PENALTY.Crowding

        End Select

        Call FILLCOMBO_POPPENALTY(ComboOptPopPenalty)

    End Sub

    'CES
    '---
    Private Sub OptModus_Change_ActDeact_CES()

        Select Case Me.mProblem.Method

            Case METH_CES
                GroupBox_CES_Hybrid.Enabled = False

            Case METH_HYBRID
                GroupBox_CES_Hybrid.Enabled = True

                Call Combo_CES_HybridType_SelectedIndexChanged(New Object(), System.EventArgs.Empty)

        End Select

    End Sub

    Private Sub ComboOptEltern_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboOptEltern.SelectedIndexChanged

        Select Case CType(ComboOptEltern.SelectedItem, EVO_ELTERN)
            Case EVO_ELTERN.XY_Diskret, EVO_ELTERN.XY_Mitteln, EVO_ELTERN.Neighbourhood, EVO_ELTERN.XY_Mitteln_Diskret
                LabelRekombxy1.Enabled = True
                LabelRekombxy3.Enabled = True
                TextRekombxy.Enabled = True
                CheckisTournamentSelection.Enabled = True
                CheckisTournamentSelection.Checked = True
            Case Else
                LabelRekombxy1.Enabled = False
                LabelRekombxy3.Enabled = False
                TextRekombxy.Enabled = False
                CheckisTournamentSelection.Enabled = False
                CheckisTournamentSelection.Checked = False
        End Select

    End Sub

    Private Sub CheckBox_SekPopBegrenzung_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_isSekPopBegrenzung.CheckedChanged

        If (Me.CheckBox_isSekPopBegrenzung.Checked) Then
            Me.LabelMaxMemberSekPop.Enabled = True
            Me.TextMaxMemberSekPop.Enabled = True
        Else
            Me.LabelMaxMemberSekPop.Enabled = False
            Me.TextMaxMemberSekPop.Enabled = False
        End If

    End Sub

    Private Sub CheckBox_CES_UseSecPop_CES_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_CES_UseSecPop_CES.CheckedChanged

        If (Me.CheckBox_CES_UseSecPop_CES.Checked) Then
            Me.GroupBox_CES_SecPop.Enabled = True
        Else
            Me.GroupBox_CES_SecPop.Enabled = False
        End If

    End Sub

    Private Sub CheckBox_CES_isMaxMemberSekPop_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_CES_isSecPopRestriction.CheckedChanged

        If (Me.CheckBox_CES_isSecPopRestriction.Checked) Then
            Me.Label_CES_NMembersSecPop.Enabled = True
            Me.Numeric_CES_n_member_SecPop.Enabled = True
        Else
            Me.Label_CES_NMembersSecPop.Enabled = False
            Me.Numeric_CES_n_member_SecPop.Enabled = False
        End If

    End Sub

    Private Sub CheckisPopul_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CheckisPopul.CheckStateChanged

        If (CheckisPopul.Checked) Then
            GroupBox_Populationen.Enabled = True
        Else
            GroupBox_Populationen.Enabled = False
        End If

    End Sub

    Private Sub CheckBox_CES_RealOptimisation_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_CES_RealOptimisation.CheckedChanged

        If (CheckBox_CES_RealOptimisation.Checked) Then
            Me.GroupBox_CES_Hybrid.Enabled = True
        Else
            Me.GroupBox_CES_Hybrid.Enabled = False
        End If

    End Sub

    'Comboboxen füllen
    '*****************
    Private Sub InitComboboxes()

        'PES
        '---
        Me.ComboOptStrategie.DataSource = System.Enum.GetValues(GetType(EVO_STRATEGIE))
        Me.ComboOptStartparameter.DataSource = System.Enum.GetValues(GetType(EVO_STARTPARAMETER))
        Me.ComboOptDnMutation.DataSource = System.Enum.GetValues(GetType(EVO_DnMutation))
        Me.ComboOptEltern.DataSource = System.Enum.GetValues(GetType(EVO_ELTERN))
        Me.ComboOptPopEltern.DataSource = System.Enum.GetValues(GetType(EVO_POP_ELTERN))
        Me.ComboOptPopStrategie.DataSource = System.Enum.GetValues(GetType(EVO_STRATEGIE))
        Call FILLCOMBO_POPPENALTY(ComboOptPopPenalty)

        'CES
        '---
        Me.Combo_CES_Reproduction.DataSource = System.Enum.GetValues(GetType(CES_REPRODOP))
        Me.Combo_CES_Mutation.DataSource = System.Enum.GetValues(GetType(CES_MUTATION))
        Me.Combo_CES_Selection.DataSource = System.Enum.GetValues(GetType(EVO_STRATEGIE))
        Me.Combo_CES_IniValues.DataSource = System.Enum.GetValues(GetType(EVO_STARTPARAMETER))
        Me.Combo_CES_HybridType.DataSource = System.Enum.GetValues(GetType(HYBRID_TYPE))
        Me.Combo_CES_MemStrategy.DataSource = System.Enum.GetValues(GetType(MEMORY_STRATEGY))

    End Sub

    Private Sub FILLCOMBO_POPPENALTY(ByRef Cntrl As System.Windows.Forms.ComboBox)

        Cntrl.Items.Clear()
        Select Case Me.msettings.PES.OptModus

            Case EVO_MODUS.Single_Objective
                Cntrl.Items.Add(EVO_POP_PENALTY.Mittelwert)
                Cntrl.Items.Add(EVO_POP_PENALTY.Schlechtester)

            Case EVO_MODUS.Multi_Objective
                'BUG 264: Popgüte bei MultiObjective überflüssig?
                Cntrl.Items.Add(EVO_POP_PENALTY.Crowding)
                Cntrl.Items.Add(EVO_POP_PENALTY.Spannweite)
        End Select

        Cntrl.SelectedIndex = 0

    End Sub

    'Einstellungen aus Form einlesen
    '*******************************
    Private Sub readForm()

        'PES
        '---
        With Me.msettings.PES

            .OptStrategie = ComboOptStrategie.SelectedItem
            .OptStartparameter = ComboOptStartparameter.SelectedItem
            'Schrittweite
            .Schrittweite.OptDnMutation = ComboOptDnMutation.SelectedItem
            .Schrittweite.DnStart = TextDeltaStart.Value
            .Schrittweite.is_DnVektor = CheckisDnVektor.Checked
            'Generationen
            .n_Gen = TextAnzGen.Value
            .n_Eltern = TextAnzEltern.Value
            .n_Nachf = TextAnzNachf.Value
            'SekPop
            If (TextInteract.Value <= 0) Then
                .SekPop.is_Interact = False
                .SekPop.n_Interact = 1
            Else
                .SekPop.is_Interact = True
                .SekPop.n_Interact = TextInteract.Value
            End If
            .SekPop.is_Begrenzung = Me.CheckBox_isSekPopBegrenzung.Checked
            .SekPop.n_MaxMembers = TextMaxMemberSekPop.Value
            'Eltern
            .OptEltern = ComboOptEltern.SelectedItem
            .n_RekombXY = TextRekombxy.Value
            .is_DiversityTournament = CheckisTournamentSelection.Checked
            'Populationen
            .Pop.is_POPUL = CheckisPopul.Checked
            .Pop.OptPopStrategie = ComboOptPopStrategie.SelectedItem
            .Pop.OptPopPenalty = ComboOptPopPenalty.SelectedItem
            .Pop.OptPopEltern = ComboOptPopEltern.SelectedItem
            If (.Pop.is_POPUL) Then
                .Pop.n_Runden = TextAnzRunden.Value
                .Pop.n_Popul = TextAnzPop.Value
                .Pop.n_PopEltern = TextAnzPopEltern.Value
            Else
                .Pop.n_Runden = 1
                .Pop.n_Popul = 1
                .Pop.n_PopEltern = 1
            End If

        End With

        'CES
        '---
        With Me.msettings.CES

            ' = me.Combo_CES_IniValues.SelectedItem
            .n_Generations = Me.Numeric_CES_n_Generations.Value
            .n_Parents = Me.Numeric_CES_n_Parents.Value
            .n_Childs = Me.Numeric_CES_n_childs.Value
            .OptStrategie = Me.Combo_CES_Selection.SelectedItem
            .OptReprodOp = Me.Combo_CES_Reproduction.SelectedItem
            .OptMutOperator = Me.Combo_CES_Mutation.SelectedItem
            .pr_MutRate = Me.Numeric_CES_MutRate.Value
            .is_SecPop = Me.CheckBox_CES_UseSecPop_CES.Checked
            .n_Interact = Me.Numeric_CES_n_exchange_SecPop.Value
            .is_SecPopRestriction = Me.CheckBox_CES_isSecPopRestriction.Checked
            .n_MemberSecondPop = Me.Numeric_CES_n_member_SecPop.Value

            'HYBRID hängt von der Methode ab
            .is_RealOpt = Me.CheckBox_CES_RealOptimisation.Checked
            .ty_Hybrid = Me.Combo_CES_HybridType.SelectedItem
            .Mem_Strategy = Me.Combo_CES_MemStrategy.SelectedItem
            .n_PES_MemSize = Me.Numeric_CES_n_MemSize.Value
            .is_PopMutStart = Me.CheckBox_CES_StartPESPop.Checked
            .is_PES_SecPop = Me.CheckBox_CES_UseSecPop_PES.Checked
            .n_PES_Interact = Me.Numeric_CES_NExchange_SecPop_PES.Value
            .n_PES_MemSecPop = Me.Numeric_CES_n_member_SecPop_PES.Value

        End With

        'Hooke and Jeeves
        '----------------
        With Me.msettings.HookJeeves

            .DnStart = Me.Numeric_HJ_DeltaStart.Value
            .DnFinish = Me.Numeric_HJ_DeltaFinish.Value
            .is_DnVektor = Me.CheckBox_HJ_DNVektor.Checked

        End With

        'MetaEvo
        '----------------
        With Me.msettings.MetaEvo

            .Role = Me.Combo_MetaEvo_Role.SelectedItem
            .OpMode = Me.Combo_MetaEvo_OpMode.SelectedItem
            .NumberGenerations = Me.Numeric_MetaEvo_Numbergenerations.Value
            .PopulationSize = Me.Numeric_MetaEvo_PopulationSize.Value
            .Numberresults = Me.Numeric_MetaEvo_NumberResults.Value
            .HJStepsize = Me.Numeric_MetaEvo_HJStepsize.Value
            .MySQL_Host = Me.TextBox_MetaEvo_MySQL_Host.Text
            .MySQL_Database = Me.TextBox_MetaEvo_MySQL_DB.Text
            .MySQL_User = Me.TextBox_MetaEvo_MySQL_User.Text
            .MySQL_Password = Me.TextBox_MetaEvo_MySQL_Password.Text

        End With

        'DDS
        '-----------------
        With Me.msettings.DDS

            .maxiter = Me.Numeric_DDS_maxiter.Value
            .r_val = Me.Numeric_DDS_r_val.Value
            .optStartparameter = Me.CheckBox_DDS_ini.Checked

        End With

        'SensiPlot
        '--------------
        With Me.msettings.SensiPlot

            'OptParameter
            ReDim .Selected_OptParameters(Me.SensiPlot_ListBox_OptParameter.SelectedIndices.Count - 1)
            For i As Integer = 0 To Me.SensiPlot_ListBox_OptParameter.SelectedIndices.Count - 1
                .Selected_OptParameters(i) = Me.SensiPlot_ListBox_OptParameter.SelectedIndices(i)
            Next

            'Objective
            .Selected_Objective = Me.SensiPlot_ListBox_Objectives.SelectedIndex

            'Modus
            If (Me.SensiPlot_RadioButton_Discrete.Checked) Then
                .Selected_SensiType = Common.EVO_Settings.SensiPlot_Settings.SensiType.discrete
            Else
                .Selected_SensiType = Common.EVO_Settings.SensiPlot_Settings.SensiType.normaldistribution
            End If

            'Anzahl Schritte
            .Num_Steps = Me.SensiPlot_NumericUpDown_NumSteps.Value

            'Wave anzeigen
            .show_Wave = Me.SensiPlot_CheckBox_wave.Checked

        End With

    End Sub

    'Setzt/Aktiviert/Deaktiviert die Einstellungen auf den PES Settings
    '******************************************************************
    Private Sub Combo_CES_HybridType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Combo_CES_HybridType.SelectedIndexChanged

        If (Me.isInitializing) Then Exit Sub

        If (Me.mProblem.Method = METH_HYBRID And Not isLoad) Then

            Dim Item As HYBRID_TYPE
            Item = Me.Combo_CES_HybridType.SelectedItem

            Select Case Item

                Case HYBRID_TYPE.Mixed_Integer

                    'Generationen
                    Me.TextAnzGen.Text = 1
                    Me.TextAnzGen.Enabled = False

                    'Eltern
                    Me.TextAnzEltern.Text = 5
                    Me.TextAnzEltern.Enabled = True
                    Me.LabelAnzEltern.Text = "Maximal Zahl der Eltern:"

                    'Childs
                    Me.TextAnzNachf.Text = 1
                    Me.TextAnzNachf.Enabled = False

                Case HYBRID_TYPE.Sequencial_1

                    'Generationen
                    Me.TextAnzGen.Text = 10
                    Me.TextAnzGen.Enabled = True

                    'Eltern
                    Me.TextAnzEltern.Text = 5
                    Me.TextAnzEltern.Enabled = True
                    Me.LabelAnzEltern.Text = "Anzahl der Eltern:"

                    'Childs
                    Me.TextAnzNachf.Text = 15
                    Me.TextAnzNachf.Enabled = True

            End Select

        End If

    End Sub


    'Einstellungen in Form schreiben
    '*******************************
    Private Sub writeForm()

        'PES
        '---
        With Me.msettings.PES

            Call OptModus_Change_ActDeact_PES()

            Me.ComboOptStrategie.SelectedItem = .OptStrategie
            Me.ComboOptStartparameter.SelectedItem = .OptStartparameter
            'Schrittweite
            Me.TextDeltaStart.Value = .Schrittweite.DnStart
            Me.ComboOptDnMutation.SelectedItem = .Schrittweite.OptDnMutation
            Me.CheckisDnVektor.Checked = .Schrittweite.is_DnVektor
            'Generationen
            Me.TextAnzGen.Value = .n_Gen
            Me.TextAnzEltern.Value = .n_Eltern
            Me.TextAnzNachf.Value = .n_Nachf
            'SekPop
            If (Me.msettings.PES.SekPop.is_Interact) Then
                Me.TextInteract.Value = .SekPop.n_Interact
            Else
                Me.TextInteract.Value = 0
            End If
            Me.CheckBox_isSekPopBegrenzung.Checked = .SekPop.is_Begrenzung
            Me.TextMaxMemberSekPop.Value = .SekPop.n_MaxMembers
            'Eltern
            Me.ComboOptEltern.SelectedItem = .OptEltern
            Me.TextRekombxy.Value = .n_RekombXY
            Me.CheckisTournamentSelection.Checked = .is_DiversityTournament
            'Populationen
            Me.CheckisPopul.Checked = .Pop.is_POPUL
            Me.ComboOptPopStrategie.SelectedItem = .Pop.OptPopStrategie
            Me.ComboOptPopPenalty.SelectedItem = .Pop.OptPopPenalty
            Me.ComboOptPopEltern.SelectedItem = .Pop.OptPopEltern
            Me.TextAnzRunden.Value = .Pop.n_Runden
            Me.TextAnzPop.Value = .Pop.n_Popul
            Me.TextAnzPopEltern.Value = .Pop.n_PopEltern

        End With

        'PES
        '---
        With Me.msettings.CES

            Call OptModus_Change_ActDeact_CES()

            'me.Combo_CES_IniValues.SelectedItem = .
            Me.Numeric_CES_n_Generations.Value = .n_Generations
            Me.Numeric_CES_n_Parents.Value = .n_Parents
            Me.Numeric_CES_n_childs.Value = .n_Childs
            Me.Combo_CES_Selection.SelectedItem = .OptStrategie
            Me.Combo_CES_Reproduction.SelectedItem = .OptReprodOp
            Me.Combo_CES_Mutation.SelectedItem = .OptMutOperator
            Me.Numeric_CES_MutRate.Value = .pr_MutRate
            Me.CheckBox_CES_UseSecPop_CES.Checked = .is_SecPop
            Me.Numeric_CES_n_exchange_SecPop.Value = .n_Interact
            Me.CheckBox_CES_isSecPopRestriction.Checked = .is_SecPopRestriction
            Me.Numeric_CES_n_member_SecPop.Value = .n_MemberSecondPop

            'HYBRID hängt von der Methode ab
            Me.CheckBox_CES_RealOptimisation.Checked = .is_RealOpt
            Me.Combo_CES_HybridType.SelectedItem = .ty_Hybrid
            Me.Combo_CES_MemStrategy.SelectedItem = .Mem_Strategy
            Me.Numeric_CES_n_MemSize.Value = .n_PES_MemSize
            Me.CheckBox_CES_StartPESPop.Checked = .is_PopMutStart
            Me.CheckBox_CES_UseSecPop_PES.Checked = .is_PES_SecPop
            Me.Numeric_CES_NExchange_SecPop_PES.Value = .n_PES_Interact
            Me.Numeric_CES_n_member_SecPop_PES.Value = .n_PES_MemSecPop

        End With

        'Hook and Jeeves
        '---------------
        With Me.msettings.HookJeeves

            Me.Numeric_HJ_DeltaStart.Value = .DnStart
            Me.Numeric_HJ_DeltaFinish.Value = .DnFinish
            Me.CheckBox_HJ_DNVektor.Checked = .is_DnVektor

        End With

        'MetaEvo
        '---------------
        With Me.msettings.MetaEvo

            Me.Combo_MetaEvo_Role.SelectedItem = .Role
            Me.Combo_MetaEvo_OpMode.SelectedItem = .OpMode
            Me.Numeric_MetaEvo_PopulationSize.Value = .PopulationSize
            Me.Numeric_MetaEvo_Numbergenerations.Value = .NumberGenerations
            Me.Numeric_MetaEvo_NumberResults.Value = .Numberresults
            Me.Numeric_MetaEvo_HJStepsize.Value = .HJStepsize
            Me.TextBox_MetaEvo_MySQL_Host.Text = .MySQL_Host
            Me.TextBox_MetaEvo_MySQL_DB.Text = .MySQL_Database
            Me.TextBox_MetaEvo_MySQL_User.Text = .MySQL_User
            Me.TextBox_MetaEvo_MySQL_Password.Text = .MySQL_Password

        End With

        'DDS
        '---------------
        With Me.msettings.DDS

            Me.Numeric_DDS_maxiter.Value = .maxiter
            Me.Numeric_DDS_r_val.Value = .r_val
            Me.CheckBox_DDS_ini.Checked = .optStartparameter

        End With

        'SensiPlot
        '--------------
        With Me.msettings.SensiPlot

            'Listboxen zurücksetzen
            Call Me.SensiPlot_ListBox_OptParameter.Items.Clear()
            Call Me.SensiPlot_ListBox_Objectives.Items.Clear()

            'Listboxen füllen
            For Each optpara As Common.OptParameter In Me.mProblem.List_OptParameter
                Call Me.SensiPlot_ListBox_OptParameter.Items.Add(optpara)
            Next
            For Each objective As Common.Objectivefunktion In Me.mProblem.List_ObjectiveFunctions
                Call Me.SensiPlot_ListBox_Objectives.Items.Add(objective)
            Next

            'OptParameter
            For Each selectedIndex As Integer In .Selected_OptParameters
                Me.SensiPlot_ListBox_OptParameter.SetSelected(selectedIndex, True)
            Next

            'Objective
            If (.Selected_Objective <> -1) Then
                Me.SensiPlot_ListBox_Objectives.SetSelected(.Selected_Objective, True)
            End If

            'Modus
            If (.Selected_SensiType = Common.EVO_Settings.SensiPlot_Settings.SensiType.discrete) Then
                Me.SensiPlot_RadioButton_Discrete.Checked = True
            Else
                Me.SensiPlot_RadioButton_NormalDistribution.Checked = True
            End If

            'Anzahl Schritte
            Me.SensiPlot_NumericUpDown_NumSteps.Value = .Num_Steps

            'Wave anzeigen
            Me.SensiPlot_CheckBox_wave.Checked = .show_Wave

        End With

        Call Application.DoEvents()

    End Sub

    'Settings für TestModus
    '**********************
    Public Sub setTestModus(ByVal Modus As CES_T_MODUS, ByVal Path() As Integer, ByVal nGen As Integer, ByVal nParents As Integer, ByVal NChilds As Integer)

        Dim i As Integer
        Dim PathStr As String

        If NChilds = 1 Then
            PathStr = "   Path: "
            For i = 0 To Path.GetUpperBound(0)
                PathStr = PathStr & Path(i) & " "
            Next
            PathStr = PathStr.TrimEnd
        Else
            PathStr = "   n_combi: "
            PathStr = PathStr & NChilds
        End If

        Me.Label_CES_OptModus.Text = "Modus: " & Modus.ToString & PathStr
        Me.Numeric_CES_n_Generations.Value = nGen
        Me.Numeric_CES_n_Parents.Minimum = 1
        Me.Numeric_CES_n_Parents.Value = nParents
        Me.Numeric_CES_n_childs.Minimum = 1
        Me.Numeric_CES_n_childs.Value = NChilds

    End Sub

#Region "Schnittstelle"

    'Schnittstelle
    'XXXXXXXXXXXXX

    'Standardeinstellungen setzen (PES)
    '**********************************
    Private Sub setStandard_PES(ByVal modus As EVO_MODUS)
        Call Me.msettings.PES.setStandard(modus)
        Call Me.writeForm()
    End Sub

    'Standardeinstellungen setzen (CES)
    '**********************************
    Private Sub setStandard_CES()
        Call Me.msettings.CES.setStandard(Me.mProblem.Method)
        Call Me.writeForm()
    End Sub

    'Standardeinstellungen setzen für HJ
    '***********************************
    Private Sub setStandard_HJ()
        Call Me.msettings.HookJeeves.setStandard()
        Call Me.writeForm()
    End Sub

    'Standardeinstellungen setzen für MetaEVO
    '****************************************
    Private Sub setStandard_MetaEvo()
        Call Me.msettings.MetaEvo.setStandard()
        Call Me.writeForm()
    End Sub

    'Standardeinstellungen setzen für DDS
    '************************************
    Private Sub setStandard_DDS()
        Call Me.msettings.DDS.setStandard()
        Call Me.writeForm()
    End Sub

    'Standardeinstellungen setzen für SensiPlot
    '******************************************
    Private Sub setStandard_SensiPlot()
        Call Me.msettings.SensiPlot.setStandard()
        Call Me.writeForm()
    End Sub

    'Speichern der EVO_Settings in einer XML-Datei
    '*********************************************
    Public Sub saveSettings(ByVal filename As String)

        Call Me.readForm()

        Dim serializer As New XmlSerializer(GetType(Common.EVO_Settings))
        Dim writer As New StreamWriter(filename)
        serializer.Serialize(writer, Me.msettings)
        writer.Close()

        Me.isSaved = True

    End Sub

    'Laden der EVO_Settings aus einer XML-Datei
    '******************************************
    Public Sub loadSettings(ByVal filename As String)

        Dim serializer As New XmlSerializer(GetType(Common.EVO_Settings))

        AddHandler serializer.UnknownElement, AddressOf serializerUnknownElement
        AddHandler serializer.UnknownAttribute, AddressOf serializerUnknownAttribute

        'Filestream öffnen
        Dim fs As New FileStream(filename, FileMode.Open)

        Try
            'Deserialisieren
            'TODO: XmlDeserializationEvents ms-help://MS.VSCC.v90/MS.MSDNQTR.v90.en/fxref_system.xml/html/e0657840-5678-bf57-6e7a-1bd93b2b27d1.htm
            Me.msettings = CType(serializer.Deserialize(fs), Common.EVO_Settings)

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


    'MetaEvo: Voreinstellungen des Formulars aufgrund der Wahl der Rolle
    Private Sub Combo_Hybrid_Role_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Combo_MetaEvo_Role.SelectedIndexChanged
        If (Me.Combo_MetaEvo_Role.SelectedItem = "Single PC") Then
            Me.Combo_MetaEvo_OpMode.Enabled = True
            Combo_MetaEvo_OpMode_SelectedIndexChanged(sender, e)
            Me.GroupBox_MetaEvo_MySQLOptions.Enabled = False
        ElseIf (Me.Combo_MetaEvo_Role.SelectedItem = "Network Client") Then
            Me.Combo_MetaEvo_OpMode.Enabled = False
            Me.GroupBox_MetaEvo_BasicOptions.Enabled = False
            Me.GroupBox_MetaEvo_TransferOptions.Enabled = False
            Me.GroupBox_MetaEvo_LocalOptions.Enabled = False
            Me.GroupBox_MetaEvo_MySQLOptions.Enabled = True
        ElseIf (Me.Combo_MetaEvo_Role.SelectedItem = "Network Server") Then
            Me.Combo_MetaEvo_OpMode.Enabled = True
            Combo_MetaEvo_OpMode_SelectedIndexChanged(sender, e)
            Me.GroupBox_MetaEvo_MySQLOptions.Enabled = True
        End If
    End Sub

    Private Sub Combo_MetaEvo_OpMode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Combo_MetaEvo_OpMode.SelectedIndexChanged
        If (Me.Combo_MetaEvo_OpMode.SelectedItem = "Both") Then
            Me.GroupBox_MetaEvo_BasicOptions.Enabled = True
            Me.GroupBox_MetaEvo_TransferOptions.Enabled = True
            Me.GroupBox_MetaEvo_LocalOptions.Enabled = True
        ElseIf (Me.Combo_MetaEvo_OpMode.SelectedItem = "Global Optimizer") Then
            Me.GroupBox_MetaEvo_BasicOptions.Enabled = True
            Me.GroupBox_MetaEvo_TransferOptions.Enabled = True
            Me.GroupBox_MetaEvo_LocalOptions.Enabled = False
        ElseIf (Me.Combo_MetaEvo_OpMode.SelectedItem = "Local Optimizer") Then
            Me.GroupBox_MetaEvo_BasicOptions.Enabled = False
            Me.GroupBox_MetaEvo_TransferOptions.Enabled = True
            Me.GroupBox_MetaEvo_LocalOptions.Enabled = True
        End If
    End Sub

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

#End Region 'Methoden

End Class
