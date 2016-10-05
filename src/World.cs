using System;
using System.Collections.Generic;
using System.Linq;
using SwinGameSDK;

namespace MyGame
{
    public class World
    {
        private Dictionary<int, Dictionary<Type, Component>> _entityComponents;
        private Dictionary<int, int> _entityMasks;
        private Dictionary<Type, int> _componentMasks;
        private List<System> _systems;
        private int _nextID;
        private Timer _gameTime;

        public World ()
        {
            _entityComponents = new Dictionary<int, Dictionary<Type, Component>>();
            _entityMasks = new Dictionary<int, int>();
            _systems = new List<System>();
            _nextID = 0;

            //Setup mapping of component masks
            _componentMasks = new Dictionary<Type, int>();
            _componentMasks.Add(typeof(PositionComponent), (int)ComponentType.Position);
            _componentMasks.Add(typeof(RenderableComponent), (int)ComponentType.Renderable);
            _componentMasks.Add(typeof(VelocityComponent), (int)ComponentType.Velocity);
            _componentMasks.Add(typeof(PlayerComponent), (int)ComponentType.Player);
            _componentMasks.Add(typeof(HealthComponent), (int)ComponentType.Health);
            _componentMasks.Add(typeof(AIComponent), (int)ComponentType.AI);
            _componentMasks.Add(typeof(ProjectileComponent), (int)ComponentType.Projectile);
            _componentMasks.Add(typeof(GunComponent), (int)ComponentType.Gun);
            _componentMasks.Add(typeof(DamageComponent), (int)ComponentType.Damage);
            _componentMasks.Add(typeof(NotMovingComponent), (int)ComponentType.NotMoving);
            _componentMasks.Add(typeof(PoisonComponent), (int)ComponentType.Poison);
            _componentMasks.Add(typeof(ApplyPoisonComponent), (int)ComponentType.ApplyPoison);

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
            return (GetMaskForEntity(entID) & _componentMasks[t]) == _componentMasks[t];
        }

        public int GetMaskForComponentType(Type t)
        {
            return _componentMasks[t];
        }

        public int GetMaskForEntity(int entID)
        {
            return _entityMasks[entID];
        }

        //Add passed in component to the World Dictionary
        //Updates Entity mask and adds to each System if not already within System
        //and if meets System component requirements
        public void AddComponentToEntity(int entID, Component c)
        {
            _entityComponents[entID].Add(c.GetType(), c);
            _entityMasks[entID] = _entityMasks[entID] | GetMaskForComponentType(c.GetType());

            foreach (System s in _systems)
            {
                //If System does not contain Entity and masks now match
                if (!s.HasEntity(entID) && s.EntityHasRequiredComponents(_entityMasks[entID]))
                {
                    s.Add(entID);
                }

                //If system contains Entity and mask no longer matches
                if (s.HasEntity(entID) && !s.EntityHasRequiredComponents(_entityMasks[entID]))
                {
                    s.Remove(entID);
                }
            }
        }

        //Adding a component now makes the entity match the mask - Add to system
        //Removing a component now makes the entity match the mask - Remove from system

        //Remove passed in component from the World Dictionary
        //Updates Entity mask and removes from each System if already within System
        //and if no longer meets System component requirements
        public void RemoveComponentFromEntity(int entID, Type t)
        {
            _entityComponents[entID].Remove(t);
            _entityMasks[entID] = _entityMasks[entID] ^ GetMaskForComponentType(t);

            foreach (System s in _systems)
            {
                //If System contains Entity and masks no longer match
                if (s.HasEntity(entID) && !s.EntityHasRequiredComponents(_entityMasks[entID]))
                {
                    s.Remove(entID);
                }

                //If System does not contain Entity and masks now match
                if (!s.HasEntity(entID) && s.EntityHasRequiredComponents(_entityMasks[entID]))
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
        public List<Component> GetAllComponentsOfEntity(int entID)
        {
            return _entityComponents[entID].Values.ToList();
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
            _entityMasks.Add(e.ID, e.Mask);

            //Add Entity's components to its entry in the Dictionary
            foreach (Component c in components)
            {
                _entityComponents[e.ID].Add(c.GetType(), c);
            }

            foreach (System s in _systems)
            {
                //If entity has required components for system
                if (s.EntityHasRequiredComponents(e.Mask))
                {
                    s.Add(e.ID); 
                }
            }
        }

        public void RemoveEntity(int e)
        {
            //Remove Entity from lookup Dictionaries
            _entityComponents.Remove(e);
            _entityMasks.Remove(e);

            foreach (System s in _systems)
            {
                s.Remove(e);
            }
        }
    }
}