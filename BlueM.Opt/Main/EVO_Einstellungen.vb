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
Imports BlueM.Opt.Common.Constants

Public Class EVO_Einstellungen
    Inherits Windows.Forms.UserControl

#Region "Eigenschaften"

    Private mSettings As BlueM.Opt.Common.Settings
    Private mProblem As BlueM.Opt.Common.Problem           'Das Problem
    Private isInitializing As Boolean

#End Region

#Region "Methoden"

    'Konstruktor
    '***********
    Public Sub New()

        Me.isInitializing = True

        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        Call Me.InitializeComponent()

        'Comboboxen initialisieren
        '-------------------------
        'PES:
        Me.PES_Combo_Strategie.DataSource = System.Enum.GetValues(GetType(EVO_STRATEGY))
        Me.PES_Combo_Startparameter.DataSource = System.Enum.GetValues(GetType(EVO_STARTPARAMETERS))
        Me.PES_Combo_DnMutation.DataSource = System.Enum.GetValues(GetType(PES_MUTATIONSOP))
        Me.PES_Combo_OptEltern.DataSource = System.Enum.GetValues(GetType(PES_REPRODOP))
        Me.PES_Combo_PopEltern.DataSource = System.Enum.GetValues(GetType(EVO_POP_ELTERN))
        Me.PES_Combo_PopStrategie.DataSource = System.Enum.GetValues(GetType(EVO_STRATEGY))

        'TSP
        Me.TSP_ComboBox_prob_instance.DataSource = System.Enum.GetValues(GetType(EnProblem))
        Me.TSP_ComboBox_Reproductionoperator.DataSource = System.Enum.GetValues(GetType(EnReprodOperator))
        Me.TSP_ComboBox_Mutationoperator.DataSource = System.Enum.GetValues(GetType(EnMutOperator))

        'Listboxen von SensiPlot werden erst bei setProblem() gefüllt!

        Me.isInitializing = False

    End Sub

    ''' <summary>
    ''' Setzt das Problem zurück
    ''' </summary>
    Public Sub Reset()
        Me.mProblem = Nothing
    End Sub

    ''' <summary>
    ''' Setzt die Settings und aktiviert die entsprechenden TabPages
    ''' </summary>
    ''' <param name="settings">Settings</param>
    Public Sub setSettings(ByRef settings As Common.Settings)

        Me.mSettings = settings

        Call Me.initTabPages()

    End Sub

    ''' <summary>
    ''' Aktualisiert die Steuerelemente mit den eingestellten Settings
    ''' </summary>
    Public Sub refreshForm()

        'Bindings aktualisieren

        'General
        '-------
        Me.BindingSource_General.Clear()
        Me.BindingSource_General.Add(Me.mSettings.General)

        'PES
        '---
        If (Not IsNothing(Me.mSettings.PES)) Then
            Me.BindingSource_PES.Clear()
            Me.BindingSource_PES.Add(Me.mSettings.PES)

            Me.BindingSource_PES_Schrittweite.Clear()
            Me.BindingSource_PES_Schrittweite.Add(Me.mSettings.PES.SetMutation)

            Me.BindingSource_PES_SekPop.Clear()
            Me.BindingSource_PES_SekPop.Add(Me.mSettings.PES.SekPop)

            Me.BindingSource_PES_Pop.Clear()
            Me.BindingSource_PES_Pop.Add(Me.mSettings.PES.Pop)
        End If

        'HookeJeeves
        '-----------
        If (Not IsNothing(Me.mSettings.HookeJeeves)) Then
            Me.BindingSource_HookeJeeves.Clear()
            Me.BindingSource_HookeJeeves.Add(Me.mSettings.HookeJeeves)
        End If

        'MetaEvo
        '-------
        If (Not IsNothing(Me.mSettings.MetaEvo)) Then
            Me.BindingSource_MetaEvo.Clear()
            Me.BindingSource_MetaEvo.Add(Me.mSettings.MetaEvo)
        End If

        'DDS
        '---
        If (Not IsNothing(Me.mSettings.DDS)) Then
            Me.BindingSource_DDS.Clear()
            Me.BindingSource_DDS.Add(Me.mSettings.DDS)
        End If

        'SensiPlot
        If (Not IsNothing(Me.mSettings.SensiPlot)) Then
            Me.BindingSource_Sensiplot.Clear()
            Me.BindingSource_Sensiplot.Add(Me.mSettings.SensiPlot)
        End If

        'TSP
        If (Not IsNothing(Me.mSettings.TSP)) Then
            Me.BindingSource_TSP.Clear()
            Me.BindingSource_TSP.Add(Me.mSettings.TSP)
        End If

    End Sub

    ''' <summary>
    ''' Deaktiviert alle angezeigten TabPages
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub freeze()
        For Each page As TabPage In Me.TabControl1.TabPages
            page.Enabled = False
        Next
    End Sub

    ''' <summary>
    ''' Setzt das Problem, 
    ''' instanziert die erforderlichen Settings mit ihren Standard-Settings, 
    ''' und zeigt die entsprechenden TabPages an
    ''' </summary>
    ''' <param name="prob">Das Problem</param>
    Public Sub setProblem(ByRef prob As BlueM.Opt.Common.Problem)

        'Problem speichern
        Me.mProblem = prob

        'Je nach Methode initialisieren
        Select Case Me.mProblem.Method

            Case METH_PES
                'PES-Settings instanzieren
                Me.mSettings.PES = New Common.Settings_PES()
                Me.mSettings.PES.setStandard(Me.mProblem.Modus)

            Case METH_HOOKEJEEVES
                'HJ-Settings instanzieren
                Me.mSettings.HookeJeeves = New Common.Settings_HookeJeeves()
                Me.mSettings.HookeJeeves.setStandard()

            Case METH_DDS
                'DDS-Settings initialisieren
                Me.mSettings.DDS = New Common.Settings_DDS()
                Me.mSettings.DDS.setStandard()

            Case METH_METAEVO
                'MetaEvo-Settings instanzieren
                Me.mSettings.MetaEvo = New Common.Settings_MetaEvo()
                Me.mSettings.MetaEvo.setStandard()

            Case METH_SENSIPLOT
                'Sensiplot-Settings instanzieren
                Me.mSettings.SensiPlot = New Common.Settings_Sensiplot()
                Me.mSettings.SensiPlot.setStandard()

                'Listboxen füllen
                Me.isInitializing = True
                Me.SensiPlot_ListBox_OptParameter.Items.Clear()
                For Each optpara As Common.OptParameter In Me.mProblem.List_OptParameter
                    Call Me.SensiPlot_ListBox_OptParameter.Items.Add(optpara)
                Next
                Me.SensiPlot_ListBox_Objectives.Items.Clear()
                For Each objective As Common.ObjectiveFunction In Me.mProblem.List_ObjectiveFunctions
                    Call Me.SensiPlot_ListBox_Objectives.Items.Add(objective)
                Next
                Me.isInitializing = False
                'Standardmäßig ersten OptParameter und erste ObjectiveFunction auswählen
                Me.SensiPlot_ListBox_OptParameter.SetSelected(0, True)
                Me.SensiPlot_ListBox_Objectives.SetSelected(0, True)

            Case METH_TSP
                'TSP-Settings instanzieren
                Me.mSettings.TSP = New Common.Settings_TSP()
                Me.mSettings.TSP.setStandard()

            Case Else
                Throw New Exception($"Unknown method '{Me.mProblem.Method}' in EVO_Einstellungen.setProblem()!")

        End Select

        Call Me.initTabPages()

    End Sub

    ''' <summary>
    ''' TabPages je nach Problem anzeigen/verstecken
    ''' </summary>
    Private Sub initTabPages()

        'Alle TabPages entfernen
        Call Me.TabControl1.TabPages.Clear()

        'TabPage "General" immer aktivieren
        Me.TabPage_General.Enabled = True
        Me.TabControl1.TabPages.Add(Me.TabPage_General)

        'Wenn Problem bereits bekannt:
        If (Not IsNothing(Me.mProblem)) Then

            'Je nach Methode TabPages anzeigen
            Select Case Me.mProblem.Method

                Case METH_PES
                    Me.TabControl1.TabPages.Add(Me.TabPage_PES)

                Case METH_HOOKEJEEVES
                    Me.TabControl1.TabPages.Add(Me.TabPage_HookeJeeves)

                Case METH_DDS
                    Me.TabControl1.TabPages.Add(Me.TabPage_DDS)

                Case METH_METAEVO
                    Me.TabControl1.TabPages.Add(Me.TabPage_MetaEvo)

                Case METH_SENSIPLOT
                    Me.TabControl1.TabPages.Add(Me.TabPage_SensiPlot)

                Case METH_TSP
                    Me.TabControl1.TabPages.Add(Me.TabPage_TSP)

                Case Else
                    Throw New Exception($"Unknown method '{Me.mProblem.Method}' in EVO_Einstellungen.initTabPages()!")

            End Select

        End If

        'Werte ins Form schreiben
        Call Me.refreshForm()

        'Alle TabPages aktivieren
        For Each page As TabPage In Me.TabControl1.TabPages
            page.Enabled = True
        Next

        'Letztes TabPage nach vorne holen
        Call Me.TabControl1.SelectTab(Me.TabControl1.TabPages.Count - 1)

    End Sub

#Region "Events"

    Private Sub SensiPlot_Validate() Handles _
        SensiPlot_ListBox_OptParameter.SelectedIndexChanged,
        SensiPlot_ListBox_Objectives.SelectedIndexChanged,
        SensiPlot_RadioButton_ModeEvenDistribution.CheckedChanged,
        SensiPlot_RadioButton_ModeRandom.CheckedChanged

        If (Me.isInitializing) Then Exit Sub

        Dim i As Integer

        'Mindestens 1 OptParameter
        If (Me.SensiPlot_ListBox_OptParameter.SelectedIndices.Count = 0) Then
            MsgBox("Please select at least one optimization parameter!", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        Me.SensiPlot_RadioButton_ModeEvenDistribution.DataBindings("Checked").WriteValue()
        Me.SensiPlot_RadioButton_ModeRandom.DataBindings("Checked").WriteValue()
        Me.SensiPlot_RadioButton_ModeLatinHypercube.DataBindings("Checked").WriteValue()

        'SelectedOptParameters einlesen
        Me.mSettings.SensiPlot.Selected_OptParameters.Clear()
        For i = 0 To Me.SensiPlot_ListBox_OptParameter.SelectedIndices.Count - 1
            Me.mSettings.SensiPlot.Selected_OptParameters.Add(Me.SensiPlot_ListBox_OptParameter.SelectedIndices(i))
        Next

        'SelectedObjective einlesen
        Me.mSettings.SensiPlot.Selected_Objective = Me.SensiPlot_ListBox_Objectives.SelectedIndex

        Call Me.refreshForm()

    End Sub

    ''' <summary>
    ''' Ensure that control value changes are saved to the data binding immediately
    ''' TODO: Why is this even necessary? And if it is, what is the point of binding?
    '''       It seems to be only necessary for comboboxes and for when the NumericUpDown_NThreads is changed by typing a number into it
    '''       Other NumericUpDowns seem to work as expected...
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ControlValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _
        NumericUpDown_NThreads.ValueChanged,
        PES_Combo_OptEltern.SelectedIndexChanged,
        PES_Combo_DnMutation.SelectedIndexChanged,
        PES_Combo_Strategie.SelectedIndexChanged,
        PES_Combo_Startparameter.SelectedIndexChanged,
        PES_Combo_PopEltern.SelectedIndexChanged,
        PES_Combo_PopStrategie.SelectedIndexChanged,
        PES_Combo_PopPenalty.SelectedIndexChanged,
        MetaEvo_Combo_Role.SelectedIndexChanged,
        MetaEvo_Combo_OpMode.SelectedIndexChanged,
        TSP_ComboBox_prob_instance.SelectedIndexChanged,
        TSP_ComboBox_Reproductionoperator.SelectedIndexChanged,
        TSP_ComboBox_Mutationoperator.SelectedIndexChanged

        CType(sender, Control).DataBindings(0).WriteValue()
    End Sub

    ''' <summary>
    ''' Enables/disables the inputs for the number of threads when the "use multithreading" option changes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CheckBox_useMultithreading_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_useMultithreading.CheckedChanged
        If CheckBox_useMultithreading.Checked Then
            Label_NThreads.Enabled = True
            NumericUpDown_NThreads.Enabled = True
        Else
            Label_NThreads.Enabled = False
            NumericUpDown_NThreads.Enabled = False
        End If
    End Sub

#End Region 'Events

#End Region 'Methoden

End Class
