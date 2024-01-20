using System;
using System.Collections.Generic;

namespace GameLoop
{
    public class MyGame
    {

        DateTime prevTime;

        // Create a list of events that exist
        // Create a list of events that fired (to have the render handle).
        
        /// <summary>
        /// Do any game initialization right here
        /// </summary>
        public void initialize()
        {
            prevTime = DateTime.Now;
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
        }

        /// <summary>
        /// Accept user input and update the simulation
        /// </summary>
        protected void update(TimeSpan elapsedTime)
        {
        }

        /// <summary>
        /// "Render" the state of the simulation
        /// </summary>
        protected void render()
        {
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
