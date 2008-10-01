﻿using System;
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

        //Eigenschaften für den Algo: Hier können Feedbackinfos abgelegt werden
        int[] feedback_int;
        string[] feedback_string;
        double[] feedback_double;

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
        int individuumnumber;
        EVO.Diagramm.ApplicationLog applog;

        public Algos(int individuumnumber_input, ref EVO.Diagramm.ApplicationLog applog_input)
        {
            individuumnumber = individuumnumber_input;
            applog = applog_input;

            algofeedbackarray = new Algofeedback[3];
            algofeedbackarray[0] = new Algofeedback("Zufällige Einfache Mutation", individuumnumber / algofeedbackarray.Length);
            algofeedbackarray[1] = new Algofeedback("Zufällige Rekombination", individuumnumber / algofeedbackarray.Length);
            algofeedbackarray[2] = new Algofeedback("Diversität aus Sortierung", individuumnumber / algofeedbackarray.Length);
        }

        public void newGeneration(ref EVO.Common.Individuum_MetaEvo[] genpool_input, ref EVO.Common.Individuum_MetaEvo[] new_generation_input, ref EVO.Common.Individuum_MetaEvo[] wastepool_input)
        {
            int startindex;

            startindex = 0;

            for (int algo_id = 0; algo_id < algofeedbackarray.Length; algo_id++)
            {
                this.build_individuals(ref genpool_input, ref new_generation_input, algo_id, startindex);
                startindex += algofeedbackarray[algo_id].number_individuals_for_nextGen;
            }
        }

        public void build_individuals(ref EVO.Common.Individuum_MetaEvo[] genpool_input, ref EVO.Common.Individuum_MetaEvo[] new_generation_input, int algo_id, int startindex_input)
        {
            //Beschreibbare Individuen für einen Algo: generation[startindex] bis generation[startindex+numberindividuums]
            int startindex = startindex_input;
            int numberindividuums = this.algofeedbackarray[algo_id].number_individuals_for_nextGen;
            int numberoptparas = genpool_input[0].get_optparas().Length;
            Random rand = new Random();

            //Sicherung gegen Rundungsfehler bei der Platzvergabe für die Algos
            if (startindex + numberindividuums >= genpool_input.Length)
            {
                numberindividuums = genpool_input.Length - startindex;
                algofeedbackarray[algo_id].number_individuals_for_nextGen = numberindividuums;
            }
            //Letzter Algo bekommt automatisch restliche Plätze
            if (algo_id == algofeedbackarray.Length - 1)
            {
                numberindividuums = genpool_input.Length - startindex;
                algofeedbackarray[algo_id].number_individuals_for_nextGen = numberindividuums;
            }

            switch (algofeedbackarray[algo_id].name)
            {
                case "Zufällige Einfache Mutation":
                    double[] mutated_optparas = new double[numberoptparas];
                    int selecteditem;

                    for (int i = 0; i < numberindividuums; i++)
                    {
                        //Zufälligen parent wählen und kopieren
                        new_generation_input[startindex + i] = genpool_input[rand.Next(0, genpool_input.Length)].Clone_MetaEvo();
                        mutated_optparas = new_generation_input[startindex + i].get_optparas();
                        //Zufälligen Wert mutieren
                        selecteditem = rand.Next(0, numberoptparas - 1);
                        mutated_optparas[selecteditem] = genpool_input[1].OptParameter[selecteditem].Min + (genpool_input[1].OptParameter[selecteditem].Max - genpool_input[1].OptParameter[selecteditem].Min) * ((double)rand.Next(0, 1000) / (double)1000);
                        //Zurückspeichern
                        new_generation_input[startindex + i].set_optparas(mutated_optparas);
                        new_generation_input[startindex + i].set_status("raw");
                        new_generation_input[startindex + i].set_generator(algo_id);
                        new_generation_input[startindex + i].ID = individuumnumber;
                        individuumnumber++;
                    }
                    if (applog.log) applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with '" + algofeedbackarray[algo_id].name + "'");
                    break;

                case "Zufällige Rekombination":
                    double[] recombinated_optparas = new double[numberoptparas];
                    double[] recombinated_optparas2 = new double[numberoptparas];
                    for (int i = 0; i < numberindividuums; i++)
                    {
                        //Zufälligen parent wählen und kopieren
                        new_generation_input[startindex + i] = genpool_input[rand.Next(0, genpool_input.Length)].Clone_MetaEvo();
                        recombinated_optparas = new_generation_input[startindex + i].get_optparas();
                        recombinated_optparas2 = genpool_input[rand.Next(0, genpool_input.Length)].get_optparas();
                        //Zufällige Rekombination
                        for (int j = 0; j < numberoptparas; j++)
                        {
                            if (rand.Next(-10, 10) > 0) recombinated_optparas2[j] = recombinated_optparas[j];
                        }
                        //Zurückspeichern
                        new_generation_input[startindex + i].set_optparas(recombinated_optparas2);
                        new_generation_input[startindex + i].set_status("raw");
                        new_generation_input[startindex + i].set_generator(algo_id);
                        new_generation_input[startindex + i].ID = individuumnumber;
                        individuumnumber++;
                    }
                    if (applog.log) applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with '" + algofeedbackarray[algo_id].name + "'");
                    break;

                case "Diversität aus Sortierung":
                    double[] diversity_optparas = new double[numberoptparas];
                    for (int i = 0; i < numberindividuums; i++)
                    {
                        if (i % 2 == 0)
                        {
                            for (int k = 0; k < genpool_input[0].get_optparas().Length; k++)
                            {
                                diversity_optparas[k] = genpool_input[0].get_optparas()[k] + (genpool_input[0].get_optparas()[k] - genpool_input[(int)i/2].get_optparas()[k]);
                                //Auf Max prüfen, ggf. Multiplikator für den Optpara anpassen anpassen
                                if (genpool_input[1].OptParameter[k].Max < diversity_optparas[k])
                                {

                                }
                            }
                        }
                        else
                        {
                            for (int k = 0; k < genpool_input[0].get_optparas().Length; k++)
                            {
                                diversity_optparas[k] = genpool_input[genpool_input.Length].get_optparas()[k] + (genpool_input[genpool_input.Length - 1].get_optparas()[k] - genpool_input[genpool_input.Length - 1 - (int)i / 2].get_optparas()[k]);
                                //Auf Min prüfen, ggf. Multiplikator für den Optpara anpassen anpassen
                                if (genpool_input[1].OptParameter[k].Min > diversity_optparas[k])
                                {

                                }
                            }
                        }
                        //Zurückspeichern
                        new_generation_input[startindex + i].set_optparas(diversity_optparas);
                        new_generation_input[startindex + i].set_status("raw");
                        new_generation_input[startindex + i].set_generator(algo_id);
                        new_generation_input[startindex + i].ID = individuumnumber;
                        individuumnumber++;
                    }
                    if (applog.log) applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with '" + algofeedbackarray[algo_id].name + "'");
                    break;

                default:
                    MessageBox.Show("Algomanager", "Algorithmus " + algofeedbackarray[algo_id].name + " ist nicht bekannt!");
                    break;
            }
        }
    }
}
