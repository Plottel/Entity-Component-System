using System;
using System.Collections.Generic;

namespace MyGame
{
    /// <summary>
    /// Represents the class that handles collisions between enemy team Entities and Player team
    /// Poison Zones. The entities stored in this System are Poison Zones with a Collision Component.
    /// The entities within the Collision Components are Enemy team Entities. This System applies poison
    /// components to Enemy Entities and removes Poison Zones from the World if they have expired.
    /// </summary>
    public class PoisonZoneCollisionHandlerSystem: System
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.PoisonZoneCollisionHandlerSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public PoisonZoneCollisionHandlerSystem(World world) : base(new List<Type> {typeof(CAppliesDebuff), typeof(CPoison), typeof(CCollision)}, 
                                                                    new List<Type>{}, 
                                                                    world)
        {
        }

        /// <summary>
        /// Represents a list of Poison Zones which have expired and need to be removed from the World.
        /// They are stored in a separate list and removed at the end of the Process method in order to
        /// prevent issues which arise when modifying lists while looping over them.
        /// </summary>
        /// <param name="toRemove">The list of Poison Zones to remove from the world.</param>
        private void RemoveDeadPoisonZones(List<int> toRemove)
        {
            foreach (int ent in toRemove)
            {
                World.RemoveEntity(ent);
            }
        }

        /// <summary>
        /// Loops through each Poison Zone and checks if it has expired. If it has, it is added to the
        /// list of dead Poison Zones. If it hasn't, each Entity that is colliding with the Poison Zone
        /// is given a Poison Component. If the Entity already has a Poison Component, its duration is refreshed.
        /// </summary>
        public override void Process()
        {
            CPoison poisonEffect;
            CPoison targetPoisonEffect;
            CCollision collision;

            List<int> deadPoisonZones = new List<int>();
            
            /// <summary>
            /// This loop represents each Poison Zone.
            /// </summary>
            for (int i = 0; i < Entities.Count; i++)
            {
                poisonEffect = World.GetComponentOfEntity(Entities[i], typeof(CPoison)) as CPoison;
                collision = World.GetComponentOfEntity(Entities[i], typeof(CCollision)) as CCollision;

                if (Utils.EffectHasEnded(World.GameTime, poisonEffect.TimeApplied, poisonEffect.Duration))
                {
                    deadPoisonZones.Add(Entities[i]);
                }
                else
                {
                    /// <summary>
                    /// This loop represents each Entity colliding with the Poison Zone.
                    /// </summary>
                    foreach (int target in collision.CollidedWith)
                    {
                        if (!World.EntityHasComponent(target, typeof(CPoison)))
                        {
                            World.AddComponentToEntity(target, new CPoison(poisonEffect.Strength, poisonEffect.Duration, World.GameTime));
                        }
                        else //If entity already has a Poison Compoent, refresh its duration.
                        {
                            targetPoisonEffect = World.GetComponentOfEntity(target, typeof(CPoison)) as CPoison;
                            targetPoisonEffect.TimeApplied = World.GameTime;
                        }
                    }
                }
            }
            RemoveDeadPoisonZones(deadPoisonZones);
        }
    }
}