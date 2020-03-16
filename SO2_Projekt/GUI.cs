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
            var winVisualisation = new Window("Wizualizacja")
            {
                X = Pos.Percent(0),
                Y = 0,
                Width = Dim.Percent(70),
                Height = Dim.Fill()
            };

            var winState = new Window("Stan Filozofów:")
            {
                X = Pos.Percent(70),
                Y = 0,
                Width = Dim.Percent(100),
                Height = Dim.Fill()
            };

            //Dodawanie kontrolek/labelów itd. do okna "Stan Filozofów"

            //DEFINICJE KOLORÓW POSZCZEGÓLNYCH FILOZOFÓW
            ColorScheme[] colorSchemes = new ColorScheme[5];
            colorSchemes[0] = new ColorScheme();
            // pierwszy kolor tekstu, drugi kolor tła
            colorSchemes[0].Normal = Terminal.Gui.Attribute.Make(Color.Black, Color.Green);
            colorSchemes[1] = new ColorScheme();
            colorSchemes[1].Normal = Terminal.Gui.Attribute.Make(Color.Black, Color.Brown);
            colorSchemes[2] = new ColorScheme();
            colorSchemes[2].Normal = Terminal.Gui.Attribute.Make(Color.Black, Color.DarkGray);
            colorSchemes[3] = new ColorScheme();
            colorSchemes[3].Normal = Terminal.Gui.Attribute.Make(Color.Black, Color.Magenta);
            colorSchemes[4] = new ColorScheme();
            colorSchemes[4].Normal = Terminal.Gui.Attribute.Make(Color.Black, Color.White);



            ProgressBar[] progres = new ProgressBar[5];

            Label[] labelsState = new Label[5];

            for(int i=0; i<5; i++)
            {
                labelsState[i] = new Label("Filozof " + (i+1) + ":")
                {
                    X = 0,
                    Y = i*2,
                    Width = 3,
                    Height = 1
                };
                winState.Add(labelsState[i]);
                labelsState[i].ColorScheme = colorSchemes[i];

                progres[i] = new ProgressBar()
                {
                    X = 0,
                    Fraction = 1,
                    Y = i * 2 + 1
                };

                progres[i].ColorScheme = colorSchemes[i];

                winState.Add(progres[i]);
            }

            //Dodawania kontrolelk/labelów itd. do okna "Wizualizacja"

            int xOffset = 25;


            //OKNA JAKO REPREZENTACJA POSZCZEGÓLNYCH FILOZOFÓW
            Window[] philosophers = new Window[5];
            philosophers[0] = new Window(new Rect(xOffset + 10, 1, 10, 5), "Filo.1");
            philosophers[1] = new Window(new Rect(xOffset + 0, 7, 10, 5), "Filo.2");
            philosophers[2] = new Window(new Rect(xOffset + 0, 17, 10, 5), "Filo.3");
            philosophers[3] = new Window(new Rect(xOffset + 20, 7, 10, 5), "Filo.4");
            philosophers[4] = new Window(new Rect(xOffset + 20, 17, 10, 5), "Filo.5");

            //OKNA JAKO REPREZENTACJA POSZCZEGÓLNYCH WIDELCÓW
            Window[] forks = new Window[5];
            forks[0] = new Window(new Rect(xOffset + 2, 2, 6, 3), "1");
            forks[1] = new Window(new Rect(xOffset + 22, 2, 6, 3), "2");
            forks[2] = new Window(new Rect(xOffset + 2, 13, 6, 3), "3");
            forks[3] = new Window(new Rect(xOffset + 22, 13, 6, 3), "4");

            forks[4] = new Window(new Rect(xOffset + 12, 18, 6, 3), "5");



            for (int i=0; i<5; i++)
            {
                philosophers[i].ColorScheme = colorSchemes[i];
                winVisualisation.Add(philosophers[i], forks[i]);                
            }

            this.Add(winVisualisation);
            this.Add(winState);      
        }
    }
}
