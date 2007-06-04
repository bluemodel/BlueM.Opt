Option Strict Off ' Off ist Default
Option Explicit On
Imports System.IO

'*******************************************************************************
'*******************************************************************************
'**** ihwb Optimierung                                                      ****
'****                                                                       ****
'**** Dirk Muschalla, Christoph Huebner, Felix Froehlich                    ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'****                                                                       ****
'**** Erstellt: Dezember 2003                                               ****
'****                                                                       ****
'**** Letzte Änderung: April 2007                                           ****
'*******************************************************************************
'*******************************************************************************

Partial Class Form1
    Inherits System.Windows.Forms.Form

#Region "Eigenschaften"

    Private IsInitializing As Boolean

    'Anwendung
    Private Anwendung As String
    Private Const ANW_BLUEM As String = "BlueM"
    Private Const ANW_SMUSI As String = "Smusi"
    Private Const ANW_TESTPROBLEME As String = "Testprobleme"
    Private Const ANW_TSP As String = "Traveling Salesman"

    'Optimierungsmethode
    Private Methode As String
    Private Const METH_RESET As String = "Reset"
    Private Const METH_PES As String = "PES"
    Private Const METH_CES As String = "CES"
    Private Const METH_CES_PES As String = "CES + PES"
    Private Const METH_SENSIPLOT As String = "SensiPlot"

    '**** Deklarationen der Module *****
    Public WithEvents Sim1 As Apps.Sim
    Public SensiPlot1 As Apps.SensiPlot
    Public CES1 As EvoKern.CES
    Public TSP1 As Apps.TSP

    '**** Globale Parameter Parameter Optimierung ****
    Dim myIsOK As Boolean
    Dim myisrun As Boolean
    Dim globalAnzPar As Short
    Dim globalAnzZiel_ParaOpt As Short
    Dim globalAnzRand As Short
    Dim array_x() As Double
    Dim array_y() As Double
    Dim Bestwert(,) As Double = {}
    Dim Population(,) As Double
    Dim mypara(,) As Double

#End Region 'Eigenschaften

#Region "Methoden"

    'Initialisierung von Form1
    '*************************
    Private Sub Form1_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        'XP-look
        System.Windows.Forms.Application.EnableVisualStyles()

        'Liste der Anwendungen in ComboBox schreiben und Anfangseinstellung wählen
        ComboBox_Anwendung.Items.AddRange(New Object() {"", ANW_BLUEM, ANW_SMUSI, ANW_TESTPROBLEME, ANW_TSP})
        ComboBox_Anwendung.SelectedIndex = 0

        'Liste der Methoden in ComboBox schreiben und Anfangseinstellung wählen
        ComboBox_Methode.Items.AddRange(New Object() {"", METH_RESET, METH_PES, METH_CES,METH_CES_PES, METH_SENSIPLOT})
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
    Private Sub IniApp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Anwendung.SelectedIndexChanged, Testprobleme1.Testproblem_Changed

        If (Me.IsInitializing = True) Then

            'Testprobleme deaktivieren
            Testprobleme1.Enabled = False
            Exit Sub

        Else

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Combobox Methode deaktivieren
            ComboBox_Methode.Enabled = False

            'Mauszeiger busy
            Cursor = System.Windows.Forms.Cursors.WaitCursor

            Me.Anwendung = ComboBox_Anwendung.SelectedItem

            Select Case Me.Anwendung

                Case "" 'Keine Anwendung ausgewählt
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Testprobleme deaktivieren
                    Testprobleme1.Enabled = False

                    'Mauszeiger wieder normal
                    Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub


                Case ANW_BLUEM 'Anwendung BlueM
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse BlueM initialisieren
                    Sim1 = New Apps.BlueM

                    'Initialisieren
                    Call Sim1.SimIni()

                    'Testprobleme deaktivieren
                    Testprobleme1.Enabled = False


                Case ANW_SMUSI 'Anwendung Smusi
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse BlueM initialisieren
                    Sim1 = New Apps.Smusi

                    'Initialisieren
                    Call Sim1.SimIni()

                    'Testprobleme deaktivieren
                    Testprobleme1.Enabled = False


                Case ANW_TESTPROBLEME 'Anwendung Testprobleme
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Test-Probleme aktivieren
                    Testprobleme1.Enabled = True

                    EVO_Einstellungen1.OptModus = Testprobleme1.OptModus

                    'Globale Parameter werden gesetzt
                    Call Testprobleme1.Parameter_Uebergabe(Testprobleme1.Combo_Testproblem.Text, Testprobleme1.Text_Sinusfunktion_Par.Text, Testprobleme1.Text_Schwefel24_Par.Text, globalAnzPar, globalAnzZiel_ParaOpt, globalAnzRand, mypara)

                    'Start-Button aktivieren (keine Methodenauswahl erforderlich)
                    Button_Start.Enabled = True


                Case ANW_TSP 'Anwendung Traveling Salesman Problem (TSP)
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    TSP1 = New Apps.TSP

                    Call TSP1.TSP_Initialize(DForm.Diag)

                    'Start-Button aktivieren (keine Methodenauswahl erforderlich)
                    Button_Start.Enabled = True

            End Select

            'Mauszeiger wieder normal
            Cursor = System.Windows.Forms.Cursors.Default

            'Combobox Methode aktivieren
            If (Not Anwendung = ANW_TESTPROBLEME and Not Anwendung = ANW_TSP) Then
                ComboBox_Methode.Enabled = True
            End If

        End If

    End Sub
    
    'Methode wurde ausgewählt
    '************************
    Private Sub IniMethod( ByVal sender As System.Object,  ByVal e As System.EventArgs) Handles Button_IniApp.Click, ComboBox_Methode.SelectedIndexChanged

        If (Me.IsInitializing = True) Then

            'EVO_Einstellungen deaktivieren
            EVO_Einstellungen1.Enabled = False
            Exit Sub

        Else

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Mauszeiger busy
            Cursor = System.Windows.Forms.Cursors.WaitCursor

            Me.Methode = ComboBox_Methode.SelectedItem

            Select Case Me.Methode

                Case "" 'Keine Methode ausgewählt
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'EVO_Einstellungen deaktivieren
                    EVO_Einstellungen1.Enabled = False

                    'Mauszeiger wieder normal
                    Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub


                Case METH_RESET 'Methode Reset
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'EVO_Einstellungen deaktivieren
                    EVO_Einstellungen1.Enabled = False

                    'Ergebnisdatenbank ausschalten
                    Sim1.Ergebnisdb = False

                    'Original ModellParameter schreiben
                    Call Sim1.ModellParameter_schreiben()

                    MsgBox("Die Startwerte der Optimierungsparameter wurden in die Eingabedateien geschrieben.", MsgBoxStyle.Information, "Info")


                Case METH_SENSIPLOT 'Methode SensiPlot
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    SensiPlot1 = New Apps.SensiPlot

                    'EVO_Einstellungen deaktivieren
                    EVO_Einstellungen1.Enabled = False

                    'Ergebnisdatenbank ausschalten
                    Sim1.Ergebnisdb = False

                    'SensiPlot für Sim vorbereiten
                    Call Sim1.prepare_Sim_PES()

                    'SensiPlot Dialog anzeigen:
                    '--------------------------
                    'List_Boxen füllen
                    Dim i As Integer
                    For i = 0 To Sim1.OptParameterListe.GetUpperBound(0)
                        Call SensiPlot1.ListBox_OptParameter_add(Sim1.OptParameterListe(i))
                    Next
                    For i = 0 To Sim1.OptZieleListe.GetUpperBound(0)
                        Call SensiPlot1.ListBox_OptZiele_add(Sim1.OptZieleListe(i))
                    Next
                    'Dialog anzeigen
                    Dim SensiPlotDiagResult As Windows.Forms.DialogResult
                    SensiPlotDiagResult = SensiPlot1.ShowDialog()
                    If (Not SensiPlotDiagResult = Windows.Forms.DialogResult.OK) Then
                        'Mauszeiger wieder normal
                        Cursor = System.Windows.Forms.Cursors.Default
                        Exit Sub
                    End If


                Case METH_PES 'Methode PES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'EVO_Einstellungen aktivieren
                    EVO_Einstellungen1.Enabled = True

                    'PES für Sim vorbereiten
                    Call Sim1.prepare_Sim_PES()

                    'Je nach Anzahl der Zielfunktionen von MO auf SO umschalten
                    If (Sim1.OptZieleListe.GetLength(0) = 1) Then
                        EVO_Einstellungen1.OptModus = 0
                    ElseIf (Sim1.OptZieleListe.GetLength(0) > 1) Then
                        EVO_Einstellungen1.OptModus = 1
                    End If

                    'Parameterübergabe an ES
                    Call Sim1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel_ParaOpt, globalAnzRand, mypara)


                Case METH_CES 'Methode CES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Funktioniert nur bei BlueM!
                    If (Not Anwendung = ANW_BLUEM) Then
                        Throw New Exception("CES funktioniert bisher nur mit BlueM!")
                    End If

                    'EVO_Einstellungen deaktiviern
                    EVO_Einstellungen1.Enabled = False

                    'Ergebnisdatenbank ausschalten
                    Sim1.Ergebnisdb = False

                    'CES für Sim vorbereiten
                    Call Sim1.prepare_Sim_CES()

                    CES1 = New EvoKern.CES

                    'Je nach Anzahl der Zielfunktionen von MO auf SO umschalten
                    If (Sim1.OptZieleListe.GetLength(0) = 1) Then
                        EVO_Einstellungen1.OptModus = 0
                    ElseIf (Sim1.OptZieleListe.GetLength(0) > 1) Then
                        EVO_Einstellungen1.OptModus = 1
                    End If

                    'Überprüfen der Kombinatorik
                    Call Sim1.Combinatoric_is_Valid()

                    'Prüfen ob Kombinatorik und Verzweigungsdatei zusammenpassen
                    Call Sim1.CES_fits_to_VER()

                    'Anzahl der Ziele, Locations und Verzeigungen wird an CES übergeben
                    CES1.n_Penalty = Sim1.OptZieleListe.GetLength(0)
                    CES1.n_Location = Sim1.LocationList.GetLength(0)
                    CES1.n_Verzweig = Sim1.VerzweigungsDatei.GetLength(0)

                    'Gibt die PathSize an für jede Pfadstelle
                    Dim i As Integer
                    ReDim CES1.n_PathDimension(CES1.n_Location - 1)
                    For i = 0 To CES1.n_Location - 1
                        CES1.n_PathDimension(i) = Sim1.LocationList(i).MassnahmeListe.GetLength(0)
                    Next

                Case METH_CES_PES 'Methode CES + PES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Funktioniert nur bei BlueM!
                    If (Not Anwendung = ANW_BLUEM) Then
                        Throw New Exception("CES funktioniert bisher nur mit BlueM!")
                    End If

                    'EVO_Einstellungen aktiviern
                    EVO_Einstellungen1.Enabled = True

                    'Ergebnisdatenbank ausschalten
                    Sim1.Ergebnisdb = False

                    'CES + PES für Sim vorbereiten
                    Call Sim1.prepare_Sim_PES()
                    Call Sim1.prepare_Sim_CES()

                    CES1 = New EvoKern.CES

                    'Je nach Anzahl der Zielfunktionen von MO auf SO umschalten
                    If (Sim1.OptZieleListe.GetLength(0) = 1) Then
                        EVO_Einstellungen1.OptModus = 0
                    ElseIf (Sim1.OptZieleListe.GetLength(0) > 1) Then
                        EVO_Einstellungen1.OptModus = 1
                    End If

                    'Überprüfen der Kombinatorik
                    Call Sim1.Combinatoric_is_Valid()
                    'Prüfen ob Kombinatorik und Verzweigungsdatei zusammenpassen
                    Call Sim1.CES_fits_to_VER()

                    'Anzahl der Ziele, Locations und Verzeigungen wird an CES übergeben
                    CES1.n_Penalty = Sim1.OptZieleListe.GetLength(0)
                    CES1.n_Location = Sim1.LocationList.GetLength(0)
                    CES1.n_Verzweig = Sim1.VerzweigungsDatei.GetLength(0)

                    'Gibt die PathSize an für jede Pfadstelle
                    Dim i As Integer
                    ReDim CES1.n_PathDimension(CES1.n_Location - 1)
                    For i = 0 To CES1.n_Location - 1
                        CES1.n_PathDimension(i) = Sim1.LocationList(i).MassnahmeListe.GetLength(0)
                    Next

                    'Parameterübergabe an ES
                    Call Sim1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel_ParaOpt, globalAnzRand, mypara)

            End Select

            'IniApp OK -> Start Button aktivieren
            Me.Button_Start.Enabled = True

            'Mauszeiger wieder normal
            Cursor = System.Windows.Forms.Cursors.Default

        End If

    End Sub

    'Arbeitsverzeichnis/Datensatz ändern
    '***********************************
    Private Sub changeDatensatz(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel_WorkDir.LinkClicked

        'Aktuelles Verzeichnis auslesen
        Dim currentDir As String = CurDir()
        'Alten Datensatz dem Dialog zuweisen
        OpenFileDialog_Datensatz.FileName = Sim1.WorkDir & Sim1.Datensatz & ".ALL"
        'Dialog öffnen
        Dim DatensatzResult As DialogResult = OpenFileDialog_Datensatz.ShowDialog()
        'Neuen Datensatz speichern
        If (DatensatzResult = Windows.Forms.DialogResult.OK) Then
            Call Sim1.saveDatensatz(OpenFileDialog_Datensatz.FileName)
        End If
        'Wieder ins Standardverzeichnis zurückwechseln
        ChDir(currentDir)
        'Startbutton deaktivieren (Der Benutzer muss zuerst Ini neu ausführen!)
        Me.Button_Start.Enabled = False

    End Sub

    'Arbeitsverzeichnis wurde geändert -> Anzeige aktualisieren
    '**********************************************************
    Private Sub displayWorkDir() Handles Sim1.WorkDirChange
        'Datensatzanzeige aktualisieren
        Me.LinkLabel_WorkDir.Text = Sim1.WorkDir & Sim1.Datensatz & ".ALL"
        Me.LinkLabel_WorkDir.Links(0).LinkData = Sim1.WorkDir
    End Sub

#End Region 'Initialisierung der Anwendungen

#Region "Start Button Pressed"

    'Start BUTTON wurde pressed
    'XXXXXXXXXXXXXXXXXXXXXXXXXX

    Private Sub STARTEN_Button_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Button_Start.Click

        'Try
        myisrun = True
        Select Case Anwendung

            Case ANW_BLUEM, ANW_SMUSI

                Select Case Methode
                    Case METH_RESET
                        Call Sim1.launchSim()
                    Case METH_SENSIPLOT
                        Call STARTEN_SensiPlot()
                    Case METH_PES
                        Call STARTEN_PES()
                    Case METH_CES
                        Call STARTEN_CES()
                    Case METH_CES_PES
                        Call STARTEN_CES_PES()
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
    End Sub

    'Anwendung SensiPlot - START; läuft ohne Evolutionsstrategie             
    '***********************************************************
    Private Sub STARTEN_SensiPlot()

        'Einschränkung:
        '------------------------------------------------------------------------
        'Die Modellparameter werden auch für die nicht ausgewählten OptParameter 
        'geschrieben, und zwar mit den in der OPT-Datei angegebenen Startwerten
        '------------------------------------------------------------------------

        'Wave deklarieren
        Dim Wave1 As New Main.Wave
        ReDim Wave1.WaveList(SensiPlot1.Anz_Sim - 1)

        'Parameterübergabe an ES
        Me.globalAnzZiel_ParaOpt = 1
        Me.globalAnzRand = 0
        Me.globalAnzPar = 1

        Dim i As Integer
        Dim durchlauf As Integer = 1
        Dim ipop As Integer = 1

        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        'Simulationsschleife
        '-------------------
        Randomize()

        For i = 0 To SensiPlot1.Anz_Sim - 1

            'OptParameterwert variieren
            Select Case SensiPlot1.Selected_SensiType
                Case "Gleichverteilt"
                    Sim1.OptParameterListe(SensiPlot1.Selected_OptParameter).SKWert = Rnd()
                Case "Diskret"
                    Sim1.OptParameterListe(SensiPlot1.Selected_OptParameter).SKWert = i / SensiPlot1.Anz_Sim
            End Select

            'Modellparameter schreiben
            Call Sim1.ModellParameter_schreiben()

            'Simulieren
            Call Sim1.launchSim()

            'Qwert berechnen
            Sim1.OptZieleListe(SensiPlot1.Selected_OptZiel).QWertTmp = Sim1.QWert(Sim1.OptZieleListe(SensiPlot1.Selected_OptZiel))

            'Diagramm aktualisieren
            DForm.Diag.Series(0).Add(Sim1.OptZieleListe(SensiPlot1.Selected_OptZiel).QWertTmp, Sim1.OptParameterListe(SensiPlot1.Selected_OptParameter).Wert, "")

            'Speichern des Simulationsergebnisses für Wave
            'BUG 119: Die WEL-Datei hat bei Smusi einen anderen Namen!
            Apps.Sim.Read_WEL(Sim1.WorkDir & Sim1.Datensatz & ".wel", Sim1.OptZieleListe(SensiPlot1.Selected_OptZiel).SimGr, Wave1.WaveList(i).Wave)

            durchlauf += 1

            System.Windows.Forms.Application.DoEvents()

        Next

        'Wave Diagramm anzeigen:
        '-----------------------
        'Achsen:
        Dim Achsen As New Collection
        Dim xAchse, yAchse As Diagramm.Achse
        xAchse.Name = "Zeit"
        xAchse.Auto = True
        Achsen.Add(xAchse)
        yAchse.Name = Sim1.OptZieleListe(SensiPlot1.Selected_OptZiel).SimGr
        yAchse.Auto = True
        Achsen.Add(yAchse)

        Call Wave1.WForm.Diag.DiagInitialise("SensiPlot", Achsen)

        'Serien initialisieren
        Dim tmpSeries As Steema.TeeChart.Styles.Line
        For i = 1 To SensiPlot1.Anz_Sim
            tmpSeries = New Steema.TeeChart.Styles.Line(Wave1.WForm.Diag.Chart)
            tmpSeries.Title = "Sim " & i.ToString()
            tmpSeries.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Nothing
        Next

        'Serien zeichnen
        Call Wave1.Wave_draw()

        'Dialog anzeigen
        Call Wave1.Show()

    End Sub


    'Anwendung Traveling Salesman - Start                         
    '************************************
    Private Sub STARTEN_TSP()

        'Laufvariable für die Generationen
        Dim gen As Integer

        'BUG 85: Nach Klasse Diagramm auslagern!
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
            Call TSP1.Reset_Childs_TSP()

            'Reproduktionsoperatoren, hier gehts dezent zur Sache
            Call TSP1.Reproduction_Control()

            'Mutationsoperatoren
            Call TSP1.Mutation_Control()

        Next gen

    End Sub

    'Anwendung CombiBM - START; läuft ohne Evolutionsstrategie             
    '*********************************************************
    Private Sub STARTEN_CES()

        'Fehlerabfragen
        If (Sim1.OptZieleListe.GetLength(0) > 2) Then
            Throw New Exception("Zu viele Ziele für CES. Max=2")
        End If

        Dim durchlauf_all As Integer = 0

        'Laufvariable für die Generationen
        Dim gen As Integer
        Dim i As Integer

        'Parents und Child werden Dimensioniert
        Call CES1.Dim_Parents()
        Call CES1.Dim_Childs()

        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        'Zufällige Kinderpfade werden generiert
        Call CES1.Generate_Random_Path()

        'HACK: Funktion zum manuellen testen aller Kombinationen
        'Call CES1.Generate_All_Test_Path()

        'Generationsschleife
        For gen = 0 To CES1.n_Generation - 1

            'Child Schleife
            For i = 0 To CES1.n_Childs - 1
                durchlauf_all += 1

                'Vorbereitung und Evaluierung des Blauen Modells
                '***********************************************
                Call Sim1.Sim_Prepare(CES1.List_Childs(i).Path)
                Call Sim1.Sim_Evaluierung_CombiOpt(CES1.n_Penalty, CES1.List_Childs(i).Penalty)
                '***********************************************

                'Zeichnen MO_SO
                Call DForm.Diag.prepareSeries(0, "Childs", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
                If CES1.n_Penalty = 1 Then
                    Call DForm.Diag.Series(0).Add(durchlauf_all, CES1.List_Childs(i).Penalty(0))
                ElseIf CES1.n_Penalty = 2 Then
                    Call DForm.Diag.Series(0).Add(CES1.List_Childs(i).Penalty(0), CES1.List_Childs(i).Penalty(1))
                End If

                ''HACK zum zeichnen aller Qualitäten
                'Call TChart1.Series(2).Add(durchlauf, CES1.ChildList_BM(i).Quality_MO(0))
                'Call TChart1.Series(3).Add(durchlauf, CES1.ChildList_BM(i).Quality_MO(1))
                'Call TChart1.Series(4).Add(durchlauf, CES1.ChildList_BM(i).Quality_MO(2))
                System.Windows.Forms.Application.DoEvents()
            Next

            'MO oder SO
            '----------
            If CES1.n_Penalty = 1 Then
                'Sortieren der Kinden anhand der Qualität
                Call CES1.Sort_Faksimile(CES1.List_Childs)
                'Selectionsprozess je nach "plus" oder "minus" Strategie
                Call CES1.Selection_Process()
                'Zeichnen des besten Elter
                For i = 0 To CES1.n_Parents - 1
                    'durchlauf += 1
                    Call DForm.Diag.prepareSeries(1, "Parent", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                    Call DForm.Diag.Series(1).Add(durchlauf_all, CES1.List_Parents(i).Penalty(0))
                Next

            ElseIf CES1.n_Penalty = 2 Then
                'NDSorting ******************
                Call CES1.NDSorting_Control()
                'Zeichnen von NDSortingResult
                Call DForm.Diag.DeleteSeries(CES1.n_Childs - 1, 1)
                Dim f As Integer
                For i = 0 To CES1.n_Childs - 1
                    f = CES1.NDSResult(i).Front
                    Call DForm.Diag.prepareSeries(f, "Front:" & f, Steema.TeeChart.Styles.PointerStyles.Circle, 4)
                    Call DForm.Diag.Series(f).Add(CES1.NDSResult(i).Penalty(0), CES1.NDSResult(i).Penalty(1))
                Next
            End If

            'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
            Call CES1.Reset_Childs()
            'Reproduktionsoperatoren, hier gehts dezent zur Sache
            Call CES1.Reproduction_Control()
            'Mutationsoperatoren
            Call CES1.Mutation_Control()

        Next

    End Sub

    'Anwendung BlueM mit CES und PES - START             
    '***************************************
    Private Sub STARTEN_CES_PES()

        'Fehlerabfragen
        If (Sim1.OptZieleListe.GetLength(0) > 2) Then
            Throw New Exception("Zu viele Ziele für CES. Max=2")
        End If

        Dim durchlauf_all As Integer = 0

        'Laufvariable für die Generationen
        Dim gen As Integer
        Dim i As Integer

        'Parents und Child werden Dimensioniert
        Call CES1.Dim_Parents()
        Call CES1.Dim_Childs()

        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        'Zufällige Kinderpfade werden generiert
        Call CES1.Generate_Random_Path()

        'HACK: Funktion zum manuellen testen aller Kombinationen
        'Call CES1.Generate_All_Test_Path()

        'Generationsschleife für CES
        'xxxxxxxxxxxxxxxxxxxxxxxxxxx
        For gen = 0 To CES1.n_Generation - 1

            'Child Schleife
            For i = 0 To CES1.n_Childs - 1
                durchlauf_all += 1

                'Vorbereitung und Evaluierung des Blauen Modells
                '***********************************************
                Call Sim1.Sim_Prepare(CES1.List_Childs(i).Path)
                Call Sim1.Sim_Evaluierung_CombiOpt(CES1.n_Penalty, CES1.List_Childs(i).Penalty)
                '******************************************

                'Zeichnen MO_SO
                Call DForm.Diag.prepareSeries(0, "Childs", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
                If CES1.n_Penalty = 1 Then
                    Call DForm.Diag.Series(0).Add(durchlauf_all, CES1.List_Childs(i).Penalty(0))
                ElseIf CES1.n_Penalty = 2 Then
                    Call DForm.Diag.Series(0).Add(CES1.List_Childs(i).Penalty(0), CES1.List_Childs(i).Penalty(1))
                End If

                ''HACK zum zeichnen aller Qualitäten
                'Call TChart1.Series(2).Add(durchlauf, CES1.ChildList_BM(i).Quality_MO(0))
                'Call TChart1.Series(3).Add(durchlauf, CES1.ChildList_BM(i).Quality_MO(1))
                'Call TChart1.Series(4).Add(durchlauf, CES1.ChildList_BM(i).Quality_MO(2))
                System.Windows.Forms.Application.DoEvents()
            Next

            'MO oder SO
            '----------
            If CES1.n_Penalty = 1 Then
                'Sortieren der Kinden anhand der Qualität
                Call CES1.Sort_Faksimile(CES1.List_Childs)
                'Selectionsprozess je nach "plus" oder "minus" Strategie
                Call CES1.Selection_Process()
                'Zeichnen des besten Elter
                For i = 0 To CES1.n_Parents - 1
                    'durchlauf += 1
                    Call DForm.Diag.prepareSeries(1, "Parent", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
                    Call DForm.Diag.Series(1).Add(durchlauf_all, CES1.List_Parents(i).Penalty(0))
                Next

            ElseIf CES1.n_Penalty = 2 Then
                'NDSorting ******************
                Call CES1.NDSorting_Control()
                'Zeichnen von NDSortingResult
                Call DForm.Diag.DeleteSeries(CES1.n_Childs - 1, 1)
                Dim f As Integer
                For i = 0 To CES1.n_Childs - 1
                    f = CES1.NDSResult(i).Front
                    Call DForm.Diag.prepareSeries(f, "Front:" & f, Steema.TeeChart.Styles.PointerStyles.Circle, 4)
                    Call DForm.Diag.Series(f).Add(CES1.NDSResult(i).Penalty(0), CES1.NDSResult(i).Penalty(1))
                Next
            End If

            'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
            Call CES1.Reset_Childs()
            'Reproduktionsoperatoren, hier gehts dezent zur Sache
            Call CES1.Reproduction_Control()
            'Mutationsoperatoren
            Call CES1.Mutation_Control()

        Next
        'Ende der Generationsschleife

        'Starten der PES mit der Front von CES
        '(MaxAnzahl ist die Zahl der Eltern -> ToDo: SecPop oder Bestwertspeicher)
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        'Einstellungen für PES werden gesetzt
        Call evo_einstellungen1.SetFor_CES_PES()

        For i = 0 To CES1.n_Parents - 1
            If CES1.List_Parents(i).Front = 1 Then
                'Bereitet das BlaueModell für die Kombinatorik ein
                '*************************************************
                Call Sim1.Sim_Prepare(CES1.List_Parents(i).Path)

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
        Dim PES1 As EvoKern.PES
        '--------------------------
        'Variablen für Optionen Evostrategie
        Dim iEvoTyp, iPopEvoTyp As Integer
        Dim isPOPUL As Boolean
        Dim isMultiObjective, isPareto, isPareto3D As Boolean
        Dim NPopEltern, NRunden, NPopul, iOptPopEltern As Integer
        Dim iOptEltern, iPopPenalty As Integer
        Dim NEltern As Integer
        Dim NRekombXY As Integer
        Dim rDeltaStart As Single
        Dim iStartPar As Integer
        Dim isdnvektor As Boolean
        Dim NGen, NNachf As Integer
        Dim Interact As Short
        Dim isInteract As Boolean
        Dim NMemberSecondPop As Short
        '--------------------------
        Dim ipop As Short = 0
        Dim igen As Short
        Dim inachf As Short
        Dim irunde As Short
        Dim QN() As Double = {}
        Dim RN() As Double = {}
        '--------------------------

        'TODO: If (ipop + igen + inachf + irunde) > 4 Then GoTo Start_Evolutionsrunden
        '????? Wie?

        myisrun = True

        'Werte an Variablen übergeben
        iEvoTyp = EVO_Einstellungen1.iEvoTyp
        iPopEvoTyp = EVO_Einstellungen1.iPopEvoTyp
        isPOPUL = EVO_Einstellungen1.isPOPUL
        isMultiObjective = EVO_Einstellungen1.isMultiObjective
        isPareto = EVO_Einstellungen1.isPareto
        isPareto3D = False
        NRunden = EVO_Einstellungen1.NRunden
        NPopul = EVO_Einstellungen1.NPopul
        NPopEltern = EVO_Einstellungen1.NPopEltern
        iOptPopEltern = EVO_Einstellungen1.iOptPopEltern
        iOptEltern = EVO_Einstellungen1.iOptEltern
        iPopPenalty = EVO_Einstellungen1.iPopPenalty
        NGen = EVO_Einstellungen1.NGen
        NEltern = EVO_Einstellungen1.NEltern
        NNachf = EVO_Einstellungen1.NNachf
        NRekombXY = EVO_Einstellungen1.NRekombXY
        rDeltaStart = EVO_Einstellungen1.rDeltaStart
        isdnvektor = EVO_Einstellungen1.isDnVektor
        iStartPar = EVO_Einstellungen1.globalOPTVORGABE
        Interact = EVO_Einstellungen1.Interact
        isInteract = EVO_Einstellungen1.isInteract
        NMemberSecondPop = EVO_Einstellungen1.NMemberSecondPop

        ReDim QN(globalAnzZiel_ParaOpt)
        ReDim RN(globalAnzRand)

        'Kontrolle der Variablen
        If NRunden = 0 Or NPopul = 0 Or NPopEltern = 0 Then
            Throw New Exception("Anzahl der Runden, Populationen oder Populationseltern ist zu klein!")
        End If
        If NGen = 0 Or NEltern = 0 Or NNachf = 0 Then
            Throw New Exception("Anzahl der Generationen, Eltern oder Nachfolger ist zu klein!")
        End If
        If rDeltaStart < 0 Then
            Throw New Exception("Die Startschrittweite ist unzulässig oder kleiner als die minimale Schrittweite!")
        End If
        If globalAnzPar = 0 Then
            Throw New Exception("Die Anzahl der Parameter ist unzulässig!")
        End If
        If NPopul < NPopEltern Then
            Throw New Exception("Die Anzahl der Populationseltern darf nicht größer als die Anzahl der Populationen sein!")
        End If

        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        '1. Schritt: PES
        'Objekt der Klasse PES wird erzeugt
        '******************************************************************************************
        PES1 = New EvoKern.PES

        '2. Schritt: PES - ES_INI
        'Die öffentlichen dynamischen Arrays werden initialisiert (Dn, An, Xn, Xmin, Xmax)
        'und die Anzahl der Zielfunktionen wird festgelegt
        '******************************************************************************************
        myIsOK = PES1.EsIni(globalAnzPar, globalAnzZiel_ParaOpt, globalAnzRand)

        '3. Schritt: PES - ES_OPTIONS
        'Optionen der Evolutionsstrategie werden übergeben
        '******************************************************************************************
        myIsOK = PES1.EsOptions(iEvoTyp, iPopEvoTyp, isPOPUL, NRunden, NPopul, NPopEltern, iOptPopEltern, iOptEltern, iPopPenalty, NGen, NEltern, NNachf, NRekombXY, rDeltaStart, iStartPar, isdnvektor, isMultiObjective, isPareto, isPareto3D, Interact, isInteract, NMemberSecondPop)

        '4. Schritt: PES - ES_LET_PARAMETER
        'Ausgangsparameter werden übergeben
        '******************************************************************************************
        For i = 1 To globalAnzPar
            myIsOK = PES1.EsLetParameter(i, mypara(i, 1))
        Next i

        '5. Schritt: PES - ES_PREPARE
        'Interne Variablen werden initialisiert, Zufallsgenerator wird initialisiert
        '******************************************************************************************
        myIsOK = PES1.EsPrepare()

        '6. Schritt: PES - ES_STARTVALUES
        'Startwerte werden zugewiesen
        '******************************************************************************************
        myIsOK = PES1.EsStartvalues()

        'Startwerte werden der Bedienoberfläche zugewiesen
        '******************************************************************************************
        EVO_Opt_Verlauf1.NRunden = PES1.NRunden
        EVO_Opt_Verlauf1.NPopul = PES1.NPopul
        EVO_Opt_Verlauf1.NGen = PES1.NGen
        EVO_Opt_Verlauf1.NNachf = PES1.NNachf
        EVO_Opt_Verlauf1.Initialisieren()

        durchlauf = 0

Start_Evolutionsrunden:
        'Cursor setzen
        'System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        'Loop über alle Runden
        '*******************************************************************************************
        Do While (PES1.EsIsNextRunde)

            irunde = PES1.iaktuelleRunde
            Call EVO_Opt_Verlauf1.Runden(irunde)

            myIsOK = PES1.EsPopBestwertspeicher()
            'Loop über alle Populationen
            '***********************************************************************************************
            Do While (PES1.EsIsNextPop)

                ipop = PES1.iaktuellePopulation
                Call EVO_Opt_Verlauf1.Populationen(ipop)

                myIsOK = PES1.EsPopVaria

                myIsOK = PES1.EsPopMutation

                'TODO: Scheint mir Schwachsinnig an dieser Stelle Weil es überschrieben wird
                durchlauf = NGen * NNachf * (irunde - 1)

                'Loop über alle Generationen
                '***********************************************************************************************
                Do While (PES1.EsIsNextGen)

                    igen = PES1.iaktuelleGeneration
                    Call EVO_Opt_Verlauf1.Generation(igen)

                    myIsOK = PES1.EsBestwertspeicher()

                    'Loop über alle Nachkommen
                    '********************************************************************
                    Do While (PES1.EsIsNextNachf)

                        inachf = PES1.iaktuellerNachfahre
                        Call EVO_Opt_Verlauf1.Nachfolger(inachf)

                        durchlauf = durchlauf + 1

                        'Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern
                        myIsOK = PES1.EsVaria

                        'Mutieren der Ausgangswerte
                        myIsOK = PES1.EsMutation

                        'Auslesen der Variierten Parameter
                        myIsOK = PES1.EsGetParameter(globalAnzPar, mypara)

                        'Auslesen des Bestwertspeichers
                        If Not PES1.isMultiObjective Then
                            myIsOK = PES1.EsGetBestwert(Bestwert)
                        End If

                        '************************************************************************************
                        '******************* Ansteuerung der zu optimierenden Anwendung *********************
                        '************************************************************************************
                        Select Case Anwendung
                            Case ANW_TESTPROBLEME
                                Call Testprobleme1.Evaluierung_TestProbleme(Testprobleme1.Combo_Testproblem.Text, globalAnzPar, mypara, durchlauf, ipop, QN, RN, DForm.Diag)
                            Case ANW_BLUEM, ANW_SMUSI
                                Call Sim1.Eval_Sim_ParaOpt(globalAnzPar, globalAnzZiel_ParaOpt, mypara, durchlauf, ipop, QN, DForm.Diag)
                        End Select

                        'Einordnen der Qualitätsfunktion im Bestwertspeicher
                        '**************************************************************************
                        myIsOK = PES1.EsBest(QN, RN)

                        System.Windows.Forms.Application.DoEvents()

                        'Ende Loop über alle Nachkommen
                        '**************************************************************************
                    Loop


                    'Die neuen Eltern werden generiert
                    myIsOK = PES1.EsEltern()

                    'Bestwerte und sekundäre Population
                    If PES1.isMultiObjective Then
                        myIsOK = PES1.EsGetBestwert(Bestwert)
                        'TODO: Call Bestwertzeichnen_Pareto(Bestwert, ipop)
                        myIsOK = PES1.esGetSekundärePopulation(Population)
                        Call SekundärePopulationZeichnen(Population)
                    End If

                    System.Windows.Forms.Application.DoEvents()

                    'Ende Loop über alle Generationen
                    '***********************************************************************************************
                Loop 'Schleife über alle Generationen

                System.Windows.Forms.Application.DoEvents()

                'Einordnen der Qualitätsfunktion im PopulationsBestwertspeicher
                myIsOK = PES1.EsPopBest()

                'Ende Loop über alle Populationen
                '***********************************************************************************************
            Loop 'Schleife über alle Populationen



            'Die neuen Populationseltern werden generiert
            myIsOK = PES1.EsPopEltern

            System.Windows.Forms.Application.DoEvents()

            'Ende Loop über alle Runden
            '***********************************************************************************************
        Loop 'Schleife über alle Runden

        'PES, letzter. Schritt
        'Objekt der Klasse PES wird vernichtet
        '***************************************************************************************************
        'UPGRADE_NOTE: Das Objekt PES1 kann erst dann gelöscht werden, wenn die Garbagecollection durchgeführt wurde. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1029"'
        'TODO: Ersetzen durch dispose funzt net
        PES1 = Nothing

    End Sub


    'Zeichenfunktionen
    'XXXXXXXXXXXXXXXXX

    Private Sub Bestwertzeichnen_Pareto(ByRef Bestwert(,) As Double, ByRef ipop As Short)
        Dim i As Short
        With DForm.Diag
            .Series(ipop).Clear()
            If UBound(Bestwert, 2) = 2 Then
                For i = 1 To UBound(Bestwert, 1)
                    .Series(ipop).Add(Bestwert(i, 1), Bestwert(i, 2), "")
                Next i
            ElseIf UBound(Bestwert, 2) = 3 Then
                For i = 1 To UBound(Bestwert, 1)
                    'TODO: Hier muss eine 3D-Punkt angezeigt werden
                    '.Series(ipop).Add(Bestwert(i, 1), Bestwert(i, 2), Bestwert(i, 2), "")
                Next i
            End If
        End With
    End Sub

    Private Sub SekundärePopulationZeichnen(ByRef Population(,) As Double)
        Dim i As Short
        Dim Datenreihe As Short
        With DForm.Diag
            If EVO_Einstellungen1.isPOPUL Then
                Datenreihe = EVO_Einstellungen1.NPopul + 1
            Else
                Datenreihe = 1
            End If
            .Series(Datenreihe).Clear()
            If UBound(Population, 2) = 2 Then
                For i = 1 To UBound(Population, 1)
                    .Series(Datenreihe).Add(Population(i, 1), Population(i, 2), "")
                Next i
            ElseIf UBound(Population, 2) = 3 Then
                For i = 1 To UBound(Population, 1)
                    'TODO: Hier muss eine 3D-Reihe angezeigt werden
                    '.Series(Datenreihe).Add(Population(i, 1), Population(i, 2), Population(i, 3), "") 
                Next i
            End If
        End With
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

                Select Case Testprobleme1.Combo_Testproblem.Text
                    Case "Sinus-Funktion"
                        Call DForm.Diag.DiagInitialise_SinusFunktion(EVO_Einstellungen1, globalAnzPar, Testprobleme1.Text_Sinusfunktion_Par.Text)
                    Case "Beale-Problem" 'x1 = [-5;5], x2=[-2;2]
                        Call DForm.Diag.DiagInitialise_BealeProblem(EVO_Einstellungen1, globalAnzPar)
                    Case "Schwefel 2.4-Problem" 'xi = [-10,10]
                        Call DForm.Diag.DiagInitialise_SchwefelProblem(EVO_Einstellungen1, globalAnzPar)
                    Case Else
                        Call DForm.Diag.DiagInitialise_MultiTestProb(EVO_Einstellungen1, Testprobleme1.Combo_Testproblem.Text)
                End Select

            Case ANW_BLUEM, ANW_SMUSI 'BlueM oder SMUSI
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                Select Case Methode

                    Case METH_SENSIPLOT 'SensiPlot
                        'XXXXXXXXXXXXXXXXXXXXXXXXX

                        'Achsen:
                        '-------
                        Dim Achse As Diagramm.Achse
                        Dim Achsen As New Collection
                        'X-Achse = QWert
                        Achse.Name = Sim1.OptZieleListe(SensiPlot1.Selected_OptZiel).Bezeichnung
                        Achse.Auto = True
                        Achse.Max = 0
                        Achsen.Add(Achse)
                        'Y-Achse = OptParameter
                        Achse.Name = Sim1.OptParameterListe(SensiPlot1.Selected_OptParameter).Bezeichnung
                        Achse.Auto = True
                        Achse.Max = 0
                        Achsen.Add(Achse)

                        'Diagramm initialisieren
                        Call DForm.Diag.DiagInitialise(Anwendung, Achsen)

                        'Series initialisieren
                        Dim tmpPoint As New Steema.TeeChart.Styles.Points(Me.DForm.Diag.Chart)
                        tmpPoint.Title = "Simulationsergebnis"
                        tmpPoint.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                        tmpPoint.Color = System.Drawing.Color.Orange
                        tmpPoint.Pointer.HorizSize = 2
                        tmpPoint.Pointer.VertSize = 2


                    Case METH_CES, METH_CES_PES 'Methode CES
                        'XXXXXXXXXXXXXXXXXXXXX

                        'Achsen:
                        '-------
                        Dim Achse As Diagramm.Achse
                        Dim Achsen As New Collection
                        'Bei SO: X-Achse = Simulationen
                        If (EVO_Einstellungen1.isMultiObjective = False) Then
                            Achse.Name = "Simulation"
                            Achse.Auto = False
                            Achse.Max = CES1.n_Childs * CES1.n_Generation
                            Achsen.Add(Achse)
                        End If
                        'für jede Zielfunktion eine weitere Achse hinzufügen
                        'HACK: Diagramm-Achsen bisher nur für Anwendung BlueM!
                        For i = 0 To Sim1.OptZieleListe.GetUpperBound(0)
                            Achse.Name = Sim1.OptZieleListe(i).Bezeichnung
                            Achse.Auto = True
                            Achse.Max = 0
                            Achsen.Add(Achse)
                        Next

                        'Diagramm initialisieren
                        Call DForm.Diag.DiagInitialise(Anwendung, Achsen)

                    Case METH_PES 'Methode PES
                        'XXXXXXXXXXXXXXXXXXXXX

                        Dim n_Kalkulationen As Integer
                        Dim n_Populationen As Integer

                        'Anzahl Kalkulationen
                        n_Kalkulationen = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf

                        'Anzahl Populationen
                        n_Populationen = 1
                        If EVO_Einstellungen1.isPOPUL Then
                            n_Populationen = EVO_Einstellungen1.NPopul
                        End If

                        'Achsen:
                        '-------
                        Dim Achse As Diagramm.Achse
                        Dim Achsen As New Collection
                        'Bei SO: X-Achse = Simulationen
                        If (EVO_Einstellungen1.isMultiObjective = False) Then
                            Achse.Name = "Simulation"
                            Achse.Auto = False
                            Achse.Max = n_Kalkulationen
                            Achsen.Add(Achse)
                        End If
                        'für jede Zielfunktion eine weitere Achse hinzufügen
                        For i = 0 To Sim1.OptZieleListe.GetUpperBound(0)
                            Achse.Name = Sim1.OptZieleListe(i).Bezeichnung
                            Achse.Auto = True
                            Achse.Max = 0
                            Achsen.Add(Achse)
                        Next

                        'Diagramm initialisieren
                        Call DForm.Diag.DiagInitialise(Anwendung, Achsen)

                        'Standard-Series initialisieren
                        If (EVO_Einstellungen1.isMultiObjective = False) Then
                            Call DForm.Diag.prepareSeries_SO(n_Populationen)
                        Else
                            Call DForm.Diag.prepareSeries_MO()
                        End If

                    Case Else 'andere Anwendungen
                        'XXXXXXXXXXXXXXXXXXXXXXXX
                        Throw New Exception("Diese Funktion ist für die Anwendung '" & Anwendung & "' nicht vorgesehen")

                End Select

        End Select

    End Sub

#End Region 'Diagrammfunktionen

#End Region 'Methoden

End Class
