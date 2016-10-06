using System;
using SwinGameSDK;

namespace MyGame
{
    public class GameMain
    {
        public static void Main()
        {
            //Open the game window
            SwinGame.OpenGraphicsWindow("GameMain", 800, 600);

            World world = new World();

            //Add System sets the world to this - don't need to specify world in System constructor
            world.AddSystem(new InputSystem(world));
            world.AddSystem(new SpawningSystem(world));
            world.AddSystem(new AISystem(world));
            world.AddSystem(new ProjectileSystem(world));
            world.AddSystem(new BulletSystem(world));
            world.AddSystem(new FreezingBulletSystem(world));
            world.AddSystem(new FrozenSystem(world));
            world.AddSystem(new ApplyPoisonSystem(world));
            world.AddSystem(new PoisonedSystem(world));
            world.AddSystem(new HealthSystem(world));
            world.AddSystem(new MovementSystem(world));
            world.AddSystem(new RenderingSystem(world));

            EntityFactory.CreatePlayer();

            //Run the game loop
            while(false == SwinGame.WindowCloseRequested() && SwinGame.KeyTyped(KeyCode.EscapeKey) == false)
            {
                //Fetch the next batch of UI interaction
                SwinGame.ProcessEvents();
                
                //Clear the screen and draw the framerate
                SwinGame.ClearScreen(Color.White);
                SwinGame.DrawFramerate(300, 10);
                world.Process();

                SwinGame.RefreshScreen(100);
            }
        }
    }
}