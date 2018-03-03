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
''' <summary>
''' Interface für Algorithmus-Controller
''' </summary>
Public Interface IController

    ''' <summary>
    ''' Gibt an, ob der Algorithmus Multithreading unterstützt
    ''' </summary>
    ReadOnly Property MultithreadingSupported() As Boolean

    ''' <summary>
    ''' Initialisiert den Controller und übergibt alle erforderlichen Objekte
    ''' </summary>
    ''' <param name="inputProblem">die Problemdefinition</param>
    ''' <param name="inputSettings">die Einstellungen</param>
    ''' <param name="inputProgress">der Verlauf</param>
    ''' <param name="inputHptDiagramm">das Hauptdiagramm</param>
    Sub Init(ByRef inputProblem As BlueM.Opt.Common.Problem, _
             ByRef inputSettings As BlueM.Opt.Common.Settings, _
             ByRef inputProgress As BlueM.Opt.Common.Progress, _
             ByRef inputHptDiagramm As BlueM.Opt.Diagramm.Hauptdiagramm)

    ''' <summary>
    ''' Initialisiert den Controller für Sim-Anwendungen
    ''' </summary>
    ''' <param name="inputSim">die Simulationsanwendung</param>
    Sub InitApp(ByRef inputSim As BlueM.Opt.Apps.Sim)

    ''' <summary>
    ''' Initialisiert den Controller für Testprobleme
    ''' </summary>
    ''' <param name="inputTestprobleme">das Testproblem</param>
    Sub InitApp(ByRef inputTestprobleme As BlueM.Opt.Apps.Testprobleme)

    ''' <summary>
    ''' Optimierung starten
    ''' </summary>
    Sub Start()

    ''' <summary>
    ''' Optimierung stoppen
    ''' </summary>
    Sub Stoppen()


End Interface
