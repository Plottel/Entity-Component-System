using System;
using SwinGameSDK;

namespace MyGame
{
    public class CRenderable : Component
    {
        private Color _color;
        
        public CRenderable (Color color)
        {
            _color = color;
        }

        public Color Color
        {
            get {return _color;}
            set {_color = value;}
        }
    }
}