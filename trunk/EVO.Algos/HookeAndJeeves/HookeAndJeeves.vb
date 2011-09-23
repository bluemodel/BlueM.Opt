' Copyright (c) 2011, ihwb, TU Darmstadt
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
Public Class HookeAndJeeves
    'Settings
    Private intAnzahlParameter As Integer
    Private dblStartSchrittweite As Double
    Private dblMinimaleSchrittweite As Double
    'Startwerte der zu optimierende Parameter
    Private dblStartparameter() As Double
    'Aktuelle Schrittweite
    Private dblAktuelleSchrittweite As Double
    Private dblLetzteParameter() As Double
    Private dblLetzteParameterBackup() As Double
    Private dblAktuelleParameter() As Double
    Private dblExtrapolationsschritt() As Double


    Public Enum TastschrittRichtung As Integer
        Vorw�rts = 1
        R�ckw�rts = -1
    End Enum

    'Schnittstellen
    Public Property AnzahlParameter() As Integer
        Get
            Return intAnzahlParameter
        End Get
        Set(ByVal value As Integer)
            If value > 0 Then
                intAnzahlParameter = value
            Else
                Throw New Exception("Die Anzahl der zu optimierenden Parameter muss mindestens 1 sein!")
            End If
        End Set
    End Property
    Public Property StartSchrittweite() As Double
        Get

        End Get
        Set(ByVal value As Double)
            If value > 0 Then
                dblStartSchrittweite = value
            Else
                Throw New Exception("Die Startschrittweite muss groesser 0 sein!")
            End If
        End Set
    End Property
    Public Property MinimaleSchrittweite() As Double
        Get
            Return dblMinimaleSchrittweite
        End Get
        Set(ByVal value As Double)
            If value > 0 Then
                dblMinimaleSchrittweite = value
            Else
                Throw New Exception("Die minimale Schrittweite muss groesser 0 sein!")
            End If
        End Set
    End Property

    Public ReadOnly Property AktuelleSchrittweite() As Double
        Get
            Return dblAktuelleSchrittweite
        End Get
    End Property


    'Konstruktor mit setzen der Anfangsbedingungen
    Public Sub New(ByVal Anzahlparameter As Integer, ByVal StartSchrittweite As Double, ByVal MinimaleSchrittweite As Double)
        Me.AnzahlParameter = Anzahlparameter
        Me.StartSchrittweite = StartSchrittweite
        Me.MinimaleSchrittweite = MinimaleSchrittweite

        ReDim dblStartparameter(Anzahlparameter - 1)
        ReDim dblLetzteParameter(Anzahlparameter - 1)
        ReDim dblLetzteParameterBackup(Anzahlparameter - 1)
        ReDim dblAktuelleParameter(Anzahlparameter - 1)
        ReDim dblExtrapolationsschritt(Anzahlparameter - 1)
    End Sub

    Public Sub Initialize(ByVal OptParameter() As EVO.Common.OptParameter)
        Dim i As Integer

        'Pr�fung
        If OptParameter.GetUpperBound(0) <> intAnzahlParameter - 1 Then
            Throw New Exception("Die Anzahl der �bergebenen Parameter ist nicht gleich der definierten Anzahl!")
        End If
        'Dynamisches Array wird mit Werten belegt (Vektor der zu optimierenden Parameter)
        For i = 0 To intAnzahlParameter - 1
            If (OptParameter(i).Xn < 0 Or OptParameter(i).Xn > 1) Then
                Throw New Exception("Der Startparameter " & i & " liegt nicht zwischen 0 und 1. Sie m�ssen hier skaliert vorliegen")
            End If
            dblStartparameter(i) = OptParameter(i).Xn
        Next
        dblStartparameter.CopyTo(dblLetzteParameter, 0)
        dblStartparameter.CopyTo(dblLetzteParameterBackup, 0)
        dblStartparameter.CopyTo(dblAktuelleParameter, 0)
        'Startschrittweite wird �bergeben
        dblAktuelleSchrittweite = dblStartSchrittweite

    End Sub

    Public Sub NaechsteIteration()
        dblLetzteParameter = dblAktuelleParameter
    End Sub

    Public Function Tastschritt(ByVal parameter As Integer, ByVal Richtung As TastschrittRichtung) As Double()

        'Pr�fung
        If parameter < 0 Then
            Throw New Exception("Der Index des aufgerufenen Parameters f�r einen Tastschritt muss >= 0 sein!")
        ElseIf parameter > intAnzahlParameter - 1 Then
            Throw New Exception("Der Index des aufgerufenen Parameters f�r einen Tastschritt ist gr��er als die Anzahl der definierten Parameter!")
        End If
        'Tastschritt
        dblAktuelleParameter(parameter) = dblLetzteParameter(parameter) + dblAktuelleSchrittweite * Richtung
        If dblAktuelleParameter(parameter) > 1 Or dblAktuelleParameter(parameter) < 0 Then
            dblAktuelleParameter(parameter) = dblLetzteParameter(parameter)
        End If
        'R�ckgabe des aktuelle Parametervektors
        Return dblAktuelleParameter

    End Function

    Public Function TastschrittResetParameter(ByVal parameter As Integer) As Double()
        'Reset des �bergebenen Parameters
        dblAktuelleParameter(parameter) = dblLetzteParameter(parameter)
        'R�ckgabe des aktuelle Parametervektors
        Return dblAktuelleParameter
    End Function

    Public Sub Extrapolationsschritt()
        Dim i As Integer
        'Bestimmen und Durchf�hren des Extrapolationsschrittes
        For i = 0 To intAnzahlParameter - 1
            'Bestimmen des Schrittes
            dblExtrapolationsschritt(i) = Math.Round((dblAktuelleParameter(i) - dblLetzteParameterBackup(i)), 7)
            'Durchf�hren des Extrapolationsschrittes
            dblLetzteParameter(i) = dblAktuelleParameter(i) + dblExtrapolationsschritt(i)
        Next
        'Backup des letzten Schrittes, falls R�ckschritt erforderlich
        dblAktuelleParameter.CopyTo(dblLetzteParameterBackup, 0)
        dblLetzteParameter.CopyTo(dblAktuelleParameter, 0)
    End Sub

    Public Sub Rueckschritt()
        dblLetzteParameterBackup.CopyTo(dblLetzteParameter, 0)
        dblLetzteParameterBackup.CopyTo(dblAktuelleParameter, 0)
    End Sub

    Public Function getAktuelleParameter() As Double()
        Return dblAktuelleParameter
    End Function

    Public Function getLetzteParameter() As Double()
        Return dblLetzteParameter
    End Function

    Public Sub Schrittweitenhalbierung()
        dblAktuelleSchrittweite = dblAktuelleSchrittweite / 2
    End Sub
End Class
