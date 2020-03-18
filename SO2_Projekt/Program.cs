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

            //GUI gui = new GUI();
            GUI.Initialize();


            ThreadsLogic threadsLogic = new ThreadsLogic();
            //Philosopher p = new Philosopher(1);
            //threadsLogic.philosophers[0].ForkTaken += GUI.OnForkTaken;

            threadsLogic.StartSimulation();

            //Thread t = new Thread(fun);
            //t.Start();

            //p.ForkTaken += GUI.OnForkTaken;

            GUI.Run();
        }
    }
}