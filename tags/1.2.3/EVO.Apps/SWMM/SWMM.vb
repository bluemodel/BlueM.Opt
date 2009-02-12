﻿Imports System.IO
Imports IHWB.SWMM.DllAdapter
Imports System.Threading

'*******************************************************************************
'*******************************************************************************
'**** Klasse SWMM                                                           ****
'****                                                                       ****
'**** Funktionen zur Kontrolle von EPA SWMM5                                ****
'****                                                                       ****
'**** Autoren: Dirk Muschalla                                               ****
'****                                                                       ****
'**** modelEAU                                                              ****
'**** Université Laval                                                      ****
'*******************************************************************************
'*******************************************************************************

Public Class SWMM
    Inherits Sim

#Region "Eigenschaften"

    'Eigenschaften
    '#############

    'SWMM DLL
    '---------
    Private swmm_dll() As SWMM_EngineDotNetAccess
    'Multithreading
    '--------------
    Dim MySWMMThreads() As SWMMThread
    Dim MyThreads() As Thread

#End Region 'Eigenschaften

#Region "Properties"

    ''' <summary>
    ''' Alle Dateiendungen (ohne Punkt), die in einem Datensatz vorkommen können
    ''' </summary>
    ''' <remarks>Die erste Dateiendung in dieser Collection repräsentiert den Datensatz (wird z.B. als Filter für OpenFile-Dialoge verwendet)</remarks>
    Public Overrides ReadOnly Property DatensatzDateiendungen() As Collections.Specialized.StringCollection
        Get
            Dim exts As Collections.Specialized.StringCollection = New Collections.Specialized.StringCollection()

            exts.AddRange(New String() {"INP", "DAT", "INI"})

            Return exts

        End Get
    End Property


#End Region 'Properties

#Region "Methoden"

    'Methoden
    '########

    'Konstruktor
    '***********
    Public Sub New(ByVal n_Proz As Integer)

        Call MyBase.New()

        'SWMM DLL instanzieren
        '----------------------
        Dim dll_path As String
        dll_path = System.Windows.Forms.Application.StartupPath() & "\SWMM\SWMM5.dll"

        'BlueM DLL instanzieren je nach Anzahl der Prozessoren
        '-----------------------------------------------------
        ReDim swmm_dll(n_Proz - 1)
        Dim i As Integer

        For i = 0 To n_Proz - 1
            If (File.Exists(dll_path)) Then
                swmm_dll(i) = New SWMM_EngineDotNetAccess(dll_path)
            Else
                Throw New Exception("SWMM.dll nicht gefunden!")
            End If
        Next

        'Anzahl der Threads
        ReDim MySWMMThreads(n_Proz - 1)
        For i = 0 To n_Proz - 1
            MySWMMThreads(i) = New SWMMThread(i, -1, "Folder", Datensatz, swmm_dll(i))
            MySWMMThreads(i).set_is_OK()
        Next
        ReDim MyThreads(n_Proz - 1)
    End Sub

    'Simulationsparameter einlesen
    '*****************************
    Protected Overrides Sub Read_SimParameter()

        Dim SimStart_Date_str As String = ""
        Dim SimEnde_Date_str As String = ""
        Dim SimStart_Time_str As String = ""
        Dim SimEnde_Time_str As String = ""
        Dim ROUTING_STEP_str As String = ""

        'INP-Datei öffnen
        '----------------
        Dim Datei As String = Me.WorkDir_Original & Me.Datensatz & ".inp"

        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
        Dim StrReadSync As TextReader = TextReader.Synchronized(StrRead)

        'Alle Zeilen durchlaufen
        Dim Zeile As String
        Dim readok As Integer = 0
        Do
            Zeile = StrRead.ReadLine.ToString()
            'Simulationszeitraum auslesen

            If Zeile.StartsWith("START_DATE") Then
                SimStart_Date_str = Zeile.Substring(21, 10)
                readok += 1
            ElseIf Zeile.StartsWith("START_TIME") Then
                SimStart_Time_str = Zeile.Substring(21, 8)
                readok += 1
            ElseIf Zeile.StartsWith("END_DATE") Then
                SimEnde_Date_str = Zeile.Substring(21, 10)
                readok += 1
            ElseIf Zeile.StartsWith("END_TIME") Then
                SimEnde_Time_str = Zeile.Substring(21, 8)
                readok += 1
            ElseIf Zeile.StartsWith("ROUTING_STEP") Then
                ROUTING_STEP_str = Zeile.Substring(21, 7)
            End If
            If readok > 4 Then Exit Do 'Sobald alle Bestandteile des Datum
            'und des Zeitschrittes gefunden wurde,
            'kann die Schleife verlassen werden
        Loop Until StrRead.Peek() = -1

        'Schließen der INP-Datei
        StrReadSync.Close()
        StrRead.Close()
        FiStr.Close()

        'SimStart und SimEnde in echtes Datum konvertieren
        Me.SimStart = New DateTime(SimStart_Date_str.Substring(6, 4), SimStart_Date_str.Substring(0, 2), SimStart_Date_str.Substring(3, 2), SimStart_Time_str.Substring(0, 2), SimStart_Time_str.Substring(3, 2), SimStart_Time_str.Substring(6, 2))
        Me.SimEnde = New DateTime(SimEnde_Date_str.Substring(6, 4), SimEnde_Date_str.Substring(0, 2), SimEnde_Date_str.Substring(3, 2), SimEnde_Time_str.Substring(0, 2), SimEnde_Time_str.Substring(3, 2), SimEnde_Time_str.Substring(6, 2))

        'Zeitschrittweite ist variable
        Me.SimDT = New TimeSpan(ROUTING_STEP_str.Substring(0, 1), ROUTING_STEP_str.Substring(2, 2), ROUTING_STEP_str.Substring(5, 2))

    End Sub

    'BlauesModell ausführen (simulieren)
    'Startet einen neuen Thread und übergibt ihm die Child ID
    '********************************************************
    Public Overrides Function launchSim(ByVal Thread_ID As Integer, ByVal Child_ID As Integer) As Boolean

        launchSim = False
        Dim Folder As String

        Folder = getThreadWorkDir(Thread_ID)
        MySWMMThreads(Thread_ID) = New SWMMThread(Thread_ID, Child_ID, Folder, Datensatz, swmm_dll(Thread_ID))
        MyThreads(Thread_ID) = New Thread(AddressOf MySWMMThreads(Thread_ID).launchSim)
        MyThreads(Thread_ID).IsBackground = True
        MyThreads(Thread_ID).Start()
        launchSim = True

        Return launchSim

    End Function

    'SWMM ohne Thread ausführen (simulieren)
    '****************************
    Public Overrides Function launchSim() As Boolean

        Dim simOK As Boolean

        Try

            Call swmm_dll(0).Initialize(Me.WorkDir_Current & Me.Datensatz)
            Call swmm_dll(0).Start(1)

            Dim elapsedTime As Double = 0.0

            Do
                Call swmm_dll(0).PerformTimeStep(elapsedTime)
            Loop While (elapsedTime > 0.0)

            'Simulation abschliessen
            Call swmm_dll(0).Finish()

            ''überprüfen, ob Simulation erfolgreich
            ''-------------------------------------
            ''rpt-Datei öffnen
            'Dim FiStr As FileStream = New FileStream(Me.WorkDir_Current & Me.Datensatz & ".rpt", FileMode.Open, IO.FileAccess.ReadWrite)
            'Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
            'Dim StrReadSync As TextReader = TextReader.Synchronized(StrRead)

            ''Alle Zeilen durchlaufen
            'Dim Zeile As String
            'simOK = False
            'Do
            '    Zeile = StrRead.ReadLine.ToString()
            '    If (Zeile.StartsWith("  Runoff Quantity Continuity")) Then
            '        simOK = True
            '        Exit Do
            '    End If

            'Loop Until StrRead.Peek() = -1

            'StrReadSync.Close()
            'StrRead.Close()
            'FiStr.Close()

            'Simulation abschliessen
            Call swmm_dll(0).Finish()

            'Simulation erfolgreich
            simOK = True

        Catch ex As Exception

            'Simulationsfehler aufgetreten
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "SWMM")

            'Simulation nicht erfolgreich
            simOK = False

        Finally

            'Ressourcen deallokieren
            Call swmm_dll(0).Dispose()

        End Try

        Return simOK

    End Function

    'Gibt zurück ob ein beliebiger Thread beendet ist und ibt die ID diesen freien Threads zurück
    '********************************************************************************************
    Public Overrides Function launchFree(ByRef Thread_ID As Integer) As Boolean
        launchFree = False

        For Each Thr_C As SWMMThread In MySWMMThreads
            If Thr_C.Sim_Is_OK = True And Thr_C.get_Child_ID = -1 Then
                launchFree = True
                Thread_ID = Thr_C.get_Thread_ID
                Exit For
            End If
        Next

    End Function

    'Prüft ob des aktuelle Child mit der ID die oben übergeben wurde fertig ist
    'Gibt die Thread ID zurück um zum auswerten in das Arbeitsverzeichnis zu wechseln
    '********************************************************************************
    Public Overrides Function launchReady(ByRef Thread_ID As Integer, ByRef SimIsOK As Boolean, ByVal Child_ID As Integer) As Boolean
        launchReady = False

        For Each Thr_C As SWMMThread In MySWMMThreads
            If Thr_C.launch_Ready = True And Thr_C.get_Child_ID = Child_ID Then
                launchReady = True
                SimIsOK = Thr_C.Sim_Is_OK
                Thread_ID = Thr_C.get_Thread_ID
                MyThreads(Thread_ID).Join()
                MySWMMThreads(Thread_ID) = New SWMMThread(Thread_ID, -1, "Folder", Datensatz, swmm_dll(Thread_ID))
                MySWMMThreads(Thread_ID).set_is_OK()
            End If
        Next

    End Function

    'Simulationsergebnis verarbeiten
    '-------------------------------
    'Simulationsergebnis verarbeiten
    '-------------------------------
    Protected Overrides Sub SIM_Ergebnis_Lesen()

    End Sub

    'Berechnung des Qualitätswerts (Zielwert)
    '****************************************
    Public Overrides Function CalculateFeature(ByVal feature As Common.Featurefunction) As Double

        CalculateFeature = 0

        Dim IsOK As Boolean

        'Fallunterscheidung Ergebnisdatei
        '--------------------------------
        Select Case feature.Datei

            Case "RPT"
                'SWMM-Ergebnisse aus RPT-Datei auslesen
                CalculateFeature = CalculateFeature_RPT(feature)

            Case Else
                'es wurde eine nicht unterstützte Ergebnisdatei angegeben
                IsOK = False

        End Select

        If (IsOK = False) Then
            'TODO: Fehlerbehandlung
        End If

        'Zielrichtung berücksichtigen
        CalculateFeature *= feature.Richtung

    End Function

    Public Function CalculateFeature_RPT(ByVal feature As Common.Featurefunction) As Double

        Dim QWert As Double
        Dim FFreqEast As Double, FFreqGath As Double, FFreqWest As Double
        Dim AvgFEast As Double, AvgFGath As Double, AvgFWest As Double
        Dim DateiPfad As String
        Dim Zeile As String

        DateiPfad = WorkDir_Current & Datensatz & ".RPT"

        'RPT-Datei öffnen
        Dim FiStr As FileStream = New FileStream(DateiPfad, FileMode.Open, IO.FileAccess.Read)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        'Richtige Zeile ansteuern und Wert auslesen
        Do
            Zeile = StrRead.ReadLine.ToString
            If (Zeile.StartsWith("  Outfall Loading Summary")) Then
                Do
                    Zeile = StrRead.ReadLine.ToString
                    If (Zeile.StartsWith("  EastOut")) Then
                        FFreqEast = Trim(Zeile.Substring(24, 5))
                        AvgFEast = Trim(Zeile.Substring(33, 6))
                        Zeile = StrRead.ReadLine.ToString
                        FFreqGath = Trim(Zeile.Substring(24, 5))
                        AvgFGath = Trim(Zeile.Substring(33, 6))
                        Zeile = StrRead.ReadLine.ToString
                        FFreqWest = Trim(Zeile.Substring(24, 5))
                        AvgFWest = Trim(Zeile.Substring(33, 6))
                        Exit Do
                    End If
                Loop Until StrRead.Peek() = -1
                QWert = (FFreqEast * AvgFEast) + (FFreqGath * AvgFGath) + (FFreqWest * AvgFWest)
                Exit Do
            End If
        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()
        Return QWert

    End Function
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
