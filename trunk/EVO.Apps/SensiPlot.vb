Partial Public Class SensiPlot
    Inherits System.Windows.Forms.Form

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse SensiPlot                                                      ****
    '****                                                                       ****
    '**** Einstellungsdialog für SensiPlot                                      ****
    '**** Speichert SensiPlot-Einstellungen                                     ****
    '****                                                                       ****
    '**** Autoren: Christoph Hübner, Felix Froehlich                            ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

    Public Selected_OptParameter() As Integer
    Public Selected_Penaltyfunction As Integer
    Public Selected_SensiType As String
    Public Anz_Steps As Integer
    Public show_Wave As Boolean

    Public Sub ListBox_OptParameter_add(ByVal OptParameter As EVO.Common.OptParameter)
        ListBox_OptParameter.Items.Add(OptParameter)
    End Sub

    Public Sub ListBox_OptZiele_add(ByVal penaltyfunction As Common.Featurefunction)
        ListBox_Penaltyfunctions.Items.Add(penaltyfunction)
    End Sub

    'Überprüfung und Anwendung der Einstellungen
    '*******************************************
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        'OptParameter und OptZiel
        If (Me.ListBox_OptParameter.SelectedIndex = -1 _
                Or Me.ListBox_OptParameter.SelectedIndices.Count > 2 _
                Or Me.ListBox_Penaltyfunctions.SelectedIndex = -1) Then
            MsgBox("Bitte einen oder zwei OptParameter und ein OptZiel auswählen!", MsgBoxStyle.Exclamation, "Fehler")
            Me.DialogResult = Windows.Forms.DialogResult.None
            Exit Sub
        End If
        'bei 2 OptParametern geht nur diskret!
        If (Me.ListBox_OptParameter.SelectedIndices.Count > 1 And Me.RadioButton_Gleichverteilt.Checked) Then
            MsgBox("Bei mehr als einem OptParameter muss 'Diskret' als Modus ausgewählt sein!", MsgBoxStyle.Exclamation, "Fehler")
            Me.DialogResult = Windows.Forms.DialogResult.None
            Exit Sub
        End If
        'OptParameter übergeben
        ReDim Me.Selected_OptParameter(Me.ListBox_OptParameter.SelectedIndices.Count - 1)
        For i As Integer = 0 To Me.ListBox_OptParameter.SelectedIndices.Count - 1
            Me.Selected_OptParameter(i) = Me.ListBox_OptParameter.SelectedIndices(i)
        Next
        'OptZiel übergeben
        Me.Selected_Penaltyfunction = Me.ListBox_Penaltyfunctions.SelectedIndex

        'Modus
        If (Me.RadioButton_Gleichverteilt.Checked) Then
            Me.Selected_SensiType = Me.RadioButton_Gleichverteilt.Text
        ElseIf (Me.RadioButton_Diskret.Checked) Then
            Me.Selected_SensiType = Me.RadioButton_Diskret.Text
        Else
            MsgBox("Bitte einen Modus (Diskret / Gleichverteilt) auswählen!", MsgBoxStyle.Exclamation, "Fehler")
            Me.DialogResult = Windows.Forms.DialogResult.None
            Exit Sub
        End If

        'Anzahl Schritte
        Me.TextBox_AnzSim.ValidatingType = GetType(System.Int32)
        If (Me.TextBox_AnzSim.ValidateText() = Nothing) Then
            MsgBox("Bitte eine Zahl > 0 für die Anzahl der Simulationen eingeben!", MsgBoxStyle.Exclamation, "Fehler")
            Me.TextBox_AnzSim.Focus()
            Me.DialogResult = Windows.Forms.DialogResult.None
            Exit Sub
        Else
            Me.Anz_Steps = TextBox_AnzSim.Text
            If (Me.Anz_Steps < 1) Then
                MsgBox("Mindestens 1 Simulation erforderlich!", MsgBoxStyle.Exclamation, "Fehler")
                Me.TextBox_AnzSim.Focus()
                Me.DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            End If
        End If

        'show Wave
        Me.show_Wave = Me.CheckBox_wave.Checked

        'Überprüfung erfolgreich 
        Me.ListBox_OptParameter.Items.Clear()
        Me.ListBox_Penaltyfunctions.Items.Clear()

    End Sub

End Class
