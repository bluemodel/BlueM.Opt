using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Net;

namespace IHWB.EVO.MetaEvo
{
    public class Controller
    {
        //### Variablen ###
        EVO.Common.Problem prob; 
        EVO.Common.EVO_Settings settings;
        EVO.Diagramm.Hauptdiagramm hauptdiagramm1;

        Networkmanager networkmanager;
        EVO.Common.Individuum_MetaEvo individuumForClient;
        EVO.Common.Individuum_MetaEvo[] generation;
        Algomanager algomanager;

        int individuumnumber;
        string role;

        //### Konstruktor ###  
        public Controller(ref EVO.Common.Problem prob_input, ref EVO.Common.EVO_Settings settings_input, ref EVO.Diagramm.Hauptdiagramm hauptdiagramm_input, ref EVO.Apps.Sim sim_input)  
        {
            //Daten einlesen
            this.prob = prob_input;
            this.settings = settings_input;
            this.hauptdiagramm1 = hauptdiagramm_input;

            this.role = this.settings.MetaEvo.Role;

            //Setzen des Problems zum Design des Individuums
            EVO.Common.Individuum_MetaEvo.Initialise(ref prob_input);
            individuumnumber = 0;

            switch (this.role)
            {
                case "Single PC":
                    //### Vorbereitung ###
                    //Initialisieren des Individuum-Arrays
                    generation = new EVO.Common.Individuum_MetaEvo[this.settings.MetaEvo.PopulationSize];
                    for (int j = 0; j < this.settings.MetaEvo.PopulationSize; j++)
                    {
                        generation[j] = new EVO.Common.Individuum_MetaEvo("MetaEvo", individuumnumber, prob_input.List_OptParameter.Length);
                        individuumnumber++;
                    }
                    //Zufällige Parents setzen
                    set_random_adults();

                    //Algomanager starten
                    algomanager = new Algomanager(ref prob);

                    //### Hauptprogramm ###
                    start_single_pc(ref sim_input);
                    break;

                case "Network Server":
                    //### Vorbereitung ###
                    //Initialisieren des Individuum-Arrays
                    generation = new EVO.Common.Individuum_MetaEvo[this.settings.MetaEvo.PopulationSize];
                    for (int j = 0; j < this.settings.MetaEvo.PopulationSize; j++)
                    {
                        generation[j] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuumnumber, prob_input.List_OptParameter.Length);
                        individuumnumber++;
                    }

                    //Algomanager starten
                    algomanager = new Algomanager(ref prob);

                    //### Hauptprogramm ###
                    networkmanager = new Networkmanager(ref this.generation[0], ref this.settings);
                    start_network_server(ref sim_input);
                    break;

                case "Network Client":
                    //### Vorbereitung ###
                    individuumForClient = new EVO.Common.Individuum_MetaEvo("MetaEvo", individuumnumber, prob_input.List_OptParameter.Length);

                    //### Hauptprogramm ###
                    networkmanager = new Networkmanager(ref this.individuumForClient, ref this.settings);
                    start_network_client(ref sim_input);
                    break;
            }  
        }



        //### Methoden ###

        // Zufällige Eltern setzen
        private bool set_random_adults()
        {
            double[] random;
            Random randomizer = new Random();

            //Für jedes Individuum durchgehen
            for (int k = 0; k < this.settings.MetaEvo.PopulationSize; k++)
            {
                random = new double[this.prob.NumParams];
                //Für jeden Parameter durchgehen
                for (int j = 0; j < this.prob.NumParams; j++)
                {
                    int max = (int)this.prob.List_OptParameter[j].Max;
                    int min = (int)this.prob.List_OptParameter[j].Min;
                    random[j] = randomizer.Next(min, max);
                }
                generation[k].set_optparas(random);
            }
            return true;
        }



        //### Methoden ### Hauptprogramm

        // Single PC
        private void start_single_pc(ref EVO.Apps.Sim sim_input)
        {
            MessageBox.Show("Single PC wird ausgeführt");

        }

        // Network Server
        private void start_network_server(ref EVO.Apps.Sim sim_input)
        {
            Client meServer = networkmanager.Network_Init_Client_Object(Dns.GetHostName());
            meServer.status = "init Genpool";

            for (int k = 0; k < settings.MetaEvo.NumberGenerations; k++)
            {
                //Einmaliges Initialisieren des Genpools
                if (meServer.status == "init Genpool")
                {
                    //Zufällige Parents setzen und in DB schreiben
                    set_random_adults();
                    networkmanager.Individuums_WriteToDB(ref generation);

                    if (networkmanager.perform_step(ref this.generation))
                    {
                        meServer.set_AlsoInDB("generate Individuums", -1, -1, -1);
                        algomanager.set_genpool(generation);
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Initialisieren des Genpools");
                    }
                }

                //Individuen erzeugen
                if (meServer.status == "generate Individuums")
                {
                    //Fertig berechnete Individuen (alle) aus der DB in generation updaten
                    networkmanager.Individuums_UpdateFromDB(ref this.generation);

                    //Evolutionsschritte
                    algomanager.eval_and_build(ref this.generation);

                    //Neue Individuen in die DB schreiben
                    networkmanager.Individuums_WriteToDB(ref this.generation);

                    //Neuen Serverstatus setzen
                    meServer.set_AlsoInDB("error watching", -1, -1, -1);
                }

                //Individuen berechnen lassen
                if (meServer.status == "error watching")
                {
                    if (networkmanager.perform_step(ref this.generation))
                    {
                        meServer.set_AlsoInDB("generate Individuums", -1, -1, -1);
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Ausführen der Kalkulation im Netzwerk");
                    }
                }
            }
            meServer.set_AlsoInDB("finished", -1, -1, -1);
        }

        // Network Client
        private void start_network_client(ref EVO.Apps.Sim sim_input)
        {
            MessageBox.Show("Network Client wird ausgeführt");

            string[] serverstatus = networkmanager.Network_ReadServer();
            Client meClient = networkmanager.Network_Init_Client_Object(Dns.GetHostName());

            //Solange der Server noch nicht fertig ist
            while (serverstatus[0] != "finished") {

                networkmanager.Individuum_ReadFromDB_Client(ref individuumForClient);

                //Falls Individuum existiert, berechnen
                if (individuumForClient.ID != 0)
                {
                    if (meClient.status != "calculating")
                    {
                        //Status zuweisen
                        meClient.set_AlsoInDB("calculating", -1, -1, -1);
                        //Anzahl der zu berechnenden Individuen aus der DB lesen
                        meClient.get_NumberIndividuumsFromDB();
                    }
                    //Individuum in DB als "calculate" markieren
                    networkmanager.Individuum_UpdateInDB(ref individuumForClient, "status", "calculate");
                    //Simulieren
                    sim_input.Evaluate(ref individuumForClient);
                    //Individuum in DB Updaten
                    networkmanager.Individuum_UpdateInDB(ref individuumForClient, "status feat const", "true");
                }

                // Wenn kein Individuum mehr da ist, warten
                else
                {
                    if (meClient.status != "ready")
                    {
                        //Status zuweisen
                        meClient.set_AlsoInDB("ready", -1, -1, -1);
                    }
                    System.Threading.Thread.Sleep(5000);
                }
                serverstatus = networkmanager.Network_ReadServer();
            }
        }



        //### Methoden ### Subroutinen
    }
}
