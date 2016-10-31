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
        private float _x;

        /// <summary>
        /// The y position.
        /// </summary>
        private float _y;

        /// <summary>
        /// The width of the Entity.
        /// </summary>
        private int _width;

        /// <summary>
        /// The height of the Entity.
        /// </summary>
        private int _height;

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
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Gets or sets the x position.
        /// </summary>
        /// <value>The x position.</value>
        public float X
        {
            get {return _x;}
            set {_x = value;}
        }

        /// <summary>
        /// Gets or sets the y position.
        /// </summary>
        /// <value>The y position.</value>
        public float Y
        {
            get {return _y;}
            set {_y = value;}
        }

        /// <summary>
        /// Gets or sets the width of the Entity.
        /// </summary>
        /// <value>The width of the Entity.</value>
        public int Width
        {
            get {return _width;}
            set {_width= value;}
        }

        /// <summary>
        /// Gets or sets the height of the Entity.
        /// </summary>
        /// <value>The height of the Entity.</value>
        public int Height
        {
            get {return _height;}
            set {_height = value;}
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