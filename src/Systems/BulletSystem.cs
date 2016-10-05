using System;
using SwinGameSDK;

namespace MyGame
{
    public class BulletSystem : System
    {
        public BulletSystem (World world) : base((int)ComponentType.Projectile | (int)ComponentType.Position | (int)ComponentType.Damage, world)
        {
        }

        public override void Process()
        {
            int playerID = World.GetAllEntitiesWithTag(typeof(PlayerComponent))[0];
            PositionComponent playerPos = World.GetComponentOfEntity(playerID, typeof(PositionComponent)) as PositionComponent;
            HealthComponent playerHealth = World.GetComponentOfEntity(playerID, typeof(HealthComponent)) as HealthComponent;
            Rectangle playerRect = SwinGame.CreateRectangle(playerPos.X, playerPos.Y, playerPos.Width, playerPos.Height);


            PositionComponent projPos;
            DamageComponent projDam;

            for (int i = 0; i < Entities.Count; i++)
            {
                projPos = World.GetComponentOfEntity(Entities[i], typeof(PositionComponent)) as PositionComponent;

                if (CollisionSystem.AreColliding(playerPos, projPos))
                {
                    projDam = World.GetComponentOfEntity(Entities[i], typeof(DamageComponent)) as DamageComponent;
                    playerHealth.Damage += projDam.Damage;

                    World.RemoveEntity(Entities[i]);
                }
            }
        }
    }
}