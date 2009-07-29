Public Class ObjectiveFunction_Aggregate
    Inherits ObjectiveFunction

    ''' <summary>
    ''' Returns the type of the ObjectiveFunction
    ''' </summary>
    Public Overrides ReadOnly Property GetObjType() As ObjectiveType
        Get
            Return ObjectiveType.Aggregate
        End Get
    End Property

    Public Overrides Function calculateObjective(ByVal SimErgebnis As SimErgebnis_Structure) As Double

        'Aggregierte Ziele müssen von ausserhalb berechnet werden
        Throw New Exception("don't go here!")

    End Function

End Class
