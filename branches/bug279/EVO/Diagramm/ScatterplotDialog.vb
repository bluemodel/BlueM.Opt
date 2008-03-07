Imports System.Windows.Forms

Public Class ScatterplotDialog

    Private Sub ScatterplotDialog_Load( ByVal sender As System.Object,  ByVal e As System.EventArgs) Handles MyBase.Load

        'Listbox f�llen
        Dim bezeichnung As String
        For Each ziel As Common.Ziel In Common.Manager.List_Ziele
            bezeichnung = ziel.Bezeichnung
            'OptZiele mit Sternchen markieren
            If (ziel.isOpt) Then bezeichnung &= " (*)"
            Me.CheckedListBox_Ziele.Items.Add(bezeichnung)
        Next

    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        'Mindestens 2 Ziele m�ssen ausgew�hlt sein
        If (Me.CheckedListBox_Ziele.CheckedIndices.Count < 2) Then
            MsgBox("Bitte mindestens 2 Ziele ausw�hlen!", MsgBoxStyle.Exclamation, "Fehler")
            Me.DialogResult = Windows.Forms.DialogResult.None
            Exit Sub
        End If
    End Sub

End Class
