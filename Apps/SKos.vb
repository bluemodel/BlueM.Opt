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
        Dim Bauwerksliste() As String = {}

        'Bauwerksliste wird erstellt
        Call create_Bauwerksliste(BlueM1, Bauwerksliste)

        'Ermitteln der massgeblichen Größen
        Call Read_TRS(BlueM1)


        Return costs
    End Function
    'Funktion zum erstellen der Bauwerksliste
    Private Sub create_Bauwerksliste(ByVal BlueM1 As BlueM, ByRef Bauwerksliste() As String)

        'Kopiert die Bauwerke aus dem BlueM
        Dim i, j, k As Integer
        Dim x As Integer = 0
        For i = 0 To BlueM1.LocationList.GetUpperBound(0)
            For j = 0 To BlueM1.LocationList(i).MassnahmeListe.GetUpperBound(0)
                For k = 0 To BlueM1.LocationList(i).MassnahmeListe(j).Bauwerke.GetUpperBound(0)
                    System.Array.Resize(Bauwerksliste, x + 1)
                    Bauwerksliste(x) = BlueM1.LocationList(i).MassnahmeListe(j).Bauwerke(k)
                    x += 1
                Next
            Next
        Next

        'Die "X" Einträge werden entfernt
        Dim TmpArray(Bauwerksliste.GetUpperBound(0)) As String
        Array.Copy(Bauwerksliste, TmpArray, Bauwerksliste.GetLength(0))
        x = 0
        For i = 0 To TmpArray.GetUpperBound(0)
            If Not TmpArray(i) = "X" Then
                Bauwerksliste(x) = TmpArray(i)
                x += 1
            End If
        Next
        System.Array.Resize(Bauwerksliste, x)

    End Sub

    'Transportstrecken einlesen
    Private Sub Read_TRS(ByVal BlueM1 As BlueM)
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
            ReDim BlueM1.LocationList(0)
            ReDim BlueM1.LocationList(0).MassnahmeListe(0)

            'Zurück zum Dateianfang und lesen
            FiStr.Seek(0, SeekOrigin.Begin)

            Dim array() As String
            Do
                Zeile = StrRead.ReadLine.ToString()
                If (Zeile.StartsWith("*") = False) Then
                    array = Zeile.Split("|")
                    'Werte zuweisen

                    If Not BlueM.Is_Name_IN(array(1).Trim(), BlueM1.LocationList) Then
                        i += 1
                        j = 0
                        System.Array.Resize(BlueM1.LocationList, i + 1)
                        BlueM1.LocationList(i).Name = array(1).Trim()
                    End If
                    System.Array.Resize(BlueM1.LocationList(i).MassnahmeListe, j + 1)
                    ReDim BlueM1.LocationList(i).MassnahmeListe(j).Schaltung(2, 1)
                    ReDim BlueM1.LocationList(i).MassnahmeListe(j).Bauwerke(3)
                    BlueM1.LocationList(i).MassnahmeListe(j).Name = array(2).Trim()
                    BlueM1.LocationList(i).MassnahmeListe(j).Schaltung(0, 0) = array(3).Trim()
                    BlueM1.LocationList(i).MassnahmeListe(j).Schaltung(0, 1) = array(4).Trim()
                    BlueM1.LocationList(i).MassnahmeListe(j).Schaltung(1, 0) = array(5).Trim()
                    BlueM1.LocationList(i).MassnahmeListe(j).Schaltung(1, 1) = array(6).Trim()
                    BlueM1.LocationList(i).MassnahmeListe(j).Schaltung(2, 0) = array(7).Trim()
                    BlueM1.LocationList(i).MassnahmeListe(j).Schaltung(2, 1) = array(8).Trim()
                    BlueM1.LocationList(i).MassnahmeListe(j).KostenTyp = array(9).Trim()
                    BlueM1.LocationList(i).MassnahmeListe(j).Bauwerke(0) = array(10).Trim()
                    BlueM1.LocationList(i).MassnahmeListe(j).Bauwerke(1) = array(11).Trim()
                    BlueM1.LocationList(i).MassnahmeListe(j).Bauwerke(2) = array(12).Trim()
                    BlueM1.LocationList(i).MassnahmeListe(j).Bauwerke(3) = array(13).Trim()
                    j += 1
                End If

            Loop Until StrRead.Peek() = -1

            StrRead.Close()
            FiStr.Close()

        Catch except As Exception
            MsgBox(except.Message & Chr(13) & Chr(10) & "Ein Fehler könnten Leerzeichen in der letzten Zeile der Datei sein.", MsgBoxStyle.Exclamation, "Fehler beim Lesen der Kombinatorik")
        End Try



    End Sub

    'Talsperren einlesen
    Private Sub Read_TAL()


    End Sub

End Class
