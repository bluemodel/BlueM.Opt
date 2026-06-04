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
using MOEA.ComponentModels.SolutionModels;
using MOEA.Core.ComponentModels;
using MOEA.Core.ProblemModels;
using BlueM.Opt.Common;

namespace BlueM.Opt.Algos.NSGAII
{
    internal class MOOProblem : IMOOProblem
    {
        private Problem problem;
        public Constants.ApplicationTypes AppType;
        public BlueM.Opt.Apps.Sim Sim1;
        public BlueM.Opt.Apps.Testproblem Testproblem1;

        private int run;

        public MOOProblem(Problem problem)
        {
            this.problem = problem;
            this.run = 0;
        }

        public double[] CalcObjective(MOOSolution s)
        {
            run++;

            Individuum ind = new Individuum_PES("NSGAII", run + 1); //+1 wegen Evaluierung der Startwerte vor Optimierungsbeginn

            //convert solution to individuum
            ContinuousVector x = (ContinuousVector)s;
            for (int j = 0; j < ind.OptParameter.Length; j++)
            {
                ind.OptParameter[j].Xn = x[j];
            }
            //evaluation
            if (this.AppType == Constants.ApplicationTypes.Sim)
            {
                //evaluate sim problem
                bool SIM_Eval_is_OK = this.Sim1.Evaluate(ref ind, true);

                //TODO: handle evaluation errors
            }
            else
            {
                //evaluate test problem
                this.Testproblem1.Evaluate(ref ind);
            }

            return ind.PrimObjectives;
        }

        public int GetDimensionCount()
        {
            return this.problem.NumOptParams;
        }

        public int GetObjectiveCount()
        {
            return this.problem.NumPrimObjective;
        }

        public double GetLowerBound(int dimension_index)
        {
            return 0;
        }

        public double GetUpperBound(int dimension_index)
        {
            return 1;
        }

        public bool IsFeasible(MOOSolution s)
        {
            //TODO: evaluate constraints
            return true;
        }

        public bool IsMaximizing()
        {
            return false;
        }
    }
}
