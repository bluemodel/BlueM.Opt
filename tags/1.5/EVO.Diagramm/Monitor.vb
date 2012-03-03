' Copyright (c) 2011, ihwb, TU Darmstadt
' All rights reserved.
' 
' Released under the BSD-2-Clause License:
' 
' Redistribution and use in source and binary forms, with or without modification, 
' are permitted provided that the following conditions are met:
' 
' * Redistributions of source code must retain the above copyright notice, this list 
'   of conditions and the following disclaimer.
' * Redistributions in binary form must reproduce the above copyright notice, this list 
'   of conditions and the following disclaimer in the documentation and/or other materials 
'   provided with the distribution.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
' EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
' OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
' SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT 
' OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
' HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
' TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
' EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'--------------------------------------------------------------------------------------------
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