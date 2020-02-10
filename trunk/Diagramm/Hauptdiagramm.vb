' Copyright (c) BlueM Dev Group
' Website: http://bluemodel.org
' 
' All rights reserved.
' 
' Released under the BSD-2-Clause License:
' 
' Redistribution and use in source and binary forms, with or without modification, 
' are permitted provided that the following conditions are met:
' 
' * Redistributions of source code must retain the above copyright notice, this list 
'   of conditions and the following disclaimer.
' * Redistributions in binary form must reproduce the above copyright notice, this list 
'   of conditions and the following disclaimer in the documentation and/or other materials 
'   provided with the distribution.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
' EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
' OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
' SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT 
' OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
' HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
' TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
' EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'--------------------------------------------------------------------------------------------
'
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
                serie = Me.getSeriesPoint("Population " & (pop + 1).ToString() & " (invalid)", "Gray", , , ColEach)
            Else
                serie = Me.getSeriesPoint("Population " & (pop + 1).ToString(), , , , ColEach)
            End If
            Select Case Me.mProblem.Method
                Case BlueM.Opt.Common.METH_PES
                    Call serie.Add(runde * Me.mSettings.PES.N_Gen * Me.mSettings.PES.N_Nachf + gen * Me.mSettings.PES.N_Nachf + nachf, ind.PrimObjectives(0) * Me.mProblem.List_PrimObjectiveFunctions(0).Richtung, ind.ID.ToString(), Farbe)
                Case BlueM.Opt.Common.METH_HYBRID, BlueM.Opt.Common.METH_CES
                    Call serie.Add(runde * Me.mSettings.CES.N_Generations * Me.mSettings.CES.N_Children + gen * Me.mSettings.CES.N_Children + nachf, ind.PrimObjectives(0) * Me.mProblem.List_PrimObjectiveFunctions(0).Richtung, ind.ID.ToString(), Farbe)
                Case BlueM.Opt.Common.METH_METAEVO
                    Call serie.Add(runde * Me.mSettings.MetaEvo.NumberGenerations * Me.mSettings.MetaEvo.PopulationSize + gen * Me.mSettings.MetaEvo.PopulationSize + nachf, ind.PrimObjectives(0) * Me.mProblem.List_PrimObjectiveFunctions(0).Richtung, ind.ID.ToString(), Farbe)
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
                    serie = Me.getSeriesPoint("Population" & " (invalid)", "Gray", , , ColEach)
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
            serie = Me.getSeriesPoint("Start value", farbe)
            Call serie.Add(1, ind.PrimObjectives(0) * Me.mProblem.List_PrimObjectiveFunctions(0).Richtung, ind.ID.ToString())
        Else
            'MultiObjective
            'xxxxxxxxxxxxxx
            If (Me.mProblem.NumPrimObjective = 2) Then
                '2D-Diagramm
                '------------------------------------------------------------------------
                serie = Me.getSeriesPoint("Start value", farbe)
                Call serie.Add(ind.PrimObjectives(0) * Me.mProblem.List_PrimObjectiveFunctions(0).Richtung, ind.PrimObjectives(1) * Me.mProblem.List_PrimObjectiveFunctions(1).Richtung, ind.ID.ToString())

            Else
                '3D-Diagramm (Es werden die ersten drei Zielfunktionswerte eingezeichnet)
                '------------------------------------------------------------------------
                Dim serie3D As Steema.TeeChart.Styles.Points3D
                serie3D = Me.getSeries3DPoint("Start value", farbe)
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
            serie = Me.getSeriesPoint("Population " & (pop + 1).ToString() & " (invalid)", "Gray")
            serie.Clear()
            serie = Me.getSeriesPoint("Population " & (pop + 1).ToString())
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
                'BUG 317: ColorLine auf Depth-Axis geht nicht!
                MsgBox("The current value on the Z-axis (" & Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexZ).Bezeichnung & ") can not be displayed (Bug 317)", MsgBoxStyle.Information)
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

    'ausgewählte Lösung anzeigen
    '***************************
    Public Sub ZeichneAusgewählteLösung(ByVal ind As BlueM.Opt.Common.Individuum)

        'Sonderfall Sensiplot
        If (Me.mProblem.Method = BlueM.Opt.Common.METH_SENSIPLOT) Then
            'BUG 327!
            Exit Sub
        End If

        '2D oder 3D?
        If (Not Me.is3D) Then

            '2D-Diagramm
            '-----------
            Dim serie As Steema.TeeChart.Styles.Series
            serie = Me.getSeriesPoint("Selected solutions", "Red", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
            serie.Marks.Visible = True
            serie.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Label
            serie.Marks.Transparency = 50
            serie.Marks.ArrowLength = 10
            If (Me.ZielIndexX = -1) Then
                'X-Achse ist Simulations-ID (Single-Objective)
                serie.Add(ind.ID, ind.Objectives(Me.ZielIndexY) * Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexY).Richtung, ind.ID.ToString())
            Else
                'X- und Y-Achsen sind beides Zielwerte
                serie.Add(ind.Objectives(Me.ZielIndexX) * Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexX).Richtung, ind.Objectives(Me.ZielIndexY) * Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexY).Richtung, ind.ID.ToString())
            End If

        Else
            '3D-Diagramm
            '-----------
            Dim serie3D As Steema.TeeChart.Styles.Points3D
            serie3D = Me.getSeries3DPoint("Selected solutions", "Red", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
            serie3D.Add(ind.Objectives(Me.ZielIndexX) * Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexX).Richtung, ind.Objectives(Me.ZielIndexY) * Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexY).Richtung, ind.Objectives(Me.ZielIndexZ) * Me.mProblem.List_ObjectiveFunctions(Me.ZielIndexZ).Richtung, ind.ID.ToString())
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
