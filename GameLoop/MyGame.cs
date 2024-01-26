using System;
using System.Collections.Generic;
using System.Security;

namespace GameLoop
{
    public class MyGame
    {
        // Game data, including Event class

        // Holds info for previous game time
        private DateTime prevTime;

        // These two are for backspace, need to know whether to render a space or not
        private ConsoleKeyInfo consoleKey;
        private bool keyPressed = false;

        // If the enter is pressed, we display a new line
        private bool enterPressed = false;

        // Holds the current prompt that the user defines
        private string tempInput = string.Empty;

        // Holds the game loop in its loop until we quit
        private bool gameRunning = true;

        // These hold the events and events fired throughout the lifetime of the game
        private List<Event> events;
        private List<Event> eventsFired;

        // If there is an empty string (no current input), then we need to render a space
        // so the cursor doesn't move backward
        private bool emptyString = false;

        // First time in game, render the cmd prompt
        private bool initialRender = true;


        /// <summary>
        ///     Internal object for events
        /// </summary>
        private class Event
        {
            public TimeSpan interval;
            public TimeSpan timeToInterval;
            public string name;
            public int times;

            public Event(TimeSpan interval, string name, int times)
            {
                this.interval = interval;
                this.name = name;
                this.times = times;
                this.timeToInterval = interval;
            }


        }



        /// <summary>
        /// Do any game initialization right here
        /// </summary>
        public void initialize()
        {
            this.prevTime = DateTime.Now;
            this.events = new List<Event>();
            this.eventsFired = new List<Event>();
            Console.WriteLine("GameLoop Demo Initializing...");


        }

        /// <summary>
        /// The is the main "Game Loop".  This method does not return until the game is done
        /// </summary>
        public void run()
        {

            while (gameRunning)
            {
                TimeSpan elapsedTime = DateTime.Now - prevTime;

                this.prevTime = DateTime.Now;

                this.processInput();
                this.update(elapsedTime);
                this.render();
            }
        }

        private void processInput()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Backspace)
                {
                    if (this.tempInput.Length > 0)
                    {
                        this.tempInput = this.tempInput.Remove(this.tempInput.Length - 1);
                        this.keyPressed = true;
                        this.consoleKey = key;
                    }
                    else
                    {
                        this.emptyString = true;
                    }

                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    this.enterPressed = true;

                    string[] input = this.tempInput.Split(" ");

                    if (input[0].ToLower() == "quit")
                    {
                        this.gameRunning = false;
                    }

                    if (input.Length >= 5)
                    {
                        if (input[0].ToLower() == "create" && input[1].ToLower() == "event")
                        {
                            try
                            {
                                string name = input[2];

                                double timeSpan = double.Parse(input[3]);

                                int times = int.Parse(input[4]);

                                TimeSpan interval = TimeSpan.FromMilliseconds(timeSpan);

                                this.events.Add(new Event(interval, name, times));
                            }

                            catch { }

                        }

                    }
                    this.tempInput = "";
                }
                else
                {
                    this.tempInput += key.KeyChar.ToString();

                }
            }
        }

        /// <summary>
        /// Accept user input and update the simulation
        /// </summary>
        protected void update(TimeSpan elapsedTime)
        {


            this.eventsFired.Clear();

            for (int i = 0; i < this.events.Count; i++)
            {
                this.events[i].timeToInterval -= elapsedTime;

                if (this.events[i].timeToInterval <= TimeSpan.Zero)
                {
                    this.events[i].timeToInterval = this.events[i].interval;
                    this.events[i].times -= 1;
                    eventsFired.Add(this.events[i]);




                }
            }

            this.events.RemoveAll(IsExpired);


        }

        private bool IsExpired(Event e)
        {
            return e.times <= 0;
        }

        /// <summary>
        /// "Render" the state of the simulation
        /// </summary>  
        protected void render()
        {
            int firedEvents = 0;
            if (this.eventsFired.Count > 0)
            {
                Console.WriteLine();
                for (int i = 0; i < this.eventsFired.Count; i++)
                {
                    firedEvents++;
                    Console.WriteLine("\tEvent: " + this.eventsFired[i].name + " (" + this.eventsFired[i].times + "remaining)");
                }
            }
            if (firedEvents > 0)
            {
                // Reprint the command line and current user input
                Console.Write("[cmd:] ");
                Console.Write(this.tempInput);
            }

            if (this.keyPressed)
            {
                Console.Write(" ");
                Console.Write(consoleKey.KeyChar.ToString());
                this.keyPressed = false;
            }
            if (this.enterPressed)
            {
                this.enterPressed = false;
                Console.WriteLine();
                Console.Write("[cmd:] ");
            }
            if (this.emptyString)
            {
                this.emptyString = false;
                Console.Write(" ");

            }
            if (this.initialRender) 
            {
                Console.Write("[cmd:] ");
                this.initialRender = false;
            }


        }

        /// <summary>
        /// Examine the input buffer and see if there are any commands to execute
        /// </summary>
        private bool executeCommand(string commandString)
        {
            return false;
        }



    }
}