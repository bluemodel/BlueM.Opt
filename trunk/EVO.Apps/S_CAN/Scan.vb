' Copyright (c) BlueM Dev Group
' Website: http://bluemodel.org
' 
' All rights reserved.
' 
' Released under the BSD-2-Clause License:
' 
' Redistribution and use in source and binary forms, with or without modification, 
' are permitted provided that the following conditions are met:
' 
' * Redistributions of source code must retain the above copyright notice, this list 
'   of conditions and the following disclaimer.
' * Redistributions in binary form must reproduce the above copyright notice, this list 
'   of conditions and the following disclaimer in the documentation and/or other materials 
'   provided with the distribution.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
' EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
' OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
' SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT 
' OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
' HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
' TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
' EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'--------------------------------------------------------------------------------------------
'
Imports System.IO
Imports BlueM

Public Class Scan
    Inherits Sim

    Private input As Wave.WEL

    Private Parameter As Collection
    Private stoffe As String()
    Private zeitreihen() As Wave.TimeSeries

    ''' <summary>
    ''' Alle Dateiendungen (ohne Punkt), die in einem Datensatz vorkommen k�nnen
    ''' </summary>
    ''' <remarks>Die erste Dateiendung in dieser Collection repr�sentiert den Datensatz (wird z.B. als Filter f�r OpenFile-Dialoge verwendet)</remarks>
    Public Overrides ReadOnly Property DatensatzDateiendungen() As Collections.Specialized.StringCollection
        Get
            Dim exts As New Collections.Specialized.StringCollection()

            exts.AddRange(New String() {"SCAN", "WEL", "PAR"})

            Return exts

        End Get
    End Property

    ''' <summary>
    ''' Ob die Anwendung Multithreading unterst�tzt
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

        'Schleife �ber Stoffe
        For k = 1 To Me.stoffe.GetUpperBound(0)

            Me.zeitreihen(k - 1) = New Wave.TimeSeries(stoffe(k))

            Dim tmpWert As Double

            'Schleife �ber Zeitschritte
            For i = 0 To input.TimeSeries(0).Length - 1
                tmpWert = 0

                'Schleife �ber Wellenl�ngen
                For j = 2 To input.TimeSeries.GetUpperBound(0)
                    tmpWert += input.TimeSeries(j).Values(i) * Parameter(input.TimeSeries(j).Title)(stoffe(k))
                Next

                tmpWert += Parameter("Konst")(stoffe(k))

                Me.zeitreihen(k - 1).AddNode(input.TimeSeries(0).Dates(i), tmpWert)

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

        Me.SimStart = Me.input.TimeSeries(0).StartDate
        Me.SimEnde = Me.input.TimeSeries(0).Enddate
        'Me.SimDT

    End Sub

    Protected Overrides Sub Read_Verzweigungen()
        'nix
    End Sub

    Protected Overrides Sub Write_Verzweigungen()
        'nix
    End Sub

End Class
