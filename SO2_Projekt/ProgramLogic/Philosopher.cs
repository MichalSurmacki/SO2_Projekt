using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Terminal.Gui;

namespace SO2_Projekt.ProgramLogic
{
    public class Philosopher
    {
        private float _hungerLevel;

        private static object eventLock = new object();

        private bool _haveLowerIndexFork;

        private bool _haveUpperIndexFork;

        public Fork[] forks = new Fork[5];

        public int Id { get; set; }

        public float HungerLevel
        {
            get => _hungerLevel;
            set
            {
                if ((value >= 0) && (value <= 1))
                {
                    _hungerLevel = value;
                }
                else
                {
                    //TOTO AFRICA: Tu chyba powinien pojawić się jakiś wyjątek :)
                }
            }
        }

        public bool IsThreadRunning { get; set; }

        public Philosopher(int id)
        {
            this.Id = id;
            this.HungerLevel = 0;
            this.IsThreadRunning = false;
        }

        //EVENT ODPOWIEDZIALNY ZA POPROSZENIE PRZEZ FILOZOFA O WIDELEC
        public delegate void TakeForkRequestEventHandler(object sender, PhilosopherEventArgs args);
        public event TakeForkRequestEventHandler TakeForkRequest;
        protected virtual bool OnTakeForkRequest(int forkId)
        {
            if (TakeForkRequest != null)
            {
                PhilosopherEventArgs args = new PhilosopherEventArgs() { PhilosopherId = this.Id, ForkId = forkId };
                TakeForkRequest(this, args);
                return args.ForkAvailable;
            }
            return false;
        }

        //EVENT ODPOWIEDZIALNY ZA ODŁOŻENIE PRZEZ FILOZOFA WIDELECA
        public delegate void GiveBackForkRequestEventHandler(object sender, PhilosopherEventArgs args);
        public event GiveBackForkRequestEventHandler GiveBackFork;
        protected virtual void OnGiveBackFork(int forkId)
        {
            if (GiveBackFork != null)
            {
                PhilosopherEventArgs args = new PhilosopherEventArgs() { PhilosopherId = this.Id, ForkId = forkId };
                GiveBackFork(this, args);
            }
        }

        public delegate void LogAddedEventHandler(object sender, PhilosopherEventArgs args);
        public event LogAddedEventHandler LogAdded;
        protected virtual void OnLogAdded(String log)
        {
            if (GiveBackFork != null)
            {
                String completeLog = "F." + this.Id + ": " + log;
                PhilosopherEventArgs args = new PhilosopherEventArgs() { Log = completeLog };
                LogAdded(this, args);
            }
        }

        public delegate void ProgresStateChangedEventHandler(object sender, PhilosopherEventArgs args);
        public event ProgresStateChangedEventHandler ProgresStateChanged;
        protected virtual void OnProgresStateChanged()
        {
            if (ProgresStateChanged != null)
            {
                PhilosopherEventArgs args = new PhilosopherEventArgs() { PhilosopherId = this.Id, HungerLevel = this.HungerLevel };
                ProgresStateChanged(this, args);
            }
        }

        public void ThreadFunction()
        {
            this.IsThreadRunning = true;
            Random random = new Random();
            int[] forksIds = new int[2] { -1, -1 };

            _haveUpperIndexFork = false;
            _haveLowerIndexFork = false;

            while (IsThreadRunning)
            {
                this.HungerLevel += 0.1f;
                OnLogAdded("Zgłodniałem (0.1)");
                OnLogAdded("Myślę proszę zostawić mnie w spokoju!!!");
                OnProgresStateChanged();
                this.SleepRandom();

                if (this.Id == 0)
                {
                    forksIds[0] = 0;
                    forksIds[1] = 4;
                }
                else
                {
                    forksIds[0] = this.Id - 1;
                    forksIds[1] = this.Id;
                }

                OnLogAdded("Spróbuję podnieść widelce...");
                //próbuj
                //WIDELEC O MNIEJSZYM INDEKSIE
                bool result = false;
                //Jeśli nie ma o niższym indeksie to próbuje go podnieść
                while (!_haveLowerIndexFork)
                {
                    lock (eventLock)
                    {
                        result = forks[forksIds[0]].IsAvailable;
                        if (result)
                        {
                            forks[forksIds[0]].IsAvailable = false;
                            OnTakeForkRequest(forksIds[0]);
                            _haveLowerIndexFork = true;
                            OnLogAdded("Wziąłem widelec o indeksie " + (forksIds[0] + 1) + ".");
                        }
                    }
                    this.HungerLevel += 0.01f;
                    OnProgresStateChanged();
                    Thread.Sleep(200);
                }

                //Jak już ma o niższym indeksie to próbuje wziąć o większym
                while (!_haveUpperIndexFork)
                {
                    lock (eventLock)
                    {
                        result = forks[forksIds[1]].IsAvailable;
                        if (result)
                        {
                            forks[forksIds[1]].IsAvailable = false;
                            OnTakeForkRequest(forksIds[1]);
                            _haveUpperIndexFork = true;
                            OnLogAdded("Wziąłem widelec o indeksie " + (forksIds[1] + 1) + ".");
                        }
                    }
                    this.HungerLevel += 0.01f;
                    OnProgresStateChanged();
                    Thread.Sleep(200);
                }

                OnLogAdded("Jem proszę mi nie przeszkadzać!!!");
                this.SleepRandom();
                this.HungerLevel = 0;
                OnProgresStateChanged();
                OnLogAdded("Skończyłem jeść... Czas odłożyć widelce.");
                //odkłada
                lock (eventLock)
                {
                    forks[forksIds[1]].IsAvailable = true;
                    OnGiveBackFork(forksIds[1]);
                    OnLogAdded("Odłożyłem widelec o indeksie " + (forksIds[1] + 1) + ".");
                }
                this._haveLowerIndexFork = false;
                lock (eventLock)
                {
                    forks[forksIds[0]].IsAvailable = true;
                    OnGiveBackFork(forksIds[0]);
                    OnLogAdded("Odłożyłem widelec o indeksie " + (forksIds[0] + 1) + ".");
                }
                _haveUpperIndexFork = false;
            }
        }
        private void SleepRandom()
        {
            Random random = new Random();
            int time = random.Next(1, 500);
            time += 1000;
            Thread.Sleep(time);
        }
    }
}
