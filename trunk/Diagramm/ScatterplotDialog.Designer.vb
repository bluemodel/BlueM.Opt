<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ScatterplotDialog
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ScatterplotDialog))
        Me.Button_OK = New System.Windows.Forms.Button
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.GroupBox_Auswahl = New System.Windows.Forms.GroupBox
        Me.LinkLabel_CheckAll = New System.Windows.Forms.LinkLabel
        Me.CheckedListBox_Auswahl = New System.Windows.Forms.CheckedListBox
        Me.CheckBox_SekPopOnly = New System.Windows.Forms.CheckBox
        Me.GroupBox_Options = New System.Windows.Forms.GroupBox
        Me.CheckBox_showIstWerte = New System.Windows.Forms.CheckBox
        Me.CheckBox_showStartValue = New System.Windows.Forms.CheckBox
        Me.CheckBox_showRef = New System.Windows.Forms.CheckBox
        Me.RadioButton_SolutionSpace = New System.Windows.Forms.RadioButton
        Me.RadioButton_DecisionSpace = New System.Windows.Forms.RadioButton
        Me.GroupBox_Raum = New System.Windows.Forms.GroupBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox_Auswahl.SuspendLayout()
        Me.GroupBox_Options.SuspendLayout()
        Me.GroupBox_Raum.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_OK
        '
        Me.Button_OK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_OK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button_OK.Location = New System.Drawing.Point(188, 275)
        Me.Button_OK.Name = "Button_OK"
        Me.Button_OK.Size = New System.Drawing.Size(67, 23)
        Me.Button_OK.TabIndex = 3
        Me.Button_OK.Text = "OK"
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_Cancel.Location = New System.Drawing.Point(261, 275)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(67, 23)
        Me.Button_Cancel.TabIndex = 4
        Me.Button_Cancel.Text = "Cancel"
        '
        'GroupBox_Auswahl
        '
        Me.GroupBox_Auswahl.Controls.Add(Me.LinkLabel_CheckAll)
        Me.GroupBox_Auswahl.Controls.Add(Me.CheckedListBox_Auswahl)
        Me.GroupBox_Auswahl.Location = New System.Drawing.Point(12, 64)
        Me.GroupBox_Auswahl.Name = "GroupBox_Auswahl"
        Me.GroupBox_Auswahl.Size = New System.Drawing.Size(164, 201)
        Me.GroupBox_Auswahl.TabIndex = 2
        Me.GroupBox_Auswahl.TabStop = False
        Me.GroupBox_Auswahl.Text = "Selection"
        '
        'LinkLabel_CheckAll
        '
        Me.LinkLabel_CheckAll.AutoSize = True
        Me.LinkLabel_CheckAll.Location = New System.Drawing.Point(6, 179)
        Me.LinkLabel_CheckAll.Name = "LinkLabel_CheckAll"
        Me.LinkLabel_CheckAll.Size = New System.Drawing.Size(50, 13)
        Me.LinkLabel_CheckAll.TabIndex = 1
        Me.LinkLabel_CheckAll.TabStop = True
        Me.LinkLabel_CheckAll.Text = "Select all"
        '
        'CheckedListBox_Auswahl
        '
        Me.CheckedListBox_Auswahl.CheckOnClick = True
        Me.CheckedListBox_Auswahl.FormattingEnabled = True
        Me.CheckedListBox_Auswahl.Location = New System.Drawing.Point(6, 19)
        Me.CheckedListBox_Auswahl.Name = "CheckedListBox_Auswahl"
        Me.CheckedListBox_Auswahl.Size = New System.Drawing.Size(148, 154)
        Me.CheckedListBox_Auswahl.TabIndex = 0
        '
        'CheckBox_SekPopOnly
        '
        Me.CheckBox_SekPopOnly.AutoSize = True
        Me.CheckBox_SekPopOnly.Checked = True
        Me.CheckBox_SekPopOnly.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox_SekPopOnly.Location = New System.Drawing.Point(9, 19)
        Me.CheckBox_SekPopOnly.Name = "CheckBox_SekPopOnly"
        Me.CheckBox_SekPopOnly.Size = New System.Drawing.Size(123, 17)
        Me.CheckBox_SekPopOnly.TabIndex = 0
        Me.CheckBox_SekPopOnly.Text = "Display sec pop only"
        Me.ToolTip1.SetToolTip(Me.CheckBox_SekPopOnly, "Display only the secondary population")
        Me.CheckBox_SekPopOnly.UseVisualStyleBackColor = True
        '
        'GroupBox_Options
        '
        Me.GroupBox_Options.Controls.Add(Me.CheckBox_showIstWerte)
        Me.GroupBox_Options.Controls.Add(Me.CheckBox_showStartValue)
        Me.GroupBox_Options.Controls.Add(Me.CheckBox_showRef)
        Me.GroupBox_Options.Controls.Add(Me.CheckBox_SekPopOnly)
        Me.GroupBox_Options.Location = New System.Drawing.Point(182, 64)
        Me.GroupBox_Options.Name = "GroupBox_Options"
        Me.GroupBox_Options.Size = New System.Drawing.Size(146, 201)
        Me.GroupBox_Options.TabIndex = 1
        Me.GroupBox_Options.TabStop = False
        Me.GroupBox_Options.Text = "Options"
        '
        'CheckBox_showIstWerte
        '
        Me.CheckBox_showIstWerte.AutoSize = True
        Me.CheckBox_showIstWerte.Location = New System.Drawing.Point(9, 66)
        Me.CheckBox_showIstWerte.Name = "CheckBox_showIstWerte"
        Me.CheckBox_showIstWerte.Size = New System.Drawing.Size(130, 17)
        Me.CheckBox_showIstWerte.TabIndex = 2
        Me.CheckBox_showIstWerte.Text = "Display current values"
        Me.ToolTip1.SetToolTip(Me.CheckBox_showIstWerte, "Displays the current values of the objective functions as areas")
        Me.CheckBox_showIstWerte.UseVisualStyleBackColor = True
        '
        'CheckBox_showStartValue
        '
        Me.CheckBox_showStartValue.AutoSize = True
        Me.CheckBox_showStartValue.Location = New System.Drawing.Point(9, 42)
        Me.CheckBox_showStartValue.Name = "CheckBox_showStartValue"
        Me.CheckBox_showStartValue.Size = New System.Drawing.Size(117, 17)
        Me.CheckBox_showStartValue.TabIndex = 1
        Me.CheckBox_showStartValue.Text = "Display start values"
        Me.ToolTip1.SetToolTip(Me.CheckBox_showStartValue, "Displays the solution that was evaluated using the start values")
        Me.CheckBox_showStartValue.UseVisualStyleBackColor = True
        '
        'CheckBox_showRef
        '
        Me.CheckBox_showRef.Location = New System.Drawing.Point(9, 89)
        Me.CheckBox_showRef.Name = "CheckBox_showRef"
        Me.CheckBox_showRef.Size = New System.Drawing.Size(128, 31)
        Me.CheckBox_showRef.TabIndex = 3
        Me.CheckBox_showRef.Text = "Display comparison result"
        Me.ToolTip1.SetToolTip(Me.CheckBox_showRef, "Displays the secondary population of the loaded comparison result")
        Me.CheckBox_showRef.UseVisualStyleBackColor = True
        '
        'RadioButton_SolutionSpace
        '
        Me.RadioButton_SolutionSpace.AutoSize = True
        Me.RadioButton_SolutionSpace.Location = New System.Drawing.Point(9, 19)
        Me.RadioButton_SolutionSpace.Name = "RadioButton_SolutionSpace"
        Me.RadioButton_SolutionSpace.Size = New System.Drawing.Size(95, 17)
        Me.RadioButton_SolutionSpace.TabIndex = 0
        Me.RadioButton_SolutionSpace.Text = "Solution space"
        Me.RadioButton_SolutionSpace.UseVisualStyleBackColor = True
        '
        'RadioButton_DecisionSpace
        '
        Me.RadioButton_DecisionSpace.AutoSize = True
        Me.RadioButton_DecisionSpace.Location = New System.Drawing.Point(179, 19)
        Me.RadioButton_DecisionSpace.Name = "RadioButton_DecisionSpace"
        Me.RadioButton_DecisionSpace.Size = New System.Drawing.Size(98, 17)
        Me.RadioButton_DecisionSpace.TabIndex = 1
        Me.RadioButton_DecisionSpace.Text = "Decision space"
        Me.RadioButton_DecisionSpace.UseVisualStyleBackColor = True
        '
        'GroupBox_Raum
        '
        Me.GroupBox_Raum.Controls.Add(Me.RadioButton_DecisionSpace)
        Me.GroupBox_Raum.Controls.Add(Me.RadioButton_SolutionSpace)
        Me.GroupBox_Raum.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_Raum.Name = "GroupBox_Raum"
        Me.GroupBox_Raum.Size = New System.Drawing.Size(316, 46)
        Me.GroupBox_Raum.TabIndex = 0
        Me.GroupBox_Raum.TabStop = False
        Me.GroupBox_Raum.Text = "Space"
        '
        'ScatterplotDialog
        '
        Me.AcceptButton = Me.Button_OK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Button_Cancel
        Me.ClientSize = New System.Drawing.Size(336, 307)
        Me.Controls.Add(Me.GroupBox_Raum)
        Me.Controls.Add(Me.GroupBox_Auswahl)
        Me.Controls.Add(Me.GroupBox_Options)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_OK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ScatterplotDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Scatterplot matrix"
        Me.GroupBox_Auswahl.ResumeLayout(False)
        Me.GroupBox_Auswahl.PerformLayout()
        Me.GroupBox_Options.ResumeLayout(False)
        Me.GroupBox_Options.PerformLayout()
        Me.GroupBox_Raum.ResumeLayout(False)
        Me.GroupBox_Raum.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LinkLabel_CheckAll As System.Windows.Forms.LinkLabel
    Private WithEvents GroupBox_Auswahl As System.Windows.Forms.GroupBox
    Private WithEvents GroupBox_Options As System.Windows.Forms.GroupBox
    Private WithEvents RadioButton_DecisionSpace As System.Windows.Forms.RadioButton
    Private WithEvents RadioButton_SolutionSpace As System.Windows.Forms.RadioButton
    Private WithEvents GroupBox_Raum As System.Windows.Forms.GroupBox
    Private WithEvents Button_OK As System.Windows.Forms.Button
    Private WithEvents Button_Cancel As System.Windows.Forms.Button
    Private WithEvents CheckBox_SekPopOnly As System.Windows.Forms.CheckBox
    Private WithEvents CheckBox_showRef As System.Windows.Forms.CheckBox
    Private WithEvents CheckedListBox_Auswahl As System.Windows.Forms.CheckedListBox
    Private WithEvents CheckBox_showIstWerte As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_showStartValue As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

End Class
