Imports System.Windows.Forms

Public Class OptionsDialog

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        'Standardeinstellungen setzen
        Me.CheckBox_drawOnlyCurrentPop.Checked = False
        Me.CheckBox_useMultithreading.Checked = True

    End Sub

    ''' <summary>
    ''' Immer nur die aktuelle Population im Diagramm anzeigen
    ''' </summary>
    Public ReadOnly Property drawOnlyCurrentPop() As Boolean
        Get
            Return Me.CheckBox_drawOnlyCurrentPop.Checked
        End Get
    End Property

    ''' <summary>
    ''' Multithreading für Simulationsanwendungen verwenden
    ''' </summary>
    Public ReadOnly Property useMultiThreading() As Boolean
        Get
            Return Me.CheckBox_useMultithreading.Checked
        End Get
    End Property

    Public Sub DisableAll()
        For Each cntrl As Control In Me.Controls
            cntrl.Enabled = False
        Next
    End Sub

    Public Sub EnableAll()
        For Each cntrl As Control In Me.Controls
            cntrl.Enabled = True
        Next
    End Sub

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
