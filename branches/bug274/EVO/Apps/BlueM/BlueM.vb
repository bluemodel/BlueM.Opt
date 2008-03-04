Imports System.IO
Imports IHWB.BlueM.DllAdapter

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

    Public Overrides ReadOnly Property Datensatzendung() As String
        Get
            Return ".ALL"
        End Get
    End Property

    'BlueM DLL
    '---------
    Private bluem_dll As BlueM_EngineDotNetAccess

    'IHA
    '---
    Friend isIHA As Boolean = False
    Friend IHASys As IHWB.IHA.IHAAnalysis
    Friend IHAProc As IHWB.EVO.IHAProcessor

#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

    'Konstruktor
    '***********
    Public Sub New()

        Call MyBase.New()

        'BlueM DLL instanzieren
        '----------------------
        Dim dll_path As String
        dll_path = System.Windows.Forms.Application.StartupPath() & "\Apps\BlueM\BlueM.dll"

        If (File.Exists(dll_path)) Then
            bluem_dll = New BlueM_EngineDotNetAccess(dll_path)
        Else
            Throw New Exception("BlueM.dll nicht gefunden!")
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

        'ALL-Datei öffnen
        '----------------
        Dim Datei As String = Me.WorkDir & Me.Datensatz & ".ALL"

        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
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

    'Optimierungsziele einlesen
    '**************************
    Protected Overrides Sub Read_ZIE()

        Call MyBase.Read_ZIE()

        'Weiterverarbeitung von ZielReihen:
        'BUG 274!
        '----------------------------------
        Dim i As Integer
        Dim IHAZielReihe As Wave.Zeitreihe
        Dim IHAStart, IHAEnde As DateTime

        IHAZielReihe = New Wave.Zeitreihe("new")

        'Gibt es eine IHA-Zielfunktion?
        'HACK: es wird immer nur das erste IHA-Ziel verwendet!
        '------------------------------
        For i = 0 To Me.OptZielMgr.List_OptZiele.GetUpperBound(0)
            If (Me.OptZielMgr.List_OptZiele(i).ZielTyp = "IHA") Then
                'IHA-Berechnung einschalten
                Me.isIHA = True
                IHAZielReihe = Me.OptZielMgr.List_OptZiele(i).ZielReihe
                IHAStart = Me.OptZielMgr.List_OptZiele(i).EvalStart
                IHAEnde = Me.OptZielMgr.List_OptZiele(i).EvalEnde
                Exit For
            End If
        Next i

        'Falls noch keins gefunden auch SekZiele durchsuchen
        If (Not Me.isIHA) Then
            For i = 0 To Me.OptZielMgr.List_SekZiele.GetUpperBound(0)
                If (Me.OptZielMgr.List_SekZiele(i).ZielTyp = "IHA") Then
                    'IHA-Berechnung einschalten
                    Me.isIHA = True
                    IHAZielReihe = Me.OptZielMgr.List_SekZiele(i).ZielReihe
                    IHAStart = Me.OptZielMgr.List_SekZiele(i).EvalStart
                    IHAEnde = Me.OptZielMgr.List_SekZiele(i).EvalEnde
                    Exit For
                End If
            Next i
        End If

        'IHA-Berechnung vorbereiten
        '--------------------------
        If (Me.isIHA) Then
            'IHAAnalyse-Objekt instanzieren
            Me.IHASys = New IHWB.IHA.IHAAnalysis(Me.WorkDir & "IHA\", IHAZielreihe, IHAStart, IHAEnde)

            'IHAProcessor-Objekt instanzieren
            Me.IHAProc = New IHWB.EVO.IHAProcessor()

            'IHA-Vergleichsmodus?
            '--------------------
            Dim reffile As String = Me.WorkDir & Me.Datensatz & ".rva"
            If (File.Exists(reffile)) Then

                Dim RVABase As New Wave.RVA(reffile)

                'Vergleichsmodus aktivieren
                Call Me.IHAProc.setComparisonMode(RVABase.RVAValues)
            End If

        End If

    End Sub

    'Kombinatorik einlesen
    '*********************
    Protected Overrides Sub Read_Kombinatorik()

        Dim Datei As String = WorkDir & Datensatz & "." & Combi_Ext

        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        Dim Zeile As String
        Dim Anz As Integer = 0

        'Anzahl der Parameter feststellen
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                Anz += 1
            End If
        Loop Until StrRead.Peek() = -1

        Dim i As Integer = -1
        Dim j As Integer = 0
        ReDim List_Locations(0)
        ReDim List_Locations(0).List_Massnahmen(0)

        'Zurück zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        Dim array() As String
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                array = Zeile.Split("|")
                'Werte zuweisen

                If Not Is_Name_IN(array(1).Trim(), List_Locations) Then
                    i += 1
                    j = 0
                    System.Array.Resize(List_Locations, i + 1)
                    List_Locations(i).Name = array(1).Trim()
                End If
                System.Array.Resize(List_Locations(i).List_Massnahmen, j + 1)
                ReDim List_Locations(i).List_Massnahmen(j).Schaltung(2, 1)
                ReDim List_Locations(i).List_Massnahmen(j).Bauwerke(3)
                List_Locations(i).List_Massnahmen(j).Name = array(2).Trim()
                List_Locations(i).List_Massnahmen(j).Schaltung(0, 0) = array(3).Trim()
                List_Locations(i).List_Massnahmen(j).Schaltung(0, 1) = array(4).Trim()
                List_Locations(i).List_Massnahmen(j).Schaltung(1, 0) = array(5).Trim()
                List_Locations(i).List_Massnahmen(j).Schaltung(1, 1) = array(6).Trim()
                List_Locations(i).List_Massnahmen(j).Schaltung(2, 0) = array(7).Trim()
                List_Locations(i).List_Massnahmen(j).Schaltung(2, 1) = array(8).Trim()
                List_Locations(i).List_Massnahmen(j).KostenTyp = array(9).Trim()
                List_Locations(i).List_Massnahmen(j).Bauwerke(0) = array(10).Trim()
                List_Locations(i).List_Massnahmen(j).Bauwerke(1) = array(11).Trim()
                List_Locations(i).List_Massnahmen(j).Bauwerke(2) = array(12).Trim()
                List_Locations(i).List_Massnahmen(j).Bauwerke(3) = array(13).Trim()
                List_Locations(i).List_Massnahmen(j).TestModus = Convert.ToInt16(array(14).Trim())
                j += 1
            End If

        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

    End Sub

    'Liest die Verzweigungen aus BlueM in ein Array ein
    'Und Dimensioniert das Verzweigungsarray
    '*******************************************************
    Protected Overrides Sub Read_Verzweigungen()

        Dim i As Integer

        Dim FiStr As FileStream = New FileStream(WorkDir & Datensatz & ".ver", FileMode.Open, IO.FileAccess.ReadWrite)
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

    'BlauesModell ausführen (simulieren)
    '***********************************
    Public Overrides Function launchSim() As Boolean

        Dim simOK As Boolean
        Dim i As Integer

        Try

            Call bluem_dll.Initialize(Me.WorkDir & Me.Datensatz)

            Dim SimEnde As DateTime = BlueM_EngineDotNetAccess.DateTime(bluem_dll.GetSimulationEndDate())

            'Simulationszeitraum 
            Do While (BlueM_EngineDotNetAccess.DateTime(bluem_dll.GetCurrentTime) <= SimEnde)
                Call bluem_dll.PerformTimeStep()
            Loop

            'Simulation erfolgreich
            simOK = True

        Catch ex As Exception

            'Simulationsfehler aufgetreten
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "BlueM")
            simOK = False

        Finally

            Call bluem_dll.Finish()
            Call bluem_dll.Dispose()

        End Try

        'Simulationsergebnis verarbeiten
        '-------------------------------
        If (simOK) Then

            'Altes Simulationsergebnis löschen
            Me.SimErgebnis.Clear()

            'WEL-Datei einlesen
            Dim WELtmp As Wave.WEL = New Wave.WEL(Me.WorkDir & Me.Datensatz & ".WEL", True)

            'Simulationsergebnis abspeichern
            For Each zre As Wave.Zeitreihe In WELtmp.Zeitreihen
                Me.SimErgebnis.Add(zre, zre.ToString())
            Next

            'Bei IHA-Berechnung jetzt IHA-Software ausführen
            If (Me.isIHA) Then
                'IHA-Ziel raussuchen und Simulationsreihe übergeben
                'HACK: es wird immer das erste IHA-Ziel verwendet!
                For i = 0 To Me.OptZielMgr.List_OptZiele.GetUpperBound(0)
                    If (Me.OptZielMgr.List_OptZiele(i).ZielTyp = "IHA") Then
                        Call Me.IHASys.calculate_IHA(Me.SimErgebnis(Me.OptZielMgr.List_OptZiele(i).SimGr))
                        Exit For
                    End If
                Next
            End If

        End If

        Return simOK

    End Function

#End Region 'Evaluierung

#Region "Qualitätswertberechnung"

    'Berechnung des Qualitätswerts (Zielwert)
    '****************************************
    Public Overrides Function QWert(ByVal OptZiel As Common.OptZiel) As Double

        QWert = 0

        'Fallunterscheidung Ergebnisdatei
        '--------------------------------
        Select Case OptZiel.Datei

            Case "WEL"
                'QWert aus WEL-Datei
                QWert = QWert_WEL(OptZiel)

            Case "PRB"
                'QWert aus PRB-Datei
                'BUG 220: PRB geht nicht, weil keine Zeitreihe
                Throw New Exception("PRB als OptZiel geht z.Zt. nicht (siehe Bug 138)")
                'QWert = QWert_PRB(OptZiel)

            Case Else
                Throw New Exception("Der Wert '" & Optziel.Datei & "' für die Datei wird bei Optimierungszielen für BlueM nicht akzeptiert!")

        End Select

    End Function

    'Qualitätswert aus WEL-Datei
    '***************************
    Private Function QWert_WEL(ByVal OptZiel As Common.OptZiel) As Double

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

            Case "Kosten"
                QWert = Me.SKos1.calculate_costs(Me)

            Case "IHA"
                QWert = Me.IHAProc.QWert_IHA(OptZiel, Me.IHASys.RVAResult)

        End Select

        Return QWert

    End Function

#End Region 'Qualitätswertberechnung

#Region "Kombinatorik"

    'Kombinatorik
    '############

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

        DateiPfad = WorkDir & Datensatz & ".ver"
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
                            StrRight = Microsoft.VisualBasic.Right(Zeilenarray(j), 49)
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

    'Hilfs Funktionen
    'XXXXXXXXXXXXXXXX

    'Hilfsfunktion um zu Prüfen ob der Name bereits vorhanden ist oder nicht
    '***********************************************************************
    Public Shared Function Is_Name_IN(ByVal Name As String, ByVal Array() As Struct_Lokation) As Boolean
        Is_Name_IN = False
        Dim i As Integer
        For i = 0 To Array.GetUpperBound(0)
            If Name = Array(i).Name Then
                Is_Name_IN = True
                Exit Function
            End If
        Next
    End Function

#End Region 'Methoden

End Class
