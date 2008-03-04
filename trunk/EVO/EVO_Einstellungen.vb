Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports IHWB.EVO.Kern

Public Class EVO_Einstellungen
    Inherits System.Windows.Forms.UserControl

    'Eigenschaften
    '#############

    Private msettings As EVO.Kern.EVO_Settings       'Sicherung sämtlicher Einstellungen
    Public isSaved As Boolean = False                'Flag der anzeigt, ob die Einstellungen bereits gesichert wurden
    Public isLoad As Boolean = False                 'Flag der anzeigt, ob die Settings aus einer XML Datei gelesen werden

    'Methoden
    '########

    'Konstruktor
    '***********
    Public Sub New()

        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        Call Me.InitializeComponent()
        
        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        'Settings instanzieren
        Me.msettings = New EVO.Kern.EVO_Settings()
        'Standard-Settings setzen
        Call Me.msettings.PES.setStandard(EVO_MODUS.Single_Objective)
        Call Me.msettings.CES.setStandard(Evo.METH_CES)
        Call Me.msettings.HookJeeves.setStandard()
        'Comboboxen füllen
        Call Me.InitComboboxes()

    End Sub

    'Laden des Formulars
    '*******************
    Private Sub EVO_Einstellungen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'EventHandler einrichten
        AddHandler Me.ÖffnenToolStripButton.Click, AddressOf Form1.Load_EVO_Settings
        AddHandler Me.SpeichernToolStripButton.Click, AddressOf Form1.Save_EVO_Settings

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
                LabelInteract.Enabled = False
                TextInteract.Enabled = False
                LabelNMemberSecondPop.Enabled = False
                TextNMemberSecondPop.Enabled = False
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
                LabelInteract.Enabled = True
                TextInteract.Enabled = True
                LabelNMemberSecondPop.Enabled = True
                TextNMemberSecondPop.Enabled = True
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

        Select Case Evo.Form1.Method

            Case evo.METH_CES
                GroupBox_Hybrid.Enabled = false

            Case evo.METH_HYBRID
                GroupBox_Hybrid.Enabled = True

                Dim sender as Object = 1
                Dim e as system.EventArgs = system.EventArgs.Empty
                call Combo_CES_HybridType_SelectedIndexChanged(sender, e)

        End Select

    End sub


    Public Sub SetFor_CES_PES(ByVal AnzGen As Integer, ByVal AnzEltern As Integer, ByVal AnzNachf As Integer)

        'Vorgaben und Anzeige
        TextAnzGen.Text = CStr(AnzGen)
        TextAnzEltern.Text = CStr(AnzEltern)
        TextAnzNachf.Text = CStr(AnzNachf)

        System.Windows.Forms.Application.DoEvents()

    End Sub


    'UPGRADE_WARNING: Das Ereignis ComboOptEltern.SelectedIndexChanged kann ausgelöst werden, wenn das Formular initialisiert wird. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2075"'
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

    'UPGRADE_WARNING: Das Ereignis CheckisPopul.CheckStateChanged kann ausgelöst werden, wenn das Formular initialisiert wird. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2075"'
    Private Sub CheckisPopul_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CheckisPopul.CheckStateChanged

        If (CheckisPopul.Checked) Then
            GroupBox_Populationen.Enabled = True
        Else
            GroupBox_Populationen.Enabled = False
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
        Me.Combo_CES_IniValues.DataSource = System.Enum.GetValues(gettype(EVO_STARTPARAMETER))
        Me.Combo_CES_HybridType.DataSource = System.Enum.GetValues(gettype(HYBRID_TYPE))
        Me.Combo_CES_MemStrategy.DataSource = System.Enum.GetValues(gettype(MEMORY_STRATEGY))

    End Sub

     Private Sub FILLCOMBO_POPPENALTY(ByRef Cntrl As System.Windows.Forms.ComboBox)

        Cntrl.Items.Clear()
        Select Case Me.msettings.PES.OptModus

            Case EVO.Kern.EVO_MODUS.Single_Objective
                Cntrl.Items.Add(EVO_POP_PENALTY.Mittelwert)
                Cntrl.Items.Add(EVO_POP_PENALTY.Schlechtester)

            Case EVO.Kern.EVO_MODUS.Multi_Objective
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
                .is_Interact = False
                .n_Interact = 1
            Else
                .is_Interact = True
                .n_Interact = TextInteract.Value
            End If
            .n_MemberSekPop = TextNMemberSecondPop.Value
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
            'Reduzierte Darstellung
            .is_paint_constraint = checkpaintconstrained.Checked

        End With

        'CES
        '---
        With Me.msettings.CES

            ' = me.Combo_CES_IniValues.SelectedItem
            .n_Generations = me.Numeric_CES_n_Generations.Value
            .n_Parents = me.Numeric_CES_n_Parents.Value
            .n_Childs = me.Numeric_CES_n_childs.Value
            .OptStrategie = me.Combo_CES_Selection.SelectedItem
            .OptReprodOp = me.Combo_CES_Reproduction.SelectedItem
            .OptMutOperator = me.Combo_CES_Mutation.SelectedItem
            .pr_MutRate = me.Numeric_CES_MutRate.Value
            .is_SecPop = me.CheckBox_CES_UseSecPop_CES.Checked
            .n_Interact = me.Numeric_CES_n_exchange_SecPop.Value
            .n_MemberSecondPop = me.Numeric_CES_n_member_SecPop.Value

            'HYBRID hängt von der Methode ab
            .is_RealOpt = me.CheckBox_CES_RealOptimisation.Checked
            .ty_Hybrid = me.Combo_CES_HybridType.SelectedItem
            .Mem_Strategy = me.Combo_CES_MemStrategy.SelectedItem
            .n_PES_MemSize = me.Numeric_CES_n_MemSize.Value
            .is_PopMutStart = me.CheckBox_CES_StartPESPop.Checked
            .is_PES_SecPop = me.CheckBox_CES_UseSecPop_PES.Checked
            .n_PES_Interact = me.Numeric_CES_n_exchange_SecPop_PES.Value
            .n_PES_MemSecPop = me.Numeric_CES_n_member_SecPop_PES.Value

        End With

        'Hooke and Jeeves
        '----------------
        With Me.msettings.HookJeeves

            .DnStart = Me.TextDeltaStartHJ.Value
            .DnFinish = Me.TextDeltaFinishHJ.Value
            .is_DnVektor = Me.CheckBoxDNVektorHJ.Checked

        End With

    End Sub

    'Setzt/Aktiviert/Deaktiviert die Einstellungen auf den PES Settings
    '******************************************************************
    Private Sub Combo_CES_HybridType_SelectedIndexChanged( ByVal sender As System.Object,  ByVal e As System.EventArgs) Handles Combo_CES_HybridType.SelectedIndexChanged


        If Form1.Method = Evo.METH_HYBRID And not isLoad Then

            Dim Item as HYBRID_TYPE
            Item = Me.Combo_CES_HybridType.SelectedItem

            Select Case Item

                case HYBRID_TYPE.Mixed_Integer

                    'Generationen
                    me.TextAnzGen.Text = 1
                    me.TextAnzGen.Enabled = False

                    'Eltern
                    me.TextAnzEltern.text = 5
                    me.TextAnzEltern.Enabled = True
                    me.LabelAnzEltern.Text = "Maximal Zahl der Eltern:"

                    'Childs
                    me.TextAnzNachf.Text = 1
                    me.TextAnzNachf.Enabled = False

                case HYBRID_TYPE.Sequencial_1

                    'Generationen
                    me.TextAnzGen.Text = 10
                    me.TextAnzGen.Enabled = True

                    'Eltern
                    me.TextAnzEltern.text = 5
                    me.TextAnzEltern.Enabled = True
                    me.LabelAnzEltern.Text = "Anzahl der Eltern:"

                    'Childs
                    me.TextAnzNachf.Text = 15
                    me.TextAnzNachf.Enabled = True

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
            If (Me.msettings.PES.is_Interact) Then
                Me.TextInteract.Value = .n_Interact
            Else
                Me.TextInteract.Value = 0
            End If
            Me.TextNMemberSecondPop.Value = .n_MemberSekPop
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
            'Reduzierte Darstellung
            Me.checkpaintconstrained.Checked = .is_paint_constraint

        End With

        'PES
        '---
        With Me.msettings.CES

            Call OptModus_Change_ActDeact_CES()

            'me.Combo_CES_IniValues.SelectedItem = .
            me.Numeric_CES_n_Generations.Value = .n_Generations
            me.Numeric_CES_n_Parents.Value = .n_Parents
            me.Numeric_CES_n_childs.Value = .n_Childs
            me.Combo_CES_Selection.SelectedItem = .OptStrategie
            me.Combo_CES_Reproduction.SelectedItem = .OptReprodOp 
            me.Combo_CES_Mutation.SelectedItem = .OptMutOperator
            me.Numeric_CES_MutRate.Value = .pr_MutRate
            me.CheckBox_CES_UseSecPop_CES.Checked = .is_SecPop
            me.Numeric_CES_n_exchange_SecPop.Value = .n_Interact
            me.Numeric_CES_n_member_SecPop.Value = .n_MemberSecondPop

            'HYBRID hängt von der Methode ab
            me.CheckBox_CES_RealOptimisation.Checked = .is_RealOpt
            me.Combo_CES_HybridType.SelectedItem = .ty_Hybrid
            me.Combo_CES_MemStrategy.SelectedItem = .Mem_Strategy
            me.Numeric_CES_n_MemSize.Value = .n_PES_MemSize
            me.CheckBox_CES_StartPESPop.Checked = .is_PopMutStart
            me.CheckBox_CES_UseSecPop_PES.Checked = .is_PES_SecPop
            me.Numeric_CES_n_exchange_SecPop_PES.Value = .n_PES_Interact
            me.Numeric_CES_n_member_SecPop_PES.Value = .n_PES_MemSecPop

        End With

        'Hook and Jeeves
        '---------------
        With Me.msettings.HookJeeves

            Me.TextDeltaStartHJ.Value = .DnStart
            Me.TextDeltaFinishHJ.Value = .DnFinish
            Me.CheckBoxDNVektorHJ.Checked = .is_DnVektor

        End With

        Call Application.DoEvents()

    End Sub

    'Settings für TestModus
    '**********************
    Public Sub setTestModus(byVal Modus as CES_T_MODUS,byVal Path() as Integer, byVal nGen as Integer,byVal nParents as Integer,byVal NChilds as Integer)
        
        Dim i as Integer
        Dim PathStr as String

        If nChilds = 1 then
            PathStr = "   Path: "
            For i = 0 to Path.GetUpperBound(0)
                PathStr = PathStr & Path(i) & " "
            Next
            PathStr = PathStr.Trimend
        Else
            PathStr = "   n_combi: "
            PathStr = PathStr & nChilds
        End If

        me.Label_CES_OptModus.Text = "Modus: " & Modus.ToString & PathStr
        me.Numeric_CES_n_Generations.Value = nGen
        me.Numeric_CES_n_Parents.Minimum = 1
        me.Numeric_CES_n_Parents.Value = nParents
        me.Numeric_CES_n_childs.Minimum = 1
        me.Numeric_CES_n_childs.Value = nChilds

    End Sub

#Region "Schnittstelle"

    'Schnittstelle
    'XXXXXXXXXXXXX

    'Standardeinstellungen setzen (PES)
    '**********************************
    Public Sub setStandard_PES(ByVal modus As Kern.EVO_MODUS)
        Call Me.msettings.PES.setStandard(modus)
        Call Me.writeForm()
    End Sub

    'Standardeinstellungen setzen (CES)
    '**********************************
    Public Sub setStandard_CES()
        Call Me.msettings.CES.setStandard(Evo.Form1.Method)
        Call Me.writeForm()
    End Sub

    'Standardeinstellungen setzen für HJ
    '***********************************
    Public Sub setStandard_HJ()
        Call Me.msettings.HookJeeves.setStandard()
        Call Me.writeForm()
    End Sub

    'PES_Settings Property
    '*********************
    Public ReadOnly Property Settings() As EVO.Kern.EVO_Settings
        Get
            'Wenn Einstellungen noch nicht gespeichert, zuerst aus Form einlesen
            If (Not Me.isSaved) Then
                Call Me.readForm()
            End If
            Settings = Me.msettings
        End Get
    End Property

    'Speichern der EVO_Settings in einer XML-Datei
    '*********************************************
    Public Sub saveSettings(ByVal filename As String)

        Call Me.readForm()

        Dim serializer As New XmlSerializer(GetType(EVO.Kern.EVO_Settings))
        Dim writer As New StreamWriter(filename)
        serializer.Serialize(writer, Me.msettings)
        writer.Close()

        Me.isSaved = True

    End Sub

    'Laden der EVO_Settings aus einer XML-Datei
    '******************************************
    Public Sub loadSettings(ByVal filename As String)

        Dim serializer As New XmlSerializer(GetType(EVO.Kern.EVO_Settings))

        ' If the XML document has been altered with unknown
        ' nodes or attributes, handle them with the
        ' UnknownNode and UnknownAttribute events.
        'AddHandler serializer.UnknownNode, AddressOf serializer_UnknownNode
        'AddHandler serializer.UnknownAttribute, AddressOf serializer_UnknownAttribute

        'XML-Datei einlesen
        Dim fs As New FileStream(filename, FileMode.Open)
        Me.msettings = CType(serializer.Deserialize(fs), EVO.Kern.EVO_Settings)
        fs.Close()

        'Geladene Settings in Form schreiben
        isLoad = True
        Call Me.writeForm()
        isLoad = False

    End Sub

#End Region 'Schnittstelle

End Class