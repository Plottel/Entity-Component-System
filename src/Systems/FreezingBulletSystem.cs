using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class FreezingBulletSystem : System
    {
        private int _exclusionMask;

        public FreezingBulletSystem (World world) : base((int)ComponentType.Projectile | (int)ComponentType.Position, world)
        {
            _exclusionMask = (int)ComponentType.Damage;
        }

        public override bool EntityHasRequiredComponents(int entMask)
        {
            //Freezing Bullet System will NOT operate on Entities with a Damage component
            return (base.EntityHasRequiredComponents(entMask)) && ((entMask & _exclusionMask) != _exclusionMask);
        }

        public override void Process()
        {
            PositionComponent bulletPos;
            PositionComponent enemyPos;

            List<int> entsToFreeze = World.GetAllEntitiesWithTag(typeof(AIComponent));

            //For each Freezing Bullet
            for (int i = 0; i < Entities.Count; i++)
            {
                if (ReachedTarget(Entities[i]))
                {
                    bulletPos = World.GetComponentOfEntity(Entities[i], typeof(PositionComponent)) as PositionComponent;

                    //For each Entity which can be frozen
                    foreach (int toFreeze in entsToFreeze)
                    {
                        enemyPos = World.GetComponentOfEntity(toFreeze, typeof(PositionComponent)) as PositionComponent;

                        if (CollisionSystem.AreColliding(bulletPos, enemyPos))
                        {
                            //Don't add multiple Freze components (entity may collide with 2 bullets)
                            if (!World.EntityHasComponent(toFreeze, typeof(NotMovingComponent)))
                            {
                                World.AddComponentToEntity(toFreeze, new NotMovingComponent(3000, World.GameTime));
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
            PositionComponent pos = World.GetComponentOfEntity(entID, typeof(PositionComponent)) as PositionComponent;
            ProjectileComponent proj = World.GetComponentOfEntity(entID, typeof(ProjectileComponent)) as ProjectileComponent;

            return SwinGame.RectanglesIntersect(SwinGame.CreateRectangle(pos.X, pos.Y, pos.Width, pos.Height), proj.Target);
        }
    }
}