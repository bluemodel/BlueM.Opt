Public Class EcoFlood

    Public Function EcoFlood_QN_Wert(ByVal ZielNr As Integer) As Double
        Dim i As Integer
        EcoFlood_QN_Wert = 0
        For i = 0 To BM_Form.OptParameterListe.GetUpperBound(0)
            EcoFlood_QN_Wert = EcoFlood_QN_Wert + BM_Form.OptParameterListe(ZielNr).Wert * 2
        Next
    End Function

End Class
