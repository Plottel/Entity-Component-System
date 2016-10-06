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

        private List<int> RemoveBulletsFromEnemies(List<int> enemies)
        {
            List<int> result = new List<int>();
            foreach (int enemy in enemies)
            {
                if (!World.EntityHasComponent(enemy, typeof(CProjectile)))
                {
                    result.Add(enemy);
                }
            }
            return result;
        }

        public override void Process()
        {
            CProjectile bulletProj;
            CPosition bulletPos;
            CDamage bulletDam;
            CPosition enemyPos;
            CHealth enemyHealth;
            List<int> enemies = World.GetAllEntitiesWithTag(typeof(CTeam));
            enemies = RemoveBulletsFromEnemies(enemies);
            List<int> deadBullets = new List<int>();

            //For each bullet
            for (int i = 0; i < Entities.Count; i++)
            {
                bulletProj = World.GetComponentOfEntity(Entities[i], typeof(CProjectile)) as CProjectile;
                bulletPos = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;
                CTeam bulletTeamComp = World.GetComponentOfEntity(Entities[i], typeof(CTeam)) as CTeam;

                //For each potential collision target
                foreach (int enemy in enemies)
                {
                    enemyPos = World.GetComponentOfEntity(enemy, typeof(CPosition)) as CPosition;
                    CTeam enemyTeamComp = World.GetComponentOfEntity(enemy, typeof(CTeam)) as CTeam;

                    if (Utils.AreColliding(bulletPos, enemyPos))
                    {
                        if (bulletTeamComp.Team != enemyTeamComp.Team)
                        {
                            bulletDam = World.GetComponentOfEntity(Entities[i], typeof(CDamage)) as CDamage;
                            enemyHealth = World.GetComponentOfEntity(enemy, typeof(CHealth)) as CHealth;

                            enemyHealth.Damage += bulletDam.Damage; //Inflict bullet damage

                            deadBullets.Add(Entities[i]);
                            break;
                        }
                    }
                }

                if (ReachedTarget(bulletProj, bulletPos))
                {
                    if (!deadBullets.Contains(Entities[i]))
                    {
                        deadBullets.Add(Entities[i]);
                    }
                }
            }
            RemoveDeadProjectiles(deadBullets);
        }
    }
}