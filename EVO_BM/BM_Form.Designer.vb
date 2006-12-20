<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BM_Form
    Inherits System.Windows.Forms.UserControl

    'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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
        Me.GroupBox_BM = New System.Windows.Forms.GroupBox
        Me.Label_ReadSysResult = New System.Windows.Forms.Label
        Me.GroupBox_Parameter = New System.Windows.Forms.GroupBox
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.Label_Parameter = New System.Windows.Forms.Label
        Me.Button_Parameter = New System.Windows.Forms.Button
        Me.GroupBox_OptModus = New System.Windows.Forms.GroupBox
        Me.Label_Pegel = New System.Windows.Forms.Label
        Me.Radio_Optimierung = New System.Windows.Forms.RadioButton
        Me.Radio_Autokalibrierung = New System.Windows.Forms.RadioButton
        Me.TextBox_Pegel = New System.Windows.Forms.TextBox
        Me.Button_Pegel = New System.Windows.Forms.Button
        Me.Label_EXE = New System.Windows.Forms.Label
        Me.TextBox_EXE = New System.Windows.Forms.TextBox
        Me.Button_Exe = New System.Windows.Forms.Button
        Me.Button_Datensatz = New System.Windows.Forms.Button
        Me.Label_Datensatz = New System.Windows.Forms.Label
        Me.TextBox_Datensatz = New System.Windows.Forms.TextBox
        Me.OpenFile_Datensatz = New System.Windows.Forms.OpenFileDialog
        Me.OpenFile_EXE = New System.Windows.Forms.OpenFileDialog
        Me.OpenFile_Pegel = New System.Windows.Forms.OpenFileDialog
        Me.Button_ReadSys = New System.Windows.Forms.Button
        Me.GroupBox_BM.SuspendLayout()
        Me.GroupBox_Parameter.SuspendLayout()
        Me.GroupBox_OptModus.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox_BM
        '
        Me.GroupBox_BM.Controls.Add(Me.Button_ReadSys)
        Me.GroupBox_BM.Controls.Add(Me.Label_ReadSysResult)
        Me.GroupBox_BM.Controls.Add(Me.GroupBox_Parameter)
        Me.GroupBox_BM.Controls.Add(Me.GroupBox_OptModus)
        Me.GroupBox_BM.Controls.Add(Me.Label_EXE)
        Me.GroupBox_BM.Controls.Add(Me.TextBox_EXE)
        Me.GroupBox_BM.Controls.Add(Me.Button_Exe)
        Me.GroupBox_BM.Controls.Add(Me.Button_Datensatz)
        Me.GroupBox_BM.Controls.Add(Me.Label_Datensatz)
        Me.GroupBox_BM.Controls.Add(Me.TextBox_Datensatz)
        Me.GroupBox_BM.Location = New System.Drawing.Point(3, 0)
        Me.GroupBox_BM.Name = "GroupBox_BM"
        Me.GroupBox_BM.Size = New System.Drawing.Size(288, 429)
        Me.GroupBox_BM.TabIndex = 35
        Me.GroupBox_BM.TabStop = False
        Me.GroupBox_BM.Text = "BlauesModell"
        '
        'Label_ReadSysResult
        '
        Me.Label_ReadSysResult.AutoSize = True
        Me.Label_ReadSysResult.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_ReadSysResult.Location = New System.Drawing.Point(96, 77)
        Me.Label_ReadSysResult.Name = "Label_ReadSysResult"
        Me.Label_ReadSysResult.Size = New System.Drawing.Size(0, 13)
        Me.Label_ReadSysResult.TabIndex = 32
        '
        'GroupBox_Parameter
        '
        Me.GroupBox_Parameter.Controls.Add(Me.TableLayoutPanel1)
        Me.GroupBox_Parameter.Controls.Add(Me.Label_Parameter)
        Me.GroupBox_Parameter.Controls.Add(Me.Button_Parameter)
        Me.GroupBox_Parameter.Location = New System.Drawing.Point(7, 188)
        Me.GroupBox_Parameter.Name = "GroupBox_Parameter"
        Me.GroupBox_Parameter.Size = New System.Drawing.Size(275, 242)
        Me.GroupBox_Parameter.TabIndex = 31
        Me.GroupBox_Parameter.TabStop = False
        Me.GroupBox_Parameter.Text = "Parameter"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 47)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(255, 37)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'Label_Parameter
        '
        Me.Label_Parameter.AutoSize = True
        Me.Label_Parameter.Location = New System.Drawing.Point(9, 20)
        Me.Label_Parameter.Name = "Label_Parameter"
        Me.Label_Parameter.Size = New System.Drawing.Size(58, 13)
        Me.Label_Parameter.TabIndex = 1
        Me.Label_Parameter.Text = "Parameter:"
        '
        'Button_Parameter
        '
        Me.Button_Parameter.Location = New System.Drawing.Point(73, 15)
        Me.Button_Parameter.Name = "Button_Parameter"
        Me.Button_Parameter.Size = New System.Drawing.Size(75, 23)
        Me.Button_Parameter.TabIndex = 0
        Me.Button_Parameter.Text = "bearbeiten"
        Me.Button_Parameter.UseVisualStyleBackColor = True
        '
        'GroupBox_OptModus
        '
        Me.GroupBox_OptModus.Controls.Add(Me.Label_Pegel)
        Me.GroupBox_OptModus.Controls.Add(Me.Radio_Optimierung)
        Me.GroupBox_OptModus.Controls.Add(Me.Radio_Autokalibrierung)
        Me.GroupBox_OptModus.Controls.Add(Me.TextBox_Pegel)
        Me.GroupBox_OptModus.Controls.Add(Me.Button_Pegel)
        Me.GroupBox_OptModus.Location = New System.Drawing.Point(7, 101)
        Me.GroupBox_OptModus.Name = "GroupBox_OptModus"
        Me.GroupBox_OptModus.Size = New System.Drawing.Size(275, 80)
        Me.GroupBox_OptModus.TabIndex = 30
        Me.GroupBox_OptModus.TabStop = False
        Me.GroupBox_OptModus.Text = "Optimierungsmodus"
        '
        'Label_Pegel
        '
        Me.Label_Pegel.AutoSize = True
        Me.Label_Pegel.Enabled = False
        Me.Label_Pegel.Location = New System.Drawing.Point(6, 50)
        Me.Label_Pegel.Name = "Label_Pegel"
        Me.Label_Pegel.Size = New System.Drawing.Size(51, 13)
        Me.Label_Pegel.TabIndex = 2
        Me.Label_Pegel.Text = "Zeitreihe:"
        '
        'Radio_Optimierung
        '
        Me.Radio_Optimierung.AutoSize = True
        Me.Radio_Optimierung.Location = New System.Drawing.Point(149, 19)
        Me.Radio_Optimierung.Name = "Radio_Optimierung"
        Me.Radio_Optimierung.Size = New System.Drawing.Size(81, 17)
        Me.Radio_Optimierung.TabIndex = 1
        Me.Radio_Optimierung.TabStop = True
        Me.Radio_Optimierung.Text = "Optimierung"
        Me.Radio_Optimierung.UseVisualStyleBackColor = True
        '
        'Radio_Autokalibrierung
        '
        Me.Radio_Autokalibrierung.AutoSize = True
        Me.Radio_Autokalibrierung.Location = New System.Drawing.Point(41, 19)
        Me.Radio_Autokalibrierung.Name = "Radio_Autokalibrierung"
        Me.Radio_Autokalibrierung.Size = New System.Drawing.Size(101, 17)
        Me.Radio_Autokalibrierung.TabIndex = 0
        Me.Radio_Autokalibrierung.TabStop = True
        Me.Radio_Autokalibrierung.Text = "Autokalibrierung"
        Me.Radio_Autokalibrierung.UseVisualStyleBackColor = True
        '
        'TextBox_Pegel
        '
        Me.TextBox_Pegel.Enabled = False
        Me.TextBox_Pegel.Location = New System.Drawing.Point(63, 47)
        Me.TextBox_Pegel.Name = "TextBox_Pegel"
        Me.TextBox_Pegel.Size = New System.Drawing.Size(174, 20)
        Me.TextBox_Pegel.TabIndex = 24
        Me.TextBox_Pegel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Button_Pegel
        '
        Me.Button_Pegel.Enabled = False
        Me.Button_Pegel.Location = New System.Drawing.Point(243, 45)
        Me.Button_Pegel.Name = "Button_Pegel"
        Me.Button_Pegel.Size = New System.Drawing.Size(24, 23)
        Me.Button_Pegel.TabIndex = 26
        Me.Button_Pegel.Text = "..."
        Me.Button_Pegel.UseVisualStyleBackColor = True
        '
        'Label_EXE
        '
        Me.Label_EXE.AutoSize = True
        Me.Label_EXE.Location = New System.Drawing.Point(6, 24)
        Me.Label_EXE.Name = "Label_EXE"
        Me.Label_EXE.Size = New System.Drawing.Size(41, 13)
        Me.Label_EXE.TabIndex = 29
        Me.Label_EXE.Text = "Modell:"
        '
        'TextBox_EXE
        '
        Me.TextBox_EXE.Location = New System.Drawing.Point(70, 21)
        Me.TextBox_EXE.Name = "TextBox_EXE"
        Me.TextBox_EXE.Size = New System.Drawing.Size(182, 20)
        Me.TextBox_EXE.TabIndex = 28
        '
        'Button_Exe
        '
        Me.Button_Exe.Location = New System.Drawing.Point(258, 19)
        Me.Button_Exe.Name = "Button_Exe"
        Me.Button_Exe.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Button_Exe.Size = New System.Drawing.Size(24, 23)
        Me.Button_Exe.TabIndex = 27
        Me.Button_Exe.Text = "..."
        Me.Button_Exe.UseVisualStyleBackColor = True
        '
        'Button_Datensatz
        '
        Me.Button_Datensatz.Location = New System.Drawing.Point(258, 43)
        Me.Button_Datensatz.Name = "Button_Datensatz"
        Me.Button_Datensatz.Size = New System.Drawing.Size(24, 23)
        Me.Button_Datensatz.TabIndex = 26
        Me.Button_Datensatz.Text = "..."
        Me.Button_Datensatz.UseVisualStyleBackColor = True
        '
        'Label_Datensatz
        '
        Me.Label_Datensatz.AutoSize = True
        Me.Label_Datensatz.Location = New System.Drawing.Point(6, 48)
        Me.Label_Datensatz.Name = "Label_Datensatz"
        Me.Label_Datensatz.Size = New System.Drawing.Size(58, 13)
        Me.Label_Datensatz.TabIndex = 25
        Me.Label_Datensatz.Text = "Datensatz:"
        '
        'TextBox_Datensatz
        '
        Me.TextBox_Datensatz.Location = New System.Drawing.Point(70, 45)
        Me.TextBox_Datensatz.Name = "TextBox_Datensatz"
        Me.TextBox_Datensatz.Size = New System.Drawing.Size(182, 20)
        Me.TextBox_Datensatz.TabIndex = 24
        Me.TextBox_Datensatz.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'OpenFile_Datensatz
        '
        Me.OpenFile_Datensatz.Filter = "ALL-Dateien|*.ALL"
        Me.OpenFile_Datensatz.Title = "Datensatz auswählen"
        '
        'OpenFile_EXE
        '
        Me.OpenFile_EXE.DereferenceLinks = False
        Me.OpenFile_EXE.Filter = "Anwendung|*.exe"
        Me.OpenFile_EXE.Title = "BlauesModell.exe auswählen"
        '
        'OpenFile_Pegel
        '
        Me.OpenFile_Pegel.Filter = "ZRE-Dateien|*.zre"
        Me.OpenFile_Pegel.Title = "Zeitreihe auswählen"
        '
        'Button_ReadSys
        '
        Me.Button_ReadSys.Location = New System.Drawing.Point(10, 72)
        Me.Button_ReadSys.Name = "Button_ReadSys"
        Me.Button_ReadSys.Size = New System.Drawing.Size(75, 23)
        Me.Button_ReadSys.TabIndex = 33
        Me.Button_ReadSys.Text = "SYS lesen"
        Me.Button_ReadSys.UseVisualStyleBackColor = True
        '
        'BM_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox_BM)
        Me.Name = "BM_Form"
        Me.Size = New System.Drawing.Size(297, 472)
        Me.GroupBox_BM.ResumeLayout(False)
        Me.GroupBox_BM.PerformLayout()
        Me.GroupBox_Parameter.ResumeLayout(False)
        Me.GroupBox_Parameter.PerformLayout()
        Me.GroupBox_OptModus.ResumeLayout(False)
        Me.GroupBox_OptModus.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox_BM As System.Windows.Forms.GroupBox
    Friend WithEvents Label_EXE As System.Windows.Forms.Label
    Friend WithEvents TextBox_EXE As System.Windows.Forms.TextBox
    Friend WithEvents Button_Exe As System.Windows.Forms.Button
    Friend WithEvents Button_Datensatz As System.Windows.Forms.Button
    Friend WithEvents Label_Datensatz As System.Windows.Forms.Label
    Friend WithEvents TextBox_Datensatz As System.Windows.Forms.TextBox
    Friend WithEvents OpenFile_Datensatz As System.Windows.Forms.OpenFileDialog
    Friend WithEvents OpenFile_EXE As System.Windows.Forms.OpenFileDialog
    Friend WithEvents GroupBox_OptModus As System.Windows.Forms.GroupBox
    Friend WithEvents Label_Pegel As System.Windows.Forms.Label
    Friend WithEvents Radio_Optimierung As System.Windows.Forms.RadioButton
    Friend WithEvents Radio_Autokalibrierung As System.Windows.Forms.RadioButton
    Friend WithEvents TextBox_Pegel As System.Windows.Forms.TextBox
    Friend WithEvents Button_Pegel As System.Windows.Forms.Button
    Friend WithEvents OpenFile_Pegel As System.Windows.Forms.OpenFileDialog
    Friend WithEvents GroupBox_Parameter As System.Windows.Forms.GroupBox
    Friend WithEvents Label_Parameter As System.Windows.Forms.Label
    Friend WithEvents Button_Parameter As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label_ReadSysResult As System.Windows.Forms.Label
    Friend WithEvents Button_ReadSys As System.Windows.Forms.Button

End Class
