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
    Private bluem_dll() As BlueM_EngineDotNetAccess

    'Misc
    '----
    Private useKWL As Boolean       'gibt an, ob die KWL-Datei benutzt wird

    'IHA
    '---
    Public isIHA As Boolean
    Public IHASys As IHWB.IHA.IHAAnalysis
    Public IHAProc As IHAProcessor

    'SKos
    '----
    Public SKos1 As SKos

    '**** Multithreading ****
    Dim MyBlueMThreads() As BlueMThread
    Dim MyThreads() As Thread

#End Region 'Eigenschaften

#Region "Properties"

    ''' <summary>
    ''' Alle Dateiendungen (ohne Punkt), die in einem Datensatz vorkommen k�nnen
    ''' </summary>
    ''' <remarks>Der erste Wert des Arrays wird als Filter f�r OpenFile-Dialoge verwendet</remarks>
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

#End Region 'Properties

#Region "Methoden"

    'Methoden
    '########

    'Konstruktor
    '***********
    Public Sub New(ByVal n_Proz As Integer)

        Call MyBase.New()

        'Daten belegen
        '-------------
        Me.useKWL = False
        Me.isIHA = False

        'BlueM_DLL
        Dim dll_path As String
        dll_path = System.Windows.Forms.Application.StartupPath() & "\BlueM\BlueM.dll"

        'BlueM DLL instanzieren je nach Anzahl der Prozessoren
        '-----------------------------------------------------
        ReDim bluem_dll(n_Proz - 1)
        Dim i As Integer

        For i = 0 To n_Proz - 1
            If (File.Exists(dll_path)) Then
                bluem_dll(i) = New BlueM_EngineDotNetAccess(dll_path)
            Else
                Throw New Exception("BlueM.dll nicht gefunden!")
            End If
        Next

        'Anzahl der Threads
        ReDim MyBlueMThreads(n_Proz - 1)
        For i = 0 To n_Proz - 1
            MyBlueMThreads(i) = New BlueMThread(i, -1, "Folder", Datensatz, bluem_dll(i))
            MyBlueMThreads(i).set_is_OK()
        Next
        ReDim MyThreads(n_Proz - 1)

    End Sub

    Public Overrides Sub setProblem(ByRef prob As EVO.Common.Problem)

        Call MyBase.setProblem(prob)

        'SKos instanzieren
        Me.SKos1 = New SKos(prob)

        'BlueM-spezifische Weiterverarbeitung von ZielReihen:
        '====================================================
        Dim feature As Common.Featurefunction

        'KWL: Feststellen, ob irgendeine Zielfunktion die KWL-Datei benutzt
        '------------------------------------------------------------------
        For Each feature In Me.mProblem.List_Featurefunctions
            If (feature.Datei = "KWL") Then
                Me.useKWL = True
                Exit For
            End If
        Next

        'IHA
        '---
        Dim IHAZielReihe As Wave.Zeitreihe
        Dim IHAStart, IHAEnde As DateTime

        IHAZielReihe = New Wave.Zeitreihe("new")

        'Gibt es eine IHA-Zielfunktion?
        'HACK: es wird immer nur das erste IHA-Ziel verwendet!
        '------------------------------
        For Each feature In Me.mProblem.List_Featurefunctions
            If (feature.Typ = "IHA") Then
                'IHA-Berechnung einschalten
                Me.isIHA = True
                IHAZielReihe = feature.RefReihe
                IHAStart = feature.EvalStart
                IHAEnde = feature.EvalEnde
                Exit For
            End If
        Next

        'IHA-Berechnung vorbereiten
        '--------------------------
        If (Me.isIHA) Then
            'IHAAnalyse-Objekt instanzieren
            Me.IHASys = New IHWB.IHA.IHAAnalysis(Me.WorkDir_Original & "IHA\", IHAZielReihe, IHAStart, IHAEnde)

            'IHAProcessor-Objekt instanzieren
            Me.IHAProc = New IHAProcessor()

            'IHA-Vergleichsmodus?
            '--------------------
            Dim reffile As String = Me.WorkDir_Original & Me.Datensatz & ".rva"
            If (File.Exists(reffile)) Then

                Dim RVABase As New Wave.RVA(reffile)

                'Vergleichsmodus aktivieren
                Call Me.IHAProc.setComparisonMode(RVABase.RVAValues)
            End If

        End If

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

        'ALL-Datei �ffnen
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

            '�berpr�fen ob die Ganglinien (.WEL Datei) ausgegeben wird
            If (Zeile.StartsWith(" Ganglinienausgabe ....... [J/N] :")) Then
                Ganglinie = Zeile.Substring(35).Trim
            End If

            '�berpr�fen ob CSV Format eingeschaltet ist
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
            Throw New Exception("Das CSV Format f�r die .WEL Datei ist nicht eingeschaltet. Bitte in .ALL unter '... CSV-Format' einschalten.")
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

        'Zur�ck zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        'Einlesen der Zeile und �bergeben an das Verzweidungsarray
        Dim ZeilenArray() As String

        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                ZeilenArray = Zeile.Split("|")
                'Verbraucher Array f�llen
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

    'Gibt zur�ck ob ein beliebiger Thread beendet ist und ibt die ID diesen freien Threads zur�ck
    '********************************************************************************************
    Public Overrides Function launchFree(ByRef Thread_ID As Integer) As Boolean
        launchFree = False

        For Each Thr_C As BlueMThread In MyBlueMThreads
            If Thr_C.Sim_Is_OK = True And Thr_C.get_Child_ID = -1 Then
                launchFree = True
                Thread_ID = Thr_C.get_Thread_ID
                Exit For
            End If
        Next

    End Function

    'BlauesModell ausf�hren (simulieren)
    'Startet einen neuen Thread und �bergibt ihm die Child ID
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

    'BlueM ohne Thread ausf�hren
    '***************************
    Public Overrides Function launchSim() As Boolean

        Dim simOK As Boolean

        Try

            'Datensatz �bergeben und initialisieren
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
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "BlueM")

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

    'Pr�ft ob des aktuelle Child mit der ID die oben �bergeben wurde fertig ist
    'Gibt die Thread ID zur�ck um zum auswerten in das Arbeitsverzeichnis zu wechseln
    '********************************************************************************
    Public Overrides Function launchReady(ByRef Thread_ID As Integer, ByRef SimIsOK As Boolean, ByVal Child_ID As Integer) As Boolean
        launchReady = False

        For Each Thr_C As BlueMThread In MyBlueMThreads
            If Thr_C.launch_Ready = True And Thr_C.get_Child_ID = Child_ID Then
                launchReady = True
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

        'Altes Simulationsergebnis l�schen
        Me.SimErgebnis.Clear()

        'WEL-Datei einlesen
        '------------------
        Dim WELtmp As Wave.WEL = New Wave.WEL(Me.WorkDir_Current & Me.Datensatz & ".WEL", True)

        'Reihen zu Simulationsergebnis hinzuf�gen
        For Each zre As Wave.Zeitreihe In WELtmp.Zeitreihen
            Me.SimErgebnis.Add(zre, zre.ToString())
        Next

        'ggf. KWL-Datei einlesen
        '-----------------------
        If (Me.useKWL) Then

            Dim KWLpath As String = Me.WorkDir_Current & Me.Datensatz & ".KWL"

            Dim KWLtmp As Wave.WEL = New Wave.WEL(KWLpath, True)

            'Reihen zu Simulationsergebnis hinzuf�gen
            For Each zre As Wave.Zeitreihe In KWLtmp.Zeitreihen
                Me.SimErgebnis.Add(zre, zre.ToString())
            Next

        End If

        'Bei IHA-Berechnung jetzt IHA-Software ausf�hren
        '-----------------------------------------------
        If (Me.isIHA) Then
            'IHA-Ziel raussuchen und Simulationsreihe �bergeben
            'HACK: es wird immer das erste IHA-Ziel verwendet!
            For Each feature As Common.Featurefunction In Me.mProblem.List_Featurefunctions
                If (feature.Typ = "IHA") Then
                    Call Me.IHASys.calculate_IHA(Me.SimErgebnis(feature.SimGr))
                    Exit For
                End If
            Next
        End If

    End Sub

#End Region 'Evaluierung

#Region "Qualit�tswertberechnung"

    'Berechnung des Qualit�tswerts (Zielwert)
    '****************************************
    Public Overrides Function CalculateFeature(ByVal feature As Common.Featurefunction) As Double

        CalculateFeature = 0

        'Fallunterscheidung Ergebnisdatei
        '--------------------------------
        Select Case feature.Datei

            Case "WEL", "KWL"
                'QWert aus WEL- oder KWL-Datei
                CalculateFeature = CalculateFeature_WEL(feature)

            Case "PRB"
                'QWert aus PRB-Datei
                'BUG 220: PRB geht nicht, weil keine Zeitreihe
                Throw New Exception("PRB als OptZiel geht z.Zt. nicht (siehe Bug 138)")
                'CalculateFeature = CalculateFeature_PRB(OptZiel)

            Case Else
                Throw New Exception("Der Wert '" & feature.Datei & "' f�r die Datei wird bei Optimierungszielen f�r BlueM nicht unterst�tzt!")

        End Select

        'Zielrichtung ber�cksichtigen
        CalculateFeature *= feature.Richtung

    End Function

    'Qualit�tswert aus WEL-Datei
    '***************************
    Private Function CalculateFeature_WEL(ByVal feature As Common.Featurefunction) As Double

        Dim featurevalue As Double
        Dim SimReihe As Wave.Zeitreihe

        'Simulationsergebnis auslesen
        SimReihe = Me.SimErgebnis(feature.SimGr).Clone()

        'Fallunterscheidung Zieltyp
        '--------------------------
        Select Case feature.Typ

            Case "Wert"
                featurevalue = MyBase.CalculateFeature_Wert(feature, SimReihe)

            Case "Reihe"
                featurevalue = MyBase.CalculateFeature_Reihe(feature, SimReihe)

            Case "Kosten"
                featurevalue = Me.SKos1.Calculate_Costs(Me.WorkDir_Current)

            Case "IHA"
                featurevalue = Me.IHAProc.CalculateFeature_IHA(feature, Me.IHASys.RVAResult)

        End Select

        Return featurevalue

    End Function

    'Qualit�tswert aus PRB-Datei
    '***************************
    Private Function CalculateFeature_PRB(ByVal feature As Common.Featurefunction) As Double

        'BUG 220: PRB geht nicht, weil keine Zeitreihe
        'Dim i As Integer
        'Dim IsOK As Boolean
        'Dim QWert As Double
        'Dim SimReihe As Object(,) = {}

        ''Simulationsergebnis auslesen
        'IsOK = Read_PRB(WorkDir & Datensatz & ".PRB", ziel.SimGr, SimReihe)

        ''Diff
        ''----
        ''�berfl�ssige St�tzstellen (P) entfernen
        ''---------------------------------------
        ''Anzahl St�tzstellen bestimmen
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
        ''Reihe um eine St�tzstelle erweitern
        ''PRBtmp(stuetz, 0) = PRBtmp(stuetz - 1, 0)
        ''PRBtmp(stuetz, 1) = PRBtmp(stuetz - 1, 1)

        ''An St�tzstellen der ZielReihe interpolieren
        ''-------------------------------------------
        'Dim PRBintp(ziel.ZielReihe.GetUpperBound(0), 1) As Object
        'Dim j As Integer
        'For i = 0 To ziel.ZielReihe.GetUpperBound(0)
        '    'zugeh�rige Lamelle in SimReihe finden
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

#End Region 'Qualit�tswertberechnung

#Region "Kombinatorik"

    'Kombinatorik
    '############

    'Mehrere Pr�fungen ob die .VER Datei des BlueM und der .CES Datei auch zusammenpassen
    '************************************************************************************
    Public Sub Validate_CES_fits_to_VER()

        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim x As Integer = 0
        Dim y As Integer = 0

        Dim FoundA(VerzweigungsDatei.GetUpperBound(0)) As Boolean

        'Pr�ft ob jede Verzweigung einmal in der LocationList vorkommt
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

        'Pr�ft ob die nicht vorkommenden Verzweigungen Verzweigungen anderer Art sind
        For i = 0 To VerzweigungsDatei.GetUpperBound(0)
            If Not VerzweigungsDatei(i, 1) = "2" And FoundA(i) = False Then
                FoundA(i) = True
            End If
        Next

        Dim FoundB As Boolean = True
        Dim TmpBool As Boolean = False

        'Pr�ft ob alle in der LocationList Vorkommenden Verzweigungen auch in der Verzweigungsdatei sind
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

        '�bergabe
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
        'Datei �ffnen
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