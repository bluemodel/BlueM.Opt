using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace IHWB.EVO.HybridAlgo
{
    public class Networkmanager
    {
        //### Variablen ###
        MySqlConnection mycon;
        MySqlCommand myCommand;
        MySqlDataReader myReader;

        EVO.Common.EVO_Settings settings;

        //### Konstruktor ###
        public Networkmanager(EVO.Common.EVO_Settings settings_input)
        {   
            this.settings = settings_input;
            mycon = new MySqlConnection("datasource=" + settings.Hybrid2008.MySQL_Host + ";username=" + settings.Hybrid2008.MySQL_User + ";password=" + settings.Hybrid2008.MySQL_Password + ";database=information_schema");
            myCommand = new MySqlCommand();
            myCommand.Connection = mycon;
        }

        //### Methoden ###
        //Verbindung zur DB prüfen bzw. Fehler ausgeben
        public bool check_connection()
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
        public void database_init()   //ToDO: Fertiges Individuum oder Problem übergeben
        {    
            //Datenbank
            myCommand.CommandText = "CREATE DATABASE IF NOT EXISTS " + settings.Hybrid2008.MySQL_Database;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();

            //Wechseln in neue Datenbank
            myCommand.Connection.ConnectionString = "datasource=" + settings.Hybrid2008.MySQL_Host + ";username=" + settings.Hybrid2008.MySQL_User + ";password=" + settings.Hybrid2008.MySQL_Password + ";database=" + settings.Hybrid2008.MySQL_Database;

            //benötigte Tabellen (->Problem/Individuum) erzeugen
            myCommand.CommandText = "CREATE TABLE IF NOT EXISTS test (col1 INT, col2 CHAR(5))";
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }

        //Datenbank löschen
        public void database_delete()
        {
            myCommand.CommandText = "DROP DATABASE IF EXISTS " + settings.Hybrid2008.MySQL_Database;
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }

        //Individuen in der Datenbank Speichern
        public void Individuums_write_to_DB(EVO.Common.Individuum[] generation_input, string tablename_input)
        {
            /*myCommand = new MySqlCommand("Select type from individuen_client WHERE id = " + id, mycon);
                mycon.Open();

                MySqlDataReader myReader;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    back = myReader.GetString(0);
                }
                myReader.Close();
                mycon.Close();
        */
        }
        /*
        public EVO.Common.Individuum Individuum_read_from_DB(string tablename_input)
        {

        }

        public void Individuum_delete_from_DB(string tablename, int id_individuum)
        {

        }

        public string[] Statusarray_get()
        {

        }
        */
        //Statusinformationen in der Datenbank setzen
        public void Statusarray_set(string[] statusarray_input)
        {

        }
    }
}
