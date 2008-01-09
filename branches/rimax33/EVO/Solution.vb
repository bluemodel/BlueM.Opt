Public Class Solution

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse Solution                                                       ****
    '****                                                                       ****
    '**** speichert die für EVO wichtigen Ergebniswerte einer Simulation        ****
    '****                                                                       ****
    '**** Autoren: Felix Froehlich                                              ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

    Public ID As Integer
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

    'Eine Lösung kopieren
    '********************
    Public Function copy() As Solution

        Dim sol As New Solution

        sol.ID = Me.ID
        sol.QWerte = Me.QWerte
        sol.OptPara = Me.OptPara
        sol.Constraints = Me.Constraints

        Return sol

    End Function

End Class
