using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IHWB.EVO.MetaEvo
{
    class Algofeedback
    {
        //Administrative Eigenschaften
        public string name;
        public int number_individuals_for_nextGen;
        public int number_individuals_survived;
        public double initiative;

        public Algofeedback(string name_input, int number_individuals_for_nextGen_input)
        {
            this.name = name_input;
            this.initiative = 10;
            this.number_individuals_for_nextGen = number_individuals_for_nextGen_input;
        }
    }

    class Algos
    {
        public Algofeedback[] algofeedbackarray;
        int individuum_id;
        int ChildsPerParent;
        int populationsize;
        int HJ_minimumstepsize;
        EVO.Diagramm.ApplicationLog applog;

        public Algos(ref EVO.Common.EVO_Settings settings_input, int individuum_id_input, ref EVO.Diagramm.ApplicationLog applog_input)
        {
            individuum_id = individuum_id_input;
            applog = applog_input;
            ChildsPerParent = settings_input.MetaEvo.ChildsPerParent;
            populationsize = settings_input.MetaEvo.PopulationSize;
            HJ_minimumstepsize = settings_input.MetaEvo.HJStepsize;
        }

        //Legt fest welche Algorithmen genutzt werden sollen
        public void set_algos(string algos2use_input) 
        {
            string[] tmp;

            tmp = algos2use_input.Split(',', ';');
            algofeedbackarray = new Algofeedback[tmp.Length];

            for (int i = 0; i < tmp.Length; i++)
            {
                algofeedbackarray[i] = new Algofeedback(tmp[i].Trim(), (populationsize * ChildsPerParent) / algofeedbackarray.Length);
                
            }
            applog.appendText("Algos: Using Algos: " + algos2use_input);
        }

        //Neue Generation erzeugen
        public void newGeneration(ref EVO.Common.Individuum_MetaEvo[] genpool_input, ref EVO.Common.Individuum_MetaEvo[] new_generation_input, ref EVO.Common.Individuum_MetaEvo[] wastepool_input)
        {
            int startindex;

            startindex = 0;

            for (int algo_id = 0; algo_id < algofeedbackarray.Length; algo_id++)
            {
                this.build_individuals(ref genpool_input, ref new_generation_input, ref wastepool_input, algo_id, startindex);
                startindex += algofeedbackarray[algo_id].number_individuals_for_nextGen;
            }
        }

        public void build_individuals(ref EVO.Common.Individuum_MetaEvo[] genpool_input, ref EVO.Common.Individuum_MetaEvo[] new_generation_input, ref EVO.Common.Individuum_MetaEvo[] wastepool_input, int algo_id, int startindex_input)
        {
            //Beschreibbare Individuen für einen Algo: generation[startindex] bis generation[startindex+numberindividuums]
            int startindex = startindex_input;
            int numberindividuums = this.algofeedbackarray[algo_id].number_individuals_for_nextGen;
            int numberoptparas = genpool_input[0].get_optparas().Length;
            int numberpenalties = genpool_input[0].Penalties.Length;
            int new_individuums_counter = 0;
            Random rand = new Random();

            //Sicherung gegen Rundungsfehler bei der Platzvergabe für die Algos
            if (startindex + numberindividuums >= new_generation_input.Length)
            {
                numberindividuums = new_generation_input.Length - startindex;
                algofeedbackarray[algo_id].number_individuals_for_nextGen = numberindividuums;
            }
            //Letzter Algo bekommt automatisch restliche Plätze
            if (algo_id == algofeedbackarray.Length - 1)
            {
                numberindividuums = new_generation_input.Length - startindex;
                algofeedbackarray[algo_id].number_individuals_for_nextGen = numberindividuums;
            }

            switch (algofeedbackarray[algo_id].name)
            {
                //Globale Algorithmen
                #region Zufällige Einfache Mutation: Mutiert an einer zufälligen Stelle innerhalb der Grenzen von Min und Max
                case "Zufällige Einfache Mutation":
                    {
                        applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with " + algo_id + ":'" + algofeedbackarray[algo_id].name + "'...done");

                        double[] mutated_optparas = new double[numberoptparas];
                        int selecteditem;

                        for (int i = 0; i < numberindividuums; i++)
                        {
                            //Zufälligen parent wählen und optparas kopieren
                            mutated_optparas = genpool_input[rand.Next(0, genpool_input.Length - 1)].get_optparas();
                            //Zufälligen Wert mutieren
                            selecteditem = rand.Next(0, numberoptparas - 1);
                            mutated_optparas[selecteditem] = genpool_input[1].OptParameter[selecteditem].Min + (genpool_input[1].OptParameter[selecteditem].Max - genpool_input[1].OptParameter[selecteditem].Min) * ((double)rand.Next(0, 1000) / (double)1000);
                            //Zurückspeichern
                            new_generation_input[startindex + i] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuum_id, numberoptparas);
                            new_generation_input[startindex + i].set_optparas(mutated_optparas);
                            new_generation_input[startindex + i].set_status("raw");
                            new_generation_input[startindex + i].set_generator(algo_id);
                            individuum_id++;
                        }
                        break;
                    }
                #endregion

                #region Feedback Mutation: Mutiert mit bewährtem Mutationsoperator und verbessert diesen
                case "Feedback Mutation":
                    {
                        //Feedbackdata: Pro Individuum: [[Mutationsparameter]] 
                        //Ein Mutationsparameter kann pro runde maximal um 10% verändert werden

                        applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with " + algo_id + ":'" + algofeedbackarray[algo_id].name + "'...done");

                        double[] fmutated_optparas = new double[numberoptparas];
                        int pointer_parent2 = 0;
                        new_individuums_counter = 0;
                        int selected_optpara = 0;

                        //Erfolgreiche Mutationsparameter suchen und damit weitere Individuen erzeugen
                        while ((new_individuums_counter < numberindividuums) && (pointer_parent2 < genpool_input.Length))
                        {
                            //Suchen ob ein Individuum aus einem Feedback Mutation Prozess überlebt hat
                            if (genpool_input[pointer_parent2].get_generator() == algo_id)
                            {
                                //Gleiche Mutation bei einem anderen parent anwenden
                                new_generation_input[startindex + new_individuums_counter] = genpool_input[rand.Next(0, genpool_input.Length - 1)].Clone_MetaEvo();
                                new_generation_input[startindex + new_individuums_counter].ID = individuum_id;
                                new_generation_input[startindex + new_individuums_counter].set_status("raw");
                                new_generation_input[startindex + new_individuums_counter].set_generator(algo_id);
                                new_generation_input[startindex + new_individuums_counter].feedbackdata = genpool_input[pointer_parent2].feedbackdata;

                                //Neue Optparas berechnen
                                fmutated_optparas = new_generation_input[startindex + new_individuums_counter].get_optparas();
                                for (int j = 0; j < numberoptparas; j++)
                                {
                                    if (new_generation_input[startindex + new_individuums_counter].feedbackdata[j, 0] != 1)
                                    {
                                        fmutated_optparas[j] *= new_generation_input[startindex + new_individuums_counter].feedbackdata[j, 0];
                                        if (fmutated_optparas[j] > genpool_input[1].OptParameter[j].Max) fmutated_optparas[j] = genpool_input[1].OptParameter[j].Max;
                                        if (fmutated_optparas[j] < genpool_input[1].OptParameter[j].Min) fmutated_optparas[j] = genpool_input[1].OptParameter[j].Min;
                                    }
                                }
                                new_generation_input[startindex + new_individuums_counter].set_optparas(fmutated_optparas);

                                individuum_id++;
                                new_individuums_counter++;



                                //Den vorhandenen parent kopieren und weiter mutieren
                                if (new_individuums_counter < numberindividuums)
                                {
                                    //Gleiche Mutation bei einem anderen parent anwenden
                                    new_generation_input[startindex + new_individuums_counter] = genpool_input[pointer_parent2].Clone_MetaEvo();
                                    new_generation_input[startindex + new_individuums_counter].ID = individuum_id;
                                    new_generation_input[startindex + new_individuums_counter].set_status("raw");
                                    new_generation_input[startindex + new_individuums_counter].set_generator(algo_id);

                                    //Optpara wählen
                                    selected_optpara = rand.Next(0, numberoptparas - 1);

                                    //Neue Optparas berechnen
                                    fmutated_optparas = new_generation_input[startindex + new_individuums_counter].get_optparas();
                                    for (int j = 0; j < numberoptparas; j++)
                                    {
                                        if (j == selected_optpara) //Nur die neue Mutation erzeugen und ausführen
                                        {
                                            new_generation_input[startindex + new_individuums_counter].feedbackdata[j, 0] = (rand.Next(0, 1000) / 50) - 10;
                                            fmutated_optparas[j] *= new_generation_input[startindex + new_individuums_counter].feedbackdata[j, 0];
                                            if (fmutated_optparas[j] > genpool_input[1].OptParameter[j].Max) fmutated_optparas[j] = genpool_input[1].OptParameter[j].Max;
                                            if (fmutated_optparas[j] < genpool_input[1].OptParameter[j].Min) fmutated_optparas[j] = genpool_input[1].OptParameter[j].Min;
                                        }
                                    }
                                    new_generation_input[startindex + new_individuums_counter].set_optparas(fmutated_optparas);

                                    individuum_id++;
                                    new_individuums_counter++;
                                }
                            }
                            pointer_parent2++;
                        }

                        //Neuen Parameter aufstellen und neue Individuen erzeugen
                        while (new_individuums_counter < numberindividuums)
                        {
                            //Gleiche Mutation bei einem anderen parent anwenden
                            new_generation_input[startindex + new_individuums_counter] = genpool_input[rand.Next(0, genpool_input.Length - 1)].Clone_MetaEvo();
                            new_generation_input[startindex + new_individuums_counter].ID = individuum_id;
                            new_generation_input[startindex + new_individuums_counter].set_status("raw");
                            new_generation_input[startindex + new_individuums_counter].set_generator(algo_id);
                            new_generation_input[startindex + new_individuums_counter].feedbackdata = new double[numberoptparas, 1];

                            //Neue Optparas berechnen
                            selected_optpara = rand.Next(0, numberoptparas - 1);
                            fmutated_optparas = new_generation_input[startindex + new_individuums_counter].get_optparas();

                            for (int j = 0; j < numberoptparas; j++)
                            {
                                if (j == selected_optpara) //Nur die neue Mutation erzeugen und ausführen
                                {
                                    new_generation_input[startindex + new_individuums_counter].feedbackdata[j, 0] = (rand.Next(0, 1000) / 50) - 10;
                                    fmutated_optparas[j] *= new_generation_input[startindex + new_individuums_counter].feedbackdata[j, 0];
                                    if (fmutated_optparas[j] > genpool_input[1].OptParameter[j].Max) fmutated_optparas[j] = genpool_input[1].OptParameter[j].Max;
                                    if (fmutated_optparas[j] < genpool_input[1].OptParameter[j].Min) fmutated_optparas[j] = genpool_input[1].OptParameter[j].Min;
                                }
                                else new_generation_input[startindex + new_individuums_counter].feedbackdata[j, 0] = 1;
                            }
                            new_generation_input[startindex + new_individuums_counter].set_optparas(fmutated_optparas);

                            individuum_id++;
                            new_individuums_counter++;
                        }
                        break;
                    }
                #endregion

                #region Ungleichverteilte Mutation: Mutiert an einer zufälligen Stelle (grössere Mutationen sind unwahrscheinlicher)
                case "Ungleichverteilte Mutation":
                    {
                        applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with " + algo_id + ":'" + algofeedbackarray[algo_id].name + "'...done");

                        double[] mutated_optparas2 = new double[numberoptparas];
                        int selecteditem2;
                        double random = 0;

                        for (int i = 0; i < numberindividuums; i++)
                        {
                            //Zufälligen parent wählen und optparas kopieren
                            mutated_optparas2 = genpool_input[rand.Next(0, genpool_input.Length - 1)].get_optparas();
                            //Zufälligen Wert mutieren
                            selecteditem2 = rand.Next(0, numberoptparas - 1);
                            random = ((rand.Next(-1000, 1000)) / 1000) * 1.15;
                            mutated_optparas2[selecteditem2] *= (random * random * random + 1); //x³ + 1: x < |1,15| -> Etwa: 0,5 < f(x) < 1,5 
                            //Grenzen prüfen
                            if (mutated_optparas2[selecteditem2] > genpool_input[1].OptParameter[selecteditem2].Max) mutated_optparas2[selecteditem2] = genpool_input[1].OptParameter[selecteditem2].Max;
                            if (mutated_optparas2[selecteditem2] < genpool_input[1].OptParameter[selecteditem2].Min) mutated_optparas2[selecteditem2] = genpool_input[1].OptParameter[selecteditem2].Min;
                            //Zurückspeichern
                            new_generation_input[startindex + i] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuum_id, numberoptparas);
                            new_generation_input[startindex + i].set_optparas(mutated_optparas2);
                            new_generation_input[startindex + i].set_status("raw");
                            new_generation_input[startindex + i].set_generator(algo_id);
                            individuum_id++;
                        }
                        break;
                    }
                #endregion

                #region Zufällige Rekombination: Die Werte zweier zufälliger Eltern werden zufällig rekombiniert
                case "Zufällige Rekombination":
                    {
                        applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with " + algo_id + ":'" + algofeedbackarray[algo_id].name + "'...done");

                        int selectedind;
                        int selectedind2;
                        double[] recombinated_optparas = new double[numberoptparas];
                        double[] recombinated_optparas2 = new double[numberoptparas];
                        for (int i = 0; i < numberindividuums; i++)
                        {
                            //Zufälligen parent wählen und kopieren
                            selectedind = rand.Next(0, genpool_input.Length - 1);
                            selectedind2 = rand.Next(0, genpool_input.Length - 1);
                            while (selectedind == selectedind2)
                            {
                                selectedind2 = rand.Next(0, genpool_input.Length - 1);
                            }
                            recombinated_optparas = genpool_input[selectedind].get_optparas();
                            recombinated_optparas2 = genpool_input[selectedind2].get_optparas();
                            //Zufällige Rekombination
                            for (int j = 0; j < numberoptparas; j++)
                            {
                                if (rand.Next(-10, 10) > 0) recombinated_optparas[j] = recombinated_optparas2[j];
                            }
                            //Zurückspeichern
                            new_generation_input[startindex + i] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuum_id, numberoptparas);
                            new_generation_input[startindex + i].set_optparas(recombinated_optparas);
                            new_generation_input[startindex + i].set_status("raw");
                            new_generation_input[startindex + i].set_generator(algo_id);
                            individuum_id++;
                        }
                        break;
                    }
                #endregion

                #region Intermediäre Rekombination: zwei Parents rekombinieren sich im Verhältnis eines Parameters
                case "Intermediäre Rekombination":
                    {
                        applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with " + algo_id + ":'" + algofeedbackarray[algo_id].name + "'...done");

                        int selectedind;
                        int selectedind2;

                        double[] recombinated_optparas = new double[numberoptparas];
                        double[] recombinated_optparas2 = new double[numberoptparas];
                        for (int i = 0; i < numberindividuums; i++)
                        {
                            //Zufälligen parent wählen und kopieren
                            selectedind = rand.Next(0, genpool_input.Length - 1);
                            selectedind2 = rand.Next(0, genpool_input.Length - 1);
                            while (selectedind == selectedind2)
                            {
                                selectedind2 = rand.Next(0, genpool_input.Length - 1);
                            }
                            recombinated_optparas = genpool_input[selectedind].get_optparas();
                            recombinated_optparas2 = genpool_input[selectedind2].get_optparas();
                            //Rekombination
                            for (int j = 0; j < numberoptparas; j++)
                            {
                                recombinated_optparas[j] += ((rand.Next(0, 1500) / 1000) - 0.25) * (recombinated_optparas2[j] - recombinated_optparas[j]);
                                if (recombinated_optparas[j] > genpool_input[1].OptParameter[j].Max) recombinated_optparas[j] = genpool_input[1].OptParameter[j].Max;
                                if (recombinated_optparas[j] < genpool_input[1].OptParameter[j].Min) recombinated_optparas[j] = genpool_input[1].OptParameter[j].Min;
                            }
                            //Zurückspeichern
                            new_generation_input[startindex + i] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuum_id, numberoptparas);
                            new_generation_input[startindex + i].set_optparas(recombinated_optparas);
                            new_generation_input[startindex + i].set_status("raw");
                            new_generation_input[startindex + i].set_generator(algo_id);
                            individuum_id++;
                        }
                        break;
                    }
                #endregion

                #region Diversität aus Sortierung: Differenzvektorbestimmung und Addierung am Rande der nach zufälligem Kriterium sortierten Generation
                case "Diversität aus Sortierung":
                    {
                        applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with " + algo_id + ":'" + algofeedbackarray[algo_id].name + "'...done");

                        double[] diversity_optparas = new double[numberoptparas];
                        double mult1 = 1;

                        for (int i = 0; i < numberindividuums; i++)
                        {
                            //Am Rand mit geringen Optpara-Werten erzeugen
                            if (i % 2 == 0)
                            {
                                for (int k = 0; k < genpool_input[0].get_optparas().Length; k++)
                                {
                                    diversity_optparas[k] = genpool_input[0].get_optparas()[k] + mult1 * (genpool_input[0].get_optparas()[k] - genpool_input[(((int)i / 2) + 1) % genpool_input.Length].get_optparas()[k]);
                                    //Auf Max prüfen, ggf. Multiplikator für den Optpara anpassen anpassen
                                    if ((genpool_input[0].OptParameter[k].Max < diversity_optparas[k]) || (genpool_input[0].OptParameter[k].Min > diversity_optparas[k]))
                                    {
                                        mult1 *= 0.5;
                                        k--;
                                    }
                                    else mult1 = 1;
                                }
                            }
                            // Am Rand mit hohen Optpara-Werten erzeugen
                            else
                            {
                                for (int k = 0; k < genpool_input[0].get_optparas().Length; k++)
                                {
                                    diversity_optparas[k] = genpool_input[genpool_input.Length - 1].get_optparas()[k];
                                    diversity_optparas[k] += mult1 * (genpool_input[genpool_input.Length - 1].get_optparas()[k] - genpool_input[(genpool_input.Length - 2) - ((int)i / 2 % (genpool_input.Length - 2))].get_optparas()[k]);
                                    //Auf Min prüfen, ggf. Multiplikator für den Optpara anpassen anpassen
                                    if ((genpool_input[0].OptParameter[k].Min > diversity_optparas[k]) || (genpool_input[0].OptParameter[k].Max < diversity_optparas[k]))
                                    {
                                        mult1 *= 0.5;
                                        k--;
                                    }
                                    else mult1 = 1;
                                }
                            }
                            //Zurückspeichern
                            new_generation_input[startindex + i] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuum_id, numberoptparas);
                            new_generation_input[startindex + i].set_optparas(diversity_optparas);
                            new_generation_input[startindex + i].set_status("raw");
                            new_generation_input[startindex + i].set_generator(algo_id);
                            individuum_id++;
                        }
                        break;
                    }
                #endregion

                #region Totaler Zufall: Alle Optparameter entstehen durch zufällige Wahl
                case "Totaler Zufall":
                    {
                        applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with " + algo_id + ":'" + algofeedbackarray[algo_id].name + "'...done");

                        double[] random_array;

                        //Für jedes Individuum durchgehen
                        for (int k = 0; k < numberindividuums; k++)
                        {
                            random_array = new double[numberoptparas];
                            //Für jeden Parameter durchgehen
                            for (int j = 0; j < numberoptparas; j++)
                            {
                                double max = genpool_input[0].OptParameter[j].Max;
                                double min = genpool_input[0].OptParameter[j].Min;
                                random_array[j] = min + (max - min) * ((double)rand.Next(0, 1000) / 1000);
                            }
                            //Zurückspeichern
                            new_generation_input[startindex + k] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuum_id, numberoptparas);
                            new_generation_input[startindex + k].set_optparas(random_array);
                            new_generation_input[startindex + k].set_status("raw");
                            new_generation_input[startindex + k].set_generator(algo_id);
                            individuum_id++;
                        }
                        break;
                    }
                #endregion

                #region Dominanzvektor: Differenzvektor eines dominierten und eines dominanten Individuums auf ein dominantes Individuum addieren
                case "Dominanzvektor":
                    {
                        applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with " + algo_id + ":'" + algofeedbackarray[algo_id].name + "'...done");

                        double[] dominanz = new double[numberoptparas];
                        int dominator = -1;
                        double multiplikator = 1;
                        int pointer_parent = 0;  //Um kein Individuum doppelt zu berechnen
                        bool generate_new = false;

                        //Für jedes Individuum durchgehen
                        for (int k = 0; k < numberindividuums; k++)
                        {
                            //In der ersten Runde hat Wastepool noch keinen Inhalt; Kopien erzeugen 
                            dominanz = genpool_input[k % genpool_input.Length].get_optparas();
                            if (wastepool_input[0] != null)
                            {
                                generate_new = true;
                                //Den Wastepool durchgehen
                                for (int j = pointer_parent; j < wastepool_input.Length; j++)
                                {
                                    if (wastepool_input[j].get_status_reason() == "dominated")
                                    {
                                        dominator = wastepool_input[j].get_status_opponent();
                                        for (int i = 0; i < genpool_input.Length; i++)
                                        {
                                            //Den Dominator im Genpool suchen der das Individuum im Wastepool dominierte
                                            if (genpool_input[i].ID == dominator)
                                            {
                                                //Den neuen Optparas-Vektor bestimmen
                                                for (int h = 0; h < numberoptparas; h++)
                                                {
                                                    dominanz[h] = genpool_input[i].get_optparas()[h] + (multiplikator * (genpool_input[i].get_optparas()[h] - wastepool_input[j].get_optparas()[h]));
                                                    //Falls die Grenzen überschritten sind
                                                    if ((dominanz[h] > genpool_input[0].OptParameter[h].Max) || (dominanz[h] < genpool_input[0].OptParameter[h].Min))
                                                    {
                                                        multiplikator *= 0.5;
                                                        h--;
                                                    }
                                                    else
                                                    {
                                                        multiplikator = 1;
                                                    }
                                                }
                                                generate_new = false;
                                                break;
                                            }
                                        }
                                        pointer_parent++;
                                        //Neue dominanz-Optparas wurden generiert und können gespeichert werden
                                        if (generate_new == false) break;
                                    }
                                }
                            }

                            //Zurückspeichern
                            new_generation_input[startindex + k] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuum_id, numberoptparas);
                            new_generation_input[startindex + k].set_optparas(dominanz);
                            new_generation_input[startindex + k].set_status("raw");
                            new_generation_input[startindex + k].set_generator(algo_id);
                            individuum_id++;
                        }
                        break;
                    }
                #endregion
                

                //Lokale Algorithmen
                #region Hook and Jeeves: Optparameter werden nacheinander variiert und zwei neue Individuen erzeugt
                case "Hook and Jeeves":  //VGL: Syrjakow S.95f
                    {
                        numberindividuums = (int)((double)numberindividuums * ((double)genpool_input.Length / (double)new_generation_input.Length));
                        applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with " + algo_id + ":'" + algofeedbackarray[algo_id].name + "'...done");
                        //Feedbackdate Pro Basis-Individuum: 0:[Gewichtungsparameter für die Zielfunktionen]1:[Tast-Schrittweiten]2:[0:Zustand,1:zu variierender optparameter,2:schon gelaufen,3:Minimumschrittweiten-Mult]
                        //Gewichtungsparameter für die Zielfunktionen [penalties]
                        //Tast-Schrittweiten [optparameter]
                        //Zustand [1]
                        //zu variierender optparameter [1]
                        //Schon gelaufen [1]

                        bool run;
                        bool firstrun = true;
                        int nextstate = 0;
                        int countready = 0;
                        double[,] weightsinfo = new double[2,numberpenalties];  //0:[min]1:[max] eines jeden optparameters
                        double[] Penalties_tmp;

                        if (firstrun)
                        {
                            //Vorbereitung für Wichtungsberechnung der einzelnen Funktionen 
                            applog.appendText("Algos: Hook and Jeeves: Starting precalculation of weights");
                            for (int i = 0; i < numberpenalties; i++) //Min,Max vorbelegen
                            {
                                weightsinfo[0, i] = genpool_input[0].Penalties[i];
                                weightsinfo[1, i] = weightsinfo[0, i];
                            }
                            for (int i = 1; i < genpool_input.Length; i++)  //Restlichen genpool durchforsten
                            {
                                Penalties_tmp = genpool_input[i].Penalties;
                                for (int j = 0; j < numberpenalties; j++) //Min,Max eines jeden Penalties suchen
                                {
                                    if (weightsinfo[0, j] > Penalties_tmp[j]) weightsinfo[0, j] = Penalties_tmp[j]; //Min
                                    if (weightsinfo[1, j] < Penalties_tmp[j]) weightsinfo[1, j] = Penalties_tmp[j]; //Max
                                }
                            }
                            firstrun = false;
                        }

                        applog.appendText("Algos: Hook and Jeeves: Starting Calculation of " + numberindividuums + " Processes");
                        for (int i = 0; i < numberindividuums; i++)
                        {
                            //Standardmässig nicht simulieren
                            new_generation_input[i * 3].set_toSimulate(false);
                            new_generation_input[i * 3 + 1].set_toSimulate(false);
                            new_generation_input[i * 3 + 2].set_toSimulate(false);
                            //Zustand auslesen
                            //-1: Schrittweiten definieren
                            //0: Tast0
                            //1: Tast1: Vergleich -Opt
                            //2: Tast2
                            //3: Tast3: Vergleich +Opt
                            //4: Tast4
                            //5: Tast5
                            //10: Vergleich der Funktionswerte
                            //20: Extrapolation
                            //30: Extrapolation zurück
                            //40: Schrittweite halbieren oder Goal
                            //50: Goal
                            if (genpool_input[i].get_generator() == algo_id) nextstate = (int)genpool_input[i].feedbackdata[2, 0];
                            else nextstate = -1;
                            
                            run = true;

                            while(run)
                            {
                                applog.appendText("Algos: Hook and Jeeves: BaseIndividuum "+ genpool_input[i].ID + ": state: " + nextstate);
                                switch (nextstate)
                                {
                                    case (-1): //-1: Schrittweiten definieren
                                        {
                                            new_generation_input[i * 3] = genpool_input[i].Clone_MetaEvo();
                                            new_generation_input[i * 3].ID = individuum_id;
                                            individuum_id++;
                                            //Schrittweiten definieren
                                            genpool_input[i].feedbackdata = new double[3, Math.Max(genpool_input[0].Penalties.Length, Math.Max(numberoptparas, 3))];
                                            for (int j = 0; j < numberoptparas; j++)
                                            {
                                                //Gewichtung berechnen
                                                genpool_input[i].feedbackdata[0, j] = 0;
                                                if (j < numberpenalties)
                                                {
                                                    for (int k = 0; k < numberpenalties; k++)
                                                    {
                                                        genpool_input[i].feedbackdata[0, j] += (weightsinfo[1, k] - genpool_input[i].Penalties[k]) / (weightsinfo[1, k] - weightsinfo[0, k]);
                                                    }
                                                    if (genpool_input[i].feedbackdata[0, j] == 0) genpool_input[i].feedbackdata[0, j] = 0.5;
                                                    else genpool_input[i].feedbackdata[0, j] = ((weightsinfo[1, j] - genpool_input[i].Penalties[j]) / (weightsinfo[1, j] - weightsinfo[0, j])) / genpool_input[i].feedbackdata[0, j];
                                                    applog.appendText("Algos: Hook and Jeeves: BaseIndividuum " + genpool_input[i].ID + ": Wichtung Penalty[" + j + "]: " + genpool_input[i].feedbackdata[0, j]);
                                                }
                                                //Initiale Tastschrittweite
                                                genpool_input[i].feedbackdata[1, j] = (genpool_input[i].OptParameter[j].Max - genpool_input[i].OptParameter[j].Min) / (double)numberindividuums;
                                            }
                                            genpool_input[i].feedbackdata[2, 0] = 0;//Zustand [1]
                                            genpool_input[i].feedbackdata[2, 1] = 0;//zu variierender optparameter [1]
                                            genpool_input[i].feedbackdata[2, 2] = 0;//Schon gelaufen [1]
                                            genpool_input[i].set_generator(algo_id);
                                            nextstate = 0;
                                            break;
                                        }
                                    case (0): //0: Tast0
                                        {
                                            double[] mutated_optparas = new_generation_input[i * 3].get_optparas();
                                            mutated_optparas[(int)genpool_input[i].feedbackdata[2, 1]] -= genpool_input[i].feedbackdata[1, (int)genpool_input[i].feedbackdata[2, 1]];
                                            //Grenzen einhalten
                                            if (mutated_optparas[(int)genpool_input[i].feedbackdata[2, 1]] < genpool_input[0].OptParameter[(int)genpool_input[i].feedbackdata[2, 1]].Min) mutated_optparas[(int)genpool_input[i].feedbackdata[2, 1]] = genpool_input[0].OptParameter[(int)genpool_input[i].feedbackdata[2, 1]].Min;
                                            if (mutated_optparas[(int)genpool_input[i].feedbackdata[2, 1]] > genpool_input[0].OptParameter[(int)genpool_input[i].feedbackdata[2, 1]].Max) mutated_optparas[(int)genpool_input[i].feedbackdata[2, 1]] = genpool_input[0].OptParameter[(int)genpool_input[i].feedbackdata[2, 1]].Max;
                                            
                                            new_generation_input[i * 3 + 1] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuum_id, numberoptparas);
                                            new_generation_input[i * 3 + 1].set_optparas(mutated_optparas);
                                            new_generation_input[i * 3 + 1].set_status("raw");
                                            new_generation_input[i * 3 + 1].set_generator(algo_id);
                                            individuum_id++;
                                            applog.appendText("Algos: Hook and Jeeves: BaseIndividuum " + genpool_input[i].ID + ": BestIndividuum " + new_generation_input[i * 3].ID + " -> " + new_generation_input[i * 3 + 1].ID + " Mutate optparas[" + (int)genpool_input[i].feedbackdata[2, 1] + "] from " + new_generation_input[i * 3].get_optparas()[(int)genpool_input[i].feedbackdata[2, 1]] + " to " + mutated_optparas[(int)genpool_input[i].feedbackdata[2, 1]] + " (- Stepsize: " + genpool_input[i].feedbackdata[1, (int)genpool_input[i].feedbackdata[2, 1]] + ")");
                                            run = false;
                                            nextstate = 1;
                                            break;
                                        }
                                    case (1): //1: Tast1: Vergleich -Opt 
                                        {
                                            if (new_generation_input[i * 3 + 1].get_status() == "true")
                                            {
                                                double[] new_penalties = new_generation_input[i * 3].Penalties;
                                                double[] new2_penalties = new_generation_input[i * 3 + 1].Penalties;
                                                double weighted_penaltie_new = 0;  //(tatsächlich Fehlergrösse)
                                                double weighted_penaltie_new2 = 0; //(tatsächlich Fehlergrösse)

                                                for (int j = 0; j < numberpenalties; j++)
                                                {
                                                    weighted_penaltie_new += new_penalties[j] * genpool_input[i].feedbackdata[0, j];
                                                    weighted_penaltie_new2 += new2_penalties[j] * genpool_input[i].feedbackdata[0, j];
                                                }
                                                applog.appendText("Algos: Hook and Jeeves: BaseIndividuum " + genpool_input[i].ID + ": WightedPenalties: BestIndividuum " + new_generation_input[i * 3].ID + " = " + weighted_penaltie_new + " / NewIndividuum " + new_generation_input[i * 3 + 1].ID + " = " + weighted_penaltie_new2);
                                                if (weighted_penaltie_new2 < weighted_penaltie_new) { nextstate = 4; break; }
                                            }
                                            nextstate = 2;
                                            break;
                                        }
                                    case (2): //2: Tast2
                                        {
                                            double[] mutated_optparas = new_generation_input[i * 3].get_optparas();
                                            mutated_optparas[(int)genpool_input[i].feedbackdata[2, 1]] += genpool_input[i].feedbackdata[1, (int)genpool_input[i].feedbackdata[2, 1]];
                                            //Grenzen einhalten
                                            if (mutated_optparas[(int)genpool_input[i].feedbackdata[2, 1]] < genpool_input[0].OptParameter[(int)genpool_input[i].feedbackdata[2, 1]].Min) mutated_optparas[(int)genpool_input[i].feedbackdata[2, 1]] = genpool_input[0].OptParameter[(int)genpool_input[i].feedbackdata[2, 1]].Min;
                                            if (mutated_optparas[(int)genpool_input[i].feedbackdata[2, 1]] > genpool_input[0].OptParameter[(int)genpool_input[i].feedbackdata[2, 1]].Max) mutated_optparas[(int)genpool_input[i].feedbackdata[2, 1]] = genpool_input[0].OptParameter[(int)genpool_input[i].feedbackdata[2, 1]].Max;
                                            
                                            new_generation_input[i * 3 + 1] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuum_id, numberoptparas);
                                            new_generation_input[i * 3 + 1].set_optparas(mutated_optparas);
                                            new_generation_input[i * 3 + 1].set_status("raw");
                                            new_generation_input[i * 3 + 1].set_generator(algo_id);
                                            individuum_id++;
                                            applog.appendText("Algos: Hook and Jeeves: BaseIndividuum " + genpool_input[i].ID + ": BestIndividuum " + new_generation_input[i * 3].ID + " Mutate optparas[" + (int)genpool_input[i].feedbackdata[2, 1] + "] from " + new_generation_input[i * 3].get_optparas()[(int)genpool_input[i].feedbackdata[2, 1]] + " to " + mutated_optparas[(int)genpool_input[i].feedbackdata[2, 1]] + " (+ Stepsize: " + genpool_input[i].feedbackdata[1, (int)genpool_input[i].feedbackdata[2, 1]] + ")");
                                            run = false;
                                            genpool_input[i].feedbackdata[1,(int)genpool_input[i].feedbackdata[2, 1]] *= -1; //Schrittweite negieren
                                            nextstate = 3;
                                            break;
                                        }
                                    case (3): //3: Tast3: Vergleich +Opt
                                        {
                                            if (new_generation_input[i * 3 + 1].get_status() == "true")
                                            {
                                                double[] new_penalties = new_generation_input[i * 3].Penalties;
                                                double[] new2_penalties = new_generation_input[i * 3 + 1].Penalties;
                                                double weighted_penaltie_new = 0;  //(tatsächlich Fehlergrösse)
                                                double weighted_penaltie_new2 = 0; //(tatsächlich Fehlergrösse)

                                                for (int j = 0; j < numberpenalties; j++)
                                                {
                                                    weighted_penaltie_new += new_penalties[j] * genpool_input[i].feedbackdata[0, j];
                                                    weighted_penaltie_new2 += new2_penalties[j] * genpool_input[i].feedbackdata[0, j];
                                                }
                                                applog.appendText("Algos: Hook and Jeeves: BaseIndividuum " + genpool_input[i].ID + ": WightedPenalties: BestIndividuum " + new_generation_input[i * 3].ID + " = " + weighted_penaltie_new + " / NewIndividuum " + new_generation_input[i * 3 + 1].ID + " = " + weighted_penaltie_new2);
                                                if (weighted_penaltie_new2 < weighted_penaltie_new) { nextstate = 4; break; }
                                            }
                                            nextstate = 5;
                                            break;
                                        }
                                    case (4): //4: Tast4
                                        { 
                                            new_generation_input[i * 3] = new_generation_input[i * 3 + 1].Clone_MetaEvo();
                                            new_generation_input[i * 3].set_toSimulate(false);
                                            applog.appendText("Algos: Hook and Jeeves: BaseIndividuum " + genpool_input[i].ID + ": Neues BestIndividuum: " + new_generation_input[i * 3].ID);
                                            nextstate = 5;
                                            break;
                                        }
                                    case (5): //5: Tast5
                                        {
                                            if (genpool_input[i].feedbackdata[2, 1] < numberoptparas - 1)
                                            {
                                                genpool_input[i].feedbackdata[2, 1]++;
                                                nextstate = 0;
                                                applog.appendText("Algos: Hook and Jeeves: BaseIndividuum " + genpool_input[i].ID + ": Optparameter " + (genpool_input[i].feedbackdata[2, 1]) + " von " + numberoptparas + " mit der vorgegebenen Schrittweite getestet");  
                                            }
                                            else
                                            {
                                                applog.appendText("Algos: Hook and Jeeves: BaseIndividuum " + genpool_input[i].ID + ": Optparameter " + numberoptparas + " von " + numberoptparas + " mit der vorgegebenen Schrittweite getestet");  
                                                genpool_input[i].feedbackdata[2, 1] = 0;//zu variierender optparameter [1]
                                                nextstate = 10;
                                            }
                                            break;
                                        }
                                    case (10): //10: Vergleich der Funktionswerte
                                        {
                                            double[] gen_penalties = genpool_input[i].Penalties;
                                            double[] new_penalties = new_generation_input[i * 3].Penalties;
                                            double weighted_penaltie_gen = 0;  //(tatsächlich Fehlergrösse)
                                            double weighted_penaltie_new = 0; //(tatsächlich Fehlergrösse)

                                            for (int j = 0; j < numberpenalties; j++)
                                            {
                                                weighted_penaltie_gen += gen_penalties[j] * genpool_input[i].feedbackdata[0, j];
                                                weighted_penaltie_new += new_penalties[j] * genpool_input[i].feedbackdata[0, j];
                                            }
                                            if (weighted_penaltie_new < weighted_penaltie_gen)
                                            {
                                                applog.appendText("Algos: Hook and Jeeves: BaseIndividuum " + genpool_input[i].ID + ": Neues BestIndividuum " + new_generation_input[i * 3].ID + " Penaltie: " + weighted_penaltie_new + " ist besser als Basisindividuum " + genpool_input[i].ID + " Penaltie: " + weighted_penaltie_gen);
                                                nextstate = 20;
                                            }
                                            else
                                            {
                                                applog.appendText("Algos: Hook and Jeeves: BaseIndividuum " + genpool_input[i].ID + ": Neues BestIndividuum " + new_generation_input[i * 3].ID + " Penaltie: " + weighted_penaltie_new + " ist NICHT besser als Basisindividuum " + genpool_input[i].ID + " Penaltie: " + weighted_penaltie_gen);
                                                nextstate = 30;
                                            }
                                            break;
                                        }
                                    case (20): //20: Extrapolation
                                        {
                                            double[] extrapolation = new double[numberoptparas];
                                            double[,] feedbackdata_tmp;
                                            string tmp = "(/";

                                            for (int j = 0; j < numberoptparas; j++)
                                            {
                                                //Extrapolationsvektor zusammenstellen
                                                extrapolation[j] = (2*new_generation_input[i * 3].get_optparas()[j]) - genpool_input[i].get_optparas()[j];
                                                //Grenzen einhalten
                                                if (extrapolation[j] < genpool_input[0].OptParameter[j].Min) extrapolation[j] = genpool_input[0].OptParameter[j].Min;
                                                if (extrapolation[j] > genpool_input[0].OptParameter[j].Max) extrapolation[j] = genpool_input[0].OptParameter[j].Max;
                                                tmp = tmp + " " + extrapolation[j] + "/";
                                            }
                                            applog.appendText("Algos: Hook and Jeeves: BaseIndividuum " + genpool_input[i].ID + ": Neues BestIndividuum "+individuum_id+" mit Extrapolationsschritt auf " + tmp + ")");
                                            //Kopieren des Individuums in den Genpool
                                            feedbackdata_tmp = genpool_input[i].feedbackdata;
                                            genpool_input[i] = new_generation_input[i * 3].Clone_MetaEvo();
                                            genpool_input[i].feedbackdata = feedbackdata_tmp;
                                            genpool_input[i].feedbackdata[2, 2] = 1;//Schon gelaufen [1]
                                            
                                            //Extrapolation auf Satelliten-Individuum anwenden
                                            new_generation_input[i * 3] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuum_id, numberoptparas);
                                            new_generation_input[i * 3].set_optparas(extrapolation);
                                            new_generation_input[i * 3].set_status("raw");
                                            new_generation_input[i * 3].set_generator(algo_id);
                                            individuum_id++;
                                            nextstate = 0;
                                            break;
                                        }
                                    case (30): //30: Extrapolation zurück
                                        {
                                            if (genpool_input[i].feedbackdata[2, 2] == 1)
                                            {
                                                applog.appendText("Algos: Hook and Jeeves: BaseIndividuum " + genpool_input[i].ID + ": Extrapolation zurückgehen");
                                                new_generation_input[i * 3] = genpool_input[i].Clone_MetaEvo();
                                                new_generation_input[i * 3].ID = individuum_id;
                                                individuum_id++;
                                                genpool_input[i].feedbackdata[2, 2] = 0;
                                                nextstate = 0;
                                            }
                                            else nextstate = 40;
                                            break;
                                        }
                                    case (40): //40: Schrittweite halbieren oder Goal
                                        {
                                            int finished = 0; //Wieviele Penalties fein genug bestimmt wurden

                                            for (int j = 0; j < numberoptparas; j++)
                                            {
                                                if (Math.Abs(genpool_input[i].feedbackdata[1, j]) < Math.Abs(genpool_input[0].OptParameter[j].Max - genpool_input[0].OptParameter[j].Min) / HJ_minimumstepsize) finished++;  
                                                genpool_input[i].feedbackdata[1, j] *= 0.5;
                                                applog.appendText("Algos: Hook and Jeeves: BaseIndividuum " + genpool_input[i].ID + ": Schrittweite[" + j + "] von " + genpool_input[i].feedbackdata[1, j] * 2 + " auf " + genpool_input[i].feedbackdata[1, j]);
                                            }
                                            if (finished == numberoptparas) nextstate = 50;
                                            else nextstate = 0;
                                            break;
                                        }
                                    case (50): //50: Goal
                                        {
                                            countready++;
                                            run = false;
                                            break;
                                        }
                                }
                            }
                            genpool_input[i].feedbackdata[2, 0] = nextstate;
                        }
                        //Alle Hook and Jeeves Prozesse fertig
                        if (countready == numberindividuums)
                        {
                            algofeedbackarray[algo_id].number_individuals_for_nextGen = 0;
                            run = false;
                        }
                        break;
                    }
                #endregion

                #region default: Error Message anzeigen
                default:
                    MessageBox.Show("Algorithmus '" + algofeedbackarray[algo_id].name + "' ist nicht bekannt!","Algomanager");
                    break;
                #endregion
            }
        }
    }
}
