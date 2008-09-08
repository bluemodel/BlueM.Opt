'*******************************************************************************
'*******************************************************************************
'**** ihwb Optimierung                                                      ****
'****                                                                       ****
'**** Christoph Huebner, Felix Froehlich, Dirk Muschalla, Dominik Kerber    ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'****                                                                       ****
'**** Erstellt: Dezember 2003                                               ****
'****                                                                       ****
'**** Letzte Änderung: Juli 2008                                            ****
'*******************************************************************************
'*******************************************************************************

Option Strict Off ' Off ist Default
Option Explicit On
Imports System.IO
Imports IHWB.EVO.Common
Imports System.ComponentModel
Imports System.Threading

Partial Class Form1
    Inherits System.Windows.Forms.Form

#Region "Eigenschaften"

    Private IsInitializing As Boolean

    Private Options As OptionsDialog

    'Anwendung
    Private Anwendung As String

    'Problem
    Private mProblem As EVO.Common.Problem

    'Apps
    Private Testprobleme1 As EVO.Apps.Testprobleme
    Friend WithEvents Sim1 As EVO.Apps.Sim
    Private SensiPlot1 As EVO.Apps.SensiPlot
    Private TSP1 As EVO.Apps.TSP

    'Methoden
    Private CES1 As EVO.Kern.CES
    'Dim hybrid2008 As EVO.Hybrid2008.Main.cs

    '**** Globale Parameter Parameter Optimierung ****
    'TODO: diese Werte sollten eigentlich nur in CES bzw PES vorgehalten werden
    Dim array_x() As Double
    Dim array_y() As Double

    '**** Verschiedenes ****
    Dim isrun As Boolean = False                        'Optimierung läuft
    Dim ispause As Boolean = False                      'Optimierung ist pausiert

    '**** Multithreading ****
    Dim SIM_Eval_is_OK As Boolean
    Private n_Threads As Integer                        'Anzahl der Threads
    Dim MI_Thread_OK As Boolean = False

    'Dialoge
    Private WithEvents solutionDialog As SolutionDialog
    Private WithEvents scatterplot1 As EVO.Diagramm.Scatterplot

    'Diagramme
    Private WithEvents Hauptdiagramm1 As IHWB.EVO.Diagramm.Hauptdiagramm
    Private WithEvents Monitor1 As EVO.Diagramm.Monitor

#End Region 'Eigenschaften

#Region "Methoden"

#Region "UI"

    'Initialisierung von Form1
    '*************************
    Private Sub Form1_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        'Anzahl der Prozessoren wird ermittelt
        Anzahl_Prozessoren(n_Threads)

        'XP-look
        System.Windows.Forms.Application.EnableVisualStyles()

        'Liste der Anwendungen in ComboBox schreiben und Anfangseinstellung wählen
        ComboBox_Anwendung.Items.AddRange(New Object() {"", ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM, ANW_TESTPROBLEME, ANW_TSP})
        ComboBox_Anwendung.SelectedIndex = 0

        'Liste der Methoden in ComboBox schreiben und Anfangseinstellung wählen
        ComboBox_Methode.Items.AddRange(New Object() {"", METH_PES, METH_CES, METH_HYBRID, METH_SENSIPLOT, METH_HOOKJEEVES})
        ComboBox_Methode.SelectedIndex = 0

        'OptionsDialog instanzieren
        Me.Options = New OptionsDialog()

        'Monitor instanzieren
        Me.Monitor1 = New EVO.Diagramm.Monitor()

        'Handler für Klick auf Serien zuweisen
        AddHandler Me.Hauptdiagramm1.ClickSeries, AddressOf seriesClick

        'Ende der Initialisierung
        IsInitializing = False

    End Sub

    'Optionen Dialog anzeigen
    '************************
    Private Sub showOptionDialog(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_Optionen.Click
        Call Me.Options.ShowDialog()
    End Sub

    'Monitor anzeigen
    '****************
    Private Sub MenuItem_MonitorAnzeigen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_MonitorAnzeigen.Click

        If (Me.MenuItem_MonitorAnzeigen.Checked) Then
            Me.Monitor1.Show()
        Else
            Me.Monitor1.Hide()
        End If

    End Sub

    'Wenn Monitor geschlossen wird, Menüeintrag aktualisieren
    Private Sub MonitorClosed() Handles Monitor1.MonitorClosed
        Me.MenuItem_MonitorAnzeigen.Checked = False
    End Sub

    'About Dialog anzeigen
    '*********************
    Private Sub MenuItem_About_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_About.Click
        Dim AboutDialog As New AboutDialog()
        Call AboutDialog.ShowDialog()
    End Sub

    'EVO.NET Wiki aufrufen
    '*********************
    Private Sub MenuItem_Wiki_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_Wiki.Click
        Call Process.Start("http://130.83.196.154/BlueM/wiki/index.php/EVO.NET")
    End Sub

#End Region 'UI

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
            Me.Hauptdiagramm1.Reset()

            'Alles deaktivieren, danach je nach Anwendung aktivieren
            '-------------------------------------------------------

            'Sim1 zerstören
            Me.Sim1 = Nothing

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Datensatz-Reset deaktivieren
            Me.MenuItem_DatensatzZurücksetzen.Enabled = False

            'Methodenauswahl deaktivieren
            Me.Label_Methode.Enabled = False
            Me.ComboBox_Methode.Enabled = False

            'Ergebnis-Buttons
            Me.Button_saveMDB.Enabled = False
            Me.Button_openMDB.Enabled = False
            Me.Button_Scatterplot.Enabled = False
            Me.Button_loadRefResult.Enabled = False

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
                    Sim1 = New EVO.Apps.BlueM(n_Threads)


                Case ANW_SMUSI 'Anwendung Smusi
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse Smusi initialisieren
                    Sim1 = New EVO.Apps.Smusi()


                Case ANW_SCAN 'Anwendung S:CAN
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse Scan initialisieren
                    Sim1 = New EVO.Apps.Scan()


                Case ANW_SWMM   'Anwendung SWMM
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Objekt der Klasse SWMM initialisieren
                    Sim1 = New EVO.Apps.SWMM()


                Case ANW_TESTPROBLEME 'Anwendung Testprobleme
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Testprobleme instanzieren
                    Testprobleme1 = New EVO.Apps.Testprobleme()


                Case ANW_TSP 'Anwendung Traveling Salesman Problem (TSP)
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    TSP1 = New EVO.Apps.TSP()

                    Call TSP1.TSP_Initialize(Me.Hauptdiagramm1)

                    'Start-Button aktivieren (keine Methodenauswahl erforderlich)
                    Button_Start.Enabled = True

            End Select

            'Datensatz UI aktivieren
            Call Me.Datensatz_initUI()

            'EVO_Verlauf zurücksetzen
            Call Me.EVO_Opt_Verlauf1.Initialisieren(EVO_Einstellungen1.Settings.PES.Pop.n_Runden, EVO_Einstellungen1.Settings.PES.Pop.n_Popul, EVO_Einstellungen1.Settings.PES.n_Gen, EVO_Einstellungen1.Settings.PES.n_Nachf)

            'Mauszeiger wieder normal
            Cursor = Cursors.Default

        End If

    End Sub

    'Datensatz-UI anzeigen
    '*********************
    Private Sub Datensatz_initUI()

        Dim pfad As String

        'UI aktivieren
        Me.Label_Datensatz.Enabled = True
        Me.ComboBox_Datensatz.Enabled = True

        'Tooltip zurücksetzen
        Me.ToolTip1.SetToolTip(Me.ComboBox_Datensatz, "")

        'Combo_Datensatz auffüllen
        Call Me.Datensatz_populateCombo()

        'Bei Simulationsanwendungen:
        If (Me.Anwendung <> ANW_TESTPROBLEME) Then

            'zuletzt benutzten Datensatz setzen
            If (My.Settings.MRUSimDatensaetze.Count > 0) Then
                pfad = My.Settings.MRUSimDatensaetze.Item(My.Settings.MRUSimDatensaetze.Count - 1)
                Me.ComboBox_Datensatz.SelectedItem = pfad
                Call Sim1.setDatensatz(pfad)
            End If

            'Browse-Button aktivieren
            Me.Button_BrowseDatensatz.Enabled = True
        End If

    End Sub

    'Combo_Datensatz auffüllen
    '*************************
    Private Sub Datensatz_populateCombo()

        Dim i As Integer

        'vorherige Einträge löschen
        Me.ComboBox_Datensatz.Items.Clear()

        Select Case Me.Anwendung

            Case ANW_TESTPROBLEME

                'Mit Tesproblemen füllen
                Me.ComboBox_Datensatz.Items.AddRange(Testprobleme1.Testprobleme)

            Case Else '(Sim-Anwendungen)

                'Mit Benutzer-MRUSimDatensätze füllen
                If (My.Settings.MRUSimDatensaetze.Count > 0) Then

                    'Combobox rückwärts füllen
                    For i = My.Settings.MRUSimDatensaetze.Count - 1 To 0 Step -1
                        Me.ComboBox_Datensatz.Items.Add(My.Settings.MRUSimDatensaetze.Item(i))
                    Next

                End If

        End Select

    End Sub


    'Arbeitsverzeichnis/Datensatz auswählen (nur Sim-Anwendungen)
    '************************************************************
    Private Sub Datensatz_browse(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_BrowseDatensatz.Click

        Dim DiagResult As DialogResult
        Dim pfad As String

        'Dialog vorbereiten
        OpenFileDialog1.Filter = Sim1.DatensatzDateiendungen(0) & "-Dateien (*." & Sim1.DatensatzDateiendungen(0) & ")|*." & Sim1.DatensatzDateiendungen(0)
        OpenFileDialog1.Title = "Datensatz auswählen"

        'Alten Datensatz dem Dialog zuweisen
        OpenFileDialog1.InitialDirectory = Sim1.WorkDir_Original
        OpenFileDialog1.FileName = Sim1.WorkDir_Original & Sim1.Datensatz & "." & Sim1.DatensatzDateiendungen(0)

        'Dialog öffnen
        DiagResult = OpenFileDialog1.ShowDialog()

        'Neuen Datensatz speichern
        If (DiagResult = Windows.Forms.DialogResult.OK) Then

            pfad = OpenFileDialog1.FileName

            'Datensatz setzen
            Call Sim1.setDatensatz(pfad)

            'Benutzereinstellungen aktualisieren
            If (My.Settings.MRUSimDatensaetze.Contains(pfad)) Then
                My.Settings.MRUSimDatensaetze.Remove(pfad)
            End If
            My.Settings.MRUSimDatensaetze.Add(pfad)

            'Benutzereinstellungen speichern
            Call My.Settings.Save()

            'Datensatzanzeige aktualisieren
            Call Me.Datensatz_populateCombo()
            Me.ComboBox_Datensatz.SelectedItem = pfad

            'Methodenauswahl wieder zurücksetzen 
            '(Der Benutzer muss zuerst Ini neu ausführen!)
            Me.ComboBox_Methode.SelectedItem = ""

        End If

    End Sub

    'Sim-Datensatz zurücksetzen
    '**************************
    Private Sub Datensatz_reset(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_DatensatzZurücksetzen.Click

        'Original ModellParameter schreiben
        Call Sim1.Write_ModellParameter()

        MsgBox("Die Startwerte der Optimierungsparameter wurden in die Eingabedateien geschrieben.", MsgBoxStyle.Information, "Info")

    End Sub

    'Datensatz wurde ausgewählt
    '**************************
    Private Sub INI_Datensatz(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Datensatz.SelectedIndexChanged

        If (Me.IsInitializing = True) Then

            Exit Sub

        Else

            'Zurücksetzen
            '------------

            'Tooltip
            Me.ToolTip1.SetToolTip(Me.ComboBox_Datensatz, "")

            'Datensatz-Reset
            Me.MenuItem_DatensatzZurücksetzen.Enabled = False

            'gewählten Datensatz an Anwendung übergeben
            '------------------------------------------
            Select Case Me.Anwendung

                Case ANW_TESTPROBLEME

                    'Tesproblem setzen
                    Testprobleme1.setTestproblem(Me.ComboBox_Datensatz.SelectedItem)

                    'Tooltip anzeigen
                    Me.ToolTip1.SetToolTip(Me.ComboBox_Datensatz, Testprobleme1.TestProblemDescription)

                Case Else '(Alle Sim-Anwendungen)

                    Call Sim1.setDatensatz(Me.ComboBox_Datensatz.SelectedItem)

            End Select

            'Methodenauswahl aktivieren und zurücksetzen
            '-------------------------------------------
            Me.Label_Methode.Enabled = True
            Me.ComboBox_Methode.Enabled = True
            Me.ComboBox_Methode.SelectedItem = ""

        End If

    End Sub

    'Methode wurde ausgewählt
    '************************
    Private Sub INI_Method(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Methode.SelectedIndexChanged

        If (Me.IsInitializing = True) Then

            Exit Sub

        Else

            'Mauszeiger busy
            Cursor = Cursors.WaitCursor

            'Diagramm zurücksetzen
            Me.Hauptdiagramm1.Reset()


            'Problemdefinition
            '=================
            If (Me.Anwendung <> ANW_TESTPROBLEME And Me.Anwendung <> ANW_TSP) Then

                'Bei allen Sim-Anwendungen
                '-------------------------

                'Neues Problem instanzieren und Methode setzen
                Me.mProblem = New EVO.Common.Problem(ComboBox_Methode.SelectedItem, Sim1.WorkDir_Original, Sim1.Datensatz)

                'EVO-Eingabedateien einlesen
                Call Me.mProblem.Read_InputFiles(Me.Sim1.SimStart, Me.Sim1.SimEnde)

                'Problem an Sim-Objekt übergeben
                Call Me.Sim1.setProblem(Me.mProblem)

                'Settings auch übergeben
                Call Me.Sim1.setSettings(Me.EVO_Einstellungen1.Settings)

            ElseIf (Me.Anwendung = ANW_TESTPROBLEME) Then

                'Bei Testproblemen definieren diese das Problem selbst
                '-----------------------------------------------------
                Me.mProblem = Testprobleme1.getProblem()

            End If

            'Problem an EVO_Einstellungen übergeben
            '--------------------------------------
            Call Me.EVO_Einstellungen1.Initialise(Me.mProblem)


            'Methodenspezifische Vorbereitungen
            '(zunächst alles deaktivieren, danach je nach Methode aktivieren)
            '================================================================

            'Start Button deaktivieren
            Me.Button_Start.Enabled = False

            'Ergebnis-Buttons deaktivieren
            Me.Button_saveMDB.Enabled = False
            Me.Button_openMDB.Enabled = False
            Me.Button_Scatterplot.Enabled = False

            Select Case Me.mProblem.Method

                Case "" 'Keine Methode ausgewählt
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Mauszeiger wieder normal
                    Cursor = Cursors.Default

                    'Ende
                    Exit Sub

                Case METH_SENSIPLOT 'Methode SensiPlot
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'SensiPlot instanzieren
                    SensiPlot1 = New EVO.Apps.SensiPlot(Me.mProblem)

                    'Monitor deaktivieren
                    Me.MenuItem_MonitorAnzeigen.Checked = False

                    'SensiPlot Dialog anzeigen:
                    Dim SensiPlotDiagResult As Windows.Forms.DialogResult
                    SensiPlotDiagResult = SensiPlot1.ShowDialog()
                    If (Not SensiPlotDiagResult = Windows.Forms.DialogResult.OK) Then
                        'Mauszeiger wieder normal
                        Cursor = Cursors.Default
                        Exit Sub
                    End If


                Case METH_PES 'Methode PES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'EVO_Opt_Verlauf initialisieren
                    Call Me.EVO_Opt_Verlauf1.Initialisieren(EVO_Einstellungen1.Settings.PES.Pop.n_Runden, EVO_Einstellungen1.Settings.PES.Pop.n_Popul, EVO_Einstellungen1.Settings.PES.n_Gen, EVO_Einstellungen1.Settings.PES.n_Nachf)


                Case METH_HOOKJEEVES
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Kontrolle: Nur SO möglich!
                    If (Me.mProblem.Modus = EVO_MODUS.Multi_Objective) Then
                        Throw New Exception("Methode von Hook und Jeeves erlaubt nur Single-Objective Optimierung!")
                    End If

                    'TODO: EVO_Opt_Verlauf initialisieren


                Case METH_CES, METH_HYBRID 'Methode CES und HYBRID
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    'Funktioniert nur bei BlueM!
                    If (Not Anwendung = ANW_BLUEM) Then
                        Throw New Exception("CES/HYBRID funktioniert bisher nur mit BlueM!")
                    End If

                    'Ergebnis-Buttons
                    Me.Button_openMDB.Enabled = True

                    If (Me.mProblem.Method = METH_HYBRID) Then

                        'Original ModellParameter schreiben
                        Call Sim1.Write_ModellParameter()

                        'Original Transportstrecken einlesen
                        Call CType(Me.Sim1, EVO.Apps.BlueM).SKos1.Read_TRS_Orig_Daten(Sim1.WorkDir_Original)

                    End If

                    'ggf. EVO_Einstellungen Testmodus einrichten
                    '-------------------------------------------
                    'Bei Testmodus wird die Anzahl der Kinder und Generationen überschrieben
                    If Not (Me.mProblem.CES_T_Modus = Common.Constants.CES_T_MODUS.No_Test) Then
                        Call EVO_Einstellungen1.setTestModus(Me.mProblem.CES_T_Modus, Sim1.TestPath, 1, 1, Me.mProblem.NumCombinations)
                    End If

                    'TODO: EVO_Opt_Verlauf initialisieren


                Case METH_Hybrid2008
                    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

                    If (Me.Anwendung = ANW_TESTPROBLEME) Then
                        'Testprobleme mit Hybrid2008 Verfahren berechnen
                        MsgBox("Berechnung der Testprobleme mit Hybrid2008", MsgBoxStyle.Information, "Info")

                    Else
                        'Modelle mit Hybrid2008 berechnen
                        MsgBox("Berechnung der Modelle mit Hybrid2008", MsgBoxStyle.Information, "Info")

                    End If

                    'Ergebnis-Buttons
                    'Me.Button_openMDB.Enabled = True

                    'EVO_Verlauf zurücksetzen
                    'Call Me.EVO_Opt_Verlauf1.Initialisieren(EVO_Einstellungen1.Settings.PES.Pop.n_Runden, EVO_Einstellungen1.Settings.PES.Pop.n_Popul, EVO_Einstellungen1.Settings.PES.n_Gen, EVO_Einstellungen1.Settings.PES.n_Nachf)

            End Select

            'IniMethod OK -> Start Button aktivieren
            Me.Button_Start.Enabled = True

            If (Me.Anwendung <> ANW_TESTPROBLEME) Then
                'Datensatz-Reset aktivieren
                Me.MenuItem_DatensatzZurücksetzen.Enabled = True
            End If

            'Mauszeiger wieder normal
            Cursor = Cursors.Default

        End If

    End Sub

    'EVO_Einstellungen laden
    '***********************
    Friend Sub Load_EVO_Settings(ByVal sender As Object, ByVal e As System.EventArgs)

        'Dialog einrichten
        OpenFileDialog1.Filter = "XML-Dateien (*.xml)|*.xml"
        OpenFileDialog1.FileName = "EVO_Settings.xml"
        OpenFileDialog1.Title = "Einstellungsdatei auswählen"
        If (Not IsNothing(Sim1)) Then
            OpenFileDialog1.InitialDirectory = Sim1.WorkDir_Original
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
        If (Not IsNothing(Sim1)) Then
            SaveFileDialog1.InitialDirectory = Sim1.WorkDir_Original
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
            'Me.Button_Start.Text = "Run"
            'Do While (Me.ispause)
            '    System.Threading.Thread.Sleep(20)
            '    Application.DoEvents()
            'Loop

        ElseIf (Me.isrun) Then
            'Optimierung weiterlaufen lassen
            '-------------------------------
            Me.ispause = False
            Me.Button_Start.Text = "Pause"

        Else
            'Optimierung starten
            '-------------------
            Me.isrun = True
            Me.Button_Start.Text = "Pause"

            'Monitor anzeigen
            If (Me.MenuItem_MonitorAnzeigen.Checked) Then
                Call Me.Monitor1.Show()
            End If

            'Ergebnis-Buttons
            If (Not IsNothing(Sim1)) Then
                Me.Button_saveMDB.Enabled = True
                Me.Button_Scatterplot.Enabled = True
                Me.Button_loadRefResult.Enabled = True
            End If

            'EVO_Einstellungen temporär speichern
            Dim dir As String
            dir = My.Computer.FileSystem.SpecialDirectories.Temp & "\"
            Call Me.EVO_Einstellungen1.saveSettings(dir & "EVO_Settings.xml")

            Select Case Anwendung

                Case ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM

                    Select Case Me.mProblem.Method
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
            Me.Button_Start.Text = "Run"
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

        Dim i, j, n, Anz_SensiPara, Anz_Sim As Integer
        Dim isOK As Boolean
        Dim ind As Common.Individuum_PES
        Dim serie As Steema.TeeChart.Styles.Series
        Dim surface As New Steema.TeeChart.Styles.Surface
        Dim SimReihe As Wave.Zeitreihe
        Dim SimReihen As Collection
        Dim Wave1 As Wave.Wave

        'Simulationen in Originalverzeichnis ausführen (keine Threads)
        Sim1.WorkDir_Current = Sim1.WorkDir_Original

        'Instanzieren
        SimReihen = New Collection()

        'Parameter
        Anz_SensiPara = SensiPlot1.Selected_OptParameter.GetLength(0)

        'Individuumsklasse wird initialisiert
        Call Common.Individuum.Initialise(Me.mProblem)

        'Anzahl Simulationen
        If (Anz_SensiPara = 1) Then
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
        If (Anz_SensiPara > 1) Then
            surface = New Steema.TeeChart.Styles.Surface(Me.Hauptdiagramm1.Chart)
            surface.IrregularGrid = True
            surface.NumXValues = SensiPlot1.Anz_Steps
            surface.NumZValues = SensiPlot1.Anz_Steps
            'Diagramm drehen (rechter Mausbutton)
            Dim rotate1 As New Steema.TeeChart.Tools.Rotate
            rotate1.Button = Windows.Forms.MouseButtons.Right
            Me.Hauptdiagramm1.Tools.Add(rotate1)
            'MarksTips
            Me.Hauptdiagramm1.add_MarksTips(surface)
            surface.Title = "SensiPlot"
            surface.Cursor = Cursors.Hand
        End If

        'Simulationsschleife
        '-------------------
        Randomize()

        n = 0

        'Äussere Schleife (2. OptParameter)
        '----------------------------------
        For i = 0 To ((SensiPlot1.Anz_Steps - 1) * (Anz_SensiPara - 1))

            '2. OptParameterwert variieren
            If (Anz_SensiPara > 1) Then
                Select Case SensiPlot1.Selected_SensiType
                    Case "Gleichverteilt"
                        Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Xn = Rnd()
                    Case "Diskret"
                        Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Xn = i / (SensiPlot1.Anz_Steps - 1)
                End Select
            End If

            'Innere Schleife (1. OptParameter)
            '---------------------------------
            For j = 0 To SensiPlot1.Anz_Steps - 1

                '1. OptParameterwert variieren
                Select Case SensiPlot1.Selected_SensiType
                    Case "Gleichverteilt"
                        Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Xn = Rnd()
                    Case "Diskret"
                        Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Xn = j / (SensiPlot1.Anz_Steps - 1)
                End Select

                n += 1

                'Verlaufsanzeige aktualisieren
                Me.EVO_Opt_Verlauf1.Nachfolger(n)

                'Individuum instanzieren
                ind = New Common.Individuum_PES("SensiPlot", n)

                'OptParameter ins Individuum kopieren
                ind.OptParameter = Me.mProblem.List_OptParameter

                'Individuum in Sim evaluieren
                isOK = Sim1.Evaluate(ind)
                'TODO: Fehlerbehandlung bei Simulationsfehler

                'BUG 253: Verletzte Constraints bei SensiPlot kenntlich machen?

                'Diagramm aktualisieren
                If (Anz_SensiPara = 1) Then
                    '1 Parameter
                    serie = Me.Hauptdiagramm1.getSeriesPoint("SensiPlot", "Orange")
                    serie.Add(ind.Penalties(SensiPlot1.Selected_Penaltyfunction), ind.OptParameter_RWerte(SensiPlot1.Selected_OptParameter(0)), n.ToString())
                Else
                    '2 Parameter
                    surface.Add(ind.OptParameter_RWerte(SensiPlot1.Selected_OptParameter(0)), ind.Penalties(SensiPlot1.Selected_Penaltyfunction), ind.OptParameter_RWerte(SensiPlot1.Selected_OptParameter(1)), n.ToString())
                End If

                'Simulationsergebnis in Wave laden
                If (SensiPlot1.show_Wave) Then
                    'SimReihe auslesen
                    SimReihe = Sim1.SimErgebnis(Me.mProblem.List_Penaltyfunctions(SensiPlot1.Selected_Penaltyfunction).SimGr)
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
        Call TSP1.TeeChart_Initialise_TSP(Me.Hauptdiagramm1)

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
                Call TSP1.TeeChart_Zeichnen_TSP(Me.Hauptdiagramm1, TSP1.ParentList(0).Image)
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

        'Hypervolumen instanzieren
        Dim Hypervolume As EVO.MO_Indicators.Indicators
        Hypervolume = EVO.MO_Indicators.MO_IndicatorFabrik.GetInstance(EVO.MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, Me.mProblem.NumPenalties)

        'Datensätze für Multithreading kopieren
        If n_Threads > 1 Then
            Call Sim1.createThreadWorkDirs(n_Threads)
        End If

        'CES initialisieren
        CES1 = New EVO.Kern.CES()
        Call CES1.CESInitialise(Me.EVO_Einstellungen1.Settings, Me.mProblem, Sim1.VerzweigungsDatei.GetLength(0))

        'EVO_Verlauf zurücksetzen
        Call Me.EVO_Opt_Verlauf1.Initialisieren(1, 1, EVO_Einstellungen1.Settings.CES.n_Generations, EVO_Einstellungen1.Settings.CES.n_Childs)

        Dim durchlauf_all As Integer = 0
        Dim ColorArray(CES1.ModSett.n_Locations, -1) As Object

        'Laufvariable für die Generationen
        Dim i_gen, i_ch, i_loc As Integer

        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        'Zufällige Kinderpfade werden generiert
        '**************************************
        Call CES1.Generate_Random_Path()
        'Falls TESTMODUS werden sie überschrieben
        If (Not Me.mProblem.CES_T_Modus = Common.Constants.CES_T_MODUS.No_Test) Then
            Call CES1.Generate_Paths_for_Tests(Sim1.TestPath, Me.mProblem.CES_T_Modus)
        End If
        '**************************************

        'Hier werden dem Child die passenden Massnahmen und deren Elemente pro Location zugewiesen
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        For i_ch = 0 To CES1.mSettings.CES.n_Childs - 1
            For i_loc = 0 To CES1.ModSett.n_Locations - 1
                Call Sim1.Identify_Measures_Elements_Parameters(i_loc, CES1.Childs(i_ch).Path(i_loc), CES1.Childs(i_ch).Measures(i_loc), CES1.Childs(i_ch).Loc(i_loc).Loc_Elem, CES1.Childs(i_ch).Loc(i_loc).PES_OptPara)
            Next
        Next

        'Falls HYBRID werden entprechend der Einstellung im PES die Parameter auf Zufällig oder Start gesetzt
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        If (Me.mProblem.Method = METH_HYBRID And EVO_Einstellungen1.Settings.CES.ty_Hybrid = Common.Constants.HYBRID_TYPE.Mixed_Integer) Then
            CES1.Set_Xn_And_Dn_per_Location()
        End If

        'Startwerte werden der Verlaufsanzeige zugewiesen
        Call Me.EVO_Opt_Verlauf1.Initialisieren(0, 0, EVO_Einstellungen1.Settings.CES.n_Generations, EVO_Einstellungen1.Settings.CES.n_Childs)

        'xxxx Optimierung xxxxxx
        'Generationsschleife CES
        'xxxxxxxxxxxxxxxxxxxxxxx
        Dim Time(CES1.mSettings.CES.n_Generations - 1) As TimeSpan
        Dim Stoppuhr As New Stopwatch()


        For i_gen = 0 To CES1.mSettings.CES.n_Generations - 1
            Stoppuhr.Reset()
            Stoppuhr.Start()

            'Child Schleife
            'xxxxxxxxxxxxxx
            Dim Thread_Free As Integer = 0
            Dim Thread_Ready As Integer = 0
            Dim Child_Run As Integer = 0
            Dim Child_Ready As Integer = 0
            Dim Ready As Boolean = False
            System.Threading.Thread.CurrentThread.Priority = Threading.ThreadPriority.Normal

            Do
                'Falls eine Simulation frei und nicht Pause
                '------------------------------------------
                If Sim1.launchFree(Thread_Free) And Child_Run < CES1.mSettings.CES.n_Childs And _
                (Child_Ready + n_Threads > Child_Run) And Me.ispause = False Then

                    durchlauf_all += 1
                    Sim1.WorkDir_Current = Sim1.getThreadWorkDir(Thread_Free)
                    CES1.Childs(Child_Run).ID = durchlauf_all

                    '****************************************
                    'Aktueller Pfad wird an Sim zurückgegeben
                    'Bereitet das BlaueModell für die Kombinatorik vor
                    Call Sim1.PREPARE_Evaluation_CES(CES1.Childs(Child_Run).Path, CES1.Childs(Child_Run).Get_All_Loc_Elem)

                    'HYBRID: Bereitet für die Optimierung mit den PES Parametern vor
                    '***************************************************************
                    If (Me.mProblem.Method = METH_HYBRID And EVO_Einstellungen1.Settings.CES.ty_Hybrid = Common.Constants.HYBRID_TYPE.Mixed_Integer) Then
                        If (Me.mProblem.Reduce_OptPara_and_ModPara(CES1.Childs(Child_Run).Get_All_Loc_Elem)) Then
                            Call Sim1.PREPARE_Evaluation_PES(CES1.Childs(Child_Run).OptParameter)
                        End If
                    End If

                    ' Simulation ******************************************
                    SIM_Eval_is_OK = Sim1.launchSim(Thread_Free, Child_Run)
                    '******************************************************

                    Child_Run += 1

                    'Falls Simulation fertig und erfogreich
                    '--------------------------------------
                ElseIf Sim1.launchReady(Thread_Ready, SIM_Eval_is_OK, Child_Ready) Then

                    Sim1.WorkDir_Current = Sim1.getThreadWorkDir(Thread_Ready)
                    If SIM_Eval_is_OK Then Sim1.SIM_Ergebnis_auswerten(CES1.Childs(Child_Ready))

                    'HYBRID: Speichert die PES Erfahrung diesen Childs im PES Memory
                    '***************************************************************
                    If (Me.mProblem.Method = METH_HYBRID And EVO_Einstellungen1.Settings.CES.ty_Hybrid = Common.Constants.HYBRID_TYPE.Mixed_Integer) Then
                        Call CES1.Memory_Store(Child_Ready, i_gen)
                    End If

                    'Lösung im TeeChart einzeichnen und mittleres Dn ausgeben
                    '========================================================
                    Call Me.Hauptdiagramm1.ZeichneIndividuum(CES1.Childs(Child_Ready), 0, 0, i_gen, Child_Ready, ColorManagement(ColorArray, CES1.Childs(Child_Ready)))
                    Me.Label_Dn_Wert.Text = Math.Round(CES1.Childs(Child_Ready).Get_mean_PES_Dn, 6).ToString
                    If Not CES1.Childs(Child_Ready).Get_mean_PES_Dn = -1 Then
                        Me.Monitor1.Zeichne_Dn(CES1.Childs(Child_Ready).ID, CES1.Childs(Child_Ready).Get_mean_PES_Dn)
                    End If

                    System.Windows.Forms.Application.DoEvents()
                    Call EVO_Opt_Verlauf1.Nachfolger(Child_Ready + 1)
                    If Child_Ready = CES1.mSettings.CES.n_Childs - 1 Then Ready = True

                    Child_Ready += 1

                    'Falls Pause und alle simulierten auch verarbeitet
                    '-------------------------------------------------
                ElseIf Me.ispause = True And Child_Ready = Child_Run Then

                    Me.Button_Start.Text = "Run"
                    Do While (Me.ispause)
                        System.Threading.Thread.Sleep(20)
                        Application.DoEvents()
                    Loop

                Else

                    System.Threading.Thread.Sleep(400)
                    Application.DoEvents()

                End If

            Loop While Ready = False

            Stoppuhr.Stop()
            Time(i_gen) = Stoppuhr.Elapsed

            System.Threading.Thread.CurrentThread.Priority = Threading.ThreadPriority.BelowNormal

            '^ ENDE der Child Schleife
            'xxxxxxxxxxxxxxxxxxxxxxx

            Call EVO_Opt_Verlauf1.Generation(i_gen + 1)

            'Die Listen müssen nach der letzten Evaluierung wieder zurückgesetzt werden
            'Sicher ob das benötigt wird?
            Call Me.mProblem.Reset_OptPara_and_ModPara()

            'MO oder SO SELEKTIONSPROZESS oder NDSorting SELEKTION
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            'BUG 259: CES: Punkt-Labels der Sekundärpopulation fehlen noch!
            If (Me.mProblem.NumPenalties = 1) Then
                'Sortieren der Kinden anhand der Qualität
                Call CES1.Sort_Individuum(CES1.Childs)
                'Selectionsprozess je nach "plus" oder "minus" Strategie
                Call CES1.Selection_Process()
                'Zeichnen der besten Eltern
                'For i_ch = 0 To EVO_Einstellungen1.Settings.CES.n_Parents - 1
                '    'durchlauf += 1
                '    serie = Me.Hauptdiagramm1.getSeriesPoint("Parent", "green")
                '    Call serie.Add(durchlauf_all, CES1.Parents(i_ch).Penalties(0))
                'Next
            Else
                'NDSorting ******************
                Call CES1.NDSorting_CES_Control(i_gen)

                'Sekundäre Population
                '--------------------
                If (Not IsNothing(Sim1)) Then
                    'SekPop abspeichern
                    Call Sim1.OptResult.setSekPop(CES1.SekundärQb, i_gen)
                End If

                'SekPop zeichnen
                Call Me.Hauptdiagramm1.ZeichneSekPopulation(CES1.SekundärQb)

                'Hypervolumen berechnen und zeichnen
                '-----------------------------------
                Call Hypervolume.update_dataset(Common.Individuum.Get_All_Penalty_of_Array(CES1.SekundärQb))
                Call Me.Hauptdiagramm1.ZeichneNadirpunkt(Hypervolume.nadir)
                Call Me.Monitor1.ZeichneHyperVolumen(i_gen, Math.Abs(Hypervolume.calc_indicator()))
            End If
            ' ^ ENDE Selectionsprozess
            'xxxxxxxxxxxxxxxxxxxxxxxxx

            'REPRODUKTION und MUTATION Nicht wenn Testmodus
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            If (Me.mProblem.CES_T_Modus = Common.Constants.CES_T_MODUS.No_Test) Then
                'Kinder werden zur Sicherheit gelöscht aber nicht zerstört ;-)
                CES1.Childs = Common.Individuum.New_Indi_Array(Individuum.Individuumsklassen.Individuum_CES, CES1.Childs.GetLength(0), "Child")
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
            If (Me.mProblem.Method = METH_HYBRID And EVO_Einstellungen1.Settings.CES.ty_Hybrid = Common.Constants.HYBRID_TYPE.Mixed_Integer) Then
                MI_Thread_OK = False
                Dim MI_Thread As Thread
                MI_Thread = New Thread(AddressOf Me.Mixed_Integer_PES)
                MI_Thread.IsBackground = True
                MI_Thread.Start(i_gen)
                While MI_Thread_OK = False
                    Thread.Sleep(100)
                    Application.DoEvents()
                End While
            End If
        Next
        'Ende der Generationsschleife CES

        'Falls jetzt noch PES ausgeführt werden soll
        'Starten der PES mit der Front von CES
        '*******************************************
        If (Me.mProblem.Method = METH_HYBRID And Me.EVO_Einstellungen1.Settings.CES.ty_Hybrid = Common.Constants.HYBRID_TYPE.Sequencial_1) Then
            Call Start_PES_after_CES()
        End If

        'Datensätze für Multithreading löschen
        '*************************************
        Call Sim1.deleteThreadWorkDirs()

    End Sub

    'Mixed_Integer Teil ermittelt die PES Parameter für jedes neues Child und jede Location
    'Achtung! wird auch als Thread gestartet um weiter aufs Form zugreifen zu können
    '**************************************************************************************
    Private Sub Mixed_Integer_PES(ByVal i_gen As Integer)

        System.Threading.Thread.CurrentThread.Priority = Threading.ThreadPriority.BelowNormal

        Dim i_ch, i_loc As Integer

        'Selection oder NDSorting für den PES Memory
        '*******************************************
        If CES1.PES_Memory.GetLength(0) > CES1.mSettings.CES.n_PES_MemSize Then
            If (Me.mProblem.NumPenalties = 1) Then
                'Sortieren des PES_Memory anhande der Qualität
                Call CES1.Sort_Individuum(CES1.PES_Memory)
                'Kürzen des PES_Memory
                ReDim Preserve CES1.PES_Memory(CES1.mSettings.CES.n_PES_MemSize - 1)
            Else
                Call CES1.NDSorting_Memory(i_gen)
            End If
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

                    'Führt das Sortieren oder NDSorting für diesen Satz durch
                    '********************************************************
                    If CES1.PES_Parents_pLoc.GetLength(0) > CES1.mSettings.PES.n_Eltern Then
                        If (Me.mProblem.NumPenalties = 1) Then
                            'Sortieren der Parents anhand der Qualität
                            Call CES1.Sort_Individuum(CES1.PES_Parents_pLoc)
                            'Kürzen der Parents
                            ReDim Preserve CES1.PES_Parents_pLoc(CES1.mSettings.PES.n_Eltern - 1)
                        Else
                            Call CES1.NDSorting_PES_Parents_per_Loc(i_gen)
                        End If
                    End If

                    Dim m As Integer
                    Select Case CES1.PES_Parents_pLoc.GetLength(0)

                        Case Is = 0
                            'Noch keine Eltern vorhanden (die Child Location bekommt neue - zufällige Werte oder original Parameter)
                            '*******************************************************************************************************
                            For m = 0 To CES1.Childs(i_ch).Loc(i_loc).PES_OptPara.GetUpperBound(0)
                                CES1.Childs(i_ch).Loc(i_loc).PES_OptPara(m).Dn = CES1.mSettings.PES.Schrittweite.DnStart
                                'Falls zufällige Startwerte
                                If CES1.mSettings.PES.OptStartparameter = Common.Constants.EVO_STARTPARAMETER.Zufall Then
                                    Randomize()
                                    CES1.Childs(i_ch).Loc(i_loc).PES_OptPara(m).Xn = Rnd()
                                End If
                            Next

                        Case Is > 0
                            'Eltern vorhanden (das PES wird gestartet)
                            '*****************************************
                            If CES1.PES_Parents_pLoc.GetLength(0) < CES1.mSettings.PES.n_Eltern Then
                                'Falls es zu wenige sind wird mit den vorhandenen aufgefüllt
                                Call CES1.fill_Parents_per_Loc(CES1.PES_Parents_pLoc, CES1.mSettings.PES.n_Eltern)
                            End If

                            'Schritt 0: PES - Objekt der Klasse PES wird erzeugt PES wird erzeugt
                            '*********************************************************************
                            Dim PES1 As EVO.Kern.PES
                            PES1 = New EVO.Kern.PES()

                            'Vorbereitung um das PES zu initieren
                            '************************************
                            Me.mProblem.List_OptParameter = CES1.Childs(i_ch).Loc(i_loc).PES_OptPara

                            'Schritte 1 - 3: PES wird initialisiert (Weiteres siehe dort ;-)
                            '**************************************************************
                            Call PES1.PesInitialise(EVO_Einstellungen1.Settings, Me.mProblem)

                            'Die PopulationsEltern des PES werden gefüllt
                            For m = 0 To CES1.PES_Parents_pLoc.GetUpperBound(0)
                                Call PES1.EsStartvalues(CES1.mSettings.CES.is_PopMutStart, CES1.PES_Parents_pLoc(m).Loc(i_loc).PES_OptPara, m)
                            Next

                            'Startet die Prozesse evolutionstheoretischen Prozesse nacheinander
                            Call PES1.EsReproMut(EVO_Einstellungen1.Settings.CES.is_PopMutStart)

                            'Auslesen der Variierten Parameter
                            CES1.Childs(i_ch).Loc(i_loc).PES_OptPara = EVO.Common.OptParameter.Clone_Array(PES1.EsGetParameter())

                    End Select
                End If

                'Hier wird mean Dn auf alle Locations und PES OptParas übertragen
                'CES1.Childs(i_ch).Set_mean_PES_Dn = CES1.Childs(i_ch).Get_mean_PES_Dn
            Next
        Next

        MI_Thread_OK = True
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
                Call Sim1.PREPARE_Evaluation_CES(CES1.Childs(i).Path, CES1.Childs(i).Get_All_Loc_Elem)

                'Hier werden Child die passenden Elemente zugewiesen
                Dim j As Integer
                For j = 0 To CES1.ModSett.n_Locations - 1
                    Call Sim1.Identify_Measures_Elements_Parameters(j, CES1.Childs(i).Path(j), CES1.Childs(i).Measures(j), CES1.Childs(i).Loc(j).Loc_Elem, CES1.Childs(i).Loc(j).PES_OptPara)
                Next

                'Reduktion der OptimierungsParameter und immer dann wenn nicht Nullvariante
                'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                If (Me.mProblem.Reduce_OptPara_and_ModPara(CES1.Childs(i).Get_All_Loc_Elem)) Then

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
        Dim ind As Common.Individuum_PES
        Dim QNBest() As Double = {}
        Dim QBest() As Double = {}
        Dim aktuellePara(Me.mProblem.NumParams - 1) As Double
        Dim SIM_Eval_is_OK As Boolean
        Dim durchlauf As Long
        Dim Iterationen As Long
        Dim Tastschritte_aktuell As Long
        Dim Tastschritte_gesamt As Long
        Dim Extrapolationsschritte As Long
        Dim Rueckschritte As Long

        Dim HookJeeves As EVO.Kern.HookeAndJeeves = New EVO.Kern.HookeAndJeeves(Me.mProblem.NumParams, EVO_Einstellungen1.Settings.HookJeeves.DnStart, EVO_Einstellungen1.Settings.HookJeeves.DnFinish)

        'Individuumsklasse wird initialisiert
        Call Common.Individuum.Initialise(Me.mProblem)

        ReDim QNBest(Me.mProblem.NumPenalties - 1)
        ReDim QBest(Me.mProblem.NumPenalties - 1)

        'Diagramm vorbereiten und initialisieren
        Call PrepareDiagramm()

        durchlauf = 0
        Tastschritte_aktuell = 0
        Tastschritte_gesamt = 0
        Extrapolationsschritte = 0
        Rueckschritte = 0
        Iterationen = 0
        b = False

        Call HookJeeves.Initialize(Me.mProblem.List_OptParameter)

        'Initialisierungssimulation
        For i = 0 To Me.mProblem.NumParams - 1
            aktuellePara(i) = Me.mProblem.List_OptParameter(i).Xn
        Next
        QNBest(0) = 1.79E+308
        QBest(0) = 1.79E+308
        k = 0

        Do While (HookJeeves.AktuelleSchrittweite > HookJeeves.MinimaleSchrittweite)

            Iterationen += 1
            durchlauf += 1

            'Bestimmen der Ausgangsgüte
            '==========================
            'Individuum instanzieren
            ind = New Common.Individuum_PES("HJ", durchlauf)

            'HACK: OptParameter ins Individuum kopieren
            For i = 0 To ind.OptParameter.Length - 1
                ind.OptParameter(i).Xn = aktuellePara(i)
            Next

            'Vorbereiten des Modelldatensatzes
            Call Sim1.PREPARE_Evaluation_PES(ind.OptParameter)

            'Evaluierung des Simulationsmodells (ToDo: Validätsprüfung fehlt)
            SIM_Eval_is_OK = Sim1.launchSim(0, 0)
            If SIM_Eval_is_OK Then Call Sim1.SIM_Ergebnis_auswerten(ind)

            'Lösung im TeeChart einzeichnen
            '------------------------------
            Dim serie As Steema.TeeChart.Styles.Series
            serie = Me.Hauptdiagramm1.getSeriesPoint("Hook and Jeeves")
            Call serie.Add(durchlauf, ind.Penalties(0), durchlauf.ToString())

            Call My.Application.DoEvents()

            'Penalties in Bestwert kopieren
            Call ind.Penalties.CopyTo(QNBest, 0)

            'Tastschritte
            '============
            For j = 0 To HookJeeves.AnzahlParameter - 1

                aktuellePara = HookJeeves.Tastschritt(j, Kern.HookeAndJeeves.TastschrittRichtung.Vorwärts)

                Tastschritte_aktuell += 1
                durchlauf += 1
                Me.EVO_Einstellungen1.Label_HJ_TSaktuelle.Text = Tastschritte_aktuell.ToString

                'Individuum instanzieren
                ind = New Common.Individuum_PES("HJ", durchlauf)

                'HACK: OptParameter ins Individuum kopieren
                For i = 0 To ind.OptParameter.Length - 1
                    ind.OptParameter(i).Xn = aktuellePara(i)
                Next

                'Vorbereiten des Modelldatensatzes
                Call Sim1.PREPARE_Evaluation_PES(ind.OptParameter)

                'Evaluierung des Simulationsmodells
                SIM_Eval_is_OK = Sim1.launchSim(0, 0)
                If SIM_Eval_is_OK Then Call Sim1.SIM_Ergebnis_auswerten(ind)

                'Lösung im TeeChart einzeichnen
                '------------------------------
                serie = Me.Hauptdiagramm1.getSeriesPoint("Hook and Jeeves")
                Call serie.Add(durchlauf, ind.Penalties(0), durchlauf.ToString())

                Call My.Application.DoEvents()

                If (ind.Penalties(0) >= QNBest(0)) Then

                    aktuellePara = HookJeeves.Tastschritt(j, Kern.HookeAndJeeves.TastschrittRichtung.Rückwärts)

                    Tastschritte_aktuell += 1
                    durchlauf += 1
                    Me.EVO_Einstellungen1.Label_HJ_TSaktuelle.Text = Tastschritte_aktuell.ToString

                    'Individuum instanzieren
                    ind = New Common.Individuum_PES("HJ", durchlauf)

                    'HACK: OptParameter ins Individuum kopieren
                    For i = 0 To ind.OptParameter.Length - 1
                        ind.OptParameter(i).Xn = aktuellePara(i)
                    Next

                    'Vorbereiten des Modelldatensatzes
                    Call Sim1.PREPARE_Evaluation_PES(ind.OptParameter)

                    'Evaluierung des Simulationsmodells
                    SIM_Eval_is_OK = Sim1.launchSim(0, 0)
                    If SIM_Eval_is_OK Then Call Sim1.SIM_Ergebnis_auswerten(ind)

                    'Lösung im TeeChart einzeichnen
                    '------------------------------
                    serie = Me.Hauptdiagramm1.getSeriesPoint("Hook and Jeeves")
                    Call serie.Add(durchlauf, ind.Penalties(0), durchlauf.ToString())

                    Call My.Application.DoEvents()

                    If (ind.Penalties(0) >= QNBest(0)) Then
                        aktuellePara = HookJeeves.TastschrittResetParameter(j)
                    Else
                        Call ind.Penalties.CopyTo(QNBest, 0)
                    End If
                Else
                    Call ind.Penalties.CopyTo(QNBest, 0)
                End If
            Next

            Tastschritte_gesamt += Tastschritte_aktuell
            Me.EVO_Einstellungen1.Label_HJ_TSgesamt.Text = Tastschritte_gesamt.ToString
            Tastschritte_aktuell = 0
            Me.EVO_Einstellungen1.Label_HJ_TSaktuelle.Text = Tastschritte_aktuell.ToString
            Me.EVO_Einstellungen1.Label_HJ_TSmittel.Text = Math.Round((Tastschritte_gesamt / Iterationen), 2).ToString

            Call My.Application.DoEvents()

            'Extrapolationsschritt
            If (QNBest(0) < QBest(0)) Then

                'Lösung im TeeChart einzeichnen
                '------------------------------
                serie = Me.Hauptdiagramm1.getSeriesPoint("Hook and Jeeves Best", "Green")
                Call serie.Add(durchlauf, ind.Penalties(0), durchlauf.ToString())

                Call My.Application.DoEvents()

                Call QNBest.CopyTo(QBest, 0)
                Call HookJeeves.Extrapolationsschritt()
                Extrapolationsschritte += 1
                Me.EVO_Einstellungen1.Label_HJ_ES.Text = Extrapolationsschritte.ToString
                Call My.Application.DoEvents()
                k += 1
                aktuellePara = HookJeeves.getLetzteParameter
                For i = 0 To HookJeeves.AnzahlParameter - 1
                    If aktuellePara(i) < 0 Or aktuellePara(i) > 1 Then
                        HookJeeves.Rueckschritt()
                        Rueckschritte += 1
                        Me.EVO_Einstellungen1.Label_HJ_RS.Text = Rueckschritte.ToString()
                        Call My.Application.DoEvents()
                        k += -1
                        HookJeeves.Schrittweitenhalbierung()
                        aktuellePara = HookJeeves.getLetzteParameter()
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
                    Me.EVO_Einstellungen1.Label_HJ_RS.Text = Rueckschritte.ToString()
                    Call My.Application.DoEvents()
                    HookJeeves.Schrittweitenhalbierung()
                    aktuellePara = HookJeeves.getLetzteParameter()
                Else
                    HookJeeves.Schrittweitenhalbierung()
                End If
            End If
        Loop
    End Sub

    'Anwendung Evolutionsstrategie für Parameter Optimierung - hier Steuerung       
    '************************************************************************
    Private Sub STARTEN_PES()

        Dim durchlauf As Integer
        Dim ind() As Common.Individuum_PES
        Dim PES1 As EVO.Kern.PES

        'Hypervolumen instanzieren
        Dim Hypervolume As EVO.MO_Indicators.Indicators
        Hypervolume = EVO.MO_Indicators.MO_IndicatorFabrik.GetInstance(EVO.MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, Me.mProblem.NumPenalties)

        'Datensätze für Multithreading kopieren (nur Sim-Anwendungen)
        If (Me.Anwendung <> ANW_TESTPROBLEME And n_Threads > 1) Then
            Call Sim1.createThreadWorkDirs(n_Threads)
        End If

        'Diagramm vorbereiten und initialisieren
        If (Not Me.mProblem.Method = METH_HYBRID And Not Me.EVO_Einstellungen1.Settings.CES.ty_Hybrid = Common.Constants.HYBRID_TYPE.Sequencial_1) Then
            Call PrepareDiagramm()
        End If

        'Individuumsklasse wird initialisiert
        Call Common.Individuum.Initialise(Me.mProblem)

        'Schritte 0: Objekt der Klasse PES wird erzeugt
        '**********************************************
        PES1 = New EVO.Kern.PES()

        'Schritte 1 - 3: ES wird initialisiert (Weiteres siehe dort ;-)
        '**************************************************************
        Call PES1.PesInitialise(EVO_Einstellungen1.Settings, Me.mProblem)

        'Startwerte werden der Verlaufsanzeige zugewiesen
        Call Me.EVO_Opt_Verlauf1.Initialisieren(EVO_Einstellungen1.Settings.PES.Pop.n_Runden, EVO_Einstellungen1.Settings.PES.Pop.n_Popul, EVO_Einstellungen1.Settings.PES.n_Gen, EVO_Einstellungen1.Settings.PES.n_Nachf)

        durchlauf = 0

Start_Evolutionsrunden:

        'Über alle Runden
        'xxxxxxxxxxxxxxxx
        For PES1.PES_iAkt.iAktRunde = 0 To EVO_Einstellungen1.Settings.PES.Pop.n_Runden - 1

            Call PES1.EsResetPopBWSpeicher() 'Nur bei Komma Strategie

            'Über alle Populationen
            'xxxxxxxxxxxxxxxxxxxxxx
            For PES1.PES_iAkt.iAktPop = 0 To EVO_Einstellungen1.Settings.PES.Pop.n_Popul - 1

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

                    Call PES1.EsResetBWSpeicher()  'Nur bei Komma Strategie
                    ReDim ind(EVO_Einstellungen1.Settings.PES.n_Nachf - 1)
                    Dim Child_False(-1) As Integer
                    Dim i As Integer

                    'Über alle Nachkommen
                    'xxxxxxxxxxxxxxxxxxxxxxxxx
                    For i = 0 To EVO_Einstellungen1.Settings.PES.n_Nachf - 1

                        durchlauf += 1

                        'Neues Individuum instanzieren
                        ind(i) = New Common.Individuum_PES("PES", durchlauf)

                        'REPRODUKTIONSPROZESS
                        '####################
                        'Ermitteln der neuen Ausgangswerte für Nachkommen aus den Eltern
                        Call PES1.EsReproduktion()

                        'MUTATIONSPROZESS
                        '################
                        'Mutieren der Ausgangswerte
                        Call PES1.EsMutation()

                        'Auslesen der Variierten Parameter und in Individuum kopieren
                        ind(i).OptParameter = EVO.Common.OptParameter.Clone_Array(PES1.EsGetParameter())

                        'Testprobleme direkt auswerten
                        If Anwendung = ANW_TESTPROBLEME Then

                            PES1.PES_iAkt.iAktNachf = i

                            'Lösung evaluieren und zeichnen
                            Call Testprobleme1.Evaluierung_TestProbleme(ind(i), PES1.PES_iAkt.iAktPop, Me.Hauptdiagramm1)
                            Me.Label_Dn_Wert.Text = Math.Round(ind(i).OptParameter(0).Dn, 6).ToString
                            Me.Monitor1.Zeichne_Dn((PES1.PES_iAkt.iAktGen + 1) * EVO_Einstellungen1.Settings.PES.n_Nachf + i, ind(i).OptParameter(0).Dn)

                            'Einordnen
                            Call PES1.EsBest(ind(i))

                            'Verlauf aktualisieren
                            Call EVO_Opt_Verlauf1.Nachfolger(PES1.PES_iAkt.iAktNachf + 1)

                            System.Windows.Forms.Application.DoEvents()

                        End If

                    Next

                    'Simulationsanwendungen nachträglich auswerten
                    If Anwendung = ANW_BLUEM Or Anwendung = ANW_SMUSI Or Anwendung = ANW_SCAN Or Anwendung = ANW_SWMM Then

                        Dim Thread_Free As Integer = 0
                        Dim Thread_Ready As Integer = 0
                        Dim Child_Run As Integer = 0
                        Dim Child_Ready As Integer = 0
                        Dim Ready As Boolean = False
                        System.Threading.Thread.CurrentThread.Priority = Threading.ThreadPriority.Normal

                        Do
                            'Falls eine Simulation frei und nicht Pause
                            '------------------------------------------
                            If Sim1.launchFree(Thread_Free) And Child_Run < EVO_Einstellungen1.Settings.PES.n_Nachf _
                            And (Child_Ready + n_Threads > Child_Run) And Me.ispause = False Then

                                Sim1.WorkDir_Current = Sim1.getThreadWorkDir(Thread_Free)

                                Call Sim1.PREPARE_Evaluation_PES(ind(Child_Run).OptParameter)

                                ' Simulation ******************************************
                                SIM_Eval_is_OK = Sim1.launchSim(Thread_Free, Child_Run)
                                '******************************************************

                                Child_Run += 1

                                'Falls Simulation fertig und erfogreich
                                '--------------------------------------
                            ElseIf Sim1.launchReady(Thread_Ready, SIM_Eval_is_OK, Child_Ready) = True And SIM_Eval_is_OK Then

                                Sim1.WorkDir_Current = Sim1.getThreadWorkDir(Thread_Ready)
                                Sim1.SIM_Ergebnis_auswerten(ind(Child_Ready))

                                'Lösung zeichnen und Dn ausgeben
                                Call Me.Hauptdiagramm1.ZeichneIndividuum(ind(Child_Ready), PES1.PES_iAkt.iAktRunde, PES1.PES_iAkt.iAktPop, PES1.PES_iAkt.iAktGen, Child_Ready, Color.Orange)
                                Me.Label_Dn_Wert.Text = Math.Round(ind(Child_Ready).OptParameter(0).Dn, 6).ToString
                                Me.Monitor1.Zeichne_Dn((PES1.PES_iAkt.iAktGen + 1) * EVO_Einstellungen1.Settings.PES.n_Nachf + Child_Ready, ind(Child_Ready).OptParameter(0).Dn)

                                'SELEKTIONSPROZESS Schritt 1
                                '###########################
                                'Einordnen der Qualitätsfunktion im Bestwertspeicher bei SO
                                'Falls MO Einordnen der Qualitätsfunktion in NDSorting
                                PES1.PES_iAkt.iAktNachf = Child_Ready
                                Call PES1.EsBest(ind(Child_Ready))

                                Call EVO_Opt_Verlauf1.Nachfolger(Child_Ready + 1)
                                System.Windows.Forms.Application.DoEvents()

                                If Child_Ready = EVO_Einstellungen1.Settings.PES.n_Nachf - 1 Then Ready = True
                                Child_Ready += 1

                                'Falls Simulation fertig aber nicht erfolgreich
                                '----------------------------------------------
                            ElseIf Sim1.launchReady(Thread_Ready, SIM_Eval_is_OK, Child_Ready) = False And SIM_Eval_is_OK = False Then

                                ReDim Preserve Child_False(Child_False.GetLength(0))
                                Child_False(Child_False.GetUpperBound(0)) = Child_Ready

                                If Child_Ready = EVO_Einstellungen1.Settings.PES.n_Nachf - 1 Then Ready = True
                                Child_Ready += 1

                                'Falls Pause und alle simulierten auch verarbeitet
                                '-------------------------------------------------
                            ElseIf Me.ispause = True And Child_Ready = Child_Run Then

                                Me.Button_Start.Text = "Run"
                                Do While (Me.ispause)
                                    System.Threading.Thread.Sleep(20)
                                    Application.DoEvents()
                                Loop

                                'Falls total im Stress
                                '---------------------
                            Else
                                System.Threading.Thread.Sleep(400)
                                Application.DoEvents()

                            End If

                        Loop While Ready = False

                    End If

                    'Do Schleife: Um Modellfehler bzw. Evaluierungsabbrüche abzufangen
                    If Child_False.GetLength(0) > -1 Then
                        For i = 0 To Child_False.GetUpperBound(0)
                            Dim Eval_Count As Integer = 0
                            Do
                                Call PES1.EsReproduktion()
                                Call PES1.EsMutation()

                                'Parameter aus PES ins Individuum kopieren
                                ind(Child_False(i)).OptParameter = EVO.Common.OptParameter.Clone_Array(PES1.EsGetParameter())

                                Sim1.WorkDir_Current = Sim1.getThreadWorkDir(0)
                                Call Sim1.PREPARE_Evaluation_PES(ind(Child_False(i)).OptParameter)

                                SIM_Eval_is_OK = Sim1.launchSim(0, Child_False(i))
                                While Sim1.launchReady(0, SIM_Eval_is_OK, Child_False(i)) = False
                                    System.Threading.Thread.Sleep(400)
                                    System.Windows.Forms.Application.DoEvents()
                                End While

                                'Lösung auswerten und zeichnen
                                If SIM_Eval_is_OK Then
                                    Call Sim1.SIM_Ergebnis_auswerten(ind(Child_False(i)))
                                    Call Me.Hauptdiagramm1.ZeichneIndividuum(ind(Child_False(i)), PES1.PES_iAkt.iAktRunde, PES1.PES_iAkt.iAktPop, PES1.PES_iAkt.iAktGen, Child_False(i), Color.Orange)
                                End If

                                PES1.PES_iAkt.iAktNachf = Child_False(i)
                                Call PES1.EsBest(ind(Child_False(i)))

                                Call EVO_Opt_Verlauf1.Nachfolger(Child_False(i) + 1)
                                System.Windows.Forms.Application.DoEvents()

                                Eval_Count += 1
                                If (Eval_Count >= 10) Then
                                    Throw New Exception("Es konnte kein gültiger Datensatz erzeugt werden!")
                                End If
                            Loop While SIM_Eval_is_OK = False
                        Next
                    End If

                    'SELEKTIONSPROZESS Schritt 2 für NDSorting sonst Xe = Xb
                    '#######################################################
                    'Die neuen Eltern werden generiert
                    Call PES1.EsEltern()

                    'Sekundäre Population
                    '====================
                    If (EVO_Einstellungen1.Settings.PES.OptModus = Common.Constants.EVO_MODUS.Multi_Objective) Then

                        'SekPop abspeichern
                        '------------------
                        If (Not IsNothing(Sim1)) Then
                            Call Sim1.OptResult.setSekPop(PES1.SekundärQb, PES1.PES_iAkt.iAktGen)
                        End If

                        'SekPop zeichnen
                        '---------------
                        Call Me.Hauptdiagramm1.ZeichneSekPopulation(PES1.SekundärQb)

                        'Hypervolumen berechnen und Zeichnen
                        '-----------------------------------
                        Call Hypervolume.update_dataset(Common.Individuum.Get_All_Penalty_of_Array(PES1.SekundärQb))
                        Call Me.Hauptdiagramm1.ZeichneNadirpunkt(Hypervolume.nadir)
                        Call Me.Monitor1.ZeichneHyperVolumen(PES1.PES_iAkt.iAktGen, Math.Abs(Hypervolume.calc_indicator()))

                    End If

                    'ggf. alte Generation im Diagramm löschen
                    If (Me.Options.showOnlyCurrentPop _
                        And PES1.PES_iAkt.iAktGen < EVO_Einstellungen1.Settings.PES.n_Gen - 1) Then
                        Call Me.Hauptdiagramm1.LöscheLetzteGeneration(PES1.PES_iAkt.iAktPop)
                    End If

                    Call EVO_Opt_Verlauf1.Generation(PES1.PES_iAkt.iAktGen + 1)
                    System.Windows.Forms.Application.DoEvents()

                Next 'Ende alle Generationen
                'xxxxxxxxxxxxxxxxxxxxxxxxxxx
                System.Windows.Forms.Application.DoEvents()

                'POPULATIONS SELEKTIONSPROZESS  Schritt 1
                '########################################
                'Einordnen der Qualitätsfunktion im PopulationsBestwertspeicher
                Call PES1.EsPopBest()
                Call EVO_Opt_Verlauf1.Population(PES1.PES_iAkt.iAktPop + 1)

            Next 'Ende alle Populationen
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

            'POPULATIONS SELEKTIONSPROZESS  Schritt 2
            '########################################
            'Die neuen Populationseltern werden generiert
            Call PES1.EsPopEltern()
            System.Windows.Forms.Application.DoEvents()
            Call EVO_Opt_Verlauf1.Runden(PES1.PES_iAkt.iAktRunde + 1)

        Next 'Ende alle Runden
        'xxxxxxxxxxxxxxxxxxxxx

    End Sub

#End Region 'Start Button Pressed

#Region "Diagrammfunktionen"

    'Diagrammfunktionen
    '###################

    'Hauptdiagramm bearbeiten
    '************************
    Private Sub Button_TChartEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChartEdit.Click
        Call Me.Hauptdiagramm1.TChartEdit(sender, e)
    End Sub

    'Chart nach Excel exportieren
    '****************************
    Private Sub TChart2Excel(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChart2Excel.Click
        SaveFileDialog1.DefaultExt = Me.Hauptdiagramm1.Export.Data.Excel.FileExtension
        SaveFileDialog1.FileName = Me.Hauptdiagramm1.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "Excel-Dateien (*.xls)|*.xls"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            Me.Hauptdiagramm1.Export.Data.Excel.Series = Nothing 'export all series
            Me.Hauptdiagramm1.Export.Data.Excel.IncludeLabels = True
            Me.Hauptdiagramm1.Export.Data.Excel.IncludeIndex = True
            Me.Hauptdiagramm1.Export.Data.Excel.IncludeHeader = True
            Me.Hauptdiagramm1.Export.Data.Excel.IncludeSeriesTitle = True
            Me.Hauptdiagramm1.Export.Data.Excel.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

    'Chart als PNG exportieren
    '*************************
    Private Sub TChart2PNG(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChart2PNG.Click
        SaveFileDialog1.DefaultExt = Me.Hauptdiagramm1.Export.Image.PNG.FileExtension
        SaveFileDialog1.FileName = Me.Hauptdiagramm1.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "PNG-Dateien (*.png)|*.png"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            Me.Hauptdiagramm1.Export.Image.PNG.GrayScale = False
            Me.Hauptdiagramm1.Export.Image.PNG.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

    'Chart in nativem TeeChart-Format abspeichern
    '********************************************
    Private Sub TChartSave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TChartSave.Click
        SaveFileDialog1.DefaultExt = Me.Hauptdiagramm1.Export.Template.FileExtension
        SaveFileDialog1.FileName = Me.Hauptdiagramm1.Name + "." + SaveFileDialog1.DefaultExt
        SaveFileDialog1.Filter = "TeeChart-Dateien (*.ten)|*.ten"
        If (Me.SaveFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            Me.Hauptdiagramm1.Export.Template.IncludeData = True
            Me.Hauptdiagramm1.Export.Template.Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

    'Hauptdiagramm vorbereiten
    '*************************
    Private Sub PrepareDiagramm()

        Dim i, j, tmpZielindex() As Integer
        Dim Achse As EVO.Diagramm.Diagramm.Achse
        Dim Achsen As New Collection

        ReDim tmpZielindex(2)                       'Maximal 3 Achsen
        'Zunächst keine Achsenzuordnung (-1)
        For i = 0 To tmpZielindex.GetUpperBound(0)
            tmpZielindex(i) = -1
        Next

        'Fallunterscheidung Anwendung/Methode
        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        Select Case Anwendung

            Case ANW_TESTPROBLEME 'Testprobleme
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                Call Testprobleme1.DiagInitialise(Me.EVO_Einstellungen1.Settings, Me.Hauptdiagramm1)

            Case ANW_BLUEM, ANW_SMUSI, ANW_SCAN, ANW_SWMM
                'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                Select Case Me.mProblem.Method

                    Case METH_SENSIPLOT 'SensiPlot
                        'XXXXXXXXXXXXXXXXXXXXXXXXX

                        If (SensiPlot1.Selected_OptParameter.GetLength(0) = 1) Then

                            '1 OptParameter:
                            '---------------

                            'Achsen:
                            '-------
                            'X-Achse = QWert
                            Achse.Title = Me.mProblem.List_Penaltyfunctions(SensiPlot1.Selected_Penaltyfunction).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)
                            'Y-Achse = OptParameter
                            Achse.Title = Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)

                            'Achsenzuordnung
                            'BUG 327!
                            For i = 0 To Me.mProblem.NumFeatures - 1
                                If (Me.mProblem.List_Featurefunctions(i).Bezeichnung = Me.mProblem.List_Penaltyfunctions(SensiPlot1.Selected_Penaltyfunction).Bezeichnung) Then
                                    Me.Hauptdiagramm1.ZielIndexX = i
                                    Exit For 'Abbruch
                                End If
                            Next
                            Me.Hauptdiagramm1.ZielIndexY = -1
                            Me.Hauptdiagramm1.ZielIndexZ = -1

                        Else
                            '2 OptParameter:
                            '---------------

                            'Achsen:
                            '-------
                            'X-Achse = OptParameter1
                            Achse.Title = Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(0)).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)
                            'Y-Achse = QWert
                            Achse.Title = Me.mProblem.List_Penaltyfunctions(SensiPlot1.Selected_Penaltyfunction).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)
                            'Z-Achse = OptParameter2
                            Achse.Title = Me.mProblem.List_OptParameter(SensiPlot1.Selected_OptParameter(1)).Bezeichnung
                            Achse.Automatic = True
                            Achse.Maximum = 0
                            Achsen.Add(Achse)

                            'Achsenzuordnung
                            'BUG 327!
                            Me.Hauptdiagramm1.ZielIndexX = -1
                            For i = 0 To Me.mProblem.NumFeatures - 1
                                If (Me.mProblem.List_Featurefunctions(i).Bezeichnung = Me.mProblem.List_Penaltyfunctions(SensiPlot1.Selected_Penaltyfunction).Bezeichnung) Then
                                    Me.Hauptdiagramm1.ZielIndexY = i
                                    Exit For 'Abbruch
                                End If
                            Next
                            Me.Hauptdiagramm1.ZielIndexZ = -1

                        End If

                        'Diagramm initialisieren
                        Call Me.Hauptdiagramm1.DiagInitialise(Anwendung, Achsen, Me.EVO_Einstellungen1.Settings, Me.mProblem)


                    Case Else 'PES, CES, CES + PES, HYBRID, HOOK & JEEVES
                        'XXXXXXXXXXXXXXXXXXXXX

                        'Achsen:
                        '-------
                        If (Me.mProblem.NumPenalties = 1) Then

                            'Single-Objective
                            '================

                            'X-Achse (Nr. der Simulation (Durchlauf))
                            '----------------------------------------
                            Achse.Title = "Simulation"
                            Achse.Automatic = False
                            If (Me.mProblem.Method = METH_PES) Then
                                'Bei PES:
                                If (EVO_Einstellungen1.Settings.PES.Pop.is_POPUL) Then
                                    Achse.Maximum = EVO_Einstellungen1.Settings.PES.n_Gen * EVO_Einstellungen1.Settings.PES.n_Nachf * EVO_Einstellungen1.Settings.PES.Pop.n_Runden + 1
                                Else
                                    Achse.Maximum = EVO_Einstellungen1.Settings.PES.n_Gen * EVO_Einstellungen1.Settings.PES.n_Nachf + 1
                                End If

                            ElseIf (Me.mProblem.Method = METH_HOOKJEEVES) Then
                                'Bei Hooke & Jeeves:
                                Achse.Automatic = True

                            Else
                                'Bei CES etc.:
                                Achse.Maximum = EVO_Einstellungen1.Settings.CES.n_Childs * EVO_Einstellungen1.Settings.CES.n_Generations
                            End If

                            Achsen.Add(Achse)

                            'Y-Achse: erste (und einzige) Zielfunktion
                            '-----------------------------------------
                            For i = 0 To Me.mProblem.NumFeatures - 1
                                If (Me.mProblem.List_Featurefunctions(i).isPenalty) Then
                                    Achse.Title = Me.mProblem.List_Featurefunctions(i).Bezeichnung
                                    Achse.Automatic = True
                                    Achse.Maximum = 0
                                    Exit For 'Abbruch nach erstem OptZiel
                                End If
                            Next

                            Achsen.Add(Achse)

                            'Achsenzuordnung speichern
                            '-------------------------
                            Me.Hauptdiagramm1.ZielIndexX = -1
                            Me.Hauptdiagramm1.ZielIndexY = i
                            Me.Hauptdiagramm1.ZielIndexZ = -1

                        Else

                            'Multi-Objective
                            '===============

                            'für jedes OptZiel eine Achse hinzufügen
                            j = 0
                            For i = 0 To Me.mProblem.NumFeatures - 1
                                If (Me.mProblem.List_Featurefunctions(i).isPenalty) Then
                                    Achse.Title = Me.mProblem.List_Featurefunctions(i).Bezeichnung
                                    Achse.Automatic = True
                                    Achse.Maximum = 0
                                    Achsen.Add(Achse)
                                    tmpZielindex(j) = i
                                    j += 1
                                    If (j > 2) Then Exit For 'Maximal 3 Achsen
                                End If
                            Next

                            'Achsenzuordnung speichern:
                            '-------------------------
                            Me.Hauptdiagramm1.ZielIndexX = tmpZielindex(0)
                            Me.Hauptdiagramm1.ZielIndexY = tmpZielindex(1)
                            Me.Hauptdiagramm1.ZielIndexZ = tmpZielindex(2)

                            'Warnung bei mehr als 3 OptZielen
                            If (Me.mProblem.NumPenalties > 3) Then
                                MsgBox("Die Anzahl der Penalty-Funktionen beträgt mehr als 3!" & eol _
                                        & "Es werden nur die ersten drei Penalty-Funktionen im Hauptdiagramm angezeigt!", MsgBoxStyle.Information)
                            End If

                        End If

                        'Diagramm initialisieren
                        Call Me.Hauptdiagramm1.DiagInitialise(Anwendung, Achsen, Me.EVO_Einstellungen1.Settings, Me.mProblem)

                        'IstWerte in Diagramm einzeichnen
                        Call Me.Hauptdiagramm1.ZeichneIstWerte()

                End Select

        End Select

        Call Application.DoEvents()

    End Sub

    'Scatterplot-Matrix anzeigen
    '****************************
    Private Sub showScatterplot(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Scatterplot.Click

        Dim Dialog As EVO.Diagramm.ScatterplotDialog
        Dim diagresult As DialogResult
        Dim sekpoponly, showRef As Boolean
        Dim zielauswahl() As Integer

        'Scatterplot-Dialog aufrufen
        Dialog = New EVO.Diagramm.ScatterplotDialog(Me.mProblem)
        If (IsNothing(Sim1.OptResultRef)) Then Dialog.GroupBox_Ref.Enabled = False
        diagresult = Dialog.ShowDialog()

        If (Not diagresult = Windows.Forms.DialogResult.OK) Then
            Exit Sub
        End If

        'Einstellungen übernehmen
        sekpoponly = Dialog.CheckBox_SekPopOnly.Checked
        showRef = Dialog.CheckBox_showRef.Checked
        ReDim zielauswahl(-1)
        For Each indexChecked As Integer In Dialog.CheckedListBox_Ziele.CheckedIndices
            ReDim Preserve zielauswahl(zielauswahl.GetUpperBound(0) + 1)
            zielauswahl(zielauswahl.GetUpperBound(0)) = indexChecked
        Next

        'Scatterplot-Matrix anzeigen
        Cursor = Cursors.WaitCursor

        scatterplot1 = New EVO.Diagramm.Scatterplot(Me.mProblem, Sim1.OptResult, Sim1.OptResultRef, zielauswahl, sekpoponly, showRef)
        Call scatterplot1.Show()

        Cursor = Cursors.Default

        Call scatterplot1.BringToFront()

    End Sub

    'Speichert die verwendeten Farben für die bisherigen Pfade und generiert neue, falls erforderlich
    '************************************************************************************************
    Private Function ColorManagement(ByRef ColorArray(,) As Object, ByVal ind As Common.Individuum_CES) As Color
        Dim i, j As Integer
        Dim count As Integer
        Dim Farbe As Color = Color.White

        'Falls der Pfad schon vorhanden ist wird diese Farbe verwendet
        For i = 0 To ColorArray.GetUpperBound(1)
            count = 0
            For j = 1 To ColorArray.GetUpperBound(0)
                If ColorArray(j, i) = ind.Path(j - 1) Then
                    count += 1
                End If
            Next
            If count = ind.Path.GetLength(0) Then
                Farbe = ColorArray(0, i)
            End If
        Next


        'Für Farbverläufe __________________________________________________________________________________

        'If ColorAray.GetLength(1) = 0 then
        '    ReDim Preserve ColorAray(ColorAray.GetUpperBound(0), ColorAray.GetLength(1))
        '    Farbe = Color.FromArgb(255, 0, 255, 255)
        'ElseIf Farbe = Color.White Then

        '    Farbe = ColorAray(0, ColorAray.GetUpperBound(1))
        '    ReDim Preserve ColorAray(ColorAray.GetUpperBound(0), ColorAray.GetLength(1))

        '    Dim R As Integer = Farbe.R
        '    Dim G As Integer = Farbe.G
        '    Dim B As Integer = Farbe.B

        '    G = G - 50
        '    If G < 0 then
        '        G = 255
        '        B = B - 50
        '        If B < 100
        '            B = 255
        '            R = R + 50
        '            If R > 255
        '                Throw New Exception("Die Anzahl der farben für die verschiedenen Pfade ist erschöpft")
        '            End If
        '        End If
        '    End If

        '    Farbe = color.FromArgb(255, R, G, B)
        '    ColorAray(0, ColorAray.GetUpperBound(1)) = Farbe

        '    For i = 1 To ColorAray.GetUpperBound(0)
        '        ColorAray(i, ColorAray.GetUpperBound(1)) = ind.Path(i - 1)
        '    Next

        'End If


        'Für zufällige Farben _________________________________________________________________________________

        If Farbe = Color.White Then
            ReDim Preserve ColorArray(ColorArray.GetUpperBound(0), ColorArray.GetLength(1))
            Dim NeueFarbe As Boolean = True
            Dim CountFarbe As Integer = 0
            Do
                Randomize()
                'Genriert Zahl zwischen
                Farbe = Drawing.Color.FromArgb(255, CInt(Int((50 * Rnd()) + 1)) * 5, _
                                                    CInt(Int((50 * Rnd()) + 1)) * 5, _
                                                    CInt(Int((50 * Rnd()) + 1)) * 5)
                For i = 0 To ColorArray.GetUpperBound(1)
                    If Farbe = ColorArray(0, i) Then
                        NeueFarbe = False
                    End If
                Next
                CountFarbe += 1

                If CountFarbe > 15000 Then
                    Farbe = Color.White
                    NeueFarbe = True
                End If
                'If CountFarbe > 15000 Then Throw New Exception("Die Anzahl der farben für die verschiedenen Pfade ist erschöpft")
            Loop Until NeueFarbe = True
            ColorArray(0, ColorArray.GetUpperBound(1)) = Farbe
            For i = 1 To ColorArray.GetUpperBound(0)
                ColorArray(i, ColorArray.GetUpperBound(1)) = ind.Path(i - 1)
            Next
        End If

        Return Farbe

    End Function


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
            Dim ind As Common.Individuum

            Try
                'Solution-ID
                indID_clicked = s.Labels(valueIndex)

                'Lösung holen
                ind = Sim1.OptResult.getSolution(indID_clicked)

                'Lösung auswählen
                Call Me.selectSolution(ind)
            Catch
                MsgBox("Lösung nicht auswählbar!", MsgBoxStyle.Information, "Info")
            End Try

        End If

    End Sub

    'Eine Lösung auswählen
    '*********************
    Private Sub selectSolution(ByVal ind As Common.Individuum) Handles scatterplot1.pointSelected

        Dim isOK As Boolean

        'Lösung zu ausgewählten Lösungen hinzufügen
        isOK = Sim1.OptResult.selectSolution(ind.ID)

        If (isOK) Then

            'Lösungsdialog initialisieren
            If (IsNothing(Me.solutionDialog)) Then
                Me.solutionDialog = New SolutionDialog(Me.mProblem)
            End If

            'Lösungsdialog anzeigen
            Call Me.solutionDialog.Show()

            'Lösung zum Lösungsdialog hinzufügen
            Call Me.solutionDialog.addSolution(ind)

            'Lösung im Hauptdiagramm anzeigen
            Call Me.Hauptdiagramm1.ZeichneAusgewählteLösung(ind)

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
        Call Me.Hauptdiagramm1.LöscheAusgewählteLösungen()

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

        Dim isOK As Boolean
        Dim isIHA As Boolean
        Dim WorkDir_Prev As String

        Dim zre As Wave.Zeitreihe
        Dim SimSeries As New Collection                 'zu zeichnende Simulationsreihen
        Dim RefSeries As New Collection                 'zu zeichnende Referenzreihen

        'Wait cursor
        Cursor = Cursors.WaitCursor

        'Simulationen in Originalverzeichnis ausführen (ohne Threads),
        'WorDir_Current aber merken, und am Ende wieder setzen!
        WorkDir_Prev = Sim1.WorkDir_Current
        Sim1.WorkDir_Current = Sim1.WorkDir_Original

        'Wave instanzieren
        Dim Wave1 As New Wave.Wave()

        'Sonderfall BlueM mit IHA-Berechnung 
        'ein 2. Wave für RVA-Diagramme instanzieren
        Dim Wave2 As Wave.Wave = Nothing
        If (TypeOf Me.Sim1 Is EVO.Apps.BlueM) Then
            If (CType(Me.Sim1, EVO.Apps.BlueM).isIHA) Then
                isIHA = True
                Wave2 = New Wave.Wave()
                Call Wave2.PrepareChart_RVA()
                'IHA-Vergleichsmodus?
                If (CType(Me.Sim1, EVO.Apps.BlueM).IHAProc.isComparison) Then
                    'Referenz-RVAErgebnis in Wave2 laden
                    Call Wave2.Display_RVA(CType(Me.Sim1, EVO.Apps.BlueM).IHAProc.RVABase)
                End If
            End If
        End If

        'Alle ausgewählten Lösungen durchlaufen
        '======================================
        For Each ind As Common.Individuum In Sim1.OptResult.getSelectedSolutions()

            'Lösung per Checkbox ausgewählt?
            '-------------------------------
            If (Not checkedSolutions.Contains(ind.ID.ToString())) Then
                Continue For
            End If

            'Individuum in Sim evaluieren (ohne in DB zu speichern)
            isOK = Sim1.Evaluate(ind, False)

            'TODO: Simulationsfehler abfangen!

            'Sonderfall IHA-Berechnung
            If (isIHA) Then
                'RVA-Ergebnis in Wave2 laden
                Dim RVAResult As Wave.RVA.Struct_RVAValues
                RVAResult = CType(Me.Sim1, EVO.Apps.BlueM).IHASys.RVAResult
                'Lösungsnummer an Titel anhängen
                RVAResult.Title = "Lösung " & ind.ID.ToString()
                Call Wave2.Display_RVA(RVAResult)
            End If

            'Zu zeichnenden Simulationsreihen zurücksetzen
            SimSeries.Clear()

            'zu zeichnenden Reihen aus Liste der Ziele raussuchen
            '----------------------------------------------------
            For Each feature As Common.Featurefunction In Me.mProblem.List_Featurefunctions

                With feature

                    'Referenzreihe in Wave laden
                    '---------------------------
                    If (.Typ = "Reihe" Or .Typ = "IHA") Then
                        'Referenzreihen nur jeweils ein Mal zeichnen
                        If (Not RefSeries.Contains(.RefReiheDatei & .RefGr)) Then
                            RefSeries.Add(.RefGr, .RefReiheDatei & .RefGr)
                            'Referenzreihe in Wave laden
                            Wave1.Display_Series(.RefReihe)
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

        'Simulationsverzeichnis zurücksetzen
        Sim1.WorkDir_Current = WorkDir_Prev

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
        Me.SaveFileDialog1.InitialDirectory = Sim1.WorkDir_Original
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
        Me.OpenFileDialog1.InitialDirectory = Sim1.WorkDir_Original
        diagresult = Me.OpenFileDialog1.ShowDialog()

        If (diagresult = Windows.Forms.DialogResult.OK) Then

            sourceFile = Me.OpenFileDialog1.FileName

            'MDBImportDialog
            '---------------
            Dim importDialog As New EVO.OptResult.MDBImportDialog(Me.mProblem)

            diagresult = importDialog.ShowDialog()

            If (diagresult = Windows.Forms.DialogResult.OK) Then

                'Cursor Wait
                Cursor = Cursors.WaitCursor

                'Daten einlesen
                '==============
                Call Sim1.OptResult.db_load(sourceFile)

                'Hauptdiagramm
                '=============

                'Achsenzuordnung
                '---------------
                Me.Hauptdiagramm1.ZielIndexX = importDialog.ListBox_ZieleX.SelectedIndex
                Me.Hauptdiagramm1.ZielIndexY = importDialog.ListBox_ZieleY.SelectedIndex
                Me.Hauptdiagramm1.ZielIndexZ = importDialog.ListBox_ZieleZ.SelectedIndex

                'Achsen
                '------
                Dim Achsen As New Collection
                Dim tmpAchse As EVO.Diagramm.Diagramm.Achse
                tmpAchse.Automatic = True
                If (Me.Hauptdiagramm1.ZielIndexZ = -1 And Me.Hauptdiagramm1.ZielIndexY = -1) Then
                    'Single-objective
                    '----------------
                    'X-Achse
                    tmpAchse.Title = "Simulation"
                    Achsen.Add(tmpAchse)
                    'Y-Achse
                    tmpAchse.Title = Me.mProblem.List_Featurefunctions(Me.Hauptdiagramm1.ZielIndexX).Bezeichnung
                    Achsen.Add(tmpAchse)
                Else
                    'Multi-objective
                    '---------------
                    'X-Achse
                    tmpAchse.Title = Me.mProblem.List_Featurefunctions(Me.Hauptdiagramm1.ZielIndexX).Bezeichnung
                    Achsen.Add(tmpAchse)
                    'Y-Achse
                    tmpAchse.Title = Me.mProblem.List_Featurefunctions(Me.Hauptdiagramm1.ZielIndexY).Bezeichnung
                    Achsen.Add(tmpAchse)
                    If (Not Me.Hauptdiagramm1.ZielIndexZ = -1) Then
                        'Z-Achse
                        tmpAchse.Title = Me.mProblem.List_Featurefunctions(Me.Hauptdiagramm1.ZielIndexZ).Bezeichnung
                        Achsen.Add(tmpAchse)
                    End If
                End If

                'Diagramm initialisieren
                '-----------------------
                Me.Hauptdiagramm1.Clear()
                Me.Hauptdiagramm1.DiagInitialise(Path.GetFileName(sourceFile), Achsen, Me.EVO_Einstellungen1.Settings, Me.mProblem)

                'IstWerte in Diagramm einzeichnen
                Call Me.Hauptdiagramm1.ZeichneIstWerte()

                Call My.Application.DoEvents()

                'Punkte eintragen
                '----------------
                Dim serie As Steema.TeeChart.Styles.Series
                Dim serie3D As Steema.TeeChart.Styles.Points3D

                'Lösungen
                '========
                If (importDialog.ComboBox_SekPop.SelectedItem <> "ausschließlich") Then

                    For Each ind As Common.Individuum In Sim1.OptResult.Solutions

                        If (Me.Hauptdiagramm1.ZielIndexZ = -1 And Me.Hauptdiagramm1.ZielIndexY = -1) Then
                            '1D
                            '--
                            'Constraintverletzung prüfen
                            If (ind.Is_Feasible) Then
                                serie = Me.Hauptdiagramm1.getSeriesPoint("Population", "red")
                            Else
                                serie = Me.Hauptdiagramm1.getSeriesPoint("Population (ungültig)", "Gray")
                            End If
                            'Zeichnen
                            serie.Add(ind.ID, ind.Features(Me.Hauptdiagramm1.ZielIndexX), ind.ID.ToString())
                        ElseIf (Me.Hauptdiagramm1.ZielIndexZ = -1) Then
                            '2D
                            '--
                            'Constraintverletzung prüfen
                            If (ind.Is_Feasible) Then
                                serie = Me.Hauptdiagramm1.getSeriesPoint("Population", "Orange")
                            Else
                                serie = Me.Hauptdiagramm1.getSeriesPoint("Population (ungültig)", "Gray")
                            End If
                            'Zeichnen
                            serie.Add(ind.Features(Me.Hauptdiagramm1.ZielIndexX), ind.Features(Me.Hauptdiagramm1.ZielIndexY), ind.ID.ToString())
                        Else
                            '3D
                            '--
                            'Constraintverletzung prüfen
                            If (ind.Is_Feasible) Then
                                serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Population", "Orange")
                            Else
                                serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Population (ungültig)", "Gray")
                            End If
                            'Zeichnen
                            serie3D.Add(ind.Features(Me.Hauptdiagramm1.ZielIndexX), ind.Features(Me.Hauptdiagramm1.ZielIndexY), ind.Features(Me.Hauptdiagramm1.ZielIndexZ), ind.ID.ToString())
                        End If

                    Next

                End If

                Call My.Application.DoEvents()

                'Sekundärpopulation
                '==================
                If (importDialog.ComboBox_SekPop.SelectedItem <> "keine") Then

                    For Each sekpopind As Common.Individuum In Sim1.OptResult.getSekPop()
                        If (Me.Hauptdiagramm1.ZielIndexZ = -1) Then
                            '2D
                            '--
                            serie = Me.Hauptdiagramm1.getSeriesPoint("Sekundäre Population", "Green")
                            serie.Add(sekpopind.Features(Me.Hauptdiagramm1.ZielIndexX), sekpopind.Features(Me.Hauptdiagramm1.ZielIndexY), sekpopind.ID.ToString())
                        Else
                            '3D
                            '--
                            serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Sekundäre Population", "Green")
                            serie3D.Add(sekpopind.Features(Me.Hauptdiagramm1.ZielIndexX), sekpopind.Features(Me.Hauptdiagramm1.ZielIndexY), sekpopind.Features(Me.Hauptdiagramm1.ZielIndexZ), sekpopind.ID.ToString())
                        End If
                    Next

                End If

                Call My.Application.DoEvents()

                'Hypervolumen
                '============
                If (importDialog.CheckBox_Hypervol.Checked) Then

                    'Hypervolumen instanzieren
                    Dim Hypervolume As EVO.MO_Indicators.Indicators
                    Hypervolume = EVO.MO_Indicators.MO_IndicatorFabrik.GetInstance(EVO.MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, Me.mProblem.NumPenalties)
                    Dim indicator As Double
                    Dim nadir() As Double

                    'Alle Generationen durchlaufen
                    For Each sekpop As EVO.OptResult.OptResult.Struct_SekPop In Sim1.OptResult.SekPops

                        'Hypervolumen berechnen
                        Call Hypervolume.update_dataset(Sim1.OptResult.getSekPopValues(sekpop.iGen))
                        indicator = Math.Abs(Hypervolume.calc_indicator())
                        nadir = Hypervolume.nadir

                        'Hypervolumen zeichnen
                        Call Me.Hauptdiagramm1.ZeichneNadirpunkt(nadir)
                        Call Me.Monitor1.ZeichneHyperVolumen(sekpop.iGen, indicator)

                        Call My.Application.DoEvents()

                    Next
                End If

                'Ergebnis-Buttons
                Me.Button_Scatterplot.Enabled = True
                Me.Button_loadRefResult.Enabled = True

                'Start-Button deaktivieren
                Me.Button_Start.Enabled = False

                'Cursor Default
                Cursor = Cursors.Default

            End If

        End If

    End Sub

    'Vergleichsergebnis aus Datenbank laden
    '**************************************
    Private Sub loadRefFromMDB(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_loadRefResult.Click

        Dim diagresult As DialogResult
        Dim sourceFile As String

        'Datei-öffnen Dialog anzeigen
        Me.OpenFileDialog1.Filter = "Access-Datenbanken (*.mdb)|*.mdb"
        Me.OpenFileDialog1.Title = "Vergleichsergebnis: Ergebnisdatenbank auswählen"
        Me.OpenFileDialog1.FileName = ""
        Me.OpenFileDialog1.InitialDirectory = Sim1.WorkDir_Original
        diagresult = Me.OpenFileDialog1.ShowDialog()

        If (diagresult = Windows.Forms.DialogResult.OK) Then

            sourceFile = Me.OpenFileDialog1.FileName

            'Cursor Wait
            Cursor = Cursors.WaitCursor

            'Daten einlesen
            '==============
            Sim1.OptResultRef = New EVO.OptResult.OptResult(Me.Sim1.Datensatz, Me.mProblem, False)
            Call Sim1.OptResultRef.db_load(sourceFile, True)

            'In Diagramm anzeigen
            '====================
            Dim serie As Steema.TeeChart.Styles.Points
            Dim serie3D As Steema.TeeChart.Styles.Points3D

            For Each sekpopind As Common.Individuum In Sim1.OptResultRef.getSekPop()
                If (Me.Hauptdiagramm1.ZielIndexZ = -1) Then
                    '2D
                    '--
                    serie = Me.Hauptdiagramm1.getSeriesPoint("Vergleichsergebnis", "Blue")
                    serie.Add(sekpopind.Features(Me.Hauptdiagramm1.ZielIndexX), sekpopind.Features(Me.Hauptdiagramm1.ZielIndexY), "Vergleichsergebnis " & sekpopind.ID)
                Else
                    '3D
                    '--
                    serie3D = Me.Hauptdiagramm1.getSeries3DPoint("Vergleichsergebnis", "Blue")
                    serie3D.Add(sekpopind.Features(Me.Hauptdiagramm1.ZielIndexX), sekpopind.Features(Me.Hauptdiagramm1.ZielIndexY), sekpopind.Features(Me.Hauptdiagramm1.ZielIndexZ), sekpopind.ID & " (Vergleichsergebnis)")
                End If
            Next

            'Hypervolumen
            '============
            Dim i As Integer
            Dim sekpopvalues(,), sekpopvaluesRef(,) As Double
            Dim HypervolumeDiff, HypervolumeRef As EVO.MO_Indicators.Hypervolume
            Dim nadir() As Double
            Dim minmax() As Boolean
            Dim indicatorDiff, indicatorRef As Double

            'Vorbereitungen
            ReDim nadir(Me.mProblem.NumPenalties - 1)
            ReDim minmax(Me.mProblem.NumPenalties - 1)
            For i = 0 To Me.mProblem.NumPenalties - 1
                nadir(i) = 0
                minmax(i) = False
            Next
            sekpopvalues = Sim1.OptResult.getSekPopValues()
            sekpopvaluesRef = Sim1.OptResultRef.getSekPopValues()

            'Hypervolumendifferenz
            '---------------------
            If (sekpopvalues.Length > 0) Then

                'Instanzierung
                HypervolumeDiff = EVO.MO_Indicators.MO_IndicatorFabrik.GetInstance(MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, minmax, nadir, sekpopvalues, sekpopvaluesRef)

                'Berechnung
                indicatorDiff = -HypervolumeDiff.calc_indicator()

                'Nadir-Punkt holen (für spätere Verwendung bei Referenz-Hypervolumen)
                nadir = HypervolumeDiff.nadir

                'In Zwischenablage kopieren
                Call Clipboard.SetDataObject(indicatorDiff, True)

                'Anzeige in Messagebox
                MsgBox("Hypervolumendifferenz zum Vergleichsergebnis:" & eol _
                        & indicatorDiff.ToString() & eol _
                        & "(Wert wurde in die Zwischenablage kopiert)", MsgBoxStyle.Information, "Hypervolumen")

            End If

            'Referenz-Hypervolumen
            '---------------------
            'Instanzierung
            HypervolumeRef = EVO.MO_Indicators.MO_IndicatorFabrik.GetInstance(MO_Indicators.MO_IndicatorFabrik.IndicatorsType.Hypervolume, minmax, nadir, sekpopvaluesRef)
            indicatorRef = -HypervolumeRef.calc_indicator()

            'Im Monitor anzeigen
            Call Me.Monitor1.ZeichneReferenzHypervolumen(indicatorRef)

            'Cursor Default
            Cursor = Cursors.Default

        End If

    End Sub

#End Region 'Ergebnisdatenbank

    'Ermittelt beim Start die Anzahl der Physikalischen Prozessoren
    '**************************************************************
    Public Sub Anzahl_Prozessoren(ByRef n_Threads As Integer)

        Dim LogCPU As Integer = 0
        LogCPU = Environment.ProcessorCount

        If LogCPU = 1 Then
            n_Threads = 3
        ElseIf LogCPU = 2 Then
            n_Threads = 4
        ElseIf LogCPU = 4 Then
            n_Threads = 7
        End If

    End Sub

#End Region 'Methoden

End Class
