Public MustInherit Class Individuum

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse Individuum f�r das Speichern eines Evaluierungssatzes          ****
    '****                                                                       ****
    '**** Autoren: Christoph H�bner, Felix Fr�hlich                             ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

#Region "Eigenschaften"

    'Variablen der Klasse
    '********************
    Protected Shared Individ_Type As Integer
    Protected Shared n_Para As Integer

    'Strukturen der Klasse
    '*********************

    Public Type As String                  '01 Typ des Individuum
    Public ID As Integer                   '02 Nummer des Individuum

    Public Zielwerte() As Double           '04 Array aller Zielfunktionswerte (inkl. sekund�r)
    Public Constrain() As Double           '05 Werte der Randbedingungen (Wenn negativ dann ung�ltig)

    'F�r ND Sorting -------------------------------------------------
    Public dominated As Boolean            '07 Kennzeichnung ob Dominiert
    Public Front As Integer                '08 Nummer der Pareto Front
    Public Distance As Double              '09 F�r crowding distance

    'Gibt die Array mit den Penalties zur�ck
    '(d.h. Zielfunktionswerte nur von OptZielen)
    '*******************************************
    Public ReadOnly Property Penalties() As Double()
        Get
            Dim i, j As Integer
            Dim Array() As Double

            ReDim Array(Common.Manager.AnzPenalty - 1)

            j = 0
            For i = 0 To Common.Manager.AnzZiele - 1
                'Nur die Zielfunktionswerte von OptZielen zur�ckgeben!
                If (Common.Manager.List_Ziele(i).isOpt) Then
                    Array(j) = Me.Zielwerte(i)
                    j += 1
                End If
            Next
            Return Array
        End Get
    End Property

    'Gibt zur�ck ob Individuum feasible ist
    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    Public ReadOnly Property Is_Feasible() As Boolean
        Get
            For i As Integer = 0 To Me.Constrain.GetUpperBound(0)
                If (Me.Constrain(i) < 0) Then Return False
            Next
            Return True
        End Get
    End Property

#End Region 'Eigenschaften

#Region "Methoden"

    'Methoden
    '########

    'Konstruktor f�r ein Individuum
    '******************************
    Public Sub New(ByVal _Type As String, ByVal _ID As Integer)

        Dim i, j As Integer

        '01 Typ des Individuum
        Me.Type = _Type

        '02 Nummer des Individuum
        Me.ID = _ID

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

        '07 Kennzeichnung ob Dominiert
        Me.dominated = False

        '08 Nummer der Pareto Front
        Me.Front = 0

        '09 F�r crowding distance
        Me.Distance = 0

    End Sub

    'Kopiert ein Individuum
    '**********************
    Public MustOverride Function Clone() As Individuum

    'Kopiert ein Array von Individuuen
    '*********************************
    Public Shared Sub Clone_Indi_Array(ByVal Source() As Individuum, ByRef Dest() As Individuum)
        Dim i As Integer

        For i = 0 To Source.GetUpperBound(0)
            Dest(i) = Source(i).Clone()
        Next
    End Sub

    'ES_GET_SEKUND�RE_POPULATIONEN - Sekund�re Population speichert immer die angegebene
    'Anzahl von Bestwerten und kann den Bestwertspeicher alle x Generationen �berschreiben
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

    Public MustOverride Function Create(Optional ByVal type As String = "tmp", Optional ByVal id As Integer = 0) As Individuum

#End Region 'Methoden

End Class
