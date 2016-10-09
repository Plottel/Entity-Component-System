using System;
using System.Collections.Generic;

namespace MyGame
{
    public class PoisonZoneCollisionHandlerSystem: System
    {

        public PoisonZoneCollisionHandlerSystem(World world) : base(new List<Type> {typeof(CAppliesDebuff), typeof(CPoison), typeof(CCollision)}, new List<Type>{}, world)
        {
        }

        private void RemoveDeadPoisonZones(List<int> toRemove)
        {
            foreach (int ent in toRemove)
            {
                World.RemoveEntity(ent);
            }
        }

        public override void Process()
        {
            CPoison poisonEffect;
            CPoison targetPoisonEffect;
            CCollision collision;

            List<int> deadPoisonZones = new List<int>();
            
            //For each poison zone
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
                    foreach (int target in collision.CollidedWith)
                    {
                        //If already poisoned, don't add another Poison component just refresh duration
                        if (!World.EntityHasComponent(target, typeof(CPoison)))
                        {
                            World.AddComponentToEntity(target, new CPoison(poisonEffect.Strength, poisonEffect.Duration, World.GameTime));
                        }
                        else
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