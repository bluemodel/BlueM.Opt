Imports System.IO

Public Class SKos


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

    Public Akt_Elemente() As String
    Public Save_TRS(,) as String

    Public Structure TRS

        Dim Name as String
        Dim Laenge as Double

        Dim A_Breite_Sohle as Double

        Dim B1_Höhe_Gerinne as Double
        Dim B2_Breite_Ufer as Double

        Dim C1_Höhe_FPlain as Double
        Dim C2_Breite_Vorl as Double

        Dim D1_Höhe_Rand as Double
        Dim D2_Breite_Rand as Double

    End Structure

    Dim TRS_Orig(-1) as TRS
    Dim TRS_Akt(-1) as TRS

    
    'Funktion für die Kalkulation der Kosten
    '***************************************
    Public Function Calculate_Costs(ByVal BlueM1 As BlueM) As Double
        Dim costs As Double = 0
        Dim Elementliste(0, 1) As Object
        Dim TRS_Array(,) As Object = {}
        Dim TAL_Array(,) As Object = {}

        'Bauwerksliste wird erstellt
        Call create_Elementliste(BlueM1, Elementliste)

        'Ermitteln der massgeblichen Größen
        Call Read_TRS_Laenge(BlueM1, TRS_Array)
        Call Read_TAL(BlueM1, TAL_Array)
        
        'Kalkulieren der Kosten für jedes Bauwerk
        Call Acquire_Costs(TRS_Array, TAL_Array, Elementliste)

        'Kosten aufsummieren
        Dim i, j As Integer
        For i = 0 To Akt_Elemente.GetUpperBound(0)
            For j = 0 To Elementliste.GetUpperBound(0)
                If Elementliste(j, 0) = Akt_Elemente(i) Then
                    costs = costs + Elementliste(j, 1)
                End If
            Next
        Next

        Return costs
    End Function
    'Funktion zum erstellen der Elementliste
    'Alle Elemente aus der CES datei werden hier in die Liste gesetzt
    '****************************************************************
    Private Sub create_Elementliste(ByVal BlueM1 As BlueM, ByRef Bauwerksliste(,) As Object)
        Dim Bauwerks_Array() As String = {}

        'Kopiert die Bauwerke aus dem BlueM
        Dim i, j, k As Integer
        Dim x As Integer = 0
        For i = 0 To BlueM1.List_Locations.GetUpperBound(0)
            For j = 0 To BlueM1.List_Locations(i).List_Massnahmen.GetUpperBound(0)
                For k = 0 To BlueM1.List_Locations(i).List_Massnahmen(j).Bauwerke.GetUpperBound(0)
                    If BlueM1.List_Locations(i).List_Massnahmen(j).KostenTyp = 1 Then
                        System.Array.Resize(Bauwerks_Array, x + 1)
                        Bauwerks_Array(x) = BlueM1.List_Locations(i).List_Massnahmen(j).Bauwerke(k)
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
    Public Sub Remove_X(ByRef Array As String())
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

    'Länge der Transportstrecken einlesen
    '************************************
    Private Sub Read_TRS_Laenge(ByVal BlueM1 As BlueM, ByRef TRS_Array(,) As Object)

        'Dim TRS_Array(,) As Object = {}
        Dim Datei As String = BlueM1.WorkDir & BlueM1.Datensatz & ".TRS"

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
        Dim j As Integer = 0
        ReDim TRS_Array(Anz - 1, 1)

        'Zurück zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        Dim array() As String
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                array = Zeile.Split("|")
                'Werte zuweisen
                TRS_Array(j, 0) = array(1).Trim()
                TRS_Array(j, 1) = array(3).Trim()
                j += 1
            End If

        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

        'Array bereinigen
        Dim x, y As Integer
        Dim TmpArray(TRS_Array.GetUpperBound(0), 1) As String
        System.Array.Copy(TRS_Array, TmpArray, TRS_Array.Length)
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

        ReDim TRS_Array(x - 1, 1)
        For y = 0 To x - 1
            TRS_Array(y, 0) = TmpArray(y, 0)
            TRS_Array(y, 1) = TmpArray(y, 1)
        Next

    End Sub

    'Flächen der Original Transportstrecken einlesen
    '***********************************************
    Public Sub Read_TRS_Orig_Daten(ByVal BlueM1 As BlueM)

        Call Read_TRS_Daten(BlueM1, TRS_Orig)

    End Sub


    'Flächen der Transportstrecken einlesen
    '**************************************
    Public Shared Sub Read_TRS_Daten(ByVal BlueM1 As BlueM, ByRef TRS_Array() As TRS)

        'Dim TRS_Array(,) As Object = {}
        Dim Datei As String = BlueM1.WorkDir & BlueM1.Datensatz & ".TRS"

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
        Dim count as Integer = 0

        'Zurück zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        Dim Array1() As String
        Dim Array2() As String

        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                Array1 = Zeile.Split("|")
                Array2 = Split(Array1(6))

                'Filtert das Array auf leere Einträge
                Dim LastNonEmpty As Integer = -1
                For t As Integer = 0 To Array2.Length - 1
                    If Array2(t) <> "" Then
                        LastNonEmpty += 1
                        Array2(LastNonEmpty) = Array2(t)
                    End If
                Next
                ReDim Preserve Array2(LastNonEmpty)

                'Werte zuweisen
                'Prüfen ob neues Element
                If Array1(1).Trim() <> "" And Array1(2).trim = "3" then
                    j += 1
                    count = 0
                    ReDim Preserve TRS_Array(TRS_Array.GetLength(0))
                    TRS_Array(j).Name = Array1(1).Trim()
                    TRS_Array(j).Laenge = Array1(3).Trim()
                    TRS_Array(j).A_Breite_Sohle = array2(1).Trim()
                ElseIf Array1(1).Trim() = "" Then
                    count += 1
                End If

                If count = 1 then
                    TRS_Array(j).B1_Höhe_Gerinne = Array2(0).Trim()
                    TRS_Array(j).B2_Breite_Ufer = Array2(1).Trim()
                else if count = 2
                    TRS_Array(j).C1_Höhe_FPlain = Array2(1).Trim()
                    TRS_Array(j).C2_Breite_Vorl = Array2(2).Trim()
                else if count = 3
                    TRS_Array(j).D1_Höhe_Rand = Array2(1).Trim()
                    TRS_Array(j).D2_Breite_Rand = Array2(2).Trim()
                    count = 0
                End If
            End If
        Loop Until StrRead.Peek() = -1

    End Sub

    'Volumen der Talsperren einlesen
    '*******************************
    Public Sub Read_TAL(ByVal BlueM1 As BlueM, ByRef TAl_Array(,) As Object)

        'Dim TAL_Array(,) As Object = {}
        Dim Datei As String = BlueM1.WorkDir & BlueM1.Datensatz & ".TAL"

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
    Private Sub Acquire_Costs(ByVal TRS_Array(,) As Object, ByVal TAL_Array(,) As Object, ByVal Bauwerksliste(,) As Object)

        Dim i, j As Integer
        Dim gefunden As Boolean = False
        Dim Volumen As Double
        Dim Laenge As Double

        For i = 0 To Bauwerksliste.GetUpperBound(0)
            If Bauwerksliste(i, 0).Startswith("T") Then
                For j = 0 To TAL_Array.GetUpperBound(0)
                    If Bauwerksliste(i, 0) = TAL_Array(j, 0) Then
                        Volumen = TAL_Array(j, 1)
                        'Kalkulation: (Kosten €) = 9.0882 * (Volumen m³) + 396586
                        Bauwerksliste(i, 1) = 9.1 * Volumen + 400000
                        gefunden = True
                    End If
                Next
            ElseIf Bauwerksliste(i, 0).Startswith("S") Then
                For j = 0 To TRS_Array.GetUpperBound(0)
                    If Bauwerksliste(i, 0) = TRS_Array(j, 0) Then
                        Laenge = TRS_Array(j, 1)
                        'Kalkulation: 100€/lfm
                        Bauwerksliste(i, 1) = Laenge * 200
                        gefunden = True
                    End If
                Next
            End If
            If Not gefunden Then
                Throw New Exception("Bauwerk wurde nicht in den Modellparametern gefunden")
            End If
        Next

    End Sub

End Class
