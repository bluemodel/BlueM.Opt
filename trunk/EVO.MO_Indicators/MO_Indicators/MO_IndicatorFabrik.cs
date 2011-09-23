/*
Copyright (c) 2011, ihwb, TU Darmstadt
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
    public class MO_IndicatorFabrik
    {
        public enum IndicatorsType { Hypervolume };

        public static Indicators GetInstance(IndicatorsType method)
        {
            if (method == IndicatorsType.Hypervolume)
                return new Hypervolume();
            else
                throw new IndexOutOfRangeException(); // should never get here
        }

        public static Indicators GetInstance(IndicatorsType method, int dimension)
        {
            if (method == IndicatorsType.Hypervolume)
                return new Hypervolume(dimension);
            else
                throw new IndexOutOfRangeException(); // should never get here
        }

        public static Indicators GetInstance(IndicatorsType method, bool[] minmax,
                                     double[] nadir)
        {
            if (method == IndicatorsType.Hypervolume)
                return new Hypervolume(minmax, nadir);
            else
                throw new IndexOutOfRangeException(); // should never get here
        }

        public static Indicators GetInstance(IndicatorsType method, bool[] minmax, 
                                             double[] nadir, double[,] dataset)
        {
            if (method == IndicatorsType.Hypervolume)
                return new Hypervolume(minmax, nadir, dataset);
            else
                throw new IndexOutOfRangeException(); // should never get here
        }
        
        public static Indicators GetInstance(IndicatorsType method, bool[] minmax,
                                             double[] nadir, double[,] dataset, 
                                             double[,] referenceset)
        {
            if (method == IndicatorsType.Hypervolume)
                return new Hypervolume(minmax, nadir, dataset, referenceset);
            else
                throw new IndexOutOfRangeException(); // should never get here
        }
    }
}

