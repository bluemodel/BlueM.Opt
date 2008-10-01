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
        EVO.Diagramm.ApplicationLog applog;

        Network network1;

        int number_optparas;     //Anzahl Optimierungsparameter
        int number_constraints;  //Anzahl constraints
        int number_features;    //Anzahl Featurefunktionswerte (inkl. Penalties)
        int populationsize;     //Anzahl Individuen in einer Population
        DateTime startingtime;       //Start des Berechnungszyklusses

        //### Konstruktor ###
        public Networkmanager(ref EVO.Common.Individuum_MetaEvo individuum_input, ref EVO.Common.EVO_Settings settings_input, ref EVO.Diagramm.ApplicationLog applog_input)
        {
            number_optparas = individuum_input.get_optparas().Length;
            number_constraints = individuum_input.Constraints.Length;
            number_features = individuum_input.Features.Length;
            populationsize = settings_input.MetaEvo.PopulationSize;
            applog = applog_input;

            myCommand = new MySqlCommand();

            //Server
            if (settings_input.MetaEvo.Role == "Network Server")
            {
                if (applog.log) applog.appendText("Network Manager: Started in 'Network Server' Mode");
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
                if (applog.log) applog.appendText("Network Manager: Started in 'Network Client' Mode");
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
                if (applog.log) applog.appendText("Network Manager: DB-Connection Successfully");
            }
            catch (MySqlException ex)
            {
                if (applog.log) applog.appendText("Network Manager: DB-Connection Failed");
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

            myCommand.CommandText = "CREATE TABLE IF NOT EXISTS `metaevo_network` (`ipName` varchar(15) NOT NULL,`type` varchar(20) NOT NULL,`status` varchar(20) NOT NULL,`timestamp` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,`speed_av` double NOT NULL,`speed_low` double NOT NULL ,PRIMARY KEY  (`ipName`)) ENGINE=MyISAM DEFAULT CHARSET=utf8;";
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
            for (int k = 1; k <= number_features; k++)
            {
                tmptxt = tmptxt + "`feat" + k + "` double NOT NULL default '0',";
            }
            tmptxt = tmptxt + "`ipName` varchar(15) NOT NULL, PRIMARY KEY  (`id`)) ENGINE=MyISAM DEFAULT CHARSET=utf8;";

            myCommand.CommandText = tmptxt;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
            if (applog.log) applog.appendText("Network Manager: DB-Construction Successfilly");
        }

        //(ok)sich als Client in DB eintragen
        public void DB_client_entry() {
            //Eintrag für Client
            myCommand.Connection.Open();
            try
            {
                myCommand.CommandText = "INSERT INTO `metaevo_network` (`ipName`, `type`, `status`, `timestamp`, `speed_av`, `speed_low`) VALUES ('" + Dns.GetHostName() + "', 'client', 'ready', '', '1000', '1000');";
                myCommand.ExecuteNonQuery();
            }
            catch
            {
            }
            //Um Timestamp richtig zu generieren:
            myCommand.CommandText = "UPDATE `metaevo_network` SET `status`= 'ready' WHERE ipName = '" + Dns.GetHostName() + "';";
            myCommand.ExecuteNonQuery();
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
            string tmptxt = "INSERT INTO `metaevo_individuums` (`id` :`status` :";

            //Datenmaske
            for (int k = 1; k <= number_optparas; k++)
            {
                tmptxt = tmptxt + "`opt" + k + "`:";
            }

            tmptxt = tmptxt + "`ipName`) VALUES ";

            //Datensätze
            for (int j = 0; j < generation_input.Length; j++)
            {
                tmptxt = tmptxt + "('" + generation_input[j].ID + "':'raw':";
                for (int k = 0; k < number_optparas; k++)
                {
                    tmptxt = tmptxt + "'" + generation_input[j].get_optparas()[k] + "':";
                }
                tmptxt = tmptxt + "'" + generation_input[j].get_client() + "'):";
            }
            tmptxt = tmptxt.TrimEnd(':').Replace(",",".").Replace(":", ",") + ";";

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
                individuum_input.set_status("raw");
            }
            individuum_input.set_optparas(optparas);

            myReader.Close();
            mycon.Close();
        }

        //(ok)prüfen wie viele Individuen fertig berechnet sind
        public int Individuums_CountReadyInDB()
        {
            myCommand.CommandText = "Select status from metaevo_individuums WHERE status = 'true' OR status = 'false'";
            myCommand.Connection.Open();
            myReader = myCommand.ExecuteReader();

            int count = 0;
            while (myReader.Read())
            {
                count++;
            }
            myCommand.Connection.Close();
            return count;
        }
        
        //(ok)alle Individuen aus der DB in der Generation updaten (ID, status, constraints, features, ipName)
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
                for (int k = 0; k < number_features; k++)
                {
                    tmptxt = tmptxt + "feat" + (k + 1) + " = '" + individuum_input.Features[k] + "': ";
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
            catch
            {
                //Eigene DB löschen um kollisionen zu vermeiden?
            }
        }


 
        //### Methoden ### Working Process -> Network Functions for Clients
        
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
                if (applog.log) applog.appendText("Scheduling: No Client found registered in DB - wait...");
                System.Threading.Thread.Sleep(3000);
                scheduling_new(ref generation_input);
            }
            else
            {
                if (applog.log) applog.appendText("Networkmanager: New Scheduling");

                //Individuen-Zuweisungen zurücksetzen 
                network1.erase_current_calc_times();


                //Scheduling berechnen
                //Für jedes Individuum
                for (int k = 0; k < generation_input.Length; k++)
                {
                    current_client = 0;
                    //Alle Clients durchgehen
                    for (int j = 1; j < network1.number_clients; j++)
                    {
                        //falls Client funktioniert
                        if (network1.Clients[j].status != "error")
                        {
                            //Falls Client schneller 
                            if ((network1.Clients[j].speed_av * network1.Clients[j].numberindividuums + network1.Clients[j].speed_av) < (network1.Clients[current_client].speed_av * network1.Clients[current_client].numberindividuums + network1.Clients[current_client].speed_av))
                            {
                                current_client = j;
                            }
                        }
                    }
                    //In der current_calc_time-Eigenschaft Zeiten addieren, Individuum addieren
                    network1.Clients[current_client].numberindividuums++;
                    //Dem Individuum den Client(=IPWorker) zuweisen
                    generation_input[k].set_Client(network1.Clients[current_client].ipName);
                }

                //Ausgabe der Zuteilung
                for (int i = 0; i < network1.Clients.Length; i++)
                {
                    if (applog.log) applog.appendText("Networkmanager: New Scheduling: Client '" + network1.Clients[i].ipName + "' " + network1.Clients[i].numberindividuums + " Individuums");
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
                if (applog.log) applog.appendText("Scheduling: No Client found registered in DB - waiting...");
                System.Threading.Thread.Sleep(3000);
                scheduling_new(ref generation_input);
            }

            //An den Daten der Clients hat sich etwas geändert (Status=ready oder Speed-av hat sich um mehr als 5% geändert)
            else if (scheduling_error)   
            {
                if (applog.log) applog.appendText("Networkmanager: Adapted Scheduling");

                //DB auf Geschwindigkeitsänderung/Ausfall von Clients untersuchen und entsprechende Individuen demarkieren
                current_client = 0;

                //Individuums-Informationen aus der DB updaten
                this.Individuums_UpdateFromDB(ref generation_input);

                //Defekte Clients suchen
                for (int k = 0; k < network1.number_clients; k++)
                {
                    if (network1.Clients[k].status == "calculating")
                    {
                        //Falls ausgefallen die zugehörigen Individuen demarkieren und Client als defekt markieren
                        if (network1.Clients[k].timestamp.AddMilliseconds(1.2 * network1.Clients[k].speed_low) < DateTime.Now)
                        {
                            network1.Clients[k].set_AlsoInDB("error", 0, 0);

                            //Evtl vorhandene Individuen des Clients demarkieren
                            for (int j = 0; j < generation_input.Length; j++)
                            {
                                if ((generation_input[j].get_client() == network1.Clients[k].ipName) && (generation_input[j].get_status() != "true") && (generation_input[j].get_status() != "false"))
                                {
                                    generation_input[j].set_status("raw");
                                    generation_input[j].set_Client("");
                                }
                            }
                        }
                    }
                }

                //Individuenzahlen der Clients neu berechnen und nur benötigte Änderungen in DB umsetzen
                //  Temporäres CurrentCalctime-Array aufbauen     Client: [aktuelle vergangene Rechenzeit beim aktuellen Individuum][bereits berechnete Individuen]
                double[,] current_status = new double[network1.Clients.Length, 2]; //{Calctime, Individuums Calculated}
                
                //      Anzahl der Berechneten Individuen pro Client
                for (int i = 0; i < generation_input.Length; i++)
                {
                    if (generation_input[i].get_status() != "raw")
                    {
                        for (int j = 1; j < network1.number_clients; j++)
                        {
                            //falls Client funktioniert
                            if (network1.Clients[j].status != "error")
                            {
                                //Falls Individuum diesem Client zugeordnet ist
                                if (generation_input[i].get_client() == network1.Clients[j].ipName)
                                {
                                    current_status[j, 1]++; 
                                }
                            }
                        }
                    }
                }
                
                //      Berechnungszeit des gerade berechneten Individuums pro Client
                for (int i = 0; i < network1.Clients.Length; i++)
                {
                    current_status[i, 0] = (DateTime.Now.Subtract(network1.Clients[i].timestamp)).TotalMilliseconds;
                }
                
                //      Individuenverteilung jetzt neu berechnen
                for (int i = 0; i < generation_input.Length; i++)
                {
                    if (generation_input[i].get_status() == "raw")
                    {
                        current_client = 0;
                        //Alle Clients durchgehen
                        for (int j = 1; j < network1.number_clients; j++)
                        {
                            //falls Client funktioniert
                            if (network1.Clients[j].status != "error")
                            {
                                //Falls schnellster Client 
                                if (current_status[j, 0] + network1.Clients[j].speed_av < (current_status[current_client, 0] + network1.Clients[current_client].speed_av))
                                {
                                    current_client = j;
                                }
                            }
                        }
                        //In der current_calc_time-Eigenschaft Zeiten addieren, Individuum addieren
                        current_status[current_client, 0] += network1.Clients[current_client].speed_av;
                        current_status[current_client, 1]++;
                    }
                }

                //Individuen von Clients mit inzwischen geringerem Speed in DB demarkieren
                for (int i = 0; i < network1.Clients.Length; i++)
                {
                    int j = 0;
                    while (current_status[i, 1] < network1.Clients[i].numberindividuums)
                    {
                        if ((generation_input[j].get_status() == "raw") && (generation_input[j].get_client() == network1.Clients[i].ipName))
                        {
                            generation_input[j].set_Client("");
                            network1.Clients[i].numberindividuums--;
                        }
                        j++;
                    }
                }

                //Freie Individuen suchen und nach den berechneten Änderungen in DB updaten
                for (int i = 0; i < network1.Clients.Length; i++)
                {
                    int j = 0;
                    while (current_status[i, 1] > network1.Clients[i].numberindividuums)
                    {
                        if (generation_input[j].get_client() == "")
                        {
                            generation_input[j].set_Client(network1.Clients[i].ipName);
                            this.Individuum_UpdateInDB(ref generation_input[j], "ipName status", "raw");
                            network1.Clients[i].numberindividuums++;
                        }
                        j++;
                    }
                }

                //Ausgabe der neuen Zuteilung
                for (int i = 0; i < network1.Clients.Length; i++)
                {
                    if (applog.log) applog.appendText("Networkmanager: Adapted Scheduling: Client '" + network1.Clients[i].ipName + "' " + network1.Clients[i].numberindividuums + " Individuums");
                }
            }
        }



        //### Hauptprogramm ### Berechnet eine Generation im Netzwerk

        public bool calculate_by_clients(ref EVO.Common.Individuum_MetaEvo[] generation_input)
        {
            int individuums_ready = 0;

            //Scheduling initialisieren und ersten Schritt rechnen
            this.scheduling_new(ref generation_input);

            //In die Datenbank schreiben
            if (applog.log) applog.appendText("Networkmanager: Write Individuums to DB");
            this.DB_ClearIndividuumsTable();
            this.Individuums_WriteToDB(ref generation_input);

            //Warten bis erste Ergebnisse vorliegen müssten
            if (applog.log) applog.appendText("Networkmanager: Waiting for first Results...");

            //Prüfen ob alle Individuen fertig berechnet sind
            while (this.Individuums_CountReadyInDB() < this.populationsize)
            {
                if (individuums_ready < this.Individuums_CountReadyInDB()) 
                {
                    individuums_ready = this.Individuums_CountReadyInDB();
                    if (applog.log) applog.appendText("Networkmanager: " + Math.Round((double)this.Individuums_CountReadyInDB() / (double)populationsize,2)*100 + "%");
                }
               
                //Scheduling-korrektur anstossen (inklusive-DB Update)
                this.scheduling_continue(ref generation_input);

                //Warten
                System.Threading.Thread.Sleep(1000);
            }

            //Fertige Individuen wieder auslesen
            this.Individuums_UpdateFromDB(ref generation_input);

            return true;
        }
    }
}
