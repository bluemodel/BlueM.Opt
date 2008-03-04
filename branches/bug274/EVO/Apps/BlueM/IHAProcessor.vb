Public Class IHAProcessor

    '***************************************************************************************
    '***************************************************************************************
    '**** Klasse IHAProcessor zur Verarbeitung von Ergebnissen von IHA-Analysen         ****
    '****                                                                               ****
    '**** Autoren: Felix Froehlich                                                      ****
    '****                                                                               ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung                      ****
    '**** TU Darmstadt                                                                  ****
    '***************************************************************************************
    '***************************************************************************************

#Region "Eigenschaften"

    'Eigenschaften
    '#############

    Public Structure Struct_RVAfx                       'Structure zum Speichern der fx Werte eines RVA-Ergebnisses
        Dim All_Avg_fx As Double
        Dim PGroup_Avg_fx() As Double
    End Structure

    Public isComparison As Boolean                      'Vergleich von IHA-Ergebnissen?
    Friend RVABase As Wave.RVA.Struct_RVAValues         'RVA-Basiswerte für den Vergleich
    Private RVAfxBase As Struct_RVAfx                   'fx-Basiswerte für den Vergleich

#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

    'Konstruktor
    '***********
    Public Sub New()

        Me.isComparison = False

    End Sub

    'IHA-Processor Modus auf Vergleich von IHA-Analysen setzen
    '*********************************************************
    Public Sub setComparisonMode(ByVal RVAResultBase As Wave.RVA.Struct_RVAValues)

        Me.isComparison = True

        Me.RVABase = RVAResultBase
        Me.RVABase.Title = "Referenz"
        Me.RVAfxBase = calcfx(Me.RVABase)

    End Sub

    'QWert aus IHA-Ergebnissen berechnen
    '***********************************
    Public Function QWert_IHA(ByVal OptZiel As Kern.OptZiel, ByVal RVAResult As Wave.RVA.Struct_RVAValues) As Double

        Dim QWert As Double
        Dim i As Integer
        Dim RVAfx As Struct_RVAfx

        'fx-Werte berechnen
        '------------------
        RVAfx = calcfx(RVAResult)

        Select Case Me.isComparison

            Case False
                'Direkte QWert-Berechnung
                '========================
                Dim fx As Double

                If (OptZiel.ZielFkt = "") Then
                    'fx(HA) Gesamtmittelwert
                    fx = RVAfx.All_Avg_fx

                Else
                    'fx(HA) Mittelwert einer Parametergruppe
                    For i = 0 To RVAResult.NGroups - 1
                        If (OptZiel.ZielFkt = RVAResult.IHAParamGroups(i).GName) Then
                            fx = RVAfx.PGroup_Avg_fx(i)
                            Exit For
                        End If
                    Next
                End If

                QWert = 1 - fx

            Case True
                'RVA-Vergleich
                '=============
                Dim diff As Double

                If (OptZiel.ZielFkt = "") Then
                    'fx(HA) Gesamtmittelwert
                    diff = RVAfx.All_Avg_fx - Me.RVAfxBase.All_Avg_fx
                Else
                    'fx(HA) Mittelwert einer Parametergruppe
                    For i = 0 To RVAResult.NGroups - 1
                        If (OptZiel.ZielFkt = RVAResult.IHAParamGroups(i).GName) Then
                            diff = RVAfx.PGroup_Avg_fx(i) - Me.RVAfxBase.PGroup_Avg_fx(i)
                            Exit For
                        End If
                    Next
                End If

                QWert = (1 - diff) ^ 2

        End Select

        Return QWert

    End Function

    'fx-Werte für ein RVA-Ergebnis berechnen
    '***************************************
    Private Shared Function calcfx(ByVal RVAResult As Wave.RVA.Struct_RVAValues) As Struct_RVAfx

        Dim All_Sum_fx, PGroup_Sum_fx() As Double
        Dim RVAfx As Struct_RVAfx
        Dim i, j As Integer

        ReDim PGroup_Sum_fx(RVAResult.NGroups - 1)
        ReDim RVAfx.PGroup_Avg_fx(RVAResult.NGroups - 1)

        All_Sum_fx = 0
        'Schleife über Parametergruppen
        For i = 0 To RVAResult.NGroups - 1

            PGroup_Sum_fx(i) = 0
            'Schleife über Parameter
            For j = 0 To RVAResult.IHAParamGroups(i).NParams - 1
                PGroup_Sum_fx(i) += fx(RVAResult.IHAParamGroups(i).IHAParams(j).HAMiddle)
            Next

            'Mittelwert für eine Parametergruppe
            RVAfx.PGroup_Avg_fx(i) = PGroup_Sum_fx(i) / RVAResult.IHAParamGroups(i).NParams

            All_Sum_fx += RVAfx.PGroup_Avg_fx(i)
        Next

        'Mittelwert über alle Parametergruppen
        RVAfx.All_Avg_fx = All_Sum_fx / RVAResult.NGroups

        Return RVAfx

    End Function

    'Transformiert einen HA-Wert [-1:infinity] in einen normalverteilten Funktionswert [0:1]
    '***************************************************************************************
    Private Shared Function fx(ByVal HA As Double) As Double

        'Parameter für Normalverteilung mit fx(0) ~= 1 und fx(-1) = fx(1) ~= 0
        '[EXCEL:] 1/(std*WURZEL(2*PI()))*EXP(-1/2*((X-avg)/std)^2)
        Dim std As Double = 0.398942423706863                   'Standardabweichung
        Dim avg As Double = 0                                   'Erwartungswert

        fx = 1 / (std * Math.Sqrt(2 * Math.PI)) * Math.Exp(-1 / 2 * ((HA - avg) / std) ^ 2)

        Return fx

    End Function

#End Region 'Methoden

End Class
