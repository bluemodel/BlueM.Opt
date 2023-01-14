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
