using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class FreezingBulletSystem : ProjectileSystem
    {

        public FreezingBulletSystem (World world) : base(world)
        {
            ExclusionMask.Add(typeof(CDamage));
        }

        public override void Process()
        {
            CProjectile bulletProj;
            CPosition bulletPos;
            CPosition enemyPos;

            List<int> enemies = World.GetAllEntitiesWithTag(typeof(CAI));

            //For each Freezing Bullet
            for (int i = 0; i < Entities.Count; i++)
            {
                bulletProj = World.GetComponentOfEntity(Entities[i], typeof(CProjectile)) as CProjectile;
                bulletPos = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;
                
                if (ReachedTarget(bulletProj, bulletPos))
                {
                    //For each Entity which can be frozen
                    foreach (int e in enemies)
                    {
                        enemyPos = World.GetComponentOfEntity(e, typeof(CPosition)) as CPosition;
                        CPosition AOE = new CPosition(bulletPos.X - 20, bulletPos.Y - 20, bulletPos.Width + 40, bulletPos.Width + 40);
                        if (CollisionSystem.AreColliding(AOE, enemyPos))
                        {
                            //Don't add multiple Freze components (entity may collide with 2 bullets)
                            if (!World.EntityHasComponent(e, typeof(CFrozen)))
                            {
                                World.AddComponentToEntity(e, new CFrozen(3000, World.GameTime));
                            }
                            else //Refresh duration on Frozen component
                            {
                                CFrozen enemyFrozenComp = World.GetComponentOfEntity(e, typeof(CFrozen)) as CFrozen;
                                enemyFrozenComp.TimeApplied = World.GameTime;
                            }
                        }
                    }

                    //Entity has reached its target and exploded, so kill it
                    World.RemoveEntity(Entities[i]);
                }
            }
        }
    }
}