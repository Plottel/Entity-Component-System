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

        private void RenderPurchaseOptions(CPlayer playerComp)
        {
        }

        private void RenderHealthBar(CHealth entHealth, CPosition entPos)
        {
            int healthBarWidth = entPos.Width - 4;
            int damageBarWidth = (int)(healthBarWidth * ((float)entHealth.Damage / entHealth.Health));

            SwinGame.FillRectangle(Color.DarkGreen, entPos.X + 2, entPos.Y + 50, healthBarWidth, 20); //Draw health bar
            SwinGame.FillRectangle(Color.Red, entPos.X + 2, entPos.Y + 50, damageBarWidth, 20); //Draw damage bar

            //SwinGame.DrawText("Health Bar Width: " + healthBarWidth, Color.Black, 200, 240);
            //SwinGame.DrawText("Damage Bar Width: " + damageBarWidth, Color.Black, 200, 260);
            //SwinGame.DrawText("Damage: " + entHealth.Damage, Color.Black, 200, 200);
            //SwinGame.DrawText("Max Health: " + entHealth.Health, Color.Black, 200, 220);
        }

        public override void Process()
        {
            CRenderable playerRend = World.GetComponentOfEntity(Entities[0], typeof(CRenderable)) as CRenderable;
            CPosition playerPos = World.GetComponentOfEntity(Entities[0], typeof(CPosition)) as CPosition;
            CHealth playerHealth = World.GetComponentOfEntity(Entities[0], typeof(CHealth)) as CHealth;
            CPlayer playerComp = World.GetComponentOfEntity(Entities[0], typeof(CPlayer)) as CPlayer;

            SwinGame.DrawBitmap(playerRend.Img, playerPos.X, playerPos.Y);

            RenderHealthBar(playerHealth, playerPos);
            RenderPurchaseOptions(playerComp);
            SwinGame.DrawText("Gold : " + playerComp.Gold, Color.Black, playerPos.X, playerPos.Y + 20);
        }
    }
}