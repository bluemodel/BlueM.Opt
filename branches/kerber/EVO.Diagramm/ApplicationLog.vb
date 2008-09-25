
Partial Public Class ApplicationLog
    Inherits System.Windows.Forms.Form

    Dim starttime As DateTime
    Public log As Boolean

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        Call InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.TextBox1.Clear()
        Me.Show()
        Me.log = False
        Me.starttime = DateTime.Now

    End Sub

    Public Sub appendText(ByVal text As String)
        Me.TextBox1.AppendText(String.Format("{0:####.##}", (DateTime.Now - starttime).TotalSeconds) + ": " + text + vbCrLf)
        Me.Refresh()
        Me.Update()
    End Sub

End Class