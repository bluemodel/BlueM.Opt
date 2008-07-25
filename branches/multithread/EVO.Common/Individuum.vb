Public Class Individuum

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse Individuum für das Speichern eines Evaluierungssatzes          ****
    '****                                                                       ****
    '**** Autoren: Christoph Hübner, Felix Fröhlich                             ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

#Region "Eigenschaften"

    'Variablen der Klasse
    '********************
    Private Shared Individ_Type As Integer
    Private Shared n_Locations As Integer
    Private Shared n_Para As Integer

    'Strukturen der Klasse
    '*********************

    Public Type As String                  '01 Typ des Individuum
    Public ID As Integer                   '02 Nummer des Individuum
    Public Path() As Integer               '03 Der Pfad

    Public Zielwerte() As Double           '04 Array aller Zielfunktionswerte (inkl. sekundär)
    Public Constrain() As Double           '05 Werte der Randbedingungen (Wenn negativ dann ungültig)
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
    Public Memory_Strat As MEMORY_STRATEGY '13 Memory_Strategie des PES Elters
    Public iLocation As Integer            '14 Location des PES Parent

    'Informationen pro Location
    '**************************
    Public Structure Location_Data

        Dim Loc_Elem() As String            '11a Die Elemente die zur Location gehören
        Dim PES_OptPara() As OptParameter   'Array für das Speicherrn der PES Parameter

    End Structure

    'Gibt die Array mit den Penalties zurück
    '(d.h. Zielfunktionswerte nur von OptZielen)
    '*******************************************
    Public ReadOnly Property Penalties() As Double()
        Get
            Dim i, j As Integer
            Dim Array() As Double

            ReDim Array(Common.Manager.AnzPenalty - 1)

            j = 0
            For i = 0 To Common.Manager.AnzZiele - 1
                'Nur die Zielfunktionswerte von OptZielen zurückgeben!
                If (Common.Manager.List_Ziele(i).isOpt) Then
                    Array(j) = Me.Zielwerte(i)
                    j += 1
                End If
            Next
            Return Array
        End Get
    End Property

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

    'Gibt zurück ob Individuum feasible ist
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    Public ReadOnly Property Is_Feasible() As Boolean
        Get
            For i As Integer = 0 To Me.Constrain.GetUpperBound(0)
                If (Me.Constrain(i) < 0) Then Return False
            Next
            Return True
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

#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

    'Initialisiert ein Individuum
    '****************************
    Public Shared Sub Initialise(ByVal _Individ_Type As Integer, ByVal _n_Locations As Integer, ByVal _n_Para As Integer)
        Individuum.Individ_Type = _Individ_Type
        Individuum.n_Locations = _n_Locations
        Individuum.n_Para = _n_Para
    End Sub

    'Konstruktor für ein Individuum
    '******************************
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

        '04 Zielfunktionswerte
        ReDim Me.Zielwerte(Common.Manager.AnzZiele - 1)
        For j = 0 To Common.Manager.AnzZiele - 1
            Me.Zielwerte(j) = Double.MaxValue           'mit maximalem Double-Wert initialisieren
        Next

        '05 Wert der Randbedingung(en)
        ReDim Me.Constrain(Common.Manager.AnzConstraints - 1)
        For j = 0 To Common.Manager.AnzConstraints - 1
            Me.Constrain(j) = Double.MinValue           'mit minimalem Double-Wert initialisieren
        Next

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
        Me.Memory_Strat = 777

        '14 Location des PES Parent
        Me.iLocation = 777

    End Sub

    'Konstruktor für ein Array von Individen
    '***************************************
    Public Shared Sub New_Indi_Array(ByVal _Type As String, ByRef Array() As Individuum)
        Dim i As Integer

        For i = 0 To Array.GetUpperBound(0)
            Array(i) = New Individuum(_Type, i)
        Next
    End Sub


    'Kopiert ein Individuum
    '**********************
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

        '04 Zielfunktionswerte
        Call Array.Copy(Me.Zielwerte, Dest.Zielwerte, Me.Zielwerte.Length)

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

    'Kopiert ein Array von Individuuen
    '*********************************
    Public Shared Sub Clone_Indi_Array(ByVal Source() As Individuum, ByRef Dest() As Individuum)
        Dim i As Integer

        For i = 0 To Source.GetUpperBound(0)
            Dest(i) = Source(i).Clone()
        Next
    End Sub

    'ES_GET_SEKUNDÄRE_POPULATIONEN - Sekundäre Population speichert immer die angegebene
    'Anzahl von Bestwerten und kann den Bestwertspeicher alle x Generationen überschreiben
    '*************************************************************************************
    Public Shared Function Get_All_Penalty_of_Array(ByVal Indi_Array() As Individuum) As Double(,)
        Dim j, i As Integer
        Dim Array(,) As Double

        ReDim Array(Indi_Array.GetUpperBound(0), Manager.AnzPenalty - 1)

        For i = 0 To Indi_Array.GetUpperBound(0)
            For j = 0 To Manager.AnzPenalty - 1
                Array(i, j) = Indi_Array(i).Penalties(j)
            Next j
        Next i
        Return Array
    End Function

#End Region 'Methoden

End Class
