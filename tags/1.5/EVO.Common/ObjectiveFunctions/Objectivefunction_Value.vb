﻿' Copyright (c) 2011, ihwb, TU Darmstadt
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
Public Class Objectivefunction_Value
    Inherits ObjectiveFunction

    ''' <summary>
    ''' Returns the type of the ObjectiveFunction
    ''' </summary>
    Public Overrides ReadOnly Property GetObjType() As ObjectiveType
        Get
            Return ObjectiveType.Value
        End Get
    End Property

    ''' <summary>
    ''' Der zu vergleichende Referenzwert
    ''' </summary>
    ''' <remarks>siehe Wiki</remarks>
    Public RefWert As Double

    ''' <summary>
    ''' Gibt den Block an, in der die Zielfunktionswerte stehen
    ''' </summary>
    ''' <remarks>Erlaubte Werte abhängig von Application: siehe Wiki</remarks>
    Public Block As String

    ''' <summary>
    ''' Gibt die Spalte im Block an, in dem díe Zielfunktionswerte stehen
    ''' </summary>
    ''' <remarks>Erlaubte Werte abhängig von Application: siehe Wiki</remarks>
    Public Spalte As String

    ''' <summary>
    ''' Calculate ObjectiveFunction value
    ''' </summary>
    ''' <param name="SimErgebnis">collection of simulation results</param>
    ''' <returns>objective function value</returns>
    Public Overrides Function calculateObjective(ByVal SimErgebnis As SimErgebnis_Structure) As Double

        Dim SimWert As Double
        Dim QWert As Double

        'SimWert aus SimErgebnis rausholen
        SimWert = SimErgebnis.Werte(Me.Bezeichnung)

        'Wertevergleich ausführen
        QWert = ObjectiveFunction.compareValues(SimWert, Me.RefWert, Me.Funktion)

        'Zielrichtung berücksichtigen
        QWert *= Me.Richtung

        Return QWert

    End Function

End Class