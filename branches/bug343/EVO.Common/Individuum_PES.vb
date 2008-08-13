﻿Public Class Individuum_PES
    Inherits Individuum

    Public PES_OptParas() As OptParameter  '06a Parameterarray für PES

    'Gibt ein Array mit den PES Parametern zurück
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    Public ReadOnly Property Get_All_PES_Para() As Double()
        Get
            Dim i As Integer
            Dim Array(-1) As Double
            For i = 0 To PES_OptParas.GetUpperBound(0)
                ReDim Preserve Array(Array.GetLength(0))
                Array(Array.GetUpperBound(0)) = PES_OptParas(i).Xn
            Next
            Return Array
        End Get
    End Property

    'Operator (geht nicht!)
    '********
    'Public Shared Narrowing Operator CType(ByVal ind As Individuum) As Individuum_PES

    'End Operator

    'Initialisiert die PES-Individuumsklasse
    '***************************************
    Public Shared Sub Initialise(ByVal _Individ_Type As Integer, ByVal _n_Para As Integer)
        Individuum.Individ_Type = _Individ_Type
        Individuum.n_Para = _n_Para
    End Sub

    'Konstruktor für ein Individuum
    '******************************
    Public Sub New(ByVal _Type As String, ByVal _ID As Integer)

        Call MyBase.New(_Type, _ID)

        Dim i, j As Integer

        '06a Parameterarray für PES
        ReDim Me.PES_OptParas(n_Para - 1)
        For i = 0 To Me.PES_OptParas.GetUpperBound(0)
            Me.PES_OptParas(i) = New OptParameter()
        Next

    End Sub

    'Kopiert ein Individuum
    '**********************
    Public Overrides Function Clone() As Individuum

        Dim i, j As Integer

        Dim Dest As New Individuum_PES(Me.Type, Me.ID)

        '04 Zielfunktionswerte
        Call Array.Copy(Me.Zielwerte, Dest.Zielwerte, Me.Zielwerte.Length)

        '05 Wert der Randbedingung(en)
        ReDim Dest.Constrain(Me.Constrain.GetUpperBound(0))
        If Not Me.Constrain.GetLength(0) = -1 Then
            Array.Copy(Me.Constrain, Dest.Constrain, Me.Constrain.Length)
        End If

        '07 Kennzeichnung ob Dominiert
        Dest.dominated = Me.dominated

        '08 Nummer der Pareto Front
        Dest.Front = Me.Front

        '09 Für crowding distance
        Dest.Distance = Me.Distance

        '06a Array für PES Parameter
        If Me.PES_OptParas.GetUpperBound(0) = -1 Then
            ReDim Dest.PES_OptParas(-1)
        Else
            ReDim Dest.PES_OptParas(Me.PES_OptParas.GetUpperBound(0))
            For i = 0 To Me.PES_OptParas.GetUpperBound(0)
                Dest.PES_OptParas(i) = Me.PES_OptParas(i).Clone
            Next
        End If
        Return Dest

    End Function

    'Konstruktor für ein Array von Individen
    '***************************************
    Public Shared Sub New_Indi_Array(ByVal _Type As String, ByRef Array() As Individuum)
        Dim i As Integer

        For i = 0 To Array.GetUpperBound(0)
            Array(i) = New Individuum_PES(_Type, i)
        Next
    End Sub

    Public Overrides Function Create(Optional ByVal type As String = "tmp", Optional ByVal id As Integer = 0) As Individuum
        Dim ind As New Individuum_PES(type, id)
        Return ind
    End Function

End Class