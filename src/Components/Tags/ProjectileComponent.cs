using System;
using SwinGameSDK;

namespace MyGame
{
    public class ProjectileComponent : Component
    {
        private Rectangle _target;

        public ProjectileComponent (Rectangle target)
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