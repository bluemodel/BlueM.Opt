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
''' Structure für Modellparameter
''' </summary>
Public Structure Struct_ModellParameter
    ''' <summary>
    ''' Name des OptParameters, aus dem dieser Modellparameter errechnet wird
    ''' </summary>
    Public OptParameter As String
    ''' <summary>
    ''' Bezeichnung des Modellparameters
    ''' </summary>
    Public Bezeichnung As String
    ''' <summary>
    ''' Einheit des Parameterwertes 
    ''' </summary>
    ''' <remarks>Optional</remarks>
    Public Einheit As String
    ''' <summary>
    ''' Dateiendung (ohne Punkt) der Eingabedatei, in der sich der Parameter befindet
    ''' </summary>
    ''' <remarks>z.B. bei BlueM: EZG, TRS, TAL, BOF, BOA, BOD, etc.</remarks>
    Public Datei As String
    ''' <summary>
    ''' Das Element auf das sich der Modellparameter bezieht
    ''' </summary>
    ''' <remarks>Optional</remarks>
    Public Element As String
    ''' <summary>
    ''' Zeile, in der sich der Modellparameter befindet (gezählt vom Dateianfang der Eingabedatei) 
    ''' </summary>
    Public ZeileNr As Integer
    ''' <summary>
    ''' Spalte, ab der der Modellparameter in der Eingabedatei definiert ist
    ''' </summary>
    Public SpVon As Integer
    ''' <summary>
    ''' Spalte, bis zu welcher der Modellparameter in der Eingabedatei definiert ist
    ''' </summary>
    Public SpBis As Integer
    ''' <summary>
    ''' Faktor fuer das Umrechnen zwischen OptParameter und ModellParameter
    ''' </summary>
    ''' <remarks>ModellParameter = OptimierungsParameter * Faktor</remarks>
    Public Faktor As Double
    ''' <summary>
    ''' Klont einen Modellparameter
    ''' </summary>
    Public Function Clone() As Struct_ModellParameter
        Clone.OptParameter = Me.OptParameter
        Clone.Bezeichnung = Me.Bezeichnung
        Clone.Einheit = Me.Einheit
        Clone.Datei = Me.Datei
        Clone.Element = Me.Element
        Clone.ZeileNr = Me.ZeileNr
        Clone.SpVon = Me.SpVon
        Clone.SpBis = Me.SpBis
        Clone.Faktor = Me.Faktor
    End Function
End Structure