using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;


namespace IHWB.EVO.MetaEvo
{
    class Algomanager
    {
        EVO.Common.EVO_Settings settings;
        public EVO.Common.Individuum_MetaEvo[] genpool;
        EVO.Common.Individuum_MetaEvo[] wastepool;
        EVO.Diagramm.Monitor monitor1;
        public Algos algos;
        bool firstrun = true;
        public EVO.MO_Indicators.Solutionvolume2 solutionvolume;
        public string localausgabe;
        public string localausgabe2;
        int localcounter;
        int localcounter2;

        //Monitor
        private Steema.TeeChart.Styles.Line Line_diversity;
        private Steema.TeeChart.Styles.Line Line_evo_avg;
        private Steema.TeeChart.Styles.Line Line_evo_threshold;

        public Algomanager(ref EVO.Common.Problem prob_input, ref EVO.Common.EVO_Settings settings_input, int individuumnumber_input) 
        {
            settings = settings_input;
            monitor1 = EVO.Diagramm.Monitor.getInstance();
            localausgabe = "";
            localausgabe2 = "";
            localcounter = settings.MetaEvo.NumberResults;
            localcounter2 = 0;

            //Algoobjekt initialisieren (enthält die algorithmus-Methoden und das Feedback zu jedem Algo)
            algos = new Algos(ref settings_input, individuumnumber_input, ref monitor1);
            if ((settings.MetaEvo.OpMode == "Both") || (settings.MetaEvo.OpMode == "Global Optimizer"))
            {
                algos.set_algos("Zufällige Einfache Mutation, Ungleichverteilte Mutation, Zufällige Rekombination, Intermediäre Rekombination, Diversität aus Sortierung, Totaler Zufall, Dominanzvektor");
                settings.MetaEvo.AlgoMode = "Global: Calculating";
            }
            else if (settings.MetaEvo.OpMode == "Local Optimizer")
            {
                algos.set_algos("Hook and Jeeves");
                settings.MetaEvo.AlgoMode = "Local: Calculating";
            }

            //Solutionvolume initialisieren
            solutionvolume = new EVO.MO_Indicators.Solutionvolume2(prob_input.NumPrimObjective, 5, 2);

            //Monitor initialisieren
            this.initMonitor();
            monitor1.SelectTabDiagramm();
            monitor1.Show();
        }

        //new_generation mit Genpool verarbeiten und neue Individuen in new_generation erzeugen
        public void new_individuals_merge_with_genpool(ref EVO.Common.Individuum_MetaEvo[] new_generation_input)
        {
            Random rand = new Random();
            int difference2genpool = 0;
            this.monitor1.LogAppend("Algo Manager: Input: Generated and Simulated Individuums: \r\n" + this.generationinfo(ref new_generation_input));

            if (settings.MetaEvo.OpMode == "Local Optimizer")
            {
                //0. Vorbereitung
                //0.1. Feasible-Status prüfen
                set_notfeasible2false(ref genpool, ref new_generation_input);

                //Solutionvolume noch einmal berechnen um Fortschritt der lokalen Optimierung zu zeigen
                solutionvolume.calculate(ref genpool);

                //Solutionvolume zeichnen
                this.updateMonitor();

                for (int i = 0; i < genpool.Length; i++ )
                {
                    if (genpool[i].feedbackdata[2, 0] != 50) difference2genpool++;
                }
                localcounter2++;

                if (localcounter != difference2genpool)
                {
                    localausgabe = localausgabe + "\t" + localcounter2 + "x " + localcounter;
                    localausgabe2 = localausgabe2 + "\t" + solutionvolume.get_last_infos();
                    localcounter2 = 0;
                    localcounter = difference2genpool;
                }
                this.monitor1.LogAppend("Algo Manager: Result: New Genpool: \r\n" + this.generationinfo(ref genpool) + "\r\n"); 
            }
            
            else if ((settings.MetaEvo.OpMode == "Both") || (settings.MetaEvo.OpMode == "Global Optimizer"))
            {
                //0. Vorbereitung
                //0.1. Feasible-Status prüfen
                set_notfeasible2false(ref genpool, ref new_generation_input);
                //1.Selektion: 
                //1.1.Sortieren nach einem zufällig gewählten Kriterium
                int kriterium = rand.Next(0, new_generation_input[0].PrimObjectives.Length);
                //1.2.Dominanzkriterium auf Penalties zwischen den neuen und den alten Individuen anwenden
                check_domination(ref genpool, ref new_generation_input);
                //1.3.Sortieren und Dominanzkriterium anwenden innerhalb der Penalties der neuen Individuen
                quicksort(ref new_generation_input, kriterium, 0, new_generation_input.Length - 1);
                //1.4.Dominanzkriterium auf Penalties innerhalb der neuen Individuen
                check_domination(ref new_generation_input, ref new_generation_input);

                //2.Mengenanpassung des Genpools:
                //2.1 Anzahl der überlebenden Individuen
                difference2genpool = calculate_difference2genpool(ref genpool, ref new_generation_input);
                //2.2.Clustering bis auf maximale Generationsgrösse, Speichern in Genpool
                if (difference2genpool > 0) crowding_selection(difference2genpool, ref genpool, ref new_generation_input);
                //2.3.Individuen wiederbeleben falls zu wenige zur Verfügung stehen
                else if (difference2genpool < 0) revive(difference2genpool, ref genpool, ref new_generation_input);

                //3.Feedback erstellen:
                newGen_composition(ref new_generation_input);

                //4.Neuen Genpool erstellen:
                //4.1.Genpool und neue Individuen sortieren
                zip(ref genpool, ref new_generation_input);
                //4.2.false-Individuen in Wastepool verschieben 
                wastepool = new_generation_input;
                //4.3.Sortieren
                //quicksort(ref wastepool, kriterium, 0, wastepool.Length - 1); //Nötig??
                quicksort(ref genpool, kriterium, 0, genpool.Length - 1);
                this.monitor1.LogAppend("Algo Manager: Result: New Genpool: \r\n" + this.generationinfo(ref genpool) + "\r\n"); 
                
                //5.Solutionvolume berechnen (Reihenfolge im IF wichtig, da immer das solutionvolume berechnet werden soll)
                if ((solutionvolume.calculate(ref genpool)) && (settings.MetaEvo.OpMode == "Both"))
                {
                    //Solutionvolume entscheidet auf Umschaltung zur lokalen Optimierung
                    settings.MetaEvo.AlgoMode = "Global: Finished";
                }

                //Solutionvolume zeichnen
                this.updateMonitor();
            }

            this.monitor1.LogAppend("Algo Manager: Solutionvolume: Last Volume: " + solutionvolume.get_complete_infos());
        }

        //Erzeugt mit Hilfe von Algos die neuen Individuen
        public void new_individuals_build(ref EVO.Common.Individuum_MetaEvo[] new_generation_input)
        {
            //Für Initialisierung mit lokaler Optimierung muss die neue generation erst erzeugt werden
            if (firstrun)
            {
                for (int i = 0; i < genpool.Length; i++)
                {
                    genpool[i].set_generator(-1);
                    new_generation_input[i * 3] = genpool[i].Clone_MetaEvo();
                    new_generation_input[i * 3 + 1] = genpool[i].Clone_MetaEvo();
                    new_generation_input[i * 3 + 2] = genpool[i].Clone_MetaEvo();
                }
                firstrun = false;
            }
            //Generierung neuer Individuen (wieder in new_generation_input)
            algos.newGeneration(ref genpool, ref new_generation_input, ref wastepool);
        }

        //Setzt den Genpool
        public void set_genpool(ref EVO.Common.Individuum_MetaEvo[] genpool_input)
        {
            this.wastepool = new EVO.Common.Individuum_MetaEvo[genpool_input.Length];
            this.genpool = new EVO.Common.Individuum_MetaEvo[genpool_input.Length];
            for (int i = 0; i < genpool_input.Length; i++)
            {
                this.genpool[i] = genpool_input[i].Clone_MetaEvo();
            }
            this.monitor1.LogAppend("Algo Manager: Genpool: \r\n" + this.generationinfo(ref this.genpool));
        }

        //Wiederbelebte Feasible-Individuen wieder auf false setzen
        private void set_notfeasible2false(ref EVO.Common.Individuum_MetaEvo[] input, ref EVO.Common.Individuum_MetaEvo[] input2)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].Is_Feasible) input[i].set_status("true");
                else input[i].set_status("false#constraints#");
            }
            for (int i = 0; i < input2.Length; i++)
            {
                if (input2[i].Is_Feasible) input2[i].set_status("true");
                else input2[i].set_status("false#constraints#");
            }
        }

        //Sortieren nach einem gegebenen Penaltie(ok)
        private void quicksort(ref EVO.Common.Individuum_MetaEvo[] input, int kriterium, int low_input, int high_input)
        {
            int low = low_input;
            int high = high_input;
            double medianvalue = input[(low_input + high_input) / 2].PrimObjectives[kriterium];
            EVO.Common.Individuum_MetaEvo individuum_tmp;

            while (low < high)
            {
                //Nächste Abweichung von der erstrebten Sortierung suchen
                while ((input[low].PrimObjectives[kriterium] < medianvalue) && (low < high)) low++;
                while ((input[high].PrimObjectives[kriterium] > medianvalue) && (low < high)) high--;
                //Tauschen falls indizes "high" und "low" entsprechend stehen
                if (low < high)
                {
                    individuum_tmp = input[low];
                    input[low] = input[high];
                    input[high] = individuum_tmp;
                    low++;
                    high--;
                }
            }
            if (low == high)
            {
                if (input[low].PrimObjectives[kriterium] < medianvalue) high++;
                else if (input[low].PrimObjectives[kriterium] == medianvalue) { high++; low--; }
                else low--;
            }
            else { low--; high++; }
            if (low_input < low) quicksort(ref input, kriterium, low_input, low);
            if (high < high_input) quicksort(ref input, kriterium, high, high_input);
        }

        //Prüfen ob ein ein Individuum von einem anderen Individuum dominiert wird (kleiner ist dominant)
        private void check_domination(ref EVO.Common.Individuum_MetaEvo[] genpool_input, ref EVO.Common.Individuum_MetaEvo[] input2)
        {
            int dominator = -1;
            int status = 0;
            int pointer_genpool = 0;
            int pointer_newgen = 0;

            //Falls input1 = input2, nur innerhalb der ersten Generation prüfen
            if (genpool_input == input2)
            {
                //Individuen vergleichen
                for (int i = 0; i < genpool_input.Length; i++)
                {
                    if (genpool_input[i].get_status() == "true")
                    {
                        //mit
                        for (int k = genpool_input.Length - 1; k > i; k--)
                        {
                            if (genpool_input[k].get_status() == "true")
                            {
                                //Jede Eigenschaft vergleichen
                                for (int j = 0; j < genpool_input[0].PrimObjectives.Length; j++)
                                {
                                    if (genpool_input[i].PrimObjectives[j] <= genpool_input[k].PrimObjectives[j]) dominator = i;
                                    else if (genpool_input[i].PrimObjectives[j] > genpool_input[k].PrimObjectives[j]) { dominator = -1; break; }
                                }
                                if (dominator > -1)
                                {
                                    genpool_input[k].set_status("false#dominated#" + genpool_input[dominator].ID);
                                    this.monitor1.LogAppend("Algo Manager: Domination: Individuum " + genpool_input[k].ID + " is dominated (by Individuum " + genpool_input[dominator].ID + ")");
                                    dominator = -1;
                                }
                            }
                        }
                    }
                }
            }
            //Sonst Generationen untereinander prüfen (aber nicht innerhalb von sich selbst)
            else
            {
                while (pointer_genpool < genpool_input.Length)
                {
                    if (genpool_input[pointer_genpool].get_status() == "true")
                    {
                        pointer_newgen = 0;
                        while (pointer_newgen < input2.Length)
                        {
                            if ((input2[pointer_newgen].get_status() == "true") && (genpool_input[pointer_genpool].get_status() == "true"))
                            {
                                //Jede Eigenschaft vergleichen
                                for (int j = 0; j < genpool_input[0].PrimObjectives.Length; j++)
                                {
                                    //falls gleich, nichts tun
                                    if (genpool_input[pointer_genpool].PrimObjectives[j] > input2[pointer_newgen].PrimObjectives[j])
                                    {
                                        if (status == -1)
                                        {
                                            status = 10;
                                            break;
                                        }
                                        status = 1;
                                    }
                                    if (genpool_input[pointer_genpool].PrimObjectives[j] < input2[pointer_newgen].PrimObjectives[j])
                                    {
                                        if (status == 1)
                                        {
                                            status = 10;
                                            break;
                                        }
                                        status = -1;
                                    }
                                }
                                switch (status)
                                {
                                    case -1:
                                        input2[pointer_newgen].set_status("false#dominated#" + genpool_input[pointer_genpool].ID);
                                        this.monitor1.LogAppend("Algo Manager: Domination: Individuum " + input2[pointer_newgen].ID + " is dominated (by Individuum " + genpool_input[pointer_genpool].ID + ")");
                                        break;

                                    case 1:
                                        genpool_input[pointer_genpool].set_status("false#dominated#" + input2[pointer_newgen].ID);
                                        this.monitor1.LogAppend("Algo Manager: Domination: Individuum " + genpool_input[pointer_genpool].ID + " is dominated (by Individuum " + input2[pointer_newgen].ID + ")");
                                        break;

                                    case 0:
                                        genpool_input[pointer_genpool].set_status("false#equal#" + input2[pointer_newgen].ID);
                                        this.monitor1.LogAppend("Algo Manager: Domination: Individuum " + genpool_input[pointer_genpool].ID + " equals Individuum " + input2[pointer_newgen].ID);
                                        break;

                                    case 10:
                                        break;
                                }
                                status = 0;
                            }
                            if (genpool_input[pointer_genpool].get_status() == "false") break;
                            pointer_newgen++;
                        }        
                    }
                    pointer_genpool++;
                }
            }    
        }

        //Individuen die einen zu geringen Abstand der Penalties besitzen, löschen
        private void crowding_selection(int killindividuums_input, ref EVO.Common.Individuum_MetaEvo[] genpool_input, ref EVO.Common.Individuum_MetaEvo[] input2)
        {
            int killindividuums = killindividuums_input;
            double[] densities = new double[genpool_input.Length + killindividuums];
            int pointer = 0;

            this.monitor1.LogAppend("Algo Manager: Number of new Individuums has a difference to Genpool-size: +" + killindividuums_input + " -> Crowding Selection");

            //Arbeits-Array erstellen (genau so gross dass alle "true"-Individuen Platz finden)
            EVO.Common.Individuum_MetaEvo[] work = new IHWB.EVO.Common.Individuum_MetaEvo[genpool_input.Length + killindividuums];
            for (int i = 0; i < genpool_input.Length; i++)
            {
                if (genpool_input[i].get_status() == "true")
                {
                    work[pointer] = genpool_input[i];
                    pointer++;
                }
            }
            for (int i = 0; i < input2.Length; i++)
            {
                if (input2[i].get_status() == "true")
                {
                    work[pointer] = input2[i];
                    pointer++;
                }
            }

            //Individuen mit höchsten Dichten entfernen
            while (killindividuums > 0)
            {
                //Dichten bestimmen lassen
                densities = calculate_densities(ref work);

                pointer = 0;
                //tmp = " [" + 0 + "]:" + densities[0] + " ";
                for (int i = 1; i < densities.Length; i++)
                {
                    if (densities[i] > densities[pointer]) pointer = i;
                    //tmp = tmp + " ["+i+"]:" + densities[i]+ " ";
                }
                //this.monitor1.LogAppend("Algo Manager: Crowding: Densities: " + tmp);

                densities[pointer] = 0;
                this.monitor1.LogAppend("Algo Manager: Crowding: Individuum " + work[pointer].ID + " is not used anymore");
                work[pointer].set_status("false#crowding#0");

                killindividuums--;
            }    
        }

        //Individuen wiederbeleben 
        private void revive(int numberawake_input, ref EVO.Common.Individuum_MetaEvo[] genpool_input, ref EVO.Common.Individuum_MetaEvo[] input2)
        {
            //Individuen überleben deren Abstand zum nächster lebenden Nachbar gering ist
            //aber dessen Abstand zu seinem nächsten lebenden Nachbar hoch ist
            double[,] distances; //Individuum[distance zu nähstem true-Individuum, pointer auf nächstes true-Individuum, Ranking]
            double[] densities;
            int pointer = 0;
            double distance = -1;

            this.monitor1.LogAppend("Algo Manager: Number of new Individuums has a difference to Genpool-size: " + numberawake_input + " -> Revive");

            //Arbeits-Array erstellen 
            EVO.Common.Individuum_MetaEvo[] work = new IHWB.EVO.Common.Individuum_MetaEvo[genpool_input.Length + input2.Length];
            for (int i = 0; i < genpool_input.Length; i++)
            {
                work[pointer] = genpool_input[i];
                pointer++;
            }
            for (int i = 0; i < input2.Length; i++)
            {
                work[pointer] = input2[i];
                pointer++;
            }

            //Näheste true-Individuen für alle false-Individuum finden
            distances = new double[work.Length, 3]; //Für alle false-Individuen
            for (int i = 0; i < work.Length; i++)
            {
                if (work[i].get_status() == "false")
                {
                    for (int j = 0; j < work.Length; j++)
                    {
                        //Nur im genpool kommen true-Individuen vor
                        if ((work[j].get_status() == "true") && (work[j].ID != work[i].ID))
                        {
                            distance = calculate_distance(work[i].PrimObjectives, work[j].PrimObjectives);
                            //Initialisierung oder Vergleich
                            if ((distances[i, 0] > distance) || (distances[i, 0] == 0))
                            {
                                distances[i, 0] = distance;
                                distances[i, 1] = j; //Pointer auf das zugehörige näheste true-Individuum
                                if (distance == -1) break;
                            }
                        }
                    }
                }
            }

            //Dichtewerte der true-Individuen berechnen lassen
            densities = calculate_densities(ref work);

            //Ranking bestimmen für alle False-Individuen
            for (int i = 0; i < work.Length; i++)
            {
                if (work[i].get_status() == "false")
                {
                    //Individuum-Kopie werden nicht berücksichtigt 
                    if (distances[i, 0] != -1)
                    {
                        //Abstand des false-Individuums zum nächsten true-Individuum soll minimal sein
                        //Abstand des true-Individuums zu seinem nächsten true-Individuum soll maximal sein
                        distances[i, 2] = densities[(int)distances[i, 1]] / distances[i, 0];
                    }
                }
                else distances[i, 2] = 0;
            }

            //Individuen "wiederbeleben"
            while (numberawake_input < 0)
            {
                int pointer_highest_ranking = 0;
                for (int i = 1; i < work.Length; i++)
                {
                    if ((distances[i, 2] > distances[pointer_highest_ranking, 2]) && (work[pointer_highest_ranking].get_status_reason() != "constraints")) pointer_highest_ranking = i;
                }
                distances[pointer_highest_ranking, 2] = -1;
                this.monitor1.LogAppend("Algo Manager: Diversity: Individuum " + work[pointer_highest_ranking].ID + " is used again");
                work[pointer_highest_ranking].set_status("true");

                numberawake_input++;
            }
        }

        //Kopieren der Verbleibenden Individuen auf die Generation input
        private void zip(ref EVO.Common.Individuum_MetaEvo[] genpool_input, ref EVO.Common.Individuum_MetaEvo[] input2)
        {
            int pointer_true = 0;
            int pointer_false = 0;
            EVO.Common.Individuum_MetaEvo tmp;

            //genpool_input: True-Individuen ; input2: False-Individuen
            while (pointer_true < genpool_input.Length)
            {
                if (genpool_input[pointer_true].get_status() == "false") 
                {
                    while (pointer_false < input2.Length)
                    {
                        if (input2[pointer_false].get_status() == "true")
                        {
                            tmp = genpool_input[pointer_true];
                            genpool_input[pointer_true] = input2[pointer_false];
                            input2[pointer_false] = tmp;
                            pointer_false++;
                            break;
                        }
                        pointer_false++;
                    }
                    if (pointer_false == input2.Length) break;
                }
                pointer_true++;
            }
        }

        //String mit Auszug der generation
        private string generationinfo(ref EVO.Common.Individuum_MetaEvo[] generation)
        {
            string back = "-----Generation-----\r\n";
            double[] optparas;

            for (int i = 0; i < generation.Length; i++)
            {
                back = back + "[ID]: " + generation[i].ID + " [Generator]: " + generation[i].get_generator() + " [Status]: " + generation[i].get_status() + " [Client]: " + generation[i].get_client();
                optparas = generation[i].get_optparas();
                back = back + "\r\nOptparas: ";
                for (int j = 0; j < optparas.Length; j++)
                {
                    back = back + "[" + j + "]: " + String.Format("{0:N3}",optparas[j]) + " ";
                }
                if (generation[0].Constraints.Length > 0)
                {
                    back = back + "\r\nConst";
                    for (int j = 0; j < generation[i].Constraints.Length; j++)
                    {
                        back = back + "[" + j + "]: " + String.Format("{0:N3}", generation[i].Constraints[j]) + " ";
                    }
                }
                if (generation[0].PrimObjectives.Length > 0)
                {
                    back = back + "\r\nPenaltie";
                    for (int j = 0; j < generation[i].PrimObjectives.Length; j++)
                    {
                        back = back + "[" + j + "]: " + String.Format("{0:N3}", generation[i].PrimObjectives[j]) + " ";
                    }
                }
                back = back + "\r\n";
            }

            return back + "----------";
        }

        //Feedback erstellen und Durchläufe zählen die keine Verbesserung erzeugt haben
        private void newGen_composition(ref EVO.Common.Individuum_MetaEvo[] new_generation_input)
        {
            double initiativensumme = 0;
            double survivingrate = 0;
            double initproind = 0;
            int currentalgo = 0;
            string[] log = new string[algos.algofeedbackarray.Length + 1];
            log[algos.algofeedbackarray.Length] = "";

            //1. Reset der Daten der letzten Generation
            for (int i = 0; i < algos.algofeedbackarray.Length; i++)
            {
                algos.algofeedbackarray[i].number_individuals_survived = 0;    
            }

            //2. Neue Überlebens-Zahlen aus der simulierten Generation auslesen
            for (int i = 0; i < new_generation_input.Length; i++)
            {
                if (new_generation_input[i].get_status() == "true")
                {
                    algos.algofeedbackarray[new_generation_input[i].get_generator()].number_individuals_survived++;
                    //survivingrate++;
                }
            }

            //survivingrate = 0;

            //3. Initiative für im Einsatz befindliche Algorithmen neu berechnen 
            for (int i = 0; i < algos.algofeedbackarray.Length; i++)
            {
                if (algos.algofeedbackarray[i].number_individuals_for_nextGen != 0)
                {
                    survivingrate = (double)algos.algofeedbackarray[i].number_individuals_survived / (double)algos.algofeedbackarray[i].number_individuals_for_nextGen;
                    algos.algofeedbackarray[i].initiative = Math.Round(algos.algofeedbackarray[i].initiative * (survivingrate * survivingrate + 0.5 * survivingrate + 0.5), 3);
                    
                }
            }

            //4. Initiative für pausierte Algorithmen leicht erhöhen (um ein halbes Individuum)
            for (int i = 0; i < algos.algofeedbackarray.Length; i++)
            {
                if (algos.algofeedbackarray[i].number_individuals_for_nextGen == 0)
                {
                    algos.algofeedbackarray[i].initiative += initiativensumme/(2*new_generation_input.Length);
                }
                log[i] = "[" + algos.algofeedbackarray[i].name + "]: Survived: " + algos.algofeedbackarray[i].number_individuals_survived + "/" + algos.algofeedbackarray[i].number_individuals_for_nextGen + " -> ";
                initiativensumme += algos.algofeedbackarray[i].initiative;
                
                //Produktionszahlen für nächste generation resetten
                algos.algofeedbackarray[i].number_individuals_for_nextGen = 0;
            }

            this.monitor1.LogAppend("Algo Manager: nemGen_composition: Initiativ-sum: " + initiativensumme);

            //5. number_individuals_for_nextGen neu setzen
            initproind = initiativensumme / new_generation_input.Length;
            for (int j = 0; j < new_generation_input.Length; j++)
            {
                currentalgo = 0;
                for (int i = 1; i < algos.algofeedbackarray.Length; i++)
                {
                    //Der Algorithmus simuliert das Individuum wenn er die höchste Restinitiative besitzt
                    if (algos.algofeedbackarray[i].initiative - ((algos.algofeedbackarray[i].number_individuals_for_nextGen) * initproind) > algos.algofeedbackarray[currentalgo].initiative - ((algos.algofeedbackarray[currentalgo].number_individuals_for_nextGen) * initproind))
                    {
                        currentalgo = i;
                    }
                }
                algos.algofeedbackarray[currentalgo].number_individuals_for_nextGen++;
            }

            //6. Ausgabe zusammenfügen
            for (int i = 0; i < algos.algofeedbackarray.Length; i++)
            {
                log[algos.algofeedbackarray.Length] = log[algos.algofeedbackarray.Length] + log[i] + " Initiative: " + algos.algofeedbackarray[i].initiative + " = " + algos.algofeedbackarray[i].number_individuals_for_nextGen + " Individuums for next generation\r\n";    
            }
            this.monitor1.LogAppend("Algo Manager: nemGen_composition: Individuuum-Composition for next Generation:\r\n" + log[algos.algofeedbackarray.Length]);


            //7. Initiative bei Bedarf erhöhen (Falls zur Berechnung zu klein)
            if (initiativensumme < 1.00)
            {
                this.monitor1.LogAppend("Algo Manager: nemGen_composition: Initiative will be increased to the sum of 1(was too low for good calculation)");
                double tmp = 1 / initiativensumme;
                for (int i = 0; i < algos.algofeedbackarray.Length; i++)
                {
                    algos.algofeedbackarray[i].initiative *= tmp;
                }
            }
        }

        //Kopiere true-Individuen auf neuen Genpool
        private void copy_true_to(ref EVO.Common.Individuum_MetaEvo[] genpool_input, ref EVO.Common.Individuum_MetaEvo[] genpool_output)
        {
            int index = 0;
            for (int i = 0; i < genpool_input.Length; i++)
            {
                if (genpool_input[i].get_status() == "true")
                {
                    genpool_output[index] = genpool_input[i];
                    index++;
                }
            }
        }

        //Kopiere true-Individuen auf neuen Genpool
        private void copy_some_to(ref EVO.Common.Individuum_MetaEvo[] newgen_input, ref EVO.Common.Individuum_MetaEvo[] newgen_output)
        {
            for (int i = 0; i < newgen_output.Length; i++)
            {
                newgen_output[i] = newgen_input[i];
            }
        }

        //Umschalten auf Lokale Algorithmen, Reduzieren der Lösungen
        public void set_calculationmode_local(ref EVO.Common.Individuum_MetaEvo[] new_generation_input)
        {
            settings.MetaEvo.AlgoMode = "Local: Calculating";

            if (settings.MetaEvo.NumberResults < genpool.Length)
            {
                this.monitor1.LogAppend("Algo Manager: Reducing Genpool to " + settings.MetaEvo.NumberResults + " Individuums");
                //Nur noch NumberResults Individuen überleben
                crowding_selection((genpool.Length - settings.MetaEvo.NumberResults), ref genpool, ref new_generation_input);
                //Lokaler Algo darf nur NumberResults Prozesse Starten
                algos.algofeedbackarray[0].number_individuals_for_nextGen = settings.MetaEvo.NumberResults;
                //Neuen Genpool und Generation wegen Lösungsreduzierung
                EVO.Common.Individuum_MetaEvo[] genpool2 = new IHWB.EVO.Common.Individuum_MetaEvo[settings.MetaEvo.NumberResults];
                EVO.Common.Individuum_MetaEvo[] new_generation_input2 = new IHWB.EVO.Common.Individuum_MetaEvo[settings.MetaEvo.NumberResults * settings.MetaEvo.ChildsPerParent];
                copy_true_to(ref genpool, ref genpool2);
                copy_some_to(ref new_generation_input, ref new_generation_input2);
                genpool = genpool2;
                new_generation_input = new_generation_input2;
                this.monitor1.LogAppend("Algo Manager: Genpool for local Optimization: \r\n" + this.generationinfo(ref genpool) + "\r\n"); 
                    
            }
            
            for (int i = 0; i < genpool.Length; i++)
            {
                genpool[i].set_generator(-1);
            }
            for (int i = 0; i < new_generation_input.Length; i++)
            {
                new_generation_input[i].set_generator(-1);
            }

            //Neue Algorithmuskomposition
            algos.set_algos("Hook and Jeeves");
        }

        //Reduzieren der Lösungen
        public void set_calculationmode_global_finished(ref EVO.Common.Individuum_MetaEvo[] new_generation_input)
        {
            if (settings.MetaEvo.NumberResults < genpool.Length)
            {
                this.monitor1.LogAppend("Algo Manager: Reducing Genpool to " + settings.MetaEvo.NumberResults + " Individuums");
                //Nur noch NumberResults Individuen überleben
                crowding_selection((genpool.Length - settings.MetaEvo.NumberResults), ref genpool, ref new_generation_input);
                //Neuen Genpool und Generation wegen Lösungsreduzierung
                EVO.Common.Individuum_MetaEvo[] genpool2 = new IHWB.EVO.Common.Individuum_MetaEvo[settings.MetaEvo.NumberResults];
                copy_true_to(ref genpool, ref genpool2);
                genpool = genpool2;
                this.monitor1.LogAppend("Algo Manager: Final Genpool: \r\n" + this.generationinfo(ref genpool) + "\r\n");
            }
        }

        //Mengendifferenz zwischen Genpoolgrösse und überlebenden Individuen
        private int calculate_difference2genpool(ref EVO.Common.Individuum_MetaEvo[] genpool_input, ref EVO.Common.Individuum_MetaEvo[] input2)
        {
            int alive = 0;

            for (int i = 0; i < genpool_input.Length; i++)
            {
                if (genpool_input[i].get_status() == "true") alive++;
            }
            for (int i = 0; i < input2.Length; i++)
            {
                if (input2[i].get_status() == "true") alive++;
            }

            return (alive - genpool_input.Length);
        }

        //Einfache Distanzsumme zwischen zwei Arrays; falls Abstand = 0, return -1
        private double calculate_distance(double[] input1, double[] input2)
        {
            double back = 0;

            for (int i = 0; i < input1.Length; i++)
            {
                back += (input1[i] - input2[i]) * (input1[i] - input2[i]);
            }
            if (back == 0) return -1;
            return System.Math.Sqrt(back);
        }

        //Dichte des Raumes in dem sich das Individuum befindet
        private double[] calculate_densities(ref EVO.Common.Individuum_MetaEvo[] work_input)
        {
            double[] densitys = new double[work_input.Length];
            double tmp;

            
            for (int i = 0; i < work_input.Length; i++)
            {
                if ((work_input[i] != null) && (work_input[i].get_status() == "true"))
                {
                    for (int j = 0; j < work_input.Length; j++)
                    {
                        if ((work_input[j] != null) && (work_input[j].get_status() == "true") && (i != j))
                        {
                            tmp = calculate_distance(work_input[i].PrimObjectives, work_input[j].PrimObjectives);
                            if (tmp == -1) densitys[i]++;
                            else densitys[i] += (1 / ((tmp + 1) * (tmp + 1)) );
                        }
                    }
                }
            }
            return densitys;
        }

        /// <summary>
        /// Monitor Initialisieren
        /// </summary>
        private void initMonitor()
        {
            //Achsen

            //Diversitätsachse
            monitor1.Diag.Axes.Left.Title.Caption = "Diversität";

            //Generationsachse (unten)
            monitor1.Diag.Axes.Bottom.Visible = true;
            monitor1.Diag.Axes.Bottom.Title.Caption = "Generation";
            monitor1.Diag.Axes.Bottom.Horizontal = true;
            monitor1.Diag.Axes.Bottom.Automatic = true;
            monitor1.Diag.Axes.Bottom.Grid.Visible = true;

            //Entwicklungsachse (rechts)
            monitor1.Diag.Axes.Right.Visible = true;
            monitor1.Diag.Axes.Right.Title.Caption = "Entwicklung";
            monitor1.Diag.Axes.Right.Title.Angle = 90;
            monitor1.Diag.Axes.Right.Automatic = true;
            monitor1.Diag.Axes.Right.Grid.Visible = false;

            //Linie Diversität 
            Line_diversity = monitor1.Diag.getSeriesLine("Diversität (Durchschnittlicher Abstand zu einem Mittelpunkt)", "Red");
            Line_diversity.Color = System.Drawing.Color.Blue;
            Line_diversity.Pointer.Visible = true;
            Line_diversity.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle;
            //Line_diversity.Pointer.Brush.Color = System.Drawing.Color.Blue;
            Line_diversity.Pointer.HorizSize = 2;
            Line_diversity.Pointer.VertSize = 2;
            Line_diversity.Pointer.Pen.Visible = false;

            //Linie Entwicklung
            Line_evo_avg = monitor1.Diag.getSeriesLine("Durchschnittliche Entwicklung über " + solutionvolume.historylength + " Generationen", "Green");
            Line_evo_avg.CustomVertAxis = monitor1.Diag.Axes.Right;
            Line_evo_avg.Color = System.Drawing.Color.LimeGreen;
            Line_evo_avg.Pointer.Visible = true;
            Line_evo_avg.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle;
            //Line_evo_avg.Pointer.Brush.Color = System.Drawing.Color.LimeGreen;
            Line_evo_avg.Pointer.HorizSize = 2;
            Line_evo_avg.Pointer.VertSize = 2;
            Line_evo_avg.Pointer.Pen.Visible = false;

            //Linie Schwellwert
            Line_evo_threshold = monitor1.Diag.getSeriesLine("Entwicklung Grenze", "Yellow");
            Line_evo_threshold.CustomVertAxis = monitor1.Diag.Axes.Right;
            Line_evo_threshold.Color = System.Drawing.Color.Green;
            Line_evo_threshold.Pointer.Visible = true;
            Line_evo_threshold.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle;
            //Line_evo_threshold.Pointer.Brush.Color = System.Drawing.Color.Green;
            Line_evo_threshold.Pointer.HorizSize = 2;
            Line_evo_threshold.Pointer.VertSize = 2;
            Line_evo_threshold.Pointer.Pen.Visible = false;
        }

        /// <summary>
        /// Aktualisiert das Monitordiagramm mit den aktuellen Solutionvolume-Werten
        /// </summary>
        private void updateMonitor()
        {
            this.Line_diversity.Add(this.settings.MetaEvo.CurrentGeneration, solutionvolume.diversity);
            this.Line_evo_avg.Add(this.settings.MetaEvo.CurrentGeneration, solutionvolume.evosum);
            this.Line_evo_threshold.Add(this.settings.MetaEvo.CurrentGeneration, solutionvolume.maxevosum / solutionvolume.faktor2switch);
        }

    }
}