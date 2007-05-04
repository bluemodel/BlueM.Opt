Public Class SensiPlot
    Inherits System.Windows.Forms.Form

    Public Selected_OptParameter As Integer
    Public Selected_OptZiel As Integer
    Public Selected_SensiType As String
    Public Anz_Sim As Integer

    Public Sub ListBox_OptParameter_add(ByVal OptParameter As Sim.OptParameter)
        ListBox_OptParameter.Items.Add(OptParameter)
    End Sub

    Public Sub ListBox_OptZiele_add(ByVal OptZiel As Sim.OptZiel)
        ListBox_OptZiele.Items.Add(OptZiel)
    End Sub

    '�berpr�fung und Anwendung der Einstellungen
    '*******************************************
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        'OptParameter und OptZiel
        If (Me.ListBox_OptParameter.SelectedIndex = -1 Or Me.ListBox_OptZiele.SelectedIndex = -1) Then
            MsgBox("Bitte jeweils einen OptParameter und ein OptZiel ausw�hlen!", MsgBoxStyle.Exclamation, "Fehler")
            Me.DialogResult = Windows.Forms.DialogResult.None
            Exit Sub
        End If
        Me.Selected_OptParameter = Me.ListBox_OptParameter.SelectedIndex
        Me.Selected_OptZiel = Me.ListBox_OptZiele.SelectedIndex

        'Modus
        If (Me.RadioButton_Gleichverteilt.Checked) Then
            Me.Selected_SensiType = Me.RadioButton_Gleichverteilt.Text
        ElseIf (Me.RadioButton_Diskret.Checked) Then
            Me.Selected_SensiType = Me.RadioButton_Diskret.Text
        Else
            MsgBox("Bitte einen Modus (Diskret / Gleichverteilt) ausw�hlen!", MsgBoxStyle.Exclamation, "Fehler")
            Me.DialogResult = Windows.Forms.DialogResult.None
            Exit Sub
        End If

        'Anzahl Simulationen
        Me.TextBox_AnzSim.ValidatingType = GetType(System.Int32)
        If (Me.TextBox_AnzSim.ValidateText() = Nothing) Then
            MsgBox("Bitte eine Zahl > 0 f�r die Anzahl der Simulationen eingeben!", MsgBoxStyle.Exclamation, "Fehler")
            Me.TextBox_AnzSim.Focus()
            Me.DialogResult = Windows.Forms.DialogResult.None
            Exit Sub
        Else
            Me.Anz_Sim = TextBox_AnzSim.Text
            If (Me.Anz_Sim < 1) Then
                MsgBox("Mindestens 1 Simulation erforderlich!", MsgBoxStyle.Exclamation, "Fehler")
                Me.TextBox_AnzSim.Focus()
                Me.DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            End If
        End If

        '�berpr�fung erfolgreich 
        Me.ListBox_OptParameter.Items.Clear()
        Me.ListBox_OptZiele.Items.Clear()

    End Sub

End Class
