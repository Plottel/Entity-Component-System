using System;
using SwinGameSDK;

namespace MyGame
{
    public class RenderableComponent : Component
    {
        private Color _color;
        
        public RenderableComponent (Color color)
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