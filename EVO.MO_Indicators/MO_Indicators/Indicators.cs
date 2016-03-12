/*
Copyright (c) BlueM Dev Group
Website: http://bluemodel.org

All rights reserved.

Released under the BSD-2-Clause License:

Redistribution and use in source and binary forms, with or without modification, 
are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list 
  of conditions and the following disclaimer.
* Redistributions in binary form must reproduce the above copyright notice, this list 
  of conditions and the following disclaimer in the documentation and/or other materials 
  provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT 
OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
--------------------------------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace ihwb.EVO.MO_Indicators
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
