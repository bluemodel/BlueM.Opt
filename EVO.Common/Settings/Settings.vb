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

    'Klasse für generelle Settings
    '-----------------------------
    Public Class Settings_General
        Private mUseMultithreading As Boolean
        Private mMultithreadingAllowed As Boolean
        Private mDrawOnlyCurrentGeneration As Boolean
        Private mBatchMode As Boolean
        Private mMPCMode As Boolean
        Public Property BatchMode() As Boolean
            Get
                Return mBatchMode
            End Get
            Set(ByVal value As Boolean)
                mBatchMode = value
            End Set
        End Property

        Public Property MPCMode() As Boolean
            Get
                Return mMPCMode
            End Get
            Set(ByVal value As Boolean)
                mMPCMode = value
            End Set
        End Property
        ''' <summary>
        ''' Multithreading erlauben/verbieten und gleichzeitig ein-/ausschalten
        ''' </summary>
        Public Property MultithreadingAllowed() As Boolean
            Get
                Return Me.mMultithreadingAllowed
            End Get
            Set(ByVal allow As Boolean)
                Me.mMultithreadingAllowed = allow
                If (allow) Then
                    Me.mMultithreadingAllowed = True
                    Me.UseMultithreading = True
                Else
                    Me.mMultithreadingAllowed = False
                    Me.UseMultithreading = False
                End If
            End Set
        End Property

        ''' <summary>
        ''' Multithreading verwenden
        ''' </summary>
        Public Property UseMultithreading() As Boolean
            Get
                Return mUseMultithreading
            End Get
            Set(ByVal value As Boolean)
                mUseMultithreading = value
            End Set
        End Property

        ''' <summary>
        ''' Nur die aktuelle Generation im Hauptdiagramm anzeigen
        ''' </summary>
        Public Property DrawOnlyCurrentGeneration() As Boolean
            Get
                Return mDrawOnlyCurrentGeneration
            End Get
            Set(ByVal value As Boolean)
                mDrawOnlyCurrentGeneration = value
            End Set
        End Property

        Public Sub setStandard()
            Me.MultithreadingAllowed = True
            Me.UseMultithreading = True
            Me.DrawOnlyCurrentGeneration = False
            Me.BatchMode = False
        End Sub
    End Class

    Public General As Settings_General
    Public PES As Settings_PES
    Public CES As Settings_CES
    Public HookeJeeves As Settings_HookeJeeves
    Public MetaEvo As Settings_MetaEvo
    Public DDS As Settings_DDS
    Public SensiPlot As Settings_Sensiplot
    Public TSP As Settings_TSP

    Public Sub New()
        Me.General = New Settings_General()
        Me.General.setStandard()
    End Sub

End Class
