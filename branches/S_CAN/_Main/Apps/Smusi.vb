Imports System.IO
Imports ihwb.SMUSI.DllAdapter

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

        'SMUSI DLL instanzieren
        '----------------------
        Dim dll_path As String
        dll_path = System.Windows.Forms.Application.StartupPath() & "\SMUSI.dll"

        If (File.Exists(dll_path)) Then
            SMUSI_dll = New SMUSI_EngineDotNetAccess(dll_path)
        Else
            Throw New Exception("SMUSI.dll nicht gefunden!")
        End If

    End Sub

    'Simulationsparameter einlesen
    '*****************************
    Protected Overrides Sub Read_SimParameter()

        Dim SimStart_str As String = ""
        Dim SimEnde_str As String = ""

        'ALL-Datei �ffnen
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

        'Schlie�en der ALL-Datei
        StrReadSync.Close()
        StrRead.Close()
        FiStr.Close()

        'SimStart und SimEnde in echtes Datum konvertieren
        Me.SimStart = New DateTime(SimStart_str.Substring(6, 4), SimStart_str.Substring(3, 2), SimStart_str.Substring(0, 2), SimStart_str.Substring(11, 2), SimStart_str.Substring(14, 2), 0)
        Me.SimEnde = New DateTime(SimEnde_str.Substring(6, 4), SimEnde_str.Substring(3, 2), SimEnde_str.Substring(0, 2), SimEnde_str.Substring(11, 2), SimEnde_str.Substring(14, 2), 0)

        'Zeitschrittweite ist immer 5 Minuten
        Me.SimDT = New TimeSpan(0, 5, 0)

    End Sub

    'SMUSI ausf�hren (simulieren)
    '***********************************
    Public Overrides Function launchSim() As Boolean

        Dim simOK As Boolean
        Dim SimCurrent, SimStart, SimEnde As DateTime

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
                '    'Nach Warnungen �berpr�fen
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

        'Simulationsergebnis verarbeiten
        '-------------------------------
        If (simOK) Then

            Dim datei, element As String
            Dim ASCtmp As Wave.ASC
            Dim elemente As New Collection()

            'Einzulesende Dateien zusammenstellen
            For Each optziel As Struct_OptZiel In Me.List_OptZiele
                element = optziel.SimGr.Substring(0, 4)
                If (Not elemente.Contains(element)) Then
                    elemente.Add(element, element)
                End If
            Next

            'Altes SimErgebnis l�schen
            Me.SimErgebnis.Clear()

            'Dateien einlesen
            For Each elem As String In elemente
                datei = elem & "_WEL.ASC"
                ASCtmp = New Wave.ASC(Me.WorkDir & datei, True)
                'Simulationsergebnis abspeichern
                For Each zre As Wave.Zeitreihe In ASCtmp.Zeitreihen
                    Me.SimErgebnis.Add(zre, elem & "_" & zre.ToString())
                Next
            Next

        End If

        Return simOK

    End Function

    'Berechnung des Qualit�tswerts (Zielwert)
    '****************************************
    Public Overrides Function QWert(ByVal OptZiel As Struct_OptZiel) As Double

        QWert = 0

        'Fallunterscheidung Ergebnisdatei
        '--------------------------------
        Select Case OptZiel.Datei

            Case "ASC"
                'QWert aus ASC-Datei
                QWert = QWert_ASC(OptZiel)

            Case Else
                Throw New Exception("Der Wert '" & OptZiel.Datei & "' f�r die Datei wird bei Optimierungszielen f�r SMUSI nicht akzeptiert!")

        End Select

    End Function

    'Qualit�tswert aus ASC-Datei
    '***************************
    Private Function QWert_ASC(ByVal OptZiel As Struct_OptZiel) As Double

        Dim QWert As Double
        Dim SimReihe As Wave.Zeitreihe

        'Simulationsergebnis auslesen
        SimReihe = Me.SimErgebnis(OptZiel.SimGr)

        'Fallunterscheidung Zieltyp
        '--------------------------
        Select Case OptZiel.ZielTyp

            Case "Wert"
                QWert = MyBase.QWert_Wert(OptZiel, SimReihe)

            Case "Reihe"
                QWert = MyBase.QWert_Reihe(OptZiel, SimReihe)

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