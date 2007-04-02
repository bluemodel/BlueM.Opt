Imports System.IO

Public Class IHA

    'Einschränkungen:
    '--------------------------------------------
    'dt = 1 Tag (24h)
    'nur eine IHA-Zielfunktion erlaubt
    'IHA-Vergleich zweier Zeitreihen (Perioden)
    'Hydrologisches Jahr fäüngt immer am 1.Okt an
    '--------------------------------------------

#Region "Eigenschaften"

    '##########################################################
    'Eigenschaften
    '##########################################################

    Private Const BegWatrYr = 275           '1. Okt.

    Private BeginPre As Integer
    Private EndPre As Integer
    Private BeginPost As Integer
    Private EndPost As Integer

    Private Data(,) As Double             'Enthält die Abflussdaten in Zeilen für jeden Tag des Jahres

#End Region 'Eigenschaften

#Region "Methoden"

    '##########################################################
    'Methoden
    '##########################################################

    '**********************************************************
    'IHA-Berechnung vorbereiten (einmalig)
    '**********************************************************
    Public Sub IHA_prepare(ByRef BlueM1 As BlueM)

        Dim IHADir As String = BlueM1.WorkDir & "IHA\"

        'IHA-Unterverzeichnis anlegen
        '----------------------------
        If (Directory.Exists(IHADir) = False) Then
            Directory.CreateDirectory(IHADir)
        End If

        Dim i As Integer

        'Datume bestimmen
        '-----------------------------
        'Referenz-Zeitreihe raussuchen
        Dim ZeitReihe(,) As Object = {}
        For i = 0 To BlueM1.OptZieleListe.GetUpperBound(0)
            If (BlueM1.OptZieleListe(i).ZielFkt = "IHA") Then
                ZeitReihe = BlueM1.OptZieleListe(i).ZielReihe
                Exit For
            End If
        Next

        'BeginPre
        Dim StartDatum As DateTime = ZeitReihe(0, 0)
        If (StartDatum.DayOfYear > BegWatrYr) Then
            BeginPre = StartDatum.Year + 1
        Else
            BeginPre = StartDatum.Year
        End If

        'EndPre
        Dim EndDatum As DateTime = ZeitReihe(ZeitReihe.GetUpperBound(0), 0)
        If (EndDatum.DayOfYear > BegWatrYr) Then
            EndPre = EndDatum.Year - 1
        Else
            EndPre = EndDatum.Year - 2
        End If

        'BeginPost und EndPost
        BeginPost = EndPre + 1
        EndPost = BeginPost + (EndPre - BeginPre)

        'Referenz-Zeitreihe entsprechend kürzen
        Dim cutLength As Integer = (New DateTime(EndPre + 1, 9, 30) - New DateTime(BeginPre, 10, 1)).Days + 1
        Dim cutBegin As Integer = (New DateTime(BeginPre, 10, 1) - StartDatum).Days
        Dim RefReihe(cutLength - 1, 1) As Object
        Array.Copy(ZeitReihe, cutBegin * 2, RefReihe, 0, cutlength * 2)

        'PAR-Datei anlegen
        '-----------------
        Try
            Dim StrWrite As StreamWriter = New StreamWriter(IHADir & "input.par", False, System.Text.Encoding.GetEncoding("iso8859-1"))
            StrWrite.WriteLine("&initial")
            StrWrite.WriteLine("infile=" & IHADir & "input.dat")
            StrWrite.WriteLine("outscore=" & IHADir & "output.sco")
            StrWrite.WriteLine("outpct=" & IHADir & "output.pct")
            StrWrite.WriteLine("outsum=" & IHADir & "output.ann")
            StrWrite.WriteLine("outRVA=" & IHADir & "output.rva")
            StrWrite.WriteLine("outBAW=" & IHADir & "output.baw")
            StrWrite.WriteLine("outLsq=" & IHADir & "output.lsq")
            StrWrite.WriteLine("outMsg=" & IHADir & "output.msg")
            StrWrite.WriteLine("BigTitle='" & BlueM1.Datensatz & "'")
            StrWrite.WriteLine("TwoPeriods=T")
            StrWrite.WriteLine("ImpactYear=" & BeginPost)
            StrWrite.WriteLine("BeginPre=" & BeginPre)
            StrWrite.WriteLine("EndPre=" & EndPre)
            StrWrite.WriteLine("BeginPost=" & BeginPost)
            StrWrite.WriteLine("EndPost=" & EndPost)
            StrWrite.WriteLine("HiPulseLvl=25")
            StrWrite.WriteLine("HiRVAlim=17")
            StrWrite.WriteLine("LoPulseLvl=25")
            StrWrite.WriteLine("LoRVALim=17")
            StrWrite.WriteLine("Parametric=F")
            StrWrite.WriteLine("BegDay=1")
            StrWrite.WriteLine("EndDay=366")
            StrWrite.WriteLine("Watershed=1")
            StrWrite.WriteLine("Normalize=1")
            StrWrite.WriteLine("BegWatrYr= " & BegWatrYr)
            StrWrite.WriteLine("Nmissing=10")
            StrWrite.WriteLine("Rmissing=999999")
            StrWrite.WriteLine("MKS=T")
            StrWrite.WriteLine("DroughtLvl=10")
            StrWrite.WriteLine("BeginFloodRate=25")
            StrWrite.WriteLine("BeginFloodLevel=75")
            StrWrite.WriteLine("EndFloodRate=10")
            StrWrite.WriteLine("EndFloodLevel=50")
            StrWrite.WriteLine("FloodOne=2")
            StrWrite.WriteLine("FloodTwo=10")
            StrWrite.WriteLine("BegSeasonOne=" & BegWatrYr)
            StrWrite.WriteLine("EndSeasonOne=" & BegWatrYr - 1)
            StrWrite.WriteLine("BegSeasonTwo=0")
            StrWrite.WriteLine("EndSeasonTwo=0")
            StrWrite.WriteLine("/")

            StrWrite.Close()

        Catch except As Exception
            MsgBox("Fehler beim Schreiben der IHA-Datei ""input.par""" & Chr(13) & Chr(10) & except.Message, MsgBoxStyle.Exclamation, "Fehler")
            Exit Sub
        End Try

        'Referenz-Zeitreihe umformatieren und in Data(,) speichern
        '---------------------------------------------------------

        'Data(,) mit Anzahl Jahre (Spalten) redimensionieren 
        Dim AnzJahre As Integer = EndPre - BeginPre
        ReDim Data(366, AnzJahre)

        'Erste Zeile mit Jahreszahlen schreiben
        Dim Jahr As Integer = BeginPre
        For i = 0 To AnzJahre
            Data(0, i) = Jahr
            Jahr += 1
        Next

        'Weitere Zeilen mit Werten füllen
        Dim j, k As Integer
        i = 0
        k = 0
        While (i <= RefReihe.GetUpperBound(0))
            For j = 1 To 366
                Data(j, k) = RefReihe(i, 1)
                'bei Nicht-Schaltjahren nach dem 28.Feb. einen Wert auffüllen
                If (RefReihe(i, 0).DayOfYear = 59 And DateTime.IsLeapYear(RefReihe(i, 0).Year) = False) Then
                    j += 1
                    Data(j, k) = -999999
                End If
                i += 1
            Next
            k += 1
        End While

    End Sub

    '**********************************************************
    'IHA-Berechnung ausführen (bei jeder QWert-Berechnung)
    '**********************************************************
    Public Sub calculate_IHA(ByVal zeitreihe As Object(,))


    End Sub

#End Region 'Methoden

End Class
