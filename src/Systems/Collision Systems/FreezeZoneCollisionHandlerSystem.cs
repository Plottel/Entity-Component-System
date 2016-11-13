using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for handling collisions with Freeze Zones. Each Entity
    /// this System operates on is a Freeze Zone. It checks each Entity each Freeze Zone has collided
    /// with and applies Frozen and GotStatusEffect Components to them. Once its collisions have been
    /// processed, each Freeze Zone is removed from the World.
    /// </summary>
    public class FreezeZoneCollisionHandlerSystem : System
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.FreezeZoneCollisionHandlerSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public FreezeZoneCollisionHandlerSystem (World world) : base(new List<Type> {typeof(CCollision), typeof(CFrozen), typeof(CAppliesDebuff)}, new List<Type> {}, world)
        {
        }

        public override void Process()
        {
            CCollision collision;
            CFrozen freezeZoneFrozen;
            CFrozen collidedFrozen;

            List<int> deadFreezeEffects = new List<int>();

            /// <summary>
            /// This loop represents each Freeze Zone.
            /// Backwards loop to allow Entities to be removed while looping.
            /// </summary>
            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                freezeZoneFrozen = World.GetComponent<CFrozen>(Entities[i]);
                collision = World.GetComponent<CCollision>(Entities[i]);

                /// <summary>
                /// This loop represents each Entity colliding with the Freeze Zone.
                /// </summary>
                for (int j = 0; j < collision.Count; j++)
                {
                    //If not already Frozen, add a Frozen Component.
                    if (!World.EntityHasComponent(collision[j], typeof(CFrozen)))
                    {
                        //Don't freeze projectiles
                        if (!World.EntityHasComponent(collision[j], typeof(CProjectile)))
                            World.AddComponent(collision[j], new CFrozen(freezeZoneFrozen.Duration, World.GameTime));

                        if (!World.EntityHasComponent(collision[j], typeof(CGotStatusEffect)))
                        {
                            World.AddComponent(collision[j], new CGotStatusEffect(typeof(CFrozen))); 
                        }
                        else
                        {
                            CGotStatusEffect statusEffects = World.GetComponent<CGotStatusEffect>(collision[j]);
                            statusEffects.AddEffect(typeof(CFrozen));
                        }
                            
                    }
                    else //If already Frozen, refresh the duration.
                    {
                        collidedFrozen = World.GetComponent<CFrozen>(collision[i]);
                        collidedFrozen.TimeApplied = World.GameTime;
                    }
                }

                /// <summary>
                /// Freeze Zone only lasts for one frame to apply Frozen Components.
                /// Each Freeze Zone is removed from the World once its collisions are processed.
                /// </summary>
                World.RemoveEntity(Entities[i]);
            }
        }
    }
}