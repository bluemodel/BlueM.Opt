Option Strict Off ' Off ist Default
Option Explicit On
Imports System.IO
Imports System.Management

'*******************************************************************************
'*******************************************************************************
'**** ihwb Optimierung                                                      ****
'****                                                                       ****
'**** Christoph Huebner, Felix Froehlich, Dirk Muschalla                    ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'****                                                                       ****
'**** Erstellt: Dezember 2003                                               ****
'****                                                                       ****
'**** Letzte �nderung: Juli 2007                                           ****
'*******************************************************************************
'*******************************************************************************

Partial Class Form1
    Inherits System.Windows.Forms.Form

#Region "Eigenschaften"

    Private IsInitializing As Boolean

    Private PhysCPU As Integer                              'Anzahl physikalischer Prozessoren
    Private LogCPU As Integer                               'Anzahl logischer Prozessoren

    'Anwendung
    Private Anwendung As String
    Private Const ANW_BLUEM As String = "BlueM"
    Private Const ANW_SMUSI As String = "Smusi"
    Private Const ANW_TESTPROBLEME As String = "Testprobleme"
    Private Const ANW_TSP As String = "Traveling Salesman"

    'Optimierungsmethode
    Private Method As String
    Private Const METH_RESET As String = "Reset"
    Private Const METH_PES As String = "PES"
    Private Const METH_CES As String = "CES"
    Private Const METH_CES_PES As String = "CES + PES"
    Private Const METH_HYBRID As String = "HYBRID"
    Private Const METH_SENSIPLOT As String = "SensiPlot"

    '**** Deklarationen der Module *****
    Public WithEvents Sim1 As Sim
    Public SensiPlot1 As SensiPlot
    Public CES1 As EVO.Kern.CES
    Public TSP1 As TSP

    '**** Globale Parameter Parameter Optimierung ****
    'ToDo: diese Werte sollten eigentlich nur in CES bzw PES vorgehalten werden
    Dim globalAnzPar As Short
    Dim globalAnzZiel As Short
    Dim globalAnzRand As Short
    Dim array_x() As Double
    Dim array_y() As Double
    Dim Bestwert(,) As Double = {}
    Dim SekPopulation(,) As Double
    Dim myPara() As Double

    '**** Verschiedenes ****
    Dim isrun As Boolean = False                        'Optimierung l�uft
    Dim ispause As Boolean = False                      'Optimierung ist pausiert

    Const eol As String = Chr(13) & Chr(10)             'Zeilenumbruch

    'Dialoge
    Public WithEvents solutionDialog As SolutionDialog
    Public WithEvents scatterplot1 As Scatterplot

#End Region 'Eigenschaften

#Region "Methoden"

    'Initialisierung von Form1
    '*************************
    Private Sub Form1_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        'Anzahl der Prozessoren wird ermittelt
        Anzahl_Prozessoren(PhysCPU, LogCPU)

        'XP-look
        System.Windows.Forms.Application.EnableVisualStyles()

        'Liste der Anwendungen in ComboBox schreiben und Anfangseinstellung w�hlen
        ComboBox_Anwendung.Items.AddRange(New Object() {"", ANW_BLUEM, ANW_SMUSI, ANW_TESTPROBLEME, ANW_TSP})
        ComboBox_Anwendung.SelectedIndex = 0

        'Liste der Methoden in ComboBox schreiben und Anfangseinstellung w�hlen
        ComboBox_Methode.Items.AddRange(New Object() {"", METH_RESET, METH_PES, METH_CES, METH_CES_PES, METH_HYBRID, METH_SENSIPLOT})
        ComboBox_Methode.SelectedIndex = 0
        ComboBox_Methode.Enabled = False

        'Ende der Initialisierung
        IsInitializing = False

    End Sub

#Region "Initialisierung der Anwendungen"

    'Die Anwendung wurde ausgew�hlt und wird jetzt initialisiert
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    'Anwendung wurde ausgew�hlt
    '**************************
    Private Sub INI_App(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Anwendung.SelectedIndexChanged, Testprobleme1.Testproblem_Changed

        If (Me.IsInitializing = True) Then

            Exit Sub

        Else

            'Diagramm zur�cksetzen
            Me.DForm.Diag.Reset()

            'Alles deaktivieren, danach je nach Anwendung aktivieren
            '-------------------------------------------------------

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Testprobleme deaktivieren
            Testprobleme1.Enabled = False

            'Combobox Methode deaktivieren
            ComboBox_Methode.Enabled = False

            'Ergebnis-Buttons
            Me.Button_openMDB.Enabled = False
            Me.Button_Scatterplot.Enabled = False

            'EVO_Settings zur�cksetzen
            EVO_Settings1.isSaved = False

            'Mauszeiger busy
            Cursor = Cursors.WaitCursor

            Me.Anwendung = ComboBox_Anwendung.SelectedItem

            Select Case Me.Anwendung

                Case "" 'Keine Anwendung ausgew�hlt
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Mauszeiger wieder normal
                    Cursor = Cursors.Default
                    Exit Sub


                Case ANW_BLUEM 'Anwendung BlueM
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse BlueM initialisieren
                    Sim1 = New BlueM()
                    Call Me.displayWorkDir()


                Case ANW_SMUSI 'Anwendung Smusi
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse Smusi initialisieren
                    Sim1 = New Smusi()
                    Call Me.displayWorkDir()


                Case ANW_TESTPROBLEME 'Anwendung Testprobleme
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Test-Probleme aktivieren
                    Testprobleme1.Enabled = True

                    'EVO_Einstellungen aktivieren
                    EVO_Settings1.Enabled = True

                    EVO_Settings1.OptModus = Testprobleme1.OptModus

                    'Globale Parameter werden gesetzt
                    Call Testprobleme1.Parameter_Uebergabe(Testprobleme1.Combo_Testproblem.Text, Testprobleme1.Text_Sinusfunktion_Par.Text, Testprobleme1.Text_Schwefel24_Par.Text, globalAnzPar, globalAnzZiel, globalAnzRand, myPara)

                    'Start-Button aktivieren (keine Methodenauswahl erforderlich)
                    Button_Start.Enabled = True


                Case ANW_TSP 'Anwendung Traveling Salesman Problem (TSP)
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    TSP1 = New TSP

                    Call TSP1.TSP_Initialize(DForm.Diag)

                    'Start-Button aktivieren (keine Methodenauswahl erforderlich)
                    Button_Start.Enabled = True

            End Select

            'Mauszeiger wieder normal
            Cursor = Cursors.Default

            'Combobox Methode aktivieren
            If (Not Anwendung = ANW_TESTPROBLEME And Not Anwendung = ANW_TSP) Then
                ComboBox_Methode.Enabled = True
            End If

        End If

    End Sub

    'Methode wurde ausgew�hlt
    '************************
    Private Sub INI_Method(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_IniMethod.Click, ComboBox_Methode.SelectedIndexChanged

        If (Me.IsInitializing = True) Then

            Exit Sub

        Else

            'Diagramm zur�cksetzen
            Me.DForm.Diag.Reset()

            'Alles deaktivieren, danach je nach Methode aktivieren
            '-----------------------------------------------------

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Ergebnis-Buttons
            Me.Button_openMDB.Enabled = False
            Me.Button_Scatterplot.Enabled = False

            'EVO_Einstellungen deaktivieren
            EVO_Settings1.Enabled = False

            'EVO_Settings zur�cksetzen
            EVO_Settings1.isSaved = False

            'Mauszeiger busy
            Cursor = Cursors.WaitCursor

            'Methode setzen und an Sim �bergeben
            Me.Method = ComboBox_Methode.SelectedItem
            Sim1.Method = Me.Method

            Select Case Me.Method

                Case "" 'Keine Methode ausgew�hlt
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Mauszeiger wieder normal
                    Cursor = Cursors.Default
                    Exit Sub


                Case METH_RESET 'Methode Reset
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Original ModellParameter schreiben
                    Call Sim1.Write_ModellParameter()

                    MsgBox("Die Startwerte der Optimierungsparameter wurden in die Eingabedateien geschrieben.", MsgBoxStyle.Information, "Info")

                    'Ergebnis-Buttons
                    Me.Button_openMDB.Enabled = True

                Case METH_SENSIPLOT 'Methode SensiPlot
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    SensiPlot1 = New SensiPlot

                    'SensiPlot f�r Sim vorbereiten
                    Call Sim1.read_and_valid_INI_Files_PES()

                    'SensiPlot Dialog anzeigen:
                    '--------------------------
                    'List_Boxen f�llen
                    Dim i As Integer
                    For i = 0 To Sim1.List_OptParameter.GetUpperBound(0)
                        Call SensiPlot1.ListBox_OptParameter_add(Sim1.List_OptParameter(i))
                    Next
                    For i = 0 To Sim1.List_OptZiele.GetUpperBound(0)
                        Call SensiPlot1.ListBox_OptZiele_add(Sim1.List_OptZiele(i))
                    Next
                    'Dialog anzeigen
                    Dim SensiPlotDiagResult As Windows.Forms.DialogResult
                    SensiPlotDiagResult = SensiPlot1.ShowDialog()
                    If (Not SensiPlotDiagResult = Windows.Forms.DialogResult.OK) Then
                        'Mauszeiger wieder normal
                        Cursor = Cursors.Default
                        Exit Sub
                    End If


                Case METH_PES 'Methode PES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'EVO_Einstellungen aktivieren
                    EVO_Settings1.Enabled = True

                    'Ergebnis-Buttons
                    Me.Button_openMDB.Enabled = True

                    'PES f�r Sim vorbereiten
                    Call Sim1.read_and_valid_INI_Files_PES()

                    'Je nach Anzahl der Zielfunktionen von MO auf SO umschalten
                    If (Sim1.List_OptZiele.GetLength(0) = 1) Then
                        EVO_Settings1.OptModus = 0
                    ElseIf (Sim1.List_OptZiele.GetLength(0) > 1) Then
                        EVO_Settings1.OptModus = 1
                    End If

                    'Parameter�bergabe an PES
                    Call Sim1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel, globalAnzRand, myPara)

                Case METH_CES, METH_CES_PES, METH_HYBRID 'Methode CES und Methode CES_PES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Ergebnis-Buttons
                    Me.Button_openMDB.Enabled = True
                    Me.Button_Scatterplot.Enabled = True

                    'Funktioniert nur bei BlueM!
                    If (Not Anwendung = ANW_BLUEM) Then
                        Throw New Exception("CES funktioniert bisher nur mit BlueM!")
                    End If

                    'Fallunterscheidung CES oder CES_PES
                    Select Case Me.Method
                        Case METH_CES
                            'CES f�r Sim vorbereiten (Files lesen und Validieren)
                            Call Sim1.read_and_valid_INI_Files_CES()

                        Case METH_CES_PES, METH_HYBRID
                            'EVO_Einstellungen aktiviern
                            EVO_Settings1.Enabled = True

                            'CES f�r Sim vorbereiten (Files lesen und Validieren)
                            Call Sim1.read_and_valid_INI_Files_CES_PES()

                    End Select

                    'CES initialisieren
                    CES1 = New EVO.Kern.CES
                    'Pr�ft ob die Zahl m�g. Kombinationen < Zahl Eltern + Nachfolger
                    If (CES1.n_Childs + CES1.n_Parents) > Sim1.No_of_Combinations Then
                        Throw New Exception("Die Zahl der Eltern + die Zahl der Kinder ist gr��er als die m�gliche Zahl der Kombinationen.")
                    End If

                    'Je nach Anzahl der Zielfunktionen von MO auf SO umschalten
                    If (Sim1.List_OptZiele.GetLength(0) = 1) Then
                        EVO_Settings1.OptModus = 0
                    ElseIf (Sim1.List_OptZiele.GetLength(0) > 1) Then
                        EVO_Settings1.OptModus = 1
                    End If

                    'Anzahl der Ziele, Locations und Verzeigungen wird an CES �bergeben
                    CES1.n_Penalty = Sim1.List_OptZiele.GetLength(0)
                    CES1.n_Constrain = Sim1.List_Constraints.GetLength(0)
                    CES1.n_Locations = Sim1.List_Locations.GetLength(0)
                    CES1.n_Verzweig = Sim1.VerzweigungsDatei.GetLength(0)
                    CES1.TestModus = Sim1.Set_TestModus
                    CES1.n_Combinations = Sim1.No_of_Combinations

                    'Bei Testmodus wird die Anzahl der Kinder und Generationen �berschrieben
                    If CES1.TestModus = 1 Then
                        CES1.n_Childs = 1
                        CES1.n_Parents = 1
                        CES1.n_Generations = 1
                        ReDim CES1.NDSResult(CES1.n_Childs + CES1.n_Parents - 1)
                    ElseIf CES1.TestModus = 2 Then
                        CES1.n_Childs = CES1.n_Combinations
                        CES1.n_Generations = 1
                        ReDim CES1.NDSResult(CES1.n_Childs + CES1.n_Parents - 1)
                    End If

                    'Gibt die PathSize an f�r jede Pfadstelle
                    Dim i As Integer
                    ReDim CES1.n_PathDimension(CES1.n_Locations - 1)
                    For i = 0 To CES1.n_Locations - 1
                        CES1.n_PathDimension(i) = Sim1.List_Locations(i).List_Massnahmen.GetLength(0)
                    Next

            End Select

            'IniApp OK -> Start Button aktivieren
            Me.Button_Start.Enabled = True

            'Mauszeiger wieder normal
            Cursor = Cursors.Default

        End If

    End Sub

    'Arbeitsverzeichnis/Datensatz �ndern
    '***********************************
    Private Sub changeDatensatz(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel_WorkDir.LinkClicked

        'Alten Datensatz dem Dialog zuweisen
        OpenFileDialog_Datensatz.InitialDirectory = Sim1.WorkDir
        OpenFileDialog_Datensatz.FileName = Sim1.WorkDir & Sim1.Datensatz & ".ALL"
        'Dialog �ffnen
        Dim DatensatzResult As DialogResult = OpenFileDialog_Datensatz.ShowDialog()
        'Neuen Datensatz speichern
        If (DatensatzResult = Windows.Forms.DialogResult.OK) Then
            Call Sim1.saveDatensatz(OpenFileDialog_Datensatz.FileName)
        End If
        'Methodenauswahl wieder zur�cksetzen (Der Benutzer muss zuerst Ini neu ausf�hren!)
        Me.ComboBox_Methode.SelectedItem = ""

    End Sub

    'Arbeitsverzeichnis wurde ge�ndert -> Anzeige aktualisieren
    '**********************************************************
    Private Sub displayWorkDir() Handles Sim1.WorkDirChange

        Dim pfad As String
        pfad = Sim1.WorkDir & Sim1.Datensatz & ".ALL"

        'Datensatzanzeige aktualisieren
        If (File.Exists(pfad)) Then
            Me.LinkLabel_WorkDir.Text = pfad
            Me.LinkLabel_WorkDir.Links(0).LinkData = Sim1.WorkDir
        Else
            Me.LinkLabel_WorkDir.Text = "bitte Datensatz ausw�hlen!"
            Me.LinkLabel_WorkDir.Links(0).LinkData = CurDir()
        End If

    End Sub

#End Region 'Initialisierung der Anwendungen

#Region "Start Button Pressed"

    'Start BUTTON wurde pressed
    'XXXXXXXXXXXXXXXXXXXXXXXXXX

    Private Sub STARTEN_Button_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Button_Start.Click

        If (Me.isrun And Not Me.ispause) Then
            'Optimierung pausieren
            '---------------------
            Me.ispause = True
            Me.Button_Start.Text = ">"
            Do While (Me.ispause)
                Application.DoEvents()
            Loop

        ElseIf (Me.isrun) Then
            'Optimierung weiterlaufen lassen
            '-------------------------------
            Me.ispause = False
            Me.Button_Start.Text = "||"

        Else
            'Optimierung starten
            '-------------------
            Me.isrun = True
            Me.Button_Start.Text = "||"

            'Ergebnis-Buttons
            Me.Button_Scatterplot.Enabled = True

            'Try

            Select Case Anwendung

                Case ANW_BLUEM, ANW_SMUSI

                    Select Case Method
                        Case METH_RESET
                            Call Sim1.launchSim()
                        Case METH_SENSIPLOT
                            Call STARTEN_SensiPlot()
                        Case METH_PES
                            Call STARTEN_PES()
                        Case METH_CES
                            Call STARTEN_CES_or_CES_PES()
                        Case METH_CES_PES
                            Call STARTEN_CES_or_CES_PES()
                        Case METH_HYBRID
                            Call STARTEN_CES_or_CES_PES()
                    End Select

                Case ANW_TESTPROBLEME
                    Call STARTEN_PES()

                Case ANW_TSP
                    Call STARTEN_TSP()

            End Select

            ''Globale Fehlerbehandlung f�r Optimierungslauf:
            'Catch ex As Exception
            '    MsgBox(ex.Message, MsgBoxStyle.Critical, "Fehler")
            'End Try

            'Optimierung beendet
            '-------------------
            Me.isrun = False
            Me.Button_Start.Text = ">"
            Me.Button_Start.Enabled = False

        End If

    End Sub

    'Anwendung SensiPlot - START; l�uft ohne Evolutionsstrategie             
    '***********************************************************
    Private Sub STARTEN_SensiPlot()

        'Hinweis:
        '------------------------------------------------------------------------
        'Die Modellparameter werden auch f�r die nicht ausgew�hlten OptParameter 
        'geschrieben, und zwar mit den in der OPT-Datei angegebenen Startwerten
        '------------------------------------------------------------------------

        Dim serie As Steema.TeeChart.Styles.Series
        Dim surface As New Steema.TeeChart.Styles.Surface

        'Parameter�bergabe an ES
        Me.globalAnzZiel = 1
        Me.globalAnzRand = 0
        Me.globalAnzPar = SensiPlot1.Selected_OptParameter.GetLength(0)

        'Anzahl Simulationen
        Dim Anz_Sim As Integer
        If (Me.globalAnzPar = 1) Then
            '1 Parameter
            Anz_Sim = SensiPlot1.Anz_Steps
        Else
            '2 Parameter
            Anz_Sim = SensiPlot1.Anz_Steps ^ 2
        End If

        'Wave deklarieren
        Dim Wave1 As New Wave.Wave

        Dim i, j, n As Integer

        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        'Oberfl�chendiagramm
        If (Me.globalAnzPar > 1) Then
            surface = New Steema.TeeChart.Styles.Surface(Me.DForm.Diag.Chart)
            surface.IrregularGrid = True
            surface.NumXValues = SensiPlot1.Anz_Steps
            surface.NumZValues = SensiPlot1.Anz_Steps
            'Diagramm drehen (rechter Mausbutton)
            Dim rotate1 As New Steema.TeeChart.Tools.Rotate
            rotate1.Button = Windows.Forms.MouseButtons.Right
            Me.DForm.Diag.Tools.Add(rotate1)
            'Punkte anklicken (linker Mausbutton)
            Me.DForm.Diag.add_MarksTips(surface)
            surface.Title = "SensiPlot"
            surface.Cursor = Cursors.Hand
        End If

        'Simulationsschleife
        '-------------------
        Randomize()

        n = 0

        '�ussere Schleife (2. OptParameter)
        '----------------------------------
        For i = 0 To ((SensiPlot1.Anz_Steps - 1) * (Me.globalAnzPar - 1))

            '2. OptParameterwert variieren
            If (Me.globalAnzPar > 1) Then
                Select Case SensiPlot1.Selected_SensiType
                    Case "Gleichverteilt"
                        Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).SKWert = Rnd()
                    Case "Diskret"
                        Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).SKWert = i / SensiPlot1.Anz_Steps
                End Select
                n += 1
            End If

            'Innere Schleife (1. OptParameter)
            '---------------------------------
            For j = 0 To SensiPlot1.Anz_Steps - 1

                '1. OptParameterwert variieren
                Select Case SensiPlot1.Selected_SensiType
                    Case "Gleichverteilt"
                        Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).SKWert = Rnd()
                    Case "Diskret"
                        Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).SKWert = j / SensiPlot1.Anz_Steps
                End Select
                n += 1

                'Modellparameter schreiben
                Call Sim1.Write_ModellParameter()

                'Simulieren
                Call Sim1.launchSim()

                'Qwert berechnen
                Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).QWertTmp = Sim1.QWert(Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel))

                'BUG 253: Constraints f�r SensiPlot fehlen noch

                'Diagramm aktualisieren
                If (Me.globalAnzPar = 1) Then
                    '1 Parameter
                    serie = DForm.Diag.getSeriesPoint("SensiPlot", "Orange")
                    serie.Add(Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).QWertTmp, Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Wert, n)
                Else
                    '2 Parameter
                    surface.Add(Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Wert, Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).QWertTmp, Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Wert, n)
                End If

                'Simulationsergebnis in Wave laden
                If (SensiPlot1.show_Wave) Then
                    'SimReihe auslesen
                    Dim SimReihe As Wave.Zeitreihe
                    SimReihe = Sim1.SimErgebnis.getReihe(Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).SimGr)
                    'OptParameter und -Wert an Titel anh�ngen
                    'TODO: bei 2-Parametern auch den Wert des 2. Parameters anh�ngen!
                    SimReihe.Title += " (" & Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Bezeichnung & ": " _
                                                    & Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Wert & ")"
                    Wave1.Display_Series(SimReihe)
                End If

                'L�sung abspeichern
                Call Sim1.OptResult.addSolution(n, Sim1.List_OptZiele, Sim1.List_Constraints, Sim1.List_OptParameter)

                System.Windows.Forms.Application.DoEvents()

            Next
        Next

        'Wave Diagramm anzeigen:
        '-----------------------
        If (SensiPlot1.show_Wave) Then
            Call Wave1.Show()
        End If

    End Sub


    'Anwendung Traveling Salesman - Start                         
    '************************************
    Private Sub STARTEN_TSP()

        'Laufvariable f�r die Generationen
        Dim gen As Integer

        'BUG 212: Nach Klasse Diagramm auslagern!
        Call TSP1.TeeChart_Initialise_TSP(DForm.Diag)

        'Arrays werden Dimensioniert
        Call TSP1.Dim_Parents_TSP()
        Call TSP1.Dim_Childs()

        'Zuf�llige Kinderpfade werden generiert
        Call TSP1.Generate_Random_Path_TSP()

        'Generationsschleife
        For gen = 1 To TSP1.n_Gen

            'Den Kindern werden die St�dte Ihres Pfades entsprechend zugewiesen
            Call TSP1.Cities_according_ChildPath()

            'Bestimmung des der Qualit�t der Kinder
            Call TSP1.Evaluate_child_Quality()

            'Sortieren der Kinden anhand der Qualit�t
            Call TSP1.Sort_Faksimile(TSP1.ChildList)

            'Selections Prozess (�bergabe der Kinder an die Eltern je nach Strategie)
            Call TSP1.Selection_Process()

            'Zeichnen des besten Elter
            'TODO: funzt nur, wenn ganz am ende gezeichnet wird
            If gen = TSP1.n_Gen Then
                Call TSP1.TeeChart_Zeichnen_TSP(DForm.Diag, TSP1.ParentList(0).Image)
            End If

            'Kinder werden Hier vollst�ndig gel�scht
            Call TSP1.Reset_Childs()

            'Reproduktionsoperatoren, hier gehts dezent zur Sache
            Call TSP1.Reproduction_Control()

            'Mutationsoperatoren
            Call TSP1.Mutation_Control()

        Next gen

    End Sub

    'Anwendung CES und CES_PES             
    '*************************
    Private Sub STARTEN_CES_or_CES_PES()

        'Fehlerabfragen
        If (Sim1.List_OptZiele.GetLength(0) > 2) Then
            Throw New Exception("Zu viele Ziele f�r CES. Max=2")
        End If

        Dim durchlauf_all As Integer = 0
        Dim serie As Steema.TeeChart.Styles.Series

        'Laufvariable f�r die Generationen
        Dim gen As Integer
        Dim i, j, m As Integer

        'Parents und Childs werden Dimensioniert
        ReDim CES1.List_Parents(CES1.n_Parents - 1)
        Call CES1.Faksimile_Dim(CES1.List_Parents, "Parent")
        ReDim CES1.List_Childs(CES1.n_Childs - 1)
        Call CES1.Faksimile_Dim(CES1.List_Childs, "Child")

        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        'Die verschiedenen Modi
        'xxxxxxxxxxxxxxxxxxxxxx
        If CES1.TestModus = 0 Then
            'Normaler Modus: Zuf�llige Kinderpfade werden generiert
            Call CES1.Generate_Random_Path()
        ElseIf CES1.TestModus = 1 Then
            'Testmodus 1: Funktion zum testen einer ausgew�hlten Kombinationen
            Sim1.get_TestPath(CES1.List_Childs(0).Path)
        ElseIf CES1.TestModus = 2 Then
            'Testmodus 2: Funktion zum  testen aller Kombinationen
            Call CES1.Generate_All_Test_Paths()
        End If

        'HYBRID ToDo sollte hier nicht der Para und Dn vector initialisiert werden? Exakt!
        'Am einfachsten die aktuellen Elemente mit dem Child f�hren
        '****** Soll er immer machen, da diese f�r Skos gebraucht werden

        'Hier werden dem Child die passenden Massnahmen und deren Elemente pro Location zugewiesen
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        For i = 0 To CES1.n_Childs - 1
            For j = 0 To CES1.n_Locations - 1
                Call Sim1.Identify_Measures_Elements_Parameters(j, CES1.List_Childs(i).Path(j), CES1.List_Childs(i).Measures(j), CES1.List_Childs(i).Loc(j).Loc_Elem, CES1.List_Childs(i).Loc(j).Loc_Para)
            Next
        Next

        'Schritt 0: PES - Objekt der Klasse PES wird erzeugt PES wird erzeugt
        '*********************************************************************
        Dim PES1 As EVO.Kern.PES
        PES1 = New EVO.Kern.PES

        'Falls HYBRID werden entprechend der Einstellung im PES die Parameter auf Zuf�llig oder Start gesetzt
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        If Method = METH_HYBRID Then
            'pro Child
            'xxxxxxxxx
            For i = 0 To CES1.n_Childs - 1
                'Und pro Location
                'xxxxxxxxxxxxxxxx
                For j = 0 To CES1.n_Locations - 1

                    'Die Parameter (falls vorhanden) werden �berschrieben
                    If Not CES1.List_Childs(i).Loc(j).Loc_Para.GetLength(1) = 0 Then

                        'Standard Parameter werden aus dem Sim besorgt
                        Call Sim1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel, globalAnzRand, myPara)

                        'Die Zahl der Parameter wird �berschrieben (AnzZiel und AnzRand sind OK)
                        'Anzahl der Parameter bezieht sich hier nur auf eine Location
                        globalAnzPar = CES1.List_Childs(i).Loc(j).Loc_Para.GetLength(1)

                        ReDim myPara(CES1.List_Childs(i).Loc(j).Loc_Para.GetUpperBound(1))
                        For m = 0 To CES1.List_Childs(i).Loc(j).Loc_Para.GetUpperBound(1)
                            myPara(m) = CES1.List_Childs(i).Loc(j).Loc_Para(1, m)
                        Next
                        '1. EVO_Settings zur�cksetzen; 2. Die Settings werden f�r Hybrid gesetzt
                        EVO_Settings1.isSaved = False
                        Call EVO_Settings1.SetFor_CES_PES(1, 1, 1)

                        'Schritte 1 - 3: PES wird initialisiert (Weiteres siehe dort ;-)
                        '**************************************************************
                        Call PES1.PesInitialise(EVO_Settings1.PES_Settings, globalAnzPar, globalAnzZiel, globalAnzRand, myPara, Method)

                        'Dem Child wird der Schrittweitenvektor zugewiesen und gegebenenfalls der Parameter zuf�llig gew�hlt
                        'wird also nicht in PES.ESStarten gemacht
                        ReDim CES1.List_Childs(i).Loc(j).Loc_Dn(CES1.List_Childs(i).Loc(j).Loc_Para.GetUpperBound(1))
                        For m = 0 To CES1.List_Childs(i).Loc(j).Loc_Para.GetUpperBound(1)
                            CES1.List_Childs(i).Loc(j).Loc_Dn(m) = EVO_Settings1.PES_Settings.DnStart
                            If EVO_Settings1.PES_Settings.iStartPar = Kern.EVO_STARTPARAMETER.Zufall Then
                                Randomize()
                                CES1.List_Childs(i).Loc(j).Loc_Para(1, m) = Rnd()
                            End If
                        Next
                    End If
                Next
            Next
        End If


        'Startwerte werden der Verlaufsanzeige werden zugewiesen
        Call Me.INI_Verlaufsanzeige(1, 1, CES1.n_Generations, CES1.n_Childs)

        'Generationsschleife CES
        'xxxxxxxxxxxxxxxxxxxxxxx
        For gen = 0 To CES1.n_Generations - 1

            Call EVO_Opt_Verlauf1.Generation(gen + 1)

            'Child Schleife
            'xxxxxxxxxxxxxx
            For i = 0 To CES1.n_Childs - 1
                durchlauf_all += 1

                Call EVO_Opt_Verlauf1.Nachfolger(i + 1)

                '****************************************
                'Aktueller Pfad wird an Sim zur�ckgegeben
                'Bereitet das BlaueModell f�r die Kombinatorik vor
                Call Sim1.PREPARE_Evaluation_CES(CES1.List_Childs(i).Path)

                'HYBRID: Bereitet f�r die Optimierung mit den PES Parametern vor
                '***************************************************************
                If Method = METH_HYBRID Then
                    Call Sim1.Reduce_OptPara_and_ModPara(CES1.List_Childs(i).All_Elem)
                    Call Sim1.PREPARE_Evaluation_PES(CES1.List_Childs(i).All_Para)
                End If

                'Simulation *************************************************************************
                Call Sim1.SIM_Evaluierung(durchlauf_all, CES1.List_Childs(i).Penalty, CES1.List_Childs(i).Constrain)
                '************************************************************************************

                'HYBRID: Speichert die PES Erfahrung diesen Childs im PES Memory
                '***************************************************************
                If Method = METH_HYBRID Then
                    Call CES1.Memory_Store(i, gen)
                End If

                'Zeichnen MO_SO der Kinder
                serie = DForm.Diag.getSeriesPoint("Childs", "", Steema.TeeChart.Styles.PointerStyles.Rectangle, 4)
                If CES1.n_Penalty = 1 Then
                    Call serie.Add(durchlauf_all, CES1.List_Childs(i).Penalty(0))
                ElseIf CES1.n_Penalty = 2 Then
                    Call serie.Add(CES1.List_Childs(i).Penalty(0), CES1.List_Childs(i).Penalty(1))
                End If

                System.Windows.Forms.Application.DoEvents()
            Next

            'MO oder SO SELEKTIONSPROZESS oder NDSorting SELEKTION
            '-----------------------------------------------------
            If CES1.n_Penalty = 1 Then
                'Sortieren der Kinden anhand der Qualit�t
                Call CES1.Sort_Faksimile(CES1.List_Childs)
                'Selectionsprozess je nach "plus" oder "minus" Strategie
                Call CES1.Selection_Process()
                'Zeichnen der besten Eltern
                For i = 0 To CES1.n_Parents - 1
                    'durchlauf += 1
                    serie = DForm.Diag.getSeriesPoint("Parent", "", Steema.TeeChart.Styles.PointerStyles.Diamond, 4)
                    Call serie.Add(durchlauf_all, CES1.List_Parents(i).Penalty(0))
                Next

            ElseIf CES1.n_Penalty = 2 Then
                'NDSorting ******************
                Call CES1.NDSorting_Control()
                'Zeichnen von NDSortingResult
                Call DForm.Diag.DeleteSeries(CES1.n_Childs - 1, 1)
                serie = DForm.Diag.getSeriesPoint("Front:" & 1, "", Steema.TeeChart.Styles.PointerStyles.Triangle, 4)
                For i = 0 To CES1.n_Childs - 1
                    Call serie.Add(CES1.NDSResult(i).Penalty(0), CES1.NDSResult(i).Penalty(1))
                Next
            End If

            'REPRODUKTION und MUTATION Nicht wenn Testmodus
            '***********************************************
            If CES1.TestModus = 0 Then
                'Kinder werden zur Sicherheit gel�scht aber nicht zerst�rt ;-)
                Call CES1.Faksimile_Dim(CES1.List_Childs, "Child")
                'Reproduktionsoperatoren, hier gehts dezent zur Sache
                Call CES1.Reproduction_Control()
                'Mutationsoperatoren
                Call CES1.Mutation_Control()
            End If

            'Hier werden dem Child die passenden Elemente pro Location zugewiesen
            For i = 0 To CES1.n_Childs - 1
                For j = 0 To CES1.n_Locations - 1
                    Call Sim1.Identify_Measures_Elements_Parameters(j, CES1.List_Childs(i).Path(j), CES1.List_Childs(i).Measures(j), CES1.List_Childs(i).Loc(j).Loc_Elem, CES1.List_Childs(i).Loc(j).Loc_Para)
                Next
            Next

            'HYBRID: REPRODUKTION und MUTATION
            '*********************************
            If Method = METH_HYBRID Then
                'pro Child
                'xxxxxxxxx
                For i = 0 To CES1.List_Childs.GetUpperBound(0)

                    'Ermittelt fuer jedes Child den PES Parent Satz
                    Call CES1.Memory_Search(CES1.List_Childs(i))

                    'und pro Location
                    'xxxxxxxxxxxxxxxx
                    For j = 0 To CES1.n_Locations - 1

                        'Die Parameter (falls vorhanden) werden �berschrieben
                        If Not CES1.List_Childs(i).Loc(j).Loc_Para.GetLength(1) = 0 Then

                            '??????????????????????
                            PES1 = New EVO.Kern.PES

                            'Standard Parameter werden aus dem Sim besorgt
                            Call Sim1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel, globalAnzRand, myPara)

                            'Die Zahl der Parameter wird �berschrieben (AnzZiel und AnzRand sind OK)
                            'Anzahl der Parameter bezieht sich hier nur auf eine Location
                            For m = 0 To CES1.PES_Parents.GetUpperBound(0)
                                If CES1.PES_Parents(m).iLocation = j + 1 Then
                                    globalAnzPar = CES1.PES_Parents(m).Loc(j).Loc_Para.GetLength(1)
                                    Exit For
                                End If
                            Next

                            'Die Anzahl der Eltern wird bestimmt
                            Dim n_eltern As Integer = 0
                            For m = 0 To CES1.PES_Parents.GetUpperBound(0)
                                If (j + 1) = CES1.PES_Parents(m).iLocation Then
                                    n_eltern += 1
                                End If
                            Next

                            'Die Kinder bekommen je nach Fall (Eltern keine Eltern) neue Parameter
                            If n_eltern = 0 Then
                                'Falls noch keine Eltern vorhanden sind
                                ReDim CES1.List_Childs(i).Loc(j).Loc_Dn(CES1.List_Childs(i).Loc(j).Loc_Para.GetUpperBound(1))
                                For m = 0 To CES1.List_Childs(i).Loc(j).Loc_Para.GetUpperBound(1)
                                    CES1.List_Childs(i).Loc(j).Loc_Dn(m) = EVO_Settings1.PES_Settings.DnStart
                                    'Falls zuf�llige Startwerte
                                    If EVO_Settings1.PES_Settings.iStartPar = Kern.EVO_STARTPARAMETER.Zufall Then
                                        Randomize()
                                        CES1.List_Childs(i).Loc(j).Loc_Para(1, m) = Rnd()
                                    End If
                                Next
                            Else
                                EVO_Settings1.isSaved = False
                                Call EVO_Settings1.SetFor_CES_PES(1, n_eltern, 1)

                                'Schritte 1 - 3: PES wird initialisiert (Weiteres siehe dort ;-)
                                '**************************************************************
                                Call PES1.PesInitialise(EVO_Settings1.PES_Settings, globalAnzPar, globalAnzZiel, globalAnzRand, myPara, Method)

                                Dim Index As Integer = 0
                                For m = 0 To CES1.PES_Parents.GetUpperBound(0)
                                    If (j + 1) = CES1.PES_Parents(m).iLocation Then
                                        'Die Startwerte werden �berschrieben
                                        Call PES1.EsStartvalues(CES1.PES_Parents(m).Loc(j).Loc_Dn, CES1.PES_Parents(m).Loc(j).Parameter, Index)
                                        Index += 1
                                    End If
                                Next

                                'Startet die Prozesse evolutionstheoretischen Prozesse nacheinander
                                Call PES1.EsReproMut()

                                'Auslesen der Variierten Parameter
                                myPara = PES1.EsGetParameter()
                                CES1.List_Childs(i).Loc(j).Parameter = PES1.EsGetParameter()

                            End If
                        End If
                    Next
                Next
            End If

        Next
        'Ende der Generationsschleife CES

        'Falls jetzt noch PES ausgef�hrt werden soll
        'Starten der PES mit der Front von CES
        '*******************************************
        If Method = METH_CES_PES Then
            Call Start_PES_after_CES()
        End If

    End Sub

    'Starten der PES mit der Front von CES
    '(MaxAnzahl ist die Zahl der Eltern -> ToDo: SecPop oder Bestwertspeicher)
    '*************************************************************************
    Private Sub Start_PES_after_CES()
        Dim i As Integer

        '1. EVO_Settings zur�cksetzen; 2. Einstellungen f�r PES werden gesetzt (AnzGen, AnzEltern, AnzNachf)
        EVO_Settings1.isSaved = False
        Call EVO_Settings1.SetFor_CES_PES(1, 3, 5)

        For i = 0 To CES1.n_Parents - 1
            If CES1.List_Parents(i).Front = 1 Then

                '****************************************
                'Aktueller Pfad wird an Sim zur�ckgegeben
                'Bereitet das BlaueModell f�r die Kombinatorik vor
                Call Sim1.PREPARE_Evaluation_CES(CES1.List_Childs(i).Path)

                'Hier werden Child die passenden Elemente zugewiesen
                Dim j As Integer
                For j = 0 To CES1.n_Locations - 1
                    Call Sim1.Identify_Measures_Elements_Parameters(j, CES1.List_Childs(i).Path(j), CES1.List_Childs(i).Measures(j), CES1.List_Childs(i).Loc(j).Loc_Elem, CES1.List_Childs(i).Loc(j).Loc_Para)
                Next

                'Reduktion der OptimierungsParameter und immer dann wenn nicht Nullvariante
                'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                If Sim1.Reduce_OptPara_and_ModPara(CES1.List_Childs(i).All_Elem) Then

                    'Parameter�bergabe an PES
                    '************************
                    Call Sim1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel, globalAnzRand, myPara)
                    'Starten der PES
                    '***************
                    Call STARTEN_PES()

                End If
            End If
        Next
    End Sub

    'Anwendung Evolutionsstrategie f�r Parameter Optimierung - hier Steuerung       
    '************************************************************************

    Private Sub STARTEN_PES()
        '==========================
        Dim i As Integer
        '--------------------------
        Dim durchlauf As Integer
        '--------------------------
        'Dimensionierung der Variablen f�r Optionen Evostrategie
        'Das Struct aus PES wird hier verwendet
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        'Check!
        Dim QN() As Double = {}
        Dim RN() As Double = {}
        '--------------------------
        'TODO: If (ipop + igen + inachf + irunde) > 4 Then GoTo Start_Evolutionsrunden '????? Wie?
        'Werte an Variablen �bergeben auskommentiert Werte finden sich im PES werden hier aber nicht zugewiesen
        'Kann der Kommentar nicht weg?
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        ReDim QN(globalAnzZiel - 1)
        ReDim RN(globalAnzRand - 1)

        'Diagramm vorbereiten und initialisieren
        If (Not Me.Method = METH_CES_PES) Then
            Call PrepareDiagramm()
        End If

        'Schritte 0: Objekt der Klasse PES wird erzeugt
        '**********************************************
        Dim PES1 As EVO.Kern.PES
        PES1 = New EVO.Kern.PES

        'Schritte 1 - 3: ES wird initialisiert (Weiteres siehe dort ;-)
        '**************************************************************
        Call PES1.PesInitialise(EVO_Settings1.PES_Settings, globalAnzPar, globalAnzZiel, globalAnzRand, myPara, Method)

        'Startwerte werden der Verlaufsanzeige werden zugewiesen
        Call Me.INI_Verlaufsanzeige(EVO_Settings1.PES_Settings.NRunden, EVO_Settings1.PES_Settings.NPopul, EVO_Settings1.PES_Settings.NGen, EVO_Settings1.PES_Settings.NNachf)

        durchlauf = 0

Start_Evolutionsrunden:

        '�ber alle Runden
        'xxxxxxxxxxxxxxxx
        For PES1.PES_iAkt.iAktRunde = 0 To PES1.PES_Settings.NRunden - 1

            Call EVO_Opt_Verlauf1.Runden(PES1.PES_iAkt.iAktRunde + 1)
            Call PES1.EsResetPopBWSpeicher() 'Nur bei Komma Strategie

            '�ber alle Populationen
            'xxxxxxxxxxxxxxxxxxxxxx
            For PES1.PES_iAkt.iAktPop = 0 To PES1.PES_Settings.NPopul - 1

                Call EVO_Opt_Verlauf1.Populationen(PES1.PES_iAkt.iAktPop + 1)

                'POPULATIONS REPRODUKTIONSPROZESS
                '################################
                'Ermitteln der neuen Ausgangswerte f�r Nachkommen aus den Eltern der Population
                Call PES1.EsPopReproduktion()

                'POPULATIONS MUTATIONSPROZESS
                '############################
                'Mutieren der Ausgangswerte der Population
                Call PES1.EsPopMutation()

                '�ber alle Generationen
                'xxxxxxxxxxxxxxxxxxxxxx
                For PES1.PES_iAkt.iAktGen = 0 To PES1.PES_Settings.NGen - 1

                    Call EVO_Opt_Verlauf1.Generation(PES1.PES_iAkt.iAktGen + 1)
                    Call PES1.EsResetBWSpeicher()  'Nur bei Komma Strategie

                    '�ber alle Nachkommen
                    'xxxxxxxxxxxxxxxxxxxxxxxxx
                    For PES1.PES_iAkt.iAktNachf = 0 To PES1.PES_Settings.NNachf - 1

                        Call EVO_Opt_Verlauf1.Nachfolger(PES1.PES_iAkt.iAktNachf + 1)

                        durchlauf += 1

                        'Um Modellfehler bzw. Evaluierungsabbr�che abzufangen
                        'TODO: noch nicht fertig das Ergebnis wird noch nicht auf Fehler ueberprueft
                        Dim Eval_Count As Integer = 0
                        Dim SIM_Eval_is_OK As Boolean = True
                        Do
                            'REPRODUKTIONSPROZESS
                            '####################
                            'Ermitteln der neuen Ausgangswerte f�r Nachkommen aus den Eltern
                            Call PES1.EsReproduktion()

                            'MUTATIONSPROZESS
                            '################
                            'Mutieren der Ausgangswerte
                            Call PES1.EsMutation()

                            'Auslesen der Variierten Parameter
                            myPara = PES1.EsGetParameter()

                            'Auslesen des Bestwertspeichers
                            If (Not EVO_Settings1.PES_Settings.is_MO_Pareto) Then
                                Bestwert = PES1.EsGetBestwert()
                            End If

                            'Ansteuerung der zu optimierenden Anwendung
                            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                            Select Case Anwendung

                                Case ANW_TESTPROBLEME

                                    Call Testprobleme1.Evaluierung_TestProbleme(Testprobleme1.Combo_Testproblem.Text, myPara, durchlauf, PES1.PES_iAkt.iAktPop, QN, RN, DForm.Diag)

                                Case ANW_BLUEM, ANW_SMUSI

                                    'Vorbereiten des Modelldatensatzes
                                    Call Sim1.PREPARE_Evaluation_PES(myPara)

                                    'Evaluierung des Simulationsmodells (ToDo: Valid�tspr�fung fehlt)
                                    SIM_Eval_is_OK = Sim1.SIM_Evaluierung(durchlauf, QN, RN)

                                    'L�sung im TeeChart einzeichnen
                                    '==============================
                                    Dim serie As Steema.TeeChart.Styles.Series

                                    'Constraintverletzung pr�fen
                                    Dim isInvalid As Boolean = False
                                    For i = 0 To globalAnzRand - 1
                                        If (RN(i) < 0) Then
                                            isInvalid = True
                                            Exit For
                                        End If
                                    Next

                                    If (Not EVO_Settings1.PES_Settings.is_MO_Pareto) Then
                                        'SingleObjective
                                        'xxxxxxxxxxxxxxx
                                        If (isInvalid) Then
                                            serie = DForm.Diag.getSeriesPoint("Population " & (PES1.PES_iAkt.iAktPop + 1).ToString() & " (ung�ltig)", "Gray")
                                        Else
                                            serie = DForm.Diag.getSeriesPoint("Population " & (PES1.PES_iAkt.iAktPop + 1).ToString())
                                        End If
                                        Call serie.Add(durchlauf, QN(0), durchlauf.ToString())

                                    Else
                                        'MultiObjective
                                        'xxxxxxxxxxxxxx
                                        If (globalAnzZiel = 2) Then
                                            '2D-Diagramm
                                            '------------------------------------------------------------------------
                                            If (isInvalid) Then
                                                serie = DForm.Diag.getSeriesPoint("Population" & " (ung�ltig)", "Gray")
                                            Else
                                                serie = DForm.Diag.getSeriesPoint("Population", "Orange")
                                            End If
                                            Call serie.Add(QN(0), QN(1), durchlauf.ToString())

                                        Else
                                            '3D-Diagramm (Es werden die ersten drei Zielfunktionswerte eingezeichnet)
                                            '------------------------------------------------------------------------
                                            Dim serie3D As Steema.TeeChart.Styles.Points3D
                                            If (isInvalid) Then
                                                serie3D = DForm.Diag.getSeries3DPoint("Population" & " (ung�ltig)", "Gray")
                                            Else
                                                serie3D = DForm.Diag.getSeries3DPoint("Population", "Orange")
                                            End If
                                            Call serie3D.Add(QN(0), QN(1), QN(2), durchlauf.ToString())

                                        End If
                                    End If

                            End Select

                            Eval_Count += 1
                            If (Eval_Count >= 10) Then
                                Throw New Exception("Es konnte kein g�ltiger Datensatz erzeugt werden!")
                            End If

                        Loop While SIM_Eval_is_OK = False

                        'SELEKTIONSPROZESS Schritt 1
                        '###########################
                        'Einordnen der Qualit�tsfunktion im Bestwertspeicher bei SO
                        'Falls MO Einordnen der Qualit�tsfunktion in NDSorting
                        Call PES1.EsBest(QN, RN)

                        System.Windows.Forms.Application.DoEvents()

                    Next 'Ende Schleife �ber alle Nachkommen

                    'SELEKTIONSPROZESS Schritt 2 f�r NDSorting sonst Xe = Xb
                    '#######################################################
                    'Die neuen Eltern werden generiert
                    Call PES1.EsEltern()

                    'Sekund�re Population
                    If (EVO_Settings1.PES_Settings.is_MO_Pareto) Then
                        SekPopulation = PES1.EsGetSekund�rePopulation()
                        If (Not IsNothing(Sim1)) Then
                            'SekPop abspeichern
                            Call Sim1.OptResult.setSekPop(SekPopulation, PES1.PES_iAkt.iAktGen)
                            'SekPop mit Solution.IDs zeichnen
                            Call Sekund�rePopulationZeichnen(PES1.PES_iAkt.iAktGen)
                        Else
                            'SekPop einfach so zeichnen
                            Call Sekund�rePopulationZeichnen(SekPopulation)
                        End If
                    End If

                    System.Windows.Forms.Application.DoEvents()

                Next 'Ende alle Generatione
                'xxxxxxxxxxxxxxxxxxxxxxxxxxx
                System.Windows.Forms.Application.DoEvents()

                'POPULATIONS SELEKTIONSPROZESS  Schritt 1
                '########################################
                'Einordnen der Qualit�tsfunktion im PopulationsBestwertspeicher
                Call PES1.EsPopBest()

            Next 'Ende alle Populationen
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

            'POPULATIONS SELEKTIONSPROZESS  Schritt 2
            '########################################
            'Die neuen Populationseltern werden generiert
            Call PES1.EsPopEltern()
            System.Windows.Forms.Application.DoEvents()

        Next 'Ende alle Runden
        'xxxxxxxxxxxxxxxxxxxxx

    End Sub

    'Zeichenfunktionen
    'XXXXXXXXXXXXXXXXX

    'Sekund�re Population zeichnen
    '*****************************
    Private Sub Sekund�rePopulationZeichnen(ByVal SekPop(,) As Double)

        Dim i As Short
        Dim serie As Steema.TeeChart.Styles.Series
        Dim serie3D As Steema.TeeChart.Styles.Points3D

        If (globalAnzZiel = 2) Then
            '2 Zielfunktionen
            '----------------------------------------------------------------
            serie = DForm.Diag.getSeriesPoint("Sekund�re Population", "Green")
            serie.Clear()
            For i = 0 To SekPop.GetUpperBound(0)
                serie.Add(SekPop(i, 0), SekPop(i, 1))
            Next i

        ElseIf (globalAnzZiel >= 3) Then
            '3 oder mehr Zielfunktionen (es werden die ersten drei angezeigt)
            '----------------------------------------------------------------
            serie3D = DForm.Diag.getSeries3DPoint("Sekund�re Population", "Green")
            serie3D.Clear()
            For i = 0 To SekPop.GetUpperBound(0)
                serie3D.Add(SekPop(i, 0), SekPop(i, 1), SekPop(i, 2))
            Next i
        End If

    End Sub

    'Sekund�re Population anhand von Sim-Ergebnisspeicher zeichnen
    '*************************************************************
    Private Sub Sekund�rePopulationZeichnen(ByVal _igen as Integer)

        Dim i As Short
        Dim serie As Steema.TeeChart.Styles.Series
        Dim serie3D As Steema.TeeChart.Styles.Points3D
        Dim solutions() As IHWB.EVO.Solution

        'SekPop holen
        solutions = Sim1.OptResult.getSekPop(_igen)

        If (globalAnzZiel = 2) Then
            '2 Zielfunktionen
            '----------------------------------------------------------------
            serie = DForm.Diag.getSeriesPoint("Sekund�re Population", "Green")
            serie.Clear()
            For i = 0 To solutions.GetUpperBound(0)
                serie.Add(solutions(i).QWerte(0), solutions(i).QWerte(1), solutions(i).ID)
            Next i

        ElseIf (globalAnzZiel >= 3) Then
            '3 oder mehr Zielfunktionen (es werden die ersten drei angezeigt)
            '----------------------------------------------------------------
            serie3D = DForm.Diag.getSeries3DPoint("Sekund�re Population", "Green")
            serie3D.Clear()
            For i = 0 To solutions.GetUpperBound(0)
                serie3D.Add(solutions(i).QWerte(0), solutions(i).QWerte(1), solutions(i).QWerte(2), solutions(i).ID)
            Next i
        End If

    End Sub

    'Startwerte werden der Verlaufsanzeige werden zugewiesen
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

    Private Sub INI_Verlaufsanzeige(ByRef NRunden As Integer, ByRef NPopul As Integer, ByRef NGen As Integer, ByRef NNachf As Integer)
        EVO_Opt_Verlauf1.NRunden = NRunden
        EVO_Opt_Verlauf1.NPopul = NPopul
        EVO_Opt_Verlauf1.NGen = NGen
        EVO_Opt_Verlauf1.NNachf = NNachf
        EVO_Opt_Verlauf1.Initialisieren()
    End Sub


#End Region 'Start Button Pressed

#Region "Diagrammfunktionen"

    'Diagrammfunktionen
    '###################

    'Achsen und Standard-Series initialisieren
    '*****************************************
    Private Sub PrepareDiagramm()

        Dim i As Integer

        Select Case Anwendung

            Case ANW_TESTPROBLEME 'Testprobleme
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                Call Testprobleme1.DiagInitialise(Me.EVO_Settings1.PES_Settings, globalAnzPar, Me.DForm.Diag)

            Case ANW_BLUEM, ANW_SMUSI 'BlueM oder SMUSI
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                Select Case Method

                    Case METH_SENSIPLOT 'SensiPlot
                        'XXXXXXXXXXXXXXXXXXXXXXXXX

                        Dim Achse As Diagramm.Achse
                        Dim Achsen As New Collection

                        If (SensiPlot1.Selected_OptParameter.GetLength(0) = 1) Then

                            '1 OptParameter:
                            '---------------

                            'Achsen:
                            '-------
                            'X-Achse = QWert
                            Achse.Name = Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).Bezeichnung
                            Achse.Auto = True
                            Achse.Max = 0
                            Achsen.Add(Achse)
                            'Y-Achse = OptParameter
                            Achse.Name = Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Bezeichnung
                            Achse.Auto = True
                            Achse.Max = 0
                            Achsen.Add(Achse)

                        Else
                            '2 OptParameter:
                            '---------------

                            'Achsen:
                            '-------
                            'X-Achse = OptParameter1
                            Achse.Name = Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Bezeichnung
                            Achse.Auto = True
                            Achse.Max = 0
                            Achsen.Add(Achse)
                            'Y-Achse = QWert
                            Achse.Name = Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).Bezeichnung
                            Achse.Auto = True
                            Achse.Max = 0
                            Achsen.Add(Achse)
                            'Z-Achse = OptParameter2
                            Achse.Name = Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Bezeichnung
                            Achse.Auto = True
                            Achse.Max = 0
                            Achsen.Add(Achse)

                        End If

                        'Diagramm initialisieren
                        Call DForm.Diag.DiagInitialise(Anwendung, Achsen)


                    Case METH_CES, METH_CES_PES, METH_HYBRID 'Methode CES
                        'XXXXXXXXXXXXXXXXXXXXX

                        'Achsen:
                        '-------
                        Dim Achse As Diagramm.Achse
                        Dim Achsen As New Collection
                        'Bei SO: X-Achse = Simulationen
                        If (EVO_Settings1.PES_Settings.is_MO_Pareto = False) Then
                            Achse.Name = "Simulation"
                            Achse.Auto = False
                            Achse.Max = CES1.n_Childs * CES1.n_Generations
                            Achsen.Add(Achse)
                        End If
                        'f�r jede Zielfunktion eine weitere Achse hinzuf�gen
                        'HACK: Diagramm-Achsen bisher nur f�r Anwendung BlueM!
                        For i = 0 To Sim1.List_OptZiele.GetUpperBound(0)
                            Achse.Name = Sim1.List_OptZiele(i).Bezeichnung
                            Achse.Auto = True
                            Achse.Max = 0
                            Achsen.Add(Achse)
                        Next

                        'Diagramm initialisieren
                        Call DForm.Diag.DiagInitialise(Anwendung, Achsen)

                    Case METH_PES 'Methode PES
                        'XXXXXXXXXXXXXXXXXXXXX

                        'Achsen:
                        '-------
                        Dim Achse As Diagramm.Achse
                        Dim Achsen As New Collection

                        'Bei Single-Objective: X-Achse = Nr. der Simulation (Durchlauf)
                        If (EVO_Settings1.PES_Settings.is_MO_Pareto = False) Then

                            Achse.Name = "Simulation"
                            Achse.Auto = False
                            If EVO_Settings1.PES_Settings.isPOPUL Then
                                Achse.Max = EVO_Settings1.PES_Settings.NGen * EVO_Settings1.PES_Settings.NNachf * EVO_Settings1.PES_Settings.NRunden + 1
                            Else
                                Achse.Max = EVO_Settings1.PES_Settings.NGen * EVO_Settings1.PES_Settings.NNachf + 1
                            End If
                            Achsen.Add(Achse)

                        End If

                        'f�r jede Zielfunktion eine weitere Achse hinzuf�gen
                        For i = 0 To Sim1.List_OptZiele.GetUpperBound(0)
                            Achse.Name = Sim1.List_OptZiele(i).Bezeichnung
                            Achse.Auto = True
                            Achse.Max = 0
                            Achsen.Add(Achse)
                        Next

                        'Diagramm initialisieren
                        Call DForm.Diag.DiagInitialise(Anwendung, Achsen)

                    Case Else 'andere Anwendungen
                        'XXXXXXXXXXXXXXXXXXXXXXXX
                        Throw New Exception("Diese Funktion ist f�r die Anwendung '" & Anwendung & "' nicht vorgesehen")

                End Select

        End Select

        Call Application.DoEvents()

    End Sub

#Region "UI"

    'Klick auf Serie in Diagramm
    '***************************
    Public Sub selectPoint(ByVal sender As Object, ByVal s As Steema.TeeChart.Styles.Series, ByVal valueIndex As Integer, ByVal e As System.Windows.Forms.MouseEventArgs)

        'Notwendige Bedingungen �berpr�fen
        '---------------------------------
        If (IsNothing(Sim1)) Then
            'Anwendung != Sim
            MsgBox("Wave funktioniert nur bei Anwendungen BlueM oder SMUSI!", MsgBoxStyle.Information, "Info")
            Exit Sub
        Else

            Dim xWert, yWert, zWert As Double
            Dim xAchse, yAchse, zAchse As String
            Dim solutionID As Integer
            Dim sol As Solution

            'Punkt-Informationen bestimmen
            '-----------------------------
            'X und Y Werte
            xWert = s.XValues(valueIndex)
            yWert = s.YValues(valueIndex)
            'X und Y Achsen (Zielfunktionen)
            xAchse = Me.DForm.Diag.Chart.Axes.Bottom.Title.Caption
            yAchse = Me.DForm.Diag.Chart.Axes.Left.Title.Caption
            'Solution-ID
            solutionID = s.Labels(valueIndex)

            'L�sung holen
            '------------
            sol = Sim1.OptResult.getSolution(solutionID)

            If (sol.ID = solutionID) Then

                'L�sung zu ausgew�hlten L�sungen hinzuf�gen
                Call Sim1.OptResult.selectSolution(sol.ID)

                'In Chart anzeigen
                If (Not s.HasZValues) Then
                    '2D-Diagramm
                    '-----------
                    Dim serie As Steema.TeeChart.Styles.Series
                    serie = Me.DForm.Diag.getSeriesPoint("ausgew�hlte L�sungen", "Red", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
                    serie.Add(xWert, yWert, sol.ID.ToString())
                    serie.Marks.Visible = True
                    serie.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Label
                    serie.Marks.Transparency = 50
                    serie.Marks.ArrowLength = 10
                Else
                    '3D-Diagramm
                    '-----------
                    Dim serie3D As Steema.TeeChart.Styles.Points3D
                    serie3D = s
                    zWert = serie3D.ZValues(valueIndex)
                    zAchse = Me.DForm.Diag.Chart.Axes.Depth.Title.Caption
                    serie3D = Me.DForm.Diag.getSeries3DPoint("ausgew�hlte L�sungen", "Red", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
                    serie3D.Add(xWert, yWert, zWert, sol.ID.ToString())
                    serie3D.Marks.Visible = True
                    serie3D.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Label
                    serie3D.Marks.Transparency = 50
                    serie3D.Marks.ArrowLength = 10
                End If

                'L�sung ausw�hlen
                Call Me.selectSolution(sol)

            End If

        End If

    End Sub

    'Eine L�sung ausw�hlen
    '*********************
    Public Sub selectSolution(ByVal sol As Solution) Handles scatterplot1.solutionSelected

        'L�sungsdialog initialisieren
        If (IsNothing(Me.solutionDialog)) Then
            Me.solutionDialog = New SolutionDialog(Sim1.List_OptParameter, Sim1.List_OptZiele, Sim1.List_Constraints)
        End If

        'L�sungsdialog anzeigen
        Call Me.solutionDialog.Show()

        'L�sung zum L�sungsdialog hinzuf�gen
        Call Me.solutionDialog.addSolution(sol)

        'L�sungsdialog nach vorne bringen
        Call Me.solutionDialog.BringToFront()

    End Sub

    'L�sungsauswahl zur�cksetzen
    '***************************
    Public Sub clearSelection(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Serie der ausgew�hlten L�sungen l�schen
        '=======================================

        'Im Hauptdiagramm
        '----------------
        If (globalAnzZiel < 3) Then
            '2D-Diagramm
            '-----------
            Dim serie As Steema.TeeChart.Styles.Series
            serie = Me.DForm.Diag.getSeriesPoint("ausgew�hlte L�sungen")
            serie.Dispose()
        Else
            '2D-Diagramm
            '-----------
            Dim serie3D As Steema.TeeChart.Styles.Points3D
            serie3D = Me.DForm.Diag.getSeries3DPoint("ausgew�hlte L�sungen")
            serie3D.Dispose()
        End If
        Call Me.DForm.Diag.Refresh()

        'In der Scatterplot-Matrix
        '-------------------------
        If (Not IsNothing(Me.scatterplot1)) Then
            Call scatterplot1.clearSelection()
        End If

        'Auswahl intern zur�cksetzen
        '===========================
        Call Sim1.OptResult.clearSelectedSolutions()

    End Sub

    'ausgew�hlte L�sungen simulieren und in Wave anzeigen
    '****************************************************
    Public Sub showWave(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim i As Integer
        Dim isOK As Boolean

        Dim zre As Wave.Zeitreihe
        Dim SimSeries As New Collection                 'zu zeichnende Simulationsreihen
        Dim RefSeries As New Collection                 'zu zeichnende Referenzreihen

        'Wait cursor
        Cursor = Cursors.WaitCursor

        'Wave instanzieren
        Dim Wave1 As New Wave.Wave()

        'Alle ausgew�hlten L�sungen durchlaufen
        '======================================
        For Each sol As Solution In Sim1.OptResult.getSelectedSolutions()

            'Simulation ausf�hren
            'xxxxxxxxxxxxxxxxxxxx

            'OptParameter �bernehmen
            For i = 0 To Sim1.List_OptParameter.GetUpperBound(0)
                Sim1.List_OptParameter(i).Wert = sol.OptPara(i)
            Next

            'Modellparameter schreiben
            Call Sim1.Write_ModellParameter()

            'Simulieren
            isOK = Sim1.launchSim()

            'Zu zeichnenden Simulationsreihen zur�cksetzen
            SimSeries.Clear()

            'zu zeichnenden Reihen aus Liste der OptZiele raussuchen
            '-------------------------------------------------------
            For i = 0 To Sim1.List_OptZiele.GetUpperBound(0)

                With Sim1.List_OptZiele(i)

                    'ggf. Referenzreihe in Wave laden
                    If (.ZielTyp = "Reihe" Or .ZielTyp = "IHA") Then
                        'Referenzreihen nur jeweils ein Mal zeichnen
                        If (Not RefSeries.Contains(.ZielReiheDatei & .ZielGr)) Then
                            RefSeries.Add(.ZielGr, .ZielReiheDatei & .ZielGr)
                            'Referenzreihe in Wave laden
                            Wave1.Display_Series(.ZielReihe)
                        End If
                    End If

                    'Simulationsergebnis in Wave laden
                    If (Not SimSeries.Contains(.SimGr)) Then
                        SimSeries.Add(.SimGr, .SimGr)
                        zre = Sim1.SimErgebnis.getReihe(.SimGr).copy()
                        'L�sungsnummer an Titel anh�ngen
                        zre.Title &= " (L�sung " & sol.ID.ToString() & ")"
                        'Simreihe in Wave laden
                        Wave1.Display_Series(zre)
                    End If

                End With
            Next

        Next sol

        'Wave anzeigen
        '-------------
        Call Wave1.Show()

        'Cursor
        Cursor = Cursors.Default

    End Sub

    'Scatterplot-Matrix anzeigen
    '****************************
    Private Sub showScatterplot(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Scatterplot.Click

        Dim diagresult As DialogResult
        Dim SekPopOnly As Boolean

        'Abfrage, ob nur Sekund�re Population gezeichnet werden soll
        '-----------------------------------------------------------
        diagresult = MsgBox("Soll nur die Sekund�re Population angezeigt werden?", MsgBoxStyle.YesNo, "Scatterplot-Matrix")

        If (diagresult = Windows.Forms.DialogResult.Yes) Then
            SekPopOnly = True
        End If

        Cursor = Cursors.WaitCursor

        'Scatterplot-Matrix
        '------------------
        scatterplot1 = New Scatterplot(Sim1.OptResult, SekPopOnly)
        Call scatterplot1.Show()

        Cursor = Cursors.Default

    End Sub

    'Optimierungsergebnis aus einer Datenbank einlesen
    '*************************************************
    Private Sub loadFromMDB(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_openMDB.Click

        Dim diagresult As DialogResult
        Dim sourceFile As String

        'Datei-�ffnen Dialog anzeigen
        Me.OpenFileDialog_MDB.InitialDirectory = Sim1.WorkDir
        diagresult = Me.OpenFileDialog_MDB.ShowDialog()

        If (diagresult = Windows.Forms.DialogResult.OK) Then

            sourceFile = Me.OpenFileDialog_MDB.FileName

            'MDBImportDialog
            '---------------
            Dim importDialog As New MDBImportDialog()
            For Each OptZiel As Sim.Struct_OptZiel In Sim1.List_OptZiele
                importDialog.ListBox_OptZieleX.Items.Add(OptZiel.Bezeichnung)
                importDialog.ListBox_OptZieleY.Items.Add(OptZiel.Bezeichnung)
                importDialog.ListBox_OptZieleZ.Items.Add(OptZiel.Bezeichnung)
            Next
            'Bei weniger als 3 Zielen Z-Achse ausblenden
            If (Sim1.List_OptZiele.Length < 3) Then
                importDialog.ListBox_OptZieleZ.Enabled = False
            End If
            diagresult = importDialog.ShowDialog()

            If (diagresult = Windows.Forms.DialogResult.OK) Then

                'Cursor Wait
                Cursor = Cursors.WaitCursor

                'Daten einlesen
                '==============
                Call Sim1.OptResult.db_load(sourceFile)

                'Hauptdiagramm
                '=============
                Dim OptZielIndexX, OptZielIndexY, OptZielIndexZ As Integer
                OptZielIndexX = importDialog.ListBox_OptZieleX.SelectedIndex
                OptZielIndexY = importDialog.ListBox_OptZieleY.SelectedIndex
                OptZielIndexZ = importDialog.ListBox_OptZieleZ.SelectedIndex

                'Achsen
                '------
                Dim Achsen As New Collection
                Dim tmpAchse As EVO.Diagramm.Achse
                tmpAchse.Auto = True
                'X-Achse
                tmpAchse.Name = importDialog.ListBox_OptZieleX.SelectedItem
                Achsen.Add(tmpAchse)
                'Y-Achse
                tmpAchse.Name = importDialog.ListBox_OptZieleY.SelectedItem
                Achsen.Add(tmpAchse)
                If (Not OptZielIndexZ = -1) Then
                    'Z-Achse
                    tmpAchse.Name = importDialog.ListBox_OptZieleZ.SelectedItem
                    Achsen.Add(tmpAchse)
                End If

                'Diagramm initialisieren
                '-----------------------
                Me.DForm.Diag.Clear()
                Me.DForm.Diag.DiagInitialise(Path.GetFileName(sourceFile), Achsen)

                'Punkte eintragen
                '----------------
                Dim serie As Steema.TeeChart.Styles.Series
                Dim serie3D As Steema.TeeChart.Styles.Points3D

                'L�sungen
                '========
                If (importDialog.ComboBox_SekPop.SelectedItem <> "ausschlie�lich") Then

                    For Each sol As Solution In Sim1.OptResult.Solutions
                        
                            If (OptZielIndexZ = -1) Then
                                '2D
                                '--
                                'Constraintverletzung pr�fen
                                If (sol.isValid) Then
                                    serie = Me.DForm.Diag.getSeriesPoint("Population", "Orange")
                                Else
                                    serie = Me.DForm.Diag.getSeriesPoint("Population (ung�ltig)", "Gray")
                                End If
                                'Zeichnen
                                serie.Add(sol.QWerte(OptZielIndexX), sol.QWerte(OptZielIndexY), sol.ID)
                            Else
                                '3D
                                '--
                                'Constraintverletzung pr�fen
                                If (sol.isValid) Then
                                    serie3D = Me.DForm.Diag.getSeries3DPoint("Population", "Orange")
                                Else
                                    serie3D = Me.DForm.Diag.getSeries3DPoint("Population (ung�ltig)", "Gray")
                                End If
                                'Zeichnen
                                serie3D.Add(sol.QWerte(OptZielIndexX), sol.QWerte(OptZielIndexY), sol.QWerte(OptZielIndexZ), sol.ID)
                            End If

                    Next

                End If

                'Sekund�rpopulation
                '==================
                If (importDialog.ComboBox_SekPop.SelectedItem <> "keine") Then

                    For Each sekpopsol As Solution In Sim1.OptResult.getSekPop()
                        If (OptZielIndexZ = -1) Then
                            '2D
                            '--
                            serie = Me.DForm.Diag.getSeriesPoint("Sekund�re Population", "Green")
                            serie.Add(sekpopsol.QWerte(OptZielIndexX), sekpopsol.QWerte(OptZielIndexY), sekpopsol.ID)
                        Else
                            '3D
                            '--
                            serie3D = Me.DForm.Diag.getSeries3DPoint("Sekund�re Population", "Green")
                            serie3D.Add(sekpopsol.QWerte(OptZielIndexX), sekpopsol.QWerte(OptZielIndexY), sekpopsol.QWerte(OptZielIndexZ), sekpopsol.ID)
                        End If
                    Next

                End If

                'Ergebnis-Buttons
                Me.Button_Scatterplot.Enabled = True

                'Cursor Default
                Cursor = Cursors.Default

            End If

        End If

    End Sub

#End Region 'Diagrammfunktionen

#End Region 'UI

    'Ermittelt beim Start die Anzahl der Physikalischen Prozessoren
    '**************************************************************
    Public Sub Anzahl_Prozessoren(ByRef PhysCPU As Integer, ByRef LogCPU As Integer)
        Dim mc As ManagementClass = New ManagementClass("Win32_Processor")
        Dim moc As ManagementObjectCollection = mc.GetInstances()
        Dim SocketDesignation As String = String.Empty
        Dim PhysCPUarray As ArrayList = New ArrayList

        Dim mo As ManagementObject
        For Each mo In moc
            LogCPU += 1
            SocketDesignation = mo.Properties("SocketDesignation").Value.ToString()
            If Not PhysCPUarray.Contains(SocketDesignation) Then
                PhysCPUarray.Add(SocketDesignation)
            End If
        Next
        PhysCPU = PhysCPUarray.Count

    End Sub

#End Region 'Methoden

End Class
