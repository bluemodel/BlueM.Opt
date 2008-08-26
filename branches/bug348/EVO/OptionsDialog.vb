Imports System.Windows.Forms

Public Class OptionsDialog

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'Me.CheckBox_showOnlyCurrentPop.Checked = False

    End Sub

    Public ReadOnly Property showOnlyCurrentPop() As Boolean
        Get
            Return Me.CheckBox_showOnlyCurrentPop.Checked
        End Get
    End Property

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    'Form schließen
    '**************
    Private Sub OptionsDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        'verhindern, dass das Form komplett gelöscht wird
        e.Cancel = True

        'Dialog verstecken
        Call Me.Hide()

    End Sub
End Class
