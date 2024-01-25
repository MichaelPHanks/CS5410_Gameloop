using System;
using System.Collections.Generic;
using System.Security;

namespace GameLoop
{
    public class MyGame
    {

        private DateTime prevTime;
        private ConsoleKeyInfo consoleKey;
        private bool keyPressed = false;
        private bool enterPressed = false;
        private string tempInput = string.Empty;

        private bool gameRunning = true;

        private List<Event> events;
        private List<Event> eventsFired;
        private List<string> currentInput;

        /// <summary>
        /// Internal object for events
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
            this.currentInput = new List<string>();
            Console.WriteLine("GameLoop Demo Initializing...");

            // TODO: Move this to render
            Console.Write("[cmd:] ");

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
                    //tempInput = tempInput.Trim();
                    if (tempInput.Length > 0)
                    {
                        tempInput = tempInput.Remove(tempInput.Length - 1);
                        this.keyPressed = true;
                        this.consoleKey = key;
                    }
                    else
                    {
                        Console.Write(" ");
                    }

                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    string[] input = tempInput.Split(" ");

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
                    tempInput = "";
                    // Do some stuff here
                }
                else
                {
                    tempInput += key.KeyChar.ToString();

                }
            }

            /*if (Console.KeyAvailable) 
            { 
                var key = Console.ReadKey();

                if (key.KeyChar.ToString() == " ")
                {
                    this.currentInput.Add("");
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (this.currentInput.Count > 0)
                    {
                        if (this.currentInput[this.currentInput.Count - 1].Length == 0)
                        {
                            // Remove the spot in the list
                            this.currentInput.RemoveAt(this.currentInput.Count - 1);
                        }
                        else if (this.currentInput[this.currentInput.Count - 1].Length > 0)
                        {
                            string tempInput = this.currentInput[this.currentInput.Count - 1];

                            tempInput = tempInput.Remove(tempInput.Length - 1);

                            this.currentInput[this.currentInput.Count - 1] = tempInput;

                            if (tempInput == "")
                            {
                                this.currentInput.RemoveAt(this.currentInput.Count - 1);
                                //Console.Write(" ");

                            }


                        }

                        this.consoleKey = key;
                        this.keyPressed = true;

                    }
                    else
                    {
                        Console.Write(" ");
                    }
                    Console.Write("");
                    
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    this.enterPressed = true;
                    if (this.currentInput.Count >= 5) 
                    {
                        if (this.currentInput[0].ToLower() == "create" && this.currentInput[1].ToLower() == "event")
                        {
                            try {
                                string name = this.currentInput[2];

                                double timeSpan = double.Parse(this.currentInput[3]);

                                int times = int.Parse(this.currentInput[4]);

                                TimeSpan interval = TimeSpan.FromMilliseconds(timeSpan);

                                this.events.Add(new Event(interval, name, times));
                            } 
                            
                            catch { }
                        }
                    }
                    this.currentInput.Clear();
                }
                else
                {
                    if (this.currentInput.Count == 0)
                    {
                        this.currentInput.Add("");
                    }
                    this.currentInput[this.currentInput.Count - 1] += key.KeyChar.ToString();
                }

            }*/



        }

        /// <summary>
        /// Accept user input and update the simulation
        /// </summary>
        protected void update(TimeSpan elapsedTime)
        {

            for(int i = 0; i < this.events.Count; i++) 
            {
                this.events[i].timeToInterval -= elapsedTime;

                if (this.events[i].timeToInterval <= TimeSpan.Zero)
                {
                    this.events[i].timeToInterval = this.events[i].interval;
                    this.events[i].times -= 1;
                    eventsFired.Add(this.events[i]);

                    if (this.events[i].times <= 0)
                    {
                        this.events.RemoveAt(i);
                        i--;
                    }


                }
            }
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
                    Console.WriteLine("\tEvent: "+ this.eventsFired[i].name + "("+ this.eventsFired[i].times + "remaining)");
                }
            }
            if (firedEvents > 0) 
            {
                this.eventsFired.Clear();
                // Reprint the command line
                Console.Write("[cmd:] ");

                /*for (int i = 0;i < this.currentInput.Count; i++)
                {
                    Console.Write(this.currentInput[i]);
                    if (i != this.currentInput.Count - 1)
                    {
                        Console.Write(" ");

                    }

                }*/
                Console.Write(tempInput);
                //Console.Write();
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
                Console.WriteLine() ;
                Console.Write("[cmd:] ");
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
