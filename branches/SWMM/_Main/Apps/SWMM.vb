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

#Region "Eigenschaften"

    'Eigenschaften
    '#############

#End Region 'Eigenschaften


#Region "Methoden"

    Public Sub New()

    End Sub


    Public Overrides Function launchSim() As Boolean

        'Aktuelles Verzeichnis bestimmen
        Dim currentDir As String = CurDir()
        Dim InpDatei As String, RptDatei As String, DatDatei As String
        'zum Arbeitsverzeichnis wechseln
        ChDrive(Me.WorkDir)
        ChDir(Me.WorkDir)
        'dll aufrufen
        InpDatei = Me.WorkDir & Me.Datensatz & ".inp"
        RptDatei = Me.WorkDir & Me.Datensatz & ".rpt"
        DatDatei = Me.WorkDir & Me.Datensatz & ".dat"
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

    Protected Overrides Sub Read_Kombinatorik()

    End Sub

    Protected Overrides Sub Read_SimParameter()

        Dim SimStartDay_str As String = ""
        Dim SimStartHour_str As String = ""
        Dim SimEndeDay_str As String = ""
        Dim SimEndeHour_str As String = ""

        'INP-Datei öffnen
        '----------------
        Dim Datei As String = Me.WorkDir & Me.Datensatz & ".INP"

        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        'Alle Zeilen durchlaufen
        Dim Zeile As String
        Do
            Zeile = StrRead.ReadLine.ToString()

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

        Loop Until StrRead.Peek() = -1

        'SimStart und SimEnde in echtes Datum konvertieren
        Me.SimStart = New DateTime(SimStartDay_str.Substring(6, 4), SimStartDay_str.Substring(0, 2), SimStartDay_str.Substring(3, 2), SimStartHour_str.Substring(0, 2), SimStartHour_str.Substring(3, 2), 0)
        Me.SimEnde = New DateTime(SimEndeDay_str.Substring(6, 4), SimEndeDay_str.Substring(0, 2), SimEndeDay_str.Substring(3, 2), SimEndeHour_str.Substring(0, 2), SimEndeHour_str.Substring(3, 2), 0)

        'Zeitschrittweite ist immer 1 Minute
        Me.SimDT = New TimeSpan(0, 1, 0)

    End Sub

    

    Protected Overrides Sub Read_Verzweigungen()

    End Sub

    Protected Overrides Sub Write_Verzweigungen()

    End Sub

    'In der ersten Version wurden die Optimierungsparameter in einer eigenen Datei vorgehalten
    'Später wurde auf das bereits bestehende System mit drei Dateien umgestellt

    'Protected Overrides Sub Read_OptZiele()
    '    'Zeile automatische durch .net eingefügt, nehme ich erstmal raus
    '    'MyBase.Read_OptZiele() 

    '    Dim AnzZiele As Integer = 0
    '    Dim i As Integer

    '    'Gehe erstmal davon aus, dass Optimierungsparameter bei SWMM in einer Datei *.opt stehen
    '    Dim Datei As String = WorkDir & Datensatz & "." & OptParameter_Ext
    '    Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)

    '    Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

    '    Dim Zeile As String = ""

    '    'Anzahl der Zielfunktionen feststellen
    '    Do
    '        Zeile = StrRead.ReadLine.ToString()
    '        If Zeile = "[Optimierungsziele]" Then
    '            Do
    '                Zeile = StrRead.ReadLine.ToString()
    '                AnzZiele = AnzZiele + 1
    '            Loop Until Zeile = "[Optimierungsparameter]"
    '            AnzZiele = AnzZiele - 4
    '        End If
    '    Loop Until StrRead.Peek() = -1

    '    If (AnzZiele > 3) Then
    '        MsgBox("Die Anzahl der Ziele beträgt mehr als 3!" & Chr(13) & Chr(10) _
    '                & "Es werden nur die ersten drei Zielfunktionen im Hauptdiagramm angezeigt!", MsgBoxStyle.Information, "Info")
    '    End If

    '    ReDim List_OptZiele(AnzZiele - 1)

    '    'Zurück zum Dateianfang und lesen
    '    FiStr.Seek(0, SeekOrigin.Begin)

    '    Do
    '        Zeile = StrRead.ReadLine.ToString()
    '        If Zeile = "[Optimierungsziele]" Then
    '            'Erst nochmal zwei Zeilen vorschieben
    '            Zeile = StrRead.ReadLine.ToString()
    '            Zeile = StrRead.ReadLine.ToString()
    '            For i = 0 To AnzZiele - 1
    '                Zeile = StrRead.ReadLine.ToString()
    '                List_OptZiele(i).Bezeichnung = Trim(Zeile.Substring(0, 19))
    '                List_OptZiele(i).ZielTyp = Trim(Zeile.Substring(19, 11))
    '                List_OptZiele(i).Datei = Trim(Zeile.Substring(30, 20))
    '                List_OptZiele(i).SimGr = Trim(Zeile.Substring(50, 11))
    '                List_OptZiele(i).ZielFkt = Trim(Zeile.Substring(61, 11))
    '                List_OptZiele(i).WertTyp = Trim(Zeile.Substring(72, 11))
    '                List_OptZiele(i).ZielWert = Trim(Zeile.Substring(83, 11))
    '            Next
    '            Exit Do
    '        End If
    '    Loop

    '    StrRead.Close()
    '    FiStr.Close()

    'End Sub


    'In der ersten Version wurden die Optimierungsparameter in einer eigenen Datei vorgehalten
    'Später wurde auf das bereits bestehende System mit drei Dateien umgestellt

    ''Optimierungsparameter einlesen
    ''******************************
    'Protected Overrides Sub Read_OptParameter()

    '    Dim Datei As String = WorkDir & Datensatz & "." & OptParameter_Ext

    '    Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
    '    Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

    '    Dim Zeile As String
    '    Dim AnzParam As Integer = 0

    '    'Anzahl der Parameter feststellen
    '    Do
    '        Zeile = StrRead.ReadLine.ToString()
    '        If Zeile = "[Optimierungsparameter]" Then
    '            Do
    '                Zeile = StrRead.ReadLine.ToString()
    '                AnzParam = AnzParam + 1
    '            Loop Until Zeile = "[Modellparameter]"
    '            AnzParam = AnzParam - 4
    '        End If
    '    Loop Until StrRead.Peek() = -1


    '    ReDim List_OptParameter(AnzParam - 1)
    '    ReDim List_OptParameter_Save(AnzParam - 1)

    '    'Zurück zum Dateianfang und lesen
    '    FiStr.Seek(0, SeekOrigin.Begin)
    '    Dim i As Integer = 0

    '    Do
    '        Zeile = StrRead.ReadLine.ToString()
    '        If Zeile = "[Optimierungsparameter]" Then
    '            'Erst nochmal zwei Zeilen vorschieben
    '            Zeile = StrRead.ReadLine.ToString()
    '            Zeile = StrRead.ReadLine.ToString()
    '            For i = 0 To AnzParam - 1
    '                Zeile = StrRead.ReadLine.ToString()
    '                List_OptParameter(i).Bezeichnung = Trim(Zeile.Substring(0, 19))
    '                List_OptParameter(i).Einheit = Trim(Zeile.Substring(19, 11))
    '                List_OptParameter(i).Wert = Trim(Zeile.Substring(30, 12))
    '                List_OptParameter(i).Min = Trim(Zeile.Substring(42, 11))
    '                List_OptParameter(i).Max = Trim(Zeile.Substring(53, 5))
    '            Next
    '            Exit Do
    '        End If
    '    Loop

    '    StrRead.Close()
    '    FiStr.Close()

    '    'OptParameter werden hier gesichert
    '    For i = 0 To List_OptParameter.GetUpperBound(0)
    '        Call copy_Struct_OptParameter(List_OptParameter(i), List_OptParameter_Save(i))
    '    Next

    'End Sub


    'In der ersten Version wurden die Optimierungsparameter in einer eigenen Datei vorgehalten
    'Später wurde auf das bereits bestehende System mit drei Dateien umgestellt

    'Protected Overrides Sub Read_ModellParameter()

    '    Dim Datei As String = WorkDir & Datensatz & "." & OptParameter_Ext

    '    Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
    '    Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

    '    Dim Zeile As String
    '    Dim AnzParam As Integer = 0

    '    'Anzahl der Parameter feststellen
    '    Do
    '        Zeile = StrRead.ReadLine.ToString()
    '        If Zeile = "[Modellparameter]" Then
    '            Do
    '                Zeile = StrRead.ReadLine.ToString()
    '                AnzParam = AnzParam + 1
    '            Loop Until Zeile = "[Ende]"
    '            AnzParam = AnzParam - 4
    '        End If
    '    Loop Until StrRead.Peek() = -1

    '    ReDim List_ModellParameter(AnzParam - 1)
    '    ReDim List_ModellParameter_Save(AnzParam - 1)

    '    'Zurück zum Dateianfang und lesen
    '    FiStr.Seek(0, SeekOrigin.Begin)

    '    'Zurück zum Dateianfang und lesen
    '    FiStr.Seek(0, SeekOrigin.Begin)
    '    Dim i As Integer = 0

    '    Do
    '        Zeile = StrRead.ReadLine.ToString()
    '        If Zeile = "[Modellparameter]" Then
    '            'Erst nochmal zwei Zeilen vorschieben
    '            Zeile = StrRead.ReadLine.ToString()
    '            Zeile = StrRead.ReadLine.ToString()
    '            For i = 0 To AnzParam - 1
    '                Zeile = StrRead.ReadLine.ToString()
    '                'Substring-Positionen müssen noch geprüft werden
    '                List_ModellParameter(i).OptParameter = Trim(Zeile.Substring(0, 19))
    '                List_ModellParameter(i).Bezeichnung = Trim(Zeile.Substring(19, 15))
    '                List_ModellParameter(i).Einheit = Trim(Zeile.Substring(34, 10))
    '                List_ModellParameter(i).Datei = Trim(Zeile.Substring(44, 10))
    '                List_ModellParameter(i).Element = Trim(Zeile.Substring(44, 10))
    '                List_ModellParameter(i).ZeileNr = Trim(Zeile.Substring(54, 10))
    '                List_ModellParameter(i).SpVon = Trim(Zeile.Substring(64, 10))
    '                List_ModellParameter(i).SpBis = Trim(Zeile.Substring(74, 9))
    '                List_ModellParameter(i).Faktor = Trim(Zeile.Substring(83, 5))
    '            Next
    '            Exit Do
    '        End If
    '    Loop

    '    StrRead.Close()
    '    FiStr.Close()

    '    'ModellParameter werden hier gesichert
    '    For i = 0 To List_ModellParameter.GetUpperBound(0)
    '        Call copy_Struct_ModellParameter(List_ModellParameter(i), List_ModellParameter_Save(i))
    '    Next

    'End Sub

    'Berechnung des Qualitätswerts (Zielwert)
    '****************************************
    Public Overrides Function QWert(ByVal OptZiel As Struct_OptZiel) As Double

        QWert = 0

        Dim IsOK As Boolean

        'Fallunterscheidung Ergebnisdatei
        '--------------------------------
        Select Case OptZiel.Datei

            Case "RPT"
                'SWMM-Ergebnisse aus RPT-Datei auslesen
                QWert = QWert_RPT(OptZiel)

            Case Else
                'es wurde eine nicht unterstützte Ergebnisdatei angegeben
                IsOK = False

        End Select

        If (IsOK = False) Then
            'TODO: Fehlerbehandlung
        End If

    End Function

    Public Function QWert_RPT(ByVal OptZiel As Struct_OptZiel) As Double

        Dim QWert As Double
        Dim DateiPfad As String
        Dim Zeile As String

        DateiPfad = WorkDir & Datensatz & ".RPT"

        'RPT-Datei öffnen
        Dim FiStr As FileStream = New FileStream(DateiPfad, FileMode.Open, IO.FileAccess.Read)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        'Richtige Zeile ansteuern und Wert auslesen
        Do
            Zeile = StrRead.ReadLine.ToString
            If (Zeile.StartsWith("  External Outflow")) Then
                QWert = Trim(Zeile.Substring(49, 7))
                Exit Do
            End If
        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()
        Return QWert

    End Function


    'Kopiert ein Strukt_OptParameter
    '**********************************
    Private Sub copy_Struct_OptParameter(ByVal Source As Struct_OptParameter, ByRef Destination As Struct_OptParameter)

        Destination.Bezeichnung = Source.Bezeichnung
        Destination.Einheit = Source.Einheit
        Destination.Wert = Source.Wert
        Destination.Min = Source.Min
        Destination.Max = Source.Max
        Destination.SKWert = Source.SKWert

    End Sub

    'Kopiert ein Strukt_ModellParameter
    '**********************************
    Private Sub copy_Struct_ModellParameter(ByVal Source As Struct_ModellParameter, ByRef Destination As Struct_ModellParameter)

        Destination.OptParameter = Source.OptParameter
        Destination.Bezeichnung = Source.Bezeichnung
        Destination.Einheit = Source.Einheit
        Destination.Datei = Source.Datei
        Destination.Element = Source.Element
        Destination.ZeileNr = Source.ZeileNr
        Destination.SpVon = Source.SpVon
        Destination.SpBis = Source.SpBis
        Destination.Faktor = Source.Faktor
        Destination.Wert = Source.Wert

    End Sub

#End Region


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
