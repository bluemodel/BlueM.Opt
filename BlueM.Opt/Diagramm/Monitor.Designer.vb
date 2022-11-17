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
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage_Diagramm = New System.Windows.Forms.TabPage
        Me.Diag = New BlueM.Opt.Diagramm.Diagramm
        Me.TabPage_Log = New System.Windows.Forms.TabPage
        Me.TextBox_Log = New System.Windows.Forms.TextBox
        Me.TabControl1.SuspendLayout()
        Me.TabPage_Diagramm.SuspendLayout()
        Me.TabPage_Log.SuspendLayout()
        Me.SuspendLayout()
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
        Me.TabPage_Diagramm.Controls.Add(Me.Diag)
        Me.TabPage_Diagramm.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Diagramm.Name = "TabPage_Diagramm"
        Me.TabPage_Diagramm.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_Diagramm.Size = New System.Drawing.Size(488, 439)
        Me.TabPage_Diagramm.TabIndex = 0
        Me.TabPage_Diagramm.Text = "Chart"
        Me.TabPage_Diagramm.UseVisualStyleBackColor = True
        '
        'Diag
        '
        Me.Diag.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        '
        '
        '
        Me.Diag.Aspect.View3D = False
        '
        '
        '
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Labels.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Axes.Bottom.Labels.Bevel.StringColorTwo = "FF808080"
        Me.Diag.Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
        Me.Diag.Axes.Bottom.MaximumOffset = 3
        Me.Diag.Axes.Bottom.MinimumOffset = 3
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Bottom.Title.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Axes.Bottom.Title.Bevel.StringColorTwo = "FF808080"
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Labels.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Axes.Depth.Labels.Bevel.StringColorTwo = "FF808080"
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Depth.Title.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Axes.Depth.Title.Bevel.StringColorTwo = "FF808080"
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Labels.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Axes.DepthTop.Labels.Bevel.StringColorTwo = "FF808080"
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.DepthTop.Title.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Axes.DepthTop.Title.Bevel.StringColorTwo = "FF808080"
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Labels.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Axes.Left.Labels.Bevel.StringColorTwo = "FF808080"
        Me.Diag.Axes.Left.MaximumOffset = 3
        Me.Diag.Axes.Left.MinimumOffset = 3
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Left.Title.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Axes.Left.Title.Bevel.StringColorTwo = "FF808080"
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Labels.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Axes.Right.Labels.Bevel.StringColorTwo = "FF808080"
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Right.Title.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Axes.Right.Title.Bevel.StringColorTwo = "FF808080"
        Me.Diag.Axes.Right.Visible = False
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Labels.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Axes.Top.Labels.Bevel.StringColorTwo = "FF808080"
        '
        '
        '
        '
        '
        '
        Me.Diag.Axes.Top.Title.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Axes.Top.Title.Bevel.StringColorTwo = "FF808080"
        Me.Diag.Axes.Top.Visible = False
        Me.Diag.BackColor = System.Drawing.Color.Transparent
        Me.Diag.Cursor = System.Windows.Forms.Cursors.Default
        '
        '
        '
        '
        '
        '
        Me.Diag.Footer.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Footer.Bevel.StringColorTwo = "FF808080"
        '
        '
        '
        '
        '
        '
        Me.Diag.Header.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Header.Bevel.StringColorTwo = "FF808080"
        Me.Diag.Header.Visible = False
        '
        '
        '
        Me.Diag.Legend.Alignment = Steema.TeeChart.LegendAlignments.Bottom
        '
        '
        '
        Me.Diag.Legend.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Legend.Bevel.StringColorTwo = "FF808080"
        Me.Diag.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Series
        '
        '
        '
        '
        '
        '
        Me.Diag.Legend.Title.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Legend.Title.Bevel.StringColorTwo = "FF808080"
        Me.Diag.Location = New System.Drawing.Point(0, 0)
        Me.Diag.Name = "Diag"
        '
        '
        '
        '
        '
        '
        Me.Diag.Panel.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Panel.Bevel.StringColorTwo = "FF808080"
        '
        '
        '
        Me.Diag.Panel.Brush.Color = System.Drawing.Color.Transparent
        '
        '
        '
        Me.Diag.Panning.Allow = Steema.TeeChart.ScrollModes.None
        Me.Diag.Size = New System.Drawing.Size(488, 439)
        '
        '
        '
        '
        '
        '
        Me.Diag.SubFooter.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.SubFooter.Bevel.StringColorTwo = "FF808080"
        '
        '
        '
        '
        '
        '
        Me.Diag.SubHeader.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.SubHeader.Bevel.StringColorTwo = "FF808080"
        Me.Diag.TabIndex = 8
        '
        '
        '
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Back.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Walls.Back.Bevel.StringColorTwo = "FF808080"
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Bottom.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Walls.Bottom.Bevel.StringColorTwo = "FF808080"
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Left.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Walls.Left.Bevel.StringColorTwo = "FF808080"
        '
        '
        '
        '
        '
        '
        Me.Diag.Walls.Right.Bevel.StringColorOne = "FFFFFFFF"
        Me.Diag.Walls.Right.Bevel.StringColorTwo = "FF808080"
        '
        'TabPage_Log
        '
        Me.TabPage_Log.Controls.Add(Me.TextBox_Log)
        Me.TabPage_Log.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Log.Name = "TabPage_Log"
        Me.TabPage_Log.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_Log.Size = New System.Drawing.Size(488, 439)
        Me.TabPage_Log.TabIndex = 1
        Me.TabPage_Log.Text = "Log"
        Me.TabPage_Log.UseVisualStyleBackColor = True
        '
        'TextBox_Log
        '
        Me.TextBox_Log.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_Log.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox_Log.Location = New System.Drawing.Point(6, 6)
        Me.TextBox_Log.Multiline = True
        Me.TextBox_Log.Name = "TextBox_Log"
        Me.TextBox_Log.ReadOnly = True
        Me.TextBox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox_Log.Size = New System.Drawing.Size(476, 427)
        Me.TextBox_Log.TabIndex = 0
        '
        'Monitor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(492, 466)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Monitor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
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
    Private WithEvents TextBox_Log As System.Windows.Forms.TextBox
End Class
