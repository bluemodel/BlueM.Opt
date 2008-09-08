using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Net;
using MySql.Data.MySqlClient;
using System.Threading;

namespace IHWB.EVO.HybridAlgo
{
    public class Networkmanager
    {
        //### Variablen ###
        MySqlConnection mycon;
        MySqlCommand myCommand;
        MySqlDataReader myReader;

        Network network1;

        int number_optparas;     //Anzahl Optimierungsparameter
        int number_constraints;  //Anzahl constraints
        int number_features;    //Anzahl Featurefunktionswerte (inkl. Penalties)
        int populationsize;     //Anzahl Individuen in einer Population
        DateTime startingtime;       //Start des Berechnungszyklusses

        //### Konstruktor ###
        public Networkmanager(ref EVO.Common.Individuum_MetaEvo individuum_input, ref EVO.Common.EVO_Settings settings_input)
        {
            number_optparas = individuum_input.get_optparas().Length;
            number_constraints = individuum_input.Constraints.Length;
            number_features = individuum_input.Features.Length;
            populationsize = settings_input.MetaEvo.PopulationSize;

            myCommand = new MySqlCommand();

            //Server
            if (settings_input.MetaEvo.Role == "Network Server")
            {
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
                mycon = new MySqlConnection("datasource=" + settings_input.MetaEvo.MySQL_Host + ";username=" + settings_input.MetaEvo.MySQL_User + ";password=" + settings_input.MetaEvo.MySQL_Password + ";database=" + settings_input.MetaEvo.MySQL_Database);
                myCommand.Connection = mycon;
                if (this.DB_check_connection())
                {
                    DB_client_entry();
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
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message,"MySQL Database");
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

            myCommand.CommandText = "CREATE TABLE IF NOT EXISTS `metaevo_network` (`ipName` varchar(15) NOT NULL,`type` varchar(20) NOT NULL,`status` varchar(20) NOT NULL,`timestamp` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,`speed-av` double NOT NULL,`speed-low` double NOT NULL, `individuums` int NOT NULL ,PRIMARY KEY  (`ipName`)) ENGINE=MyISAM DEFAULT CHARSET=utf8;";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            //Eintrag für Server
            myCommand.CommandText = "INSERT INTO `metaevo_network` (`ipName`, `type`, `status`, `timestamp`, `speed-av`, `speed-low`) VALUES ('" + Dns.GetHostName() + "', 'server', 'generate Individuums', '', '0', '0');";
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
            for (int k = 1; k <= number_features; k++)
            {
                tmptxt = tmptxt + "`feat" + k + "` double NOT NULL default '0',";
            }
            tmptxt = tmptxt + "`ipName` varchar(15) NOT NULL, PRIMARY KEY  (`id`)) ENGINE=MyISAM DEFAULT CHARSET=utf8;";

            myCommand.CommandText = tmptxt;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }

        //(ok)sich als Client in DB eintragen
        private void DB_client_entry() {
            //Eintrag für Client
            myCommand.CommandText = "INSERT INTO `metaevo_network` (`ipName`, `type`, `status`, `timestamp`, `speed-av`, `speed-low`, `individuums`) VALUES ('" + Dns.GetHostName() + "', 'client', 'ready', '', '1000', '1000', '0');";
            myCommand.Connection.Open();
            try
            {
                myCommand.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                myCommand.CommandText = "UPDATE `metaevo_network` SET status = 'ready' WHERE ipName = '" + Dns.GetHostName() + "');";
                myCommand.ExecuteNonQuery();
                MessageBox.Show("Client existiert bereits in der Datenbank", "MySQL Database");
            }
            myCommand.Connection.Close();
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



        //### Methoden ### Working Process -> Individuums 

        //(ok)Neue Individuen in die Datenbank einfügen (ID, status, optparas, ipName)
        public void Individuums_WriteToDB(ref EVO.Common.Individuum_MetaEvo[] generation_input)
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
                tmptxt = tmptxt + "('" + generation_input[j].ID + "','raw',";
                for (int k = 0; k < number_optparas; k++)
                {
                    tmptxt = tmptxt + "'" + generation_input[j].get_optparas()[k] + "',";
                }
                tmptxt = tmptxt + "'" + generation_input[j].get_client() + "'),";
            }
            tmptxt = tmptxt.TrimEnd(',') + ";";

            myCommand.CommandText = tmptxt;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }
        
        //(ok)nächstes eigenes Individuum aus der DB lesen (ID, optparas)
        public void Individuum_ReadFromDB_Client(ref EVO.Common.Individuum_MetaEvo individuum_input)
        {
            myCommand = new MySqlCommand("Select * from metaevo_individuums WHERE ipName = '" + Dns.GetHostName() + "' AND (status = 'raw' OR status = 'calculate') LIMIT 1", mycon);
            mycon.Open();
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
            }
            individuum_input.set_optparas(optparas);

            myReader.Close();
            mycon.Close();
        }

        //prüfen wie viele Individuen fertig berechnet sind
        public int Individuums_CountReadyInDB()
        {
            myCommand = new MySqlCommand("Select status from metaevo_individuums WHERE status = 'true' OR status = 'false'", mycon);
            mycon.Open();
            myReader = myCommand.ExecuteReader();

            int count = 0;
            while (myReader.Read())
            {
                count++;
            }
            return count;
        }
        
        //alle Individuen aus der DB in der Generation updaten (ID, status, constraints, features, ipName)
        public void Individuums_UpdateFromDB(ref EVO.Common.Individuum_MetaEvo[] generation_input)
        {
            myCommand = new MySqlCommand("Select * from metaevo_individuums", mycon);
            mycon.Open();
            myReader = myCommand.ExecuteReader();

            int exportPosition;
            int individuumcounter = 0;

            double[] constraints = new double[number_constraints];
            double[] features = new double[number_features];

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

                //Features setzen
                for (int k = 0; k < number_features; k++)
                {
                    generation_input[individuumcounter].Features[k] = myReader.GetDouble(exportPosition);
                    exportPosition++;
                }

                //Client setzen
                generation_input[individuumcounter].set_Client(myReader.GetString(exportPosition));
            }

            myReader.Close();
            mycon.Close();
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
                tmptxt = tmptxt + "status = '" + status_input + "' ";
            }
            //opt
            if (fields2update.Contains("opt")) 
            {
                for (int k = 0; k < number_constraints; k++)
                {
                    tmptxt = tmptxt + "opt" + (k + 1) + " = '" + individuum_input.get_optparas()[k] + "', ";
                }
            }
            //feat
            if (fields2update.Contains("feat")) 
            {
                for (int k = 0; k < number_features; k++)
                {
                    tmptxt = tmptxt + "feat" + (k + 1) + " = '" + individuum_input.Features[k] + "', ";
                }
            }
            //const
            if (fields2update.Contains("const")) 
            {
                for (int k = 0; k < number_features; k++)
                {
                    tmptxt = tmptxt + "const" + (k + 1) + " = '" + individuum_input.Constraints[k] + "', ";
                }
            }
            if (fields2update.Contains("ipName"))
            {
                tmptxt = tmptxt + "ipName = '" + individuum_input.get_client() + "' ";
            }
            tmptxt = tmptxt + "WHERE id = '" + individuum_input.ID + "' LIMIT 1;"; ;

            myCommand.CommandText = tmptxt;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }


 
        //### Methoden ### Working Process -> Network Functions for Clients
         
        //Eigene Eigenschaften eines Clients/Servers updaten 
        public void Network_UpdateInDB(string status_input, int speed_av_input, int speed_low_input)
        {
            //Client in DB updaten 
            string tmptxt = "UPDATE `metaevo_network` SET ";
            string status = status_input;

            //status
            if (status_input != "")
            {
                tmptxt = tmptxt + "status = '" + status_input + "' ";
            }
            //speed
            if (speed_av_input != 0)
            {
                tmptxt = tmptxt + "speed-av = " + speed_av_input + " ";
            }
            //lowest speed
            if (speed_low_input != 0)
            {
                tmptxt = tmptxt + "speed-low = " + speed_low_input + " ";
            }
            tmptxt = tmptxt + "WHERE ipName = '" + Dns.GetHostName() + "' LIMIT 1;";
            myCommand.CommandText = tmptxt;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }

        //(ok)Status von Server lesen // Rückgabe stringarray[1][status, timestamp]
        public string[] Network_ReadServer()
        {
            myCommand = new MySqlCommand("Select status, timestamp from metaevo_network WHERE type = 'server' LIMIT 1", mycon);
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

        //Scheduling //Rückgabe Wartezeit bis erster Client fertig ist in ms //Eingabe "new" oder "continue"
        private TimeSpan scheduling(ref EVO.Common.Individuum_MetaEvo[] generation_input, string modus_input)
        {
            int current_ind = 0;
            TimeSpan waitfor = new TimeSpan();
            int current_client;
            bool scheduling_error = false;

            //Statustabelle der Clients aktualisieren
            network1.update_From_DB();

            

            //Falls Modus = "new" 
            if (modus_input == "new")
            {
                network1.erase_current_calc_times();
            }



            //Falls Modus = "continue"
            else
            {
               //Falls kein Client fertig ist, sofort weitere Wartezeit zurückgeben
                current_client = 0;
                for (int k = 0; k < network1.number_clients; k++)
                {
                    if (network1.Clients[k].status == "ready")
                    {
                        if (network1.Clients[k].current_calc_time < network1.Clients[current_client].current_calc_time)
                        {
                            current_client = k;
                        }
                    }
                }
                waitfor = this.startingtime.AddMilliseconds(network1.Clients[current_client].current_calc_time).Subtract(DateTime.Now);
                if (waitfor.TotalMilliseconds > 0) return waitfor;


                //Mindestens ein Client ist fertig:
                //DB auf Geschwindigkeitsänderung/Ausfall von Clients untersuchen
                current_client = 0;

                //Individuums-Informationen aus der DB updaten
                this.Individuums_UpdateFromDB(ref generation_input);

                for (int k = 0; k < network1.number_clients; k++)
                {
                    if (network1.Clients[k].status == "ready")
                    {
                        //Suche "raw" Individuum was von einem langsameren Client berechnet werden soll 
                        //  -> fehler im scheduling
                        scheduling_error = true;
                    }
                }

                if (scheduling_error)
                {
                    //Findet neu ausgefallene Clients
                    for (int k = 0; k < network1.number_clients; k++)
                    {
                        if (network1.Clients[k].status == "calculating")
                        {
                            //Falls ausgefallen die zugehörigen Individuen demarkieren und Client als defekt markieren
                            if (network1.Clients[k].timestamp.AddMilliseconds(network1.Clients[k].speed_low) < DateTime.Now)
                            {
                                network1.Clients[k].setstatus_AlsoInDB("error");
                            }
                            //Alle "raw"-Individuen demarkieren
                            else
                            {

                            }
                        }
                    }
                }
            }

            if (scheduling_error || modus_input == "new")
            {

                //Scheduling berechnen
                //  Für jedes Individuum
                for (int k = 0; k < generation_input.Length; k++)
                {
                    //Falls das Individuum noch zu berechnen ist
                    if (generation_input[k].get_status() == "raw")
                    {
                        current_ind = 0;
                        //  Alle Clients durchgehen
                        for (int j = 1; j < network1.number_clients; j++)
                        {
                            if (network1.Clients[j].status != "error") //falls Individuum funktioniert
                            {
                                if ((network1.Clients[j].current_calc_time + network1.Clients[j].speed_av) < (network1.Clients[current_ind].current_calc_time + network1.Clients[current_ind].speed_av))
                                {
                                    current_ind = j;
                                }
                            }
                        }
                        //In der current_calc_time-Eigenschaft Zeiten addieren, Individuum addieren
                        network1.Clients[current_ind].current_calc_time += network1.Clients[current_ind].speed_av;
                        network1.Clients[current_ind].numberindividuums++;
                        //Dem Individuum den Client(=IPWorker) zuweisen
                        generation_input[k].set_Client(network1.Clients[current_ind].ipName);
                        //Diese Information auch in die DB schreiben
                        this.Individuum_UpdateInDB(ref generation_input[k], "ipName", "");
                    }
                }

                //Rückgabewert = Dauer bis erster Client fertig
                current_client = 0;

                for (int j = 1; j < network1.number_clients; j++)
                {
                    if (network1.Clients[j].status != "error") //falls Client funktioniert
                    {
                        if (network1.Clients[j].current_calc_time < network1.Clients[current_client].current_calc_time)
                        {
                            current_ind = j;
                        }
                    }
                }
                waitfor = this.startingtime.AddMilliseconds(network1.Clients[current_client].current_calc_time).Subtract(DateTime.Now);
                return waitfor;
            }
        }
        return //ansonsten wartezeit bis langsamster client fertig ist


        //### Hauptprogramm ###
        public bool perform_step(ref EVO.Common.Individuum_MetaEvo[] generation_input)
        {
            TimeSpan waitfor;
            startingtime = DateTime.Now;

            //Scheduling initialisieren und ersten Schritt rechnen
            waitfor = this.scheduling(ref generation_input, "new");
            //Warten
            System.Threading.Thread.Sleep(waitfor);

            //Prüfen ob alle Individuen fertig berechnet sind
            while (this.Individuums_CountReadyInDB() < this.populationsize)
            {
                //Scheduling anstossen
                waitfor = this.scheduling(ref generation_input, "continue");

                //Warten
                System.Threading.Thread.Sleep(waitfor);
            }
            return true;
        }

    }
}
