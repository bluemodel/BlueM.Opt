<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ScatterplotAbfrage
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ScatterplotAbfrage))
        Me.Label_XAchse = New System.Windows.Forms.Label
        Me.ListBox_OptZieleX = New System.Windows.Forms.ListBox
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.CheckBox_Scatterplot = New System.Windows.Forms.CheckBox
        Me.GroupBox_Hauptdiagramm = New System.Windows.Forms.GroupBox
        Me.CheckBox_Hauptdiagramm = New System.Windows.Forms.CheckBox
        Me.Label_YAchse = New System.Windows.Forms.Label
        Me.Label_ZAchse = New System.Windows.Forms.Label
        Me.ListBox_OptZieleY = New System.Windows.Forms.ListBox
        Me.ListBox_OptZieleZ = New System.Windows.Forms.ListBox
        Me.GroupBox_Scatterplot = New System.Windows.Forms.GroupBox
        Me.CheckBox_onlySekPop = New System.Windows.Forms.CheckBox
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox_Hauptdiagramm.SuspendLayout()
        Me.GroupBox_Scatterplot.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label_XAchse
        '
        Me.Label_XAchse.AutoSize = True
        Me.Label_XAchse.Location = New System.Drawing.Point(6, 38)
        Me.Label_XAchse.Name = "Label_XAchse"
        Me.Label_XAchse.Size = New System.Drawing.Size(50, 13)
        Me.Label_XAchse.TabIndex = 6
        Me.Label_XAchse.Text = "X-Achse:"
        '
        'ListBox_OptZieleX
        '
        Me.ListBox_OptZieleX.FormattingEnabled = True
        Me.ListBox_OptZieleX.Location = New System.Drawing.Point(9, 58)
        Me.ListBox_OptZieleX.Name = "ListBox_OptZieleX"
        Me.ListBox_OptZieleX.Size = New System.Drawing.Size(130, 186)
        Me.ListBox_OptZieleX.TabIndex = 5
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(282, 325)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 7
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
        Me.Cancel_Button.Text = "Abbrechen"
        '
        'CheckBox_Scatterplot
        '
        Me.CheckBox_Scatterplot.AutoSize = True
        Me.CheckBox_Scatterplot.Location = New System.Drawing.Point(9, 19)
        Me.CheckBox_Scatterplot.Name = "CheckBox_Scatterplot"
        Me.CheckBox_Scatterplot.Size = New System.Drawing.Size(123, 17)
        Me.CheckBox_Scatterplot.TabIndex = 8
        Me.CheckBox_Scatterplot.Text = "Scatterplot anzeigen"
        Me.CheckBox_Scatterplot.UseVisualStyleBackColor = True
        '
        'GroupBox_Hauptdiagramm
        '
        Me.GroupBox_Hauptdiagramm.Controls.Add(Me.CheckBox_Hauptdiagramm)
        Me.GroupBox_Hauptdiagramm.Controls.Add(Me.Label_XAchse)
        Me.GroupBox_Hauptdiagramm.Controls.Add(Me.Label_YAchse)
        Me.GroupBox_Hauptdiagramm.Controls.Add(Me.Label_ZAchse)
        Me.GroupBox_Hauptdiagramm.Controls.Add(Me.ListBox_OptZieleX)
        Me.GroupBox_Hauptdiagramm.Controls.Add(Me.ListBox_OptZieleY)
        Me.GroupBox_Hauptdiagramm.Controls.Add(Me.ListBox_OptZieleZ)
        Me.GroupBox_Hauptdiagramm.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_Hauptdiagramm.Name = "GroupBox_Hauptdiagramm"
        Me.GroupBox_Hauptdiagramm.Size = New System.Drawing.Size(418, 251)
        Me.GroupBox_Hauptdiagramm.TabIndex = 9
        Me.GroupBox_Hauptdiagramm.TabStop = False
        Me.GroupBox_Hauptdiagramm.Text = "Hauptdiagramm"
        '
        'CheckBox_Hauptdiagramm
        '
        Me.CheckBox_Hauptdiagramm.AutoSize = True
        Me.CheckBox_Hauptdiagramm.Checked = True
        Me.CheckBox_Hauptdiagramm.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox_Hauptdiagramm.Location = New System.Drawing.Point(9, 19)
        Me.CheckBox_Hauptdiagramm.Name = "CheckBox_Hauptdiagramm"
        Me.CheckBox_Hauptdiagramm.Size = New System.Drawing.Size(146, 17)
        Me.CheckBox_Hauptdiagramm.TabIndex = 12
        Me.CheckBox_Hauptdiagramm.Text = "Hauptdiagramm anzeigen"
        Me.CheckBox_Hauptdiagramm.UseVisualStyleBackColor = True
        '
        'Label_YAchse
        '
        Me.Label_YAchse.AutoSize = True
        Me.Label_YAchse.Location = New System.Drawing.Point(141, 38)
        Me.Label_YAchse.Name = "Label_YAchse"
        Me.Label_YAchse.Size = New System.Drawing.Size(50, 13)
        Me.Label_YAchse.TabIndex = 11
        Me.Label_YAchse.Text = "Y-Achse:"
        '
        'Label_ZAchse
        '
        Me.Label_ZAchse.AutoSize = True
        Me.Label_ZAchse.Location = New System.Drawing.Point(277, 38)
        Me.Label_ZAchse.Name = "Label_ZAchse"
        Me.Label_ZAchse.Size = New System.Drawing.Size(96, 13)
        Me.Label_ZAchse.TabIndex = 14
        Me.Label_ZAchse.Text = "Z-Achse (optional):"
        '
        'ListBox_OptZieleY
        '
        Me.ListBox_OptZieleY.FormattingEnabled = True
        Me.ListBox_OptZieleY.Location = New System.Drawing.Point(144, 58)
        Me.ListBox_OptZieleY.Name = "ListBox_OptZieleY"
        Me.ListBox_OptZieleY.Size = New System.Drawing.Size(130, 186)
        Me.ListBox_OptZieleY.TabIndex = 10
        '
        'ListBox_OptZieleZ
        '
        Me.ListBox_OptZieleZ.FormattingEnabled = True
        Me.ListBox_OptZieleZ.Location = New System.Drawing.Point(280, 58)
        Me.ListBox_OptZieleZ.Name = "ListBox_OptZieleZ"
        Me.ListBox_OptZieleZ.Size = New System.Drawing.Size(130, 186)
        Me.ListBox_OptZieleZ.TabIndex = 13
        '
        'GroupBox_Scatterplot
        '
        Me.GroupBox_Scatterplot.Controls.Add(Me.CheckBox_Scatterplot)
        Me.GroupBox_Scatterplot.Location = New System.Drawing.Point(12, 269)
        Me.GroupBox_Scatterplot.Name = "GroupBox_Scatterplot"
        Me.GroupBox_Scatterplot.Size = New System.Drawing.Size(418, 46)
        Me.GroupBox_Scatterplot.TabIndex = 10
        Me.GroupBox_Scatterplot.TabStop = False
        Me.GroupBox_Scatterplot.Text = "Scatterplot"
        '
        'CheckBox_onlySekPop
        '
        Me.CheckBox_onlySekPop.AutoSize = True
        Me.CheckBox_onlySekPop.Location = New System.Drawing.Point(21, 332)
        Me.CheckBox_onlySekPop.Name = "CheckBox_onlySekPop"
        Me.CheckBox_onlySekPop.Size = New System.Drawing.Size(147, 17)
        Me.CheckBox_onlySekPop.TabIndex = 11
        Me.CheckBox_onlySekPop.Text = "nur sekundäre Population"
        Me.CheckBox_onlySekPop.UseVisualStyleBackColor = True
        '
        'ScatterplotAbfrage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(440, 366)
        Me.Controls.Add(Me.CheckBox_onlySekPop)
        Me.Controls.Add(Me.GroupBox_Scatterplot)
        Me.Controls.Add(Me.GroupBox_Hauptdiagramm)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ScatterplotAbfrage"
        Me.Text = "Scatterplot"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupBox_Hauptdiagramm.ResumeLayout(False)
        Me.GroupBox_Hauptdiagramm.PerformLayout()
        Me.GroupBox_Scatterplot.ResumeLayout(False)
        Me.GroupBox_Scatterplot.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label_XAchse As System.Windows.Forms.Label
    Friend WithEvents ListBox_OptZieleX As System.Windows.Forms.ListBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents GroupBox_Hauptdiagramm As System.Windows.Forms.GroupBox
    Friend WithEvents ListBox_OptZieleY As System.Windows.Forms.ListBox
    Friend WithEvents Label_YAchse As System.Windows.Forms.Label
    Friend WithEvents GroupBox_Scatterplot As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBox_Scatterplot As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Hauptdiagramm As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_onlySekPop As System.Windows.Forms.CheckBox
    Friend WithEvents Label_ZAchse As System.Windows.Forms.Label
    Friend WithEvents ListBox_OptZieleZ As System.Windows.Forms.ListBox
End Class
