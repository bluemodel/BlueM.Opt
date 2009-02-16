''' <summary>
''' Der Monitor stellt ein Diagramm und ein Textfeld (Log) zur Verfügung
''' </summary>
Partial Public Class Monitor
    Inherits System.Windows.Forms.Form

    Public Event MonitorClosed()
    Public Event MonitorOpened()

    ''' <summary>
    ''' Das Monitordiagramm
    ''' </summary>
    ''' <remarks></remarks>
    Public WithEvents Diag As Diagramm

    ''' <summary>
    ''' Der Log
    ''' </summary>
    Public Property LogText() As String
        Get
            Return Me.TextBox_Log.Text
        End Get
        Set(ByVal value As String)
            Me.TextBox_Log.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Fügt dem Log einen Text hinzu
    ''' </summary>
    ''' <param name="text">der Text</param>
    Public Sub LogAppend(ByVal text As String)

        Call Me.TextBox_Log.AppendText(text & EVO.Common.Constants.eol)

    End Sub

    ''' <summary>
    ''' Löscht den Log
    ''' </summary>
    Public Sub LogClear()
        Call Me.TextBox_Log.Clear()
    End Sub

    ''' <summary>
    ''' Bringt das Diagramm nach vorne
    ''' </summary>
    Public Sub SelectTabDiagramm()
        Me.TabControl1.SelectedTab = Me.TabPage_Diagramm
    End Sub

    ''' <summary>
    ''' Bringt den Log nach vorne
    ''' </summary>
    Public Sub SelectTabLog()
        Me.TabControl1.SelectedTab = Me.TabPage_Log
    End Sub

#Region "UI"

    'Diagramm bearbeiten
    '*******************
    Private Sub Diag_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Diag.DoubleClick
        Call Me.Diag.ShowEditor()
    End Sub

    'Form schließen
    '**************
    Private Sub Monitor_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        'verhindern, dass das Form komplett gelöscht wird
        e.Cancel = True

        'Dialog verstecken
        Call Me.Hide()

        'Event auslösen (wird von Form1 verarbeitet)
        RaiseEvent MonitorClosed()

    End Sub

    'Form laden
    '**********
    Private Sub Monitor_FormLoaded(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Event auslösen (wird von Form1 verarbeitet)
        RaiseEvent MonitorOpened()

    End Sub

#End Region

End Class