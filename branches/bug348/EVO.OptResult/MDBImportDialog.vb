Public Class MDBImportDialog

    Private mProblem As EVO.Common.Problem

    Public Sub New(ByRef prob As EVO.Common.Problem)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.mProblem = prob

    End Sub

    'Form load
    '*********
    Private Sub MDBImportDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Listboxen füllen
        Dim bezeichnung As String
        For Each feature As Common.Featurefunction In Me.mProblem.List_Featurefunctions
            bezeichnung = feature.Bezeichnung
            'Penalty-Funktionen mit Sternchen markieren
            If (feature.isPenalty) Then bezeichnung &= " (*)"
            Me.ListBox_ZieleX.Items.Add(bezeichnung)
            Me.ListBox_ZieleY.Items.Add(bezeichnung)
            Me.ListBox_ZieleZ.Items.Add(bezeichnung)
        Next

        'Bei weniger als 3 Zielen Z-Achse ausblenden
        If (Me.mProblem.NumFeatures < 3) Then
            Me.ListBox_ZieleZ.Enabled = False
        End If
        'Bei weniger als 2 Zielen Y-Achse und SekPop-Optionen ausblenden
        If (Me.mProblem.NumFeatures < 2) Then
            Me.ListBox_ZieleY.Enabled = False
            Me.GroupBox_SekPop.Enabled = False
        End If

        'SekPop Combobox
        If (Me.mProblem.NumPenalties < 2) Then
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