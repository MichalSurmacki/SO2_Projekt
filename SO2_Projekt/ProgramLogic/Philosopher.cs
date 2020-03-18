using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Terminal.Gui;

namespace SO2_Projekt.ProgramLogic
{
    public class Philosopher
    {
        int[] debug = new int[2];

        private float _hungerLevel;

        public int Id { get; set; }
        
        public bool IsThinking { get; set; }

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
            this.IsThinking = true;
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

        //DELEGAT
        public delegate void ProgresStateChangedEventHandler(object sender, PhilosopherEventArgs args);
        //EVENT
        public event ProgresStateChangedEventHandler ProgresStateChanged;
        //PUBLISHER
        protected virtual void OnProgresStateChanged()
        {
            if (ProgresStateChanged != null)
            {
                PhilosopherEventArgs args = new PhilosopherEventArgs() { PhilosopherId = this.Id, HungerLevel = this.HungerLevel };
                ProgresStateChanged(this, args);
            }
        }

        private void SleepRandom()
        {
            Random random = new Random();
            //int time = random.Next(1, 500);
            int time = 1000;
            Thread.Sleep(time);
        }

        public void ThreadFunction()
        {
            this.IsThreadRunning = true;
            Random random = new Random();
            int[] forksIds = new int[2] { -1, -1 };

            while (IsThreadRunning)
            {
                if (IsThinking)
                {
                    Console.WriteLine("elo z " + this.Id);
                    this.HungerLevel += 0.05f;
                    this.SleepRandom();
                    //próbuje
                    //WIDELEC O MNIEJSZYM INDEKSIE
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
                    Console.WriteLine(Id + " Chce wziąć o indeksie " + forksIds[0]);
                    bool result = OnTakeForkRequest(forksIds[0]);
                    Console.WriteLine(Id + " " + result);
                    if (result)
                    {
                        Console.WriteLine(Id + " Chce wziąć o indeksie " + forksIds[1]);
                        result = OnTakeForkRequest(forksIds[1]);
                        Console.WriteLine(Id + " " + result);
                    }

                    //jak result == true to wziął oba
                    if(result)
                    {
                        debug[1] = 1;
                        IsThinking = false;
                        continue;
                        Console.WriteLine(Id + "Koontynuje");
                        //kontynuacja jedzonka
                    }
                    else
                    {
                        IsThinking = true;
                        HungerLevel += 0.05f;
                        OnProgresStateChanged();
                        this.SleepRandom();
                    }
                }
                else
                {
                    Console.WriteLine(Id + " JEM " + forksIds[0]);
                    //je
                    //this.SleepRandom();
                    Thread.Sleep(1000);
                    this.HungerLevel = 0;
                    OnProgresStateChanged();
                    //odkłada
                    Console.WriteLine(Id + " oddaje o indeksie " + forksIds[1]);
                    OnGiveBackFork(forksIds[1]);
                    debug[0] = 0;
                    Console.WriteLine(Id + " oddaje o indeksie " + forksIds[0]);
                    OnGiveBackFork(forksIds[0]);
                    //Application.Refresh();
                    debug[1] = 0;
                    IsThinking = true;
                }
            }
        }

    }
}
