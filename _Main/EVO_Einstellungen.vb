Option Strict Off
Option Explicit On
Public Class EVO_Einstellungen
    Inherits System.Windows.Forms.UserControl

    Private _settings As EVO.Kern.PES.Struct_Settings                'Sicherung sämtlicher Einstellungen
    'BUG 225: isSaved müsste bei Neustart zurückgesetzt werden
    Private isSaved As Boolean = False                              'Flag der anzeigt, ob die Einstellungen bereits gesichert wurden

    Private OptModusValue As Short = EVO_MODUS_SINGEL_OBJECTIVE
    Dim isMultiObjectiveOptimierung As Boolean

    'Optimierungsmodus wurde geändert
    '********************************
    Private Sub OptModus_Change()

        Select Case Me.OptModus

            Case EVO_MODUS_SINGEL_OBJECTIVE
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

            Case EVO_MODUS_MULTIOBJECTIVE_PARETO
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

    Public Sub SetFor_CES_PES()
        'Vorgaben und Anzeige
        Label_OptModusValue.Text = "MultiObjective Pareto"
        TextAnzGen.Text = CStr(1)
        TextAnzEltern.Text = CStr(3)
        TextAnzNachf.Text = CStr(5)

    End Sub


    'UPGRADE_WARNING: Das Ereignis ComboOptEltern.SelectedIndexChanged kann ausgelöst werden, wenn das Formular initialisiert wird. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup2075"'
    Private Sub ComboOptEltern_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ComboOptEltern.SelectedIndexChanged
        Select Case VB6.GetItemData(ComboOptEltern, ComboOptEltern.SelectedIndex)
            Case EVO_ELTERN_XY_DISKRET, EVO_ELTERN_XY_MITTELN, EVO_ELTERN_Neighbourhood
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
        Cntrl.Items.Add(New VB6.ListBoxItem("'+' (Eltern+Nachfolger)", EVO_PLUS))
        Cntrl.Items.Add(New VB6.ListBoxItem("',' (nur Nachfolger)", EVO_KOMMA))
        Cntrl.SelectedIndex = 0
    End Sub

    Private Sub FILLCOMBO_OPTPOPELTERN(ByRef Cntrl As System.Windows.Forms.ComboBox)
        Cntrl.Items.Add(New VB6.ListBoxItem("mit Rekombination", EVO_POPELTERN_REKOMB))
        Cntrl.Items.Add(New VB6.ListBoxItem("Mittelwerte", EVO_POPELTERN_MITTEL))
        Cntrl.Items.Add(New VB6.ListBoxItem("Selektion", EVO_POPELTERN_SELEKT))
        Cntrl.SelectedIndex = 0
    End Sub

    Private Sub FILLCOMBO_OPTELTERN(ByRef Cntrl As System.Windows.Forms.ComboBox)
        Cntrl.Items.Clear()
        Cntrl.Items.Add(New VB6.ListBoxItem("Selektion", EVO_ELTERN_SELEKT))
        Cntrl.Items.Add(New VB6.ListBoxItem("Rekomb x/x, diskret", EVO_ELTERN_XX_DISKRET))
        Cntrl.Items.Add(New VB6.ListBoxItem("Rekomb x/x, mitteln", EVO_ELTERN_XX_MITTELN))
        Cntrl.Items.Add(New VB6.ListBoxItem("Rekomb x/y, diskret", EVO_ELTERN_XY_DISKRET))
        Cntrl.Items.Add(New VB6.ListBoxItem("Rekomb x/y, mitteln", EVO_ELTERN_XY_MITTELN))
        If (Me.OptModus = EVO_MODUS_MULTIOBJECTIVE_PARETO) Then
            Cntrl.Items.Add(New VB6.ListBoxItem("Neighbourhood", EVO_ELTERN_Neighbourhood))
        End If
        Cntrl.SelectedIndex = 1
    End Sub

    Private Sub FILLCOMBO_OPTVORGABE(ByRef Cntrl As System.Windows.Forms.ComboBox)
        Cntrl.Items.Add(New VB6.ListBoxItem("Zufällig", globalVORGABE_ZUFALL))
        Cntrl.Items.Add(New VB6.ListBoxItem("Originalparameter", globalVORGABE_Original))
        Cntrl.SelectedIndex = 1
    End Sub

    Private Sub FILLCOMBO_POPPENALTY(ByRef Cntrl As System.Windows.Forms.ComboBox)
        Cntrl.Items.Clear()
        Select Case Me.OptModusValue
            Case EVO_MODUS_SINGEL_OBJECTIVE
                Cntrl.Items.Add(New VB6.ListBoxItem("Mittelwert", EVO_POP_PENALTY_MITTELWERT))
                Cntrl.Items.Add(New VB6.ListBoxItem("Schlechtester", EVO_POP_PENALTY_SCHLECHTESTER))
                Cntrl.SelectedIndex = 0
            Case EVO_MODUS_MULTIOBJECTIVE_PARETO
                Cntrl.Items.Add(New VB6.ListBoxItem("Crowding", EVO_POP_PENALTY_CROWDING))
                Cntrl.Items.Add(New VB6.ListBoxItem("Spannweite", EVO_POP_PENALTY_SPANNWEITE))
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

    'Speichern der Einstellungen
    '***************************
    Private Sub saveSettings()

        _settings.NEltern = Val(TextAnzEltern.Text)
        _settings.NNachf = Val(TextAnzNachf.Text)
        _settings.NGen = Val(TextAnzGen.Text)
        _settings.iEvoTyp = VB6.GetItemData(ComboStrategie, ComboStrategie.SelectedIndex)
        _settings.iPopEvoTyp = VB6.GetItemData(ComboPopStrategie, ComboPopStrategie.SelectedIndex)
        _settings.iPopPenalty = VB6.GetItemData(ComboPopPenalty, ComboPopPenalty.SelectedIndex)
        _settings.is_MO_Pareto = isMultiObjectiveOptimierung
        _settings.isPOPUL = CheckisPopul.Checked
        If (_settings.isPOPUL) Then
            _settings.NRunden = Val(TextAnzRunden.Text)
            _settings.NPopul = Val(TextAnzPop.Text)
            _settings.NPopEltern = Val(TextAnzPopEltern.Text)
        Else
            _settings.NRunden = 1
            _settings.NPopul = 1
            _settings.NPopEltern = 1
        End If
        _settings.iOptEltern = VB6.GetItemData(ComboOptEltern, ComboOptEltern.SelectedIndex)
        _settings.iOptPopEltern = VB6.GetItemData(ComboOptPopEltern, ComboOptPopEltern.SelectedIndex)
        _settings.NRekombXY = Val(TextRekombxy.Text)
        _settings.rDeltaStart = Val(TextDeltaStart.Text)
        _settings.iStartPar = VB6.GetItemData(ComboOptVorgabe, ComboOptVorgabe.SelectedIndex)
        _settings.isDnVektor = CheckisDnVektor.Checked
        If (Val(TextInteract.Text) <= 0) Then
            _settings.isInteract = False
            _settings.interact = 1
        Else
            _settings.isInteract = True
            _settings.interact = Val(TextInteract.Text)
        End If
        _settings.NMemberSecondPop = Val(TextNMemberSecondPop.Text)

        'Flag setzen
        Me.isSaved = True

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

    Public ReadOnly Property PES_Settings() As EVO.Kern.PES.Struct_Settings
        Get
            'Wenn Einstellungen noch nicht gespeichert, zuerst speichern
            If (Not Me.isSaved) Then
                Call saveSettings()
            End If
            PES_Settings = Me._settings
        End Get
    End Property

#End Region 'Schnittstelle

End Class