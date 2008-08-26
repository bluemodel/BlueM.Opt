Public Module Manager

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse Manager                                                        ****
    '****                                                                       ****
    '**** Autoren: Christoph Hübner, Felix Fröhlich                             ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '****                                                                       ****
    '**** November 2007                                                         ****
    '****                                                                       ****
    '**** Letzte Änderung: November 2007                                        ****
    '*******************************************************************************
    '*******************************************************************************

    'Eigenschaften
    '#############

    Public Method As String                             'Optimierungsmethode

    Public List_Featurefunctions() As Featurefunction           'Liste der Feature Functions
    Public List_Constraintfunctions() As Constraintfunction     'Liste der Constraint Functions

    'Properties
    '##########

    'Gibt die Gesamtanzahl der Ziele zurück
    '**************************************
    Public ReadOnly Property NumFeatures() As Integer
        Get
            Return Manager.List_Featurefunctions.Length
        End Get
    End Property

    'Gibt die Anzahl der Penalty Functions zurück
    '********************************************
    Public ReadOnly Property NumPenalties() As Integer
        Get
            Dim n As Integer

            n = 0
            For Each feature As Featurefunction In Manager.List_Featurefunctions
                If (feature.isPenalty) Then n += 1
            Next

            Return n
        End Get
    End Property

    'Gibt die OptimierungsZiele zurück
    '*********************************
    Public ReadOnly Property List_Penaltyfunctions() As Featurefunction()
        Get
            Dim i As Integer
            Dim array() As Featurefunction

            ReDim array(Manager.NumPenalties - 1)

            i = 0
            For Each feature As Featurefunction In Manager.List_Featurefunctions
                If (feature.isPenalty) Then
                    array(i) = feature
                    i += 1
                End If
            Next

            Return array
        End Get
    End Property

    'Bibt die Anzahl Constraints zurück
    '**********************************
    Public ReadOnly Property NumConstraints() As Integer
        Get
            Return Manager.List_Constraintfunctions.Length
        End Get
    End Property

End Module
