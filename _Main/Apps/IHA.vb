Imports System.IO

Public Class IHA

    '***************************************************************************************
    '***************************************************************************************
    '**** Klasse IHA zur Ermittlung der Hydrologischen Ver�nderung                      ****
    '**** mittels "IHA Software" v7.03 von The Nature Conservancy                       ****
    '**** http://www.nature.org/initiatives/freshwater/conservationtools/art17004.html  ****
    '****                                                                               ****
    '**** Felix Froehlich                                                               ****
    '****                                                                               ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung                      ****
    '**** TU Darmstadt                                                                  ****
    '****                                                                               ****
    '**** Erstellt: April 2007                                                          ****
    '****                                                                               ****
    '**** Letzte �nderung: April 2007                                                   ****
    '****                                                                               ****
    '**** Einschr�nkungen:                                                              ****
    '**** ----------------                                                              ****
    '**** Zeitreihen dt = 1 Tag (24h)                                                   ****
    '**** nur eine einzige IHA-Zielfunktion erlaubt                                     ****
    '**** Vergleich der Simulationszeitreihe mit einer Referenzzeitreihe                ****
    '**** Hydrologisches Jahr f�ngt immer am 1. Okt an                                  ****
    '****                                                                               ****
    '**** Hinweise:                                                                     ****
    '**** ---------                                                                     ****
    '**** bei Jahreszahlen gilt: 1994 entspricht Zeitraum 1.10.1993 bis 30.09.1994      ****
    '****                                                                               ****
    '***************************************************************************************
    '***************************************************************************************

#Region "Eigenschaften"

    'Eigenschaften
    '#############

    Private IHAZiel As Sim.Struct_OptZiel           'Kopie des OptZiels mit ZielTyp "IHA"
    Private IHADir As String                        'Verzeichnis f�r IHA-Dateien

    'IHA-Ergebnisse
    '--------------
    Private Structure IHAParam                      'Struktur f�r IHA Ergebnis eines Parameters
        Public PName As String                      'Parametername
        Public HAMiddle As Double                   'Hydrologic Alteration (HA) - Middle RVA Category
    End Structure

    Private Structure IHAParamGroup                 'Struktur f�r IHA Ergebnis einer Parametergruppe
        Public No As Short                          'Gruppennummer
        Public GName As String                      'Gruppennname
        Public Avg As Double                        'HA-Mittelwert der Gruppe
        Public IHAParams() As IHAParam              'Liste der Paremeter
    End Structure

    Private Structure IHAResult                     'Struktur f�r alle IHA Ergebnisse zusammen
        Public Avg As Double                        'HA-Mittelwert �ber alle Gruppen
        Public IHAParamGroups() As IHAParamGroup    'Liste der Parametergruppen
    End Structure

    Private IHARes As IHAResult

    'IHA-Software-Parameter
    '----------------------
    Private Const BegWatrYr as Integer = 275        '1. Okt.

    'Jahreszahlen
    Private BeginPre As Integer
    Private EndPre As Integer
    Private BeginPost_sim As Integer                'tats�chlich
    Private EndPost_sim As Integer                  'tats�chlich
    Private BeginPost As Integer                    'f�r IHA verwendet
    Private EndPost As Integer                      'f�r IHA verwendet

    Private RefData(,) As Double                    'Enth�lt die Referenz-Abflussdaten in Zeilen f�r jeden Tag des Jahres

#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

    'Konstruktor
    '***********
    Public Sub New(ByVal OptZiel as Sim.Struct_OptZiel)

        'IHAZiel kopieren
        '----------------
        Me.IHAZiel = OptZiel

        'IHA-Parametergruppen definieren
        '-------------------------------
        With Me.IHARes

            ReDim .IHAParamGroups(4)

            'Parameter group #1
            .IHAParamGroups(0).No = 1
            .IHAParamGroups(0).GName = "Quantity"
            ReDim .IHAParamGroups(0).IHAParams(11)

            'Parameter group #2
            .IHAParamGroups(1).No = 2
            .IHAParamGroups(1).GName = "Extremes"
            ReDim .IHAParamGroups(1).IHAParams(11)

            'Parameter group #3
            .IHAParamGroups(2).No = 3
            .IHAParamGroups(2).GName = "Timing"
            ReDim .IHAParamGroups(2).IHAParams(1)

            'Parameter group #4
            .IHAParamGroups(3).No = 4
            .IHAParamGroups(3).GName = "Frequency"
            ReDim .IHAParamGroups(3).IHAParams(3)

            'Parameter group #5
            .IHAParamGroups(4).No = 5
            .IHAParamGroups(4).GName = "Rate"
            ReDim .IHAParamGroups(4).IHAParams(2)

        End With

    End Sub

    'IHA-Berechnung vorbereiten (einmalig)
    '*************************************
    Public Sub prepare_IHA(ByRef BlueM1 As BlueM)

        Dim i, j, k As Integer

        'IHA-Unterverzeichnis anlegen
        '----------------------------
        Me.IHADir = BlueM1.WorkDir & "IHA\"

        If (Directory.Exists(Me.IHADir) = False) Then
            Directory.CreateDirectory(Me.IHADir)
            'IHA_Batchfor.exe in Verzeichnis kopieren
            Dim ZielDatei As String = Me.IHADir & "IHA_Batchfor.exe"

            'aktuelles Verzeichnis bestimmen
            Dim currentDir As String = CurDir()
            'Pfad zur Assembly bestimmen (\_Main\bin\)
            Dim binpath As String = System.Windows.Forms.Application.StartupPath()
            'in das \_Main\bin Verzeichnis wechseln
            ChDrive(binpath)
            ChDir(binpath)
            'in das \Apps Verzeichnis wechseln
            ChDir("..\Apps")
            'Datei kopieren
            My.Computer.FileSystem.CopyFile("IHA_Batchfor.exe", ZielDatei, True)
            'zur�ck in das Ausgangsverzeichnis wechseln
            ChDrive(currentDir)
            ChDir(currentDir)

        End If

        'Datume bestimmen
        '-----------------------------
        'Referenz-Zeitreihe raussuchen
        Dim RefReihe As New Wave.Zeitreihe("")
        RefReihe = IHAZiel.ZielReihe.copy()

        'BeginPre
        Dim StartDatum As DateTime = RefReihe.XWerte(0)
        If (StartDatum.DayOfYear > BegWatrYr) Then
            Me.BeginPre = StartDatum.Year + 2
        Else
            Me.BeginPre = StartDatum.Year + 1
        End If

        'EndPre
        Dim EndDatum As DateTime = RefReihe.XWerte(RefReihe.Length - 1)
        If (EndDatum.DayOfYear >= BegWatrYr) Then
            Me.EndPre = EndDatum.Year
        Else
            Me.EndPre = EndDatum.Year - 1
        End If

        'BeginPost
        If (BlueM1.SimStart.DayOfYear > BegWatrYr) Then
            Me.BeginPost_sim = BlueM1.SimStart.Year + 2
        Else
            Me.BeginPost_sim = BlueM1.SimStart.Year + 1
        End If
        Me.BeginPost = Me.EndPre + 1 '(Post-Zeitraum wird an das Ende des Pre-Zeitraums angeh�ngt!)

        'EndPost
        If (BlueM1.SimEnde.DayOfYear >= BegWatrYr) Then
            Me.EndPost_sim = BlueM1.SimEnde.Year
        Else
            Me.EndPost_sim = BlueM1.SimEnde.Year - 1
        End If
        Me.EndPost = Me.BeginPost + (Me.EndPost_sim - Me.BeginPost_sim)

        'Zeitreihe k�rzen
        '----------------
        Call RefReihe.cut(New DateTime(Me.BeginPre - 1, 10, 1), New DateTime(Me.EndPre, 9, 30))

        'Referenz-Zeitreihe umformatieren und in RefData(,) speichern
        '---------------------------------------------------------
        'RefData(,) mit Anzahl Jahren (Spalten) redimensionieren 
        Dim AnzJahre As Integer = Me.EndPre - Me.BeginPre + 1
        ReDim Me.RefData(366, AnzJahre - 1)

        'Erste Zeile mit Jahreszahlen
        Dim Jahr As Integer = Me.BeginPre
        For i = 0 To AnzJahre - 1
            Me.RefData(0, i) = Jahr
            Jahr += 1
        Next

        'Weitere Zeilen mit Werten f�llen
        i = 0
        k = 0
        While (i <= RefReihe.Length - 1)
            For j = 1 To 366
                Me.RefData(j, k) = RefReihe.YWerte(i)
                'bei Nicht-Schaltjahren nach dem 28.Feb. einen Wert auff�llen
                If (RefReihe.XWerte(i).DayOfYear = 59 And DateTime.IsLeapYear(RefReihe.XWerte(i).Year) = False) Then
                    j += 1
                    Me.RefData(j, k) = -999999
                End If
                i += 1
            Next
            k += 1
        End While

        'input.par Datei schreiben
        '-------------------------
        Call write_InputPar(BlueM1.Datensatz)

    End Sub

    'input.par Datei schreiben
    '*************************
    Private Sub write_InputPar(ByVal title As String)

        Dim StrWrite As StreamWriter = New StreamWriter(Me.IHADir & "input.par", False, System.Text.Encoding.GetEncoding("iso8859-1"))
        StrWrite.WriteLine("&initial")
        StrWrite.WriteLine("infile=" & Me.IHADir & "input.dat")
        StrWrite.WriteLine("outscore=" & Me.IHADir & "output.sco")
        StrWrite.WriteLine("outpct=" & Me.IHADir & "output.pct")
        StrWrite.WriteLine("outsum=" & Me.IHADir & "output.ann")
        StrWrite.WriteLine("outRVA=" & Me.IHADir & "output.rva")
        StrWrite.WriteLine("outBAW=" & Me.IHADir & "output.baw")
        StrWrite.WriteLine("outLsq=" & Me.IHADir & "output.lsq")
        StrWrite.WriteLine("outMsg=" & Me.IHADir & "output.msg")
        StrWrite.WriteLine("BigTitle='" & title & "'")
        StrWrite.WriteLine("TwoPeriods=T")
        StrWrite.WriteLine("ImpactYear=" & Me.BeginPost)
        StrWrite.WriteLine("BeginPre=" & Me.BeginPre)
        StrWrite.WriteLine("EndPre=" & Me.EndPre)
        StrWrite.WriteLine("BeginPost=" & Me.BeginPost)
        StrWrite.WriteLine("EndPost=" & Me.EndPost)
        StrWrite.WriteLine("HiPulseLvl=25")
        StrWrite.WriteLine("HiRVAlim=17")
        StrWrite.WriteLine("LoPulseLvl=25")
        StrWrite.WriteLine("LoRVALim=17")
        StrWrite.WriteLine("Parametric=F")
        StrWrite.WriteLine("BegDay=1")
        StrWrite.WriteLine("EndDay=366")
        StrWrite.WriteLine("Watershed=1")
        StrWrite.WriteLine("Normalize=1")
        StrWrite.WriteLine("BegWatrYr=" & BegWatrYr)
        StrWrite.WriteLine("Nmissing=10")
        StrWrite.WriteLine("Rmissing=999999")
        StrWrite.WriteLine("MKS=F")
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

    End Sub

    'input.dat Datei schreiben
    '*************************
    Private Sub write_InputDat(ByVal Data(,) As Double)
        Dim Zeile As String
        Dim j, k As Integer

        Dim StrWrite As StreamWriter = New StreamWriter(Me.IHADir & "input.dat", False, System.Text.Encoding.GetEncoding("iso8859-1"))

        'Erste Zeile mit Jahreszahlen
        j = 0
        Zeile = ""
        For k = 0 To Data.GetUpperBound(1)
            Zeile &= "              " & Data(j, k).ToString("0000")
        Next
        StrWrite.WriteLine(Zeile)

        'Weitere Zeilen mit Daten
        For j = 1 To Data.GetUpperBound(0)
            Zeile = ""
            For k = 0 To Data.GetUpperBound(1)
                Zeile &= "   " & Data(j, k).ToString(" 0.000000000E+0;-0.000000000E+0")
            Next
            StrWrite.WriteLine(Zeile)
        Next

        StrWrite.Close()

    End Sub

    'IHA-Berechnung ausf�hren
    '************************
    Public Sub calculate_IHA(ByVal WELFile As String)

        Dim i, j, k As Integer

        'Simulationsreihe einlesen
        '-------------------------
        Dim SimReihe As New Wave.Zeitreihe(Me.IHAZiel.SimGr)
        Dim WEL As New Wave.WEL(WELFile, Me.IHAZiel.SimGr)
        SimReihe = WEL.Read_WEL()(0)

        'Simulationsreihe entsprechend k�rzen
        SimReihe.cut(New DateTime(Me.BeginPost_sim - 1, 10, 1), New DateTime(Me.EndPost_sim, 9, 30))

        'Data(,) dimensionieren 
        Dim AnzJahre As Integer = Me.EndPost - Me.BeginPost + 1
        Dim Data(366, AnzJahre - 1) As Double

        'RefData(,) nach Data(,) kopieren
        Array.Copy(Me.RefData, Data, Me.RefData.Length)

        'Data(,) redimensionieren
        Dim AnzJahre_gesamt As Integer = Me.EndPost - Me.BeginPre + 1
        ReDim Preserve Data(366, AnzJahre_gesamt - 1)

        'Data(,) mit Werten aus PostReihe(,) auff�llen
        '---------------------------------------------
        'Erste Zeile mit Jahreszahlen
        Dim Jahr As Integer = BeginPost
        For i = AnzJahre To AnzJahre_gesamt - 1
            Data(0, i) = Jahr
            Jahr += 1
        Next

        'Weitere Zeilen mit Werten f�llen
        i = 0
        k = Me.BeginPost - Me.BeginPre
        While (i <= PostReihe.Length - 1)
            For j = 1 To 366
                Data(j, k) = PostReihe.YWerte(i)
                'bei Nicht-Schaltjahren nach dem 28.Feb. einen Wert auff�llen
                If (PostReihe.XWerte(i).DayOfYear = 59 And DateTime.IsLeapYear(PostReihe.XWerte(i).Year) = False) Then
                    j += 1
                    Data(j, k) = -999999
                End If
                i += 1
            Next
            k += 1
        End While

        'input.dat Datei schreiben
        '-------------------------
        Call write_InputDat(Data)

        'IHA-Berechnung ausf�hren
        '------------------------
        Call launch_IHA()

        'IHA-Ergebnisse einlesen
        '-----------------------
        Call read_IHAResults()

    End Sub

    'QWert aus IHA-Ergebnissen berechnen
    '***********************************
    Public Function QWert_IHA(ByVal OptZiel As Sim.Struct_OptZiel) As Double

        Dim QWert As Double
        Dim HA As Double
        Dim i As Integer
        'Parameter f�r Normalverteilung mit f(0) ~= 1
        Dim std As Double = 0.398942423706863                   'Standardabweichung
        Dim avg As Double = 0                                   'Erwartungswert

        'HA-Wert bestimmen
        '-----------------
        If (OptZiel.ZielFkt = "") Then
            'HA Gesamtmittelwert
            HA = Me.IHARes.Avg
        Else
            'HA Mittelwert einer Parametergruppe
            For i = 0 To Me.IHARes.IHAParamGroups.GetUpperBound(0)
                If (OptZiel.ZielFkt = Me.IHARes.IHAParamGroups(i).GName) Then
                    HA = Me.IHARes.IHAParamGroups(i).Avg
                    Exit For
                End If
            Next
        End If

        'QWert = 1 - f(x)
        '[EXCEL:] 1/(std*WURZEL(2*PI()))*EXP(-1/2*((X-avg)/std)^2)
        QWert = 1 - 1 / (std * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 / 2 * ((HA - avg) / std) ^ 2)

        Return QWert

    End Function

    'IHA Ergebnisse einlesen
    '***********************
    Private Sub read_IHAResults()

        'RVA-Datei �ffnen und einlesen
        '-----------------------------

        Dim FiStr As FileStream = New FileStream(Me.IHADir & "output.rva", FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        Dim i, j As Integer
        Dim Zeile As String
        Dim Psum, Gsum As Double

        'Schleife �ber Parametergruppen
        '------------------------------
        Gsum = 0
        For i = 0 To IHARes.IHAParamGroups.GetUpperBound(0)

            'Ergebnisse einer Parametergruppe einlesen
            '-----------------------------------------
            With Me.IHARes.IHAParamGroups(i)

                Do
                    Zeile = StrRead.ReadLine.ToString
                    If (Zeile.Contains("Parameter Group #" & .No)) Then

                        Psum = 0
                        'Schleife �ber Parameter
                        For j = 0 To .IHAParams.GetUpperBound(0)
                            Zeile = StrRead.ReadLine.ToString
                            .IHAParams(j).PName = Zeile.Substring(0, 20).Trim
                            .IHAParams(j).HAMiddle = Convert.ToDouble(Zeile.Substring(171, 14).Trim)
                            Psum += .IHAParams(j).HAMiddle
                        Next

                        Exit Do
                    End If
                Loop Until StrRead.Peek() = -1

                'Mittelwert einer Parametergruppe berechnen
                '------------------------------------------
                .Avg = Psum / .IHAParams.GetLength(0)
                Gsum += .Avg

            End With

        Next 'Ende Schleife �ber Parametergruppen

        'Mittelwert aller Parametergruppen berechnen
        '-------------------------------------------
        Me.IHARes.Avg = Gsum / Me.IHARes.IHAParamGroups.GetLength(0)

    End Sub

    'IHA_Batchfor.exe ausf�hren
    '**************************
    Private Sub launch_IHA()
        'Aktuelles Verzeichnis bestimmen
        Dim currentDir As String = CurDir()
        'zum Arbeitsverzeichnis wechseln
        ChDrive(Me.IHADir)
        ChDir(Me.IHADir)
        'EXE aufrufen
        Dim ProcID As Integer = Shell("""IHA_Batchfor.exe"" input.par", AppWinStyle.MinimizedNoFocus, True)
        'zur�ck in Ausgangsverzeichnis wechseln
        ChDrive(currentDir)
        ChDir(currentDir)
    End Sub

#End Region 'Methoden

End Class
