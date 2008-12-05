Imports System.IO


'*******************************************************************************
'*******************************************************************************
'**** Klasse SWMM5                                                          ****
'****                                                                       ****
'**** Funktionen zur Kontrolle von SWMM5                                    ****
'****                                                                       ****
'**** Steffen Heusch                                                        ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'****                                                                       ****
'**** Erstellt: November 2007                                               ****
'****                                                                       ****
'**** Letzte Änderung: November 2007                                        ****
'*******************************************************************************
'*******************************************************************************


Public Class SWMM
    Inherits Sim

#Region "Properties"

    ''' <summary>
    ''' Alle Dateiendungen (ohne Punkt), die in einem Datensatz vorkommen können
    ''' </summary>
    ''' <remarks>Der erste Wert des Arrays wird als Filter für OpenFile-Dialoge verwendet</remarks>
    Public Overrides ReadOnly Property DatensatzDateiendungen() As Collections.Specialized.StringCollection
        Get
            Dim exts As Collections.Specialized.StringCollection = New Collections.Specialized.StringCollection()

            exts.AddRange(New String() {"INP"})

            'TODO: Dateiendungen für SWMM-Datensatz

            Return exts

        End Get
    End Property

#End Region 'Properties


#Region "Methoden"

    'Konstruktor
    '***********
    Public Sub New()

        Call MyBase.New()

    End Sub

    
    Public Overrides Function launchSim() As Boolean

        'Aktuelles Verzeichnis bestimmen
        Dim currentDir As String = CurDir()
        Dim InpDatei As String, RptDatei As String, DatDatei As String
        'zum Arbeitsverzeichnis wechseln
        ChDrive(Me.WorkDir_Current)
        ChDir(Me.WorkDir_Current)
        'dll aufrufen
        InpDatei = Me.WorkDir_Current & Me.Datensatz & ".inp"
        RptDatei = Me.WorkDir_Current & Me.Datensatz & ".rpt"
        DatDatei = Me.WorkDir_Current & Me.Datensatz & ".dat"
        RunSwmmDll(InpDatei, RptDatei, DatDatei)
        'zurück ins Ausgangsverzeichnis wechseln
        ChDrive(currentDir)
        ChDir(currentDir)

        'überprüfen, ob Simulation erfolgreich
        '-------------------------------------
        'rpt-Datei öffnen
        Dim FiStr As FileStream = New FileStream(RptDatei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        'Alle Zeilen durchlaufen
        Dim Zeile As String
        launchSim = False
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("  Runoff Quantity Continuity")) Then
                launchSim = True
                Exit Do
            End If

        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

    End Function

    Public Overrides Function launchSim(ByVal Thread_ID As Integer, ByVal Child_ID As Integer) As Boolean

        Return Me.launchSim()

    End Function

    Public Overrides Function launchFree(ByRef Thread_ID As Integer) As Boolean

    End Function
    Public Overrides Function launchReady(ByRef Thread_ID As Integer, ByRef SimIsOK As Boolean, ByVal Child_ID As Integer) As Boolean

    End Function

    'Simulationsergebnis verarbeiten
    '-------------------------------
    Protected Overrides Sub SIM_Ergebnis_Lesen()

    End Sub

    Protected Overrides Sub Read_SimParameter()

        Dim SimStartDay_str As String = ""
        Dim SimStartHour_str As String = ""
        Dim SimEndeDay_str As String = ""
        Dim SimEndeHour_str As String = ""

        'INP-Datei öffnen
        '----------------
        Dim Datei As String = Me.WorkDir_Current & Me.Datensatz & ".INP"

        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
        'Verhindert parallelisierung, d.h. Einlesen wird erst abgeschlossen
        Dim StrReadSync As TextReader = TextReader.Synchronized(StrRead)

        'Alle Zeilen durchlaufen
        Dim Zeile As String
        Do
            Zeile = StrReadSync.ReadLine.ToString()

            'Simulationszeitraum auslesen
            If (Zeile.StartsWith("START_DATE")) Then
                SimStartDay_str = Zeile.Substring(21, 10)
            End If

            If (Zeile.StartsWith("START_TIME")) Then
                SimStartHour_str = Zeile.Substring(21, 5)
            End If

            If (Zeile.StartsWith("END_DATE")) Then
                SimEndeDay_str = Zeile.Substring(21, 10)
            End If

            If (Zeile.StartsWith("END_TIME")) Then
                SimEndeHour_str = Zeile.Substring(21, 5)
            End If

        Loop Until StrReadSync.Peek() = -1

        'SimStart und SimEnde in echtes Datum konvertieren
        Me.SimStart = New DateTime(SimStartDay_str.Substring(6, 4), SimStartDay_str.Substring(0, 2), SimStartDay_str.Substring(3, 2), SimStartHour_str.Substring(0, 2), SimStartHour_str.Substring(3, 2), 0)
        Me.SimEnde = New DateTime(SimEndeDay_str.Substring(6, 4), SimEndeDay_str.Substring(0, 2), SimEndeDay_str.Substring(3, 2), SimEndeHour_str.Substring(0, 2), SimEndeHour_str.Substring(3, 2), 0)

        'Zeitschrittweite ist immer 1 Minute
        Me.SimDT = New TimeSpan(0, 1, 0)

        StrReadSync.Close()
        StrRead.Close()
        FiStr.Close()

    End Sub



    Protected Overrides Sub Read_Verzweigungen()

    End Sub

    Protected Overrides Sub Write_Verzweigungen()

    End Sub

    'In der ersten Version wurden die Optimierungsparameter in einer eigenen Datei vorgehalten
    'Später wurde auf das bereits bestehende System mit drei Dateien umgestellt


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

#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

End Class
