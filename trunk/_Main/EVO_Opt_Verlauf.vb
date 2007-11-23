Option Strict Off
Option Explicit On
Public Partial Class EVO_Opt_Verlauf
    Inherits System.Windows.Forms.UserControl

    Private Structure EigenschaftTyp
        Dim NRunden As Short
        Dim NPopul As Short
        Dim NGen As Short
        Dim NNachf As Short
    End Structure

    Dim Eigenschaft As EigenschaftTyp

    '********************************************************************
    'Schnittstelle
    '********************************************************************

    Public Property NRunden() As Short
        Get
            NRunden = Eigenschaft.NRunden
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.NRunden = Value
        End Set
    End Property


    Public Property NPopul() As Short
        Get
            NPopul = Eigenschaft.NPopul
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.NPopul = Value
        End Set
    End Property


    Public Property NGen() As Short
        Get
            NGen = Eigenschaft.NGen
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.NGen = Value
        End Set
    End Property


    Public Property NNachf() As Short
        Get
            NNachf = Eigenschaft.NNachf
        End Get
        Set(ByVal Value As Short)
            Eigenschaft.NNachf = Value
        End Set
    End Property

    Public Sub Initialisieren()
        LabelAnzRunden.Text = Eigenschaft.NRunden
        LabelAnzPop.Text = Eigenschaft.NPopul
        LabelAnzGen.Text = Eigenschaft.NGen
        LabelAnzNachf.Text = Eigenschaft.NNachf

        ProgressBarRunde.Minimum = 0
        ProgressBarRunde.Maximum = Eigenschaft.NRunden
        ProgressBarRunde.Value = 0

        ProgressBarPop.Minimum = 0
        ProgressBarPop.Maximum = Eigenschaft.NPopul
        ProgressBarPop.Value = 0

        ProgressBarGen.Minimum = 0
        ProgressBarGen.Maximum = Eigenschaft.NGen
        ProgressBarGen.Value = 0

        ProgressBarNach.Minimum = 0
        ProgressBarNach.Maximum = Eigenschaft.NNachf
        ProgressBarNach.Value = 0

    End Sub


    Public Sub Runden(ByRef NRunden As Short)
        Eigenschaft.NRunden = NRunden
        ProgressBarRunde.Value = Eigenschaft.NRunden
        LabelaktRunde.Text = Eigenschaft.NRunden.ToString()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Public Sub Populationen(ByRef NPopul As Short)
        Eigenschaft.NPopul = NPopul
        ProgressBarPop.Value = Eigenschaft.NPopul
        LabelaktPop.Text = Eigenschaft.NPopul.ToString()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Public Sub Generation(ByRef NGen As Short)
        Eigenschaft.NGen = NGen
        ProgressBarGen.Value = Eigenschaft.NGen
        LabelaktGen.Text = Eigenschaft.NGen.ToString()
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Public Sub Nachfolger(ByRef NNachf As Short)
        Eigenschaft.NNachf = NNachf
        ProgressBarNach.Value = Eigenschaft.NNachf
        LabelaktNachf.Text = Eigenschaft.NNachf.ToString()
        System.Windows.Forms.Application.DoEvents()
    End Sub

End Class