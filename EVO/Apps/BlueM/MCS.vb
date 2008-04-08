Imports System.IO
Imports IHWB.BlueM.DllAdapter

Public Class MCS

    Dim Pstd(10000) As Double 'Basisniederschlagsvektor
    Dim Dauer, Lauf As Integer
    Dim P_Menge, P_Area, P_area_ohTS As Double
    Dim StFac(10000), Schaden As Double
    Dim rand(2000), Teibereiche As Double
    Dim Parray(2000) As Double
    Dim Qzu_Lehn(2000), Qzu_Kling(2000), Qzu_Malter(2000) As Double
    Dim Qab_Lehn(2000), Qab_Kling(2000), Qab_Malter(2000), Qab_Cotta(2000), Qab_Ammel(2000) As Double
    Public MAE, RMSE, NS, QReheSum, QReheSumB, QPegel(10000), QReheMaxB As Double
    Dim WSP_Malter, WSP_Lehn, WSP_Kling As Double
    Dim QAmmelSum, QDippsSum, QzuSum, QsimAmmel(10000), QsimRehefeld(10000), QsimDipps(10000) As Double
    Dim damage, damage_ww, damage_rw, damage_vw, P_Dauer, QSKLI As Double
    Dim QmaxLehn, QmaxKling, QmaxMalter, VmaxLehn, VmaxKling, VmaxMalter, QmaxAmmel, QmaxDipps As Double
    Dim QmaxAbLehn, QmaxAbKling, QmaxAbMalter, QmaxCotta, Dmin, Dmax, VAmmel, VDipps, Qschwell As Double
    Dim vary_V0, vary_VR, read_P, solve_BlueM, Polder, pol_kli, pol_mal, pol_leh As Boolean
    Dim HWE_LEH, HWE_KLI, HWE_MAL As Boolean
    Dim SummeP, Text3, V0Lehn, V0Kling, V0Malter As String
    
    Public Datensatz As String                           'Name des zu simulierenden Datensatzes
    Public WorkDir As String                             'Arbeitsverzeichnis für das Blaue Modell
    Public SimStart As DateTime                          'Anfangsdatum der Simulation
    Public SimEnde As DateTime                           'Enddatum der Simulation
    Public SimDT As TimeSpan                             'Zeitschrittweite der Simulation

    Private bluem_dll As BlueM_EngineDotNetAccess
    Private Damage1 As Damage
 
    '#######################################################################
    '# Einstellungen
    '#######################################################################

    Dim nTeilbereiche As Integer = 4
    Dim nStationen As Integer = 15
   
    Public Vorlaufzeit As Integer  '!Auch in Sim.vb/ Write_ModellParameter() festlegen!!

    Dim Pmin As Double = 100
    Dim Pmax As Double = 500
    Dim PMGN1 As Double = 200
    Dim PMGN12 As Double = 325
    Dim PMGN24 As Double = 400
    Dim PMGN72 As Double = 500

    Dim DistMax As Double = 40.0 '--> variable FORM excel auswertung

    Dim StDist(,) As Double = {{0.0, 2.22, 11.15, 28.93, 25.89, 24.06, 7.17, 15.64, 15.71, 14.53, 18.96, 17.92, 4.51, 20.95, 9.22} _
                            , {2.22, 0.0, 13.37, 31.16, 28.0, 26.13, 8.44, 17.85, 17.5, 16.75, 21.18, 20.14, 6.71, 22.76, 7.44} _
                            , {11.15, 13.37, 0.0, 17.8, 15.43, 13.96, 9.61, 4.92, 9.21, 3.41, 8.07, 6.82, 6.68, 13.18, 19.73} _
                            , {28.93, 31.16, 17.8, 0.0, 8.91, 10.74, 26.2, 13.51, 19.02, 14.48, 10.23, 11.21, 24.47, 17.09, 37.14} _
                            , {25.89, 28.0, 15.43, 8.91, 0.0, 2.33, 21.24, 13.26, 12.32, 12.23, 11.29, 9.19, 21.47, 8.85, 34.92} _
                            , {24.06, 26.13, 13.96, 10.74, 2.33, 0.0, 19.12, 12.47, 10.04, 10.97, 11.05, 8.31, 19.7, 6.58, 33.17} _
                            , {7.17, 8.44, 9.61, 26.2, 21.24, 19.12, 0.0, 14.48, 9.57, 12.17, 17.39, 15.07, 5.74, 14.78, 15.85} _
                            , {15.64, 17.85, 4.92, 13.51, 13.26, 12.47, 14.48, 0.0, 11.73, 3.02, 3.34, 4.16, 11.32, 14.1, 23.62} _
                            , {15.71, 17.5, 9.21, 19.02, 12.32, 10.04, 9.57, 11.73, 0.0, 8.73, 13.08, 9.53, 12.02, 5.26, 24.9} _
                            , {14.53, 16.75, 3.41, 14.48, 12.23, 10.97, 12.17, 3.02, 8.73, 0.0, 5.26, 3.41, 10.04, 11.46, 23.13} _
                            , {18.96, 21.18, 8.07, 10.23, 11.29, 11.05, 17.39, 3.34, 13.08, 5.26, 0.0, 3.68, 14.62, 14.23, 26.93} _
                            , {17.92, 20.14, 6.82, 11.21, 9.19, 8.31, 15.07, 4.16, 9.53, 3.41, 3.68, 0.0, 13.42, 10.6, 26.54} _
                            , {4.51, 6.71, 6.68, 24.47, 21.47, 19.7, 5.74, 11.32, 12.02, 10.04, 14.62, 13.42, 0.0, 17.11, 13.47} _
                            , {20.95, 22.76, 13.18, 17.09, 8.85, 6.58, 14.78, 14.1, 5.26, 11.46, 14.23, 10.6, 17.11, 0.0, 30.15} _
                            , {9.22, 7.44, 19.73, 37.14, 34.92, 33.17, 15.85, 23.62, 24.9, 23.13, 26.93, 26.54, 13.47, 30.15, 0.0}}

    'Flächenanteil aus Polygonmethode
    Dim PolyFac() As Double = {0.07, 0.1, 0.1, 0.06, 0.04, 0.06, 0.09, 0.03, 0.11, 0.05, 0.05, 0.1, 0.08, 0.02, 0.04}
    Dim PolyFac_ohTS() As Double = {0.11, 0.16, 0.17, 0.0, 0.0, 0.0, 0.15, 0.0, 0.18, 0.0, 0.0, 0.0, 0.14, 0.03, 0.06}

    '#######################################################################


    Public Sub MonteCarlo(ByVal iLauf As Integer, ByVal nt As Integer,ByVal evo as Boolean)

        Vorlaufzeit = nt
        Lauf = iLauf

        Call getPath() 'ermittle Arbeitsverzeichnis
        Call readDate() 'Simulationszeitraum ermitteln

        read_Nied(iLauf)

        writeZRE("AltenbergKipsdorf.zre", StFac(1))
        writeZRE("AltenbergSchellerau.zre", StFac(2))
        writeZRE("DippoldiswaldeReinberg.zre", StFac(3))
        writeZRE("DresdenLeutewitz.zre", StFac(4))
        writeZRE("Grumbach.zre", StFac(5))
        writeZRE("HarthaFoerdergersdorf.zre", StFac(6))
        writeZRE("HartmannsdorfReichenau.zre", StFac(7))
        writeZRE("Karsdorf.zre", StFac(8))
        writeZRE("KlingenbergTS.zre", StFac(9))
        writeZRE("MalterTS.zre", StFac(10))
        writeZRE("Possendorf.zre", StFac(11))
        writeZRE("Rabenau.zre", StFac(12))
        writeZRE("Schmiedeberg.zre", StFac(13))
        writeZRE("TharandtGrillenburg.zre", StFac(14))
        writeZRE("ZinnwaldGeorgenfeld.zre", StFac(15))

        modify_TS_V0("/../ZRE/S0.txt", "Lehnmuehle", iLauf)
        modify_TS_V0("/../ZRE/S0.txt", "Klingenberg", iLauf)
        modify_TS_V0("/../ZRE/S0.txt", "Malter", iLauf)

        If Not evo Then
            'runBluem()
            'readWel(Me.WorkDir, Me.Datensatz)
            'Me.Damage1 = New Damage()
            'Schaden = Me.Damage1.QWert_Damage(WorkDir & Datensatz & ".wel")
            output(iLauf)
        End If

    End Sub

    '* allgemeine Niederschlagsdatei erstellen
    '*****************************************************
    Public Sub erstelle_Niederschlag(ByVal iStart As Integer, ByVal iEnde As Integer)

        Dim i As Integer = 0
        Dim k As Integer

        Dim GradMax As Double
        Dim StNr, Grad As Double

        Call getPath() 'ermittle Arbeitsverzeichnis
        Call readDate() 'Simulationszeitraum ermitteln

        Dim out As String = ""

        For Lauf = iStart To iEnde

            Dauer = Zufallsdauer()

            Dim P_Matrix() As Double = {Pmin, Pmax, PMGN1, PMGN12, PMGN24, PMGN72}

100:        P_Menge = NiedGen(P_Matrix(0), P_Matrix(1), Lauf)


           
            If CheckPSum(1) > P_Matrix(2) Then GoTo 100
            If CheckPSum(12) > P_Matrix(3) Then GoTo 100
            If CheckPSum(24) > P_Matrix(4) Then GoTo 100
            If CheckPSum(72) > P_Matrix(5) Then GoTo 100

            '*****************************************
            '* räumlichen Abmindungsfaktor ermitteln *
            '*****************************************

            'Console.Out.WriteLine(P_Menge)

            GradMax = P_Menge / DistMax

            'linearer Minderungsgradient
            Randomize()
            Grad = GradMax * Rnd() ' zufällige Auswahl zw. 0 und Maximum

            Randomize()
            StNr = CDbl(Int(Rnd() * (nStationen - 1)))

            For i = 1 To nStationen

                StFac(i) = (P_Menge - Grad * StDist(i - 1, StNr)) / P_Menge
                'Console.Out.WriteLine(StFac(i))
            Next

            '*****************************************
            'Schreiben der allgemeinen P-Dateien
            '*****************************************

            Dim StrWri As StreamWriter = New StreamWriter(WorkDir + "ZRE/Nied_" + Lauf.ToString + ".txt", False, System.Text.Encoding.GetEncoding("iso8859-1"))

            out = ""
            'Faktoren für räumliche Verteilung zuerst
            For k = 1 To nStationen
                out += StFac(k).ToString + vbCrLf
            Next k
            'out += "# Niederschlagsdaten" + vbCrLf
            For k = 1 To 72 + 24
                out += Pstd(k).ToString + vbCrLf
            Next k

            StrWri.Write(out)
            StrWri.Close()

            '*****************************************

            'read_Nied(Lauf) 'if readP-erneut
        Next Lauf


        '**************************************
        '* zur INFO: Gebietsniederschlag ausrechnen 
        '**************************************
        ' basiert auf Polygonmethode

        P_Area = 0.0

        For k = 1 To nStationen
            P_Area += P_Menge * StFac(k) * PolyFac(k - 1)
        Next k

        '* oberhalb der TS
        '**************************************
        P_area_ohTS = 0.0

        P_area_ohTS += P_Menge * StFac(1) * PolyFac_ohTS(1 - 1)
        P_area_ohTS += P_Menge * StFac(2) * PolyFac_ohTS(2 - 1)
        P_area_ohTS += P_Menge * StFac(3) * PolyFac_ohTS(3 - 1)
        P_area_ohTS += P_Menge * StFac(7) * PolyFac_ohTS(7 - 1)
        P_area_ohTS += P_Menge * StFac(9) * PolyFac_ohTS(9 - 1)
        P_area_ohTS += P_Menge * StFac(13) * PolyFac_ohTS(13 - 1)
        P_area_ohTS += P_Menge * StFac(14) * PolyFac_ohTS(14 - 1)
        P_area_ohTS += P_Menge * StFac(15) * PolyFac_ohTS(15 - 1)

    End Sub

    '* Niederschlagsdauer von 12h - 72h mit dh=6 ermitteln 
    '*****************************************************
    Public Function Zufallsdauer() As Integer
        Dim nDauer As Integer

        Randomize()
        If Rnd() <= 6 / 6 Then nDauer = 72
        If Rnd() < 5 / 6 Then nDauer = 60
        If Rnd() < 4 / 6 Then nDauer = 48
        If Rnd() < 3 / 6 Then nDauer = 36
        If Rnd() < 2 / 6 Then nDauer = 24
        If Rnd() < 1 / 6 Then nDauer = 12

        Return nDauer
    End Function

    '* Generiert Basisereignis
    '*************************
    Public Function NiedGen(ByVal Pmin As Integer, ByVal Pmax As Integer, ByVal num As Integer) As Double

        Dim SumP1(1000), SumP2(1000), ZF1(1000), ZF2(1000), Sum1, Sum2, zahl As Double
        Dim n, m, k, L_Tb, n_Tb, P_Sum, ctrsum As Integer
        Dim out As String
        Dim Pmax1h, Pmax12h As Double
        Dim PP As Double

        For k = 1 To 72 + 24
            Pstd(k) = 0.0
        Next k

        Randomize()
        P_Sum = Pmin + Int(Rnd() * (Pmax - Pmin))

        n_Tb = nTeilbereiche
        L_Tb = Dauer / n_Tb                    'default 48/4=12

        Sum1 = 0.0
        Sum2 = 0.0
        Pmax1h = 0.0
        Pmax12h = 0.0
        out = ""
        ctrsum = 0.0

        '-----------------------------------------------
        'Ermittle Zufallszahlen für die Teilbereiche
        '-----------------------------------------------

        For n = 1 To n_Tb
            Randomize()              'Initialisiert den Zufallszahlengenerator
            ZF1(n) = Rnd()           'Ermittle gleichverteilte ZF
            Sum1 = Sum1 + ZF1(n)     'summiere ZF
            'Console.Out.WriteLine(ZF1(n))
        Next n

        zahl = P_Sum / Sum1

        For n = 1 To n_Tb
            SumP1(n) = zahl * ZF1(n)
            If (SumP1(n) > Pmax12h) Then Pmax12h = SumP1(n) 'keep Maximum
        Next n

        '-----------------------------------------------
        'Ermittelt Zufallszahlen für Niederschlagsintervall
        '-----------------------------------------------

        For n = 1 To n_Tb         'Für jeden Teilbereich

            Sum2 = 0

            For m = 1 To L_Tb     'Für jede Stunde
                Randomize()
                ZF2(m) = Rnd()
                Sum2 = Sum2 + ZF2(m)
            Next m

            zahl = SumP1(n) / Sum2

            For m = 1 To L_Tb
                k = m + (n - 1) * L_Tb
                Pstd(k) = zahl * ZF2(m)
                ctrsum = ctrsum + Pstd(k)
                If (Pstd(k) > Pmax1h) Then Pmax1h = Pstd(k)
                'Console.Out.WriteLine(Pstd(k))
            Next m
        Next n

        'Nachregenzeit von 24h anhängen

        For k = 1 To 24
            Pstd(k + Dauer) = 0.0
        Next k

        '*****************************************
        'Schreiben der P-Dateien
        '*****************************************

        Dim Datei As String = WorkDir + "ZRE/Nied_" + num.ToString + ".txt"

        Dim StrWri As StreamWriter = New StreamWriter(Datei, False, System.Text.Encoding.GetEncoding("iso8859-1"))
        out = ""
        'Faktoren für räumliche Verteilung zuerst, nur Initialisierung!, da noch nicht bekannt
        For k = 1 To nStationen
            out = out + "0.0" + vbCrLf
        Next k

        For k = 1 To 72 + 24
            out = out + Pstd(k).ToString + vbCrLf
            PP = PP + Pstd(k)
            'Console.Out.WriteLine(PP)
        Next k

        StrWri.Write(out)
        StrWri.Close()

        Return PP

    End Function

    '* Kontrolliert P-Summe bzgl. MGN
    '********************************
    Function CheckPSum(ByVal zeit As Integer) As Double
        Dim CtrSum, MaxCtrSum As Double
        Dim k, m, n As Integer
        MaxCtrSum = 0.0
        For k = 1 To Dauer - zeit + 1
            CtrSum = 0.0
            For n = 1 To zeit
                m = n + k - 1
                CtrSum = CtrSum + Pstd(m)
            Next n
            If CtrSum > MaxCtrSum Then MaxCtrSum = CtrSum
        Next k
        Return MaxCtrSum
    End Function

    '* liest vorher geniertes Niederschlagsereignis ein
    '***************************************************
    Public Sub read_Nied(ByVal number As Integer)
        Dim Text As String

        Dim FiStr As FileStream = New FileStream(WorkDir + "/../ZRE/Nied_" + number.ToString + ".txt", FileMode.Open, FileAccess.ReadWrite)
        Dim StrRe As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
        Dim n, k As Integer

        Dauer = 0.0
        P_Menge = 0.0
        P_Dauer = 0.0
        n = 0
        k = 0

        Do
            Text = StrRe.ReadLine.ToString

            n += 1
            
            If n > 0 And n <= nStationen Then
                StFac(n) = CDbl(Text.Trim)
            End If

            If n > nStationen Then
                k += 1
                Pstd(k) = CDbl(Text.Trim)
                'Console.Out.WriteLine(Pstd(k))
                P_Menge += Pstd(k)
                If Pstd(k) > 0.0 Then P_Dauer = k
                'If Pstd(k) > 0.0 Then Console.Out.WriteLine(Pstd(k))
            End If

        Loop Until StrRe.Peek() = -1

        StrRe.Close()
        FiStr.Close()

        '**************************************
        '* zur INFO: Gebietsniederschlag ausrechnen 
        '**************************************
        ' basiert auf Polygonmethode

        P_Area = 0.0

        For k = 1 To nStationen
            P_Area += P_Menge * StFac(k) * PolyFac(k - 1)
        Next k

        '* oberhalb der TS
        '**************************************
        P_area_ohTS = 0.0

        P_area_ohTS += P_Menge * StFac(1) * PolyFac_ohTS(1 - 1)
        P_area_ohTS += P_Menge * StFac(2) * PolyFac_ohTS(2 - 1)
        P_area_ohTS += P_Menge * StFac(3) * PolyFac_ohTS(3 - 1)
        P_area_ohTS += P_Menge * StFac(7) * PolyFac_ohTS(7 - 1)
        P_area_ohTS += P_Menge * StFac(9) * PolyFac_ohTS(9 - 1)
        P_area_ohTS += P_Menge * StFac(13) * PolyFac_ohTS(13 - 1)
        P_area_ohTS += P_Menge * StFac(14) * PolyFac_ohTS(14 - 1)
        P_area_ohTS += P_Menge * StFac(15) * PolyFac_ohTS(15 - 1)

    End Sub

    'Pfad zum Datensatz verarbeiten und speichern
    '********************************************
    Public Sub getPath()

        'Datensatz
        '---------
        Dim pfad As String
        pfad = My.Settings.Datensatz

        If (File.Exists(pfad)) Then
            'Datensatzname bestimmen
            Me.Datensatz = Path.GetFileNameWithoutExtension(pfad)
            'Arbeitsverzeichnis bestimmen
            Me.WorkDir = Path.GetDirectoryName(pfad) & "\"
        End If


    End Sub

    'Schreibt Niederschlagdateien
    '********************************************
    Sub writeZRE(ByVal zreName As String, ByVal Faktor As Double)

        Dim i As Integer = 0
        Dim Text As String = ""
        Dim actDate, date2, dateVor As Date

        Text = "*ZRE" + vbCrLf
        Text += "ZRE-Format     mm/h" + vbCrLf
        Text += "1 1   1" + vbCrLf
        Text += Me.SimStart.ToString("yyyyMMdd HH:mm") + " " + Me.SimEnde.ToString("yyyyMMdd HH:mm") + vbCrLf

        actDate = Me.SimStart
        date2 = actDate
        dateVor = actDate.AddHours(Vorlaufzeit)

        While date2 <= Me.SimEnde

            If date2 > dateVor Then
                i += 1
                Text += date2.ToString("yyyyMMdd HH:mm") + " " + Math.Round(Pstd(i) * Faktor, 3).ToString + vbCrLf
            Else
                Text += date2.ToString("yyyyMMdd HH:mm") + " " + "0.0" + vbCrLf
            End If

            date2 = date2.Add(Me.SimDT)
        End While

        Dim StrWri As StreamWriter = New StreamWriter(WorkDir + "/../ZRE/" + zreName)
        StrWri.Write(Text)
        StrWri.Close()

    End Sub


    Private Sub readDate()

        Dim SimStart_str As String = ""
        Dim SimEnde_str As String = ""
        Dim SimDT_str As String = ""
        Dim Ganglinie As String = ""
        Dim CSV_Format As String = ""

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
            If (Zeile.StartsWith(" SimBeginn - SimEnde ............:")) Then
                SimStart_str = Zeile.Substring(35, 16)
                SimEnde_str = Zeile.Substring(54, 16)
            End If

            'Zeitschrittweite auslesen
            If (Zeile.StartsWith(" Zeitschrittlaenge [min] ........:")) Then
                SimDT_str = Zeile.Substring(35).Trim
            End If

            'Überprüfen ob die Ganglinien (.WEL Datei) ausgegeben wird
            If (Zeile.StartsWith(" Ganglinienausgabe ....... [J/N] :")) Then
                Ganglinie = Zeile.Substring(35).Trim
            End If

            'Überprüfen ob CSV Format eingeschaltet ist
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
            Throw New Exception("Das CSV Format für die .WEL Datei ist nicht eingeschaltet. Bitte in .ALL unter '... CSV-Format' einschalten.")
        End If

        FiStr.Close()
        StrRead.Close()


    End Sub

    'verändere TS V0
    '********************************************
    Private Sub modify_TS_V0(ByVal PfadSO As String, ByVal TS As String, ByVal iLauf As Integer)
        Dim Text, TextVol, TextBla, Textnew, fill As String
        Dim Vmax, Vmin, izeile, iread, V0_new As Integer

        If TS = "Lehnmuehle" Then
            Vmax = 24400
            Vmin = 5000
            izeile = 11
            iread = 1
        End If
        If TS = "Klingenberg" Then
            Vmax = 18000
            Vmin = 4000
            izeile = 10
            iread = 2
        End If
        If TS = "Malter" Then
            Vmax = 10540
            Vmin = 4440
            izeile = 13
            iread = 3
        End If


        'Randomize() 'Initialisiert den Zufallszahlengenerator
        'Dim V0_new As Integer = Vmin + Int(Rnd() * (Vmax - Vmin)) 'ganzzahlige Zufallszahl
        'lese V0-Zufallszahlen:

        Try
            Dim FiStr As FileStream = New FileStream(WorkDir + PfadSO, FileMode.Open, FileAccess.ReadWrite)
            Dim StrRe As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
            Dim i As Integer

            Text = StrRe.ReadLine.ToString

            For i = 1 To Lauf
                Text = StrRe.ReadLine.ToString

                Dim strarray() As String = Text.Split(";"c)

                V0_new = CDbl(strarray(iread))

            Next i

            StrRe.Close()
            FiStr.Close()

            If TS = "Lehnmuehle" Then
                V0Lehn = V0_new.ToString
            End If
            If TS = "Klingenberg" Then
                V0Kling = V0_new.ToString
            End If
            If TS = "Malter" Then
                V0Malter = V0_new.ToString
            End If



        Catch except As Exception
            MsgBox(except.Message, "Fehler in modify_TS_V0", MsgBoxStyle.Exclamation)
        End Try

        '#########################################


        Try
            Dim FiStr As FileStream = New FileStream(WorkDir + Datensatz + ".TAL", FileMode.Open, FileAccess.ReadWrite)
            Dim StrRe As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
            Dim i, j As Integer

            Do
                Text = StrRe.ReadLine.ToString
                i += 1
                TextBla = ""

                If i = izeile Then
                    Dim strarray() As String = Text.Split("|"c)
                    strarray(1) = " |" + strarray(1)

                    For j = 1 To 1
                        TextBla = TextBla + strarray(j) + "|"
                    Next

                    TextVol = strarray(2).Trim()
                    fill = "      "

                    If V0_new < 10000 Then fill = "       "

                    Text = TextBla + fill + V0_new.ToString + " |"

                    For j = 3 To 9
                        Text = Text + strarray(j) + "|"
                    Next
                End If

                Textnew = Textnew + Text + Environment.NewLine

            Loop Until StrRe.Peek() = -1

            StrRe.Close()
            FiStr.Close()

            Dim StrWri As StreamWriter = New StreamWriter(WorkDir + Datensatz + ".TAL", False, System.Text.Encoding.GetEncoding("iso8859-1"))
            StrWri.Write(Textnew)
            StrWri.Close()

            


        Catch except As Exception
            MsgBox(except.Message, "Fehler in modify_TS_V0", MsgBoxStyle.Exclamation)
        End Try



    End Sub

    'schreibe output.xls
    '********************************************
    Public Sub output(ByVal iLauf As Integer)

        Dim outpu, header As String

        outpu = ""

        If (iLauf = 1) Then
            Console.Out.WriteLine(Me.WorkDir & Me.Datensatz & ".ALL")
            header = "Lauf" + Chr(9) + "V0Lehn" + Chr(9) _
                                    + "V0Kling" + Chr(9) _
                                    + "V0Malter" + Chr(9) _
                                    + "WSP_Lehn" + Chr(9) _
                                    + "WSP_Kling" + Chr(9) _
                                    + "WSP_Malter" + Chr(9) _
                                    + "HWE_Lehn" + Chr(9) _
                                    + "HWE_Kling" + Chr(9) _
                                    + "HWE_Malter" + Chr(9) _
                                    + "QmaxLehn" + Chr(9) _
                                    + "QmaxKling" + Chr(9) _
                                    + "QmaxMalter" + Chr(9) _
                                    + "QmaxAbLehn" + Chr(9) _
                                    + "QmaxAbKling" + Chr(9) _
                                    + "QmaxAbMalter" + Chr(9) _
                                    + "QmaxCotta" + Chr(9) _
                                    + "VmaxLehn" + Chr(9) _
                                    + "VmaxKling" + Chr(9) _
                                    + "VmaxMalter" + Chr(9) _
                                    + "Schaden" + Chr(9) _
                                    + "QS_TS_KLI" + Chr(9) _
                                    + "QmaxAmmel" + Chr(9) _
                                    + "VAmmel" + Chr(9) _
                                    + "QmaxDipps" + Chr(9) _
                                    + "VDipps" + Chr(9) _
                                    + "P_Dauer" + Chr(9) _
                                    + "hN_max" + Chr(9) _
                                    + "hN_area" + Chr(9) _
                                    + "hN_area_ohTS" + Chr(9)


            Console.Out.WriteLine(header)
            Dim sFilePathe1 As String = Me.WorkDir & "output.xls"
            Dim streami1 As FileStream = New FileStream(sFilePathe1, FileMode.Create)
            Dim StrWri1 As StreamWriter = New StreamWriter(streami1, System.Text.Encoding.Default)
            StrWri1.Write(header + vbCrLf)
            StrWri1.Close()
        End If

        If (iLauf >= 1) Then

            outpu = Lauf.ToString + Chr(9) + V0Lehn.ToString + Chr(9) _
                                           + V0Kling.ToString + Chr(9) _
                                           + V0Malter.ToString + Chr(9) _
                                           + WSP_Lehn.ToString + Chr(9) _
                                           + WSP_Kling.ToString + Chr(9) _
                                           + WSP_Malter.ToString + Chr(9) _
                                           + HWE_LEH.ToString + Chr(9) _
                                           + HWE_KLI.ToString + Chr(9) _
                                                   + HWE_MAL.ToString + Chr(9) _
                                                    + QmaxLehn.ToString + Chr(9) _
                                                    + QmaxKling.ToString + Chr(9) _
                                                    + QmaxMalter.ToString + Chr(9) _
                                                    + QmaxAbLehn.ToString + Chr(9) _
                                                    + QmaxAbKling.ToString + Chr(9) _
                                                    + QmaxAbMalter.ToString + Chr(9) _
                                                    + QmaxCotta.ToString + Chr(9) _
                                                    + VmaxLehn.ToString + Chr(9) _
                                                    + VmaxKling.ToString + Chr(9) _
                                                    + VmaxMalter.ToString + Chr(9) _
                                                    + Schaden.ToString + Chr(9) _
                                                    + QSKLI.ToString + Chr(9) _
                                                    + QmaxAmmel.ToString + Chr(9) _
                                                    + VAmmel.ToString + Chr(9) _
                                                    + QmaxDipps.ToString + Chr(9) _
                                                    + VDipps.ToString + Chr(9) _
                                                    + P_Dauer.ToString + Chr(9) _
                                                    + P_Menge.ToString + Chr(9) _
                                                    + P_Area.ToString + Chr(9) _
                                                    + P_area_ohTS.ToString


            Console.Out.WriteLine(outpu)

            Dim sFilePathe2 As String = Me.WorkDir & "output.xls"
            Dim streami2 As FileStream = New FileStream(sFilePathe2, FileMode.Append)
            Dim StrWri2 As StreamWriter = New StreamWriter(streami2, System.Text.Encoding.Default)
            StrWri2.Write(outpu + vbCrLf)
            StrWri2.Close()


            'copyFile(Me.WorkDir, Me.Datensatz + ".wel", Lauf)

        End If

    End Sub

    'kopiere *.wel nach final/
    '********************************************
    Sub copyFile(ByVal Pfad1 As String, ByVal AnwName1 As String, ByVal iLauf As Integer)
        Dim FiStr As FileStream = New FileStream(Pfad1 + AnwName1, FileMode.Open, FileAccess.ReadWrite)
        Dim StrRe As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
        Dim Text As String
        Text = ""
        Do
            Text = Text + StrRe.ReadLine.ToString + vbCrLf
        Loop Until StrRe.Peek() = -1
        StrRe.Close()
        FiStr.Close()

        Dim StrWri As StreamWriter = New StreamWriter(Pfad1 + "final\" + Lauf.ToString + "_" + AnwName1, False, System.Text.Encoding.GetEncoding("iso8859-1"))
        StrWri.Write(Text)
        StrWri.Close()
    End Sub

    Public Sub readWel(ByVal Pfad1 As String, ByVal AnwName1 As String)
        Dim FiStr As FileStream = New FileStream(Pfad1 + AnwName1 + ".wel", FileMode.Open, FileAccess.ReadWrite)
        Dim StrRe As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
        Dim zeile, kp, kzulehn, kzukling, kzumalter, kablehn, kabkling, kabmalter, kabcotta, ksammel, kaammel, kdipps As Integer
        Dim k_mal_wsp, k_leh_wsp, k_kli_wsp As Integer
        Dim k_QH1, k_QH2, k_QH3, k_TKLI_QS1 As Integer
        Dim outQzuLehn, outQzuKling, outQzuMalter, outQabLehn, outQabKling, outQabMalter As String
        Dim Text, outP As String

        Try
            zeile = 0
            kp = 1
            kzulehn = 1
            kzukling = 1
            kzumalter = 1
            kablehn = 1
            kabkling = 1
            kabmalter = 1
            kabcotta = 1
            ksammel = 1
            kaammel = 1
            kdipps = 1
            k_mal_wsp = 1
            k_leh_wsp = 1
            k_kli_wsp = 1
            k_QH1 = 1
            k_QH2 = 1
            k_QH3 = 1
            k_TKLI_QS1 = 1

            HWE_LEH = False
            HWE_KLI = False
            HWE_MAL = False

            outP = ""
            outQzuLehn = ""
            outQzuKling = ""
            outQzuMalter = ""

            outQabLehn = ""
            outQabKling = ""
            outQabMalter = ""

            QmaxLehn = 0.0
            QmaxKling = 0.0
            QmaxMalter = 0.0

            QmaxAmmel = 0.0
            QmaxDipps = 0.0

            QmaxAbLehn = 0.0
            QmaxAbKling = 0.0
            QmaxAbMalter = 0.0
            QmaxCotta = 0.0

            VmaxLehn = 0.0
            VmaxKling = 0.0
            VmaxMalter = 0.0

            WSP_Malter = 0.0
            WSP_Lehn = 0.0
            WSP_Kling = 0.0

            VAmmel = 0.0
            VDipps = 0.0

            QSKLI = 0.0

            Do
                Text = StrRe.ReadLine.ToString
                zeile += 1

                If zeile = 2 Then
                    Dim strarray() As String = Text.Split(";"c)

                    Do While strarray(k_TKLI_QS1).Trim <> "TKLI_QS1"
                        k_TKLI_QS1 = k_TKLI_QS1 + 1
                    Loop

                    Do While strarray(k_QH1).Trim <> "TLEH_QH1"
                        k_QH1 = k_QH1 + 1
                    Loop

                    Do While strarray(k_QH2).Trim <> "TKLI_QH1"
                        k_QH2 = k_QH2 + 1
                    Loop

                    Do While strarray(k_QH3).Trim <> "TMAL_QH6"
                        k_QH3 = k_QH3 + 1
                    Loop

                    Do While strarray(k_mal_wsp).Trim <> "TMAL_WSP"
                        k_mal_wsp = k_mal_wsp + 1
                    Loop
                    Do While strarray(k_leh_wsp).Trim <> "TLEH_WSP"
                        k_leh_wsp = k_leh_wsp + 1
                    Loop
                    Do While strarray(k_kli_wsp).Trim <> "TKLI_WSP"
                        k_kli_wsp = k_kli_wsp + 1
                    Loop

                    Do While strarray(kzulehn).Trim <> "TLEH_1ZU"
                        kzulehn = kzulehn + 1
                    Loop
                    Do While strarray(kzukling).Trim <> "TKLI_1ZU"
                        kzukling = kzukling + 1
                    Loop
                    Do While strarray(kzumalter).Trim <> "TMAL_1ZU"
                        kzumalter = kzumalter + 1
                    Loop
                    '***
                    Do While strarray(kablehn).Trim <> "S109_1ZU"
                        kablehn = kablehn + 1
                    Loop
                    Do While strarray(ksammel).Trim <> "S100_1AB"
                        ksammel = ksammel + 1
                    Loop
                    Do While strarray(kaammel).Trim <> "A100_1AB"
                        kaammel = kaammel + 1
                    Loop
                    Do While strarray(kabkling).Trim <> "S118_1ZU"
                        kabkling = kabkling + 1
                    Loop
                    Do While strarray(kabmalter).Trim <> "S31 _1ZU"
                        kabmalter = kabmalter + 1
                    Loop
                    Do While strarray(kabcotta).Trim <> "S209_1AB"
                        kabcotta = kabcotta + 1
                    Loop
                    Do While strarray(kdipps).Trim <> "SH2 _1AB"
                        kdipps = kdipps + 1
                    Loop
                    '***
                    Do While strarray(kp).Trim <> "A102_NIE"
                        kp = kp + 1
                    Loop
                End If
                'Console.Out.WriteLine("hallo")

                If zeile > 3 Then
                    Dim strarray() As String = Text.Split(";"c)
                    QSKLI += CDbl(strarray(k_TKLI_QS1)) * 3600.0
                    If (CDbl(strarray(k_mal_wsp)) > WSP_Malter) Then WSP_Malter = CDbl(strarray(k_mal_wsp))
                    If (CDbl(strarray(k_leh_wsp)) > WSP_Lehn) Then WSP_Lehn = CDbl(strarray(k_leh_wsp))
                    If (CDbl(strarray(k_kli_wsp)) > WSP_Kling) Then WSP_Kling = CDbl(strarray(k_kli_wsp))

                    If CDbl(strarray(k_QH1) > 0) Then HWE_LEH = True
                    If CDbl(strarray(k_QH2) > 0) Then HWE_KLI = True
                    If CDbl(strarray(k_QH3) > 0) Then HWE_MAL = True

                    Qzu_Lehn(zeile - 3) = CDbl(strarray(kzulehn))
                    Qzu_Kling(zeile - 3) = CDbl(strarray(kzukling))
                    Qzu_Malter(zeile - 3) = CDbl(strarray(kzumalter))

                    Qab_Lehn(zeile - 3) = CDbl(strarray(kablehn))
                    Qab_Kling(zeile - 3) = CDbl(strarray(kabkling))
                    Qab_Malter(zeile - 3) = CDbl(strarray(kabmalter))

                    Qab_Ammel(zeile - 3) = CDbl(strarray(kaammel)) + CDbl(strarray(ksammel))

                    Qab_Cotta(zeile - 3) = CDbl(strarray(kabcotta))

                    Parray(zeile - 3) = CDbl(strarray(kp))

                    'Berechne Qmax
                    If Qzu_Lehn(zeile - 3) > QmaxLehn Then QmaxLehn = Qzu_Lehn(zeile - 3)
                    If Qzu_Kling(zeile - 3) > QmaxKling Then QmaxKling = Qzu_Kling(zeile - 3)
                    If Qzu_Malter(zeile - 3) > QmaxMalter Then QmaxMalter = Qzu_Malter(zeile - 3)

                    If Qab_Lehn(zeile - 3) > QmaxAbLehn Then QmaxAbLehn = Qab_Lehn(zeile - 3)
                    If Qab_Kling(zeile - 3) > QmaxAbKling Then QmaxAbKling = Qab_Kling(zeile - 3)
                    If Qab_Malter(zeile - 3) > QmaxAbMalter Then QmaxAbMalter = Qab_Malter(zeile - 3)

                    If Qab_Cotta(zeile - 3) > QmaxCotta Then QmaxCotta = Qab_Cotta(zeile - 3)

                    If CDbl(strarray(kdipps)) > QmaxDipps Then QmaxDipps = CDbl(strarray(kdipps))
                    If Qab_Ammel(zeile - 3) > QmaxAmmel Then QmaxAmmel = Qab_Ammel(zeile - 3)

                    'Berechne maximale TS-Füllung 
                    VmaxLehn = Qzu_Lehn(zeile - 3) * 3600.0
                    VmaxKling = Qzu_Kling(zeile - 3) * 3600.0
                    VmaxMalter = Qzu_Malter(zeile - 3) * 3600.0

                    'Berechne Wellenvolumen 
                    If (Qzu_Lehn(zeile - 3) > 5.0) Then VAmmel += Qab_Ammel(zeile - 3) * 3600.0 / 1000000.0
                    If (CDbl(strarray(kdipps)) > 5.0) Then VDipps += CDbl(strarray(kdipps)) * 3600.0 / 1000000.0
                End If

            Loop Until StrRe.Peek() = -1

            StrRe.Close()
            FiStr.Close()

        Catch except As Exception
            MsgBox(except.Message, "Fehler in readWel", MsgBoxStyle.Exclamation)
        End Try



    End Sub



    Public Sub runBluem()
        'BlueM DLL instanzieren
        '----------------------
        Dim dll_path As String
        dll_path = System.Windows.Forms.Application.StartupPath() & "\Apps\BlueM\BlueM.dll"

        If (File.Exists(dll_path)) Then
            bluem_dll = New BlueM_EngineDotNetAccess(dll_path)
        Else
            Throw New Exception("BlueM.dll nicht gefunden!")
        End If

        Try

            Call bluem_dll.Initialize(Me.WorkDir & Me.Datensatz)

            Dim SimEnde As DateTime = BlueM_EngineDotNetAccess.DateTime(bluem_dll.GetSimulationEndDate())

            'Simulationszeitraum 
            Do While (BlueM_EngineDotNetAccess.DateTime(bluem_dll.GetCurrentTime) <= SimEnde)
                Call bluem_dll.PerformTimeStep()
            Loop

            'Simulation erfolgreich
  
        Catch ex As Exception

            'Simulationsfehler aufgetreten
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "BlueM")
  
        Finally

            Call bluem_dll.Finish()
            Call bluem_dll.Dispose()

        End Try

    End Sub

End Class
