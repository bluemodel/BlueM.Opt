'*******************************************************************************
'*******************************************************************************
'**** Klasse SKos zur Ermittlung der Schäden und Kosten im HW Fall          ****
'****                                                                       ****
'**** Christoph Hübner                                                      ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'****                                                                       ****
'**** Februar 2007                                                          ****
'****                                                                       ****
'**** Letzte Änderung: April 2007                                           ****
'*******************************************************************************
'*******************************************************************************

Imports System.IO

Public Class ObjectiveFunction_SKos
    Inherits ObjectiveFunction

    ''' <summary>
    ''' Returns the type of the ObjectiveFunction
    ''' </summary>
    Public Overrides ReadOnly Property GetObjType() As ObjectiveType
        Get
            Return ObjectiveType.SKos
        End Get
    End Property

    'Das Problem
    Private mProblem As EVO.Common.Problem

    'Aktuelles Working Directory (wichtig bei Multithreading)
    Public Akt_Elemente() As String
    Public WorkDir_Current As String
    Public Akt_Path() As Integer

    'Struktur welche Informationen der Transportstrecken enthält
    '***********************************************************
    Private Structure TRS

        Dim Name As String
        Dim Laenge As Double
        Dim DeltaVolume As Double
        Dim Costs As Double

        Dim A0_Hoehe_Sohle As Double
        Dim A1_Breite_Sohle_L As Double
        Dim A2_Breite_Sohle_R As Double

        Dim B0_Hoehe_Gerinne As Double
        Dim B1_Breite_Ufer_L As Double
        Dim B2_Breite_Ufer_R As Double

        Dim C0_Hoehe_FPlain As Double
        Dim C1_Breite_Vorl_L As Double
        Dim C2_Breite_Vorl_R As Double

        Dim D0_Hoehe_Rand As Double
        Dim D1_Breite_Rand_L As Double
        Dim D2_Breite_Rand_R As Double


    End Structure

    Dim TRS_Orig(-1) As TRS
    Dim TRS_Akt(-1) As TRS

    ''' <summary>
    ''' SKos initialisieren
    ''' </summary>
    ''' <param name="prob">Das Problem</param>
    Public Sub initialize(ByRef prob As EVO.Common.Problem)

        'Referenz zu Problem speichern
        Me.mProblem = prob

        'Original Transportstrecken einlesen
        Call Me.Read_TRS_Orig_Daten()

    End Sub

    ''' <summary>
    ''' Calculate the objective function value
    ''' </summary>
    ''' <param name="SimErgebnis">collection of simulation results</param>
    ''' <returns>objective function value</returns>
    Public Overrides Function calculateObjective(ByVal SimErgebnis As SimErgebnis_Structure) As Double

        'SimErgebnis wird nicht genutzt
        Return Me.Calculate_Costs()

    End Function

    'Funktion für die Kalkulation der Kosten
    '***************************************
    Private Function Calculate_Costs() As Double

        Dim CostsFromCES As Boolean = False
        '* defniert ob die Kosten aus der CES Datei berechnet werden

        Dim costs As Double = 0
        Dim i, j As Integer
    

        If CostsFromCES Then

            For i = 0 To Me.mProblem.List_Locations.GetUpperBound(0)
                costs += Me.mProblem.List_Locations(i).List_Massnahmen(Akt_Path(i)).Kosten
            Next

        Else
            Dim Elementliste(0, 1) As Object
            Dim TRS_Array(,) As Object = {}
            Dim TAL_Array(,) As Object = {}

            'Bauwerksliste wird erstellt
            Call create_Elementliste(Elementliste)

            'Ermitteln der massgeblichen Größen aus den Dateien
            Call Read_TAL(TAL_Array, Me.WorkDir_Current)
            Call Read_TRS_Daten(TRS_Akt, Me.WorkDir_Current)

            'Berechnen der Volumen Differenzen aus der Original TRS und der Aktuellen TRS
            Call Calc_Volume(TRS_Orig, TRS_Akt)

            'Kalkulieren der Kosten für jedes Bauwerk
            Call Acquire_Costs(TAL_Array, TRS_Akt, Elementliste)

            'Kosten aufsummieren
            For i = 0 To Akt_Elemente.GetUpperBound(0)
                For j = 0 To Elementliste.GetUpperBound(0)
                    If Elementliste(j, 0) = Akt_Elemente(i) Then
                        costs = costs + Elementliste(j, 1)
                    End If
                Next
            Next
        End If

        Return costs
    End Function

    'Funktion zum erstellen der Elementliste
    'Alle Elemente aus der CES datei werden hier in die Liste gesetzt
    '****************************************************************
    Private Sub create_Elementliste(ByRef Bauwerksliste(,) As Object)

        Dim Bauwerks_Array() As String = {}

        'Kopiert die Bauwerke aus dem BlueM
        Dim i, j, k As Integer
        Dim x As Integer = 0
        For i = 0 To Me.mProblem.List_Locations.GetUpperBound(0)
            For j = 0 To Me.mProblem.List_Locations(i).List_Massnahmen.GetUpperBound(0)
                For k = 0 To Me.mProblem.List_Locations(i).List_Massnahmen(j).Bauwerke.GetUpperBound(0)
                    If Me.mProblem.List_Locations(i).List_Massnahmen(j).KostenTyp = 1 Then
                        System.Array.Resize(Bauwerks_Array, x + 1)
                        Bauwerks_Array(x) = Me.mProblem.List_Locations(i).List_Massnahmen(j).Bauwerke(k)
                        x += 1
                    End If
                Next
            Next
        Next

        Call Remove_X(Bauwerks_Array)

        'Die Werte des Arrays werden an die Liste übertragen
        ReDim Bauwerksliste(Bauwerks_Array.GetUpperBound(0), 1)
        For i = 0 To Bauwerks_Array.GetUpperBound(0)
            Bauwerksliste(i, 0) = Bauwerks_Array(i)
        Next
    End Sub

    'Hilfsfunktion: Die "X" Einträge werden entfernt
    '***********************************************
    Private Sub Remove_X(ByRef Array As String())
        Dim x As Integer
        Dim i As Integer
        Dim TmpArray(Array.GetUpperBound(0)) As String
        System.Array.Copy(Array, TmpArray, Array.GetLength(0))
        x = 0
        For i = 0 To TmpArray.GetUpperBound(0)
            If Not TmpArray(i) = "X" Then
                Array(x) = TmpArray(i)
                x += 1
            End If
        Next
        System.Array.Resize(Array, x)
    End Sub

    'Informationen der Original Transportstrecken einlesen
    '*****************************************************
    Private Sub Read_TRS_Orig_Daten()

        Call Read_TRS_Daten(TRS_Orig, Me.mProblem.WorkDir)

    End Sub


    'Inforationen der Transportstrecken einlesen
    'Hier werden nur die Informationen einer Seite eingelesenund Symerie angenommen (könnte mann leicht erweitern)
    '*************************************************************************************************************
    Private Sub Read_TRS_Daten(ByRef TRS_Array() As TRS, ByRef WorkDir As String)

        ReDim TRS_Array(-1)

        'Dim TRS_Array(,) As Object = {}
        Dim Datei As String = WorkDir & Me.mProblem.Datensatz & ".TRS"

        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        Dim Zeile As String
        Dim Anz As Integer = 0

        'Anzahl der Zeilen feststellen
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                Anz += 1
            End If
        Loop Until StrRead.Peek() = -1

        Dim i As Integer = -1
        Dim j As Integer = -1
        Dim count As Integer = 0

        'Zurück zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        Dim Array1() As String
        Dim Array2(6) As String

        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                Array1 = Zeile.Split("|")
                Array2(0) = Array1(6).Substring(1, 1).Trim
                Array2(1) = Array1(6).Substring(3, 6).Trim
                Array2(2) = Array1(6).Substring(9, 6).Trim
                Array2(3) = Array1(6).Substring(15, 5).Trim
                Array2(4) = Array1(6).Substring(21, 6).Trim
                Array2(5) = Array1(6).Substring(27, 5).Trim
                Array2(6) = Array1(6).Substring(33, 5).Trim

                'Werte zuweisen
                'Prüfen ob neues Element
                If Array1(1).Trim() <> "" And Array1(2).Trim = "3" Then
                    j += 1
                    count = 0
                    ReDim Preserve TRS_Array(TRS_Array.GetLength(0))
                    TRS_Array(j).Name = Array1(1).Trim()
                    TRS_Array(j).Laenge = Array1(3).Trim()
                    TRS_Array(j).A0_Hoehe_Sohle = Array2(1)
                    TRS_Array(j).A1_Breite_Sohle_L = Array2(2)
                    TRS_Array(j).A2_Breite_Sohle_R = Array2(4)

                ElseIf Array1(1).Trim() = "" Then
                    count += 1
                End If

                If count = 1 Then
                    TRS_Array(j).B0_Hoehe_Gerinne = Array2(1)
                    TRS_Array(j).B1_Breite_Ufer_L = Array2(2)
                    TRS_Array(j).B2_Breite_Ufer_R = Array2(4)
                ElseIf count = 2 Then
                    TRS_Array(j).C0_Hoehe_FPlain = Array2(1)
                    TRS_Array(j).C1_Breite_Vorl_L = Array2(2)
                    TRS_Array(j).C2_Breite_Vorl_R = Array2(4)
                ElseIf count = 3 Then
                    TRS_Array(j).D0_Hoehe_Rand = Array2(1)
                    TRS_Array(j).D1_Breite_Rand_L = Array2(2)
                    TRS_Array(j).D2_Breite_Rand_R = Array2(4)
                    count = 0
                End If
            End If
        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

    End Sub

    'Verändertes Volumen und Kosten der Transportstrecken berechnen
    '**************************************************************
    Private Sub Calc_Volume(ByVal TRS_Orig() As TRS, ByVal TRS_Act() As TRS)

        Dim i As Integer
        Dim L1, L2, H1, H2, F1, F2 As Double
        Dim FLinks_orig, FLinks_Act, FRechts_orig, FRechts_Act, Delta_L, Delta_R, Delta_Ges As Double

        For i = 0 To TRS_Orig.GetUpperBound(0)

            'Linke Seite Original
            '********************
            L1 = TRS_Orig(i).C1_Breite_Vorl_L - TRS_Orig(i).B1_Breite_Ufer_L
            L2 = TRS_Orig(i).D1_Breite_Rand_L - TRS_Orig(i).C1_Breite_Vorl_L
            H1 = (TRS_Orig(i).B0_Hoehe_Gerinne + TRS_Orig(i).C0_Hoehe_FPlain) / 2
            H2 = (TRS_Orig(i).C0_Hoehe_FPlain + TRS_Orig(i).D0_Hoehe_Rand) / 2

            F1 = L1 * H1
            F2 = L2 * H2

            FLinks_orig = F1 + F2

            'Rechte Seite Original
            '*********************
            L1 = TRS_Orig(i).C2_Breite_Vorl_R - TRS_Orig(i).B2_Breite_Ufer_R
            L2 = TRS_Orig(i).D2_Breite_Rand_R - TRS_Orig(i).C2_Breite_Vorl_R
            H1 = (TRS_Orig(i).B0_Hoehe_Gerinne + TRS_Orig(i).C0_Hoehe_FPlain) / 2
            H2 = (TRS_Orig(i).C0_Hoehe_FPlain + TRS_Orig(i).D0_Hoehe_Rand) / 2

            F1 = L1 * H1
            F2 = L2 * H2

            FRechts_orig = F1 + F2

            'Linke Seite Aktuell
            '*******************
            L1 = TRS_Act(i).C1_Breite_Vorl_L - TRS_Act(i).B1_Breite_Ufer_L
            L2 = TRS_Act(i).D1_Breite_Rand_L - TRS_Act(i).C1_Breite_Vorl_L
            H1 = (TRS_Act(i).B0_Hoehe_Gerinne + TRS_Act(i).C0_Hoehe_FPlain) / 2
            H2 = (TRS_Act(i).C0_Hoehe_FPlain + TRS_Act(i).D0_Hoehe_Rand) / 2

            F1 = L1 * H1
            F2 = L2 * H2

            FLinks_Act = F1 + F2

            'Rechte Seite Aktuell
            '********************
            L1 = TRS_Act(i).C2_Breite_Vorl_R - TRS_Act(i).B2_Breite_Ufer_R
            L2 = TRS_Act(i).D2_Breite_Rand_R - TRS_Act(i).C2_Breite_Vorl_R
            H1 = (TRS_Act(i).B0_Hoehe_Gerinne + TRS_Act(i).C0_Hoehe_FPlain) / 2
            H2 = (TRS_Act(i).C0_Hoehe_FPlain + TRS_Act(i).D0_Hoehe_Rand) / 2

            F1 = L1 * H1
            F2 = L2 * H2

            FRechts_Act = F1 + F2

            Delta_L = FLinks_orig - FLinks_Act
            Delta_R = FRechts_orig - FRechts_Act

            Delta_Ges = Delta_L + Delta_R

            TRS_Act(i).DeltaVolume = Delta_Ges * TRS_Act(i).Laenge

        Next

    End Sub

    'Volumen der Talsperren einlesen
    '*******************************
    Private Sub Read_TAL(ByRef TAl_Array(,) As Object, ByRef WorkDir As String)

        'Dim TAL_Array(,) As Object = {}
        Dim Datei As String = WorkDir & Me.mProblem.Datensatz & ".TAL"

        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        Dim Zeile As String
        Dim Anz As Integer = 0
        Dim gelesen As Boolean = False

        'Anzahl der Zeilen feststellen
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                Anz += 1
            End If
        Loop Until StrRead.Peek() = -1

        Dim i As Integer = -1
        Dim j As Integer = 0
        ReDim TAl_Array(Anz - 1, 1)

        'Zurück zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        Dim array() As String
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                array = Zeile.Split("|")
                'Werte zuweisen
                TAl_Array(j, 0) = array(1).Trim()
                TAl_Array(j, 1) = array(3).Trim()
                j += 1
                gelesen = True
            End If
            If (Zeile.StartsWith("*") = True And gelesen) Then
                Exit Do
            End If

        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

        'Array bereinigen
        'UPGRADE: Bereinigung könnte stark verkürzt werden, da das Array keine "leeren" Plätze enthält
        Dim x, y As Integer
        Dim TmpArray(TAl_Array.GetUpperBound(0), 1) As String
        System.Array.Copy(TAl_Array, TmpArray, TAl_Array.Length)
        x = 0

        For y = 0 To TmpArray.GetUpperBound(0)
            If Not TmpArray(y, 0) = "" Then
                If String.Compare(TmpArray(y, 0), 1, "S", 1, 1, True) Then
                    TmpArray(x, 0) = TmpArray(y, 0)
                    TmpArray(x, 1) = TmpArray(y, 1)
                    x += 1
                End If
            End If
        Next

        ReDim TAl_Array(x - 1, 1)
        For y = 0 To x - 1
            TAl_Array(y, 0) = TmpArray(y, 0)
            TAl_Array(y, 1) = TmpArray(y, 1)
        Next

    End Sub

    'Weist den Bauwerken die Kosten zu
    '*********************************
    Private Sub Acquire_Costs(ByVal TAL_Array(,) As Object, ByVal TRS_act As TRS(), ByVal Bauwerksliste(,) As Object)

        Dim i, j As Integer
        Dim gefunden As Boolean = False
        Dim Volumen As Double

        For i = 0 To Bauwerksliste.GetUpperBound(0)
            If Bauwerksliste(i, 0).Startswith("T") Then
                For j = 0 To TAL_Array.GetUpperBound(0)
                    If Bauwerksliste(i, 0) = TAL_Array(j, 0) Then
                        Volumen = TAL_Array(j, 1)
                        '=1925*POTENZ(H28;0,564)
                        Bauwerksliste(i, 1) = 1925 * System.Math.Pow(Volumen, 0.564)
                        gefunden = True
                    End If
                Next
            ElseIf Bauwerksliste(i, 0).Startswith("S") Then

                'Kosten für den Aushub bei Vorland Abgrabungen
                For j = 0 To TRS_act.GetUpperBound(0)
                    If Bauwerksliste(i, 0) = TRS_act(j).Name Then
                        'Kalkulation: 20€/m³
                        Bauwerksliste(i, 1) = TRS_act(j).DeltaVolume * 20
                        gefunden = True
                    End If
                Next

                'Länge fürs Meandern wird nicht mehr berechnet
                '*********************************************
                'For j = 0 To TRS_act.GetUpperBound(0)
                '    If Bauwerksliste(i, 0) = TRS_act(j).Name Then
                '        Laenge = TRS_act(j).Laenge
                '        'Kalkulation: 100€/lfm
                '        Bauwerksliste(i, 1) = Laenge * 200
                '        gefunden = True
                '    End If
                'Next

            End If
            If Not gefunden Then
                Throw New Exception("Bauwerk wurde nicht in den Modellparametern gefunden")
            End If
        Next

    End Sub

End Class
