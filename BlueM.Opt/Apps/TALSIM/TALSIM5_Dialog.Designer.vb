<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TALSIM5_Dialog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TALSIM5_Dialog))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.Label_Database = New System.Windows.Forms.Label()
        Me.Label_Scenario = New System.Windows.Forms.Label()
        Me.Label_Simulation = New System.Windows.Forms.Label()
        Me.Label_TimeseriesFolder = New System.Windows.Forms.Label()
        Me.Label_PathDatabase = New System.Windows.Forms.Label()
        Me.ComboBox_Scenario = New System.Windows.Forms.ComboBox()
        Me.ComboBox_Simulation = New System.Windows.Forms.ComboBox()
        Me.TextBox_TimeseriesPath = New System.Windows.Forms.TextBox()
        Me.Button_BrowseFolder = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(172, 119)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
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
        Me.Cancel_Button.Text = "Cancel"
        '
        'Label_Database
        '
        Me.Label_Database.AutoSize = True
        Me.Label_Database.Location = New System.Drawing.Point(12, 9)
        Me.Label_Database.Name = "Label_Database"
        Me.Label_Database.Size = New System.Drawing.Size(56, 13)
        Me.Label_Database.TabIndex = 0
        Me.Label_Database.Text = "Database:"
        '
        'Label_Scenario
        '
        Me.Label_Scenario.AutoSize = True
        Me.Label_Scenario.Location = New System.Drawing.Point(12, 34)
        Me.Label_Scenario.Name = "Label_Scenario"
        Me.Label_Scenario.Size = New System.Drawing.Size(52, 13)
        Me.Label_Scenario.TabIndex = 2
        Me.Label_Scenario.Text = "Scenario:"
        '
        'Label_Simulation
        '
        Me.Label_Simulation.AutoSize = True
        Me.Label_Simulation.Location = New System.Drawing.Point(13, 61)
        Me.Label_Simulation.Name = "Label_Simulation"
        Me.Label_Simulation.Size = New System.Drawing.Size(58, 13)
        Me.Label_Simulation.TabIndex = 4
        Me.Label_Simulation.Text = "Simulation:"
        '
        'Label_TimeseriesFolder
        '
        Me.Label_TimeseriesFolder.AutoSize = True
        Me.Label_TimeseriesFolder.Location = New System.Drawing.Point(13, 90)
        Me.Label_TimeseriesFolder.Name = "Label_TimeseriesFolder"
        Me.Label_TimeseriesFolder.Size = New System.Drawing.Size(89, 13)
        Me.Label_TimeseriesFolder.TabIndex = 6
        Me.Label_TimeseriesFolder.Text = "Timeseries folder:"
        '
        'Label_PathDatabase
        '
        Me.Label_PathDatabase.AutoSize = True
        Me.Label_PathDatabase.Location = New System.Drawing.Point(114, 9)
        Me.Label_PathDatabase.Name = "Label_PathDatabase"
        Me.Label_PathDatabase.Size = New System.Drawing.Size(74, 13)
        Me.Label_PathDatabase.TabIndex = 1
        Me.Label_PathDatabase.Text = "pathDatabase"
        '
        'ComboBox_Scenario
        '
        Me.ComboBox_Scenario.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_Scenario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Scenario.FormattingEnabled = True
        Me.ComboBox_Scenario.Location = New System.Drawing.Point(117, 31)
        Me.ComboBox_Scenario.Name = "ComboBox_Scenario"
        Me.ComboBox_Scenario.Size = New System.Drawing.Size(201, 21)
        Me.ComboBox_Scenario.TabIndex = 3
        '
        'ComboBox_Simulation
        '
        Me.ComboBox_Simulation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_Simulation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Simulation.FormattingEnabled = True
        Me.ComboBox_Simulation.Location = New System.Drawing.Point(117, 58)
        Me.ComboBox_Simulation.Name = "ComboBox_Simulation"
        Me.ComboBox_Simulation.Size = New System.Drawing.Size(201, 21)
        Me.ComboBox_Simulation.TabIndex = 5
        '
        'TextBox_TimeseriesPath
        '
        Me.TextBox_TimeseriesPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_TimeseriesPath.Location = New System.Drawing.Point(117, 87)
        Me.TextBox_TimeseriesPath.Name = "TextBox_TimeseriesPath"
        Me.TextBox_TimeseriesPath.Size = New System.Drawing.Size(165, 20)
        Me.TextBox_TimeseriesPath.TabIndex = 7
        '
        'Button_BrowseFolder
        '
        Me.Button_BrowseFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_BrowseFolder.Location = New System.Drawing.Point(288, 85)
        Me.Button_BrowseFolder.Name = "Button_BrowseFolder"
        Me.Button_BrowseFolder.Size = New System.Drawing.Size(30, 23)
        Me.Button_BrowseFolder.TabIndex = 8
        Me.Button_BrowseFolder.Text = "..."
        Me.Button_BrowseFolder.UseVisualStyleBackColor = True
        '
        'TALSIM5_Dialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(330, 160)
        Me.Controls.Add(Me.Button_BrowseFolder)
        Me.Controls.Add(Me.TextBox_TimeseriesPath)
        Me.Controls.Add(Me.ComboBox_Simulation)
        Me.Controls.Add(Me.ComboBox_Scenario)
        Me.Controls.Add(Me.Label_PathDatabase)
        Me.Controls.Add(Me.Label_TimeseriesFolder)
        Me.Controls.Add(Me.Label_Simulation)
        Me.Controls.Add(Me.Label_Scenario)
        Me.Controls.Add(Me.Label_Database)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(346, 199)
        Me.Name = "TALSIM5_Dialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Talsim5 settings"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label_Database As Windows.Forms.Label
    Friend WithEvents Label_Scenario As Windows.Forms.Label
    Friend WithEvents Label_Simulation As Windows.Forms.Label
    Friend WithEvents Label_TimeseriesFolder As Windows.Forms.Label
    Friend WithEvents Label_PathDatabase As Windows.Forms.Label
    Friend WithEvents ComboBox_Scenario As Windows.Forms.ComboBox
    Friend WithEvents ComboBox_Simulation As Windows.Forms.ComboBox
    Friend WithEvents TextBox_TimeseriesPath As Windows.Forms.TextBox
    Friend WithEvents Button_BrowseFolder As Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As Windows.Forms.FolderBrowserDialog
End Class
