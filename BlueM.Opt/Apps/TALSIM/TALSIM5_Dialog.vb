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
Imports Microsoft.Data.Sqlite
Imports System.Windows.Forms

Friend Class TALSIM5_Dialog

    Private dbPath As String

    Friend ReadOnly Property SelectedScenario As Talsim5.Scenario
        Get
            Try
                Return CType(ComboBox_Scenario.SelectedItem, Talsim5.Scenario)
            Catch
                Return Nothing
            End Try
        End Get
    End Property

    Friend ReadOnly Property SelectedSimulation As Talsim5.Simulation
        Get
            Try
                Return CType(ComboBox_Simulation.SelectedItem, Talsim5.Simulation)
            Catch
                Return Nothing
            End Try
        End Get
    End Property

    Friend ReadOnly Property TimeseriesPath As String
        Get
            Return Me.TextBox_TimeseriesPath.Text.Trim()
        End Get
    End Property

    Public Sub New(dbPath As String)
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()
        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Me.dbPath = dbPath

        'show dtabase path
        Me.Label1.Text = dbPath

        'read scenarios from database
        Dim scenarios As New List(Of Talsim5.Scenario)
        Using connection As New SQLiteConnection($"Data Source={dbPath}")
            connection.Open()
            Using command As SQLiteCommand = connection.CreateCommand()
                command.CommandText = "
                    SELECT Id, Name
                    FROM Scenario
                    ORDER BY id
                "
                Using reader As SQLiteDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim id As Integer = reader.GetInt32(0)
                        Dim name As String = reader.GetString(1)
                        Dim scenario As New Talsim5.Scenario() With {
                            .Id = id,
                            .Name = name
                        }
                        scenarios.Add(scenario)
                    End While
                End Using
            End Using
            connection.Close()
        End Using

        Me.ComboBox_Scenario.Items.AddRange(scenarios.ToArray())
        Me.ComboBox_Scenario.SelectedIndex = 0

        ''Reset simulations
        'Me.ComboBox_Simulation.Items.Clear()
        'Me.ComboBox_Simulation.Enabled = False

    End Sub

    Private Sub ComboBox_Scenario_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_Scenario.SelectedIndexChanged
        'read simulations from database
        Dim simulations As New List(Of Talsim5.Simulation)
        Using connection As New SQLiteConnection($"Data Source={dbPath}")
            connection.Open()
            Using command As SQLiteCommand = connection.CreateCommand()
                command.CommandText = "
                    SELECT Id, Description
                    FROM Simulation
                    WHERE ScenarioId = @ScenarioId
                    ORDER BY id
                "
                command.Parameters.AddWithValue("@ScenarioId", Me.SelectedScenario.Id)
                Using reader As SQLiteDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim id As Integer = reader.GetInt32(0)
                        Dim name As String = reader.GetString(1)
                        Dim simulation As New Talsim5.Simulation() With {
                            .Id = id,
                            .Name = name
                        }
                        simulations.Add(simulation)
                    End While
                End Using
            End Using
            connection.Close()
        End Using

        Me.ComboBox_Simulation.Items.Clear()
        Me.ComboBox_Simulation.Items.AddRange(simulations.ToArray())
        Me.ComboBox_Simulation.SelectedIndex = 0
        'Me.ComboBox_Simulation.Enabled = True

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Me.TimeseriesPath <> "" Then
            Me.FolderBrowserDialog1.SelectedPath = Me.TimeseriesPath
        End If
        If Me.FolderBrowserDialog1.ShowDialog() <> DialogResult.OK Then
            Return
        End If
        Me.TextBox_TimeseriesPath.Text = Me.FolderBrowserDialog1.SelectedPath
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Me.SelectedScenario Is Nothing Then
            MessageBox.Show("Please select a scenario!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        If Me.SelectedSimulation Is Nothing Then
            MessageBox.Show("Please select a simulation!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        If Me.TimeseriesPath = "" Then
            MessageBox.Show("Please select a timeseries path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        ElseIf Not IO.Directory.Exists(Me.TimeseriesPath) Then
            MessageBox.Show("The selected timeseries path does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class
