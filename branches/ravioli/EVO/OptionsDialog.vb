Imports System.Windows.Forms

''' <summary>
''' Zeigt die generellen Settings an
''' </summary>
Public Class OptionsDialog

    Private mySettings As EVO.Common.EVO_Settings

    Private _MultithreadingAllowed As Boolean

    ''' <summary>
    ''' Multithreading erlauben/verbieten und gleichzeitig ein-/ausschalte
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
                Me.mySettings.General.useMultithreading = True
            Else
                Me.CheckBox_useMultithreading.Checked = False
                Me.CheckBox_useMultithreading.Enabled = False
                Me.mySettings.General.useMultithreading = False
            End If
        End Set
    End Property

    ''' <summary>
    ''' Konstruktor
    ''' </summary>
    ''' <param name="settings">Eine Referenz auf die EVO_Settings</param>
    Public Sub New(ByRef settings As EVO.Common.EVO_Settings)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        'Settings übergeben
        Me.mySettings = settings

    End Sub

    ''' <summary>
    ''' Settings setzen
    ''' </summary>
    ''' <param name="settings">EVO_Settings</param>
    Public Sub setSettings(ByRef settings As EVO.Common.EVO_Settings)

        'Settings übergeben
        Me.mySettings = settings

    End Sub

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

    ''' <summary>
    ''' OK geklickt, Einstellungen aus UI übernehmen
    ''' </summary>
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Me.mySettings.General.useMultithreading = Me.CheckBox_useMultithreading.Checked
        Me.mySettings.General.drawOnlyCurrentGeneration = Me.CheckBox_drawOnlyCurrentGen.Checked

    End Sub

    ''' <summary>
    ''' Bei Dialoganzeige aktuelle Einstellungen in UI übernehmen
    ''' </summary>
    Private Sub Visible_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged

        If (CType(sender, OptionsDialog).Visible) Then
            Me.CheckBox_useMultithreading.Checked = Me.mySettings.General.useMultithreading
            Me.CheckBox_drawOnlyCurrentGen.Checked = Me.mySettings.General.drawOnlyCurrentGeneration
        End If

    End Sub

    ''' <summary>
    ''' Form schließen
    ''' </summary>
    ''' <remarks>verhindert, dass das Form komplett gelöscht wird</remarks>
    Private Sub OptionsDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        'verhindern, dass das Form komplett gelöscht wird
        e.Cancel = True

        'Dialog verstecken
        Call Me.Hide()

    End Sub

End Class
