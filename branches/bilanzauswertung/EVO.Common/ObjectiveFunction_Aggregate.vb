Public Class ObjectiveFunction_Aggregate
    Inherits ObjectiveFunction

    Public Overrides Function calculateObjective(ByVal SimErgebnis As SimErgebnis_Structure) As Double

        'Aggregierte Ziele müssen von ausserhalb berechnet werden
        Throw New Exception("don't go here!")

    End Function

End Class
