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
'**** Letzte Änderung: Juli 2007                                           ****
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
    Dim isrun As Boolean = False                        'Optimierung läuft
    Dim ispause As Boolean = False                      'Optimierung ist pausiert

#End Region 'Eigenschaften

#Region "Methoden"

    'Initialisierung von Form1
    '*************************
    Private Sub Form1_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        'Anzahl der Prozessoren wird ermittelt
        Anzahl_Prozessoren(PhysCPU, LogCPU)

        'XP-look
        System.Windows.Forms.Application.EnableVisualStyles()

        'Liste der Anwendungen in ComboBox schreiben und Anfangseinstellung wählen
        ComboBox_Anwendung.Items.AddRange(New Object() {"", ANW_BLUEM, ANW_SMUSI, ANW_TESTPROBLEME, ANW_TSP})
        ComboBox_Anwendung.SelectedIndex = 0

        'Liste der Methoden in ComboBox schreiben und Anfangseinstellung wählen
        ComboBox_Methode.Items.AddRange(New Object() {"", METH_RESET, METH_PES, METH_CES, METH_CES_PES, METH_HYBRID, METH_SENSIPLOT})
        ComboBox_Methode.SelectedIndex = 0
        ComboBox_Methode.Enabled = False

        'Ende der Initialisierung
        IsInitializing = False

    End Sub

#Region "Initialisierung der Anwendungen"

    'Die Anwendung wurde ausgewählt und wird jetzt initialisiert
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    'Anwendung wurde ausgewählt
    '**************************
    Private Sub INI_App(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Anwendung.SelectedIndexChanged, Testprobleme1.Testproblem_Changed

        If (Me.IsInitializing = True) Then

            Exit Sub

        Else

            'Diagramm zurücksetzen
            Me.DForm.Diag.Reset()

            'Alles deaktivieren, danach je nach Anwendung aktivieren
            '-------------------------------------------------------

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Testprobleme deaktivieren
            Testprobleme1.Enabled = False

            'Combobox Methode deaktivieren
            ComboBox_Methode.Enabled = False

            'Scatterplot deaktivieren
            Me.Button_Scatterplot.Enabled = False

            'EVO_Settings zurücksetzen
            EVO_Settings1.isSaved = False

            'Mauszeiger busy
            Cursor = Cursors.WaitCursor

            Me.Anwendung = ComboBox_Anwendung.SelectedItem

            Select Case Me.Anwendung

                Case "" 'Keine Anwendung ausgewählt
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

    'Methode wurde ausgewählt
    '************************
    Private Sub INI_Method(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_IniMethod.Click, ComboBox_Methode.SelectedIndexChanged

        If (Me.IsInitializing = True) Then

            Exit Sub

        Else

            'Diagramm zurücksetzen
            Me.DForm.Diag.Reset()

            'Alles deaktivieren, danach je nach Methode aktivieren
            '-----------------------------------------------------

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Scatterplot deaktivieren
            Me.Button_Scatterplot.Enabled = False

            'EVO_Einstellungen deaktivieren
            EVO_Settings1.Enabled = False

            'EVO_Settings zurücksetzen
            EVO_Settings1.isSaved = False

            'Mauszeiger busy
            Cursor = Cursors.WaitCursor

            'Methode setzen und an Sim übergeben
            Me.Method = ComboBox_Methode.SelectedItem
            Sim1.Method = Me.Method

            Select Case Me.Method

                Case "" 'Keine Methode ausgewählt
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Mauszeiger wieder normal
                    Cursor = Cursors.Default
                    Exit Sub


                Case METH_RESET 'Methode Reset
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Ergebnisdatenbank ausschalten
                    Sim1.Ergebnisdb = False

                    'Original ModellParameter schreiben
                    Call Sim1.Write_ModellParameter()

                    MsgBox("Die Startwerte der Optimierungsparameter wurden in die Eingabedateien geschrieben.", MsgBoxStyle.Information, "Info")


                Case METH_SENSIPLOT 'Methode SensiPlot
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    SensiPlot1 = New SensiPlot

                    'Ergebnisdatenbank einschalten
                    Sim1.Ergebnisdb = True

                    'SensiPlot für Sim vorbereiten
                    Call Sim1.read_and_valid_INI_Files_PES()

                    'SensiPlot Dialog anzeigen:
                    '--------------------------
                    'List_Boxen füllen
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

                    'Ergebnisdatenbank einschalten
                    Sim1.Ergebnisdb = True

                    'Scatterplot aktivieren
                    Me.Button_Scatterplot.Enabled = True

                    'PES für Sim vorbereiten
                    Call Sim1.read_and_valid_INI_Files_PES()

                    'Je nach Anzahl der Zielfunktionen von MO auf SO umschalten
                    If (Sim1.List_OptZiele.GetLength(0) = 1) Then
                        EVO_Settings1.OptModus = 0
                    ElseIf (Sim1.List_OptZiele.GetLength(0) > 1) Then
                        EVO_Settings1.OptModus = 1
                    End If

                    'Parameterübergabe an PES
                    Call Sim1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel, globalAnzRand, myPara)

                Case METH_CES, METH_CES_PES, METH_HYBRID 'Methode CES und Methode CES_PES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Funktioniert nur bei BlueM!
                    If (Not Anwendung = ANW_BLUEM) Then
                        Throw New Exception("CES funktioniert bisher nur mit BlueM!")
                    End If

                    'Ergebnisdatenbank einschalten
                    Sim1.Ergebnisdb = True

                    'Fallunterscheidung CES oder CES_PES
                    Select Case Me.Method
                        Case METH_CES
                            'CES für Sim vorbereiten (Files lesen und Validieren)
                            Call Sim1.read_and_valid_INI_Files_CES()

                        Case METH_CES_PES, METH_HYBRID
                            'EVO_Einstellungen aktiviern
                            EVO_Settings1.Enabled = True

                            'CES für Sim vorbereiten (Files lesen und Validieren)
                            Call Sim1.read_and_valid_INI_Files_CES_PES()

                    End Select

                    'CES initialisieren
                    CES1 = New EVO.Kern.CES
                    'Prüft ob die Zahl mög. Kombinationen < Zahl Eltern + Nachfolger
                    If (CES1.n_Childs + CES1.n_Parents) > Sim1.No_of_Combinations Then
                        Throw New Exception("Die Zahl der Eltern + die Zahl der Kinder ist größer als die mögliche Zahl der Kombinationen.")
                    End If

                    'Je nach Anzahl der Zielfunktionen von MO auf SO umschalten
                    If (Sim1.List_OptZiele.GetLength(0) = 1) Then
                        EVO_Settings1.OptModus = 0
                    ElseIf (Sim1.List_OptZiele.GetLength(0) > 1) Then
                        EVO_Settings1.OptModus = 1
                    End If

                    'Anzahl der Ziele, Locations und Verzeigungen wird an CES übergeben
                    CES1.n_Penalty = Sim1.List_OptZiele.GetLength(0)
                    Ces1.n_Constrain = Sim1.List_Constraints.GetLength(0)
                    CES1.n_Locations = Sim1.List_Locations.GetLength(0)
                    CES1.n_Verzweig = Sim1.VerzweigungsDatei.GetLength(0)
                    CES1.TestModus = Sim1.Set_TestModus
                    CES1.n_Combinations = Sim1.No_of_Combinations

                    'Bei Testmodus wird die Anzahl der Kinder und Generationen überschrieben
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

                    'Gibt die PathSize an für jede Pfadstelle
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

    'Arbeitsverzeichnis/Datensatz ändern
    '***********************************
    Private Sub changeDatensatz(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel_WorkDir.LinkClicked

        'Alten Datensatz dem Dialog zuweisen
        OpenFileDialog_Datensatz.InitialDirectory = Sim1.WorkDir
        OpenFileDialog_Datensatz.FileName = Sim1.WorkDir & Sim1.Datensatz & ".ALL"
        'Dialog öffnen
        Dim DatensatzResult As DialogResult = OpenFileDialog_Datensatz.ShowDialog()
        'Neuen Datensatz speichern
        If (DatensatzResult = Windows.Forms.DialogResult.OK) Then
            Call Sim1.saveDatensatz(OpenFileDialog_Datensatz.FileName)
        End If
        'Methodenauswahl wieder zurücksetzen (Der Benutzer muss zuerst Ini neu ausführen!)
        Me.ComboBox_Methode.SelectedItem = ""

    End Sub

    'Arbeitsverzeichnis wurde geändert -> Anzeige aktualisieren
    '**********************************************************
    Private Sub displayWorkDir() Handles Sim1.WorkDirChange

        Dim pfad As String
        pfad = Sim1.WorkDir & Sim1.Datensatz & ".ALL"

        'Datensatzanzeige aktualisieren
        If (File.Exists(pfad)) Then
            Me.LinkLabel_WorkDir.Text = pfad
            Me.LinkLabel_WorkDir.Links(0).LinkData = Sim1.WorkDir
        Else
            Me.LinkLabel_WorkDir.Text = "bitte Datensatz auswählen!"
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

            ''Globale Fehlerbehandlung für Optimierungslauf:
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

    'Anwendung SensiPlot - START; läuft ohne Evolutionsstrategie             
    '***********************************************************
    Private Sub STARTEN_SensiPlot()

        'Hinweis:
        '------------------------------------------------------------------------
        'Die Modellparameter werden auch für die nicht ausgewählten OptParameter 
        'geschrieben, und zwar mit den in der OPT-Datei angegebenen Startwerten
        '------------------------------------------------------------------------

        Dim serie As Steema.TeeChart.Styles.Series
        Dim surface As New Steema.TeeChart.Styles.Surface

        'Parameterübergabe an ES
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

        Dim i, j As Integer

        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        'Oberflächendiagramm
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

        'Äussere Schleife (2. OptParameter)
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

                'Modellparameter schreiben
                Call Sim1.Write_ModellParameter()

                'Simulieren
                Call Sim1.launchSim()

                'Qwert berechnen
                Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).QWertTmp = Sim1.QWert(Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel))

                'Diagramm aktualisieren
                If (Me.globalAnzPar = 1) Then
                    '1 Parameter
                    serie = DForm.Diag.getSeriesPoint("SensiPlot", "Orange")
                    serie.Add(Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).QWertTmp, Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Wert, "")
                Else
                    '2 Parameter
                    surface.Add(Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Wert, Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).QWertTmp, Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Wert)
                End If

                'Simulationsergebnis in Wave laden
                If (SensiPlot1.show_Wave) Then
                    'BUG 182: Die WEL-Datei hat bei Smusi einen anderen Namen!
                    Dim WEL As New Wave.WEL(Sim1.WorkDir & Sim1.Datensatz & ".wel", Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).SimGr)
                    'OptParameter und -Wert an Titel anhängen
                    'TODO: bei 2-Parametern auch den Wert des 2. Parameters anhängen!
                    WEL.Zeitreihen(0).Title += " (" & Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Bezeichnung & ": " _
                                                    & Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Wert & ")"
                    Wave1.Display_Series(WEL.Zeitreihen(0))
                End If

                'Qualitätswerte und OptParameter in DB speichern
                If (Sim1.Ergebnisdb = True) Then
                    Call Sim1.db_update()
                End If

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

        'Laufvariable für die Generationen
        Dim gen As Integer

        'BUG 212: Nach Klasse Diagramm auslagern!
        Call TSP1.TeeChart_Initialise_TSP(DForm.Diag)

        'Arrays werden Dimensioniert
        Call TSP1.Dim_Parents_TSP()
        Call TSP1.Dim_Childs()

        'Zufällige Kinderpfade werden generiert
        Call TSP1.Generate_Random_Path_TSP()

        'Generationsschleife
        For gen = 1 To TSP1.n_Gen

            'Den Kindern werden die Städte Ihres Pfades entsprechend zugewiesen
            Call TSP1.Cities_according_ChildPath()

            'Bestimmung des der Qualität der Kinder
            Call TSP1.Evaluate_child_Quality()

            'Sortieren der Kinden anhand der Qualität
            Call TSP1.Sort_Faksimile(TSP1.ChildList)

            'Selections Prozess (Übergabe der Kinder an die Eltern je nach Strategie)
            Call TSP1.Selection_Process()

            'Zeichnen des besten Elter
            'TODO: funzt nur, wenn ganz am ende gezeichnet wird
            If gen = TSP1.n_Gen Then
                Call TSP1.TeeChart_Zeichnen_TSP(DForm.Diag, TSP1.ParentList(0).Image)
            End If

            'Kinder werden Hier vollständig gelöscht
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
            Throw New Exception("Zu viele Ziele für CES. Max=2")
        End If

        Dim durchlauf_all As Integer = 0
        Dim serie As Steema.TeeChart.Styles.Series

        'Laufvariable für die Generationen
        Dim gen As Integer
        Dim i, j, m As Integer

        'Parents und Childs werden Dimensioniert
        Redim CES1.List_Parents(CES1.n_Parents -1)
        Call CES1.Faksimile_Dim(CES1.List_Parents, "Parent")
        Redim CES1.List_Childs(CES1.n_Childs -1)
        Call CES1.Faksimile_Dim(CES1.List_Childs, "Child")

        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        'Die verschiedenen Modi
        'xxxxxxxxxxxxxxxxxxxxxx
        If CES1.TestModus = 0 Then
            'Normaler Modus: Zufällige Kinderpfade werden generiert
            Call CES1.Generate_Random_Path()
        ElseIf CES1.TestModus = 1 Then
            'Testmodus 1: Funktion zum testen einer ausgewählten Kombinationen
            Sim1.get_TestPath(CES1.List_Childs(0).Path)
        ElseIf CES1.TestModus = 2 Then
            'Testmodus 2: Funktion zum  testen aller Kombinationen
            Call CES1.Generate_All_Test_Paths()
        End If

        'HYBRID ToDo sollte hier nicht der Para und Dn vector initialisiert werden? Exakt!
        'Am einfachsten die aktuellen Elemente mit dem Child führen
        '****** Soll er immer machen, da diese für Skos gebraucht werden

        'Hier werden dem Child die passenden Massnahmen und deren Elemente pro Location zugewiesen
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        For i = 0 To CES1.n_Childs - 1
            For j = 0 To CES1.n_Locations - 1
                Call Sim1.Identify_Measures_Elements_Parameters(j, CES1.List_Childs(i).Path(j), CES1.List_Childs(i).Measures(j), CES1.List_Childs(i).Loc(j).Loc_Elem, CES1.List_Childs(i).Loc(j).Loc_Para)
            Next
        Next

        '1. Schritt: PES - Objekt der Klasse PES wird erzeugt PES wird erzeugt
        '*********************************************************************
        Dim PES1 As EVO.Kern.PES
        PES1 = New EVO.Kern.PES

        'Falls HYBRID werden entprechend der Einstellung im PES die Parameter auf Zufällig oder Start gesetzt
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        If Method = METH_HYBRID Then
            'pro Child
            'xxxxxxxxx
            For i = 0 To CES1.n_Childs - 1
                'Und pro Location
                'xxxxxxxxxxxxxxxx
                For j = 0 To CES1.n_Locations - 1

                    'Die Parameter (falls vorhanden) werden überschrieben
                    If Not CES1.List_Childs(i).Loc(j).Loc_Para.GetLength(1) = 0 Then

                        'Standard Parameter werden aus dem Sim besorgt
                        Call Sim1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel, globalAnzRand, myPara)

                        'Die Zahl der Parameter wird überschrieben (AnzZiel und AnzRand sind OK)
                        'Anzahl der Parameter bezieht sich hier nur auf eine Location
                        globalAnzPar = CES1.List_Childs(i).Loc(j).Loc_Para.GetLength(1)

                        ReDim myPara(CES1.List_Childs(i).Loc(j).Loc_Para.GetLength(1))
                        For m = 0 To CES1.List_Childs(i).Loc(j).Loc_Para.GetUpperBound(1)
                            myPara(m + 1) = CES1.List_Childs(i).Loc(j).Loc_Para(1, m)
                        Next
                        '1. EVO_Settings zurücksetzen; 2. Die Settings werden für Hybrid gesetzt
                        EVO_Settings1.isSaved = False
                        Call EVO_Settings1.SetFor_CES_PES(1, 1, 1)

                        'Schritte 2 - 5 PES wird initialisiert (Weiteres siehe dort ;-)
                        '**************************************************************
                        Call PES1.PesInitialise(EVO_Settings1.PES_Settings, globalAnzPar, globalAnzZiel, globalAnzRand, myPara, Method)

                        'Dem Child wird der Schrittweitenvektor zugewiesen und gegebenenfalls der Parameter zufällig gewählt
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
                'Aktueller Pfad wird an Sim zurückgegeben
                'Bereitet das BlaueModell für die Kombinatorik vor
                Call Sim1.PREPARE_Evaluation_CES(CES1.List_Childs(i).Path)

                'HYBRID: Bereitet für die Optimierung mit den PES Parametern vor
                '***************************************************************
                If Method = METH_HYBRID Then
                    Call Sim1.Reduce_OptPara_and_ModPara(CES1.List_Childs(i).All_Elem)
                    Call Sim1.PREPARE_Evaluation_PES(CES1.List_Childs(i).All_Para)
                End If

                'Simulation *************************************************************************
                Call Sim1.SIM_Evaluierung(CES1.List_Childs(i).Penalty, CES1.List_Childs(i).Constrain)
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
                'Sortieren der Kinden anhand der Qualität
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
                'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
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

                        'Die Parameter (falls vorhanden) werden überschrieben
                        If Not CES1.List_Childs(i).Loc(j).Loc_Para.GetLength(1) = 0 Then

                            '??????????????????????
                            PES1 = New EVO.Kern.PES

                            'Standard Parameter werden aus dem Sim besorgt
                            Call Sim1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel, globalAnzRand, myPara)

                            'Die Zahl der Parameter wird überschrieben (AnzZiel und AnzRand sind OK)
                            'Anzahl der Parameter bezieht sich hier nur auf eine Location
                            For m = 0 To CES1.PES_Parents.GetUpperBound(0)
                                If CES1.PES_Parents(m).iLocation = j + 1 Then
                                    globalAnzPar = CES1.PES_Parents(m).Loc(j).Loc_Para.GetLength(1)
                                    Exit For
                                End If
                            Next

                            '1. EVO_Settings zurücksetzen; 2. Die Settings werden auf Basis des Memory gesetzt
                            Dim n_eltern As Integer = 0
                            For m = 0 To CES1.PES_Parents.GetUpperBound(0)
                                If j = CES1.PES_Parents(m).iLocation Then
                                    n_eltern += 1
                                End If
                            Next

                            EVO_Settings1.isSaved = False
                            Call EVO_Settings1.SetFor_CES_PES(1, n_eltern, 1)

                            If n_eltern = 0 Then
                                'Falls noch keine Eltern vorhanden sind
                                ReDim CES1.List_Childs(i).Loc(j).Loc_Dn(CES1.List_Childs(i).Loc(j).Loc_Para.GetUpperBound(1))
                                CES1.List_Childs(i).Loc(j).Loc_Dn(m) = EVO_Settings1.PES_Settings.DnStart
                                For m = 0 To CES1.List_Childs(i).Loc(j).Loc_Para.GetUpperBound(1)
                                    'Falls zufällige Startwerte
                                    If EVO_Settings1.PES_Settings.iStartPar = Kern.EVO_STARTPARAMETER.Zufall Then
                                        Randomize()
                                        CES1.List_Childs(i).Loc(j).Loc_Para(1, m) = Rnd()
                                    End If
                                Next
                            Else
                                'Falls Eltern vorhanden sind Selektion, Reproduktion, Mutation
                                Dim k As Integer = 0
                                k = CES1.PES_Parents(0).iLocation





                            End If
                        End If
                    Next
                Next
            End If

        Next
        'Ende der Generationsschleife CES

        'Falls jetzt noch PES ausgeführt werden soll
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

        '1. EVO_Settings zurücksetzen; 2. Einstellungen für PES werden gesetzt (AnzGen, AnzEltern, AnzNachf)
        EVO_Settings1.isSaved = False
        Call EVO_Settings1.SetFor_CES_PES(1, 3, 5)

        For i = 0 To CES1.n_Parents - 1
            If CES1.List_Parents(i).Front = 1 Then

                '****************************************
                'Aktueller Pfad wird an Sim zurückgegeben
                'Bereitet das BlaueModell für die Kombinatorik vor
                Call Sim1.PREPARE_Evaluation_CES(CES1.List_Childs(i).Path)

                'Hier werden Child die passenden Elemente zugewiesen
                Dim j As Integer
                For j = 0 To CES1.n_Locations - 1
                    Call Sim1.Identify_Measures_Elements_Parameters(j, CES1.List_Childs(i).Path(j), ces1.List_Childs(i).Measures(j), CES1.List_Childs(i).Loc(j).Loc_Elem, CES1.List_Childs(i).Loc(j).Loc_Para)
                Next

                'Reduktion der OptimierungsParameter und immer dann wenn nicht Nullvariante
                'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                If Sim1.Reduce_OptPara_and_ModPara(ces1.List_Childs(i).All_Elem) Then

                    'Parameterübergabe an PES
                    '************************
                    Call Sim1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel, globalAnzRand, myPara)
                    'Starten der PES
                    '***************
                    Call STARTEN_PES()

                End If
            End If
        Next
    End Sub

    'Anwendung Evolutionsstrategie für Parameter Optimierung - hier Steuerung       
    '************************************************************************

    Private Sub STARTEN_PES()
        '==========================
        Dim i As Integer
        '--------------------------
        Dim durchlauf As Integer
        '--------------------------
        'Dimensionierung der Variablen für Optionen Evostrategie
        'Das Struct aus PES wird hier verwendet
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        'Check!
        Dim QN() As Double = {}
        Dim RN() As Double = {}
        '--------------------------
        'TODO: If (ipop + igen + inachf + irunde) > 4 Then GoTo Start_Evolutionsrunden '????? Wie?
        'Werte an Variablen übergeben auskommentiert Werte finden sich im PES werden hier aber nicht zugewiesen
        'Kann der Kommentar nicht weg?
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        ReDim QN(globalAnzZiel - 1)
        ReDim RN(globalAnzRand - 1)

        'Diagramm vorbereiten und initialisieren
        If (Not Me.Method = METH_CES_PES) Then
            Call PrepareDiagramm()
        End If

        '1. Schritt: PES
        'Objekt der Klasse PES wird erzeugt
        '**********************************
        Dim PES1 As EVO.Kern.PES
        PES1 = New EVO.Kern.PES

        'Schritte 2 - 5 PES wird initialisiert (Weiteres siehe dort ;-)
        '**************************************************************
        Call PES1.PesInitialise(EVO_Settings1.PES_Settings, globalAnzPar, globalAnzZiel, globalAnzRand, myPara, Method)

        'Startwerte werden der Verlaufsanzeige werden zugewiesen
        Call Me.INI_Verlaufsanzeige(EVO_Settings1.PES_Settings.NRunden, EVO_Settings1.PES_Settings.NPopul, EVO_Settings1.PES_Settings.NGen, EVO_Settings1.PES_Settings.NNachf)

        durchlauf = 0

Start_Evolutionsrunden:

        'Über alle Runden
        'xxxxxxxxxxxxxxxx
        For PES1.PES_iAkt.iAktRunde = 1 To PES1.PES_Settings.NRunden

            Call EVO_Opt_Verlauf1.Runden(PES1.PES_iAkt.iAktRunde)
            Call PES1.EsResetPopBWSpeicher()

            'Über alle Populationen
            'xxxxxxxxxxxxxxxxxxxxxx
            For PES1.PES_iAkt.iAktPop = 1 To PES1.PES_Settings.NPopul

                Call EVO_Opt_Verlauf1.Populationen(PES1.PES_iAkt.iAktPop)
                Call PES1.EsPopVaria()
                Call PES1.EsPopMutation()

                'Über alle Generationen
                'xxxxxxxxxxxxxxxxxxxxxx
                For PES1.PES_iAkt.iAktGen = 1 To PES1.PES_Settings.NGen

                    Call EVO_Opt_Verlauf1.Generation(PES1.PES_iAkt.iAktGen)
                    Call PES1.EsResetBWSpeicher()

                    'Über alle Nachkommen
                    'xxxxxxxxxxxxxxxxxxxxxxxxx
                    For PES1.PES_iAkt.iAktNachf = 1 To PES1.PES_Settings.NNachf

                        Call EVO_Opt_Verlauf1.Nachfolger(PES1.PES_iAkt.iAktNachf)

                        durchlauf += 1

                        'Um Modellfehler bzw. Evaluierungsabbrüche abzufangen
                        'TODO: noch nicht fertig das Ergebnis wird noch nicht auf Fehler ueberprueft
                        Dim Eval_Count As Integer = 0
                        Dim SIM_Eval_is_OK As Boolean = True
                        Do
                            'REPRODUKTIONSPROZESS - Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern
                            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                            Call PES1.EsVaria()

                            'MUTATIONSPROZESS - Mutieren der Ausgangswerte
                            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
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

                                    'Evaluierung des Simulationsmodells (ToDo: Validätsprüfung fehlt)
                                    SIM_Eval_is_OK = Sim1.SIM_Evaluierung(QN, RN)

                                    'Lösung im TeeChart einzeichnen
                                    '==============================
                                    Dim serie As Steema.TeeChart.Styles.Series

                                    'Constraintverletzung prüfen
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
                                            serie = DForm.Diag.getSeriesPoint("Population " & PES1.PES_iAkt.iAktPop & " (ungültig)", "Gray")
                                        Else
                                            serie = DForm.Diag.getSeriesPoint("Population " & PES1.PES_iAkt.iAktPop, "Orange")
                                        End If
                                        Call serie.Add(durchlauf, QN(0))

                                    Else
                                        'MultiObjective
                                        'xxxxxxxxxxxxxx
                                        If (globalAnzZiel = 2) Then
                                            '2D-Diagramm
                                            '------------------------------------------------------------------------
                                            If (isInvalid) Then
                                                serie = DForm.Diag.getSeriesPoint("Population" & " (ungültig)", "Gray")
                                            Else
                                                serie = DForm.Diag.getSeriesPoint("Population", "Orange")
                                            End If
                                            Call serie.Add(QN(0), QN(1))

                                        Else
                                            '3D-Diagramm (Es werden die ersten drei Zielfunktionswerte eingezeichnet)
                                            '------------------------------------------------------------------------
                                            Dim serie3D As Steema.TeeChart.Styles.Points3D
                                            If (isInvalid) Then
                                                serie3D = DForm.Diag.getSeries3DPoint("Population" & " (ungültig)", "Gray")
                                            Else
                                                serie3D = DForm.Diag.getSeries3DPoint("Population", "Orange")
                                            End If
                                            Call serie3D.Add(QN(0), QN(1), QN(2))

                                        End If
                                    End If

                            End Select

                            Eval_Count += 1
                            If (Eval_Count >= 10) Then
                                Throw New Exception("Es konnte kein gültiger Datensatz erzeugt werden!")
                            End If

                        Loop While SIM_Eval_is_OK = False

                        'Einordnen der Qualitätsfunktion im Bestwertspeicher bei SO
                        'Falls MO Einordnen der Qualitätsfunktion in NDSorting
                        Call PES1.EsBest(QN, RN)

                        System.Windows.Forms.Application.DoEvents()

                    Next 'Ende Schleife über alle Nachkommen
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Die neuen Eltern werden generiert
                    Call PES1.EsEltern()

                    'Sekundäre Population 'BUG 135: SekPop(,) fängt bei 1 an!
                    If (EVO_Settings1.PES_Settings.is_MO_Pareto) Then
                        SekPopulation = PES1.EsGetSekundärePopulation()
                        'SekPop zeichnen
                        Call SekundärePopulationZeichnen(SekPopulation)
                        'SekPop in DB speichern
                        If (Not IsNothing(Sim1)) Then
                            If (Sim1.Ergebnisdb) Then
                                Call Sim1.db_setSekPop(SekPopulation, PES1.PES_iAkt.iAktGen)
                            End If
                        End If
                    End If

                    System.Windows.Forms.Application.DoEvents()

                Next 'Ende alle Generatione
                'xxxxxxxxxxxxxxxxxxxxxxxxxxx
                System.Windows.Forms.Application.DoEvents()

                'Einordnen der Qualitätsfunktion im PopulationsBestwertspeicher
                Call PES1.EsPopBest()

            Next 'Ende alle Populationen
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

            'Die neuen Populationseltern werden generiert
            Call PES1.EsPopEltern()
            System.Windows.Forms.Application.DoEvents()

        Next 'Ende alle Runden
        'xxxxxxxxxxxxxxxxxxxxx

    End Sub

    'Zeichenfunktionen
    'XXXXXXXXXXXXXXXXX

    'Sekundäre Population zeichnen
    '*****************************
    Private Sub SekundärePopulationZeichnen(ByVal SekPop(,) As Double)

        Dim i As Short
        Dim serie As Steema.TeeChart.Styles.Series

            'BUG 135: SekPop(,) fängt bei 1 an!
        If (UBound(SekPop, 2) = 2) Then
            '2 Zielfunktionen
            '----------------------------------------------------------------
            serie = DForm.Diag.getSeriesPoint("Sekundäre Population", "Green", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
            serie.Clear()
                For i = 1 To UBound(SekPop, 1)
                serie.Add(SekPop(i, 1), SekPop(i, 2), "")
                Next i

        ElseIf (UBound(SekPop, 2) >= 3) Then
            '3 oder mehr Zielfunktionen (es werden die ersten drei angezeigt)
            '----------------------------------------------------------------
            Dim serie3D As Steema.TeeChart.Styles.Points3D
            serie3D = DForm.Diag.getSeries3DPoint("Sekundäre Population", "Green")
            serie3D.Clear()
                For i = 1 To UBound(SekPop, 1)
                serie3D.Add(SekPop(i, 1), SekPop(i, 2), SekPop(i, 3))
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
                        'für jede Zielfunktion eine weitere Achse hinzufügen
                        'HACK: Diagramm-Achsen bisher nur für Anwendung BlueM!
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

                        'für jede Zielfunktion eine weitere Achse hinzufügen
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
                        Throw New Exception("Diese Funktion ist für die Anwendung '" & Anwendung & "' nicht vorgesehen")

                End Select

        End Select

        Call Application.DoEvents()

    End Sub

    'Klick auf Serie in Diagramm
    '***************************
    Public Sub showWave(ByVal sender As Object, ByVal s As Steema.TeeChart.Styles.Series, ByVal valueIndex As Integer, ByVal e As System.Windows.Forms.MouseEventArgs)

        'Notwendige Bedingungen überprüfen
        '---------------------------------
        If (IsNothing(Sim1)) Then
            'Anwendung != Sim
            MsgBox("Wave funktioniert nur bei Anwendungen BlueM oder SMUSI!", MsgBoxStyle.Information, "Info")
            Exit Sub

        ElseIf (Not Sim1.Ergebnisdb) Then
            'ErgebnisDB ist deaktiviert
            MsgBox("Wave funktioniert nur bei angeschlossener Ergebnisdatenbank!", MsgBoxStyle.Information, "Info")
            Exit Sub

        Else

            'Punkt-Informationen bestimmen
            '-----------------------------
            'X und Y Werte
            Dim xWert, yWert As Double
            xWert = s.XValues(valueIndex)
            yWert = s.YValues(valueIndex)
            'X und Y Achsen (Zielfunktionen)
            Dim xAchse, yAchse As String
            xAchse = Me.DForm.Diag.Chart.Axes.Bottom.Title.Caption
            yAchse = Me.DForm.Diag.Chart.Axes.Left.Title.Caption

            'Parametersatz aus der DB übernehmen
            '-----------------------------------
            Dim i As Integer
            Dim isOK As Boolean
            Dim res As MsgBoxResult
            Dim eol As String = Chr(13) & Chr(10)   'Zeilenumbruch
            Dim ParamString As String = ""          'String für die Anzeige der OptParameter / des Pfads

            isOK = Sim1.db_getPara(xAchse, xWert, yAchse, yWert)

            If (isOK) Then

                'Unterscheidung für die Methoden
                '-------------------------------
                Select Case Me.Method

                    Case METH_PES, METH_SENSIPLOT

                        'String für die Anzeige der OptParameter wird generiert
                        ParamString = eol & "OptParameter: " & eol
                        For i = 0 To Sim1.List_OptParameter.GetUpperBound(0)
                            With Sim1.List_OptParameter(i)
                                ParamString &= eol & .Bezeichnung & ": " & .Wert.ToString()
                            End With
                        Next


                    Case METH_CES

                        'String für die Anzeige der Pfade wird generiert
                        ParamString = eol & "Pfad: " & eol
                        For i = 0 To Sim1.Akt.Measures.GetUpperBound(0)
                            ParamString &= eol & Sim1.List_Locations(i).Name & ": " & Sim1.Akt.Measures(i).ToString()
                        Next


                    Case METH_CES_PES

                        'String für die Anzeige von Pfad/OptParameter wird generiert
                        ParamString = eol & "Pfad: " & eol
                        For i = 0 To Sim1.Akt.Measures.GetUpperBound(0)
                            ParamString &= eol & Sim1.List_Locations(i).Name & ": " & Sim1.Akt.Measures(i).ToString()
                        Next
                        ParamString &= eol & eol & "OptParameter: " & eol
                        For i = 0 To Sim1.List_OptParameter.GetUpperBound(0)
                            With Sim1.List_OptParameter(i)
                                ParamString &= eol & .Bezeichnung & ": " & .Wert.ToString()
                            End With
                        Next

                End Select

                'MessageBox
                res = MsgBox("Diesen Parametersatz simulieren?" & eol & ParamString, MsgBoxStyle.OkCancel, "Info")

                If (res = MsgBoxResult.Ok) Then

                    'Simulation ausführen
                    'xxxxxxxxxxxxxxxxxxxx

                    Dim SimSeries As New Collection                 'zu zeichnende Simulationsgrößen
                    Dim RefSeries As New Collection                 'zu zeichnende Referenzreihen
                    Dim QWertString As String                       'String für die Anzeige der QWerte
                    Dim ConstrString As String = ""                 'String für die Anzeige der Constraints

                    'Simulieren
                    Sim1.launchSim()

                    'Wave instanzieren
                    Dim Wave1 As New Wave.Wave

                    'QWerte berechnen, in String speichern und zugehörige Reihen anzeigen
                    '--------------------------------------------------------------------
                    QWertString = "QWerte: " & eol

                    'zu zeichnenden Reihen aus Liste der OptZiele raussuchen
                    For i = 0 To Sim1.List_OptZiele.GetUpperBound(0)

                        With Sim1.List_OptZiele(i)

                            'Qualitätswert berechnen und an String anhängen
                            .QWertTmp = Sim1.QWert(Sim1.List_OptZiele(i))
                            QWertString &= eol & .Bezeichnung & ": " & .QWertTmp.ToString()

                            'Name der WEL-Simulationsergebnisdatei
                            'BUG 171: Name der Ergebnisdatei
                            Dim WELFile As String = ""
                            If (Anwendung = ANW_BLUEM) Then
                                WELFile = Sim1.WorkDir & Sim1.Datensatz & ".WEL"
                            ElseIf (Anwendung = ANW_SMUSI) Then
                                WELFile = Sim1.WorkDir & .SimGr.Substring(0, 4) & "_WEL.ASC"
                            End If

                            'Simulationsgrößen nur jeweils ein Mal zeichnen
                            If (Not SimSeries.Contains(.SimGr)) Then
                                SimSeries.Add(.SimGr, .SimGr)
                                'Simulationsergebnis in Wave laden
                                Wave1.Import_WEL(WELFile, .SimGr)
                            End If

                            'ggf. Referenzreihe in Wave speichern
                            If (.ZielTyp = "Reihe" Or .ZielTyp = "IHA") Then
                                'Referenzreihen nur jeweils ein Mal zeichnen
                                If (Not RefSeries.Contains(.ZielReiheDatei & .ZielGr)) Then
                                    RefSeries.Add(.ZielGr, .ZielReiheDatei & .ZielGr)
                                    'Referenzreihe in Wave laden
                                    Wave1.Display_Series(.ZielReihe)
                                End If
                            End If

                        End With
                    Next

                    'Constraints berechnen und in String speichern
                    '---------------------------------------------
                    If (Sim1.List_Constraints.GetLength(0) > 0) Then
                        ConstrString = eol & eol & "Constraints: " & eol
                        For i = 0 To Sim1.List_Constraints.GetUpperBound(0)
                            With Sim1.List_Constraints(i)
                                .ConstTmp = Sim1.Constraint(Sim1.List_Constraints(i))
                                ConstrString &= eol & .Bezeichnung & ": " & .ConstTmp.ToString()
                            End With
                        Next
                    End If

                    'Annotation anzeigen
                    '-------------------
                    Dim anno1 As New Steema.TeeChart.Tools.Annotation(Wave1.TChart1.Chart)
                    anno1.Text = QWertString & eol & ParamString & ConstrString
                    anno1.Position = Steema.TeeChart.Tools.AnnotationPositions.LeftTop

                    'Wave anzeigen
                    '-------------
                    Call Wave1.Show()

                End If

            End If

        End If

    End Sub

    'Daten aus DB laden und als Scatterplot-Matrix anzeigen
    '*******************************************************
    Private Sub showScatterplot(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Scatterplot.Click

        Dim i As Integer
        Dim diagresult As DialogResult

        'Datei-öffnen Dialog anzeigen
        Me.OpenFileDialog_MDB.InitialDirectory = Sim1.WorkDir
        diagresult = Me.OpenFileDialog_MDB.ShowDialog()

        If (diagresult = Windows.Forms.DialogResult.OK) Then

            'Neuen DB-Pfad speichern
            Sim1.db_path = Me.OpenFileDialog_MDB.FileName

            'Abfrageform
            Dim Form2 As New ScatterplotAbfrage
            For Each OptZiel As Sim.Struct_OptZiel In Sim1.List_OptZiele
                Form2.ListBox_OptZieleX.Items.Add(OptZiel.Bezeichnung)
                Form2.ListBox_OptZieleY.Items.Add(OptZiel.Bezeichnung)
                Form2.ListBox_OptZieleZ.Items.Add(OptZiel.Bezeichnung)
            Next
            diagresult = Form2.ShowDialog()

            If (diagresult = Windows.Forms.DialogResult.OK) Then

                'Daten einlesen
                Cursor = Cursors.WaitCursor
                Dim OptResult As EVO.OptResult = Sim1.db_getOptResult(Form2.CheckBox_onlySekPop.Checked)
                Cursor = Cursors.Default

                If (Form2.CheckBox_Hauptdiagramm.Checked) Then
                    'Hauptdiagramm
                    '=============
                    Dim OptZielIndexX, OptZielIndexY, OptZielIndexZ As Integer
                    OptZielIndexX = Form2.ListBox_OptZieleX.SelectedIndex
                    OptZielIndexY = Form2.ListBox_OptZieleY.SelectedIndex
                    OptZielIndexZ = Form2.ListBox_OptZieleZ.SelectedIndex

                    'Achsen
                    '------
                    Dim Achsen As New Collection
                    Dim tmpAchse As EVO.Diagramm.Achse
                    tmpAchse.Auto = True
                    'X-Achse
                    tmpAchse.Name = Form2.ListBox_OptZieleX.SelectedItem
                    Achsen.Add(tmpAchse)
                    'Y-Achse
                    tmpAchse.Name = Form2.ListBox_OptZieleY.SelectedItem
                    Achsen.Add(tmpAchse)
                    If (Not OptZielIndexZ = -1) Then
                        'Z-Achse
                        tmpAchse.Name = Form2.ListBox_OptZieleZ.SelectedItem
                        Achsen.Add(tmpAchse)
                    End If

                    'Diagramm initialisieren
                    '-----------------------
                    Me.DForm.Diag.Clear()
                    Me.DForm.Diag.DiagInitialise(Path.GetFileName(Sim1.db_path), Achsen)

                    'Punkte eintragen
                    '----------------
                    Dim serie As Steema.TeeChart.Styles.Series
                    Dim serie3D As Steema.TeeChart.Styles.Points3D

                    For i = 0 To OptResult.Solutions.GetUpperBound(0)
                        With OptResult.Solutions(i)
                            If (OptZielIndexZ = -1) Then
                                '2D
                                '--
                                'Constraintverletzung prüfen
                                If (.isValid) Then
                                    serie = Me.DForm.Diag.getSeriesPoint("Population", "Orange")
                                Else
                                    serie = Me.DForm.Diag.getSeriesPoint("Population (ungültig)", "Gray")
                                End If
                                'Zeichnen
                                serie.Add(.QWerte(OptZielIndexX), .QWerte(OptZielIndexY))
                            Else
                                '3D
                                '--
                                'Constraintverletzung prüfen
                                If (.isValid) Then
                                    serie3D = Me.DForm.Diag.getSeries3DPoint("Population", "Orange")
                                Else
                                    serie3D = Me.DForm.Diag.getSeries3DPoint("Population (ungültig)", "Gray")
                                End If
                                'Zeichnen
                                serie3D.Add(.QWerte(OptZielIndexX), .QWerte(OptZielIndexY), .QWerte(OptZielIndexZ))
                            End If
                        End With
                    Next

                End If

                If (Form2.CheckBox_Scatterplot.Checked) Then
                    'Scatterplot
                    '-----------
                    Cursor = Cursors.WaitCursor
                    Dim scatterplot1 As New Scatterplot
                    Call scatterplot1.zeichnen(OptResult)
                    Call scatterplot1.Show()
                    Cursor = Cursors.Default
                End If

            End If

        End If

    End Sub

#End Region 'Diagrammfunktionen

    'Ermittelt die beim Start die Anzahl der Physikalischen Prozessoren
    '******************************************************************
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
