Public Class ResultStore

    'Structure für Sekundäre Population
    Public Structure Struct_SekPop
        Public iGen As Integer                      'Generationsnummer
        Public SolutionIDs() As Integer             'Array von Solution-IDs
    End Structure

    'Array von Lösungen
    Public Solutions() As Common.Individuum

    'Array von Sekundären Populationen
    Public SekPops() As Struct_SekPop

    'Konstruktor
    '***********
    Public Sub New()

        ReDim Me.Solutions(-1)
        ReDim Me.SekPops(-1)

    End Sub

    'Array von Lösungen anhand von IDs holen
    '***************************************
    Public Function getSolutions(ByVal IDs() As Integer) As Common.Individuum()

        Dim i As Integer
        Dim tmpsolutions() As Common.Individuum

        ReDim tmpsolutions(IDs.GetUpperBound(0))

        For i = 0 To tmpsolutions.GetUpperBound(0)
            tmpsolutions(i) = Me.getSolution(IDs(i))
        Next

        Return tmpsolutions

    End Function

    'Eine Lösung identifizieren
    '**************************
    Public Function getSolution(ByVal ID As Integer) As Common.Individuum

        Dim i As Integer

        For i = 0 To Me.Solutions.GetUpperBound(0)
            If (Me.Solutions(i).ID = ID) Then
                Return Me.Solutions(i)
            End If
        Next

        Throw New Exception("Konnte Lösung nicht identifizieren!")

    End Function

    'Sekundäre Population zu OptResult hinzufügen
    '********************************************
    Public Sub addSekPop(ByVal _sekpop As Struct_SekPop)

        'Array von Sekundären Populationen um eins erweitern
        ReDim Preserve SekPops(SekPops.GetUpperBound(0) + 1)
        'SekPop hinzufügen
        SekPops(SekPops.GetUpperBound(0)) = _sekpop

    End Sub

    'Sekundäre Population holen
    '**************************
    Public Function getSekPop(Optional ByVal _igen As Integer = -1) As Common.Individuum()

        Dim tmpsekpopsolutions() As Common.Individuum

        'Wenn keine Generation angegeben, dann letzte SekPop ausgeben
        If (_igen = -1) Then
            For Each tmpsekpop As Struct_SekPop In Me.SekPops
                If (tmpsekpop.iGen > _igen) Then _igen = tmpsekpop.iGen
            Next
        End If

        ReDim tmpsekpopsolutions(-1)

        'Alle Sekundären Populationen durchlaufen
        For Each tmpsekpop As Struct_SekPop In Me.SekPops
            If (tmpsekpop.iGen = _igen) Then
                'SekPop gefunden, Lösungen holen
                tmpsekpopsolutions = Me.getSolutions(tmpsekpop.SolutionIDs)
            End If
        Next

        Return tmpsekpopsolutions

    End Function

    'Gibt die Penalty-Werte einer Sekundären Population zurück
    '*********************************************************
    Public Function getSekPopValues(Optional ByVal igen As Integer = -1) As Double(,)

        Dim inds() As Common.Individuum
        Dim values(,) As Double
        Dim i, j As Integer

        'Wenn keine Generation angegeben, dann letzte SekPop ausgeben
        If (igen = -1) Then
            'Letzte Generation bestimmen
            For Each tmpsekpop As Struct_SekPop In Me.SekPops
                If (tmpsekpop.iGen > igen) Then igen = tmpsekpop.iGen
            Next
        End If

        'Wenn es keine Sekundäre Population in der DB gibt, abbrechen
        If (igen = -1) Then
            ReDim values(-1, -1)
            Return values
        End If

        inds = Me.getSekPop(igen)

        ReDim values(inds.GetUpperBound(0), Common.Manager.AnzPenalty - 1)

        For i = 0 To inds.GetUpperBound(0)
            For j = 0 To Common.Manager.AnzPenalty - 1
                values(i, j) = inds(i).Penalties(j)
            Next
        Next

        Return values

    End Function

End Class
