using System;
using System.Collections.Generic;

namespace MyGame
{
    public class ApplyPoisonSystem : System
    {

        public ApplyPoisonSystem (World world) : base(new List<Type> {typeof(CAppliesDebuff), typeof(CPoison), typeof(CPosition)}, new List<Type>{}, world)
        {
        }

        public override void Process()
        {
            CPoison debufferPoison;
            CPosition debufferPos;
            CPosition toPoisonPos;

            List<int> entsToPoison = World.GetAllEntitiesWithTag(typeof(CAI));

            //For each Poison Applicator
            for (int i = 0; i < Entities.Count; i++)
            {
                debufferPoison = World.GetComponentOfEntity(Entities[i], typeof(CPoison)) as CPoison;
                debufferPos = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;

                if (!Utils.EffectHasEnded(World.GameTime, debufferPoison.TimeApplied, debufferPoison.Duration))
                {
                    //For each entity which can be poisoned
                    foreach (int toPoison in entsToPoison)
                    {
                        toPoisonPos = World.GetComponentOfEntity(toPoison, typeof(CPosition)) as CPosition;

                        //If entity is inside a poison zone
                        if (Utils.AreColliding(toPoisonPos, debufferPos))
                        {
                            //Don't add multiple poison components (entity may be in 2 zones)
                            if (!World.EntityHasComponent(toPoison, typeof(CPoison)))
                            {
                                World.AddComponentToEntity(toPoison, new CPoison(debufferPoison.Strength, debufferPoison.Duration, World.GameTime));
                            }
                            else
                            {   //Refresh poison duration is inside poison zone
                                CPoison toPoisonPoison = World.GetComponentOfEntity(toPoison, typeof(CPoison)) as CPoison;
                                toPoisonPoison.TimeApplied = World.GameTime;
                            }
                        }
                    }
                }
                else //Applicator is no longer active - remove entity
                {
                    World.RemoveEntity(Entities[i]);
                }
            }
        }
    }
}