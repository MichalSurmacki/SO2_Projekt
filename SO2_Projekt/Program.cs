using System;
using Terminal.Gui;
using SO2_Projekt.ProgramLogic;
using System.Threading;


namespace SO2_Projekt
{
    class Program
    {
        static void Main()
        {
            ThreadsLogic threadsLogic = new ThreadsLogic();

            GUI.ShowWelcomeMessage();

            GUI.Initialize();
            
            threadsLogic.StartSimulation();

            GUI.StartApplication();

        }
    }
}