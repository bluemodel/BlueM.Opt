using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using MySql.Data.MySqlClient;

namespace IHWB.EVO.MetaEvo
{
    
    //Struktur eines Clients

    public class Client
    {
        public readonly string ipName;
        public string status;               //{Client Status: {claculating, error, ready} Server Status: {init Genpool, generate Individuums, waiting for client-calculation, finished}
        public DateTime timestamp; //Zeit seit dem letzten Update der Db durch diesen Client
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
                tmptxt = tmptxt + "speed_av = '" + speed_av_input + "' ";
                this.speed_av = speed_av_input;
            }
            //speed_low
            if (speed_low_input != -1)
            {
                tmptxt = tmptxt + "speed_low = '" + speed_low_input + "' ";
                this.speed_low = speed_low_input;
            }
            //individuen
            /*if (numberindividuums_input != -1)
            {
                tmptxt = tmptxt + "individuums = '" + numberindividuums_input + "' ";
                this.numberindividuums = numberindividuums_input;
            }*/
            tmptxt = tmptxt.TrimEnd(',', ' ') + " WHERE ipName = '" + this.ipName + "' LIMIT 1;";
            myCommand.CommandText = tmptxt;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }

        //Von der Datenbank updaten
        public void get_NumberIndividuumsFromDB()
        {
            myCommand.CommandText ="Select * from metaevo_network WHERE ipName = '" + this.ipName + "'";
            myCommand.Connection.Open();
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                this.numberindividuums = myReader.GetInt32(6);
            }
            myCommand.Connection.Close();
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

        public Network(ref MySqlConnection mycon_input)
        {
            //Datenbankverbindung
            mycon = mycon_input;
            myCommand = new MySqlCommand();
            myCommand.Connection = mycon;

            number_clients = 0;
        }

        //Aktuelle Daten aus der DB holen
        public bool update_From_DB()
        {
            Client[] tmp = this.Clients;
            bool back = false; //Ob Scheduling neu berechnet werden muss

            //Clients zählen
            myCommand = new MySqlCommand("Select * from metaevo_network WHERE type = 'client'", mycon);
            mycon.Open();
            myReader = myCommand.ExecuteReader();

            this.number_clients = 0;

            while (myReader.Read())
            {
                number_clients++;
            }
            mycon.Close();

            //Array mit Clients ggf. erweitern
            if (number_clients > Clients.Length)
            {
                Clients = new Client[number_clients];
            }

            //Clients einlesen
            myCommand.CommandText = "Select * from metaevo_network WHERE type = 'client'";
            myCommand.Connection.Open();
            myReader = myCommand.ExecuteReader();

            number_clients = 0;

            while (myReader.Read())
            {
                //Updaten
                if (number_clients < Clients.Length) 
                {
                    Clients[number_clients] = tmp[number_clients];
                    if ((Clients[number_clients].status != myReader.GetString(2)) || (Clients[number_clients].speed_av != myReader.GetDouble(4)) ) back = true;
                    Clients[number_clients].status = myReader.GetString(2);
                    Clients[number_clients].timestamp = myReader.GetDateTime(3);
                    Clients[number_clients].speed_av = myReader.GetDouble(4);
                    Clients[number_clients].speed_low = myReader.GetDouble(5);
                }

                //oder neu anlegen
                else 
                {
                    Clients[number_clients] = new Client(ref mycon, myReader.GetString(0), myReader.GetString(2), myReader.GetDateTime(3), myReader.GetDouble(4), myReader.GetDouble(5), 0);
                    back = true;
                }

                number_clients++;
            }
            myReader.Close();
            mycon.Close();
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
    }
}
