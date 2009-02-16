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
        Me.CheckBox_drawOnlyCurrentGen = New System.Windows.Forms.CheckBox
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.GroupBox_Sim = New System.Windows.Forms.GroupBox
        Me.CheckBox_useMultithreading = New System.Windows.Forms.CheckBox
        GroupBox_Diagramm = New System.Windows.Forms.GroupBox
        GroupBox_Diagramm.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox_Sim.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox_Diagramm
        '
        GroupBox_Diagramm.Controls.Add(Me.CheckBox_drawOnlyCurrentGen)
        GroupBox_Diagramm.Location = New System.Drawing.Point(12, 72)
        GroupBox_Diagramm.Name = "GroupBox_Diagramm"
        GroupBox_Diagramm.Size = New System.Drawing.Size(188, 62)
        GroupBox_Diagramm.TabIndex = 1
        GroupBox_Diagramm.TabStop = False
        GroupBox_Diagramm.Text = "Diagramm"
        '
        'CheckBox_drawOnlyCurrentGen
        '
        Me.CheckBox_drawOnlyCurrentGen.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox_drawOnlyCurrentGen.Location = New System.Drawing.Point(6, 19)
        Me.CheckBox_drawOnlyCurrentGen.Name = "CheckBox_drawOnlyCurrentGen"
        Me.CheckBox_drawOnlyCurrentGen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_drawOnlyCurrentGen.Size = New System.Drawing.Size(158, 37)
        Me.CheckBox_drawOnlyCurrentGen.TabIndex = 1
        Me.CheckBox_drawOnlyCurrentGen.Text = "Nur die aktuelle Generation anzeigen:"
        Me.CheckBox_drawOnlyCurrentGen.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(54, 146)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.DialogResult = System.Windows.Forms.DialogResult.OK
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
        'GroupBox_Sim
        '
        Me.GroupBox_Sim.Controls.Add(Me.CheckBox_useMultithreading)
        Me.GroupBox_Sim.Location = New System.Drawing.Point(12, 13)
        Me.GroupBox_Sim.Name = "GroupBox_Sim"
        Me.GroupBox_Sim.Size = New System.Drawing.Size(188, 53)
        Me.GroupBox_Sim.TabIndex = 2
        Me.GroupBox_Sim.TabStop = False
        Me.GroupBox_Sim.Text = "Simulationen"
        '
        'CheckBox_useMultithreading
        '
        Me.CheckBox_useMultithreading.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox_useMultithreading.Location = New System.Drawing.Point(6, 20)
        Me.CheckBox_useMultithreading.Name = "CheckBox_useMultithreading"
        Me.CheckBox_useMultithreading.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox_useMultithreading.Size = New System.Drawing.Size(158, 24)
        Me.CheckBox_useMultithreading.TabIndex = 0
        Me.CheckBox_useMultithreading.Text = "Multithreading benutzen:"
        Me.CheckBox_useMultithreading.UseVisualStyleBackColor = True
        '
        'OptionsDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(210, 187)
        Me.Controls.Add(Me.GroupBox_Sim)
        Me.Controls.Add(GroupBox_Diagramm)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OptionsDialog"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Allgemeine Einstellungen"
        GroupBox_Diagramm.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupBox_Sim.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CheckBox_drawOnlyCurrentGen As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox_useMultithreading As System.Windows.Forms.CheckBox
    Private WithEvents GroupBox_Sim As System.Windows.Forms.GroupBox
    Private WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Private WithEvents OK_Button As System.Windows.Forms.Button
    Private WithEvents Cancel_Button As System.Windows.Forms.Button

End Class
