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
Imports System.IO
Imports BlueM

''' <summary>
''' Klasse Problem
''' Definiert das zu lösende Optimierungsproblem
''' </summary>
Public Class Problem

    'Konstanten
    '##########
    ''' <summary>
    ''' Erweiterung der Datei mit den Optimierungsparametern (*.OPT)
    ''' </summary>
    Public Const FILEEXT_OPT As String = "OPT"
    ''' <summary>
    ''' Erweiterung der Datei mit den Modellparametern (*.MOD)
    ''' </summary>
    Public Const FILEEXT_MOD As String = "MOD"
    ''' <summary>
    ''' Erweiterung der Datei mit den Zielfunktionen (*.OBF)
    ''' </summary>
    Public Const FILEEXT_OBF As String = "OBF"
    ''' <summary>
    ''' Erweiterung der Datei mit den Constraints (*.CON)
    ''' </summary>
    Public Const FILEEXT_CON As String = "CON"

    'Eigenschaften
    '#############

    Private mWorkDir As String
    Private mDatensatz As String

    Private mMethod As String

    ''' <summary>
    ''' Liste der Modellparameter
    ''' </summary>
    Public List_ModellParameter() As Struct_ModellParameter
    ''' <summary>
    ''' Liste der OptParameter
    ''' </summary>
    Public List_OptParameter() As OptParameter
    ''' <summary>
    ''' Liste der Objective Functions
    ''' </summary>
    ''' <remarks>Enthält sowohl Objective Functions als auch PrimaryObjectiveFunctions</remarks>
    Public List_ObjectiveFunctions() As ObjectiveFunction
    ''' <summary>
    ''' Liste der Constraint Functions
    ''' </summary>
    Public List_Constraintfunctions() As Constraintfunction

    'Properties
    '##########

    ''' <summary>
    ''' Arbeitsverzeichnis
    ''' </summary>
    ''' <remarks>nur bei Sim-Anwendungen relevant</remarks>
    Public Property WorkDir() As String
        Get
            Return Me.mWorkDir
        End Get
        Set(ByVal value As String)
            Me.mWorkDir = value
        End Set
    End Property

    ''' <summary>
    ''' Name des zu optimierenden Datensatzes
    ''' </summary>
    ''' <remarks>nur bei Sim-Anwendungen relevant</remarks>
    Public Property Datensatz() As String
        Get
            Return Me.mDatensatz
        End Get
        Set(ByVal value As String)
            Me.mDatensatz = value
        End Set
    End Property

    ''' <summary>
    ''' Name der verwendeten Optimierungsmethode
    ''' </summary>
    Public ReadOnly Property Method() As String
        Get
            Return Me.mMethod
        End Get
    End Property

    ''' <summary>
    ''' Number of model parameters
    ''' </summary>
    Public ReadOnly Property NumModelParams() As Integer
        Get
            Return Me.List_ModellParameter.Length
        End Get
    End Property

    ''' <summary>
    ''' Anzahl Optparameter
    ''' </summary>
    Public ReadOnly Property NumOptParams() As Integer
        Get
            Return Me.List_OptParameter.Length
        End Get
    End Property

    ''' <summary>
    ''' Optimierungsmodus
    ''' </summary>
    ''' <returns>Single-Objective oder Multi-Objective</returns>
    Public ReadOnly Property Modus() As BlueM.Opt.Common.Constants.EVO_MODE
        Get
            Select Case Me.NumPrimObjective
                Case 1
                    Return EVO_MODE.Single_Objective
                Case Is > 1
                    Return EVO_MODE.Multi_Objective
                Case Else
                    Throw New Exception("No primary objective functions are defined!")
            End Select
        End Get
    End Property

    ''' <summary>
    ''' Gesamtanzahl Objective Functions
    ''' </summary>
    ''' <remarks>Inklusive PrimaryObjective Functions!</remarks>
    Public ReadOnly Property NumObjectives() As Integer
        Get
            Return Me.List_ObjectiveFunctions.Length
        End Get
    End Property

    ''' <summary>
    ''' Number of secondary objective functions
    ''' </summary>
    Public ReadOnly Property NumSecObjectives() As Integer
        Get
            Return Me.NumObjectives - Me.NumPrimObjective
        End Get
    End Property

    ''' <summary>
    ''' Anzahl PrimaryObjective Functions
    ''' </summary>
    Public ReadOnly Property NumPrimObjective() As Integer
        Get
            Dim n As Integer

            n = 0
            For Each objective As ObjectiveFunction In Me.List_ObjectiveFunctions
                If (objective.isPrimObjective) Then n += 1
            Next

            Return n
        End Get
    End Property

    ''' <summary>
    ''' Liste der PrimaryObjective Functions
    ''' </summary>
    ''' <remarks>ReadOnly! Zum Setzen von Werten die List_Objectivefunctions verwenden!</remarks>
    Public ReadOnly Property List_PrimObjectiveFunctions() As ObjectiveFunction()
        Get
            Dim i As Integer
            Dim array() As ObjectiveFunction

            ReDim array(Me.NumPrimObjective - 1)

            i = 0
            For Each objective As ObjectiveFunction In Me.List_ObjectiveFunctions
                If (objective.isPrimObjective) Then
                    array(i) = objective
                    i += 1
                End If
            Next

            Return array
        End Get
    End Property

    ''' <summary>
    ''' Anzahl Constraint Functions
    ''' </summary>
    Public ReadOnly Property NumConstraints() As Integer
        Get
            Return Me.List_Constraintfunctions.Length
        End Get
    End Property

    ''' <summary>
    ''' Gets a text description of the problem, listing all objectives, parameters, etc.
    ''' </summary>
    Public ReadOnly Property Description() As String
        Get
            Dim msg As String
            msg = $"Objective Functions ({Me.NumPrimObjective} primary, {Me.NumSecObjectives} secondary):" & eol
            For Each obj As ObjectiveFunction In Me.List_ObjectiveFunctions
                msg &= "* " & obj.Bezeichnung & eol
            Next
            msg &= $"Optimization parameters ({Me.NumOptParams}):" & eol
            For Each optparam As OptParameter In Me.List_OptParameter
                msg &= "* " & optparam.Bezeichnung & eol
            Next
            msg &= $"Model parameters ({Me.NumModelParams}):" & eol
            For Each modparam As Struct_ModellParameter In Me.List_ModellParameter
                msg &= "* " & modparam.Bezeichnung & eol
            Next
            msg &= $"Constraints ({Me.NumConstraints}):" & eol
            For Each constraint As Constraintfunction In Me.List_Constraintfunctions
                msg &= "* " & constraint.Bezeichnung & eol
            Next
            Return msg
        End Get
    End Property

#Region "Methoden"

    ''' <summary>
    ''' Konstruktor
    ''' </summary>
    ''' <param name="Method">zu verwendende Methode</param>
    Public Sub New(ByVal Method As String)

        'Methode setzen
        Me.mMethod = Method

        'Datenstrukturen initialisieren
        ReDim Me.List_ObjectiveFunctions(-1)
        ReDim Me.List_Constraintfunctions(-1)
        ReDim Me.List_OptParameter(-1)
        ReDim Me.List_ModellParameter(-1)

    End Sub

    ''' <summary>
    ''' Alle EVO-Eingabedateien einlesen
    ''' </summary>
    ''' <param name="simstart">Startzeitpunkt der Simulation</param>
    ''' <param name="simende">Endzeitpunkt der Simulation</param>
    ''' <remarks>Liest je nach eingestellter Methode die jeweils erforderlichen Dateien ein</remarks>
    Public Sub Read_InputFiles(ByVal simstart As DateTime, ByVal simende As DateTime)

        'EVO-Eingabedateien einlesen
        '---------------------------
        'Zielfunktionen einlesen
        Call Me.Read_OBF(simstart, simende)

        'Constraints einlesen
        Call Me.Read_CON(simstart, simende)

        'Optimierungsparameter einlesen
        Call Me.Read_OPT()
        'ModellParameter einlesen
        Call Me.Read_MOD()

        'Validierung
        '-----------
        'Modell-/Optparameter validieren
        Call Me.Validate_OPT_fits_to_MOD()
        'Prüfen der Anfangswerte
        Call Me.Validate_Startvalues()


    End Sub

    ''' <summary>
    ''' Optimierungsparameter (*.OPT-Datei) einlesen
    ''' </summary>
    ''' <remarks>http://wiki.bluemodel.org/index.php/OPT-Datei</remarks>
    Private Sub Read_OPT()

        'Format:
        '*|--------------|-------|-----------|--------|--------|-----------|----------|-----------|
        '*| Bezeichnung  | Einh. | Anfangsw. |  Min   |  Max   | Beziehung |  Objekt  | Zeitpunkt |
        '*|-<---------->-|-<--->-|-<------->-|-<---->-|-<---->-|-<------->-|-<------>-|-<------->-|

        Dim Datei As String = IO.Path.Combine(Me.mWorkDir, Me.Datensatz & "." & FILEEXT_OPT)

        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        Dim Zeile As String
        Dim AnzParam As Integer = 0

        'Anzahl der Parameter feststellen
        Do
            Zeile = StrRead.ReadLine()
            If Zeile.Trim().Length = 0 Or Zeile.StartsWith("*") Then
                Continue Do
            End If
            AnzParam += 1
        Loop Until StrRead.Peek() = -1

        ReDim List_OptParameter(AnzParam - 1)

        'Zurück zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        Dim array() As String
        Dim Bez_str As String = ""
        Dim i As Integer = 0
        Do
            Zeile = StrRead.ReadLine()
            If Zeile.Trim().Length = 0 Or Zeile.StartsWith("*") Then
                Continue Do
            End If
            'OptParameter instanzieren
            List_OptParameter(i) = New BlueM.Opt.Common.OptParameter()
            array = Zeile.Split("|")
            'Werte zuweisen
            List_OptParameter(i).Bezeichnung = array(1).Trim()
            List_OptParameter(i).Einheit = array(2).Trim()
            List_OptParameter(i).StartWert = Convert.ToDouble(array(3).Trim(), Common.Provider.FortranProvider)
            List_OptParameter(i).Min = Convert.ToDouble(array(4).Trim(), Common.Provider.FortranProvider)
            List_OptParameter(i).Max = Convert.ToDouble(array(5).Trim(), Common.Provider.FortranProvider)

            'liegt eine Beziehung vor?
            If (i > 0 And array.GetUpperBound(0) > 6) Then
                If Not array(6).Trim() = "" Then
                    Me.List_OptParameter(i).Beziehung = Common.Constants.getRelationship(array(6).Trim())
                End If
            End If

            'Eingelesenen Startwert setzen
            List_OptParameter(i).RWert = List_OptParameter(i).StartWert
            i += 1
        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

    End Sub

    ''' <summary>
    ''' Modellparameter (*.MOD-Datei) einlesen
    ''' </summary>
    ''' <remarks>http://wiki-bluemodel.org/index.php/MOD-Datei</remarks>
    Private Sub Read_MOD()

        'Format:
        '*|--------------|--------------|-------|-------|-------|-------|-----|-----|--------|
        '*| OptParameter | Bezeichnung  | Einh. | Datei | Elem  | Zeile | von | bis | Faktor |
        '*|-<---------->-|-<---------->-|-<--->-|-<--->-|-<--->-|-<--->-|-<->-|-<->-|-<---->-|

        Dim Datei As String = IO.Path.Combine(Me.mWorkDir, Me.Datensatz & "." & FILEEXT_MOD)

        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        Dim Zeile As String
        Dim AnzParam As Integer = 0

        'Anzahl der Parameter feststellen
        Do
            Zeile = StrRead.ReadLine()
            If Zeile.Trim().Length = 0 Or Zeile.StartsWith("*") Then
                Continue Do
            End If
            AnzParam += 1
        Loop Until StrRead.Peek() = -1

        ReDim Me.List_ModellParameter(AnzParam - 1)

        'Zurück zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        Dim array() As String
        Dim i As Integer = 0

        Do
            Zeile = StrRead.ReadLine()
            If Zeile.Trim().Length = 0 Or Zeile.StartsWith("*") Then
                Continue Do
            End If
            array = Zeile.Split("|")
            'Werte zuweisen
            With Me.List_ModellParameter(i)
                .OptParameter = array(1).Trim()
                .Bezeichnung = array(2).Trim()
                .Einheit = array(3).Trim()
                .Datei = array(4).Trim()
                .Element = array(5).Trim()
                .ZeileNr = Convert.ToInt16(array(6).Trim())
                .SpVon = Convert.ToInt16(array(7).Trim())
                .SpBis = Convert.ToInt16(array(8).Trim())
                .Faktor = Convert.ToDouble(array(9).Trim(), Common.Provider.FortranProvider)
            End With
            i += 1
        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

    End Sub

    ''' <summary>
    ''' ObjectiveFunctions (*.OBF) einlesen
    ''' </summary>
    ''' <param name="SimStart">Startzeitpunkt der Simulation</param>
    ''' <param name="SimEnde">Endzeitpunkt der Simulation</param>
    ''' <remarks>http://wiki.bluemodel.org/index.php/OBF-file</remarks>
    Private Sub Read_OBF(ByVal SimStart As DateTime, ByVal SimEnde As DateTime)

        Const AnzSpalten_ObjFSeries As Integer = 13                 'Anzahl Spalten Reihenvergleich in der OBF-Datei
        Const AnzSpalten_ObjFValue As Integer = 12                  'Anzahl Spalten Wertevergleich in der OBF-Datei
        Const AnzSpalten_ObjFValueFromSeries As Integer = 13        'Anzahl Spalten Reihenwertevergleich in der OBF-Datei
        Const AnzSpalten_ObjFAggregate As Integer = 5               'Anzahl Spalten Aggregierte Ziele in der OBF-Datei

        Dim i As Integer
        Dim Zeile As String
        Dim WerteArray() As String

        'Path to file
        Dim filepath As String = IO.Path.Combine(Me.mWorkDir, Me.Datensatz & "." & FILEEXT_OBF)

        'Open the file
        Dim FiStr As New FileStream(filepath, FileMode.Open, IO.FileAccess.Read)
        Dim StrRead As New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        ReDim Me.List_ObjectiveFunctions(-1)
        Dim currentObjectiveType As Common.ObjectiveFunction.ObjectiveType

        Try

            'Read the file
            i = 0
            Do
                Zeile = StrRead.ReadLine()

                'Determine the current block / objective type
                '--------------------------------------------
                If Zeile.StartsWith("*Series") Then
                    currentObjectiveType = ObjectiveFunction.ObjectiveType.Series
                ElseIf Zeile.StartsWith("*Values") Then
                    currentObjectiveType = ObjectiveFunction.ObjectiveType.Value
                ElseIf Zeile.StartsWith("*ValueFromSeries") Then
                    currentObjectiveType = ObjectiveFunction.ObjectiveType.ValueFromSeries
                ElseIf Zeile.StartsWith("*Aggregate") Then
                    currentObjectiveType = ObjectiveFunction.ObjectiveType.Aggregate
                ElseIf Zeile.StartsWith("*SKos") Or Zeile.StartsWith("*Ecology") Then
                    Throw New Exception("The blocks ""SKos"" and ""Ecology"" are no longer supported in the OBF file!")
                End If

                'Skip comment and empty lines
                '----------------------------
                If Zeile.Trim().Length = 0 Or Zeile.StartsWith("*") Then
                    Continue Do
                End If

                WerteArray = Zeile.Split("|")

                'Fallunterscheidung je nach aktuellem Block
                Select Case currentObjectiveType

                    Case ObjectiveFunction.ObjectiveType.Series

                        'Reihenvergleich
                        '===============

                        'Kontrolle
                        If (WerteArray.GetUpperBound(0) <> AnzSpalten_ObjFSeries + 1) Then
                            Throw New Exception("The block ""Series"" in the OBF input file has the wrong number of columns!")
                        End If

                        'ObjectiveFunction instanzieren
                        Dim Objective_Series As New Common.ObjectiveFunction_Series()

                        'Gemeinsame Spalten einlesen
                        Call Me.Read_OBF_CommonColumns(Objective_Series, Zeile)

                        'Restliche Spalten einlesen
                        With Objective_Series
                            If (WerteArray(9).Trim() <> "") Then
                                .EvalStart = WerteArray(9).Trim()
                            Else
                                .EvalStart = SimStart
                            End If
                            If WerteArray(10).Trim() <> "" Then
                                .EvalEnde = WerteArray(10).Trim()
                                'Check
                                If .EvalEnde > SimEnde Then
                                    Throw New Exception($"The end of the evaluation period of the objective function '{ .Bezeichnung}' ({ .EvalEnde}) is later than the simulation end ({SimEnde})!")
                                End If
                            Else
                                .EvalEnde = SimEnde
                            End If
                            .RefGr = WerteArray(11).Trim()
                            .RefReiheDatei = WerteArray(12).Trim()
                            If (WerteArray(13).Trim() <> "") Then
                                .hasIstWert = True
                                .IstWert = Convert.ToDouble(WerteArray(13).Trim(), Common.Provider.FortranProvider)
                                'Reverse the sign for objective functions that should be maximized (#198)
                                If .Richtung = EVO_DIRECTION.Maximization Then
                                    .IstWert = .IstWert * -1
                                End If
                            Else
                                .hasIstWert = False
                            End If

                            'Referenzreihe einlesen
                            .RefReihe = Me.Read_OBF_RefSeries(IO.Path.Combine(Me.mWorkDir, .RefReiheDatei), .RefGr, .EvalStart, .EvalEnde)

                        End With

                        'Neue ObjectiveFunction abspeichern
                        ReDim Preserve Me.List_ObjectiveFunctions(i)
                        Me.List_ObjectiveFunctions(i) = Objective_Series
                        i += 1

                    Case ObjectiveFunction.ObjectiveType.Value

                        'Wertevergleich
                        '==============

                        'Kontrolle
                        If (WerteArray.GetUpperBound(0) <> AnzSpalten_ObjFValue + 1) Then
                            Throw New Exception("The block ""Values"" in the OBF input file has the wrong number of columns!")
                        End If

                        'ObjectiveFunction instanzieren
                        Dim Objective_Value As New Common.Objectivefunction_Value()

                        'Gemeinsame Spalten einlesen
                        Call Me.Read_OBF_CommonColumns(Objective_Value, Zeile)

                        'Restliche Spalten einlesen
                        With Objective_Value
                            .Block = WerteArray(9).Trim()
                            .Spalte = WerteArray(10).Trim()
                            If (WerteArray(11).Trim() <> "") Then
                                .RefWert = Convert.ToDouble(WerteArray(11).Trim(), Common.Provider.FortranProvider)
                            End If
                            If (WerteArray(12).Trim() <> "") Then
                                .hasIstWert = True
                                .IstWert = Convert.ToDouble(WerteArray(12).Trim(), Common.Provider.FortranProvider)
                                'Reverse the sign for objective functions that should be maximized (#198)
                                If .Richtung = EVO_DIRECTION.Maximization Then
                                    .IstWert = .IstWert * -1
                                End If
                            Else
                                .hasIstWert = False
                            End If
                        End With

                        'Neue ObjectiveFunction abspeichern
                        ReDim Preserve Me.List_ObjectiveFunctions(i)
                        Me.List_ObjectiveFunctions(i) = Objective_Value
                        i += 1

                    Case ObjectiveFunction.ObjectiveType.ValueFromSeries

                        'ReihenWertevergleich
                        '====================

                        'Kontrolle
                        If (WerteArray.GetUpperBound(0) <> AnzSpalten_ObjFValueFromSeries + 1) Then
                            Throw New Exception("The block ""ValueFromSeries"" in the OBF input file has the wrong number of columns!")
                        End If

                        'ObjectiveFunction instanzieren
                        Dim Objective_ValueFromSeries As New Common.ObjectiveFunction_ValueFromSeries()

                        'Gemeinsame Spalten einlesen
                        Call Me.Read_OBF_CommonColumns(Objective_ValueFromSeries, Zeile)

                        'Restliche Spalten einlesen
                        With Objective_ValueFromSeries
                            If (WerteArray(9).Trim() <> "") Then
                                .EvalStart = WerteArray(9).Trim()
                            Else
                                .EvalStart = SimStart
                            End If
                            If WerteArray(10).Trim() <> "" Then
                                .EvalEnde = WerteArray(10).Trim()
                                'Check
                                If .EvalEnde > SimEnde Then
                                    Throw New Exception($"The end of the evaluation period of the objective function '{ .Bezeichnung}' ({ .EvalEnde}) is later than the simulation end ({SimEnde})!")
                                End If
                            Else
                                .EvalEnde = SimEnde
                            End If
                            .WertFunktion = WerteArray(11).Trim()
                            If (WerteArray(12).Trim() <> "") Then
                                .RefWert = Convert.ToDouble(WerteArray(12).Trim(), Common.Provider.FortranProvider)
                            End If
                            If (WerteArray(13).Trim() <> "") Then
                                .hasIstWert = True
                                .IstWert = Convert.ToDouble(WerteArray(13).Trim(), Common.Provider.FortranProvider)
                                'Reverse the sign for objective functions that should be maximized (#198)
                                If .Richtung = EVO_DIRECTION.Maximization Then
                                    .IstWert = .IstWert * -1
                                End If
                            Else
                                .hasIstWert = False
                            End If
                        End With

                        'Neue ObjectiveFunction abspeichern
                        ReDim Preserve Me.List_ObjectiveFunctions(i)
                        Me.List_ObjectiveFunctions(i) = Objective_ValueFromSeries
                        i += 1

                    Case ObjectiveFunction.ObjectiveType.Aggregate

                        'Aggregierte Ziele
                        '=================

                        'Kontrolle
                        If (WerteArray.GetUpperBound(0) <> AnzSpalten_ObjFAggregate + 1) Then
                            Throw New Exception("The block ""Aggregate"" in the OBF input file has the wrong number of columns!")
                        End If

                        'ObjectiveFunction instanzieren
                        Dim Objective_Aggregate As New Common.ObjectiveFunction_Aggregate()

                        'Spalten einlesen
                        With Objective_Aggregate
                            If (WerteArray(1).Trim().ToUpper() = "P") Then
                                .isPrimObjective = True
                            Else
                                .isPrimObjective = False
                            End If
                            .Bezeichnung = WerteArray(2).Trim()
                            .Gruppe = WerteArray(3).Trim()
                            If (WerteArray(4).Trim() = "+") Then
                                .Richtung = Common.EVO_DIRECTION.Maximization
                            Else
                                .Richtung = Common.EVO_DIRECTION.Minimization
                            End If
                            If (WerteArray(5).Trim() <> "") Then
                                .hasIstWert = True
                                .IstWert = Convert.ToDouble(WerteArray(5).Trim(), Common.Provider.FortranProvider)
                                'Reverse the sign for objective functions that should be maximized (#198)
                                If .Richtung = EVO_DIRECTION.Maximization Then
                                    .IstWert = .IstWert * -1
                                End If
                            Else
                                .hasIstWert = False
                            End If

                        End With

                        'Neue ObjectiveFunction abspeichern
                        ReDim Preserve Me.List_ObjectiveFunctions(i)
                        Me.List_ObjectiveFunctions(i) = Objective_Aggregate
                        i += 1

                    Case Else

                        Throw New Exception("Could not read the OBF file! Please check the file format.")

                End Select

            Loop Until StrRead.Peek() = -1

        Catch ex As Exception

            Throw ex

        Finally

            StrRead.Close()
            FiStr.Close()

        End Try

        'Call Validate_Objectives()

    End Sub

    ''' <summary>
    ''' Liest die Spalten 1 bis 8 der OBF-Datei ein
    ''' </summary>
    ''' <param name="objective">objective function in der die Werte abgelegt werden sollen</param>
    ''' <param name="zeile">Zeile der OBF-Datei</param>
    ''' <remarks></remarks>
    Private Sub Read_OBF_CommonColumns(ByRef objective As BlueM.Opt.Common.ObjectiveFunction, ByVal zeile As String)

        Dim WerteArray() As String

        WerteArray = zeile.Split("|")

        With objective
            If (WerteArray(1).Trim().ToUpper() = "P") Then
                .isPrimObjective = True
            Else
                .isPrimObjective = False
            End If
            .Bezeichnung = WerteArray(2).Trim()
            .Gruppe = WerteArray(3).Trim()
            If (WerteArray(4).Trim() = "+") Then
                .Richtung = Common.EVO_DIRECTION.Maximization
            Else
                .Richtung = Common.EVO_DIRECTION.Minimization
            End If

            If (WerteArray(5).Trim() = "+") Then
                .OpFact = 1
            ElseIf (WerteArray(5).Trim() = "-") Then
                .OpFact = -1
            ElseIf Not (WerteArray(5).Trim() = "") Then
                .OpFact = Convert.ToDouble(WerteArray(5).Trim())
            End If
            .Datei = WerteArray(6).Trim()
            .SimGr = WerteArray(7).Trim()
            .Funktion = WerteArray(8).Trim()
        End With

    End Sub

    ''' <summary>
    ''' Reads the reference series from file and cuts it to the evaluation period
    ''' </summary>
    ''' <param name="filePath">Path to the time series file</param>
    ''' <param name="refName">Name of the series within the file (can be empty)</param>
    ''' <param name="EvalStart">Start date of evaluation period</param>
    ''' <param name="EvalEnde">End date of evaluation period</param>
    ''' <returns>Zeitreihe</returns>
    Private Function Read_OBF_RefSeries(ByVal filePath As String, ByVal refName As String, ByVal EvalStart As DateTime, ByVal EvalEnde As DateTime) As Wave.TimeSeries

        Dim refSeries As Wave.TimeSeries

        'Referenzreihe aus Datei einlesen
        Try
            Dim fileInstance As Wave.TimeSeriesFile = Wave.TimeSeriesFile.getInstance(filePath)
            If refName = "" Then
                refSeries = fileInstance.getTimeSeries()
            Else
                refSeries = fileInstance.getTimeSeries(refName)
            End If
        Catch ex As Exception
            Throw New Exception($"Unable to read reference series '{filePath}'!{eol}Error: {ex.Message}", ex)
        End Try

        'Zeitraum der Referenzreihe überprüfen
        If (refSeries.StartDate > EvalStart Or refSeries.EndDate < EvalEnde) Then
            'Referenzreihe deckt Evaluierungszeitraum nicht ab
            Throw New Exception($"The reference series '{filePath}' does not cover the evaluation period!")
        End If

        'Referenzreihe auf Evaluierungszeitraum kürzen
        Call refSeries.Cut(EvalStart, EvalEnde)
        If refSeries.Length = 0 Then
            Throw New Exception($"The reference series '{filePath}' is empty after cutting to the evaluation period!")
        End If

        'Check reference series for NaN values
        If refSeries.NaNCount > 0 Then
            MsgBox($"The reference series '{filePath}' contains NaN values. These and the equivalent nodes in the simulation time series will be filtered before calculating the objective function values!", MsgBoxStyle.Exclamation)
        End If

        'Referenzreihe umbenennen
        refSeries.Title += " (reference)"

        Return refSeries

    End Function

    ''' <summary>
    ''' Constraint Functions (*.CON) einlesen
    ''' </summary>
    ''' <param name="SimStart">Startzeitpunkt der Simulation</param>
    ''' <param name="SimEnde">Endzeitpunkt der Simulation</param>
    ''' <remarks>http://wiki.bluemodel.org/index.php/CON-Datei</remarks>
    Private Sub Read_CON(ByVal SimStart As DateTime, ByVal SimEnde As DateTime)

        'Format:
        '*|---------------|----------|-------|-----------|------------|----------------------|-----------------------------|
        '*|               |          |       |           |            |      Grenzwert       |        Grenzreihe           |
        '*| Bezeichnung   | GrenzTyp | Datei | SimGröße  | Oben/Unten | WertTyp  | Grenzwert | Grenzgröße | Datei          |
        '*|---------------|----------|-------|-----------|------------|----------|-----------|------------|----------------|

        Dim i As Integer
        Dim Zeile As String
        Dim WerteArray() As String
        Const AnzSpalten As Integer = 9

        Dim Datei As String = IO.Path.Combine(Me.mWorkDir, Me.Datensatz & "." & FILEEXT_CON)

        If (File.Exists(Datei)) Then

            Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.Read)
            Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

            i = 0
            Do
                'Zeile einlesen
                Zeile = StrRead.ReadLine()
                If Zeile.Trim().Length = 0 Or Zeile.StartsWith("*") Then
                    Continue Do
                End If
                WerteArray = Zeile.Split("|")
                'Kontrolle
                If (WerteArray.GetUpperBound(0) <> AnzSpalten + 1) Then
                    Throw New Exception("The CON input file has the wrong number of columns!")
                End If
                'Neues Constraint anlegen
                ReDim Preserve Me.List_Constraintfunctions(i)
                Me.List_Constraintfunctions(i) = New Common.Constraintfunction()
                'Werte zuweisen
                With Me.List_Constraintfunctions(i)
                    .Bezeichnung = WerteArray(1).Trim()
                    .Typ = WerteArray(2).Trim()
                    .Datei = WerteArray(3).Trim()
                    .SimGr = WerteArray(4).Trim()
                    .GrenzPos = WerteArray(5).Trim()
                    .WertFunktion = WerteArray(6).Trim()
                    If (WerteArray(7).Trim() <> "") Then
                        .GrenzWert = Convert.ToDouble(WerteArray(7).Trim(), Common.Provider.FortranProvider)
                    End If
                    .GrenzGr = WerteArray(8).Trim()
                    .GrenzReiheDatei = WerteArray(9).Trim()
                End With
                i += 1
            Loop Until StrRead.Peek() = -1

            StrRead.Close()
            FiStr.Close()

            'Kontrolle
            '---------
            For i = 0 To Me.NumConstraints - 1
                With Me.List_Constraintfunctions(i)
                    If (Not {"VALUE", "SERIES", "WERT", "REIHE"}.Contains(.Typ.ToUpper())) Then Throw New Exception("Constraints: ThreshType must be either 'Value' or 'Series'!")
                    If (Not .Datei = "WEL") Then Throw New Exception("Constraints: Only 'WEL' file format is currently supported!")
                    If (Not {"UPPER", "LOWER", "OBERGRENZE", "UNTERGRENZE"}.Contains(.GrenzPos.ToUpper())) Then Throw New Exception("Constraints: Bound must be either 'Upper' or 'Lower'!")
                End With
            Next

            'Falls mit Reihen verglichen werden soll werden hier die Reihen eingelesen
            Dim GrenzStart As Date
            Dim GrenzEnde As Date

            For i = 0 To Me.NumConstraints - 1
                With Me.List_Constraintfunctions(i)
                    If ({"SERIES", "REIHE"}.Contains(.Typ.ToUpper())) Then

                        'Read threshold series from file
                        Dim fileInstance As Wave.TimeSeriesFile = Wave.TimeSeriesFile.getInstance(IO.Path.Combine(Me.mWorkDir, .GrenzReiheDatei))
                        If .GrenzGr = "" Then
                            .GrenzReihe = fileInstance.getTimeSeries()
                        Else
                            .GrenzReihe = fileInstance.getTimeSeries(.GrenzGr)
                        End If

                        'Zeitraum der Grenzwertreihe überprüfen
                        '--------------------------------------
                        GrenzStart = .GrenzReihe.StartDate
                        GrenzEnde = .GrenzReihe.EndDate

                        If (GrenzStart > SimStart Or GrenzEnde < SimEnde) Then
                            'Grenzwertreihe deckt Simulationszeitraum nicht ab
                            Throw New Exception($"Constraints: The threshold series '{ .GrenzReiheDatei}' does not cover the simulation period!")
                        Else
                            'Zielreihe auf Simulationszeitraum kürzen
                            Call .GrenzReihe.Cut(SimStart, SimEnde)
                            If .GrenzReihe.Length = 0 Then
                                Throw New Exception($"Constraints: The threshold series '{ .GrenzReiheDatei}' is empty after cutting to the simulation period!")
                            End If
                        End If

                        'Check threshold series for NaN values
                        If .GrenzReihe.NaNCount > 0 Then
                            MsgBox($"The threshold series '{ .GrenzReiheDatei}' contains NaN values. These and the equivalent nodes in the simulation time series will be filtered before checking for contraint violations!", MsgBoxStyle.Exclamation)
                        End If

                        'Grenzwertreihe umbenennen
                        .GrenzReihe.Title += " (threshold)"
                    End If
                End With
            Next

        Else
            'CON-Datei existiert nicht -> keine Constraints
            ReDim Me.List_Constraintfunctions(-1)
        End If

    End Sub


    ''' <summary>
    ''' Prüft ob Optparameter und Modellparameter zusammenpassen
    ''' </summary>
    Private Sub Validate_OPT_fits_to_MOD()

        Dim i, j As Integer
        Dim isValid_A As Boolean = True
        Dim isValid_B As Boolean = True
        Dim isValid As Boolean = False

        'A: Prüfung ob für jeden OptParameter mindestens ein Modellparameter existiert
        For i = 0 To List_OptParameter.GetUpperBound(0)
            isValid = False
            For j = 0 To List_ModellParameter.GetUpperBound(0)
                If List_OptParameter(i).Bezeichnung = List_ModellParameter(j).OptParameter Then
                    isValid = True
                End If
            Next
            If isValid = False Then
                isValid_A = False
            End If
        Next

        'B: Prüfung ob jeder ModellParameter einem richtigen OptParameter zugewiesen ist.
        For i = 0 To List_ModellParameter.GetUpperBound(0)
            isValid = False
            For j = 0 To List_OptParameter.GetUpperBound(0)
                If List_ModellParameter(i).OptParameter = List_OptParameter(j).Bezeichnung Then
                    isValid = True
                End If
            Next
            If isValid = False Then
                isValid_B = False
            End If
        Next

        If Not isValid_A Then
            Throw New Exception("At least one optimization parameter is missing corresponding model parameters!")
        End If

        If Not isValid_B Then
            Throw New Exception("At least one model parameter is not assigned to an optimization parameter!")
        End If

    End Sub

    ''' <summary>
    ''' Prüft ob die Startwerte der OptParameter innerhalb der Min und Max Grenzen liegen
    ''' </summary>
    Private Sub Validate_Startvalues()
        Dim i As Integer

        For i = 0 To List_OptParameter.GetUpperBound(0)
            If Not List_OptParameter(i).RWert <= List_OptParameter(i).Max Or Not List_OptParameter(i).RWert >= List_OptParameter(i).Min Then
                Throw New Exception($"The start value of the optimization parameter {List_OptParameter(i).Bezeichnung} is not within the defined value range!")
            End If
        Next
    End Sub

    '''' <summary>
    '''' Validierungsfunktion der Ziele Datei (Objectives), prüft ob die Gruppenzuordnung passt
    '''' </summary>

    'Public Sub Validate_Objectives()

    '    Dim i As Integer = 0
    '    Dim j As Integer = 1
    '    Dim Group As Boolean = False
    '    Dim nMembers As Integer = 0

    '    For i = 0 To List_ObjectiveFunctions.GetUpperBound(0)

    '        If List_ObjectiveFunctions(i).isGroupLeader Then
    '            Group = True
    '            nMembers = 0
    '            Do While List_ObjectiveFunctions(i).Bezeichnung = List_ObjectiveFunctions(i + 1 + nMembers).Gruppe
    '                nMembers += 1
    '            Loop
    '            If nMembers = 0 Then
    '                Throw New Exception("Dieser GroupLeader hat keine Mitglieder")
    '            End If
    '            i += nMembers
    '        End If

    '    Next
    'End Sub

    ''' <summary>
    ''' Test Tagesganglinie mit Autokalibrierung
    ''' </summary>
    ''' <remarks>
    ''' Beta-Version - erlaubt Kalirbierung der Tagesganlinie
    ''' dafür muss für den jeweiligen Tagesgangwert in der .mod Datei in der Spalte "Elem" "TGG_QH" eingetragen werden
    ''' Vorschlag: Aktivierung der kalibrierung des Tagesganlinie über einen Schalter, damit diese Funktion nicht bei jeder optimierung aufgerufen wird
    ''' Kontakt: Valentin Gamerith
    ''' </remarks>
    Private Sub VG_Kalibrierung_Tagesganglinie()

        Dim i, j As Integer
        Dim VG_sum_TGG As Double
        Dim VG_check_24 As Integer
        Dim VG_Faktor As Double

        VG_check_24 = 0
        VG_sum_TGG = 0

        'Bestimmen der Paramterersumme zum berechenen des notwendigen Faktors um auf 24 zu kommen
        For i = 0 To List_ModellParameter.GetUpperBound(0)
            If Trim(List_ModellParameter(i).Element) = "TGG_QH" Then
                VG_check_24 = VG_check_24 + 1

                For j = 0 To List_OptParameter.GetUpperBound(0)
                    If Trim(List_OptParameter(j).Bezeichnung) = Trim(List_ModellParameter(i).OptParameter) Then 'Parameter gefunden
                        VG_sum_TGG = VG_sum_TGG + List_OptParameter(j).RWert 'aufsummieren
                    End If
                Next
            End If
        Next
        'Überprüft ob 24 Werte zugeordnet wurden
        If VG_check_24 = 24 Then
            'Faktor um auf 24 zu kommen:Xi = Xsim,i * n/Summe(Xi,Sim)
            VG_Faktor = VG_check_24 / VG_sum_TGG
            For i = 0 To List_ModellParameter.GetUpperBound(0)
                If Trim(List_ModellParameter(i).Element) = "TGG_QH" Then
                    List_ModellParameter(i).Faktor = VG_Faktor 'setzt den Faktor für den jeweiligen Tagesgangwert
                End If
            Next
        Else
        End If
    End Sub

    ''' <summary>
    ''' Gibt ein neues Individuum zurück, dessen Optparameter alle auf die Startwerte gesetzt sind
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Das Individuum erhält die ID 1</remarks>
    Public Function getIndividuumStart() As BlueM.Opt.Common.Individuum

        Dim startind As BlueM.Opt.Common.Individuum

        Dim i As Integer

        startind = New BlueM.Opt.Common.Individuum_PES("start", 1)
        'Startwerte der OptParameter setzen
        For i = 0 To Me.NumOptParams - 1
            startind.OptParameter(i).RWert = Me.List_OptParameter(i).StartWert
        Next

        Return startind

    End Function

#End Region 'Methoden

End Class
