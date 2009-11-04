'*******************************************************************************
'*******************************************************************************
'**** Klasse EVO_Einstellungen                                              ****
'****                                                                       ****
'**** Autoren: Christoph H�bner, Felix Fr�hlich, Dirk Muschalla             ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'****                                                                       ****
'**** November 2007                                                         ****
'****                                                                       ****
'**** Letzte �nderung: November 2007                                        ****
'*******************************************************************************
'*******************************************************************************

Imports System.IO
Imports IHWB.EVO.Common.Constants

Public Class EVO_Einstellungen
    Inherits System.Windows.Forms.UserControl

#Region "Eigenschaften"

    Private mSettings As EVO.Common.Settings
    Private mProblem As EVO.Common.Problem           'Das Problem
    Private isInitializing As Boolean

#End Region

#Region "Methoden"

    'Konstruktor
    '***********
    Public Sub New()

        Me.isInitializing = True

        ' Dieser Aufruf ist f�r den Windows Form-Designer erforderlich.
        Call Me.InitializeComponent()

        'Comboboxen initialisieren
        '-------------------------
        'PES:
        Me.PES_Combo_Strategie.DataSource = System.Enum.GetValues(GetType(EVO_STRATEGIE))
        Me.PES_Combo_Startparameter.DataSource = System.Enum.GetValues(GetType(EVO_STARTPARAMETER))
        Me.PES_Combo_DnMutation.DataSource = System.Enum.GetValues(GetType(PES_MUTATION))
        Me.PES_Combo_OptEltern.DataSource = System.Enum.GetValues(GetType(PES_REPRODOP))
        Me.PES_Combo_PopEltern.DataSource = System.Enum.GetValues(GetType(EVO_POP_ELTERN))
        Me.PES_Combo_PopStrategie.DataSource = System.Enum.GetValues(GetType(EVO_STRATEGIE))

        'CES
        Me.CES_Combo_Reproduction.DataSource = System.Enum.GetValues(GetType(CES_REPRODOP))
        Me.CES_Combo_Mutation.DataSource = System.Enum.GetValues(GetType(CES_MUTATION))
        Me.CES_Combo_Selection.DataSource = System.Enum.GetValues(GetType(EVO_STRATEGIE))
        Me.CES_Combo_IniValues.DataSource = System.Enum.GetValues(GetType(EVO_STARTPARAMETER))
        Me.CES_Combo_HybridType.DataSource = System.Enum.GetValues(GetType(HYBRID_TYPE))
        Me.CES_Combo_MemStrategy.DataSource = System.Enum.GetValues(GetType(MEMORY_STRATEGY))

        'TSP
        Me.TSP_ComboBox_prob_instance.DataSource = System.Enum.GetValues(GetType(EnProblem))
        Me.TSP_ComboBox_Reproductionoperator.DataSource = System.Enum.GetValues(GetType(EnReprodOperator))
        Me.TSP_ComboBox_Mutationoperator.DataSource = System.Enum.GetValues(GetType(EnMutOperator))

        'Listboxen von SensiPlot werden erst bei setProblem() gef�llt!

        Me.isInitializing = False

    End Sub

    ''' <summary>
    ''' Setzt das Problem zur�ck
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
            Me.BindingSource_PES_Schrittweite.Add(Me.mSettings.PES.Mutation)

            Me.BindingSource_PES_SekPop.Clear()
            Me.BindingSource_PES_SekPop.Add(Me.mSettings.PES.SekPop)

            Me.BindingSource_PES_Pop.Clear()
            Me.BindingSource_PES_Pop.Add(Me.mSettings.PES.Pop)
        End If

        'CES
        '---
        If (Not IsNothing(Me.mSettings.CES)) Then
            Me.BindingSource_CES.Clear()
            Me.BindingSource_CES.Add(Me.mSettings.CES)
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
    Public Sub setProblem(ByRef prob As EVO.Common.Problem)

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

            Case METH_CES, METH_HYBRID
                'PES-Settings instanzieren
                Me.mSettings.PES = New Common.Settings_PES()
                Me.mSettings.PES.setStandard(Me.mProblem.Modus)
                'CES-Settings instanzieren
                Me.mSettings.CES = New Common.Settings_CES()
                Me.mSettings.CES.setStandard(Me.mProblem.Method)

            Case METH_METAEVO
                'MetaEvo-Settings instanzieren
                Me.mSettings.MetaEvo = New Common.Settings_MetaEvo()
                Me.mSettings.MetaEvo.setStandard()

            Case METH_SENSIPLOT
                'Sensiplot-Settings instanzieren
                Me.mSettings.SensiPlot = New Common.Settings_Sensiplot()
                Me.mSettings.SensiPlot.setStandard()

                'Listboxen f�llen
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
                'Standardm��ig ersten OptParameter und erste ObjectiveFunction ausw�hlen
                Me.SensiPlot_ListBox_OptParameter.SetSelected(0, True)
                Me.SensiPlot_ListBox_Objectives.SetSelected(0, True)

            Case METH_TSP
                'TSP-Settings instanzieren
                Me.mSettings.TSP = New Common.Settings_TSP()
                Me.mSettings.TSP.setStandard()

            Case Else
                Throw New Exception("Unbekannte Methode '" & Me.mProblem.Method & "' in EVO_Einstellungen.Initialise()")

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

                Case METH_CES, METH_HYBRID
                    Me.TabControl1.TabPages.Add(Me.TabPage_PES)
                    Me.TabControl1.TabPages.Add(Me.TabPage_CES)

                Case METH_METAEVO
                    Me.TabControl1.TabPages.Add(Me.TabPage_MetaEvo)

                Case METH_SENSIPLOT
                    Me.TabControl1.TabPages.Add(Me.TabPage_SensiPlot)

                Case METH_TSP
                    Me.TabControl1.TabPages.Add(Me.TabPage_TSP)

                Case Else
                    Throw New Exception("Unbekannte Methode '" & Me.mProblem.Method & "' in EVO_Einstellungen.Initialise()")

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

    Private Sub SensiPlot_Validate() Handles SensiPlot_ListBox_OptParameter.SelectedIndexChanged, SensiPlot_ListBox_Objectives.SelectedIndexChanged, SensiPlot_RadioButton_Discrete.CheckedChanged

        If (Me.isInitializing) Then Exit Sub

        Dim i As Integer

        'Entweder 1 oder 2 OptParameter
        If (Me.SensiPlot_ListBox_OptParameter.SelectedIndices.Count = 0 _
            Or Me.SensiPlot_ListBox_OptParameter.SelectedIndices.Count > 2) Then
            MsgBox("Bitte zwischen 1 und 2 OptParameter ausw�hlen!", MsgBoxStyle.Exclamation, "SensiPlot")
            'Auswahl zur�cksetzen
            Me.isInitializing = True
            For i = 0 To Me.SensiPlot_ListBox_OptParameter.Items.Count - 1
                Call Me.SensiPlot_ListBox_OptParameter.SetSelected(i, False)
            Next
            Me.isInitializing = False
            'Ersten OptParameter ausw�hlen
            Me.SensiPlot_ListBox_OptParameter.SetSelected(0, True)
            Exit Sub
        End If

        'bei 2 OptParametern geht nur diskret!
        If (Me.SensiPlot_ListBox_OptParameter.SelectedIndices.Count = 2 And _
            Me.SensiPlot_RadioButton_NormalDistribution.Checked) Then
            MsgBox("Bei mehr als einem OptParameter muss 'Diskret' als Modus ausgew�hlt sein!", MsgBoxStyle.Exclamation, "SensiPlot")
            Me.SensiPlot_RadioButton_Discrete.Checked = True
            Exit Sub
        End If

        Me.SensiPlot_RadioButton_Discrete.DataBindings("Checked").WriteValue()
        Me.SensiPlot_RadioButton_NormalDistribution.DataBindings("Checked").WriteValue()

        'SelectedOptParameters einlesen
        Me.mSettings.SensiPlot.Selected_OptParameters.Clear()
        For i = 0 To Me.SensiPlot_ListBox_OptParameter.SelectedIndices.Count - 1
            Me.mSettings.SensiPlot.Selected_OptParameters.Add(Me.SensiPlot_ListBox_OptParameter.SelectedIndices(i))
        Next

        'SelectedObjective einlesen
        Me.mSettings.SensiPlot.Selected_Objective = Me.SensiPlot_ListBox_Objectives.SelectedIndex

        Call Me.refreshForm()

    End Sub

    Private Sub CES_HybridTypeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CES_Combo_HybridType.SelectedIndexChanged

        If (Me.isInitializing) Then Exit Sub

        'Sofortige Aktualisierung erzwingen
        Me.CES_Combo_HybridType.DataBindings(0).WriteValue()

        'PES-Einstellungen �ndern
        Select Case Me.mSettings.CES.HybridType

            Case HYBRID_TYPE.Mixed_Integer

                'Generationen
                Me.mSettings.PES.N_Gen = 1
                Me.PES_Numeric_AnzGen.Enabled = False

                'Eltern
                Me.mSettings.PES.N_Eltern = 5
                Me.PES_Numeric_AnzEltern.Enabled = True
                Me.LabelAnzEltern.Text = "Maximal Zahl der Eltern:"

                'Children
                Me.mSettings.PES.N_Nachf = 1
                Me.PES_Numeric_AnzNachf.Enabled = False

            Case HYBRID_TYPE.Sequencial_1

                'Generationen
                Me.mSettings.PES.N_Gen = 10
                Me.PES_Numeric_AnzGen.Enabled = True

                'Eltern
                Me.mSettings.PES.N_Eltern = 5
                Me.PES_Numeric_AnzEltern.Enabled = True
                Me.LabelAnzEltern.Text = "Anzahl der Eltern:"

                'Children
                Me.mSettings.PES.N_Nachf = 15
                Me.PES_Numeric_AnzNachf.Enabled = True

        End Select

        'WICHTIG!
        Call Me.refreshForm()

    End Sub

    Private Sub PES_Combo_OptEltern_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PES_Combo_OptEltern.SelectedIndexChanged
        'Sofortige Aktualisierung erzwingen!
        Me.PES_Combo_OptEltern.DataBindings(0).WriteValue()
    End Sub

    Private Sub CES_Combo_Reproduction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CES_Combo_Reproduction.SelectedIndexChanged
        'Sofortige Aktualisierung erzwingen
        Me.CES_Combo_Reproduction.DataBindings(0).WriteValue()
    End Sub

    Private Sub MetaEvo_Combo_Role_SelectedIndexChanged( ByVal sender As System.Object,  ByVal e As System.EventArgs) Handles MetaEvo_Combo_Role.SelectedIndexChanged
        'Sofortige Aktualisierung erzwingen
        Me.MetaEvo_Combo_Role.DataBindings(0).WriteValue()
    End Sub

    Private Sub MetaEvo_Combo_OpMode_SelectedIndexChanged( ByVal sender As System.Object,  ByVal e As System.EventArgs) Handles MetaEvo_Combo_OpMode.SelectedIndexChanged
        'Sofortige Aktualisierung erzwingen
        Me.MetaEvo_Combo_OpMode.DataBindings(0).WriteValue()
    End Sub

#End Region 'Events

#End Region 'Methoden

End Class
