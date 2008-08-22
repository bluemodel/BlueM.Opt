Public Class Hauptdiagramm
    Inherits Diagramm

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse Hauptdiagramm                                                  ****
    '****                                                                       ****
    '**** Diagramm zeigt Lösungen (Individuen) im Lösungsraum an                ****
    '****                                                                       ****
    '**** Autor: Felix Froehlich                                                ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

    'lokale Kopie der EVO_Einstellungen
    Private mEVO_Settings As Common.EVO_Settings

    'Zuordnung zwischen Zielfunktionen und Achsen
    Public ZielIndexX, ZielIndexY, ZielIndexZ As Integer

    'Flag, der anzeigt, ob es sich um ein 3D-Diagramm handelt
    Private is3D As Boolean

    'Diagramm Initialisierung (Titel und Achsen)
    '*******************************************
    Public Sub DiagInitialise(ByVal Titel As String, ByVal Achsen As Collection, ByRef rEVO_Settings As Common.EVO_Settings)

        Dim xachse, yachse, zachse As Diagramm.Achse

        'Referenz zu EVO_Einstellungen lokal speichern
        Me.mEVO_Settings = rEVO_Settings

        With Me

            .Clear()
            .is3D = False
            .Header.Text = Titel
            .Aspect.View3D = False
            .Legend.Visible = False

            'Formatierung der Axen
            '---------------------
            'X-Achse:
            xachse = Achsen(1)
            .Chart.Axes.Bottom.Title.Caption = xachse.Title
            .Chart.Axes.Bottom.Automatic = xachse.Automatic
            .Chart.Axes.Bottom.Minimum = xachse.Minimum
            .Chart.Axes.Bottom.Maximum = xachse.Maximum
            .Chart.Axes.Bottom.Increment = xachse.Increment
            .Chart.Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
            'Y-Achse:
            yachse = Achsen(2)
            .Chart.Axes.Left.Title.Caption = yachse.Title
            .Chart.Axes.Left.Automatic = yachse.Automatic
            .Chart.Axes.Left.Minimum = yachse.Minimum
            .Chart.Axes.Left.Maximum = yachse.Maximum
            .Chart.Axes.Left.Increment = yachse.Increment
            .Chart.Axes.Left.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value

            'Bei mehr als 2 Achsen 3D-Diagramm
            '---------------------------------
            If (Achsen.Count > 2) Then

                .is3D = True

                'Z-Achse:
                zachse = Achsen(3)
                .Chart.Axes.Depth.Title.Caption = zachse.Title
                .Chart.Axes.Depth.Automatic = zachse.Automatic
                .Chart.Axes.Depth.Minimum = zachse.Minimum
                .Chart.Axes.Depth.Maximum = zachse.Maximum
                .Chart.Axes.Depth.Increment = zachse.Increment
                .Chart.Axes.Depth.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
                .Chart.Axes.Depth.Visible = True

                '3D-Diagramm vorbereiten
                .Chart.Aspect.View3D = True
                .Chart.Aspect.Chart3DPercent = 90
                .Chart.Aspect.Elevation = 348
                .Chart.Aspect.Orthogonal = False
                .Chart.Aspect.Perspective = 62
                .Chart.Aspect.Rotation = 329
                .Chart.Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality
                .Chart.Aspect.VertOffset = -20
                .Chart.Aspect.Zoom = 66

                'Rotate Tool
                Dim rotate As New Steema.TeeChart.Tools.Rotate(.Chart)
                rotate.Button = Windows.Forms.MouseButtons.Right

            End If

        End With

    End Sub

#Region "Zeichenfunktionen"

    'Lösung zeichnen
    '***************
    Public Sub ZeichneIndividuum(ByVal ind As Common.Individuum, ByVal runde As Integer, ByVal pop As Integer, ByVal gen As Integer, ByVal nachf As Integer, _
                               ByVal Farbe As System.Drawing.Color, Optional ByVal ColEach As Boolean = False)

        Dim serie As Steema.TeeChart.Styles.Series

        'Ungültige Individuen immer Grau anzeigen!
        If (Not ind.Is_Feasible) Then
            Farbe = System.Drawing.Color.Gray
        End If

        If (Common.Manager.AnzPenalty = 1) Then
            'SingleObjective
            'xxxxxxxxxxxxxxx
            If (Not ind.Is_Feasible) Then
                serie = Me.getSeriesPoint("Population " & (pop + 1).ToString() & " (ungültig)", "Gray", , , ColEach)
            Else
                serie = Me.getSeriesPoint("Population " & (pop + 1).ToString(), , , , ColEach)
            End If
            Call serie.Add(runde * Me.mEVO_Settings.PES.n_Gen * Me.mEVO_Settings.PES.n_Nachf + gen * Me.mEVO_Settings.PES.n_Nachf + nachf, ind.Penalties(0), ind.ID.ToString())

        Else
            'MultiObjective
            'xxxxxxxxxxxxxx
            If (Common.Manager.AnzPenalty = 2) Then
                '2D-Diagramm
                '------------------------------------------------------------------------
                If (Not ind.Is_Feasible) Then
                    serie = Me.getSeriesPoint("Population" & " (ungültig)", "Gray", , , ColEach)
                Else
                    serie = Me.getSeriesPoint("Population", "Orange", , , ColEach)
                End If
                Call serie.Add(ind.Penalties(0), ind.Penalties(1), ind.ID.ToString(), Farbe)

            Else
                '3D-Diagramm (Es werden die ersten drei Zielfunktionswerte eingezeichnet)
                '------------------------------------------------------------------------
                Dim serie3D As Steema.TeeChart.Styles.Points3D
                If (Not ind.Is_Feasible) Then
                    serie3D = Me.getSeries3DPoint("Population" & " (ungültig)", "Gray", , , ColEach)
                Else
                    serie3D = Me.getSeries3DPoint("Population", "Orange", , , ColEach)
                End If
                Call serie3D.Add(ind.Penalties(0), ind.Penalties(1), ind.Penalties(2), ind.ID.ToString(), Farbe)
            End If
        End If
    End Sub

    'Population zeichnen
    '*******************
    Public Sub ZeichneSekPopulation(ByVal pop() As Common.Individuum)

        Dim i As Integer
        Dim serie As Steema.TeeChart.Styles.Series
        Dim serie3D As Steema.TeeChart.Styles.Points3D
        Dim values(,) As Double

        'Population in Array von Penalties transformieren
        values = Common.Individuum.Get_All_Penalty_of_Array(pop)

        If (Common.Manager.AnzPenalty = 2) Then
            '2 Zielfunktionen
            '----------------------------------------------------------------
            serie = Me.getSeriesPoint("Sekundäre Population", "Green")
            serie.Clear()
            For i = 0 To values.GetUpperBound(0)
                serie.Add(values(i, 0), values(i, 1))
            Next i

        ElseIf (Common.Manager.AnzPenalty >= 3) Then
            '3 oder mehr Zielfunktionen (es werden die ersten drei angezeigt)
            '----------------------------------------------------------------
            serie3D = Me.getSeries3DPoint("Sekundäre Population", "Green")
            serie3D.Clear()
            For i = 0 To values.GetUpperBound(0)
                serie3D.Add(values(i, 0), values(i, 1), values(i, 2))
            Next i
        End If

    End Sub

    'Alte Generation im Hauptdiagramm löschen
    '****************************************
    Public Sub LöscheLetzteGeneration(ByVal pop As Integer)

        Dim serie As Steema.TeeChart.Styles.Series

        If (Common.Manager.AnzPenalty = 1) Then
            'SingleObjective
            'xxxxxxxxxxxxxxx
            serie = Me.getSeriesPoint("Population " & (pop + 1).ToString() & " (ungültig)", "Gray")
            serie.Clear()
            serie = Me.getSeriesPoint("Population " & (pop + 1).ToString())
            serie.Clear()
        Else
            'MultiObjective
            'xxxxxxxxxxxxxx
            If (Common.Manager.AnzPenalty = 2) Then
                '2D-Diagramm
                '------------------------------------------------------------------------
                serie = Me.getSeriesPoint("Population (ungültig)", "Gray")
                serie.Clear()
                serie = Me.getSeriesPoint("Population", "Orange")
                serie.Clear()
            Else
                '3D-Diagramm
                '-----------
                Dim serie3D As Steema.TeeChart.Styles.Points3D
                serie3D = Me.getSeries3DPoint("Population (ungültig)", "Gray")
                serie3D.Clear()
                serie3D = Me.getSeries3DPoint("Population", "Orange")
                serie3D.Clear()
            End If
        End If

    End Sub

    'IstWerte im Hauptdiagramm darstellen
    '************************************
    Public Sub ZeichneIstWerte()

        'Hinweis:
        'Die Zuordnung von Achsen zu Zielfunktionen 
        'muss im Diagramm bereits hinterlegt sein!
        '-------------------------------------------

        Dim colorline1 As Steema.TeeChart.Tools.ColorLine

        'X-Achse:
        If (Me.ZielIndexX <> -1) Then
            If (Common.Manager.List_Ziele(Me.ZielIndexX).hasIstWert) Then
                colorline1 = New Steema.TeeChart.Tools.ColorLine(Me.Chart)
                colorline1.Pen.Color = System.Drawing.Color.Red
                colorline1.AllowDrag = False
                colorline1.Draw3D = True
                colorline1.Axis = Me.Axes.Bottom
                colorline1.Value = EVO.Common.Manager.List_Ziele(Me.ZielIndexX).IstWert
            End If
        End If

        'Y-Achse:
        If (Me.ZielIndexY <> -1) Then
            If (Common.Manager.List_Ziele(Me.ZielIndexY).hasIstWert) Then
                colorline1 = New Steema.TeeChart.Tools.ColorLine(Me.Chart)
                colorline1.Pen.Color = System.Drawing.Color.Red
                colorline1.AllowDrag = False
                colorline1.Draw3D = True
                colorline1.Axis = Me.Axes.Left
                colorline1.Value = Common.Manager.List_Ziele(Me.ZielIndexY).IstWert
            End If
        End If

        'Z-Achse:
        If (Me.ZielIndexZ <> -1) Then
            If (Common.Manager.List_Ziele(Me.ZielIndexZ).hasIstWert) Then
                'BUG 317: ColorLine auf Depth-Axis geht nicht!
                MsgBox("Der IstWert auf der Z-Achse (" & Common.Manager.List_Ziele(Me.ZielIndexZ).Bezeichnung & ") kann leider nicht angezeigt werden (Bug 317)", MsgBoxStyle.Information, "Info")
                'colorline1 = New Steema.TeeChart.Tools.ColorLine(Me.Chart)
                'colorline1.Pen.Color = System.Drawing.Color.Red
                'colorline1.AllowDrag = False
                'colorline1.Draw3D = True
                'colorline1.Axis = Me.Axes.Depth
                'colorline1.Value = Common.Manager.List_Ziele(Me.Hauptdiagramm.ZielIndexZ).IstWert
            End If
        End If

    End Sub

    'Nadirpunkt einzeichnen
    '**********************
    Public Sub ZeichneNadirpunkt(ByVal nadir() As Double)

        If (Common.Manager.AnzPenalty = 2) Then
            '2D
            '--
            Dim serie2 As Steema.TeeChart.Styles.Points
            serie2 = Me.getSeriesPoint("Nadirpunkt", "Blue", Steema.TeeChart.Styles.PointerStyles.Diamond)
            serie2.Clear()
            serie2.Add(nadir(0), nadir(1), "Nadirpunkt")
        Else
            '3D
            '--
            Dim serie3 As Steema.TeeChart.Styles.Points3D
            serie3 = Me.getSeries3DPoint("Nadirpunkt", "Blue", Steema.TeeChart.Styles.PointerStyles.Diamond)
            serie3.Clear()
            serie3.Add(nadir(0), nadir(1), nadir(2), "Nadirpunkt")
        End If

    End Sub

#End Region 'Zeichenfunktionen

#Region "Lösungsauswahl"

    'ausgewählte Lösung anzeigen
    '***************************
    Public Sub ZeichneAusgewählteLösung(ByVal ind As EVO.Common.Individuum)

        'Sonderfall Sensiplot
        If (EVO.Common.Method = EVO.Common.METH_SENSIPLOT) Then
            'BUG 327!
            Exit Sub
        End If

        '2D oder 3D?
        If (Not Me.is3D) Then

            '2D-Diagramm
            '-----------
            Dim serie As Steema.TeeChart.Styles.Series
            serie = Me.getSeriesPoint("ausgewählte Lösungen", "Red", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
            serie.Marks.Visible = True
            serie.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Label
            serie.Marks.Transparency = 50
            serie.Marks.ArrowLength = 10
            If (Me.ZielIndexX = -1) Then
                'X-Achse ist Simulations-ID (Single-Objective)
                serie.Add(ind.ID, ind.Zielwerte(Me.ZielIndexY), ind.ID.ToString())
            Else
                'X- und Y-Achsen sind beides Zielwerte
                serie.Add(ind.Zielwerte(Me.ZielIndexX), ind.Zielwerte(Me.ZielIndexY), ind.ID.ToString())
            End If

        Else
            '3D-Diagramm
            '-----------
            Dim serie3D As Steema.TeeChart.Styles.Points3D
            serie3D = Me.getSeries3DPoint("ausgewählte Lösungen", "Red", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
            serie3D.Add(ind.Zielwerte(Me.ZielIndexX), ind.Zielwerte(Me.ZielIndexY), ind.Zielwerte(Me.ZielIndexZ), ind.ID.ToString())
            serie3D.Marks.Visible = True
            serie3D.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Label
            serie3D.Marks.Transparency = 50
            serie3D.Marks.ArrowLength = 10

        End If

    End Sub

    'Serie der ausgewählten Lösungen löschen
    '***************************************
    Public Sub LöscheAusgewählteLösungen()

        If (Not Me.is3D) Then
            '2D-Diagramm
            '-----------
            Dim serie As Steema.TeeChart.Styles.Series
            serie = Me.getSeriesPoint("ausgewählte Lösungen")
            serie.Dispose()

        Else
            '3D-Diagramm
            '-----------
            Dim serie3D As Steema.TeeChart.Styles.Points3D
            serie3D = Me.getSeries3DPoint("ausgewählte Lösungen")
            serie3D.Dispose()
        End If

        Call Me.Refresh()

    End Sub


#End Region

End Class
