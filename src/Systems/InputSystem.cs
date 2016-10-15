using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the System which handles all user input. This includes spell casts and purchase options.
    /// The System fetches the Player Gold System to process shop purchases.
    /// </summary>
    public class InputSystem : System
    {
        public InputSystem (World world) : base(new List<Type> {typeof(CPlayer)}, new List<Type> {}, world)
        {
        }

        public override void Process()
        {
            /// <summary>
            /// Mouse Position is evaluated once per frame and passed to relevant methods.
            /// </summary>
            Point2D pt = SwinGame.MousePosition();

            CPosition playerPos = World.GetComponentOfEntity(Entities[0], typeof(CPosition)) as CPosition;
            CPlayer player = World.GetComponentOfEntity(Entities[0], typeof(CPlayer)) as CPlayer;
            PlayerGoldSystem shop = World.GetSystem(typeof(PlayerGoldSystem)) as PlayerGoldSystem;

            if (SwinGame.KeyTyped(KeyCode.WKey))
            {
                shop.BuyWizard(player);
            }

            if (SwinGame.KeyDown(KeyCode.AKey))
            {
                shop.BuyArcher(player);
            }

            if (SwinGame.MouseClicked(MouseButton.LeftButton))
            {
                if (PlayerCooldownSystem.AbilityIsReady(World.GameTime, player.UsedLastPoisonZoneAt, player.PoisonZoneCooldown))
                {
                    EntityFactory.CreatePoisonZone(pt.X, pt.Y);

                    /// <summary>
                    /// Used to determine if ability is ready.
                    /// </summary>
                    player.UsedLastPoisonZoneAt = World.GameTime;
                }
            }

            if (SwinGame.MouseClicked(MouseButton.RightButton))
            {
                if (PlayerCooldownSystem.AbilityIsReady(World.GameTime, player.UsedLastFreezingBulletAt, player.FreezingBulletCooldown))
                {
                    EntityFactory.CreateFreezingBullet(playerPos.Centre.X, playerPos.Centre.Y, pt.X, pt.Y, 7);

                    /// <summary>
                    /// Used to determine if ability is ready.
                    /// </summary>
                    player.UsedLastFreezingBulletAt = World.GameTime;
                }
            }
        }
    }
}