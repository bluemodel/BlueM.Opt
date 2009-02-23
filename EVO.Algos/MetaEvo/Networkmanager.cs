using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Net;
using MySql.Data.MySqlClient;
using System.Threading;

namespace IHWB.EVO.MetaEvo
{
    public class Networkmanager
    {
        //### Variablen ###
        MySqlConnection mycon;
        MySqlCommand myCommand;
        MySqlDataReader myReader;
        EVO.Diagramm.Monitor monitor1;

        Network network1;

        int number_optparas;     //Anzahl Optimierungsparameter
        int number_constraints;  //Anzahl Constraints
        int number_objectives;   //Anzahl Objectivefunctionwerte (inkl. primary objectives !!!)

        //### Konstruktor ###
        public Networkmanager(ref EVO.Common.Individuum_MetaEvo individuum_input, ref EVO.Common.EVO_Settings settings_input, ref EVO.Common.Problem prob_input, ref EVO.Diagramm.Monitor monitor_input)
        {
            number_optparas = individuum_input.get_optparas().Length;
            number_constraints = individuum_input.Constraints.Length;
            number_objectives = individuum_input.Objectives.Length;
            monitor1 = monitor_input;

            myCommand = new MySqlCommand();

            //Server
            if (settings_input.MetaEvo.Role == "Network Server")
            {
                this.monitor1.LogAppend("Network Manager: Started in 'Network Server' Mode");
                mycon = new MySqlConnection("datasource=" + settings_input.MetaEvo.MySQL_Host + ";username=" + settings_input.MetaEvo.MySQL_User + ";password=" + settings_input.MetaEvo.MySQL_Password + ";database=information_schema");
                myCommand.Connection = mycon;

                if (this.DB_check_connection())
                {
                    DB_init(ref settings_input);  //Inklusive Server-Entry
                    network1 = new Network(ref mycon); //Verwaltung der Netzwerkstruktur
                }
            }
            // Client
            else
            {
                this.monitor1.LogAppend("Network Manager: Started in 'Network Client' Mode");
                mycon = new MySqlConnection("datasource=" + settings_input.MetaEvo.MySQL_Host + ";username=" + settings_input.MetaEvo.MySQL_User + ";password=" + settings_input.MetaEvo.MySQL_Password + ";database=" + settings_input.MetaEvo.MySQL_Database);
                myCommand.Connection = mycon;
                if (this.DB_check_connection())
                {
                    DB_client_entry_update(ref prob_input);
                }
            }
        }



        //### Methoden ### Initialisierung

        //(ok)Verbindung zur DB prüfen bzw. Fehler ausgeben
        private bool DB_check_connection()
        {
            try
            {
                mycon.Open();
                mycon.Close();
                this.monitor1.LogAppend("Network Manager: DB-Connection Successfully");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Function 'DB_check_connection': \r\nFehler beim Verbinden mit der Datenbank: \r\n" + (ex.Message), "MetaEvo - Networkmanager");
                return false;
            }
            return true;
        }

        //(ok)Datenbank erzeugen
        private void DB_init(ref EVO.Common.EVO_Settings settings_input)   
        {
            //Datenbank
            myCommand.CommandText = "CREATE DATABASE IF NOT EXISTS " + settings_input.MetaEvo.MySQL_Database;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            //Wechseln in neue Datenbank
            myCommand.Connection.ConnectionString = "datasource=" + settings_input.MetaEvo.MySQL_Host + ";username=" + settings_input.MetaEvo.MySQL_User + ";password=" + settings_input.MetaEvo.MySQL_Password + ";database=" + settings_input.MetaEvo.MySQL_Database;

            //benötigte Tabellen: "metaevo_network" 
            myCommand.CommandText = "DROP TABLE IF EXISTS `metaevo_network`;";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            myCommand.CommandText = "CREATE TABLE IF NOT EXISTS `metaevo_network` (`ipName` varchar(15) NOT NULL,`type` varchar(20) NOT NULL,`status` varchar(30) NOT NULL,`timestamp` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,`speed_av` double,`speed_low` double ,PRIMARY KEY  (`ipName`)) ENGINE=MyISAM DEFAULT CHARSET=utf8;";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            //Eintrag für Server
            myCommand.CommandText = "INSERT INTO `metaevo_network` (`ipName`, `type`, `status`) VALUES ('" + Dns.GetHostName() + "', 'server', 'generate Individuums');";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            //benötigte Tabellen: "metaevo_individuums" 
            myCommand.CommandText = "DROP TABLE IF EXISTS `metaevo_individuums`;";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            string tmptxt = "CREATE TABLE IF NOT EXISTS `metaevo_individuums` (`id` int(11) NOT NULL,`status` varchar(12) NOT NULL,";
            for (int k = 1; k <= number_optparas; k++)
            {
                tmptxt = tmptxt + "`opt" + k + "` double NOT NULL,";
            }
            for (int k = 1; k <= number_constraints; k++)
            {
                tmptxt = tmptxt + "`const" + k + "` double NOT NULL default '0',";
            }
            for (int k = 1; k <= number_objectives; k++)
            {
                tmptxt = tmptxt + "`feat" + k + "` double NOT NULL default '0',";
            }
            tmptxt = tmptxt + "`ipName` varchar(15) NOT NULL, PRIMARY KEY  (`id`)) ENGINE=MyISAM DEFAULT CHARSET=utf8;";

            myCommand.CommandText = tmptxt;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            //benötigte Tabellen: "metaevo_final_individuums" 
            myCommand.CommandText = "DROP TABLE IF EXISTS `metaevo_final_individuums`;";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            tmptxt = "CREATE TABLE IF NOT EXISTS `metaevo_final_individuums` (`id` int(11) NOT NULL,";
            for (int k = 1; k <= number_optparas; k++)
            {
                tmptxt = tmptxt + "`opt" + k + "` double NOT NULL,";
            }
            for (int k = 1; k <= number_constraints; k++)
            {
                tmptxt = tmptxt + "`const" + k + "` double NOT NULL default '0',";
            }
            for (int k = 1; k <= number_objectives; k++)
            {
                tmptxt = tmptxt + "`feat" + k + "` double NOT NULL default '0',";
            }
            tmptxt = tmptxt + " PRIMARY KEY  (`id`)) ENGINE=MyISAM DEFAULT CHARSET=utf8;";

            myCommand.CommandText = tmptxt;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            //benötigte Tabellen: "metaevo_infos"
            myCommand.CommandText = "DROP TABLE IF EXISTS `metaevo_infos`;";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            tmptxt = "CREATE TABLE IF NOT EXISTS `metaevo_infos` (`id` int(11) NOT NULL auto_increment,`bezeichnung` varchar(50) NOT NULL,`wert` varchar(50) NOT NULL, PRIMARY KEY  (`id`)) ENGINE=MyISAM DEFAULT CHARSET=utf8 AUTO_INCREMENT=0 ;";

            myCommand.CommandText = tmptxt;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            this.monitor1.LogAppend("Network Manager: DB-Construction Successfully");
        }

        //(ok)sich als Client in DB eintragen
        private void DB_client_entry(ref EVO.Common.Problem prob_input) {
            //Prüfen ob Problem des Clients und des Servers konsistent sind
            if (prob_input.Datensatz == DB_get_info("Datensatz"))
            {
                //Eintrag für Client
                myCommand.CommandText = "INSERT INTO `metaevo_network` (`ipName`, `type`, `status`, `speed_av`, `speed_low`) VALUES ('" + Dns.GetHostName() + "', 'client', 'ready', '1000', '1000');";

                try
                {
                    myCommand.Connection.Open();
                    myCommand.ExecuteNonQuery();
                    myCommand.Connection.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Function 'DB_client_entry': \r\nFehler beim Schreiben in die Datenbank: \r\n" + (ex.Message), "MetaEvo - Networkmanager");
                }
            }
            else
            {
                MessageBox.Show("Function 'DB_client_entry': \r\nFalscher Problemdatensatz! '" + prob_input.Datensatz + "'(Client) != '" + DB_get_info("Datensatz") + "'(Server)", "MetaEvo - Networkmanager");
            }
        }

        //(ok)Client in DB updaten
        public void DB_client_entry_update(ref EVO.Common.Problem prob_input)
        {
            int affectedrows = 0;
            //Eintrag für Client
            //Zur aktualisierung des Timestamp 
            myCommand.CommandText = "UPDATE `metaevo_network` SET `status`= 'ready2' WHERE ipName = '" + Dns.GetHostName() + "' LIMIT 1; UPDATE `metaevo_network` SET `status`= 'ready' WHERE ipName = '" + Dns.GetHostName() + "' LIMIT 1;";

            try
            {
                myCommand.Connection.Open();
                affectedrows = myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Function 'DB_client_entry_update': \r\nFehler beim Schreiben in die Datenbank: \r\n" + (ex.Message), "MetaEvo - Networkmanager");
            }
            if (affectedrows == 0)
            {
                this.monitor1.LogAppend("Network Manager: Server-restart detected, creating new entry...");
                DB_client_entry(ref prob_input);
            }
        }

        //(ok)Individuum-Datenbank säubern
        private void DB_ClearIndividuumsTable()
        {
            myCommand.CommandText = "TRUNCATE TABLE `metaevo_individuums`";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }

        //(ok)Datenbank löschen
        public void DB_delete(ref EVO.Common.EVO_Settings settings_input)
        {
            myCommand.CommandText = "DROP DATABASE IF EXISTS " + settings_input.MetaEvo.MySQL_Database;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }

        //Eigenschaften setzen
        public void DB_set_info(string bezeichnung, string wert)
        {
            myCommand.CommandText = "UPDATE `metaevo_infos` SET `wert`= '" + wert + "' WHERE bezeichnung = '" + bezeichnung + "' LIMIT 1; ";
            int affectedrows = 0;

            try
            {
                myCommand.Connection.Open();
                affectedrows = myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Function 'DB_set_info': \r\nFehler beim setzen einer Eigenschaft in der DB: \r\n" + (ex.Message), "MetaEvo - Networkmanager");
            }
            
            if (affectedrows == 0) 
            {
                myCommand.CommandText = "INSERT INTO `metaevo_infos` (`bezeichnung` ,`wert`) VALUES ('" + bezeichnung + "', '" + wert + "');";
                try
                {
                    myCommand.Connection.Open();
                    myCommand.ExecuteNonQuery();
                    myCommand.Connection.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Function 'DB_set_info': \r\nFehler beim setzen einer Eigenschaft in der DB: \r\n" + (ex.Message), "MetaEvo - Networkmanager");
                }
            }
        }

        //Eigenschaften lesen
        public string DB_get_info(string bezeichnung)
        {
            myCommand.CommandText = "Select * from metaevo_infos WHERE bezeichnung = '" + bezeichnung + "' LIMIT 1";
            string back = "";
            
            try
            {
                myCommand.Connection.Open();
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    back = myReader.GetString(2);    
                }
                myReader.Close();
                myCommand.Connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Function 'Individuum_ReadFromDB_Client': \r\nFehler beim Lesen der Datenbank:\r\n " + (ex.Message), "MetaEvo - Networkmanager");
            }

            return back;
        }

        //### Methoden ### Working Process -> Individuums 

        //(ok)Neue Individuen in die Datenbank einfügen (ID, status, optparas, ipName)
        public int Individuums_WriteToDB(ref EVO.Common.Individuum_MetaEvo[] generation_input)
        {
            int number_tosimulate = 0;

            //Alte Individuen aus DB löschen
            this.DB_ClearIndividuumsTable();

            //Schreiben der Individuen-Daten in die DB (alle gleichzeitig)
            string tmptxt = "INSERT INTO `metaevo_individuums` (`id` #`status` #";

            //Datenmaske
            for (int k = 1; k <= number_optparas; k++)
            {
                tmptxt = tmptxt + "`opt" + k + "`#";
            }

            tmptxt = tmptxt + "`ipName`) VALUES ";

            //Datensätze
            for (int j = 0; j < generation_input.Length; j++)
            {
                if (generation_input[j].get_toSimulate())
                {
                    tmptxt = tmptxt + "('" + generation_input[j].ID + "'#'raw'#";
                    for (int k = 0; k < number_optparas; k++)
                    {
                        tmptxt = tmptxt + "'" + generation_input[j].get_optparas()[k] + "'#";
                    }
                    tmptxt = tmptxt + "'" + generation_input[j].get_client() + "')#";
                    number_tosimulate++;
                }
            }
            //Bevor falscher DB-Eintrag gesendet wird
            if (number_tosimulate == 0) return 0;

            //In der DB müssen Kommawerte mit . angegeben werden
            tmptxt = tmptxt.Replace(",",".").Replace("#",",").TrimEnd(',') + ";";

            myCommand.CommandText = tmptxt;
            try
            {
                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Function 'Individuums_WriteToDB': \r\nFehler beim Schreiben in die Datenbank: \r\n" + (ex.Message), "MetaEvo - Networkmanager");
            }
            return number_tosimulate;
        }

        //(ok)Neue Individuen in die Speicher-DB einfügen (ID, status, optparas, objectives, constraints)
        private void Individuums_StoreFinalInDB(ref EVO.Common.Individuum_MetaEvo[] generation_input)
        {
            int number_tosimulate = 0;
            //Schreiben der Individuen-Daten in die DB (alle gleichzeitig)
            string tmptxt = "INSERT INTO `metaevo_final_individuums` (`id` #";

            //Datenmaske
            for (int k = 1; k <= number_optparas; k++)
            {
                tmptxt = tmptxt + "`opt" + k + "`#";
            }
            for (int k = 1; k <= number_objectives; k++)
            {
                tmptxt = tmptxt + "`feat" + k + "`#";
            }
            for (int k = 1; k <= number_constraints; k++)
            {
                tmptxt = tmptxt + "`const" + k + "`#";
            }

            tmptxt = tmptxt + ") VALUES ";

            //Datensätze
            for (int j = 0; j < generation_input.Length; j++)
            {
                if (generation_input[j].get_toSimulate())
                {
                    tmptxt = tmptxt + "('" + generation_input[j].ID + "'#";
                    for (int k = 0; k < number_optparas; k++)
                    {
                        tmptxt = tmptxt + "'" + generation_input[j].get_optparas()[k] + "'#";
                    }
                    for (int k = 0; k < number_objectives; k++)
                    {
                        tmptxt = tmptxt + "'" + generation_input[j].Objectives[k] + "'#";
                    }
                    for (int k = 0; k < number_constraints; k++)
                    {
                        tmptxt = tmptxt + "'" + generation_input[j].Constraints[k] + "'#";
                    }
                    tmptxt = tmptxt + ")#";
                    number_tosimulate++;
                }
            }
            if (number_tosimulate != 0) 
            {
                tmptxt = tmptxt.Replace(",", ".").Replace("#", ",").Replace(",)", ")").TrimEnd(',') + ";";

                myCommand.CommandText = tmptxt;
                try
                {
                    myCommand.Connection.Open();
                    myCommand.ExecuteNonQuery();
                    myCommand.Connection.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Function 'Individuums_StoreFinalInDB': \r\nFehler beim Schreiben in die Datenbank: \r\n" + (ex.Message), "MetaEvo - Networkmanager");
                }
            }
        }
        
        //(ok)nächstes eigenes Individuum aus der DB lesen (ID, optparas)
        public void Individuum_ReadFromDB_Client(ref EVO.Common.Individuum_MetaEvo individuum_input)
        {
            myCommand.CommandText = "Select * from metaevo_individuums WHERE ipName = '" + Dns.GetHostName() + "' AND (status = 'raw' OR status = 'calculate') LIMIT 1";
            try
            {
                myCommand.Connection.Open();
                myReader = myCommand.ExecuteReader();

                //Aus der DB lesen und in Individuum speichern
                int ExportPosition = 0;

                double[] optparas = new double[number_optparas];

                while (myReader.Read())
                {
                    individuum_input.ID = myReader.GetInt32(ExportPosition);
                    ExportPosition += 2;

                    for (int k = 0; k < number_optparas; k++)
                    {
                        optparas[k] = myReader.GetDouble(ExportPosition);
                        ExportPosition++;
                    }
                    individuum_input.set_status("raw");
                }
                individuum_input.set_optparas(optparas);


                myReader.Close();
                myCommand.Connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Function 'Individuum_ReadFromDB_Client': \r\nFehler beim Lesen der Datenbank:\r\n " + (ex.Message), "MetaEvo - Networkmanager");
            }
        }

        //(ok)prüfen wie viele Individuen fertig berechnet sind
        private int Individuums_CountReadyInDB()
        {
            int count = 0;

            myCommand.CommandText = "Select status from metaevo_individuums WHERE status = 'true' OR status = 'false'";
            try
            {
                myCommand.Connection.Open();
                myReader = myCommand.ExecuteReader();
                
                while (myReader.Read())
                {
                    count++;
                }
                myReader.Close();
                myCommand.Connection.Close();  
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Function 'Individuums_CountReadyInDB': \r\nFehler beim Lesen der Datenbank: \r\n" + (ex.Message), "MetaEvo - Networkmanager");
            }
            return count;
        }
        
        //(ok)alle Individuen aus der DB in der Generation updaten (ID, status, constraints, objectives, ipName)
        private void Individuums_UpdateFromDB(ref EVO.Common.Individuum_MetaEvo[] generation_input)
        {
            myCommand.CommandText = "Select * from metaevo_individuums";
            try
            {
                myCommand.Connection.Open();
                myReader = myCommand.ExecuteReader();

                int exportPosition;
                int individuumcounter = 0;

                double[] constraints = new double[number_constraints];
                double[] objectives = new double[number_objectives];

                //Aus der DB lesen und in Individuum speichern
                while (myReader.Read())
                {
                    //Passendes Individuum zum DB-Export in der Individuentabelle finden (id-Vergleich)
                    for (int k = 0; k < generation_input.Length; k++)
                    {
                        if (generation_input[k].ID == myReader.GetInt32(0))
                        {
                            individuumcounter = k;
                        }
                    }

                    //Status setzen
                    exportPosition = 1;
                    generation_input[individuumcounter].set_status(myReader.GetString(exportPosition));

                    //Optparameter überspringen
                    exportPosition += (this.number_optparas + 1);

                    //Constraints setzen
                    for (int k = 0; k < number_constraints; k++)
                    {
                        generation_input[individuumcounter].Constraints[k] = myReader.GetDouble(exportPosition);
                        exportPosition++;
                    }

                    //Objectives setzen
                    for (int k = 0; k < number_objectives; k++)
                    {
                        generation_input[individuumcounter].Objectives[k] = myReader.GetDouble(exportPosition);
                        exportPosition++;
                    }

                    //Client setzen
                    generation_input[individuumcounter].set_Client(myReader.GetString(exportPosition));
                }

                myReader.Close();
                myCommand.Connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Function 'Individuums_UpdateFromDB': \r\nFehler beim Lesen der Datenbank: \r\n" + (ex.Message), "MetaEvo - Networkmanager");
            }
        }

        //(ok)bekanntes Individuum in DB updaten (abhängig vom Eingabeparameter) //Options: status, opt, feat, const, ipName
        public void Individuum_UpdateInDB(ref EVO.Common.Individuum_MetaEvo individuum_input, string fields2update, string status_input)
        {
            //Individuum in DB updaten 
            string tmptxt = "UPDATE `metaevo_individuums` SET ";
            string status = status_input;
            
            //status
            if (fields2update.Contains("status"))
            {
                tmptxt = tmptxt + "status = '" + status_input + "': ";
            }
            //opt
            if (fields2update.Contains("opt")) 
            {
                for (int k = 0; k < number_constraints; k++)
                {
                    tmptxt = tmptxt + "opt" + (k + 1) + " = '" + individuum_input.get_optparas()[k] + "': ";
                }
            }
            //feat
            if (fields2update.Contains("feat")) 
            {
                for (int k = 0; k < number_objectives; k++)
                {
                    tmptxt = tmptxt + "feat" + (k + 1) + " = '" + individuum_input.Objectives[k] + "': ";
                }
            }
            //const
            if (fields2update.Contains("const")) 
            {
                for (int k = 0; k < number_constraints; k++)
                {
                    tmptxt = tmptxt + "const" + (k + 1) + " = '" + individuum_input.Constraints[k] + "': ";
                }
            }
            if (fields2update.Contains("ipName"))
            {
                tmptxt = tmptxt + "ipName = '" + individuum_input.get_client() + "' ";
            }
            tmptxt = tmptxt.TrimEnd(':', ' ').Replace(",", ".").Replace(":", ",") + " WHERE id = '" + individuum_input.ID + "' LIMIT 1;"; ;

            myCommand.CommandText = tmptxt;
            try
            {
                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Function 'Individuum_UpdateInDB': \r\nFehler beim Schreiben in die Datenbank: \r\n" + (ex.Message), "MetaEvo - Networkmanager");
            }
        }


 
        //### Methoden ### Working Process -> Network Functions for Clients
        
        //(ok)Status von Server lesen // Rückgabe stringarray[1][status, timestamp]
        public string[] Network_ReadServer()
        {
            string[] back = new string[2];
            myCommand = new MySqlCommand("Select status, timestamp from metaevo_network WHERE type = 'server' LIMIT 1", mycon);
            try
            {
                myCommand.Connection.Open();
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    back[0] = myReader.GetString(0);
                    back[1] = myReader.GetString(1);
                }
                myReader.Close();
                myCommand.Connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Function 'Network_ReadServer': \r\nFehler beim Lesen der Datenbank: \r\n" + (ex.Message), "MetaEvo - Networkmanager");
            }

            return back;
        }

        //(ok)Client-Individuum initialisieren
        public Client Network_Init_Client_Object(string ipName_input)
        {
            Client back = new Client(ref mycon, ipName_input, "ready", DateTime.Now, 1000, 1000, 0);
            return back;
        }



        //### Methoden ### Working Process -> Scheduling

        //Scheduling 
        private void scheduling_new(ref EVO.Common.Individuum_MetaEvo[] generation_input)
        {
            int current_client;

            //Statustabelle der Clients aktualisieren
            network1.update_From_DB();

            //Falls kein Aktiver Client vorhanden ist, 3 Sekunden warten und dann nochmal aufrufen
            if (network1.number_clients == 0)
            {
                this.monitor1.LogAppend("Scheduling: No Client found registered in DB - wait...");
                System.Threading.Thread.Sleep(3000);
                scheduling_new(ref generation_input);
            }
            else
            {
                 this.monitor1.LogAppend("Networkmanager: New Scheduling");

                //Scheduling berechnen
                //Für jedes Individuum
                for (int k = 0; k < generation_input.Length; k++)
                {
                    if (generation_input[k].get_toSimulate())
                    {
                        current_client = 0;
                        //Alle Clients durchgehen
                        for (int j = 1; j < network1.number_clients; j++)
                        {
                            //Falls Client schneller 
                            if ((network1.Clients[j].speed_av * (network1.Clients[j].numberindividuums + 1)) < (network1.Clients[current_client].speed_av * (network1.Clients[current_client].numberindividuums + 1)))
                            {
                                current_client = j;
                            }
                        }
                        //In der current_calc_time-Eigenschaft Zeiten addieren, Individuum addieren
                        network1.Clients[current_client].numberindividuums++;
                        //Dem Individuum den Client(=ipName) zuweisen
                        generation_input[k].set_Client(network1.Clients[current_client].ipName);
                    }
                }

                //Ausgabe der Zuteilung
                for (int i = 0; i < network1.Clients.Length; i++)
                {
                     //this.monitor1.LogAppend("Networkmanager: New Scheduling: Client '" + network1.Clients[i].ipName + "' " + network1.Clients[i].numberindividuums + " Individuums");
                }
            }
        }

        //Scheduling
        private void scheduling_continue(ref EVO.Common.Individuum_MetaEvo[] generation_input)
        {
            int current_client;
            bool scheduling_error = false;

            //Statustabelle der Clients aktualisieren
            scheduling_error = network1.update_From_DB();

            //Falls kein Aktiver Client vorhanden ist, 3 Sekunden warten und dann scheduling_new aufrufen
            if (network1.number_clients == 0)
            {
                this.monitor1.LogAppend("Scheduling (new): No Client found registered in DB - waiting...");
                System.Threading.Thread.Sleep(3000);
                scheduling_new(ref generation_input);
            }

            //An den Daten der Clients hat sich etwas geändert (Neues Individuum, Speed-av hat sich um mehr als 5% geändert oder speed-low ist um 20% überschritten)
            else if (scheduling_error)   
            {
                this.monitor1.LogAppend("Networkmanager: Adapting Scheduling...");

                //tmp-array für schlussendlich die Sollwerte der Clients
                //  Client: [Client][0:vergangene Rechenzeit beim aktuellen Individuum + Rechenzeit für neue Individuen, (-1 = Error)
                //                   1:bereits berechnete Individuen + zu berechnende Individuen]
                double[,] current_status = new double[network1.Clients.Length, 2]; 
                

                //## Vorbereitung ##
                for (int j = 0; j < network1.Clients.Length; j++)
                {
                    if (network1.Clients[j].status == "error")
                    {
                        current_status[j, 0] = -1;

                        for (int i = 0; i < generation_input.Length; i++)
                        {
                            if ((generation_input[i].get_client() == network1.Clients[j].ipName) && (generation_input[i].get_toSimulate()))
                            {
                                //Calculate-Individuen von error-Clients wieder freigeben
                                if (generation_input[i].get_status() == "calculate")
                                {
                                    generation_input[i].set_status("raw");
                                }

                                //tmp-array füllen (1:bereits berechnete Individuen)
                                if ((generation_input[i].get_status() == "true") || (generation_input[i].get_status() == "false"))
                                {
                                    current_status[j, 1]++;
                                }

                                //Bisher zugeordnete Individuen zählen
                                network1.Clients[j].numberindividuums++;
                            }
                        }
                    }
                    else
                    {
                        //Alte Individuenzuordnungen aus der generation auslesen und im netzwerk setzen (numberindividuums in network1 ist bereits für alle clients auf 0)
                        for (int i = 0; i < generation_input.Length; i++)
                        {
                            if ((generation_input[i].get_client() == network1.Clients[j].ipName) && (generation_input[i].get_toSimulate()))
                            {
                                //tmp-array füllen (0:vergangene Rechenzeit beim aktuellen Individuum)
                                if (generation_input[i].get_status() == "calculate")
                                {
                                    current_status[j, 0] = (DateTime.Now.Subtract(network1.Clients[j].timestamp)).TotalMilliseconds;
                                }

                                //tmp-array füllen (1:bereits berechnete Individuen)
                                if ((generation_input[i].get_status() == "true") || (generation_input[i].get_status() == "false"))
                                {
                                    current_status[j,1]++;
                                }

                                //Bisher zugeordnete Individuen zählen
                                network1.Clients[j].numberindividuums++;
                            }
                        }
                    }
                }

                //## Durchführung ##
                bool init = true;
                current_client = 0;
                //Scheduling theoretisch
                for (int i = 0; i < generation_input.Length; i++) 
                {
                    if ((generation_input[i].get_status() == "raw") && (generation_input[i].get_toSimulate()))
                    {
                        init = true;
                        for (int j = 0; j < network1.number_clients; j++)
                        {
                            //falls Client funktioniert
                            if (network1.Clients[j].status != "error")
                            {
                                if (init)
                                {
                                    current_client = j;
                                    init = false;
                                }
                                else
                                {
                                    //Falls schnellerer Client 
                                    if (current_status[j, 0] + network1.Clients[j].speed_av < (current_status[current_client, 0] + network1.Clients[current_client].speed_av))
                                    {
                                        current_client = j;
                                    }
                                }
                            }
                        }
                        //In der current_calc_time-Eigenschaft Zeiten addieren, Individuum addieren
                        current_status[current_client, 0] += network1.Clients[current_client].speed_av;
                        current_status[current_client, 1]++;
                    }
                }

                //Scheduling in DB umsetzen
                //Falls Sollwert geringer als Istwert, Individuum in DB demarkieren
                for (int i = 0; i < network1.Clients.Length; i++)
                {
                    int j = 0;
                    while ((current_status[i, 1] < network1.Clients[i].numberindividuums) && (j < generation_input.Length))
                    {
                        if ((generation_input[j].get_status() == "raw") && (generation_input[j].get_client() == network1.Clients[i].ipName) &&  (generation_input[j].get_toSimulate()))
                        {
                            generation_input[j].set_Client("");
                            network1.Clients[i].numberindividuums--;
                        }
                        j++; 
                    } 
                }
                //Scheduling in DB umsetzen
                //Falls Sollwert höher als Istwert, Individuum in DB markieren
                for (int i = 0; i < network1.Clients.Length; i++)
                {
                    int j = 0;
                    while (current_status[i, 1] > network1.Clients[i].numberindividuums)
                    {
                        if ((generation_input[j].get_client() == "") && (generation_input[j].get_toSimulate()))
                        {
                            generation_input[j].set_Client(network1.Clients[i].ipName);
                            this.Individuum_UpdateInDB(ref generation_input[j], "ipName", "");
                            network1.Clients[i].numberindividuums++;
                        }
                        j++;
                    }
                }

                //Ausgabe der neuen Zuteilung
                for (int i = 0; i < network1.Clients.Length; i++)
                {
                    //this.monitor1.LogAppend("Networkmanager: Adapted Scheduling: Client '" + network1.Clients[i].ipName + "' " + network1.Clients[i].numberindividuums + " Individuums");
                }
            }
        }



        //### Hauptprogramm ### Berechnet eine Generation im Netzwerk

        public bool calculate_by_clients(ref EVO.Common.Individuum_MetaEvo[] generation_input, ref EVO.Diagramm.Hauptdiagramm hauptdiagramm_input, ref EVO.Common.Progress progress_input, ref bool stopped)
        {
            int individuums_ready = 0;
            int individuums_ready_now = 0;
            int number_tosimulate = 0;

            //Scheduling initialisieren und ersten Schritt rechnen
            this.scheduling_new(ref generation_input);

            //In die Datenbank schreiben
            this.monitor1.LogAppend("Networkmanager: Write Individuums to DB");
            this.DB_ClearIndividuumsTable();
            number_tosimulate = this.Individuums_WriteToDB(ref generation_input);

            //Warten bis erste Ergebnisse vorliegen müssten
            this.monitor1.LogAppend("Networkmanager: Waiting for first Results...");

            //Prüfen ob alle Individuen fertig berechnet sind
            while (individuums_ready_now < number_tosimulate)
            {
                if (stopped) break;

                if (individuums_ready < individuums_ready_now) 
                {
                    progress_input.iNachf = (short)individuums_ready_now;

                    //Fertige Individuen wieder auslesen
                    this.Individuums_UpdateFromDB(ref generation_input);

                    //Neue fertige Individuen zeichnen
                    for (int i = 0; i < generation_input.Length; i++)
                    {
                        if ((generation_input[i].get_status() == "true") || (generation_input[i].get_status() == "false"))
                        {
                            hauptdiagramm_input.ZeichneIndividuum(generation_input[i], 1, 1, 1, generation_input[i].ID % generation_input.Length, System.Drawing.Color.Orange, false);
                            System.Windows.Forms.Application.DoEvents();
                        }
                    }

                    this.monitor1.LogAppend("Networkmanager: " + Math.Round((double)individuums_ready_now / ((double)number_tosimulate), 2) * 100 + "%");

                    individuums_ready = individuums_ready_now;
                }
               
                //Scheduling-korrektur anstossen (inklusive-DB Update der Änderungen)
                this.scheduling_continue(ref generation_input);

                //Warten
                System.Threading.Thread.Sleep(1000);

                individuums_ready_now = this.Individuums_CountReadyInDB();
            }

            //In speicher-DB schreiben
            this.Individuums_UpdateFromDB(ref generation_input);
            this.Individuums_StoreFinalInDB(ref generation_input);

            return true;
        }
    }
}