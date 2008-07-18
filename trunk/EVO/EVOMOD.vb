Module EVOMOD

    '***********************************************************************************
    '***********************************************************************************
    '**** Klasse EVOMOD                                                             ****
    '****                                                                           ****
    '**** Autoren: Christoph H�bner, Felix Fr�hlich, Dirk Muschalla, Dominik Kerber ****
    '****                                                                           ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung                  ****
    '**** TU Darmstadt                                                              ****
    '****                                                                           ****
    '**** November 2007                                                             ****
    '****                                                                           ****
    '**** Letzte �nderung: November 2007                                            ****
    '***********************************************************************************
    '***********************************************************************************

    'Anwendungen
    Public Const ANW_BLUEM As String = "BlueM"
    Public Const ANW_SMUSI As String = "SMUSI"
    Public Const ANW_SCAN As String = "S:CAN"
    Public Const ANW_SWMM As String = "SWMM"
    Public Const ANW_TESTPROBLEME As String = "Testprobleme"
    Public Const ANW_TSP As String = "Traveling Salesman"

    'Optimierungsmethoden
    Public Const METH_RESET As String = "Reset"
    Public Const METH_PES As String = "PES"
    Public Const METH_CES As String = "CES"
    Public Const METH_HYBRID As String = "HYBRID"
    Public Const METH_SENSIPLOT As String = "SensiPlot"
    Public Const METH_HOOKJEEVES As String = "Hooke & Jeeves"
    Public Const METH_Hybrid2008 As String = "Hybrid2008"

    'Verschiedenes
    Public Const eol As String = Chr(13) & Chr(10)             'Zeilenumbruch

End Module