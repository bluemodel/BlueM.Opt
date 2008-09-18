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

        public void build_individuals(ref EVO.Common.Individuum_MetaEvo[] genpool_input, ref EVO.Common.Individuum_MetaEvo[] new_generation_input, Algofeedback feedback_input, int startindex_input, ref int individuumnumer_input)
        {
            //Beschreibbare Individuen für einen Algo: generation[startindex] bis generation[startindex+numberindividuums]
            int startindex = startindex_input;
            int numberindividuums = feedback_input.number_individuals_for_nextGen;
            int numberoptparas = genpool_input[0].get_optparas().Length;
            Random rand = new Random();

            new_generation_input.Initialize();

            switch (feedback_input.name)
            {
                case "Zufällige Einfache Mutation":
                    double[] mutated_optparas = new double[numberoptparas];
                    for (int i = 0; i < numberindividuums; i++)
                    {
                        //Zufälligen parent wählen und kopieren
                        new_generation_input[startindex + i] = genpool_input[rand.Next(0, genpool_input.Length)].Clone_MetaEvo();
                        mutated_optparas = new_generation_input[startindex + i].get_optparas();
                        //Zufälligen Wert mutieren
                        mutated_optparas[rand.Next(0, numberoptparas - 1)] *= ((double)rand.Next(-200, +200)/100.00);
                        //Zurückspeichern
                        new_generation_input[startindex + i].set_optparas(mutated_optparas);
                        new_generation_input[startindex + i].set_status("raw");
                        new_generation_input[startindex + i].ID = individuumnumer_input;
                        individuumnumer_input++;
                    }
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
                        new_generation_input[startindex + i].ID = individuumnumer_input;
                        individuumnumer_input++;
                    }
                    break;
                default:
                    MessageBox.Show("Algomanager", "Algorithmus " + feedback_input.name + " ist nicht bekannt!");
                    break;
            }
        }
    }
}
