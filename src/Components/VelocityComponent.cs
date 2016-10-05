using System;
namespace MyGame
{
    public class VelocityComponent : Component
    {
        private float _dx;
        private float _dy;
        private float _speed;

        public VelocityComponent (float dx, float dy, float speed)
        {
            _dx = dx;
            _dy = dy;
            _speed = speed;
        }

        public float DX
        {
            get {return _dx;}
            set {_dx = value;}
        }

        public float DY
        {
            get {return _dy;}
            set {_dy = value;}
        }

        public float Speed
        {
            get {return _speed;}
            set {_speed = value;}
        }
    }
}