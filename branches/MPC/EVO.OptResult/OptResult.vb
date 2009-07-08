Imports System.Data.OleDb

Public Class OptResult

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse OptResult                                                      ****
    '****                                                                       ****
    '**** Speichert und verwaltet die Ergebnisse eines Optimierungslaufs,       ****
    '**** schreibt die Ergebnisse in eine Datenbank                             ****
    '****                                                                       ****
    '**** Autoren: Felix Froehlich, Christoph H�bner                            ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

    'Allgemeine Einstellungen
    Private Datensatz As String

    'Das Problem
    Private mProblem As EVO.Common.Problem

    'Ergebnisdatenbank
    Public Ergebnisdb As Boolean = True              'Gibt an, ob die Ergebnisdatenbank geschrieben werden soll
    Private db_path As String                        'Pfad zur Ergebnisdatenbank
    Private db As OleDb.OleDbConnection

    'Array von L�sungen
    Public Solutions() As Common.Individuum

    'Structure f�r Sekund�re Population
    Public Structure Struct_SekPop
        Public iGen As Integer                      'Generationsnummer
        Public SolutionIDs() As Integer             'Array von Solution-IDs
    End Structure

    'Array von Sekund�ren Populationen
    Public SekPops() As Struct_SekPop

    'Array von ausgew�hlten L�sungen
    Private selSolutionIDs() As Integer

    'Konstruktor
    '***********
    Public Sub New(ByVal Datensatzname As String, ByRef prob As EVO.Common.Problem, Optional ByVal createNewMdb As Boolean = True)

        'Datensatzname speichern
        Me.Datensatz = Datensatzname

        'Problem speichern
        Me.mProblem = prob

        ReDim Me.Solutions(-1)
        ReDim Me.selSolutionIDs(-1)
        ReDim Me.SekPops(-1)

        If (createNewMdb) Then
            'DB initialiseren
            Call Me.db_init()
        End If

    End Sub

#Region "Ergebnisspeicher"

    'Eine L�sung ausw�hlen
    '*********************
    Public Function selectSolution(ByVal ID As Integer) As Boolean

        '�berpr�fen, ob L�sung bereits ausgew�hlt ist      
        For Each _id As Integer In Me.selSolutionIDs
            If (_id = ID) Then
                Return False
            End If
        Next

        'L�sung zu Auswahl hinzuf�gen
        ReDim Preserve Me.selSolutionIDs(Me.selSolutionIDs.GetUpperBound(0) + 1)
        Me.selSolutionIDs(Me.selSolutionIDs.GetUpperBound(0)) = ID

        Return True

    End Function

    'Ausgew�hlte L�sungen holen
    '**************************
    Public ReadOnly Property getSelectedSolutions() As Common.Individuum()
        Get
            Dim solutions() As Common.Individuum

            solutions = getSolutions(Me.selSolutionIDs)

            Return solutions
        End Get
    End Property

    'L�sungsauswahl zur�cksetzen
    '***************************
    Public Sub clearSelectedSolutions()

        ReDim Me.selSolutionIDs(-1)

    End Sub

    'Eine L�sung zum Optimierungsergebnis hinzuf�gen
    '***********************************************
    Public Sub addSolution(ByVal Ind As Common.Individuum)

        'L�sung zu OptResult hinzuf�gen
        ReDim Preserve Me.Solutions(Me.Solutions.GetUpperBound(0) + 1)
        Me.Solutions(Me.Solutions.GetUpperBound(0)) = Ind.Clone()

        'In DB speichern:
        'Sonderfall f�r Individuum_CES:
        If (TypeOf (Ind) Is Common.Individuum_CES) Then
            Call Me.db_insert(CType(Ind, Common.Individuum_CES))
        Else
            Call Me.db_insert(Ind)
        End If

    End Sub

    'Eine L�sung identifizieren
    '**************************
    Public Function getSolution(ByVal ID As Integer) As Common.Individuum

        Dim i As Integer

        For i = 0 To Me.Solutions.GetUpperBound(0)
            If (Me.Solutions(i).ID = ID) Then
                Return Me.Solutions(i)
            End If
        Next

        Throw New Exception("Konnte L�sung nicht identifizieren!")

    End Function

    'Sekund�re Population hinzuf�gen
    '*******************************
    Public Sub setSekPop(ByVal pop() As EVO.Common.Individuum, ByVal _igen As Integer)

        Dim SekPop As Struct_SekPop
        Dim sekpopvalues(,) As Double

        'Population in Array von Penalty-Werten transformieren
        sekpopvalues = Common.Individuum.Get_All_Penalty_of_Array(pop)

        'SekPop in DB speichern
        Call Me.db_setSekPop(sekpopvalues, _igen)

        'SekPop aus DB lesen
        SekPop = Me.db_getSekPop(_igen)

        'SekPop zu OptResult hinzuf�gen
        Call Me.addSekPop(SekPop)

    End Sub

    'Sekund�re Population zu OptResult hinzuf�gen
    '********************************************
    Private Sub addSekPop(ByVal _sekpop As Struct_SekPop)

        'Array von Sekund�ren Populationen um eins erweitern
        ReDim Preserve Me.SekPops(Me.SekPops.GetUpperBound(0) + 1)
        'SekPop hinzuf�gen
        Me.SekPops(Me.SekPops.GetUpperBound(0)) = _sekpop

    End Sub

    'Sekund�re Population holen
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

        'Alle Sekund�ren Populationen durchlaufen
        For Each sekpop As Struct_SekPop In Me.SekPops
            If (sekpop.iGen = _igen) Then
                'SekPop gefunden, L�sungen holen
                sekpopsolutions = getSolutions(sekpop.SolutionIDs)
            End If
        Next

        Return sekpopsolutions

    End Function

    'Gibt die Penalty-Werte einer Sekund�ren Population zur�ck
    '*********************************************************
    Public Function getSekPopValues(Optional ByVal igen As Integer = -1) As Double(,)

        Dim inds() As Common.Individuum
        Dim values(,) As Double
        Dim i, j As Integer

        'Wenn keine Generation angegeben, dann letzte SekPop ausgeben
        If (igen = -1) Then
            igen = Me.db_getLastGenNo()
        End If

        'Wenn es keine Sekund�re Population in der DB gibt, abbrechen
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

    'L�sungen anhand von IDs holen
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

    'Beste L�sung zur�ckgeben
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

    'Methoden f�r die Ergebnisdatenbank
    '##################################

    'Datenbank vorbereiten
    '*********************
    Private Sub db_init()

        'Ergebnisdatenbank in tempor�rem Verzeichnis anlegen
        '---------------------------------------------------

        'Datenbankpfad
        Me.db_path = My.Computer.FileSystem.SpecialDirectories.Temp & "\EVO.mdb"

        'Pfad zur Vorlage
        Dim db_source_path As String = System.Windows.Forms.Application.StartupPath() & "\EVO.mdb"

        'Datei kopieren
        My.Computer.FileSystem.CopyFile(db_source_path, Me.db_path, True)

        'Tabellen anpassen
        '-----------------

        'Allgemeine Anpassungen
        Call Me.db_prepare()
        'Methodenspezifische Anpassungen
        Select Case Me.mProblem.Method
            Case EVO.Common.METH_PES, EVO.Common.METH_MetaEvo, EVO.Common.METH_SENSIPLOT, EVO.Common.METH_HOOKJEEVES, EVO.Common.METH_DDS
                Call Me.db_prepare_PES()
            Case EVO.Common.METH_CES
                Call Me.db_prepare_CES()
            Case EVO.Common.METH_HYBRID
                Call Me.db_prepare_PES()
                Call Me.db_prepare_CES()
        End Select

    End Sub

    'Ergebnisdatenbank vorbereiten
    '*****************************
    Private Sub db_prepare()

        'Tabellen anpassen
        '=================
        Dim i As Integer

        Call db_connect()
        Dim command As OleDbCommand = New OleDbCommand("", db)

        'Tabelle 'QWerte'
        '----------------
        'Spalten festlegen:
        Dim fieldnames As String = ""
        For i = 0 To Me.mProblem.NumObjectives - 1
            If (i > 0) Then
                fieldnames &= ", "
            End If
            fieldnames &= "[" & Me.mProblem.List_ObjectiveFunctions(i).Bezeichnung & "] DOUBLE"
        Next
        'Tabelle anpassen
        command.CommandText = "ALTER TABLE QWerte ADD COLUMN " & fieldnames
        command.ExecuteNonQuery()

        'Tabelle 'Constraints'
        '----------------
        If (Me.mProblem.NumConstraints > 0) Then
            'Spalten festlegen:
            fieldnames = ""
            For i = 0 To Me.mProblem.NumConstraints - 1
                If (i > 0) Then
                    fieldnames &= ", "
                End If
                fieldnames &= "[" & Me.mProblem.List_Constraintfunctions(i).Bezeichnung & "] DOUBLE"
            Next
            'Tabelle anpassen
            command.CommandText = "ALTER TABLE [Constraints] ADD COLUMN " & fieldnames
            command.ExecuteNonQuery()
        End If

        Call db_disconnect()

    End Sub

    'Ergebnisdatenbank f�r PES vorbereiten
    '*************************************
    Private Sub db_prepare_PES()

        Call db_connect()
        Dim command As OleDbCommand = New OleDbCommand("", db)

        'Tabelle 'OptParameter'
        '----------------------
        'Spalten festlegen:
        Dim fieldnames As String = ""
        Dim i As Integer

        For i = 0 To Me.mProblem.List_OptParameter.GetUpperBound(0)
            If (i > 0) Then
                fieldnames &= ", "
            End If
            fieldnames &= "[" & Me.mProblem.List_OptParameter(i).Bezeichnung & "] DOUBLE"
        Next
        'Tabelle anpassen
        command.CommandText = "ALTER TABLE OptParameter ADD COLUMN " & fieldnames
        command.ExecuteNonQuery()

        Call db_disconnect()

    End Sub

    'Ergebnisdatenbank f�r CES vorbereiten
    '*************************************
    Private Sub db_prepare_CES()

        Call db_connect()
        Dim command As OleDbCommand = New OleDbCommand("", db)

        'Tabelle 'Pfad'
        '--------------
        'Spalten festlegen:
        Dim fieldnames As String = ""
        Dim i As Integer

        For i = 0 To Me.mProblem.List_Locations.GetUpperBound(0)
            If (i > 0) Then
                fieldnames &= ", "
            End If
            fieldnames &= "[" & Me.mProblem.List_Locations(i).Name & "] TEXT"
        Next
        'Tabelle anpassen
        command.CommandText = "ALTER TABLE Pfad ADD COLUMN " & fieldnames
        command.ExecuteNonQuery()

        Call db_disconnect()

    End Sub

    'Mit Ergebnisdatenbank verbinden
    '*******************************
    Private Sub db_connect()
        Dim ConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Me.db_path
        db = New OleDb.OleDbConnection(ConnectionString)
        db.Open()
    End Sub

    'Mit einer benutzerdefinierten Ergebnisdatenbank verbinden
    '*********************************************************
    Private Sub db_connect(ByVal file As String)
        Dim ConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & file
        db = New OleDb.OleDbConnection(ConnectionString)
        db.Open()
    End Sub

    'Verbindung zu Ergebnisdatenbank schlie�en
    '*****************************************
    Private Sub db_disconnect()
        db.Close()
    End Sub

    'Eine PES-L�sung in die ErgebnisDB schreiben
    '*******************************************
    Private Overloads Function db_insert(ByVal ind As Common.Individuum) As Boolean

        Call db_connect()

        Dim i As Integer

        Dim command As OleDbCommand = New OleDbCommand("", db)

        'Sim schreiben
        '-------------
        command.CommandText = "INSERT INTO Sim (ID, Name) VALUES (" & ind.ID & ", '" & Me.Datensatz & "')"
        command.ExecuteNonQuery()

        'QWerte schreiben 
        '----------------
        Dim fieldnames As String = ""
        Dim fieldvalues As String = ""
        For i = 0 To Me.mProblem.NumObjectives - 1
            fieldnames &= ", [" & Me.mProblem.List_ObjectiveFunctions(i).Bezeichnung & "]"
            fieldvalues &= ", " & ind.Objectives(i).ToString(Common.Provider.FortranProvider)
        Next
        command.CommandText = "INSERT INTO QWerte (Sim_ID" & fieldnames & ") VALUES (" & ind.ID & fieldvalues & ")"
        command.ExecuteNonQuery()

        'Constraints schreiben 
        '---------------------
        If (Me.mProblem.NumConstraints > 0) Then
            fieldnames = ""
            fieldvalues = ""
            For i = 0 To Me.mProblem.NumConstraints - 1
                fieldnames &= ", [" & Me.mProblem.List_Constraintfunctions(i).Bezeichnung & "]"
                fieldvalues &= ", " & ind.Constraints(i).ToString(Common.Provider.FortranProvider)
            Next
            command.CommandText = "INSERT INTO [Constraints] (Sim_ID" & fieldnames & ") VALUES (" & ind.ID & fieldvalues & ")"
            command.ExecuteNonQuery()
        End If

        'OptParameter schreiben
        '----------------------
        fieldnames = ""
        fieldvalues = ""
        For i = 0 To Me.mProblem.List_OptParameter.GetUpperBound(0)
            fieldnames &= ", [" & Me.mProblem.List_OptParameter(i).Bezeichnung & "]"
            fieldvalues &= ", " & ind.OptParameter(i).RWert.ToString(Common.Provider.FortranProvider)
        Next
        command.CommandText = "INSERT INTO OptParameter (Sim_ID" & fieldnames & ") VALUES (" & ind.ID & fieldvalues & ")"
        command.ExecuteNonQuery()

        Call db_disconnect()

    End Function

    'Eine CES/Hybrid-L�sung in die ErgebnisDB schreiben
    '**************************************************
    Private Overloads Function db_insert(ByVal ind As Common.Individuum_CES) As Boolean

        Call db_connect()

        Dim i, x, y As Integer

        Dim command As OleDbCommand = New OleDbCommand("", db)

        'Sim schreiben
        '-------------
        command.CommandText = "INSERT INTO Sim (ID, Name) VALUES (" & ind.ID & ", '" & Me.Datensatz & "')"
        command.ExecuteNonQuery()

        'QWerte schreiben 
        '----------------
        Dim fieldnames As String = ""
        Dim fieldvalues As String = ""
        For i = 0 To Me.mProblem.NumObjectives - 1
            fieldnames &= ", [" & Me.mProblem.List_ObjectiveFunctions(i).Bezeichnung & "]"
            fieldvalues &= ", " & ind.Objectives(i).ToString(Common.Provider.FortranProvider)
        Next
        command.CommandText = "INSERT INTO QWerte (Sim_ID" & fieldnames & ") VALUES (" & ind.ID & fieldvalues & ")"
        command.ExecuteNonQuery()

        'Constraints schreiben 
        '---------------------
        If (Me.mProblem.NumConstraints > 0) Then
            fieldnames = ""
            fieldvalues = ""
            For i = 0 To Me.mProblem.NumConstraints - 1
                fieldnames &= ", [" & Me.mProblem.List_Constraintfunctions(i).Bezeichnung & "]"
                fieldvalues &= ", " & ind.Constraints(i).ToString(Common.Provider.FortranProvider)
            Next
            command.CommandText = "INSERT INTO [Constraints] (Sim_ID" & fieldnames & ") VALUES (" & ind.ID & fieldvalues & ")"
            command.ExecuteNonQuery()
        End If

        'Pfad schreiben
        '--------------
        fieldnames = ""
        fieldvalues = ""
        For i = 0 To Me.mProblem.List_Locations.GetUpperBound(0)
            fieldnames &= ", [" & Me.mProblem.List_Locations(i).Name & "]"
            fieldvalues &= ", '" & ind.Measures(i) & "'"
        Next
        command.CommandText = "INSERT INTO Pfad (Sim_ID" & fieldnames & ") VALUES (" & ind.ID & fieldvalues & ")"
        command.ExecuteNonQuery()

        If (Me.mProblem.Method = EVO.Common.METH_HYBRID) Then

            Dim found As Boolean

            'OptParameter schreiben
            '----------------------
            fieldnames = ""
            fieldvalues = ""
            For i = 0 To Me.mProblem.List_OptParameter_Save.GetUpperBound(0)
                found = False
                fieldnames &= ", [" & Me.mProblem.List_OptParameter_Save(i).Bezeichnung & "]"
                For x = 0 To ind.Loc.GetUpperBound(0)
                    For y = 0 To ind.Loc(x).PES_OptPara.GetUpperBound(0)
                        If ind.Loc(x).PES_OptPara(y).Bezeichnung = Me.mProblem.List_OptParameter_Save(i).Bezeichnung Then
                            fieldvalues &= ", " & ind.Loc(x).PES_OptPara(y).RWert.ToString(Common.Provider.FortranProvider)
                            found = True
                        End If
                    Next
                Next
                If found = False Then
                    fieldvalues &= ", " & "-7"
                End If
            Next
            command.CommandText = "INSERT INTO OptParameter (Sim_ID" & fieldnames & ") VALUES (" & ind.ID & fieldvalues & ")"
            command.ExecuteNonQuery()

        End If

        Call db_disconnect()

    End Function

    'Sekund�re Population in DB speichern
    '************************************
    Private Sub db_setSekPop(ByVal SekPop(,) As Double, ByVal igen As Integer)

        Call db_connect()

        Dim command As OleDbCommand = New OleDbCommand("", db)

        ''Alte SekPop l�schen
        'command.CommandText = "DELETE FROM SekPop"
        'command.ExecuteNonQuery()

        'Neue SekPop speichern
        Dim i, j As Integer
        Dim bedingung As String
        Dim Sim_ID As Integer
        For i = 0 To SekPop.GetUpperBound(0)

            'zugeh�rige Sim_ID bestimmen
            bedingung = ""
            For j = 0 To Me.mProblem.NumPrimObjective - 1
                bedingung &= " AND QWerte.[" & Me.mProblem.List_PrimObjectiveFunctions(j).Bezeichnung & "] = " & SekPop(i, j).ToString(Common.Provider.FortranProvider)
            Next
            command.CommandText = "SELECT Sim.ID FROM Sim INNER JOIN QWerte ON Sim.ID = QWerte.Sim_ID WHERE (1=1" & bedingung & ")"
            Sim_ID = command.ExecuteScalar()

            If (Sim_ID > 0) Then
                'SekPop Member speichern
                command.CommandText = "INSERT INTO SekPop (Generation, Sim_ID) VALUES (" & igen & ", " & Sim_ID & ")"
                command.ExecuteNonQuery()
            End If
        Next

        Call db_disconnect()

    End Sub

    'SekPop aus DB lesen
    '*******************
    Private Function db_getSekPop(ByVal igen As Integer) As Struct_SekPop

        Dim i As Integer
        Dim q As String
        Dim adapter As OleDbDataAdapter
        Dim ds As DataSet
        Dim numrows As Integer
        Dim SekPop As Struct_SekPop

        Call db_connect()

        q = "SELECT Sim_ID FROM SekPop WHERE Generation = " & igen

        adapter = New OleDbDataAdapter(q, db)

        ds = New DataSet("EVO")
        numrows = adapter.Fill(ds, "SekPop")

        Call db_disconnect()

        If (numrows > 0) Then

            SekPop.iGen = igen
            ReDim SekPop.SolutionIDs(numrows - 1)

            For i = 0 To numrows - 1
                SekPop.SolutionIDs(i) = ds.Tables("SekPop").Rows(i).Item("Sim_ID")
            Next

        Else
            Throw New Exception("Sekund�re Population nicht in DB vorhanden!")
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

    'Ergebnisdatenbank abspeichern (kopieren)
    '****************************************
    Public Sub db_save(ByVal targetFile As String)

        'Datei kopieren
        My.Computer.FileSystem.CopyFile(Me.db_path, targetFile, True)

        'Neuen Dateipfad speichern
        Me.db_path = targetFile

    End Sub

    ''' <summary>
    ''' Optimierungsergebnis aus einer DB einlesen
    ''' </summary>
    ''' <param name="sourceFile">Pfad zur mdb-Datei</param>
    ''' <returns>True if successful</returns>
    ''' <remarks>
    ''' Das Optimierungsproblem (d.h. Feature Functions, OptParameter, Constraints), 
    ''' ebenso wie die Methode, m�ssen mit der DB �bereinstimmen!
    ''' </remarks>
    Public Function db_load(ByVal sourceFile As String) As Boolean

        Try

            'Neuen Dateipfad speichern
            Me.db_path = sourceFile

            Select Case Me.mProblem.Method
                Case EVO.Common.METH_PES, EVO.Common.METH_HOOKJEEVES, EVO.Common.METH_SENSIPLOT, EVO.Common.METH_MetaEvo
                    Call db_getIndividuen_PES()

                Case EVO.Common.METH_CES, EVO.Common.METH_HYBRID
                    Call db_getIndividuen_CES()

                Case Else
                    Throw New Exception("OptResult.db_load() f�r Methode '" & Me.mProblem.Method & "' noch nicht implementiert!")
            End Select

            'Sekund�rpopulationen laden
            Call Me.db_loadSekPops()

            Return True

        Catch ex As Exception
            MsgBox("Laden des Optimierungsergebnisses fehlgeschlagen!" & EVO.Common.eol & ex.Message, MsgBoxStyle.Critical)
            Return False
        End Try

    End Function

    'Alle L�sungen aus der DB als PES-Individuen einlesen
    '****************************************************
    Private Sub db_getIndividuen_PES()

        Dim i, j As Integer
        Dim numSolutions As Integer
        Dim q As String = ""
        Dim adapter As OleDbDataAdapter
        Dim ds As DataSet

        'Connect
        Call db_connect()

        'Alle L�sungen aus DB lesen
        '--------------------------
        q = "SELECT Sim.ID, OptParameter.*, QWerte.*, Constraints.* FROM ((Sim LEFT JOIN [Constraints] ON Sim.ID=Constraints.Sim_ID) INNER JOIN OptParameter ON Sim.ID=OptParameter.Sim_ID) INNER JOIN QWerte ON Sim.ID=QWerte.Sim_ID ORDER BY Sim.ID"

        adapter = New OleDbDataAdapter(q, db)

        ds = New DataSet("EVO")
        numSolutions = adapter.Fill(ds, "Result")

        'Disconnect
        Call db_disconnect()

        'Alle L�sungen als Individuen �bernehmen
        '---------------------------------------
        ReDim Me.Solutions(numSolutions - 1)

        For i = 0 To numSolutions - 1

            Me.Solutions(i) = New Common.Individuum_PES("Solution", i)

            With CType(Me.Solutions(i), Common.Individuum_PES)
                'ID
                '--
                .ID = ds.Tables(0).Rows(i).Item("Sim.ID")

                'OptParameter
                '------------
                For j = 0 To Me.mProblem.NumOptParams - 1
                    .OptParameter(j).RWert = ds.Tables(0).Rows(i).Item(Me.mProblem.List_OptParameter_Save(j).Bezeichnung)
                Next

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

    'Alle L�sungen aus der DB als CES-Individuen einlesen
    '****************************************************
    Private Sub db_getIndividuen_CES()

        Dim i, j As Integer
        Dim numSolutions As Integer
        Dim q As String = ""
        Dim adapter As OleDbDataAdapter
        Dim ds As DataSet

        'Connect
        Call db_connect()

        'Alle L�sungen aus DB lesen
        '--------------------------
        Select Case Me.mProblem.Method
            Case EVO.Common.METH_CES
                q = "SELECT Sim.ID, Pfad.*, QWerte.*, Constraints.* FROM ((Sim LEFT JOIN [Constraints] ON Sim.ID=Constraints.Sim_ID) INNER JOIN Pfad ON Sim.ID=Pfad.Sim_ID) INNER JOIN QWerte ON Sim.ID=QWerte.Sim_ID"
            Case EVO.Common.METH_HYBRID
                q = "SELECT Sim.ID, Pfad.*, OptParameter.*, QWerte.*, Constraints.* FROM (((Sim LEFT JOIN [Constraints] ON Sim.ID=Constraints.Sim_ID) INNER JOIN Pfad ON Sim.ID=Pfad.Sim_ID) INNER JOIN OptParameter ON Sim.ID=OptParameter.Sim_ID) INNER JOIN QWerte ON Sim.ID=QWerte.Sim_ID ORDER BY Sim.ID"
        End Select

        adapter = New OleDbDataAdapter(q, db)

        ds = New DataSet("EVO")
        numSolutions = adapter.Fill(ds, "Result")

        'Disconnect
        Call db_disconnect()

        'Alle L�sungen als Individuen �bernehmen
        '---------------------------------------
        ReDim Me.Solutions(numSolutions - 1)

        For i = 0 To numSolutions - 1

            Me.Solutions(i) = New Common.Individuum_CES("Solution", i)

            With CType(Me.Solutions(i), Common.Individuum_CES)
                'ID
                '--
                .ID = ds.Tables(0).Rows(i).Item("Sim.ID")

                'OptParameter
                '------------
                'TODO: OptParameter aus DB in Individuum_CES einlesen

                'Constraints
                '-----------
                For j = 0 To Me.mProblem.NumConstraints - 1
                    .Constraints(j) = ds.Tables(0).Rows(i).Item(Me.mProblem.List_Constraintfunctions(j).Bezeichnung)
                Next

                'Pfad
                '----
                ReDim .Measures(Me.mProblem.List_Locations.GetUpperBound(0))
                For j = 0 To Me.mProblem.List_Locations.GetUpperBound(0)
                    .Measures(j) = ds.Tables(0).Rows(i).Item(Me.mProblem.List_Locations(j).Name)
                Next

                'Features
                '--------
                For j = 0 To Me.mProblem.NumObjectives - 1
                    .Objectives(j) = ds.Tables(0).Rows(i).Item(Me.mProblem.List_ObjectiveFunctions(j).Bezeichnung)
                Next

            End With

        Next

    End Sub

    'Sekund�rpopulationen aus DB laden
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