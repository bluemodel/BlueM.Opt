'BlueM.Opt
'Copyright (C) BlueM Dev Group
'Website: <https://www.bluemodel.org>
'
'This program is free software: you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.
'
'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
'GNU General Public License for more details.
'
'You should have received a copy of the GNU General Public License
'along with this program. If not, see <https://www.gnu.org/licenses/>.
'
Imports System.IO
Imports IHWB.SWMM.DLLAdapter
Imports System.Threading

''' <summary>
''' Klasse SWMM
''' Funktionen zur Kontrolle von EPA SWMM5
''' </summary>
Public Class SWMM
    Inherits Sim

#Region "Eigenschaften"

    'Eigenschaften
    '#############

    'SWMM DLL
    '---------
    Private dll_path As String
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
            Dim exts As New Collections.Specialized.StringCollection()

            exts.AddRange(New String() {"INP", "DAT", "INI"})

            Return exts

        End Get
    End Property

    ''' <summary>
    ''' Ob die Anwendung Multithreading unterstützt
    ''' </summary>
    ''' <returns>True</returns>
    Public Overrides ReadOnly Property MultithreadingSupported() As Boolean
        Get
            Return True
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

        'Pfad zu SWMM5.DLL bestimmen
        '---------------------------
        dll_path = IO.Path.Combine(System.Windows.Forms.Application.StartupPath(), "SWMM\SWMM5.dll")

        If (Not File.Exists(dll_path)) Then
            Throw New Exception("SWMM.dll nicht gefunden!")
        End If

    End Sub

    ''' <summary>
    ''' SWMM Simulationen vorbereiten
    ''' </summary>
    Public Overrides Sub prepareSimulation()

        Call MyBase.prepareSimulation()

        'SWMM DLL instanzieren je nach Anzahl der Threads
        '------------------------------------------------
        If swmm_dll Is Nothing Then
            ReDim swmm_dll(Me.n_Threads - 1)
            Dim i As Integer

            For i = 0 To Me.n_Threads - 1
                'toDo: prüfen, ob schon instanziert
                swmm_dll(i) = New SWMM_EngineDotNetAccess(dll_path)
            Next
        End If

        'Thread-Objekte instanzieren
        ReDim MySWMMThreads(Me.n_Threads - 1)
        For i = 0 To Me.n_Threads - 1
            MySWMMThreads(i) = New SWMMThread(i, -1, "Folder", Datensatz, swmm_dll(i))
            MySWMMThreads(i).set_is_OK()
        Next
        ReDim MyThreads(Me.n_Threads - 1)

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
        Dim Datei As String = IO.Path.Combine(Me.WorkDir_Original, Me.Datensatz & ".inp")

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

    'SWMM ausführen (simulieren)
    'Startet einen neuen Thread und übergibt ihm die Child ID
    '********************************************************
    Protected Overrides Function launchSim(ByVal Thread_ID As Integer, ByVal Child_ID As Integer) As Boolean

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
    Protected Overrides Function launchSim() As Boolean

        Dim simOK As Boolean

        Try

            Call swmm_dll(0).Initialize(IO.Path.Combine(Me.WorkDir_Current, Me.Datensatz))
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
            'Dim FiStr As FileStream = New FileStream(IO.Path.Combine(Me.WorkDir_Current, Me.Datensatz & ".rpt"), FileMode.Open, IO.FileAccess.ReadWrite)
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
    Protected Overrides Function ThreadFree(ByRef Thread_ID As Integer) As Boolean
        ThreadFree = False

        For Each Thr_C As SWMMThread In MySWMMThreads
            If Thr_C.Sim_Is_OK = True And Thr_C.get_Child_ID = -1 Then
                ThreadFree = True
                Thread_ID = Thr_C.get_Thread_ID
                Exit For
            End If
        Next

    End Function

    'Prüft ob des aktuelle Child mit der ID die oben übergeben wurde fertig ist
    'Gibt die Thread ID zurück um zum auswerten in das Arbeitsverzeichnis zu wechseln
    '********************************************************************************
    Protected Overrides Function ThreadReady(ByRef Thread_ID As Integer, ByRef SimIsOK As Boolean, ByVal Child_ID As Integer) As Boolean
        ThreadReady = False

        For Each Thr_C As SWMMThread In MySWMMThreads
            If Thr_C.launch_Ready = True And Thr_C.get_Child_ID = Child_ID Then
                ThreadReady = True
                SimIsOK = Thr_C.Sim_Is_OK
                Thread_ID = Thr_C.get_Thread_ID
                MyThreads(Thread_ID).Join()
                MySWMMThreads(Thread_ID) = New SWMMThread(Thread_ID, -1, "Folder", Datensatz, swmm_dll(Thread_ID))
                MySWMMThreads(Thread_ID).set_is_OK()
            End If
        Next

    End Function

    'Simulationsergebnis einlesen
    '----------------------------
    Protected Overrides Sub SIM_Ergebnis_Lesen()

        'TODO: Objectives durchgehen und erforderliche Werte in SimErgebnis speichern (#2)
        'bisher nur Einlesen der RPT-Datei moeglich
        'Einlesen des Binaerfiles mit den Ganglinien spaeter dazunehmen 
        Dim DateiPfad As String
        Dim Zeile As String

        'Altes Simulationsergebnis löschen
        Me.SimResult.Clear()

        'Ablauf
        'Durchgehen aller ObjectiveFunctions - Objekte
        'Einlesen in das Objekt SimErgebnis

        For Each obj As Common.ObjectiveFunction In Me.mProblem.List_ObjectiveFunctions

            'Unterscheidung nach ObjectiveType
            Select Case obj.GetObjType
                Case Common.ObjectiveFunction.ObjectiveType.Series
                  'TODO
                  'Zeitreihe einlesen
                Case Common.ObjectiveFunction.ObjectiveType.Value
                    'TODO: Umbauen, so dass Datei nicht jedes mal geoeffnet werden muss
                    '.RPT-Datei oeffnen
                    DateiPfad = IO.Path.Combine(WorkDir_Current, Datensatz & "." & obj.FileExtension)
                    Dim FiStr As FileStream = New FileStream(DateiPfad, FileMode.Open, IO.FileAccess.Read)
                    Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
                    Dim KeyWord_Block As String, KeyWord_SimGr As String
                    Dim NoSpalte As Short
                    Dim tmpValue As Double
                    Dim blnValueAdded As Boolean
                    'Datei durchgehen und mit Block und Spaltenangabe aus obj den gesuchten Wert ermitteln
                    'und diesen dann in Sim_Ergebnis schreiben
                    Dim objValue As Common.Objectivefunction_Value
                    objValue = obj
                    Select Case objValue.Block
                        Case "NodeFlooding"
                            KeyWord_Block = "  Node Flooding Summary"
                            Select Case objValue.Column
                                Case "HoursFlooded"
                                    NoSpalte = 2
                                Case "FloodVolume"
                                    NoSpalte = 6
                                Case Else
                                    Throw New Exception("Das Schluesselwort für die Spalte ist ungueltig!")
                            End Select
                        Case "StorageVolume"
                            KeyWord_Block = "  Storage Volume Summary"
                            Select Case objValue.Column
                                Case "AvgVolume"
                                    NoSpalte = 2
                                Case "AvgPctFull"
                                    NoSpalte = 3
                                Case "MaxVolume"
                                    NoSpalte = 4
                                Case "MaxPctFull"
                                    NoSpalte = 5
                                Case "MaxOutflow"
                                    NoSpalte = 8
                                Case Else
                                    Throw New Exception("Das Schluesselwort für die Spalte ist ungueltig!")
                            End Select
                        Case "OutfallLoad"
                            KeyWord_Block = "  Outfall Loading Summary"
                            Select Case objValue.Column
                                Case "MaxFlow"
                                    NoSpalte = 4
                                Case "FlowVolume"
                                    NoSpalte = 5
                                Case "Pollutant_01"
                                    NoSpalte = 6
                                Case "Pollutant_02"
                                    NoSpalte = 7
                                Case "Pollutant_03"
                                    NoSpalte = 8
                                Case "Pollutant_04"
                                    NoSpalte = 9
                                Case "Pollutant_05"
                                    NoSpalte = 10
                                Case "Pollutant_06"
                                    NoSpalte = 11
                                Case Else
                                    Throw New Exception("Das Schluesselwort für die Spalte ist ungueltig!")
                            End Select
                        Case "LinkFlow"
                            KeyWord_Block = "  Link Flow Summary"
                        Case "ConduitSurcharge"
                            KeyWord_Block = "  Conduit Surcharge Summary"
                        Case "Pumping"
                            KeyWord_Block = "  Pumping Summary"
                            Select Case objValue.Column
                                Case "OnlineTime"
                                    NoSpalte = 2
                                Case "TotalEnergy"
                                    NoSpalte = 6
                                    Throw New Exception("Das Schluesselwort für die Spalte ist ungueltig!")
                            End Select
                        Case Else
                            Throw New Exception("Das Schluesselword für den Block ist ungueltig")
                    End Select
                    'Datei durchgehen und nach Schluesselwort suchen
                    blnValueAdded = False
                    Do
                        KeyWord_SimGr = "  " & objValue.SimResultName
                        Zeile = StrRead.ReadLine.ToString
                        If (Zeile.StartsWith(KeyWord_Block)) Then
                            Zeile = StrRead.ReadLine.ToString    'Es folgt immer eine Zeile mit Sternen
                            Zeile = StrRead.ReadLine.ToString    'und dann noch eine Leerzeile
                            Do
                                Zeile = StrRead.ReadLine.ToString
                                If (Zeile.StartsWith(KeyWord_SimGr)) Then
                                    tmpValue = Convert.ToDouble(Zeile.Split(" ", options:=StringSplitOptions.RemoveEmptyEntries)(NoSpalte - 1), Common.Provider.FortranProvider)
                                    Me.SimResult.Values.Add(obj.Description, tmpValue)
                                    blnValueAdded = True
                                    Exit Do
                                    'Falls keine Nodes überstaut sind bei Node Flooding Summary muss geährleistet werden,
                                    'dass der Wert 0 übergeben wird. 
                                    'Select Case objValue.Block
                                    '   Case "NodeFlooding"
                                    '      If Zeile.TrimStart.StartsWith("No nodes were flooded.") Then
                                    '         tmpValue = 0.0
                                    '         Me.SimErgebnis.Werte.Add(obj.Bezeichnung, tmpValue)
                                    '         blnValueAdded = True
                                    '         Exit Do
                                    'Case Else
                                    'End Select
                                    'Wenn die SimGr nicht im Block auftaucht muss tmpvalue = 0 gesetzt werden
                                ElseIf Zeile.TrimStart.StartsWith("**********************") Then    'nächster Block beginnt
                                    tmpValue = 0.0
                                    Me.SimResult.Values.Add(obj.Description, tmpValue)
                                    blnValueAdded = True
                                    Exit Do
                                End If
                            Loop Until StrRead.Peek() = -1
                        End If
                        If blnValueAdded Then Exit Do
                    Loop Until StrRead.Peek() = -1
                    StrRead.Close()
                    FiStr.Close()


                Case Common.ObjectiveFunction.ObjectiveType.ValueFromSeries
                    'TODO
                Case Else
                    'TODO
            End Select
        Next


    End Sub

#End Region 'Methoden

End Class

