'BlueM.Opt
'Copyright (C) BlueM Dev Group
'Website: <https://www.bluemodel.org>
'
'This program is free software: you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.
'
'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
'GNU General Public License for more details.
'
'You should have received a copy of the GNU General Public License
'along with this program. If not, see <https://www.gnu.org/licenses/>.
'
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
        Call Me.LogAppendText(text)
        System.Windows.Forms.Application.DoEvents()
    End Sub

    ''' <summary>
    ''' Alles zurücksetzen
    ''' </summary>
    Public Sub Reset()
        Call Me.InitMonitorDiagramm()
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

#End Region 'Public Methoden

#Region "Private Methoden"

    ''' <summary>
    ''' Setzt das Monitordiagramm zurück
    ''' </summary>
    Private Sub InitMonitorDiagramm()

        With Me.Diag()
            .Reset()
            .Aspect.View3D = False
            .Aspect.ZOffset = 0
            .Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
            .Axes.Bottom.MaximumOffset = 3
            .Axes.Bottom.MinimumOffset = 3
            .Axes.Left.MaximumOffset = 3
            .Axes.Left.MinimumOffset = 3
            .Axes.Right.Visible = False
            .Axes.Top.Visible = False
            .Chart.Axes.Left.Ticks.Color = Drawing.Color.Black
            .Chart.Axes.Right.Ticks.Color = Drawing.Color.Black
            .Chart.Axes.Left.Ticks.Width = 1
            .Chart.Axes.Right.Ticks.Width = 1
            .BackColor = System.Drawing.Color.Transparent
            .Cursor = System.Windows.Forms.Cursors.Default
            .Header.Visible = False
            '.Legend.Visible = False
            .Legend.Alignment = Steema.TeeChart.LegendAlignments.Bottom
            .Legend.LegendStyle = Steema.TeeChart.LegendStyles.Series
            .Location = New System.Drawing.Point(0, 0)
            .Panel.Brush.Color = System.Drawing.Color.Transparent
            .Panel.Color = Drawing.Color.White
            .Panel.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
            .Panel.Bevel.Inner = Steema.TeeChart.Drawing.BevelStyles.None
            'Farbverlauf am Rand das Chart
            .Panel.Gradient.Visible = False
            .Walls.Visible = False
            .Panning.Allow = Steema.TeeChart.ScrollModes.None
        End With

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