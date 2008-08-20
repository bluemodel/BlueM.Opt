using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace IHWB.EVO.HybridAlgo
{
    public class Controller
    {
        //### Variablen ###
        EVO.Common.EVO_Settings settings;

        Networkmanager networkmanager2008;

        string role;
        bool ok;

        //### Konstruktor ###
        public Controller(EVO.Common.EVO_Settings settings_input)  //Problem und Zeichner sollte noch übergeben werden
        {
            //Daten einlesen
            this.ok = true;
            this.settings = settings_input;
            this.role = this.settings.Hybrid2008.Role; 

            //Falls Berechnung im Netzwerk: Networkmanager instanziieren und Verbindung zur Datenbank checken
            if (settings.Hybrid2008.Role != "Single PC")
            {
                this.ok = false;
                networkmanager2008 = new Networkmanager(this.settings);
                this.ok = networkmanager2008.check_connection();
            }

            //Ausführen des Hauptprogramms
            if (ok)
            {
                networkmanager2008.database_init();
            }
        }

        //### Methoden ###
        public void testmethode()
        {
            MessageBox.Show("Testmethode wird ausgeführt");
        }
    }
}
