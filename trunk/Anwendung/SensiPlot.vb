Imports System.Windows.Forms

Public Class SensiPlot
    Public Selected_OptParameter As String
    Public Selected_OptZiel As String
    Public Selected_SensiType As String
    Public Anz_Sim As Integer

    Public Function ListBox_OptParameter_add(ByVal Bezeichnung As String) As Boolean
        ListBox_OptParameter.Items.Add(Bezeichnung)
    End Function

    Public Function ListBox_OptZiele_add(ByVal Bezeichnung As String) As Boolean
        ListBox_OptZiele.Items.Add(Bezeichnung)
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Selected_OptParameter = ListBox_OptParameter.SelectedItem
        Selected_OptZiel = ListBox_OptZiele.SelectedItem

        If RadioButton_Gleichverteilt.Checked And Not RadioButton_Diskret.Checked Then
            Selected_SensiType = "Gleichverteilt"
        ElseIf Not RadioButton_Gleichverteilt.Checked And RadioButton_Diskret.Checked Then
            Selected_SensiType = "Diskret"
        Else
            MsgBox("Gleichverteilt und Diskret ... das willst du nicht", MsgBoxStyle.Exclamation, "Fehler")
        End If
        Anz_Sim = TextBox_AnzSim.Text
        ListBox_OptParameter.Items.Clear()
        ListBox_OptZiele.Items.Clear()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        ListBox_OptParameter.Items.Clear()
        ListBox_OptZiele.Items.Clear()
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    '************************ TeeChart Funktionen ***************************************

    Public Sub TeeChart_Ini_SensiPlot(ByRef TChart1 As Steema.TeeChart.TChart, ByVal NPopul As Short, ByVal BottomTitle As String, ByVal LeftTitle As String)
        Dim Populationen As Short

        Populationen = NPopul

        With TChart1
            .Clear()
            .Header.Text = "BlauesModell"
            .Aspect.View3D = False
            .Legend.Visible = False

            'Formatierung der Axen
            .Chart.Axes.Bottom.Title.Caption = BottomTitle 'HACK: Beschriftung der Axen
            .Chart.Axes.Bottom.Automatic = True
            .Chart.Axes.Left.Title.Caption = LeftTitle 'HACK: Beschriftung der Axen
            .Chart.Axes.Left.Automatic = True

            'Series(0): Series für die Population.
            Dim Point1 As New Steema.TeeChart.Styles.Points(.Chart)
            Point1.Title = "Population"
            Point1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point1.Color = System.Drawing.Color.Orange
            Point1.Pointer.HorizSize = 2
            Point1.Pointer.VertSize = 2

            'Series(1): Series für die Sekundäre Population
            Dim Point2 As New Steema.TeeChart.Styles.Points(.Chart)
            Point2.Title = "Sekundäre Population"
            Point2.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point2.Color = System.Drawing.Color.Blue
            Point2.Pointer.HorizSize = 3
            Point2.Pointer.VertSize = 3

            'Series(2): Series für Bestwert
            Dim Point3 As New Steema.TeeChart.Styles.Points(.Chart)
            Point3.Title = "Bestwerte"
            Point3.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
            Point3.Color = System.Drawing.Color.Green
            Point3.Pointer.HorizSize = 3
            Point3.Pointer.VertSize = 3

        End With
    End Sub

End Class
