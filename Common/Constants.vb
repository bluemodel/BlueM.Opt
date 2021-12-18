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
Option Strict Off
Option Explicit On

Public Module Constants

    '*******************************************************************************
    '*******************************************************************************
    '**** Modul mit Konstanten                                                  ****
    '****                                                                       ****
    '**** Autoren: Felix Fröhlich, Christoph Hübner, Dirk Muschalla                            ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

    'URL zur Hilfe
    Public Const HelpURL As String = "http://wiki.bluemodel.org/index.php/BlueM.Opt"

    'Verschiedenes
    Public Const eol As String = Chr(13) & Chr(10)             'Zeilenumbruch

    'ApplicationTypes
    Public Enum ApplicationTypes As Integer
        Sim = 1
        Testproblems = 2
    End Enum

    'Anwendungen
    Public Const ANW_BLUEM As String = "BlueM.Sim"
    Public Const ANW_SMUSI As String = "SMUSI"
    Public Const ANW_SWMM As String = "SWMM"
    Public Const ANW_TALSIM As String = "TALSIM"
    Public Const ANW_TESTPROBLEMS As String = "Testproblems"
    Public Const ANW_TSP As String = "Traveling Salesman"

    'Optimierungsmethoden
    Public Const METH_PES As String = "PES"
    Public Const METH_SENSIPLOT As String = "SensiPlot"
    Public Const METH_HOOKEJEEVES As String = "Hooke & Jeeves"
    Public Const METH_METAEVO As String = "MetaEvo"
    Public Const METH_DDS As String = "DDS"
    Public Const METH_TSP As String = "TSP"

    'Lösungs- / Entscheidungsraum
    Public Enum SPACE As Integer
        DecisionSpace = 0
        SolutionSpace = 1
    End Enum

    'Option für Startvorgabe für Optimierung
    Public Enum EVO_STARTPARAMETERS As Integer
        Random = 1
        Original = 2
    End Enum

    'Modus der Optimierung
    Public Enum EVO_MODE As Integer
        Single_Objective = 0
        Multi_Objective = 1
    End Enum

    'Evo-Strategie-Typ
    Public Enum EVO_STRATEGY As Integer
        Plus_Strategy = 1                                'Eltern + Nachfolger
        Comma_Strategy = 2                               'nur Nachfolger
    End Enum

    'Option zur Erzeugung der Eltern
    Public Enum PES_REPRODOP As Integer
        Selection = 1                           'zufällige Selektion as Bestwertspeicher
        XX_Discrete = 2                         'Rekombination nach x/x-Schema, diskretes vertauschen der Gene
        XX_Average = 3                          'Rekombination nach x/x-Schema, mittelwertbildung der Gene über y-Eltern
        XY_Discrete = 4                         'Rekombination nach x/y-Schema, diskretes vertauschen der Gene
        XY_Average = 5                          'Rekombination nach x/y-Schema, mittelwertbildung der Gene über y-Eltern
        Neighborhood = 6                        'Neighborhood-Rekombination
        XX_Average_Discrete = 7                 'Rekombination nach x/x-Schema, diskretes vertauschen der Gene, miteln der Strategieparameter 
        XY_Average_Discrete = 8                 'Rekombination nach x/y-Schema, diskretes vertauschen der Gene, miteln der Strategieparameter
    End Enum

    'Option für die Mutation
    Public Enum PES_MUTATIONSOP As Integer
        Rechenberg = 1
        Schwefel = 2
    End Enum


    'Option zur Erzeugung der Pop-Eltern
    Public Enum EVO_POP_ELTERN As Integer
        Recombination = 1                       'Rekombination aus den PopEltern
        Average = 2                             'Mittelwertbildung aus den PopEltern
        Selection = 3                           'zufällige Selektion aus Bestwertspeicher
    End Enum

    'Option zur Ermittlung der Populationsqualität
    Public Enum EVO_POP_PENALTY As Integer
        Average = 1                             'SingleObjective
        Worst = 2                               'SingleObjective
        Crowding = 3                            'MultiObjective
        Span = 4                                'MultiObjective
    End Enum

    'Beziehung
    Public Enum Relationship As Integer
        none = 0
        smaller_than = 1
        smaller_equal = 2
        larger_than = 3
        larger_equal = 4
    End Enum

    'String in der Form < >, <=, >= in Beziehung umwandeln
    '*****************************************************
    Public Function getRelationship(ByVal rel_str As String) As Relationship
        Select Case rel_str
            Case "<"
                Return Relationship.smaller_than
            Case "<="
                Return Relationship.smaller_equal
            Case ">"
                Return Relationship.larger_than
            Case ">="
                Return Relationship.larger_equal
            Case Else
                Throw New Exception("Relationship '" & rel_str & "' not recognized!")
        End Select
    End Function

    'Richtung einer Zielfunktion
    Public Enum EVO_DIRECTION As Integer
        Minimization = 1
        Maximization = -1
    End Enum

    'TSP Enums
    Enum EnReprodOperator
        Order_Crossover_OX = 1
        Partially_Mapped_Crossover_PMX = 2
    End Enum

    Enum EnMutOperator
        Inversion_SIM = 1
        Translocation_3_Opt = 2
        Translocation_n_Opt = 3
        Exchange_Mutation_EM = 4
    End Enum

    Enum EnProblem
        circle = 0
        random = 1
    End Enum

End Module
