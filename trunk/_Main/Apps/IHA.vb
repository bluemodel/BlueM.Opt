Imports System.IO

Public Class IHA

    '***************************************************************************************
    '***************************************************************************************
    '**** Klasse IHA zur Ermittlung der Hydrologischen Veränderung                      ****
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
    '**** Letzte Änderung: April 2007                                                   ****
    '****                                                                               ****
    '**** Einschränkungen:                                                              ****
    '**** ----------------                                                              ****
    '**** Zeitreihen dt = 1 Tag (24h)                                                   ****
    '**** nur eine einzige IHA-Zielfunktion erlaubt                                     ****
    '**** Vergleich der Simulationszeitreihe mit einer Referenzzeitreihe                ****
    '**** Hydrologisches Jahr fängt immer am 1. Okt an                                  ****
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

    Private IHAZiel As Sim.Struct_OptZiel               'Kopie des OptZiels mit ZielTyp "IHA"
    Private IHADir As String                            'Verzeichnis für IHA-Dateien
    Private exe_path As String                          'Pfad zur IHA_Batchfor.exe

    'IHA-Ergebnisse
    '--------------
    Private Structure IHAParam                          'Struktur für IHA Ergebnis eines Parameters
        Public PName As String                          'Parametername
        Public HAMiddle As Double                       'Hydrologic Alteration (HA) - Middle RVA Category
        Public ReadOnly Property fx_HA() As Double      'normalverteilter Funktionswert des HA-Werts
            Get
                Return fx(HAMiddle)
            End Get
        End Property
    End Structure

    Private Structure IHAParamGroup                     'Struktur für IHA Ergebnis einer Parametergruppe
        Public No As Short                              'Gruppennummer
        Public GName As String                          'Gruppennname
        Public Avg_fx_HA As Double                      'Mittelwert der fx(HA)-Werte der Gruppe
        Public IHAParams() As IHAParam                  'Liste der Paremeter
    End Structure

    Private Structure IHAResult                         'Struktur für alle IHA Ergebnisse zusammen
        Public GAvg_fx_HA As Double                     'Mittelwert der fx(HA)-Werte über alle Gruppen
        Public IHAParamGroups() As IHAParamGroup        'Liste der Parametergruppen
    End Structure

    Private IHARes As IHAResult

    'IHA-Software-Parameter
    '----------------------
    Private Const BegWatrYr As Integer = 275            '1. Okt.

    'Jahreszahlen
    Private BeginPre As Integer
    Private EndPre As Integer
    Private BeginPost_sim As Integer                    'tatsächlich
    Private EndPost_sim As Integer                      'tatsächlich
    Private BeginPost As Integer                        'für IHA verwendet
    Private EndPost As Integer                          'für IHA verwendet

    Private RefData(,) As Double                        'Enthält die Referenz-Abflussdaten in Zeilen für jeden Tag des Jahres

#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

    'Konstruktor
    '***********
    Public Sub New(ByVal OptZiel As Sim.Struct_OptZiel)

        'Pfad zur EXE festlegen
        '----------------------
        Me.exe_path = System.Windows.Forms.Application.StartupPath() & "\Apps\IHA_Batchfor.exe"
        If (Not File.Exists(Me.exe_path)) Then
        	Throw New Exception("IHA_Batchfor.exe nicht gefunden!")
       	End If

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
        Me.BeginPost = Me.EndPre + 1 '(Post-Zeitraum wird an das Ende des Pre-Zeitraums angehängt!)

        'EndPost
        If (BlueM1.SimEnde.DayOfYear >= BegWatrYr) Then
            Me.EndPost_sim = BlueM1.SimEnde.Year
        Else
            Me.EndPost_sim = BlueM1.SimEnde.Year - 1
        End If
        Me.EndPost = Me.BeginPost + (Me.EndPost_sim - Me.BeginPost_sim)

        'Zeitreihe kürzen
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

        'Weitere Zeilen mit Werten füllen
        i = 0
        k = 0
        While (i <= RefReihe.Length - 1)
            For j = 1 To 366
                Me.RefData(j, k) = RefReihe.YWerte(i)
                'bei Nicht-Schaltjahren nach dem 28.Feb. einen Wert auffüllen
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
        StrWrite.WriteLine("Parametric=F")                              'F: non-parametric; T: parametric
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

    'IHA-Berechnung ausführen
    '************************
    Public Sub calculate_IHA(ByVal WELFile As String)

        Dim i, j, k As Integer

        'Simulationsreihe einlesen
        '-------------------------
        Dim SimReihe As New Wave.Zeitreihe(Me.IHAZiel.SimGr)
        Dim WEL As New Wave.WEL(WELFile, Me.IHAZiel.SimGr)
        SimReihe = WEL.Zeitreihen(0)

        'Simulationsreihe entsprechend kürzen
        SimReihe.cut(New DateTime(Me.BeginPost_sim - 1, 10, 1), New DateTime(Me.EndPost_sim, 9, 30))

        'Data(,) dimensionieren 
        Dim AnzJahre As Integer = Me.EndPost - Me.BeginPost + 1
        Dim Data(366, AnzJahre - 1) As Double

        'RefData(,) nach Data(,) kopieren
        Array.Copy(Me.RefData, Data, Me.RefData.Length)

        'Data(,) redimensionieren
        Dim AnzJahre_gesamt As Integer = Me.EndPost - Me.BeginPre + 1
        ReDim Preserve Data(366, AnzJahre_gesamt - 1)

        'Data(,) mit Werten aus PostReihe(,) auffüllen
        '---------------------------------------------
        'Erste Zeile mit Jahreszahlen
        Dim Jahr As Integer = BeginPost
        For i = AnzJahre To AnzJahre_gesamt - 1
            Data(0, i) = Jahr
            Jahr += 1
        Next

        'Weitere Zeilen mit Werten füllen
        i = 0
        k = Me.BeginPost - Me.BeginPre
        While (i <= SimReihe.Length - 1)
            For j = 1 To 366
                Data(j, k) = SimReihe.YWerte(i)
                'bei Nicht-Schaltjahren nach dem 28.Feb. einen Wert auffüllen
                If (SimReihe.XWerte(i).DayOfYear = 59 And DateTime.IsLeapYear(SimReihe.XWerte(i).Year) = False) Then
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

        'IHA-Berechnung ausführen
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
        Dim fx_HA As Double
        Dim i As Integer

        'fx(HA) Wert bestimmen
        '-----------------
        If (OptZiel.ZielFkt = "") Then
            'fx(HA) Gesamtmittelwert
            fx_HA = Me.IHARes.GAvg_fx_HA
        Else
            'fx(HA) Mittelwert einer Parametergruppe
            For i = 0 To Me.IHARes.IHAParamGroups.GetUpperBound(0)
                If (OptZiel.ZielFkt = Me.IHARes.IHAParamGroups(i).GName) Then
                    fx_HA = Me.IHARes.IHAParamGroups(i).Avg_fx_HA
                    Exit For
                End If
            Next
        End If

        QWert = 1 - fx_HA

        Return QWert

    End Function

    'Transformiert einen HA-Wert in einen normalverteilten Funktionswert
    '*******************************************************************
    Private Shared Function fx(ByVal HA As Double) As Double

        'Parameter für Normalverteilung mit f(0) ~= 1
        '[EXCEL:] 1/(std*WURZEL(2*PI()))*EXP(-1/2*((X-avg)/std)^2)
        Dim std As Double = 0.398942423706863                   'Standardabweichung
        Dim avg As Double = 0                                   'Erwartungswert

        fx = 1 / (std * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 / 2 * ((HA - avg) / std) ^ 2)

        Return fx

    End Function

    'IHA Ergebnisse einlesen
    '***********************
    Private Sub read_IHAResults()

        'RVA-Datei öffnen und einlesen
        '-----------------------------

        Dim FiStr As FileStream = New FileStream(Me.IHADir & "output.rva", FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        Dim i, j As Integer
        Dim Zeile As String
        Dim Psum, Gsum As Double                        'Summen der fx_HA Werte

        'Schleife über Parametergruppen
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
                        'Schleife über Parameter
                        For j = 0 To .IHAParams.GetUpperBound(0)
                            Zeile = StrRead.ReadLine.ToString
                            .IHAParams(j).PName = Zeile.Substring(0, 20).Trim
                            .IHAParams(j).HAMiddle = Convert.ToDouble(Zeile.Substring(171, 14).Trim)
                            Psum += .IHAParams(j).fx_HA
                        Next

                        Exit Do
                    End If
                Loop Until StrRead.Peek() = -1

                'Mittelwert für eine Parametergruppe berechnen
                '---------------------------------------------
                .Avg_fx_HA = Psum / .IHAParams.GetLength(0)
                Gsum += .Avg_fx_HA

            End With

        Next 'Ende Schleife über Parametergruppen

        'Mittelwert aller Parametergruppen berechnen
        '-------------------------------------------
        Me.IHARes.GAvg_fx_HA = Gsum / Me.IHARes.IHAParamGroups.GetLength(0)

    End Sub

    'IHA_Batchfor.exe ausführen
    '**************************
    Private Sub launch_IHA()
        'Aktuelles Verzeichnis bestimmen
        Dim currentDir As String = CurDir()
        'zum IHA-Verzeichnis wechseln
        ChDrive(Me.IHADir)
        ChDir(Me.IHADir)
        'EXE aufrufen
        Dim ProcID As Integer = Shell("""" & Me.exe_path & """ input.par", AppWinStyle.MinimizedNoFocus, True)
        'zurück in Ausgangsverzeichnis wechseln
        ChDrive(currentDir)
        ChDir(currentDir)
    End Sub

#End Region 'Methoden

End Class
