using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class PlayerRenderingSystem : System
    {
        public PlayerRenderingSystem (World world) : base(new List<Type> {typeof(CPlayer)}, new List<Type> {}, world) 
        {
        }

        private int TimeSinceUsed(uint usedAt)
        {
            return (int)(World.GameTime - usedAt);
        }

        private void RenderCooldowns(CPlayer playerComp)
        {
            int cooldownBarWidth = 250;
            int freezeBarWidth = (int)(cooldownBarWidth * ((float)TimeSinceUsed(playerComp.UsedLastFreezingBulletAt) / playerComp.FreezingBulletCooldown));
            int poisonBarWidth = (int)(cooldownBarWidth * ((float)TimeSinceUsed(playerComp.UsedLastPoisonZoneAt) / playerComp.PoisonZoneCooldown));

            SwinGame.DrawText("Freezing Bullet: ", Color.Black, SwinGame.FontNamed("GameFont"), 700, 10);
            SwinGame.FillRectangle(Color.DarkGray, 820, 5, cooldownBarWidth, 20);

            if (PlayerCooldownSystem.AbilityIsReady(World.GameTime, playerComp.UsedLastFreezingBulletAt, playerComp.FreezingBulletCooldown))
            {
                SwinGame.FillRectangle(Color.DarkBlue, 820, 5, cooldownBarWidth, 20);
                SwinGame.DrawText("READY", Color.White, SwinGame.FontNamed("GameFont"), 910, 10);
            }
            else
            {
                SwinGame.FillRectangle(Color.DarkBlue, 820, 5, freezeBarWidth, 20);
            }

            SwinGame.DrawText("Poison Zone: ", Color.Black, SwinGame.FontNamed("GameFont"), 700, 50);
            SwinGame.FillRectangle(Color.DarkGray, 820, 45, cooldownBarWidth, 20);

            if (PlayerCooldownSystem.AbilityIsReady(World.GameTime, playerComp.UsedLastPoisonZoneAt, playerComp.PoisonZoneCooldown))
            {
                SwinGame.FillRectangle(Color.Purple, 820, 45, cooldownBarWidth, 20);
                SwinGame.DrawText("READY", Color.White, SwinGame.FontNamed("GameFont"), 910, 50);
            }
            else
            {
                SwinGame.FillRectangle(Color.Purple, 820, 45, poisonBarWidth, 20);
            }
        }

        private void RenderShop(CPlayer playerComp)
        {
            SwinGame.DrawText("Gold: " + playerComp.Gold, Color.Black, SwinGame.FontNamed("GameFont"), 120, 35);
            SwinGame.DrawText("Archers: " + playerComp.ArcherCount, Color.Black, SwinGame.FontNamed("GameFont"), 220, 35);
            SwinGame.DrawText("Wizards: " + playerComp.WizardCount, Color.Black, SwinGame.FontNamed("GameFont"), 220, 60);

            SwinGame.DrawText("Buy Wizard (W)", Color.Black, SwinGame.FontNamed("GameFont"), 400, 10);
            SwinGame.DrawText("20 Gold", Color.Black, SwinGame.FontNamed("SmallGameFont"), 430, 30);

            SwinGame.DrawText("Buy Archer (A)", Color.Black, SwinGame.FontNamed("GameFont"), 530, 10);
            SwinGame.DrawText("10 Gold", Color.Black, SwinGame.FontNamed("SmallGameFont"), 560, 30);
        }

        private void RenderHealthBar(CHealth entHealth)
        {
            int healthBarWidth = 200;
            int damageBarWidth = (int)(healthBarWidth * ((float)entHealth.Damage / entHealth.Health));

            SwinGame.FillRectangle(Color.DarkGreen, 150, 5, healthBarWidth, 20); //Draw health bar
            SwinGame.FillRectangle(Color.Red, 150, 5, damageBarWidth, 20); //Draw damage bar
            SwinGame.DrawText("HP", Color.Black, SwinGame.FontNamed("GameFont"), 120, 10);
        }

        public override void Process()
        {
            CRenderable playerRend = World.GetComponentOfEntity(Entities[0], typeof(CRenderable)) as CRenderable;
            CPosition playerPos = World.GetComponentOfEntity(Entities[0], typeof(CPosition)) as CPosition;
            CHealth playerHealth = World.GetComponentOfEntity(Entities[0], typeof(CHealth)) as CHealth;
            CPlayer playerComp = World.GetComponentOfEntity(Entities[0], typeof(CPlayer)) as CPlayer;

            SwinGame.DrawBitmap(playerRend.Img, playerPos.X, playerPos.Y);

            RenderHealthBar(playerHealth);
            RenderCooldowns(playerComp);
            RenderShop(playerComp);
        }
    }
}