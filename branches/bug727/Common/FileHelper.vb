' Copyright (c) BlueM Dev Group
' Website: http://bluemodel.org
' 
' All rights reserved.
' 
' Released under the BSD-2-Clause License:
' 
' Redistribution and use in source and binary forms, with or without modification, 
' are permitted provided that the following conditions are met:
' 
' * Redistributions of source code must retain the above copyright notice, this list 
'   of conditions and the following disclaimer.
' * Redistributions in binary form must reproduce the above copyright notice, this list 
'   of conditions and the following disclaimer in the documentation and/or other materials 
'   provided with the distribution.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
' EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
' OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
' SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT 
' OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
' HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
' TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
' EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'--------------------------------------------------------------------------------------------
'
Imports System.IO

Public Module FileHelper

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
