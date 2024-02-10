<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class Scatterplot
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Scatterplot))
        Me.matrix = New System.Windows.Forms.TableLayoutPanel()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton_highlight = New System.Windows.Forms.ToolStripButton()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'matrix
        '
        Me.matrix.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.matrix.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
        Me.matrix.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.matrix.Location = New System.Drawing.Point(0, 28)
        Me.matrix.Margin = New System.Windows.Forms.Padding(0)
        Me.matrix.Name = "matrix"
        Me.matrix.Size = New System.Drawing.Size(800, 772)
        Me.matrix.TabIndex = 0
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton_highlight})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(800, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton_highlight
        '
        Me.ToolStripButton_highlight.CheckOnClick = True
        Me.ToolStripButton_highlight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_highlight.Image = Global.BlueM.Opt.Diagramm.My.Resources.Resources.crosshair
        Me.ToolStripButton_highlight.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_highlight.Name = "ToolStripButton_highlight"
        Me.ToolStripButton_highlight.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton_highlight.Text = "Highlight solutions on hover"
        '
        'Scatterplot
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 800)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.matrix)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Scatterplot"
        Me.Text = "Scatterplot matrix"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents matrix As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ToolStrip1 As Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton_highlight As Windows.Forms.ToolStripButton
End Class
