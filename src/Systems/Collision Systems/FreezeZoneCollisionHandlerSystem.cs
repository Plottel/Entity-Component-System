using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class FreezeZoneCollisionHandlerSystem : System
    {
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
            CFrozen freezeEffect;
            CFrozen targetFreezeEffect;

            List<int> deadFreezeEffects = new List<int>();

            //For each Freeze Zone
            for (int i = 0; i < Entities.Count; i++)
            {
                freezeEffect = World.GetComponentOfEntity(Entities[i], typeof(CFrozen)) as CFrozen;
                collision = World.GetComponentOfEntity(Entities[i], typeof(CCollision)) as CCollision;

                foreach (int target in collision.CollidedWith)
                {
                    //If already frozen, don't add another Frozen component just refresh duration
                    if (!World.EntityHasComponent(target, typeof(CFrozen)))
                    {
                        World.AddComponentToEntity(target, new CFrozen(freezeEffect.Duration, World.GameTime));
                    }
                    else
                    {
                        targetFreezeEffect = World.GetComponentOfEntity(target, typeof(CFrozen)) as CFrozen;
                        targetFreezeEffect.TimeApplied = World.GameTime;
                    }
                }
                deadFreezeEffects.Add(Entities[i]);
            }
            RemoveDeadFreezeEffects(deadFreezeEffects);
        }
    }
}