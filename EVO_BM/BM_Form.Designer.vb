<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BM_Form
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BM_Form))
        Me.GroupBox_BM = New System.Windows.Forms.GroupBox
        Me.Label_EXE = New System.Windows.Forms.Label
        Me.TextBox_EXE = New System.Windows.Forms.TextBox
        Me.Button_Exe = New System.Windows.Forms.Button
        Me.Button_Datensatz = New System.Windows.Forms.Button
        Me.Label_Datensatz = New System.Windows.Forms.Label
        Me.TextBox_Datensatz = New System.Windows.Forms.TextBox
        Me.GroupBox_Parameter = New System.Windows.Forms.GroupBox
        Me.Label_OptParameter = New System.Windows.Forms.Label
        Me.TextBox_OptParameter_Pfad = New System.Windows.Forms.TextBox
        Me.Button_OptParameter = New System.Windows.Forms.Button
        Me.GroupBox_OptZiele = New System.Windows.Forms.GroupBox
        Me.Label_OptZiele = New System.Windows.Forms.Label
        Me.TextBox_OptZiele_Pfad = New System.Windows.Forms.TextBox
        Me.Button_OptZielWert = New System.Windows.Forms.Button
        Me.OpenFile_Datensatz = New System.Windows.Forms.OpenFileDialog
        Me.OpenFile_EXE = New System.Windows.Forms.OpenFileDialog
        Me.OpenFile_OptZiele = New System.Windows.Forms.OpenFileDialog
        Me.OpenFile_OptParameter = New System.Windows.Forms.OpenFileDialog
        Me.Button_OK = New System.Windows.Forms.Button
        Me.Label_ModellParameter = New System.Windows.Forms.Label
        Me.TextBox_ModellParameter_Pfad = New System.Windows.Forms.TextBox
        Me.Button_ModellParameter = New System.Windows.Forms.Button
        Me.OpenFile_ModellParameter = New System.Windows.Forms.OpenFileDialog
        Me.GroupBox_BM.SuspendLayout()
        Me.GroupBox_Parameter.SuspendLayout()
        Me.GroupBox_OptZiele.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox_BM
        '
        Me.GroupBox_BM.Controls.Add(Me.Label_EXE)
        Me.GroupBox_BM.Controls.Add(Me.TextBox_EXE)
        Me.GroupBox_BM.Controls.Add(Me.Button_Exe)
        Me.GroupBox_BM.Controls.Add(Me.Button_Datensatz)
        Me.GroupBox_BM.Controls.Add(Me.Label_Datensatz)
        Me.GroupBox_BM.Controls.Add(Me.TextBox_Datensatz)
        Me.GroupBox_BM.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_BM.Name = "GroupBox_BM"
        Me.GroupBox_BM.Size = New System.Drawing.Size(368, 79)
        Me.GroupBox_BM.TabIndex = 35
        Me.GroupBox_BM.TabStop = False
        Me.GroupBox_BM.Text = "BlauesModell"
        '
        'Label_EXE
        '
        Me.Label_EXE.AutoSize = True
        Me.Label_EXE.Location = New System.Drawing.Point(6, 24)
        Me.Label_EXE.Name = "Label_EXE"
        Me.Label_EXE.Size = New System.Drawing.Size(46, 13)
        Me.Label_EXE.TabIndex = 29
        Me.Label_EXE.Text = "BM.exe:"
        '
        'TextBox_EXE
        '
        Me.TextBox_EXE.Location = New System.Drawing.Point(70, 21)
        Me.TextBox_EXE.Name = "TextBox_EXE"
        Me.TextBox_EXE.Size = New System.Drawing.Size(262, 20)
        Me.TextBox_EXE.TabIndex = 28
        '
        'Button_Exe
        '
        Me.Button_Exe.Location = New System.Drawing.Point(338, 19)
        Me.Button_Exe.Name = "Button_Exe"
        Me.Button_Exe.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Button_Exe.Size = New System.Drawing.Size(24, 23)
        Me.Button_Exe.TabIndex = 27
        Me.Button_Exe.Text = "..."
        Me.Button_Exe.UseVisualStyleBackColor = True
        '
        'Button_Datensatz
        '
        Me.Button_Datensatz.Location = New System.Drawing.Point(338, 43)
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
        Me.TextBox_Datensatz.Size = New System.Drawing.Size(262, 20)
        Me.TextBox_Datensatz.TabIndex = 24
        Me.TextBox_Datensatz.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'GroupBox_Parameter
        '
        Me.GroupBox_Parameter.Controls.Add(Me.Button_ModellParameter)
        Me.GroupBox_Parameter.Controls.Add(Me.TextBox_ModellParameter_Pfad)
        Me.GroupBox_Parameter.Controls.Add(Me.Label_ModellParameter)
        Me.GroupBox_Parameter.Controls.Add(Me.Label_OptParameter)
        Me.GroupBox_Parameter.Controls.Add(Me.TextBox_OptParameter_Pfad)
        Me.GroupBox_Parameter.Controls.Add(Me.Button_OptParameter)
        Me.GroupBox_Parameter.Location = New System.Drawing.Point(12, 97)
        Me.GroupBox_Parameter.Name = "GroupBox_Parameter"
        Me.GroupBox_Parameter.Size = New System.Drawing.Size(368, 81)
        Me.GroupBox_Parameter.TabIndex = 31
        Me.GroupBox_Parameter.TabStop = False
        Me.GroupBox_Parameter.Text = "Optimierungsparameter"
        '
        'Label_OptParameter
        '
        Me.Label_OptParameter.AutoSize = True
        Me.Label_OptParameter.Location = New System.Drawing.Point(6, 20)
        Me.Label_OptParameter.Name = "Label_OptParameter"
        Me.Label_OptParameter.Size = New System.Drawing.Size(49, 13)
        Me.Label_OptParameter.TabIndex = 1
        Me.Label_OptParameter.Text = "OptPara:"
        '
        'TextBox_OptParameter_Pfad
        '
        Me.TextBox_OptParameter_Pfad.Location = New System.Drawing.Point(68, 17)
        Me.TextBox_OptParameter_Pfad.Name = "TextBox_OptParameter_Pfad"
        Me.TextBox_OptParameter_Pfad.Size = New System.Drawing.Size(264, 20)
        Me.TextBox_OptParameter_Pfad.TabIndex = 24
        Me.TextBox_OptParameter_Pfad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Button_OptParameter
        '
        Me.Button_OptParameter.Location = New System.Drawing.Point(338, 15)
        Me.Button_OptParameter.Name = "Button_OptParameter"
        Me.Button_OptParameter.Size = New System.Drawing.Size(24, 23)
        Me.Button_OptParameter.TabIndex = 26
        Me.Button_OptParameter.Text = "..."
        Me.Button_OptParameter.UseVisualStyleBackColor = True
        '
        'GroupBox_OptZiele
        '
        Me.GroupBox_OptZiele.Controls.Add(Me.Label_OptZiele)
        Me.GroupBox_OptZiele.Controls.Add(Me.TextBox_OptZiele_Pfad)
        Me.GroupBox_OptZiele.Controls.Add(Me.Button_OptZielWert)
        Me.GroupBox_OptZiele.Location = New System.Drawing.Point(12, 184)
        Me.GroupBox_OptZiele.Name = "GroupBox_OptZiele"
        Me.GroupBox_OptZiele.Size = New System.Drawing.Size(368, 51)
        Me.GroupBox_OptZiele.TabIndex = 30
        Me.GroupBox_OptZiele.TabStop = False
        Me.GroupBox_OptZiele.Text = "Optimierungsziele"
        '
        'Label_OptZiele
        '
        Me.Label_OptZiele.AutoSize = True
        Me.Label_OptZiele.Location = New System.Drawing.Point(6, 22)
        Me.Label_OptZiele.Name = "Label_OptZiele"
        Me.Label_OptZiele.Size = New System.Drawing.Size(55, 13)
        Me.Label_OptZiele.TabIndex = 2
        Me.Label_OptZiele.Text = "ZIE-Datei:"
        '
        'TextBox_OptZiele_Pfad
        '
        Me.TextBox_OptZiele_Pfad.Location = New System.Drawing.Point(70, 19)
        Me.TextBox_OptZiele_Pfad.Name = "TextBox_OptZiele_Pfad"
        Me.TextBox_OptZiele_Pfad.Size = New System.Drawing.Size(262, 20)
        Me.TextBox_OptZiele_Pfad.TabIndex = 24
        Me.TextBox_OptZiele_Pfad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Button_OptZielWert
        '
        Me.Button_OptZielWert.Location = New System.Drawing.Point(338, 17)
        Me.Button_OptZielWert.Name = "Button_OptZielWert"
        Me.Button_OptZielWert.Size = New System.Drawing.Size(24, 23)
        Me.Button_OptZielWert.TabIndex = 26
        Me.Button_OptZielWert.Text = "..."
        Me.Button_OptZielWert.UseVisualStyleBackColor = True
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
        'OpenFile_OptZiele
        '
        Me.OpenFile_OptZiele.Filter = "ZIE-Dateien|*.zie"
        Me.OpenFile_OptZiele.Title = "Zielfunktionsdatei auswählen"
        '
        'OpenFile_OptParameter
        '
        Me.OpenFile_OptParameter.Filter = "Optimierungs-Parameter|*.opt"
        '
        'Button_OK
        '
        Me.Button_OK.Location = New System.Drawing.Point(305, 331)
        Me.Button_OK.Name = "Button_OK"
        Me.Button_OK.Size = New System.Drawing.Size(75, 23)
        Me.Button_OK.TabIndex = 36
        Me.Button_OK.Text = "OK"
        Me.Button_OK.UseVisualStyleBackColor = True
        '
        'Label_ModellParameter
        '
        Me.Label_ModellParameter.AutoSize = True
        Me.Label_ModellParameter.Location = New System.Drawing.Point(6, 46)
        Me.Label_ModellParameter.Name = "Label_ModellParameter"
        Me.Label_ModellParameter.Size = New System.Drawing.Size(63, 13)
        Me.Label_ModellParameter.TabIndex = 27
        Me.Label_ModellParameter.Text = "ModellPara:"
        '
        'TextBox_ModellParameter_Pfad
        '
        Me.TextBox_ModellParameter_Pfad.Location = New System.Drawing.Point(68, 43)
        Me.TextBox_ModellParameter_Pfad.Name = "TextBox_ModellParameter_Pfad"
        Me.TextBox_ModellParameter_Pfad.Size = New System.Drawing.Size(264, 20)
        Me.TextBox_ModellParameter_Pfad.TabIndex = 24
        Me.TextBox_ModellParameter_Pfad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Button_ModellParameter
        '
        Me.Button_ModellParameter.Location = New System.Drawing.Point(338, 40)
        Me.Button_ModellParameter.Name = "Button_ModellParameter"
        Me.Button_ModellParameter.Size = New System.Drawing.Size(24, 23)
        Me.Button_ModellParameter.TabIndex = 29
        Me.Button_ModellParameter.Text = "..."
        Me.Button_ModellParameter.UseVisualStyleBackColor = True
        '
        'OpenFile_ModellParameter
        '
        Me.OpenFile_ModellParameter.Filter = "Modell-Parameter|*.opt"
        '
        'BM_Form
        '
        Me.AcceptButton = Me.Button_OK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(392, 366)
        Me.Controls.Add(Me.Button_OK)
        Me.Controls.Add(Me.GroupBox_OptZiele)
        Me.Controls.Add(Me.GroupBox_Parameter)
        Me.Controls.Add(Me.GroupBox_BM)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "BM_Form"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Blaues Modell"
        Me.TopMost = True
        Me.GroupBox_BM.ResumeLayout(False)
        Me.GroupBox_BM.PerformLayout()
        Me.GroupBox_Parameter.ResumeLayout(False)
        Me.GroupBox_Parameter.PerformLayout()
        Me.GroupBox_OptZiele.ResumeLayout(False)
        Me.GroupBox_OptZiele.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox_BM As System.Windows.Forms.GroupBox
    Friend WithEvents Label_EXE As System.Windows.Forms.Label
    Friend WithEvents Button_Exe As System.Windows.Forms.Button
    Friend WithEvents Button_Datensatz As System.Windows.Forms.Button
    Friend WithEvents Label_Datensatz As System.Windows.Forms.Label
    Friend WithEvents OpenFile_Datensatz As System.Windows.Forms.OpenFileDialog
    Friend WithEvents OpenFile_EXE As System.Windows.Forms.OpenFileDialog
    Friend WithEvents GroupBox_OptZiele As System.Windows.Forms.GroupBox
    Friend WithEvents Label_OptZiele As System.Windows.Forms.Label
    Friend WithEvents Button_OptZielWert As System.Windows.Forms.Button
    Friend WithEvents OpenFile_OptZiele As System.Windows.Forms.OpenFileDialog
    Friend WithEvents GroupBox_Parameter As System.Windows.Forms.GroupBox
    Friend WithEvents Label_OptParameter As System.Windows.Forms.Label
    Friend WithEvents Button_OptParameter As System.Windows.Forms.Button
    Friend WithEvents OpenFile_OptParameter As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Button_OK As System.Windows.Forms.Button
    Public WithEvents TextBox_EXE As System.Windows.Forms.TextBox
    Public WithEvents TextBox_Datensatz As System.Windows.Forms.TextBox
    Public WithEvents TextBox_OptZiele_Pfad As System.Windows.Forms.TextBox
    Public WithEvents TextBox_OptParameter_Pfad As System.Windows.Forms.TextBox
    Friend WithEvents Button_ModellParameter As System.Windows.Forms.Button
    Friend WithEvents Label_ModellParameter As System.Windows.Forms.Label
    Public WithEvents TextBox_ModellParameter_Pfad As System.Windows.Forms.TextBox
    Friend WithEvents OpenFile_ModellParameter As System.Windows.Forms.OpenFileDialog

End Class
