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
using BlueM.Opt.Common;
using BlueM.Opt.Diagramm;
using MOEA.AlgorithmModels;
using MOEA.Benchmarks;
using MOEA.ComponentModels;
using MOEA.ComponentModels.SolutionModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlueM.Opt.Algos.NSGAII
{
    public class NSGAIIController: BlueM.Opt.Algos.IController
    {
        /// <summary>
        /// Multithreading Support
        /// </summary>
        public bool MultithreadingSupported
        {
            get { return false; }
        }

        private MOOProblem mMOOProblem;
        private BlueM.Opt.Common.Settings mSettings;
        private BlueM.Opt.Common.Progress mProgress;
        private bool stopped;

        /// <summary>
        /// Initialisiert den NSGAII-Controller und übergibt alle erforderlichen Objekte
        /// </summary>
        /// <param name="inputProblem">das Problem</param>
        /// <param name="inputSettings">die Einstellungen</param>
        /// <param name="inputProgress">der Verlauf</param>
        /// <param name="inputHauptdiagramm">das Hauptdiagramm</param>
        public void Init(ref BlueM.Opt.Common.Problem inputProblem, ref BlueM.Opt.Common.Settings inputSettings, ref BlueM.Opt.Common.Progress inputProgress, ref BlueM.Opt.Diagramm.Hauptdiagramm inputHauptdiagramm)
        {
            //Objekte übergeben
            this.mSettings = inputSettings;
            this.mProgress = inputProgress;

            this.mMOOProblem = new MOOProblem(inputProblem);
            this.mMOOProblem.Hauptdiagramm1 = inputHauptdiagramm;
        }

        /// <summary>
        /// Initialisiert den Controller für Sim-Anwendungen
        /// </summary>
        /// <param name="inputSim">Sim-Objekt</param>
        public void InitApp(ref BlueM.Opt.Apps.Sim inputSim)
        {
            this.mMOOProblem.Sim1 = inputSim;
            this.mMOOProblem.myAppType = BlueM.Opt.Common.Constants.ApplicationTypes.Sim;
        }

        /// <summary>
        /// Initialisiert den Controller für Testprobleme
        /// </summary>
        /// <param name="inputTestproblem">Testproblem-Objekt</param>
        public void InitApp(ref BlueM.Opt.Apps.Testprobleme inputTestproblem)
        {
            this.mMOOProblem.Testproblem = inputTestproblem;
            this.mMOOProblem.myAppType = BlueM.Opt.Common.Constants.ApplicationTypes.Testproblems;
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

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //Initialize
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            NSGAII<ContinuousVector> algorithm = new NSGAII<ContinuousVector>(this.mMOOProblem);

            //TODO: get this from settings
            algorithm.PopulationSize = 100;

            this.mProgress.Initialize(0, 0, 1000, 0);

            algorithm.Initialize();

            //TODO: add event handler
            //algorithm.SolutionEvaluated += Algorithm_SolutionEvaluated;

            //run optimization
            while (!algorithm.IsTerminated && !this.stopped)
            {
                algorithm.Evolve();
                this.mProgress.NextGen();
                Console.WriteLine("Current Generation: {0}", algorithm.CurrentGeneration);
                Console.WriteLine("Size of Archive: {0}", algorithm.NondominatedArchiveSize);

                //draw pareto front
                NondominatedPopulation<ContinuousVector> paretoFront = algorithm.NondominatedArchive;
                List<Individuum> pop = new List<Individuum>();
                i = 0;
                foreach (ContinuousVector s in paretoFront.Solutions)
                {
                    var ind = new Individuum_PES("NSGAII", i + 1); //TODO: this is not the actual ID
                    for (j = 0; j < mMOOProblem.GetObjectiveCount(); j++)
                    {
                        ind.Objectives[j] = s.FindObjectiveAt(j);
                    }
                    pop.Add(ind);
                    i++;
                }
                this.mMOOProblem.Hauptdiagramm1.ZeichneSekPopulation(pop.ToArray());

                Application.DoEvents();
            }
            ContinuousVector finalSolution = algorithm.GlobalBestSolution;
            //NondominatedPopulation<ContinuousVector> paretoFront = algorithm.NondominatedArchive;

            //var serie = this.mMOOProblem.Hauptdiagramm1.getSeriesPoint("NSGAII", "Orange", Steema.TeeChart.Styles.PointerStyles.Circle, 3, false);
            //serie.Add(run, ind.PrimObjectives[0], run.ToString());

            //TODO: Verlaufsanzeige (this.mProgress)
        }

        private void Algorithm_SolutionEvaluated(ContinuousVector solution, int solution_index)
        {
            throw new NotImplementedException();
        }

        public void Stoppen()
        {
            this.stopped = true;
        }
    }
}
