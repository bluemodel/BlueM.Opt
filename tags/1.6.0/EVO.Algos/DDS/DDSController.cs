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

namespace modelEAU.DDS
{
    /// <summary>
    /// Kontrolliert den Ablauf des DDS
    /// </summary>
    public class DDSController : IHWB.EVO.IController
    {
        /// <summary>
        /// Multithreading Support
        /// </summary>
        public bool MultithreadingSupported
        {
            get {return false;}
        }

        private IHWB.EVO.Common.Problem mProblem;
        private IHWB.EVO.Common.Settings mSettings;
        private IHWB.EVO.Common.Progress mProgress;
        private IHWB.EVO.Diagramm.Monitor mMonitor; //z.Zt. nicht benutzt
        private IHWB.EVO.Diagramm.Hauptdiagramm Hauptdiagramm1;
        private IHWB.EVO.Apps.Sim Sim1;
        private IHWB.EVO.Apps.Testprobleme Testproblem;

        private IHWB.EVO.Common.Constants.ApplicationTypes myAppType;

        private bool stopped;

        /// <summary>
        /// Initialisiert den DDS-Controller und übergibt alle erforderlichen Objekte
        /// </summary>
        /// <param name="inputProblem">das Problem</param>
        /// <param name="inputSettings">die Einstellungen</param>
        /// <param name="inputProgress">der Verlauf</param>
        /// <param name="inputHauptdiagramm">das Hauptdiagramm</param>
        public void Init(ref IHWB.EVO.Common.Problem inputProblem, ref IHWB.EVO.Common.Settings inputSettings, ref IHWB.EVO.Common.Progress inputProgress, ref IHWB.EVO.Diagramm.Hauptdiagramm inputHauptdiagramm)
        {
            //Objekte übergeben
            this.mProblem = inputProblem;
            this.mSettings = inputSettings;
            this.mProgress = inputProgress;
            this.Hauptdiagramm1 = inputHauptdiagramm;

            this.mMonitor = IHWB.EVO.Diagramm.Monitor.getInstance();
        }

        /// <summary>
        /// Initialisiert den Controller für Sim-Anwendungen
        /// </summary>
        /// <param name="inputSim">Sim-Objekt</param>
        public void InitApp(ref IHWB.EVO.Apps.Sim inputSim)
        {
            this.Sim1 = inputSim;
            this.myAppType = IHWB.EVO.Common.Constants.ApplicationTypes.Sim;
        }

        /// <summary>
        /// Initialisiert den Controller für Testprobleme
        /// </summary>
        /// <param name="inputTestproblem">Testproblem-Objekt</param>
        public void InitApp(ref IHWB.EVO.Apps.Testprobleme inputTestproblem)
        {
            this.Testproblem = inputTestproblem;
            this.myAppType = IHWB.EVO.Common.Constants.ApplicationTypes.Testproblems;
        }

        /// <summary>
        /// Startet die Optimierung
        /// </summary>
        public void Start()
        {
            this.stopped = false;

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //Declarations
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            int i, j;
            int run = 0;
            double[] Ini_Parameter;
            double[] Current_Parameter;
            IHWB.EVO.Common.Individuum ind;
            modelEAU.DDS.DDS DDS;
            bool SIM_Eval_is_OK;
            Steema.TeeChart.Styles.Series serie;

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //Initialize
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            Current_Parameter = new double[this.mProblem.NumOptParams];
            DDS = new modelEAU.DDS.DDS();

            if (this.mProblem.List_ObjectiveFunctions[0].Richtung == IHWB.EVO.Common.Constants.EVO_DIRECTION.Maximization)
            {
                DDS.to_max = -1.0;
            }
            else
            {
                DDS.to_max = 1.0;
            }

            Ini_Parameter = new double[this.mProblem.NumOptParams];

            for (i = 0; i < this.mProblem.NumOptParams; i++)
            {
                if (this.mProblem.List_OptParameter[i].Xn < 0 | this.mProblem.List_OptParameter[i].Xn > 1)
                {
                    throw new Exception("Ini parameter " + i + " not between 0 and 1");
                }
                Ini_Parameter[i] = this.mProblem.List_OptParameter[i].Xn;
            }

            if (this.mSettings.DDS.RandomStartparameters)
            {
                //Zufällige Startparameter
                DDS.initialize(this.mSettings.DDS.R_val, this.mSettings.DDS.MaxIter, this.mProblem.NumOptParams);
            }
            else
            {
                //Vorgegebene Startparameter
                DDS.initialize(this.mSettings.DDS.R_val, this.mSettings.DDS.MaxIter, this.mProblem.NumOptParams, Ini_Parameter);
            }

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //Ini objective function evaluations
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            for (i = 0; i < DDS.ini_fevals; i++)
            {
                //Stop?
                if (this.stopped) return;

                run += 1;

                Current_Parameter = DDS.ini_solution_candidate();

                ind = new IHWB.EVO.Common.Individuum_PES("DDS", run + 1); //+1 wegen Evaluierung der Starwerte vor Optimierungsbeginn

                //OptParameter ins Individuum kopieren
                //------------------------------------
                for (j = 0; j < ind.OptParameter.Length; j++)
                {
                    ind.OptParameter[j].Xn = Current_Parameter[j];
                }

                //Evaluierung
                if (this.myAppType == IHWB.EVO.Common.Constants.ApplicationTypes.Sim)
                {
                    //Evaluierung des Simulationsmodells
                    //----------------------------------
                    SIM_Eval_is_OK = this.Sim1.Evaluate(ref ind, true);

                    //TODO: Evaluierungsfehler verarbeiten

                    System.Windows.Forms.Application.DoEvents();

                    //Lösung im TeeChart einzeichnen
                    //------------------------------
                    serie = this.Hauptdiagramm1.getSeriesPoint("DDS", "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 3, false);
                    serie.Add(run, ind.PrimObjectives[0], run.ToString());
                }
                else
                {
                    //Evaluierung des Testproblems
                    //----------------------------
                    this.Testproblem.Evaluate(ref ind, 0, ref this.Hauptdiagramm1);
                }

                System.Windows.Forms.Application.DoEvents();

                //Bestwertspeicher und Searchhistorie aktualisieren
                //-------------------------------------------------
                if (run == 1)
                {
                    DDS.ini_Fbest(ind.PrimObjectives[0]);
                }
                else
                {
                    DDS.update_Fbest(ind.PrimObjectives[0]);
                }
                DDS.update_search_historie(ind.PrimObjectives[0], run - 1);

                //TODO: Verlaufsanzeige (this.mProgress)
            }
            DDS.track_ini();

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //Ende ini objective function evaluations
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //Code below is now the DDS algorithm as presented in Figure 1 of 
            //Tolson and Shoemaker (2007) 
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //start the OUTER DDS ALGORITHM LOOP for remaining allowble function evaluations (ileft)
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            for (i = 1; i <= DDS.ileft; i++)
            {
                //Stop?
                if (this.stopped) return;

                run += 1;

                Current_Parameter = DDS.determine_DV(i);

                ind = new IHWB.EVO.Common.Individuum_PES("DDS", run + 1);
                //OptParameter ins Individuum kopieren
                //------------------------------------
                for (j = 0; j < ind.OptParameter.Length; j++)
                {
                    ind.OptParameter[j].Xn = Current_Parameter[j];
                }

                //Evaluierung
                if (this.myAppType == IHWB.EVO.Common.Constants.ApplicationTypes.Sim)
                {
                    //Evaluierung des Simulationsmodells
                    //----------------------------------
                    SIM_Eval_is_OK = this.Sim1.Evaluate(ref ind, true);

                    //TODO: Evaluierungsfehler verarbeiten

                    System.Windows.Forms.Application.DoEvents();

                    //Lösung im TeeChart einzeichnen
                    //------------------------------
                    serie = this.Hauptdiagramm1.getSeriesPoint("DDS", "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 3, false);
                    serie.Add(run, ind.PrimObjectives[0], run.ToString());
                }
                else
                {
                    //Evaluierung des Testproblems
                    //----------------------------
                    this.Testproblem.Evaluate(ref ind, 0, ref this.Hauptdiagramm1);
                }

                System.Windows.Forms.Application.DoEvents();

                DDS.update_Fbest(ind.PrimObjectives[0]);

                DDS.update_search_historie(ind.PrimObjectives[0], run - 1);

                //TODO: Verlaufsanzeige (this.mProgress)
            }

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //ends OUTER DDS ALGORITHM LOOP
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        }

        public void Stoppen()
        {
            this.stopped = true;
        }
    }
}
