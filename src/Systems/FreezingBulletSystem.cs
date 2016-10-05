using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class FreezingBulletSystem : System
    {

        public FreezingBulletSystem (World world) : base(new List<Type> {typeof(CProjectile), typeof(CPosition)}, new List<Type> {typeof(CDamage)}, world)
        {
        }

        public override void Process()
        {
            CPosition bulletPos;
            CPosition enemyPos;

            List<int> entsToFreeze = World.GetAllEntitiesWithTag(typeof(CAI));

            //For each Freezing Bullet
            for (int i = 0; i < Entities.Count; i++)
            {
                if (ReachedTarget(Entities[i]))
                {
                    bulletPos = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;

                    //For each Entity which can be frozen
                    foreach (int toFreeze in entsToFreeze)
                    {
                        enemyPos = World.GetComponentOfEntity(toFreeze, typeof(CPosition)) as CPosition;

                        if (CollisionSystem.AreColliding(bulletPos, enemyPos))
                        {
                            //Don't add multiple Freze components (entity may collide with 2 bullets)
                            if (!World.EntityHasComponent(toFreeze, typeof(CNotMoving)))
                            {
                                World.AddComponentToEntity(toFreeze, new CNotMoving(3000, World.GameTime));
                            }
                        }
                    }

                    //Entity has reached its target and exploded, so kill it
                    World.RemoveEntity(Entities[i]);
                }
            }
        }

        private bool ReachedTarget(int entID)
        {
            CPosition pos = World.GetComponentOfEntity(entID, typeof(CPosition)) as CPosition;
            CProjectile proj = World.GetComponentOfEntity(entID, typeof(CProjectile)) as CProjectile;

            return SwinGame.RectanglesIntersect(SwinGame.CreateRectangle(pos.X, pos.Y, pos.Width, pos.Height), proj.Target);
        }
    }
}