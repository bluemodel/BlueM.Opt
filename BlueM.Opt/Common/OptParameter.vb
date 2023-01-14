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
Public Class OptParameter

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse OptParameter                                                   ****
    '**** für das Speichern eines Optimierungsparameters                        ****
    '**** und zugehöriger Informationen                                         ****
    '****                                                                       ****
    '**** Autoren:                                                              ****
    '**** Felix Froehlich                                                       ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

#Region "Eigenschaften"

    'Metadaten
    Public Bezeichnung As String
    Public Einheit As String

    'Parameterwerte
    Public Xn As Double                         'Skalierter Parameterwert
    Public Min As Double                        'Minwert für die Umrechnung in reellen Parameterwert
    Public Max As Double                        'Maxwert für die Umrechnung in reellen Parameterwert

    Public Property RWert() As Double           'Reeller Parameterwert
        Get
            Return Me.Min + (Me.Max - Me.Min) * Me.Xn
        End Get
        Set(ByVal value As Double)
            Me.Xn = (value - Me.Min) / (Me.Max - Me.Min)
        End Set
    End Property

    Public StartWert As Double                  'Reeller Startwert

    'Schrittweite
    Public Dn As Double
    'Schiefemaß
    Public C As Double

    'Beziehung
    Public Beziehung As Common.Constants.Relationship

#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

    'Konstruktor
    '***********
    Public Sub New()

        'Default Werte setzen
        Me.Bezeichnung = "[nicht gesetzt]"
        Me.Einheit = "[-]"
        Me.Min = 0
        Me.Max = 1
        Me.Xn = 0.5
        Me.StartWert = 0.5
        Me.Dn = 0.1
        Me.Beziehung = Constants.Relationship.none

    End Sub

    'ToString
    '********
    Public Overrides Function ToString() As String
        Return Me.Bezeichnung
    End Function

    'Duplizieren eines OptParameters
    '*******************************
    Public Function Clone() As OptParameter

        Clone = New OptParameter()

        Clone.Bezeichnung = Me.Bezeichnung
        Clone.Einheit = Me.Einheit
        Clone.Xn = Me.Xn
        Clone.Dn = Me.Dn
        Clone.Min = Me.Min
        Clone.Max = Me.Max
        Clone.StartWert = Me.StartWert
        Clone.Beziehung = Me.Beziehung

        Return Clone

    End Function

    'Kopiert ein Array von OptParametern
    '*********************************
    Public Shared Function Clone_Array(ByVal sourcearray() As OptParameter) As OptParameter()

        Dim i As Integer
        Dim clonearray() As OptParameter

        ReDim clonearray(sourcearray.GetUpperBound(0))

        For i = 0 To sourcearray.GetUpperBound(0)
            clonearray(i) = sourcearray(i).Clone()
        Next

        Return clonearray

    End Function

    'Konvertiert eine Liste von OptParametern in ein Array von Doubles (Xn)
    '**********************************************************************
    Public Shared Function Get_OptParas_Xn(ByVal OptParameter() As BlueM.Opt.Common.OptParameter) As Double()

        Dim i As Integer
        Dim Xn() As Double

        ReDim Xn(OptParameter.GetUpperBound(0))

        For i = 0 To OptParameter.GetUpperBound(0)
            Xn(i) = OptParameter(i).Xn
        Next

        Return Xn

    End Function

#End Region 'Methoden

End Class