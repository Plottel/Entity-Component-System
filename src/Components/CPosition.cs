using System;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the Position Component. This indicates where an Entity exists in the program and also 
    /// determines its bounding box used for collisions.
    /// </summary>
    public class CPosition : Component
    {
        /// <summary>
        /// The x position.
        /// </summary>
        public float X {get; set;}

        /// <summary>
        /// The y position.
        /// </summary>
        public float Y {get; set;}

        /// <summary>
        /// The width of the Entity.
        /// </summary>
        public int Width {get; set;}

        /// <summary>
        /// The height of the Entity.
        /// </summary>
        public int Height {get; set;}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CPosition"/> class.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="size">The size. Indicates that the Entity is a square.</param>
        public CPosition (float x, float y, int size) : this(x, y, size, size)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.CPosition"/> class.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width of the Entity.</param>
        /// <param name="height">The height of the Entity.</param>
        public CPosition(float x, float y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Returns a bounding box Rectangle derived from the x, y, width and height values.
        /// </summary>
        /// <value>The bounding box Rectangle.</value>
        public Rectangle Rect
        {
            get {return SwinGame.CreateRectangle(X, Y, Width, Height);}
        }

        /// <summary>
        /// Returns the centre point of the bounding box Rectangle.
        /// </summary>
        /// <value>The centre point of the bounding box Rectangle.</value>
        public Point2D Centre
        {
            get {return SwinGame.RectangleCenter(Rect);}
        }
    }
}