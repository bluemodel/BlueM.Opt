Imports System.IO

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
'**** Letzte �nderung: April 2007                                           ****
'*******************************************************************************
'*******************************************************************************

Public Class BlueM
    Inherits Sim

#Region "Eigenschaften"

    'Eigenschaften
    '#############

    'IHA
    '---
    Private IHA1 As New IHA()

    'Kombinatorik
    '------------
    Public SKos1 As New SKos()

    Public Structure Massnahme
        Public Name As String
        Public Schaltung(,) As String
        Public KostenTyp As Integer
        Public Bauwerke() As String
    End Structure

    Public Structure Lokation
        Public Name As String
        Public MassnahmeListe() As Massnahme
    End Structure

    Public LocationList() As Lokation

    Public VerzweigungsDatei(,) As String

    'Public Schaltung(2, 1) As Object
    'Public Ma�nahme As Collection
    'Public Kombinatorik As Collection

#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

    'Simulationsparameter einlesen
    '*****************************
    Protected Overrides Sub SimParameter_einlesen()

        Dim SimStart_str As String = ""
        Dim SimEnde_str As String = ""

        'ALL-Datei �ffnen
        '----------------
        Try
            Dim Datei As String = Me.WorkDir & Me.Datensatz & ".ALL"

            Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
            Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

            'Alle Zeilen durchlaufen
            Dim Zeile As String
            Do
                Zeile = StrRead.ReadLine.ToString()
                'Simulationszeitraum auslesen
                If (Zeile.StartsWith(" SimBeginn - SimEnde ............:") = True) Then
                    SimStart_str = Zeile.Substring(35, 16)
                    SimEnde_str = Zeile.Substring(54, 16)
                End If
            Loop Until StrRead.Peek() = -1

            'SimStart und SimEnde in echtes Datum konvertieren
            Me.SimStart = New DateTime(SimStart_str.Substring(6, 4), SimStart_str.Substring(3, 2), SimStart_str.Substring(0, 2), SimStart_str.Substring(11, 2), SimStart_str.Substring(14, 2), 0)
            Me.SimEnde = New DateTime(SimEnde_str.Substring(6, 4), SimEnde_str.Substring(3, 2), SimEnde_str.Substring(0, 2), SimEnde_str.Substring(11, 2), SimEnde_str.Substring(14, 2), 0)

        Catch except As Exception
            MsgBox("Fehler beim einlesen der BlueM-Simulationsparameter:" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Critical, "Fehler")
            Exit Sub
        End Try

    End Sub

    'Die ModellParameter in die BM-Eingabedateien schreiben
    '******************************************************
    Public Overrides Sub ModellParameter_schreiben()
        Dim Wert As String
        Dim AnzZeil As Integer
        Dim j As Integer
        Dim Zeilenarray() As String
        Dim Zeile As String
        Dim StrLeft As String
        Dim StrRight As String
        Dim DateiPfad As String

        'ModellParameter aus OptParametern kalkulieren()
        Call OptParameter_to_ModellParameter()

        'Alle ModellParameter durchlaufen
        For i As Integer = 0 To ModellParameterListe.GetUpperBound(0)
            Try
                DateiPfad = WorkDir & Datensatz & "." & ModellParameterListe(i).Datei
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

                'Zeile �ndern
                Zeile = Zeilenarray(ModellParameterListe(i).ZeileNr - 1)
                Dim Length As Short = ModellParameterListe(i).SpBis - ModellParameterListe(i).SpVon
                StrLeft = Microsoft.VisualBasic.Left(Zeile, ModellParameterListe(i).SpVon - 1)
                StrRight = Microsoft.VisualBasic.Right(Zeile, Len(Zeile) - ModellParameterListe(i).SpBis + 1)

                Wert = ModellParameterListe(i).Wert.ToString()
                If (Wert.Length > Length) Then
                    'TODO: Parameter wird f�r erforderliche Stringl�nge einfach abgeschnitten, sollte aber gerundet werden!
                    Wert = Wert.Substring(0, Length)
                Else
                    Wert = Wert.PadLeft(Length)
                End If
                Zeilenarray(ModellParameterListe(i).ZeileNr - 1) = StrLeft & Wert & StrRight

                'Alle Zeilen wieder in Datei schreiben
                Dim StrWrite As StreamWriter = New StreamWriter(DateiPfad, False, System.Text.Encoding.GetEncoding("iso8859-1"))
                For j = 0 To AnzZeil - 1
                    StrWrite.WriteLine(Zeilenarray(j))
                Next

                StrWrite.Close()

            Catch except As Exception
                MsgBox("Fehler beim Schreiben der Mutierten Parameter" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
            End Try
        Next

    End Sub

    'BlauesModell ausf�hren (simulieren)
    '***********************************
    Public Overrides Sub launchSim()
        'starte Programm mit neuen Parametern
        Dim ProcID As Integer
        'Aktuelles Arbeitsverzeichnis feststellen
        Dim currentDir As String = CurDir()
        'zum gew�nschten Arbeitsverzeichnis navigieren
        ChDrive(WorkDir) 'nur n�tig falls Arbeitsverzeichnis und aktuelles Verzeichnis auf verschiedenen Laufwerken sind
        ChDir(WorkDir)
        'EXE aufrufen
        ProcID = Shell("""" & Exe & """ " & Datensatz, AppWinStyle.MinimizedNoFocus, True)
        'Arbeitsverzeichnis wieder zur�cksetzen (optional)
        ChDrive(currentDir)
        ChDir(currentDir)

        '�berpr�fen, ob Simulation erfolgreich
        '-------------------------------------
        If (File.Exists(WorkDir & "$FEHL.TMP")) Then

            'Fehler aufgetreten
            Dim DateiInhalt As String = ""

            Try
                Dim FiStr As FileStream = New FileStream(WorkDir & "$fehl.tmp", FileMode.Open, IO.FileAccess.Read)
                Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

                Do
                    DateiInhalt = DateiInhalt & Chr(13) & Chr(10) & StrRead.ReadLine.ToString
                Loop Until StrRead.Peek() = -1

                MsgBox("Das BlaueModell hat einen Fehler zur�ckgegeben:" & Chr(13) & Chr(10) & DateiInhalt, MsgBoxStyle.Exclamation, "Simulationsfehler")

            Catch except As Exception
                MsgBox("Konnte Datei ""$FEHL.TMP"" nicht lesen!" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
            End Try

        End If

    End Sub

    'Qualit�tswert aus WEL-Datei
    '***************************
    Protected Overrides Function QWert_WEL(ByVal OptZiel As OptZiel) As Double

        Dim IsOK As Boolean
        Dim QWert As Double
        Dim SimReihe(,) As Object = {}

        'Simulationsergebnis auslesen
        IsOK = Read_WEL(WorkDir & Datensatz & ".wel", OptZiel.SimGr, SimReihe)

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
                QWert = Me.IHA1.calculate_IHA(SimReihe)

        End Select

        Return QWert

    End Function


#Region "TeeChart"

    'TeeChart
    '########

    'TeeChart Initialisierung f�r SingleObjective
    '********************************************
    Public Overrides Sub TeeChartInitialise_SO(ByVal n_Populationen As Integer, ByVal n_Kalkulationen As Integer, ByRef TChart1 As Steema.TeeChart.TChart)

        Call MyBase.TeeChartInitialise_SO(n_Populationen, n_Kalkulationen, TChart1)
        TChart1.Header.Text = "BlauesModell"

    End Sub

    'TeeChart Initialisierung f�r MultiObjective
    '*******************************************
    Public Overrides Sub TeeChartInitialise_MO(ByRef TChart1 As Steema.TeeChart.TChart)

        Call MyBase.TeeChartInitialise_MO(TChart1)
        TChart1.Header.Text = "BlauesModell"

    End Sub

#End Region 'TeeChart

#Region "Kombinatorik"

    'Kombinatorik
    '############

    'Kombinatorik einlesen
    '*********************
    Public Sub Read_CES()
        Try
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
            ReDim LocationList(0)
            ReDim LocationList(0).MassnahmeListe(0)

            'Zur�ck zum Dateianfang und lesen
            FiStr.Seek(0, SeekOrigin.Begin)

            Dim array() As String
            Do
                Zeile = StrRead.ReadLine.ToString()
                If (Zeile.StartsWith("*") = False) Then
                    array = Zeile.Split("|")
                    'Werte zuweisen

                    If Not Is_Name_IN(array(1).Trim(), LocationList) Then
                        i += 1
                        j = 0
                        System.Array.Resize(LocationList, i + 1)
                        LocationList(i).Name = array(1).Trim()
                    End If
                    System.Array.Resize(LocationList(i).MassnahmeListe, j + 1)
                    ReDim LocationList(i).MassnahmeListe(j).Schaltung(2, 1)
                    ReDim LocationList(i).MassnahmeListe(j).Bauwerke(3)
                    LocationList(i).MassnahmeListe(j).Name = array(2).Trim()
                    LocationList(i).MassnahmeListe(j).Schaltung(0, 0) = array(3).Trim()
                    LocationList(i).MassnahmeListe(j).Schaltung(0, 1) = array(4).Trim()
                    LocationList(i).MassnahmeListe(j).Schaltung(1, 0) = array(5).Trim()
                    LocationList(i).MassnahmeListe(j).Schaltung(1, 1) = array(6).Trim()
                    LocationList(i).MassnahmeListe(j).Schaltung(2, 0) = array(7).Trim()
                    LocationList(i).MassnahmeListe(j).Schaltung(2, 1) = array(8).Trim()
                    LocationList(i).MassnahmeListe(j).KostenTyp = array(9).Trim()
                    LocationList(i).MassnahmeListe(j).Bauwerke(0) = array(10).Trim()
                    LocationList(i).MassnahmeListe(j).Bauwerke(1) = array(11).Trim()
                    LocationList(i).MassnahmeListe(j).Bauwerke(2) = array(12).Trim()
                    LocationList(i).MassnahmeListe(j).Bauwerke(3) = array(13).Trim()
                    j += 1
                End If

            Loop Until StrRead.Peek() = -1

            StrRead.Close()
            FiStr.Close()

        Catch except As Exception
            MsgBox(except.Message & Chr(13) & Chr(10) & "Ein Fehler k�nnten Leerzeichen in der letzten Zeile der Datei sein.", MsgBoxStyle.Exclamation, "Fehler beim Lesen der Kombinatorik")
        End Try

    End Sub

    'Validierungsfunktion der Kombinatorik Pr�ft ob Verbraucher an zwei Standorten Dopp vorhanden sind
    '*************************************************************************************************
    Public Function Combinatoric_is_Valid() As Boolean
        Combinatoric_is_Valid = True
        Dim i, j, x, y, m, n As Integer

        For i = 0 To LocationList.GetUpperBound(0)
            For j = 1 To LocationList.GetUpperBound(0)
                For x = 0 To LocationList(i).MassnahmeListe.GetUpperBound(0)
                    For y = 0 To LocationList(j).MassnahmeListe.GetUpperBound(0)
                        For m = 0 To 2
                            For n = 0 To 2
                                If Not LocationList(i).MassnahmeListe(x).Schaltung(m, 0) = "X" And LocationList(j).MassnahmeListe(y).Schaltung(n, 0) = "X" Then
                                    If LocationList(i).MassnahmeListe(x).Schaltung(m, 0) = LocationList(j).MassnahmeListe(y).Schaltung(n, 0) Then
                                        Combinatoric_is_Valid = False
                                    End If
                                End If
                            Next
                        Next
                    Next
                Next
            Next
        Next
    End Function

    'Liest die Verzweigungen aus dem BModel in ein Array ein
    '*******************************************************
    Public Sub Verzweigung_Read()
        Dim i As Integer

        Try
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

        Catch except As Exception
            MsgBox(except.Message, MsgBoxStyle.Exclamation, "Fehler beim Lesen der Kombinatorik")
        End Try
    End Sub

    'Mehrere Pr�fungen ob die .VER Datei des BlueM und der .CES Datei auch zusammenpassen
    '***************************
    Public Function CES_fits_to_VER() As Boolean
        CES_fits_to_VER = True
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim x As Integer = 0
        Dim y As Integer = 0

        Dim FoundA(VerzweigungsDatei.GetUpperBound(0)) As Boolean

        'Pr�ft ob jede Verzweigung einmal in der LocationList vorkommt
        For i = 0 To VerzweigungsDatei.GetUpperBound(0)
            For j = 0 To LocationList.GetUpperBound(0)
                For x = 0 To LocationList(j).MassnahmeListe.GetUpperBound(0)
                    For y = 0 To LocationList(j).MassnahmeListe(x).Schaltung.GetUpperBound(0)
                        If VerzweigungsDatei(i, 0) = LocationList(j).MassnahmeListe(x).Schaltung(y, 0) And VerzweigungsDatei(i, 1) = "2" Then
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
        For j = 0 To LocationList.GetUpperBound(0)
            For x = 0 To LocationList(j).MassnahmeListe.GetUpperBound(0)
                For y = 0 To LocationList(j).MassnahmeListe(x).Schaltung.GetUpperBound(0)
                    If Not LocationList(j).MassnahmeListe(x).Schaltung(y, 0) = "X" Then
                        TmpBool = False
                        For i = 0 To VerzweigungsDatei.GetUpperBound(0)
                            If VerzweigungsDatei(i, 0) = LocationList(j).MassnahmeListe(x).Schaltung(y, 0) And VerzweigungsDatei(i, 1) = "2" Then
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
            CES_fits_to_VER = False
        Else
            For i = 0 To FoundA.GetUpperBound(0)
                If FoundA(i) = False Then
                    CES_fits_to_VER = False
                End If
            Next
        End If

    End Function

    'Schreibt die neuen Verzweigungen
    '********************************
    Public Sub Verzweigung_Write(ByVal SchaltArray(,))

        Dim AnzZeil As Integer
        Dim i, j As Integer
        Dim Zeilenarray() As String
        Dim Zeile As String
        Dim StrLeft As String
        Dim StrRight As String
        Dim DateiPfad As String
        Dim SplitZeile() As String

        Try
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
            For i = 0 To SchaltArray.GetUpperBound(0)
                If Not SchaltArray(i, 1) = Nothing Then
                    For j = 0 To Zeilenarray.GetUpperBound(0)
                        If Not Zeilenarray(j).StartsWith("*") Then
                            SplitZeile = Zeilenarray(j).Split("|")
                            If SchaltArray(i, 0) = SplitZeile(1).Trim Then
                                StrLeft = Microsoft.VisualBasic.Left(Zeilenarray(j), 31)
                                StrRight = Microsoft.VisualBasic.Right(Zeilenarray(j), 49)
                                If SchaltArray(i, 1) = "1" Then
                                    Zeilenarray(j) = StrLeft & "      100     " & StrRight
                                ElseIf (SchaltArray(i, 1) = "0") Then
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

        Catch except As Exception
            MsgBox("Fehler beim Schreiben der Mutierten Parameter" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
        End Try

    End Sub

#End Region 'Kombinatorik

#Region "IHA"

    'IHA
    '###

    'Optimierungsziele einlesen
    '**************************
    Protected Overrides Sub OptZiele_einlesen()

        Call MyBase.OptZiele_einlesen()

        'Weiterverarbeitung von ZielReihen:
        '----------------------------------
        Dim i As Integer

        'IHA
        For i = 0 To Me.OptZieleListe.GetUpperBound(0)
            If (Me.OptZieleListe(i).ZielTyp = "IHA") Then
                'IHA-Berechnung vorbereiten
                Call Me.IHA1.IHA_prepare(Me)
                Exit For
            End If
        Next

    End Sub

#End Region

    'Hilfs Funktionen
    'XXXXXXXXXXXXXXXX

    'Hilfsfunktion um zu Pr�fen ob der Name bereits vorhanden ist oder nicht
    '***********************************************************************
    Public Shared Function Is_Name_IN(ByVal Name As String, ByVal Array() As Lokation) As Boolean
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
