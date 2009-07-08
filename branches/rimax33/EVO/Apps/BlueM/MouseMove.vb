Option Explicit On
Imports System.IO

Public Class MouseMove


    Public Declare Sub mouse_event Lib "user32" (ByVal dwFlags As Int32, ByVal dx As Int32, ByVal dy As Int32, ByVal cButtons As Int32, ByVal dwExtraInfo As Int32)

    Public Declare Function SetCursorPos Lib "user32" (ByVal X As Long, ByVal Y As Long) As Long

    Public Declare Function GetCursorPos Lib "user32" (ByVal lpPoint As PointAPI) As Long

    Public Const MOUSEEVENTF_LEFTDOWN As Integer = &H2
    Public Const MOUSEEVENTF_LEFTUP As Integer = &H4
    Public Const MOUSEEVENTF_MIDDLEDOWN As Integer = &H20
    Public Const MOUSEEVENTF_MIDDLEUP As Integer = &H40
    Public Const MOUSEEVENTF_RIGHTDOWN As Integer = &H8
    Public Const MOUSEEVENTF_RIGHTUP As Integer = &H10
    Public Const MOUSEEVENTF_MOVE As Integer = &H1

    Public Structure PointAPI
        Public x As Int32
        Public y As Int32
    End Structure

    Public Function Maus_X() As Long
        Dim n As POINTAPI
        GetCursorPos(n)
        Maus_X = n.x
    End Function

    'Mausposition Y ermitteln
    Public Function Maus_Y() As Long
        Dim n As POINTAPI
        GetCursorPos(n)
        Maus_Y = n.y
    End Function

    Public Sub LeftClick()
        LeftDown()
        LeftUp()
    End Sub

    Public Sub LeftDown()
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0)
    End Sub

    Public Sub LeftUp()
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0)
    End Sub

    Public Sub MiddleClick()
        MiddleDown()
        MiddleUp()
    End Sub

    Public Sub MiddleDown()
        mouse_event(MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, 0)
    End Sub

    Public Sub MiddleUp()
        mouse_event(MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0)
    End Sub

    Public Sub MoveMouse(ByVal xMove As Long, ByVal yMove As Long)
        mouse_event(MOUSEEVENTF_MOVE, xMove, yMove, 0, 0)
    End Sub

    Public Sub RightClick()
        RightDown()
        RightUp()
    End Sub

    Public Sub RightDown()
        mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0)
    End Sub

    Public Sub RightUp()
        mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0)
    End Sub
End Class
