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

    Public Aktuell_Elemente() As String

    'Funktion für die Kalkulation der Kosten
    '***************************************
    Public Function calculate_costs(ByVal BlueM1 As BlueM) As Double
        Dim costs As Double = 0
        Dim Bauwerksliste(0, 1) As Object
        Dim TRS_Array(,) As Object = {}
        Dim TAL_Array(,) As Object = {}

        'Bauwerksliste wird erstellt
        Call create_Bauwerksliste(BlueM1, Bauwerksliste)

        'Ermitteln der massgeblichen Größen
        Call Read_TRS(BlueM1, TRS_Array)
        Call Read_TAL(BlueM1, TAL_Array)

        'Kalkulieren der Kosten für jedes Bauwerk
        Call Acquire_Costs(TRS_Array, TAL_Array, Bauwerksliste)

        'Kosten aufsummieren
        Dim i, j As Integer
        For i = 0 To Aktuell_Elemente.GetUpperBound(0)
            For j = 0 To Bauwerksliste.GetUpperBound(0)
                If Bauwerksliste(j, 0) = Aktuell_Elemente(i) Then
                    costs = costs + Bauwerksliste(j, 1)
                End If
            Next
        Next

        Return costs
    End Function
    'Funktion zum erstellen der Bauwerksliste
    '****************************************
    Private Sub create_Bauwerksliste(ByVal BlueM1 As BlueM, ByRef Bauwerksliste(,) As Object)
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
    Private Sub Read_TRS(ByVal BlueM1 As BlueM, ByRef TRS_Array(,) As Object)

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

    'Volumen der Talsperren einlesen
    Private Sub Read_TAL(ByVal BlueM1 As BlueM, ByRef TAl_Array(,) As Object)

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
