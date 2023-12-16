<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SolutionDialog
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SolutionDialog))
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Selection = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton_Wave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButton_unselect = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton_Clear = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripTextBox_ID = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripButton_SelectByID = New System.Windows.Forms.ToolStripButton()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Selection})
        Me.DataGridView1.Location = New System.Drawing.Point(0, 25)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        Me.DataGridView1.ShowEditingIcon = False
        Me.DataGridView1.ShowRowErrors = False
        Me.DataGridView1.Size = New System.Drawing.Size(601, 282)
        Me.DataGridView1.TabIndex = 0
        '
        'Selection
        '
        Me.Selection.FalseValue = "False"
        Me.Selection.HeaderText = "Selection"
        Me.Selection.Name = "Selection"
        Me.Selection.TrueValue = "True"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton_Wave, Me.ToolStripSeparator1, Me.ToolStripButton_unselect, Me.ToolStripButton_Clear, Me.ToolStripSeparator2, Me.ToolStripLabel1, Me.ToolStripTextBox_ID, Me.ToolStripButton_SelectByID})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(601, 25)
        Me.ToolStrip1.TabIndex = 15
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton_Wave
        '
        Me.ToolStripButton_Wave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_Wave.Image = Global.BlueM.Opt.My.Resources.Resources.table_lightning
        Me.ToolStripButton_Wave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_Wave.Name = "ToolStripButton_Wave"
        Me.ToolStripButton_Wave.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_Wave.Text = "Simulate the checked solutions and display the results in Wave"
        Me.ToolStripButton_Wave.ToolTipText = "Simulate the checked solutions and display the results in Wave"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripButton_unselect
        '
        Me.ToolStripButton_unselect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_unselect.Image = Global.BlueM.Opt.My.Resources.Resources.table_row_delete
        Me.ToolStripButton_unselect.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_unselect.Name = "ToolStripButton_unselect"
        Me.ToolStripButton_unselect.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_unselect.Text = "Unselect the unchecked solutions"
        Me.ToolStripButton_unselect.ToolTipText = "Unselect the unchecked solutions"
        '
        'ToolStripButton_Clear
        '
        Me.ToolStripButton_Clear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_Clear.Image = Global.BlueM.Opt.My.Resources.Resources.table_delete
        Me.ToolStripButton_Clear.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_Clear.Name = "ToolStripButton_Clear"
        Me.ToolStripButton_Clear.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_Clear.Text = "Unselect all solutions"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(71, 22)
        Me.ToolStripLabel1.Text = "Select by ID:"
        '
        'ToolStripTextBox_ID
        '
        Me.ToolStripTextBox_ID.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ToolStripTextBox_ID.Name = "ToolStripTextBox_ID"
        Me.ToolStripTextBox_ID.Size = New System.Drawing.Size(40, 25)
        '
        'ToolStripButton_SelectByID
        '
        Me.ToolStripButton_SelectByID.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_SelectByID.Image = Global.BlueM.Opt.My.Resources.Resources.magnifier
        Me.ToolStripButton_SelectByID.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_SelectByID.Name = "ToolStripButton_SelectByID"
        Me.ToolStripButton_SelectByID.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_SelectByID.Text = "Select by ID"
        '
        'SolutionDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(601, 306)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "SolutionDialog"
        Me.Text = "Selected solutions"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton_Wave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton_Clear As System.Windows.Forms.ToolStripButton
    Friend WithEvents SpalteAuswahl As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Selection As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents ToolStripButton_unselect As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripLabel1 As ToolStripLabel
    Friend WithEvents ToolStripTextBox_ID As ToolStripTextBox
    Friend WithEvents ToolStripButton_SelectByID As ToolStripButton
End Class
