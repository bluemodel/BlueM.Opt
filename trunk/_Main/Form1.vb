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
    Public CES1 As EvoKern.CES
    Public TSP1 As TSP

    '**** Globale Parameter Parameter Optimierung ****
    Dim myIsOK As Boolean
    Dim globalAnzPar As Short
    Dim globalAnzZiel As Short
    Dim globalAnzRand As Short
    Dim array_x() As Double
    Dim array_y() As Double
    Dim Bestwert(,) As Double = {}
    Dim SekPopulation(,) As Double
    Dim myPara(,) As Double

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

        'Exchange wird initialisiert
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

            'Diagramm zurücksetzen
            Me.DForm.Diag.Reset()

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

                    'Methode setzen
                    Sim1.Method = METH_RESET

                    'Ergebnisdatenbank ausschalten
                    Sim1.Ergebnisdb = False

                    'Original ModellParameter schreiben
                    Call Sim1.Prepare_Write_ModellParameter()

                    MsgBox("Die Startwerte der Optimierungsparameter wurden in die Eingabedateien geschrieben.", MsgBoxStyle.Information, "Info")


                Case METH_SENSIPLOT 'Methode SensiPlot
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    SensiPlot1 = New SensiPlot

                    'Methode setzen
                    Sim1.Method = METH_SENSIPLOT

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
                        Cursor = System.Windows.Forms.Cursors.Default
                        Exit Sub
                    End If


                Case METH_PES 'Methode PES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'EVO_Einstellungen aktivieren
                    EVO_Einstellungen1.Enabled = True

                    'Methode setzen
                    Sim1.Method = METH_PES

                    'Ergebnisdatenbank einschalten
                    Sim1.Ergebnisdb = True

                    'Scatterplot aktivieren
                    Me.Button_Scatterplot.Enabled = True

                    'PES für Sim vorbereiten
                    Call Sim1.read_and_valid_INI_Files_PES()

                    'Je nach Anzahl der Zielfunktionen von MO auf SO umschalten
                    If (Sim1.List_OptZiele.GetLength(0) = 1) Then
                        EVO_Einstellungen1.OptModus = 0
                    ElseIf (Sim1.List_OptZiele.GetLength(0) > 1) Then
                        EVO_Einstellungen1.OptModus = 1
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
                            'Methode setzen
                            Sim1.Method = METH_CES

                            'CES für Sim vorbereiten (Files lesen und Validieren)
                            Call Sim1.read_and_valid_INI_Files_CES()

                        Case METH_CES_PES, METH_HYBRID
                            'Methode setzen
                            Sim1.Method = METH_CES_PES

                            'EVO_Einstellungen aktiviern
                            EVO_Einstellungen1.Enabled = True

                            'CES für Sim vorbereiten (Files lesen und Validieren)
                            Call Sim1.read_and_valid_INI_Files_CES_PES()

                    End Select

                    'CES initialisieren
                    CES1 = New EvoKern.CES
                    'Prüft ob die Zahl mög. Kombinationen < Zahl Eltern + Nachfolger
                    If (CES1.n_Childs + CES1.n_Parents) > Sim1.No_of_Combinations Then
                        Throw New Exception("Die Zahl der Eltern + die Zahl der Kinder ist größer als die mögliche Zahl der Kombinationen.")
                    End If

                    'Je nach Anzahl der Zielfunktionen von MO auf SO umschalten
                    If (Sim1.List_OptZiele.GetLength(0) = 1) Then
                        EVO_Einstellungen1.OptModus = 0
                    ElseIf (Sim1.List_OptZiele.GetLength(0) > 1) Then
                        EVO_Einstellungen1.OptModus = 1
                    End If

                    'Anzahl der Ziele, Locations und Verzeigungen wird an CES übergeben
                    CES1.n_Penalty = Sim1.List_OptZiele.GetLength(0)
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
                            Call STARTEN_CES_or_CES_PES()
                        Case METH_CES_PES
                            Call STARTEN_CES_or_CES_PES()
                        Case METH_HYBRID
                            Call STARTEN_CES_or_CES_PES()
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

        'Hinweis:
        '------------------------------------------------------------------------
        'Die Modellparameter werden auch für die nicht ausgewählten OptParameter 
        'geschrieben, und zwar mit den in der OPT-Datei angegebenen Startwerten
        '------------------------------------------------------------------------

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

        Dim i, j, n As Integer

        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        'Oberflächendiagramm
        Dim surface As New Steema.TeeChart.Styles.Surface
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
            Me.DForm.Diag.add_MarksTips()
            surface.Title = "Population"
            surface.Cursor = Cursors.Hand
        End If

        'Simulationsschleife
        '-------------------
        Randomize()

        n = 0
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
                Call Sim1.Prepare_Write_ModellParameter()

                'Simulieren
                Call Sim1.launchSim()

                'Qwert berechnen
                Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).QWertTmp = Sim1.QWert(Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel))

                'Diagramm aktualisieren
                If (Me.globalAnzPar = 1) Then
                    '1 Parameter
                    DForm.Diag.Series(0).Add(Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).QWertTmp, Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Wert, "")
                Else
                    '2 Parameter
                    surface.Add(Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Wert, Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).QWertTmp, Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Wert)
                End If

                'Simulationsergebnis in Wave laden
                If (SensiPlot1.show_Wave) Then
                    'BUG 119: Die WEL-Datei hat bei Smusi einen anderen Namen!
                    Dim WEL As New Wave.WEL(Sim1.WorkDir & Sim1.Datensatz & ".wel", Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).SimGr)
                    'OptParameter und -Wert an Titel anhängen
                    'TODO: bei 2-Parametern auch den Wert des 2. Parameters anhängen!
                    WEL.Zeitreihen(0).Title += " (" & Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Bezeichnung & ": " _
                                                    & Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Wert & ")"
                    Wave1.Import_Zeitreihe(WEL.Zeitreihen(0))
                End If

                'Qualitätswerte und OptParameter in DB speichern
                If (Sim1.Ergebnisdb = True) Then
                    Call Sim1.db_update(n, 1)
                End If

                n += 1

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

        'Startwerte werden der Verlaufsanzeige werden zugewiesen
        Call Me.INI_Verlaufsanzeige(1, 1, CES1.n_Generations, CES1.n_Childs)

        'Generationsschleife CES
        'xxxxxxxxxxxxxxxxxxxxxxx
        For gen = 0 To CES1.n_Generations - 1

            Call EVO_Opt_Verlauf1.Generation(gen + 1)

            'Child Schleife
            For i = 0 To CES1.n_Childs - 1
                durchlauf_all += 1

                Call EVO_Opt_Verlauf1.Nachfolger(i + 1)

                '****************************************
                'Aktueller Pfad wird an Sim zurückgegeben
                'Bereitet das BlaueModell für die Kombinatorik vor
                Call Sim1.PREPARE_Evaluation_CES(CES1.List_Childs(i).Path)

                'HYBRID
                '******
                If Method = METH_HYBRID Then
                    Call Sim1.Reduce_OptPara_ModPara()
                    Call Sim1.SaveParameter_to_Child(CES1.List_Childs(i).myPara)
                    Call Sim1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel, globalAnzRand, myPara)
                    Call Sim1.PREPARE_Evaluation_PES(myPara)
                End If

                Call Sim1.SIM_Evaluierung_CES(CES1.List_Childs(i).Penalty)
                '*********************************************************

                'HYBRID: Speichert die PES Erfahrung diesen Childs im PES Memory
                '***************************************************************
                If Method = METH_HYBRID Then
                    Call CES1.Memory_Store(i, gen)
                End If

                'Zeichnen MO_SO Zeichnen
                Call DForm.Diag.prepareSeries(0, "Childs", Steema.TeeChart.Styles.PointerStyles.Triangle, 4)
                If CES1.n_Penalty = 1 Then
                    Call DForm.Diag.Series(0).Add(durchlauf_all, CES1.List_Childs(i).Penalty(0))
                ElseIf CES1.n_Penalty = 2 Then
                    Call DForm.Diag.Series(0).Add(CES1.List_Childs(i).Penalty(0), CES1.List_Childs(i).Penalty(1))
                End If

                System.Windows.Forms.Application.DoEvents()
            Next

            'MO oder SO Selectionsprozess oder NDSorting
            '-------------------------------------------
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

            'HYBRID:
            '*******
            If Method = METH_HYBRID Then
                'Child Schleife hier da für jedes Child die PES Funktionen angesteuert werden
                For i = 0 To CES1.List_Childs.GetUpperBound(0)
                    'Ermittelt fuer jedes Child den PES Parent Satz
                    Call CES1.Memory_Search(CES1.List_Childs(i))

                    'PES Geschichten
                    'Objekt der Klasse PES wird erzeugt PES wird erzeugt
                    Dim PES1 As EvoKern.PES
                    PES1 = New EvoKern.PES
                    'Die öffentlichen dynamischen Arrays werden initialisiert (Dn, An, Xn, Xmin, Xmax)
                    'und die Anzahl der Zielfunktionen wird festgelegt
                    '******************************************************************************************
                    myIsOK = PES1.EsIni(globalAnzPar, globalAnzZiel, globalAnzRand)
                    '3. Schritt: PES - ES_OPTIONS
                    'Optionen der Evolutionsstrategie werden übergeben
                    '******************************************************************************************
                    'myIsOK = PES1.EsOptions(iEvoTyp, iPopEvoTyp, isPOPUL, NRunden, NPopul, NPopEltern, iOptPopEltern, iOptEltern, iPopPenalty, NGen, NEltern, NNachf, NRekombXY, rDeltaStart, iStartPar, isdnvektor, isMultiObjective, isPareto, isPareto3D, Interact, isInteract, NMemberSecondPop, globalAnzPar)


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

        'Einstellungen für PES werden gesetzt
        Call EVO_Einstellungen1.SetFor_CES_PES()

        For i = 0 To CES1.n_Parents - 1
            If CES1.List_Parents(i).Front = 1 Then

                '****************************************
                'Aktueller Pfad wird an Sim zurückgegeben
                'Bereitet das BlaueModell für die Kombinatorik vor
                Call Sim1.PREPARE_Evaluation_CES(CES1.List_Childs(i).Path)

                'Reduktion der OptimierungsParameter und immer dann wenn nicht Nullvariante
                '****************************************************************************
                If Sim1.Reduce_OptPara_ModPara() Then

                    'Parameterübergabe an PES
                    '************************
                    Call Sim1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel, globalAnzRand, myPara)
                    'Starten der PES
                    '***************
                    Call STARTEN_PES(Exchange)

                    'Series wird zwei weiter gesetzt
                    Me.Exchange.Series_No += 2

                End If
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
        'Dimensionierung der Variablen für Optionen Evostrategie
        'Das Struct aus PES wird hier verwendet
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        Dim PES_Options As EvoKern.PES.Struct_Settings

        'Check!
        Dim ipop As Short = 0
        Dim igen As Short
        Dim inachf As Short
        Dim irunde As Short
        Dim QN() As Double = {}
        Dim RN() As Double = {}
        '--------------------------
        'TODO: If (ipop + igen + inachf + irunde) > 4 Then GoTo Start_Evolutionsrunden '????? Wie?
        'Werte an Variablen übergeben auskommentiert Werte finden sich im PES werden hier aber nicht zugewiesen
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        PES_Options.NEltern = EVO_Einstellungen1.NEltern
        PES_Options.NNachf = EVO_Einstellungen1.NNachf
        PES_Options.NGen = EVO_Einstellungen1.NGen
        PES_Options.iEvoTyp = EVO_Einstellungen1.iEvoTyp
        PES_Options.iPopEvoTyp = EVO_Einstellungen1.iPopEvoTyp
        PES_Options.iPopPenalty = EVO_Einstellungen1.iPopPenalty
        PES_Options.isPOPUL = EVO_Einstellungen1.isPOPUL
        PES_Options.isMultiObjective = EVO_Einstellungen1.isMultiObjective
        PES_Options.isPareto = EVO_Einstellungen1.isPareto
        PES_Options.isPareto3D = False
        PES_Options.NRunden = EVO_Einstellungen1.NRunden
        PES_Options. NPopul = EVO_Einstellungen1.NPopul
        PES_Options.NPopEltern = EVO_Einstellungen1.NPopEltern
        PES_Options.iOptPopEltern = EVO_Einstellungen1.iOptPopEltern
        PES_Options.iOptEltern = EVO_Einstellungen1.iOptEltern
        PES_Options.NRekombXY = EVO_Einstellungen1.NRekombXY
        PES_Options.rDeltaStart = EVO_Einstellungen1.rDeltaStart
        PES_Options.iStartPar = EVO_Einstellungen1.globalOPTVORGABE
        PES_Options.isDnVektor = EVO_Einstellungen1.isDnVektor
        PES_Options.Interact = EVO_Einstellungen1.Interact
        PES_Options.isInteract = EVO_Einstellungen1.isInteract
        PES_Options.NMemberSecondPop = EVO_Einstellungen1.NMemberSecondPop

        ReDim QN(globalAnzZiel)
        ReDim RN(globalAnzRand)

        'Diagramm vorbereiten und initialisieren
        If Not Me.Method = METH_CES_PES Then
            Call PrepareDiagramm()
        End If

        '1. Schritt: PES
        'Objekt der Klasse PES wird erzeugt
        '******************************************************************************************
        PES1 = New EvoKern.PES

        '2. Schritt: PES - ES_OPTIONS
        'Optionen der Evolutionsstrategie werden übergeben
        '******************************************************************************************
        myIsOK = PES1.EsOptions(PES_Options)

        '3. Schritt: PES - ES_INI
        'Die öffentlichen dynamischen Arrays werden initialisiert (Dn, An, Xn, Xmin, Xmax)
        'und die Anzahl der Zielfunktionen wird festgelegt
        '******************************************************************************************
        myIsOK = PES1.EsIni(globalAnzPar, globalAnzZiel, globalAnzRand)

        '4. Schritt: PES - ES_LET_PARAMETER
        'Ausgangsparameter werden übergeben
        '******************************************************************************************
        For i = 1 To globalAnzPar
            myIsOK = PES1.EsLetParameter(i, myPara(i, 1))
        Next i

        '5. Schritt: PES - ES_PREPARE
        'Interne Variablen werden initialisiert, Zufallsgenerator wird initialisiert
        '******************************************************************************************
        myIsOK = PES1.EsPrepare()

        '6. Schritt: PES - ES_STARTVALUES
        'Startwerte werden zugewiesen
        '******************************************************************************************
        myIsOK = PES1.EsStartvalues()

        'Startwerte werden der Verlaufsanzeige werden zugewiesen
        Call Me.INI_Verlaufsanzeige(EVO_Einstellungen1.NRunden, EVO_Einstellungen1.NPopul, EVO_Einstellungen1.NGen, EVO_Einstellungen1.NNachf)

        durchlauf = 0

Start_Evolutionsrunden:
        'Cursor setzen
        'System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        'Loop über alle Runden
        '*******************************************************************************************
        Do While (PES1.EsIsNextRunde(Me.Method))

            irunde = PES1.PES_iAkt.iAktRunde
            Call EVO_Opt_Verlauf1.Runden(irunde)

            myIsOK = PES1.EsPopBestwertspeicher()
            'Loop über alle Populationen
            '***********************************************************************************************
            Do While (PES1.EsIsNextPop)

                ipop = PES1.PES_iAkt.iAktPop
                Call EVO_Opt_Verlauf1.Populationen(ipop)

                myIsOK = PES1.EsPopVaria

                myIsOK = PES1.EsPopMutation

                'Loop über alle Generationen
                '***********************************************************************************************
                Do While (PES1.EsIsNextGen)

                    igen = PES1.PES_iAkt.iAktGen
                    Call EVO_Opt_Verlauf1.Generation(igen)

                    myIsOK = PES1.EsBestwertspeicher()

                    'Loop über alle Nachkommen
                    '********************************************************************
                    Do While (PES1.EsIsNextNachf)

                        inachf = PES1.PES_iAkt.iAktNachf
                        Call EVO_Opt_Verlauf1.Nachfolger(inachf)

                        durchlauf = durchlauf + 1

                        'Um Modellfehler bzw. Evaluierungsabbrüche abzufangen
                        'TODO: noch nicht fertig das Ergebnis wird noch nicht auf Fehler ueberprueft
                        Versuch = 0

GenerierenAusgangswerte:

                        Versuch = Versuch + 1
                        If Versuch > 10 Then
                            Throw New Exception("Es konnte kein gültiger Datensatz erzeugt werden!")
                        End If

                        'Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern
                        myIsOK = PES1.EsVaria

                        'Mutieren der Ausgangswerte
                        myIsOK = PES1.EsMutation

                        'Auslesen der Variierten Parameter
                        myIsOK = PES1.EsGetParameter(globalAnzPar, myPara)

                        'Auslesen des Bestwertspeichers
                        If Not Evo_Einstellungen1.isMultiObjective Then
                            myIsOK = PES1.EsGetBestwert(Bestwert)
                        End If

                        '************************************************************************************
                        '******************* Ansteuerung der zu optimierenden Anwendung *********************
                        '************************************************************************************
                        Select Case Anwendung
                            Case ANW_TESTPROBLEME
                                Call Testprobleme1.Evaluierung_TestProbleme(Testprobleme1.Combo_Testproblem.Text, globalAnzPar, myPara, durchlauf, ipop, QN, RN, DForm.Diag)
                            Case ANW_BLUEM, ANW_SMUSI

                                'Vorbereiten des Modelldatensatzes
                                Call Sim1.PREPARE_Evaluation_PES(myPara)

                                'Simulation und Evaluierung
                                If Not Sim1.SIM_Evaluierung_PES(durchlauf, ipop, QN, RN) Then
                                    GoTo GenerierenAusgangswerte
                                End If

                                'Qualitätswerte im TeeChart zeichnen
                                'BUG 144: TODO: Bei Verletzung von Constraints Punkt anders malen!
                                If (Sim1.List_OptZiele.Length = 1) Then
                                    'SingleObjective
                                    Call DForm.Diag.prepareSeries(ipop - 1, "Population " & ipop)
                                    DForm.Diag.Series(ipop - 1).Cursor = Cursors.Hand
                                    Call DForm.Diag.Series(ipop - 1).Add(durchlauf, Sim1.List_OptZiele(0).QWertTmp)
                                Else
                                    'MultiObjective
                                    'BUG 66: nur die ersten beiden Zielfunktionen werden gezeichnet
                                    Call DForm.Diag.prepareSeries(0, "Population", Steema.TeeChart.Styles.PointerStyles.Circle, 4)
                                    DForm.Diag.Series(0).Cursor = Cursors.Hand
                                    Call DForm.Diag.Series(0).Add(Sim1.List_OptZiele(0).QWertTmp, Sim1.List_OptZiele(1).QWertTmp)
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
                    If EVO_Einstellungen1.isMultiObjective Then
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
        Call DForm.Diag.prepareSeries(Me.Exchange.Series_No + 1, "Sekundäre Population", Steema.TeeChart.Styles.PointerStyles.Circle, 2)
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

                        If (SensiPlot1.Selected_OptParameter.GetLength(0) = 1) Then

                            '1 OptParameter:
                            '---------------

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
                            Achse.Name = Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Bezeichnung
                            Achse.Auto = True
                            Achse.Max = 0
                            Achsen.Add(Achse)

                            'Diagramm initialisieren
                            Call DForm.Diag.DiagInitialise(Anwendung, Achsen)

                            'Series initialisieren
                            Dim tmpPoint As New Steema.TeeChart.Styles.Points(Me.DForm.Diag.Chart)
                            tmpPoint.Title = "Population"
                            tmpPoint.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle
                            tmpPoint.Color = System.Drawing.Color.Orange
                            tmpPoint.Pointer.HorizSize = 2
                            tmpPoint.Pointer.VertSize = 2
                            tmpPoint.Cursor = Cursors.Hand

                            Call DForm.Diag.add_MarksTips()

                        Else
                            '2 OptParameter:
                            '---------------

                            'Achsen:
                            '-------
                            Dim Achse As Diagramm.Achse
                            Dim Achsen As New Collection
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

                            'Diagramm initialisieren
                            Call DForm.Diag.DiagInitialise(Anwendung, Achsen)

                            'Z-Achse = OptParameter2
                            DForm.Diag.Axes.Depth.Title.Caption = Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Bezeichnung
                            DForm.Diag.Axes.Depth.Automatic = True
                            DForm.Diag.Axes.Depth.Visible = True

                            '3D-Diagramm vorbereiten
                            DForm.Diag.Aspect.View3D = True
                            DForm.Diag.Aspect.Chart3DPercent = 90
                            DForm.Diag.Aspect.Elevation = 348
                            DForm.Diag.Aspect.Orthogonal = False
                            DForm.Diag.Aspect.Perspective = 62
                            DForm.Diag.Aspect.Rotation = 329
                            DForm.Diag.Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality
                            DForm.Diag.Aspect.VertOffset = -20
                            DForm.Diag.Aspect.Zoom = 66

                        End If


                    Case METH_CES, METH_CES_PES, METH_HYBRID 'Methode CES
                        'XXXXXXXXXXXXXXXXXXXXX

                        'Achsen:
                        '-------
                        Dim Achse As Diagramm.Achse
                        Dim Achsen As New Collection
                        'Bei SO: X-Achse = Simulationen
                        If (EVO_Einstellungen1.isMultiObjective = False) Then
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

        'Die X uns Y werte aus der dem Punkt mit der ValueID organisiert
        Dim xWert, yWert As Double
        xWert = s.XValues(valueIndex)
        yWert = s.YValues(valueIndex)

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

            'Unterscheidung für die Methoden
            '*******************************
            Dim db_ID_QWert As Integer
            Dim db_ID_Pfad As Integer
            Dim res As MsgBoxResult
            'String für die Anzeige der OptParameter oder Pfade
            Dim MsgString As String = ""

            Select Case Method
                Case METH_PES

                    Dim i As Integer

                    'Bestimmung der DB_ID durch x und y Werte
                    db_ID_QWert = Sim1.db_get_ID_QWert(xWert, yWert)

                    'OptParameter aus DB lesen
                    Call Sim1.db_getOptPara(db_ID_QWert)

                    'Modellparameter schreiben
                    Call Sim1.Prepare_Write_ModellParameter()

                    'String für die Anzeige der OptParameter wird generiert
                    MsgString = Chr(13) & Chr(10) & "OptParameter: " & Chr(13) & Chr(10)
                    For i = 0 To Sim1.List_OptParameter.GetUpperBound(0)
                        With Sim1.List_OptParameter(i)
                            MsgString &= Chr(13) & Chr(10) & .Bezeichnung & ": " & .Wert.ToString()
                        End With
                    Next

                    'MessageBox
                    res = MsgBox("Diesen Parametersatz simulieren?" & Chr(13) & Chr(10) & MsgString, MsgBoxStyle.OkCancel, "Info")

                Case METH_CES
                    Dim i As Integer

                    'Bestimmung der DB_ID durch x und y werte
                    db_ID_QWert = Sim1.db_get_ID_QWert(xWert, yWert)

                    'Pfad aus DB lesen
                    Call Sim1.db_getPfad(db_ID_QWert)

                    'Bereitet das BlaueModell für die Kombinatorik vor
                    Call Sim1.PREPARE_Evaluation_CES()

                    'String für die Anzeige der Pfade wird generiert
                    MsgString = Chr(13) & Chr(10) & "Pfad: " & Chr(13) & Chr(10)
                    For i = 0 To Sim1.Aktuelle_Massnahmen.GetUpperBound(0)
                        MsgString &= Chr(13) & Chr(10) & Sim1.List_Locations(i).Name & ": " & Sim1.Aktuelle_Massnahmen(i).ToString()
                    Next

                    'MessageBox
                    res = MsgBox("Diesen Pfad simulieren?" & Chr(13) & Chr(10) & MsgString, MsgBoxStyle.OkCancel, "Info")

                Case METH_CES_PES

                    Dim i As Integer

                    'Bestimmung der DB_ID durch x und y werte
                    db_ID_QWert = Sim1.db_get_ID_QWert(xWert, yWert)
                    db_ID_Pfad = Sim1.db_get_ID_Pfad(db_ID_QWert)

                    'Pfad und Parameter aus DB lesen
                    Call Sim1.db_getOptPara(db_ID_QWert)
                    Call Sim1.db_getPfad(db_ID_Pfad)

                    'Bereitet das BlaueModell für die Kombinatorik vor
                    Call Sim1.PREPARE_Evaluation_CES()
                    'Modellparameter schreiben
                    Call Sim1.Prepare_Write_ModellParameter()

                    'String für die Anzeige der Pfade wird generiert
                    MsgString = Chr(13) & Chr(10) & "Pfad: " & Chr(13) & Chr(10)
                    For i = 0 To Sim1.Aktuelle_Massnahmen.GetUpperBound(0)
                        MsgString &= Chr(13) & Chr(10) & Sim1.List_Locations(i).Name & ": " & Sim1.Aktuelle_Massnahmen(i).ToString()
                    Next
                    'MsgString &= Chr(13) & Chr(10) & "OptParameter: " & Chr(13) & Chr(10)
                    'For i = 0 To Sim1.List_OptParameter.GetUpperBound(0)
                    '    With Sim1.List_OptParameter(i)
                    '        MsgString &= Chr(13) & Chr(10) & .Bezeichnung & ": " & .Wert.ToString()
                    '    End With
                    'Next

                    'MessageBox
                    res = MsgBox("Diesen Pfad simulieren?" & Chr(13) & Chr(10) & MsgString, MsgBoxStyle.OkCancel, "Info")

            End Select


            If (res = MsgBoxResult.Ok) Then

                Dim SimSeries As New Collection                 'zu zeichnende Simulationsgrößen
                Dim RefSeries As New Collection                 'zu zeichnende Referenzreihen
                Dim QWertString As String                       'String für die Anzeige der QWerte

                'Simulieren
                Sim1.launchSim()

                'Wave instanzieren
                Dim Wave1 As New Wave.Wave

                QWertString = "QWerte: " & Chr(13) & Chr(10)

                'zu zeichnenden Reihen aus Liste der OptZiele raussuchen
                Dim i As Integer
                For i = 0 To Sim1.List_OptZiele.GetUpperBound(0)

                    With Sim1.List_OptZiele(i)

                        'Qualitätswert berechnen und an String anhängen
                        .QWertTmp = Sim1.QWert(Sim1.List_OptZiele(i))
                        QWertString &= Chr(13) & Chr(10) & .Bezeichnung & ": " & .QWertTmp.ToString()

                        'Name der WEL-Simulationsergebnisdatei
                        'BUG 137: Name der Ergebnisdatei
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
                                Wave1.Import_Zeitreihe(.ZielReihe)
                            End If
                        End If

                    End With
                Next

                'Annotation anzeigen
                Dim anno1 As New Steema.TeeChart.Tools.Annotation(Wave1.TChart1.Chart)
                anno1.Text = QWertString & Chr(13) & Chr(10) & MsgString
                anno1.Position = Steema.TeeChart.Tools.AnnotationPositions.LeftTop

                'Wave anzeigen
                Call Wave1.Show()

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
