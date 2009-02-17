using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Net;
using MySql.Data.MySqlClient;

namespace IHWB.EVO.MetaEvo
{
    
    //Struktur eines Clients

    public class Client
    {
        public readonly string ipName;
        public string status;               //{Client Status: {claculating, error, ready} Server Status: {init Genpool, generate Individuums, waiting for client-calculation, finished}
        public DateTime timestamp;          //Zeit seit dem letzten Update der Db durch diesen Client
        public double speed_av;             //in Millisekunden durchschnittliche Simulationszeit
        public double speed_low;            //in Millisekunden langsamste Simulationszeit
        public int numberindividuums;       //Wie viele Individuen der Client berechnen soll

        MySqlCommand myCommand;
        MySqlDataReader myReader;

        public Client()
        {
            
        }

        public Client(ref MySqlConnection mycon_input, string ipName_input, string status_input, DateTime timestamp_input, double speed_av_input, double speed_low_input, int numberindividuums_input)
        {
            ipName = ipName_input;
            status = status_input;
            timestamp = timestamp_input;
            speed_av = speed_av_input;
            speed_low = speed_low_input;
            numberindividuums = numberindividuums_input;

            myCommand = new MySqlCommand();
            myCommand.Connection = mycon_input;
        }

        //In der Datenbank updaten
        public void set_AlsoInDB(string status_input, double speed_av_input, double speed_low_input)
        {
            //Client in DB updaten 
            string tmptxt = "UPDATE `metaevo_network` SET status = 'ready2' WHERE ipName = '" + this.ipName + "' LIMIT 1; UPDATE `metaevo_network` SET ";

            //status
            if (status_input != "")
            {
                tmptxt = tmptxt + "status = '" + status_input + "', ";
                this.status = status_input;
            }
            //speed_av
            if (speed_av_input != -1)
            {
                tmptxt = tmptxt + "speed_av = '" + speed_av_input + "', ";
                this.speed_av = speed_av_input;
            }
            //speed_low
            if (speed_low_input != -1)
            {
                tmptxt = tmptxt + "speed_low = '" + speed_low_input + "' ";
                this.speed_low = speed_low_input;
            }
            tmptxt = tmptxt.TrimEnd(',', ' ') + " WHERE ipName = '" + this.ipName + "' LIMIT 1;";
            myCommand.CommandText = tmptxt;
            try
            {
                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Function 'set_AlsoInDB': \r\nFehler beim Lesen der Datenbank: \r\n" + (ex.Message), "MetaEvo - Network");
            }
        }

        //Von der Datenbank updaten
        public void get_NumberIndividuumsFromDB()
        {
            myCommand.CommandText ="Select * from metaevo_network WHERE ipName = '" + this.ipName + "'";
            try
            {
                myCommand.Connection.Open();
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    this.numberindividuums = myReader.GetInt32(6);
                }
                myReader.Close();
                myCommand.Connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Function 'get_NumberIndividuumsFromDB': \r\nFehler beim Lesen der Datenbank: \r\n" + (ex.Message), "MetaEvo - Network");
            }
        }
    }

    //Objekt um die Daten der Clients komfortabel zu verwalten

    class Network
    {
        MySqlConnection mycon;
        MySqlCommand myCommand;
        MySqlDataReader myReader;

        public int number_clients;     //Anzahl Clients

        public Client[] Clients;
        private Client[] Clients_old;

        public Network(ref MySqlConnection mycon_input)
        {
            //Datenbankverbindung
            mycon = mycon_input;
            myCommand = new MySqlCommand();
            myCommand.Connection = mycon;
            Clients = new Client[0];

            number_clients = 0;
        }

        //Aktuelle Daten aus der DB holen
        public bool update_From_DB()
        {
            bool back = false; //Ob Scheduling neu berechnet werden muss

            //Clients zählen
            myCommand.CommandText = "Select * from metaevo_network WHERE type = 'client'";
            myCommand.Connection.Open();
            myReader = myCommand.ExecuteReader();

            number_clients = 0;

            while (myReader.Read())
            {
                number_clients++;
            }
            myCommand.Connection.Close();

            if (number_clients > 0)
            {
                Clients_old = Clients;

                //Neues Client-Array erstellen
                Clients = new Client[number_clients];

                DateTime tmp2;

                //Clients neu einlesen
                try
                {
                    myCommand.CommandText = "Select * from metaevo_network WHERE type = 'client'";
                    myCommand.Connection.Open();
                    myReader = myCommand.ExecuteReader();

                    number_clients = 0;

                    while (myReader.Read())
                    {
                        tmp2 = new DateTime(myReader.GetMySqlDateTime(3).Year, myReader.GetMySqlDateTime(3).Month, myReader.GetMySqlDateTime(3).Day, myReader.GetMySqlDateTime(3).Hour, myReader.GetMySqlDateTime(3).Minute, myReader.GetMySqlDateTime(3).Second);
                        Clients[number_clients] = new Client(ref mycon, myReader.GetString(0), myReader.GetString(2), tmp2, myReader.GetDouble(4), myReader.GetDouble(5), 0);
                        number_clients++;
                    }
                    myReader.Close();
                    myCommand.Connection.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Fehler beim Lesen der Datenbank: " + (ex.Message), "MetaEvo - Network");
                }

                //Auf Bedingungen für neues Scheduling prüfen
                //  Anzahl der Clients hat sich verändert
                if (Clients_old.Length != Clients.Length) back = true;

                for (int i = 0; i < Clients.Length; i++) {
                    //  Client ist defekt (50% über lowest speed)
                    if (DateTime.Now.Subtract(Clients[i].timestamp).TotalMilliseconds > 1.5 * Clients[i].speed_low) {
                        back = true;
                        if (Clients[i].speed_av != 1000)
                        {
                            //MessageBox.Show("Function 'update_From_DB': \r\nClient '" + Clients[i].ipName + "' Fehlerhaft: \r\nVerbrauchte Zeit: " + DateTime.Now.Subtract(Clients[i].timestamp).TotalMilliseconds + " \r\nErlaubte Zeit: " + 1.2 * Clients[i].speed_low + "\r\nRechnung: \r\nJetzt: " + DateTime.Now + "\r\nClient Timestamp: " + Clients[i].timestamp, "MetaEvo - Network");
                            Clients[i].set_AlsoInDB("error", -1, -1);
                        }
                    }

                    //  Falls ein Client auf Ready ist, es aber noch raw-Individuen gibt
                    if ((Clients[i].status == "ready") && (individuums2calculate_exist()))
                    {
                        back = true;
                    }
                }
            }
            //keine Clients vorhanden
            else
            {
                Clients = new Client[0];
            }
            return back;
        }

        //Anzahl der nach scheduling bisher zu berechnenden Individuen updaten
        private bool individuums2calculate_exist()
        {
            //Anzahl der Individuen in jedem Client auf 0 setzen
            for (int k = 0; k < Clients.Length; k++)
            {
                Clients[k].numberindividuums = 0;
            }

            bool back = false;

            try
            {
                myCommand.CommandText = "Select status from metaevo_individuums where status = 'raw' LIMIT 1";
                myCommand.Connection.Open();
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    back = true;
                }

                myReader.Close();
                myCommand.Connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Fehler beim Lesen der Datenbank: " + (ex.Message), "MetaEvo - Network");
            }
            return back;
        }
    }
}
