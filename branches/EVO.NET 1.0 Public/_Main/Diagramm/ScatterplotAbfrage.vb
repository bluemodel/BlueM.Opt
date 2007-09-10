Public Class ScatterplotAbfrage

    '�berpr�fung der Benutzereingabe
    '*******************************
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        'Hauptdiagramm-Einstellungen
        If (Me.CheckBox_Hauptdiagramm.Checked) Then
            'Jeweils eine X- und Y-Achse ausgew�hlt?
            If (Me.ListBox_OptZieleX.SelectedIndex = -1 Or Me.ListBox_OptZieleY.SelectedIndex = -1) Then
                MsgBox("Bitte jeweils eine X- und eine Y-Achse ausw�hlen!", MsgBoxStyle.Exclamation, "Fehler")
                Me.DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            End If
            'Unterschiedliche Achsen ausgew�hlt?
            If (Me.ListBox_OptZieleX.SelectedIndex = Me.ListBox_OptZieleY.SelectedIndex) Then
                MsgBox("X- und Y-Achse m�ssen unterschiedlich sein!", MsgBoxStyle.Exclamation, "Fehler")
                Me.DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            End If
        End If

    End Sub
End Class