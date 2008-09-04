using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Net;
using MySql.Data.MySqlClient;

namespace IHWB.EVO.HybridAlgo
{
    public class Networkmanager
    {
        //### Variablen ###
        MySqlConnection mycon;
        MySqlCommand myCommand;
        MySqlDataReader myReader;       

        int number_optparas;
        int number_constraints;  //Anzahl constraints
        int number_features;    //Anzahl Featurefunktionswerte (inkl. Penalties)

        int[,] calctimes;   //Scheduling-Tabelle

        //### Konstruktor ###
        public Networkmanager(ref EVO.Common.Individuum_MetaEvo individuum_input, ref EVO.Common.EVO_Settings settings_input)
        {
            number_optparas = individuum_input.get_optparas().Length;
            number_constraints = individuum_input.Constraints.Length;
            number_features = individuum_input.Features.Length;

            myCommand = new MySqlCommand();

            //Server
            if (settings_input.MetaEvo.Role == "Network Server")
            {
                mycon = new MySqlConnection("datasource=" + settings_input.MetaEvo.MySQL_Host + ";username=" + settings_input.MetaEvo.MySQL_User + ";password=" + settings_input.MetaEvo.MySQL_Password + ";database=information_schema");
                myCommand.Connection = mycon;

                if (this.DB_check_connection())
                {
                    DB_init(ref settings_input);  //Inklusive Server-Entry
                }
            }
            // Client
            else
            {
                mycon = new MySqlConnection("datasource=" + settings_input.MetaEvo.MySQL_Host + ";username=" + settings_input.MetaEvo.MySQL_User + ";password=" + settings_input.MetaEvo.MySQL_Password + ";database=" + settings_input.MetaEvo.MySQL_Database);
                myCommand.Connection = mycon;
                if (this.DB_check_connection())
                {
                    DB_client_entry();
                }
            }
        }



        //### Methoden ### Initialisierung

        //Verbindung zur DB prüfen bzw. Fehler ausgeben
        private bool DB_check_connection()
        {
            try
            {
                mycon.Open();
                mycon.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message,"MySQL Database");
                return false;
            }
            return true;
        }

        //Datenbank erzeugen
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

            myCommand.CommandText = "CREATE TABLE IF NOT EXISTS `metaevo_network` (`ipName` varchar(15) NOT NULL,`type` varchar(20) NOT NULL,`status` varchar(20) NOT NULL,`timestamp` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,`speed-index` varchar(20) NOT NULL,`lowest speed` varchar(20) NOT NULL,PRIMARY KEY  (`ipName`)) ENGINE=MyISAM DEFAULT CHARSET=utf8;";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            //Eintrag für Server
            myCommand.CommandText = "INSERT INTO `metaevo_network` (`ipName`, `type`, `status`, `timestamp`, `speed-index`, `lowest speed`) VALUES ('" + Dns.GetHostName() + "', 'server', 'generate Individuums', '', '0', '0');";
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
                tmptxt = tmptxt + "`const" + k + "` double default NULL,";
            }
            for (int k = 1; k <= number_features; k++)
            {
                tmptxt = tmptxt + "`feat" + k + "` double default NULL,";
            }
            tmptxt = tmptxt + "`ipName` varchar(15) NOT NULL, PRIMARY KEY  (`id`)) ENGINE=MyISAM DEFAULT CHARSET=utf8;";

            myCommand.CommandText = tmptxt;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }

        //sich als Client in DB eintragen
        private void DB_client_entry() {
            //Eintrag für Client
            myCommand.CommandText = "INSERT INTO `metaevo_network` (`ipName`, `type`, `status`, `timestamp`, `speed-index`, `lowest speed`) VALUES ('" + Dns.GetHostName() + "', 'client', 'ready', '', '0', '0');";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }

        //Individuum-Datenbank säubern
        private void DB_ClearIndividuumsTable()
        {
            myCommand.CommandText = "TRUNCATE TABLE `metaevo_individuums`";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }

        //Datenbank löschen
        private void DB_delete(ref EVO.Common.EVO_Settings settings_input)
        {
            myCommand.CommandText = "DROP DATABASE IF EXISTS " + settings_input.MetaEvo.MySQL_Database;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }



        //### Methoden ### Working Process -> Individuums 

        //Individuen in die Datenbank einfügen (ID, optparas, ipName)
        public void Individuums_WriteToDB(EVO.Common.Individuum_MetaEvo[] generation_input)
        {
            //Alte Individuen aus DB löschen
            this.DB_ClearIndividuumsTable();

            //Schreiben der Individuen-Daten in die DB (alle gleichzeitig)
            string tmptxt = "INSERT INTO `metaevo_individuums` (`id` ,`status` ,";

            //Datenmaske
            for (int k = 1; k <= number_optparas; k++)
            {
                tmptxt = tmptxt + "`opt" + k + "`,";
            }

            tmptxt = tmptxt + "`ipName`) VALUES ";

            //Datensätze
            for (int j = 0; j < generation_input.Length; j++)
            {
                tmptxt = tmptxt + "(" + generation_input[j].ID + ", 'raw',";
                for (int k = 1; k <= number_optparas; k++)
                {
                    tmptxt = tmptxt + generation_input[j].get_optparas()[k] + ",";
                }
                tmptxt = tmptxt + ", " + generation_input[j].get_Client() + "),";
            }
            tmptxt = tmptxt.TrimEnd(',') + ";";

            myCommand.CommandText = tmptxt;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }
        
        //nächstes eigenes Individuum aus der DB lesen (constraints, features, ipName)
        public EVO.Common.Individuum_MetaEvo Individuum_ReadFromDB()
        {
            myCommand = new MySqlCommand("Select * from metaevo_individuums WHERE ipName = " + Dns.GetHostName() + " LIMIT 1", mycon);
            mycon.Open();
            myReader = myCommand.ExecuteReader();

            //Aus der DB lesen und in Individuum speichern
            int ExportPosition = 0;
            EVO.Common.Individuum_MetaEvo individuumnew = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", 1);

            double[] constraints = new double[number_constraints];
            double[] features = new double[number_features];

            while (myReader.Read())
            {
                individuumnew.ID = myReader.GetInt32(ExportPosition);
                ExportPosition += (this.number_optparas + 2); //Optparameter überspringen

                for (int k = 1; k <= number_constraints; k++)
                {
                    constraints[k] = myReader.GetInt32(ExportPosition);
                    ExportPosition++;
                }
                for (int k = 1; k <= number_features; k++)
                {
                    features[k] = myReader.GetInt32(ExportPosition);
                    ExportPosition++;
                }
            }
            individuumnew.Constraints = constraints;
            individuumnew.Features = features;

            myReader.Close();
            mycon.Close();

            //Individuum in der DB als "calculate" markieren
            myCommand.CommandText = "UPDATE `metaevo_individuums`  SET `status` = 'calculate' WHERE `id` = " + individuumnew.ID + " LIMIT 1 ;";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            //Client in der DB als "claculating" markieren
            this.Status_SetInDB(Dns.GetHostName(), "calculating");

            return individuumnew;
        }

        //Bekanntes Individuum in DB updaten (constraints, features, ipName)
        public void Individuum_UpdateInDB(ref EVO.Common.Individuum_MetaEvo individuum_input)
        {
            //Individuum in DB updaten (Daten und Status auf "ready")
            string tmptxt = "UPDATE `metaevo_individuums` SET ";
            
            for (int k = 1; k <= number_constraints; k++)
            {
                tmptxt = tmptxt + "const" + k + " = " + individuum_input.Constraints[k] + ", ";
            }
            for (int k = 1; k <= number_features; k++)
            {
                tmptxt = tmptxt + "feat" + k + " = " + individuum_input.Features[k] + ", ";
            }
            tmptxt = tmptxt + "ipName = " + individuum_input.get_Client() + " ";
            tmptxt = tmptxt + "WHERE `id` = " + individuum_input.ID + "LIMIT 1 ;"; ;

            myCommand.CommandText = tmptxt;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            //Client in DB als "standby" markieren
            this.Status_SetInDB(Dns.GetHostName(), "standby");
        }
      


        //### Methoden ### Working Process -> Client/Server Status 
         
        //Status eines Clients/Servers setzen
        public void Status_SetInDB(string ipName, string status)
        {
            myCommand.CommandText = "UPDATE `metaevo_network` SET `status` = '" + status + "' WHERE `ipName` = " + ipName + " LIMIT 1;";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }
        
        //Status von Clients lesen // Rückgabe stringarray[#clients][ipName,status,timestamp,speed-index,lowest speed]
        public string[,] Status_ReadClients()
        {
            myCommand = new MySqlCommand("Select * from metaevo_network WHERE type = client", mycon);
            mycon.Open();
            myReader = myCommand.ExecuteReader();

            string[,] back = new string[1, 5];
            int k = 0;

            while (myReader.Read())
            {
                if (k == 0) back = new string[myReader.Depth, 5];
                back[k,0] = myReader.GetString(0);
                back[k,1] = myReader.GetString(2);
                back[k,2] = myReader.GetString(3);
                back[k,3] = myReader.GetString(4);
                back[k,4] = myReader.GetString(5);
                k++;
            }
            myReader.Close();
            mycon.Close();

            return back;
        }

        //Status von Server lesen // Rückgabe stringarray[1][status, timestamp]
        public string[] Status_ReadServer()
        {
            myCommand = new MySqlCommand("Select status, timestamp from metaevo_network WHERE type = server LIMIT 1", mycon);
            mycon.Open();
            myReader = myCommand.ExecuteReader();

            string[] back = new string[2];

            while (myReader.Read())
            {
                back[0] = myReader.GetString(0);
                back[1] = myReader.GetString(1);
            }
            myReader.Close();
            mycon.Close();

            return back;
        }



        //### Methoden ### Working Process -> Scheduling

        //Scheduling
        private int scheduling(ref EVO.Common.Individuum_MetaEvo[] generation_input, string modus_input)
        {
            int current_ind = 0;
            
            //Statustabelle der Clients aus DB
            string[,] status_clients = this.Status_ReadClients();

            //Falls Modus = "new" 
            if (modus_input == "new")
            {
                //Tabelle anlegen [#clients][momentane Summe der Berechnungszeiten, speed-index (-1 = defekt)]
                calctimes = new int[status_clients.Length, 2];
                for (int j = 0; j < status_clients.Length; j++)
                {
                    if (status_clients[j, 1] == "error") calctimes[j, 1] = -1;
                    else calctimes[j, 1] = Convert.ToInt32(status_clients[j, 3]);
                }
            }

            //Falls Modus = "continue"
            else
            {
                //Vergleich ob neue Clients in DB, ggf SchedTabelle vergrössern (defekte Clients bleiben in der Tabelle !!)
                if (status_clients.Length != calctimes.Length)
                {
                    int[,] tmp = calctimes;
                    calctimes = new int[status_clients.Length, 2];
                    for (int k = 0; k < tmp.Length; k++)
                    {
                        calctimes[k, 0] = tmp[k, 0];
                        calctimes[k, 1] = tmp[k, 1];
                    }
                }
            }
            

            //Scheduling berechnen
            //  Für jedes Individuum
            for (int k = 0; k < generation_input.Length; k++)
            {
                current_ind = 0;
                //  Alle Clients durchgehen
                for (int j = 1; j < status_clients.Length; j++)
                {
                    if (calctimes[j, 1] > -1) //falls Individuum funktioniert
                    {
                        if ((calctimes[j, 0] + calctimes[j, 1]) < (calctimes[current_ind, 0] + calctimes[current_ind, 1]))
                        {
                            current_ind = j;
                        }
                    }
                }
                //In der Hilfstabelle Zeiten addieren
                calctimes[current_ind, 0] += calctimes[current_ind, 1];
                //Dem Individuum den Client(=IPWorker) zuweisen
                generation_input[k].set_Client(status_clients[current_ind,0]);
            }

            //Rückgabewert = Dauer bis erster Client fertig
            current_ind = 0;

            for (int j = 1; j < calctimes.Length; j++)
            {
                if (calctimes[j, 1] > 0) //falls Individuum funktioniert
                {
                    if (calctimes[j, 0] < calctimes[current_ind, 0])
                    {
                        current_ind = j;
                    }
                }
            }
            return calctimes[current_ind,0];
        }
    }
}
