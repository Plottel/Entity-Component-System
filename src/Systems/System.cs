using System;
using System.Collections.Generic;

namespace MyGame
{
    public abstract class System : EntityListener
    {
        private List<int> _entities;
        private List<Type> _include;
        private List<Type> _exclude;
        private World _world;

        protected System(List<Type> include, List<Type> exclude, World world)
        {
            _entities = new List<int>();
            _include = include;
            _exclude = exclude;
            _world = world;
        }

        public World World
        {
            get {return _world;}
            set {_world = value;}
        }

        public List<int> Entities
        {
            get {return _entities;}
            set {_entities = value;}
        }

        public List<Type> Include
        {
            get {return _include;}
            set {_include= value;}
        }

        public List<Type> Exclude
        {
            get {return _exclude;}
            set {_exclude = value;}
        }

        public bool HasEntity(int entID)
        {
            return Entities.Contains(entID);
        }

        public bool EntityPassesFilter(int entID)
        {
            Dictionary<Type, Component> entMask = World.GetAllComponentsOfEntity(entID);

            foreach (Type t in Include)
            {
                if (!entMask.ContainsKey(t))
                {
                    return false;
                }
            }

            foreach (Type t in Exclude)
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