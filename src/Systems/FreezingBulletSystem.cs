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

            List<int> deadFreezingBullets = new List<int>();

            List<int> enemies = World.GetAllEntitiesWithTag(typeof(CAI));

            //For each Freezing Bullet
            for (int i = 0; i < Entities.Count; i++)
            {
                bulletProj = World.GetComponentOfEntity(Entities[i], typeof(CProjectile)) as CProjectile;
                bulletPos = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;
                
                if (ReachedTarget(bulletProj, bulletPos))
                {
                    EntityFactory.CreateAnimation(bulletPos.X - 50, 
                                                  bulletPos.Y - 50, 
                                                  SwinGame.CreateAnimation("Splash", SwinGame.AnimationScriptNamed("FreezingBulletSplashAnim")), 
                                                  SwinGame.BitmapNamed("FreezingBulletSplash"));

                    //For each Entity which can be frozen
                    foreach (int enemy in enemies)
                    {
                        enemyPos = World.GetComponentOfEntity(enemy, typeof(CPosition)) as CPosition;
                        CPosition AOE = new CPosition(bulletPos.X - 50, bulletPos.Y - 50, bulletPos.Width + 100, bulletPos.Width + 100);
                        if (Utils.AreColliding(AOE, enemyPos))
                        {
                            //Don't add multiple Freze components (entity may collide with 2 bullets)
                            if (!World.EntityHasComponent(enemy, typeof(CFrozen)))
                            {
                                World.AddComponentToEntity(enemy, new CFrozen(3000, World.GameTime));
                            }
                            else //Refresh duration on Frozen component
                            {
                                CFrozen enemyFrozen= World.GetComponentOfEntity(enemy, typeof(CFrozen)) as CFrozen;
                                enemyFrozen.TimeApplied = World.GameTime;
                            }
                        }
                    }
                    //Entity has reached its target and exploded, so kill it
                    deadFreezingBullets.Add(Entities[i]);
                }
            }
            RemoveDeadProjectiles(deadFreezingBullets);
        }
    }
}