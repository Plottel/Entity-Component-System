﻿using System;
namespace MyGame
{
    public class Entity
    {
        private int _id;

        public Entity (int id)
        {
            _id = id;
        }

        public int ID
        {
            get {return _id;}
            set {_id = value;}
        }
    }
}