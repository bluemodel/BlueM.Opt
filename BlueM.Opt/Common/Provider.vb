'BlueM.Opt
'Copyright (C) BlueM Dev Group
'Website: <https://www.bluemodel.org>
'
'This program is free software: you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.
'
'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
'GNU General Public License for more details.
'
'You should have received a copy of the GNU General Public License
'along with this program. If not, see <https://www.gnu.org/licenses/>.
'
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
