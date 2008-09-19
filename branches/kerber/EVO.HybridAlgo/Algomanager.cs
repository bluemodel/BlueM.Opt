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
        public double initiative;

        //Eigenschaften für den Algo: Hier können Feedbackinfos abgelegt werden
        int[] feedback_int;
        string[] feedback_string;
        double[] feedback_double;

        public Algofeedback(string name_input)
        {
            this.name = name_input;
            this.initiative = 10;
            this.number_individuals_for_nextGen = 1;
        }
    }


    class Algomanager
    {
        Algofeedback[] algofeedbackarray;
        EVO.Common.Individuum_MetaEvo[] genpool;
        Algos algos;
        int startindex;
        int individuumnumber;

        public Algomanager(ref EVO.Common.Problem prob_input, int individuumnumber_input) //Zeichenobjekt übergeben
        {
            individuumnumber = individuumnumber_input;
            //Algoarray initialisieren in der Grösse der Anzahl der Algorithmen
            algos = new Algos();

            algofeedbackarray = new Algofeedback[2];

            algofeedbackarray[0] = new Algofeedback("Zufällige Einfache Mutation");
            algofeedbackarray[1] = new Algofeedback("Zufällige Rekombination");
        }

        public void set_genpool(EVO.Common.Individuum_MetaEvo[] genpool_input) 
        {
            this.genpool = genpool_input;
        }

        public void eval_and_build(ref EVO.Common.Individuum_MetaEvo[] new_generation_input)
        {
            //1.Selektion: 
            //1.1.Dominanzanalyse
            //1.1.2.Sortieren nach einem zufällig gewählten Kriterium
            Random rand = new Random();
            int kriterium = rand.Next(0, new_generation_input[0].get_optparas().Length);

            //1.1.3.Sortieren und Dominanzkriterium anwenden innerhalb der neuen Individuen
            quicksort(ref new_generation_input, kriterium, 0, new_generation_input.Length-1);
            check_domination(ref new_generation_input, ref new_generation_input);
            
            //1.1.4.Dominanzkriterium anwenden zwischen den neuen und den alten Individuen
            check_domination(ref new_generation_input, ref genpool);

            //1.2.Clustering bis auf maximale Generationsgrösse
            clustering(ref genpool, ref new_generation_input);

            //1.2.1.Sortieren des genpools
            quicksort(ref genpool, kriterium, 0, new_generation_input.Length - 1);

            //2.Feedback erstellen
            //2.1.Initiative berechnen
            //2.2.Anzahl Individuen pro Algo für nächste Generation berechnen

            //3.Rückgabe für Zeichnen generieren (aus neuem Genpool)

            //4.Generierung neuer Individuen
            startindex = 0;
            for (int k = 0; k < algofeedbackarray.Length; k++)
            {
                algos.build_individuals(ref genpool, ref new_generation_input, algofeedbackarray[k], startindex, ref individuumnumber);
                startindex += algofeedbackarray[k].number_individuals_for_nextGen;
            }
        }

        private void quicksort(ref EVO.Common.Individuum_MetaEvo[] input, int kriterium, int low_input, int high_input)
        {
            int low = low_input;
            int high = high_input;
            double medianvalue = input[(low_input + high_input) / 2].get_optparas()[kriterium];
            EVO.Common.Individuum_MetaEvo individuum_tmp;
            while (low < high)
            {
                //Nächste Abweichung von der erstrebten Sortierung suchen
                while ((input[low].get_optparas()[kriterium] <= medianvalue) && (low < high)) low++;
                while ((input[high].get_optparas()[kriterium] >= medianvalue) && (low < high)) high--;
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
            low--;
            high++;
            if (low_input < low) quicksort(ref input, kriterium, low_input, low);
            if (high < high_input) quicksort(ref input, kriterium, high, high_input);
        }

        private void check_domination(ref EVO.Common.Individuum_MetaEvo[] input, ref EVO.Common.Individuum_MetaEvo[] input2)
        {
            bool dominated = true;
            int status = 0;

            //Falls input1 = input2, nur innerhalb der ersten Generation prüfen
            if (input == input2)
            {
                //Individuen vergleichen
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i].get_status() == "true")
                    {
                        //mit
                        for (int k = i + 1; k < input.Length; k++)
                        {
                            if (input[k].get_status() == "true")
                            {
                                //Jede Eigenschaft vergleichen
                                for (int j = 0; j < input[0].Penalties.Length; j++)
                                {
                                    if (input[i].Penalties[j] > input[k].Penalties[j]) { dominated = false; break; }
                                }
                                if (dominated) { input[i].set_status("false"); dominated = true; break; }
                            }
                        }
                    }
                }
            }
            //Sonst Generationen untereinander prüfen
            else
            {
                //Individuen vergleichen
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i].get_status() == "true")
                    {
                        //mit
                        for (int k = 0; k < input2.Length; k++)
                        {
                            if (input2[k].get_status() == "true")
                            {
                                //Jede Eigenschaft vergleichen
                                for (int j = 0; j < input[0].Penalties.Length; j++)
                                {
                                    if (input[i].Penalties[j] > input2[k].Penalties[j])
                                    {
                                        if (status == 0) status = 1;
                                        else if (status == -1)
                                        {
                                            status = 0;
                                            break;
                                        }
                                    }
                                    if (input[i].Penalties[j] < input2[k].Penalties[j])
                                    { 
                                        if (status == 0) status = -1;
                                        else if (status == 1)
                                        {
                                            status = 0;
                                            break;
                                        }
                                    }
                                }
                                if (status == 1) { status = 0; input2[k].set_status("false"); break; }
                                if (status == -1) { status = 0; input[i].set_status("false"); }
                            }
                        }
                    }
                }
            }

           
        }

        private void clustering(ref EVO.Common.Individuum_MetaEvo[] input, ref EVO.Common.Individuum_MetaEvo[] input2)
        {
            int maxindividuums = genpool.Length;
            double distance;

            for (int i = 0; i < maxindividuums; i++)
            {
                if (input[i].get_status() == "true") maxindividuums--;
                if (input2[i].get_status() == "true") maxindividuums--;
            }
            // zu viele Individuen vorhanden
            if (maxindividuums < 0)
            {
                maxindividuums *= -1;
                double[,] individuums2kill = new double[maxindividuums, 2];

                //Arbeits-Array erstellen
                EVO.Common.Individuum_MetaEvo[] work = new IHWB.EVO.Common.Individuum_MetaEvo[2 * input.Length];
                for (int i = 0; i < 2 * input.Length; i++)
                {
                    if (i > input.Length) work[i] = input2[i - input.Length];
                    else work[i] = input[i];
                }

                //Geringste Abstände finden
                for (int i = 0; i < work.Length; i++)
                {
                    if (work[i].get_status() == "true")
                    {
                        //mit
                        for (int k = i + 1; k < work.Length; k++)
                        {
                            if (work[k].get_status() == "true")
                            {
                                for (int j = 1; j <= individuums2kill.Length; j++)
                                {
                                    distance = easydistance(work[i].get_optparas(), work[k].get_optparas());
                                    if (distance < individuums2kill[individuums2kill.Length - j, 1])
                                    {
                                        if (j != 1)
                                        {
                                            individuums2kill[individuums2kill.Length - j + 1, 0] = individuums2kill[individuums2kill.Length - j, 0];
                                            individuums2kill[individuums2kill.Length - j + 1, 1] = individuums2kill[individuums2kill.Length - j, 1];
                                            individuums2kill[individuums2kill.Length - j, 0] = i;
                                            individuums2kill[individuums2kill.Length - j, 1] = distance;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //Zu nahe Individuen killen
                for (int i = 0; i < individuums2kill.Length; i++)
                {
                    work[(int)individuums2kill[i, 0]].set_status("false");
                }
                //Zur überprüfung ob genug Individuen reduziert wurden
                clustering(ref input, ref input2);
            }
            //Wenn die Individuenanzahl der definierten Generationsgrösse entspricht, alles in den Genpool transferieren
            else
            {
                int pointer = 0;

                for (int i = 0; i < 2*input.Length; i++)
                {
                    if (i > input.Length)
                    {
                        if (input2[i - input.Length].get_status() == "true")
                        {
                            genpool[pointer] = input2[i - input.Length];
                            pointer++;
                        }
                    }
                    else
                    {
                        if (input[i].get_status() == "true")
                        {
                            genpool[pointer] = input[i];
                            pointer++;
                        }
                    }
                }
            }
        }

        private double easydistance(double[] input1, double[] input2)
        {
            double back = 0;

            for (int i = 0; i < input1.Length; i++)
            {
                back += System.Math.Abs(input1[i] - input2[i]);
            }

            return back;
        }
    }


}
