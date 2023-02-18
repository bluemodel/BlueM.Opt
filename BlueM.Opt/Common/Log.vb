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
''' <summary>
''' The log
''' </summary>
Public Module Log

    ''' <summary>
    ''' Is raised when a new message is added
    ''' </summary>
    ''' <param name="msg"></param>
    Public Event LogMessageAdded(msg As String)

    Private _Log As String
    Private _File As String

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
    Public Sub AddMessage(msg As String)

        'format message
        msg = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") & ": " & msg & BlueM.Opt.Common.Constants.eol

        'store
        _Log &= msg

        'append to file
        If Not IsNothing(_File) Then
            IO.File.AppendAllText(_File, msg)
        End If

        'raise event
        RaiseEvent LogMessageAdded(msg)
    End Sub

End Module
