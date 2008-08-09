Option Strict Off
Option Explicit On
Partial Public Class EVO_Opt_Verlauf
    Inherits System.Windows.Forms.UserControl

#Region "Vom Windows Form-Designer generierter Code "

    Public Sub New()
        MyBase.New()
        'Dieser Aufruf ist f�r den Windows Form-Designer erforderlich.
        InitializeComponent()
    End Sub

    'Das Formular �berschreibt den L�schvorgang, um die Komponentenliste zu bereinigen.
    Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not components Is Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub

    'Wird vom Windows Form-Designer ben�tigt.
    Private components As System.ComponentModel.IContainer
    'Hinweis: Die folgende Prozedur wird vom Windows Form-Designer ben�tigt.
    'Das Ver�ndern mit dem Windows Form-Designer ist nicht m�glich.
    'Das Ver�ndern mit dem Code-Editor ist nicht m�glich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Anzeige_Verlauf = New System.Windows.Forms.GroupBox
        Me.ProgressBarRunde = New System.Windows.Forms.ProgressBar
        Me.ProgressBarPop = New System.Windows.Forms.ProgressBar
        Me.ProgressBarGen = New System.Windows.Forms.ProgressBar
        Me.ProgressBarNach = New System.Windows.Forms.ProgressBar
        Me.lblNachfolger = New System.Windows.Forms.Label
        Me.lblGeneration = New System.Windows.Forms.Label
        Me.lblPopulation = New System.Windows.Forms.Label
        Me.lblRunde = New System.Windows.Forms.Label
        Me.lblvon_4 = New System.Windows.Forms.Label
        Me.lblvon_3 = New System.Windows.Forms.Label
        Me.lblvon_2 = New System.Windows.Forms.Label
        Me.lblvon_1 = New System.Windows.Forms.Label
        Me.LabelaktNachf = New System.Windows.Forms.Label
        Me.LabelaktGen = New System.Windows.Forms.Label
        Me.LabelaktPop = New System.Windows.Forms.Label
        Me.LabelaktRunde = New System.Windows.Forms.Label
        Me.LabelAnzNachf = New System.Windows.Forms.Label
        Me.LabelAnzGen = New System.Windows.Forms.Label
        Me.LabelAnzPop = New System.Windows.Forms.Label
        Me.LabelAnzRunden = New System.Windows.Forms.Label
        Me.Anzeige_Verlauf.SuspendLayout()
        Me.SuspendLayout()
        '
        'Anzeige_Verlauf
        '
        Me.Anzeige_Verlauf.BackColor = System.Drawing.SystemColors.Control
        Me.Anzeige_Verlauf.Controls.Add(Me.ProgressBarRunde)
        Me.Anzeige_Verlauf.Controls.Add(Me.ProgressBarPop)
        Me.Anzeige_Verlauf.Controls.Add(Me.ProgressBarGen)
        Me.Anzeige_Verlauf.Controls.Add(Me.ProgressBarNach)
        Me.Anzeige_Verlauf.Controls.Add(Me.lblNachfolger)
        Me.Anzeige_Verlauf.Controls.Add(Me.lblGeneration)
        Me.Anzeige_Verlauf.Controls.Add(Me.lblPopulation)
        Me.Anzeige_Verlauf.Controls.Add(Me.lblRunde)
        Me.Anzeige_Verlauf.Controls.Add(Me.lblvon_4)
        Me.Anzeige_Verlauf.Controls.Add(Me.lblvon_3)
        Me.Anzeige_Verlauf.Controls.Add(Me.lblvon_2)
        Me.Anzeige_Verlauf.Controls.Add(Me.lblvon_1)
        Me.Anzeige_Verlauf.Controls.Add(Me.LabelaktNachf)
        Me.Anzeige_Verlauf.Controls.Add(Me.LabelaktGen)
        Me.Anzeige_Verlauf.Controls.Add(Me.LabelaktPop)
        Me.Anzeige_Verlauf.Controls.Add(Me.LabelaktRunde)
        Me.Anzeige_Verlauf.Controls.Add(Me.LabelAnzNachf)
        Me.Anzeige_Verlauf.Controls.Add(Me.LabelAnzGen)
        Me.Anzeige_Verlauf.Controls.Add(Me.LabelAnzPop)
        Me.Anzeige_Verlauf.Controls.Add(Me.LabelAnzRunden)
        Me.Anzeige_Verlauf.Location = New System.Drawing.Point(0, 0)
        Me.Anzeige_Verlauf.Name = "Anzeige_Verlauf"
        Me.Anzeige_Verlauf.Size = New System.Drawing.Size(465, 73)
        Me.Anzeige_Verlauf.TabIndex = 0
        Me.Anzeige_Verlauf.TabStop = False
        Me.Anzeige_Verlauf.Text = "Verlauf der Optimierung"
        '
        'ProgressBarRunde
        '
        Me.ProgressBarRunde.Location = New System.Drawing.Point(139, 24)
        Me.ProgressBarRunde.Name = "ProgressBarRunde"
        Me.ProgressBarRunde.Size = New System.Drawing.Size(73, 17)
        Me.ProgressBarRunde.TabIndex = 1
        '
        'ProgressBarPop
        '
        Me.ProgressBarPop.Location = New System.Drawing.Point(139, 40)
        Me.ProgressBarPop.Name = "ProgressBarPop"
        Me.ProgressBarPop.Size = New System.Drawing.Size(73, 17)
        Me.ProgressBarPop.TabIndex = 2
        '
        'ProgressBarGen
        '
        Me.ProgressBarGen.Location = New System.Drawing.Point(382, 24)
        Me.ProgressBarGen.Name = "ProgressBarGen"
        Me.ProgressBarGen.Size = New System.Drawing.Size(73, 17)
        Me.ProgressBarGen.TabIndex = 3
        '
        'ProgressBarNach
        '
        Me.ProgressBarNach.Location = New System.Drawing.Point(382, 40)
        Me.ProgressBarNach.Name = "ProgressBarNach"
        Me.ProgressBarNach.Size = New System.Drawing.Size(73, 17)
        Me.ProgressBarNach.TabIndex = 4
        '
        'lblNachfolger
        '
        Me.lblNachfolger.AutoSize = True
        Me.lblNachfolger.Location = New System.Drawing.Point(222, 42)
        Me.lblNachfolger.Name = "lblNachfolger"
        Me.lblNachfolger.Size = New System.Drawing.Size(59, 13)
        Me.lblNachfolger.TabIndex = 20
        Me.lblNachfolger.Text = "Nachfolger"
        '
        'lblGeneration
        '
        Me.lblGeneration.AutoSize = True
        Me.lblGeneration.Location = New System.Drawing.Point(222, 24)
        Me.lblGeneration.Name = "lblGeneration"
        Me.lblGeneration.Size = New System.Drawing.Size(59, 13)
        Me.lblGeneration.TabIndex = 19
        Me.lblGeneration.Text = "Generation"
        '
        'lblPopulation
        '
        Me.lblPopulation.AutoSize = True
        Me.lblPopulation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPopulation.Location = New System.Drawing.Point(8, 42)
        Me.lblPopulation.Name = "lblPopulation"
        Me.lblPopulation.Size = New System.Drawing.Size(57, 13)
        Me.lblPopulation.TabIndex = 18
        Me.lblPopulation.Text = "Population"
        '
        'lblRunde
        '
        Me.lblRunde.AutoSize = True
        Me.lblRunde.Location = New System.Drawing.Point(8, 24)
        Me.lblRunde.Name = "lblRunde"
        Me.lblRunde.Size = New System.Drawing.Size(39, 13)
        Me.lblRunde.TabIndex = 17
        Me.lblRunde.Text = "Runde"
        '
        'lblvon_4
        '
        Me.lblvon_4.AutoSize = True
        Me.lblvon_4.Location = New System.Drawing.Point(317, 42)
        Me.lblvon_4.Name = "lblvon_4"
        Me.lblvon_4.Size = New System.Drawing.Size(25, 13)
        Me.lblvon_4.TabIndex = 16
        Me.lblvon_4.Text = "von"
        '
        'lblvon_3
        '
        Me.lblvon_3.AutoSize = True
        Me.lblvon_3.Location = New System.Drawing.Point(317, 24)
        Me.lblvon_3.Name = "lblvon_3"
        Me.lblvon_3.Size = New System.Drawing.Size(25, 13)
        Me.lblvon_3.TabIndex = 15
        Me.lblvon_3.Text = "von"
        '
        'lblvon_2
        '
        Me.lblvon_2.AutoSize = True
        Me.lblvon_2.Location = New System.Drawing.Point(86, 42)
        Me.lblvon_2.Name = "lblvon_2"
        Me.lblvon_2.Size = New System.Drawing.Size(25, 13)
        Me.lblvon_2.TabIndex = 14
        Me.lblvon_2.Text = "von"
        '
        'lblvon_1
        '
        Me.lblvon_1.AutoSize = True
        Me.lblvon_1.Location = New System.Drawing.Point(86, 24)
        Me.lblvon_1.Name = "lblvon_1"
        Me.lblvon_1.Size = New System.Drawing.Size(25, 13)
        Me.lblvon_1.TabIndex = 13
        Me.lblvon_1.Text = "von"
        '
        'LabelaktNachf
        '
        Me.LabelaktNachf.ForeColor = System.Drawing.Color.Blue
        Me.LabelaktNachf.Location = New System.Drawing.Point(285, 42)
        Me.LabelaktNachf.Name = "LabelaktNachf"
        Me.LabelaktNachf.Size = New System.Drawing.Size(30, 13)
        Me.LabelaktNachf.TabIndex = 12
        Me.LabelaktNachf.Text = "0"
        '
        'LabelaktGen
        '
        Me.LabelaktGen.ForeColor = System.Drawing.Color.Blue
        Me.LabelaktGen.Location = New System.Drawing.Point(285, 24)
        Me.LabelaktGen.Name = "LabelaktGen"
        Me.LabelaktGen.Size = New System.Drawing.Size(30, 13)
        Me.LabelaktGen.TabIndex = 11
        Me.LabelaktGen.Text = "0"
        '
        'LabelaktPop
        '
        Me.LabelaktPop.ForeColor = System.Drawing.Color.Blue
        Me.LabelaktPop.Location = New System.Drawing.Point(63, 42)
        Me.LabelaktPop.Name = "LabelaktPop"
        Me.LabelaktPop.Size = New System.Drawing.Size(23, 13)
        Me.LabelaktPop.TabIndex = 10
        Me.LabelaktPop.Text = "0"
        '
        'LabelaktRunde
        '
        Me.LabelaktRunde.ForeColor = System.Drawing.Color.Blue
        Me.LabelaktRunde.Location = New System.Drawing.Point(63, 24)
        Me.LabelaktRunde.Name = "LabelaktRunde"
        Me.LabelaktRunde.Size = New System.Drawing.Size(23, 13)
        Me.LabelaktRunde.TabIndex = 9
        Me.LabelaktRunde.Text = "0"
        '
        'LabelAnzNachf
        '
        Me.LabelAnzNachf.ForeColor = System.Drawing.Color.Blue
        Me.LabelAnzNachf.Location = New System.Drawing.Point(346, 42)
        Me.LabelAnzNachf.Name = "LabelAnzNachf"
        Me.LabelAnzNachf.Size = New System.Drawing.Size(30, 13)
        Me.LabelAnzNachf.TabIndex = 8
        Me.LabelAnzNachf.Text = "0"
        '
        'LabelAnzGen
        '
        Me.LabelAnzGen.ForeColor = System.Drawing.Color.Blue
        Me.LabelAnzGen.Location = New System.Drawing.Point(346, 24)
        Me.LabelAnzGen.Name = "LabelAnzGen"
        Me.LabelAnzGen.Size = New System.Drawing.Size(30, 13)
        Me.LabelAnzGen.TabIndex = 7
        Me.LabelAnzGen.Text = "0"
        '
        'LabelAnzPop
        '
        Me.LabelAnzPop.ForeColor = System.Drawing.Color.Blue
        Me.LabelAnzPop.Location = New System.Drawing.Point(108, 42)
        Me.LabelAnzPop.Name = "LabelAnzPop"
        Me.LabelAnzPop.Size = New System.Drawing.Size(23, 13)
        Me.LabelAnzPop.TabIndex = 6
        Me.LabelAnzPop.Text = "0"
        '
        'LabelAnzRunden
        '
        Me.LabelAnzRunden.ForeColor = System.Drawing.Color.Blue
        Me.LabelAnzRunden.Location = New System.Drawing.Point(108, 24)
        Me.LabelAnzRunden.Name = "LabelAnzRunden"
        Me.LabelAnzRunden.Size = New System.Drawing.Size(23, 13)
        Me.LabelAnzRunden.TabIndex = 5
        Me.LabelAnzRunden.Text = "0"
        '
        'EVO_Opt_Verlauf
        '
        Me.Controls.Add(Me.Anzeige_Verlauf)
        Me.Name = "EVO_Opt_Verlauf"
        Me.Size = New System.Drawing.Size(467, 75)
        Me.Anzeige_Verlauf.ResumeLayout(False)
        Me.Anzeige_Verlauf.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents Anzeige_Verlauf As System.Windows.Forms.GroupBox
    Private WithEvents ProgressBarRunde As System.Windows.Forms.ProgressBar
    Private WithEvents ProgressBarPop As System.Windows.Forms.ProgressBar
    Private WithEvents ProgressBarGen As System.Windows.Forms.ProgressBar
    Private WithEvents ProgressBarNach As System.Windows.Forms.ProgressBar
    Private WithEvents lblNachfolger As System.Windows.Forms.Label
    Private WithEvents lblGeneration As System.Windows.Forms.Label
    Private WithEvents lblPopulation As System.Windows.Forms.Label
    Private WithEvents lblRunde As System.Windows.Forms.Label
    Private WithEvents lblvon_4 As System.Windows.Forms.Label
    Private WithEvents lblvon_3 As System.Windows.Forms.Label
    Private WithEvents lblvon_1 As System.Windows.Forms.Label
    Private WithEvents LabelaktNachf As System.Windows.Forms.Label
    Private WithEvents LabelaktGen As System.Windows.Forms.Label
    Private WithEvents LabelaktPop As System.Windows.Forms.Label
    Private WithEvents LabelaktRunde As System.Windows.Forms.Label
    Private WithEvents LabelAnzNachf As System.Windows.Forms.Label
    Private WithEvents LabelAnzGen As System.Windows.Forms.Label
    Private WithEvents LabelAnzPop As System.Windows.Forms.Label
    Private WithEvents LabelAnzRunden As System.Windows.Forms.Label
    Private WithEvents lblvon_2 As System.Windows.Forms.Label

#End Region

End Class