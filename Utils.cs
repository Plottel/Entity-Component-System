using System;
using SwinGameSDK;

namespace MyGame
{
    public static class Utils
    {
        public static bool EffectHasEnded(uint gameTime, uint timeApplied, int duration)
        {
            return (gameTime - timeApplied >= duration);
        }

        public static bool AreColliding(CPosition p1, CPosition p2)
        {
            Rectangle r1 = SwinGame.CreateRectangle(p1.X, p1.Y, p1.Width, p1.Height);
            Rectangle r2 = SwinGame.CreateRectangle(p2.X, p2.Y, p2.Width, p2.Height);

            return SwinGame.RectanglesIntersect(r1, r2);
        }
    }
}