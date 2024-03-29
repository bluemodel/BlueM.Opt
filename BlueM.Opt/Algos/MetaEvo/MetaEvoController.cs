/*
BlueM.Opt
Copyright (C) BlueM Dev Group
Website: <https://www.bluemodel.org>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see <https://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Net;

namespace BlueM.Opt.Algos.MetaEvo
{
    public class MetaEvoController : BlueM.Opt.Algos.IController
    {
        /// <summary>
        /// Multithreading Support
        /// </summary>
        public bool MultithreadingSupported
        {
            get {return true;}
        }

        //### Variablen ###
        BlueM.Opt.Apps.Sim sim;
        BlueM.Opt.Apps.Testprobleme testprobleme;

        BlueM.Opt.Common.Constants.ApplicationTypes apptype;

        BlueM.Opt.Common.Problem prob; 
        BlueM.Opt.Common.Settings settings;
        BlueM.Opt.Common.Individuum_MetaEvo individuumForClient;
        BlueM.Opt.Common.Individuum_MetaEvo[] generation;
        BlueM.Opt.Common.Progress progress1;

        BlueM.Opt.Diagramm.Hauptdiagramm hauptdiagramm1;

        Networkmanager networkmanager;
        Algomanager algomanager;

        int individuumnumber;
        string role;

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
        /// <param name="hauptdiagramm_input">das Hauptdiagramm</param>
        public void Init(ref BlueM.Opt.Common.Problem prob_input, ref BlueM.Opt.Common.Settings settings_input, ref BlueM.Opt.Common.Progress progress1_input, ref BlueM.Opt.Diagramm.Hauptdiagramm hauptdiagramm_input)  
        {
            //Daten einlesen
            this.prob = prob_input;
            this.settings = settings_input;
            this.progress1 = progress1_input;
            this.hauptdiagramm1 = hauptdiagramm_input;
        }

        /// <summary>
        /// Initialisiert den Controller für Sim-Anwendungen
        /// </summary>
        /// <param name="sim_input">Sim-Objekt</param>
        public void InitApp(ref BlueM.Opt.Apps.Sim sim_input)
        {
            this.apptype = BlueM.Opt.Common.Constants.ApplicationTypes.Sim;
            this.sim = sim_input;
        }

        /// <summary>
        /// Initialisiert den Controller für Testprobleme
        /// </summary>
        /// <param name="inputTestproblem">Testproblem-Objekt</param>
        public void InitApp(ref BlueM.Opt.Apps.Testprobleme testprobleme_input)
        {
            this.apptype = BlueM.Opt.Common.Constants.ApplicationTypes.Testproblems;
            this.testprobleme = testprobleme_input;
        }

        /// <summary>
        /// Startet die Optimierung
        /// </summary>
        public void Start()
        {
            this.stopped = false;

            this.role = this.settings.MetaEvo.Role;

            individuumnumber = 2;

            Common.Log.AddMessage(Common.Log.levels.info, "Controller: Task: " + prob.Datensatz);

            //Progress Initialisieren
            progress1.Initialize(1, 1, (short)settings.MetaEvo.NumberGenerations, (short)(settings.MetaEvo.ChildrenPerParent * settings.MetaEvo.PopulationSize));

            switch (this.role)
            {
                case "Single PC":
                    Common.Log.AddMessage(Common.Log.levels.info, "Controller: MetaEvo started in 'Single PC'-Mode");
                    //### Vorbereitung ###
                    //Initialisieren des Individuum-Arrays
                    if (settings.MetaEvo.OpMode == "Local Optimizer")
                    {
                        settings.MetaEvo.PopulationSize = settings.MetaEvo.NumberResults;
                        progress1.Initialize(1, 1, 0, (short)settings.MetaEvo.PopulationSize);
                    }

                    generation = new BlueM.Opt.Common.Individuum_MetaEvo[this.settings.MetaEvo.PopulationSize];

                    for (int j = 0; j < this.settings.MetaEvo.PopulationSize; j++)
                    {
                        generation[j] = new BlueM.Opt.Common.Individuum_MetaEvo("MetaEvo", individuumnumber, this.prob.List_OptParameter.Length);
                        individuumnumber++;
                    }

                    //Algomanager starten
                    algomanager = new Algomanager(ref prob, ref settings, individuumnumber);

                    //### Hauptprogramm ###
                    if (settings.General.UseMultithreading) start_single_pc_multithreading();
                    else start_single_pc();
                    break;

                case "Network Server":
                    Common.Log.AddMessage(Common.Log.levels.info, "Controller: MetaEvo started in 'Network Server'-Mode");
                    //### Vorbereitung ###
                    //Initialisieren des Individuum-Arrays
                    generation = new BlueM.Opt.Common.Individuum_MetaEvo[this.settings.MetaEvo.PopulationSize];
                    for (int j = 0; j < this.settings.MetaEvo.PopulationSize; j++)
                    {
                        generation[j] = new BlueM.Opt.Common.Individuum_MetaEvo("MetaEvo", individuumnumber, this.prob.List_OptParameter.Length);
                        individuumnumber++;
                    }

                    //Algomanager starten
                    algomanager = new Algomanager(ref prob, ref settings, individuumnumber);

                    //### Hauptprogramm ###
                    networkmanager = new Networkmanager(ref this.generation[0], ref this.settings, ref prob);
                    //Info-Datenbank füllen
                    networkmanager.DB_set_info("Datensatz", "" + prob.Datensatz);
                    networkmanager.DB_set_info("Individuen im Genpool", "" + settings.MetaEvo.PopulationSize);
                    networkmanager.DB_set_info("Individuen / Generation", "" + settings.MetaEvo.ChildrenPerParent * settings.MetaEvo.PopulationSize);
                    networkmanager.DB_set_info("Anzahl der Ergebnisse", "" + settings.MetaEvo.NumberResults);
                    networkmanager.DB_set_info("Generation", "Initialisierung");
                    start_network_server();
                    break;

                case "Network Client":
                    Common.Log.AddMessage(Common.Log.levels.info, "Controller: MetaEvo started in 'Network Client'-Mode");
                    //### Vorbereitung ###
                    individuumForClient = new BlueM.Opt.Common.Individuum_MetaEvo("MetaEvo", 0, this.prob.List_OptParameter.Length);
                    //Individuum-Speicher des Clients ausschalten
                    if (this.apptype == BlueM.Opt.Common.Constants.ApplicationTypes.Sim)
                    {
                        sim.StoreIndividuals = false;
                    }

                    //### Hauptprogramm ###
                    networkmanager = new Networkmanager(ref this.individuumForClient, ref this.settings, ref prob);

                    //Initialisierung einiger Variablen
                    meClient.numberindividuums = 0;
                    meClient = networkmanager.Network_Init_Client_Object(Dns.GetHostName());
                    clienttmp = new double[5];

                    if (settings.General.UseMultithreading) start_network_client_multithreading();
                    else start_network_client();
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
        private bool set_random_parents(ref BlueM.Opt.Common.Individuum_MetaEvo[] generation_input)
        {
            double[] random;
            Random randomizer = new Random();
            Common.Log.AddMessage(Common.Log.levels.info, "Controller: Construct random Genpool");

            //Für jedes Individuum durchgehen
            for (int k = 0; k < this.settings.MetaEvo.PopulationSize; k++)
            {
                random = new double[this.prob.NumOptParams];
                //Für jeden Parameter durchgehen
                for (int j = 0; j < this.prob.NumOptParams; j++)
                {
                    double max = this.prob.List_OptParameter[j].Max;
                    double min = this.prob.List_OptParameter[j].Min;
                    random[j] = min + (max - min) * ((double)randomizer.Next(0, 1000) / 1000);
                }
                generation_input[k].set_optparas(random);
                generation_input[k].set_status("raw");
                generation_input[k].set_generator(-1);
            }
            return true;
        }



        //### Methoden ### Hauptprogramm
        #region Single PC ohne Multithreading
        private bool start_single_pc()
        {
            Client mePC = new Client();
            mePC.status = "Init Genpool";
            settings.MetaEvo.CurrentGeneration = 1;

            while (mePC.status != "finished")
            {
                Common.Log.AddMessage(Common.Log.levels.info, "Controller: Status: " + mePC.status);

                if (this.stopped) return false;

                #region Zustand: Init Genpool
                if (mePC.status == "Init Genpool")
                {
                    //Zufällige Parents setzen
                    set_random_parents(ref generation);

                    //Genpool simulieren
                    Common.Log.AddMessage(Common.Log.levels.info, "Controller: Genpool: Simulating Individuums...");
                    for (int i = 0; i < generation.Length; i++)
                    {
                        evaluate(ref generation[i], i);
                        Common.Log.AddMessage(Common.Log.levels.info, "Controller: Individuum " + generation[i].ID + " (" + Math.Round(((double)(i + 1) / (double)generation.Length), 2) * 100 + "%)");
                        System.Windows.Forms.Application.DoEvents();
                    }

                    //Genpool speichern
                    algomanager.set_genpool(ref generation);

                    generation = new BlueM.Opt.Common.Individuum_MetaEvo[this.settings.MetaEvo.ChildrenPerParent * this.settings.MetaEvo.PopulationSize];

                    mePC.status = "generate Individuums";
                }
                #endregion

                #region Zustand: generate Individuums
                else if (mePC.status == "generate Individuums")
                {
                    Common.Log.AddMessage(Common.Log.levels.info, "Controller: ### Building new Individuums for Generation " + settings.MetaEvo.CurrentGeneration + " ###");
                    algomanager.new_individuals_build(ref generation);
                    mePC.status = "simulate Individuums";
                }
                #endregion

                #region Zustand: simulate Individuums
                else if (mePC.status == "simulate Individuums")
                {
                    Common.Log.AddMessage(Common.Log.levels.info, "Controller: Individuums for Generation " + settings.MetaEvo.CurrentGeneration + ": Simulating Individuums...");
                    progress1.iNachf = 0;
                    for (int i = 0; i < generation.Length; i++)
                    {
                        //Simulieren und zeichnen
                        if (generation[i].get_toSimulate())
                        {
                            Common.Log.AddMessage(Common.Log.levels.info, "Controller: Simulating Individuum " + generation[i].ID + " (" + Math.Round(((double)(i + 1) / (double)generation.Length), 2) * 100 + "%)...   " + algomanager.algos.algofeedbackarray[generation[i].get_generator()].name);
                            evaluate(ref generation[i], i);
                            System.Windows.Forms.Application.DoEvents();

                            progress1.NextNachf();
                            if (this.stopped) return false;
                        }
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
                            //settings.MetaEvo.CurrentGeneration--;

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

            progress1.iGen = progress1.NGen;
            Common.Log.AddMessage(Common.Log.levels.info, "Controller: Calculation Finished");
            return true;
        }
        #endregion

        #region Single PC mit Multithreading
        private bool start_single_pc_multithreading()
        {
            Client mePC = new Client();
            mePC.status = "Init Genpool";
            settings.MetaEvo.CurrentGeneration = 1;

            while (mePC.status != "finished")
            {
                Common.Log.AddMessage(Common.Log.levels.info, "Controller: Status: " + mePC.status);

                if (this.stopped) return false;

                #region Zustand: Init Genpool
                if (mePC.status == "Init Genpool")
                {
                    //Zufällige Parents setzen
                    set_random_parents(ref generation);

                    //Genpool simulieren
                    Common.Log.AddMessage(Common.Log.levels.info, "Controller: Genpool: Simulating Individuums...");
                    evaluate_multi_4single(ref generation);

                    //Genpool speichern
                    algomanager.set_genpool(ref generation);

                    generation = new BlueM.Opt.Common.Individuum_MetaEvo[this.settings.MetaEvo.ChildrenPerParent * this.settings.MetaEvo.PopulationSize];

                    mePC.status = "generate Individuums";
                }
                #endregion

                #region Zustand: generate Individuums
                else if (mePC.status == "generate Individuums")
                {
                    Common.Log.AddMessage(Common.Log.levels.info, "Controller: ### Building new Individuums for Generation " + settings.MetaEvo.CurrentGeneration + " ###");
                    algomanager.new_individuals_build(ref generation);
                    mePC.status = "simulate Individuums";
                }
                #endregion

                #region Zustand: simulate Individuums
                else if (mePC.status == "simulate Individuums")
                {
                    Common.Log.AddMessage(Common.Log.levels.info, "Controller: Individuums for Generation " + settings.MetaEvo.CurrentGeneration + ": Simulating Individuums...");
                    progress1.iNachf = 0;
                    //Simulieren

                    if (this.apptype == BlueM.Opt.Common.Constants.ApplicationTypes.Testproblems)
                    {
                        for (int i = 0; i < generation.Length; i++)
                        {
                            if (generation[i].get_toSimulate())
                            {
                                Common.Log.AddMessage(Common.Log.levels.info, "Controller: Simulating Individuum " + generation[i].ID + " (" + Math.Round(((double)(i + 1) / (double)generation.Length), 2) * 100 + "%)...   " + algomanager.algos.algofeedbackarray[generation[i].get_generator()].name);
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
                        BlueM.Opt.Common.Individuum_MetaEvo[] generation_tmp;

                        for (int i = 0; i < generation.Length; i++)
                        {
                            if (generation[i].get_toSimulate()) j++;
                        }
                        generation_tmp = new BlueM.Opt.Common.Individuum_MetaEvo[j];
                        for (int i = 0; i < generation.Length; i++)
                        {
                            if (generation[i].get_toSimulate())
                            {
                                generation_tmp[j - 1] = generation[i];
                                j--;
                            }
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
                            //settings.MetaEvo.CurrentGeneration--;

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

            progress1.iGen = progress1.NGen;
            Common.Log.AddMessage(Common.Log.levels.info, "Controller: Calculation Finished");
            return true;
        }

        /// <summary>
        /// Evaluiert Meta-Individuen mit Multithreadding (nicht bei Testproblemen)
        /// </summary>
        /// <param name="ind_meta">das Individuum</param>
        /// <param name="iNachf">Nachfahrennummer</param>
        private void evaluate_multi_4single(ref BlueM.Opt.Common.Individuum_MetaEvo[] ind_meta)
        {
            bool[] evaluate_success;
            BlueM.Opt.Common.Individuum[] ind = new BlueM.Opt.Common.Individuum[ind_meta.Length];

            //Funktion für Event registrieren
            sim.IndividuumEvaluated += new BlueM.Opt.Apps.Sim.IndividuumEvaluatedEventHandler(this.evaluate_multi_4single_Event);


            for (int i = 0; i < ind_meta.Length; i++)
            {
                ind_meta[i].set_toSimulate(false);
                //Als Referenz gesetzt
                ind[i] = (BlueM.Opt.Common.Individuum)ind_meta[i];
            }

            //Simulation mit Events starten
            evaluate_success = sim.Evaluate(ref ind, true);

            for (int i = 0; i < ind_meta.Length; i++)
            {
                if (!evaluate_success[i]) ind_meta[i].set_status("false");
                if (!ind_meta[i].Is_Feasible) ind_meta[i].set_status("false#constraints#");
                else
                {
                    ind_meta[i].set_status("true");
                }
            }

            //Regsitrierung der Funktion für das Event zurücknehmen
            sim.IndividuumEvaluated -= new BlueM.Opt.Apps.Sim.IndividuumEvaluatedEventHandler(this.evaluate_multi_4single_Event);
        }

        /// <summary>
        /// Vom Event getriggerte Methode zur Weiterverarbeitung bei laufendem Multithreadding
        /// </summary>
        /// <param name="ind">ein Individuum</param>
        /// <param name="zahl">Nummer des Individuums in der Generation</param>
        /// <remarks>Es werden nur Individuen mit Status "true" verwendet!</remarks>
        public void evaluate_multi_4single_Event(ref BlueM.Opt.Common.Individuum ind, int zahl)
        {
            Common.Log.AddMessage(Common.Log.levels.info, "Controller: Simulating Individuum " + ind.ID + "   (" + Math.Round(((double)(progress1.iNachf + 1) / (double)generation.Length), 2) * 100 + "%)...");
            System.Windows.Forms.Application.DoEvents();

            progress1.NextNachf();

            if (this.stopped)
            {
                sim.isStopped = true;
            }

            this.hauptdiagramm1.ZeichneIndividuum(ind, 1, 1, 1, this.individuumnumber % generation.Length, System.Drawing.Color.Orange, false);
        }
        #endregion;

        #region Network Server
        private bool start_network_server()
        {
            Client meServer = new Client();
            meServer = networkmanager.Network_Init_Client_Object(Dns.GetHostName());
            meServer.status = "Init Genpool";
            settings.MetaEvo.CurrentGeneration = 1;

            while (meServer.status != "finished")
            {
                Common.Log.AddMessage(Common.Log.levels.info, "Controller: Status: " + meServer.status);

                if (this.stopped) return false;

                #region Zustand: Init Genpool
                if (meServer.status == "Init Genpool")
                {
                    //Zufällige Parents setzen und in DB schreiben
                    set_random_parents(ref generation);

                    //Von den Clients ausrechnen lassen
                    Common.Log.AddMessage(Common.Log.levels.info, "Controller: Calculate Genpool by Clients");
                    MessageBox.Show("Wait for Clients to register for calculation. Press ok to start", "MetaEvo - Networkmanager");
                    if (networkmanager.calculate_by_clients(ref generation, ref hauptdiagramm1, ref progress1, ref this.stopped))
                    {
                        algomanager.set_genpool(ref generation);

                        generation = new BlueM.Opt.Common.Individuum_MetaEvo[this.settings.MetaEvo.ChildrenPerParent * this.settings.MetaEvo.PopulationSize];
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
                    Common.Log.AddMessage(Common.Log.levels.info, "Controller: ### Building new Individuums for Generation " + settings.MetaEvo.CurrentGeneration + " ###");
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
                    networkmanager.DB_set_info("Solutionvolume", "" + algomanager.solutionvolume.get_last_infos());
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
                            //settings.MetaEvo.CurrentGeneration--;

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

            progress1.iGen = progress1.NGen;
            Common.Log.AddMessage(Common.Log.levels.info, "Controller: Calculation Finished");
            return true;
        }
        #endregion;

        #region Network Client ohne Multithreading
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
                    Common.Log.AddMessage(Common.Log.levels.info, "Controller: Individuum " + individuumForClient.ID + " simulating...");
                    evaluate(ref individuumForClient, individuumForClient.ID);
                    System.Windows.Forms.Application.DoEvents();

                    //Individuum in DB Updaten
                    networkmanager.Individuum_UpdateInDB(ref individuumForClient, "status feat const", individuumForClient.get_status());
                    
                    //Neuen Speed-Daten berechnen
                    Berechnungsdauer = Math.Round((DateTime.Now.Subtract(Berechnungsstart)).TotalMilliseconds, 0);
                    if (meClient.numberindividuums == 1) meClient.speed_av = Berechnungsdauer;
                    else meClient.speed_av += Math.Round((Berechnungsdauer - meClient.speed_av) / 10, 0);
                    if (Berechnungsdauer > meClient.speed_low) meClient.speed_low = Berechnungsdauer;
                    Common.Log.AddMessage(Common.Log.levels.info, "Controller: Average Speed is set to " + meClient.speed_av + " Milliseconds, Lowest Speed is set to " + meClient.speed_low + " Milliseconds");

                    //Client ind DB Updaten
                    meClient.set_AlsoInDB("", meClient.speed_av, meClient.speed_low);
                }

                // Wenn kein Individuum mehr da ist, warten
                else
                {
                    //Status zuweisen
                    meClient.set_AlsoInDB("ready", -1, -1);

                    Common.Log.AddMessage(Common.Log.levels.info, "Controller: No Individuum found in DB (for this Client) - waiting...");
                    System.Threading.Thread.Sleep(3000);
                    //Prüfen ob Client-Entry noch besteht bzw. neu eintragen
                    networkmanager.DB_client_entry_update(ref prob);
                }

                //Serverstatus neu lesen
                serverstatus = networkmanager.Network_ReadServer();
            }
            progress1.iGen = progress1.NGen;
            Common.Log.AddMessage(Common.Log.levels.info, "Controller: Calculation Finished");
            return true;
        }
        #endregion

        #region Network Client mit Multithreading
        private bool start_network_client_multithreading()
        {
            BlueM.Opt.Common.Individuum[] generation_tmp;

            string[] serverstatus = networkmanager.Network_ReadServer();
            bool[] evaluate_success;
            
            //clienttmp[]: [0:Menge der zugeordneten Individuen, 1:Laufnummer in der Serie, 2: Berechnungsdauer

            //Solange der Server noch nicht fertig ist
            while (serverstatus[0] != "finished")
            {
                if (this.stopped) return true;
                clienttmp[0] = networkmanager.Individuums_CountMineRawInDB();
                clienttmp[1] = 0;

                //Falls neues Individuum in DB existiert, berechnen
                if (clienttmp[0] != 0)
                {
                    Berechnungsstart = DateTime.Now;

                    generation = new BlueM.Opt.Common.Individuum_MetaEvo[(int)clienttmp[0]];
                    generation_tmp = new BlueM.Opt.Common.Individuum[(int)clienttmp[0]];

                    //Zugeordnete Individuen aus der DB holen
                    for (int i = 0; i < (int)clienttmp[0]; i++)
                    {
                        generation[i] = new BlueM.Opt.Common.Individuum_MetaEvo("MetaEvo", 0, this.prob.List_OptParameter.Length);
                        networkmanager.Individuum_ReadFromDB_Client(ref generation[i]);

                        //Individuum in DB als "calculate" markieren
                        networkmanager.Individuum_UpdateInDB(ref generation[i], "status", "calculate");
                        generation_tmp[i] = (BlueM.Opt.Common.Individuum)generation[i];
                    }
                    Common.Log.AddMessage(Common.Log.levels.info, "Controller: Read " + (int)clienttmp[0] + " Individuums from DB, starting Simulation...");

                    //Status zuweisen
                    meClient.set_AlsoInDB("calculating", -1, -1);

                    //für Event Registrieren und Simulieren
                    //('evaluate_multi_4client_Event' wird ausgeführt, sobald das Event 'sim.IndividuumEvaluated' auslöst
                    sim.IndividuumEvaluated += new BlueM.Opt.Apps.Sim.IndividuumEvaluatedEventHandler(this.evaluate_multi_4client_Event);
                    evaluate_success = sim.Evaluate(ref generation_tmp, false);
                    sim.IndividuumEvaluated -= new BlueM.Opt.Apps.Sim.IndividuumEvaluatedEventHandler(this.evaluate_multi_4client_Event);

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

                    Common.Log.AddMessage(Common.Log.levels.info, "Controller: No Individuum found in DB (for this Client) - waiting...");
                    System.Threading.Thread.Sleep(3000);
                    //Prüfen ob Client-Entry noch besteht bzw. neu eintragen
                    networkmanager.DB_client_entry_update(ref prob);
                }

                //Serverstatus neu lesen
                serverstatus = networkmanager.Network_ReadServer();
            }
            progress1.iGen = progress1.NGen;
            Common.Log.AddMessage(Common.Log.levels.info, "Controller: Calculation Finished");
            return true;
        }

        private void evaluate_multi_4client_Event(ref BlueM.Opt.Common.Individuum ind, int zahl)
        {
            Common.Log.AddMessage(Common.Log.levels.info, "Controller: Individuum " + ind.ID + " simulated");
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
            clienttmp[1]++;

            //Multithreading beenden
            if ((networkmanager.Individuums_CountMineRawInDB() != clienttmp[0]) || (this.stopped))
            {
                sim.isStopped = true;
                
                //Clientdaten in DB Updaten
                meClient.set_AlsoInDB("", meClient.speed_av, meClient.speed_low);
            }

            //Berechnung der Client-Geschwindigkeit für im Idealfall 5 Individuen
            if ((clienttmp[1] == 5) || (networkmanager.Individuums_CountMineRawInDB() != clienttmp[0]) || (this.stopped))
            {
                clienttmp[2] = Math.Round((DateTime.Now.Subtract(Berechnungsstart)).TotalMilliseconds, 0);
                meClient.speed_av += Math.Round(((clienttmp[2] / clienttmp[1]) - meClient.speed_av) / (5 + Math.Abs(5 - clienttmp[1])), 0);
                if (clienttmp[2] > meClient.speed_low) meClient.speed_low = clienttmp[2];
                Common.Log.AddMessage(Common.Log.levels.info, "Controller: Average Speed is set to " + meClient.speed_av + " Milliseconds, Lowest Speed is set to " + meClient.speed_low + " Milliseconds");
            }
        }
        #endregion;

        /// <summary>
        /// Evaluiert ein Meta-Individuum
        /// </summary>
        /// <param name="ind_meta">das Individuum</param>
        /// <param name="iNachf">Nachfahrennummer</param>
        private void evaluate(ref BlueM.Opt.Common.Individuum_MetaEvo ind_meta, int iNachf)
        {
            bool storeInDB;
            BlueM.Opt.Common.Individuum ind;
            ind = ind_meta; //referenz

            //in ErgebnisDB speichern?
            storeInDB = (this.role == "Single PC") ? true : false;

            //Testproblem
            if (this.apptype == BlueM.Opt.Common.Constants.ApplicationTypes.Testproblems)
            {
                //Evaluieren und zeichnen
                this.testprobleme.Evaluate(ref ind, 0, ref this.hauptdiagramm1);
            }

            //Simulation
            else if (this.apptype == BlueM.Opt.Common.Constants.ApplicationTypes.Sim)
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
        /// Zeichnet (und speichert ggf. im OptResult) die Sekundäre Population (aktuelle Front)
        /// </summary>
        /// <param name="genpool">der Genpool</param>
        /// <remarks>Es werden nur Individuen mit Status "true" verwendet!</remarks>
        private void draw_and_store_sekpop(ref BlueM.Opt.Common.Individuum_MetaEvo[] genpool)
        {
            int i, j;
            BlueM.Opt.Common.Individuum[] sekpop;

            sekpop = new BlueM.Opt.Common.Individuum[0];

            //alle nicht-dominierten Lösungen herausfiltern
            j = 0;
            for (i = 0; i < genpool.Length; i++)
            {
                if (genpool[i].get_status() == "true")
                {
                    Array.Resize<BlueM.Opt.Common.Individuum>(ref sekpop, j + 1);
                    sekpop[j] = genpool[i].Clone();
                    j++;
                }
            }

            //SekPop in DB speichern (nur bei Sim und "Single PC")
            if (this.apptype == BlueM.Opt.Common.Constants.ApplicationTypes.Sim
                && this.role == "Single PC")
            {
                this.sim.OptResult.setSekPop(sekpop, settings.MetaEvo.CurrentGeneration - 1);
            }

            //SekPop zeichnen
            this.hauptdiagramm1.ZeichneSekPopulation(sekpop);
            System.Windows.Forms.Application.DoEvents();
        }
    }
}
