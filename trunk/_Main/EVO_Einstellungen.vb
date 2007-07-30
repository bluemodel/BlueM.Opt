Option Strict Off
Option Explicit On
Public Class EVO_Einstellungen
    Inherits System.Windows.Forms.UserControl

    Private OptModusValue As Short = EVO_MODUS_SINGEL_OBJECTIVE
    Dim isParetoOptimierung, isMultiObjectiveOptimierung As Boolean

    'Optimierungsmodus wurde geändert
    '********************************
    Private Sub OptModus_Change()

        Dim i As Short

        Select Case Me.OptModus

            Case EVO_MODUS_SINGEL_OBJECTIVE
                'Vorgaben und Anzeige
                Label_OptModusValue.Text = "Single Objective"
                TextAnzGen.Text = CStr(20)
                TextAnzEltern.Text = CStr(3)
                TextAnzNachf.Text = CStr(10)
                FramePop.Enabled = False
                TextAnzRunden.Enabled = False
                TextAnzRunden.Text = CStr(10)
                TextAnzPop.Enabled = False
                TextAnzPopEltern.Enabled = False
                ComboStrategie.Enabled = True
                ComboOptPopEltern.Enabled = False
                ComboPopStrategie.Enabled = False
                ComboPopPenalty.Enabled = False
                For i = 1 To 6
                    LabelFramePop(i).Enabled = False
                Next i
                'Setzen der Modusabhängigen Vorgaben
                isParetoOptimierung = False
                isMultiObjectiveOptimierung = False
                CheckisPopul.Enabled = True
                CheckisPopul.CheckState = CheckState.Unchecked
                LabelInteract.Enabled = False
                TextInteract.Enabled = False
                LabelNMemberSecondPop.Enabled = False
                TextNMemberSecondPop.Enabled = False

            Case EVO_MODUS_MULTIOBJECTIVE_PARETO
                'Vorgaben und Anzeige
                Label_OptModusValue.Text = "MultiObjective Pareto"
                TextAnzGen.Text = CStr(250)
                TextAnzEltern.Text = CStr(25)
                TextAnzNachf.Text = CStr(75)
                FramePop.Enabled = False
                TextAnzRunden.Enabled = False
                TextAnzRunden.Text = CStr(10)
                TextAnzPop.Enabled = False
                TextAnzPopEltern.Enabled = False
                ComboStrategie.Text = "'+' (Eltern+Nachfolger)"
                ComboStrategie.Enabled = False
                ComboOptPopEltern.Enabled = False
                ComboPopStrategie.Enabled = False
                ComboPopPenalty.Enabled = False
                For i = 1 To 6
                    LabelFramePop(i).Enabled = False
                Next i
                'Setzen der Modusabhängigen Vorgaben
                isMultiObjectiveOptimierung = True
                isParetoOptimierung = True
                'CheckisPopul.CheckState = CheckState.Unchecked
                CheckisPopul.Enabled = False
                LabelInteract.Enabled = True
                TextInteract.Enabled = True
                LabelNMemberSecondPop.Enabled = True
                TextNMemberSecondPop.Enabled = True

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
        Dim i As Short

        If CheckisPopul.CheckState = System.Windows.Forms.CheckState.Checked Then
            FramePop.Enabled = True
            TextAnzRunden.Enabled = True
            TextAnzPop.Enabled = True
            TextAnzPopEltern.Enabled = True
            ComboOptPopEltern.Enabled = True
            ComboPopStrategie.Enabled = True
            ComboPopPenalty.Enabled = True

            For i = 1 To 6
                LabelFramePop(i).Enabled = True
            Next i
        Else
            FramePop.Enabled = False
            TextAnzRunden.Enabled = False
            TextAnzPop.Enabled = False
            TextAnzPopEltern.Enabled = False
            ComboOptPopEltern.Enabled = False
            ComboPopStrategie.Enabled = False
            ComboPopPenalty.Enabled = False
            For i = 1 To 6
                LabelFramePop(i).Enabled = False
            Next i
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

    '********************************************************************
    'Schnittstelle
    '********************************************************************

    Public ReadOnly Property isDnVektor() As Boolean
        Get
            isDnVektor = False
            If CheckisDnVektor.CheckState = System.Windows.Forms.CheckState.Checked Then isDnVektor = True
        End Get
    End Property

    Public ReadOnly Property globalOPTVORGABE() As Integer
        Get
            globalOPTVORGABE = VB6.GetItemData(ComboOptVorgabe, ComboOptVorgabe.SelectedIndex)
        End Get
    End Property

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

    Public ReadOnly Property rDeltaStart() As Single
        Get
            rDeltaStart = Val(TextDeltaStart.Text)
        End Get
    End Property


    Public ReadOnly Property NNachf() As Integer
        Get
            NNachf = Val(TextAnzNachf.Text)
        End Get
    End Property

    Public ReadOnly Property NEltern() As Integer
        Get
            NEltern = Val(TextAnzEltern.Text)
        End Get
    End Property

    Public ReadOnly Property NGen() As Integer
        Get
            NGen = Val(TextAnzGen.Text)
        End Get
    End Property

    Public ReadOnly Property iOptEltern() As Integer
        Get
            iOptEltern = VB6.GetItemData(ComboOptEltern, ComboOptEltern.SelectedIndex)
        End Get
    End Property

    Public ReadOnly Property iPopPenalty() As Integer
        Get
            iPopPenalty = VB6.GetItemData(ComboPopPenalty, ComboPopPenalty.SelectedIndex)
        End Get
    End Property

    Public ReadOnly Property NRekombXY() As Integer
        Get
            NRekombXY = Val(TextRekombxy.Text)
        End Get
    End Property

    Public ReadOnly Property iOptPopEltern() As Integer
        Get
            iOptPopEltern = VB6.GetItemData(ComboOptPopEltern, ComboOptPopEltern.SelectedIndex)
        End Get
    End Property

    Public ReadOnly Property NPopEltern() As Integer
        Get
            NPopEltern = Val(TextAnzPopEltern.Text)
        End Get
    End Property

    Public ReadOnly Property iEvoTyp() As Integer
        Get
            iEvoTyp = VB6.GetItemData(ComboStrategie, ComboStrategie.SelectedIndex)
        End Get
    End Property

    Public ReadOnly Property iPopEvoTyp() As Integer
        Get
            iPopEvoTyp = VB6.GetItemData(ComboPopStrategie, ComboPopStrategie.SelectedIndex)
        End Get
    End Property

    Public ReadOnly Property isPOPUL() As Boolean
        Get
            isPOPUL = False
            If CheckisPopul.CheckState = System.Windows.Forms.CheckState.Checked Then isPOPUL = True
        End Get
    End Property

    Public ReadOnly Property isMultiObjective() As Boolean
        Get
            isMultiObjective = isMultiObjectiveOptimierung
        End Get
    End Property

    Public ReadOnly Property isPareto() As Boolean
        Get
            isPareto = isParetoOptimierung
        End Get
    End Property

    Public ReadOnly Property NRunden() As Integer
        Get
            NRunden = Val(TextAnzRunden.Text)
        End Get
    End Property

    Public ReadOnly Property NPopul() As Integer
        Get
            NPopul = Val(TextAnzPop.Text)
        End Get
    End Property

    Public ReadOnly Property Interact() As Short
        Get
            If Val(TextInteract.Text) <= 0 Then
                Interact = 1
            Else
                Interact = Val(TextInteract.Text)
            End If
        End Get
    End Property

    Public ReadOnly Property NMemberSecondPop() As Short
        Get
            NMemberSecondPop = Val(TextNMemberSecondPop.Text)
        End Get
    End Property

    Public ReadOnly Property isInteract() As Boolean
        Get
            If Val(TextInteract.Text) <= 0 Then
                isInteract = False
            Else
                isInteract = True
            End If
        End Get
    End Property

End Class