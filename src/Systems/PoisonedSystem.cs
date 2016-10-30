using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the System that handles Entities with a Poison Debuff. This System operates on a tick interval
    /// and will apply Poison Damage to each poisoned Entity each tick. It will also check if the Entity's poison
    /// component has expired. If it has, it will be removed from the Entity.
    /// </summary>
    public class PoisonedSystem : System
    {
        /// <summary>
        /// How often the System will operate.
        /// </summary>
        private uint _tickInterval;

        /// <summary>
        /// The last time the System operated. This is used to determine the next System tick.
        /// </summary>
        private uint _lastTick;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.PoisonedSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public PoisonedSystem (World world) : base(new List<Type> {typeof(CPoison), typeof(CHealth)}, new List<Type> {}, world)
        {
            _tickInterval = 2000;
        }

        /// <summary>
        /// Loops through each Poisoned Entity and applies poison damage. If the poison has expired, then
        /// the Poison Component is removed from the Entity.
        /// </summary>
        public override void Process()
        {
            if (World.GameTime - _lastTick >= _tickInterval)
            {
                _lastTick = World.GameTime;

                CPoison entPoison;
                CHealth entHealth;

                for (int i = 0; i < Entities.Count; i++)
                {
                    entPoison = World.GetComponent<CPoison>(Entities[i]);

                    if (!Utils.EffectHasEnded(World.GameTime, entPoison.TimeApplied, entPoison.Duration))
                    {
                        entHealth = World.GetComponent<CHealth>(Entities[i]);
                        entHealth.Damage += entPoison.Strength;
                    }
                    else
                    {
                        World.RemoveComponent<CPoison>(Entities[i]);
                    }
                }
            }
        }
    }
}