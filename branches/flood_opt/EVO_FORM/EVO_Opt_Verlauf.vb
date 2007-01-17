Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("EVO_Opt_Verlauf_NET.EVO_Opt_Verlauf")> Public Class EVO_Opt_Verlauf
	Inherits System.Windows.Forms.UserControl
#Region "Vom Windows Form-Designer generierter Code "
	Public Sub New()
		MyBase.New()
		'Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		InitializeComponent()
	End Sub
	'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
	Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Wird vom Windows Form-Designer benötigt.
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ProgressBarRunde As System.Windows.Forms.ProgressBar
    Friend WithEvents ProgressBarPop As System.Windows.Forms.ProgressBar
    Friend WithEvents ProgressBarGen As System.Windows.Forms.ProgressBar
    Friend WithEvents ProgressBarNach As System.Windows.Forms.ProgressBar
	Friend WithEvents lblNachfolger As System.Windows.Forms.Label
	Friend WithEvents lblGeneration As System.Windows.Forms.Label
	Friend WithEvents lblPopulation As System.Windows.Forms.Label
	Friend WithEvents lblRunde As System.Windows.Forms.Label
	Friend WithEvents _lblvon_4 As System.Windows.Forms.Label
	Friend WithEvents _lblvon_3 As System.Windows.Forms.Label
	Friend WithEvents _lblvon_2 As System.Windows.Forms.Label
	Friend WithEvents _lblvon_1 As System.Windows.Forms.Label
	Friend WithEvents LabelaktNachf As System.Windows.Forms.Label
	Friend WithEvents LabelaktGen As System.Windows.Forms.Label
	Friend WithEvents LabelaktPop As System.Windows.Forms.Label
	Friend WithEvents LabelaktRunde As System.Windows.Forms.Label
	Friend WithEvents LabelAnzNachf As System.Windows.Forms.Label
	Friend WithEvents LabelAnzGen As System.Windows.Forms.Label
	Friend WithEvents LabelAnzPop As System.Windows.Forms.Label
	Friend WithEvents LabelAnzRunden As System.Windows.Forms.Label
	Friend WithEvents Anzeige_Verlauf As System.Windows.Forms.GroupBox
	Friend WithEvents lblvon As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	'Hinweis: Die folgende Prozedur wird vom Windows Form-Designer benötigt.
	'Das Verändern mit dem Windows Form-Designer ist nicht möglich.
	'Das Verändern mit dem Code-Editor ist nicht möglich.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
Me.components = New System.ComponentModel.Container
Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(EVO_Opt_Verlauf))
Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
Me.Anzeige_Verlauf = New System.Windows.Forms.GroupBox
Me.ProgressBarRunde = New System.Windows.Forms.ProgressBar
Me.ProgressBarPop = New System.Windows.Forms.ProgressBar
Me.ProgressBarGen = New System.Windows.Forms.ProgressBar
Me.ProgressBarNach = New System.Windows.Forms.ProgressBar
Me.lblNachfolger = New System.Windows.Forms.Label
Me.lblGeneration = New System.Windows.Forms.Label
Me.lblPopulation = New System.Windows.Forms.Label
Me.lblRunde = New System.Windows.Forms.Label
Me._lblvon_4 = New System.Windows.Forms.Label
Me._lblvon_3 = New System.Windows.Forms.Label
Me._lblvon_2 = New System.Windows.Forms.Label
Me._lblvon_1 = New System.Windows.Forms.Label
Me.LabelaktNachf = New System.Windows.Forms.Label
Me.LabelaktGen = New System.Windows.Forms.Label
Me.LabelaktPop = New System.Windows.Forms.Label
Me.LabelaktRunde = New System.Windows.Forms.Label
Me.LabelAnzNachf = New System.Windows.Forms.Label
Me.LabelAnzGen = New System.Windows.Forms.Label
Me.LabelAnzPop = New System.Windows.Forms.Label
Me.LabelAnzRunden = New System.Windows.Forms.Label
Me.lblvon = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
Me.Anzeige_Verlauf.SuspendLayout()
'CType(Me.ProgressBarRunde, System.ComponentModel.ISupportInitialize).BeginInit()
'CType(Me.ProgressBarPop, System.ComponentModel.ISupportInitialize).BeginInit()
'CType(Me.ProgressBarGen, System.ComponentModel.ISupportInitialize).BeginInit()
'CType(Me.ProgressBarNach, System.ComponentModel.ISupportInitialize).BeginInit()
CType(Me.lblvon, System.ComponentModel.ISupportInitialize).BeginInit()
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
Me.Anzeige_Verlauf.Controls.Add(Me._lblvon_4)
Me.Anzeige_Verlauf.Controls.Add(Me._lblvon_3)
Me.Anzeige_Verlauf.Controls.Add(Me._lblvon_2)
Me.Anzeige_Verlauf.Controls.Add(Me._lblvon_1)
Me.Anzeige_Verlauf.Controls.Add(Me.LabelaktNachf)
Me.Anzeige_Verlauf.Controls.Add(Me.LabelaktGen)
Me.Anzeige_Verlauf.Controls.Add(Me.LabelaktPop)
Me.Anzeige_Verlauf.Controls.Add(Me.LabelaktRunde)
Me.Anzeige_Verlauf.Controls.Add(Me.LabelAnzNachf)
Me.Anzeige_Verlauf.Controls.Add(Me.LabelAnzGen)
Me.Anzeige_Verlauf.Controls.Add(Me.LabelAnzPop)
Me.Anzeige_Verlauf.Controls.Add(Me.LabelAnzRunden)
Me.Anzeige_Verlauf.ForeColor = System.Drawing.SystemColors.ControlText
Me.Anzeige_Verlauf.Location = New System.Drawing.Point(0, 0)
Me.Anzeige_Verlauf.Name = "Anzeige_Verlauf"
Me.Anzeige_Verlauf.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.Anzeige_Verlauf.Size = New System.Drawing.Size(465, 73)
Me.Anzeige_Verlauf.TabIndex = 0
Me.Anzeige_Verlauf.TabStop = False
Me.Anzeige_Verlauf.Text = "Verlauf der Optimierung"
'
'ProgressBarRunde
'
'Me.ProgressBarRunde.ContainingControl = Me
Me.ProgressBarRunde.Location = New System.Drawing.Point(136, 24)
Me.ProgressBarRunde.Name = "ProgressBarRunde"
'Me.ProgressBarRunde.OcxState = CType(resources.GetObject("ProgressBarRunde.OcxState"), System.Windows.Forms.AxHost.State)
Me.ProgressBarRunde.Size = New System.Drawing.Size(73, 17)
Me.ProgressBarRunde.TabIndex = 1
'
'ProgressBarPop
'
'Me.ProgressBarPop.ContainingControl = Me
Me.ProgressBarPop.Location = New System.Drawing.Point(136, 40)
Me.ProgressBarPop.Name = "ProgressBarPop"
'Me.ProgressBarPop.OcxState = CType(resources.GetObject("ProgressBarPop.OcxState"), System.Windows.Forms.AxHost.State)
Me.ProgressBarPop.Size = New System.Drawing.Size(73, 17)
Me.ProgressBarPop.TabIndex = 2
'
'ProgressBarGen
'
'Me.ProgressBarGen.ContainingControl = Me
Me.ProgressBarGen.Location = New System.Drawing.Point(376, 24)
Me.ProgressBarGen.Name = "ProgressBarGen"
'Me.ProgressBarGen.OcxState = CType(resources.GetObject("ProgressBarGen.OcxState"), System.Windows.Forms.AxHost.State)
Me.ProgressBarGen.Size = New System.Drawing.Size(73, 17)
Me.ProgressBarGen.TabIndex = 3
'
'ProgressBarNach
'
'Me.ProgressBarNach.ContainingControl = Me
Me.ProgressBarNach.Location = New System.Drawing.Point(376, 40)
Me.ProgressBarNach.Name = "ProgressBarNach"
'Me.ProgressBarNach.OcxState = CType(resources.GetObject("ProgressBarNach.OcxState"), System.Windows.Forms.AxHost.State)
Me.ProgressBarNach.Size = New System.Drawing.Size(73, 17)
Me.ProgressBarNach.TabIndex = 4
'
'lblNachfolger
'
Me.lblNachfolger.AutoSize = True
Me.lblNachfolger.BackColor = System.Drawing.SystemColors.Control
Me.lblNachfolger.Cursor = System.Windows.Forms.Cursors.Default
Me.lblNachfolger.ForeColor = System.Drawing.SystemColors.ControlText
Me.lblNachfolger.Location = New System.Drawing.Point(216, 42)
Me.lblNachfolger.Name = "lblNachfolger"
Me.lblNachfolger.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.lblNachfolger.Size = New System.Drawing.Size(59, 16)
Me.lblNachfolger.TabIndex = 20
Me.lblNachfolger.Text = "Nachfolger"
'
'lblGeneration
'
Me.lblGeneration.AutoSize = True
Me.lblGeneration.BackColor = System.Drawing.SystemColors.Control
Me.lblGeneration.Cursor = System.Windows.Forms.Cursors.Default
Me.lblGeneration.ForeColor = System.Drawing.SystemColors.ControlText
Me.lblGeneration.Location = New System.Drawing.Point(216, 24)
Me.lblGeneration.Name = "lblGeneration"
Me.lblGeneration.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.lblGeneration.Size = New System.Drawing.Size(60, 16)
Me.lblGeneration.TabIndex = 19
Me.lblGeneration.Text = "Generation"
'
'lblPopulation
'
Me.lblPopulation.AutoSize = True
Me.lblPopulation.BackColor = System.Drawing.SystemColors.Control
Me.lblPopulation.Cursor = System.Windows.Forms.Cursors.Default
Me.lblPopulation.ForeColor = System.Drawing.SystemColors.ControlText
Me.lblPopulation.Location = New System.Drawing.Point(16, 42)
Me.lblPopulation.Name = "lblPopulation"
Me.lblPopulation.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.lblPopulation.Size = New System.Drawing.Size(58, 16)
Me.lblPopulation.TabIndex = 18
Me.lblPopulation.Text = "Population"
'
'lblRunde
'
Me.lblRunde.AutoSize = True
Me.lblRunde.BackColor = System.Drawing.SystemColors.Control
Me.lblRunde.Cursor = System.Windows.Forms.Cursors.Default
Me.lblRunde.ForeColor = System.Drawing.SystemColors.ControlText
Me.lblRunde.Location = New System.Drawing.Point(16, 24)
Me.lblRunde.Name = "lblRunde"
Me.lblRunde.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.lblRunde.Size = New System.Drawing.Size(38, 16)
Me.lblRunde.TabIndex = 17
Me.lblRunde.Text = "Runde"
'
'_lblvon_4
'
Me._lblvon_4.AutoSize = True
Me._lblvon_4.BackColor = System.Drawing.SystemColors.Control
Me._lblvon_4.Cursor = System.Windows.Forms.Cursors.Default
Me._lblvon_4.ForeColor = System.Drawing.SystemColors.ControlText
Me.lblvon.SetIndex(Me._lblvon_4, CType(4, Short))
Me._lblvon_4.Location = New System.Drawing.Point(315, 42)
Me._lblvon_4.Name = "_lblvon_4"
Me._lblvon_4.RightToLeft = System.Windows.Forms.RightToLeft.No
Me._lblvon_4.Size = New System.Drawing.Size(22, 16)
Me._lblvon_4.TabIndex = 16
Me._lblvon_4.Text = "von"
'
'_lblvon_3
'
Me._lblvon_3.AutoSize = True
Me._lblvon_3.BackColor = System.Drawing.SystemColors.Control
Me._lblvon_3.Cursor = System.Windows.Forms.Cursors.Default
Me._lblvon_3.ForeColor = System.Drawing.SystemColors.ControlText
Me.lblvon.SetIndex(Me._lblvon_3, CType(3, Short))
Me._lblvon_3.Location = New System.Drawing.Point(315, 24)
Me._lblvon_3.Name = "_lblvon_3"
Me._lblvon_3.RightToLeft = System.Windows.Forms.RightToLeft.No
Me._lblvon_3.Size = New System.Drawing.Size(22, 16)
Me._lblvon_3.TabIndex = 15
Me._lblvon_3.Text = "von"
'
'_lblvon_2
'
Me._lblvon_2.AutoSize = True
Me._lblvon_2.BackColor = System.Drawing.SystemColors.Control
Me._lblvon_2.Cursor = System.Windows.Forms.Cursors.Default
Me._lblvon_2.ForeColor = System.Drawing.SystemColors.ControlText
Me.lblvon.SetIndex(Me._lblvon_2, CType(2, Short))
Me._lblvon_2.Location = New System.Drawing.Point(91, 42)
Me._lblvon_2.Name = "_lblvon_2"
Me._lblvon_2.RightToLeft = System.Windows.Forms.RightToLeft.No
Me._lblvon_2.Size = New System.Drawing.Size(22, 16)
Me._lblvon_2.TabIndex = 14
Me._lblvon_2.Text = "von"
'
'_lblvon_1
'
Me._lblvon_1.AutoSize = True
Me._lblvon_1.BackColor = System.Drawing.SystemColors.Control
Me._lblvon_1.Cursor = System.Windows.Forms.Cursors.Default
Me._lblvon_1.ForeColor = System.Drawing.SystemColors.ControlText
Me.lblvon.SetIndex(Me._lblvon_1, CType(1, Short))
Me._lblvon_1.Location = New System.Drawing.Point(91, 24)
Me._lblvon_1.Name = "_lblvon_1"
Me._lblvon_1.RightToLeft = System.Windows.Forms.RightToLeft.No
Me._lblvon_1.Size = New System.Drawing.Size(22, 16)
Me._lblvon_1.TabIndex = 13
Me._lblvon_1.Text = "von"
'
'LabelaktNachf
'
Me.LabelaktNachf.BackColor = System.Drawing.Color.Transparent
Me.LabelaktNachf.Cursor = System.Windows.Forms.Cursors.Default
Me.LabelaktNachf.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(128, Byte))
Me.LabelaktNachf.Location = New System.Drawing.Point(288, 42)
Me.LabelaktNachf.Name = "LabelaktNachf"
Me.LabelaktNachf.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.LabelaktNachf.Size = New System.Drawing.Size(23, 17)
Me.LabelaktNachf.TabIndex = 12
Me.LabelaktNachf.Text = "1"
Me.LabelaktNachf.TextAlign = System.Drawing.ContentAlignment.TopRight
'
'LabelaktGen
'
Me.LabelaktGen.BackColor = System.Drawing.Color.Transparent
Me.LabelaktGen.Cursor = System.Windows.Forms.Cursors.Default
Me.LabelaktGen.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(128, Byte))
Me.LabelaktGen.Location = New System.Drawing.Point(288, 24)
Me.LabelaktGen.Name = "LabelaktGen"
Me.LabelaktGen.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.LabelaktGen.Size = New System.Drawing.Size(23, 17)
Me.LabelaktGen.TabIndex = 11
Me.LabelaktGen.Text = "1"
Me.LabelaktGen.TextAlign = System.Drawing.ContentAlignment.TopRight
'
'LabelaktPop
'
Me.LabelaktPop.BackColor = System.Drawing.Color.Transparent
Me.LabelaktPop.Cursor = System.Windows.Forms.Cursors.Default
Me.LabelaktPop.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(128, Byte))
Me.LabelaktPop.Location = New System.Drawing.Point(64, 42)
Me.LabelaktPop.Name = "LabelaktPop"
Me.LabelaktPop.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.LabelaktPop.Size = New System.Drawing.Size(23, 17)
Me.LabelaktPop.TabIndex = 10
Me.LabelaktPop.Text = "1"
Me.LabelaktPop.TextAlign = System.Drawing.ContentAlignment.TopRight
'
'LabelaktRunde
'
Me.LabelaktRunde.BackColor = System.Drawing.Color.Transparent
Me.LabelaktRunde.Cursor = System.Windows.Forms.Cursors.Default
Me.LabelaktRunde.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(128, Byte))
Me.LabelaktRunde.Location = New System.Drawing.Point(64, 24)
Me.LabelaktRunde.Name = "LabelaktRunde"
Me.LabelaktRunde.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.LabelaktRunde.Size = New System.Drawing.Size(23, 17)
Me.LabelaktRunde.TabIndex = 9
Me.LabelaktRunde.Text = "1"
Me.LabelaktRunde.TextAlign = System.Drawing.ContentAlignment.TopRight
'
'LabelAnzNachf
'
Me.LabelAnzNachf.BackColor = System.Drawing.Color.Transparent
Me.LabelAnzNachf.Cursor = System.Windows.Forms.Cursors.Default
Me.LabelAnzNachf.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(128, Byte))
Me.LabelAnzNachf.Location = New System.Drawing.Point(344, 42)
Me.LabelAnzNachf.Name = "LabelAnzNachf"
Me.LabelAnzNachf.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.LabelAnzNachf.Size = New System.Drawing.Size(23, 17)
Me.LabelAnzNachf.TabIndex = 8
Me.LabelAnzNachf.Text = "1"
Me.LabelAnzNachf.TextAlign = System.Drawing.ContentAlignment.TopRight
'
'LabelAnzGen
'
Me.LabelAnzGen.BackColor = System.Drawing.Color.Transparent
Me.LabelAnzGen.Cursor = System.Windows.Forms.Cursors.Default
Me.LabelAnzGen.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(128, Byte))
Me.LabelAnzGen.Location = New System.Drawing.Point(344, 24)
Me.LabelAnzGen.Name = "LabelAnzGen"
Me.LabelAnzGen.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.LabelAnzGen.Size = New System.Drawing.Size(23, 17)
Me.LabelAnzGen.TabIndex = 7
Me.LabelAnzGen.Text = "1"
Me.LabelAnzGen.TextAlign = System.Drawing.ContentAlignment.TopRight
'
'LabelAnzPop
'
Me.LabelAnzPop.BackColor = System.Drawing.Color.Transparent
Me.LabelAnzPop.Cursor = System.Windows.Forms.Cursors.Default
Me.LabelAnzPop.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(128, Byte))
Me.LabelAnzPop.Location = New System.Drawing.Point(104, 42)
Me.LabelAnzPop.Name = "LabelAnzPop"
Me.LabelAnzPop.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.LabelAnzPop.Size = New System.Drawing.Size(23, 17)
Me.LabelAnzPop.TabIndex = 6
Me.LabelAnzPop.Text = "1"
Me.LabelAnzPop.TextAlign = System.Drawing.ContentAlignment.TopRight
'
'LabelAnzRunden
'
Me.LabelAnzRunden.BackColor = System.Drawing.Color.Transparent
Me.LabelAnzRunden.Cursor = System.Windows.Forms.Cursors.Default
Me.LabelAnzRunden.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(128, Byte))
Me.LabelAnzRunden.Location = New System.Drawing.Point(104, 24)
Me.LabelAnzRunden.Name = "LabelAnzRunden"
Me.LabelAnzRunden.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.LabelAnzRunden.Size = New System.Drawing.Size(23, 17)
Me.LabelAnzRunden.TabIndex = 5
Me.LabelAnzRunden.Text = "1"
Me.LabelAnzRunden.TextAlign = System.Drawing.ContentAlignment.TopRight
'
'EVO_Opt_Verlauf
'
Me.Controls.Add(Me.Anzeige_Verlauf)
Me.Name = "EVO_Opt_Verlauf"
Me.Size = New System.Drawing.Size(467, 75)
Me.Anzeige_Verlauf.ResumeLayout(False)
'CType(Me.ProgressBarRunde, System.ComponentModel.ISupportInitialize).EndInit()
'CType(Me.ProgressBarPop, System.ComponentModel.ISupportInitialize).EndInit()
'CType(Me.ProgressBarGen, System.ComponentModel.ISupportInitialize).EndInit()
'CType(Me.ProgressBarNach, System.ComponentModel.ISupportInitialize).EndInit()
CType(Me.lblvon, System.ComponentModel.ISupportInitialize).EndInit()
Me.ResumeLayout(False)

    End Sub
#End Region 
	
    Private Structure EigenschaftTyp
        Dim NRunden As Short
        Dim NPopul As Short
        Dim NGen As Short
        Dim NNachf As Short
    End Structure

    Dim Eigenschaft As EigenschaftTyp

    '********************************************************************
    'Schnittstelle
    '********************************************************************


    Public Property NRunden() As Short
        Get
            NRunden = Eigenschaft.NRunden
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.NRunden = Value
        End Set
    End Property


    Public Property NPopul() As Short
        Get
            NPopul = Eigenschaft.NPopul
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.NPopul = Value
        End Set
    End Property


    Public Property NGen() As Short
        Get
            NGen = Eigenschaft.NGen
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.NGen = Value
        End Set
    End Property


    Public Property NNachf() As Short
        Get
            NNachf = Eigenschaft.NNachf
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.NNachf = Value
        End Set
    End Property

    Public Sub Initialisieren()
        LabelAnzRunden.Text = Eigenschaft.NRunden
        LabelAnzPop.Text = Eigenschaft.NPopul
        LabelAnzGen.Text = Eigenschaft.NGen
        LabelAnzNachf.Text = Eigenschaft.NNachf

        ProgressBarRunde.Visible = True
        ProgressBarRunde.Minimum = 0

        ProgressBarRunde.Maximum = Eigenschaft.NRunden
        ProgressBarRunde.Value = 1
        ProgressBarPop.Visible = True
        ProgressBarPop.Minimum = 0
        ProgressBarPop.Maximum = Eigenschaft.NPopul
        ProgressBarPop.Value = 1

        ProgressBarGen.Visible = True
        ProgressBarGen.Minimum = 0
        ProgressBarGen.Maximum = Eigenschaft.NGen
        ProgressBarGen.Value = 1

        ProgressBarNach.Visible = True
        ProgressBarNach.Minimum = 0
        ProgressBarNach.Maximum = Eigenschaft.NNachf
        ProgressBarNach.Value = 1
    End Sub


    Public Sub Runden(ByRef NRunden As Short)
        Eigenschaft.NRunden = NRunden
        ProgressBarRunde.Value = Eigenschaft.NRunden
        LabelaktRunde.Text = Eigenschaft.NRunden
    End Sub

    Public Sub Populationen(ByRef NPopul As Short)
        Eigenschaft.NPopul = NPopul
        ProgressBarPop.Value = Eigenschaft.NPopul
        LabelaktPop.Text = VB6.Format(Eigenschaft.NPopul)
    End Sub

    Public Sub Generation(ByRef NGen As Short)
        Eigenschaft.NGen = NGen
        ProgressBarGen.Value = Eigenschaft.NGen
        LabelaktGen.Text = VB6.Format(Eigenschaft.NGen)
    End Sub

    Public Sub Nachfolger(ByRef NNachf As Short)
        Eigenschaft.NNachf = NNachf
        ProgressBarNach.Value = Eigenschaft.NNachf
        LabelaktNachf.Text = VB6.Format(Eigenschaft.NNachf)
    End Sub

    Private Sub LabelAnzRunden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelAnzRunden.Click

    End Sub
End Class