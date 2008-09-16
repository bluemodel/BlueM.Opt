using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IHWB.EVO.MetaEvo
{
    class Algos
    {
        

        public Algos()
        {
           
        }

        public void build_individuals(ref EVO.Common.Individuum_MetaEvo[] genpool_input, ref EVO.Common.Individuum_MetaEvo[] new_generation_input, Algofeedback feedback_input, int startindex_input)
        {
            //Beschreibbare Individuen für einen Algo: generation[startindex] bis generation[startindex+numberindividuums]
            int startindex = startindex_input;
            int numberindividuums = feedback_input.number_individuals_for_nextGen;

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
