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


    Public Function calculate_costs(ByVal BlueM1 As BlueM)
        Dim costs As Double
        Dim Bauwerksliste(0, 1) As Object
        Dim TRS_Array(,) As Object = {}
        Dim TAL_Array(,) As Object = {}


        'Bauwerksliste wird erstellt
        Call create_Bauwerksliste(BlueM1, Bauwerksliste)

        'Ermitteln der massgeblichen Größen
        Call Read_TRS(BlueM1, TRS_Array)
        Call Read_TAL(BlueM1, TAL_Array)


        Return costs
    End Function
    'Funktion zum erstellen der Bauwerksliste
    Private Sub create_Bauwerksliste(ByVal BlueM1 As BlueM, ByRef Bauwerksliste As Object)
        Dim Bauwerks_Array() As Object = {}

        'Kopiert die Bauwerke aus dem BlueM
        Dim i, j, k As Integer
        Dim x As Integer = 0
        For i = 0 To BlueM1.LocationList.GetUpperBound(0)
            For j = 0 To BlueM1.LocationList(i).MassnahmeListe.GetUpperBound(0)
                For k = 0 To BlueM1.LocationList(i).MassnahmeListe(j).Bauwerke.GetUpperBound(0)
                    System.Array.Resize(Bauwerks_Array, x + 1)
                    Bauwerks_Array(x) = BlueM1.LocationList(i).MassnahmeListe(j).Bauwerke(k)
                    x += 1
                Next
            Next
        Next

        'Die "X" Einträge werden entfernt
        Dim TmpArray(Bauwerks_Array.GetUpperBound(0)) As String
        Array.Copy(Bauwerks_Array, TmpArray, Bauwerks_Array.GetLength(0))
        x = 0
        For i = 0 To TmpArray.GetUpperBound(0)
            If Not TmpArray(i) = "X" Then
                Bauwerks_Array(x) = TmpArray(i)
                x += 1
            End If
        Next

        'Die Werte des Arrays werden an die Liste übertragen
        System.Array.Resize(Bauwerks_Array, x)
        ReDim Bauwerksliste(x - 1, 1)
        For i = 0 To Bauwerks_Array.GetUpperBound(0)
            Bauwerksliste(i, 1) = Bauwerks_Array(i)
        Next
    End Sub

    'Länge der Transportstrecken einlesen
    Private Sub Read_TRS(ByVal BlueM1 As BlueM, ByRef TRS_Array As Object)
        'Dim TRS_Array(,) As Object = {}
        Try
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

        Catch except As Exception
            MsgBox(except.Message & Chr(13) & Chr(10) & "Ein Fehler könnten Leerzeichen in der letzten Zeile der Datei sein.", MsgBoxStyle.Exclamation, "Fehler beim Lesen der Kombinatorik")
        End Try

        'Array bereinigen
        Dim x, y As Integer
        Dim TmpArray(TRS_Array.GetUpperBound(0), 1) As String
        Array.Copy(TRS_Array, TmpArray, TRS_Array.Length)
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
    Private Sub Read_TAL(ByVal BlueM1 As BlueM, ByRef TAl_Array As Object)
        'Dim TAL_Array(,) As Object = {}
        Try
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
            ReDim TAL_Array(Anz - 1, 1)

            'Zurück zum Dateianfang und lesen
            FiStr.Seek(0, SeekOrigin.Begin)

            Dim array() As String
            Do
                Zeile = StrRead.ReadLine.ToString()
                If (Zeile.StartsWith("*") = False) Then
                    array = Zeile.Split("|")
                    'Werte zuweisen
                    TAL_Array(j, 0) = array(1).Trim()
                    TAL_Array(j, 1) = array(3).Trim()
                    j += 1
                    gelesen = True
                End If
                If (Zeile.StartsWith("*") = True And gelesen) Then
                    Exit Do
                End If

            Loop Until StrRead.Peek() = -1

            StrRead.Close()
            FiStr.Close()

        Catch except As Exception
            MsgBox(except.Message & Chr(13) & Chr(10) & "Ein Fehler könnten Leerzeichen in der letzten Zeile der Datei sein.", MsgBoxStyle.Exclamation, "Fehler beim Lesen der Kombinatorik")
        End Try

        'Array bereinigen
        'ToDo: Bereinigung könnte stark verkürzt werden, da das Array keine "leeren" Plätze enthält
        Dim x, y As Integer
        Dim TmpArray(TAL_Array.GetUpperBound(0), 1) As String
        Array.Copy(TAL_Array, TmpArray, TAL_Array.Length)
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

        ReDim TAL_Array(x - 1, 1)
        For y = 0 To x - 1
            TAL_Array(y, 0) = TmpArray(y, 0)
            TAL_Array(y, 1) = TmpArray(y, 1)
        Next

    End Sub

End Class
