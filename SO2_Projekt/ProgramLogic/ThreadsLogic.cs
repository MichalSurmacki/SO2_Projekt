using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SO2_Projekt.ProgramLogic
{
    public class ThreadsLogic
    {
        public Philosopher[] philosophers = new Philosopher[5];
        private Thread[] threads = new Thread[5];


        public void StartSimulation()
        {
            for(int i=0; i<5; i++)
            {
                philosophers[i] = new Philosopher(i);
                //Tworzenie wątków
                threads[i] = new Thread(philosophers[i].ThreadFunction);
                threads[i].Start();
            }
        }


    }
}
