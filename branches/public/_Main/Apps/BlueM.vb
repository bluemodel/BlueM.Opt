Imports System.IO
Imports IHWB.BlueM.DllAdapter

'*******************************************************************************
'*******************************************************************************
'**** Klasse BlueM                                                          ****
'****                                                                       ****
'**** Funktionen zur Kontrolle des BlauenModells                            ****
'****                                                                       ****
'**** Christoph Huebner, Felix Froehlich                                    ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'****                                                                       ****
'**** Erstellt: Dezember 2006                                               ****
'****                                                                       ****
'**** Letzte �nderung: Juli 2007                                           ****
'*******************************************************************************
'*******************************************************************************

Public Class BlueM
    Inherits Sim

#Region "Eigenschaften"

    'Eigenschaften
    '#############

    'BlueM DLL
    '---------
    Private bluem_dll As BlueM_EngineDotNetAccess

    'IHA
    '---
    Private isIHA As Boolean = False
    Private IHA1 As IHA

#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

    'Konstruktor
    '***********
    Public Sub New()

        Call MyBase.New()

        If (Me.isDll) Then
            'BlueM DLL instanzieren
            bluem_dll = New BlueM_EngineDotNetAccess(Me.Dll)
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

    'Optimierungsziele einlesen
    '**************************
    Protected Overrides Sub Read_OptZiele()

        Call MyBase.Read_OptZiele()

        'Weiterverarbeitung von ZielReihen:
        '----------------------------------
        Dim i As Integer

        'Gibt es eine IHA-Zielfunktion?
        For i = 0 To Me.List_OptZiele.GetUpperBound(0)
            If (Me.List_OptZiele(i).ZielTyp = "IHA") Then
                'HACK: es wird immer das erste IHA-Ziel verwendet!
                'IHA-Berechnung einschalten
                Me.isIHA = True
                'IHA-Objekt instanziieren
                Me.IHA1 = New IHA(Me.List_OptZiele(i))
                'IHA-Berechnung vorbereiten
                Call Me.IHA1.prepare_IHA(Me)
                Exit For
            End If
        Next


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

        'Zur�ck zum Dateianfang und lesen
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

    'BlauesModell ausf�hren (simulieren)
    '***********************************
    Public Overrides Function launchSim() As Boolean

        Dim simOK As Boolean

        If (Not Me.isDll) Then

            'Als EXE simulieren
            'xxxxxxxxxxxxxxxxxx

            'Aktuelles Verzeichnis bestimmen
            Dim currentDir As String = CurDir()
            'zum Arbeitsverzeichnis wechseln
            ChDrive(WorkDir)
            ChDir(WorkDir)
            'EXE aufrufen
            Dim ProcID As Integer = Shell("""" & Exe & """ " & Datensatz, AppWinStyle.MinimizedNoFocus, True)
            'zur�ck ins Ausgangsverzeichnis wechseln
            ChDrive(currentDir)
            ChDir(currentDir)

            '�berpr�fen, ob Simulation erfolgreich
            '-------------------------------------
            If (File.Exists(WorkDir & "$FEHL.TMP")) Then

                'Simulationsfehler aufgetreten
                simOK = False

                Dim DateiInhalt As String = ""
                Dim FiStr As FileStream = New FileStream(WorkDir & "$fehl.tmp", FileMode.Open, IO.FileAccess.Read)
                Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
                Do
                    DateiInhalt = DateiInhalt & Chr(13) & Chr(10) & StrRead.ReadLine.ToString
                Loop Until StrRead.Peek() = -1

                MsgBox("BlueM hat einen Fehler zur�ckgegeben:" & Chr(13) & Chr(10) & DateiInhalt, MsgBoxStyle.Exclamation, "BlueM")

            Else
                'Simulation erfolgreich
                simOK = True
            End If


        Else

            'Als DLL simulieren
            'xxxxxxxxxxxxxxxxxx

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

        End If

        'Bei IHA-Berechnung jetzt IHA-Software ausf�hren
        '-----------------------------------------------
        If (simOK And Me.isIHA) Then
            Call Me.IHA1.calculate_IHA(Me.WorkDir & Me.Datensatz & ".WEL")
        End If

        Return simOK

    End Function

#End Region 'Evaluierung

#Region "Qualit�tswertberechnung"

    'Qualit�tswert aus WEL-Datei
    '***************************
    Protected Overrides Function QWert_WEL(ByVal OptZiel As Struct_OptZiel) As Double

        Dim QWert As Double

        'Simulationsergebnis auslesen
        Dim SimReihe As New Wave.Zeitreihe(OptZiel.SimGr)
        Dim WEL As New Wave.WEL(WorkDir & Datensatz & ".wel", OptZiel.SimGr)
        SimReihe = WEL.Read_WEL()(0)

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
                QWert = Me.IHA1.QWert_IHA(OptZiel)

        End Select

        Return QWert

    End Function

#End Region 'Qualit�tswertberechnung

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

    'Hilfsfunktion um zu Pr�fen ob der Name bereits vorhanden ist oder nicht
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