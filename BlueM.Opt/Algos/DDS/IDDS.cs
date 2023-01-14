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
