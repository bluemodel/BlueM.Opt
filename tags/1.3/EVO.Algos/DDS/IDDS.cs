namespace modelEAU.DDS
{
    /// <summary>
    /// Interface of the DDS-class
    /// </summary>
    public interface IDDS
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
