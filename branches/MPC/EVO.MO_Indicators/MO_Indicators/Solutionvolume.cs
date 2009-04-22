using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IHWB.EVO.MO_Indicators
{
    public class Solutionvolume
    {
        double[] solutionvolume; 
        int historylength;    //Anzahl der Generationen die zum Vergleich herangenommen werden
        double[] basepoint;   //Basiswert von dem aus die Distanzquadrate berechnet werden
        double minimumchange = -1;  //Minimaler Änderungswert über |historylength| generationen
        EVO.Diagramm.Monitor monitor1;

        public Solutionvolume(int historylength_input)
        {
            historylength = historylength_input;
            solutionvolume = new double[historylength];
        }

        public Solutionvolume(int historylength_input, double minimumchange_input, ref EVO.Diagramm.Monitor monitor_input)
        {
            historylength = historylength_input;
            solutionvolume = new double[historylength];
            minimumchange = minimumchange_input;
            monitor1 = monitor_input;
        }

        public double get_last_volume() 
        {
            return Math.Round(solutionvolume[0],3);
        }

        //Summierte Distanzquadrate der Lösungen zum Nullpunkt
        public double[] calculate(ref EVO.Common.Individuum_MetaEvo[] generation)
        {
            double[] primobjectives_input;

            if (basepoint == null) 
            {
                basepoint = new double[generation[0].PrimObjectives.Length];
            }

            //Alte Ergebnisse im Array nach hinten schieben
            for (int i = historylength - 1; i > 0; i--)
            {
                solutionvolume[i] = solutionvolume[i-1];
            }

            solutionvolume[0] = 0;

            //Neues Solutionvolume ausrechnen
            for (int j = 0; j < generation.Length; j++) 
            {
                primobjectives_input = generation[j].PrimObjectives;
                for (int i = 0; i < primobjectives_input.Length; i++) 
                {
                    solutionvolume[0] += (primobjectives_input[i]+1) * (primobjectives_input[i]+1);
                }
            }
            solutionvolume[0] = solutionvolume[0] / generation.Length;

            return solutionvolume;
        }

        public bool calculate_and_decide(ref EVO.Common.Individuum_MetaEvo[] generation)
        {
            double sum = 0;

            calculate(ref generation);
            
            //Arbeitet nur wenn minimumchange initialisiert wurde und 
            //die gegebene Anzahl an zu vergleichenden solutionvolumes schon einmal berechnet wurden
            if ((minimumchange != -1) && (solutionvolume[historylength-1] != 0))
            {
                monitor1.LogAppend("Algo Manager: Solutionvolume: Actual change: " + Math.Round(((solutionvolume[1] / solutionvolume[0]) - 1) * 100, 2) + "% during last generation"); 

                for (int i = 0; i < solutionvolume.Length - 1; i++)
                {
                    sum += Math.Max((solutionvolume[i+1]/solutionvolume[i]),(solutionvolume[i]/solutionvolume[i+1])); 
                }
                sum = (sum / (solutionvolume.Length - 1)) - 1; //Prozentuale Änderung innerhalb der letzten |historylength| Generationen

                if (sum < minimumchange)
                {
                    monitor1.LogAppend("Algo Manager: Solutionvolume: Less than " + Math.Round(minimumchange * 100, 2) + "% (" + Math.Round(sum, 2) + "%) change during last " + historylength + " generations");
                    monitor1.LogAppend("Algo Manager: Solutionvolume: [0]:" + solutionvolume[0] + " [1]:" + solutionvolume[1] + " [2]:" + solutionvolume[2] + "[3]:" + solutionvolume[3] + " [4]:" + solutionvolume[4]); 
                    return true;
                }
            }
            return false;
        }
    }
}
