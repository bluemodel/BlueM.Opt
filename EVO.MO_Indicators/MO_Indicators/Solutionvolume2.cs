using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IHWB.EVO.MO_Indicators
{
    public class Solutionvolume2
    {
        double[,] values;    //[]([0] = Diversität der Generation(Abstandssumme zu Basepoint), [1] = Abstand der Basepoints
        double[] maxvaluesum;
        int historylength;    //Anzahl der Generationen die zum Vergleich herangenommen werden
        double[] basepoint_old;   //Basiswert von dem aus die Distanzquadrate berechnet werden
        double[] basepoint;
        double faktor2switch; //Wenn der Wert für Diversität oder Entwicklung zur Paretofront geringer ist als 1/faktor2switch wird umgeschaltet
        EVO.Diagramm.Monitor monitor1;

        public Solutionvolume2(ref EVO.Common.Individuum_MetaEvo settings_input, int historylength_input, int faktor2switch_input, ref EVO.Diagramm.Monitor monitor_input)
        {
            historylength = historylength_input;
            values = new double[historylength,2];
            monitor1 = monitor_input;
            basepoint = new double[settings_input.PrimObjectives.Length];
            maxvaluesum = new double[2];
            faktor2switch = faktor2switch_input;
        }

        //Summierte Distanzquadrate der Lösungen zum Nullpunkt
        public bool calculate(ref EVO.Common.Individuum_MetaEvo[] generation)
        {
            double[] tmp = new double[2];
            bool back = false;

            //Speichern von basepoint in basepoint_old
            basepoint.CopyTo(basepoint_old, 0);

            //basepoint = Durchschnitt der Generation
            basepoint = durchschnittsIndividuum(ref generation);

            //changes pushen 
            for (int i = values.Length-1; i > 0; i--)
            {
                values[i, 0] = values[i - 1, 0];
                values[i, 1] = values[i - 1, 1];
            }

            //Distanzsumme zu basepoint (Diversität)
            values[0, 0] = distanzsumme(ref generation, basepoint);
 
            //Abstand der Durchschnittsindividuen (Entwicklung richtung Paretofront)
            values[0, 1] = abstand(basepoint, basepoint_old);

            //Summe der Indikatorwerte
            tmp[0] = 0;
            tmp[1] = 0;
            for (int i = 0; i < historylength; i++)
            {
                tmp[0] += values[0, i];
                tmp[1] += values[1, i];
            }

            //Neue Maxwerte setzen oder neue Werte sind weniger als halb so gross wie bisherige -> Umschalten
            if (tmp[0] > maxvaluesum[0])  //Diversität
            {
                maxvaluesum[0] = tmp[0];
            }

            else if (maxvaluesum[0] > tmp[0] * faktor2switch)
            {
                back = true;
            }

            if (tmp[1] > maxvaluesum[1])  //Entwicklung richtung Paretofront
            {
                maxvaluesum[1] = tmp[1];
            }
            else if (maxvaluesum[1] > tmp[1] * faktor2switch)
            {
                back = true;
            }

            return back;
        }

        private double abstand(double[] eins, double[] zwei)
        {
            double abstand = 0;

            for (int i = 0; i < eins.Length; i++)
            {
                abstand += Math.Sqrt(eins[i] * eins[i] + zwei[i] * zwei[i]);
            }
            return abstand;
        }

        private double distanzsumme(ref EVO.Common.Individuum_MetaEvo[] generation, double[] basepoint)
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
