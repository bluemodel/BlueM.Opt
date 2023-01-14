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
    public abstract class Indicators
    {
        //Private Variablen
        protected int _dim;  /* number of objectives */
        protected bool[] _obj;  /* _obj[i] = 0 means objective i is to be minimized */
        protected int _method;  /* 0 = no reference set, 1 = with respect to reference set */
        protected double[] _nadir;  /* reference point for hypervolume calculation */
        protected struct front
        {
            public double[] point;
        }
        protected front[] _dataset; /*front to be analysed*/
        protected front[] _referenceset; /*Referenceset*/
        protected bool _disposed = false;

        //Abstrakte Methoden
        public abstract double calc_indicator();
        public abstract void update_dataset(double[,] dataset);
        public abstract void Dispose();

        //Eigenschaften
        public int dimension
        {
            get{return (_dim);}
            set{_dim = value;}
        }
        public bool[] minmax
        {
            get{return (_obj);}
            set{_obj = value;}
        }

        public int method
        {
            get{return (_method);}
            set
            {
                if (value < 0 || value > 1)
                    throw new System.Exception("Only method 0 or 1 implemented");
                else
                    _method = value;
            }
        }

        public double[] nadir
        {
            get{return (_nadir);}
            set{_nadir = value;}
        }

        public double[,] dataset
        {
            get
            {
                int i;
                int j;
                double[,] tempdataset;
                i = _dataset.GetLength(0);
                j = _dataset[0].point.GetLength(0);
                tempdataset = new double[i, j];
                for (i = 0; i < tempdataset.GetLength(0); i++)
                {
                    for (j = 0; j < tempdataset.GetLength(1); j++)
                    {
                        tempdataset[i, j] = _dataset[i].point[j];
                    }
                }
                return (tempdataset);
            }
            set
            {
                int i;
                int j;
                _dataset = new front[value.GetLength(0)];
                for (i = 0; i < value.GetLength(0); i++)
                {
                    _dataset[i].point = new double[value.GetLength(1)];
                }

                for (i = 0; i < value.GetLength(0); i++)
                {
                    for (j = 0; j < value.GetLength(1); j++)
                    {
                        _dataset[i].point[j] = value[i, j];
                    }
                }
            }
        }

        public double[,] referenceset
        {
            get
            {
                int i;
                int j;
                double[,] tempdataset;
                i = _referenceset.GetLength(0);
                j = _referenceset[0].point.GetLength(0);
                tempdataset = new double[i, j];
                for (i = 0; i < tempdataset.GetLength(0); i++)
                {
                    for (j = 0; j < tempdataset.GetLength(1); j++)
                    {
                        tempdataset[i, j] = _referenceset[i].point[j];
                    }
                }
                return (tempdataset);
            }
            set
            {
                int i;
                int j;
                _referenceset = new front[value.GetLength(0)];
                for (i = 0; i < value.GetLength(0); i++)
                {
                    _referenceset[i].point = new double[value.GetLength(1)];
                }

                for (i = 0; i < value.GetLength(0); i++)
                {
                    for (j = 0; j < value.GetLength(1); j++)
                    {
                        _referenceset[i].point[j] = value[i, j];
                    }
                }
            }
        } 
    }
}
