Imports System.IO

Public Class Scan
    Inherits Sim

    Public Overrides ReadOnly Property Datensatzendung() As String
        Get
            Return ".ALL"
        End Get
    End Property

    Private input As Wave.WEL

    Public Overrides Function launchSim() As Boolean

        Dim i, j, k, AnzZeil As Integer
        Dim parameterdatei, Zeile, ZeilenArray(), stoffe(), tmp() As String
        Dim FiStr As FileStream
        Dim StrRead As StreamReader
        Dim StrReadSync As TextReader
        Dim Parameter As Collection


        'Parameter einlesen
        parameterdatei = Me.WorkDir & Me.Datensatz & ".PAR"

        FiStr = New FileStream(parameterdatei, FileMode.Open, IO.FileAccess.Read)
        StrRead = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))
        StrReadSync = TextReader.Synchronized(StrRead)

        'Anzahl der Zeilen feststellen
        AnzZeil = 0
        Do
            Zeile = StrRead.ReadLine.ToString
            AnzZeil += 1
        Loop Until StrRead.Peek() = -1

        ReDim ZeilenArray(AnzZeil - 1)

        'Datei komplett einlesen
        FiStr.Seek(0, SeekOrigin.Begin)
        For j = 0 To AnzZeil - 1
            ZeilenArray(j) = StrRead.ReadLine.ToString
        Next

        StrReadSync.Close()
        StrRead.Close()
        FiStr.Close()

        Parameter = New Collection()
        ReDim stoffe(-1)

        For i = 0 To ZeilenArray.GetUpperBound(0)
            If (i = 1) Then
                stoffe = ZeilenArray(i).Split(";")
                For j = 0 To stoffe.GetUpperBound(0)
                    stoffe(j) = stoffe(j).Trim()
                Next
            ElseIf (i > 1) Then
                tmp = ZeilenArray(i).Split(";")
                Dim werte As New Collection()
                For j = 1 To tmp.GetUpperBound(0)
                    werte.Add(Convert.ToDouble(tmp(j).Trim(), FortranProvider), stoffe(j))
                Next
                Parameter.Add(werte, tmp(0))
            End If
        Next

        'Berechnung
        '----------
        Me.SimErgebnis.Clear()

        'Schleife über Stoffe
        For k = 1 To stoffe.GetUpperBound(0)

            Dim zre As New Wave.Zeitreihe(stoffe(k))
            zre.Length = input.Zeitreihen(0).Length

            Dim tmpWert As Double

            'Schleife über Zeitschritte
            For i = 0 To zre.Length - 1
                tmpWert = 0

                'Schleife über Wellenlängen
                For j = 2 To input.Zeitreihen.GetUpperBound(0)
                    tmpWert += input.Zeitreihen(j).YWerte(i) * Parameter(input.Zeitreihen(j).Title)(stoffe(k))
                Next

                tmpWert += Parameter("Konst")(stoffe(k))

                zre.XWerte(i) = input.Zeitreihen(0).XWerte(i)
                zre.YWerte(i) = tmpWert

            Next

            Me.SimErgebnis.Add(zre, zre.Title)

        Next

        Return True

    End Function

    Public Overrides Function QWert(ByVal OptZiel As Sim.Struct_OptZiel) As Double

        QWert = QWert_Reihe(OptZiel, Me.SimErgebnis(OptZiel.SimGr))

    End Function

    Protected Overrides Sub Read_Kombinatorik()
        'nix
    End Sub

    Protected Overrides Sub Read_SimParameter()

        Dim inputdatei As String

        inputdatei = Me.WorkDir & Me.Datensatz & "_input.WEL"
        Me.input = New Wave.WEL(inputdatei, True)

        Me.SimStart = Me.input.Zeitreihen(0).XWerte(0)
        Me.SimEnde = Me.input.Zeitreihen(0).XWerte(Me.input.Zeitreihen(0).XWerte.GetUpperBound(0))
        'Me.SimDT
    End Sub

    Protected Overrides Sub Read_Verzweigungen()
        'nix
    End Sub

    Protected Overrides Sub Write_Verzweigungen()
        'nix
    End Sub
End Class
