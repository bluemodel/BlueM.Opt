﻿Public Class Individuum_CES
    Inherits Individuum

    Private Shared n_Locations As Integer
    Public Path() As Integer               '03 Der Pfad
    Public mutated As Boolean              '06 Gibt an ob der Wert bereits mutiert ist oder nicht

    'Information pro Location ---------------------------------------
    Public Measures() As String            '09a Die Namen der Maßnahmen
    Public Loc() As Location_Data          '10 + 11a Information pro Location

    Public PES_OptParas() As OptParameter  '06a Parameterarray für PES

    'Für PES Memory -------------------------------------------------
    Public Generation As Integer           '12 Die Generation (eher zur Information)

    'Für PES Parent -------------------------------------------------
    Public Memory_Strat As MEMORY_STRATEGY '13 Memory_Strategie des PES Elters
    Public iLocation As Integer            '14 Location des PES Parent

    'Informationen pro Location
    '**************************
    Public Structure Location_Data

        Dim Loc_Elem() As String            '11a Die Elemente die zur Location gehören
        Dim PES_OptPara() As OptParameter   'Array für das Speicherrn der PES Parameter

    End Structure

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

    'Gibt ein Array mit den PES Parametern aller Locations zurück
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    Public ReadOnly Property Get_All_Loc_PES_Para() As Double()
        Get
            Dim i, j, x As Integer
            Dim array(-1) As Double
            x = 0
            For i = 0 To Loc.GetUpperBound(0)
                For j = 0 To Loc(i).PES_OptPara.GetUpperBound(0)
                    ReDim Preserve array(x)
                    array(x) = Loc(i).PES_OptPara(j).Xn
                    x += 1
                Next
            Next
            Return Array
        End Get
    End Property

    'Gibt ein Array mit den DNs Parametern aller Locations zurück
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    Public ReadOnly Property Get_All_Loc_PES_Dn() As Double()
        Get
            Dim i, j, x As Integer
            Dim Array(-1) As Double
            x = 0
            For i = 0 To Loc.GetUpperBound(0)
                For j = 0 To Loc(i).PES_OptPara.GetUpperBound(0)
                    ReDim Preserve Array(x)
                    Array(x) = Loc(i).PES_OptPara(j).Dn
                    x += 1
                Next
            Next
            Return Array
        End Get

    End Property

    'Gibt ein Array mit den Elementen aller Locations zurück
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    Public ReadOnly Property Get_All_Loc_Elem() As String()
        Get
            Dim i As Integer
            Dim Array() As String = {}
            For i = 0 To Loc.GetUpperBound(0)
                If Loc(i).Loc_Elem.GetLength(0) = 0 Then
                    Throw New Exception("Die Element Gesamtliste wurde abgerufen bevor die Elemente pro Location ermittelt wurden")
                End If
                ReDim Preserve Array(Array.GetUpperBound(0) + Loc(i).Loc_Elem.GetLength(0))
                System.Array.Copy(Loc(i).Loc_Elem, 0, Array, Array.GetUpperBound(0) - Loc(i).Loc_Elem.GetUpperBound(0), Loc(i).Loc_Elem.GetLength(0))
            Next
            Return Array
        End Get
    End Property


    'Schreibt alle Parameter aus der DB zurück ins Individuum
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    Public WriteOnly Property SetAll_Para() As Double()
        Set(ByVal Array() As Double)
            Dim i, j, x As Integer
            For i = 0 To Loc.GetUpperBound(0)
                For j = 0 To Loc(i).PES_OptPara.GetUpperBound(0)
                    Loc(i).PES_OptPara(j).Xn = Array(x)
                    x += 1
                Next
            Next
        End Set
    End Property

    'Initialisiert die CES_Individuum Klasse
    '***************************************
    Public Overloads Shared Sub Initialise(ByVal _Individ_Type As Integer, ByVal _n_Locations As Integer, ByVal _n_Para As Integer)
        Individuum_CES.Individ_Type = _Individ_Type
        Individuum_CES.n_Locations = _n_Locations
        Individuum_CES.n_Para = _n_Para
    End Sub

    'Konstruktor für ein Individuum
    '******************************
    Public Sub New(ByVal _Type As String, ByVal _ID As Integer)

        Call MyBase.New(_Type, _ID)

        Dim i, j As Integer

        '03 Der Pfad - zur Kontrolle wird falscher Pfad gesetzt
        ReDim Me.Path(n_Locations - 1)
        For j = 0 To Me.Path.GetUpperBound(0)
            Me.Path(j) = 777
        Next

        '06 Gibt an ob der Wert bereits mutiert ist oder nicht
        Me.mutated = False

        '06a Parameterarray für PES
        ReDim Me.PES_OptParas(n_Para - 1)
        For i = 0 To Me.PES_OptParas.GetUpperBound(0)
            Me.PES_OptParas(i) = New OptParameter()
        Next

        '09a Die Namen der Maßnahmen
        ReDim Me.Measures(n_Locations - 1)

        '11 + 10 Informationen pro Location
        ReDim Me.Loc(n_Locations - 1)

        For i = 0 To Me.Loc.GetUpperBound(0)

            ReDim Me.Loc(i).PES_OptPara(-1)

            '11a Die Elemente die zur Location gehören
            ReDim Me.Loc(i).Loc_Elem(0)
            For j = 0 To Me.Loc(i).Loc_Elem.GetUpperBound(0)
                Me.Loc(i).Loc_Elem(j) = "Leer"
            Next
        Next

        '12 Die Generation (eher zur Information)
        Me.Generation = 0

        '13 MemoryRang des PES Elters
        Me.Memory_Strat = 777

        '14 Location des PES Parent
        Me.iLocation = 777

    End Sub

    'Kopiert ein Individuum
    '**********************
    Public Overrides Function Clone() As Individuum

        Dim i, j As Integer
        Dim Dest As Individuum_CES

        Dest = MyBase.Clone()

        '03 Der Pfad - zur Kontrolle wird falscher Pfad gesetzt
        ReDim Dest.Path(Me.Path.GetUpperBound(0))
        Array.Copy(Me.Path, Dest.Path, Me.Path.Length)

        '06a Array für PES Parameter
        If Me.PES_OptParas.GetUpperBound(0) = -1 Then
            ReDim Dest.PES_OptParas(-1)
        Else
            ReDim Dest.PES_OptParas(Me.PES_OptParas.GetUpperBound(0))
            For i = 0 To Me.PES_OptParas.GetUpperBound(0)
                Dest.PES_OptParas(i) = Me.PES_OptParas(i).Clone
            Next
        End If

        '06 Gibt an ob der Wert bereits mutiert ist oder nicht
        Dest.mutated = Me.mutated

        '09a Die Namen der Maßnahmen
        ReDim Dest.Measures(Me.Measures.GetUpperBound(0))
        Array.Copy(Me.Measures, Dest.Measures, Me.Measures.Length)

        '10 + 11 Die PES Informationen
        ReDim Dest.Loc(Me.Loc.GetUpperBound(0))

        For i = 0 To Me.Loc.GetUpperBound(0)

            'Falls nur CES gibt es keine OptParameter
            If (Me.Loc(i).PES_OptPara.GetUpperBound(0) = -1) Then
                ReDim Dest.Loc(i).PES_OptPara(-1)
            Else
                ReDim Dest.Loc(i).PES_OptPara(Me.Loc(i).PES_OptPara.GetUpperBound(0))
                For j = 0 To Me.Loc(i).PES_OptPara.GetUpperBound(0)
                    Dest.Loc(i).PES_OptPara(j) = Me.Loc(i).PES_OptPara(j).Clone()
                Next
            End If

            '11a Die Elemente die zur Location gehören
            ReDim Dest.Loc(i).Loc_Elem(Me.Loc(i).Loc_Elem.GetUpperBound(0))
            Array.Copy(Me.Loc(i).Loc_Elem, Dest.Loc(i).Loc_Elem, Me.Loc(i).Loc_Elem.Length)
        Next

        '12 Die Generation (eher zur Information)
        Dest.Generation = Me.Generation

        '13 MemoryRang des PES Elters
        Dest.Memory_Strat = Me.Memory_Strat

        '14 Location des PES Parent
        Dest.iLocation = Me.iLocation

        Return Dest

    End Function

    'Konstruktor für ein Array von Individen
    '***************************************
    Public Shared Sub New_Indi_Array(ByVal _Type As String, ByRef Array() As Individuum_CES)
        Dim i As Integer

        For i = 0 To Array.GetUpperBound(0)
            Array(i) = New Individuum_CES(_Type, i)
        Next
    End Sub



End Class
