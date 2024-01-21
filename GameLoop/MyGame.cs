using System;
using System.Collections.Generic;
using System.Security;

namespace GameLoop
{
    public class MyGame
    {

        private DateTime prevTime;
        

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
            prevTime = DateTime.Now;
            events = new List<Event>();
            eventsFired = new List<Event>();
            currentInput = new List<string>();
            Console.Write("[cmd:] ");

        }

        /// <summary>
        /// The is the main "Game Loop".  This method does not return until the game is done
        /// </summary>
        public void run()
        {
            bool done = false;

            while (!done)
            { 
                TimeSpan elapsedTime = DateTime.Now - prevTime;

                prevTime = DateTime.Now;

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

                if (key.KeyChar.ToString() == " ")
                {
                    currentInput.Add("");
                    events.Add(new Event(TimeSpan.FromMilliseconds(1000),"Yeah",10));
                }
                else if (key.Key == ConsoleKey.Backspace) 
                {
                    if (currentInput[currentInput.Count - 1].Length == 0) 
                    { 
                        // Remove the spot in the list
                        currentInput.RemoveAt(currentInput.Count - 1);
                    }
                }
                else
                {
                    if (currentInput.Count == 0)
                    {
                        currentInput.Add("");
                    }
                    currentInput[currentInput.Count - 1] += key.Key.ToString();
                }

            }
            
                /*while (Console.KeyAvailable)
                {
                    
                }*/
            
        }

        /// <summary>
        /// Accept user input and update the simulation
        /// </summary>
        protected void update(TimeSpan elapsedTime)
        {

            for(int i = 0; i < events.Count; i++) 
            {
                events[i].timeToInterval -= elapsedTime;

                if (events[i].timeToInterval <= TimeSpan.Zero)
                {
                    events[i].timeToInterval = events[i].interval;
                    events[i].times -= 1;
                    eventsFired.Add(events[i]);

                    if (events[i].times <= 0)
                    {
                        events.RemoveAt(i);
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
            if (eventsFired.Count > 0)
            {
                Console.WriteLine();
                for (int i = 0; i < eventsFired.Count; i++)
                {
                    firedEvents++;
                    Console.WriteLine(eventsFired[i].name + eventsFired[i].times);
                }
            }
            if (firedEvents > 0) 
            {
                eventsFired.Clear();
                // Reprint the command line
                Console.Write("[cmd:] ");

                for (int i = 0;i < currentInput.Count; i++)
                {
                    Console.Write(currentInput[i]);
                    Console.Write(" ");
                    
                }
                //Console.Write();
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
