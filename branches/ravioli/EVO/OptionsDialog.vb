Imports System.Windows.Forms

Public Class OptionsDialog

    Private _MultithreadingAllowed As Boolean

    ''' <summary>
    ''' Ob Multithreading erlaubt ist oder nicht
    ''' </summary>
    Public Property MultithreadingAllowed() As Boolean
        Get
            Return Me._MultithreadingAllowed
        End Get
        Set(ByVal allow As Boolean)
            Me._MultithreadingAllowed = allow
            If (allow) Then
                Me.CheckBox_useMultithreading.Enabled = True
                Me.CheckBox_useMultithreading.Checked = True
            Else
                Me.CheckBox_useMultithreading.Checked = False
                Me.CheckBox_useMultithreading.Enabled = False
            End If
        End Set
    End Property

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

    ''' <summary>
    ''' Alle Kontrollelemente deaktivieren
    ''' </summary>
    Public Sub DisableAll()
        For Each cntrl As Control In Me.Controls
            cntrl.Enabled = False
        Next
    End Sub

    ''' <summary>
    ''' Alle Kontrollelemente aktivieren
    ''' </summary>
    Public Sub EnableAll()
        For Each cntrl As Control In Me.Controls
            cntrl.Enabled = True
        Next
        'Ausser ggf. CheckBox_useMultithreading
        If (Not Me.MultithreadingAllowed) Then
            Me.CheckBox_useMultithreading.Enabled = False
        End If
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
