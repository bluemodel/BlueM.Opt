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