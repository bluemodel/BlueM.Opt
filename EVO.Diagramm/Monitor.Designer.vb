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
        Me.SuspendLayout()
        '
        'Monitordiagramm
        '
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
        Me.Monitordiagramm.Size = New System.Drawing.Size(492, 466)
        Me.Monitordiagramm.TabIndex = 8
        '
        'Monitor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(492, 466)
        Me.Controls.Add(Me.Monitordiagramm)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Monitor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "BlueM.Opt Monitor"
        Me.ResumeLayout(False)

    End Sub
End Class
