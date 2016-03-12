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

    'Für MPC
    Public Objekt As String                     'Objektbezeichnung (Zur Gruppierung der Optimierungsparameter)
    Public Zeitpunkt As Integer                 'Zeitschritt zu dem der Optparameter gehört

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
    Public Beziehung As Common.Constants.Beziehung

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
        Me.Beziehung = Constants.Beziehung.keine
        Me.Objekt = ""
        Me.Zeitpunkt = 0

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
        Clone.Objekt = Me.Objekt
        Clone.Zeitpunkt = Me.Zeitpunkt

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
    Public Shared Function Get_OptParas_Xn(ByVal OptParameter() As EVO.Common.OptParameter) As Double()

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