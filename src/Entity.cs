using System;
namespace MyGame
{
    public class Entity
    {
        private int _id;
        private int _mask;

        public Entity (int id)
        {
            _id = id;
        }

        public int ID
        {
            get {return _id;}
            set {_id = value;}
        }

        public int Mask
        {
            get {return _mask;}
            set {_mask = value;}
        }
    }
}