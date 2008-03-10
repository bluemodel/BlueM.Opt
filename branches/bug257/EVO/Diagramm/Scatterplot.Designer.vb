<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Scatterplot
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Scatterplot))
        Me.matrix = New System.Windows.Forms.TableLayoutPanel
        Me.SuspendLayout()
        '
        'matrix
        '
        Me.matrix.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
        Me.matrix.Location = New System.Drawing.Point(0, 0)
        Me.matrix.Name = "matrix"
        Me.matrix.Size = New System.Drawing.Size(800, 800)
        Me.matrix.TabIndex = 0
        '
        'Scatterplot
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 800)
        Me.Controls.Add(Me.matrix)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Scatterplot"
        Me.Text = "Scatterplot-Matrix"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents matrix As System.Windows.Forms.TableLayoutPanel
End Class
