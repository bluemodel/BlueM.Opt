Imports System.Data.OleDb

Public Class ResultManager

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse OptResult                                                      ****
    '****                                                                       ****
    '**** Speichert und verwaltet die Ergebnisse eines Optimierungslaufs,       ****
    '**** schreibt die Ergebnisse in eine Datenbank                             ****
    '****                                                                       ****
    '**** Autoren: Felix Froehlich, Christoph Hübner                            ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

    'Allgemeine Einstellungen
    Private Datensatz As String

    'Ergebnisdatenbank
    Public Shared Ergebnisdb As Boolean = True              'Gibt an, ob die Ergebnisdatenbank geschrieben werden soll
    Private Shared db_path As String                        'Pfad zur Ergebnisdatenbank
    Private _dbconnection As OleDbConnection
    Private ReadOnly Property mydbconnection() As OleDbConnection
        Get
            If (_dbconnection.State = ConnectionState.Closed) Then
                Call Me.db_connect()
            End If
            Return _dbconnection
        End Get
    End Property

    'Anzahl Manager-Instanzen
    Shared nManagers As Integer = 0

    'Optimierungsbedingungen
    Public List_OptParameter() As EVO.Common.OptParameter
    Public List_OptParameter_Save() As EVO.Common.OptParameter
    Public List_Locations() As Sim.Struct_Lokation

    'Ergebnisspeicher
    Public Shared OptResult As ResultStore
    Public Shared OptResultRef As ResultStore                       'Vergleichsergebnis

    'Array von ausgewählten Lösungen
    Private Shared selSolutionIDs() As Integer

    'Konstruktor
    '***********
    Public Sub New(ByVal Sim1 As Sim)

        If (nManagers = 0) Then
            ResultManager.OptResult = New ResultStore()
            ReDim selSolutionIDs(-1)
        End If

        nManagers += 1

        'Datensatzname speichern
        Me.Datensatz = Sim1.Datensatz

        'Optimierungsbedingungen kopieren
        Me.List_OptParameter = Sim1.List_OptParameter
        Me.List_OptParameter_Save = Sim1.List_OptParameter_Save
        Me.List_Locations = Sim1.List_Locations

        'DB initialiseren
        _dbconnection = New OleDbConnection()
        Call Me.db_init()

    End Sub

    'Copy-Constructor
    '****************
    Public Sub New(ByVal optresult As ResultManager)

        Dim i As Integer

        nManagers += 1

        'neue DB-Connection!
        Me._dbconnection = New OleDbConnection()

        'Den Rest kopieren
        ReDim Me.List_OptParameter(optresult.List_OptParameter.GetUpperBound(0))
        For i = 0 To optresult.List_OptParameter.GetUpperBound(0)
            Me.List_OptParameter(i) = optresult.List_OptParameter(i).Clone()
        Next

        ReDim Me.List_OptParameter_Save(optresult.List_OptParameter_Save.GetUpperBound(0))
        For i = 0 To optresult.List_OptParameter_Save.GetUpperBound(0)
            Me.List_OptParameter_Save(i) = optresult.List_OptParameter_Save(i).Clone()
        Next

        ReDim Me.List_Locations(optresult.List_Locations.GetUpperBound(0))
        For i = 0 To optresult.List_Locations.GetUpperBound(0)
            Me.List_Locations(i) = optresult.List_Locations(i).Clone()
        Next

    End Sub

#Region "Ergebnisspeicher"

    'Eine Lösung auswählen
    '*********************
    Public Function selectSolution(ByVal ID As Integer) As Boolean

        'Überprüfen, ob Lösung bereits ausgewählt ist      
        For Each _id As Integer In selSolutionIDs
            If (_id = ID) Then
                Return False
            End If
        Next

        'Lösung zu Auswahl hinzufügen
        ReDim Preserve selSolutionIDs(selSolutionIDs.GetUpperBound(0) + 1)
        selSolutionIDs(selSolutionIDs.GetUpperBound(0)) = ID

        Return True

    End Function

    'Ausgewählte Lösungen holen
    '**************************
    Public Shared ReadOnly Property getSelectedSolutions() As Common.Individuum()
        Get
            Dim solutions() As Common.Individuum

            solutions = OptResult.getSolutions(selSolutionIDs)

            Return solutions
        End Get
    End Property

    'Lösungsauswahl zurücksetzen
    '***************************
    Public Sub clearSelectedSolutions()

        ReDim selSolutionIDs(-1)

    End Sub

    'Eine Lösung zum Optimierungsergebnis hinzufügen
    '***********************************************
    Public Sub addSolution(ByVal Ind As Common.Individuum)

        'In DB speichern
        Call Me.db_insert(Ind)

        'Lösung zu OptResult hinzufügen
        ReDim Preserve OptResult.Solutions(OptResult.Solutions.GetUpperBound(0) + 1)
        OptResult.Solutions(OptResult.Solutions.GetUpperBound(0)) = Ind.Clone()

    End Sub

    'Sekundäre Population hinzufügen
    '*******************************
    Public Sub setSekPop(ByVal pop() As Common.Individuum, ByVal _igen As Integer)

        Dim tmpsekpop As ResultStore.Struct_SekPop
        Dim tmpsekpopvalues(,) As Double

        'Population in Array von Penalty-Werten transformieren
        tmpsekpopvalues = Common.Individuum.Get_All_Penalty_of_Array(pop)

        'SekPop in DB speichern
        Call Me.db_setSekPop(tmpsekpopvalues, _igen)

        'SekPop aus DB lesen
        tmpsekpop = Me.db_getSekPop(_igen)

        'SekPop zu OptResult hinzufügen
        Call OptResult.addSekPop(tmpsekpop)

    End Sub

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
        db_path = My.Computer.FileSystem.SpecialDirectories.Temp & "\EVO.mdb"

        'Pfad zur Vorlage
        Dim db_source_path As String = System.Windows.Forms.Application.StartupPath() & "\EVO.mdb"

        'Datei kopieren
        My.Computer.FileSystem.CopyFile(db_source_path, db_path, True)

        'Tabellen anpassen
        '-----------------

        'Allgemeine Anpassungen
        Call Me.db_prepare()
        'Methodenspezifische Anpassungen
        Select Case EVO.Form1.Method
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

        Dim command As OleDbCommand = New OleDbCommand("", mydbconnection)

        'Tabelle 'QWerte'
        '----------------
        'Spalten festlegen:
        Dim fieldnames As String = ""
        For i = 0 To Common.Manager.AnzZiele - 1
            If (i > 0) Then
                fieldnames &= ", "
            End If
            fieldnames &= "[" & Common.Manager.List_Ziele(i).Bezeichnung & "] DOUBLE"
        Next
        'Tabelle anpassen
        command.CommandText = "ALTER TABLE QWerte ADD COLUMN " & fieldnames
        command.ExecuteNonQuery()

        'Tabelle 'Constraints'
        '----------------
        If (Common.Manager.AnzConstraints > 0) Then
            'Spalten festlegen:
            fieldnames = ""
            For i = 0 To Common.Manager.AnzConstraints - 1
                If (i > 0) Then
                    fieldnames &= ", "
                End If
                fieldnames &= "[" & Common.Manager.List_Constraints(i).Bezeichnung & "] DOUBLE"
            Next
            'Tabelle anpassen
            command.CommandText = "ALTER TABLE [Constraints] ADD COLUMN " & fieldnames
            command.ExecuteNonQuery()
        End If

    End Sub

    'Ergebnisdatenbank für PES vorbereiten
    '*************************************
    Private Sub db_prepare_PES()

        Dim command As OleDbCommand = New OleDbCommand("", mydbconnection)

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

    End Sub

    'Ergebnisdatenbank für CES vorbereiten
    '*************************************
    Private Sub db_prepare_CES()

        Dim command As OleDbCommand = New OleDbCommand("", mydbconnection)

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

    End Sub

    'Mit Ergebnisdatenbank verbinden
    '*******************************
    Private Sub db_connect()
        Dim ConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & db_path
        _dbconnection = New OleDb.OleDbConnection(ConnectionString)
        _dbconnection.Open()
    End Sub

    'Verbindung zu Ergebnisdatenbank schließen
    '*****************************************
    Private Sub db_disconnect()
        _dbconnection.Close()
    End Sub

    'Eine Lösung in die ErgebnisDB schreiben
    '***************************************
    Private Function db_insert(ByVal ind As Common.Individuum) As Boolean

        Dim i, x, y As Integer
        Dim command As OleDbCommand
        Dim transaction As OleDbTransaction

        'Start the transaction
        transaction = Me.mydbconnection.BeginTransaction()
        command = New OleDbCommand("", Me.mydbconnection, transaction)

        'Sim schreiben
        '-------------
        command.CommandText = "INSERT INTO Sim (ID, Name) VALUES (" & ind.ID & ", '" & Me.Datensatz & "')"
        command.ExecuteNonQuery()

        'QWerte schreiben 
        '----------------
        Dim fieldnames As String = ""
        Dim fieldvalues As String = ""
        For i = 0 To Common.Manager.AnzZiele - 1
            fieldnames &= ", [" & Common.Manager.List_Ziele(i).Bezeichnung & "]"
            fieldvalues &= ", " & ind.Zielwerte(i).ToString(Common.Provider.FortranProvider)
        Next
        command.CommandText = "INSERT INTO QWerte (Sim_ID" & fieldnames & ") VALUES (" & ind.ID & fieldvalues & ")"
        command.ExecuteNonQuery()

        'Constraints schreiben 
        '---------------------
        If (Common.Manager.AnzConstraints > 0) Then
            fieldnames = ""
            fieldvalues = ""
            For i = 0 To Common.Manager.AnzConstraints - 1
                fieldnames &= ", [" & Common.Manager.List_Constraints(i).Bezeichnung & "]"
                fieldvalues &= ", " & ind.Constrain(i).ToString(Common.Provider.FortranProvider)
            Next
            command.CommandText = "INSERT INTO [Constraints] (Sim_ID" & fieldnames & ") VALUES (" & ind.ID & fieldvalues & ")"
            command.ExecuteNonQuery()
        End If

        If (EVO.Form1.Method = METH_PES _
            Or EVO.Form1.Method = METH_SENSIPLOT _
            Or EVO.Form1.Method = METH_HOOKJEEVES) Then

            'OptParameter schreiben
            '----------------------
            fieldnames = ""
            fieldvalues = ""
            For i = 0 To Me.List_OptParameter.GetUpperBound(0)
                fieldnames &= ", [" & Me.List_OptParameter(i).Bezeichnung & "]"
                fieldvalues &= ", " & ind.PES_OptParas(i).RWert.ToString(Common.Provider.FortranProvider)
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

            Dim found As Boolean

            'OptParameter schreiben
            '----------------------
            fieldnames = ""
            fieldvalues = ""
            For i = 0 To Me.List_OptParameter_Save.GetUpperBound(0)
                found = False
                fieldnames &= ", [" & Me.List_OptParameter_Save(i).Bezeichnung & "]"
                For x = 0 To ind.Loc.GetUpperBound(0)
                    For y = 0 To ind.Loc(x).PES_OptPara.GetUpperBound(0)
                        If ind.Loc(x).PES_OptPara(y).Bezeichnung = Me.List_OptParameter_Save(i).Bezeichnung Then
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

        'Commit the transaction
        Call transaction.Commit()

    End Function

    'Sekundäre Population in DB speichern
    '************************************
    Private Sub db_setSekPop(ByVal SekPop(,) As Double, ByVal igen As Integer)

        Dim command As OleDbCommand

        'Start the transaction
        command = New OleDbCommand("", Me.mydbconnection)

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
            For j = 0 To Common.Manager.AnzPenalty - 1
                bedingung &= " AND QWerte.[" & Common.Manager.List_OptZiele(j).Bezeichnung & "] = " & SekPop(i, j).ToString(Common.Provider.FortranProvider)
            Next
            command.CommandText = "SELECT Sim.ID FROM Sim INNER JOIN QWerte ON Sim.ID = QWerte.Sim_ID WHERE (1=1" & bedingung & ")"
            Sim_ID = command.ExecuteScalar()

            If (Sim_ID > 0) Then
                'SekPop Member speichern
                command.CommandText = "INSERT INTO SekPop (Generation, Sim_ID) VALUES (" & igen & ", " & Sim_ID & ")"
                command.ExecuteNonQuery()
            Else
                Throw New Exception("Konnte Lösung der sekundären Population nicht in DB identifizieren!")
            End If
        Next

    End Sub

    'SekPop aus DB lesen
    '*******************
    Private Function db_getSekPop(ByVal igen As Integer) As ResultStore.Struct_SekPop

        Dim i As Integer
        Dim q As String
        Dim adapter As OleDbDataAdapter
        Dim ds As DataSet
        Dim numrows As Integer
        Dim tmpsekpop As ResultStore.Struct_SekPop

        q = "SELECT Sim_ID FROM SekPop WHERE Generation = " & igen

        adapter = New OleDbDataAdapter(q, mydbconnection)

        ds = New DataSet("EVO")
        numrows = adapter.Fill(ds, "SekPop")

        If (numrows > 0) Then

            tmpsekpop.iGen = igen
            ReDim tmpsekpop.SolutionIDs(numrows - 1)

            For i = 0 To numrows - 1
                tmpsekpop.SolutionIDs(i) = ds.Tables("SekPop").Rows(i).Item("Sim_ID")
            Next

        Else
            Throw New Exception("Sekundäre Population nicht in DB vorhanden!")
        End If

        Return tmpsekpop

    End Function

    'Letzte Generation bestimmen
    '***************************
    Private Function db_getLastGenNo() As Integer

        Dim command As OleDbCommand
        Dim igen As Integer

        Try
            command = New OleDbCommand("", mydbconnection)
            command.CommandText = "SELECT MAX(Generation) FROM SekPop"
            igen = command.ExecuteScalar()
        Catch ex As Exception
            'Keine SekPop vorhanden
            igen = -1
        End Try

        Return igen

    End Function

    'Ergebnisdatenbank abspeichern (kopieren)
    '****************************************
    Public Sub db_save(ByVal targetFile As String)

        'Datei kopieren
        My.Computer.FileSystem.CopyFile(db_path, targetFile, True)

        'Neuen Dateipfad speichern
        db_path = targetFile

    End Sub

    'Optimierungsergebnis aus einer DB lesen
    '*****************************************************
    Public Function db_load(ByVal sourceFile As String, Optional ByVal QWerteOnly As Boolean = False) As ResultStore

        '---------------------------------------------------------------------------
        'Hinweise:
        'Die EVO-Eingabedateien müssen eingelesen sein und mit der DB übereinstimmen
        '---------------------------------------------------------------------------

        Dim i, j As Integer
        Dim numSolutions As Integer
        Dim q As String = ""
        Dim adapter As OleDbDataAdapter
        Dim ds As DataSet
        Dim tmpresult As New ResultStore()

        'Neuen Dateipfad speichern
        db_path = sourceFile

        'Read
        '----
        'Alle Lösungen
        Select Case Form1.Method
            Case METH_PES, METH_SENSIPLOT, METH_HOOKJEEVES
                q = "SELECT Sim.ID, OptParameter.*, QWerte.*, Constraints.* FROM ((Sim LEFT JOIN [Constraints] ON Sim.ID=Constraints.Sim_ID) INNER JOIN OptParameter ON Sim.ID=OptParameter.Sim_ID) INNER JOIN QWerte ON Sim.ID=QWerte.Sim_ID ORDER BY Sim.ID"
            Case METH_CES
                q = "SELECT Sim.ID, Pfad.*, QWerte.*, Constraints.* FROM ((Sim LEFT JOIN [Constraints] ON Sim.ID=Constraints.Sim_ID) INNER JOIN Pfad ON Sim.ID=Pfad.Sim_ID) INNER JOIN QWerte ON Sim.ID=QWerte.Sim_ID"
            Case METH_HYBRID
                q = "SELECT Sim.ID, Pfad.*, OptParameter.*, QWerte.*, Constraints.* FROM (((Sim LEFT JOIN [Constraints] ON Sim.ID=Constraints.Sim_ID) INNER JOIN Pfad ON Sim.ID=Pfad.Sim_ID) INNER JOIN OptParameter ON Sim.ID=OptParameter.Sim_ID) INNER JOIN QWerte ON Sim.ID=QWerte.Sim_ID ORDER BY Sim.ID"
        End Select

        adapter = New OleDbDataAdapter(q, mydbconnection)

        ds = New DataSet("EVO")
        numSolutions = adapter.Fill(ds, "Result")

        'Alle Lösungen übernehmen
        '========================
        ReDim tmpresult.Solutions(numSolutions - 1)

        For i = 0 To numSolutions - 1

            tmpresult.Solutions(i) = New Common.Individuum("Solution", i)

            With tmpresult.Solutions(i)
                'ID
                '--
                .ID = ds.Tables(0).Rows(i).Item("Sim.ID")

                If (Not QWerteOnly) Then

                    ReDim .PES_OptParas(Me.List_OptParameter_Save.GetUpperBound(0))

                    'OptParameter
                    '------------
                    For j = 0 To Me.List_OptParameter_Save.GetUpperBound(0)
                        .PES_OptParas(j) = Me.List_OptParameter_Save(j).Clone()
                        .PES_OptParas(j).RWert = ds.Tables(0).Rows(i).Item(Me.List_OptParameter_Save(j).Bezeichnung)
                    Next

                    'Constraints
                    '-----------
                    For j = 0 To Common.Manager.AnzConstraints - 1
                        .Constrain(j) = ds.Tables(0).Rows(i).Item(Common.Manager.List_Constraints(j).Bezeichnung)
                    Next

                    'Pfad
                    '----
                    ReDim .Measures(Me.List_Locations.GetUpperBound(0))
                    For j = 0 To Me.List_Locations.GetUpperBound(0)
                        .Measures(j) = ds.Tables(0).Rows(i).Item(Me.List_Locations(j).Name)
                    Next

                End If

                'QWerte
                '------
                For j = 0 To Common.Manager.AnzZiele - 1
                    .Zielwerte(j) = ds.Tables(0).Rows(i).Item(Common.Manager.List_Ziele(j).Bezeichnung)
                Next

            End With

        Next

        'Sekundärpopulationen laden
        tmpresult.SekPops = Me.db_loadSekPops()

        Return tmpresult

    End Function


    'Sekundärpopulationen aus DB laden
    '*********************************
    Private Function db_loadSekPops() As ResultStore.Struct_SekPop()

        Dim i, igen As Integer
        Dim tmpSekPops() As ResultStore.Struct_SekPop

        'Letzte SekPop-Generation bestimmen
        igen = db_getLastGenNo()

        ReDim tmpSekPops(igen)

        'Alle SekPops durchlaufen und einlesen
        For i = 0 To igen
            tmpSekPops(i) = db_getSekPop(i)
        Next

        Return tmpSekPops

    End Function

#End Region 'Ergebnisdatenbank

End Class
