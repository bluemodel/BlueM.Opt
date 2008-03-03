Public Class Individuum

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse Individuum für das Speichern eines Evaluierungssatzes          ****
    '****                                                                       ****
    '**** Christoph Hübner                                                      ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '****                                                                       ****
    '**** November 2007                                                         ****
    '****                                                                       ****
    '**** Letzte Änderung: November 2007                                        ****
    '*******************************************************************************
    '*******************************************************************************

#Region "Eigenschaften"

    'Variablen der Klasse
    '********************
    Private Shared Individ_Type As Integer
    Private Shared n_Locations As Integer
    Private Shared n_Para As Integer
    Private Shared n_Penalty As Integer
    Private Shared n_Constrain As Integer

    Public Shared Sub Initialise(ByVal _Individ_Type As Integer, ByVal _n_Locations As Integer, ByVal _n_Para As Integer, ByVal _n_Penalty As Integer, ByVal _n_Constrain As Integer)
        Individuum.Individ_Type = _Individ_Type
        Individuum.n_Locations = _n_Locations
        Individuum.n_Para = _n_Para
        Individuum.n_Penalty = _n_Penalty
        Individuum.n_Constrain = _n_Constrain
    End Sub


    'Strukturen der Klasse
    '*********************

    Public Type As String                  '01 Typ des Individuum
    Public ID As Integer                   '02 Nummer des Individuum
    Public Path() As Integer               '03 Der Pfad
    Public Penalty() As Double             '04 Werte der Penaltyfunktion(en)
    Public Constrain() As Double           '05 Wert der Randbedingung(en)
    Public mutated As Boolean              '06 Gibt an ob der Wert bereits mutiert ist oder nicht

    'Für ND Sorting -------------------------------------------------
    Public PES_OptParas() As OptParameter  '06a Parameterarray für PES
    Public dominated As Boolean            '07 Kennzeichnung ob Dominiert
    Public Front As Integer                '08 Nummer der Pareto Front
    Public Distance As Double              '09 Für crowding distance

    'Information pro Location ---------------------------------------
    Public Measures() As String            '09a Die Namen der Maßnahmen
    Public Loc() As Location_Data          '10 + 11a Information pro Location

    'Für PES Memory -------------------------------------------------
    Public Generation As Integer           '12 Die Generation (eher zur Information)

    'Für PES Parent -------------------------------------------------
    Public Memory_Rank As Integer          '13 MemoryRang des PES Elters
    Public iLocation As Integer            '14 Location des PES Parent

    'Gibt zurück ob Individuum gültig ist
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    Public ReadOnly Property feasible() As Boolean
        Get
            For i As Integer = 0 To Me.Constrain.GetUpperBound(0)
                If (Me.Constrain(i) < 0) Then Return False
            Next
            Return True
        End Get
    End Property

    'Gibt ein Array mit den Elementen aller Locations zurück
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    Public ReadOnly Property All_Elem() As String ()
        Get
            Dim i As Integer
            Dim array() As String = {}
            For i = 0 To Loc.GetUpperBound(0)
                If Loc(i).Loc_Elem.GetLength(0) = 0 Then
                    Throw New Exception("Die Element Gesamtliste wurde abgerufen bevor die Elemente pro Location ermittelt wurden")
                End If
                ReDim Preserve array(array.GetUpperBound(0) + Loc(i).Loc_Elem.GetLength(0))
                System.Array.Copy(Loc(i).Loc_Elem, 0, array, array.GetUpperBound(0) - Loc(i).Loc_Elem.GetUpperBound(0), Loc(i).Loc_Elem.GetLength(0))
            Next
            All_Elem = array.Clone
        End Get
    End Property

    'Gibt ein Array mit den Parametern aller Locations zurück !oder!
    'Setzt die Zahl der locations auf 1 und schreibt dort alle Parameter rein
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    Public ReadOnly Property All_PES_Para() As Double()
        Get
            Dim i As Integer
            Dim Array(-1) As Double
            For i = 0 To PES_OptParas.GetUpperBound(0)
                ReDim Preserve Array(Array.GetLength(0))
                Array(Array.GetUpperBound(0)) = PES_OptParas(i).Xn
            Next
            All_PES_Para = Array.Clone
        End Get
    End Property


    'Gibt ein Array mit den Parametern aller Locations zurück !oder!
    'Setzt die Zahl der locations auf 1 und schreibt dort alle Parameter rein
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    Public ReadOnly Property All_Loc_Para() As Double()
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
            All_Loc_Para = array.Clone
        End Get
    End Property

    'Schreibt alle Parameter aus der DB zurück ins Individuum
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    Public WriteOnly Property SetAll_Para() as double()
        Set (Array() as Double)
            Dim i, j, x as Integer
            For i = 0 To Loc.GetUpperBound(0)
                For j = 0 to loc(i).PES_OptPara.GetUpperBound(0)
                    loc(i).PES_OptPara(j).Xn = array(x)
                    x += 1
                Next
            Next
        End Set
    End Property


    'Gibt ein Array mit den DNs aller Locations zurück !oder!
    'Setzt die Zahl der locations auf 1 und schreibt dort alle DNs rein
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    Public ReadOnly Property All_DN() As Double()
        Get
            Dim i, j, x As Integer
            Dim array(-1) As Double
            x = 0
            For i = 0 To Loc.GetUpperBound(0)
                For j = 0 To Loc(i).PES_OptPara.GetUpperBound(0)
                    ReDim Preserve array(x)
                    array(x) = Loc(i).PES_OptPara(j).Dn
                    x += 1
                Next
            Next
            All_DN = array.Clone
        End Get

    End Property


    'Informationen pro Location
    '**************************
    Public Structure Location_Data

        Dim Loc_Elem() As String            '11a Die Elemente die zur Location gehören
        Dim PES_OptPara() As OptParameter   'Array für das Speicherrn der PES Parameter

    End Structure

#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

    'Konstruktor
    '***********
    Public Sub New(ByVal _Type As String, ByVal _ID As Integer)

        Dim i, j As Integer

        '01 Typ des Individuum
        Me.Type = _Type

        '02 Nummer des Individuum
        Me.ID = _ID

        '03 Der Pfad - zur Kontrolle wird falscher Pfad gesetzt
        ReDim Me.Path(n_Locations - 1)
        For j = 0 To Me.Path.GetUpperBound(0)
            Me.Path(j) = 777
        Next

        '04 Werte der Penaltyfunktion(en)
        ReDim Me.Penalty(n_Penalty - 1)
        For j = 0 To n_Penalty - 1
            Me.Penalty(j) = 1.0E+300
        Next

        '05 Wert der Randbedingung(en)
        If n_Constrain = 0 Then
            ReDim Me.Constrain(-1)
        Else
            ReDim Me.Constrain(n_Constrain - 1)
            For j = 0 To Me.Constrain.GetUpperBound(0)
                Me.Constrain(j) = -1.0E+300
            Next
        End If

        '06 Gibt an ob der Wert bereits mutiert ist oder nicht
        Me.mutated = False

        '06a Parameterarray für PES
        ReDim Me.PES_OptParas(n_Para - 1)
        For i = 0 To Me.PES_OptParas.GetUpperBound(0)
            Me.PES_OptParas(i) = New OptParameter()
        Next

        '07 Kennzeichnung ob Dominiert
        Me.dominated = False

        '08 Nummer der Pareto Front
        Me.Front = 0

        '09 Für crowding distance
        Me.Distance = 0

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
        Me.Memory_Rank = 777

        '14 Location des PES Parent
        Me.iLocation = 777

    End Sub

    Public Shared Sub New_Array(ByVal _Type As String, ByRef Array() As Individuum)
        Dim i As Integer

        For i = 0 To Array.GetUpperBound(0)
            Array(i) = New Individuum(_Type, i)
        Next
    End Sub


    'Überladen Methode die ein Individuum kopiert
    '*********************************************
    Public Function Clone() As Individuum

        Dim i, j As Integer
        Dim Dest As New Individuum(Me.Type, Me.ID)

        '01 Typ des Individuum
        'Schon in der Deklaration

        '02 Nummer des Individuum
        'Schon in der Deklaration

        '03 Der Pfad - zur Kontrolle wird falscher Pfad gesetzt
        ReDim Dest.Path(Me.Path.GetUpperBound(0))
        Array.Copy(Me.Path, Dest.Path, Me.Path.Length)

        '04 Werte der Penaltyfunktion(en)
        ReDim Dest.Penalty(Me.Penalty.GetUpperBound(0))
        Array.Copy(Me.Penalty, Dest.Penalty, Me.Penalty.Length)

        '05 Wert der Randbedingung(en)
        ReDim Dest.Constrain(Me.Constrain.GetUpperBound(0))
        If Not Me.Constrain.GetLength(0) = -1 Then
            Array.Copy(Me.Constrain, Dest.Constrain, Me.Constrain.Length)
        End If

        '06 Gibt an ob der Wert bereits mutiert ist oder nicht
        Dest.mutated = Me.mutated

        '06a Array für PES Parameter
        If Me.PES_OptParas.GetUpperBound(0) = -1 Then
            ReDim Dest.PES_OptParas(-1)
        Else
            ReDim Dest.PES_OptParas(Me.PES_OptParas.GetUpperBound(0))
            For i = 0 To Me.PES_OptParas.GetUpperBound(0)
                Dest.PES_OptParas(i) = Me.PES_OptParas(i).Clone
            Next
        End If

        '07 Kennzeichnung ob Dominiert
        Dest.dominated = Me.dominated

        '08 Nummer der Pareto Front
        Dest.Front = Me.Front

        '09 Für crowding distance
        Dest.Distance = Me.Distance

        '09a Die Namen der Maßnahmen
        ReDim Dest.Measures(Me.Measures.GetUpperBound(0))
        Array.Copy(Me.Measures, Dest.Measures, Me.Measures.Length)

        '10 + 11 Die PES Informationen
        ReDim Dest.Loc(Me.Loc.GetUpperBound(0))

        For i = 0 To Me.Loc.GetUpperBound(0)

            'Falls nur CES gibt es keine OptParameter
            If (Me.loc(i).PES_OptPara.GetUpperBound(0) = -1) Then
                ReDim Dest.loc(i).PES_OptPara(-1)
            Else
                ReDim Dest.loc(i).PES_OptPara(Me.Loc(i).PES_OptPara.GetUpperBound(0))
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
        Dest.Memory_Rank = Me.Memory_Rank

        '14 Location des PES Parent
        Dest.iLocation = Me.iLocation

        Return Dest

    End Function

    'Überladen Methode die ein Array aus Individuen kopiert
    '******************************************************
    Public Shared Sub Copy_Array(ByVal Source() As Individuum, ByRef Dest() As Individuum)
        Dim i As Integer

        For i = 0 To Source.GetUpperBound(0)
            Dest(i) = Source(i).Clone()
        Next
    End Sub

    'Achtung Überladen!
    Public Shared Function QN_RN_Indi(ByVal n As Integer, ByRef QN() As Double, ByRef RN() As Double, ByVal MyPara() As Kern.OptParameter) As Individuum

        Dim Indi As New Individuum("QN_RN_Indi", 0)

        'Achtung hier wird bewust per Reference!
        Indi.Penalty = QN
        Indi.Constrain = RN

        'Hier nicht per Reference!
        Indi.ID = n
        Indi.PES_OptParas = MyPara.Clone

        'Hier per Reference
        QN_RN_Indi = Indi

    End Function

    'Achtung Überladen!
    Public Shared Function QN_RN_Indi(ByVal n As Integer, ByRef QN() As Double, ByRef RN() As Double, ByVal MyPara() As Double) As Individuum
        Dim i As Integer
        Dim Indi As New Individuum("QN_RN_Indi", 0)

        'Achtung hier wird bewust per Reference!
        Indi.Penalty = QN
        Indi.Constrain = RN

        'Hier nicht per Reference!
        Indi.ID = n
        For i = 0 To MyPara.GetUpperBound(0)
            Indi.PES_OptParas(i).Xn = MyPara(i)
        Next

        'Hier per Reference
        QN_RN_Indi = Indi

    End Function

#End Region 'Methoden

End Class
