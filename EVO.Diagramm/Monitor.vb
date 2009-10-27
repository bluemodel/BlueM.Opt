Imports System.IO

''' <summary>
''' Der Monitor stellt ein Diagramm und ein Textfeld (Log) zur Verfügung
''' </summary>
Partial Public Class Monitor
    Inherits System.Windows.Forms.Form

    Private Shared myInstance As Monitor 'Singleton

    ' This delegate enables asynchronous calls for setting
    ' the text property on a TextBox control.
    Delegate Sub LogAppendTextCallback(ByVal [text] As String)

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
    Public ReadOnly Property LogText() As String
        Get
            Return Me.TextBox_Log.Text
        End Get
    End Property

#End Region 'Properties

#Region "Methoden"

#Region "Public Methoden"

    ''' <summary>
    ''' Konstruktor
    ''' </summary>
    Private Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Call Me.Reset()

    End Sub

    ''' <summary>
    ''' Gibt die (einzige) Instanz des Monitors zurück
    ''' </summary>
    ''' <returns>Instanz des Monitors</returns>
    Public Shared Function getInstance() As Monitor
        If (IsNothing(Monitor.myInstance)) Then
            Monitor.myInstance = New Monitor()
        End If

        Return Monitor.myInstance

    End Function

    ''' <summary>
    ''' Fügt dem Log einen Text hinzu
    ''' </summary>
    ''' <param name="text">der Text</param>
    Public Sub LogAppend(ByVal text As String)
        Call Me.LogAppendText(Format((DateTime.Now - starttime).TotalSeconds, "###,###,##0.00") & ": " & text & EVO.Common.Constants.eol)
        System.Windows.Forms.Application.DoEvents()
    End Sub

    ''' <summary>
    ''' Alles zurücksetzen
    ''' </summary>
    Public Sub Reset()
        Call Me.InitMonitorDiagramm()
        Call Me.TextBox_Log.Clear()
        Me.starttime = DateTime.Now
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
        Dim jetzt = DateTime.Now

        Dim SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        'Dialog(einrichten)
        SaveFileDialog1.Filter = "Text-Dateien (*.txt)|*.txt"
        SaveFileDialog1.FileName = "ApplicationLog_" + jetzt.Year.ToString + _
            jetzt.Month.ToString + jetzt.Day.ToString + "_" + jetzt.Hour.ToString + _
            jetzt.Minute.ToString + jetzt.Second.ToString + ".txt"
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


    Public Sub savelog(ByRef Path As String)

        Dim sw As StreamWriter
        Dim jetzt = DateTime.Now

        sw = File.CreateText(Path & "ApplicationLog_" + jetzt.Year.ToString + _
            jetzt.Month.ToString + jetzt.Day.ToString + "_" + jetzt.Hour.ToString + _
            jetzt.Minute.ToString + jetzt.Second.ToString + ".txt")
        sw.Write(Me.TextBox_Log.Text)
        sw.Flush()
        sw.Close()

    End Sub

#End Region 'Public Methoden

#Region "Private Methoden"

    ''' <summary>
    ''' Setzt das Monitordiagramm zurück
    ''' </summary>
    Private Sub InitMonitorDiagramm()
        Me.Diag.Reset()
        Me.Diag.Aspect.View3D = False
        Me.Diag.Aspect.ZOffset = 0
        Me.Diag.Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
        Me.Diag.Axes.Bottom.MaximumOffset = 3
        Me.Diag.Axes.Bottom.MinimumOffset = 3
        Me.Diag.Axes.Left.MaximumOffset = 3
        Me.Diag.Axes.Left.MinimumOffset = 3
        Me.Diag.Axes.Right.Visible = False
        Me.Diag.Axes.Top.Visible = False
        Me.Diag.BackColor = System.Drawing.Color.Transparent
        Me.Diag.Cursor = System.Windows.Forms.Cursors.Default
        Me.Diag.Header.Visible = False
        Me.Diag.Legend.Alignment = Steema.TeeChart.LegendAlignments.Bottom
        Me.Diag.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Series
        Me.Diag.Location = New System.Drawing.Point(0, 0)
        Me.Diag.Panel.Brush.Color = System.Drawing.Color.Transparent
        Me.Diag.Panning.Allow = Steema.TeeChart.ScrollModes.None
    End Sub

    ' This method demonstrates a pattern for making thread-safe
    ' calls on a Windows Forms control. 
    '
    ' If the calling thread is different from the thread that
    ' created the TextBox control, this method creates a
    ' LogAppendTextCallback and calls itself asynchronously using the
    ' Invoke method.
    '
    ' If the calling thread is the same as the thread that created
    ' the TextBox control, the Text property is set directly. 
    Private Sub LogAppendText(ByVal [text] As String)

        ' InvokeRequired required compares the thread ID of the
        ' calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If (Me.TextBox_Log.InvokeRequired) Then
            Dim d As New LogAppendTextCallback(AddressOf LogAppendText)
            Me.Invoke(d, New Object() {[text]})
        Else
            Me.TextBox_Log.AppendText([text])
        End If
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

#End Region 'Private Methoden

#End Region 'Methoden

End Class