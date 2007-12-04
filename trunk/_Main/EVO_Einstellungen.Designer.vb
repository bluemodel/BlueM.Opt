<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EVO_Einstellungen

    Inherits System.Windows.Forms.UserControl

    Public Sub New()
        MyBase.New()
        'Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        InitializeComponent()
        UserControl_Initialize()
        System.Windows.Forms.Application.EnableVisualStyles()
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
    Friend WithEvents LabelAnzNachf As System.Windows.Forms.Label
    Friend WithEvents LabelAnzEltern As System.Windows.Forms.Label
    Friend WithEvents LabelAnzGen As System.Windows.Forms.Label
    Friend WithEvents GroupBox_Generationen As System.Windows.Forms.GroupBox
    Friend WithEvents ComboPopPenalty As System.Windows.Forms.ComboBox
    Friend WithEvents ComboPopStrategie As System.Windows.Forms.ComboBox
    Friend WithEvents ComboOptPopEltern As System.Windows.Forms.ComboBox
    Friend WithEvents TextAnzPopEltern As System.Windows.Forms.TextBox
    Friend WithEvents TextAnzPop As System.Windows.Forms.TextBox
    Friend WithEvents TextAnzRunden As System.Windows.Forms.TextBox
    Friend WithEvents LabelPopPenalty As System.Windows.Forms.Label
    Friend WithEvents LabelPopStrategie As System.Windows.Forms.Label
    Friend WithEvents LabelOptPopEltern As System.Windows.Forms.Label
    Friend WithEvents LabelAnzPopEltern As System.Windows.Forms.Label
    Friend WithEvents LabelAnzPop As System.Windows.Forms.Label
    Friend WithEvents LabelAnzRunden As System.Windows.Forms.Label
    Friend WithEvents GroupBox_Populationen As System.Windows.Forms.GroupBox
    Friend WithEvents LabelStartwerte As System.Windows.Forms.Label
    Friend WithEvents LabelStartSchrittweite As System.Windows.Forms.Label
    Friend WithEvents LabelStrategie As System.Windows.Forms.Label
    Friend WithEvents GroupBox_Einstellungen As System.Windows.Forms.GroupBox
    'Hinweis: Die folgende Prozedur wird vom Windows Form-Designer benötigt.
    'Das Verändern mit dem Windows Form-Designer ist nicht möglich.
    'Das Verändern mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox_Einstellungen = New System.Windows.Forms.GroupBox
        Me.Label_OptModus = New System.Windows.Forms.Label
        Me.Label_OptModusValue = New System.Windows.Forms.Label
        Me.LabelStrategie = New System.Windows.Forms.Label
        Me.ComboStrategie = New System.Windows.Forms.ComboBox
        Me.LabelStartwerte = New System.Windows.Forms.Label
        Me.ComboOptVorgabe = New System.Windows.Forms.ComboBox
        Me.LabelStartSchrittweite = New System.Windows.Forms.Label
        Me.TextDeltaStart = New System.Windows.Forms.TextBox
        Me.CheckisDnVektor = New System.Windows.Forms.CheckBox
        Me.GroupBox_Generationen = New System.Windows.Forms.GroupBox
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
        Me.LabelAnzNachf = New System.Windows.Forms.Label
        Me.LabelAnzEltern = New System.Windows.Forms.Label
        Me.LabelAnzGen = New System.Windows.Forms.Label
        Me.CheckisPopul = New System.Windows.Forms.CheckBox
        Me.GroupBox_Populationen = New System.Windows.Forms.GroupBox
        Me.LabelAnzRunden = New System.Windows.Forms.Label
        Me.TextAnzRunden = New System.Windows.Forms.TextBox
        Me.LabelAnzPop = New System.Windows.Forms.Label
        Me.TextAnzPop = New System.Windows.Forms.TextBox
        Me.LabelAnzPopEltern = New System.Windows.Forms.Label
        Me.TextAnzPopEltern = New System.Windows.Forms.TextBox
        Me.LabelOptPopEltern = New System.Windows.Forms.Label
        Me.ComboOptPopEltern = New System.Windows.Forms.ComboBox
        Me.LabelPopStrategie = New System.Windows.Forms.Label
        Me.ComboPopStrategie = New System.Windows.Forms.ComboBox
        Me.LabelPopPenalty = New System.Windows.Forms.Label
        Me.ComboPopPenalty = New System.Windows.Forms.ComboBox
        Me.GroupBox_Einstellungen.SuspendLayout()
        Me.GroupBox_Generationen.SuspendLayout()
        Me.GroupBox_Populationen.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox_Einstellungen
        '
        Me.GroupBox_Einstellungen.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox_Einstellungen.Controls.Add(Me.Label_OptModus)
        Me.GroupBox_Einstellungen.Controls.Add(Me.Label_OptModusValue)
        Me.GroupBox_Einstellungen.Controls.Add(Me.LabelStrategie)
        Me.GroupBox_Einstellungen.Controls.Add(Me.ComboStrategie)
        Me.GroupBox_Einstellungen.Controls.Add(Me.LabelStartwerte)
        Me.GroupBox_Einstellungen.Controls.Add(Me.ComboOptVorgabe)
        Me.GroupBox_Einstellungen.Controls.Add(Me.LabelStartSchrittweite)
        Me.GroupBox_Einstellungen.Controls.Add(Me.TextDeltaStart)
        Me.GroupBox_Einstellungen.Controls.Add(Me.CheckisDnVektor)
        Me.GroupBox_Einstellungen.Controls.Add(Me.GroupBox_Generationen)
        Me.GroupBox_Einstellungen.Controls.Add(Me.CheckisPopul)
        Me.GroupBox_Einstellungen.Controls.Add(Me.GroupBox_Populationen)
        Me.GroupBox_Einstellungen.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox_Einstellungen.Name = "GroupBox_Einstellungen"
        Me.GroupBox_Einstellungen.Size = New System.Drawing.Size(217, 585)
        Me.GroupBox_Einstellungen.TabIndex = 0
        Me.GroupBox_Einstellungen.TabStop = False
        Me.GroupBox_Einstellungen.Text = "Einstellungen"
        '
        'Label_OptModus
        '
        Me.Label_OptModus.AutoSize = True
        Me.Label_OptModus.Location = New System.Drawing.Point(16, 20)
        Me.Label_OptModus.Name = "Label_OptModus"
        Me.Label_OptModus.Size = New System.Drawing.Size(42, 13)
        Me.Label_OptModus.TabIndex = 31
        Me.Label_OptModus.Text = "Modus:"
        '
        'Label_OptModusValue
        '
        Me.Label_OptModusValue.AutoSize = True
        Me.Label_OptModusValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_OptModusValue.Location = New System.Drawing.Point(84, 19)
        Me.Label_OptModusValue.Name = "Label_OptModusValue"
        Me.Label_OptModusValue.Size = New System.Drawing.Size(0, 13)
        Me.Label_OptModusValue.TabIndex = 32
        '
        'LabelStrategie
        '
        Me.LabelStrategie.AutoSize = True
        Me.LabelStrategie.Location = New System.Drawing.Point(16, 46)
        Me.LabelStrategie.Name = "LabelStrategie"
        Me.LabelStrategie.Size = New System.Drawing.Size(54, 13)
        Me.LabelStrategie.TabIndex = 19
        Me.LabelStrategie.Text = "Selektion:"
        '
        'ComboStrategie
        '
        Me.ComboStrategie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboStrategie.Location = New System.Drawing.Point(84, 42)
        Me.ComboStrategie.Name = "ComboStrategie"
        Me.ComboStrategie.Size = New System.Drawing.Size(123, 21)
        Me.ComboStrategie.TabIndex = 0
        '
        'LabelStartwerte
        '
        Me.LabelStartwerte.AutoSize = True
        Me.LabelStartwerte.Location = New System.Drawing.Point(16, 73)
        Me.LabelStartwerte.Name = "LabelStartwerte"
        Me.LabelStartwerte.Size = New System.Drawing.Size(58, 13)
        Me.LabelStartwerte.TabIndex = 29
        Me.LabelStartwerte.Text = "Startwerte:"
        '
        'ComboOptVorgabe
        '
        Me.ComboOptVorgabe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptVorgabe.Location = New System.Drawing.Point(84, 69)
        Me.ComboOptVorgabe.Name = "ComboOptVorgabe"
        Me.ComboOptVorgabe.Size = New System.Drawing.Size(123, 21)
        Me.ComboOptVorgabe.TabIndex = 1
        '
        'LabelStartSchrittweite
        '
        Me.LabelStartSchrittweite.AutoSize = True
        Me.LabelStartSchrittweite.Location = New System.Drawing.Point(13, 99)
        Me.LabelStartSchrittweite.Name = "LabelStartSchrittweite"
        Me.LabelStartSchrittweite.Size = New System.Drawing.Size(90, 13)
        Me.LabelStartSchrittweite.TabIndex = 27
        Me.LabelStartSchrittweite.Text = "Start-Schrittweite:"
        '
        'TextDeltaStart
        '
        Me.TextDeltaStart.Location = New System.Drawing.Point(164, 96)
        Me.TextDeltaStart.MaxLength = 0
        Me.TextDeltaStart.Name = "TextDeltaStart"
        Me.TextDeltaStart.Size = New System.Drawing.Size(43, 20)
        Me.TextDeltaStart.TabIndex = 2
        Me.TextDeltaStart.Text = "0.1"
        Me.TextDeltaStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CheckisDnVektor
        '
        Me.CheckisDnVektor.Location = New System.Drawing.Point(16, 120)
        Me.CheckisDnVektor.Name = "CheckisDnVektor"
        Me.CheckisDnVektor.Size = New System.Drawing.Size(144, 18)
        Me.CheckisDnVektor.TabIndex = 3
        Me.CheckisDnVektor.Text = "mit Schrittweitenvektor"
        '
        'GroupBox_Generationen
        '
        Me.GroupBox_Generationen.Controls.Add(Me.TextNMemberSecondPop)
        Me.GroupBox_Generationen.Controls.Add(Me.TextInteract)
        Me.GroupBox_Generationen.Controls.Add(Me.TextRekombxy)
        Me.GroupBox_Generationen.Controls.Add(Me.ComboOptEltern)
        Me.GroupBox_Generationen.Controls.Add(Me.TextAnzNachf)
        Me.GroupBox_Generationen.Controls.Add(Me.TextAnzEltern)
        Me.GroupBox_Generationen.Controls.Add(Me.TextAnzGen)
        Me.GroupBox_Generationen.Controls.Add(Me.LabelNMemberSecondPop)
        Me.GroupBox_Generationen.Controls.Add(Me.LabelInteract)
        Me.GroupBox_Generationen.Controls.Add(Me.Line2)
        Me.GroupBox_Generationen.Controls.Add(Me.LabelRekombxy3)
        Me.GroupBox_Generationen.Controls.Add(Me.LabelRekombxy1)
        Me.GroupBox_Generationen.Controls.Add(Me.Line1)
        Me.GroupBox_Generationen.Controls.Add(Me.Label2)
        Me.GroupBox_Generationen.Controls.Add(Me.LabelAnzNachf)
        Me.GroupBox_Generationen.Controls.Add(Me.LabelAnzEltern)
        Me.GroupBox_Generationen.Controls.Add(Me.LabelAnzGen)
        Me.GroupBox_Generationen.Location = New System.Drawing.Point(8, 144)
        Me.GroupBox_Generationen.Name = "GroupBox_Generationen"
        Me.GroupBox_Generationen.Size = New System.Drawing.Size(199, 217)
        Me.GroupBox_Generationen.TabIndex = 4
        Me.GroupBox_Generationen.TabStop = False
        Me.GroupBox_Generationen.Text = "Generationen:"
        '
        'TextNMemberSecondPop
        '
        Me.TextNMemberSecondPop.Location = New System.Drawing.Point(140, 120)
        Me.TextNMemberSecondPop.MaxLength = 0
        Me.TextNMemberSecondPop.Name = "TextNMemberSecondPop"
        Me.TextNMemberSecondPop.Size = New System.Drawing.Size(43, 20)
        Me.TextNMemberSecondPop.TabIndex = 4
        Me.TextNMemberSecondPop.Text = "50"
        Me.TextNMemberSecondPop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextInteract
        '
        Me.TextInteract.Location = New System.Drawing.Point(140, 96)
        Me.TextInteract.MaxLength = 0
        Me.TextInteract.Name = "TextInteract"
        Me.TextInteract.Size = New System.Drawing.Size(43, 20)
        Me.TextInteract.TabIndex = 3
        Me.TextInteract.Text = "10"
        Me.TextInteract.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextRekombxy
        '
        Me.TextRekombxy.Location = New System.Drawing.Point(32, 192)
        Me.TextRekombxy.MaxLength = 0
        Me.TextRekombxy.Name = "TextRekombxy"
        Me.TextRekombxy.Size = New System.Drawing.Size(25, 20)
        Me.TextRekombxy.TabIndex = 6
        Me.TextRekombxy.Text = "2"
        Me.TextRekombxy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ComboOptEltern
        '
        Me.ComboOptEltern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptEltern.Location = New System.Drawing.Point(64, 160)
        Me.ComboOptEltern.Name = "ComboOptEltern"
        Me.ComboOptEltern.Size = New System.Drawing.Size(121, 21)
        Me.ComboOptEltern.TabIndex = 5
        '
        'TextAnzNachf
        '
        Me.TextAnzNachf.Location = New System.Drawing.Point(140, 61)
        Me.TextAnzNachf.MaxLength = 0
        Me.TextAnzNachf.Name = "TextAnzNachf"
        Me.TextAnzNachf.Size = New System.Drawing.Size(43, 20)
        Me.TextAnzNachf.TabIndex = 2
        Me.TextAnzNachf.Text = "10"
        Me.TextAnzNachf.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextAnzEltern
        '
        Me.TextAnzEltern.Location = New System.Drawing.Point(140, 39)
        Me.TextAnzEltern.MaxLength = 0
        Me.TextAnzEltern.Name = "TextAnzEltern"
        Me.TextAnzEltern.Size = New System.Drawing.Size(43, 20)
        Me.TextAnzEltern.TabIndex = 1
        Me.TextAnzEltern.Text = "3"
        Me.TextAnzEltern.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextAnzGen
        '
        Me.TextAnzGen.Location = New System.Drawing.Point(140, 16)
        Me.TextAnzGen.MaxLength = 0
        Me.TextAnzGen.Name = "TextAnzGen"
        Me.TextAnzGen.Size = New System.Drawing.Size(43, 20)
        Me.TextAnzGen.TabIndex = 0
        Me.TextAnzGen.Text = "1"
        Me.TextAnzGen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelNMemberSecondPop
        '
        Me.LabelNMemberSecondPop.Location = New System.Drawing.Point(8, 120)
        Me.LabelNMemberSecondPop.Name = "LabelNMemberSecondPop"
        Me.LabelNMemberSecondPop.Size = New System.Drawing.Size(128, 16)
        Me.LabelNMemberSecondPop.TabIndex = 40
        Me.LabelNMemberSecondPop.Text = "Max. Mitglieder 2nd Pop"
        '
        'LabelInteract
        '
        Me.LabelInteract.AutoSize = True
        Me.LabelInteract.Location = New System.Drawing.Point(8, 96)
        Me.LabelInteract.Name = "LabelInteract"
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
        Me.LabelRekombxy3.Location = New System.Drawing.Point(56, 195)
        Me.LabelRekombxy3.Name = "LabelRekombxy3"
        Me.LabelRekombxy3.Size = New System.Drawing.Size(129, 17)
        Me.LabelRekombxy3.TabIndex = 25
        Me.LabelRekombxy3.Text = "-Rekombination"
        '
        'LabelRekombxy1
        '
        Me.LabelRekombxy1.Location = New System.Drawing.Point(8, 195)
        Me.LabelRekombxy1.Name = "LabelRekombxy1"
        Me.LabelRekombxy1.Size = New System.Drawing.Size(24, 17)
        Me.LabelRekombxy1.TabIndex = 6
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
        Me.Label2.Location = New System.Drawing.Point(8, 155)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 34)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "Ermitteln der Eltern:"
        '
        'LabelAnzNachf
        '
        Me.LabelAnzNachf.AutoSize = True
        Me.LabelAnzNachf.Location = New System.Drawing.Point(8, 64)
        Me.LabelAnzNachf.Name = "LabelAnzNachf"
        Me.LabelAnzNachf.Size = New System.Drawing.Size(97, 13)
        Me.LabelAnzNachf.TabIndex = 16
        Me.LabelAnzNachf.Text = "Anzahl Nachfolger:"
        '
        'LabelAnzEltern
        '
        Me.LabelAnzEltern.AutoSize = True
        Me.LabelAnzEltern.Location = New System.Drawing.Point(7, 42)
        Me.LabelAnzEltern.Name = "LabelAnzEltern"
        Me.LabelAnzEltern.Size = New System.Drawing.Size(72, 13)
        Me.LabelAnzEltern.TabIndex = 15
        Me.LabelAnzEltern.Text = "Anzahl Eltern:"
        '
        'LabelAnzGen
        '
        Me.LabelAnzGen.AutoSize = True
        Me.LabelAnzGen.Location = New System.Drawing.Point(7, 20)
        Me.LabelAnzGen.Name = "LabelAnzGen"
        Me.LabelAnzGen.Size = New System.Drawing.Size(127, 13)
        Me.LabelAnzGen.TabIndex = 14
        Me.LabelAnzGen.Text = "Anzahl der Generationen:"
        '
        'CheckisPopul
        '
        Me.CheckisPopul.Location = New System.Drawing.Point(9, 370)
        Me.CheckisPopul.Name = "CheckisPopul"
        Me.CheckisPopul.Size = New System.Drawing.Size(112, 18)
        Me.CheckisPopul.TabIndex = 5
        Me.CheckisPopul.Text = "mit Populationen"
        '
        'GroupBox_Populationen
        '
        Me.GroupBox_Populationen.Controls.Add(Me.LabelAnzRunden)
        Me.GroupBox_Populationen.Controls.Add(Me.TextAnzRunden)
        Me.GroupBox_Populationen.Controls.Add(Me.LabelAnzPop)
        Me.GroupBox_Populationen.Controls.Add(Me.TextAnzPop)
        Me.GroupBox_Populationen.Controls.Add(Me.LabelAnzPopEltern)
        Me.GroupBox_Populationen.Controls.Add(Me.TextAnzPopEltern)
        Me.GroupBox_Populationen.Controls.Add(Me.LabelOptPopEltern)
        Me.GroupBox_Populationen.Controls.Add(Me.ComboOptPopEltern)
        Me.GroupBox_Populationen.Controls.Add(Me.LabelPopStrategie)
        Me.GroupBox_Populationen.Controls.Add(Me.ComboPopStrategie)
        Me.GroupBox_Populationen.Controls.Add(Me.LabelPopPenalty)
        Me.GroupBox_Populationen.Controls.Add(Me.ComboPopPenalty)
        Me.GroupBox_Populationen.Enabled = False
        Me.GroupBox_Populationen.Location = New System.Drawing.Point(8, 392)
        Me.GroupBox_Populationen.Name = "GroupBox_Populationen"
        Me.GroupBox_Populationen.Size = New System.Drawing.Size(199, 177)
        Me.GroupBox_Populationen.TabIndex = 6
        Me.GroupBox_Populationen.TabStop = False
        Me.GroupBox_Populationen.Text = "Populationen:"
        '
        'LabelAnzRunden
        '
        Me.LabelAnzRunden.AutoSize = True
        Me.LabelAnzRunden.Location = New System.Drawing.Point(8, 16)
        Me.LabelAnzRunden.Name = "LabelAnzRunden"
        Me.LabelAnzRunden.Size = New System.Drawing.Size(101, 13)
        Me.LabelAnzRunden.TabIndex = 6
        Me.LabelAnzRunden.Text = "Anzahl der Runden:"
        '
        'TextAnzRunden
        '
        Me.TextAnzRunden.AcceptsReturn = True
        Me.TextAnzRunden.Location = New System.Drawing.Point(140, 13)
        Me.TextAnzRunden.MaxLength = 0
        Me.TextAnzRunden.Name = "TextAnzRunden"
        Me.TextAnzRunden.Size = New System.Drawing.Size(43, 20)
        Me.TextAnzRunden.TabIndex = 0
        Me.TextAnzRunden.Text = "50"
        Me.TextAnzRunden.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelAnzPop
        '
        Me.LabelAnzPop.AutoSize = True
        Me.LabelAnzPop.Location = New System.Drawing.Point(7, 36)
        Me.LabelAnzPop.Name = "LabelAnzPop"
        Me.LabelAnzPop.Size = New System.Drawing.Size(107, 13)
        Me.LabelAnzPop.TabIndex = 7
        Me.LabelAnzPop.Text = "Anzahl Populationen:"
        '
        'TextAnzPop
        '
        Me.TextAnzPop.AcceptsReturn = True
        Me.TextAnzPop.Location = New System.Drawing.Point(140, 36)
        Me.TextAnzPop.MaxLength = 0
        Me.TextAnzPop.Name = "TextAnzPop"
        Me.TextAnzPop.Size = New System.Drawing.Size(43, 20)
        Me.TextAnzPop.TabIndex = 1
        Me.TextAnzPop.Text = "5"
        Me.TextAnzPop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelAnzPopEltern
        '
        Me.LabelAnzPopEltern.AutoSize = True
        Me.LabelAnzPopEltern.Location = New System.Drawing.Point(7, 58)
        Me.LabelAnzPopEltern.Name = "LabelAnzPopEltern"
        Me.LabelAnzPopEltern.Size = New System.Drawing.Size(112, 13)
        Me.LabelAnzPopEltern.TabIndex = 8
        Me.LabelAnzPopEltern.Text = "Anzahl Eltern [max=5]:"
        '
        'TextAnzPopEltern
        '
        Me.TextAnzPopEltern.AcceptsReturn = True
        Me.TextAnzPopEltern.Location = New System.Drawing.Point(140, 58)
        Me.TextAnzPopEltern.MaxLength = 0
        Me.TextAnzPopEltern.Name = "TextAnzPopEltern"
        Me.TextAnzPopEltern.Size = New System.Drawing.Size(43, 20)
        Me.TextAnzPopEltern.TabIndex = 2
        Me.TextAnzPopEltern.Text = "2"
        Me.TextAnzPopEltern.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelOptPopEltern
        '
        Me.LabelOptPopEltern.Location = New System.Drawing.Point(8, 86)
        Me.LabelOptPopEltern.Name = "LabelOptPopEltern"
        Me.LabelOptPopEltern.Size = New System.Drawing.Size(76, 28)
        Me.LabelOptPopEltern.TabIndex = 9
        Me.LabelOptPopEltern.Text = "Ermittlung der Pop-Eltern:"
        '
        'ComboOptPopEltern
        '
        Me.ComboOptPopEltern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboOptPopEltern.Location = New System.Drawing.Point(85, 90)
        Me.ComboOptPopEltern.Name = "ComboOptPopEltern"
        Me.ComboOptPopEltern.Size = New System.Drawing.Size(108, 21)
        Me.ComboOptPopEltern.TabIndex = 3
        '
        'LabelPopStrategie
        '
        Me.LabelPopStrategie.Location = New System.Drawing.Point(8, 116)
        Me.LabelPopStrategie.Name = "LabelPopStrategie"
        Me.LabelPopStrategie.Size = New System.Drawing.Size(76, 17)
        Me.LabelPopStrategie.TabIndex = 31
        Me.LabelPopStrategie.Text = "Selektion:"
        '
        'ComboPopStrategie
        '
        Me.ComboPopStrategie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboPopStrategie.Location = New System.Drawing.Point(85, 114)
        Me.ComboPopStrategie.Name = "ComboPopStrategie"
        Me.ComboPopStrategie.Size = New System.Drawing.Size(108, 21)
        Me.ComboPopStrategie.TabIndex = 4
        '
        'LabelPopPenalty
        '
        Me.LabelPopPenalty.Location = New System.Drawing.Point(8, 133)
        Me.LabelPopPenalty.Name = "LabelPopPenalty"
        Me.LabelPopPenalty.Size = New System.Drawing.Size(76, 29)
        Me.LabelPopPenalty.TabIndex = 36
        Me.LabelPopPenalty.Text = "Ermittlung der Pop-Güte:"
        '
        'ComboPopPenalty
        '
        Me.ComboPopPenalty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboPopPenalty.Location = New System.Drawing.Point(85, 138)
        Me.ComboPopPenalty.Name = "ComboPopPenalty"
        Me.ComboPopPenalty.Size = New System.Drawing.Size(108, 21)
        Me.ComboPopPenalty.TabIndex = 5
        '
        'EVO_Einstellungen
        '
        Me.Controls.Add(Me.GroupBox_Einstellungen)
        Me.Name = "EVO_Einstellungen"
        Me.Size = New System.Drawing.Size(217, 586)
        Me.GroupBox_Einstellungen.ResumeLayout(False)
        Me.GroupBox_Einstellungen.PerformLayout()
        Me.GroupBox_Generationen.ResumeLayout(False)
        Me.GroupBox_Generationen.PerformLayout()
        Me.GroupBox_Populationen.ResumeLayout(False)
        Me.GroupBox_Populationen.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label_OptModusValue As System.Windows.Forms.Label
    Friend WithEvents Label_OptModus As System.Windows.Forms.Label
End Class