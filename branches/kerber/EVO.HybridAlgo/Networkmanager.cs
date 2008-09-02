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

        //### Konstruktor ###
        public Networkmanager(ref EVO.Common.Individuum_MetaEvo individuum_input, ref EVO.Common.EVO_Settings settings_input)
        {
            this.individuum = individuum_input;
            this.settings = settings_input;
            myCommand = new MySqlCommand();

            //Server
            if (this.settings.MetaEvo.Role == "Network Server")
            {
                mycon = new MySqlConnection("datasource=" + settings.MetaEvo.MySQL_Host + ";username=" + settings.MetaEvo.MySQL_User + ";password=" + settings.MetaEvo.MySQL_Password + ";database=information_schema");
                myCommand.Connection = mycon;

                if (this.check_connection())
                {
                    database_init();  
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
            int number_genes;
            int number_constraints;  //Anzahl constraints
            int number_features;     //Anzahl Featurefunktionswerte (inkl. Penalties)

            //Datenbank
            myCommand.CommandText = "CREATE DATABASE IF NOT EXISTS " + settings.MetaEvo.MySQL_Database;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            //Wechseln in neue Datenbank
            myCommand.Connection.ConnectionString = "datasource=" + settings.MetaEvo.MySQL_Host + ";username=" + settings.MetaEvo.MySQL_User + ";password=" + settings.MetaEvo.MySQL_Password + ";database=" + settings.MetaEvo.MySQL_Database;

            //benötigte Tabellen: "Status" 
            myCommand.CommandText = "DROP TABLE IF EXISTS `metaevo_status`;";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            myCommand.CommandText = "CREATE TABLE IF NOT EXISTS `metaevo_status` (`name` varchar(15) NOT NULL,`type` varchar(20) NOT NULL,`status` varchar(20) NOT NULL,`timestamp` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,`speed-index` varchar(20) NOT NULL,`lowest speed` varchar(20) NOT NULL,PRIMARY KEY  (`ip`)) ENGINE=MyISAM DEFAULT CHARSET=utf8;";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            //Eintrag für Server
            myCommand.CommandText = "INSERT INTO `metaevo_status` (`name`, `type`, `status`, `timestamp`, `speed-index`, `lowest speed`) VALUES ('" + Dns.GetHostName() + "', 'server', 'generate Individuums', '', '0', '0');";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            //Individuen-Tabelle
            number_genes = individuum.get_genes().Length;
            number_constraints = individuum.Constraints.Length;
            number_features = individuum.Features.Length;

        }

        private void client_entry() {
            //Eintrag für Client
            myCommand.CommandText = "INSERT INTO `metaevo_status` (`name`, `type`, `status`, `timestamp`, `speed-index`, `lowest speed`) VALUES ('" + Dns.GetHostName() + "', 'client', 'ready', '', '0', '0');";
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
            myCommand.CommandText = "TRUNCATE TABLE `metaevo_individuals`";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }



        //### Methoden ### Working Process -> Individuums 

        //Individuen in der Datenbank Speichern
        public void IndividuumsWriteToDB(EVO.Common.Individuum[] generation_input)
        {
            //Scheduling-Tabelle bearbeiten

            //Schreiben der Individuen-Daten in die DB
        }
        /*
        // Individuum aus der DB lesen
        public EVO.Common.Individuum IndividuumReadFromDB(string tablename_input)
        {
            //Ein Individuum mit eigener IP lesen
            //Individuum in der DB als "calculate" markieren
            //Client in der DB als "claculating" markieren
        }
        // Bekanntes Individuum updaten 
        public void IndividuumUpdateInDB(int id_individuum, )
        {
            //Individuum updaten (Daten und Status auf "ready")
            //Client in DB als "standby" markieren
        }

        //### Methoden ### Working Process -> Client/Server Status 
         
        //Status eines Clients/Servers setzen
        public void StatusSetInDB(string name)
        {

        }
        
        //Zeit anhand der DB synchronisieren (Offset Berechnen) 
        //Status von Clients lesen
        //Status von Server lesen
         
        */

        //### Methoden ### Working Process -> Scheduling
        

        //
    }
}
