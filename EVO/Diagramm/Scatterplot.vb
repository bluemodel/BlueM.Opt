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
    Public ListstatusQuoInput() As Struct_UserInput        'Liste der Zielfunktionen
    Public ListUserInputBR1() As Struct_UserInput
    Public ListUserInputBR2() As Struct_UserInput
    Public ListUserInputBR3() As Struct_UserInput

    Public ListReferenceInput() As Struct_UserInput



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

    'Nutzereingaben einlesen
    '**************************
    Protected Overridable Sub Read_statusQuoInput()

        'Format:
        '*|-------|-----------------------|-----------------------|-------------|-------|----------|----------|
        '*| Type  | OptParameter          | Axis title            | Value       | Colour| Style    | Width    |
        '*|-------|-----------------------|-----------------------|-------------|-------|----------|----------|

        Dim AnzEingaben As Integer = 0
        Dim i As Integer
        Dim Zeile As String
        Dim ZeilenArray() As String

        Dim Datei As String = WorkDir & Me.Datensatz & "." & "SCAq"
        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        'Anzahl der Eingaben feststellen
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False And Zeile.Contains("|")) Then
                AnzEingaben += 1
            End If
        Loop Until StrRead.Peek() = -1

        ReDim ListstatusQuoInput(AnzEingaben - 1)

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
                With ListstatusQuoInput(i)
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

    'Nutzereingaben einlesen
    '**************************
    Protected Overridable Sub Read_UserInputBR1()

        'Format:
        '*|-------|-----------------------|-----------------------|-------------|-------|----------|----------|
        '*| Type  | OptParameter          | Axis title            | Value       | Colour| Style    | Width    |
        '*|-------|-----------------------|-----------------------|-------------|-------|----------|----------|

        Dim AnzEingaben As Integer = 0
        Dim i As Integer
        Dim Zeile As String
        Dim ZeilenArray() As String

        Dim Datei As String = WorkDir & Me.Datensatz & "." & "SCABR1"
        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        'Anzahl der Eingaben feststellen
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False And Zeile.Contains("|")) Then
                AnzEingaben += 1
            End If
        Loop Until StrRead.Peek() = -1

        ReDim ListUserInputBR1(AnzEingaben - 1)

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
                With ListUserInputBR1(i)
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

    'Nutzereingaben einlesen
    '**************************
    Protected Overridable Sub Read_UserInputBR2()

        'Format:
        '*|-------|-----------------------|-----------------------|-------------|-------|----------|----------|
        '*| Type  | OptParameter          | Axis title            | Value       | Colour| Style    | Width    |
        '*|-------|-----------------------|-----------------------|-------------|-------|----------|----------|

        Dim AnzEingaben As Integer = 0
        Dim i As Integer
        Dim Zeile As String
        Dim ZeilenArray() As String

        Dim Datei As String = WorkDir & Me.Datensatz & "." & "SCABR2"
        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        'Anzahl der Eingaben feststellen
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False And Zeile.Contains("|")) Then
                AnzEingaben += 1
            End If
        Loop Until StrRead.Peek() = -1

        ReDim ListUserInputBR2(AnzEingaben - 1)

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
                With ListUserInputBR2(i)
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

    'Nutzereingaben einlesen
    '**************************
    Protected Overridable Sub Read_UserInputBR3()

        'Format:
        '*|-------|-----------------------|-----------------------|-------------|-------|----------|----------|
        '*| Type  | OptParameter          | Axis title            | Value       | Colour| Style    | Width    |
        '*|-------|-----------------------|-----------------------|-------------|-------|----------|----------|

        Dim AnzEingaben As Integer = 0
        Dim i As Integer
        Dim Zeile As String
        Dim ZeilenArray() As String

        Dim Datei As String = WorkDir & Me.Datensatz & "." & "SCABR3"
        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        'Anzahl der Eingaben feststellen
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False And Zeile.Contains("|")) Then
                AnzEingaben += 1
            End If
        Loop Until StrRead.Peek() = -1

        ReDim ListUserInputBR3(AnzEingaben - 1)

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
                With ListUserInputBR3(i)
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

        Dim exiSca, exiScaQuo, exiBR1, exiBR2, exiBR3 As Boolean
        Dim list As Integer

        exiSca = False 'ob eine Scatterplotdatei existiert
        exiScaQuo = False
        exiBR1 = False
        exiBR2 = False
        exiBR3 = False

        Dim i, j, j1, n, m As Integer
        Dim xAchse, yAchse As String
        Dim min() As Double
        Dim max() As Double
        Dim serie, serie_inv, seriep, serieValid, seriepQuo As Steema.TeeChart.Styles.Series
        Dim serie_ist As Steema.TeeChart.Styles.Series
        Dim colorline1 As Steema.TeeChart.Tools.ColorLine

        'Matrix dimensionieren
        '---------------------
        Call Me.dimensionieren()
        Call Me.getPath()

        Dim pfad As String
        pfad = My.Settings.Datensatz

        'Kontrolle ob *SCA Datei existieren
        '-----------------------------------
        If (File.Exists(WorkDir & Me.Datensatz & "." & "SCA")) Then
            exiSca = True
            Call Me.Read_UserInput()
        End If

        If (File.Exists(WorkDir & Me.Datensatz & "." & "SCAq")) Then
            exiScaQuo = True
            Call Me.Read_statusQuoInput()
        End If

        If (File.Exists(WorkDir & Me.Datensatz & "." & "SCABR1")) Then
            exiBR1 = True
            Call Me.Read_UserInputBR1()
        End If

        If (File.Exists(WorkDir & Me.Datensatz & "." & "SCABR2")) Then
            exiBR2 = True
            Call Me.Read_UserInputBR2()
        End If

        If (File.Exists(WorkDir & Me.Datensatz & "." & "SCABR3")) Then
            exiBR3 = True
            Call Me.Read_UserInputBR3()
        End If


        
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


        Dim serieMin_i As Double = 99999999999
        Dim serieMax_i As Double = 0
        Dim serieMin_j As Double = 99999999999
        Dim serieMax_j As Double = 0
        Dim ivgl, jvgl, ivglQuo, jvglQuo As Double
        Dim ivglBR1, jvglBR1, ivglBR2, jvglBR2, ivglBR3, jvglBR3 As Double

        Dim serieIstp As Steema.TeeChart.Styles.Series
        Dim serieIstv, serieIsth, serieIsth2, serieIstv2 As Steema.TeeChart.Styles.Line
        Dim freeboard As Boolean
        Dim seriepBR1, seriepBR2, seriepBR3 As Steema.TeeChart.Styles.Series
        Dim seriehLine1, seriehLine2, seriehLine3, seriehLineQ, seriehLineUser As Steema.TeeChart.Styles.Line
        Dim serievLine1, serievLine2, serievLine3, serievLineQ, serievLineUser As Steema.TeeChart.Styles.Line


        'Schleife über Spalten
        '---------------------
        For i = 0 To Me.matrix.ColumnCount - 2
            serieMin_i = 99999999999
            serieMin_j = 99999999999
            serieMax_i = 0

            'Schleife über Reihen
            '--------------------
            For j = 1 To Me.matrix.RowCount - 1
                'Console.Out.WriteLine(matrix.ColumnCount.ToString + " " + matrix.RowCount.ToString)
                serieMin_j = 99999999999
                serieMin_i = 99999999999
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

                    'Korrektur Anzeige
                    '------
                    'xKorrektur = Common.Manager.List_Ziele(Me.Zielauswahl(i)).ZielFkt
                    'yKorrektur = Common.Manager.List_Ziele(Me.Zielauswahl(j)).ZielFkt


                    ' User Parameter einzeichnen
                    '----------------------------------
                    If (exiSca) Then
                        For j1 = 0 To Me.ListUserInput.Length - 1
                            If (Common.Manager.List_OptZiele(j).Bezeichnung.StartsWith(Me.ListUserInput(j1).OptParameter)) Then
                                jvgl = Me.ListUserInput(j1).Value
                                yAchse = Me.ListUserInput(j1).Title
                            End If
                            If (Common.Manager.List_OptZiele(i).Bezeichnung.StartsWith(Me.ListUserInput(j1).OptParameter)) Then
                                ivgl = Me.ListUserInput(j1).Value
                                xAchse = Me.ListUserInput(j1).Title
                            End If
                        Next j1
                    End If

                    ' status quo Parameter einzeichnen
                    '----------------------------------
                    If (exiScaQuo) Then
                        For j1 = 0 To Me.ListstatusQuoInput.Length - 1
                            If (Common.Manager.List_OptZiele(j).Bezeichnung.StartsWith(Me.ListstatusQuoInput(j1).OptParameter)) Then
                                jvglQuo = Me.ListstatusQuoInput(j1).Value
                                yAchse = Me.ListstatusQuoInput(j1).Title
                            End If
                            If (Common.Manager.List_OptZiele(i).Bezeichnung.StartsWith(Me.ListstatusQuoInput(j1).OptParameter)) Then
                                ivglQuo = Me.ListstatusQuoInput(j1).Value
                                xAchse = Me.ListstatusQuoInput(j1).Title
                            End If
                        Next j1
                    End If

                    ' Betriebsregel 1 Parameter einzeichnen
                    '----------------------------------
                    If (exiBR1) Then
                        For j1 = 0 To Me.ListUserInputBR1.Length - 1
                            If (Common.Manager.List_OptZiele(j).Bezeichnung.StartsWith(Me.ListUserInputBR1(j1).OptParameter)) Then
                                jvglBR1 = Me.ListUserInputBR1(j1).Value
                                yAchse = Me.ListUserInputBR1(j1).Title
                            End If
                            If (Common.Manager.List_OptZiele(i).Bezeichnung.StartsWith(Me.ListUserInputBR1(j1).OptParameter)) Then
                                ivglBR1 = Me.ListUserInputBR1(j1).Value
                                xAchse = Me.ListUserInputBR1(j1).Title
                            End If
                        Next j1
                    End If

                    ' Betriebsregel 2 Parameter einzeichnen
                    '----------------------------------
                    If (exiBR2) Then
                        For j1 = 0 To Me.ListUserInputBR2.Length - 1
                            If (Common.Manager.List_OptZiele(j).Bezeichnung.StartsWith(Me.ListUserInputBR2(j1).OptParameter)) Then
                                jvglBR2 = Me.ListUserInputBR2(j1).Value
                                yAchse = Me.ListUserInputBR2(j1).Title
                            End If
                            If (Common.Manager.List_OptZiele(i).Bezeichnung.StartsWith(Me.ListUserInputBR2(j1).OptParameter)) Then
                                ivglBR2 = Me.ListUserInputBR2(j1).Value
                                xAchse = Me.ListUserInputBR2(j1).Title
                            End If
                        Next j1
                    End If

                    ' Betriebsregel 3 Parameter einzeichnen
                    '----------------------------------
                    If (exiBR3) Then
                        For j1 = 0 To Me.ListUserInputBR3.Length - 1
                            If (Common.Manager.List_OptZiele(j).Bezeichnung.StartsWith(Me.ListUserInputBR3(j1).OptParameter)) Then
                                jvglBR3 = Me.ListUserInputBR3(j1).Value
                                yAchse = Me.ListUserInputBR3(j1).Title
                            End If
                            If (Common.Manager.List_OptZiele(i).Bezeichnung.StartsWith(Me.ListUserInputBR3(j1).OptParameter)) Then
                                ivglBR3 = Me.ListUserInputBR3(j1).Value
                                xAchse = Me.ListUserInputBR3(j1).Title
                            End If
                        Next j1
                    End If


                    If (ivgl < 1000.0) Then
                        .Axes.Bottom.Labels.ValueFormat = "    0.00"
                    Else
                        .Axes.Bottom.Labels.ValueFormat = "0.0+E00"
                    End If

                    If (jvgl < 1000.0) Then
                        .Axes.Left.Labels.ValueFormat = "    0.00"
                    Else
                        .Axes.Left.Labels.ValueFormat = "0.0+E00"
                    End If


                    .Axes.Bottom.Title.Caption = xAchse
                    .Axes.Left.Title.Caption = yAchse

                    'Labels
                    .Axes.Left.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value
                    .Axes.Bottom.Labels.Style = Steema.TeeChart.AxisLabelStyle.Value

                    'Min und Max
                    '.Axes.Bottom.Automatic = False
                    '.Axes.Bottom.Minimum = min(i)
                    '.Axes.Bottom.Maximum = max(i)
                    '.Axes.Bottom.MinimumOffset = 2
                    '.Axes.Bottom.MaximumOffset = 2

                    '.Axes.Left.Automatic = False
                    '.Axes.Left.Minimum = min(j)
                    '.Axes.Left.Maximum = max(j)
                    ' .Axes.Left.MinimumOffset = 2
                    ' .Axes.Left.MaximumOffset = 2

                    'Beschriftungsformat
                    If (max(i) >= 1000 Or min(i) <= -1000) Then .Axes.Bottom.Labels.ValueFormat = "0.##E0"
                    If (max(j) >= 1000 Or min(j) <= -1000) Then .Axes.Left.Labels.ValueFormat = "0.##E0"

                    'Achsen nur an den Rändern anzeigen
                    '----------------------------------
                    'YAchsen
                    If (i > 0) Then
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
                        serieValid = .getSeriesPoint(xAchse & ", " & yAchse & " (user selection)", "Yellow", Steema.TeeChart.Styles.PointerStyles.Circle, 2)

                        'Bestimmung min max für linienen

                        If ivglQuo <> 0.0 Then serieMin_i = ivglQuo
                        If ivglBR1 <> 0.0 And ivglBR1 < serieMin_i Then serieMin_i = ivglBR1
                        If ivglBR2 <> 0.0 And ivglBR2 < serieMin_i Then serieMin_i = ivglBR2
                        If ivglBR3 <> 0.0 And ivglBR3 < serieMin_i Then serieMin_i = ivglBR3
                        If ivgl <> 0.0 And ivgl < serieMin_i Then serieMin_i = ivgl

                        If jvglQuo <> 0.0 Then serieMin_j = jvglQuo
                        If jvglBR1 <> 0.0 And jvglBR1 < serieMin_j Then serieMin_j = jvglBR1
                        If jvglBR2 <> 0.0 And jvglBR2 < serieMin_j Then serieMin_j = jvglBR2
                        If jvglBR3 <> 0.0 And jvglBR3 < serieMin_j Then serieMin_j = jvglBR3
                        If jvgl <> 0.0 And jvgl < serieMin_j Then serieMin_j = jvgl

                       
                        serieMax_i = Math.Max(Math.Max(Math.Max(Math.Max(ivglQuo, ivglBR3), ivglBR2), ivglBR1), ivgl)
                        serieMax_j = Math.Max(Math.Max(Math.Max(Math.Max(jvglQuo, jvglBR3), jvglBR2), jvglBR1), jvgl)

                        For Each ind As Common.Individuum In Me.OptResult.getSekPop()

                            serie.Add(ind.Zielwerte(Math.Abs(Me.Zielauswahl(i))), ind.Zielwerte(Math.Abs(Me.Zielauswahl(j))), ind.ID.ToString())

                            'finde Min und Max zur Darstellung für Linien
                            If (exiSca Or exiScaQuo) Then
                                If ind.Zielwerte(Me.Zielauswahl(i)) < serieMin_i Then serieMin_i = ind.Zielwerte(Me.Zielauswahl(i))
                                If ind.Zielwerte(Me.Zielauswahl(i)) > serieMax_i Then serieMax_i = ind.Zielwerte(Me.Zielauswahl(i))
                                If ind.Zielwerte(Me.Zielauswahl(j)) < serieMin_j Then serieMin_j = ind.Zielwerte(Me.Zielauswahl(j))
                                If ind.Zielwerte(Me.Zielauswahl(j)) > serieMax_j Then serieMax_j = ind.Zielwerte(Me.Zielauswahl(j))

                            End If
                        Next

                        


                        If (exiSca) Then

                            ReDim ListReferenceInput(ListUserInput.Length - 1)
                            'Auswahl der Referenz
                            For list = 0 To ListReferenceInput.Length - 1
                                ' Console.Out.WriteLine(Me.ComboBox1.SelectedItem)
                                'Console.Out.WriteLine(Me.ComboBox1.SelectedIndex)
                                If (Me.ComboBox1.SelectedIndex = 0) Then ListReferenceInput(list) = ListUserInput(list)
                                If (Me.ComboBox1.SelectedIndex = 1) Then ListReferenceInput(list) = ListstatusQuoInput(list)
                                If (Me.ComboBox1.SelectedIndex = 2) Then ListReferenceInput(list) = ListUserInputBR1(list)
                                If (Me.ComboBox1.SelectedIndex = 3) Then ListReferenceInput(list) = ListUserInputBR2(list)
                                If (Me.ComboBox1.SelectedIndex = 4) Then ListReferenceInput(list) = ListUserInputBR3(list)

                            Next
                            'Markiere Lösungen die alle angegebenen Zielwerte aus Scatterplotdatei erfüllen
                            For Each ind As Common.Individuum In Me.OptResult.getSekPop()
                                n = 0
                                'über alle individuen (ZF)
                                For m = 0 To ind.Zielwerte.Length - 1
                                    'über alle Userparameter
                                    For j1 = 0 To Me.ListReferenceInput.Length - 1
                                        'Console.Out.WriteLine(Me.ListReferenceInput(j1).OptParameter)
                                        If (Common.Manager.List_OptZiele(m).Bezeichnung.StartsWith(Me.ListReferenceInput(j1).OptParameter)) Then
                                            ' Console.Out.WriteLine(ind.Zielwerte(m).ToString + " " + Me.ListReferenceInput(j1).Value.ToString)
                                            If (ind.Zielwerte(m) < Me.ListReferenceInput(j1).Value) Then
                                                n += 1
                                            End If
                                        End If
                                    Next
                                Next

                                'Console.Out.WriteLine(n)
                                ' Console.Out.WriteLine(ind.Zielwerte.Length)

                                If n = ind.Zielwerte.Length Then
                                    'Console.Out.WriteLine(ind.Zielwerte(Me.Zielauswahl(i)).ToString + " " + ind.Zielwerte(Me.Zielauswahl(j)).ToString)

                                    serieValid.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID.ToString())
                                End If
                            Next


                            ivgl = Math.Abs(ivgl)
                            jvgl = Math.Abs(jvgl)
                            ivglQuo = Math.Abs(ivglQuo)
                            jvglQuo = Math.Abs(jvglQuo)
                            ivglBR1 = Math.Abs(ivglBR1)
                            jvglBR1 = Math.Abs(jvglBR1)
                            ivglBR2 = Math.Abs(ivglBR2)
                            jvglBR2 = Math.Abs(jvglBR2)
                            ivglBR3 = Math.Abs(ivglBR3)
                            jvglBR3 = Math.Abs(jvglBR3)
                           

                            seriep = .getSeriesPoint(xAchse & ", " & yAchse & " (user input)", "Black", Steema.TeeChart.Styles.PointerStyles.Rectangle, 3)
                            seriep.Add(ivgl, jvgl)

                            seriepQuo = .getSeriesPoint(xAchse & ", " & yAchse & " (status quo)", "Blue", Steema.TeeChart.Styles.PointerStyles.Rectangle, 3)
                            seriepQuo.Add(ivglQuo, jvglQuo)

                            seriepBR1 = .getSeriesPoint(xAchse & ", " & yAchse & " (strategy 1)", "Green", Steema.TeeChart.Styles.PointerStyles.Rectangle, 3)
                            seriepBR1.Add(ivglBR1, jvglBR1)

                            seriepBR2 = .getSeriesPoint(xAchse & ", " & yAchse & " (strategy 2)", "Yellow", Steema.TeeChart.Styles.PointerStyles.Rectangle, 3)
                            seriepBR2.Add(ivglBR2, jvglBR2)

                            seriepBR3 = .getSeriesPoint(xAchse & ", " & yAchse & " (strategy 3)", "Red", Steema.TeeChart.Styles.PointerStyles.Rectangle, 3)
                            seriepBR3.Add(ivglBR3, jvglBR3)

                            seriehLineUser = .getSeriesLine("lineUh", "Black")
                            seriehLineUser.Add(serieMin_i, jvgl, "Nutzereingabe")
                            seriehLineUser.Add(ivgl, jvgl, "Nutzereingabe")
                            serievLineUser = .getSeriesLine("lineUv", "Black")
                            serievLineUser.Add(ivgl, serieMin_j, "Nutzereingabe")
                            serievLineUser.Add(ivgl, jvgl, "Nutzereingabe")

                            seriehLineQ = .getSeriesLine("lineQh", "Blue")
                            seriehLineQ.Add(serieMin_i, jvglQuo, "Ist-zustand")
                            seriehLineQ.Add(ivglQuo, jvglQuo, "Ist-zustand")
                            serievLineQ = .getSeriesLine("lineQv", "Blue")
                            serievLineQ.Add(ivglQuo, serieMin_j, "Ist-zustand")
                            serievLineQ.Add(ivglQuo, jvglQuo, "Ist-zustand")

                            seriehLine1 = .getSeriesLine("line1h", "Green")
                            seriehLine1.Add(serieMin_i, jvglBR1, "Betriebsregel1")
                            seriehLine1.Add(ivglBR1, jvglBR1, "Betriebsregel1")
                            serievLine1 = .getSeriesLine("line1v", "Green")
                            serievLine1.Add(ivglBR1, serieMin_j, "Betriebsregel1")
                            serievLine1.Add(ivglBR1, jvglBR1, "Betriebsregel1")

                            seriehLine2 = .getSeriesLine("line2h", "Yellow")
                            seriehLine2.Add(serieMin_i, jvglBR2, "Betriebsregel2")
                            seriehLine2.Add(ivglBR2, jvglBR2, "Betriebsregel2")
                            serievLine2 = .getSeriesLine("line2v", "Yellow")
                            serievLine2.Add(ivglBR2, serieMin_j, "Betriebsregel2")
                            serievLine2.Add(ivglBR2, jvglBR2, "Betriebsregel2")

                            seriehLine3 = .getSeriesLine("line3h", "Red")
                            seriehLine3.Add(serieMin_i, jvglBR3, "Betriebsregel3")
                            seriehLine3.Add(ivglBR3, jvglBR3, "Betriebsregel3")
                            serievLine3 = .getSeriesLine("line3v", "Red")
                            serievLine3.Add(ivglBR3, serieMin_j, "Betriebsregel3")
                            serievLine3.Add(ivglBR3, jvglBR3, "Betriebsregel3")


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
                                serie.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID.ToString())
                            Else
                                'ungültige Lösung zeichnen
                                serie_inv.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID.ToString())
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
                        'Hintergrund grau anzeigen
                        .Walls.Back.Transparent = False
                        .Walls.Back.Gradient.Visible = False
                        'MarksTips entfernen
                        .Tools.Clear(True)
                        'Serien unsichtbar machen
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
                        serie.Add(ind.Zielwerte(Me.Zielauswahl(i)), ind.Zielwerte(Me.Zielauswahl(j)), ind.ID.ToString())

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

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Common.Constants.ReferenceSolution = Me.ComboBox1.SelectedItem
        Me.ComboBox1.Dispose()
        Me.matrix.Dispose()
        Me.Button1.Dispose()
        ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
        InitializeComponent()
        Me.ComboBox1.Text = Common.Constants.ReferenceSolution

        'Diagramme zeichnen
        Call Me.zeichnen()
    End Sub
End Class
