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

