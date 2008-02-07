<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Testprobleme
    Inherits System.Windows.Forms.Form

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Testprobleme))
        Me.TextBox_Einstellung = New System.Windows.Forms.NumericUpDown
        Me.Label_Beschreibungstext = New System.Windows.Forms.Label
        Me.Label_Einstellung = New System.Windows.Forms.Label
        Me.Combo_Testproblem = New System.Windows.Forms.ComboBox
        Me.Label_Testprobleme = New System.Windows.Forms.Label
        Me.Label_Beschreibung = New System.Windows.Forms.Label
        CType(Me.TextBox_Einstellung, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TextBox_Einstellung
        '
        Me.TextBox_Einstellung.Location = New System.Drawing.Point(111, 85)
        Me.TextBox_Einstellung.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.TextBox_Einstellung.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TextBox_Einstellung.Name = "TextBox_Einstellung"
        Me.TextBox_Einstellung.Size = New System.Drawing.Size(44, 20)
        Me.TextBox_Einstellung.TabIndex = 5
        Me.TextBox_Einstellung.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TextBox_Einstellung.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'Label_Beschreibungstext
        '
        Me.Label_Beschreibungstext.Location = New System.Drawing.Point(111, 41)
        Me.Label_Beschreibungstext.Name = "Label_Beschreibungstext"
        Me.Label_Beschreibungstext.Size = New System.Drawing.Size(177, 41)
        Me.Label_Beschreibungstext.TabIndex = 3
        Me.Label_Beschreibungstext.Text = "Parameter an Sinusfunktion anpassen"
        '
        'Label_Einstellung
        '
        Me.Label_Einstellung.AutoSize = True
        Me.Label_Einstellung.Location = New System.Drawing.Point(12, 88)
        Me.Label_Einstellung.Name = "Label_Einstellung"
        Me.Label_Einstellung.Size = New System.Drawing.Size(93, 13)
        Me.Label_Einstellung.TabIndex = 4
        Me.Label_Einstellung.Text = "Anzahl Parameter:"
        '
        'Combo_Testproblem
        '
        Me.Combo_Testproblem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Combo_Testproblem.Location = New System.Drawing.Point(111, 12)
        Me.Combo_Testproblem.Name = "Combo_Testproblem"
        Me.Combo_Testproblem.Size = New System.Drawing.Size(177, 21)
        Me.Combo_Testproblem.TabIndex = 1
        '
        'Label_Testprobleme
        '
        Me.Label_Testprobleme.AutoSize = True
        Me.Label_Testprobleme.Location = New System.Drawing.Point(12, 15)
        Me.Label_Testprobleme.Name = "Label_Testprobleme"
        Me.Label_Testprobleme.Size = New System.Drawing.Size(68, 13)
        Me.Label_Testprobleme.TabIndex = 0
        Me.Label_Testprobleme.Text = "Testproblem:"
        '
        'Label_Beschreibung
        '
        Me.Label_Beschreibung.AutoSize = True
        Me.Label_Beschreibung.Location = New System.Drawing.Point(12, 41)
        Me.Label_Beschreibung.Name = "Label_Beschreibung"
        Me.Label_Beschreibung.Size = New System.Drawing.Size(75, 13)
        Me.Label_Beschreibung.TabIndex = 2
        Me.Label_Beschreibung.Text = "Beschreibung:"
        '
        'Testprobleme
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.ClientSize = New System.Drawing.Size(304, 113)
        Me.Controls.Add(Me.Label_Beschreibung)
        Me.Controls.Add(Me.Label_Testprobleme)
        Me.Controls.Add(Me.Combo_Testproblem)
        Me.Controls.Add(Me.Label_Einstellung)
        Me.Controls.Add(Me.Label_Beschreibungstext)
        Me.Controls.Add(Me.TextBox_Einstellung)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Testprobleme"
        Me.ShowInTaskbar = False
        Me.Text = "Testprobleme"
        Me.TopMost = True
        CType(Me.TextBox_Einstellung, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public Sub New()
        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        IsInitializing = True
        InitializeComponent()
        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        System.Windows.Forms.Application.EnableVisualStyles()
    End Sub
    Private WithEvents Label_Beschreibungstext As System.Windows.Forms.Label
    Private WithEvents Label_Einstellung As System.Windows.Forms.Label
    Private WithEvents TextBox_Einstellung As System.Windows.Forms.NumericUpDown
    Private WithEvents Label_Testprobleme As System.Windows.Forms.Label
    Private WithEvents Label_Beschreibung As System.Windows.Forms.Label
    Friend WithEvents Combo_Testproblem As System.Windows.Forms.ComboBox
End Class
