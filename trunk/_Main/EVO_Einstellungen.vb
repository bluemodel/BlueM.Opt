Option Strict Off
Option Explicit On
Imports IHWB.EVO.Kern

Public Class EVO_Einstellungen
    Inherits System.Windows.Forms.UserControl

    Private _settings As PES.Struct_Settings        'Sicherung sämtlicher Einstellungen
    Public isSaved As Boolean = False               'Flag der anzeigt, ob die Einstellungen bereits gesichert wurden

    Private OptModusValue As Short = EVO_MODUS.Single_Objective
    Dim isMultiObjectiveOptimierung As Boolean

    'Optimierungsmodus wurde geändert
    '********************************
    Private Sub OptModus_Change()

        Select Case Me.OptModus

            Case EVO_MODUS.Single_Objective
                'Vorgaben und Anzeige
                Label_OptModusValue.Text = "Single Objective"
                TextAnzGen.Text = CStr(20)
                TextAnzEltern.Text = CStr(3)
                TextAnzNachf.Text = CStr(10)
                TextAnzRunden.Text = CStr(10)
                'Modus
                isMultiObjectiveOptimierung = False
                'Strategie
                ComboStrategie.Enabled = True
                'Sekundäre Population
                LabelInteract.Enabled = False
                TextInteract.Enabled = False
                LabelNMemberSecondPop.Enabled = False
                TextNMemberSecondPop.Enabled = False
                'Populationen
                CheckisPopul.Enabled = True
                CheckisPopul.Checked = False
                GroupBox_Populationen.Enabled = False

            Case EVO_MODUS.Multi_Objective
                'Vorgaben und Anzeige
                Label_OptModusValue.Text = "MultiObjective Pareto"
                TextAnzGen.Text = CStr(250)
                TextAnzEltern.Text = CStr(25)
                TextAnzNachf.Text = CStr(75)
                TextAnzRunden.Text = CStr(10)
                'Modus
                isMultiObjectiveOptimierung = True
                'Strategie
                ComboStrategie.Text = "'+' (Eltern+Nachfolger)"
                ComboStrategie.Enabled = False
                'Sekundäre Population
                LabelInteract.Enabled = True
                TextInteract.Enabled = True
                LabelNMemberSecondPop.Enabled = True
                TextNMemberSecondPop.Enabled = True
                'Populationen
                CheckisPopul.Enabled = False
                CheckisPopul.Checked = False
                GroupBox_Populationen.Enabled = False

        End Select

        Call FILLCOMBO_OPTELTERN(ComboOptEltern)
        Call FILLCOMBO_POPPENALTY(ComboPopPenalty)
    End Sub

    Public Sub SetFor_CES_PES(byVal AnzGen as integer, byVal AnzEltern as integer, byVal AnzNachf as integer)
        'Vorgaben und Anzeige
        Label_OptModusValue.Text = "MultiObjective Pareto"
        TextAnzGen.Text = CStr(AnzGen)
        TextAnzEltern.Text = CStr(AnzEltern)
        TextAnzNachf.Text = CStr(AnzNachf)

    End Sub


    'UPGRADE_WARNING: Das Ereignis ComboOptEltern.SelectedIndexChanged kann ausgelöst werden, wenn das Formular initialisiert wird. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2075"'
    Private Sub ComboOptEltern_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboOptEltern.SelectedIndexChanged
        Select Case VB6.GetItemData(ComboOptEltern, ComboOptEltern.SelectedIndex)
            Case EVO_ELTERN.XY_Diskret, EVO_ELTERN.XY_Mitteln, EVO_ELTERN.Neighbourhood
                LabelRekombxy1.Enabled = True
                LabelRekombxy3.Enabled = True
                TextRekombxy.Enabled = True
            Case Else
                LabelRekombxy1.Enabled = False
                LabelRekombxy3.Enabled = False
                TextRekombxy.Enabled = False
        End Select
    End Sub

    Private Sub TextAnzEltern_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextAnzEltern.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        'UPGRADE_ISSUE: Zuweisung wird nicht unterstützt: KeyAscii an Nicht-Null-Wert Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1058"'
        KeyAscii = KEYOK(KeyAscii, AllowIntegerOnly)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub TextAnzGen_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextAnzGen.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        'UPGRADE_ISSUE: Zuweisung wird nicht unterstützt: KeyAscii an Nicht-Null-Wert Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1058"'
        KeyAscii = KEYOK(KeyAscii, AllowIntegerOnly)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub TextAnzNachf_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextAnzNachf.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        'UPGRADE_ISSUE: Zuweisung wird nicht unterstützt: KeyAscii an Nicht-Null-Wert Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1058"'
        KeyAscii = KEYOK(KeyAscii, AllowIntegerOnly)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub TextAnzPop_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextAnzPop.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        'UPGRADE_ISSUE: Zuweisung wird nicht unterstützt: KeyAscii an Nicht-Null-Wert Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1058"'
        KeyAscii = KEYOK(KeyAscii, AllowIntegerOnly)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub TextAnzPopEltern_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextAnzPopEltern.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        'UPGRADE_ISSUE: Zuweisung wird nicht unterstützt: KeyAscii an Nicht-Null-Wert Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1058"'
        KeyAscii = KEYOK(KeyAscii, AllowIntegerOnly)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub TextAnzRunden_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextAnzRunden.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        'UPGRADE_ISSUE: Zuweisung wird nicht unterstützt: KeyAscii an Nicht-Null-Wert Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1058"'
        KeyAscii = KEYOK(KeyAscii, AllowIntegerOnly)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub TextDeltaStart_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextDeltaStart.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        'UPGRADE_ISSUE: Zuweisung wird nicht unterstützt: KeyAscii an Nicht-Null-Wert Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1058"'
        KeyAscii = KEYOK(KeyAscii, AllowPositiveFigures)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub TextDeltaMin_KeyPress(ByRef KeyAscii As Short)
        KeyAscii = KEYOK(KeyAscii, AllowPositiveFigures)
    End Sub

    Private Sub TextaDeltaStart_KeyPress(ByRef KeyAscii As Short)
        KeyAscii = KEYOK(KeyAscii, AllowPositiveFigures)
    End Sub

    Private Sub TextaDeltaMin_KeyPress(ByRef KeyAscii As Short)
        KeyAscii = KEYOK(KeyAscii, AllowPositiveFigures)
    End Sub

    Private Sub TextRekombxy_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles TextRekombxy.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        'UPGRADE_ISSUE: Zuweisung wird nicht unterstützt: KeyAscii an Nicht-Null-Wert Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1058"'
        KeyAscii = KEYOK(KeyAscii, AllowIntegerOnly)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    'UPGRADE_WARNING: Das Ereignis CheckisPopul.CheckStateChanged kann ausgelöst werden, wenn das Formular initialisiert wird. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2075"'
    Private Sub CheckisPopul_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CheckisPopul.CheckStateChanged

        If (CheckisPopul.Checked) Then
            GroupBox_Populationen.Enabled = True
        Else
            GroupBox_Populationen.Enabled = False
        End If
    End Sub


    Private Sub FILLCOMBO_STRATEGIE(ByRef Cntrl As System.Windows.Forms.ComboBox)
        Cntrl.Items.Add(New VB6.ListBoxItem("'+' (Eltern+Nachfolger)", EVO_STRATEGIE.Plus))
        Cntrl.Items.Add(New VB6.ListBoxItem("',' (nur Nachfolger)", EVO_STRATEGIE.Komma))
        Cntrl.SelectedIndex = 0
    End Sub

    Private Sub FILLCOMBO_OPTPOPELTERN(ByRef Cntrl As System.Windows.Forms.ComboBox)
        Cntrl.Items.Add(New VB6.ListBoxItem("mit Rekombination", EVO_POP_ELTERN.Rekombination))
        Cntrl.Items.Add(New VB6.ListBoxItem("Mittelwerte", EVO_POP_ELTERN.Mittelwert))
        Cntrl.Items.Add(New VB6.ListBoxItem("Selektion", EVO_POP_ELTERN.Selektion))
        Cntrl.SelectedIndex = 0
    End Sub

    Private Sub FILLCOMBO_OPTELTERN(ByRef Cntrl As System.Windows.Forms.ComboBox)
        Cntrl.Items.Clear()
        Cntrl.Items.Add(New VB6.ListBoxItem("Selektion", EVO_ELTERN.Selektion))
        Cntrl.Items.Add(New VB6.ListBoxItem("Rekomb x/x, diskret", EVO_ELTERN.XX_Diskret))
        Cntrl.Items.Add(New VB6.ListBoxItem("Rekomb x/x, mitteln", EVO_ELTERN.XX_Mitteln))
        Cntrl.Items.Add(New VB6.ListBoxItem("Rekomb x/y, diskret", EVO_ELTERN.XY_Diskret))
        Cntrl.Items.Add(New VB6.ListBoxItem("Rekomb x/y, mitteln", EVO_ELTERN.XY_Mitteln))
        If (Me.OptModus = EVO_MODUS.Multi_Objective) Then
            Cntrl.Items.Add(New VB6.ListBoxItem("Neighbourhood", EVO_ELTERN.Neighbourhood))
        End If
        Cntrl.SelectedIndex = 1
    End Sub

    Private Sub FILLCOMBO_OPTVORGABE(ByRef Cntrl As System.Windows.Forms.ComboBox)
        Cntrl.Items.Add(New VB6.ListBoxItem("Zufällig", EVO_STARTPARAMETER.Zufall))
        Cntrl.Items.Add(New VB6.ListBoxItem("Originalparameter", EVO_STARTPARAMETER.Original))
        Cntrl.SelectedIndex = 1
    End Sub

    Private Sub FILLCOMBO_POPPENALTY(ByRef Cntrl As System.Windows.Forms.ComboBox)
        Cntrl.Items.Clear()
        Select Case Me.OptModusValue
            Case EVO.Kern.EVO_MODUS.Single_Objective
                Cntrl.Items.Add(New VB6.ListBoxItem("Mittelwert", EVO_POP_PENALTY.Mittelwert))
                Cntrl.Items.Add(New VB6.ListBoxItem("Schlechtester", EVO_POP_PENALTY.Schlechtester))
                Cntrl.SelectedIndex = 0
            Case EVO.Kern.EVO_MODUS.Multi_Objective
                Cntrl.Items.Add(New VB6.ListBoxItem("Crowding", EVO_POP_PENALTY.Crowding))
                Cntrl.Items.Add(New VB6.ListBoxItem("Spannweite", EVO_POP_PENALTY.Spannweite))
                Cntrl.SelectedIndex = 0
        End Select
    End Sub

    Private Sub UserControl_Initialize()
        Call FILLCOMBO_STRATEGIE(ComboStrategie)
        Call FILLCOMBO_STRATEGIE(ComboPopStrategie)
        Call FILLCOMBO_OPTPOPELTERN(ComboOptPopEltern)
        Call FILLCOMBO_OPTELTERN(ComboOptEltern)
        Call FILLCOMBO_OPTVORGABE(ComboOptVorgabe)
        Call FILLCOMBO_POPPENALTY(ComboPopPenalty)
    End Sub

    'Einstellungen aus Form einlesen
    '*******************************
    Public Sub readSettings()

        _settings.n_Eltern = Val(TextAnzEltern.Text)
        _settings.n_Nachf = Val(TextAnzNachf.Text)
        _settings.n_Gen = Val(TextAnzGen.Text)
        _settings.ty_EvoTyp = VB6.GetItemData(ComboStrategie, ComboStrategie.SelectedIndex)
        _settings.ty_PopEvoTyp = VB6.GetItemData(ComboPopStrategie, ComboPopStrategie.SelectedIndex)
        _settings.ty_PopPenalty = VB6.GetItemData(ComboPopPenalty, ComboPopPenalty.SelectedIndex)
        _settings.is_MO_Pareto = isMultiObjectiveOptimierung
        _settings.is_POPUL = CheckisPopul.Checked
        If (_settings.is_POPUL) Then
            _settings.n_Runden = Val(TextAnzRunden.Text)
            _settings.n_Popul = Val(TextAnzPop.Text)
            _settings.n_PopEltern = Val(TextAnzPopEltern.Text)
        Else
            _settings.n_Runden = 1
            _settings.n_Popul = 1
            _settings.n_PopEltern = 1
        End If
        _settings.ty_OptEltern = VB6.GetItemData(ComboOptEltern, ComboOptEltern.SelectedIndex)
        _settings.ty_OptPopEltern = VB6.GetItemData(ComboOptPopEltern, ComboOptPopEltern.SelectedIndex)
        _settings.n_RekombXY = Val(TextRekombxy.Text)
        _settings.DnStart = Val(TextDeltaStart.Text)
        _settings.ty_StartPar = VB6.GetItemData(ComboOptVorgabe, ComboOptVorgabe.SelectedIndex)
        _settings.is_DnVektor = CheckisDnVektor.Checked
        If (Val(TextInteract.Text) <= 0) Then
            _settings.is_Interact = False
            _settings.n_Interact = 1
        Else
            _settings.is_Interact = True
            _settings.n_Interact = Val(TextInteract.Text)
        End If
        _settings.n_MemberSecondPop = Val(TextNMemberSecondPop.Text)

    End Sub

#Region "Schnittstelle"

    'Schnittstelle
    'XXXXXXXXXXXXX

    'Dieses Property nicht ReadOnly weil die Anzahl der Zielfunktionen durch OptZiele bestimmt werden kann
    Public Property OptModus() As Short
        Get
            OptModus = Me.OptModusValue
        End Get
        Set(ByVal Index As Short)
            Me.OptModusValue = Index
            Call OptModus_Change()
        End Set
    End Property

    Public ReadOnly Property PES_Settings() As PES.Struct_Settings
        Get
            'Wenn Einstellungen noch nicht gespeichert, zuerst einlesen
            If (Not Me.isSaved) Then
                Call readSettings()
            End If
            PES_Settings = Me._settings
        End Get
    End Property

#End Region 'Schnittstelle

End Class