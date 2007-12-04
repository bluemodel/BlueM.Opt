<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Testprobleme
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
        Me.GroupBox_Testproblem = New System.Windows.Forms.GroupBox
        Me.Combo_Testproblem = New System.Windows.Forms.ComboBox
        Me.Label_Beschreibung = New System.Windows.Forms.Label
        Me.Label_Einstellung = New System.Windows.Forms.Label
        Me.TextBox_Einstellung = New System.Windows.Forms.TextBox
        Me.GroupBox_Testproblem.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox_Testproblem
        '
        Me.GroupBox_Testproblem.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox_Testproblem.Controls.Add(Me.TextBox_Einstellung)
        Me.GroupBox_Testproblem.Controls.Add(Me.Label_Beschreibung)
        Me.GroupBox_Testproblem.Controls.Add(Me.Label_Einstellung)
        Me.GroupBox_Testproblem.Controls.Add(Me.Combo_Testproblem)
        Me.GroupBox_Testproblem.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox_Testproblem.Name = "GroupBox_Testproblem"
        Me.GroupBox_Testproblem.Size = New System.Drawing.Size(217, 113)
        Me.GroupBox_Testproblem.TabIndex = 2
        Me.GroupBox_Testproblem.TabStop = False
        Me.GroupBox_Testproblem.Text = "Testprobleme"
        '
        'Combo_Testproblem
        '
        Me.Combo_Testproblem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Combo_Testproblem.Location = New System.Drawing.Point(7, 20)
        Me.Combo_Testproblem.Name = "Combo_Testproblem"
        Me.Combo_Testproblem.Size = New System.Drawing.Size(202, 21)
        Me.Combo_Testproblem.TabIndex = 26
        '
        'Label_Beschreibung
        '
        Me.Label_Beschreibung.Location = New System.Drawing.Point(6, 49)
        Me.Label_Beschreibung.Name = "Label_Beschreibung"
        Me.Label_Beschreibung.Size = New System.Drawing.Size(203, 34)
        Me.Label_Beschreibung.TabIndex = 27
        Me.Label_Beschreibung.Text = "Parameter an Sinusfunktion anpassen"
        '
        'Label_Einstellung
        '
        Me.Label_Einstellung.AutoSize = True
        Me.Label_Einstellung.Location = New System.Drawing.Point(6, 89)
        Me.Label_Einstellung.Name = "Label_Einstellung"
        Me.Label_Einstellung.Size = New System.Drawing.Size(93, 13)
        Me.Label_Einstellung.TabIndex = 28
        Me.Label_Einstellung.Text = "Anzahl Parameter:"
        '
        'TextBox_Einstellung
        '
        Me.TextBox_Einstellung.Location = New System.Drawing.Point(173, 86)
        Me.TextBox_Einstellung.Name = "TextBox_Einstellung"
        Me.TextBox_Einstellung.Size = New System.Drawing.Size(36, 20)
        Me.TextBox_Einstellung.TabIndex = 29
        Me.TextBox_Einstellung.Text = "50"
        Me.TextBox_Einstellung.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Testprobleme
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.Controls.Add(Me.GroupBox_Testproblem)
        Me.Name = "Testprobleme"
        Me.Size = New System.Drawing.Size(232, 142)
        Me.GroupBox_Testproblem.ResumeLayout(False)
        Me.GroupBox_Testproblem.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox_Testproblem As System.Windows.Forms.GroupBox
    Private WithEvents Combo_Testproblem As System.Windows.Forms.ComboBox

    Public Sub New()
        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        IsInitializing = True
        InitializeComponent()
        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        System.Windows.Forms.Application.EnableVisualStyles()
    End Sub
    Private WithEvents Label_Beschreibung As System.Windows.Forms.Label
    Private WithEvents Label_Einstellung As System.Windows.Forms.Label
    Private WithEvents TextBox_Einstellung As System.Windows.Forms.TextBox
End Class
