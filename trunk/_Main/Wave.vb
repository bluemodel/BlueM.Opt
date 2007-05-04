Public Class Wave

    Public Structure Wave
        Public Bezeichnung As String                'Bezeichnung
        Public Einheit As String                    'Einheit
        Public Typ As String                        'Q, N, W ....
        Public Wave(,) As Object
    End Structure

    Public WaveList() As Wave = {}                  'Liste der Waves

    'gespeicherte Serien in Diagramm eintragen
    '*****************************************
    Public Sub Wave_draw()

        Dim i, j As Integer

        For i = 0 To WaveList.GetUpperBound(0)
            For j = 0 To WaveList(i).Wave.GetUpperBound(0)
                Me.Diag.Series(i).Add(WaveList(i).Wave(j, 0), WaveList(i).Wave(j, 1))
            Next j
        Next i

    End Sub

    'BUG 85: TeeChart-Bearbeitung kopiert aus Form1 - sollte vereinheitlicht (ausgelagert) werden
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    'Chart bearbeiten
    '****************
    Private Sub TChartEdit(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChartEdit.Click
        Diag.ShowEditor()
    End Sub

    'Chart nach Excel exportieren
    '****************************
    Private Sub TChart2Excel(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChart2Excel.Click
        SaveFileDialog1.DefaultExt = Diag.Export.Data.Excel.FileExtension
        SaveFileDialog1.FileName = Diag.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "Excel-Dateien (*.xls)|*.xls"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            Diag.Export.Data.Excel.Series = Nothing 'export all series
            Diag.Export.Data.Excel.IncludeLabels = True
            Diag.Export.Data.Excel.IncludeIndex = True
            Diag.Export.Data.Excel.IncludeHeader = True
            Diag.Export.Data.Excel.IncludeSeriesTitle = True
            Diag.Export.Data.Excel.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

    'Chart als PNG exportieren
    '*************************
    Private Sub TChart2PNG(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChart2PNG.Click
        SaveFileDialog1.DefaultExt = Diag.Export.Image.PNG.FileExtension
        SaveFileDialog1.FileName = Diag.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "PNG-Dateien (*.png)|*.png"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            Diag.Export.Image.PNG.GrayScale = False
            Diag.Export.Image.PNG.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

    'Chart in nativem TeeChart-Format abspeichern
    '********************************************
    Private Sub TChartSave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChartSave.Click
        SaveFileDialog1.DefaultExt = Diag.Export.Template.FileExtension
        SaveFileDialog1.FileName = Diag.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "TeeChart-Dateien (*.ten)|*.ten"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            Diag.Export.Template.IncludeData = True
            Diag.Export.Template.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

End Class