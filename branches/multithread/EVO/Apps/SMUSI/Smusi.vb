Imports System.IO
imports ihwb.SMUSI.DllAdapter

'*******************************************************************************
'*******************************************************************************
'**** Klasse SMUSI                                                          ****
'****                                                                       ****
'**** Funktionen zur Kontrolle von SMUSI                                    ****
'****                                                                       ****
'**** Autoren: Christoph Huebner, Felix Froehlich, Dirk Muschalla           ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'*******************************************************************************
'*******************************************************************************

Public Class Smusi
    Inherits Sim

#Region "Eigenschaften"

    'Eigenschaften
    '#############

    'SMUSI DLL
    '---------
    Private smusi_dll As SMUSI_EngineDotNetAccess


#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

    'Konstruktor
    '***********
    Public Sub New()

        Call MyBase.New()

        Me.mDatensatzendung = ".ALL"

    End Sub

    'Simulationsparameter einlesen
    '*****************************
    Protected Overrides Sub Read_SimParameter()

        Dim SimStart_str As String = ""
        Dim SimEnde_str As String = ""

        'ALL-Datei öffnen
        '----------------
        Dim Datei As String = Me.WorkDir & Me.Datensatz & ".ALL"

        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
        Dim StrReadSync As TextReader = TextReader.Synchronized(StrRead)

        'Alle Zeilen durchlaufen
        Dim Zeile As String
        Do
            Zeile = StrRead.ReadLine.ToString()

            'Simulationszeitraum auslesen
            If (Zeile.StartsWith("    SimBeginn - SimEnde")) Then
                SimStart_str = Zeile.Substring(37, 16)
                SimEnde_str = Zeile.Substring(56, 16)
                Exit Do 'Sobald das Datum gefunden wurde, kann die schleife verlassen werden
            End If

        Loop Until StrRead.Peek() = -1

        'Schließen der ALL-Datei
        StrReadSync.Close()
        StrRead.Close()
        FiStr.Close()

        'SimStart und SimEnde in echtes Datum konvertieren
        Me.SimStart = New DateTime(SimStart_str.Substring(6, 4), SimStart_str.Substring(3, 2), SimStart_str.Substring(0, 2), SimStart_str.Substring(11, 2), SimStart_str.Substring(14, 2), 0)
        Me.SimEnde = New DateTime(SimEnde_str.Substring(6, 4), SimEnde_str.Substring(3, 2), SimEnde_str.Substring(0, 2), SimEnde_str.Substring(11, 2), SimEnde_str.Substring(14, 2), 0)

        'Zeitschrittweite ist immer 5 Minuten
        Me.SimDT = New TimeSpan(0, 5, 0)

    End Sub

    'SMUSI ausführen (simulieren)
    '***********************************
    Public Overrides Function launchSim(ByVal Thread_ID As Integer, ByVal Child_ID As Integer) As Boolean

        Dim simOK As Boolean
        Dim SimCurrent, SimStart, SimEnde As DateTime
        Dim EXE_DLL As Boolean 'true = EXE, false = DLL

        EXE_DLL = True

        If EXE_DLL Then
            Dim exe_path As String
            Dim String1 As String
            Dim String3 As String
            dim String4 as string

            exe_path = System.Windows.Forms.Application.StartupPath() & "\SMUSI.WIN.exe"
            String1 = exe_path
            String3 = Me.WorkDir & Me.Datensatz & ".all"
            String4 = """

            Dim ExterneAnwendung As New System.Diagnostics.Process()

            externeanwendung.StartInfo.FileName = string1
            ExterneAnwendung.StartInfo.Arguments = String4 & String3 & String4
            externeanwendung.StartInfo.CreateNoWindow = True
            ExterneAnwendung.Start()

            Do While (Not ExterneAnwendung.HasExited)
                ExterneAnwendung.WaitForExit(250)
                My.Application.DoEvents()
            Loop

            If Not ExterneAnwendung.HasExited Then
                ExterneAnwendung.Kill()
            End If
            ExterneAnwendung = Nothing


            If (File.Exists(Me.WorkDir & Me.Datensatz & ".sum")) Then
                simOK = True
            Else
                simOK = False
            End If
        Else

            'SMUSI DLL instanzieren
            '----------------------
            Dim dll_path As String
            dll_path = System.Windows.Forms.Application.StartupPath() & "\SMUSI.dll"

            smusi_dll = Nothing

            If (File.Exists(dll_path)) Then
                smusi_dll = New SMUSI_EngineDotNetAccess(dll_path)
            Else
                Throw New Exception("SMUSI.dll nicht gefunden!")
            End If

            'Falls vorher schon initialisiert wurde
            Call smusi_dll.Finish()
            Call smusi_dll.Dispose()

            Try

                Call smusi_dll.Initialize(Me.WorkDir & Me.Datensatz)

                'Dim SimEnde As DateTime = SMUSI_EngineDotNetAccess.DateTime(smusi_dll.GetSimulationEndDate())

                ''Simulationszeitraum 
                'Do While (SMUSI_EngineDotNetAccess.DateTime(smusi_dll.GetCurrentTime) < SimEnde)
                '    Call smusi_dll.PerformTimeStep()
                'Loop
                'Simulationsdaten auslesen
                SimStart = SMUSI_EngineDotNetAccess.DateTime(smusi_dll.GetSimulationStartDate())
                SimEnde = SMUSI_EngineDotNetAccess.DateTime(smusi_dll.GetSimulationEndDate())
                SimCurrent = SimStart
                Do While (SimCurrent < SimEnde)

                    Call smusi_dll.PerformTimeStep()
                    'Me.ProgressBar1.PerformStep()

                    SimCurrent = SMUSI_EngineDotNetAccess.DateTime(smusi_dll.GetCurrentTime)

                    'If (SimCurrent.Hour = 0 And SimCurrent.Minute = 0) Then
                    '    'Nach Warnungen überprüfen
                    '    'Call checkForWarnings()
                    '    'Anzeige aktualisieren
                    '    Me.Label_SimDate.Text = SimCurrent
                    'End If

                    Application.DoEvents()

                Loop

                'Simulation erfolgreich
                simOK = True

            Catch ex As Exception

                'Simulationsfehler aufgetreten
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, "SMUSI")
                simOK = False

            Finally

                Call smusi_dll.Finish()
                Call smusi_dll.Dispose()

            End Try

            smusi_dll = Nothing

        End If

        Return simOK

    End Function

    Public Overrides Function launchFree(ByRef Thread_ID As Integer) As Boolean

    End Function
    Public Overrides Function launchReady(ByRef Thread_ID As Integer, ByVal Child_ID As Integer) As Boolean

    End Function

    'Simulationsergebnis verarbeiten
    '-------------------------------
    Public Overrides Sub WelDateiVerwursten()

        Dim datei, element As String
        Dim ASCtmp As Wave.ASC
        Dim elemente As New Collection()

        'Einzulesende Dateien zusammenstellen
        For Each ziel As Common.Ziel In Common.Manager.List_Ziele
            element = ziel.SimGr.Substring(0, 4)
            If (Not elemente.Contains(element)) Then
                elemente.Add(element, element)
            End If
        Next

        'Altes SimErgebnis löschen
        Me.SimErgebnis.Clear()

        'Dateien einlesen
        For Each elem As String In elemente
            datei = elem & "_WEL.ASC"
            ASCtmp = New Wave.ASC(Me.WorkDir & datei, True)
            'Simulationsergebnis abspeichern
            For Each zre As Wave.Zeitreihe In ASCtmp.Zeitreihen
                Me.SimErgebnis.Add(zre, elem & "_" & zre.ToString())
            Next
            ASCtmp = Nothing
        Next

        elemente = Nothing

    End Sub

    'Berechnung des Qualitätswerts (Zielwert)
    '****************************************
    Public Overrides Function QWert(ByVal ziel As Common.Ziel) As Double

        QWert = 0

        'Fallunterscheidung Ergebnisdatei
        '--------------------------------
        Select Case ziel.Datei

            Case "ASC"
                'QWert aus ASC-Datei
                QWert = QWert_ASC(ziel)

            Case Else
                Throw New Exception("Der Wert '" & ziel.Datei & "' für die Datei wird bei Optimierungszielen für SMUSI nicht akzeptiert!")

        End Select

        'Zielrichtung berücksichtigen
        QWert *= ziel.Richtung

    End Function

    'Qualitätswert aus ASC-Datei
    '***************************
    Private Function QWert_ASC(ByVal ziel As Common.Ziel) As Double

        Dim QWert As Double
        Dim SimReihe As Wave.Zeitreihe

        'Simulationsergebnis auslesen
        SimReihe = Me.SimErgebnis(ziel.SimGr)

        'Fallunterscheidung Zieltyp
        '--------------------------
        Select Case ziel.ZielTyp

            Case "Wert"
                QWert = MyBase.QWert_Wert(ziel, SimReihe)

            Case "Reihe"
                QWert = MyBase.QWert_Reihe(ziel, SimReihe)

        End Select

        Return QWert

    End Function


#Region "Kombinatorik"

    'Kombinatorik
    '############

    'Kombinatorik einlesen
    '*********************
    Protected Overrides Sub Read_Kombinatorik()

    End Sub

    'Liest die Verzweigungen aus SMUSI in ein Array ein
    'Und Dimensioniert das Verzweigungsarray
    '*******************************************************
    Protected Overrides Sub Read_Verzweigungen()

    End Sub

    'Schreibt die neuen Verzweigungen
    '********************************
    Protected Overrides Sub Write_Verzweigungen()

    End Sub

#End Region 'Kombinatorik

#End Region 'Methoden

End Class
