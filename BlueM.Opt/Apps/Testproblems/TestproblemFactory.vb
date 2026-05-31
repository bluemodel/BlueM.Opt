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
''' Factory module for creating Testproblem instances
''' </summary>
Public Module TestproblemFactory

    ''' <summary>
    ''' Defined test problems
    ''' </summary>
    Public Enum Testproblems
        Ackley
        Beale
        Box
        CONSTR
        Deb1
        DependentParameters
        FloodMitigation
        Schwefel24
        SinusFunktion
        ZitzlerDebT1
        ZitzlerDebT2
        ZitzlerDebT3
        ZitzlerDebT4
    End Enum

    ''' <summary>
    ''' Creates a test problem instance of the given type
    ''' </summary>
    ''' <param name="testproblemtype">testproblem type</param>
    ''' <returns></returns>
    Public Function CreateTestProblem(testproblemtype As Testproblems) As Testproblem

        Dim testproblem As Testproblem
        Select Case testproblemtype
            Case Testproblems.Ackley
                testproblem = New Ackley()
            Case Testproblems.Beale
                testproblem = New Beale()
            Case Testproblems.Box
                testproblem = New Box()
            Case Testproblems.CONSTR
                testproblem = New Constraints()
            Case Testproblems.Deb1
                testproblem = New Deb1()
            Case Testproblems.DependentParameters
                testproblem = New DependentParameters()
            Case Testproblems.FloodMitigation
                testproblem = New FloodMitigation()
            Case Testproblems.Schwefel24
                testproblem = New Schwefel24()
            Case Testproblems.SinusFunktion
                testproblem = New Sinus()
            Case Testproblems.ZitzlerDebT1
                testproblem = New ZitzlerDebT1()
            Case Testproblems.ZitzlerDebT2
                testproblem = New ZitzlerDebT2()
            Case Testproblems.ZitzlerDebT3
                testproblem = New ZitzlerDebT3()
            Case Testproblems.ZitzlerDebT4
                testproblem = New ZitzlerDebT4()
            Case Else
                Throw New Exception("Unknown test problem: " & testproblemtype)
        End Select

        Return testproblem

    End Function

End Module
