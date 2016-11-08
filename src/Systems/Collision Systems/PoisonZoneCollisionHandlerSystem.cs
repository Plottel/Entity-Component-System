using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the class that handles collisions between Enemy team Entities and Player team
    /// Poison Zones. The Entities stored in this System are Poison Zones with a Collision Component.
    /// The entities within the Collision Components are Enemy team Entities. This System applies Poison
    /// Components and GotStatusEffect Components to Enemy Entities.
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
        /// Checks each Entity colliding with each Poison Zone. If the Entity does not have
        /// a Poison Component, then a Poison Component and a GotStatusEffect Component are added.
        /// Otherwise, the Entity's Poison Component duration is refreshed.
        /// </summary>
        public override void Process()
        {
            CPoison poisonEffect;
            CPoison targetPoisonEffect;
            CCollision collision;
            
            /// <summary>
            /// This loop represents each Poison Zone.
            /// </summary>
            for (int i = 0; i < Entities.Count; i++)
            {
                poisonEffect = World.GetComponent<CPoison>(Entities[i]);
                collision = World.GetComponent<CCollision>(Entities[i]);

                /// <summary>
                /// This loop represents each Entity colliding with the Poison Zone.
                /// </summary>
                foreach (ulong target in collision.CollidedWith)
                {
                    if (!World.EntityHasComponent(target, typeof(CPoison)))
                    {
                        World.AddComponent(target, new CPoison(poisonEffect.Strength, poisonEffect.Duration, World.GameTime));

                        if (!World.EntityHasComponent(target, typeof(CGotStatusEffect)))
                        {
                            World.AddComponent(target, new CGotStatusEffect(typeof(CPoison)));
                        }
                        else
                        {
                            CGotStatusEffect statusEffects = World.GetComponent<CGotStatusEffect>(target);
                            statusEffects.AddEffect(typeof(CPoison));
                        }
                            
                    }
                    else //If entity already has a Poison Component, refresh its duration.
                    {
                        targetPoisonEffect = World.GetComponent<CPoison>(target);
                        targetPoisonEffect.TimeApplied = World.GameTime;
                    }
                }
            }
        }
    }
}