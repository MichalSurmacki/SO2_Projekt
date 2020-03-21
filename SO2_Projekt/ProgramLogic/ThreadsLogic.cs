using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Terminal.Gui;

namespace SO2_Projekt.ProgramLogic
{
    //Klasa odpowiedzalna za inicjalizację filozofów, widelców, wątków
    /*Problem zagłodzenia rozwiązywany jest w taki sposób, że filozofowie jedzą "w kółko", tzn. na samym początku filozof sięga po widelec o niższym indeksie, jeśli uda mi się go chwycić,
      to czeka na widelec o wyższym indeksie (dla filozofa o indeksie 0 "niższym" indeksem widelca jest indeks 0). Kiedy widelec o wyższym indeksie zostaje zwolniony filozof, który na niego 
      czekał chwyta go i zaczyna proces "jedzenia". Następnie, odkłada widelce w odwrotnej kolejności do tej, w której je pobrał tj. odkłada widelec o wyższym indeksie, oraz o niższym
      - w ten sposób wyeliminowany jest porblem zagładzania - każdy filozof w, końcu dostanie 2 widelce.*/
    public class ThreadsLogic
    {
        private Fork[] forks = new Fork[5];
        private Philosopher[] philosophers = new Philosopher[5];
        private Thread[] threads = new Thread[5];

        public ThreadsLogic()
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(myHandler);
            for (int i = 0; i < 5; i++)
            {
                forks[i] = new Fork(i);
            }
            for (int i = 0; i < 5; i++)
            {
                philosophers[i] = new Philosopher(i, forks);
            }
        }

        public void StartSimulation()
        {
            for (int i = 0; i < 5; i++)
            {
                philosophers[i] = new Philosopher(i, forks);
                philosophers[i].TakeFork += GUI.OnForkTaken;
                philosophers[i].GiveBackFork += GUI.OnForkGivenBack;
                philosophers[i].ProgresStateChanged += GUI.OnProgresStateChanged;
                philosophers[i].LogAdded += GUI.OnLogAdded;

                threads[i] = new Thread(philosophers[i].ThreadFunction);
                threads[i].Start();
            }
        }

        protected void myHandler(object sender, ConsoleCancelEventArgs args)
        {
            for (int i = 0; i < 5; i++)
            {
                philosophers[i].IsThreadRunning = false;
            }

            Application.MainLoop.Invoke(() => Application.RequestStop());
            Environment.Exit(0);
        }
    }
}
