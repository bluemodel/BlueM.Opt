Imports System.Xml.Serialization

Public Class Settings_DDS

    Public maxiter As Integer                  'Number of iterations
    Public r_val As Double                     'DDS perturbation parameter
    Public optStartparameter As Boolean

    'Standardwerte setzen
    '********************
    Public Sub setStandard()
        Me.maxiter = 1000
        Me.r_val = 0.2
        Me.optStartparameter = True
    End Sub

End Class
