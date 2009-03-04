using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Net;

namespace IHWB.EVO.MetaEvo
{
    public class Controller : IHWB.EVO.IController
    {
        //### Variablen ###
        EVO.Apps.Sim sim;
        EVO.Apps.Testprobleme testprobleme;

        EVO.Common.Constants.ApplicationTypes apptype;

        EVO.Common.Problem prob; 
        EVO.Common.EVO_Settings settings;
        EVO.Common.Individuum_MetaEvo individuumForClient;
        EVO.Common.Individuum_MetaEvo[] generation;
        EVO.Common.Progress progress1;

        EVO.Diagramm.Monitor monitor1;
        EVO.Diagramm.Hauptdiagramm hauptdiagramm1;

        Networkmanager networkmanager;
        Algomanager algomanager;

        int individuumnumber;
        string role;

        private string[,] result;

        private bool stopped;

        //Objekte für den Client (Ursache: Multithreading)
        Client meClient = new Client();
        DateTime Berechnungsstart;
        double[] clienttmp;

        /// <summary>
        /// Initialisiert den MetaEVO-Controller und übergibt alle erforderlichen Objekte
        /// </summary>
        /// <param name="prob_input">das Problem</param>
        /// <param name="settings_input">die Einstellungen</param>
        /// <param name="progress1_input">der Verlauf</param>
        /// <param name="monitor_input">der Monitor</param>
        /// <param name="hauptdiagramm_input">das Hauptdiagramm</param>
        public void Init(ref EVO.Common.Problem prob_input, ref EVO.Common.EVO_Settings settings_input, ref EVO.Common.Progress progress1_input, ref EVO.Diagramm.Monitor monitor_input, ref EVO.Diagramm.Hauptdiagramm hauptdiagramm_input)  
        {
            //Daten einlesen
            this.prob = prob_input;
            this.settings = settings_input;
            this.progress1 = progress1_input;
            this.monitor1 = monitor_input;
            this.hauptdiagramm1 = hauptdiagramm_input;

            //Result initialisieren
            this.result = new string[this.settings.MetaEvo.NumberGenerations + 2, 9];
        }

        /// <summary>
        /// Initialisiert den Controller für Sim-Anwendungen
        /// </summary>
        /// <param name="sim_input">Sim-Objekt</param>
        public void InitApp(ref IHWB.EVO.Apps.Sim sim_input)
        {
            this.apptype = IHWB.EVO.Common.Constants.ApplicationTypes.Sim;
            this.sim = sim_input;
        }

        /// <summary>
        /// Initialisiert den Controller für Testprobleme
        /// </summary>
        /// <param name="inputTestproblem">Testproblem-Objekt</param>
        public void InitApp(ref IHWB.EVO.Apps.Testprobleme testprobleme_input)
        {
            this.apptype = IHWB.EVO.Common.Constants.ApplicationTypes.Testprobleme;
            this.testprobleme = testprobleme_input;
        }

        /// <summary>
        /// Startet die Optimierung
        /// </summary>
        public void Start()
        {
            this.stopped = false;

            this.role = this.settings.MetaEvo.Role;

            //Setzen des Problems zum Design des Individuums
            EVO.Common.Individuum_MetaEvo.Initialise(ref this.prob);
            individuumnumber = 2;

            this.monitor1.Show();
            this.monitor1.SelectTabLog();
            this.monitor1.LogAppend("Controller: Task: " + prob.Datensatz);

            //Progress Initialisieren
            progress1.Initialize(1, 1, (short)settings.MetaEvo.NumberGenerations, (short)(settings.MetaEvo.ChildsPerParent * settings.MetaEvo.PopulationSize));

            switch (this.role)
            {
                case "Single PC":
                    this.monitor1.LogAppend("Controller: MetaEvo started in 'Single PC'-Mode");
                    //### Vorbereitung ###
                    //Initialisieren des Individuum-Arrays
                    if (settings.MetaEvo.OpMode == "Local Optimizer")
                    {
                        settings.MetaEvo.PopulationSize = settings.MetaEvo.NumberResults;
                        progress1.Initialize(1, 1, 0, (short)settings.MetaEvo.PopulationSize);
                    }

                    generation = new EVO.Common.Individuum_MetaEvo[this.settings.MetaEvo.PopulationSize];

                    for (int j = 0; j < this.settings.MetaEvo.PopulationSize; j++)
                    {
                        generation[j] = new EVO.Common.Individuum_MetaEvo("MetaEvo", individuumnumber, this.prob.List_OptParameter.Length);
                        individuumnumber++;
                    }

                    //Algomanager starten
                    algomanager = new Algomanager(ref prob, ref settings, individuumnumber, ref monitor1, ref this.result);

                    //### Hauptprogramm ###
                    start_single_pc();
                    break;

                case "Network Server":
                    this.monitor1.LogAppend("Controller: MetaEvo started in 'Network Server'-Mode");
                    //### Vorbereitung ###
                    //Initialisieren des Individuum-Arrays
                    generation = new EVO.Common.Individuum_MetaEvo[this.settings.MetaEvo.PopulationSize];
                    for (int j = 0; j < this.settings.MetaEvo.PopulationSize; j++)
                    {
                        generation[j] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuumnumber, this.prob.List_OptParameter.Length);
                        individuumnumber++;
                    }

                    //Algomanager starten
                    algomanager = new Algomanager(ref prob, ref settings, individuumnumber, ref monitor1, ref this.result);

                    //### Hauptprogramm ###
                    networkmanager = new Networkmanager(ref this.generation[0], ref this.settings, ref prob, ref monitor1);
                    //Info-Datenbank füllen
                    networkmanager.DB_set_info("Datensatz", "" + prob.Datensatz);
                    networkmanager.DB_set_info("Individuen im Genpool", "" + settings.MetaEvo.PopulationSize);
                    networkmanager.DB_set_info("Individuen / Generation", "" + settings.MetaEvo.ChildsPerParent * settings.MetaEvo.PopulationSize);
                    networkmanager.DB_set_info("Anzahl der Ergebnisse", "" + settings.MetaEvo.NumberResults);
                    networkmanager.DB_set_info("Generation", "Initialisierung");
                    start_network_server();
                    break;

                case "Network Client":
                    this.monitor1.LogAppend("Controller: MetaEvo started in 'Network Client'-Mode");
                    //### Vorbereitung ###
                    individuumForClient = new EVO.Common.Individuum_MetaEvo("MetaEvo", 0, this.prob.List_OptParameter.Length);
                    //Individuum-Speicher des Clients ausschalten
                    if (this.apptype == IHWB.EVO.Common.Constants.ApplicationTypes.Sim)
                    {
                        sim.StoreIndividuals = false; 
                    }

                    //### Hauptprogramm ###
                    networkmanager = new Networkmanager(ref this.individuumForClient, ref this.settings, ref prob, ref monitor1);

                    //Initialisierung einiger Variablen
                    meClient.numberindividuums = 0;
                    meClient = networkmanager.Network_Init_Client_Object(Dns.GetHostName());
                    clienttmp = new double[5];

                    start_network_client2();
                    break;
            }
        }

        public void Stoppen()
        {
            this.stopped = true;
            //TODO: hat bisher keine Auswirkung!
        }


        //### Methoden ###

        // Zufällige Eltern setzen
        private bool set_random_parents(ref EVO.Common.Individuum_MetaEvo[] generation_input)
        {
            double[] random;
            Random randomizer = new Random();
            this.monitor1.LogAppend("Controller: Construct random Genpool");

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
                generation_input[k].set_generator(-1);
            }
            return true;
        }



        //### Methoden ### Hauptprogramm

        // Single PC
        private bool start_single_pc()
        {
            Client mePC = new Client(); 
            mePC.status = "Init Genpool";
            settings.MetaEvo.CurrentGeneration = 1;

            while (mePC.status != "finished")
            {
                this.monitor1.LogAppend("Controller: Status: " + mePC.status);

                if (this.stopped) return false;

                #region Zustand: Init Genpool
                if (mePC.status == "Init Genpool")
                {
                    //Zufällige Parents setzen
                    set_random_parents(ref generation);

                    //Genpool simulieren
                    this.monitor1.LogAppend("Controller: Genpool: Simulating Individuums...");
                    evaluate_multi_4single(ref generation);

                    //Genpool speichern
                    algomanager.set_genpool(ref generation);

                    generation = new EVO.Common.Individuum_MetaEvo[this.settings.MetaEvo.ChildsPerParent * this.settings.MetaEvo.PopulationSize];

                    mePC.status = "generate Individuums";
                }
                #endregion

                #region Zustand: generate Individuums
                else if (mePC.status == "generate Individuums")
                {
                    this.monitor1.LogAppend("Controller: ### Building new Individuums for Generation " + settings.MetaEvo.CurrentGeneration + " ###");
                    algomanager.new_individuals_build(ref generation);
                    mePC.status = "simulate Individuums";
                }
                #endregion

                #region Zustand: simulate Individuums
                else if (mePC.status == "simulate Individuums")
                {
                    this.monitor1.LogAppend("Controller: Individuums for Generation " + settings.MetaEvo.CurrentGeneration + ": Simulating Individuums...");
                    progress1.iNachf = 0;
                    //Simulieren
                    
                    if (this.apptype == IHWB.EVO.Common.Constants.ApplicationTypes.Testprobleme)
                    {
                        for (int i = 0; i < generation.Length; i++)
                        {
                            if (generation[i].get_toSimulate())
                            {
                                this.monitor1.LogAppend("Controller: Simulating Individuum " + generation[i].ID + " (" + Math.Round(((double)(i + 1) / (double)generation.Length), 2) * 100 + "%)...   " + algomanager.algos.algofeedbackarray[generation[i].get_generator()].name);
                                evaluate_multi_4single(ref generation);
                                System.Windows.Forms.Application.DoEvents();

                                progress1.NextNachf();
                                if (this.stopped) return false;
                            }
                        }
                    }
                    else
                    {
                        int j = 0;
                        EVO.Common.Individuum_MetaEvo[] generation_tmp;

                        for (int i = 0; i < generation.Length; i++)
                        {
                            if (generation[i].get_toSimulate()) j++;
                        }
                        generation_tmp = new EVO.Common.Individuum_MetaEvo[j];
                        for (int i = 0; i < generation.Length; i++)
                        {
                            if (generation[i].get_toSimulate()) generation_tmp[j-1] = generation[i];
                            j--;
                        }

                        evaluate_multi_4single(ref generation_tmp);
                    }

                    mePC.status = "select Individuums";
                }
                #endregion

                #region Zustand: select Individuums
                else if (mePC.status == "select Individuums")
                {
                    algomanager.new_individuals_merge_with_genpool(ref generation);

                    //Sekpop zeichnen und ggf. speichern
                    this.draw_and_store_sekpop(ref algomanager.genpool);

                    settings.MetaEvo.CurrentGeneration++;

                    //Umschalt- und Abbruchbedingungen prüfen
                    //  Maximale Anzahl der Generationen erreicht oder Umschaltpunkt erreicht
                    if ((settings.MetaEvo.CurrentGeneration == settings.MetaEvo.NumberGenerations) || (settings.MetaEvo.AlgoMode == "Global: Finished"))
                    {
                        //Umschalten zur lokalen Optimierung
                        if (settings.MetaEvo.OpMode == "Both")
                        {
                            settings.MetaEvo.OpMode = "Local Optimizer";

                            algomanager.set_calculationmode_local(ref generation);
                            settings.MetaEvo.CurrentGeneration--;

                            progress1.Initialize(1, 1, (short)settings.MetaEvo.NumberGenerations, (short)(settings.MetaEvo.NumberResults));
                            progress1.iGen = (short)settings.MetaEvo.CurrentGeneration;
                            mePC.status = "generate Individuums";
                        }

                        //Reduzierung auf gewünschte Anzahl der Ergebnisse und Ausgabe
                        else if (settings.MetaEvo.OpMode == "Global Optimizer")
                        {
                            algomanager.set_calculationmode_global_finished(ref generation);
                            mePC.status = "finished";
                        }
                    }
                    //  Abbruchbedingung der lokalen Optimierung  
                    else if (settings.MetaEvo.AlgoMode == "Local: Finished")
                    {
                        mePC.status = "finished";
                    }

                    else
                    {
                        mePC.status = "generate Individuums";                         
                    }

                    if (settings.MetaEvo.OpMode != "Local Optimizer")
                    {
                        try
                        {
                            progress1.NextGen();
                        }
                        catch { }
                    }
                }
                #endregion     
            }

            //Zusatzergebnisse der lokalen Optimierung speichern
            if (settings.MetaEvo.OpMode == "Local Optimizer")
            {
                this.result[progress1.iGen + 1, 0] = algomanager.localausgabe;
                this.result[progress1.iGen + 1, algomanager.algos.algofeedbackarray.Length] = algomanager.localausgabe2;
            }
            progress1.iGen = progress1.NGen;
            this.monitor1.LogAppend("Controller: Calculation Finished");
            this.appendResultToLog();
            this.monitor1.savelog();
            return true;
        }

        // Network Server
        private bool start_network_server()
        {
            Client meServer = new Client(); 
            meServer = networkmanager.Network_Init_Client_Object(Dns.GetHostName());
            meServer.status = "Init Genpool";
            settings.MetaEvo.CurrentGeneration = 1;

            while (meServer.status != "finished")
            {
                this.monitor1.LogAppend("Controller: Status: " + meServer.status);

                if (this.stopped) return false;

                #region Zustand: Init Genpool
                if (meServer.status == "Init Genpool")
                {
                    //Zufällige Parents setzen und in DB schreiben
                    set_random_parents(ref generation);

                    //Von den Clients ausrechnen lassen
                    this.monitor1.LogAppend("Controller: Calculate Genpool by Clients");
                    MessageBox.Show("Wait for Clients to register for calculation. Press ok to start","MetaEvo - Networkmanager");
                    if (networkmanager.calculate_by_clients(ref generation, ref hauptdiagramm1, ref progress1, ref this.stopped))
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
                #endregion

                #region Zustand: generate Individuums
                else if (meServer.status == "generate Individuums")
                {
                    //Neue Individuen
                    this.monitor1.LogAppend("Controller: ### Building new Individuums for Generation " + settings.MetaEvo.CurrentGeneration + " ###");
                    algomanager.new_individuals_build(ref generation);

                    //Neuen Serverstatus setzen
                    meServer.set_AlsoInDB("waiting for client-calculation", -1, -1);
                }
                #endregion

                #region Zustand: waiting for client-calculation
                else if (meServer.status == "waiting for client-calculation")
                {
                    //Von den Clients ausrechnen lassen
                    if (networkmanager.calculate_by_clients(ref generation, ref hauptdiagramm1, ref progress1, ref this.stopped))
                    {
                        if (settings.MetaEvo.OpMode == "Local Optimizer")
                        {
                            networkmanager.DB_set_info("Generation", "--/" + settings.MetaEvo.NumberGenerations);
                        }
                        else
                        {
                            networkmanager.DB_set_info("Generation", settings.MetaEvo.CurrentGeneration + "/" + settings.MetaEvo.NumberGenerations);
                        }
                        meServer.set_AlsoInDB("select Individuums", -1, -1);
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Ausführen der Kalkulation im Netzwerk");
                    }
                }
                #endregion

                #region Zustand: select Individuums
                else if (meServer.status == "select Individuums")
                {
                    algomanager.new_individuals_merge_with_genpool(ref generation);

                    //Sekpop zeichnen und ggf. speichern
                    this.draw_and_store_sekpop(ref algomanager.genpool);

                    networkmanager.DB_set_info("Berechnungsmodus", "" + settings.MetaEvo.OpMode);
                    networkmanager.DB_set_info("Solutionvolume", "" + algomanager.solutionvolume.get_last_volume());
                    settings.MetaEvo.CurrentGeneration++;

                    //Umschalt- und Abbruchbedingungen prüfen
                    //  Maximale Anzahl der Generationen erreicht oder Umschaltpunkt erreicht
                    if ((settings.MetaEvo.CurrentGeneration == settings.MetaEvo.NumberGenerations) || (settings.MetaEvo.AlgoMode == "Global: Finished"))
                    {
                        //Umschalten zur lokalen Optimierung
                        if (settings.MetaEvo.OpMode == "Both")
                        {
                            settings.MetaEvo.OpMode = "Local Optimizer";

                            algomanager.set_calculationmode_local(ref generation);
                            settings.MetaEvo.CurrentGeneration--;

                            progress1.Initialize(1, 1, (short)settings.MetaEvo.NumberGenerations, (short)(settings.MetaEvo.NumberResults));
                            progress1.iGen = (short)settings.MetaEvo.CurrentGeneration;
                            meServer.set_AlsoInDB("generate Individuums", -1, -1);
                        }

                        //Reduzierung auf gewünschte Anzahl der Ergebnisse und Ausgabe
                        else if (settings.MetaEvo.OpMode == "Global Optimizer")
                        {
                            algomanager.set_calculationmode_global_finished(ref generation);
                            meServer.set_AlsoInDB("finished", -1, -1);
                        }
                    }
                    //  Abbruchbedingung der lokalen Optimierung  
                    else if (settings.MetaEvo.AlgoMode == "Local: Finished")
                    {
                        meServer.set_AlsoInDB("finished", -1, -1);
                    }


                    else
                    {
                        if (settings.MetaEvo.OpMode != "Local Optimizer")
                        {
                            try
                            {
                                progress1.NextGen();
                            }
                            catch { }
                        }
                        meServer.set_AlsoInDB("generate Individuums", -1, -1);
                    }
                }
                #endregion  
            }

            //Zusatzresulte der lokalen Optimierung speichern
            if (settings.MetaEvo.OpMode == "Local Optimizer")
            {
                this.result[progress1.iGen + 1, 0] = algomanager.localausgabe;
                this.result[progress1.iGen + 1, algomanager.algos.algofeedbackarray.Length] = algomanager.localausgabe2;
            }

            progress1.iGen = progress1.NGen;
            this.monitor1.LogAppend("Controller: Calculation Finished");
            this.appendResultToLog();
            this.monitor1.savelog();
            return true;
        }

        // Network Client
        private bool start_network_client()
        {
            Client meClient = new Client();
            meClient.numberindividuums = 0;
            meClient = networkmanager.Network_Init_Client_Object(Dns.GetHostName());
            string[] serverstatus = networkmanager.Network_ReadServer();
            DateTime Berechnungsstart;
            double Berechnungsdauer;
            
            //Solange der Server noch nicht fertig ist
            while (serverstatus[0] != "finished") {

                if (this.stopped) return true;

                networkmanager.Individuum_ReadFromDB_Client(ref individuumForClient);

                //Falls neues Individuum in DB existiert, berechnen
                if (individuumForClient.get_status() == "raw")
                {
                    Berechnungsstart = DateTime.Now;

                    //Status zuweisen
                    meClient.set_AlsoInDB("calculating", -1, -1);
                    meClient.numberindividuums++;

                    //Individuum in DB als "calculate" markieren
                    networkmanager.Individuum_UpdateInDB(ref individuumForClient, "status", "calculate");

                    //Simulieren
                    this.monitor1.LogAppend("Controller: Individuum " + individuumForClient.ID + " simulating...");
                    evaluate(ref individuumForClient, individuumForClient.ID);
                    System.Windows.Forms.Application.DoEvents();

                    //Individuum in DB Updaten
                    networkmanager.Individuum_UpdateInDB(ref individuumForClient, "status feat const", individuumForClient.get_status());
                    
                    //Neuen Speed-Daten berechnen
                    Berechnungsdauer = Math.Round((DateTime.Now.Subtract(Berechnungsstart)).TotalMilliseconds, 0);
                    if (meClient.numberindividuums == 1) meClient.speed_av = Berechnungsdauer;
                    else meClient.speed_av += Math.Round((Berechnungsdauer - meClient.speed_av) / 10, 0);
                    if (Berechnungsdauer > meClient.speed_low) meClient.speed_low = Berechnungsdauer;
                    this.monitor1.LogAppend("Controller: Average Speed is set to " + meClient.speed_av + " Milliseconds, Lowest Speed is set to " + meClient.speed_low + " Milliseconds");

                    //Client ind DB Updaten
                    meClient.set_AlsoInDB("", meClient.speed_av, meClient.speed_low);
                }

                // Wenn kein Individuum mehr da ist, warten
                else
                {
                    //Status zuweisen
                    meClient.set_AlsoInDB("ready", -1, -1);

                    this.monitor1.LogAppend("Controller: No Individuum found in DB (for this Client) - waiting...");
                    System.Threading.Thread.Sleep(3000);
                    //Prüfen ob Client-Entry noch besteht bzw. neu eintragen
                    networkmanager.DB_client_entry_update(ref prob);
                }

                //Serverstatus neu lesen
                serverstatus = networkmanager.Network_ReadServer();
            }
            progress1.iGen = progress1.NGen;
            this.monitor1.LogAppend("Controller: Calculation Finished");
            return true;
        }

        // Network Client2
        private bool start_network_client2()
        {
            IHWB.EVO.Common.Individuum[] generation_tmp;

            string[] serverstatus = networkmanager.Network_ReadServer();
            bool[] evaluate_success;
            
            //clienttmp[]: [0:Menge der zugeordneten Individuen, 1:Laufnummer in der Serie, 2: Berechnungsdauer

            //Solange der Server noch nicht fertig ist
            while (serverstatus[0] != "finished")
            {
                if (this.stopped) return true;
                clienttmp[0] = networkmanager.Individuums_CountMineInDB();
                clienttmp[1] = 0;

                //Falls neues Individuum in DB existiert, berechnen
                if (clienttmp[0] != 0)
                {
                    Berechnungsstart = DateTime.Now;

                    generation = new IHWB.EVO.Common.Individuum_MetaEvo[(int)clienttmp[0]];
                    generation_tmp = new IHWB.EVO.Common.Individuum[(int)clienttmp[0]];

                    //Zugeordnete Individuen aus der DB holen
                    for (int i = 0; i < (int)clienttmp[0]; i++)
                    {
                        generation[i] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", 0, this.prob.List_OptParameter.Length);
                        networkmanager.Individuum_ReadFromDB_Client(ref generation[i]);

                        //Individuum in DB als "calculate" markieren
                        networkmanager.Individuum_UpdateInDB(ref generation[i], "status", "calculate");
                        generation_tmp[i] = (EVO.Common.Individuum)generation[i];
                    }
                    this.monitor1.LogAppend("Controller: Read " + (int)clienttmp[0] + " Individuums from DB, starting Simulation...");

                    //Status zuweisen
                    meClient.set_AlsoInDB("calculating", -1, -1);

                    //für Event Registrieren und Simulieren
                    sim.IndividuumEvaluated += new IHWB.EVO.Apps.Sim.IndividuumEvaluatedEventHandler(this.evaluate_multi_4client_Event);
                    if (generation_tmp[0].OptParameter_RWerte[0] == 0) MessageBox.Show("Optparameter ungültig!"); 
                    evaluate_success = sim.Evaluate(ref generation_tmp, false);
                    sim.IndividuumEvaluated -= new IHWB.EVO.Apps.Sim.IndividuumEvaluatedEventHandler(this.evaluate_multi_4client_Event);

                    for (int i = 0; i < (int)clienttmp[0]; i++)
                    {
                        //ToDo: Was wird von sim.Evaluate für die verbliebenen Individuen zurückgegeben falls es abgebrochen wird?
                        if (!evaluate_success[i]) generation[i].set_status("false");
                        if (!generation[i].Is_Feasible) generation[i].set_status("false#constraints#");
                        else
                        {
                            generation[i].set_status("true");
                        }
                        networkmanager.Individuum_UpdateInDB(ref generation[i], "status", generation[i].get_status());
                    }
                }

                // Wenn kein Individuum mehr da ist, warten
                else
                {
                    //Status zuweisen
                    meClient.set_AlsoInDB("ready", -1, -1);

                    this.monitor1.LogAppend("Controller: No Individuum found in DB (for this Client) - waiting...");
                    System.Threading.Thread.Sleep(3000);
                    //Prüfen ob Client-Entry noch besteht bzw. neu eintragen
                    networkmanager.DB_client_entry_update(ref prob);
                }

                //Serverstatus neu lesen
                serverstatus = networkmanager.Network_ReadServer();
            }
            progress1.iGen = progress1.NGen;
            this.monitor1.LogAppend("Controller: Calculation Finished");
            return true;
        }

        private void evaluate_multi_4client_Event(ref EVO.Common.Individuum ind, int zahl)
        {
            this.monitor1.LogAppend("Controller: Individuum " + ind.ID + " simulated");
            //Im der DB updaten
            for (int i = 0; i < generation.Length; i++)
            {
                if (ind.ID == generation[i].ID)
                {
                    generation[i].set_status("finished");
                    networkmanager.Individuum_UpdateInDB(ref generation[i], "status feat const", generation[i].get_status());
                    break;
                }
            }
            System.Windows.Forms.Application.DoEvents();
            clienttmp[1]++;

            if ((networkmanager.Individuums_CountMineInDB() != clienttmp[0]) || (this.stopped))
            {
                sim.isStopped = true;
                
                //Neuen Speed-Daten berechnen
                clienttmp[2] = Math.Round((DateTime.Now.Subtract(Berechnungsstart)).TotalMilliseconds, 0);
                if (clienttmp[1] == 1) meClient.speed_av = clienttmp[2];
                //Je mehr Individuen eine Serie enthält, desto ausschlaggebender ist ihre Berechnungsgeschwindigkeit (1/20 bis 1/10) 
                else meClient.speed_av += Math.Round((clienttmp[2] / clienttmp[1] - meClient.speed_av) / Math.Max(20 - clienttmp[1], 10), 0);
                if (clienttmp[2] > meClient.speed_low) meClient.speed_low = clienttmp[2];
                this.monitor1.LogAppend("Controller: Average Speed is set to " + meClient.speed_av + " Milliseconds, Lowest Speed is set to " + meClient.speed_low + " Milliseconds");

                //Clientdaten in DB Updaten
                meClient.set_AlsoInDB("", meClient.speed_av, meClient.speed_low);
            }
        }

        /// <summary>
        /// Evaluiert ein Meta-Individuum
        /// </summary>
        /// <param name="ind_meta">das Individuum</param>
        /// <param name="iNachf">Nachfahrennummer</param>
        private void evaluate(ref EVO.Common.Individuum_MetaEvo ind_meta, int iNachf)
        {
            bool storeInDB;
            EVO.Common.Individuum ind;
            ind = ind_meta; //referenz

            //in ErgebnisDB speichern?
            storeInDB = (this.role == "Single PC") ? true : false;

            //Testproblem
            if (this.apptype == IHWB.EVO.Common.Constants.ApplicationTypes.Testprobleme)
            {
                //Evaluieren und zeichnen
                this.testprobleme.Evaluate(ref ind, 0, ref this.hauptdiagramm1);
            }

            //Simulation
            else if (this.apptype == IHWB.EVO.Common.Constants.ApplicationTypes.Sim)
            {
                ind_meta.set_toSimulate(false);

                //simulieren
                if (!this.sim.Evaluate(ref ind, storeInDB))
                {
                    ind_meta.set_status("false");
                }
                else
                {
                    //Zeichnen
                    //TODO: Generationsnummer übergeben
                    this.hauptdiagramm1.ZeichneIndividuum(ind, 1, 1, 1, iNachf, System.Drawing.Color.Orange, false);
                }
            }

            //post-processing
            if (!ind.Is_Feasible)
            {
                ind_meta.set_status("false#constraints#"); 
            }
            else
            {
                ind_meta.set_status("true");
            }

        }

        /// <summary>
        /// Evaluiert Meta-Individuen mit Multithreadding (nicht bei Testproblemen)
        /// </summary>
        /// <param name="ind_meta">das Individuum</param>
        /// <param name="iNachf">Nachfahrennummer</param>
        private void evaluate_multi_4single(ref EVO.Common.Individuum_MetaEvo[] ind_meta)
        {
            bool[] evaluate_success;
            EVO.Common.Individuum[] ind = new EVO.Common.Individuum[ind_meta.Length];

            //Funktion für Event registrieren
            sim.IndividuumEvaluated += new IHWB.EVO.Apps.Sim.IndividuumEvaluatedEventHandler(this.evaluate_multi_4single_Event);


            for (int i = 0; i < ind_meta.Length; i++ )
            {
                ind_meta[i].set_toSimulate(false);
                //Als Referenz gesetzt
                ind[i] = (EVO.Common.Individuum)ind_meta[i];
            }

            //Simulation mit Events starten
            evaluate_success = sim.Evaluate(ref ind, true);

            for (int i = 0; i < ind_meta.Length; i++ )
            {
                if (!evaluate_success[i]) ind_meta[i].set_status("false");
                if (!ind_meta[i].Is_Feasible) ind_meta[i].set_status("false#constraints#");
                else
                {
                    ind_meta[i].set_status("true");   
                }
            }

            //Regsitrierung der Funktion für das Event zurücknehmen
            sim.IndividuumEvaluated -= new IHWB.EVO.Apps.Sim.IndividuumEvaluatedEventHandler(this.evaluate_multi_4single_Event);
        }

        /// <summary>
        /// Vom Event getriggerte Methode zur Weiterverarbeitung bei laufendem Multithreadding
        /// </summary>
        /// <param name="ind">ein Individuum</param>
        /// <param name="zahl">Nummer des Individuums in der Generation</param>
        /// <remarks>Es werden nur Individuen mit Status "true" verwendet!</remarks>
        public void evaluate_multi_4single_Event(ref EVO.Common.Individuum ind, int zahl)
        {
            this.monitor1.LogAppend("Controller: Simulating Individuum " + ind.ID + "   (" + Math.Round(((double)(progress1.iNachf + 1) / (double)generation.Length), 2) * 100 + "%)...");
            System.Windows.Forms.Application.DoEvents();

            progress1.NextNachf();

            if (this.stopped)
            {
                sim.isStopped = true;
            }

            this.hauptdiagramm1.ZeichneIndividuum(ind, 1, 1, 1, this.individuumnumber % generation.Length, System.Drawing.Color.Orange, false);
        }

        /// <summary>
        /// Zeichnet (und speichert ggf. im OptResult) die Sekundäre Population (aktuelle Front)
        /// </summary>
        /// <param name="genpool">der Genpool</param>
        /// <remarks>Es werden nur Individuen mit Status "true" verwendet!</remarks>
        private void draw_and_store_sekpop(ref EVO.Common.Individuum_MetaEvo[] genpool)
        {
            int i, j;
            EVO.Common.Individuum[] sekpop;

            sekpop = new IHWB.EVO.Common.Individuum[0];

            //alle nicht-dominierten Lösungen herausfiltern
            j = 0;
            for (i = 0; i < genpool.Length; i++)
            {
                if (genpool[i].get_status() == "true")
                {
                    Array.Resize<EVO.Common.Individuum>(ref sekpop, j + 1);
                    sekpop[j] = genpool[i].Clone();
                    j++;
                }
            }

            //SekPop in DB speichern (nur bei Sim und "Single PC")
            if (this.apptype == IHWB.EVO.Common.Constants.ApplicationTypes.Sim
                && this.role == "Single PC")
            {
                this.sim.OptResult.setSekPop(sekpop, settings.MetaEvo.CurrentGeneration - 1);
            }

            //SekPop zeichnen
            this.hauptdiagramm1.ZeichneSekPopulation(sekpop);
            System.Windows.Forms.Application.DoEvents();
        }

        /// <summary>
        /// Hängt das Result an den Log an
        /// </summary>
        private void appendResultToLog()
        {
            string tmp;
            int i, j;

            tmp = "";
            for (i = 0; i <= 8; i++)
            {
                for (j = 0; j <= this.result.GetUpperBound(0); j++)
                {
                    tmp = tmp + this.result[j, i] + "\t";
                }
                tmp = tmp + EVO.Common.Constants.eol;
            }
            //Result an Log anhängen
            this.monitor1.LogAppend("Result:" + EVO.Common.Constants.eol + tmp);
        }
    }
}
