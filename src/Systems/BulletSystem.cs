using System;
using System.Collections.Generic;

namespace MyGame
{
    public class BulletSystem : ProjectileSystem
    {
        public BulletSystem (World world) : base(world)
        {
            InclusionMask.Add(typeof(CDamage));
        }

        private void RemoveDeadBullets(List<int> toRemove)
        {
            foreach (int bullet in toRemove)
            {
                World.RemoveEntity(bullet);
            }
        }

        public override void Process()
        {
            CProjectile bulletProj;
            CPosition bulletPos;
            CDamage bulletDam;
            CPosition enemyPos;
            CHealth enemyHealth;
            List<int> deadBullets = new List<int>();

            //For each bullet
            for (int i = 0; i < Entities.Count; i++)
            {
                bulletProj = World.GetComponentOfEntity(Entities[i], typeof(CProjectile)) as CProjectile;
                bulletPos = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;

                /*              THIS WILL BE USEFUL WHEN MAKING BULLETS THAT SHOOT UNITS OTHER THAN THE CASTLE
                 * 
                 * 
                 * 
                 * 
                List<int> enemies = World.GetAllEntitiesWithTag(typeof(CAI)); //Get new list for each bullet - entities can be removed
                //For each potential collision target
                foreach (int e in enemies)
                {
                    enemyPos = World.GetComponentOfEntity(e, typeof(CPosition)) as CPosition;

                    if (CollisionSystem.AreColliding(bulletPos, enemyPos))
                    {
                        bulletDam = World.GetComponentOfEntity(Entities[i], typeof(CDamage)) as CDamage;
                        enemyHealth = World.GetComponentOfEntity(e, typeof(CHealth)) as CHealth;

                        enemyHealth.Damage += bulletDam.Damage; //Inflict bullet damage

                        deadBullets.Add(Entities[i]);
                        break;
                    }
                }*/

                if (ReachedTarget(bulletProj, bulletPos))
                {
                    deadBullets.Add(Entities[i]);
                }
            }
            RemoveDeadBullets(deadBullets);
        }
    }
}