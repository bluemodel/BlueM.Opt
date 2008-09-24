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
        EVO.Diagramm.ApplicationLog applog;

        int individuumnumber;
        string role;

        //### Konstruktor ###  
        public Controller(ref EVO.Common.Problem prob_input, ref EVO.Common.EVO_Settings settings_input, ref EVO.Diagramm.Hauptdiagramm hauptdiagramm_input, ref EVO.Apps.Sim sim_input)
        {
            applog = new IHWB.EVO.Diagramm.ApplicationLog();
            if (settings_input.MetaEvo.Log) applog.log = true;

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
                    if (applog.log) applog.appendText("Controller: MetaEvo started in 'Single PC'-Mode");
                    //### Vorbereitung ###
                    //Initialisieren des Individuum-Arrays
                    generation = new EVO.Common.Individuum_MetaEvo[this.settings.MetaEvo.PopulationSize];
                    for (int j = 0; j < this.settings.MetaEvo.PopulationSize; j++)
                    {
                        generation[j] = new EVO.Common.Individuum_MetaEvo("MetaEvo", individuumnumber, prob_input.List_OptParameter.Length);
                        individuumnumber++;
                    }

                    //Algomanager starten
                    algomanager = new Algomanager(ref prob, individuumnumber, ref applog);

                    //### Hauptprogramm ###
                    start_single_pc(ref sim_input);
                    break;

                case "Network Server":
                    if (applog.log) applog.appendText("Controller: MetaEvo started in 'Network Server'-Mode");
                    //### Vorbereitung ###
                    //Initialisieren des Individuum-Arrays
                    generation = new EVO.Common.Individuum_MetaEvo[this.settings.MetaEvo.PopulationSize];
                    for (int j = 0; j < this.settings.MetaEvo.PopulationSize; j++)
                    {
                        generation[j] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuumnumber, prob_input.List_OptParameter.Length);
                        individuumnumber++;
                    }

                    //Algomanager starten
                    algomanager = new Algomanager(ref prob, individuumnumber, ref applog);

                    //### Hauptprogramm ###
                    networkmanager = new Networkmanager(ref this.generation[0], ref this.settings, ref applog);
                    start_network_server(ref sim_input);
                    break;

                case "Network Client":
                    if (applog.log) applog.appendText("Controller: MetaEvo started in 'Network Client'-Mode");
                    //### Vorbereitung ###
                    individuumForClient = new EVO.Common.Individuum_MetaEvo("MetaEvo", individuumnumber, prob_input.List_OptParameter.Length);

                    //### Hauptprogramm ###
                    networkmanager = new Networkmanager(ref this.individuumForClient, ref this.settings, ref applog);
                    start_network_client(ref sim_input);
                    break;
            }
        }


        //### Methoden ### Hauptprogramm

        // Single PC
        private void start_single_pc(ref EVO.Apps.Sim sim_input)
        {
            Client mePC = new Client(); 
            mePC.status = "init";
            int generationcounter = 1;

            while (generationcounter < settings.MetaEvo.NumberGenerations)
            {
                if (mePC.status == "init")
                {
                    //Genpool setzen
                    algomanager.set_genpool(generation);

                    mePC.status = "perform";
                }

                else if (mePC.status == "perform")
                {
                    if (applog.log) applog.appendText("Controller: ### Starting new Generation ###");
                    if (applog.log) applog.appendText("Controller: Gen" + generationcounter + ": Simulate Individuals");

                    //Simulieren 
                    if (applog.log) applog.appendText("Controller: Simulating...");
                    for (int i = 0; i < generation.Length; i++)
                    {
                        sim_input.Evaluate_MetaEvo(ref generation[i]);
                    }
                    if (applog.log) applog.appendText("Controller: Simulation finished");

                    //Zeichnen
                    algomanager.draw(ref generation, ref hauptdiagramm1);

                    //Neue Generation
                    algomanager.eval_and_build(ref generation);

                    generationcounter++;
                }
            }
            if (applog.log) applog.appendText("Controller: Calculation Finished");
        }

        // Network Server
        private void start_network_server(ref EVO.Apps.Sim sim_input)
        {
            Client meServer = new Client(); 
            meServer = networkmanager.Network_Init_Client_Object(Dns.GetHostName());
            meServer.status = "init Genpool";
            int generationcounter = 0;

            while (generationcounter < settings.MetaEvo.NumberGenerations)
            {
                //Einmaliges Initialisieren des Genpools
                if (meServer.status == "init Genpool")
                {
                    //Zufällige Parents setzen und in DB schreiben
                    networkmanager.Individuums_WriteToDB(ref generation);

                    if (networkmanager.perform_step(ref generation))
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
                else if (meServer.status == "generate Individuums")
                {
                    //Fertig berechnete Individuen (alle) aus der DB in generation updaten
                    networkmanager.Individuums_UpdateFromDB(ref generation);

                    //Zeichnen
                    algomanager.draw(ref generation, ref hauptdiagramm1);

                    //Evolutionsschritte
                    algomanager.eval_and_build(ref this.generation);

                    //Neue Individuen in die DB schreiben
                    networkmanager.Individuums_WriteToDB(ref generation);

                    //Neuen Serverstatus setzen
                    meServer.set_AlsoInDB("error watching", -1, -1, -1);

                    generationcounter++;
                }

                //Individuen berechnen lassen
                else if (meServer.status == "error watching")
                {
                    if (networkmanager.perform_step(ref generation))
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
            if (applog.log) applog.appendText("Controller: Calculation Finished");
        }

        // Network Client
        private void start_network_client(ref EVO.Apps.Sim sim_input)
        {

            Client meClient = new Client();
            meClient = networkmanager.Network_Init_Client_Object(Dns.GetHostName());
            string[] serverstatus = networkmanager.Network_ReadServer();
            

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
                    sim_input.Evaluate_MetaEvo(ref individuumForClient);
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
            if (applog.log) applog.appendText("Controller: Calculation Finished");
        }



        //### Methoden ### Subroutinen
    }
}
