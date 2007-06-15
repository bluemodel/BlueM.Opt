Public Class Wave

    Public Structure Wave
        Public Bezeichnung As String                'Bezeichnung
        Public Einheit As String                    'Einheit
        Public Typ As String                        'Q, N, W ....
        Public Wave(,) As Object                    'Zeitreihe
    End Structure

    Public WaveList() As Wave = {}                  'Liste der Waves

    'Initialisierung
    '***************
    Private Sub Wave_Load(sender as Object, e as System.EventArgs) Handles MyBase.Load
 
       'Größen und Positionen des Diagrammforms anpassen
        Me.WForm.Diag.Width = 800
        Me.WForm.Diag.Height = 600
        Me.WForm.GroupBox_TChartButtons.Location = New System.Drawing.Point(10, 610)
 
       'Legende anzeigen
        Me.WForm.Diag.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Series
        Me.WForm.Diag.Legend.Visible = True

        'Handler für Klick auf Series entfernen
        RemoveHandler WForm.Diag.ClickSeries, AddressOf Main.Form1.showWave

    End Sub

    'gespeicherte Serien in Diagramm eintragen
    '*****************************************
    Public Sub Wave_draw()

        Dim i, j As Integer

        For i = 0 To WaveList.GetUpperBound(0)
            'Serie initialisieren, wenn noch nicht geschehen
            Me.WForm.Diag.prepareSeries(i, WaveList(i).Bezeichnung)
            'Punkte zur Serie hinzufügen
            For j = 0 To WaveList(i).Wave.GetUpperBound(0)
                Me.WForm.Diag.Series(i).Add(WaveList(i).Wave(j, 0), WaveList(i).Wave(j, 1))
            Next j
        Next i

    End Sub

End Class