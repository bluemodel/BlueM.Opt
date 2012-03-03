' Copyright (c) 2011, ihwb, TU Darmstadt
' All rights reserved.
' 
' Released under the BSD-2-Clause License:
' 
' Redistribution and use in source and binary forms, with or without modification, 
' are permitted provided that the following conditions are met:
' 
' * Redistributions of source code must retain the above copyright notice, this list 
'   of conditions and the following disclaimer.
' * Redistributions in binary form must reproduce the above copyright notice, this list 
'   of conditions and the following disclaimer in the documentation and/or other materials 
'   provided with the distribution.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
' EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
' OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
' SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT 
' OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
' HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
' TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
' EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'--------------------------------------------------------------------------------------------
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
        Private mObjBoundary  As Double
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
        
        Public Property ObjBoundary() As Double
            Get
                Return mObjBoundary
            End Get
            Set(ByVal value As double)
                mObjBoundary = value
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
