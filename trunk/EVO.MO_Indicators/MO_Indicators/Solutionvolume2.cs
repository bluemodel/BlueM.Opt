using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IHWB.EVO.MO_Indicators
{
    public class Solutionvolume2
    {
        double[,] values;    //[]([0] = Diversität der Generation(Abstandssumme zu Basepoint), [1] = Abstand der Basepoints
        double[] maxvaluesum;
        int historylength;    //Anzahl der Generationen die zum Vergleich herangenommen werden
        double[] basepoint_old;   //Basiswert von dem aus die Distanzquadrate berechnet werden
        double[] basepoint;
        double faktor2switch; //Wenn der Wert für Diversität oder Entwicklung zur Paretofront geringer ist als 1/faktor2switch wird umgeschaltet
        EVO.Diagramm.Monitor monitor1;
        string completeinfo;
        EVO.Common.EVO_Settings settings;

        //Monitor
        private Steema.TeeChart.Styles.Line Line1; 
        private Steema.TeeChart.Styles.Line Line2;
        private Steema.TeeChart.Styles.Line Line3;
        private Steema.TeeChart.Styles.Line Line4; 

        public Solutionvolume2(ref EVO.Common.Problem probelm_input, int historylength_input, int faktor2switch_input, ref EVO.Diagramm.Monitor monitor_input, ref EVO.Common.EVO_Settings settings_input)
        {
            historylength = historylength_input;
            settings = settings_input;
            values = new double[historylength,2];
            monitor1 = monitor_input;
            basepoint = new double[probelm_input.List_PrimObjectiveFunctions.Length];
            basepoint_old = new double[probelm_input.List_PrimObjectiveFunctions.Length];
            maxvaluesum = new double[2];
            faktor2switch = faktor2switch_input;
            completeinfo = "";
            this.InitMonitor();
            monitor1.SelectTabDiagramm();
            monitor1.Show();
        }

        
        public bool calculate(ref EVO.Common.Individuum_MetaEvo[] generation)
        {
            double[] tmp = new double[2];
            bool back = false;

            //Speichern von basepoint in basepoint_old
            basepoint.CopyTo(basepoint_old, 0);

            //basepoint = Durchschnittswerte im Zielfunktionsraum der Generation
            basepoint = durchschnittsIndividuum(ref generation);

            //values der Generationen pushen 
            for (int i = (values.Length/2)-1; i > 0; i--)
            {
                values[i, 0] = values[i - 1, 0];
                values[i, 1] = values[i - 1, 1];
            }

            //Durchschnittliche Distanz der Individuen zum basepoint (Diversität)
            values[0, 0] = durchschnittliche_distanz(ref generation, basepoint);
 
            //Abstand der Durchschnittsindividuen (Entwicklung richtung Paretofront)
            values[0, 1] = abstand(basepoint, basepoint_old);

            //SUMMEN der Indikatorwerte über historylength Generationen
            tmp[0] = 0;
            tmp[1] = 0;
            for (int i = 0; i < historylength; i++)
            {
                tmp[0] += values[i, 0];
                tmp[1] += values[i, 1];
            }

            //Neue Maxwerte setzen oder neue Werte sind weniger als 1/faktor2switch so gross wie bisherige -> Umschalten
            if (tmp[0] > maxvaluesum[0])  //Diversität
            {
                maxvaluesum[0] = tmp[0];
            }

            else if (maxvaluesum[0] > tmp[0] * faktor2switch)
            {
                back = true;
            }

            if (tmp[1] > maxvaluesum[1])  //Entwicklung richtung Paretofront
            {
                maxvaluesum[1] = tmp[1];
            }
            else if (maxvaluesum[1] > tmp[1] * faktor2switch)
            {
                back = true;
            }

            //Zeichnen
            this.ZeichneLinie("Diversität", tmp[0]);
            this.ZeichneLinie("Diversität Grenze", maxvaluesum[0]/faktor2switch);
            this.ZeichneLinie("Entwicklung", tmp[1]);
            this.ZeichneLinie("Entwicklung Grenze", maxvaluesum[1]/faktor2switch);

            completeinfo = "Div: " + values[0, 0] + " Sum of last " + historylength + " generations: " + tmp[0] + " (MaxSum:" + maxvaluesum[0] + ") Evo: " + values[0, 1] + " Sum of last " + historylength + " generations: " + tmp[1] + " (MaxSum:" + maxvaluesum[1] + ") - Faktor2switch: " + faktor2switch;
            if (back) completeinfo += " -> switch to local optimization)";
            return back;
        }

        public string get_last_infos()
        {
            return "Div: " + values[0,0] + " Evo: " + values[0,1];
        }

        public string get_complete_infos()
        {
            return completeinfo;
        }

        private void InitMonitor()
        {
            //Achsen

            //Schrittweitenachse
            monitor1.Diag.Axes.Left.Title.Caption = "Diversität";

            //Generationsachse (oben)
            monitor1.Diag.Axes.Bottom.Visible = true;
            monitor1.Diag.Axes.Bottom.Title.Caption = "Generation";
            monitor1.Diag.Axes.Bottom.Horizontal = true;
            monitor1.Diag.Axes.Bottom.Automatic = true;
            monitor1.Diag.Axes.Bottom.Grid.Visible = false;

            //Hypervolumenachse (rechts)
            monitor1.Diag.Axes.Right.Visible = true;
            monitor1.Diag.Axes.Right.Title.Caption = "Entwicklung";
            monitor1.Diag.Axes.Right.Title.Angle = 90;
            monitor1.Diag.Axes.Right.Automatic = true;
            monitor1.Diag.Axes.Right.Grid.Visible = false;
            
            //Linien Diversität
            Line1 = monitor1.Diag.getSeriesLine("Durchschnittliche Diversität über " + historylength + " Generationen", "Blue");
            Line1.CustomHorizAxis = monitor1.Diag.Axes.Top;
            Line1.CustomVertAxis = monitor1.Diag.Axes.Left;
            Line1.Pointer.Visible = true;
            Line1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle;
            Line1.Pointer.Brush.Color = System.Drawing.Color.Blue;
            Line1.Pointer.HorizSize = 2;
            Line1.Pointer.VertSize = 2;
            Line1.Pointer.Pen.Visible = false;

            //Linien Diversität Grenze
            Line2 = monitor1.Diag.getSeriesLine("Diversität Grenze", "Red");
            Line2.CustomHorizAxis = monitor1.Diag.Axes.Top;
            Line2.CustomVertAxis = monitor1.Diag.Axes.Left;
            Line2.Color = System.Drawing.Color.SteelBlue;
            Line2.Pointer.Visible = true;
            Line2.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle;
            Line2.Pointer.Brush.Color = System.Drawing.Color.SteelBlue;
            Line2.Pointer.HorizSize = 2;
            Line2.Pointer.VertSize = 2;
            Line2.Pointer.Pen.Visible = false;

            //Linien Diversität
            Line3 = monitor1.Diag.getSeriesLine("Durchschnittliche Entwicklung über " + historylength + " Generationen", "Green");
            Line3.CustomHorizAxis = monitor1.Diag.Axes.Top;
            Line3.CustomVertAxis = monitor1.Diag.Axes.Right;
            Line3.Pointer.Visible = true;
            Line3.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle;
            Line3.Pointer.Brush.Color = System.Drawing.Color.Green;
            Line3.Pointer.HorizSize = 2;
            Line3.Pointer.VertSize = 2;
            Line3.Pointer.Pen.Visible = false;

            //Linien Diversität Grenze
            Line4 = monitor1.Diag.getSeriesLine("Entwicklung Grenze", "Yellow");
            Line4.CustomHorizAxis = monitor1.Diag.Axes.Top;
            Line4.CustomVertAxis = monitor1.Diag.Axes.Right;
            Line4.Color = System.Drawing.Color.SpringGreen;
            Line4.Pointer.Visible = true;
            Line4.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle;
            Line4.Pointer.Brush.Color = System.Drawing.Color.SpringGreen;
            Line4.Pointer.HorizSize = 2;
            Line4.Pointer.VertSize = 2;
            Line4.Pointer.Pen.Visible = false;
        }

        private void ZeichneLinie(string type, double value)
        {
            switch (type)
            {
                case "Diversität":
                    Line1.Add(this.settings.MetaEvo.CurrentGeneration, value);
                    break;
                case "Diversität Grenze":
                    Line2.Add(this.settings.MetaEvo.CurrentGeneration, value);
                    break;
                case "Entwicklung":
                    Line3.Add(this.settings.MetaEvo.CurrentGeneration, value);
                    break;
                case "Entwicklung Grenze":
                    Line4.Add(this.settings.MetaEvo.CurrentGeneration, value);
                    break;
            }
        }

        private double abstand(double[] eins, double[] zwei)
        {
            double abstand = 0;

            for (int i = 0; i < eins.Length; i++)
            {
                abstand += Math.Sqrt(eins[i] * eins[i] + zwei[i] * zwei[i]);
            }
            return abstand;
        }

        private double durchschnittliche_distanz(ref EVO.Common.Individuum_MetaEvo[] generation, double[] basepoint)
        {
            double distanzsumme = 0;
            int numberPrimobjectives = generation[0].PrimObjectives.Length;
            double[] primobjectives = new double[numberPrimobjectives];

            for (int i = 0; i < generation.Length; i++)
            {
                primobjectives = generation[i].PrimObjectives;
                for (int j = 0; j < primobjectives.Length; j++)
                {
                    distanzsumme += Math.Abs(primobjectives[j] - basepoint[j]);
                }
            }
            return distanzsumme / generation.Length;
        }

        private double[] durchschnittsIndividuum(ref EVO.Common.Individuum_MetaEvo[] generation)
        {
            int numberPrimobjectives = generation[0].PrimObjectives.Length;
            double[] primobjectivesSum = new double[numberPrimobjectives];
            double[] primobjectives = new double[numberPrimobjectives];

            for (int j = 0; j < primobjectives.Length; j++)
            {
                primobjectivesSum[j] = 0;
                primobjectives[j] = 0;
            }

            for (int i = 0; i < generation.Length; i++)
            {
                primobjectives = generation[i].PrimObjectives;
                for (int j = 0; j < primobjectives.Length; j++)
                {
                    primobjectivesSum[j] += (primobjectives[j]/generation.Length);
                }
            }
            return primobjectivesSum;
        }
    }
}
