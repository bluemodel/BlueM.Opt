'BlueM.Opt
'Copyright (C) BlueM Dev Group
'Website: <https://www.bluemodel.org>
'
'This program is free software: you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.
'
'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
'GNU General Public License for more details.
'
'You should have received a copy of the GNU General Public License
'along with this program. If not, see <https://www.gnu.org/licenses/>.
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