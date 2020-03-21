using System;
using System.Collections.Generic;
using System.Text;

namespace SO2_Projekt.ProgramLogic
{
    public class PhilosopherEventArgs : EventArgs
    {
        public int PhilosopherId { get; set; }
        public float HungerLevel { get; set; }
        public int ForkId { get; set; }
        /// <summary>
        /// Domyślnie właściwość ustawiana jest na wartość - false
        /// </summary>
        public bool ForkAvailable { get; set; }
        public String Log { get; set; }

        public PhilosopherEventArgs()
        {
            Log = String.Empty;
            HungerLevel = -1;
            PhilosopherId = -1;
            ForkId = -1;
            ForkAvailable = false;
        }
    }
}
