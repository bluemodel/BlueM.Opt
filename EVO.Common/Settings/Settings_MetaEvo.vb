Imports System.Xml.Serialization

Public Class Settings_MetaEvo

    Public Role As String                       'Was stellt dieser PC da: Single PC, Network Server, Network Client
    Public OpMode As String                     'Optimierungen: Local Optimizer, Global Optimizer, Both
    Public AlgoMode As String                   'Zustand des Algomanagers

    'Einstellungen für einen PC der das Problem berechnet (Global)
    Public PopulationSize As Integer            'Populationsgrösse
    Public NumberGenerations As Integer         'Anzahl der Generationen die berechnet werden
    Public CurrentGeneration As Integer         'Aktuelle Generationenzahl
    Public NumberResults As Integer             'Anzahl der Lösungen nach der Globalen Optimierung und am Ende

    'Einstellungen für einen PC der das Problem berechnet (Global)
    Public HJStepsize As Integer                '1/x minimale Schrittweite der Optparas bei Hook&Jeeves 

    'Datenbank-Connection
    Public MySQL_Host As String
    Public MySQL_Database As String
    Public MySQL_User As String
    Public MySQL_Password As String

    'Versteckte Optionen
    Public ChildrenPerParent As Integer

    'Standardwerte setzen
    '********************
    Public Sub setStandard()

        Me.Role = "Single PC"
        Me.OpMode = "Both"

        Me.PopulationSize = 15
        Me.NumberGenerations = 50
        Me.CurrentGeneration = 0
        Me.NumberResults = 10

        Me.HJStepsize = 50

        Me.MySQL_Host = "localhost"
        Me.MySQL_Database = "metaevo_db"
        Me.MySQL_User = "remoteuser"
        Me.MySQL_Password = ""

        Me.ChildrenPerParent = 3
    End Sub

End Class
