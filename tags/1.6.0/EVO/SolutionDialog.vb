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
Partial Public Class SolutionDialog
    Inherits System.Windows.Forms.Form

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse SolutionDialog                                                 ****
    '****                                                                       ****
    '**** Zeigt die ausgewählten Lösungen an                                    ****
    '****                                                                       ****
    '**** Autoren: Felix Froehlich                                              ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

    'Das Problem
    Private mProblem As EVO.Common.Problem

    'Properties
    '**********
    Private ReadOnly Property checkedSolutions() As Collection
        Get
            checkedSolutions = New Collection()
            For Each row As DataGridViewRow In Me.DataGridView1.Rows
                If (row.Cells(0).Value = "True") Then
                    checkedSolutions.Add(row.HeaderCell.Value, row.HeaderCell.Value)
                End If
            Next
            Return checkedSolutions
        End Get
    End Property

    'Events
    '******
    Public Event WaveClicked(ByVal checkedSolutions As Collection)
    Public Event ClearClicked()


    ''' <summary>
    ''' Konstruktor
    ''' </summary>
    ''' <param name="prob">Das Problem</param>
    Public Sub New(ByRef prob As EVO.Common.Problem)

        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        'Problem speichern
        Me.mProblem = prob

        'Spalten einrichten
        '==================

        Dim column As DataGridViewColumn
        Dim cellstyle As DataGridViewCellStyle

        'Allgemein
        '---------
        cellstyle = Me.DataGridView1.DefaultCellStyle.Clone()
        cellstyle.Format = "G5"

        'Ziele
        '--------
        For Each feature As Common.ObjectiveFunction In Me.mProblem.List_ObjectiveFunctions
            column = New DataGridViewTextBoxColumn()
            If (feature.isPrimObjective) Then
                cellstyle.BackColor = Color.LightGreen
                column.HeaderText = feature.Bezeichnung & " (*)"
                column.HeaderCell.ToolTipText = "Primary objective function"
            Else
                cellstyle.BackColor = Color.LightBlue
                column.HeaderText = feature.Bezeichnung
                column.HeaderCell.ToolTipText = "Secondary objective function"
            End If
            column.ReadOnly = True
            column.Name = feature.Bezeichnung
            column.DefaultCellStyle = cellstyle.Clone()
            Me.DataGridView1.Columns.Add(column)
        Next

        'Constraints
        '-----------
        cellstyle.BackColor = Color.LightCoral

        For Each Constraint As Common.Constraintfunction In Me.mProblem.List_Constraintfunctions
            column = New DataGridViewTextBoxColumn()
            column.ReadOnly = True
            column.HeaderText = Constraint.Bezeichnung
            column.HeaderCell.ToolTipText = "Constraint"
            column.Name = Constraint.Bezeichnung
            column.DefaultCellStyle = cellstyle.Clone()
            Me.DataGridView1.Columns.Add(column)
        Next

        'Locations
        '---------
        cellstyle.BackColor = Color.AliceBlue

        For Each Location As EVO.Common.Locations.Struct_Lokation In Me.mProblem.List_Locations
            column = New DataGridViewTextBoxColumn()
            column.ReadOnly = True
            column.HeaderText = Location.Name
            column.HeaderCell.ToolTipText = "Location"
            column.Name = Location.Name
            column.DefaultCellStyle = cellstyle.Clone()
            Me.DataGridView1.Columns.Add(column)
        Next

        'OptParameter
        '------------
        cellstyle.BackColor = Color.LightGray

        For Each OptPara As EVO.Common.OptParameter In Me.mProblem.List_OptParameter_Save
            column = New DataGridViewTextBoxColumn()
            column.ReadOnly = True
            column.HeaderText = OptPara.Bezeichnung
            column.HeaderCell.ToolTipText = "Optimization parameter"
            column.Name = OptPara.Bezeichnung
            column.DefaultCellStyle = cellstyle.Clone()
            Me.DataGridView1.Columns.Add(column)
        Next

        'Handler einrichten
        '==================
        AddHandler Me.WaveClicked, AddressOf Form1.showWave
        AddHandler Me.ClearClicked, AddressOf Form1.clearSelection

    End Sub

    ''' <summary>
    ''' Ein Individuum zur Lösungsauswahl hinzufügen
    ''' </summary>
    ''' <param name="ind">das ausgewählte Individuum</param>
    Public Sub addSolution(ByVal ind As Common.Individuum)

        'Sonderfall CES-Individuum
        If (TypeOf ind Is EVO.Common.Individuum_CES) Then
            Call Me.addSolution_CES(CType(ind, EVO.Common.Individuum_CES))
            Exit Sub
        End If

        Dim i As Integer
        Dim cellvalues() As Object
        Dim row As DataGridViewRow

        'Daten zusammenstellen
        '---------------------
        ReDim cellvalues(Me.DataGridView1.ColumnCount - 1)

        cellvalues(0) = True

        i = 1

        'Ziele
        For Each featurevalue As Double In ind.Objectives
            cellvalues(i) = featurevalue
            i += 1
        Next

        'Constraints
        For Each constraintvalue As Double In ind.Constraints
            cellvalues(i) = constraintvalue
            i += 1
        Next

        'OptParameter
        For Each optpara As Double In ind.OptParameter_RWerte
            cellvalues(i) = optpara
            i += 1
        Next

        'Zeile erstellen
        row = New DataGridViewRow()
        row.CreateCells(Me.DataGridView1, cellvalues)
        row.HeaderCell.Value = ind.ID.ToString()

        'Zeile hinzufügen
        Me.DataGridView1.Rows.Add(row)

        'Spalten anpassen
        Call Me.DataGridView1.AutoResizeColumns()

    End Sub

    ''' <summary>
    ''' Ein CES-Individuum zur Lösungsauswahl hinzufügen
    ''' </summary>
    ''' <param name="ind">das ausgewählte CES-Individuum</param>
    Private Sub addSolution_CES(ByVal ind As Common.Individuum_CES)

        Dim i As Integer
        Dim cellvalues() As Object
        Dim row As DataGridViewRow

        'Daten zusammenstellen
        '---------------------
        ReDim cellvalues(Me.DataGridView1.ColumnCount - 1)

        cellvalues(0) = True

        i = 1

        'Ziele
        For Each featurevalue As Double In ind.Objectives
            cellvalues(i) = featurevalue
            i += 1
        Next

        'Constraints
        For Each constraintvalue As Double In ind.Constraints
            cellvalues(i) = constraintvalue
            i += 1
        Next

        'Measures
        For Each measure As String In ind.Measures
            cellvalues(i) = measure
            i += 1
        Next

        'OptParameter CES
        Dim found As Boolean

        Do While i < Me.DataGridView1.ColumnCount
            found = False
            For Each loc As Common.Individuum_CES.Location_Data In CType(ind, Common.Individuum_CES).Loc
                For Each optpara As Common.OptParameter In loc.PES_OptPara
                    If optpara.Bezeichnung = Me.DataGridView1.Columns(i).HeaderText Then
                        cellvalues(i) = optpara.RWert
                        found = True
                    End If
                    If (found) Then Exit For
                Next
                If (found) Then Exit For
            Next
            If (Not found) Then
                cellvalues(i) = "---"
            End If
            i += 1
        Loop

        'Zeile erstellen
        row = New DataGridViewRow()
        row.CreateCells(Me.DataGridView1, cellvalues)
        row.HeaderCell.Value = ind.ID.ToString()

        'Zeile hinzufügen
        Me.DataGridView1.Rows.Add(row)

        'Spalten anpassen
        Call Me.DataGridView1.AutoResizeColumns()

    End Sub

    'Automatisches speichern von Zellenänderungen
    '********************************************
    Private Sub dataGridView1_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DataGridView1.CurrentCellDirtyStateChanged

        If (DataGridView1.IsCurrentCellDirty) Then
            DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If

    End Sub

    'Lösungsauswahl zurücksetzen
    '***************************
    Private Sub ToolStripButton_Clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_Clear.Click

        'Alle Zeilen löschen
        Call Me.DataGridView1.Rows.Clear()

        'Lösungsdialog verstecken
        Call Me.Hide()

        'Event auslösen
        RaiseEvent ClearClicked()

    End Sub

    'Wave anzeigen
    '*************
    Private Sub ToolStripButton_Wave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_Wave.Click

        'Cursor
        Cursor = Cursors.WaitCursor

        'Event auslösen
        RaiseEvent WaveClicked(Me.checkedSolutions)

        'Cursor
        Cursor = Cursors.Default

    End Sub

    'Form resize
    '***********
    Private Sub SolutionDialog_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize

        Me.DataGridView1.Width = Me.ClientSize.Width
        Me.DataGridView1.Height = Me.ClientSize.Height - 25       'Minus Höhe der Toolbar

    End Sub

    'Form schließen
    '**************
    Private Sub SolutionDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        'verhindern, dass das Formular komplett gelöscht wird
        e.Cancel = True

        'Formular verstecken
        Call Me.Hide()

    End Sub

End Class
