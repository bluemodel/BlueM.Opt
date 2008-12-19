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

        MySqlConnection mycon;
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
            string tmptxt = "UPDATE `metaevo_network` SET ";

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
        private int number_clients_old;

        public Client[] Clients;

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
            myCommand = new MySqlCommand("Select * from metaevo_network WHERE type = 'client'", mycon);
            mycon.Open();
            myReader = myCommand.ExecuteReader();

            number_clients = 0;
            number_clients_old = Clients.Length;

            while (myReader.Read())
            {
                number_clients++;
            }
            mycon.Close();

            if (number_clients > 0)
            {
                //Altes Client-Array zwischenspeichern
                Client[] tmp = this.Clients;

                //Array mit Clients ggf. erweitern
                if (number_clients > Clients.Length)
                {
                    Clients = new Client[number_clients];
                }
                
                DateTime tmp2;
               
                //Clients einlesen
                try
                {
                    myCommand.CommandText = "Select * from metaevo_network WHERE type = 'client'";
                    myCommand.Connection.Open();
                    myReader = myCommand.ExecuteReader();

                    number_clients = 0;

                    while (myReader.Read())
                    {
                        //Updaten
                        if (number_clients < number_clients_old)
                        {
                            //Kopieren der bisherigen Daten des Servers
                            Clients[number_clients] = tmp[number_clients];

                            //Überprüfungen ob neues Scheduling erforderlich ist
                            if (!(back))
                            {
                                //Falls sich der Status eines Clients in der DB unterscheidet vom gespeicherten Status
                                if (Clients[number_clients].status != myReader.GetString(2)) back = true;
                            }
                            if (!(back))
                            {
                                //5% Toleranz für durchschnittliche Geschwindigkeit des Clients bis neues Scheduling berechnet werden muss
                                if (Math.Abs(Clients[number_clients].speed_av - myReader.GetDouble(4)) > Clients[number_clients].speed_av * 0.05) back = true;
                            }
                            if (!(back))
                            {
                                //20% Toleranz für maximale Berechnungsdauer bis neues Scheduling den alive-Status der Individuen prüfen muss (hängt sehr vom Server ab)
                                if (Clients[number_clients].timestamp.Subtract(DateTime.Now).TotalMilliseconds > 1.2 * Clients[number_clients].speed_low) back = true;
                            }
                            Clients[number_clients].status = myReader.GetString(2);
                            Clients[number_clients].timestamp = new DateTime(myReader.GetMySqlDateTime(3).Year, myReader.GetMySqlDateTime(3).Month, myReader.GetMySqlDateTime(3).Day, myReader.GetMySqlDateTime(3).Hour, myReader.GetMySqlDateTime(3).Minute, myReader.GetMySqlDateTime(3).Second);
                            Clients[number_clients].speed_av = myReader.GetDouble(4);
                            Clients[number_clients].speed_low = myReader.GetDouble(5);
                        }

                        //oder neu anlegen
                        else
                        {

                            tmp2 = new DateTime(myReader.GetMySqlDateTime(3).Year, myReader.GetMySqlDateTime(3).Month, myReader.GetMySqlDateTime(3).Day, myReader.GetMySqlDateTime(3).Hour, myReader.GetMySqlDateTime(3).Minute, myReader.GetMySqlDateTime(3).Second);
                            Clients[number_clients] = new Client(ref mycon, myReader.GetString(0), myReader.GetString(2), tmp2, myReader.GetDouble(4), myReader.GetDouble(5), 0);
                            back = true;
                        }

                        number_clients++;
                    }
                    myReader.Close();
                    mycon.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Fehler beim Lesen der Datenbank: " + (ex.Message), "MetaEvo - Network");
                }

                update_numberclients(ref Clients);
            }
            return back;
        }

        //Aktuelle Berechnungszeiten löschen (neues Scheduling)
        public void erase_current_calc_times()
        {
            for (int k = 0; k < Clients.Length; k++)
            {
                Clients[k].numberindividuums = 0;
            }
        }

        //Anzahl der nach scheduling bisher zu berechnenden Individuen updaten
        private void update_numberclients(ref Client[] Clients)
        {
            erase_current_calc_times();

            try
            {
                myCommand.CommandText = "Select ipName from metaevo_individuums";
                myCommand.Connection.Open();
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    for (int j = 0; j < Clients.Length; j++ )
                    {
                        if (Clients[j].ipName == myReader.GetString(0)) { Clients[j].numberindividuums++; }
                    }
                }

                myReader.Close();
                mycon.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Fehler beim Lesen der Datenbank: " + (ex.Message), "MetaEvo - Network");
            }
        }
    }
}
