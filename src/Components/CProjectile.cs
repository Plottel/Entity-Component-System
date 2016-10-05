using System;
using SwinGameSDK;

namespace MyGame
{
    public class CProjectile : Component
    {
        private Rectangle _target;

        public CProjectile (Rectangle target)
        {
            _target = target;
        }

        public Rectangle Target
        {
            get {return _target;}
            set {_target = value;}
        }
    }
}