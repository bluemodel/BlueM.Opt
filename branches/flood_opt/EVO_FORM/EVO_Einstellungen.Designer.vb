<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EVO_Einstellungen

    Inherits System.Windows.Forms.UserControl

    Public Sub New()
        MyBase.New()
        'Dieser Aufruf ist f�r den Windows Form-Designer erforderlich.
        InitializeComponent()
        UserControl_Initialize()
        System.Windows.Forms.Application.EnableVisualStyles()
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
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ComboModus As System.Windows.Forms.ComboBox
    Friend WithEvents CheckisDnVektor As System.Windows.Forms.CheckBox
    Friend WithEvents ComboOptVorgabe As System.Windows.Forms.ComboBox
    Friend WithEvents TextDeltaStart As System.Windows.Forms.TextBox
    Friend WithEvents CheckisPopul As System.Windows.Forms.CheckBox
    Friend WithEvents ComboStrategie As System.Windows.Forms.ComboBox
    Friend WithEvents TextNMemberSecondPop As System.Windows.Forms.TextBox
    Friend WithEvents TextInteract As System.Windows.Forms.TextBox
    Friend WithEvents TextRekombxy As System.Windows.Forms.TextBox
    Friend WithEvents ComboOptEltern As System.Windows.Forms.ComboBox
    Friend WithEvents TextAnzNachf As System.Windows.Forms.TextBox
    Friend WithEvents TextAnzEltern As System.Windows.Forms.TextBox
    Friend WithEvents TextAnzGen As System.Windows.Forms.TextBox
    Friend WithEvents LabelNMemberSecondPop As System.Windows.Forms.Label
    Friend WithEvents LabelInteract As System.Windows.Forms.Label
    Friend WithEvents Line2 As System.Windows.Forms.Label
    Friend WithEvents LabelRekombxy3 As System.Windows.Forms.Label
    Friend WithEvents LabelRekombxy1 As System.Windows.Forms.Label
    Friend WithEvents Line1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents _Label1_8 As System.Windows.Forms.Label
    Friend WithEvents _Label1_7 As System.Windows.Forms.Label
    Friend WithEvents _Label1_6 As System.Windows.Forms.Label
    Friend WithEvents FrameGen As System.Windows.Forms.GroupBox
    Friend WithEvents ComboPopPenalty As System.Windows.Forms.ComboBox
    Friend WithEvents ComboPopStrategie As System.Windows.Forms.ComboBox
    Friend WithEvents ComboOptPopEltern As System.Windows.Forms.ComboBox
    Friend WithEvents TextAnzPopEltern As System.Windows.Forms.TextBox
    Friend WithEvents TextAnzPop As System.Windows.Forms.TextBox
    Friend WithEvents TextAnzRunden As System.Windows.Forms.TextBox
    Friend WithEvents _LabelFramePop_6 As System.Windows.Forms.Label
    Friend WithEvents _LabelFramePop_5 As System.Windows.Forms.Label
    Friend WithEvents _LabelFramePop_4 As System.Windows.Forms.Label
    Friend WithEvents _LabelFramePop_3 As System.Windows.Forms.Label
    Friend WithEvents _LabelFramePop_2 As System.Windows.Forms.Label
    Friend WithEvents _LabelFramePop_1 As System.Windows.Forms.Label
    Friend WithEvents FramePop As System.Windows.Forms.GroupBox
    Friend WithEvents _Label1_1 As System.Windows.Forms.Label
    Friend WithEvents _Label1_15 As System.Windows.Forms.Label
    Friend WithEvents _Label1_12 As System.Windows.Forms.Label
    Friend WithEvents _Label1_0 As System.Windows.Forms.Label
    Friend WithEvents FrameOptions As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
    Friend WithEvents LabelFramePop As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
    'Hinweis: Die folgende Prozedur wird vom Windows Form-Designer ben�tigt.
    'Das Ver�ndern mit dem Windows Form-Designer ist nicht m�glich.
    'Das Ver�ndern mit dem Code-Editor ist nicht m�glich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.FrameOptions = New System.Windows.Forms.GroupBox
        Me.ComboModus = New System.Windows.Forms.ComboBox
        Me.CheckisDnVektor = New System.Windows.Forms.CheckBox
        Me.ComboOptVorgabe = New System.Windows.Forms.ComboBox
        Me.TextDeltaStart = New System.Windows.Forms.TextBox
        Me.CheckisPopul = New System.Windows.Forms.CheckBox
        Me.ComboStrategie = New System.Windows.Forms.ComboBox
        Me.FrameGen = New System.Windows.Forms.GroupBox
        Me.TextNMemberSecondPop = New System.Windows.Forms.TextBox
        Me.TextInteract = New System.Windows.Forms.TextBox
        Me.TextRekombxy = New System.Windows.Forms.TextBox
        Me.ComboOptEltern = New System.Windows.Forms.ComboBox
        Me.TextAnzNachf = New System.Windows.Forms.TextBox
        Me.TextAnzEltern = New System.Windows.Forms.TextBox
        Me.TextAnzGen = New System.Windows.Forms.TextBox
        Me.LabelNMemberSecondPop = New System.Windows.Forms.Label
        Me.LabelInteract = New System.Windows.Forms.Label
        Me.Line2 = New System.Windows.Forms.Label
        Me.LabelRekombxy3 = New System.Windows.Forms.Label
        Me.LabelRekombxy1 = New System.Windows.Forms.Label
        Me.Line1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me._Label1_8 = New System.Windows.Forms.Label
        Me._Label1_7 = New System.Windows.Forms.Label
        Me._Label1_6 = New System.Windows.Forms.Label
        Me.FramePop = New System.Windows.Forms.GroupBox
        Me.ComboPopPenalty = New System.Windows.Forms.ComboBox
        Me.ComboPopStrategie = New System.Windows.Forms.ComboBox
        Me.ComboOptPopEltern = New System.Windows.Forms.ComboBox
        Me.TextAnzPopEltern = New System.Windows.Forms.TextBox
        Me.TextAnzPop = New System.Windows.Forms.TextBox
        Me.TextAnzRunden = New System.Windows.Forms.TextBox
        Me._LabelFramePop_6 = New System.Windows.Forms.Label
        Me._LabelFramePop_5 = New System.Windows.Forms.Label
        Me._LabelFramePop_4 = New System.Windows.Forms.Label
        Me._LabelFramePop_3 = New System.Windows.Forms.Label
        Me._LabelFramePop_2 = New System.Windows.Forms.Label
        Me._LabelFramePop_1 = New System.Windows.Forms.Label
        Me._Label1_1 = New System.Windows.Forms.Label
        Me._Label1_15 = New System.Windows.Forms.Label
        Me._Label1_12 = New System.Windows.Forms.Label
        Me._Label1_0 = New System.Windows.Forms.Label
        Me.Label1 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.LabelFramePop = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.FrameOptions.SuspendLayout()
        Me.FrameGen.SuspendLayout()
        Me.FramePop.SuspendLayout()
        CType(Me.Label1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LabelFramePop, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'FrameOptions
        '
        Me.FrameOptions.BackColor = System.Drawing.SystemColors.Control
        Me.FrameOptions.Controls.Add(Me.CheckisDnVektor)
        Me.FrameOptions.Controls.Add(Me.ComboModus)
        Me.FrameOptions.Controls.Add(Me.ComboOptVorgabe)
        Me.FrameOptions.Controls.Add(Me.TextDeltaStart)
        Me.FrameOptions.Controls.Add(Me.CheckisPopul)
        Me.FrameOptions.Controls.Add(Me.ComboStrategie)
        Me.FrameOptions.Controls.Add(Me.FrameGen)
        Me.FrameOptions.Controls.Add(Me.FramePop)
        Me.FrameOptions.Controls.Add(Me._Label1_1)
        Me.FrameOptions.Controls.Add(Me._Label1_15)
        Me.FrameOptions.Controls.Add(Me._Label1_12)
        Me.FrameOptions.Controls.Add(Me._Label1_0)
        Me.FrameOptions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FrameOptions.Location = New System.Drawing.Point(0, 0)
        Me.FrameOptions.Name = "FrameOptions"
        Me.FrameOptions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameOptions.Size = New System.Drawing.Size(217, 585)
        Me.FrameOptions.TabIndex = 0
        Me.FrameOptions.TabStop = False
        Me.FrameOptions.Text = "Einstellungen"
        '
        'ComboModus
        '
        Me.ComboModus.BackColor = System.Drawing.SystemColors.Window
        Me.ComboModus.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboModus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboModus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboModus.Location = New System.Drawing.Point(84, 64)
        Me.ComboModus.Name = "ComboModus"
        Me.ComboModus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboModus.Size = New System.Drawing.Size(123, 21)
        Me.ComboModus.TabIndex = 33
        '
        'CheckisDnVektor
        '
        Me.CheckisDnVektor.BackColor = System.Drawing.SystemColors.Control
        Me.CheckisDnVektor.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckisDnVektor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckisDnVektor.Location = New System.Drawing.Point(16, 120)
        Me.CheckisDnVektor.Name = "CheckisDnVektor"
        Me.CheckisDnVektor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckisDnVektor.Size = New System.Drawing.Size(144, 13)
        Me.CheckisDnVektor.TabIndex = 30
        Me.CheckisDnVektor.Text = "mit Schrittweitenvektor"
        Me.CheckisDnVektor.UseVisualStyleBackColor = False
        '
        'ComboOptVorgabe
        '
        Me.ComboOptVorgabe.BackColor = System.Drawing.SystemColors.Window
        Me.ComboOptVorgabe.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboOptVorgabe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptVorgabe.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboOptVorgabe.Location = New System.Drawing.Point(84, 40)
        Me.ComboOptVorgabe.Name = "ComboOptVorgabe"
        Me.ComboOptVorgabe.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboOptVorgabe.Size = New System.Drawing.Size(123, 21)
        Me.ComboOptVorgabe.TabIndex = 28
        '
        'TextDeltaStart
        '
        Me.TextDeltaStart.AcceptsReturn = True
        Me.TextDeltaStart.BackColor = System.Drawing.SystemColors.Window
        Me.TextDeltaStart.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextDeltaStart.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextDeltaStart.Location = New System.Drawing.Point(164, 96)
        Me.TextDeltaStart.MaxLength = 0
        Me.TextDeltaStart.Name = "TextDeltaStart"
        Me.TextDeltaStart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextDeltaStart.Size = New System.Drawing.Size(43, 20)
        Me.TextDeltaStart.TabIndex = 26
        Me.TextDeltaStart.Text = "0.1"
        Me.TextDeltaStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CheckisPopul
        '
        Me.CheckisPopul.BackColor = System.Drawing.SystemColors.Control
        Me.CheckisPopul.Cursor = System.Windows.Forms.Cursors.Default
        Me.CheckisPopul.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckisPopul.Location = New System.Drawing.Point(16, 376)
        Me.CheckisPopul.Name = "CheckisPopul"
        Me.CheckisPopul.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckisPopul.Size = New System.Drawing.Size(112, 16)
        Me.CheckisPopul.TabIndex = 18
        Me.CheckisPopul.Text = "mit Populationen"
        Me.CheckisPopul.UseVisualStyleBackColor = False
        '
        'ComboStrategie
        '
        Me.ComboStrategie.BackColor = System.Drawing.SystemColors.Window
        Me.ComboStrategie.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboStrategie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboStrategie.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboStrategie.Location = New System.Drawing.Point(84, 16)
        Me.ComboStrategie.Name = "ComboStrategie"
        Me.ComboStrategie.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboStrategie.Size = New System.Drawing.Size(123, 21)
        Me.ComboStrategie.TabIndex = 17
        '
        'FrameGen
        '
        Me.FrameGen.BackColor = System.Drawing.SystemColors.Control
        Me.FrameGen.Controls.Add(Me.TextNMemberSecondPop)
        Me.FrameGen.Controls.Add(Me.TextInteract)
        Me.FrameGen.Controls.Add(Me.TextRekombxy)
        Me.FrameGen.Controls.Add(Me.ComboOptEltern)
        Me.FrameGen.Controls.Add(Me.TextAnzNachf)
        Me.FrameGen.Controls.Add(Me.TextAnzEltern)
        Me.FrameGen.Controls.Add(Me.TextAnzGen)
        Me.FrameGen.Controls.Add(Me.LabelNMemberSecondPop)
        Me.FrameGen.Controls.Add(Me.LabelInteract)
        Me.FrameGen.Controls.Add(Me.Line2)
        Me.FrameGen.Controls.Add(Me.LabelRekombxy3)
        Me.FrameGen.Controls.Add(Me.LabelRekombxy1)
        Me.FrameGen.Controls.Add(Me.Line1)
        Me.FrameGen.Controls.Add(Me.Label2)
        Me.FrameGen.Controls.Add(Me._Label1_8)
        Me.FrameGen.Controls.Add(Me._Label1_7)
        Me.FrameGen.Controls.Add(Me._Label1_6)
        Me.FrameGen.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FrameGen.Location = New System.Drawing.Point(8, 144)
        Me.FrameGen.Name = "FrameGen"
        Me.FrameGen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FrameGen.Size = New System.Drawing.Size(199, 217)
        Me.FrameGen.TabIndex = 10
        Me.FrameGen.TabStop = False
        Me.FrameGen.Text = "Generationen:"
        '
        'TextNMemberSecondPop
        '
        Me.TextNMemberSecondPop.AcceptsReturn = True
        Me.TextNMemberSecondPop.BackColor = System.Drawing.SystemColors.Window
        Me.TextNMemberSecondPop.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextNMemberSecondPop.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextNMemberSecondPop.Location = New System.Drawing.Point(140, 120)
        Me.TextNMemberSecondPop.MaxLength = 0
        Me.TextNMemberSecondPop.Name = "TextNMemberSecondPop"
        Me.TextNMemberSecondPop.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextNMemberSecondPop.Size = New System.Drawing.Size(43, 20)
        Me.TextNMemberSecondPop.TabIndex = 39
        Me.TextNMemberSecondPop.Text = "50"
        Me.TextNMemberSecondPop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextInteract
        '
        Me.TextInteract.AcceptsReturn = True
        Me.TextInteract.BackColor = System.Drawing.SystemColors.Window
        Me.TextInteract.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextInteract.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextInteract.Location = New System.Drawing.Point(140, 96)
        Me.TextInteract.MaxLength = 0
        Me.TextInteract.Name = "TextInteract"
        Me.TextInteract.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextInteract.Size = New System.Drawing.Size(43, 20)
        Me.TextInteract.TabIndex = 37
        Me.TextInteract.Text = "10"
        Me.TextInteract.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextRekombxy
        '
        Me.TextRekombxy.AcceptsReturn = True
        Me.TextRekombxy.BackColor = System.Drawing.SystemColors.Window
        Me.TextRekombxy.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextRekombxy.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextRekombxy.Location = New System.Drawing.Point(32, 192)
        Me.TextRekombxy.MaxLength = 0
        Me.TextRekombxy.Name = "TextRekombxy"
        Me.TextRekombxy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextRekombxy.Size = New System.Drawing.Size(25, 19)
        Me.TextRekombxy.TabIndex = 23
        Me.TextRekombxy.Text = "2"
        Me.TextRekombxy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ComboOptEltern
        '
        Me.ComboOptEltern.BackColor = System.Drawing.SystemColors.Window
        Me.ComboOptEltern.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboOptEltern.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboOptEltern.Location = New System.Drawing.Point(64, 160)
        Me.ComboOptEltern.Name = "ComboOptEltern"
        Me.ComboOptEltern.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboOptEltern.Size = New System.Drawing.Size(121, 21)
        Me.ComboOptEltern.TabIndex = 20
        Me.ComboOptEltern.Text = "ComboOptEltern"
        '
        'TextAnzNachf
        '
        Me.TextAnzNachf.AcceptsReturn = True
        Me.TextAnzNachf.BackColor = System.Drawing.SystemColors.Window
        Me.TextAnzNachf.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextAnzNachf.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextAnzNachf.Location = New System.Drawing.Point(140, 61)
        Me.TextAnzNachf.MaxLength = 0
        Me.TextAnzNachf.Name = "TextAnzNachf"
        Me.TextAnzNachf.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextAnzNachf.Size = New System.Drawing.Size(43, 20)
        Me.TextAnzNachf.TabIndex = 13
        Me.TextAnzNachf.Text = "10"
        Me.TextAnzNachf.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextAnzEltern
        '
        Me.TextAnzEltern.AcceptsReturn = True
        Me.TextAnzEltern.BackColor = System.Drawing.SystemColors.Window
        Me.TextAnzEltern.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextAnzEltern.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextAnzEltern.Location = New System.Drawing.Point(140, 39)
        Me.TextAnzEltern.MaxLength = 0
        Me.TextAnzEltern.Name = "TextAnzEltern"
        Me.TextAnzEltern.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextAnzEltern.Size = New System.Drawing.Size(43, 20)
        Me.TextAnzEltern.TabIndex = 12
        Me.TextAnzEltern.Text = "3"
        Me.TextAnzEltern.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextAnzGen
        '
        Me.TextAnzGen.AcceptsReturn = True
        Me.TextAnzGen.BackColor = System.Drawing.SystemColors.Window
        Me.TextAnzGen.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextAnzGen.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextAnzGen.Location = New System.Drawing.Point(140, 16)
        Me.TextAnzGen.MaxLength = 0
        Me.TextAnzGen.Name = "TextAnzGen"
        Me.TextAnzGen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextAnzGen.Size = New System.Drawing.Size(43, 20)
        Me.TextAnzGen.TabIndex = 11
        Me.TextAnzGen.Text = "1"
        Me.TextAnzGen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelNMemberSecondPop
        '
        Me.LabelNMemberSecondPop.BackColor = System.Drawing.SystemColors.Control
        Me.LabelNMemberSecondPop.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelNMemberSecondPop.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelNMemberSecondPop.Location = New System.Drawing.Point(8, 120)
        Me.LabelNMemberSecondPop.Name = "LabelNMemberSecondPop"
        Me.LabelNMemberSecondPop.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelNMemberSecondPop.Size = New System.Drawing.Size(128, 16)
        Me.LabelNMemberSecondPop.TabIndex = 40
        Me.LabelNMemberSecondPop.Text = "Max. Mitglieder 2nd Pop"
        '
        'LabelInteract
        '
        Me.LabelInteract.AutoSize = True
        Me.LabelInteract.BackColor = System.Drawing.SystemColors.Control
        Me.LabelInteract.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelInteract.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelInteract.Location = New System.Drawing.Point(8, 96)
        Me.LabelInteract.Name = "LabelInteract"
        Me.LabelInteract.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelInteract.Size = New System.Drawing.Size(121, 13)
        Me.LabelInteract.TabIndex = 38
        Me.LabelInteract.Text = "Austausch mit sek. Pop:"
        '
        'Line2
        '
        Me.Line2.BackColor = System.Drawing.SystemColors.WindowText
        Me.Line2.Location = New System.Drawing.Point(8, 148)
        Me.Line2.Name = "Line2"
        Me.Line2.Size = New System.Drawing.Size(176, 1)
        Me.Line2.TabIndex = 41
        '
        'LabelRekombxy3
        '
        Me.LabelRekombxy3.BackColor = System.Drawing.SystemColors.Control
        Me.LabelRekombxy3.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelRekombxy3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelRekombxy3.Location = New System.Drawing.Point(56, 195)
        Me.LabelRekombxy3.Name = "LabelRekombxy3"
        Me.LabelRekombxy3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelRekombxy3.Size = New System.Drawing.Size(129, 17)
        Me.LabelRekombxy3.TabIndex = 25
        Me.LabelRekombxy3.Text = "-Rekombination"
        '
        'LabelRekombxy1
        '
        Me.LabelRekombxy1.BackColor = System.Drawing.SystemColors.Control
        Me.LabelRekombxy1.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelRekombxy1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelRekombxy1.Location = New System.Drawing.Point(8, 195)
        Me.LabelRekombxy1.Name = "LabelRekombxy1"
        Me.LabelRekombxy1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelRekombxy1.Size = New System.Drawing.Size(24, 17)
        Me.LabelRekombxy1.TabIndex = 22
        Me.LabelRekombxy1.Text = "X /"
        '
        'Line1
        '
        Me.Line1.BackColor = System.Drawing.SystemColors.WindowText
        Me.Line1.Location = New System.Drawing.Point(8, 88)
        Me.Line1.Name = "Line1"
        Me.Line1.Size = New System.Drawing.Size(176, 1)
        Me.Line1.TabIndex = 42
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(8, 155)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(57, 25)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "Ermitteln der Eltern:"
        '
        '_Label1_8
        '
        Me._Label1_8.AutoSize = True
        Me._Label1_8.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.SetIndex(Me._Label1_8, CType(8, Short))
        Me._Label1_8.Location = New System.Drawing.Point(8, 64)
        Me._Label1_8.Name = "_Label1_8"
        Me._Label1_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_8.Size = New System.Drawing.Size(97, 13)
        Me._Label1_8.TabIndex = 16
        Me._Label1_8.Text = "Anzahl Nachfolger:"
        '
        '_Label1_7
        '
        Me._Label1_7.AutoSize = True
        Me._Label1_7.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.SetIndex(Me._Label1_7, CType(7, Short))
        Me._Label1_7.Location = New System.Drawing.Point(7, 42)
        Me._Label1_7.Name = "_Label1_7"
        Me._Label1_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_7.Size = New System.Drawing.Size(112, 13)
        Me._Label1_7.TabIndex = 15
        Me._Label1_7.Text = "Anzahl Eltern [max=5]:"
        '
        '_Label1_6
        '
        Me._Label1_6.AutoSize = True
        Me._Label1_6.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.SetIndex(Me._Label1_6, CType(6, Short))
        Me._Label1_6.Location = New System.Drawing.Point(7, 20)
        Me._Label1_6.Name = "_Label1_6"
        Me._Label1_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_6.Size = New System.Drawing.Size(127, 13)
        Me._Label1_6.TabIndex = 14
        Me._Label1_6.Text = "Anzahl der Generationen:"
        '
        'FramePop
        '
        Me.FramePop.BackColor = System.Drawing.SystemColors.Control
        Me.FramePop.Controls.Add(Me.ComboPopPenalty)
        Me.FramePop.Controls.Add(Me.ComboPopStrategie)
        Me.FramePop.Controls.Add(Me.ComboOptPopEltern)
        Me.FramePop.Controls.Add(Me.TextAnzPopEltern)
        Me.FramePop.Controls.Add(Me.TextAnzPop)
        Me.FramePop.Controls.Add(Me.TextAnzRunden)
        Me.FramePop.Controls.Add(Me._LabelFramePop_6)
        Me.FramePop.Controls.Add(Me._LabelFramePop_5)
        Me.FramePop.Controls.Add(Me._LabelFramePop_4)
        Me.FramePop.Controls.Add(Me._LabelFramePop_3)
        Me.FramePop.Controls.Add(Me._LabelFramePop_2)
        Me.FramePop.Controls.Add(Me._LabelFramePop_1)
        Me.FramePop.Enabled = False
        Me.FramePop.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FramePop.Location = New System.Drawing.Point(8, 392)
        Me.FramePop.Name = "FramePop"
        Me.FramePop.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FramePop.Size = New System.Drawing.Size(199, 177)
        Me.FramePop.TabIndex = 1
        Me.FramePop.TabStop = False
        Me.FramePop.Text = "Populationen:"
        '
        'ComboPopPenalty
        '
        Me.ComboPopPenalty.BackColor = System.Drawing.SystemColors.Window
        Me.ComboPopPenalty.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboPopPenalty.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboPopPenalty.Location = New System.Drawing.Point(80, 138)
        Me.ComboPopPenalty.Name = "ComboPopPenalty"
        Me.ComboPopPenalty.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboPopPenalty.Size = New System.Drawing.Size(113, 21)
        Me.ComboPopPenalty.TabIndex = 35
        Me.ComboPopPenalty.Text = "ComboPopPenalty"
        '
        'ComboPopStrategie
        '
        Me.ComboPopStrategie.BackColor = System.Drawing.SystemColors.Window
        Me.ComboPopStrategie.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboPopStrategie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboPopStrategie.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboPopStrategie.Location = New System.Drawing.Point(80, 114)
        Me.ComboPopStrategie.Name = "ComboPopStrategie"
        Me.ComboPopStrategie.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboPopStrategie.Size = New System.Drawing.Size(113, 21)
        Me.ComboPopStrategie.TabIndex = 32
        '
        'ComboOptPopEltern
        '
        Me.ComboOptPopEltern.BackColor = System.Drawing.SystemColors.Window
        Me.ComboOptPopEltern.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboOptPopEltern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptPopEltern.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboOptPopEltern.Location = New System.Drawing.Point(80, 90)
        Me.ComboOptPopEltern.Name = "ComboOptPopEltern"
        Me.ComboOptPopEltern.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboOptPopEltern.Size = New System.Drawing.Size(113, 21)
        Me.ComboOptPopEltern.TabIndex = 5
        '
        'TextAnzPopEltern
        '
        Me.TextAnzPopEltern.AcceptsReturn = True
        Me.TextAnzPopEltern.BackColor = System.Drawing.SystemColors.Window
        Me.TextAnzPopEltern.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextAnzPopEltern.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextAnzPopEltern.Location = New System.Drawing.Point(140, 58)
        Me.TextAnzPopEltern.MaxLength = 0
        Me.TextAnzPopEltern.Name = "TextAnzPopEltern"
        Me.TextAnzPopEltern.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextAnzPopEltern.Size = New System.Drawing.Size(43, 20)
        Me.TextAnzPopEltern.TabIndex = 4
        Me.TextAnzPopEltern.Text = "2"
        Me.TextAnzPopEltern.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextAnzPop
        '
        Me.TextAnzPop.AcceptsReturn = True
        Me.TextAnzPop.BackColor = System.Drawing.SystemColors.Window
        Me.TextAnzPop.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextAnzPop.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextAnzPop.Location = New System.Drawing.Point(140, 36)
        Me.TextAnzPop.MaxLength = 0
        Me.TextAnzPop.Name = "TextAnzPop"
        Me.TextAnzPop.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextAnzPop.Size = New System.Drawing.Size(43, 20)
        Me.TextAnzPop.TabIndex = 3
        Me.TextAnzPop.Text = "5"
        Me.TextAnzPop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextAnzRunden
        '
        Me.TextAnzRunden.AcceptsReturn = True
        Me.TextAnzRunden.BackColor = System.Drawing.SystemColors.Window
        Me.TextAnzRunden.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextAnzRunden.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextAnzRunden.Location = New System.Drawing.Point(140, 13)
        Me.TextAnzRunden.MaxLength = 0
        Me.TextAnzRunden.Name = "TextAnzRunden"
        Me.TextAnzRunden.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextAnzRunden.Size = New System.Drawing.Size(43, 20)
        Me.TextAnzRunden.TabIndex = 2
        Me.TextAnzRunden.Text = "50"
        Me.TextAnzRunden.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        '_LabelFramePop_6
        '
        Me._LabelFramePop_6.BackColor = System.Drawing.SystemColors.Control
        Me._LabelFramePop_6.Cursor = System.Windows.Forms.Cursors.Default
        Me._LabelFramePop_6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelFramePop.SetIndex(Me._LabelFramePop_6, CType(6, Short))
        Me._LabelFramePop_6.Location = New System.Drawing.Point(8, 131)
        Me._LabelFramePop_6.Name = "_LabelFramePop_6"
        Me._LabelFramePop_6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._LabelFramePop_6.Size = New System.Drawing.Size(80, 29)
        Me._LabelFramePop_6.TabIndex = 36
        Me._LabelFramePop_6.Text = "Ermittlung der Pop-G�te:"
        '
        '_LabelFramePop_5
        '
        Me._LabelFramePop_5.BackColor = System.Drawing.SystemColors.Control
        Me._LabelFramePop_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._LabelFramePop_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelFramePop.SetIndex(Me._LabelFramePop_5, CType(5, Short))
        Me._LabelFramePop_5.Location = New System.Drawing.Point(8, 116)
        Me._LabelFramePop_5.Name = "_LabelFramePop_5"
        Me._LabelFramePop_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._LabelFramePop_5.Size = New System.Drawing.Size(65, 17)
        Me._LabelFramePop_5.TabIndex = 31
        Me._LabelFramePop_5.Text = "Selektion:"
        '
        '_LabelFramePop_4
        '
        Me._LabelFramePop_4.BackColor = System.Drawing.SystemColors.Control
        Me._LabelFramePop_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._LabelFramePop_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelFramePop.SetIndex(Me._LabelFramePop_4, CType(4, Short))
        Me._LabelFramePop_4.Location = New System.Drawing.Point(8, 88)
        Me._LabelFramePop_4.Name = "_LabelFramePop_4"
        Me._LabelFramePop_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._LabelFramePop_4.Size = New System.Drawing.Size(80, 32)
        Me._LabelFramePop_4.TabIndex = 9
        Me._LabelFramePop_4.Text = "Ermittlung der Pop-Eltern:"
        '
        '_LabelFramePop_3
        '
        Me._LabelFramePop_3.AutoSize = True
        Me._LabelFramePop_3.BackColor = System.Drawing.SystemColors.Control
        Me._LabelFramePop_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._LabelFramePop_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelFramePop.SetIndex(Me._LabelFramePop_3, CType(3, Short))
        Me._LabelFramePop_3.Location = New System.Drawing.Point(7, 58)
        Me._LabelFramePop_3.Name = "_LabelFramePop_3"
        Me._LabelFramePop_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._LabelFramePop_3.Size = New System.Drawing.Size(112, 13)
        Me._LabelFramePop_3.TabIndex = 8
        Me._LabelFramePop_3.Text = "Anzahl Eltern [max=5]:"
        '
        '_LabelFramePop_2
        '
        Me._LabelFramePop_2.AutoSize = True
        Me._LabelFramePop_2.BackColor = System.Drawing.SystemColors.Control
        Me._LabelFramePop_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._LabelFramePop_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelFramePop.SetIndex(Me._LabelFramePop_2, CType(2, Short))
        Me._LabelFramePop_2.Location = New System.Drawing.Point(7, 36)
        Me._LabelFramePop_2.Name = "_LabelFramePop_2"
        Me._LabelFramePop_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._LabelFramePop_2.Size = New System.Drawing.Size(107, 13)
        Me._LabelFramePop_2.TabIndex = 7
        Me._LabelFramePop_2.Text = "Anzahl Populationen:"
        '
        '_LabelFramePop_1
        '
        Me._LabelFramePop_1.AutoSize = True
        Me._LabelFramePop_1.BackColor = System.Drawing.SystemColors.Control
        Me._LabelFramePop_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._LabelFramePop_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelFramePop.SetIndex(Me._LabelFramePop_1, CType(1, Short))
        Me._LabelFramePop_1.Location = New System.Drawing.Point(8, 16)
        Me._LabelFramePop_1.Name = "_LabelFramePop_1"
        Me._LabelFramePop_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._LabelFramePop_1.Size = New System.Drawing.Size(101, 13)
        Me._LabelFramePop_1.TabIndex = 6
        Me._LabelFramePop_1.Text = "Anzahl der Runden:"
        '
        '_Label1_1
        '
        Me._Label1_1.AutoSize = True
        Me._Label1_1.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.SetIndex(Me._Label1_1, CType(1, Short))
        Me._Label1_1.Location = New System.Drawing.Point(13, 64)
        Me._Label1_1.Name = "_Label1_1"
        Me._Label1_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_1.Size = New System.Drawing.Size(42, 13)
        Me._Label1_1.TabIndex = 34
        Me._Label1_1.Text = "Modus:"
        '
        '_Label1_15
        '
        Me._Label1_15.AutoSize = True
        Me._Label1_15.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_15.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_15.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.SetIndex(Me._Label1_15, CType(15, Short))
        Me._Label1_15.Location = New System.Drawing.Point(13, 40)
        Me._Label1_15.Name = "_Label1_15"
        Me._Label1_15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_15.Size = New System.Drawing.Size(58, 13)
        Me._Label1_15.TabIndex = 29
        Me._Label1_15.Text = "Startwerte:"
        '
        '_Label1_12
        '
        Me._Label1_12.AutoSize = True
        Me._Label1_12.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_12.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.SetIndex(Me._Label1_12, CType(12, Short))
        Me._Label1_12.Location = New System.Drawing.Point(13, 99)
        Me._Label1_12.Name = "_Label1_12"
        Me._Label1_12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_12.Size = New System.Drawing.Size(90, 13)
        Me._Label1_12.TabIndex = 27
        Me._Label1_12.Text = "Start-Schrittweite:"
        '
        '_Label1_0
        '
        Me._Label1_0.AutoSize = True
        Me._Label1_0.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.SetIndex(Me._Label1_0, CType(0, Short))
        Me._Label1_0.Location = New System.Drawing.Point(13, 16)
        Me._Label1_0.Name = "_Label1_0"
        Me._Label1_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_0.Size = New System.Drawing.Size(54, 13)
        Me._Label1_0.TabIndex = 19
        Me._Label1_0.Text = "Selektion:"
        '
        'EVO_Einstellungen
        '
        Me.Controls.Add(Me.FrameOptions)
        Me.Name = "EVO_Einstellungen"
        Me.Size = New System.Drawing.Size(218, 586)
        Me.FrameOptions.ResumeLayout(False)
        Me.FrameOptions.PerformLayout()
        Me.FrameGen.ResumeLayout(False)
        Me.FrameGen.PerformLayout()
        Me.FramePop.ResumeLayout(False)
        Me.FramePop.PerformLayout()
        CType(Me.Label1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LabelFramePop, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
End Class