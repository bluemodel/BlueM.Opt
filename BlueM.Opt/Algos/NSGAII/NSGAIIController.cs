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
using BlueM.Opt.Apps;
using BlueM.Opt.Common;
using BlueM.Opt.Diagramm;
using MOEA.AlgorithmModels;
using MOEA.ComponentModels;
using MOEA.ComponentModels.SolutionModels;
using System.Collections.Generic;
using System.Drawing;
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
        private Settings mSettings;
        private Progress mProgress;
        private Hauptdiagramm Hauptdiagramm1;
        private bool stopped;

        /// <summary>
        /// Initialize the NSGAII controller
        /// </summary>
        /// <param name="inputProblem">the problem</param>
        /// <param name="inputSettings">the settings</param>
        /// <param name="inputProgress">the progress indicator</param>
        /// <param name="inputHauptdiagramm">the main chart</param>
        public void Init(ref Problem inputProblem, ref Settings inputSettings, ref Progress inputProgress, ref Hauptdiagramm inputHauptdiagramm)
        {
            //store objects
            this.mSettings = inputSettings;
            this.mProgress = inputProgress;

            this.mMOOProblem = new MOOProblem(inputProblem);
            this.Hauptdiagramm1 = inputHauptdiagramm;
        }

        /// <summary>
        /// Inialize the controller for simulation apps
        /// </summary>
        /// <param name="inputSim">sim instance</param>
        public void InitApp(ref BlueM.Opt.Apps.Sim inputSim)
        {
            this.mMOOProblem.Sim1 = inputSim;
            this.mMOOProblem.AppType = Constants.ApplicationTypes.Sim;
        }

        /// <summary>
        /// Initialize the controller for testproblem apps
        /// </summary>
        /// <param name="inputTestproblem">test problem instance</param>
        public void InitApp(ref BlueM.Opt.Apps.Testproblem inputTestproblem)
        {
            this.mMOOProblem.Testproblem1 = inputTestproblem;
            this.mMOOProblem.AppType = Constants.ApplicationTypes.Testproblems;
        }

        /// <summary>
        /// Starts the optimization process
        /// </summary>
        public void Start()
        {
            this.stopped = false;

            int i, j;

            //initialize NSGAII algorithm
            NSGAII<ContinuousVector> algorithm = new NSGAII<ContinuousVector>(this.mMOOProblem);
            //TODO: get this from settings
            algorithm.PopulationSize = 100;
            algorithm.Initialize();

            //initialize progress bar
            this.mProgress.Initialize(0, 0, 1000, algorithm.PopulationSize);

            //add event handler
            algorithm.SolutionEvaluated += Algorithm_SolutionEvaluated;

            //run optimization
            while (!algorithm.IsTerminated && !this.stopped)
            {
                algorithm.Evolve();

                this.mProgress.NextGen();
                this.mProgress.iNachf = 0;

                Log.AddMessage(Log.levels.info, $"Current Generation: {algorithm.CurrentGeneration}");
                Log.AddMessage(Log.levels.info, $"Size of Archive: {algorithm.NondominatedArchiveSize}");

                //process pareto front
                NondominatedPopulation<ContinuousVector> paretoFront = algorithm.NondominatedArchive;
                List<Individuum> pop = new List<Individuum>();
                i = 0;
                foreach (ContinuousVector solution in paretoFront.Solutions)
                {
                    var ind = new Individuum_PES("NSGAII", i + 1); //TODO: this is not the actual ID
                    for (j = 0; j < mMOOProblem.GetObjectiveCount(); j++)
                    {
                        ind.Objectives[j] = solution.FindObjectiveAt(j);
                    }
                    for (j = 0; j < mMOOProblem.GetDimensionCount(); j++)
                    {
                        ind.OptParameter[j].Xn = solution[j];
                    }
                    pop.Add(ind);
                    i++;
                }
                if (this.mMOOProblem.AppType == Constants.ApplicationTypes.Sim)
                {
                    //store in database
                    this.mMOOProblem.Sim1.OptResult.setSekPop(pop.ToArray(), algorithm.CurrentGeneration);
                    //Umweg über Sim1.OptResult gehen, weil es keine Individuum-IDs gibt (#177)
                    this.Hauptdiagramm1.ZeichneSekPopulation(this.mMOOProblem.Sim1.OptResult.getSekPop());
                }
                else {
                    this.Hauptdiagramm1.ZeichneSekPopulation(pop.ToArray());
                }

                Application.DoEvents();
            }
            //TODO: do something with this final solution?
            ContinuousVector finalSolution = algorithm.GlobalBestSolution;
        }

        private void Algorithm_SolutionEvaluated(ContinuousVector solution, int solution_index)
        {
            this.mProgress.NextNachf();

            //convert solution to individuum
            Individuum ind = new Individuum_PES("NSGAII", solution_index + 1);
            for (int i = 0; i < this.mMOOProblem.GetObjectiveCount(); i++)
            {
                ind.Objectives[i] = solution.FindObjectiveAt(i);
            }

            //TODO: store solution in database here ?

            //paint solution
            if (this.mMOOProblem.AppType == Constants.ApplicationTypes.Sim)
            {
                //paint sim solution
                //TODO: handle invalid solutions
                this.Hauptdiagramm1.ZeichneIndividuum(ind, 0, 0, 0, solution_index + 1, Color.Orange);
            }
            else
            {
                //paint testproblem solution
                this.mMOOProblem.Testproblem1.PaintSolution(ind, 0, ref this.Hauptdiagramm1);
            }
        }

        public void Stoppen()
        {
            this.stopped = true;
        }
    }
}
