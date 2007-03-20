<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SensiPlot
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.ListBox_OptParameter = New System.Windows.Forms.ListBox
        Me.Label_OptParameter = New System.Windows.Forms.Label
        Me.ListBox_OptZiele = New System.Windows.Forms.ListBox
        Me.Label_OptZiele = New System.Windows.Forms.Label
        Me.TextBox_AnzSim = New System.Windows.Forms.MaskedTextBox
        Me.Label_AnzSim = New System.Windows.Forms.Label
        Me.RadioButton_Gleichverteilt = New System.Windows.Forms.RadioButton
        Me.RadioButton_Diskret = New System.Windows.Forms.RadioButton
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(294, 219)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Abbrechen"
        '
        'ListBox_OptParameter
        '
        Me.ListBox_OptParameter.FormattingEnabled = True
        Me.ListBox_OptParameter.Location = New System.Drawing.Point(12, 32)
        Me.ListBox_OptParameter.Name = "ListBox_OptParameter"
        Me.ListBox_OptParameter.Size = New System.Drawing.Size(130, 212)
        Me.ListBox_OptParameter.TabIndex = 1
        '
        'Label_OptParameter
        '
        Me.Label_OptParameter.AutoSize = True
        Me.Label_OptParameter.Location = New System.Drawing.Point(12, 13)
        Me.Label_OptParameter.Name = "Label_OptParameter"
        Me.Label_OptParameter.Size = New System.Drawing.Size(69, 13)
        Me.Label_OptParameter.TabIndex = 2
        Me.Label_OptParameter.Text = "OptPrameter:"
        '
        'ListBox_OptZiele
        '
        Me.ListBox_OptZiele.FormattingEnabled = True
        Me.ListBox_OptZiele.Location = New System.Drawing.Point(148, 32)
        Me.ListBox_OptZiele.Name = "ListBox_OptZiele"
        Me.ListBox_OptZiele.Size = New System.Drawing.Size(130, 212)
        Me.ListBox_OptZiele.TabIndex = 3
        '
        'Label_OptZiele
        '
        Me.Label_OptZiele.AutoSize = True
        Me.Label_OptZiele.Location = New System.Drawing.Point(145, 13)
        Me.Label_OptZiele.Name = "Label_OptZiele"
        Me.Label_OptZiele.Size = New System.Drawing.Size(50, 13)
        Me.Label_OptZiele.TabIndex = 4
        Me.Label_OptZiele.Text = "OptZiele:"
        '
        'TextBox_AnzSim
        '
        Me.TextBox_AnzSim.Location = New System.Drawing.Point(370, 83)
        Me.TextBox_AnzSim.Mask = "00000"
        Me.TextBox_AnzSim.Name = "TextBox_AnzSim"
        Me.TextBox_AnzSim.Size = New System.Drawing.Size(67, 20)
        Me.TextBox_AnzSim.TabIndex = 7
        Me.TextBox_AnzSim.Text = "7"
        Me.TextBox_AnzSim.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextBox_AnzSim.ValidatingType = GetType(Integer)
        '
        'Label_AnzSim
        '
        Me.Label_AnzSim.AutoSize = True
        Me.Label_AnzSim.Location = New System.Drawing.Point(294, 86)
        Me.Label_AnzSim.Name = "Label_AnzSim"
        Me.Label_AnzSim.Size = New System.Drawing.Size(62, 13)
        Me.Label_AnzSim.TabIndex = 8
        Me.Label_AnzSim.Text = "Anzahl Sim:"
        '
        'RadioButton_Gleichverteilt
        '
        Me.RadioButton_Gleichverteilt.AutoSize = True
        Me.RadioButton_Gleichverteilt.Checked = True
        Me.RadioButton_Gleichverteilt.Location = New System.Drawing.Point(297, 32)
        Me.RadioButton_Gleichverteilt.Name = "RadioButton_Gleichverteilt"
        Me.RadioButton_Gleichverteilt.Size = New System.Drawing.Size(86, 17)
        Me.RadioButton_Gleichverteilt.TabIndex = 9
        Me.RadioButton_Gleichverteilt.TabStop = True
        Me.RadioButton_Gleichverteilt.Text = "Gleichverteilt"
        Me.RadioButton_Gleichverteilt.UseVisualStyleBackColor = True
        '
        'RadioButton_Diskret
        '
        Me.RadioButton_Diskret.AutoSize = True
        Me.RadioButton_Diskret.Location = New System.Drawing.Point(297, 55)
        Me.RadioButton_Diskret.Name = "RadioButton_Diskret"
        Me.RadioButton_Diskret.Size = New System.Drawing.Size(58, 17)
        Me.RadioButton_Diskret.TabIndex = 10
        Me.RadioButton_Diskret.Text = "Diskret"
        Me.RadioButton_Diskret.UseVisualStyleBackColor = True
        '
        'SensiPlot
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(452, 260)
        Me.Controls.Add(Me.RadioButton_Diskret)
        Me.Controls.Add(Me.RadioButton_Gleichverteilt)
        Me.Controls.Add(Me.Label_AnzSim)
        Me.Controls.Add(Me.TextBox_AnzSim)
        Me.Controls.Add(Me.Label_OptZiele)
        Me.Controls.Add(Me.ListBox_OptZiele)
        Me.Controls.Add(Me.Label_OptParameter)
        Me.Controls.Add(Me.ListBox_OptParameter)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SensiPlot"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "SensiPlot"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents ListBox_OptParameter As System.Windows.Forms.ListBox
    Friend WithEvents Label_OptParameter As System.Windows.Forms.Label
    Friend WithEvents ListBox_OptZiele As System.Windows.Forms.ListBox
    Friend WithEvents Label_OptZiele As System.Windows.Forms.Label
    Friend WithEvents TextBox_AnzSim As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label_AnzSim As System.Windows.Forms.Label
    Friend WithEvents RadioButton_Gleichverteilt As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_Diskret As System.Windows.Forms.RadioButton

End Class
