using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace modelEAU.DDS
{
    /// <summary>
    /// Interface of the DSS-class
    /// </summary>
    public interface IDSS
    {
        void initialize(double r_val_in, int maxiter_in, int num_dec_in, double[] ini_par);
        void initialize(double r_val_in, int maxiter_in, int num_dec_in);
        double[] ini_solution_candidate();
        void ini_Fbest(double fvalue);
        void update_Fbest(double fvalue);
        void update_search_historie(double fvalue, int index);
        void track_ini();
        double[] determine_DV(int i);
    }
}
