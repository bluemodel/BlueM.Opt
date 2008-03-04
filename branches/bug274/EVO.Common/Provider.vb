Imports System.Globalization

Public Module Provider

    Public ReadOnly Property FortranProvider() As NumberFormatInfo
        Get
            'Fortran Provider einrichten
            '---------------------------
            Dim provider As New NumberFormatInfo()

            provider.NumberDecimalSeparator = "."
            provider.NumberGroupSeparator = ""
            provider.NumberGroupSizes = New Integer() {3}

            Return provider
        End Get

    End Property

End Module
