Imports System.IO

''' <summary>
''' Der Monitor stellt ein Diagramm und ein Textfeld (Log) zur Verfügung
''' </summary>
Partial Public Class Monitor
    Inherits System.Windows.Forms.Form

    Private starttime As DateTime

    Public Event MonitorClosed()
    Public Event MonitorOpened()

    ''' <summary>
    ''' Das Monitordiagramm
    ''' </summary>
    Public WithEvents Diag As Diagramm

#Region "Properties"

    ''' <summary>
    ''' Der Log-Text
    ''' </summary>
    Public Property LogText() As String
        Get
            Return Me.TextBox_Log.Text
        End Get
        Set(ByVal value As String)
            Me.TextBox_Log.Text = value
        End Set
    End Property

#End Region 'Properties

#Region "Methoden"

    ''' <summary>
    ''' Konstruktor
    ''' </summary>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.starttime = DateTime.Now

    End Sub

    ''' <summary>
    ''' Fügt dem Log einen Text hinzu
    ''' </summary>
    ''' <param name="text">der Text</param>
    Public Sub LogAppend(ByVal text As String)
        Call Me.TextBox_Log.AppendText(Format((DateTime.Now - starttime).TotalSeconds, "###,###,##0.00") & ": " & text & EVO.Common.Constants.eol)
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

    ''' <summary>
    ''' Ruft den Speichern-Dialog um den Log-Inhalt als Textdatei abzuspeichern
    ''' </summary>
    Public Sub savelog()

        Dim sw As StreamWriter
        Dim SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Dim jetzt = DateTime.Now

        'Dialog(einrichten)
        SaveFileDialog1.Filter = "Text-Dateien (*.txt)|*.txt"
        SaveFileDialog1.FileName = "ApplicationLog_" + jetzt.Year.ToString + jetzt.Month.ToString + jetzt.Day.ToString + "_" + jetzt.Hour.ToString + jetzt.Minute.ToString + jetzt.Second.ToString + ".txt"
        SaveFileDialog1.DefaultExt = "txt"
        SaveFileDialog1.Title = "Log speichern"
        SaveFileDialog1.InitialDirectory = CurDir()

        'Dialog anzeigen
        If (SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            sw = File.CreateText(SaveFileDialog1.FileName)
            sw.Write(Me.TextBox_Log.Text)
            sw.Flush()
            sw.Close()
        End If
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

#End Region 'UI

#End Region 'Methoden

End Class