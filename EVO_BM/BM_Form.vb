Public Class BM_Form
    'Public Properties
    Property Exe() As String
        Get
            Return Me.TextBox_EXE.Text
        End Get
        Set(ByVal value As String)
            Me.TextBox_EXE.Text = value
        End Set
    End Property

    Property Datensatz() As String
        Get
            Return Me.TextBox_Datensatz.Text
        End Get
        Set(ByVal value As String)
            Me.TextBox_Datensatz.Text = value
        End Set
    End Property

    Private Sub Button_Exe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Exe.Click
        Me.OpenFile_EXE.ShowDialog()
        Me.TextBox_EXE.Clear()
        Me.TextBox_EXE.AppendText(Me.OpenFile_EXE.FileName)
    End Sub

    Private Sub Button_Datensatz_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Datensatz.Click
        Me.OpenFile_Datensatz.ShowDialog()
        Me.TextBox_Datensatz.Clear()
        Me.TextBox_Datensatz.AppendText(Me.OpenFile_Datensatz.FileName)
    End Sub
End Class
