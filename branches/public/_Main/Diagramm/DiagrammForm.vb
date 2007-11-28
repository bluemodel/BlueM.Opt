Partial Public Class DiagrammForm
    Inherits System.Windows.Forms.UserControl

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse DiagrammForm                                                   ****
    '****                                                                       ****
    '**** Steuerelement, das ein TeeChart Objekt (der Klasse EVO.Diagramm)      ****
    '**** sowie Schaltflächen zum Bedienen des Charts enthält                   ****
    '****                                                                       ****
    '**** Autor: Felix Froehlich                                                ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

    'Form laden
    '**********
    Private Sub DForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        'Handler für Klick auf Serien zuweisen
        AddHandler Me.Diag.ClickSeries, AddressOf EVO.Form1.selectPoint
    End Sub

    'Chart bearbeiten
    '****************
    Private Sub TChartEdit(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChartEdit.Click
        Me.Diag.ShowEditor()
    End Sub

    'Chart nach Excel exportieren
    '****************************
    Private Sub TChart2Excel(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChart2Excel.Click
        SaveFileDialog1.DefaultExt = Me.Diag.Export.Data.Excel.FileExtension
        SaveFileDialog1.FileName = Me.Diag.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "Excel-Dateien (*.xls)|*.xls"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            Me.Diag.Export.Data.Excel.Series = Nothing 'export all series
            Me.Diag.Export.Data.Excel.IncludeLabels = True
            Me.Diag.Export.Data.Excel.IncludeIndex = True
            Me.Diag.Export.Data.Excel.IncludeHeader = True
            Me.Diag.Export.Data.Excel.IncludeSeriesTitle = True
            Me.Diag.Export.Data.Excel.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

    'Chart als PNG exportieren
    '*************************
    Private Sub TChart2PNG(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChart2PNG.Click
        SaveFileDialog1.DefaultExt = Me.Diag.Export.Image.PNG.FileExtension
        SaveFileDialog1.FileName = Me.Diag.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "PNG-Dateien (*.png)|*.png"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            Me.Diag.Export.Image.PNG.GrayScale = False
            Me.Diag.Export.Image.PNG.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

    'Chart in nativem TeeChart-Format abspeichern
    '********************************************
    Private Sub TChartSave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChartSave.Click
        SaveFileDialog1.DefaultExt = Me.Diag.Export.Template.FileExtension
        SaveFileDialog1.FileName = Me.Diag.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "TeeChart-Dateien (*.ten)|*.ten"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            Me.Diag.Export.Template.IncludeData = True
            Me.Diag.Export.Template.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub


End Class
