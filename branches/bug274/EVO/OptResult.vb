Imports System.Data.OleDb

Public Class OptResult

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse OptResult                                                      ****
    '****                                                                       ****
    '**** Speichert und verwaltet die Ergebnisse eines Optimierungslaufs,       ****
    '**** schreibt die Ergebnisse in eine Datenbank                             ****
    '****                                                                       ****
    '**** Autoren: Felix Froehlich                                              ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

    'Allgemeine Einstellungen
    Private Datensatz As String

    'Ergebnisdatenbank
    Public Ergebnisdb As Boolean = True              'Gibt an, ob die Ergebnisdatenbank geschrieben werden soll
    Private db_path As String                        'Pfad zur Ergebnisdatenbank
    Private db As OleDb.OleDbConnection

    'Optimierungsbedingungen
    Public List_OptZiele() As Sim.Struct_OptZiel
    Public List_OptParameter() As EVO.Kern.OptParameter
    Public List_OptParameter_Save() As EVO.Kern.OptParameter
    Public List_Constraints() As Sim.Struct_Constraint
    Public List_Locations()As Sim.Struct_Lokation

    'Array von Lösungen
    Public Solutions() As Kern.Individuum

    'Structure für Sekundäre Population
    Public Structure Struct_SekPop
        Public iGen As Integer                      'Generationsnummer
        Public SolutionIDs() As Integer             'Array von Solution-IDs
    End Structure

    'Array von Sekundären Populationen
    Private SekPops() As Struct_SekPop

    'Array von ausgewählten Lösungen
    Private selSolutionIDs() As Integer

    'Konstruktor
    '***********
    Public Sub New(ByVal Sim1 As Sim)

        'Datensatzname speichern
        Me.Datensatz = Sim1.Datensatz

        'Optimierungsbedingungen kopieren
        Me.List_OptZiele = Sim1.List_OptZiele
        Me.List_OptParameter = Sim1.List_OptParameter
        Me.List_OptParameter_Save = sim1.List_OptParameter_Save
        Me.List_Constraints = Sim1.List_Constraints
        Me.List_Locations = Sim1.List_Locations

        ReDim Me.Solutions(-1)
        ReDim Me.selSolutionIDs(-1)
        ReDim Me.SekPops(-1)

        'DB initialiseren
        Call Me.db_init()

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
    Public Function getSelectedSolutions() As Kern.Individuum()

        Dim solutions() As Kern.Individuum

        solutions = getSolutions(Me.selSolutionIDs)

        Return solutions

    End Function

    'Lösungsauswahl zurücksetzen
    '***************************
    Public Sub clearSelectedSolutions()

        ReDim Me.selSolutionIDs(-1)

    End Sub

    'Eine Lösung zum Optimierungsergebnis hinzufügen
    '***********************************************
    Public Sub addSolution(ByVal Ind as Kern.Individuum)

        'Lösung zu OptResult hinzufügen
        ReDim Preserve Me.Solutions(Me.Solutions.GetUpperBound(0) + 1)
        Me.Solutions(Me.Solutions.GetUpperBound(0)) = Ind.Clone()

        'In DB speichern
        Call Me.db_insert(Ind)

    End Sub

    'Eine Lösung identifizieren
    '**************************
    Public Function getSolution(ByVal ID As Integer) As Kern.Individuum

        Dim i As Integer

        For i = 0 To Me.Solutions.GetUpperBound(0)
            If (Me.Solutions(i).ID = ID) Then
                Return Me.Solutions(i)
            End If
        Next

        Return New Kern.Individuum("Solution", 0) 'TODO: Fehlerbehandlung

    End Function

    'Sekundäre Population hinzufügen
    '*******************************
    Public Sub setSekPop(ByVal _sekpop(,) As Double, ByVal _igen As Integer)

        Dim SekPop As Struct_SekPop

        'SekPop in DB speichern
        Call Me.db_setSekPop(_sekpop, _igen)

        'SekPop aus DB lesen
        SekPop = db_getSekPop(_igen)

        'SekPop zu OptResult hinzufügen
        Call addSekPop(SekPop, _igen)

    End Sub

    'Sekundäre Population zu OptResult hinzufügen
    '********************************************
    Private Sub addSekPop(ByVal _sekpop As Struct_SekPop, ByVal _igen As Integer)

        'Array von Sekundären Populationen um eins erweitern
        ReDim Preserve Me.SekPops(_igen)
        Me.SekPops(_igen) = _sekpop

    End Sub

    'Sekundäre Population holen
    '**************************
    Public Function getSekPop(Optional ByVal _igen As Integer = -1) As Kern.Individuum()

        Dim sekpopsolutions() As Kern.Individuum

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

    'Lösungen anhand von IDs holen
    '*****************************
    Private Function getSolutions(ByVal IDs() As Integer) As Kern.Individuum()

        Dim i As Integer
        Dim solutions() As Kern.Individuum

        ReDim solutions(IDs.GetUpperBound(0))

        For i = 0 To solutions.GetUpperBound(0)
            solutions(i) = getSolution(IDs(i))
        Next

        Return solutions

    End Function

#End Region 'Ergebnisspeicher

#Region "Ergebnisdatenbank"

    'Methoden für die Ergebnisdatenbank
    '##################################

    'Datenbank vorbereiten
    '*********************
    Private Sub db_init()

        'Ergebnisdatenbank in temporärem Verzeichnis anlegen
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
        Select Case Evo.Form1.Method
            Case METH_PES, METH_SENSIPLOT, METH_HOOKJEEVES
                Call Me.db_prepare_PES()
            Case METH_CES
                Call Me.db_prepare_CES()
            Case METH_HYBRID
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
        For i = 0 To List_OptZiele.GetUpperBound(0)
            If (i > 0) Then
                fieldnames &= ", "
            End If
            fieldnames &= "[" & List_OptZiele(i).Bezeichnung & "] DOUBLE"
        Next
        'Tabelle anpassen
        command.CommandText = "ALTER TABLE QWerte ADD COLUMN " & fieldnames
        command.ExecuteNonQuery()

        'Tabelle 'Constraints'
        '----------------
        If (Me.List_Constraints.GetLength(0) > 0) Then
            'Spalten festlegen:
            fieldnames = ""
            For i = 0 To Me.List_Constraints.GetUpperBound(0)
                If (i > 0) Then
                    fieldnames &= ", "
                End If
                fieldnames &= "[" & Me.List_Constraints(i).Bezeichnung & "] DOUBLE"
            Next
            'Tabelle anpassen
            command.CommandText = "ALTER TABLE [Constraints] ADD COLUMN " & fieldnames
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
        Dim fieldnames As String = ""
        Dim i As Integer

        For i = 0 To List_OptParameter_Save.GetUpperBound(0)
            If (i > 0) Then
                fieldnames &= ", "
            End If
            fieldnames &= "[" & List_OptParameter_Save(i).Bezeichnung & "] DOUBLE"
        Next
        'Tabelle anpassen
        command.CommandText = "ALTER TABLE OptParameter ADD COLUMN " & fieldnames
        command.ExecuteNonQuery()

        Call db_disconnect()

    End Sub

    'Ergebnisdatenbank für CES vorbereiten
    '*************************************
    Private Sub db_prepare_CES()

        Call db_connect()
        Dim command As OleDbCommand = New OleDbCommand("", db)

        'Tabelle 'Pfad'
        '--------------
        'Spalten festlegen:
        Dim fieldnames As String = ""
        Dim i As Integer

        For i = 0 To Me.List_Locations.GetUpperBound(0)
            If (i > 0) Then
                fieldnames &= ", "
            End If
            fieldnames &= "[" & Me.List_Locations(i).Name & "] TEXT"
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

    'Verbindung zu Ergebnisdatenbank schließen
    '*****************************************
    Private Sub db_disconnect()
        db.Close()
    End Sub

    'Eine Lösung in die ErgebnisDB schreiben
    '***************************************
    Private Function db_insert(ByVal ind As Kern.Individuum) As Boolean

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
        For i = 0 To List_OptZiele.GetUpperBound(0)
            fieldnames &= ", [" & List_OptZiele(i).Bezeichnung & "]"
            fieldvalues &= ", " & ind.Penalty(i).ToString(Sim.FortranProvider)
        Next
        command.CommandText = "INSERT INTO QWerte (Sim_ID" & fieldnames & ") VALUES (" & ind.ID & fieldvalues & ")"
        command.ExecuteNonQuery()

        'Constraints schreiben 
        '---------------------
        If (Me.List_Constraints.GetLength(0) > 0) Then
            fieldnames = ""
            fieldvalues = ""
            For i = 0 To Me.List_Constraints.GetUpperBound(0)
                fieldnames &= ", [" & Me.List_Constraints(i).Bezeichnung & "]"
                fieldvalues &= ", " & ind.Constrain(i).ToString(Sim.FortranProvider)
            Next
            command.CommandText = "INSERT INTO [Constraints] (Sim_ID" & fieldnames & ") VALUES (" & ind.ID & fieldvalues & ")"
            command.ExecuteNonQuery()
        End If

        If (Evo.Form1.Method = METH_PES _
            Or Evo.Form1.Method = METH_SENSIPLOT _
            Or EVO.Form1.Method = METH_HOOKJEEVES) Then

            'OptParameter schreiben
            '----------------------
            fieldnames = ""
            fieldvalues = ""
            For i = 0 To Me.List_OptParameter.GetUpperBound(0)
                fieldnames &= ", [" & Me.List_OptParameter(i).Bezeichnung & "]"
                fieldvalues &= ", " & ind.PES_OptParas(i).RWert.ToString(Sim.FortranProvider)
            Next
            command.CommandText = "INSERT INTO OptParameter (Sim_ID" & fieldnames & ") VALUES (" & ind.ID & fieldvalues & ")"
            command.ExecuteNonQuery()

        End If

        If (EVO.Form1.Method = METH_CES _
            Or EVO.Form1.Method = METH_HYBRID) Then

            'Pfad schreiben
            '--------------
            fieldnames = ""
            fieldvalues = ""
            For i = 0 To Me.List_Locations.GetUpperBound(0)
                fieldnames &= ", [" & Me.List_Locations(i).Name & "]"
                fieldvalues &= ", '" & ind.Measures(i) & "'"
            Next
            command.CommandText = "INSERT INTO Pfad (Sim_ID" & fieldnames & ") VALUES (" & ind.ID & fieldvalues & ")"
            command.ExecuteNonQuery()

        End If

        If (EVO.Form1.Method = METH_HYBRID) Then

            Dim found as Boolean

            'OptParameter schreiben
            '----------------------
            fieldnames = ""
            fieldvalues = ""
            For i = 0 To Me.List_OptParameter_Save.GetUpperBound(0)
                found  = False
                fieldnames &= ", [" & Me.List_OptParameter_Save(i).Bezeichnung & "]"
                For x = 0 to Ind.Loc.GetUpperBound(0)
                    For y = 0 to Ind.Loc(x).PES_OptPara.GetUpperBound(0)
                        If Ind.Loc(x).PES_OptPara(y).Bezeichnung = Me.List_OptParameter_Save(i).Bezeichnung then
                            fieldvalues &= ", " & Ind.Loc(x).PES_OptPara(y).RWert.ToString(Sim.FortranProvider)
                            found = True
                        End If
                    Next
                Next
                If found = False
                    fieldvalues &= ", " & "-7"
                End If
            Next
            command.CommandText = "INSERT INTO OptParameter (Sim_ID" & fieldnames & ") VALUES (" & ind.ID & fieldvalues & ")"
            command.ExecuteNonQuery()

        End If

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
            For j = 0 To Me.List_OptZiele.GetUpperBound(0)
                bedingung &= " AND QWerte.[" & Me.List_OptZiele(j).Bezeichnung & "] = " & SekPop(i, j).ToString(EVO.Sim.FortranProvider)
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
            Throw New Exception("Sekundäre Population nicht in DB vorhanden!")
        End If

        Return SekPop

    End Function

    'Ergebnisdatenbank abspeichern (kopieren)
    '****************************************
    Public Sub db_save(ByVal targetFile As String)

        'Datei kopieren
        My.Computer.FileSystem.CopyFile(Me.db_path, targetFile, True)

        'Neuen Dateipfad speichern
        Me.db_path = targetFile

    End Sub

    'Optimierungsergebnis aus einer DB lesen
    '*****************************************************
    Public Sub db_load(ByVal sourceFile As String)

        '---------------------------------------------------------------------------
        'Hinweise:
        'Die EVO-Eingabedateien müssen eingelesen sein und mit der DB übereinstimmen
        '---------------------------------------------------------------------------

        Dim i, j As Integer
        Dim numSolutions, igen As Integer
        Dim q As String
        Dim adapter As OleDbDataAdapter
        Dim ds As DataSet
        Dim command As OleDbCommand

        'Neuen Dateipfad speichern
        Me.db_path = sourceFile

        'Connect
        Call db_connect()

        'Read
        '----
        'Alle Lösungen
        Select form1.Method
            Case METH_PES, METH_SENSIPLOT, METH_HOOKJEEVES
                q = "SELECT Sim.ID, OptParameter.*, QWerte.*, Constraints.* FROM ((Sim LEFT JOIN [Constraints] ON Sim.ID=Constraints.Sim_ID) INNER JOIN OptParameter ON Sim.ID=OptParameter.Sim_ID) INNER JOIN QWerte ON Sim.ID=QWerte.Sim_ID ORDER BY Sim.ID"
            Case METH_CES
                q = "SELECT Sim.ID, Pfad.*, QWerte.*, Constraints.* FROM ((Sim LEFT JOIN [Constraints] ON Sim.ID=Constraints.Sim_ID) INNER JOIN Pfad ON Sim.ID=Pfad.Sim_ID) INNER JOIN QWerte ON Sim.ID=QWerte.Sim_ID"
            Case METH_HYBRID
                q = "SELECT Sim.ID, Pfad.*, OptParameter.*, QWerte.*, Constraints.* FROM (((Sim LEFT JOIN [Constraints] ON Sim.ID=Constraints.Sim_ID) INNER JOIN Pfad ON Sim.ID=Pfad.Sim_ID) INNER JOIN OptParameter ON Sim.ID=OptParameter.Sim_ID) INNER JOIN QWerte ON Sim.ID=QWerte.Sim_ID ORDER BY Sim.ID"
        End Select

        adapter = New OleDbDataAdapter(q, db)

        ds = New DataSet("EVO")
        numSolutions = adapter.Fill(ds, "Result")

        'Letzte SekPop-Generation bestimmen
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

        'Alle Lösungen übernehmen
        '========================
        ReDim Me.Solutions(numSolutions - 1)

        For i = 0 To numSolutions - 1

            Me.Solutions(i) = New Kern.Individuum("Solution", i)

            With Me.Solutions(i)
                'ID
                '--
                .ID = ds.Tables(0).Rows(i).Item("Sim.ID")
                
                ReDim .PES_OptParas(Me.List_OptParameter_Save.GetUpperBound(0))

                'OptParameter
                '------------
                For j = 0 To Me.List_OptParameter_Save.GetUpperBound(0)
                    .PES_OptParas(j) = Me.List_OptParameter_Save(j).Clone()
                    .PES_OptParas(j).RWert = ds.Tables(0).Rows(i).Item(Me.List_OptParameter_Save(j).Bezeichnung)
                Next

                'QWerte
                '------
                ReDim .Penalty(Me.List_OptZiele.GetUpperBound(0))
                For j = 0 To Me.List_OptZiele.GetUpperBound(0)
                    .Penalty(j) = ds.Tables(0).Rows(i).Item(Me.List_OptZiele(j).Bezeichnung)
                Next

                'Constraints
                '-----------
                ReDim .Constrain(Me.List_Constraints.GetUpperBound(0))
                For j = 0 To Me.List_Constraints.GetUpperBound(0)
                    .Constrain(j) = ds.Tables(0).Rows(i).Item(Me.List_Constraints(j).Bezeichnung)
                Next

                'Pfad
                '----
                ReDim .Measures(Me.List_Locations.GetUpperBound(0))
                For j = 0 To Me.List_Locations.GetUpperBound(0)
                    .Measures(j) = ds.Tables(0).Rows(i).Item(Me.List_Locations(j).Name)
                Next

            End With

        Next

        'Sekundärpopulation übernehmen
        '=============================
        ReDim Me.SekPops(-1)

        If (igen > -1) Then
            Dim SekPop As Struct_SekPop
            SekPop = db_getSekPop(igen)
            Call Me.addSekPop(SekPop, igen)
        End If

    End Sub

#End Region 'Ergebnisdatenbank

End Class
