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
        private readonly Dictionary<ulong, Dictionary<Type, Component>> _entityComponents;

        /// <summary>
        /// The master list of all Systems the World knows about.
        /// </summary>
        private readonly List<System> _systems;

        /// <summary>
        /// Represents the next unique ID to be given to an Entity. 
        /// </summary>
        private ulong _nextID;

        /// <summary>
        /// The game time. This is the only timer in the program.
        /// All time-based code will utilise this timer.
        /// </summary>
        private static Timer _gameTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.World"/> class.
        /// Object will have no functionality until it is given at least one System or Entity.
        /// </summary>
        public World ()
        {
            _entityComponents = new Dictionary<ulong, Dictionary<Type, Component>>();
            _systems = new List<System>();
            _nextID = 1;
            _gameTime = new Timer();

            /// <summary>
            /// The game time commences as soon as the World is created.
            /// </summary>
            SwinGame.StartTimer(_gameTime);
        }

        /// <summary>
        /// Gets the current ticks of the Game Time Timer.
        /// </summary>
        public static uint GameTime
        {
            get {return SwinGame.TimerTicks(_gameTime);}
        }

        /// <summary>
        /// Gets the next unique Entity ID.
        /// </summary>
        /// <value>The next unique Entity ID.</value>
        public ulong NextEntityID
        {
            get {return _nextID++;}
        }

        /// <summary>
        /// Tells each System in the World to call their Process method.
        /// This method is called each frame and is essentially the Game Loop.
        /// </summary>
        public void Process()
        {
            foreach (System s in _systems)
                s.Process();
        }

        /// <summary>
        /// Adds the passed in Entity to the World and associates it with the passed in List of Components
        /// </summary>
        /// <param name="entID">The Entity to be added.</param>
        /// <param name="components">The List of Components to be associated with the Entity.</param>
        public void AddEntity(ulong entID, List<Component> components)
        {
            _entityComponents.Add(entID, new Dictionary<Type, Component>());

            //Add Entity's components to its entry in the Dictionary
            foreach (Component c in components)
                _entityComponents[entID].Add(c.GetType(), c);

            foreach (System s in _systems)
            {
                //If Entity meets the requirements of the System
                if (s.EntityPassesFilter(entID))
                    s.Add(entID); 
            }
        }

        /// <summary>
        /// Removes the passed in Entity and all its Components from the World and all Systems.
        /// </summary>
        /// <param name="entID">The Entity to be removed.</param>
        public void RemoveEntity(ulong entID)
        {
            foreach (System s in _systems)
            {
                s.Remove(entID);
            }

            _entityComponents.Remove(entID);
        }

        /// <summary>
        /// Returns a passed in Component Type belonging to the passed in Entity.
        /// </summary>
        /// <returns>The Component of the passed in Type for the passed in Entity.</returns>
        /// <param name="entID">The Entity to fetch the Component for.</param>
        /// <typeparam name="T">The Component Type to be fetched.</typeparam>
        public T GetComponent<T>(ulong entID) where T : Component
        {
            return (T)_entityComponents[entID][typeof(T)];
        }

        /// <summary>
        /// Returns a Dictionary containing all Components belonging to a specified Entity.
        /// The Dictionary maps Component Types to the actual Component. 
        /// </summary>
        /// <returns>The Dictionary mapping all Component Types to Components for the specified Entity.</returns>
        /// <param name="entID">The Entity to get all Components of.</param>
        public Dictionary<Type, Component> GetAllComponentsOfEntity(ulong entID)
        {   
            return _entityComponents[entID];
        }

        /// <summary>
        /// Adds a passed in Component to a passed in Entity. Re-evaluates if the Entity meets the requirements 
        /// for each System and tells each System to add / remove the Entity accordingly.
        /// </summary>
        /// <param name="entID">The Entity to add the Component to.</param>
        /// <param name="c">The Component to add.</param>
        public void AddComponent(ulong entID, Component c)
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
        public void RemoveComponent<T>(ulong entID) where T : Component
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
        /// Specifies whether or not an Entity has a passed in Component Type.
        /// </summary>
        /// <returns><c>true</c> if the Entity has the Component Type, <c>false</c> otherwise.</returns>
        /// <param name="entID">The Entity to check.</param>
        /// <param name="t">The Component Type to check.</param>
        public bool EntityHasComponent(ulong entID, Type t)
        {
            return _entityComponents[entID].ContainsKey(t);
        }

        /// <summary>
        /// Specifies whether or not the World has a passed in Entity.
        /// This is primarily used to determine if an Entity is still alive.
        /// </summary>
        /// <returns><c>true</c> if the World has the Entity, <c>false</c> otherwise.</returns>
        /// <param name="entID">The Entity to check.</param>
        public bool HasEntity(ulong entID)
        {
            return _entityComponents.ContainsKey(entID);
        }

        /// <summary>
        /// Returns a System from the World of a specified Type. This is used by other Systems to quickly 
        /// fetch data that is already grouped, rather than asking the World to fetch it from the master list.
        /// </summary>
        /// <returns>The System of the passed in Type.</returns>
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
    }
}