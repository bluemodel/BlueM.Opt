Imports System.Windows.Forms

Public Class ScatterplotDialog

    Private Sub ScatterplotDialog_Load( ByVal sender As System.Object,  ByVal e As System.EventArgs) Handles MyBase.Load

        'Listbox füllen
        Dim bezeichnung As String
        For Each feature As Common.Featurefunction In Common.Manager.List_Featurefunctions
            bezeichnung = feature.Bezeichnung
            'Penalty-Functions mit Sternchen markieren
            If (feature.isPenalty) Then bezeichnung &= " (*)"
            Me.CheckedListBox_Ziele.Items.Add(bezeichnung)
        Next

    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_OK.Click
        'Mindestens 2 Ziele müssen ausgewählt sein
        If (Me.CheckedListBox_Ziele.CheckedIndices.Count < 2) Then
            MsgBox("Bitte mindestens 2 Ziele auswählen!", MsgBoxStyle.Exclamation)
            Me.DialogResult = Windows.Forms.DialogResult.None
            Exit Sub
        End If
    End Sub

    Private Sub LinkLabel_CheckAll_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel_CheckAll.LinkClicked

        Dim i As Integer

        'Alle Ziele auswählen
        For i = 0 To Me.CheckedListBox_Ziele.Items.Count - 1
            Me.CheckedListBox_Ziele.SetItemCheckState(i, CheckState.Checked)
        Next

    End Sub
End Class
