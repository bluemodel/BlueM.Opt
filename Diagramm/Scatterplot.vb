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

Imports System.Windows.Forms
Imports System.Drawing

''' <summary>
''' Zeigt den L�sungs- oder Entscheidungsraum in Form einer Scatterplot-Matrix an
''' </summary>
Partial Public Class Scatterplot
    Inherits System.Windows.Forms.Form

#Region "Eigenschaften"

    'Das Problem
    Private mProblem As BlueM.Opt.Common.Problem

    Private Diags(,) As BlueM.Opt.Diagramm.Diagramm
    Private dimension As Integer
    Private OptResult, OptResultRef As BlueM.Opt.OptResult.OptResult
    Private Auswahl() As Integer
    Private ShowSekPopOnly, ShowStartValue, ShowIstWerte, ShowRefResult As Boolean
    Private ShownSpace As BlueM.Opt.Common.SPACE

#End Region 'Eigenschaften

#Region "Events"

    ''' <summary>
    ''' Event wird ausgel�st, wenn in der Scatterplot-Matrix eine L�sung ausgew�hlt wird
    ''' </summary>
    ''' <param name="ind">Das ausgew�hlte Individuum</param>
    ''' <remarks>wird von Form1.selectSolution() verarbeitet</remarks>
    Public Event pointSelected(ByVal ind As Common.Individuum)

#End Region 'Events

#Region "Methoden"

    ''' <summary>
    ''' Konstruktor
    ''' </summary>
    ''' <param name="prob">Das Optimierungsproblem</param>
    ''' <param name="optres">Das Optimierungsergebnis</param>
    ''' <param name="optresref">Ein Referenz-Optimierungsergebnis (darf Nothing sein)</param>
    Public Sub New(ByRef prob As BlueM.Opt.Common.Problem, ByVal optres As BlueM.Opt.OptResult.OptResult, ByVal optresref As BlueM.Opt.OptResult.OptResult)

        Dim Dialog As ScatterplotDialog
        Dim diagresult As DialogResult

        ' Dieser Aufruf ist f�r den Windows Form-Designer erforderlich.
        InitializeComponent()

        ' F�gen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        'Problem speichern
        Me.mProblem = prob

        'Optimierungsergebnis �bergeben
        Me.OptResult = optres
        Me.OptResultRef = optresref

        'Scatterplot-Dialog aufrufen
        Dim refResultExists As Boolean = Not IsNothing(Me.OptResultRef)
        Dialog = New BlueM.Opt.Diagramm.ScatterplotDialog(Me.mProblem, refResultExists)
        diagresult = Dialog.ShowDialog()

        If (diagresult = DialogResult.OK) Then
            'Einstellungen �bernehmen
            Me.ShownSpace = Dialog.selectedSpace
            Me.Auswahl = Dialog.selectedVariables
            Me.ShowSekPopOnly = Dialog.ShowSekPopOnly
            Me.ShowRefResult = Dialog.ShowRefResult
            Me.ShowStartValue = Dialog.ShowStartValue
            Me.ShowIstWerte = Dialog.ShowIstWerte

            If (Me.ShowRefResult And Me.ShownSpace = Common.SPACE.DecisionSpace)
                If (Not Me.OptResultRef.holdsOptparameters)
                    MsgBox("The comparison result was loaded without optimization parameters and can therefore not be displayed in the decision space!", MsgBoxStyle.Information, "Scatterplot matrix")
                    Me.ShowRefResult = False
                End If
            End If
            

            Application.DoEvents()

            'Anzeigen
            Call Me.Display()
        Else
            'Abbruch
            Me.Dispose()
        End If

    End Sub

    'Scatterplot-Matrix anzeigen
    '***************************
    Private Sub Display()

        'Matrix dimensionieren
        Call Me.dimensionieren(Me.Auswahl.Length)

        'Diagramme zeichnen
        Select Case Me.ShownSpace

            Case Common.SPACE.SolutionSpace
                Me.Text &= " - Solution space"
                Call Me.draw_solutionspace()

            Case Common.SPACE.DecisionSpace
                Me.Text &= " - Decision space"
                Call Me.draw_decisionspace()

        End Select

        'Bereits ausgew�hlte L�sungen anzeigen
        For Each ind As Common.Individuum In Me.OptResult.getSelectedSolutions
            Call Me.showSelectedSolution(ind)
        Next

        Call Me.Show()
        Call Me.BringToFront()

    End Sub

    'L�sungsraum zeichnen
    '********************
    Private Sub draw_solutionspace()

        Dim i, j As Integer
        Dim xAchse, yAchse As String
        Dim min() As Double
        Dim max() As Double
        Dim ind As BlueM.Opt.Common.Individuum
        Dim serie, serie_inv As Steema.TeeChart.Styles.Series
        Dim shape1 As Steema.TeeChart.Styles.Shape

        'Min und Max f�r Achsen bestimmen
        '--------------------------------
        ReDim min(Me.dimension - 1)
        ReDim max(Me.dimension - 1)
        For i = 0 To Me.dimension - 1
            min(i) = Double.MaxValue
            max(i) = Double.MinValue
            If (Me.ShowSekPopOnly) Then
                'Nur Sekund�re Population
                For Each ind In Me.OptResult.getSekPop()
                    min(i) = Math.Min(ind.Objectives(Me.Auswahl(i)), min(i))
                    max(i) = Math.Max(ind.Objectives(Me.Auswahl(i)), max(i))
                Next
            Else
                'Alle L�sungen
                For Each ind In Me.OptResult.Solutions
                    min(i) = Math.Min(ind.Objectives(Me.Auswahl(i)), min(i))
                    max(i) = Math.Max(ind.Objectives(Me.Auswahl(i)), max(i))
                Next
            End If

            'Startwert
            If (Me.ShowStartValue) Then
                ind = Me.OptResult.getSolution(1)
                min(i) = Math.Min(ind.Objectives(Me.Auswahl(i)), min(i))
                max(i) = Math.Max(ind.Objectives(Me.Auswahl(i)), max(i))
                'IstWerte
                If (Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).hasIstWert) Then
                    min(i) = Math.Min(Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).IstWert, min(i))
                    max(i) = Math.Max(Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).IstWert, max(i))
                End If
            End If

            'Vergleichsergebnis
            If (Me.ShowRefResult) Then
                For Each ind In Me.OptResultRef.getSekPop()
                    min(i) = Math.Min(ind.Objectives(Me.Auswahl(i)), min(i))
                    max(i) = Math.Max(ind.Objectives(Me.Auswahl(i)), max(i))
                Next
            End If

        Next

        'Schleife �ber Spalten
        '---------------------
        For i = 0 To Me.dimension - 1
            'Schleife �ber Reihen
            '--------------------
            For j = 0 To Me.dimension - 1

                'Neues Diagramm erstellen
                Me.Diags(i, j) = New BlueM.Opt.Diagramm.Diagramm()
                Me.matrix.Controls.Add(Me.Diags(i, j), i, j)

                With Me.Diags(i, j)

                    'Diagramm formatieren
                    '====================
                    .Header.Visible = False
                    .Aspect.View3D = False
                    .Legend.Visible = False
                    .BackColor = Color.White
                    .Panel.Gradient.Visible = False
                    .Panel.Brush.Color = Color.White
                    .Walls.Back.Transparent = False
                    .Walls.Back.Gradient.Visible = False
                    .Walls.Back.Color = Color.White

                    'Achsen
                    '------
                    'Titel
                    xAchse = Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).Bezeichnung
                    yAchse = Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(j)).Bezeichnung

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

                    'Achsen nur an den R�ndern anzeigen
                    '----------------------------------
                    'YAchsen
                    If (i = 0) Then
                        'Achse standardm��ig anzeigen
                    ElseIf (i = Me.dimension - 1) Then
                        'Achse rechts anzeigen
                        .Axes.Left.OtherSide = True
                    Else
                        'Achse verstecken
                        .Axes.Left.Title.Visible = False
                        .Axes.Left.Labels.CustomSize = 1
                        .Axes.Left.Labels.Font.Color = System.Drawing.Color.Empty
                    End If
                    'XAchsen
                    If (j = 0) Then
                        'Achse oben anzeigen
                        .Axes.Bottom.OtherSide = True
                    ElseIf (j = Me.dimension - 1) Then
                        'Achse standardm��ig anzeigen
                    Else
                        'Achse verstecken
                        .Axes.Bottom.Title.Visible = False
                        .Axes.Bottom.Labels.CustomSize = 1
                        .Axes.Bottom.Labels.Font.Color = System.Drawing.Color.Empty
                    End If

                    'IstWerte eintragen
                    '==================
                    If (Me.ShowIstWerte) Then
                        If (i <> j And _
                            (Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).hasIstWert Or _
                            Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(j)).hasIstWert)) Then

                            shape1 = New Steema.TeeChart.Styles.Shape(.Chart)
                            shape1.Style = Steema.TeeChart.Styles.ShapeStyles.Rectangle
                            shape1.Title = "Area of improvement"

                            'Shape formatieren
                            shape1.Color = Color.Black
                            shape1.Brush.Style = Drawing2D.HatchStyle.DarkUpwardDiagonal
                            shape1.Brush.Color = Color.Black
                            shape1.Brush.ForegroundColor = Color.White
                            shape1.Brush.Transparency = 75
                            shape1.Pen.Transparency = 0
                            shape1.Pen.Color = Color.Red
                            shape1.Pen.Width = 1

                            If (Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).hasIstWert And _
                                 Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(j)).hasIstWert) Then
                                'X- und Y-Werte:
                                '---------------
                                shape1.X0 = min(i) * 0.9 ^ (min(i) / Math.Abs(min(i)))
                                shape1.X1 = Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).IstWert
                                shape1.Y0 = min(j) * 0.9 ^ (min(j) / Math.Abs(min(j)))
                                shape1.Y1 = Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(j)).IstWert

                            ElseIf (Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).hasIstWert) Then
                                'Nur X-Wert:
                                '-----------
                                shape1.X0 = min(i) * 0.9 ^ (min(i) / Math.Abs(min(i)))
                                shape1.X1 = Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(i)).IstWert
                                shape1.Y0 = min(j) * 0.9 ^ (min(j) / Math.Abs(min(j)))
                                shape1.Y1 = max(j) * 1.1 ^ (max(j) / Math.Abs(max(j)))

                            ElseIf (Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(j)).hasIstWert) Then
                                'Nur Y-Wert:
                                '-----------
                                shape1.X0 = min(i) * 0.9 ^ (min(i) / Math.Abs(min(i)))
                                shape1.X1 = max(i) * 1.1 ^ (max(i) / Math.Abs(max(i)))
                                shape1.Y0 = min(j) * 0.9 ^ (min(j) / Math.Abs(min(j)))
                                shape1.Y1 = Me.mProblem.List_ObjectiveFunctions(Me.Auswahl(j)).IstWert
                            End If

                        End If
                    End If

                    'L�sungen eintragen
                    '==================
                    If (Me.ShowSekPopOnly) Then
                        'Nur Sekund�re Population
                        '------------------------
                        serie = .getSeriesPoint(xAchse & ", " & yAchse, "Green", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind In Me.OptResult.getSekPop()
                            serie.Add(ind.Objectives(Me.Auswahl(i)), ind.Objectives(Me.Auswahl(j)), ind.ID.ToString())
                        Next
                    Else
                        'Alle L�sungen
                        '-------------
                        serie = .getSeriesPoint(xAchse & ", " & yAchse, "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        serie_inv = .getSeriesPoint(xAchse & ", " & yAchse & " (invalid)", "Gray", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind In Me.OptResult.Solutions
                            'Constraintverletzung pr�fen
                            If (ind.Is_Feasible) Then
                                'g�ltige L�sung Zeichnen
                                serie.Add(ind.Objectives(Me.Auswahl(i)), ind.Objectives(Me.Auswahl(j)), ind.ID.ToString())
                            Else
                                'ung�ltige L�sung zeichnen
                                serie_inv.Add(ind.Objectives(Me.Auswahl(i)), ind.Objectives(Me.Auswahl(j)), ind.ID.ToString())
                            End If
                        Next
                        'draw sec pop
                        serie = .getSeriesPoint(xAchse & ", " & yAchse & " (sec pop)", "Green", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind In Me.OptResult.getSekPop()
                            serie.Add(ind.Objectives(Me.Auswahl(i)), ind.Objectives(Me.Auswahl(j)), ind.ID.ToString())
                        Next
                    End If

                    'Startwert
                    '=========
                    If (Me.ShowStartValue) Then
                        serie = .getSeriesPoint("Start value", "Yellow", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        ind = Me.OptResult.getSolution(1)
                        serie.Add(ind.Objectives(Me.Auswahl(i)), ind.Objectives(Me.Auswahl(j)), ind.ID.ToString())
                    End If


                    'Vergleichsergebnis anzeigen
                    '===========================
                    If (Me.ShowRefResult) Then
                        serie = .getSeriesPoint(xAchse & ", " & yAchse & " (comparison result)", "Blue", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind In Me.OptResultRef.getSekPop()
                            serie.Add(ind.Objectives(Me.Auswahl(i)), ind.Objectives(Me.Auswahl(j)), ind.ID & " (comparison result)")
                        Next
                    End If

                    'Diagramme auf der Diagonalen ausblenden
                    '=======================================
                    If (i = j) Then
                        'Hintergrund grau anzeigen
                        .Walls.Back.Color = Color.LightGray
                        'MarksTips entfernen
                        .Tools.Clear(True)
                        'Serien unsichtbar machen
                        For Each s As Steema.TeeChart.Styles.Points In .Series
                            s.Cursor = Cursors.Default          'Kein Hand-Cursor
                            s.Pointer.Color = Color.Empty       'Punkte unsichtbar
                            s.Pointer.Pen.Color = Color.Empty   'Punkte unsichtbar
                        Next
                    Else
                        'alle anderen kriegen Handler f�r seriesClick
                        AddHandler .ClickSeries, AddressOf Me.seriesClick
                    End If

                End With
            Next
        Next

    End Sub

    'Entscheidungsraum zeichnen
    '**************************
    Private Sub draw_decisionspace()

        Dim i, j As Integer
        Dim xAchse, yAchse As String
        Dim min() As Double
        Dim max() As Double
        Dim serie, serie_inv As Steema.TeeChart.Styles.Series

        'Min und Max f�r Achsen bestimmen
        '--------------------------------
        ReDim min(Me.dimension - 1)
        ReDim max(Me.dimension - 1)
        For i = 0 To Me.dimension - 1
            min(i) = Me.mProblem.List_OptParameter(Me.Auswahl(i)).Min
            max(i) = Me.mProblem.List_OptParameter(Me.Auswahl(i)).Max
        Next

        'Schleife �ber Spalten
        '---------------------
        For i = 0 To Me.dimension - 1
            'Schleife �ber Reihen
            '--------------------
            For j = 0 To Me.dimension - 1

                'Neues Diagramm erstellen
                Me.Diags(i, j) = New BlueM.Opt.Diagramm.Diagramm()
                Me.matrix.Controls.Add(Me.Diags(i, j), i, j)

                With Me.Diags(i, j)

                    'Diagramm formatieren
                    '====================
                    .Header.Visible = False
                    .Aspect.View3D = False
                    .Legend.Visible = False
                    .BackColor = Color.White
                    .Panel.Gradient.Visible = False
                    .Panel.Brush.Color = Color.White
                    .Walls.Back.Transparent = False
                    .Walls.Back.Gradient.Visible = False
                    .Walls.Back.Color = Color.White

                    'Achsen
                    '------
                    'Titel
                    xAchse = Me.mProblem.List_OptParameter(Me.Auswahl(i)).Bezeichnung
                    yAchse = Me.mProblem.List_OptParameter(Me.Auswahl(j)).Bezeichnung

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

                    ''Beschriftungsformat
                    'If (max(i) >= 1000 Or min(i) <= -1000) Then .Axes.Bottom.Labels.ValueFormat = "0.##E0"
                    'If (max(j) >= 1000 Or min(j) <= -1000) Then .Axes.Left.Labels.ValueFormat = "0.##E0"

                    'Achsen nur an den R�ndern anzeigen
                    '----------------------------------
                    'YAchsen
                    If (i = 0) Then
                        'Achse standardm��ig anzeigen
                    ElseIf (i = Me.dimension - 1) Then
                        'Achse rechts anzeigen
                        .Axes.Left.OtherSide = True
                    Else
                        'Achse verstecken
                        .Axes.Left.Title.Visible = False
                        .Axes.Left.Labels.CustomSize = 1
                        .Axes.Left.Labels.Font.Color = System.Drawing.Color.Empty
                    End If
                    'XAchsen
                    If (j = 0) Then
                        'Achse oben anzeigen
                        .Axes.Bottom.OtherSide = True
                    ElseIf (j = dimension - 1) Then
                        'Achse standardm��ig anzeigen
                    Else
                        'Achse verstecken
                        .Axes.Bottom.Title.Visible = False
                        .Axes.Bottom.Labels.CustomSize = 1
                        .Axes.Bottom.Labels.Font.Color = System.Drawing.Color.Empty
                    End If

                    'Punkte eintragen
                    '================
                    If (Me.ShowSekPopOnly) Then
                        'Nur Sekund�re Population
                        '------------------------
                        serie = .getSeriesPoint(xAchse & ", " & yAchse, "Green", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind As Common.Individuum In Me.OptResult.getSekPop()
                            serie.Add(ind.OptParameter_RWerte(Me.Auswahl(i)), ind.OptParameter_RWerte(Me.Auswahl(j)), ind.ID.ToString())
                        Next
                    Else
                        'Alle L�sungen
                        '-------------
                        serie = .getSeriesPoint(xAchse & ", " & yAchse, "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        serie_inv = .getSeriesPoint(xAchse & ", " & yAchse & " (invalid)", "Gray", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind As Common.Individuum In Me.OptResult.Solutions
                            'Constraintverletzung pr�fen
                            If (ind.Is_Feasible) Then
                                'g�ltige L�sung Zeichnen
                                serie.Add(ind.OptParameter_RWerte(Me.Auswahl(i)), ind.OptParameter_RWerte(Me.Auswahl(j)), ind.ID.ToString())
                            Else
                                'ung�ltige L�sung zeichnen
                                serie_inv.Add(ind.OptParameter_RWerte(Me.Auswahl(i)), ind.OptParameter_RWerte(Me.Auswahl(j)), ind.ID.ToString())
                            End If
                        Next
                        'draw sec pop
                        serie = .getSeriesPoint(xAchse & ", " & yAchse & " (sec pop)", "Green", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind As Common.Individuum In Me.OptResult.getSekPop()
                            serie.Add(ind.OptParameter_RWerte(Me.Auswahl(i)), ind.OptParameter_RWerte(Me.Auswahl(j)), ind.ID.ToString())
                        Next
                    End If

                    'Vergleichsergebnis anzeigen
                    '===========================
                    If (Me.ShowRefResult) Then
                        serie = .getSeriesPoint(xAchse & ", " & yAchse & " (comparison result)", "Blue", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        For Each ind As Common.Individuum In Me.OptResultRef.getSekPop()
                            serie.Add(ind.OptParameter_RWerte(Me.Auswahl(i)), ind.OptParameter_RWerte(Me.Auswahl(j)), ind.ID & " (comparison result)")
                        Next
                    End If

                    'Startwerte der Parameter eintragen
                    '==================================
                    If (Me.ShowStartValue) Then
                        serie = .getSeriesPoint("Start value", "Yellow", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                        serie.Add(Me.mProblem.List_OptParameter(Me.Auswahl(i)).StartWert, Me.mProblem.List_OptParameter(Me.Auswahl(j)).StartWert, "Start value")
                    End If

                    'Diagramme auf der Diagonalen ausblenden
                    '=======================================
                    If (i = j) Then
                        'Hintergrund grau anzeigen
                        .Walls.Back.Color = Color.LightGray
                        'MarksTips entfernen
                        .Tools.Clear(True)
                        'Serien unsichtbar machen
                        For Each s As Steema.TeeChart.Styles.Points In .Series
                            s.Cursor = Cursors.Default          'Kein Hand-Cursor
                            s.Pointer.Color = Color.Empty       'Punkte unsichtbar
                            s.Pointer.Pen.Color = Color.Empty   'Punkte unsichtbar
                        Next
                    Else
                        'alle anderen kriegen Handler f�r seriesClick
                        AddHandler .ClickSeries, AddressOf Me.seriesClick
                    End If

                End With
            Next
        Next

    End Sub

    'Matrix dimensionieren
    '*********************
    Private Sub dimensionieren(ByVal _dimension As Integer)

        Me.dimension = _dimension

        ReDim Me.Diags(Me.dimension - 1, Me.dimension - 1)

        Dim i As Integer

        Me.matrix.Name = "Matrix"

        Me.matrix.ColumnCount = Me.dimension
        For i = 1 To Me.dimension
            Me.matrix.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100 / Me.dimension))
        Next

        Me.matrix.RowCount = Me.dimension
        For i = 1 To Me.dimension
            Me.matrix.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100 / Me.dimension))
        Next

    End Sub

    'Form Resize
    '***********
    Private Sub ScatterplotResize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Me.matrix.Width = Me.ClientSize.Width
        Me.matrix.Height = Me.ClientSize.Height

    End Sub

#Region "L�sungsauswahl"

    'Einen Punkt ausw�hlen
    '*********************
    Private Sub seriesClick(ByVal sender As Object, ByVal s As Steema.TeeChart.Styles.Series, ByVal valueIndex As Integer, ByVal e As System.Windows.Forms.MouseEventArgs)

        Dim indID_clicked As Integer
        Dim ind As Common.Individuum

        'Punkt-Informationen bestimmen
        '-----------------------------
        Try
            'Solution-ID
            indID_clicked = s.Labels(valueIndex)

            'L�sung holen
            '------------
            ind = Me.OptResult.getSolution(indID_clicked)

            If (ind.ID = indID_clicked) Then

                'L�sung ausw�hlen (wird von Form1.selectSolution() verarbeitet)
                RaiseEvent pointSelected(ind)

            End If

        Catch ex As Exception
            MsgBox("Solution is not selectable!", MsgBoxStyle.Information)
        End Try

    End Sub

    ''' <summary>
    ''' Eine ausgew�hlte L�sung in den Diagrammen anzeigen
    ''' </summary>
    ''' <param name="ind">das ausgew�hlte Individuum</param>
    ''' <remarks>wird von Form1.selectSolution() aufgerufen</remarks>
    Public Sub showSelectedSolution(ByVal ind As Common.Individuum)

        Dim serie As Steema.TeeChart.Styles.Series
        Dim i, j As Integer

        'L�sung in alle Diagramme eintragen
        '----------------------------------
        For i = 0 To dimension - 1
            For j = 0 To dimension - 1

                With Me.Diags(i, j)

                    'Roten Punkt zeichnen
                    serie = .getSeriesPoint("Selected solutions", "Red", Steema.TeeChart.Styles.PointerStyles.Circle, 3)

                    Select Case Me.ShownSpace
                        Case Common.SPACE.SolutionSpace
                            serie.Add(ind.Objectives(Me.Auswahl(i)), ind.Objectives(Me.Auswahl(j)), ind.ID.ToString())
                        Case Common.SPACE.DecisionSpace
                            serie.Add(ind.OptParameter_RWerte(Me.Auswahl(i)), ind.OptParameter_RWerte(Me.Auswahl(j)), ind.ID.ToString())
                    End Select

                    'Mark anzeigen
                    serie.Marks.Visible = True
                    serie.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Label
                    serie.Marks.Transparency = 25
                    serie.Marks.ArrowLength = 10
                    serie.Marks.Arrow.Visible = False

                End With
            Next j
        Next i

    End Sub

    ''' <summary>
    ''' Serie der ausgew�hlten L�sungen l�schen
    ''' </summary>
    Public Sub clearSelection()

        Dim i, j As Integer
        Dim serie As Steema.TeeChart.Styles.Series

        For i = 0 To Me.Diags.GetUpperBound(0)
            For j = 0 To Me.Diags.GetUpperBound(1)
                With Me.Diags(i, j)

                    'Serie l�schen
                    serie = .getSeriesPoint("Selected solutions")
                    serie.Dispose()

                End With
            Next j
        Next i

    End Sub

#End Region 'L�sungsauswahl

#End Region 'Methoden

End Class