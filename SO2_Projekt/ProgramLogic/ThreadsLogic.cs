using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SO2_Projekt.ProgramLogic
{
    public class ThreadsLogic
    {
        
        private Fork[] forks = new Fork[5];
        private Philosopher[] philosophers = new Philosopher[5];
        private Thread[] threads = new Thread[5];
        private static object eventLock = new object();

        public void StartSimulation()
        {
            for(int i=0; i<5; i++)
            {
                forks[i] = new Fork(i);
            }
            for(int i=0; i<5; i++)
            {
                philosophers[i] = new Philosopher(i);
                philosophers[i].TakeForkRequest += this.TakeFork;
                philosophers[i].GiveBackFork += this.GiveBackFork;
                philosophers[i].ProgresStateChanged += this.ProgresStateChanged;
                //Tworzenie wątków
                threads[i] = new Thread(philosophers[i].ThreadFunction);
                threads[i].Start();
            }
        }

        public delegate void TakeForkEventHandler(object sender, PhilosopherEventArgs args);
        public event TakeForkEventHandler TakeForkGUI;
        protected virtual void OnTakeForkGUI(PhilosopherEventArgs args)
        {
            if(TakeForkGUI != null)
            {
                TakeForkGUI(this, args);
            }
        }

        public delegate void GiveBackForkEventHandler(object sender, PhilosopherEventArgs args);
        public event GiveBackForkEventHandler GiveBackForkGUI;
        protected virtual void OnGiveBackForkGUI(PhilosopherEventArgs args)
        {
            if (GiveBackForkGUI != null)
            {
                GiveBackForkGUI(this, args);
            }
        }

        public delegate void ProgresStateChangedEventHandler(object sender, PhilosopherEventArgs args);
        public event ProgresStateChangedEventHandler ProgresStateChangedGUI;
        protected virtual void OnProgresStateChangedGUI(PhilosopherEventArgs args)
        {
            if (ProgresStateChangedGUI != null)
            { 
                ProgresStateChangedGUI(this, args);
            }
        }

        private void TakeFork(object sender, PhilosopherEventArgs args)
        {
            lock(eventLock)
            {
                bool available = CheckIfForkIsAvailable(args.ForkId);
                args.ForkAvailable = available;
                if(available)
                {
                    forks[args.ForkId].IsAvailable = false;
                }
                OnTakeForkGUI(args);
            }
        }
        
        private void GiveBackFork(object sender, PhilosopherEventArgs args)
        {
            lock(eventLock)
            {
                forks[args.ForkId].IsAvailable = true;
                OnGiveBackForkGUI(args);
            }
        }

        private void ProgresStateChanged(object sender, PhilosopherEventArgs args)
        {
            lock(eventLock)
            {
                OnProgresStateChangedGUI(args);
            }
        }

        private bool CheckIfForkIsAvailable(int id)
        {
            if (id >= 0 && id < 5)
            {
                return forks[id].IsAvailable;
            }
            else
            {
                return false;
            }
        }


    }
}
