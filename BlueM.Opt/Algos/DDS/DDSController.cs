/*
BlueM.Opt
Copyright (C) BlueM Dev Group
Website: <https://www.bluemodel.org>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see <https://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace modelEAU.DDS
{
    /// <summary>
    /// Kontrolliert den Ablauf des DDS
    /// </summary>
    public class DDSController : BlueM.Opt.Algos.IController
    {
        /// <summary>
        /// Multithreading Support
        /// </summary>
        public bool MultithreadingSupported
        {
            get {return false;}
        }

        private BlueM.Opt.Common.Problem mProblem;
        private BlueM.Opt.Common.Settings mSettings;
        private BlueM.Opt.Common.Progress mProgress;
        private BlueM.Opt.Diagramm.Hauptdiagramm Hauptdiagramm1;
        private BlueM.Opt.Apps.Sim Sim1;
        private BlueM.Opt.Apps.Testprobleme Testproblem;

        private BlueM.Opt.Common.Constants.ApplicationTypes myAppType;

        private bool stopped;

        /// <summary>
        /// Initialisiert den DDS-Controller und übergibt alle erforderlichen Objekte
        /// </summary>
        /// <param name="inputProblem">das Problem</param>
        /// <param name="inputSettings">die Einstellungen</param>
        /// <param name="inputProgress">der Verlauf</param>
        /// <param name="inputHauptdiagramm">das Hauptdiagramm</param>
        public void Init(ref BlueM.Opt.Common.Problem inputProblem, ref BlueM.Opt.Common.Settings inputSettings, ref BlueM.Opt.Common.Progress inputProgress, ref BlueM.Opt.Diagramm.Hauptdiagramm inputHauptdiagramm)
        {
            //Objekte übergeben
            this.mProblem = inputProblem;
            this.mSettings = inputSettings;
            this.mProgress = inputProgress;
            this.Hauptdiagramm1 = inputHauptdiagramm;
        }

        /// <summary>
        /// Initialisiert den Controller für Sim-Anwendungen
        /// </summary>
        /// <param name="inputSim">Sim-Objekt</param>
        public void InitApp(ref BlueM.Opt.Apps.Sim inputSim)
        {
            this.Sim1 = inputSim;
            this.myAppType = BlueM.Opt.Common.Constants.ApplicationTypes.Sim;
        }

        /// <summary>
        /// Initialisiert den Controller für Testprobleme
        /// </summary>
        /// <param name="inputTestproblem">Testproblem-Objekt</param>
        public void InitApp(ref BlueM.Opt.Apps.Testprobleme inputTestproblem)
        {
            this.Testproblem = inputTestproblem;
            this.myAppType = BlueM.Opt.Common.Constants.ApplicationTypes.Testproblems;
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
            BlueM.Opt.Common.Individuum ind;
            modelEAU.DDS.DDS DDS;
            bool SIM_Eval_is_OK;
            Steema.TeeChart.Styles.Series serie;

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //Initialize
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            Current_Parameter = new double[this.mProblem.NumOptParams];
            DDS = new modelEAU.DDS.DDS();

            if (this.mProblem.List_ObjectiveFunctions[0].Direction == BlueM.Opt.Common.Constants.EVO_DIRECTION.Maximization)
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

                ind = new BlueM.Opt.Common.Individuum_PES("DDS", run + 1); //+1 wegen Evaluierung der Starwerte vor Optimierungsbeginn

                //OptParameter ins Individuum kopieren
                //------------------------------------
                for (j = 0; j < ind.OptParameter.Length; j++)
                {
                    ind.OptParameter[j].Xn = Current_Parameter[j];
                }

                //Evaluierung
                if (this.myAppType == BlueM.Opt.Common.Constants.ApplicationTypes.Sim)
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

                ind = new BlueM.Opt.Common.Individuum_PES("DDS", run + 1);
                //OptParameter ins Individuum kopieren
                //------------------------------------
                for (j = 0; j < ind.OptParameter.Length; j++)
                {
                    ind.OptParameter[j].Xn = Current_Parameter[j];
                }

                //Evaluierung
                if (this.myAppType == BlueM.Opt.Common.Constants.ApplicationTypes.Sim)
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
