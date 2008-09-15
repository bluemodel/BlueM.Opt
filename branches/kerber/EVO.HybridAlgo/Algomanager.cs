using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IHWB.EVO.HybridAlgo
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
            //1.1.3.Dominanzkriterium anwenden innerhalb der neuen Individuen
            //1.1.4.Dominanzkriterium anwenden zwischen den neuen und den alten Individuen

            //1.2.Clustering bis auf Generationsgrösse

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

        private void quicksort(ref EVO.Common.Individuum_MetaEvo[] individuen_input)
        {
            Random rand = new Random();
            int kriterium;
            kriterium = rand.Next(0,individuen_input[0].get_optparas().Length);
        }
    }
}
