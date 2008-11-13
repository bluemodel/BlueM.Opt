Imports System.IO

Partial Public Class ApplicationLog
    Inherits System.Windows.Forms.Form

    Dim settings As EVO.Common.EVO_Settings
    Dim starttime As DateTime

    Public Sub New(ByRef settings_input As EVO.Common.EVO_Settings)
        Call InitializeComponent()
        Me.TextBox1.Clear()
        Me.Show()

        settings = settings_input
        Me.starttime = DateTime.Now
    End Sub

    Public Sub appendText(ByVal text As String)
        If (settings.MetaEvo.Log) Then
            Me.Show()
            Me.TextBox1.AppendText(Format((DateTime.Now - starttime).TotalSeconds, "###,###,##0.00") + ": " + text + vbCrLf)
            System.Windows.Forms.Application.DoEvents()
        Else
            Me.Hide()
        End If
    End Sub

    Public Sub savelog()
        If (settings.MetaEvo.Log) Then
            Dim sw As StreamWriter
            Dim SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
            Dim jetzt = DateTime.Now

            'Dialog einrichten
            SaveFileDialog1.Filter = "Text-Dateien (*.txt)|*.txt"
            SaveFileDialog1.FileName = "ApplicationLog_" + jetzt.Year.ToString + jetzt.Month.ToString + jetzt.Day.ToString + "_" + jetzt.Hour.ToString + jetzt.Minute.ToString + ".txt"
            SaveFileDialog1.DefaultExt = "txt"
            SaveFileDialog1.Title = "Einstellungsdatei speichern"
            SaveFileDialog1.InitialDirectory = CurDir()

            'Dialog anzeigen
            If (SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                sw = File.CreateText(SaveFileDialog1.FileName)
                sw.Write(Me.TextBox1.Text)
                sw.Flush()
                sw.Close()
            End If

        End If
    End Sub

    Private Sub ApplicationLog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        'verhindern, dass das Form komplett gelöscht wird
        e.Cancel = True

        'Dialog verstecken
        Call Me.Hide()

        settings.MetaEvo.Log = False

    End Sub
End Class