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
    Private Method As String
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

    'Array von Lösungen
    Public Solutions() As Solution

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

        'Methode speichern
        Me.Method = Sim1.Method

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
        Me.db_path = Me.Datensatz & ".mdb"
        Call Me.db_init(Me.db_path)

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
    Public Function getSelectedSolutions() As Solution()

        Dim solutions() As Solution

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
    Public Sub addSolution(ByVal ID As Integer, ByVal lOptZiele() As Sim.Struct_OptZiel, ByVal lConstraints() As Sim.Struct_Constraint, ByVal lOptParameter() As Sim.Struct_OptParameter)

        Dim i As Integer
        Dim sol As Solution

        'Lösung übernehmen
        Me.List_OptParameter = lOptParameter
        Me.List_OptZiele = lOptZiele
        Me.List_Constraints = lConstraints

        'Lösung hinzufügen
        sol = New Solution()
        sol.ID = ID
        ReDim sol.OptPara(Me.List_OptParameter.GetUpperBound(0))
        ReDim sol.QWerte(Me.List_OptZiele.GetUpperBound(0))
        ReDim sol.Constraints(Me.List_Constraints.GetUpperBound(0))

        For i = 0 To Me.List_OptParameter.GetUpperBound(0)
            sol.OptPara(i) = Me.List_OptParameter(i).Wert
        Next

        For i = 0 To Me.List_OptZiele.GetUpperBound(0)
            sol.QWerte(i) = Me.List_OptZiele(i).QWertTmp
        Next

        For i = 0 To Me.List_Constraints.GetUpperBound(0)
            sol.Constraints(i) = Me.List_Constraints(i).ConstTmp
        Next

        ReDim Preserve Me.Solutions(Me.Solutions.GetUpperBound(0) + 1)
        Me.Solutions(Me.Solutions.GetUpperBound(0)) = sol

        'In DB speichern
        Call Me.db_insert(sol)

    End Sub

    'Eine Lösung identifizieren
    '**************************
    Public Function getSolution(ByVal ID As Integer) As Solution

        Dim i As Integer

        For i = 0 To Me.Solutions.GetUpperBound(0)
            If (Me.Solutions(i).ID = ID) Then
                Return Me.Solutions(i)
            End If
        Next

        Return New Solution() 'TODO: Fehlerbehandlung

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
    Public Function getSekPop(Optional ByVal _igen As Integer = -1) As Solution()

        Dim sekpopsolutions() As Solution

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
    Private Function getSolutions(ByVal IDs() As Integer) As Solution()

        Dim i As Integer
        Dim solutions() As Solution

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

    'Optimierungsergebnis in mdb-Datei abspeichern
    '*********************************************
    Public Sub db_save(ByVal targetFile As String)

        Call db_init(targetFile)
        For Each iSolution As Solution In Me.Solutions
            Call db_insert(iSolution)
        Next

    End Sub

    'Datenbank vorbereiten
    '*********************
    Private Sub db_init(ByVal targetFile As String)

        'Ergebnisdatenbank anlegen
        '-------------------------

        'Pfad zur Vorlage
        Dim db_path_source As String = System.Windows.Forms.Application.StartupPath() & "\EVO.mdb"

        'Datei kopieren
        My.Computer.FileSystem.CopyFile(db_path_source, targetFile, True)

        'Pfad setzen
        Me.db_path = targetFile

        'Tabellen anpassen
        '-----------------

        'Allgemeine Anpassungen
        Call Me.db_prepare()
        'Methodenspezifische Anpassungen
        Select Case Me.Method
            Case "PES", "SensiPlot"
                Call Me.db_prepare_PES()
            Case "CES"
                Call Me.db_prepare_CES()
            Case "CES + PES", "Hybrid"
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

    'Verbindung zu Ergebnisdatenbank schließen
    '*****************************************
    Private Sub db_disconnect()
        db.Close()
    End Sub

    'Eine Lösung in die ErgebnisDB schreiben
    '***************************************
    Private Function db_insert(ByVal sol As Solution) As Boolean

        Call db_connect()

        Dim i As Integer

        Dim command As OleDbCommand = New OleDbCommand("", db)

        'Sim schreiben
        '-------------
        command.CommandText = "INSERT INTO Sim (ID, Name) VALUES (" & sol.ID & ", '" & Me.Datensatz & "')"
        command.ExecuteNonQuery()

        'QWerte schreiben 
        '----------------
        Dim fieldnames As String = ""
        Dim fieldvalues As String = ""
        For i = 0 To List_OptZiele.GetUpperBound(0)
            fieldnames &= ", [" & List_OptZiele(i).Bezeichnung & "]"
            fieldvalues &= ", " & sol.QWerte(i).ToString(Sim.FortranProvider)
        Next
        command.CommandText = "INSERT INTO QWerte (Sim_ID" & fieldnames & ") VALUES (" & sol.ID & fieldvalues & ")"
        command.ExecuteNonQuery()

        'Constraints schreiben 
        '---------------------
        If (Me.List_Constraints.GetLength(0) > 0) Then
            fieldnames = ""
            fieldvalues = ""
            For i = 0 To Me.List_Constraints.GetUpperBound(0)
                fieldnames &= ", [" & Me.List_Constraints(i).Bezeichnung & "]"
                fieldvalues &= ", " & sol.Constraints(i).ToString(Sim.FortranProvider)
            Next
            command.CommandText = "INSERT INTO [Constraints] (Sim_ID" & fieldnames & ") VALUES (" & sol.ID & fieldvalues & ")"
            command.ExecuteNonQuery()
        End If

        If (Me.Method = "PES" Or Me.Method = "CES + PES" Or Me.Method = "SensiPlot") Then

            'OptParameter schreiben
            '----------------------
            fieldnames = ""
            fieldvalues = ""
            For i = 0 To Me.List_OptParameter.GetUpperBound(0)
                fieldnames &= ", [" & Me.List_OptParameter(i).Bezeichnung & "]"
                fieldvalues &= ", " & sol.OptPara(i).ToString(Sim.FortranProvider)
            Next
            command.CommandText = "INSERT INTO OptParameter (Sim_ID" & fieldnames & ") VALUES (" & sol.ID & fieldvalues & ")"
            command.ExecuteNonQuery()

        End If

        'BUG 260: db_insert für CES

        'If (Me.Method = "CES" Or Me.Method = "CES + PES") Then

        '    'Pfad schreiben
        '    '--------------
        '    fieldnames = ""
        '    fieldvalues = ""
        '    For i = 0 To Me.List_Locations.GetUpperBound(0)
        '        fieldnames &= ", [" & Me.List_Locations(i).Name & "]"
        '        fieldvalues &= ", '" & sol.Akt(i).Measures(i) & "'"
        '    Next
        '    command.CommandText = "INSERT INTO Pfad (Sim_ID" & fieldnames & ") VALUES (" & sol.ID & fieldvalues & ")"
        '    command.ExecuteNonQuery()

        'End If

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
            Throw New Exception("Sekundäre Population nicht in DB vorhanden!")
        End If

        Return SekPop

    End Function

    'Einen Parametersatz aus der DB übernehmen
    '*****************************************
    Private Function db_getPara(ByRef Solution As Solution, ByVal xAchse As String, ByVal xWert As Double, ByVal yAchse As String, ByVal yWert As Double) As Boolean

        Dim isOK As Boolean = False
        Dim q As String
        Dim adapter As OleDbDataAdapter
        Dim ds As DataSet
        Dim numrows As Integer

        Solution = New Solution()
        ReDim Solution.OptPara(Me.List_OptParameter.GetUpperBound(0))

        Call db_connect()

        'Fallunterscheidung nach Methode
        Select Case Me.Method

            Case "PES", "SensiPlot"

                'Unterscheidung
                If (Me.Method = "SensiPlot") Then
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

                'Anzahl Übereinstimmungen überprüfen
                If (numrows = 0) Then
                    MsgBox("Es wurde keine Übereinstimmung in der Datenbank gefunden!", MsgBoxStyle.Exclamation, "Problem")
                    Return False
                ElseIf (numrows > 1) Then
                    MsgBox("Es wurden mehr als eine Entsprechung von OptParametern für den gewählten Punkt gefunden!" & Chr(13) & Chr(10) & "Es wird nur das erste Ergebnis verwendet!", MsgBoxStyle.Exclamation, "Problem")
                End If

                'OptParametersatz übernehmen
                For i As Integer = 0 To Me.List_OptParameter.GetUpperBound(0)
                    Solution.OptPara(i) = ds.Tables("OptParameter").Rows(0).Item(Me.List_OptParameter(i).Bezeichnung)
                Next

                isOK = True

                Return isOK

                'BUG 260: db_getPara für CES


                'Case "CES"

                '    q = "SELECT Pfad.* FROM Pfad INNER JOIN QWerte ON Pfad.Sim_ID = QWerte.Sim_ID WHERE (QWerte.[" & xAchse & "] = " & xWert & " AND QWerte.[" & yAchse & "] = " & yWert & ")"

                '    adapter = New OleDbDataAdapter(q, db)

                '    ds = New DataSet("EVO")
                '    numrows = adapter.Fill(ds, "Pfad")

                '    'Anzahl Übereinstimmungen überprüfen
                '    If (numrows = 0) Then
                '        MsgBox("Es wurde keine Übereinstimmung in der Datenbank gefunden!", MsgBoxStyle.Exclamation, "Problem")
                '        Return False
                '    ElseIf (numrows > 1) Then
                '        MsgBox("Es wurden mehr als eine Entsprechung von Pfaden für den gewählten Punkt gefunden!" & Chr(13) & Chr(10) & "Es wird nur das erste Ergebnis verwendet!", MsgBoxStyle.Exclamation, "Problem")
                '    End If

                '    'Pfad übernehmen
                '    For i As Integer = 0 To Me.Akt.Measures.GetUpperBound(0)
                '        Me.Akt.Measures(i) = ds.Tables("Pfad").Rows(0).Item(List_Locations(i).Name)
                '    Next

                '    'Bereitet das BlaueModell für die Kombinatorik vor
                '    Call Me.PREPARE_Evaluation_CES()


                'Case "CES + PES"

                '    q = "SELECT OptParameter.*, Pfad.* FROM (((Sim LEFT JOIN Constraints ON Sim.ID = Constraints.Sim_ID) INNER JOIN OptParameter ON Sim.ID = OptParameter.Sim_ID) INNER JOIN Pfad ON Sim.ID = Pfad.Sim_ID) INNER JOIN QWerte ON Sim.ID = QWerte.Sim_ID WHERE (QWerte.[" & xAchse & "] = " & xWert & " AND QWerte.[" & yAchse & "] = " & yWert & ")"

                '    adapter = New OleDbDataAdapter(q, db)

                '    ds = New DataSet("EVO")
                '    adapter.Fill(ds, "OptParameter_Pfad")

                '    'Anzahl Übereinstimmungen überprüfen
                '    numrows = ds.Tables("OptParameter_Pfad").Rows.Count

                '    If (numrows = 0) Then
                '        MsgBox("Es wurde keine Übereinstimmung in der Datenbank gefunden!", MsgBoxStyle.Exclamation, "Problem")
                '        Return False
                '    ElseIf (numrows > 1) Then
                '        MsgBox("Es wurden mehr als eine Entsprechung von OptParametern / Pfad für den gewählten Punkt gefunden!" & Chr(13) & Chr(10) & "Es wird nur das erste Ergebnis verwendet!", MsgBoxStyle.Exclamation, "Problem")
                '    End If

                '    'Pfad übernehmen
                '    For i As Integer = 0 To Me.Akt.Measures.GetUpperBound(0)
                '        Me.Akt.Measures(i) = ds.Tables("OptParameter_Pfad").Rows(0).Item(List_Locations(i).Name)
                '    Next

                '    'Bereitet das BlaueModell für die Kombinatorik vor
                '    Call Me.PREPARE_Evaluation_CES()

                '    'OptParametersatz übernehmen
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

    'Optimierungsergebnis aus einer DB lesen
    '***************************************
    Public Sub db_load(ByVal sourceFile As String)

        '---------------------------------------------------------------------------
        'Hinweise:
        'Die EVO-Eingabedateien müssen eingelesen sein und mit der DB übereinstimmen
        'Funktioniert momentan nur für PES
        '---------------------------------------------------------------------------

        Dim i, j As Integer

        'Pfad setzen
        Me.db_path = sourceFile

        'Connect
        Call db_connect()

        'Read
        Dim q As String
        Dim numSolutions, igen As Integer

        'Alle Lösungen
        q = "SELECT Sim.ID, OptParameter.*, QWerte.*, Constraints.* FROM ((Sim LEFT JOIN [Constraints] ON Sim.ID=Constraints.Sim_ID) INNER JOIN OptParameter ON Sim.ID=OptParameter.Sim_ID) INNER JOIN QWerte ON Sim.ID=QWerte.Sim_ID ORDER BY Sim.ID"

        Dim adapter As OleDbDataAdapter = New OleDbDataAdapter(q, db)

        Dim ds As New DataSet("EVO")
        numSolutions = adapter.Fill(ds, "PESResult")

        'Letzte SekPop-Generation bestimmen
        Try
            Dim command As New OleDbCommand("", db)
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

            Me.Solutions(i) = New Solution()

            With Me.Solutions(i)
                'ID
                '--
                .ID = ds.Tables(0).Rows(i).Item("Sim.ID")
                'OptParameter
                '------------
                ReDim .OptPara(Me.List_OptParameter.GetUpperBound(0))
                For j = 0 To Me.List_OptParameter.GetUpperBound(0)
                    .OptPara(j) = ds.Tables(0).Rows(i).Item(Me.List_OptParameter(j).Bezeichnung)
                Next
                'QWerte
                '------
                ReDim .QWerte(Me.List_OptZiele.GetUpperBound(0))
                For j = 0 To Me.List_OptZiele.GetUpperBound(0)
                    .QWerte(j) = ds.Tables(0).Rows(i).Item(Me.List_OptZiele(j).Bezeichnung)
                Next
                'Constraints
                '-----------
                ReDim .Constraints(Me.List_Constraints.GetUpperBound(0))
                For j = 0 To Me.List_Constraints.GetUpperBound(0)
                    .Constraints(j) = ds.Tables(0).Rows(i).Item(Me.List_Constraints(j).Bezeichnung)
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
