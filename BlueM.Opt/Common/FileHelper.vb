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

Public Module FileHelper

    'Ändert rekursiv die Attribute von Dateien und Unterverzeichnissen von Read-Only zu Normal
    '*****************************************************************************************
    Public Sub purgeReadOnly(ByVal path As String)

        Dim mainDir As New IO.DirectoryInfo(path)
        Dim fInfo As IO.FileInfo() = mainDir.GetFiles("*.*")

        'now loop through all the files and change the file attributes to normal
        Dim file As IO.FileInfo

        For Each file In fInfo
            If (file.Attributes And IO.FileAttributes.ReadOnly) Then
                file.Attributes = IO.FileAttributes.Normal
            End If
        Next

        'do the same for the directories
        Dim dInfo As IO.DirectoryInfo() = mainDir.GetDirectories("*.*")
        Dim dir As IO.DirectoryInfo

        For Each dir In dInfo
            If (dir.Attributes And IO.FileAttributes.ReadOnly) Then
                dir.Attributes = IO.FileAttributes.Normal
            End If

            'Call method recursively
            Call purgeReadOnly(dir.FullName)
        Next

    End Sub

End Module
