Public Module Locations

    'Kombinatorik
    '------------

    Public Structure Struct_Massnahme
        Public Name As String
        Public Schaltung(,) As String
        Public KostenTyp As Integer
        Public Bauwerke() As String
        Public TestModus As Integer
    End Structure

    Public Structure Struct_Lokation
        Public Name As String
        Public List_Massnahmen() As Struct_Massnahme
    End Structure

End Module
