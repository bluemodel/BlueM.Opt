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
    Public List_OptParameter() As Sim.Struct_OptParameter
    Public List_Constraints() As Sim.Struct_Constraint
    Public List_Locations()As Sim.Struct_Lokation

    'Array von L�sungen
    Public Solutions() As Kern.Individuum

    'Structure f�r Sekund�re Population
    Public Structure Struct_SekPop
        Public iGen As Integer                      'Generationsnummer
        Public SolutionIDs() As Integer             'Array von Solution-IDs
    End Structure

    'Array von Sekund�ren Populationen
    Private SekPops() As Struct_SekPop

    'Array von ausgew�hlten L�sungen
    Private selSolutionIDs() As Integer

    'Konstruktor
    '***********
    Public Sub New(ByVal Sim1 As Sim)

        'Datensatzname speichern
        Me.Datensatz = Sim1.Datensatz

        'Optimierungsbedingungen kopieren
        Me.List_OptZiele = Sim1.List_OptZiele
        Me.List_OptParameter = Sim1.List_OptParameter
        Me.List_Constraints = Sim1.List_Constraints
        Me.List_Locations = Sim1.List_Locations

        ReDim Me.Solutions(-1)
        ReDim Me.selSolutionIDs(-1)
        ReDim Me.SekPops(-1)

        'DB initialiseren
        Call Me.db_init()

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
    Public Function getSelectedSolutions() As Kern.Individuum()

        Dim solutions() As Kern.Individuum

        solutions = getSolutions(Me.selSolutionIDs)

        Return solutions

    End Function

    'L�sungsauswahl zur�cksetzen
    '***************************
    Public Sub clearSelectedSolutions()

        ReDim Me.selSolutionIDs(-1)

    End Sub

    'Eine L�sung zum Optimierungsergebnis hinzuf�gen
    '***********************************************
    Public Sub addSolution(ByVal Ind as Kern.Individuum)

        'L�sung zu OptResult hinzuf�gen
        ReDim Preserve Me.Solutions(Me.Solutions.GetUpperBound(0) + 1)
        Me.Solutions(Me.Solutions.GetUpperBound(0)) = Ind.Copy

        'In DB speichern
        Call Me.db_insert(Ind)

    End Sub

    'Eine L�sung identifizieren
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

    'Sekund�re Population hinzuf�gen
    '*******************************
    Public Sub setSekPop(ByVal _sekpop(,) As Double, ByVal _igen As Integer)

        Dim SekPop As Struct_SekPop

        'SekPop in DB speichern
        Call Me.db_setSekPop(_sekpop, _igen)

        'SekPop aus DB lesen
        SekPop = db_getSekPop(_igen)

        'SekPop zu OptResult hinzuf�gen
        Call addSekPop(SekPop, _igen)

    End Sub

    'Sekund�re Population zu OptResult hinzuf�gen
    '********************************************
    Private Sub addSekPop(ByVal _sekpop As Struct_SekPop, ByVal _igen As Integer)

        'Array von Sekund�ren Populationen um eins erweitern
        ReDim Preserve Me.SekPops(_igen)
        Me.SekPops(_igen) = _sekpop

    End Sub

    'Sekund�re Population holen
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

        'Alle Sekund�ren Populationen durchlaufen
        For Each sekpop As Struct_SekPop In Me.SekPops
            If (sekpop.iGen = _igen) Then
                'SekPop gefunden, L�sungen holen
                sekpopsolutions = getSolutions(sekpop.SolutionIDs)
            End If
        Next

        Return sekpopsolutions

    End Function

    'L�sungen anhand von IDs holen
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

        For i = 0 To List_OptParameter.GetUpperBound(0)
            If (i > 0) Then
                fieldnames &= ", "
            End If
            fieldnames &= "[" & List_OptParameter(i).Bezeichnung & "] DOUBLE"
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

    'Verbindung zu Ergebnisdatenbank schlie�en
    '*****************************************
    Private Sub db_disconnect()
        db.Close()
    End Sub

    'Eine L�sung in die ErgebnisDB schreiben
    '***************************************
    Private Function db_insert(ByVal ind As Kern.Individuum) As Boolean

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

        If (Evo.Form1.Method = METH_PES Or Evo.Form1.Method = METH_SENSIPLOT) Then

            'OptParameter schreiben
            '----------------------
            fieldnames = ""
            fieldvalues = ""
            For i = 0 To Me.List_OptParameter.GetUpperBound(0)
                fieldnames &= ", [" & Me.List_OptParameter(i).Bezeichnung & "]"
                fieldvalues &= ", " & ind.PES_X(i).ToString(Sim.FortranProvider)
            Next
            command.CommandText = "INSERT INTO OptParameter (Sim_ID" & fieldnames & ") VALUES (" & ind.ID & fieldvalues & ")"
            command.ExecuteNonQuery()

        End If

        'BUG 260: db_insert f�r CES

        'If (Me.Method = "CES" Or Me.Method = "CES + PES") Then

        '    'Pfad schreiben
        '    '--------------
        '    fieldnames = ""
        '    fieldvalues = ""
        '    For i = 0 To Me.List_Locations.GetUpperBound(0)
        '        fieldnames &= ", [" & Me.List_Locations(i).Name & "]"
        '        fieldvalues &= ", '" & ind.Akt(i).Measures(i) & "'"
        '    Next
        '    command.CommandText = "INSERT INTO Pfad (Sim_ID" & fieldnames & ") VALUES (" & ind.ID & fieldvalues & ")"
        '    command.ExecuteNonQuery()

        'End If

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
            For j = 0 To Me.List_OptZiele.GetUpperBound(0)
                bedingung &= " AND QWerte.[" & Me.List_OptZiele(j).Bezeichnung & "] = " & SekPop(i, j)
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

    'Einen Parametersatz aus der DB �bernehmen
    'TODO: Funktion db_getPara wird nicht mehr genutzt!
    '**************************************************
    Private Function db_getPara(ByRef ind As Kern.Individuum, ByVal xAchse As String, ByVal xWert As Double, ByVal yAchse As String, ByVal yWert As Double) As Boolean

        Dim isOK As Boolean = False
        Dim q As String
        Dim adapter As OleDbDataAdapter
        Dim ds As DataSet
        Dim numrows As Integer

        'TODO: eigentlich m�sste die ID aus der db �bernommen werden
        ind = New Kern.Individuum("Solution", 0)
        ReDim ind.PES_X(Me.List_OptParameter.GetUpperBound(0))

        Call db_connect()

        'Fallunterscheidung nach Methode
        Select Case Evo.Form1.Method

            Case METH_PES, METH_SENSIPLOT

                'Unterscheidung
                If (Evo.Form1.Method = METH_SENSIPLOT) Then
                    'xAchse ist QWert, yAchse ist OptPara
                    q = "SELECT OptParameter.* FROM OptParameter INNER JOIN QWerte ON OptParameter.Sim_ID = QWerte.Sim_ID WHERE (OptParameter.[" & yAchse & "] = " & yWert & " AND QWerte.[" & xAchse & "] = " & xWert & ")"
                ElseIf (Me.List_OptZiele.Length = 1) Then
                    'nur yAchse ist QWert
                    q = "SELECT OptParameter.* FROM OptParameter INNER JOIN QWerte ON OptParameter.Sim_ID = QWerte.Sim_ID WHERE (QWerte.[" & yAchse & "] = " & yWert & ")"
                Else
                    'xAchse und yAchse sind beides QWerte
                    q = "SELECT OptParameter.* FROM OptParameter INNER JOIN QWerte ON OptParameter.Sim_ID = QWerte.Sim_ID WHERE (QWerte.[" & xAchse & "] = " & xWert & " AND QWerte.[" & yAchse & "] = " & yWert & ")"
                End If

                adapter = New OleDbDataAdapter(q, db)

                ds = New DataSet("EVO")
                numrows = adapter.Fill(ds, "OptParameter")

                'Anzahl �bereinstimmungen �berpr�fen
                If (numrows = 0) Then
                    MsgBox("Es wurde keine �bereinstimmung in der Datenbank gefunden!", MsgBoxStyle.Exclamation, "Problem")
                    Return False
                ElseIf (numrows > 1) Then
                    MsgBox("Es wurden mehr als eine Entsprechung von OptParametern f�r den gew�hlten Punkt gefunden!" & eol & "Es wird nur das erste Ergebnis verwendet!", MsgBoxStyle.Exclamation, "Problem")
                End If

                'OptParametersatz �bernehmen
                For i As Integer = 0 To Me.List_OptParameter.GetUpperBound(0)
                    ind.PES_X(i) = ds.Tables("OptParameter").Rows(0).Item(Me.List_OptParameter(i).Bezeichnung)
                Next

                isOK = True

                Return isOK

                'BUG 260: db_getPara f�r CES


                'Case METH_CES

                '    q = "SELECT Pfad.* FROM Pfad INNER JOIN QWerte ON Pfad.Sim_ID = QWerte.Sim_ID WHERE (QWerte.[" & xAchse & "] = " & xWert & " AND QWerte.[" & yAchse & "] = " & yWert & ")"

                '    adapter = New OleDbDataAdapter(q, db)

                '    ds = New DataSet("EVO")
                '    numrows = adapter.Fill(ds, "Pfad")

                '    'Anzahl �bereinstimmungen �berpr�fen
                '    If (numrows = 0) Then
                '        MsgBox("Es wurde keine �bereinstimmung in der Datenbank gefunden!", MsgBoxStyle.Exclamation, "Problem")
                '        Return False
                '    ElseIf (numrows > 1) Then
                '        MsgBox("Es wurden mehr als eine Entsprechung von Pfaden f�r den gew�hlten Punkt gefunden!" & eol & "Es wird nur das erste Ergebnis verwendet!", MsgBoxStyle.Exclamation, "Problem")
                '    End If

                '    'Pfad �bernehmen
                '    For i As Integer = 0 To Me.Akt.Measures.GetUpperBound(0)
                '        Me.Akt.Measures(i) = ds.Tables("Pfad").Rows(0).Item(List_Locations(i).Name)
                '    Next

                '    'Bereitet das BlaueModell f�r die Kombinatorik vor
                '    Call Me.PREPARE_Evaluation_CES()


                'Case METH_CES_PES

                '    q = "SELECT OptParameter.*, Pfad.* FROM (((Sim LEFT JOIN Constraints ON Sim.ID = Constraints.Sim_ID) INNER JOIN OptParameter ON Sim.ID = OptParameter.Sim_ID) INNER JOIN Pfad ON Sim.ID = Pfad.Sim_ID) INNER JOIN QWerte ON Sim.ID = QWerte.Sim_ID WHERE (QWerte.[" & xAchse & "] = " & xWert & " AND QWerte.[" & yAchse & "] = " & yWert & ")"

                '    adapter = New OleDbDataAdapter(q, db)

                '    ds = New DataSet("EVO")
                '    adapter.Fill(ds, "OptParameter_Pfad")

                '    'Anzahl �bereinstimmungen �berpr�fen
                '    numrows = ds.Tables("OptParameter_Pfad").Rows.Count

                '    If (numrows = 0) Then
                '        MsgBox("Es wurde keine �bereinstimmung in der Datenbank gefunden!", MsgBoxStyle.Exclamation, "Problem")
                '        Return False
                '    ElseIf (numrows > 1) Then
                '        MsgBox("Es wurden mehr als eine Entsprechung von OptParametern / Pfad f�r den gew�hlten Punkt gefunden!" & eol & "Es wird nur das erste Ergebnis verwendet!", MsgBoxStyle.Exclamation, "Problem")
                '    End If

                '    'Pfad �bernehmen
                '    For i As Integer = 0 To Me.Akt.Measures.GetUpperBound(0)
                '        Me.Akt.Measures(i) = ds.Tables("OptParameter_Pfad").Rows(0).Item(List_Locations(i).Name)
                '    Next

                '    'Bereitet das BlaueModell f�r die Kombinatorik vor
                '    Call Me.PREPARE_Evaluation_CES()

                '    'OptParametersatz �bernehmen
                '    For i As Integer = 0 To Me.List_OptParameter.GetUpperBound(0)
                '        With Me.List_OptParameter(i)
                '            .Wert = ds.Tables("OptParameter_Pfad").Rows(0).Item(.Bezeichnung)
                '        End With
                '    Next

                '    'Modellparameter schreiben
                '    Call Me.Write_ModellParameter()

        End Select

        Call db_disconnect()

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
    '***************************************
    Public Sub db_load(ByVal sourceFile As String)

        '---------------------------------------------------------------------------
        'Hinweise:
        'Die EVO-Eingabedateien m�ssen eingelesen sein und mit der DB �bereinstimmen
        'Funktioniert momentan nur f�r PES
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
        'Alle L�sungen
        q = "SELECT Sim.ID, OptParameter.*, QWerte.*, Constraints.* FROM ((Sim LEFT JOIN [Constraints] ON Sim.ID=Constraints.Sim_ID) INNER JOIN OptParameter ON Sim.ID=OptParameter.Sim_ID) INNER JOIN QWerte ON Sim.ID=QWerte.Sim_ID ORDER BY Sim.ID"

        adapter = New OleDbDataAdapter(q, db)

        ds = New DataSet("EVO")
        numSolutions = adapter.Fill(ds, "PESResult")

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

        'Alle L�sungen �bernehmen
        '========================
        ReDim Me.Solutions(numSolutions - 1)

        For i = 0 To numSolutions - 1

            Me.Solutions(i) = New Kern.Individuum("Solution", i)

            With Me.Solutions(i)
                'ID
                '--
                .ID = ds.Tables(0).Rows(i).Item("Sim.ID")
                'OptParameter
                '------------
                ReDim .PES_X(Me.List_OptParameter.GetUpperBound(0))
                For j = 0 To Me.List_OptParameter.GetUpperBound(0)
                    .PES_X(j) = ds.Tables(0).Rows(i).Item(Me.List_OptParameter(j).Bezeichnung)
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
            End With

        Next

        'Sekund�rpopulation �bernehmen
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
