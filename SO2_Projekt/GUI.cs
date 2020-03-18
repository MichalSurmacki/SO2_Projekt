using NStack;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using SO2_Projekt.ProgramLogic;

namespace SO2_Projekt
{
    //Statyczna klasa opakowująca działania z wykorzystaniem biblioteki "Terminal.Gui"
    public static class GUI
    {
        private static Window[] forks = new Window[5];
        private static ColorScheme[] colorSchemes = new ColorScheme[6];
        private static ProgressBar[] progresState = new ProgressBar[5];
        private static object eventLock = new object();

        public static void Initialize()
        {
            Application.Init();
            var top = Application.Top;
            var main = new Window("SYSTEMY OPERACYJNE 2 - PROBLEM UCZTUJCYCH FILOZOFÓW");

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
            //********************************************************

            //DEFINICJE KOLORÓW POSZCZEGÓLNYCH FILOZOFÓW

            // pierwszy kolor tekstu, drugi kolor tła
            colorSchemes[0] = new ColorScheme();
            colorSchemes[0].Normal = Terminal.Gui.Attribute.Make(Color.Black, Color.Green);
            colorSchemes[1] = new ColorScheme();
            colorSchemes[1].Normal = Terminal.Gui.Attribute.Make(Color.Black, Color.Brown);
            colorSchemes[2] = new ColorScheme();
            colorSchemes[2].Normal = Terminal.Gui.Attribute.Make(Color.Black, Color.DarkGray);
            colorSchemes[3] = new ColorScheme();
            colorSchemes[3].Normal = Terminal.Gui.Attribute.Make(Color.Black, Color.Magenta);
            colorSchemes[4] = new ColorScheme();
            colorSchemes[4].Normal = Terminal.Gui.Attribute.Make(Color.Black, Color.White);
            colorSchemes[5] = new ColorScheme();
            colorSchemes[5].Normal = Terminal.Gui.Attribute.Make(Color.White, Color.BrightBlue);

            Label[] labelsState = new Label[5];

            for (int i = 0; i < 5; i++)
            {
                labelsState[i] = new Label("Filozof " + (i + 1) + ":")
                {
                    X = 0,
                    Y = i * 2,
                    Width = 3,
                    Height = 1
                };
                labelsState[i].ColorScheme = colorSchemes[i];

                progresState[i] = new ProgressBar()
                {
                    X = 0,
                    Fraction = 1,
                    Y = i * 2 + 1
                };
                progresState[i].ColorScheme = colorSchemes[i];

                winState.Add(labelsState[i], progresState[i]);
            }

            //Dodawanie kontrolelk/labelów itd. do okna "Wizualizacja"
            //********************************************************

            //OKNA JAKO REPREZENTACJA POSZCZEGÓLNYCH FILOZOFÓW
            Window[] philosophers = new Window[5];
            philosophers[0] = new Window("Filo.1")
            {
                X = Pos.Percent(40),
                Y = Pos.Percent(10),
                Width = Dim.Percent(33),
                Height = Dim.Percent(20)
            };
            philosophers[1] = new Window("Filo.2")
            {
                X = Pos.Percent(20),
                Y = Pos.Percent(30),
                Width = Dim.Percent(25),
                Height = Dim.Percent(25)
            };
            philosophers[2] = new Window("Filo.3")
            {
                X = Pos.Percent(20),
                Y = Pos.Percent(70),
                Width = Dim.Percent(25),
                Height = Dim.Percent(50)
            };
            philosophers[3] = new Window("Filo.4")
            {
                X = Pos.Percent(60),
                Y = Pos.Percent(70),
                Width = Dim.Percent(50),
                Height = Dim.Percent(50)
            };
            philosophers[4] = new Window("Filo.5")
            {
                X = Pos.Percent(60),
                Y = Pos.Percent(30),
                Width = Dim.Percent(50),
                Height = Dim.Percent(25)
            };

            //OKNA JAKO REPREZENTACJA POSZCZEGÓLNYCH WIDELCÓW
            forks[0] = new Window("1")
            {
                X = Pos.Percent(26.6666f),
                Y = Pos.Percent(15),
                Width = Dim.Percent(9),
                Height = Dim.Percent(11)
            };
            forks[1] = new Window("2")
            {
                X = Pos.Percent(26.6666f),
                Y = Pos.Percent(55),
                Width = Dim.Percent(9),
                Height = Dim.Percent(19.5f)
            };
            forks[2] = new Window("3")
            {
                X = Pos.Percent(46.6666f),
                Y = Pos.Percent(75),
                Width = Dim.Percent(12.5f),
                Height = Dim.Percent(35)
            };
            forks[3] = new Window("4")
            {
                X = Pos.Percent(66.6666f),
                Y = Pos.Percent(55),
                Width = Dim.Percent(20),
                Height = Dim.Percent(19.5f)
            };
            forks[4] = new Window("5")
            {
                X = Pos.Percent(66.6666f),
                Y = Pos.Percent(15),
                Width = Dim.Percent(20),
                Height = Dim.Percent(11)
            };

            for (int i = 0; i < 5; i++)
            {
                philosophers[i].ColorScheme = colorSchemes[i];
                winVisualisation.Add(philosophers[i], forks[i]);
            }

            main.Add(winVisualisation);
            main.Add(winState);
            top.Add(main);
        }

        public static void Run()
        {
            Application.Run();
        }

        public static void OnForkTaken(object source, PhilosopherEventArgs args)
        {
            Application.MainLoop.Invoke(() => {
                Random random = new Random();
                
                lock(eventLock)
                {
                    forks[args.ForkId].ColorScheme = colorSchemes[args.PhilosopherId];
                }
                
                Application.Refresh();
            });
        }

        public static void OnForkGivenBack(object source, PhilosopherEventArgs args)
        {
            Application.MainLoop.Invoke(() => {
                Random random = new Random();

                lock (eventLock)
                {
                    forks[args.ForkId].ColorScheme = colorSchemes[5];
                }

                Application.Refresh();
            });
        }

        public static void OnProgresStateChanged(object source, PhilosopherEventArgs args)
        {
            Application.MainLoop.Invoke(() => {
                lock (eventLock)
                {
                    progresState[args.PhilosopherId].Fraction = args.HungerLevel;
                }

                Application.Refresh();
            });
        }
    }
}
