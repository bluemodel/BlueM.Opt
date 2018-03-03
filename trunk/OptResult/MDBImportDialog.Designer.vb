<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MDBImportDialog
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
        Dim GroupBox_Zielfunktionen As System.Windows.Forms.GroupBox
        Dim Label_XAchse As System.Windows.Forms.Label
        Dim Label_YAchse As System.Windows.Forms.Label
        Dim Label_ZAchse As System.Windows.Forms.Label
        Dim Label_SekPop As System.Windows.Forms.Label
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MDBImportDialog))
        Me.ListBox_ZieleX = New System.Windows.Forms.ListBox
        Me.ListBox_ZieleY = New System.Windows.Forms.ListBox
        Me.ListBox_ZieleZ = New System.Windows.Forms.ListBox
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.ComboBox_SekPop = New System.Windows.Forms.ComboBox
        Me.GroupBox_SekPop = New System.Windows.Forms.GroupBox
        Me.CheckBox_Hypervol = New System.Windows.Forms.CheckBox
        GroupBox_Zielfunktionen = New System.Windows.Forms.GroupBox
        Label_XAchse = New System.Windows.Forms.Label
        Label_YAchse = New System.Windows.Forms.Label
        Label_ZAchse = New System.Windows.Forms.Label
        Label_SekPop = New System.Windows.Forms.Label
        GroupBox_Zielfunktionen.SuspendLayout()
        Me.GroupBox_SekPop.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox_Zielfunktionen
        '
        GroupBox_Zielfunktionen.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        GroupBox_Zielfunktionen.Controls.Add(Label_XAchse)
        GroupBox_Zielfunktionen.Controls.Add(Label_YAchse)
        GroupBox_Zielfunktionen.Controls.Add(Label_ZAchse)
        GroupBox_Zielfunktionen.Controls.Add(Me.ListBox_ZieleX)
        GroupBox_Zielfunktionen.Controls.Add(Me.ListBox_ZieleY)
        GroupBox_Zielfunktionen.Controls.Add(Me.ListBox_ZieleZ)
        GroupBox_Zielfunktionen.Location = New System.Drawing.Point(12, 12)
        GroupBox_Zielfunktionen.Name = "GroupBox_Zielfunktionen"
        GroupBox_Zielfunktionen.Size = New System.Drawing.Size(418, 224)
        GroupBox_Zielfunktionen.TabIndex = 10
        GroupBox_Zielfunktionen.TabStop = False
        GroupBox_Zielfunktionen.Text = "Objectives (for main chart)"
        '
        'Label_XAchse
        '
        Label_XAchse.AutoSize = True
        Label_XAchse.Location = New System.Drawing.Point(6, 21)
        Label_XAchse.Name = "Label_XAchse"
        Label_XAchse.Size = New System.Drawing.Size(38, 13)
        Label_XAchse.TabIndex = 6
        Label_XAchse.Text = "X-axis:"
        '
        'Label_YAchse
        '
        Label_YAchse.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Label_YAchse.AutoSize = True
        Label_YAchse.Location = New System.Drawing.Point(141, 21)
        Label_YAchse.Name = "Label_YAchse"
        Label_YAchse.Size = New System.Drawing.Size(39, 13)
        Label_YAchse.TabIndex = 11
        Label_YAchse.Text = "Y-Axis:"
        '
        'Label_ZAchse
        '
        Label_ZAchse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Label_ZAchse.AutoSize = True
        Label_ZAchse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Label_ZAchse.Location = New System.Drawing.Point(277, 21)
        Label_ZAchse.Name = "Label_ZAchse"
        Label_ZAchse.Size = New System.Drawing.Size(85, 13)
        Label_ZAchse.TabIndex = 14
        Label_ZAchse.Text = "Z-Axis (optional):"
        '
        'ListBox_ZieleX
        '
        Me.ListBox_ZieleX.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ListBox_ZieleX.FormattingEnabled = True
        Me.ListBox_ZieleX.Location = New System.Drawing.Point(9, 41)
        Me.ListBox_ZieleX.Name = "ListBox_ZieleX"
        Me.ListBox_ZieleX.Size = New System.Drawing.Size(130, 173)
        Me.ListBox_ZieleX.TabIndex = 5
        '
        'ListBox_ZieleY
        '
        Me.ListBox_ZieleY.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox_ZieleY.FormattingEnabled = True
        Me.ListBox_ZieleY.Location = New System.Drawing.Point(144, 41)
        Me.ListBox_ZieleY.Name = "ListBox_ZieleY"
        Me.ListBox_ZieleY.Size = New System.Drawing.Size(130, 173)
        Me.ListBox_ZieleY.TabIndex = 10
        '
        'ListBox_ZieleZ
        '
        Me.ListBox_ZieleZ.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox_ZieleZ.FormattingEnabled = True
        Me.ListBox_ZieleZ.Location = New System.Drawing.Point(280, 41)
        Me.ListBox_ZieleZ.Name = "ListBox_ZieleZ"
        Me.ListBox_ZieleZ.Size = New System.Drawing.Size(130, 173)
        Me.ListBox_ZieleZ.TabIndex = 13
        '
        'Label_SekPop
        '
        Label_SekPop.AutoSize = True
        Label_SekPop.Location = New System.Drawing.Point(9, 22)
        Label_SekPop.Name = "Label_SekPop"
        Label_SekPop.Size = New System.Drawing.Size(44, 13)
        Label_SekPop.TabIndex = 14
        Label_SekPop.Text = "Display:"
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OK_Button.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OK_Button.Location = New System.Drawing.Point(290, 292)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(363, 292)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'ComboBox_SekPop
        '
        Me.ComboBox_SekPop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_SekPop.FormattingEnabled = True
        Me.ComboBox_SekPop.Items.AddRange(New Object() {"last", "none", "exclusively"})
        Me.ComboBox_SekPop.Location = New System.Drawing.Point(69, 19)
        Me.ComboBox_SekPop.Name = "ComboBox_SekPop"
        Me.ComboBox_SekPop.Size = New System.Drawing.Size(94, 21)
        Me.ComboBox_SekPop.TabIndex = 13
        '
        'GroupBox_SekPop
        '
        Me.GroupBox_SekPop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox_SekPop.Controls.Add(Me.CheckBox_Hypervol)
        Me.GroupBox_SekPop.Controls.Add(Label_SekPop)
        Me.GroupBox_SekPop.Controls.Add(Me.ComboBox_SekPop)
        Me.GroupBox_SekPop.Location = New System.Drawing.Point(12, 242)
        Me.GroupBox_SekPop.Name = "GroupBox_SekPop"
        Me.GroupBox_SekPop.Size = New System.Drawing.Size(175, 73)
        Me.GroupBox_SekPop.TabIndex = 15
        Me.GroupBox_SekPop.TabStop = False
        Me.GroupBox_SekPop.Text = "Secondary population"
        '
        'CheckBox_Hypervol
        '
        Me.CheckBox_Hypervol.AutoSize = True
        Me.CheckBox_Hypervol.Location = New System.Drawing.Point(9, 46)
        Me.CheckBox_Hypervol.Name = "CheckBox_Hypervol"
        Me.CheckBox_Hypervol.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.CheckBox_Hypervol.Size = New System.Drawing.Size(133, 17)
        Me.CheckBox_Hypervol.TabIndex = 15
        Me.CheckBox_Hypervol.Text = "Calculate hypervolume"
        Me.CheckBox_Hypervol.UseVisualStyleBackColor = True
        '
        'MDBImportDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(443, 327)
        Me.Controls.Add(Me.GroupBox_SekPop)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(GroupBox_Zielfunktionen)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MDBImportDialog"
        Me.Text = "Load optimization result"
        GroupBox_Zielfunktionen.ResumeLayout(False)
        GroupBox_Zielfunktionen.PerformLayout()
        Me.GroupBox_SekPop.ResumeLayout(False)
        Me.GroupBox_SekPop.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents OK_Button As System.Windows.Forms.Button
    Private WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents GroupBox_SekPop As System.Windows.Forms.GroupBox
    Public WithEvents ListBox_ZieleX As System.Windows.Forms.ListBox
    Public WithEvents ListBox_ZieleY As System.Windows.Forms.ListBox
    Public WithEvents ListBox_ZieleZ As System.Windows.Forms.ListBox
    Public WithEvents ComboBox_SekPop As System.Windows.Forms.ComboBox
    Public WithEvents CheckBox_Hypervol As System.Windows.Forms.CheckBox
End Class
