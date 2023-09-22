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
Imports System.Windows.Forms

Public Class ScatterplotDialog

    Private mProblem As BlueM.Opt.Common.Problem
    Private isInitializing As Boolean
    Private RefResultExists As Boolean

    Public ReadOnly Property selectedSpace() As BlueM.Opt.Common.SPACE
        Get
            If (Me.RadioButton_SolutionSpace.Checked) Then
                Return Common.SPACE.SolutionSpace
            Else
                Return Common.SPACE.DecisionSpace
            End If
        End Get
    End Property

    Public ReadOnly Property selectedVariables() As Integer()
        Get
            Dim selection(-1) As Integer
            For Each indexChecked As Integer In Me.CheckedListBox_Auswahl.CheckedIndices
                ReDim Preserve selection(selection.GetUpperBound(0) + 1)
                selection(selection.GetUpperBound(0)) = indexChecked
            Next
            Return selection
        End Get
    End Property

    Public ReadOnly Property ShowSekPopOnly() As Boolean
        Get
            Return Me.CheckBox_SekPopOnly.Checked
        End Get
    End Property

    Public ReadOnly Property ShowStartValue() As Boolean
        Get
            Return Me.CheckBox_showStartValue.Checked
        End Get
    End Property

    Public ReadOnly Property ShowIstWerte() As Boolean
        Get
            Return Me.CheckBox_showIstWerte.Checked
        End Get
    End Property

    Public ReadOnly Property ShowRefResult() As Boolean
        Get
            If (Me.CheckBox_showRef.Enabled) Then
                Return Me.CheckBox_showRef.Checked
            Else
                Return False
            End If
        End Get
    End Property

    Public Sub New(ByRef prob As BlueM.Opt.Common.Problem, Optional ByVal _refResultExists As Boolean = False)

        ' This call is required by the Windows Form Designer.
        Me.isInitializing = True
        InitializeComponent()
        Me.isInitializing = False

        ' Add any initialization after the InitializeComponent() call.

        'Problem speichern
        Me.mProblem = prob

        'Optionen speichern
        Me.RefResultExists = _refResultExists

    End Sub

    Private Sub ScatterplotDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Option für Referenz-Ergebnis ggf. ausblenden
        If (Not Me.RefResultExists) Then
            Me.CheckBox_showRef.Enabled = False
        End If

        'Deactive SekPop only option for SensiPlot
        If Me.mProblem.Method = Common.METH_SENSIPLOT Then
            Me.CheckBox_SekPopOnly.Checked = False
            Me.CheckBox_SekPopOnly.Enabled = False
        End If

        'Default-Auswahl setzen
        Me.RadioButton_SolutionSpace.Checked = True

    End Sub

    Private Sub LinkLabel_CheckAll_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel_CheckAll.LinkClicked

        Dim i As Integer

        'Alle Variablen auswählen
        For i = 0 To Me.CheckedListBox_Auswahl.Items.Count - 1
            Me.CheckedListBox_Auswahl.SetItemCheckState(i, CheckState.Checked)
        Next

    End Sub

    Private Sub selectedSpace_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_SolutionSpace.CheckedChanged, RadioButton_DecisionSpace.CheckedChanged

        If (Me.isInitializing) Then
            Exit Sub
        End If

        Dim bezeichnung As String

        Call Me.CheckedListBox_Auswahl.Items.Clear()

        Select Case Me.selectedSpace

            Case Common.SPACE.SolutionSpace
                'Solution Space
                For Each feature As Common.ObjectiveFunction In Me.mProblem.List_ObjectiveFunctions
                    bezeichnung = feature.Description
                    'Penalty-Functions mit Sternchen markieren
                    If (feature.isPrimObjective) Then bezeichnung &= " (*)"
                    Me.CheckedListBox_Auswahl.Items.Add(bezeichnung)
                Next

            Case Common.SPACE.DecisionSpace
                'Decision Space
                For Each param As Common.OptParameter In Me.mProblem.List_OptParameter
                    bezeichnung = param.Bezeichnung
                    Me.CheckedListBox_Auswahl.Items.Add(bezeichnung)
                Next

        End Select

    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_OK.Click
        'Mindestens 2 Variablen müssen ausgewählt sein
        If (Me.CheckedListBox_Auswahl.CheckedIndices.Count < 2) Then
            MsgBox("Bitte mindestens 2 Variablen auswählen!", MsgBoxStyle.Exclamation)
            Me.DialogResult = Windows.Forms.DialogResult.None
            Exit Sub
        End If
    End Sub

End Class
