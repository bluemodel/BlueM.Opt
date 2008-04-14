Imports System.IO

Partial Public Class Scatterplot
    Inherits System.Windows.Forms.Form

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse Scatterplot                                                    ****
    '****                                                                       ****
    '**** Stellt ein Optimierungsergebnis als Scatterplot-Matrix dar            ****
    '****                                                                       ****
    '**** Autor: Felix Froehlich                                                ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

    Private Diags(,) As EVO.Diagramm
    Private OptResult, OptResultRef As EVO.OptResult
    Private Zielauswahl() As Integer
    Private SekPopOnly, ShowRef As Boolean
    Public Event pointSelected(ByVal ind As Common.Individuum)


    Public Datensatz As String
    Public WorkDir As String


    Public Structure Struct_UserInput
        Public Type As String
        Public OptParameter As String
        Public Title As String
        Public Value As Double
        Public Colour As String
        Public Style As String
        Public Width As Integer
    End Structure

    Public ListUserInput() As Struct_UserInput        'Liste der Zielfunktionen
    'Konstruktor
    '***********
    Public Sub New(ByVal optres As EVO.OptResult, ByVal optresref As EVO.OptResult, ByVal _zielauswahl() As Integer, ByVal _sekpoponly As Boolean, ByVal _showRef As Boolean)

        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        'Optimierungsergebnis übergeben
        Me.OptResult = optres
        Me.OptResultRef = optresref

        'Optionen übernehmen
        Me.SekPopOnly = _sekpoponly
        Me.ShowRef = _showref

        'Zielauswahl speichern
        Me.Zielauswahl = _zielauswahl

        'Diagramme zeichnen
        Call Me.zeichnen()

        'Bereits ausgewählte Lösungen anzeigen
        For Each ind As Common.Individuum In Me.OptResult.getSelectedSolutions
            Call Me.showSelectedSolution(ind)
        Next

    End Sub

    'Nutzereingaben einlesen
    '**************************
    Protected Overridable Sub Read_UserInput()

        'Format:
        '*|-------|-----------------------|-----------------------|-------------|-------|----------|----------|
        '*| Type  | OptParameter          | Axis title            | Value       | Colour| Style    | Width    |
        '*|-------|-----------------------|-----------------------|-------------|-------|----------|----------|

        Dim AnzEingaben As Integer = 0
        Dim i As Integer
        Dim Zeile As String
        Dim ZeilenArray() As String

        Dim Datei As String = WorkDir & Me.Datensatz & "." & "SCA"
        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        'Anzahl der Eingaben feststellen
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False And Zeile.Contains("|")) Then
                AnzEingaben += 1
            End If
        Loop Until StrRead.Peek() = -1

        ReDim ListUserInput(AnzEingaben - 1)

        'Zurück zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        'Einlesen der Zeile und übergeben an die Liste
        i = 0
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False And Zeile.Contains("|")) Then
                ZeilenArray = Zeile.Split("|")
                'Kontrolle
                If (ZeilenArray.GetUpperBound(0) <> 8) Then
                    Throw New Exception("Die SCA-Datei hat die falsche Anzahl Spalten!")
                End If
                'Werte zuweisen
                With ListUserInput(i)
                    .Type = ZeilenArray(1).Trim()
                    .OptParameter = ZeilenArray(2).Trim()
                    .Title = ZeilenArray(3).Trim()
                    .Value = ZeilenArray(4).Trim()
                    .Colour = ZeilenArray(5).Trim()
                    If ZeilenArray(1).Trim() = "point" Then
                        .Style = ZeilenArray(6).Trim()
                    End If
                    If ZeilenArray(1).Trim() = "line" Then
                        .Width = ZeilenArray(7).Trim()
                    End If
                End With
                i += 1
            End If
        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()


    End Sub

    'Diagramme zeichnen
    '******************
    Private Sub zeichnen()

        Dim i, j As Integer
        Dim xAchse, yAchse As String
        Dim min() As Double
        Dim max() As Double
        Dim serie, serie_inv, serie_ist As Steema.TeeChart.Styles.Series
        Dim colorline1 As Steema.TeeChart.Tools.ColorLine

        'Matrix dimensionieren
        '---------------------
        Call Me.dimensionieren()
        Call Me.getPath()
        Call Me.Read_UserInput()

        'Min und Max für Achsen bestimmen
        '--------------------------------
        ReDim min(Me.Zielauswahl.GetUpperBound(0))
        ReDim max(Me.Zielauswahl.GetUpperBound(0))
        For i = 0 To Me.Zielauswahl.GetUpperBound(0)
            min(i) = Double.MaxValue
            max(i) = Double.MinValue
            If (Me.SekPopOnly) Then
                'Nur Sekundäre Population
                For Each ind As Common.Individuum In Me.OptResult.getSekPop()
                    min(i) = Math.Min(ind.Zielwerte(Me.Zielauswahl(i)), min(i))
                    max(i) = Math.Max(ind.Zielwerte(Me.Zielauswahl(i)), max(i))
                Next
            Else
                'Alle Lösungen
                For Each ind As Common.Individuum In Me.OptResult.Solutions
                    min(i) = Math.Min(ind.Zielwerte(Me.Zielauswahl(i)), min(i))
                    max(i) = Math.Max(ind.Zielwerte(Me.Zielauswahl(i)), max(i))
                Next
            End If
            'IstWerte
            If (Common.Manager.List_Ziele(Me.Zielauswahl(i)).hasIstWert) Then
                min(i) = Math.Min(Common.Manager.List_Ziele(Me.Zielauswahl(i)).IstWert, min(i))
                max(i) = Math.Max(Common.Manager.List_Ziele(Me.Zielauswahl(i)).IstWert, max(i))
            End If
            'Vergleichsergebnis
            If (Me.ShowRef)
                For Each ind As Common.Individuum In Me.OptResultRef.getSekPop()
                    min(i) = Math.Min(ind.Zielwerte(Me.Zielauswahl(i)), min(i))
                    max(i) = Math.Max(ind.Zielwerte(Me.Zielauswahl(i)), max(i))
                Next
            End If
        Next

        Dim j1 As Integer
        Dim serieMin_i As Double = 99999999999
        Dim serieMax_i As Double = 0
        Dim serieMin_j As Double = 99999999999
        Dim serieMax_j As Double = 0
        Dim ivgl, jvgl As Double

        Dim serieIstp As Steema.TeeChart.Styles.Series
        Dim serieIstv, serieIsth, serieIsth2, serieIstv2 As Steema.TeeChart.Styles.Line
        Dim freeboard As Boolean

        'Schleife über Spalten
        '---------------------
        For i = 0 To Me.matrix.ColumnCount - 2
            serieMin_i = 99999999999
            serieMax_i = 0
            'Schleife über Reihen
            '--------------------
            For j = 1 To Me.matrix.RowCount - 1
                'Console.Out.WriteLine(matrix.ColumnCount.ToString + " " + matrix.RowCount.ToString)
                serieMin_j = 99999999999
                serieMax_j = 0
                'Console.Out.WriteLine(i.ToString + " " + j.ToString)

                'Neues Diagramm erstellen
                Me.Diags(i, j) = New EVO.Diagramm()
                Me.matrix.Controls.Add(Me.Diags(i, j), i, j)
                Me.Diags(i, j).BackColor = Color.White

                With Me.Diags(i, j)

                    AddHandler .DoubleClick, AddressOf Me.ShowEditor

                    'Diagramm formatieren
                    '====================
                    .Header.Visible = False
                    .Aspect.View3D = False
                    .Legend.Visible = False

                    'Achsen
                    '------
                    'Titel
                    xAchse = Common.Manager.List_Ziele(Me.Zielauswahl(i)).Bezeichnung
                    yAchse = Common.Manager.List_Ziele(Me.Zielauswahl(j)).Bezeichnung

                    'langfristige Parameter einzeichnen
                    '----------------------------------
                    For j1 = 0 To Me.ListUserInput.Length - 1
                        If (Common.Manager.List_OptZiele(j).Bezeichnung.StartsWith(Me.ListUserInput(j1).OptParameter)) Then
                            jvgl = Me.ListUserInput(j1).Value
                            yAchse = Me.ListUserInput(j1).Title
                        End If
                        If (Common.Manager.List_OptZiele(i).Bezeichnung.StartsWith(Me.ListUserInput(j1).OptParameter)) Then
                            jvgl = Me.ListUserInput(j1).Value
                            yAchse = Me.ListUserInput(j1).Title
                        End If
                    Next j1




                    'Kurzfristige Parameter einzeichnen
                    '----------------------------------

                    If (Common.Manager.List_OptZiele(j).Bezeichnung.StartsWith("fMalter")) Then
                        jvgl = 334.12
                        yAchse = "Z Malter [mNN]"
                    ElseIf (Common.Manager.List_OptZiele(j).Bezeichnung.StartsWith("fLehnm")) Then
                        jvgl = 525.07
                        yAchse = "Z Lehnmuehle [mNN]"
                    ElseIf (Common.Manager.List_OptZiele(j).Bezeichnung.StartsWith("fKling")) Then
                        jvgl = 393.78
                        yAchse = "Z Klingenberg [mNN]"
                    ElseIf (Common.Manager.List_OptZiele(j).Bezeichnung.StartsWith("GA")) Then
                        jvgl = 10188634
                        yAchse = "V outlets [m³]"
                    ElseIf (Common.Manager.List_OptZiele(j).Bezeichnung.StartsWith("Gesamt")) Then
                        jvgl = 108064362
                        yAchse = "damage [Euro]"
                    End If

                    If (Common.Manager.List_OptZiele(i).Bezeichnung.StartsWith("fMalter")) Then
                        ivgl = 334.12
                        xAchse = "Z Malter [mNN]"
                    ElseIf (Common.Manager.List_OptZiele(i).Bezeichnung.StartsWith("fLehnm")) Then
                        ivgl = 525.07
                        xAchse = "Z Lehnmuehle [mNN]"
                    ElseIf (Common.Manager.List_OptZiele(i).Bezeichnung.StartsWith("fKling")) Then
                        ivgl = 393.78
                        xAchse = "Z Klingenberg [mNN]"
                    ElseIf (Common.Manager.List_OptZiele(i).Bezeichnung.StartsWith("GA")) Then
                        ivgl = 10188634
                        xAchse = "V outlets [m³]"
                    ElseIf (Common.Manager.List_OptZiele(i).Bezeichnung.StartsWith("Gesamt")) Then
                        ivgl = 108064362
                        xAchse = "damage [Euro]"
                    End If

                    If (xAchse.StartsWith("f")) Then
                        .Axes.Bottom.Labels.ValueFormat = "   0.0"
                    Else
                        .Axes.Bottom.Labels.ValueFormat = "0.0+E00"
                    End If

                    If (yAchse.StartsWith("f")) Then
                        .Axes.Left.Labels.ValueFormat = "   0.0"
                    Else
                        .Axes.Left.Labels.ValueFormat = "0.0+E00"
                    End If


                    .Axes.Bottom.Title.Caption = xAchse
                    .Axes.Left.Title.Caption = yAchse

                    'Labels
                    .Axes.Left.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
                    .Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value

                    'Min und Max
                    .Axes.Bottom.Automatic = False
                    .Axes.Bottom.Minimum = min(i)
                    .Axes.Bottom.Maximum = max(i)
                    .Axes.Bottom.MinimumOffset = 2
                    .Axes.Bottom.MaximumOffset = 2

                    .Axes.Left.Automatic = False
                    .Axes.Left.Minimum = min(j)
                    .Axes.Left.Maximum = max(j)
                    .Axes.Left.MinimumOffset = 2
                    .Axes.Left.MaximumOffset = 2

                    'Beschriftungsformat
                    If (max(i) >= 1000 Or min(i) <= -1000) Then .Axes.Bottom.Labels.ValueFormat = "0.##E0"
                    If (max(j) >= 1000 Or min(j) <= -1000) Then .Axes.Left.Labels.ValueFormat = "0.##E0"

                    'Achsen nur an den Rändern anzeigen
                    '----------------------------------
                    'YAchsen
                    If (i = 0) Then
                        'Achse standardmäßig anzeigen
                    	'ElseIf (i = Me.Zielauswahl.Length - 1) Then
                        '	'Achse rechts anzeigen
                        '	.Axes.Left.OtherSide = True
                    Else
                        'Achse verstecken
                        .Axes.Left.Title.Visible = False
                        .Axes.Left.Labels.CustomSize = 1
                        .Axes.Left.Labels.Font.Color = Color.Empty
                    End If
                    'XAchsen
                    .Axes.Bottom.Labels.Angle = 90

                    If (j = matrix.RowCount - 1) Then
                        'Achse oben anzeigen
                        .Axes.Bottom.OtherSide = False
                    Else
                        'Achse verstecken
                        .Axes.Bottom.Title.Visible = False
                        .Axes.Bottom.Labels.CustomSize = 1
                        .Axes.Bottom.Labels.Font.Color = Color.Empty
                    End If

                    'Punkte eintragen
                    '================
                    If (Me.SekPopOnly) Then
                        'Nur Sekundäre Population
                        '------------------------
                        serie = .getSeriesPoint(xAchse & ", " & yAchse, "Green", Steema.TeeChart.Styles.PointerStyles.Circle, 2)

                        For Each ind As Common.Individuum In Me.OptResult.getSekPop()
                            serie.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID)
                            If ind.Zielwerte(Me.Zielauswahl(i)) < serieMin_i Then serieMin_i = ind.Zielwerte(Me.Zielauswahl(i))
                            If ind.Zielwerte(Me.Zielauswahl(i)) > serieMax_i Then serieMax_i = ind.Zielwerte(Me.Zielauswahl(i))
                            If ind.Zielwerte(Me.Zielauswahl(j)) < serieMin_j Then serieMin_j = ind.Zielwerte(Me.Zielauswahl(j))
                            If ind.Zielwerte(Me.Zielauswahl(j)) > serieMax_j Then serieMax_j = ind.Zielwerte(Me.Zielauswahl(j))
                        Next


                        serieIstp = .getSeriesPoint(xAchse & ", " & yAchse & " (status quo)", "Black", Steema.TeeChart.Styles.PointerStyles.Rectangle, 3)

                        serieIsth = .getSeriesLine("line3", "Gray")
                        serieIsth.LinePen.Width = 2
                        serieIstv = .getSeriesLine("line4", "Gray")
                        serieIstv.LinePen.Width = 2

                        serieIsth2 = .getSeriesLine("line5", "Black")
                        serieIsth2.LinePen.Width = 2
                        serieIstv2 = .getSeriesLine("line6", "Black")
                        serieIstv2.LinePen.Width = 2

                        serieIstv.Add(ivgl, serieMin_j, "status quo")
                        serieIstv.Add(ivgl, jvgl, "status quo")
                        serieIsth.Add(serieMin_i, jvgl, "status quo")
                        serieIsth.Add(ivgl, jvgl, "status quo")
                        serieIstp.Add(ivgl, jvgl)

                     

                        If freeboard Then
                            serieIsth2.Add(serieMin_i, jvgl, "status quo")
                            serieIsth2.Add(ivgl, jvgl, "status quo")
                            serieIstv2.Add(ivgl, serieMin_j, "status quo")
                            serieIstv2.Add(ivgl, jvgl, "status quo")
                        End If



                    Else
                        'Alle Lösungen
                        '-------------
                        serie = .getSeriesPoint(xAchse & ", " & yAchse, "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        serie_inv = .getSeriesPoint(xAchse & ", " & yAchse & " (ungültig)", "Gray", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind As Common.Individuum In Me.OptResult.Solutions
                            'Constraintverletzung prüfen
                            If (ind.Is_Feasible) Then
                                'gültige Lösung Zeichnen
                                serie.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID)
                            Else
                                'ungültige Lösung zeichnen
                                serie_inv.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID)
                            End If
                        Next
                    End If

                    'IstWerte eintragen
                    '==================
                    If (Common.Manager.List_Ziele(Me.Zielauswahl(i)).hasIstWert And Common.Manager.List_Ziele(Me.Zielauswahl(j)).hasIstWert) Then
                        'X und Y: LinePoint
                        '------------------
                        serie_ist = New Steema.TeeChart.Styles.LinePoint(.Chart)
                        serie_ist.Color = Color.Empty
                        serie_ist.Title = "IstWert"
                        Call .add_MarksTips(serie_ist, Steema.TeeChart.Styles.MarksStyles.XY)
                        serie_ist.Add(Common.Manager.List_Ziele(Me.Zielauswahl(i)).IstWert, Common.Manager.List_Ziele(Me.Zielauswahl(j)).IstWert)

                    ElseIf (Common.Manager.List_Ziele(Me.Zielauswahl(i)).hasIstWert) Then
                        'Nur X: Colorline
                        '----------------
                        colorline1 = New Steema.TeeChart.Tools.ColorLine(.Chart)
                        colorline1.Pen.Color = Color.Red
                        colorline1.Axis = .Axes.Bottom
                        colorline1.AllowDrag = False
                        colorline1.NoLimitDrag = True
                        colorline1.Value = Common.Manager.List_Ziele(Me.Zielauswahl(i)).IstWert

                    ElseIf (Common.Manager.List_Ziele(Me.Zielauswahl(j)).hasIstWert) Then
                        'Nur Y: Colorline
                        '----------------
                        colorline1 = New Steema.TeeChart.Tools.ColorLine(.Chart)
                        colorline1.Pen.Color = Color.Red
                        colorline1.Axis = .Axes.Left
                        colorline1.AllowDrag = False
                        colorline1.NoLimitDrag = True
                        colorline1.Value = Common.Manager.List_Ziele(Me.Zielauswahl(j)).IstWert

                    End If

                    'Vergleichsergebnis anzeigen
                    '===========================
                    If (Me.ShowRef) Then
                        serie = .getSeriesPoint(xAchse & ", " & yAchse & " (Vergleichsergebnis)", "Blue", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind As Common.Individuum In Me.OptResultRef.getSekPop()
                            serie.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID & " (Vergleichsergebnis)")
                        Next
                    End If


                    If (i >= j) Then
                        'Diagramme ausblenden
                        .Walls.Back.Transparent = False     'Grau anzeigen
                        .Tools.Clear(True)                  'Um MarksTips zu entfernen
                        For Each s As Steema.TeeChart.Styles.Series In .Series
                            s.Cursor = Cursors.Default      'Kein Hand-Cursor
                            s.Color = Color.Empty           'Punkte unsichtbar
                        Next
                    Else
                        'alle anderen kriegen Handler für seriesClick
                        AddHandler .ClickSeries, AddressOf Me.seriesClick
                        .Walls.Back.Transparent = False
                        .Walls.Back.Visible = True
                        .Walls.Back.Color = Color.White
                    End If

                End With
            Next
        Next

    End Sub

    'Matrix dimensionieren
    '*********************
    Private Sub dimensionieren()

        ReDim Me.Diags(Me.Zielauswahl.Length - 1, Me.Zielauswahl.Length - 1)

        Dim i, fstColFac As Integer

        Me.matrix.Name = "Matrix"

        fstColFac = 100 / Me.Zielauswahl.Length * 0.25

        Me.matrix.ColumnCount = Me.Zielauswahl.Length

        For i = 1 To Me.Zielauswahl.Length
            If i = 1 Then 'erste Spalte
                Me.matrix.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100 / Me.Zielauswahl.Length + fstColFac))
                'Console.Out.WriteLine(SizeType.Percent, 100 / Me.nOptZiele + fstColFac)
            Else
                Me.matrix.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, (100 - (100 / Me.Zielauswahl.Length + fstColFac)) / (Me.Zielauswahl.Length - 1)))
                'Console.Out.WriteLine((100 - (100 / Me.nOptZiele + fstColFac)) / (Me.nOptZiele - 1))
            End If
        Next

        Me.matrix.RowCount = Me.Zielauswahl.Length
        For i = 1 To Me.Zielauswahl.Length
            'Me.matrix.RowStyles.Add(New RowStyle(SizeType.Percent, 100 / Me.nOptZiele))
            'Console.Out.WriteLine(100 / Me.nOptZiele)

            If i = Me.Zielauswahl.Length Then
                Me.matrix.RowStyles.Add(New RowStyle(SizeType.Percent, 100 / Me.Zielauswahl.Length + fstColFac))
            Else
                Me.matrix.RowStyles.Add(New RowStyle(SizeType.Percent, (100 - (100 / Me.Zielauswahl.Length + fstColFac)) / (Me.Zielauswahl.Length - 1)))
            End If
        Next

    End Sub

    'Ruft bei Doppelklick auf Diagramm den TeeChart Editor auf
    '*********************************************************
    Private Sub ShowEditor(ByVal sender As Object, ByVal e As System.EventArgs)

        Call sender.ShowEditor()

    End Sub

    'Form Resize
    '***********
    Private Sub ScatterplotResize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Me.matrix.Width = Me.ClientSize.Width
        Me.matrix.Height = Me.ClientSize.Height

    End Sub

    'Pfad zum Datensatz verarbeiten und speichern
    '********************************************
    Public Sub getPath()

        'Datensatz
        '---------
        Dim pfad As String
        pfad = My.Settings.Datensatz

        If (File.Exists(pfad)) Then
            'Datensatzname bestimmen
            Me.Datensatz = Path.GetFileNameWithoutExtension(pfad)
            'Arbeitsverzeichnis bestimmen
            Me.WorkDir = Path.GetDirectoryName(pfad) & "\"
        End If


    End Sub


#Region "Lösungsauswahl"

    'Einen Punkt auswählen
    '*********************
    Private Sub seriesClick(ByVal sender As Object, ByVal s As Steema.TeeChart.Styles.Series, ByVal valueIndex As Integer, ByVal e As System.Windows.Forms.MouseEventArgs)

        Dim indID_clicked As Integer
        Dim ind As Common.Individuum

        'Punkt-Informationen bestimmen
        '-----------------------------
        Try
            'Solution-ID
            indID_clicked = s.Labels(valueIndex)


            'Lösung holen
            '------------
            ind = Me.OptResult.getSolution(indID_clicked)

            If (ind.ID = indID_clicked) Then

                'Lösung auswählen (wird von Form1.selectSolution() verarbeitet)
                RaiseEvent pointSelected(ind)

            End If
        Catch
            MsgBox("Lösung nicht auswählbar!", MsgBoxStyle.Information, "Info")
        End Try

    End Sub

    'Eine ausgewählte Lösung in den Diagrammen anzeigen
    'wird von Form1.selectSolution() aufgerufen
    '**************************************************
    Friend Sub showSelectedSolution(ByVal ind As Common.Individuum)

        Dim serie As Steema.TeeChart.Styles.Series
        Dim i, j As Integer

        'Lösung in alle Diagramme eintragen
        '----------------------------------
        For i = 0 To Me.Diags.GetUpperBound(0)
            For j = 0 To Me.Diags.GetUpperBound(1)
                With Me.Diags(i, j)
                    If (i < j) Then
                        'Roten Punkt zeichnen
                        serie = .getSeriesPoint("ausgewählte Lösungen", "Red", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
                        serie.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID)

                        'Mark anzeigen
                        serie.Marks.Visible = True
                        serie.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Label
                        serie.Marks.Transparency = 25
                        serie.Marks.ArrowLength = 10
                        serie.Marks.Arrow.Visible = False
                    End If
                End With
            Next j
        Next i
    End Sub

    'Serie der ausgewählten Lösungen löschen
    '***************************************
    Public Sub clearSelection()

        Dim i, j As Integer
        Dim serie As Steema.TeeChart.Styles.Series

        For i = 0 To Me.Diags.GetUpperBound(0)
            For j = 0 To Me.Diags.GetUpperBound(1)
                With Me.Diags(i, j)
                    If (i < j) Then
                        'Serie löschen
                        serie = .getSeriesPoint("ausgewählte Lösungen")
                        serie.Dispose()
                    End If

                End With
            Next j
        Next i

    End Sub

#End Region 'Lösungsauswahl

End Class
