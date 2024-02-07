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
Imports IHWB.SMUSI.DllAdapter

''' <summary>
''' Klasse SMUSI
''' Funktionen zur Kontrolle von SMUSI
''' </summary>
Public Class Smusi
    Inherits Sim

#Region "Eigenschaften"

    'Eigenschaften
    '#############

    Private Const useSmusiExe As Boolean = False 'true = EXE, false = DLL

    'SMUSI DLL
    '---------
    Private dll_path As String
    Private smusi_dll As SMUSI_EngineDotNetAccess

#End Region 'Eigenschaften

#Region "Properties"

    ''' <summary>
    ''' Alle Dateiendungen (ohne Punkt), die in einem Datensatz vorkommen können
    ''' </summary>
    ''' <remarks>Die erste Dateiendung in dieser Collection repräsentiert den Datensatz (wird z.B. als Filter für OpenFile-Dialoge verwendet)</remarks>
    Public Overrides ReadOnly Property DatensatzDateiendungen() As Collections.Specialized.StringCollection
        Get
            Dim exts As New Collections.Specialized.StringCollection()

            exts.AddRange(New String() {"ALL", "AUS", "BEK", "BKL", "BOF", "BWN", "DRO",
                                        "EIN", "FKA", "JGG", "KLA", "RKL", "RUE", "SAM",
                                        "SMZ", "SOP", "SYS", "TGG", "VER", "WIN", "WMB",
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

        If (Not useSmusiExe) Then

            'SMUSI DLL instanzieren
            '----------------------
            Me.dll_path = IO.Path.Combine(System.Windows.Forms.Application.StartupPath(), "SMUSI\smusi.dll")

            If (File.Exists(Me.dll_path)) Then
                Me.smusi_dll = New SMUSI_EngineDotNetAccess(Me.dll_path)
            Else
                Throw New Exception("SMUSI.dll nicht gefunden!")
            End If
        End If

    End Sub

    'Simulationsparameter einlesen
    '*****************************
    Protected Overrides Sub Read_SimParameter()

        Dim SimStart_str As String = ""
        Dim SimEnde_str As String = ""

        'ALL-Datei öffnen
        '----------------
        Dim Datei As String = IO.Path.Combine(Me.WorkDir_Original, Me.Datensatz & ".ALL")

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
    Protected Overrides Function launchSim() As Boolean

        Dim simOK As Boolean
        Dim SimCurrent, SimStart, SimEnde As DateTime

        If (useSmusiExe) Then

            'verwende SMUSI.Win.exe
            '----------------------
            Dim exe_path As String
            Dim String1 As String
            Dim String3 As String
            Dim String4 As String

            exe_path = IO.Path.Combine(System.Windows.Forms.Application.StartupPath(), "SMUSI.WIN.exe")
            String1 = exe_path
            String3 = IO.Path.Combine(Me.WorkDir_Current, Me.Datensatz & ".all")
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


            If File.Exists(IO.Path.Combine(Me.WorkDir_Current, Me.Datensatz & ".sum")) Then
                simOK = True
            Else
                simOK = False
            End If

        Else

            'verwende SMUSI.dll und SMUSI.DllAdapter.dll
            '-------------------------------------------

            'SMUSI DLL neu instanzieren (muss das sein?)
            '-------------------------------------------
            Me.smusi_dll = Nothing

            Me.smusi_dll = New SMUSI_EngineDotNetAccess(dll_path)

            'Falls vorher schon initialisiert wurde
            Call Me.smusi_dll.Finish()
            Call Me.smusi_dll.Dispose()

            Try

                Call smusi_dll.Initialize(IO.Path.Combine(Me.WorkDir_Current, Me.Datensatz))

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

                Call Me.smusi_dll.Finish()
                Call Me.smusi_dll.Dispose()

            End Try

            Me.smusi_dll = Nothing

        End If

        Return simOK

    End Function

    'TODO: SMUSI Thread-Funktionen
    '#############################
    Protected Overrides Function launchSim(ByVal Thread_ID As Integer, ByVal Child_ID As Integer) As Boolean
        Return Me.launchSim()
    End Function

    Protected Overrides Function ThreadFree(ByRef Thread_ID As Integer) As Boolean
        Return True
    End Function

    Protected Overrides Function ThreadReady(ByRef Thread_ID As Integer, ByRef SimIsOK As Boolean, ByVal Child_ID As Integer) As Boolean
        Return True
    End Function

    'Simulationsergebnis verarbeiten
    '-------------------------------
    Protected Overrides Sub SIM_Ergebnis_Lesen()

        Dim datei As String, DateiPfad As String, element As String, Zeile As String
        Dim ASCtmp As Wave.Fileformats.ASC
        Dim SpalteVon As Long, SpalteLen As Long, BezVon As Integer
        Dim blnValueAdded As Boolean

        'Altes SimErgebnis löschen
        Me.SimResult.Clear()

        'Neu Steffen
        For Each obj As Common.ObjectiveFunction In Me.mProblem.List_ObjectiveFunctions
            'Unterscheidung nach ObjectiveType
            Select Case obj.GetObjType
                Case Common.ObjectiveFunction.ObjectiveType.Series
                    element = obj.SimResultName.Substring(0, 4)
                    datei = element & "_WEL.ASC"
                    ASCtmp = New Wave.Fileformats.ASC(IO.Path.Combine(Me.WorkDir_Current, datei), True)
                    'Simulationsergebnis abspeichern
                    For Each zre As Wave.TimeSeries In ASCtmp.TimeSeries.Values
                        Me.SimResult.Series.Add(element & "_" & zre.Title, zre)
                    Next
                    ASCtmp = Nothing
                    'Next
                Case Common.ObjectiveFunction.ObjectiveType.Value
                    'TODO: Umbauen, so dass Datei nicht jedes mal geoeffnet werden muss
                    '.RPT-Datei oeffnen
                    DateiPfad = IO.Path.Combine(WorkDir_Current, Datensatz & "." & obj.FileExtension)
                    Dim FiStr As FileStream = New FileStream(DateiPfad, FileMode.Open, IO.FileAccess.Read)
                    Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
                    Dim KeyWord_Block As String
                    Dim tmpValue As Double
                    'Datei durchgehen und mit Block und Spaltenangabe aus obj den gesuchten Wert ermitteln
                    'und diesen dann in Sim_Ergebnis schreiben
                    Dim objValue As Common.Objectivefunction_Value
                    objValue = obj
                    Select Case objValue.Block
                        Case "EntlVolumen"
                            KeyWord_Block = "* Zulauf"
                            Select Case objValue.Column
                                Case "SumVol"
                                    SpalteVon = 116
                                    SpalteLen = 10
                                    BezVon = 3
                                Case Else
                                    Throw New Exception("Das Schluesselwort für die Spalte ist ungueltig!")
                            End Select
                        Case "MaxAbfluss"
                            KeyWord_Block = "* Maximal"
                            Select Case objValue.Column
                                Case "Qmax"
                                    SpalteVon = 22
                                    SpalteLen = 7
                                    BezVon = 3
                                Case Else
                                    Throw New Exception("Das Schluesselwort für die Spalte ist ungueltig!")
                            End Select
                        Case "EntlFracht"
                            KeyWord_Block = "* Schmutzfracht"
                            Select Case objValue.Column
                                Case "CSBspez"
                                    SpalteVon = 108
                                    SpalteLen = 5
                                    BezVon = 27
                                Case Else
                                    Throw New Exception("Das Schluesselwort für die Spalte ist ungueltig!")
                            End Select
                        Case Else
                            Throw New Exception("Das Schluesselword für den Block ist ungueltig")
                    End Select
                    'Datei durchgehen und nach Schluesselwort suchen
                    blnValueAdded = False
                    Do
                        Zeile = Trim(StrRead.ReadLine.ToString)
                        Debug.Print(Zeile)
                        If (Zeile.StartsWith(KeyWord_Block)) Then
                            Do
                                Zeile = StrRead.ReadLine.ToString
                                If (Trim(Zeile.Substring(BezVon, 4)) = obj.SimResultName) Then
                                    tmpValue = Convert.ToDouble(Zeile.Substring(SpalteVon, SpalteLen))
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
        'Ende neu Steffen

        'Dateien einlesen


        'elemente = Nothing

    End Sub

#End Region 'Methoden

End Class
