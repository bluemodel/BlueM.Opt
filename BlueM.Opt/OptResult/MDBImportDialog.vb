'BlueM.Opt
'Copyright (C) BlueM Dev Group
'Website: <https://www.bluemodel.org>
'
'This program is free software: you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.
'
'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
'GNU General Public License for more details.
'
'You should have received a copy of the GNU General Public License
'along with this program. If not, see <https://www.gnu.org/licenses/>.
'
Public Class MDBImportDialog

    Private mProblem As BlueM.Opt.Common.Problem

    Public Sub New(ByRef prob As BlueM.Opt.Common.Problem)

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
        For Each feature As Common.ObjectiveFunction In Me.mProblem.List_ObjectiveFunctions
            bezeichnung = feature.Bezeichnung
            'Penalty-Funktionen mit Sternchen markieren
            If (feature.isPrimObjective) Then bezeichnung &= " (*)"
            Me.ListBox_ZieleX.Items.Add(bezeichnung)
            Me.ListBox_ZieleY.Items.Add(bezeichnung)
            Me.ListBox_ZieleZ.Items.Add(bezeichnung)
        Next

        'Bei weniger als 3 Zielen Z-Achse ausblenden
        If (Me.mProblem.NumObjectives < 3) Then
            Me.ListBox_ZieleZ.Enabled = False
        End If
        'Bei weniger als 2 Zielen Y-Achse und SekPop-Optionen ausblenden
        If (Me.mProblem.NumObjectives < 2) Then
            Me.ListBox_ZieleY.Enabled = False
            Me.GroupBox_SekPop.Enabled = False
        End If

        'SekPop Combobox
        If (Me.mProblem.NumPrimObjective < 2) Then
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
                MsgBox("Please select at least one X and Y axis!", MsgBoxStyle.Exclamation)
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
                MsgBox("Please select different axes for each objective function!", MsgBoxStyle.Exclamation)
                Me.DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            End If
        End If
    End Sub

End Class