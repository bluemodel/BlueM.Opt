Imports System.Windows.Forms

Public Class SensiPlot
    Public Selected_OptParameter As String
    Public Selected_OptZiel As String
    Public Selected_SensiType As String
    Public Anz_Sim As Integer

    Public Sub ListBox_OptParameter_add(ByVal Bezeichnung As String)
        ListBox_OptParameter.Items.Add(Bezeichnung)
    End Sub

    Public Sub ListBox_OptZiele_add(ByVal Bezeichnung As String)
        ListBox_OptZiele.Items.Add(Bezeichnung)
    End Sub

    'Überprüfung der Einstellungen
    '*****************************
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        'Selectboxen
        Me.Selected_OptParameter = Me.ListBox_OptParameter.SelectedItem
        Me.Selected_OptZiel = Me.ListBox_OptZiele.SelectedItem

        If (Me.Selected_OptParameter = "" Or Me.Selected_OptZiel = "") Then
            MsgBox("Bitte jeweils einen OptParameter und ein OptZiel auswählen!", MsgBoxStyle.Exclamation, "Fehler")
            Me.DialogResult = Windows.Forms.DialogResult.None
            Exit Sub
        End If

        'Anzahl Simulationen
        Me.TextBox_AnzSim.ValidatingType = GetType(System.Int32)
        If (Me.TextBox_AnzSim.ValidateText() = Nothing) Then
            MsgBox("Bitte eine Zahl > 0 für die Anzahl der Simulationen eingeben!", MsgBoxStyle.Exclamation, "Fehler")
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

        'Überprüfung erfolgreich 
        Me.ListBox_OptParameter.Items.Clear()
        Me.ListBox_OptZiele.Items.Clear()

    End Sub

End Class
