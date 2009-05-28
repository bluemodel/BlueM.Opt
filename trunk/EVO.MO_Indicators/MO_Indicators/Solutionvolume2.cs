using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IHWB.EVO.MO_Indicators
{
    public class Solutionvolume2
    {
        /// <summary>
        /// Diversität
        /// </summary>
        public double diversity;

        /// <summary>
        /// Durchschnitt der Evolution der letzten historylength Generationen
        /// </summary>
        public double evosum;

        /// <summary>
        /// Wenn der Wert für Diversität oder Entwicklung zur Paretofront geringer ist als 1/faktor2switch wird umgeschaltet
        /// </summary>
        public double faktor2switch;

        /// <summary>
        /// Maximaler Durchschnitt der Evolution der letzten historylength Generationen
        /// </summary>
        public double maxevosum;

        /// <summary>
        /// Anzahl der Generationen die zum Vergleich herangenommen werden
        /// </summary>
        public int historylength;

        double[] evo;    //Abstand der Basepoints zueinander
        double[] basepoint_old;   //Basiswert von dem aus die Distanzquadrate berechnet werden
        double[] basepoint;
        string completeinfo;
        bool firstrun;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="NumPrimObjectives">Number of primary objectives</param>
        /// <param name="historylength_input"></param>
        /// <param name="faktor2switch_input"></param>
        public Solutionvolume2(int NumPrimObjectives, int historylength_input, int faktor2switch_input)
        {
            historylength = historylength_input;
            faktor2switch = faktor2switch_input;
     
            basepoint = new double[NumPrimObjectives];
            basepoint_old = new double[NumPrimObjectives];
            evo = new double[historylength];

            evosum = 0;
            maxevosum = 0;
            diversity = 0;
            completeinfo = "";
            firstrun = true;
        }

        /// <summary>
        /// Solutionvolume berechnen
        /// </summary>
        /// <param name="generation">Eine Generation von Meta-Individuen</param>
        /// <returns>boolean</returns>
        public bool calculate(ref EVO.Common.Individuum_MetaEvo[] generation)
        {
            bool back = false;

            //Speichern von basepoint in basepoint_old
            basepoint.CopyTo(basepoint_old, 0);

            //basepoint = Durchschnittswerte im Zielfunktionsraum der Generation
            basepoint = durchschnittsIndividuum(ref generation);
            
            //Durchschnittliche Distanz der Individuen zum basepoint (Diversität)
            diversity = durchschnittliche_distanz(ref generation, basepoint);
            
            //values der Generationen pushen 
            for (int i = evo.Length-1; i > 0; i--)
            {
                evo[i] = evo[i - 1];
            }

            //Abstand der Durchschnittsindividuen (Entwicklung richtung Paretofront)
            if (firstrun) { evo[0] = 0; firstrun = false; }
            else evo[0] = abstand(basepoint, basepoint_old);

            //SUMMEN der Indikatorwerte über historylength Generationen
            evosum = 0;
            for (int i = 0; i < historylength; i++)
            {
                evosum += evo[i];
            }

            //Neue Maxwerte setzen oder neue Werte sind weniger als 1/faktor2switch so gross wie bisherige -> Umschalten
            if (evosum > maxevosum)  //Entwicklung richtung Paretofront
            {
                maxevosum = evosum;
            }
            else if (maxevosum > evosum * faktor2switch)
            {
                back = true;
            }

            completeinfo = "Div: " + diversity + " Evo: " + evo[0] + " Sum of last " + historylength + " generations: " + evosum + " (Frontier:" + maxevosum + ") - Faktor2switch: " + faktor2switch;
            if (back) completeinfo += " -> switch to local optimization)";
            return back;
        }

        public string get_last_infos()
        {
            return "Div: " + diversity + " Evo: " + evo[0];
        }

        public string get_complete_infos()
        {
            return completeinfo;
        }

        private double abstand(double[] eins, double[] zwei)
        {
            double abstand = 0;

            for (int i = 0; i < eins.Length; i++)
            {
                abstand += Math.Pow(eins[i]-zwei[i],2);
            }
            return Math.Sqrt(abstand);
        }

        private double durchschnittliche_distanz(ref EVO.Common.Individuum_MetaEvo[] generation, double[] basepoint)
        {
            double distanzsumme = 0;
            int numberPrimobjectives = generation[0].PrimObjectives.Length;
            double[] primobjectives = new double[numberPrimobjectives];

            for (int i = 0; i < generation.Length; i++)
            {
                primobjectives = generation[i].PrimObjectives;
                for (int j = 0; j < primobjectives.Length; j++)
                {
                    distanzsumme += Math.Abs(primobjectives[j] - basepoint[j]);
                }
            }
            return distanzsumme / generation.Length;
        }

        private double[] durchschnittsIndividuum(ref EVO.Common.Individuum_MetaEvo[] generation)
        {
            int numberPrimobjectives = generation[0].PrimObjectives.Length;
            double[] primobjectivesSum = new double[numberPrimobjectives];
            double[] primobjectives = new double[numberPrimobjectives];

            for (int j = 0; j < primobjectives.Length; j++)
            {
                primobjectivesSum[j] = 0;
                primobjectives[j] = 0;
            }

            for (int i = 0; i < generation.Length; i++)
            {
                primobjectives = generation[i].PrimObjectives;
                for (int j = 0; j < primobjectives.Length; j++)
                {
                    primobjectivesSum[j] += (primobjectives[j]/generation.Length);
                }
            }
            return primobjectivesSum;
        }
    }
}
