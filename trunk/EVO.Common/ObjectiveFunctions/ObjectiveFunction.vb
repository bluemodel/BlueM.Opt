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
Imports BlueM

''' <summary>
''' Klasse für die Definition von Objective Funktionen
''' </summary>
Public MustInherit Class ObjectiveFunction

    ''' <summary>
    ''' Struktur für Simulationsergebnisse
    ''' </summary>
    Public Structure SimErgebnis_Structure
        ''' <summary>
        ''' Ergebniswerte (Key ist ObjectiveFunction Name)
        ''' </summary>
        Public Werte As Dictionary(Of String, Double)
        ''' <summary>
        ''' Ergebnisreihen (Key ist ObjectiveFunction Name)
        ''' </summary>
        Public Reihen As Dictionary(Of String, Wave.TimeSeries)
        ''' <summary>
        ''' Löscht alle vorhandenen Ergebnisse
        ''' </summary>
        Public Sub Clear()
            Me.Werte = New Dictionary(Of String, Double)
            Me.Reihen = New Dictionary(Of String, Wave.TimeSeries)
        End Sub
    End Structure

    Public Enum ObjectiveType As Integer
        Series = 1
        Value = 2
        ValueFromSeries = 3
        Aggregate = 5
        SKos = 6
        Ecology = 7
    End Enum

    ''' <summary>
    ''' Gibt an ob es sich um eine PrimaryObjective Function handelt
    ''' </summary>
    Public isPrimObjective As Boolean

    ''' <summary>
    ''' Bezeichnung
    ''' </summary>
    Public Bezeichnung As String

    ''' <summary>
    ''' Gruppe
    ''' </summary>
    Public Gruppe As String

    ''' <summary>
    ''' Richtung der ObjectiveFunction (d.h. zu maximieren oder zu minimieren)
    ''' </summary>
    Public Richtung As Constants.EVO_RICHTUNG

    ''' <summary>
    ''' Operator bzw Faktor
    ''' </summary>
    Public OpFact As Double

    ''' <summary>
    ''' Die Dateiendung der Ergebnisdatei, aus der das Simulationsergebnis ausgelesen werden soll
    ''' </summary>
    ''' <remarks>Erlaubte Werte: z.B. "WEL" oder "ASC"</remarks>
    Public Datei As String

    ''' <summary>
    ''' Die Simulationsgröße, auf dessen Basis der Objectivewert berechnet werden soll
    ''' </summary>
    Public SimGr As String

    ''' <summary>
    ''' Name der Funktion, mit der der Objectivewert berechnet werden soll
    ''' </summary>
    ''' <remarks>Erlaubte Werte: "AbQuad", "Diff", "nÜber", "sÜber", "nUnter", "sUnter", "Volf". Siehe auch Wiki</remarks>
    Public Funktion As String

    ''' <summary>
    ''' Gibt an, ob die Objective Function einen IstWert besitzt
    ''' </summary>
    Public hasIstWert As Boolean

    ''' <summary>
    ''' Objective Wert im Istzustand
    ''' </summary>
    Public IstWert As Double

    ''' <summary>
    ''' Gibt die Bezeichnung zurück
    ''' </summary>
    ''' <returns>Bezeichnung</returns>
    Public Overrides Function ToString() As String
        Return Me.Bezeichnung
    End Function

    ''' <summary>
    ''' Gibt an ob es ein GruppenLeader ist
    ''' </summary>
    Public ReadOnly Property isGroupLeader() As Boolean
        Get
            If (Me.GetObjType = ObjectiveType.Aggregate) Then
                Return True
            End If
            Return False
        End Get
    End Property

    ''' <summary>
    ''' Returns the type of the ObjectiveFunction
    ''' </summary>
    Public MustOverride ReadOnly Property GetObjType() As ObjectiveType

    ''' <summary>
    ''' Calculate the objective function value
    ''' </summary>
    ''' <param name="SimErgebnis">collection of simulation results</param>
    ''' <returns>objective function value</returns>
    Public MustOverride Function calculateObjective(ByVal SimErgebnis As SimErgebnis_Structure) As Double

    ''' <summary>
    ''' compare two values using a function
    ''' </summary>
    ''' <param name="SimWert">simulation value</param>
    ''' <param name="RefWert">reference value</param>
    ''' <param name="Funktion">comparison function</param>
    ''' <returns>function value</returns>
    ''' <remarks>BUG 218: Konstante und gleiche Zeitschrittweiten vorausgesetzt!</remarks>
    Protected Shared Function compareValues(ByVal SimWert As Double, ByVal RefWert As Double, ByVal Funktion As String)

        Dim QWert As Double

        'Fallunterscheidung Zielfunktion
        Select Case Funktion

            Case "AbQuad"
                'quadratische Abweichung
                '-----------------------
                QWert = (RefWert - SimWert) ^ 2

            Case "Diff"
                'absolute Abweichung
                '-------------------
                QWert = Math.Abs(RefWert - SimWert)

            Case Else
                Throw New Exception("Die Zielfunktion '" & Funktion & "' wird für Wertevergleiche nicht unterstützt!")

        End Select

        Return QWert

    End Function

    ''' <summary>
    ''' compare two series using a function
    ''' </summary>
    ''' <param name="SimReihe">simulation series</param>
    ''' <param name="RefReihe">reference series</param>
    ''' <param name="Funktion">comparison function</param>
    ''' <returns>function value</returns>
    ''' <remarks>BUG 218: Konstante und gleiche Zeitschrittweiten vorausgesetzt!</remarks>
    Protected Shared Function compareSeries(ByVal SimReihe As Wave.TimeSeries, ByVal RefReihe As Wave.TimeSeries, ByVal Funktion As String) As Double

        Dim QWert As Double
        Dim i As Integer

        'BUG 218: Kontrolle
        If (RefReihe.Length <> SimReihe.Length) Then
            Throw New Exception("Die Reihen '" & SimReihe.Title & "' und '" & RefReihe.Title & "' sind nicht kompatibel! (Länge/Zeitschritt?) Siehe Bug 218")
        End If

        'Fallunterscheidung Zielfunktion
        Select Case Funktion

            Case "AbQuad"
                'Summe der Fehlerquadrate
                '------------------------
                QWert = 0
                For i = 0 To SimReihe.Length - 1
                    QWert += (RefReihe.Values(i) - SimReihe.Values(i)) ^ 2
                Next

            Case "Diff"
                'Summe der Fehler
                '----------------
                QWert = 0
                For i = 0 To SimReihe.Length - 1
                    QWert += Math.Abs(RefReihe.Values(i) - SimReihe.Values(i))
                Next

            Case "Volf"
                'Volumenfehler
                '-------------
                Dim VolSim As Double = 0
                Dim VolZiel As Double = 0
                For i = 0 To SimReihe.Length - 1
                    VolSim += SimReihe.Values(i)
                    VolZiel += RefReihe.Values(i)
                Next
                'Differenz bilden und auf ZielVolumen beziehen
                QWert = Math.Abs(VolZiel - VolSim) / VolZiel * 100

            Case "nUnter"
                'Relative Anzahl der Zeitschritte mit Unterschreitungen (in Prozent)
                '-------------------------------------------------------------------
                Dim nUnter As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.Values(i) < RefReihe.Values(i)) Then
                        nUnter += 1
                    End If
                Next
                QWert = nUnter / SimReihe.Length * 100

            Case "sUnter"
                'Summe der Unterschreitungen
                '---------------------------
                Dim sUnter As Double = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.Values(i) < RefReihe.Values(i)) Then
                        sUnter += RefReihe.Values(i) - SimReihe.Values(i)
                    End If
                Next
                QWert = sUnter

            Case "nÜber"
                'Relative Anzahl der Zeitschritte mit Überschreitungen (in Prozent)
                '------------------------------------------------------------------
                Dim nUeber As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.Values(i) > RefReihe.Values(i)) Then
                        nUeber += 1
                    End If
                Next
                QWert = nUeber / SimReihe.Length * 100

            Case "sÜber"
                'Summe der Überschreitungen
                '--------------------------
                Dim sUeber As Double = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.Values(i) > RefReihe.Values(i)) Then
                        sUeber += SimReihe.Values(i) - RefReihe.Values(i)
                    End If
                Next
                QWert = sUeber

            Case "NashSutt"
                'Nash Sutcliffe
                '--------------
                'Mittelwert bilden
                Dim Qobs_quer, zaehler, nenner As Double
                For i = 0 To SimReihe.Length - 1
                    Qobs_quer += RefReihe.Values(i)
                Next
                Qobs_quer = Qobs_quer / (SimReihe.Length)
                For i = 0 To SimReihe.Length - 1
                    zaehler += (RefReihe.Values(i) - SimReihe.Values(i)) * (RefReihe.Values(i) - SimReihe.Values(i))
                    nenner += (RefReihe.Values(i) - Qobs_quer) * (RefReihe.Values(i) - Qobs_quer)
                Next
                'abgeänderte Nash-Sutcliffe Formel: 0 als Zielwert (1- weggelassen)
                QWert = zaehler / nenner

            Case "Korr"
                'Korrelationskoeffizient (lineare Regression)
                'Es wird das Bestimmtheitsmaß r^2 zurückgegeben [0-1]
                '----------------------------------------------------
                Dim kovar, var_x, var_y, avg_x, avg_y As Double
                'Mittelwerte
                avg_x = SimReihe.getWert("Average")
                avg_y = RefReihe.getWert("Average")
                'r^2 = sxy^2 / (sx^2 * sy^2)
                'Standardabweichung: var_x = sx^2 = 1 / (n-1) * SUMME[(x_i - x_avg)^2]
                'Kovarianz: kovar= sxy = 1 / (n-1) * SUMME[(x_i - x_avg) * (y_i - y_avg)]
                kovar = 0
                var_x = 0
                var_y = 0
                For i = 0 To SimReihe.Length - 1
                    kovar += (SimReihe.Values(i) - avg_x) * (RefReihe.Values(i) - avg_y)
                    var_x += (SimReihe.Values(i) - avg_x) ^ 2
                    var_y += (RefReihe.Values(i) - avg_y) ^ 2
                Next
                var_x = 1 / (SimReihe.Length - 1) * var_x
                var_y = 1 / (SimReihe.Length - 1) * var_y
                kovar = 1 / (SimReihe.Length - 1) * kovar
                'Bestimmtheitsmaß = Korrelationskoeffizient^2
                QWert = kovar ^ 2 / (var_x * var_y)

            Case Else
                Throw New Exception("Die Zielfunktion '" & Funktion & "' wird für Reihenvergleiche nicht unterstützt!")

        End Select

        Return QWert

    End Function

End Class
