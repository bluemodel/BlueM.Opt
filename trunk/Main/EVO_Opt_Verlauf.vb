' Copyright (c) BlueM Dev Group
' Website: http://bluemodel.org
' 
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
'**** Klasse EVO_Opt_Verlauf                                                ****
'****                                                                       ****
'**** Autoren: Christoph Hübner, Felix Fröhlich, Dirk Muschalla             ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'*******************************************************************************
'*******************************************************************************

''' <summary>
''' Ein Benutzersteuerelement, dass den in einem Progress-Objekt 
''' abgebildeten Optimierungsverlauf in GUI-Form wiedergibt
''' </summary>
Partial Public Class EVO_Opt_Verlauf
    Inherits System.Windows.Forms.UserControl

    Private WithEvents mProgress As BlueM.Opt.Common.Progress

    ''' <summary>
    ''' EVO_Opt_Verlauf initialisieren
    ''' </summary>
    ''' <param name="progress">Übergabe des Progress-Objekts</param>
    ''' <remarks>Braucht nur ein einziges Mal aufgerufen zu werden</remarks>
    Public Sub Initialisieren(ByRef progress As BlueM.Opt.Common.Progress)

        'Progress-Objekt speichern
        Me.mProgress = progress

        'Zurücksetzen
        Call Me.Reset()

    End Sub

    Private Sub Reset() Handles mProgress.Initialized

        'Anzeige initialisieren
        LabelAnzRunden.Text = Me.mProgress.NRunden
        LabelaktRunde.Text = Me.mProgress.iRunde

        ProgressBarRunde.Minimum = 0
        ProgressBarRunde.Maximum = Me.mProgress.NRunden
        ProgressBarRunde.Value = Me.mProgress.iRunde

        LabelAnzPop.Text = Me.mProgress.NPopul
        LabelaktPop.Text = Me.mProgress.iPopul

        ProgressBarPop.Minimum = 0
        ProgressBarPop.Maximum = Me.mProgress.NPopul
        ProgressBarPop.Value = Me.mProgress.iPopul

        LabelAnzGen.Text = Me.mProgress.NGen
        LabelaktGen.Text = Me.mProgress.iGen

        ProgressBarGen.Minimum = 0
        ProgressBarGen.Maximum = Me.mProgress.NGen
        ProgressBarGen.Value = Me.mProgress.iGen

        LabelAnzNachf.Text = Me.mProgress.NNachf
        LabelaktNachf.Text = Me.mProgress.iNachf

        ProgressBarNach.Minimum = 0
        ProgressBarNach.Maximum = Me.mProgress.NNachf
        ProgressBarNach.Value = Me.mProgress.iNachf

    End Sub

    Private Sub ShowiRunde() Handles mProgress.iRundeChanged

        ProgressBarRunde.Value = Me.mProgress.iRunde
        LabelaktRunde.Text = Me.mProgress.iRunde.ToString()
        System.Windows.Forms.Application.DoEvents()

    End Sub

    Private Sub ShowIPopulation() Handles mProgress.iPopulChanged

        ProgressBarPop.Value = Me.mProgress.iPopul
        LabelaktPop.Text = Me.mProgress.iPopul.ToString()
        System.Windows.Forms.Application.DoEvents()

    End Sub

    Private Sub ShowIGeneration() Handles mProgress.iGenChanged

        ProgressBarGen.Value = Me.mProgress.iGen
        LabelaktGen.Text = Me.mProgress.iGen.ToString()
        System.Windows.Forms.Application.DoEvents()

    End Sub

    Private Sub ShowINachfahre() Handles mProgress.iNachfChanged

        ProgressBarNach.Value = Me.mProgress.iNachf
        LabelaktNachf.Text = Me.mProgress.iNachf.ToString()
        System.Windows.Forms.Application.DoEvents()

    End Sub

End Class