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
//Dynamically dimensioned Search (DDS) version 1.1 algorithm by Bryan Tolson
//C# version based on the Fortran version (original was coded in Matlab)
//Fortran version was coded in Nov 2005 by Bryan Tolson
//This version is a reengineered version coded in Nov 2008 by Dirk Muschalla

//DDS is an n-dimensional continuous global optimization algorithm.  It is coded as a 
//minimizer but built into the code is a hidden transformation to make it capable of 
//solving a maximization problem.  In a maximization problem, the algorithm minimizes 
//the negative of the objective function F (-1*F).  User specifies in inputs 
//whether it is a max or a min problem.

//REFERENCE FOR THIS ALGORITHM:  
//Tolson, B. A., and C. A. Shoemaker (2007), Dynamically dimensioned search algorithm 
//for computationally efficient watershed model calibration, Water Resour. Res., 43, 
//W01413, doi:10.1029/2005WR004723.

    public class DDS : modelEAU.DDS.IDDS
    {
        # region declarations
        
        //declarations
        private int _ini_fevals;

        private int _ini_fevals_typ; // <-INPUT. random = 1, original = 2

        /// <summary>
        /// DDS perturbation parameter
        /// </summary>
        private double _r_val; // <-INPUT.  DDS perturbation parameter = std dev/DV range to 
        //determine variance of normal DV perturbation. Scales all DVs to be same 
        //perturbation size realtive to their DV range.  Default is 0.2
        
        /// <summary>
        /// max # of iterations or obj. function calls
        /// </summary>
        private int _maxiter; // <-INPUT. the max # of iterations or obj. function calls 
	    //in each optimization trial.  DDS always runs to maximum iterations.
        
        private double _to_max; // <-INPUT.   -1.0 if maximization, 1.0 if minimization --> when
        //algorithm called it acts as a minimizer.  This input seemlessly allows 
        //users to max or min their original objective function.
        
        private int _num_dec; // <-INPUT.  integer number of decision variables to help read DV bounds file
        //& allocate variables efficiently. Must match the data in the bounds file.

        private int _ileft;

        private double[] s_min, s_max, ini_soln, sbest, stest, Ftests, Fbests, initials;
        private double[,] stests;
        private double Fbest;
        private int[] f_count;
        internal System.Random ranval = new System.Random();
        

        #endregion

        #region attributes
        
        //attributes
        public double r_val
        {
            get
            {
                return (_r_val);
            }
            set
            {
                if (value > 0.0 && value <= 1.0) _r_val = value;
                else CreateAndThrowException("Error! DDS perturbation parameter (0.0, 1.0]!");
            }
        }

        public int maxiter
        {
            get
            {
                return (_maxiter);
            }
            set
            {
                if (value > 5 && value <= 1000000) _maxiter = value;
                else CreateAndThrowException("Error! Max# of function evals in range 6-1000000!");
            }
        }

        public double to_max
        {
            get
            {
                return (_to_max);
            }
            set
            {
                if (value == 1.0 || value == -1.0) _to_max = value;
                else CreateAndThrowException("Error! -1.0 or 1.0 for to_max input!");
            }
        }

        public int num_dec
        {
            get
            {
                return (_num_dec);
            }
            set
            {
                if (value > 1) _num_dec = value;
                else CreateAndThrowException("Error! >= 1 decision variables!");
            }
        }

        public int ini_fevals_typ
        {
            get
            {
                return (_ini_fevals_typ);
            }
            set
            {
                if (value >0  && value < 3) _ini_fevals_typ = value;
                else CreateAndThrowException("Error! >= 1 or 2 for ini_fevals_typ input!");
            }
        }

        public int ini_fevals
        {
            get { return (_ini_fevals); }
        }
        
        public int ileft
        {
            get { return (_ileft); }
        }


        #endregion

        #region constructor

        /// <summary>
        /// Standard constructor
        /// </summary>
        public DDS()
        {
            _r_val = 0.2;
            _to_max = 1.0; //minimize
            _ini_fevals_typ = 2; //original
            _ini_fevals = 1;
        }

        #endregion

        #region public methods
        /// <summary>
        /// see Interface (IDDS)
        /// </summary>
        public void initialize(double r_val_in, int maxiter_in, int num_dec_in,
                               double[] initials_in)
        {
            r_val = r_val_in;
            maxiter = maxiter_in;
            num_dec = num_dec_in;

            DDS_allocate();

            //Check
            if (initials_in.GetUpperBound(0) != _num_dec - 1)
            {CreateAndThrowException("Dimension of initials Array <> num_dec!");}
            initials = (double[])initials_in.Clone();
            ini_fevals_typ = 2;
            DDS_ini_fevals();

            DDS_bounds();
        }

        /// <summary>
        /// see Interface (IDDS)
        /// </summary>
        public void initialize(double r_val_in, int maxiter_in, int num_dec_in)
        {
            r_val = r_val_in;
            maxiter = maxiter_in;
            num_dec = num_dec_in;

            DDS_allocate();

            ini_fevals_typ = 1;
            DDS_ini_fevals();

            DDS_bounds();
        }

        /// <summary>
        /// see Interface (IDDS)
        /// </summary>
        public double[] ini_solution_candidate()
        {
            if (_ini_fevals_typ == 1)
            {
                for (int i = 0; i < _num_dec; i++)
                {
                    stest[i] = s_min[i] + ranval.NextDouble() * (s_max[i] - s_min[i]);
                }
            }
            return stest;
        }

        /// <summary>
        /// see Interface (IDDS)
        /// </summary>
        public void ini_Fbest(double fvalue)
        {
            double Ftest;

            Ftest = to_max * fvalue;  // to_max handles min (=1) and max (=-1) problems
            Fbest = Ftest;
            sbest = (double[])stest.Clone();
        }

        /// <summary>
        /// see Interface (IDDS)
        /// </summary>
        public void update_Fbest(double fvalue)
        {
            double Ftest;

            Ftest = to_max * fvalue;  // to_max handles min (=1) and max (=-1) problems
            if (Ftest <= Fbest)
            {
                Fbest = Ftest;
                sbest = (double[])stest.Clone();
            }
        }

        /// <summary>
        /// see Interface (IDDS)
        /// </summary>
        public void update_search_historie(double fvalue, int index)
        {
		    
            f_count[index]=index+1;
		    Ftests[index]= fvalue;  //candidate solution objective function value (untransformed)
		    Fbests[index]=to_max * Fbest;  //best current solution objective function value (untransformed)
		    for (int j =0; j < _num_dec; j++)
            {
                stests[index, j]=stest[j];           //candidate solution (decision variable values)
            }
        }

        /// <summary>
        /// see Interface (IDDS)
        /// </summary>
        public void  track_ini()
        {
            ini_soln = (double[])sbest.Clone(); //optional part of code to track where DDS really started
        }

        public double[] determine_DV(int i)
        {
            double Pn;
            double new_value;
            int dvn_count, dv;

            //Determine Decision Variable (DV) selected for perturbation:
            //probability each DV selected
		    Pn=1.0-Math.Log((double)i)/Math.Log((double)_ileft); 
            // counter for how many DVs selected for perturbation
		    dvn_count=0;
            //define stest initially as best current solution
		    stest = (double[])sbest.Clone();  
		
		    for (int j=0; j < num_dec; j++)
            {
			    //get one uniform random number
                //jth DV selected for perturbation
                if (ranval.NextDouble() < Pn) 
                {
                    dvn_count += 1;
			        //call 1-D perturbation function to get new DV value (new_value):
                    new_value = neigh_value(sbest[j], s_min[j], s_max[j], r_val);
                    //note that r_val is the value of the DDS r perturbation size parameter (0.2 by default)
			        //change relevant DV value in stest
                    stest[j] = new_value;
			    }
		    }
            //no DVs selected at random, so select ONE
	        if (dvn_count == 0)
            {
                //get one uniform random number
                //index for one DV 
			    dv =(int)Math.Ceiling(((double)num_dec) * ranval.NextDouble())-1;
                //call 1-D perturbation function to get new DV value (new_value):
                new_value = neigh_value(sbest[dv], s_min[dv], s_max[dv], _r_val);
                //change relevant DV value in stest
                stest[dv] = new_value; 
            }
            return stest;
        }
            

        #endregion

        #region private methods

        /// <summary>
        /// Allocate all DDS array variables
        /// </summary>
        private void DDS_allocate()
        {
            s_min = new double[_num_dec];
            s_max = new double[_num_dec];    
            ini_soln = new double[_num_dec];
            sbest = new double[_num_dec]; 
            stest = new double[_num_dec];
            Ftests = new double[_maxiter];
            Fbests = new double[_maxiter];
            stests = new double[_maxiter,_num_dec];
            f_count = new int[_maxiter];
            initials = new double[_num_dec];
        }

        /// <summary>
        /// Sets decision variable bounds
        /// </summary>
        private void DDS_bounds()
        {
            for (int i = 0; i != _num_dec; i++)
            {
                s_min[i] = 0.0;
                s_max[i] = 1.0;
            }
        }

        private void DDS_ini_fevals()
        {
            if (_ini_fevals_typ == 2) 
            {
                _ini_fevals = 1; //only 1 function evaluation used to initialize DDS
                stest = (double[])initials.Clone(); // solution to evaluate
            }
            else //standard random initialization as described in Tolson & Shoemaker (2007).
            {
                _ini_fevals = Math.Max(5, Convert.ToInt32(0.005*_maxiter));
            }

            _ileft = _maxiter - _ini_fevals; //reduce number of fevals in DDS loop
	        //note that maxiter is the total allowable objective function evaluations 
            //to solve the problem
            if (ileft <= 0) CreateAndThrowException("Error! #Initialization samples >= Max # func evaluations.");
        }

        /// <summary>
        /// Purpose is to generate a neighboring decision variable value for a single
        /// decision variable value being perturbed by the DDS optimization algorithm.
        /// New DV value respects the upper and lower DV bounds.
        /// Coded by Bryan Tolson, Nov 2005, reengineered by Dirk Muschalla, Nov 2008.
        /// </summary>
        private double neigh_value(double x_cur, double x_min, double x_max, double r)
        //I/O variable definitions:
        //x_cur - current decision variable (DV) value
        //x_min - min DV value
        //x_max - max DV value
        //r  - the neighborhood perturbation factor
        //new_value - new DV variable value (within specified min and max)
        {
            //declarations
            double new_value;
            double Work1 = 0.0, Work2 = 0.0, Work3;
            double zvalue, x_range;
            //System.Random ranval = new System.Random();

            x_range = x_max - x_min;

            //Below returns a standard Gaussian random number based upon Numerical recipes gasdev and 
            //Marsagalia-Bray Algorithm

            Work3 = 2.0;
            while ((Work3 >= 1.0) || (Work3 == 0.0))
            {
                Work1 = 2.0 * ranval.NextDouble() - 1.0;
                Work2 = 2.0 * ranval.NextDouble() - 1.0;
                Work3 = Work1 * Work1 + Work2 * Work2;
            }
            Work3 = Math.Pow(((-2.0 * Math.Log(Work3)) / Work3), 0.5);  //natural log
            //pick one of two deviates at random (don't worry about trying to use both):
            if (ranval.NextDouble() < 0.5)
                zvalue = Work1 * Work3;
            else
                zvalue = Work2 * Work3;

            //calculate new decision variable value:
            new_value = x_cur + zvalue * r * x_range;
            //check new value is within DV bounds.  If not, bounds are reflecting.
            if (new_value < x_min)
            {
                new_value = x_min + (x_min - new_value);
                if (new_value > x_max) new_value = x_min;
                //if reflection goes past x_max then value should be x_min since 
                //without reflection the approach goes way past lower bound.  
                //This keeps x close to lower bound when x_cur is close to lower bound
                //Practically speaking, this should never happen with r values <0.3
            }
            else if (new_value > x_max)
            {
                new_value = x_max - (new_value - x_max);
                if (new_value < x_min) new_value = x_max;
                //if reflection goes past x_min then value should be x_max for same reasons as above
            }

            return new_value;
        }

        /// <summary>
        /// Error handling
        /// </summary>
        private void CreateAndThrowException(string error)
        {
            throw new Exception(error);
        }

        #endregion
    }
}
