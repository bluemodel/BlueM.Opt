Public Class Individuum_CES
    Inherits Individuum

    Private Shared n_Locations As Integer

    Public Path() As Integer               '03 Der Pfad
    Public mutated As Boolean              '06 Gibt an ob der Wert bereits mutiert ist oder nicht

    'Information pro Location ---------------------------------------
    Public Measures() As String            '09a Die Namen der Maßnahmen
    Public Loc() As Location_Data          '10 + 11a Information pro Location

    'Nur fuer die Datenbank ollte sonst nicht verwendet werden!
    Public PES_OptParas_fuer_DB() As OptParameter  '06a Parameterarray für PES

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

    'Gibt ein Array mit den PES_Opt_Parametern aller Locations zurück
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    Public Property Get_a_Set_All_Loc_PES_Opt_Para() As OptParameter()
        Get
            Dim i, j, x As Integer
            Dim array(-1) As OptParameter
            x = 0
            For i = 0 To Loc.GetUpperBound(0)
                For j = 0 To Loc(i).PES_OptPara.GetUpperBound(0)
                    ReDim Preserve array(x)
                    array(x) = Loc(i).PES_OptPara(j).Clone
                    x += 1
                Next
            Next
            Return array
        End Get

        Set(ByVal Array() As OptParameter)
            Dim i, j, x As Integer

            x = 0
            For i = 0 To Loc.GetUpperBound(0)
                For j = 0 To Loc(i).PES_OptPara.GetUpperBound(0)
                    Loc(i).PES_OptPara(j) = Array(x).Clone
                    x += 1
                Next
            Next
        End Set
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

    'Gibt ein Array mit den DNs aller Locations zurück
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

    'Gibt das durchschnittliche DN zurück
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    Public ReadOnly Property Get_mean_PES_Dn() As Double
        Get
            Dim n As Integer = 0
            Dim sum As Double = 0
            Dim i As Integer
            Dim Dn_Mean As Double

            n = Me.Get_All_Loc_PES_Dn.GetLength(0)
            If n = 0 Then
                Return -1
            End If

            For i = 0 To Me.Get_All_Loc_PES_Dn.GetUpperBound(0)
                sum = sum + Me.Get_All_Loc_PES_Dn(i)
            Next
            Dn_Mean = sum / n

            Return Dn_Mean
        End Get

    End Property

    'Setzt das durchnittliche Dn für alle OptParas
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    Public WriteOnly Property Set_mean_PES_Dn() As Double
        Set(ByVal Dn_Mean As Double)
            Dim i, j As Integer

            For i = 0 To Loc.GetUpperBound(0)
                For j = 0 To Loc(i).PES_OptPara.GetUpperBound(0)
                    Loc(i).PES_OptPara(j).Dn = Dn_Mean
                Next
            Next

        End Set
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

    ''' <summary>
    ''' Initialisiert die CES-Individuumsklasse
    ''' </summary>
    ''' <param name="AnzahlParameter">Anzahl der Parameter, die jedes Individuum besitzen soll</param>
    ''' <param name="AnzahlLocations">Anzahl der Locations, die jedes Individuum besitzen soll</param>
    ''' <remarks></remarks>
    Public Overloads Shared Sub Initialise(ByVal AnzahlParameter As Integer, ByVal AnzahlLocations As Integer)
        Individuum_CES.n_Locations = AnzahlLocations
        Individuum_CES.n_Para = AnzahlParameter
    End Sub

    ''' <summary>
    ''' Konstruktor
    ''' </summary>
    ''' <param name="type">Frei definierbarer String</param>
    ''' <param name="id">Eindeutige Nummer</param>
    Public Sub New(ByVal type As String, ByVal id As Integer)

        'Basisindividuum instanzieren
        Call MyBase.New(type, id)

        'zusätzliche Individuum_CES-Eigenschaften:
        '-----------------------------------------
        Dim i, j As Integer

        'Der Pfad - zur Kontrolle wird falscher Pfad gesetzt
        ReDim Me.Path(n_Locations - 1)
        For j = 0 To Me.Path.GetUpperBound(0)
            Me.Path(j) = 777
        Next

        'Gibt an ob der Wert bereits mutiert ist oder nicht
        Me.mutated = False

        'Parameterarray für PES
        '(eigentlich nur bei METH_HYBRID gebraucht)
        ReDim Me.PES_OptParas_fuer_DB(n_Para - 1)
        For i = 0 To Me.PES_OptParas_fuer_DB.GetUpperBound(0)
            Me.PES_OptParas_fuer_DB(i) = New OptParameter()
        Next

        'Die Namen der Maßnahmen
        ReDim Me.Measures(n_Locations - 1)

        'Informationen pro Location
        ReDim Me.Loc(n_Locations - 1)

        For i = 0 To Me.Loc.GetUpperBound(0)

            ReDim Me.Loc(i).PES_OptPara(-1)

            'Die Elemente die zur Location gehören
            ReDim Me.Loc(i).Loc_Elem(0)
            For j = 0 To Me.Loc(i).Loc_Elem.GetUpperBound(0)
                Me.Loc(i).Loc_Elem(j) = "Leer"
            Next
        Next

        'Die Generation (eher zur Information)
        Me.Generation = 0

        'MemoryRang des PES Elters
        Me.Memory_Strat = 777

        'Location des PES Parent
        Me.iLocation = 777

    End Sub

    ''' <summary>
    ''' Kopiert ein Individuum
    ''' </summary>
    ''' <returns>Individuum</returns>
    Public Overrides Function Clone() As Individuum

        Dim i, j As Integer

        Dim Dest As New Individuum_CES(Me.mType, Me.ID)

        'Feature-Werte
        Call Array.Copy(Me.Features, Dest.Features, Me.Features.Length)

        'Constraint-Werte
        If (Not Me.Constraints.GetLength(0) = -1) Then
            Array.Copy(Me.Constraints, Dest.Constraints, Me.Constraints.Length)
        End If

        'Kennzeichnung ob Dominiert
        Dest.Dominated = Me.Dominated

        'Nummer der Pareto Front
        Dest.Front = Me.Front

        'Für crowding distance
        Dest.Distance = Me.Distance

        'CES-Spezifische Eigenschaften:
        '------------------------------

        '03 Der Pfad - zur Kontrolle wird falscher Pfad gesetzt
        ReDim Dest.Path(Me.Path.GetUpperBound(0))
        Array.Copy(Me.Path, Dest.Path, Me.Path.Length)

        '06a Array für PES Parameter
        If Me.PES_OptParas_fuer_DB.GetUpperBound(0) = -1 Then
            ReDim Dest.PES_OptParas_fuer_DB(-1)
        Else
            ReDim Dest.PES_OptParas_fuer_DB(Me.PES_OptParas_fuer_DB.GetUpperBound(0))
            For i = 0 To Me.PES_OptParas_fuer_DB.GetUpperBound(0)
                Dest.PES_OptParas_fuer_DB(i) = Me.PES_OptParas_fuer_DB(i).Clone
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

    ''' <summary>
    ''' Erzeugt ein neues (leeres) Individuum von der gleichen Klasse
    ''' </summary>
    ''' <returns>Das neue Individuum</returns>
    Public Overrides Function Create(Optional ByVal type As String = "tmp", Optional ByVal id As Integer = 0) As Individuum
        Dim ind As New Individuum_CES(type, id)
        Return ind
    End Function

#Region "Shared"

    ''' <summary>
    ''' Kopiert ein Array von CES-Individuen
    ''' </summary>
    ''' <param name="Source">zu kopierendes Array von CES-Individuen</param>
    ''' <returns>Array von CES-Individuen</returns>
    Public Overloads Shared Function Clone_Indi_Array(ByVal Source() As Individuum_CES) As Individuum_CES()

        Dim i As Integer
        Dim ClonedArray() As Individuum_CES

        ReDim ClonedArray(Source.GetUpperBound(0))

        For i = 0 To Source.GetUpperBound(0)
            ClonedArray(i) = Source(i).Clone()
        Next

        Return ClonedArray

    End Function

#End Region 'Shared

End Class
