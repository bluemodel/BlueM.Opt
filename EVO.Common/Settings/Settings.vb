'*******************************************************************************
'*******************************************************************************
'**** Klasse Settings                                                       ****
'**** zum Speichern aller EVO-Einstellungen aus dem Form                    ****
'****                                                                       ****
'**** Autoren: Christoph Hübner, Felix Froehlich, Dirk Muschalla            ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'****                                                                       ****
'**** November 2007                                                         ****
'****                                                                       ****
'**** Letzte Änderung: Juli 2009                                            ****
'*******************************************************************************
'*******************************************************************************

Imports System.Xml.Serialization

Public Class Settings

    'Struct für generelle Settings
    '-----------------------------
    Public Structure General_Settings
        Public useMultithreading As Boolean
        Public drawOnlyCurrentGeneration As Boolean
        Public Sub setStandard()
            Me.useMultithreading = True
            Me.drawOnlyCurrentGeneration = False
        End Sub
    End Structure

    Public General As General_Settings
    Public PES As Settings_PES
    Public CES As Settings_CES
    Public HookeJeeves As Settings_HookeJeeves
    Public MetaEvo As Settings_MetaEvo
    Public DDS As Settings_DDS
    Public SensiPlot As Settings_Sensiplot
    Public TSP As Settings_TSP

    ''' <summary>
    ''' Setzt alle Settings zurück (löscht sie)
    ''' </summary>
    Public Sub Reset()
        Me.PES = Nothing
        Me.CES = Nothing
        Me.HookeJeeves = Nothing
        Me.MetaEvo = Nothing
        Me.DDS = Nothing
        Me.SensiPlot = Nothing
        Me.TSP = Nothing
    End Sub

End Class
