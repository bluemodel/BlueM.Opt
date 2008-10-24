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
        EVO.Diagramm.ApplicationLog applog;

        public Algos(ref EVO.Common.EVO_Settings settings_input, int individuum_id_input, ref EVO.Diagramm.ApplicationLog applog_input)
        {
            individuum_id = individuum_id_input;
            applog = applog_input;
            ChildsPerParent = settings_input.MetaEvo.ChildsPerParent;
        }

        //Legt fest welche Algorithmen genutzt werden sollen
        public void set_algos(string algos2use_input) 
        {
            string[] tmp;

            tmp = algos2use_input.Split(',', ';');
            algofeedbackarray = new Algofeedback[tmp.Length];

            for (int i = 0; i < tmp.Length; i++)
            {
                algofeedbackarray[i] = new Algofeedback(tmp[i].Trim(), (individuum_id * ChildsPerParent) / algofeedbackarray.Length);
                
            }
            applog.appendText("Algos: Using Algos " + algos2use_input);
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
                #endregion

                #region Feedback Mutation: Mutiert mit bewährtem Mutationsoperator und verbessert diesen
                case "Feedback Mutation":
                    //Feedbackdata: Pro Individuum: [[Mutationsparameter]] 
                    //Ein Mutationsparameter kann pro runde maximal um 10% verändert werden

                     applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with "+algo_id+":'" + algofeedbackarray[algo_id].name + "'...done");

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
                #endregion

                #region Ungleichverteilte Mutation: Mutiert an einer zufälligen Stelle (grössere Mutationen sind unwahrscheinlicher)
                case "Ungleichverteilte Mutation":
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
                        random = ((rand.Next(-1000,1000))/1000)*1.15;
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
                #endregion

                #region Zufällige Rekombination: Die Werte zweier zufälliger Eltern werden zufällig rekombiniert
                case "Zufällige Rekombination":
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
                #endregion

                //ToDo: Intermediäre Rekombination (S.60, S.75 Muschalla)

                #region Diversität aus Sortierung: Differenzvektorbestimmung und Addierung am Rande der nach zufälligem Kriterium sortierten Generation
                case "Diversität aus Sortierung":
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
                                diversity_optparas[k] = genpool_input[0].get_optparas()[k] + mult1*(genpool_input[0].get_optparas()[k] - genpool_input[(((int)i/2)+1)%genpool_input.Length].get_optparas()[k]);
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
                                diversity_optparas[k] = genpool_input[genpool_input.Length - 1].get_optparas()[k] + mult1 * (genpool_input[genpool_input.Length - 1].get_optparas()[k] - genpool_input[(genpool_input.Length - 2 - (int)i / 2)%genpool_input.Length].get_optparas()[k]);
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
                #endregion

                #region Totaler Zufall: Alle Optparameter entstehen durch zufällige Wahl
                case "Totaler Zufall":
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
                #endregion

                #region Dominanzvektor: Differenzvektor eines dominierten und eines dominanten Individuums auf ein dominantes Individuum addieren
                case "Dominanzvektor":
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
                #endregion

                #region Hook and Jeeves V1: Optparameter werden nacheinander variiert und zwei neue Individuen erzeugt
                case "Hook and Jeeves V1":
                     applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with " + algo_id + ":'" + algofeedbackarray[algo_id].name + "'...done");
                    //Statusparameter im Feedbackarray:
                    //Doublearray: Pro Individuum: [Optparameter][Schrittweite, Schrittweitenteilungen in Parent und Child, Anzahl Kinder in parent]

                    double[] haj = new double[numberoptparas];
                    double[] haj2 = new double[numberoptparas];
                    bool construct_new = true;
                    pointer_parent = 0;
                    int pointer2 = 0;
                    int pointer_optpara = 0;
                    startindex = startindex + numberindividuums;

                    //Falls Individuen erzeugt werden sollen
                    if (numberindividuums > 0)
                    {
                        //Genpool durchsuchen
                        while ((numberindividuums > 0) && (pointer_parent < genpool_input.Length))
                        {
                            //Suchen ob ein Individuum aus einem Hook and Jeeves Prozess überlebt hat
                            if (genpool_input[pointer_parent].get_generator() == algo_id)
                            {
                                //Suchen ob von diesem Parent nur eine einzige Individuum-Variante erzeugt wurde  
                                for (int i = 0; i < numberoptparas; i++) 
                                {
                                    if (genpool_input[pointer_parent].feedbackdata[i,2] == 1) 
                                    {
                                        //im parent vermerken
                                        genpool_input[pointer_parent].feedbackdata[i, 2] = 2;

                                        haj = genpool_input[pointer_parent].get_optparas();
                                        haj[i] += genpool_input[pointer_parent].feedbackdata[i, 0];
                                        if (haj[i] > genpool_input[pointer_parent].OptParameter[i].Max) haj[i] = genpool_input[pointer_parent].OptParameter[i].Max;

                                        //Zurückspeichern
                                        new_generation_input[startindex - numberindividuums] = genpool_input[pointer_parent].Clone_MetaEvo();
                                        new_generation_input[startindex - numberindividuums].ID = individuum_id;
                                        new_generation_input[startindex - numberindividuums].set_optparas(haj);
                                        new_generation_input[startindex - numberindividuums].set_status("raw");
                                        new_generation_input[startindex - numberindividuums].set_generator(algo_id);
                                        //Feedbackdaten setzen
                                        new_generation_input[startindex - numberindividuums].feedbackdata[i, 0] = genpool_input[pointer_parent].feedbackdata[i, 0] / 2;
                                        new_generation_input[startindex - numberindividuums].feedbackdata[i, 1] = genpool_input[pointer_parent].feedbackdata[i, 1] + 1;
                                        new_generation_input[startindex - numberindividuums].feedbackdata[i, 2] = 0;

                                        individuum_id++;
                                        numberindividuums--;
                                        construct_new = false;
                                        break;
                                    }
                                }

                                //Sonst neue Individuen mit neuen Feedbackdaten erstellen
                                if (construct_new) 
                                {
                                    //Schrittweitenteilung durchgehen (wie oft ein Optparameter schon als Individuums-Basis verändert wurde) niedrigsten mit pointer2 ausstatten
                                    for (int i = 1; i < numberoptparas; i++)
                                    {
                                        if (genpool_input[pointer_parent].feedbackdata[i, 1] < genpool_input[pointer_parent].feedbackdata[pointer2, 1]) pointer2 = i;
                                    }
				                    //Zufälligen der am wenigsten benutzten Optparameter auswählen
                                    while (genpool_input[pointer_parent].feedbackdata[pointer_optpara, 1] > genpool_input[pointer_parent].feedbackdata[pointer2, 1]) {
                                        pointer_optpara = rand.Next(0, numberoptparas - 1); //pointer_optpara zeigt auf random Optpara
                                    }
                    				
				                    //Individuum erstellen
                                    genpool_input[pointer_parent].feedbackdata[pointer_optpara, 2] = 1;

                                    haj = genpool_input[pointer_parent].get_optparas();
                                    haj[pointer_optpara] -= genpool_input[pointer_parent].feedbackdata[pointer_optpara, 0];
                                    if (haj[pointer_optpara] < genpool_input[pointer_parent].OptParameter[pointer_optpara].Min) haj[pointer_optpara] = genpool_input[pointer_parent].OptParameter[pointer_optpara].Min;

                                    //Zurückspeichern 
                                    new_generation_input[startindex - numberindividuums] = genpool_input[pointer_parent].Clone_MetaEvo();
                                    new_generation_input[startindex - numberindividuums].ID = individuum_id;
                                    new_generation_input[startindex - numberindividuums].set_optparas(haj);
                                    new_generation_input[startindex - numberindividuums].set_status("raw");
                                    new_generation_input[startindex - numberindividuums].set_generator(algo_id);
                                    //Feedbackdaten setzen
                                    new_generation_input[startindex - numberindividuums].feedbackdata[pointer_optpara, 0] = genpool_input[pointer_parent].feedbackdata[pointer_optpara, 0] / 2;
                                    new_generation_input[startindex - numberindividuums].feedbackdata[pointer_optpara, 1] = genpool_input[pointer_parent].feedbackdata[pointer_optpara, 1] + 1;
                                    new_generation_input[startindex - numberindividuums].feedbackdata[pointer_optpara, 2] = 0;

                                    individuum_id++;
                                    numberindividuums--;

                                    //Falls erlaubt, zweites Individuen erstellen 
                                    if (numberindividuums > 0)
                                    {
                                        genpool_input[pointer_parent].feedbackdata[pointer_optpara, 2] = 2;
                                        haj2 = genpool_input[pointer_parent].get_optparas();
                                        haj2[pointer_optpara] += genpool_input[pointer_parent].feedbackdata[pointer_optpara, 0];
                                        if (haj2[pointer_optpara] > genpool_input[pointer_parent].OptParameter[pointer_optpara].Max) haj[pointer_optpara] = genpool_input[pointer_parent].OptParameter[pointer_optpara].Max;

                                        //Zurückspeichern
                                        new_generation_input[startindex - numberindividuums] = genpool_input[pointer_parent].Clone_MetaEvo();
                                        new_generation_input[startindex - numberindividuums].ID = individuum_id;
                                        new_generation_input[startindex - numberindividuums].set_optparas(haj2);
                                        new_generation_input[startindex - numberindividuums].set_status("raw");
                                        new_generation_input[startindex - numberindividuums].set_generator(algo_id);
                                        //Feedbackdaten setzen
                                        new_generation_input[startindex - numberindividuums].feedbackdata[pointer_optpara, 0] = genpool_input[pointer_parent].feedbackdata[pointer_optpara, 0] / 2;
                                        new_generation_input[startindex - numberindividuums].feedbackdata[pointer_optpara, 1] = genpool_input[pointer_parent].feedbackdata[pointer_optpara, 1] + 1;
                                        new_generation_input[startindex - numberindividuums].feedbackdata[pointer_optpara, 2] = 0;

                                        individuum_id++;
                                        numberindividuums--;
                                    }   
                                }
                                construct_new = true;
                            }
                            pointer_parent++;
                        }

                        //Falls noch Individuen erzeugt werden dürfen (neuen Hook and Jeeves Prozess starten)
                        while (numberindividuums > 0)
                        {
                            //Zufälliges Individuum als Startpunkt wählen was noch keinen H&J-Prozess besitzt
                            pointer_parent = rand.Next(0, genpool_input.Length - 1);
                            while (genpool_input[pointer_parent].get_generator() == 5)
                            {
                                pointer_parent = rand.Next(0, genpool_input.Length - 1);
                            }
                            haj = genpool_input[pointer_parent].get_optparas();

                            //Parent-Individuum modifizieren
                            genpool_input[pointer_parent].feedbackdata = new double[numberoptparas, 3];
                            genpool_input[pointer_parent].feedbackdata[pointer_optpara, 1] = 0;
                            genpool_input[pointer_parent].feedbackdata[pointer_optpara, 2] = 0;

                            //Parameter bestimmen
                            //Alle Schrittweiten berechnen und speichern (extremste Optparameter des Genpools suchen)
                            //haj als Zwischenspeicher nutzen
                            haj = genpool_input[0].get_optparas();
                            haj2 = genpool_input[0].get_optparas();
                            
                            for (int j = 0; j < numberoptparas; j++)
                            {
                                for (int i = 1; i < genpool_input.Length; i++)
                                {
                                    if (genpool_input[i].get_optparas()[j] < haj[j]) haj[j] = genpool_input[i].get_optparas()[j];
                                    if (genpool_input[i].get_optparas()[j] > haj2[j]) haj2[j] = genpool_input[i].get_optparas()[j];
                                }
                                genpool_input[pointer_parent].feedbackdata[j, 0] = (haj2[j] - haj[j]) / 2;
                            }

                            //Optparameter zufällig wählen
                            pointer_optpara = rand.Next(0, numberoptparas - 1);

                            //Optparas erstellen
                            haj = genpool_input[pointer_parent].get_optparas();
                            haj[pointer_optpara] -= genpool_input[pointer_parent].feedbackdata[pointer_optpara, 0];
                            if (haj[pointer_optpara] < genpool_input[pointer_parent].OptParameter[pointer_optpara].Min) haj[pointer_optpara] = genpool_input[pointer_parent].OptParameter[pointer_optpara].Min;
                            
                            //Individuum erstellen
                            genpool_input[pointer_parent].feedbackdata[pointer_optpara, 2] = 1;
                            //Zurückspeichern
                            new_generation_input[startindex - numberindividuums] = genpool_input[pointer_parent].Clone_MetaEvo();
                            new_generation_input[startindex - numberindividuums].ID = individuum_id; 
                            new_generation_input[startindex - numberindividuums].set_status("raw");
                            new_generation_input[startindex - numberindividuums].set_optparas(haj);
                            new_generation_input[startindex - numberindividuums].set_generator(algo_id);
                            //Feedbackdaten setzen
                            new_generation_input[startindex - numberindividuums].feedbackdata[pointer_optpara, 0] = genpool_input[pointer_parent].feedbackdata[pointer_optpara, 0] / 2;
                            new_generation_input[startindex - numberindividuums].feedbackdata[pointer_optpara, 1] = genpool_input[pointer_parent].feedbackdata[pointer_optpara, 1] + 1;
                            new_generation_input[startindex - numberindividuums].feedbackdata[pointer_optpara, 2] = 0;

                            individuum_id++;
                            numberindividuums--;

                            //Falls erlaubt, zweites Individuen erstellen 
                            if (numberindividuums > 0) 
                            {
                                haj2 = genpool_input[pointer_parent].get_optparas();
                                haj2[pointer_optpara] += genpool_input[pointer_parent].feedbackdata[pointer_optpara, 0];
                                if (haj2[pointer_optpara] > genpool_input[pointer_parent].OptParameter[pointer_optpara].Max) haj2[pointer_optpara] = genpool_input[pointer_parent].OptParameter[pointer_optpara].Max;
                                genpool_input[pointer_parent].feedbackdata[pointer_optpara, 2] = 2;
                                //Zurückspeichern
                                new_generation_input[startindex - numberindividuums] = genpool_input[pointer_parent].Clone_MetaEvo();
                                new_generation_input[startindex - numberindividuums].ID = individuum_id; 
                                new_generation_input[startindex - numberindividuums].set_status("raw");
                                new_generation_input[startindex - numberindividuums].set_optparas(haj2);
                                new_generation_input[startindex - numberindividuums].set_generator(algo_id);
                                //Feedbackdaten setzen
                                new_generation_input[startindex - numberindividuums].feedbackdata[pointer_optpara, 0] = genpool_input[pointer_parent].feedbackdata[pointer_optpara, 0] / 2;
                                new_generation_input[startindex - numberindividuums].feedbackdata[pointer_optpara, 1] = genpool_input[pointer_parent].feedbackdata[pointer_optpara, 1] + 1;
                                new_generation_input[startindex - numberindividuums].feedbackdata[pointer_optpara, 2] = 0;

                                individuum_id++;
                                numberindividuums--;
                            }
                        }
                    }
                    break;
                #endregion

                

                //Lokale Algorithmen
                #region Hook and Jeeves V2: Optparameter werden nacheinander variiert und zwei neue Individuen erzeugt
                case "Hook and Jeeves V2":
                     applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with " + algo_id + ":'" + algofeedbackarray[algo_id].name + "'...done");
                    
                    for (int i = 0; i < numberindividuums; i++)
                    {
                        if (genpool_input[i].get_generator() == algo_id)
                        {

                        }
                    }
                    break;
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
