Imports System.IO

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
        Me.TextBox1.AppendText(Format((DateTime.Now - starttime).TotalSeconds, "###,###,##0.00") + ": " + text + vbCrLf)
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Public Sub savelog()
        Dim sw As StreamWriter = File.CreateText("c:\MetaEvo_Logfile.txt")

        sw.Write(Me.TextBox1.Text)
        sw.Flush()
        sw.Close()
    End Sub

End Class