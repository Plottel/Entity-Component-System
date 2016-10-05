using System;
using SwinGameSDK;

namespace MyGame
{
    public class CollisionSystem : System
    {
        public CollisionSystem (World world) : base ((int)ComponentType.None, world)
        {
        }


        public override void Process()
        {
            
        }

        public static bool AreColliding(PositionComponent p1, PositionComponent p2)
        {
            Rectangle r1 = SwinGame.CreateRectangle(p1.X, p1.Y, p1.Width, p1.Height);
            Rectangle r2 = SwinGame.CreateRectangle(p2.X, p2.Y, p2.Width, p2.Height);

            return SwinGame.RectanglesIntersect(r1, r2);
        }
    }
}