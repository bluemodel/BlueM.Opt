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
'*******************************************************************************
'*******************************************************************************
'**** Klasse EVO_Einstellungen                                              ****
'****                                                                       ****
'**** Autoren: Christoph Hübner, Felix Froehlich, Dirk Muschalla            ****
'*******************************************************************************
'*******************************************************************************

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

    Private Sub SensiPlot_Validate() Handles SensiPlot_ListBox_OptParameter.SelectedIndexChanged, SensiPlot_ListBox_Objectives.SelectedIndexChanged, SensiPlot_RadioButton_ModeEvenDistribution.CheckedChanged

        If (Me.isInitializing) Then Exit Sub

        Dim i As Integer

        'Entweder 1 oder 2 OptParameter
        If (Me.SensiPlot_ListBox_OptParameter.SelectedIndices.Count = 0 _
            Or Me.SensiPlot_ListBox_OptParameter.SelectedIndices.Count > 2) Then
            MsgBox("Bitte zwischen 1 und 2 OptParameter auswählen!", MsgBoxStyle.Exclamation, "SensiPlot")
            'Auswahl zurücksetzen
            Me.isInitializing = True
            For i = 0 To Me.SensiPlot_ListBox_OptParameter.Items.Count - 1
                Call Me.SensiPlot_ListBox_OptParameter.SetSelected(i, False)
            Next
            Me.isInitializing = False
            'Ersten OptParameter auswählen
            Me.SensiPlot_ListBox_OptParameter.SetSelected(0, True)
            Exit Sub
        End If

        'bei 2 OptParametern geht nur diskret!
        If (Me.SensiPlot_ListBox_OptParameter.SelectedIndices.Count = 2 And
            Me.SensiPlot_RadioButton_ModeRandom.Checked) Then
            MsgBox("Bei mehr als einem OptParameter muss 'Diskret' als Modus ausgewählt sein!", MsgBoxStyle.Exclamation, "SensiPlot")
            Me.SensiPlot_RadioButton_ModeEvenDistribution.Checked = True
            Exit Sub
        End If

        Me.SensiPlot_RadioButton_ModeEvenDistribution.DataBindings("Checked").WriteValue()
        Me.SensiPlot_RadioButton_ModeRandom.DataBindings("Checked").WriteValue()

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
    ''' Ensure that combobox changes are saved to the data binding immediately
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _
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

        CType(sender, ComboBox).DataBindings(0).WriteValue()
    End Sub

#End Region 'Events

#End Region 'Methoden

End Class
