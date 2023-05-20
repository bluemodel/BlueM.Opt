'BlueM.Opt
'Copyright (C) BlueM Dev Group
'Website: <https://www.bluemodel.org>
'
'This program is free software: you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.
'
'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
'GNU General Public License for more details.
'
'You should have received a copy of the GNU General Public License
'along with this program. If not, see <https://www.gnu.org/licenses/>.
'
''' <summary>
''' Diagramm zeigt Lösungen (Individuen) im Lösungsraum an
''' </summary>
Public Class Hauptdiagramm
    Inherits Diagramm

    'lokale Referenz auf Settings
    Private mSettings As BlueM.Opt.Common.Settings

    'Das Problem
    Private mProblem As BlueM.Opt.Common.Problem

    'Zuordnung zwischen Zielfunktionen und Achsen
    Public ZielIndexX, ZielIndexY, ZielIndexZ As Integer

    'Flag, der anzeigt, ob es sich um ein 3D-Diagramm handelt
    Private is3D As Boolean

    'Diagramm Initialisierung (Titel und Achsen)
    '*******************************************
    Public Sub DiagInitialise(ByVal Titel As String, ByVal Achsen As Collection, ByRef prob As BlueM.Opt.Common.Problem)

        Dim xachse, yachse, zachse As Diagramm.Achse

        'Problem speichern
        Me.mProblem = prob

        With Me

            'Chartformatierung
            '-----------------
            .Clear()
            .is3D = False
            .Header.Text = Titel
            '.Header.Visible = False
            .Aspect.View3D = False
            .Legend.Visible = False
            'Rahmenschattierung um das TeeChart Form---
            .Panel.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None
            .Panel.Bevel.Inner = Steema.TeeChart.Drawing.BevelStyles.None
            'Farbverlauf am Rand das Chart---
            .Panel.Gradient.Visible = False
            .Walls.Visible = False
            '*Das Panel---
            .Panel.Color = Drawing.Color.White
            .Chart.Axes.Left.Ticks.Color = Drawing.Color.Black
            .Chart.Axes.Right.Ticks.Color = Drawing.Color.Black
            .Chart.Axes.Left.Ticks.Width = 1
            .Chart.Axes.Right.Ticks.Width = 1

            'Formatierung der Axen
            '---------------------
            'X-Achse:
            xachse = Achsen(1)
            .Chart.Axes.Bottom.Title.Caption = xachse.Title
            .Chart.Axes.Bottom.Automatic = xachse.Automatic
            .Chart.Axes.Bottom.Minimum = xachse.Minimum
            .Chart.Axes.Bottom.Maximum = xachse.Maximum
            .Chart.Axes.Bottom.Increment = xachse.Increment
            .Chart.Axes.Bottom.Grid.Visible = True
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

    ''' <summary>
    ''' Settings setzen
    ''' </summary>
    ''' <param name="settings">Settings</param>
    Public Sub setSettings(ByRef settings As BlueM.Opt.Common.Settings)

        'Settings übergeben
        Me.mSettings = settings

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

        If (Me.mProblem.NumPrimObjective = 1) Then
            'SingleObjective
            'xxxxxxxxxxxxxxx
            If (Not ind.Is_Feasible) Then
                serie = Me.getSeriesPoint($"Population {pop + 1} (invalid)", "Gray", , , ColEach)
            Else
                serie = Me.getSeriesPoint($"Population {pop + 1}", , , , ColEach)
            End If
            Select Case Me.mProblem.Method
                Case BlueM.Opt.Common.METH_PES
                    Call serie.Add(runde * Me.mSettings.PES.N_Gen * Me.mSettings.PES.N_Nachf + gen * Me.mSettings.PES.N_Nachf + nachf, ind.PrimObjectives(0) * Me.mProblem.List_PrimObjectiveFunctions(0).Richtung, ind.ID.ToString(), Farbe)
                Case Else
                    Throw New Exception("Drawing function not defined for this single objective method!")
            End Select
        Else
            'MultiObjective
            'xxxxxxxxxxxxxx
            If (Me.mProblem.NumPrimObjective = 2) Then
                '2D-Diagramm
                '------------------------------------------------------------------------
                If (Not ind.Is_Feasible) Then
                    serie = Me.getSeriesPoint("Population (invalid)", "Gray", , , ColEach)
                Else
                    serie = Me.getSeriesPoint("Population", "Orange", , , ColEach)
                End If
                Call serie.Add(ind.PrimObjectives(0) * Me.mProblem.List_PrimObjectiveFunctions(0).Richtung, ind.PrimObjectives(1) * Me.mProblem.List_PrimObjectiveFunctions(1).Richtung, ind.ID.ToString(), Farbe)

            Else
                '3D-Diagramm (Es werden die ersten drei Zielfunktionswerte eingezeichnet)
                '------------------------------------------------------------------------
                Dim serie3D As Steema.TeeChart.Styles.Points3D
                If (Not ind.Is_Feasible) Then
                    serie3D = Me.getSeries3DPoint("Population" & " (invalid)", "Gray", , , ColEach)
                Else
                    serie3D = Me.getSeries3DPoint("Population", "Orange", , , ColEach)
                End If
                Call serie3D.Add(ind.PrimObjectives(0) * Me.mProblem.List_PrimObjectiveFunctions(0).Richtung, ind.PrimObjectives(1) * Me.mProblem.List_PrimObjectiveFunctions(1).Richtung, ind.PrimObjectives(2) * Me.mProblem.List_PrimObjectiveFunctions(2).Richtung, ind.ID.ToString(), Farbe)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Start-Individuum zeichnen
    ''' </summary>
    ''' <param name="ind">das Individuum, das mit den Startwerten evaluiert wurde</param>
    Public Sub ZeichneStartWert(ByVal ind As Common.Individuum)

        Dim farbe As String
        Dim serie As Steema.TeeChart.Styles.Series

        'Ungültige Individuen immer Grau anzeigen!
        If (Not ind.Is_Feasible) Then
            farbe = "Gray"
        Else
            'ansonsten Gelb
            farbe = "Yellow"
        End If

        If (Me.mProblem.NumPrimObjective = 1) Then
            'SingleObjective
            'xxxxxxxxxxxxxxx
            serie = Me.getSeriesPoint("Start value", farbe, Steema.TeeChart.Styles.PointerStyles.Circle, 4)
            Call serie.Add(1, ind.PrimObjectives(0) * Me.mProblem.List_PrimObjectiveFunctions(0).Richtung, ind.ID.ToString())
        Else
            'MultiObjective
            'xxxxxxxxxxxxxx
            If (Me.mProblem.NumPrimObjective = 2) Then
                '2D-Diagramm
                '------------------------------------------------------------------------
                serie = Me.getSeriesPoint("Start value", farbe, Steema.TeeChart.Styles.PointerStyles.Circle, 4)
                Call serie.Add(ind.PrimObjectives(0) * Me.mProblem.List_PrimObjectiveFunctions(0).Richtung, ind.PrimObjectives(1) * Me.mProblem.List_PrimObjectiveFunctions(1).Richtung, ind.ID.ToString())

            Else
                '3D-Diagramm (Es werden die ersten drei Zielfunktionswerte eingezeichnet)
                '------------------------------------------------------------------------
                Dim serie3D As Steema.TeeChart.Styles.Points3D
                serie3D = Me.getSeries3DPoint("Start value", farbe, Steema.TeeChart.Styles.PointerStyles.Circle, 4)
                Call serie3D.Add(ind.PrimObjectives(0) * Me.mProblem.List_PrimObjectiveFunctions(0).Richtung, ind.PrimObjectives(1) * Me.mProblem.List_PrimObjectiveFunctions(1).Richtung, ind.PrimObjectives(2) * Me.mProblem.List_PrimObjectiveFunctions(2).Richtung, ind.ID.ToString())
            End If
        End If
    End Sub

    'Population zeichnen
    '*******************
    Public Sub ZeichneSekPopulation(ByVal pop() As Common.Individuum)

        Dim i As Integer
        Dim serie, serie_inv As Steema.TeeChart.Styles.Series
        Dim serie3D, serie3D_inv As Steema.TeeChart.Styles.Points3D
        Dim values(,) As Double

        'Population in Array von Penalties transformieren
        values = Common.Individuum.Get_All_Penalty_of_Array(pop)

        If (Me.mProblem.NumPrimObjective = 2) Then
            '2 Zielfunktionen
            '----------------------------------------------------------------
            serie = Me.getSeriesPoint("Secondary population", "Green")
            serie_inv = Me.getSeriesPoint("Secondary population (invalid)", "Gray")
            serie.Clear()
            serie_inv.Clear()
            For i = 0 To values.GetUpperBound(0)
                If pop(i).Is_Feasible Then
                    serie.Add(values(i, 0) * Me.mProblem.List_PrimObjectiveFunctions(0).Richtung, values(i, 1) * Me.mProblem.List_PrimObjectiveFunctions(1).Richtung, pop(i).ID.ToString())
                Else
                    serie_inv.Add(values(i, 0) * Me.mProblem.List_PrimObjectiveFunctions(0).Richtung, values(i, 1) * Me.mProblem.List_PrimObjectiveFunctions(1).Richtung, pop(i).ID.ToString())
                End If

            Next i

        ElseIf (Me.mProblem.NumPrimObjective >= 3) Then
            '3 oder mehr Zielfunktionen (es werden die ersten drei angezeigt)
            '----------------------------------------------------------------
            serie3D = Me.getSeries3DPoint("Secondary population", "Green")
            serie3D_inv = Me.getSeries3DPoint("Secondary population (invalid)", "Gray")
            serie3D.Clear()
            serie3D_inv.Clear()
            For i = 0 To values.GetUpperBound(0)
                If pop(i).Is_Feasible Then
                    serie3D.Add(values(i, 0) * Me.mProblem.List_PrimObjectiveFunctions(0).Richtung, values(i, 1) * Me.mProblem.List_PrimObjectiveFunctions(1).Richtung, values(i, 2) * Me.mProblem.List_PrimObjectiveFunctions(2).Richtung, pop(i).ID.ToString())
                Else
                    serie3D_inv.Add(values(i, 0) * Me.mProblem.List_PrimObjectiveFunctions(0).Richtung, values(i, 1) * Me.mProblem.List_PrimObjectiveFunctions(1).Richtung, values(i, 2) * Me.mProblem.List_PrimObjectiveFunctions(2).Richtung, pop(i).ID.ToString())
                End If
            Next i
        End If

    End Sub

    'Alte Generation im Hauptdiagramm löschen
    '****************************************
    Public Sub LöscheLetzteGeneration(ByVal pop As Integer)

        Dim serie As Steema.TeeChart.Styles.Series

        If (Me.mProblem.NumPrimObjective = 1) Then
            'SingleObjective
            'xxxxxxxxxxxxxxx
            serie = Me.getSeriesPoint($"Population {pop + 1} (invalid)", "Gray")
            serie.Clear()
            serie = Me.getSeriesPoint($"Population {pop + 1}")
            serie.Clear()
        Else
            'MultiObjective
            'xxxxxxxxxxxxxx
            If (Me.mProblem.NumPrimObjective = 2) Then
                '2D-Diagramm
                '------------------------------------------------------------------------
                serie = Me.getSeriesPoint("Population (invalid)", "Gray")
                serie.Clear()
                serie = Me.getSeriesPoint("Population", "Orange")
                serie.Clear()
            Else
                '3D-Diagramm
                '-----------
                Dim serie3D As Steema.TeeChart.Styles.Points3D
                serie3D = Me.getSeries3DPoint("Population (invalid)", "Gray")
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
            If (Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexX).hasIstWert) Then
                colorline1 = New Steema.TeeChart.Tools.ColorLine(Me.Chart)
                colorline1.Pen.Color = System.Drawing.Color.Red
                colorline1.AllowDrag = False
                colorline1.Draw3D = True
                colorline1.Axis = Me.Axes.Bottom
                colorline1.Value = Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexX).IstWert * Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexX).Richtung
            End If
        End If

        'Y-Achse:
        If (Me.ZielIndexY <> -1) Then
            If (Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexY).hasIstWert) Then
                colorline1 = New Steema.TeeChart.Tools.ColorLine(Me.Chart)
                colorline1.Pen.Color = System.Drawing.Color.Red
                colorline1.AllowDrag = False
                colorline1.Draw3D = True
                colorline1.Axis = Me.Axes.Left
                colorline1.Value = Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexY).IstWert * Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexY).Richtung
            End If
        End If

        'Z-Achse:
        If (Me.ZielIndexZ <> -1) Then
            If (Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexZ).hasIstWert) Then
                'TODO: ColorLine auf Depth-Axis geht nicht! (#203)
                MsgBox($"The current value on the Z-axis ({Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexZ).Bezeichnung}) can not be displayed (see #203)", MsgBoxStyle.Information)
                'colorline1 = New Steema.TeeChart.Tools.ColorLine(Me.Chart)
                'colorline1.Pen.Color = System.Drawing.Color.Red
                'colorline1.AllowDrag = False
                'colorline1.Draw3D = True
                'colorline1.Axis = Me.Axes.Depth
                'colorline1.Value = Me.mProblem.List_Featurefunctions(Me.ZielIndexZ).IstWert * Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexZ).Richtung
            End If
        End If

    End Sub

#End Region 'Zeichenfunktionen

#Region "Lösungsauswahl"

    ''' <summary>
    ''' Draw a selected solution in the diagram
    ''' </summary>
    ''' <param name="ind">the solution to draw</param>
    Public Sub DrawSelectedSolution(ind As Common.Individuum)

        Dim x, y, z As Double

        If Not Me.is3D Then
            '2D diagram
            Dim serie As Steema.TeeChart.Styles.Series
            serie = Me.getSeriesPoint("Selected solutions", "Red", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
            serie.Marks.Visible = True
            serie.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Label
            serie.Marks.Transparency = 50
            serie.Marks.ArrowLength = 10
            If Me.mProblem.Method = Common.METH_SENSIPLOT Then
                'x axis is optparameter, y axis is objective function
                x = ind.OptParameter(Me.mSettings.SensiPlot.Selected_OptParameters(0)).RWert
                y = ind.Objectives(Me.mSettings.SensiPlot.Selected_Objective) * Me.mProblem.List_ObjectiveFunctions(Me.mSettings.SensiPlot.Selected_Objective).Richtung
            Else
                If Me.mProblem.Modus = Common.Constants.EVO_MODE.Single_Objective Then
                    'x axis is simulation ID (single objective)
                    x = ind.ID
                    y = ind.Objectives(Me.ZielIndexY) * Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexY).Richtung
                Else
                    'x and y axes are both objective functions
                    x = ind.Objectives(Me.ZielIndexX) * Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexX).Richtung
                    y = ind.Objectives(Me.ZielIndexY) * Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexY).Richtung
                End If
            End If
            serie.Add(x, y, ind.ID.ToString())
        Else
            '3D diagram
            Dim serie3D As Steema.TeeChart.Styles.Points3D
            serie3D = Me.getSeries3DPoint("Selected solutions", "Red", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
            serie3D.Marks.Visible = True
            serie3D.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Label
            serie3D.Marks.Transparency = 50
            serie3D.Marks.ArrowLength = 10
            If Me.mProblem.Method = Common.METH_SENSIPLOT Then
                'x and z axis are optparameters, y axis is objective function
                x = ind.OptParameter(Me.mSettings.SensiPlot.Selected_OptParameters(0)).RWert
                z = ind.OptParameter(Me.mSettings.SensiPlot.Selected_OptParameters(1)).RWert
                y = ind.Objectives(Me.mSettings.SensiPlot.Selected_Objective) * Me.mProblem.List_ObjectiveFunctions(Me.mSettings.SensiPlot.Selected_Objective).Richtung
            Else
                'x, y and z axes are all objective functions
                x = ind.Objectives(Me.ZielIndexX) * Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexX).Richtung
                y = ind.Objectives(Me.ZielIndexY) * Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexY).Richtung
                z = ind.Objectives(Me.ZielIndexZ) * Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexZ).Richtung
            End If
            serie3D.Add(x, y, z, ind.ID.ToString())
        End If

    End Sub

    'Serie der ausgewählten Lösungen löschen
    '***************************************
    Public Sub LöscheAusgewählteLösungen()

        If (Not Me.is3D) Then
            '2D-Diagramm
            '-----------
            Dim serie As Steema.TeeChart.Styles.Series
            serie = Me.getSeriesPoint("Selected solutions")
            serie.Dispose()

        Else
            '3D-Diagramm
            '-----------
            Dim serie3D As Steema.TeeChart.Styles.Points3D
            serie3D = Me.getSeries3DPoint("Selected solutions")
            serie3D.Dispose()
        End If

        Call Me.Refresh()

    End Sub


#End Region

End Class
