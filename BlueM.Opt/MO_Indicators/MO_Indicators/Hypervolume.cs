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

namespace BlueM.Opt.MO_Indicators
{
    public class Hypervolume : Indicators
    {
        //**************************************
        //interne Felder
        //**************************************

        //**************************************
        //Eigenschaften
        //**************************************

        //**************************************
        //Konstruktoren
        //**************************************
        
        //Standardkonstruktor
        public Hypervolume()
        {
            this.dimension = 0;
            this.minmax = null;
            this.method = 0;
            this.nadir = null;
            this._dataset = null;
        }
        //1. Überladung - Methode 0, kein Dataset und keine Referenzfront erforderlich, kein nadir, kein minmax erforderlich
        public Hypervolume(int dimension)
        {
            int i;
            this.dimension = dimension;
            this.minmax = new bool[dimension];
            this.nadir = new double[dimension];

            for (i = 0; i < dimension; i++)
            {
                this.minmax[i] = false;
                this.nadir[i] = 0;
            }

            this.method = 0;
            this._dataset = null;
        }
        //2. Überladung - Methode 0, kein Dataset und keine Referenzfront erforderlich
        public Hypervolume(bool[] minmax, double[] nadir)
        {
            this.dimension = nadir.GetLength(0);
            this.minmax = minmax;
            this.method = 0;
            this.nadir = nadir;
            this._dataset = null;
        }
        //3. Überladung - Methode 0, keine Referenzfront erforderlich
        public Hypervolume(bool[] minmax, double[] nadir, double[,] dataset)
        {
            this.dimension = dataset.GetLength(1);
            this.minmax = minmax;
            this.method = 0;
            this.nadir = nadir;
            this.dataset = dataset;
        }
        //4. Überladung - Methode 1, Referenzfront erforderlich
        public Hypervolume(bool[] minmax, double[] nadir, double[,] dataset, double[,] referenceset)
        {
            this.dimension = dataset.GetLength(1);
            this.minmax = minmax;
            this.method = 1;
            this.nadir = nadir;
            this.dataset = dataset;
            this.referenceset = referenceset;
        }

        //**************************************
        //Destruktor
        //**************************************
        public override void Dispose()
        {
            if (!this._disposed)
            {
                GC.SuppressFinalize(this);
            }
        } 

        ~Hypervolume()
        {
            Dispose();
        }

        //**************************************
        //öffentliche Methoden
        //**************************************
        public override double calc_indicator()
        {
            int referenceset_size;  /* number of points in the reference set */
            int dataset_size;  /* number of points associated with the current run */
            double ref_set_value = 0;
            double ind_value;

            if (_method == 1)
            {
                referenceset_size = _referenceset.GetLength(0);
                ref_set_value = calc_ind_value(ref _referenceset, referenceset_size);
            }
            else
            {
                _referenceset = null;
                referenceset_size = 0;
            }
            dataset_size = _dataset.GetLength(0);
            ind_value = calc_ind_value(ref _dataset, dataset_size);
            if (_method == 1)
                return ref_set_value - ind_value;
            else
                return -ind_value;
        }

        public override void update_dataset(double[,] dataset)
        {
            this.dataset = dataset;
        }


        //**************************************
        //Interne Methoden
        //**************************************

        bool dominates(double[] point1, double[] point2, int no_objectives)
        /*returns true if 'point1' dominates 'points2' with respect to the
          first 'no_objectives' objectives*/
        {
            int i;
            bool better_in_any_objective;
            bool worse_in_any_objective;

            better_in_any_objective = false;
            worse_in_any_objective = false;

            for (i = 0; i < no_objectives && !worse_in_any_objective; i++)
            {
                if (point1[i] > point2[i])
                    better_in_any_objective = true;
                else if (point1[i] < point2[i])
                    worse_in_any_objective = true;
            }

            return (!worse_in_any_objective && better_in_any_objective);
        }

        bool weakly_dominates(double[] point1, double[] point2, int no_objectives)
        /* returns true if 'point1' weakly dominates 'points2' with respect to the
           to the first 'no_objectives' objectives*/
        {
            int i;
            bool worse_in_any_objective;

            worse_in_any_objective = false;

            for (i = 0; i < no_objectives && !worse_in_any_objective; i++)
                if (point1[i] < point2[i])
                    worse_in_any_objective = true;
            return (!worse_in_any_objective);
        }

        void swap(ref front[] front, int i, int j)
        {
            int k;
            double temp;

            for (k = 0; k < _dim; k++)
            {
                temp = front[i].point[k];
                front[i].point[k] = front[j].point[k];
                front[j].point[k] = temp;
            }
        }

        int filter_nondominated_set(ref front[] front, int no_points, int no_objectives)
        /* all nondominated points regarding the first 'no_objectives' dimensions
           are collected; the points 0..no_points-1 in 'front' are
           considered; the points in 'front' are resorted, such that points
           [0..n-1] represent the nondominated points; n is returned
        */
        {
            int i, j;
            int n;

            n = no_points;
            i = 0;
            while (i < n)
            {
                j = i + 1;
                while (j < n)
                {
                    if (dominates(front[i].point, front[j].point, no_objectives))
                    {
                        /* remove point 'j' */
                        n--;
                        swap(ref front, j, n);
                    }
                    else if (dominates(front[j].point, front[i].point, no_objectives))
                    {
                        /* remove point 'i'; ensure that the point copied to index 'i'
                           is considered in the next outer loop (thus, decrement i) */
                        n--;
                        swap(ref front, i, n);
                        i--;
                        break;
                    }
                    else
                        j++;
                }
                i++;
            }
            return n;
        }

        double surface_unchanged_to(front[] front, int  no_points, int objective)
        /* calculate next value regarding dimension 'objective'; consider
       points 0..no_points-1 in 'front'
        */
        {
            int i;
            double min, value;

            if (front.GetLength(0) < 1)
                throw new System.Exception("run-time error");

            min = front[0].point[objective];
            for (i = 1; i < no_points; i++)
            {
                value = front[i].point[objective];
                if (value < min) min = value;
            }
            return min;
        }

        int reduce_nondominated_set(ref front[] front, int no_points, int objective, double threshold)
        /* remove all points which have a value <= 'threshold' regarding the
           dimension 'objective'; the points [0..no_points-1] in 'front' are
           considered; 'front' is resorted, such that points [0..n-1] represent
           the remaining points; 'n' is returned
        */
        {
            int n;
            int i;

            n = no_points;
            for (i = 0; i < n; i++)
                if (front[i].point[objective] <= threshold)
                {
                    n--;
                    swap(ref front, i, n);
                }

            return n;
        }

        double calc_hypervolume(front[] front, int no_points, int no_objectives)
        {
            int n;
            double volume, distance;

            volume = 0;
            distance = 0;
            n = no_points;
            while (n > 0)
            {
                int no_nondominated_points;
                double temp_vol, temp_dist;

                no_nondominated_points = filter_nondominated_set(ref front, n, no_objectives - 1);
                temp_vol = 0;
                if (no_objectives < 3)
                {
                    if(no_nondominated_points < 1)
                    throw new System.Exception("run-time error");
                    temp_vol = front[0].point[0];
                }
                else
                    temp_vol = calc_hypervolume(front, no_nondominated_points,
                                no_objectives - 1);
                temp_dist = surface_unchanged_to(front, n, no_objectives - 1);
                volume += temp_vol * (temp_dist - distance);
                distance = temp_dist;
                n = reduce_nondominated_set(ref front, n, no_objectives - 1, distance);
            }

            return volume;
        }



        double calc_ind_value(ref front[] a, int  size_a)
        {
            int i, k;
            front[] tempfront;
            bool update_Nadir = false;

            tempfront = new front[a.GetLength(0)];
            for (i = 0; i < a.GetLength(0); i++)
            {
                tempfront[i].point = new double[a[0].point.GetLength(0)];
            }

            do
            {
                update_Nadir = false;
                for (i = 0; i < size_a; i++)
                {
                    for (k = 0; k < _dim; k++)
                    {
                        if (_obj[k]) //maximieren
                        {
                            tempfront[i].point[k] = a[i].point[k] - _nadir[k];
                            if (tempfront[i].point[k] < 0)
                            {
                                _nadir[k] = a[i].point[k];
                                update_Nadir = true;
                                break;
                            }
                            //throw new System.Exception("error in data or reference set file");
                        }
                        else //minimieren
                        {
                            tempfront[i].point[k] = _nadir[k] - a[i].point[k];
                            if (tempfront[i].point[k] < 0)
                            {
                                _nadir[k] = a[i].point[k];
                                update_Nadir = true;
                                break;
                            }
                            //throw new System.Exception("error in data or reference set file");
                        }
                    }
                    if (update_Nadir) break;
                }
            }
            while (update_Nadir);

            a = tempfront;
           
            /* calculate indicator values */
            return calc_hypervolume(a, size_a,_dim);
        }

    }
}
