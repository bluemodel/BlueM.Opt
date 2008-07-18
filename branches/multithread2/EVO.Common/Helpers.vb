Imports System.IO

Public Module Helpers

    'Ändert rekursiv die Attribute von Dateien und Unterverzeichnissen von Read-Only zu Normal
    '*****************************************************************************************
    Public Sub purgeReadOnly(ByVal path As String)

        Dim mainDir As New DirectoryInfo(path)
        Dim fInfo As IO.FileInfo() = mainDir.GetFiles("*.*")

        'now loop through all the files and change the file attributes to normal
        Dim file As IO.FileInfo

        For Each file In fInfo
            If (file.Attributes And FileAttributes.ReadOnly) Then
                file.Attributes = FileAttributes.Normal
            End If
        Next

        'do the same for the directories
        Dim dInfo As DirectoryInfo() = mainDir.GetDirectories("*.*")
        Dim dir As DirectoryInfo

        For Each dir In dInfo
            If (dir.Attributes And FileAttributes.ReadOnly) Then
                dir.Attributes = FileAttributes.Normal
            End If

            'Call method recursively
            Call purgeReadOnly(dir.FullName)

        Next

    End Sub

End Module
