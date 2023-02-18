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

    ''' <summary>
    ''' Adds a new message to the log
    ''' </summary>
    ''' <param name="msg"></param>
    Public Sub AddMessage(msg As String)
        msg = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") & ": " & msg & BlueM.Opt.Common.Constants.eol
        _Log &= msg
        'IO.File.AppendAllText("BlueM.Opt.log", msg)
        RaiseEvent LogMessageAdded(msg)
    End Sub

End Module
