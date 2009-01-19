<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Monitor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Monitor))
        Me.Monitordiagramm = New Steema.TeeChart.TChart
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage_Diagramm = New System.Windows.Forms.TabPage
        Me.TabPage_Log = New System.Windows.Forms.TabPage
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.TabControl1.SuspendLayout()
        Me.TabPage_Diagramm.SuspendLayout()
        Me.TabPage_Log.SuspendLayout()
        Me.SuspendLayout()
        '
        'Monitordiagramm
        '
        Me.Monitordiagramm.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        '
        '
        '
        Me.Monitordiagramm.Aspect.View3D = False
        Me.Monitordiagramm.Aspect.ZOffset = 0
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Monitordiagramm.Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
        Me.Monitordiagramm.Axes.Bottom.MaximumOffset = 3
        Me.Monitordiagramm.Axes.Bottom.MinimumOffset = 3
        '
        '
        '
        Me.Monitordiagramm.Axes.Left.MaximumOffset = 3
        Me.Monitordiagramm.Axes.Left.MinimumOffset = 3
        '
        '
        '
        Me.Monitordiagramm.Axes.Right.Visible = False
        '
        '
        '
        Me.Monitordiagramm.Axes.Top.Visible = False
        Me.Monitordiagramm.BackColor = System.Drawing.Color.Transparent
        Me.Monitordiagramm.Cursor = System.Windows.Forms.Cursors.Default
        '
        '
        '
        Me.Monitordiagramm.Header.Visible = False
        '
        '
        '
        Me.Monitordiagramm.Legend.Alignment = Steema.TeeChart.LegendAlignments.Bottom
        Me.Monitordiagramm.Legend.CheckBoxes = True
        Me.Monitordiagramm.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Series
        Me.Monitordiagramm.Location = New System.Drawing.Point(0, 0)
        Me.Monitordiagramm.Name = "Monitordiagramm"
        '
        '
        '
        Me.Monitordiagramm.Panning.Allow = Steema.TeeChart.ScrollModes.None
        Me.Monitordiagramm.Size = New System.Drawing.Size(488, 439)
        Me.Monitordiagramm.TabIndex = 8
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage_Diagramm)
        Me.TabControl1.Controls.Add(Me.TabPage_Log)
        Me.TabControl1.Location = New System.Drawing.Point(0, 2)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(496, 465)
        Me.TabControl1.TabIndex = 9
        '
        'TabPage_Diagramm
        '
        Me.TabPage_Diagramm.Controls.Add(Me.Monitordiagramm)
        Me.TabPage_Diagramm.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Diagramm.Name = "TabPage_Diagramm"
        Me.TabPage_Diagramm.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_Diagramm.Size = New System.Drawing.Size(488, 439)
        Me.TabPage_Diagramm.TabIndex = 0
        Me.TabPage_Diagramm.Text = "Diagramm"
        Me.TabPage_Diagramm.UseVisualStyleBackColor = True
        '
        'TabPage_Log
        '
        Me.TabPage_Log.Controls.Add(Me.TextBox1)
        Me.TabPage_Log.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Log.Name = "TabPage_Log"
        Me.TabPage_Log.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_Log.Size = New System.Drawing.Size(488, 439)
        Me.TabPage_Log.TabIndex = 1
        Me.TabPage_Log.Text = "Log"
        Me.TabPage_Log.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(6, 6)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox1.Size = New System.Drawing.Size(476, 427)
        Me.TextBox1.TabIndex = 0
        '
        'Monitor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(492, 466)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Monitor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "BlueM.Opt Monitor"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage_Diagramm.ResumeLayout(False)
        Me.TabPage_Log.ResumeLayout(False)
        Me.TabPage_Log.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents TabControl1 As System.Windows.Forms.TabControl
    Private WithEvents TabPage_Diagramm As System.Windows.Forms.TabPage
    Private WithEvents TabPage_Log As System.Windows.Forms.TabPage
    Private WithEvents TextBox1 As System.Windows.Forms.TextBox
End Class
