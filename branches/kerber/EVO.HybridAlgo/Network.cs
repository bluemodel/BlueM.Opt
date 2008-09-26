﻿using System;
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
        public string status;
        public readonly DateTime timestamp; //Zeit seit dem letzten Update der Db durch diesen Client
        public double speed_av; //in Millisekunden durchschnittliche Zeit
        public double speed_low; //in Millisekunden langsamste Zeit
        public double current_calc_time; //in Millisekunden wie lange dieser Client wohl für seine Individuen braucht
        public int numberindividuums;   //Wie viele Individuen der Client berechnen soll

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
            current_calc_time = speed_av_input * numberindividuums_input;  //In Millisekunden
            numberindividuums = numberindividuums_input;

            myCommand = new MySqlCommand();
            myCommand.Connection = mycon_input;
        }

        //In der Datenbank updaten
        public void set_AlsoInDB(string status_input, double speed_av_input, double speed_low_input, int numberindividuums_input)
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
            if (numberindividuums_input != -1)
            {
                tmptxt = tmptxt + "individuums = '" + numberindividuums_input + "' ";
                this.numberindividuums = numberindividuums_input;
            }
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

            number_clients = 1;
        }

        //Aktuelle Daten aus der DB holen
        public void update_From_DB()
        {
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

            //Array mit Clients erzeugen
            Clients = new Client[number_clients]; 

            //Clients einlesen
            myCommand = new MySqlCommand("Select * from metaevo_network WHERE type = 'client'", mycon);
            mycon.Open();
            myReader = myCommand.ExecuteReader();

            number_clients = 0;

            while (myReader.Read())
            {
                Clients[number_clients] = new Client(ref mycon, myReader.GetString(0), myReader.GetString(2), myReader.GetDateTime(3), myReader.GetDouble(4), myReader.GetDouble(5), myReader.GetInt32(6));
                number_clients++;
            }
            myReader.Close();
            mycon.Close();
        }

        //Aktuelle Berechnungszeiten löschen (neues Scheduling)
        public void erase_current_calc_times()
        {
            for (int k = 0; k < Clients.Length; k++)
            {
                Clients[k].current_calc_time = 0;
                Clients[k].numberindividuums = 0;
            }
        }
    }
}
