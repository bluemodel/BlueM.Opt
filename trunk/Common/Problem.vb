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
'**** Klasse Problem                                                        ****
'****                                                                       ****
'**** Autoren: Christoph Hübner, Felix Fröhlich                             ****
'****                                                                       ****
'**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
'**** TU Darmstadt                                                          ****
'*******************************************************************************
'*******************************************************************************

Imports System.IO
Imports BlueM

''' <summary>
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
    ''' <summary>
    ''' Erweiterung der Datei mit der Kombinatorik  (*.CES)
    ''' </summary>
    Public Const FILEEXT_CES As String = "CES"

    'Eigenschaften
    '#############

    Private mWorkDir As String
    Private mDatensatz As String

    Private mMethod As String

    ''' <summary>
    ''' Aktuelle Liste der Modellparameter
    ''' </summary>
    Public List_ModellParameter() As Struct_ModellParameter
    ''' <summary>
    ''' Original-Liste der Modellparameter, die nicht verändert wird
    ''' </summary>
    Public List_ModellParameter_Save() As Struct_ModellParameter
    ''' <summary>
    ''' Aktuelle Liste der OptParameter
    ''' </summary>
    Public List_OptParameter() As OptParameter
    ''' <summary>
    ''' Original-Liste der OptParameter, die nicht verändert wird
    ''' </summary>
    Public List_OptParameter_Save() As OptParameter
    ''' <summary>
    ''' Liste der Objective Functions
    ''' </summary>
    ''' <remarks>Enthält sowohl Objective Functions als auch PrimaryObjectiveFunctions</remarks>
    Public List_ObjectiveFunctions() As ObjectiveFunction
    ''' <summary>
    ''' Liste der Constraint Functions
    ''' </summary>
    Public List_Constraintfunctions() As Constraintfunction
    ''' <summary>
    ''' Liste der Locations
    ''' </summary>
    ''' <remarks>nur bei Kombinatorik verwendet</remarks>
    Public List_Locations() As Struct_Lokation
    ''' <summary>
    ''' Zeigt ob der CES-TestModus aktiv ist
    ''' </summary>
    ''' <remarks>nur bei Kombinatorik verwendet</remarks>
    Public CES_T_Modus As Constants.CES_T_MODUS

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
    ''' Pfad Dimension
    ''' </summary>
    ''' <remarks>nur bei Kombinatorik verwendet</remarks>
    Public ReadOnly Property n_PathDimension() As Integer()
        Get
            Dim i As Integer
            Dim tmpArray() As Integer = {}

            If List_Locations.GetLength(0) = 0 Then
                Throw New Exception("Die Element Gesamtliste wurde abgerufen bevor die Elemente pro Location ermittelt wurden")
            End If

            ReDim tmpArray(List_Locations.GetUpperBound(0))
            For i = 0 To List_Locations.GetUpperBound(0)
                tmpArray(i) = List_Locations(i).List_Massnahmen.GetLength(0)
            Next

            n_PathDimension = tmpArray.Clone
        End Get
    End Property

    ''' <summary>
    ''' Gets a text description of the problem, listing all objectives, parameters, etc.
    ''' </summary>
    Public ReadOnly Property Description() As String
        Get
            Dim msg As String
            msg = "Objective Functions (" & Me.NumPrimObjective & " primary, " & Me.NumSecObjectives & " secondary):" & eol
            For Each obj As ObjectiveFunction In Me.List_ObjectiveFunctions
                msg &= "* " & obj.Bezeichnung & eol
            Next
            msg &= "Optimization parameters (" & Me.NumOptParams & "):" & eol
            For Each optparam As OptParameter In Me.List_OptParameter
                msg &= "* " & optparam.Bezeichnung & eol
            Next
            msg &= "Model parameters (" & Me.NumModelParams & "):" & eol
            For Each modparam As Struct_ModellParameter In Me.List_ModellParameter
                msg &= "* " & modparam.Bezeichnung & eol
            Next
            msg &= "Constraints (" & Me.NumConstraints & "):" & eol
            For Each constraint As Constraintfunction In Me.List_Constraintfunctions
                msg &= "* " & constraint.Bezeichnung & eol
            Next
            'TODO: Problem description for CES
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
        ReDim Me.List_OptParameter_Save(-1)
        ReDim Me.List_ModellParameter(-1)
        ReDim Me.List_ModellParameter_Save(-1)
        ReDim Me.List_Locations(-1)

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
        If (Me.Method <> METH_CES) Then
            Call Me.Read_OPT()
            'ModellParameter einlesen
            Call Me.Read_MOD()
        End If

        'Kombinatorik einlesen
        If (Me.Method = METH_HYBRID Or Me.Method = METH_CES) Then
            Call Me.Read_CES()
            'Testmodus wird ermittelt
            Me.CES_T_Modus = Set_TestModus()
        End If

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
        ReDim List_OptParameter_Save(AnzParam - 1)

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

        'OptParameter werden hier gesichert
        For i = 0 To List_OptParameter.GetUpperBound(0)
            List_OptParameter_Save(i) = List_OptParameter(i).Clone()
        Next

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
        ReDim Me.List_ModellParameter_Save(AnzParam - 1)

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

        'ModellParameter werden hier gesichert
        For i = 0 To Me.List_ModellParameter.GetUpperBound(0)
            Me.List_ModellParameter_Save(i) = Me.List_ModellParameter(i).Clone()
        Next

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
        Const AnzSpalten_ObjFSKos As Integer = 5                    'Anzahl Spalten SKos in der OBF-Datei
        Const AnzSpalten_ObjFEcology As Integer = 5                 'Anzahl Spalten SKos in der OBF-Datei

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
                ElseIf Zeile.StartsWith("*SKos") Then
                    currentObjectiveType = ObjectiveFunction.ObjectiveType.SKos
                ElseIf Zeile.StartsWith("*Ecology") Then
                    currentObjectiveType = ObjectiveFunction.ObjectiveType.Ecology
                ElseIf Zeile.StartsWith("*Aggregate") Then
                    currentObjectiveType = ObjectiveFunction.ObjectiveType.Aggregate
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
                                    Throw New Exception("The end of the evaluation period of the objective function '" & .Bezeichnung & "' (" & .EvalEnde & ") is later than the simulation end (" & SimEnde & "!")
                                End If
                            Else
                                .EvalEnde = SimEnde
                            End If
                            .RefGr = WerteArray(11).Trim()
                            .RefReiheDatei = WerteArray(12).Trim()
                            If (WerteArray(13).Trim() <> "") Then
                                .hasIstWert = True
                                .IstWert = Convert.ToDouble(WerteArray(13).Trim(), Common.Provider.FortranProvider)
                                'BUG 303: Reverse the sign for objective functions that should be maximized
                                If .Richtung = EVO_DIRECTION.Maximization Then
                                    .IstWert = .IstWert * -1
                                End If
                            Else
                                .hasIstWert = False
                            End If

                            'Referenzreihe einlesen
                            .RefReihe = Me.Read_ZIE_RefReihe(IO.Path.Combine(Me.mWorkDir, .RefReiheDatei), .RefGr, .EvalStart, .EvalEnde)

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
                                'BUG 303: Reverse the sign for objective functions that should be maximized
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
                                    Throw New Exception("The end of the evaluation period of the objective function '" & .Bezeichnung & "' (" & .EvalEnde & ") is later than the simulation end (" & SimEnde & "!")
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
                                'BUG 303: Reverse the sign for objective functions that should be maximized
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

                    Case ObjectiveFunction.ObjectiveType.SKos

                        'SKos
                        '=================

                        Dim Objective_SKos As New Common.ObjectiveFunction_SKos()

                        'Kontrolle
                        If (WerteArray.GetUpperBound(0) <> AnzSpalten_ObjFSKos + 1) Then
                            Throw New Exception("The block ""SKos"" in the OBF input file has the wrong number of columns!")
                        End If

                        'Spalten einlesen
                        With Objective_SKos
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
                        End With

                        'SKos initialisieren
                        Objective_SKos.initialize(Me)

                        'Neue ObjectiveFunction abspeichern
                        ReDim Preserve Me.List_ObjectiveFunctions(i)
                        Me.List_ObjectiveFunctions(i) = Objective_SKos
                        i += 1

                    Case ObjectiveFunction.ObjectiveType.Ecology

                        'Ecology
                        '=================

                        Dim Objective_Ecology As New Common.ObjectiveFunction_Ecology()

                        'Kontrolle
                        If (WerteArray.GetUpperBound(0) <> AnzSpalten_ObjFEcology + 1) Then
                            Throw New Exception("The block ""Ecology"" in the OBF input file has the wrong number of columns!")
                        End If

                        'Spalten einlesen
                        With Objective_Ecology
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
                        End With

                        'Ecology initialisieren
                        Objective_Ecology.initialize(Me)

                        'Neue ObjectiveFunction abspeichern
                        ReDim Preserve Me.List_ObjectiveFunctions(i)
                        Me.List_ObjectiveFunctions(i) = Objective_Ecology
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
                                'BUG 303: Reverse the sign for objective functions that should be maximized
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
    ''' Referenzreihe aus Datei einlesen und zuschneiden
    ''' </summary>
    ''' <param name="dateipfad">Pfad zur Zeitreihendatei</param>
    ''' <param name="refgroesse">Name der einzulesenden Reihe</param>
    ''' <param name="EvalStart">Startpunkt der Evaluierung</param>
    ''' <param name="EvalEnde">Endpunkt der Evaluierung</param>
    ''' <returns>Zeitreihe</returns>
    Private Function Read_ZIE_RefReihe(ByVal dateipfad As String, ByVal refgroesse As String, ByVal EvalStart As DateTime, ByVal EvalEnde As DateTime) As Wave.TimeSeries

        Dim RefReihe As Wave.TimeSeries

        'Referenzreihe aus Datei einlesen
        Dim dateiobjekt As Wave.FileFormatBase = Wave.FileFactory.getFileInstance(dateipfad)
        If refgroesse = "" Then
            RefReihe = dateiobjekt.getTimeSeries()
        Else
            RefReihe = dateiobjekt.getTimeSeries(refgroesse)
        End If

        'Zeitraum der Referenzreihe überprüfen
        If (RefReihe.StartDate > EvalStart Or RefReihe.EndDate < EvalEnde) Then
            'Referenzreihe deckt Evaluierungszeitraum nicht ab
            Throw New Exception("The reference series '" & dateipfad & "' does not cover the evaluation period!")
        Else
            'Referenzreihe auf Evaluierungszeitraum kürzen
            Call RefReihe.Cut(EvalStart, EvalEnde)
        End If

        'Check reference series for NaN values
        If RefReihe.Nodes.Count > RefReihe.NodesClean.Count Then
            Throw New Exception("The reference series '" & dateipfad & "' contains NaN values, please remove all NaN values before use!")
        End If

        'Referenzreihe umbenennen
        RefReihe.Title += " (reference)"

        Return RefReihe

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

        Dim ext As String
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
                    If (Not {"UPPER", "LOWER", "OBERGRENZE", "UNTERGRENZE"}.Contains(.GrenzPos.ToUpper())) Then Throw New Exception("Constraints: Bound must be wither 'Upper' or 'Lower'!")
                End With
            Next

            'Falls mit Reihen verglichen werden soll werden hier die Reihen eingelesen
            Dim GrenzStart As Date
            Dim GrenzEnde As Date

            For i = 0 To Me.NumConstraints - 1
                With Me.List_Constraintfunctions(i)
                    If ({"SERIES", "REIHE"}.Contains(.Typ.ToUpper())) Then

                        'Dateiendung der Grenzwertdatei bestimmen und Reihe einlesen
                        ext = System.IO.Path.GetExtension(.GrenzReiheDatei)
                        Select Case (ext.ToUpper)
                            Case ".WEL"
                                Dim WEL As New Wave.WEL(IO.Path.Combine(Me.mWorkDir, .GrenzReiheDatei))
                                .GrenzReihe = WEL.getTimeSeries(.GrenzGr)
                            Case ".ZRE"
                                Dim ZRE As New Wave.ZRE(IO.Path.Combine(Me.mWorkDir, .GrenzReiheDatei))
                                .GrenzReihe = ZRE.getTimeSeries(0)
                            Case Else
                                Throw New Exception("Constraints: The file format of the threshold series '" & .GrenzReiheDatei & "' is not supported!")
                        End Select

                        'Zeitraum der Grenzwertreihe überprüfen
                        '--------------------------------------
                        GrenzStart = .GrenzReihe.StartDate
                        GrenzEnde = .GrenzReihe.EndDate

                        If (GrenzStart > SimStart Or GrenzEnde < SimEnde) Then
                            'Grenzwertreihe deckt Simulationszeitraum nicht ab
                            Throw New Exception("Constraints: The threshold series '" & .GrenzReiheDatei & "' does not cover the simulation period!")
                        Else
                            'Zielreihe auf Simulationszeitraum kürzen
                            Call .GrenzReihe.Cut(SimStart, SimEnde)
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
    ''' Kombinatorik (*.CES) einlesen
    ''' </summary>
    ''' <remarks>Nur bei Kombinatorik verwendet. http://wiki.bluemodel.org/index.php/CES-Datei</remarks>
    Private Sub Read_CES()

        Dim Datei As String = IO.Path.Combine(Me.mWorkDir, Me.Datensatz & "." & BlueM.Opt.Common.Problem.FILEEXT_CES)

        Dim FiStr As FileStream = New FileStream(Datei, FileMode.Open, IO.FileAccess.ReadWrite)
        Dim StrRead As StreamReader = New StreamReader(FiStr, System.Text.Encoding.GetEncoding("iso8859-1"))

        Dim Zeile As String
        Dim Anz As Integer = 0

        'Anzahl der Parameter feststellen
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                Anz += 1
            End If
        Loop Until StrRead.Peek() = -1

        Dim i As Integer = -1
        Dim j As Integer = 0
        ReDim Me.List_Locations(0)
        ReDim Me.List_Locations(0).List_Massnahmen(0)

        'Zurück zum Dateianfang und lesen
        FiStr.Seek(0, SeekOrigin.Begin)

        Dim array() As String
        Do
            Zeile = StrRead.ReadLine.ToString()
            If (Zeile.StartsWith("*") = False) Then
                array = Zeile.Split("|")
                'Werte zuweisen

                If Not Is_Name_IN(array(1).Trim(), Me.List_Locations) Then
                    i += 1
                    j = 0
                    System.Array.Resize(Me.List_Locations, i + 1)
                    Me.List_Locations(i).Name = array(1).Trim()
                End If
                System.Array.Resize(Me.List_Locations(i).List_Massnahmen, j + 1)
                ReDim Me.List_Locations(i).List_Massnahmen(j).Schaltung(2, 1)
                ReDim Me.List_Locations(i).List_Massnahmen(j).Bauwerke(3)
                With Me.List_Locations(i).List_Massnahmen(j)
                    .Name = array(2).Trim()
                    .Schaltung(0, 0) = array(3).Trim()
                    .Schaltung(0, 1) = array(4).Trim()
                    .Schaltung(1, 0) = array(5).Trim()
                    .Schaltung(1, 1) = array(6).Trim()
                    .Schaltung(2, 0) = array(7).Trim()
                    .Schaltung(2, 1) = array(8).Trim()
                    .KostenTyp = array(9).Trim()
                    .Volumen = array(10).Trim()
                    .Bauwerke(0) = array(11).Trim()
                    .Bauwerke(1) = array(12).Trim()
                    .Bauwerke(2) = array(13).Trim()
                    .Bauwerke(3) = array(14).Trim()
                    .TestModus = Convert.ToInt16(array(15).Trim())
                End With
                j += 1
            End If

        Loop Until StrRead.Peek() = -1

        StrRead.Close()
        FiStr.Close()

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
                Throw New Exception("The start value of the optimization parameter " & List_OptParameter(i).Bezeichnung & " is not within the defined value range!")
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
    ''' Validierungsfunktion der Kombinatorik Prüft ob Verbraucher an zwei Standorten doppelt vorhanden sind
    ''' </summary>
    ''' <remarks>Nur bei Kombinatorik verwendet</remarks>
    Public Sub Validate_Combinatoric()

        Dim i, j, x, y, m, n As Integer

        For i = 0 To List_Locations.GetUpperBound(0)
            For j = 1 To List_Locations.GetUpperBound(0)
                For x = 0 To List_Locations(i).List_Massnahmen.GetUpperBound(0)
                    For y = 0 To List_Locations(j).List_Massnahmen.GetUpperBound(0)
                        For m = 0 To 2
                            For n = 0 To 2
                                If Not List_Locations(i).List_Massnahmen(x).Schaltung(m, 0) = "X" And List_Locations(j).List_Massnahmen(y).Schaltung(n, 0) = "X" Then
                                    If List_Locations(i).List_Massnahmen(x).Schaltung(m, 0) = List_Locations(j).List_Massnahmen(y).Schaltung(n, 0) Then
                                        Throw New Exception("Kombinatorik ist nicht valid!")
                                    End If
                                End If
                            Next
                        Next
                    Next
                Next
            Next
        Next

    End Sub

    ''' <summary>
    ''' Reduziert die OptParameter und die ModellParameter auf die aktiven Elemente. 
    ''' !Wird jetzt aus den Elementen des Child generiert!
    ''' </summary>
    ''' <param name="Elements">???</param>
    ''' <returns>???</returns>
    ''' <remarks>Nur bei Kombinatorik verwendet</remarks>
    Public Function Reduce_OptPara_and_ModPara(ByRef Elements() As String) As Boolean

        Reduce_OptPara_and_ModPara = True 'Wird wirklich abgefragt!
        Dim i As Integer

        'Kopieren der Listen aus den Sicherungen
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        Call Reset_OptPara_and_ModPara()

        'Reduzierung der ModParameter
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxx
        Dim j, count As Integer
        Dim TMP_ModPara() As Struct_ModellParameter
        ReDim TMP_ModPara(List_ModellParameter.GetUpperBound(0))

        count = 0
        For i = 0 To List_ModellParameter.GetUpperBound(0)
            For j = 0 To Elements.GetUpperBound(0)
                If List_ModellParameter(i).Element = Elements(j) Then
                    TMP_ModPara(count) = List_ModellParameter(i).Clone()
                    count += 1
                End If
            Next
        Next

        'Immer dann wenn nicht Nullvariante
        '**********************************
        If count = 0 Then
            Reduce_OptPara_and_ModPara = False
        Else
            Array.Resize(TMP_ModPara, count)
            Array.Resize(List_ModellParameter, count)

            For i = 0 To TMP_ModPara.GetUpperBound(0)
                List_ModellParameter(i) = TMP_ModPara(i).Clone()
            Next

            'Reduzierung der OptParameter
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxx
            Dim TMP_OptPara() As BlueM.Opt.Common.OptParameter
            ReDim TMP_OptPara(List_OptParameter.GetUpperBound(0))

            count = 0
            For i = 0 To List_OptParameter.GetUpperBound(0)
                For j = 0 To List_ModellParameter.GetUpperBound(0)
                    If List_OptParameter(i).Bezeichnung = List_ModellParameter(j).OptParameter Then
                        TMP_OptPara(count) = List_OptParameter(i).Clone()
                        count += 1
                        Exit For
                    End If
                Next
            Next

            'ToDo: Dieser Fall ist nur Relevant, wenn CES + PES sequentiell
            If count = 0 Then
                Throw New Exception("Die aktuelle Kombination enthält keine Bauwerke, für die OptimierungsParameter vorliegen")
            End If

            Array.Resize(TMP_OptPara, count)
            Array.Resize(List_OptParameter, count)

            For i = 0 To TMP_OptPara.GetUpperBound(0)
                List_OptParameter(i) = TMP_OptPara(i).Clone()
            Next

        End If

    End Function

    ''' <summary>
    ''' Setzt die Listen der OptParameter und Modellparameter wieder zurück auf alles was in den Eingabedateien steht
    ''' </summary>
    Public Sub Reset_OptPara_and_ModPara()
        Dim i As Integer

        'Kopieren der Listen aus den Sicherungen
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        ReDim List_ModellParameter(List_ModellParameter_Save.GetUpperBound(0))
        For i = 0 To List_ModellParameter_Save.GetUpperBound(0)
            List_ModellParameter(i) = List_ModellParameter_Save(i).Clone()
        Next
        ReDim List_OptParameter(List_OptParameter_Save.GetUpperBound(0))
        For i = 0 To List_OptParameter_Save.GetUpperBound(0)
            List_OptParameter(i) = List_OptParameter_Save(i).Clone()
        Next

    End Sub

    ''' <summary>
    ''' Anzahl maximal möglicher Kombinationen
    ''' </summary>
    ''' <remarks>Nur bei Kombinatorik verwendet</remarks>
    Public ReadOnly Property NumCombinations() As Integer
        Get
            If (Me.CES_T_Modus = BlueM.Opt.Common.Constants.CES_T_MODUS.One_Combi) Then
                NumCombinations = 1
            Else
                Dim i As Integer
                NumCombinations = Me.List_Locations(0).List_Massnahmen.GetLength(0)
                For i = 1 To Me.List_Locations.GetUpperBound(0)
                    NumCombinations = NumCombinations * Me.List_Locations(i).List_Massnahmen.GetLength(0)
                Next
            End If
        End Get
    End Property

    ''' <summary>
    ''' Anzahl Locations
    ''' </summary>
    ''' <remarks>Nur bei Kombinatorik verwendet</remarks>
    Public ReadOnly Property NumLocations() As Integer
        Get
            Return Me.List_Locations.Length
        End Get
    End Property

    ''' <summary>
    ''' Hilfsfunktion um zu Prüfen ob der Name bereits vorhanden ist oder nicht
    ''' </summary>
    ''' <param name="name">???</param>
    ''' <param name="array_modellparameter">???</param>
    ''' <returns>???</returns>
    ''' <remarks>Nur bei Kombinatorik verwendet</remarks>
    Public Shared Function Is_Name_IN(ByVal name As String, ByVal array_modellparameter() As BlueM.Opt.Common.Struct_Lokation) As Boolean
        Is_Name_IN = False
        Dim i As Integer
        For i = 0 To array_modellparameter.GetUpperBound(0)
            If name = array_modellparameter(i).Name Then
                Is_Name_IN = True
                Exit Function
            End If
        Next
    End Function

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
    ''' Überprüft ob und welcher CES-TestModus aktiv ist
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Nur bei Kombinatorik verwendet</remarks>
    Public Function Set_TestModus() As Common.Constants.CES_T_MODUS

        Dim i, j As Integer
        Dim count_A As Integer
        Dim count_B As Integer
        Dim Bool As Boolean = False

        'Prüft auf den Modus "0" kein TestModus
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        count_A = 0
        count_B = 0
        For i = 0 To Me.List_Locations.GetUpperBound(0)
            For j = 0 To Me.List_Locations(i).List_Massnahmen.GetUpperBound(0)
                count_A += 1
                If (Me.List_Locations(i).List_Massnahmen(j).TestModus = 0) Then
                    count_B += 1
                End If
            Next
        Next

        If count_A = count_B Then
            Set_TestModus = Common.Constants.CES_T_MODUS.No_Test
            Exit Function
        End If

        'Prüft aus Testen einer definierten Kombination
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        count_A = 0
        count_B = 0
        For i = 0 To Me.List_Locations.GetUpperBound(0)
            count_A += 1
            For j = 0 To Me.List_Locations(i).List_Massnahmen.GetUpperBound(0)
                If Me.List_Locations(i).List_Massnahmen(j).TestModus = 1 Then
                    count_B += 1
                End If
            Next
        Next

        If count_A = count_B Then
            Set_TestModus = Common.Constants.CES_T_MODUS.One_Combi
            Exit Function
        End If

        'Prüft auf einmaliges Testen aller möglichen Kombinationen
        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        count_A = 0
        count_B = 0
        For i = 0 To Me.List_Locations.GetUpperBound(0)
            For j = 0 To Me.List_Locations(i).List_Massnahmen.GetUpperBound(0)
                count_A += 1
                If Me.List_Locations(i).List_Massnahmen(j).TestModus = 1 Then
                    count_B += 1
                End If
            Next
        Next

        If count_A = count_B Then
            Set_TestModus = Constants.CES_T_MODUS.All_Combis
            Exit Function
        End If

        Throw New Exception("Fehler bei der Angabe des Testmodus")

    End Function

    ''' <summary>
    ''' Gibt ein neues Individuum zurück, dessen Optparameter alle auf die Startwerte gesetzt sind
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Das Individuum erhält die ID 1</remarks>
    Public Function getIndividuumStart() As BlueM.Opt.Common.Individuum

        Dim startind As BlueM.Opt.Common.Individuum

        Dim i As Integer

        Select Case Me.Method
            Case METH_CES, METH_HYBRID

                'CES und HYBRID
                '==============
                Dim IndCES As BlueM.Opt.Common.Individuum_CES
                IndCES = New BlueM.Opt.Common.Individuum_CES("start", 1)
                'Startpfad setzen
                For i = 0 To IndCES.Path.GetUpperBound(0)
                    IndCES.Path(i) = 0
                Next
                'TODO:
                'For i = 0 To Me.List_Locations.GetUpperBound(0)
                '    For j = 0 To Me.List_Locations(i).List_Massnahmen.GetUpperBound(0)
                '        If Not Me.List_Locations(i).List_Massnahmen(j).TestModus = 0 Then
                '            IndCES.path(i) = 
                '        End If
                '    Next
                'Next
                'TODO: Startparameter für HYBRID?
                startind = IndCES

            Case Else
                'Alle anderen Methoden
                '=====================
                startind = New BlueM.Opt.Common.Individuum_PES("start", 1)
                'Startwerte der OptParameter setzen
                For i = 0 To Me.NumOptParams - 1
                    startind.OptParameter(i).RWert = Me.List_OptParameter(i).StartWert
                Next
        End Select

        Return startind

    End Function

#End Region 'Methoden

End Class
