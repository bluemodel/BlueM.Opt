using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace IHWB.EVO.MetaEvo
{
    class Algomanager
    {
        EVO.Common.Individuum_MetaEvo[] genpool;
        EVO.Diagramm.ApplicationLog applog;
        Algos algos;

        public Algomanager(ref EVO.Common.Problem prob_input, int individuumnumber_input, ref EVO.Diagramm.ApplicationLog applog_input) //Zeichenobjekt übergeben
        {
            applog = applog_input;
            //Algoobjekt initialisieren (enthält die algorithmus-Methoden und das Feedback zu jedem Algo)
            algos = new Algos(individuumnumber_input);
        }

        public void set_genpool(EVO.Common.Individuum_MetaEvo[] genpool_input) 
        {
            this.genpool = new EVO.Common.Individuum_MetaEvo[genpool_input.Length];
            for (int i = 0; i < genpool_input.Length; i++)
            {
                this.genpool[i] = genpool_input[i].Clone_MetaEvo();
            }
        }

        //new_generation mit Genpool verarbeiten und neue Individuen in new_generation erzeugen
        public void eval_and_build(ref EVO.Common.Individuum_MetaEvo[] new_generation_input)
        {
            if (applog.log) applog.appendText("Algo Manager: Input: Generated and Simulated Individuums: \r\n" + this.generationinfo(ref new_generation_input));
            //1.Selektion: 
            //1.1.Dominanzanalyse
            //1.1.2.Sortieren nach einem zufällig gewählten Kriterium
            Random rand = new Random();
            int kriterium = rand.Next(0, new_generation_input[0].Penalties.Length);

            //1.1.3.Sortieren und Dominanzkriterium anwenden innerhalb der Penalties der neuen Individuen
            if (applog.log) applog.appendText("Algo Manager: Starting quicksort using criterion "+kriterium);
            quicksort(ref new_generation_input, kriterium, 0, new_generation_input.Length-1);
            if (applog.log) applog.appendText("Algo Manager: Starting domination-check");
            check_domination(ref new_generation_input, ref new_generation_input);
            
            //1.1.4.Dominanzkriterium auf Penalties zwischen den neuen und den alten Individuen anwenden
            check_domination(ref new_generation_input, ref genpool);

            //1.2.Clustering bis auf maximale Generationsgrösse, Speichern in Genpool
            clustering_kill(ref genpool, ref new_generation_input);

            //2.Feedback erstellen
            newGen_composition(ref new_generation_input);

            //3.Genpool und neue Individuen zu neuem Genpool zusammenfassen 
            zip(ref genpool, ref new_generation_input);
            if (applog.log) applog.appendText("Algo Manager: Result: New Genpool: \r\n" + this.generationinfo(ref genpool));
            quicksort(ref genpool, kriterium, 0, new_generation_input.Length - 1);

            //4.Generierung neuer Individuen (wieder in new_generation_input
            algos.newGeneration(ref genpool, ref new_generation_input);
        }

        //Sortieren nach einem gegebenen Penaltie(ok)
        private void quicksort(ref EVO.Common.Individuum_MetaEvo[] input, int kriterium, int low_input, int high_input)
        {
            int low = low_input;
            int high = high_input;
            double medianvalue = input[(low_input + high_input) / 2].Penalties[kriterium];
            EVO.Common.Individuum_MetaEvo individuum_tmp;

            while (low < high)
            {
                //Nächste Abweichung von der erstrebten Sortierung suchen
                while ((input[low].Penalties[kriterium] < medianvalue) && (low < high)) low++;
                while ((input[high].Penalties[kriterium] > medianvalue) && (low < high)) high--;
                //Tauschen falls indizes "high" und "low" entsprechend stehen
                if (low < high)
                {
                    individuum_tmp = input[low];
                    input[low] = input[high];
                    input[high] = individuum_tmp;
                    low++;
                    high--;
                }
            }
            if (low == high)
            {
                if (input[low].Penalties[kriterium] < medianvalue) high++;
                else if (input[low].Penalties[kriterium] == medianvalue) { high++; low--; }
                else low--;
            }
            else { low--; high++; }
            if (low_input < low) quicksort(ref input, kriterium, low_input, low);
            if (high < high_input) quicksort(ref input, kriterium, high, high_input);
        }
        //Prüfen ob ein ein Individuum von einem anderen Individuum dominiert wird
        private void check_domination(ref EVO.Common.Individuum_MetaEvo[] input, ref EVO.Common.Individuum_MetaEvo[] input2)
        {
            int dominated = 0;
            int status = 0;

            //Falls input1 = input2, nur innerhalb der ersten Generation prüfen
            if (input == input2)
            {
                //Individuen vergleichen
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i].get_status() == "true")
                    {
                        //mit
                        for (int k = i + 1; k < input.Length; k++)
                        {
                            if (input[k].get_status() == "true")
                            {
                                //Jede Eigenschaft vergleichen
                                for (int j = 0; j < input[0].Penalties.Length; j++)
                                {
                                    dominated = j;
                                    if (input[i].Penalties[j] > input[k].Penalties[j]) { dominated = 0; break; }
                                }
                                if (dominated > 0) { 
                                    input[i].set_status("false");
                                    if (applog.log) applog.appendText("Algo Manager: Domination: Individuum " + input[i].ID + " is dominated by Individuum " + input[dominated].ID);
                                    break; 
                                }
                            }
                        }
                    }
                }
            }
            //Sonst Generationen untereinander prüfen
            else
            {
                //Individuen vergleichen
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i].get_status() == "true")
                    {
                        //mit
                        for (int k = 0; k < input2.Length; k++)
                        {
                            if (input2[k].get_status() == "true")
                            {
                                //Jede Eigenschaft vergleichen
                                for (int j = 0; j < input[0].Penalties.Length; j++)
                                {
                                    //falls gleich, nichts tun
                                    if (input[i].Penalties[j] > input2[k].Penalties[j])
                                    {
                                        if (status == 0) status = 1;
                                        else if (status == -1)
                                        {
                                            status = 0;
                                            break;
                                        }
                                    }
                                    if (input[i].Penalties[j] < input2[k].Penalties[j])
                                    { 
                                        if (status == 0) status = -1;
                                        else if (status == 1)
                                        {
                                            status = 0;
                                            break;
                                        }
                                    }
                                }
                                if (status == 1) { 
                                    status = 0; 
                                    input2[k].set_status("false");
                                    if (applog.log) applog.appendText("Algo Manager: Domination: Individuum " + input[k].ID + " is dominated by Individuum " + input[i].ID);
                                    break; 
                                }
                                if (status == -1) { 
                                    status = 0; 
                                    input[i].set_status("false");
                                    if (applog.log) applog.appendText("Algo Manager: Domination: Individuum " + input[i].ID + " is dominated by Individuum " + input[k].ID);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

           
        }
        //Individuen die einen zu geringen Abstand der Penalties besitzen, löschen
        private void clustering_kill(ref EVO.Common.Individuum_MetaEvo[] input, ref EVO.Common.Individuum_MetaEvo[] input2)
        {
            int maxindividuums = genpool.Length;
            double distance;

            for (int i = 0; i < genpool.Length; i++)
            {
                if (input[i].get_status() == "true") maxindividuums--;
                if (input2[i].get_status() == "true") maxindividuums--;
            }
            // zu viele Individuen vorhanden  (Wenn die Individuenanzahl der definierten Generationsgrösse entspricht, abbrechen)
            if (maxindividuums < 0)
            {
                maxindividuums *= -1;
                double[,] individuums2kill = new double[maxindividuums, 2];

                //Arbeits-Array erstellen
                EVO.Common.Individuum_MetaEvo[] work = new IHWB.EVO.Common.Individuum_MetaEvo[2 * input.Length];
                for (int i = 0; i < 2 * input.Length; i++)
                {
                    if (i > input.Length) work[i] = input2[i - input.Length];
                    else work[i] = input[i];
                }

                //Geringste Abstände finden
                for (int i = 0; i < work.Length; i++)
                {
                    if (work[i].get_status() == "true")
                    {
                        //mit
                        for (int k = i + 1; k < work.Length; k++)
                        {
                            if (work[k].get_status() == "true")
                            {
                                for (int j = 1; j <= individuums2kill.Length; j++)
                                {
                                    distance = easydistance(work[i].Penalties, work[k].Penalties);
                                    if (distance < individuums2kill[individuums2kill.Length - j, 1])
                                    {
                                        if (j != 1)
                                        {
                                            individuums2kill[individuums2kill.Length - j + 1, 0] = individuums2kill[individuums2kill.Length - j, 0];
                                            individuums2kill[individuums2kill.Length - j + 1, 1] = individuums2kill[individuums2kill.Length - j, 1];
                                            individuums2kill[individuums2kill.Length - j, 0] = i;
                                            individuums2kill[individuums2kill.Length - j, 1] = distance;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //Zu nahe Individuen killen
                for (int i = 0; i < individuums2kill.Length; i++)
                {
                    work[(int)individuums2kill[i, 0]].set_status("false");
                    if (applog.log) applog.appendText("Algo Manager: Clustering: Individuum " + work[(int)individuums2kill[i, 0]].ID + " is not used anymore");
                }
                //Zur überprüfung ob genug Individuen reduziert wurden
                clustering_kill(ref input, ref input2);
            }  
        }
        //Kopieren der Verbleibenden Individuen auf die Generation input
        private void zip(ref EVO.Common.Individuum_MetaEvo[] input, ref EVO.Common.Individuum_MetaEvo[] input2)
        {
            int pointer = 0;

            for (int i = 0; i < 2 * input.Length; i++)
            {
                if (i >= input.Length)
                {
                    if (input2[i - input.Length].get_status() == "true")
                    {
                        genpool[pointer] = input2[i - input.Length];
                        pointer++;
                    }
                }
                else
                {
                    if (input[i].get_status() == "true")
                    {
                        genpool[pointer] = input[i];
                        pointer++;
                    }
                }
            }
        }
        //Einfache Distanzsumme zwischen zwei Arrays
        private double easydistance(double[] input1, double[] input2)
        {
            double back = 0;

            for (int i = 0; i < input1.Length; i++)
            {
                back += System.Math.Abs(input1[i] - input2[i]);
            }

            return back;
        }
        //String mit Auszug der generation
        private string generationinfo(ref EVO.Common.Individuum_MetaEvo[] generation)
        {
            string back = "-----Generation-----\r\n";
            double[] optparas;

            for (int i = 0; i < generation.Length; i++)
            {
                back = back + "[ID]: " + generation[i].ID + " [Generator]: " + generation[i].get_generator() + " [Status]: " + generation[i].get_status() + " [Client]: " + generation[i].get_client() + "\r\nOptparas: ";
                optparas = generation[i].get_optparas();
                for (int j = 0; j < optparas.Length; j++)
                {
                    back = back + "[" + j + "]: " + String.Format("{0:####}", optparas[j]) + " ";
                }
                back = back + "\r\nPenalties";
                for (int j = 0; j < generation[i].Penalties.Length; j++)
                {
                    back = back + "[" + j + "]: " + String.Format("{0:####}", generation[i].Penalties[j]) + " ";
                }
                back = back + "\r\n";
            }

            return back + "----------";
        }
        //Feedback erstellen
        private void newGen_composition(ref EVO.Common.Individuum_MetaEvo[] new_generation_input)
        {
            double initiativensumme = 0;
            string log = "";

            //1. Reset der Daten der letzten Generation
            for (int i = 0; i < algos.algofeedbackarray.Length; i++)
            {
                algos.algofeedbackarray[i].number_individuals_survived = 0;
            }

            //2. Neue Überlebens-Zahlen aus der simulierten Generation auslesen
            for (int i = 0; i < new_generation_input.Length; i++)
            {
                if (new_generation_input[i].get_status() == "true")
                {
                    algos.algofeedbackarray[new_generation_input[i].get_generator()].number_individuals_survived++;
                }
            }

            //3. Initiative berechnen 
            for (int i = 0; i < algos.algofeedbackarray.Length; i++)
            {
                //Falls der Algo keine Individuen in der letzten Generation erzeugt hat, Initiative leicht erhöhen
                if (algos.algofeedbackarray[i].number_individuals_for_nextGen == 0) algos.algofeedbackarray[i].initiative += 0.5;
                //Ansonsten Initiative anpassen
                else algos.algofeedbackarray[i].initiative *= (((algos.algofeedbackarray[i].number_individuals_survived/algos.algofeedbackarray[i].number_individuals_for_nextGen)*(3/2))+0.5);
                initiativensumme += algos.algofeedbackarray[i].initiative;
            }
            if (applog.log) applog.appendText("Algo Manager: nemGen_composition: Initiativensumme: " + initiativensumme);

            //4. number_individuals_for_nextGen neu setzen
            for (int i = 0; i < algos.algofeedbackarray.Length; i++)
            {
                algos.algofeedbackarray[i].number_individuals_for_nextGen = (int)(((double)algos.algofeedbackarray[i].initiative / (double)initiativensumme) * (double)new_generation_input.Length);
                log = log + "[" + algos.algofeedbackarray[i].name + "]: Survived: " + algos.algofeedbackarray[i].number_individuals_survived + "/" + algos.algofeedbackarray[i].number_individuals_for_nextGen + " Initiative: " + algos.algofeedbackarray[i].initiative + " -> " + algos.algofeedbackarray[i].number_individuals_for_nextGen + " Individuen\r\n";
            }
            if (applog.log) applog.appendText("Algo Manager: nemGen_composition: Individuenverteilung für die neue Generation berechnen:\r\n" + log);
        }
        //Zeichnen der neuen Individuen und des Genpools
        public void draw(ref EVO.Common.Individuum_MetaEvo[] genpool, ref EVO.Diagramm.Hauptdiagramm hauptdiagramm_input)
        {
            //hauptdiagramm_input.
        }
    }


}
