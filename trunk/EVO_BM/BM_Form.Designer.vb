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
        Me.GroupBox_Parameter = New System.Windows.Forms.GroupBox
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.Label_Parameter = New System.Windows.Forms.Label
        Me.TextBox_OptParameter_Pfad = New System.Windows.Forms.TextBox
        Me.Button_OptParameter = New System.Windows.Forms.Button
        Me.GroupBox_Ziel = New System.Windows.Forms.GroupBox
        Me.Label_OptZielReihe = New System.Windows.Forms.Label
        Me.Label_OptZielWert = New System.Windows.Forms.Label
        Me.TextBox_OptZielWert_Pfad = New System.Windows.Forms.TextBox
        Me.Button_OptZielWert = New System.Windows.Forms.Button
        Me.Label_EXE = New System.Windows.Forms.Label
        Me.TextBox_EXE = New System.Windows.Forms.TextBox
        Me.Button_Exe = New System.Windows.Forms.Button
        Me.Button_Datensatz = New System.Windows.Forms.Button
        Me.Label_Datensatz = New System.Windows.Forms.Label
        Me.TextBox_Datensatz = New System.Windows.Forms.TextBox
        Me.OpenFile_Datensatz = New System.Windows.Forms.OpenFileDialog
        Me.OpenFile_EXE = New System.Windows.Forms.OpenFileDialog
        Me.OpenFile_OptZielWert = New System.Windows.Forms.OpenFileDialog
        Me.OpenFile_OptParameter = New System.Windows.Forms.OpenFileDialog
        Me.TextBox_OptZielReihe_Pfad = New System.Windows.Forms.MaskedTextBox
        Me.Button_OptZielReihe = New System.Windows.Forms.Button
        Me.OpenFile_OptZielReihe = New System.Windows.Forms.OpenFileDialog
        Me.GroupBox_BM.SuspendLayout()
        Me.GroupBox_Parameter.SuspendLayout()
        Me.GroupBox_Ziel.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox_BM
        '
        Me.GroupBox_BM.Controls.Add(Me.GroupBox_Parameter)
        Me.GroupBox_BM.Controls.Add(Me.GroupBox_Ziel)
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
        'GroupBox_Parameter
        '
        Me.GroupBox_Parameter.Controls.Add(Me.TableLayoutPanel1)
        Me.GroupBox_Parameter.Controls.Add(Me.Label_Parameter)
        Me.GroupBox_Parameter.Controls.Add(Me.TextBox_OptParameter_Pfad)
        Me.GroupBox_Parameter.Controls.Add(Me.Button_OptParameter)
        Me.GroupBox_Parameter.Location = New System.Drawing.Point(9, 72)
        Me.GroupBox_Parameter.Name = "GroupBox_Parameter"
        Me.GroupBox_Parameter.Size = New System.Drawing.Size(275, 102)
        Me.GroupBox_Parameter.TabIndex = 31
        Me.GroupBox_Parameter.TabStop = False
        Me.GroupBox_Parameter.Text = "Optimierungsparameter"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 44)
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
        'TextBox_OptParameter_Pfad
        '
        Me.TextBox_OptParameter_Pfad.Location = New System.Drawing.Point(73, 17)
        Me.TextBox_OptParameter_Pfad.Name = "TextBox_OptParameter_Pfad"
        Me.TextBox_OptParameter_Pfad.Size = New System.Drawing.Size(166, 20)
        Me.TextBox_OptParameter_Pfad.TabIndex = 24
        Me.TextBox_OptParameter_Pfad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Button_OptParameter
        '
        Me.Button_OptParameter.Location = New System.Drawing.Point(245, 14)
        Me.Button_OptParameter.Name = "Button_OptParameter"
        Me.Button_OptParameter.Size = New System.Drawing.Size(24, 23)
        Me.Button_OptParameter.TabIndex = 26
        Me.Button_OptParameter.Text = "..."
        Me.Button_OptParameter.UseVisualStyleBackColor = True
        '
        'GroupBox_Ziel
        '
        Me.GroupBox_Ziel.Controls.Add(Me.Button_OptZielReihe)
        Me.GroupBox_Ziel.Controls.Add(Me.TextBox_OptZielReihe_Pfad)
        Me.GroupBox_Ziel.Controls.Add(Me.Label_OptZielReihe)
        Me.GroupBox_Ziel.Controls.Add(Me.Label_OptZielWert)
        Me.GroupBox_Ziel.Controls.Add(Me.TextBox_OptZielWert_Pfad)
        Me.GroupBox_Ziel.Controls.Add(Me.Button_OptZielWert)
        Me.GroupBox_Ziel.Location = New System.Drawing.Point(7, 180)
        Me.GroupBox_Ziel.Name = "GroupBox_Ziel"
        Me.GroupBox_Ziel.Size = New System.Drawing.Size(275, 73)
        Me.GroupBox_Ziel.TabIndex = 30
        Me.GroupBox_Ziel.TabStop = False
        Me.GroupBox_Ziel.Text = "Ziel"
        '
        'Label_OptZielReihe
        '
        Me.Label_OptZielReihe.AutoSize = True
        Me.Label_OptZielReihe.Location = New System.Drawing.Point(6, 49)
        Me.Label_OptZielReihe.Name = "Label_OptZielReihe"
        Me.Label_OptZielReihe.Size = New System.Drawing.Size(72, 13)
        Me.Label_OptZielReihe.TabIndex = 27
        Me.Label_OptZielReihe.Text = "OptZielReihe:"
        '
        'Label_OptZielWert
        '
        Me.Label_OptZielWert.AutoSize = True
        Me.Label_OptZielWert.Location = New System.Drawing.Point(6, 25)
        Me.Label_OptZielWert.Name = "Label_OptZielWert"
        Me.Label_OptZielWert.Size = New System.Drawing.Size(67, 13)
        Me.Label_OptZielWert.TabIndex = 2
        Me.Label_OptZielWert.Text = "OptZielWert:"
        '
        'TextBox_OptZielWert_Pfad
        '
        Me.TextBox_OptZielWert_Pfad.Location = New System.Drawing.Point(79, 22)
        Me.TextBox_OptZielWert_Pfad.Name = "TextBox_OptZielWert_Pfad"
        Me.TextBox_OptZielWert_Pfad.Size = New System.Drawing.Size(158, 20)
        Me.TextBox_OptZielWert_Pfad.TabIndex = 24
        Me.TextBox_OptZielWert_Pfad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Button_OptZielWert
        '
        Me.Button_OptZielWert.Location = New System.Drawing.Point(243, 20)
        Me.Button_OptZielWert.Name = "Button_OptZielWert"
        Me.Button_OptZielWert.Size = New System.Drawing.Size(24, 23)
        Me.Button_OptZielWert.TabIndex = 26
        Me.Button_OptZielWert.Text = "..."
        Me.Button_OptZielWert.UseVisualStyleBackColor = True
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
        'OpenFile_OptZielWert
        '
        Me.OpenFile_OptZielWert.Filter = "ZIE-Dateien|*.zie"
        Me.OpenFile_OptZielWert.Title = "OptZielWert auswählen"
        '
        'OpenFile_OptParameter
        '
        Me.OpenFile_OptParameter.Filter = "Optimierungs-Parameter|*.opt"
        '
        'TextBox_OptZielReihe_Pfad
        '
        Me.TextBox_OptZielReihe_Pfad.Location = New System.Drawing.Point(79, 46)
        Me.TextBox_OptZielReihe_Pfad.Name = "TextBox_OptZielReihe_Pfad"
        Me.TextBox_OptZielReihe_Pfad.Size = New System.Drawing.Size(158, 20)
        Me.TextBox_OptZielReihe_Pfad.TabIndex = 28
        Me.TextBox_OptZielReihe_Pfad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Button_OptZielReihe
        '
        Me.Button_OptZielReihe.Location = New System.Drawing.Point(243, 44)
        Me.Button_OptZielReihe.Name = "Button_OptZielReihe"
        Me.Button_OptZielReihe.Size = New System.Drawing.Size(24, 22)
        Me.Button_OptZielReihe.TabIndex = 29
        Me.Button_OptZielReihe.Text = "..."
        Me.Button_OptZielReihe.UseVisualStyleBackColor = True
        '
        'OpenFile_OptZielReihe
        '
        Me.OpenFile_OptZielReihe.Filter = "ZIE-Dateien|*.zie"
        Me.OpenFile_OptZielReihe.Title = "OptZielReihe auswählen"
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
        Me.GroupBox_Ziel.ResumeLayout(False)
        Me.GroupBox_Ziel.PerformLayout()
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
    Friend WithEvents GroupBox_Ziel As System.Windows.Forms.GroupBox
    Friend WithEvents Label_OptZielWert As System.Windows.Forms.Label
    Friend WithEvents TextBox_OptZielWert_Pfad As System.Windows.Forms.TextBox
    Friend WithEvents Button_OptZielWert As System.Windows.Forms.Button
    Friend WithEvents OpenFile_OptZielWert As System.Windows.Forms.OpenFileDialog
    Friend WithEvents GroupBox_Parameter As System.Windows.Forms.GroupBox
    Friend WithEvents Label_Parameter As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TextBox_OptParameter_Pfad As System.Windows.Forms.TextBox
    Friend WithEvents Button_OptParameter As System.Windows.Forms.Button
    Friend WithEvents OpenFile_OptParameter As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Label_OptZielReihe As System.Windows.Forms.Label
    Friend WithEvents TextBox_OptZielReihe_Pfad As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Button_OptZielReihe As System.Windows.Forms.Button
    Friend WithEvents OpenFile_OptZielReihe As System.Windows.Forms.OpenFileDialog

End Class
