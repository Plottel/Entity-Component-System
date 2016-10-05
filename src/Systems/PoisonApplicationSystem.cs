using System;
using System.Collections.Generic;

namespace MyGame
{
    public class PoisonApplicationSystem : System
    {

        public PoisonApplicationSystem (World world) : base(new List<Type> {typeof(CApplyPoison), typeof(CPoison), typeof(CPosition)}, new List<Type>{}, world)
        {
        }

        private bool StillActive(CPoison poisonComp)
        {
            return !(World.GameTime - poisonComp.TimeApplied >= poisonComp.Duration);
        }

        public override void Process()
        {
            CPoison poisonComp;
            CPosition poisonPos;
            CPosition entPos;

            List<int> entsToPoison = World.GetAllEntitiesWithTag(typeof(CAI));

            //For each Poison Applicator
            for (int i = 0; i < Entities.Count; i++)
            {
                poisonComp = World.GetComponentOfEntity(Entities[i], typeof(CPoison)) as CPoison;
                poisonPos = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;

                if (StillActive(poisonComp))
                {
                    //For each entity which can be poisoned
                    foreach (int toPoison in entsToPoison)
                    {
                        entPos = World.GetComponentOfEntity(toPoison, typeof(CPosition)) as CPosition;

                        //If entity is inside a poison zone
                        if (CollisionSystem.AreColliding(poisonPos, entPos))
                        {
                            //Don't add multiple poison components (entity may be in 2 zones)
                            if (!World.EntityHasComponent(toPoison, typeof(CPoison)))
                            {
                                World.AddComponentToEntity(toPoison, new CPoison(poisonComp.Strength, poisonComp.Duration, World.GameTime));
                            }
                            else
                            {   //Refresh poison duration is inside poison zone
                                CPoison entPoisonComp = World.GetComponentOfEntity(toPoison, typeof(CPoison)) as CPoison;
                                entPoisonComp.TimeApplied = World.GameTime;
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