using System;
using System.Collections.Generic;
using System.Linq;
using SwinGameSDK;

namespace MyGame
{
    public class World
    {
        private Dictionary<int, Dictionary<Type, Component>> _entityComponents;
        private List<System> _systems;
        private int _nextID;
        private Timer _gameTime;

        public World ()
        {
            _entityComponents = new Dictionary<int, Dictionary<Type, Component>>();
            _systems = new List<System>();
            _nextID = 0;

            //Pass world to Entity factory so it can create 
            EntityFactory.World = this;

            _gameTime = new Timer();
            SwinGame.StartTimer(_gameTime);
        }

        public uint GameTime
        {
            get
            {
                return SwinGame.TimerTicks(_gameTime);
            }
        }

        public List<int> GetAllEntitiesWithTag(Type t)
        {
            List<int> result = new List<int>();

            foreach (int entID in _entityComponents.Keys)
            {
                if (EntityHasComponent(entID, t))
                {
                    result.Add(entID);
                }
            }
            return result;
        }

        public void Process()
        {
            foreach (System s in _systems)
            {
                s.Process();
            }
        }

        public bool EntityHasComponent(int entID, Type t)
        {
            return _entityComponents[entID].ContainsKey(t);
        }

        //Add passed in component to the World Dictionary
        //Updates Entity mask and adds to each System if not already within System
        //and if meets System component requirements
        public void AddComponentToEntity(int entID, Component c)
        {
            //Add new component to entity
            _entityComponents[entID].Add(c.GetType(), c);

            foreach (System s in _systems)
            {
                //If System does not contain Entity and masks now match
                if (!s.HasEntity(entID) && s.EntityPassesFilter(entID))
                {
                    s.Add(entID);
                }

                //If system contains Entity and mask no longer matches
                if (s.HasEntity(entID) && !s.EntityPassesFilter(entID))
                {
                    s.Remove(entID);
                }
            }
        }


        //Remove passed in component from the World Dictionary
        //Updates Entity mask and removes from each System if already within System
        //and if no longer meets System component requirements
        public void RemoveComponentFromEntity(int entID, Type t)
        {
            _entityComponents[entID].Remove(t);

            foreach (System s in _systems)
            {
                //If System contains Entity and masks no longer match
                if (s.HasEntity(entID) && !s.EntityPassesFilter(entID))
                {
                    s.Remove(entID);
                }

                //If System does not contain Entity and masks now match
                if (!s.HasEntity(entID) && s.EntityPassesFilter(entID))
                {
                    s.Add(entID);
                }
            }
        }

        //Gets a specified component associated with the specified Entity ID
        public Component GetComponentOfEntity(int entID, Type t)
        {
            return _entityComponents[entID][t];
        }

        //Return a list of components associated with the specified Entity ID
        public Dictionary<Type, Component> GetAllComponentsOfEntity(int entID)
        {
            return _entityComponents[entID];
        }

        //Fetches a System of a given type - used for Tag Manager
        public System GetSystem(Type t)
        {
            foreach (System s in _systems)
            {
                if (s.GetType() == t)
                {
                    return s;
                }
            }
            return null;
        }

        public void AddSystem(System s)
        {
            _systems.Add(s);
        }

        //Gets the next unique ID and creates an Entity with this ID
        public Entity CreateEntity()
        {
            //Recycling not important

            Entity result = new Entity(_nextID);
            _nextID++;
            return result;
        }

        public void AddEntity(Entity e, List<Component> components)
        {
            //Add Entity to lookup Dictionaries
            _entityComponents.Add(e.ID, new Dictionary<Type, Component>());

            //Add Entity's components to its entry in the Dictionary
            foreach (Component c in components)
            {
                _entityComponents[e.ID].Add(c.GetType(), c);
            }

            foreach (System s in _systems)
            {
                //If entity has required components for system
                if (s.EntityPassesFilter(e.ID))
                {
                    s.Add(e.ID); 
                }
            }
        }

        public void RemoveEntity(int e)
        {
            //Remove Entity from lookup Dictionaries
            _entityComponents.Remove(e);

            foreach (System s in _systems)
            {
                s.Remove(e);
            }
        }
    }
}