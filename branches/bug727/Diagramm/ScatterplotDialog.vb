' Copyright (c) BlueM Dev Group
' Website: http://bluemodel.org
' 
' All rights reserved.
' 
' Released under the BSD-2-Clause License:
' 
' Redistribution and use in source and binary forms, with or without modification, 
' are permitted provided that the following conditions are met:
' 
' * Redistributions of source code must retain the above copyright notice, this list 
'   of conditions and the following disclaimer.
' * Redistributions in binary form must reproduce the above copyright notice, this list 
'   of conditions and the following disclaimer in the documentation and/or other materials 
'   provided with the distribution.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
' EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
' OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
' SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT 
' OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
' HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
' TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
' EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'--------------------------------------------------------------------------------------------
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
                    bezeichnung = feature.Bezeichnung
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
