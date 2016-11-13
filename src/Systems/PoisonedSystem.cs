using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the System that handles Entities with a Poison Debuff. This System operates on a tick interval
    /// and will register an Attack with the Damage System to each poisoned Entity each tick. It will also check if the Entity's poison
    /// component has expired. If it has, it will be removed from the Entity.
    /// </summary>
    public class PoisonedSystem : System
    {
        /// <summary>
        /// How often the System will operate.
        /// </summary>
        private int _tickInterval = 250;

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
        }

        /// <summary>
        /// Loops through each Poisoned Entity and applies poison damage. If the poison has expired, then
        /// the Poison Component is removed from the Entity.
        /// </summary>
        public override void Process()
        {
            /// <summary>
            /// The System where Attacks are registered.
            /// </summary>
            DamageSystem damageSystem = World.GetSystem<DamageSystem>();

            if (ReadyToApplyPoison(_lastTick, _tickInterval))
            {
                _lastTick = World.GameTime;

                CPoison poison;

                for (int i = 0; i < Entities.Count; i++)
                {
                    poison = World.GetComponent<CPoison>(Entities[i]);

                    if (!Utils.DurationReached(poison.TimeApplied, poison.Duration))
                        damageSystem.RegisterAttack(Entities[i], poison.Strength);
                    else
                        World.RemoveComponent<CPoison>(Entities[i]);
                }
            }
        }

        /// <summary>
        /// Specifies whether or not the System is ready to apply poison. This is calculated
        /// based on the Systen's Tick Interval and the last time poison was applied.
        /// </summary>
        /// <returns><c>true</c>, if the System is ready to apply poison, <c>false</c> otherwise.</returns>
        /// <param name="lastTickTime">Time the System last applied poison.</param>
        /// <param name="tickInterval">How often the System applies poison.</param>
        public bool ReadyToApplyPoison(uint lastTickTime, int tickInterval)
        {
            return Utils.DurationReached(lastTickTime, tickInterval);
        }
    }
}