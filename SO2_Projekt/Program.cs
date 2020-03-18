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
            GUI.Initialize();


            ThreadsLogic threadsLogic = new ThreadsLogic();
            threadsLogic.StartSimulation();

            threadsLogic.GiveBackForkGUI += GUI.OnForkGivenBack;

            threadsLogic.ProgresStateChangedGUI += GUI.OnProgresStateChanged;

            threadsLogic.TakeForkGUI += GUI.OnForkTaken;

            //Thread t = new Thread(fun);
            //t.Start();

            //p.ForkTaken += GUI.OnForkTaken;

            GUI.Run();
        }
    }
}