Imports System.Xml.Serialization

Public Class Settings_TSP

    Public n_Cities As Integer
    Public n_Gen As Integer
    Public n_Parents As Integer
    Public n_Children As Integer

    Public Problem As EnProblem
    Public ReprodOperator As EnReprodOperator
    Public MutOperator As EnMutOperator
    Public Strategy As EVO_STRATEGIE

    Public Sub setStandard()

        n_Cities = 70
        n_Gen = 20000
        n_Parents = 5   'mindestens 3 Eltern!
        n_Children = 40

        Problem = EnProblem.circle
        ReprodOperator = EnReprodOperator.Order_Crossover_OX
        MutOperator = EnMutOperator.Translocation_3_Opt
        Strategy = EVO_STRATEGIE.Plus_Strategie
    End Sub

End Class
