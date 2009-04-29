Imports System.IO
Imports IHWB.BlueM.DllAdapter
Imports System.Threading

'*******************************************************************************
'*******************************************************************************
'**** Klasse BlueM                                                          ****
'****                                                                       ****
'**** Funktionen zur Kontrolle von BlueM                                    ****
'****                                                                       ****
'**** Autoren: Christoph Huebner, Felix Froehlich                           ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'*******************************************************************************
'*******************************************************************************

Public Class BlueM
    Inherits Sim

#Region "Eigenschaften"

    'Eigenschaften
    '#############

    'BlueM DLL
    '---------
    Private dll_path As String
    Private bluem_dll() As BlueM_EngineDotNetAccess

    'Misc
    '----
    Private useKWL As Boolean       'gibt an, ob die KWL-Datei benutzt wird

    'SKos
    '----
    Public SKos1 As SKos

    '**** Multithreading ****
    Dim MyBlueMThreads() As BlueMThread
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

            exts.AddRange(New String() {"ALL", "SYS", "FKT", "KTR", "EXT", "JGG", "WGG", _
                                        "TGG", "TAL", "HYA", "TRS", "EZG", "EIN", "URB", _
                                        "VER", "RUE", "BEK", "BOA", "BOD", "LNZ", "EFL", _
                                        "DIF", "ZRE", "BIN", "FKA"})

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

        'Daten belegen
        '-------------
        Me.useKWL = False

        'Pfad zu BlueM.DLL bestimmen
        '---------------------------
        dll_path = System.Windows.Forms.Application.StartupPath() & "\BlueM\BlueM.dll"

        If (Not File.Exists(dll_path)) Then
            Throw New Exception("BlueM.dll nicht gefunden!")
        End If

    End Sub

    ''' <summary>
    ''' BlueM auf Multithreading vorbereiten
    ''' </summary>
    ''' <param name="input_n_Threads">Anzahl Threads</param>
    Public Overrides Sub prepareThreads(ByVal input_n_Threads As Integer)

        Me.n_Threads = input_n_Threads

        'BlueM DLL instanzieren je nach Anzahl der Threads
        '-------------------------------------------------
        ReDim bluem_dll(Me.n_Threads - 1)
        Dim i As Integer

        For i = 0 To Me.n_Threads - 1
            bluem_dll(i) = New BlueM_EngineDotNetAccess(dll_path)
        Next

        'Thread-Objekte instanzieren
        ReDim MyBlueMThreads(Me.n_Threads - 1)
        For i = 0 To Me.n_Threads - 1
            MyBlueMThreads(i) = New BlueMThread(i, -1, "Folder", Datensatz, bluem_dll(i))
            MyBlueMThreads(i).set_is_OK()
        Next
        ReDim MyThreads(Me.n_Threads - 1)

    End Sub

    Public Overrides Sub setProblem(ByRef prob As EVO.Common.Problem)

        Call MyBase.setProblem(prob)

        'SKos instanzieren
        Me.SKos1 = New SKos(prob)

        'BlueM-spezifische Weiterverarbeitung von ZielReihen:
        '====================================================
        Dim objective As Common.ObjectiveFunction

        'KWL: Feststellen, ob irgendeine Zielfunktion die KWL-Datei benutzt
        '------------------------------------------------------------------
        For Each objective In Me.mProblem.List_ObjectiveFunctions
            If (objective.Datei = "KWL") Then
                Me.useKWL = True
                Exit For
            End If
        Next

    End Sub

#Region "Eingabedateien lesen"

    'Simulationsparameter einlesen
    '*****************************
    Protected Overrides Sub Read_SimParameter()

        Dim SimStart_str As String = ""
        Dim SimEnde_str As String = ""
        Dim SimDT_str As String = ""
        Dim Ganglinie As String = ""
        Dim CSV_Format As String = ""

        'ALL-Datei öffnen
        '----------------
        Dim Datei As String = Me.WorkDir_Original & Me.Datensatz & ".ALL"

        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.Read)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        'Alle Zeilen durchlaufen
        Dim Zeile As String
        Do
            Zeile = StrRead.ReadLine.ToString()

            'Simulationszeitraum auslesen
            If (Zeile.StartsWith(" SimBeginn - SimEnde ............:")) Then
                SimStart_str = Zeile.Substring(35, 16)
                SimEnde_str = Zeile.Substring(54, 16)
            End If

            'Zeitschrittweite auslesen
            If (Zeile.StartsWith(" Zeitschrittlaenge [min] ........:")) Then
                SimDT_str = Zeile.Substring(35).Trim
            End If

            'Überprüfen ob die Ganglinien (.WEL Datei) ausgegeben wird
            If (Zeile.StartsWith(" Ganglinienausgabe ....... [J/N] :")) Then
                Ganglinie = Zeile.Substring(35).Trim
            End If

            'Überprüfen ob CSV Format eingeschaltet ist
            If (Zeile.StartsWith(" ... CSV-Format .......... [J/N] :")) Then
                CSV_Format = Zeile.Substring(35).Trim
            End If

        Loop Until StrRead.Peek() = -1

        'SimStart und SimEnde in echtes Datum konvertieren
        Me.SimStart = New DateTime(SimStart_str.Substring(6, 4), SimStart_str.Substring(3, 2), SimStart_str.Substring(0, 2), SimStart_str.Substring(11, 2), SimStart_str.Substring(14, 2), 0)
        Me.SimEnde = New DateTime(SimEnde_str.Substring(6, 4), SimEnde_str.Substring(3, 2), SimEnde_str.Substring(0, 2), SimEnde_str.Substring(11, 2), SimEnde_str.Substring(14, 2), 0)

        'Zeitschrittweite in echte Dauer konvertieren
        Me.SimDT = New TimeSpan(0, Convert.ToInt16(SimDT_str), 0)

        'Fehlermeldung Ganglinie nicht eingeschaltet
        If Ganglinie <> "J" Then
            Throw New Exception("Die Ganglinienausgabe (.WEL Datei) ist nicht eingeschaltet. Bitte in .ALL Datei unter 'Ganglinienausgabe' einschalten")
        End If

        'Fehlermeldung CSv Format nicht eingeschaltet
        If CSV_Format <> "J" Then
            Throw New Exception("Das CSV Format für die .WEL Datei ist nicht eingeschaltet. Bitte in .ALL unter '... CSV-Format' einschalten.")
        End If


    End Sub

    'Liest die Verzweigungen aus BlueM in ein Array ein
    'Und Dimensioniert das Verzweigungsarray
    '*******************************************************
    Protected Overrides Sub Read_Verzweigungen()

        Dim i As Integer

        Dim FiStr As FileStream = New FileStream(WorkDir_Current & Datensatz & ".ver", FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        'Anzahl der Parameter feststellen
        Dim Zeile As String
        Dim Anz As Integer = 0

        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                Anz += 1
            End If
        Loop Until StrRead.Peek() = -1
        ReDim VerzweigungsDatei(Anz - 1, 3)

        'Zurück zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        'Einlesen der Zeile und übergeben an das Verzweidungsarray
        Dim ZeilenArray() As String

        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                ZeilenArray = Zeile.Split("|")
                'Verbraucher Array füllen
                VerzweigungsDatei(i, 0) = ZeilenArray(1).Trim
                VerzweigungsDatei(i, 1) = ZeilenArray(2).Trim
                VerzweigungsDatei(i, 2) = ZeilenArray(3).Trim
                VerzweigungsDatei(i, 3) = ZeilenArray(4).Trim
                i += 1
            End If

        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

        'Hier wird das Verzweigungsarray Dimensioniert
        ReDim Akt.VER_ONOFF(VerzweigungsDatei.GetUpperBound(0), 1)

    End Sub

#End Region 'Eingabedateien lesen

#Region "Evaluierung"

    'Gibt zurück ob ein beliebiger Thread beendet ist und ibt die ID diesen freien Threads zurück
    '********************************************************************************************
    Public Overrides Function ThreadFree(ByRef Thread_ID As Integer) As Boolean
        ThreadFree = False

        For Each Thr_C As BlueMThread In MyBlueMThreads
            If Thr_C.Sim_Is_OK = True And Thr_C.get_Child_ID = -1 Then
                ThreadFree = True
                Thread_ID = Thr_C.get_Thread_ID
                Exit For
            End If
        Next

    End Function

    'BlauesModell ausführen (simulieren)
    'Startet einen neuen Thread und übergibt ihm die Child ID
    '********************************************************
    Public Overrides Function launchSim(ByVal Thread_ID As Integer, ByVal Child_ID As Integer) As Boolean

        launchSim = False
        Dim Folder As String

        Folder = getThreadWorkDir(Thread_ID)
        MyBlueMThreads(Thread_ID) = New BlueMThread(Thread_ID, Child_ID, Folder, Datensatz, bluem_dll(Thread_ID))
        MyThreads(Thread_ID) = New Thread(AddressOf MyBlueMThreads(Thread_ID).launchSim)
        MyThreads(Thread_ID).IsBackground = True
        MyThreads(Thread_ID).Start()
        launchSim = True

        Return launchSim

    End Function

    'BlueM ohne Thread ausführen
    '***************************
    Public Overrides Function launchSim() As Boolean

        Dim simOK As Boolean

        Try

            'Datensatz übergeben und initialisieren
            Call bluem_dll(0).Initialize(Me.WorkDir_Current & Me.Datensatz)

            Dim SimEnde As DateTime = BlueM_EngineDotNetAccess.BlueMDate2DateTime(bluem_dll(0).GetSimulationEndDate())

            'Simulationszeitraum 
            Do While (BlueM_EngineDotNetAccess.BlueMDate2DateTime(bluem_dll(0).GetCurrentTime) <= SimEnde)
                Call bluem_dll(0).PerformTimeStep()
            Loop

            'Simulation abschliessen
            Call bluem_dll(0).Finish()

            'Simulation erfolgreich
            simOK = True

        Catch ex As Exception

            'Simulationsfehler aufgetreten
            EVO.Diagramm.Monitor.getInstance().LogAppend(ex.Message)

            'Simulation abschliessen
            Call bluem_dll(0).Finish()

            'Simulation nicht erfolgreich
            simOK = False

        Finally

            'Ressourcen deallokieren
            Call bluem_dll(0).Dispose()

        End Try

        Return simOK

    End Function

    'Prüft ob des aktuelle Child mit der ID die oben übergeben wurde fertig ist
    'Gibt die Thread ID zurück um zum auswerten in das Arbeitsverzeichnis zu wechseln
    '********************************************************************************
    Public Overrides Function ThreadReady(ByRef Thread_ID As Integer, ByRef SimIsOK As Boolean, ByVal Child_ID As Integer) As Boolean
        ThreadReady = False

        For Each Thr_C As BlueMThread In MyBlueMThreads
            If Thr_C.launch_Ready = True And Thr_C.get_Child_ID = Child_ID Then
                ThreadReady = True
                SimIsOK = Thr_C.Sim_Is_OK
                Thread_ID = Thr_C.get_Thread_ID
                MyThreads(Thread_ID).Join()
                MyBlueMThreads(Thread_ID) = New BlueMThread(Thread_ID, -1, "Folder", Datensatz, bluem_dll(Thread_ID))
                MyBlueMThreads(Thread_ID).set_is_OK()
            End If
        Next

    End Function

    'Simulationsergebnis verarbeiten
    '-------------------------------
    Protected Overrides Sub SIM_Ergebnis_Lesen()

        'TODO: hier nur die Reihen einlesen, die auch für objfunctions gebraucht werden

        'Altes Simulationsergebnis löschen
        Me.SimErgebnis.Clear()

        'WEL-Datei einlesen
        '------------------
        Dim WELtmp As Wave.WEL = New Wave.WEL(Me.WorkDir_Current & Me.Datensatz & ".WEL", True)

        'Reihen zu Simulationsergebnis hinzufügen
        For Each zre As Wave.Zeitreihe In WELtmp.Zeitreihen
            Me.SimErgebnis.Reihen.Add(zre.Title, zre)
        Next

        'ggf. KWL-Datei einlesen
        '-----------------------
        If (Me.useKWL) Then

            Dim KWLpath As String = Me.WorkDir_Current & Me.Datensatz & ".KWL"

            Dim KWLtmp As Wave.WEL = New Wave.WEL(KWLpath, True)

            'Reihen zu Simulationsergebnis hinzufügen
            For Each zre As Wave.Zeitreihe In KWLtmp.Zeitreihen
                Me.SimErgebnis.Reihen.Add(zre.Title, zre)
            Next

        End If

    End Sub

#End Region 'Evaluierung

#Region "Qualitätswertberechnung"

    'Qualitätswert aus PRB-Datei
    'BUG 220: PRB geht nicht
    '***********************
    Private Function CalculateObjective_PRB(ByVal objective As Common.ObjectiveFunction) As Double

        'Dim i As Integer
        'Dim IsOK As Boolean
        'Dim QWert As Double
        'Dim SimReihe As Object(,) = {}

        ''Simulationsergebnis auslesen
        'IsOK = Read_PRB(WorkDir & Datensatz & ".PRB", ziel.SimGr, SimReihe)

        ''Diff
        ''----
        ''Überflüssige Stützstellen (P) entfernen
        ''---------------------------------------
        ''Anzahl Stützstellen bestimmen
        'Dim stuetz As Integer = 0
        'Dim P_vorher As Double = -99
        'For i = 0 To SimReihe.GetUpperBound(0)
        '    If (i = 0 Or Not SimReihe(i, 1) = P_vorher) Then
        '        stuetz += 1
        '        P_vorher = SimReihe(i, 1)
        '    End If
        'Next
        ''Werte in neues Array schreiben
        'Dim PRBtmp(stuetz, 1) As Object
        'stuetz = 0
        'For i = 0 To SimReihe.GetUpperBound(0)
        '    If (i = 0 Or Not SimReihe(i, 1) = P_vorher) Then
        '        PRBtmp(stuetz, 0) = SimReihe(i, 0)
        '        PRBtmp(stuetz, 1) = SimReihe(i, 1)
        '        P_vorher = SimReihe(i, 1)
        '        stuetz += 1
        '    End If
        'Next
        ''Reihe um eine Stützstelle erweitern
        ''PRBtmp(stuetz, 0) = PRBtmp(stuetz - 1, 0)
        ''PRBtmp(stuetz, 1) = PRBtmp(stuetz - 1, 1)

        ''An Stützstellen der ZielReihe interpolieren
        ''-------------------------------------------
        'Dim PRBintp(ziel.ZielReihe.GetUpperBound(0), 1) As Object
        'Dim j As Integer
        'For i = 0 To ziel.ZielReihe.GetUpperBound(0)
        '    'zugehörige Lamelle in SimReihe finden
        '    j = 0
        '    Do While (PRBtmp(j, 1) < ziel.ZielReihe(i, 1))
        '        j += 1
        '    Loop
        '    'interpolieren
        '    PRBintp(i, 0) = (PRBtmp(j + 1, 0) - PRBtmp(j, 0)) / (PRBtmp(j + 1, 1) - PRBtmp(j, 1)) * (ziel.ZielReihe(i, 1) - PRBtmp(j, 1)) + PRBtmp(j, 0)
        '    PRBintp(i, 1) = ziel.ZielReihe(i, 1)
        'Next

        'For i = 0 To ziel.ZielReihe.GetUpperBound(0)
        '    QWert += Math.Abs(ziel.ZielReihe(i, 0) - PRBintp(i, 0))
        'Next

        'Return QWert

    End Function

    'Ein Ergebnis aus einer PRB-Datei einlesen
    '*****************************************
    Private Function Read_PRB(ByVal DateiPfad As String, ByVal ZielGr As String, ByRef PRB(,) As Object) As Boolean

    Dim ZeileStart As Integer = 0
    Dim AnzZeil As Integer = 26                   'Anzahl der Zeilen ist immer 26, definiert durch MAXSTZ in BM
    Dim j As Integer = 0
    Dim Zeile As String
        Read_PRB = True

    Dim FiStr As FileStream = New FileStream(DateiPfad, FileMode.Open, IO.FileAccess.ReadWrite)
    Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
    Dim StrReadSync As TextReader = TextReader.Synchronized(StrRead)

    'Array redimensionieren
        ReDim PRB(AnzZeil - 1, 1)

    'Anfangszeile suchen
        Do
            Zeile = StrRead.ReadLine.ToString
            If (Zeile.Contains("+ Wahrscheinlichkeitskeitsverteilung: " & ZielGr)) Then
                Exit Do
            End If
        Loop Until StrRead.Peek() = -1

    'Zeile mit Spaltenüberschriften überspringen
        Zeile = StrRead.ReadLine.ToString

        For j = 0 To AnzZeil - 1
            Zeile = StrRead.ReadLine.ToString()
            PRB(j, 0) = Convert.ToDouble(Zeile.Substring(2, 10))        'X-Wert
            PRB(j, 1) = Convert.ToDouble(Zeile.Substring(13, 8))        'P(Jahr)
        Next
        StrReadSync.Close()
        StrRead.Close()
        FiStr.Close()

    'Überflüssige Stützstellen (P) entfernen
    '---------------------------------------
    'Anzahl Stützstellen bestimmen
    Dim stuetz As Integer = 0
    Dim P_vorher As Double = -99
        For j = 0 To PRB.GetUpperBound(0)
            If (j = 0 Or Not PRB(j, 1) = P_vorher) Then
                stuetz += 1
                P_vorher = PRB(j, 1)
            End If
        Next
    'Werte in neues Array schreiben
    Dim PRBtmp(stuetz - 1, 1) As Object
        stuetz = 0
        For j = 0 To PRB.GetUpperBound(0)
            If (j = 0 Or Not PRB(j, 1) = P_vorher) Then
                PRBtmp(stuetz, 0) = PRB(j, 0)
                PRBtmp(stuetz, 1) = PRB(j, 1)
                P_vorher = PRB(j, 1)
                stuetz += 1
            End If
        Next
        PRB = PRBtmp

    End Function

#End Region 'Qualitätswertberechnung

#Region "Kombinatorik"

    'Kombinatorik
    '############

    'Mehrere Prüfungen ob die .VER Datei des BlueM und der .CES Datei auch zusammenpassen
    '************************************************************************************
    Public Sub Validate_CES_fits_to_VER()

        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim x As Integer = 0
        Dim y As Integer = 0

        Dim FoundA(VerzweigungsDatei.GetUpperBound(0)) As Boolean

        'Prüft ob jede Verzweigung einmal in der LocationList vorkommt
        For i = 0 To VerzweigungsDatei.GetUpperBound(0)
            For j = 0 To Me.mProblem.List_Locations.GetUpperBound(0)
                For x = 0 To Me.mProblem.List_Locations(j).List_Massnahmen.GetUpperBound(0)
                    For y = 0 To Me.mProblem.List_Locations(j).List_Massnahmen(x).Schaltung.GetUpperBound(0)
                        If VerzweigungsDatei(i, 0) = Me.mProblem.List_Locations(j).List_Massnahmen(x).Schaltung(y, 0) And VerzweigungsDatei(i, 1) = "2" Then
                            FoundA(i) = True
                        End If
                    Next
                Next
            Next
        Next

        'Prüft ob die nicht vorkommenden Verzweigungen Verzweigungen anderer Art sind
        For i = 0 To VerzweigungsDatei.GetUpperBound(0)
            If Not VerzweigungsDatei(i, 1) = "2" And FoundA(i) = False Then
                FoundA(i) = True
            End If
        Next

        Dim FoundB As Boolean = True
        Dim TmpBool As Boolean = False

        'Prüft ob alle in der LocationList Vorkommenden Verzweigungen auch in der Verzweigungsdatei sind
        For j = 0 To Me.mProblem.List_Locations.GetUpperBound(0)
            For x = 0 To Me.mProblem.List_Locations(j).List_Massnahmen.GetUpperBound(0)
                For y = 0 To Me.mProblem.List_Locations(j).List_Massnahmen(x).Schaltung.GetUpperBound(0)
                    If Not Me.mProblem.List_Locations(j).List_Massnahmen(x).Schaltung(y, 0) = "X" Then
                        TmpBool = False
                        For i = 0 To VerzweigungsDatei.GetUpperBound(0)
                            If VerzweigungsDatei(i, 0) = Me.mProblem.List_Locations(j).List_Massnahmen(x).Schaltung(y, 0) And VerzweigungsDatei(i, 1) = "2" Then
                                TmpBool = True
                            End If
                        Next
                        If Not TmpBool Then
                            FoundB = False
                        End If
                    End If

                Next
            Next
        Next

        'Übergabe
        If FoundB = False Then
            Throw New Exception(".VER und .CES Dateien passen nicht zusammen! Eine Verzweigung in der VER Datei kommt in der CES Datei nicht vor und ist nicht nicht vom Typ Prozentsatz (Kennung 2)")
        Else
            For i = 0 To FoundA.GetUpperBound(0)
                If FoundA(i) = False Then
                    Throw New Exception(".VER und .CES Dateien passen nicht zusammen! Eine in der CES Datei angegebene Verzeigung kommt in der VEr Datei nicht vor.")
                End If
            Next
        End If

    End Sub

    'Schreibt die neuen Verzweigungen
    '********************************
    Protected Overrides Sub Write_Verzweigungen()

        Dim AnzZeil As Integer
        Dim i, j As Integer
        Dim Zeilenarray() As String
        Dim Zeile As String
        Dim StrLeft As String
        Dim StrRight As String
        Dim DateiPfad As String
        Dim SplitZeile() As String

        DateiPfad = WorkDir_Current & Datensatz & ".ver"
        'Datei öffnen
        Dim FiStr As FileStream = New FileStream(DateiPfad, FileMode.Open, IO.FileAccess.Read)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        'Anzahl der Zeilen feststellen
        AnzZeil = 0
        Do
            Zeile = StrRead.ReadLine.ToString
            AnzZeil += 1
        Loop Until StrRead.Peek() = -1

        ReDim Zeilenarray(AnzZeil - 1)

        'Datei komplett einlesen
        FiStr.Seek(0, SeekOrigin.Begin)
        For j = 0 To AnzZeil - 1
            Zeilenarray(j) = StrRead.ReadLine.ToString
        Next

        StrRead.Close()
        FiStr.Close()

        'ZeilenArray wird zu neuer Datei zusammen gebaut
        For i = 0 To Akt.VER_ONOFF.GetUpperBound(0)
            If Not Akt.VER_ONOFF(i, 1) = Nothing Then
                For j = 0 To Zeilenarray.GetUpperBound(0)
                    If Not Zeilenarray(j).StartsWith("*") Then
                        SplitZeile = Zeilenarray(j).Split("|")
                        If Akt.VER_ONOFF(i, 0) = SplitZeile(1).Trim Then
                            StrLeft = Microsoft.VisualBasic.Left(Zeilenarray(j), 31)
                            StrRight = Microsoft.VisualBasic.Right(Zeilenarray(j), 41)
                            If Akt.VER_ONOFF(i, 1) = "1" Then
                                Zeilenarray(j) = StrLeft & "      100     " & StrRight
                            ElseIf (Akt.VER_ONOFF(i, 1) = "0") Then
                                Zeilenarray(j) = StrLeft & "        0     " & StrRight
                            End If
                        End If
                    End If
                Next
            End If
        Next

        'Alle Zeilen wieder in Datei schreiben
        Dim StrWrite As StreamWriter = New StreamWriter(DateiPfad, False, System.Text.Encoding.GetEncoding("iso8859-1"))
        For j = 0 To AnzZeil - 1
            StrWrite.WriteLine(Zeilenarray(j))
        Next

        StrWrite.Close()

    End Sub

#End Region 'Kombinatorik

#End Region 'Methoden

End Class