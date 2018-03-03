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