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
        string[] feedback_string;
        double[] feedback_double;

        public Algofeedback(string name_input)
        {
            this.name = name_input;
            this.initiative = 10;
            this.number_individuals_for_nextGen = 0;
        }
    }


    class Algomanager
    {
        Algofeedback[] algofeedbackarray;
        EVO.Common.Individuum_MetaEvo[] genpool;
        Algos algos;
        int startindex;
        int minimumdistance;

        public Algomanager(ref EVO.Common.Problem prob_input)
        {
            //Algoarray initialisieren in der Grösse der Anzahl der Algorithmen
            algos = new Algos();

            algofeedbackarray = new Algofeedback[5];

            algofeedbackarray[0] = new Algofeedback("testalgo_0");
            algofeedbackarray[1] = new Algofeedback("testalgo_1");
        }

        public void set_genpool(EVO.Common.Individuum_MetaEvo[] genpool_input) 
        {
            this.genpool = genpool_input;

            //minimumdistance initialisieren -> maximum der Distanz zwischen zwei Nachbarn
        }

        public void eval_and_build(ref EVO.Common.Individuum_MetaEvo[] new_generation_input)
        {
            //1.Selektion: 
            //1.1.Dominanzanalyse
            //1.1.2.Sortieren nach einem zufällig gewählten Kriterium
            Random rand = new Random();
            int kriterium = rand.Next(0, new_generation_input[0].get_optparas().Length);

            //1.1.3.Sortieren und Dominanzkriterium anwenden innerhalb der neuen Individuen
            quicksort(ref new_generation_input, kriterium, 0, new_generation_input[0].get_optparas().Length-1);
            check_domination(ref new_generation_input, ref new_generation_input, kriterium);

            //1.1.4.Sortieren innerhalb der alten Individuen
            quicksort(ref genpool, kriterium, 0, new_generation_input[0].get_optparas().Length - 1); 
            
            //1.1.5.Dominanzkriterium anwenden zwischen den neuen und den alten Individuen
            check_domination(ref new_generation_input, ref genpool, kriterium);

            //1.2.Clustering bis auf maximale Generationsgrösse

            //2.Feedback erstellen
            //2.1.Initiative berechnen
            //2.2.Individuen für nächste Generation berechnen

            //3.Genpool neu erstellen
            
            //4.Rückgabe für Zeichnen generieren (aus neuem Genpool)

            //5.Generierung neuer Individuen
            startindex = 0;
            for (int k = 0; k < algofeedbackarray.Length; k++)
            {
                algos.build_individuals(ref genpool, ref new_generation_input, algofeedbackarray[k], startindex);
                startindex += algofeedbackarray[k].number_individuals_for_nextGen;
            }
        }

        private void quicksort(ref EVO.Common.Individuum_MetaEvo[] input, int kriterium, int low_input, int high_input)
        {
            int low = low_input;
            int high = high_input;
            double medianvalue = input[(low_input + high_input) / 2].get_optparas()[kriterium];
            EVO.Common.Individuum_MetaEvo individuum_tmp;
            while (low_input <= high_input)
            {
                while (input[low].get_optparas()[kriterium] < medianvalue) low++;
                while (input[high].get_optparas()[kriterium] > medianvalue) high++;
                if (low <= high)
                {
                    individuum_tmp = input[low];
                    input[low] = input[high];
                    input[high] = individuum_tmp;
                    low++;
                    high++;
                }
            }
            if (low_input < low) quicksort(ref input, kriterium, low_input, low);
            if (high < high_input) quicksort(ref input, kriterium, high, high_input);
        }

        private void check_domination(ref EVO.Common.Individuum_MetaEvo[] input, ref EVO.Common.Individuum_MetaEvo[] input2, int kriterium)
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
                                for (int j = 0; j < input[0].Features.Length; j++)
                                {
                                    if (input[i].Features[j] > input[k].Features[j]) { dominated = false; break; }
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
                                for (int j = 0; j < input[0].Features.Length; j++)
                                {
                                    if (input[i].Features[j] > input2[k].Features[j]) {
                                        if (status == 0) status = 1;
                                        else if (status == -1)
                                        {
                                            status = 0;
                                            break;
                                        }
                                    }
                                    if (input[i].Features[j] < input2[k].Features[j]) { 
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
    
        
    }
}
