<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AboutBox
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

    Friend WithEvents LogoPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents LabelProductName As System.Windows.Forms.Label
    Friend WithEvents LabelVersion As System.Windows.Forms.Label
    Friend WithEvents LabelCompanyName As System.Windows.Forms.Label
    Friend WithEvents TextBox_Description As System.Windows.Forms.TextBox
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents LabelCopyright As System.Windows.Forms.Label

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AboutBox))
        Me.LogoPictureBox = New System.Windows.Forms.PictureBox
        Me.LabelProductName = New System.Windows.Forms.Label
        Me.LabelVersion = New System.Windows.Forms.Label
        Me.LabelCopyright = New System.Windows.Forms.Label
        Me.LabelCompanyName = New System.Windows.Forms.Label
        Me.TextBox_Description = New System.Windows.Forms.TextBox
        Me.OKButton = New System.Windows.Forms.Button
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage_Description = New System.Windows.Forms.TabPage
        Me.TabPage_License = New System.Windows.Forms.TabPage
        Me.TextBox_License = New System.Windows.Forms.TextBox
        Me.TabPage_Credits = New System.Windows.Forms.TabPage
        Me.TextBox_Credits = New System.Windows.Forms.TextBox
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage_Description.SuspendLayout()
        Me.TabPage_License.SuspendLayout()
        Me.TabPage_Credits.SuspendLayout()
        Me.SuspendLayout()
        '
        'LogoPictureBox
        '
        Me.LogoPictureBox.Image = Global.IHWB.EVO.My.Resources.Resources.BlueM
        Me.LogoPictureBox.Location = New System.Drawing.Point(12, 12)
        Me.LogoPictureBox.Name = "LogoPictureBox"
        Me.LogoPictureBox.Size = New System.Drawing.Size(104, 104)
        Me.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.LogoPictureBox.TabIndex = 0
        Me.LogoPictureBox.TabStop = False
        '
        'LabelProductName
        '
        Me.LabelProductName.AutoSize = True
        Me.LabelProductName.Location = New System.Drawing.Point(125, 12)
        Me.LabelProductName.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
        Me.LabelProductName.MaximumSize = New System.Drawing.Size(0, 17)
        Me.LabelProductName.Name = "LabelProductName"
        Me.LabelProductName.Size = New System.Drawing.Size(75, 13)
        Me.LabelProductName.TabIndex = 0
        Me.LabelProductName.Text = "Product Name"
        Me.LabelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelVersion
        '
        Me.LabelVersion.AutoSize = True
        Me.LabelVersion.Location = New System.Drawing.Point(125, 42)
        Me.LabelVersion.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
        Me.LabelVersion.MaximumSize = New System.Drawing.Size(0, 17)
        Me.LabelVersion.Name = "LabelVersion"
        Me.LabelVersion.Size = New System.Drawing.Size(42, 13)
        Me.LabelVersion.TabIndex = 0
        Me.LabelVersion.Text = "Version"
        Me.LabelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelCopyright
        '
        Me.LabelCopyright.AutoSize = True
        Me.LabelCopyright.Location = New System.Drawing.Point(125, 72)
        Me.LabelCopyright.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
        Me.LabelCopyright.MaximumSize = New System.Drawing.Size(0, 17)
        Me.LabelCopyright.Name = "LabelCopyright"
        Me.LabelCopyright.Size = New System.Drawing.Size(51, 13)
        Me.LabelCopyright.TabIndex = 0
        Me.LabelCopyright.Text = "Copyright"
        Me.LabelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelCompanyName
        '
        Me.LabelCompanyName.AutoSize = True
        Me.LabelCompanyName.Location = New System.Drawing.Point(125, 102)
        Me.LabelCompanyName.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
        Me.LabelCompanyName.MaximumSize = New System.Drawing.Size(0, 17)
        Me.LabelCompanyName.Name = "LabelCompanyName"
        Me.LabelCompanyName.Size = New System.Drawing.Size(82, 13)
        Me.LabelCompanyName.TabIndex = 0
        Me.LabelCompanyName.Text = "Company Name"
        Me.LabelCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBox_Description
        '
        Me.TextBox_Description.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox_Description.Location = New System.Drawing.Point(0, 0)
        Me.TextBox_Description.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.TextBox_Description.Multiline = True
        Me.TextBox_Description.Name = "TextBox_Description"
        Me.TextBox_Description.ReadOnly = True
        Me.TextBox_Description.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox_Description.Size = New System.Drawing.Size(326, 100)
        Me.TextBox_Description.TabIndex = 0
        Me.TextBox_Description.TabStop = False
        Me.TextBox_Description.Text = resources.GetString("TextBox_Description.Text")
        '
        'OKButton
        '
        Me.OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.OKButton.Location = New System.Drawing.Point(271, 254)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 0
        Me.OKButton.Text = "&OK"
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(13, 259)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(131, 13)
        Me.LinkLabel1.TabIndex = 1
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "http://www.bluemodel.org"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage_Description)
        Me.TabControl1.Controls.Add(Me.TabPage_License)
        Me.TabControl1.Controls.Add(Me.TabPage_Credits)
        Me.TabControl1.Location = New System.Drawing.Point(12, 122)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(334, 126)
        Me.TabControl1.TabIndex = 2
        '
        'TabPage_Description
        '
        Me.TabPage_Description.Controls.Add(Me.TextBox_Description)
        Me.TabPage_Description.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Description.Name = "TabPage_Description"
        Me.TabPage_Description.Size = New System.Drawing.Size(326, 100)
        Me.TabPage_Description.TabIndex = 0
        Me.TabPage_Description.Text = "Description"
        Me.TabPage_Description.UseVisualStyleBackColor = True
        '
        'TabPage_License
        '
        Me.TabPage_License.Controls.Add(Me.TextBox_License)
        Me.TabPage_License.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_License.Name = "TabPage_License"
        Me.TabPage_License.Size = New System.Drawing.Size(326, 100)
        Me.TabPage_License.TabIndex = 1
        Me.TabPage_License.Text = "License"
        Me.TabPage_License.UseVisualStyleBackColor = True
        '
        'TextBox_License
        '
        Me.TextBox_License.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox_License.Location = New System.Drawing.Point(0, 0)
        Me.TextBox_License.Multiline = True
        Me.TextBox_License.Name = "TextBox_License"
        Me.TextBox_License.ReadOnly = True
        Me.TextBox_License.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox_License.Size = New System.Drawing.Size(326, 100)
        Me.TextBox_License.TabIndex = 1
        Me.TextBox_License.Text = resources.GetString("TextBox_License.Text")
        '
        'TabPage_Credits
        '
        Me.TabPage_Credits.Controls.Add(Me.TextBox_Credits)
        Me.TabPage_Credits.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Credits.Name = "TabPage_Credits"
        Me.TabPage_Credits.Size = New System.Drawing.Size(326, 100)
        Me.TabPage_Credits.TabIndex = 2
        Me.TabPage_Credits.Text = "Credits"
        Me.TabPage_Credits.UseVisualStyleBackColor = True
        '
        'TextBox_Credits
        '
        Me.TextBox_Credits.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox_Credits.Location = New System.Drawing.Point(0, 0)
        Me.TextBox_Credits.Multiline = True
        Me.TextBox_Credits.Name = "TextBox_Credits"
        Me.TextBox_Credits.ReadOnly = True
        Me.TextBox_Credits.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox_Credits.Size = New System.Drawing.Size(326, 100)
        Me.TextBox_Credits.TabIndex = 0
        Me.TextBox_Credits.Text = resources.GetString("TextBox_Credits.Text")
        '
        'AboutBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.OKButton
        Me.ClientSize = New System.Drawing.Size(358, 289)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.LabelCompanyName)
        Me.Controls.Add(Me.LabelCopyright)
        Me.Controls.Add(Me.LabelVersion)
        Me.Controls.Add(Me.LabelProductName)
        Me.Controls.Add(Me.LogoPictureBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AboutBox"
        Me.Padding = New System.Windows.Forms.Padding(9)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About BlueM.Opt"
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage_Description.ResumeLayout(False)
        Me.TabPage_Description.PerformLayout()
        Me.TabPage_License.ResumeLayout(False)
        Me.TabPage_License.PerformLayout()
        Me.TabPage_Credits.ResumeLayout(False)
        Me.TabPage_Credits.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage_Description As System.Windows.Forms.TabPage
    Friend WithEvents TabPage_License As System.Windows.Forms.TabPage
    Friend WithEvents TextBox_License As System.Windows.Forms.TextBox
    Friend WithEvents TabPage_Credits As System.Windows.Forms.TabPage
    Friend WithEvents TextBox_Credits As System.Windows.Forms.TextBox

End Class
