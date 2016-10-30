using System;
using System.Collections.Generic;
using System.Linq;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the overall controller of the program. Co-ordinates all Entities and Systems 
    /// and provides various helper functions. A World functions based on the Systems and Entities
    /// it knows about. There can be multiple Worlds to, for example, handle multiple game states.
    /// </summary>   
    public class World
    {
        /// <summary>
        /// The master list of Entities the World knows about and all their Components. Maps an Entity ID
        /// to another Dictionary mapping the Component Type to the actual Component.
        /// </summary>
        private Dictionary<int, Dictionary<Type, Component>> _entityComponents;

        /// <summary>
        /// The master list of all Systems the World knows about.
        /// </summary>
        private List<System> _systems;

        /// <summary>
        /// Represents the Entity IDs from dead Entities which can be recycled.
        /// </summary>
        private List<int> _recycledIDs;

        /// <summary>
        /// Represents the next unique ID to be given to an Entity. If there are no
        /// recycled IDs available, this ID will be given to the next Entity.
        /// </summary>
        private int _nextID;

        /// <summary>
        /// The game time. This is the only timer in the program.
        /// All time-based code will utilise this timer.
        /// </summary>
        private Timer _gameTime;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.World"/> class.
        /// Object will have no functionality until it is given at least one System or Entity.
        /// </summary>
        public World ()
        {
            _entityComponents = new Dictionary<int, Dictionary<Type, Component>>();
            _systems = new List<System>();
            _nextID = 1;
            _recycledIDs = new List<int>();
            _gameTime = new Timer();

            /// <summary>
            /// The game time commences as soon as the World is created.
            /// </summary>
            SwinGame.StartTimer(_gameTime);
        }

        /// <summary>
        /// Gets the current ticks of the Game Time.
        /// </summary>
        public uint GameTime
        {
            get
            {
                return SwinGame.TimerTicks(_gameTime);
            }
        }

        /// <summary>
        /// Specifies whether or not two Entities are on the same team
        /// </summary>
        public bool EntitiesOnSameTeam(int entOne, int entTwo)
        {
            if (EntityHasComponent(entOne, typeof(CPlayerTeam)) && EntityHasComponent(entTwo, typeof(CPlayerTeam)))
                return true;

            if (EntityHasComponent(entOne, typeof(CEnemyTeam)) && EntityHasComponent(entTwo, typeof(CEnemyTeam)))
                return true;

            return false;
        }

        /// <summary>
        /// Returns a list of all Entities which have a passed in Component Type.
        /// </summary>
        /// <param name="t">The Component Type to check.</param>
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

        /// <summary>
        /// Tells each System in the World to call their Process method.
        /// This method is called each frame and is essentially the Game Loop.
        /// </summary>
        public void Process()
        {
            foreach (System s in _systems)
            {
                s.Process();
            }
        }

        /// <summary>
        /// Specifies whether or not an Entity has a passed in Component Type.
        /// </summary>
        /// <param name="entID">The Entity to check.</param>
        /// <param name="t">The Component Type to check.</param>
        public bool EntityHasComponent(int entID, Type t)
        {
            return _entityComponents[entID].ContainsKey(t);
        }

        /// <summary>
        /// Specifies whether or not the World has a passed in Entity.
        /// This is primarily used to determine if an Entity is still alive.
        /// </summary>
        /// <param name="entID">The Entity to check.</param>
        public bool HasEntity(int entID)
        {
            return _entityComponents.ContainsKey(entID);
        }

        /// <summary>
        /// Adds a passed in Component to a passed in Entity. Re-evaluates if the Entity meets the requirements 
        /// for each System and tells each System to add / remove the Entity accordingly.
        /// </summary>
        /// <param name="entID">The Entity to add the Component to.</param>
        /// <param name="c">The Component to add.</param>
        public void AddComponent(int entID, Component c)
        {
            _entityComponents[entID].Add(c.GetType(), c);

            foreach (System s in _systems)
            {
                //If System does not contain Entity and Entity now meets requirements.
                if (!s.HasEntity(entID) && s.EntityPassesFilter(entID))
                {
                    s.Add(entID);
                }

                //If system contains Entity and Entity no longer meets requirements.
                if (s.HasEntity(entID) && !s.EntityPassesFilter(entID))
                {
                    s.Remove(entID);
                }
            }
        }

        /// <summary>
        /// Removes a passed in Component Type from a passed in Entity. Re-evaluates if the Entity meets the requirements 
        /// for each System and tells each System to add / remove the Entity accordingly.
        /// </summary>
        /// <param name="entID">The Entity to remove the Component from.</param>
        /// <typeparam name="T">The Component Type to be removed.</typeparam>
        public void RemoveComponent<T>(int entID) where T : Component
        {
            _entityComponents[entID].Remove(typeof(T));

            foreach (System s in _systems)
            {
                //If System contains Entity and Entity no longer meets requirements
                if (s.HasEntity(entID) && !s.EntityPassesFilter(entID))
                {
                    s.Remove(entID);
                }

                //If System does not contain Entity and Entity now meets requirements
                if (!s.HasEntity(entID) && s.EntityPassesFilter(entID))
                {
                    s.Add(entID);
                }
            }
        }

        /// <summary>
        /// Returns a pssed in Component Type belonging to the passed in Entity.
        /// </summary>
        /// <param name="entID">The Entity to fetch the Component for.</param>
        /// <typeparam name="T">The Component Type to be fetched.</typeparam>
        public T GetComponent<T>(int entID) where T : Component
        {
            return (T)_entityComponents[entID][typeof(T)];
        }

        /// <summary>
        /// Returns a Dictionary containing all Components belonging to a specified Entity.
        /// The Dictionary maps Component Types to the actual Component. 
        /// </summary>
        /// <param name="entID">The Entity to get all Components of.</param>
        public Dictionary<Type, Component> GetAllComponentsOfEntity(int entID)
        {   
            return _entityComponents[entID];
        }

        /// <summary>
        /// Returns a System from the World of a specified Type. This is used by other Systems to quickly 
        /// fetch data that is already grouped, rather than asking the World to fetch it from the master list.
        /// </summary>
        /// <typeparam name="T">The System Type to fetch.</typeparam>
        public T GetSystem<T>() where T : System
        {
            Type t = typeof(T);

            foreach (System s in _systems)
            {
                if (s.GetType() == t)
                {
                    return s as T;
                }
            }
            return null;
        }

        /// <summary>
        /// Adds a passed in System to the World's list of Systems.
        /// </summary>
        /// <param name="s">The System to add.</param>
        public void AddSystem(System s)
        {
            _systems.Add(s);
        }

        /// <summary>
        /// Returns an Entity. If there are any IDs which can be recycled, the new
        /// Entity's ID will be the last recyclable ID. Otherwise, it will be the next unique ID.
        /// </summary>
        public Entity CreateEntity()
        {
            Entity newEntity;

            if (_recycledIDs.Any())
            {
                newEntity = new Entity(_recycledIDs[_recycledIDs.Count - 1]);
                _recycledIDs.RemoveAt(_recycledIDs.Count - 1);
            }
            else
            {
                newEntity = new Entity(_nextID);
                _nextID++;
            }

            return newEntity;
        }

        /// <summary>
        /// Adds the passed in Entity to the World and associates it with the passed in List of Components
        /// </summary>
        /// <param name="e">The Entity to be added.</param>
        /// <param name="components">The List of Components to be associated with the Entity.</param>
        public void AddEntity(Entity e, List<Component> components)
        {
            _entityComponents.Add(e.ID, new Dictionary<Type, Component>());

            //Add Entity's components to its entry in the Dictionary
            foreach (Component c in components)
            {
                _entityComponents[e.ID].Add(c.GetType(), c);
            }

            foreach (System s in _systems)
            {
                //If Entity meets the requirements of the System
                if (s.EntityPassesFilter(e.ID))
                {
                    s.Add(e.ID); 
                }
            }
        }

        /// <summary>
        /// Removes the passed in Entity and all its Components from the World and all Systems.
        /// </summary>
        /// <param name="e">The Entity to be removed.</param>
        public void RemoveEntity(int e)
        {
            foreach (System s in _systems)
            {
                s.Remove(e);
            }
            _entityComponents.Remove(e);
        }
    }
}