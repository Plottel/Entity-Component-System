using System;
using System.Collections.Generic;

namespace MyGame
{
    public abstract class System : EntityListener
    {
        private List<int> _entities;
        private int _mask;
        private World _world;

        protected System(int mask, World world)
        {
            _entities = new List<int>();
            _mask = mask;
            _world = world;
        }

        public List<int> Entities
        {
            get {return _entities;}
            set {_entities = value;}
        }

        public int Mask
        {
            get {return _mask;}
            set {_mask = value;}
        }

        protected World World
        {
            get {return _world;}
            set {_world = value;}
        }

        public bool HasEntity(int entID)
        {
            foreach (int e in Entities)
            {
                if (e == entID)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool EntityHasRequiredComponents(int entMask)
        {
            return (entMask & Mask) == Mask;
        }

        public abstract void Process();

        public void Add(int entID)
        {
            Entities.Add(entID);
        }

        public void Remove(int entID)
        {
            Entities.Remove(entID);
        }
    }
}