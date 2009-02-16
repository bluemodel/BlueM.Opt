Imports System.Windows.Forms

Public Class ScatterplotDialog

    Private mProblem As EVO.Common.Problem
    Private isInitializing As Boolean
    Private RefResultExists As Boolean

    Public ReadOnly Property selectedSpace() As EVO.Common.SPACE
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

    Public ReadOnly Property ShowRefResult() As Boolean
        Get
            If (Me.CheckBox_showRef.Enabled) Then
                Return Me.CheckBox_showRef.Checked
            Else
                Return False
            End If
        End Get
    End Property

    Public Sub New(ByRef prob As EVO.Common.Problem, ByVal _RefResultExists As Boolean)

        ' This call is required by the Windows Form Designer.
        Me.isInitializing = True
        InitializeComponent()
        Me.isInitializing = False

        ' Add any initialization after the InitializeComponent() call.

        'Problem speichern
        Me.mProblem = prob

        Me.RefResultExists = _RefResultExists

    End Sub

    Private Sub ScatterplotDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Option f�r Referenz-Ergebnis ggf. ausblenden
        If (Not Me.RefResultExists) Then
            Me.GroupBox_Ref.Enabled = False
        End If

        'Default-Auswahl setzen
        Me.RadioButton_SolutionSpace.Checked = True

    End Sub

    Private Sub LinkLabel_CheckAll_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel_CheckAll.LinkClicked

        Dim i As Integer

        'Alle Variablen ausw�hlen
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
                For Each feature As Common.Objectivefunktion In Me.mProblem.List_ObjectiveFunctions
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
        'Mindestens 2 Variablen m�ssen ausgew�hlt sein
        If (Me.CheckedListBox_Auswahl.CheckedIndices.Count < 2) Then
            MsgBox("Bitte mindestens 2 Variablen ausw�hlen!", MsgBoxStyle.Exclamation)
            Me.DialogResult = Windows.Forms.DialogResult.None
            Exit Sub
        End If
    End Sub

End Class
