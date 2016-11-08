using System;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the class containing various utility functions for the program.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Checks the passed in start time and duration and evalutes them agains the Game Time.
        /// This is used for functionality such as checking cooldowns and whether or not effects have expired.
        /// </summary>
        /// <returns><c>true</c>, the duration has been reached, <c>false</c> otherwise.</returns>
        /// <param name="startTime">The start time.</param>
        /// <param name="duration">How long the timer goes for.</param>
        public static bool DurationReached(uint startTime, int duration)
        {
            return (World.GameTime - startTime >= duration);
        }

        /// <summary>
        /// Calculates the appropriate X and Y velocities for an Entity in order to reach
        /// a specified location. This also takes into account the Entity's speed
        /// </summary>
        /// <returns>A Vector containing the X and Y velocities.</returns>
        /// <param name="x">The x coordinate of the Entity.</param>
        /// <param name="y">The y coordinate of the Entity.</param>
        /// <param name="targetX">The x coordinate of the Target.</param>
        /// <param name="targetY">The y coordinate of the Target.</param>
        /// <param name="speed">The speed of the Entity.</param>
        public static Vector GetVectorBetweenPoints(float x, float y, float targetX, float targetY, float speed)
        {
            //How far away the Entity is from the target on each axis
            float xOffset = (targetX - x);
            float yOffset = (targetY - y);

            //Pythagoras calculating the direct distance between the Entity and target.
            float vectorLength = (float)Math.Sqrt((xOffset * xOffset) + (yOffset * yOffset));

            //The x and y velocities
            float xVel = (xOffset / vectorLength) * speed;
            float yVel = (yOffset / vectorLength) * speed;

            return SwinGame.VectorTo(xVel, yVel);
        }
    }
}