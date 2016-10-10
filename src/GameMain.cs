using System;
using SwinGameSDK;

namespace MyGame
{
    public class GameMain
    {
        public static void Main()
        {
            //Open the game window
            SwinGame.OpenGraphicsWindow("GameMain", 1200, 600);
            SwinGame.LoadResourceBundleNamed("GameResources", "GameResources.txt", false);

            World world = new World();

            world.AddSystem(new InputSystem(world));
            world.AddSystem(new PlayerGoldSystem(world));
            world.AddSystem(new PlayerCooldownSystem(world));
            world.AddSystem(new SpawningSystem(world));
            world.AddSystem(new MovementSystem(world));           
            world.AddSystem(new EnemyAISystem(world));
            world.AddSystem(new ProjectileSystem(world));
            world.AddSystem(new FreezingBulletSystem(world));
            world.AddSystem(new FrozenSystem(world));
            world.AddSystem(new PoisonedSystem(world));
            world.AddSystem(new PlayerLootSystem(world));
            world.AddSystem(new CollisionCheckSystem(world));
            world.AddSystem(new BulletCollisionHandlerSystem(world));
            world.AddSystem(new FreezeZoneCollisionHandlerSystem(world));
            world.AddSystem(new PoisonZoneCollisionHandlerSystem(world));
            world.AddSystem(new HealthSystem(world));
            world.AddSystem(new HealthRenderingSystem(world));
            world.AddSystem(new AnimationRenderingSystem(world));
            world.AddSystem(new RenderingSystem(world));
            world.AddSystem(new PlayerRenderingSystem(world));
            world.AddSystem(new CollisionCleanupSystem(world));


            EntityFactory.CreatePlayer();

            //Run the game loop
            while(false == SwinGame.WindowCloseRequested() && SwinGame.KeyTyped(KeyCode.EscapeKey) == false)
            {
                //Fetch the next batch of UI interaction
                SwinGame.ProcessEvents();
                
                //Clear the screen and draw the framerate
                SwinGame.ClearScreen(Color.SkyBlue);
                SwinGame.DrawFramerate(200, 570);
                world.Process();

                SwinGame.RefreshScreen(60);
            }
        }
    }
}