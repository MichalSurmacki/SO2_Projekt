using NStack;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace SO2_Projekt
{
    public class GUI : Window
    {

        public GUI() : base("SYSTEMY OPERACYJNE 2 - PROBLEM UCZTUJCYCH FILOZOFÓW")
        {

            var win = new Window("Hello")
            {
                X = Pos.Percent(0),
                Y = 0,
                Width = Dim.Percent(70),
                Height = Dim.Fill()
            };

            var win2 = new Window("Hello2")
            {
                X = Pos.Percent(70),
                Y = 0,
                Width = Dim.Percent(100),
                Height = Dim.Fill()
            };

            ProgressBar[] progres = new ProgressBar[5];

            Label[] labels = new Label[5];

            for(int i=0; i<5; i++)
            {
                labels[i] = new Label("Filozof " + (i+1) + ":")
                {
                    X = 0,
                    Y = i*2,
                    Width = 3,
                    Height = 1
                };
                win2.Add(labels[i]);

                progres[i] = new ProgressBar();
                progres[i].X = 0;
                progres[i].Fraction = 1;
                progres[i].Y = i*2+1;
                win2.Add(progres[i]);
            }

            this.Add(win);
            this.Add(win2);      
        }
    }
}
