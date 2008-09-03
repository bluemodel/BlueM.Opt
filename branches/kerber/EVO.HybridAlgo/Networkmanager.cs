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

        EVO.Common.Individuum_MetaEvo individuum;
        EVO.Common.EVO_Settings settings;

        int number_optparas;
        int number_constraints;  //Anzahl constraints
        int number_features;    //Anzahl Featurefunktionswerte (inkl. Penalties)

        //### Konstruktor ###
        public Networkmanager(ref EVO.Common.Individuum_MetaEvo individuum_input, ref EVO.Common.EVO_Settings settings_input)
        {
            this.individuum = individuum_input;
            number_optparas = individuum.get_optparas().Length;
            number_constraints = individuum.Constraints.Length; 
            number_features = individuum.Features.Length;  

            this.settings = settings_input;
            myCommand = new MySqlCommand();

            //Server
            if (this.settings.MetaEvo.Role == "Network Server")
            {
                mycon = new MySqlConnection("datasource=" + settings.MetaEvo.MySQL_Host + ";username=" + settings.MetaEvo.MySQL_User + ";password=" + settings.MetaEvo.MySQL_Password + ";database=information_schema");
                myCommand.Connection = mycon;

                if (this.check_connection())
                {
                    database_init();  //Inklusive Server-Entry
                }
            }
            // Client
            else
            {
                mycon = new MySqlConnection("datasource=" + settings.MetaEvo.MySQL_Host + ";username=" + settings.MetaEvo.MySQL_User + ";password=" + settings.MetaEvo.MySQL_Password + ";database=" + settings.MetaEvo.MySQL_Database);
                myCommand.Connection = mycon;
                if (this.check_connection())
                {
                    client_entry();
                }
            }
        }



        //### Methoden ### Initialisierung

        //Verbindung zur DB prüfen bzw. Fehler ausgeben
        private bool check_connection()
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
        private void database_init()   
        {
            //Datenbank
            myCommand.CommandText = "CREATE DATABASE IF NOT EXISTS " + settings.MetaEvo.MySQL_Database;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            //Wechseln in neue Datenbank
            myCommand.Connection.ConnectionString = "datasource=" + settings.MetaEvo.MySQL_Host + ";username=" + settings.MetaEvo.MySQL_User + ";password=" + settings.MetaEvo.MySQL_Password + ";database=" + settings.MetaEvo.MySQL_Database;

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
        private void client_entry() {
            //Eintrag für Client
            myCommand.CommandText = "INSERT INTO `metaevo_network` (`ipName`, `type`, `status`, `timestamp`, `speed-index`, `lowest speed`) VALUES ('" + Dns.GetHostName() + "', 'client', 'ready', '', '0', '0');";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }

        //Datenbank löschen
        public void database_delete()
        {
            myCommand.CommandText = "DROP DATABASE IF EXISTS " + settings.MetaEvo.MySQL_Database;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }

        //Individuum-Datenbank säubern
        public void ClearIndividuumsTable()
        {
            myCommand.CommandText = "TRUNCATE TABLE `metaevo_individuums`";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }



        //### Methoden ### Working Process -> Individuums 

        //Individuen in die Datenbank einfügen (ID, optparas, ipName)
        public void IndividuumsWriteToDB(EVO.Common.Individuum_MetaEvo[] generation_input)
        {
            //Scheduling-Tabelle bearbeiten


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
                tmptxt = tmptxt + ", " + generation_input[j].get_IPWorker() + "),";
            }
            tmptxt = tmptxt.TrimEnd(',') + ";";

            myCommand.CommandText = tmptxt;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }
        
        //nächstes eigenes Individuum aus der DB lesen (constraints, features, ipName)
        public EVO.Common.Individuum_MetaEvo IndividuumReadFromDB()
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
                individuum.ID = myReader.GetInt32(ExportPosition);
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
            individuum.Constraints = constraints;
            individuum.Features = features;

            myReader.Close();
            mycon.Close();

            //Individuum in der DB als "calculate" markieren
            myCommand.CommandText = "UPDATE `metaevo_individuums`  SET `status` = 'calculate' WHERE `id` = " + individuum.ID + " LIMIT 1 ;";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            //Client in der DB als "claculating" markieren
            this.StatusSetInDB(Dns.GetHostName(), "calculating");

            return individuum;
        }

        //Bekanntes Individuum updaten (constraints, features, ipName)
        public void IndividuumUpdateInDB(EVO.Common.Individuum_MetaEvo individuum_input)
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
            tmptxt = tmptxt + "ipName = " + individuum_input.get_IPWorker() + " ";
            tmptxt = tmptxt + "WHERE `id` = " + individuum.ID + "LIMIT 1 ;"; ;

            myCommand.CommandText = tmptxt;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            //Client in DB als "standby" markieren
            this.StatusSetInDB(Dns.GetHostName(), "standby");
        }
      


        //### Methoden ### Working Process -> Client/Server Status 
         
        //Status eines Clients/Servers setzen
        public void StatusSetInDB(string ipName, string status)
        {
            myCommand.CommandText = "UPDATE `metaevo_network` SET `status` = '" + status + "' WHERE `ipName` = " + ipName + " LIMIT 1;";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }
        
        //Status von Clients lesen // Rückgabe stringarray[ipName][status,timestamp,speed-index,lowest speed]
        public string[,] StatusReadClients()
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

        //Status von Server lesen // Rückgabe stringarray[status, timestamp]
        public string[] StatusReadServer()
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


        //
    }
}
