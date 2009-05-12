Public Class Individuum_CES
    Inherits Individuum

    Public CES_Dn As Double                '02 Dn fuer CES um den Schrittweitenvektor zu verhindern

    Public Path() As Integer               '03 Der Pfad
    Public mutated As Boolean              '06 Gibt an ob der Wert bereits mutiert ist oder nicht

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

    ''' <summary>
    ''' Die OptParameter des aktuellen Pfads, sortiert nach der List_OptParameter_Save
    ''' </summary>
    Public Overrides Property OptParameter() As OptParameter()
        Get
            Dim i, j, k, n As Integer
            Dim found As Boolean
            Dim params() As OptParameter

            ReDim params(-1)
            n = 0

            'Alle definierten OptParameter durchlaufen
            For i = 0 To Individuum.mProblem.List_OptParameter_Save.Length - 1

                found = False

                'Alle aktuellen Locations durchlaufen
                For j = 0 To Me.Loc.Length - 1

                    'OptParameter der Locations durchlaufen
                    For k = 0 To Me.Loc(j).PES_OptPara.Length - 1

                        If (Me.Loc(j).PES_OptPara(k).Bezeichnung = Individuum.mProblem.List_OptParameter_Save(i).Bezeichnung) Then
                            'gefundenen OptParameter zuweisen
                            ReDim Preserve params(n)
                            params(n) = Me.Loc(j).PES_OptPara(k)
                            n += 1
                            found = True
                        End If
                        If (found) Then Exit For

                    Next k
                    If (found) Then Exit For

                Next j

            Next i

            Return params

        End Get

        Set(ByVal params() As OptParameter)

            'Prüfung: Anzahl Parameter
            'TODO: Bei Multithreading sollte nicht auf diese statische Variable zugegriffen werden, sie ändert sich ständig!
            If (params.Length <> Individuum.mProblem.NumOptParams) Then
                Throw New Exception("Falsche Anzahl Parameter übergeben!")
            End If

            Dim i, j, k As Integer
            Dim found As Boolean

            'Übergebene Parameter durchlaufen
            For i = 0 To params.Length - 1

                found = False

                'Alle aktuellen Locations durchlaufen
                For j = 0 To Me.Loc.Length - 1

                    'OptParameter der Locations durchlaufen
                    For k = 0 To Me.Loc(j).PES_OptPara.Length - 1

                        If (Me.Loc(j).PES_OptPara(k).Bezeichnung = params(i).Bezeichnung) Then
                            'gefundenen Parameter zuweisen
                            Me.Loc(j).PES_OptPara(k) = params(i)
                            found = True
                        End If
                        If (found) Then Exit For

                    Next k
                    If (found) Then Exit For

                Next j
                If (Not found) Then Throw New Exception("Konnte Parameter '" & params(i).Bezeichnung & "' nicht in den Locations finden!")

            Next i

        End Set

    End Property

    'Gibt ein Array mit den DNs aller Locations zurück
    'VORSICHT: Reihenfolge stimmt _NICHT_ mit Problem.ListOptParameter überein!
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    Public ReadOnly Property Get_All_Loc_PES_Dn() As Double()
        Get
            Dim i, j, x As Integer
            Dim Dn(-1) As Double
            x = 0
            For i = 0 To Loc.GetUpperBound(0)
                For j = 0 To Loc(i).PES_OptPara.GetUpperBound(0)
                    ReDim Preserve Dn(x)
                    Dn(x) = Loc(i).PES_OptPara(j).Dn
                    x += 1
                Next
            Next
            Return Dn
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

        'Das Dn auf minus zur Kontrolle
        CES_Dn = -1

        'Der Pfad - zur Kontrolle wird falscher Pfad gesetzt
        ReDim Me.Path(Individuum.mProblem.NumLocations - 1)
        For j = 0 To Me.Path.GetUpperBound(0)
            Me.Path(j) = 777
        Next

        'Gibt an ob der Wert bereits mutiert ist oder nicht
        Me.mutated = False

        'Die Namen der Maßnahmen
        ReDim Me.Measures(Individuum.mProblem.NumLocations - 1)

        'Informationen pro Location
        ReDim Me.Loc(Individuum.mProblem.NumLocations - 1)

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

        'Dn Wert
        Dest.CES_Dn = Me.CES_Dn

        'Objective-Werte
        Call Array.Copy(Me.Objectives, Dest.Objectives, Me.Objectives.Length)

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
