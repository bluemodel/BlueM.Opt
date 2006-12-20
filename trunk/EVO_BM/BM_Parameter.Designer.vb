<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BM_Parameter
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
        Me.ComboBox_Elemente = New System.Windows.Forms.ComboBox
        Me.Label_Elemente = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'ComboBox_Elemente
        '
        Me.ComboBox_Elemente.FormattingEnabled = True
        Me.ComboBox_Elemente.Location = New System.Drawing.Point(67, 10)
        Me.ComboBox_Elemente.Name = "ComboBox_Elemente"
        Me.ComboBox_Elemente.Size = New System.Drawing.Size(200, 21)
        Me.ComboBox_Elemente.TabIndex = 0
        '
        'Label_Elemente
        '
        Me.Label_Elemente.AutoSize = True
        Me.Label_Elemente.Location = New System.Drawing.Point(13, 13)
        Me.Label_Elemente.Name = "Label_Elemente"
        Me.Label_Elemente.Size = New System.Drawing.Size(48, 13)
        Me.Label_Elemente.TabIndex = 1
        Me.Label_Elemente.Text = "Element:"
        '
        'BM_Parameter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(491, 433)
        Me.Controls.Add(Me.Label_Elemente)
        Me.Controls.Add(Me.ComboBox_Elemente)
        Me.Name = "BM_Parameter"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Parameter"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboBox_Elemente As System.Windows.Forms.ComboBox
    Friend WithEvents Label_Elemente As System.Windows.Forms.Label
End Class
