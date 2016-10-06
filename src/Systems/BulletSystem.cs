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
                CTeam bulletTeamComp = World.GetComponentOfEntity(Entities[i], typeof(CTeam)) as CTeam;
                Team bulletTeam = bulletTeamComp.Team;

                List<int> enemies = World.GetAllEntitiesWithTag(typeof(CAI)); //Get new list for each bullet - entities can be removed

                //For each potential collision target
                foreach (int e in enemies)
                {
                    enemyPos = World.GetComponentOfEntity(e, typeof(CPosition)) as CPosition;
                    CTeam enemyTeamComp = World.GetComponentOfEntity(e, typeof(CTeam)) as CTeam;
                    Team enemyTeam = enemyTeamComp.Team;

                    if (Utils.AreColliding(bulletPos, enemyPos))
                    {
                        if (bulletTeam != enemyTeam)
                        {
                            bulletDam = World.GetComponentOfEntity(Entities[i], typeof(CDamage)) as CDamage;
                            enemyHealth = World.GetComponentOfEntity(e, typeof(CHealth)) as CHealth;

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