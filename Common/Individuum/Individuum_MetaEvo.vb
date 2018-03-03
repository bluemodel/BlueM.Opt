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
Public Class Individuum_MetaEvo
    Inherits Individuum

    'Optparas: Eingabewerte

    'Primobjectives: Ergebnisse der zu minimierenden Eigenschaftsfunktionen (Teil der Objectives)
    'Constraints: Ergebnisse der Randbedingungsfunktionen (Negativ = ungültiges Individuum)
    'Objectives: Ergebnisse der Eigenschaftsfunktionen 

    Private generator_id As Integer         'von welchem Algorithmus das Individuum gemacht wurde
    Private Client As String                'Welcher Rechner dieses Individuum berechnen soll [ip oder Rechnername]
    Private numberOptparas As Integer       'Anzahl der Optparas des Problems
    Private status As String                '{raw, calculate, finished, true, false}
    Private statusreason As String          '{false: dominated, crowding}
    Private statusopponent As Integer        'Individuen-ID durch den dieses Individuum gelöscht wurde
    Private mOptparameter() As OptParameter
    Public feedbackdata(,) As Double        'Spezifischer Feedback für einen Algorithmus
    Private toSimulate As Boolean           'Ob dieses Individuum simuliert werden soll

    '### Initialisierung
    Public Sub New(ByVal type As String, ByVal id As Integer, ByVal numberOptparas_input As Integer)


        'Basisindividuum instanzieren
        Call MyBase.New(type, id)

        Dim i As Integer
        ReDim Me.mOptparameter(numberOptparas_input)
        numberOptparas = numberOptparas_input
        Me.status = "false"
        Me.toSimulate = True

        'Initialisieren der Optparameter
        For i = 0 To numberOptparas - 1
            Me.mOptparameter(i) = New OptParameter()
            Me.mOptparameter(i).Min = Individuum.mProblem.List_OptParameter(i).Min
            Me.mOptparameter(i).Max = Individuum.mProblem.List_OptParameter(i).Max
            Me.mOptparameter(i).Bezeichnung = Individuum.mProblem.List_OptParameter(i).Bezeichnung
        Next

        'Constraints auf 0 setzen
        For i = 0 To Me.Constraints.Length - 1
            Me.Constraints(i) = 0
        Next

        'Objectives auf 0 setzen
        For i = 0 To Me.Objectives.Length - 1
            Me.Objectives(i) = 0
        Next

    End Sub

    '### Überschriebene Methoden

    Public Overrides Function Clone() As Individuum
        Return Me.Clone_MetaEvo()
    End Function

    Public Function Clone_MetaEvo() As Individuum_MetaEvo

        Dim Dest As New Individuum_MetaEvo(Me.mType, Me.ID, Me.numberOptparas)
        Dim i As Integer

        Dest.ID = Me.ID
        Dest.Client = Me.Client
        Dest.status = Me.status
        Dest.statusreason = Me.statusreason
        Dest.statusopponent = Me.statusopponent

        For i = 0 To numberOptparas - 1
            Dest.OptParameter(i) = Me.OptParameter(i).Clone()
        Next

        For i = 0 To Me.Objectives.Length - 1
            Dest.Objectives(i) = Me.Objectives(i)
        Next

        For i = 0 To mConstraints.Length - 1
            Dest.Constraints(i) = Me.Constraints(i)
        Next

        Dest.feedbackdata = Me.feedbackdata
        Dest.toSimulate = False
        Dest.generator_id = Me.generator_id

        Return Dest

    End Function

    Public Overrides Function Create(Optional ByVal type As String = "tmp", Optional ByVal id As Integer = 0) As Individuum
        Dim ind As New Individuum_MetaEvo(type, id, Me.numberOptparas)
        Return ind
    End Function

    Public Overrides Property OptParameter() As OptParameter()
        Get
            Return Me.mOptparameter
        End Get
        Set(ByVal value As OptParameter())
            Me.mOptparameter = value
            Me.toSimulate = True
        End Set
    End Property

    '### Neue Methoden
    'Gene setzen
    Public Function set_optparas(ByVal optparas_input As Double())

        Me.OptParameter_RWerte = optparas_input

        Me.toSimulate = True
        Return True
    End Function

    'Gene zurückgeben
    Public Function get_optparas() As Double()
        Return Me.OptParameter_RWerte
    End Function

    'Optparameter nach Definition zurückgaben
    Public Function get_mOptparas() As OptParameter()
        Return Me.mOptparameter
    End Function

    'Generator setzen
    Public Function set_generator(ByVal generator_input As Integer)
        Me.generator_id = generator_input
        Return True
    End Function

    'Generator auslesen
    Public Function get_generator() As Integer
        Return Me.generator_id
    End Function

    'Client setzen
    Public Function set_Client(ByVal client_input As String)
        Me.Client = client_input
        Return True
    End Function

    'Client auslesen
    Public Function get_client() As String
        Return Me.Client
    End Function

    'Status setzen
    Public Function set_status(ByVal status_input As String)
        If (status_input.Contains("false")) Then
            'Dim stringarray As String(3)
            Me.status = "false"
            Try
                Me.statusreason = status_input.Split("#")(1)
                Me.statusopponent = CInt(status_input.Split("#")(2))
            Catch ex As Exception

            End Try
            Return True
        Else
            Me.status = status_input
            Me.statusreason = ""
            Me.statusopponent = -1
            Return True
        End If
    End Function

    'Status auslesen
    Public Function get_status() As String
        Return Me.status
    End Function

    Public Function get_status_reason() As String
        Return Me.statusreason
    End Function

    Public Function get_status_opponent() As Integer
        Return Me.statusopponent
    End Function

    'Simulieren?
    Public Function set_toSimulate(ByVal toSimulate_input As Boolean)
        Me.toSimulate = toSimulate_input
        Return True
    End Function

    Public Function get_toSimulate() As Boolean
        Return Me.toSimulate
    End Function
End Class
