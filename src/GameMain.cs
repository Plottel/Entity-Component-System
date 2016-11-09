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

            #region MISC SYSTEMS
            world.AddSystem(new InputSystem(world)); 
            world.AddSystem(new PlayerSystem(world)); 
            world.AddSystem(new SpawningSystem(world)); 
            world.AddSystem(new MovementSystem(world)); 
            world.AddSystem(new LifetimeSystem(world)); 
            #endregion MISC SYSTEMS

            #region AI SYSTEMS
            world.AddSystem(new PlayerAISystem(world));
            world.AddSystem(new EnemyAISystem(world)); 
            #endregion AI SYSTEMS

            #region PROJECTILE SYSTEMS
            world.AddSystem(new ProjectileSystem(world)); 
            world.AddSystem(new FreezingBulletSystem(world)); 
            #endregion PROJECTILE SYSTEMS

            #region COLLISION SYSTEMS
            world.AddSystem(new CollisionCheckSystem(world)); 
            world.AddSystem(new FreezeZoneCollisionHandlerSystem(world)); 
            world.AddSystem(new PoisonZoneCollisionHandlerSystem(world)); 
            world.AddSystem(new DamageCollisionHandlerSystem(world)); 
            #endregion COLLISION SYSTEMS

            #region STATUS EFFECT SYSTEMS
            world.AddSystem(new FrozenSystem(world)); 
            world.AddSystem(new PoisonedSystem(world)); 
            world.AddSystem(new GotStatusEffectSystem(world)); 
            #endregion STATUS EFFECT SYSTEMS

            world.AddSystem(new DamageSystem(world));
            world.AddSystem(new ExplosionManSystem(world)); 

            #region RENDERING SYSTEMS
            world.AddSystem(new HealthRenderingSystem(world)); 
            world.AddSystem(new AnimationRenderingSystem(world)); 
            world.AddSystem(new RenderingSystem(world)); 
            world.AddSystem(new PlayerRenderingSystem(world)); 
            world.AddSystem(new StatusAnimationRenderingSystem(world)); 
            #endregion RENDERING SYSTEMS

            world.AddSystem(new CollisionCleanupSystem(world));

            EntityFactory.World = world;
            EntityFactory.CreatePlayer();

            //Run the game loop
            while(false == SwinGame.WindowCloseRequested() && SwinGame.KeyTyped(KeyCode.EscapeKey) == false)
            {
                //Fetch the next batch of UI interaction
                SwinGame.ProcessEvents();             
                
                //Clear the screen and draw the framerate
                SwinGame.ClearScreen(Color.SandyBrown);
                SwinGame.DrawFramerate(200, 570);
                world.Process();

                SwinGame.RefreshScreen(60);
            }
        }
    }
}