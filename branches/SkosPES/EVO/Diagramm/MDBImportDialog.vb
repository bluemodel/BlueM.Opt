Public Class MDBImportDialog

    'Form load
    '*********
    Private Sub MDBImportDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Listboxen füllen
        Dim bezeichnung As String
        For Each ziel As Common.Ziel In Common.Manager.List_Ziele
            bezeichnung = ziel.Bezeichnung
            'OptZiele mit Sternchen markieren
            If (ziel.isOpt) Then bezeichnung &= " (*)"
            Me.ListBox_ZieleX.Items.Add(bezeichnung)
            Me.ListBox_ZieleY.Items.Add(bezeichnung)
            Me.ListBox_ZieleZ.Items.Add(bezeichnung)
        Next

        'Bei weniger als 3 Zielen Z-Achse ausblenden
        If (Common.Manager.AnzZiele < 3) Then
            Me.ListBox_ZieleZ.Enabled = False
        End If
        'Bei weniger als 2 Zielen Y-Achse und SekPop-Optionen ausblenden
        If (Common.Manager.AnzZiele < 2) Then
            Me.ListBox_ZieleY.Enabled = False
            Me.GroupBox_SekPop.Enabled = False
        End If

        'SekPop Combobox
        If (Common.Manager.AnzPenalty < 2) Then
            Me.ComboBox_SekPop.SelectedIndex = 1
        Else
            Me.ComboBox_SekPop.SelectedIndex = 0
        End If
    End Sub

    'Überprüfung der Benutzereingabe
    '*******************************
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If (Me.ListBox_ZieleY.Enabled) Then
            'Mindestens eine X- und Y-Achse ausgewählt?
            If (Me.ListBox_ZieleX.SelectedIndex = -1 Or Me.ListBox_ZieleY.SelectedIndex = -1) Then
                MsgBox("Bitte mindestens eine X- und eine Y-Achse auswählen!", MsgBoxStyle.Exclamation, "Fehler")
                Me.DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            End If
        End If

        'Unterschiedliche Achsen ausgewählt?
        If (Me.ListBox_ZieleY.Enabled) Then
            If (Me.ListBox_ZieleX.SelectedIndex = Me.ListBox_ZieleY.SelectedIndex Or _
                (Not Me.ListBox_ZieleZ.SelectedIndex = -1 And _
                    (Me.ListBox_ZieleX.SelectedIndex = Me.ListBox_ZieleZ.SelectedIndex Or _
                    Me.ListBox_ZieleY.SelectedIndex = Me.ListBox_ZieleZ.SelectedIndex))) Then
                MsgBox("Achsen müssen unterschiedlich sein!", MsgBoxStyle.Exclamation, "Fehler")
                Me.DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            End If
        End If
    End Sub

End Class