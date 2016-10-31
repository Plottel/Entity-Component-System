using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for handling collisions with Freeze Zones. Each Entity
    /// this System operates on is a Freeze Zone. It checks each Entity each Freeze Zone has collided
    /// with and applies Frozen and GotStatusEffect Components to them. This System is also responsible
    /// for removing expired Freeze Zones.
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

        private void RemoveDeadFreezeEffects(List<int> toRemove)
        {
            foreach (int ent in toRemove)
            {
                World.RemoveEntity(ent);
            }
        }

        public override void Process()
        {
            CCollision collision;
            CFrozen freezeZoneFrozen;
            CFrozen collidedFrozen;

            List<int> deadFreezeEffects = new List<int>();

            //For each Freeze Zone
            for (int i = 0; i < Entities.Count; i++)
            {
                freezeZoneFrozen = World.GetComponent<CFrozen>(Entities[i]);
                collision = World.GetComponent<CCollision>(Entities[i]);

                foreach (int target in collision.CollidedWith)
                {
                    //If already frozen, don't add another Frozen component just refresh duration
                    if (!World.EntityHasComponent(target, typeof(CFrozen)))
                    {
                        //Don't freeze projectiles
                        if (!World.EntityHasComponent(target, typeof(CProjectile)))
                            World.AddComponent(target, new CFrozen(freezeZoneFrozen.Duration, World.GameTime));

                        if (!World.EntityHasComponent(target, typeof(CGotStatusEffect)))
                            World.AddComponent(target, new CGotStatusEffect());
                    }
                    else
                    {
                        collidedFrozen = World.GetComponent<CFrozen>(target);
                        collidedFrozen.TimeApplied = World.GameTime;
                    }
                }
                deadFreezeEffects.Add(Entities[i]);
            }
            RemoveDeadFreezeEffects(deadFreezeEffects);
        }
    }
}