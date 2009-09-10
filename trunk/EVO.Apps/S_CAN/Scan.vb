Imports System.IO

Public Class Scan
    Inherits Sim

    Private input As Wave.WEL

    Private Parameter As Collection
    Private stoffe As String()
    Private zeitreihen() As Wave.Zeitreihe

    ''' <summary>
    ''' Alle Dateiendungen (ohne Punkt), die in einem Datensatz vorkommen können
    ''' </summary>
    ''' <remarks>Die erste Dateiendung in dieser Collection repräsentiert den Datensatz (wird z.B. als Filter für OpenFile-Dialoge verwendet)</remarks>
    Public Overrides ReadOnly Property DatensatzDateiendungen() As Collections.Specialized.StringCollection
        Get
            Dim exts As New Collections.Specialized.StringCollection()

            exts.AddRange(New String() {"SCAN", "WEL", "PAR"})

            Return exts

        End Get
    End Property

    ''' <summary>
    ''' Ob die Anwendung Multithreading unterstützt
    ''' </summary>
    ''' <returns>False</returns>
    Public Overrides ReadOnly Property MultithreadingSupported() As Boolean
        Get
            Return False
        End Get
    End Property

    'Konstruktor
    '***********
    Public Sub New()

        Call MyBase.New()

    End Sub

    ''' <summary>
    ''' Simulieren
    ''' </summary>
    Protected Overrides Function launchSim() As Boolean

        Dim i, j, AnzZeil As Integer
        Dim parameterdatei, Zeile, ZeilenArray(), tmp() As String
        Dim FiStr As FileStream
        Dim StrRead As StreamReader
        Dim StrReadSync As TextReader

        'Parameter einlesen
        parameterdatei = Me.WorkDir_Current & Me.Datensatz & ".PAR"

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
                    werte.Add(Convert.ToDouble(tmp(j).Trim(), Common.Provider.FortranProvider), stoffe(j))
                Next
                Parameter.Add(werte, tmp(0))
            End If
        Next

        'Berechnung
        '----------
        ReDim Me.zeitreihen(Me.stoffe.GetUpperBound(0) - 1)

        'Schleife über Stoffe
        For k = 1 To Me.stoffe.GetUpperBound(0)

            Me.zeitreihen(k - 1) = New Wave.Zeitreihe(stoffe(k))

            Dim tmpWert As Double

            'Schleife über Zeitschritte
            For i = 0 To input.Zeitreihen(0).Length - 1
                tmpWert = 0

                'Schleife über Wellenlängen
                For j = 2 To input.Zeitreihen.GetUpperBound(0)
                    tmpWert += input.Zeitreihen(j).YWerte(i) * Parameter(input.Zeitreihen(j).Title)(stoffe(k))
                Next

                tmpWert += Parameter("Konst")(stoffe(k))

                Me.zeitreihen(k - 1).AddNode(input.Zeitreihen(0).XWerte(i), tmpWert)

            Next

        Next

        Return True

    End Function

    Protected Overrides Function launchSim(ByVal Thread_ID As Integer, ByVal Child_ID As Integer) As Boolean
        Return Me.launchSim()
    End Function

    Protected Overrides Function ThreadFree(ByRef Thread_ID As Integer) As Boolean
        Return True
    End Function

    Protected Overrides Function ThreadReady(ByRef Thread_ID As Integer, ByRef SimIsOK As Boolean, ByVal Child_ID As Integer) As Boolean
        Return True
    End Function

    'Simulationsergebnis einlesen
    '----------------------------
    Protected Overrides Sub SIM_Ergebnis_Lesen()

        Me.SimErgebnis.Clear()

        For i = 0 To Me.zeitreihen.Length - 1
            Me.SimErgebnis.Reihen.Add(Me.zeitreihen(i).Title, Me.zeitreihen(i))
        Next

    End Sub

    Protected Overrides Sub Read_SimParameter()

        Dim inputdatei As String

        inputdatei = Me.WorkDir_Current & Me.Datensatz & "_input.WEL"
        Me.input = New Wave.WEL(inputdatei, True)

        Me.SimStart = Me.input.Zeitreihen(0).Anfangsdatum
        Me.SimEnde = Me.input.Zeitreihen(0).Enddatum
        'Me.SimDT

    End Sub

    Protected Overrides Sub Read_Verzweigungen()
        'nix
    End Sub

    Protected Overrides Sub Write_Verzweigungen()
        'nix
    End Sub

End Class
