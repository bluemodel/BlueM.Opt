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
'**** Letzte Änderung: Juli 2007                                            ****
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

    'Optimierungsmethode
    Public Shared Method As String

    '**** Deklarationen der Module *****
    Private WithEvents Testprobleme1 As Testprobleme
    Friend WithEvents Sim1 As Sim
    Private SensiPlot1 As SensiPlot
    Private CES1 As EVO.Kern.CES
    Private TSP1 As TSP

    '**** Globale Parameter Parameter Optimierung ****
    'TODO: diese Werte sollten eigentlich nur in CES bzw PES vorgehalten werden
    Dim globalAnzPar As Short
    Dim globalAnzZiel As Short
    Dim globalAnzRand As Short
    Dim array_x() As Double
    Dim array_y() As Double
    'TODO: Bestwertspeicher wird nicht genutzt!
    'Dim Bestwert(,) As Double
    Dim SekPopulation(,) As Double
    Dim myPara() As EVO.Kern.OptParameter

    '**** Verschiedenes ****
    Dim isrun As Boolean = False                        'Optimierung läuft
    Dim ispause As Boolean = False                      'Optimierung ist pausiert

    'Dialoge
    Private WithEvents solutionDialog As SolutionDialog
    Private WithEvents scatterplot1 As Scatterplot

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
        ComboBox_Anwendung.Items.AddRange(New Object() {"", ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM, ANW_TESTPROBLEME, ANW_TSP})
        ComboBox_Anwendung.SelectedIndex = 0

        'Liste der Methoden in ComboBox schreiben und Anfangseinstellung wählen
        ComboBox_Methode.Items.AddRange(New Object() {"", METH_RESET, METH_PES, METH_CES, METH_HYBRID, METH_SENSIPLOT, METH_HOOKJEEVES})
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
    Friend Sub INI_App(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Anwendung.SelectedIndexChanged

        If (Me.IsInitializing = True) Then

            Exit Sub

        Else

            'Diagramm zurücksetzen
            Me.DForm.Diag.Reset()

            'Alles deaktivieren, danach je nach Anwendung aktivieren
            '-------------------------------------------------------

            'Sim1 zerstören
            Me.Sim1 = Nothing

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Datensatz deaktivieren
            Me.Label_Datensatz.Enabled = False
            Me.LinkLabel_WorkDir.Enabled = False

            'Methode deaktivieren
            Me.Label_Methode.Enabled = False
            Me.ComboBox_Methode.Enabled = False

            'Ergebnis-Buttons
            Me.Button_saveMDB.Enabled = False
            Me.Button_openMDB.Enabled = False
            Me.Button_Scatterplot.Enabled = False

            'EVO_Settings zurücksetzen
            Me.EVO_Einstellungen1.Enabled = False
            Me.EVO_Einstellungen1.isSaved = False

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


                Case ANW_SMUSI 'Anwendung Smusi
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse Smusi initialisieren
                    Sim1 = New Smusi()


                Case ANW_SCAN 'Anwendung S:CAN
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse Scan initialisieren
                    Sim1 = New Scan()


                Case ANW_SWMM   'Anwendung SWMM
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse SWMM initialisieren
                    Sim1 = New SWMM()


                Case ANW_TESTPROBLEME 'Anwendung Testprobleme
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Testprobleme instanzieren
                    If (isNothing(Testprobleme1)) Then
                        Testprobleme1 = New Testprobleme()
                    End If

                    'Testprobleme anzeigen
                    Call Me.Testprobleme1.Show()

                    'EVO_Einstellungen aktivieren
                    EVO_Einstellungen1.Enabled = True

                    Call EVO_Einstellungen1.setStandard_PES(Testprobleme1.OptModus)

                    'Globale Parameter werden gesetzt
                    Call Testprobleme1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel, globalAnzRand, myPara)

                    'Start-Button aktivieren (keine Methodenauswahl erforderlich)
                    Button_Start.Enabled = True


                Case ANW_TSP 'Anwendung Traveling Salesman Problem (TSP)
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    TSP1 = New TSP()

                    Call TSP1.TSP_Initialize(DForm.Diag)

                    'Start-Button aktivieren (keine Methodenauswahl erforderlich)
                    Button_Start.Enabled = True

            End Select

            'Bei Simulationsanwendungen
            If (Me.Anwendung <> ANW_TESTPROBLEME And Anwendung <> ANW_TSP) Then
                
                'Datensatz aktivieren
                Me.Label_Datensatz.Enabled = True
                Me.LinkLabel_WorkDir.Enabled = True

                'Datensatz anzeigen
                Call Me.displayWorkDir()

                'Methode aktivieren
                Me.Label_Methode.Enabled = True
                Me.ComboBox_Methode.Enabled = True
            End If

            'EVO_Verlauf zurücksetzen
            Call Me.EVO_Opt_Verlauf1.Initialisieren(EVO_Einstellungen1.Settings.PES.Pop.n_Runden, EVO_Einstellungen1.Settings.PES.Pop.n_Popul, EVO_Einstellungen1.Settings.PES.n_Gen, EVO_Einstellungen1.Settings.PES.n_Nachf)

            'Mauszeiger wieder normal
            Cursor = Cursors.Default

        End If

    End Sub

    'Methode wurde ausgewählt
    '************************
    Private Sub INI_Method(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Methode.SelectedIndexChanged

        If (Me.IsInitializing = True) Then

            Exit Sub

        Else

            'Diagramm zurücksetzen
            Me.DForm.Diag.Reset()

            'Alles deaktivieren, danach je nach Methode aktivieren
            '-----------------------------------------------------

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Ergebnis-Buttons
            Me.Button_saveMDB.Enabled = False
            Me.Button_openMDB.Enabled = False
            Me.Button_Scatterplot.Enabled = False

            'EVO_Einstellungen deaktivieren
            EVO_Einstellungen1.Enabled = False

            'EVO_Einstellungen zurücksetzen
            EVO_Einstellungen1.isSaved = False

            'Mauszeiger busy
            Cursor = Cursors.WaitCursor

            'Methode setzen
            Form1.Method = ComboBox_Methode.SelectedItem

            Select Case Form1.Method

                Case "" 'Keine Methode ausgewählt
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

                    SensiPlot1 = New SensiPlot()

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

                    'Ergebnis-Buttons
                    Me.Button_openMDB.Enabled = True

                    'PES für Sim vorbereiten
                    Call Sim1.read_and_valid_INI_Files_PES()

                    'EVO_Einstellungen einrichten
                    EVO_Einstellungen1.Enabled = True
                    Me.EVO_Einstellungen1.TabControl1.SelectedTab = Me.EVO_Einstellungen1.TabPage_PES
                    If (Sim1.List_OptZiele.GetLength(0) = 1) Then
                        Call EVO_Einstellungen1.setStandard_PES(Kern.EVO_MODUS.Single_Objective)
                    ElseIf (Sim1.List_OptZiele.GetLength(0) > 1) Then
                        Call EVO_Einstellungen1.setStandard_PES(Kern.EVO_MODUS.Multi_Objective)
                    End If

                    'Parameterübergabe an PES
                    Call Sim1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel, globalAnzRand, myPara)

                    'EVO_Verlauf zurücksetzen
                    Call Me.EVO_Opt_Verlauf1.Initialisieren(EVO_Einstellungen1.Settings.PES.Pop.n_Runden, EVO_Einstellungen1.Settings.PES.Pop.n_Popul, EVO_Einstellungen1.Settings.PES.n_Gen, EVO_Einstellungen1.Settings.PES.n_Nachf)


                Case METH_HOOKJEEVES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'EVO_Einstellungen einrichten
                    Me.EVO_Einstellungen1.Enabled = True
                    Me.EVO_Einstellungen1.TabControl1.SelectedTab = Me.EVO_Einstellungen1.TabPage_HookeJeeves
                    'Nur SO möglich
                    If (Sim1.List_OptZiele.GetLength(0) = 1) Then
                        Call EVO_Einstellungen1.setStandard_HJ()
                    ElseIf Sim1.List_OptZiele.GetLength(0) > 1 Then
                        Throw New Exception("Methode von Hook und Jeeves erlaubt nur SO-Optimierung!")
                    End If

                    'TODO: eigenen read and valid methode für hookJeeves
                    Call Sim1.read_and_valid_INI_Files_PES()

                    'TODO: eigenen Parameterübergabe an HookJeeves (evtl.überladen von Parameter_Uebergabe)
                    Call Sim1.Parameter_Uebergabe(globalAnzPar, globalAnzZiel, globalAnzRand, myPara)


                Case METH_CES, METH_HYBRID 'Methode CES und Methode CES_PES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Ergebnis-Buttons
                    Me.Button_openMDB.Enabled = True

                    'Funktioniert nur bei BlueM!
                    If (Not Anwendung = ANW_BLUEM) Then
                        Throw New Exception("CES funktioniert bisher nur mit BlueM!")
                    End If

                    'Fallunterscheidung CES oder Hybrid
                    Select Case Form1.Method
                        Case METH_CES
                            'CES für Sim vorbereiten (Files lesen und Validieren)
                            Call Sim1.read_and_valid_INI_Files_CES()

                        Case METH_HYBRID

                            'Original ModellParameter schreiben
                            Call Sim1.Write_ModellParameter()

                            'EVO_Einstellungen aktiviern
                            EVO_Einstellungen1.Enabled = True

                            'CES für Sim vorbereiten (Files lesen und Validieren)
                            Call Sim1.read_and_valid_INI_Files_HYBRID()
                    End Select

                    'EVO_Einstellungen einrichten
                    '****************************
                    EVO_Einstellungen1.Enabled = True
                    Me.EVO_Einstellungen1.TabControl1.SelectedTab = Me.EVO_Einstellungen1.TabPage_CES

                    'Je nach Methode nur CES oder HYBRID
                    Call EVO_Einstellungen1.setStandard_CES()

                    'Je nach Anzahl der Zielfunktionen von MO auf SO umschalten PES
                    If (Sim1.List_OptZiele.GetLength(0) = 1) Then
                        Call EVO_Einstellungen1.setStandard_PES(Kern.EVO_MODUS.Single_Objective)
                    ElseIf (Sim1.List_OptZiele.GetLength(0) > 1) Then
                        Call EVO_Einstellungen1.setStandard_PES(Kern.EVO_MODUS.Multi_Objective)
                    End If

                    'Bei Testmodus wird die Anzahl der Kinder und Generationen überschrieben
                    If Not Sim1.CES_T_Modus = Kern.CES_T_MODUS.No_Test
                        call EVO_Einstellungen1.setTestModus(Sim1.CES_T_Modus, Sim1.TestPath, 1 ,1 ,Sim1.n_Combinations)
                    End If

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

        Dim DiagResult As DialogResult

        'Dialog vorbereiten
        OpenFileDialog1.Filter = "ALL-Dateien (*.ALL)|*.ALL|INP-Dateien (*.INP)|*.INP"
        OpenFileDialog1.Title = "Datensatz auswählen"

        'Alten Datensatz dem Dialog zuweisen
        OpenFileDialog1.InitialDirectory = Sim1.WorkDir
        OpenFileDialog1.FileName = Sim1.WorkDir & Sim1.Datensatz & Sim1.Datensatzendung

        'Dialog öffnen
        DiagResult = OpenFileDialog1.ShowDialog()

        'Neuen Datensatz speichern
        If (DiagResult = Windows.Forms.DialogResult.OK) Then
            Call Sim1.saveDatensatz(OpenFileDialog1.FileName)
        End If

        'Methodenauswahl wieder zurücksetzen (Der Benutzer muss zuerst Ini neu ausführen!)
        Me.ComboBox_Methode.SelectedItem = ""

    End Sub

    'Arbeitsverzeichnis wurde geändert -> Anzeige aktualisieren
    '**********************************************************
    Private Sub displayWorkDir() Handles Sim1.WorkDirChange

        Dim pfad As String
        pfad = Sim1.WorkDir & Sim1.Datensatz & Sim1.Datensatzendung

        'Datensatzanzeige aktualisieren
        If (File.Exists(pfad)) Then
            Me.LinkLabel_WorkDir.Text = pfad
            Me.LinkLabel_WorkDir.Links(0).LinkData = Sim1.WorkDir
        Else
            Me.LinkLabel_WorkDir.Text = "bitte Datensatz auswählen!"
            Me.LinkLabel_WorkDir.Links(0).LinkData = CurDir()
        End If

    End Sub

    'EVO_Einstellungen laden
    '***********************
    Friend Sub Load_EVO_Settings(ByVal sender As Object, ByVal e As System.EventArgs)

        'Dialog einrichten
        OpenFileDialog1.Filter = "XML-Dateien (*.xml)|*.xml"
        OpenFileDialog1.FileName = "EVO_Settings.xml"
        OpenFileDialog1.Title = "Einstellungsdatei auswählen"
        If (Not isNothing(Sim1)) Then
            OpenFileDialog1.InitialDirectory = Sim1.WorkDir
        Else
            OpenFileDialog1.InitialDirectory = CurDir()
        End If

        'Dialog anzeigen
        If (OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            Call EVO_Einstellungen1.loadSettings(OpenFileDialog1.FileName)
        End If
    End Sub

    'EVO_Einstellungen speichern
    '***************************
    Friend Sub Save_EVO_Settings(ByVal sender As Object, ByVal e As System.EventArgs)

        'Dialog einrichten
        SaveFileDialog1.Filter = "XML-Dateien (*.xml)|*.xml"
        SaveFileDialog1.FileName = "EVO_Settings.xml"
        SaveFileDialog1.DefaultExt = "xml"
        SaveFileDialog1.Title = "Einstellungsdatei speichern"
        If (Not isNothing(Sim1)) Then
            SaveFileDialog1.InitialDirectory = Sim1.WorkDir
        Else
            SaveFileDialog1.InitialDirectory = CurDir()
        End If

        'Dialog anzeigen
        If (SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            Call EVO_Einstellungen1.saveSettings(SaveFileDialog1.FileName)
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
            If (Not isNothing(Sim1)) Then
                Me.Button_saveMDB.Enabled = True
                Me.Button_Scatterplot.Enabled = True
            End If

            'EVO_Einstellungen temporär speichern
            Dim dir As String
            dir = My.Computer.FileSystem.SpecialDirectories.Temp & "\"
            Call Me.EVO_Einstellungen1.saveSettings(dir & "EVO_Settings.xml")

            'Try

            Select Case Anwendung

                Case ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM

                    Select Case Method
                        Case METH_RESET
                            Call Sim1.launchSim()
                        Case METH_SENSIPLOT
                            Call STARTEN_SensiPlot()
                        Case METH_PES
                            Call STARTEN_PES()
                        Case METH_CES
                            Call STARTEN_CES_or_HYBRID()
                        Case METH_HYBRID
                            Call STARTEN_CES_or_HYBRID()
                        Case METH_HOOKJEEVES
                            Call STARTEN_HookJeeves()
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

        Dim i, j, n, Anz_Sim As Integer
        Dim QN(), RN() As Double
        Dim serie As Steema.TeeChart.Styles.Series
        Dim surface As New Steema.TeeChart.Styles.Surface
        Dim SimReihe As Wave.Zeitreihe
        Dim SimReihen As Collection
        Dim Wave1 As Wave.Wave

        'Instanzieren
        ReDim QN(Sim1.List_OptZiele.GetUpperBound(0))
        ReDim RN(Sim1.List_Constraints.GetUpperBound(0))
        SimReihen = New Collection

        'Parameterübergabe an ES
        Me.globalAnzZiel = 1
        Me.globalAnzRand = 0
        Me.globalAnzPar = SensiPlot1.Selected_OptParameter.GetLength(0)

        'Anzahl Simulationen
        If (Me.globalAnzPar = 1) Then
            '1 Parameter
            Anz_Sim = SensiPlot1.Anz_Steps
        Else
            '2 Parameter
            Anz_Sim = SensiPlot1.Anz_Steps ^ 2
        End If

        'Startwerte werden der Verlaufsanzeige zugewiesen
        Call Me.EVO_Opt_Verlauf1.Initialisieren(1, 1, 1, Anz_Sim)
        Call Me.EVO_Opt_Verlauf1.Runden(1)
        Call Me.EVO_Opt_Verlauf1.Population(1)
        Call Me.EVO_Opt_Verlauf1.Generation(1)

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

        n = 0

        'Äussere Schleife (2. OptParameter)
        '----------------------------------
        For i = 0 To ((SensiPlot1.Anz_Steps - 1) * (Me.globalAnzPar - 1))

            '2. OptParameterwert variieren
            If (Me.globalAnzPar > 1) Then
                Select Case SensiPlot1.Selected_SensiType
                    Case "Gleichverteilt"
                        Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Xn = Rnd()
                    Case "Diskret"
                        Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Xn = i / SensiPlot1.Anz_Steps
                End Select
            End If

            'Innere Schleife (1. OptParameter)
            '---------------------------------
            For j = 0 To SensiPlot1.Anz_Steps - 1

                '1. OptParameterwert variieren
                Select Case SensiPlot1.Selected_SensiType
                    Case "Gleichverteilt"
                        Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Xn = Rnd()
                    Case "Diskret"
                        Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Xn = j / SensiPlot1.Anz_Steps
                End Select

                n += 1

                'Modellparameter schreiben
                Call Sim1.Write_ModellParameter()

                'Verlauf aktualisieren
                Me.EVO_Opt_Verlauf1.Nachfolger(n)

                'Evaluieren
                Call Sim1.SIM_Evaluierung(kern.Individuum.QN_RN_Indi(n, QN, RN, sim1.List_OptParameter))

                'BUG 253: Verletzte Constraints bei SensiPlot kenntlich machen?

                'Diagramm aktualisieren
                If (Me.globalAnzPar = 1) Then
                    '1 Parameter
                    serie = DForm.Diag.getSeriesPoint("SensiPlot", "Orange")
                    serie.Add(Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).QWertTmp, Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).RWert, n)
                Else
                    '2 Parameter
                    surface.Add(Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).RWert, Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).QWertTmp, Sim1.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).RWert, n)
                End If

                'Simulationsergebnis in Wave laden
                If (SensiPlot1.show_Wave) Then
                    'SimReihe auslesen
                    SimReihe = Sim1.SimErgebnis(Sim1.List_OptZiele(SensiPlot1.Selected_OptZiel).SimGr)
                    'Lösungs-ID an Titel anhängen
                    SimReihe.Title += " (Lösung " & n.ToString() & ")"
                    'SimReihe zu Collection hinzufügen
                    SimReihen.Add(SimReihe)
                End If

                System.Windows.Forms.Application.DoEvents()

            Next
        Next

        'Wave Diagramm anzeigen:
        '-----------------------
        If (SensiPlot1.show_Wave) Then
            Wave1 = New Wave.Wave()
            For Each zre As Wave.Zeitreihe In SimReihen
                Wave1.Display_Series(zre)
            Next
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
    Private Sub STARTEN_CES_or_HYBRID()

        'CES initialisieren
        '******************
        CES1 = New EVO.Kern.CES()
        Call Ces1.CESInitialise(EVO_Einstellungen1.Settings, Method, sim1.CES_T_Modus, Sim1.List_OptZiele.GetLength(0), Sim1.List_Constraints.GetLength(0), Sim1.List_Locations.GetLength(0), Sim1.VerzweigungsDatei.GetLength(0), sim1.n_Combinations, sim1.n_PathDimension)
        
        'Die alten Bekannten
        globalAnzZiel = CES1.ModSett.n_Penalty
        globalAnzRand = CES1.ModSett.n_Constrain

        'EVO_Verlauf zurücksetzen
        '************************
        Call Me.EVO_Opt_Verlauf1.Initialisieren(1, 1, EVO_Einstellungen1.Settings.CES.n_Generations, EVO_Einstellungen1.Settings.CES.n_Childs)

        Dim durchlauf_all As Integer = 0
        Dim serie As Steema.TeeChart.Styles.Series

        'Laufvariable für die Generationen
        Dim i_gen, i_ch, i_loc As Integer
        Dim m As Integer
        
        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        'Zufällige Kinderpfade werden generiert
        '**************************************
        Call CES1.Generate_Random_Path()
        'Falls TESTMODUS werden sie überschrieben
        If Not Sim1.CES_T_Modus = Kern.CES_T_MODUS.No_Test
            Call CES1.Generate_Paths_for_Tests(sim1.TestPath, sim1.CES_T_Modus)
        End If
        '**************************************

        'Hier werden dem Child die passenden Massnahmen und deren Elemente pro Location zugewiesen
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        For i_ch = 0 To CES1.Settings.CES.n_Childs - 1
            For i_loc = 0 To CES1.ModSett.n_Locations - 1
                Call Sim1.Identify_Measures_Elements_Parameters(i_loc, CES1.Childs(i_ch).Path(i_loc), CES1.Childs(i_ch).Measures(i_loc), CES1.Childs(i_ch).Loc(i_loc).Loc_Elem, CES1.Childs(i_ch).Loc(i_loc).PES_OptPara)
            Next
        Next

        'Falls HYBRID werden entprechend der Einstellung im PES die Parameter auf Zufällig oder Start gesetzt
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        If Method = METH_HYBRID AND EVO_Einstellungen1.Settings.CES.ty_Hybrid = EVO.Kern.HYBRID_TYPE.Mixed_Integer Then
            CES1.Set_Xn_And_Dn_per_Location()
        End If

        'Startwerte werden der Verlaufsanzeige zugewiesen
        Call Me.EVO_Opt_Verlauf1.Initialisieren(1, 1, EVO_Einstellungen1.Settings.CES.n_Generations, EVO_Einstellungen1.Settings.CES.n_Childs)

        'xxxx Optimierung xxxxxx
        'Generationsschleife CES
        'xxxxxxxxxxxxxxxxxxxxxxx
        For i_gen = 0 To CES1.Settings.CES.n_Generations - 1

            Call EVO_Opt_Verlauf1.Generation(i_gen + 1)

            'Child Schleife
            'xxxxxxxxxxxxxx
            For i_ch = 0 To CES1.Settings.CES.n_Childs - 1

                durchlauf_all += 1
                CES1.Childs(i_ch).ID = durchlauf_all
                Call EVO_Opt_Verlauf1.Nachfolger(i_ch + 1)

                '****************************************
                'Aktueller Pfad wird an Sim zurückgegeben
                'Bereitet das BlaueModell für die Kombinatorik vor
                Call Sim1.PREPARE_Evaluation_CES(CES1.Childs(i_ch).Path, CES1.Childs(i_ch).All_Elem)

                'HYBRID: Bereitet für die Optimierung mit den PES Parametern vor
                '***************************************************************
                If Method = METH_HYBRID AND EVO_Einstellungen1.Settings.CES.ty_Hybrid = EVO.Kern.HYBRID_TYPE.Mixed_Integer Then
                    if Sim1.Reduce_OptPara_and_ModPara(CES1.Childs(i_ch).All_Elem) then
                        Call Sim1.PREPARE_Evaluation_PES(CES1.Childs(i_ch).All_Loc_Para)
                    End If
                End If

                'Simulation *************************************************************************
                Call Sim1.SIM_Evaluierung(CES1.Childs(i_ch))
                '************************************************************************************

                'HYBRID: Speichert die PES Erfahrung diesen Childs im PES Memory
                '***************************************************************
                If Method = METH_HYBRID AND EVO_Einstellungen1.Settings.CES.ty_Hybrid = EVO.Kern.HYBRID_TYPE.Mixed_Integer Then
                    Call CES1.Memory_Store(i_ch, i_gen)
                End If

                'Lösung im TeeChart einzeichnen
                '==============================
                If (CES1.ModSett.n_Penalty = 1) Then
                    'SingleObjective
                    '---------------
                    serie = DForm.Diag.getSeriesPoint("Childs", "Orange")
                    Call serie.Add(durchlauf_all, CES1.Childs(i_ch).Penalty(0), durchlauf_all.ToString())
                ElseIf (CES1.ModSett.n_Penalty = 2) Then
                    'MultiObjective 2D-Diagramm
                    '--------------------------
                    serie = DForm.Diag.getSeriesPoint("Childs", "Orange")
                    Call serie.Add(CES1.Childs(i_ch).Penalty(0), CES1.Childs(i_ch).Penalty(1), durchlauf_all.ToString())
                ElseIf (CES1.ModSett.n_Penalty = 3) Then
                    'MultiObjective 3D-Diagramm (Es werden die ersten drei Zielfunktionswerte eingezeichnet)
                    '---------------------------------------------------------------------------------------
                    Dim serie3D As Steema.TeeChart.Styles.Points3D
                    serie3D = DForm.Diag.getSeries3DPoint("Childs", "Orange")
                    Call serie3D.Add(CES1.Childs(i_ch).Penalty(0), CES1.Childs(i_ch).Penalty(1), CES1.Childs(i_ch).Penalty(2), durchlauf_all.ToString())
                End If

                System.Windows.Forms.Application.DoEvents()
            Next
            '^ ENDE der Child Schleife
            'xxxxxxxxxxxxxxxxxxxxxxx

            'Die Listen müssen nach der letzten Evaluierung wieder zurückgesetzt werden
            'Sicher ob das benötigt wird?
            Call Sim1.Reset_OptPara_and_ModPara()

            'MO oder SO SELEKTIONSPROZESS oder NDSorting SELEKTION
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            'BUG 259: CES: Punkt-Labels der Sekundärpopulation fehlen noch!
            If CES1.ModSett.n_Penalty = 1 Then
                'Sortieren der Kinden anhand der Qualität
                Call CES1.Sort_Individuum(CES1.Childs)
                'Selectionsprozess je nach "plus" oder "minus" Strategie
                Call CES1.Selection_Process()
                'Zeichnen der besten Eltern
                For i_ch = 0 To EVO_Einstellungen1.Settings.CES.n_Parents - 1
                    'durchlauf += 1
                    serie = DForm.Diag.getSeriesPoint("Parent", "green")
                    Call serie.Add(durchlauf_all, CES1.Parents(i_ch).Penalty(0))
                Next
            Else
                'NDSorting ******************
                Call CES1.NDSorting_CES_Control(i_gen)

                'Sekundäre Population
                SekPopulation = CES1.SekundärQb_Get()
                If (Not IsNothing(Sim1)) Then
                    'SekPop abspeichern
                    Call Sim1.OptResult.setSekPop(SekPopulation, i_gen)
                    'SekPop mit Solution.IDs zeichnen
                    Call SekundärePopulationZeichnen(i_gen)
                Else
                    'SekPop einfach so zeichnen
                    Call SekundärePopulationZeichnen(SekPopulation)
                End If
            End If
            ' ^ ENDE Selectionsprozess
            'xxxxxxxxxxxxxxxxxxxxxxxxx

            'REPRODUKTION und MUTATION Nicht wenn Testmodus
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            If sim1.CES_T_Modus = kern.CES_T_MODUS.No_Test Then
                'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
                Call Kern.Individuum.New_Array("Child", CES1.Childs)
                'Reproduktionsoperatoren, hier gehts dezent zur Sache
                Call CES1.Reproduction_Control()
                'Mutationsoperatoren
                Call CES1.Mutation_Control()
            End If

            'Hier werden dem Child die passenden Massnahmen und deren Elemente pro Location zugewiesen
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            For i_ch = 0 To EVO_Einstellungen1.Settings.CES.n_Childs - 1
                For i_loc = 0 To CES1.ModSett.n_Locations - 1
                    Call Sim1.Identify_Measures_Elements_Parameters(i_loc, CES1.Childs(i_ch).Path(i_loc), CES1.Childs(i_ch).Measures(i_loc), CES1.Childs(i_ch).Loc(i_loc).Loc_Elem, CES1.Childs(i_ch).Loc(i_loc).PES_OptPara)
                Next
            Next

            'HYBRID: REPRODUKTION und MUTATION
            '*********************************
            If Method = METH_HYBRID AND EVO_Einstellungen1.Settings.CES.ty_Hybrid = EVO.Kern.HYBRID_TYPE.Mixed_Integer Then
                'NDSorting für den PES Memory
                '****************************
                If CES1.PES_Memory.GetLength(0) > ces1.Settings.CES.n_PES_MemSize
                    Call CES1.NDSorting_Memory(i_gen)
                End If

                'pro Child
                'xxxxxxxxx
                For i_ch = 0 To CES1.Childs.GetUpperBound(0)

                    'Ermittelt fuer jedes Child den PES Parent Satz (PES_Parents ist das Ergebnis)
                    Call CES1.Memory_Search_per_Child(CES1.Childs(i_ch))

                    'und pro Location
                    'xxxxxxxxxxxxxxxx
                    For i_loc = 0 To CES1.ModSett.n_Locations - 1

                        'Die Parameter (falls vorhanden) werden überschrieben
                        If Not CES1.Childs(i_ch).Loc(i_loc).PES_OptPara.GetLength(0) = 0 Then

                            'Ermittelt fuer jede Location den PES Parent Satz (PES_Parents ist das Ergebnis)
                            '*******************************************************************************
                            Call CES1.Memory_Search_per_Location(i_loc)

                            'Führt das NDSorting für diesen Satz durch
                            '*****************************************
                            If CES1.PES_Parents_pLoc.GetLength(0) > CES1.Settings.PES.n_Eltern Then
                                Call CES1.NDSorting_PES_Parents_per_Loc(i_gen)
                            End If

                            Select CES1.PES_Parents_pLoc.GetLength(0)

                                Case = 0
                                    'Noch keine Eltern vorhanden (die Child Location bekommt neue - zufällige Werte oder original Parameter)
                                    '*******************************************************************************************************
                                    For m = 0 To CES1.Childs(i_ch).Loc(i_loc).PES_OptPara.GetUpperBound(0)
                                        CES1.Childs(i_ch).Loc(i_loc).PES_OptPara(m).Dn = CES1.Settings.PES.Schrittweite.DnStart
                                        'Falls zufällige Startwerte
                                        If CES1.Settings.PES.OptStartparameter = Kern.EVO_STARTPARAMETER.Zufall Then
                                            Randomize()
                                            CES1.Childs(i_ch).Loc(i_loc).PES_OptPara(m).Xn = Rnd()
                                        End If
                                    Next
                                
                                Case > 0
                                    'Eltern vorhanden (das PES wird gestartet)
                                    '*****************************************
                                    If CES1.PES_Parents_pLoc.GetLength(0) < CES1.Settings.PES.n_Eltern
                                        'Falls es zu wenige sind wird mit den vorhandenen aufgefüllt
                                        Call CES1.fill_Parents_per_Loc(CES1.PES_Parents_pLoc, CES1.Settings.PES.n_Eltern)
                                    End if

                                    'Schritt 0: PES - Objekt der Klasse PES wird erzeugt PES wird erzeugt
                                    '*********************************************************************
                                    Dim PES1 As EVO.Kern.PES
                                    PES1 = New EVO.Kern.PES

                                    'Vorbereitung um das PES zu initieren
                                    '************************************
                                    globalAnzPar = CES1.Childs(i_ch).Loc(i_loc).PES_OptPara.GetLength(0)
                                    myPara = CES1.Childs(i_ch).Loc(i_loc).PES_OptPara.Clone

                                    'Schritte 1 - 3: PES wird initialisiert (Weiteres siehe dort ;-)
                                    '**************************************************************
                                    Call PES1.PesInitialise(EVO_Einstellungen1.Settings, globalAnzPar, globalAnzZiel, globalAnzRand, myPara, Method)

                                    'Die PopulationsEltern des PES werden gefüllt
                                    For m = 0 To CES1.PES_Parents_pLoc.GetUpperBound(0)
                                        Call PES1.EsStartvalues(CES1.Settings.CES.is_PopMutStart, CES1.PES_Parents_pLoc(m).Loc(i_loc).PES_OptPara, m)
                                    Next

                                    'Startet die Prozesse evolutionstheoretischen Prozesse nacheinander
                                    Call PES1.EsReproMut(EVO_Einstellungen1.Settings.CES.is_PopMutStart)

                                    'Auslesen der Variierten Parameter
                                    CES1.Childs(i_ch).Loc(i_loc).PES_OptPara = PES1.EsGetParameter()

                            End Select
                        End If
                    Next
                Next
            End If
            ' ^ Ende der HYBRID PES Schleife
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        Next
        'Ende der Generationsschleife CES

        'Falls jetzt noch PES ausgeführt werden soll
        'Starten der PES mit der Front von CES
        '*******************************************
        If Method = METH_HYBRID AND EVO_Einstellungen1.Settings.CES.ty_Hybrid = EVO.Kern.HYBRID_TYPE.Sequencial_1
            Call Start_PES_after_CES()
        End If

    End Sub

    'Starten der PES mit der Front von CES
    '(MaxAnzahl ist die Zahl der Eltern -> ToDo: SecPop oder Bestwertspeicher)
    '*************************************************************************
    Private Sub Start_PES_after_CES()
        Dim i As Integer

        For i = 0 To EVO_Einstellungen1.Settings.CES.n_Parents - 1
            If CES1.Parents(i).Front = 1 Then

                '****************************************
                'Aktueller Pfad wird an Sim zurückgegeben
                'Bereitet das BlaueModell für die Kombinatorik vor
                Call Sim1.PREPARE_Evaluation_CES(CES1.Childs(i).Path, CES1.Childs(i).All_Elem)

                'Hier werden Child die passenden Elemente zugewiesen
                Dim j As Integer
                For j = 0 To CES1.ModSett.n_Locations - 1
                    Call Sim1.Identify_Measures_Elements_Parameters(j, CES1.Childs(i).Path(j), CES1.Childs(i).Measures(j), CES1.Childs(i).Loc(j).Loc_Elem, CES1.Childs(i).Loc(j).PES_OptPara)
                Next

                'Reduktion der OptimierungsParameter und immer dann wenn nicht Nullvariante
                'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                If Sim1.Reduce_OptPara_and_ModPara(CES1.Childs(i).All_Elem) Then

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

    'Anwendung des Verfahrens von Hook und Jeeves zur Parameteroptimierung
    '*********************************************************************
    Private Sub STARTEN_HookJeeves()
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim b As Boolean
        Dim QN() As Double = {}
        Dim QNBest() As Double = {}
        Dim QBest() As Double = {}
        Dim RN() As Double
        Dim aktuellePara(globalAnzPar - 1) As Double
        Dim SIM_Eval_is_OK As Boolean
        Dim durchlauf As Long
        Dim Iterationen As Long
        Dim Tastschritte_aktuell As Long
        Dim Tastschritte_gesamt As Long
        Dim Extrapolationsschritte As Long
        Dim Rueckschritte As Long

        Dim HookJeeves As EVO.Kern.HookeAndJeeves = New EVO.Kern.HookeAndJeeves(globalAnzPar, EVO_Einstellungen1.Settings.HookJeeves.DnStart, EVO_Einstellungen1.Settings.HookJeeves.DnFinish)

        ReDim QN(globalAnzZiel - 1)
        ReDim QNBest(globalAnzZiel - 1)
        ReDim QBest(globalAnzZiel - 1)
        ReDim RN(-1)

        'Diagramm vorbereiten und initialisieren

        Call PrepareDiagramm()

        durchlauf = 0
        Tastschritte_aktuell = 0
        Tastschritte_gesamt = 0
        Extrapolationsschritte = 0
        Rueckschritte = 0
        Iterationen = 0
        b = False

        Call HookJeeves.Initialize(Kern.OptParameter.MyParaDouble(myPara))
        'Initialisierungssimulation
        myPara.CopyTo(aktuellePara, 0)
        QNBest(0) = 1.79E+308
        QBest(0) = 1.79E+308
        k = 0

        'TODO: Wie kannst du hier 2 Arrays miteinander vergleichen?
        Do While (HookJeeves.AktuelleSchrittweite > HookJeeves.MinimaleSchrittweite)
            Iterationen += 1
            'Bestimmen der Ausgangsgüte
            'Vorbereiten des Modelldatensatzes
            Call Sim1.PREPARE_Evaluation_PES(aktuellePara)
            'Evaluierung des Simulationsmodells (ToDo: Validätsprüfung fehlt)
            durchlauf += 1
            SIM_Eval_is_OK = Sim1.SIM_Evaluierung(Kern.Individuum.QN_RN_Indi(durchlauf, QN, RN, aktuellePara))
            'Lösung im TeeChart einzeichnen
            '==============================
            Dim serie As Steema.TeeChart.Styles.Series
            serie = DForm.Diag.getSeriesPoint("Hook and Jeeves".ToString())
            Call serie.Add(durchlauf, QN(0), durchlauf.ToString())
            QN.CopyTo(QNBest, 0)

            'Tastschritte
            For j = 0 To HookJeeves.AnzahlParameter - 1
                aktuellePara = HookJeeves.Tastschritt(j, Kern.HookeAndJeeves.TastschrittRichtung.Vorwärts)
                Tastschritte_aktuell += 1
                Me.EVO_Einstellungen1.LabelTSHJaktuelle.Text = Tastschritte_aktuell.ToString
                'Vorbereiten des Modelldatensatzes
                Call Sim1.PREPARE_Evaluation_PES(aktuellePara)
                'Evaluierung des Simulationsmodells
                durchlauf += 1
                SIM_Eval_is_OK = Sim1.SIM_Evaluierung(Kern.Individuum.QN_RN_Indi(durchlauf, QN, RN, aktuellePara))
                serie = DForm.Diag.getSeriesPoint("Hook and Jeeves".ToString())
                Call serie.Add(durchlauf, QN(0), durchlauf.ToString())
                If QN(0) >= QNBest(0) Then
                    aktuellePara = HookJeeves.Tastschritt(j, Kern.HookeAndJeeves.TastschrittRichtung.Rückwärts)
                    Tastschritte_aktuell += 1
                    Me.EVO_Einstellungen1.LabelTSHJaktuelle.Text = Tastschritte_aktuell.ToString
                    'Vorbereiten des Modelldatensatzes
                    Call Sim1.PREPARE_Evaluation_PES(aktuellePara)
                    'Evaluierung des Simulationsmodells
                    durchlauf += 1
                    SIM_Eval_is_OK = Sim1.SIM_Evaluierung(Kern.Individuum.QN_RN_Indi(durchlauf, QN, RN, aktuellePara))
                    serie = DForm.Diag.getSeriesPoint("Hook and Jeeves".ToString())
                    Call serie.Add(durchlauf, QN(0), durchlauf.ToString())
                    If QN(0) >= QNBest(0) Then
                        aktuellePara = HookJeeves.TastschrittResetParameter(j)
                    Else
                        QN.CopyTo(QNBest, 0)
                    End If
                Else
                    QN.CopyTo(QNBest, 0)
                End If
            Next

            Tastschritte_gesamt += Tastschritte_aktuell
            Me.EVO_Einstellungen1.LabelTSHJgesamt.Text = Tastschritte_gesamt.ToString
            Tastschritte_aktuell = 0
            Me.EVO_Einstellungen1.LabelTSHJaktuelle.Text = Tastschritte_aktuell.ToString
            Me.EVO_Einstellungen1.LabelTSHJmittel.Text = Math.Round((Tastschritte_gesamt / Iterationen), 2).ToString

            'Extrapolationsschritt
            If QNBest(0) < QBest(0) Then

                serie = DForm.Diag.getSeriesPoint("Hook and Jeeves Best".ToString(), "GREEN")
                Call serie.Add(durchlauf, QN(0), durchlauf.ToString())

                QNBest.CopyTo(QBest, 0)
                Call HookJeeves.Extrapolationsschritt()
                Extrapolationsschritte += 1
                Me.EVO_Einstellungen1.LabelESHJ.Text = Extrapolationsschritte.ToString
                k += 1
                aktuellePara = HookJeeves.getLetzteParameter
                For i = 0 To HookJeeves.AnzahlParameter - 1
                    If aktuellePara(i) < 0 Or aktuellePara(i) > 1 Then
                        HookJeeves.Rueckschritt()
                        Rueckschritte += 1
                        Me.EVO_Einstellungen1.LabelRSHJ.Text = Rueckschritte.ToString
                        k += -1
                        HookJeeves.Schrittweitenhalbierung()
                        aktuellePara = HookJeeves.getLetzteParameter
                        Exit For
                    End If
                Next
                b = True
            Else
                'If b Then
                '    HookJeeves.Rueckschritt()
                '    b = False
                'Else
                '    HookJeeves.Schrittweitenhalbierung()
                'End If
                If k > 0 Then
                    HookJeeves.Rueckschritt()
                    Me.EVO_Einstellungen1.LabelRSHJ.Text = Rueckschritte.ToString
                    HookJeeves.Schrittweitenhalbierung()
                    aktuellePara = HookJeeves.getLetzteParameter
                Else
                    HookJeeves.Schrittweitenhalbierung()
                End If
            End If
        Loop
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

        'Dim Hypervolume As EVO.Kern.Hypervolumen
        'Hypervolume = New EVO.Kern.Hypervolumen
        'Hypervolume.Dimension = globalAnzZiel
        'Hypervolume.Normalisiert = True
        'Dim HV as double

        'TODO: If (ipop + igen + inachf + irunde) > 4 Then GoTo Start_Evolutionsrunden '????? Wie?
        'Werte an Variablen übergeben auskommentiert Werte finden sich im PES werden hier aber nicht zugewiesen
        'Kann der Kommentar nicht weg?
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        ReDim QN(globalAnzZiel - 1)
        ReDim RN(globalAnzRand - 1)

        'Diagramm vorbereiten und initialisieren
        If (Not Form1.Method = METH_HYBRID And Not EVO_Einstellungen1.Settings.CES.ty_Hybrid = EVO.Kern.HYBRID_TYPE.Sequencial_1) Then
            Call PrepareDiagramm()
        End If

        'Individuum wird initialisiert
        Call Kern.Individuum.Initialise(1, 0, globalAnzPar, globalAnzZiel, globalAnzRand)


        'Schritte 0: Objekt der Klasse PES wird erzeugt
        '**********************************************
        Dim PES1 As EVO.Kern.PES
        PES1 = New EVO.Kern.PES()

        'Schritte 1 - 3: ES wird initialisiert (Weiteres siehe dort ;-)
        '**************************************************************
        Call PES1.PesInitialise(EVO_Einstellungen1.Settings, globalAnzPar, globalAnzZiel, globalAnzRand, myPara, Method)

        'Startwerte werden der Verlaufsanzeige zugewiesen
        Call Me.EVO_Opt_Verlauf1.Initialisieren(EVO_Einstellungen1.Settings.PES.Pop.n_Runden, EVO_Einstellungen1.Settings.PES.Pop.n_Popul, EVO_Einstellungen1.Settings.PES.n_Gen, EVO_Einstellungen1.Settings.PES.n_Nachf)

        durchlauf = 0

        'Hypervolume wird initialisiert


Start_Evolutionsrunden:

        'Über alle Runden
        'xxxxxxxxxxxxxxxx
        For PES1.PES_iAkt.iAktRunde = 0 To EVO_Einstellungen1.Settings.PES.Pop.n_Runden - 1

            Call EVO_Opt_Verlauf1.Runden(PES1.PES_iAkt.iAktRunde + 1)
            Call PES1.EsResetPopBWSpeicher() 'Nur bei Komma Strategie

            'Über alle Populationen
            'xxxxxxxxxxxxxxxxxxxxxx
            For PES1.PES_iAkt.iAktPop = 0 To EVO_Einstellungen1.Settings.PES.Pop.n_Popul - 1

                Call EVO_Opt_Verlauf1.Population(PES1.PES_iAkt.iAktPop + 1)

                'POPULATIONS REPRODUKTIONSPROZESS
                '################################
                'Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern der Population
                Call PES1.EsPopReproduktion()

                'POPULATIONS MUTATIONSPROZESS
                '############################
                'Mutieren der Ausgangswerte der Population
                Call PES1.EsPopMutation()

                'Über alle Generationen
                'xxxxxxxxxxxxxxxxxxxxxx
                For PES1.PES_iAkt.iAktGen = 0 To EVO_Einstellungen1.Settings.PES.n_Gen - 1

                    Call EVO_Opt_Verlauf1.Generation(PES1.PES_iAkt.iAktGen + 1)
                    Call PES1.EsResetBWSpeicher()  'Nur bei Komma Strategie

                    'Über alle Nachkommen
                    'xxxxxxxxxxxxxxxxxxxxxxxxx
                    For PES1.PES_iAkt.iAktNachf = 0 To EVO_Einstellungen1.Settings.PES.n_Nachf - 1

                        Call EVO_Opt_Verlauf1.Nachfolger(PES1.PES_iAkt.iAktNachf + 1)

                        durchlauf += 1

                        'Um Modellfehler bzw. Evaluierungsabbrüche abzufangen
                        'TODO: noch nicht fertig das Ergebnis wird noch nicht auf Fehler ueberprueft
                        Dim Eval_Count As Integer = 0
                        Dim SIM_Eval_is_OK As Boolean = True
                        Do
                            'REPRODUKTIONSPROZESS
                            '####################
                            'Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern
                            Call PES1.EsReproduktion()

                            'MUTATIONSPROZESS
                            '################
                            'Mutieren der Ausgangswerte
                            Call PES1.EsMutation()

                            'Auslesen der Variierten Parameter
                            myPara = PES1.EsGetParameter()

                            'Auslesen des Bestwertspeichers
                            'TODO: Bestwertspeicher wird nicht genutzt!
                            'If (EVO_Einstellungen1.Settings.PES.OptModus = Kern.EVO_MODUS.Single_Objective) Then
                            '    Bestwert = PES1.EsGetBestwert()
                            'End If

                            'Ansteuerung der zu optimierenden Anwendung
                            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                            Select Case Anwendung

                                Case ANW_TESTPROBLEME

                                    Call Testprobleme1.Evaluierung_TestProbleme(myPara, durchlauf, PES1.PES_iAkt.iAktPop, QN, RN, DForm.Diag)

                                Case ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM

                                    'Vorbereiten des Modelldatensatzes
                                    Call Sim1.PREPARE_Evaluation_PES(myPara)

                                    'Evaluierung des Simulationsmodells (ToDo: Validätsprüfung fehlt)
                                    SIM_Eval_is_OK = Sim1.SIM_Evaluierung(kern.Individuum.QN_RN_Indi(durchlauf, QN, RN, myPara))

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

                                    If (globalAnzZiel = 1) Then
                                        'SingleObjective
                                        'xxxxxxxxxxxxxxx
                                        If (isInvalid) Then
                                            serie = DForm.Diag.getSeriesPoint("Population " & (PES1.PES_iAkt.iAktPop + 1).ToString() & " (ungültig)", "Gray")
                                        Else
                                            serie = DForm.Diag.getSeriesPoint("Population " & (PES1.PES_iAkt.iAktPop + 1).ToString())
                                        End If
                                        Call serie.Add(PES1.PES_iAkt.iAktRunde * EVO_Einstellungen1.Settings.PES.n_Gen * EVO_Einstellungen1.Settings.PES.n_Nachf + PES1.PES_iAkt.iAktGen * EVO_Einstellungen1.Settings.PES.n_Nachf + PES1.PES_iAkt.iAktNachf, QN(0), durchlauf.ToString())

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
                                            Call serie.Add(QN(0), QN(1), durchlauf.ToString())

                                        Else
                                            '3D-Diagramm (Es werden die ersten drei Zielfunktionswerte eingezeichnet)
                                            '------------------------------------------------------------------------
                                            Dim serie3D As Steema.TeeChart.Styles.Points3D
                                            If (isInvalid) Then
                                                serie3D = DForm.Diag.getSeries3DPoint("Population" & " (ungültig)", "Gray")
                                            Else
                                                serie3D = DForm.Diag.getSeries3DPoint("Population", "Orange")
                                            End If
                                            Call serie3D.Add(QN(0), QN(1), QN(2), durchlauf.ToString())

                                        End If
                                    End If

                            End Select

                            Eval_Count += 1
                            If (Eval_Count >= 10) Then
                                Throw New Exception("Es konnte kein gültiger Datensatz erzeugt werden!")
                            End If

                        Loop While SIM_Eval_is_OK = False

                        'SELEKTIONSPROZESS Schritt 1
                        '###########################
                        'Einordnen der Qualitätsfunktion im Bestwertspeicher bei SO
                        'Falls MO Einordnen der Qualitätsfunktion in NDSorting
                        Call PES1.EsBest(QN, RN)

                        System.Windows.Forms.Application.DoEvents()

                    Next 'Ende Schleife über alle Nachkommen

                    'SELEKTIONSPROZESS Schritt 2 für NDSorting sonst Xe = Xb
                    '#######################################################
                    'Die neuen Eltern werden generiert
                    Call PES1.EsEltern()

                    'Sekundäre Population
                    If (EVO_Einstellungen1.Settings.PES.OptModus = Kern.EVO_MODUS.Multi_Objective) Then
                        SekPopulation = PES1.SekundärQb_Get()
                        If (Not IsNothing(Sim1)) Then
                            'SekPop abspeichern
                            Call Sim1.OptResult.setSekPop(SekPopulation, PES1.PES_iAkt.iAktGen)
                            'SekPop mit Solution.IDs zeichnen
                            Call SekundärePopulationZeichnen(PES1.PES_iAkt.iAktGen)
                        Else
                            'SekPop einfach so zeichnen
                            Call SekundärePopulationZeichnen(SekPopulation)
                        End If
                    End If

                    System.Windows.Forms.Application.DoEvents()

                    'Serie im TeeChart löschen
                    '==============================
                    If EVO_Einstellungen1.Settings.PES.is_paint_constraint Then
                        Dim serie As Steema.TeeChart.Styles.Series

                        If (globalAnzZiel = 1) Then
                            'SingleObjective
                            'xxxxxxxxxxxxxxx
                            serie = DForm.Diag.getSeriesPoint("Population " & (PES1.PES_iAkt.iAktPop + 1).ToString() & " (ungültig)", "Gray")
                            serie.Clear()
                            serie = DForm.Diag.getSeriesPoint("Population " & (PES1.PES_iAkt.iAktPop + 1).ToString())
                            serie.Clear()
                        Else
                            'MultiObjective
                            'xxxxxxxxxxxxxx
                            If (globalAnzZiel = 2) Then
                                '2D-Diagramm
                                '------------------------------------------------------------------------
                                serie = DForm.Diag.getSeriesPoint("Population" & " (ungültig)", "Gray")
                                serie.Clear()
                                serie = DForm.Diag.getSeriesPoint("Population", "Orange")
                                serie.Clear()
                            Else
                                '3D-Diagramm (Es werden die ersten drei Zielfunktionswerte eingezeichnet)
                                '------------------------------------------------------------------------
                                Dim serie3D As Steema.TeeChart.Styles.Points3D
                                serie3D = DForm.Diag.getSeries3DPoint("Population" & " (ungültig)", "Gray")
                                serie3D.Clear()
                                serie3D = DForm.Diag.getSeries3DPoint("Population", "Orange")
                                serie3D.Clear()
                            End If
                        End If
                    End If

                    'Hypervolumenberechnung
                    '======================
                    If (EVO_Einstellungen1.Settings.PES.OptModus = Kern.EVO_MODUS.Multi_Objective _
                        And PES1.PES_iAkt.iAktRunde = 0 _
                        And PES1.PES_iAkt.iAktPop = 0 _
                        And PES1.PES_iAkt.iAktGen = 0) Then

                        'Referenzpunkt für Hypervolumen ermitteln
                        '----------------------------------------
                        Dim j As Integer
                        Dim k As Integer
                        Dim Referenzpunkt(globalAnzZiel - 1) As Double

                        For j = 0 To globalAnzZiel - 1
                            Referenzpunkt(j) = 0
                            For k = 0 To UBound(SekPopulation)
                                If SekPopulation(k, j) > Referenzpunkt(j) Then
                                    Referenzpunkt(j) = SekPopulation(k, j)
                                End If
                            Next
                        Next
                        'Hypervolume.Referenzpunkt = Referenzpunkt

                    Else
                        'Hypervolumen berechnen
                        '----------------------
                        'HV = Hypervolume.GetHypervolume(UBound(SekPopulation), SekPopulation)
                    End If

                Next 'Ende alle Generationen
                'xxxxxxxxxxxxxxxxxxxxxxxxxxx
                System.Windows.Forms.Application.DoEvents()

                'POPULATIONS SELEKTIONSPROZESS  Schritt 1
                '########################################
                'Einordnen der Qualitätsfunktion im PopulationsBestwertspeicher
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

    'Sekundäre Population zeichnen
    '*****************************
    Private Sub SekundärePopulationZeichnen(ByVal SekPop(,) As Double)

        Dim i As Short
        Dim serie As Steema.TeeChart.Styles.Series
        Dim serie3D As Steema.TeeChart.Styles.Points3D

        If (globalAnzZiel = 2) Then
            '2 Zielfunktionen
            '----------------------------------------------------------------
            serie = DForm.Diag.getSeriesPoint("Sekundäre Population", "Green")
            serie.Clear()
            For i = 0 To SekPop.GetUpperBound(0)
                serie.Add(SekPop(i, 0), SekPop(i, 1))
            Next i

        ElseIf (globalAnzZiel >= 3) Then
            '3 oder mehr Zielfunktionen (es werden die ersten drei angezeigt)
            '----------------------------------------------------------------
            serie3D = DForm.Diag.getSeries3DPoint("Sekundäre Population", "Green")
            serie3D.Clear()
            For i = 0 To SekPop.GetUpperBound(0)
                serie3D.Add(SekPop(i, 0), SekPop(i, 1), SekPop(i, 2))
            Next i
        Else
            Throw New Exception("Der Parameter 'globalAnzZiel' weist ungültige Parameter auf.")
        End If

    End Sub

    'Sekundäre Population anhand von Sim-Ergebnisspeicher zeichnen
    '*************************************************************
    Private Sub SekundärePopulationZeichnen(ByVal _igen As Integer)

        Dim i As Short
        Dim serie As Steema.TeeChart.Styles.Series
        Dim serie3D As Steema.TeeChart.Styles.Points3D
        Dim solutions() As Kern.Individuum

        'SekPop holen
        solutions = Sim1.OptResult.getSekPop(_igen)

        If (globalAnzZiel = 2) Then
            '2 Zielfunktionen
            '----------------------------------------------------------------
            serie = DForm.Diag.getSeriesPoint("Sekundäre Population", "Green")
            serie.Clear()
            For i = 0 To solutions.GetUpperBound(0)
                serie.Add(solutions(i).Penalty(0), solutions(i).Penalty(1), solutions(i).ID)
            Next i

        ElseIf (globalAnzZiel >= 3) Then
            '3 oder mehr Zielfunktionen (es werden die ersten drei angezeigt)
            '----------------------------------------------------------------
            serie3D = DForm.Diag.getSeries3DPoint("Sekundäre Population", "Green")
            serie3D.Clear()
            For i = 0 To solutions.GetUpperBound(0)
                serie3D.Add(solutions(i).Penalty(0), solutions(i).Penalty(1), solutions(i).Penalty(2), solutions(i).ID)
            Next i
        Else
            Throw New Exception("Der Parameter 'globalAnzZiel' weist ungültige Parameter auf.")
        End If

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

                Call Testprobleme1.DiagInitialise(Me.EVO_Einstellungen1.Settings, globalAnzPar, Me.DForm.Diag)

            Case ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM
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

                    Case METH_HOOKJEEVES
                        'Achsen:
                        '-------
                        Dim Achse As Diagramm.Achse
                        Dim Achsen As New Collection
                        Achse.Name = "Simulation"
                        Achse.Auto = True
                        Achsen.Add(Achse)
                        'für jede Zielfunktion eine weitere Achse hinzufügen
                        For i = 0 To Sim1.List_OptZiele.GetUpperBound(0)
                            Achse.Name = Sim1.List_OptZiele(i).Bezeichnung
                            Achse.Auto = True
                            Achse.Max = 0
                            Achsen.Add(Achse)
                        Next
                        'Diagramm initialisieren
                        Call DForm.Diag.DiagInitialise(Anwendung, Achsen)


                    Case Else 'PES, CES, CES + PES, HYBRID
                        'XXXXXXXXXXXXXXXXXXXXX

                        'Achsen:
                        '-------
                        Dim Achse As Diagramm.Achse
                        Dim Achsen As New Collection

                        'Bei Single-Objective: X-Achse = Nr. der Simulation (Durchlauf)
                        If (globalAnzZiel = 1) Then

                            Achse.Name = "Simulation"
                            Achse.Auto = False
                            If (Form1.Method = METH_PES) Then
                                'Bei PES:
                                '--------
                                If (EVO_Einstellungen1.Settings.PES.Pop.is_POPUL) Then
                                    Achse.Max = EVO_Einstellungen1.Settings.PES.n_Gen * EVO_Einstellungen1.Settings.PES.n_Nachf * EVO_Einstellungen1.Settings.PES.Pop.n_Runden + 1
                                Else
                                    Achse.Max = EVO_Einstellungen1.Settings.PES.n_Gen * EVO_Einstellungen1.Settings.PES.n_Nachf + 1
                                End If
                            Else
                                'Bei CES etc.:
                                '-------------
                                Achse.Max = EVO_Einstellungen1.Settings.CES.n_Childs * EVO_Einstellungen1.Settings.CES.n_Generations
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

                End Select

        End Select

        Call Application.DoEvents()

    End Sub

    'Scatterplot-Matrix anzeigen
    '****************************
    Private Sub showScatterplot(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Scatterplot.Click

        Dim diagresult As DialogResult
        Dim SekPopOnly As Boolean

        'Abfrage, ob nur Sekundäre Population gezeichnet werden soll
        '-----------------------------------------------------------
        diagresult = MsgBox("Soll nur die Sekundäre Population angezeigt werden?", MsgBoxStyle.YesNo, "Scatterplot-Matrix")

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

#Region "Lösungsauswahl"

    'Klick auf Serie in Diagramm
    '***************************
    Public Sub seriesClick(ByVal sender As Object, ByVal s As Steema.TeeChart.Styles.Series, ByVal valueIndex As Integer, ByVal e As System.Windows.Forms.MouseEventArgs)

        'Notwendige Bedingungen überprüfen
        '---------------------------------
        If (IsNothing(Sim1)) Then
            'Anwendung != Sim
            MsgBox("Lösungsauswahl funktioniert nur bei Simulationsanwendungen!", MsgBoxStyle.Information, "Info")
            Exit Sub
        Else

            Dim indID_clicked As Integer
            Dim ind As Kern.Individuum

            'Solution-ID
            indID_clicked = s.Labels(valueIndex)

            'Lösung holen
            ind = Sim1.OptResult.getSolution(indID_clicked)

            If (ind.ID = indID_clicked) Then

                'Lösung auswählen
                Call Me.selectSolution(ind)

            End If

        End If

    End Sub

    'Eine Lösung auswählen
    '*********************
    Private Sub selectSolution(ByVal ind As Kern.Individuum) Handles scatterplot1.pointSelected

        Dim isOK As Boolean

        'Lösung zu ausgewählten Lösungen hinzufügen
        isOK = Sim1.OptResult.selectSolution(ind.ID)

        If (isOK) Then

            'Lösungsdialog initialisieren
            If (IsNothing(Me.solutionDialog)) Then
                Me.solutionDialog = New SolutionDialog(Sim1.List_OptParameter_Save, Sim1.List_OptZiele, Sim1.List_Constraints, Sim1.List_Locations)
            End If

            'Lösungsdialog anzeigen
            Call Me.solutionDialog.Show()

            'Lösung zum Lösungsdialog hinzufügen
            Call Me.solutionDialog.addSolution(ind)

            'Lösung im Hauptdiagramm anzeigen
            Call Me.DForm.Diag.showSelectedSolution(Me.Sim1.List_OptZiele, ind)

            'Lösung im Scatterplot anzeigen
            If (Not IsNothing(Me.scatterplot1)) Then
                Call Me.scatterplot1.showSelectedSolution(ind)
            End If

        End If

        'Lösungsdialog nach vorne bringen
        Call Me.solutionDialog.BringToFront()

    End Sub

    'Lösungsauswahl zurücksetzen
    '***************************
    Public Sub clearSelection()

        'Serie der ausgewählten Lösungen löschen
        '=======================================

        'Im Hauptdiagramm
        '----------------
        Call Me.DForm.Diag.clearSelection()

        'In der Scatterplot-Matrix
        '-------------------------
        If (Not IsNothing(Me.scatterplot1)) Then
            Call scatterplot1.clearSelection()
        End If

        'Auswahl intern zurücksetzen
        '===========================
        Call Sim1.OptResult.clearSelectedSolutions()

    End Sub

    'ausgewählte Lösungen simulieren und in Wave anzeigen
    '****************************************************
    Public Sub showWave(ByVal checkedSolutions As Collection)

        Dim i As Integer
        Dim isOK As Boolean
        Dim isIHA As Boolean

        Dim zre As Wave.Zeitreihe
        Dim SimSeries As New Collection                 'zu zeichnende Simulationsreihen
        Dim RefSeries As New Collection                 'zu zeichnende Referenzreihen

        'Wait cursor
        Cursor = Cursors.WaitCursor

        'Wave instanzieren
        Dim Wave1 As New Wave.Wave()

        'Sonderfall BlueM mit IHA-Berechnung 
        'ein 2. Wave für RVA-Diagramme instanzieren
        Dim Wave2 As Wave.Wave = Nothing
        If (TypeOf Me.Sim1 Is IHWB.EVO.BlueM) Then
            If (CType(Me.Sim1, IHWB.EVO.BlueM).isIHA) Then
                isIHA = True
                Wave2 = New Wave.Wave()
                Call Wave2.PrepareChart_RVA()
                'IHA-Vergleichsmodus?
                If (CType(Me.Sim1, IHWB.EVO.BlueM).IHAProc.isComparison) Then
                    'Referenz-RVAErgebnis in Wave2 laden
                    Call Wave2.Display_RVA(CType(Me.Sim1, IHWB.EVO.BlueM).IHAProc.RVABase)
                End If
            End If
        End If

        'Alle ausgewählten Lösungen durchlaufen
        '======================================
        For Each ind As Kern.Individuum In Sim1.OptResult.getSelectedSolutions()

            'Lösung per Checkbox ausgewählt?
            '-------------------------------
            If (Not checkedSolutions.Contains(ind.ID.ToString())) Then
                Continue For
            End If

            'Simulation vorbereiten
            'xxxxxxxxxxxxxxxxxxxxxx

            Select Case Form1.Method

                Case METH_PES

                    'Bereitet das BlueM für PES vor
                    Call Sim1.PREPARE_Evaluation_PES(ind.All_PES_Para)

                Case METH_CES, METH_HYBRID

                    'Aktueller Pfad wird an Sim zurückgegeben
                    'Bereitet das BlaueModell für die Kombinatorik vor
                    Call Sim1.PREPARE_Evaluation_CES(ind.Path, ind.All_Elem)

                    'HYBRID: Bereitet für die Optimierung mit den PES Parametern vor
                    If Form1.Method = METH_HYBRID And EVO_Einstellungen1.Settings.CES.ty_Hybrid = EVO.Kern.HYBRID_TYPE.Mixed_Integer Then
                        Call Sim1.Reduce_OptPara_and_ModPara(ind.All_Elem)
                        Call Sim1.PREPARE_Evaluation_PES(ind.All_Loc_Para)
                    End If

            End Select

            'Simulation ausführen
            'xxxxxxxxxxxxxxxxxxxx

            'Simulieren
            isOK = Sim1.launchSim()

            'Sonderfall IHA-Berechnung
            If (isIHA) Then
                'RVA-Ergebnis in Wave2 laden
                Dim RVAResult As Wave.RVA.Struct_RVAValues
                RVAResult = CType(Me.Sim1, IHWB.EVO.BlueM).IHASys.RVAResult
                'Lösungsnummer an Titel anhängen
                RVAResult.Title = "Lösung " & ind.ID.ToString()
                Call Wave2.Display_RVA(RVAResult)
            End If

            'Zu zeichnenden Simulationsreihen zurücksetzen
            SimSeries.Clear()

            'zu zeichnenden Reihen aus Liste der OptZiele raussuchen
            '-------------------------------------------------------
            For i = 0 To Sim1.List_OptZiele.GetUpperBound(0)

                With Sim1.List_OptZiele(i)

                    'Referenzreihe in Wave laden
                    '---------------------------
                    If (.ZielTyp = "Reihe" Or .ZielTyp = "IHA") Then
                        'Referenzreihen nur jeweils ein Mal zeichnen
                        If (Not RefSeries.Contains(.ZielReiheDatei & .ZielGr)) Then
                            RefSeries.Add(.ZielGr, .ZielReiheDatei & .ZielGr)
                            'Referenzreihe in Wave laden
                            Wave1.Display_Series(.ZielReihe)
                        End If
                    End If

                    'Simulationsergebnis in Wave laden
                    '---------------------------------
                    'Simulationsreihen nur jeweils ein Mal zeichnen
                    If (Not SimSeries.Contains(.SimGr)) Then
                        Call SimSeries.Add(.SimGr, .SimGr)
                        zre = Sim1.SimErgebnis(.SimGr).Clone()
                        'Lösungsnummer an Titel anhängen
                        zre.Title &= " (Lösung " & ind.ID.ToString() & ")"
                        'Simreihe in Wave laden
                        Call Wave1.Display_Series(zre)
                    End If

                End With
            Next

        Next ind

        'Wave anzeigen
        '-------------
        Call Wave1.Show()
        If (Not IsNothing(Wave2)) Then Call Wave2.Show()

        'Cursor
        Cursor = Cursors.Default

    End Sub

#End Region 'Lösungsauswahl

#End Region 'Diagrammfunktionen

#Region "Ergebnisdatenbank"

    'Ergebnisdatenbank speichern
    '***************************
    Private Sub saveMDB(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_saveMDB.Click

        Dim diagresult As DialogResult

        'Datei-speichern Dialog anzeigen
        Me.SaveFileDialog1.Filter = "Access-Datenbanken (*.mdb)|*.mdb"
        Me.SaveFileDialog1.DefaultExt = "mdb"
        Me.SaveFileDialog1.Title = "Ergebnisdatenbank speichern unter..."
        Me.SaveFileDialog1.FileName = Sim1.Datensatz & "_EVO.mdb"
        Me.SaveFileDialog1.InitialDirectory = Sim1.WorkDir
        diagresult = Me.SaveFileDialog1.ShowDialog()

        If (diagresult = Windows.Forms.DialogResult.OK) Then

            'Ergebnisdatenbank speichern
            Call Sim1.OptResult.db_save(Me.SaveFileDialog1.FileName)

        End If

    End Sub

    'Optimierungsergebnis aus einer Datenbank einlesen
    '*************************************************
    Private Sub loadFromMDB(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_openMDB.Click

        Dim diagresult As DialogResult
        Dim sourceFile As String

        'Datei-öffnen Dialog anzeigen
        Me.OpenFileDialog1.Filter = "Access-Datenbanken (*.mdb)|*.mdb"
        Me.OpenFileDialog1.Title = "Ergebnisdatenbank auswählen"
        Me.OpenFileDialog1.FileName = ""
        Me.OpenFileDialog1.InitialDirectory = Sim1.WorkDir
        diagresult = Me.OpenFileDialog1.ShowDialog()

        If (diagresult = Windows.Forms.DialogResult.OK) Then

            sourceFile = Me.OpenFileDialog1.FileName

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
            'Bei weniger als 2 Zielen Y-Achse ausblenden
            If (Sim1.List_OptZiele.Length < 2) Then
                importDialog.ListBox_OptZieleY.Enabled = False
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
                'Single-objective
                If (OptZielIndexZ = -1 And OptZielIndexY = -1) Then
                    tmpAchse.Name = "Simulation"
                    Achsen.Add(tmpAchse)
                    tmpAchse.Name = importDialog.ListBox_OptZieleX.SelectedItem
                    Achsen.Add(tmpAchse)
                Else
                    'Multi-objective
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
                End If

                'Diagramm initialisieren
                '-----------------------
                Me.DForm.Diag.Clear()
                Me.DForm.Diag.DiagInitialise(Path.GetFileName(sourceFile), Achsen)

                'Punkte eintragen
                '----------------
                Dim serie As Steema.TeeChart.Styles.Series
                Dim serie3D As Steema.TeeChart.Styles.Points3D

                'Lösungen
                '========
                If (importDialog.ComboBox_SekPop.SelectedItem <> "ausschließlich") Then

                    For Each ind As Kern.Individuum In Sim1.OptResult.Solutions

                        If (OptZielIndexZ = -1 And OptZielIndexY = -1) Then
                            '1D
                            'Constraintverletzung prüfen
                            If (ind.feasible) Then
                                serie = Me.DForm.Diag.getSeriesPoint("Population", "red")
                            Else
                                serie = Me.DForm.Diag.getSeriesPoint("Population (ungültig)", "Gray")
                            End If
                            'Zeichnen
                            serie.Add(ind.ID, ind.Penalty(OptZielIndexX), ind.ID)
                        ElseIf (OptZielIndexZ = -1) Then
                            '2D
                            '--
                            'Constraintverletzung prüfen
                            If (ind.feasible) Then
                                serie = Me.DForm.Diag.getSeriesPoint("Population", "Orange")
                            Else
                                serie = Me.DForm.Diag.getSeriesPoint("Population (ungültig)", "Gray")
                            End If
                            'Zeichnen
                            serie.Add(ind.Penalty(OptZielIndexX), ind.Penalty(OptZielIndexY), ind.ID)
                        Else
                            '3D
                            '--
                            'Constraintverletzung prüfen
                            If (ind.feasible) Then
                                serie3D = Me.DForm.Diag.getSeries3DPoint("Population", "Orange")
                            Else
                                serie3D = Me.DForm.Diag.getSeries3DPoint("Population (ungültig)", "Gray")
                            End If
                            'Zeichnen
                            serie3D.Add(ind.Penalty(OptZielIndexX), ind.Penalty(OptZielIndexY), ind.Penalty(OptZielIndexZ), ind.ID)
                        End If

                    Next

                End If

                'Sekundärpopulation
                '==================
                If (importDialog.ComboBox_SekPop.SelectedItem <> "keine") Then

                    For Each sekpopind As Kern.Individuum In Sim1.OptResult.getSekPop()
                        If (OptZielIndexZ = -1) Then
                            '2D
                            '--
                            serie = Me.DForm.Diag.getSeriesPoint("Sekundäre Population", "Green")
                            serie.Add(sekpopind.Penalty(OptZielIndexX), sekpopind.Penalty(OptZielIndexY), sekpopind.ID)
                        Else
                            '3D
                            '--
                            serie3D = Me.DForm.Diag.getSeries3DPoint("Sekundäre Population", "Green")
                            serie3D.Add(sekpopind.Penalty(OptZielIndexX), sekpopind.Penalty(OptZielIndexY), sekpopind.Penalty(OptZielIndexZ), sekpopind.ID)
                        End If
                    Next

                End If

                'Ergebnis-Buttons
                Me.Button_Scatterplot.Enabled = True

                'Start-Button deaktivieren
                Me.Button_Start.Enabled = False

                'Cursor Default
                Cursor = Cursors.Default

            End If

        End If

    End Sub

#End Region 'Ergebnisdatenbank

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
