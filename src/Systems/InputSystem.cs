using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    /// <summary>
    /// Represents the System which handles all user input. This includes spell casts and purchase options.
    /// The System fetches the Player System to process shop purchases.
    /// </summary>
    public class InputSystem : System
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MyGame.InputSystem"/> class.
        /// </summary>
        /// <param name="world">The World the System belongs to.</param>
        public InputSystem (World world) : base(new List<Type> {typeof(CExcludeAll)}, new List<Type> {}, world)
        {
        }

        public override void Process()
        {
            /// <summary>
            /// Mouse Position is evaluated once per frame and passed to relevant methods.
            /// </summary>
            Point2D pt = SwinGame.MousePosition();

            PlayerSystem playerSystem = World.GetSystem<PlayerSystem>();

            if (SwinGame.KeyDown(KeyCode.AKey))            
                playerSystem.BuyArcher();
            

            if (SwinGame.KeyTyped(KeyCode.EKey))            
                playerSystem.BuyExplosionMan();
            

            if (SwinGame.MouseClicked(MouseButton.LeftButton))            
                playerSystem.CastPoisonZone(pt);
            

            if (SwinGame.MouseClicked(MouseButton.RightButton))            
                playerSystem.CastFreezingBullet(pt);
        }
    }
}