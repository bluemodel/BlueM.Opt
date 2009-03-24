using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IHWB.EVO.MO_Indicators
{
    public class Solutionvolume2
    {
        double diversity;  //Diversität
        double[] evo;    //Abstand der Basepoints zueinander
        double evosum;     //Durchschnitt der Evolution der letzten historylength Generationen
        double maxevosum;  //Maximaler Durchschnitt der Evolution der letzten historylength Generationen
        int historylength;    //Anzahl der Generationen die zum Vergleich herangenommen werden
        double[] basepoint_old;   //Basiswert von dem aus die Distanzquadrate berechnet werden
        double[] basepoint;
        double faktor2switch; //Wenn der Wert für Diversität oder Entwicklung zur Paretofront geringer ist als 1/faktor2switch wird umgeschaltet
        EVO.Diagramm.Monitor monitor1;
        string completeinfo;
        EVO.Common.EVO_Settings settings;
        bool firstrun;

        //Monitor
        private Steema.TeeChart.Styles.Line Line2;
        private Steema.TeeChart.Styles.Line Line3;
        private Steema.TeeChart.Styles.Line Line4; 

        public Solutionvolume2(ref EVO.Common.Problem probelm_input, int historylength_input, int faktor2switch_input, ref EVO.Diagramm.Monitor monitor_input, ref EVO.Common.EVO_Settings settings_input)
        {
            historylength = historylength_input;
            settings = settings_input;
            faktor2switch = faktor2switch_input;
     
            monitor1 = monitor_input;
            basepoint = new double[probelm_input.List_PrimObjectiveFunctions.Length];
            basepoint_old = new double[probelm_input.List_PrimObjectiveFunctions.Length];
            evo = new double[historylength];

            evosum = 0;
            maxevosum = 0;
            diversity = 0;
            completeinfo = "";
            firstrun = true;

            this.InitMonitor();
            monitor1.SelectTabDiagramm();
            monitor1.Show();
        }

        
        public bool calculate(ref EVO.Common.Individuum_MetaEvo[] generation)
        {
            bool back = false;
            

            //Speichern von basepoint in basepoint_old
            basepoint.CopyTo(basepoint_old, 0);

            //basepoint = Durchschnittswerte im Zielfunktionsraum der Generation
            basepoint = durchschnittsIndividuum(ref generation);


            //Durchschnittliche Distanz der Individuen zum basepoint (Diversität)
            diversity = durchschnittliche_distanz(ref generation, basepoint);
            


            //values der Generationen pushen 
            for (int i = evo.Length-1; i > 0; i--)
            {
                evo[i] = evo[i - 1];
            }

            //Abstand der Durchschnittsindividuen (Entwicklung richtung Paretofront)
            if (firstrun) { evo[0] = 0; firstrun = false; }
            else evo[0] = abstand(basepoint, basepoint_old);

            //SUMMEN der Indikatorwerte über historylength Generationen
            evosum = 0;
            for (int i = 0; i < historylength; i++)
            {
                evosum += evo[i];
            }

            //Neue Maxwerte setzen oder neue Werte sind weniger als 1/faktor2switch so gross wie bisherige -> Umschalten
            if (evosum > maxevosum)  //Entwicklung richtung Paretofront
            {
                maxevosum = evosum;
            }
            else if (maxevosum > evosum * faktor2switch)
            {
                back = true;
            }

            //Zeichnen
            this.ZeichneLinie("Diversität", diversity);
            this.ZeichneLinie("Entwicklung", evosum);
            this.ZeichneLinie("Entwicklung Grenze", maxevosum / faktor2switch);

            completeinfo = "Div: " + diversity + " Evo: " + evo[0] + " Sum of last " + historylength + " generations: " + evosum + " (Frontier:" + maxevosum + ") - Faktor2switch: " + faktor2switch;
            if (back) completeinfo += " -> switch to local optimization)";
            return back;
        }

        public string get_last_infos()
        {
            return "Div: " + diversity + " Evo: " + evo[0];
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
            Line2 = monitor1.Diag.getSeriesLine("Diversität (Durchschnittlicher Abstand zu einem Mittelpunkt)", "Red");
            Line2.CustomHorizAxis = monitor1.Diag.Axes.Top;
            Line2.CustomVertAxis = monitor1.Diag.Axes.Left;
            Line2.Color = System.Drawing.Color.Blue;
            Line2.Pointer.Visible = true;
            Line2.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle;
            //Line2.Pointer.Brush.Color = System.Drawing.Color.Blue;
            Line2.Pointer.HorizSize = 2;
            Line2.Pointer.VertSize = 2;
            Line2.Pointer.Pen.Visible = false;

            //Linien Diversität
            Line3 = monitor1.Diag.getSeriesLine("Durchschnittliche Entwicklung über " + historylength + " Generationen", "Green");
            Line3.CustomHorizAxis = monitor1.Diag.Axes.Top;
            Line3.CustomVertAxis = monitor1.Diag.Axes.Right;
            Line3.Color = System.Drawing.Color.LimeGreen;
            Line3.Pointer.Visible = true;
            Line3.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle;
            //Line3.Pointer.Brush.Color = System.Drawing.Color.LimeGreen;
            Line3.Pointer.HorizSize = 2;
            Line3.Pointer.VertSize = 2;
            Line3.Pointer.Pen.Visible = false;

            //Linien Diversität Grenze
            Line4 = monitor1.Diag.getSeriesLine("Entwicklung Grenze", "Yellow");
            Line4.CustomHorizAxis = monitor1.Diag.Axes.Top;
            Line4.CustomVertAxis = monitor1.Diag.Axes.Right;
            Line4.Color = System.Drawing.Color.Green;
            Line4.Pointer.Visible = true;
            Line4.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Circle;
            //Line4.Pointer.Brush.Color = System.Drawing.Color.Green;
            Line4.Pointer.HorizSize = 2;
            Line4.Pointer.VertSize = 2;
            Line4.Pointer.Pen.Visible = false;
        }

        private void ZeichneLinie(string type, double value)
        {
            switch (type)
            {
                case "Diversität":
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
                abstand += Math.Pow(eins[i]-zwei[i],2);
            }
            return Math.Sqrt(abstand);
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
