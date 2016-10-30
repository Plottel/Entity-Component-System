using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class PlayerRenderingSystem : System
    {
        private Bitmap _readyText;
        private Bitmap _freezingBulletText;
        private Bitmap _poisonZoneText;
        private Bitmap _goldText;
        private Bitmap _archersText;
        private Bitmap _wizardsText;
        private Bitmap _buyWizardText;
        private Bitmap _buyArcherText;
        private Bitmap _tenGoldText;
        private Bitmap _twentyGoldText;
        private Bitmap _hpText;

        public PlayerRenderingSystem (World world) : base(new List<Type> {typeof(CPlayer)}, new List<Type> {}, world) 
        {
            _readyText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "READY", Color.White, Color.Transparent);
            _freezingBulletText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "Freezing Bullet: ", Color.Black, Color.Transparent);
            _poisonZoneText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "Poison Zone: ", Color.Black, Color.Transparent);
            _goldText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "Gold: ", Color.Black, Color.Transparent);
            _archersText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "Archers: ", Color.Black, Color.Transparent);
            _wizardsText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "Wizards: ", Color.Black, Color.Transparent);
            _buyWizardText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "Buy Wizard (W)", Color.Black, Color.Transparent);
            _buyArcherText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "Buy Archer (A)", Color.Black, Color.Transparent);
            _tenGoldText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("SmallGameFont"), "10 Gold", Color.Black, Color.Transparent);
            _twentyGoldText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("SmallGameFont"), "20 Gold", Color.Black, Color.Transparent);
            _hpText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "HP", Color.Black, Color.Transparent);
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

            SwinGame.DrawBitmap(_freezingBulletText, 700, 10);
            SwinGame.FillRectangle(Color.DarkGray, 820, 5, cooldownBarWidth, 20);

            if (PlayerCooldownSystem.AbilityIsReady(World.GameTime, playerComp.UsedLastFreezingBulletAt, playerComp.FreezingBulletCooldown))
            {
                SwinGame.FillRectangle(Color.DarkBlue, 820, 5, cooldownBarWidth, 20);
                SwinGame.DrawBitmap(_readyText, 910, 10);
            }
            else
            {
                SwinGame.FillRectangle(Color.DarkBlue, 820, 5, freezeBarWidth, 20);
            }

            SwinGame.DrawBitmap(_poisonZoneText, 700, 50);
            SwinGame.FillRectangle(Color.DarkGray, 820, 45, cooldownBarWidth, 20);

            if (PlayerCooldownSystem.AbilityIsReady(World.GameTime, playerComp.UsedLastPoisonZoneAt, playerComp.PoisonZoneCooldown))
            {
                SwinGame.FillRectangle(Color.Purple, 820, 45, cooldownBarWidth, 20);
                SwinGame.DrawBitmap(_readyText, 910, 50);
            }
            else
            {
                SwinGame.FillRectangle(Color.Purple, 820, 45, poisonBarWidth, 20);
            }
        }

        private void RenderShop(CPlayer playerComp)
        {
            SwinGame.DrawBitmap(_goldText, 120, 35);
            SwinGame.DrawText(playerComp.Gold.ToString(), Color.Black, SwinGame.FontNamed("GameFont"), 165, 35);

            SwinGame.DrawBitmap(_archersText, 220, 35);
            SwinGame.DrawText(playerComp.ArcherCount.ToString(), Color.Black, SwinGame.FontNamed("GameFont"), 290, 35);

            SwinGame.DrawBitmap(_wizardsText, 220, 60);
            SwinGame.DrawText(playerComp.WizardCount.ToString(), Color.Black, SwinGame.FontNamed("GameFont"), 290, 60);

            SwinGame.DrawBitmap(_buyWizardText, 400, 10);
            SwinGame.DrawBitmap(_twentyGoldText, 430, 30);

            SwinGame.DrawBitmap(_buyArcherText, 530, 10);
            SwinGame.DrawBitmap(_tenGoldText, 560, 30);
        }

        private void RenderHealthBar(CHealth entHealth)
        {
            int healthBarWidth = 200;
            int damageBarWidth = (int)(healthBarWidth * ((float)entHealth.Damage / entHealth.Health));

            SwinGame.FillRectangle(Color.DarkGreen, 150, 5, healthBarWidth, 20); //Draw health bar
            SwinGame.FillRectangle(Color.Red, 150, 5, damageBarWidth, 20); //Draw damage bar
            SwinGame.DrawBitmap(_hpText, 120, 10);
        }

        public override void Process()
        {
            CRenderable playerRend = World.GetComponent<CRenderable>(Entities[0]);
            CPosition playerPos = World.GetComponent<CPosition>(Entities[0]);
            CHealth playerHealth = World.GetComponent<CHealth>(Entities[0]);
            CPlayer playerComp = World.GetComponent<CPlayer>(Entities[0]);

            SwinGame.DrawBitmap(playerRend.Img, playerPos.X, playerPos.Y);

            RenderHealthBar(playerHealth);
            RenderCooldowns(playerComp);
            RenderShop(playerComp);
        }
    }
}