using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SO2_Projekt.ProgramLogic
{
    public class Philosopher
    {
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

        //DELEGAT
        public delegate void ForkTakenEventHandler(int id);
        //EVENT
        public event ForkTakenEventHandler ForkTaken;
        //PUBLISHER
        protected virtual void OnForkTaken()
        {
            if(ForkTaken != null)
            {
                ForkTaken(Id);
            }
        }

        //DELEGAT
        public delegate void ProgresStateChangedEventHandler(int id, float value);
        //EVENT
        public event ProgresStateChangedEventHandler ProgresStateChanged;
        //PUBLISHER
        protected virtual void OnProgresStateChanged()
        {
            if (ProgresStateChanged != null)
            {
                ProgresStateChanged(Id, HungerLevel);
            }
        }

        private void SleepRandom()
        {
            Random random = new Random();
            int time = random.Next(3, 6);
            Thread.Sleep(time);
        }

        private void Eat()
        {

            //Wzięcie widelca o mniejszym indeksie

            //Wzięcie widelca o większym indeksie

            //jem

            //Odkładam o większym indeksie

            //i o mniejszym
        }

        public void ThreadFunction()
        {
            this.IsThreadRunning = true;
            Random random = new Random();
            this.ForkTaken += GUI.OnForkTaken;
            this.ProgresStateChanged += GUI.OnProgresStateChanged;
            while(IsThreadRunning)
            {
                if(IsThinking)
                {
                    this.HungerLevel += 0.05f;
                    //próbuje

                }
                else
                {
                    //je 
                    Thread.Sleep(1000);
                    this.HungerLevel = Convert.ToSingle(random.NextDouble());
                    OnProgresStateChanged();
                    OnForkTaken();
                    //odkłada
                }
            }
        }

    }
}
