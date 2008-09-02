﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace IHWB.EVO.HybridAlgo
{
    public class Controller
    {
        //### Variablen ###
        EVO.Common.Problem prob; 
        EVO.Common.EVO_Settings settings;
        EVO.Diagramm.Hauptdiagramm hauptdiagramm1;

        Networkmanager networkmanager;
        string role;
        EVO.Common.Individuum_MetaEvo[] generation;
        int individuumnumber;

        //### Konstruktor ###
        public Controller(ref EVO.Common.Problem prob_input, ref EVO.Common.EVO_Settings settings_input, ref EVO.Diagramm.Hauptdiagramm hauptdiagramm_input)  //Problem und Zeichner sollte noch übergeben werden
        {
            //Daten einlesen
            this.prob = prob_input;
            this.settings = settings_input;
            this.hauptdiagramm1 = hauptdiagramm_input;

            this.role = this.settings.MetaEvo.Role; 

            //Initialisieren des Individuum-Arrays
            individuumnumber = 0;
            generation = new EVO.Common.Individuum_MetaEvo[this.settings.MetaEvo.PopulationSize];
                //Setzen des Problems
            EVO.Common.Individuum_MetaEvo.Initialise(ref prob_input);
                //Füllen des Arrays
            for (int j = 0; j < this.settings.MetaEvo.PopulationSize; j++) 
            {
                generation[j] = new IHWB.EVO.Common.Individuum_MetaEvo("MetaEvo", individuumnumber);
                individuumnumber++;
            }
                //Zufällige Parents setzen
            set_random_adults();


            //Ausführen des Hauptprogramms (Single PC, Network Server, Network Client)
            switch (this.role)
            {
                case "Single PC":
                    start_single_pc();
                    break;
                case "Network Server":
                    networkmanager = new Networkmanager(ref this.generation[0], ref this.settings); 
                    start_network_server();
                    break;
                case "Network Client":
                    networkmanager = new Networkmanager(ref this.generation[0], ref this.settings); 
                    start_network_client();
                    break;
            }  
        }

        //### Methoden ###

        // Zufällige Eltern setzen
        private bool set_random_adults()
        {
            double[] random;
            Random randomizer = new Random();

            //Für jedes Individuum durchgehen
            for (int k = 0; k < this.prob.NumParams; k++)
            {
                random = new double[this.prob.NumParams];
                //Für jeden Parameter durchgehen
                for (int j = 0; j < this.prob.NumParams; j++)
                {
                    int max = (int)this.prob.List_OptParameter[j].Max;
                    int min = (int)this.prob.List_OptParameter[j].Min;
                    random[j] = randomizer.Next(min, max);
                }
                generation[k].set_genes(random);
            }
            return true;
        }

        // Single PC
        private void start_single_pc()
        {
            MessageBox.Show("Testmethode wird ausgeführt");
        }

        // Network Server
        private void start_network_server()
        {
            MessageBox.Show("Testmethode wird ausgeführt");
            /*
            ++ Server-Status "generate Individuums"
	            Server liest Individuen von DB
	            Server erzeugt Individuen
	            Server führt Scheduling durch falls Hash(Speed-Indizes auf 10sek gerundet) anders{
		            wh++
			            Finde Client (Aktuelle Rechenzeit + speed-index) am kleinsten ist.
			            in SchedTab Client: Individuums += 1 
		            --
	            }
	            Server speichert neue Individuen in Tabelle mit Anleitung von SchedTab
	            Server-Status "error watching"
	            x = 10
            --

            ++ Server-Status "error watching"
	            Falls mindestens ein Client Status: ready besitzt {
		            Rechner-tabelle laden 
		            x = 2
		            Prüfe auf Funktionalität der Clients ("error" = Now - Last Timestamp > 105% lowest speed) {
			            unfertige Individuen des clients demarkieren
			            in SchedTab -= setzen
			            new scheduling = true
		            }
		            Prüfe ungenaues Sched(Individuen unfertiger Clients * Speed-Index) + (Now - Timestamp) > kleinster speed-index {
			            "raw" Individuen des clients demarkieren
			            in SchedTab -= (summe-der-raw-individuen) setzen
			            new scheduling = true
		            }
		            Falls (new scheduling) {
			            Scheduling fortsetzen
		            }
		            Falls alle Individuen Status: ready sind {
			            Server-Status "generate Individuals"
		            }
	            }
            --

            delay x

            Nochmal aufrufen
             */
        }

        // Network Client
        private void start_network_client()
        {
            MessageBox.Show("Testmethode wird ausgeführt");
            /*
            ++ Client Status "ready"
	            Server-Status lesen -> Abbruch-Kriterium für Clients möglich
	            Falls Timestamp neu, dann 
		            Client-Status "calculating"
		            start = true
		            x = 0
            --

            ++ Client Status "calculating"
	            Falls Individuen-Status "Raw" und Rechner-IP die eigene existiert 
		            Markiert Individuum als Status "calculate", timestamp
		            Client simuliert
		            Client editiert Individuum mit Ergebnissen und Status "ready" oder "false", timestamp
		            [ende]Client schreibt in Rechner-Tabelle: Speed-Index [Durchschnitt der Berechnungen], 
	            Sonst
		            x = 20
		            Client Status "standby"
            --

            ++ Client Status "standby"
	            Falls Individuum mit für ihn markierter IP existiert
		            Client Status "claculating"
		            x = 0
	            else (Falls alle Individuen	Status:ready sind) {
		            Client Status "ready"
	            }
            --

            delay x

            Nochmal aufrufen
             */
        }
    }
}
