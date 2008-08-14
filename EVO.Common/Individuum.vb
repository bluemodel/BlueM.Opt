Public MustInherit Class Individuum

    '*******************************************************************************
    '*******************************************************************************
    '**** Basisklasse Individuum für das Speichern eines Evaluierungssatzes     ****
    '****                                                                       ****
    '**** Autoren: Christoph Hübner, Felix Fröhlich                             ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

#Region "Eigenschaften"

    ''' <summary>
    ''' Individuumsklassen
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum Individuumsklassen As Integer
        Individuum_PES = 1
        Individuum_CES = 2
    End Enum

    'Shared Variablen der Klasse
    '***************************
    Protected Shared n_Para As Integer      'Anzahl Parameter

    'Eigenschaften
    '*************
    Protected mType As String               'Typ des Individuums
    Protected mID As Integer                'ID des Individuums

    Protected mZielwerte() As Double        'Array aller Zielfunktionswerte (inkl. sekundär)
    Protected mConstrain() As Double        'Werte der Randbedingungen (Wenn negativ dann ungültig)

    'Für ND Sorting -------------------------------------------------
    Protected mDominated As Boolean         'Kennzeichnung ob Dominiert
    Protected mFront As Integer             'Nummer der Pareto Front
    Protected mDistance As Double           'Für crowding distance

#End Region 'Eigenschaften

#Region "Properties"

    ''' <summary>
    ''' Eindeutige Nummer des Individuums
    ''' </summary>
    Public Property ID() As Integer
        Get
            Return Me.mID
        End Get
        Set(ByVal value As Integer)
            Me.mID = value
        End Set
    End Property

    ''' <summary>
    ''' Werte der Zielfunktionen
    ''' </summary>
    ''' <remarks>Zielfunktionen beinhalten die Penaltyfunktionen und die Eigenschaftsfunktionen</remarks>
    Public Property Zielwerte() As Double()
        Get
            Return Me.mZielwerte
        End Get
        Set(ByVal value As Double())
            Me.mZielwerte = value
        End Set
    End Property

    ''' <summary>
    ''' Werte der Penalty-Funktionen
    ''' </summary>
    ''' <remarks>Zielfunktionswerte nur von OptZielen</remarks>
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

    ''' <summary>
    ''' Werte der Constraintfunktionen
    ''' </summary>
    ''' <remarks>Negativer Constraintwert bedeutet Randbedingung ist verletzt</remarks>
    Public Property Constrain() As Double()
        Get
            Return Me.mConstrain
        End Get
        Set(ByVal value As Double())
            Me.mConstrain = value
        End Set
    End Property

    ''' <summary>
    ''' Zeigt an, ob das Individuum dominiert wird
    ''' </summary>
    Public Property Dominated() As Boolean
        Get
            Return Me.mDominated
        End Get
        Set(ByVal value As Boolean)
            Me.mDominated = value
        End Set
    End Property

    ''' <summary>
    ''' Nummer der Pareto-Front, zu dem das Individuum gehört
    ''' </summary>
    Public Property Front() As Integer
        Get
            Return Me.mFront
        End Get
        Set(ByVal value As Integer)
            Me.mFront = value
        End Set
    End Property

    ''' <summary>
    ''' Crowding Distance
    ''' </summary>
    Public Property Distance() As Double
        Get
            Return Me.mDistance
        End Get
        Set(ByVal value As Double)
            Me.mDistance = value
        End Set
    End Property

    ''' <summary>
    ''' Zeigt an, ob das Individuum gültig ist
    ''' </summary>
    ''' <remarks>wenn einer der Werte der Constraint-Funktionen negativ ist, ist das Individuum ungültig (Is_Feasible = False)</remarks>
    Public ReadOnly Property Is_Feasible() As Boolean
        Get
            For i As Integer = 0 To Me.Constrain.GetUpperBound(0)
                If (Me.Constrain(i) < 0) Then Return False
            Next
            Return True
        End Get
    End Property

#End Region 'Properties

#Region "Methoden"

    'Methoden
    '########

    ''' <summary>
    ''' Initialisiert die Basis-Individuumsklasse
    ''' </summary>
    ''' <remarks>Diese Methode bewirkt nichts</remarks>
    Public Shared Sub Initialise()

    End Sub

    ''' <summary>
    ''' Konstruktor
    ''' </summary>
    ''' <param name="type">Frei definierbarer String</param>
    ''' <param name="id">Eindeutige Nummer</param>
    Public Sub New(ByVal type As String, ByVal id As Integer)

        Dim i As Integer

        'Typ des Individuum
        Me.mType = type

        'Nummer des Individuum
        Me.mID = id

        'Zielfunktionswerte
        ReDim Me.Zielwerte(Common.Manager.AnzZiele - 1)
        For i = 0 To Common.Manager.AnzZiele - 1
            Me.Zielwerte(i) = Double.MaxValue           'mit maximalem Double-Wert initialisieren
        Next

        'Wert der Randbedingung(en)
        ReDim Me.Constrain(Common.Manager.AnzConstraints - 1)
        For i = 0 To Common.Manager.AnzConstraints - 1
            Me.Constrain(i) = Double.MinValue           'mit minimalem Double-Wert initialisieren
        Next

        'Kennzeichnung ob Dominiert
        Me.Dominated = False

        'Nummer der Pareto Front
        Me.Front = 0

        'Für crowding distance
        Me.Distance = 0

    End Sub

    ''' <summary>
    ''' Kopiert ein Individuum
    ''' </summary>
    ''' <returns>Individuum</returns>
    Public MustOverride Function Clone() As Individuum

    ''' <summary>
    ''' Erzeugt ein Array von neuen Individuen
    ''' </summary>
    ''' <param name="klasse">Individuumsklasse</param>
    ''' <param name="length">Länge des Arrays</param>
    ''' <param name="type">beliebiger String</param>
    ''' <returns>Array von Individuen</returns>
    ''' <remarks></remarks>
    Public Shared Function New_Indi_Array(ByVal klasse As Individuumsklassen, ByVal length As Integer, Optional ByVal type As String = "tmp") As Individuum()

        Dim i As Integer

        Dim inds(-1) As Individuum

        Select Case klasse
            Case Individuumsklassen.Individuum_PES
                inds = Array.CreateInstance(GetType(Individuum_PES), length)
                For i = 0 To length - 1
                    inds(i) = New Individuum_PES(type, 0)
                Next

            Case Individuumsklassen.Individuum_CES
                inds = Array.CreateInstance(GetType(Individuum_CES), length)
                For i = 0 To length - 1
                    inds(i) = New Individuum_CES(type, 0)
                Next

        End Select

        Return inds

    End Function

    ''' <summary>
    ''' Kopiert ein Array von Individuen
    ''' </summary>
    ''' <remarks>die spezifische Klasse der einzelnen Individuen wird beibehalten</remarks>
    ''' <param name="Source">zu kopierendes Array von Individuen</param>
    ''' <returns>Array von Individuen</returns>
    Public Shared Function Clone_Indi_Array(ByVal Source() As Individuum) As Individuum()

        Dim i As Integer
        Dim Dest() As Individuum

        ReDim Dest(Source.GetUpperBound(0))

        For i = 0 To Source.GetUpperBound(0)
            Dest(i) = Source(i).Clone()
        Next

        Return Dest

    End Function

    ''' <summary>
    ''' Wandelt die Penalty-Werte eines Arrays von Individuen in ein zweidimensionales Array von Double-Werten um
    ''' </summary>
    ''' <param name="Indi_Array">zu verarbeitendes Array von Individuen</param>
    ''' <returns>zweidimensionales Array von Penalty-Werten: 1. Dimension: Individuum, 2. Dimension Penalty-Werte</returns>
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

    ''' <summary>
    ''' Erzeugt ein neues (leeres) Individuum von der gleichen Klasse
    ''' </summary>
    ''' <returns>Das neue Individuum</returns>
    Public MustOverride Function Create(Optional ByVal type As String = "tmp", Optional ByVal id As Integer = 0) As Individuum

#End Region 'Methoden

End Class
