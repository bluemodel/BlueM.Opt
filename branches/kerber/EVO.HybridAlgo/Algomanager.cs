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
        EVO.Common.Individuum_MetaEvo[] wastepool;
        EVO.Diagramm.ApplicationLog applog;
        Algos algos;

        public Algomanager(ref EVO.Common.Problem prob_input, int individuumnumber_input, ref EVO.Diagramm.ApplicationLog applog_input) 
        {
            applog = applog_input;
            //Algoobjekt initialisieren (enthält die algorithmus-Methoden und das Feedback zu jedem Algo)
            algos = new Algos(individuumnumber_input, ref applog);
        }

        public void set_genpool(ref EVO.Common.Individuum_MetaEvo[] genpool_input) 
        {
            this.wastepool = new EVO.Common.Individuum_MetaEvo[genpool_input.Length];
            this.genpool = new EVO.Common.Individuum_MetaEvo[genpool_input.Length];
            for (int i = 0; i < genpool_input.Length; i++)
            {
                this.genpool[i] = genpool_input[i].Clone_MetaEvo();
            }
            if (applog.log) applog.appendText("Algo Manager: Genpool: \r\n" + this.generationinfo(ref this.genpool));
        }

        //new_generation mit Genpool verarbeiten und neue Individuen in new_generation erzeugen
        public void new_individuals_merge_with_genpool(ref EVO.Common.Individuum_MetaEvo[] new_generation_input)
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

            //3.Genpool und neue Individuen zu neuem Genpool zusammenfassen, restliche Individuen in Wastepool verschieben 
            zip(ref genpool, ref new_generation_input);
            this.wastepool = new_generation_input;
            quicksort(ref wastepool, kriterium, 0, new_generation_input.Length - 1);
            quicksort(ref genpool, kriterium, 0, new_generation_input.Length - 1);
            if (applog.log) applog.appendText("Algo Manager: Result: New Genpool: \r\n" + this.generationinfo(ref genpool) + "\r\n");
        }

        public void new_individuals_build(ref EVO.Common.Individuum_MetaEvo[] new_generation_input)
        {
            //4.Generierung neuer Individuen (wieder in new_generation_input)
            algos.newGeneration(ref genpool, ref new_generation_input, ref wastepool);
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
            int dominator = -1;
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
                        for (int k = input.Length-1; k > i; k--)
                        {
                            if (input[k].get_status() == "true")
                            {
                                //Jede Eigenschaft vergleichen
                                for (int j = 0; j < input[0].Penalties.Length; j++)
                                {
                                    dominator = k;
                                    if (input[i].Penalties[j] > input[k].Penalties[j]) { dominator = -1; break; }
                                }
                                if (dominator > -1) {
                                    input[i].set_status("false#dominated#" + input[dominator].ID);
                                    if (applog.log) applog.appendText("Algo Manager: Domination: Individuum " + input[i].ID + " is dominated by Individuum " + input[dominator].ID);
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
                        for (int k = input2.Length - 1; k >= 0; k--)
                        {
                            if ((input2[k].get_status() == "true") && (input[i].ID != input2[k].ID))
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
                                    input2[k].set_status("false#dominated#" + input[i].ID);
                                    if (applog.log) applog.appendText("Algo Manager: Domination: Individuum " + input2[k].ID + " is dominated by Individuum " + input[i].ID);
                                    break; 
                                }
                                if (status == -1) { 
                                    status = 0;
                                    input[i].set_status("false#dominated#" + input2[k].ID);
                                    if (applog.log) applog.appendText("Algo Manager: Domination: Individuum " + input[i].ID + " is dominated by Individuum " + input2[k].ID);
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
                int pointer = 0;

                //Arbeits-Array erstellen
                EVO.Common.Individuum_MetaEvo[] work = new IHWB.EVO.Common.Individuum_MetaEvo[input.Length + maxindividuums];
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i].get_status() == "true")
                    {
                        work[pointer] = input[i];
                        pointer++;
                    }
                    if (input2[i].get_status() == "true")
                    {
                        work[pointer] = input2[i];
                        pointer++;
                    }
                }

                //Geringste Abstände finden
                for (int i = 0; i < work.Length - 1; i++)
                {
                    //mit
                    for (int k = i + 1; k < work.Length; k++)
                    {
                        distance = easydistance(work[i].Penalties, work[k].Penalties);
                        //Falls nur ein idnividuum entfernt werden muss
                        if ((individuums2kill.Length == 2) && (distance < individuums2kill[0, 1]))
                        {
                            individuums2kill[0, 0] = i;
                            individuums2kill[0, 1] = distance;
                        }
                        //Für mehrere Individuen
                        else
                        {
                            for (int j = individuums2kill.Length/2 - 1; j > 0; j--)
                            {
                                if (distance < individuums2kill[j, 1])
                                {
                                    individuums2kill[j, 0] = individuums2kill[j - 1, 0];
                                    individuums2kill[j, 1] = individuums2kill[j - 1, 1];
                                    individuums2kill[j - 1, 0] = i;
                                    individuums2kill[j - 1, 1] = distance;
                                }
                                else break;
                            }
                        }
                    }
                }
                //Zu nahe Individuen killen
                for (int i = 0; i < individuums2kill.Length/2; i++)
                {
                    work[(int)individuums2kill[i, 0]].set_status("false#crowding#0");
                    if (applog.log) applog.appendText("Algo Manager: Clustering: Individuum " + work[(int)individuums2kill[i, 0]].ID + " is not needed anymore");
                }
                //Zur überprüfung ob genug Individuen reduziert wurden
                clustering_kill(ref input, ref input2);
            }  
        }
        //Kopieren der Verbleibenden Individuen auf die Generation input
        private void zip(ref EVO.Common.Individuum_MetaEvo[] input, ref EVO.Common.Individuum_MetaEvo[] input2)
        {
            int pointer = 0;
            EVO.Common.Individuum_MetaEvo tmp;

            for (int i = 0; i < 2 * input.Length; i++)
            {
                if (i >= input.Length)
                {
                    if (input2[i - input.Length].get_status() == "true")
                    {
                        tmp = input2[i - input.Length];
                        input2[i - input.Length] = input[pointer];
                        input[pointer] = tmp;
                        pointer++;
                    }
                }
                else
                {
                    if (input[i].get_status() == "true")
                    {
                        tmp = input[i];
                        input[i] = input[pointer];
                        input[pointer] = tmp;
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
                back += (input1[i] - input2[i]) * (input1[i] - input2[i]);
            }
            return System.Math.Sqrt(back);
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
            double survivingrate = 0;
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
                else
                {
                    survivingrate = (double)algos.algofeedbackarray[i].number_individuals_survived / (double)algos.algofeedbackarray[i].number_individuals_for_nextGen;
                    algos.algofeedbackarray[i].initiative = Math.Round(algos.algofeedbackarray[i].initiative*(survivingrate * survivingrate + 0.5 * survivingrate + 0.5),3);
                }
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
        //Zeichnen der neuen Individuen
        public void draw_individuals(ref EVO.Common.Individuum_MetaEvo[] genpool, ref EVO.Diagramm.Hauptdiagramm hauptdiagramm_input)
        {
            //hauptdiagramm_input.
        }
        //Zeichnen des Genpools
        public void draw_genpool(ref EVO.Diagramm.Hauptdiagramm hauptdiagramm_input)
        {
            //hauptdiagramm_input.
        }
    }


}
