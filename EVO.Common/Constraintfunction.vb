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
''' <summary>
''' Klasse für die Definition von Constraint Funktionen
''' </summary>
Public Class Constraintfunction

    ''' <summary>
    ''' Bezeichnung
    ''' </summary>
    Public Bezeichnung As String

    ''' <summary>
    ''' Gibt an ob es sich um einen Wert oder um eine Reihe handelt
    ''' </summary>
    ''' <remarks>Erlaubte Werte: "Wert" und "Reihe"</remarks>
    Public Typ As String                   '

    ''' <summary>
    ''' Die Dateiendung der Ergebnisdatei, aus der das Simulationsergebnis ausgelesen werden soll
    ''' </summary>
    ''' <remarks>z.B. "WEL"</remarks>
    Public Datei As String

    ''' <summary>
    ''' Die Simulationsgröße, die auf Verletzung der Grenze überprüft werden soll
    ''' </summary>
    Public SimGr As String                      '

    ''' <summary>
    ''' Grenzposition (Ober-/Untergrenze)
    ''' </summary>
    ''' <remarks>Erlaubte Werte: "Obergrenze" und "Untergrenze"</remarks>
    Public GrenzPos As String

    ''' <summary>
    ''' Gibt an wie der Wert, der mit dem Grenzwert verglichen werden soll, aus dem Simulationsergebnis berechnet werden soll
    ''' </summary>
    ''' <remarks>Nur bei Typ = "Wert". Erlaubte Werte: "MaxWert", "MinWert", "Average", "AnfWert", "EndWert". Siehe auch Wiki</remarks>
    Public WertFunktion As String

    ''' <summary>
    ''' Der vorgegebene Grenzwert
    ''' </summary>
    ''' <remarks>Nur bei Typ = "Wert"</remarks>
    Public GrenzWert As Double

    ''' <summary>
    ''' Der Dateiname der Grenzwertreihe
    ''' </summary>
    ''' <remarks>Nur bei Typ = "Reihe". Pfadangabe relativ zum Datensatz</remarks>
    Public GrenzReiheDatei As String

    ''' <summary>
    ''' Zu verwendender Spaltenname falls Grenzwertreihe eine .WEL Datei ist
    ''' </summary>
    ''' <remarks>Nur bei Typ = "Reihe"</remarks>
    Public GrenzGr As String

    ''' <summary>
    ''' Die Grenzwertreihe
    ''' </summary>
    ''' <remarks>Nur bei Typ = "Reihe"</remarks>
    Public GrenzReihe As Wave.Zeitreihe

    ''' <summary>
    ''' Gibt die Bezeichnung zurück
    ''' </summary>
    ''' <returns>Bezeichnung</returns>
    Public Overrides Function ToString() As String
        Return Bezeichnung
    End Function

End Class
