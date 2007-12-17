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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MDBImportDialog))
        Me.GroupBox_Hauptdiagramm = New System.Windows.Forms.GroupBox
        Me.Label_XAchse = New System.Windows.Forms.Label
        Me.Label_YAchse = New System.Windows.Forms.Label
        Me.Label_ZAchse = New System.Windows.Forms.Label
        Me.ListBox_OptZieleX = New System.Windows.Forms.ListBox
        Me.ListBox_OptZieleY = New System.Windows.Forms.ListBox
        Me.ListBox_OptZieleZ = New System.Windows.Forms.ListBox
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.ComboBox_SekPop = New System.Windows.Forms.ComboBox
        Me.Label_SekPop = New System.Windows.Forms.Label
        Me.GroupBox_Hauptdiagramm.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox_Hauptdiagramm
        '
        Me.GroupBox_Hauptdiagramm.Controls.Add(Me.Label_XAchse)
        Me.GroupBox_Hauptdiagramm.Controls.Add(Me.Label_YAchse)
        Me.GroupBox_Hauptdiagramm.Controls.Add(Me.Label_ZAchse)
        Me.GroupBox_Hauptdiagramm.Controls.Add(Me.ListBox_OptZieleX)
        Me.GroupBox_Hauptdiagramm.Controls.Add(Me.ListBox_OptZieleY)
        Me.GroupBox_Hauptdiagramm.Controls.Add(Me.ListBox_OptZieleZ)
        Me.GroupBox_Hauptdiagramm.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_Hauptdiagramm.Name = "GroupBox_Hauptdiagramm"
        Me.GroupBox_Hauptdiagramm.Size = New System.Drawing.Size(418, 236)
        Me.GroupBox_Hauptdiagramm.TabIndex = 10
        Me.GroupBox_Hauptdiagramm.TabStop = False
        Me.GroupBox_Hauptdiagramm.Text = "Hauptdiagramm"
        '
        'Label_XAchse
        '
        Me.Label_XAchse.AutoSize = True
        Me.Label_XAchse.Location = New System.Drawing.Point(6, 21)
        Me.Label_XAchse.Name = "Label_XAchse"
        Me.Label_XAchse.Size = New System.Drawing.Size(50, 13)
        Me.Label_XAchse.TabIndex = 6
        Me.Label_XAchse.Text = "X-Achse:"
        '
        'Label_YAchse
        '
        Me.Label_YAchse.AutoSize = True
        Me.Label_YAchse.Location = New System.Drawing.Point(141, 21)
        Me.Label_YAchse.Name = "Label_YAchse"
        Me.Label_YAchse.Size = New System.Drawing.Size(50, 13)
        Me.Label_YAchse.TabIndex = 11
        Me.Label_YAchse.Text = "Y-Achse:"
        '
        'Label_ZAchse
        '
        Me.Label_ZAchse.AutoSize = True
        Me.Label_ZAchse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ZAchse.Location = New System.Drawing.Point(277, 21)
        Me.Label_ZAchse.Name = "Label_ZAchse"
        Me.Label_ZAchse.Size = New System.Drawing.Size(96, 13)
        Me.Label_ZAchse.TabIndex = 14
        Me.Label_ZAchse.Text = "Z-Achse (optional):"
        '
        'ListBox_OptZieleX
        '
        Me.ListBox_OptZieleX.FormattingEnabled = True
        Me.ListBox_OptZieleX.Location = New System.Drawing.Point(9, 41)
        Me.ListBox_OptZieleX.Name = "ListBox_OptZieleX"
        Me.ListBox_OptZieleX.Size = New System.Drawing.Size(130, 186)
        Me.ListBox_OptZieleX.TabIndex = 5
        '
        'ListBox_OptZieleY
        '
        Me.ListBox_OptZieleY.FormattingEnabled = True
        Me.ListBox_OptZieleY.Location = New System.Drawing.Point(144, 41)
        Me.ListBox_OptZieleY.Name = "ListBox_OptZieleY"
        Me.ListBox_OptZieleY.Size = New System.Drawing.Size(130, 186)
        Me.ListBox_OptZieleY.TabIndex = 10
        '
        'ListBox_OptZieleZ
        '
        Me.ListBox_OptZieleZ.FormattingEnabled = True
        Me.ListBox_OptZieleZ.Location = New System.Drawing.Point(280, 41)
        Me.ListBox_OptZieleZ.Name = "ListBox_OptZieleZ"
        Me.ListBox_OptZieleZ.Size = New System.Drawing.Size(130, 186)
        Me.ListBox_OptZieleZ.TabIndex = 13
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OK_Button.Location = New System.Drawing.Point(290, 254)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(363, 254)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Abbrechen"
        '
        'ComboBox_SekPop
        '
        Me.ComboBox_SekPop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_SekPop.FormattingEnabled = True
        Me.ComboBox_SekPop.Items.AddRange(New Object() {"letzte", "keine", "ausschließlich"})
        Me.ComboBox_SekPop.Location = New System.Drawing.Point(169, 256)
        Me.ComboBox_SekPop.Name = "ComboBox_SekPop"
        Me.ComboBox_SekPop.Size = New System.Drawing.Size(86, 21)
        Me.ComboBox_SekPop.TabIndex = 13
        '
        'Label_SekPop
        '
        Me.Label_SekPop.AutoSize = True
        Me.Label_SekPop.Location = New System.Drawing.Point(12, 259)
        Me.Label_SekPop.Name = "Label_SekPop"
        Me.Label_SekPop.Size = New System.Drawing.Size(151, 13)
        Me.Label_SekPop.TabIndex = 14
        Me.Label_SekPop.Text = "Sekundärpopulation anzeigen:"
        '
        'MDBImportDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(436, 281)
        Me.Controls.Add(Me.Label_SekPop)
        Me.Controls.Add(Me.ComboBox_SekPop)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.GroupBox_Hauptdiagramm)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MDBImportDialog"
        Me.Text = "Optimierungsergebnis laden"
        Me.GroupBox_Hauptdiagramm.ResumeLayout(False)
        Me.GroupBox_Hauptdiagramm.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox_Hauptdiagramm As System.Windows.Forms.GroupBox
    Friend WithEvents Label_XAchse As System.Windows.Forms.Label
    Friend WithEvents Label_YAchse As System.Windows.Forms.Label
    Friend WithEvents Label_ZAchse As System.Windows.Forms.Label
    Friend WithEvents ListBox_OptZieleX As System.Windows.Forms.ListBox
    Friend WithEvents ListBox_OptZieleY As System.Windows.Forms.ListBox
    Friend WithEvents ListBox_OptZieleZ As System.Windows.Forms.ListBox
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents ComboBox_SekPop As System.Windows.Forms.ComboBox
    Friend WithEvents Label_SekPop As System.Windows.Forms.Label
End Class
