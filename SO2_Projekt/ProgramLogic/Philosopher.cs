using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Terminal.Gui;

namespace SO2_Projekt.ProgramLogic
{
    //Klasa odpowiedzialna za implementację zachowania filozofa (wątku)
    public class Philosopher
    {
        private float _hungerLevel;

        private static object _eventLock = new object();

        private bool _haveLowerIndexFork;

        private bool _haveUpperIndexFork;

        public Fork[] forks = new Fork[5];

        public int Id { get; set; }

        public float HungerLevel
        {
            get => _hungerLevel;
            set
            {
                //Watrość właściwości HungerLevel nie może przekroczyć 1
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

        public bool IsDone { get; set; }

        //Konstruktor klasy Philosopher - wymagana jest referencja do tablicy obiektów klasy Fork - zasób wspólny dla wszystkich filozofów
        public Philosopher(int id, Fork[] forks)
        {
            this.IsDone = false;
            this.forks = forks;
            this.Id = id;
            this.HungerLevel = 0;
            this.IsThreadRunning = false;
        }

        //Delegat dla wszystkich eventów dotyczących zmiany GUI
        public delegate void GUIEventHandler(object sender, PhilosopherEventArgs args);

        //Eventy
        public event GUIEventHandler TakeFork;
        public event GUIEventHandler GiveBackFork;
        public event GUIEventHandler LogAdded;
        public event GUIEventHandler ProgresStateChanged;

        //OPIS ZNAJDUJCYCH SIĘ NIŻEJ PUBLISHERÓW EVENTÓW
        //W Aplikcaji wyświetlanie wizualizacji w konsoli realizowane jest przez bibliotekę "Terminal.Gui", niżej znajdujący się publisherowie podnoszą eventy, które wychwytywane są przez subscriberów,
        //znajdujących się w klasie GUI. Subscriberowie znajdujący się w klasie GUI odpowiedzialni są jedynie za zmiany graficzne interfejsu użytkownika.

        //Publisher eventu TakeFork - event podnoszony jest, w momencie podniesienia widelca przez filozofa
        protected virtual bool OnTakeFork(int forkId)
        {
            if (TakeFork != null)
            {
                PhilosopherEventArgs args = new PhilosopherEventArgs() { PhilosopherId = this.Id, ForkId = forkId };
                TakeFork(this, args);
                return args.ForkAvailable;
            }
            return false;
        }
        //Publisher eventu GiveBackFork - event podnoszony jest w momencie odłożenia widelca przez filozofa
        protected virtual void OnGiveBackFork(int forkId)
        {
            if (GiveBackFork != null)
            {
                PhilosopherEventArgs args = new PhilosopherEventArgs() { PhilosopherId = this.Id, ForkId = forkId };
                GiveBackFork(this, args);
            }
        }
        //Publisher eventu OnLogAdded - event podnoszony jest w momencie dodania logu, do logów programu
        protected virtual void OnLogAdded(String log)
        {
            if (LogAdded != null)
            {
                String completeLog = "F." + this.Id + ": " + log;
                PhilosopherEventArgs args = new PhilosopherEventArgs() { Log = completeLog };
                LogAdded(this, args);
            }
        }
        //Publisher eventu ProgresStateChanged - event podnoszony jest w momencie zmiany stanu głodu filozofa
        protected virtual void OnProgresStateChanged()
        {
            if (ProgresStateChanged != null)
            {
                PhilosopherEventArgs args = new PhilosopherEventArgs() { PhilosopherId = this.Id, HungerLevel = this.HungerLevel };
                ProgresStateChanged(this, args);
            }
        }

        //Funkcja wątku
        public void ThreadFunction()
        {
            this.IsThreadRunning = true;
            Random random = new Random();
            int[] forksIds = new int[2] { -1, -1 };

            _haveUpperIndexFork = false;
            _haveLowerIndexFork = false;

            while (IsThreadRunning)
            {
                //Poziom głodu zwiększa się z każdą wykonaną pętlą
                this.HungerLevel += 0.1f;
                OnLogAdded("Zgłodniałem (0.1)");
                //Metoda SleepRandom() w tym miejscu reprezentuje proces myślenia przez filozofa
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
                bool result = false;
                //Jeśli filozof nie trzyma widelca o niższym indeksie to próbuje go podnieść
                while (!_haveLowerIndexFork)
                {
                    lock (_eventLock)
                    {
                        result = forks[forksIds[0]].IsAvailable;
                        if (result)
                        {
                            forks[forksIds[0]].IsAvailable = false;
                            OnTakeFork(forksIds[0]);
                            _haveLowerIndexFork = true;
                            OnLogAdded("Wziąłem widelec o indeksie " + (forksIds[0] + 1) + ".");
                        }
                    }
                    //W między czasie poziom jest głodu rośnie
                    this.HungerLevel += 0.01f;
                    OnProgresStateChanged();
                    Thread.Sleep(200);
                }

                //Jak filozof trzyma widelec o niższym indeksie to próbuje wziąć widelec o większym indeksie
                while (!_haveUpperIndexFork)
                {
                    lock (_eventLock)
                    {
                        result = forks[forksIds[1]].IsAvailable;
                        if (result)
                        {
                            forks[forksIds[1]].IsAvailable = false;
                            OnTakeFork(forksIds[1]);
                            _haveUpperIndexFork = true;
                            OnLogAdded("Wziąłem widelec o indeksie " + (forksIds[1] + 1) + ".");
                        }
                    }
                    //W między czasie poziom jest głodu rośnie
                    this.HungerLevel += 0.01f;
                    OnProgresStateChanged();
                    Thread.Sleep(200);
                }

                //Metoda SleepRandom() w tym miejscu reprezentuje okres jedzenia przez filozofa
                OnLogAdded("Jem proszę mi nie przeszkadzać!!!");
                this.SleepRandom();
                this.HungerLevel = 0;
                OnProgresStateChanged();
                OnLogAdded("Skończyłem jeść... Czas odłożyć widelce.");
                //Filozof odkłada widelce najpierw o indeksie większym, potem o indeksie mniejszym
                lock (_eventLock)
                {
                    forks[forksIds[1]].IsAvailable = true;
                    OnGiveBackFork(forksIds[1]);
                    OnLogAdded("Odłożyłem widelec o indeksie " + (forksIds[1] + 1) + ".");
                }
                this._haveLowerIndexFork = false;
                lock (_eventLock)
                {
                    forks[forksIds[0]].IsAvailable = true;
                    OnGiveBackFork(forksIds[0]);
                    OnLogAdded("Odłożyłem widelec o indeksie " + (forksIds[0] + 1) + ".");
                }
                _haveUpperIndexFork = false;
            }
            IsDone = true;
        }

        //Funkcja odpowiedzialna za usypianie wątku w granicach 1 - 1,5 sekundy
        private void SleepRandom()
        {
            Random random = new Random();
            int time = random.Next(1, 500);
            time += 1000;
            Thread.Sleep(time);
        }
    }
}
