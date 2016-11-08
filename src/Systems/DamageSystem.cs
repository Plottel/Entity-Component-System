using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for applying damage to Entities and determining
    /// whether or not they are out of health. Attacks are registered with thie System and 
    /// damage applied. If the Entity is out of health, it is removed from the World.
    /// </summary>
    public class DamageSystem : System
    {
        /// <summary>
        /// Represents the Attacks which have been registered with this System for the current frame.
        /// Maps the attack target's Entity ID to a list of damage values to be applied.
        /// </summary>
        private Dictionary<ulong, List<int>> _attacks = new Dictionary<ulong, List<int>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.HealthSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public DamageSystem(World world) : base (new List<Type> {typeof(CHealth)}, new List<Type> {}, world)
        {
        }

        /// <summary>
        /// Adds the Entity to the Attacks dictionary so attacks against it are checked.
        /// </summary>
        /// <param name="entID">The Entity to be added.</param>
        public override void Add (ulong entID)
        {
            _attacks.Add(entID, new List<int>());
            base.Add (entID);
        }

        /// <summary>
        /// Removes the Entity from the Attacks dictionary so attacks against it are no longer checked.
        /// </summary>
        /// <param name="entID">The Entity to be removed.</param>
        public override void Remove (ulong entID)
        {
            if (HasEntity(entID))
                _attacks.Remove(entID);

            base.Remove(entID);
        }

        /// <summary>
        /// Processes all Attacks against each Entity. After each attack, the Entity's health is checked.
        /// If the Entity is out of health, it is removed from the World.
        /// </summary>
        public override void Process()
        {
            CHealth health;

            /// <summary>
            /// For each Entity in the System.
            /// </summary>
            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                health = World.GetComponent<CHealth>(Entities[i]);

                /// <summary>
                /// For each attack registered against the Entity.
                /// </summary>
                foreach (int damage in _attacks[Entities[i]])
                {
                    health.Damage += damage;

                    if (health.OutOfHealth)
                    {
                        World.RemoveEntity(Entities[i]);
                        break; //Stop when the Entity is out of health to prevent overkill.
                    }
                }


            }

            /// <summary>
            /// Remove all Attacks ready for the next frame.
            /// </summary>
            for (int i = 0; i < Entities.Count; i++)
            {
                _attacks[Entities[i]].Clear();
            }
        }

        /// <summary>
        /// Checks if the attack target is in the System.
        /// If it is, a new Attack is registered using the passed in Damage Component.
        /// </summary>
        /// <param name="attackTarget">The Attack target.</param>
        /// <param name="damage">The amount of damage to inflict.</param>
        public void RegisterAttack(ulong attackTarget, int damage)
        {
            if (HasEntity(attackTarget))
                _attacks[attackTarget].Add(damage);
        }
    }
}