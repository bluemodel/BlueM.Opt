Public Class OptResult

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse OptResult                                                      ****
    '****                                                                       ****
    '**** Speichert die Ergebnisse eines Optimierungslaufs                      ****
    '****                                                                       ****
    '**** Autoren: Felix Froehlich                                              ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

    'Optimierungsbedingungen
    Public List_OptZiele() As Sim.Struct_OptZiel
    Public List_OptParameter() As Sim.Struct_OptParameter
    Public List_Constraints() As Sim.Struct_Constraint

    'Structure einer Lösung
    Public Structure Struct_Solution
        Public QWerte() As Double
        Public OptPara() As Double
        Public Constraints() As Double
        Public ReadOnly Property isValid() As Boolean
            Get
                For i As Integer = 0 To Me.Constraints.GetUpperBound(0)
                    If (Me.Constraints(i) < 0) Then Return False
                Next
                Return True
            End Get
        End Property
    End Structure

    'Array von Lösungen
    Public Solutions() As Struct_Solution

End Class
