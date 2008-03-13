Public Module Manager

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse Manager                                                        ****
    '****                                                                       ****
    '**** Autoren: Christoph H�bner, Felix Fr�hlich                             ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '****                                                                       ****
    '**** November 2007                                                         ****
    '****                                                                       ****
    '**** Letzte �nderung: November 2007                                        ****
    '*******************************************************************************
    '*******************************************************************************

    'Eigenschaften
    '#############

    Public List_Ziele() As Ziel                 'Liste der Zielfunktionen

    'Properties
    '##########

    'Gibt die Gesamtanzahl der Ziele zur�ck
    '**************************************
    Public ReadOnly Property AnzZiele() As Integer
        Get
            Return Manager.List_Ziele.Length
        End Get
    End Property

    'Gibt die Anzahl der OptimierungsZiele zur�ck
    '********************************************
    Public ReadOnly Property AnzPenalty() As Integer
        Get
            Dim n As Integer

            n = 0
            For Each ziel As Ziel In Manager.List_Ziele
                If (ziel.isOpt) Then n += 1
            Next

            Return n
        End Get
    End Property

    'Gibt die OptimierungsZiele zur�ck
    '*********************************
    Public ReadOnly Property List_OptZiele() As Ziel()
        Get
            Dim i As Integer
            Dim array() As Ziel

            ReDim array(Manager.AnzPenalty - 1)

            i = 0
            For Each ziel As Ziel In Manager.List_Ziele
                If (ziel.isOpt) Then
                    array(i) = ziel
                    i += 1
                End If
            Next

            Return array
        End Get
    End Property

End Module
