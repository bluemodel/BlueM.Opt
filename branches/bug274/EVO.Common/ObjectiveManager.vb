Imports System.IO

Public Class ObjectiveManager

    Private SimStart, SimEnde As DateTime

    Public List_OptZiele() As OptZiel          'Liste der Zielfunktionen
    Public List_SekZiele() As OptZiel          'Liste der Sekundären Zielfunktionen

    'Konstruktor
    '***********
    Public Sub New(ByVal start As DateTime, ByVal ende As DateTime)

        ReDim Me.List_OptZiele(-1)
        ReDim Me.List_SekZiele(-1)

        Me.SimStart = start
        Me.SimEnde = ende

    End Sub


    'ZIE-Datei einlesen
    '******************
    Public Sub Read_ZIE(ByVal ZIE_Datei As String)

        'Format:
        '*|------|---------------|---------|-------|----------|---------|--------------|--------------------|---------------------------|
        '*| Opt  | Bezeichnung   | ZielTyp | Datei | SimGröße | ZielFkt | EvalZeitraum |       Zielwert   ODER      Zielreihe           |
        '*|      |               |         |       |          |         | Start | Ende | WertTyp | ZielWert | ZielGröße | Datei         |
        '*|------|---------------|---------|-------|----------|---------|-------|------|---------|----------|-----------|---------------|

        Const AnzSpalten As Integer = 12                       'Anzahl Spalten in der ZIE-Datei
        Dim WorkDir As String
        Dim tmpZiele() As OptZiel
        Dim i As Integer
        Dim Zeile As String
        Dim WerteArray() As String

        ReDim tmpZiele(-1)

        'Arbeitsverzeichnis bestimmen
        WorkDir = Path.GetDirectoryName(ZIE_Datei) & "\"

        'Einlesen aller Ziele und Speichern in tmpZiele
        '##############################################
        Dim FiStr As FileStream = New FileStream(ZIE_Datei, FileMode.Open, IO.FileAccess.ReadWrite)

        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        i = 0
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False And Zeile.Contains("|")) Then
                WerteArray = Zeile.Split("|")
                'Kontrolle
                If (WerteArray.GetUpperBound(0) <> AnzSpalten + 1) Then
                    Throw New Exception("Die ZIE-Datei hat die falsche Anzahl Spalten!")
                End If
                'Neues Ziel anlegen
                ReDim Preserve tmpZiele(i)
                tmpZiele(i) = New OptZiel()
                'Werte einlesen
                With tmpZiele(i)
                    If (WerteArray(1).Trim().ToUpper() = "J") Then
                        .isOpt = True
                    Else
                        .isOpt = False
                    End If
                    .Bezeichnung = WerteArray(2).Trim()
                    .ZielTyp = WerteArray(3).Trim()
                    .Datei = WerteArray(4).Trim()
                    .SimGr = WerteArray(5).Trim()
                    .ZielFkt = WerteArray(6).Trim()
                    If (WerteArray(7).Trim() <> "") Then
                        .EvalStart = WerteArray(7).Trim()
                    Else
                        .EvalStart = Me.SimStart
                    End If
                    If WerteArray(8).Trim() <> "" Then
                        .EvalEnde = WerteArray(8).Trim()
                    Else
                        .EvalEnde = Me.SimEnde
                    End If
                    .WertTyp = WerteArray(9).Trim()
                    If (WerteArray(10).Trim() <> "") Then .ZielWert = Convert.ToDouble(WerteArray(9).Trim(), Provider.FortranProvider)
                    .ZielGr = WerteArray(11).Trim()
                    .ZielReiheDatei = WerteArray(12).Trim()
                End With
                i += 1
            End If
        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

        'Bei ZielReihen Reihen einlesen
        '##############################
        Dim ZielStart As Date
        Dim ZielEnde As Date
        Dim ext As String

        For i = 0 To tmpZiele.GetUpperBound(0)
            With tmpZiele(i)
                If (.ZielTyp = "Reihe" Or .ZielTyp = "IHA") Then

                    'Dateiendung der Zielreihendatei bestimmen und Reihe einlesen
                    '------------------------------------------------------------
                    ext = Path.GetExtension(.ZielReiheDatei)
                    Select Case (ext.ToUpper)
                        Case ".WEL"
                            Dim WEL As New Wave.WEL(WorkDir & .ZielReiheDatei, True)
                            .ZielReihe = WEL.getReihe(.ZielGr)
                        Case ".ASC"
                            Dim ASC As New Wave.ASC(WorkDir & .ZielReiheDatei, True)
                            .ZielReihe = ASC.getReihe(.ZielGr)
                        Case ".ZRE"
                            Dim ZRE As New Wave.ZRE(WorkDir & .ZielReiheDatei, True)
                            .ZielReihe = ZRE.Zeitreihen(0)
                            'Case ".PRB"
                            'BUG 183: geht nicht mehr, weil PRB-Dateien keine Zeitreihen sind!
                            'IsOK = Read_PRB(Me.WorkDir & .ZielReiheDatei, .ZielGr, .ZielReihe)
                        Case Else
                            Throw New Exception("Das Format der Zielreihe '" & .ZielReiheDatei & "' wird nicht unterstützt!")
                    End Select

                    'Zeitraum der Zielreihe überprüfen (nur bei WEL und ZRE)
                    '-------------------------------------------------------
                    If (ext.ToUpper = ".WEL" Or ext.ToUpper = ".ZRE" Or ext.ToUpper = ".ASC") Then

                        ZielStart = .ZielReihe.XWerte(0)
                        ZielEnde = .ZielReihe.XWerte(.ZielReihe.Length - 1)

                        If (ZielStart > .EvalStart Or ZielEnde < .EvalEnde) Then
                            'Zielreihe deckt Evaluierungszeitraum nicht ab
                            Throw New Exception("Die Zielreihe '" & .ZielReiheDatei & "' deckt den Evaluierungszeitraum nicht ab!")
                        Else
                            'Zielreihe auf Evaluierungszeitraum kürzen
                            Call .ZielReihe.Cut(.EvalStart, .EvalEnde)
                        End If

                    End If

                    'Zielreihe umbenennen
                    .ZielReihe.Title += " (Referenz)"

                End If
            End With
        Next

        'Aufteilen der tmpZiele in OptZiele und SekZiele
        '###############################################
        For i = 0 To tmpZiele.GetUpperBound(0)
            If (tmpZiele(i).isOpt) Then
                'OptZiel
                '-------
                'Liste um 1 erweitern
                ReDim Preserve Me.List_OptZiele(Me.List_OptZiele.GetUpperBound(0) + 1)
                'Ziel kopieren
                Me.List_OptZiele(Me.List_OptZiele.GetUpperBound(0)) = tmpZiele(i)
            Else
                'SekZiel
                '-------
                'Liste um 1 erweitern
                ReDim Preserve Me.List_SekZiele(Me.List_SekZiele.GetUpperBound(0) + 1)
                'Ziel kopieren
                Me.List_SekZiele(Me.List_SekZiele.GetUpperBound(0)) = tmpZiele(i)
            End If
        Next i


    End Sub
End Class
