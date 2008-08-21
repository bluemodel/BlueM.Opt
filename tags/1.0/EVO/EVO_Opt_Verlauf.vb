Partial Public Class EVO_Opt_Verlauf
    Inherits System.Windows.Forms.UserControl

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse EVO_Opt_Verlauf                                                ****
    '****                                                                       ****
    '**** Autoren: Christoph Hübner, Felix Fröhlich, Dirk Muschalla             ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '****                                                                       ****
    '**** November 2007                                                         ****
    '****                                                                       ****
    '**** Letzte Änderung: November 2007                                        ****
    '*******************************************************************************
    '*******************************************************************************

    Private NRunden As Short
    Private NPopul As Short
    Private NGen As Short
    Private NNachf As Short

    '********************************************************************
    'Schnittstelle
    '********************************************************************

    'Initialisieren
    '**************
    Public Sub Initialisieren(ByVal NRunden As Integer, ByVal NPopul As Integer, ByVal NGen As Integer, ByVal NNachf As Integer)

        Me.NRunden = NRunden
        Me.NPopul = NPopul
        Me.NGen = NGen
        Me.NNachf = NNachf

        LabelAnzRunden.Text = Me.NRunden
        LabelAnzPop.Text = Me.NPopul
        LabelAnzGen.Text = Me.NGen
        LabelAnzNachf.Text = Me.NNachf

        ProgressBarRunde.Minimum = 0
        ProgressBarRunde.Maximum = Me.NRunden
        ProgressBarRunde.Value = 0

        ProgressBarPop.Minimum = 0
        ProgressBarPop.Maximum = Me.NPopul
        ProgressBarPop.Value = 0

        ProgressBarGen.Minimum = 0
        ProgressBarGen.Maximum = Me.NGen
        ProgressBarGen.Value = 0

        ProgressBarNach.Minimum = 0
        ProgressBarNach.Maximum = Me.NNachf
        ProgressBarNach.Value = 0

    End Sub

    Public Sub Runden(ByVal NRunden As Short)
        Me.NRunden = NRunden
        ProgressBarRunde.Value = Me.NRunden
        LabelaktRunde.Text = Me.NRunden.ToString()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Public Sub Population(ByVal NPopul As Short)
        Me.NPopul = NPopul
        ProgressBarPop.Value = Me.NPopul
        LabelaktPop.Text = Me.NPopul.ToString()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Public Sub Generation(ByVal NGen As Short)
        Me.NGen = NGen
        ProgressBarGen.Value = Me.NGen
        LabelaktGen.Text = Me.NGen.ToString()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Public Sub Nachfolger(ByVal NNachf As Short)
        Me.NNachf = NNachf
        ProgressBarNach.Value = Me.NNachf
        LabelaktNachf.Text = Me.NNachf.ToString()
        System.Windows.Forms.Application.DoEvents()
    End Sub

End Class