Imports System.Globalization

Public Class Provider

    Public Shared Function FortranProvider() As NumberFormatInfo

        'Fortran Provider einrichten
        '-------------------
        Dim provider As NumberFormatInfo

        provider = New NumberFormatInfo()
        provider.NumberDecimalSeparator = "."
        provider.NumberGroupSeparator = ""
        provider.NumberGroupSizes = New Integer() {3}

        Return provider

    End Function

End Class
