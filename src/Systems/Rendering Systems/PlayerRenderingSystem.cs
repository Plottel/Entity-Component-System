using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the System responsible for drawing everything related to the Player. This includes all UI elements
    /// such as Gold, Unit Counts, Ability Cooldowns etc.
    /// </summary>
    public class PlayerRenderingSystem : System
    {
        /// <summary>
        /// Represents the constant widths of cooldown and health bars.
        /// These are used for rendering purposes.
        /// </summary>
        private const int COOLDOWN_BAR_WIDTH = 250;
        private const int HEALTH_BAR_WIDTH = 200;

        /// <summary>
        /// Rendering Text to the screen is a slow operation. Text which does not change is first drawn to a Bitmap and
        /// then that Bitmap is drawn to the screen rather than drawing the text every frame.
        /// </summary>
        private Bitmap _readyText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "READY", Color.White, Color.Transparent);
        private Bitmap _freezingBulletText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "Freezing Bullet: ", Color.Black, Color.Transparent);
        private Bitmap _poisonZoneText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "Poison Zone: ", Color.Black, Color.Transparent);
        private Bitmap _goldText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "Gold: ", Color.Black, Color.Transparent);
        private Bitmap _archersText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "Archers: ", Color.Black, Color.Transparent);
        //private Bitmap _wizardsText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "Wizards: ", Color.Black, Color.Transparent);
        //private Bitmap _buyWizardText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "Buy Wizard (W)", Color.Black, Color.Transparent);
        private Bitmap _buyArcherText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "Buy Archer (A)", Color.Black, Color.Transparent);
        private Bitmap _tenGoldText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("SmallGameFont"), "10 Gold", Color.Black, Color.Transparent); 
        //private Bitmap _twentyGoldText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("SmallGameFont"), "20 Gold", Color.Black, Color.Transparent);
        private Bitmap _hpText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "HP", Color.Black, Color.Transparent);
        private Bitmap _buyExplosionManText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("GameFont"), "Buy Exploder (E)", Color.Black, Color.Transparent);
        private Bitmap _fiftyGoldText = SwinGame.DrawTextToBitmap(SwinGame.FontNamed("SmallGameFont"), "50 Gold", Color.Black, Color.Transparent);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.PlayerRenderingSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public PlayerRenderingSystem (World world) : base(new List<Type> {typeof(CPlayer)}, new List<Type> {}, world) 
        {           
        }

        /// <summary>
        /// Renders everything related to the Player, including UI elements.
        /// </summary>
        public override void Process()
        {
            CRenderable renderable = World.GetComponent<CRenderable>(PlayerSystem.PLAYER_ENTITY_ID);
            CHealth health = World.GetComponent<CHealth>(PlayerSystem.PLAYER_ENTITY_ID);
            CPlayer player = World.GetComponent<CPlayer>(PlayerSystem.PLAYER_ENTITY_ID);

            RenderCooldowns(player);
            RenderShop(player);
            RenderHealthBar(health);
        }

        /// <summary>
        /// Specifies the percentage of an Ability cooldown which has passed.
        /// </summary>
        /// <returns>A float between 0 and 1 indicate the percentage completion.</returns>
        /// <param name="usedAt">When the ability was last used.</param>
        /// <param name="cooldown">The cooldown of the ability.</param>
        private float CooldownPercentage(uint usedAt, int cooldown)
        {
            return (float)(World.GameTime - usedAt) / cooldown;
        }

        /// <summary>
        /// Renders details for each cooldown the Player has. This includes progress bars and text.
        /// </summary>
        /// <param name="player">Player.</param>
        private void RenderCooldowns(CPlayer player)
        {
            #region FREEZING BULLET

            SwinGame.DrawBitmap(_freezingBulletText, 800, 10);
            SwinGame.FillRectangle(Color.DarkGray, 920, 5, COOLDOWN_BAR_WIDTH, 20);

            if (PlayerSystem.AbilityIsReady(player.TimeOfLastFreezingBullet, PlayerSystem.FREEZING_BULLET_COOLDOWN))
            {
                SwinGame.FillRectangle(Color.DarkBlue, 920, 5, COOLDOWN_BAR_WIDTH, 20);
                SwinGame.DrawBitmap(_readyText, 1010, 10);
            }
            else
            {
                float freezeBarWidth = COOLDOWN_BAR_WIDTH * CooldownPercentage(player.TimeOfLastFreezingBullet, PlayerSystem.FREEZING_BULLET_COOLDOWN);
                SwinGame.FillRectangle(Color.DarkBlue, 920, 5, freezeBarWidth, 20);
            }

            #endregion FREEZING BULLET

            #region POISON ZONE

            SwinGame.DrawBitmap(_poisonZoneText, 800, 50);
            SwinGame.FillRectangle(Color.DarkGray, 920, 45, COOLDOWN_BAR_WIDTH, 20);

            if (PlayerSystem.AbilityIsReady(player.TimeOfLastPoisonZone, PlayerSystem.POISON_ZONE_COOLDOWN))
            {
                SwinGame.FillRectangle(Color.Purple, 920, 45, COOLDOWN_BAR_WIDTH, 20);
                SwinGame.DrawBitmap(_readyText, 1010, 50);
            }
            else
            {
                float poisonBarWidth = COOLDOWN_BAR_WIDTH * CooldownPercentage(player.TimeOfLastPoisonZone, PlayerSystem.POISON_ZONE_COOLDOWN);
                SwinGame.FillRectangle(Color.Purple, 920, 45, poisonBarWidth, 20);
            }

            #endregion POISON ZONE
        }

        /// <summary>
        /// Renders all text related to the Player's purchase options.
        /// </summary>
        /// <param name="player">The Player.</param>
        private void RenderShop(CPlayer player)
        {
            SwinGame.DrawBitmap(_goldText, 120, 35);
            SwinGame.DrawText(player.Gold.ToString(), Color.Black, SwinGame.FontNamed("GameFont"), 165, 35);

            SwinGame.DrawBitmap(_archersText, 220, 35);
            SwinGame.DrawText(PlayerSystem.ARCHER_COUNT.ToString(), Color.Black, SwinGame.FontNamed("GameFont"), 290, 35);

            SwinGame.DrawBitmap(_buyArcherText, 505, 10);
            SwinGame.DrawBitmap(_tenGoldText, 535, 30);

            SwinGame.DrawBitmap(_buyExplosionManText, 635, 10);
            SwinGame.DrawBitmap(_fiftyGoldText, 665, 30);
        }

        /// <summary>
        /// Renders the Player's health bar. This is done separately to the HealthRenderingSystem
        /// as the Player's health bar is not rendered relative to its position.
        /// </summary>
        /// <param name="health">Health.</param>
        private void RenderHealthBar(CHealth health)
        {
            SwinGame.DrawBitmap(_hpText, 120, 10);
            SwinGame.FillRectangle(Color.DarkGreen, 150, 5, HEALTH_BAR_WIDTH, 20); //Draw health bar

            if (health.Damage > 0)
            {
                float damageBarWidth = (HEALTH_BAR_WIDTH * ((float)health.Damage / health.Health));
                SwinGame.FillRectangle(Color.Red, 150, 5, damageBarWidth, 20); //Draw damage bar
            }
        }
    }
}