using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the core functionality of all Systems. Defines methods for 
    /// adding / removing Entities and checking if an Entity meets the requirements of the System.
    /// </summary>
    public abstract class System
    {
        /// <summary>
        /// The Entities the System will operate on. This cannot be overwritten to a new List.
        /// </summary>
        private readonly List<ulong> _entities;

        /// <summary>
        /// The Component Types the System will operate on.
        /// An Entity must have ALL of these Components to be added to the System.
        /// </summary>
        private List<Type> _include;

        /// <summary>
        /// The Component Types the System will NOT operate on.
        /// An Entity must have NONE of these Components to be added to the System.
        /// </summary>
        private List<Type> _exclude;

        /// <summary>
        /// The World the System belongs to. This defines which Entities the System can have.
        /// </summary>
        private World _world;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.System"/> class.
        /// </summary>
        /// <param name="include">The Component Types the System will operate on.</param>
        /// <param name="exclude">The Component Types the System will NOT operate on.</param>
        /// <param name="world">The World the System belongs to.</param>
        protected System(List<Type> include, List<Type> exclude, World world)
        {
            _entities = new List<ulong>();
            _include = include;
            _exclude = exclude;
            _world = world;
        }

        /// <summary>
        /// Gets or sets the World the System belongs to.
        /// </summary>
        /// <value>The World.</value>
        public World World
        {
            get {return _world;}
            set {_world = value;}
        }

        /// <summary>
        /// Gets the List of Entities the System knows about. This cannot be overwritten to a new List.
        /// </summary>
        /// <value>The Entities.</value>
        public List<ulong> Entities
        {
            get {return _entities;}
        }

        /// <summary>
        /// Gets or sets the List of Component Types the System will operate on.
        /// </summary>
        /// <value>The Component Types.</value>
        public List<Type> Include
        {
            get {return _include;}
            set {_include= value;}
        }

        /// <summary>
        /// Gets or sets the List of Component Types the System will NOT operate on.
        /// </summary>
        /// <value>The Component Types.</value>
        public List<Type> Exclude
        {
            get {return _exclude;}
            set {_exclude = value;}
        }

        /// <summary>
        /// Represents the core functionality of the System. The World will call this
        /// method on each System it knows about. Child Systems will implement their own 
        /// definition of Process.
        /// </summary>
        public abstract void Process();

        /// <summary>
        /// Adds the passed in Entity to the System's list of Entities.
        /// </summary>
        /// <param name="entID">The Entity to be added.</param>
        public virtual void Add(ulong entID)
        {
            Entities.Add(entID);
        }

        /// <summary>
        /// Removes the passed in Entity from the System's list of Entities.
        /// Child Systems can define their own Remove methods for specialised functionality. 
        /// </summary>
        /// <param name="entID">The Entity to be removed.</param>
        public virtual void Remove(ulong entID)
        {
            Entities.Remove(entID);
        }

        /// <summary>
        /// Determines whether or not the passed in Entity meets the requirements of the System.
        /// This checks against both the Include and Exclude Type Lists.
        /// </summary>
        /// <returns><c>true</c> if the Entity passes the filter, <c>false</c> otherwise.</returns>
        /// <param name="entID">The Entity to check.</param>
        public bool EntityPassesFilter(ulong entID)
        {
            //Check Entity has all components the System will operate on.
            foreach (Type t in Include)
            {
                if (!World.EntityHasComponent(entID, t))
                    return false;
            }

            //Check Entity does NOT have any components the System will NOT operate on.
            foreach (Type t in Exclude)
            {
                if (World.EntityHasComponent(entID, t))
                    return false;
            }

            //The Entity meets the requirements and will be added to the System.
            return true;
        }

        /// <summary>
        /// Specifies whether or not the System contains the passed in Entity.
        /// </summary>
        /// <returns><c>true</c> if the System has the Entity, <c>false</c> otherwise.</returns>
        /// <param name="entID">The Entity to check.</param>
        public bool HasEntity(ulong entID)
        {
            return Entities.Contains(entID);
        }
    }
}