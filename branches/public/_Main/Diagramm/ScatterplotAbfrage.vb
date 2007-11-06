Public Class ScatterplotAbfrage

    'Überprüfung der Benutzereingabe
    '*******************************
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        'Hauptdiagramm-Einstellungen
        If (Me.CheckBox_Hauptdiagramm.Checked) Then
            'Mindestens eine X- und Y-Achse ausgewählt?
            If (Me.ListBox_OptZieleX.SelectedIndex = -1 Or Me.ListBox_OptZieleY.SelectedIndex = -1) Then
                MsgBox("Bitte mindestens eine X- und eine Y-Achse auswählen!", MsgBoxStyle.Exclamation, "Fehler")
                Me.DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            End If
            'Unterschiedliche Achsen ausgewählt?
            If (Me.ListBox_OptZieleX.SelectedIndex = Me.ListBox_OptZieleY.SelectedIndex Or _
                (Not Me.ListBox_OptZieleZ.SelectedIndex = -1 And _
                    (Me.ListBox_OptZieleX.SelectedIndex = Me.ListBox_OptZieleZ.SelectedIndex Or _
                    Me.ListBox_OptZieleY.SelectedIndex = Me.ListBox_OptZieleZ.SelectedIndex))) Then
                MsgBox("Achsen müssen unterschiedlich sein!", MsgBoxStyle.Exclamation, "Fehler")
                Me.DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            End If

        ElseIf (Not Me.CheckBox_Scatterplot.Checked) Then
            MsgBox("Bitte entweder 'Hauptdiagramm' oder 'Scatterplot auswählen!", MsgBoxStyle.Exclamation, "Fehler")
            Me.DialogResult = Windows.Forms.DialogResult.None
            Exit Sub
        End If

    End Sub
End Class