using System;
using SwinGameSDK;

namespace MyGame
{
    public class CPosition : Component
    {
        private float _x;
        private float _y;
        private int _width;
        private int _height;

        public CPosition (float x, float y, int size) : this(x, y, size, size)
        {
        }

        public CPosition(float x, float y, int width, int height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public float X
        {
            get {return _x;}
            set {_x = value;}
        }

        public float Y
        {
            get {return _y;}
            set {_y = value;}
        }

        public int Width
        {
            get {return _width;}
            set {_width= value;}
        }

        public int Height
        {
            get {return _height;}
            set {_height = value;}
        }

        public Rectangle Rect
        {
            get {return SwinGame.CreateRectangle(X, Y, Width, Height);}
        }

        public Point2D Centre
        {
            get {return SwinGame.RectangleCenter(Rect);}
        }
    }
}