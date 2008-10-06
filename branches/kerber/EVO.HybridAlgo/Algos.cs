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

            algofeedbackarray = new Algofeedback[4];
            algofeedbackarray[0] = new Algofeedback("Zufällige Einfache Mutation", individuumnumber / algofeedbackarray.Length);
            algofeedbackarray[1] = new Algofeedback("Zufällige Rekombination", individuumnumber / algofeedbackarray.Length);
            algofeedbackarray[2] = new Algofeedback("Diversität aus Sortierung", individuumnumber / algofeedbackarray.Length);
            algofeedbackarray[3] = new Algofeedback("Totaler Zufall", individuumnumber / algofeedbackarray.Length);
        }

        private string generationinfo(ref EVO.Common.Individuum_MetaEvo[] generation)
        {
            string back = "-----Generation-----\r\n";
            double[] optparas;

            for (int i = 0; i < generation.Length; i++)
            {
                back = back + "[ID]: " + generation[i].ID + " [Generator]: " + generation[i].get_generator() + " [Status]: " + generation[i].get_status() + " [Client]: " + generation[i].get_client() + "\r\nOptparas: ";
                optparas = generation[i].get_optparas();
                for (int j = 0; j < optparas.Length; j++)
                {
                    back = back + "[" + j + "]: " + String.Format("{0:N3}", optparas[j]) + " ";
                }
                if (generation[0].Constraints.Length > 0)
                {
                    back = back + "\r\nConst";
                    for (int j = 0; j < generation[i].Constraints.Length; j++)
                    {
                        back = back + "[" + j + "]: " + String.Format("{0:N3}", generation[i].Constraints[j]) + " ";
                    }
                }
                if (generation[0].Penalties.Length > 0)
                {
                    back = back + "\r\nPenaltie";
                    for (int j = 0; j < generation[i].Penalties.Length; j++)
                    {
                        back = back + "[" + j + "]: " + String.Format("{0:N3}", generation[i].Penalties[j]) + " ";
                    }
                }
                back = back + "\r\n";
            }

            return back + "----------";
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
                case "Zufällige Einfache Mutation": //Mutiert an einer zufälligen Stelle innerhalb der Grenzen von Min und Max
                    if (applog.log) applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with '" + algofeedbackarray[algo_id].name + "'...done");
                    
                    double[] mutated_optparas = new double[numberoptparas];
                    int selecteditem;

                    for (int i = 0; i < numberindividuums; i++)
                    {
                        //Zufälligen parent wählen und optparas kopieren
                        mutated_optparas = new_generation_input[rand.Next(0, genpool_input.Length - 1)].get_optparas();
                        //Zufälligen Wert mutieren
                        selecteditem = rand.Next(0, numberoptparas - 1);
                        mutated_optparas[selecteditem] = genpool_input[1].OptParameter[selecteditem].Min + (genpool_input[1].OptParameter[selecteditem].Max - genpool_input[1].OptParameter[selecteditem].Min) * ((double)rand.Next(0, 1000) / (double)1000);
                        //Zurückspeichern
                        new_generation_input[startindex + i] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuumnumber, numberoptparas);
                        new_generation_input[startindex + i].set_optparas(mutated_optparas);
                        new_generation_input[startindex + i].set_status("raw");
                        new_generation_input[startindex + i].set_generator(algo_id);
                        individuumnumber++; 
                    }
                    break;

                case "Zufällige Rekombination": //Die Werte zweier zufälliger Eltern werden zufällig rekombiniert
                    if (applog.log) applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with '" + algofeedbackarray[algo_id].name + "'...done");
                    
                    double[] recombinated_optparas = new double[numberoptparas];
                    double[] recombinated_optparas2 = new double[numberoptparas];
                    for (int i = 0; i < numberindividuums; i++)
                    {
                        //Zufälligen parent wählen und kopieren
                        recombinated_optparas = genpool_input[rand.Next(0, genpool_input.Length - 1)].get_optparas();
                        recombinated_optparas2 = genpool_input[rand.Next(0, genpool_input.Length - 1)].get_optparas();
                        //Zufällige Rekombination
                        for (int j = 0; j < numberoptparas; j++)
                        {
                            if (rand.Next(-10, 10) > 0) recombinated_optparas[j] = recombinated_optparas2[j];
                        }
                        //Zurückspeichern
                        new_generation_input[startindex + i] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuumnumber, numberoptparas);
                        new_generation_input[startindex + i].set_optparas(recombinated_optparas);
                        new_generation_input[startindex + i].set_status("raw");
                        new_generation_input[startindex + i].set_generator(algo_id);
                        individuumnumber++;
                    }
                    break;

                case "Diversität aus Sortierung": //Differenzvektorbestimmung und Addierung am Rande der nach zufälligem Kriterium sortierten Generation
                    if (applog.log) applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with '" + algofeedbackarray[algo_id].name + "'...done");
                    
                    double[] diversity_optparas = new double[numberoptparas]; 
                    double[] mult1 = new double[numberoptparas];
                    double[] mult2 = new double[numberoptparas];
                    for (int i = 0; i < numberoptparas; i++)
                    {
                        mult1[i] = 1;
                        mult2[i] = 1;
                    }

                    for (int i = 0; i < numberindividuums; i++)
                    {
                        if (i % 2 == 0)
                        {
                            for (int k = 0; k < genpool_input[0].get_optparas().Length; k++)
                            {
                                diversity_optparas[k] = genpool_input[0].get_optparas()[k] + mult1[k]*(genpool_input[0].get_optparas()[k] - genpool_input[((int)i/2)+1].get_optparas()[k]);
                                //Auf Max prüfen, ggf. Multiplikator für den Optpara anpassen anpassen
                                if ((genpool_input[0].OptParameter[k].Max < diversity_optparas[k]) || (genpool_input[0].OptParameter[k].Min > diversity_optparas[k]))
                                {
                                    mult1[k] *= 0.5;
                                    k--;
                                }
                            }
                        }
                        else
                        {
                            for (int k = 0; k < genpool_input[0].get_optparas().Length; k++)
                            {
                                diversity_optparas[k] = genpool_input[genpool_input.Length - 1].get_optparas()[k] + mult2[k] * (genpool_input[genpool_input.Length - 1].get_optparas()[k] - genpool_input[genpool_input.Length - 2 - (int)i / 2].get_optparas()[k]);
                                //Auf Min prüfen, ggf. Multiplikator für den Optpara anpassen anpassen
                                if ((genpool_input[0].OptParameter[k].Min > diversity_optparas[k]) || (genpool_input[0].OptParameter[k].Max < diversity_optparas[k]))
                                {
                                    mult2[k] *= 0.5;
                                    k--;
                                }
                            }
                        }
                        //Zurückspeichern
                        new_generation_input[startindex + i] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuumnumber, numberoptparas);
                        new_generation_input[startindex + i].set_optparas(diversity_optparas);
                        new_generation_input[startindex + i].set_status("raw");
                        new_generation_input[startindex + i].set_generator(algo_id);
                        individuumnumber++;
                    }
                    break;

                case "Totaler Zufall": //Alle Optparameter entstehen durch zufällige Wahl
                    if (applog.log) applog.appendText("Algos: Buliding " + numberindividuums + " Individuums with '" + algofeedbackarray[algo_id].name + "'...done"); 
                    
                    double[] random;
                    Random randomizer = new Random();

                    //Für jedes Individuum durchgehen
                    for (int k = 0; k < numberindividuums; k++)
                    {
                        random = new double[numberoptparas];
                        //Für jeden Parameter durchgehen
                        for (int j = 0; j < numberoptparas; j++)
                        {
                            double max = genpool_input[0].OptParameter[j].Max;
                            double min = genpool_input[0].OptParameter[j].Min;
                            random[j] = min + (max - min) * ((double)randomizer.Next(0, 1000) / 1000);
                        }
                        //Zurückspeichern
                        new_generation_input[startindex + k] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuumnumber, numberoptparas);
                        new_generation_input[startindex + k].set_optparas(random);
                        new_generation_input[startindex + k].set_status("raw");
                        new_generation_input[startindex + k].set_generator(algo_id);
                        individuumnumber++;
                    }
                    break;


                default:
                    MessageBox.Show("Algomanager", "Algorithmus " + algofeedbackarray[algo_id].name + " ist nicht bekannt!");
                    break;
            }
        }
    }
}
