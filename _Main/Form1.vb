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
    Private Method As String
    Private Const METH_RESET As String = "Reset"
    Private Const METH_PES As String = "PES"
    Private Const METH_CES As String = "CES"
    Private Const METH_CES_PES As String = "CES + PES"
    Private Const METH_SENSIPLOT As String = "SensiPlot"

    '**** Deklarationen der Module *****
    Public WithEvents Sim1 As Sim
    Public SensiPlot1 As SensiPlot
    Public CES1 As EvoKern.CES
    Public TSP1 As TSP

    '**** Globale Parameter Parameter Optimierung ****
    Dim myIsOK As Boolean
    Dim globalAnzPar As Short
    Dim globalAnzZiel_ParaOpt As Short
    Dim globalAnzRand As Short
    Dim array_x() As Double
    Dim array_y() As Double
    Dim Bestwert(,) As Double = {}
    Dim SekPopulation(,) As Double
    Dim mypara(,) As Double

    '**** Verschiedenes ****
    Dim isrun As Boolean = False                        'Optimierung läuft
    Dim ispause As Boolean = False                      'Optimierung ist pausiert

    Structure Struct_Exchange                           'Struktur um Informationen zwischen PES und CES auszutauschen 
        Dim Series_No As Integer
    End Structure

    Dim Exchange As Struct_Exchange

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
        ComboBox_Methode.Items.AddRange(New Object() {"", METH_RESET, METH_PES, METH_CES, METH_CES_PES, METH_SENSIPLOT})
        ComboBox_Methode.SelectedIndex = 0
        ComboBox_Methode.Enabled = False

        'Exchenge wird initialisiert
        Me.Exchange.Series_No = 0

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

            'Mauszeiger busy
            Cursor = System.Windows.Forms.Cursors.WaitCursor

            Me.Anwendung = ComboBox_Anwendung.SelectedItem

            Select Case Me.Anwendung

                Case "" 'Keine Anwendung ausgewählt
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Mauszeiger wieder normal
                    Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub


                Case ANW_BLUEM 'Anwendung BlueM
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse BlueM initialisieren
                    Sim1 = New BlueM

                    'Initialisieren
                    Call Sim1.SimIni()


                Case ANW_SMUSI 'Anwendung Smusi
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse Smusi initialisieren
                    Sim1 = New Smusi

                    'Initialisieren
                    Call Sim1.SimIni()


                Case ANW_TESTPROBLEME 'Anwendung Testprobleme
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Test-Probleme aktivieren
                    Testprobleme1.Enabled = True

                    'EVO_Einstellungen aktivieren
                    EVO_Einstellungen1.Enabled = True

                    EVO_Einstellungen1.OptModus = Testprobleme1.OptModus

                    'Globale Parameter werden gesetzt
                    Call Testprobleme1.Parameter_Uebergabe(Testprobleme1.Combo_Testproblem.Text, Testprobleme1.Text_Sinusfunktion_Par.Text, Testprobleme1.Text_Schwefel24_Par.Text, globalAnzPar, globalAnzZiel_ParaOpt, globalAnzRand, mypara)

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
            Cursor = System.Windows.Forms.Cursors.Default

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

            'Alles deaktivieren, danach je nach Methode aktivieren
            '-----------------------------------------------------

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Scatterplot deaktivieren
            Me.Button_Scatterplot.Enabled = False

            'EVO_Einstellungen deaktivieren
            EVO_Einstellungen1.Enabled = False

            'Mauszeiger busy
            Cursor = System.Windows.Forms.Cursors.WaitCursor

            Me.Method = ComboBox_Methode.SelectedItem

            Select Case Me.Method

                Case "" 'Keine Methode ausgewählt
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Mauszeiger wieder normal
                    Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub


                Case METH_RESET 'Methode Reset
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Ergebnisdatenbank ausschalten
                    Sim1.Ergebnisdb = False

                    'Original ModellParameter schreiben
                    Call Sim1.ModellParameter_schreiben()

                    MsgBox("Die Startwerte der Optimierungsparameter wurden in die Eingabedateien geschrieben.", MsgBoxStyle.Information, "Info")


                Case METH_SENSIPLOT 'Methode SensiPlot
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    SensiPlot1 = New SensiPlot

                    'Ergebnisdatenbank ausschalten
                    Sim1.Ergebnisdb = False

                    'SensiPlot für Sim vorbereiten
                    Call Sim1.prepare_PES()

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
                        Cursor = System.Windows.Forms.Cursors.Default
                        Exit Sub
                    End If


                Case METH_PES 'Methode PES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'EVO_Einstellungen aktivieren
                    EVO_Einstellungen1.Enabled = True

                    'Ergebnisdatenbank einschalten
                    Sim1.Ergebnisdb = True

                    'Scatterplot aktivieren
                    Me.Button_Scatterplot.Enabled = True

                    'PES für Sim vorbereiten
                    Call Sim1.prepare_PES()

                    'Je nach Anzahl der Zielfunktionen von MO auf SO umschalten
                    If (Sim1.List_OptZiele.GetLength(0) = 1) Then
                        EVO_Einstellungen1.OptModus = 0
                    ElseIf (Sim1.List_OptZiele.GetLength(0) > 1) Then
                        EVO_Einstellungen1.OptModus = 1
                    End If

                    'Parameterübergabe an PES
                    Call Sim1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel_ParaOpt, globalAnzRand, mypara)

                Case METH_CES 'Methode CES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Funktioniert nur bei BlueM!
                    If (Not Anwendung = ANW_BLUEM) Then
                        Throw New Exception("CES funktioniert bisher nur mit BlueM!")
                    End If

                    'Ergebnisdatenbank ausschalten
                    Sim1.Ergebnisdb = False

                    'CES für Sim vorbereiten
                    Call Sim1.prepare_CES()

                    CES1 = New EvoKern.CES

                    'Je nach Anzahl der Zielfunktionen von MO auf SO umschalten
                    If (Sim1.List_OptZiele.GetLength(0) = 1) Then
                        EVO_Einstellungen1.OptModus = 0
                    ElseIf (Sim1.List_OptZiele.GetLength(0) > 1) Then
                        EVO_Einstellungen1.OptModus = 1
                    End If

                    'Anzahl der Ziele, Locations und Verzeigungen wird an CES übergeben
                    CES1.n_Penalty = Sim1.List_OptZiele.GetLength(0)
                    CES1.n_Location = Sim1.List_Locations.GetLength(0)
                    CES1.n_Verzweig = Sim1.VerzweigungsDatei.GetLength(0)
                    CES1.TestModus = Sim1.Set_TestModus
                    CES1.n_Combinations = Sim1.No_of_Combinations

                    'Bei Testmodus wird die Anzahl der Kinder und Generationen überschrieben
                    If CES1.TestModus = 1 Then
                        CES1.n_Childs = 1
                        CES1.n_Parents = 1
                        CES1.n_Generation = 1
                        ReDim CES1.NDSResult(CES1.n_Childs + CES1.n_Parents - 1)
                    ElseIf CES1.TestModus = 2 Then
                        CES1.n_Childs = CES1.n_Combinations
                        CES1.n_Generation = 1
                        ReDim CES1.NDSResult(CES1.n_Childs + CES1.n_Parents - 1)
                    End If

                    'Gibt die PathSize an für jede Pfadstelle
                    Dim i As Integer
                    ReDim CES1.n_PathDimension(CES1.n_Location - 1)
                    For i = 0 To CES1.n_Location - 1
                        CES1.n_PathDimension(i) = Sim1.List_Locations(i).List_Massnahmen.GetLength(0)
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

                    'CES für Sim vorbereiten
                    Call Sim1.prepare_CES()
                    'PES für Sim vorbereiten
                    Call Sim1.prepare_PES()

                    CES1 = New EvoKern.CES

                    'Je nach Anzahl der Zielfunktionen von MO auf SO umschalten
                    If (Sim1.List_OptZiele.GetLength(0) = 1) Then
                        EVO_Einstellungen1.OptModus = 0
                    ElseIf (Sim1.List_OptZiele.GetLength(0) > 1) Then
                        EVO_Einstellungen1.OptModus = 1
                    End If

                    'Anzahl der Ziele, Locations und Verzeigungen wird an CES übergeben
                    CES1.n_Penalty = Sim1.List_OptZiele.GetLength(0)
                    CES1.n_Location = Sim1.List_Locations.GetLength(0)
                    CES1.n_Verzweig = Sim1.VerzweigungsDatei.GetLength(0)
                    CES1.TestModus = Sim1.Set_TestModus
                    CES1.n_Combinations = Sim1.No_of_Combinations

                    'Bei Testmodus wird die Anzahl der Kinder und Generationen überschrieben
                    If CES1.TestModus = 1 Then
                        CES1.n_Childs = 1
                        CES1.n_Parents = 1
                        CES1.n_Generation = 1
                        ReDim CES1.NDSResult(CES1.n_Childs + CES1.n_Parents - 1)
                    ElseIf CES1.TestModus = 2 Then
                        CES1.n_Childs = CES1.n_Combinations
                        CES1.n_Generation = 1
                        ReDim CES1.NDSResult(CES1.n_Childs + CES1.n_Parents - 1)
                    End If

                    'Gibt die PathSize an für jede Pfadstelle
                    Dim i As Integer
                    ReDim CES1.n_PathDimension(CES1.n_Location - 1)
                    For i = 0 To CES1.n_Location - 1
                        CES1.n_PathDimension(i) = Sim1.List_Locations(i).List_Massnahmen.GetLength(0)
                    Next

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
        'Datensatzanzeige aktualisieren
        Me.LinkLabel_WorkDir.Text = Sim1.WorkDir & Sim1.Datensatz & ".ALL"
        Me.LinkLabel_WorkDir.Links(0).LinkData = Sim1.WorkDir
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
                            Call STARTEN_PES(Exchange)
                        Case METH_CES
                            Call STARTEN_CES()
                        Case METH_CES_PES
                            Call STARTEN_CES_PES()
                    End Select

                Case ANW_TESTPROBLEME
                    Call STARTEN_PES(Exchange)

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

        End If

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

        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        'Simulationsschleife
        '-------------------
        Randomize()

        For i = 0 To SensiPlot1.Anz_Sim - 1

            'OptParameterwert variieren
            Select Case SensiPlot1.Selected_SensiType
                Case "Gleichverteilt"
                    Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter).SKWert = Rnd()
                Case "Diskret"
                    Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter).SKWert = i / SensiPlot1.Anz_Sim
            End Select

            'Modellparameter schreiben
            Call Sim1.ModellParameter_schreiben()

            'Simulieren
            Call Sim1.launchSim()

            'Qwert berechnen
            Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).QWertTmp = Sim1.QWert(Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel))

            'Diagramm aktualisieren
            DForm.Diag.Series(0).Add(Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).QWertTmp, Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter).Wert, "")

            'Speichern des Simulationsergebnisses für Wave
            'BUG 119: Die WEL-Datei hat bei Smusi einen anderen Namen!
            Sim.Read_WEL(Sim1.WorkDir & Sim1.Datensatz & ".wel", Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).SimGr, Wave1.WaveList(i).Wave)
            Wave1.WaveList(i).Bezeichnung = Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter).Bezeichnung & ": " _
                                            & Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter).Wert

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
        yAchse.Name = Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).SimGr
        yAchse.Auto = True
        Achsen.Add(yAchse)

        Call Wave1.WForm.Diag.DiagInitialise("SensiPlot", Achsen)

        'Serien initialisieren
        Dim tmpSeries As Steema.TeeChart.Styles.Line
        For i = 0 To SensiPlot1.Anz_Sim - 1
            tmpSeries = New Steema.TeeChart.Styles.Line(Wave1.WForm.Diag.Chart)
            tmpSeries.Title = Wave1.WaveList(i).Bezeichnung
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
        If (Sim1.List_OptZiele.GetLength(0) > 2) Then
            Throw New Exception("Zu viele Ziele für CES. Max=2")
        End If

        Dim durchlauf_all As Integer = 0

        'Laufvariable für die Generationen
        Dim gen As Integer
        Dim i As Integer

        'Parents und Child werden Dimensioniert
        Call CES1.Dim_Faksimile(CES1.List_Parents)
        Call CES1.Dim_Faksimile(CES1.List_Childs)

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

        'Generationsschleife
        For gen = 0 To CES1.n_Generation - 1

            'Child Schleife
            For i = 0 To CES1.n_Childs - 1
                durchlauf_all += 1

                'Vorbereitung und Evaluierung des Blauen Modells
                '***********************************************
                Call Sim1.Prepare_Evaluation_CES(CES1.List_Childs(i).Path)
                Call Sim1.Set_Elemente(CES1.List_Childs(i).Elemente)
                Call Sim1.Sim_Evaluierung_CombiOpt(CES1.n_Penalty, CES1.List_Childs(i).Penalty)
                '***********************************************

                'Zeichnen MO_SO
                Call DForm.Diag.prepareSeries(0, "Childs", Steema.TeeChart.Styles.PointerStyles.Circle, 3)
                If CES1.n_Penalty = 1 Then
                    Call DForm.Diag.Series(0).Add(durchlauf_all, CES1.List_Childs(i).Penalty(0))
                ElseIf CES1.n_Penalty = 2 Then
                    Call DForm.Diag.Series(0).Add(CES1.List_Childs(i).Penalty(0), CES1.List_Childs(i).Penalty(1))
                End If

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

            'Nur wenn Testmodus
            '******************
            If CES1.TestModus = 0 Then
                'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
                Call CES1.Reset_Childs()
                'Reproduktionsoperatoren, hier gehts dezent zur Sache
                Call CES1.Reproduction_Control()
                'Mutationsoperatoren
                Call CES1.Mutation_Control()
            End If


        Next

    End Sub

    'Anwendung BlueM mit CES und PES - START             
    '***************************************
    Private Sub STARTEN_CES_PES()

        'Fehlerabfragen
        If (Sim1.List_OptZiele.GetLength(0) > 2) Then
            Throw New Exception("Zu viele Ziele für CES. Max=2")
        End If

        Dim durchlauf_all As Integer = 0

        'Laufvariable für die Generationen
        Dim gen As Integer
        Dim i As Integer

        'Parents und Child werden Dimensioniert
        Call CES1.Dim_Faksimile(CES1.List_Parents)
        Call CES1.Dim_Faksimile(CES1.List_Childs)

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

        'Generationsschleife für CES
        'xxxxxxxxxxxxxxxxxxxxxxxxxxx
        For gen = 0 To CES1.n_Generation - 1

            'Child Schleife
            For i = 0 To CES1.n_Childs - 1
                durchlauf_all += 1

                'Vorbereitung und Evaluierung des Blauen Modells
                '***********************************************
                Call Sim1.Prepare_Evaluation_CES(CES1.List_Childs(i).Path)
                Call Sim1.Set_Elemente(CES1.List_Childs(i).Elemente)
                Call Sim1.Sim_Evaluierung_CombiOpt(CES1.n_Penalty, CES1.List_Childs(i).Penalty)
                '******************************************

                'Zeichnen MO_SO
                Call DForm.Diag.prepareSeries(0, "Childs", Steema.TeeChart.Styles.PointerStyles.Triangle, 3)
                If CES1.n_Penalty = 1 Then
                    Call DForm.Diag.Series(0).Add(durchlauf_all, CES1.List_Childs(i).Penalty(0))
                ElseIf CES1.n_Penalty = 2 Then
                    Call DForm.Diag.Series(0).Add(CES1.List_Childs(i).Penalty(0), CES1.List_Childs(i).Penalty(1))
                End If

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
                    Call DForm.Diag.prepareSeries(1, "Parent", Steema.TeeChart.Styles.PointerStyles.Triangle, 3)
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
                    Me.Exchange.Series_No = CES1.NDSResult(i).Front + 1
                    Call DForm.Diag.prepareSeries(f, "Front:" & f, Steema.TeeChart.Styles.PointerStyles.Triangle, 4)
                    Call DForm.Diag.Series(f).Add(CES1.NDSResult(i).Penalty(0), CES1.NDSResult(i).Penalty(1))
                Next
            End If

            'Nicht wenn Testmodus
            '********************
            If CES1.TestModus = 0 Then
                'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
                Call CES1.Reset_Childs()
                'Reproduktionsoperatoren, hier gehts dezent zur Sache
                Call CES1.Reproduction_Control()
                'Mutationsoperatoren
                Call CES1.Mutation_Control()
            End If

        Next
        'Ende der Generationsschleife

        'Starten der PES mit der Front von CES
        '(MaxAnzahl ist die Zahl der Eltern -> ToDo: SecPop oder Bestwertspeicher)
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        'Einstellungen für PES werden gesetzt
        Call EVO_Einstellungen1.SetFor_CES_PES()

        For i = 0 To CES1.n_Parents - 1
            If CES1.List_Parents(i).Front = 1 Then
                'Bereitet das BlaueModell für die Kombinatorik vor
                '*************************************************
                Call Sim1.Prepare_Evaluation_CES(CES1.List_Parents(i).Path)

                'Reduktion der OptimierungsParameter
                '***********************************
                Call Sim1.Reduce_OptPara_ModPara(CES1.List_Parents(i).Elemente)

                'Parameterübergabe an PES
                '************************
                Call Sim1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel_ParaOpt, globalAnzRand, mypara)
                'Starten der PES
                '***************
                Call STARTEN_PES(Exchange)

                'Series wird zwei weiter gesetzt
                Me.Exchange.Series_No += 2

            End If
        Next
    End Sub

    'Anwendung Evolutionsstrategie für Parameter Optimierung - hier Steuerung       
    '************************************************************************

    Private Sub STARTEN_PES(ByRef Exchange As Struct_Exchange)
        '==========================
        Dim i As Integer
        '--------------------------
        Dim durchlauf As Integer
        Dim Versuch As Integer
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
        Try
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
            If NNachf <= NEltern Then
                Throw New Exception("Die Anzahl der Eltern kann nicht größer als die Anzahl der Nachfahren sein!" & Chr(13) & Chr(10) & "Optimal ist ein Verhältnis von 1:3 bis 1:5.")
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Fehler")
            Exit Sub
        End Try

        'Diagramm vorbereiten und initialisieren
        If Not Me.Method = METH_CES_PES Then
            Call PrepareDiagramm()
        End If

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
        Do While (PES1.EsIsNextRunde(Me.Method))

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

                        Versuch = 0

GenerierenAusgangswerte:

                        Versuch = Versuch + 1
                        If Versuch > 10 Then
                            Throw New Exception("Es konnte keingültiger Datensatz erzeugt werden!")
                        End If

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
                                If Not Sim1.Eval_Sim_ParaOpt(globalAnzPar, globalAnzZiel_ParaOpt, mypara, durchlauf, ipop, Exchange.Series_No, QN, DForm.Diag) Then
                                    GoTo GenerierenAusgangswerte
                                End If
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

                    'sekundäre Population zeichnen
                    If PES1.isMultiObjective Then
                        myIsOK = PES1.esGetSekundärePopulation(SekPopulation)
                        Call SekundärePopulationZeichnen(SekPopulation)
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

    'Sekundäre Population zeichnen
    '*****************************
    Private Sub SekundärePopulationZeichnen(ByVal Population(,) As Double)
        Dim i As Short
        Call DForm.Diag.prepareSeries(Me.Exchange.Series_No + 1, "Sekundäre Population", Steema.TeeChart.Styles.PointerStyles.Circle, 4)
        With DForm.Diag.Series(Me.Exchange.Series_No + 1)
            .Clear()
            If UBound(Population, 2) = 2 Then
                For i = 1 To UBound(Population, 1)
                    .Add(Population(i, 1), Population(i, 2), "")
                Next i
            ElseIf UBound(Population, 2) = 3 Then
                For i = 1 To UBound(Population, 1)
                    'TODO: Hier muss eine 3D-Reihe angezeigt werden
                    '.Add(Population(i, 1), Population(i, 2), Population(i, 3), "") 
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

                Select Case Method

                    Case METH_SENSIPLOT 'SensiPlot
                        'XXXXXXXXXXXXXXXXXXXXXXXXX

                        'Achsen:
                        '-------
                        Dim Achse As Diagramm.Achse
                        Dim Achsen As New Collection
                        'X-Achse = QWert
                        Achse.Name = Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).Bezeichnung
                        Achse.Auto = True
                        Achse.Max = 0
                        Achsen.Add(Achse)
                        'Y-Achse = OptParameter
                        Achse.Name = Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter).Bezeichnung
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

                        Call DForm.Diag.add_MarksTips()


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
                        'Bei SO: X-Achse = Simulationen
                        If (EVO_Einstellungen1.isMultiObjective = False) Then
                            Achse.Name = "Simulation"
                            Achse.Auto = False
                            If EVO_Einstellungen1.isPOPUL Then
                                Achse.Max = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf * EVO_Einstellungen1.NRunden + 1
                            Else
                                Achse.Max = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf + 1
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

        'nur bei aktiver ErgebnisDB ausführen
        If (Not Sim1.Ergebnisdb) Then

            MsgBox("Wave funktioniert nur bei angeschlossener Ergebnisdatenbank!", MsgBoxStyle.Exclamation, "Fehler")
            Exit Sub

        Else

            'nur bei Population-Serien ausführen
            If (Not s.Title.StartsWith("Population")) Then
                MsgBox("Parametersätze können leider nur" & Chr(13) & Chr(10) _
                        & "für Populations-Punkte" & Chr(13) & Chr(10) _
                        & "aus der DB abgerufen werden!", MsgBoxStyle.Information, "Info")
                Exit Sub
            End If

            'Bestimmung der Parametersatz-ID
            Dim dbID As Integer
            'valueIndex fängt bei 0 an, DB-ID aber bei 1
            If (Me.EVO_Einstellungen1.isPOPUL) Then
                Dim ipop As Integer = Convert.ToInt32(s.Title.Substring(10).Trim)
                Dim nKalk As Integer = EVO_Einstellungen1.NGen * EVO_Einstellungen1.NNachf
                dbID = ((ipop - 1) * nKalk) + (valueIndex + 1)
            Else
                dbID = valueIndex + 1
            End If

            Dim i As Integer

            'OptParameter aus DB lesen
            Call Sim1.db_getOptPara(dbID)

            'Modellparameter schreiben
            Call Sim1.ModellParameter_schreiben()

            'String für die Anzeige der OptParameter
            Dim OptParaString As String
            OptParaString = Chr(13) & Chr(10) & "OptParameter: " & Chr(13) & Chr(10)
            For i = 0 To Sim1.List_OptParameter.GetUpperBound(0)
                With Sim1.List_OptParameter(i)
                    OptParaString &= Chr(13) & Chr(10) & .Bezeichnung & ": " & .Wert.ToString()
                End With
            Next

            'MessageBox
            Dim res As MsgBoxResult = MsgBox("Diesen Parametersatz simulieren?" & Chr(13) & Chr(10) & OptParaString, MsgBoxStyle.OkCancel, "Info")
            If (res = MsgBoxResult.Ok) Then

                'Simulieren
                Sim1.launchSim()

                'Wave anzeigen
                '-------------
                Dim Wave1 As New Wave()
                Dim n As Integer = 0                            'Anzahl Waves
                Dim SimSeries As New Collection                 'zu zeichnende Simulationsgrößen
                Dim RefSeries As New Collection                 'zu zeichnende Referenzreihen
                Dim QWertString As String                       'String für die Anzeige der QWerte

                QWertString = "QWerte: " & Chr(13) & Chr(10)

                'zu zeichnenden Reihen raussuchen
                For i = 0 To Sim1.List_OptZiele.GetUpperBound(0)

                    With Sim1.List_OptZiele(i)

                        'Qualitätswert berechnen und an String anhängen
                        .QWertTmp = Sim1.QWert(Sim1.List_OptZiele(i))
                        QWertString &= Chr(13) & Chr(10) & .Bezeichnung & ": " & .QWertTmp.ToString()

                        'Name der WEL-Simulationsergebnisdatei
                        Dim WELFile As String = ""
                        If (Anwendung = ANW_BLUEM) Then
                            WELFile = Sim1.WorkDir & Sim1.Datensatz & ".WEL"
                        ElseIf (Anwendung = ANW_SMUSI) Then
                            WELFile = Sim1.WorkDir & .SimGr.Substring(0, 4) & "_WEL.ASC"
                        End If

                        'Simulationsgrößen nur jeweils ein Mal zeichnen
                        If (Not SimSeries.Contains(.SimGr)) Then
                            SimSeries.Add(.SimGr, .SimGr)
                            'Simulationsergebnis in Wave speichern
                            Dim simresult(,) As Object = {}
                            Dim isOK As Boolean = Sim.Read_WEL(WELFile, .SimGr, simresult)
                            n += 1
                            ReDim Preserve Wave1.WaveList(n - 1)
                            Wave1.WaveList(n - 1).Bezeichnung = .SimGr
                            Wave1.WaveList(n - 1).Wave = simresult
                        End If

                        'ggf. Referenzreihe in Wave speichern
                        If (.ZielTyp = "Reihe" Or .ZielTyp = "IHA") Then
                            'Referenzreihen nur jeweils ein Mal zeichnen
                            If (Not RefSeries.Contains(.ZielReiheDatei & .ZielGr)) Then
                                RefSeries.Add(.ZielGr, .ZielReiheDatei & .ZielGr)
                                n += 1
                                ReDim Preserve Wave1.WaveList(n - 1)
                                Wave1.WaveList(n - 1).Bezeichnung = .SimGr & " (REF)"
                                Wave1.WaveList(n - 1).Wave = .ZielReihe
                            End If
                        End If

                    End With
                Next

                'Titel
                Dim Titel As String = "Simulationsergebnis"
                'Achsen
                Dim Achsen As New Collection
                'X-Achse: Zeit
                Dim XAchse As Diagramm.Achse
                XAchse.Name = "Zeit"
                XAchse.Auto = True
                Achsen.Add(XAchse)
                'Y-Achse: 
                Dim YAchse As Diagramm.Achse
                YAchse.Name = ""
                YAchse.Auto = True
                Achsen.Add(YAchse)

                'Initialisierung
                Call Wave1.WForm.Diag.DiagInitialise(Titel, Achsen)

                'Annotation anzeigen
                Dim anno1 As New Steema.TeeChart.Tools.Annotation(Wave1.WForm.Diag.Chart)
                anno1.Text = QWertString & Chr(13) & Chr(10) & OptParaString
                anno1.Position = Steema.TeeChart.Tools.AnnotationPositions.LeftTop

                'Serien initialisieren
                Dim tmpSeries As Steema.TeeChart.Styles.Line
                For i = 0 To Wave1.WaveList.GetUpperBound(0)
                    tmpSeries = New Steema.TeeChart.Styles.Line(Wave1.WForm.Diag.Chart)
                    tmpSeries.Title = Wave1.WaveList(i).Bezeichnung
                    tmpSeries.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Nothing
                Next

                'Reihen zeichnen
                Wave1.Wave_draw()

                'Form anzeigen
                Wave1.Show()

            End If

        End If

    End Sub

    'Daten aus DB laden und als Scatterplot-Matrix anzeigen
    '*******************************************************
    Private Sub showScatterplot(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Scatterplot.Click

        Dim diagresult As DialogResult

        'Datei-öffnen Dialog anzeigen
        Me.OpenFileDialog_MDB.InitialDirectory = Sim1.WorkDir
        diagresult = Me.OpenFileDialog_MDB.ShowDialog()

        If (diagresult = Windows.Forms.DialogResult.OK) Then

            'Neuen DB-Pfad speichern
            Sim1.db_path = Me.OpenFileDialog_MDB.FileName

            'Methode zurücksetzen, damit die ausgewählte DB später nicht überschrieben wird
            Me.ComboBox_Methode.SelectedItem = ""

            'Daten einlesen
            Cursor = Cursors.WaitCursor
            Dim series As Collection = Sim1.db_readQWerte()
            Cursor = Cursors.Default

            'Abfrageform
            Dim Form2 As New ScatterplotAbfrage
            For Each serie As Main.Serie In series
                Form2.ListBox_OptZieleX.Items.Add(serie.name)
                Form2.ListBox_OptZieleY.Items.Add(serie.name)
            Next
            diagresult = Form2.ShowDialog()

            If (diagresult = Windows.Forms.DialogResult.OK) Then

                If (Form2.CheckBox_Hauptdiagramm.Checked) Then
                    'Hauptdiagramm
                    '-------------
                    Dim Achsen As New Collection
                    Dim tmpAchse As Main.Diagramm.Achse
                    tmpAchse.Auto = True
                    tmpAchse.Name = Form2.ListBox_OptZieleX.SelectedItem
                    Achsen.Add(tmpAchse)
                    tmpAchse.Name = Form2.ListBox_OptZieleY.SelectedItem
                    Achsen.Add(tmpAchse)
                    Me.DForm.Diag.Clear()
                    Me.DForm.Diag.DiagInitialise(Path.GetFileName(Sim1.db_path), Achsen)
                    Me.DForm.Diag.prepareSeries(0, "Population")
                    Me.DForm.Diag.Chart.Series(0).Cursor = Cursors.Hand
                    For i As Integer = 0 To series(1).values.getUpperBound(0)
                        Me.DForm.Diag.Chart.Series(0).Add(series(Form2.ListBox_OptZieleX.SelectedIndex + 1).values(i), series(Form2.ListBox_OptZieleY.SelectedIndex + 1).values(i))
                    Next
                End If

                If (Form2.CheckBox_Scatterplot.Checked) Then
                    'Scatterplot
                    '-----------
                    Cursor = Cursors.WaitCursor
                    Dim scatterplot1 As New Scatterplot
                    Call scatterplot1.zeichnen(series)
                    Call scatterplot1.Show()
                    Cursor = Cursors.Default
                End If

            End If

        End If

    End Sub

#End Region 'Diagrammfunktionen

#End Region 'Methoden

End Class
