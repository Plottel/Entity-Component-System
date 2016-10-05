using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class BulletSystem : System
    {
        public BulletSystem (World world) : base(new List<Type> {typeof(CProjectile), typeof(CPosition), typeof(CDamage)}, new List<Type> {}, world)
        {
        }

        public override void Process()
        {
            int playerID = World.GetAllEntitiesWithTag(typeof(CPlayer))[0];
            CPosition playerPos = World.GetComponentOfEntity(playerID, typeof(CPosition)) as CPosition;
            CHealth playerHealth = World.GetComponentOfEntity(playerID, typeof(CHealth)) as CHealth;
            Rectangle playerRect = SwinGame.CreateRectangle(playerPos.X, playerPos.Y, playerPos.Width, playerPos.Height);

            CPosition projPos;
            CDamage projDam;

            for (int i = 0; i < Entities.Count; i++)
            {
                projPos = World.GetComponentOfEntity(Entities[i], typeof(CPosition)) as CPosition;

                if (CollisionSystem.AreColliding(playerPos, projPos))
                {
                    projDam = World.GetComponentOfEntity(Entities[i], typeof(CDamage)) as CDamage;
                    playerHealth.Damage += projDam.Damage;

                    World.RemoveEntity(Entities[i]);
                }
            }
        }
    }
}