Imports System.Windows.Forms

Public Class SensiPlot
    Public Selected_OptParameter As String
    Public Selected_OptZiel As String
    Public Selected_SensiType As String
    Public Anz_Sim As Integer

    Public Function ListBox_OptParameter_add(ByVal Bezeichnung As String) As Boolean
        ListBox_OptParameter.Items.Add(Bezeichnung)
    End Function

    Public Function ListBox_OptZiele_add(ByVal Bezeichnung As String) As Boolean
        ListBox_OptZiele.Items.Add(Bezeichnung)
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Selected_OptParameter = ListBox_OptParameter.SelectedItem
        Selected_OptZiel = ListBox_OptZiele.SelectedItem

        If RadioButton_Gleichverteilt.Checked And Not RadioButton_Diskret.Checked Then
            Selected_SensiType = "Gleichverteilt"
        ElseIf Not RadioButton_Gleichverteilt.Checked And RadioButton_Diskret.Checked Then
            Selected_SensiType = "Diskret"
        Else
            MsgBox("Gleichverteilt und Diskret ... das willst du nicht", MsgBoxStyle.Exclamation, "Fehler")
        End If
        Anz_Sim = TextBox_AnzSim.Text
        ListBox_OptParameter.Items.Clear()
        ListBox_OptZiele.Items.Clear()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        ListBox_OptParameter.Items.Clear()
        ListBox_OptZiele.Items.Clear()
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class
