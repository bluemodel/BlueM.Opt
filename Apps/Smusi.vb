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
    Protected Overrides Sub Read_SimParameter()

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

        'Zeitschrittweite ist immer 5 Minuten
        Me.SimDT = New TimeSpan(0, 5, 0)

    End Sub

    'SMUSI ausführen (simulieren)
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


#Region "Kombinatorik"

    'Kombinatorik
    '############

    'Kombinatorik einlesen
    '*********************
    Protected Overrides Sub Read_Kombinatorik()

    End Sub

    'Validierungsfunktion der Kombinatorik Prüft ob Verbraucher an zwei Standorten Dopp vorhanden sind
    '*************************************************************************************************
    Public Overrides Sub Combinatoric_is_Valid()

    End Sub

    'Liest die Verzweigungen aus dem BModel in ein Array ein
    'Und Dimensioniert das Verzweigungsarray
    '*******************************************************
    Protected Overrides Sub Read_Verzweigungen()

    End Sub

    'Mehrere Prüfungen ob die .VER Datei des BlueM und der .CES Datei auch zusammenpassen
    '************************************************************************************
    Public Overrides Sub CES_fits_to_VER()

    End Sub

    'Die Liste mit den aktuellen Bauwerken des Kindes wird erstellt und in SKos geschrieben
    '**************************************************************************************
    Public Overrides Sub Define_aktuelle_Bauwerke(ByVal Path() As Integer)

    End Sub

    'Ermittelt das aktuelle Verzweigungsarray
    '****************************************
    Public Overrides Sub Verzweigung_ON_OFF(ByVal Path() As Integer)

    End Sub

    'Schreibt die neuen Verzweigungen
    '********************************
    Public Overrides Sub Write_Verzweigungen()

    End Sub

#End Region 'Kombinatorik

#End Region 'Methoden

End Class
