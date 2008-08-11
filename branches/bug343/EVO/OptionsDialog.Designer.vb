<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OptionsDialog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim GroupBox_Diagramm As System.Windows.Forms.GroupBox
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OptionsDialog))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.Label_showOnlyCurrentPop = New System.Windows.Forms.Label
        Me.CheckBox_showOnlyCurrentPop = New System.Windows.Forms.CheckBox
        GroupBox_Diagramm = New System.Windows.Forms.GroupBox
        Me.TableLayoutPanel1.SuspendLayout()
        GroupBox_Diagramm.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(44, 84)
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
        Me.Cancel_Button.Text = "Cancel"
        '
        'GroupBox_Diagramm
        '
        GroupBox_Diagramm.Controls.Add(Me.CheckBox_showOnlyCurrentPop)
        GroupBox_Diagramm.Controls.Add(Me.Label_showOnlyCurrentPop)
        GroupBox_Diagramm.Location = New System.Drawing.Point(13, 13)
        GroupBox_Diagramm.Name = "GroupBox_Diagramm"
        GroupBox_Diagramm.Size = New System.Drawing.Size(180, 62)
        GroupBox_Diagramm.TabIndex = 1
        GroupBox_Diagramm.TabStop = False
        GroupBox_Diagramm.Text = "Diagramm"
        '
        'Label_showOnlyCurrentPop
        '
        Me.Label_showOnlyCurrentPop.Location = New System.Drawing.Point(7, 20)
        Me.Label_showOnlyCurrentPop.Name = "Label_showOnlyCurrentPop"
        Me.Label_showOnlyCurrentPop.Size = New System.Drawing.Size(141, 33)
        Me.Label_showOnlyCurrentPop.TabIndex = 0
        Me.Label_showOnlyCurrentPop.Text = "Nur die aktuelle Population jeder Generation anzeigen:"
        '
        'CheckBox_showOnlyCurrentPop
        '
        Me.CheckBox_showOnlyCurrentPop.AutoSize = True
        Me.CheckBox_showOnlyCurrentPop.Location = New System.Drawing.Point(153, 26)
        Me.CheckBox_showOnlyCurrentPop.Name = "CheckBox_showOnlyCurrentPop"
        Me.CheckBox_showOnlyCurrentPop.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_showOnlyCurrentPop.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox_showOnlyCurrentPop.TabIndex = 1
        Me.CheckBox_showOnlyCurrentPop.UseVisualStyleBackColor = True
        '
        'Options
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(202, 125)
        Me.Controls.Add(GroupBox_Diagramm)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Options"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Optionen"
        Me.TableLayoutPanel1.ResumeLayout(False)
        GroupBox_Diagramm.ResumeLayout(False)
        GroupBox_Diagramm.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents CheckBox_showOnlyCurrentPop As System.Windows.Forms.CheckBox
    Friend WithEvents Label_showOnlyCurrentPop As System.Windows.Forms.Label

End Class
