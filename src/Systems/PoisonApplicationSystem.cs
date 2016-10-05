using System;
using System.Collections.Generic;

namespace MyGame
{
    public class PoisonApplicationSystem : System
    {
        public PoisonApplicationSystem (World world) : base((int)ComponentType.ApplyPoison | (int)ComponentType.Poison | (int)ComponentType.Position, world)
        {
        }

        private bool StillActive(PoisonComponent poisonComp)
        {
            return !(World.GameTime - poisonComp.TimeApplied >= poisonComp.Duration);
        }

        public override void Process()
        {
            PoisonComponent poisonComp;
            PositionComponent poisonPos;
            PositionComponent entPos;
            List<int> entsToPoison = World.GetAllEntitiesWithTag(typeof(AIComponent));

            //For each Poison Applicator
            for (int i = 0; i < Entities.Count; i++)
            {
                poisonComp = World.GetComponentOfEntity(Entities[i], typeof(PoisonComponent)) as PoisonComponent;
                poisonPos = World.GetComponentOfEntity(Entities[i], typeof(PositionComponent)) as PositionComponent;

                if (StillActive(poisonComp))
                {
                    //For each entity which can be poisoned
                    foreach (int toPoison in entsToPoison)
                    {
                        entPos = World.GetComponentOfEntity(toPoison, typeof(PositionComponent)) as PositionComponent;

                        //If entity is inside a poison zone
                        if (CollisionSystem.AreColliding(poisonPos, entPos))
                        {
                            //Don't add multiple poison components (entity may be in 2 zones)
                            if (!World.EntityHasComponent(toPoison, typeof(PoisonComponent)))
                            {
                                World.AddComponentToEntity(toPoison, new PoisonComponent(poisonComp.Strength, poisonComp.Duration, World.GameTime));
                            }
                            else
                            {   //Refresh poison duration is inside poison zone
                                PoisonComponent entPoisonComp = World.GetComponentOfEntity(toPoison, typeof(PoisonComponent)) as PoisonComponent;
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