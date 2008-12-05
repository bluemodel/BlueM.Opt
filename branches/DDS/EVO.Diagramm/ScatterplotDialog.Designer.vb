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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ScatterplotDialog))
        Me.Button_OK = New System.Windows.Forms.Button
        Me.Button_Cancel = New System.Windows.Forms.Button
        Me.GroupBox_Zielauswahl = New System.Windows.Forms.GroupBox
        Me.LinkLabel_CheckAll = New System.Windows.Forms.LinkLabel
        Me.CheckedListBox_Ziele = New System.Windows.Forms.CheckedListBox
        Me.CheckBox_SekPopOnly = New System.Windows.Forms.CheckBox
        Me.GroupBox_SekPop = New System.Windows.Forms.GroupBox
        Me.GroupBox_Ref = New System.Windows.Forms.GroupBox
        Me.CheckBox_showRef = New System.Windows.Forms.CheckBox
        Me.GroupBox_Zielauswahl.SuspendLayout()
        Me.GroupBox_SekPop.SuspendLayout()
        Me.GroupBox_Ref.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_OK
        '
        Me.Button_OK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Button_OK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button_OK.Location = New System.Drawing.Point(188, 190)
        Me.Button_OK.Name = "Button_OK"
        Me.Button_OK.Size = New System.Drawing.Size(67, 23)
        Me.Button_OK.TabIndex = 0
        Me.Button_OK.Text = "OK"
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_Cancel.Location = New System.Drawing.Point(261, 190)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(67, 23)
        Me.Button_Cancel.TabIndex = 1
        Me.Button_Cancel.Text = "Abbrechen"
        '
        'GroupBox_Zielauswahl
        '
        Me.GroupBox_Zielauswahl.Controls.Add(Me.LinkLabel_CheckAll)
        Me.GroupBox_Zielauswahl.Controls.Add(Me.CheckedListBox_Ziele)
        Me.GroupBox_Zielauswahl.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_Zielauswahl.Name = "GroupBox_Zielauswahl"
        Me.GroupBox_Zielauswahl.Size = New System.Drawing.Size(164, 201)
        Me.GroupBox_Zielauswahl.TabIndex = 1
        Me.GroupBox_Zielauswahl.TabStop = False
        Me.GroupBox_Zielauswahl.Text = "Zielauswahl"
        '
        'LinkLabel_CheckAll
        '
        Me.LinkLabel_CheckAll.AutoSize = True
        Me.LinkLabel_CheckAll.Location = New System.Drawing.Point(6, 179)
        Me.LinkLabel_CheckAll.Name = "LinkLabel_CheckAll"
        Me.LinkLabel_CheckAll.Size = New System.Drawing.Size(78, 13)
        Me.LinkLabel_CheckAll.TabIndex = 1
        Me.LinkLabel_CheckAll.TabStop = True
        Me.LinkLabel_CheckAll.Text = "Alle auswählen"
        '
        'CheckedListBox_Ziele
        '
        Me.CheckedListBox_Ziele.CheckOnClick = True
        Me.CheckedListBox_Ziele.FormattingEnabled = True
        Me.CheckedListBox_Ziele.Location = New System.Drawing.Point(6, 19)
        Me.CheckedListBox_Ziele.Name = "CheckedListBox_Ziele"
        Me.CheckedListBox_Ziele.Size = New System.Drawing.Size(148, 154)
        Me.CheckedListBox_Ziele.TabIndex = 0
        '
        'CheckBox_SekPopOnly
        '
        Me.CheckBox_SekPopOnly.AutoSize = True
        Me.CheckBox_SekPopOnly.Checked = True
        Me.CheckBox_SekPopOnly.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox_SekPopOnly.Location = New System.Drawing.Point(9, 19)
        Me.CheckBox_SekPopOnly.Name = "CheckBox_SekPopOnly"
        Me.CheckBox_SekPopOnly.Size = New System.Drawing.Size(128, 17)
        Me.CheckBox_SekPopOnly.TabIndex = 2
        Me.CheckBox_SekPopOnly.Text = "nur SekPop anzeigen"
        Me.CheckBox_SekPopOnly.UseVisualStyleBackColor = True
        '
        'GroupBox_SekPop
        '
        Me.GroupBox_SekPop.Controls.Add(Me.CheckBox_SekPopOnly)
        Me.GroupBox_SekPop.Location = New System.Drawing.Point(182, 12)
        Me.GroupBox_SekPop.Name = "GroupBox_SekPop"
        Me.GroupBox_SekPop.Size = New System.Drawing.Size(146, 45)
        Me.GroupBox_SekPop.TabIndex = 3
        Me.GroupBox_SekPop.TabStop = False
        Me.GroupBox_SekPop.Text = "Sekundäre Population"
        '
        'GroupBox_Ref
        '
        Me.GroupBox_Ref.Controls.Add(Me.CheckBox_showRef)
        Me.GroupBox_Ref.Location = New System.Drawing.Point(182, 64)
        Me.GroupBox_Ref.Name = "GroupBox_Ref"
        Me.GroupBox_Ref.Size = New System.Drawing.Size(146, 57)
        Me.GroupBox_Ref.TabIndex = 4
        Me.GroupBox_Ref.TabStop = False
        Me.GroupBox_Ref.Text = "Vergleichsergebnis"
        '
        'CheckBox_showRef
        '
        Me.CheckBox_showRef.Location = New System.Drawing.Point(9, 20)
        Me.CheckBox_showRef.Name = "CheckBox_showRef"
        Me.CheckBox_showRef.Size = New System.Drawing.Size(128, 31)
        Me.CheckBox_showRef.TabIndex = 0
        Me.CheckBox_showRef.Text = "Vergleichsergebnis anzeigen"
        Me.CheckBox_showRef.UseVisualStyleBackColor = True
        '
        'ScatterplotDialog
        '
        Me.AcceptButton = Me.Button_OK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Button_Cancel
        Me.ClientSize = New System.Drawing.Size(336, 219)
        Me.Controls.Add(Me.GroupBox_Zielauswahl)
        Me.Controls.Add(Me.GroupBox_SekPop)
        Me.Controls.Add(Me.GroupBox_Ref)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Button_OK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ScatterplotDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Scatterplot-Matrix"
        Me.GroupBox_Zielauswahl.ResumeLayout(False)
        Me.GroupBox_Zielauswahl.PerformLayout()
        Me.GroupBox_SekPop.ResumeLayout(False)
        Me.GroupBox_SekPop.PerformLayout()
        Me.GroupBox_Ref.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button_OK As System.Windows.Forms.Button
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents GroupBox_Zielauswahl As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox_SekPop As System.Windows.Forms.GroupBox
    Friend WithEvents LinkLabel_CheckAll As System.Windows.Forms.LinkLabel
    Public WithEvents GroupBox_Ref As System.Windows.Forms.GroupBox
    Public WithEvents CheckBox_SekPopOnly As System.Windows.Forms.CheckBox
    Public WithEvents CheckBox_showRef As System.Windows.Forms.CheckBox
    Public WithEvents CheckedListBox_Ziele As System.Windows.Forms.CheckedListBox

End Class
