using System;
using System.Collections.Generic;
using System.Text;

namespace SO2_Projekt.ProgramLogic
{
    public class Fork
    {
        public int Id { get; set; }
        public bool IsAvailable { get; set; }

        public Fork(int id)
        {
            IsAvailable = true;
            Id = id;
        }
    }
}
