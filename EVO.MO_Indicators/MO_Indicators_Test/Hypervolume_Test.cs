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
using ihwb.EVO.MO_Indicators;

namespace ihwb.EVO.MO_Indicators
{
    using NUnit.Framework;
    [TestFixture]
    public class Hypervolume_Test
    {
        private ihwb.EVO.MO_Indicators.Indicators oTestclass;
        private static char[] deliminator = { '\n' };

        #region Setup / Teardown
        [SetUp]
        public void Init()
        {
            try
            {
                
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Fehler beim Initialisieren des Tests");
                this.WriteException(e.Message);
                throw (e);
            }
        }

        [TearDown]
        public void ClearUp()
        {
            try
            {
                
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Fehler beim ClearUp des Tests");
                this.WriteException(e.Message);
                throw (e);
            }
        }

        #endregion

        #region Init (auch mit provizierten Fehlern)
        [Test]
        public void Initialize_Method_0_Uebergabeparameter()
        {
            int _dim;
            bool[] _obj;
            int _method;
            double[] _nadir;
            double[,] _dataset;

            bool[] minmax = {false,false};
            double[] nadir = {4.0,4.0};
            double[,] dataset = {{1.0, 3.0}, {1.5, 1.5}, {3.0, 1.0}};

            try
            {
                oTestclass = MO_IndicatorFabrik.GetInstance(MO_IndicatorFabrik.IndicatorsType.Hypervolume,
                                                            minmax, nadir, dataset);
                Assert.AreEqual(oTestclass.GetType(), typeof(ihwb.EVO.MO_Indicators.Hypervolume));
                _dim = oTestclass.dimension;
                Assert.AreEqual(_dim, 2);
                _obj = oTestclass.minmax;
                Assert.IsTrue(_obj[0] == false && _obj[1] == false);
                _method = oTestclass.method;
                Assert.AreEqual(_method, 0);
                _nadir = oTestclass.nadir;
                Assert.IsTrue(_nadir[0] == 4.0 && _nadir[1] == 4.0);
                _dataset = oTestclass.dataset;
                Assert.IsTrue(dataset[0, 0] == _dataset[0, 0] &
                              dataset[0, 1] == _dataset[0, 1] &
                              dataset[1, 0] == _dataset[1, 0] &
                              dataset[1, 1] == _dataset[1, 1] &
                              dataset[2, 0] == _dataset[2, 0] &
                              dataset[2, 1] == _dataset[2, 1]);

                oTestclass = null;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("------------------------------");
                System.Console.WriteLine("Konstruktor Methode 0 mit Übergabeparameter");
                System.Console.WriteLine("------------------------------");

                this.WriteException(e.Message);

                System.Console.WriteLine("-------------------------");

                throw (e);
            }
        }

        [Test]
        public void Initialize_Method_0()
        {

            bool[] minmax = { false, false };
            double[] nadir = { 4.0, 4.0 };
            double[,] dataset = { { 1.0, 3.0 }, { 1.5, 1.5 }, { 3.0, 1.0 } };

            try
            {
                oTestclass = MO_IndicatorFabrik.GetInstance(MO_IndicatorFabrik.IndicatorsType.Hypervolume,
                                                            minmax, nadir, dataset);
                Assert.AreEqual(oTestclass.GetType(), typeof(ihwb.EVO.MO_Indicators.Hypervolume));
                oTestclass = null;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("------------------------------");
                System.Console.WriteLine("Standard-Konstruktor Methode 0");
                System.Console.WriteLine("------------------------------");

                this.WriteException(e.Message);

                System.Console.WriteLine("-------------------------");

                throw (e);
            }
        }

        [Test]
        public void Initialize_Method_1()
        {

            bool[] minmax = { false, false };
            double[] nadir = { 4.0, 4.0 };
            double[,] dataset = { { 1.0, 3.0 }, { 1.5, 1.5 }, { 3.0, 1.0 } };
            double[,] refset = { { 1.0, 3.0 }, { 1.5, 1.5 }, { 3.0, 1.0 } };

            try
            {
                oTestclass = MO_IndicatorFabrik.GetInstance(MO_IndicatorFabrik.IndicatorsType.Hypervolume,
                                                            minmax, nadir, dataset, refset);
                Assert.AreEqual(oTestclass.GetType(), typeof(ihwb.EVO.MO_Indicators.Hypervolume));
                oTestclass = null;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("------------------------------");
                System.Console.WriteLine("Standard-Konstruktor Methode 1");
                System.Console.WriteLine("------------------------------");

                this.WriteException(e.Message);

                System.Console.WriteLine("-------------------------");

                throw (e);
            }
        }

        [Test]
        public void Initialize_Method_1_Uebergabeparameter()
        {
            int _dim;
            bool[] _obj;
            int _method;
            double[] _nadir;
            double[,] _dataset;
            double[,] _refset;

            bool[] minmax = { false, false };
            double[] nadir = { 4.0, 4.0 };
            double[,] dataset = { { 1.0, 3.0 }, { 1.5, 1.5 }, { 3.0, 1.0 } };
            double[,] refset = { { 1.0, 3.0 }, { 1.5, 1.5 }, { 3.0, 1.0 } };

            try
            {
                oTestclass = MO_IndicatorFabrik.GetInstance(MO_IndicatorFabrik.IndicatorsType.Hypervolume,
                                                            minmax, nadir, dataset, refset);
                Assert.AreEqual(oTestclass.GetType(), typeof(ihwb.EVO.MO_Indicators.Hypervolume));
                _dim = oTestclass.dimension;
                Assert.AreEqual(_dim, 2);
                _obj = oTestclass.minmax;
                Assert.IsTrue(_obj[0] == false && _obj[1] == false);
                _method = oTestclass.method;
                Assert.AreEqual(_method, 1);
                _nadir = oTestclass.nadir;
                Assert.IsTrue(_nadir[0] == 4.0 && _nadir[1] == 4.0);
                _dataset = oTestclass.dataset;
                Assert.IsTrue(dataset[0, 0] == _dataset[0, 0] &
                              dataset[0, 1] == _dataset[0, 1] &
                              dataset[1, 0] == _dataset[1, 0] &
                              dataset[1, 1] == _dataset[1, 1] &
                              dataset[2, 0] == _dataset[2, 0] &
                              dataset[2, 1] == _dataset[2, 1]);
                _refset = oTestclass.referenceset;
                Assert.IsTrue(refset[0, 0] == _refset[0, 0] &
                              refset[0, 1] == _refset[0, 1] &
                              refset[1, 0] == _refset[1, 0] &
                              refset[1, 1] == _refset[1, 1] &
                              refset[2, 0] == _refset[2, 0] &
                              refset[2, 1] == _refset[2, 1]);

                oTestclass = null;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("-------------------------------------------");
                System.Console.WriteLine("Konstruktor Methode 1 mit Übergabeparameter");
                System.Console.WriteLine("-------------------------------------------");

                this.WriteException(e.Message);

                System.Console.WriteLine("-------------------------------------------");

                throw (e);
            }
        }


        #endregion

        [Test]
        public void Calculate_Method_0()
        {

            bool[] minmax = { false, false };
            double[] nadir = { 4.0, 4.0 };
            double[,] dataset = { { 1.0, 3.0 }, { 1.5, 1.5 }, { 3.0, 1.0 } };
            double calculated_value;
            double[] _nadir;

            try
            {
                oTestclass = MO_IndicatorFabrik.GetInstance(MO_IndicatorFabrik.IndicatorsType.Hypervolume,
                                                            minmax, nadir, dataset);
                calculated_value = oTestclass.calc_indicator();
                _nadir = oTestclass.nadir;
                Assert.IsTrue(calculated_value == -7.25);
                oTestclass = null;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("-------------------------------");
                System.Console.WriteLine("Calculate Hypervolume Methode 0");
                System.Console.WriteLine("-------------------------------");

                this.WriteException(e.Message);

                System.Console.WriteLine("-------------------------------");

                throw (e);
            }
        }


        [Test]
        public void Calculate_Method_1()
        {

            bool[] minmax = { false, false };
            double[] nadir = { 4.0, 4.0 };
            double[,] dataset = { { 1.0, 3.0 }, { 1.5, 1.5 }, { 3.0, 1.0 } };
            double[,] refset = { { 1.0, 3.0 }, { 1.5, 1.5 }, { 3.0, 1.0 } };
            double calculated_value;

            try
            {
                oTestclass = MO_IndicatorFabrik.GetInstance(MO_IndicatorFabrik.IndicatorsType.Hypervolume,
                                                            minmax, nadir, dataset, refset);
                calculated_value = oTestclass.calc_indicator();
                Assert.IsTrue(calculated_value == 0.0);
                oTestclass = null;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("-------------------------------");
                System.Console.WriteLine("Calculate Hypervolume Methode 1");
                System.Console.WriteLine("-------------------------------");

                this.WriteException(e.Message);

                System.Console.WriteLine("-------------------------");

                throw (e);
            }
        }

        [Test]
        public void Calculate_Method_0_Iteration()
        {

            bool[] minmax = { false, false };
            double[] nadir = { 4.0, 4.0 };
            double[,] dataset = { { 1.0, 3.0 }, { 1.5, 1.5 }, { 3.0, 1.0 } };
            double calculated_value;
            double[] _nadir;

            try
            {
                oTestclass = MO_IndicatorFabrik.GetInstance(MO_IndicatorFabrik.IndicatorsType.Hypervolume,
                                                            minmax, nadir, dataset);
                calculated_value = oTestclass.calc_indicator();
                Assert.IsTrue(calculated_value == -7.25);
                dataset[0, 0] = 0.0;
                oTestclass.update_dataset(dataset);
                calculated_value = oTestclass.calc_indicator();
                Assert.IsTrue(calculated_value == -8.25);
                dataset[0, 1] = 4.0;
                oTestclass.update_dataset(dataset);
                calculated_value = oTestclass.calc_indicator();
                Assert.IsTrue(calculated_value == -6.75);
                oTestclass = null;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("-------------------------------");
                System.Console.WriteLine("Calculate Hypervolume Methode 0 Iteration");
                System.Console.WriteLine("-------------------------------");

                this.WriteException(e.Message);

                System.Console.WriteLine("-------------------------");

                throw (e);
            }
        }

        [Test]
        public void Calculate_Method_0_Iteration_update_Nadir()
        {

            bool[] minmax = { false, false };
            double[] nadir = { 4.0, 4.0 };
            double[,] dataset = { { 1.0, 3.0 }, { 1.5, 1.5 }, { 3.0, 1.0 } };
            double calculated_value;
            double[] _nadir;

            try
            {
                oTestclass = MO_IndicatorFabrik.GetInstance(MO_IndicatorFabrik.IndicatorsType.Hypervolume,
                                                            minmax, nadir, dataset);
                calculated_value = oTestclass.calc_indicator();
                Assert.IsTrue(calculated_value == -7.25);
                dataset[0, 0] = 0.0;
                oTestclass.update_dataset(dataset);
                calculated_value = oTestclass.calc_indicator();
                Assert.IsTrue(calculated_value == -8.25);
                dataset[0, 1] = 5.0;
                oTestclass.update_dataset(dataset);
                calculated_value = oTestclass.calc_indicator();
                _nadir = oTestclass.nadir;
                Assert.IsTrue(_nadir[1] == 5.0);
                Assert.IsTrue(calculated_value == -9.25);
                oTestclass = null;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("-------------------------------");
                System.Console.WriteLine("Calculate Hypervolume Methode 0 Iteration update Nadir");
                System.Console.WriteLine("-------------------------------");

                this.WriteException(e.Message);

                System.Console.WriteLine("-------------------------");

                throw (e);
            }
        }

        protected void WriteException(string message)
        {
            for (int i = 0; i < message.Split(deliminator).Length; i++)
            {
                System.Console.WriteLine((message.Split(deliminator)[i]));
            }
        }
    }


}
