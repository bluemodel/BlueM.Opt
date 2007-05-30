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
'**** Letzte Änderung: April 2007                                           ****
'*******************************************************************************
'*******************************************************************************

Public Class Smusi
    Inherits Sim

#Region "Eigenschaften"

    'Eigenschaften
    '#############

#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

    'Simulationsparameter einlesen
    '*****************************
    Protected Overrides Sub SimParameter_einlesen()

        Dim SimStart_str As String = ""
        Dim SimEnde_str As String = ""

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
            If (Zeile.StartsWith("    SimBeginn - SimEnde")) Then
                SimStart_str = Zeile.Substring(38, 16)
                SimEnde_str = Zeile.Substring(57, 16)
            End If

        Loop Until StrRead.Peek() = -1

        'SimStart und SimEnde in echtes Datum konvertieren
        Me.SimStart = New DateTime(SimStart_str.Substring(6, 4), SimStart_str.Substring(3, 2), SimStart_str.Substring(0, 2), SimStart_str.Substring(11, 2), SimStart_str.Substring(14, 2), 0)
        Me.SimEnde = New DateTime(SimEnde_str.Substring(6, 4), SimEnde_str.Substring(3, 2), SimEnde_str.Substring(0, 2), SimEnde_str.Substring(11, 2), SimEnde_str.Substring(14, 2), 0)

        'Zeitschrittweite in echte Dauer konvertieren
        Me.SimDT = New TimeSpan(0, 5, 0)

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

            DateiPfad = WorkDir & Datensatz & "." & ModellParameterListe(i).Datei
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

            'Zeile ändern
            Zeile = Zeilenarray(ModellParameterListe(i).ZeileNr - 1)
            Dim Length As Short = ModellParameterListe(i).SpBis - ModellParameterListe(i).SpVon
            StrLeft = Microsoft.VisualBasic.Left(Zeile, ModellParameterListe(i).SpVon - 1)
            StrRight = Microsoft.VisualBasic.Right(Zeile, Len(Zeile) - ModellParameterListe(i).SpBis + 1)

            Wert = ModellParameterListe(i).Wert.ToString()
            If (Wert.Length > Length) Then
                'TODO: Parameter wird für erforderliche Stringlänge einfach abgeschnitten, sollte aber gerundet werden!
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

        Next

    End Sub

    'BlauesModell ausführen (simulieren)
    '***********************************
    Public Overrides Sub launchSim()
        'starte Programm mit neuen Parametern
        Dim ProcID As Integer
        'Aktuelles Arbeitsverzeichnis feststellen
        Dim currentDir As String = CurDir()
        'zum gewünschten Arbeitsverzeichnis navigieren
        ChDrive(WorkDir) 'nur nötig falls Arbeitsverzeichnis und aktuelles Verzeichnis auf verschiedenen Laufwerken sind
        ChDir(WorkDir)
        'EXE aufrufen
        ProcID = Shell("""" & Exe & """ " & Datensatz & " 1", AppWinStyle.MinimizedNoFocus, True)
        'Arbeitsverzeichnis wieder zurücksetzen (optional)
        ChDrive(currentDir)
        ChDir(currentDir)

        'überprüfen, ob Simulation erfolgreich
        '-------------------------------------
        If (File.Exists(WorkDir & Datensatz & ".FEL")) Then

            'Simulationsfehler aufgetreten
            Dim DateiInhalt As String = ""

            Dim FiStr As FileStream = New FileStream(WorkDir & Datensatz & ".FEL", FileMode.Open, IO.FileAccess.Read)
            Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

            Do
                DateiInhalt = DateiInhalt & Chr(13) & Chr(10) & StrRead.ReadLine.ToString
            Loop Until StrRead.Peek() = -1

            Throw New Exception("SMUSI hat einen Fehler zurückgegeben:" & Chr(13) & Chr(10) & DateiInhalt)

        End If

    End Sub

    'Qualitätswert aus WEL-Datei
    '***************************
    Protected Overrides Function QWert_WEL(ByVal OptZiel As OptZiel) As Double

        Dim IsOK As Boolean
        Dim QWert As Double
        Dim SimReihe(,) As Object = {}

        'Simulationsergebnis auslesen
        Dim datei As String = OptZiel.SimGr.Substring(0, 4) & "_WEL.ASC"
        IsOK = Read_WEL(WorkDir & datei, OptZiel.SimGr, SimReihe)

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


#End Region 'Methoden

End Class
