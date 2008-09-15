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
        public double initiative;

        //Eigenschaften für den Algo: Hier können Feedbackinfos abgelegt werden
        string feedback_string;
        double feedback_double;

        public Algofeedback(string name_input)
        {
            this.name = name_input;
            this.initiative = 10;
        }
    }


    class Algomanager
    {
        Algofeedback[] algofeedbackarray;
        EVO.Common.Individuum_MetaEvo[] genpool;
        Algos algos;
        int startindex;

        public Algomanager(ref EVO.Common.Problem prob_input)
        {
            //Algoarray initialisieren in der Grösse der Anzahl der Algorithmen
            algofeedbackarray = new Algofeedback[5];

            algofeedbackarray[0] = new Algofeedback("testalgo_0");
            algofeedbackarray[1] = new Algofeedback("testalgo_1");
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
            quicksort(ref new_generation_input, kriterium, 0, new_generation_input[0].get_optparas().Length-1);
            domination(ref new_generation_input, ref new_generation_input);

            //1.1.4.Sortieren innerhalb der alten Individuen
            quicksort(ref genpool, kriterium, 0, new_generation_input[0].get_optparas().Length - 1); 
            
            //1.1.5.Dominanzkriterium anwenden zwischen den neuen und den alten Individuen
            domination(ref new_generation_input, ref genpool);

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
                algos.build_individuals(algofeedbackarray[k], ref new_generation_input, startindex);
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

        private void domination(ref EVO.Common.Individuum_MetaEvo[] input, ref EVO.Common.Individuum_MetaEvo[] input2)
        {
            //Falls input1 = input2, nur innerhalb der ersten Generation prüfen
        }
    }
}
