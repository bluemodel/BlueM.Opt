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

    Public IHAZiel As Sim.Struct_OptZiel                'Kopie des OptZiels mit ZielTyp "IHA"
    Friend IHADir As String                             'Verzeichnis für IHA-Dateien
    Private exe_path As String                          'Pfad zur IHA_Batchfor.exe

    Private RVAResult As Wave.RVA.Struct_RVAValues      'RVA-Ergebnis

    Private isComparison As Boolean                     'Gibt an, ob mit bestehenden RVA-Werten verglichen werden soll
    Private RVAResultBase As Wave.RVA.Struct_RVAValues  'zu vergleichendes RVA-Ergebnis

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
        Me.exe_path = System.Windows.Forms.Application.StartupPath() & "\Apps\BlueM\IHA_Batchfor.exe"
        If (Not File.Exists(Me.exe_path)) Then
        	Throw New Exception("IHA_Batchfor.exe nicht gefunden!")
       	End If

        'IHAZiel kopieren
        '----------------
        Me.IHAZiel = OptZiel

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

        'Falls vorhanden, Referenz RVA-Werte einlesen
        '--------------------------------------------
        Dim reffile As String = BlueM1.WorkDir & BlueM1.Datensatz & ".rva"
        If (File.Exists(reffile)) Then

            Me.isComparison = True

            Dim RVA As New Wave.RVA(reffile)
            Me.RVAResultBase = RVA.RVAValues

        End If

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
    Public Sub calculate_IHA(ByVal SimReihe As Wave.Zeitreihe)

        Dim i, j, k As Integer

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
        Dim RVA As New Wave.RVA(Me.IHADir & "output.rva")
        Me.RVAResult = RVA.RVAValues

    End Sub

    'QWert aus IHA-Ergebnissen berechnen
    '***********************************
    Public Function QWert_IHA(ByVal OptZiel As Sim.Struct_OptZiel) As Double

        Dim QWert As Double
        Dim fx_HA, diff As Double
        Dim i As Integer

        If (Me.isComparison) Then

            'RVA-Werte vergleichen
            '---------------------
            If (OptZiel.ZielFkt = "") Then
                'Gesamtmittelwert
                diff = Me.RVAResultBase.GAvg_fx_HA - Me.RVAResult.GAvg_fx_HA
            Else
                'fx(HA) Mittelwert einer Parametergruppe
                For i = 0 To Me.RVAResult.IHAParamGroups.GetUpperBound(0)
                    If (OptZiel.ZielFkt = Me.RVAResult.IHAParamGroups(i).GName) Then
                        diff = Me.RVAResultBase.IHAParamGroups(i).Avg_fx_HA - Me.RVAResult.IHAParamGroups(i).Avg_fx_HA
                        Exit For
                    End If
                Next
            End If

            QWert = 1 - diff

        Else

            'fx(HA) Wert bestimmen
            '-----------------
            If (OptZiel.ZielFkt = "") Then
                'fx(HA) Gesamtmittelwert
                fx_HA = Me.RVAResult.GAvg_fx_HA
            Else
                'fx(HA) Mittelwert einer Parametergruppe
                For i = 0 To Me.RVAResult.IHAParamGroups.GetUpperBound(0)
                    If (OptZiel.ZielFkt = Me.RVAResult.IHAParamGroups(i).GName) Then
                        fx_HA = Me.RVAResult.IHAParamGroups(i).Avg_fx_HA
                        Exit For
                    End If
                Next
            End If

            QWert = 1 - fx_HA

        End If

        Return QWert

    End Function

    'IHA_Batchfor.exe ausführen
    '**************************
    Private Sub launch_IHA()
        'Aktuelles Verzeichnis bestimmen
        Dim currentDir As String = CurDir()
        'zum IHA-Verzeichnis wechseln
        ChDrive(Me.IHADir)
        ChDir(Me.IHADir)
        'EXE aufrufen
        Dim ProcID As Integer = Shell("""" & Me.exe_path & """ input.par", AppWinStyle.Hide, True)
        'zurück in Ausgangsverzeichnis wechseln
        ChDrive(currentDir)
        ChDir(currentDir)
    End Sub

#End Region 'Methoden

End Class
