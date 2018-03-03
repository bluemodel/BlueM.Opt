/*
Copyright (c) BlueM Dev Group
Website: http://bluemodel.org

All rights reserved.

Released under the BSD-2-Clause License:

Redistribution and use in source and binary forms, with or without modification, 
are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list 
  of conditions and the following disclaimer.
* Redistributions in binary form must reproduce the above copyright notice, this list 
  of conditions and the following disclaimer in the documentation and/or other materials 
  provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT 
OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
--------------------------------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace BlueM.Opt.MO_Indicators
{
    public class Solutionvolume
    {
        public System.Collections.Specialized.StringCollection messages;

        double[] solutionvolume; 
        int historylength;    //Anzahl der Generationen die zum Vergleich herangenommen werden
        double[] basepoint;   //Basiswert von dem aus die Distanzquadrate berechnet werden
        double minimumchange = -1;  //Minimaler Änderungswert über |historylength| generationen

        public Solutionvolume(int historylength_input)
        {
            historylength = historylength_input;
            solutionvolume = new double[historylength];
            messages = new System.Collections.Specialized.StringCollection();
        }

        public Solutionvolume(int historylength_input, double minimumchange_input)
        {
            historylength = historylength_input;
            solutionvolume = new double[historylength];
            minimumchange = minimumchange_input;
            messages = new System.Collections.Specialized.StringCollection();
        }

        public double get_last_volume() 
        {
            return Math.Round(solutionvolume[0],3);
        }

        //Summierte Distanzquadrate der Lösungen zum Nullpunkt
        public double[] calculate(ref BlueM.Opt.Common.Individuum_MetaEvo[] generation)
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

        public bool calculate_and_decide(ref BlueM.Opt.Common.Individuum_MetaEvo[] generation)
        {
            double sum = 0;
            this.messages.Clear();

            calculate(ref generation);
            
            //Arbeitet nur wenn minimumchange initialisiert wurde und 
            //die gegebene Anzahl an zu vergleichenden solutionvolumes schon einmal berechnet wurden
            if ((minimumchange != -1) && (solutionvolume[historylength-1] != 0))
            {
                this.messages.Add("Algo Manager: Solutionvolume: Actual change: " + Math.Round(((solutionvolume[1] / solutionvolume[0]) - 1) * 100, 2) + "% during last generation"); 

                for (int i = 0; i < solutionvolume.Length - 1; i++)
                {
                    sum += Math.Max((solutionvolume[i+1]/solutionvolume[i]),(solutionvolume[i]/solutionvolume[i+1])); 
                }
                sum = (sum / (solutionvolume.Length - 1)) - 1; //Prozentuale Änderung innerhalb der letzten |historylength| Generationen

                if (sum < minimumchange)
                {
                    this.messages.Add("Algo Manager: Solutionvolume: Less than " + Math.Round(minimumchange * 100, 2) + "% (" + Math.Round(sum, 2) + "%) change during last " + historylength + " generations");
                    this.messages.Add("Algo Manager: Solutionvolume: [0]:" + solutionvolume[0] + " [1]:" + solutionvolume[1] + " [2]:" + solutionvolume[2] + "[3]:" + solutionvolume[3] + " [4]:" + solutionvolume[4]); 
                    return true;
                }
            }
            return false;
        }
    }
}
