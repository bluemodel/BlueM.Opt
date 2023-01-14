'BlueM.Opt
'Copyright (C) BlueM Dev Group
'Website: <https://www.bluemodel.org>
'
'This program is free software: you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.
'
'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
'GNU General Public License for more details.
'
'You should have received a copy of the GNU General Public License
'along with this program. If not, see <https://www.gnu.org/licenses/>.
'
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
'*******************************************************************************
'*******************************************************************************

Imports System.Xml.Serialization

Public Class Settings

    'Klasse für generelle Settings
    '-----------------------------
    Public Class Settings_General
        Private mUseMultithreading As Boolean
        Private mNThreads As Integer
        Private mMultithreadingAllowed As Boolean
        Private mDrawOnlyCurrentGeneration As Boolean

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
        ''' Number of threads to use for multithreading
        ''' </summary>
        Public Property NThreads() As Integer
            Get
                Return mNThreads
            End Get
            Set(ByVal value As Integer)
                mNThreads = value
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
            Me.NThreads = Environment.ProcessorCount
            Me.DrawOnlyCurrentGeneration = False
        End Sub
    End Class

    Public General As Settings_General
    Public PES As Settings_PES
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
