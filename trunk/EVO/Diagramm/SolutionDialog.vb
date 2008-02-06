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

    'Properties
    '**********
    Private ReadOnly Property checkedSolutions As Collection
        Get
            checkedSolutions = New Collection
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


    'Konstruktor
    '***********
    Public Sub New(ByVal lOptPara() As Sim.Struct_OptParameter, ByVal lOptZiele() As Sim.Struct_OptZiel, ByVal lConst() As Sim.Struct_Constraint)

        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        'Spalten einrichten
        '==================

        Dim column As DataGridViewColumn
        Dim cellstyle As DataGridViewCellStyle

        'Allgemein
        '---------
        cellstyle = Me.DataGridView1.DefaultCellStyle.Clone()
        cellstyle.Format = "G5"

        'OptParameter
        '------------
        cellstyle.BackColor = Color.LightGray

        For Each OptPara As Sim.Struct_OptParameter In lOptPara
            column = New DataGridViewTextBoxColumn()
            column.ReadOnly = True
            column.HeaderText = OptPara.Bezeichnung
            column.HeaderCell.ToolTipText = "OptParameter"
            column.Name = OptPara.Bezeichnung
            column.DefaultCellStyle = cellstyle.Clone()
            Me.DataGridView1.Columns.Add(column)
        Next

        'OptZiele
        '--------
        cellstyle.BackColor = Color.LightBlue

        For Each OptZiel As Sim.Struct_OptZiel In lOptZiele
            column = New DataGridViewTextBoxColumn()
            column.ReadOnly = True
            column.HeaderText = OptZiel.Bezeichnung
            column.HeaderCell.ToolTipText = "OptZiel"
            column.Name = OptZiel.Bezeichnung
            column.DefaultCellStyle = cellstyle.Clone()
            Me.DataGridView1.Columns.Add(column)
        Next

        'Constraints
        '-----------
        cellstyle.BackColor = Color.LightCoral

        For Each Constraint As Sim.Struct_Constraint In lConst
            column = New DataGridViewTextBoxColumn()
            column.ReadOnly = True
            column.HeaderText = Constraint.Bezeichnung
            column.HeaderCell.ToolTipText = "Constraint"
            column.Name = Constraint.Bezeichnung
            column.DefaultCellStyle = cellstyle.Clone()
            Me.DataGridView1.Columns.Add(column)
        Next

        'Handler einrichten
        '==================
        AddHandler Me.WaveClicked, AddressOf Form1.showWave
        AddHandler Me.ClearClicked, AddressOf Form1.clearSelection

    End Sub

    'Eine Lösung hinzufügen
    '**********************
    Public Sub addSolution(ByVal sol As Kern.Individuum)

        Dim i As Integer
        Dim cellvalues() As Object
        Dim row As DataGridViewRow

        'Daten zusammenstellen
        '---------------------
        ReDim cellvalues(Me.DataGridView1.ColumnCount - 1)

        cellvalues(0) = True

        i = 1

        For Each optpara As Double In sol.PES_X
            cellvalues(i) = optpara
            i += 1
        Next

        For Each optziel As Double In sol.Penalty
            cellvalues(i) = optziel
            i += 1
        Next

        For Each constraint As Double In sol.Constrain
            cellvalues(i) = constraint
            i += 1
        Next

        'Zeile erstellen
        row = New DataGridViewRow()
        row.CreateCells(Me.DataGridView1, cellvalues)
        row.HeaderCell.Value = sol.ID.ToString()

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