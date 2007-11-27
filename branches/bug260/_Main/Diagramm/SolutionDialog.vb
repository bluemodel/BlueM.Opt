Partial Public Class SolutionDialog
    Inherits System.Windows.Forms.Form

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse SolutionDialog                                                 ****
    '****                                                                       ****
    '**** Zeigt die ausgew�hlten L�sungen an                                    ****
    '****                                                                       ****
    '**** Autoren: Felix Froehlich                                              ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

    'Konstruktor
    '***********
    Public Sub New(ByVal lOptPara() As Sim.Struct_OptParameter, ByVal lOptZiele() As Sim.Struct_OptZiel, ByVal lConst() As Sim.Struct_Constraint)

        ' Dieser Aufruf ist f�r den Windows Form-Designer erforderlich.
        InitializeComponent()

        ' F�gen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        Dim column As DataGridViewColumn
        Dim cellstyle As DataGridViewCellStyle

        'Spalten einrichten
        '==================

        'Allgemein
        '---------
        cellstyle = Me.DataGridView1.DefaultCellStyle.Clone()
        cellstyle.Format = "G5"

        'OptParameter
        '------------
        cellstyle.BackColor = Color.LightGray

        For Each OptPara As Sim.Struct_OptParameter In lOptPara
            column = New DataGridViewTextBoxColumn()
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
            column.HeaderText = Constraint.Bezeichnung
            column.HeaderCell.ToolTipText = "Constraint"
            column.Name = Constraint.Bezeichnung
            column.DefaultCellStyle = cellstyle.Clone()
            Me.DataGridView1.Columns.Add(column)
        Next

        'Handler einrichten
        '------------------
        AddHandler Me.ToolStripButton_Wave.Click, AddressOf Form1.showWave
        AddHandler Me.ToolStripButton_Clear.Click, AddressOf Form1.clearSelection

    End Sub

    'Eine L�sung hinzuf�gen
    '**********************
    Public Sub addSolution(ByVal sol As Solution)

        Dim i As Integer
        Dim row() As Object

        'Daten in ein Array entpacken
        '----------------------------
        ReDim row(Me.DataGridView1.ColumnCount - 1)

        row(0) = sol.ID

        i = 1

        For Each optpara As Double In sol.OptPara
            row(i) = optpara
            i += 1
        Next

        For Each optziel As Double In sol.QWerte
            row(i) = optziel
            i += 1
        Next

        For Each constraint As Double In sol.Constraints
            row(i) = constraint
            i += 1
        Next

        'Zeile hinzuf�gen
        Me.DataGridView1.Rows.Add(row)

        'Spalten anpassen
        Call Me.DataGridView1.AutoResizeColumns()

    End Sub

    'L�sungsauswahl zur�cksetzen
    '***************************
    Private Sub ToolStripButton_Clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_Clear.Click

        'Alle Zeilen l�schen
        Call Me.DataGridView1.Rows.Clear()

        'L�sungsdialog verstecken
        Call Me.Hide()

    End Sub

    'Form resize
    '***********
    Private Sub SolutionDialog_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize

        Me.DataGridView1.Width = Me.ClientSize.Width
        Me.DataGridView1.Height = Me.ClientSize.Height - 25       'Minus H�he der Toolbar

    End Sub

    'Form schlie�en
    '**************
    Private Sub SolutionDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        'verhindern, dass das Formular komplett gel�scht wird
        e.Cancel = True

        'Formular verstecken
        Call Me.Hide()

    End Sub

End Class