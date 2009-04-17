Imports System.IO
Imports IHWB.SMUSI.DllAdapter

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

#Region "Properties"

    ''' <summary>
    ''' Alle Dateiendungen (ohne Punkt), die in einem Datensatz vorkommen können
    ''' </summary>
    ''' <remarks>Die erste Dateiendung in dieser Collection repräsentiert den Datensatz (wird z.B. als Filter für OpenFile-Dialoge verwendet)</remarks>
    Public Overrides ReadOnly Property DatensatzDateiendungen() As Collections.Specialized.StringCollection
        Get
            Dim exts As Collections.Specialized.StringCollection = New Collections.Specialized.StringCollection()

            exts.AddRange(New String() {"ALL", "AUS", "BEK", "BKL", "BOF", "BWN", "DRO", _
                                        "EIN", "FKA", "JGG", "KLA", "RKL", "RUE", "SAM", _
                                        "SMZ", "SOP", "SYS", "TGG", "VER", "WIN", "WMB", _
                                        "XYZ"})

            'TODO: Dateiendungen für SMUSI-Datensatz auf Komplettheit prüfen

            Return exts

        End Get
    End Property

    ''' <summary>
    ''' Ob die Anwendung Multithreading unterstützt
    ''' </summary>
    ''' <returns>False</returns>
    Public Overrides ReadOnly Property MultithreadingSupported() As Boolean
        Get
            Return False
        End Get
    End Property


#End Region 'Properties

#Region "Methoden"

    'Methoden
    '########

    'Konstruktor
    '***********
    Public Sub New()

        Call MyBase.New()

    End Sub

    'Simulationsparameter einlesen
    '*****************************
    Protected Overrides Sub Read_SimParameter()

        Dim SimStart_str As String = ""
        Dim SimEnde_str As String = ""

        'ALL-Datei öffnen
        '----------------
        Dim Datei As String = Me.WorkDir_Original & Me.Datensatz & ".ALL"

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
    '****************************
    Public Overrides Function launchSim() As Boolean

        Dim simOK As Boolean
        Dim SimCurrent, SimStart, SimEnde As DateTime
        Dim EXE_DLL As Boolean 'true = EXE, false = DLL

        EXE_DLL = False

        If EXE_DLL Then
            Dim exe_path As String
            Dim String1 As String
            Dim String3 As String
            Dim String4 As String

            exe_path = System.Windows.Forms.Application.StartupPath() & "\SMUSI.WIN.exe"
            String1 = exe_path
            String3 = Me.WorkDir_Current & Me.Datensatz & ".all"
            String4 = """"

            Dim ExterneAnwendung As New System.Diagnostics.Process()

            ExterneAnwendung.StartInfo.FileName = String1
            ExterneAnwendung.StartInfo.Arguments = String4 & String3 & String4
            ExterneAnwendung.StartInfo.CreateNoWindow = True
            ExterneAnwendung.Start()

            Do While (Not ExterneAnwendung.HasExited)
                ExterneAnwendung.WaitForExit(250)
            Loop

            If Not ExterneAnwendung.HasExited Then
                ExterneAnwendung.Kill()
            End If
            ExterneAnwendung = Nothing


            If (File.Exists(Me.WorkDir_Current & Me.Datensatz & ".sum")) Then
                simOK = True
            Else
                simOK = False
            End If
        Else

            'SMUSI DLL instanzieren
            '----------------------
            Dim dll_path As String
            dll_path = System.Windows.Forms.Application.StartupPath() & "\SMUSI\SMUSI.dll"

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

                Call smusi_dll.Initialize(Me.WorkDir_Current & Me.Datensatz)

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

	'TODO: SMUSI Thread-Funktionen
	'#############################
    Public Overrides Function launchSim(ByVal Thread_ID As Integer, ByVal Child_ID As Integer) As Boolean
        Return Me.launchSim()
    End Function

    Public Overrides Function ThreadFree(ByRef Thread_ID As Integer) As Boolean
        Return True
    End Function

    Public Overrides Function ThreadReady(ByRef Thread_ID As Integer, ByRef SimIsOK As Boolean, ByVal Child_ID As Integer) As Boolean
        Return True
    End Function

    'Simulationsergebnis verarbeiten
    '-------------------------------
    Protected Overrides Sub SIM_Ergebnis_Lesen()

        Dim datei, element As String
        Dim ASCtmp As Wave.ASC
        Dim elemente As New Collection()

        'Einzulesende Dateien zusammenstellen
        For Each feature As Common.ObjectiveFunction In Me.mProblem.List_ObjectiveFunctions
            element = feature.SimGr.Substring(0, 4)
            If (Not elemente.Contains(element)) Then
                elemente.Add(element, element)
            End If
        Next

        'Altes SimErgebnis löschen
        Me.SimErgebnis.Clear()

        'Dateien einlesen
        For Each elem As String In elemente
            datei = elem & "_WEL.ASC"
            ASCtmp = New Wave.ASC(Me.WorkDir_Current & datei, True)
            'Simulationsergebnis abspeichern
            For Each zre As Wave.Zeitreihe In ASCtmp.Zeitreihen
                Me.SimErgebnis.Add(zre, elem & "_" & zre.ToString())
            Next
            ASCtmp = Nothing
        Next

        elemente = Nothing

    End Sub

#Region "Kombinatorik"

    'Kombinatorik
    '############

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
