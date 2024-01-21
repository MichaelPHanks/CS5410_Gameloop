using System;
using System.Collections.Generic;

namespace GameLoop
{
    public class MyGame
    {

        private DateTime prevTime;
        private string currentInput;
        

        private List<Event> events;
        private List<Event> eventsFired;

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
                currentInput += key;

                if (key.KeyChar.ToString() == " ")
                {
                    events.Add(new Event(TimeSpan.FromMilliseconds(1000),"Yeah",10));
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
