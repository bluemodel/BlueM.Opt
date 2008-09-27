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
            individuumnumber = 1;

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
                    individuumForClient = new EVO.Common.Individuum_MetaEvo("MetaEvo", 0, prob_input.List_OptParameter.Length);

                    //### Hauptprogramm ###
                    networkmanager = new Networkmanager(ref this.individuumForClient, ref this.settings, ref applog);
                    start_network_client(ref sim_input);
                    break;
            }  
        }



        //### Methoden ###

        // Zufällige Eltern setzen
        private bool set_random_parents(ref EVO.Common.Individuum_MetaEvo[] generation_input)
        {
            double[] random;
            Random randomizer = new Random();
            if (applog.log) applog.appendText("Controller: Construct random Genpool");

            //Für jedes Individuum durchgehen
            for (int k = 0; k < this.settings.MetaEvo.PopulationSize; k++)
            {
                random = new double[this.prob.NumParams];
                //Für jeden Parameter durchgehen
                for (int j = 0; j < this.prob.NumParams; j++)
                {
                    double max = this.prob.List_OptParameter[j].Max;
                    double min = this.prob.List_OptParameter[j].Min;
                    random[j] = min + (max - min)*((double)randomizer.Next(0, 1000)/1000);
                }
                generation_input[k].set_optparas(random);
                generation_input[k].set_status("raw");
            }
            return true;
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
                    //Zufällige Parents setzen
                    set_random_parents(ref generation);

                    //Genpool simulieren und setzen
                    if (applog.log) applog.appendText("Controller: Genpool: Simulating Individuums...");
                    for (int i = 0; i < generation.Length; i++)
                    {
                        //Simulieren 
                        sim_input.Evaluate_MetaEvo(ref generation[i]);
                        if (applog.log) applog.appendText("Controller: Individuum " + generation[i].ID + " (" + Math.Round(((double)(i + 1) / (double)generation.Length),2) * 100 + "%)");
                    }
                    algomanager.set_genpool(ref generation);


                    mePC.status = "perform";
                }

                else if (mePC.status == "perform")
                {
                    //Neue Generation
                    if (applog.log) applog.appendText("Controller: ### Building new Individuums for Generation " + generationcounter + " ###");
                    algomanager.new_individuals_build(ref generation);
                    
                    //Neue Generation Simulieren
                    if (applog.log) applog.appendText("Controller: Individuums for Generation " + generationcounter + ": Simulating Individuums...");
                    for (int i = 0; i < generation.Length; i++)
                    {
                        //Simulieren 
                        sim_input.Evaluate_MetaEvo(ref generation[i]);
                        if (applog.log) applog.appendText("Controller: Individuum " + generation[i].ID + " (" + Math.Round(((double)(i + 1) / (double)generation.Length),2) * 100 + "%)");
                    }
                    //neue Individuen Zeichnen
                    algomanager.draw_individuals(ref generation, ref hauptdiagramm1);

                    //Neue Generation auswerten und neuen Genpool erstellen 
                    algomanager.new_individuals_merge_with_genpool(ref generation);

                    //Genpool Zeichnen
                    algomanager.draw_genpool(ref hauptdiagramm1);

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
                    set_random_parents(ref generation);

                    //Von den Clients ausrechnen lassen
                    if (applog.log) applog.appendText("Controller: Calculate Genpool by Clients");
                    if (networkmanager.calculate_by_clients(ref generation))
                    {
                        algomanager.set_genpool(ref generation);
                        meServer.set_AlsoInDB("generate Individuums", -1, -1);
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Initialisieren des Genpools");
                    }
                }

                //Individuen erzeugen
                else if (meServer.status == "generate Individuums")
                {
                    //Neue Individuen Zeichnen
                    algomanager.draw_individuals(ref generation, ref hauptdiagramm1);

                    //Mit Genpool verrechnen
                    algomanager.new_individuals_merge_with_genpool(ref generation);

                    //Genpool Zeichnen
                    algomanager.draw_genpool(ref hauptdiagramm1);

                    //Evolutionsschritte
                    algomanager.new_individuals_build(ref generation);

                    //Neuen Serverstatus setzen
                    meServer.set_AlsoInDB("waiting for client-calculation", -1, -1);
                }

                //Individuen berechnen lassen
                else if (meServer.status == "waiting for client-calculation")
                {
                    //Von den Clients ausrechnen lassen
                    if (networkmanager.calculate_by_clients(ref generation))
                    {
                        generationcounter++;
                        meServer.set_AlsoInDB("generate Individuums", -1, -1);
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Ausführen der Kalkulation im Netzwerk");
                    }
                }
            }
            meServer.set_AlsoInDB("finished", -1, -1);
            if (applog.log) applog.appendText("Controller: Calculation Finished");
        }

        // Network Client
        private void start_network_client(ref EVO.Apps.Sim sim_input)
        {
            Client meClient = new Client();
            meClient.numberindividuums = 0;
            meClient = networkmanager.Network_Init_Client_Object(Dns.GetHostName());
            string[] serverstatus = networkmanager.Network_ReadServer();
            DateTime Berechnungsstart;
            double Berechnungsdauer;
            
            //Solange der Server noch nicht fertig ist
            while (serverstatus[0] != "finished") {

                networkmanager.Individuum_ReadFromDB_Client(ref individuumForClient);

                //Falls Individuum existiert, berechnen
                if (individuumForClient.get_status() == "raw")
                {
                    //Status zuweisen
                    meClient.set_AlsoInDB("calculating", -1, -1);
                    meClient.numberindividuums++;

                    //Individuum in DB als "calculate" markieren
                    networkmanager.Individuum_UpdateInDB(ref individuumForClient, "status", "calculate");

                    //Simulieren
                    Berechnungsstart = DateTime.Now;
                    if (applog.log) applog.appendText("Controller: Individuum " + individuumForClient.ID + " simulating...");
                    sim_input.Evaluate_MetaEvo(ref individuumForClient);

                    //Neuen Speed-Daten berechnen
                    Berechnungsdauer = DateTime.Now.Subtract(Berechnungsstart).Milliseconds;
                    meClient.speed_av += (meClient.speed_av - Berechnungsdauer) / meClient.numberindividuums;
                    if (Berechnungsdauer > meClient.speed_low) meClient.speed_low = Berechnungsdauer;
                    if (applog.log) applog.appendText("Controller: Average Speed is set to " + meClient.speed_av + " Milliseconds, Lowest Speed is set to "+meClient.speed_low+" Milliseconds");

                    //Individuum in DB Updaten
                    networkmanager.Individuum_UpdateInDB(ref individuumForClient, "status feat const", individuumForClient.get_status());

                    //Client ind DB Updaten
                    meClient.set_AlsoInDB("", meClient.speed_av, meClient.speed_low);
                }

                // Wenn kein Individuum mehr da ist, warten
                else
                {
                    meClient.set_AlsoInDB("ready", -1, -1);
                    if (applog.log) applog.appendText("Controller: No Individuum found in DB (for this Client) - waiting...");
                    System.Threading.Thread.Sleep(3000);
                }

                //Serverstatus neu lesen
                serverstatus = networkmanager.Network_ReadServer();
            }
            if (applog.log) applog.appendText("Controller: Calculation Finished");
        }
    }
}
