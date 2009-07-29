''' <summary>
''' Interface für Algorithmus-Controller
''' </summary>
Public Interface IController

    ''' <summary>
    ''' Initialisiert den Controller und übergibt alle erforderlichen Objekte
    ''' </summary>
    ''' <param name="inputProblem">die Problemdefinition</param>
    ''' <param name="inputSettings">die Einstellungen</param>
    ''' <param name="inputProgress">der Verlauf</param>
    ''' <param name="inputHptDiagramm">das Hauptdiagramm</param>
    Sub Init(ByRef inputProblem As EVO.Common.Problem, _
             ByRef inputSettings As EVO.Common.Settings, _
             ByRef inputProgress As EVO.Common.Progress, _
             ByRef inputHptDiagramm As EVO.Diagramm.Hauptdiagramm)

    ''' <summary>
    ''' Initialisiert den Controller für Sim-Anwendungen
    ''' </summary>
    ''' <param name="inputSim">die Simulationsanwendung</param>
    Sub InitApp(ByRef inputSim As EVO.Apps.Sim)

    ''' <summary>
    ''' Initialisiert den Controller für Testprobleme
    ''' </summary>
    ''' <param name="inputTestprobleme">das Testproblem</param>
    Sub InitApp(ByRef inputTestprobleme As EVO.Apps.Testprobleme)

    ''' <summary>
    ''' Optimierung starten
    ''' </summary>
    Sub Start()

    ''' <summary>
    ''' Optimierung stoppen
    ''' </summary>
    Sub Stoppen()


End Interface
