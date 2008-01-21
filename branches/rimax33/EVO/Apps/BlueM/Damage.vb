Imports System.IO

Public Class Damage

    Public Function QWert_Damage(ByVal Pfad As String) As Double

        Dim Damage As Double
        Try
            Dim FiStr As FileStream = New FileStream(Pfad, FileMode.Open, FileAccess.ReadWrite)
            Dim StrRe As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
            Dim Text As String
            Dim zeile, k_s26, k_s29, k_s31 As Integer
            Dim k_s206, k_s205, k_s255, k_s204, k_s209, k_s200 As Integer
            Dim k_s111, k_s115, k_s116, k_s118 As Integer
            Dim Qmax26, Qmax29, Qmax31 As Double
            Dim Qmax206, Qmax205, Qmax204, Qmax209, Qmax200 As Double
            Dim Qmax111, Qmax115, Qmax116, Qmax118 As Double
            Dim D26, D29, D31 As Double
            Dim D204, D206, D205, D209, D200 As Double
            Dim D111, D115, D116, D118 As Double
            Dim y1, y2, x1, dx As Double


            ' Dim x() As Double = {20.0, 40.0, 60.0, 80.0, 100.0, 120.0, 140.0, 160.0, 180.0, 200.0, 220.0, 240.0}
            ' Dim D204() As Double = {0.0, 12983.0, 90195.0, 149956.0, 784363.0, 1381044.0, 1682669.0, 3324740.0, 3807618.0, 4233851.0, 4797472.0}

            zeile = 0
            k_s255 = 1 'Zwischenstück wegen Poldereinbau
            k_s26 = 1
            k_s29 = 1
            k_s31 = 1
            k_s206 = 1
            k_s205 = 1
            k_s204 = 1
            k_s209 = 1
            k_s200 = 1
            k_s118 = 1
            k_s116 = 1
            k_s115 = 1
            k_s111 = 1

            'Console.WriteLine("test")

            Do
                Text = StrRe.ReadLine.ToString
                zeile += 1

                If zeile = 2 Then
                    Dim strarray() As String = Text.Split(";"c)

                    Do While strarray(k_s31).Trim <> "S31 _1ZU"
                        k_s31 = k_s31 + 1
                    Loop

                    Do While strarray(k_s26).Trim <> "S26 _1ZU"
                        k_s26 = k_s26 + 1
                    Loop

                    Do While strarray(k_s29).Trim <> "S29 _1ZU"
                        k_s29 = k_s29 + 1
                    Loop

                    Do While strarray(k_s200).Trim <> "S200_1ZU"
                        k_s200 = k_s200 + 1
                    Loop

                    Do While strarray(k_s204).Trim <> "S204_1ZU"
                        k_s204 = k_s204 + 1
                    Loop

                    Do While strarray(k_s205).Trim <> "S205_1ZU"
                        k_s205 = k_s205 + 1
                    Loop

                    Do While strarray(k_s206).Trim <> "S206_1ZU"
                        k_s206 = k_s206 + 1
                    Loop

                    Do While strarray(k_s209).Trim <> "S209_1ZU"
                        k_s209 = k_s209 + 1
                    Loop

                    Do While strarray(k_s111).Trim <> "S111_1ZU"
                        k_s111 = k_s111 + 1
                    Loop

                    Do While strarray(k_s115).Trim <> "S115_1ZU"
                        k_s115 = k_s115 + 1
                    Loop

                    Do While strarray(k_s116).Trim <> "S116_1ZU"
                        k_s116 = k_s116 + 1
                    Loop

                    Do While strarray(k_s118).Trim <> "S118_1ZU"
                        k_s118 = k_s118 + 1
                    Loop
                End If

                If zeile > 3 Then
                    Dim strarray() As String = Text.Split(";"c)
                    If (CDbl(strarray(k_s31)) > Qmax31) Then Qmax31 = CDbl(strarray(k_s31))
                    If (CDbl(strarray(k_s26)) > Qmax26) Then Qmax26 = CDbl(strarray(k_s26))
                    If (CDbl(strarray(k_s29)) > Qmax29) Then Qmax29 = CDbl(strarray(k_s29))
                    If (CDbl(strarray(k_s200)) > Qmax200) Then Qmax200 = CDbl(strarray(k_s200))
                    If (CDbl(strarray(k_s204)) > Qmax204) Then Qmax204 = CDbl(strarray(k_s204))
                    If (CDbl(strarray(k_s205)) > Qmax205) Then Qmax205 = CDbl(strarray(k_s205))
                    'If (CDbl(strarray(k_s255)) > Qmax255) Then Qmax255 = CDbl(strarray(k_s255))
                    If (CDbl(strarray(k_s206)) > Qmax206) Then Qmax206 = CDbl(strarray(k_s206))
                    If (CDbl(strarray(k_s209)) > Qmax209) Then Qmax209 = CDbl(strarray(k_s209))
                    If (CDbl(strarray(k_s111)) > Qmax111) Then Qmax111 = CDbl(strarray(k_s111))
                    If (CDbl(strarray(k_s115)) > Qmax115) Then Qmax115 = CDbl(strarray(k_s115))
                    If (CDbl(strarray(k_s116)) > Qmax116) Then Qmax116 = CDbl(strarray(k_s116))
                    If (CDbl(strarray(k_s118)) > Qmax118) Then Qmax118 = CDbl(strarray(k_s118))
                End If

                Qmax200 = Math.Max(Qmax200, Qmax209) 'S200 und S209 bei Schadensbetrachtung zusammengefasst


            Loop Until StrRe.Peek() = -1

            'Qmax_209pub = Qmax209
            'Qmax_31pub = Qmax31

            StrRe.Close()
            FiStr.Close()

            '*** S111 *************************

            dx = 20.0

            If (Qmax111 >= 40 And Qmax111 < 60) Then
                x1 = 40
                y1 = 0
                y2 = 15719
            ElseIf (Qmax111 >= 60 And Qmax111 < 80) Then
                x1 = 60
                y1 = 15719
                y2 = 52057
            ElseIf (Qmax111 >= 80 And Qmax111 < 100) Then
                x1 = 80
                y1 = 52057
                y2 = 117957
            ElseIf (Qmax111 >= 100 And Qmax111 < 120) Then
                x1 = 100
                y1 = 117957
                y2 = 260892
            ElseIf (Qmax111 >= 120 And Qmax111 < 140) Then
                x1 = 120
                y1 = 260892
                y2 = 383053
            ElseIf (Qmax111 >= 140 And Qmax111 < 160) Then
                x1 = 140
                y1 = 383053
                y2 = 559475
            ElseIf (Qmax111 >= 160 And Qmax111 < 180) Then
                x1 = 160
                y1 = 559475
                y2 = 684339
            ElseIf (Qmax111 >= 180 And Qmax111 < 200) Then
                x1 = 180
                y1 = 684339
                y2 = 862142
            ElseIf (Qmax111 >= 200 And Qmax111 < 220) Then
                x1 = 200
                y1 = 862142
                y2 = 1040910
            ElseIf (Qmax111 >= 220 And Qmax111 < 240) Then
                x1 = 220
                y1 = 1040910
                y2 = 1147466
            End If

            If Qmax111 > 40 Then D111 = (y2 - y1) / dx * Qmax111 + y1 - (y2 - y1) / dx * x1

            '*** S115 *************************

            dx = 20.0

            If (Qmax115 >= 40 And Qmax115 < 60) Then
                x1 = 40
                y1 = 0
                y2 = 47446
            ElseIf (Qmax115 >= 60 And Qmax115 < 80) Then
                x1 = 60
                y1 = 47446
                y2 = 114003
            ElseIf (Qmax115 >= 80 And Qmax115 < 100) Then
                x1 = 80
                y1 = 114003
                y2 = 148714
            ElseIf (Qmax115 >= 100 And Qmax115 < 120) Then
                x1 = 100
                y1 = 148714
                y2 = 262328
            ElseIf (Qmax115 >= 120 And Qmax115 < 140) Then
                x1 = 120
                y1 = 262328
                y2 = 504107
            ElseIf (Qmax115 >= 140 And Qmax115 < 160) Then
                x1 = 140
                y1 = 504107
                y2 = 888925
            ElseIf (Qmax115 >= 160 And Qmax115 < 180) Then
                x1 = 160
                y1 = 888925
                y2 = 1163739
            ElseIf (Qmax115 >= 180 And Qmax115 < 200) Then
                x1 = 180
                y1 = 1163739
                y2 = 1740539
            ElseIf (Qmax115 >= 200 And Qmax115 < 220) Then
                x1 = 200
                y1 = 1740539
                y2 = 2177980
            ElseIf (Qmax115 >= 220 And Qmax115 < 240) Then
                x1 = 220
                y1 = 2177980
                y2 = 2364356
            End If

            If Qmax115 > 40 Then D115 = (y2 - y1) / dx * Qmax115 + y1 - (y2 - y1) / dx * x1

            'S116 Schadenssumme vernachlässigbar klein

            '*** S118 *************************

            dx = 20.0

            If (Qmax118 >= 40 And Qmax118 < 60) Then
                x1 = 40
                y1 = 31400
                y2 = 235759
            ElseIf (Qmax118 >= 60 And Qmax118 < 80) Then
                x1 = 60
                y1 = 235759
                y2 = 474915
            ElseIf (Qmax118 >= 80 And Qmax118 < 100) Then
                x1 = 80
                y1 = 474915
                y2 = 610229
            ElseIf (Qmax118 >= 100 And Qmax118 < 120) Then
                x1 = 100
                y1 = 610229
                y2 = 714157
            ElseIf (Qmax118 >= 120 And Qmax118 < 140) Then
                x1 = 120
                y1 = 714157
                y2 = 857550
            ElseIf (Qmax118 >= 140 And Qmax118 < 160) Then
                x1 = 140
                y1 = 857550
                y2 = 1092831
            ElseIf (Qmax118 >= 160 And Qmax118 < 180) Then
                x1 = 160
                y1 = 1092831
                y2 = 1150456
            ElseIf (Qmax118 >= 180 And Qmax118 < 200) Then
                x1 = 180
                y1 = 1150456
                y2 = 1242932
            ElseIf (Qmax118 >= 200 And Qmax118 < 220) Then
                x1 = 200
                y1 = 1242932
                y2 = 1328038
            ElseIf (Qmax118 >= 220 And Qmax118 < 240) Then
                x1 = 220
                y1 = 1328038
                y2 = 1433795
            End If

            If Qmax118 > 40 Then D118 = (y2 - y1) / dx * Qmax118 + y1 - (y2 - y1) / dx * x1

            If Qmax118 > 240 Then D118 = 1500000


            '*** S204 *************************

            dx = 20.0

            If (Qmax204 >= 20 And Qmax204 < 40) Then
                x1 = 20
                y1 = 0
                y2 = 5556
            ElseIf (Qmax204 >= 40 And Qmax204 < 60) Then
                x1 = 40
                y1 = 5556
                y2 = 12983
            ElseIf (Qmax204 >= 60 And Qmax204 < 80) Then
                x1 = 60
                y1 = 12983
                y2 = 90195
            ElseIf (Qmax204 >= 80 And Qmax204 < 100) Then
                x1 = 80
                y1 = 90195
                y2 = 149956
            ElseIf (Qmax204 >= 100 And Qmax204 < 120) Then
                x1 = 100
                y1 = 149956
                y2 = 7884363
            ElseIf (Qmax204 >= 120 And Qmax204 < 140) Then
                x1 = 120
                y1 = 7884363
                y2 = 1381044
            ElseIf (Qmax204 >= 140 And Qmax204 < 160) Then
                x1 = 140
                y1 = 1381044
                y2 = 1682669
            ElseIf (Qmax204 >= 160 And Qmax204 < 180) Then
                x1 = 160
                y1 = 1682669
                y2 = 3324739
            ElseIf (Qmax204 >= 180 And Qmax204 < 200) Then
                x1 = 180
                y1 = 3324739
                y2 = 3807618
            ElseIf (Qmax204 >= 200 And Qmax204 < 220) Then
                x1 = 200
                y1 = 3807618
                y2 = 4233850
            ElseIf (Qmax204 >= 220 And Qmax204 < 240) Then
                x1 = 220
                y1 = 4233850
                y2 = 4797472
            ElseIf (Qmax204 >= 240 And Qmax204 < 300) Then
                dx = 240.0
                x1 = 220
                y1 = 4797472
                y2 = 8010994
            ElseIf (Qmax204 >= 300 And Qmax204 < 400) Then
                dx = 300.0
                x1 = 220
                y1 = 8010994
                y2 = 11414730
            ElseIf (Qmax204 >= 400 And Qmax204 < 500) Then
                dx = 400.0
                x1 = 220
                y1 = 11414730
                y2 = 14560616
            ElseIf (Qmax204 >= 500 And Qmax204 < 600) Then
                dx = 500.0
                x1 = 220
                y1 = 14560616
                y2 = 16741791
            ElseIf (Qmax204 >= 600 And Qmax204 < 1000) Then
                dx = 400.0
                x1 = 600
                y1 = 16741791
                y2 = 22344275
            End If

            If Qmax204 > 20 Then D204 = (y2 - y1) / dx * Qmax204 + y1 - (y2 - y1) / dx * x1

            '**** S200 Dresden 2d-Modell ***********************************************************

            dx = 20.0


            If (Qmax200 >= 100 And Qmax200 < 120) Then
                x1 = 100
                y1 = 221134
                y2 = 1040317
            ElseIf (Qmax200 >= 120 And Qmax200 < 140) Then
                x1 = 120
                y1 = 1040317
                y2 = 1879550
            ElseIf (Qmax200 >= 140 And Qmax200 < 180) Then
                dx = 40.0
                x1 = 140
                y1 = 1879550
                y2 = 17200876
            ElseIf (Qmax200 >= 180 And Qmax200 < 200) Then
                x1 = 180
                y1 = 17200876
                y2 = 21028539
            ElseIf (Qmax200 >= 200 And Qmax200 < 240) Then
                dx = 40.0
                x1 = 200
                y1 = 21028539
                y2 = 31606047
            ElseIf (Qmax200 >= 240 And Qmax200 < 260) Then
                x1 = 240
                y1 = 31606047
                y2 = 31910600
            ElseIf (Qmax200 >= 260 And Qmax200 < 300) Then
                dx = 40.0
                x1 = 260
                y1 = 31910600
                y2 = 52298495
            ElseIf (Qmax200 >= 300 And Qmax200 < 400) Then
                dx = 100.0
                x1 = 300
                y1 = 52298495
                y2 = 70335770
            ElseIf (Qmax200 >= 400 And Qmax200 < 477) Then
                dx = 77.0
                x1 = 400
                y1 = 70335770
                y2 = 76719335
            ElseIf (Qmax200 >= 477 And Qmax200 < 600) Then
                dx = 123.0
                x1 = 477
                y1 = 76719335
                y2 = 84363049
            ElseIf (Qmax200 >= 600 And Qmax200 < 800) Then
                dx = 200.0
                x1 = 600
                y1 = 84363049
                y2 = 97394199
            ElseIf (Qmax200 >= 800 And Qmax200 < 1000) Then
                dx = 200.0
                x1 = 800
                y1 = 97394199
                y2 = 110000000 'extrapoliert!
            End If

            If Qmax200 > 100 Then D200 = (y2 - y1) / dx * Qmax200 + y1 - (y2 - y1) / dx * x1

            '**** S205 vor Pl. Grund 1D *******************************************************

            dx = 20.0

            If (Qmax205 >= 20 And Qmax205 < 40) Then
                x1 = 20.0
                y1 = 0.0
                y2 = 151.0
            ElseIf (Qmax205 >= 40 And Qmax205 < 60) Then
                x1 = 40.0
                y1 = 151.0
                y2 = 1023.0
            ElseIf (Qmax205 >= 60 And Qmax205 < 80) Then
                x1 = 60.0
                y1 = 1023.0
                y2 = 3427.0
            ElseIf (Qmax205 >= 80 And Qmax205 < 100) Then
                x1 = 80.0
                y1 = 3427.0
                y2 = 8931.0
            ElseIf (Qmax205 >= 100 And Qmax205 < 120) Then
                x1 = 100.0
                y1 = 8931.0
                y2 = 26209.0
            ElseIf (Qmax205 >= 120 And Qmax205 < 140) Then
                x1 = 120.0
                y1 = 26209.0
                y2 = 67722.0
            ElseIf (Qmax205 >= 140 And Qmax205 < 160) Then
                x1 = 140.0
                y1 = 67722.0
                y2 = 155214.0
            ElseIf (Qmax205 >= 160 And Qmax205 < 180) Then
                x1 = 160.0
                y1 = 155214.0
                y2 = 360644.0
            ElseIf (Qmax205 >= 180 And Qmax205 < 200) Then
                x1 = 180.0
                y1 = 360644.0
                y2 = 680726.0
            ElseIf (Qmax205 >= 200 And Qmax205 < 220) Then
                x1 = 200.0
                y1 = 680726.0
                y2 = 1747273.0
            ElseIf (Qmax205 >= 220 And Qmax205 < 300) Then
                dx = 80.0
                x1 = 220.0
                y1 = 1747273.0
                y2 = 4414731
            ElseIf (Qmax205 >= 300 And Qmax205 < 400) Then
                dx = 100.0
                x1 = 300
                y1 = 4414731
                y2 = 7591105
            ElseIf (Qmax205 >= 400 And Qmax205 < 500) Then
                dx = 100.0
                x1 = 400
                y1 = 7591105
                y2 = 8800384
            ElseIf (Qmax205 >= 500 And Qmax205 < 600) Then
                dx = 100.0
                x1 = 500
                y1 = 8800384
                y2 = 9901490
            ElseIf (Qmax205 >= 600 And Qmax205 < 1000) Then
                dx = 400.0
                x1 = 600
                y1 = 9901490
                y2 = 13750500
            End If

            If Qmax205 > 20 Then D205 = (y2 - y1) / dx * Qmax205 + y1 - (y2 - y1) / dx * x1

            '**** S206 ***************************************************************************

            dx = 20.0

            If (Qmax206 >= 120 And Qmax206 < 140) Then
                x1 = 120.0
                y1 = 0.0
                y2 = 43936.0
            ElseIf (Qmax206 >= 140 And Qmax206 < 160) Then
                x1 = 140.0
                y1 = 43936.0
                y2 = 62007.0
            ElseIf (Qmax206 >= 160 And Qmax206 < 200) Then
                dx = 40.0
                x1 = 160.0
                y1 = 62007.0
                y2 = 105625.0
            ElseIf (Qmax206 >= 200 And Qmax206 < 220) Then
                x1 = 200.0
                y1 = 105625.0
                y2 = 780687.0
            ElseIf (Qmax206 >= 220 And Qmax206 < 300) Then 'nur bis max 240 gerechnet!
                x1 = 220.0
                dx = 80
                y1 = 780687.0
                y2 = 2243323
            ElseIf (Qmax206 >= 300 And Qmax206 < 400) Then 'nur bis max 240 gerechnet!
                x1 = 300.0
                dx = 100
                y1 = 2243323.0
                y2 = 3857381
            ElseIf (Qmax206 >= 400 And Qmax206 < 500) Then 'nur bis max 240 gerechnet!
                x1 = 400.0
                dx = 100
                y1 = 3857381.0
                y2 = 4471870
            ElseIf (Qmax206 >= 500 And Qmax206 < 600) Then 'nur bis max 240 gerechnet!
                x1 = 500.0
                dx = 100
                y1 = 4471870.0
                y2 = 5031391
            ElseIf (Qmax206 >= 600 And Qmax206 < 1000) Then 'nur bis max 240 gerechnet!
                x1 = 600.0
                dx = 400
                y1 = 5031391.0
                y2 = 6987246
            End If

            If Qmax206 > 120 Then D206 = (y2 - y1) / dx * Qmax206 + y1 - (y2 - y1) / dx * x1

            '**** S26 ***************************************************************************

            dx = 20.0

            If (Qmax26 >= 20 And Qmax26 < 40) Then
                x1 = 20.0
                y1 = 0.0
                y2 = 15396.0
            ElseIf (Qmax26 >= 40 And Qmax26 < 60) Then
                x1 = 40.0
                y1 = 15396.0
                y2 = 155725.0
            ElseIf (Qmax26 >= 60 And Qmax26 < 80) Then
                x1 = 60.0
                y1 = 155725.0
                y2 = 354221.0
            ElseIf (Qmax26 >= 80 And Qmax26 < 100) Then
                x1 = 80.0
                y1 = 354221.0
                y2 = 594610.0
            ElseIf (Qmax26 >= 100 And Qmax26 < 120) Then
                x1 = 100.0
                y1 = 594610.0
                y2 = 845576.0
            ElseIf (Qmax26 >= 120 And Qmax26 < 140) Then
                x1 = 120.0
                y1 = 845576.0
                y2 = 1083662.0
            ElseIf (Qmax26 >= 140 And Qmax26 < 160) Then
                x1 = 140.0
                y1 = 1083662.0
                y2 = 2034517.0
            ElseIf (Qmax26 >= 160 And Qmax26 < 180) Then
                x1 = 160.0
                y1 = 2034517.0
                y2 = 2165292.0
            ElseIf (Qmax26 >= 180 And Qmax26 < 200) Then
                x1 = 180.0
                y1 = 2165292.0
                y2 = 2583038.0
            ElseIf (Qmax26 >= 200 And Qmax26 < 220) Then
                x1 = 200.0
                y1 = 2583038.0
                y2 = 2927183.0
            ElseIf (Qmax26 >= 220 And Qmax26 < 240) Then
                x1 = 220.0
                y1 = 2927183.0
                y2 = 3223547.0
            ElseIf (Qmax26 >= 240 And Qmax26 < 260) Then
                x1 = 240.0
                y1 = 3223547.0
                y2 = 3731913.0
            ElseIf (Qmax26 >= 260 And Qmax26 < 280) Then
                x1 = 260.0
                y1 = 3731913.0
                y2 = 3930000.0
            ElseIf (Qmax26 >= 280 And Qmax26 < 350) Then
                x1 = 280.0
                y1 = 3930000.0
                y2 = 4500000.0
            End If

            If Qmax26 > 20 Then D26 = (y2 - y1) / dx * Qmax26 + y1 - (y2 - y1) / dx * x1

            '**** S29 ***************************************************************************
            'geprüft!

            If (Qmax29 >= 220) Then D29 = -0.48009455 * Qmax29 ^ 2 + 672.7465198 * Qmax29 - 49530.52186239

            '**** S31 ***************************************************************************
            'geprüft!

            If (Qmax31 >= 40) Then D31 = 0.00022 * Qmax31 ^ 4 - 0.156622 * Qmax31 ^ 3 + 35.475182 * Qmax31 ^ 2 - 956.380786 * Qmax31 - 10845.274214

            '**** Gesamtschaden ***************************************************************************

            'Console.WriteLine(D29.ToString + Chr(9) + D31.ToString + Chr(9) + D26.ToString + Chr(9) + D200.ToString + Chr(9) + _
            '               D204.ToString + Chr(9) + D205.ToString + Chr(9) + D206.ToString + Chr(9) + D209.ToString + Chr(9) + D111.ToString + Chr(9) + _
            '                  D115.ToString + Chr(9) + D116.ToString + Chr(9) + D118.ToString)
            'Console.WriteLine(Qmax29.ToString + Chr(9) + Qmax31.ToString + Chr(9) + Qmax26.ToString + Chr(9) + Qmax200.ToString + Chr(9) + _
            '                   Qmax204.ToString + Chr(9) + Qmax205.ToString + Chr(9) + Qmax206.ToString + Chr(9) + Qmax209.ToString + Chr(9) + Qmax111.ToString + Chr(9) + _
            '                Qmax115.ToString + Chr(9) + Qmax116.ToString + Chr(9) + Qmax118.ToString)


            Damage = D29 + D31 + D26 + D200 + D204 + D205 + D206 + D209 + D111 + D115 + D116 + D118
            'damage_ww = D111 + D115 + D116 + D118
            'damage_rw = D29 + D31 + D26
            ' damage_vw = D200 + D204 + D205 + D206 + D209

            '*** check Schadensfunktion: Q - D - Beziehung
            'Console.WriteLine(nrun.ToString + Chr(9) + D29.ToString + Chr(9) + Qmax29.ToString + Chr(9) + D31.ToString + Chr(9) + Qmax31.ToString + Chr(9) + D26.ToString + Chr(9) + Qmax26.ToString + Chr(9) + D200.ToString + Chr(9) + Qmax200.ToString + Chr(9) + D204.ToString + Chr(9) + Qmax204.ToString + Chr(9) + D205.ToString + Chr(9) + Qmax205.ToString + Chr(9) + D206.ToString + Chr(9) + Qmax206.ToString + Chr(9) + D209.ToString + Chr(9) + Qmax209.ToString + Chr(9) + D111.ToString + Chr(9) + Qmax111.ToString + Chr(9) + D115.ToString + Chr(9) + Qmax115.ToString + Chr(9) + D116.ToString + Chr(9) + Qmax116.ToString + Chr(9) + D118.ToString + Chr(9) + Qmax118.ToString)

        Catch except As Exception
            MsgBox(except.Message, "Fehler in readWel", MsgBoxStyle.Exclamation)
        End Try


        Return Damage

    End Function


    Public Sub get_damage(ByVal Pfad1 As String, ByVal AnwName1 As String)


        

    End Sub

End Class
