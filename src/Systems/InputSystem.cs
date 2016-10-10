using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class InputSystem : System
    {
        public InputSystem (World world) : base(new List<Type> {typeof(CPlayer)}, new List<Type> {}, world)
        {
            
        }

        public override void Process()
        {
            Point2D pt = SwinGame.MousePosition();
            CPosition playerPos = World.GetComponentOfEntity(Entities[0], typeof(CPosition)) as CPosition;
            CPlayer player = World.GetComponentOfEntity(Entities[0], typeof(CPlayer)) as CPlayer;
            PlayerGoldSystem shop = World.GetSystem(typeof(PlayerGoldSystem)) as PlayerGoldSystem;

            if (SwinGame.KeyTyped(KeyCode.WKey))
            {
                shop.BuyWizard(player);
            }

            if (SwinGame.MouseClicked(MouseButton.LeftButton))
            {
                if (PlayerCooldownSystem.AbilityIsReady(World.GameTime, player.UsedLastPoisonZoneAt, player.PoisonZoneCooldown))
                {
                    EntityFactory.CreatePoisonPool(pt.X, pt.Y);
                    player.UsedLastPoisonZoneAt = World.GameTime;
                }
            }

            if (SwinGame.MouseClicked(MouseButton.RightButton))
            {
                if (PlayerCooldownSystem.AbilityIsReady(World.GameTime, player.UsedLastFreezingBulletAt, player.FreezingBulletCooldown))
                {
                    EntityFactory.CreateFreezingBullet(playerPos.Centre.X, playerPos.Centre.Y, pt.X, pt.Y, 7);
                    player.UsedLastFreezingBulletAt = World.GameTime;
                }
            }
        }
    }
}