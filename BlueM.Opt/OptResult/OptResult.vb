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
Imports System.Data.OleDb

''' <summary>
''' Speichert und verwaltet die Ergebnisse eines Optimierungslaufs,
''' schreibt die Ergebnisse in eine Datenbank
''' </summary>
Public Class OptResult

    'Allgemeine Einstellungen
    Private Datensatz As String

    'Das Problem
    Private mProblem As Common.Problem

    'Ergebnisdatenbank
    Public Ergebnisdb As Boolean = True              'Gibt an, ob die Ergebnisdatenbank geschrieben werden soll
    Private db_path As String                        'Pfad zur Ergebnisdatenbank
    Private db As OleDb.OleDbConnection

    'Array von Lösungen
    Public Solutions() As Common.Individuum

    'Structure für Sekundäre Population
    Public Structure Struct_SekPop
        Public iGen As Integer                      'Generationsnummer
        Public SolutionIDs() As Integer             'Array von Solution-IDs
    End Structure

    'Array von Sekundären Populationen
    Public SekPops() As Struct_SekPop

    'Array von ausgewählten Lösungen
    Private selSolutionIDs() As Integer

    ''' <summary>
    ''' Zeigt an, ob auch die Optparameter-Werte im OptResult enthalten sind
    ''' </summary>
    Public holdsOptparameters As Boolean

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="Datensatzname"></param>
    ''' <param name="prob"></param>
    ''' <param name="createNewMdb"></param>
    ''' <param name="starttime">optional start time to use for the database filename</param>
    Public Sub New(ByVal Datensatzname As String, ByRef prob As Common.Problem, Optional ByVal createNewMdb As Boolean = True, Optional starttime As DateTime = Nothing)

        'Standardmäßig mit Optparametern
        Me.holdsOptparameters = True

        'Datensatzname speichern
        Me.Datensatz = Datensatzname

        'Problem speichern
        Me.mProblem = prob

        ReDim Me.Solutions(-1)
        ReDim Me.selSolutionIDs(-1)
        ReDim Me.SekPops(-1)

        If (createNewMdb) Then
            If IsNothing(starttime) Then
                starttime = DateTime.Now
            End If
            'DB initialiseren
            Call Me.db_init(prob.WorkDir, prob.Datensatz, starttime)
        End If

    End Sub

#Region "Ergebnisspeicher"

    'Eine Lösung auswählen
    '*********************
    Public Function selectSolution(ByVal ID As Integer) As Boolean

        'Überprüfen, ob Lösung bereits ausgewählt ist      
        For Each _id As Integer In Me.selSolutionIDs
            If (_id = ID) Then
                Return False
            End If
        Next

        'Lösung zu Auswahl hinzufügen
        ReDim Preserve Me.selSolutionIDs(Me.selSolutionIDs.GetUpperBound(0) + 1)
        Me.selSolutionIDs(Me.selSolutionIDs.GetUpperBound(0)) = ID

        Return True

    End Function

    'Ausgewählte Lösungen holen
    '**************************
    Public ReadOnly Property getSelectedSolutions() As Common.Individuum()
        Get
            Dim solutions() As Common.Individuum

            solutions = getSolutions(Me.selSolutionIDs)

            Return solutions
        End Get
    End Property

    'Lösungsauswahl zurücksetzen
    '***************************
    Public Sub clearSelectedSolutions()

        ReDim Me.selSolutionIDs(-1)

    End Sub

    'Eine Lösung zum Optimierungsergebnis hinzufügen
    '***********************************************
    Public Sub addSolution(ByVal Ind As Common.Individuum)

        'Lösung zu OptResult hinzufügen
        ReDim Preserve Me.Solutions(Me.Solutions.GetUpperBound(0) + 1)
        Me.Solutions(Me.Solutions.GetUpperBound(0)) = Ind.Clone()

        'In DB speichern:
        Call Me.db_insert(Ind)

    End Sub

    'Eine Lösung identifizieren
    '**************************
    Public Function getSolution(ByVal ID As Integer) As Common.Individuum

        Dim i As Integer

        For i = 0 To Me.Solutions.GetUpperBound(0)
            If (Me.Solutions(i).ID = ID) Then
                Return Me.Solutions(i)
            End If
        Next

        Throw New Exception($"Unable to identify the solution with the ID {ID}!")

    End Function

    'Sekundäre Population hinzufügen
    '*******************************
    Public Sub setSekPop(ByVal pop() As Common.Individuum, ByVal _igen As Integer)

        Dim SekPop As Struct_SekPop
        Dim sekpopvalues(,) As Double

        'Population in Array von Penalty-Werten transformieren
        sekpopvalues = Common.Individuum.Get_All_Penalty_of_Array(pop)

        'SekPop in DB speichern
        Call Me.db_setSekPop(sekpopvalues, _igen)

        'SekPop aus DB lesen
        SekPop = Me.db_getSekPop(_igen)

        'SekPop zu OptResult hinzufügen
        Call Me.addSekPop(SekPop)

    End Sub

    'Sekundäre Population zu OptResult hinzufügen
    '********************************************
    Private Sub addSekPop(ByVal _sekpop As Struct_SekPop)

        'Array von Sekundären Populationen um eins erweitern
        ReDim Preserve Me.SekPops(Me.SekPops.GetUpperBound(0) + 1)
        'SekPop hinzufügen
        Me.SekPops(Me.SekPops.GetUpperBound(0)) = _sekpop

    End Sub

    'Sekundäre Population holen
    '**************************
    Public Function getSekPop(Optional ByVal _igen As Integer = -1) As Common.Individuum()

        Dim sekpopsolutions() As Common.Individuum

        'Wenn keine Generation angegeben, dann letzte SekPop ausgeben
        If (_igen = -1) Then
            For Each sekpop As Struct_SekPop In Me.SekPops
                If (sekpop.iGen > _igen) Then _igen = sekpop.iGen
            Next
        End If

        ReDim sekpopsolutions(-1)

        'Alle Sekundären Populationen durchlaufen
        For Each sekpop As Struct_SekPop In Me.SekPops
            If (sekpop.iGen = _igen) Then
                'SekPop gefunden, Lösungen holen
                sekpopsolutions = getSolutions(sekpop.SolutionIDs)
            End If
        Next

        Return sekpopsolutions

    End Function

    'Gibt die Penalty-Werte einer Sekundären Population zurück
    '*********************************************************
    Public Function getSekPopValues(Optional ByVal igen As Integer = -1) As Double(,)

        Dim inds() As Common.Individuum
        Dim values(,) As Double
        Dim i, j As Integer

        'Wenn keine Generation angegeben, dann letzte SekPop ausgeben
        If (igen = -1) Then
            igen = Me.db_getLastGenNo()
        End If

        'Wenn es keine Sekundäre Population in der DB gibt, abbrechen
        If (igen = -1) Then
            ReDim values(-1, -1)
            Return values
        End If

        inds = Me.getSekPop(igen)

        ReDim values(inds.GetUpperBound(0), Me.mProblem.NumPrimObjective - 1)

        For i = 0 To inds.GetUpperBound(0)
            For j = 0 To Me.mProblem.NumPrimObjective - 1
                values(i, j) = inds(i).PrimObjectives(j)
            Next
        Next

        Return values

    End Function

    'Lösungen anhand von IDs holen
    '*****************************
    Private Function getSolutions(ByVal IDs() As Integer) As Common.Individuum()

        Dim i As Integer
        Dim solutions() As Common.Individuum

        ReDim solutions(IDs.GetUpperBound(0))

        For i = 0 To solutions.GetUpperBound(0)
            solutions(i) = getSolution(IDs(i))
        Next

        Return solutions

    End Function

    'Beste Lösung zurückgeben
    '************************
    Public Function getBestSolution() As Common.Individuum

        Dim i As Integer
        Dim bestInd As Common.Individuum

        bestInd = Solutions(0)

        For i = 1 To Solutions.Length - 1
            If (Math.Abs(Solutions(i).PrimObjectives(0)) < Math.Abs(bestInd.PrimObjectives(0))) Then
                bestInd = Solutions(i)
            End If
        Next

        Return bestInd

    End Function

#End Region 'Ergebnisspeicher

#Region "Ergebnisdatenbank"

    'Methoden für die Ergebnisdatenbank
    '##################################

    ''' <summary>
    ''' Initialize the result database by copying the template and then initiating db_prepare()
    ''' </summary>
    ''' <param name="workdir">Directory to save the database in</param>
    ''' <param name="datasetname">Dataset name to use for the filename</param>
    ''' <param name="starttime">Start time to use for the filename</param>
    Private Sub db_init(workdir As String, datasetname As String, starttime As DateTime)

        'Ergebnisdatenbank anlegen
        '-------------------------

        'Datenbankpfad
        Dim filename As String = $"{datasetname}.BlueM.Opt.{starttime:yyyyMMddHHmm}.mdb"
        Me.db_path = IO.Path.Combine(workdir, filename)

        'Pfad zur Vorlage
        Dim db_source_path As String = IO.Path.Combine(System.Windows.Forms.Application.StartupPath(), "EVO.mdb")

        'Datei kopieren
        My.Computer.FileSystem.CopyFile(db_source_path, Me.db_path, True)

        'Tabellen anpassen
        '-----------------

        'Allgemeine Anpassungen
        Call Me.db_prepare()

        'Methodenspezifische Anpassungen
        Select Case Me.mProblem.Method
            Case Common.METH_PES, Common.METH_METAEVO, Common.METH_SENSIPLOT, Common.METH_HOOKEJEEVES, Common.METH_DDS
                Call Me.db_prepare_PES()
            Case Else
                Throw New NotImplementedException($"Method '{Me.mProblem.Method}' not implemented in OptResult.db_init()!")
        End Select

        Common.Log.AddMessage(Common.Log.levels.info, $"Initialized result database {filename}")

    End Sub

    'Ergebnisdatenbank vorbereiten
    '*****************************
    Private Sub db_prepare()

        'Tabellen anpassen
        '=================
        Dim fieldnames As List(Of String)

        Call db_connect()
        Dim command As OleDbCommand = New OleDbCommand("", db)

        'Tabelle 'QWerte'
        '----------------
        'Spalten festlegen:
        fieldnames = New List(Of String)
        For Each objfun As Common.ObjectiveFunction In Me.mProblem.List_ObjectiveFunctions
            fieldnames.Add($"[{objfun.Bezeichnung}] DOUBLE")
        Next
        'Tabelle anpassen
        command.CommandText = "ALTER TABLE QWerte ADD COLUMN " & String.Join(", ", fieldnames) & ";"
        command.ExecuteNonQuery()

        'Tabelle 'Constraints'
        '----------------
        If (Me.mProblem.NumConstraints > 0) Then
            'Spalten festlegen:
            fieldnames = New List(Of String)
            For Each constraint As Common.Constraintfunction In Me.mProblem.List_Constraintfunctions
                fieldnames.Add($"[{constraint.Bezeichnung}] DOUBLE")
            Next
            'Tabelle anpassen
            command.CommandText = "ALTER TABLE [Constraints] ADD COLUMN " & String.Join(", ", fieldnames) & ";"
            command.ExecuteNonQuery()
        End If

        Call db_disconnect()

    End Sub

    'Ergebnisdatenbank für PES vorbereiten
    '*************************************
    Private Sub db_prepare_PES()

        Call db_connect()
        Dim command As OleDbCommand = New OleDbCommand("", db)

        'Tabelle 'OptParameter'
        '----------------------
        'Spalten festlegen:
        Dim fieldnames As New List(Of String)
        For Each optpara As Common.OptParameter In Me.mProblem.List_OptParameter
            fieldnames.Add($"[{optpara.Bezeichnung}] DOUBLE")
        Next
        'Tabelle anpassen
        command.CommandText = "ALTER TABLE OptParameter ADD COLUMN " & String.Join(", ", fieldnames) & ";"
        command.ExecuteNonQuery()

        Call db_disconnect()

    End Sub

    'Mit Ergebnisdatenbank verbinden
    '*******************************
    Private Sub db_connect()
        Call db_connect(Me.db_path)
    End Sub

    'Mit einer benutzerdefinierten Ergebnisdatenbank verbinden
    '*********************************************************
    Private Sub db_connect(ByVal file As String)
        Try
            'Try using Microsoft.Jet.OLEDB.4.0
            Dim ConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & file
            db = New OleDb.OleDbConnection(ConnectionString)
            db.Open()
        Catch ex As Exception
            'fallback to Microsoft.ACE.OLEDB.12.0
            Dim ConnectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & file
            db = New OleDb.OleDbConnection(ConnectionString)
            db.Open()
        End Try
    End Sub

    'Verbindung zu Ergebnisdatenbank schließen
    '*****************************************
    Private Sub db_disconnect()
        db.Close()
    End Sub

    'Eine PES-Lösung in die ErgebnisDB schreiben
    '*******************************************
    Private Overloads Function db_insert(ByVal ind As Common.Individuum) As Boolean

        Call db_connect()

        Dim i As Integer
        Dim fieldnames As List(Of String)
        Dim fieldvalues As List(Of String)

        Dim command As OleDbCommand = New OleDbCommand("", db)

        'Sim schreiben
        '-------------
        command.CommandText = $"INSERT INTO Sim (ID, Name) VALUES ({ind.ID}, '{Me.Datensatz}');"
        command.ExecuteNonQuery()

        'QWerte schreiben 
        '----------------
        fieldnames = New List(Of String)
        fieldvalues = New List(Of String)
        For i = 0 To Me.mProblem.NumObjectives - 1
            fieldnames.Add($"[{Me.mProblem.List_ObjectiveFunctions(i).Bezeichnung}]")
            fieldvalues.Add(ind.Objectives(i).ToString(Common.Provider.FortranProvider))
        Next
        command.CommandText = "INSERT INTO QWerte (Sim_ID, " & String.Join(", ", fieldnames) & $") VALUES ({ind.ID}, " & String.Join(", ", fieldvalues) & ");"
        command.ExecuteNonQuery()

        'Constraints schreiben 
        '---------------------
        If (Me.mProblem.NumConstraints > 0) Then
            fieldnames = New List(Of String)
            fieldvalues = New List(Of String)
            For i = 0 To Me.mProblem.NumConstraints - 1
                fieldnames.Add($"[{Me.mProblem.List_Constraintfunctions(i).Bezeichnung}]")
                fieldvalues.Add(ind.Constraints(i).ToString(Common.Provider.FortranProvider))
            Next
            command.CommandText = "INSERT INTO [Constraints] (Sim_ID, " & String.Join(", ", fieldnames) & $") VALUES ({ind.ID}, " & String.Join(", ", fieldvalues) & ");"
            command.ExecuteNonQuery()
        End If

        'OptParameter schreiben
        '----------------------
        fieldnames = New List(Of String)
        fieldvalues = New List(Of String)
        For i = 0 To Me.mProblem.List_OptParameter.GetUpperBound(0)
            fieldnames.Add($"[{Me.mProblem.List_OptParameter(i).Bezeichnung}]")
            fieldvalues.Add(ind.OptParameter(i).RWert.ToString(Common.Provider.FortranProvider))
        Next
        command.CommandText = "INSERT INTO OptParameter (Sim_ID, " & String.Join(", ", fieldnames) & $") VALUES ({ind.ID}, " & String.Join(", ", fieldvalues) & ");"
        command.ExecuteNonQuery()

        Call db_disconnect()

    End Function

    'Sekundäre Population in DB speichern
    '************************************
    Private Sub db_setSekPop(ByVal SekPop(,) As Double, ByVal igen As Integer)

        Call db_connect()

        Dim command As OleDbCommand = New OleDbCommand("", db)

        ''Alte SekPop löschen
        'command.CommandText = "DELETE FROM SekPop"
        'command.ExecuteNonQuery()

        'Neue SekPop speichern
        Dim i, j As Integer
        Dim bedingung As String
        Dim Sim_ID As Integer
        For i = 0 To SekPop.GetUpperBound(0)

            'zugehörige Sim_ID bestimmen
            bedingung = ""
            For j = 0 To Me.mProblem.NumPrimObjective - 1
                bedingung &= $" AND QWerte.[{Me.mProblem.List_PrimObjectiveFunctions(j).Bezeichnung}] = " & SekPop(i, j).ToString(Common.Provider.FortranProvider)
            Next
            command.CommandText = $"SELECT Sim.ID FROM Sim INNER JOIN QWerte ON Sim.ID = QWerte.Sim_ID WHERE (1=1{bedingung});"
            Sim_ID = command.ExecuteScalar()

            If (Sim_ID > 0) Then
                'SekPop Member speichern
                command.CommandText = $"INSERT INTO SekPop (Generation, Sim_ID) VALUES ({igen}, {Sim_ID});"
                command.ExecuteNonQuery()
            End If
        Next

        Call db_disconnect()

    End Sub

    'SekPop aus DB lesen
    '*******************
    Private Function db_getSekPop(ByVal iGen As Integer) As Struct_SekPop

        Dim i As Integer
        Dim q As String
        Dim adapter As OleDbDataAdapter
        Dim ds As DataSet
        Dim numrows As Integer
        Dim SekPop As Struct_SekPop

        Call db_connect()

        q = "SELECT Sim_ID FROM SekPop WHERE Generation = " & iGen

        adapter = New OleDbDataAdapter(q, db)

        ds = New DataSet("EVO")
        numrows = adapter.Fill(ds, "SekPop")

        Call db_disconnect()

        If (numrows > 0) Then

            SekPop.iGen = iGen
            ReDim SekPop.SolutionIDs(numrows - 1)

            For i = 0 To numrows - 1
                SekPop.SolutionIDs(i) = ds.Tables("SekPop").Rows(i).Item("Sim_ID")
            Next

        Else
            Throw New Exception($"Secondary population of generation {iGen} not found in database!")
        End If

        Return SekPop

    End Function

    'Letzte Generation bestimmen
    '***************************
    Private Function db_getLastGenNo() As Integer

        Dim command As OleDbCommand
        Dim igen As Integer

        'Connect
        Call db_connect()

        Try
            command = New OleDbCommand("", db)
            command.CommandText = "SELECT MAX(Generation) FROM SekPop"
            igen = command.ExecuteScalar()
        Catch ex As Exception
            'Keine SekPop vorhanden
            igen = -1
        End Try

        'Disconnect
        Call db_disconnect()

        Return igen

    End Function

    ''' <summary>
    ''' Optimierungsergebnis aus einer DB einlesen
    ''' </summary>
    ''' <param name="sourceFile">Pfad zur mdb-Datei</param>
    ''' <param name="loadOptParameters">Ob auch die OptParameter-Werte eingelesen werden sollen</param>
    ''' <returns>True if successful</returns>
    ''' <remarks>
    ''' Das Optimierungsproblem (d.h. ObjectiveFunctions, OptParameter, Constraints), 
    ''' ebenso wie die Methode, müssen mit der DB übereinstimmen!
    ''' </remarks>
    Public Function db_load(ByVal sourceFile As String, Optional ByVal loadOptParameters As Boolean = True) As Boolean

        'Optparameter gewünscht?
        Me.holdsOptparameters = loadOptParameters

        Try

            'Neuen Dateipfad speichern
            Me.db_path = sourceFile

            Select Case Me.mProblem.Method
                Case Common.METH_PES, Common.METH_HOOKEJEEVES, Common.METH_SENSIPLOT, Common.METH_METAEVO
                    Call db_getIndividuen_PES()
                Case Else
                    Throw New NotImplementedException($"Method '{Me.mProblem.Method}' not implemented in OptResult.db_load()!")
            End Select

            'Sekundärpopulationen laden
            Call Me.db_loadSekPops()

            Return True

        Catch ex As Exception
            MsgBox("Failed to load optimization result!" & Common.eol & ex.Message, MsgBoxStyle.Critical)
            Return False
        End Try

    End Function

    'Alle Lösungen aus der DB als PES-Individuen einlesen
    '****************************************************
    Private Sub db_getIndividuen_PES()

        Dim i, j As Integer
        Dim numSolutions As Integer
        Dim q As String = ""
        Dim adapter As OleDbDataAdapter
        Dim ds As DataSet

        'Connect
        Call db_connect()

        'Alle Lösungen aus DB lesen
        '--------------------------
        If (Me.holdsOptparameters) Then
            'mit OptParameter
            q = "SELECT Sim.ID, OptParameter.*, QWerte.*, Constraints.* FROM ((Sim LEFT JOIN [Constraints] ON Sim.ID=Constraints.Sim_ID) INNER JOIN OptParameter ON Sim.ID=OptParameter.Sim_ID) INNER JOIN QWerte ON Sim.ID=QWerte.Sim_ID ORDER BY Sim.ID"
        Else
            'ohne OptParameter
            q = "SELECT Sim.ID, QWerte.*, Constraints.* FROM (Sim LEFT JOIN [Constraints] ON Sim.ID=Constraints.Sim_ID) INNER JOIN QWerte ON Sim.ID=QWerte.Sim_ID ORDER BY Sim.ID"
        End If

        adapter = New OleDbDataAdapter(q, db)

        ds = New DataSet("EVO")
        numSolutions = adapter.Fill(ds, "Result")

        'Disconnect
        Call db_disconnect()

        'Alle Lösungen als Individuen übernehmen
        '---------------------------------------
        ReDim Me.Solutions(numSolutions - 1)

        For i = 0 To numSolutions - 1

            Me.Solutions(i) = New Common.Individuum_PES("Solution", i)

            With CType(Me.Solutions(i), Common.Individuum_PES)
                'ID
                '--
                .ID = ds.Tables(0).Rows(i).Item("Sim.ID")

                If (Me.holdsOptparameters) Then
                    'OptParameter
                    '------------
                    For j = 0 To Me.mProblem.NumOptParams - 1
                        .OptParameter(j).RWert = ds.Tables(0).Rows(i).Item(Me.mProblem.List_OptParameter(j).Bezeichnung)
                    Next
                End If

                'Constraints
                '-----------
                For j = 0 To Me.mProblem.NumConstraints - 1
                    .Constraints(j) = ds.Tables(0).Rows(i).Item(Me.mProblem.List_Constraintfunctions(j).Bezeichnung)
                Next

                'Features
                '--------
                For j = 0 To Me.mProblem.NumObjectives - 1
                    .Objectives(j) = ds.Tables(0).Rows(i).Item(Me.mProblem.List_ObjectiveFunctions(j).Bezeichnung)
                Next

            End With

        Next

    End Sub

    'Sekundärpopulationen aus DB laden
    '*********************************
    Private Sub db_loadSekPops()

        Dim i, igen As Integer
        Dim SekPop As Struct_SekPop

        ReDim Me.SekPops(-1)

        'Letzte SekPop-Generation bestimmen
        igen = db_getLastGenNo()

        'Alle SekPops durchlaufen und einlesen
        For i = 0 To igen
            SekPop = db_getSekPop(i)
            Call Me.addSekPop(SekPop)
        Next

    End Sub

#End Region 'Ergebnisdatenbank

End Class
