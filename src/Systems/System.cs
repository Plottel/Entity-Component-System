using System;
using System.Collections.Generic;

namespace MyGame
{
    public abstract class System : EntityListener
    {
        private List<int> _entities;
        private List<Type> _inclusionMask;
        private List<Type> _exclusionMask;
        private World _world;

        protected System(List<Type> inclusionMask, List<Type> exclusionMask, World world)
        {
            _entities = new List<int>();
            _inclusionMask = inclusionMask;
            _exclusionMask = exclusionMask;
            _world = world;
        }

        public List<int> Entities
        {
            get {return _entities;}
            set {_entities = value;}
        }

        public List<Type> InclusionMask
        {
            get {return _inclusionMask;}
            set {_inclusionMask = value;}
        }

        public List<Type> ExclusionMask
        {
            get {return _exclusionMask;}
            set {_exclusionMask = value;}
        }

        protected World World
        {
            get {return _world;}
            set {_world = value;}
        }

        //Optimise with key - Contains
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

        public bool EntityPassesFilter(int entID)
        {
            Dictionary<Type, Component> entMask = World.GetAllComponentsOfEntity(entID);

            foreach (Type t in InclusionMask)
            {
                if (!entMask.ContainsKey(t))
                {
                    return false;
                }
            }

            foreach (Type t in ExclusionMask)
            {
                if (entMask.ContainsKey(t))
                {
                    return false;
                }
            }
            return true;
        }

        public abstract void Process();

        public void Add(int entID)
        {
            Entities.Add(entID);
        }

        public virtual void Remove(int entID)
        {
            Entities.Remove(entID);
        }
    }
}