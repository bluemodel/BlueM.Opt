﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Net;

namespace IHWB.EVO.MetaEvo
{
    public class Controller
    {
        //### Variablen ###
        EVO.Apps.Sim sim;
        EVO.Apps.Testprobleme testprobleme;

        EVO.Common.Problem prob; 
        EVO.Common.EVO_Settings settings;
        EVO.Common.Individuum_MetaEvo individuumForClient;
        EVO.Common.Individuum_MetaEvo[] generation;
        EVO.Common.Progress progress1;

        EVO.Diagramm.Hauptdiagramm hauptdiagramm1;
        EVO.Diagramm.ApplicationLog applog;

        Networkmanager networkmanager;
        Algomanager algomanager;

        int individuumnumber;
        string role;

        //### Konstruktor ###  
        public Controller(ref EVO.Common.Problem prob_input, ref EVO.Common.EVO_Settings settings_input, ref EVO.Diagramm.Hauptdiagramm hauptdiagramm_input, ref EVO.Common.Progress progress1, ref EVO.Apps.Testprobleme testprobleme_input)
        {
            settings_input.MetaEvo.Application = "testprobleme";
            this.testprobleme = testprobleme_input;
            init(ref prob_input, ref settings_input, ref hauptdiagramm_input, ref progress1);
        }
        public Controller(ref EVO.Common.Problem prob_input, ref EVO.Common.EVO_Settings settings_input, ref EVO.Diagramm.Hauptdiagramm hauptdiagramm_input, ref EVO.Common.Progress progress1, ref EVO.Apps.Sim sim_input)
        {
            settings_input.MetaEvo.Application = "sim";
            this.sim = sim_input;
            init(ref prob_input, ref settings_input, ref hauptdiagramm_input, ref progress1);
        }

        //### Konstruktor ###
        public void init(ref EVO.Common.Problem prob_input, ref EVO.Common.EVO_Settings settings_input, ref EVO.Diagramm.Hauptdiagramm hauptdiagramm_input, ref EVO.Common.Progress progress1_input)  
        {
            applog = new IHWB.EVO.Diagramm.ApplicationLog(ref settings_input);

            //Daten einlesen
            this.prob = prob_input;
            this.settings = settings_input;
            this.progress1 = progress1_input;
            this.hauptdiagramm1 = hauptdiagramm_input;

            this.role = this.settings.MetaEvo.Role;

            //Setzen des Problems zum Design des Individuums
            EVO.Common.Individuum_MetaEvo.Initialise(ref prob_input);
            individuumnumber = 1;
            applog.appendText("Controller: Task: " + prob.Datensatz);

            //Progress Initialisieren
            progress1.Initialize(1, 1, (short)settings.MetaEvo.NumberGenerations, (short)(settings.MetaEvo.ChildsPerParent * settings.MetaEvo.PopulationSize));

            switch (this.role)
            {
                case "Single PC":
                     applog.appendText("Controller: MetaEvo started in 'Single PC'-Mode");
                    //### Vorbereitung ###
                    //Initialisieren des Individuum-Arrays
                    generation = new EVO.Common.Individuum_MetaEvo[this.settings.MetaEvo.PopulationSize];
                    for (int j = 0; j < this.settings.MetaEvo.PopulationSize; j++)
                    {
                        generation[j] = new EVO.Common.Individuum_MetaEvo("MetaEvo", individuumnumber, prob_input.List_OptParameter.Length);
                        individuumnumber++;
                    }

                    //Algomanager starten
                    algomanager = new Algomanager(ref prob, ref settings, individuumnumber, ref applog, ref hauptdiagramm1);

                    //### Hauptprogramm ###
                    start_single_pc();
                    break;

                case "Network Server":
                     applog.appendText("Controller: MetaEvo started in 'Network Server'-Mode");
                    //### Vorbereitung ###
                    //Initialisieren des Individuum-Arrays
                    generation = new EVO.Common.Individuum_MetaEvo[this.settings.MetaEvo.PopulationSize];
                    for (int j = 0; j < this.settings.MetaEvo.PopulationSize; j++)
                    {
                        generation[j] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuumnumber, prob_input.List_OptParameter.Length);
                        individuumnumber++;
                    }

                    //Algomanager starten
                    algomanager = new Algomanager(ref prob, ref settings, individuumnumber, ref applog, ref hauptdiagramm1);

                    //### Hauptprogramm ###
                    networkmanager = new Networkmanager(ref this.generation[0], ref this.settings, ref applog);
                    start_network_server();
                    break;

                case "Network Client":
                     applog.appendText("Controller: MetaEvo started in 'Network Client'-Mode");
                    //### Vorbereitung ###
                    individuumForClient = new EVO.Common.Individuum_MetaEvo("MetaEvo", 0, prob_input.List_OptParameter.Length);
                    //Microsoft SQL-DB des Clients ausschalten
                    if (this.settings.MetaEvo.Application == "sim") { sim.StoreIndividuals = false; }

                    //### Hauptprogramm ###
                    networkmanager = new Networkmanager(ref this.individuumForClient, ref this.settings, ref applog);
                    start_network_client();
                    break;
            }  
        }



        //### Methoden ###

        // Zufällige Eltern setzen
        private bool set_random_parents(ref EVO.Common.Individuum_MetaEvo[] generation_input)
        {
            double[] random;
            Random randomizer = new Random();
            applog.appendText("Controller: Construct random Genpool");

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
        private void start_single_pc()
        {
            Client mePC = new Client(); 
            mePC.status = "init";
            int generationcounter = 1;
            string tmp = "";

            while (generationcounter <= settings.MetaEvo.NumberGenerations)
            {
                #region Zustand: init
                if (mePC.status == "init")
                {
                    //Zufällige Parents setzen
                    set_random_parents(ref generation);

                    //Genpool simulieren
                    applog.appendText("Controller: Genpool: Simulating Individuums...");
                    for (int i = 0; i < generation.Length; i++)
                    {
                        //Simulieren 
                        if (this.settings.MetaEvo.Application == "testprobleme")
                        {
                            testprobleme.Evaluierung_TestProbleme_MetaEvo(ref generation[i],0,ref hauptdiagramm1);
                        }
                        if (this.settings.MetaEvo.Application == "sim") sim.Evaluate_MetaEvo(ref generation[i]);
                        applog.appendText("Controller: Individuum " + generation[i].ID + " (" + Math.Round(((double)(i + 1) / (double)generation.Length),2) * 100 + "%)");
                    }

                    //Genpool speichern und zeichnen
                    algomanager.set_genpool(ref generation);
                    generation = new EVO.Common.Individuum_MetaEvo[this.settings.MetaEvo.ChildsPerParent*this.settings.MetaEvo.PopulationSize];

                    if (settings.MetaEvo.OpMode == "Both") mePC.status = "perform_both";
                    else if (settings.MetaEvo.OpMode == "Global Optimizer") mePC.status = "perform_global";
                    else if (settings.MetaEvo.OpMode == "Local Optimizer") mePC.status = "perform_local";
                }
                #endregion
                
                #region Zustand: perform_both oder perform global
                else if ((mePC.status == "perform_both") || (mePC.status == "perform_global"))
                {
                    //Neue Generation bauen
                    applog.appendText("Controller: ### Building new Individuums for Generation " + generationcounter + " ###");
                    algomanager.new_individuals_build(ref generation);
                    
                    //Neue Generation Simulieren
                    applog.appendText("Controller: Individuums for Generation " + generationcounter + ": Simulating Individuums...");
                    progress1.iNachf = 0;
                    for (int i = 0; i < generation.Length; i++)
                    {
                        tmp = "(";
                        for (int j = 0; j < generation[0].get_optparas().Length; j++)
                        {
                            tmp = tmp + generation[i].get_optparas()[j] + " / ";
                        }
                        tmp = tmp.TrimEnd(' ','/') + ")";
                        

                        //Simulieren und zeichnen
                        if (generation[i].get_toSimulate()){
                            if (this.settings.MetaEvo.Application == "testprobleme") 
                            {
                                applog.appendText("Controller: Simulating Individuum " + generation[i].ID + " (" + Math.Round(((double)(i + 1) / (double)generation.Length), 2) * 100 + "%)...   " + algomanager.algos.algofeedbackarray[generation[i].get_generator()].name); // +": Optparas: " + tmp
                                testprobleme.Evaluierung_TestProbleme_MetaEvo(ref generation[i], 0, ref hauptdiagramm1);
                            }
                            if (this.settings.MetaEvo.Application == "sim")
                            {
                                applog.appendText("Controller: Simulating Individuum " + generation[i].ID + " (" + Math.Round(((double)(i + 1) / (double)generation.Length), 2) * 100 + "%)...   " + algomanager.algos.algofeedbackarray[generation[i].get_generator()].name); // +": Optparas: " + tmp
                                sim.Evaluate_MetaEvo(ref generation[i]);
                                hauptdiagramm1.ZeichneIndividuum(generation[i], 1, 1, 1, generation[i].ID % generation.Length, System.Drawing.Color.Yellow, true);
                                System.Windows.Forms.Application.DoEvents();
                            }
                        }
                        //else applog.appendText("Controller: NOT Simulating Individuum " + generation[i].ID);
                        progress1.NextNachf();
                    }

                    //Neue Individuen mit Genpool verrechnen und Genpool zeichnen
                    algomanager.new_individuals_merge_with_genpool(ref generation);

                    generationcounter++;

                    //Falls beide Berechnungsarten genutzt werden sollen, auf Umschaltpunkt prüfen
                    if (mePC.status == "perform_both") 
                    {
                        if ((settings.MetaEvo.OpMode == "Local Optimizer") || (!(generationcounter <= settings.MetaEvo.NumberGenerations)))
                        {
                            mePC.status = "perform_local";
                            generationcounter--;
                        }
                    }
                    progress1.NextGen();
                }
                #endregion

                #region Zustand: perform_local
                else if (mePC.status == "perform_local")
                {
                    //Neue Generation bauen
                    applog.appendText("Controller: ### Building new Individuums for Hook&Jeeves " + generationcounter + " ###");
                    algomanager.new_individuals_build(ref generation);

                    //Neue Generation Simulieren
                    applog.appendText("Controller: Individuums for Hook&Jeeves: Simulating Individuums...");
                    for (int i = 0; i < generation.Length; i++)
                    {
                        tmp = "(";
                        for (int j = 0; j < generation[0].get_optparas().Length; j++)
                        {
                            tmp = tmp + generation[i].get_optparas()[j] + " / ";
                        }
                        tmp = tmp.TrimEnd(' ', '/') + ")";


                        //Simulieren und zeichnen
                        if (generation[i].get_toSimulate())
                        {
                            if (this.settings.MetaEvo.Application == "testprobleme")
                            {
                                applog.appendText("Controller: Simulating Individuum " + generation[i].ID + " (" + Math.Round(((double)(i + 1) / (double)generation.Length), 2) * 100 + "%)...   " + algomanager.algos.algofeedbackarray[generation[i].get_generator()].name); // +": Optparas: " + tmp
                                testprobleme.Evaluierung_TestProbleme_MetaEvo(ref generation[i], 0, ref hauptdiagramm1);
                            }
                            if (this.settings.MetaEvo.Application == "sim")
                            {
                                applog.appendText("Controller: Simulating Individuum " + generation[i].ID + " (" + Math.Round(((double)(i + 1) / (double)generation.Length), 2) * 100 + "%)...   " + algomanager.algos.algofeedbackarray[generation[i].get_generator()].name); // +": Optparas: " + tmp
                                sim.Evaluate_MetaEvo(ref generation[i]);
                                hauptdiagramm1.ZeichneIndividuum(generation[i], 1, 1, 1, generation[i].ID % generation.Length, System.Drawing.Color.Yellow, true);
                                System.Windows.Forms.Application.DoEvents();
                            }
                        }
                        //else applog.appendText("Controller: NOT Simulating Individuum " + generation[i].ID);
                    }

                    //Neue Individuen mit Genpool verrechnen und Genpool zeichnen
                    algomanager.new_individuals_merge_with_genpool(ref generation);

                    //Abbruchbedingung für lokale Optimierung
                    if (algomanager.algos.algofeedbackarray[0].number_individuals_for_nextGen == 0) generationcounter = settings.MetaEvo.NumberGenerations + 1;
                }
                #endregion
            }
            progress1.iGen = progress1.NGen;
            applog.appendText("Controller: Calculation Finished");
            MessageBox.Show("Berechnung beendet", "MetaEvo");
            applog.savelog();
        }

        // Network Server
        private void start_network_server()
        {
            Client meServer = new Client(); 
            meServer = networkmanager.Network_Init_Client_Object(Dns.GetHostName());
            meServer.status = "init Genpool";
            int generationcounter = 0;

            while (generationcounter <= settings.MetaEvo.NumberGenerations)
            {
                //Einmaliges Initialisieren des Genpools
                if (meServer.status == "init Genpool")
                {
                    //Zufällige Parents setzen und in DB schreiben
                    set_random_parents(ref generation);

                    //Von den Clients ausrechnen lassen
                    applog.appendText("Controller: Calculate Genpool by Clients");
                    MessageBox.Show("Wait for Clients to register for calculation. Press ok to start","MetaEvo - Networkmanager");
                    if (networkmanager.calculate_by_clients(ref generation, ref hauptdiagramm1))
                    {
                        algomanager.set_genpool(ref generation);
                        generation = new EVO.Common.Individuum_MetaEvo[this.settings.MetaEvo.ChildsPerParent * this.settings.MetaEvo.PopulationSize];
                        meServer.set_AlsoInDB("generate Individuums", -1, -1);
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Initialisieren des Genpools", "MetaEvo - Controller");
                    }
                }

                //Individuen erzeugen
                else if (meServer.status == "generate Individuums")
                {
                    //Evolutionsschritte
                    algomanager.new_individuals_build(ref generation);

                    //Neuen Serverstatus setzen
                    meServer.set_AlsoInDB("waiting for client-calculation", -1, -1);
                }

                //Individuen berechnen lassen
                else if (meServer.status == "waiting for client-calculation")
                {
                    //Von den Clients ausrechnen lassen
                    if (networkmanager.calculate_by_clients(ref generation, ref hauptdiagramm1))
                    {
                        if (settings.MetaEvo.OpMode == "Local Optimizer")
                        {
                            //Abbruchbedingung für lokale Optimierung
                            if (algomanager.algos.algofeedbackarray[0].number_individuals_for_nextGen == 0) generationcounter = settings.MetaEvo.NumberGenerations + 1;
                        }
                        else
                        {
                            progress1.NextGen();
                            generationcounter++;
                        }
                        meServer.set_AlsoInDB("select Individuums", -1, -1);
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Ausführen der Kalkulation im Netzwerk");
                    }
                }

                //Individuen selektieren
                else if (meServer.status == "select Individuums")
                {
                    //Neue Individuen mit Genpool verrechnen und Genpool zeichnen
                    algomanager.new_individuals_merge_with_genpool(ref generation);

                    //Neuen Serverstatus setzen
                    meServer.set_AlsoInDB("generate Individuums", -1, -1);
                }
            }
            progress1.iGen = progress1.NGen;
            meServer.set_AlsoInDB("finished", -1, -1);
            applog.appendText("Controller: Calculation Finished");
            MessageBox.Show("Berechnung beendet", "MetaEvo");
        }

        // Network Client
        private void start_network_client()
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
                    Berechnungsstart = DateTime.Now;
                    networkmanager.Individuum_UpdateInDB(ref individuumForClient, "status", "calculate");

                    //Simulieren
                    applog.appendText("Controller: Individuum " + individuumForClient.ID + " simulating...");
                    if (this.settings.MetaEvo.Application == "testprobleme")
                    {
                        testprobleme.Evaluierung_TestProbleme_MetaEvo(ref individuumForClient, 0, ref hauptdiagramm1);
                    }
                    if (this.settings.MetaEvo.Application == "sim") sim.Evaluate_MetaEvo(ref individuumForClient);

                    //Individuum in DB Updaten
                    networkmanager.Individuum_UpdateInDB(ref individuumForClient, "status feat const", individuumForClient.get_status());
                    
                    //Neuen Speed-Daten berechnen
                    Berechnungsdauer = Math.Round((DateTime.Now.Subtract(Berechnungsstart)).TotalMilliseconds, 0);
                    meClient.speed_av += Math.Round((Berechnungsdauer - meClient.speed_av) / meClient.numberindividuums, 0);
                    if (Berechnungsdauer > meClient.speed_low) meClient.speed_low = Berechnungsdauer;
                    applog.appendText("Controller: Average Speed is set to " + meClient.speed_av + " Milliseconds, Lowest Speed is set to " + meClient.speed_low + " Milliseconds");

                    //Client ind DB Updaten
                    meClient.set_AlsoInDB("", meClient.speed_av, meClient.speed_low);
                }

                // Wenn kein Individuum mehr da ist, warten
                else
                {
                    applog.appendText("Controller: No Individuum found in DB (for this Client) - waiting...");
                    System.Threading.Thread.Sleep(3000);
                    //Prüfen ob Client-Entry noch besteht bzw. neu eintragen
                    networkmanager.DB_client_entry_update();
                }

                //Serverstatus neu lesen
                serverstatus = networkmanager.Network_ReadServer();
            }
            applog.appendText("Controller: Calculation Finished");
            MessageBox.Show("Berechnung beendet", "MetaEvo");
        }
    }
}
