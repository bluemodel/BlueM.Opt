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
Imports System.IO
Imports System.Net.Sockets
''' <summary>
''' The log
''' </summary>
Public Module Log

    ''' <summary>
    ''' Is raised when a new message is added
    ''' </summary>
    ''' <param name="msg"></param>
    Public Event LogMessageAdded(msg As String)

    Private locker As New Object()
    Private _Log As String
    Private _File As String

    ''' <summary>
    ''' Log levels
    ''' </summary>
    Public Enum levels As Short
        debug
        info
        warning
        [error]
    End Enum

    ''' <summary>
    ''' Sets the path to the log file to write to
    ''' </summary>
    ''' <param name="path"></param>
    Public Sub SetLogFile(path As String)
        _File = path
        'write any already existing messages to file
        If Not IsNothing(_Log) Then
            IO.File.WriteAllText(_File, _Log)
        End If
    End Sub

    ''' <summary>
    ''' Clears the log and resets any file association
    ''' </summary>
    Public Sub Reset()
        _Log = Nothing
        _File = Nothing
    End Sub

    ''' <summary>
    ''' Adds a new message to the log
    ''' </summary>
    ''' <param name="msg"></param>
    Public Sub AddMessage(level As levels, msg As String)

        'format message
        msg = $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} {level.ToString.ToUpper()}: {msg}{eol}"

        'store
        _Log &= msg

        'append to file
        If Not IsNothing(_File) Then
            Try
                'use a lock to ensure only one thread writes to the file at a time
                SyncLock locker
                    Using writer As New StreamWriter(_File, True)
                        writer.WriteLine(msg)
                    End Using
                End SyncLock
            Catch ex As Exception
                Console.WriteLine($"Error writing to file: {ex.Message}")
            End Try
        End If

        'raise event
        RaiseEvent LogMessageAdded(msg)
    End Sub

End Module
