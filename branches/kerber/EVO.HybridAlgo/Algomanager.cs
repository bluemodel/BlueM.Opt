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
        public int number_individuals;
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
        Algofeedback[] algoarray;
        EVO.Common.Individuum_MetaEvo[] genpool;

        public Algomanager(ref EVO.Common.Problem prob_input)
        {
            //Algoarray initialisieren
            algoarray = new Algofeedback[5];

            algoarray[0] = new Algofeedback("testalgo_0");
            algoarray[1] = new Algofeedback("testalgo_1");
        }

        public void set_genpool(EVO.Common.Individuum_MetaEvo[] genpool_input) 
        {
            this.genpool = genpool_input;
        }

        public void build_new_generation(ref EVO.Common.Individuum_MetaEvo[] new_generation_input)
        {
            //Selektion: Algos für neue Generation Anzahl Individuen zuteilen, Genpool neu erstellen
            
            //Rückgabe für Zeichnen generieren (neuer Genpool)

            //Generierung neuer Individuen
            for (int k = 0; k < algoarray.Length; k++)
            {
                build_individuals(algoarray[k]);
            }
        }

        private void build_individuals(Algofeedback feedback_input)
        {
            switch (feedback_input.name)
            {
                case "testalgo_0":

                    break;
                case "testalgo_1":

                    break;
                default:
                    MessageBox.Show("Algomanager", "Algorithmus " + feedback_input.name + " ist nicht bekannt!");
                    break;
            }
        }
    }
}
